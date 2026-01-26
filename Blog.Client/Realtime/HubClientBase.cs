using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using BlogApi.Client.Security;
using BlogApi.Client.Common.Auth;

namespace Blog.Client.Realtime
{
    public abstract class HubClientBase : IAsyncDisposable
    {
        protected readonly NavigationManager _navigationManager;
        protected readonly string _apiBaseUrl;
        protected readonly ITokenService _tokenService;

        private HubConnection? _hubConnection;
        private readonly SemaphoreSlim _initLock = new(1, 1);
        private bool _disposed;

        public event Action? OnReconnecting;
        public event Action<string?>? OnReconnected;
        public event Action<Exception?>? OnClosed;

        public HubConnectionState? ConnectionState => _hubConnection?.State;
        public bool IsConnected => _hubConnection?.State == HubConnectionState.Connected;

        protected HubClientBase(
            NavigationManager navigationManager,
            IConfiguration configuration,
            ITokenService tokenService)
        {
            _navigationManager = navigationManager;
            _apiBaseUrl = Environment.GetEnvironmentVariable("DockerHost") ?? configuration["LocalHost"]!;
             

            _tokenService = tokenService;
        }

        protected async Task InitializeAsync(string hubRelativePath)
        {
            if (_disposed) return;

            await _initLock.WaitAsync();
            try
            {
                if (_disposed) return;
                if (_hubConnection != null) return;

                var hubUrl = $"{_apiBaseUrl.TrimEnd('/')}/{hubRelativePath.TrimStart('/')}";

                _hubConnection = new HubConnectionBuilder()
                    .WithUrl(hubUrl, options =>
                    {
                        options.AccessTokenProvider = () => _tokenService.GetAccessTokenAsync();
                    })
                    .WithAutomaticReconnect(new[] { TimeSpan.Zero, TimeSpan.FromSeconds(2), TimeSpan.FromSeconds(10) })
                    .Build();

                RegisterEventHandlers();

                await _hubConnection.StartAsync();
            }
            catch (ObjectDisposedException)
            {
              
            }
            catch
            {
                throw;
            }
            finally
            {
                if (!_disposed)
                {
                    _initLock.Release();
                }
            }
        }

        private void RegisterEventHandlers()
        {
            if (_hubConnection == null) return;

            _hubConnection.Reconnecting += error =>
            {
                OnReconnecting?.Invoke();
                return Task.CompletedTask;
            };

            _hubConnection.Reconnected += connectionId =>
            {
                OnReconnected?.Invoke(connectionId);
                return Task.CompletedTask;
            };

            _hubConnection.Closed += error =>
            {
                OnClosed?.Invoke(error);
                return Task.CompletedTask;
            };
        }

        protected void Subscribe(string method, Action handler)
        {
            EnsureConnected();
            _hubConnection!.On(method, handler);
        }

        protected void Subscribe<T>(string method, Action<T> handler)
        {
            EnsureConnected();
            _hubConnection!.On(method, handler);
        }

        protected void Subscribe<T1, T2>(string method, Action<T1, T2> handler)
        {
            EnsureConnected();
            _hubConnection!.On(method, handler);
        }

        protected void Subscribe<T1, T2, T3>(string method, Action<T1, T2, T3> handler)
        {
            EnsureConnected();
            _hubConnection!.On(method, handler);
        }

        protected async Task InvokeAsync(string method, params object?[] args)
        {
            EnsureConnected();
            await _hubConnection!.InvokeAsync(method, args);
        }

        protected async Task<T> InvokeAsync<T>(string method, params object?[] args)
        {
            EnsureConnected();
            return await _hubConnection!.InvokeAsync<T>(method, args);
        }

        private void EnsureConnected()
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(HubClientBase));

            if (_hubConnection == null)
                throw new InvalidOperationException("Hub not initialized. Call InitializeAsync first.");
        }

        public async ValueTask DisposeAsync()
        {
            if (_disposed) return;

            _disposed = true;

            OnReconnecting = null;
            OnReconnected = null;
            OnClosed = null;

            if (_hubConnection != null)
            {
                try
                {
                    await _hubConnection.DisposeAsync();
                }
                catch
                {
                    // Ignore disposal errors
                }
                _hubConnection = null;
            }

            try
            {
                _initLock.Dispose();
            }
            catch (ObjectDisposedException)
            {
                // Already disposed, ignore
            }

            GC.SuppressFinalize(this);
        }
    }
}