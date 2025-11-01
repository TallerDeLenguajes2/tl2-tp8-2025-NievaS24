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

}