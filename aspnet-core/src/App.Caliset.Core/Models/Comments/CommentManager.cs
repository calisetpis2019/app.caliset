using Abp.Domain.Repositories;
using Abp.Domain.Services;
using Abp.UI;
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
        public CommentManager(IRepository<Comment> repositoryComment)
        {
            _repositoryComments = repositoryComment;
        }

        public async Task<Comment> Create(Comment entity)
        {
            var Sample = _repositoryComments.FirstOrDefault(x => x.Id == entity.Id);
            if (Sample != null)
            {
                throw new UserFriendlyException("Already Exist");
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
                throw new UserFriendlyException("No Data Found");
            }
            else
            {
                _repositoryComments.Delete(Comment);
            }
        }

        public IEnumerable<Comment> GetAll()
        {
            return _repositoryComments.GetAll();
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
