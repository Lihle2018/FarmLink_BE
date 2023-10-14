
namespace FarmLink.Shared.ResponseModel
{
    public abstract class ResponseBase<T>
    {
      public  T Data { get; set; }
      public string Message { get; set; }
      public bool Error { get; set; }
    }
}
