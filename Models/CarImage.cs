namespace Models;

public class CarImage
{
    public string Plate { get; set; }
    public string ImageName { get; set; }
    public bool IsMain { get; set; }
    public Car Car { get; set; }
}
