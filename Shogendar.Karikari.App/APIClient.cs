using System.Net;
using System.Security.Cryptography;
using System.Linq;
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
    public static APIClient Instance { get; set; }
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
    /// 現在のユーザーにかかるLoanの一覧を取得します
    /// </summary>
    /// <returns></returns>
    public async Task<IEnumerable<Loan>> GetLoans(User repayer)
    {
#if DEBUG
        await Task.Delay(500);
        return MockLoans.Where(l => l.RepayerId == repayer.Id);
#else
        HttpRequestMessage request = CreateRequest(HttpMethod.Get, "loans");
        HttpResponseMessage response = await httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();
        string responseBody = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<List<Loan>>(responseBody);
#endif
    }
    public async Task<Loan> GetLoan(int id, User user)
    {
        await Task.Delay(500);
        return MockLoans.Where(l => l.Id == id && (l.PayerId == user.Id || l.RepayerId == user.Id)).FirstOrDefault();
    }
    internal static List<Loan> MockLoans
    {
        get
        {
            return
            [
                new() { Id = 1, Amount = 100, Event = 1, PayerId = 1, RepayerId = 2, Payer = MockUsers[0], Repayer = MockUsers[1] },
                new() { Id = 2, Amount = 200, Event = 2, PayerId = 2, RepayerId = 1, Payer = MockUsers[1], Repayer = MockUsers[0] },
                new() { Id = 3, Amount = 300, Event = 3, PayerId = 1, RepayerId = 2, Payer = MockUsers[0], Repayer = MockUsers[1] },
                new() { Id = 4, Amount = 400, Event = 4, PayerId = 2, RepayerId = 1, Payer = MockUsers[1], Repayer = MockUsers[0] },
                new() { Id = 5, Amount = 500, Event = 5, PayerId = 1, RepayerId = 2, Payer = MockUsers[0], Repayer = MockUsers[1] },
            ];
        }
    }
    internal static List<User> MockUsers
    {
        get
        {
            return
            [
                new() { Id = 1, Name = "Alice" },
                new() { Id = 2, Name = "Bob" },
                new() { Id = 3, Name = "Charlie" },
                new() { Id = 4, Name = "David" },
                new() { Id = 5, Name = "Eve" },
            ];
        }
    }

}