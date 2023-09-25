using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PMDB.Models;

namespace PMAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TbOrgUsersController : ControllerBase
    {
        private readonly PmdbContext _context;

        public TbOrgUsersController(PmdbContext context)
        {
            _context = context;
        }

        // GET: api/TbOrgUsers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TbOrgUser>>> GetTbOrgUsers()
        {
          if (_context.TbOrgUsers == null)
          {
              return NotFound();
          }
            return await _context.TbOrgUsers.ToListAsync();
        }

        // GET: api/TbOrgUsers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TbOrgUser>> GetTbOrgUser(string id)
        {
          if (_context.TbOrgUsers == null)
          {
              return NotFound();
          }
            var tbOrgUser = await _context.TbOrgUsers.FindAsync(id);

            if (tbOrgUser == null)
            {
                return NotFound();
            }

            return tbOrgUser;
        }

        // PUT: api/TbOrgUsers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTbOrgUser(string id, TbOrgUser tbOrgUser)
        {
            if (id != tbOrgUser.Uid)
            {
                return BadRequest();
            }

            _context.Entry(tbOrgUser).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TbOrgUserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/TbOrgUsers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TbOrgUser>> PostTbOrgUser(TbOrgUser tbOrgUser)
        {
          if (_context.TbOrgUsers == null)
          {
              return Problem("Entity set 'PmdbContext.TbOrgUsers'  is null.");
          }
            _context.TbOrgUsers.Add(tbOrgUser);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (TbOrgUserExists(tbOrgUser.Uid))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetTbOrgUser", new { id = tbOrgUser.Uid }, tbOrgUser);
        }

        // DELETE: api/TbOrgUsers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTbOrgUser(string id)
        {
            if (_context.TbOrgUsers == null)
            {
                return NotFound();
            }
            var tbOrgUser = await _context.TbOrgUsers.FindAsync(id);
            if (tbOrgUser == null)
            {
                return NotFound();
            }

            _context.TbOrgUsers.Remove(tbOrgUser);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TbOrgUserExists(string id)
        {
            return (_context.TbOrgUsers?.Any(e => e.Uid == id)).GetValueOrDefault();
        }
    }
}
