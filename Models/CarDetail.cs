namespace Models;

public class CarDetail
{
    public string Plate { get; set; }
    public int MaxSpeed { get; set; }
    public int TopSpeed { get; set; }
    public float Acceleration { get; set; }
    public int Weight { get; set; }
    public int Hp { get; set; }
    public Car Car { get; set; }
}
