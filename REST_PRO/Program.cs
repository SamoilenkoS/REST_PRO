using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace REST_PRO
{
    public class Post
    {
        public int Id { get; set; }
        [JsonPropertyName("post_id")]
        public int PostId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Body { get; set; }
    }

    class Program
    {
        private static string CommentsPath = "public/v2/comments";
        static Random random = new Random();
        public static RestRequest Get()
        {
            RestRequest request = new RestRequest(CommentsPath);

            return request;
        }

        public static RestRequest Get(int id)
        {
            var request = new RestRequest(CommentsPath + $"/{id}");

            return request;
        }

        public static RestRequest Post(Post post)
        {
            var request = new RestRequest(CommentsPath);
            request.AddQueryParameter("page", 1);
            request.AddBody(post);
            request.Method = Method.Post;

            return request;
        }

        static string GetRandom(int min = 5, int max = 25)
        {
            var length = random.Next(min, max);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < length; i++)
            {
                sb.Append((char)random.Next('a', 'z'));
            }

            return sb.ToString();
        }

        static string GetRandomMail()
        {
            return GetRandom(5, 10) + "@gmail.com";
        }

        static async Task Main(string[] args)
        {
            using var client = new RestClient("https://gorest.co.in/");
            client.Authenticator = new JwtAuthenticator("925264f91826142ffa2640ef84757deab3e20c4a3d61cc5c559f831d31932aaf");
            int count;
            int current = 1;
            int input;
            do
            {
                Console.WriteLine("1 - Add");
                Console.WriteLine("2 - Get by id");
                Console.WriteLine("3 - Get all (by page)");
                Console.WriteLine("0 - exit");
                input = Convert.ToInt32(Console.ReadLine());
                RestRequest request = null;
                switch (input)
                {
                    case 1:
                        request = Post(
                            new Post
                            {
                                Body = GetRandom(),
                                Email = GetRandomMail(),
                                Name = GetRandom(),
                                PostId = 1000
                            });
                        break;
                    case 2:
                        Console.Write("ID:");
                        int id = Convert.ToInt32(Console.ReadLine());
                        request = Get(id);
                        break;
                    case 3:
                        request = Get();
                        break;
                    default:
                        return;
                        break;
                }

                var response = await client.ExecuteAsync(request);
                Console.WriteLine(response.Content);
            } while (input != 0);

        }
    }
}
