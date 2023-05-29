using System.ComponentModel.DataAnnotations;

namespace UnifieldTech.Models;

public class Cliente
{
    public int ClienteID { get; set; }

    [Required(ErrorMessage = "Esse campo é obrigatorio")] //transforma o campo em obrigatorio
    [StringLength(100)] //limita o caracter em 100
    public string? NomeCliente { get; set; }

    [Required(ErrorMessage = "Esse campo é obrigatorio")]
    public string? CPF { get; set; }

    [Required(ErrorMessage = "Esse campo é obrigatorio")]
    public string? E_Mail { get; set; }

    [Required(ErrorMessage = "Esse campo é obrigatorio")]
    public DateTime DataNacs { get; set; }

    [Required(ErrorMessage = "Esse campo é obrigatorio")]
    public string? Password { get; set; }

    //Referencia para:
    public ICollection<Celular>? celular { get; set; }
    public ICollection<Fazenda>? fazenda { get; set; }
}
