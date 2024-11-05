using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.AspNetCore.Http;
using System.Net.Http;
using System.Net;
using Microsoft.Net.Http.Headers;
using PreadmissionDTOs;
using corewebapi18072016.Delegates;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage;
using DataAccessMsSqlServerProvider;

namespace corewebapi18072016.Controllers
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class InstituteImageUploadController : Controller
    {

        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IHttpContextAccessor _accessor;
        private readonly DomainModelMsSqlServerContext _context;
        // private static FacadeUrl _config;

        //string accountname = "dcampusstrg";
        //string accesskey = "DqwhzgBbVF53spoTwJrPQWMMVaKbban9Ls+y73xceTeCZKAHV4C+C2x1H7R4ltGn9thflAX+6ARlwzjx7w4XNw==";
        string accountname = "";
        string accesskey = "";

        InstitutionDelegate sad = new InstitutionDelegate();
        public InstituteImageUploadController(IHostingEnvironment hostingEnvironment, IHttpContextAccessor httpContextAccessor, DomainModelMsSqlServerContext context)
        {
            _hostingEnvironment = hostingEnvironment;
            _accessor = httpContextAccessor;
            _context = context;
        }

        public async Task<string> uploadtoazure(IFormFile file, string containername, string folder)
        {
            string newImageNamePath = "";
            string newImageName = "";
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            try
            {
                var data = _context.IVRM_Storage_path_Details.ToList();
                if (data.Count() > 0)
                {
                    accountname = data.FirstOrDefault().IVRM_SD_Access_Name;
                    accesskey = data.FirstOrDefault().IVRM_SD_Access_Key;
                }

                StorageCredentials cre = new StorageCredentials(accountname, accesskey);
                CloudStorageAccount acc = new CloudStorageAccount(cre, useHttps: true);
                // Create a blob client.
                CloudBlobClient blobClient = acc.CreateCloudBlobClient();

                // Get a reference to a container 
                CloudBlobContainer container = blobClient.GetContainerReference(containername);
                string fileExt = "";
                if (file.ContentType == "image/jpeg")
                {
                    fileExt = ".jpg";
                }
                if (file.ContentType == "application/pdf")
                {
                    fileExt = ".pdf";
                }
                if (file.ContentType == "application/kswps")
                {
                    fileExt = ".pdf";
                }
                else if (file.ContentType == "image/png")
                {
                    fileExt = ".png";
                }
                else if (file.ContentType == "video/mp4")
                {
                    fileExt = ".mp4";
                }
                //else if (file.ContentType == "video/x-ms-wmv")
                //{
                //    fileExt = ".wmv";
                //}
                //string fileExt = ".jpg";
                var ext = fileExt.ToLower();
                var guid = Guid.NewGuid().ToString();
                newImageName = guid + ext;

                //string blobName = Path.GetFileName(ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"'));
                string blobName = newImageName;
                await container.CreateIfNotExistsAsync();
                await container.SetPermissionsAsync(new BlobContainerPermissions
                {
                    PublicAccess = BlobContainerPublicAccessType.Blob
                });
                //Get reference to a blob

                var blockBlob = container.GetBlockBlobReference(mid + "/" + folder + "/" + blobName);
                //var fileStr = file.OpenReadStream();
                //await blockBlob.UploadFromStreamAsync(ss);
                //ss.Dispose();
                using (var fileStream = file.OpenReadStream())
                {
                    await blockBlob.UploadFromStreamAsync(fileStream);
                }
                newImageNamePath = blockBlob.Uri.ToString();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return newImageNamePath;
        }




        [Route("UploadInstitutionBackgroundImage")]
        public async Task<string> UploadFiles(FileDescriptionDTO fileDescriptionShort)
        {
            var names = new List<string>();
            var contentTypes = new List<string>();
            // int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));

            string newImageName = "";
            //string fp = null;
            InstitutionDTO stu = new InstitutionDTO();
            if (ModelState.IsValid)
            {
                foreach (var file in fileDescriptionShort.File)
                {
                    try
                    {
                        string containername = "files";
                        newImageName = await uploadtoazure(file, containername, "InstitutionBackgroundImage");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    //if (file.Length > 0)
                    //{
                    //    using (var reader = new StreamReader(file.OpenReadStream()))
                    //    {
                    //        var fileContent = reader.ReadToEnd();
                    //        var parsedContentDisposition = ContentDispositionHeaderValue.Parse(file.ContentDisposition);
                    //        Console.WriteLine(fileContent.ToString());
                    //    }

                    //   // var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    //    var fileName = string.Format("{0}", Guid.NewGuid(), file.ContentType);

                    //    string fileExt = "jpg";
                    //    if (file.ContentType == "image/png") { fileExt = "png"; }
                    //    else if (file.ContentType == "image/gif") { fileExt = "gif"; }
                    //    else if (file.ContentType == "image/jpeg") { fileExt = "jpg"; }
                    //    //contentTypes.Add(file.ContentType);
                    //    names.Add(fileName);
                    //    string ss = file.ContentDisposition.ToString();

                    //    Console.WriteLine(ss);
                    //    string webRootPath = _hostingEnvironment.WebRootPath;
                    //    var uploads = Path.Combine(webRootPath, "images\\uploads\\InstituteBackgroundImage\\");
                    //    // set variable as Parent directory I do this to make sure the path exists if not
                    //    // I will create the directory.
                    //    var directoryInfo = new FileInfo(uploads).Directory;

                    //    if (directoryInfo != null)
                    //        directoryInfo.Create();

                    //    var filePath = uploads + fileName+ "." + fileExt;
                    //    file.CopyTo(new FileStream(filePath, FileMode.Create));
                    //    fp = filePath;

                    //    fp =  fp.Substring(fp.IndexOf("images"));
                    //    stu.MI_Id = fileDescriptionShort.Id;
                    //    stu.MI_BackgroundImage = fp;
                    //sad.saveInstituteBackgroundImagedetails(stu);
                }
            }
            return newImageName;
        }


        // Instituion Logo upload
        [Route("UploadInstitutionLogo")]
        public async Task<string> UploadInstitutionLogo(FileDescriptionDTO fileDescriptionShort)
        {
            var names = new List<string>();
            var contentTypes = new List<string>();
            string newImageName = "";
            List<string> FilesPaths = new List<string>();
            //  string fp = null;
            InstitutionDTO stu = new InstitutionDTO();
            if (ModelState.IsValid)
            {
                foreach (var file in fileDescriptionShort.File)
                {
                    try
                    {
                        string containername = "files";
                        newImageName = await uploadtoazure(file, containername, "InstitutionLogo");
                        FilesPaths.Add(newImageName);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    //if (file.Length > 0)
                    //{
                    //    using (var reader = new StreamReader(file.OpenReadStream()))
                    //    {
                    //        var fileContent = reader.ReadToEnd();
                    //        var parsedContentDisposition = ContentDispositionHeaderValue.Parse(file.ContentDisposition);
                    //        Console.WriteLine(fileContent.ToString());
                    //    }

                    //   // var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    //    var fileName = string.Format("{0}", Guid.NewGuid(), file.ContentType);
                    //    string fileExt = "jpg";
                    //    if (file.ContentType == "image/png") { fileExt = "png"; }
                    //    else if (file.ContentType == "image/gif") { fileExt = "gif"; }
                    //    else if (file.ContentType == "image/jpeg") { fileExt = "jpg"; }
                    //    //contentTypes.Add(file.ContentType);
                    //    names.Add(fileName);
                    //    string ss = file.ContentDisposition.ToString();

                    //    Console.WriteLine(ss);
                    //    string webRootPath = _hostingEnvironment.WebRootPath;
                    //    var uploads = Path.Combine(webRootPath, "images\\uploads\\InstituteLogoImage\\");

                    //    // set variable as Parent directory I do this to make sure the path exists if not
                    //    // I will create the directory.
                    //    var directoryInfo = new FileInfo(uploads).Directory;

                    //    if (directoryInfo != null)
                    //        directoryInfo.Create();

                    //    var filePath = uploads + fileName+ "."+ fileExt;
                    //    file.CopyTo(new FileStream(filePath, FileMode.Create));
                    //    fp = filePath;
                    //    fp = fp.Substring(fp.IndexOf("images"));

                    //    stu.MI_Id = fileDescriptionShort.Id;
                    //    stu.MI_Logo = fp;
                    //  sad.saveInstitutionLogoImagedetails(stu);

                }
            }
            return newImageName;
        }
    }
}