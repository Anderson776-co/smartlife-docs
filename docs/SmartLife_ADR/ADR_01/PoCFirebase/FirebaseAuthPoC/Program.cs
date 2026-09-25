using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

var firebaseProjectId = "pocfirebase-c2915";

var credentialPath = Path.Combine(builder.Environment.ContentRootPath, "firebase-service-account.json");

FirebaseApp.Create(new AppOptions()
{
    Credential = CredentialFactory.FromFile(credentialPath, "service_account")
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = $"https://securetoken.google.com/{firebaseProjectId}";
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidIssuer = $"https://securetoken.google.com/{firebaseProjectId}",
            ValidAudience = firebaseProjectId,
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true
        };

        options.Events = new JwtBearerEvents
        {
            OnTokenValidated = async context =>
            {
                var authHeader = context.Request.Headers.Authorization.ToString();
                var rawToken = authHeader.StartsWith("Bearer ")
                    ? authHeader["Bearer ".Length..]
                    : authHeader;

                try
                {
                    await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(rawToken, checkRevoked: true);
                }
                catch (FirebaseAuthException)
                {
                    context.Fail("El token fue revocado (sesion cerrada). Vuelve a iniciar sesion.");
                }
            }
        };
    });

builder.Services.AddAuthorization();
builder.Services.AddControllers();

// Permite que login.html (servido en otro puerto, ej. localhost:3000)
// pueda llamar a esta API sin que el navegador lo bloquee por CORS.
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

app.UseHttpsRedirection();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();