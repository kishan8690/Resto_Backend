using Microsoft.Data.SqlClient;
using Resto_Backend.Models;
using System.Net.Mail;
using System.Net;

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
        public List<BookingModel> SelectAllExpiredBooking()
        {
            List<BookingModel> bookingList = new List<BookingModel>();
            using (SqlConnection sqlConnection = new SqlConnection(this._configuration.GetConnectionString("ConnectionString")))
            {
                SqlCommand sqlCommand = new SqlCommand("PR_TableBooking_Select_Expired", sqlConnection)
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
                if (rowAffected > 0)
                {
                    // Fetch user email (assuming you have a method for this)
                    string userEmail = GetUserEmail(bookingStatus.BookingID, conn);

                    if (!string.IsNullOrEmpty(userEmail))
                    {
                        SendEmailNotification(userEmail, bookingStatus.BookingStatus);
                    }

                    return true;
                }

                return false;
            }
        }
        private string GetUserEmail(int bookingID, SqlConnection conn)
        {
            string email = "";
            using (SqlCommand cmd = new SqlCommand("SELECT Email FROM [User] WHERE UserID = (SELECT UserID FROM TableBooking WHERE BookingID = @BookingID)", conn))
            {
                cmd.Parameters.AddWithValue("@BookingID", bookingID);
                object result = cmd.ExecuteScalar();
                if (result != null)
                {
                    email = result.ToString();
                }
            }
            return email;
        }
        // Method to send email
        private void SendEmailNotification(string toEmail, string bookingStatus)
        {
            try
            {
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress("amarrestaurant02@gmail.com"); // Your email
                mail.To.Add(toEmail);
                mail.Subject = "Booking Status Update";
                mail.Body = $"Dear {toEmail}, your booking status has been updated to:{bookingStatus}.";

                SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587)
                {
                    Credentials = new NetworkCredential("amarrestaurant02@gmail.com", "siru vdoc tjdx jwbf"),
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false // Ensure this is false!
                };

                smtp.Send(mail);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Email Error: " + ex.Message);
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
