using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProductCatalog.Data;
using ProductCatalog.Repositories;
using Swashbuckle.AspNetCore.Swagger;

namespace ProductCatalog
{
    public class Startup
    {
        public IConfiguration Configuration { get; set; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();  //adicionando o serviço do MVC

            //Defininindo quem e quais métodos podem acessar a API - nesse caso estou deixando aberto a todos
            services.AddCors(options =>
            {
                options.AddPolicy("AllowMyOrigin", builder =>
                {
                    builder.AllowAnyOrigin();
                    builder.AllowAnyHeader();
                    builder.AllowAnyMethod();
                    builder.AllowCredentials();
                });
            });

            services.AddResponseCompression(); //utilizado para comprimir os retornos das requisições


            services.AddScoped<StoreDataContext, StoreDataContext>(); //verifica se já existe uma conexão na memoria, caso não cria uma
            services.AddTransient<ProductRepository, ProductRepository>(); //utilizando o Transient porque toda vez que eu adicionar um productRepository 
                                                                           //eu quero uma nova instância dele.
            services.AddTransient<CategoryRepository, CategoryRepository>();
            
            //configurando o swagger para documentar nossa API
            services.AddSwaggerGen(x =>
            {
                x.SwaggerDoc("v1", new Info { Title = "Store Api", Version = "v1" });
            });

            //TODO: Verificar implementação
            //services.AddDbContext<StoreDataContext>(opt =>
            //{
            //    opt.UseSqlServer(Configuration.GetConnectionString("SQLConnection"));

            //});
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            app.UseCors("AllowMyOrigin");
            app.UseMvc();
            app.UseResponseCompression();
        
            app.UseSwagger();

            //configurando a interface do swagger
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Store - API");
            });
        }
    }
}
