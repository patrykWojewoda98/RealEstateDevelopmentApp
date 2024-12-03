using realEstateDevelopment.MVVM.ViewModel.Modals;
using System.Windows;
namespace realEstateDevelopment.MVVM.View.Modals
{
    public partial class ErrorModalView : Window
    {
        public ErrorModalView(string errors)
        {
            InitializeComponent();
            DataContext = new ErrorModalViewModel(errors);
            base.Title = "Niepoprawne dane!";
        }
    }
}