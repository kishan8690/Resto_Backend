using Microsoft.Data.SqlClient;
using Resto_Backend.Models;

namespace Resto_Backend.Data
{
    public class BookingRepository
    {

        public readonly IConfiguration _configuration;
        public BookingRepository(IConfiguration configuration)
        {
            this._configuration = configuration;
        }
        public List<BookingModel> SelectAllBooking()
        {
            List<BookingModel> bookingList = new List<BookingModel>();
            using (SqlConnection sqlConnection = new SqlConnection(this._configuration.GetConnectionString("ConnectionString")))
            {
                SqlCommand sqlCommand = new SqlCommand("PR_TableBooking_Select", sqlConnection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                sqlConnection.Open();
                SqlDataReader reader = sqlCommand.ExecuteReader();
                while (reader.Read())
                {
                    BookingModel booking = new BookingModel
                    {
                        BookingID = Convert.ToInt32(reader["BookingID"]),
                        BookingDate = Convert.ToDateTime(reader["BookingDate"]),
                        UserID = Convert.ToInt32(reader["UserID"]),
                        UserName = reader["UserName"].ToString(),
                        NumberOfPerson = Convert.ToInt32(reader["NumberOfGuests"]),
                        BookingStatus = reader["BookingStatus"].ToString()
                    };
                    bookingList.Add(booking);
                }
            }
            return bookingList;
        }
        public List<BookingModel> SelectAllBookingByUserID(int userID)
        {
            List<BookingModel> bookingList = new List<BookingModel>();
            using (SqlConnection sqlConnection = new SqlConnection(this._configuration.GetConnectionString("ConnectionString")))
            {
                SqlCommand sqlCommand = new SqlCommand("PR_TableBooking_SelectByUserID", sqlConnection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                sqlCommand.Parameters.AddWithValue("@UserID", userID);
                sqlConnection.Open();
                SqlDataReader reader = sqlCommand.ExecuteReader();
                while (reader.Read())
                {
                    BookingModel booking = new BookingModel
                    {
                        BookingID = Convert.ToInt32(reader["BookingID"]),
                        BookingDate = Convert.ToDateTime(reader["BookingDate"]),
                        UserID = Convert.ToInt32(reader["UserID"]),
                        UserName = reader["UserName"].ToString(),
                        NumberOfPerson = Convert.ToInt32(reader["NumberOfGuests"]),
                        BookingStatus = reader["BookingStatus"].ToString()
                    };
                    bookingList.Add(booking);
                }
            }
            return bookingList;
        }
        public BookingModel SelectBookingByPk(int bookingId)
        {
            BookingModel booking = new BookingModel();
            using (SqlConnection sqlConnection = new SqlConnection(this._configuration.GetConnectionString("ConnectionString")))
            {
                SqlCommand sqlCommand = new SqlCommand("PR_TableBooking_Select_By_PK", sqlConnection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                sqlCommand.Parameters.AddWithValue("@BookingID", bookingId);
                sqlConnection.Open();
                SqlDataReader reader = sqlCommand.ExecuteReader();
                if (reader.Read()) // Ensure only one record is processed
                {
                    booking.BookingID = Convert.ToInt32(reader["BookingID"]);
                    booking.BookingDate = Convert.ToDateTime(reader["BookingDate"]);
                    booking.UserID = Convert.ToInt32(reader["UserID"]);
                    booking.UserName = reader["UserName"].ToString();
                    booking.NumberOfPerson = Convert.ToInt32(reader["NumberOfGuests"]);
                    booking.BookingStatus = reader["BookingStatus"].ToString();
                }
            }
            return booking;
        }
        public bool InsertBooking(BookingModel booking)
        {
            using (SqlConnection conn = new SqlConnection(this._configuration.GetConnectionString("ConnectionString")))
            {
                SqlCommand sqlCommand = new SqlCommand("PR_TableBooking_Insert", conn)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                sqlCommand.Parameters.AddWithValue("@BookingDate", booking.BookingDate);
                sqlCommand.Parameters.AddWithValue("@UserID", booking.UserID);
                sqlCommand.Parameters.AddWithValue("@NumberOfGuests", booking.NumberOfPerson);
                sqlCommand.Parameters.AddWithValue("@BookingStatus", "Pendding");
                conn.Open();
                int rowAffected = sqlCommand.ExecuteNonQuery();
                return rowAffected > 0;
            }
        }
        public bool UpdateBooking(BookingModel booking)
        {
            using (SqlConnection conn = new SqlConnection(this._configuration.GetConnectionString("ConnectionString")))
            {
                SqlCommand sqlCommand = new SqlCommand("PR_TableBooking_Update", conn)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                sqlCommand.Parameters.AddWithValue("@BookingID", booking.BookingID);
                sqlCommand.Parameters.AddWithValue("@BookingDate", booking.BookingDate);
                sqlCommand.Parameters.AddWithValue("@UserID", booking.UserID);
                sqlCommand.Parameters.AddWithValue("@NumberOfGuests", booking.NumberOfPerson);
                conn.Open();
                int rowAffected = sqlCommand.ExecuteNonQuery();
                return rowAffected > 0;
            }
        }
        public bool UpdateStatus(BookingStatusModel bookingStatus)
        {
            using (SqlConnection conn = new SqlConnection(this._configuration.GetConnectionString("ConnectionString")))
            {
                SqlCommand sqlCommand = new SqlCommand("PR_BookingStatus_Update", conn)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                sqlCommand.Parameters.AddWithValue("@BookingID", bookingStatus.BookingID);
                sqlCommand.Parameters.AddWithValue("@BookingStatus", bookingStatus.BookingStatus);
                conn.Open();
                int rowAffected = sqlCommand.ExecuteNonQuery();
                return rowAffected > 0;
            }
        }
        public bool DeleteBooking(int bookingId)
        {
            using (SqlConnection conn = new SqlConnection(this._configuration.GetConnectionString("ConnectionString")))
            {
                SqlCommand sqlCommand = new SqlCommand("PR_TableBooking_Delete", conn)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                sqlCommand.Parameters.AddWithValue("@BookingID", bookingId);
                conn.Open();
                int rowAffected = sqlCommand.ExecuteNonQuery();
                return rowAffected > 0;
            }
        }
    }
}
