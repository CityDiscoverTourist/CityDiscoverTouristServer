using System;
using System.Security.Cryptography;
using System.IO;
using System.Text;
using System.Net;
using System.Net.Http.Headers;

namespace MoMo
{
    class PaymentRequest
    {
        public PaymentRequest() {
        }
        public static string sendPaymentRequest(string endpoint, string postJsonString)
        {

            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(endpoint);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("Authorization", "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes("momo:momo")));
                HttpResponseMessage result = client.PostAsync(endpoint, new StringContent(postJsonString, Encoding.UTF8, "application/json")).Result;
                string resultString = "";


                using (var reader = new StreamReader(result.Content.ReadAsStreamAsync().Result))
                {
                    resultString = reader.ReadToEnd();
                }
                //return new MomoResponse(mtid, jsonresponse);
                return resultString;
            }
            catch (WebException e)
            {
                return e.Message;
            }
        }

        public static string sendConfirmPaymentRequest(string endpoint, string postJsonString)
        {

            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(endpoint);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("Authorization", "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes("momo:momo")));
                HttpResponseMessage result = client.PostAsync(endpoint, new StringContent(postJsonString, Encoding.UTF8, "application/json")).Result;
                string resultString = "";


                using (var reader = new StreamReader(result.Content.ReadAsStreamAsync().Result))
                {
                    resultString = reader.ReadToEnd();
                }
                //return new MomoResponse(mtid, jsonresponse);
                return resultString;
            }
            catch (WebException e)
            {
                return e.Message;
            }
        }
    }
}