using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatalogoDVD
{
    class Dvd
    {
        private int _codigo;
        private string _titulo;
        private string _artista;
        private string _pais;
        private string _compania;
        private double _precio;
        private string _anio;

        public int Codigo
        {
            get { return _codigo; }
            set { _codigo = value; }
        }
        public string Titulo
        {
            get { return _titulo; }
            set { _titulo = value; }
        }
        public string Artista
        {
            get { return _artista; }
            set { _artista = value; }
        }
        public string Pais
        {
            get { return _pais; }
            set { _pais = value; }
        }
        public string Compania
        {
            get { return _compania; }
            set { _compania = value; }
        }
        public double Precio
        {
            get { return _precio; }
            set { _precio = value; }
        }
        public string Anio
        {
            get { return _anio; }
            set { _anio = value; }
        }

        #region Metodos sobreescritos
        public override string ToString()
        {
            return _codigo.ToString() + "," + _titulo.ToString() + "," + _artista.ToString() + "," + _pais.ToString() + "," + _compania.ToString() + "," + _precio.ToString() + "," + _anio.ToString();
        }

        #endregion
    }
}
