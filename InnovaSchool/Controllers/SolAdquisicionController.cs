using InnovaSchool.DAL;
using InnovaSchool.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using System.Threading.Tasks;
using System.Net;
using System.Data;
using System.Data.Entity;
using System.IO;

namespace InnovaSchool.Controllers
{
    public class SolAdquisicionController : Controller
    {
        private innovaEntities context = new innovaEntities();

        // GET: SolAdquisicion
        public ViewResult ConsultaAdquisicion()
        {

            List<GrillaSolAdquisicion> lGrillaSolAdquisicion = new DA_SolAdquisicion().lGrillaSolAdquisicion();

            return View(lGrillaSolAdquisicion.ToPagedList(1, 50));
        }

        public ViewResult NuevoAdquisicion()
        {
            var oQuery = (from oEmpleado in context.GD_Empleado
                          where oEmpleado.CodEmpleado != 1
                          select new
                          {
                              oEmpleado.CodEmpleado,
                              FullNombre = oEmpleado.ApePaterno + " " + oEmpleado.ApeMaterno + ", " + oEmpleado.Nombres
                          }).ToList();

            ViewBag.CodSolicitante = new SelectList(oQuery, "CodEmpleado", "FullNombre");
            ViewBag.CodEstado = new SelectList(context.GD_Estado, "CodEstado", "DescEstado");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> NuevoAdquisicion([Bind(Include = "FechaEmision,CodSolicitante,NroInforme,Observacion")] GD_SolAdquisicion oSolAdquisicion)
        {
            if (ModelState.IsValid)
            {
                GD_SolAdquisicion oSolAdq = new GD_SolAdquisicion();
                oSolAdq.FechaEmision = oSolAdquisicion.FechaEmision;
                oSolAdq.CodSolicitante = oSolAdquisicion.CodSolicitante;
                oSolAdq.CodEmpleado = 1;
                oSolAdq.CodEstado = "E";
                oSolAdq.NroInforme = oSolAdquisicion.NroInforme;
                oSolAdq.Observacion = oSolAdquisicion.Observacion;

                new DA_SolAdquisicion().Create(oSolAdq);
                await context.SaveChangesAsync();
                return RedirectToAction("ConsultaAdquisicion");
            }

            var oSolicitante = (from oEmpleado in context.GD_Empleado
                          where oEmpleado.CodEmpleado != 1
                          select new
                          {
                              oEmpleado.CodEmpleado,
                              FullNombre = oEmpleado.ApePaterno + " " + oEmpleado.ApeMaterno + ", " + oEmpleado.Nombres
                          }).ToList();

            ViewBag.CodSolicitante = new SelectList(oSolicitante, "CodEmpleado", "FullNombre", oSolAdquisicion.CodSolicitante);

            var oEstado = new DA_Estado().Single(x => x.CodEstado == "E").GetType().GetProperties().ToList();

            ViewBag.CodEstado = new SelectList(oEstado, "CodEstado", "DescEstado", oSolAdquisicion.CodEstado);
            return View(oSolAdquisicion);
        }

        public ViewResult EditarAdquisicion(int id)
        {
            var oQuery = (from oEmpleado in context.GD_Empleado
                          where oEmpleado.CodEmpleado != 1
                          select new
                          {
                              oEmpleado.CodEmpleado,
                              FullNombre = oEmpleado.ApePaterno + " " + oEmpleado.ApeMaterno + ", " + oEmpleado.Nombres
                          }).ToList();

            ViewBag.CodSolicitante = new SelectList(oQuery, "CodEmpleado", "FullNombre");
            ViewBag.CodEstado = new SelectList(context.GD_Estado, "CodEstado", "DescEstado");

            //EntSolAdquisicion oSolAdquisicion = (EntSolAdquisicion)new DA_SolAdquisicion().Single(x => x.CodSolAdquisicion == id);
            var oSolAdquisicion = context.GD_SolAdquisicion.Where(x => x.CodSolAdquisicion == id).GetType().GetProperties().ToList();

            return View(oSolAdquisicion);
        }

        public ViewResult VerAdquisicion()
        {
            ViewBag.CodSolicitante = new SelectList(context.GD_Empleado, "CodEmpleado", "ApePaterno");
            ViewBag.CodEstado = new SelectList(context.GD_Estado, "CodEstado", "DescEstado");
            return View();
        }

        public ViewResult EliminarAdquisicion()
        {

            ViewBag.CodSolicitante = new SelectList(context.GD_Empleado, "CodEmpleado", "ApePaterno");
            ViewBag.CodEstado = new SelectList(context.GD_Estado, "CodEstado", "DescEstado");
            return View();
        }

        public JsonResult Adjuntar(HttpPostedFileBase documento)
        {
            var respuesta = new ResponseModel
            {
                respuesta = true,
                error = ""
            };

            if (documento != null)
            {
                string adjunto = Path.GetFileName(documento.FileName);
                if (!(System.IO.File.Exists(Server.MapPath("~/uploads/" + adjunto))))
                    documento.SaveAs(Server.MapPath("~/uploads/" + adjunto));
            }
            else
            {
                respuesta.respuesta = false;
                respuesta.error = "Debe adjuntar un documento";
            }

            return Json(respuesta);
        }

    }
}