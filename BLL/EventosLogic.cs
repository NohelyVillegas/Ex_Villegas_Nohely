using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using Entities;
using Eventos = Entities.Eventos;

namespace BLL
{
    public class EventosLogic
    {
        public Eventos Create(Eventos evento)
        {
            Eventos _evento = null;
            using (var repository = RepositoryFactory.CreateRepository())
            {
                Eventos _result = repository.Retrieve<Eventos>
                    (e => e.Nombre == evento.Nombre && e.Lugar == evento.Lugar && e.FechaInicio == evento.FechaInicio);
                if (_result == null)
                {
                    _evento = repository.Create(evento);
                }
                else
                {
                    throw new Exception("El evento ya existe");
                }
            }
            return evento;
        }

        public Eventos RetrieveById(int id)
        {
            Eventos _evento = null;
            using (var repository = RepositoryFactory.CreateRepository())
            {
                _evento = repository.Retrieve<Eventos>(e => e.EventoID == id);
            }
            return _evento;
        }

        public bool Update(Eventos evento)
        {
            bool _updated = false;
            using (var repository = RepositoryFactory.CreateRepository())
            {
                Eventos _result = repository.Retrieve<Eventos>
                    (e => e.Nombre == evento.Nombre && e.EventoID != evento.EventoID);
                if (_result == null)
                {
                    _updated = repository.Update(evento);
                }
                else
                {
                    throw new Exception("Otro evento con el mismo nombre ya existe");
                }
            }
            return _updated;
        }

        public bool Delete(int id)
        {
            bool _delete = false;
            var _evento = RetrieveById(id);
            if (_evento != null)
            {
                using (var repository = RepositoryFactory.CreateRepository())
                {
                    _delete = repository.Delete(_evento);
                }
            }
            return _delete;
        }

        public List<Eventos> RetrieveAll()
        {
            using (var repository = RepositoryFactory.CreateRepository())
            {
                // Usar una expresión lambda para obtener todos los eventos
                return repository.Filter<Eventos>(e => e.EventoID > 0).ToList();
            }
        }
    }
}
