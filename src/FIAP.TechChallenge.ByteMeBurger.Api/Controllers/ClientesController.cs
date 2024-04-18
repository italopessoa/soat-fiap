using FIAP.TechChallenge.ByteMeBurger.Application.UseCases;
using FIAP.TechChallenge.ByteMeBurger.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace FIAP.TechChallenge.ByteMeBurger.Api.Controllers
{
    public class ClientesController : ControllerBase
    {
        private readonly IClienteUseCase _clienteUseCase;

        public ClientesController(IClienteUseCase clienteUseCase)
        {
            _clienteUseCase = clienteUseCase;
        }

        [HttpGet]
        public IActionResult Get(string cpf)
        {
            return Ok(_clienteUseCase.GetByCpf(cpf));
        }


        [HttpPost]
        public IActionResult Post([FromBody] Cliente cliente)
        {
            throw new NotImplementedException();
        }
    }
}
