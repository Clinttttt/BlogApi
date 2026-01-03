using BlogApi.Application.Common.Interfaces;
using BlogApi.Application.Models;
using BlogApi.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BlogApi.Infrastructure.Services
{
    public class GoogleTokenValidator : IGoogleTokenValidator
    {

        private readonly HttpClient _httpClient;

        public GoogleTokenValidator(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<Result<GoogleUserInfo>?> ValidateAsync(string idToken)
        {
            try
            {

                var response = await _httpClient.GetAsync(
                    $"https://oauth2.googleapis.com/tokeninfo?id_token={idToken}");

                if (response is null)
                    return Result<GoogleUserInfo>.NotFound();

                var json = await response.Content.ReadFromJsonAsync<JsonElement>();

                return Result<GoogleUserInfo>.Success(new GoogleUserInfo(

                json.GetProperty("sub").GetString()!,
                json.GetProperty("email").GetString()!,
                json.GetProperty("name").GetString()!,
                json.GetProperty("picture").GetString()
                    ));

            }
            catch
            {
                return Result<GoogleUserInfo>.NotFound();
            }
        }
    }
}
