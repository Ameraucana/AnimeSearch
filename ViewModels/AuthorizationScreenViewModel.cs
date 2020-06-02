using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.IO;
using ReactiveUI;

namespace AnimeSearch.ViewModels
{
    public class AuthorizationScreenViewModel : ViewModelBase
    {
        public AuthorizationScreenViewModel(MainWindowViewModel context)
        {
            this.context = context;
            var startInfo = new ProcessStartInfo
            {
                FileName = "https://anilist.co/api/v2/oauth/authorize?client_id=3328&response_type=token",
                UseShellExecute = true
            };
            Process.Start(startInfo);
        }
        private string key;
        public string Key
        {
            get => key;
            set => key = value;
        }
        private MainWindowViewModel context;

        public async void SubmitCommand()
        {
            await File.WriteAllTextAsync("./Assets/Files/token.txt", Key);
            context.Content = new LoadingScreenViewModel(context);
        }
    }
}
