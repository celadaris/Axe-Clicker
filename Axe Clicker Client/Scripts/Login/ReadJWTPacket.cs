using Newtonsoft.Json;
using System;
using System.Text;

public class ReadJWTPacket
{
    //https://stackoverflow.com/a/26354677
    public JWTPacketDetails JWTConverter(string jwt)
    {
        string jwtPacket = jwt.Split('.')[1];

        string incoming = jwtPacket
    .Replace('_', '/').Replace('-', '+');
        switch (jwtPacket.Length % 4)
        {
            case 2: incoming += "=="; break;
            case 3: incoming += "="; break;
        }
        byte[] bytes = Convert.FromBase64String(incoming);
        string jsonText = Encoding.ASCII.GetString(bytes);
        return JsonConvert.DeserializeObject<JWTPacketDetails>(jsonText);
    }
}
