namespace EmailClient
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            var smtpSettings = builder.Configuration.GetSection("SmtpSettings");

            // Port number is not parsing correctly


            /*if (!int.TryParse(portValue, out int smtpPort))
            {
                throw new Exception("Invalid SMTP port number in appsettings.json");
            }*/

            if (!int.TryParse(smtpSettings["Port"], out int smtpPort))
            {
                throw new Exception("Invalid SMTP port number in appsettings.json");
            }

            builder.Services.AddSingleton(provider =>
                new EmailClient.Services.EmailService(
                    smtpSettings["Host"],
                    smtpPort,
                    smtpSettings["User"],
                    smtpSettings["Pass"],
                    builder.Configuration["LogFilePath"]
                )
            );

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
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
