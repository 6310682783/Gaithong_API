using Autofac;
using Autofac.Extensions.DependencyInjection;
using ElmahCore.Mvc;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using GaiThong_API.Repositories;
using GaiThong_API.Services;
using Serilog;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    Args = args,
    // Look for static files in "wwwroot"
    WebRootPath = "wwwroot"
});

ConfigurationManager Configuration = builder.Configuration;

//allow cors policy
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      builder =>
                      {
                          _ = builder.WithOrigins(Configuration.GetSection("CorsOrigins").Get<string[]>())
                          .AllowAnyMethod()
                          .AllowAnyHeader()
                          .AllowCredentials();
                      });
});

//setting serilog
var logger = new LoggerConfiguration()
  .ReadFrom.Configuration(builder.Configuration)
  .Enrich.FromLogContext()
  .CreateLogger();
builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddElmah(options =>
{
    //options.CheckPermissionAction = context => context.User.Identity.IsAuthenticated;
    options.Path = @"elmah";
});

// ===== Add Jwt Authentication ========
//JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear(); // => remove default claims
//builder.Services
//    .AddAuthentication(options =>
//    {
//        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
//        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

//    })
//    .AddJwtBearer(cfg =>
//    {
//        cfg.RequireHttpsMetadata = false;
//        cfg.SaveToken = true;
//        cfg.TokenValidationParameters = new TokenValidationParameters
//        {
//            ValidIssuer = Configuration["JwtBearer:JwtIssuer"],
//            ValidAudience = Configuration["JwtBearer:JwtAudience"],
//            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JwtBearer:JwtKey"])),
//            ClockSkew = TimeSpan.Zero // remove delay of token when expire
//        };
//    });

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

//Now register our services with Autofac container
// Call UseServiceProviderFactory on the Host sub property 
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

builder.Host.ConfigureContainer<ContainerBuilder>(builder =>
{
    var serviceAssembly = typeof(ReminderService).Assembly;
    builder.RegisterAssemblyTypes(serviceAssembly).Where(t => t.Name.EndsWith("Service"))
    .AsImplementedInterfaces()
    .SingleInstance();

    var repositoryAssembly = typeof(ReminderRepository).Assembly;
    builder.RegisterAssemblyTypes(repositoryAssembly).Where(t => t.Name.EndsWith("Repository"))
    .AsImplementedInterfaces()
    .SingleInstance();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    /* Disable swagger schemas at bottom*/
    c.DocExpansion(DocExpansion.None);
    //c.DefaultModelsExpandDepth(0); 
});

app.UseElmah();

app.UseCors(MyAllowSpecificOrigins);

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.Run();