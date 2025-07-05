using Repositories.Pagging;
using Services.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repositories.KitDeliveryRepo;
using Model;
using Repositories.ExRequestRepo;
using Repositories.StaffRepo;

namespace Services.KitDeliverySS
{
    public class KitDeliveryS : IKitDeliveryS
    {

        private readonly IKitDeliveryRepository _kitDeliveryRepository;
        private readonly IExRequestRepository _exReqRepo;
        private readonly IStaffRepository _staffRepo;
        public KitDeliveryS(IKitDeliveryRepository kitDeliveryRepository, IStaffRepository staffRepository
            , IExRequestRepository exRequestRepository)
        {
            _kitDeliveryRepository = kitDeliveryRepository;
            _exReqRepo = exRequestRepository;
            _staffRepo = staffRepository;
        }

      





        public async Task<KitDeliveryResponseDTO?> AddAsync(AddKitDeliveryDTO addKitDeliveryDto)
        {
            if (addKitDeliveryDto == null)
            {
                throw new ArgumentNullException(nameof(addKitDeliveryDto), "Kit delivery data is required.");
            }

           

            var delivery = new KitDelivery
            {
                RequestId = addKitDeliveryDto.RequestId,
                KitId = addKitDeliveryDto.KitId,
                KitType = addKitDeliveryDto.Kittype,
                StatusId = "Pending",
                SentAt = DateTime.UtcNow
            };
         

            await _kitDeliveryRepository.AddAsync(delivery);
          

            return new KitDeliveryResponseDTO
            {
                KitDeliveryId = delivery.Id,
                RequestId = delivery.RequestId,

                KitType = delivery.KitType,
                Status = delivery.StatusId,
                SentAt = delivery.SentAt
            };
        }

       

        public async Task DeleteAsync(int id)
        {
            var delivery = await _kitDeliveryRepository.GetByIdAsync(id);
            if (delivery == null)
            {
                throw new KeyNotFoundException($"KitDelivery with ID {id} not found.");
            }
            await _kitDeliveryRepository.DeleteAsync(id);

        }

        public async Task<PaginatedList<KitDelivery>> GetAll(int pageNumber, int pageSize)
        {
            IQueryable<KitDelivery> kitDeliveries = _kitDeliveryRepository.GetAll().AsQueryable();
            return await PaginatedList<KitDelivery>.CreateAsync(kitDeliveries, pageNumber, pageSize);
        }

        public async Task<IEnumerable<KitDelivery>> GetAllKitAsync()
        {
            return await _kitDeliveryRepository.GetkitAsync();
        }

        public async Task<KitDelivery?> GetByIdAsync(int id)
        {
            return await _kitDeliveryRepository.GetByIdAsync(id);
        }

        public async Task<KitDeliveryStatusResponseDTO> MarkAsSentAsync(int kitDeliveryId)
        {
            var delivery = await _kitDeliveryRepository.GetByIdAsync(kitDeliveryId);
            if (delivery == null)
                throw new KeyNotFoundException($"KitDelivery with ID {kitDeliveryId} not found.");

            // 2. Chỉ cho phép Pending → Sent
            if (delivery.StatusId != "Pending")
                throw new InvalidOperationException("Chỉ có thể đánh dấu Sent khi đang ở trạng thái Pending.");

            // 3. Cập nhật
            delivery.StatusId = "Sent";
            delivery.ReceivedAt = DateTime.UtcNow;
            await _kitDeliveryRepository.UpdateAsync(delivery);

            // 4. Trả về DTO
            return new KitDeliveryStatusResponseDTO
            {
                KitDeliveryId = delivery.Id,
                Status = delivery.StatusId,
                ReceivedAt = delivery.ReceivedAt!.Value
            };
        }
        

        public async Task<KitDelivery?> UpdateAsync(int id, UpdateKitDeliveryDTO updateKitDeliveryDto)
        {

            if (updateKitDeliveryDto == null)
            {
                throw new ArgumentNullException(nameof(updateKitDeliveryDto), "kitDeliveries data is required.");
            }

            var kitDeliveries = await _kitDeliveryRepository.GetByIdAsync(id);
            if (kitDeliveries == null)
            {
                throw new KeyNotFoundException($"kitDeliveries with ID {id} not found.");
            }
          
            kitDeliveries.StatusId = updateKitDeliveryDto.StatusId;
            kitDeliveries.ReceivedAt = DateTime.UtcNow;

            await _kitDeliveryRepository.UpdateAsync(kitDeliveries);

            return kitDeliveries;

        }
        public async Task<UpdateKitDeliverySSResponseDTO> AcknowledgeAsync(int kitDeliveryId, UpdateKitDeliverySSDTO dto)
        {
            if (dto == null || string.IsNullOrWhiteSpace(dto.Status))
                throw new ArgumentException("Status is required.", nameof(dto));

            // 1. Lấy entity
            var delivery = await _kitDeliveryRepository.GetByIdAsync(kitDeliveryId);
            if (delivery == null)
                throw new KeyNotFoundException($"KitDelivery with ID {kitDeliveryId} not found.");

            // 2. Chỉ cho phép chuyển từ "Sent" → "Received" hoặc "Returned"
            if (delivery.StatusId != "Sent")
                throw new InvalidOperationException("Chỉ được xác nhận khi trạng thái hiện tại là Sent.");
            if (dto.Status != "Received" && dto.Status != "Returned")
                throw new InvalidOperationException("Status phải là 'Received' hoặc 'Returned'.");

            // 3. Cập nhật trạng thái và UpdatedAt
            delivery.StatusId = dto.Status;
            delivery.ReceivedAt = DateTime.UtcNow;
            await _kitDeliveryRepository.UpdateAsync(delivery);

            // 4. Build và trả về response
            return new UpdateKitDeliverySSResponseDTO
            {
                KitDeliveryId = delivery.Id,
                Status = delivery.StatusId,
                ReceivedAt = delivery.ReceivedAt!.Value
            };
        }


    }
}


