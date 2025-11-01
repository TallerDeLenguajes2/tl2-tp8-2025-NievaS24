using tl2_tp8_2025_NievaS24.Models;
using Microsoft.Data.Sqlite;

namespace tl2_tp8_2025_NievaS24.Repository;

public class ProductoRepository
{
    private readonly string cadenaConexion = "Data Source=Tienda.db";
    public void Create(Productos producto)
    {
        using var con = new SqliteConnection(cadenaConexion);
        con.Open();
        string sql = "INSERT INTO Productos (Descripcion, Precio) VALUES (@Descripcion, @Precio); SELECT last_insert_rowid();";
        using var cmd = new SqliteCommand(sql, con);
        cmd.Parameters.Add(new SqliteParameter("@Descripcion", producto.Descripcion));
        cmd.Parameters.Add(new SqliteParameter("@Precio", producto.Precio));
        producto.idProducto = Convert.ToInt32(cmd.ExecuteScalar());
        con.Close();
    }

    public void Update(int id, Productos producto)
    {

        using var con = new SqliteConnection(cadenaConexion);
        con.Open();
        string sql = "UPDATE Productos SET Descripcion = @Descripcion, Precio = @Precio WHERE idProducto = @idProducto";
        using var cmd = new SqliteCommand(sql, con);
        cmd.Parameters.Add(new SqliteParameter("@Descripcion", producto.Descripcion));
        cmd.Parameters.Add(new SqliteParameter("@Precio", producto.Precio));
        cmd.Parameters.Add(new SqliteParameter("@idProducto", id));
        int modificado = cmd.ExecuteNonQuery();
        if (modificado <= 0) throw new KeyNotFoundException($"El producto {id} no existe");
        producto.idProducto = id;
        con.Close();
    }

    public List<Productos> GetAll()
    {
        List<Productos> productos = [];
        using var con = new SqliteConnection(cadenaConexion);
        con.Open();
        string sql = "SELECT * FROM Productos";
        using var cmd = new SqliteCommand(sql, con);
        using SqliteDataReader reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            var producto = new Productos
            {
                idProducto = Convert.ToInt32(reader["idProducto"]),
                Descripcion = reader["Descripcion"].ToString(),
                Precio = Convert.ToInt32(reader["Precio"])
            };
            productos.Add(producto);
        }
        con.Close();
        return productos;
    }

    public Productos GetById(int id)
    {
        using var con = new SqliteConnection(cadenaConexion);
        con.Open();
        string sql = "SELECT * FROM Productos WHERE idProducto = @idProducto";
        using var cmd = new SqliteCommand(sql, con);
        cmd.Parameters.Add(new SqliteParameter("@idProducto", id));
        using SqliteDataReader reader = cmd.ExecuteReader();
        if (!reader.Read()) throw new KeyNotFoundException($"El producto {id} no existe");
        var producto = new Productos
        {
            idProducto = Convert.ToInt32(reader["idProducto"]),
            Descripcion = reader["Descripcion"].ToString(),
            Precio = Convert.ToInt32(reader["Precio"])
        };
        con.Close();
        return producto;
    }

    public void Delete(int id)
    {
        using var con = new SqliteConnection(cadenaConexion);
        con.Open();
        string sql = "DELETE FROM Productos WHERE idProducto = @idProducto";
        using var cmd = new SqliteCommand(sql, con);
        cmd.Parameters.Add(new SqliteParameter("@idProducto", id));
        int modificado = cmd.ExecuteNonQuery();
        if (modificado <= 0) throw new KeyNotFoundException($"El producto {id} no existe");
        con.Close();
    }
}
