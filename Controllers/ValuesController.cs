using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using helpDeskAPI.Models;

namespace helpDeskAPI.Controllers
{
    [RoutePrefix("api/values")]
    public class ValuesController : ApiController
    {
        // POST api/values -- obtener informacion para validacion de login 
        [AcceptVerbs("POST")]
        [HttpPost()]
        [Route("getEmpresas")]
        public IEnumerable<object> getEmpresas([FromBody] PARAM oPARAM)
        {
            using (dbHelpDesk db = new dbHelpDesk())
            {
                try
                {

                    var oEMPRESAS = db.hdCLIENTE
                        .Select(x => new { x.IDCLIENTE, x.RAZONSOCIAL })
                        .ToList();

                    /*
                    if (oUSUARIOS == null)
                        throw new Exception("El usuario no existe.");

                    if (oUSUARIOS.ESTATUS == "B")
                        throw new Exception("El usuario esta en estatus BAJA.");

                    if (_PASSW != oUSUARIOS.PASSW)
                        throw new Exception("La constraseña falló.");
                    */

                    return oEMPRESAS;

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
    }
}
