using tl2_tp8_2025_NievaS24.Models;
using Microsoft.Data.Sqlite;

namespace tl2_tp8_2025_NievaS24.Repository;

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
        string sql = "SELECT * FROM Presupuestos;";
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
        // string sql = "SELECT * FROM Presupuestos INNER JOIN PresupuestosDetalle USING(idPresupuesto) INNER JOIN Productos USING(idProducto) ORDER BY idPresupuesto;";
        // using var cmd = new SqliteCommand(sql, con);
        // using SqliteDataReader reader = cmd.ExecuteReader();
        // while (reader.Read())
        // {
        //     int idPresupuestoActual = Convert.ToInt32(reader["idPresupuesto"]);
        //     Presupuestos presupuesto = null;
        //     foreach (var pre in presupuestos) //Controlo si ya existe el presupuesto
        //     {
        //         if (pre.idPresupuesto == idPresupuestoActual)
        //         {
        //             presupuesto = pre;
        //             break;
        //         }
        //     }

        //     if (presupuesto == null) //Si no existe lo creo
        //     {
        //         presupuesto = new Presupuestos
        //         {
        //             idPresupuesto = idPresupuestoActual,
        //             NombreDestinatario = reader["NombreDestinatario"].ToString(),
        //             FechaCreacion = DateOnly.Parse(reader["FechaCreacion"].ToString()),
        //             detalles = []
        //         };
        //         presupuestos.Add(presupuesto);
        //     }

        //     //Agrego los productos al presupusto que pretenece
        //     presupuesto.detalles.Add(new PresupuestosDetalle
        //     {
        //         Producto = new Productos
        //         {
        //             idProducto = Convert.ToInt32(reader["idProducto"]),
        //             Descripcion = reader["Descripcion"].ToString(),
        //             Precio = Convert.ToInt32(reader["Precio"])
        //         },
        //         Cantidad = Convert.ToInt32(reader["Cantidad"])
        //     });
        // }
        con.Close();
        return presupuestos;
    }

    public Presupuestos GetById(int id)
    {
        using var con = new SqliteConnection(cadenaConexion);
        con.Open();
        string sql = "SELECT * FROM Presupuestos LEFT JOIN PresupuestosDetalle USING(idPresupuesto) LEFT JOIN Productos USING(idProducto) WHERE idPresupuesto = @idPresupuesto; ";
        using var cmd = new SqliteCommand(sql, con);
        cmd.Parameters.Add(new SqliteParameter("@idPresupuesto", id));
        using SqliteDataReader reader = cmd.ExecuteReader();
        if (!reader.HasRows) throw new KeyNotFoundException($"El presupuesto {id} no existe");
        Presupuestos presupuesto = null;
        while (reader.Read())
        {
            if (presupuesto == null) //Lo creo por una unica vez
            {
                presupuesto = new Presupuestos
                {
                    idPresupuesto = Convert.ToInt32(reader["idPresupuesto"]),
                    NombreDestinatario = reader["NombreDestinatario"].ToString(),
                    FechaCreacion = DateOnly.Parse(reader["FechaCreacion"].ToString()),
                    detalles = []
                };
            }
            //Agrego los productos al presupusto
            if (reader["idProducto"] != DBNull.Value)
            {
                presupuesto.detalles.Add(new PresupuestosDetalle
                {
                    Producto = new Productos
                    {
                        idProducto = Convert.ToInt32(reader["idProducto"]),
                        Descripcion = reader["Descripcion"].ToString(),
                        Precio = Convert.ToInt32(reader["Precio"])
                    },
                    Cantidad = Convert.ToInt32(reader["Cantidad"])
                });
            }
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

    public void Update(int id, Presupuestos presupuesto)
    {

        using var con = new SqliteConnection(cadenaConexion);
        con.Open();
        string sql = "UPDATE Presupuestos SET NombreDestinatario = @NombreDestinatario, FechaCreacion = @FechaCreacion WHERE idPresupuesto = @idPresupuesto";
        using var cmd = new SqliteCommand(sql, con);
        cmd.Parameters.Add(new SqliteParameter("@NombreDestinatario", presupuesto.NombreDestinatario));
        cmd.Parameters.Add(new SqliteParameter("@FechaCreacion", presupuesto.FechaCreacion.ToString("yyyy-MM-dd")));
        cmd.Parameters.Add(new SqliteParameter("@idPresupuesto", id));
        int modificado = cmd.ExecuteNonQuery();
        if (modificado <= 0) throw new KeyNotFoundException($"El producto {id} no existe");
        presupuesto.idPresupuesto = id;
        con.Close();
    }
}