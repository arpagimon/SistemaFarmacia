using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemaFarmacia.Clases
{
    public class DatosCorreo
    {
        public DatosCorreo()
        {
            
        }

        public String SMTP_SSL { get; set; }
        public String SMTP_CORREO { get; set; }
        public String SMTP_PASS { get; set; }
        public String SMTP_HOST { get; set; }
        public String SMTP_PUERTO { get; set; }
        public String SMTP_SUJETO { get; set; }
        public String SMTP_MENSAJE { get; set; }
        public String SMTP_IMAGEN { get; set; }
        public String UltimoEnvio { get; set; }
        public String DiasAntes { get; set; }
        public String PRUEBAS { get; set; }
        public String SMTP_CORREO_PRUEBA { get; set; }
        public String ENV_ESTADO { get; set; }
    }
}