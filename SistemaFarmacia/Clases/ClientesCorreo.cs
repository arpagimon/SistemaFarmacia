using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemaFarmacia.Clases
{
    public class ClienteCorreo
    {
        public int ID_CLIENTE { get; set; }
        public string Nombre  { get; set; }
        public string Apellido_paterno { get; set; }
        public string Apellido_materno { get; set; }
        public string EMAIL { get; set; }
        public string Enviar_Correo { get; set; }
    }
}