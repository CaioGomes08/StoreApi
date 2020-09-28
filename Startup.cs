using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ProductCatalog.Data;
using ProductCatalog.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProductCatalog
{
    public class Startup
    {
        public IConfiguration Configuration { get; set; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().AddNewtonsoftJson();  //adicionando o serviço do MVC

            

            // Adicionando o middleware de autenticação jwt
            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,

                    ValidIssuer = "https://localhost:5001",
                    ValidAudience = "https://localhost:5001",
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration.GetValue<string>("SecrectKey")))
                };
            });

            //Defininindo quem e quais métodos podem acessar a API - nesse caso estou deixando aberto a todos
            services.AddCors(options =>
            {
                options.AddPolicy("AllowMyOrigin", builder =>
                {
                    builder.AllowAnyOrigin();
                    builder.AllowAnyHeader();
                    builder.AllowAnyMethod();
                });
            });

            services.Configure<IISOptions>(o =>
            {
                o.ForwardClientCertificate = false;
            });

            services.AddResponseCompression(); //utilizado para comprimir os retornos das requisições


            //services.AddScoped<StoreDataContext, StoreDataContext>(); //verifica se já existe uma conexão na memoria, caso não cria uma
            services.AddTransient<UserRepository, UserRepository>();
            services.AddTransient<CategoryRepository, CategoryRepository>();
            services.AddTransient<ProductRepository, ProductRepository>(); //utilizando o Transient porque toda vez que eu adicionar um productRepository 
                                                                           //eu quero uma nova instância dele.

            //Definição do DbContext
            services.AddDbContext<StoreDataContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("SQLConnection"));
            });

            //configurando o swagger para documentar nossa API
            services.AddSwaggerGen(x =>
            {
                x.SwaggerDoc("v1", new OpenApiInfo { Title = "Store Api", Version = "V1.0" });

                // Security session using bearer token
                x.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter into field the word 'Bearer' following by space and JWT",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                //x.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>>
                //{{"Bearer", Enumerable.Empty<string>()}});
                //x.CustomSchemaIds(x => x.FullName); 

                x.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,
                        },
                        new List<string>()
                    }
                });

            });


        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            app.UseCors("AllowMyOrigin");
            app.UseAuthentication();

            //app.UseHttpsRedirection();
            
            app.UseRouting();
            app.UseResponseCompression();

            app.UseSwagger();

            //configurando a interface do swagger
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Store - API");
                c.RoutePrefix = string.Empty;
            });

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
