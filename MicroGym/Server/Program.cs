using MicroGym.Data.Context;
using MicroGym.Data.Repository.AttendanceRepository;
using MicroGym.Data.Repository.MemberRepo;
using MicroGym.Data.Repository.MembershipTypeRepo;
using MicroGym.Data.Repository.RevenueRepository;
using MicroGym.Data.Repository.UserRepository;
using MicroGym.Server.Middleware;
using MicroGym.Service.AttendanceSection;
using MicroGym.Service.Auth;
using MicroGym.Service.MemberInfo;
using MicroGym.Service.MembershipTypeSection;
using MicroGym.Service.RevenueSection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// ── Database ─────────────────────────────────────────────────────────────────
builder.Services.AddDbContext<GymDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ── Repositories ──────────────────────────────────────────────────────────────
builder.Services.AddScoped<IUserRepository,           UserRepository>();
builder.Services.AddScoped<IMemberRepository,         MemberRepository>();
builder.Services.AddScoped<IMembershipTypeRepository, MembershipTypeRepository>();
builder.Services.AddScoped<IRevenueRepository,        RevenueRepository>();
builder.Services.AddScoped<IAttendanceRepository,     AttendanceRepository>();

// ── Services ──────────────────────────────────────────────────────────────────
builder.Services.AddScoped<IMemberInfoService,      MemberInfoService>();
builder.Services.AddScoped<IMembershipTypeService,  MembershipTypeService>();
builder.Services.AddScoped<IRevenueSectionService,  RevenueSectionService>();
builder.Services.AddScoped<IAttendanceSectionService, AttendanceSectionService>();
builder.Services.AddScoped<IJwtService,             JwtService>();
builder.Services.AddScoped<IAuthService,            AuthService>();

// ── JWT Authentication ────────────────────────────────────────────────────────
// The secret key is read from:
//   Development : appsettings.Development.json → JwtSettings:SecretKey
//   Production  : environment variable         → JwtSettings__SecretKey
//
// Never commit the production secret to source control.
// On your production server, set the environment variable:
//   Windows IIS  : Application Pool → Advanced Settings → Environment Variables
//   Linux systemd: Add  Environment="JwtSettings__SecretKey=<your-key>"  to the service file
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey   = jwtSettings["SecretKey"]
    ?? throw new InvalidOperationException(
        "JwtSettings:SecretKey is missing. " +
        "For development, add it to appsettings.Development.json. " +
        "For production, set the environment variable JwtSettings__SecretKey.");

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme    = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer           = true,
        ValidateAudience         = true,
        ValidateLifetime         = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer              = jwtSettings["Issuer"],
        ValidAudience            = jwtSettings["Audience"],
        IssuerSigningKey         = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
    };
});

builder.Services.AddMemoryCache();
builder.Services.AddAuthorization();
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build();

// ── Global Exception Middleware ───────────────────────────────────────────────
// Must be first so it wraps every request. Returns JSON instead of the HTML crash page.
app.UseMiddleware<GlobalExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseBlazorFrameworkFiles();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();
