using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;

namespace InnovaSchool.Models
{
    public partial class EntSolAdquisicion : GD_SolAdquisicion
    {
        [Required]
        public string NroInforme { get; set; }
    }
}