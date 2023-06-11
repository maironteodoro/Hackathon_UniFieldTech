using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace UnifieldTech.Models;

public class Celular
{
    public int CelularID { get; set; }

    [Required(ErrorMessage = "Esse campo é obrigatorio")]
    [RegularExpression(@"^\([1-9]{2}\) 9?[6-9][0-9]{3}\-[0-9]{4}$", ErrorMessage = "O campo CelularN deve estar no formato (99) 99999-9999")]
    public string? CelularN { get; set; }

    //referencia de:
    public int ClienteID { get; set; }
    [JsonIgnore]
    public Cliente? cliente { get; set; }
}
