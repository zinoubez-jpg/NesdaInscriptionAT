using Anade.Data.Abstractions;
using Anade.Domain.Core;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;


namespace Anade.Business.Core
{
    public class GenericBusinessService<T, TKey> : IBusinessService<T, TKey> where T : class, IEntity<TKey>
    {
        protected readonly IUnitOfWork _unitOfWork;
        protected readonly IRepository<T, TKey> _repository;

        public GenericBusinessService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _repository = _unitOfWork.GetRepository<T, TKey>();
        }
        public BusinessResult Add(T entity)
        {
            BusinessResult businessResult;

            try
            {
                OnAdding(entity);
                _repository.Add(entity);
                if (_unitOfWork.SaveChanges() == 0)
                    throw new DataNotUpdatedException("Opération non enregsitrée !");
                OnAdded(entity);
                businessResult = BusinessResult.Success;
            }
            catch (DataNotUpdatedException ex)
            {
                businessResult = new BusinessResult();
                businessResult.Messages.Add(new MessageResult { Message = ex.Message, MessageType = MessageType.Warning });
            }
            catch(BusinessException ex)
            {
                businessResult = new BusinessResult();
                businessResult.Messages.Add(new MessageResult { Message = ex.Message, MessageType = MessageType.Warning });
            }
            catch(Exception)
            {
                businessResult = new BusinessResult();
                businessResult.Messages.Add(new MessageResult { Message = "Un erreur s'est produite réessayez, si cela persiste contactez votre administrateur ", MessageType = MessageType.Warning });
            }

            return businessResult;
        }

        protected virtual void OnAdded(T entity)
        {
            
        }

        protected virtual void OnAdding(T entity)
        {
            
        }

        public BusinessResult Delete(T entity)
        {
            BusinessResult businessResult;

            try
            {
                OnDeleting(entity);
                _repository.Delete(entity);
                if (_unitOfWork.SaveChanges() == 0)
                    throw new DataNotUpdatedException("Opération non enregsitrée !");
                OnDeleted(entity);
                businessResult = BusinessResult.Success;
            }
            catch (DataNotUpdatedException ex)
            {
                businessResult = new BusinessResult();
                businessResult.Messages.Add(new MessageResult { Message = ex.Message, MessageType = MessageType.Warning });
            }
            catch (BusinessException ex)
            {
                businessResult = new BusinessResult();
                businessResult.Messages.Add(new MessageResult { Message = ex.Message, MessageType = MessageType.Warning });
            }
            catch (Exception)
            {
                businessResult = new BusinessResult();
                businessResult.Messages.Add(new MessageResult { Message = "Un erreur s'est produite réessayez, si cela persiste contactez votre administrateur ", MessageType = MessageType.Warning });
            }

            return businessResult;
        }

        protected virtual void OnDeleted(T entity)
        {
            
        }

        protected virtual void OnDeleting(T entity)
        {
            
        }

        protected virtual void OnUpdated(T entity)
        {
            
        }

        protected virtual void OnUpdating(T entity)
        {
            
        }

        public List<T> GetAll(params Expression<Func<T, object>>[] navigationPropertiesToLoad)
        {
            return _repository.GetAll(navigationPropertiesToLoad);
        }

        public List<T> GetAllFiltered(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] navigationPropertiesToLoad)
        {
            return _repository.GetAllFiltered(predicate, navigationPropertiesToLoad);
        }

        public PagedResult<T> GetAllFilteredPaged(Expression<Func<T, bool>> predicate, string orderBy="Id", int startRowIndex = 0, int maxRows = 10, params Expression<Func<T, object>>[] navigationPropertiesToLoad)
        {
            return _repository.GetAllFilteredPaged(predicate, orderBy, startRowIndex, maxRows, navigationPropertiesToLoad);
        }

        public PagedResult<T> GetAllPaged(string orderBy="Id", int startRowIndex = 0, int maxRows = 10, params Expression<Func<T, object>>[] navigationPropertiesToLoad)
        {
            return _repository.GetAllPaged(orderBy, startRowIndex, maxRows, navigationPropertiesToLoad);
        }

        public T GetById(TKey id, params Expression<Func<T, object>>[] navigationPropertiesToLoad)
        {
            return _repository.GetSingle(x => x.Id.Equals(id), navigationPropertiesToLoad);
        }

        public T GetSingle(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] navigationPropertiesToLoad)
        {
            return _repository.GetSingle(predicate, navigationPropertiesToLoad);
        }

        public BusinessResult Update(T entity)
        {
            BusinessResult businessResult;
            try
            {
                OnUpdating(entity);
                _repository.Update(entity);
                if (_unitOfWork.SaveChanges() == 0)
                    throw new DataNotUpdatedException("Opération non enregsitrée !");
                OnUpdated(entity);
                businessResult = BusinessResult.Success;
            }
            catch (DataNotUpdatedException ex)
            {
                businessResult = new BusinessResult();
                businessResult.Messages.Add(new MessageResult { Message = ex.Message, MessageType = MessageType.Warning });
            }
            catch (BusinessException ex)
            {
                businessResult = new BusinessResult();
                businessResult.Messages.Add(new MessageResult { Message = ex.Message, MessageType = MessageType.Warning });
            }
            catch (Exception)
            {
                businessResult = new BusinessResult();
                businessResult.Messages.Add(new MessageResult { Message = "Un erreur s'est produite réessayez, si cela persiste contactez votre administrateur ", MessageType = MessageType.Warning });
            }

            return businessResult;
        }

        public int Count(Expression<Func<T, bool>> predicate = null)
        {
            if (predicate != null)
                return _repository.Count(predicate);

            return _repository.Count();
        }
        public virtual Expression<Func<T, object>>[] GetDefaultLoadProperties()
        {
            return Array.Empty<Expression<Func<T, object>>>();
        }
    }
}
