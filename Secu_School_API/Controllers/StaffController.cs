using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Secu_School_API.Data;
using Secu_School_API.DTOs;
using Secu_School_API.Models;

namespace Secu_School_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StaffController : ControllerBase
    {
        private readonly SchoolDbContext _context;

        public StaffController(SchoolDbContext context)
        {
            _context = context;
        }

        [HttpGet("check-phone")]
        public async Task<ActionResult<bool>> CheckPhoneUnique([FromQuery] string phone, [FromQuery] int? excludeStaffId = null)
        {
            if (string.IsNullOrEmpty(phone))
                return BadRequest("Phone number is required");

            if (phone.Length != 10 || !phone.All(char.IsDigit))
                return BadRequest("Invalid phone number format");

            var query = _context.Staffs
                .Where(e => e.Phone == phone);

            if (excludeStaffId.HasValue)
            {
                query = query.Where(e => e.StaffId != excludeStaffId.Value);
            }

            var exists = await query.AnyAsync();
            return Ok(!exists);
        }

        [HttpGet("check-email")]
        public async Task<ActionResult<bool>> CheckEmailUnique(
            [FromQuery] string email,
            [FromQuery] int? excludeStaffId = null)
        {
            if (string.IsNullOrEmpty(email))
                return BadRequest("Email is required");

            // Format validation
            if (!IsValidEmail(email))
                return BadRequest("Invalid email format");

            var query = _context.Staffs
                .Where(e => e.Email == email);

            if (excludeStaffId.HasValue)
            {
                query = query.Where(e => e.StaffId != excludeStaffId.Value);
            }

            var exists = await query.AnyAsync();
            return Ok(!exists);
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllStaffs()
        {
            try
            {
                var staffs = await _context.Staffs
                    .Include(e => e.Department)
                    .Include(e => e.Designation)
                    .Include(e => e.EmployeeType)
                    .Include(e => e.School)
                    .Select(e => new
                    {
                        e.StaffId,
                        e.SchoolId,
                        e.Name,
                        e.RoleId,
                        e.Gender,
                        DateOfBirth = e.DateOfBirth.HasValue ? e.DateOfBirth.Value.ToString("yyyy-MM-dd") : null,
                        e.JoiningDate,
                        e.Email,
                        e.Phone,
                        e.Address,
                        e.DepartmentId,
                        e.DesignationId,
                        e.EmployeeTypeId,
                        e.IsActive,
                        e.IsDeleted
                    })
                    .ToListAsync();

                return Ok(staffs);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving staffs: {ex}");
                return StatusCode(500, new
                {
                    message = "Error retrieving staffs.",
                    error = ex.Message
                });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetStaff(int id)
        {
            try
            {
                var Staff = await _context.Staffs
                    .Include(e => e.Department)
                    .Include(e => e.Designation)
                    .Include(e => e.EmployeeType)
                    .Include(e => e.School)
                    .Where(e => e.StaffId == id)
                    .Select(e => new
                    {
                        e.StaffId,
                        e.SchoolId,
                        e.Name,
                        e.RoleId,
                        e.Gender,
                        DateOfBirth = e.DateOfBirth.HasValue ? e.DateOfBirth.Value.ToString("yyyy-MM-dd") : null,
                        JoiningDate = e.JoiningDate,
                        e.Email,
                        e.Phone,
                        e.Address,
                        e.DepartmentId,
                        e.DesignationId,
                        e.EmployeeTypeId,
                        e.IsActive,
                        e.IsDeleted
                    })
                    .FirstOrDefaultAsync();

                if (Staff == null)
                    return NotFound(new { message = "Staff not found." });

                return Ok(Staff);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving staff: {ex}");
                return StatusCode(500, new
                {
                    message = "Error retrieving staff.",
                    error = ex.Message
                });
            }
        }

        [HttpPost("create-staff")]
        public async Task<IActionResult> CreateStaff([FromBody] StaffDto dto)
        {
            try
            {
                // Validate phone
                var phoneValidation = await ValidatePhone(dto.Phone, null);
                if (phoneValidation != null) return phoneValidation;

                // Validate email
                var emailValidation = await ValidateEmail(dto.Email, null);
                if (emailValidation != null) return emailValidation;

                var entity = new Staff
                {
                    SchoolId = dto.SchoolId,
                    Name = dto.Name,
                    RoleId = dto.RoleId,
                    Gender = dto.Gender,
                    DateOfBirth = dto.DateOfBirth,
                    JoiningDate = dto.JoiningDate,
                    Email = dto.Email,
                    Phone = dto.Phone,
                    Address = dto.Address,
                    DepartmentId = dto.DepartmentId,
                    DesignationId = dto.DesignationId,
                    EmployeeTypeId = dto.EmployeeTypeId,
                    IsActive = dto.IsActive,
                    IsDeleted = dto.IsDeleted,
                };

                _context.Staffs.Add(entity);
                await _context.SaveChangesAsync();

                dto.StaffId = entity.StaffId;
                var role = await _context.Roles.Where(r => r.RoleName.ToLower() == "teacher").FirstOrDefaultAsync();


                // If role is teacher, create a login entry in tbl_login
                if(role != null && dto.RoleId == role.RoleId)
                {
                    string userType = "Teacher";

                    // Create Login entry (tbl_login)
                    var login = new UserLogin
                    {
                        UserName = dto.Email, // Or use employee.First_Name or custom rule
                        PasswordHash = dto.Phone, // Default password (can hash this in real app)
                        UserType = "Teacher", // Set based on role or logic
                        RefId = (int)dto.StaffId,
                        SchoolId = entity.SchoolId
                    };

                    _context.Logins.Add(login);
                    await _context.SaveChangesAsync();


                }

                return Ok(new { message = "Staff and login created successfully.", staff = dto });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating staff: {ex}");
                return BadRequest(new
                {
                    message = "Error creating staff.",
                    error = ex.Message
                });
            }
        }

        [HttpPut("update-staff")]
        public async Task<IActionResult> UpdateStaff([FromBody] StaffDto dto)
        {
            if (!dto.StaffId.HasValue)
                return BadRequest("Staff ID is required.");

            try
            {
                // Validate phone
                var phoneValidation = await ValidatePhone(dto.Phone, dto.StaffId);
                if (phoneValidation != null) return phoneValidation;

                // Validate email
                var emailValidation = await ValidateEmail(dto.Email, dto.StaffId);
                if (emailValidation != null) return emailValidation;

                var entity = await _context.Staffs.FindAsync(dto.StaffId.Value);
                if (entity == null)
                    return NotFound(new { message = "Staff not found." });

                entity.SchoolId = dto.SchoolId;
                entity.Name = dto.Name;
                entity.RoleId = dto.RoleId;
                entity.Gender = dto.Gender;
                entity.DateOfBirth = dto.DateOfBirth;
                entity.JoiningDate = dto.JoiningDate;
                entity.Email = dto.Email;
                entity.Phone = dto.Phone;
                entity.Address = dto.Address;
                entity.DepartmentId = dto.DepartmentId;
                entity.DesignationId = dto.DesignationId;
                entity.EmployeeTypeId = dto.EmployeeTypeId;
                entity.IsActive = dto.IsActive;
                entity.IsDeleted = dto.IsDeleted;

                await _context.SaveChangesAsync();

                return Ok(new { message = "Staff updated successfully.", staff = dto });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating staff: {ex}");
                return BadRequest(new
                {
                    message = "Error updating staff.",
                    error = ex.Message
                });
            }
        }

        [HttpDelete("delete-staff/{id}")]
        public async Task<IActionResult> DeleteStaff([FromRoute] int id)
        {
            try
            {
                var entity = await _context.Staffs.FindAsync(id);
                if (entity == null)
                    return NotFound(new { message = "Staff not found." });

                _context.Staffs.Remove(entity);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Staff deleted successfully.", staff_id = id });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting staff: {ex}");
                return BadRequest(new
                {
                    message = "Error deleting staff.",
                    error = ex.Message
                });
            }
        }

        private async Task<BadRequestObjectResult?> ValidatePhone(string? phone, int? staffId)
        {
            if (string.IsNullOrEmpty(phone))
                return null; // Phone is optional

            // Format validation
            if (phone.Length != 10)
                return BadRequest(new { message = "Phone number must be 10 digits" });

            if (!phone.All(char.IsDigit))
                return BadRequest(new { message = "Phone number must contain only digits" });

            // Uniqueness validation
            var query = _context.Staffs
                .Where(e => e.Phone == phone);

            if (staffId.HasValue)
            {
                query = query.Where(e => e.StaffId != staffId.Value);
            }

            if (await query.AnyAsync())
            {
                return BadRequest(new { message = "Phone number already exists" });
            }

            return null;
        }

        // Add this validation method
        private async Task<BadRequestObjectResult?> ValidateEmail(string? email, int? staffId)
        {
            if (string.IsNullOrEmpty(email))
                return null; // Email is optional

            // Format validation
            if (!IsValidEmail(email))
                return BadRequest(new { message = "Invalid email format" });

            // Uniqueness
            var query = _context.Staffs
                .Where(e => e.Email == email);

            if (staffId.HasValue)
            {
                query = query.Where(e => e.StaffId != staffId.Value);
            }

            if (await query.AnyAsync())
            {
                return BadRequest(new { message = "Email already exists" });
            }

            return null;
        }
    }
}
