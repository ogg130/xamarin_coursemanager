using CourseManager.Data;
using CourseManager.ViewModel;
using System;
using System.IO;
using Xamarin.Forms;

namespace CourseManager
{
    public partial class App : Application
    {
        private static Database _database;

        public static Database Database
        {
            get
            {
                if (_database == null)
                {
                    var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Notes.db3");

                    _database = new Database(path);
                }
                return _database;
            }
        }

        public App()
        {
            InitializeComponent();

            var viewModel = new TermViewModel();

            var page = new TermPage()
            {
                BindingContext = viewModel
            };

            MainPage = new NavigationPage(page);
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}