using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Resto_Backend.Data;
using Resto_Backend.Models;

namespace Resto_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly BookingRepository bookingRepository;

        public BookingController(BookingRepository bookingRepository)
        {
            this.bookingRepository = bookingRepository;
        }
        [HttpGet]
        public IActionResult GetAllBooking()
        {
            var bookings = bookingRepository.SelectAllBooking();
            if (bookings == null)
            {
                return NotFound();
            }
            return Ok(bookings);
        }
        [HttpGet("/ExplairedBooking")]
        public IActionResult GetAllExpiredBooking()
        {
            var bookings = bookingRepository.SelectAllExpiredBooking();
            if (bookings == null)
            {
                return NotFound();
            }
            return Ok(bookings);
        }
        [HttpGet("/UserwiseBooking/{userID}")]
        public IActionResult GetAllBookingByUserWise(int userID)
        {
            var bookings = bookingRepository.SelectAllBookingByUserID(userID);
            if (bookings == null)
            {
                return NotFound();
            }
            return Ok(bookings);
        }
        [HttpGet("{id}")]
        public IActionResult GetBookingByID(int id)
        {
            var booking = bookingRepository.SelectBookingByPk(id);
            if (booking == null)
            {
                return NotFound();
            }
            return Ok(booking);
        }
        [HttpPost]
        public IActionResult AddBooking([FromBody] BookingModel booking)
        {
            if (booking == null)
            {
                return BadRequest();
            }
            bool isInsert = bookingRepository.InsertBooking(booking);
            if (isInsert)
            {
                return Ok(new { Message = "Booking is Inserted Successfully !" });
            }
            return StatusCode(500, "An Error occurred while Inserting");
        }
        [HttpPut("{id}")]
        public IActionResult UpdateBooking([FromForm] BookingModel booking)
        {
            if (booking == null)
            {
                return BadRequest();
            }
            bool isUpdate = bookingRepository.UpdateBooking(booking);
            if (isUpdate)
            {
                return Ok(new { Message = "Booking is Updated Successfully !" });
            }
            return StatusCode(500, "An Error occurred while Updating");
        }
        [HttpPatch("{id}")]
        public IActionResult UpdateBookingStatus([FromBody] BookingStatusModel booking)
        {
            if (booking == null)
            {
                return BadRequest();
            }

            bool isUpdate = bookingRepository.UpdateStatus(booking);
            if (isUpdate)
            {
                return Ok(new { Message = "Booking Status is Updated Successfully!" });
            }
            return StatusCode(500, "An Error occurred while Updating");
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteBooking(int id) {
            bool isDelete = bookingRepository.DeleteBooking(id);
            if (isDelete)
            {
                return Ok(new { Message = "Booking is Deleted Successfully !" });
            }
            return StatusCode(500, "An Error occurred while Deleting");
        }
    }
}
