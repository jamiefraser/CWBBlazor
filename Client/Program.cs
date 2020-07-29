using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CWBBlazor.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");

            //Add our CWBApiAuthorization Handler to the scoped services -> you create a different AuthorizationHandler for every unique endpoint you want to access (uniqueness is thought of in the context of the URI and Scope)
            builder.Services.AddScoped<CWBApiAuthorizationHandler>();
            
            //Now create a named service instance for HttpClient that points to the correct base address (based on your handler configuration) and instantiates the right authorization handler for that client
            //See FetchData.razor to see how we then use this client
            builder.Services.AddHttpClient("ServerAPI",
                client => client.BaseAddress = new Uri("https://localhost:44345/api/"))
                .AddHttpMessageHandler<CWBApiAuthorizationHandler>();
            builder.Services.AddMsalAuthentication(options =>
            {
                builder.Configuration.Bind("AzureAd", options.ProviderOptions.Authentication);
                options.ProviderOptions.DefaultAccessTokenScopes.Add("https://nsdevelopment.onmicrosoft.com/DualBlazor/user_impersonation");
            });
            builder.Services.AddAuthorizationCore();
            await builder.Build().RunAsync();
        }
    }
}
