using SharedLibrary.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Core.Services
{
    public interface IGenericService< TDto>
        where TDto : class
    {
        Task<ResponseDto<TDto>> GetByIdAsync(int id);
        Task<ResponseDto<List<TDto>>> GetAllAsync();
        Task<ResponseDto<TDto>> AddAsync(TDto entiy);
        ResponseDto<TDto> Remove(TDto entiy);
        ResponseDto<TDto> Update(TDto entiy);
    }
}
