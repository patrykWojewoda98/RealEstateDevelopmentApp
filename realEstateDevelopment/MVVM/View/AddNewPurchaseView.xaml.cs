﻿using System.Windows.Controls;

namespace realEstateDevelopment.MVVM.View
{
    public partial class AddNewPurchaseView : UserControl
    {
        public AddNewPurchaseView()
        {
            InitializeComponent();
        }
        private void ComboBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedComboItem = sender as ComboBox;
            string name = selectedComboItem.SelectedItem as string;

        }
    }
}