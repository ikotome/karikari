using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();

// 設定ファイルを読み込む
builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
builder.Configuration.AddEnvironmentVariables();

// Google認証情報を設定ファイルから取得
var googleAuthSection = builder.Configuration.GetSection("Authentication:Google");
var clientId = googleAuthSection["ClientId"];
var clientSecret = googleAuthSection["ClientSecret"];

// 認証を追加
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
})
.AddCookie()
.AddGoogle(googleOptions =>
{
    googleOptions.ClientId = clientId;
    googleOptions.ClientSecret = clientSecret;
    googleOptions.CallbackPath = "/signin-google"; // リダイレクトURI
});

// CORS ポリシーを追加
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy =>
        {
            policy.AllowAnyOrigin()   // すべてのオリジンを許可
                  .AllowAnyMethod()   // すべての HTTP メソッドを許可
                  .AllowAnyHeader();  // すべてのヘッダーを許可
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll"); // CORS を有効化

app.UseAuthentication();    // 認証を使えるようにする
app.UseAuthorization();

app.UseHttpsRedirection();

app.MapControllers();
app.Run();
