using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace AnimeSearch.Views
{
    public class AuthorizationScreenView : UserControl
    {
        public AuthorizationScreenView()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
