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
        private Stack<ChanceCommunity> ChanceCommunities = new();
        protected List<Player> Players = [];
        private List<Ellipse> players = [];
        private Random rng = new Random();
        private int truncount = 0;
        public MainWindow()
        {
            InitializeComponent();
            //If you want to add player via the eclipse icons

            //For Testing purposes
            players.Add(TestPlayer);
            players.Add(TestPlayer2);
            players.Add(TestPlayer3);
            players.Add(TestPlayer4);

            Players.Add(new Player("Kaiden", 1000));
            Players.Add(new Player("David", 1000));
            Players.Add(new Player("Kyle", 1000));
            Players.Add(new Player("Daniel", 1000));


            Cards = database.GetProperties();
            List<ChanceCommunity> ComChaCards = database.GetCommunityChance();
            Shuffle.Shuffle.ShuffleList(ComChaCards);
            var chanceCommunities = new Stack<ChanceCommunity>(ComChaCards);
            ChanceCommunities = chanceCommunities;
            //MessageBox.Show(Canvas.GetLeft(pos0).ToString() + Canvas.GetTop(pos0).ToString());

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
                MovePlayer(players[truncount], Players[truncount].GetPosition());
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
            foreach(Card card in Cards)
            {
                if (pos == card.GetPosition())
                {
                    int cut = 0;
                    foreach (Player player in Players)
                    {
                        bool onby = player.CheckSet(card);
                        if (onby == true)
                        {
                            if (player != Players[turn])
                            {
                                int r = card.GetRent();
                                player.addMoney(r);
                                Players[turn].LoseMoney(r);
                            }
                            if ((player == Players[turn]) && (card is Property) && (player is not algorithm))
                            {
                                Property tempProp = card as Property;
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
                                    player.LoseMoney(cost);
                                    Property pro = card as Property;
                                    player.AddHouse(pro);
                                }
                            }
                            else if ((player == Players[turn]) && (card is Property))
                            {
                                Property tempProp = card as Property;
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
                                player.LoseMoney(cost);
                                Property pro = card as Property;
                                player.AddHouse(pro);
                            }
                        }
                        cut = cut + 1;
                    }

                    if (null == card.IsOwned)
                    {
                        if ((card is Property) || (card is Transport) || (card is Utility) && (Players[turn] is not algorithm))
                        {
                            MessageBoxResult result = MessageBox.Show("Do you want to buy?", "Buying", MessageBoxButton.YesNo);
                            if (result == MessageBoxResult.Yes)
                            {
                                Players[turn].buy(true, card);
                            }
                            else
                            {
                                Players[turn].buy(false, card);
                            }
                        }
                        else if ((card is Property) || (card is Transport) || (card is Utility))
                        {
                            Players[turn].buy(true, card);
                        }
                        if ((Players[turn].GetPosition() == 2) || (Players[turn].GetPosition() == 33) || (Players[turn].GetPosition() == 28))
                        {
                            getComCha(Players[turn]);
                        }
                        if ((Players[turn].GetPosition() == 7) || (Players[turn].GetPosition() == 22) || (Players[turn].GetPosition() == 36))
                        {
                            getComCha(Players[turn]);
                        }
                        if (Players[turn].GetPosition() == 30)
                        {
                            Players[turn].SetInJaile(true);
                            Players[turn].setPosition(-1);
                        }
                    }
                    t = false;
                }
            }
        }
        public void getComCha(Player player)
        {
            ChanceCommunity temp = ChanceCommunities.Pop();
            temp.SetEffect();
            temp.Execute(player);
        }
        
        /// <summary>
        /// Moves the visual elements on the board
        /// </summary>
        /// <param name="e">The player eclipse icons (Or anything that can be used to feature a player)</param>
        /// <param name="Position"></param>
        public void MovePlayer(UIElement e, int Position)
        {
            foreach (UIElement element in gameBoardGrid.Children)
            {
                if (element is Rectangle rectangle)
                {
                    if (rectangle.Name == ("pos") + Position.ToString())
                    {
                        Grid.SetColumn(e, Grid.GetColumn(element));
                        Grid.SetRow(e, Grid.GetRow(element));
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
                string st = "your number of money is"+Players[truncount].getMoney;
                displaymoney.Content = st;
                while (card != null)
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

        private void unmoragagepickacard_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void moragagepickacard_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}