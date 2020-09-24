using Prism.Mvvm;

namespace DoctorApp11.ViewModels
{
    public class MainViewModel : BindableBase
    {
        private string _title = "Prism Application";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public MainViewModel()
        {

        }
    }
}
