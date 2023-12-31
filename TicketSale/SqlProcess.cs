﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TicketSale
{
    public class SqlProcess
    {

        private string connectionString = "Server=.;Database=TicketSale;User Id=Cuni;Password=123456;";
        SqlConnection connection = null;
        SqlCommand command = null;
        private Dictionary<string, Tuple<string, string>> airports = new Dictionary<string, Tuple<string, string>>();
        private List<Tuple<int, string, string, DateTime, DateTime, int, double>> flights = new List<Tuple<int, string, string, DateTime, DateTime, int, double>>();
        //private (string departureAirport, string arrivalAirport, DateTime departureTime, DateTime arrivalTime, int capacity) flights;


        public bool SqlConnect(string connectionString) // veri tabanı bağlantısını açan metot
        {
            try
            {
                connection = new SqlConnection(connectionString);
                connection.Open();
                return true; // bağlantı başarıyla açılırsa true değerini döndürür
            }
            catch (Exception ex)
            {
                MessageBox.Show("ex.message: " + ex.Message + " stacktrace:" + ex.StackTrace, "Sql Bağlantı Hatası");
                return false; // bir hata oluşursa false değerini döndürür
            }
        }

        public Dictionary<string, Tuple<string, string>> GetAirports(string query) // veri tabanından kayıtlı hava alanlarını çeken metot
        {
            try
            {
                SqlDataReader reader = null;
                if (!SqlConnect(connectionString)) // veri tabanı bağlantısını açar bir hata oluşursa boş değer döndürür
                    return null;

                command = new SqlCommand(query, connection);
                reader = command.ExecuteReader();

                while (reader.Read())
                    airports.Add(reader.GetString(1), Tuple.Create(reader.GetString(2), reader.GetString(3)));

                command.Dispose();
                command = null;
                reader.Close();
                reader = null;

                return airports;
            }
            catch (Exception ex)
            {
                MessageBox.Show("ex.message: " + ex.Message + " stacktrace: " + ex.StackTrace, "GetAirports Hatası");
                return null;
            }
        }

        public List<Tuple<int, string, string, DateTime, DateTime, int, double>> GetFlights(string query) // veri tabanında kayıtlı uçuşları çeker
        {
            try
            {
                SqlDataReader reader = null;
                if (!SqlConnect(connectionString))
                    return null;
                command = new SqlCommand(query, connection);
                reader = command.ExecuteReader();
                while (reader.Read())
                    flights.Add(Tuple.Create(int.Parse(reader.GetByte(0).ToString()), reader.GetString(1), reader.GetString(2), DateTime.Parse(reader.GetTimeSpan(3).ToString()), DateTime.Parse(reader.GetTimeSpan(4).ToString()), int.Parse(reader.GetByte(5).ToString()), double.Parse(reader.GetDecimal(6).ToString())));
                command.Dispose();
                command = null;
                reader.Close();
                reader = null;
                return flights;
            }
            catch (Exception ex)
            {
                MessageBox.Show("ex.message: " + ex.Message + " stacktrace: " + ex.StackTrace, "GetFlights Hatası");
                return null;
            }
        }

        public bool UpdateCapacity(string query)
        {
            try
            {
                if (!SqlConnect(connectionString))
                    return false;
                command = new SqlCommand(query, connection); 
                command.ExecuteNonQuery();
                command.Dispose();
                command = null;
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("ex.message: " + ex.Message + " stacktrace: " + ex.StackTrace, "UpdateCapacity Hatası");
                return false;
            }
        }
    }
}
