using System.Net.Http.Json;

namespace _01_http_client
{
    public class ToDoItem
    {
        public int UserId { get; set; }
        public int Id { get; set; }
        public string Title { get; set; }
        public bool Completed { get; set; }

        public override string ToString()
        {
            return $"[{Id}]: {Title}, status: {Completed}";
        }
    }

    internal class Program
    {
        static async Task Main(string[] args)
        {
            // load page content
            ShowPageContent(@"https://www.google.com/blablabla");

            // get JSON object as class instance
            var item = await GetToDoItem(@"https://jsonplaceholder.typicode.com/todos/4");
            Console.WriteLine(item);

            // download file
            const string url = @"https://img.theculturetrip.com/wp-content/uploads/2017/09/field-2513145_1280.jpg";
            LoadFile(url);
        }

        private static void ShowPageContent(string url)
        {
            using (HttpClient client = new())
            {
                HttpRequestMessage request = new(HttpMethod.Get, url);

                HttpResponseMessage response = client.Send(request);

                Console.WriteLine("Status: " + response.StatusCode);

                string content = response.Content.ReadAsStringAsync().Result;

                Console.WriteLine("--------- Content ---------");
                Console.WriteLine(content);
            }
        }
        private async static Task<ToDoItem?> GetToDoItem(string url)
        {
            using (HttpClient client = new())
            {
                HttpRequestMessage request = new(HttpMethod.Get, url);

                HttpResponseMessage response = client.Send(request);

                Console.WriteLine("Status: " + response.StatusCode);

                return await response.Content.ReadFromJsonAsync<ToDoItem>();
            }
        }
        private async static void LoadFile(string url)
        {
            using (HttpClient client = new())
            {
                HttpRequestMessage request = new(HttpMethod.Get, url);
                HttpResponseMessage response = client.Send(request);

                Console.WriteLine("Status: " + response.StatusCode);
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                   Console.WriteLine("File not found!");
                   return;
                }

                // get desktop path
                string desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                // generate random name
                string name = Guid.NewGuid().ToString();
                // get original extension
                string extension = Path.GetExtension(url);
                // combine destination path
                string destination = Path.Combine(desktop, name + extension);

                File.WriteAllBytes(destination, await response.Content.ReadAsByteArrayAsync());
                await Console.Out.WriteLineAsync("File loaded!");
            }
        }
    }
}