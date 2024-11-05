using DataAccessMsSqlServerProvider.com.vapstech.ClubManagement;
using DomainModel.Model.com.vapstech.ClubManagement;
using PreadmissionDTOs.com.vaps.ClubManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClubManagement.Services
{
    public class CMS_MasterDepartmentIMPL : Interfaces.CMS_MasterDepartmentInerface
    {
        public ClubManagementContext _CmsContext;
        public CMS_MasterDepartmentIMPL(ClubManagementContext cmsContext)
        {
            _CmsContext = cmsContext;
        }
        public CMS_MasterDepartmentDTO loaddata(int id)
        {
           
            CMS_MasterDepartmentDTO dto = new CMS_MasterDepartmentDTO();
            try
            {
                dto.pages = _CmsContext.CMS_MasterDepartmenDMO.Where(R => R.MI_Id == id).Distinct().ToArray();
            }
            
             catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                dto.returnval = "admin";
            }
            return dto;
           
        }
        public CMS_MasterDepartmentDTO savedata(CMS_MasterDepartmentDTO data)
        {
            try
            {
                if (data.CMSMDEPT_Id > 0)
                {
                    var result = _CmsContext.CMS_MasterDepartmenDMO.Where(R => R.CMSMDEPT_Id != data.CMSMDEPT_Id && R.CMSMDEPT_DepartmentName==data.CMSMDEPT_DepartmentName && R.CMSMDEPT_DeptCode==data.CMSMDEPT_DeptCode && R.MI_Id==data.MI_Id).ToList();
                    if(result.Count > 0)
                    {
                        data.returnval = "exit";
                    }
                    else
                    {
                        var resultwo = _CmsContext.CMS_MasterDepartmenDMO.Where(R => R.CMSMDEPT_Id == data.CMSMDEPT_Id).FirstOrDefault();
                        if(resultwo.CMSMDEPT_Id > 0)
                        {
                            resultwo.CMSMDEPT_DepartmentName = data.CMSMDEPT_DepartmentName;
                            resultwo.CMSMDEPT_DeptCode = data.CMSMDEPT_DeptCode;
                            resultwo.CMSMDEPT_UpdatedBy = data.UserId;
                            resultwo.CMSMDEPT_UpdatedDate = DateTime.Now;
                            _CmsContext.Update(resultwo);
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
                    }
                }
                else
                {
                    var result = _CmsContext.CMS_MasterDepartmenDMO.Where(R => R.CMSMDEPT_DepartmentName == data.CMSMDEPT_DepartmentName && R.CMSMDEPT_DeptCode == data.CMSMDEPT_DeptCode && R.MI_Id == data.MI_Id).ToList();
                    if (result.Count > 0)
                    {
                        data.returnval = "exit";
                    }
                    else
                    {
                        CMS_MasterDepartmenDMO obj = new CMS_MasterDepartmenDMO();
                        obj.MI_Id = data.MI_Id;
                        obj.CMSMDEPT_DepartmentName = data.CMSMDEPT_DepartmentName;
                        obj.CMSMDEPT_DeptCode = data.CMSMDEPT_DeptCode;
                        obj.CMSMDEPT_ActiveFlag = true;
                        obj.CMSMDEPT_CreatedDate = DateTime.Now;
                        obj.CMSMDEPT_CreatedBy = data.UserId;
                        obj.CMSMDEPT_UpdatedBy = data.UserId;
                        obj.CMSMDEPT_UpdatedDate = DateTime.Now;
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
        //deactive
        public CMS_MasterDepartmentDTO deactive(CMS_MasterDepartmentDTO data)
        {
            try
            {
                var resultwo = _CmsContext.CMS_MasterDepartmenDMO.Where(R => R.CMSMDEPT_Id == data.CMSMDEPT_Id &&  R.MI_Id == data.MI_Id).FirstOrDefault();
                if(resultwo.CMSMDEPT_Id > 0)
                {
                    if (resultwo.CMSMDEPT_ActiveFlag == true)
                    {
                        resultwo.CMSMDEPT_ActiveFlag = false;
                    }
                    else
                    {
                        resultwo.CMSMDEPT_ActiveFlag = true;
                    }
                    resultwo.CMSMDEPT_UpdatedBy = data.UserId;
                    resultwo.CMSMDEPT_UpdatedDate = DateTime.Now;
                    _CmsContext.Update(resultwo);
                    var contactExists = _CmsContext.SaveChanges();
                    if (contactExists > 0)
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
        //configaration
        public CMS_ConfigurationDTO loaddataconfigure(int id)
        {
            CMS_ConfigurationDTO dto = new CMS_ConfigurationDTO();
            try
            {
                dto.getreport = _CmsContext.CMS_ConfigurationDMO.Where(R => R.MI_Id == id).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                dto.returnval = "admin";
            }
           
            //CMS_ConfigurationDMO
            return dto;
        }
        //saveconfigure
        public CMS_ConfigurationDTO saveconfigure(CMS_ConfigurationDTO data)
        {
            try
            {
                if(data.CMSCON_Id > 0)
                {
                    var con= _CmsContext.CMS_ConfigurationDMO.Where(R => R.MI_Id == data.MI_Id && R.CMSCON_Id==data.CMSCON_Id).Distinct().ToList();
                    if(con.Count > 0)
                    {
                        var update= _CmsContext.CMS_ConfigurationDMO.Where(R => R.MI_Id == data.MI_Id && R.CMSCON_Id == data.CMSCON_Id).Distinct().FirstOrDefault();
                        update.CMSCON_ApplicationApplFlg = data.CMSCON_ApplicationApplFlg;
                        update.CMSCON_DiscountApplFlg = data.CMSCON_DiscountApplFlg;
                        update.CMSCON_BOMFlg = data.CMSCON_BOMFlg;
                        update.CMSCON_CategorywiseFlg = data.CMSCON_CategorywiseFlg;
                        update.CMSCON_CreditFlg = data.CMSCON_CreditFlg;
                        update.CMSCON_IncentiveApplFlg = data.CMSCON_IncentiveApplFlg;
                        update.CMSCON_TaxApplFlg = data.CMSCON_TaxApplFlg;
                        update.CMSCON_PayLateFeeInterestFlg = data.CMSCON_PayLateFeeInterestFlg;
                        update.CMSCON_InterestPercent = data.CMSCON_InterestPercent;
                        update.CMSCON_MaxNoDependent = data.CMSCON_MaxNoDependent;
                        update.CMSCON_NoOfProposal = data.CMSCON_NoOfProposal;
                        update.CMSCON_AllowNonMemberCreditTransFlg = data.CMSCON_AllowNonMemberCreditTransFlg;
                        update.CMSCON_CoverChargeAmtFlg = data.CMSCON_CoverChargeAmtFlg;                                        
                        update.CMSCON_UpdatedDate = DateTime.Now;
                        update.CMSCON_UpdatedBy = data.UserId;
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
                        data.returnval = "admin";
                    }
                }
                else
                {
                    var duplicate = _CmsContext.CMS_ConfigurationDMO.Where(R => R.MI_Id == data.MI_Id).Distinct().ToList();
                    if (duplicate.Count > 0)
                    {
                        data.returnval = "configure";
                    }
                    else
                    {
                        CMS_ConfigurationDMO obj = new CMS_ConfigurationDMO();
                        obj.MI_Id = data.MI_Id;
                        obj.CMSCON_ApplicationApplFlg = data.CMSCON_ApplicationApplFlg;
                        obj.CMSCON_DiscountApplFlg = data.CMSCON_DiscountApplFlg;
                        obj.CMSCON_BOMFlg = data.CMSCON_BOMFlg;
                        obj.CMSCON_CategorywiseFlg = data.CMSCON_CategorywiseFlg;
                        obj.CMSCON_CreditFlg = data.CMSCON_CreditFlg;
                        obj.CMSCON_IncentiveApplFlg = data.CMSCON_IncentiveApplFlg;
                        obj.CMSCON_TaxApplFlg = data.CMSCON_TaxApplFlg;
                        obj.CMSCON_PayLateFeeInterestFlg = data.CMSCON_PayLateFeeInterestFlg;
                        obj.CMSCON_InterestPercent = data.CMSCON_InterestPercent;
                        obj.CMSCON_MaxNoDependent = data.CMSCON_MaxNoDependent;
                        obj.CMSCON_NoOfProposal = data.CMSCON_NoOfProposal;
                        obj.CMSCON_AllowNonMemberCreditTransFlg = data.CMSCON_AllowNonMemberCreditTransFlg;
                        obj.CMSCON_CoverChargeAmtFlg = data.CMSCON_CoverChargeAmtFlg;
                        obj.CMSCON_ActiveFlag = true;
                        obj.CMSCON_CreatedDate = DateTime.Now;
                        obj.CMSCON_CreatedBy = data.UserId;
                        obj.CMSCON_UpdatedDate = DateTime.Now;
                        obj.CMSCON_UpdatedBy = data.UserId;
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
        //confdeactive
        public CMS_ConfigurationDTO confdeactive(CMS_ConfigurationDTO data)
        {
            try
            {
                if (data.CMSCON_Id > 0)
                {
                    var con = _CmsContext.CMS_ConfigurationDMO.Where(R => R.MI_Id == data.MI_Id && R.CMSCON_Id == data.CMSCON_Id).Distinct().ToList();
                    if (con.Count > 0)
                    {
                        var deactive = _CmsContext.CMS_ConfigurationDMO.Where(R => R.MI_Id == data.MI_Id && R.CMSCON_Id == data.CMSCON_Id).Distinct().FirstOrDefault();
           
                        if (deactive.CMSCON_ActiveFlag == true)
                        {
                            deactive.CMSCON_ActiveFlag = false;
                        }
                        else
                        {
                            deactive.CMSCON_ActiveFlag = true;
                        }
                        _CmsContext.Update(deactive);
                        var contactExists = _CmsContext.SaveChanges();
                        if (contactExists > 0)
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
