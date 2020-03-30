using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InnovaSchool.Models
{
    public class GrillaSolAdquisicion
    {
        [Display(Name = "Codigo")]
        public int Codigo { get; set; }
        [Display(Name = "Codigo de Solicitante")]
        public int CodSolicitante { get; set; }
        [Display(Name = "Solicitante")]
        public string Solicitante { get; set; }
        [Display(Name = "Codigo de Area")]
        public int CodArea { get; set; }
        [Display(Name = "Area")]
        public string Area { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Fecha de Emision")]
        public DateTime FechaEmision { get; set; }
        [Display(Name = "Codigo de Estado")]
        public string CodEstado { get; set; }
        [Display(Name = "Estado")]
        public string Estado { get; set; }

    }
}