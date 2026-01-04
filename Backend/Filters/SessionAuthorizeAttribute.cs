using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

public class SessionAuthorizeAttribute : ActionFilterAttribute
{
    private readonly string _role;

    public SessionAuthorizeAttribute(string role = "")
    {
        _role = role;
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var httpContext = context.HttpContext;
        var userRole = httpContext.Session.GetString("UserRole");
        var userName = httpContext.Session.GetString("UserName");

        // 🔒 If session not found → redirect to Login
        if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(userRole))
        {
            context.Result = new RedirectToActionResult("Login", "Account", null);
            return;
        }

        // 🔒 If role is required and doesn’t match → redirect to AccessDenied
        if (!string.IsNullOrEmpty(_role) && userRole != _role)
        {
            context.Result = new RedirectToActionResult("AccessDenied", "Account", null);
            return;
        }

        base.OnActionExecuting(context);
    }
}
