#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Lms.Core.Entities;
using Lms.Data.Data;
using Lms.Core.Dto;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;

namespace Lms.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ModulesController : ControllerBase
    {
        private readonly LmsApiContext _context;
        private readonly IMapper mapper;

        public ModulesController(LmsApiContext context, IMapper mapper)
        {
            _context = context;
            this.mapper = mapper;
        }

        // GET: api/Modules
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ModuleDto>>> GetModule()
        {
            var moduleDto = mapper.ProjectTo<ModuleDto>(_context.Module);

            return Ok(await moduleDto.ToListAsync());

            //return await _context.Module.ToListAsync();
        }

        // GET: api/Modules/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ModuleDto>> GetModule(int id)
        {
            var @module = mapper.Map<ModuleDto>( await _context.Module.FindAsync(id));

            if (@module == null)
            {
                return NotFound();
            }

            return @module;
        }

        // PUT: api/Modules/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutModule(int id, ModuleForUpdateDto @module)
        {
            //if (id != @module.Id)
            //{
            //    return BadRequest();
            //}

            var moduleFromRepo = await _context.Module.FindAsync(id);

            if(moduleFromRepo == null) { return NotFound(); }

            mapper.Map(@module, moduleFromRepo);

            _context.Entry(moduleFromRepo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ModuleExists(id))
                {
                    return NotFound();
                }
                else
                {
                    return StatusCode(500);
                    //throw;
                }
            }

            return NoContent();
        }

        // POST: api/Modules
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Module>> PostModule(ModuleForUpdateDto @module)
        {
            var moduleEntity = mapper.Map<Module>(@module);

            _context.Module.Add(moduleEntity);
            await _context.SaveChangesAsync();
            
            var moduleToReturn = mapper.Map<ModuleDto>(moduleEntity);

            return CreatedAtAction("GetModule", new { id = moduleEntity.Id }, @module);
        }

        // DELETE: api/Modules/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteModule(int id)
        {
            var @module = await _context.Module.FindAsync(id);
            if (@module == null)
            {
                return NotFound();
            }

            _context.Module.Remove(@module);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPatch("{moduleId}")]
        public async Task<ActionResult<ModuleDto>> PatchModule(int moduleId,
            JsonPatchDocument<ModuleForUpdateDto> patchDocument)
        {
            if (!ModuleExists(moduleId)) { return NotFound(); } 

            var moduleFromRepo = await _context.Module.FindAsync(moduleId);
            if(moduleFromRepo == null) { return NotFound(); }

            var moduleToPatch = mapper.Map<ModuleForUpdateDto>(moduleFromRepo);
            //ToDo PatchModules: Mapper funkar inte. Fixa så det funkar....
            //AutoMapper.AutoMapperMappingException: Missing type map configuration or unsupported mapping.

            patchDocument.ApplyTo(moduleToPatch, ModelState);

            if (!TryValidateModel(moduleToPatch))
            {
                return ValidationProblem(ModelState);
            }
             mapper.Map(moduleToPatch, moduleFromRepo);
            _context.Update(moduleFromRepo);
            await _context.SaveChangesAsync();

            return NoContent();
            
        }



        private bool ModuleExists(int id)
        {
            return _context.Module.Any(e => e.Id == id);
        }
    }
}
