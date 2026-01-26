
using Blog.Application.Common.Interfaces.Repositories;
using Blog.Application.Queries.Notification.GetListNotification;
using Blog.Application.Queries.Posts.GetApprovalTotal;
using Blog.Domain.Entities;
using BlogApi.Application.Models;
using BlogApi.Domain.Common;
using BlogApi.Domain.Entities;
using BlogApi.Domain.Enums;
using BlogApi.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static BlogApi.Domain.Enums.EntityEnum;

namespace Blog.Infrastructure.Respository
{
    public class NotificationRespository(IAppDbContext context) : INotificationRespository
    {

        public async Task<Result<PagedResult<GetListNotificationDto>>> GetPaginatedNotificationAsync(int PageNumber, int PageSze, Expression<Func<Notification, bool>>? filter = null, CancellationToken cancellationToken = default)
        {

            IQueryable<Notification> entities = context.Notifications.AsNoTracking();

            if (filter != null)
            {
                entities = entities.Where(filter);
            }
            var notifcount = entities.Count();

            var selected = await entities
                 .OrderByDescending(s => s.CreatedAt)
                 .Skip((PageNumber - 1) * PageSze)
                 .Take(PageSze)
                 .Select(s => new GetListNotificationDto
                 {
                     Id = s.Id,
                     Type = s.Type,
                     PostId = s.PostId,
                     UserName = s.User!.UserName,
                     CreatedAt = s.CreatedAt,
                     IsRead = s.IsRead
                 }).ToListAsync();

            if (!selected.Any())
                return Result<PagedResult<GetListNotificationDto>>.NoContent();

            var dto = new PagedResult<GetListNotificationDto>
            {
                Items = selected,
                TotalCount = notifcount,
                PageNumber = PageNumber,
                PageSize = PageSze
            };

            return Result<PagedResult<GetListNotificationDto>>.Success(dto);
        }

        public async Task<Result<UnreadDto>> GetunreadAsync(Guid? UserId, Expression<Func<Post, bool>>? filter = null, CancellationToken cancellationToken = default)
        {
            IQueryable<Post> entity = context.Posts.AsNoTracking();

            if (filter != null)
            {
                entity = entity
                    .Include(s=> s.Notifications)
                    .Where(filter);
            }
      
            var postCount = await entity
              .Include(s => s.Notifications)
              .Where(s => s.Status == Status.Pending)
              .CountAsync(cancellationToken);


            var approval = new[] { EntityEnum.Type.PostApproval, EntityEnum.Type.PostDecline };
            var notifcount = await context.Notifications
                .AsNoTracking()
                .Where(s => s.RecipientUserId == UserId && s.IsRead == false && !approval.Contains(s.Type))
                .CountAsync(cancellationToken);

            var dto = new UnreadDto
            {
                PendingCount = postCount,
                NotificationCount = notifcount
            };

            return Result<UnreadDto>.Success(dto);
        }



    }
}
