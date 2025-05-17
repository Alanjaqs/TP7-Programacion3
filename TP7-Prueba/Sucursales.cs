using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TP7_Prueba
{
    public class Sucursales
    {
        // ATRIBUTOS
           
        private int id;
        private string nombre;
        private string descripcion;

        // CONSTRUCTOR

        public Sucursales(int id, string nombre, string descripcion)
        {
            this.id = id;
            this.nombre = nombre; 
            this.descripcion = descripcion;
        }
    }
}