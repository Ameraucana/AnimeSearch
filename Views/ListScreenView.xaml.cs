using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace AnimeSearch.Views
{
    public class ListScreenView : UserControl
    {
        public ListScreenView()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
