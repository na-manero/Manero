namespace Manero.Lib.Models;

public class Product
{
    public string? Id { get; set; }
    public string ProductName { get; set; } = null!;
    public string ProductDescription { get; set; } = null!;
    public int Stock { get; set; }
    public bool IsBestSeller { get; set; }
    public bool IsOnSale { get; set; }
    public decimal Price { get; set; }
    public decimal? DiscountPrice { get; set; }
    public string? Category { get; set; }
    public string? ThumbnailImage { get; set; }
    public string? ImgUrl { get; set; }
}
