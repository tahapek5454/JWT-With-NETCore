using SharedLibrary.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Core.Services
{
    public interface IGenericService<TEntiy, TDto>
        where TDto : class
        where TEntiy: class
    {
        Task<ResponseDto<TDto>> GetByIdAsync(int id);
        Task<ResponseDto<List<TDto>>> GetAllAsync();
        Task<ResponseDto<TDto>> AddAsync(TEntiy entiy);
        ResponseDto<TDto> Remove(TEntiy entiy);
        ResponseDto<TDto> Update(TEntiy entiy);
    }
}
