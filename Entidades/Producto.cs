namespace Entidades;

public class Producto
{
    public int IdProducto { get; set; }
    public string Nombre { get; set; } = "";
    public string CantidadPorUnidad { get; set; } = "";
    public decimal Precio { get; set; }
    public short Stock { get; set; }
}
