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
using Microsoft.AspNetCore.Identity;
using Repositories.UserRepo;
using Services.AccountS;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
//var port = Environment.GetEnvironmentVariable("PORT") ?? "8080"; // Cổng mặc định nếu không có PORT

//builder.WebHost.ConfigureKestrel(serverOptions =>
//{
//    serverOptions.ListenAnyIP(int.Parse(port));  // Lắng nghe cổng từ Railway
//});
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", policy =>
    {
        policy.AllowAnyOrigin()  // Cho phép mọi origin
                 .AllowAnyMethod()  // Cho phép mọi phương thức HTTP (GET, POST, PUT, DELETE, ...)
                 .AllowAnyHeader(); 
    });
});


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

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            // 1. Xác thực issuer
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["AppSettings:Issuer"],

            // 2. Xác thực audience
            ValidateAudience = true,
            ValidAudience = builder.Configuration["AppSettings:Audience"],

            // 3. Xác thực thời gian sống của token
            ValidateLifetime = true,

            // 4. Khóa ký token
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                                          Encoding.UTF8.GetBytes(
                                            builder.Configuration["AppSettings:Token"]!))
        };
    });
builder.Services.AddAuthorization();

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
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

    // 1) Khai báo scheme Bearer (HTTP)
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Nhập JWT token theo định dạng: Bearer {token}",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });

    // 2) Bắt buộc mọi operation (hoặc những operation có [Authorize]) dùng scheme trên
    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme {
                Reference = new OpenApiReference {
                    Type = ReferenceType.SecurityScheme,
                    Id   = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});



var app = builder.Build();

// Configure the HTTP request pipeline.


    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
        c.RoutePrefix = string.Empty; // Swagger UI sẽ được đặt ở trang chủ
    });

    // Chỉ kích hoạt HTTP redirect khi môi trường không phải phát triển
    app.UseHttpsRedirection();  // Chuyển hướng tất cả HTTP yêu cầu thành HTTPS


app.UseRouting();
app.UseCors("AllowAllOrigins");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
