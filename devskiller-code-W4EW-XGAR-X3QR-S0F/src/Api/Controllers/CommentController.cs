using System;
using System.Linq;
using System.Threading.Tasks;
using AppServices.Models;
using AppServices.Models.Creation;
using AppServices.Services;
using Ardalis.GuardClauses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Api.Controllers
{
    [ApiController]
    [Route("comments")]
    public class CommentController : ControllerBase
    {
        private readonly ICommentAppService appService;
        private readonly ILogger<CommentController> logger;

        public CommentController(ICommentAppService appService, ILogger<CommentController> logger)
        {
            this.appService = Guard.Against.Null(appService);
            this.logger = Guard.Against.Null(logger);
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

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CommentCreateDTO comment)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage));
                return new BadRequestObjectResult(errors);
            }

            var result = await appService.CreateAsync(comment);

            return Created("/", result);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Put([FromRoute] Guid id, [FromBody] CommentDTO comment)
        {
            if (id != comment.Id)
            {
                return BadRequest("The ID on Quey and PostDTO does not match.");
            }

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage));
                return new BadRequestObjectResult(errors);
            }

            var result = await appService.UpdateAsync(comment);

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
    }
}