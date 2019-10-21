using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace DevelopersDays.Web
{
    public class Startup
    {
        private const string ServiceAccountPath = "/var/run/secrets/kubernetes.io/serviceaccount/";
        private const string ServiceAccountTokenKeyFileName = "token";
        private const string CaKeyFileName = "ca.crt";

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpClient("client", c =>
                {
                    var token = File.ReadAllText(Path.Combine(ServiceAccountPath, ServiceAccountTokenKeyFileName));
                    c.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                })
                .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler().AddCert(Path.Combine(ServiceAccountPath, CaKeyFileName)));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.Run(async (context) =>
            {
                var clientFactory = context.RequestServices.GetRequiredService<IHttpClientFactory>();
                var http = clientFactory.CreateClient("client");

                if (context.Request.Path.StartsWithSegments(new PathString("/kube")))
                {
                    var apiResponse = await http.GetAsync("https://kubernetes.default/api");
                    var apiResult = await apiResponse.Content.ReadAsStringAsync();
                    await context.Response.WriteAsync(apiResult);
                }
                else
                {
                    var response = await http.GetAsync("http://developersdays-back-service");
                    var result = response.IsSuccessStatusCode ? await response.Content.ReadAsStringAsync() : "FAIL";
                    await context.Response.WriteAsync(result);
                }
            });
        }
    }

    public static class Extensions
    {
        public static HttpClientHandler AddCert(this HttpClientHandler handler, string certPath)
        {
            var ca = new X509Certificate2(certPath);
            handler.ServerCertificateCustomValidationCallback =
                (HttpRequestMessage sender, X509Certificate2 certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) =>
                {
                    if (sslPolicyErrors == SslPolicyErrors.None)
                    {
                        return true;
                    }

                    if ((sslPolicyErrors & SslPolicyErrors.RemoteCertificateChainErrors) != 0)
                    {
                        chain.ChainPolicy.RevocationMode = X509RevocationMode.NoCheck;
                        chain.ChainPolicy.ExtraStore.Add(ca);
                        chain.ChainPolicy.VerificationFlags = X509VerificationFlags.AllowUnknownCertificateAuthority;
                        var isValid = chain.Build((X509Certificate2)certificate);
                        var rootCert = chain.ChainElements[chain.ChainElements.Count - 1].Certificate;
                        var isTrusted = rootCert.RawData.SequenceEqual(ca.RawData);

                        return isValid && isTrusted;
                    }

                    return false;
                };

            return handler;
        }
    }
}
