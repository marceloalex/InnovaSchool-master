using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InnovaSchool.Models
{
    public class GD_Articulos
    {
        public string CodigoArticulo  { get; set; }
        public string Descripcion { get; set; }
        public string Marca { get; set; }
        public string Proveedor { get; set; }
        public string UnidadMedida { get; set; }
        public string Cantidad { get; set; }

        public int CodProveedor { get; set; }
    }
}