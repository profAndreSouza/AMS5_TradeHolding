public interface IUserRepository
{
     void Add(User user);
    User? GetById(Guid id);
    List<User> GetAll();
    void Update(User user);
    void Delete(Guid id);
    User? GetByEmail(string email);
}