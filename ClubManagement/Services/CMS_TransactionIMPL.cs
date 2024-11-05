using DataAccessMsSqlServerProvider.com.vapstech.ClubManagement;
using DomainModel.Model.com.vapstech.ClubManagement;
using PreadmissionDTOs.com.vaps.ClubManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClubManagement.Services
{
    public class CMS_TransactionIMPL : Interfaces.CMS_TransactionInterface
    {
        public ClubManagementContext _CmsContext;
        public CMS_TransactionIMPL(ClubManagementContext cmsContext)
        {
            _CmsContext = cmsContext;
        }
        public CMS_TransactionDTO loaddata(int id)
       {

            CMS_TransactionDTO dto = new CMS_TransactionDTO();
            try
            {
                dto.getreport = (from a in _CmsContext.CMS_TransactionDMO
                                 from b in _CmsContext.CMS_MasterMemberDMO
                                 from c in _CmsContext.IVRM_Master_FinancialYear
                                 where (a.MI_Id == id && a.CMSMMEM_Id == b.CMSMMEM_Id && b.CMSMMEM_ActiveFlag == true && a.IMFY_Id == c.IMFY_Id)
                                 select new CMS_TransactionDTO
                                 {
                                     membername = b.CMSMMEM_MemberFirstName + (string.IsNullOrEmpty(b.CMSMMEM_MemberMiddleName) ? "" : ' ' + b.CMSMMEM_MemberMiddleName) + (string.IsNullOrEmpty(b.CMSMMEM_MemberLastName) ? "" : ' ' + b.CMSMMEM_MemberLastName),
                                     CMSMMEM_Id = a.CMSMMEM_Id,
                                     CMSTRANS_Id = a.CMSTRANS_Id,
                                     IMFY_Id = a.IMFY_Id,
                                     IMFY_FinancialYear = c.IMFY_FinancialYear,
                                     CMSTRANS_TransactionNo = a.CMSTRANS_TransactionNo,
                                     CMSTRANS_TotalNetAmount = a.CMSTRANS_TotalNetAmount,
                                     CMSTRANS_ActiveFlg = a.CMSTRANS_ActiveFlg,
                                     CMSTRANS_Date = a.CMSTRANS_Date,
                                     CMSTRANS_TotalAmount = a.CMSTRANS_TotalAmount,
                                     CMSTRANS_TotalTax = a.CMSTRANS_TotalTax,
                                     CMSTRANS_CreditTransFlg = a.CMSTRANS_CreditTransFlg,
                                     CMSTRANS_NoOFGuests = a.CMSTRANS_NoOFGuests,
                                     CMSTRANS_GuestsName = a.CMSTRANS_GuestsName,
                                     CMSTRANS_GuestContactNo = a.CMSTRANS_GuestContactNo,
                                     CMSTRANS_Remarks = a.CMSTRANS_Remarks,


                                 }
                         ).Distinct().ToArray();
               
                dto.finacial = _CmsContext.IVRM_Master_FinancialYear.Distinct().ToArray();

                dto.getname = (from a in _CmsContext.CMS_MasterMemberDMO
                               where (a.MI_Id == id && a.CMSMMEM_ActiveFlag == true)
                               select new CMS_Member_StatusDTO
                               {
                                   
                                   CMSMMEM_MemberFirstName = a.CMSMMEM_MemberFirstName + (string.IsNullOrEmpty(a.CMSMMEM_MemberMiddleName) ? "" : ' ' + a.CMSMMEM_MemberLastName) + (string.IsNullOrEmpty(a.CMSMMEM_MemberLastName) ? "" : ' ' + a.CMSMMEM_MemberLastName) + (string.IsNullOrEmpty(a.CMSMMEM_MembershipNo) ? "" : '/' + a.CMSMMEM_MembershipNo).Trim(),
                                   CMSMMEM_Id = a.CMSMMEM_Id
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
        public CMS_TransactionDTO savedata(CMS_TransactionDTO data)
        {
            try
            {
               if(data.CMSTRANS_Id > 0)
                {
                    var updateresult = _CmsContext.CMS_TransactionDMO.Where(R => R.CMSTRANS_Id == data.CMSTRANS_Id && R.MI_Id == data.MI_Id).FirstOrDefault();
                    if (updateresult.CMSTRANS_Id > 0)
                    {
                        updateresult.CMSMMEM_Id = data.CMSMMEM_Id;
                        updateresult.IMFY_Id = data.IMFY_Id;
                        updateresult.CMSTRANS_TransactionNo = data.CMSTRANS_TransactionNo;
                        updateresult.CMSTRANS_Date = data.CMSTRANS_Date;
                        updateresult.CMSTRANS_TotalAmount = data.CMSTRANS_TotalAmount;
                        updateresult.CMSTRANS_TotalTax = data.CMSTRANS_TotalTax;
                        updateresult.CMSTRANS_TotalNetAmount = data.CMSTRANS_TotalNetAmount;
                        updateresult.CMSTRANS_CreditTransFlg = false;
                        updateresult.CMSTRANS_NoOFGuests = data.CMSTRANS_NoOFGuests;
                        updateresult.CMSTRANS_GuestsName = data.CMSTRANS_GuestsName;
                        updateresult.CMSTRANS_GuestContactNo = data.CMSTRANS_GuestContactNo;
                        updateresult.CMSTRANS_Remarks = data.CMSTRANS_Remarks;
                        updateresult.CMSTRANS_UpdatedDate = DateTime.Now;
                        updateresult.CMSTRANS_UpdatedBy = data.UserId;
                        _CmsContext.Update(updateresult);
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
                        data.returnval = "admin";
                    }
                }
                else
                {
                    CMS_TransactionDMO obj = new CMS_TransactionDMO();
                    obj.CMSMMEM_Id = data.CMSMMEM_Id;
                    obj.MI_Id = data.MI_Id;
                    obj.IMFY_Id = data.IMFY_Id;
                    obj.CMSTRANS_TransactionNo = data.CMSTRANS_TransactionNo;
                    obj.CMSTRANS_Date = data.CMSTRANS_Date;
                    obj.CMSTRANS_TotalAmount = data.CMSTRANS_TotalAmount;
                    obj.CMSTRANS_TotalTax = data.CMSTRANS_TotalTax;
                    obj.CMSTRANS_TotalNetAmount = data.CMSTRANS_TotalNetAmount;
                    obj.CMSTRANS_CreditTransFlg = true;
                    obj.CMSTRANS_NoOFGuests = data.CMSTRANS_NoOFGuests;
                    obj.CMSTRANS_GuestsName = data.CMSTRANS_GuestsName;
                    obj.CMSTRANS_GuestContactNo = data.CMSTRANS_GuestContactNo;
                    obj.CMSTRANS_Remarks = data.CMSTRANS_Remarks;
                    obj.CMSTRANS_ActiveFlg = true;
                    obj.CMSTRANS_CreatedDate = DateTime.Now;
                    obj.CMSTRANS_CreatedBy = data.UserId;
                    obj.CMSTRANS_UpdatedDate = DateTime.Now;
                    obj.CMSTRANS_UpdatedBy = data.UserId;
                    _CmsContext.Add(obj);
                    if(data.CMSMMEM_Id !=0)
                    {
                        CMS_Transaction_MemberDMO dmo = new CMS_Transaction_MemberDMO();
                        dmo.CMSTRANS_Id = obj.CMSTRANS_Id;
                        dmo.CMSMMEM_Id = data.CMSMMEM_Id;
                        dmo.CMSTRANSMEM_ActiveFlg = true;
                        dmo.CMSTRANSMEM_CreatedDate = DateTime.Now;
                        dmo.CMSTRANSMEM_CreatedBy = data.UserId;
                        dmo.CMSTRANSMEM_UpdatedDate = DateTime.Now;
                        dmo.CMSTRANSMEM_UpdatedBy = data.UserId;
                        _CmsContext.Add(dmo);
                    }
                    if(data.TransctionNon_Member !=null)
                    {
                        for (int c = 0; c < data.TransctionNon_Member.Length; c++)
                        {
                            CMS_Transaction_NonMemberDMO nonobj = new CMS_Transaction_NonMemberDMO();
                            nonobj.CMSTRANS_Id = obj.CMSTRANS_Id;
                            nonobj.CMSMMEM_Id = data.CMSMMEM_Id;
                            nonobj.CMSTRANSNMEM_NonMemberName = data.TransctionNon_Member[c].CMSTRANSNMEM_NonMemberName;
                            nonobj.CMSTRANSNMEM_ContactNo = data.TransctionNon_Member[c].CMSTRANSNMEM_ContactNo;
                            nonobj.CMSTRANSNMEM_EmailId = data.TransctionNon_Member[c].CMSTRANSNMEM_EmailId;
                            nonobj.CMSTRANSNMEM_Address = data.TransctionNon_Member[c].CMSTRANSNMEM_Address;
                            nonobj.CMSTRANNSMEM_ActiveFlg = true;
                            nonobj.CMSTRANSNMEM_CreatedDate =DateTime.Now;
                            nonobj.CMSTRANSNMEM_UpdatedDate = DateTime.Now;
                            nonobj.CMSTRANSNMEM_CreatedBy = data.UserId;
                            nonobj.CMSTRANSNMEM_UpdatedBy = data.UserId;
                            _CmsContext.Add(nonobj);
                        }
                           
                    }
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
        //edit
        public CMS_TransactionDTO edit(CMS_TransactionDTO data)
        {
            try
            {
                if (data.CMSTRANS_Id > 0)
                {
                   
                    var result = _CmsContext.CMS_TransactionDMO.Where(P => P.CMSTRANS_Id == data.CMSTRANS_Id && P.MI_Id == data.MI_Id).ToList();
                    if(result.Count > 0)
                    {
                        data.edittransction = result.Distinct().ToArray();
                        data.editnmember = _CmsContext.CMS_Transaction_NonMemberDMO.Where(R => R.CMSTRANS_Id == data.CMSTRANS_Id).Distinct().ToArray();

                    }
                    else
                    {
                        data.returnval = "admin";
                    }
                    // 

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

        //deactive
        public CMS_TransactionDTO deactive(CMS_TransactionDTO data)
        {
            try
            {
                if(data.CMSTRANS_Id > 0)
                {
                    var result = _CmsContext.CMS_TransactionDMO.Where(P => P.CMSTRANS_Id == data.CMSTRANS_Id && P.MI_Id == data.MI_Id).FirstOrDefault();
                    if (result.CMSTRANS_ActiveFlg == true)
                    {
                        result.CMSTRANS_ActiveFlg = false;
                    }
                    else
                    {
                        result.CMSTRANS_ActiveFlg = true;
                    }
                    result.CMSTRANS_UpdatedDate = DateTime.Now;
                    result.CMSTRANS_UpdatedBy = data.UserId;
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
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                data.returnval = "admin";
            }
            return data;
        }


        //Tranasction Details
        public CMS_TransactionDetailsDTO loaddatatwo(int id)
        {

            CMS_TransactionDetailsDTO dto = new CMS_TransactionDetailsDTO();
            try
            {
               
              // dto.getreport=(from a in _c)
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                dto.returnval = "admin";
            }
            return dto;



        }

        public CMS_TransactionDetailsDTO savedatatwo(CMS_TransactionDetailsDTO data)
        {
            try
            {
               if(data.CMSTRANSDET_Id > 0)
                {
                    var update= _CmsContext.CMS_Transaction_DetailsDMO.Where(R => R.CMSTRANSDET_Id == data.CMSTRANSDET_Id).FirstOrDefault();
                    update.CMSTRANS_Id = data.CMSTRANS_Id;
                    update.CMSTRANSMEMTYINT_Id = data.CMSTRANSMEMTYINT_Id;
                    update.CMSTRANSDET_Qty = data.CMSTRANSDET_Qty;
                    update.CMSTRANSDET_Amount = data.CMSTRANSDET_Amount;
                    update.CMSTRANSDET_Tax = data.CMSTRANSDET_Tax;
                    update.CMSTRANSDET_NetAmount = data.CMSTRANSDET_NetAmount;
                    update.CMSTRANSDET_ActiveFlg = true;                   
                    update.CMSTRANSDET_UpdatedDate = DateTime.Now;
                    update.CMSTRANSDET_UpdatedBy = data.UserId;
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
                    var duplicate = _CmsContext.CMS_Transaction_DetailsDMO.Where(R => R.CMSTRANS_Id == data.CMSTRANS_Id && R.CMSTRANSMEMTYINT_Id == data.CMSTRANSMEMTYINT_Id).ToList();
                    if(duplicate.Count > 0)
                    {
                      
                    }
                    else
                    {
                        CMS_Transaction_DetailsDMO obj = new CMS_Transaction_DetailsDMO();
                        obj.CMSTRANS_Id = data.CMSTRANS_Id;
                        obj.CMSTRANSMEMTYINT_Id = data.CMSTRANSMEMTYINT_Id;
                        obj.CMSTRANSDET_Qty = data.CMSTRANSDET_Qty;
                        obj.CMSTRANSDET_Amount = data.CMSTRANSDET_Amount;
                        obj.CMSTRANSDET_Tax = data.CMSTRANSDET_Tax;
                        obj.CMSTRANSDET_NetAmount = data.CMSTRANSDET_NetAmount;
                        obj.CMSTRANSDET_ActiveFlg = true;
                        obj.CMSTRANSDET_CreatedDate = DateTime.Now;
                        obj.CMSTRANSDET_CreatedBy = data.UserId;
                        obj.CMSTRANSDET_UpdatedDate = DateTime.Now;
                        obj.CMSTRANSDET_UpdatedBy = data.UserId;
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
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                data.returnval = "admin";
            }
            return data;
        }
        //edittwo
        public CMS_TransactionDetailsDTO edittwo(CMS_TransactionDetailsDTO data)
        {
            try
            {
                if(data.CMSTRANSDET_Id > 0)
                {
                    data.editarray = _CmsContext.CMS_Transaction_DetailsDMO.Where(R => R.CMSTRANSDET_Id == data.CMSTRANSDET_Id).Distinct().ToArray();
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                data.returnval = "admin";
            }
            return data;
        }
        public CMS_TransactionDetailsDTO deactivetwo(CMS_TransactionDetailsDTO data)
        {
            try
            {
                if (data.CMSTRANS_Id > 0)
                {
                    var result = _CmsContext.CMS_Transaction_DetailsDMO.Where(P => P.CMSTRANSDET_Id == data.CMSTRANSDET_Id).FirstOrDefault();
                    if (result.CMSTRANSDET_ActiveFlg == true)
                    {
                        result.CMSTRANSDET_ActiveFlg = false;
                    }
                    else
                    {
                        result.CMSTRANSDET_ActiveFlg = true;
                    }
                    result.CMSTRANSDET_UpdatedDate = DateTime.Now;
                    result.CMSTRANSDET_UpdatedBy = data.UserId;
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
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                data.returnval = "admin";
            }
            return data;
        }
    }
}
