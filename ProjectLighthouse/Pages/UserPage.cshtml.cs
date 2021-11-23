#nullable enable
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LBPUnion.ProjectLighthouse.Pages.Layouts;
using LBPUnion.ProjectLighthouse.Types;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LBPUnion.ProjectLighthouse.Pages
{
    public class UserPage : BaseLayout
    {
        public UserPage(Database database) : base(database)
        {}

        public User? ProfileUser;
        public List<Photo>? Photos;
        public bool IsProfileUserHearted;

        public async Task<IActionResult> OnGet([FromRoute] int userId)
        {
            this.ProfileUser = await this.Database.Users.FirstOrDefaultAsync(u => u.UserId == userId);
            if (this.ProfileUser == null) return this.NotFound();

            this.Photos = await this.Database.Photos.OrderByDescending(p => p.Timestamp).Where(p => p.CreatorId == userId).Take(5).ToListAsync();

            if (this.User != null)
            {

                this.IsProfileUserHearted = (await this.Database.HeartedProfiles.FirstOrDefaultAsync
                                                (u => u.UserId == this.User.UserId && u.HeartedUserId == this.ProfileUser.UserId)) !=
                                            null;
            }

            return this.Page();
        }
    }
}