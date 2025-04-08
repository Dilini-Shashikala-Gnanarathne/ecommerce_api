
public interface IAuthService
{
    LoginResponse Login(LoginRequest request);
    RegisterResponse Register(RegisterRequest request);
}