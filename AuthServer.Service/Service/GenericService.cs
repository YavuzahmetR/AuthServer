using AuthServer.Core.Repositories;
using AuthServer.Core.Services;
using AuthServer.Core.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using SharedLib.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Service.Service
{
    public class GenericService<T, TDto>(IUnitOfWork unitOfWork, IGenericRepository<T> genericRepository) : IGenericService<T, TDto> where T : class where TDto : class
    {
        public async Task<Response<TDto>> AddAsync(TDto Entity)
        {
            var newEntity = ObjectMapper.Mapper.Map<T>(Entity);

            await genericRepository.AddAsync(newEntity);

            await unitOfWork.CommitAsync();

            var newDto = ObjectMapper.Mapper.Map<TDto>(newEntity);

            return Response<TDto>.Success(newDto, 200);
        }

        public async Task<Response<NoDataDto>> Delete(int id)
        {
            var isDataExists = await genericRepository.GetByIdAsync(id);

            if (isDataExists == null)
            {
                return Response<NoDataDto>.Fail("Product Not Found", 404, true);
            }
            genericRepository.Delete(isDataExists);
            await unitOfWork.CommitAsync();
            return Response<NoDataDto>.Success(204);
        }

        public async Task<Response<IEnumerable<TDto>>> GetAllAsync()
        {
            var entities = ObjectMapper.Mapper.Map<List<TDto>>(await genericRepository.GetAllAsync());
            return Response<IEnumerable<TDto>>.Success(entities, 200);
        }

        public async Task<Response<TDto>> GetByIdAsync(int id)
        {
            var entity = await genericRepository.GetByIdAsync(id);

            if (entity == null)
            {
                return Response<TDto>.Fail("Product Not Found", 404, true);
            }

            return Response<TDto>.Success(ObjectMapper.Mapper.Map<TDto>(entity), 200);
        }

        public async Task<Response<NoDataDto>> Update(TDto Entity, int id)
        {
            var isDataExists = await genericRepository.GetByIdAsync(id);

            if (isDataExists == null)
            {
                return Response<NoDataDto>.Fail("Product Not Found", 404, true);
            }

            var updateData = ObjectMapper.Mapper.Map<T>(Entity);

            genericRepository.Update(updateData);
            await unitOfWork.CommitAsync();
            return Response<NoDataDto>.Success(204);
        }

        public async Task<Response<IEnumerable<TDto>>> Where(Expression<Func<T, bool>> predicate)
        {
            var list = genericRepository.Where(predicate);

            return Response<IEnumerable<TDto>>.Success(ObjectMapper.Mapper.Map<IEnumerable<TDto>>
                (await list.ToListAsync()), 200);
        }
    }
}
