using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace Shogendar.Karikari.App;

class APIClient
{
    /// <summary>
    /// Karikari バックエンドのベースURL
    /// </summary>
    private readonly string m_baseUrl;
    /// <summary>
    /// 通信に使用するHttpClient
    /// </summary>
    private readonly HttpClient httpClient;
    private readonly string m_token;
    private readonly string m_secret;

    /// <summary>
    /// Karikari バックエンドを叩くクライアントを生成します
    /// </summary>
    /// <param name="baseUrl">Karikari バックエンドのベースURL</param>
    /// <param name="token">リクエストをするユーザーのトークン</param>
    /// <param name="secret">リクエストをするユーザーのシークレット</param>
    public APIClient(string baseUrl, string token, string secret)
    {
        this.m_baseUrl = baseUrl;
        this.httpClient = new HttpClient();
        this.m_token = token;
        this.m_secret = secret;
    }
    /// <summary>
    /// HMAC-SHA256で署名されたHttpリクエストを作成します
    /// </summary>
    /// <param name="method">リクエストメソッド</param>
    /// <param name="requestEndPoint">リクエスト先のエンドポイント</param>
    /// <returns>作成したHttpリクエスト</returns>
    private HttpRequestMessage CreateRequest(HttpMethod method, string requestEndPoint)
    {
        string nonce = Guid.NewGuid().ToString();
        long t = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

        string stringToSign = $"{m_token}{t}{nonce}";
        string sign = ComputeHmacSha256(m_secret, stringToSign);

        HttpRequestMessage request = new HttpRequestMessage(method, $"{this.m_baseUrl}/{requestEndPoint}");

        request.Headers.Add("Authorization", m_token);
        request.Headers.Add("charset", "utf8");
        request.Headers.Add("t", t.ToString());
        request.Headers.Add("sign", sign);
        request.Headers.Add("nonce", nonce);

        return request;
    }
    /// <summary>
    /// HMAC-SHA256で指定したメッセージをハッシュ化します
    /// </summary>
    /// <param name="secret">シークレット</param>
    /// <param name="message">メッセージ</param>
    /// <returns>ハッシュ化されたメッセージをBase64エンコードした文字列</returns>
    private string ComputeHmacSha256(string secret, string message)
    {
        using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(secret)))
        {
            byte[] hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(message));
            return Convert.ToBase64String(hashBytes);
        }
    }

}