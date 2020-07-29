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
using TG.Blazor.IndexedDB;
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

            builder.Services.AddIndexedDB(dbStore =>
            {
                dbStore.DbName = "D365Entities";
                //Important!  Every time you decide to add another table like you're doing in the the dbStore.Stores.Add call below, increment the dbStore.Version.  That way you can extend the database schema without losing the already persisted data
                //If you mess up your PrimaryKey definition(s) you may need to fiddle around to get rid of the messed up table -> changing the Version won't fix the problem (it seems).  Instead you need to either delete the table or delete the database and start again
                //You can manage IndexedDb instances in the browser -> Go to Developer Tools/Application
                dbStore.Version = 2;
                dbStore.Stores.Add(new StoreSchema
                {
                    Name = "Accounts",
                    //The Name and KeyPath are json cased, meaning the first character of the field name is lower case regardless of how you named the Property in your class
                    //For instance, FirstName becomes firstName, and Id becomes id
                    //If you don't get it right when you try to insert records the insert will fail
                    PrimaryKey = new IndexSpec
                    {
                        Name = "accountid",
                        KeyPath = "accountid",
                        Auto = false,
                        Unique = true
                    },
                    Indexes = new List<IndexSpec>
                    {
                        new IndexSpec{Name="name", KeyPath="name", Auto=false}
                    }
                });
                dbStore.Stores.Add(new StoreSchema
                {
                    Name = "Forecasts",
                    PrimaryKey = new IndexSpec
                    {
                        Name = "id",
                        KeyPath = "id",
                        Auto = false
                    }
                });
            }
            );
            builder.Services.AddAuthorizationCore();
            await builder.Build().RunAsync();
        }
    }
}
