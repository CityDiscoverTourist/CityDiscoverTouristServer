using System.Net;
using System.Text;

namespace CityDiscoverTourist.Business.Momo;

internal class PaymentRequest
{
    public static string SendPaymentRequest(string endpoint, string postJsonString)
    {
        try
        {
#pragma warning disable CS0618
            var httpWReq = (HttpWebRequest) WebRequest.Create(endpoint);
#pragma warning restore CS0618

            var data = Encoding.UTF8.GetBytes(postJsonString);

            httpWReq.ProtocolVersion = HttpVersion.Version11;
            httpWReq.Method = "POST";
            httpWReq.ContentType = "application/json";

            httpWReq.ContentLength = data.Length;
            httpWReq.ReadWriteTimeout = 30000;
            httpWReq.Timeout = 15000;
            var stream = httpWReq.GetRequestStream();
            stream.Write(data, 0, data.Length);
            stream.Close();

            var response = (HttpWebResponse) httpWReq.GetResponse();

            var jsonresponse = "";

            using (var reader = new StreamReader(response.GetResponseStream()))
            {
                string temp;
                while ((temp = reader.ReadLine()!) != null) jsonresponse += temp;
            }


            return jsonresponse;
            //return new MomoResponse(mtid, jsonresponse);
        }
        catch (WebException e)
        {
            return e.Message;
        }
    }
}