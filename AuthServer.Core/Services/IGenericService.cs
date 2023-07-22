using SharedLibrary.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Core.Services
{
    public interface IGenericService<Tentiy, TDto>
        where TDto : class
    {
        Task<ResponseDto<TDto>> GetByIdAsync(int id);
        ResponseDto<List<TDto>> GetAll();
        Task<ResponseDto<TDto>> AddAsync(TDto entiy);
        Task<ResponseDto<TDto>> RemoveAsync(int id);
        ResponseDto<TDto> Update(TDto entiy);
    }
}
