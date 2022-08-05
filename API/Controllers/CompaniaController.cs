using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Models;
using Core.Dto;
using Infraestructura.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class CompaniaController : ControllerBase
    {
      
        private readonly ApplicationDbContext _db;
        private ResponseDto _response;

        public CompaniaController(ApplicationDbContext db)
        {
            _db = db;
            _response = new ResponseDto();
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Compania>>> GetCompanias()
        {
           var listas = await _db.TbCompania.ToListAsync();
           _response.Resultado = listas;
           _response.Mensaje =" Listado de Clientes";
           return Ok(_response);
        }
        [HttpGet("{id}", Name = "GetCompania" )]
        public async Task<ActionResult<Compania>> GetCompania(int id)
        {
            var comp = await _db.TbCompania.FindAsync(id);
                _response.Resultado = comp;
                _response.Mensaje ="Datos de Compañias" + comp.Id;

            return Ok(_response); // Status code = 200
        }
        [HttpPost]
        public async Task<ActionResult<Compania>> PostCompania([FromBody] Compania compania){
            await _db.TbCompania.AddAsync( compania);
            await _db.SaveChangesAsync();
            return CreatedAtRoute("GetCompania", new{id = compania.Id}, compania); // status code = 201
        }
        [HttpPut("{id}")] 
        public async Task<ActionResult> PutCompania( int id, [FromBody] Compania compania){
            if (id != compania.Id){
                return BadRequest("Id de compañian no coincide");
            }
            _db.Update(compania);
            await _db.SaveChangesAsync();
            return Ok(compania);
        }

        [HttpDelete("{id}")]
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