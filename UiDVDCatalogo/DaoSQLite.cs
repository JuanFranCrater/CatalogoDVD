using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Data;
using CatalogoDVD;
using System.Collections.ObjectModel;
namespace UiDVDCatalogo
{
    class DaoSQLite : IDao
    {
        private SQLiteConnection conexion;
        public bool Conectar(string srv, string db, string user, string pwd)
        {
            string cadenaConexion = "Data Source" + db + ";Version=3;" + "FailfMissing=true;";
            try
            {
                conexion = new SQLiteConnection(cadenaConexion);
                conexion.Open();
                return true;
            }
            catch (SQLiteException ex)
            {
            throw new Exception("Error de conexion: " + ex.Data);
            }
        }

        public bool Desconectar()
        {
            try
            {
                if (conexion != null)
                    conexion.Close();
                return true;
            }
            catch (SQLiteException)
            {
                
                throw;
            }
        }

        public bool Conectado()
        {
            if (conexion != null)
                return conexion.State == System.Data.ConnectionState.Open;
            else
                return false;
        }

        public System.Data.DataTable SeleccionarTB(string codigo)
        {
            DataTable dt = new DataTable();
            string sql;
            if (codigo == null)
                sql = "select codigo, titulo, artista, pais compania,precio anio from dvd";
            else
                sql = "select codigo, titulo, artista, pais compania,precio anio from dvd where codigo = "+codigo;
            SQLiteCommand cmd = new SQLiteCommand(sql, conexion);
            SQLiteDataAdapter da = new SQLiteDataAdapter(cmd);
            da.Fill(dt);

            return dt;
        }

        public System.Collections.ObjectModel.ObservableCollection<CatalogoDVD.Dvd> Seleccionar(string codigo)
        {
            ObservableCollection<Dvd> resultado = new ObservableCollection<Dvd>();

            string orden;
            if (codigo == null)
            {
                orden = "select codigo, titulo, artista, pais, compania, precio, anio from dvd";
            }
            else
            { orden = "select codigo, titulo, artista, pais, compania, precio, anio from dvd where codigo = '" + codigo + "'"; }

            SQLiteCommand cmd = new SQLiteCommand(orden, conexion);

            try
            {
                SQLiteDataReader lector = cmd.ExecuteReader();



                while (lector.Read())
                {
                    Dvd undvd = new Dvd();
                    undvd.Codigo = ushort.Parse(lector["codigo"].ToString());
                    undvd.Titulo = lector["titulo"].ToString();
                    undvd.Artista = lector["artista"].ToString();
                    undvd.Pais = lector["pais"].ToString();
                    undvd.Compania = lector["compania"].ToString();
                    undvd.Precio = double.Parse(lector["precio"].ToString());
                    undvd.Anio = lector["anio"].ToString();

                    resultado.Add(undvd);
                }

                lector.Close();
                return resultado;
            }
            catch (SQLiteException)
            {
                throw new Exception("No tiene permisos para ejecutar esta orden");
            }
        }

        public void Insertar()
        {
            throw new NotImplementedException();
        }

        public void Actualizar()
        {
            throw new NotImplementedException();
        }

        public int Borrar(string codigo)
        {
            throw new NotImplementedException();
        }
    }
}
