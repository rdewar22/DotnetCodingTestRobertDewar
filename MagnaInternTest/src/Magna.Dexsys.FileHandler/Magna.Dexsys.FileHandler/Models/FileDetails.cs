using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magna.Dexsys.FileHandler.Models;
public class FileDetails
{
    public string Path { get; set; }
    public string Name { get; set; }
    public int? ContentLength { get; set; }
    public string? Content { get; set; }

    public FileDetails(string path, string name)
    {
        Path = path;
        Name = name;
    }

    public FileDetails(string path, string name, int contentLength, string content) : this(path, name)
    {
        ContentLength = contentLength;
        Content = content;
    }
}
