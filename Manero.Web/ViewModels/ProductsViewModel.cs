using Manero.Lib.Models;

namespace Manero.Web.ViewModels;

public class ProductsViewModel
{
    public List<Product> Products { get; set; } = [];
    public List<Product> FeaturedProducts { get; set; } = [];
    public List<Product> Bestsellers { get; set; } = [];
}
