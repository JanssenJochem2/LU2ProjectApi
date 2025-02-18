using Microsoft.AspNetCore.Mvc;
using projectapi.Webapi.Interfaces;

namespace projectapi.Webapi.Controllers;

[ApiController]

//[Route("[controller]")] is classname as route, [Route("blablabla")] is blablabla: as route localhost:7058/blablabla
[Route("[controller]")]

public class Object2DController : ControllerBase
{

    private readonly ILogger<Object2DController> _logger;
    private readonly IObject2DRepository object2DRepository;

    public Object2DController(ILogger<Object2DController> logger, IObject2DRepository object2DRepository)
    {
        _logger = logger;
        this.object2DRepository = object2DRepository;
    }

    [HttpGet(Name = "Object2D")]

    [HttpPost]
    public ActionResult Create(Object2D object2D)
    {
        return Created("", object2D);
    }
}