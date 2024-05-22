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
    /// Interaction logic for CardDetails.xaml
    /// </summary>
    public partial class CardDetails : Window
    {
        private Player player;
        public CardDetails()
        {
            InitializeComponent();
        }

        public void GetPlayer(Player GotPlayer)
        {
            player = GotPlayer;
        }

        public void SetupFields()
        {
            foreach (Property property in player.CardsOwned)
            {
                if (property.GetName() == PlayerCards.SelectedValue.ToString())
                {
                    int[] houseValues = property.GetHouseRents();
                    Name.Text = property.GetName();
                    Rent.Text = property.GetRent().ToString();
                    Price.Text = property.GetPrice().ToString();
                    Colour.Text = property.GetColour();
                    House1.Text = houseValues[0].ToString();
                    House2.Text = houseValues[1].ToString();
                    House3.Text = houseValues[2].ToString();
                    House4.Text = houseValues[3].ToString();
                    House5.Text = houseValues[4].ToString();
                }
            }
        }

        private void PlayerCards_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SetupFields();
        }
    }
}
