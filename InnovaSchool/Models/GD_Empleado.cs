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
    
    public partial class GD_Empleado
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public GD_Empleado()
        {
            this.GD_SolAdquisicion = new HashSet<GD_SolAdquisicion>();
            this.GD_SolAdquisicion1 = new HashSet<GD_SolAdquisicion>();
        }
    
        public int CodEmpleado { get; set; }
        public int CodDocumento { get; set; }
        public string NroDocumento { get; set; }
        public string ApePaterno { get; set; }
        public string ApeMaterno { get; set; }
        public string Nombres { get; set; }
        public int CodArea { get; set; }
    
        public virtual GD_Area GD_Area { get; set; }
        public virtual GD_Documento GD_Documento { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GD_SolAdquisicion> GD_SolAdquisicion { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GD_SolAdquisicion> GD_SolAdquisicion1 { get; set; }
    }
}
