using Microsoft.EntityFrameworkCore;
using S4.SocialMediaAzure.Entities.Models.Context;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace S4.SocialMediaAzure.DataAccess.Base
{
    /// <summary>
    /// Base generic repository class for encapsulation of DBContext functionalities
    /// </summary>
    /// <typeparam name = "T" ></ typeparam >
    public class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        /// <summary>
        /// Database context
        /// </summary>
        protected SocialMediaContext context;

        /// <summary>
        /// Sets the context to the provided parameter item
        /// </summary>
        /// <param name="context"></param>
        public RepositoryBase(SocialMediaContext context)
        {
            Context = context;
        }

        /// <summary>
        /// Initializes the context
        /// </summary>
        public RepositoryBase()
        {
            context = new SocialMediaContext();
        }

        /// <summary>
        /// The database context
        /// </summary>
        public virtual SocialMediaContext Context
        {
            get { return context; }
            set { context = value; }
        }

        /// <summary>
        /// Adds an item
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public virtual async Task AddAsync(T t)
        {
            context.Set<T>().Add(t);
            await context.SaveChangesAsync();
        }

        /// <summary>
        /// Gets an item by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task<T> GetByIdAsync(int? id)
        {
            return await context.Set<T>().FindAsync(id);
        }

        /// <summary>
        /// Gets all the items
        /// </summary>
        /// <returns></returns>
        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await context.Set<T>().ToListAsync();
        }

        /// <summary>
        /// Updates an item
        /// </summary>
        /// <param name="t"></param>
        public virtual async Task UpdateAsync(T t)
        {
            context.Set<T>().Update(t);
            await context.SaveChangesAsync();
        }

        /// <summary>
        /// Deletes an item
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public virtual async Task DeleteAsync(T t)
        {
            context.Set<T>().Remove(t);
            await context.SaveChangesAsync();
        }

        public virtual async Task<bool> Exists(int id)
        {
            return await context.Set<T>().FindAsync(id) != null;
        }
    }
}
