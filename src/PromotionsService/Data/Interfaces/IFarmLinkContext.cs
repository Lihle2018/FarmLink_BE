using MongoDB.Driver;
using PromotionsService.Models;

namespace PromotionsService.Data.Interfaces
{
    public interface IFarmLinkContext
    {
        IMongoCollection<Promotion> Promotions { get; }
    }
}
