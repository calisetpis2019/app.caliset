using Abp.Domain.Repositories;
using Abp.Domain.Services;
using Abp.UI;
using App.Caliset.Models.Operations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Caliset.Models.Comments
{
    public class CommentManager : DomainService, ICommentManager
    {
        private readonly IRepository<Comment> _repositoryComments;
        private readonly IRepository<Operation> _repositoryOperations;
        public CommentManager(IRepository<Comment> repositoryComment, IRepository<Operation> repositoryOperations)
        {
            _repositoryComments = repositoryComment;
            _repositoryOperations = repositoryOperations;
        }

        public async Task<Comment> Create(Comment entity)
        {
            var Comment = _repositoryComments.FirstOrDefault(x => x.Id == entity.Id);
            if (Comment != null)
            {
                throw new UserFriendlyException("Error", "Ya existe comentario.");
            }
            else if(_repositoryOperations.FirstOrDefault(entity.OperationId).OperationStateId == 3)
            {
                throw new UserFriendlyException("Error", "La operacion ya esta finalizada.");
            }
            else
            {
                return await _repositoryComments.InsertAsync(entity);
            }
        }

        public async Task<Comment> CreateFO(Comment entity)
        {
            var Comment = _repositoryComments.FirstOrDefault(x => x.Id == entity.Id);
            if (Comment != null)
            {
                throw new UserFriendlyException("Error", "Ya existe comentario.");
            }
            else
            {
                return await _repositoryComments.InsertAsync(entity);
            }
        }

        public void Delete(int id)
        {
            var Comment = _repositoryComments.FirstOrDefault(x => x.Id == id);
            if (Comment == null)
            {
                throw new UserFriendlyException("Error", "No existe comentario.");
            }
            else
            {
                _repositoryComments.Delete(Comment);
            }
        }

        public IEnumerable<Comment> GetAll()
        {
            return _repositoryComments.GetAll().Include(asset => asset.CreatorUser);
        }

        public Comment GetCommentById(int id)
        {
            return _repositoryComments.Get(id);
        }

        public void Update(Comment entity)
        {
            _repositoryComments.Update(entity);
        }

        public IEnumerable<Comment> GetCommentsByOperation(int operationId)
        {
            var comments = from Comment in this.GetAll()
                              where Comment.OperationId == operationId
                              select Comment;

            return comments;
        }
    }
}
