using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Entidades;
using Negocio;

namespace Presentacion;

public partial class MainWindow : Window
{
    private readonly ProductoNegocio _negocio = new();
    private int _idSeleccionado;

    public MainWindow()
    {
        InitializeComponent();
        Cargar();
    }

    private void Cargar() => Ejecutar(() => grid.ItemsSource = _negocio.Buscar(txtBuscar.Text));

    private void Buscar_Click(object sender, RoutedEventArgs e) => Cargar();

    private void TxtBuscar_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter) Cargar();
    }

    private void Nuevo_Click(object sender, RoutedEventArgs e) => LimpiarFormulario();

    private void Guardar_Click(object sender, RoutedEventArgs e)
    {
        if (!decimal.TryParse(txtPrecio.Text, out var precio) || !short.TryParse(txtStock.Text, out var stock))
        {
            MessageBox.Show("Precio y stock deben ser numéricos.");
            return;
        }
        var producto = new Producto
        {
            IdProducto = _idSeleccionado,
            Nombre = txtNombre.Text,
            CantidadPorUnidad = txtCantidad.Text,
            Precio = precio,
            Stock = stock
        };
        Ejecutar(() =>
        {
            _negocio.Guardar(producto);
            LimpiarFormulario();
            Cargar();
        });
    }

    private void Eliminar_Click(object sender, RoutedEventArgs e)
    {
        if (MessageBox.Show("¿Eliminar el producto?", "Confirmar", MessageBoxButton.YesNo) != MessageBoxResult.Yes) return;
        Ejecutar(() =>
        {
            _negocio.Eliminar(_idSeleccionado);
            LimpiarFormulario();
            Cargar();
        });
    }

    private void Grid_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (grid.SelectedItem is not Producto p) return;
        _idSeleccionado = p.IdProducto;
        txtNombre.Text = p.Nombre;
        txtCantidad.Text = p.CantidadPorUnidad;
        txtPrecio.Text = p.Precio.ToString();
        txtStock.Text = p.Stock.ToString();
    }

    private void LimpiarFormulario()
    {
        _idSeleccionado = 0;
        grid.SelectedItem = null;
        txtNombre.Clear();
        txtCantidad.Clear();
        txtPrecio.Clear();
        txtStock.Clear();
    }

    private static void Ejecutar(Action accion)
    {
        try { accion(); }
        catch (Exception ex) { MessageBox.Show(ex.Message, "Error"); }
    }
}
