namespace API.DTOs;

public class UserDto
{

    public string DisplayName { get; set; }
    public string Token { get; set; }
    public string Username { get; set; }
    public AvatarDto Avatar { get; set; } = null;
}

public class AvatarDto
{
    public string AvatarName { get; set; }
    public string Url { get; set; }
}