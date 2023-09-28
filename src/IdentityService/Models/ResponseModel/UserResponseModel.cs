using FarmLink.Shared.ResponseModel;

namespace FarmLink.IndentityService.Models.RequestModels
{
    public class UserResponseModel:ResponseBase<User>
    {
        public UserResponseModel(User user,string message=null,bool error=false)
        {
           
            if(user!=null)
            {
                user.Password=string.Empty;
                user.Otp=string.Empty;
                user.Salt=string.Empty;
                Data = user;
            }   
            Message = message;
            Error = error;
        }
    }
}
