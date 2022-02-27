namespace MonoGame.Randomchaos.Services.Interfaces
{
    public interface IEncryptionService
    {
        byte[] Encrypt(string data);
        string Decrypt(byte[] data);
    }
}
