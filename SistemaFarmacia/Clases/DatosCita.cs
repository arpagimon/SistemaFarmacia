using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemaFarmacia.Clases
{
    public class DatosCita
    {
        public int ID_Cita { get; set; }
        public string Fecha { get; set; }
        public int ID_Cliente { get; set; }
        public string Nombre { get; set; }
        public string ApPaterno { get; set; }
        public string ApMaterno { get; set; }
        public string Correo { get; set; }
        public string Hora_Inicio { get; set; }
        public string Hora_fin { get; set; }
        public string Nota { get; set; }
    }
}