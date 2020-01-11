using System.ComponentModel;
using CourseManager.Model;

namespace CourseManager.ViewModel
{
    public class TermViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public Term Term { get; set; }

        public TermViewModel()
        {
        }

        public void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}