using System.Security.Claims;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;

namespace Esport.Client.Services
{
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly ILocalStorageService _localStorage;
        private ClaimsPrincipal _anonymous = new ClaimsPrincipal(new ClaimsIdentity());
        private ClaimsPrincipal _currentUser;

        public CustomAuthenticationStateProvider(ILocalStorageService localStorage)
        {
            _localStorage = localStorage;
            _currentUser = _anonymous;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            // Odczytaj zapisane dane, np. nazwę użytkownika i rolę
            var userName = await _localStorage.GetItemAsync<string>("userName");
            var role = await _localStorage.GetItemAsync<string>("role");

            if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(role))
            {
                return new AuthenticationState(_anonymous);
            }

            var identity = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, userName),
                new Claim(ClaimTypes.Role, role)
            }, "apiauth_type");

            _currentUser = new ClaimsPrincipal(identity);
            return new AuthenticationState(_currentUser);
        }

        public async Task MarkUserAsAuthenticated(string userName, string role)
        {
            // Zapisz dane w localStorage
            await _localStorage.SetItemAsync("userName", userName);
            await _localStorage.SetItemAsync("role", role);

            var identity = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, userName),
                new Claim(ClaimTypes.Role, role)
            }, "apiauth_type");

            _currentUser = new ClaimsPrincipal(identity);
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_currentUser)));
        }

        public async Task MarkUserAsLoggedOut()
        {
            await _localStorage.RemoveItemAsync("userName");
            await _localStorage.RemoveItemAsync("role");

            _currentUser = _anonymous;
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_currentUser)));
        }
    }
}
