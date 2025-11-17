using System.ComponentModel.DataAnnotations;
namespace tl2_tp8_2025_NievaS24.ViewModels;

public class LoginViewModel
{
    [Required(ErrorMessage = "El username es obligatorio.")]
    public string Username { get; set; }
    [Required(ErrorMessage = "La password es obligatoria.")]
    public string Password { get; set; }
    public string ErrorMessage { get; set; }
}