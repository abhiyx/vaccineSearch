using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using vaccine.Models;
using System.Linq;
using System.Threading;

namespace vaccine
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Thread.Sleep(2000);
                VaccineSlots slots = SearchSlots();
                var available = slots.centers.Any(x => x.sessions.Any(y => y.min_age_limit == 18 && y.available_capacity > 3));
                Console.WriteLine(available);
                if (available)
                {
                    Console.Beep();
                }
            }
        }

        public static VaccineSlots SearchSlots()
        {
            string html = string.Empty;
            string url = @"https://cdn-api.co-vin.in/api/v2/appointment/sessions/calendarByDistrict?district_id=230&date=06-05-2021";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.AutomaticDecompression = DecompressionMethods.GZip;

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                html = reader.ReadToEnd();
            }
            return JsonConvert.DeserializeObject<VaccineSlots>(html);
        }
    }
}
