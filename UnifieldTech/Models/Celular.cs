using System.ComponentModel.DataAnnotations;

namespace UnifieldTech.Models;

public class Celular
{
    public int CelularID { get; set; }

    [Required(ErrorMessage = "Esse campo é obrigatorio")]
    public string? CelularN { get; set; }

    //referencia de:
    public int ClienteID { get; set; }
    public Cliente? cliente { get; set; }
}
