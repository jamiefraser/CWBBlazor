using CWB.Api.Models;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using TG.Blazor.IndexedDB;

namespace CWBBlazor.Client.ClientServices
{
    public class AccountsService : ComponentBase, INotifyPropertyChanged
    {
        IHttpClientFactory httpClientFactory;
        IndexedDBManager dbManager;
        HttpClient client;
        public AccountsService(IHttpClientFactory _httpClientFactory, IndexedDBManager _dBManager)
        {
            dbManager = _dBManager;
            dbManager.ActionCompleted += OnIndexedDbNotification;
            httpClientFactory = _httpClientFactory;
            client = httpClientFactory.CreateClient("ServerAPI");
        }
        private void OnIndexedDbNotification(object sender, IndexedDBNotificationArgs args)
        {
            System.Diagnostics.Debug.WriteLine(args.Message);
            if (args.Outcome == IndexDBActionOutCome.Failed)
            {
                System.Diagnostics.Debug.WriteLine(args.Message);
            }
        }
        private async Task PersistAccountLocally(List<Account> accountsToPersist)
        {
            var newCount = 0;
            var updateCount = 0;
            var skipCount = 0;
            foreach(Account a in accountsToPersist)
            {
                var getAccount = await dbManager.GetRecordById<string, Account>("Accounts", a.accountid);
                
                var writeAccount = new StoreRecord<Account>
                {
                    Storename = "Accounts",
                    Data = a
                };
                if (getAccount == null)
                {
                    System.Diagnostics.Debug.WriteLine("Writing a new account!");
                    await dbManager.AddRecord(writeAccount);
                    newCount++;
                }
                else
                {
                    if (getAccount.modifiedon != a.modifiedon)
                    {

                        await dbManager.UpdateRecord<Account>(writeAccount);
                        updateCount++;
                    }
                    else
                    {
                        skipCount++;
                    }
                }
            }
            System.Diagnostics.Debug.WriteLine($"New: {newCount}/Updated: {updateCount}/Skipped: {skipCount}");
        }
        private async Task<List<Account>> GetLocalAccounts()
        {
            var results = await dbManager.GetRecords<Account>("Accounts");
            return results.ToList<Account>();
        }
        private async Task RefreshAccounts()
        {
            await GetLocalAccounts().ContinueWith(ac =>
            {
                this.Accounts = ac.Result;
            }).ContinueWith(async (acContinue) =>
            {
                this.Accounts = await client.GetFromJsonAsync<List<Account>>("accounts");
                await PersistAccountLocally(this.Accounts);
            });

        }
        public async Task RefreshAccounts(CancellationToken token)
        {
            var task = Task.Run(async () =>
            {
                token.ThrowIfCancellationRequested();
                await RefreshAccounts();
            }, token);
            try
            {
                await task;
            }
            catch(TaskCanceledException tcEx)
            {
                System.Diagnostics.Debug.WriteLine("Cancelled Refresh Accounts request");
            }
        }
        public List<Account>Accounts 
        {
            get
            {
                return accounts;
            }
            set
            {
                accounts = value;
                NotifyPropertyChanged("Accounts");
            }
        }
        public async Task<Account>GetAccountAsync(string accountId)
        {
            var ret = await dbManager.GetRecordById<string, Account>("Accounts", accountId);
            return ret;
        }
        private List<Account> accounts;

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
