using BackEnd.Contracts;
using BackEnd.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace BackEnd.DataManager
{
    public class PackageTypDataManager : IPackageType
    {
        private readonly ApplicationDbContext dbContext;
        public PackageTypDataManager(ApplicationDbContext _dbContext)
        {
            dbContext = _dbContext;
        }
        public bool Delete(PackageType entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            dbContext.PackageTypes.Remove(entity);
            return this.dbContext.SaveChanges() > 0;
        }

        public void Delete(List<PackageType> entities)
        {
            foreach (var entity in entities)
            {
                Delete(entity);
            }
        }

        public IQueryable<PackageType> Find(Func<PackageType, bool> predicate)
        {
            if (predicate == null)
                throw new ArgumentNullException("predicate");
            return dbContext.PackageTypes.Where(predicate).AsQueryable();
        }

        public IQueryable<PackageType> Get()
        {
            return dbContext.PackageTypes.AsQueryable();
        }

        public PackageType Get(object id)
        {
           return dbContext.PackageTypes.Find(id);

        }

        public bool Insert(PackageType entity)
        {
            dbContext.PackageTypes.Add(entity);
            return dbContext.SaveChanges() > 0;
        }

        public bool Update(PackageType entity)
        {
            if (entity==null)
            {
                throw new ArgumentNullException("Entity");
            }
            dbContext.Entry(entity).State = EntityState.Modified;
            return dbContext.SaveChanges() > 0;
        }
    }
}