using Model;
using Microsoft.EntityFrameworkCore;
using Services.KitS;
using Repositories.KitRepo;
using Repositories.KitDeliveryRepo;
using Services.KitDeliverySS;
using Services.ExResultSS;
using Repositories.ExResultRepo;
using Repositories.SampleMethodRepo;
using Services.SampleMethodSS;
using Services.ServiceSS;
using Repositories.ServiceRepo;
using Repositories.ExRequestRepo;
using Services.ExRequestSS;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
//var port = Environment.GetEnvironmentVariable("PORT") ?? "8080"; // Cổng mặc định nếu không có PORT

//builder.WebHost.ConfigureKestrel(serverOptions =>
//{
//    serverOptions.ListenAnyIP(int.Parse(port));  // Lắng nghe cổng từ Railway
//});

// Add services to the container.
builder.Services.AddScoped<IKitService, KitService>();
builder.Services.AddScoped<IKitRepository, KitRepository>();
builder.Services.AddScoped<IKitDeliveryRepository, KitDeliveryRepository>();
builder.Services.AddScoped<IKitDeliveryS, KitDeliveryS>();

builder.Services.AddScoped<IExResultS, ExResultS>();
builder.Services.AddScoped<IExResultRepository, ExResultRepository>();

builder.Services.AddScoped<ISampleMethodRepository, SampleMethodRepository>();
builder.Services.AddScoped<ISampleMethodS, SampleMethodS>();

builder.Services.AddScoped<IServiceRepository, ServiceRepository>();
builder.Services.AddScoped<IServiceBB, ServiceBB>();

builder.Services.AddScoped<IExRequestRepository, ExRequestRepository>();
builder.Services.AddScoped<IExRequestS, ExRequestS>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<BloodlineDbContext>(options =>options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddSwaggerGen(options =>
{
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath);
});





var app = builder.Build();

// Configure the HTTP request pipeline.


    app.UseSwagger();
    app.UseSwaggerUI();



app.UseAuthorization();
app.UseRouting();
app.MapControllers();

app.Run();
