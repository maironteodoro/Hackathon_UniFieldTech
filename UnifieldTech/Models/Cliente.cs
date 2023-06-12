using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using UnifieldTech.Models;

namespace UnifieldTech.Models;

public class Cliente : IValidatableObject
{
    public int ClienteID { get; set; }

    [Required(ErrorMessage = "Esse campo é obrigatorio")] //transforma o campo em obrigatorio
    [StringLength(100)] //limita o caracter em 100
    public string? NomeCliente { get; set; }

    [Required(ErrorMessage = "Esse campo é obrigatorio")]
    [RegularExpression(@"^\d{3}\.\d{3}\.\d{3}-\d{2}$", ErrorMessage = "O CPF deve estar no formato XXX.XXX.XXX-XX")]
    public string? CPF { get; set; }
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        List<ValidationResult> validationResults = new List<ValidationResult>();

        //Validar se o CPF está preenchido
        if (string.IsNullOrEmpty(CPF))
        {
            validationResults.Add(new ValidationResult("O CPF é obrigatório.", new[] { nameof(CPF) }));
        }
        else
        {
            //Validar se o CPF é válido utilizando o método ValidaCPF.validaCPF
            if (!ValidaCPF.validaCPF(CPF))
            {
                validationResults.Add(new ValidationResult("CPF inválido.", new[] { nameof(CPF) }));
            }
        }

        // Adicione mais validações personalizadas conforme necessário

        return validationResults;
    }

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
