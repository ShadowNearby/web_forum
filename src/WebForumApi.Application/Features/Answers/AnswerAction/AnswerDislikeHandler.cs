﻿using Ardalis.Result;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebForumApi.Application.Common;
using WebForumApi.Domain.Auth.Interfaces;
using WebForumApi.Domain.Entities;

namespace WebForumApi.Application.Features.Answers.AnswerAction;

public class AnswerDislikeHandler : IRequestHandler<AnswerLikeRequest, Result>
{
    private readonly IContext _context;
    private readonly ISession _session;

    public AnswerDislikeHandler(IContext context, ISession session)
    {
        _context = context;
        _session = session;
    }

    public async Task<Result> Handle(AnswerLikeRequest request, CancellationToken cancellationToken)
    {
        // select the target answer
        Answer answer = _context.Answers.First(a => a.Id == new Guid(request.AnswerId));
        // select the user
        User user = _context.Users.First(u => u.Id == _session.UserId);
        UserAnswerAction? action = user.UserAnswerActions.Find(u => u.AnswerId == answer.Id);
        if (action == null)
        {
            action = new UserAnswerAction
            {
                AnswerId = answer.Id,
                UserId = user.Id,
                IsLike = false,
                IsDislike = true,
                IsStar = false
            };
            user.UserAnswerActions.Add(action);
            // update the answer
            answer.DislikeCount += 1;
        }
        else
        {
            // update the answer
            answer.DislikeCount += action.IsDislike ? -1 : 1;

            // update the action
            action.IsDislike = !action.IsDislike;
        }

        await _context.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}