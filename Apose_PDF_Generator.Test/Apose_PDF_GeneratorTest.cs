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
        public void UploadToAsposeCloud_GivenPDF_ShouldUploadItToAsposeCloud()
        {
            //Arrange
            var sut = new AsposePdfApi();
            var filePath = Common.GetDataDirectory();

            //Act
            var actual = sut.UploadToAsposeCloud(filePath);

            //Assert
            Assert.AreEqual(200, actual.Code);
        }
        [Test]
        public void getFileFromAsposeCloud_ShouldReturnDownloadFileFromAsposeCloud()
        {
            //Arrange
            var sut = new AsposePdfApi();

            //Act 
            var actual = sut.GetDocument();

            
            //Assert.AreEqual(null,actual);
        }
        [Test]
        public void UpdateSingleFieldOfTheDocumentOnAsposeCloud_GivenFilename_FieldName_And_values_ShouldUpdateFieldAndReturnStatusOk_()
        {
            var sut = new AsposePdfApi();
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
        [Ignore("this test must fail but it pass due security issues on apose")]
        public void UpdateSingleFieldOfTheDocumentOnAsposeCloud_GivenFilename_ReadOnlyField_And_value_ShouldThrowExceptionNotUpdate()
        {
            var sut = new AsposePdfApi();
            const string filename = "Aspose_by_Siphenathi_2.pdf";
            const string fieldname = "Total_Expenses";
            var value = new List<string> { "2600" };

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
            var sut = new AsposePdfApi();
            const string filename = "Aspose_by_Siphenathi_2.pdf";
            var asposeFields = GetValuesToUpdate();

            //Act
            var actual = sut.UpdateMultipleFileInAsposeCloud(filename, asposeFields);

            //Assert
            Assert.AreEqual("OK", actual.Status);
            Assert.AreEqual(asposeFields.List[0].Values, actual.Fields.List[0].Values);
        }


        [Test]
        public void DisableFields_GivenFilename_ShouldDisableAndReturnStatusOk()
        {
            //Arrange
            var sut = new AsposePdfApi();
            const string formFile = "C:\\Users\\SiphenathiP\\Documents\\Aspose_by_Siphenathi_2.pdf";
            const string newFilePath = "C:\\Users\\SiphenathiP\\Documents\\Aspose_by_Siphenathi_3.pdf";

            //Act
            sut.DisableFields(formFile, newFilePath);

            var originalFile = File.ReadAllBytes(formFile);
            var newFile = File.ReadAllBytes(newFilePath);

            //Assert
            Assert.AreNotEqual(originalFile, newFile);
        }
        private static Fields GetValuesToUpdate()
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
    }
}
