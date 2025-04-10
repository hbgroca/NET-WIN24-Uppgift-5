using Business.Dtos;
using Business.Models;
using Data.Entities;

namespace Business.Factories
{
    public static class StatusFactory
    {
        // Clients
        public static ClientStatusEntity Create(ClientStatusModel status)
        {
            return new ClientStatusEntity
            {
                Id = status.Id,
                Description = status.Description
            };
        }
        public static ClientStatusEntity Create(ClientStatusFormModel status)
        {
            return new ClientStatusEntity
            {
                Description = status.Description
            };
        }
        public static ClientStatusModel Create(ClientStatusEntity status)
        {
            return new ClientStatusModel
            {
                Id = status.Id,
                Description = status.Description
            };
        }


        // Projects
        public static MemberStatusEntity Create(MemberStatusModel status)
        {
            return new MemberStatusEntity
            {
                Id = status.Id,
                Description = status.Description
            };
        }
        public static MemberStatusEntity Create(MemberStatusFormModel status)
        {
            return new MemberStatusEntity
            {
                Description = status.Description
            };
        }
        public static MemberStatusModel Create(MemberStatusEntity status)
        {
            return new MemberStatusModel
            {
                Id = status.Id,
                Description = status.Description
            };
        }
        
    }
}
