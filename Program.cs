using EmailClient.Services;

namespace EmailClient
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            // Get SMTP settings from the configuration
            var smtpSettings = builder.Configuration.GetSection("SmtpSettings");

            if (!int.TryParse(smtpSettings["Port"], out int smtpPort))
            {
                throw new InvalidOperationException("SMTP Port is not a valid integer.");
            }

            // Add EmailService as a singleton, with configuration values injected from appsettings.json
            builder.Services.AddSingleton(provider =>
                new EmailService(
                    smtpSettings["Host"] ?? throw new ArgumentNullException(nameof(smtpSettings), "Host not configured in SmtpSettings"),
                    smtpPort,
                    smtpSettings["LogFilePath"] ?? throw new ArgumentNullException(nameof(smtpSettings), "LogFilePath not configured in SmtpSettings"),
                    smtpSettings["UserName"] ?? throw new ArgumentNullException(nameof(smtpSettings), "UserName not configured in SmtpSettings"),
                    smtpSettings["Password"] ?? throw new ArgumentNullException(nameof(smtpSettings), "Password not configured in SmtpSettings")
                )
                
            );

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
