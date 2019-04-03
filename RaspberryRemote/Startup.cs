using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RaspberryRemote.Hubs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;

namespace RaspberryRemote
{
    public class Startup
    {
        // This doesn't really go here, but it's easy, fast, and it works. :)
        public static HashSet<string> AllowedKeys;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });


            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddSignalR();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            AllowedKeys = this.GetAllowedKeys();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseSignalR(routes =>
            {
                routes.MapHub<LircHub>("/LircHub");
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        private HashSet<string> GetAllowedKeys()
        {
            // Thanks to https://github.com/mikaelsnavy/LircSharpAPI for making this easy on me
            HashSet<string> keys = new HashSet<string>();

            ProcessStartInfo procInfo = new ProcessStartInfo
            {
                FileName = "/bin/bash",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = true,
                Arguments = "-c \"irsend list 'LG_AKB72915207' ''\"",
            };

            Process proc = Process.Start(procInfo);
            string strOut = proc.StandardOutput.ReadToEnd();
            proc.WaitForExit();

            /* Extract the remote names from the LIRC return*/
            using (StringReader reader = new StringReader(strOut))
            {
                string line = string.Empty;
                do
                {
                    line = reader.ReadLine();
                    if (!string.IsNullOrWhiteSpace(line) && line.Contains(' '))
                    {
                        string key = line.Split(' ')[1];
                        Console.WriteLine("Adding key " + key);
                        keys.Add(key);
                    }

                } while (line != null);
            }

            return keys;
        }
    }
}
