using Domain.Commands;
using Domain.EmailTemplates;
using Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class EmailsController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Send([FromServices] ISmtpService smtpService, [FromForm]SendEmailCommand command)
        {
            /** Note:
             * Templates are optionals. You can send your body message directly
             */
            var template = new BasicTemplate
            {
                Subject = command.Subject,
                Message = command.Body,
                SentAt = DateTime.Now
            };

            command.Body = template.Build();

            bool isSuccess = await smtpService.SendEmailAsync(command);

            return Ok(isSuccess);
        }
    }
}
