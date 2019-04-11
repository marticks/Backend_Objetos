using Microsoft.AspNetCore.Mvc;
using WebApiObjetos.Data;
using WebApiObjetos.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiObjetos.Controllers
{

    // Modelstate.IsValid chequea que lo que le mandes en el body matchee la clase a la que lo estas bindeando.
    //throw new InvalidOperationException("invalido")
    //los metodos deberían retornar actionResult así podes retornar el código y eso (200 ej) y en el "body" retornas lo que quieras, que sería lo que retornas comunmente en los métodos
    //BadRequest(ModelState)/// CreatedAtAction(texto, podría ser como acceder el nuevo recurso get/location/32 ej) /// OK()

    [Route("api/[controller]")]
    [ApiController]
    public class LocationController : Controller
    {
        private ILocationService locationsService;


        public LocationController(ILocationService locationsService)
        {
            this.locationsService = locationsService;
        }

        // GET api/location
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "location papa" };
        }

        [HttpPost]
        public ActionResult<string> Post()
        {
            return Ok("location papa post");
        }

    }
}
