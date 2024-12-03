namespace realEstateDevelopment.MVVM.ViewModel.Modals
{
    public class ErrorModalViewModel
    {
        public string _errors { get; set; }
        public ErrorModalViewModel(string errors)
        {
            _errors = errors;
        }
    }
}