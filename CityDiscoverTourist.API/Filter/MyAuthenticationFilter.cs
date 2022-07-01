using Hangfire.Dashboard;

namespace CityDiscoverTourist.API.Filter;

/// <summary>
///
/// </summary>
public class MyAuthenticationFilter : IDashboardAuthorizationFilter
{
    /// <summary>
    ///
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public bool Authorize(DashboardContext context)
    {
        if (context.GetHttpContext().User.IsInRole("Admin"))
        {
            return true;
        }

        return false;
    }
}