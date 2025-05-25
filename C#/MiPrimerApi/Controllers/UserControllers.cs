using Microsoft.AspNetCore.Mvc;

namespace MiPrimerApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private static List<User> users = new List<User>
        {
            new User { Id = 1, Name = "Juan" },
            new User { Id = 2, Name = "Ana" }
        };

       /// <summary>
        /// Obtiene la lista de todos los usuarios.
        /// </summary>
        /// <returns>Lista de usuarios</returns>
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(users);
        }

        /// <summary>
        /// Obtiene un usuario por su identificador.
        /// </summary>
        /// <param name="id">Identificador del usuario</param>
        /// <returns>Usuario encontrado o 404</returns>
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var user = users.FirstOrDefault(u => u.Id == id);
            if (user == null)
                return NotFound();
            return Ok(user);
        }

        /// <summary>
        /// Crea un nuevo usuario.
        /// </summary>
        /// <param name="newUser">Datos del usuario a crear</param>
        /// <returns>Usuario creado con su identificador</returns>
        [HttpPost]
        public IActionResult Post(User newUser)
        {
            newUser.Id = users.Count + 1;
            users.Add(newUser);
            return CreatedAtAction(nameof(Get), new { id = newUser.Id }, newUser);
        }
    }
}
