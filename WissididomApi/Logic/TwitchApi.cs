using System.Net;
using System.Net.Http.Headers;
using System.Reflection;
using WissididomApi.JsonModels.Twitch;

namespace WissididomApi.Logic;

public class TwitchApi
{
    private readonly string _clientId;
    private readonly string _clientSecret;
    private AppAuthenticationResponse? _authData;
    private readonly HttpClient _client = new();

    public TwitchApi(string clientId, string clientSecret, string? userAgent = null)
    {
        this._clientId = clientId;
        this._clientSecret = clientSecret;
        _client.DefaultRequestHeaders.Add("Accept", "application/json");
        userAgent ??= $"WissididomApi/${Assembly.GetExecutingAssembly().GetName().Version}";
        _client.DefaultRequestHeaders.Add("User-Agent", userAgent);
    }
    
    public async Task<AppAuthenticationResponse?> Authenticate()
    {
        const string url = "https://id.twitch.tv/oauth2/token";
        var body = new Dictionary<string, string>
        {
            {"client_id", _clientId},
            {"client_secret", _clientSecret},
            {"grant_type", "client_credentials"},
        };
        var response = await _client.PostAsync(url, new FormUrlEncodedContent(body));
        if (!response.IsSuccessStatusCode)
        {
            return new AppAuthenticationResponse
            {
                Error = await response.Content.ReadFromJsonAsync<TwitchError>(),
            };
        }
        this._authData = await response.Content.ReadFromJsonAsync<AppAuthenticationResponse>();
        return this._authData;
    }

    public async Task<TwitchUser[]?> GetUsers(string[]? ids = null, string[]? logins = null)
    {
        if (_authData is null)
        {
            await Authenticate();
            if (_authData is null)
                throw new Exception("Failed to authenticate with Twitch API.");
        }
        List<string> queryParams = [];
        if (ids is not null)
            queryParams.AddRange(ids.Select(id => $"id={id}"));
        if (logins is not null)
            queryParams.AddRange(logins.Select(login => $"login={login}"));
        var url = $"https://api.twitch.tv/helix/users?{string.Join('&', queryParams)}";
        using var request = new HttpRequestMessage(HttpMethod.Get, url);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _authData.AccessToken);
        request.Headers.Add("Client-ID", _clientId);
        var response = await _client.SendAsync(request);
        if (!response.IsSuccessStatusCode)
        {
            if (response.StatusCode != HttpStatusCode.Unauthorized)
            {
                return [new TwitchUser()
                {
                    Error = await response.Content.ReadFromJsonAsync<TwitchError>(),
                }];
            }
            Console.WriteLine($"{response.StatusCode}: {await response.Content.ReadAsStringAsync()}");
            await Authenticate();
            return await GetUsers(ids, logins);
        }
        var responseObject  = await response.Content.ReadFromJsonAsync<GetUsersResponse>();
        return responseObject?.Data?.Length < 1 ? [] : responseObject?.Data;
    }

    public async Task<ChatSettings[]?> GetChatSettings(string broadcasterId, string? moderatorId = null)
    {
        if (_authData is null)
        {
            await Authenticate();
            if (_authData is null)
                throw new Exception("Failed to authenticate with Twitch API.");
        }
        const string url = "https://api.twitch.tv/helix/chat/settings";
        var query = moderatorId is null ? $"broadcaster_id={broadcasterId}" : $"broadcaster_id={broadcasterId}&moderator_id={moderatorId}";
        using var request = new HttpRequestMessage(HttpMethod.Get, $"{url}?{query}");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _authData.AccessToken);
        request.Headers.Add("Client-ID", _clientId);
        var response = await _client.SendAsync(request);
        if (!response.IsSuccessStatusCode)
        {
            if (response.StatusCode != HttpStatusCode.Unauthorized)
            {
                return [new ChatSettings
                {
                    Error = await response.Content.ReadFromJsonAsync<TwitchError>(),
                }];
            }
            Console.WriteLine($"{response.StatusCode}: {await response.Content.ReadAsStringAsync()}");
            await Authenticate();
            return await GetChatSettings(broadcasterId, moderatorId);
        }
        var responseObject  = await response.Content.ReadFromJsonAsync<GetChatSettingsResponse>();
        return responseObject?.Data?.Length < 1 ? [] : responseObject?.Data;
    }
}
