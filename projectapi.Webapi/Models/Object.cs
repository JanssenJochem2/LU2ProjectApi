namespace projectapi.Webapi.Models
{
    public class Object
    {
        public Guid ObjectId { get; set; }
        public Guid WorldId { get; set; }
        public string PrefabId { get; set; }
        public float PositionX { get; set; }
        public float PositionY { get; set; }
        public float ScaleX { get; set; }
        public float ScaleY { get; set; }
        public float RotationZ { get; set; }
        public int LayerZ { get; set; }
    }

    public class refreshUser
    {
        public string refreshToken { get; set; }
    }

    public class LoginRequest
    {
        public string Username { get; set; }
    }

    public class World
    {
        public Guid WorldId { get; set; }
        public string PlayerId { get; set; } 
        public string WorldName { get; set; } 
        public int Width { get; set; }
        public int Height { get; set; } 

    }
    public class RemoveWorld
    {
        public Guid WorldId { get; set; }
    }

    public class PlayerRequest
    {
        public string PlayerId { get; set; }
    }


    public class AddWorld
    {
        public string WorldName { get; set; }
        public string PlayerId { get; set; } 
        public int Width { get; set; }
        public int Height { get; set; }
        public string AccessToken { get; set; }
    }



    public class InsertObjectRequest
    {
        public Guid? ObjectId { get; set; }
        public Guid WorldId { get; set; }
        public string PrefabId { get; set; }
        public float PositionX { get; set; }
        public float PositionY { get; set; }
        public float ScaleX { get; set; }
        public float ScaleY { get; set; }
        public float RotationZ { get; set; }
        public int LayerZ { get; set; }
    }

    public class ReadObjectsRequest
    {
        public Guid ObjectId { get; set; }
    }

    public class UpdateObjectRequest
    {
        public Guid ObjectId { get; set; }
        public string PrefabId { get; set; }
        public float PositionX { get; set; }
        public float PositionY { get; set; }
        public float ScaleX { get; set; }
        public float ScaleY { get; set; }
        public float RotationZ { get; set; }
        public int LayerZ { get; set; }
    }

    public class DeleteObjectRequest
    {
        public Guid ObjectId { get; set; }
    }

    public class ReadAllObjectsRequest
    {
        public Guid WorldId { get; set; }
        public Guid ObjectId { get; set; }
    }

    public class ObjectCollection
    {
        public IEnumerable<Models.Object> Objects { get; set; }

    }
}
