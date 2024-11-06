using cMainatoS5B.DataService;

namespace cMainatoS5B
{
    public partial class App : Application
    {
        public static DatabaseService Database { get; private set; }

        public App()
        {
             
        InitializeComponent();

        Database = new DatabaseService(AppConstants.DatabasePath);
        MainPage = new NavigationPage(new Pages.MainPage());
        }
    }
}
