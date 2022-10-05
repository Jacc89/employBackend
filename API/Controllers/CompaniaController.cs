using System.Net;
using Core.Models;
using Core.Dto;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Infraestructura.Data.IRepositorio;

namespace API.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class CompaniaController : ControllerBase
    {
      
        
        private ResponseDto _response;
        private readonly ILogger<CompaniaController> _logger;
        private readonly IMapper _mapper;
        private readonly IUnidadTrabajo _unidadTrabajo;

        public CompaniaController(IUnidadTrabajo unidadTrabajo, ILogger<CompaniaController> logger,
                                                        IMapper mapper)
        {
            _unidadTrabajo = unidadTrabajo;
            _mapper = mapper;
            _logger = logger;
            
            _response = new ResponseDto();
        }
        // get 
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Compania>>> GetCompanias()
        {
           _logger.LogInformation("Listado de companias get companias");
           var listas = await _unidadTrabajo.Compania.ObtenerTodos();
           _response.Resultado = listas;
           _response.Mensaje =" Listado de Clientes";
           _response.StatusCode = HttpStatusCode.OK;
           return Ok(_response);
        }
        // get id
        [HttpGet("{id}", Name = "GetCompania" )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Compania>> GetCompania(int id)
        {
            if(id == 0){
                _logger.LogError("Debe de enviar un ID  valido");
                _response.Mensaje="Debe de enviar el ID valido.";
                _response.IsExistoso =false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }

            var comp = await _unidadTrabajo.Compania.ObtenerPrimero(c =>c.Id == id);

            if(comp == null){
                _logger.LogError("LA compañia no existe get compania.");
                _response.Mensaje="Compañia no existe";
                _response.IsExistoso =false;
                _response.StatusCode = HttpStatusCode.NotFound;
                return NotFound(_response); // la informacion no existe
            }
                _response.Resultado = comp;
                _response.Mensaje ="Datos de Compañias " + comp.Id;
                _response.StatusCode = HttpStatusCode.OK;

            return Ok(_response); // Status code = 200
        }
        // post
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Compania>> PostCompania([FromBody] CompaniaDto companiaDto)
        {            
            
            if(companiaDto == null){
                _response.Mensaje ="Información incorrecta";
                _response.IsExistoso = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response); //informacion invalida
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var companiaExiste = await _unidadTrabajo.Compania.ObtenerPrimero(c  => c.NombreCompania.ToLower() ==  companiaDto.NombreCompania.ToLower());
            if (companiaExiste != null)
            {
                // ModelState.AddModelError("NombreDiplicado", "Nombre de la compánia ya existe");
                _response.IsExistoso = false;
                _response.Mensaje = "Nombre de la compánia ya existe";
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }

            Compania compania  = _mapper.Map<Compania>(companiaDto);

            await _unidadTrabajo.Compania.Agregar(compania);                                  
            await _unidadTrabajo.Guardar();
            _response.IsExistoso = true;
            _response.Mensaje = "Guardar compánia con exito";
            _response.StatusCode = HttpStatusCode.Created;
            
            return CreatedAtRoute("GetCompania", new{id = compania.Id}, compania); // status code = 201
        }

// put
        [HttpPut("{id}")] 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> PutCompania( int id, [FromBody] CompaniaDto companiaDto){
            if (id != companiaDto.Id){
                _response.IsExistoso = false;
                _response.Mensaje = "Id de compañian no coincide";
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }

            if (!ModelState.IsValid){
                return BadRequest(ModelState);
                
            }

            var companiaExiste = await _unidadTrabajo.Compania.ObtenerPrimero(c => c.NombreCompania == companiaDto.NombreCompania && c.Id != companiaDto.Id);

            if(companiaExiste! == null)
            {
                // ModelState.AddModelError("nombreDuplicado", " Nombre de la compania ya existe verifique.");
                _response.IsExistoso = false;
                _response.Mensaje = "Nombre de la compania ya existe verifique.";
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
                
            }

            Compania compania  = _mapper.Map<Compania>(companiaDto);

            _unidadTrabajo.Compania.Actualizar(compania);
            await _unidadTrabajo.Guardar();
            _response.IsExistoso = false;
            _response.Mensaje = "Compania Actualizada.";
            _response.StatusCode = HttpStatusCode.OK;
            return Ok(compania);

        }
// delete
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteCompania(int id){
            var compania = await _unidadTrabajo.Compania.ObtenerPrimero(c=>c.Id == id);
            if(compania == null){
                _response.IsExistoso = false;
                _response.Mensaje = "Compania No existe verifique.";
                _response.StatusCode = HttpStatusCode.NotFound;
                return NotFound(_response);
            }
           _unidadTrabajo.Compania.Remove(compania);
            await _unidadTrabajo.Guardar();
            _response.IsExistoso = false;
            _response.Mensaje = "Compania Eliminada";
            _response.StatusCode = HttpStatusCode.NoContent;
            return Ok(_response);
        }
        

    }
}