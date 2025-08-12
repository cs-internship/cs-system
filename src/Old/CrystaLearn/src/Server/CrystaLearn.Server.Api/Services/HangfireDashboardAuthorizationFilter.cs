using Hangfire.Annotations;
using Hangfire.Dashboard;

namespace CrystaLearn.Server.Api.Services;

public class HangfireDashboardAuthorizationFilter : IDashboardAuthorizationFilter
{
    public bool Authorize([NotNull] DashboardContext context)
    {
        return context.GetHttpContext().User.IsInRole(AppRoles.SUPER_ADMIN);
    }
}
