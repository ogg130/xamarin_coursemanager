using CourseManager.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Forms;

namespace CourseManager.Selector
{
    public class AssessmentGridSelector : DataTemplateSelector
    {
        public DataTemplate EvenTemplate { get; set; }
        public DataTemplate UnevenTemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            return ((ObservableCollection<Assessment>)((ListView)container).ItemsSource).IndexOf(item as Assessment) % 2 == 0 ? EvenTemplate : UnevenTemplate;
        }
    }
}