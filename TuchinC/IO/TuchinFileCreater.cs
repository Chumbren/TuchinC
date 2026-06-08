using System;
using System.Collections.Generic;
using System.Text;

namespace TuchinC.IO
{
    public class TuchinFileCreater(string directory, string project)
    {
        public readonly string Directory = directory;
        public readonly string Project = project;

        public void CreateByteFile(in IReadOnlyList<byte> bytes)
        {
            if (!System.IO.Directory.Exists(Directory))
                throw new DirectoryNotFoundException();

            string bobj = $"{Directory}/bobj";
            if (!System.IO.Directory.Exists(bobj))
                System.IO.Directory.CreateDirectory(bobj);

            File.WriteAllBytes($"{bobj}/{Project}.btnz", [.. bytes]);
        }


    }
}
