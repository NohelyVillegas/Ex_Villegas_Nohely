using System;
using System.Collections.Generic;
using System.Web.Mvc;
using BLL;
using Entities;

namespace YourNamespace.Controllers
{
    public class HomeController : Controller
    {
        private EventosLogic _eventosLogic = new EventosLogic();

        public ActionResult Index()
        {
            return RedirectToAction("List");
        }

        public ActionResult List()
        {
            try
            {
                var eventos = _eventosLogic.RetrieveAll();
                return View("EventList", eventos);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View("EventList", new List<Eventos>());
            }
        }

        public ActionResult Details(int id)
        {
            try
            {
                var evento = _eventosLogic.RetrieveById(id);
                if (evento == null)
                {
                    ViewBag.ErrorMessage = "Evento no encontrado.";
                    return RedirectToAction("List");
                }
                return View("EventDetails", evento);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return RedirectToAction("List");
            }
        }

        public ActionResult CUDForm(int? id = null)
        {
            Eventos model = id.HasValue ? _eventosLogic.RetrieveById(id.Value) : new Eventos();
            if (model == null && id.HasValue)
            {
                ViewBag.ErrorMessage = "Evento no encontrado.";
                return RedirectToAction("List");
            }
            return View("EventCUD", model);
        }

        [HttpPost]
        public ActionResult CUD(Eventos model, string CreateBtn, string UpdateBtn, string DeleteBtn)
        {
            try
            {
                if (!string.IsNullOrEmpty(CreateBtn))
                {
                    // Crear evento
                    _eventosLogic.Create(model);
                    ViewBag.SuccessMessage = "Evento creado exitosamente.";
                }
                else if (!string.IsNullOrEmpty(UpdateBtn))
                {
                    // Actualizar evento
                    _eventosLogic.Update(model);
                    ViewBag.SuccessMessage = "Evento actualizado exitosamente.";
                }
                else if (!string.IsNullOrEmpty(DeleteBtn))
                {
                    // Eliminar evento
                    if (_eventosLogic.Delete(model.EventoID))
                    {
                        ViewBag.SuccessMessage = "Evento eliminado exitosamente.";
                    }
                    else
                    {
                        ViewBag.ErrorMessage = "No se puede eliminar el evento.";
                    }
                }
                return RedirectToAction("List");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View("EventCUD", model);
            }
        }
        public ActionResult Delete(int id)
        {
            try
            {
                var evento = _eventosLogic.RetrieveById(id);
                if (evento == null)
                {
                    ViewBag.ErrorMessage = "Evento no encontrado.";
                    return RedirectToAction("List");
                }

                _eventosLogic.Delete(id);
                ViewBag.SuccessMessage = "Evento eliminado exitosamente.";
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
            }
            return RedirectToAction("List");
        }

    }
}
