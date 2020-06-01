using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using AnimeSearch.Models;
using System.Threading.Tasks;
using System.Diagnostics;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;

namespace AnimeSearch.ViewModels
{
    public class LoadingScreenViewModel : ViewModelBase
    {
        private static readonly HttpClient client = new HttpClient();
        private MainWindowViewModel context;

        public LoadingScreenViewModel(MainWindowViewModel context)
        {
            this.context = context;
            string query =
            @"
            query($user: String, $type: MediaType, $status: MediaListStatus) {
              Page {
                mediaList(userName: $user, type: $type, status: $status) {
                  progress
                  id
                  score
                  media {
                    title {romaji}
                    coverImage {large}
                    startDate {
                      year
                      month
                      day
                    }
                    format
                    nextAiringEpisode {
                      episode
                      timeUntilAiring
                    }
                    episodes
                  }
                }
              }
            }
            ";
            Dictionary<string, object> variables = new Dictionary<string, object>
            {
                { "user", "Araucana" },
                { "type", "ANIME" },
                { "status", "CURRENT" }
            };

            Query content = new Query(query, variables);
            GetApiData(JsonConvert.SerializeObject(content));
        }

        private async void GetApiData(string postContent)
        {
            string token = await File.ReadAllTextAsync("./Assets/Files/token.txt");

            var request = new HttpRequestMessage(HttpMethod.Post, "https://graphql.anilist.co");
            request.Content = new StringContent(postContent, Encoding.UTF8, "application/json");
            request.Headers.Authorization = 
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var response = await client.SendAsync(request);
            var responseString = await response.Content.ReadAsStringAsync();

            var responseObject = JObject.Parse(responseString);
            try
            {
                if (responseObject["errors"][0]["message"].ToString() == "Invalid token")
                {
                    context.Content = new AuthorizationScreenViewModel(context);
                    Debug.WriteLine("invalid token");
                }
            }
            catch
            {
                //TODO write to database
                //TODO switch to ListScreen
                Debug.WriteLine("end of path");
                
            }
            Debug.WriteLine(responseString);
        }
    }
}