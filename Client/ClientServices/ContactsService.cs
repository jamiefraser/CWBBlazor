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
    public class ContactsService: ComponentBase, INotifyPropertyChanged
    {
        #region ctor
        IHttpClientFactory httpClientFactory;
        IndexedDBManager dbManager;
        HttpClient client;
        public ContactsService(IHttpClientFactory _httpClientFactory, IndexedDBManager _dBManager)
        {
            dbManager = _dBManager;
            dbManager.ActionCompleted += OnIndexedDbNotification;
            httpClientFactory = _httpClientFactory;
            client = httpClientFactory.CreateClient("ServerAPI");
            refreshContacts();
        }
        private void OnIndexedDbNotification(object sender, IndexedDBNotificationArgs args)
        {
            System.Diagnostics.Debug.WriteLine(args.Message);
            if (args.Outcome == IndexDBActionOutCome.Failed)
            {
                System.Diagnostics.Debug.WriteLine(args.Message);
            }
        }
        #endregion

        #region Private Methods
        private async Task persistContactsLocally(List<Contact> contactsToPersist)
        {
            var newCount = 0;
            var updateCount = 0;
            var skipCount = 0;
            foreach (Contact c in contactsToPersist)
            {
                var getContact = await dbManager.GetRecordById<string, Contact>("Contacts", c.contactid);

                var writeContact = new StoreRecord<Contact>
                {
                    Storename = "Contacts",
                    Data = c
                };
                if (getContact== null)
                {
                    await dbManager.AddRecord(writeContact);
                    newCount++;
                }
                else
                {
                    if (getContact.modifiedon != c.modifiedon)
                    {

                        await dbManager.UpdateRecord<Contact>(writeContact);
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
        private async Task<List<Contact>> getLocalContacts()
        {
            var results = await dbManager.GetRecords<Contact>("Contacts");
            return results.ToList<Contact>();
        }
        private async Task refreshContacts()
        {
            await getLocalContacts().ContinueWith(ac =>
            {
                this.Contacts = ac.Result;
            }).ContinueWith(async (acContinue) =>
            {
                this.Contacts = await client.GetFromJsonAsync<List<Contact>>("contacts");
                await persistContactsLocally(this.Contacts);
            });

        }
        #endregion

        #region Public Methods
        public async Task RefreshContacts(CancellationToken token)
        {
            var task = Task.Run(async () =>
            {
                token.ThrowIfCancellationRequested();
                await refreshContacts();
            }, token);
            try
            {
                await task;
            }
            catch (TaskCanceledException tcEx)
            {
                System.Diagnostics.Debug.WriteLine("Cancelled Refresh Accounts request");
            }
        }

        public async Task<Contact> GetContactAsync(string contactId)
        {
            var ret = await dbManager.GetRecordById<string, Contact>("Contacts", contactId);
            return ret;
        }
        #endregion

        #region Properties
        private List<Contact> contacts;
        public List<Contact> Contacts
        {
            get
            {
                return contacts;
            }
            set
            {
                contacts= value;
                NotifyPropertyChanged("Contacts");
            }
        }
        #endregion
        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
