using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Models;
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

        public CompaniaController(ApplicationDbContext db)
        {
            _db = db;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Compania>>> GetCompanias()
        {
           var listas = await _db.TbCompania.ToListAsync();
           return Ok(listas);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Compania>> GetCompania(int id)
        {
            var comp = await _db.TbCompania.FindAsync(id);
            return Ok(comp);
        }
        
    }
}