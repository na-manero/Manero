using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Manero.Web.ViewModels;
using Microsoft.AspNetCore.Identity;
using System.Net;
using Manero.Web.Helpers;
using System.Diagnostics;

namespace Manero.Web.Controllers;

public class AuthController(IApiHelper client, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager) : Controller
{
    private readonly IApiHelper _client = client;
    private readonly UserManager<IdentityUser> _userManager = userManager;
    private readonly SignInManager<IdentityUser> _signInManager = signInManager;

    [HttpGet]
    [Route("/signin")]
    public IActionResult SignIn()
    {
        return View();
    }

    [HttpPost]
    [Route("/signin")]
    public async Task<IActionResult> SignIn(SignInViewModel model)
    {
        try
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Home", "Default");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine("AuthController : " + ex.Message);
        }

        ViewData["StatusMessage"] = "Something went wrong. please try again";
        return View();
    }

    [HttpGet]
    [Route("/signup")]
    public IActionResult SignUp()
    {
        return View();
    }

    [HttpPost]
    [Route("/signup")]
    public async Task<IActionResult> SignUp(SignUpViewModel model)
    {
        try
        {
            if (ModelState.IsValid)
            {
                var response = await _client.PostAsync("https://cms23-authprovider-cm.azurewebsites.net/api/auth/signup", model);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Home", "Default");
                }
                else if (response.StatusCode == HttpStatusCode.Conflict)
                {
                    ViewData["StatusMessage"] = "User with same email already exist";
                }
                else
                {
                    ViewData["StatusMessage"] = "Something went wrong. please try again";
                }
            }
            else
            {
                ViewData["StatusMessage"] = "Please enter all information correctly";
            }
        }
        catch (Exception ex) 
        { 
            Debug.WriteLine(ex.Message);
        }

        ViewData["StatusMessage"] = "Something went wrong. please try again";
        return View();
    }

    [Route("/signout")]
    public new async Task<IActionResult> SignOut()
    {
        try
        {
            await _signInManager.SignOutAsync();
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }
        catch (Exception ex) { Debug.WriteLine(ex.Message); }

        return RedirectToAction("Home", "Default");
    }
}


