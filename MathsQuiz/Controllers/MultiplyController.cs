using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace MathsQuiz.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MultiplyController : Controller
    {
        /// <summary>
        /// Multiply two numbers.
        /// </summary>
        /// <param name="x">The first number.</param>
        /// <param name="y">The second number.</param>
        /// <returns></returns>
        [HttpGet]
        [SwaggerResponse(Status200OK, "The result of the multiplication is returned.")]
        [SwaggerResponse(Status400BadRequest, "The inputs are invalid.", typeof(ValidationProblemDetails))]
        [SwaggerResponse(Status500InternalServerError, "An unexplained error has occurred.", typeof(ProblemDetails))]
        [Route("")]
        public ActionResult<int> Get(
            [Required, Range(-32768, 32767)]
            int x,
            [Required, Range(-32768, 32767)]
            int y)
        {
            checked
            {
                return x * y;
            }
        }
    }
}
