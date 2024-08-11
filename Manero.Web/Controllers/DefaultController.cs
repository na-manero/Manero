using Manero.Lib.Models;
using Manero.Web.Helpers;
using Manero.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Manero.Web.Controllers;

public class DefaultController(IApiHelper apiHelper) : Controller
{
    private readonly IApiHelper _client = apiHelper;

    public IActionResult Home()
    {
        try
        {
            var productList = ProductsList();

            if (productList != null)
            {
                var model = new ProductsViewModel()
                {
                    Products = productList,
                    Bestsellers = productList.Where(p => p.IsBestSeller == true).ToList(),
                    FeaturedProducts = productList.Where(p => p.IsOnSale == true).ToList()
                };

                return View(model);
            }

        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
        }

        return View();
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
                IsBestSeller = true,
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
                IsOnSale = true,
                Price = 79.99m,
                DiscountPrice = 49.99m,
                Category = "Shoes"
            }
    ];
}
