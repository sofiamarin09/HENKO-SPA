using HankoSpa.Data; 
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;


// El resto del código permanece igual


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

            if (!user.Identity?.IsAuthenticated ?? true)
            {
                context.Result = new RedirectToRouteResult(new { controller = "Account", action = "Login" });
                return;
            }

            // Obtener el servicio de base de datos
            var db = context.HttpContext.RequestServices.GetRequiredService<AppDbContext>();

            // Obtener el usuario autenticado
            var userId = user.Claims.FirstOrDefault(c => c.Type == "sub" || c.Type == "UserId" || c.Type == "Id")?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                context.Result = new ForbidResult();
                return;
            }

            // Buscar el usuario y su rol
            var usuario = db.Users.FirstOrDefault(u => u.Id == userId);
            if (usuario == null || usuario.CustomRolId == 0)
            {
                context.Result = new ForbidResult();
                return;
            }

            // Buscar los permisos del rol
            var tienePermiso = db.RolPermissions
                .Where(rp => rp.CustomRolId == usuario.CustomRolId)
                .Any(rp => rp.Permission.NombrePermiso == Permission && rp.Permission.Module == Module);

            if (!tienePermiso)
            {
                context.Result = new ForbidResult();
            }
        }
    }
}
