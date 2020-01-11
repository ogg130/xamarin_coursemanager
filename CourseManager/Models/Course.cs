using SQLite;
using System;
using System.ComponentModel;

namespace CourseManager.Model
{
    public class Course
    {
        private int _id;
        private string _name;
        private DateTime _start;
        private DateTime _end;

        private int _termId;

        private DateTime _dueDate;
        private int? _cu;
        private string _courseCode;
        private string _status;

        private string _instructorName;
        private string _instructorEmail;
        private string _instructorPhone;

        private string _notes;

        public Course()
        {
        }

        public Course(int id, DateTime dueDate)
        {
            _id = id;
            _dueDate = dueDate;
        }

        public int TermId
        {
            get => _termId;
            set
            {
                _termId = value;
                RaisePropertyChanged(nameof(TermId));
            }
        }

        public DateTime DueDate
        {
            get => _dueDate;
            set
            {
                _dueDate = value;
                RaisePropertyChanged(nameof(DueDate));
            }
        }

        public int? Cu
        {
            get => _cu;
            set
            {
                _cu = value;
                RaisePropertyChanged(nameof(Cu));
            }
        }

        public string CourseCode
        {
            get => _courseCode;
            set
            {
                _courseCode = value;
                RaisePropertyChanged(nameof(CourseCode));
            }
        }

        public string Status
        {
            get => _status;
            set
            {
                _status = value;
                RaisePropertyChanged(nameof(Status));
            }
        }

        public string InstructorName
        {
            get => _instructorName;
            set
            {
                _instructorName = value;
                RaisePropertyChanged(nameof(InstructorName));
            }
        }

        public string InstructorEmail
        {
            get => _instructorEmail;
            set
            {
                _instructorEmail = value;
                RaisePropertyChanged(nameof(InstructorEmail));
            }
        }

        public string InstructorPhone
        {
            get => _instructorPhone;
            set
            {
                _instructorPhone = value;
                RaisePropertyChanged(nameof(InstructorPhone));
            }
        }

        public string Notes
        {
            get => _notes;
            set
            {
                _notes = value;
                RaisePropertyChanged(nameof(Notes));
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

        public void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}