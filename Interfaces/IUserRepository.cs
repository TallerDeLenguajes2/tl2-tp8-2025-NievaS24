using tl2_tp8_2025_NievaS24.Models;

namespace tl2_tp8_2025_NievaS24.Interface;

public interface IUserRepository
{
    Usuario GetUser(string username, string password);
}