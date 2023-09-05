using FarmLink.Shared.ResponseModel;

namespace ProductService.Models.ResponseModels
{
    public class CategoryResponseModel:ResponseBase<Category>
    {
        public CategoryResponseModel(Category category,string message=null)
        {
            Data = category;
            Message = message;
        }
    }
}
