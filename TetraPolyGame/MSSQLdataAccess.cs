using Microsoft.Data.SqlClient;
using System.Text;

namespace TetraPolyGame
{
    internal class MSSQLdataAccess
    {
        private readonly string _SQLconnectionStrng;

        public MSSQLdataAccess()
        {
            _SQLconnectionStrng = "Data Source=kaidenserver.database.windows.net;Initial Catalog=PublicQuestions;User ID=ReadOnly;Connect Timeout=60;Encrypt=True;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";
        }

        public List<Object> GetCards()
        {
            List<Object> cards = new List<Object>();
            try
            {
                using (SqlConnection conn = new SqlConnection(_SQLconnectionStrng))
                {
                    conn.Open();
                    SqlCommand command = new SqlCommand("SELECT * FROM dbo.PropCards", conn);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            switch (reader.GetInt32(0))
                            {
                                case 1:
                                    Question newQ = new TextQuestion(reader.GetString(1), reader.GetString(2), reader.GetInt32(3));
                                    cards.Add(newQ);
                                    break;

                                case 2:
                                    TrueFalseQuestion newTFQ = new TrueFalseQuestion(reader.GetString(1), bool.Parse(reader.GetString(2)), reader.GetInt32(3));
                                    cards.Add(newTFQ);
                                    break;

                                case 3:
                                    string line = reader.GetString(4);

                                    List<string> options = line.Split(";").ToList();

                                    MultipleChoiceQuestion newMQ = new MultipleChoiceQuestion(reader.GetString(1), reader.GetString(2), options, reader.GetInt32(3));
                                    cards.Add(newMQ);
                                    break;
                            }
                            //cards.Add(new Question(reader.GetString(1), reader.GetString(2), reader.GetInt32(3)));
                        }
                    }
                }
            }
            catch
            {
                throw new Exception("Unable to retrieve cards!");
            }
            return cards;
        }

        public void InsertQuestionIntoDB(int QType, string question, string answer, int value, string[] options)
        {
            string sql = "";
            StringBuilder sb = new StringBuilder();
            if (QType == 0)
            {
                sql = "INSERT INTO [dbo].[publicquestion] ([type], [question], [answer], [value], [multichoice]) VALUES (1, N'" + question + "', N'" + answer + "', " + value + ", NULL)";
            }
            else if (QType == 1)
            {
                sql = "INSERT INTO [dbo].[publicquestion] ([type], [question], [answer], [value], [multichoice]) VALUES (2, N'" + question + "', N'" + answer + "', " + value + ", NULL)";
            }
            else if (QType == 2)
            {
                sb.Append("INSERT INTO [dbo].[publicquestion] ([type], [question], [answer], [value], [multichoice]) VALUES (3, N'" + question + "', N'" + answer + "', " + value + ", N'");

                foreach (string option in options)
                {
                    sb.Append(option + ";");
                }
                sb.Remove(sb.Length, 1);
                sb.Append("')");
                sql = sb.ToString();
                MessageBox.Show(sql);
            }
            using (SqlConnection cnn = new SqlConnection(_SQLconnectionStrng))
            {
                try
                {
                    // Open the connection to the database.
                    // This is the first critical step in the process.
                    // If we cannot reach the db then we have connectivity problems
                    cnn.Open();

                    // Prepare the command to be executed on the db
                    using (SqlCommand cmd = new SqlCommand(sql, cnn))
                    {
                        // Let's ask the db to execute the query
                        int rowsAdded = cmd.ExecuteNonQuery();
                        if (rowsAdded > 0)
                            MessageBox.Show("Row inserted!!");
                        else
                            // Well this should never really happen
                            MessageBox.Show("No row inserted");
                    }
                }
                catch (Exception ex)
                {
                    // We should log the error somewhere,
                    // for this example let's just show a message
                    MessageBox.Show("ERROR:" + ex.Message);
                }
            }
        }
    }
}