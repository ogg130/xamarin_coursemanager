using SQLite;
using System.Collections.Generic;
using System.Threading.Tasks;
using CourseManager.Model;

namespace CourseManager.Data
{
    public class Database
    {
        private readonly SQLiteAsyncConnection _database;

        public Database(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
        }

        public async Task DropTables()
        {
            _database.DropTableAsync<Course>().Wait();
            _database.DropTableAsync<Assessment>().Wait();
            _database.DropTableAsync<LocalNotification>().Wait();
            _database.DropTableAsync<Term>().Wait();
            _database.DropTableAsync<Setting>().Wait();
        }

        public async Task BuildTables()
        {
            _database.CreateTableAsync<Course>().Wait();
            _database.CreateTableAsync<Assessment>().Wait();
            _database.CreateTableAsync<LocalNotification>().Wait();
            _database.CreateTableAsync<Term>().Wait();
            _database.CreateTableAsync<Setting>().Wait();
        }

        public async Task<string> DisplayContents()
        {
            var retVal = "";
            var tempNotes = await this.GetNotificationsAsync();
            foreach (var note in tempNotes)
            {
                retVal = retVal + $"ID: {note.Id} - TITLE: {note.Title} - DATE: {note.Date} - COURSEID: {note.CourseId} - ASSID: {note.AssessmentId}\n";
            }

            retVal = retVal + "\n\nASSESSMENTS-----";
            var tempAssessments = await this.GetAssessmentsAsync();
            foreach (var ass in tempAssessments)
            {
                retVal = retVal + $"ID: {ass.Id} - NAME: {ass.Name} - TYPE: {ass.Type} - START: {ass.Start} - END: {ass.End} - COURSEID: {ass.CourseId}";
            }

            retVal = retVal + "\n\nCOURSES-----";
            var tempCourses = await this.GetCoursesAsync();
            foreach (var crs in tempCourses)
            {
                retVal = retVal + $"ID: {crs.Id} - NAME: {crs.Name} - START: {crs.Start} - END: {crs.End}";
                //- COURSECODE: {crs.CourseCode}" - CU: {crs.Cu} - TERMID: {crs.TermId} - STATUS: {crs.Status} - INAME: {crs.InstructorName} - IPHONE: {crs.InstructorPhone} - END: {crs.InstructorPhone}");
            }

            retVal = retVal + "\n\nTERMS-----";
            var tempTerms = await this.GetTermsAsync();
            foreach (var term in tempTerms)
            {
                retVal = retVal + $"ID: {term.Id} - NAME: {term.Name} - START: {term.Start} - END {term.End}";
            }
            retVal = retVal + "\n";
            return retVal;
        }

        public Task<Course> GetCourseAsync(int id)
        {
            return _database.Table<Course>()
                            .Where(i => i.Id == id)
                            .FirstOrDefaultAsync();
        }

        public Task<Assessment> GetAssessmentAsync(int id)
        {
            return _database.Table<Assessment>()
                            .Where(i => i.Id == id)
                            .FirstOrDefaultAsync();
        }

        public Task<LocalNotification> GetNotificationAsync(int id)
        {
            return _database.Table<LocalNotification>()
                            .Where(i => i.Id == id)
                            .FirstOrDefaultAsync();
        }

        public Task<Term> GetTermAsync(int id)
        {
            return _database.Table<Term>()
                            .Where(i => i.Id == id)
                            .FirstOrDefaultAsync();
        }

        public Task<List<Setting>> GetSettingsAsync()
        {
            return _database.Table<Setting>().ToListAsync();
        }

        public Task<List<Term>> GetTermsAsync()
        {
            return _database.Table<Term>().ToListAsync();
        }

        public Task<List<Assessment>> GetAssessmentsAsync()
        {
            return _database.Table<Assessment>().ToListAsync();
        }

        public Task<List<Course>> GetCoursesAsync()
        {
            return _database.Table<Course>().ToListAsync();
        }

        public Task<List<Course>> GetCoursesAsync(int termId)
        {
            return _database.Table<Course>().Where(r => r.TermId == termId).ToListAsync();
        }

        public Task<List<LocalNotification>> GetNotificationsAsync()
        {
            return _database.Table<LocalNotification>().ToListAsync();
        }

        public Task<int> SaveCourseAsync(Course course)
        {
            if (course.Id != 0)
            {
                return _database.UpdateAsync(course);
            }
            else
            {
                return _database.InsertAsync(course);
            }
        }

        public Task<int> SaveSettingAsync(Setting setting)
        {
            if (setting.Id != 0)
            {
                return _database.UpdateAsync(setting);
            }
            else
            {
                return _database.InsertAsync(setting);
            }
        }

        public Task<int> SaveAssessmentAsync(Assessment assessment)
        {
            if (assessment.Id != 0)
            {
                return _database.UpdateAsync(assessment);
            }
            else
            {
                return _database.InsertAsync(assessment);
            }
        }

        public Task<int> SaveTermAsync(Term term)
        {
            if (term.Id != 0)
            {
                return _database.UpdateAsync(term);
            }
            else
            {
                return _database.InsertAsync(term);
            }
        }

        public Task<int> SaveNotificationAsync(LocalNotification notification)
        {
            if (notification.Id != 0)
            {
                return _database.UpdateAsync(notification);
            }
            else
            {
                return _database.InsertAsync(notification);
            }
        }

        public Task<int> DeleteNotificationAsync(LocalNotification notification)
        {
            return _database.DeleteAsync(notification);
        }

        public Task<int> DeleteTermAsync(Term term)
        {
            return _database.DeleteAsync(term);
        }

        public Task<int> DeleteAssessmentAsync(Assessment assessment)
        {
            return _database.DeleteAsync(assessment);
        }

        public Task<int> DeleteCourseAsync(Course course)
        {
            return _database.DeleteAsync(course);
        }
    }
}