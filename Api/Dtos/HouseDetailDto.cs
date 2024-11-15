using System.ComponentModel.DataAnnotations;

public record HouseDetailDto(int Id, string? Address, string? Country,
     int Price, string? Description, string? Photo);