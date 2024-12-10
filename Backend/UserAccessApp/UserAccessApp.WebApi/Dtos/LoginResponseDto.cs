namespace UserAccessApp.WebApi.Dtos
{
    public class LoginResponseDto
    {
        public string AccessToken { get; set; }
        public int UserId { get; set; }
        public bool IsActive { get; set; }
        public string UserName { get; set; }
    }
}
