namespace AssetManagement.Core.DTOs
{
    public class TokenDto
    {
        public string Token { get; set; } = string.Empty;
        public UserDto User { get; set; } = null!;
    }
}