using Core.Models;
using Core.Dto;
using Infraestructura.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace API.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class EmpleadoController : ControllerBase
    {
      
        private readonly ApplicationDbContext _db;
        private ResponseDto _response;
        private readonly ILogger<EmpleadoController> _logger;
        private readonly IMapper _mapper;

        public EmpleadoController(ApplicationDbContext db, ILogger<EmpleadoController> logger,
                                                        IMapper mapper)
        {
            _mapper = mapper;
            _logger = logger;
            _db = db;
            _response = new ResponseDto();
        }
        // get 
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<EmpleadoReadDto>>> GetEmpleados()
        {
            _logger.LogInformation("Listado de Empleados get Empleados");
           var listas = await _db.TbEmpleado.Include(c=>c.Compania).ToListAsync();
           _response.Resultado = _mapper.Map<IEnumerable<Empleado>, IEnumerable<EmpleadoReadDto>>(listas);
           _response.Mensaje =" Listado de Empleados";
           return Ok(_response);
        }

        // otro get

        [HttpGet]
        [Route("empleadosporcompania/{companiaId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]

        public async Task<ActionResult<IEnumerable<EmpleadoReadDto>>> Getempleadosporcompania( int companiaId){
            _logger.LogInformation("lista de empleados  compania");
            var list = await _db.TbEmpleado.Include(c=>c.Compania).Where(e=>e.CompaniaId == companiaId).ToListAsync();
            _response.Resultado = _mapper.Map<IEnumerable<Empleado>, IEnumerable<EmpleadoReadDto>>(list);
            _response.IsExistoso = true;
            _response.Mensaje= "Listado de companias emmpleados";
            return Ok(_response);
        }








        // get id
        [HttpGet("{id}", Name = "GetEmpleado" )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Empleado>> GetEmpleado(int id)
        {
            if(id == 0){
                _logger.LogError("Debe de enviar un ID  valido");
                _response.Mensaje="Debe de enviar el ID valido.";
                _response.IsExistoso =false;
                return BadRequest(_response);
            }

            var emp = await _db.TbEmpleado.Include(c=>c.Compania).FirstOrDefaultAsync(e=>e.Id==id);

            if(emp == null){
                 _logger.LogError("LA empleado no existe get compania.");
                _response.Mensaje="empleado no existe";
                _response.IsExistoso =false;
                return NotFound(_response); // la informacion no existe
            }
                _response.Resultado = _mapper.Map<Empleado, EmpleadoReadDto>(emp);
                _response.Mensaje ="Datos de empleados " + emp.Id;

            return Ok(_response); // Status code = 200
        }
        // post
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Empleado>> PostEmpleado([FromBody] EmpleadoUpsertDto empleadoDto)
        {            
            
            if(empleadoDto == null){
                _response.Mensaje ="Información incorrecta";
                _response.IsExistoso = false;
                return BadRequest(_response); //informacion invalida
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var EmpleadoExiste = await _db.TbEmpleado.FirstOrDefaultAsync
                                (c  => c.Nombres.ToLower() ==  empleadoDto.Nombres.ToLower() &&
                                       c.Apellidos.ToLower() ==  empleadoDto.Apellidos.ToLower());
            if (EmpleadoExiste != null)
            {
                ModelState.AddModelError("NombreDiplicado", "Nombre del empleado ya existe");
                return BadRequest(ModelState);
            }

            Empleado empleado  = _mapper.Map<Empleado>(empleadoDto);

            await _db.TbEmpleado.AddAsync( empleado);                                   
            await _db.SaveChangesAsync();
            return CreatedAtRoute("GetCempleado", new{id = empleado.Id}, empleado); // status code = 201
        }

// put
        [HttpPut("{id}")] 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> PutEmpleado( int id, [FromBody] EmpleadoUpsertDto empleadoDto){
            if (id != empleadoDto.Id){
                return BadRequest("Id de compañian no coincide");
            }

            if (!ModelState.IsValid){
                return BadRequest(ModelState);
                
            }

            var cempleadoExiste = await _db.TbEmpleado.FirstOrDefaultAsync(c  => c.Nombres.ToLower() ==  empleadoDto.Nombres.ToLower() &&
                                       c.Apellidos.ToLower() ==  empleadoDto.Apellidos.ToLower()
                                       && c.Id != empleadoDto.Id);

            if(cempleadoExiste! == null)
            {
                ModelState.AddModelError("nombreDuplicado", " Nombre de la cempleado ya existe verifique.");
                return BadRequest(ModelState);
                
            }

            Empleado empleado  = _mapper.Map<Empleado>(empleadoDto);

            _db.Update(empleado);
            await _db.SaveChangesAsync();
            return Ok(empleado);

        }
// delete
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteEmpleado(int id){
            var empleado = await _db.TbEmpleado.FindAsync(id);
            if(empleado == null){
                return NotFound();
            }
            _db.TbEmpleado.Remove(empleado);
            await _db.SaveChangesAsync();
            return NoContent();
        }
        

    }
}