//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace InnovaSchool.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class GD_ProveedorArticulo
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public GD_ProveedorArticulo()
        {
            this.GD_SolAdquisicionDet = new HashSet<GD_SolAdquisicionDet>();
        }
    
        public int CodProveedor { get; set; }
        public int CodArticulo { get; set; }
    
        public virtual GD_Articulo GD_Articulo { get; set; }
        public virtual GD_Proveedor GD_Proveedor { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GD_SolAdquisicionDet> GD_SolAdquisicionDet { get; set; }
    }
}
