using System.ComponentModel;

namespace CourseManager.ViewModel
{
    public class CourseDetailViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public CourseDetailViewModel()
        {
        }

        public void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}