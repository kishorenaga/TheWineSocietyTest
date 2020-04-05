using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheWineSociety.FunctionalTests.Core
{
    public class FileReader
    {

        public FileReader()
        {

        }
        public string getCurrentTestResult()
        {
            Environment.CurrentDirectory = AppDomain.CurrentDomain.BaseDirectory;
            return Environment.CurrentDirectory = Directory.GetDirectories("../../Resources")[2];

        }
        public string getCurrentDriverPath()
        {
            Environment.CurrentDirectory = AppDomain.CurrentDomain.BaseDirectory;
           
            return Environment.CurrentDirectory = Directory.GetDirectories("../../Resources")[0];

        }

        public string getProjectRootPath()
        {
            String rootPath = System.IO.Directory.GetCurrentDirectory().ToString();
            return rootPath;
        }
        
        public string getScreenShotsPath()
        {
            String env = System.IO.Directory.GetCurrentDirectory().ToString();
            String strgroupids = env.Remove(env.Length - 49);
            String newPath = strgroupids + @"\ScreenShots\";           
            return newPath;
        }
                    
        public string readFile(string env)
        {
            Environment.CurrentDirectory = AppDomain.CurrentDomain.BaseDirectory;

            Environment.CurrentDirectory = Directory.GetDirectories("../../Resources")[1];

            string path = Environment.CurrentDirectory + "\\" + env + "_ENV.properties";
            // This text is added only once to the file.
            if (!File.Exists(path))
            {
                // Create a file to write to.
                string createText = "Hello and Welcome" + Environment.NewLine;
                File.WriteAllText(path, createText);
            }
            return path;
        }
       

       
       


    }
}
