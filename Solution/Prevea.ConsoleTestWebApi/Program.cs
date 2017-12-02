namespace Prevea.ConsoleTestWebApi
{
    #region Using

    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using Newtonsoft.Json;
    using Model.Model;
    using static System.Console;

    #endregion

    class Program
    {
        static void Main(string[] args)
        {
            WriteLine("Press ENTER to exit . . .");
            WaitForEnter();

            PruebaSinAuthorization();
            WriteLine("Press ENTER to exit . . .");
            WaitForEnter();


            //var client = new HttpClient();
            //var response = client.GetAsync(baseAddress + "test").Result;
            //Console.WriteLine(response);

            //Console.WriteLine();

            //var authorizationHeader = Convert.ToBase64String(Encoding.UTF8.GetBytes("admin:gNwPDFaR"));
            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authorizationHeader);

            //var form = new Dictionary<string, string>
            //{
            //    {"grant_type", "password"},
            //    {"username", "admin"},
            //    {"password", "gNwPDFaR"},
            //};

            //var tokenResponse = client.PostAsync(baseAddress + "accesstoken", new FormUrlEncodedContent(form)).Result;
            //var stringResult = tokenResponse.Content.ReadAsStringAsync().Result;
            //var token = JsonConvert.DeserializeObject<Token>(stringResult);

            //Console.WriteLine("Token issued is: {0}", token.AccessToken);

            //Console.WriteLine();

            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.AccessToken);
            //var authorizedResponse = client.GetAsync(baseAddress + "Usuarios/Flotas").Result;

            //Console.WriteLine(authorizedResponse);

            //if (authorizedResponse.StatusCode == HttpStatusCode.OK)
            //{
            //    stringResult = authorizedResponse.Content.ReadAsStringAsync().Result;
            //    var flotas = JsonConvert.DeserializeObject<List<VUsuario>>(stringResult);

            //    Console.WriteLine();
            //    Console.WriteLine("Flotas:");

            //    foreach (var flota in flotas)
            //        Console.WriteLine("{0} => {1}", flota.Id, flota.Nombre);
            //}
            //else
            //{
            //    Console.WriteLine("Error: " + authorizedResponse.StatusCode);
            //}

            //WaitForEnter();

        }

        private static void PruebaSinAuthorization()
        {
            const string baseAddress = "http://localhost:50783/";

            var client = new HttpClient();
            var authorizedResponse = client.GetAsync(baseAddress + "Reports/Users").Result;

            WriteLine(authorizedResponse);

            var stringResult = authorizedResponse.Content.ReadAsStringAsync().Result;
            var users = JsonConvert.DeserializeObject<List<User>>(stringResult);

            WriteLine();
            WriteLine("Users:");

            foreach (var user in users)
                WriteLine("{0} => {1} {2}", user.Id, user.FirstName, user.LastName);
        }

        static void WaitForEnter()
        {
            while (ReadKey(intercept: true).Key != ConsoleKey.Enter) ;
        }
    }
}
