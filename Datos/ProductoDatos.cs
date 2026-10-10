using Entidades;
using Microsoft.Data.SqlClient;

namespace Datos;

public class ProductoDatos
{
    private const string CadenaConexion =
        @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Neptuno;Integrated Security=True;TrustServerCertificate=True;";

    public List<Producto> Listar(string filtro)
    {
        const string sql = @"SELECT idproducto, nombreProducto, cantidadPorUnidad, precioUnidad, unidadesEnExistencia
                             FROM productos
                             WHERE suspendido = 0 AND nombreProducto LIKE @filtro
                             ORDER BY nombreProducto";
        var lista = new List<Producto>();
        using var cn = new SqlConnection(CadenaConexion);
        using var cmd = new SqlCommand(sql, cn);
        cmd.Parameters.AddWithValue("@filtro", "%" + filtro + "%");
        cn.Open();
        using var rd = cmd.ExecuteReader();
        while (rd.Read())
        {
            lista.Add(new Producto
            {
                IdProducto = rd.GetInt32(0),
                Nombre = rd.IsDBNull(1) ? "" : rd.GetString(1),
                CantidadPorUnidad = rd.IsDBNull(2) ? "" : rd.GetString(2),
                Precio = rd.IsDBNull(3) ? 0 : rd.GetDecimal(3),
                Stock = rd.IsDBNull(4) ? (short)0 : rd.GetInt16(4)
            });
        }
        return lista;
    }

    public void Insertar(Producto p)
    {
        const string sql = @"INSERT INTO productos (idproducto, nombreProducto, cantidadPorUnidad, precioUnidad, unidadesEnExistencia, suspendido)
                             SELECT ISNULL(MAX(idproducto), 0) + 1, @nombre, @cantidad, @precio, @stock, 0 FROM productos";
        Ejecutar(sql, p);
    }

    public void Actualizar(Producto p)
    {
        const string sql = @"UPDATE productos
                             SET nombreProducto = @nombre, cantidadPorUnidad = @cantidad,
                                 precioUnidad = @precio, unidadesEnExistencia = @stock
                             WHERE idproducto = @id";
        Ejecutar(sql, p);
    }

    public void Eliminar(int id)
    {
        const string sql = "UPDATE productos SET suspendido = 1 WHERE idproducto = @id";
        using var cn = new SqlConnection(CadenaConexion);
        using var cmd = new SqlCommand(sql, cn);
        cmd.Parameters.AddWithValue("@id", id);
        cn.Open();
        cmd.ExecuteNonQuery();
    }

    private static void Ejecutar(string sql, Producto p)
    {
        using var cn = new SqlConnection(CadenaConexion);
        using var cmd = new SqlCommand(sql, cn);
        cmd.Parameters.AddWithValue("@id", p.IdProducto);
        cmd.Parameters.AddWithValue("@nombre", p.Nombre);
        cmd.Parameters.AddWithValue("@cantidad", p.CantidadPorUnidad);
        cmd.Parameters.AddWithValue("@precio", p.Precio);
        cmd.Parameters.AddWithValue("@stock", p.Stock);
        cn.Open();
        cmd.ExecuteNonQuery();
    }
}
