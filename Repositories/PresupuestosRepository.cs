using tl2_tp7_2025_NievaS24.Models;
using Microsoft.Data.Sqlite;

namespace tl2_tp7_2025_NievaS24.Repository;

public class PresupuestosRepository
{
    private readonly string cadenaConexion = "Data Source=Tienda.db";
    public void Create(Presupuestos presupuesto)
    {
        using var con = new SqliteConnection(cadenaConexion);
        con.Open();
        string sql = "INSERT INTO Presupuestos (NombreDestinatario, FechaCreacion) VALUES (@NombreDestinatario, @FechaCreacion); SELECT last_insert_rowid();";
        using var cmd = new SqliteCommand(sql, con);
        cmd.Parameters.Add(new SqliteParameter("@NombreDestinatario", presupuesto.NombreDestinatario));
        cmd.Parameters.Add(new SqliteParameter("@FechaCreacion", presupuesto.FechaCreacion));
        presupuesto.idPresupuesto = Convert.ToInt32(cmd.ExecuteScalar());
        con.Close();
    }

    public List<Presupuestos> GetAll()
    {
        List<Presupuestos> presupuestos = [];
        using var con = new SqliteConnection(cadenaConexion);
        con.Open();
        string sql = "SELECT * FROM Presupuestos";
        using var cmd = new SqliteCommand(sql, con);
        using SqliteDataReader reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            var presupuesto = new Presupuestos
            {
                idPresupuesto = Convert.ToInt32(reader["idPresupuesto"]),
                NombreDestinatario = reader["NombreDestinatario"].ToString(),
                FechaCreacion = DateOnly.Parse(reader["FechaCreacion"].ToString()),
                detalles = []
            };
            presupuestos.Add(presupuesto);
        }
        string sql2 = "SELECT * FROM PresupuestosDetalle LEFT JOIN Productos using (idProducto)";
        using var cmd2 = new SqliteCommand(sql2, con);
        using SqliteDataReader reader2 = cmd2.ExecuteReader();
        while (reader2.Read())
        {
            var detalle = new PresupuestosDetalle
            {
                Producto = new Productos
                {
                    idProducto = Convert.ToInt32(reader2["idProducto"]),
                    Descripcion = reader2["Descripcion"].ToString(),
                    Precio = Convert.ToInt32(reader2["Precio"])
                },
                Cantidad = Convert.ToInt32(reader2["Cantidad"])
            };
            presupuestos.FirstOrDefault(p => p.idPresupuesto == Convert.ToInt32(reader2["idPresupuesto"])).detalles.Add(detalle);
        }
        con.Close();
        return presupuestos;
    }

    public Presupuestos GetById(int id)
    {
        using var con = new SqliteConnection(cadenaConexion);
        con.Open();
        string sql = "SELECT * FROM Presupuestos WHERE idPresupuesto = @idPresupuesto";
        using var cmd = new SqliteCommand(sql, con);
        cmd.Parameters.Add(new SqliteParameter("@idPresupuesto", id));
        using SqliteDataReader reader = cmd.ExecuteReader();
        if (!reader.Read()) throw new KeyNotFoundException($"El presupuesto {id} no existe");
        var presupuesto = new Presupuestos
        {
            idPresupuesto = Convert.ToInt32(reader["idPresupuesto"]),
            NombreDestinatario = reader["NombreDestinatario"].ToString(),
            FechaCreacion = DateOnly.Parse(reader["FechaCreacion"].ToString()),
            detalles = []
        };
        string sql2 = "SELECT * FROM PresupuestosDetalle LEFT JOIN Productos using (idProducto) WHERE idPresupuesto = @idPresupuesto";
        using var cmd2 = new SqliteCommand(sql2, con);
        cmd2.Parameters.Add(new SqliteParameter("@idPresupuesto", id));
        using SqliteDataReader reader2 = cmd2.ExecuteReader();
        while (reader2.Read())
        {
            var detalle = new PresupuestosDetalle
            {
                Producto = new Productos
                {
                    idProducto = Convert.ToInt32(reader2["idProducto"]),
                    Descripcion = reader2["Descripcion"].ToString(),
                    Precio = Convert.ToInt32(reader2["Precio"])
                },
                Cantidad = Convert.ToInt32(reader2["Cantidad"])
            };
            presupuesto.detalles.Add(detalle);
        }
        con.Close();
        return presupuesto;
    }

    public void Delete(int id)
    {
        using var con = new SqliteConnection(cadenaConexion);
        con.Open();
        string sql = "DELETE FROM Presupuestos WHERE idPresupuesto = @idPresupuesto";
        using var cmd = new SqliteCommand(sql, con);
        cmd.Parameters.Add(new SqliteParameter("@idPresupuesto", id));
        int modificado = cmd.ExecuteNonQuery();
        if (modificado <= 0) throw new KeyNotFoundException($"El presupuesto {id} no existe");
        con.Close();
    }

    public void CreateProd(int idPresupuesto, int idProducto, int cant)
    {
        using var con = new SqliteConnection(cadenaConexion);
        con.Open();
        string sql = "SELECT idPresupuesto FROM Presupuestos WHERE idPresupuesto = @idPresupuesto"; 
        using var cmd = new SqliteCommand(sql, con);
        cmd.Parameters.Add(new SqliteParameter("@idPresupuesto", idPresupuesto));
        using SqliteDataReader reader = cmd.ExecuteReader();
        if (!reader.Read()) throw new KeyNotFoundException($"El presupuesto {idPresupuesto} no existe");
        string sql2 = "SELECT idProducto FROM Productos WHERE idProducto = @idProducto"; 
        using var cmd2 = new SqliteCommand(sql2, con);
        cmd2.Parameters.Add(new SqliteParameter("@idProducto", idProducto));
        using SqliteDataReader reader2 = cmd2.ExecuteReader();
        if (!reader2.Read()) throw new KeyNotFoundException($"El producto {idProducto} no existe");
        string sql3 = "INSERT INTO PresupuestosDetalle VALUES (@idPresupuesto, @idProducto, @Cantidad);";
        using var cmd3 = new SqliteCommand(sql3, con);
        cmd3.Parameters.Add(new SqliteParameter("@idPresupuesto", idPresupuesto));
        cmd3.Parameters.Add(new SqliteParameter("@idProducto", idProducto));
        cmd3.Parameters.Add(new SqliteParameter("@Cantidad", cant));
        cmd3.ExecuteNonQuery();
        con.Close();
    }
}