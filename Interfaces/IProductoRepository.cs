using tl2_tp8_2025_NievaS24.Models;

namespace tl2_tp8_2025_NievaS24.Interface;

public interface IProductoRepository
{
    public void Create(Productos producto);
    public void Update(int id, Productos producto);
    public List<Productos> GetAll();
    public Productos GetById(int id);
    public void Delete(int id);
}