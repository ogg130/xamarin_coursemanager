using Plugin.LocalNotifications;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using CourseManager.Model;
using CourseManager.ViewModel;

namespace CourseManager
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TermPage : ContentPage
    {
        private ObservableCollection<Term> _term;
        private ObservableCollection<Term> _terms;
        private bool _firstLoad = false;

        public TermPage()
        {
            InitializeComponent();
        }

        public TermPage(Term rawTerm)
        {
            InitializeComponent();
            var termList = new List<Term>();
            termList.Add(rawTerm);
            var term = new ObservableCollection<Term>(termList);
            _term = term;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            var settings = new List<Setting>();
            try
            {
                settings = await App.Database.GetSettingsAsync();
            }
            catch
            {
                settings = null;
            }
            //settings = null;
            if (settings == null)
            {
                App.Database.DropTables();
                App.Database.BuildTables();

                var data = new StartData();
                foreach (var setting in data.Settings)
                {
                    await App.Database.SaveSettingAsync(setting);
                }
                foreach (var term in data.Terms)
                {
                    await App.Database.SaveTermAsync(term);
                }
                var assessments = new List<Assessment>();
                foreach (var assessment in data.Assessments)
                {
                    await App.Database.SaveAssessmentAsync(assessment);
                    assessments.Add(assessment);
                }

                foreach (var course in data.Courses)
                {
                    await App.Database.SaveCourseAsync(course);
                }
            }

            var courses = await App.Database.GetCoursesAsync();
            foreach (var course in courses)
            {
                if (course.Start.Date == DateTime.Now.Date)
                {
                    var action = "Starting";
                    var date = course.Start;

                    var noteMessage = $"{action} today:\n\n{course.CourseCode} - {course.Name}";
                    var noteTitle = $"{course.CourseCode} - {action} today";
                    var newNote = new LocalNotification(0, noteTitle, noteMessage, date, course.Id, -1);

                    await App.Database.SaveNotificationAsync(newNote);
                    CrossLocalNotifications.Current.Show(noteTitle, noteMessage, newNote.Id, date);
                }

                if (course.End.Date == DateTime.Now.Date)
                {
                    var action = "Ending";
                    var date = course.End;

                    var noteMessage = $"{action} today:\n\n{course.CourseCode} - {course.Name}";
                    var noteTitle = $"{course.CourseCode} - {action} today";
                    var newNote = new LocalNotification(0, noteTitle, noteMessage, date, course.Id, -1);

                    await App.Database.SaveNotificationAsync(newNote);
                    CrossLocalNotifications.Current.Show(noteTitle, noteMessage, newNote.Id, date);
                }

                //Get assessments for course - iterate,
                var assessments = await App.Database.GetAssessmentsAsync();
                var filteredAssessments = assessments.Where(r => r.CourseId == course.Id);
                foreach (var assessment in filteredAssessments)
                {
                    if (assessment.Start.Date == DateTime.Now.Date)
                    {
                        var action = "Starting";
                        var date = assessment.Start;

                        // Add new notification
                        var noteMessage = $"{action} today:\n\n{course.CourseCode} - {course.Name}";
                        var noteTitle = $"{course.CourseCode} - {assessment.Name} - {action} today";
                        var newNote = new LocalNotification(0, noteTitle, noteMessage, date, course.Id, assessment.Id);

                        await App.Database.SaveNotificationAsync(newNote);
                        CrossLocalNotifications.Current.Show(noteTitle, noteMessage, newNote.Id, date);
                    }

                    if (assessment.End.Date == DateTime.Now.Date)
                    {
                        var action = "Ending";
                        var date = assessment.End;

                        // Add new notification
                        var noteMessage = $"{action} today:\n\n{course.CourseCode} - {course.Name}";
                        var noteTitle = $"{course.CourseCode} - {assessment.Name} - {action} today";
                        var newNote = new LocalNotification(0, noteTitle, noteMessage, date, course.Id, assessment.Id);

                        await App.Database.SaveNotificationAsync(newNote);
                        CrossLocalNotifications.Current.Show(noteTitle, noteMessage, newNote.Id, date);
                    }
                }
            }

            var emptyMessage_Tap = new TapGestureRecognizer();
            emptyMessage_Tap.Tapped += async (s, e) =>
            {
                await AddTerm();
            };
            EmptyMessage.GestureRecognizers.Add(emptyMessage_Tap);

            var termz = await App.Database.GetTermsAsync();
            if (termz.Count > 0)
            {
                lstTerms.IsVisible = true;
                EmptyMessage.IsVisible = false;
            }
            else
            {
                lstTerms.IsVisible = false;
                EmptyMessage.IsVisible = true;
            }

            var viewModel = (TermViewModel)this.BindingContext;
            if (viewModel.Term == null)
            {
                var rawTerm = await App.Database.GetTermAsync(1);
                var termList = new List<Term>();
                termList.Add(rawTerm);
                var term = new ObservableCollection<Term>(termList);
                _term = term;
            }
            var rawTerms = await App.Database.GetTermsAsync();
            var terms = new ObservableCollection<Term>(rawTerms);
            _terms = terms;

            lstTerms.ItemsSource = terms;
        }

        protected override bool OnBackButtonPressed()
        {
            System.Diagnostics.Process.GetCurrentProcess().CloseMainWindow();
            return true;
        }

        public async void BtnEdit_Clicked(object sender, EventArgs e)
        {
            var button = (ImageButton)sender;
            var termId = Convert.ToInt32(button.CommandParameter.ToString());

            var detailViewModel = new TermDetailViewModel();

            var currentTerm = await App.Database.GetTermAsync(termId);
            var page = new TermDetailPage(false, currentTerm)
            {
                BindingContext = detailViewModel
            };

            await this.Navigation.PushAsync(page, true);
        }

        public async void BtnAdd_Clicked(object sender, EventArgs e)
        {
            await AddTerm();
        }

        private async System.Threading.Tasks.Task AddTerm()
        {
            var detailViewModel = new TermDetailViewModel();

            var detailPage = new TermDetailPage(true, new Term(0))
            {
                BindingContext = detailViewModel
            };

            await this.Navigation.PushAsync(detailPage, true);
        }

        private async void LstTerms_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var term = (Term)e.Item;

            var courseViewModel = new CourseViewModel();

            var coursesList = await App.Database.GetCoursesAsync();

            var filteredCourses = coursesList.Where(r => r.TermId == term.Id).ToList();
            var courses = new ObservableCollection<Course>(filteredCourses);
            courseViewModel.Courses = courses;

            var coursePage = new CoursePage(term, _terms)
            {
                BindingContext = courseViewModel
            };

            await this.Navigation.PushAsync(coursePage, true);
        }
    }
}