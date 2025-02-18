using System.ComponentModel.DataAnnotations;

namespace projectapi.Webapi;

public class Object2D
{
    public int Id { get; set; }
    public int EnviromentId { get; set; }
    public float PositionX { get; set; }
    public float PositionY { get; set; }
    public float ScaleX { get; set; }
    public float ScaleY { get; set; }
    public float RotationZ { get; set; }
    public string SortingLayer { get; set; }
}

