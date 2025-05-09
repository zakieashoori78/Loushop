using Loushop.Data;
using Loushop.Data.Repositories;
using Loushop.ExternalServices;
using Loushop.services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Security.Claims;

namespace Loushop
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
            services.AddControllersWithViews();
            services.AddRazorPages();

            #region Db Context

            services.AddDbContext<LouShopContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });

            #endregion

            #region IoC

            services.AddScoped<IGroupRepository, GroupRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IPaymentService, PaymentService>();
            services.AddScoped<IZibalService, ZibalService>();
            services.AddHttpClient<IZibalService, ZibalService>();
            services.AddTransient<IEmailSender, EmailSender>();

            // تنظیمات سشن و کش توزیع‌شده
            services.AddDistributedMemoryCache(); // حافظه کش برای مدیریت سشن‌ها
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30); // مدت زمان انقضا سشن
                options.Cookie.HttpOnly = true; // جلوگیری از دسترسی جاوااسکریپت به کوکی
                options.Cookie.IsEssential = true; // تنظیم کوکی به عنوان کوکی ضروری
            });

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>(); // اضافه کردن IHttpContextAccessor برای دسترسی به سشن در کل برنامه

            #endregion

            #region Authentication

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/Account/Login"; // مسیر ورود
                    options.LogoutPath = "/Account/Logout"; // مسیر خروج
                });
            services.AddAuthorization(); // مجوزهای دسترسی

            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts(); // افزودن HSTS
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles(); // استفاده از فایل‌های استاتیک

            app.UseRouting(); // استفاده از روتینگ برای درخواست‌ها
            app.UseSession(); // اضافه کردن Middleware برای استفاده از سشن

            app.UseAuthentication(); // فعال‌سازی احراز هویت
            app.UseAuthorization(); // فعال‌سازی مجوزهای دسترسی

            // Middleware برای مدیریت دسترسی به بخش Admin
            app.Use(async (context, next) =>
            {
                if (context.Request.Path.StartsWithSegments("/Admin"))
                {
                    if (!context.User.Identity.IsAuthenticated)
                    {
                        context.Response.Redirect("/Account/Login");
                    }
                    else if (!bool.Parse(context.User.FindFirstValue("IsAdmin")))
                    {
                        context.Response.Redirect("/Account/Login");
                    }
                }
                await next.Invoke();
            });

            // تنظیمات مربوط به endpoint ها
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
                endpoints.MapRazorPages();
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            // تنظیمات مخصوص بخش‌های مختلف سایت
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "areas",
                    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
