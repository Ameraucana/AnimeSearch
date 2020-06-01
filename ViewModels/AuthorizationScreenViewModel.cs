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
