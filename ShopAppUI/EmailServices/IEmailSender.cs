namespace ShopAppUI.EmailServices
{
    public interface IEmailSender
    {
        // smtp => gmail, hotmail 
        // api  => sendgrid //hosting ile aldğıdımız sitede mevcuttur.

        Task SendEmailAsync(string email, string subject, string htmlMessage);
    }
}
