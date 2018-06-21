using System.Collections.Generic;
using Apose_PDF_Generator.Model;
using Aspose.Pdf.Cloud.Sdk.Model;
using Aspose.Storage.Cloud.Sdk.Model;

namespace Apose_PDF_Generator.Boundary
{
    public interface IAsposePdfApi
    {
        UploadResultsTransferObject UploadToAsposeCloud(string path);
        DownloadResultsTransferObject DownloadDocumentFromTheCloud(string name, string path);
        FieldResponse UpdateSingleFieldOfTheDocumentOnAsposeCloud(string fileName, string fieldName, List<string> value);
        FieldsResponse UpdateMultipleFileInAsposeCloud(string fileName, Fields asposeFields);
        void DisableFields(string originalFilePath, string newFile, List<string> fieldsToDisable);
        RemoveFileResponse DeleteFileInCloud(string fileName);


    }
}
