namespace AuctionsServices.Entites.Dtos;
public class UpdateAuctionDto
{
    public string Make { get; set; }
    public string Model { get; set; }
    public int? Year { get; set; }
    public int? Mileage { get; set; }
    public string Color { get; set; }

}
