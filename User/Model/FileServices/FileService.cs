using Syncfusion.XlsIO;

namespace User.Model.FileServices
{
    public class FileService : IFileService
    {
        public string Open(string filename) { return ""; }
        public void Save(string filename, IApplication xlApp)
        {
            xlApp.Application.ActiveWorkbook.SaveAs(filename);
        }
    }
}
