using MediatR;
using System;
using WebForumApi.Application.Common.Requests;
using WebForumApi.Application.Common.Responses;
using WebForumApi.Application.Features.Questions.Dto;

namespace WebForumApi.Application.Features.Users.GetQuestionsStaredByUserId;

public record GetQuestionsStaredByUserIdRequest : PaginatedRequest, IRequest<PaginatedList<QuestionCardDto>>
{
    public Guid Id { get; init; }
}