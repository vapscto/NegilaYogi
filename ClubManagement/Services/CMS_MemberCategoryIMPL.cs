using DataAccessMsSqlServerProvider.com.vapstech.ClubManagement;
using DomainModel.Model.com.vapstech.ClubManagement;
using PreadmissionDTOs.com.vaps.ClubManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClubManagement.Services
{
    public class CMS_MemberCategoryIMPL : Interfaces.CMS_MemberCategoryInterface
    {
        public ClubManagementContext _CmsContext;
        public CMS_MemberCategoryIMPL(ClubManagementContext cmsContext)
        {
            _CmsContext = cmsContext;
        }
        public CMS_MemberCategoryDTO loaddata(int id)
        {

            CMS_MemberCategoryDTO dto = new CMS_MemberCategoryDTO();

            dto.getreport = _CmsContext.CMS_Member_CategoryDMO.Where(R => R.MI_Id == id).Distinct().ToArray();
              return dto;

        }
        //edit
        public CMS_MemberCategoryDTO edit(CMS_MemberCategoryDTO dto)
        {
            try
            {
                if(dto.CMSMCAT_Id  > 0)
                {
                    dto.edit = _CmsContext.CMS_Member_CategoryDMO.Where(R => R.MI_Id == dto.MI_Id && R.CMSMCAT_Id == dto.CMSMCAT_Id).Distinct().ToArray();

                }
                else
                {
                    dto.returnval = "admin";
                }
              
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                dto.returnval = "admin";
            }
             return dto;

        }

        public CMS_MemberCategoryDTO savedata(CMS_MemberCategoryDTO data)
        {
            try
            {
               if(data.CMSMCAT_Id > 0)
                {
                    var result = _CmsContext.CMS_Member_CategoryDMO.Where(P => P.MI_Id == data.MI_Id && P.CMSMCAT_Id == data.CMSMCAT_Id).FirstOrDefault();
                    if (result.CMSMCAT_Id > 0)
                    {
                        result.CMSMCAT_CategoryName = data.CMSMCAT_CategoryName;
                        result.CMSMCAT_CategoryCode = data.CMSMCAT_CategoryCode;
                        result.CMSMCAT_AllowCreditTransFlg = data.CMSMCAT_AllowCreditTransFlg;
                        result.CMSMCAT_MaxCreditLimit = data.CMSMCAT_MaxCreditLimit;
                        result.CMSMCAT_MaxNoOfGuest = data.CMSMCAT_MaxNoOfGuest;
                        result.CMSMCAT_EligibleForProposerFlg = false;
                        result.CMSMCAT_MinTransApplFlg = false;
                        result.CMSMCAT_MinTransAmt = data.CMSMCAT_MinTransAmt;
                        result.CMSMCAT_AllowBlockFlg = false;
                        result.CMSMCAT_AllowTerminateFlg = false;
                        result.CMSMCAT_PayLateFeeInterestFlg = false;
                        result.CMSMCAT_TakeCompulsoryServicesFlg = false;
                        result.CMSMCAT_MaxNoOfDependents = data.CMSMCAT_MaxNoOfDependents;
                        result.CMSMCAT_MembershipExpiryFlg = data.CMSMCAT_MembershipExpiryFlg;
                        result.CMSMCAT_MembershipDuration = data.CMSMCAT_MembershipDuration;
                        result.CMSMCAT_UpdatedDate = DateTime.Now;
                        result.CMSMCAT_UpdatedBy = data.UserId;
                        _CmsContext.Update(result);
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
                    else
                    {
                        data.returnval = "admin";
                    }
                }
                else
                {
                    CMS_Member_CategoryDMO obj = new CMS_Member_CategoryDMO();
                    obj.MI_Id = data.MI_Id;
                    obj.CMSMCAT_CategoryName = data.CMSMCAT_CategoryName;
                    obj.CMSMCAT_CategoryCode = data.CMSMCAT_CategoryCode;
                    obj.CMSMCAT_AllowCreditTransFlg = data.CMSMCAT_AllowCreditTransFlg;
                    obj.CMSMCAT_MaxCreditLimit = data.CMSMCAT_MaxCreditLimit;
                    obj.CMSMCAT_MaxNoOfGuest = data.CMSMCAT_MaxNoOfGuest;
                    obj.CMSMCAT_EligibleForProposerFlg = false;
                    obj.CMSMCAT_MinTransApplFlg = false;
                    obj.CMSMCAT_MinTransAmt = data.CMSMCAT_MinTransAmt;
                    obj.CMSMCAT_AllowBlockFlg = false;
                    obj.CMSMCAT_AllowTerminateFlg = false;
                    obj.CMSMCAT_PayLateFeeInterestFlg = false;
                    obj.CMSMCAT_TakeCompulsoryServicesFlg = false;
                    obj.CMSMCAT_MaxNoOfDependents = data.CMSMCAT_MaxNoOfDependents;
                    obj.CMSMCAT_MembershipExpiryFlg = data.CMSMCAT_MembershipExpiryFlg;
                    obj.CMSMCAT_MembershipDuration = data.CMSMCAT_MembershipDuration;
                    obj.CMSMCAT_ActiveFlag = true;
                    obj.CMSMCAT_CreatedDate = DateTime.Now;
                    obj.CMSMCAT_CreatedBy = data.UserId;
                    obj.CMSMCAT_UpdatedDate = DateTime.Now;
                    obj.CMSMCAT_UpdatedBy = data.UserId;
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
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                data.returnval = "admin";
            }
            return data;
        }
        //deactive
        public CMS_MemberCategoryDTO deactive(CMS_MemberCategoryDTO data)
        {
            try
            {
                if (data.CMSMCAT_Id > 0)
                {
                    var result = _CmsContext.CMS_Member_CategoryDMO.Where(P => P.MI_Id == data.MI_Id && P.CMSMCAT_Id == data.CMSMCAT_Id).FirstOrDefault();
                    if(result.CMSMCAT_Id > 0)
                    {
                        if(result.CMSMCAT_ActiveFlag==true)
                        {
                            result.CMSMCAT_ActiveFlag = false;
                        }
                        else
                        {
                            result.CMSMCAT_ActiveFlag = true;
                        }
                        result.CMSMCAT_UpdatedBy = data.UserId;
                        result.CMSMCAT_UpdatedDate = DateTime.Now;
                        _CmsContext.Update(result);
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
