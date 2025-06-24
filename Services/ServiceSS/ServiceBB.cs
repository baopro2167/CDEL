using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using Repositories.Pagging;
using Repositories.SampleMethodRepo;
using Repositories.ServiceRepo;
using Services.DTO;
using Repositories.ServiceSMRepo;
namespace Services.ServiceSS
{
    public class ServiceBB : IServiceBB
    {
        private readonly IServiceRepository _serviceRepository;
        private readonly ISampleMethodRepository _sampleMethodRepository;
        private readonly IServiceSMRepository _serviceSampleMethodRepository;
        public ServiceBB(IServiceRepository serviceRepository, ISampleMethodRepository sampleMethodRepository, IServiceSMRepository serviceSampleMethodRepository)
        {
            _serviceRepository = serviceRepository;
            _sampleMethodRepository = sampleMethodRepository;
            _serviceSampleMethodRepository = serviceSampleMethodRepository;
        }
        public async Task<ServiceGetDTO?> GetByIdAsync(int id)
        {
            var service = await _serviceRepository.GetByIdAsync(id);

            if (service == null)
            {
                return null; // Nếu không tìm thấy service, trả về null
            }

            // Lấy SampleMethods liên quan đến service
            var sampleMethods = await _sampleMethodRepository.GetByServiceIdAsync(service.Id);

            // Chuyển đổi Service và SampleMethods sang DTO
            var result = new ServiceGetDTO
            {
                ServiceId = service.Id,
                Name = service.Name,
                Description = service.Description,
                SampleMethods = sampleMethods.Select(sm => new SampleMethodDTO
                {
                    Name = sm.Name,
                    Description = sm.Description
                }).ToList()
            };

            return result;
        }
        public async Task<IEnumerable<ServiceGetPriceDTO>> GetPricingAsync()
        {
            var services = await _serviceRepository.GetPricingAsync();

            var result = services.Select(s => new ServiceGetPriceDTO
            {
                ServiceId = s.Id,
                Name = s.Name,
                Price=s.Price
            });

            return result;
        }


        public async Task<IEnumerable<Service>> GetAllAsync()
        {
            return await _serviceRepository.GetAllAsync();
        }
        public async Task<PaginatedList<Service>> GetAll(int pageNumber, int pageSize)
        {
            IQueryable<Service> services = _serviceRepository.GetAll().AsQueryable();
            return await PaginatedList<Service>.CreateAsync(services, pageNumber, pageSize);
        }
        public async Task<ServiceGetNameDTO> AddAsync(AddServiceBDTO addServiceBDto)
        {
            if (addServiceBDto == null)
            {
                throw new ArgumentNullException(nameof(addServiceBDto), "Service data is required.");
            }
            var service = new Service
            {
                Name = addServiceBDto.Name,
                Description = addServiceBDto.Description,
                Price = addServiceBDto.Price,
                Type = addServiceBDto.Type,
                CreateAt = DateTime.UtcNow,
                UpdateAt = DateTime.UtcNow

            };
            await _serviceRepository.AddAsync(service);
            if (addServiceBDto.SampleMethodIds != null && addServiceBDto.SampleMethodIds.Any())
            {
                foreach (var sampleMethodId in addServiceBDto.SampleMethodIds)
                {
                    var serviceSampleMethod = new ServiceSampleMethod
                    {
                        ServiceId = service.Id,
                        SampleMethodId = sampleMethodId
                    };
                    await _serviceSampleMethodRepository.AddAsync(serviceSampleMethod);  // Thêm vào bảng ServiceSampleMethod
                }
            }

            // Tạo DTO để trả về thông tin dịch vụ vừa tạo
            var result = new ServiceGetNameDTO
            {
                ServiceId = service.Id,
                Name = service.Name
            };

            return result;
        }

        public async Task<ServiceGetNameDTO> UpdateAsync(int id, UpdateServiceBDTO updateServiceBDto)
        {
            if (updateServiceBDto == null)
            {
                throw new ArgumentNullException(nameof(updateServiceBDto), "Service data is required.");
            }

            // Lấy dịch vụ từ cơ sở dữ liệu
            var service = await _serviceRepository.GetByIdAsync(id);
            if (service == null)
            {
                throw new KeyNotFoundException($"Service with ID {id} not found.");
            }

            // Cập nhật các thuộc tính của service
            service.Name = updateServiceBDto.Name;
            service.Description = updateServiceBDto.Description;
            service.Price = updateServiceBDto.Price;
            service.Type = updateServiceBDto.Type;  // Cập nhật service type
            service.UpdateAt = DateTime.UtcNow;

            // Cập nhật mối quan hệ giữa Service và SampleMethod
            // Xóa các SampleMethods cũ liên kết với Service này
            await _serviceSampleMethodRepository.DeleteAsync(service.Id);

            // Thêm các SampleMethods mới
            if (updateServiceBDto.SampleMethodIds != null && updateServiceBDto.SampleMethodIds.Any())
            {
                foreach (var sampleMethodId in updateServiceBDto.SampleMethodIds)
                {
                    var serviceSampleMethod = new ServiceSampleMethod
                    {
                        ServiceId = service.Id,
                        SampleMethodId = sampleMethodId
                    };
                    await _serviceSampleMethodRepository.AddAsync(serviceSampleMethod);  // Thêm vào bảng ServiceSampleMethod
                }
            }

            // Cập nhật dịch vụ vào cơ sở dữ liệu
            await _serviceRepository.UpdateAsync(service);

            // Trả về thông tin dịch vụ đã cập nhật
            var result = new ServiceGetNameDTO
            {
                ServiceId = service.Id,
                Name = service.Name
            };

            return result;
        }
        public async Task DeleteAsync(int id)
        {
            var Dservice = await _serviceRepository.GetByIdAsync(id);
            if (Dservice == null)
            {
                throw new KeyNotFoundException($"Service with ID {id} not found.");
            }
            await _serviceRepository.DeleteAsync(id);

        }
    }
}
