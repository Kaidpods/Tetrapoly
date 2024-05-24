using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Printing;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
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
        private ObservableCollection<Card> Cards = new();
        private Stack<ChanceCommunity> ChanceCommunities = new();
        protected List<Player> Players = [];
        private List<Ellipse> players = [];
        private Random rng = new Random();
        private List<ChanceCommunity> ComChaCards = [];
        private int truncount = 0;
        public MainViewModel ViewModel { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            EndTurnBtn.IsEnabled = false;


            ViewModel = (MainViewModel)DataContext;
            //For Testing purposes
            players.Add(TestPlayer);
            players.Add(TestPlayer2);
            players.Add(TestPlayer3);
            players.Add(TestPlayer4);


            SetupCards();

            //MessageBox.Show(Canvas.GetLeft(pos0).ToString() + Canvas.GetTop(pos0).ToString());
        }
        //set the players
        public void AddPlayer(Player p)
        {
            Players.Add(p);
        }

        private void SetupCards()
        {
            Cards = database.GetProperties();
            ObservableCollection<Card> tempTransport = database.GetTransport();
            tempTransport.ToList().ForEach(Cards.Add);
            ComChaCards = database.GetCommunityChance();
            Shuffle.Shuffle.ShuffleList(ComChaCards);
            var chanceCommunities = new Stack<ChanceCommunity>(ComChaCards);
            ChanceCommunities = chanceCommunities;
        }

        private void MoveClockwise(int endRow, int endColumn)
        {
            // Determine the next position (clockwise)
            int gridSize = 11;
            int newRow = 0, newColumn = 0;
            if (players[truncount] != null)
            {
                var currentRow = Grid.GetRow(players[truncount]);
                var currentColumn = Grid.GetColumn(players[truncount]);

                if (currentRow == 0 && currentColumn < gridSize - 1)
                {
                    newRow = currentRow;
                    newColumn = currentColumn + 1;
                }
                else if (currentColumn == gridSize - 1 && currentRow < gridSize - 1)
                {
                    newRow = currentRow + 1;
                    newColumn = currentColumn;
                }
                else if (currentRow == gridSize - 1 && currentColumn > 0)
                {
                    newRow = currentRow;
                    newColumn = currentColumn - 1;
                }
                else if (currentRow > 0 && currentColumn == 0)
                {
                    newRow = currentRow - 1;
                    newColumn = currentColumn;
                }
                // Stop the movement when reaching (10, 3)
                if (currentRow == endRow && currentColumn == endColumn)
                {
                    return;
                }

                // Create an animation to move the player to the next position
                var rowAnimation = new Int32Animation(currentRow, newRow, TimeSpan.FromMilliseconds(100));
                var columnAnimation = new Int32Animation(currentColumn, newColumn, TimeSpan.FromMilliseconds(100));

                rowAnimation.Completed += (sender, e) =>
                {
                    Grid.SetRow(players[truncount], newRow);
                    Grid.SetColumn(players[truncount], newColumn);
                    MoveClockwise(endRow, endColumn); // Repeat the clockwise movement
                };

                players[truncount].BeginAnimation(Grid.RowProperty, rowAnimation);
                players[truncount].BeginAnimation(Grid.ColumnProperty, columnAnimation);
            }
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
            if (!Onlyoneleft())
            {
                ViewModel.Players[truncount].MovePlayer(DiceRollCounter);
                MovePlayer(players[truncount], ViewModel.Players[truncount].GetPosition());
                checkposition(truncount);
            }
            else
            {

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
            foreach (var player in ViewModel.Players)
            {
                if (player.GetAilve() == true)
                {
                    count++;
                }
            }
            if (count == 1)
            {
                b = true;
            }
            else if (count > 1)
            {
                b = false;
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
            int pos = ViewModel.Players[turn].GetPosition();
            foreach (Card card in Cards)
            {
                if (pos == card.GetPosition())
                {

                    if (card.WhoOwns() != null)
                    {
                        if (card.WhoOwns() != ViewModel.Players[turn] && card.IsMortgaged() == false)
                        {
                            int r = card.GetRent();
                            ViewModel.Players[turn].Money -= r;
                            ViewModel.Players[turn].CheckMoney(r);
                        }
                        else if ((card.WhoOwns() == ViewModel.Players[turn]) && (card is Property) && (ViewModel.Players[turn] is not algorithm))
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
                                ViewModel.Players[turn].CheckMoney(cost);
                                ViewModel.Players[turn].AddHouse(tempProp);
                            }
                        }
                        else if ((card.WhoOwns() == ViewModel.Players[turn]) && (card is Property) && ViewModel.Players[turn] is algorithm)
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
                            ViewModel.Players[turn].CheckMoney(cost);
                            Property pro = (Property)card;
                            ViewModel.Players[turn].AddHouse(pro);
                        }
                    }

                    else if (card.WhoOwns() == null)
                    {
                        if ((card is Property) || (card is Transport) || (card is Utility) && (ViewModel.Players[turn] is not algorithm))
                        {
                            MessageBoxResult result = MessageBox.Show("Do you want to buy: " + card.GetName() + "\nThe price is: $" + card.GetPrice(), "Buying", MessageBoxButton.YesNo);
                            if (result == MessageBoxResult.Yes)
                            {
                                ViewModel.Players[turn].buy(true, card);
                            }
                            else
                            {
                                ViewModel.Players[turn].buy(false, card);
                            }
                        }
                        else if ((card is Property) || (card is Transport) || (card is Utility))
                        {
                            ViewModel.Players[turn].buy(true, card);
                        }

                    }

                    t = false;

                }
            }
            if ((ViewModel.Players[turn].GetPosition() == 2) || (ViewModel.Players[turn].GetPosition() == 33) || (ViewModel.Players[turn].GetPosition() == 28))
            {
                getComCha(ViewModel.Players[turn]);
            }
            else if ((ViewModel.Players[turn].GetPosition() == 7) || (ViewModel.Players[turn].GetPosition() == 22) || (ViewModel.Players[turn].GetPosition() == 36))
            {
                getComCha(ViewModel.Players[turn]);
            }
            if (ViewModel.Players[turn].GetPosition() == 30)
            {
                ViewModel.Players[turn].SetInJaile(true);
                ViewModel.Players[turn].setPosition(-1);
                MovePlayer(players[turn], -1);


            }
        }
        /// <summary>
        /// Retrieves a Chance or Community Chest card, sets its effect, and executes it on the player.
        /// </summary>
        /// <param name="player">The player to execute the card's effect on.</param>
        public void getComCha(Player p)
        {
            ChanceCommunity temp;
            try
            {
                temp = ChanceCommunities.Pop();

                MessageBox.Show(temp.GetDesc());
                switch (temp.GetDesc())
                {
                    case "Advance To Boardwalk":
                        p.MoveToPosition(39);
                        MovePlayer(players[truncount], 39);
                        break;

                    case "Advance To Go":
                        p.Money += (200); p.SetPos(0);
                        MovePlayer(players[truncount], 0);
                        break;

                    case "Go back 3 spaces":
                        p.SetPos(p.GetPosition() - 3);
                        MovePlayer(players[truncount], p.GetPosition());
                        break;

                    case "Go to Jail. Go directly to Jail, do not pass Go, do not collect $200.":
                        p.SetPos(-1);
                        p.SetInJaile(true);
                        MovePlayer(players[truncount], p.GetPosition());
                        break;

                    case "Fined for a LEZ (Light Emmision Zone) Charge":
                        p.CheckMoney(60);

                        break;

                    case "Your building loan matures. Collect $150":
                        p.Money += (150);

                        break;

                    case "Get Out of Jail Free":
                        p.SetPos(10); p.SetInJaile(false);
                        MovePlayer(players[truncount], 10);
                        break;

                    case "Advocate for affordable housing! Pay 100 Coins but gain 200 back!":
                        p.Money += (100);

                        break;

                    case "Your investment in a women-led business has turned out amazing for you! You've profited 200!":
                        p.Money += (200);

                        break;

                    case "You stumble upon a beach littered with plastic waste. Clean it up and move forward 2 spaces.":
                        p.SetPos(p.GetPosition() + 2);
                        MovePlayer(players[truncount], p.GetPosition());

                        break;
                }
            }
            catch (Exception e)
            {
                Shuffle.Shuffle.ShuffleList(ComChaCards);
                var chanceCommunities = new Stack<ChanceCommunity>(ComChaCards);
                ChanceCommunities = chanceCommunities;

                getComCha(p);
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
                if (element is System.Windows.Shapes.Rectangle rectangle)
                {
                    if (rectangle.Name == ("pos") + Position.ToString() && Position != -1)
                    {
                        MoveClockwise(Grid.GetRow(rectangle), Grid.GetColumn(rectangle));
                        break;
                    }
                    else if (Position == -1)
                    {
                        Grid.SetRow(e, 0); Grid.SetColumn(e, 11);
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
            int i = ViewModel.Players[truncount].Money;
            if (i > 0)
            {
                turnorder();
                rolldice.IsEnabled = false;
                EndTurnBtn.IsEnabled = true;
            }
            else
            {
                ViewModel.Players[truncount].asktomortgage();
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
            unmoragagepickacard.Items.Clear();
            moragagepickacard.Items.Clear();
            ObservableCollection<Card> cards = ViewModel.Players[truncount].CardsOwned;
            string str;
            string b5;
            unmoragagepickacard.SelectedIndex = 0;
            moragagepickacard.SelectedIndex = 0;
            if (cards == null)
            {
                b5 = "there are no cards";

                unmoragagepickacard.Items.Add(b5);
                moragagepickacard.Items.Add(b5);
                moragagepickacard.IsEnabled = false;
                unmoragagepickacard.IsEnabled = false;
            }
            else
            {
                foreach (Card card in cards)
                {
                    str = card.ToString();
                    bool b1 = card.IsMortgaged();

                    if (b1 == true)
                    {
                        unmoragagepickacard.Items.Add(str);
                    }
                    else
                    {
                        moragagepickacard.Items.Add(str);
                    }
                }
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
            ObservableCollection<Card> cards = ViewModel.Players[truncount].CardsOwned;
            string st = (string)unmoragagepickacard.SelectedValue;
            foreach (Card card in cards)
            {
                if (card.ToString() == st)
                {
                    ViewModel.Players[truncount].UnMortgageCard(card);
                    changebox();
                }
            }
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
            ObservableCollection<Card> cards = ViewModel.Players[truncount].CardsOwned;
            string st = (string)moragagepickacard.SelectedValue;
            foreach (Card card in cards)
            {
                if (card.ToString() == st)
                {
                    ViewModel.Players[truncount].MortgageCard(card);
                    changebox();
                }
            }

        }

        private void unmoragagepickacard_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void moragagepickacard_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void EndTurnButton_Click(object sender, RoutedEventArgs e)
        {
            if (truncount != players.Count - 1)
            {
                truncount = truncount + 1;
            }
            else
            {
                truncount = 0;
            }
            rolldice.IsEnabled = true;
            EndTurnBtn.IsEnabled = false;
            changebox();
        }

        private void CardsButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var parent = button.Parent as FrameworkElement;
            var popup = new CardDetails();
            popup.Owner = this; // Set the owner to the main window

            popup.Closing += delegate
            {
                popup.Owner = null;
                this.Focus();
            };

            if (parent.Name == PlayerContainer1.Name)
            {
                popup.PlayerCards.ItemsSource = ViewModel.Players[0].CardsNames;
                popup.Show();
                popup.GetPlayer(ViewModel.Players[0]);
            }
            else if (parent.Name == PlayerContainer2.Name)
            {
                popup.PlayerCards.ItemsSource = ViewModel.Players[1].CardsNames;
                popup.Show();
                popup.GetPlayer(ViewModel.Players[1]);
            }
            else if (parent.Name == PlayerContainer3.Name)
            {
                popup.PlayerCards.ItemsSource = ViewModel.Players[2].CardsNames;
                popup.Show();
                popup.GetPlayer(ViewModel.Players[2]);
            }
            else if (parent.Name == PlayerContainer4.Name)
            {
                popup.PlayerCards.ItemsSource = ViewModel.Players[3].CardsNames;
                popup.Show();
                popup.GetPlayer(ViewModel.Players[3]);
            }

        }
    }
}