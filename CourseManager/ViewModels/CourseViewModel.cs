using System.Collections.ObjectModel;
using System.ComponentModel;
using CourseManager.Model;

namespace CourseManager.ViewModel
{
    public class CourseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<Course> Courses { get; set; }

        public CourseViewModel()
        {
        }

        public void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}