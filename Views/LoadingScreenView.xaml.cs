using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace AnimeSearch.Views
{
    public class LoadingScreenView : UserControl
    {
        public LoadingScreenView()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
