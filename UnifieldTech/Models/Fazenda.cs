
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace UnifieldTech.Models;

public class Fazenda
{
    public int FazendaID { get; set; }

    [Required(ErrorMessage = "Esse campo é obrigatorio")]
    public string? NomeFazenda { get; set; }

    [Required(ErrorMessage = "Esse campo é obrigatorio")]
    public string? Hectar { get; set; }

    [Required(ErrorMessage = "Esse campo é obrigatorio")]
    public string? Cultivar { get; set; }// tipo de planta


    public string? Rua { get; set; }


    public string? Num { get; set; }//número da rua

    public string? Cidade { get; set; }

    public string? Estado { get; set; }
    public string? Latitude { get; set; }
    public string? Longitude { get; set; }

    [Required(ErrorMessage = "Esse campo é obrigatorio")]
    public string? TipoPlantio { get; set; }

    [Required(ErrorMessage = "Esse campo é obrigatorio")]
    public bool AreaMecanizada { get; set; }
    //Referencia de:
    public int ClienteID { get; set; }
	[JsonIgnore]
	public Cliente? cliente { get; set; }
}
