public class HouseEntity 
{
    //because it is called Id, it will be the primary key
    public int Id {get; set;}
    public string? Address { get; set; }
    public string? Country { get; set; }
    public string? Description { get; set; }
    public int Price { get; set; }
    public string? Photo { get; set; }
}