 public class PaystackService
    {
        private readonly HttpClient _httpClient;
        private readonly string _secretKey;

        public PaystackService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _secretKey = configuration["Paystack:SecretKey"];
        }

        public async Task<PaystackPaymentResponse> InitializePaymentAsync(decimal amount, string email)
        {
            var requestBody = new
            {
                amount = (int)(amount * 100), // Convert amount to kobo
                email = email,
                callback_url = "https://yourdomain.com/api/payments/verify"
            };

            var requestContent = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _secretKey);

            var response = await _httpClient.PostAsync("https://api.paystack.co/transaction/initialize", requestContent);

            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<PaystackPaymentResponse>(responseContent);
        }

        public async Task<PaystackVerificationResponse> VerifyPaymentAsync(string reference)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _secretKey);

            var response = await _httpClient.GetAsync($"https://api.paystack.co/transaction/verify/{reference}");

            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<PaystackVerificationResponse>(responseContent);
        }