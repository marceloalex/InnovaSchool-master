using InnovaSchool.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InnovaSchool.DAL
{
    public class DA_SolAdquisicion : BaseRepository<GD_SolAdquisicion>
    {

        public List<GrillaSolAdquisicion> lGrillaSolAdquisicion()
        {
            List<GrillaSolAdquisicion> lGrilla = new List<GrillaSolAdquisicion>();

            using (innovaEntities context = new innovaEntities())
            {
                var query = (from oSol in context.GD_SolAdquisicion
                             join oEmp in context.GD_Empleado on oSol.CodSolicitante equals oEmp.CodEmpleado
                             join oArea in context.GD_Area on oEmp.CodArea equals oArea.CodArea
                             join oEstado in context.GD_Estado on oSol.CodEstado equals oEstado.CodEstado
                             select new
                             {
                                 Codigo = oSol.CodSolAdquisicion,
                                 CodSolicitante = oSol.CodSolicitante,
                                 Solicitante = oEmp.ApePaterno + " " + oEmp.ApeMaterno + " " + oEmp.Nombres,
                                 CodArea = oEmp.CodArea,
                                 Area = oArea.DescArea,
                                 FechaEmision = oSol.FechaEmision,
                                 CodEstado = oSol.CodEstado,
                                 Estado = oEstado.DescEstado
                             }).ToList();

                foreach (var oCampos in query)
                {
                    GrillaSolAdquisicion oGrilla = new GrillaSolAdquisicion();
                    oGrilla.Codigo = oCampos.Codigo;
                    oGrilla.CodSolicitante = oCampos.CodSolicitante;
                    oGrilla.Solicitante = oCampos.Solicitante;
                    oGrilla.CodArea = oCampos.CodArea;
                    oGrilla.Area = oCampos.Area;
                    oGrilla.FechaEmision = oCampos.FechaEmision;
                    oGrilla.CodEstado = oCampos.CodEstado;
                    oGrilla.Estado = oCampos.Estado;

                    lGrilla.Add(oGrilla);
                }
            }

            return lGrilla;
        }

    }
}