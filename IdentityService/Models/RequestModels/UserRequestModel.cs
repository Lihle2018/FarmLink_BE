
namespace FarmLink.IndentityService.Models.RequestModels
{
    public class UserRequestModel:User
    {
        public bool isNew { get { return string.IsNullOrEmpty(Id); } }
    }
}
