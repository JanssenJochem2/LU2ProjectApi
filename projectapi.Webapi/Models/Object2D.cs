namespace projectapi.Webapi.Models
{
    public class Object2D
    {
        int Id { get; set; }
        int EnviromentId { get; set; }
        float PositionX { get; set; }
        float PositionY { get; set; }
        float ScaleX { get; set; }
        float ScaleY { get; set; }
        float RotationZ { get; set; }
        string SortingLayer { get; set; }
    }
}
