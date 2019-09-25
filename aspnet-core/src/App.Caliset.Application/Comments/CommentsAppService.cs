using Abp.Application.Services;
using App.Caliset.Comments.Dto;
using App.Caliset.Models.Comments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Caliset.Comments
{
    public class CommentsAppService : ApplicationService, ICommentsAppService
    {

        private readonly CommentManager _commentManager;
        public CommentsAppService(CommentManager commentManager)
        {
            _commentManager = commentManager;
        }

        public async Task Create(CreateCommentInput input)
        {
            var Comment = ObjectMapper.Map<Comment>(input);
            await _commentManager.Create(Comment);
        }

        public void Delete(DeleteCommentInput input)
        {
            _commentManager.Delete(input.Id);
        }

        public IEnumerable<GetCommentOutput> GetAll()
        {
            var getAll = _commentManager.GetAll().ToList();
            List<GetCommentOutput> output = ObjectMapper.Map<List<GetCommentOutput>>(getAll);
            return output;
        }

        public GetCommentOutput GetCommentById(GetCommentInput input)
        {

            var getComment = _commentManager.GetCommentById(input.Id);
            GetCommentOutput output = ObjectMapper.Map<GetCommentOutput>(getComment);
            return output;
        }

        public IEnumerable<GetCommentOutput> GetCommentsByOperation(int operationId)
        {
            List<GetCommentOutput> output = ObjectMapper.Map<List<GetCommentOutput>>(_commentManager.GetCommentsByOperation(operationId));

            return output;
        }

        public void Update(UpdateCommentInput input)
        {
            var Comment = _commentManager.GetCommentById(input.Id);
            ObjectMapper.Map(input, Comment);
            _commentManager.Update(Comment);
        }
    }
}
