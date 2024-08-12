using Manero.Lib.Models;
using Manero.Web.Helpers;
using Manero.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;

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
                return RedirectToAction("Profile", "User");
            }

            return View(user);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
        }

        return NotFound();
    }

    [HttpPost("upload")]
    public async Task<IActionResult> UploadImage(IFormFile file)
    {
        if (string.IsNullOrWhiteSpace(file.FileName))
            return BadRequest("File must be provided.");

        try
        {
            // Create the multipart form-data content
            using var multipartContent = new MultipartFormDataContent();

            // Create StreamContent from the uploaded file
            using var fileStream = file.OpenReadStream();
            using var streamContent = new StreamContent(fileStream);

            // Set the content type of the file
            streamContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);

            // Add the file to the multipart content
            multipartContent.Add(streamContent, "file", file.FileName);

            // Send the POST request to the API
            var response = await _client.PostFormData("https://cms23-imageprovider-cm.azurewebsites.net/api/Image/upload", multipartContent);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                try
                {
                    // Get the currently logged-in user's ID
                    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                    // Fetch the current user data
                    var user = await _client.GetAsync<UserViewModel>($"https://cms23-userprovider-cm.azurewebsites.net/api/account/getuserbyid?userId={userId}");

                    if (user == null) 
                        return NotFound("User not found.");

                    var jsonResponse = await response.Content.ReadAsStringAsync();

                    // Update the ImageUrl with the new URL
                    var result = _client.DeserializeJson<ImageResponse>(jsonResponse);

                    if (result == null) 
                        return BadRequest();

                    user.ImageUrl = result.Url;

                    // Send the updated user data back to the API
                    var updateResult = await _client.PostAsync($"https://cms23-userprovider-cm.azurewebsites.net/api/account/update-user", user);

                    if (updateResult.StatusCode == HttpStatusCode.OK)
                    {
                        return RedirectToAction("Home", "Default");
                    }

                    return BadRequest("Failed to update user image URL.");
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("UploadImage : " + ex.Message);
                    return StatusCode(500, "An error occurred while updating the user's image.");
                }
            }
        }
        catch (Exception ex) { Debug.WriteLine("UploadImage : " + ex.Message); }

        return BadRequest("Failed to upload image.");
    }
}
