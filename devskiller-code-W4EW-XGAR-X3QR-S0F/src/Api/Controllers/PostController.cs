using System;
using System.Linq;
using System.Threading.Tasks;
using AppServices.Models;
using AppServices.Models.Results;
using AppServices.Services;
using Ardalis.GuardClauses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Controllers
{
    [ApiController]
    [Route("posts")]
    public class PostController : ControllerBase
    {
        private readonly IPostAppService appService;
        private readonly ILogger<PostController> logger;

        public PostController(IPostAppService appService, ILogger<PostController> logger)
        {
            this.appService = Guard.Against.Null(appService);
            this.logger = Guard.Against.Null(logger);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var result = await appService.GetAllAsync();

            if (result.Content == null)
            {
                return NoContent();
            }

            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> Get([FromRoute] Guid id)
        {
            if (Guid.Empty == id)
            {
                return BadRequest("Id should not be all zeros.");
            }

            var result = await appService.GetAsync(id);

            if (result.Content == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpPost()]
        public async Task<IActionResult> Post([FromBody] PostCreateDTO post)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage));
                return new BadRequestObjectResult(errors);
            }

            var result = await appService.CreateAsync(post);

            return Created($"[GET] posts/{result?.Content.Id}", result);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Put([FromRoute] Guid id, [FromBody] PostDTO post)
        {
            if (id != post.Id)
            {
                return BadRequest("The ID on Quey and PostDTO does not match.");
            }

            var result = await appService.UpdateAsync(post);

            return Ok(result);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            if (Guid.Empty == id)
            {
                return BadRequest("Id should not be all zeros.");
            }

            await appService.DeleteAsync(id);

            return NoContent();
        }

        [HttpGet("{id:guid}/comments")]
        public async Task<IActionResult> GetComments([FromRoute] Guid id)
        {
            if (Guid.Empty == id)
            {
                return BadRequest("Id should not be all zeros.");
            }

            var result = await appService.GetComments(id);

            if (result.Content == null)
            {
                return NoContent();
            }

            return Ok(result);
        }
    }
}