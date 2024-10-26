using SharedLib.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Core.Services
{
    public interface IGenericService<T,TDto>where T : class where TDto : class
    {
        Task<Response<IEnumerable<TDto>>> GetAllAsync();
        Task<Response<IEnumerable<TDto>>> Where(Expression<Func<T, bool>> predicate);
        Task<Response<TDto>> GetByIdAsync(int id);
        Task<Response<TDto>> AddAsync(TDto Entity);
        Task<Response<NoDataDto>> Update(TDto Entity,int id);
        Task<Response<NoDataDto>> Delete(int id);
    }
}
