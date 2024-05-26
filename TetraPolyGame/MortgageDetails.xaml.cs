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
    /// Interaction logic for MortgageDetails.xaml
    /// </summary>
    public partial class MortgageDetails : Window
    {
        private Player player;


        public MortgageDetails()
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
                        int price = property.GetMortgagePrice();

                        price += (property.GetHouseCount() * 25);
                        int[] houseValues = property.GetHouseRents();
                        Name.Text = property.GetName();
                        Rent.Text = property.GetRent().ToString();
                        Price.Text = price.ToString();
                        Colour.Text = property.GetColour();
                        Owned1.Content = "1 House";
                        Owned2.Content = "2 House";
                        Owned3.Content = "3 House";
                        Owned4.Content = "4 House";
                        Owned5.Visibility = Visibility.Visible;
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
                        Price.Text = transport.GetMortgagePrice().ToString();
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
                        Owned5.Visibility = Visibility.Collapsed;
                        House5.Visibility = Visibility.Collapsed;
                    }
                }
                else if (card is Utility)
                {
                    Utility utility = (Utility)card;
                    if (utility.GetName() == PlayerCards.SelectedValue.ToString())
                    {
                        Name.Text = utility.GetName();
                        Rent.Text = "Not applicable";
                        Price.Text = utility.GetMortgagePrice().ToString();
                        Colour.Visibility = Visibility.Collapsed;
                        ColourLbl.Visibility = Visibility.Collapsed;
                        House1.Text = "Dice roll (Times) 4";
                        House2.Text = "Dice roll (Times) 10";

                        Owned1.Content = "1 Owned";
                        Owned2.Content = "2 Owned";
                        Owned3.Visibility = Visibility.Collapsed;
                        Owned4.Visibility = Visibility.Collapsed;
                        Owned5.Visibility = Visibility.Collapsed;
                        House5.Visibility = Visibility.Collapsed;
                    }
                }

            }
        }


        private void PlayerCards_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MortgageBtn.IsEnabled = true;
            SetupFields();
        }

        public Card GetCardSelected()
        {
            foreach (Card card in player.CardsOwned)
            {
                if (PlayerCards.SelectedItem != null && card.ToString() == PlayerCards.SelectedItem.ToString())
                {
                    return card;
                }
            }
            return null;
        }

        private void MortgageButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
