public interface IUserService
{
    UserDTO RegisterUser(UserDTO userDto);
    UserDTO? GetUserDetails(Guid id);
    List<UserDTO> GetAllUsers();
    UserDTO? UpdateUser(Guid id, UserDTO userDto);
    bool DeleteUser(Guid id);
    UserDTO? ValidateUser(string email, string password);
}