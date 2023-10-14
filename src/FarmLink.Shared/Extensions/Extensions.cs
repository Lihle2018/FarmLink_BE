namespace IdentityService.Extensions
{
    public static class Extensions
    {
        public static string GenerateOtp(this string otp)
        {
            var rnd = new Random();
            otp = rnd.Next(10000, 100000).ToString();
            return otp;
        }
    }
}
