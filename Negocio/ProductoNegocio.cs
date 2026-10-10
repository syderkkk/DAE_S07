using Datos;
using Entidades;

namespace Negocio;

public class ProductoNegocio
{
    private readonly ProductoDatos _datos = new();

    public List<Producto> Buscar(string filtro) => _datos.Listar(filtro.Trim());

    public void Guardar(Producto p)
    {
        Validar(p);
        if (p.IdProducto == 0) _datos.Insertar(p);
        else _datos.Actualizar(p);
    }

    public void Eliminar(int id)
    {
        if (id <= 0) throw new ArgumentException("Seleccione un producto.");
        _datos.Eliminar(id);
    }

    private static void Validar(Producto p)
    {
        if (string.IsNullOrWhiteSpace(p.Nombre)) throw new ArgumentException("El nombre es obligatorio.");
        if (p.Precio < 0) throw new ArgumentException("El precio no puede ser negativo.");
        if (p.Stock < 0) throw new ArgumentException("El stock no puede ser negativo.");
    }
}
