using Google.Apis.Auth.OAuth2.Responses;
using GoogleCalenderApplication.Application.Abstractions;
using GoogleCalenderApplication.Domain.Abstractions;
using GoogleCalenderApplication.Domain.DTOs;
using GoogleCalenderApplication.Domain.Models;
using MapsterMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleCalenderApplication.Application.Services
{
    internal class GoogleOAuthService : IGoogleOAuthService
    {
        #region private

        private readonly HttpClient _httpClient;
        private readonly string scopeURL1 = "https://accounts.google.com/o/oauth2/auth?redirect_uri={0}&prompt={1}&response_type={2}&client_id={3}&scope={4}&access_type={5}";
        private readonly string redirectURL = "https://localhost:7234/auth/callback";
        private readonly string prompt = "consent";
        private readonly string response_type = "code";
        private readonly string clientId = "245177443031-0dq9gnmade809hp5jpqj2j69a9ekdb9j.apps.googleusercontent.com";
        private readonly string scope = "https://www.googleapis.com/auth/calendar+https://www.googleapis.com/auth/calendar.events";
        private readonly string access_type = "offline";
        private readonly string redirect_uri_encode = "https://localhost:7234/auth/callback";
        private readonly string clientSecret = "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";
        private readonly string tokenEndpoint = "https://accounts.google.com/o/oauth2/token";
        private readonly string RefreshEndpoint = "https://oauth2.googleapis.com/token";
        private readonly string RevokeEndpoint = "https://oauth2.googleapis.com/revoke";

        private readonly IGenericRepository<Token> _tokenRepo;
        private readonly UserManager<User> _userManager;
        #endregion

        public GoogleOAuthService(IGenericRepository<Token> tokenRepo,  UserManager<User> userManager)
        {
            _httpClient = new HttpClient();
            _tokenRepo = tokenRepo;
            _userManager = userManager;
        }
        public string GetAuthCode(HttpContext httpContext , string email)
        {
            try
            {
                var mainURL = string.Format(scopeURL1, redirect_uri_encode, prompt, response_type, clientId, scope, access_type);

                httpContext.Response.Cookies.Append("UserEmail", email);

                return mainURL;
            }
            catch (Exception ex) 
            { return ex.ToString(); }
        }
        public async Task<ResponseModel<ResponseTokenDto>> GetToken(string code , string email)
        {
           
            var content = new StringContent($"code={code}&redirect_uri={Uri.EscapeDataString(redirectURL)}&client_id={clientId}&client_secret={clientSecret}&grant_type=authorization_code", Encoding.UTF8, "application/x-www-form-urlencoded");

            var response = await _httpClient.PostAsync(tokenEndpoint, content);
            var responseContent = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                var tokenResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<ResponseTokenDto>(responseContent);

                var user = await _userManager.FindByEmailAsync(email);
                Token token = new Token()
                {
                    access_token = tokenResponse.access_token,
                    expires_in = tokenResponse.expires_in,
                    refresh_token = tokenResponse.refresh_token,
                    scope = tokenResponse.scope,
                    token_type = tokenResponse.token_type,
                    UserId = user.Id,
                };

                await _tokenRepo.Add(token);
                await _tokenRepo.Save();

                return ResponseModel<ResponseTokenDto>.Success(tokenResponse);
            }
            else
            {
                return ResponseModel<ResponseTokenDto>.Error($"Failed to authenticate: {responseContent}");
            }
        }

        public async Task<ResponseModel<string>> GetNewRefreshToken(string refeshToken,string userId)
        {
            var content = new StringContent($"client_id={clientId}&client_secret={clientSecret}&refresh_token={refeshToken}&grant_type=refresh_token", Encoding.UTF8, "application/x-www-form-urlencoded");
            
            var response = await _httpClient.PostAsync(RefreshEndpoint, content);
            
            var responseContent = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                return ResponseModel<string>.Success(responseContent);
            }
            else
            {
                return ResponseModel<string>.Error();
            }
        }


    }
}
