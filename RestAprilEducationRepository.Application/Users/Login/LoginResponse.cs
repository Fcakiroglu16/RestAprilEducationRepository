namespace RestAprilEducationRepository.Application.Users.Login
{
    public record LoginResponse(string Token, DateTime Expiration);
}
