using FarmLink.Shared.ResponseModel;

namespace ProductService.Models.ResponseModels
{
    public class CategoryResponseModel:ResponseBase<Category>
    {
        public CategoryResponseModel(Category category,string message=null,bool error=false)
        {
            Data = category;
            Message = message;
            Error = error;
        }
    }
}
