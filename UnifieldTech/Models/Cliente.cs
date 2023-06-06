using System.ComponentModel.DataAnnotations;

namespace UnifieldTech.Models;

public class Cliente
{
    public int ClienteID { get; set; }

    [Required(ErrorMessage = "Esse campo é obrigatorio")] //transforma o campo em obrigatorio
    [StringLength(100)] //limita o caracter em 100
    public string? NomeCliente { get; set; }

    [Required(ErrorMessage = "Esse campo é obrigatorio")]
    [RegularExpression(@"^\d{3}\.\d{3}\.\d{3}-\d{2}$", ErrorMessage = "O CPF deve estar no formato XXX.XXX.XXX-XX")]
    public string? CPF { get; set; }

    [Required(ErrorMessage = "Esse campo é obrigatorio")]
    public string? E_Mail { get; set; }

    [Required(ErrorMessage = "Esse campo é obrigatorio")]
    public DateTime DataNacs { get; set; }

    [Required(ErrorMessage = "Esse campo é obrigatorio")]
    public string Password { get; set; }
    public string? Codigo { get; set; }

    //Referencia para:
    public ICollection<Celular>? celular { get; set; }
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
