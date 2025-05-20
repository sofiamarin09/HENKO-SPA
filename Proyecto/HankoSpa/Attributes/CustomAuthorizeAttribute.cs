using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace HankoSpa.Attributes
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true)]
    public class CustomAuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        public string Permission { get; set; }
        public string Module { get; set; }

        public CustomAuthorizeAttribute(string permission, string module)
        {
            Permission = permission;
            Module = module;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;

            // Si el usuario no está autenticado
            if (!user.Identity?.IsAuthenticated ?? true)
            {
                context.Result = new RedirectToRouteResult(new { controller = "Account", action = "Login" });
                return;
            }

            // Ejemplo: Supón que los permisos están en claims tipo "Permission" y "Module"
            var hasPermission = user.Claims.Any(c =>
                c.Type == "Permission" && c.Value == Permission &&
                user.Claims.Any(m => m.Type == "Module" && m.Value == Module)
            );

            // Si el usuario no tiene el permiso requerido
            if (!hasPermission)
            {
                context.Result = new ForbidResult();
            }
            
        }
    }
}

