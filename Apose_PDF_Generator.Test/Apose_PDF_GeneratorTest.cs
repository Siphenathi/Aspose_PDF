using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Apose_PDF_Generator.Service;
using Aspose.Pdf.Cloud.Sdk.Model;
using NUnit.Framework;

namespace Apose_PDF_Generator.Test
{
    [TestFixture]
    public class AposePdfGeneratorTest
    {
        [Test]
        public void UploadToAsposeCloud_GivenPDFPath_ShouldUploadItToAsposeCloud()
        {
            //Arrange
            var sut = GreateAsposeApi();
            var filePath = Common.GetDataDirectory();

            //Act
            var actual = sut.UploadToAsposeCloud(filePath);

            //Assert
            Assert.AreEqual(200, actual.UploadResults.Code);
        }

        [Test]
        public void UploadToAsposeCloud_GivenNonExistingFilePath_ShouldReturnFileDoesNotExist()
        {
            //Arrange
            var sut = GreateAsposeApi();
            var filePath = @"C:\Users\SiphenathiP\source\repos\Apose_PDF_Generator\Apose_PDF_Generator.Test\bin\Debug\Documentz\Aspose_by_Siphenathi_2.pdf"; 
            var expectedMessage = "File path you looking for does not exist, check path name and try again";

            //Act
            var actual = sut.UploadToAsposeCloud(filePath);

            //Assert
            Assert.AreEqual(expectedMessage, actual.ErrorMessage);
        }

        [TestCase("")]
        [TestCase(" ")]
        [TestCase(null)]
        public void UploadToAsposeCloud_GivenInvalidPDFpath_ShouldUploadItToAsposeCloud(string filePath)
        {
            //Arrange
            var sut = GreateAsposeApi();
            var expectedMessage = "Invalid file path";

            //Act
            var actual = sut.UploadToAsposeCloud(filePath);

            //Assert
            Assert.AreEqual(expectedMessage, actual.ErrorMessage);
        }

        [Test]
        public void DownloadDocumentFromTheCloud_GivenFileName_ShouldDownloadFileFromAsposeCloud()
        {
            //Arrange
            var sut = GreateAsposeApi();
            const string fileName = "Aspose_by_Siphenathi_2.pdf";
            var downloadedPdf = Common.GetDirectoryToStoreTheDocumentFromTheCloud();

            //Act 
            var actual = sut.DownloadDocumentFromTheCloud(fileName, downloadedPdf);

            var bytesOfDownloadedFile = File.ReadAllBytes(downloadedPdf);


            Assert.AreEqual(bytesOfDownloadedFile, actual.Bytes);
        }

        [TestCase("")]
        [TestCase(" ")]
        [TestCase(null)]
        public void DownloadDocumentFromTheCloud_GivenInvalidFileName_ShouldReturnInvalidFileName(string fileName)
        {
            //Arrange
            var sut = GreateAsposeApi();
            const string expected = "Invalid file name";

            //Act 
            var actual =  sut.DownloadDocumentFromTheCloud(fileName, Common.GetDirectoryToStoreTheDocumentFromTheCloud());
            
            Assert.AreEqual(expected, actual.ErrorMessage);
        }

        [Test]
        public void DownloadDocumentFromTheCloud_GivenFileNameNotExistingInCloud_ShouldReturnFileDoesNotExist()
        {
            //Arrange
            var sut = GreateAsposeApi();
            const string fileName = "File.pdf";
            const string expected = "File you looking for does not exist, check filename and try again";

            //Act 
            var actual = sut.DownloadDocumentFromTheCloud(fileName, Common.GetDirectoryToStoreTheDocumentFromTheCloud());


            Assert.AreEqual(expected, actual.ErrorMessage);
        }

