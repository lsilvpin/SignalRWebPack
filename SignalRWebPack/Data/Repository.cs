using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;

namespace SignalRWebPack.Data
{
    public abstract class Repository<T>
        where T : class, IIdentifiable
    {
        private readonly ILogger<Repository<T>> logger;
        private readonly EfContext db;
        private readonly DbSet<T> set;

        protected Repository(EfContext db, 
            ILogger<Repository<T>> logger)
        {
            this.logger = logger;
            this.db = db;
            set = db.Set<T>();
        }

        public int Create(T entity)
        {
            set.Add(entity);
            return entity.Id;
        }

        public List<int> Create(List<T> entities)
        {
            List<int> ids = new List<int>();
            entities.ForEach(e => ids.Add(Create(e)));
            return ids;
        }

        public T FirstOrDefault(Func<T, bool> lambda)
        {
            return set.FirstOrDefault(lambda);
        }

        public List<T> Where(Func<T, bool> lambda)
        {
            return set.Where(lambda).ToList();
        }

        public List<OUT> Select<OUT>(Func<T, OUT> lambda)
        {
            return set.Select(lambda).ToList();
        }

        public List<T> OrderBy<OUT>(Func<T, OUT> lambda)
        {
            return set.OrderBy(lambda).ToList();
        }

        public void Update(T entity)
        {
            if (entity.Id < 0)
            {
                logger.LogError("Entity not found");
            }
            else
            {
                set.Update(entity);
            }
        }

        public void Update(List<T> entities)
        {
            entities.ForEach(e => Update(e));
        }

        public void Delete(int entityId)
        {
            if (entityId < 0)
            {
                logger.LogError("EntityNotFound");
            }
            else
            {
                T entity = set.Find(entityId);
                set.Remove(entity);
            }
        }

        public void Delete(List<int> entitiesIds)
        {
            entitiesIds.ForEach(id => Delete(id));
        }

        public void Commit()
        {
            using IDbContextTransaction trans = db.Database.BeginTransaction();
            try
            {
                db.SaveChanges();
                trans.Commit();
            }
            catch (DbException ex)
            {
                logger.LogError(ex.Message);
                throw;
            }
        }
    }
}
