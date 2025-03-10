using Microsoft.AspNetCore.Mvc;
using NetCoreSeguridadEmpleados.Filters;
using NetCoreSeguridadEmpleados.Models;
using NetCoreSeguridadEmpleados.Repositories;

namespace NetCoreSeguridadEmpleados.Controllers
{
    public class EmpleadosController : Controller
    {
        private RepositoryHospital repo;
        public EmpleadosController(RepositoryHospital repo)
        {
            this.repo = repo;
        }
        public async Task<IActionResult> Index()
        {
            List<Empleado> empleados = await this.repo.GetEmpleadosAsync();
            return View(empleados);
        }
        public async Task<IActionResult> Details
            (int idEmpleado)
        {
            Empleado empleado = await this.repo.FindEmpleadoAsync(idEmpleado);
            return View(empleado);
        }
        [AuthorizeEmpleados]
        public async Task<IActionResult> PerfilEmpleado
            ()
        {
            //Empleado empleado = await this.repo.FindEmpleadoAsync(idEmpleado);
            return View();
        }
    }
}
