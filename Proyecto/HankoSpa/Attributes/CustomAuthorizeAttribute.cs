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
            // Aquí deberías implementar la lógica real de autorización
            // Por ejemplo, verificar si el usuario tiene el permiso y módulo requeridos
            // Si no está autorizado:
            // context.Result = new ForbidResult();
        }
    }
}

