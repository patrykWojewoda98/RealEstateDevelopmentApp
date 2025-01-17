using GalaSoft.MvvmLight.Messaging;
using MVVMFirma.Helper;
using realEstateDevelopment.Core;
using realEstateDevelopment.Helper;
using realEstateDevelopment.MVVM.Model.Entities;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace realEstateDevelopment.MVVM.ViewModel
{
    public abstract class LoadAllViewModel<T> : WorkspaceViewModel 
    {
        #region Fields
        protected readonly RealEstateEntities realEstateEntities;
        private ObservableCollection<T> _List;
        private int _selectedItem;
        public int SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
                OnPropertyChanged(() => SelectedItem);
            }
        }

        #region Commands
        private BaseCommand _LoadCommand;
        private BaseCommand _UpdateCommand;
        public RealyCommand ReloadCommand { get; set; }
        public RealyCommand ApplyFiltersCommand { get; set; }
        #endregion


        #endregion

        #region Properties
        
        public ICommand LoadCommand
        {
            get
            {
                if (_LoadCommand == null)
                    _LoadCommand = new BaseCommand(() => LoadAsync());
                return _LoadCommand;
            }
        }
        public ICommand UpdateCommand
        {
            get
            {
                if (_UpdateCommand == null)
                    _UpdateCommand = new BaseCommand(() => Update(SelectedItem));
                return _UpdateCommand;
            }
        }



        public ObservableCollection<T> List
        {
            get
            {
                if (_List == null)
                    LoadAsync();
                return _List;
            }
            set
            {
                _List = value;
                OnPropertyChanged(() => List);
            }
        }
            #endregion

            #region Constructor
            public LoadAllViewModel()
            {
                realEstateEntities = new RealEstateEntities();

            ReloadCommand = new RealyCommand(async o =>
            {
                ReloadAsync();
            });

            ApplyFiltersCommand = new RealyCommand(async o =>
            {
                await ApplyFiltersAsync();
            });
            }
        #endregion

        #region Helpers
        public abstract Task LoadAsync();
        public async void ReloadAsync()
        {
                List.Clear();
                await LoadAsync();
        }

        public abstract Task ApplyFiltersAsync();

        public void Update(int id)
        {
            var updateMessage = new UpdateMessage(this.GetType().Name+ "Update", id);
            Console.WriteLine("Moja Widomośc: "+ updateMessage.Message +"  "+updateMessage.Data);
            Messenger.Default.Send(updateMessage);
        }

        public void Delate(int id)
        {
            var updateMessage = new UpdateMessage(this.GetType().Name + "Delate", id);
            Console.WriteLine("Moja Widomośc: " + updateMessage.Message + "  " + updateMessage.Data);
            Messenger.Default.Send(updateMessage);
        }
        #endregion
    }

}

