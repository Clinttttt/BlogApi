using BlogApi.Domain.Common;
using Microsoft.Identity.Client;
using System.Net;
using System.Runtime.InteropServices;

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
            return response.StatusCode switch
            {
                HttpStatusCode.OK => await HandleOkAsync<TResponse>(response),
                HttpStatusCode.Created => await HandleOkAsync<TResponse>(response), 
                HttpStatusCode.NoContent => Result<TResponse>.Success(default!), 
                HttpStatusCode.NotFound => Result<TResponse>.NotFound(),
                HttpStatusCode.Forbidden => Result<TResponse>.Forbidden(),
                HttpStatusCode.Unauthorized => Result<TResponse>.Unauthorized(),
                HttpStatusCode.Conflict => Result<TResponse>.Conflict(),
                HttpStatusCode.BadRequest => Result<TResponse>.Failure(), 
                HttpStatusCode.InternalServerError => Result<TResponse>.Failure(), 
                _ => Result<TResponse>.Failure()
            };
        }
        public async Task<Result<T>> HandleOkAsync<T>(HttpResponseMessage response)
        {
            var value = await response.Content.ReadFromJsonAsync<T>();
            if (value is null)
            {
                return Result<T>.Failure();
            }
            return Result<T>.Success(value);
        }
    }
}
