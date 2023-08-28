using FarmLink.IndentityService.Models;
using MongoDB.Driver;

namespace IdentityService.Data.Interfaces
{
    public interface IFarmLinkContext
    {
        IMongoCollection<User> Users { get; }
    }
}
