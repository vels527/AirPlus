using System.Linq;
using CoreAirPlus.Data;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using CoreAirPlus.Entities;

namespace CoreAirPlus.Services
{
    public class DbReadService:IDbReadService
    {
        private DataDBContext _db;
        public DbReadService(DataDBContext db)
        {
            _db = db;
        }

        public IQueryable<TEntity> Get<TEntity>() where TEntity : class
        {
            return _db.Set<TEntity>();
        }
        
        public TEntity Get<TEntity>(int id, bool includeRelatedEntities = false) where TEntity : class
        {
            var record = _db.Set<TEntity>().Find(new object[] { id });
            if(record != null && includeRelatedEntities)
            {
                var entities = GetEntityNames<TEntity>();
                foreach (var entity in entities.collections)
                    _db.Entry(record).Collection(entity).Load();
                foreach (var entity in entities.references)
                    _db.Entry(record).Reference(entity).Load();
            }
            return record;
        }

        private (IEnumerable<string> collections, IEnumerable<string> references) GetEntityNames<TEntity>() where TEntity : class
        {
            var dbsets = typeof(DataDBContext).GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(z => z.PropertyType.Name.Contains("DbSet")).Select(z => z.Name);
            // Get the names of all the properties (tables) in the generic 
            // type T that is represented by a DbSet 
            var properties = typeof(TEntity) .GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var collections = properties .Where(l => dbsets.Contains(l.Name)) .Select(s => s.Name);
            var classes = properties .Where(c => dbsets.Contains(c.Name + "s")) .Select(s => s.Name);
            return (collections: collections, references: classes);
        }
        public IEnumerable<TEntity> GetWithIncludes<TEntity>() where TEntity : class
        {
            var entityNames = GetEntityNames<TEntity>();
            var dbset = _db.Set<TEntity>();
            var entities = entityNames.collections.Union(entityNames.references);
            foreach (var entity in entities)
                _db.Set<TEntity>().Include(entity).Load();
            return dbset;
        }
        public TEntity Get<TEntity>(string userId, int id) where TEntity : class
        {
            var record = _db.Set<TEntity>().Find(new object[] { userId, id });
            return record;
        }
        public SelectList GetSelectList<TEntity>(string valueField, string textField) where TEntity : class
        {
            var items = Get<TEntity>();
            return new SelectList(items, valueField, textField);
        }

        public bool SaveReservation(Reservation reservation)
        {
            _db.reservations.Update(reservation);
            _db.SaveChanges();
            return true;
        }
        public bool SaveHost(Host host)
        {
            _db.hosts.Update(host);
            _db.SaveChanges();
            return true;
        }
    }
}