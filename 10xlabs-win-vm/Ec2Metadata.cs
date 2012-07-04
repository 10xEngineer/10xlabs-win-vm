using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RestSharp;

namespace TenxLabsService
{
    class Ec2Metadata
    {
        private static string META_END = "http://169.254.169.254/";
        private static string META_DATA = "latest/meta-data/{key}";

        public static string getKey(string key)
        {
            string res = null;

            var client = new RestClient(META_END);

            var request = new RestRequest(META_DATA, Method.GET);
            request.AddUrlSegment("key", key);

            var response = client.Execute(request);

            Console.WriteLine(response.Content);

            return response.Content;
        }

    }
}