        [Test]
        public void UpdateSingleFieldOfTheDocumentOnAsposeCloud_GivenFilename_FieldName_And_values_ShouldUpdateFieldAndReturnStatusOk_()
        {
            var sut = GreateAsposeApi();
            const string filename = "Aspose_by_Siphenathi_2.pdf";
            const string fieldname = "First_Name";
            var value = new List<string> { "Aspose" };

            //Act
            var actual = sut.UpdateSingleFieldOfTheDocumentOnAsposeCloud(filename,fieldname,value);
            
            

            //Assert
            Assert.AreEqual("OK", actual.Status);
            Assert.AreEqual(value.ElementAt(0), actual.Field.Values.ElementAt(0));
        }

        [Test]
        [Ignore("This test is expected to fail but it's passing due to Api failing to recognize readonly fields")]
        public void UpdateSingleFieldOfTheDocumentOnAsposeCloud_GivenFilename_ReadOnlyField_And_value_ShouldThrowExceptionNotUpdate()
        {
            var sut = GreateAsposeApi();
            const string filename = "Aspose_by_Siphenathi_2.pdf";
            const string fieldname = "Total_Net_Income";
            var value = new List<string> { "Value" };

            //Act
            var actual = sut.UpdateSingleFieldOfTheDocumentOnAsposeCloud(filename, fieldname, value);

            //Assert
            Assert.AreEqual("OK", actual.Status);
            Assert.AreEqual(value.ElementAt(0), actual.Field.Values.ElementAt(0));
        }

        [Test]
        public void UpdateMultipleFieldsOfTheDocumentOnAsposeCloud_GivenFilename_And_values_ShouldUpdateFieldAndReturnStatusOk_()
        {
            //Arrange
            var sut = GreateAsposeApi();
            const string filename = "Aspose_by_Siphenathi_2.pdf";
            var asposeFields = GetFieldsAndValuesToUpdate();

            //Act
            var actual = sut.UpdateMultipleFileInAsposeCloud(filename, asposeFields);

            //Assert
            Assert.AreEqual("OK", actual.Status);
            Assert.AreEqual(asposeFields.List[0].Values, actual.Fields.List[0].Values);
        }

        [Test]
        public void DisableFields_GivenFilenames_ShouldDisableFieldsAndReturnStatusOk()
        {
            //Arrange
            var sut = GreateAsposeApi();
            const string formFile = @"C:\Users\SiphenathiP\source\repos\Apose_PDF_Generator\Apose_PDF_Generator.Test\bin\Debug\Documents\Aspose_by_Siphenathi_2.pdf";
            const string newFilePath = @"C:\Users\SiphenathiP\source\repos\Apose_PDF_Generator\Apose_PDF_Generator.Test\bin\Debug\Documents\Aspose_by_Siphenathi_2(Readonly).pdf";

            var fieldsToDisable = new List<string> { "Date_of_Birth", "First_Name", "Surname" };

            //Act
            sut.DisableFields(formFile, newFilePath, fieldsToDisable);

            var originalFile = File.ReadAllBytes(formFile);
            var newFile = File.ReadAllBytes(newFilePath);

            //Assert
            Assert.AreNotEqual(originalFile, newFile);
        }

        [Test]
        public void DeleteFileInCloud_GivenFileName_ShouldDeleteFileAndReturnStatusOk()
        {
            //Arrange
            var fileName = "Aspose_by_Siphenathi_3.pdf";
            var sut = GreateAsposeApi();

            //Act
            var actual = sut.DeleteFileInCloud(fileName);

            //Assert
            Assert.AreEqual("OK",actual.Status);
        }

        private static Fields GetFieldsAndValuesToUpdate()
        {
            return new Fields
            {
                List = new List<Field>
                {
                    new Field
                    {
                        Name = "First_Name",
                        Values = new List<string>
                        {
                            "Bob"
                        }
                    },
                    new Field
                    {
                        Name = "Surname",
                        Values = new List<string>
                        {
                            "Pantshwa"
                        }
                    }
                }
            };
        }
        private static IAsposePdfApi GreateAsposeApi()
        {
            return new AsposePdfApi();
        }
    }

}
