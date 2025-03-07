## Para adicionar hash na senha do usuário
dotnet add package BCrypt.Net-Next


# Em service, ao mapear UserDTO para User, use a senha criptografada
var hashedPassword = BCrypt.Net.BCrypt.HashPassword(userDto.Password);