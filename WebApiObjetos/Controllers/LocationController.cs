using Microsoft.AspNetCore.Mvc;
using WebApiObjetos.Data;
using WebApiObjetos.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiObjetos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationController : Controller
    {
        private ILocationService locationsService;


        public LocationController(ILocationService locationsService)
        {
            this.locationsService = locationsService;
        }


    }
}
