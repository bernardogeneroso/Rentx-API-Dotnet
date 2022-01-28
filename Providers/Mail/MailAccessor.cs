using FluentEmail.Core;
using Microsoft.Extensions.Configuration;
using Services.Interfaces;

namespace Providers.Mail;

public class MailAccessor : IMailAccessor
{
    private readonly IConfiguration _config;
    private readonly IFluentEmail _mail;
    public MailAccessor(IConfiguration config, IFluentEmail mail)
    {
        _mail = mail;
        _config = config;
    }

    public async Task SendMail(string to, string subject, string displayName, string body = null)
    {
        body = body ?? "Test email of RentX";

        var path = Path.Combine(Directory.GetCurrentDirectory(), "Templates", "test.cshtml");

        var newEmail = await _mail
            .To(to)
            .Subject(subject)
            .UsingTemplateFromFile(path, new { DisplayName = "Bernardo", Body = body })
            .SendAsync();


        if (!newEmail.Successful) throw new Exception("Error sending email");
    }
}
