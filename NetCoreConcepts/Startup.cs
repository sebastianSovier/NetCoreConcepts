using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace NetCoreConcepts
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            // Configuración JWT
            ConfigureAuthentication(services);

            // Configuración de CORS
            ConfigureCors(services);

            services.Configure<FormOptions>(options =>
            {
                options.MultipartBodyLengthLimit = long.MaxValue;
            });

            services.AddControllers();

            // Swagger para documentación
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "NetCoreConcepts", Version = "v1" });
            });
        }

        private void ConfigureAuthentication(IServiceCollection services)
        {
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(cfg =>
            {
                cfg.RequireHttpsMetadata = false;
                cfg.SaveToken = true;
                cfg.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = Configuration["JwtIssuer"],
                    ValidAudience = Configuration["JwtIssuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JwtKey"])),
                    ClockSkew = TimeSpan.Zero
                };
                cfg.Events = new JwtBearerEvents
                {
                    OnMessageReceived = ValidateTokenAsync
                };
            });
        }

        private void ConfigureCors(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin", builder =>
                {
                    builder.WithOrigins(Configuration["AllowedOrigins"]?.Split(",") ?? Array.Empty<string>())
                           .WithMethods("GET", "POST", "PUT", "DELETE")
                           .AllowAnyHeader();
                });
            });
        }

        public static Task ValidateTokenAsync(MessageReceivedContext context)
        {
            try
            {
                if (!context.Request.Headers.TryGetValue("Authorization", out var authorizationHeader))
                    throw new InvalidOperationException("Authorization token does not exist");

                var token = authorizationHeader.ToString().Split(" ").Last();

                var tokenHandler = new JwtSecurityTokenHandler();
                tokenHandler.ValidateToken(token, context.Options.TokenValidationParameters, out var validatedToken);

                var jwtToken = validatedToken as JwtSecurityToken;
                var claimsIdentity = new ClaimsIdentity(jwtToken.Claims, "JwtBearer");

                context.Principal = new ClaimsPrincipal(claimsIdentity);
                context.Success();
            }
            catch (Exception ex)
            {
                context.Fail(ex.Message);
            }

            return Task.CompletedTask;
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "NetCoreConcepts v1"));
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseCors("AllowSpecificOrigin");

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
