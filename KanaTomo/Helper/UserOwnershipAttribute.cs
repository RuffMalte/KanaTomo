using System.Security.Claims;
using KanaTomo.API.APIUser;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

public class UserOwnershipAttribute : TypeFilterAttribute
{
    public UserOwnershipAttribute() : base(typeof(UserOwnershipFilter))
    {
    }
}

public class UserOwnershipFilter : IAsyncActionFilter
{
    private readonly IApiUserService _userService;

    public UserOwnershipFilter(IApiUserService userService)
    {
        _userService = userService;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var userId = context.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        var userGuid = Guid.Parse(userId);

        if (context.ActionArguments.TryGetValue("id", out var idObj) && idObj is Guid id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null || user.Id != userGuid)
            {
                context.Result = new ForbidResult();
                return;
            }
        }

        await next();
    }
}