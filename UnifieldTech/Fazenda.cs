
using System.ComponentModel.DataAnnotations;

namespace UnifieldTech;

public class Fazenda
{
    public int FazendaID { get; set; }

    [Required(ErrorMessage = "Esse campo é obrigatorio")]
    public string NomeFazenda { get; set; }

    [Required(ErrorMessage = "Esse campo é obrigatorio")]
    public string Hectar { get; set; }

    [Required(ErrorMessage = "Esse campo é obrigatorio")]
    public string Cultivar { get; set; }// tipo de planta

    [Required(ErrorMessage = "Esse campo é obrigatorio")]
    public string Rua { get; set; }

    [Required(ErrorMessage = "Esse campo é obrigatorio")]
    public string Num { get; set; }

    [Required(ErrorMessage = "Esse campo é obrigatorio")]
    public string Cidade { get; set; }//número da rua

    [Required(ErrorMessage = "Esse campo é obrigatorio")]
    public string Estado { get; set; }
    public string Latitude { get; set; }
    public string Longitude { get; set; }

    [Required(ErrorMessage = "Esse campo é obrigatorio")]
    public bool TipoPlantio { get; set; }

    [Required(ErrorMessage = "Esse campo é obrigatorio")]
    public bool AreaMecanizada { get; set; }
    //Referencia de:
    public int ClienteID { get; set; }
    public Cliente cliente { get; set; }
}
