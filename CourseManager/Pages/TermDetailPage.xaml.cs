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
    public partial class TermDetailPage : ContentPage
    {
        private readonly bool _addFlag;
        private ObservableCollection<Term> _term;
        private ObservableCollection<Term> _terms;

        public TermDetailPage(bool add, Term rawTerm)
        {
            InitializeComponent();
            _addFlag = add;

            if (add)
            {
                BtnDelete.IsVisible = false;
                LblHeader.Text = "Add Term";
            }
            var termList = new List<Term>();
            termList.Add(rawTerm);
            var term = new ObservableCollection<Term>(termList);

            _term = term;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (!_addFlag)
            {
                TxtName.Text = _term[0].Name;
            }
        }

        protected override bool OnBackButtonPressed()
        {
            Return();
            return true;
        }

        private async void Return()
        {
            var viewModel = new TermViewModel();

            var page = new TermPage(_term[0])
            {
                BindingContext = viewModel
            };

            await this.Navigation.PushAsync(page, true);
        }

        private async void BtnSave_Clicked(object sender, EventArgs e)
        {
            var name = TxtName.Text;
            var start = DtpStart.Date;
            var end = DtpEnd.Date;
            var now = DateTime.Now.Date;
            var term = _term[0];

            if (name == null || name == "")
            {
                await DisplayAlert("Warning - Missing Data!", "Terms must have a name.", "Ok");
            }
            else if (start < now && _addFlag)
            {
                await DisplayAlert("Warning - Data Problem!", "Start dates must not be in the past.", "Ok");
            }
            else if (end < now && _addFlag)
            {
                await DisplayAlert("Warning - Data Problem!", "End dates must not be in the past.", "Ok");
            }
            else if (start > end)
            {
                await DisplayAlert("Warning - Data Problem!", "Start date cannot be after end date.", "Ok");
            }
            else
            {
                // false, this indicates an update
                var title = "Term updated.";
                var message = "Your term has been updated.";
                if (_addFlag)
                {
                    // If true, this indicates an add
                    title = "Term added.";
                    message = "Your term has been added.";
                }

                term.Name = name;
                term.Start = start;
                term.End = end;

                await App.Database.SaveTermAsync(_term[0]);

                await DisplayAlert(title, message, "Ok");

                Return();
            }
        }

        public async void BtnDelete_Clicked(object sender, EventArgs e)
        {
            var term = _term[0];
            var courses = await App.Database.GetCoursesAsync();
            var coursesInTerm = courses.Where(r => r.TermId == term.Id).ToList();

            if (coursesInTerm.Count != 0)
            {
                await DisplayAlert($"Delete Term - {term.Name} - Warning!", "You may not delete terms that have courses in them. If you wish to delete the term, first delete the course and then delete the term.", "Ok");
            }
            else
            {
                //Throw a yes / no prompt to confirm delete
                var answer = await DisplayAlert($"Delete Term - {term.Name}?", $"Are you sure you wish to delete term - {term.Name}?", "Yes", "No");

                if (answer) // If the delete is confirmed
                {
                    // Remove the term
                    await App.Database.DeleteTermAsync(term);
                    Return();
                }
            }
        }
    }
}