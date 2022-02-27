using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace DearlerPlatform.Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [EnableCors("any")]
    public class BaseController : ControllerBase
    {

    }
}