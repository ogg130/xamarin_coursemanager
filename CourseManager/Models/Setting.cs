using SQLite;
using System.ComponentModel;

namespace CourseManager.Model
{
    public class Setting
    {
        private int _id { get; set; }
        private string _name { get; set; }

        private string _value { get; set; }

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

        public string Value
        {
            get => _value;
            set
            {
                _value = value;
                RaisePropertyChanged(nameof(Value));
            }
        }

        public void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}