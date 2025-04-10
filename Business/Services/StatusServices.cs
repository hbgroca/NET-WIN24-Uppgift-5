using Business.Dtos;
using Business.Factories;
using Business.Interfaces;
using Business.Models;
using Data.Interfaces;
using System.Diagnostics;

namespace Business.Services
{
    public class StatusServices(IClientStatusRepository clientStatusRepository, IMemberStatusRepository memberStatusRepository) : IStatusServices
    {
        private readonly IClientStatusRepository _clientStatusRepository = clientStatusRepository;
        private readonly IMemberStatusRepository _memberStatusRepository = memberStatusRepository;

        // Do not repeat yourself (DRY) principle does not apply here. It's more KISS (Keep It Simple, Stupid) principle. :)

        // Clients
        public async Task<IEnumerable<ClientStatusModel>> GetClientStatuses()
        {
            var results = await _clientStatusRepository.GetAllAsync();
            var listWithStatusModels = results.Select(StatusFactory.Create);
            return listWithStatusModels ?? [];
        }

        public async Task<bool> AddStatus(ClientStatusFormModel status)
        {
            if (status == null)
            {
                Debug.WriteLine("Status is missing");
                return false;
            }
            try
            {
                // Begin transaction
                await _clientStatusRepository.BeginTransactionAsync();

                // Check if the status already exists
                var existingStatus = await _clientStatusRepository.ExistInDb(desc => desc.Description == status.Description);
                if (existingStatus)
                {
                    Debug.WriteLine("Status already exists");
                    return false;
                }

                var entity = StatusFactory.Create(status);
                await _clientStatusRepository.CreateAsync(entity);
                // Save & Commit the transaction
                var result = await _clientStatusRepository.SaveAsync();
                await _clientStatusRepository.CommitTransactionAsync();

                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error adding status: " + ex.Message);
                await _clientStatusRepository.RollbackTransactionAsync();
                return false;
            }
        }

        public async Task<bool> RemoveClientStatus(int id)
        {
            try
            {
                var entity = await _clientStatusRepository.GetOneAsync(x => x.Id == id);
                if (entity == null)
                {
                    Debug.WriteLine("Status not found");
                    return false;
                }

                var result = _clientStatusRepository.Delete(entity);
                if (!result)
                {
                    Debug.WriteLine("Failed to remove status");
                    return false;
                }
                await _clientStatusRepository.SaveAsync();
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error removing status: " + ex.Message);
                return false;
            }
        }




        // Members
        public async Task<IEnumerable<MemberStatusModel>> GetMemberStatuses()
        {
            var results = await _memberStatusRepository.GetAllAsync();
            var listWithStatusModels = results.Select(StatusFactory.Create);
            return listWithStatusModels ?? [];
        }

        public async Task<bool> AddStatus(MemberStatusFormModel status)
        {
            if (status == null)
            {
                Debug.WriteLine("Status is missing");
                return false;
            }
            try
            {
                // Begin transaction
                await _memberStatusRepository.BeginTransactionAsync();

                // Check if the status already exists
                var existingStatus = await _memberStatusRepository.ExistInDb(desc => desc.Description == status.Description);
                if (existingStatus)
                {
                    Debug.WriteLine("Status already exists");
                    return false;
                }

                var entity = StatusFactory.Create(status);
                await _memberStatusRepository.CreateAsync(entity);
                // Save & Commit the transaction
                var result = await _memberStatusRepository.SaveAsync();
                await _memberStatusRepository.CommitTransactionAsync();

                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error adding status: " + ex.Message);
                await _memberStatusRepository.RollbackTransactionAsync();
                return false;
            }
        }

        public async Task<bool> RemoveMemberStatus(int id)
        {
            try
            {
                var entity = await _memberStatusRepository.GetOneAsync(x => x.Id == id);
                if (entity == null)
                {
                    Debug.WriteLine("Status not found");
                    return false;
                }

                var result = _memberStatusRepository.Delete(entity);
                if (!result)
                {
                    Debug.WriteLine("Failed to remove status");
                    return false;
                }
                await _memberStatusRepository.SaveAsync();
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error removing status: " + ex.Message);
                return false;
            }
        }
    }
}
