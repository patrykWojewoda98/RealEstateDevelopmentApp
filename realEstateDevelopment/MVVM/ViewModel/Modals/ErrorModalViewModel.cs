using realEstateDevelopment.Core;
using System;

namespace realEstateDevelopment.MVVM.ViewModel.Modals
{
    public class ErrorModalViewModel : WorkspaceViewModel
    {
        public string _errors { get; set; }
        
        public ErrorModalViewModel(string errors)
        {
            _errors = errors;
        }
    }
}