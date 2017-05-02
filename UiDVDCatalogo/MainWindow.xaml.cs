using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
//---------------------------------
using System.Collections.ObjectModel;
using CatalogoDVD;


namespace UiDVDCatalogo
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ObservableCollection<Dvd> resultado = new ObservableCollection<Dvd>();
        DaoDvdMysql dao = new DaoDvdMysql();
        static string host = "localhost";
        static string db = "catalogo";
        static string usr = "usr_catalogo";
        static string pwd = "123456789";
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Conectar(object sender, RoutedEventArgs e)
        {
            if (!dao.Conectado())
            {
                if (dao.Conectar(host, db, usr, pwd))
                {
                    menuEstado.Header = "Conectado";
                    btnConectarDesconectar.Content = "Desconectar";
                    btnConectarDesconectar.Click += Desconectar;
                    btnListar.IsEnabled = true;
                }
            }
        }

        private void Desconectar(object sender, RoutedEventArgs e)
        {
            if (dao.Conectado())
            {
                if (dao.Desconectar())
                { menuEstado.Header = "Desconectado";
                btnConectarDesconectar.Content = "Conectar";
                btnConectarDesconectar.Click += Conectar;
                btnListar.IsEnabled = false;
                }
            }
        }

        private void ListarDVD(object sender, RoutedEventArgs e)
        {
            if (dao.Conectado())
            {
                dgtabla.Items.Clear();
                resultado = dao.Seleccionar(null);
                for (int i = 0; i < resultado.Count; i++)
                {
                    dgtabla.Items.Add(resultado[i]);
                }
                dgtabla.Visibility = Visibility.Visible;
            }
            
        }
        /// <summary>
        /// Cancela el intento de editar en app
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void IntentoEditar(object sender, DataGridBeginningEditEventArgs e)
        {
            e.Cancel = true;
        }
    }
}
