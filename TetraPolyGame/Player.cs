using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using System.Threading.Channels;
using System.Windows;
using System.Windows.Automation.Peers;
namespace TetraPolyGame
{
    public class Player
    {


        protected bool _IsAilve;
        protected bool _InJail;
        protected string _Name;
        protected int _Money;
        private int _Position;
        protected List<Card> _CardsOwend;
        protected List<ChanceCommunity> _chanceCommunitiesOwend;

        public event PropertyChangedEventHandler PropertyChanged;
        //this is the constroctor for the Player
        public Player(string name, int money)
        {
            _Name = name;
            _Money = money;
            _Position = 0;
            _CardsOwend = new List<Card>();
            _chanceCommunitiesOwend = new List<ChanceCommunity>();
            _IsAilve = true;
            _InJail = false;
        }
        //check a is the card 
        public bool CheckSet(Card check)
        {
            //
            int[] colours;
            bool t = false;
            int count = 0;
            if (_CardsOwend != null)
            {
                foreach (Card card in _CardsOwend)
                {
                    if (check is Property checkProperty)
                    {
                        if (card is Property property && property.GetColour() == checkProperty.GetColour())
                        {

                        }
                    }
                    count = count + 1;
                }
                return t;
            }
            else
            {
                return false;
            }

        }
        /// <summary>
        /// Adds a house to the specified property.
        /// </summary>
        /// <param name="pro">The property to which the house will be added.</param>
        public void AddHouse(Property pro)
        {
            pro.AddHouse();
        }
        /// <summary>Removes a specified number of houses from a property.</summary>
        /// <param name="pro">The property from which houses will be removed.</param>
        /// <param name="mnyhosetoremove">The number of houses to remove.</param>
        public void removeHouse(Property pro, int mnyhosetoremove)
        {
            for (int x = 0; x != mnyhosetoremove; x++)
            {
                pro.RemoveHouse(mnyhosetoremove);
            }
        }

        /// <summary>Adds a card to the player's owned cards list, sets the owner of the card to the player, and displays a message box.</summary>
        /// <param name="CARD">The card to be added.</param>
        public void byCard(Card CARD)
        {
            _CardsOwend.Add(CARD);
            CARD.SetOwner(this);
            MessageBox.Show("You now own: " + CARD.GetName());
        }

        /// <summary>Sells a card by removing it from the player's owned cards.</summary>
        /// <param name="CARD">The card to be sold.</param>
        public void SellCard(Card CARD)
        {
            _CardsOwend.Remove(CARD);
        }

        /// <summary>Mortgages a card and adds the mortgage price to the player's money.</summary>
        /// <param name="card">The card to be mortgaged.</param>
        public void MortgageCard(Card card)
        {
            card.SetMorgaged(true);
            int mor = card.GetMortgagePrice();
            addMoney(mor);
        }
        /// <summary>Handles the mortgage action for a card.</summary>
        /// <param name="card">The card to be mortgaged.</param>
        /// <remarks>This method calculates the mortgage price of the card, deducts the amount from the player's funds, and sets the card as not mortgaged.</remarks>
        public void OnMortgageCard(Card card)
        {
            int mor = card.GetMortgagePrice();
            LoseMoney(mor);
            card.SetMorgaged(false);
        }

        /// <summary>
        /// Simulates rolling a six-sided dice and returns the result.
        /// </summary>
        /// <returns>An integer representing the result of the dice roll (between 1 and 6).</returns>
        public int RollDice()
        {
            Random d = new Random();
            int dn = d.Next(1, 6);
            return dn;
        }

        /// <summary>
        /// Moves the player on the game board based on dice rolls.
        /// </summary>
        /// <remarks>
        /// The player can only move if they are alive and not in jail.
        /// If the player rolls doubles three times in a row, they go to jail.
        /// If the player is in jail, they must roll doubles to get out.
        /// </remarks>
        virtual public void MovePlayer()
        {
            if (_IsAilve == true)
            {
                if (_InJail == false)
                {
                    int count = 0;
                    bool b = true;
                    while (b)
                    {
                        int move = RollDice();
                        int move2 = RollDice();
                        _Position = _Position + move + move2;
                        if (_Position > 39)
                        {
                            addMoney(200);
                            setPosition(_Position - 40);
                        }
                        if (move2 == move)
                        {
                            count = count + 1;
                            if (count != 3)
                            {
                                b = true;
                            }
                            else
                            {
                                _Position = -1;
                                _InJail = true;
                                b = false;
                            }
                        }
                        else
                        {
                            b = false;
                        }
                    }
                }
                if (_InJail == true)
                {
                    int r1 = RollDice();
                    int r2 = RollDice();
                    if (r1 == r2)
                    {
                        _InJail = false;
                        _Position = 10;
                    }
                }
            }
        }

