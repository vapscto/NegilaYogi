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
using CommonLibrary;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage;
using DataAccessMsSqlServerProvider;
using ImageMagick;
using System.Text.RegularExpressions;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.IO.Image;
using iText.Layout.Element;
using iText.Kernel.Utils;
using Microsoft.Office.Interop.Word;
using System.Text;
using Document = Microsoft.Office.Interop.Word.Document;
using Microsoft.WindowsAzure.Storage.Core.Util;

namespace corewebapi18072016.Controllers
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class ImageUploadController : Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IHttpContextAccessor _accessor;
        private readonly DomainModelMsSqlServerContext _context;
        string accountname = "";
        string accesskey = "";
        StudentApplicationDelegate sad = new StudentApplicationDelegate();
        public ImageUploadController(IHostingEnvironment hostingEnvironment, IHttpContextAccessor httpContextAccessor, DomainModelMsSqlServerContext context)
        {
            _hostingEnvironment = hostingEnvironment;
            _accessor = httpContextAccessor;
            _context = context;
        }
        [Route("Uploadprofilepicpreadmission")]
        public async Task<string> Uploadprofilepicpreadmin(FileDescriptionDTO fileDescriptionShort)
        {
            var names = new List<string>();
            var contentTypes = new List<string>();
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id_Preadmission"));
            string newImageName = "";
            StudentApplicationDTO stu = new StudentApplicationDTO();
            if (ModelState.IsValid)
            {
                foreach (var file in fileDescriptionShort.File)
                {
                    if (file.Length > 0)
                    {
                        try
                        {
                            string containername = "files";
                            newImageName = await uploadtoazure_Preadmission(file, containername, "OnlineUserProfiles");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
            }
            return newImageName;
        }
        public async Task<string> uploadtoazure_Preadmission(IFormFile file, string containername, string folder)
        {
            string newImageNamePath = "";
            string newImageName = "";
            var newFileName = string.Empty;
            var fileName = string.Empty;
            var nameofthefile = "";
            var files = HttpContext.Request.Form.Files;
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id_Preadmission"));
            try
            {
                if (HttpContext.Request.Form.Files != null)
                {
                    foreach (var file1 in files)
                    {
                        if (file.Length > 0)
                        {
                            string fileExt = "";
                            if (file.ContentType == "image/jpeg")
                            {
                                fileExt = ".jpg";
                            }
                            if (file.ContentType == "application/pdf")
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
                            if (fileExt == ".jpg" || fileExt == ".png")
                            {
                                var uploads = Path.Combine(_hostingEnvironment.WebRootPath, "UploadImages");
                                fileName = file.FileName;
                                using (var fileStream = new FileStream(Path.Combine(uploads, fileName), FileMode.Create))
                                {
                                    await file.CopyToAsync(fileStream);
                                    nameofthefile = fileName;
                                }
                                fileName = System.Net.Http.Headers.ContentDispositionHeaderValue.Parse(file1.ContentDisposition).FileName.Trim('"');
                                fileName = Path.Combine(_hostingEnvironment.WebRootPath, "UploadImages") + $@"\{fileName}";
                            }
                            var data = _context.IVRM_Storage_path_Details.ToList();
                            if (data.Count() > 0)
                            {
                                accountname = data.FirstOrDefault().IVRM_SD_Access_Name;
                                accesskey = data.FirstOrDefault().IVRM_SD_Access_Key;
                            }
                            StorageCredentials cre = new StorageCredentials(accountname, accesskey);
                            CloudStorageAccount acc = new CloudStorageAccount(cre, useHttps: true);
                            CloudBlobClient blobClient = acc.CreateCloudBlobClient();
                            CloudBlobContainer container = blobClient.GetContainerReference(containername);

                            var ext = fileExt.ToLower();
                            var guid = Guid.NewGuid().ToString();
                            newImageName = guid + ext;
                            string blobName = newImageName;
                            await container.CreateIfNotExistsAsync();
                            await container.SetPermissionsAsync(new BlobContainerPermissions
                            {
                                PublicAccess = BlobContainerPublicAccessType.Blob
                            });
                            var blockBlob = container.GetBlockBlobReference(mid + "/" + folder + "/" + blobName);
                            if (fileExt == ".jpg" || fileExt == ".png")
                            {
                                const int size = 900;
                                const int quality = 100;
                                using (var image = new MagickImage(fileName))
                                {
                                    image.Resize(size, size);
                                    image.AutoOrient();
                                    image.Strip();
                                    image.Quality = quality;
                                    image.Write(fileName);
                                }
                                await blockBlob.UploadFromFileAsync(fileName);
                                newImageNamePath = blockBlob.Uri.ToString();
                                //Delete File From Folder
                                if (System.IO.File.Exists(fileName))
                                {
                                    System.IO.File.Delete(fileName);
                                }
                            }
                            else
                            {
                                using (var fileStream = file.OpenReadStream())
                                {
                                    await blockBlob.UploadFromStreamAsync(fileStream);
                                }
                                newImageNamePath = blockBlob.Uri.ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return newImageNamePath;
        }
        public async Task<string> uploadtoazure_1(IFormFile file, string containername, string folder)
        {
            string newImageNamePath = "";
            string newImageName = "";
            var newFileName = string.Empty;
            var fileName = string.Empty;
            var nameofthefile = "";
            var files = HttpContext.Request.Form.Files;
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            try
            {
                if (HttpContext.Request.Form.Files != null)
                {
                    foreach (var file1 in files)
                    {
                        if (file.Length > 0)
                        {
                            string fileExt = "";
                            if (file.ContentType == "image/jpeg")
                            {
                                fileExt = ".jpg";
                            }
                            if (file.ContentType == "image/jpg")
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
                            else if (file.ContentType == "audio/mp3")
                            {
                                fileExt = ".mp3";
                            }
                            else if (file.ContentType == "audio/mpeg")
                            {
                                fileExt = ".mp3";
                            }
                            else if (file.ContentType == "video/x-ms-wmv")
                            {
                                fileExt = ".wmv";
                            }
                            else if (file.ContentType == "application/vnd.ms-excel")
                            {
                                fileExt = ".xls";
                            }
                            else if (file.ContentType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                            {
                                fileExt = ".xlsx";
                            }
                            else if (file.ContentType == "application/msword")
                            {
                                fileExt = ".doc";
                            }
                            else if (file.ContentType == "application/vnd.openxmlformats-officedocument.wordprocessingml.document")
                            {
                                fileExt = ".docx";
                            }
                            else if (file.ContentType == "application/vnd.ms-powerpoint")
                            {
                                fileExt = ".ppt";
                            }
                            else if (file.ContentType == "application/vnd.openxmlformats-officedocument.presentationml.presentation")
                            {
                                fileExt = ".pptx";
                            }
                            else if (file.ContentType == "application/vnd.openxmlformats-officedocument.presentationml.slideshow")
                            {
                                fileExt = ".ppsx";
                            }
                            if (fileExt == ".jpg" || fileExt == ".png")
                            {
                                var uploads = Path.Combine(_hostingEnvironment.WebRootPath, "UploadImages");
                                fileName = file.FileName;
                                using (var fileStream = new FileStream(Path.Combine(uploads, fileName), FileMode.Create))
                                {
                                    await file.CopyToAsync(fileStream);
                                    nameofthefile = fileName;
                                }
                                fileName = System.Net.Http.Headers.ContentDispositionHeaderValue.Parse(file1.ContentDisposition).FileName.Trim('"');
                                fileName = Path.Combine(_hostingEnvironment.WebRootPath, "UploadImages") + $@"\{fileName}";
                            }
                            var data = _context.IVRM_Storage_path_Details.ToList();
                            if (data.Count() > 0)
                            {
                                accountname = data.FirstOrDefault().IVRM_SD_Access_Name;
                                accesskey = data.FirstOrDefault().IVRM_SD_Access_Key;
                            }
                            StorageCredentials cre = new StorageCredentials(accountname, accesskey);
                            BlobRequestOptions requestoptions = new BlobRequestOptions()
                            {
                                SingleBlobUploadThresholdInBytes = 1024 * 1024 * 50, //50MB
                                ParallelOperationThreadCount = 12,
                            };

                            CloudStorageAccount acc = new CloudStorageAccount(cre, useHttps: true);
                            CloudBlobClient blobClient = acc.CreateCloudBlobClient();
                            CloudBlobContainer container = blobClient.GetContainerReference(containername);
                            var ext = fileExt.ToLower();
                            var guid = Guid.NewGuid().ToString();
                            newImageName = guid + ext;
                            string blobName = newImageName;
                            await container.CreateIfNotExistsAsync();
                            await container.SetPermissionsAsync(new BlobContainerPermissions
                            {
                                PublicAccess = BlobContainerPublicAccessType.Blob
                            });
                            var blockBlob = container.GetBlockBlobReference(mid + "/" + folder + "/" + blobName);
                            if (fileExt == ".jpg" || fileExt == ".png")
                            {
                                const int size = 900;
                                const int quality = 100;
                                using (var image = new MagickImage(fileName))
                                {
                                    image.Resize(size, size);
                                    image.AutoOrient();
                                    image.Strip();
                                    image.Quality = quality;
                                    image.Write(fileName);
                                }
                                await blockBlob.UploadFromFileAsync(fileName);
                                newImageNamePath = blockBlob.Uri.ToString();
                                if (System.IO.File.Exists(fileName))
                                {
                                    System.IO.File.Delete(fileName);
                                }
                            }
                            else
                            {
                                using (var fileStream = file.OpenReadStream())
                                {
                                    await blockBlob.UploadFromStreamAsync(fileStream);
                                }
                                newImageNamePath = blockBlob.Uri.ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return newImageNamePath;
        }
        public async Task<string> uploadtoazure(IFormFile file, string containername, string folder)
        {
            string newImageNamePath = "";
            string newImageName = "";
            var newFileName = string.Empty;
            var fileName = string.Empty;
            var nameofthefile = "";
            var files = HttpContext.Request.Form.Files;
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            try
            {
                if (HttpContext.Request.Form.Files != null)
                {
                    foreach (var file1 in files)
                    {
                        if (file.Length > 0)
                        {
                            string fileExt = "";
                            if (file.ContentType == "image/jpeg")
                            {
                                fileExt = ".jpg";
                            }
                            if (file.ContentType == "image/jpg")
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
                            else if (file.ContentType == "audio/mp3")
                            {
                                fileExt = ".mp3";
                            }
                            else if (file.ContentType == "audio/mpeg")
                            {
                                fileExt = ".mp3";
                            }
                            else if (file.ContentType == "video/x-ms-wmv")
                            {
                                fileExt = ".wmv";
                            }
                            else if (file.ContentType == "application/vnd.ms-excel")
                            {
                                fileExt = ".xls";
                            }
                            else if (file.ContentType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                            {
                                fileExt = ".xlsx";
                            }
                            else if (file.ContentType == "application/msword")
                            {
                                fileExt = ".doc";
                            }
                            else if (file.ContentType == "application/vnd.openxmlformats-officedocument.wordprocessingml.document")
                            {
                                fileExt = ".docx";
                            }
                            else if (file.ContentType == "application/vnd.ms-powerpoint")
                            {
                                fileExt = ".ppt";
                            }
                            else if (file.ContentType == "application/vnd.openxmlformats-officedocument.presentationml.presentation")
                            {
                                fileExt = ".pptx";
                            }
                            else if (file.ContentType == "application/vnd.openxmlformats-officedocument.presentationml.slideshow")
                            {
                                fileExt = ".ppsx";
                            }
                            if (fileExt == ".jpg" || fileExt == ".png")
                            {
                                var uploads = Path.Combine(_hostingEnvironment.WebRootPath, "UploadImages");
                                fileName = file.FileName;
                                using (var fileStream = new FileStream(Path.Combine(uploads, fileName), FileMode.Create))
                                {
                                    await file.CopyToAsync(fileStream);
                                    nameofthefile = fileName;
                                }
                                fileName = System.Net.Http.Headers.ContentDispositionHeaderValue.Parse(file1.ContentDisposition).FileName.Trim('"');
                                fileName = Path.Combine(_hostingEnvironment.WebRootPath, "UploadImages") + $@"\{fileName}";
                            }
                            var data = _context.IVRM_Storage_path_Details.ToList();
                            if (data.Count() > 0)
                            {
                                accountname = data.FirstOrDefault().IVRM_SD_Access_Name;
                                accesskey = data.FirstOrDefault().IVRM_SD_Access_Key;
                            }
                            StorageCredentials cre = new StorageCredentials(accountname, accesskey);
                            BlobRequestOptions requestoptions = new BlobRequestOptions()
                            {
                                SingleBlobUploadThresholdInBytes = 1024 * 1024 * 50, //50MB
                                ParallelOperationThreadCount = 12,
                            };

                            CloudStorageAccount acc = new CloudStorageAccount(cre, useHttps: true);
                            CloudBlobClient blobClient = acc.CreateCloudBlobClient();
                            CloudBlobContainer container = blobClient.GetContainerReference(containername);
                            var ext = fileExt.ToLower();
                            var guid = Guid.NewGuid().ToString();
                            newImageName = guid + ext;
                            string blobName = newImageName;
                            await container.CreateIfNotExistsAsync();
                            await container.SetPermissionsAsync(new BlobContainerPermissions
                            {
                                PublicAccess = BlobContainerPublicAccessType.Blob
                            });
                            var blockBlob = container.GetBlockBlobReference(mid + "/" + folder + "/" + blobName);
                            if (fileExt == ".jpg" || fileExt == ".png")
                            {
                                const int size = 900;
                                const int quality = 100;
                                using (var image = new MagickImage(fileName))
                                {
                                    image.Resize(size, size);
                                    image.AutoOrient();
                                    image.Strip();
                                    image.Quality = quality;
                                    image.Write(fileName);
                                }
                                await blockBlob.UploadFromFileAsync(fileName);
                                newImageNamePath = blockBlob.Uri.ToString();
                                if (System.IO.File.Exists(fileName))
                                {
                                    System.IO.File.Delete(fileName);
                                }
                            }
                            else
                            {
                                using (var fileStream = file.OpenReadStream())
                                {
                                    await blockBlob.UploadFromStreamAsync(fileStream);
                                }
                                newImageNamePath = blockBlob.Uri.ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return newImageNamePath;
        }
        [HttpPost]
        public async Task<bool> UploadFiles(FileDescriptionDTO fileDescriptionShort)
        {
            var names = new List<string>();
            var contentTypes = new List<string>();
            StudentApplicationDTO stu = new StudentApplicationDTO();
            if (ModelState.IsValid)
            {
                foreach (var file in fileDescriptionShort.File)
                {
                    if (file.Length > 0)
                    {
                        using (var reader = new StreamReader(file.OpenReadStream()))
                        {
                            var fileContent = reader.ReadToEnd();
                            var parsedContentDisposition = ContentDispositionHeaderValue.Parse(file.ContentDisposition);
                            Console.WriteLine(fileContent.ToString());
                        }

                        var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName;
                        //contentTypes.Add(file.ContentType);
                        names.Add(fileName.ToString().Trim());
                        string ss = file.ContentDisposition.ToString();

                        Console.WriteLine(ss);
                        string webRootPath = _hostingEnvironment.WebRootPath;
                        var uploads = Path.Combine(webRootPath, "images\\uploads\\");

                        var filePath = uploads + fileName;

                        file.CopyTo(new FileStream(filePath, FileMode.Create));
                        stu.PASMD_DocumentName = fileName.ToString().Trim();
                        stu.PASMD_Path = filePath;
                        stu.pasR_Id = fileDescriptionShort.Id;
                        sad.savestudentdetails(stu);
                        // Extension method update RC2 has removed this 
                        //await file.SaveAsAsync(Path.Combine(_optionsApplicationConfiguration.Value.ServerUploadFolder, fileName));
                    }
                }
            }

            //var files = new FileResult
            //{
            //    FileNames = names,
            //    ContentTypes = contentTypes,
            //    Description = fileDescriptionShort.Description,
            //    CreatedTimestamp = DateTime.UtcNow,
            //    UpdatedTimestamp = DateTime.UtcNow,
            //};

            // _fileRepository.AddFileDescriptions(files);

            return true;
        }

        //Student Profile
        [Route("Uploadprofilepic")]
        public async Task<string> Uploadprofilepic(FileDescriptionDTO fileDescriptionShort)
        {
            var names = new List<string>();
            var contentTypes = new List<string>();
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            string newImageName = "";
            StudentApplicationDTO stu = new StudentApplicationDTO();
            if (ModelState.IsValid)
            {
                foreach (var file in fileDescriptionShort.File)
                {
                    if (file.Length > 0)
                    {
                        try
                        {
                            string containername = "files";
                            newImageName = await uploadtoazure(file, containername, "StudentProfilePics");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
            }
            return newImageName;
        }

        [Route("Librarybooks")]
        public async Task<string> Librarybooks(FileDescriptionDTO fileDescriptionShort)
        {
            var names = new List<string>();
            var contentTypes = new List<string>();
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            string newImageName = "";
            StudentApplicationDTO stu = new StudentApplicationDTO();
            if (ModelState.IsValid)
            {
                foreach (var file in fileDescriptionShort.File)
                {
                    if (file.Length > 0)
                    {
                        try
                        {
                            string containername = "files";
                            newImageName = await uploadtoazure(file, containername, "Librarybooks");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
            }
            return newImageName;
        }

        [Route("ParentsProfilePics")]
        public async Task<string> ParentsProfilePics(FileDescriptionDTO fileDescriptionShort)
        {
            var names = new List<string>();
            var contentTypes = new List<string>();
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            string newImageName = "";
            StudentApplicationDTO stu = new StudentApplicationDTO();
            if (ModelState.IsValid)
            {
                foreach (var file in fileDescriptionShort.File)
                {
                    if (file.Length > 0)
                    {
                        try
                        {
                            string containername = "files";
                            newImageName = await uploadtoazure(file, containername, "ParentsProfilePics");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
            }
            return newImageName;
        }

        [Route("UploadStudentDocuments")]
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
                        string containername = "files";
                        newImageName = await uploadtoazure(file, containername, "StudentDocuments");
                    }
                }
            }
            return newImageName;
        }

        [Route("uploadGuardianDocs")]
        public async Task<string> UploadGuardianPhoto(FileDescriptionDTO fileDescriptionShort)
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
                        string containername = "files";
                        newImageName = await uploadtoazure(file, containername, "GuardianDocs");
                    }
                }
            }
            return newImageName;
        }

        [Route("UploadParentsDocs")]
        public async Task<string> UploadParentsDocs(FileDescriptionDTO fileDescriptionShort)
        {
            var names = new List<string>();
            var contentTypes = new List<string>();
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            string newImageName = "";
            StudentApplicationDTO stu = new StudentApplicationDTO();
            if (ModelState.IsValid)
            {
                foreach (var file in fileDescriptionShort.File)
                {
                    if (file.Length > 0)
                    {
                        string containername = "files";
                        newImageName = await uploadtoazure(file, containername, "StudentParentsDocs");
                    }
                }
            }
            return newImageName;
        }

        [Route("Upload_GalleryImgVideos")]
        public async Task<List<string>> Upload_ISMAttachment(FileDescriptionDTO fileDescriptionShort)
        {
            var names = new List<string>();
            var contentTypes = new List<string>();
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            string newImageName = "";
            List<string> FilesPaths = new List<string>();
            StudentApplicationDTO stu = new StudentApplicationDTO();
            if (ModelState.IsValid)
            {
                foreach (var file in fileDescriptionShort.File)
                {
                    if (file.Length > 0)
                    {
                        try
                        {
                            string containername = "files";
                            newImageName = await uploadtoazure_1(file, containername, "ISM_Attachments");
                            FilesPaths.Add(newImageName);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
            }
            return FilesPaths;
        }

        [Route("Alumni_Gallery")]
        public async Task<List<filedetails>> Alumni_Gallery(FileDescriptionDTO fileDescriptionShort)
        {
            var names = new List<string>();
            var contentTypes = new List<string>();
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            string newImageName = "";
            List<string> Filesname = new List<string>();
            List<filedetails> FilesPaths = new List<filedetails>();
            StudentApplicationDTO stu = new StudentApplicationDTO();
            if (ModelState.IsValid)
            {
                foreach (var file in fileDescriptionShort.File)
                {
                    if (file.Length > 0)
                    {
                        try
                        {
                            string containername = "files";
                            var folder = "";
                            if (fileDescriptionShort.folder == null)
                            {
                                folder = "AlumniGallery";
                            }
                            else
                            {
                                folder = fileDescriptionShort.folder;
                            }

                            newImageName = await uploadtoazure_1(file, containername, folder);
                            filedetails emp = new filedetails();
                            emp.name = file.FileName;
                            emp.path = newImageName;
                            FilesPaths.Add(emp);

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }

            }
            return FilesPaths;
        }

        [Route("InteractionUpload")]
        public async Task<List<string>> InteractionUpload(FileDescriptionDTO fileDescriptionShort)
        {
            var names = new List<string>();
            var contentTypes = new List<string>();
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            string newImageName = "";
            List<string> FilesPaths = new List<string>();
            StudentApplicationDTO stu = new StudentApplicationDTO();
            if (ModelState.IsValid)
            {
                foreach (var file in fileDescriptionShort.File)
                {
                    if (file.Length > 0)
                    {
                        try
                        {
                            string containername = "files";
                            newImageName = await uploadtoazure_1(file, containername, "InteractionUpload");
                            FilesPaths.Add(newImageName);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
            }
            return FilesPaths;
        }

        [Route("NoticeUpload")]
        public async Task<List<filedetails>> NoticeUpload(FileDescriptionDTO fileDescriptionShort)
        {
            var names = new List<string>();
            var contentTypes = new List<string>();
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            string newImageName = "";
            List<string> Filesname = new List<string>();
            List<filedetails> FilesPaths = new List<filedetails>();
            StudentApplicationDTO stu = new StudentApplicationDTO();
            if (ModelState.IsValid)
            {
                foreach (var file in fileDescriptionShort.File)
                {
                    if (file.Length > 0)
                    {
                        try
                        {
                            string containername = "files";
                            var folder = "";
                            if (fileDescriptionShort.folder == null)
                            {
                                folder = "NoticeUpload";
                            }
                            else
                            {
                                folder = fileDescriptionShort.folder;
                            }

                            newImageName = await uploadtoazure_1(file, containername, folder);
                            filedetails emp = new filedetails();
                            emp.name = file.FileName;
                            emp.path = newImageName;
                            FilesPaths.Add(emp);

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }

            }
            return FilesPaths;
        }

        [Route("HomeworkUpload")]
        public async Task<List<filedetails>> HomeworkkUpload(FileDescriptionDTO fileDescriptionShort)
        {
            var names = new List<string>();
            var contentTypes = new List<string>();
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            string newImageName = "";
            List<string> Filesname = new List<string>();
            List<filedetails> FilesPaths = new List<filedetails>();
            StudentApplicationDTO stu = new StudentApplicationDTO();
            if (ModelState.IsValid)
            {
                foreach (var file in fileDescriptionShort.File)
                {
                    if (file.Length > 0)
                    {
                        try
                        {
                            string containername = "files";
                            newImageName = await uploadtoazure_1(file, containername, "HomeworkUpload");

                            filedetails emp = new filedetails();
                            emp.name = file.FileName;
                            emp.path = newImageName;
                            FilesPaths.Add(emp);

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }

            }
            return FilesPaths;
        }

        [Route("ClassworkUpload")]
        public async Task<List<filedetails>> ClassworkUpload(FileDescriptionDTO fileDescriptionShort)
        {
            var names = new List<string>();
            var contentTypes = new List<string>();
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            string newImageName = "";
            List<string> Filesname = new List<string>();
            List<filedetails> FilesPaths = new List<filedetails>();
            StudentApplicationDTO stu = new StudentApplicationDTO();
            if (ModelState.IsValid)
            {
                foreach (var file in fileDescriptionShort.File)
                {
                    if (file.Length > 0)
                    {
                        try
                        {
                            string containername = "files";
                            newImageName = await uploadtoazure_1(file, containername, "ClassworkUpload");

                            filedetails emp = new filedetails();
                            emp.name = file.FileName;
                            emp.path = newImageName;
                            FilesPaths.Add(emp);

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }

            }
            return FilesPaths;
        }


        // [Route("Upload_GalleryImgVideos")]
        public async Task<List<string>> Upload_GalleryImgVideos(FileDescriptionDTO fileDescriptionShort)
        {
            var names = new List<string>();
            var contentTypes = new List<string>();
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            string newImageName = "";
            List<string> FilesPaths = new List<string>();
            StudentApplicationDTO stu = new StudentApplicationDTO();
            if (ModelState.IsValid)
            {
                foreach (var file in fileDescriptionShort.File)
                {
                    if (file.Length > 0)
                    {
                        try
                        {
                            string containername = "files";
                            newImageName = await uploadtoazure_1(file, containername, "GalleryImgVideos");
                            FilesPaths.Add(newImageName);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
            }
            return FilesPaths;
        }

        //Mahaboob for COE Module
        [Route("Upload_imgs_vids")]
        public async Task<List<string>> Upload_imgs_vids(FileDescriptionDTO fileDescriptionShort)
        {
            var names = new List<string>();
            var contentTypes = new List<string>();
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            string newImageName = "";
            List<string> FilesPaths = new List<string>();
            StudentApplicationDTO stu = new StudentApplicationDTO();
            if (ModelState.IsValid)
            {
                foreach (var file in fileDescriptionShort.File)
                {
                    if (file.Length > 0)
                    {
                        try
                        {
                            string containername = "files";
                            newImageName = await uploadtoazure(file, containername, "COE_Images_Videos");
                            FilesPaths.Add(newImageName);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
            }
            return FilesPaths;
        }

        [Route("ClgUpload_Img_Videos")]
        public async Task<List<string>> ClgUpload_Img_Videos(FileDescriptionDTO fileDescriptionShort)
        {
            var names = new List<string>();
            var contentTypes = new List<string>();
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            string newImageName = "";
            List<string> FilesPaths = new List<string>();
            StudentApplicationDTO stu = new StudentApplicationDTO();
            if (ModelState.IsValid)
            {
                foreach (var file in fileDescriptionShort.File)
                {
                    if (file.Length > 0)
                    {
                        try
                        {
                            string containername = "files";
                            newImageName = await uploadtoazure(file, containername, "clg_portal_images_videos");
                            FilesPaths.Add(newImageName);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
            }
            return FilesPaths;
        }

        //Documents Upload
        [Route("Upload_Docs")]
        public async Task<List<string>> Upload_Docs(FileDescriptionDTO fileDescriptionShort)
        {
            var names = new List<string>();
            var contentTypes = new List<string>();
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            string newImageName = "";
            List<string> FilesPaths = new List<string>();
            StudentApplicationDTO stu = new StudentApplicationDTO();
            if (ModelState.IsValid)
            {
                foreach (var file in fileDescriptionShort.File)
                {
                    if (file.Length > 0)
                    {
                        try
                        {
                            string containername = "files";
                            newImageName = await uploadtoazure(file, containername, "Portal_Document");
                            FilesPaths.Add(newImageName);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
            }
            return FilesPaths;
        }

        //Documents Upload
        [Route("Upload_Noticefiles")]
        public async Task<List<string>> Upload_Noticefiles(FileDescriptionDTO fileDescriptionShort)
        {
            var names = new List<string>();
            var contentTypes = new List<string>();
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            string newImageName = "";
            List<string> FilesPaths = new List<string>();
            StudentApplicationDTO stu = new StudentApplicationDTO();
            if (ModelState.IsValid)
            {
                foreach (var file in fileDescriptionShort.File)
                {
                    if (file.Length > 0)
                    {
                        try
                        {
                            string containername = "files";
                            newImageName = await uploadtoazure(file, containername, "Notice_Files");
                            FilesPaths.Add(newImageName);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
            }
            return FilesPaths;
        }

        [Route("Waviedofffiles")]
        public async Task<List<string>> Waviedofffiles(FileDescriptionDTO fileDescriptionShort)
        {
            var names = new List<string>();
            var contentTypes = new List<string>();
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            string newImageName = "";
            List<string> FilesPaths = new List<string>();
            StudentApplicationDTO stu = new StudentApplicationDTO();
            if (ModelState.IsValid)
            {
                foreach (var file in fileDescriptionShort.File)
                {
                    if (file.Length > 0)
                    {
                        try
                        {
                            string containername = "files";
                            newImageName = await uploadtoazure(file, containername, "Waviedofffiles");
                            FilesPaths.Add(newImageName);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
            }
            return FilesPaths;
        }

        [Route("Upload_Principal_sign")]
        public async Task<string> Upload_Principal_sign(FileDescriptionDTO fileprincipalsign)
        {
            var names = new List<string>();
            var contentTypes = new List<string>();
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            string newImageName = "";
            StudentApplicationDTO stu = new StudentApplicationDTO();
            if (ModelState.IsValid)
            {
                foreach (var file in fileprincipalsign.File)
                {
                    if (file.Length > 0)
                    {
                        try
                        {
                            string containername = "files";
                            newImageName = await uploadtoazure(file, containername, "principalsign");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
            }
            return newImageName;
        }

        [Route("Upload_Manager_sign")]
        public async Task<string> Upload_Manager_sign(FileDescriptionDTO filemanagersign)
        {
            var names = new List<string>();
            var contentTypes = new List<string>();
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            string newImageName = "";
            StudentApplicationDTO stu = new StudentApplicationDTO();
            if (ModelState.IsValid)
            {
                foreach (var file in filemanagersign.File)
                {
                    if (file.Length > 0)
                    {
                        try
                        {
                            string containername = "files";
                            newImageName = await uploadtoazure(file, containername, "managersign");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
            }
            return newImageName;
        }

        [Route("uploadvehicleimg")]
        public async Task<string> uploadvehicleimg(FileDescriptionDTO filevehicleimg)
        {
            var names = new List<string>();
            var contentTypes = new List<string>();
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            string newImageName = "";
            StudentApplicationDTO stu = new StudentApplicationDTO();
            if (ModelState.IsValid)
            {
                foreach (var file in filevehicleimg.File)
                {
                    if (file.Length > 0)
                    {
                        try
                        {
                            string containername = "files";
                            newImageName = await uploadtoazure(file, containername, "Transport_VehicleImg");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
            }
            return newImageName;
        }
        //Uploadsoundtrack IVRS
        [Route("Uploadsoundtrack")]
        public async Task<string> Uploadsoundtrack(FileDescriptionDTO fileDescriptionShort)
        {
            var names = new List<string>();
            var contentTypes = new List<string>();
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            string newImageName = "";
            if (ModelState.IsValid)
            {
                foreach (var file in fileDescriptionShort.File)
                {
                    if (file.Length > 0)
                    {
                        try
                        {
                            string containername = "files";
                            newImageName = await uploadtoazure(file, containername, "IVRSSOUNDTRACKS");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
            }
            return newImageName;
        }

        //Upload Lessonplanner Documents
        [Route("lessonplannerdoc")]
        public async Task<string> lessonplannerdoc(FileDescriptionDTO fileDescriptionShort)
        {
            var names = new List<string>();
            var contentTypes = new List<string>();
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            string newImageName = "";
            if (ModelState.IsValid)
            {
                foreach (var file in fileDescriptionShort.File)
                {
                    if (file.Length > 0)
                    {
                        try
                        {
                            string containername = "files";
                            newImageName = await uploadtoazure(file, containername, "LessonPlanner");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
            }
            return newImageName;
        }

        [Route("OnlineProgramdoc")]
        public async Task<string> OnlineProgramdoc(FileDescriptionDTO fileDescriptionShort)
        {
            var names = new List<string>();
            var contentTypes = new List<string>();
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            string newImageName = "";
            if (ModelState.IsValid)
            {
                foreach (var file in fileDescriptionShort.File)
                {
                    if (file.Length > 0)
                    {
                        try
                        {
                            string containername = "files";
                            newImageName = await uploadtoazure(file, containername, "OnlineProgram");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
            }
            return newImageName;
        }

        //NAAC DOCUMENTS UPLOAD 
        [Route("emailfileattachupload")]
        public async Task<string> emailfileattachupload(FileDescriptionDTO fileDescriptionShort)
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
                        string containername = "files";
                        newImageName = await uploadtoazure(file, containername, "emailfileattachupload");
                    }
                }
            }
            return newImageName;
        }

        //NAAC DOCUMENTS UPLOAD 
        [Route("Uploadnaacdocuments")]
        public async Task<string> Uploadnaacdocuments(FileDescriptionDTO fileDescriptionShort)
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
                        string containername = "files";
                        newImageName = await uploadtoazure(file, containername, "NAACDocumentsUpload");
                    }
                }
            }
            return newImageName;
        }

        [Route("Addcandidate")]
        public async Task<string> Addcandidate(FileDescriptionDTO fileDescriptionShort)
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
                        string containername = "files";
                        newImageName = await uploadtoazure(file, containername, "NCandidateDocuments");
                    }
                }
            }
            return newImageName;
        }

        [Route("Uploadtrnsportdocuments")]
        public async Task<string> Uploadtrnsportdocuments(FileDescriptionDTO fileDescriptionShort)
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
                        string containername = "files";
                        newImageName = await uploadtoazure(file, containername, "Trnsportdocuments");
                    }
                }
            }
            return newImageName;
        }

        [Route("paonlinexam")]
        public async Task<string> paonlinexam(FileDescriptionDTO fileDescriptionShort)
        {
            var names = new List<string>();
            var contentTypes = new List<string>();
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            string newImageName = "";
            if (ModelState.IsValid)
            {
                foreach (var file in fileDescriptionShort.File)
                {
                    if (file.Length > 0)
                    {
                        try
                        {
                            string containername = "files";
                            newImageName = await uploadtoazure(file, containername, "PAOnlineExam");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
            }
            return newImageName;
        }

        [Route("UploadEmployeeDocuments")]
        public async Task<string> UploadEmployeeDocuments(FileDescriptionDTO fileDescriptionShort)
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
                        string containername = "files";
                        newImageName = await uploadtoazure(file, containername, "EmployeeDocuments");
                    }
                }
            }
            return newImageName;
        }

        [Route("UploadinhouseDTo")]
        public async Task<string> uploadinhouseDTo(FileDescriptionDTO fileDescriptionShort)
        {
            var names = new List<string>();
            var contentTypes = new List<string>();
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            string newImageName = "";
            StudentApplicationDTO stu = new StudentApplicationDTO();
            if (ModelState.IsValid)
            {
                foreach (var file in fileDescriptionShort.File)
                {
                    if (file.Length > 0)
                    {
                        try
                        {
                            string containername = "E:\\Upload\\";
                            newImageName = await inhouser(file, containername, "StudentProfilePics");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
            }
            return newImageName;
        }

        public async Task<string> inhouser(IFormFile file, string containername, string folder)
        {
            string newImageNamePath = "";
            string newImageName = "";
            var newFileName = string.Empty;
            var fileName = string.Empty;
            var nameofthefile = "";
            var files = HttpContext.Request.Form.Files;
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            string mi_id = HttpContext.Session.GetInt32("Session_MI_Id").ToString();
            try
            {
                if (HttpContext.Request.Form.Files != null)
                {
                    foreach (var file1 in files)
                    {
                        if (file.Length > 0)
                        {
                            string fileExt = "";
                            if (file.ContentType == "image/jpeg")
                            {
                                fileExt = ".jpg";
                            }
                            if (file.ContentType == "image/jpg")
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
                            else if (file.ContentType == "audio/mp3")
                            {
                                fileExt = ".mp3";
                            }
                            else if (file.ContentType == "video/x-ms-wmv")
                            {
                                fileExt = ".wmv";
                            }
                            else if (file.ContentType == "application/vnd.ms-excel")
                            {
                                fileExt = ".xls";
                            }
                            else if (file.ContentType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                            {
                                fileExt = ".xlsx";
                            }
                            else if (file.ContentType == "application/msword")
                            {
                                fileExt = ".doc";
                            }
                            else if (file.ContentType == "application/vnd.openxmlformats-officedocument.wordprocessingml.document")
                            {
                                fileExt = ".docx";
                            }
                            else if (file.ContentType == "application/vnd.ms-powerpoint")
                            {
                                fileExt = ".ppt";
                            }
                            else if (file.ContentType == "application/vnd.openxmlformats-officedocument.presentationml.presentation")
                            {
                                fileExt = ".pptx";
                            }

                            var uploads = Path.Combine(containername, mi_id, folder);

                            if (Directory.Exists(uploads))
                            {
                                Console.WriteLine("That path exists already.");
                            }
                            else
                            {
                                DirectoryInfo di = Directory.CreateDirectory(uploads);
                            }
                            fileName = file.FileName;
                            using (var fileStream = new FileStream(Path.Combine(uploads, fileName), FileMode.Create))
                            {
                                await file.CopyToAsync(fileStream);
                                nameofthefile = fileName;
                            }

                            //using (var stream = new FileStream(uploads, FileMode.Create))
                            //{
                            //    await file.CopyToAsync(stream);
                            //}

                            string newpath = Path.Combine(uploads) + $@"\{fileName}";

                            newImageNamePath = newpath;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return newImageNamePath;
        }

        //Upload Lessonplanner Documents 
        [Route("visitordocuments")]
        public async Task<string> visitordocuments(FileDescriptionDTO fileDescriptionShort)
        {
            var names = new List<string>();
            var contentTypes = new List<string>();
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            string newImageName = "";
            if (ModelState.IsValid)
            {
                foreach (var file in fileDescriptionShort.File)
                {
                    if (file.Length > 0)
                    {
                        try
                        {
                            string containername = "files";
                            newImageName = await uploadtoazure(file, containername, "VisitorManagement");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
            }
            return newImageName;
        }

        [Route("UploadMasterNaacDocuments")]
        public async Task<string> UploadMasterNaacDocuments(FileDescriptionDTO fileDescriptionShort)
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
                        string containername = "files";
                        newImageName = await uploadtoazure(file, containername, "NaacMasterDocuments");
                    }
                }
            }
            return newImageName;
        }

        [Route("OnlineExamMergeFiles")]
        public async Task<FileDescriptionDTO> OnlineExamMergeFiles([FromBody] FileDescriptionDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

                data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
                data.Foldername = "LessonPlanner";
                string merged_filepath = "";
                var check_merged_filepath = _context.LP_Students_ExamDMO.Where(a => a.MI_Id == data.MI_Id && a.AMST_Id == data.AMST_Id 
                && a.LPMOEEX_Id == data.LPMOEEX_Id && a.LPSTUEX_Id == data.LPSTUEX_Id).ToList();

                if (check_merged_filepath.Count > 0)
                {
                    merged_filepath = check_merged_filepath.FirstOrDefault().LPSTUEX_MergedFile;
                }

                if(merged_filepath ==null || merged_filepath == "")
                {
                    data = await MergeFileDataAsync(data);

                    if (data.filepath != null && data.filepath != "")
                    {
                        var examresult = _context.LP_Students_ExamDMO.Where(a => a.MI_Id == data.MI_Id && a.AMST_Id == data.AMST_Id
                        && a.LPMOEEX_Id == data.LPMOEEX_Id && a.LPSTUEX_Id == data.LPSTUEX_Id).ToList();

                        if (examresult.Count > 0)
                        {
                            var result = _context.LP_Students_ExamDMO.Single(a => a.MI_Id == data.MI_Id && a.AMST_Id == data.AMST_Id && a.LPMOEEX_Id == data.LPMOEEX_Id
                             && a.LPSTUEX_Id == data.LPSTUEX_Id);
                            result.LPSTUEX_UpdatedBy = data.User_Id;
                            result.UpdatedDate = indiantime0;
                            result.LPSTUEX_MergedFile = data.filepath;
                            _context.Update(result);
                            var i = _context.SaveChanges();
                        }
                    }
                }
                else
                {
                    data.filepath = merged_filepath;
                }                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return data;
        }

        [Route("DeleteMergedFolder")]
        public FileDescriptionDTO DeleteMergedFolder([FromBody] FileDescriptionDTO data)
        {
            string wwwPath = _hostingEnvironment.WebRootPath;
            string contentPath = _hostingEnvironment.ContentRootPath;

            string pathname = "UploadImages/" + data.AMST_Id;
            string path = Path.Combine(_hostingEnvironment.WebRootPath, pathname);

            if (Directory.Exists(path))
            {
                System.GC.Collect();
                System.GC.WaitForPendingFinalizers();
                Directory.Delete(path, true);
            }

            return data;
        }

        public async Task<FileDescriptionDTO> MergeFileDataAsync(FileDescriptionDTO data)
        {
            try
            {
                if (data.MergeFilesDTO != null && data.MergeFilesDTO.Length > 0)
                {
                    string wwwPath = _hostingEnvironment.WebRootPath;
                    string contentPath = _hostingEnvironment.ContentRootPath;

                    string pathname = "UploadImages/" + data.AMST_Id;
                    string MergePdfOutput = "UploadImages/" + data.AMST_Id + "/FinalPdf";

                    string path = Path.Combine(_hostingEnvironment.WebRootPath, pathname);
                    if (Directory.Exists(path))
                    {
                        System.GC.Collect();
                        System.GC.WaitForPendingFinalizers();
                        Directory.Delete(path, true);
                    }

                    Directory.CreateDirectory(path);

                    string path_outputpdf = Path.Combine(_hostingEnvironment.WebRootPath, MergePdfOutput);
                    if (Directory.Exists(path_outputpdf))
                    {
                        System.GC.Collect();
                        System.GC.WaitForPendingFinalizers();
                        Directory.Delete(path_outputpdf, true);
                    }

                    Directory.CreateDirectory(path_outputpdf);
                    string accountname = "";
                    string accesskey = "";

                    var dataq = _context.IVRM_Storage_path_Details.ToList();
                    if (dataq.Count() > 0)
                    {
                        accountname = dataq.FirstOrDefault().IVRM_SD_Access_Name;
                        accesskey = dataq.FirstOrDefault().IVRM_SD_Access_Key;
                    }
                    string target = path;

                    foreach (var d in data.MergeFilesDTO)
                    {
                        MemoryStream ms = new MemoryStream();

                        string[] filepatharray = d.FilePath.Split("/");
                        //string rest = string.Join("/", filepatharray.Skip(4));
                        //string filepath = rest;
                        // string containername = filepatharray[3];

                        string filepath = data.MI_Id + "/" + data.Foldername + "/" + filepatharray[filepatharray.Length - 1];
                        string filedetails_name = filepatharray[filepatharray.Length - 1];

                        string[] filepatharray_extensions = filedetails_name.Split(".");

                        StorageCredentials cre = new StorageCredentials(accountname, accesskey);
                        CloudStorageAccount acc = new CloudStorageAccount(cre, useHttps: true);
                        CloudBlobClient blobClient = acc.CreateCloudBlobClient();
                        CloudBlobContainer container = blobClient.GetContainerReference("files");
                        if (await container.ExistsAsync())
                        {
                            CloudBlob file = container.GetBlobReference(filepath);
                            if (await file.ExistsAsync())
                            {
                                var uploads = Path.Combine(_hostingEnvironment.WebRootPath, target);
                                //string fileName = d.FileName;
                                string fileName = data.StudentName + "-" + d.FileOrder.ToString();
                                fileName = fileName + "." + filepatharray_extensions[1];
                                fileName = Path.Combine(_hostingEnvironment.WebRootPath, target) + $@"\{fileName}";

                                //await file.DownloadToFileAsync(fileName, FileMode.Create);
                                System.Threading.Tasks.Task.WaitAll(file.DownloadToFileAsync(fileName, FileMode.Create));

                                if (filepatharray_extensions[1].Equals("png", StringComparison.OrdinalIgnoreCase)
                                    || filepatharray_extensions[1].Equals("jpg", StringComparison.OrdinalIgnoreCase)
                                    || filepatharray_extensions[1].Equals("jpeg", StringComparison.OrdinalIgnoreCase))
                                {
                                    Imagetopdf(pathname, filepatharray[filepatharray.Length - 1], d.FileOrder.ToString(), data.StudentName);
                                    string path_delete = fileName;
                                    FileInfo file_delete = new FileInfo(path_delete);
                                    if (file_delete.Exists)
                                    {
                                        System.GC.Collect();
                                        System.GC.WaitForPendingFinalizers();
                                        file_delete.Delete();
                                    }
                                }

                                if (filepatharray_extensions[1].Equals("doc", StringComparison.OrdinalIgnoreCase)
                                    || filepatharray_extensions[1].Equals("docx", StringComparison.OrdinalIgnoreCase))
                                {
                                    DocToPdf(pathname, filepatharray[filepatharray.Length - 1], d.FileOrder.ToString(), data.StudentName);

                                    string path_delete = fileName;
                                    FileInfo file_delete = new FileInfo(path_delete);
                                    if (file_delete.Exists)
                                    {
                                        System.GC.Collect();
                                        System.GC.WaitForPendingFinalizers();
                                        file_delete.Delete();
                                    }
                                }
                            }
                        }
                    }

                    //Merge All Pdf
                    //System.Threading.Tasks.Task.WaitAll(Mergepdf(MergePdfOutput, pathname, data.StudentName));
                    string returnfilename = await Mergepdf(MergePdfOutput, pathname, data.StudentName);

                    //data.returnpath = MergePdfOutput + "/" + returnfilename;

                    System.Threading.Thread.Sleep(10000);

                    string[] filePaths = Directory.GetFiles(path_outputpdf, "*.pdf");
                    List<string> files = new List<string>();
                    foreach (var c in filePaths)
                    {
                        files.Add(c);
                    }

                    if (files.Count > 0)
                    {
                        string final_pdf_file = files[0];
                        var c = await AzureFileUploadAsync(final_pdf_file, "files", data.MI_Id.ToString(), data.Foldername);
                        data.filepath = c;
                    }

                    ////Deleting folder after merging the files
                    if (Directory.Exists(path))
                    {
                        Directory.Delete(path, true);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public void Imagetopdf(string outputfilepath, string imagefilename, string fileorder, string studentname)
        {
            var exePath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
            outputfilepath = outputfilepath.Replace("/", @"\");
            string wwwPath = _hostingEnvironment.WebRootPath;
            string contentPath = _hostingEnvironment.ContentRootPath;
            Regex appPathMatcher = new Regex(@"(?<!fil)[A-Za-z]:\\+[\S\s]*?(?=\\+bin)");
            string[] filenamelist = imagefilename.Split(".");
            var appRoot = appPathMatcher.Match(exePath).Value;
            var uploads = Path.Combine(wwwPath, outputfilepath);
            string ORIG = imagefilename;
            string OUTPUT_FOLDER = uploads;

            string filename = studentname + "-" + fileorder;
            fileorder = studentname + "-" + fileorder + "." + filenamelist[filenamelist.Length - 1];
            OUTPUT_FOLDER = Path.Combine(appRoot, OUTPUT_FOLDER);
            ORIG = Path.Combine(uploads) + $@"\{fileorder}";
            //PdfDocument pdfDocument = new PdfDocument(new PdfWriter(OUTPUT_FOLDER + "\\" + filenamelist[0] + ".pdf"));
            PdfDocument pdfDocument = new PdfDocument(new PdfWriter(OUTPUT_FOLDER + "\\" + filename + ".pdf"));
            try
            {
                iText.Layout.Document document = new iText.Layout.Document(pdfDocument);
                ImageData imageData = ImageDataFactory.Create(ORIG);
                Image image = new Image(imageData);
                image.SetWidth(pdfDocument.GetDefaultPageSize().GetWidth() - 50);
                image.SetAutoScaleHeight(true);
                document.Add(image);
                document.Close();
                pdfDocument.Close();
            }
            catch (Exception ex)
            {
                pdfDocument.Close();
                Console.WriteLine(ex.Message);
            }
        }
        public void DocToPdf(string outputfilepath, string imagefilename, string fileorder, string studentname)
        {
            // Create a new Microsoft Word application object
            Microsoft.Office.Interop.Word.Application word = new Microsoft.Office.Interop.Word.Application();
            try
            {
                var exePath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
                outputfilepath = outputfilepath.Replace("/", @"\");

                string wwwPath = _hostingEnvironment.WebRootPath;
                string contentPath = _hostingEnvironment.ContentRootPath;

                Regex appPathMatcher = new Regex(@"(?<!fil)[A-Za-z]:\\+[\S\s]*?(?=\\+bin)");
                string[] filenamelist = imagefilename.Split(".");
                var appRoot = appPathMatcher.Match(exePath).Value;
                var uploads = Path.Combine(wwwPath, outputfilepath);
                string ORIG = imagefilename;
                string OUTPUT_FOLDER = uploads;

                // C# doesn't have optional arguments so we'll need a dummy value
                object oMissing = System.Reflection.Missing.Value;

                // Get list of Word files in specified directory
                DirectoryInfo dirInfo = new DirectoryInfo(uploads);

                FileInfo[] wordFiles = dirInfo.GetFiles("*.doc");
                FileInfo[] docxwordFiles = dirInfo.GetFiles("*.docx");

                word.Visible = false;
                word.ScreenUpdating = false;

                //Doc
                foreach (FileInfo wordFile in wordFiles)
                {
                    // Cast as Object for word Open method
                    Object filename = (Object)wordFile.FullName;

                    // Use the dummy value as a placeholder for optional arguments
                    Document doc = word.Documents.Open(ref filename, ref oMissing,
                        ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing,
                        ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing,
                        ref oMissing, ref oMissing, ref oMissing, ref oMissing);
                    doc.Activate();

                    object outputFileName = wordFile.FullName.Replace(".doc", ".pdf");
                    object fileFormat = WdSaveFormat.wdFormatPDF;

                    // Save document into PDF Format
                    doc.SaveAs(ref outputFileName,
                        ref fileFormat, ref oMissing, ref oMissing,
                        ref oMissing, ref oMissing, ref oMissing, ref oMissing,
                        ref oMissing, ref oMissing, ref oMissing, ref oMissing,
                        ref oMissing, ref oMissing, ref oMissing, ref oMissing);

                    // Close the Word document, but leave the Word application open.
                    // doc has to be cast to type _Document so that it will find the
                    // correct Close method.                
                    object saveChanges = WdSaveOptions.wdDoNotSaveChanges;
                    ((_Document)doc).Close(ref saveChanges, ref oMissing, ref oMissing);
                    doc = null;
                }

                //Docx
                foreach (FileInfo wordFile in docxwordFiles)
                {
                    // Cast as Object for word Open method
                    Object filename = (Object)wordFile.FullName;

                    // Use the dummy value as a placeholder for optional arguments
                    Document doc = word.Documents.Open(ref filename, ref oMissing,
                        ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing,
                        ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing,
                        ref oMissing, ref oMissing, ref oMissing, ref oMissing);
                    doc.Activate();

                    object outputFileName = wordFile.FullName.Replace(".docx", ".pdf");
                    object fileFormat = WdSaveFormat.wdFormatPDF;

                    // Save document into PDF Format
                    doc.SaveAs(ref outputFileName,
                        ref fileFormat, ref oMissing, ref oMissing,
                        ref oMissing, ref oMissing, ref oMissing, ref oMissing,
                        ref oMissing, ref oMissing, ref oMissing, ref oMissing,
                        ref oMissing, ref oMissing, ref oMissing, ref oMissing);

                    // Close the Word document, but leave the Word application open.
                    // doc has to be cast to type _Document so that it will find the
                    // correct Close method.                
                    object saveChanges = WdSaveOptions.wdDoNotSaveChanges;
                    ((_Document)doc).Close(ref saveChanges, ref oMissing, ref oMissing);
                    doc = null;
                }

// word has to be cast to type _Application so that it will find
// the correct Quit method.
((_Application)word).Quit(ref oMissing, ref oMissing, ref oMissing);
                word = null;
            }
            catch (Exception ex)
            {
                word = null;
                Console.WriteLine(ex.Message);
            }
        }
        public async Task<string> Mergepdf(string outputfilepath, string pdffolder, string outputfilename)
        {
            string wwwPath = _hostingEnvironment.WebRootPath;
            string filename = outputfilename + "-" + Guid.NewGuid().ToString() + ".pdf";
            DocumentViewDTO data = new DocumentViewDTO();
            var exePath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
            Regex appPathMatcher = new Regex(@"(?<!fil)[A-Za-z]:\\+[\S\s]*?(?=\\+bin)");
            var appRoot = appPathMatcher.Match(exePath).Value;
            var uploads = Path.Combine(wwwPath, outputfilepath);
            var pdf_uploads = Path.Combine(wwwPath, pdffolder);
            string OUTPUT_FOLDER = uploads;
            string FILE1 = "Merged.pdf";
            FILE1 = Path.Combine(uploads) + $@"\{FILE1}";
            PdfDocument pdfDocument = new PdfDocument(new PdfWriter(OUTPUT_FOLDER + "/" + filename));
            try
            {
                string[] filePaths = Directory.GetFiles(pdf_uploads, "*.pdf");
                List<string> files = new List<string>();
                foreach (var c in filePaths)
                {
                    files.Add(c);
                }

                for (int i = 0; i < files.Count; i++)
                {
                    string FILE2 = files[i].ToString();
                    PdfDocument pdfDocument2 = new PdfDocument(new PdfReader(FILE2));
                    PdfMerger merger = new PdfMerger(pdfDocument);
                    merger.Merge(pdfDocument2, 1, pdfDocument2.GetNumberOfPages());
                    pdfDocument2.Close();
                }
                pdfDocument.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                pdfDocument.Close();
            }
            return filename;
        }
        public async Task<string> AzureFileUploadAsync(string fileName, string containerName, string MI_Id, string Foldername)
        {
            string myFile;
            string myFileName;
            Stream file;
            string newImageName = "";
            string newImageNamePath = "";
            try
            {

                string accountname = "";
                string accesskey = "";

                var dataq = _context.IVRM_Storage_path_Details.ToList();
                if (dataq.Count() > 0)
                {
                    accountname = dataq.FirstOrDefault().IVRM_SD_Access_Name;
                    accesskey = dataq.FirstOrDefault().IVRM_SD_Access_Key;
                }

                //file = new FileStream(fileName, FileMode.Open);
                //StorageCredentials cre = new StorageCredentials(accountname, accesskey);
                //CloudStorageAccount acc = new CloudStorageAccount(cre, useHttps: true);
                //CloudBlobClient blobClient = acc.CreateCloudBlobClient();
                //CloudBlobContainer container = blobClient.GetContainerReference(containerName);
                //var ext = ".pdf";
                //var guid = Guid.NewGuid().ToString();
                //newImageName = guid + ext;
                //string blobName = newImageName;
                //await container.CreateIfNotExistsAsync();

                //await container.SetPermissionsAsync(new BlobContainerPermissions
                //{
                //    PublicAccess = BlobContainerPublicAccessType.Blob
                //});


                //myFile = Path.GetExtension(fileName);
                //myFileName = Path.GetFileName(fileName);
                //CloudBlockBlob myBlockBlob = container.GetBlockBlobReference(MI_Id + "/" + Foldername + "/" + blobName);
                //myBlockBlob.Properties.ContentType = myFile;
                //var c = myBlockBlob.UploadFromStreamAsync(file);


                StorageCredentials cre = new StorageCredentials(accountname, accesskey);
                CloudStorageAccount acc = new CloudStorageAccount(cre, useHttps: true);
                CloudBlobClient blobClient = acc.CreateCloudBlobClient();
                CloudBlobContainer container = blobClient.GetContainerReference(containerName);
                var ext = ".pdf";
                var guid = Guid.NewGuid().ToString();
                newImageName = guid + ext;
                string blobName = newImageName;
                await container.CreateIfNotExistsAsync();
                await container.SetPermissionsAsync(new BlobContainerPermissions
                {
                    PublicAccess = BlobContainerPublicAccessType.Blob
                });
                var blockBlob = container.GetBlockBlobReference(MI_Id + "/" + Foldername + "/" + blobName);

                using (var fileStream = System.IO.File.OpenRead(fileName))
                {
                    await blockBlob.UploadFromStreamAsync(fileStream);
                }
                newImageNamePath = blockBlob.Uri.ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return newImageNamePath;
        }
        public void imgtopdf()
        {
            // string[] filenamelist = filename.Split(".");
            ////Document document = new Document(PageSize.A4, 10f, 10f, 10f, 10f);
            //Document document = new Document();
            //using (var stream = new FileStream(filenamelist[0]+"g"+ ".pdf", FileMode.Create, FileAccess.Write, FileShare.None))
            //{
            //    PdfWriter.GetInstance(document, stream);
            //    document.Open();
            //    using (var imageStream = new FileStream(filepath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            //    {                    
            //        var image = iTextSharp.text.Image.GetInstance(imageStream);
            //        image.ScaleToFit(800,1000);                   
            //        document.Add(image);
            //    }
            //    document.Close();
            //}
        }
    }
    public class filedetails
    {
        public string name { get; set; }
        public string path { get; set; }
    }
}