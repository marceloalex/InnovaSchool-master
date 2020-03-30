using InnovaSchool.DAL;
using InnovaSchool.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InnovaSchool.Controllers
{
    public class SolCotizacionController : Controller
    {
        private innovaEntities context = new innovaEntities();

        //
        // GET: /SolCotizacion/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SolicitudCotizacion()
        {

            Session["ListArticulosSolitudAdquisicion"] = null;

            List<GD_SolicitudCotizacion> oList = new List<GD_SolicitudCotizacion>();
            GD_SolicitudCotizacion oSolicitudCotizacion;



            //DAL_I.innova22Entities oDA = new DAL_I.innova22Entities();
            foreach (var item in context.sp_ListaCotizaciones())
            {
                oSolicitudCotizacion = new GD_SolicitudCotizacion()
                {
                    CodSolCotizacion = item.CodSolCotizacion.ToString(),
                    CodSolAdquisicion = item.CodSolAdquisicion.ToString(),
                    RazonSocial = item.RazonSocial,
                    FechaSolicitud = item.FechaCotizacion.ToString(),
                    EstadoSolicitud = item.DescEstado
                };
                oList.Add(oSolicitudCotizacion);

            }


            ViewBag.MostrarBotonesSolicitud = false;
            ViewBag.ListSolicitudAdquisicion = oList;
            return View();
        }

        public ActionResult SolicitudNueva()
        {
            return View();
        }

        public ActionResult BuscarArticulos(string strCodSolAdquisicion = "")
        {
            List<GD_Articulos> oList = new List<GD_Articulos>();
            GD_Articulos oArticulos;

            int intMostrarBotonGrabar = 0;

            //DAL_I.innova22Entities oDA = new DAL_I.innova22Entities();
            foreach (var item in context.sp_BuscarArticulos_SolAdquisicion(int.Parse(strCodSolAdquisicion)))
            {
                oArticulos = new GD_Articulos()
                {
                    CodigoArticulo = item.CodArticulo.ToString(),
                    Descripcion = item.DescArticulo,
                    Marca = item.Marca,
                    Proveedor = item.RazonSocial,
                    UnidadMedida = item.DescArticulo,
                    Cantidad = item.Cantidad.ToString(),
                    CodProveedor = item.CodProveedor
                };
                oList.Add(oArticulos);
                intMostrarBotonGrabar++;
            }

            if (intMostrarBotonGrabar > 0)
                ViewBag.MostrarBotonGrabar = true;
            else
                ViewBag.MostrarBotonGrabar = false;

                Session["ListArticulosSolitudAdquisicion"] = oList;
            ViewBag.ListArticulos = oList;
            return View();
        }



        public ActionResult Grabar(string strCodSolAdquisicion = "")
        {
            string strCodSolCotizacion = "";
            string strRespuesta = "";
            if (Session["ListArticulosSolitudAdquisicion"] != null)
            {
                //DAL_I.innova22Entities oDA = new DAL_I.innova22Entities();
                List<GD_Articulos> oList = new List<GD_Articulos>();
                oList = (List<GD_Articulos>)Session["ListArticulosSolitudAdquisicion"];
                var listDistinctProveedor = oList.GroupBy(i => i.CodProveedor, (key, group) => group.First()).ToArray();
                foreach (var item in listDistinctProveedor)
                {
                    //Console.WriteLine(item.CodProveedor);
                    foreach (var itemCat in context.sp_GrabarCabSolCotizacion(int.Parse(strCodSolAdquisicion), item.CodProveedor))
                    {
                        strCodSolCotizacion = itemCat.Value.ToString();

                        foreach (var itemDetInsert in context.sp_GrabarDetSolCotizacion(int.Parse(strCodSolCotizacion), int.Parse(strCodSolAdquisicion), item.CodProveedor))
                        {
                            strRespuesta = itemDetInsert.Trim().ToString();
                        }
                    }
                }

            }
            return Json(new
            {
                Respuesta = strRespuesta.ToString()
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SolicitudConsultar(string strCodSolCotizacion = "")
        {
            List<GD_Articulos> oList = new List<GD_Articulos>();
            GD_Articulos oArticulos;

            //DAL_I.innova22Entities oDA = new DAL_I.innova22Entities();
            foreach (var item in context.sp_ConsultarCotizacion(int.Parse(strCodSolCotizacion)))
            {
                ViewBag.Solicitud = item.CodSolCotizacion;
                ViewBag.Adquisicion = item.CodSolAdquisicion;
                ViewBag.Proveedor = item.RazonSocial;

                oArticulos = new GD_Articulos()
                {
                    CodigoArticulo = item.CodArticulo.ToString(),
                    Descripcion = item.DescArticulo,
                    Marca = item.Marca,
                    UnidadMedida = item.DescUniMedida,
                    Cantidad = item.Cantidad.ToString()
                };
                oList.Add(oArticulos);
            }

            ViewBag.ListArticulosModificar = oList;
            return View();
        }

        public ActionResult SolicitudModificar(string strCodSolCotizacion = "")
        {
            List<GD_Articulos> oList = new List<GD_Articulos>();
            GD_Articulos oArticulos;

            //DAL_I.innova22Entities oDA = new DAL_I.innova22Entities();
            foreach (var item in context.sp_ConsultarCotizacion(int.Parse(strCodSolCotizacion)))
            {
                ViewBag.Solicitud = item.CodSolCotizacion;
                ViewBag.Adquisicion = item.CodSolAdquisicion;
                ViewBag.Proveedor = item.RazonSocial;

                oArticulos = new GD_Articulos()
                {
                    CodigoArticulo = item.CodArticulo.ToString(),
                    Descripcion = item.DescArticulo,
                    Marca = item.Marca,
                    UnidadMedida = item.DescUniMedida,
                    Cantidad = item.Cantidad.ToString()
                };
                oList.Add(oArticulos);
            }
            ViewBag.ListArticulosModificar = oList;
            return View();
        }

        public ActionResult EliminarArticulo(string strCodSolCotizacion = "", string strCodArticulo = "")
        {
            string strRespuesta = "";
            //DAL_I.innova22Entities oDA = new DAL_I.innova22Entities();
            foreach (var item in context.sp_EliminarArticuloSolCotizacion(int.Parse(strCodSolCotizacion), int.Parse(strCodArticulo)))
            {
                strRespuesta = item.Trim().ToString();
            }
            return Json(new
            {
                Respuesta = strRespuesta.ToString()
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SolicitudEliminar(string strCodSolCotizacion = "")
        {
            int intRespuesta = 0;
            //DAL_I.innova22Entities oDA = new DAL_I.innova22Entities();
            intRespuesta = context.sp_EliminarCotizacion(int.Parse(strCodSolCotizacion));

            return Json(new
            {
                Respuesta = intRespuesta.ToString()
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SolicitudActualizarEstado(string strCodSolCotizacion = "")
        {
            int intRespuesta = 0;
            //DAL_I.innova22Entities oDA = new DAL_I.innova22Entities();
            intRespuesta = context.sp_ActualizarCotizacion(int.Parse(strCodSolCotizacion));

            return Json(new
            {
                Respuesta = intRespuesta.ToString()
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ObtenerEstadoCotizacion(string strCodSolCotizacion = "")
        {
            string strRespuesta = "";
            //DAL_I.innova22Entities oDA = new DAL_I.innova22Entities();

            foreach (var item in context.sp_ObetenerEstadoCotizacion(int.Parse(strCodSolCotizacion)))
            {
                strRespuesta = item.Trim().ToString();
            }

            return Json(new
            {
                Respuesta = strRespuesta.ToString()
            }, JsonRequestBehavior.AllowGet);
        }

    }
}