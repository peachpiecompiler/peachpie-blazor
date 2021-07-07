using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Peachpie.Blazor
{
    /// <summary>
    /// The class provides a file management using the Javascript interoperability. 
    /// </summary>
    public class FileManager
    {
        private PHPModule _module;
        private ILogger<FileManager> _logger;

        private List<FormFile> _fetched;
        private Dictionary<int, string> _downloaded;

        public FileManager(IPHPService phpService, ILoggerFactory factory)
        {
            _module = phpService.GetModule();
            _logger = factory.CreateLogger<FileManager>();

            _fetched = new List<FormFile>();
            _downloaded = new Dictionary<int, string>();
        }

        /// <summary>
        /// Gets uploaded files by an HTML form.
        /// </summary>
        public List<FormFile> FetchFiles()
        {
            if (_module.isFilesPresented())
            {
                var files = _module.GetFiles();
                foreach (var file in files)
                {
                    _fetched.Add(file);
                }
            }

            return _fetched;
        }

        /// <summary>
        /// Load file contents into memory.
        /// </summary>
        public async Task DownloadFilesAsync()
        {
            if (_fetched.Count == 0)
                return;

            foreach (var item in _fetched)
            {
                Log.DownloadFile(_logger, item);
                _downloaded.Add(item.id, await _module.ReadFileContentAsBase64(item.id));
            }

            _fetched = new List<FormFile>();
        }

        /// <summary>
        /// Gets file content saved in memory.
        /// </summary>
        /// <param name="id"></param>
        public string GetFileData(int id)
        {
            if (_downloaded.TryGetValue(id, out string result))
                return result;
            else
                return null;
        }   
    }
}
