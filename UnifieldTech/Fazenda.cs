namespace UnifieldTech;

public class Fazenda
{
    public int FazendaID { get; set; }
    public string NomeFazenda { get; set; }
    public string Hectar { get; set; }
    public string Cultivar { get; set; }// tipo de planta
    public string Rua { get; set; }


    public string Num { get; set; }
    public string Cidade { get; set; }//número da rua
    public string Estado { get; set; }

    public string Latitude { get; set; }
    public string Longitude { get; set; }
    public bool TipoPlantio { get; set; }
    public bool AreaMecanizada { get; set; }
    //Referencia de:
    public int ClienteID { get; set; }
    public Cliente cliente { get; set; }
}
