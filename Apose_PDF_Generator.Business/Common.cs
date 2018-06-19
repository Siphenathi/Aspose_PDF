using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Apose_PDF_Generator.Service
{
    public class Common
    {
        public static string GetDataDirectory()
        {
            var path = AppDomain.CurrentDomain.BaseDirectory + "\\Documents\\Aspose_by_Siphenathi_2.pdf"; 
            return path;
        }
    }
}
