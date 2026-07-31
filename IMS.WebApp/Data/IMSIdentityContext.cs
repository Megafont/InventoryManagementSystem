using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IMS.WebApp.Data
{
	public class IMSIdentityContext(DbContextOptions<IMSIdentityContext> options)
		: IdentityDbContext<IMS.WebApp.Data.ApplicationUser>(options)
	{
	}
}
