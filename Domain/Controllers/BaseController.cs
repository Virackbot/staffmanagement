using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace Domain.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BaseController : ControllerBase
    {
        public BaseController()
        {

        }
        readonly IMapper? _mapper;
        protected readonly IServiceProvider? _provider;
        public BaseController(IMapper mapper, IServiceProvider provider)
        {
            _mapper = mapper;
            _provider = provider;
        }

        protected async Task<IActionResult> MapAsync<TSource, TDest>(Func<Task<TSource>> @delegate)
        {
            try
            {
                if (_mapper is null)
                {
                    throw new Exception("Mapper not provide.");
                }
                var result = _mapper.Map<TSource, TDest>(await @delegate.Invoke());
                return Ok(result);
            }
            catch (Exception except)
            {
                throw new Exception(except.Message + "\n\r" + except.StackTrace);
            }
        }
    }
}
