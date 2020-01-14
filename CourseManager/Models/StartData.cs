using System;
using System.Collections.ObjectModel;

namespace CourseManager.Model
{
    public class StartData
    {
        public ObservableCollection<Term> Terms { get; set; }

        public ObservableCollection<Course> Courses { get; set; }

        public ObservableCollection<Setting> Settings { get; set; }

        public ObservableCollection<Assessment> Assessments { get; set; }

        public StartData()
        {
            LoadData();
        }

        private void LoadData()
        {
            if (Settings == null)
            {
                Settings = new ObservableCollection<Setting>
                {
                    new Setting
                     {
                        Id = 0,
                        Name = "firstload",
                        Value = "true"
                    }
                };
            }

            if (Terms == null)
            {
                Terms = new ObservableCollection<Term>
                {
                    new Term
                    {
                        Id = 0,
                        Name = "Term 1",
                        Start = new DateTime(2020, 1, 1),
                        End = new DateTime(2020, 6, 30)
                    }
                };

                if (Courses == null)
                {
                    Courses = new ObservableCollection<Course>
                    {
                        new Course
                        {
                            Id = 0,
                            TermId = 1,
                            Name = "Intermediate Algebra",
                            Start = DateTime.Now.Date,
                            End = new DateTime(2020, 5, 30),
                            DueDate = new DateTime(2020, 6, 30),
                            Cu = 3,
                            CourseCode = "C463",
                            Status = "Plan to Take",
                            InstructorName = "Robert T Ogden III",
                            InstructorEmail = "rogden1@wgu.edu",
                            InstructorPhone = "(602)550-7980",
                            Notes = "These are notes... lots of notes... I like to write notes beacause notes are cool. Here are some more notes and a few more. These are very descriptive notes, I hope I get a lot out of them."
                        }
                    };

                    if (Assessments == null)
                    {
                        Assessments = new ObservableCollection<Assessment>
                        {
                            new Assessment
                            {
                                Id = 0,
                                CourseId = 1,
                                Name = "Assessment 1",
                                Start = DateTime.Now.Date,
                                End = new DateTime(2020, 4, 15),
                                Type = "Performance",
                            },
                            new Assessment
                            {
                                Id = 0,
                                CourseId = 1,
                                Name = "Assessment 2",
                                Start = new DateTime(2020, 5, 1),
                                End = DateTime.Now.Date,
                                Type = "Objective",
                            },
                        };
                    }
                }
            }
        }
    }
}