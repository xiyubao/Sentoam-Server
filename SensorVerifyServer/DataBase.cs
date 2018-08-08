using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SensorVerifyServer
{
    public partial class MainWindow : Window
    {
        private MySqlConnection sqlconnection=null;
        private string serverip = "111.231.57.188";
        private string database_name = "sensorverify";
        private string username = "victor";
        private string password = "250413";
        private string train_table = "train_table";
        private string validation_table = "validation_table";

        private void Connect_Database(object sender, RoutedEventArgs e)
        {
            if (sqlconnection == null)
            {
                sqlconnection = LoadSqlServer(database_name, username, password);
                if (sqlconnection != null)
                {
                    Light(true, 2);
                    button2.Content = "Close DB";
                    log("connect to database successfully");

                    UpdateID();

                }
                else
                {
                    log("can not connect to database");
                }
            }
            else
            {
                train_id = 0;
                validation_id = 0;
                Database_Clear();
                Light(false, 2);
                button2.Content = "Connect DB";
            }
        }

        public MySqlConnection LoadSqlServer(String database, String username, String password)
        {
            
            String connectionString = "server="+serverip+";user id=" + username + ";pwd=" + password + ";database=" + database;
            MySqlConnection conn = null;
            try
            {
                conn = new MySqlConnection(connectionString);
                conn.Open();
            }
            catch
            {
                conn = null;
                Console.WriteLine("error in MysqlConnection");
            }
            return conn;
        }

        private bool ReadTrain()
        {
            train_set = new List<train>();

            string str = "select * from " + train_table + " where (true)";
            //sql数据库

            MySqlCommand command = new MySqlCommand(str, sqlconnection);
            MySqlDataReader reader = command.ExecuteReader();

            while(reader.Read())
            {
                int index = 0;
                int id = int.Parse(reader[index++].ToString());
                int d = int.Parse(reader[index++].ToString());
                double []msg1 = new double[d];
                double[] msg2 = new double[d];
                for (int i = 0; i < d; ++i)
                    msg1[i] = double.Parse(reader[index++].ToString());
                for (int i = 0; i < d; ++i)
                    msg2[i] = double.Parse(reader[index++].ToString());
                bool flag = reader[index++].ToString() == "True";
                train_set.Add(new train(id, d, msg1, msg2, flag));
            }

            reader.Close();

            log("Get train dataset from database");

            return train_set.Count != 0;
        }

        private bool ReadValidation()
        {
            validation_set = new List<validation>();

            string str = "select * from " + validation_table + " where (true)";
            //sql数据库

            MySqlCommand command = new MySqlCommand(str, sqlconnection);
            MySqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                int index = 0;
                int id = int.Parse(reader[index++].ToString());
                int d = int.Parse(reader[index++].ToString());
                double[] msg1 = new double[d];
                double[] msg2 = new double[d];
                for (int i = 0; i < d; ++i)
                    msg1[i] = double.Parse(reader[index++].ToString());
                for (int i = 0; i < d; ++i)
                    msg2[i] = double.Parse(reader[index++].ToString());
                String flag = reader[index++].ToString();
                bool flagR = reader[index++].ToString() == "True"||reader[index++].ToString()=="1";
                validation_set.Add(new validation(id, d, msg1, msg2, flag,flagR));
            }

            reader.Close();

            log("Get validation dataset from database");

            return  validation_set.Count != 0;
        }

        public void Write(train t)
        {
            string str = "insert into " + train_table + " values(" + t.id + ","+t.d+",";
            for(int i=0;i<t.d;++i)
            {
                str += t.msg1[i] + ",";
            }
            for(int i=0;i<t.d;++i)
            {
                str += t.msg2[i] + ",";
            }
            str += t.flag+")";

            if (sqlconnection == null)
            {
                log("database is closing");
                train_id--;
            }
            else
                log(str);
            
            MySqlCommand command = new MySqlCommand(str, sqlconnection);
            command.CommandText = str;
            command.ExecuteNonQuery();
            
        }

        public void Write(validation t)
        {
            string str = "insert into " + validation_table + " values(" + t.id + "," + t.d + ",";
            for (int i = 0; i < t.d; ++i)
            {
                str += t.msg1[i] + ",";
            }
            for (int i = 0; i < t.d; ++i)
            {
                str += t.msg2[i] + ",";
            }
            str += t.flag + "," + t.flagR + ")";

            if (sqlconnection == null)
            {
                log("database is closing");
                validation_id--;
            }
            else
                log(str);

            MySqlCommand command = new MySqlCommand(str, sqlconnection);
            command.CommandText = str;
            command.ExecuteNonQuery();
        }

        private void UpdateID()
        {
            string str = "select * from " + train_table + " where (true)";
            //sql数据库

            MySqlCommand command = new MySqlCommand(str, sqlconnection);
            MySqlDataReader reader = command.ExecuteReader();
            while(reader.Read())
            {
                train_id++;
            }

            reader.Close();

            str = "select * from " + validation_table + " where (true)";
            command = new MySqlCommand(str, sqlconnection);
            reader = command.ExecuteReader();
            while(reader.Read())
            {
                validation_id++;
            }

            reader.Close();
        }

        private void Database_Clear()
        {
            log("database has been disconnected");
            sqlconnection.Close();
            sqlconnection = null;
        }
    }
}
