namespace UnifieldTech;

public class Celular
{
    public int CelularID { get; set; }
    public string CelularN { get; set; }

    //referencia de:
    public int ClienteID { get; set; }
    public Cliente? cliente { get; set; }
}
