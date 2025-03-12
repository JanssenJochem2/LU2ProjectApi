using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using projectapi.Webapi.Models;

namespace projectapi.Webapi.Interfaces
{
    public interface IObjectRepository
    {
        Task<List<Models.Object>> ReadObjectsAsync(Guid worldId);
        Task<IEnumerable<Models.World>> GetWorlds(string playerId);
        Task<Models.World?> AddWorlds(string PlayerId, string WorldName, int Width, int Height);
        Task<string?> GetUser(Models.LoginRequest loginRequest);
        Task<Models.Object?> ReadObjectDataAsync(Guid objectId);
        Task<Models.RemoveWorld?> DeleteWorld(Guid worldId);
        Task<Models.Object?> UpdateObjectsAsync(Guid objectId, string prefabId, float positionX, float positionY, float scaleX, float scaleY, float rotationZ, int layerZ);
        Task<Models.DeleteObjectRequest?> DeleteObjectsAsync(DeleteObjectRequest objectId);
        Task<Models.Object?> InsertObjectAsync( Guid worldId, string prefabId, float positionX, float positionY, float scaleX, float scaleY, float rotationZ, int layerZ);
    }

}