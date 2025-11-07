using System.ComponentModel.DataAnnotations;

namespace tl2_tp8_2025_NievaS24.ViewModels;

public class PresupuestoViewModel
{
    public int idPresupuesto { get; set; }
    [Display(Name = "Email del Destinatario")]
    [Required(ErrorMessage = "El email es obligatorio.")]
    [EmailAddress(ErrorMessage = "El formato del email no es valido.")]
    public string NombreDestinatario { get; set; }
    [Display(Name = "Fecha de Creación")]
    [Required(ErrorMessage = "La fecha es obligatoria.")]
    [DataType(DataType.Date)]
    public DateOnly FechaCreacion { get; set; }
}