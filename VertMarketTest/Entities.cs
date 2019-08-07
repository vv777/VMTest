using System;
using System.Collections.Generic;

namespace VertMarketTest
{
    public class Evidence
    {
        public bool success { get; set; }
        public string token { get; set; }
        public string message { get; set; }
    }

    public class Categories
    {
        public bool success { get; set; }
        public string token { get; set; }
        public string message { get; set; }
        public IList<string> data { get; set; }
    }

    public class Magazines
    {
        public bool success { get; set; }
        public string token { get; set; }
        public string message { get; set; }
        public IList<Magazine> data { get; set; }
    }

    public class Magazine
    {
        public int id { get; set; }
        public string name { get; set; }
        public string category { get; set; }
    }

    public class Subscribers
    {
        public bool success { get; set; }
        public string token { get; set; }
        public string message { get; set; }
        public IList<Subscriber> data { get; set; }

    }

    public class Subscriber
    {
        public string id { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public IList<int> magazineIds { get; set; }
    }

    public class Answer
    {
        public Answer()
        {
            this.subscribers = new List<string>();
        }

        public IList<string> subscribers { get; private set; }
    }
}
