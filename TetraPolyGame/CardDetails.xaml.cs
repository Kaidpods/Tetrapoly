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
            foreach (Card card in player.CardsOwned)
            {
                if (card is Property)
                {
                    Property property = (Property)card;
                    if (property.GetName() == PlayerCards.SelectedValue.ToString())
                    {
                        int[] houseValues = property.GetHouseRents();
                        Name.Text = property.GetName();
                        Rent.Text = property.GetRent().ToString();
                        Price.Text = property.GetPrice().ToString();
                        Colour.Text = property.GetColour();
                        House1.Text = houseValues[1].ToString();
                        House2.Text = houseValues[2].ToString();
                        House3.Text = houseValues[3].ToString();
                        House4.Text = houseValues[4].ToString();
                        House5.Text = houseValues[5].ToString();
                    }

                }
                else if (card is Transport)
                {
                    Transport transport = (Transport)card;
                    if (transport.GetName() == PlayerCards.SelectedValue.ToString())
                    {
                        int rent = transport.GetRent();
                        Name.Text = transport.GetName();
                        Rent.Text = rent.ToString();
                        Price.Text = transport.GetPrice().ToString();
                        Colour.Visibility = Visibility.Collapsed;
                        ColourLbl.Visibility = Visibility.Collapsed;
                        House1.Text = (rent * 1).ToString();
                        House2.Text = (rent * 2).ToString();
                        House3.Text = (rent * 4).ToString();
                        House4.Text = (rent * 8).ToString();
                        Owned1.Content = "1 Owned";
                        Owned2.Content = "2 Owned";
                        Owned3.Content = "3 Owned";
                        Owned4.Content = "4 Owned";
                        House5.Visibility = Visibility.Collapsed;
                    }
                }

            }
        }


        private void PlayerCards_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SetupFields();
        }
    }
}
