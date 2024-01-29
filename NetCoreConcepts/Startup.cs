using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
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

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear(); // => remove default claims
            services
                .AddAuthentication(options =>
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
                        ClockSkew = TimeSpan.Zero // remove delay of token when expire
                    };
                    cfg.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = ValidateToken
                    };
                });
            services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin", builder =>
                {
                    builder.WithOrigins("http://localhost:8080").WithMethods("POST").WithHeaders("Authorization");
                });
            });
            services.AddControllers();
            
           
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "NetCoreConcepts", Version = "v1" });
            });

        }
        public static string GetTokenFromHeader(IHeaderDictionary requestHeaders)
        {
            if (!requestHeaders.TryGetValue("Authorization", out var authorizationHeader))
                throw new InvalidOperationException("Authorization token does not exists");

            var authorization = authorizationHeader.FirstOrDefault()!.Split(" ");

            var type = authorization[0];

            if (type != "Bearer") throw new InvalidOperationException("You should provide a Bearer token");

            var value = authorization[1] ?? throw new InvalidOperationException("Authorization token does not exists");
            return value;
        }
        public static Task ValidateToken(MessageReceivedContext context)
        {
            try
            {
                context.Token = GetTokenFromHeader(context.Request.Headers);

                var tokenHandler = new JwtSecurityTokenHandler();
                tokenHandler.ValidateToken(context.Token, context.Options.TokenValidationParameters, out var validatedToken);

                var jwtSecurityToken = validatedToken as JwtSecurityToken;

                context.Principal = new ClaimsPrincipal();

                var claimsIdentity = new ClaimsIdentity(jwtSecurityToken.Claims.ToList(),
                        "JwtBearerToken", ClaimTypes.NameIdentifier, ClaimTypes.Role);
                context.Principal.AddIdentity(claimsIdentity);

                context.Success();

                return Task.CompletedTask;
            }
            catch (Exception e)
            {
                context.Fail(e);
            }

            return Task.CompletedTask;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
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
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseCors("AllowSpecificOrigin");
            
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
     


        }
    }
}
