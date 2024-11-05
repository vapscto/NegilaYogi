using DataAccessMsSqlServerProvider.com.vapstech.ClubManagement;
using DomainModel.Model.com.vapstech.ClubManagement;
using PreadmissionDTOs.com.vaps.ClubManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClubManagement.Services
{
    public class CMS_MembershipApplicationIMPL : Interfaces.CMS_MembershipApplicationInterface
    {
        public ClubManagementContext _CmsContext;
        public CMS_MembershipApplicationIMPL(ClubManagementContext cmsContext)
        {
            _CmsContext = cmsContext;
        }
        public CMS_MembershipApplicationDTO loaddata(int id)
        {

            CMS_MembershipApplicationDTO dto = new CMS_MembershipApplicationDTO();
            //  dto.pages = _CmsContext.CMS_Master_InstallmentsDMO.Distinct().ToArray();
            dto.getarray = _CmsContext.CMS_MembershipApplicationDMO.Where(P => P.MI_Id == id).Distinct().ToArray();
            

            return dto;

        }
        public CMS_MembershipApplicationDTO savedata(CMS_MembershipApplicationDTO data)
        {
            try
            {
                if(data.CMSMAPPL_Id > 0)
                {
                    var resultwo = _CmsContext.CMS_MembershipApplicationDMO.Where(R => R.CMSMAPPL_Id == data.CMSMAPPL_Id && R.MI_Id==data.MI_Id).FirstOrDefault();
                    if(resultwo.CMSMAPPL_Id > 0)
                    {
                        resultwo.CMSMAPPL_ApplicantsName = data.CMSMAPPL_ApplicantsName;
                        resultwo.CMSMAPPL_Address = data.CMSMAPPL_Address;
                        resultwo.CMSMAPPL_PhoneNo = data.CMSMAPPL_PhoneNo;
                        resultwo.CMSMAPPL_EMailId = data.CMSMAPPL_EMailId;
                        resultwo.CMSMAPPL_ApplicationDate = data.CMSMAPPL_ApplicationDate;
                        resultwo.CMSMAPPL_ApplicationNo = data.CMSMAPPL_ApplicationNo;

                        resultwo.CMSMAPPL_UpdatedDate = DateTime.Now;
                        resultwo.CMSMAPPL_UpdatedBy = data.UserId;
                        _CmsContext.Update(resultwo);

                        var contactExists = _CmsContext.SaveChanges();
                        if (contactExists == 1)

                        {
                            data.returnval = "update";

                        }
                        else
                        {
                            data.returnval = "Notupdate";
                        }


                    }
                }
                else
                {
                    var result = _CmsContext.CMS_MembershipApplicationDMO.Where(R => R.CMSMAPPL_ApplicantsName == data.CMSMAPPL_ApplicantsName && R.CMSMAPPL_ApplicationNo == data.CMSMAPPL_ApplicationNo && R.CMSMAPPL_PhoneNo == data.CMSMAPPL_PhoneNo && R.MI_Id==data.MI_Id).ToList();
                    if(result.Count > 0)
                    {
                        data.returnval = "exit";
                    }
                    else
                    {
                        CMS_MembershipApplicationDMO obj = new CMS_MembershipApplicationDMO();
                        obj.MI_Id = data.MI_Id;
                        obj.CMSMAPPL_ApplicantsName = data.CMSMAPPL_ApplicantsName;
                        obj.CMSMAPPL_Address = data.CMSMAPPL_Address;
                        obj.CMSMAPPL_PhoneNo = data.CMSMAPPL_PhoneNo;
                        obj.CMSMAPPL_EMailId = data.CMSMAPPL_EMailId;
                        obj.CMSMAPPL_ApplicationDate = data.CMSMAPPL_ApplicationDate;
                        obj.CMSMAPPL_ApplicationNo = data.CMSMAPPL_ApplicationNo;
                        obj.CMSMAPPL_ApplicationStatus = "Pending";
                        obj.CMSMAPPL_ReferredBy = data.CMSMAPPL_ReferredBy;
                        obj.CMSMAPPL_ApplCancelledFlg = false;
                        obj.CMSMAPPL_ApplCancelledDate = data.CMSMAPPL_ApplCancelledDate;
                        obj.CMSMAPPL_ApplCancelledReason = data.CMSMAPPL_ApplCancelledReason;
                        obj.CMSMAPPL_ActiveFlag = true;
                        obj.CMSMAPPL_CreatedDate = DateTime.Now;
                        obj.CMSMAPPL_CreatedBy = data.UserId;
                        obj.CMSMAPPL_UpdatedDate = DateTime.Now;
                        obj.CMSMAPPL_UpdatedBy = data.UserId;
                        _CmsContext.Add(obj);
                        var contactExists = _CmsContext.SaveChanges();
                        if (contactExists == 1)
                        {
                            data.returnval = "save";

                        }
                        else
                        {
                            data.returnval = "Notsave";
                        }

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
        public CMS_MembershipApplicationDTO deactive(CMS_MembershipApplicationDTO data)
        {
            try
            {
                if (data.CMSMAPPL_Id > 0)
                {
                    var resultwo = _CmsContext.CMS_MembershipApplicationDMO.Where(R => R.CMSMAPPL_Id == data.CMSMAPPL_Id).FirstOrDefault();
                    if (resultwo.CMSMAPPL_ActiveFlag == true)
                    {
                        resultwo.CMSMAPPL_ActiveFlag = false;
                    }
                    else
                    {
                        resultwo.CMSMAPPL_ActiveFlag = true;
                    }
                    resultwo.CMSMAPPL_UpdatedBy = data.UserId;
                    resultwo.CMSMAPPL_UpdatedDate = DateTime.Now;
                    _CmsContext.Update(resultwo);
                    var contactExists = _CmsContext.SaveChanges();
                    if (contactExists == 1)
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
