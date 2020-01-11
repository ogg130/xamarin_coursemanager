using Plugin.LocalNotifications;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using CourseManager.Model;
using CourseManager.ViewModel;

namespace CourseManager
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CourseDetailPage : ContentPage
    {
        private readonly bool _addFlag;
        private List<Assessment> _preDelete;
        private List<int> _toDelete = new List<int>();
        private ObservableCollection<Course> _course;
        private ObservableCollection<Term> _term;
        private ObservableCollection<Assessment> _assessments;

        public CourseDetailPage(bool add, Course rawCourse, Term rawTerm, ObservableCollection<Assessment> assessments)
        {
            InitializeComponent();
            _addFlag = add;

            var courseList = new List<Course>
            {
                rawCourse
            };
            var course = new ObservableCollection<Course>(courseList);
            _course = course;

            var termList = new List<Term>
            {
                rawTerm
            };
            var term = new ObservableCollection<Term>(termList);
            _term = term;

            _assessments = assessments;

            if (add)
            {
                BtnDelete.IsVisible = false;
                LblHeader.Text = "Add Course";
            }
            PkrStatus.SelectedItem = course[0].Status;
        }

        public CourseDetailPage(bool add, Course rawCourse, Term rawTerm)
        {
            InitializeComponent();
            _addFlag = add;

            var courseList = new List<Course>
            {
                rawCourse
            };
            var course = new ObservableCollection<Course>(courseList);
            _course = course;

            var termList = new List<Term>
            {
                rawTerm
            };
            var term = new ObservableCollection<Term>(termList);
            _term = term;

            if (add)
            {
                BtnDelete.IsVisible = false;
                LblHeader.Text = "Add Course";
            }
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            var emptyMessage_Tap = new TapGestureRecognizer();
            emptyMessage_Tap.Tapped += async (s, e) =>
            {
                await AddAssessment();
            };
            EmptyMessage.GestureRecognizers.Add(emptyMessage_Tap);

            var course = _course[0];

            if (!_addFlag)
            {
                TxtCourseCode.Text = course.CourseCode;
                TxtCu.Text = course.Cu.ToString();
                TxtName.Text = course.Name;
                TxtNotes.Text = course.Notes;
                TxtInstructorName.Text = course.InstructorName;
                TxtInstructorEmail.Text = course.InstructorEmail;
                TxtInstructorPhone.Text = course.InstructorPhone;
                PkrStatus.SelectedItem = course.Status;
                DtpStart.Date = course.Start;
                DtpEnd.Date = course.End;
                if (_assessments.Count == 0)
                {
                    LstAssessments.IsVisible = false;
                    EmptyMessage.IsVisible = true;

                    var assessments = new ObservableCollection<Assessment>();
                    LstAssessments.ItemsSource = assessments;
                }
                LstAssessments.ItemsSource = _assessments;
            }
            else
            {
                LstAssessments.IsVisible = false;
                EmptyMessage.IsVisible = true;

                var assessments = new ObservableCollection<Assessment>();
                LstAssessments.ItemsSource = assessments;
            }
            DtpDueDate.Date = course.DueDate;
        }

        protected override bool OnBackButtonPressed()
        {
            Return();
            return true;
        }

        private async void Return()
        {
            var viewModel = new CourseViewModel();

            var page = new CoursePage(_term[0]);
            {
                BindingContext = viewModel;
            };

            await this.Navigation.PushAsync(page);
        }

        private async void BtnShare_Clicked(object sender, EventArgs e)
        {
            if (TxtNotes.Text != "" && TxtNotes.Text != null)
            {
                await Share.RequestAsync(new ShareTextRequest
                {
                    Text = TxtNotes.Text,
                    Title = "Share Notes"
                });
            }
            else
            {
                await DisplayAlert("Warning - Missing Data!", "You must first enter notes to be able to share them.", "Ok");
            }
        }

        private async void Save_Clicked(object sender, EventArgs e)
        {
            var course = _course[0];
            int cu = 0;
            var validCu = (int.TryParse(TxtCu.Text, out int n));
            if (validCu)
            {
                cu = Convert.ToInt32(TxtCu.Text);
            }

            var status = "";
            var statusNull = (PkrStatus.SelectedItem == null);
            if (!statusNull)
            {
                status = PkrStatus.SelectedItem.ToString();
            }

            var courseCode = TxtCourseCode.Text;

            var courseName = TxtName.Text;
            var dueDate = DtpDueDate.Date;
            var end = DtpEnd.Date;
            var instructorEmail = TxtInstructorEmail.Text;
            var instructorName = TxtInstructorName.Text;
            var instructorPhone = TxtInstructorPhone.Text;
            var name = TxtName.Text;
            var notes = TxtNotes.Text;
            var start = DtpStart.Date;
            var due = DtpDueDate.Date;

            //// Setup regex for phone and postal code validations
            var phoneRegex = new Regex(@"^(\+\d{1,2}\s)?\(?\d{3}\)?[\s.-]?\d{3}[\s.-]?\d{4}$");
            var emailRegex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            var noCode = (courseCode == "" || courseCode == null);
            var noName = (name == "" || name == null);
            var invalidPhoneNumber = false;
            if (instructorPhone != null && !phoneRegex.IsMatch(instructorPhone))
            {
                invalidPhoneNumber = true;
            }
            var invalidEmail = false;
            if (instructorEmail != null && !emailRegex.IsMatch(instructorEmail))
            {
                invalidEmail = true;
            }

            if (_preDelete != null && _preDelete.Count == 1 && (_preDelete[0].Name == null || _preDelete[0].Name == ""))
            {
                await DisplayAlert("Warning - Invalid Data!", "The first assessment you added is missing an assessment name. Please resolve.", "Ok");
                return;
            }
            else if (_preDelete != null && _preDelete.Count == 1 && _preDelete[0].Type == null)
            {
                await DisplayAlert("Warning - Invalid Data!", "The first assessment you added is missing an assessment type. Please resolve.", "Ok");
                return;
            }
            else if (_preDelete != null && _preDelete.Count == 2 && (_preDelete[0].Name == null || _preDelete[0].Name == ""))
            {
                await DisplayAlert("Warning - Invalid Data!", "The first assessment you added is missing an assessment name. Please resolve.", "Ok");
                return;
            }
            else if (_preDelete != null && _preDelete.Count == 2 && _preDelete[0].Type == null)
            {
                await DisplayAlert("Warning - Invalid Data!", "The first assessment you added is missing an assessment type. Please resolve.", "Ok");
                return;
            }
            else if (_preDelete != null && _preDelete.Count == 2 && (_preDelete[1].Name == null || _preDelete[1].Name == ""))
            {
                await DisplayAlert("Warning - Invalid Data!", "The second assessment you added is missing an assessment name. Please resolve.", "Ok");
                return;
            }
            else if (_preDelete != null && _preDelete.Count == 2 && _preDelete[1].Type == null)
            {
                await DisplayAlert("Warning - Invalid Data!", "The second assessment you added is missing an assessment type. Please resolve.", "Ok");
                return;
            }
            else if (_preDelete != null && _preDelete.Count == 2 && _preDelete[0].Type == _preDelete[1].Type)
            {
                await DisplayAlert("Warning - Incorrect Data!", "You may only assign one Performance Assessment and one Objective Assessment to a course.", "Ok");
                return;
            }
            else if (instructorName == "" || instructorName == null)
            {
                await DisplayAlert("Warning - Invalid Data!", "Please enter an instructor name.", "Ok");
                return;
            }
            else if (instructorPhone == "" || instructorPhone == null)
            {
                await DisplayAlert("Warning - Invalid Data!", "Please enter an instructor phone number.", "Ok");
                return;
            }
            else if (instructorEmail == "" || instructorEmail == null)
            {
                await DisplayAlert("Warning - Invalid Data!", "Please enter an instructor email address.", "Ok");
                return;
            }
            else if (!validCu)
            {
                await DisplayAlert("Warning - Invalid Data!", "Course CUs must be a numeric value.", "Ok");
                return;
            }
            else if (statusNull)
            {
                await DisplayAlert("Warning - Missing Data!", "Courses must have a status.", "Ok");
                return;
            }
            else if (noName)
            {
                await DisplayAlert("Warning - Missing Data!", "Courses must have a course name.", "Ok");
                return;
            }
            else if (noCode)
            {
                await DisplayAlert("Warning - Missing Data!", "Courses must have a course code.", "Ok");
                return;
            }
            else if (invalidPhoneNumber)
            {
                await DisplayAlert("Warning - Invalid Data!", "The phone number you have entered is not a proper phone number format.\n\nAppropriate style examples would be:\n\n123-456-7890\n(123)456-7890\n123 456 7890\n123.456.7890\n+ 91(123) 456-7890\n1234567890", "Ok");
                return;
            }
            else if (invalidEmail)
            {
                await DisplayAlert("Warning - Missing Data!", "The email adddress you have entered is not a proper email address format.", "Ok");
                return;
            }
            else if (start < DateTime.Now.Date && _addFlag)
            {
                await DisplayAlert("Warning - Data Problem!", "Start dates must not be in the past.", "Ok");
                return;
            }
            else if (end < DateTime.Now.Date && _addFlag)
            {
                await DisplayAlert("Warning - Data Problem!", "End dates must not be in the past.", "Ok");
                return;
            }
            else if (start > end.Date)
            {
                await DisplayAlert("Warning - Data Problem!", "Start date cannot be after end date.", "Ok");

                return;
            }
            else if (end.Date > dueDate.Date)
            {
                await DisplayAlert("Warning - Data Problem!", "End date cannot be after due date.", "Ok");
                return;
            }
            else if (start.Date > dueDate.Date)
            {
                await DisplayAlert("Warning - Data Problem!", "Start date cannot be after due date.", "Ok");
                return;
            }
            else
            {
                var notifications = await App.Database.GetNotificationsAsync();
                var filteredNotes = notifications.Where(r => r.CourseId == course.Id).ToList();

                foreach (var note in filteredNotes)
                {
                    await App.Database.DeleteNotificationAsync(note);
                    CrossLocalNotifications.Current.Cancel(note.Id);
                }

                var courseId = course.Id;

                var title = "Course updated.";
                var message = "Your course has been updated.";
                if (_addFlag)
                {
                    title = "Course added.";
                    message = "Your course has been added.";
                }

                course.Cu = cu;
                course.TermId = _term[0].Id;
                course.CourseCode = courseCode;
                course.DueDate = dueDate;
                course.End = end;
                course.InstructorEmail = instructorEmail;
                course.InstructorName = instructorName;
                course.InstructorPhone = instructorPhone;
                course.Name = name;
                course.Notes = notes;
                course.Start = start;
                course.Status = status;

                await App.Database.SaveCourseAsync(_course[0]);

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

                var assessmentz = (ObservableCollection<Assessment>)LstAssessments.ItemsSource;
                foreach (var assessment in assessmentz)
                {
                    assessment.CourseId = course.Id;
                    await App.Database.SaveAssessmentAsync(assessment);
                }

                if (_toDelete.Count > 0)
                {
                    foreach (var recordId in _toDelete)
                    {
                        var recordToDelete = await App.Database.GetAssessmentAsync(recordId);
                        if (recordToDelete != null)
                        {
                            await App.Database.DeleteAssessmentAsync(recordToDelete);
                        }
                    }
                }

                var assessments = await App.Database.GetAssessmentsAsync();
                var filteredAssessments = assessments.Where(r => r.CourseId == course.Id).ToList();
                foreach (var azzezzment in filteredAssessments)
                {
                    if (azzezzment.Start.Date == DateTime.Now.Date)
                    {
                        var action = "Starting";
                        var date = azzezzment.Start;

                        // Add new notification
                        var noteMessage = $"{action} today:\n\n{course.CourseCode} - {course.Name}";
                        var noteTitle = $"{course.CourseCode} - {azzezzment.Name} - {action} today";
                        var newNote = new LocalNotification(0, noteTitle, noteMessage, date, course.Id, azzezzment.Id);

                        await App.Database.SaveNotificationAsync(newNote);
                        CrossLocalNotifications.Current.Show(noteTitle, noteMessage, newNote.Id, date);
                    }

                    if (azzezzment.End.Date == DateTime.Now.Date)
                    {
                        var action = "Ending";
                        var date = azzezzment.End;

                        // Add new notification
                        var noteMessage = $"{action} today:\n\n{course.CourseCode} - {course.Name}";
                        var noteTitle = $"{course.CourseCode} - {azzezzment.Name} - {action} today";
                        var newNote = new LocalNotification(0, noteTitle, noteMessage, date, course.Id, azzezzment.Id);

                        await App.Database.SaveNotificationAsync(newNote);
                        CrossLocalNotifications.Current.Show(noteTitle, noteMessage, newNote.Id, date);
                    }
                }

                await DisplayAlert(title, message, "Ok");

                Return();
            }
        }

        public async void BtnAddAssessment_Clicked(object sender, EventArgs e)
        {
            await AddAssessment();
        }

        public async void BtnDeleteAssessment_Clicked(object sender, EventArgs e)
        {
            //Get whats in the list before delete
            var button = (ImageButton)sender;
            var assessmentId = Convert.ToInt32(button.CommandParameter.ToString());
            var assessment = await App.Database.GetAssessmentAsync(assessmentId);

            if (assessment != null)
            {
                var answer = await DisplayAlert($"Are you sure you wish to delete assessment - {assessment.Name}?", $"Delete Assessment - {assessment.Name}?", "Yes", "No");

                if (answer) // If the delete is confirmed
                {
                    var courseId = assessment.CourseId;
                    var rawAssessments = (ObservableCollection<Assessment>)LstAssessments.ItemsSource;
                    var assessmentsForCourse = rawAssessments.Where(r => r.CourseId == courseId).ToList();
                    assessmentsForCourse.RemoveAll(r => r.Id == assessmentId);
                    var assessments = new ObservableCollection<Assessment>(assessmentsForCourse);

                    if (assessments.Count == 0)
                    {
                        LstAssessments.IsVisible = false;
                        EmptyMessage.IsVisible = true;
                    }
                    LstAssessments.ItemsSource = assessments;
                    _toDelete.Add(assessmentId);
                    var inPreDelete = _preDelete.FirstOrDefault(r => r.Id == assessmentId);
                    _preDelete.Remove(inPreDelete);
                }
                else
                {
                    var allAssessments = await App.Database.GetAssessmentsAsync();
                    var next = allAssessments.Max(r => r.Id);
                    var newAssessment = new Assessment(next, assessment.Name, assessment.Start, assessment.End, assessment.Type, _course[0].Id);
                    await App.Database.SaveAssessmentAsync(newAssessment);
                }
            }
            else
            {
                var rawAssessments = (ObservableCollection<Assessment>)LstAssessments.ItemsSource;
                rawAssessments.RemoveAt(0);
                var assessments = new ObservableCollection<Assessment>(rawAssessments);
                LstAssessments.ItemsSource = assessments;
            }

            if (_preDelete != null)
            {
                if (_preDelete.Count == 1)
                {
                    var preDelete = new ObservableCollection<Assessment>(_preDelete);
                    var tempListView = new ListView
                    {
                        ItemsSource = preDelete
                    };

                    var list = (ObservableCollection<Assessment>)tempListView.ItemsSource;

                    if (assessment == null && list.Count == 0)
                    {
                        var previousAssessment = (ObservableCollection<Assessment>)tempListView.ItemsSource;
                        if (previousAssessment.Count == 1)
                        {
                            if (previousAssessment[0].CourseId == 0 && previousAssessment[0].Id == 0)
                            {
                                LstAssessments.ItemsSource = new ObservableCollection<Assessment>();
                            }
                        }
                    }
                    if (assessment == null && list.Count == 1)
                    {
                        LstAssessments.ItemsSource = tempListView.ItemsSource;
                    }
                    else
                    {
                        LstAssessments.ItemsSource = new ObservableCollection<Assessment>();
                    }
                }
            }
        }

        public async void Delete_Clicked(object sender, EventArgs e)
        {
            var course = _course[0];
            var courseCode = course.CourseCode;

            //Throw a yes / no prompt to confirm delete
            var answer = await DisplayAlert($"Are you sure you wish to delete course - {courseCode}?", $"Delete Course - {courseCode}?", "Yes", "No");

            if (answer) // If the delete is confirmed
            {
                var courseId = course.Id;
                var allNotifications = await App.Database.GetNotificationsAsync();
                var notifications = allNotifications.Where(r => r.CourseId == courseId).ToList();

                foreach (var notification in notifications)
                {
                    CrossLocalNotifications.Current.Cancel(notification.Id);
                    await App.Database.DeleteNotificationAsync(notification);
                }

                await App.Database.DeleteCourseAsync(course);

                var allAssessments = await App.Database.GetAssessmentsAsync();
                var assessments = allAssessments.Where(r => r.CourseId == courseId).ToList();

                foreach (var assessment in assessments)
                {
                    await App.Database.DeleteAssessmentAsync(assessment);
                }

                Return();
            }
        }

        private async Task AddAssessment()
        {
            var assessments = (ObservableCollection<Assessment>)LstAssessments.ItemsSource;

            if (assessments.Count > 1)
            {
                await DisplayAlert("Warning!", "You may only have 2 assessments registered for a given course.", "Ok");
                return;
            }
            IEnumerable<Assessment> obsCollection = (IEnumerable<Assessment>)LstAssessments.ItemsSource;
            _preDelete = new List<Assessment>(obsCollection);
            LstAssessments.IsVisible = true;
            EmptyMessage.IsVisible = false;

            var assessment = new Assessment();
            assessments.Add(assessment);
            _preDelete.Add(assessment);

            LstAssessments.ItemsSource = assessments;
        }
    }
}