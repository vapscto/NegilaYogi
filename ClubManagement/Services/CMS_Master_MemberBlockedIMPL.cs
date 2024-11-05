using DataAccessMsSqlServerProvider.com.vapstech.ClubManagement;
using DomainModel.Model.com.vapstech.ClubManagement;
using PreadmissionDTOs.com.vaps.ClubManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClubManagement.Services
{
    public class CMS_Master_MemberBlockedIMPL : Interfaces.CMS_Master_MemberBlockedInterface
    {
        public ClubManagementContext _CmsContext;
        public CMS_Master_MemberBlockedIMPL(ClubManagementContext cmsContext)
        {
            _CmsContext = cmsContext;
        }
        public CMS_Master_MemberBlockedDTO loaddata(int id)
        {
            CMS_Master_MemberBlockedDTO dto = new CMS_Master_MemberBlockedDTO();
            dto.getreport = (from a in _CmsContext.CMS_MasterMemberDMO
                             from b in _CmsContext.CMS_Master_MemberBlockedDMO
                             where (a.CMSMMEM_Id == b.CMSMMEM_Id && a.MI_Id == id)
                             select new CMS_Master_MemberBlockedDTO
                             {
                                 CMSMMEMBLK_Id = b.CMSMMEMBLK_Id,
                                 CMSMMEM_Id = b.CMSMMEM_Id,
                                 MemberName = a.CMSMMEM_MemberFirstName + (string.IsNullOrEmpty(a.CMSMMEM_MemberMiddleName) ? "" : ' ' + a.CMSMMEM_MemberLastName) + (string.IsNullOrEmpty(a.CMSMMEM_MemberLastName) ? "" : ' ' + a.CMSMMEM_MemberLastName) + (string.IsNullOrEmpty(a.CMSMMEM_MembershipNo) ? "" : '/' + a.CMSMMEM_MembershipNo).Trim(),
                                 CMSMMEMBLK_BlockedDate = b.CMSMMEMBLK_BlockedDate,
                                 CMSMMEMBLK_ReasonForBlock = b.CMSMMEMBLK_ReasonForBlock,
                                 CMSMMEMBLK_RenewalDate=b.CMSMMEMBLK_RenewalDate,
                                 CMSMMEMBLK_ActiveFlg=b.CMSMMEMBLK_ActiveFlg,
                                 CMSMMEMBLK_CreatedDate=b.CMSMMEMBLK_CreatedDate
                             }
                             ).Distinct().OrderByDescending(R=>R.CMSMMEMBLK_CreatedDate).ToArray();
            //   dto.getname = _CmsContext.CMS_MasterMemberDMO.Where(R => R.MI_Id == id && R.CMSMMEM_ActiveFlag == true).Distinct().ToArray();
            dto.getname = (from a in _CmsContext.CMS_MasterMemberDMO
                           where (a.MI_Id == id && a.CMSMMEM_ActiveFlag == true)
                           select new CMS_Member_StatusDTO
                           {
                               //CMSMMEM_MembershipNo
                               CMSMMEM_MemberFirstName = a.CMSMMEM_MemberFirstName + (string.IsNullOrEmpty(a.CMSMMEM_MemberMiddleName) ? "" : ' ' + a.CMSMMEM_MemberLastName) + (string.IsNullOrEmpty(a.CMSMMEM_MemberLastName) ? "" : ' ' + a.CMSMMEM_MemberLastName) + (string.IsNullOrEmpty(a.CMSMMEM_MembershipNo) ? "" : '/' + a.CMSMMEM_MembershipNo).Trim(),
                               CMSMMEM_Id = a.CMSMMEM_Id
                           }
                               ).Distinct().ToArray();

            return dto;

        }
        public CMS_Master_MemberBlockedDTO deactive(CMS_Master_MemberBlockedDTO data)
        {
            try
            {
                if (data.CMSMMEMBLK_Id > 0)
                {
                    var update = _CmsContext.CMS_Master_MemberBlockedDMO.Where(R => R.CMSMMEMBLK_Id == data.CMSMMEMBLK_Id).FirstOrDefault();
                    if (update.CMSMMEMBLK_ActiveFlg == true)
                    {
                        update.CMSMMEMBLK_ActiveFlg = false;
                    }
                    else
                    {
                        update.CMSMMEMBLK_ActiveFlg = true;
                    }
                    update.CMSMMEMBLK_UpdatedBy = data.UserId;
                    update.CMSMMEMBLK_UpdatedDate = DateTime.Now;
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
        public CMS_Master_MemberBlockedDTO savedetail1(CMS_Master_MemberBlockedDTO data)
        {
            try
            {
                if(data.CMSMMEMBLK_Id > 0)
                {
                    var duplicate = _CmsContext.CMS_Master_MemberBlockedDMO.Where(R => R.CMSMMEM_Id == data.CMSMMEM_Id && R.CMSMMEMBLK_BlockedDate == data.CMSMMEMBLK_BlockedDate && R.CMSMMEMBLK_Id!=data.CMSMMEMBLK_Id).ToList();
                    if (duplicate.Count > 0)
                    {
                        data.returnval = "exit";
                    }
                    else
                    {
                        var update = _CmsContext.CMS_Master_MemberBlockedDMO.Where(R => R.CMSMMEMBLK_Id == data.CMSMMEMBLK_Id).FirstOrDefault();
                        //   update.CMSMMEM_Id = data.CMSMMEM_Id;
                        update.CMSMMEMBLK_BlockedDate = data.CMSMMEMBLK_BlockedDate;
                        update.CMSMMEMBLK_ReasonForBlock = data.CMSMMEMBLK_ReasonForBlock;
                        update.CMSMMEMBLK_RenewalDate = data.CMSMMEMBLK_RenewalDate;
                        update.CMSMMEMBLK_UpdatedDate = DateTime.Now;
                        update.CMSMMEMBLK_UpdatedBy = data.UserId;
                        _CmsContext.Update(update);
                        int i = _CmsContext.SaveChanges();
                        if (i > 0)
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
                    var duplicate = _CmsContext.CMS_Master_MemberBlockedDMO.Where(R => R.CMSMMEM_Id == data.CMSMMEM_Id && R.CMSMMEMBLK_BlockedDate == data.CMSMMEMBLK_BlockedDate).ToList();
                    if(duplicate.Count > 0)
                    {
                        data.returnval = "exit";
                    }
                    else
                    {
                        CMS_Master_MemberBlockedDMO obj = new CMS_Master_MemberBlockedDMO();
                        obj.CMSMMEM_Id = data.CMSMMEM_Id;
                        obj.CMSMMEMBLK_BlockedDate = data.CMSMMEMBLK_BlockedDate;
                        obj.CMSMMEMBLK_ReasonForBlock = data.CMSMMEMBLK_ReasonForBlock;
                        obj.CMSMMEMBLK_RenewalDate = data.CMSMMEMBLK_RenewalDate;
                        obj.CMSMMEMBLK_ActiveFlg = true;
                        obj.CMSMMEMBLK_CreatedDate = DateTime.Now;
                        obj.CMSMMEMBLK_CreatedBy = data.UserId;
                        obj.CMSMMEMBLK_UpdatedDate = DateTime.Now;
                        obj.CMSMMEMBLK_UpdatedBy = data.UserId;
                        _CmsContext.Add(obj);
                        int i = _CmsContext.SaveChanges();
                        if(i > 0)
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
    }
}
