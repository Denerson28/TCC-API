using api.Domain.Classes;
using api.Domain.DTOs;

namespace api.Services.Interfaces
{
    public interface IPublishService
    {
        public Task<PublishResponseDTO> Upload(Guid userId,PublishRequestDTO publish);
        public Task Delete(Guid userId, Guid publishId);
        public Task Update(Guid publishId, PublishRequestDTO publish);
        public Task<PublishResponseDTO> Get(Guid userId);
    }
}
