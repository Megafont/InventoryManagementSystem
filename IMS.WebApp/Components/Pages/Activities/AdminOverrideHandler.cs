using Microsoft.AspNetCore.Authorization;

namespace IMS.WebApp.Components.Pages.Activities
{
	/// <summary>
	/// This class is an AuthorizationHandler that allows an "Admin" user to access all pages.
	/// </summary>
	public class AdminOverrideHandler : IAuthorizationHandler
	{
		public Task HandleAsync(AuthorizationHandlerContext context)
		{
			// Check if the user is in the Admin role (or has an Admin claim)
			if (context.User.HasClaim("Department","Administration"))
			{
				// Crucial step: Mark EVERY pending requirement for the current policy as succeeded
				foreach (var requirement in context.PendingRequirements.ToList())
				{
					context.Succeed(requirement);
				}
			}

			return Task.CompletedTask;
		}
	}
}
