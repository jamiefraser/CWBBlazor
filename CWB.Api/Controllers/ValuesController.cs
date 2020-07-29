
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using CWB.Api.Models;
using Microsoft.Identity.Client;
using Microsoft.Xrm.Tooling.Connector;
using Newtonsoft.Json;
using Refit;
namespace CWB.Api.Controllers
{
    [System.Web.Http.Authorize]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ValuesController : ApiController
    {
        private static string aadInstance = ConfigurationManager.AppSettings["ida:AADInstance"];
        private static string tenant = ConfigurationManager.AppSettings["ida:Tenant"];
        private static string clientId = ConfigurationManager.AppSettings["ida:ClientID"];
        private static string appKey = ConfigurationManager.AppSettings["ida:AppKey"];
        private static string graphUserUrl = ConfigurationManager.AppSettings["ida:GraphUserUrl"];
        private static string redirectUri = ConfigurationManager.AppSettings["ida:RedirectUri"];

        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };
        // GET api/values
        public async Task<IEnumerable<WeatherForecast>> Get()
        {
            var rng = new Random();
            //var user = await CallGraphAPIOnBehalfOfUser();
            var token = await GetAccessTokenForScopeOnBehalfOfLoggedInUser("https://nsappdev03.api.crm.dynamics.com/user_impersonation");
            var svc = Refit.RestService.For<IDynamicsService>("https://nsappdev03.api.crm.dynamics.com");
            var accounts = await svc.GetAccounts($"Bearer {token}");
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            }).ToArray();
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
        private async Task<string> GetAccessTokenForScopeOnBehalfOfLoggedInUser(string scope)
        {
            string[] scopes = { scope };

            // We will use MSAL.NET to get a token to call the API On Behalf Of the current user
            try
            {
                string authority = String.Format(CultureInfo.InvariantCulture, aadInstance, tenant);

                // Creating a ConfidentialClientApplication using the Build pattern (https://github.com/AzureAD/microsoft-authentication-library-for-dotnet/wiki/Client-Applications)
                var app = ConfidentialClientApplicationBuilder.Create(clientId)
                   .WithAuthority(authority)
                   .WithClientSecret(appKey)
                   .WithRedirectUri(redirectUri)
                   .Build();

                // Hooking MSALPerUserSqlTokenCacheProvider class on ConfidentialClientApplication's UserTokenCache.
                //MSALPerUserSqlTokenCacheProvider sqlCache = new MSALPerUserSqlTokenCacheProvider(app.UserTokenCache, dbContext, ClaimsPrincipal.Current);

                //Grab the Bearer token from the HTTP Header using the identity bootstrap context. This requires SaveSigninToken to be true at Startup.Auth.cs
                var bootstrapContext = ClaimsPrincipal.Current.Identities.First().BootstrapContext.ToString();

                // Creating a UserAssertion based on the Bearer token sent by TodoListClient request.
                //urn:ietf:params:oauth:grant-type:jwt-bearer is the grant_type required when using On Behalf Of flow: https://docs.microsoft.com/en-us/azure/active-directory/develop/v2-oauth2-on-behalf-of-flow
                UserAssertion userAssertion = new UserAssertion(bootstrapContext, "urn:ietf:params:oauth:grant-type:jwt-bearer");

                // Acquiring an AuthenticationResult for the scope user.read, impersonating the user represented by userAssertion, using the OBO flow
                AuthenticationResult result = await app.AcquireTokenOnBehalfOf(scopes, userAssertion)
                    .ExecuteAsync();

                string accessToken = result.AccessToken;
                if (accessToken == null)
                {
                    throw new Exception("Access Token could not be acquired.");
                }
                return accessToken;
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }
        private async Task<UserProfile> CallGraphAPIOnBehalfOfUser()
        {
            string[] scopes = { "user.read" };
            UserProfile profile = null;

            // We will use MSAL.NET to get a token to call the API On Behalf Of the current user
            try
            {
                string authority = String.Format(CultureInfo.InvariantCulture, aadInstance, tenant);

                // Creating a ConfidentialClientApplication using the Build pattern (https://github.com/AzureAD/microsoft-authentication-library-for-dotnet/wiki/Client-Applications)
                var app = ConfidentialClientApplicationBuilder.Create(clientId)
                   .WithAuthority(authority)
                   .WithClientSecret(appKey)
                   .WithRedirectUri(redirectUri)
                   .Build();

                // Hooking MSALPerUserSqlTokenCacheProvider class on ConfidentialClientApplication's UserTokenCache.
                //MSALPerUserSqlTokenCacheProvider sqlCache = new MSALPerUserSqlTokenCacheProvider(app.UserTokenCache, dbContext, ClaimsPrincipal.Current);

                //Grab the Bearer token from the HTTP Header using the identity bootstrap context. This requires SaveSigninToken to be true at Startup.Auth.cs
                var bootstrapContext = ClaimsPrincipal.Current.Identities.First().BootstrapContext.ToString();

                // Creating a UserAssertion based on the Bearer token sent by TodoListClient request.
                //urn:ietf:params:oauth:grant-type:jwt-bearer is the grant_type required when using On Behalf Of flow: https://docs.microsoft.com/en-us/azure/active-directory/develop/v2-oauth2-on-behalf-of-flow
                UserAssertion userAssertion = new UserAssertion(bootstrapContext, "urn:ietf:params:oauth:grant-type:jwt-bearer");

                // Acquiring an AuthenticationResult for the scope user.read, impersonating the user represented by userAssertion, using the OBO flow
                AuthenticationResult result = await app.AcquireTokenOnBehalfOf(scopes, userAssertion)
                    .ExecuteAsync();

                string accessToken = result.AccessToken;
                if (accessToken == null)
                {
                    throw new Exception("Access Token could not be acquired.");
                }

                // Call the Graph API and retrieve the user's profile.
                string requestUrl = String.Format(CultureInfo.InvariantCulture, graphUserUrl, HttpUtility.UrlEncode(tenant));
                HttpClient client = new HttpClient();
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, requestUrl);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                HttpResponseMessage response = await client.SendAsync(request);

                // Return the user's profile.
                if (response.IsSuccessStatusCode)
                {
                    string responseString = await response.Content.ReadAsStringAsync();
                    profile = JsonConvert.DeserializeObject<UserProfile>(responseString);
                    return (profile);
                }

                // An unexpected error occurred calling the Graph API.
                throw new Exception("An unexpected error occurred calling the Graph API.");
            }
            catch (MsalUiRequiredException msalServiceException)
            {
                /*
                * If you used the scope `.default` on the client application, the user would have been prompted to consent for Graph API back there
                * and no incremental consents are required (this exception is not expected). However, if you are using the scope `access_as_user`,
                * this exception will be thrown at the first time the API tries to access Graph on behalf of the user for an incremental consent.
                * You must then, add the logic to delegate the consent screen to your client application here.
                * This sample doesn't use the incremental consent strategy.
                */
                throw msalServiceException;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
