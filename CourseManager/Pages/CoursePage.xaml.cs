using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using CourseManager.Model;
using CourseManager.ViewModel;

namespace CourseManager
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CoursePage : ContentPage
    {
        private ObservableCollection<Term> _term;
        private ObservableCollection<Term> _terms;

        public CoursePage()
        {
            InitializeComponent();
        }

        public CoursePage(Term rawTerm, ObservableCollection<Term> terms)
        {
            InitializeComponent();
            var termList = new List<Term>();
            termList.Add(rawTerm);
            var term = new ObservableCollection<Term>(termList);
            _term = term;
            _terms = terms;
        }

        public CoursePage(Term rawTerm)
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

            var emptyMessage_Tap = new TapGestureRecognizer();
            emptyMessage_Tap.Tapped += async (s, e) =>
            {
                await AddCourse();
            };
            EmptyMessage.GestureRecognizers.Add(emptyMessage_Tap);

            var rawCourses = await App.Database.GetCoursesAsync();
            var filteredCourses = rawCourses.Where(r => r.TermId == _term[0].Id).ToList();
            var courses = new ObservableCollection<Course>(filteredCourses);

            if (courses.Count > 0)
            {
                CoursesListView.IsVisible = true;
                EmptyMessage.IsVisible = false;
            }
            else
            {
                CoursesListView.IsVisible = false;
                EmptyMessage.IsVisible = true;
            }
            CoursesListView.ItemsSource = courses;
        }

        protected override bool OnBackButtonPressed()
        {
            var pageViewModel = new TermViewModel();

            var page = new TermPage(_term[0])
            {
                BindingContext = pageViewModel
            };

            this.Navigation.PushAsync(page);
            return true;
        }

        public async void Add_Clicked(object sender, EventArgs e)
        {
            await AddCourse();
        }

        private async Task AddCourse()
        {
            var rawCourses = await App.Database.GetCoursesAsync();
            var filteredCourses = rawCourses.Where(r => r.TermId == _term[0].Id);
            var courses = new ObservableCollection<Course>(filteredCourses);

            var termDueDate = _term[0].End;

            if (courses.Count == 6)
            {
                await DisplayAlert("Warning - Too many courses!", "You may not add more than 6 courses per term.", "Ok");
            }
            else
            {
                var detailViewModel = new CourseDetailViewModel();

                var detailPage = new CourseDetailPage(true, new Course(0, termDueDate), _term[0])
                {
                    BindingContext = detailViewModel // Set the binding context
                };

                await this.Navigation.PushAsync(detailPage);
            }
        }

        public async void BtnEdit_Clicked(object sender, EventArgs e)
        {
            var button = (ImageButton)sender;
            var courseId = Convert.ToInt32(button.CommandParameter.ToString());
            var course = await App.Database.GetCourseAsync(courseId);
            var viewModel = new CourseDetailViewModel();

            var assessments = await App.Database.GetAssessmentsAsync();
            var assessmentList = assessments.Where(r => r.CourseId == courseId);
            var assessmentsForCourse = new ObservableCollection<Assessment>(assessmentList);

            var page = new CourseDetailPage(false, course, _term[0], assessmentsForCourse)
            {
                BindingContext = viewModel
            };

            await this.Navigation.PushAsync(page);
        }

        private async void CoursesListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var course = (Course)e.Item;

            var viewModel = new CourseDetailViewModel();

            var assessments = await App.Database.GetAssessmentsAsync();
            var assessmentList = assessments.Where(r => r.CourseId == course.Id);
            var assessmentsForCourse = new ObservableCollection<Assessment>(assessmentList);

            var page = new CourseDetailPage(false, course, _term[0], assessmentsForCourse)
            {
                BindingContext = viewModel
            };

            await this.Navigation.PushAsync(page);
        }
    }
}