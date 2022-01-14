using Mango.Services.Email.Messages;

namespace Mango.Services.Email.Repositories.Contracts;

public interface IEmailRepository
{
    Task SendAndLogEmail(UpdatePaymentResultMessage message);
}