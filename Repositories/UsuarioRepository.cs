using tl2_tp8_2025_NievaS24.Models;
using tl2_tp8_2025_NievaS24.Interface;
using Microsoft.Data.Sqlite;

namespace tl2_tp8_2025_NievaS24.Repository;

public class UsuarioRepository : IUserRepository
{
    private readonly string cadenaConexion = "Data Source=Tienda.db";
    public Usuario GetUser(string username, string password) 
    {
        Usuario user = null;
        //Consulta SQL que busca por Usuario Y Contrasena
        string sql = "SELECT Id, Nombre, User, Pass, Rol FROM Usuarios WHERE User = @Usuario AND Pass = @Contrasena";
        using var con = new SqliteConnection(cadenaConexion);
        con.Open();
        using var cmd = new SqliteCommand(sql, con);
        // Se usan parámetros para prevenir inyección SQL
        cmd.Parameters.Add(new SqliteParameter("@Usuario", username));
        cmd.Parameters.Add(new SqliteParameter("@Contrasena", password));
        using SqliteDataReader reader = cmd.ExecuteReader();
        if (reader.Read())
        {
        // Si el lector encuentra una fila, el usuario existe y las credenciales son correctas
            user = new Usuario
            {
                Id = reader.GetInt32(0),
                Nombre = reader.GetString(1),
                User = reader.GetString(2),
                Pass = reader.GetString(3),
                Rol = reader.GetString(4)
            };
        }
        con.Close();
        return user;
    }
}