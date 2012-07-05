using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using RestSharp;
using System.Globalization;
using System.Dynamic;
using System.Data.Entity.Design.PluralizationServices;

namespace TenxLabsService
{
    class Microcloud
    {
        private RestClient _client;
        private PluralizationService ps;

        public Microcloud(string endpoint)
        {
            // FIXME hardcoded endpoint
            this._client = new RestClient(endpoint);
            this.ps = PluralizationService.CreateService(CultureInfo.GetCultureInfo("en-us"));
        }

        public void notify(string resource, string res_id, string action, Dictionary<string,string> descriptor)
        {
            var resources = ps.Pluralize(resource);

            var request = new RestRequest(resources + "/{id}/notify", Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.AddUrlSegment("id", res_id);

            // prepare request body
            dynamic flexible = new ExpandoObject();
            var body = (IDictionary<string, object>) flexible;

            body.Add("action", action);
            body.Add(resource, descriptor);

            request.AddBody(body);

            var response = this._client.Execute(request);

            // TODO handle response (error handling, logging)
        }
    }
}
