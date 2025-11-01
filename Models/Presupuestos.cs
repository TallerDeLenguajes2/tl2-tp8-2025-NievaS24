namespace tl2_tp8_2025_NievaS24.Models;

public class Presupuestos
{
    public int idPresupuesto { get; set; }
    public string NombreDestinatario { get; set; }
    public DateOnly FechaCreacion { get; set; }
    public List<PresupuestosDetalle> detalles { get; set; }

    public double MontoPresupuesto()
    {
        double total = 0;
        foreach (var detalle in detalles)
        {
            total += detalle.Producto.Precio * detalle.Cantidad;
        }
        return total;
    }

    public double MontoPresupuestoConIva()
    {
        return MontoPresupuesto() * 1.21;
    }
}