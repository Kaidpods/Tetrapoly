using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace TetraPolyGame
{
    /// <summary>
    /// Interaction logic for PlayerSelect.xaml
    /// </summary>
    public partial class PlayerSelect : Window
    {
        public PlayerSelect()
        {
            InitializeComponent();
        }

        private void P1_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Get the DataContext (ViewModel)

        }

        private void P2_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Get the DataContext (ViewModel)

        }
        private void P3_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Get the DataContext (ViewModel)

        }
        private void P4_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Get the DataContext (ViewModel)

        }

        private void PlayerAmount_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = PlayerAmount.SelectedItem;
            TextBox textBox = (TextBox)selectedItem;

            MessageBox.Show("Starting the game with: " + textBox.Text.ToString() + " Players", "Game Start!", MessageBoxButton.OK, MessageBoxImage.Asterisk);
            this.Close();
        }
    }
}
