using Magna.Dexsys.FileHandler.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Magna.Dexsys.FileHandler.Services;
public class FileSearchService
{
    public IReadOnlyList<FileDetails> FilesLocated => _filesLocated.AsReadOnly();

    private readonly List<FileDetails> _filesLocated = [];

    /// <summary>
    /// Populate the instance's FilesLocated member with a collection of
    /// files which contain the partialContent value anywhere in the 
    /// file.
    /// </summary>
    /// <param name="directory">Directory containing files to search</param>
    /// <param name="searchValue">Data to search for in files</param>
    /// <returns>Return the number of files located</returns>
    /// <exception cref="NotImplementedException"></exception>
    public int LocateFilesContainingSearchValue(string directory, string searchValue)
    {
        // check if string is null or empty
        if (string.IsNullOrEmpty(searchValue)) return 0;

        // keep track of number of matches found
        int count = 0;
        // create an object that can lock out other processes when we update shared resources
        object lockObj = new object();

        // loop through all the files in parallel
        Parallel.ForEach(Directory.GetFiles(directory), filePath =>
        {
            try
            {
                // read all text in files with the UTF-16 encoding
                string content = File.ReadAllText(filePath, Encoding.Unicode);

                // check if searchValue is contained in the file
                if (content.Contains(searchValue))
                {
                    // lock out other processes when updating shared resources
                    lock (lockObj)
                    {
                        count++;
                        // I don't know if I am correctly adding the last two parameters here
                        _filesLocated.Add(new FileDetails(
                            filePath,
                            Path.GetFileName(filePath),
                            searchValue.Length,
                            searchValue));
                    }
                }
            }
            catch (DirectoryNotFoundException)
            {
                Console.WriteLine("Directory not found");
            }
            catch (UnauthorizedAccessException)
            {
                Console.WriteLine("No permission to access directory");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        });

        return count;
    }

}
        

        
       
