using Manero.Lib.Models;
using Manero.Web.Helpers;
using Manero.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net;
using System.Security.Claims;

namespace Manero.Web.Controllers;

[Authorize]
public class UserController(IApiHelper client, UserManager<IdentityUser> userManager) : Controller
{
    private readonly IApiHelper _client = client;
    private readonly UserManager<IdentityUser> _userManager = userManager;

    [HttpGet]
    [Route("/profile")]
    public async Task<IActionResult> Profile()
    {
        try
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _client.GetAsync<UserViewModel>($"https://cms23-userprovider-cm.azurewebsites.net/api/account/getuserbyid?userId={userId}");

            if (user != null)
            {
                return View(user);
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine("UserController : " + ex.Message);
            return RedirectToAction("SignOut", "Auth");
        }

        return NotFound();
    }    
    
    [HttpGet]
    [Route("/profile/edit")]
    public async Task<IActionResult> Edit()
    {
        if (!ModelState.IsValid)
            return RedirectToAction("Home", "Default");

        try
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _client.GetAsync<UserViewModel>($"https://cms23-userprovider-cm.azurewebsites.net/api/account/getuserbyid?userId={userId}");

            return View(user);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return NotFound();
        }

    }
    
    [HttpPost]
    [Route("/profile/edit")]
    public async Task<IActionResult> Edit(UserViewModel user)
    {
        if (!ModelState.IsValid)
            return RedirectToAction("Home", "Default");

        try
        {
            var result = await _client.PostAsync($"https://cms23-userprovider-cm.azurewebsites.net/api/account/update-user", user);

            if (result.StatusCode == HttpStatusCode.OK )
            {
                return RedirectToAction("Profile");
            }

            return View(user);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
        }

        return NotFound();
    }
}
