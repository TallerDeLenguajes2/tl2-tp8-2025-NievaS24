using Microsoft.AspNetCore.Mvc;
using tl2_tp8_2025_NievaS24.Models;
using tl2_tp8_2025_NievaS24.Repository;

namespace tl2_tp8_2025_NievaS24.Controllers;

public class ProductosController : Controller
{
    private ProductoRepository productoRepository;
    public ProductosController()
    {
        productoRepository = new ProductoRepository();
    }

    [HttpGet]
    public IActionResult Index()
    {
        List<Productos> productos = productoRepository.GetAll();
        return View(productos);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View(new Productos());
    }


    [HttpPost]
    public IActionResult Create(Productos producto)
    {
        productoRepository.Create(producto);
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult Update(int id)
    {
        return View(productoRepository.GetById(id));
    }


    [HttpPost]
    public IActionResult Update(Productos producto)
    {
        productoRepository.Update(producto.idProducto, producto);
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult Delete(int id)
    {
        Productos prodABorrar = productoRepository.GetById(id);
        return View(prodABorrar);
    }

    [HttpPost]
    public IActionResult DeleteConfirmed(int idProducto)
    {
        productoRepository.Delete(idProducto);
        return RedirectToAction("Index");
    }
}