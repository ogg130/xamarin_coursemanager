using CourseManager.Model;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace CourseManager.Selector
{
    public class TermGridSelector : DataTemplateSelector
    {
        public DataTemplate EvenTemplate { get; set; }
        public DataTemplate UnevenTemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            return ((ObservableCollection<Term>)((ListView)container).ItemsSource).IndexOf(item as Term) % 2 == 0 ? EvenTemplate : UnevenTemplate;
        }
    }
}