using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Http;
using AnimeSearch.Models;
using System.Threading.Tasks;
using System.Diagnostics;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using ReactiveUI;

namespace AnimeSearch.ViewModels
{
    public class LoadingScreenViewModel : ViewModelBase
    {
        private static readonly HttpClient client = new HttpClient();
        private MainWindowViewModel context;
        private bool errorIsVisible;
        public bool ErrorIsVisible
        {
            get => errorIsVisible;
            set => this.RaiseAndSetIfChanged(ref errorIsVisible, value);
        }
        private string error = "Error(s): ";
        public string Error
        {
            get => error;
            set
            {
                this.RaiseAndSetIfChanged(ref error, value);
                ErrorIsVisible = true;
            }
        }

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
                    status
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

            //if you're connected to the internet (i think)
            if (response.StatusCode != HttpStatusCode.RequestTimeout)
            {
                Debug.WriteLine(response.StatusCode);
                try
                {
                    //where auth token is invalid
                    if (responseObject["errors"][0]["message"].ToString() == "Invalid token")
                    {
                        context.Content = new AuthorizationScreenViewModel(context);
                        Debug.WriteLine(responseObject["errors"][0].GetType());
                    }
                    //where something else went wrong
                    else
                    {
                        foreach (JObject error in responseObject["errors"])
                        {
                            Error += $"{error["message"].ToString()}\n";
                        }
                    }
                }
                catch
                {
                    //TODO switch to ListScreen
                    List<MediaItem> mediaList = new List<MediaItem>();
                    var root = responseObject["data"]["Page"]["mediaList"];
                    foreach (JObject entry in root)
                    {
                        int year = entry["media"]["startDate"]["year"].ToObject<int>();
                        int month = entry["media"]["startDate"]["month"].ToObject<int>();
                        int day = entry["media"]["startDate"]["day"].ToObject<int>();
                        DateTime date = new DateTime(year, month, day);

                        MediaItem newMedia = new MediaItem()
                        {
                            Name = entry["media"]["title"]["romaji"].ToString(),
                            Progress = entry["progress"].ToObject<int>(),
                            Episodes = entry["media"]["episodes"].ToObject<int?>(),
                            ReleaseType = entry["media"]["format"].ToString(),
                            StartDate = date.ToString(),
                            Status = entry["media"]["status"].ToString(),
                            Rating = entry["score"].ToObject<int>()
                        };
                        Debug.WriteLine("zzz");

                        mediaList.Add(newMedia);
                    }
                    await Task.Run(() => Database.WriteMediaList(mediaList));
                }
            }
            //if you're disconnected from the internet
            else
                Error += $"error 408 (check your connection)";
            
            Debug.WriteLine(responseString);
        }
    }
}