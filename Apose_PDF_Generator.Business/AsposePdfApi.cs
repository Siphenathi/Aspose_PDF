using System.Collections.Generic;
using System.IO;
using Aspose.Pdf.Cloud.Sdk.Api;
using Aspose.Pdf.Cloud.Sdk.Model;
using Aspose.Storage.Cloud.Sdk.Api;
using Aspose.Storage.Cloud.Sdk.Model;
using Aspose.Storage.Cloud.Sdk.Model.Requests;
using iTextSharp.text.pdf;

namespace Apose_PDF_Generator.Service
{
    public class AsposePdfApi
    {
        protected const string APIKEY = "10311b1bb7f25457692006a82bd78856";
        protected const string APPSID = "14024f73-3bde-492c-aab1-7bea7de60548";

        private readonly StorageApi _storageApi = new StorageApi(APIKEY, APPSID);
        private readonly PdfApi _target = new PdfApi(APIKEY, APPSID);
        public AsposePdfApi()
        {

        }

        public UploadResponse UploadToAsposeCloud(string path)
        {
            var name = GetFileName(path);
            dynamic response;

            using (var stream = new FileStream(path, FileMode.Open))
            {
                var request = new PutCreateRequest(name, stream);
                response = _storageApi.PutCreate(request);
            }
            return response;
        }

        public Stream GetDocument()
        {
             const string name = "Aspose_by_Siphenathi_2.pdf";
  
            var response = _target.GetDocument(name, null, null);

            const string fileFullPath = "J:\\PDF from the cloud";

            var fileStream = File.Create(fileFullPath, (int)response.Length);

            // Initialize the bytes array with the stream length and then fill it with data
            var bytesInStream = new byte[response.Length];
            response.Read(bytesInStream, 0, bytesInStream.Length);

            // Use write method to write to the file specified above
            fileStream.Write(bytesInStream, 0, bytesInStream.Length);


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

        public void DisableFields(string originalFilePath, string newFile)
        {
            var reader = new PdfReader(originalFilePath);

            using (var stamper = new PdfStamper(reader, new FileStream(newFile, FileMode.Create)))
            {
                var fields = stamper.AcroFields;

                fields.SetFieldProperty("Date_Of_Birth", "setfflags", PdfFormField.FF_READ_ONLY, null);
                fields.SetFieldProperty("First_Name", "setfflags", PdfFormField.FF_READ_ONLY, null);
                fields.SetFieldProperty("Surname", "setfflags", PdfFormField.FF_READ_ONLY, null);

                stamper.Close();
            }
        }
    }
}


    
