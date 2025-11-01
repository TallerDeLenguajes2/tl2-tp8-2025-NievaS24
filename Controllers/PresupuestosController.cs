using Microsoft.AspNetCore.Mvc;
using tl2_tp7_2025_NievaS24.Models;
using tl2_tp7_2025_NievaS24.Repository;
using tl2_tp8_2025_NievaS24.Models;

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
        List<Presupuestos> Presupuestos = presupuestosRepository.GetAll();
        return View(Presupuestos);
    }
}