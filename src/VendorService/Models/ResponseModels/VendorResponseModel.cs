using FarmLink.Shared.ResponseModel;

namespace VendorService.Models.ResponseModels
{
    public class VendorResponseModel:ResponseBase<Vendor>
    {
        public VendorResponseModel(Vendor vendor,string message=null,bool error=false)
        {
            Data = vendor;
            Message = message;
            Error = error;
        }
    }
}
