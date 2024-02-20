using Adobe.PDFServicesSDK.auth;
using Adobe.PDFServicesSDK.exception;
using Adobe.PDFServicesSDK.io;
using Adobe.PDFServicesSDK.pdfops;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Adobe.PDFServicesSDK;
using ExecutionContext = Adobe.PDFServicesSDK.ExecutionContext;
using log4net.Config;
using log4net.Repository;
using log4net;
using System.Reflection;
using Adobe.PDFServicesSDK.options.exportpdftoimages;

 
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AdobePdfApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PdfController : ControllerBase
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(Program));
        public PdfController()
        {
                
        }
        // GET: api/<PdfController>
        [HttpPost]
        [Route("CreatePdfFromDocX")]
        public IActionResult CreatePdfFromDocX()
        {
           
                // Initial setup, create credentials instance.
              try{  Credentials credentials = Credentials.ServicePrincipalCredentialsBuilder()
                    .WithClientId(("5fdb1a36c64144dab522a6afd537d432"))
                    .WithClientSecret(("p8e-AMBatyTbWulPLP-WyAPYF_4VPsV6PdPs"))
                    .Build();

                //Create an ExecutionContext using credentials and create a new operation instance.
                ExecutionContext executionContext = ExecutionContext.Create(credentials);
                CreatePDFOperation createPdfOperation = CreatePDFOperation.CreateNew();

                // Set operation input from a source file.
                FileRef source = FileRef.CreateFromLocalFile(@"createPdfInput.docx");
                createPdfOperation.SetInput(source);

                // Execute the operation.
                FileRef result = createPdfOperation.Execute(executionContext);

                //Generating a file name
                String outputFilePath = CreateOutputFilePath();

                // Save the result to the specified location.
                result.SaveAs(Directory.GetCurrentDirectory() + outputFilePath);
            }
            catch (ServiceUsageException ex)
            {
                log.Error("Exception encountered while executing operation", ex);
            }
            catch (ServiceApiException ex)
            {
                log.Error("Exception encountered while executing operation", ex);
            }
            catch (SDKException ex)
            {
                log.Error("Exception encountered while executing operation", ex);
            }
            catch (IOException ex)
            {
                log.Error("Exception encountered while executing operation", ex);
            }
            catch (Exception ex)
            {
                log.Error("Exception encountered while executing operation", ex);
            }
            return StatusCode(200);
        }

        // GET api/<PdfController>/5
        [HttpPost("ExportPDFToJPEG")]
        public IActionResult ExportPDFToJPEG(int id)
        {
            ConfigureLogging();
            try
            {
                // Initial setup, create credentials instance.
                Credentials credentials = Credentials.ServicePrincipalCredentialsBuilder()
                     .WithClientId(("5fdb1a36c64144dab522a6afd537d432"))
                    .WithClientSecret(("p8e-AMBatyTbWulPLP-WyAPYF_4VPsV6PdPs"))
                    .Build();

                //Create an ExecutionContext using credentials and create a new operation instance.
                ExecutionContext executionContext = ExecutionContext.Create(credentials);
                ExportPDFToImagesOperation exportPDFToImagesOperation = ExportPDFToImagesOperation.CreateNew(ExportPDFToImagesTargetFormat.JPEG);

                // Set operation input from a source file.
                FileRef sourceFileRef = FileRef.CreateFromLocalFile(@"exportPDFToImagesInput.pdf");
                exportPDFToImagesOperation.SetInput(sourceFileRef);

                // Execute the operation.
                List<FileRef> result = exportPDFToImagesOperation.Execute(executionContext);

                //Generating a file name
                String outputFilePath = CreateOutputFilePath();

                // Save the result to the specified location.
                int index = 0;
                foreach (FileRef fileRef in result)
                {
                    fileRef.SaveAs(Directory.GetCurrentDirectory() + String.Format(outputFilePath, index));
                    index++;
                }

            }
            catch (ServiceUsageException ex)
            {
                log.Error("Exception encountered while executing operation", ex);
            }
            catch (ServiceApiException ex)
            {
                log.Error("Exception encountered while executing operation", ex);
            }
            catch (SDKException ex)
            {
                log.Error("Exception encountered while executing operation", ex);
            }
            catch (IOException ex)
            {
                log.Error("Exception encountered while executing operation", ex);
            }
            catch (Exception ex)
            {
                log.Error("Exception encountered while executing operation", ex);
            }

            return StatusCode(200);
        }

        // POST api/<PdfController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<PdfController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<PdfController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
        static void ConfigureLogging()
        {
            ILoggerRepository logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
        }

        //Generates a string containing a directory structure and file name for the output file.
        public static string CreateOutputFilePath()
        {
            String timeStamp = DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH'-'mm'-'ss");
            return ("/output/create" + timeStamp + ".pdf");
        }
    }
}
