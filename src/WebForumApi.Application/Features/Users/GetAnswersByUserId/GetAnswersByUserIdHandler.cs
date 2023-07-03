using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebForumApi.Application.Common;
using WebForumApi.Application.Common.Responses;
using WebForumApi.Application.Features.Questions.Dto;
using WebForumApi.Domain.Auth.Interfaces;

namespace WebForumApi.Application.Features.Users.GetAnswersByUserId;

public class GetAnswersByUserIdHandler : IRequestHandler<GetAnswersByUserIdRequest, PaginatedList<AnswerCardDto>>
{
    private readonly IContext _context;
    private readonly ISession _session;

    public GetAnswersByUserIdHandler(IContext context, ISession session)
    {
        _context = context;
        _session = session;
    }
    public async Task<PaginatedList<AnswerCardDto>> Handle(GetAnswersByUserIdRequest request, CancellationToken cancellationToken)
    {
        return await _context.Answers.Where(a => a.CreateUserId == request.Id).Select(a => new AnswerCardDto
        {
            Id = a.Id.ToString(), Content = a.Content, VoteNumber = a.LikeCount
        }).OrderByDescending(x => x.VoteNumber).ToPaginatedListAsync(request.CurrentPage, request.PageSize);
    }
}