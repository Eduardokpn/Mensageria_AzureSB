using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using MensageriaProject.Interfaces;
using Newtonsoft.Json;

public class EmailService : IEmailService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey = "re_2cm9YNG9_DSJ5PE8JaV6PCYx8Mn9nbcKi";

    public EmailService()
    {
        _httpClient = new HttpClient();
        
    }

    public async Task<bool> SendEmailResendAsync(string to)
    {
        var cleanedTo = to.Replace("\"", "").Trim();
        var emailData = new
        {
            from = "Eduardo <onboarding@resend.dev>",
            to = new[] { cleanedTo },
            subject = "Seu Cadastro foi Realizado",
            html = "<p>Obrigado por se Cadastrar</p>"
        };

        var json = JsonConvert.SerializeObject(emailData);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", "re_2cm9YNG9_DSJ5PE8JaV6PCYx8Mn9nbcKi");

        try
        {
            var response = await _httpClient.PostAsync("https://api.resend.com/emails", content);

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("✅ E-mail enviado com sucesso!");
                return true;
            }
            else
            {
                Console.WriteLine($"❌ Falha ao enviar e-mail. Código: {response.StatusCode}");
                var errorContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Detalhes: {errorContent}");
                return false;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Erro inesperado: {ex.Message}");
            return false;
        }
    }


    public async Task SendEmailsAsync(List<string> emails)
    {
        foreach (var email in emails)
        {
            await SendEmailResendAsync(email);
        }
    }




}
