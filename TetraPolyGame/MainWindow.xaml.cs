using System.Collections;
using System.ComponentModel.Design;
using System.Printing;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace TetraPolyGame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static AllCards cards = AllCards.Instance;
        private MSSQLdataAccess database = new();
        private List<Card> Cards = new();
        protected List<Player> Players = new();
        private int truncount = 0;
        public MainWindow()
        {
            //If you want to add player via the eclipse icons
            List<UIElement> players = [];

            //players.Add(TestPlayer);

            InitializeComponent();
            Cards = database.GetProperties();
            MessageBox.Show(Canvas.GetLeft(pos0).ToString() + Canvas.GetTop(pos0).ToString());

        }
        //set the players
        public void setPlayers(Player p)
        {
            Players.Add(p);
        }
        // next turn 
        public void turnorder()
        {
            bool t = true;
            while (t == true)
            {
                if (Players[truncount] == null)
                {
                    truncount = 0;
                }
                Players[truncount].MovePlayer();
                MovePlayer(UIElement e, Players[truncount].GetPosition);
                checkposition(truncount);
                t = Onlyoneleft();
                truncount = truncount + 1;
            }
        }
        // Onlyone left
        public bool Onlyoneleft()
        {
            bool b = true;
            int c = 0;
            int count = 0;
            while (Players[c] == null)
            {
                if (Players[c].GetAilve() == true)
                {
                    count = count + 1;
                }
            }
            if (count < 2)
            {
                b = false;
            }
            else if (count > 1)
            {
                b = true;
            }
            return b;
        }
        public void checkposition(int turn)
        {
            bool t = true;
            int count = 0;
            int pos = Players[turn].GetPosition();
            while (t == true)
            {
                if (pos == Cards[count].GetPosition())
                {
                    int cut = 0;
                    while (Players[cut] != null)
                    {
                        bool onby = Players[cut].CheckSet(Cards[count]);
                        if (onby == true)
                        {
                            if (Players[cut] != Players[turn])
                            {
                                int r = Cards[count].GetRent();
                                Players[cut].addMoney(r);
                                Players[turn].LoseMoney(r);
                            }
                            if ((Players[cut] == Players[turn]) && (Cards[count] is Property) && (Players[cut] is not algorithm))
                            {
                                Property tempProp = Cards[count] as Property;
                                MessageBoxResult result = MessageBox.Show("Do you want to buy a house?", "House Buying", MessageBoxButton.YesNo);
                                if (result == MessageBoxResult.Yes)
                                {
                                    int cost = 0;
                                    switch (tempProp.GetColour())
                                    {
                                        case "BROWN": { cost = 50; break; }
                                        case "LBLUE": { cost = 50; break; }
                                        case "PINK": { cost = 50; break; }

                                        case "ORANGE": { cost = 100; break; }
                                        case "RED": { cost = 100; break; }

                                        case "YELLOW": { cost = 150; break; }
                                        case "GREEN": { cost = 150; break; }

                                        case "DBLUE": { cost = 200; break; }

                                    }
                                    Players[cut].LoseMoney(cost);
                                    Property pro = Cards[count] as Property;
                                    Players[cut].AddHouse(pro);
                                }
                            }
                            else if ((Players[cut] == Players[turn]) && (Cards[count] is Property))
                            {
                                Property tempProp = Cards[count] as Property;
                                int cost = 0;
                                switch (tempProp.GetColour())
                                {
                                    case "BROWN": { cost = 50; break; }
                                    case "LBLUE": { cost = 50; break; }
                                    case "PINK": { cost = 50; break; }

                                    case "ORANGE": { cost = 100; break; }
                                    case "RED": { cost = 100; break; }

                                    case "YELLOW": { cost = 150; break; }
                                    case "GREEN": { cost = 150; break; }

                                    case "DBLUE": { cost = 200; break; }

                                }
                                Players[cut].LoseMoney(cost);
                                Property pro = Cards[count] as Property;
                                Players[cut].AddHouse(pro);
                            }
                        }
                        cut = cut + 1;
                    }

                    if (null == Cards[count].IsOwned)
                    {
                        if ((Cards[count] is Property) || (Cards[count] is Transport) || (Cards[count] is Utility) && (Players[cut] is not algorithm))
                        {
                            MessageBoxResult result = MessageBox.Show("Do you want to buy a house?", "House Buying", MessageBoxButton.YesNo);
                            if (result == MessageBoxResult.Yes)
                            {
                                Players[turn].buy(true, Cards[count]);
                            }
                            else
                            {
                                Players[turn].buy(false, Cards[count]);
                            }
                        }
                        else if ((Cards[count] is Property) || (Cards[count] is Transport) || (Cards[count] is Utility))
                        {
                            Players[turn].buy(true, Cards[count]);
                        }
                        if ((Players[turn].GetPosition() == 2) || (Players[turn].GetPosition() == 33) || (Players[turn].GetPosition() == 28))
                        {
                            Commun.getcard();
                        }
                        if ((Players[turn].GetPosition() == 7) || (Players[turn].GetPosition() == 22) || (Players[turn].GetPosition() == 36))
                        {
                            Chance.getcard();
                        }
                        if (Players[turn].GetPosition() == 30)
                        {
                            Players[turn].SetInJaile(true);
                            Players[turn].setPosition(-1);
                        }
                    }
                    t = false;
                }
                count = count + 1;
            }
        }

        

        public void MovePlayer(UIElement e, int Position)
        {
            foreach (UIElement element in gameBoardGrid.Children)
            {
                if (element is Rectangle rectangle)
                {
                    if (rectangle.Name == ("pos") + Position.ToString())
                    {
                        Grid.SetColumn(e, Grid.GetColumn(rectangle));
                        Grid.SetRow(e, Grid.GetRow(rectangle));
                    }
                }
            }

        }
        private void rolldice_Click(object sender, RoutedEventArgs e)
        {
            int i = Players[truncount].getMoney();
            if (i > 0)
            {
                turnorder();
            }
            else
            {
                Players[truncount].asktomortgage();
            }
            changebox();
        }
        public void changebox()
        {
            try
            {
                unmoragagepickacard.Items.Clear();
                moragagepickacard.Items.Clear();
                List<Card> card = Players[truncount].GetCards();
                int ii = 0;
                string str;
                string b5;
                string b6;
                unmoragagepickacard.SelectedIndex = 0;
                moragagepickacard.SelectedIndex = 0;
                string st = "your nomber of money is"+Players[truncount].getMoney;
                displaymoney.Text = st;
                while (card[ii] != null)
                {
                    try
                    {
                        b5 = card.ToString().Split(", ")[0];
                        b6 = card.ToString().Split(", ")[3];
                    }
                    catch (Exception ae)
                    {
                        b5 = "there are no cards";
                        b6 = null;
                    }
                    bool b1 = card[ii].IsMortgaged();
                    string s = b5+","+b6+","+ii;
                    if (b1 == false)
                    {
                        unmoragagepickacard.Items.Add(s);
                    }
                    else
                    {
                        moragagepickacard.Items.Add(s);
                    }
                    ii = ii + 1;

                }
            }
            catch (Exception ee)
            {
            }
        }

        private void unmorgage_Click(object sender, RoutedEventArgs e)
        {
            List<Card> card = Players[truncount].GetCards();
            string st = (string)unmoragagepickacard.SelectedValue;
            int check=int.Parse(st.Split(",")[2]);
            Players[truncount].OnMortgageCard(card[check]);
            changebox();
        }

        private void morgage_Click(object sender, RoutedEventArgs e)
        {
            List<Card> card = Players[truncount].GetCards();
            string st = (string)unmoragagepickacard.SelectedValue;
            int check = int.Parse(st.Split(",")[2]);
            Players[truncount].MortgageCard(card[check]);
            changebox();
        }
    }
}