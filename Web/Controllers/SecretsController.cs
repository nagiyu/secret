using Microsoft.AspNetCore.Mvc;
using Service.Models;
using Service.Repositories;

namespace Web.Controllers
{
    public class SecretsController : Controller
    {
        private readonly ISecretRepository _repository;

        public SecretsController(ISecretRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Secret>>> GetSecrets()
        {
            return Ok(await _repository.GetAllAsync());
        }

        [HttpGet]
        public async Task<ActionResult<Secret>> GetSecret(int id)
        {
            var secret = await _repository.GetByIdAsync(id);
            if (secret == null)
            {
                return NotFound();
            }
            return Ok(secret);
        }

        [HttpPost]
        public async Task<ActionResult<Secret>> PostSecret([FromBody] Secret secret)
        {
            await _repository.AddAsync(secret);
            return CreatedAtAction(nameof(GetSecret), new { id = secret.Id }, secret);
        }

        [HttpPut]
        public async Task<IActionResult> PutSecret(int id, [FromBody] Secret secret)
        {
            secret.Id = id;
            await _repository.UpdateAsync(secret);
            return NoContent();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteSecret(int id)
        {
            await _repository.DeleteAsync(id);
            return NoContent();
        }
    }
}
