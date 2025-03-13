using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Shogendar.Karikari.Models;

namespace Shogendar.Karikari.App;

/// <summary>
/// Karikari バックエンドを叩くクライアント
/// </summary>
/// <param name="baseUrl">Karikari バックエンドのベースURL</param>
/// <param name="token">リクエストをするユーザーのトークン</param>
/// <param name="secret">リクエストをするユーザーのシークレット</param>
class APIClient(string baseUrl, string token, string secret)
{
    /// <summary>
    /// Karikari バックエンドのベースURL
    /// </summary>
    private readonly string m_baseUrl = baseUrl;
    /// <summary>
    /// 通信に使用するHttpClient
    /// </summary>
    private static readonly HttpClient httpClient = new();
    private readonly string m_token = token;
    private readonly string m_secret = secret;

    /// <summary>
    /// HMAC-SHA256で署名されたHttpリクエストを作成します
    /// </summary>
    /// <param name="method">リクエストメソッド</param>
    /// <param name="requestEndPoint">リクエスト先のエンドポイント</param>
    /// <returns>作成したHttpリクエスト</returns>
    private HttpRequestMessage CreateRequest(HttpMethod method, string requestEndPoint, Dictionary<string, string> queryParameters = null)
    {
        string nonce = Guid.NewGuid().ToString();
        long t = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

        string stringToSign = $"{m_token}{t}{nonce}";
        string sign = ComputeHmacSha256(m_secret, stringToSign);

        HttpRequestMessage request = new HttpRequestMessage(method, $"{this.m_baseUrl}/{requestEndPoint}?{new FormUrlEncodedContent(queryParameters).ReadAsStringAsync().Result}");

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
        using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(secret));
        byte[] hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(message));
        return Convert.ToBase64String(hashBytes);
    }
    /// <summary>
    /// 現在のユーザーがもつ債務を取得します
    /// </summary>
    /// <returns></returns>
    public async Task<List<Loan>> GetLoans()
    {
        HttpRequestMessage request = CreateRequest(HttpMethod.Get, "loans");
        HttpResponseMessage response = await httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();
        string responseBody = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<List<Loan>>(responseBody);
    }
    /// <summary>
    /// 現在のユーザーがもつ債権を取得します
    /// </summary>
    /// <returns></returns>
    public async Task<List<Loan>> GetReceivables()
    {
        HttpRequestMessage request = CreateRequest(HttpMethod.Get, "receivables");
        HttpResponseMessage response = await httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();
        string responseBody = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<List<Loan>>(responseBody);
    }

}