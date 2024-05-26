using Microsoft.Data.SqlClient;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;

namespace TetraPolyGame
{
    public class MSSQLdataAccess
    {
        private readonly string _SQLconnectionStrng;

        public MSSQLdataAccess()
        {
            _SQLconnectionStrng = "Data Source=kaidenserver.database.windows.net;Initial Catalog=PublicQuestions;User ID=ReadOnly;Password=Reading123;Connect Timeout=60;Encrypt=True;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";
        }

        /// <summary>Retrieves a collection of property cards from the database.</summary>
        /// <returns>An ObservableCollection of Card objects representing the property cards.</returns>
        /// <exception cref="Exception">Thrown when unable to retrieve property cards from the database.</exception>
        public ObservableCollection<Card> GetProperties()
        {
            ObservableCollection<Card> properties = [];
            try
            {
                using (SqlConnection conn = new SqlConnection(_SQLconnectionStrng))
                {
                    conn.Open();
                    SqlCommand command = new SqlCommand("SELECT * FROM dbo.PropertyCards", conn);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int[] houseRents = new int[6];
                            // Read data from each row and create card objects
                            string name = reader.GetString(0);
                            int position = reader.GetInt32(2);
                            int price = reader.GetInt32(3);
                            int rent = reader.GetInt32(4);
                            houseRents[0] = reader.GetInt32(4);
                            houseRents[1] = reader.GetInt32(5);
                            houseRents[2] = reader.GetInt32(6);
                            houseRents[3] = reader.GetInt32(7);
                            houseRents[4] = reader.GetInt32(8);
                            houseRents[5] = reader.GetInt32(9);
                            int mortgagePrice = reader.GetInt32(10);
                            int mortgageCost = reader.GetInt32(11);
                            int numHouses = reader.GetInt32(12);
                            string colour = reader.GetString(13);
                            // Other relevant columns can be retrieved similarly

                            Property tempProp = new Property(name, position,price, rent, houseRents, null, false, mortgagePrice, mortgageCost, numHouses, colour);
                            properties.Add(tempProp);


                        }
                    }
                }
            }
            catch
            {
                throw new Exception("Unable to retrieve property cards!");
            }
            return properties;
        }

        /// <summary>Retrieves a collection of transport cards from the database.</summary>
        /// <returns>An ObservableCollection of Card objects representing transport cards.</returns>
        /// <exception cref="Exception">Thrown when unable to retrieve transport cards from the database.</exception>
        public ObservableCollection<Card> GetTransport()
        {
            ObservableCollection<Card> Transports = [];
            try
            {
                using (SqlConnection conn = new SqlConnection(_SQLconnectionStrng))
                {
                    conn.Open();
                    SqlCommand command = new SqlCommand("SELECT * FROM dbo.TransportCards", conn);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // Read data from each row and create card objects
                            string name = reader.GetString(0);
                            int position = reader.GetInt32(1);
                            int price = reader.GetInt32(2);
                            int rent = reader.GetInt32(3);
                            int mortgagePrice = reader.GetInt32(4);
                            int mortgageCost = reader.GetInt32(4);
                            // Other relevant columns can be retrieved similarly

                            Transport tempTransport = new Transport(name, position, price, rent, null, false, mortgagePrice, mortgageCost);
                            Transports.Add(tempTransport);


                        }
                    }
                }
            }
            catch
            {
                throw new Exception("Unable to retrieve transport cards!");
            }
            return Transports;
        }

        /// <summary>
        /// Retrieves a collection of utility cards from the database.
        /// </summary>
        /// <returns>An ObservableCollection of Card objects representing utility cards.</returns>
        /// <exception cref="Exception">Thrown when unable to retrieve utility cards from the database.</exception>
        public ObservableCollection<Card> GetUtility()
        {
            ObservableCollection<Card> Utilities = [];
            try
            {
                using (SqlConnection conn = new SqlConnection(_SQLconnectionStrng))
                {
                    conn.Open();
                    SqlCommand command = new SqlCommand("SELECT * FROM dbo.UtilityCards", conn);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // Read data from each row and create card objects
                            string name = reader.GetString(0);
                            int position = reader.GetInt32(1);
                            int price = reader.GetInt32(2);

                            int mortgagePrice = reader.GetInt32(4);
                            int mortgageCost = reader.GetInt32(4);
                            // Other relevant columns can be retrieved similarly

                            Utility tempUtility = new Utility(name, position, price, 0, null, false, mortgagePrice, mortgageCost);
                            Utilities.Add(tempUtility);


                        }
                    }
                }
            }
            catch
            {
                throw new Exception("Unable to retrieve utility cards!");
            }
            return Utilities;
        }

        /// <summary>Retrieves a list of ChanceCommunity objects from the database.</summary>
        /// <returns>A list of ChanceCommunity objects.</returns>
        /// <exception cref="Exception">Thrown when unable to retrieve property cards from the database.</exception>
        public List<ChanceCommunity> GetCommunityChance()
        {
            List<ChanceCommunity> chanceCommunities = [];
            try
            {
                using (SqlConnection conn = new SqlConnection(_SQLconnectionStrng))
                {
                    conn.Open();
                    SqlCommand command = new SqlCommand("SELECT * FROM dbo.ChanceCommunity", conn);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string desc = reader.GetString(0);
                            CardType type = Enum.Parse<CardType>(reader.GetString(1));

                            //Create the community card
                            ChanceCommunity chanceCommunity = new ChanceCommunity(desc, type);
                            chanceCommunities.Add(chanceCommunity);
                        }
                    }
                }
            }
            catch
            {
                throw new Exception("Unable to retrieve property cards!");
            }
            return chanceCommunities;
        }
    }
}