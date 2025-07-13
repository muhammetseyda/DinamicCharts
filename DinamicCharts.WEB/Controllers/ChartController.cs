using DinamicCharts.WEB.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace DinamicCharts.WEB.Controllers
{
    public class ChartController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public ChartController(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            var username = HttpContext.Session.GetString("Username");
            if (string.IsNullOrEmpty(username))
                return RedirectToAction("Login");
            return View();
        }
        public async Task<IActionResult> Login(string username, string password)
        {
            var client = _httpClientFactory.CreateClient();

            var apiUrl = _configuration["ApiBaseUrl"] + "/api/token";

            var loginRequest = new LoginRequestDto
            {
                Username = username,
                Password = password
            };

            var json = JsonSerializer.Serialize(loginRequest);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(apiUrl, content);

            if (response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                var tokenResponse = JsonSerializer.Deserialize<TokenResponseDto>(responseBody);

                if (tokenResponse != null && !string.IsNullOrEmpty(tokenResponse.Token))
                {
                    HttpContext.Session.SetString("Username", username);
                    HttpContext.Session.SetString("Token", tokenResponse.Token);

                    return RedirectToAction("Index"); 
                }
            }

            if(loginRequest.Username != null && loginRequest.Password != null){
                ViewBag.Message = "Giriş başarısız. Kullanıcı adı veya şifre hatalı.";

            }
            return View("Login");
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear(); 
            return RedirectToAction("Login"); 
        }

        [HttpPost]
        public async Task<IActionResult> GetViews([FromBody] ConnectionInfoDto dto)
        {
            var token = HttpContext.Session.GetString("Token");

            if (string.IsNullOrEmpty(token))
                return Unauthorized(new { error = "Token bulunamadı. Lütfen tekrar giriş yapın." });

            var client = _httpClientFactory.CreateClient();
            var apiUrl = _configuration["ApiBaseUrl"] + "/api/inspect/views";

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var json = JsonSerializer.Serialize(dto);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            var response = await client.PostAsync(apiUrl, content);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                return Json(JsonSerializer.Deserialize<object>(result));
            }

            return Json(new { error = "Failed to fetch views" });
        }

        [HttpPost]
        public async Task<IActionResult> GetProcedures([FromBody] ConnectionInfoDto dto)
        {
            var token = HttpContext.Session.GetString("Token");

            if (string.IsNullOrEmpty(token))
                return Unauthorized(new { error = "Token bulunamadı. Lütfen tekrar giriş yapın." });
            var client = _httpClientFactory.CreateClient();
            var apiUrl = _configuration["ApiBaseUrl"] + "/api/inspect/procedures";

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var json = JsonSerializer.Serialize(dto);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            var response = await client.PostAsync(apiUrl, content);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                return Json(JsonSerializer.Deserialize<object>(result));
            }

            return Json(new { error = "Failed to fetch procedures" });
        }

        [HttpPost]
        public async Task<IActionResult> GetFunctions([FromBody] ConnectionInfoDto dto)
        {
            var token = HttpContext.Session.GetString("Token");

            if (string.IsNullOrEmpty(token))
                return Unauthorized(new { error = "Token bulunamadı. Lütfen tekrar giriş yapın." });
            var client = _httpClientFactory.CreateClient();
            var apiUrl = _configuration["ApiBaseUrl"] + "/api/inspect/functions";

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var json = JsonSerializer.Serialize(dto);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            var response = await client.PostAsync(apiUrl, content);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                return Json(JsonSerializer.Deserialize<object>(result));
            }

            return Json(new { error = "Failed to fetch functions" });
        }

        [HttpPost]
        public async Task<IActionResult> ExecuteObject([FromBody] ExecuteObjectRequest request)
        {
            var token = HttpContext.Session.GetString("Token");

            if (string.IsNullOrEmpty(token))
                return Unauthorized(new { error = "Token bulunamadı. Lütfen tekrar giriş yapın." });
            var client = _httpClientFactory.CreateClient();
            var apiUrl = _configuration["ApiBaseUrl"] + "/api/execute-object";

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var json = JsonSerializer.Serialize(request);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            var response = await client.PostAsync(apiUrl, content);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                return Json(JsonSerializer.Deserialize<object>(result));
            }

            return Json(new { error = "Failed to execute object" });
        }
    }

    public class ConnectionInfoDto
    {
        public string Host { get; set; }
        public string DbAdi { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class ExecuteObjectRequest
    {
        public string Host { get; set; }
        public string DbAdi { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string ObjectType { get; set; }
        public string ObjectName { get; set; }

        public ConnectionInfoDto ToConnectionInfoDto()
        {
            return new ConnectionInfoDto
            {
                Host = Host,
                DbAdi = DbAdi,
                Username = Username,
                Password = Password
            };
        }
    }
}
