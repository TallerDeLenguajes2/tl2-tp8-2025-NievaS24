using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using tl2_tp8_2025_NievaS24.Interface;
using tl2_tp8_2025_NievaS24.Models;
using tl2_tp8_2025_NievaS24.ViewModels;

namespace tl2_tp8_2025_NievaS24.Controllers;

public class PresupuestosController : Controller
{
    private IPresupuestoRepository _presupuestosRepository;
    private IProductoRepository _productoRepository;
    private readonly IAuthenticationService _authService;
    public PresupuestosController(IPresupuestoRepository presupuestosRepository, IProductoRepository productoRepository, IAuthenticationService servicio)
    {
        _presupuestosRepository = presupuestosRepository;
        _productoRepository = productoRepository;
        _authService = servicio;
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
    private IActionResult CheckAllPermissions()
    {
        // 1. No logueado? -> vuelve al login
        if (!_authService.IsAuthenticated())
        {
            return RedirectToAction("Index", "Login");
        }

        // 2. No es Administrador ni cliente? -> Da Error
        if (!(_authService.HasAccessLevel("Administrador") || _authService.HasAccessLevel("Cliente")))
        {
            return RedirectToAction("Index", "Login");
        }
        return null; // Permiso concedido
    }

    [HttpGet]
    public IActionResult Index()
    {
        var securityCheck = CheckAllPermissions();
        if (securityCheck != null) return securityCheck;
        List<Presupuestos> presupuestos = _presupuestosRepository.GetAll();
        return View(presupuestos);
    }

    public IActionResult Details(int id)
    {
        var securityCheck = CheckAllPermissions();
        if (securityCheck != null) return securityCheck;
        try
        {
            Presupuestos presupuesto = _presupuestosRepository.GetById(id);
            return View(presupuesto);
        }
        catch (KeyNotFoundException)
        {
            return RedirectToAction("Index");
        }

    }

    [HttpGet]
    public IActionResult Create()
    {
        var securityCheck = CheckAdminPermissions();
        if (securityCheck != null) return securityCheck;
        return View();
    }


    [HttpPost]
    public IActionResult Create(PresupuestoViewModel presupuestoVM)
    {
        var securityCheck = CheckAdminPermissions();
        if (securityCheck != null) return securityCheck;
        if (!ModelState.IsValid)
        {
            return View(presupuestoVM);
        }
        var nuevoPresupuesto = new Presupuestos
        {
            NombreDestinatario = presupuestoVM.NombreDestinatario,
            FechaCreacion = presupuestoVM.FechaCreacion
        };
        _presupuestosRepository.Create(nuevoPresupuesto);
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult Update(int id)
    {
        var securityCheck = CheckAdminPermissions();
        if (securityCheck != null) return securityCheck;
        var presupuesto = _presupuestosRepository.GetById(id);
        var presupuestoAEditar = new PresupuestoViewModel
        {
            idPresupuesto = presupuesto.idPresupuesto,
            NombreDestinatario = presupuesto.NombreDestinatario,
            FechaCreacion = presupuesto.FechaCreacion
        };
        return View(presupuestoAEditar);
    }


    [HttpPost]
    public IActionResult Update(PresupuestoViewModel presupuestoVM)
    {
        var securityCheck = CheckAdminPermissions();
        if (securityCheck != null) return securityCheck;
        if (!ModelState.IsValid)
        {
            return View(presupuestoVM);
        }
        var presupuestoEditado = new Presupuestos
        {
            idPresupuesto = presupuestoVM.idPresupuesto,
            NombreDestinatario = presupuestoVM.NombreDestinatario,
            FechaCreacion = presupuestoVM.FechaCreacion
        };
        _presupuestosRepository.Update(presupuestoEditado.idPresupuesto, presupuestoEditado);
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult Delete(int id)
    {
        var securityCheck = CheckAdminPermissions();
        if (securityCheck != null) return securityCheck;
        Presupuestos preABorrar = _presupuestosRepository.GetById(id);
        return View(preABorrar);
    }

    [HttpPost]
    public IActionResult DeleteConfirmed(int idPresupuesto)
    {
        var securityCheck = CheckAdminPermissions();
        if (securityCheck != null) return securityCheck;
        _presupuestosRepository.Delete(idPresupuesto);
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult AgregarProducto(int id)
    {
        var securityCheck = CheckAdminPermissions();
        if (securityCheck != null) return securityCheck;
        List<Productos> productos = _productoRepository.GetAll();
        AgregarProductoViewModel model = new AgregarProductoViewModel
        {
            idPresupuesto = id,
            ListaProductos = new SelectList(productos, "idProducto", "Descripcion")
        };
        return View(model);
    }

    [HttpPost]
    public IActionResult AgregarProducto(AgregarProductoViewModel modelVM)
    {
        var securityCheck = CheckAdminPermissions();
        if (securityCheck != null) return securityCheck;
        if (!ModelState.IsValid)
        {
            var productos = _productoRepository.GetAll();
            modelVM.ListaProductos = new SelectList(productos, "idProducto", "Descripcion");
            return View(modelVM);
        }
        _presupuestosRepository.CreateProd(modelVM.idPresupuesto, modelVM.idProducto, modelVM.Cantidad);
        return RedirectToAction("Details", new { id = modelVM.idPresupuesto });
    }

    public IActionResult AccesoDenegado()
    {
        return View();
    }
}