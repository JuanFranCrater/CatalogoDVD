using System.Collections.ObjectModel;
using System.Data;
using CatalogoDVD;

namespace UiDVDCatalogo
{
    interface IDao
    {
         bool Conectar(string srv, string db, string user, string pwd);
         bool Desconectar();
         bool Conectado();
         DataTable SeleccionarTB(string codigo);
         ObservableCollection<Dvd> Seleccionar(string codigo);
         void Insertar();
         void Actualizar();
         int Borrar(string codigo);
    }
}
