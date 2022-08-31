using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    public class CompaniaController : ControllerBase
    {
      
        private readonly ApplicationDbContext _db;
        private ResponseDto _response;
        private readonly ILogger<CompaniaController> _logger;
        private readonly IMapper _mapper;

        public CompaniaController(ApplicationDbContext db, ILogger<CompaniaController> logger,
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
        public async Task<ActionResult<IEnumerable<Compania>>> GetCompanias()
        {
            _logger.LogInformation("Listado de companias get companias");
           var listas = await _db.TbCompania.ToListAsync();
           _response.Resultado = listas;
           _response.Mensaje =" Listado de Clientes";
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
                return BadRequest(_response);
            }

            var comp = await _db.TbCompania.FindAsync(id);

            if(comp == null){
                 _logger.LogError("LA compañia no existe get compania.");
                _response.Mensaje="Compañia no existe";
                _response.IsExistoso =false;
                return NotFound(_response); // la informacion no existe
            }
                _response.Resultado = comp;
                _response.Mensaje ="Datos de Compañias " + comp.Id;

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
                return BadRequest(_response); //informacion invalida
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var companiaExiste = await _db.TbCompania.FirstOrDefaultAsync
                                (c  => c.NombreCompania.ToLower() ==  companiaDto.NombreCompania.ToLower());
            if (companiaExiste != null)
            {
                ModelState.AddModelError("NombreDiplicado", "Nombre de la compánia ya existe");
                return BadRequest(ModelState);
            }

            Compania compania  = _mapper.Map<Compania>(companiaDto);

            await _db.TbCompania.AddAsync( compania);                                   
            await _db.SaveChangesAsync();
            return CreatedAtRoute("GetCompania", new{id = compania.Id}, compania); // status code = 201
        }

// put
        [HttpPut("{id}")] 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> PutCompania( int id, [FromBody] CompaniaDto companiaDto){
            if (id != companiaDto.Id){
                return BadRequest("Id de compañian no coincide");
            }

            if (!ModelState.IsValid){
                return BadRequest(ModelState);
                
            }

            var companiaExiste = await _db.TbCompania.FirstOrDefaultAsync(c => c.NombreCompania == companiaDto.NombreCompania && c.Id != companiaDto.Id);

            if(companiaExiste! == null)
            {
                ModelState.AddModelError("nombreDuplicado", " Nombre de la compania ya existe verifique.");
                return BadRequest(ModelState);
                
            }

            Compania compania  = _mapper.Map<Compania>(companiaDto);

            _db.Update(compania);
            await _db.SaveChangesAsync();
            return Ok(compania);

        }
// delete
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteCompania(int id){
            var compania = await _db.TbCompania.FindAsync(id);
            if(compania == null){
                return NotFound();
            }
            _db.TbCompania.Remove(compania);
            await _db.SaveChangesAsync();
            return NoContent();
        }
        

    }
}