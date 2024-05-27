﻿using System;
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
        private Stack<ChanceCommunity> ChanceCards = new();
        private Stack<ChanceCommunity> CommunityCards = new();
        private List<Event> Events = RandomEvent.GetEvents();
        public static int PastRoll;
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
            PlayerSelection();
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

        private void PlayerSelection()
        {
            var PlayerSelectWin = new PlayerSelect();
            PlayerSelectWin.StartBtn.Click += delegate
            {
                switch (PlayerSelectWin.PlayerAmount.SelectedIndex)
                {
                    case -1:

                        break;

                    case 0:
                        ViewModel.Players =
            [
                new(PlayerSelectWin.P1.Text, 1000),
                new(PlayerSelectWin.P2.Text, 1000)
            ];
                        TestPlayer3.Visibility = Visibility.Hidden;
                        TestPlayer4.Visibility = Visibility.Hidden;
                        PlayerContainer3.Visibility = Visibility.Hidden;
                        PlayerContainer4.Visibility = Visibility.Hidden;
                        break;

                    case 1:
                        ViewModel.Players = [
                new(PlayerSelectWin.P1.Text, 1000),
                new(PlayerSelectWin.P2.Text, 1000),
                new(PlayerSelectWin.P3.Text, 1000)
            ];
                        TestPlayer4.Visibility = Visibility.Hidden;
                        PlayerContainer4.Visibility = Visibility.Hidden;
                        break;

                    case 2:
                        ViewModel.Players = [
                new(PlayerSelectWin.P1.Text, 1000),
                new(PlayerSelectWin.P2.Text, 1000),
                new(PlayerSelectWin.P3.Text, 1000),
                new(PlayerSelectWin.P4.Text, 1000)
            ];
                        break;

                }
                PlayerSelectWin.Close();
            };

            PlayerSelectWin.ShowDialog();
        }

        private void SetupCards()
        {
            Cards = database.GetProperties();
            ObservableCollection<Card> tempTransport = database.GetTransport();
            ObservableCollection<Card> tempUtility = database.GetUtility();
            tempTransport.ToList().ForEach(Cards.Add);
            tempUtility.ToList().ForEach(Cards.Add);
            ComChaCards = database.GetCommunityChance();

            UpdateChanceCommunity();
        }

        private void UpdateChanceCommunity()
        {
            
            Shuffle.Shuffle.ShuffleList(ComChaCards);
            var chanceCommunities = new Stack<ChanceCommunity>(ComChaCards);

            foreach (var card in chanceCommunities)
            {
                switch (card.GetCardType())
                {
                    case "COMMUNITY":
                        CommunityCards.Push(card);
                        break;

                    case "CHANCE":
                        ChanceCards.Push(card);
                        break;
                }

            }
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
                // Stop the movement when reaching (Endrow, Endcollumn)
                if (currentRow == endRow && currentColumn == endColumn)
                {
                    EndTurnBtn.IsEnabled = true;
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
                        if (card is Utility && card.WhoOwns() != ViewModel.Players[turn] && card.IsMortgaged() == false)
                        {
                            Utility tempUtility = (Utility)card;

                            int r = tempUtility.GetRollRent(PastRoll);
                            MessageBox.Show("You landed on " + card.WhoOwns().GetPlayerName() + "'s Property! \nYou now have to pay $" + r + " rent!", "Oh no!", MessageBoxButton.OK, MessageBoxImage.Warning);
                            ViewModel.Players[turn].Money -= r;
                            card.WhoOwns().Money += r;
                            ViewModel.Players[turn].CheckMoney();
                        }
                        else if (card is Transport && card.WhoOwns() != ViewModel.Players[turn] && card.IsMortgaged() == false)
                        {
                            Transport tempTransport = (Transport)card;

                            int r = tempTransport.GetRent();
                            MessageBox.Show("You landed on " + card.WhoOwns().GetPlayerName() + "'s Property! \nYou now have to pay $" + r + " rent!", "Oh no!", MessageBoxButton.OK, MessageBoxImage.Warning);
                            ViewModel.Players[turn].Money -= r;
                            card.WhoOwns().Money += r;
                            ViewModel.Players[turn].CheckMoney();
                        }
                        else if (card.WhoOwns() != ViewModel.Players[turn] && card.IsMortgaged() == false)
                        {
                            int r = card.GetRent();
                            MessageBox.Show("You landed on " + card.WhoOwns().GetPlayerName() + "'s Property! \nYou now have to pay $" + r + " rent!", "Oh no!", MessageBoxButton.OK, MessageBoxImage.Warning);
                            ViewModel.Players[turn].Money -= r;
                            card.WhoOwns().Money += r;
                            ViewModel.Players[turn].CheckMoney();
                        }
                    }
                    if ((card.WhoOwns() == ViewModel.Players[turn]) && (card is Property) && (ViewModel.Players[turn] is not algorithm))
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
                            if (ViewModel.Players[turn].Money <= cost)
                            {
                                MessageBox.Show("Cant buy the house", "Not Enough!", MessageBoxButton.OK, MessageBoxImage.Stop);
                            }
                            else
                            {
                                ViewModel.Players[turn].Money -= cost;
                                ViewModel.Players[turn].AddHouse(tempProp);
                            }

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
                        if (ViewModel.Players[turn].Money <= cost)
                        {
                            MessageBox.Show("Cant buy the house", "Not Enough!", MessageBoxButton.OK, MessageBoxImage.Stop);
                        }
                        else
                        {
                            ViewModel.Players[turn].Money -= cost;
                            ViewModel.Players[turn].AddHouse(tempProp);
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
                }
            }
            if ((ViewModel.Players[turn].GetPosition() == 2) || (ViewModel.Players[turn].GetPosition() == 33) || (ViewModel.Players[turn].GetPosition() == 17))
            {
                GetCommunity(ViewModel.Players[turn]);
            }
            else if ((ViewModel.Players[turn].GetPosition() == 7) || (ViewModel.Players[turn].GetPosition() == 22) || (ViewModel.Players[turn].GetPosition() == 36))
            {
                GetChance(ViewModel.Players[turn]);
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
        public void GetChance(Player p)
        {
            ChanceCommunity temp;
            try
            {
                temp = ChanceCards.Pop();

                MessageBox.Show(temp.GetDesc(), "Chance Card");
                if (temp.GetCardType() == "CHANCE")
                {
                    switch (temp.GetDesc())
                    {
                        case "Advance To Boardwalk":
                            p.MoveToPosition(39);
                            MovePlayer(players[truncount], 39);
                            checkposition(truncount);
                            break;

                        case "Advance To Go":
                            p.Money += (200); p.SetPos(0);
                            MovePlayer(players[truncount], 0);
                            break;

                        case "Go back 3 spaces":
                            p.SetPos(p.GetPosition() - 3);
                            MovePlayer(players[truncount], p.GetPosition());
                            checkposition(truncount);
                            break;

                        case "Go to Jail. Go directly to Jail, do not pass Go, do not collect $200.":
                            p.SetPos(-1);
                            p.SetInJaile(true);
                            MovePlayer(players[truncount], p.GetPosition());
                            break;

                        case "Fined for a LEZ (Light Emmision Zone) Charge ($60)":
                            p.Money -= 60;
                            p.CheckMoney();
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
                            checkposition(truncount);

                            break;
                    }
                }
            }
            catch (Exception e)
            {
                UpdateChanceCommunity();
                GetChance(p);
            }


        }

        /// <summary>
        /// Retrieves a community card for a player and performs the corresponding action.
        /// </summary>
        /// <param name="p">The player for whom the community card action is performed.</param>
        /// <remarks>
        /// The method pops a community card from the stack, displays its description in a message box, and then executes the action based on the card type and description.
        /// </remarks>
        public void GetCommunity(Player p)
        {
            ChanceCommunity temp;
            try
            {
                temp = CommunityCards.Pop();

                MessageBox.Show(temp.GetDesc(), "Community Card");

                if (temp.GetCardType() == "COMMUNITY")
                {
                    switch (temp.GetDesc())
                    {
                        case "Advance To Go":
                            p.Money += (200); p.SetPos(0);
                            MovePlayer(players[truncount], 0);
                            break;

                        case "Bank error in your favor. Collect $200":
                            p.Money += (200);
                            break;

                        case "Your properties are known to be cared for. The extra attention brings in more customers, Collect $100.":
                            p.Money += (100);
                            break;

                        case "You organize a free health screening event for the community. Pay $25 for supplies, but advance to GO (collect $200) for your good deed.":
                            p.Money += (200); p.SetPos(0);
                            MovePlayer(players[truncount], 0);
                            break;

                        case "You help install a new water filtration system in your community. Pay $50 for parts, but your property values increase. Collect $100.":
                            p.Money += (50);

                            break;

                        case "You plant trees in your community, enhancing local biodiversity. Collect $75 as a grant for your green initiative.":
                            p.Money += (75);
                            break;

                        case "You form a partnership with local businesses to support SDGs. Collect $200 as a funding reward.":
                            p.Money += (200);

                            break;

                        case "A drought has affected the local community garden, reducing harvests. Pay $50 for emergency food supplies.":
                            p.Money -= (50);
                            p.CheckMoney();
                            break;

                        case "The local school loses funding, affecting education quality. Pay $50 to support after-school programs.":
                            p.Money -= (50);
                            p.CheckMoney();
                            break;

                        case "New infrastructure project causes road closures, affecting your commute. Pay $25 for additional transportation costs.":
                            p.Money -= (50);
                            p.CheckMoney();
                            break;

                        case "Partnership project fails due to lack of coordination. Pay $75 to cover losses.":
                            p.Money -= (750);
                            p.CheckMoney();
                            break;

                        case "You start a community garden that provides fresh produce to your neighborhood. Collect $100 for your efforts.":
                            p.Money += (100);
                            break;

                        case "You participate in a climate change awareness rally. Pay $10 to cover event costs, but collect $50 for raising awareness.":
                            p.Money += (40);
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                UpdateChanceCommunity();
                GetCommunity(p);
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
                        EndTurnBtn.IsEnabled = true;
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
                //EndTurnBtn.IsEnabled = true;
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
            if (truncount != ViewModel.Players.Count - 1)
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
            switch (truncount)
            {
                case 0:
                    Player1Active.Visibility = Visibility.Visible;

                    Player2Active.Visibility = Visibility.Hidden;
                    Player3Active.Visibility = Visibility.Hidden;
                    Player4Active.Visibility = Visibility.Hidden;
                    break;

                case 1:
                    Player2Active.Visibility = Visibility.Visible;

                    Player1Active.Visibility = Visibility.Hidden;
                    Player3Active.Visibility = Visibility.Hidden;
                    Player4Active.Visibility = Visibility.Hidden;
                    break;

                case 2:
                    Player3Active.Visibility = Visibility.Visible;

                    Player2Active.Visibility = Visibility.Hidden;
                    Player1Active.Visibility = Visibility.Hidden;
                    Player4Active.Visibility = Visibility.Hidden;
                    break;

                case 3:
                    Player4Active.Visibility = Visibility.Visible;

                    Player1Active.Visibility = Visibility.Hidden;
                    Player2Active.Visibility = Visibility.Hidden;
                    Player3Active.Visibility = Visibility.Hidden;
                    break;
            }

            Random RandomID = new Random();
            if (RandomEvent.EventChance())
            {
                CheckEvent(RandomID.Next(1, 11));
            }
        }

        private void CheckEvent(int ID)
        {
            foreach (Event AnEvent in Events)
            {
                if (AnEvent.GetId() == ID)
                {
                    int index = 0;
                    int money = 0;
                    MessageBox.Show(AnEvent.GetDesc(), AnEvent.GetName(), MessageBoxButton.OK, MessageBoxImage.Information);
                    switch (AnEvent.GetId())
                    {
                        case 1:
                            foreach (Player player in ViewModel.Players)
                            {
                                player.Money -= 25;
                                player.CheckMoney();
                            }
                            break;

                        case 2:
                            foreach (Player player in ViewModel.Players)
                            {
                                player.Money -= 75;
                                player.CheckMoney();
                            }
                            break;

                        case 3:
                            foreach (Player player in ViewModel.Players)
                            {
                                player.SetPos(35);
                                MovePlayer(players[index], 35);
                                checkposition(index);
                                index++;
                            }
                            break;

                        case 4:
                            foreach (Player player in ViewModel.Players)
                            {
                                player.Money -= 30;
                                player.CheckMoney();
                            }
                            break;

                        case 5:

                            foreach (Player player in ViewModel.Players)
                            {
                                foreach (Card card in player.CardsOwned)
                                {
                                    if (card is Property)
                                    {
                                        money += 10;
                                    }
                                }
                                player.Money -= money;
                                player.CheckMoney();
                            }
                            break;

                        case 6:
                            foreach (Player player in ViewModel.Players)
                            {
                                if (player.GetPosition() != 0)
                                {
                                    player.SetPos(player.GetPosition() - 1);
                                    MovePlayer(players[index], player.GetPosition());
                                    checkposition(index);
                                }
                                index++;
                            }
                            break;

                        case 7:
                            foreach (Player player in ViewModel.Players)
                            {
                                player.SetPos(player.GetPosition() + 2);
                                MovePlayer(players[index], player.GetPosition());
                                checkposition(index);
                                index++;
                            }
                            break;

                        case 8:
                            foreach (Player player in ViewModel.Players)
                            {
                                player.Money -= 40;
                                player.CheckMoney();
                            }
                            break;

                        case 9:

                            foreach (Player player in ViewModel.Players)
                            {
                                foreach (Card card in player.CardsOwned)
                                {
                                    if (card is Property)
                                    {
                                        money += 25;
                                    }
                                }
                                player.Money -= money;
                                player.CheckMoney();
                            }
                            break;

                        case 10:
                            foreach (Player player in ViewModel.Players)
                            {
                                player.Money -= 100;
                                player.CheckMoney();
                            }
                            break;

                        case 11:
                            foreach (Player player in ViewModel.Players)
                            {
                                player.Money -= 120;
                                player.CheckMoney();
                            }
                            break;
                    }
                }
            }
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