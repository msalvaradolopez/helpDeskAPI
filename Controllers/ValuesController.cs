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
        [Route("getLogin")]
        public object getLogin([FromBody] PARAM oPARAM)
        {
            using (dbQuantusEntities db = new dbQuantusEntities())
            {
                try
                {

                    var _login = db.hdUSUARIO
                        .Where(x => x.IDUSUARIO == oPARAM.idusuario)
                        .Select(x => new { x.IDCLIENTE, x.IDUSUARIO, x.ROL, x.ESTATUS, x.PASSW })
                        .FirstOrDefault();

                    
                    if (_login == null)
                        throw new Exception("El usuario no existe.");

                    if (_login.ESTATUS == "B")
                        throw new Exception("El usuario esta en estatus BAJA.");

                    if (oPARAM.passw != _login.PASSW)
                        throw new Exception("La constraseña falló.");
                    

                    return _login;

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        // POST api/values -- obtener informacion para validacion de login 
        [AcceptVerbs("POST")]
        [HttpPost()]
        [Route("getEmpresas")]
        public IEnumerable<object> getEmpresas([FromBody] PARAM oPARAM)
        {
            using (dbQuantusEntities db = new dbQuantusEntities())
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

        // POST api/values -- LISTADO DE SUCURSALES.
        [AcceptVerbs("POST")]
        [HttpPost()]
        [Route("getSucursalesList")]
        public IEnumerable<object> getSucursalesList([FromBody] PARAM oPARAM)
        {
            using (dbQuantusEntities db = new dbQuantusEntities())
            {
                try
                {
                    string _valor = oPARAM.valor == "" ? "0" : oPARAM.valor;

                    var oSUCURSALES = db.hdSUCURSAL
                        .Where(x => x.IDCLIENTE == oPARAM.idcliente && (x.IDSUCURSAL.Contains(_valor) || x.NOMSUCURSAL.Contains(_valor) || _valor == "0" ))
                        .Select(x => new { x.IDSUCURSAL, x.NOMSUCURSAL })
                        .ToList();

                    /*
                    if (oUSUARIOS == null)
                        throw new Exception("El usuario no existe.");

                    if (oUSUARIOS.ESTATUS == "B")
                        throw new Exception("El usuario esta en estatus BAJA.");

                    if (_PASSW != oUSUARIOS.PASSW)
                        throw new Exception("La constraseña falló.");
                    */

                    return oSUCURSALES;

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        // POST api/values -- LISTADO DE SUCURSALES.
        [AcceptVerbs("POST")]
        [HttpPost()]
        [Route("getSucursalByID")]
        public object getSucursalByID([FromBody] PARAM oPARAM)
        {
            using (dbQuantusEntities db = new dbQuantusEntities())
            {
                try
                {
                    string _valor = oPARAM.valor == "" ? "0" : oPARAM.valor;

                    var oSUCURSALES = db.hdSUCURSAL
                        .Where(x => x.IDCLIENTE == oPARAM.idcliente && x.IDSUCURSAL == _valor)
                        .Select(x => new { x.IDSUCURSAL, x.IDCLIENTE, x.NOMSUCURSAL})
                        .FirstOrDefault();

                    return oSUCURSALES;

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        // POST api/values -- INGRESA SUCURSAL.
        [AcceptVerbs("POST")]
        [HttpPost()]
        [Route("insSucursal")]
        public string insSucursal([FromBody] hdSUCURSAL oSUCURSAL)
        {
            using (dbQuantusEntities db = new dbQuantusEntities())
            {
                try
                {

                    var _sucursal = db.hdSUCURSAL
                        .Where(x => x.IDSUCURSAL == oSUCURSAL.IDSUCURSAL && x.IDCLIENTE == oSUCURSAL.IDCLIENTE)
                        .SingleOrDefault();


                    if (_sucursal  != null)
                        throw new Exception("La sucursal YA existe.");

                    db.hdSUCURSAL.Add(oSUCURSAL);
                    db.SaveChanges();

                    return "Sucursal ingresada ok.";

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        // POST api/values -- INGRESA SUCURSAL.
        [AcceptVerbs("POST")]
        [HttpPost()]
        [Route("updSucursal")]
        public string updSucursal([FromBody] hdSUCURSAL oSUCURSAL)
        {
            using (dbQuantusEntities db = new dbQuantusEntities())
            {
                try
                {

                    var _sucursal = db.hdSUCURSAL
                        .Where(x => x.IDSUCURSAL == oSUCURSAL.IDSUCURSAL && x.IDCLIENTE == oSUCURSAL.IDCLIENTE)
                        .SingleOrDefault();


                    if (_sucursal == null)
                        throw new Exception("La sucursal no existe.");


                    _sucursal.NOMSUCURSAL = oSUCURSAL.NOMSUCURSAL;
                    db.SaveChanges();

                    return "Sucursal ingresada ok.";

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        // POST api/values -- INGRESA SUCURSAL.
        [AcceptVerbs("POST")]
        [HttpPost()]
        [Route("delSucursal")]
        public string delSucursal([FromBody] hdSUCURSAL oSUCURSAL)
        {
            using (dbQuantusEntities db = new dbQuantusEntities())
            {
                try
                {

                    var _sucursal = db.hdSUCURSAL
                        .Where(x => x.IDSUCURSAL == oSUCURSAL.IDSUCURSAL && x.IDCLIENTE == oSUCURSAL.IDCLIENTE)
                        .SingleOrDefault();


                    if (_sucursal == null)
                        throw new Exception("La sucursal no existe.");


                    db.hdSUCURSAL.Remove(_sucursal);
                    db.SaveChanges();

                    return "Sucursal eliminada ok.";

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
    }
}
