using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Apose_PDF_Generator.Service
{
    public class FileRouteFinder
    {
        public static string GetDirectoryOfTheMainDocument()
        {
            var path = AppDomain.CurrentDomain.BaseDirectory + "Documents\\Aspose_by_Siphenathi_2.pdf"; 
            return path;
        }
        public static string GetDirectoryToStoreTheDocumentFromTheCloud()
        {
            var path = AppDomain.CurrentDomain.BaseDirectory + "Documents\\Document.pdf";
            return path;                        
        }

        public static string GetDirectoryToStoreTheDocumentWithDisableProperties()
        {
            var path = AppDomain.CurrentDomain.BaseDirectory + "Documents\\Aspose_by_Siphenathi_2(Readonly).pdf";
            return path;
        }
    }
}