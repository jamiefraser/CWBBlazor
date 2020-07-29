using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using API.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Graph;
using Microsoft.Identity.Client;
using Microsoft.Identity.Web;
using Newtonsoft.Json;

namespace API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ITokenAcquisition _tokenAcquisition;

        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, ITokenAcquisition tokenAcquisition)
        {
            _logger = logger;
            _tokenAcquisition = tokenAcquisition;
        }

        [HttpGet]
        public async Task<IEnumerable<WeatherForecast>> Get()
        {

            System.Diagnostics.Debug.WriteLine(HttpContext.User.Identity.IsAuthenticated);
            System.Diagnostics.Debug.WriteLine(HttpContext.User.Identity.Name);
            try
            {
                var profile = CallGraphApiOnBehalfOfUser().GetAwaiter().GetResult();

                if (profile is string)
                {
                    if (profile == "interaction required")
                    {
                        HttpContext.Response.ContentType = "application/json";
                        HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                        await HttpContext.Response.WriteAsync(JsonConvert.SerializeObject("interaction required"));
                    }
                }
                else
                {
                    var s = (string)profile.UserPrincipalName;
                    System.Diagnostics.Debug.WriteLine(s);
                }
            }
            catch (MsalException ex)
            {
                HttpContext.Response.ContentType = "application/json";
                HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                await HttpContext.Response.WriteAsync(JsonConvert.SerializeObject("An authentication error occurred while acquiring a token for downstream API\n" + ex.ErrorCode + "\n" + ex.Message));
            }
            catch (Exception ex)
            {
                HttpContext.Response.ContentType = "application/json";
                HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                await HttpContext.Response.WriteAsync(JsonConvert.SerializeObject("An error occurred while calling the downstream API\n" + ex.Message));
            }
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }

        public async Task<dynamic> CallGraphApiOnBehalfOfUser()
        {
            string[] scopes = { "User.Read" };
            dynamic response;

            // we use MSAL.NET to get a token to call the API On Behalf Of the current user
            try
            {
                string accessToken = await _tokenAcquisition.GetAccessTokenForUserAsync(scopes);
                GraphHelper.Initialize(accessToken);
                User me = await GraphHelper.GetMeAsync();
                response = me;
            }
            catch (MsalUiRequiredException ex)
            {
                await _tokenAcquisition.ReplyForbiddenWithWwwAuthenticateHeaderAsync(scopes, ex);
                return "interaction required";
            }

            return response;
        }
    }
}
