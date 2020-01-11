using SQLite;
using System;
using System.ComponentModel;

namespace CourseManager.Model
{
    public class LocalNotification
    {
        private int _id;
        private string _title;
        private string _message;
        private DateTime _date;
        private int _courseId;
        private int _assessmentId;

        public LocalNotification()
        {
        }

        public LocalNotification(int id, string title, string message, DateTime date, int courseId, int assessmentId)
        {
            _id = id;
            _title = title;
            _message = message;
            _date = date;
            _courseId = courseId;
            _assessmentId = assessmentId;
        }

        public int AssessmentId
        {
            get => _assessmentId;
            set
            {
                _assessmentId = value;
                RaisePropertyChanged(nameof(AssessmentId));
            }
        }

        public int CourseId
        {
            get => _courseId;
            set
            {
                _courseId = value;
                RaisePropertyChanged(nameof(CourseId));
            }
        }

        public DateTime Date
        {
            get => _date;
            set
            {
                _date = value;
                RaisePropertyChanged(nameof(Date));
            }
        }

        [PrimaryKey, AutoIncrement]
        public int Id
        {
            get => _id;
            set
            {
                _id = value;
                RaisePropertyChanged(nameof(Id));
            }
        }

        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                RaisePropertyChanged(nameof(Title));
            }
        }

        public string Message
        {
            get => _message;
            set
            {
                _message = value;
                RaisePropertyChanged(nameof(Message));
            }
        }

        public void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}