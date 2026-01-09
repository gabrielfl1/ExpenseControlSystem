using ExpenseControlSystem.DTOs.SendEmailDto;
using ExpenseControlSystem.DTOs.UserDtos;
using ExpenseControlSystem.Services;

namespace ExpenseControlSystem.Interfaces {
    public interface IEmailServices {

        Task<ServiceResult<ResponseSendEmailDto>> SendEmail(SendEmailDto dto, string document) ;

    }
}
