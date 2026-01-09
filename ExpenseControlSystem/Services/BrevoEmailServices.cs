using ExpenseControlSystem.Configurations;
using ExpenseControlSystem.DTOs.SendEmailDto;
using ExpenseControlSystem.DTOs.UserDtos;
using ExpenseControlSystem.Interfaces;
using Microsoft.Extensions.Options;
using System.Text;

namespace ExpenseControlSystem.Services {
    public class BrevoEmailServices : IEmailServices {

        private readonly EmailConfigurations _config;
        private readonly HttpClient _httpClient;

        public BrevoEmailServices(IOptions<EmailConfigurations> options, HttpClient httpClient) {
            _config = options.Value;
            _httpClient = httpClient;

            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("api-key", _config.ApiKey);
        }

        public async Task<ServiceResult<ResponseSendEmailDto>> SendEmail(
            SendEmailDto dto,
            string document) {

            var stringBuilder = new StringBuilder();
            stringBuilder.Append($"<p><strong>Olá { dto.ToName}, tudo bem ?.</strong></p> <br />");
            stringBuilder.Append("<p>Segue em anexo o relatório solicitado</p>");

            var content = new {
                sender = new {
                    email = _config.FromEmail,
                    name = _config.FromName
                },
                to = new[] {
                    new {
                        email = dto.ToEmail,
                        name = dto.ToName
                    }
                },
                subject = $"relatorio de gasto customizado {dto.ToName}",
                htmlContent = 1, //stringBuilder.ToString(),
                attachment = new[] {
                    new { 
                        content = document,
                        name = "relatorio.xlsx"
                    } 
                }
            };

            var response = await _httpClient.PostAsJsonAsync("https://api.brevo.com/v3/smtp/email", content);
            var status = (int)response.StatusCode;

            if (!response.IsSuccessStatusCode) {

                var responseBody = await response.Content.ReadAsStringAsync();

                return new ServiceResult<ResponseSendEmailDto> {
                    Success = false,
                    Result = new ResponseSendEmailDto {
                        Message = responseBody,
                        StatusCode = status,
                    }
                };
            }

            return new ServiceResult<ResponseSendEmailDto> { 
                Success = true,
                Result = new ResponseSendEmailDto {
                    Message = "Email Enviado com sucesso",
                    StatusCode = status
                }
            };
        }
    }
}
