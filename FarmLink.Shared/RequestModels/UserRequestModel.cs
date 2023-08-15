using FarmLink.Shared.Entiities;

namespace FarmLink.Shared.RequestModels
{
    public class UserRequestModel:User
    {
        public bool isNew { get { return string.IsNullOrEmpty(Id); } }
    }
}
