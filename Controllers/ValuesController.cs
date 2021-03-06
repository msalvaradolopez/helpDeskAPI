﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using helpDeskAPI.Models;
using helpDeskAPI.EnviarEmail;
using System.Data;

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

        // POST api/values -- ACTUALIZA SUCURSAL.
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
                        .Where(x => x.IDCLIENTE == oPARAM.idcliente && (x.ROL == oPARAM.rol || oPARAM.rol == "0") && (x.NOMUSUARIO.Contains(_valor) || _valor == "0"))
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
                        .Select(x => new { x.IDCLIENTE, x.IDPRIORIDAD, x.RESOLVEREN, x.RESPONDEREN, x.ESTATUS })
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
                        .Where(x => x.IDCLIENTE == oPARAM.idcliente && x.IDPRIORIDAD.ToString() == _valor)
                        .Select(x => new { x.IDCLIENTE, x.IDPRIORIDAD, x.RESOLVEREN, x.RESPONDEREN, x.ESTATUS })
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
                        .Where(x => x.IDPRIORIDAD == oSLA.IDPRIORIDAD && x.IDCLIENTE == oSLA.IDCLIENTE)
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
                        .Where(x => x.IDPRIORIDAD == oSLA.IDPRIORIDAD && x.IDCLIENTE == oSLA.IDCLIENTE)
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


        public string getNewIdTicket(int idcliente)
        {
            using (dbQuantusEntities db = new dbQuantusEntities())
            {
                try
                {
                    int _idticket = 0;

                    hdPARAM _tickets = db.hdPARAM
                        .Where(x => x.IDCLIENTE == idcliente && x.IDPARAM == "idTicket")
                        .FirstOrDefault();

                    if (_tickets != null)
                        _idticket = Int32.Parse(_tickets.VALOR);

                    _idticket++;

                    _tickets.VALOR = _idticket.ToString();

                    db.SaveChanges();

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
                    DateTime _fechaActual = DateTime.Now;
                    string _valor = oPARAM.valor == "" ? "0" : oPARAM.valor;

                    // List<hdTICKET> tickets = new List<hdTICKET>();

                    var _tickets = db.hdTICKET
                        .Where(x => x.IDCLIENTE == oPARAM.idcliente && ((oPARAM.rol == "A" || oPARAM.rol == "S") || (oPARAM.rol == "U" && x.IDUSUARIO == oPARAM.idusuario)))
                        .OrderByDescending(x => new { x.FECHA, x.IDTICKET })
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
                            x.FECHA,
                            x.hdSLA.RESOLVEREN,
                            SLA = "",
                            x.FECHACIERRE
                        }).ToList();

                    List<object> tickets = new List<object>();

                    foreach (var x in _tickets)
                    {
                        string _resp = "";
                        double xTotalHoras = 0;
                        DateTime xFecha = x.FECHA.GetValueOrDefault();

                        if(x.ESTATUS == "C")
                            _fechaActual = x.FECHACIERRE.GetValueOrDefault();
                        
                        xTotalHoras = _fechaActual.Subtract(xFecha).TotalHours;

                        if (xTotalHoras < x.RESOLVEREN)
                            _resp = "EN TIEMPO";
                        else
                            _resp = "ATRASADO";

                        tickets.Add(
                            new
                            {
                                x.IDTICKET,
                                x.IDCLIENTE,
                                x.IDPRIORIDAD,
                                x.IDTIPO,
                                x.IDUSUARIO,
                                x.NOMUSUARIO,
                                x.ASUNTO,
                                x.DESCTICKET,
                                x.ESTATUS,
                                x.ASIGNADOA,
                                x.NOMASIGNADO,
                                x.ORIGEN,
                                x.FECHA,
                                x.RESOLVEREN,
                                SLA = _resp,
                                x.FECHACIERRE
                            }
                            );
                    }

                    return tickets;

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
        public object getTicketByID([FromBody] PARAM oPARAM)
        {
            using (dbQuantusEntities db = new dbQuantusEntities())
            {
                try
                {
                    string _valor = oPARAM.valor == "" ? "0" : oPARAM.valor;

                    var _tickets = db.hdTICKET
                        .Where(x => x.IDCLIENTE == oPARAM.idcliente && x.IDTICKET == _valor)
                        .Select(x => new
                        {
                            x.IDTICKET,
                            x.IDCLIENTE,
                            x.IDPRIORIDAD,
                            x.IDTIPO,
                            x.hdTIPO.NOMTIPO,
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
                        .FirstOrDefault();

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
                    int _idticket = 0;
                    DateTime _fechaActual = DateTime.Now;
                    //string _fechaApp = oTICKET.FECHA.ToString();
                    //string[] _fechaSplit = _fechaApp.Split(new string[] { "/" }, StringSplitOptions.None);
                    //DateTime _fechaSistema = new DateTime(Int32.Parse(_fechaSplit[2].Substring(0, 4)), Int32.Parse(_fechaSplit[1]), Int32.Parse(_fechaSplit[0]));

                    // GENERA NUEVO TICKET
                    hdPARAM _PARAM = db.hdPARAM
                       .Where(x => x.IDCLIENTE == oTICKET.IDCLIENTE && x.IDPARAM == "idTicket")
                       .FirstOrDefault();

                    if (_PARAM != null)
                        _idticket = Int32.Parse(_PARAM.VALOR);

                    _idticket++;

                    _PARAM.VALOR = _idticket.ToString();

                    string _numeroTicket = _idticket.ToString("INC00000");

                    var _ticket = db.hdTICKET
                        .Where(x => x.IDTICKET == _numeroTicket && x.IDCLIENTE == oTICKET.IDCLIENTE)
                        .SingleOrDefault();


                    if (_ticket != null)
                        throw new Exception("El Ticket YA existe.");

                    oTICKET.IDTICKET = _numeroTicket;
                    oTICKET.FECHA = _fechaActual;

                    db.hdTICKET.Add(oTICKET);
                    db.SaveChanges();

                    return _numeroTicket;

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
                    DateTime _fechaActual = DateTime.Now;

                    var _ticket = db.hdTICKET
                        .Where(x => x.IDTICKET == oTICKET.IDTICKET && x.IDCLIENTE == oTICKET.IDCLIENTE)
                        .SingleOrDefault();


                    if (_ticket == null)
                        throw new Exception("el Ticket no existe.");


                    _ticket.ESTATUS = oTICKET.ESTATUS;
                    _ticket.ASIGNADOA = oTICKET.ASIGNADOA == null ? _ticket.ASIGNADOA : oTICKET.ASIGNADOA;
                    _ticket.IDPRIORIDAD = oTICKET.IDPRIORIDAD == 0 ? _ticket.IDPRIORIDAD : oTICKET.IDPRIORIDAD;
                    _ticket.ORIGEN = oTICKET.ORIGEN;


                    if (_ticket.ESTATUS == "C")
                        _ticket.FECHACIERRE = _fechaActual;

                    
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
                        .OrderByDescending(x => x.IDTICKETDET)
                        .Select(x => new { x.IDTICKETDET, x.IDTICKET, x.IDCLIENTE, x.IDUSUARIO, x.DESCTICKETDET, x.FECHA, x.hdTICKET.hdUSUARIO.NOMUSUARIO })
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
                using (var dbTranstaction = db.Database.BeginTransaction())
                {

                    try
                    {
                        string _fechaApp = oTICKETDET.FECHA.ToString();
                        // string[] _fechaSplit = _fechaApp.Split(new string[] { "/" }, StringSplitOptions.None);
                        // DateTime _fechaSistema = new DateTime(Int32.Parse(_fechaSplit[2].Substring(0, 4)), Int32.Parse(_fechaSplit[1]), Int32.Parse(_fechaSplit[0]));


                        // oTICKETDET.FECHA = _fechaSistema;
                        db.hdTICKETDET.Add(oTICKETDET);
                        db.SaveChanges();

                        var _ticket = db.hdTICKET.Where(x => x.IDCLIENTE == oTICKETDET.IDCLIENTE && x.IDTICKET == oTICKETDET.IDTICKET)
                            .Select(x => new { x.IDUSUARIO, x.hdUSUARIO.NOMUSUARIO, x.ASUNTO, x.ESTATUS, x.hdUSUARIO.EMAIL, x.ASIGNADOA, NOMASGINADOA = x.hdUSUARIO1.NOMUSUARIO, x.FECHA })
                            .FirstOrDefault();

                        string _asunto = _ticket.ASUNTO;
                        string _descticket = oTICKETDET.DESCTICKETDET;
                        string _nomusuario = _ticket.NOMUSUARIO;
                        string _usuarioemail = _ticket.EMAIL;
                        string _nomasignado = _ticket.NOMASGINADOA;
                        string _estatus = _ticket.ESTATUS;
                        string _fecha = (DateTime.Parse(_ticket.FECHA.ToString())).ToString("dd/MM/yyyy h:mm");

                        if (_estatus == "O")
                            _estatus = "ABIERTO";
                        if (_estatus == "A")
                            _estatus = "ASIGNADO";
                        if (_estatus == "R")
                            _estatus = "RE-ABIERTO";
                        if (_estatus == "C")
                            _estatus = "CERRADO";

                        string mensaje = @"<html>
                                        <body>
                                        <p> Esimado(a) usuario :" + _nomusuario + ",</p> " +
                                                " <p>" + "Reciba la notificaión del ticket #" + oTICKETDET.IDTICKET +
                                                        " con estatus actual : " + _estatus +
                                                        " fecha generación :" + _fecha + " </p> " +
                                                        " <p>" + "Descripción: " + oTICKETDET.DESCTICKETDET + " </p> ";


                        sendEmail _email = new sendEmail();

                        _email.mandarEmail(_usuarioemail, "HELPDESK : " + _asunto, mensaje);

                        var _usuarios = db.hdUSUARIO
                            .Where(x => x.IDCLIENTE == oTICKETDET.IDCLIENTE && x.ROL == "A")
                            .Select(x => new { x.NOMUSUARIO, x.EMAIL }).ToList();

                        foreach (var item in _usuarios)
                        {
                            _nomusuario = item.NOMUSUARIO;
                            _usuarioemail = item.EMAIL;

                            mensaje = @"<html>
                                        <body>
                                        <p> Esimado(a) usuario :" + _nomusuario + ",</p> " +
                                                " <p>" + "Reciba la notificaión del ticket #" + oTICKETDET.IDTICKET +
                                                        " con estatus actual : " + _estatus +
                                                        " fecha generación :" + _fecha + " </p> " +
                                                        " <p>" + "Descripción: " + oTICKETDET.DESCTICKETDET + " </p> ";

                            _email.mandarEmail(_usuarioemail, "HELPDESK : " + _asunto, mensaje);
                        }

                        dbTranstaction.Commit();
                        return "Registro ingresado ok.";

                    }
                    catch (Exception ex)
                    {
                        dbTranstaction.Rollback();
                        throw ex;
                    }
                }
            }
        }
        #endregion

        // POST api/values -- LISTADO DE PARAMETROS.
        [AcceptVerbs("POST")]
        [HttpPost()]
        [Route("getParamList")]
        public IEnumerable<object> getParamList([FromBody] PARAM oPARAM)
        {
            using (dbQuantusEntities db = new dbQuantusEntities())
            {
                try
                {
                    string _valor = oPARAM.valor == "" ? "0" : oPARAM.valor;

                    var _param = db.hdPARAM
                        .Where(x => x.IDCLIENTE == oPARAM.idcliente)
                        .Select(x => new { x.IDPARAM, x.IDCLIENTE, x.DESCPARAM, x.VALOR })
                        .ToList();

                    return _param;

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
        [Route("getParamByID")]
        public object getParamByID([FromBody] PARAM oPARAM)
        {
            using (dbQuantusEntities db = new dbQuantusEntities())
            {
                try
                {
                    string _valor = oPARAM.valor == "" ? "0" : oPARAM.valor;

                    var _param = db.hdPARAM
                        .Where(x => x.IDCLIENTE == oPARAM.idcliente && x.IDPARAM == _valor)
                        .Select(x => new { x.IDPARAM, x.IDCLIENTE, x.DESCPARAM, x.VALOR })
                        .FirstOrDefault();

                    return _param;

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }


        // POST api/values -- ACTUALIZA PARAMETROS.
        [AcceptVerbs("POST")]
        [HttpPost()]
        [Route("updParam")]
        public string updParam([FromBody] hdPARAM oPARAM)
        {
            using (dbQuantusEntities db = new dbQuantusEntities())
            {
                try
                {

                    var _param = db.hdPARAM
                        .Where(x => x.IDCLIENTE == oPARAM.IDCLIENTE && x.IDPARAM == oPARAM.IDPARAM)
                        .SingleOrDefault();


                    if (_param == null)
                        throw new Exception("El parametro no existe.");


                    _param.VALOR = oPARAM.VALOR;
                    db.SaveChanges();

                    return "Registro actualizado ok.";

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }


        // POST api/values -- LISTADO DE PARAMETROS.
        [AcceptVerbs("POST")]
        [HttpPost()]
        [Route("getDashBoardIndicadores")]
        public object getDashBoardIndicadores([FromBody] PARAM oPARAM)
        {
            using (dbQuantusEntities db = new dbQuantusEntities())
            {
                try
                {
                    string _valor = oPARAM.valor == "" ? "0" : oPARAM.valor;

                    var _tickets = db.hdTICKET
                        .Where(x => x.IDCLIENTE == oPARAM.idcliente &&
                        ((x.ESTATUS == "O" || x.ESTATUS == "A") || _valor == "0") &&
                        oPARAM.sucursales.Any(t2 => t2.Contains(x.hdUSUARIO.IDSUCURSAL)) &&
                        oPARAM.temas.Any(t3 => t3.Contains(x.IDTIPO.ToString())))
                        .Select(x => new { x.ESTATUS, x.hdSLA.RESOLVEREN, x.FECHA })
                        .ToList();

                    int _total = 0;
                    int _sinasignar = 0;
                    int _atrasados = 0;
                    int _reabiertos = 0;
                    DateTime _fechaActual = DateTime.Now;

                    foreach (var item in _tickets)
                    {
                        _total++;
                        if (item.ESTATUS == "O") _sinasignar++;
                        if (item.ESTATUS == "R") _reabiertos++;
                        if (item.ESTATUS != "C")
                        {
                            string _resp = "";
                            DateTime xFecha = item.FECHA.GetValueOrDefault();
                            double xTotalHoras = _fechaActual.Subtract(xFecha).TotalHours;
                            if (xTotalHoras > item.RESOLVEREN) _atrasados++;

                        }
                    }

                    var _indicadores = new { TOTAL = _total, SINASIGNAR = _sinasignar, ATRASADOS = _atrasados, REABIERTOS = _reabiertos };

                    return _indicadores;

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        // POST api/values -- LISTADO DE ESTADISTICA USUARIO QUIEN OTORGAN EL SERVICIO.
        [AcceptVerbs("POST")]
        [HttpPost()]
        [Route("getIndicadorByUser")]
        public IEnumerable<object> getIndicadorByUser([FromBody] PARAM oPARAM)
        {
            using (dbQuantusEntities db = new dbQuantusEntities())
            {
                try
                {
                    string _valor = oPARAM.valor == "" ? "0" : oPARAM.valor;

                    string IDUSUARIO = "";
                    string NOMUSUARIO = "";
                    int _total = 0;
                    int _abiertos = 0;
                    int _atrasados = 0;
                    int _reabiertos = 0;
                    int _cerrados = 0;
                    DateTime _fechaActual = DateTime.Now;

                    var _usuarios = db.hdUSUARIO
                        .Where(x => x.IDCLIENTE == oPARAM.idcliente && x.ESTATUS == "A" && x.ROL != "S")
                        .ToList();

                    List<object> indByUser = new List<object>();

                    foreach (hdUSUARIO usuario in _usuarios)
                    {


                        if (usuario.ROL == "A")
                        {
                            IDUSUARIO = "";
                            NOMUSUARIO = "";
                            _total = 0;
                            _abiertos = 0;
                            _atrasados = 0;
                            _reabiertos = 0;
                            _cerrados = 0;

                            var _tickets = db.hdTICKET
                        .Where(x => x.IDCLIENTE == usuario.IDCLIENTE && x.ASIGNADOA == usuario.IDUSUARIO)
                        .Select(x => new { x.ESTATUS, x.hdSLA.RESOLVEREN, x.FECHA, x.ASIGNADOA, NOMASIGNADO = x.hdUSUARIO1.NOMUSUARIO, x.hdUSUARIO.NOMUSUARIO })
                        .ToList();

                            foreach (var item in _tickets)
                            {
                                IDUSUARIO = item.ASIGNADOA;
                                NOMUSUARIO = item.NOMASIGNADO;
                                _total++;
                                if (item.ESTATUS == "O") _abiertos++;
                                if (item.ESTATUS == "R") _reabiertos++;
                                if (item.ESTATUS != "C")
                                {
                                    DateTime xFecha = item.FECHA.GetValueOrDefault();
                                    double xTotalHoras = _fechaActual.Subtract(xFecha).TotalHours;
                                    if (xTotalHoras > item.RESOLVEREN) _atrasados++;
                                }
                                if (item.ESTATUS == "C") _cerrados++;

                            }

                            if (_total > 0) indByUser.Add(new { IDUSUARIO = IDUSUARIO, NOMUSUARIO = NOMUSUARIO, usuario.ROL, TOTAL = _total, ABIERTOS = _abiertos, ATRASADOS = _atrasados, REABIERTOS = _reabiertos, CERRADOS = _cerrados });
                        }
                        else if (usuario.ROL == "U")
                        {
                            IDUSUARIO = "";
                            NOMUSUARIO = "";
                            _total = 0;
                            _abiertos = 0;
                            _atrasados = 0;
                            _reabiertos = 0;
                            _cerrados = 0;

                            var _tickets = db.hdTICKET
                        .Where(x => x.IDCLIENTE == usuario.IDCLIENTE && x.IDUSUARIO == usuario.IDUSUARIO)
                        .Select(x => new { x.ESTATUS, x.hdSLA.RESOLVEREN, x.FECHA, x.ASIGNADOA, NOMASIGNADO = x.hdUSUARIO1.NOMUSUARIO, x.hdUSUARIO.NOMUSUARIO, x.IDUSUARIO })
                        .ToList();

                            foreach (var item in _tickets)
                            {
                                IDUSUARIO = item.IDUSUARIO;
                                NOMUSUARIO = item.NOMUSUARIO;
                                _total++;
                                if (item.ESTATUS == "O") _abiertos++;
                                if (item.ESTATUS == "R") _reabiertos++;
                                if (item.ESTATUS != "C")
                                {
                                    DateTime xFecha = item.FECHA.GetValueOrDefault();
                                    double xTotalHoras = _fechaActual.Subtract(xFecha).TotalHours;
                                    if (xTotalHoras > item.RESOLVEREN) _atrasados++;
                                }
                                if (item.ESTATUS == "C") _cerrados++;


                            }
                            if (_total > 0) indByUser.Add(new { IDUSUARIO = IDUSUARIO, NOMUSUARIO = NOMUSUARIO, usuario.ROL, TOTAL = _total, ABIERTOS = _abiertos, ATRASADOS = _atrasados, REABIERTOS = _reabiertos, CERRADOS = _cerrados });
                        }

                    }

                    return indByUser;

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }


        // estrucutura auxiliar
        private class gpoTemas
        {
            public string NOMTIPO { get; set; }
            public int CTDTEMA { get; set; }
            public List<gpoSuc> gpoSuc { get; set; }
            public gpoTemas()
            {
                gpoSuc = new List<gpoSuc>();
            }
        }

        private class gpoSuc
        {
            public string NOMSUCURSAL { get; set; }
            public int CTDSUCURSAL { get; set; }
            public List<gpoTemas> gpoTemas { get; set; }
            public gpoSuc()
            {
                gpoTemas = new List<gpoTemas>();
            }
        }

        // POST api/values -- obtener indicadores por sucursal
        [AcceptVerbs("POST")]
        [HttpPost()]
        [Route("getGrupoSucursales")]
        public IEnumerable<object> getGrupoSucursales([FromBody] PARAM oPARAM)
        {
            using (dbQuantusEntities db = new dbQuantusEntities())
            {
                try
                {
                    string _valor = oPARAM.valor == "" ? "0" : oPARAM.valor;
                    List<gpoSuc> gpoSuc = new List<gpoSuc>();

                    var _grupoSucursales = db.hdTICKET
                        .Where(x => x.IDCLIENTE == oPARAM.idcliente &&
                        ((x.ESTATUS == "O" || x.ESTATUS == "A") || _valor == "0") &&
                        oPARAM.sucursales.Any(t2 => t2.Contains(x.hdUSUARIO.IDSUCURSAL)) &&
                        oPARAM.temas.Any(t3 => t3.Contains(x.IDTIPO.ToString())))
                        .GroupBy(x => x.hdUSUARIO.hdSUCURSAL.NOMSUCURSAL)
                        .Select(x => new { nomSucursal = x.Key, ctdSucursal = x.Count() })
                        .ToList();

                    foreach (var item in _grupoSucursales)
                    {

                        var _grupoTemas = db.hdTICKET
                            .Where(x => x.hdUSUARIO.hdSUCURSAL.NOMSUCURSAL == item.nomSucursal &&
                            x.IDCLIENTE == oPARAM.idcliente &&
                        ((x.ESTATUS == "O" || x.ESTATUS == "A") || _valor == "0") &&
                        oPARAM.sucursales.Any(t2 => t2.Contains(x.hdUSUARIO.IDSUCURSAL)) &&
                        oPARAM.temas.Any(t3 => t3.Contains(x.IDTIPO.ToString())))
                            .GroupBy(x => new { x.hdUSUARIO.hdSUCURSAL.NOMSUCURSAL, x.hdTIPO.NOMTIPO })
                            .Select(x => new { x.Key.NOMSUCURSAL, x.Key.NOMTIPO, CTDTEMA = x.Count() })
                            .ToList();

                        var _suc = new gpoSuc();

                        _suc.NOMSUCURSAL = item.nomSucursal;
                        _suc.CTDSUCURSAL = item.ctdSucursal;

                        foreach (var itemTema in _grupoTemas)
                        {
                            var _gpo = new gpoTemas();
                            _gpo.NOMTIPO = itemTema.NOMTIPO;
                            _gpo.CTDTEMA = itemTema.CTDTEMA;
                            _suc.gpoTemas.Add(_gpo);
                        }

                        gpoSuc.Add(_suc);
                    }

                    return gpoSuc;

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        // POST api/values -- obtener indicadores por sucursal
        [AcceptVerbs("POST")]
        [HttpPost()]
        [Route("getGrupoTemas")]
        public IEnumerable<object> getGrupoTemas([FromBody] PARAM oPARAM)
        {
            using (dbQuantusEntities db = new dbQuantusEntities())
            {
                try
                {
                    string _valor = oPARAM.valor == "" ? "0" : oPARAM.valor;
                    List<gpoTemas> _gpoTemas = new List<gpoTemas>();

                    var _grupoTemas = db.hdTICKET
                            .Where(x => x.IDCLIENTE == oPARAM.idcliente &&
                            ((x.ESTATUS == "O" || x.ESTATUS == "A") || _valor == "0") &&
                            oPARAM.sucursales.Any(t2 => t2.Contains(x.hdUSUARIO.IDSUCURSAL)) &&
                            oPARAM.temas.Any(t3 => t3.Contains(x.IDTIPO.ToString())))
                            .GroupBy(x => new { x.hdTIPO.IDTIPO, x.hdTIPO.NOMTIPO })
                            .Select(x => new { x.Key.IDTIPO, x.Key.NOMTIPO, CTDTEMA = x.Count() })
                            .ToList();

                    foreach (var item in _grupoTemas)
                    {
                        var _grupoSucursales = db.hdTICKET
                        .Where(x => x.hdTIPO.IDTIPO == item.IDTIPO &&
                            x.IDCLIENTE == oPARAM.idcliente &&
                            ((x.ESTATUS == "O" || x.ESTATUS == "A") || _valor == "0") &&
                            oPARAM.sucursales.Any(t2 => t2.Contains(x.hdUSUARIO.IDSUCURSAL)) &&
                            oPARAM.temas.Any(t3 => t3.Contains(x.IDTIPO.ToString())))
                        .GroupBy(x => new { x.hdUSUARIO.hdSUCURSAL.IDSUCURSAL, x.hdUSUARIO.hdSUCURSAL.NOMSUCURSAL })
                        .Select(x => new { x.Key.NOMSUCURSAL, CTDSUC = x.Count() })
                        .ToList();

                        var _temas = new gpoTemas();

                        _temas.NOMTIPO = item.NOMTIPO;
                        _temas.CTDTEMA = item.CTDTEMA;

                        foreach (var itemSuc in _grupoSucursales)
                        {
                            var _gpo = new gpoSuc();
                            _gpo.NOMSUCURSAL = itemSuc.NOMSUCURSAL;
                            _gpo.CTDSUCURSAL = itemSuc.CTDSUC;
                            _temas.gpoSuc.Add(_gpo);
                        }

                        _gpoTemas.Add(_temas);
                    }

                    return _gpoTemas;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        // POST api/values -- LISTADO DE TICKETS DETALLE.
        [AcceptVerbs("POST")]
        [HttpPost()]
        [Route("getFiltroSucursales")]
        public IEnumerable<object> getFiltroSucursales([FromBody] PARAM oPARAM)
        {
            using (dbQuantusEntities db = new dbQuantusEntities())
            {
                try
                {
                    string _valor = oPARAM.valor == "" ? "0" : oPARAM.valor;

                    var _sucursales = db.hdTICKET
                        .Where(x => x.IDCLIENTE == oPARAM.idcliente && ((x.ESTATUS == "O" || x.ESTATUS == "A") || _valor == "0"))
                        .GroupBy(x => new { x.hdUSUARIO.IDSUCURSAL, x.hdUSUARIO.hdSUCURSAL.NOMSUCURSAL })
                        .OrderBy(x => x.Key.NOMSUCURSAL)
                        .Select(x => new { x.Key.NOMSUCURSAL, x.Key.IDSUCURSAL })
                        .ToList();

                    return _sucursales;

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        // POST api/values -- LISTADO DE TICKETS DETALLE.
        [AcceptVerbs("POST")]
        [HttpPost()]
        [Route("getFiltroTemas")]
        public IEnumerable<object> getFiltroTemas([FromBody] PARAM oPARAM)
        {
            using (dbQuantusEntities db = new dbQuantusEntities())
            {
                try
                {
                    string _valor = oPARAM.valor == "" ? "0" : oPARAM.valor;

                    var _temas = db.hdTICKET
                        .Where(x => x.IDCLIENTE == oPARAM.idcliente && ((x.ESTATUS == "O" || x.ESTATUS == "A") || _valor == "0"))
                        .GroupBy(x => new { x.hdTIPO.IDTIPO, x.hdTIPO.NOMTIPO })
                        .OrderBy(x => x.Key.NOMTIPO)
                        .Select(x => new { x.Key.NOMTIPO, x.Key.IDTIPO })
                        .ToList();

                    return _temas;

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
        [Route("getTicketsDashBoard")]
        public IEnumerable<object> getTicketsDashBoard([FromBody] PARAM oPARAM)
        {
            using (dbQuantusEntities db = new dbQuantusEntities())
            {
                try
                {
                    string _valor = oPARAM.valor == "" ? "0" : oPARAM.valor;

                    var _tickets = db.hdTICKET
                        .Where(x => x.IDCLIENTE == oPARAM.idcliente &&
                        ((x.ESTATUS == "O" || x.ESTATUS == "A") || _valor == "0") &&
                        oPARAM.sucursales.Any(t2 => t2.Contains(x.hdUSUARIO.IDSUCURSAL)) &&
                        oPARAM.temas.Any(t3 => t3.Contains(x.IDTIPO.ToString())))
                        .Select(x => new { x.IDTICKET, x.ASUNTO, x.hdUSUARIO.NOMUSUARIO, x.ESTATUS, x.FECHA, x.IDPRIORIDAD })
                        .ToList();

                    return _tickets;

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        // POST api/values -- obtener info para estatus de tickets por sucrusal y tema
        [AcceptVerbs("POST")]
        [HttpPost()]
        [Route("ticketsBySucTemaList")]
        public IEnumerable<object> ticketsBySucTemaList([FromBody] PARAM oPARAM)
        {
            using (dbQuantusEntities db = new dbQuantusEntities())
            {
                try
                {

                    int _valor = Int32.Parse(oPARAM.valor);
                    int _idcliente = oPARAM.idcliente;
                    string _sucursales = string.Join(",", oPARAM.sucursales);
                    string _temas = string.Join(",", oPARAM.temas);
                    string _estatus = "O,A,R";
                    int _rangoFechasSiNo = 0;
                    DateTime _fechaIni = DateTime.Now;
                    DateTime _fechaFin = DateTime.Now;

                    var _resp = db.ticketsBySucTemaList(_idcliente, _sucursales, _temas, _estatus, _rangoFechasSiNo, _fechaIni, _fechaFin).ToList();


                    return _resp;

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        // POST api/values -- obtener info para estatus de tickets por tema y sucrusal.
        [AcceptVerbs("POST")]
        [HttpPost()]
        [Route("ticketsByTemaSucList")]
        public IEnumerable<object> ticketsByTemaSucList([FromBody] PARAM oPARAM)
        {
            using (dbQuantusEntities db = new dbQuantusEntities())
            {
                try
                {

                    int _valor = Int32.Parse(oPARAM.valor);
                    int _idcliente = oPARAM.idcliente;
                    string _sucursales = string.Join(",", oPARAM.sucursales);
                    string _temas = string.Join(",", oPARAM.temas);
                    string _estatus = "O,A,R";
                    int _rangoFechasSiNo = 0;
                    DateTime _fechaIni = DateTime.Now;
                    DateTime _fechaFin = DateTime.Now;

                    var _resp = db.ticketsByTemaSucList(_idcliente, _sucursales, _temas, _estatus, _rangoFechasSiNo, _fechaIni, _fechaFin).ToList();


                    return _resp;

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        // POST api/values -- obtener info para estatus de tickets por tema y sucrusal.
        [AcceptVerbs("POST")]
        [HttpPost()]
        [Route("porcBySuc")]
        public IEnumerable<object> porcBySuc([FromBody] PARAM oPARAM)
        {
            using (dbQuantusEntities db = new dbQuantusEntities())
            {
                try
                {

                    int _valor = Int32.Parse(oPARAM.valor);
                    int _idcliente = oPARAM.idcliente;
                    string _sucursales = string.Join(",", oPARAM.sucursales);
                    string _temas = string.Join(",", oPARAM.temas);
                    string _estatus = "O,A,R,C";
                    int _rangoFechasSiNo = 0;
                    DateTime _fechaIni = DateTime.Now;
                    DateTime _fechaFin = DateTime.Now;

                    var _resp = db.porcBySuc(_idcliente, _sucursales, _temas, _estatus, _rangoFechasSiNo, _fechaIni, _fechaFin).ToList();


                    return _resp;

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        // POST api/values -- obtener info PORC temas.
        [AcceptVerbs("POST")]
        [HttpPost()]
        [Route("porcByTema")]
        public IEnumerable<object> porcByTema([FromBody] PARAM oPARAM)
        {
            using (dbQuantusEntities db = new dbQuantusEntities())
            {
                try
                {

                    int _valor = Int32.Parse(oPARAM.valor);
                    int _idcliente = oPARAM.idcliente;
                    string _sucursales = string.Join(",", oPARAM.sucursales);
                    string _temas = string.Join(",", oPARAM.temas);
                    string _estatus = "O,A,R,C";
                    int _rangoFechasSiNo = 0;
                    DateTime _fechaIni = DateTime.Now;
                    DateTime _fechaFin = DateTime.Now;

                    var _resp = db.porcByTema(_idcliente, _sucursales, _temas, _estatus, _rangoFechasSiNo, _fechaIni, _fechaFin).ToList();


                    return _resp;

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        // POST api/values -- obtener info para dashboard v2.0.
        [AcceptVerbs("POST")]
        [HttpPost()]
        [Route("getTicketsByDashBoard")]
        public IEnumerable<object> getTicketsByDashBoard([FromBody] PARAM oPARAM)
        {
            using (dbQuantusEntities db = new dbQuantusEntities())
            {
                try
                {

                    int _valor = Int32.Parse(oPARAM.valor);
                    int _idcliente = oPARAM.idcliente;
                    string _sucursales = string.Join(",", oPARAM.sucursales);
                    string _temas = string.Join(",", oPARAM.temas);
                    string _estatus = "O,A,R,C";
                    int _rangoFechasSiNo = 0;
                    DateTime _fechaIni = DateTime.Now;
                    DateTime _fechaFin = DateTime.Now;

                    var _resp = db.getTicketsByDashBoard(_idcliente, _sucursales, _temas, _estatus, _rangoFechasSiNo, _fechaIni, _fechaFin).ToList();


                    return _resp;

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
    }
}
