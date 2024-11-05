using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage;
using PreadmissionDTOs;
using DataAccessMsSqlServerProvider;


// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers.com.vapstech.HRMS
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class EmployeeDocumentUploadController : Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IHttpContextAccessor _accessor;
        private readonly DomainModelMsSqlServerContext _context;


        //string accountname = "bdcampusstrg";
        //string accesskey = "2GbpRyxTMjVYZc0wnKLbpgCAPYRrdX3HPUE6kcYLmk19vkq8ErTC1eYIMl1oFMhzihqlq3j0eqWmiOGt1sfZ5w==";

        string accountname = "";
        string accesskey = "";
        //private static FacadeUrl _config;
        public EmployeeDocumentUploadController(IHostingEnvironment hostingEnvironment, IHttpContextAccessor httpContextAccessor, DomainModelMsSqlServerContext context)
        {
            _hostingEnvironment = hostingEnvironment;
            _accessor = httpContextAccessor;
            _context = context;
        }
        public async Task<string> uploadtoazure(IFormFile file, string containername, string folder, int mid)
        {
            string newImageNamePath = "";
            string newImageName = "";
            try
            {
                var data = _context.IVRM_Storage_path_Details.ToList();
                if (data.Count() > 0)
                {
                    accountname = data.FirstOrDefault().IVRM_SD_Access_Name;
                    accesskey = data.FirstOrDefault().IVRM_SD_Access_Key;
                

                StorageCredentials cre = new StorageCredentials(accountname, accesskey);
                CloudStorageAccount acc = new CloudStorageAccount(cre, useHttps: true);
                // Create a blob client.
                CloudBlobClient blobClient = acc.CreateCloudBlobClient();

                // Get a reference to a container 
                CloudBlobContainer container = blobClient.GetContainerReference(containername);
                string fileExt = "";
                string[] fi = file.FileName.Split('.');
                if (fi.Length > 0)
                {
                     fileExt = fi.Last();
                }
                
                var ext = fileExt.ToLower();
                var guid = Guid.NewGuid().ToString();
                newImageName = guid + "."+ ext;

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
                }else
                {
                    newImageNamePath = "PathNotFound";
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return newImageNamePath;
        }

        //Employee Profile
        [Route("UploadEmployeeprofilepic")]
        public async Task<string> UploadEmployeeprofilepic(FileDescriptionDTO fileDescriptionShort)
        {
            var names = new List<string>();
            var contentTypes = new List<string>();

            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            string newImageName = "";
            //string fp = null;
            if (ModelState.IsValid)
            {
                foreach (var file in fileDescriptionShort.File)
                {
                    if (file.Length > 0)
                    {
                        string fileExt = "";
                        string[] fi = file.FileName.Split('.');
                        if (fi.Length > 0)
                        {
                            fileExt = fi.Last();
                        }
                        if (fileExt == "docx" || fileExt == "jpg" || fileExt == "jpeg" || fileExt == "gif" || fileExt == "png" || fileExt == "jfif")
                        {
                            try
                            {
                                string containername = "files";
                                newImageName = await uploadtoazure(file, containername, "EmployeeProfilePics", mid);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                        }
                        else
                        {

                        }
                        
                    }
                }
            }

            return newImageName;
        }


        [Route("UploadEmployeeDocuments")]
        public async Task<string> UploadStudentDocuments(FileDescriptionDTO fileDescriptionShort)
        {
            var names = new List<string>();
            var contentTypes = new List<string>();
            string newImageName = "";
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            StudentApplicationDTO stu = new StudentApplicationDTO();
            if (ModelState.IsValid)
            {
                foreach (var file in fileDescriptionShort.File)
                {
                    if (file.Length > 0)
                    {

                        string fileExt = "";
                        string[] fi = file.FileName.Split('.');
                        if (fi.Length > 0)
                        {
                            fileExt = fi.Last();
                        }
                        if (fileExt == "docx" || fileExt == "doc" || fileExt == "pdf" || fileExt == "jpg" || fileExt == "JPEG")
                        {
                            try
                            {
                                string containername = "files";
                                newImageName = await uploadtoazure(file, containername, "EmployeeDocuments", mid);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                        }
                    }
                }
            }
            return newImageName;
        }
    }
}
