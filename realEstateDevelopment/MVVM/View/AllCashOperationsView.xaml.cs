using GalaSoft.MvvmLight.Messaging;
using realEstateDevelopment.Helper;
using realEstateDevelopment.MVVM.Model.EntitiesForView;
using realEstateDevelopment.MVVM.ViewModel;
using System;
using System.Windows.Controls;
using System.Windows.Input;

namespace realEstateDevelopment.MVVM.View
{
    
    public partial class AllCashOperationsView : UserControl
    {
        public AllCashOperationsView()
        {
            InitializeComponent();
        }
        private void DataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is DataGrid dataGrid && dataGrid.SelectedItem is AllCashOperationsEntityForView selectedOperation)
            {
                if (DataContext is AllCashOperationsViewModel viewModel)
                {
                    // Ustaw ID w ViewModelu na podstawie wybranego obiektu
                    viewModel.SelectedItem = selectedOperation.Id;

                    if (selectedOperation.Type == "Przychód")
                    {
                        var updateMessage = new UpdateMessage("RevenueUpdate", selectedOperation.Id);
                        Messenger.Default.Send(updateMessage);
                    }else if (selectedOperation.Type == "Koszt")
                    {
                        var updateMessage = new UpdateMessage("ExpenceUpdate", selectedOperation.Id);
                        Messenger.Default.Send(updateMessage);
                    }
                    else
                    {
                        var updateMessage = new UpdateMessage("UnknownUpdate", -1);
                        Messenger.Default.Send(updateMessage);
                    }
                }
            }
        }
    }
}
