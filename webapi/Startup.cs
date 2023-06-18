using Microsoft.AspNetCore.Authentication.Negotiate;
using Microsoft.AspNetCore.Server.IISIntegration;

public class Startup
{
    public IConfiguration ConfigRoot
    {
        get;
    }

    public Startup(IConfiguration configuration)
    {
        ConfigRoot = configuration;
    }

    public void ConfigServices(IServiceCollection services)
    {
        services.AddControllers();
        //services.AddAuthentication(IISDefaults.Ntlm);
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddCors(opt =>
        {
            opt.AddPolicy("CorsPolicy", builder => builder
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials()
                .WithOrigins(ConfigRoot.GetSection("CorsOrigins").Get<string[]>()))
                ;
        });
        services.AddAuthentication(configureOptions =>
        {
            configureOptions.DefaultAuthenticateScheme = NegotiateDefaults.AuthenticationScheme;
            configureOptions.DefaultChallengeScheme = NegotiateDefaults.AuthenticationScheme;
        }).AddNegotiate();
    }

    public void Configure(WebApplication app, IWebHostEnvironment env)
    {
        // Configure the HTTP request pipeline.
        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseCors("CorsPolicy");

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
