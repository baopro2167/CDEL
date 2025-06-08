using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using Repositories.Pagging;
using Repositories.ServiceRepo;
using Services.DTO;
namespace Services.ServiceSS
{
    public class ServiceBB : IServiceBB
    {
        private readonly IServiceRepository _serviceRepository;
        public ServiceBB(IServiceRepository serviceRepository)
        {
            _serviceRepository = serviceRepository;
        }
        public async Task<Service?> GetByIdAsync(int id)
        {
            return await _serviceRepository.GetByIdAsync(id);
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
        public async Task<Service> AddAsync(AddServiceBDTO addServiceBDto)
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
                Type = addServiceBDto.Type

            };
            await _serviceRepository.AddAsync(service);
            return service;
        }

        public async Task<Service?> UpdateAsync(int id, UpdateServiceBDTO updateServiceBDto)
        {
            if (updateServiceBDto == null)
            {
                throw new ArgumentNullException(nameof(updateServiceBDto), "Service data is required.");
            }
            var service = await _serviceRepository.GetByIdAsync(id);
            if (service == null)
            {
                throw new KeyNotFoundException($"Service with ID {id} not found.");
            }
            service.Name = updateServiceBDto.Name;
            service.Description = updateServiceBDto.Description;
            service.Price = updateServiceBDto.Price;
            return await _serviceRepository.UpdateAsync(service);
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
