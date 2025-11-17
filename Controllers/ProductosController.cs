using Microsoft.AspNetCore.Mvc;
using tl2_tp8_2025_NievaS24.Interface;
using tl2_tp8_2025_NievaS24.Models;
using tl2_tp8_2025_NievaS24.Repository;
using tl2_tp8_2025_NievaS24.ViewModels;

namespace tl2_tp8_2025_NievaS24.Controllers;

public class ProductosController : Controller
{
    private readonly IProductoRepository _productoRepository;
    private readonly IAuthenticationService _authService;
    public ProductosController(IProductoRepository productoRepository, IAuthenticationService authService)
    {
        _productoRepository = productoRepository;
        _authService = authService;
    }

    private IActionResult CheckAdminPermissions()
    {
        // 1. No logueado? -> vuelve al login
        if (!_authService.IsAuthenticated())
        {
            return RedirectToAction("Index", "Login");
        }

        // 2. No es Administrador? -> Da Error
        if (!_authService.HasAccessLevel("Administrador"))
        {
            // Llamamos a AccesoDenegado (llama a la vista correspondiente de Productos)
            return RedirectToAction(nameof(AccesoDenegado));
        }
        return null; // Permiso concedido
    }

    [HttpGet]
    public IActionResult Index()
    {
        var securityCheck = CheckAdminPermissions();
        if (securityCheck != null) return securityCheck;
        List<Productos> productos = _productoRepository.GetAll();
        return View(productos);
    }

    [HttpGet]
    public IActionResult Create()
    {
        var securityCheck = CheckAdminPermissions();
        if (securityCheck != null) return securityCheck;
        return View();
    }


    [HttpPost]
    public IActionResult Create(ProductoViewModel productoVM)
    {
        var securityCheck = CheckAdminPermissions();
        if (securityCheck != null) return securityCheck;
        if (!ModelState.IsValid)
        {
            return View(productoVM);
        }
        var nuevoProducto = new Productos
        {
            Descripcion = productoVM.Descripcion,
            Precio = productoVM.Precio
        };
        _productoRepository.Create(nuevoProducto);
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult Update(int id)
    {
        var securityCheck = CheckAdminPermissions();
        if (securityCheck != null) return securityCheck;
        var producto = _productoRepository.GetById(id);
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
        var securityCheck = CheckAdminPermissions();
        if (securityCheck != null) return securityCheck;
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
        _productoRepository.Update(productoEditado.idProducto, productoEditado);
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult Delete(int id)
    {
        var securityCheck = CheckAdminPermissions();
        if (securityCheck != null) return securityCheck;
        Productos prodABorrar = _productoRepository.GetById(id);
        return View(prodABorrar);
    }

    [HttpPost]
    public IActionResult DeleteConfirmed(int idProducto)
    {
        var securityCheck = CheckAdminPermissions();
        if (securityCheck != null) return securityCheck;
        _productoRepository.Delete(idProducto);
        return RedirectToAction("Index");
    }

    public IActionResult AccesoDenegado()
    {
        return View();
    }
}