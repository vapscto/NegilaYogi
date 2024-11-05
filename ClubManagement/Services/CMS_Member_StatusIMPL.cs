using DataAccessMsSqlServerProvider.com.vapstech.ClubManagement;
using DomainModel.Model.com.vapstech.ClubManagement;
using PreadmissionDTOs.com.vaps.ClubManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClubManagement.Services
{
    public class CMS_Member_StatusIMPL : Interfaces.CMS_Member_StatusInterface
    {
        public ClubManagementContext _CmsContext;
        public CMS_Member_StatusIMPL(ClubManagementContext cmsContext)
        {
            _CmsContext = cmsContext;
        }
        public CMS_Member_StatusDTO loaddata(int id)
        {

            CMS_Member_StatusDTO dto = new CMS_Member_StatusDTO();
            try
            {
                dto.getreport = (from a in _CmsContext.CMS_Member_StatusDMO
                                 from b in _CmsContext.CMS_MasterMemberDMO
                                 from c in _CmsContext.IVRM_Master_FinancialYear
                                 where (a.CMSMMEM_Id == b.CMSMMEM_Id && a.IMFY_Id == c.IMFY_Id && a.MI_Id == id)
                                 select new CMS_Member_StatusDTO
                                 {
                                     CMSMMEM_MemberFirstName = b.CMSMMEM_MemberFirstName + (string.IsNullOrEmpty(b.CMSMMEM_MemberMiddleName) ? "" : ' ' + b.CMSMMEM_MemberLastName) + (string.IsNullOrEmpty(b.CMSMMEM_MemberLastName) ? "" : ' ' + b.CMSMMEM_MemberLastName) +(string.IsNullOrEmpty(b.CMSMMEM_MembershipNo) ? "" : '/' + b.CMSMMEM_MembershipNo).Trim(),
                                     CMSMEMSTS_Id=a.CMSMEMSTS_Id,
                                     CMSMMEM_Id=a.CMSMMEM_Id,
                                     IMFY_Id=a.IMFY_Id,
                                     IMFY_FinancialYear=c.IMFY_FinancialYear,
                                     CMSMEMSTS_OpeningBalance=a.CMSMEMSTS_OpeningBalance,
                                     CMSMEMSTS_OBCRDRFlg=a.CMSMEMSTS_OBCRDRFlg,
                                     CMSMEMSTS_TotalDR=a.CMSMEMSTS_TotalDR,
                                     CMSMEMSTS_TotalDRTrans=a.CMSMEMSTS_TotalDRTrans,
                                     CMSMEMSTS_TotalCRTrans=a.CMSMEMSTS_TotalCRTrans,
                                     CMSMEMSTS_ClosingBalance=a.CMSMEMSTS_ClosingBalance,
                                     CMSMEMSTS_CBDRDRFlg=a.CMSMEMSTS_CBDRDRFlg,
                                     CMSMEMSTS_ActiveFlg=a.CMSMEMSTS_ActiveFlg,
                                     CMSMEMSTS_CreatedDate=a.CMSMEMSTS_CreatedDate

                                 }
                          ).Distinct().OrderByDescending(R => R.CMSMEMSTS_CreatedDate).ToArray();
                dto.finacial = _CmsContext.IVRM_Master_FinancialYear.Distinct().ToArray();
                dto.getname = (from a in _CmsContext.CMS_MasterMemberDMO
                               where (a.MI_Id == id && a.CMSMMEM_ActiveFlag == true)
                               select new CMS_Member_StatusDTO
                               {
                                   
                                   CMSMMEM_MemberFirstName = a.CMSMMEM_MemberFirstName + (string.IsNullOrEmpty(a.CMSMMEM_MemberMiddleName) ? "" : ' ' + a.CMSMMEM_MemberLastName) + (string.IsNullOrEmpty(a.CMSMMEM_MemberLastName) ? "" : ' ' + a.CMSMMEM_MemberLastName) + (string.IsNullOrEmpty(a.CMSMMEM_MembershipNo) ? "" : '/' + a.CMSMMEM_MembershipNo).Trim(),
                                   CMSMMEM_Id=a.CMSMMEM_Id
                               }
                              ).Distinct().ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                dto.returnval = "admin";
            }                       
            return dto;

        }
        public CMS_Member_StatusDTO savedata(CMS_Member_StatusDTO data)
        {
            try
            {
               if(data.CMSMEMSTS_Id > 0)
                {
                    var update = _CmsContext.CMS_Member_StatusDMO.Where(R => R.CMSMEMSTS_Id == data.CMSMEMSTS_Id && R.MI_Id == data.MI_Id).FirstOrDefault();
                    update.CMSMMEM_Id = data.CMSMMEM_Id;
                    update.IMFY_Id = data.IMFY_Id;
                    update.CMSMEMSTS_OpeningBalance = data.CMSMEMSTS_OpeningBalance;
                    update.CMSMEMSTS_OBCRDRFlg = data.CMSMEMSTS_OBCRDRFlg;
                    update.CMSMEMSTS_TotalDR = data.CMSMEMSTS_TotalDR;
                    update.CMSMEMSTS_TotalDRTrans = data.CMSMEMSTS_TotalDRTrans;
                    update.CMSMEMSTS_TotalCRTrans = data.CMSMEMSTS_TotalCRTrans;
                    update.CMSMEMSTS_ClosingBalance = data.CMSMEMSTS_ClosingBalance;
                    update.CMSMEMSTS_CBDRDRFlg = data.CMSMEMSTS_CBDRDRFlg;
                    update.CMSMEMSTS_UpdatedDate = DateTime.Now;
                    update.CMSMEMSTS_UpdatedBy = data.UserId;
                    _CmsContext.Update(update);
                    var contactExists = _CmsContext.SaveChanges();
                    if (contactExists > 0)
                    {
                        data.returnval = "update";
                    }
                    else
                    {
                        data.returnval = "Notupdate";
                    }
                }
                else
                {
                    CMS_Member_StatusDMO obj = new CMS_Member_StatusDMO();
                    obj.MI_Id = data.MI_Id;
                    obj.CMSMMEM_Id = data.CMSMMEM_Id;
                    obj.IMFY_Id = data.IMFY_Id;
                    obj.CMSMEMSTS_OpeningBalance = data.CMSMEMSTS_OpeningBalance;
                    obj.CMSMEMSTS_OBCRDRFlg = data.CMSMEMSTS_OBCRDRFlg;
                    obj.CMSMEMSTS_TotalDR = data.CMSMEMSTS_TotalDR;
                    obj.CMSMEMSTS_TotalDRTrans = data.CMSMEMSTS_TotalDRTrans;
                    obj.CMSMEMSTS_TotalCRTrans = data.CMSMEMSTS_TotalCRTrans;
                    obj.CMSMEMSTS_ClosingBalance = data.CMSMEMSTS_ClosingBalance;
                    obj.CMSMEMSTS_CBDRDRFlg = data.CMSMEMSTS_CBDRDRFlg;
                    obj.CMSMEMSTS_ActiveFlg = true;
                    obj.CMSMEMSTS_CreatedDate = DateTime.Now;
                    obj.CMSMEMSTS_CreatedBy = data.UserId;
                    obj.CMSMEMSTS_UpdatedDate = DateTime.Now;
                    obj.CMSMEMSTS_UpdatedBy = data.UserId;
                    _CmsContext.Add(obj);
                    var contactExists = _CmsContext.SaveChanges();
                    if (contactExists > 0)
                    {
                        
                        data.returnval = "save";
                    }
                    else
                    {
                        data.returnval = "Notsave";
                    }

                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                data.returnval = "admin";
            }
            return data;
        }
        //deactive
        public CMS_Member_StatusDTO deactive(CMS_Member_StatusDTO data)
        {
            try
            {
                if (data.CMSMEMSTS_Id > 0)
                {
                    var update = _CmsContext.CMS_Member_StatusDMO.Where(R => R.CMSMEMSTS_Id == data.CMSMEMSTS_Id).FirstOrDefault();
                    if (update.CMSMEMSTS_ActiveFlg == true)
                    {
                        update.CMSMEMSTS_ActiveFlg = false;
                    }
                    else
                    {
                        update.CMSMEMSTS_ActiveFlg = true;
                    }
                    update.CMSMEMSTS_UpdatedBy = data.UserId;
                    update.CMSMEMSTS_UpdatedDate = DateTime.Now;
                    _CmsContext.Update(update);
                    int i = _CmsContext.SaveChanges();
                    if (i > 0)
                    {
                        data.returnval = "active";
                    }
                    else
                    {
                        data.returnval = "notactive";
                    }
                }
                else
                {
                    data.returnval = "admin";
                }



            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                data.returnval = "admin";
            }
            return data;
        }

    }
}
