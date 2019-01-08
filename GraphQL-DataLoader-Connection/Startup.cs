using GraphQL;
using GraphQL.DataLoader;
using GraphQL.Execution;
using GraphQL.Http;
using GraphQL.Relay.Types;
using GraphQL.Server;
using GraphQL.Types.Relay;
using GraphQL_DataLoader_Connection.Types;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace GraphQL_DataLoader_Connection
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
            // Register these relay types
            services.AddTransient(typeof(ConnectionType<>));
            services.AddTransient(typeof(EdgeType<>));
            services.AddTransient<NodeInterface>();
            services.AddTransient<PageInfoType>();

            // Our various types
            services.AddSingleton<RootSchema>();
            services.AddSingleton<RootQuery>();
            services.AddSingleton<CompanyType>();
            services.AddSingleton<PersonType>();

            // And some stuff to make GraphQL work
            services.TryAddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.TryAddSingleton<IDependencyResolver>(q => new FuncDependencyResolver(q.GetRequiredService));
            services.TryAddSingleton<IDocumentExecuter, DocumentExecuter>();
            services.TryAddSingleton<IDocumentWriter, DocumentWriter>();
            services.TryAddSingleton<IDataLoaderContextAccessor, DataLoaderContextAccessor>();
            services.TryAddSingleton<IDocumentExecutionListener, DataLoaderDocumentListener>();

            services.AddGraphQL(options =>
            {
                options.EnableMetrics = true;
                options.ExposeExceptions = true;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseGraphQL<RootSchema>("/graph");
        }
    }
}
