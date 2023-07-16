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
        ResponseDto<List<TDto>> GetAllAsync();
        Task<ResponseDto<TDto>> AddAsync(TDto entiy);
        Task<ResponseDto<TDto>> Remove(int id);
        ResponseDto<TDto> Update(TDto entiy);
    }
}
