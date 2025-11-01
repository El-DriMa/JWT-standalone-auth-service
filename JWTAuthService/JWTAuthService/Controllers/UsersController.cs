using MapsterMapper;
using JWTAuthService.Database;
using JWTAuthService.Helpers;
using JWTAuthService.Interfaces;
using JWTAuthService.Models.Requests;
using JWTAuthService.Models.Responses;
using JWTAuthService.SearchObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Identity.Client;

namespace JWTAuthService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
  //  [Authorize(Roles = "Admin")]
  //  [EnableRateLimiting("fixed")]
    public class UserController : BaseCRUDController<UserResponse, UserSearchObject, UserInsertRequest, UserUpdateRequest>
    {
        private readonly MyDatabaseContext _context;
        private readonly IMapper _mapper;
        public UserController(ILogger<BaseController<UserResponse, UserSearchObject>> logger, IUserService service, MyDatabaseContext context, IMapper mapper) : base(logger, service)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request, [FromServices] IUserService userService)
        {
            var userIdStr = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdStr, out int userId))
                return Unauthorized();

            try
            {
                await userService.ChangePassword(userId, request);
                return Ok("Password has been changed successfully");
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [EnableRateLimiting("fixed")]
        public override async Task<PagedResult<UserResponse>> GetList(UserSearchObject search)
        {
            var result= await _service.GetPaged(search);
            return result;
        }


        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public override async Task<IActionResult> Delete(int id)
        {
            var result = await _service.Delete(id); 

            if (!result)
                return NotFound();

            return NoContent();
        }


    }

}
