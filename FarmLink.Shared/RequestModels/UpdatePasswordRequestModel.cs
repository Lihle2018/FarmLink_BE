namespace FarmLink.Shared.RequestModels
{
    public class UpdatePasswordRequest
    {
        public string Otp { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
