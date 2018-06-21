using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apose_PDF_Generator.Model
{
    public class DownloadResultsTransferObject
    {
        public byte[] Bytes { get; set; }
        public string ErrorMessage { get; set; }
    }
}
