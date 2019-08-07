using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VertMarketTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Store store = new Store("http://magazinestore.azurewebsites.net/api");

            int categoryCount = store.Categories.Count; 

            List<Subscriber> list = new List<Subscriber>();
            foreach (Subscriber subscriber in store.Subscribers)
            {
                int subscriberCategoryCount = (from magazine in store.Magazines
                                               where subscriber.magazineIds.Contains(magazine.id)
                                               select magazine.category).Distinct().Count();

                if (subscriberCategoryCount == categoryCount) list.Add(subscriber); 
            }

            Answer answer = new Answer();
            foreach (Subscriber subscriber in list)
            {
                answer.subscribers.Add(subscriber.id);

                Console.WriteLine(string.Format("{0}: {1} {2}", subscriber.id, subscriber.firstName, subscriber.lastName));  
            }

            Console.WriteLine();
            Console.WriteLine("Service response:");
            Console.WriteLine();
            Console.WriteLine(store.PostResult(answer)); 

            Console.WriteLine();
            Console.WriteLine("Press your favorite key to exit...");
            Console.ReadKey();
        }
    }
}
