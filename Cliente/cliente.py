import zeep
from zeep.transports import Transport
from requests import Session
import urllib3  # Importar para deshabilitar las advertencias

# Deshabilitar todas las advertencias de SSL
urllib3.disable_warnings(urllib3.exceptions.InsecureRequestWarning)

# URL del servicio SOAP para eventos
url = 'https://localhost:44332/EventosService.asmx?WSDL'

# Deshabilitar la verificación del certificado SSL
session = Session()
session.verify = False  # No verificar el certificado SSL

# Actualizar los encabezados de la solicitud si es necesario
session.headers.update({
    'User-Agent': 'ZeepClient',
    'Content-Type': 'text/xml;charset=UTF-8'
})

# Crear un transporte con la sesión modificada
transport = Transport(session=session)

# Crear el cliente SOAP usando la URL del WSDL y el transporte
try:
    client = zeep.Client(url, transport=transport)
    print("Cliente SOAP creado correctamente.")
except Exception as e:
    print(f"Error al crear el cliente SOAP: {e}")
    exit()

# Método para obtener todos los eventos
def obtener_eventos():
    try:
        # Llamada al método "GetAllEventos" del servicio SOAP
        eventos = client.service.GetAllEventos()
        
        # Mostrar los eventos obtenidos
        for evento in eventos:
            print(f"ID: {evento.EventoID}, Nombre: {evento.Nombre}, Fecha Inicio: {evento.FechaInicio}, Lugar: {evento.Lugar}")
    except Exception as e:
        print(f"Error al obtener eventos: {e}")

# Método para obtener un evento por ID
def obtener_evento_por_id(id_evento):
    try:
        # Llamada al método "GetEventoById" del servicio SOAP
        evento = client.service.GetEventoById(id_evento)
        
        # Mostrar el evento obtenido
        if evento:
            print(f"ID: {evento.EventoID}, Nombre: {evento.Nombre}, Fecha Inicio: {evento.FechaInicio}, Lugar: {evento.Lugar}")
        else:
            print(f"Evento con ID {id_evento} no encontrado.")
    except Exception as e:
        print(f"Error al obtener evento por ID: {e}")

# Método para agregar un nuevo evento
def agregar_evento(nombre, lugar, fecha_inicio, fecha_fin, descripcion):
    try:
        # Crear un objeto de evento serializable
        evento = {
            'EventoID': 0,
            'Nombre': nombre,
            'Lugar': lugar,
            'FechaInicio': fecha_inicio,
            'FechaFin': fecha_fin,
            'Descripcion': descripcion
        }
        
        # Llamada al método "CreateEvento" del servicio SOAP
        client.service.CreateEvento(evento)
        
        print(f"Evento '{nombre}' agregado con éxito.")
    except Exception as e:
        print(f"Error al agregar evento: {e}")

# Método para actualizar un evento existente
def actualizar_evento(id_evento, nombre, lugar, fecha_inicio, fecha_fin, descripcion):
    try:
        # Crear un objeto de evento serializable
        evento = {
            'EventoID': id_evento,
            'Nombre': nombre,
            'Lugar': lugar,
            'FechaInicio': fecha_inicio,
            'FechaFin': fecha_fin,
            'Descripcion': descripcion
        }
        
        # Llamada al método "UpdateEvento" del servicio SOAP
        client.service.UpdateEvento(evento)
        
        print(f"Evento con ID {id_evento} actualizado con éxito.")
    except Exception as e:
        print(f"Error al actualizar evento: {e}")

# Método para eliminar un evento
def eliminar_evento(id_evento):
    try:
        # Llamada al método "DeleteEvento" del servicio SOAP
        client.service.DeleteEvento(id_evento)
        
        print(f"Evento con ID {id_evento} eliminado con éxito.")
    except Exception as e:
        print(f"Error al eliminar evento: {e}")

# Llamada de ejemplo para obtener todos los eventos
obtener_eventos()

# Llamada de ejemplo para obtener un evento por ID
obtener_evento_por_id(6)

# Llamada de ejemplo para agregar un evento
agregar_evento("Conferencia 2024", "Sala A", "2024-12-15T10:00:00", "2024-12-15T12:00:00", "Evento de tecnología")

# Llamada de ejemplo para actualizar un evento (usando un ID ficticio)
actualizar_evento(4, "Conferencia 2024 Actualizada", "Sala B", "2024-12-16T10:00:00", "2024-12-16T12:00:00", "Evento de tecnología actualizado")

# Llamada de ejemplo para eliminar un evento (usando un ID ficticio)
eliminar_evento(10)
