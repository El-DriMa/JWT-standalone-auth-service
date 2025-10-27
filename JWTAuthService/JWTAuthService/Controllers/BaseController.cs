using JWTAuthService.Interfaces;
using JWTAuthService.Models.Responses;
using JWTAuthService.SearchObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JWTAuthService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController<TModel, TSearch> : ControllerBase where TSearch : BaseSearchObject
    {
        protected IService<TModel, TSearch> _service;
        protected readonly ILogger<BaseController<TModel, TSearch>> _logger;


        public BaseController(ILogger<BaseController<TModel, TSearch>> logger, IService<TModel, TSearch> service)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet("")]
        public async Task<PagedResult<TModel>> GetList([FromQuery] TSearch searchObject)
        {
            return await _service.GetPaged(searchObject);
        }

        [HttpGet("{id}")]
        public async Task<TModel?> GetById(int id)
        {
            return await _service.GetById(id);
        }

    }

}
