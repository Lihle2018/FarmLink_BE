namespace FarmLink.IndentityService.Models.RequestModels
{
    public class VerifyOtpRequestModel
    {
        public string UserId { get; set; }
        public string Otp { get; set; }
    }
}
