#nullable enable

namespace pwm.Core.Crypto
{
    public interface ICryptographyLogic
    {
        string ToHash(string data);

        string EnCryptography(string textData, string password);

        string DeCryptography(string base64Data, string password);
    }
}
