using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
//____________________________
using System.Collections.ObjectModel;
using System.Data;
using UiDVDCatalogo;

namespace CatalogoDVD 
{
    class DaoDvdMysql : IDao
    {
        public MySqlConnection conexion;

        public bool Conectar(string srv, string db, string user, string pwd)
        {
            string cadenaConexion = "server=" + srv + ";" + "database=" + db + ";" + "uid=" + user + ";" + "pwd=" + pwd + ";";
            try
            {
                conexion = new MySqlConnection(cadenaConexion);
                conexion.Open();
                return true;
            }
            catch (MySqlException e)
            {
                switch (e.ErrorCode)
                {
                    case 0:
                        throw new Exception("Error de conexion");
                    case 1045:
                        throw new Exception("Usuario o contraseña incorrectos");
                    default:
                        throw new Exception(e.Message);
                }
            }
            
        }

        public bool Desconectar()
        {
            try
            {
                conexion.Close();
                return true;
            }
            catch (MySqlException)
            {
                throw;
            } 
        }

        public bool Conectado()
        {
            if (conexion != null)
            {
                return conexion.State == System.Data.ConnectionState.Open;
                // Posbile depuracion devuelve si la conexion es igual a una conexion abierta
            }
            else
            {
                return false;
            }
        }

        public DataTable SeleccionarTB(string codigo)
        {
            DataTable dt = new DataTable();
            string orden;

            if (codigo == null)
            {
                orden = "select codigo, titulo, artista, pais, compania, precio, anio from dvd";
            }
            else 
            {
                orden="select codigo, titulo, artista, pais, compania, precio, anio from dvd where codigo = '" + codigo + "'";
            }
            MySqlCommand cmd = new MySqlCommand(orden, conexion);
            MySqlDataAdapter da = new MySqlDataAdapter(cmd);
            da.Fill(dt);
            return dt;
         

        
        }
        public ObservableCollection<Dvd> Seleccionar(string codigo)
        {
            ObservableCollection<Dvd> resultado = new ObservableCollection<Dvd>();
   
            string orden;
            if (codigo == null)
            {
                orden = "select codigo, titulo, artista, pais, compania, precio, anio from dvd";
            }else
            { orden = "select codigo, titulo, artista, pais, compania, precio, anio from dvd where codigo = '" + codigo + "'"; }

            MySqlCommand cmd = new MySqlCommand(orden, conexion);

            try
            {
                MySqlDataReader lector = cmd.ExecuteReader();

       

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
            catch (MySqlException)
            {
                throw new Exception("No tiene permisos para ejecutar esta orden");
            }

        }

        public ObservableCollection<Dvd> SeleccionarPA(string codigo)
        {
            ObservableCollection<Dvd> resultado = new ObservableCollection<Dvd>();
            int resul;

            MySqlCommand cmd = new MySqlCommand("selectDVD", conexion);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            // Armado de parametros del procedure selectDVD
            cmd.Parameters.AddWithValue("@codi", codigo);
            cmd.Parameters.AddWithValue("@titu", null);
            cmd.Parameters.AddWithValue("@arti", null);
            cmd.Parameters.AddWithValue("@elPais", null);
            cmd.Parameters.AddWithValue("@comp", null);
            cmd.Parameters.AddWithValue("@prec", null);
            cmd.Parameters.AddWithValue("@elAnio", null);
            cmd.Parameters.AddWithValue("@resul", MySqlDbType.Int16);
            cmd.Parameters["@resul"].Direction = System.Data.ParameterDirection.Output;

            try
            {
                MySqlDataReader lector = cmd.ExecuteReader();
            
                resul = (int)cmd.Parameters["@resul"].Value;

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
            catch (MySqlException)
            {
            throw new Exception("No tiene permisos para ejecutar esta orden");
            }
        }

        public void Insertar()
        {
        }

        public void Actualizar()
        {
        }
        /// <summary>
        /// Borrado de filas de la tabla DVD
        /// </summary>
        /// <param name="codigo">Clave de fila a eliminar</param>
        /// <returns>n= numero de filas aceptadas, n=-1 parametro pasado null, no valido</returns>
        public int Borrar(string codigo)
        {
            string orden;
            if (codigo != null)
            {
                orden = "delete from dvd where codigo = '" + codigo + "'";
                MySqlCommand cmd = new MySqlCommand(orden, conexion);
                return cmd.ExecuteNonQuery();
            }
            return -1;
        }
        public void BorrarPA()
        {
        }
    }
}
