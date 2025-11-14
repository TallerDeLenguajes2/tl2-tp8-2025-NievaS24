using tl2_tp8_2025_NievaS24.Models;

namespace tl2_tp8_2025_NievaS24.Interface;

public interface IPresupuestoRepository
{
    public void Create(Presupuestos presupuesto);
    public List<Presupuestos> GetAll();
    public Presupuestos GetById(int id);
    public void Delete(int id);
    public void CreateProd(int idPresupuesto, int idProducto, int cant);
    public void Update(int id, Presupuestos presupuesto);
}