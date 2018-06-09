using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fieldscribe_Windows_App
{
    public interface ILynxFileService
    {
        bool LynxFilesExist();

        string GetSCH();

        string GetPPL();

        string GetEVT();
    }
}
