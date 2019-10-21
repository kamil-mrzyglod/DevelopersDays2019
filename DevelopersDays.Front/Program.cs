using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;

namespace DevelopersDays.Front
{
    class Program
    {
        private const string ServiceAccountPath = "/var/run/secrets/kubernetes.io/serviceaccount/";
        private const string ServiceAccountTokenKeyFileName = "token";
        private const string CaKeyFileName = "ca.crt";

        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello Console World!");
            
            var handler = new HttpClientHandler();

            handler.ConfigureK8SCert(Path.Combine(ServiceAccountPath, CaKeyFileName));

            var http = new HttpClient(handler);

            var token = File.ReadAllText(Path.Combine(ServiceAccountPath, ServiceAccountTokenKeyFileName));
            http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            while (true)
            {
                var response = await http.GetAsync("http://developersdays-back-service");
                var result = response.IsSuccessStatusCode ? await response.Content.ReadAsStringAsync() : "FAIL";

                Console.WriteLine(result);

                var apiResponse = await http.GetAsync("https://kubernetes.default/api");
                Console.WriteLine(await apiResponse.Content.ReadAsStringAsync());

                Thread.Sleep(1000);
            }
        }
    }

    public static class Extensions
    {
        public static void ConfigureK8SCert(this HttpClientHandler handler, string certPath)
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
        }
    }
}
