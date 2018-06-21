using System.Collections.Generic;
using System.Configuration;
using System.IO;
using Apose_PDF_Generator.Model;
using Aspose.Pdf.Cloud.Sdk.Api;
using Aspose.Pdf.Cloud.Sdk.Model;
using Aspose.Storage.Cloud.Sdk.Api;
using Aspose.Storage.Cloud.Sdk.Model;
using Aspose.Storage.Cloud.Sdk.Model.Requests;
using iTextSharp.text.pdf;
using RestSharp.Extensions;

namespace Apose_PDF_Generator.Service
{
    public class AsposePdfApi : IAsposePdfApi
    {
        private readonly StorageApi _storageApi;
        private readonly PdfApi _target;
        public AsposePdfApi()
        {
            var apiKey = ConfigurationManager.AppSettings["_apiKey"];
            var appSid = ConfigurationManager.AppSettings["_appSid"];
            _storageApi = new StorageApi(apiKey, appSid);
            _target = new PdfApi(apiKey, appSid);

        }
        public UploadResultsTransferObject UploadToAsposeCloud(string path)
        {
            var results = new UploadResultsTransferObject();
            if (Invalid(path))
            {
                results.ErrorMessage = "Invalid file path";
                return results;
            }
            var name = GetFileName(path);

            if (FileDoesNotExists(path))
            {
                results.ErrorMessage = "File path you looking for does not exist, check path name and try again";
                return results;
            }

            using (var stream = new FileStream(path, FileMode.Open))
            {
                var request = new PutCreateRequest(name, stream);
                results.UploadResults = _storageApi.PutCreate(request);
            }
            return results;
        }
        public DownloadResultsTransferObject DownloadDocumentFromTheCloud(string name, string path)
        {
            var results = new DownloadResultsTransferObject();

            if (Invalid(name))
            {
                results.ErrorMessage = "Invalid file name";
                return results;
            }
            var request = new GetDownloadRequest(name);
            using (var response = _storageApi.GetDownload(request))
            {
                if (response == null)
                {
                    results.ErrorMessage = "File you looking for does not exist, check filename and try again";
                    return results;
                }
                results.Bytes = response.ReadAsBytes();
                File.WriteAllBytes(path, results.Bytes);
            }

            return results;
        }
        public FieldResponse UpdateSingleFieldOfTheDocumentOnAsposeCloud(string fileName, string fieldName, List<string> value)
        {
            var body = new Field
            {
                Name = fieldName,
                Values = value
            };
            var apiResponse = _target.PutUpdateField(fileName, fieldName, body);

            return apiResponse;
        }
        public FieldsResponse UpdateMultipleFileInAsposeCloud(string fileName, Fields asposeFields)
        {
            var apiResponse = _target.PutUpdateFields(fileName, asposeFields);
            return apiResponse;
        }
        public void DisableFields(string originalFilePath, string newFile, List<string> fieldsToDisable)
        {
            var reader = new PdfReader(originalFilePath);

            using (var stamper = new PdfStamper(reader, new FileStream(newFile, FileMode.Create)))
            {
                SetToReadonly(fieldsToDisable, stamper);
                stamper.Close();
            }
        }
        public RemoveFileResponse DeleteFileInCloud(string fileName)
        {
            var request = new DeleteFileRequest(fileName);
            var response = _storageApi.DeleteFile(request);
            return response;
        }
        private static string GetFileName(string path)
        {
            var startPoint = 0;
            for (var i = 0; i < path.Length; i++)
            {
                if (path[i].ToString().Equals("\\"))
                {
                    startPoint = i;
                }
            }
            var name = path.Substring(startPoint + 1);
            return name;
        }
        private static bool Invalid(string path)
        {
            return string.IsNullOrWhiteSpace(path);
        }
        private static bool FileDoesNotExists(string path)
        {
            return !File.Exists(path);
        }
        private static void SetToReadonly(IEnumerable<string> fieldsToDisable, PdfStamper stamper)
        {
            var fields = stamper.AcroFields;

            foreach (var field in fieldsToDisable)
            {
                fields.SetFieldProperty(field, "setfflags", PdfFormField.FF_READ_ONLY, null);
            }

        }
        
    }
}



