using GatewayAPI.Web.Extensions;
using GatewayAPI.Web.Middlewares;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Polly;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});
builder.Services.ConfigureLogger(builder.Configuration, builder.Environment, "ElasticConfiguration:Uri");

builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);
builder.Services.AddOcelot(builder.Configuration).AddPolly();

builder.Services.AddEndpointsApiExplorer();

builder.Services.ConfigureSwagger(builder.Configuration);
builder.Services.ConfigureJWT(builder.Configuration.GetSection("JwtSettingsConfig"));

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerForOcelotUI(opt =>
{
    opt.PathToSwaggerGenerator = "/swagger/docs";
});

app.UseHttpsRedirection();
app.UseCors();
app.UseAuthentication();

var configuration = new OcelotPipelineConfiguration
{
    AuthorizationMiddleware = async (httpContext, next) =>
    {
        await OcelotAuthorizationMiddleware.Authorize(httpContext, next);
    }
};
app.UseOcelot(configuration).Wait();

app.UseAuthorization();

app.Run();
