using FarmLink.Shared.ResponseModel;

namespace FarmLink.CustomerService.Models.ResponseModels
{
    public class CustomerResponseModel:ResponseBase<Customer>
    {
        public CustomerResponseModel(Customer customer,string message=null)
        {
            Data = customer;
            Message = message;
        }
    }
}
