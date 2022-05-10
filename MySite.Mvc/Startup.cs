using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MySite.Services.AutoMapper.Profiles;
using MySite.Services.Extensions;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using MySite.Entities.Concrete;
using MySite.Mvc.AutoMapper.Profiles;
using MySite.Mvc.Filters;
using MySite.Mvc.Helpers.Abstract;
using MySite.Mvc.Helpers.Concrete;
using MySite.Shared.Utilities.Extensions;

namespace MySite.Mvc
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
            services.AddRouting(options => options.LowercaseUrls = true);
            services.Configure<WebsiteInfo>(Configuration.GetSection("WebsiteInfo"));
            services.Configure<AboutUsPageInfo>(Configuration.GetSection("AboutUsPageInfo"));
            services.Configure<ReferencesPageInfo>(Configuration.GetSection("ReferencesPageInfo"));
            services.Configure<ContactPageInfo>(Configuration.GetSection("ContactPageInfo"));
            services.Configure<SmtpSettings>(Configuration.GetSection("SmtpSettings"));
            services.Configure<ArticleRightSideBarWidgetOptions>(Configuration.GetSection("ArticleRightSideBarWidgetOptions"));
            services.ConfigureWritable<WebsiteInfo>(Configuration.GetSection("WebsiteInfo"));
            services.ConfigureWritable<AboutUsPageInfo>(Configuration.GetSection("AboutUsPageInfo"));
            services.ConfigureWritable<ReferencesPageInfo>(Configuration.GetSection("ReferencesPageInfo"));
            services.ConfigureWritable<ContactPageInfo>(Configuration.GetSection("ContactPageInfo"));
            services.ConfigureWritable<SmtpSettings>(Configuration.GetSection("SmtpSettings"));
            services.ConfigureWritable<ArticleRightSideBarWidgetOptions>(Configuration.GetSection("ArticleRightSideBarWidgetOptions"));
            services.AddControllersWithViews(options =>
            {
                options.ModelBindingMessageProvider.SetValueMustNotBeNullAccessor(value => "Bu alan boþ geçilmemelidir.");
                options.Filters.Add<MvcExceptionFilter>();
            }).AddRazorRuntimeCompilation().AddJsonOptions(opt =>
            {
                opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                opt.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
            }).AddNToastNotifyToastr();
            services.AddSession();
            services.AddAutoMapper(typeof(CategoryProfile), typeof(TagProfile), typeof(ArticleProfile), typeof(UserProfile), typeof(ViewModelsProfile), typeof(CommentProfile), typeof(UploadProfile));
            services.LoadMyServices(connectionString: Configuration.GetConnectionString("LocalDB"));
            services.AddScoped<IImageHelper, ImageHelper>();
            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = new PathString("/Admin/Auth/Giris");
                options.LogoutPath = new PathString("/Admin/Auth/Cikis");
                options.Cookie = new CookieBuilder
                {
                    Name = "MySite",
                    HttpOnly = true,
                    SameSite = SameSiteMode.Strict,
                    SecurePolicy = CookieSecurePolicy.SameAsRequest // Always
                };
                options.SlidingExpiration = true;
                options.ExpireTimeSpan = System.TimeSpan.FromDays(7);
                options.AccessDeniedPath = new PathString("/Admin/Auth/YetkisizErisim");
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseStatusCodePages();
            }

            app.UseSession();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseNToastNotify();
            //app.UseMvc(routes =>
            //{
            //    routes.MapRoute(
            //        name: "blog",
            //        template: "{title}",
            //        defaults: new { controller = "Article", action = "Detail" });

            //    routes.MapRoute(
            //        name: "default",
            //        template: "Admin/{controller=Home}/{action=Index}/{id?}");
            //});

            app.UseEndpoints(endpoints =>
            {

                endpoints.MapControllerRoute(
                    name: "blog",
                    pattern: "{title}-{articleId}",
                    defaults: new { controller = "Makale", action = "Detay" });
                endpoints.MapControllers();

                endpoints.MapControllerRoute(
                    name: "kategori",
                    pattern: "kategori/{categoryTitle}",
                    defaults: new { controller = "Blog", action = "Index" });
                endpoints.MapControllers();

                endpoints.MapAreaControllerRoute(
                    name: "Admin",
                    areaName: "Admin",
                    pattern: "Admin/{controller=Home}/{action=Index}/{id?}"
                );
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
