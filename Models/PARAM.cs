using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace helpDeskAPI.Models
{
    public class PARAM
    {
        public string valor { get; set; }
        public int idcliente { get; set; }
        public string idusuario { get; set; }
        public string passw { get; set; }
        public string rol { get; set; }
        public List<string> sucursales { get; set; }
        public List<string> temas { get; set; }
    }
}