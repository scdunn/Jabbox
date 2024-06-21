using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Jabbox.API.Models;
using Jabbox.Data.Interfaces;
using Microsoft.Extensions.Options;

namespace Jabbox.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AccountsController : BaseController
    {
        private readonly TokenHandler _tokenHandler;    // Handles Tokens

        public AccountsController(IUnitOfWork unitOfWork, IMapper mapper, IOptions<AppSettings> appSettings, TokenHandler tokenHandler) : base(unitOfWork, mapper, appSettings)
        { 
            _tokenHandler = tokenHandler;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] AuthLoginDTO loginModel)
        {
            // find user
            var account = _unitOfWork.Accounts.Login(loginModel.Username, loginModel.Password);

            // user not found or password incorrect
            if (account == null)
                return Unauthorized(new AuthResponseDTO { ErrorMessage = "Invalid Authentication" });

            // successful login, return token for account
            return Ok(new AuthResponseDTO { UserName=account.Username, IsAuthSuccessful = true, Token = _tokenHandler.GetToken(account) });
        }



        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] AuthRegisterDTO registerModel)
        {
            // create a new account
            var account = _unitOfWork.Accounts.Register(registerModel.Username, registerModel.Password);
            _unitOfWork.SaveChanges();

            return Ok(new AuthResponseDTO { UserName = account.Username, IsAuthSuccessful = true, Token = _tokenHandler.GetToken(account) });
        }


    }
}
