public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

     public UserDTO RegisterUser(UserDTO userDto)
    {
        var user = new User 
        {
            Name = userDto.Name, 
            Email = userDto.Email, 
            Phone = userDto.Phone,
            Address = userDto.Address,
            Password = userDto.Password,
            Photo = userDto.Photo
        };
        _userRepository.Add(user);
        return userDto;
    }

    public UserDTO? GetUserDetails(int id)
    {
        var user = _userRepository.GetById(id);
        return user != null ? new UserDTO 
        { 
            Name = user.Name, 
            Email = user.Email,
            Phone = user.Phone,
            Address = user.Address,
            Password = user.Password,
            Photo = user.Photo
        } : null;
    }

    public List<UserDTO> GetAllUsers()
    {
        return _userRepository.GetAll().Select(user => new UserDTO
        {
            Name = user.Name,
            Email = user.Email,
            Phone = user.Phone,
            Address = user.Address,
            Password = user.Password,
            Photo = user.Photo
        }).ToList();
    }

    public UserDTO? UpdateUser(int id, UserDTO userDto)
    {
        var user = _userRepository.GetById(id);
        if (user == null) return null;
        
        user.Name = userDto.Name;
        user.Email = userDto.Email;
        user.Phone = userDto.Phone;
        user.Address = userDto.Address;
        user.Password = userDto.Password;
        user.Photo = userDto.Photo;
        
        _userRepository.Update(user);
        return userDto;
    }

    public bool DeleteUser(int id)
    {
        var user = _userRepository.GetById(id);
        if (user == null) return false;
        _userRepository.Delete(id);
        return true;
    }

}