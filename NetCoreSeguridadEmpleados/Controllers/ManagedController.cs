﻿using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using NetCoreSeguridadEmpleados.Models;
using NetCoreSeguridadEmpleados.Repositories;
using System.Security.Claims;

namespace NetCoreSeguridadEmpleados.Controllers
{
    public class ManagedController : Controller
    {
        private RepositoryHospital repo;
        public ManagedController(RepositoryHospital repo)
        {
            this.repo = repo;
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login
            (string username, string password)
        {
            int idEmpleado = int.Parse(password);
            Empleado empleado = await
                this.repo.LogInEmpleadoAsync(username, idEmpleado);
            if (empleado != null)
            {
                ClaimsIdentity identity =
                    new ClaimsIdentity(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        ClaimTypes.Name, ClaimTypes.Role);
                //  ALMACENAMOS EL NOMBRE DEL USUARIO
                Claim claimName =
                    new Claim(ClaimTypes.Name, empleado.Apellido);
                identity.AddClaim(claimName);
                //  ALMACENAMOS EL ID DEL USUARIO
                Claim claimId =
                    new Claim(ClaimTypes.NameIdentifier, empleado.IdEmpleado.ToString());
                identity.AddClaim(claimId);
                //  POR AHORA, NO VAMOSA A UTILIZAR ROLES
                //  NO LO HACEMOS

                //  COMO ROLE, VOY A UTILIZAR EL DATO DEL OFICIO
                Claim claimOficio =
                    new Claim(ClaimTypes.Role, empleado.Oficio);
                identity.AddClaim(claimOficio);
                Claim claimSalario =
                    new Claim("Salario", empleado.Salario.ToString());
                identity.AddClaim(claimSalario);
                Claim claimDept =
                    new Claim("Departamento", empleado.Departamento.ToString());
                identity.AddClaim(claimDept);

                ClaimsPrincipal userPrincipal =
                    new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    userPrincipal);
                //  LLEVAMOS AL USER UNA VISTA QUE TODAVIA NO TENEMOS
                //  PERO QUE HAREMOS EN BREVE
                return RedirectToAction("PerfilEmpleado", "Empleados");

            }
            else
            {
                ViewData["MENSAJE"] = "Usuario/Password incorrectos";
            }
            return View();
        }
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        public IActionResult ErrorAcceso()
        {
            return View();
        }
    }
}
