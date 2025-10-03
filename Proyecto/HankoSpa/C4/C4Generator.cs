using Structurizr;
using Structurizr.Api;
using System;

namespace HankoSpa.C4
{
    public static class C4Generator
    {
        public static void GenerateAndUpload()
        {
            try
            {
                // ========== CONFIGURACIÓN C4 STRUCTURIZR - VERSIÓN FINAL Y COMPLETA ==========
                const long workspaceId = 106784;
                const string apiKey = "c12858ea-5790-4fe8-9711-7965d37f8837";
                const string apiSecret = "aedb9a08-9919-450e-a50c-6bd05632c21f";

                var workspace = new Workspace("Sistema HankoSpa", "Sistema completo de gestión para spa con control de roles");
                var model = workspace.Model;

                // ========== 1. ACTORES (PERSONAS) ==========
                var cliente = model.AddPerson("Cliente", "Usuario que agenda citas y consulta servicios");
                var empleado = model.AddPerson("Empleado", "Personal que gestiona citas y servicios");
                var administrador = model.AddPerson("Administrador", "Administra usuarios y configuración");
                var superusuario = model.AddPerson("SuperUsuario", "Acceso total al sistema");

                // ========== 2. SISTEMA PRINCIPAL ==========
                var hankoSpa = model.AddSoftwareSystem("Sistema HankoSpa", "Gestión integral de spa con control de accesos");

                // ========== 3. CONTENEDORES ==========
                var webApp = hankoSpa.AddContainer("Aplicación Web MVC", "ASP.NET Core MVC", "ASP.NET Core MVC");
                var appServices = hankoSpa.AddContainer("Servicios de Aplicación", "Lógica de negocio", "ASP.NET Core Services");
                var dataAccess = hankoSpa.AddContainer("Acceso a Datos", "Entity Framework", "Entity Framework Core");
                var database = hankoSpa.AddContainer("Base de Datos", "SQL Server", "Microsoft SQL Server");
                var infrastructure = hankoSpa.AddContainer("Infraestructura", "Helpers y utilidades", "Librerías .NET");
                var notificaciones = hankoSpa.AddContainer("Notificaciones", "Toast notifications", "Librería .NET");

                // ========== 4. SISTEMAS EXTERNOS ==========
                var identityFramework = model.AddSoftwareSystem("Identity Framework", "Autenticación ASP.NET Core");
                // NOTA: Se ha eliminado la declaración de Pasarela de Pago.
                var notificacionEmail = model.AddSoftwareSystem(Location.External, "Servicio de Email", "Envía confirmaciones, recordatorios y alertas por correo");

                // ========== 5. COMPONENTES - APLICACIÓN WEB ==========
                var accountController = webApp.AddComponent("AccountController", "Autenticación y registro", "Controller");
                var citasController = webApp.AddComponent("CitasController", "Gestión de citas", "Controller");
                var servicioController = webApp.AddComponent("ServicioController", "Gestión de servicios", "Controller");
                var userController = webApp.AddComponent("UserController", "Gestión de usuarios", "Controller");
                var rolController = webApp.AddComponent("RolController", "Gestión de roles", "Controller");
                var permissionsController = webApp.AddComponent("PermissionsController", "Gestión de permisos", "Controller");
                var homeController = webApp.AddComponent("HomeController", "Páginas principales", "Controller");
                var customAuthorizeAttribute = webApp.AddComponent("CustomAuthorizeAttribute", "Filtro de autorización basado en permisos y módulos", "Authorization Filter");

                // ========== 6. COMPONENTES - SERVICIOS ==========
                var userService = appServices.AddComponent("UserService", "Gestión de usuarios", "Service");
                var citasService = appServices.AddComponent("CitasService", "Gestión de citas", "Service");
                var servicioService = appServices.AddComponent("ServicioService", "Gestión de servicios", "Service");
                var customRolService = appServices.AddComponent("CustomRolService", "Gestión de roles", "Service");
                var permissionService = appServices.AddComponent("PermissionService", "Gestión de permisos", "Service");
                var rolPermissionService = appServices.AddComponent("RolPermissionService", "Permisos de roles", "Service");

                // ========== 7. COMPONENTES - ACCESO A DATOS ==========
                var userRepository = dataAccess.AddComponent("UserRepository", "Repositorio usuarios", "Repository");
                var citaRepository = dataAccess.AddComponent("CitaRepository", "Repositorio citas", "Repository");
                var servicioRepository = dataAccess.AddComponent("ServicioRepository", "Repositorio servicios", "Repository");
                var customRolRepository = dataAccess.AddComponent("CustomRolRepository", "Repositorio roles", "Repository");
                var permissionRepository = dataAccess.AddComponent("PermissionRepository", "Repositorio permisos", "Repository");
                var rolPermissionRepository = dataAccess.AddComponent("RolPermissionRepository", "Repositorio permisos-roles", "Repository");
                var appDbContext = dataAccess.AddComponent("AppDbContext", "DbContext principal", "DbContext");

                // ========== 8. COMPONENTES - BASE DE DATOS ==========
                var usersTable = database.AddComponent("Users", "Tabla usuarios", "SQL Table");
                var citasTable = database.AddComponent("Citas", "Tabla citas", "SQL Table");
                var serviciosTable = database.AddComponent("Servicios", "Tabla servicios", "SQL Table");
                var customRolesTable = database.AddComponent("CustomRoles", "Tabla roles", "SQL Table");
                var permissionsTable = database.AddComponent("Permissions", "Tabla permisos", "SQL Table");
                var rolPermissionsTable = database.AddComponent("RolPermissions", "Tabla roles-permisos", "SQL Table");

                // ========== 9. COMPONENTES - INFRAESTRUCTURA ==========
                var autoMapper = infrastructure.AddComponent("AutoMapper", "Mapeo de objetos", "Object Mapper");
                var combosHelper = infrastructure.AddComponent("CombosHelper", "Listas desplegables", "Utility");
                var customClaimsFactory = infrastructure.AddComponent("CustomClaimsFactory", "Claims personalizados", "Security");
                var notyfService = infrastructure.AddComponent("NotyfService", "Notificaciones", "Notification Service");

                // ========== 10. RELACIONES PRINCIPALES ==========
                // Actores -> Sistema
                cliente.Uses(webApp, "Agendar citas y consultar servicios", "HTTPS");
                empleado.Uses(webApp, "Gestionar operaciones diarias", "HTTPS");
                administrador.Uses(webApp, "Administrar sistema", "HTTPS");
                superusuario.Uses(webApp, "Configuración total", "HTTPS");

                // Actores -> Sistema principal (para que aparezcan en el Contexto)
                cliente.Uses(hankoSpa, "Usa el sistema para agendar citas y servicios", "HTTPS");
                empleado.Uses(hankoSpa, "Usa el sistema para gestionar operaciones", "HTTPS");
                administrador.Uses(hankoSpa, "Usa el sistema para administrar usuarios y configuraciones", "HTTPS");
                superusuario.Uses(hankoSpa, "Usa el sistema para gestión total", "HTTPS");

                // Relaciones de Sistemas Externos
                // NOTA: Se ha eliminado la relación con Pasarela de Pago.
                hankoSpa.Uses(notificacionEmail, "Envía notificaciones de citas y registro", "SMTP/API");

                // Contenedores
                webApp.Uses(appServices, "Usa servicios via DI");
                appServices.Uses(dataAccess, "Accede a datos via DI");
                dataAccess.Uses(database, "Persiste datos");
                webApp.Uses(identityFramework, "Autentica usuarios");
                webApp.Uses(notificaciones, "Muestra notificaciones");
                appServices.Uses(infrastructure, "Usa utilidades");

                // Controladores -> Servicios
                accountController.Uses(userService, "Usa para autenticación");
                citasController.Uses(citasService, "Usa para gestión citas");
                citasController.Uses(servicioService, "Usa para servicios");
                citasController.Uses(userService, "Usa para usuarios");
                servicioController.Uses(servicioService, "Usa para servicios");
                userController.Uses(userService, "Usa para usuarios");
                rolController.Uses(customRolService, "Usa para roles");
                permissionsController.Uses(permissionService, "Usa para permisos");

                // Controladores -> Filtro de Autorización Personalizado
                accountController.Uses(customAuthorizeAttribute, "Protegido en Logout");
                citasController.Uses(customAuthorizeAttribute, "Protegido por políticas de citas");
                servicioController.Uses(customAuthorizeAttribute, "Protegido por políticas de servicios");
                userController.Uses(customAuthorizeAttribute, "Protegido por políticas de usuarios");
                rolController.Uses(customAuthorizeAttribute, "Protegido por políticas de roles");
                permissionsController.Uses(customAuthorizeAttribute, "Protegido por políticas de permisos");

                // Servicios -> Repositorios
                userService.Uses(userRepository, "Accede a usuarios");
                citasService.Uses(citaRepository, "Accede a citas");
                citasService.Uses(servicioRepository, "Accede a servicios");
                citasService.Uses(userRepository, "Accede a usuarios");
                servicioService.Uses(servicioRepository, "Accede a servicios");
                customRolService.Uses(customRolRepository, "Accede a roles");
                permissionService.Uses(permissionRepository, "Accede a permisos");
                rolPermissionService.Uses(rolPermissionRepository, "Accede a permisos-roles");

                // Repositorios -> DbContext
                userRepository.Uses(appDbContext, "Usa DbContext");
                citaRepository.Uses(appDbContext, "Usa DbContext");
                servicioRepository.Uses(appDbContext, "Usa DbContext");
                customRolRepository.Uses(appDbContext, "Usa DbContext");
                permissionRepository.Uses(appDbContext, "Usa DbContext");
                rolPermissionRepository.Uses(appDbContext, "Usa DbContext");

                // DbContext -> Tablas
                appDbContext.Uses(usersTable, "Mapea tabla");
                appDbContext.Uses(citasTable, "Mapea tabla");
                appDbContext.Uses(serviciosTable, "Mapea tabla");
                appDbContext.Uses(customRolesTable, "Mapea tabla");
                appDbContext.Uses(permissionsTable, "Mapea tabla");
                appDbContext.Uses(rolPermissionsTable, "Mapea tabla");

                // Utilidades
                userService.Uses(autoMapper, "Usa para mapeo");
                userController.Uses(combosHelper, "Usa para combos");
                userService.Uses(customClaimsFactory, "Usa para claims");
                webApp.Uses(notyfService, "Usa para notificaciones");

                // ========== 11. CONFIGURAR VISTAS ==========
                var views = workspace.Views;
                var contextView = views.CreateSystemContextView(hankoSpa, "1-Contexto", "Diagrama de Contexto - Muestra actores y sistemas externos");
                contextView.AddAllPeople();
                contextView.AddAllSoftwareSystems();

                var containerView = views.CreateContainerView(hankoSpa, "2-Contenedores", "Diagrama de Contenedores - Arquitectura");
                containerView.AddAllPeople();
                containerView.AddAllSoftwareSystems();
                containerView.AddAllContainers();

                var webComponentsView = views.CreateComponentView(webApp, "3-ComponentesWeb", "Componentes de la Aplicación Web");
                webComponentsView.AddAllComponents();
                webComponentsView.Add(accountController);
                webComponentsView.Add(citasController);
                webComponentsView.Add(servicioController);
                webComponentsView.Add(userController);
                webComponentsView.Add(rolController);
                webComponentsView.Add(permissionsController);
                webComponentsView.Add(homeController);
                webComponentsView.Add(userService);
                webComponentsView.Add(citasService);
                webComponentsView.Add(servicioService);
                webComponentsView.Add(customRolService);
                webComponentsView.Add(permissionService);

                var userCodeView = views.CreateComponentView(webApp, "4-Codigo-Usuarios", "Flujo completo - Gestión de Usuarios");
                userCodeView.Add(userService);
                userCodeView.Add(userController);
                userCodeView.Add(userRepository);
                userCodeView.Add(appDbContext);
                userCodeView.Add(usersTable);
                userCodeView.Add(customRolesTable);
                userCodeView.Add(identityFramework);
                userCodeView.Add(autoMapper);

                var citasCodeView = views.CreateComponentView(webApp, "5-Codigo-Citas", "Flujo completo - Gestión de Citas");
                citasCodeView.Add(citasService);
                citasCodeView.Add(citasController);
                citasCodeView.Add(citaRepository);
                citasCodeView.Add(servicioService);
                citasCodeView.Add(userService);
                citasCodeView.Add(appDbContext);
                citasCodeView.Add(citasTable);
                citasCodeView.Add(serviciosTable);
                citasCodeView.Add(usersTable);

                var rolesCodeView = views.CreateComponentView(webApp, "6-Codigo-Roles", "Arquitectura completa - Sistema de Roles");
                rolesCodeView.Add(customRolService);
                rolesCodeView.Add(rolController);
                rolesCodeView.Add(permissionsController);
                rolesCodeView.Add(permissionService);
                rolesCodeView.Add(rolPermissionService);
                rolesCodeView.Add(customRolRepository);
                rolesCodeView.Add(permissionRepository);
                rolesCodeView.Add(rolPermissionRepository);
                rolesCodeView.Add(appDbContext);
                rolesCodeView.Add(customRolesTable);
                rolesCodeView.Add(permissionsTable);
                rolesCodeView.Add(rolPermissionsTable);

                // ========== 12. ESTILOS ==========
                var styles = views.Configuration.Styles;
                styles.Add(new ElementStyle(Tags.Person) { Background = "#0d47a1", Color = "#ffffff", Shape = Shape.Person });
                styles.Add(new ElementStyle(Tags.SoftwareSystem) { Background = "#1168bd", Color = "#ffffff" });
                styles.Add(new ElementStyle(Tags.Container) { Background = "#438dd5", Color = "#ffffff" });
                styles.Add(new ElementStyle(Tags.Component) { Background = "#85bbf0", Color = "#000000" });
                styles.Add(new ElementStyle("Database") { Background = "#1f4e7a", Color = "#ffffff", Shape = Shape.Cylinder });

                // ========== 13. SUBIR ==========
                var client = new StructurizrClient(apiKey, apiSecret);
                client.PutWorkspace(workspaceId, workspace);

                Console.WriteLine("✅ C4 COMPLETO subido: https://structurizr.com/workspace/" + workspaceId);
                Console.WriteLine("📊 6 DIAGRAMAS creados.");
            }
            catch (Exception ex)
            {
                // Mensaje de error más claro en el log
                Console.WriteLine("❌ Error al subir el Workspace C4: " + ex.Message);
            }
        }
    }
}