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
                        throw new Exception("La constraseña no es correcta.");


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
                        .Where(x => x.IDCLIENTE == oPARAM.idcliente && (x.IDSUCURSAL.Contains(_valor) || x.NOMSUCURSAL.Contains(_valor) || _valor == "0"))
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
                        .Select(x => new { x.IDSUCURSAL, x.IDCLIENTE, x.NOMSUCURSAL })
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


                    if (_sucursal != null)
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

        // POST api/values -- LISTADO DE DEPTOS.
        [AcceptVerbs("POST")]
        [HttpPost()]
        [Route("getDeptosList")]
        public IEnumerable<object> getDeptosList([FromBody] PARAM oPARAM)
        {
            using (dbQuantusEntities db = new dbQuantusEntities())
            {
                try
                {
                    string _valor = oPARAM.valor == "" ? "0" : oPARAM.valor;

                    var _deptos = db.hdDEPTO
                        .Where(x => x.IDCLIENTE == oPARAM.idcliente && (x.NOMDEPTO.Contains(_valor) || _valor == "0"))
                        .Select(x => new { x.IDDEPTO, x.NOMDEPTO })
                        .ToList();

                    /*
                    if (oUSUARIOS == null)
                        throw new Exception("El usuario no existe.");

                    if (oUSUARIOS.ESTATUS == "B")
                        throw new Exception("El usuario esta en estatus BAJA.");

                    if (_PASSW != oUSUARIOS.PASSW)
                        throw new Exception("La constraseña falló.");
                    */

                    return _deptos;

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
        [Route("getDeptoByID")]
        public object getDeptoByID([FromBody] PARAM oPARAM)
        {
            using (dbQuantusEntities db = new dbQuantusEntities())
            {
                try
                {
                    string _valor = oPARAM.valor == "" ? "0" : oPARAM.valor;

                    var _deptos = db.hdDEPTO
                        .Where(x => x.IDCLIENTE == oPARAM.idcliente && x.IDDEPTO.ToString() == _valor)
                        .Select(x => new { x.IDDEPTO, x.IDCLIENTE, x.NOMDEPTO })
                        .FirstOrDefault();

                    return _deptos;

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
        [Route("insDepto")]
        public string insDepto([FromBody] hdDEPTO oDEPTO)
        {
            using (dbQuantusEntities db = new dbQuantusEntities())
            {
                try
                {

                    var _depto = db.hdDEPTO
                        .Where(x => x.IDDEPTO == oDEPTO.IDDEPTO && x.IDCLIENTE == oDEPTO.IDCLIENTE)
                        .SingleOrDefault();


                    if (_depto != null)
                        throw new Exception("El departamento YA existe.");

                    db.hdDEPTO.Add(oDEPTO);
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
        [Route("updDepto")]
        public string updDepto([FromBody] hdDEPTO oDEPTO)
        {
            using (dbQuantusEntities db = new dbQuantusEntities())
            {
                try
                {

                    var _depto = db.hdDEPTO
                        .Where(x => x.IDDEPTO == oDEPTO.IDDEPTO && x.IDCLIENTE == oDEPTO.IDCLIENTE)
                        .SingleOrDefault();


                    if (_depto == null)
                        throw new Exception("el departamento no existe.");


                    _depto.NOMDEPTO = oDEPTO.NOMDEPTO;
                    db.SaveChanges();

                    return "Registro ingresado ok.";

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
        [Route("delDepto")]
        public string delDepto([FromBody] hdDEPTO oDEPTO)
        {
            using (dbQuantusEntities db = new dbQuantusEntities())
            {
                try
                {

                    var _depto = db.hdDEPTO
                        .Where(x => x.IDDEPTO == oDEPTO.IDDEPTO && x.IDCLIENTE == oDEPTO.IDCLIENTE)
                        .SingleOrDefault();


                    if (_depto == null)
                        throw new Exception("El departamento no existe.");


                    db.hdDEPTO.Remove(_depto);
                    db.SaveChanges();

                    return "Registro eliminado ok.";

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        // POST api/values -- LISTADO DE DEPTOS.
        [AcceptVerbs("POST")]
        [HttpPost()]
        [Route("getUsuariosList")]
        public IEnumerable<object> getUsuariosList([FromBody] PARAM oPARAM)
        {
            using (dbQuantusEntities db = new dbQuantusEntities())
            {
                try
                {
                    string _valor = oPARAM.valor == "" ? "0" : oPARAM.valor;

                    var _usarios = db.hdUSUARIO
                        .Where(x => x.IDCLIENTE == oPARAM.idcliente && (x.NOMUSUARIO.Contains(_valor) || _valor == "0"))
                        .Select(x => new { x.IDUSUARIO, x.IDCLIENTE, x.hdSUCURSAL.NOMSUCURSAL, x.hdDEPTO.NOMDEPTO, x.NOMUSUARIO, x.EMAIL, x.TELEFONO, x.PASSW, x.ROL, x.ESTATUS })
                        .ToList();

                    /*
                    if (oUSUARIOS == null)
                        throw new Exception("El usuario no existe.");

                    if (oUSUARIOS.ESTATUS == "B")
                        throw new Exception("El usuario esta en estatus BAJA.");

                    if (_PASSW != oUSUARIOS.PASSW)
                        throw new Exception("La constraseña falló.");
                    */

                    return _usarios;

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
        [Route("getUsuarioByID")]
        public object getUsuarioByID([FromBody] PARAM oPARAM)
        {
            using (dbQuantusEntities db = new dbQuantusEntities())
            {
                try
                {
                    string _valor = oPARAM.valor == "" ? "0" : oPARAM.valor;

                    var _usuario = db.hdUSUARIO
                        .Where(x => x.IDCLIENTE == oPARAM.idcliente && x.IDUSUARIO == _valor)
                        .Select(x => new { x.IDUSUARIO, x.IDCLIENTE, x.IDSUCURSAL, x.IDDEPTO, x.NOMUSUARIO, x.EMAIL, x.TELEFONO, x.PASSW, x.ROL, x.ESTATUS })
                        .FirstOrDefault();

                    return _usuario;

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
        [Route("insUsuario")]
        public string insUsuario([FromBody] hdUSUARIO oUSUARIO)
        {
            using (dbQuantusEntities db = new dbQuantusEntities())
            {
                try
                {

                    var _usuario = db.hdUSUARIO
                        .Where(x => x.IDUSUARIO == oUSUARIO.IDUSUARIO && x.IDCLIENTE == oUSUARIO.IDCLIENTE)
                        .SingleOrDefault();


                    if (_usuario != null)
                        throw new Exception("El usuario YA existe.");

                    db.hdUSUARIO.Add(oUSUARIO);
                    db.SaveChanges();

                    return "Registro ingresado ok.";

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
        [Route("updUsuario")]
        public string updUsuario([FromBody] hdUSUARIO oUSUARIO)
        {
            using (dbQuantusEntities db = new dbQuantusEntities())
            {
                try
                {

                    var _usuario = db.hdUSUARIO
                        .Where(x => x.IDUSUARIO == oUSUARIO.IDUSUARIO && x.IDCLIENTE == oUSUARIO.IDCLIENTE)
                        .SingleOrDefault();


                    if (_usuario == null)
                        throw new Exception("el usuario no existe.");


                    _usuario.IDSUCURSAL = oUSUARIO.IDSUCURSAL;
                    _usuario.IDDEPTO = oUSUARIO.IDDEPTO;
                    _usuario.NOMUSUARIO = oUSUARIO.NOMUSUARIO;
                    _usuario.EMAIL = oUSUARIO.EMAIL;
                    _usuario.TELEFONO = oUSUARIO.TELEFONO;
                    _usuario.PASSW = oUSUARIO.PASSW;
                    _usuario.ROL = oUSUARIO.ROL;
                    _usuario.ESTATUS = oUSUARIO.ESTATUS;

                    db.SaveChanges();

                    return "Registro ingresado ok.";

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
        [Route("delUsuario")]
        public string delUsuario([FromBody] hdUSUARIO oUSUARIO)
        {
            using (dbQuantusEntities db = new dbQuantusEntities())
            {
                try
                {

                    var _usuario = db.hdUSUARIO
                        .Where(x => x.IDUSUARIO == oUSUARIO.IDUSUARIO && x.IDCLIENTE == oUSUARIO.IDCLIENTE)
                        .SingleOrDefault();


                    if (_usuario == null)
                        throw new Exception("El usuario no existe.");

                    db.hdUSUARIO.Remove(_usuario);
                    db.SaveChanges();

                    return "Registro eliminado ok.";

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        // POST api/values -- LISTADO DE DEPTOS.
        [AcceptVerbs("POST")]
        [HttpPost()]
        [Route("getTiposList")]
        public IEnumerable<object> getTiposList([FromBody] PARAM oPARAM)
        {
            using (dbQuantusEntities db = new dbQuantusEntities())
            {
                try
                {
                    string _valor = oPARAM.valor == "" ? "0" : oPARAM.valor;

                    var _tipos = db.hdTIPO
                        .Where(x => x.IDCLIENTE == oPARAM.idcliente && (x.NOMTIPO.Contains(_valor) || _valor == "0"))
                        .Select(x => new { x.IDTIPO, x.NOMTIPO })
                        .ToList();

                    /*
                    if (oUSUARIOS == null)
                        throw new Exception("El usuario no existe.");

                    if (oUSUARIOS.ESTATUS == "B")
                        throw new Exception("El usuario esta en estatus BAJA.");

                    if (_PASSW != oUSUARIOS.PASSW)
                        throw new Exception("La constraseña falló.");
                    */

                    return _tipos;

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
        [Route("getTipoByID")]
        public object getTipoByID([FromBody] PARAM oPARAM)
        {
            using (dbQuantusEntities db = new dbQuantusEntities())
            {
                try
                {
                    string _valor = oPARAM.valor == "" ? "0" : oPARAM.valor;

                    var _tipo = db.hdTIPO
                        .Where(x => x.IDCLIENTE == oPARAM.idcliente && x.IDTIPO.ToString() == _valor)
                        .Select(x => new { x.IDTIPO, x.IDCLIENTE, x.NOMTIPO })
                        .FirstOrDefault();

                    return _tipo;

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
        [Route("insTipo")]
        public string insTipo([FromBody] hdTIPO oTIPO)
        {
            using (dbQuantusEntities db = new dbQuantusEntities())
            {
                try
                {

                    var _tipo = db.hdTIPO
                        .Where(x => x.IDTIPO == oTIPO.IDTIPO && x.IDCLIENTE == oTIPO.IDCLIENTE)
                        .SingleOrDefault();


                    if (_tipo != null)
                        throw new Exception("El tipo YA existe.");

                    db.hdTIPO.Add(oTIPO);
                    db.SaveChanges();

                    return "Registro ingresado ok.";

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
        [Route("updTipo")]
        public string updTipo([FromBody] hdTIPO oTIPO)
        {
            using (dbQuantusEntities db = new dbQuantusEntities())
            {
                try
                {

                    var _tipo = db.hdTIPO
                        .Where(x => x.IDTIPO == oTIPO.IDTIPO && x.IDCLIENTE == oTIPO.IDCLIENTE)
                        .SingleOrDefault();


                    if (_tipo == null)
                        throw new Exception("el tipo no existe.");


                    _tipo.NOMTIPO = oTIPO.NOMTIPO;
                    db.SaveChanges();

                    return "Registro actualizado ok.";

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        // POST api/values -- ELIMINA TIPO.
        [AcceptVerbs("POST")]
        [HttpPost()]
        [Route("detTipo")]
        public string detTipo([FromBody] hdTIPO oTIPO)
        {
            using (dbQuantusEntities db = new dbQuantusEntities())
            {
                try
                {

                    var _tipo = db.hdTIPO
                        .Where(x => x.IDTIPO == oTIPO.IDTIPO && x.IDCLIENTE == oTIPO.IDCLIENTE)
                        .SingleOrDefault();


                    if (_tipo == null)
                        throw new Exception("El tipo no existe.");


                    db.hdTIPO.Remove(_tipo);
                    db.SaveChanges();

                    return "Registro eliminado ok.";

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        // POST api/values -- LISTADO DE REGLAS SLA.
        [AcceptVerbs("POST")]
        [HttpPost()]
        [Route("getSLAsList")]
        public IEnumerable<object> getSLAsList([FromBody] PARAM oPARAM)
        {
            using (dbQuantusEntities db = new dbQuantusEntities())
            {
                try
                {
                    string _valor = oPARAM.valor == "" ? "0" : oPARAM.valor;

                    var _slas = db.hdSLA
                        .Where(x => x.IDCLIENTE == oPARAM.idcliente)
                        .Select(x => new { x.IDSLA, x.IDCLIENTE, x.IDPRIORIDAD, x.RESOLVEREN, x.RESPONDEREN, x.ESTATUS })
                        .ToList();

                    /*
                    if (oUSUARIOS == null)
                        throw new Exception("El usuario no existe.");

                    if (oUSUARIOS.ESTATUS == "B")
                        throw new Exception("El usuario esta en estatus BAJA.");

                    if (_PASSW != oUSUARIOS.PASSW)
                        throw new Exception("La constraseña falló.");
                    */

                    return _slas;

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
        [Route("getSLAByID")]
        public object getSLAByID([FromBody] PARAM oPARAM)
        {
            using (dbQuantusEntities db = new dbQuantusEntities())
            {
                try
                {
                    string _valor = oPARAM.valor == "" ? "0" : oPARAM.valor;

                    var _sla = db.hdSLA
                        .Where(x => x.IDCLIENTE == oPARAM.idcliente && x.IDSLA.ToString() == _valor)
                        .Select(x => new { x.IDSLA, x.IDCLIENTE, x.IDPRIORIDAD, x.RESOLVEREN, x.RESPONDEREN, x.ESTATUS })
                        .FirstOrDefault();

                    return _sla;

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        // POST api/values -- INGRESA REGLA SLA.
        [AcceptVerbs("POST")]
        [HttpPost()]
        [Route("insSLA")]
        public string insSLA([FromBody] hdSLA oSLA)
        {
            using (dbQuantusEntities db = new dbQuantusEntities())
            {
                try
                {

                    var _sla = db.hdSLA
                        .Where(x => x.IDSLA == oSLA.IDSLA && x.IDCLIENTE == oSLA.IDCLIENTE)
                        .SingleOrDefault();


                    if (_sla != null)
                        throw new Exception("El SLA YA existe.");

                    db.hdSLA.Add(oSLA);
                    db.SaveChanges();

                    return "Registro ingresado ok.";

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        // POST api/values -- ACTUALIZA REGLA SLA.
        [AcceptVerbs("POST")]
        [HttpPost()]
        [Route("updSLA")]
        public string updSLA([FromBody] hdSLA oSLA)
        {
            using (dbQuantusEntities db = new dbQuantusEntities())
            {
                try
                {

                    var _sla = db.hdSLA
                        .Where(x => x.IDSLA == oSLA.IDSLA && x.IDCLIENTE == oSLA.IDCLIENTE)
                        .SingleOrDefault();


                    if (_sla == null)
                        throw new Exception("el SLA no existe.");


                    _sla.RESPONDEREN = oSLA.RESPONDEREN;
                    _sla.RESOLVEREN = oSLA.RESOLVEREN;
                    _sla.ESTATUS = oSLA.ESTATUS;
                    db.SaveChanges();

                    return "Registro actualizado ok.";

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        #region TICKETS

        // POST api/values -- GET ID NEW TICKET
        [AcceptVerbs("POST")]
        [HttpPost()]
        [Route("getNewIdTicket")]
        public string getNewIdTicket([FromBody] PARAM oPARAM)
        {
            using (dbQuantusEntities db = new dbQuantusEntities())
            {
                try
                {
                    int _idticket = 0;
                    string _valor = oPARAM.valor == "" ? "0" : oPARAM.valor;

                    hdPARAM _tickets = db.hdPARAM
                        .Where(x => x.IDCLIENTE == oPARAM.idcliente && x.IDPARAM == "idTicket")
                        .FirstOrDefault();

                    if (_tickets != null)
                        _idticket = Int32.Parse(_tickets.VALOR);

                    _idticket++;

                    string _numeroTicket = _idticket.ToString("INC00000");

                    return _numeroTicket;

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }


        // POST api/values -- LISTADO DE TICKETS.
        [AcceptVerbs("POST")]
        [HttpPost()]
        [Route("getTicketsList")]
        public IEnumerable<object> getTicketsList([FromBody] PARAM oPARAM)
        {
            using (dbQuantusEntities db = new dbQuantusEntities())
            {
                try
                {
                    string _valor = oPARAM.valor == "" ? "0" : oPARAM.valor;

                    var _tickets = db.hdTICKET
                        .Where(x => x.IDCLIENTE == oPARAM.idcliente && ((oPARAM.rol == "A") || (oPARAM.rol == "U" && x.IDUSUARIO == oPARAM.idusuario)))
                        .Select(x => new
                        {
                            x.IDTICKET,
                            x.IDCLIENTE,
                            x.IDPRIORIDAD,
                            x.IDTIPO,
                            x.IDUSUARIO,
                            x.hdUSUARIO.NOMUSUARIO,
                            x.ASUNTO,
                            x.DESCTICKET,
                            x.ESTATUS,
                            x.ASIGNADOA,
                            NOMASIGNADO = x.hdUSUARIO1.NOMUSUARIO,
                            x.ORIGEN,
                            x.FECHA
                        })
                        .ToList();

                    return _tickets;

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        // POST api/values -- LISTADO DE TICKETS.
        [AcceptVerbs("POST")]
        [HttpPost()]
        [Route("getTicketByID")]
        public IEnumerable<object> getTicketByID([FromBody] PARAM oPARAM)
        {
            using (dbQuantusEntities db = new dbQuantusEntities())
            {
                try
                {
                    string _valor = oPARAM.valor == "" ? "0" : oPARAM.valor;

                    var _tickets = db.hdTICKET
                        .Where(x => x.IDCLIENTE == oPARAM.idcliente && x.IDTICKET == _valor && x.IDUSUARIO == oPARAM.idusuario)
                        .Select(x => new
                        {
                            x.IDTICKET,
                            x.IDCLIENTE,
                            x.IDPRIORIDAD,
                            x.IDTIPO,
                            x.IDUSUARIO,
                            x.hdUSUARIO.NOMUSUARIO,
                            x.ASUNTO,
                            x.DESCTICKET,
                            x.ESTATUS,
                            x.ASIGNADOA,
                            NOMASIGNADO = x.hdUSUARIO1.NOMUSUARIO,
                            x.ORIGEN,
                            x.FECHA
                        })
                        .ToList();

                    return _tickets;

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }


        // POST api/values -- INGRESA TICKET.
        [AcceptVerbs("POST")]
        [HttpPost()]
        [Route("insTICKET")]
        public string insTICKET([FromBody] hdTICKET oTICKET)
        {
            using (dbQuantusEntities db = new dbQuantusEntities())
            {
                try
                {

                    var _sla = db.hdTICKET
                        .Where(x => x.IDTICKET == oTICKET.IDTICKET && x.IDCLIENTE == oTICKET.IDCLIENTE)
                        .SingleOrDefault();


                    if (_sla != null)
                        throw new Exception("El Ticket YA existe.");

                    db.hdTICKET.Add(oTICKET);
                    db.SaveChanges();

                    return "Registro ingresado ok.";

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }


        // POST api/values -- ACTUALIZA TICKET.
        [AcceptVerbs("POST")]
        [HttpPost()]
        [Route("updTicket")]
        public string updTicket([FromBody] hdTICKET oTICKET)
        {
            using (dbQuantusEntities db = new dbQuantusEntities())
            {
                try
                {

                    var _ticket = db.hdTICKET
                        .Where(x => x.IDTICKET == oTICKET.IDTICKET && x.IDCLIENTE == oTICKET.IDCLIENTE)
                        .SingleOrDefault();


                    if (_ticket == null)
                        throw new Exception("el Ticket no existe.");


                    _ticket.ESTATUS = oTICKET.ESTATUS;
                    _ticket.ASIGNADOA = oTICKET.ASIGNADOA;
                    _ticket.IDPRIORIDAD = oTICKET.IDPRIORIDAD;

                    db.SaveChanges();

                    return "Registro actualizado ok.";

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        // POST api/values -- ELIMINA TIPO.
        [AcceptVerbs("POST")]
        [HttpPost()]
        [Route("delTicket")]
        public string delTicket([FromBody] hdTICKET oTICKET)
        {
            using (dbQuantusEntities db = new dbQuantusEntities())
            {
                try
                {

                    var _ticket = db.hdTICKET
                        .Where(x => x.IDTICKET == oTICKET.IDTICKET && x.IDCLIENTE == oTICKET.IDCLIENTE)
                        .SingleOrDefault();


                    if (_ticket == null)
                        throw new Exception("El Ticket no existe.");


                    db.hdTICKET.Remove(_ticket);
                    db.SaveChanges();

                    return "Registro eliminado ok.";

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }


        #endregion

        #region TICKET DETALLE

        // POST api/values -- LISTADO DE TICKETS DETALLE.
        [AcceptVerbs("POST")]
        [HttpPost()]
        [Route("getTicketDetList")]
        public IEnumerable<object> getTicketDetList([FromBody] PARAM oPARAM)
        {
            using (dbQuantusEntities db = new dbQuantusEntities())
            {
                try
                {
                    string _valor = oPARAM.valor == "" ? "0" : oPARAM.valor;

                    var _ticketsDet = db.hdTICKETDET
                        .Where(x => x.IDCLIENTE == oPARAM.idcliente && x.IDTICKET == _valor)
                        .Select(x => new { x.IDTICKETDET, x.IDTICKET, x.IDCLIENTE, x.IDUSUARIO, x.DESCTICKETDET, x.FECHA })
                        .ToList();

                    return _ticketsDet;

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }


        // POST api/values -- INGRESA TICKET.
        [AcceptVerbs("POST")]
        [HttpPost()]
        [Route("insTicketDet")]
        public string insTicketDet([FromBody] hdTICKETDET oTICKETDET)
        {
            using (dbQuantusEntities db = new dbQuantusEntities())
            {
                try
                {

                    var _ticketdet = db.hdTICKETDET
                        .Where(x => x.IDTICKET == oTICKETDET.IDTICKET && x.IDCLIENTE == oTICKETDET.IDCLIENTE)
                        .SingleOrDefault();


                    if (_ticketdet != null)
                        throw new Exception("El Ticket YA existe.");

                    db.hdTICKETDET.Add(oTICKETDET);
                    db.SaveChanges();

                    return "Registro ingresado ok.";

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }




        #endregion

    }
}
