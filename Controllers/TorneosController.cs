// Usings necesarios
using DBExamen3.Clases;  // Namespace de clsTorneo
using DBExamen3.Models;  // Namespace de Torneo (usado en Insertar/Actualizar)
using System;
using System.Collections.Generic; // Para List<>
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description; 
namespace DBExamen3.Controllers
{
    [RoutePrefix("api/torneos")] 
    [Authorize]                 
    public class TorneosController : ApiController
    {
        private clsTorneo _clsTorneo = new clsTorneo();
        [HttpGet]
        [Route("ConsultarTodos")]
        [ResponseType(typeof(List<object>))]
        public IHttpActionResult ConsultarTodos()
        {
            try
            {
                List<object> torneos = _clsTorneo.ConsultarTodos();
                return Ok(torneos); 
            }
            catch (Exception ex)
            {
                return InternalServerError(new Exception("Error al consultar todos los torneos.", ex));
            }
        }


        [HttpGet]
        [Route("ConsultarFiltrado")]
        [ResponseType(typeof(List<object>))]
        public IHttpActionResult ConsultarFiltrado([FromUri] string tipoTorneo = null, [FromUri] string nombreTorneo = null, [FromUri] DateTime? fechaTorneo = null)
        {
            try
            {
                List<object> torneos = _clsTorneo.ConsultarFiltrado(tipoTorneo, nombreTorneo, fechaTorneo);
                return Ok(torneos); 
            }
            catch (Exception ex)
            {
                return InternalServerError(new Exception("Error al consultar torneos con filtro.", ex));
            }
        }
        [HttpPost]
        [Route("Insertar")]
        [ResponseType(typeof(string))]
        public IHttpActionResult Insertar([FromBody] Torneo torneo)
        {
            if (torneo == null || !ModelState.IsValid)
            {
                return BadRequest(ModelState); 
            }

            try
            {
                string username = User.Identity.Name;
                if (string.IsNullOrEmpty(username))
                {
                    return Unauthorized(); 
                }
                int? adminId = _clsTorneo.ObtenerIdAdminPorUsuario(username);
                if (!adminId.HasValue)
                {
                    return BadRequest($"No se encontró administrador para el usuario '{username}'.");
                }
                string resultado = _clsTorneo.Insertar(torneo, adminId.Value);

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return InternalServerError(new Exception("Error al insertar el torneo.", ex));
            }
        }
        [HttpPut]
        [Route("Actualizar")]
        [ResponseType(typeof(string))]
        public IHttpActionResult Actualizar([FromBody] Torneo torneo)
        {
            if (torneo == null || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                string resultado = _clsTorneo.Actualizar(torneo);
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return InternalServerError(new Exception("Error al actualizar el torneo.", ex));
            }
        }
        [HttpDelete]
        [Route("Eliminar/{id}")] 
        [ResponseType(typeof(string))]
        public IHttpActionResult Eliminar(int id)
        {
            try
            {
                string resultado = _clsTorneo.Eliminar(id);
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return InternalServerError(new Exception($"Error al eliminar torneo con ID {id}.", ex));
            }
        }

    }
}