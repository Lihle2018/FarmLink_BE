
namespace FarmLink.Shared.ResponseModel
{
    public class ResponseBase<T>
    {
      public  T Data { get; set; }
      public string Message { get; set; }
    }
}
