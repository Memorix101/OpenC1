using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using OneAmEngine;

namespace OpenC1.Parsers
{

    class SettingsFile : BaseTextFile
    {
        public SettingsFile()
            : base(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "OpenC1Settings.cfg"))
        {
            GameVars.DrawDistance = ReadLineAsInt() * 10;
            GameVars.FullScreen = ReadLineAsBool();
            
            CloseFile();
        }
    }
}
