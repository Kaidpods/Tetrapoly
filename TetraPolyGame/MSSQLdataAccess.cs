using Microsoft.Data.SqlClient;
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

        public List<Card> GetProperties()
        {
            List<Card> properties = [];
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
                            if (properties.Count == 9)
                            {
                                MessageBox.Show("Wait");
                            }
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
                            String effectDesc = reader.GetString(2);

                            //Create the community card
                            ChanceCommunity chanceCommunity = new ChanceCommunity(desc, type, effectDesc);
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