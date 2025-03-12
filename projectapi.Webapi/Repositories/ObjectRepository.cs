using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Dapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using projectapi.Webapi.Interfaces;
using projectapi.Webapi.Models;
//using projectapi.Webapi.Interfaces;

namespace projectapi.Webapi.Repositories
{
    public class ObjectRepository : IObjectRepository
    {
        public string sqlConnectionString;
        public ObjectRepository(string connectionString)
        {
            sqlConnectionString = connectionString;
        }

        public async Task<List<Models.Object>> ReadObjectsAsync(Guid worldId)
        {
            using (var sqlConnection = new SqlConnection(sqlConnectionString))
            {
                var query = "SELECT * FROM Objects WHERE WorldId = @WorldId";
                var objects = await sqlConnection.QueryAsync<Models.Object>(query, new { WorldId = worldId });

                return objects.ToList();
            }
        }

        public async Task<IEnumerable<Models.World>> GetWorlds(string playerId)
        {
            using (var sqlConnection = new SqlConnection(sqlConnectionString))
            {
                var query = "SELECT * FROM Worlds WHERE PlayerId = @PlayerId";
                return await sqlConnection.QueryAsync<Models.World>(query, new { PlayerId = playerId });
            }
        }

        public async Task<Models.RemoveWorld?> DeleteWorld(Guid worldId)
        {
            using (var sqlConnection = new SqlConnection(sqlConnectionString))
            {
                var query = "DELETE FROM Worlds WHERE WorldId = @WorldId";
                return await sqlConnection.QuerySingleOrDefaultAsync<Models.RemoveWorld>(query, new { WorldId = worldId });

            }
        }

        public async Task<Models.World?> AddWorlds(string PlayerId, string WorldName, int Width, int Height)
        {
      
            Guid newWorldId = Guid.NewGuid();

            using (var sqlConnection = new SqlConnection(sqlConnectionString))
            {
                var insertQuery = @"
        INSERT INTO Worlds (WorldId, PlayerId, WorldName, Width, Height)
        VALUES (@WorldId, @PlayerId, @WorldName, @Width, @Height)";

                await sqlConnection.ExecuteAsync(insertQuery, new
                {
                    WorldId = newWorldId,
                    PlayerId = PlayerId,  
                    WorldName = WorldName,
                    Width = Width,
                    Height = Height
                });

                var selectQuery = "SELECT * FROM Worlds WHERE WorldId = @WorldId";
                return await sqlConnection.QuerySingleOrDefaultAsync<Models.World>(selectQuery, new { WorldId = newWorldId });
            }

            
        }

        public async Task<string?> GetUser(Models.LoginRequest loginRequest)
        {
            using (var sqlConnection = new SqlConnection(sqlConnectionString))
            {
                var query = "SELECT Id, PasswordHash FROM [Periode3].[auth].[AspNetUsers] WHERE UserName = @UserName";

                return await sqlConnection.QuerySingleOrDefaultAsync<string>(
                    query, new { UserName = loginRequest.Username }
                );
            }
        }

        public async Task<Models.Object?> ReadObjectDataAsync(Guid objectId)
        {

            using (var sqlConnection = new SqlConnection(sqlConnectionString))
            {
                var query = "SELECT * FROM Objects WHERE ObjectId = @ObjectId";
                return await sqlConnection.QuerySingleOrDefaultAsync<Models.Object>(query, new { ObjectId = objectId });
            }
        }

        public async Task<Models.Object?> UpdateObjectsAsync(Guid objectId, string prefabId, float positionX, float positionY, float scaleX, float scaleY, float rotationZ, int layerZ)
        {
            using (var sqlConnection = new SqlConnection(sqlConnectionString))
            {
                var query = @"
                    UPDATE Objects
                    SET 
                        PrefabId = @PrefabId,
                        PositionX = @PositionX,
                        PositionY = @PositionY,
                        ScaleX = @ScaleX,
                        ScaleY = @ScaleY,
                        RotationZ = @RotationZ,
                        LayerZ = @LayerZ
                    WHERE 
                        ObjectId = @ObjectId";

                return await sqlConnection.QuerySingleOrDefaultAsync<Models.Object>(query, new
                {
                    ObjectId = objectId,
                    PrefabId = prefabId,
                    PositionX = positionX,
                    PositionY = positionY,
                    ScaleX = scaleX,
                    ScaleY = scaleY,
                    RotationZ = rotationZ,
                    LayerZ = layerZ
                });

            }
        }

        public async Task<Models.DeleteObjectRequest?> DeleteObjectsAsync(DeleteObjectRequest objectId)
        {
            using (var sqlConnection = new SqlConnection(sqlConnectionString))
            {
                var query = "DELETE FROM Objects WHERE ObjectId = @ObjectId";
                //return await sqlConnection.QuerySingleOrDefaultAsync<Models.DeleteObjectRequest>(query, new { ObjectId = objectId });
                return await sqlConnection.QuerySingleOrDefaultAsync<Models.DeleteObjectRequest>(query, new { ObjectId = objectId.ObjectId });


            }
        }

        public async Task<Models.Object?> InsertObjectAsync(Guid worldId, string prefabId, float positionX, float positionY, float scaleX, float scaleY, float rotationZ, int layerZ)
        {
            using (var sqlConnection = new SqlConnection(sqlConnectionString))
            {
                Guid newObjectId = Guid.NewGuid();

                var query = @"
            INSERT INTO Objects (ObjectId, WorldId, PrefabId, PositionX, PositionY, ScaleX, ScaleY, RotationZ, LayerZ) 
            VALUES (@ObjectId, @WorldId, @PrefabId, @PositionX, @PositionY, @ScaleX, @ScaleY, @RotationZ, @LayerZ)";

                return await sqlConnection.QuerySingleOrDefaultAsync<Models.Object>(query, new
                {
                    ObjectId = newObjectId,
                    WorldId = worldId,
                    PrefabId = prefabId,
                    PositionX = positionX,
                    PositionY = positionY,
                    ScaleX = scaleX,
                    ScaleY = scaleY,
                    RotationZ = rotationZ,
                    LayerZ = layerZ
                });
            }

                
            }

    }
}


