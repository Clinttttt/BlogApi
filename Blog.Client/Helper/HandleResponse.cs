using BlogApi.Domain.Common;
using Microsoft.Identity.Client;
using System.Net;
using System.Net.Http.Json;
using System.Runtime.InteropServices;
using System.Text.Json;

namespace BlogApi.Client.Helper
{
    public class HandleResponse
    {
        private readonly HttpClient httpClient;

        public HandleResponse(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<Result<TResponse>> UpdateAsync<TRequest, TResponse>(string url, TRequest value)
        {
            var response = await httpClient.PatchAsJsonAsync(url, value);
            return await MapStatusCodeAsync<TResponse>(response);
        }

        public async Task<Result<TResponse>> UpdateAsync<TResponse>(string url)
        {
            var response = await httpClient.PatchAsync(url, null);
            return await MapStatusCodeAsync<TResponse>(response);
        }

        public async Task<Result<TResponse>> DeleteAsync<TResponse>(string url)
        {
            var response = await httpClient.DeleteAsync(url);
            return await MapStatusCodeAsync<TResponse>(response);
        }

        public async Task<Result<TResponse>> DeleteAsync<TRequest, TResponse>(string url, TRequest value)
        {
            var request = new HttpRequestMessage(HttpMethod.Delete, url)
            {
                Content = JsonContent.Create(value)
            };

            var response = await httpClient.SendAsync(request);
            return await MapStatusCodeAsync<TResponse>(response);
        }

        public async Task<Result<TResponse>> GetAsync<TResponse>(string Url)
        {
            var response = await httpClient.GetAsync(Url);
            return await MapStatusCodeAsync<TResponse>(response);
        }

        public async Task<Result<TResponse>> PostAsync<TRequest, TResponse>(string url, TRequest value)
        {
            var response = await httpClient.PostAsJsonAsync(url, value);
            return await MapStatusCodeAsync<TResponse>(response);
        }

        public async Task<Result<TResponse>> PostAsync<TResponse>(string url)
        {
            var response = await httpClient.PostAsync(url, null);
            return await MapStatusCodeAsync<TResponse>(response);
        }

        public async Task<Result<TResponse>> MapStatusCodeAsync<TResponse>(HttpResponseMessage response)
        {
            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                var errorResponse = await response.Content.ReadFromJsonAsync<ValidationErrorResponse>();

                if (errorResponse?.Errors != null && errorResponse.Errors.Any())
                {
                   
                    return Result<TResponse>.ValidationFailure(errorResponse.Errors);
                }

                
                return Result<TResponse>.Failure("Bad request", 400);
            }



            return response.StatusCode switch
            {
                HttpStatusCode.OK => await HandleOkAsync<TResponse>(response),
                HttpStatusCode.Created => await HandleOkAsync<TResponse>(response),
                HttpStatusCode.NoContent => Result<TResponse>.NoContent(),
                HttpStatusCode.NotFound => Result<TResponse>.NotFound(),
                HttpStatusCode.Forbidden => Result<TResponse>.Forbidden(),
                HttpStatusCode.Unauthorized => Result<TResponse>.Unauthorized(),
                HttpStatusCode.Conflict => Result<TResponse>.Conflict(),
                HttpStatusCode.InternalServerError => await HandleErrorResponseAsync<TResponse>(response, 500),
                _ => await HandleErrorResponseAsync<TResponse>(response, (int)response.StatusCode)
            };
        }

        public async Task<Result<T>> HandleOkAsync<T>(HttpResponseMessage response)
        {
            var value = await response.Content.ReadFromJsonAsync<T>();
            if (value is null)
            {
                return Result<T>.Failure("Failed to deserialize response", 500);
            }
            return Result<T>.Success(value);
        }


        private async Task<Result<T>> HandleErrorResponseAsync<T>(HttpResponseMessage response, int statusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            var errorMessage = TryExtractErrorMessage(errorContent);
            return Result<T>.Failure(errorMessage, statusCode);
        }

        private string TryExtractErrorMessage(string jsonContent)
        {
            if (string.IsNullOrWhiteSpace(jsonContent))
                return jsonContent;

            try
            {
                using var doc = JsonDocument.Parse(jsonContent);
                var root = doc.RootElement;

                if (root.TryGetProperty("errors", out var errors) && errors.ValueKind == JsonValueKind.Object)
                {
                    var messages = errors.EnumerateObject()
                        .SelectMany(e => e.Value.EnumerateArray().Select(m => m.GetString()))
                        .Where(m => !string.IsNullOrWhiteSpace(m))
                        .ToList();

                    if (messages.Any())
                        return string.Join("; ", messages);
                }
            }
            catch
            {

            }

            return jsonContent;
        }

        public class ValidationErrorResponse
        {
            public Dictionary<string, string[]>? Errors { get; set; }
        }
    }
}