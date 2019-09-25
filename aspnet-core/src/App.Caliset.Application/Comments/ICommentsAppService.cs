using Abp.Application.Services;
using App.Caliset.Comments.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace App.Caliset.Comments
{
    public interface ICommentsAppService: IApplicationService
    {
        IEnumerable<GetCommentOutput> GetAll();
        Task Create(CreateCommentInput input);
        void Delete(DeleteCommentInput input);
        void Update(UpdateCommentInput input);
        GetCommentOutput GetCommentById(GetCommentInput input);
        IEnumerable<GetCommentOutput> GetCommentsByOperation(int operationId);
    }
}
