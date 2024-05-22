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
        public MainViewModel ViewModel { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            ViewModel = (MainViewModel)DataContext;
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
        public void AddPlayer(Player p)
        {
            Players.Add(p);
        }

        public void SetImageRes()
        {

        }
        // next turn 
        /// <summary>
        /// Manages the turn order of players in a game.
        /// </summary>
        /// <remarks>
        /// This method iterates through the players in a rounds, allowing each player to make a move.
        /// After each player's turn, it checks if there is only one player left in the game.
        /// </remarks>
        public void turnorder()
        {
            bool t = true;
            while (t == true)
            {
                Players[truncount].MovePlayer();
                MovePlayer(players[truncount], Players[truncount].GetPosition());
                checkposition(truncount);
                t = Onlyoneleft();
                if (truncount != players.Count - 1)
                {
                    truncount = truncount + 1;
                }
                else
                {
                    truncount = 0;
                }

            }
        }
        // Onlyone left
        /// <summary>Determines if there is only one player left alive.</summary>
        /// <returns>True if there is only one player alive, false otherwise.</returns>
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

        /// <summary>
        /// Checks the position of a player and performs corresponding actions based on the game rules.
        /// </summary>
        /// <param name="turn">The turn of the player to check.</param>
        /// <remarks>
        /// This method checks if the player lands on a card's position, and then executes actions based on the card type and ownership.
        public void checkposition(int turn)
        {
            bool t = true;
            int count = 0;
            int pos = Players[turn].GetPosition();
            foreach (Card card in Cards)
            {
                if (pos == card.GetPosition())
                {

                    if (card.IsOwned() != null)
                    {
                        if (card.IsOwned() != Players[turn])
                        {
                            int r = card.GetRent();
                            Players[turn].LoseMoney(r);
                        }
                        else if ((card.IsOwned() == Players[turn]) && (card is Property) && (Players[turn] is not algorithm))
                        {
                            Property tempProp = (Property)card;
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
                                Players[turn].LoseMoney(cost);
                                Players[turn].AddHouse(tempProp);
                            }
                        }
                        else if ((card.IsOwned() == Players[turn]) && (card is Property) && Players[turn] is algorithm)
                        {
                            Property tempProp = (Property)card;
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
                            Players[turn].LoseMoney(cost);
                            Property pro = (Property)card;
                            Players[turn].AddHouse(pro);
                        }
                    }

                    else if (card.IsOwned() == null)
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
                        
                    }
                    
                    t = false;

                }
                else if ((Players[turn].GetPosition() == 2) || (Players[turn].GetPosition() == 33) || (Players[turn].GetPosition() == 28))
                {
                    getComCha(Players[turn]);
                }
                else if ((Players[turn].GetPosition() == 7) || (Players[turn].GetPosition() == 22) || (Players[turn].GetPosition() == 36))
                {
                    getComCha(Players[turn]);
                }
                else if (Players[turn].GetPosition() == 30)
                {
                    Players[turn].SetInJaile(true);
                    Players[turn].setPosition(-1);
                }
            }
        }
        /// <summary>
        /// Retrieves a Chance or Community Chest card, sets its effect, and executes it on the player.
        /// </summary>
        /// <param name="player">The player to execute the card's effect on.</param>
        public void getComCha(Player p)
        {
            ChanceCommunity temp = ChanceCommunities.Pop();
            
            switch (temp.GetDesc())
            {
                case "Advance To Boardwalk":
                    p.MoveToPosition(39);
                    break;

                case "Advance To Go":
                    p.addMoney(200); p.SetPos(0);
                    break;

                case "Go back 3 spaces":
                    p.SetPos(p.GetPosition() - 3);
                    break;

                case "Go to Jail. Go directly to Jail, do not pass Go, do not collect $200.":
                    p.SetPos(-1);
                    break;

                case "Fined for a LEZ (Light Emmision Zone) Charge":
                    p.LoseMoney(60);

                    break;

                case "Your building loan matures. Collect $150":
                    p.addMoney(150);

                    break;

                case "Get Out of Jail Free":
                    p.SetPos(10); p.SetInJaile(false);

                    break;

                case "Advocate for affordable housing! Pay 100 Coins but gain 200 back!":
                    p.addMoney(100);

                    break;

                case "Your investment in a women-led business has turned out amazing for you! You've profited 200!":
                    p.addMoney(200);

                    break;

                case "You stumble upon a beach littered with plastic waste. Clean it up and move forward 2 spaces.":
                    p.SetPos(p.GetPosition() + 2);

                    break;
            }
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
                        Grid.SetColumn(e, Grid.GetColumn(rectangle));
                        Grid.SetRow(e, Grid.GetRow(rectangle));
                        break;
                    }
                }
            }

        }
        /// <summary>
        /// Handles the click event when the "rolldice" button is clicked.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The event arguments.</param>
        /// <remarks>
        /// This method checks the current player's money status. If the player has money,
        /// it proceeds with the turn order. If the player has no money, it prompts the player
        /// to mortgage properties. Finally, it updates the user interface.
        /// </remarks>
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
        /// <summary>
        /// Clears and updates the items in the unmortgaged and mortgaged card pickers based on the player's cards.
        /// Also displays the player's current money.
        /// </summary>
        /// <remarks>
        /// This method populates the unmortgaged and mortgaged card pickers with the player's cards.
        /// It also displays the player's current money on the UI.
        /// </remarks>
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
                //string st = "your number of money is: " + Players[truncount].getMoney();
                //displaymoney.Content = st;
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
                    string s = b5 + "," + b6 + "," + ii;
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

        /// <summary>
        /// Handles the click event when the "unmortgage" button is clicked.
        /// Retrieves the list of cards owned by the current player, selects a card based on user input,
        /// unmortgages the selected card, and updates the UI accordingly.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The event arguments.</param>
        private void unmorgage_Click(object sender, RoutedEventArgs e)
        {
            List<Card> card = Players[truncount].GetCards();
            string st = (string)unmoragagepickacard.SelectedValue;
            int check = int.Parse(st.Split(",")[2]);
            Players[truncount].OnMortgageCard(card[check]);
            changebox();
        }

        /// <summary>
        /// Handles the click event when the mortgage button is clicked.
        /// Mortgages a selected card from the player's cards list.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The event arguments.</param>
        /// <remarks>
        /// This method retrieves the selected card from the player's cards list,
        /// parses the selected card information, mortgages the card, and updates the UI.
        /// </remarks>
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