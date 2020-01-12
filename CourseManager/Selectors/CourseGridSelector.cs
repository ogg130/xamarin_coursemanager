using CourseManager.Model;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace CourseManager.Selector
{
    public class CourseGridSelector : DataTemplateSelector
    {
        public DataTemplate EvenTemplate { get; set; }
        public DataTemplate UnevenTemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            return ((ObservableCollection<Course>)((ListView)container).ItemsSource).IndexOf(item as Course) % 2 == 0 ? EvenTemplate : UnevenTemplate;
        }
    }
}