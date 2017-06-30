﻿// ———————————————————————————————
// <copyright file="VstsService.cs">
// Licensed under the MIT License. See License.txt in the project root for license information.
// </copyright>
// <summary>
// Contains the ProfileService
// </summary>
// ———————————————————————————————

namespace Vsar.TSBot
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.TeamFoundation.Build.WebApi;
    using Microsoft.TeamFoundation.Core.WebApi;
    using Microsoft.VisualStudio.Services.Account;
    using Microsoft.VisualStudio.Services.Account.Client;
    using Microsoft.VisualStudio.Services.OAuth;
    using Microsoft.VisualStudio.Services.Profile;
    using Microsoft.VisualStudio.Services.Profile.Client;
    using Microsoft.VisualStudio.Services.ReleaseManagement.WebApi;
    using Microsoft.VisualStudio.Services.ReleaseManagement.WebApi.Clients;
    using Microsoft.VisualStudio.Services.ReleaseManagement.WebApi.Contracts;
    using Microsoft.VisualStudio.Services.WebApi;
    using Resources;

    /// <summary>
    /// Contains method(s) for accessing VSTS.
    /// </summary>
    public class VstsService : IVstsService
    {
        private const string VstsUrl = "https://{0}.visualstudio.com";
        private const string VstsRmUrl = "https://{0}.vsrm.visualstudio.com";

        private readonly Uri vstsAppUrl = new Uri("https://app.vssps.visualstudio.com");

        /// <inheritdoc />
        public async Task ChangeApprovalStatus(string account, string teamProject, VstsProfile profile, int approvalId, ApprovalStatus status, string comments)
        {
            if (string.IsNullOrWhiteSpace(account))
            {
                throw new ArgumentNullException(nameof(account));
            }

            if (string.IsNullOrWhiteSpace(teamProject))
            {
                throw new ArgumentNullException(nameof(teamProject));
            }

            if (profile == null)
            {
                throw new ArgumentNullException(nameof(profile));
            }

            OAuthToken token = profile.Token;

            using (var client = await GetConnectedClientAsync<ReleaseHttpClient2>(await this.GetAccountUriAsync(account, token), token))
            {
                var approval = await client.GetApprovalAsync(teamProject, approvalId);
                approval.Status = status;
                approval.Comments = comments;

                await client.UpdateReleaseApprovalAsync(approval, teamProject, approvalId);
            }
        }

        /// <inheritdoc/>
        public async Task<IList<Account>> GetAccounts(OAuthToken token, Guid memberId)
        {
            if (token == null)
            {
                throw new ArgumentNullException(nameof(token));
            }

            using (var client = await GetConnectedClientAsync<AccountHttpClient>(this.vstsAppUrl, token))
            {
                return await client.GetAccountsByMemberAsync(memberId);
            }
        }

        /// <inheritdoc/>
        public async Task<ReleaseApproval> GetApproval(string account, string teamProject, int approvalId, OAuthToken token)
        {
            if (string.IsNullOrWhiteSpace(account))
            {
                throw new ArgumentNullException(nameof(account));
            }

            if (string.IsNullOrWhiteSpace(teamProject))
            {
                throw new ArgumentNullException(nameof(teamProject));
            }

            if (approvalId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(approvalId));
            }

            if (token == null)
            {
                throw new ArgumentNullException(nameof(token));
            }

            using (var client = await GetConnectedClientAsync<ReleaseHttpClient2>(await this.GetAccountUriAsync(account, token), token))
            {
                return await client.GetApprovalAsync(teamProject, approvalId);
            }
        }

        /// <inheritdoc/>
        public async Task<IList<ReleaseApproval>> GetApprovals(string account, string teamProject, VstsProfile profile)
        {
            if (account == null)
            {
                throw new ArgumentNullException(nameof(account));
            }

            if (string.IsNullOrWhiteSpace(teamProject))
            {
                throw new ArgumentNullException(nameof(teamProject));
            }

            if (profile == null)
            {
                throw new ArgumentNullException(nameof(profile));
            }

            OAuthToken token = profile.Token;

            using (var client = await GetConnectedClientAsync<ReleaseHttpClient2>(await this.GetAccountUriAsync(account, token), token))
            {
                return await client.GetApprovalsAsync2(teamProject, profile.Id.ToString());
            }
        }

        /// <inheritdoc/>
        public async Task<Profile> GetProfile(OAuthToken token)
        {
            if (token == null)
            {
                throw new ArgumentNullException(nameof(token));
            }

            using (var client = await GetConnectedClientAsync<ProfileHttpClient>(this.vstsAppUrl, token))
            {
                return await client.GetProfileAsync(new ProfileQueryContext(AttributesScope.Core));
            }
        }

        /// <inheritdoc />
        public async Task<IEnumerable<TeamProjectReference>> GetProjects(string account, OAuthToken token)
        {
            if (string.IsNullOrWhiteSpace(account))
            {
                throw new ArgumentNullException(nameof(account));
            }

            if (token == null)
            {
                throw new ArgumentNullException(nameof(token));
            }

            using (var client = await GetConnectedClientAsync<ProjectHttpClient>(await this.GetAccountUriAsync(account, token), token))
            {
                return await client.GetProjects();
            }
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<BuildDefinitionReference>> GetBuildDefinitionsAsync(string project, string account, OAuthToken token)
        {
            if (string.IsNullOrWhiteSpace(project))
            {
                throw new ArgumentNullException(nameof(project));
            }

            if (string.IsNullOrWhiteSpace(account))
            {
                throw new ArgumentNullException(nameof(account));
            }

            if (token == null)
            {
                throw new ArgumentNullException(nameof(token));
            }

            var teamProject = await this.GetProjectAsync(project, account, token);

            using (var client = await GetConnectedClientAsync<BuildHttpClient>(await this.GetAccountUriAsync(account, token), token))
            {
                return await client.GetDefinitionsAsync(teamProject.Id);
            }
        }

        /// <inheritdoc />
        public async Task CreateReleaseAsync(string account, string teamProject, int definitionId, OAuthToken token)
        {
            if (string.IsNullOrWhiteSpace(account))
            {
                throw new ArgumentNullException(nameof(account));
            }

            if (string.IsNullOrWhiteSpace(teamProject))
            {
                throw new ArgumentNullException(nameof(teamProject));
            }

            if (definitionId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(definitionId));
            }

            if (token == null)
            {
                throw new ArgumentNullException(nameof(token));
            }

            ReleaseDefinition definition;

            Uri accountUri = await this.GetAccountUriAsync(account, token);

            using (var client = await GetConnectedClientAsync<ReleaseHttpClient2>(accountUri, token))
            {
                definition = await client.GetReleaseDefinitionAsync(teamProject, definitionId);
            }

            Artifact artifact;
            Build build;

            using (var client = await GetConnectedClientAsync<BuildHttpClient>(accountUri, token))
            {
                artifact = definition.Artifacts.FirstOrDefault(a => a.IsPrimary);

                var builds = await client.GetBuildsAsync2(teamProject, new List<int> { Convert.ToInt32(artifact.DefinitionReference["definition"].Id) });
                build = builds.FirstOrDefault();
            }

            using (var client = await GetConnectedClientAsync<ReleaseHttpClient2>(accountUri, token))
            {
                var artifactMetaData = new ArtifactMetadata
                {
                    Alias = artifact.Alias,
                    InstanceReference = new BuildVersion { Id = build.Id.ToString() }
                };

                var metaData = new ReleaseStartMetadata { DefinitionId = definitionId, Artifacts = new List<ArtifactMetadata> { artifactMetaData } };
                await client.CreateReleaseAsync(metaData, teamProject);
            }
        }

        /// <summary>
        /// Gets a connected client.
        /// </summary>
        /// <typeparam name="T">The client type.</typeparam>
        /// <param name="accountUri">The URL to the account.</param>
        /// <param name="token">The OAuth token.</param>
        /// <returns>A client.</returns>
        private static async Task<T> GetConnectedClientAsync<T>(Uri accountUri, OAuthToken token)
            where T : VssHttpClientBase
        {
            var credentials = new VssOAuthAccessTokenCredential(new VssOAuthAccessToken(token.AccessToken));

            return await new VssConnection(accountUri, credentials).GetClientAsync<T>();
        }

        private Task<Uri> GetAccountUriAsync(string account, OAuthToken token)
        {
            // Uri accountUri = (await this.GetAccountAsync(account, token)).AccountUri;
            return Task.Run(() => new Uri(string.Format(CultureInfo.InvariantCulture, VstsUrl, account)));
        }

        /// <summary>
        /// Gets team project from VSTS account by name
        /// </summary>
        /// <param name="projectName">The team project name.</param>
        /// <param name="account">The VSTS Account name.</param>
        /// <param name="token">The <see cref="OAuthToken"/> for authentication.</param>
        /// <returns><see cref="TeamProjectReference"/></returns>
        private async Task<TeamProjectReference> GetProjectAsync(string projectName, string account, OAuthToken token)
        {
            var projects = await this.GetProjects(account, token);
            var project = projects.FirstOrDefault(p => string.Equals(p.Name, projectName, StringComparison.OrdinalIgnoreCase));

            if (project == default(TeamProjectReference))
            {
                throw new ArgumentOutOfRangeException(nameof(projectName), projectName, Exceptions.TeamProjectNotFound);
            }

            return project;
        }

        /// <summary>
        /// Gets VSTS account by name
        /// </summary>
        /// <param name="accountName">The VSTS Account name.</param>
        /// <param name="token">The <see cref="OAuthToken"/> for authentication.</param>
        /// <returns><see cref="TeamProjectReference"/></returns>
        private async Task<Account> GetAccountAsync(string accountName, OAuthToken token)
        {
            var profile = await this.GetProfile(token);
            var accounts = await this.GetAccounts(token, profile.Id);
            var account = accounts.FirstOrDefault(a => string.Equals(a.AccountName, accountName, StringComparison.OrdinalIgnoreCase));

            if (account == default(Account))
            {
                throw new ArgumentOutOfRangeException(nameof(accountName), accountName, Exceptions.AccountNotFound);
            }

            return account;
        }
    }
}