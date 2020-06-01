using System;
using System.Collections.Generic;
using System.Text;
using ReactiveUI;

namespace AnimeSearch.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public MainWindowViewModel()
        {
            Content = new LoadingScreenViewModel();
        }
        private ViewModelBase content;
        public ViewModelBase Content
        {
            get => content;
            set => this.RaiseAndSetIfChanged(ref content, value);
        }
    }
}
