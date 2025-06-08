using Model;
using Repositories.KitRepo;
using Repositories.Pagging;
using Services.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Services.KitS
{
    public class KitService : IKitService
    {
        private readonly IKitRepository _kitRepository;

        public KitService(IKitRepository kitRepository)
        {
            _kitRepository = kitRepository;
        }

        public async Task<Kit> AddKitAsync(AddKitDTO createKitDto)
        {

            if (createKitDto == null)
            {
                throw new ArgumentNullException(nameof(createKitDto), "kit data is required.");
            }

            if (createKitDto.UpdateAt <= createKitDto.CreateAt)
            {
                throw new ArgumentException("EndDate must be after StartDate.", nameof(createKitDto.UpdateAt));
            }

            var kit = new Kit
            {
                Name = createKitDto.Name,
                Description = createKitDto.Description,
                CreateAt = createKitDto.CreateAt.Kind == DateTimeKind.Unspecified
         ? DateTime.SpecifyKind(createKitDto.CreateAt, DateTimeKind.Utc)
         : createKitDto.CreateAt.ToUniversalTime(),
                UpdateAt = createKitDto.UpdateAt.Kind == DateTimeKind.Unspecified
         ? DateTime.SpecifyKind(createKitDto.UpdateAt, DateTimeKind.Utc)
         : createKitDto.UpdateAt.ToUniversalTime(),
            };
            await _kitRepository.AddAsync(kit);
           
           

            return kit;


        }

        public async Task DeleteKitAsync(int id)
        {
            var kit = await _kitRepository.GetByIdAsync(id);
            if (kit == null)
            {
                throw new KeyNotFoundException($"Kit with ID {id} not found.");
            }
            await _kitRepository.DeleteAsync(id);
        }

        public async Task<PaginatedList<Kit>> GetAll(int pageNumber, int pageSize)
        {
            IQueryable<Kit> kit = _kitRepository.GetAll().AsQueryable();
            return await PaginatedList<Kit>.CreateAsync(kit, pageNumber, pageSize);
        }

        public async Task<IEnumerable<Kit>> GetAllKitAsync()
        {
            return await _kitRepository.GetkitAsync();
        }

        public async Task<Kit?> GetByIdAsync(int id)
        {
            return await _kitRepository.GetByIdAsync(id);
        }

        public async Task<Kit?> UpdateKitAsync(int id, UpdateKitDTO updateKitDto)
        {
            if (updateKitDto == null)
            {
                throw new ArgumentNullException(nameof(updateKitDto), "Kit data is required.");
            }

            var kit = await _kitRepository.GetByIdAsync(id);
            if (kit == null)
            {
                throw new KeyNotFoundException($"Kit with ID {id} not found.");
            }

            kit.Name = updateKitDto.Name;
            kit.Description = updateKitDto.Description; // Nullable field
            kit.UpdateAt = updateKitDto.UpdateAt.Kind == DateTimeKind.Unspecified
                ? DateTime.SpecifyKind(updateKitDto.UpdateAt, DateTimeKind.Utc)
                : updateKitDto.UpdateAt.ToUniversalTime();

            await _kitRepository.UpdateAsync(kit);

            return kit;
        }
    }
}
