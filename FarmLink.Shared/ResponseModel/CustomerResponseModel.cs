using FarmLink.Shared.Entiities;

namespace FarmLink.Shared.ResponseModel
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
