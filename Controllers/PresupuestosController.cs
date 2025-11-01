using Microsoft.AspNetCore.Mvc;
using tl2_tp7_2025_NievaS24.Models;
using tl2_tp7_2025_NievaS24.Repository;

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
}