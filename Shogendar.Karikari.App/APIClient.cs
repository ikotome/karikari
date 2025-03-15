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
    public static UserComparer UserComparer { get; set; } = new();
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
    /// 現在のユーザーが返すべきローンを取得します
    /// </summary>
    /// <returns></returns>
    public async Task<List<Loan>> GetLoans(User currentUser, User targetUser = null)
    {
#if DEBUG
        await Task.Delay(500);
        if (targetUser is null)
            return MockLoans.Where(l => l.PayerId == currentUser.Id).ToList();
        else
            return MockLoans.Where(l => l.PayerId == currentUser.Id && l.RepayerId == targetUser.Id).ToList();
#else
        HttpRequestMessage request = CreateRequest(HttpMethod.Get, "loans");
        HttpResponseMessage response = await httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();
        string responseBody = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<List<Loan>>(responseBody);
#endif
    }
    /// <summary>
    /// 現在のユーザーが貸しているローンを取得します
    /// </summary>
    /// <param name="payer"></param>
    /// <returns></returns>
    public async Task<List<Loan>> GetReturns(User payer)
    {
        await Task.Delay(500);
        return MockLoans.Where(l => l.PayerId == payer.Id).ToList();
    }
    public async Task<Loan> PutLoanAsync(Loan loan)
    {
        return await PutLoanAsync(loan.Title, loan.Description, loan.PayerId, loan.RepayerId, loan.Amount, loan.PayDate, loan.RepayDate, loan.Type, loan.Method);
    }
    public async Task<Loan> PutLoanAsync(string title, string description, int payerId, int repayerId, decimal amount, DateTime paydate, DateTime repaydate, LoanType type, PaymentMethod method)
    {
        HttpResponseMessage response = await httpClient.PutAsync($"{m_baseUrl}/Loan?id=0&title={title}&description={description}&payerId={payerId}&repayerId={repayerId}&amount={amount}&paydate={paydate}&repaydate={repaydate}&type={(int)type}&method={(int)method}", null);
        response.EnsureSuccessStatusCode();
        string responseBody = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<Loan>(responseBody);
    }
    public async Task<Loan> GetLoanAsync(int id)
    {
        HttpResponseMessage response = await httpClient.GetAsync($"{m_baseUrl}/Loan?id={id}");
        response.EnsureSuccessStatusCode();
        string responseBody = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<Loan>(responseBody);
    }

    public async Task<User> GetUserAsync(string name)
    {
        HttpResponseMessage response = await httpClient.GetAsync($"{m_baseUrl}/User?name={name}");
        response.EnsureSuccessStatusCode();
        string responseBody = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<User>(responseBody);
    }
    public async Task<User> GetUserAsync(int id)
    {
        HttpResponseMessage response = await httpClient.GetAsync($"{m_baseUrl}/User?id={id}");
        response.EnsureSuccessStatusCode();
        string responseBody = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<User>(responseBody);
    }
    public async Task<List<User>> GetUsersAsync(User user, bool isReturn)
    {
        HttpResponseMessage response = await httpClient.GetAsync($"{m_baseUrl}/GetUsers?id={user.Id}&userType={isReturn}");
        response.EnsureSuccessStatusCode();
        string responseBody = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<List<User>>(responseBody);
    }
    public async Task<List<Loan>> GetLoansAsync(User my, User other, bool isReturn)
    {
        HttpResponseMessage response = await httpClient.GetAsync($"{m_baseUrl}/GetLoans?myId={my.Id}&otherId={other.Id}&userType={isReturn}");
        response.EnsureSuccessStatusCode();
        string responseBody = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<List<Loan>>(responseBody);
    }
    internal static List<Loan> MockLoans
    {
        get
        {
            return
            [
                new() { Id = 1, Amount = 100, Title = "マクド", Description = "マクド サイゼ カプチョ", Type = LoanType.Lent, PayerId = 1, RepayerId = 2, Payer = MockUsers[0], Repayer = MockUsers[1] },
                new() { Id = 2, Amount = 200, Title = "ギター", Description = "ギター ベース ドラム", Type = LoanType.Lent, PayerId = 2, RepayerId = 1, Payer = MockUsers[1], Repayer = MockUsers[0] },
                new() { Id = 3, Amount = 300, Title = "タコス", Description = "タコス ココス コスタ", Type = LoanType.Return, PayerId = 1, RepayerId = 3, Payer = MockUsers[0], Repayer = MockUsers[2] },
                new() { Id = 4, Amount = 400, Title = "コンビニ", Description = "コンビニ ミラクル ミルク", Type = LoanType.Return, PayerId = 2, RepayerId = 2, Payer = MockUsers[2], Repayer = MockUsers[0] },
                new() { Id = 5, Amount = 500, Title = "ティラミス", Description = "ティラミス ヨウセイ アマイ", Type = LoanType.Return, PayerId = 1, RepayerId = 2, Payer = MockUsers[0], Repayer = MockUsers[1] },
            ];
        }
    }
    internal static List<User> MockUsers
    {
        get
        {
            return
            [
                new() { Id = 1, Name = "taiseiue" },
                new() { Id = 2, Name = "ikotome" },
                new() { Id = 3, Name = "openHackU" },
            ];
        }
    }

}