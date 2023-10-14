using FarmLink.Shared.ResponseModel;

namespace ProductService.Models.ResponseModels
{
    public class ProductResponseModel:ResponseBase<Product>
    {
        public ProductResponseModel(Product product, string message=null,bool error=false)
        {
            Data = product;
            Message = message;
            Error = error;
        }
    }
}
