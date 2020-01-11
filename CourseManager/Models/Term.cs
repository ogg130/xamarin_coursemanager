using SQLite;
using System;
using System.ComponentModel;

namespace CourseManager.Model
{
    public class Term
    {
        private int _id;
        private string _name;
        private DateTime _start;
        private DateTime _end;

        public Term()
        {
        }

        public Term(int id)
        {
            _id = id;
            _start = DateTime.Now;
            _end = DateTime.Now;
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