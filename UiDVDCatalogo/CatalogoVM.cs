using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//----------------
using CatalogoDVD;
using System.Collections.ObjectModel;


namespace UiDVDCatalogo
{
    class CatalogoVM : INotifyPropertyChanged
    {
        #region Campos
        DaoDvdMysql _dao;
        bool _tipoConexion = true; //MySql - true , Sqlite -false

       

        ObservableCollection<Dvd> _listado;


        string _mensaje;

        

        static string host = "localhost";
        static string db = "catalogo";
        static string usr = "usr_catalogo";
        static string pwd = "123456789";

        #endregion
        #region Propiedades
        public bool TipoConexion
        {
            get { return _tipoConexion; }
            set
            {
                if (_tipoConexion != value)
                    _tipoConexion = value;
                NotificarCambioDePropiedad("TipoConexion");
                    }
        }

        public string Mensaje
        {
            get { return _mensaje; }
            set { _mensaje = value; }
        }
        public ObservableCollection<Dvd> Listado
        {
            get { return _listado; }
            set { _listado = value; }
        }
        public bool Conectado
        {
            get 
            {
                if (_dao == null)
                   return false;
                else
                {
                    return _dao.Conectado();
                }
            }
        }

        public string ColorConectar
        {
            get 
            
            {
                if (Conectado)
                    return "green";
                else
                    return "red";
            }
        }

        #endregion
        #region Eventos
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotificarCambioDePropiedad(string propiedad)
        {
            PropertyChangedEventHandler manejador = PropertyChanged;
            if (manejador != null)
            { 
                manejador(this,new PropertyChangedEventArgs(propiedad));
            }
        }
        #endregion
        #region Commands
        private void ConectarBD()
        {
            _dao = null;
            if (TipoConexion) //MySQl
            {

            }
            else //SQLite
            { }
        }
        #endregion
    }
}
