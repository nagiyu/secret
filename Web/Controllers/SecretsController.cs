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
        [Route("/Secret/{key}")]
        public async Task<ActionResult<Secret>> GetSecret(string key)
        {
            var secret = await _repository.GetSecret(key);
            if (secret == null)
            {
                return NotFound();
            }
            return Ok(secret);
        }

        [HttpGet]
        public async Task<ActionResult<Secret>> GetSecretById(int id)
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
            return CreatedAtAction(nameof(GetSecretById), new { id = secret.Id }, secret);
        }

        [HttpPut]
        public async Task<IActionResult> PutSecret(int id, [FromBody] Secret secret)
        {
            var existingSecret = await _repository.GetByIdAsync(id);
            if (existingSecret == null)
            {
                return NotFound();
            }

            existingSecret.Key = secret?.Key;
            existingSecret.Value = secret?.Value;

            await _repository.UpdateAsync(existingSecret);
            return NoContent();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteSecret(int id)
        {
            var secret = await _repository.GetByIdAsync(id);
            if (secret == null)
            {
                return NotFound();
            }

            await _repository.DeleteAsync(id);
            return NoContent();
        }
    }
}
