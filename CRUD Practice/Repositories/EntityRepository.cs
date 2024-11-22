using CRUD_Practice.Data;
using Microsoft.EntityFrameworkCore;

namespace CRUD_Practice.Repositories
{
    public class EntityRepository<T> : IEntityRepository<T> where T : class
    {
        private readonly AppDbContext _appDbContext;

        public EntityRepository(AppDbContext context)
        {
            _appDbContext = context;
        }

        public void Add(T entity)
        {
            _appDbContext.Set<T>().Add(entity);
            _appDbContext.SaveChanges();
        }

        public void Delete(int id)
        {
            var entity = GetById(id);
            if (entity != null)
            {
                _appDbContext.Remove(entity);
                _appDbContext.SaveChanges();
            } 
        }

        public IEnumerable<T> GetAll()
        {
            return _appDbContext.Set<T>().ToList();
        }

        public T GetById(int id)
        {
            return _appDbContext.Set<T>().Find(id);
        }

        public void Update(T entity)
        {
            _appDbContext.Set<T>().Update(entity);
            _appDbContext.SaveChanges();
        }
    }
}
