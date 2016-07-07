namespace PhantomNet.Mvc.FileProviders
{
    public interface IApiTokenProvider
    {
        void GenerateToken(string secretKey, string command, double timeOut, out string timeStamp, out string token);

        bool ValidateToken(string secretKey, string command, string timeStamp, string token);
    }
}
