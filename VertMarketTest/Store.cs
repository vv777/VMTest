using System;
using System.Web;
using System.Net;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq; 

namespace VertMarketTest
{
    public class Store
    {
        private string _url = null;
        private string _token = null;

        private IList<string> _categories = null;
        private IList<Magazine> _magazines = new List<Magazine>();
        private IList<Subscriber> _subscribers = null;

        public IList<string> Categories { get { return _categories; } }
        public IList<Magazine> Magazines { get { return _magazines; } }
        public IList<Subscriber> Subscribers { get { return _subscribers; } }

        public Store(string url)
        {
            _url = url;

            this.GetEntities();
        }

        private void GetEntities()
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(_url + "/token" );
            request.Method = "GET";
            Evidence evidence = this.GetResponse<Evidence>(request);
            
            _token = evidence.token;

            request = (HttpWebRequest)WebRequest.Create(_url + "/categories/" + _token);
            request.Method = "GET";
            _categories = this.GetResponse<Categories>(request).data;

            // here comes the bottleneck...
            foreach (string category in _categories)
            {
                request = (HttpWebRequest)WebRequest.Create(string.Format("{0}/magazines/{1}/{2}", _url, _token, category));
                request.Method = "GET";
                Magazines magazines = this.GetResponse<Magazines>(request);
                foreach (Magazine magazine in magazines.data)
                {
                    _magazines.Add(magazine);
                }
            }

            request = (HttpWebRequest)WebRequest.Create(_url + "/subscribers/" + _token);
            request.Method = "GET";
            _subscribers = this.GetResponse<Subscribers>(request).data;
        }

        public string PostResult(Answer answer)
        {
            string data = JsonConvert.SerializeObject(answer);

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(_url + "/answer/" + _token);
            request.Method = "POST";
            request.ContentType = "application/json";
            request.ContentLength = data.Length;
            using (Stream requestStream = request.GetRequestStream())
            {
                using (StreamWriter requestWriter = new StreamWriter(requestStream, Encoding.ASCII))
                {
                    requestWriter.Write(data);
                }
            }

            string result = null;
            try
            {
                result = this.GetResponse(request);
            }
            catch (Exception ex)
            {
                result = ex.ToString();
            }

            return result;
        }

        private string GetResponse(HttpWebRequest request)
        {
            string data = null;

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using (Stream webStream = response.GetResponseStream())
            {
                using (StreamReader responseReader = new StreamReader(webStream))
                {
                    data = responseReader.ReadToEnd();
                }
            }

            return data;
        }

        private T GetResponse<T>(HttpWebRequest request)
        {
            string data = null;
            
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using (Stream webStream = response.GetResponseStream())
            {
                using (StreamReader responseReader = new StreamReader(webStream))
                {
                    data = responseReader.ReadToEnd();
                }
            }

            return JsonConvert.DeserializeObject<T>(data);
        }
    }
}
