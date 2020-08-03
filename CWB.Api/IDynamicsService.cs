using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CWB.Api.Models;
using Refit;
namespace CWB.Api
{
    public interface IDynamicsService
    {
        [Get("/api/data/v9.1/accounts")]
        Task<AccountsResponse> GetAccounts([Header("Authorization")] string authHeader);
        [Get("/api/data/v9.1/contacts")]
        Task<ContactsResponse> GetContacts([Header("Authorization")] string authHeader);
    }
}
