using PortfolioApp.Application.Common.Models;
using PortfolioApp.Application.DTOs.Contact;

namespace PortfolioApp.Application.Interfaces;

public interface IContactService
{
    Task<ApiResponse<PagedResponse<ContactMessageDto>>> GetAllMessagesAsync(QueryParameters parameters);
    Task<ApiResponse<ContactMessageDto>> GetMessageByIdAsync(int id);
    Task<ApiResponse<ContactMessageDto>> SendMessageAsync(CreateContactMessageDto dto, string ipAddress, string userAgent);
    Task<ApiResponse<bool>> UpdateMessageStatusAsync(UpdateContactStatusDto dto);
    Task<ApiResponse<bool>> DeleteMessageAsync(int id);
    Task<ApiResponse<int>> GetUnreadCountAsync();
}
