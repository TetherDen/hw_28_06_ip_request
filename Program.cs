using System.Net.Http;
using Newtonsoft.Json;
namespace hw_28_06_ip_request
{
    public class Ip
    {
        public string? ip {  get; set; }
        public bool success {  get; set; }  
        public string country { get; set; }
        public string region { get; set; }
        public string city { get; set; }
        public override string ToString()
        {
            return $"Ip: {ip}, Country: {country}, Region: {region}, City: {city}";
        }

    }

    internal class Program
    {
        static async Task GetIpData(string ipAddress, Ip ip)   //
        {
            string url = "http://ipwho.is/"+ ipAddress;
            HttpClient client = new HttpClient();
            var response = await client.GetAsync(url);  //
            if(response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                if(content.Length > 0)
                {
                    var data = JsonConvert.DeserializeObject<Ip>(content);
                    if(data.success)
                    {
                        ip.ip = data.ip;
                        ip.success = data.success;
                        ip.country = data.country;
                        ip.region = data.region;
                        ip.city = data.city;
                        //Console.WriteLine($"Ip: {data.ip}");
                        //Console.WriteLine($"Country: {data.country}");
                        //Console.WriteLine($"Region: {data.region}");
                        //Console.WriteLine($"City: {data.city}");
                    }
                    else
                    {
                        throw new Exception("Error. Invalid IP address ?");
                    }
                }
            }

        }

        public static async Task AsyncGetIp(string ipAddress, Ip ip)
        {
            await Task.Run(() => GetIpData(ipAddress, ip));
        }


        public static void SerializeIp(Ip ip)
        {
            //string path = @"..\..\..\" + DateTime.Now.ToString() + ".json";
            string path = @"..\..\..\" + DateTime.Now.ToString("yyyy.mm.dd-hh_mm_ss_ff") + ".json";
            string jsonStr = JsonConvert.SerializeObject(ip);
            if(jsonStr.Length >0)
            {
                File.WriteAllText(path, jsonStr);
            }
        }

        static void Main(string[] args)
        {
            Ip ip = new Ip();
            Console.WriteLine("Enter IP to check");
            string? ipAddress = Console.ReadLine();
            try
            {
                AsyncGetIp(ipAddress, ip).Wait();  //
                SerializeIp(ip); 
                //GetIpData(ipAddress, ip).Wait();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            if(ip.success)
            {
                Console.WriteLine(ip);
            }

            Console.ReadLine();
        }
    }
}
