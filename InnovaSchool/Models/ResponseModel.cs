using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InnovaSchool.Models
{
    public class ResponseModel
    {
        public bool respuesta { get; set; }
        public string redirect { get; set; }
        public string error { get; set; }
    }
}