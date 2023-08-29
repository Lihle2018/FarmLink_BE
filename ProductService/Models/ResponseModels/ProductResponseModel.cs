using FarmLink.Shared.ResponseModel;

namespace ProductService.Models.ResponseModels
{
    public class ProductResponseModel:ResponseBase<Product>
    {
        public ProductResponseModel(Product product, string message=null)
        {
            Data = product;
            Message = message;
        }
    }
}
