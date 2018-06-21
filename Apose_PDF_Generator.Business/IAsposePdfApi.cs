using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Apose_PDF_Generator.Model;
using Aspose.Pdf.Cloud.Sdk.Model;
using Aspose.Storage.Cloud.Sdk.Model;

namespace Apose_PDF_Generator.Service
{
    public interface IAsposePdfApi
    {
        UploadResultsModel UploadToAsposeCloud(string path);
        DownloadResultsModel DownloadDocumentFromTheCloud(string name, string path);
        FieldResponse UpdateSingleFieldOfTheDocumentOnAsposeCloud(string fileName, string fieldName, List<string> value);
        FieldsResponse UpdateMultipleFileInAsposeCloud(string fileName, Fields asposeFields);
        void DisableFields(string originalFilePath, string newFile, List<string> fieldsToDisable);
        RemoveFileResponse DeleteFileInCloud(string fileName);


    }
}
