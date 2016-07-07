namespace PhantomNet.Mvc.RemoteFolder
{
    public class RemoteFolderErrorDescriber : ErrorDescriber
    {
        public virtual GenericError FileSystemError(string exception)
        {
            return new GenericError {
                Code = nameof(FileSystemError),
                Description = exception
            };
        }

        public virtual GenericError NoFileToUpload()
        {
            return new GenericError {
                Code = nameof(NoFileToUpload),
                Description = Resources.NoFileToUploadError
            };
        }

        public virtual GenericError FileNotFound(string fileName)
        {
            return new GenericError {
                Code = nameof(FileNotFound),
                Description = string.Format(Resources.FileNotFoundError, fileName)
            };
        }
    }
}
