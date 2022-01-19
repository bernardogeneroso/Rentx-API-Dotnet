namespace Models;

public class CarImage
{
    public int Id { get; set; }
    public string Plate { get; set; }
    public string ImageName { get; set; }
    public string Url { get; set; }
    public bool IsMain { get; set; }
    public Car Car { get; set; }
}
