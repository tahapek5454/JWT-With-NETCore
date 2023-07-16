using AuthServer.Core.Models;
using AuthServer.Core.Repositories;
using AuthServer.Core.Services;
using AuthServer.Core.UnitOfWork;
using SharedLibrary.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Service.Services
{
    public class GenericService<TEntiy,TDto> : IGenericService<TDto> where TDto : class where TEntiy : class
    {
        private readonly IUnitOfWork _unitOfWorkunitOfWork;
        private readonly IGenericRepository<TEntiy> _genericRepository;


        public GenericService(IUnitOfWork unitOfWorkunitOfWork, IGenericRepository<TEntiy> genericRepository)
        {
            _unitOfWorkunitOfWork = unitOfWorkunitOfWork;
            _genericRepository = genericRepository;
        }

        public async Task<ResponseDto<TDto>> AddAsync(TDto entiy)
        {
            TEntiy entity = ObjectMapper.Mapper.Map<TEntiy>(entiy);

            await _genericRepository.AddAsync(entity);

            await _unitOfWorkunitOfWork.CommitAsync();

            TDto dto = ObjectMapper.Mapper.Map<TDto>(entity);

            return ResponseDto<TDto>.Sucess(dto, 200);
            
        }

        public  ResponseDto<List<TDto>> GetAllAsync()
        {
            List<TDto> dtos = ObjectMapper.Mapper.Map<List<TDto>>(_genericRepository.GetAllAsync().ToList());

            return ResponseDto<List<TDto>>.Sucess(dtos, 200);
        }

        public async Task<ResponseDto<TDto>> GetByIdAsync(int id)
        {
            TEntiy entity = await _genericRepository.GetByIdAsync(id);

            if (entity == null)
                return ResponseDto<TDto>.Fail("id not found", true, 404);

            TDto dto = ObjectMapper.Mapper.Map<TDto>(entity);
            return ResponseDto<TDto>.Sucess(dto, 200);
        }

        public async Task<ResponseDto<TDto>> Remove(int id)
        {
            TEntiy entiy = await _genericRepository.GetByIdAsync(id);

            if(entiy == null)
                return ResponseDto<TDto>.Fail("id not found", true, 404);

            _genericRepository.Remove(entiy);
            await _unitOfWorkunitOfWork.CommitAsync();
            return ResponseDto<TDto>.Sucess(204);
        }

        public ResponseDto<TDto> Update(TDto entiy)
        {
            TEntiy entity = ObjectMapper.Mapper.Map<TEntiy>(entiy);
            TEntiy newenttiy = _genericRepository.Update(entity);
            _unitOfWorkunitOfWork.Commit();

            TDto dto = ObjectMapper.Mapper.Map<TDto>(newenttiy);
            return ResponseDto<TDto>.Sucess(dto, 200);
        }
    }
}
