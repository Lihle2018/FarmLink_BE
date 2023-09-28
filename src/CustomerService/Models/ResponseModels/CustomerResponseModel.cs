using FarmLink.Shared.ResponseModel;

namespace FarmLink.CustomerService.Models.ResponseModels
{
    public class CustomerResponseModel:ResponseBase<Customer>
    {
        public CustomerResponseModel(Customer customer,string message=null,bool error=true)
        {
            Data = customer;
            Message = message;
            Error = error;
        }
    }
}
