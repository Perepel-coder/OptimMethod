using Syncfusion.XlsIO;

namespace User.Model.FileServices
{
    public interface IFileService
    {
        string Open(string filename);
        void Save(string filename, IApplication FileArray);
    }
}
