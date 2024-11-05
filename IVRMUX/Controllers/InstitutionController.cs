using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs;
using corewebapi18072016.Delegates;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using DataAccessMsSqlServerProvider;
using Microsoft.AspNetCore.Hosting;

namespace corewebapi18072016.Controllers
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class InstitutionController : Controller
    {
        private readonly DomainModelMsSqlServerContext _context;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IHttpContextAccessor _accessor;
        public InstitutionController(IHostingEnvironment hostingEnvironment, IHttpContextAccessor httpContextAccessor, DomainModelMsSqlServerContext context)
        {
            _hostingEnvironment = hostingEnvironment;
            _accessor = httpContextAccessor;
            _context = context;
        }

        InstitutionDelegate instute = new InstitutionDelegate();
        
        [HttpPost]
        public InstitutionDTO SaveInstitution([FromBody] InstitutionDTO Ins)
        {
            Ins.sessionMO_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MO_Id"));
            Ins.sessionMI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            Ins.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return instute.saveInstitutiondetails(Ins);
        }

        [Route("getInstituteById/{id:int}")]
        public InstitutionDTO GetInstituteDetailsById(int id)
        {
            InstitutionDTO data = new InstitutionDTO();
            data= instute.getInstitutionDetailsbyInstituteId(id);

           
            //string accountname = "";
            //string accesskey = "";
            //var data2 = _context.IVRM_Storage_path_Details.ToList();
            //if (data2.Count() > 0)
            //{
            //    accountname = data2.FirstOrDefault().IVRM_SD_Access_Name;
            //    accesskey = data2.FirstOrDefault().IVRM_SD_Access_Key;
            //}

           


            //StorageCredentials cre = new StorageCredentials(accountname, accesskey);
            //CloudStorageAccount acc = new CloudStorageAccount(cre, useHttps: true);
           
            //CloudBlobClient blobClient = acc.CreateCloudBlobClient();
            //if (data.MI_BackgroundImage != null && data.MI_BackgroundImage != "")
            //{
             
            //    CloudBlobContainer container = blobClient.GetContainerReference("files" +"/"+ "InstitutionBackgroundImage");
            //    string filename = data.MI_BackgroundImage;
            //    var blockBlob = container.GetBlockBlobReference(filename);

            //    var blobsName = blockBlob.Uri.ToString();

            //    data.MI_BackgroundImage = blobsName;
            //}
            //else
            //{
            //    data.MI_BackgroundImage = "";
            //}
            //if (data.MI_Logo != null && data.MI_Logo != "")
            //{
               
            //    CloudBlobContainer container = blobClient.GetContainerReference("files" + "/" + "InstitutionLogo");
            //    string filename = data.MI_Logo;
            //    var blockBlob = container.GetBlockBlobReference(filename);

            //    var blobsName = blockBlob.Uri.ToString();

            //    data.MI_Logo = blobsName;
            //}
            //else
            //{
            //    data.MI_Logo = "";
            //}
            return data;
                

        }
        
        [HttpDelete]
        [Route("deletedetails/{id:int}")]
        public InstitutionDTO Delete(int id)
        {
            return instute.deleterec(id);
        }

        [Route("getalldetails")]
        public InstitutionDTO Get([FromBody] InstitutionDTO Ins)
        {
            Ins.sessionMO_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MO_Id"));
            Ins.sessionMI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));

            Ins.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));

            Ins.instutePagination.PageSize = 5;

            Ins.instutePagination.CurrentPageIndex = 1;

            Ins.subscriptionPagination.CurrentPageIndex = 1;
            Ins.subscriptionPagination.PageSize = 5;
            return instute.getInstitutiondata(Ins);
        }

        [Route("DuplicateInstitionDataFind")]
        public InstitutionDTO DuplicateInstitionDataFind([FromBody] InstitutionDTO Ins)
        {
            return instute.DuplicateInstitionDataFind(Ins);
        }

        [Route("saveSubscription")]
        public Master_Institution_SubscriptionValidityDTO saveSubscription([FromBody]Master_Institution_SubscriptionValidityDTO sub)
        {
            return instute.SaveSubscriptionValidity(sub);
        }       

        [HttpDelete]
        [Route("deleteSubscription/{id:int}")]
        public Master_Institution_SubscriptionValidityDTO deleteSubscription(int id)
        {
            return instute.deleteSubscriptionrec(id);
        }

        // for search and pagination

        [Route("getInstitutionSearchedDetails")]
        public InstitutionDTO InstitutionSearchedDetails([FromBody] SortingPagingInfoDTO Ins)
        {
            Ins.PageSize = 5;
            return instute.getInstitutionSearchedDetails(Ins);
        }

        [Route("getSubscriptionSearchedDetails")]
        public Master_Institution_SubscriptionValidityDTO SubscriptionSearchedDetails([FromBody]SortingPagingInfoDTO sub)
        {
            sub.PageSize = 5;
            return instute.getSubscriptionSearchedDetails(sub);
        }

        [Route("OnClickSaveAutoMapping")]
        public InstitutionDTO OnClickSaveAutoMapping([FromBody] InstitutionDTO Ins)
        {
            return instute.OnClickSaveAutoMapping(Ins);
        }

    }
}