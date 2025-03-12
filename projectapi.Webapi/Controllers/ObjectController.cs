using System.Net;
using System.Reflection;
using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using projectapi.Webapi.Interfaces;
using projectapi.Webapi.Models;
using projectapi.Webapi.Repositories;

namespace projectapi.Webapi.Controllers;

[ApiController]
//[Route("[controller]")] is classname as route, [Route("blablabla")] is blablabla: as route localhost:7058/blablabla
[Route("[controller]")]
//[Authorize]

public class ObjectController : ControllerBase
{

    private readonly ILogger<ObjectController> _logger;
    private readonly IObjectRepository _objectRepository;

    public ObjectController(ILogger<ObjectController> logger, IObjectRepository objectRepository)
    {
        _logger = logger;
        _objectRepository = objectRepository;
    }

    //[HttpGet(Name = "ObjectData")] only use the GET if route is public, else use POST

    [HttpPost("LoadAllObjects1")]
    public ActionResult LoadAllObjects(Models.ReadAllObjectsRequest objectData)
    {
        // Call the repository method with WorldId
        var result = _objectRepository.ReadObjectsAsync(objectData.WorldId).Result;

        // Check if no objects were found
        if (result == null || result.Count == 0)
        {
            return NotFound($"No objects found in world with ID: {objectData.WorldId}");
        }

        return Ok(result);
    }

    [HttpPost("GetWorlds")]
    public async Task<ActionResult> GetWorldsByUser(Models.PlayerRequest playerRequest)
    {
        // Ensure the playerId is not null or empty
        if (string.IsNullOrEmpty(playerRequest.PlayerId))
        {
            return BadRequest("PlayerId is required.");
        }

        // Fetch worlds by PlayerId
        var result = await _objectRepository.GetWorlds(playerRequest.PlayerId);

        if (result == null || !result.Any())
        {
            return NotFound($"No worlds found for user with ID: {playerRequest.PlayerId}");
        }

        return Ok(result);
    }


    [HttpPost("AddWorlds")]
    public async Task<ActionResult> AddWorlds(Models.World objectData)
    {
        // Extract the AccessToken from the request body
        var result = await _objectRepository.AddWorlds(objectData.PlayerId, objectData.WorldName, objectData.Width, objectData.Height);

        // Check if no objects were found
        if (result == null)
        {
            return NotFound($"No objects found in world with name: {objectData.WorldName}");
        }

        return Ok(result);
    }


    [HttpPost("RemoveWorld")]
    public ActionResult RemoveWorld(Models.RemoveWorld objectData)  // Your original method
    {
        var result = _objectRepository.DeleteWorld(objectData.WorldId).Result;
        if (result == null)
            return Ok(result);

        return NotFound($"No objectdata found with ID: {objectData.WorldId}");
    }


    [HttpPost("LoadObjectData")]
    public ActionResult LoadObjectData(Models.ReadObjectsRequest objectData)  // Your original method
    {
        var result = _objectRepository.ReadObjectDataAsync(objectData.ObjectId).Result;
        if (result == null)
            return Ok(result);

        return NotFound($"No objectdata found with ID: {objectData.ObjectId}");
    }

    [HttpPost("GetUser")]
    public async Task<ActionResult> GetUser(Models.LoginRequest loginRequest)
    {
        var result = await _objectRepository.GetUser(loginRequest);

        if (result == null)
            return NotFound($"No user found with username: {loginRequest.Username}");

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
            message = $"Created {objectData.ObjectId}"
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

        // Extract the AccessToken from the request body
        return await _objectRepository.DeleteObjectsAsync(new DeleteObjectRequest { ObjectId = objectData.ObjectId });


    }
}