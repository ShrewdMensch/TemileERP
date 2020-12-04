using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Web.API
{
    public class AppUsersController : BaseController
    {
        [HttpGet("checkusername")]
        public async Task<ActionResult<bool>> IsUserNameTaken(string username)
        {
            var user = await UserManager.FindByNameAsync(username);

            if (user == null)
            {
                return Ok();
            }
            return NotFound();
        }
    }
}