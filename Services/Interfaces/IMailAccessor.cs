namespace Services.Interfaces;

public interface IMailAccessor
{
    Task SendMail(string to, string subject, string displayName, string body = null);
}
