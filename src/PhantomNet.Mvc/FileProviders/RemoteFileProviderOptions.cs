namespace PhantomNet.Mvc.FileProviders
{
    public class RemoteFileProviderOptions
    {
        public string SecretKey { get; set; }

        public double? TokenTimeOut { get; set; }

        public string EndPoint { get; set; }
    }
}