        /// <summary>
        /// Initiates a purchase using a card.
        /// </summary>
        /// <param name="chois">A boolean indicating whether the purchase is confirmed.</param>
        /// <param name="gc">The card used for the purchase.</param>
        /// <remarks>
        /// If the purchase is confirmed, the method deducts the price from the account balance
        /// based on the card's price and processes the purchase using the provided card.
        /// </remarks>
        public virtual void buy(bool chois, Card gc)
        {
            if (chois == true)
            {
                int se = gc.GetPrice();
                LoseMoney(se);
                byCard(gc);
            }

        }
        /// <summary>
        /// Simulates losing money from the player's balance.
        /// </summary>
        /// <param name="money">The amount of money to deduct from the player's balance.</param>
        /// <remarks>
        /// If the deduction results in a negative balance, it checks if the player can mortgage properties to cover the debt.
        /// If mortgaging is not possible, the player is marked as not alive.
        /// </remarks>
        virtual public void LoseMoney(int money)
        {
            _Money = _Money - money;
            if (_Money < 0)
            {
                bool b = checktotalmorgag();
                if (b == true)
                {
                    asktomortgage();
                }
                if (b == false)
                {
                    _IsAilve = false;
                }
            }
        }
        /// <summary>Checks if the total mortgage value of owned cards is greater than or equal to the negative money balance.</summary>
        /// <returns>True if the total mortgage value is greater than or equal to the negative money balance, false otherwise.</returns>
        public bool checktotalmorgag()
        {
            int count = 0;
            int total = 0;
            while (_CardsOwend[count] != null)
            {
                if (_CardsOwend[count] is Card && _CardsOwend[count].IsMortgaged() == false)
                {
                    total = _CardsOwend[count].GetMortgagePrice();
                }

                count = count + 1;
            }
            if (total >= (_Money * -1))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// Displays a message box informing the user to mortgage some of their properties.
        /// </summary>
        /// <remarks>
        /// This method prompts the user with a message indicating the need to mortgage certain properties.
        /// </remarks>
        public void asktomortgage()
        {
            string s = "you need to morgae some of your a cards";
            MessageBoxResult w = MessageBox.Show(s);
        }

        /// <summary>
        /// Gets the amount of money the player has.
        /// </summary>
        /// <returns>The amount of money.</returns>
        public int getMoney()
        {
            return _Money;

        }

        /// <summary>Adds the specified amount of money to the current balance.</summary>
        /// <param name="money">The amount of money to add.</param>
        public void addMoney(int money)
        {
            _Money += money;
            OnPropertyChanged();
        }

        /// <summary>Handles the action when a player passes the "Go" position on the board.</summary>
        /// <remarks>If the player's position is at or beyond the "Go" position (0), the player's position is adjusted
        /// to the appropriate position after passing "Go" and the player receives $200.</remarks>
        public void PassedGo()
        {
            if (_Position <= 40)
            {
                _Position = _Position - 40;
                addMoney(200);
            }
        }

        /// <summary>
        /// Gets the current position.
        /// </summary>
        /// <returns>The current position.</returns>
        public int GetPosition()
        {
            return _Position;
        }

        /// <summary>Sets the position value.</summary>
        /// <param name="position">The new position value to set.</param>
        public void setPosition(int position)
        {
            _Position = position;
        }

        /// <summary>
        /// Gets the current status of the object's "alive" state.
        /// </summary>
        /// <returns>True if the object is alive, false otherwise.</returns>
        public bool GetAilve()
        {
            return _IsAilve;
        }

        /// <summary>Sets the status of the object to alive or not alive.</summary>
        /// <param name="B">A boolean value indicating whether the object is alive (true) or not alive (false).</param>
        public void SetAilve(bool B)
        {
            _IsAilve = B;
            OnPropertyChanged();
        }

        /// <summary>
        /// Gets the current status of whether the player is in jail.
        /// </summary>
        /// <returns>True if the player is in jail, false otherwise.</returns>
        public bool GetInJail()
        {
            return _InJail;
        }

        /// <summary>Sets the status of whether an object is in jail or not.</summary>
        /// <param name="b">A boolean value indicating if the object is in jail.</param>
        public void SetInJaile(bool b)
        {
            _InJail = b;
            OnPropertyChanged();
        }

        /// <summary>
        /// Gets the amount of money.
        /// </summary>
        /// <returns>The amount of money.</returns>
        public int Getmoney()
        {
            return _Money;
        }

        public int Money
        {
            get => _Money;
            set
            {
                _Money = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Retrieves a list of cards owned by the player.
        /// </summary>
        /// <returns>A list of Card objects representing the cards owned by the player.</returns>
        public List<Card> GetCards()
        {
            return _CardsOwend;
        }

        /// <summary>
        /// Retrieves a list of ChanceCommunity objects owned by the player.
        /// </summary>
        /// <returns>A list of ChanceCommunity objects owned by the player.</returns>
        public List<ChanceCommunity> GetchanceCommunitiesOwend()
        {
            return _chanceCommunitiesOwend;
        }

        /// <summary>Adds a ChanceCommunity to the list of owned ChanceCommunities.</summary>
        /// <param name="chance">The ChanceCommunity to be added.</param>
        public void AddchanceCommunitiesOwend(ChanceCommunity chance)
        {
            _chanceCommunitiesOwend.Add(chance);
            OnPropertyChanged();
        }

        /// <summary>Removes a ChanceCommunity object from the list of owned ChanceCommunity objects.</summary>
        /// <param name="i">The index of the ChanceCommunity object to be removed.</param>
        public void removechanceCommunitiesOwend(int i)
        {
            ChanceCommunity chance = _chanceCommunitiesOwend[i];
            _chanceCommunitiesOwend.Remove(chance);
            OnPropertyChanged();
        }
        /// <summary>Sets the position based on the provided number.</summary>
        /// <param name="number">The number to set the position to.</param>
        public void SetPos(int number)
        {
            setPosition(number);

        }
        public void MoveToPosition(int newPosition)
        {
            SetPos(newPosition);
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}