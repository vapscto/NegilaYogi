using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DataAccessMsSqlServerProvider.com.vapstech.Portals;
using ImageMagick;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using PortalHub.com.vaps.MobileApp.Interfaces;
using PreadmissionDTOs.com.vaps.MobileApp;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PortalHub.com.vaps.MobileApp.Controllers
{
    [Route("api/[controller]")]
    public class MobileCtrl : Controller
    {
        public MobileInterface _admissionInterface;
        private readonly PortalContext _PortalContext;
        public MobileCtrl(PortalContext cpContext, MobileInterface admissionInterface)
        {
            _PortalContext = cpContext;
            _admissionInterface = admissionInterface;
        }



        [Route("getAcademicYear")]
        public AdmissionDTO.getAcademicyear getAcademicYear([FromBody] AdmissionDTO.getAcademicyear obj)
        {
            return _admissionInterface.getAcademicYear(obj);
        }

        [Route("getclass")]
        public AdmissionDTO.getClass getclass([FromBody] AdmissionDTO.getClass obj)
        {
            return _admissionInterface.getclass(obj);
        }

        [Route("getsection")]
        public AdmissionDTO.getSection getsection([FromBody] AdmissionDTO.getSection obj)
        {
            return _admissionInterface.getsection(obj);
        }
        [Route("AcademicyearwiseClass")]
        public AdmissionDTO.getClass AcademicyearwiseClass([FromBody] AdmissionDTO.getClass obj)
        {
            return _admissionInterface.AcademicyearwiseClass(obj);
        }
        [Route("AcademicyearwiseClassSection")]
        public AdmissionDTO.getSection AcademicyearwiseClassSection([FromBody] AdmissionDTO.getSection obj)
        {
            return _admissionInterface.AcademicyearwiseClassSection(obj);
        }
        [Route("AcademicyearwiseClassSectionStudent")]
        public AdmissionDTO.getstudent AcademicyearwiseClassSectionStudent([FromBody] AdmissionDTO.getstudent obj)
        {
            return _admissionInterface.AcademicyearwiseClassSectionStudent(obj);
        }





        //Certifecate Apply
        [Route("getOnloadCertificateapply")]
        public AdmissionDTO.getCertificateApply getOnloadCertificateapply([FromBody] AdmissionDTO.getCertificateApply obj)
        {
            return _admissionInterface.getOnloadCertificateapply(obj);
        }
        [Route("applyCertifateApplySave")]
        Task<AdmissionDTO.saveCertificateApply> applyCertifateApplySave([FromBody] AdmissionDTO.saveCertificateApply obj)
        {
            return _admissionInterface.applyCertifateApplySave(obj);
        }
        [Route("getCertificateDetails")]
        public AdmissionDTO.getCertificateDetails getCertificateDetails([FromBody] AdmissionDTO.getCertificateDetails obj)
        {
            return _admissionInterface.getCertificateDetails(obj);
        }

        //Student Feedback
        [Route("getloadFeedbackdata")]
        public AdmissionDTO.getloadFeedbackdata getloadFeedbackdata([FromBody] AdmissionDTO.getloadFeedbackdata obj)
        {
            return _admissionInterface.getloadFeedbackdata(obj);
        }
        [Route("savefeedback")]
        public AdmissionDTO.saveFeedbackFormDTO savefeedback([FromBody] AdmissionDTO.saveFeedbackFormDTO obj)
        {
            return _admissionInterface.savefeedback(obj);
        }


        //Interaction
        [Route("getInteractionloaddata")]
        public Task<AdmissionDTO.OnloadInteractionsDTO> getInteractionloaddata([FromBody] AdmissionDTO.OnloadInteractionsDTO obj)
        {
            return _admissionInterface.getInteractionloaddata(obj);
        }
        [Route("intractionreply")]
        public Task<AdmissionDTO.replyInteractionsDTO> intractionreply([FromBody] AdmissionDTO.replyInteractionsDTO obj)
        {
            return _admissionInterface.intractionreply(obj);
        }
        [Route("intractionreplysave")]
        public AdmissionDTO.replysaveInteractionsDTO intractionreplysave([FromBody] AdmissionDTO.replysaveInteractionsDTO obj)
        {
            return _admissionInterface.intractionreplysave(obj);
        }
        [Route("intractioncomposeOnload")]
        public Task<AdmissionDTO.ComposeOnloadInteractionsDTO> intractioncomposeOnload([FromBody] AdmissionDTO.ComposeOnloadInteractionsDTO obj)
        {
            return _admissionInterface.intractioncomposeOnload(obj);
        }
        [Route("composeOnselectOFTeacher")]
        public Task<AdmissionDTO.composeOnselectOFTeacher> composeOnselectOFTeacher([FromBody] AdmissionDTO.composeOnselectOFTeacher obj)
        {
            return _admissionInterface.composeOnselectOFTeacher(obj);
        }
        [Route("ComposeStudentSubmit")]
        public AdmissionDTO.composeOnsubmitOFStudent ComposeStudentSubmit([FromBody] AdmissionDTO.composeOnsubmitOFStudent obj)
        {
            return _admissionInterface.ComposeStudentSubmit(obj);
        }


        //class wise timetable by kavita
        [Route("ttgetloaddata")]
        public AdmissionDTO.ttLoadData ttgetloaddata([FromBody] AdmissionDTO.ttLoadData obj)
        {
            return _admissionInterface.ttgetloaddata(obj);
        }

        [Route("getStudentTT")]
        public AdmissionDTO.ttGetStudent getStudentTT([FromBody] AdmissionDTO.ttGetStudent obj)
        {
            return _admissionInterface.getStudentTT(obj);
        }
        [Route("stdDashboardExam")]
        public AdmissionDTO.stdDashboardExam stdDashboardExam([FromBody] AdmissionDTO.stdDashboardExam obj)
        {
            return _admissionInterface.stdDashboardExam(obj);
        }

        //class wise timetable
        //Attendance

        [Route("Attgetloaddata")]
        public AdmissionDTO.attGetLoadData Attgetloaddata([FromBody] AdmissionDTO.attGetLoadData obj)
        {
            return _admissionInterface.Attgetloaddata(obj);
        }

        [Route("attGetdetails")]
        public AdmissionDTO.attGetdetails attGetdetails([FromBody] AdmissionDTO.attGetdetails obj)
        {
            return _admissionInterface.attGetdetails(obj);
        }



        //Added by sanjeev
        //onclick_LIB
        [Route("onclick_LIB")]
        public AdmissionDTO.onclick_LIB onclick_LIB([FromBody] AdmissionDTO.onclick_LIB obj)
        {
            return _admissionInterface.onclick_LIB(obj);
        }
        //getCoedata
        [Route("getloaddataCoe")]
        public AdmissionDTO.getCoedata getloaddataCoe([FromBody] AdmissionDTO.getCoedata obj)
        {
            return _admissionInterface.getloaddataCoe(obj);
        }
        [HttpPost]
        [Route("getcoedata")]
        public AdmissionDTO.getCoedata getcoedata([FromBody]AdmissionDTO.getCoedata sddto)
        {
            return _admissionInterface.getcoedata(sddto);
        }



        //  added by roopa
        [Route("onclick_Classwork_load")]
        public AdmissionDTO.onclickClasswork onclick_Classwork_load([FromBody] AdmissionDTO.onclickClasswork data)
        {
            return _admissionInterface.onclick_Classwork_load(data);
        }

        [Route("onclick_Homework_load")]
        public AdmissionDTO.onclick_Homework_load onclick_Homework_load([FromBody] AdmissionDTO.onclick_Homework_load data)
        {
            return _admissionInterface.onclick_Homework_load(data);
        }


        [Route("onclick_notice")]
        public AdmissionDTO.onclick_notice onclick_notice([FromBody] AdmissionDTO.onclick_notice data)
        {
            return _admissionInterface.onclick_notice(data);
        }


        //upload homework


        [Route("HomeworkUpload")]
        public async Task<List<AdmissionDTO.filedetails>> HomeworkkUpload(AdmissionDTO.HomeworkkUpload fileDescriptionShort)
        {
            var names = new List<string>();
            var contentTypes = new List<string>();

            string newImageName = "";
            List<string> Filesname = new List<string>();
            List<AdmissionDTO.filedetails> FilesPaths = new List<AdmissionDTO.filedetails>();
            AdmissionDTO stu = new AdmissionDTO();
            if (ModelState.IsValid)
            {
                foreach (var file in fileDescriptionShort.File)
                {
                    if (file.Length > 0)
                    {
                        try
                        {
                            string containername = "files";
                            newImageName = await uploadtoazure_1(file, containername, "HomeworkUpload", fileDescriptionShort.MI_Id);

                            AdmissionDTO.filedetails emp = new AdmissionDTO.filedetails();
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


        public async Task<string> uploadtoazure_1(IFormFile file, string containername, string folder, long MI_Id)
        {
            string newImageNamePath = "";
            string newImageName = "";
            var newFileName = string.Empty;
            var fileName = string.Empty;
            var nameofthefile = "";
            string accountname = "";
            string accesskey = "";

            try
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

                        var exePath = Path.GetDirectoryName(System.Reflection
                           .Assembly.GetExecutingAssembly().CodeBase);

                        Regex appPathMatcher = new System.Text.RegularExpressions.Regex(@"(?<!fil)[A-Za-z]:\\+[\S\s]*?(?=\\+bin)");
                        var appRoot = appPathMatcher.Match(exePath).Value;
                        appRoot = appRoot.Replace("WebApplication1", "IVRMUX");
                        var uploads = Path.Combine(appRoot, "wwwroot\\UploadImages");


                        fileName = file.FileName;
                        using (var fileStream = new FileStream(Path.Combine(uploads, fileName), FileMode.Create))
                        {
                            await file.CopyToAsync(fileStream);
                            nameofthefile = fileName;
                        }
                        fileName = System.Net.Http.Headers.ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                        fileName = Path.Combine(appRoot, "wwwroot\\UploadImages") + $@"\{fileName}";
                    }
                    var data = _PortalContext.IVRM_Storage_path_Details.ToList();
                    if (data.Count() > 0)
                    {
                        accountname = data.FirstOrDefault().IVRM_SD_Access_Name;
                        accesskey = data.FirstOrDefault().IVRM_SD_Access_Key;
                    }
                    StorageCredentials cre = new StorageCredentials(accountname, accesskey);
                    BlobRequestOptions requestoptions = new Microsoft.WindowsAzure.Storage.Blob.BlobRequestOptions()
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
                    var blockBlob = container.GetBlockBlobReference(MI_Id + "/" + folder + "/" + blobName);
                    if (fileExt == ".jpg" || fileExt == ".png")
                    {
                        const int size = 900;
                        const int quality = 100;
                        using (var image = new MagickImage(fileName))
                        {
                            image.Resize(size, size);
                            //image.Rotate(90d);
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
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return newImageNamePath;
        }


        //getClassworksave 
        [Route("savecls_doc")]
        public AdmissionDTO.getClassworksave savecls_doc([FromBody] AdmissionDTO.getClassworksave data)
        {
            return _admissionInterface.savecls_doc(data);
        }

        //gethomeworksave
        [Route("savehome_doc")]
        public AdmissionDTO.gethomeworksave savehome_doc([FromBody] AdmissionDTO.gethomeworksave data)
        {
            return _admissionInterface.savehome_doc(data);
        }
        [Route("stdDashboardDet")]
        public AdmissionDTO.stdDashboardLoad stdDashboardDet([FromBody] AdmissionDTO.stdDashboardLoad obj)
        {
            return _admissionInterface.stdDashboardDet(obj);
        }

        [Route("stdFeeDue")]
        public AdmissionDTO.getstudent stdFeeDue([FromBody] AdmissionDTO.getstudent obj)
        {
            return _admissionInterface.stdFeeDue(obj);
        }

        [Route("daywiseTimetable")]
        public AdmissionDTO.daywiseTimetable daywiseTimetable([FromBody] AdmissionDTO.daywiseTimetable obj)
        {
            return _admissionInterface.daywiseTimetable(obj);
        }

        [Route("clgdaywiseTimetable")]
        public AdmissionDTO.daywiseTimetable clgdaywiseTimetable([FromBody] AdmissionDTO.daywiseTimetable obj)
        {
            return _admissionInterface.clgdaywiseTimetable(obj);
        }

        // Dashboard
        [Route("UserDashboardDetails")]
        public AdmissionDTO.userDashboardLoad UserDashboardDetails([FromBody] AdmissionDTO.userDashboardLoad obj)
        {
            return _admissionInterface.UserDashboardDetails(obj);
        }
        [Route("UserProfileDetails")]
        public AdmissionDTO.UserProfileDetailsDTO UserProfileDetails([FromBody] AdmissionDTO.UserProfileDetailsDTO obj)
        {
            return _admissionInterface.UserProfileDetails(obj);
        }
        //Staff Dashboard
        [Route("staffDashboardDetails")]
        public AdmissionDTO.staffDashboardLoad staffDashboardDetails([FromBody] AdmissionDTO.staffDashboardLoad obj)
        {
            return _admissionInterface.staffDashboardDetails(obj);
        }

        //Manager Dashboard
        [Route("ManagerDashboardDetails")]
        public AdmissionDTO.ManagerDashboard ManagerDashboardDetails([FromBody] AdmissionDTO.ManagerDashboard obj)
        {
            return _admissionInterface.ManagerDashboardDetails(obj);
        }

        //College Student Dashboard

        [Route("CollegeUserDashboardDetails")]
        public Task<AdmissionDTO.CollegeUserDashboardDetails> CollegeUserDashboardDetails([FromBody]AdmissionDTO.CollegeUserDashboardDetails data)
        {
            return _admissionInterface.CollegeUserDashboardDetails(data);
        }
       
        //CollegeUserProfileDetails
        [Route("CollegeUserProfileDetails")]
        public AdmissionDTO.CLGUserProfileDetailsDTO CollegeUserProfileDetails([FromBody] AdmissionDTO.CLGUserProfileDetailsDTO obj)
        {
            return _admissionInterface.CollegeUserProfileDetails(obj);
        }
        //LibraryDetails
        [Route("LibraryDetails")]
        public AdmissionDTO.ExamLibararyDTO LibraryDetails([FromBody] AdmissionDTO.ExamLibararyDTO obj)
        {
            return _admissionInterface.LibraryDetails(obj);
        }
        //FineIsuuesRecipt

        [Route("FineIsuuesRecipt")]
        public AdmissionDTO.ExamLibararyDTO FineIsuuesRecipt([FromBody] AdmissionDTO.ExamLibararyDTO obj)
        {
            return _admissionInterface.FineIsuuesRecipt(obj);
        }
        [Route("PushNotification")]
        public AdmissionDTO.PushNotification PushNotification([FromBody] AdmissionDTO.PushNotification obj)
        {
            return _admissionInterface.PushNotification(obj);
        }
        //NoticeBoadCollegeDto
        [Route("NoticeBoadCollege")]
        public AdmissionDTO.NoticeBoadCollegeDto NoticeBoadCollege([FromBody] AdmissionDTO.NoticeBoadCollegeDto obj)
        {
            return _admissionInterface.NoticeBoadCollege(obj);
        }

        [Route("StaffNoticeBoadCollege")]
        public AdmissionDTO.NoticeBoadCollegeDto StaffNoticeBoadCollege([FromBody] AdmissionDTO.NoticeBoadCollegeDto obj)
        {
            return _admissionInterface.StaffNoticeBoadCollege(obj);
        }

        [Route("NoticeBoadCollegedatawise")]
        public AdmissionDTO.NoticeBoadCollegeDto NoticeBoadCollegedatawise([FromBody] AdmissionDTO.NoticeBoadCollegeDto obj)
        {
            return _admissionInterface.NoticeBoadCollegedatawise(obj);
        }
        [Route("StaffNoticeBoadCollegedatawise")]
        public AdmissionDTO.NoticeBoadCollegeDto StaffNoticeBoadCollegedatawise([FromBody] AdmissionDTO.NoticeBoadCollegeDto obj)
        {
            return _admissionInterface.StaffNoticeBoadCollegedatawise(obj);
        }

        //NoticeBoadCollegeDto
        [Route("NoticeBoadCollege_onclick")]
        public AdmissionDTO.NoticeBoadCollegeDto NoticeBoadCollege_onclick([FromBody] AdmissionDTO.NoticeBoadCollegeDto obj)
        {
            return _admissionInterface.NoticeBoadCollege_onclick(obj);
        }
        [Route("StaffNoticeBoadCollege_onclick")]
        public AdmissionDTO.NoticeBoadCollegeDto StaffNoticeBoadCollege_onclick([FromBody] AdmissionDTO.NoticeBoadCollegeDto obj)
        {
            return _admissionInterface.StaffNoticeBoadCollege_onclick(obj);
        }
        //NoticeBordCollege

        [Route("shortageOfAttendanceAlert")]
        public Task<AdmissionDTO.ShortageOFAttandence> shortageOfAttendanceAlert([FromBody]AdmissionDTO.ShortageOFAttandence data)
        {
            return _admissionInterface.shortageOfAttendanceAlert(data);
        }


        [Route("clg_shortageOfAttendanceAlert")]
        public Task<AdmissionDTO.ShortageOFAttandence> clg_shortageOfAttendanceAlert([FromBody]AdmissionDTO.ShortageOFAttandence data)
        {
            return _admissionInterface.clg_shortageOfAttendanceAlert(data);
        }

        [Route("staffProfile")]
        public AdmissionDTO.staffProfileDTO staffProfile([FromBody] AdmissionDTO.staffProfileDTO obj)
        {
            return _admissionInterface.staffProfile(obj);
        }


        [Route("PushNotificationonload")]
        public AdmissionDTO.PushNotificationonload PushNotificationonload([FromBody] AdmissionDTO.PushNotificationonload obj)
        {
            return _admissionInterface.PushNotificationonload(obj);
        }
        [Route("NotificationonloadRead")]
        public AdmissionDTO.PushNotificationonload NotificationonloadRead([FromBody] AdmissionDTO.PushNotificationonload obj)
        {
            return _admissionInterface.NotificationonloadRead(obj);
        }
        [Route("AcademicwiseFeesDetails")]
        public AdmissionDTO.AcademicFeesData AcademicwiseFeesDetails([FromBody] AdmissionDTO.AcademicFeesData obj)
        {
            return _admissionInterface.AcademicwiseFeesDetails(obj);
        }
        [Route("AcademicwiseClassFeesDetails")]
        public AdmissionDTO.AcademicFeesData AcademicwiseClassFeesDetails([FromBody] AdmissionDTO.AcademicFeesData obj)
        {
            return _admissionInterface.AcademicwiseClassFeesDetails(obj);
        }

        [Route("Mobileversion_control")]
        public AdmissionDTO.versiondetails Mobileversion_control([FromBody] AdmissionDTO.versiondetails obj)
        {
            return _admissionInterface.Mobileversion_control(obj);
        }



        //College Staff TimeTable
        

        [Route("getdata")]
        public AdmissionDTO.ClgTimeTable getdata([FromBody] AdmissionDTO.ClgTimeTable data)
        {
            return _admissionInterface.getdata(data);
        }
        [Route("clggetdaily_data")]
        public AdmissionDTO.ClgTimeTable clggetdaily_data([FromBody] AdmissionDTO.ClgTimeTable data)
        {
            return _admissionInterface.clggetdaily_data(data);
        }


        //

        [Route("CLGstdFeeDue")]
        public AdmissionDTO.CLGgetstudent CLGstdFeeDue([FromBody] AdmissionDTO.CLGgetstudent obj)
        {
            return _admissionInterface.CLGstdFeeDue(obj);
        }
        [Route("clgfeedbackDetails")]
        public AdmissionDTO.clgfeedbackDetailsDTO clgfeedbackDetails([FromBody] AdmissionDTO.clgfeedbackDetailsDTO obj)
        {
            return _admissionInterface.clgfeedbackDetails(obj);
        }


    }
}
