using NGOAutomationAPI.DTOs;

namespace NGOAutomationAPI.Services
{
	public interface IAuthService
	{
		Task<string> Register(UserRegisterDto userRegisterDto);
		Task<string> Login(UserLoginDto userLoginDto);
	}
}
