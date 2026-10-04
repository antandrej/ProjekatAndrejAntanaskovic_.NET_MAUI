using ProjekatAndrejAntanaskovic.Views;

namespace ProjekatAndrejAntanaskovic
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(
                new EntryPage());
        }
    }
}
