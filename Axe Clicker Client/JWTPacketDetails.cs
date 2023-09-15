
public class JWTPacketDetails
{
    public int nbf { get; set; } = 0;
    public int exp { get; set; } = 0;
    public string iss { get; set; } = string.Empty;
    public string aud { get; set; } = string.Empty;
}
