﻿using realEstateDevelopment.Themes;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Input;

namespace realEstateDevelopment.MVVM.View
{
    
    public partial class AddNewBuildingView : UserControl
    {
        public AddNewBuildingView()
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
