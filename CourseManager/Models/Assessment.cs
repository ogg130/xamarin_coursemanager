using SQLite;
using System;
using System.ComponentModel;

namespace CourseManager.Model
{
    public class Assessment
    {
        private int _id;
        private string _name;
        private DateTime _start;
        private DateTime _end;
        private int _courseId;

        private string _type;

        public Assessment()
        {
            _start = DateTime.Now;
            _end = DateTime.Now;
        }

        public Assessment(int fakeId)
        {
            _id = fakeId;
            _start = DateTime.Now;
            _end = DateTime.Now;
        }

        public Assessment(int id, string name, DateTime start, DateTime end, string type, int courseId)
        {
            _id = id;
            _name = name;
            _start = start;
            _end = end;
            _type = type;
            _courseId = courseId;
        }

        public string Type
        {
            get => _type;
            set
            {
                _type = value;
                RaisePropertyChanged(nameof(Type));
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

        public int CourseId
        {
            get => _courseId;
            set
            {
                _courseId = value;
                RaisePropertyChanged(nameof(CourseId));
            }
        }

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                RaisePropertyChanged(nameof(Name));
            }
        }

        public DateTime Start
        {
            get => _start;
            set
            {
                _start = value;
                RaisePropertyChanged(nameof(Start));
            }
        }

        public DateTime End
        {
            get => _end;
            set
            {
                _end = value;
                RaisePropertyChanged(nameof(End));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}