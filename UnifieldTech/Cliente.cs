namespace UnifieldTech;

public class Cliente
{
    public int ClienteID { get; set; }
    public string NomeCliente { get; set; }
    public string CPF { get; set; }
    public string? E_Mail { get; set; }
    public DateTime DataNacs { get; set; }
    public string Password { get; set; }

    //Referencia para:
    public ICollection<Celular>? celular { get; set; }
    public ICollection<Fazenda>? fazenda { get; set; }
}
