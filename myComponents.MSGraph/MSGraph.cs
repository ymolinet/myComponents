using Microsoft.Graph;
using Microsoft.Graph.Auth;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace myComponents.MSGraph
{
    public class MSGraphHelper
    {
        private IConfidentialClientApplication _confidentialClientApplication;
        private ClientCredentialProvider _authProvider;
        private GraphServiceClient _graphClient;
        public MSGraphHelper()
        { }
        public MSGraphHelper(string AppID, string AppSecret, string TenantUUID)
        {
            Connect(AppID, AppSecret, TenantUUID);
        }

        public void Connect(string AppID, string AppSecret, string TenantUUID)
        {
            _confidentialClientApplication = ConfidentialClientApplicationBuilder
               .Create(AppID)
               .WithTenantId(TenantUUID)
               .WithClientSecret(AppSecret)
               .Build();

            _authProvider = new ClientCredentialProvider(_confidentialClientApplication);
            _graphClient = new GraphServiceClient(_authProvider);
        }

        public void Disconnect()
        {
        }

        public async Task<List<TenantLicenceData>> GetTenantLicencesInfosAsync()
        {
            List<TenantLicenceData> licences = new List<TenantLicenceData>();
            IGraphServiceSubscribedSkusCollectionPage skus = await _graphClient.SubscribedSkus.Request().GetAsync();
            licences.AddRange(SetTenantLicence(skus));
            while (skus.NextPageRequest != null)
            {
                skus = await skus.NextPageRequest.GetAsync();
                licences.AddRange(SetTenantLicence(skus));
            }
            return licences;
        }

        private List<TenantLicenceData> SetTenantLicence(IGraphServiceSubscribedSkusCollectionPage skus)
        {
            List<TenantLicenceData> licences = new List<TenantLicenceData>();
            foreach (SubscribedSku sku in skus.CurrentPage)
            {
                TenantLicenceData licence = new TenantLicenceData(sku);
                licences.Add(licence);
            }
            return licences;
        }

        public async Task<List<UserLicenceData>> GetUsersLicencesInfosAsync()
        {
            List<UserLicenceData> licences = new List<UserLicenceData>();
            IGraphServiceUsersCollectionPage users = await _graphClient.Users.Request().Select("Id,userPrincipalName,Department,LicenseDetails").GetAsync();
            licences.AddRange(await SetUserLicenceAsync(users));
            while (users.NextPageRequest != null)
            {
                users = await users.NextPageRequest.GetAsync();
                licences.AddRange(await SetUserLicenceAsync(users));
            }
            return licences;
        }

        private async Task<List<UserLicenceData>> SetUserLicenceAsync(IGraphServiceUsersCollectionPage users)
        {
            List<UserLicenceData> licences = new List<UserLicenceData>();
            foreach (Microsoft.Graph.User user in users.CurrentPage)
            {
                IUserLicenseDetailsCollectionPage licenseDetails = await _graphClient.Users[user.Id].LicenseDetails.Request().GetAsync();
                UserLicenceData licence = new UserLicenceData(user, licenseDetails);
                licences.Add(licence);
            }
            return licences;
        }



    }
}
