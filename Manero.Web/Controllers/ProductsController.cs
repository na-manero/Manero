using Manero.Lib.Models;
using Manero.Web.Helpers;
using Manero.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Manero.Web.Controllers;

public class ProductsController(IApiHelper client) : Controller
{
    private readonly IApiHelper _client = client;

    [HttpGet]
    [Route("/products")]
    public IActionResult Products()
    {
        return View(ProductsList());
    }

    [HttpGet]
    [Route("/products/{id}")]
    public async Task<IActionResult> ProductDetails(string id)
    {
        var product = new Product();
        try 
        { 
            //product = await _client.GetAsync<Product>($"https://localhost:7068/api/{id}");
            product = ProductsList().FirstOrDefault(p => p.Id == id);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"An error occurred: {ex.Message}");
        }

        return View(product);
    }

    private List<Product> ProductsList() => [
            new() {
                Id = "1",
                ProductName = "Classic T-Shirt",
                ProductDescription = "A comfortable and stylish classic t-shirt made from 100% cotton.",
                Stock = 150,
                IsBestSeller = true,
                IsOnSale = false,
                Price = 19.99m,
                DiscountPrice = null,
                Category = "T-Shirts"
            },
            new() {
                Id = "2",
                ProductName = "Slim Fit Jeans",
                ProductDescription = "Trendy slim fit jeans with a slight stretch for comfort.",
                Stock = 80,
                IsBestSeller = false,
                IsOnSale = true,
                Price = 49.99m,
                DiscountPrice = 39.99m,
                Category = "Jeans"
            },
            new() {
                Id = "3",
                ProductName = "Hooded Sweatshirt",
                ProductDescription = "Cozy hooded sweatshirt perfect for casual wear.",
                Stock = 120,
                IsBestSeller = true,
                IsOnSale = true,
                Price = 29.99m,
                DiscountPrice = 24.99m,
                Category = "Sweatshirts"
            },
            new() {
                Id = "4",
                ProductName = "Chino Shorts",
                ProductDescription = "Lightweight chino shorts ideal for warm weather.",
                Stock = 60,
                IsBestSeller = false,
                IsOnSale = false,
                Price = 34.99m,
                DiscountPrice = null,
                Category = "Shorts"
            },
            new() {
                Id = "5",
                ProductName = "Leather Jacket",
                ProductDescription = "Stylish leather jacket for a bold look.",
                Stock = 30,
                IsBestSeller = true,
                IsOnSale = true,
                Price = 149.99m,
                DiscountPrice = 119.99m,
                Category = "Jackets"
            },
            new() {
                Id = "6",
                ProductName = "Running Shoes",
                ProductDescription = "Lightweight and durable running shoes for all types of terrain.",
                Stock = 200,
                IsBestSeller = true,
                IsOnSale = false,
                Price = 79.99m,
                DiscountPrice = null,
                Category = "Shoes"
            }
        ];
}
