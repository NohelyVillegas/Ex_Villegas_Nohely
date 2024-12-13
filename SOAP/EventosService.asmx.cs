using System.Collections.Generic;
using System.Web.Services;
using BLL;
using Entidades = Entities; // Alias para el espacio de nombres Entities
using System.Xml.Serialization;

namespace SOAP
{
    /// <summary>
    /// Servicio web para la gestión de eventos.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    public class EventosService : System.Web.Services.WebService
    {
        private EventosLogic _logic = new EventosLogic();

        [WebMethod]
        public List<SerializableEvento> GetAllEventos()
        {
            var eventos = _logic.RetrieveAll();
            return eventos.ConvertAll(e => new SerializableEvento(e));
        }

        [WebMethod]
        public SerializableEvento GetEventoById(int id)
        {
            var evento = _logic.RetrieveById(id);
            if (evento == null)
            {
                return null;
            }
            return new SerializableEvento(evento);
        }

        [WebMethod]
        public void CreateEvento(SerializableEvento evento)
        {
            _logic.Create(evento.ToEvento());
        }

        [WebMethod]
        public void UpdateEvento(SerializableEvento evento)
        {
            _logic.Update(evento.ToEvento());
        }

        [WebMethod]
        public void DeleteEvento(int id)
        {
            _logic.Delete(id);
        }
    }

    /// <summary>
    /// Clase serializable para eventos.
    /// </summary>
    [XmlRoot("Evento")]
    public class SerializableEvento
    {
        public int EventoID { get; set; }
        public string Nombre { get; set; }
        public string Lugar { get; set; }
        public string FechaInicio { get; set; } // Usamos string para facilitar la serialización.
        public string FechaFin { get; set; } // Nullable, pero serializado como string.
        public string Descripcion { get; set; }

        public SerializableEvento() { }

        public SerializableEvento(Entidades.Eventos evento)
        {
            EventoID = evento.EventoID;
            Nombre = evento.Nombre;
            Lugar = evento.Lugar;
            FechaInicio = evento.FechaInicio.ToString("yyyy-MM-ddTHH:mm:ss");
            FechaFin = evento.FechaFin?.ToString("yyyy-MM-ddTHH:mm:ss");
            Descripcion = evento.Descripcion;
        }

        public Entidades.Eventos ToEvento()
        {
            return new Entidades.Eventos
            {
                EventoID = this.EventoID,
                Nombre = this.Nombre,
                Lugar = this.Lugar,
                FechaInicio = System.DateTime.Parse(this.FechaInicio),
                FechaFin = string.IsNullOrEmpty(this.FechaFin) ? (System.DateTime?)null : System.DateTime.Parse(this.FechaFin),
                Descripcion = this.Descripcion
            };
        }
    }
}
