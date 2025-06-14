using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using Repositories.Pagging;
using Repositories.SampleMethodRepo;
using Services.DTO;
namespace Services.SampleMethodSS
{
    public class SampleMethodS : ISampleMethodS
    {
        private readonly ISampleMethodRepository _sampleMethodRepository;
        public SampleMethodS(ISampleMethodRepository sampleMethodRepository)
        {
            _sampleMethodRepository = sampleMethodRepository;
        }
        public async Task<SampleMethod?> GetByIdAsync(int id)
        {
            return await _sampleMethodRepository.GetByIdAsync(id);
        }
        public async Task<PaginatedList<SampleMethod>> GetAll(int pageNumber, int pageSize)
        {
            IQueryable<SampleMethod> methods = _sampleMethodRepository.GetAll().AsQueryable();
            return await PaginatedList<SampleMethod>.CreateAsync(methods, pageNumber, pageSize);
        }

        public async Task<IEnumerable<SampleMethod>> GetAllAsync()
        {
            return await _sampleMethodRepository.GetAsync();
        }
        public async Task<SampleMethod> AddAsync(AddSampleMethodDTO addSampleMethodDto)
        {

            if (addSampleMethodDto == null)
            {
                throw new ArgumentNullException(nameof(addSampleMethodDto), "methods data is required.");
            }

          

            var addMethod = new SampleMethod
            {
                Name = addSampleMethodDto.Name,
                Description = addSampleMethodDto.Description,
                
            };
             await _sampleMethodRepository.AddAsync(addMethod);
            return addMethod;
        }

        public async Task<SampleMethod?> UpdateAsync(int id, UpdateSampleMethodDTO updateSampleMethodDto)
        {
            if (updateSampleMethodDto == null)
            {
                throw new ArgumentNullException(nameof(updateSampleMethodDto), "methods data is required.");
            }

            var upmethods = await _sampleMethodRepository.GetByIdAsync(id);
            if (upmethods == null)
            {
                throw new KeyNotFoundException($"methods with ID {id} not found.");
            }
            upmethods.Name = updateSampleMethodDto.Name;
            upmethods.Description = updateSampleMethodDto.Description; // Nullable field
            upmethods.UpdateAt = DateTime.UtcNow; // Update timestamp
            await _sampleMethodRepository.UpdateAsync(upmethods);
            return upmethods;
        }
      
        public async Task DeleteAsync(int id)
        {
            var Dmethods = await _sampleMethodRepository.GetByIdAsync(id);
            if (Dmethods == null)
            {
                throw new KeyNotFoundException($"methods with ID {id} not found.");
            }
            await _sampleMethodRepository.DeleteAsync(id);
        }
    }
}
