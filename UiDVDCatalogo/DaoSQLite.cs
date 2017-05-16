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
            string cadenaConexion = "Data Source=" + db + ";Version=3;" + "FailfMissing=true;";
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
                    if (lector["precio"] != null)
                   undvd.Precio=double.Parse(lector["precio"].ToString());
                    if(lector["anio"] != null)
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


        public Pais SeleccionarPais(string iso2)
        {
            Pais resultado = new Pais();

            string orden;
            if (iso2 != null)
            {
                orden = "select nombre from pais where iso2 ='" + iso2 + "'";



                SQLiteCommand cmd = new SQLiteCommand(orden, conexion);

                try
                {

                    object salida = cmd.ExecuteScalar();

                    if (salida != null)
                    {
                        resultado.Iso2 = iso2;
                        resultado.Nombre = salida.ToString();
                    }


                }
                catch (SQLiteException)
                {
                    throw new Exception("No tiene permisos para ejecutar esta orden");
                }
            }

            return resultado;
        }
        public void Insertar()
        {
            throw new NotImplementedException();
        }

        public int Actualizar(Dvd unDVD)
        {
            string orden;
            if (unDVD != null)
            { 
                orden = "update dvd set titulo = '"+unDVD.Titulo +"',artista = '"+ unDVD.Artista+"', precio= '"+unDVD.Precio+"', compania = '"+unDVD.Compania+"', anio= '"+unDVD.Anio+"+' where codigo = "+unDVD.Codigo;
                SQLiteCommand cmd = new SQLiteCommand(orden, conexion);
                return cmd.ExecuteNonQuery();
            }
            return -1;
        }

        public int Borrar(Dvd unDvd)
        {
            string orden;
            if (unDvd != null)
            {
                orden = "delete from dvd where codigo = '" + unDvd.Codigo + "'";
                SQLiteCommand cmd = new SQLiteCommand(orden, conexion);
                return cmd.ExecuteNonQuery();
            }
            return -1;
        }
    }
}
