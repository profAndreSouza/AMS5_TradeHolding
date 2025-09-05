using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost]
    public IActionResult RegisterUser(UserDTO userDto)
    {
        var result = _userService.RegisterUser(userDto);
        return Ok(result);
    }

    [Authorize]
    [HttpGet("{id}")]
    public IActionResult GetUserDetails(Guid id)
    {
        var user = _userService.GetUserDetails(id);
        return user != null ? Ok(user) : NotFound();
    }

    [Authorize]
    [HttpGet]
    public IActionResult GetAllUsers()
    {
        var users = _userService.GetAllUsers();
        return Ok(users);
    }

    [Authorize]
    [HttpPut("{id}")]
    public IActionResult UpdateUser(Guid id, UserDTO userDto)
    {
        var updatedUser = _userService.UpdateUser(id, userDto);
        return updatedUser != null ? Ok(updatedUser) : NotFound();
    }

    [Authorize]
    [HttpDelete("{id}")]
    public IActionResult DeleteUser(Guid id)
    {
        var result = _userService.DeleteUser(id);
        return result ? NoContent() : NotFound();
    }
}