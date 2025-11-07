using Microsoft.AspNetCore.Mvc;
using tl2_tp8_2025_NievaS24.Models;
using tl2_tp8_2025_NievaS24.Repository;
using tl2_tp8_2025_NievaS24.ViewModels;

namespace tl2_tp8_2025_NievaS24.Controllers;

public class PresupuestosController : Controller
{
    private PresupuestosRepository presupuestosRepository;
    public PresupuestosController()
    {
        presupuestosRepository = new PresupuestosRepository();
    }

    [HttpGet]
    public IActionResult Index()
    {
        List<Presupuestos> presupuestos = presupuestosRepository.GetAll();
        return View(presupuestos);
    }

    public IActionResult Details(int id)
    {
        try
        {
            Presupuestos presupuesto = presupuestosRepository.GetById(id);
            return View(presupuesto);
        }
        catch (KeyNotFoundException)
        {
            return View();
        }

    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }


    [HttpPost]
    public IActionResult Create(PresupuestoViewModel presupuestoVM)
    {
        if (!ModelState.IsValid)
        {
            return View(presupuestoVM);
        }
        var nuevoPresupuesto = new Presupuestos
        {
            NombreDestinatario = presupuestoVM.NombreDestinatario,
            FechaCreacion = presupuestoVM.FechaCreacion
        };
        presupuestosRepository.Create(nuevoPresupuesto);
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult Update(int id)
    {
        var presupuesto = presupuestosRepository.GetById(id);
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
        presupuestosRepository.Update(presupuestoEditado.idPresupuesto, presupuestoEditado);
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult Delete(int id)
    {
        Presupuestos preABorrar = presupuestosRepository.GetById(id);
        return View(preABorrar);
    }

    [HttpPost]
    public IActionResult DeleteConfirmed(int idPresupuesto)
    {
        presupuestosRepository.Delete(idPresupuesto);
        return RedirectToAction("Index");
    }
}