using Microsoft.AspNetCore.Mvc;
using tl2_tp8_2025_NievaS24.Interface;
using tl2_tp8_2025_NievaS24.Models;
using tl2_tp8_2025_NievaS24.Repository;
using tl2_tp8_2025_NievaS24.ViewModels;

namespace tl2_tp8_2025_NievaS24.Controllers;

public class ProductosController : Controller
{
    private readonly IProductoRepository productoRepository;
    private readonly IAuthenticationService servicio;
    public ProductosController(IProductoRepository productoRepository, IAuthenticationService servicio)
    {
        this.productoRepository = productoRepository;
        this.servicio = servicio;
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
        return View();
    }


    [HttpPost]
    public IActionResult Create(ProductoViewModel productoVM)
    {
        if (!ModelState.IsValid)
        {
            return View(productoVM);
        }
        var nuevoProducto = new Productos
        {
            Descripcion = productoVM.Descripcion,
            Precio = productoVM.Precio
        };
        productoRepository.Create(nuevoProducto);
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult Update(int id)
    {
        var producto = productoRepository.GetById(id);
        var productoAEditar = new ProductoViewModel
        {
            idProducto = producto.idProducto,
            Descripcion = producto.Descripcion,
            Precio = producto.Precio
        };
        return View(productoAEditar);
    }


    [HttpPost]
    public IActionResult Update(ProductoViewModel productoVM)
    {
        if (!ModelState.IsValid)
        {
            return View(productoVM);
        }
        var productoEditado = new Productos
        {
            idProducto = productoVM.idProducto,
            Descripcion = productoVM.Descripcion,
            Precio = productoVM.Precio
        };
        productoRepository.Update(productoEditado.idProducto, productoEditado);
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