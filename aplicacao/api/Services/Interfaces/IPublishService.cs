using api.Domain.Classes;
using api.Domain.DTOs;

namespace api.Services.Interfaces
{
    public interface IPublishService
    {
        public Task Upload(Guid userId,PublishDTO pdf);
    }
}
