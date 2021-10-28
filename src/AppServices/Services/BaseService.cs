using System;
using System.Linq;
using System.Linq.Expressions;
using AppServices.Repositories;
using Domain.Entities;
using Domain.Framework;

namespace AppServices.Services
{
    public abstract class BaseService<T, U> : IBaseService<T, U> where T : BaseEntity where U : IBaseRepository<T>
    {
        protected U Repository { get; private set; }
        public BaseService(U mainRepository)
        {
            Repository = mainRepository;
        }
        protected abstract TaskResult<T> ValidateOnCreate(T entity);
        protected abstract TaskResult ValidateOnDelete(T entity);
        protected abstract TaskResult<T> ValidateOnUpdate(T entity);
        public virtual TaskResult<T> Create(T entity)
        {
            var taskResult = ValidateOnCreate(entity);
            if (taskResult.Success)
            {
                try
                {
                    Repository.Insert(entity);
                    Repository.CommitChanges();
                    taskResult.AddMessage("Item created successfully");
                    taskResult.Data = entity;
                }
                catch (Exception ex)
                {
                    taskResult.AddErrorMessage(ex.Message);
                    if (ex.InnerException != null)
                        taskResult.AddErrorMessage(ex.InnerException.Message);
                }
            }
            return taskResult;
        }
        public virtual TaskResult<T> Update(T entity)
        {
            var taskResult = ValidateOnUpdate(entity);
            if (taskResult.Success)
            {
                try
                {
                    Repository.DetachAllEntries();
                    Repository.Update(entity);
                    Repository.CommitChanges();
                    taskResult.AddMessage("Item updated successfully");
                    taskResult.Data = entity;
                }
                catch (Exception ex)
                {
                    taskResult.AddErrorMessage(ex.Message);
                    if (ex.InnerException != null)
                        taskResult.AddErrorMessage(ex.InnerException.Message);
                }
            }
            return taskResult;
        }
        public virtual TaskResult Delete(int id)
        {
            var entity = Repository.Get(x => x.IsActive && x.Id == id).FirstOrDefault();
            var taskResult = ValidateOnDelete(entity);
            if (taskResult.Success)
            {
                try
                {
                    Repository.SoftDelete(entity.Id);
                    Repository.CommitChanges();
                    taskResult.AddMessage("Item deleted successfully");
                }
                catch (Exception ex)
                {
                    taskResult.AddErrorMessage(ex.Message);
                    if (ex.InnerException != null)
                        taskResult.AddErrorMessage(ex.InnerException.Message);
                }
            }
            return taskResult;
        }
        public IQueryable<T> DataSet()
        {
            return Repository.GetAll();
        }
        public IQueryable<T> DataSet(Expression<Func<T, bool>> where, string includeProperties = "")
        {
            return Repository.Get(where, includeProperties);
        }
        public IQueryable<T> DataSet(Expression<Func<T, bool>> where, params Expression<Func<T, object>>[] include)
        {
            return Repository.Get(where, include);
        }
        public IQueryable<T> DataSet(params Expression<Func<T, object>>[] include)
        {
            return Repository.Get(include);
        }
    }
    public interface IBaseService<T, U> where T : BaseEntity where U : IBaseRepository<T>
    {
        IQueryable<T> DataSet();
        IQueryable<T> DataSet(Expression<Func<T, bool>> where, string includeProperties = "");
        IQueryable<T> DataSet(Expression<Func<T, bool>> where, params Expression<Func<T, object>>[] include);
        IQueryable<T> DataSet(params Expression<Func<T, object>>[] include);
        TaskResult<T> Create(T entity);
        TaskResult<T> Update(T entity);
        TaskResult Delete(int id);
    }
}
