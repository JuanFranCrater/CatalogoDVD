using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//----------------
using CatalogoDVD;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace UiDVDCatalogo
{
    class CatalogoVM : INotifyPropertyChanged
    {
        #region Campos
        IDao _dao;
        bool _tipoConexion = true; //MySql - true , Sqlite -false

        ObservableCollection<Dvd> _listado;
        Dvd _dvd;
        string _mensaje;
        string _nombrePais = "No data";

        public string NombrePais
        {
            get { return _nombrePais; }
            set {
                if (_nombrePais != value)
                {
                    _nombrePais = value;
                    NotificarCambioDePropiedad("NombrePais");
                }
            }
        }

        static string host = "localhost";
        static string db = "catalogo";
        static string usr = "usr_catalogo";
        static string pwd = "123456789";

        #endregion
        #region Propiedades
        public Dvd DvdSeleccionado
        {
            get { return _dvd; }
            set {

                if (_dvd != value)
                {

                    _dvd = value;
                    if (_dao.Conectado() && _dvd != null)
                        NombrePais = _dao.SeleccionarPais(_dvd.Pais).Nombre;
                    else
                        NombrePais = "sin identificar";
                    NotificarCambioDePropiedad("DvdSeleccionadoNoNulo");
                }
            }
        }

        public bool DvdSeleccionadoNoNulo
        {
            get { return DvdSeleccionado != null; }
        }

        public bool TipoConexion
        {
            get { return _tipoConexion; }
            set
            {
                if (_tipoConexion != value)
                {
                    _tipoConexion = value;
                    NotificarCambioDePropiedad("TipoConexion");
                }
            }
        }

        public string Mensaje
        {
            get { return _mensaje; }
            set
            {
                if (_mensaje != value)
                {
                    _mensaje = value;
                    NotificarCambioDePropiedad("Mensaje");
                }
            }
        }
        public ObservableCollection<Dvd> Listado
        {
            get { return _listado; }
            set {
                if (_listado != value)
                {
                    _listado = value;
                    NotificarCambioDePropiedad("Listado");
                }
            }
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
            set
            {
                NotificarCambioDePropiedad("ColorConectar");
            
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
                manejador(this, new PropertyChangedEventArgs(propiedad));
            }
        }
        #endregion
        #region Commands
        private void LeerTodosDvD()
        {
            if (TipoConexion)
            {
                Listado = _dao.Seleccionar(null);
                
            }
            else
            {
                Listado = _dao.Seleccionar(null);
            }
            Mensaje = "Datos obtenidos";
        }

        private void ConectarBD()
        {
            try
            {
                _dao = null;
                if (TipoConexion) //MySQl
                {
                    _dao = new DaoDvdMysql();
                    _dao.Conectar(host, db, usr, pwd);
                    Mensaje = "Conectado a la BD MysSQL /MariaDB";
                   
                    NotificarCambioDePropiedad("ColorConectar");
                 
                }
                else //SQLite
                {
                    _dao = new DaoSQLite();
                    _dao.Conectar(null, "catalogo.db", null, null);
                    Mensaje = "Conectado a la BD SQLite";

                    NotificarCambioDePropiedad("ColorConectar");
                }
            }
            catch (Exception e)
            {
                Mensaje = e.Message; 
                NotificarCambioDePropiedad("Conectado");

            }
            NotificarCambioDePropiedad("Conectado");
        }
        private void DesconectarBD()
        {
            try
            {
                _dao.Desconectar();
                Mensaje = "Desconexion de la BD con éxito";
                NotificarCambioDePropiedad("ColorConectar");
                _listado = null;
                NotificarCambioDePropiedad("Titulo");
            }
            catch
            { }
            NotificarCambioDePropiedad("Conectado");
        }
        private void BorrarDVD()
        { 
            if(_dao.Borrar(DvdSeleccionado) == 1)
            {
                Mensaje = "Dvd eliminado correctamente";
            }
            else
            {
                Mensaje = "Error al intentar eliminar el DVD";
            }
        }
        private void ActualizarDVD()
        {
            if (DvdSeleccionado != null)
            {
                if (_dao.Actualizar(DvdSeleccionado) == 1)
                {
                    Mensaje = "Dvd actualizado correctamente";
                }
                else
                {
                    Mensaje = "Error al intentar actualizar el DVD";
                }
            }
        }
        public ICommand ConectarBD_Click
        {

            get { return new RelayCommand(o => ConectarBD(), o => true); }


        }
        public ICommand DesconectarBD_Click
        {

            get { return new RelayCommand(o => DesconectarBD(), o => true); }


        }
        public ICommand LeerTodosDVD_Click
        {

            get { return new RelayCommand(o => LeerTodosDvD(), o => true); }

        }
        public ICommand BorrarClick
        {

            get { return new RelayCommand(o => BorrarDVD(), o => true); }

        }
        public ICommand ActualizarClick
        {

            get { return new RelayCommand(o => ActualizarDVD(), o => true); }

        }
        #endregion
    }

}
