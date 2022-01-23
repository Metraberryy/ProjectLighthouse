using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using LBPUnion.ProjectLighthouse.Helpers;
using LBPUnion.ProjectLighthouse.Pages.Layouts;
using LBPUnion.ProjectLighthouse.Types;
using LBPUnion.ProjectLighthouse.Types.Settings;
using Microsoft.AspNetCore.Mvc;

namespace LBPUnion.ProjectLighthouse.Pages;

public class UsersPage : BaseLayout
{
    public int PageAmount;

    public int PageNumber;

    public int UserCount;

    public List<User> Users;

    public UsersPage([NotNull] Database database) : base(database)
    {}

    public async Task<IActionResult> OnGet([FromRoute] int pageNumber)
    {
        this.UserCount = await StatisticsHelper.UserCount();

        this.PageNumber = pageNumber;
        this.PageAmount = (int)Math.Ceiling((double)this.UserCount / ServerStatics.PageSize);

        if (this.PageNumber < 0 || this.PageNumber >= this.PageAmount) return this.Redirect($"/users/{Math.Clamp(this.PageNumber, 0, this.PageAmount - 1)}");

        this.Users = await this.Database.Users.Where
                (u => !u.Banned)
            .Skip(pageNumber * ServerStatics.PageSize)
            .Take(ServerStatics.PageSize)
            .ToAsyncEnumerable()
            .OrderBy(u => u.Status)
            .ThenByDescending(u => u.UserId)
            .ToListAsync();

        return this.Page();
    }
}