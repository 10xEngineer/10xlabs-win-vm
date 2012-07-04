using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RestSharp;
using RestSharp.Contrib;
using System.Net;

namespace TenxLabsService
{
    class Ec2Metadata
    {
        private static string META_END = "http://169.254.169.254/";
        private const string META_DATA = "latest/meta-data/{key}";

        public static Boolean onEC2()
        {
            var instanceId = getKey("instance-id");

            if (instanceId == null) return false;

            return true;
        }

        public static string getKey(string key)
        {
            var response = getClient().Execute(buildRequest(key));

            if (response.StatusCode != HttpStatusCode.OK) return null;

            return response.Content;
        }

        public static string getCustomData()
        {
            var response = getClient().Execute<RestSharp.Serializers.JsonSerializer>(buildRequest("user-data", "/latest/{key}"));

            if (response.StatusCode != HttpStatusCode.OK) return null;

            return response.Content;
        }


        public static RestRequest buildRequest(string key, string path = META_DATA)
        {
            var request = new RestRequest(path, Method.GET);
            request.AddUrlSegment("key", key);

            return request;
        }

        public static RestClient getClient()
        {
            return new RestClient(META_END); ;
        }

    }
}
