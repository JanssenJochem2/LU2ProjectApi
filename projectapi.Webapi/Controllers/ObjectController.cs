using System.Net;
using System.Reflection;
using Azure.Core;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using projectapi.Webapi.Interfaces;
using projectapi.Webapi.Models;
using projectapi.Webapi.Repositories;

namespace projectapi.Webapi.Controllers;

[ApiController]
[Route("[controller]")]

public class ObjectController : ControllerBase
{

    private readonly ILogger<ObjectController> _logger;
    private readonly IObjectRepository _objectRepository;
    private readonly IAuthenticationService _auth;

    public ObjectController(ILogger<ObjectController> logger, IObjectRepository objectRepository, IAuthenticationService Auth)
    {
        _logger = logger;
        _objectRepository = objectRepository;
        _auth = Auth;
    }

    [HttpPost("GetWorlds")]
    public async Task<ActionResult> GetWorldsByUser()
    {
        Guid User_id = new Guid(_auth.GetCurrentAuthenticatedUserId());

        var result = await _objectRepository.GetWorlds(User_id);

        if (result == null || !result.Any())
        {
            return NotFound($"No worlds found for user with ID: {User_id}");
        }

        return Ok(result);
    }


    [HttpPost("AddWorlds")]
    public async Task<ActionResult> AddWorlds(Models.World2 objectData)
    {
        Guid User_id = new Guid(_auth.GetCurrentAuthenticatedUserId());

     
        if (string.IsNullOrWhiteSpace(objectData.WorldName) || objectData.WorldName.Length < 1 || objectData.WorldName.Length > 25)
        {
            return BadRequest("De naam van de wereld moet tussen de 1 en 25 karakters lang zijn.");
        }

        var userWorlds = await _objectRepository.GetWorlds(User_id);

        if (userWorlds.Any(w => w.WorldName.Equals(objectData.WorldName, StringComparison.OrdinalIgnoreCase)))
        {
            return BadRequest("Je hebt al een wereld met deze naam. Kies een andere naam.");
        }

        if (userWorlds.Count() >= 5)
        {
            return BadRequest("Je kunt niet meer dan 5 werelden hebben.");
        }

        var result = await _objectRepository.AddWorlds(User_id, objectData.WorldName, objectData.Width, objectData.Height);

        if (result == null)
        {
            return NotFound($"No objects found in world with name: {objectData.WorldName}");
        }

        return Ok(result);
    }



    [HttpPost("RemoveWorld")]
    public ActionResult RemoveWorld(Models.RemoveWorld objectData) 
    {
        var result = _objectRepository.DeleteWorld(objectData.WorldId).Result;
        if (result == null)
        {
            return Ok(result);
        }

        return NotFound($"No objectdata found with ID: {objectData.WorldId}");
    }

    //Objects
    [HttpPost("LoadAllObjects")]
    public ActionResult LoadAllObjects(Models.ReadAllObjectsRequest2 objectData)
    {
        var result = _objectRepository.ReadObjectsAsync(objectData.WorldId).Result;

        if (result == null || result.Count == 0)
        {
            return NotFound($"No objects found in world with ID: {objectData.WorldId}");
        }

        return Ok(result);
    }


    [HttpPost("LoadObjectData")]
    public ActionResult LoadObjectData(Models.ReadObjectsRequest objectData)
    {
        var result = _objectRepository.ReadObjectDataAsync(objectData.ObjectId).Result;

        if (result == null)
        {
            return NotFound($"No objectdata found with ID: {objectData.ObjectId}");
        }

        return Ok(result);
    }

    [HttpPost("AddObject")]
    public async Task<IActionResult> AddObject(Models.InsertObjectRequest objectData)
    {
        var newObject = await _objectRepository.InsertObjectAsync(
            objectData.WorldId, objectData.PrefabId, objectData.PositionX, objectData.PositionY,
            objectData.ScaleX, objectData.ScaleY, objectData.RotationZ, objectData.LayerZ
        );

        return Ok( new
        {
            message = $"Placed object"
        });
    }

    [HttpPost("ReplaceObject")]
    public async Task<IActionResult> ReplaceObject(Models.UpdateObjectRequest objectData)
    {
        if (objectData == null)
            return BadRequest("Invalid object data.");

        var updatedObject = await _objectRepository.UpdateObjectsAsync(
            objectData.ObjectId, objectData.PrefabId, objectData.PositionX, objectData.PositionY,
            objectData.ScaleX, objectData.ScaleY, objectData.RotationZ, objectData.LayerZ
        );

        return Ok(new
        {
            message = "Replaced"
        });
    }

    [HttpPost("RemoveObject")]
     public async Task<Models.DeleteObjectRequest?> RemoveObject(Models.DeleteObjectRequest objectData)
    {
        return await _objectRepository.DeleteObjectsAsync(new DeleteObjectRequest { ObjectId = objectData.ObjectId });
    }
}