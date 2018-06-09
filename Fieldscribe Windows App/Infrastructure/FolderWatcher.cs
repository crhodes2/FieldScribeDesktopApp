using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fieldscribe_Windows_App
{
    internal class FolderWatcher
    {
        private static FolderWatcher _instance = null;
        private FileSystemWatcher _watcher = null;

        internal FileSystemWatcher FSWatcher { get { return _watcher; } }

        private static readonly object padlock = new object();


        private FolderWatcher()
        {
            _watcher = new FileSystemWatcher();
            _watcher.NotifyFilter = NotifyFilters.LastWrite;         
        }

        internal void WatchDirectory(string dir, string filter = "*.*")
        {
            try
            {
                _watcher.Path = dir;
                _watcher.Filter = filter;
                _watcher.EnableRaisingEvents = true;
            }
            catch(Exception e)
            {

            }
        }

        public static FolderWatcher Instance
        {
            get
            {
                lock(padlock)
                {
                    if (_instance == null)
                    {
                        _instance = new FolderWatcher();
                    }
                    return _instance;
                }
            }
        }
    }
}
