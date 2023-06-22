using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace UnifieldTech.Models;

public class Cliente
{
    public int ClienteID { get; set; }

    [Required(ErrorMessage = "Esse campo é obrigatorio")] //transforma o campo em obrigatorio
    [StringLength(100)] //limita o caracter em 100
    public string? NomeCliente { get; set; }

    [Required(ErrorMessage = "Esse campo é obrigatorio")]
    [RegularExpression(@"^\d{3}\.\d{3}\.\d{3}-\d{2}$", ErrorMessage = "O CPF deve estar no formato XXX.XXX.XXX-XX")]
    private string? cpf;

    [RegularExpression(@"^\d{3}\.\d{3}\.\d{3}-\d{2}$", ErrorMessage = "Insira um CPF válido.")]
    public string? CPF
    {
        get { return cpf; }
        set { cpf = value; }
    }

    public bool IsCPFValid()
    {
        if (string.IsNullOrEmpty(CPF))
            return true; // CPF é opcional, então não é validado se estiver vazio ou nulo

        return ValidateCPF(CPF);
    }

    public bool ValidateCPF(string cpf)
    {
        cpf = Regex.Replace(cpf, @"\D", ""); // Remove caracteres não numéricos do CPF

        if (cpf.Length != 11)
            return false; // CPF deve ter 11 dígitos

        int[] multiplicadores1 = { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
        int[] multiplicadores2 = { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

        string tempCpf = cpf.Substring(0, 9);
        int soma = 0;

        for (int i = 0; i < 9; i++)
        {
            soma += int.Parse(tempCpf[i].ToString()) * multiplicadores1[i];
        }

        int resto = soma % 11;
        if (resto < 2)
        {
            resto = 0;
        }
        else
        {
            resto = 11 - resto;
        }

        string digitoVerificador = resto.ToString();
        tempCpf += digitoVerificador;

        soma = 0;
        for (int i = 0; i < 10; i++)
        {
            soma += int.Parse(tempCpf[i].ToString()) * multiplicadores2[i];
        }

        resto = soma % 11;
        if (resto < 2)
        {
            resto = 0;
        }
        else
        {
            resto = 11 - resto;
        }

        digitoVerificador += resto.ToString();

        return cpf.EndsWith(digitoVerificador);
    }

    [Required(ErrorMessage = "Esse campo é obrigatorio")]
    [RegularExpression(@"^\([1-9]{2}\) 9?[6-9][0-9]{3}\-[0-9]{4}$", ErrorMessage = "O campo CelularN deve estar no formato (99) 99999-9999")]
    public string? CelularN { get; set; }

    [Required(ErrorMessage = "Esse campo é obrigatorio")]
    public string? E_Mail { get; set; }

    [Required(ErrorMessage = "Esse campo é obrigatorio")]
    public DateTime DataNacs { get; set; }

    [Required(ErrorMessage = "Esse campo é obrigatorio")]
    public string? Password { get; set; }

    public string? Codigo { get; set; }

	//Referencia para:
	[JsonIgnore]
	public ICollection<Fazenda>? fazenda { get; set; }

    public string GerarStringAleatoria()
    {
        Random random = new Random();
        const string caracteresPermitidos = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

        string randomString = new string(Enumerable.Repeat(caracteresPermitidos, 6)
                                      .Select(s => s[random.Next(s.Length)]).ToArray());

        return randomString;
    }
}
