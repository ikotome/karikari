using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Shogendar.Karikari.Models;

namespace Shogendar.Karikari.App;

class LocalUser
{
    public static LocalUser Instance
    {
        get
        {
            m_instance ??= new LocalUser();
            return m_instance;
        }
    }
    private static LocalUser m_instance;
    private string? m_token;
    public string? Token
    {
        get
        {
#if IOS || MACCATALYST
            // iOS と MacOSでは、SecureStorageの使用にAppleDeveloper登録が必要なため、
            // 安全ではないが、Preferencesを使う
            m_token ??= Preferences.Get("user_token", m_token);
#else
            m_token ??= SecureStorage.GetAsync("user_token").Result;
#endif
            return m_token;
        }
        set
        {
            m_token = value;
#if IOS || MACCATALYST
            // iOS と MacOSでは、SecureStorageの使用にAppleDeveloper登録が必要なため、
            // 安全ではないが、Preferencesを使う
            Preferences.Set("user_token", value);
#else
            SecureStorage.Default.SetAsync("user_token", value).Wait();
#endif
        }
    }
    public bool LoggedIn()
    {
        return Token is not null;
    }
    public async Task Login()
    {
#if DEBUG
        // デバッグ時は、テストキーを与えてログインをモックする
        Token = "DUMMY_TOKEN";
#else
        WebAuthenticatorResult authResult = await WebAuthenticator.Default.AuthenticateAsync(
        new WebAuthenticatorOptions()
        {
            Url = new Uri("https://api.wsoft.ws/auth/google"),
            CallbackUrl = new Uri("karikari://"),
            PrefersEphemeralWebBrowserSession = true
        });
        Token = authResult?.AccessToken;
#endif
    }
    public void Logout()
    {
        Token = null;
    }
    public User User { get; set; } = APIClient.MockUsers.First();
}
