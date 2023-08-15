using FarmLink.Shared.Entiities;

namespace FarmLink.Shared.ResponseModel
{
    public class UserResponseModel:ResponseBase<User>
    {
        public UserResponseModel(User user,string message=null)
        {
           
            if(user!=null)
            {
                user.Password=string.Empty;
                user.Otp=string.Empty;
                user.Salt=string.Empty;
                Data = user;
            }   
            Message = message;
        }
    }
}
