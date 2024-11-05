using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.VMS.Sales;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.VMS.Sales;
using Recruitment.com.vaps.Interfaces;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CommonLibrary;

namespace Recruitment.com.vaps.Services
{
    public class Sales_Lead_Master_IMPL : Interfaces.Sales_Lead_Master_Interface
    {
        public VMSContext _vmsconte;
        public HRMSContext _hrmscon;
        private DomainModelMsSqlServerContext _db;
        public Sales_Lead_Master_IMPL(VMSContext vmsContext, HRMSContext hrms, DomainModelMsSqlServerContext db)
        {
            _vmsconte = vmsContext;
            _hrmscon = hrms;
            _db = db;
        }
        //===============================Category==============================================
        public ISM_Sales_Master_Category_DTO get_load_Cat(int id)
        {
            ISM_Sales_Master_Category_DTO CD = new ISM_Sales_Master_Category_DTO();
            try
            {
                CD.category_list = _vmsconte.ISM_Sales_Master_Category_DMO_con.Where(a => a.MI_Id == id).ToList().ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return CD;
        }
        public ISM_Sales_Master_Category_DTO Edit_details_Cat(ISM_Sales_Master_Category_DTO dto)
        {
            try
            {
                dto.edit_category_list = _vmsconte.ISM_Sales_Master_Category_DMO_con.Where(t => t.ISMSMCA_Id == dto.ISMSMCA_Id 
                && t.MI_Id == dto.MI_Id).ToList().ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }
        public ISM_Sales_Master_Category_DTO SaveEdit_Cat(ISM_Sales_Master_Category_DTO dto)
        {
            try
            {
                ISM_Sales_Master_Category_DMO CD = new ISM_Sales_Master_Category_DMO();

                if (dto.ISMSMCA_Id > 0)
                {
                    var result = _vmsconte.ISM_Sales_Master_Category_DMO_con.Single(t => t.ISMSMCA_Id == dto.ISMSMCA_Id);
                    result.ISMSMCA_CategoryName = dto.ISMSMCA_CategoryName;
                    result.ISMSMCA_Remarks = dto.ISMSMCA_Remarks;
                    result.MI_Id = dto.MI_Id;
                    result.UpdatedBy = dto.userId;
                    result.ISMSMCA_UpdatedDate = DateTime.Now;
                    _vmsconte.Update(result);
                    _vmsconte.SaveChanges();
                    dto.returnvalue = "Update";
                }
                else
                {
                    CD.ISMSMCA_CategoryName = dto.ISMSMCA_CategoryName;
                    CD.ISMSMCA_Remarks = dto.ISMSMCA_Remarks;
                    CD.MI_Id = dto.MI_Id;
                    CD.ISMSMCA_ActiveFlag = true;
                    CD.CreatedBy = dto.userId;
                    CD.ISMSMCA_CreatedDate = DateTime.Now;
                    _vmsconte.Add(CD);
                    _vmsconte.SaveChanges();
                    dto.returnvalue = "Add";
                }
            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }
        public ISM_Sales_Master_Category_DTO deactivate_Cat(ISM_Sales_Master_Category_DTO dto)
        {
            try
            {
                var result = _vmsconte.ISM_Sales_Master_Category_DMO_con.Single(t => t.ISMSMCA_Id == dto.ISMSMCA_Id);

                if (dto.ISMSMCA_ActiveFlag == true)
                {
                    result.ISMSMCA_ActiveFlag = false;
                    result.UpdatedBy = dto.userId;
                    result.ISMSMCA_UpdatedDate = DateTime.Now;
                    _vmsconte.Update(result);
                    var flag = _vmsconte.SaveChanges();
                    dto.retbool = false;
                }
                else
                {
                    result.ISMSMCA_ActiveFlag = true;
                    result.UpdatedBy = dto.userId;
                    result.ISMSMCA_UpdatedDate = DateTime.Now;
                    _vmsconte.Update(result);
                    var flag = _vmsconte.SaveChanges();
                    dto.retbool = true;
                }
            }

            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
            }
            return dto;
        }

        //===============================Product==============================================
        public ISM_Sales_Master_Product_DTO get_load_pro(int id)
        {
            ISM_Sales_Master_Product_DTO CD = new ISM_Sales_Master_Product_DTO();
            try
            {
                CD.product_list = _vmsconte.ISM_Sales_Master_Product_DMO_con.Where(a => a.MI_Id == id).ToList().ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return CD;
        }
        public ISM_Sales_Master_Product_DTO Edit_details_pro(ISM_Sales_Master_Product_DTO dto)
        {
            try
            {
                dto.edit_product_list = _vmsconte.ISM_Sales_Master_Product_DMO_con.Where(t => t.ISMSMPR_Id == dto.ISMSMPR_Id 
                && t.MI_Id == dto.MI_Id).ToList().ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }
        public ISM_Sales_Master_Product_DTO SaveEdit_pro(ISM_Sales_Master_Product_DTO dto)
        {
            try
            {
                ISM_Sales_Master_Product_DMO CD = new ISM_Sales_Master_Product_DMO();

                if (dto.ISMSMPR_Id > 0)
                {
                    var result = _vmsconte.ISM_Sales_Master_Product_DMO_con.Single(t => t.ISMSMPR_Id == dto.ISMSMPR_Id);
                    result.ISMSMPR_ProductName = dto.ISMSMPR_ProductName;
                    result.ISMSMPR_Remarks = dto.ISMSMPR_Remarks;
                    result.MI_Id = dto.MI_Id;
                    result.ISMSMPR_UpdatedBy = dto.userId;
                    result.UpdatedDate = DateTime.Now;
                    _vmsconte.Update(result);
                    _vmsconte.SaveChanges();
                    dto.returnvales = "Update";
                }
                else
                {
                    CD.ISMSMPR_ProductName = dto.ISMSMPR_ProductName;
                    CD.ISMSMPR_Remarks = dto.ISMSMPR_Remarks;
                    CD.MI_Id = dto.MI_Id;
                    CD.ISMSMPR_ActiveFlag = true;
                    CD.ISMSMPR_CreatedBy = dto.userId;
                    CD.CreatedDate = DateTime.Now;
                    _vmsconte.Add(CD);
                    _vmsconte.SaveChanges();
                    dto.returnvales = "Add";
                }
            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }
        public ISM_Sales_Master_Product_DTO deactivate_pro(ISM_Sales_Master_Product_DTO dto)
        {
            try
            {
                var result = _vmsconte.ISM_Sales_Master_Product_DMO_con.Single(t => t.ISMSMPR_Id == dto.ISMSMPR_Id);

                if (dto.ISMSMPR_ActiveFlag == true)
                {
                    result.ISMSMPR_ActiveFlag = false;
                    result.ISMSMPR_UpdatedBy = dto.userId;
                    result.UpdatedDate = DateTime.Now;
                    _vmsconte.Update(result);
                    var flag = _vmsconte.SaveChanges();
                    dto.retbool = false;
                }
                else
                {
                    result.ISMSMPR_ActiveFlag = true;
                    result.ISMSMPR_UpdatedBy = dto.userId;
                    result.UpdatedDate = DateTime.Now;
                    _vmsconte.Update(result);
                    var flag = _vmsconte.SaveChanges();
                    dto.retbool = true;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
            }
            return dto;
        }

        //======================Source==================
        public ISM_Sales_Master_Source_DTO get_load_src(int id)
        {
            ISM_Sales_Master_Source_DTO CD = new ISM_Sales_Master_Source_DTO();
            try
            {
                CD.source_list = _vmsconte.ISM_Sales_Master_Source_DMO_con.Where(a => a.MI_Id == id).ToList().ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return CD;
        }
        public ISM_Sales_Master_Source_DTO Edit_details_src(ISM_Sales_Master_Source_DTO dto)
        {
            try
            {
                dto.edit_source_list = _vmsconte.ISM_Sales_Master_Source_DMO_con.Where(t => t.ISMSMSO_Id == dto.ISMSMSO_Id && t.MI_Id == dto.MI_Id).ToList().ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }
        public ISM_Sales_Master_Source_DTO SaveEdit_src(ISM_Sales_Master_Source_DTO dto)
        {
            try
            {
                ISM_Sales_Master_Source_DMO CD = new ISM_Sales_Master_Source_DMO();

                if (dto.ISMSMSO_Id > 0)
                {
                    var result = _vmsconte.ISM_Sales_Master_Source_DMO_con.Single(t => t.ISMSMSO_Id == dto.ISMSMSO_Id);
                    result.ISMSMSO_SourceName = dto.ISMSMSO_SourceName;
                    result.ISMSMSO_Remarks = dto.ISMSMSO_Remarks;
                    result.MI_Id = dto.MI_Id;
                    result.ISMSMSO_UpdatedBy = dto.userId;
                    result.UpdatedDate = DateTime.Now;
                    _vmsconte.Update(result);
                    _vmsconte.SaveChanges();
                    dto.returnvales = "Update";
                }
                else
                {
                    CD.ISMSMSO_SourceName = dto.ISMSMSO_SourceName;
                    CD.ISMSMSO_Remarks = dto.ISMSMSO_Remarks;
                    CD.MI_Id = dto.MI_Id;
                    CD.ISMSMSO_ActiveFlag = true;
                    CD.ISMSMSO_CreatedBy = dto.userId;
                    CD.CreatedDate = DateTime.Now;
                    _vmsconte.Add(CD);
                    _vmsconte.SaveChanges();
                    dto.returnvales = "Add";
                }
            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }
        public ISM_Sales_Master_Source_DTO deactivate_src(ISM_Sales_Master_Source_DTO dto)
        {
            try
            {
                var result = _vmsconte.ISM_Sales_Master_Source_DMO_con.Single(t => t.ISMSMSO_Id == dto.ISMSMSO_Id);

                if (dto.ISMSMSO_ActiveFlag == true)
                {
                    result.ISMSMSO_ActiveFlag = false;
                    result.ISMSMSO_UpdatedBy = dto.userId;
                    result.UpdatedDate = DateTime.Now;
                    _vmsconte.Update(result);
                    var flag = _vmsconte.SaveChanges();
                    dto.retbool = false;
                }
                else
                {
                    result.ISMSMSO_ActiveFlag = true;
                    result.ISMSMSO_UpdatedBy = dto.userId;
                    result.UpdatedDate = DateTime.Now;
                    _vmsconte.Update(result);
                    var flag = _vmsconte.SaveChanges();
                    dto.retbool = true;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
            }
            return dto;
        }

        //======================Status==================
        public ISM_Sales_Master_Status_DTO get_load_sts(int id)
        {
            ISM_Sales_Master_Status_DTO CD = new ISM_Sales_Master_Status_DTO();
            try
            {
                CD.status_list = _vmsconte.ISM_Sales_Master_Status_DMO_con.Where(a => a.MI_Id == id).ToList().ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return CD;
        }
        public ISM_Sales_Master_Status_DTO Edit_details_sts(ISM_Sales_Master_Status_DTO dto)
        {
            try
            {
                dto.edit_status_list = _vmsconte.ISM_Sales_Master_Status_DMO_con.Where(t => t.ISMSMST_Id == dto.ISMSMST_Id && t.MI_Id == dto.MI_Id).ToList().ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }
        public ISM_Sales_Master_Status_DTO SaveEdit_sts(ISM_Sales_Master_Status_DTO dto)
        {
            try
            {
                ISM_Sales_Master_Status_DMO CD = new ISM_Sales_Master_Status_DMO();

                if (dto.ISMSMST_Id > 0)
                {
                    var result = _vmsconte.ISM_Sales_Master_Status_DMO_con.Single(t => t.ISMSMST_Id == dto.ISMSMST_Id);
                    result.ISMSMST_StatusName = dto.ISMSMST_StatusName;
                    result.ISMSMST_Remarks = dto.ISMSMST_Remarks;
                    result.MI_Id = dto.MI_Id;
                    result.ISMSMST_UpdatedBy = dto.userId;
                    result.UpdatedDate = DateTime.Now;
                    _vmsconte.Update(result);
                    _vmsconte.SaveChanges();
                    dto.returnvalue = "Update";
                }
                else
                {
                    CD.ISMSMST_StatusName = dto.ISMSMST_StatusName;
                    CD.ISMSMST_Remarks = dto.ISMSMST_Remarks;
                    CD.MI_Id = dto.MI_Id;
                    CD.ISMSMST_ActiveFlag = true;
                    CD.ISMSMST_CreatedBy = dto.userId;
                    CD.CreatedDate = DateTime.Now;
                    _vmsconte.Add(CD);
                    _vmsconte.SaveChanges();
                    dto.returnvalue = "Add";
                }
            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }
        public ISM_Sales_Master_Status_DTO deactivate_sts(ISM_Sales_Master_Status_DTO dto)
        {
            try
            {
                var result = _vmsconte.ISM_Sales_Master_Status_DMO_con.Single(t => t.ISMSMST_Id == dto.ISMSMST_Id);

                if (dto.ISMSMST_ActiveFlag == true)
                {
                    result.ISMSMST_ActiveFlag = false;
                    result.ISMSMST_UpdatedBy = dto.userId;
                    result.UpdatedDate = DateTime.Now;
                    _vmsconte.Update(result);
                    var flag = _vmsconte.SaveChanges();
                    dto.retbool = false;
                }
                else
                {
                    result.ISMSMST_ActiveFlag = true;
                    result.ISMSMST_UpdatedBy = dto.userId;
                    result.UpdatedDate = DateTime.Now;
                    _vmsconte.Update(result);
                    var flag = _vmsconte.SaveChanges();
                    dto.retbool = true;
                }
            }

            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
            }
            return dto;


        }
        //======================Lead==================
        public ISM_Sales_Lead_DTO load_all_lead(ISM_Sales_Lead_DTO dto)
        {
            try
            {
                dto.product_dd = _vmsconte.ISM_Sales_Master_Product_DMO_con.Where(a => a.MI_Id == dto.MI_Id && a.ISMSMPR_ActiveFlag == true).ToArray();

                dto.category_dd = _vmsconte.ISM_Sales_Master_Category_DMO_con.Where(a => a.MI_Id == dto.MI_Id && a.ISMSMCA_ActiveFlag == true).ToArray();

                dto.source_dd = _vmsconte.ISM_Sales_Master_Source_DMO_con.Where(a => a.MI_Id == dto.MI_Id && a.ISMSMSO_ActiveFlag == true).OrderByDescending(t => t.CreatedDate).ToArray();

                dto.status_dd = _vmsconte.ISM_Sales_Master_Status_DMO_con.Where(a => a.MI_Id == dto.MI_Id && a.ISMSMST_ActiveFlag == true).OrderByDescending(t => t.CreatedDate).ToArray();

                dto.country_dd = _hrmscon.IVRM_Master_Country.ToArray();

                dto.lead_list = (from b in _vmsconte.ISM_Sales_Master_Category_DMO_con
                                 from c in _vmsconte.ISM_Sales_Master_Source_DMO_con
                                 from d in _vmsconte.ISM_Sales_Lead_DMO_con
                                 from e in _vmsconte.ISM_Sales_Master_Status_DMO_con
                                 where d.ISMSMCA_Id == b.ISMSMCA_Id && d.ISMSMSO_Id == c.ISMSMSO_Id && d.MI_Id == dto.MI_Id && d.ISMSMST_Id == e.ISMSMST_Id
                                 select new ISM_Sales_Lead_DTO
                                 {
                                     ISMSLE_LeadName = d.ISMSLE_LeadName,
                                     ISMSLE_LeadCode = d.ISMSLE_LeadCode,
                                     ISMSMCA_CategoryName = b.ISMSMCA_CategoryName,
                                     ISMSMSO_SourceName = c.ISMSMSO_SourceName,
                                     ISMSMST_StatusName = e.ISMSMST_StatusName,
                                     ISMSLE_ActiveFlag = d.ISMSLE_ActiveFlag,
                                     ISMSLE_Remarks = d.ISMSLE_Remarks,
                                     ISMSLE_Id = d.ISMSLE_Id,
                                     ISMSLE_VisitedDate = d.ISMSLE_VisitedDate
                                 }).OrderByDescending(t => t.ISMSLE_VisitedDate).ToList().ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }
        public ISM_Sales_Lead_DTO select_state(ISM_Sales_Lead_DTO dto)
        {
            try
            {
                dto.state_dd = _hrmscon.IVRM_Master_State.Where(a => a.IVRMMC_Id == dto.IVRMMC_Id).ToArray();
            }
            catch (Exception eee)
            {
                Console.WriteLine(eee.Message);
            }
            return dto;
        }
        public ISM_Sales_Lead_DTO checkemailtemplet(ISM_Sales_Lead_DTO dto)
        {
            try
            {
                var result1 = _vmsconte.ISM_Sales_Master_Source_DMO.Where(a => a.ISMSMSO_Id == dto.ISMSMSO_Id).ToList();
                if (result1.Count > 0)
                {
                    var template = _db.smsEmailSetting.Where(e => e.MI_Id == dto.MI_Id && e.ISES_Template_Name == result1.FirstOrDefault().ISMSMSO_Templet 
                    && e.ISES_MailActiveFlag == true).ToList();

                    if (template.Count > 0)
                    {
                        dto.returnvalue = false;
                    }
                    else
                    {
                        dto.returnvalue = true;
                    }
                }
                else
                {
                    dto.returnvalue = true;
                }
            }
            catch (Exception eee)
            {
                Console.WriteLine(eee.Message);
            }
            return dto;
        }
        public ISM_Sales_Lead_DTO Save_Edit_SaleLead(ISM_Sales_Lead_DTO dto)
        {
            try
            {
                if (dto.ISMSLE_Id > 0)
                {
                    var result = _vmsconte.ISM_Sales_Lead_DMO_con.Single(a => a.ISMSLE_Id == dto.ISMSLE_Id);
                    var result2 = _vmsconte.ISM_Sales_Lead_Products_DMO_con.Where(a => a.ISMSLE_Id == dto.ISMSLE_Id).ToList();
                    foreach (var item in result2)
                    {
                        var result1 = _vmsconte.ISM_Sales_Lead_Products_DMO_con.Single(a => a.ISMSMPR_Id == item.ISMSMPR_Id && a.ISMSLE_Id == dto.ISMSLE_Id);
                        _vmsconte.Remove(result1);

                    }
                    result.ISMSLE_LeadName = dto.ISMSLE_LeadName;
                    result.ISMSLE_LeadCode = dto.ISMSLE_LeadCode;
                    result.ISMSLE_ContactPerson = dto.ISMSLE_ContactPerson;
                    result.ISMSLE_LeadAddress1 = dto.ISMSLE_LeadAddress1;
                    result.ISMSLE_LeadAddress2 = dto.ISMSLE_LeadAddress2;
                    result.ISMSLE_LeadAddress3 = dto.ISMSLE_LeadAddress3;
                    result.ISMSLE_VisitedDate = dto.ISMSLE_VisitedDate;
                    result.ISMSLE_Pincode = dto.ISMSLE_Pincode;
                    result.ISMSLE_ContactNo = dto.ISMSLE_ContactNo;
                    result.ISMSMST_Id = dto.ISMSMST_Id;
                    result.IVRMMC_Id = dto.IVRMMC_Id;
                    result.ISMSLE_EmailId = dto.ISMSLE_EmailId;
                    result.ISMSMCA_Id = dto.ISMSMCA_Id;
                    result.ISMSMSO_Id = dto.ISMSMSO_Id;
                    result.ISMSLE_Reference = dto.ISMSLE_Reference;
                    result.ISMSLE_ContactDesignation = dto.ISMSLE_ContactDesignation;
                    result.IVRMMS_Id = dto.IVRMMS_Id;
                    result.ISMSLE_StudentStrength = dto.ISMSLE_StudentStrength;
                    result.ISMSLE_StaffStrength = dto.ISMSLE_StaffStrength;
                    result.ISMSLE_NoOfInstitutions = dto.ISMSLE_NoOfInstitutions;
                    result.ISMSLE_Remarks = dto.ISMSLE_Remarks;
                    result.UpdatedDate = DateTime.Now;
                    result.ISMSLE_UpdatedBy = dto.user_id;
                    _vmsconte.Update(result);

                    foreach (var item in dto.product_list_save)
                    {
                        ISM_Sales_Lead_Products_DMO ld = new ISM_Sales_Lead_Products_DMO();
                        ld.ISMSMPR_Id = item.ISMSMPR_Id;
                        ld.ISMSLE_Id = result.ISMSLE_Id;
                        ld.MI_Id = dto.MI_Id;
                        ld.ISMSLEPR_ActiveFlag = true;
                        ld.UpdatedDate = DateTime.Now;
                        ld.ISMSLEPR_UpdatedBy = dto.user_id;
                        _vmsconte.Add(ld);
                    }
                    _vmsconte.SaveChanges();
                    dto.return_value = "Update";

                    var result3 = _vmsconte.ISM_Sales_Master_Source_DMO.Where(a => a.ISMSMSO_Id == dto.ISMSMSO_Id).ToList();
                    if (result3.Count > 0)
                    {
                        var template = _db.smsEmailSetting.Where(e => e.MI_Id == dto.MI_Id && e.ISES_Template_Name == result3.FirstOrDefault().ISMSMSO_Templet && e.ISES_MailActiveFlag == true).ToList();
                        if (template.Count > 0)
                        {
                            if (dto.sendemail == true)
                            {
                                if (dto.ISMSLE_EmailId != null && dto.ISMSLE_EmailId != "")
                                {
                                    sendmail(dto.MI_Id, dto.ISMSLE_EmailId, result3.FirstOrDefault().ISMSMSO_Templet, dto.ISMSLE_Id);
                                }
                            }
                        }
                    }
                    //sendmail(dto.MI_Id, dto.ISMSLE_EmailId, "LEAD_CREATE", dto.ISMSLE_Id);
                }
                else
                {
                    ISM_Sales_Lead_DMO sl = new ISM_Sales_Lead_DMO();
                    sl.ISMSLE_LeadName = dto.ISMSLE_LeadName;
                    sl.ISMSLE_LeadCode = dto.ISMSLE_LeadCode;
                    sl.ISMSLE_ContactPerson = dto.ISMSLE_ContactPerson;
                    sl.ISMSLE_LeadAddress1 = dto.ISMSLE_LeadAddress1;
                    sl.ISMSLE_LeadAddress2 = dto.ISMSLE_LeadAddress2;
                    sl.ISMSLE_LeadAddress3 = dto.ISMSLE_LeadAddress3;
                    sl.ISMSLE_VisitedDate = dto.ISMSLE_VisitedDate;
                    sl.ISMSLE_Pincode = dto.ISMSLE_Pincode;
                    sl.ISMSLE_ContactNo = dto.ISMSLE_ContactNo;
                    sl.ISMSMST_Id = dto.ISMSMST_Id;
                    sl.IVRMMC_Id = dto.IVRMMC_Id;
                    sl.ISMSLE_EmailId = dto.ISMSLE_EmailId;
                    sl.ISMSMCA_Id = dto.ISMSMCA_Id;
                    sl.ISMSMSO_Id = dto.ISMSMSO_Id;
                    sl.ISMSLE_Reference = dto.ISMSLE_Reference;
                    sl.ISMSLE_ContactDesignation = dto.ISMSLE_ContactDesignation;
                    sl.IVRMMS_Id = dto.IVRMMS_Id;
                    sl.ISMSLE_StudentStrength = dto.ISMSLE_StudentStrength;
                    sl.ISMSLE_StaffStrength = dto.ISMSLE_StaffStrength;
                    sl.ISMSLE_NoOfInstitutions = dto.ISMSLE_NoOfInstitutions;
                    sl.ISMSLE_Remarks = dto.ISMSLE_Remarks;
                    sl.MI_Id = dto.MI_Id;
                    sl.ISMSLE_ActiveFlag = true;
                    sl.CreatedDate = DateTime.Now;
                    sl.ISMSLE_CreatedBy = dto.user_id;
                    _vmsconte.Add(sl);

                    foreach (var item in dto.product_list_save)
                    {
                        ISM_Sales_Lead_Products_DMO ld = new ISM_Sales_Lead_Products_DMO();
                        ld.ISMSMPR_Id = item.ISMSMPR_Id;
                        ld.ISMSLE_Id = sl.ISMSLE_Id;
                        ld.MI_Id = dto.MI_Id;
                        ld.ISMSLEPR_ActiveFlag = true;
                        ld.CreatedDate = DateTime.Now;
                        ld.ISMSLEPR_CreatedBy = dto.user_id;
                        ld.UpdatedDate = DateTime.Now;
                        ld.ISMSLEPR_UpdatedBy = dto.user_id;
                        _vmsconte.Add(ld);
                    }

                    var check = _vmsconte.SaveChanges();
                    if (check > 0)
                    {
                        dto.return_value = "Add";
                        var result1 = _vmsconte.ISM_Sales_Master_Source_DMO.Where(a => a.ISMSMSO_Id == dto.ISMSMSO_Id).ToList();
                        if (result1.Count > 0)
                        {
                            var template = _db.smsEmailSetting.Where(e => e.MI_Id == dto.MI_Id && e.ISES_Template_Name == result1.FirstOrDefault().ISMSMSO_Templet && e.ISES_MailActiveFlag == true).ToList();
                            if (template.Count > 0)
                            {
                                if (dto.sendemail == true)
                                {
                                    if (dto.ISMSLE_EmailId != null && dto.ISMSLE_EmailId != "")
                                    {
                                        sendmail(dto.MI_Id, dto.ISMSLE_EmailId, result1.FirstOrDefault().ISMSMSO_Templet, sl.ISMSLE_Id);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }
        public ISM_Sales_Lead_DTO Sales_lead_edit(ISM_Sales_Lead_DTO dTO)
        {
            try
            {
                dTO.sales_lead_edit = (from a in _vmsconte.ISM_Sales_Lead_DMO_con
                                       from b in _vmsconte.ISM_Sales_Master_Category_DMO_con
                                       from c in _vmsconte.IVRM_Master_State
                                       from d in _vmsconte.ISM_Sales_Master_Source_DMO_con
                                       from e in _vmsconte.ISM_Sales_Master_Status_DMO_con
                                       from f in _vmsconte.IVRM_Master_Country
                                       where a.ISMSMCA_Id == b.ISMSMCA_Id && a.IVRMMS_Id == c.IVRMMS_Id && a.ISMSMSO_Id == d.ISMSMSO_Id && a.MI_Id == dTO.MI_Id && a.ISMSMST_Id == e.ISMSMST_Id && a.IVRMMC_Id == f.IVRMMC_Id && a.ISMSLE_Id == dTO.ISMSLE_Id
                                       select new ISM_Sales_Lead_DTO
                                       {
                                           ISMSLE_Id = a.ISMSLE_Id,
                                           ISMSLE_LeadName = a.ISMSLE_LeadName,
                                           ISMSLE_LeadCode = a.ISMSLE_LeadCode,
                                           ISMSLE_ContactPerson = a.ISMSLE_ContactPerson,
                                           ISMSLE_LeadAddress1 = a.ISMSLE_LeadAddress1,
                                           ISMSLE_LeadAddress2 = a.ISMSLE_LeadAddress2,
                                           ISMSLE_LeadAddress3 = a.ISMSLE_LeadAddress3,
                                           ISMSLE_VisitedDate = Convert.ToDateTime(a.ISMSLE_VisitedDate),
                                           ISMSLE_Pincode = Convert.ToInt64(a.ISMSLE_Pincode),
                                           ISMSLE_ContactNo = a.ISMSLE_ContactNo,
                                           ISMSMST_Id = a.ISMSMST_Id,
                                           IVRMMS_Name = c.IVRMMS_Name,
                                           ISMSLE_EmailId = a.ISMSLE_EmailId,
                                           ISMSMCA_Id = a.ISMSMCA_Id,
                                           ISMSMCA_CategoryName = b.ISMSMCA_CategoryName,
                                           ISMSMSO_Id = a.ISMSMSO_Id,
                                           ISMSMSO_SourceName = d.ISMSMSO_SourceName,
                                           ISMSLE_Reference = a.ISMSLE_Reference,
                                           ISMSLE_ContactDesignation = a.ISMSLE_ContactDesignation,
                                           IVRMMS_Id = a.IVRMMS_Id,
                                           ISMSLE_StudentStrength = a.ISMSLE_StudentStrength,
                                           ISMSLE_StaffStrength = a.ISMSLE_StaffStrength,
                                           ISMSLE_NoOfInstitutions = a.ISMSLE_NoOfInstitutions,
                                           ISMSLE_Remarks = a.ISMSLE_Remarks,
                                           ISMSMST_StatusName = e.ISMSMST_StatusName,
                                           IVRMMC_Id = a.IVRMMC_Id,
                                           IVRMMC_CountryName = f.IVRMMC_CountryName
                                       }).ToList().ToArray();

                dTO.sales_lead_edit_product_dd_product = (from a in _vmsconte.ISM_Sales_Lead_Products_DMO_con
                                                          from b in _vmsconte.ISM_Sales_Master_Product_DMO_con
                                                          where a.ISMSMPR_Id == b.ISMSMPR_Id && a.MI_Id == dTO.MI_Id && a.ISMSLE_Id == dTO.ISMSLE_Id
                                                          select new ISM_Sales_Lead_DTO
                                                          {
                                                              ISMSMPR_ProductName = b.ISMSMPR_ProductName,
                                                              ISMSMPR_Id = a.ISMSMPR_Id,
                                                              ISMSLEPR_ActiveFlag = a.ISMSLEPR_ActiveFlag,
                                                              ISMSLE_Id = a.ISMSLE_Id

                                                          }).ToArray();

                dTO.sales_lead_edit_product_dd = (from a in _vmsconte.ISM_Sales_Lead_Products_DMO_con
                                                  from b in _vmsconte.ISM_Sales_Master_Product_DMO_con
                                                  where a.ISMSMPR_Id == b.ISMSMPR_Id && a.MI_Id == dTO.MI_Id && a.ISMSLE_Id == dTO.ISMSLE_Id
                                                  && a.ISMSLEPR_ActiveFlag == true
                                                  select new ISM_Sales_Lead_DTO
                                                  {
                                                      ISMSMPR_ProductName = b.ISMSMPR_ProductName,
                                                      ISMSMPR_Id = a.ISMSMPR_Id,
                                                  }).ToArray();

                dTO.product_dd = _vmsconte.ISM_Sales_Master_Product_DMO_con.Where(a => a.MI_Id == dTO.MI_Id && a.ISMSMPR_ActiveFlag == true).ToArray();
                dTO.category_dd = _vmsconte.ISM_Sales_Master_Category_DMO_con.Where(a => a.MI_Id == dTO.MI_Id && a.ISMSMCA_ActiveFlag == true).ToArray();
                dTO.source_dd = _vmsconte.ISM_Sales_Master_Source_DMO_con.Where(a => a.MI_Id == dTO.MI_Id && a.ISMSMSO_ActiveFlag == true).ToArray();
                dTO.status_dd = _vmsconte.ISM_Sales_Master_Status_DMO_con.Where(a => a.MI_Id == dTO.MI_Id && a.ISMSMST_ActiveFlag == true).ToArray();
                var LeadCountry = _vmsconte.ISM_Sales_Lead_DMO_con.Single(t => t.ISMSLE_Id == dTO.ISMSLE_Id).IVRMMC_Id;
                dTO.state_dd = _hrmscon.IVRM_Master_State.Where(t => t.IVRMMC_Id == LeadCountry).ToArray();
                dTO.country_dd = _hrmscon.IVRM_Master_Country.ToArray();
                dTO.IVRMMC_Id = LeadCountry;
                dTO.IVRMMS_Id = _vmsconte.ISM_Sales_Lead_DMO_con.Single(t => t.ISMSLE_Id == dTO.ISMSLE_Id).IVRMMS_Id;

                var sourceid = _vmsconte.ISM_Sales_Lead_DMO_con.Single(t => t.ISMSLE_Id == dTO.ISMSLE_Id).ISMSMSO_Id;
                var result1 = _vmsconte.ISM_Sales_Master_Source_DMO.Where(a => a.ISMSMSO_Id == sourceid).ToList();
                if (result1.Count > 0)
                {
                    var template = _db.smsEmailSetting.Where(e => e.MI_Id == dTO.MI_Id && e.ISES_Template_Name == result1.FirstOrDefault().ISMSMSO_Templet && e.ISES_MailActiveFlag == true).ToList();
                    if (template.Count > 0)
                    {
                        dTO.returnvalue = false;
                    }
                    else
                    {
                        dTO.returnvalue = true;
                    }
                }
                else
                {
                    dTO.returnvalue = true;
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dTO;
        }
        public ISM_Sales_Lead_DTO deactiv_prde(ISM_Sales_Lead_DTO dto)
        {
            try
            {

                var result = _vmsconte.ISM_Sales_Lead_Products_DMO_con.Single(t => t.ISMSMPR_Id == dto.ISMSMPR_Id && t.ISMSLE_Id == dto.ISMSLE_Id);

                if (dto.ISMSLEPR_ActiveFlag == true)
                {
                    result.ISMSLEPR_ActiveFlag = false;
                    result.ISMSLEPR_UpdatedBy = dto.user_id;
                    result.UpdatedDate = DateTime.Now;
                    _vmsconte.Update(result);
                    var flag = _vmsconte.SaveChanges();
                    dto.retbool = false;
                }
                else
                {

                    result.ISMSLEPR_ActiveFlag = true;
                    result.ISMSLEPR_UpdatedBy = dto.user_id;
                    result.UpdatedDate = DateTime.Now;
                    _vmsconte.Update(result);
                    var flag = _vmsconte.SaveChanges();
                    dto.retbool = true;
                }


            }


            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
            }
            return dto;


        }

        //=====================Lead Demo===========================
        public ISM_Sales_Lead_Demo_DTO get_load_lead_demo(ISM_Sales_Lead_Demo_DTO dto)
        {
            try
            {
                dto.lead_list_dd = _vmsconte.ISM_Sales_Lead_DMO_con.Where(a => a.ISMSLE_ActiveFlag == true
                && a.MI_Id == dto.MI_Id).OrderBy(a => a.ISMSLE_LeadName).ToArray();

                dto.hrme_list_dd = (from a in _vmsconte.Hr_Master_Employee_con
                                    from b in _vmsconte.Institution

                                    where (a.HRME_ActiveFlag == true && a.HRME_LeftFlag == false && a.MI_Id == b.MI_Id && b.MI_ActiveFlag == 1)
                                    select new ISM_Sales_Lead_Demo_DTO
                                    {
                                        employeename = ((a.HRME_EmployeeFirstName == null || a.HRME_EmployeeFirstName == "" ? "" : a.HRME_EmployeeFirstName)
                                        + (a.HRME_EmployeeMiddleName == null || a.HRME_EmployeeMiddleName == "" ? "" : " " + a.HRME_EmployeeMiddleName)
                                        + (a.HRME_EmployeeLastName == null || a.HRME_EmployeeLastName == "" ? "" : " " + a.HRME_EmployeeLastName)).Trim(),
                                        HRME_Id = a.HRME_Id
                                    }).Distinct().OrderBy(a => a.employeename).ToList().ToArray();

                dto.product_list_dd = _vmsconte.ISM_Sales_Master_Product_DMO_con.Where(a => a.ISMSMPR_ActiveFlag == true && a.MI_Id == dto.MI_Id).OrderBy(a => a.ISMSMPR_ProductName).ToArray();

                dto.demo_list = (from a in _vmsconte.ISM_Sales_Lead_DMO_con
                                 from b in _vmsconte.ISM_Sales_Master_Status_DMO_con
                                 from c in _vmsconte.ISM_Sales_Lead_Demo_DMO_con
                                 where a.ISMSMST_Id == b.ISMSMST_Id && a.ISMSLE_Id == c.ISMSLE_Id && a.MI_Id == dto.MI_Id

                                 select new ISM_Sales_Lead_Demo_DTO
                                 {
                                     ISMSLEDM_Id = c.ISMSLEDM_Id,
                                     ISMSLE_LeadName = a.ISMSLE_LeadName,
                                     ISMSLE_LeadCode = a.ISMSLE_LeadCode,
                                     ISMSLE_Id = a.ISMSLE_Id,
                                     ISMSLEDM_DemoType = c.ISMSLEDM_DemoType,
                                     ISMSMST_StatusName = b.ISMSMST_StatusName,
                                     ISMSMST_Id = b.ISMSMST_Id,
                                     ISMSLEDM_ContactPerson = c.ISMSLEDM_ContactPerson,
                                     ISMSLEDM_DemoDate = c.ISMSLEDM_DemoDate,
                                     ISMSLEDM_ActiveFlag = c.ISMSLEDM_ActiveFlag,
                                     ISMSLEDM_Status_Flg = c.ISMSLEDM_Status_Flg
                                 }).ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }
        public ISM_Sales_Lead_Demo_DTO Edit_details_lead_demo(ISM_Sales_Lead_Demo_DTO dto)
        {
            try
            {
                dto.Edit_details_lead_demo = (from a in _vmsconte.ISM_Sales_Lead_DMO_con
                                              from b in _vmsconte.ISM_Sales_Master_Status_DMO_con
                                              from c in _vmsconte.ISM_Sales_Lead_Demo_DMO_con
                                              from d in _vmsconte.Hr_Master_Employee_con
                                              where a.ISMSMST_Id == b.ISMSMST_Id && a.ISMSLE_Id == c.ISMSLE_Id && a.MI_Id == dto.MI_Id
                                              && c.ISMSLEDM_Id == dto.ISMSLEDM_Id && a.ISMSLE_Id == dto.ISMSLE_Id && c.HRME_Id == d.HRME_Id

                                              select new ISM_Sales_Lead_Demo_DTO
                                              {
                                                  employeename = ((d.HRME_EmployeeFirstName == null || d.HRME_EmployeeFirstName == "" ? "" : d.HRME_EmployeeFirstName)
                                                  + (d.HRME_EmployeeMiddleName == null || d.HRME_EmployeeMiddleName == "" ? "" : " " + d.HRME_EmployeeMiddleName)
                                                  + (d.HRME_EmployeeLastName == null || d.HRME_EmployeeLastName == "" ? "" : " " + d.HRME_EmployeeLastName)).Trim(),
                                                  HRME_Id = d.HRME_Id,
                                                  ISMSLEDM_Id = c.ISMSLEDM_Id,
                                                  ISMSLE_LeadName = a.ISMSLE_LeadName,
                                                  ISMSLE_LeadCode = a.ISMSLE_LeadCode,
                                                  ISMSLE_Id = a.ISMSLE_Id,
                                                  ISMSLEDM_DemoType = c.ISMSLEDM_DemoType,
                                                  ISMSMST_StatusName = b.ISMSMST_StatusName,
                                                  ISMSMST_Id = b.ISMSMST_Id,
                                                  ISMSLEDM_DemoAddress = c.ISMSLEDM_DemoAddress,
                                                  ISMSLEDM_ContactPerson = c.ISMSLEDM_ContactPerson,
                                                  ISMSLEDM_DemoDate = Convert.ToDateTime(c.ISMSLEDM_DemoDate),
                                                  ISMSLEDM_ActiveFlag = c.ISMSLEDM_ActiveFlag,
                                                  ISMSLEDM_Remarks = c.ISMSLEDM_Remarks
                                              }).ToArray();

                dto.Lead_Demo_Products_list = (from b in _vmsconte.ISM_Sales_Lead_Demo_Products_DMO_con
                                               from c in _vmsconte.ISM_Sales_Lead_Demo_DMO_con
                                               from d in _vmsconte.ISM_Sales_Master_Product_DMO_con
                                               where b.ISMSLEDM_Id == c.ISMSLEDM_Id && b.ISMSLEDM_Id == dto.ISMSLEDM_Id && c.ISMSLE_Id == dto.ISMSLE_Id
                                               && b.MI_Id == dto.MI_Id && b.ISMSMPR_Id == d.ISMSMPR_Id

                                               select new ISM_Sales_Lead_Demo_DTO
                                               {
                                                   ISMSMPR_ProductName = d.ISMSMPR_ProductName,
                                                   ISMSMPR_Id = b.ISMSMPR_Id,
                                                   ISMSLEDMPR_DiscussionPoints = b.ISMSLEDMPR_DiscussionPoints
                                               }).ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }
        public ISM_Sales_Lead_Demo_DTO SaveEdit_lead_demo(ISM_Sales_Lead_Demo_DTO dto)
        {
            try
            {
                if (dto.ISMSLEDM_Id > 0)
                {
                    var result1 = _vmsconte.ISM_Sales_Lead_Demo_Products_DMO_con.Where(a => a.ISMSLEDM_Id == dto.ISMSLEDM_Id).ToList();
                    foreach (var item in result1)
                    {
                        var res = _vmsconte.ISM_Sales_Lead_Demo_Products_DMO_con.Single(a => a.ISMSLEDMPR_Id == item.ISMSLEDMPR_Id);
                        _vmsconte.Remove(res);
                    }

                    var result = _vmsconte.ISM_Sales_Lead_Demo_DMO_con.Single(a => a.ISMSLEDM_Id == dto.ISMSLEDM_Id);

                    result.MI_Id = dto.MI_Id;
                    result.ISMSLE_Id = dto.ISMSLE_Id;
                    result.HRME_Id = dto.HRME_Id;
                    result.ISMSLEDM_DemoType = dto.ISMSLEDM_DemoType;
                    result.ISMSLEDM_DemoDate = dto.ISMSLEDM_DemoDate;
                    result.ISMSLEDM_ContactPerson = dto.ISMSLEDM_ContactPerson;
                    result.ISMSLEDM_DemoAddress = dto.ISMSLEDM_DemoAddress;
                    result.ISMSLEDM_Remarks = dto.ISMSLEDM_Remarks;
                    result.UpdatedDate = DateTime.Now;
                    result.ISMSLEDM_UpdatedBy = dto.user_id;
                    _vmsconte.Update(result);

                    foreach (var pdm in dto.product_demo_master_temp1)
                    {
                        ISM_Sales_Lead_Demo_Products_DMO dm = new ISM_Sales_Lead_Demo_Products_DMO();
                        dm.ISMSMPR_Id = pdm.ISMSMPR_Id;
                        dm.ISMSLEDMPR_DiscussionPoints = pdm.ISMSLEDMPR_DiscussionPoints;
                        dm.ISMSLEDM_Id = result.ISMSLEDM_Id;
                        dm.UpdatedDate = DateTime.Now;
                        dm.ISMSLEDMPR_UpdatedBy = dto.user_id;
                        dm.MI_Id = dto.MI_Id;
                        _vmsconte.Add(dm);
                    }
                    _vmsconte.SaveChanges();
                    dto.return_status = "Update";
                }
                else
                {
                    ISM_Sales_Lead_Demo_DMO res = new ISM_Sales_Lead_Demo_DMO();
                    res.MI_Id = dto.MI_Id;
                    res.ISMSLE_Id = dto.ISMSLE_Id;
                    res.HRME_Id = dto.HRME_Id;
                    res.ISMSLEDM_DemoType = dto.ISMSLEDM_DemoType;
                    res.ISMSLEDM_DemoDate = dto.ISMSLEDM_DemoDate;
                    res.ISMSLEDM_ContactPerson = dto.ISMSLEDM_ContactPerson;
                    res.ISMSLEDM_DemoAddress = dto.ISMSLEDM_DemoAddress;
                    res.ISMSLEDM_Remarks = dto.ISMSLEDM_Remarks;
                    res.CreatedDate = DateTime.Now;
                    res.ISMSLEDM_ActiveFlag = true;
                    res.ISMSLEDM_CreatedBy = dto.user_id;
                    _vmsconte.Add(res);
                    foreach (var pdm in dto.product_demo_master_temp1)
                    {
                        ISM_Sales_Lead_Demo_Products_DMO dm = new ISM_Sales_Lead_Demo_Products_DMO();
                        dm.ISMSMPR_Id = pdm.ISMSMPR_Id;
                        dm.ISMSLEDMPR_DiscussionPoints = pdm.ISMSLEDMPR_DiscussionPoints;
                        dm.ISMSLEDM_Id = res.ISMSLEDM_Id;
                        dm.CreatedDate = DateTime.Now;
                        dm.ISMSLEDMPR_CreatedBy = dto.user_id;
                        dm.MI_Id = dto.MI_Id;
                        _vmsconte.Add(dm);
                    }
                    _vmsconte.SaveChanges();
                    dto.return_status = "Add";

                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }
        public ISM_Sales_Lead_Demo_DTO Save_response_lead_demo(ISM_Sales_Lead_Demo_DTO dto)
        {
            try
            {
                foreach (var p in dto.product_demo_master_temp2)
                {
                    var result = _vmsconte.ISM_Sales_Lead_Demo_Products_DMO_con.Single(a => a.ISMSLEDM_Id == p.ISMSLEDM_Id && a.ISMSMPR_Id == p.ISMSMPR_Id);

                    var contactExistsP = _vmsconte.Database.ExecuteSqlCommand("ISM_demo_response_update_proc @p0,@p1,@p2,@p3,@p4", p.ISMSLEDM_Id, result.ISMSLEDMPR_Id, dto.MI_Id, p.ISMSMST_Id, p.ISMSLEDMPR_Remarks);
                    if (contactExistsP > 0)
                    {
                        dto.return_status = "Updated";
                    }
                    else
                    {
                        dto.return_status = "notUpdated";
                    }

                    //SqlConnection con = new SqlConnection("Data Source=demovaps.database.windows.net,1433;Initial Catalog=DevelopmentDataBase;Persist Security Info=False;User ID=demovaps;Password=vaps@123;Connection Timeout=30;");

                    //SqlCommand cmd = new SqlCommand("ISM_demo_response_update_proc", con);
                    //cmd.CommandType = CommandType.StoredProcedure;
                    //cmd.Parameters.AddWithValue("@ISMSLEDM_Id", p.ISMSLEDM_Id);
                    //cmd.Parameters.AddWithValue("@ISMSLEDMPR_Id", result.ISMSLEDMPR_Id);
                    //cmd.Parameters.AddWithValue("@MI_Id", dto.MI_Id);
                    //cmd.Parameters.AddWithValue("@ISMSMST_Id", p.ISMSMST_Id);
                    //cmd.Parameters.AddWithValue("@ISMSLEDMPR_Remarks", p.ISMSLEDMPR_Remarks);
                    //con.Open();
                    //cmd.ExecuteNonQuery();
                    //con.Close();
                }
                _vmsconte.SaveChanges();
                dto.return_status = "Update";
            }


            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }
        public ISM_Sales_Lead_Demo_DTO view_lead_demo(ISM_Sales_Lead_Demo_DTO dto)
        {
            try
            {
                dto.view_lead_demo = (from a in _vmsconte.ISM_Sales_Lead_DMO_con
                                      from b in _vmsconte.ISM_Sales_Master_Status_DMO_con
                                      from c in _vmsconte.ISM_Sales_Lead_Demo_DMO_con
                                      from d in _vmsconte.Hr_Master_Employee_con

                                      where a.ISMSMST_Id == b.ISMSMST_Id && a.ISMSLE_Id == c.ISMSLE_Id && a.MI_Id == dto.MI_Id && c.ISMSLEDM_Id == dto.ISMSLEDM_Id
                                      && a.ISMSLE_Id == dto.ISMSLE_Id && c.HRME_Id == d.HRME_Id

                                      select new ISM_Sales_Lead_Demo_DTO
                                      {
                                          employeename = ((d.HRME_EmployeeFirstName == null || d.HRME_EmployeeFirstName == "" ? "" : d.HRME_EmployeeFirstName)
                                          + (d.HRME_EmployeeMiddleName == null || d.HRME_EmployeeMiddleName == "" ? "" : " " + d.HRME_EmployeeMiddleName)
                                          + (d.HRME_EmployeeLastName == null || d.HRME_EmployeeLastName == "" ? "" : " " + d.HRME_EmployeeLastName)).Trim(),
                                          HRME_Id = d.HRME_Id,
                                          ISMSLE_LeadName = a.ISMSLE_LeadName,
                                          ISMSLE_LeadCode = a.ISMSLE_LeadCode,
                                          ISMSLEDM_ContactPerson = c.ISMSLEDM_ContactPerson,
                                          ISMSLEDM_DemoAddress = c.ISMSLEDM_DemoAddress,
                                          ISMSLEDM_Remarks = c.ISMSLEDM_Remarks

                                      }).ToArray();

                dto.product_dd_s = (from b in _vmsconte.ISM_Sales_Lead_Demo_Products_DMO_con
                                    from c in _vmsconte.ISM_Sales_Lead_Demo_DMO_con
                                    from d in _vmsconte.ISM_Sales_Master_Product_DMO_con
                                    where b.ISMSLEDM_Id == c.ISMSLEDM_Id && b.ISMSLEDM_Id == dto.ISMSLEDM_Id && c.ISMSLE_Id == dto.ISMSLE_Id && b.MI_Id == dto.MI_Id 
                                    && b.ISMSMPR_Id == d.ISMSMPR_Id

                                    select new ISM_Sales_Lead_Demo_DTO
                                    {
                                        ISMSMPR_ProductName = d.ISMSMPR_ProductName,
                                        ISMSMPR_Id = b.ISMSMPR_Id,
                                        ISMSLEDMPR_DiscussionPoints = b.ISMSLEDMPR_DiscussionPoints,
                                        ISMSLEDMPR_ActiveFlag = b.ISMSLEDMPR_ActiveFlag
                                    }).ToArray();

                if (dto.viewall == 1)
                {
                    dto.demo_response_details = (from b in _vmsconte.ISM_Sales_Lead_Demo_Products_DMO_con
                                                 from c in _vmsconte.ISM_Sales_Lead_Demo_DMO_con
                                                 from d in _vmsconte.ISM_Sales_Master_Product_DMO_con
                                                 from e in _vmsconte.ISM_Sales_Master_Status_DMO_con
                                                 where b.ISMSLEDM_Id == c.ISMSLEDM_Id && b.ISMSLEDM_Id == dto.ISMSLEDM_Id && b.MI_Id == dto.MI_Id 
                                                 && b.ISMSMPR_Id == d.ISMSMPR_Id && e.ISMSMST_Id == b.ISMSMST_Id

                                                 select new ISM_Sales_Lead_Demo_DTO
                                                 {
                                                     ISMSMPR_ProductName = d.ISMSMPR_ProductName,
                                                     ISMSMPR_Id = b.ISMSMPR_Id,
                                                     ISMSLEDMPR_DiscussionPoints = b.ISMSLEDMPR_DiscussionPoints,
                                                     ISMSLEDM_Id = c.ISMSLEDM_Id,
                                                     ISMSLEDMPR_Remarks = b.ISMSLEDMPR_Remarks,
                                                     ISMSMST_Id = b.ISMSMST_Id,
                                                     ISMSMST_StatusName = e.ISMSMST_StatusName
                                                 }).ToArray();
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }
        public ISM_Sales_Lead_Demo_DTO Edit_response_lead_demo(ISM_Sales_Lead_Demo_DTO dto)
        {
            try
            {
                dto.demo_response_list = (from b in _vmsconte.ISM_Sales_Lead_Demo_Products_DMO_con
                                          from c in _vmsconte.ISM_Sales_Lead_Demo_DMO_con
                                          from d in _vmsconte.ISM_Sales_Master_Product_DMO_con
                                          where b.ISMSLEDM_Id == c.ISMSLEDM_Id && b.ISMSLEDM_Id == dto.ISMSLEDM_Id && c.ISMSLE_Id == dto.ISMSLE_Id && b.MI_Id == dto.MI_Id && b.ISMSMPR_Id == d.ISMSMPR_Id && b.ISMSMST_Id == null

                                          select new ISM_Sales_Lead_Demo_DTO
                                          {
                                              ISMSMPR_ProductName = d.ISMSMPR_ProductName,
                                              ISMSMPR_Id = b.ISMSMPR_Id,
                                              ISMSLEDMPR_DiscussionPoints = b.ISMSLEDMPR_DiscussionPoints,
                                              ISMSLEDM_Id = c.ISMSLEDM_Id
                                          }).ToArray();

                dto.status_demo_master = _vmsconte.ISM_Sales_Master_Status_DMO_con.Where(a => a.ISMSMST_ActiveFlag == true).ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }

        //Lead Email-------------
        public string sendmail(long MI_Id, string Email, string Template, long UserID)
        {
            try
            {
                string mailcc = "";
                string mailbcc = "";
                Dictionary<string, string> val = new Dictionary<string, string>();
                var template = _db.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_MailActiveFlag == true).ToList();
                if (template.Count == 0)
                {
                    return "";
                }

                var institutionName = _db.Institution.Where(i => i.MI_Id == MI_Id).ToList();
                var Paramaeters = _db.SMS_MAIL_SAVED_PARAMETER_DMO.Where(i => i.MI_Id == MI_Id && i.ISES_Id == template[0].ISES_Id 
                && i.Flag == "M").Select(d => d.ISMP_ID).ToList();

                var ParamaetersName = _db.SMS_MAIL_PARAMETER_DMO.Where(i => Paramaeters.Contains(i.ISMP_ID)).ToList();
                string Mailcontent = template.FirstOrDefault().ISES_SMSMessage;
                string Mailmsg = template.FirstOrDefault().ISES_MailHTMLTemplate;
                string Resultsms = Mailcontent;
                string result = Mailmsg;
                List<Match> variables = new List<Match>();
                foreach (Match match in Regex.Matches(Mailmsg, @"\[.*?\]"))
                {
                    variables.Add(match);
                }
                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {

                    cmd.CommandText = "SMSMAILPARAMETER";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@UserID",
                        SqlDbType.BigInt)
                    {
                        Value = UserID
                    });
                    cmd.Parameters.Add(new SqlParameter("@template",
                       SqlDbType.VarChar)
                    {
                        Value = Template
                    });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();
                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                                {
                                    dataRow.Add(
                                        dataReader.GetName(iFiled),
                                        dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                                    );
                                    var datatype = dataReader.GetFieldType(iFiled);
                                    if (datatype.Name == "DateTime")
                                    {
                                        var dateval = (Convert.ToDateTime(dataReader[iFiled]).Date).ToString("d");
                                        val.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dateval);
                                    }
                                    else
                                    {
                                        val.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled].ToString());
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        return ex.Message;
                    }

                }
                for (int j = 0; j < ParamaetersName.Count; j++)
                {
                    for (int p = 0; p < val.Count; p++)
                    {
                        if (ParamaetersName[j].ISMP_NAME.Equals(val.Keys.ToArray()[p]))
                        {
                            result = Mailmsg.Replace(ParamaetersName[j].ISMP_NAME, val.Values.ToArray()[p]);
                            Mailmsg = result;
                        }
                    }
                }
                Mailmsg = result;
                for (int j = 0; j < ParamaetersName.Count; j++)
                {
                    for (int p = 0; p < val.Count; p++)
                    {
                        if (ParamaetersName[j].ISMP_NAME.Equals(val.Keys.ToArray()[p]))
                        {
                            Resultsms = Mailcontent.Replace(ParamaetersName[j].ISMP_NAME, val.Values.ToArray()[p]);
                            Mailcontent = Resultsms;
                        }
                    }
                }
                Mailcontent = Resultsms;
                List<EMAIL_DETAILS_DMO> alldetails = new List<EMAIL_DETAILS_DMO>();
                alldetails = _db.EMAIL_DETAILS_DMO.Where(t => t.MI_ID.Equals(MI_Id)).ToList();
                string Attechement = "";
                try
                {
                    List<ISM_Sales_Lead_Demo_DTO> dtonewtemp = new List<ISM_Sales_Lead_Demo_DTO>();
                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "Fetch_Leadccmails";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@HRME_Id",
                        SqlDbType.BigInt)
                        {
                            Value = 1
                        });

                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();

                        var retObject = new List<dynamic>();
                        try
                        {
                            using (var dataReader = cmd.ExecuteReader())
                            {
                                while (dataReader.Read())
                                {
                                    var dataRow = new ExpandoObject() as IDictionary<string, object>;
                                    dtonewtemp.Add(new ISM_Sales_Lead_Demo_DTO
                                    {
                                        return_status = Convert.ToString(dataReader["cc"]),
                                        ISMSLEDM_Remarks = Convert.ToString(dataReader["bcc"])
                                    });
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                    if (dtonewtemp.Count > 0)
                    {
                        for (int j = 0; j < dtonewtemp.Count; j++)
                        {
                            mailcc = dtonewtemp[j].return_status;
                            mailbcc = dtonewtemp[j].ISMSLEDM_Remarks;
                        }
                    }

                }
                catch (Exception error)
                {
                    //
                }
                if (alldetails.Count > 0)
                {
                    string SendingEmail = alldetails[0].IVRMMD_Mail_ID;
                    string SendingEmailPassword = alldetails[0].IVRMMD_Mail_PASSWORD;
                    string SendingEmailHostName = alldetails[0].IVRMMD_HOSTNAME;
                    Int32 PortNumber = Convert.ToInt32(alldetails[0].IVRMMD_PORTNO);
                    string Subject = template[0].ISES_MailSubject.ToString();
                    string sengridkey = alldetails[0].IVRM_sendgridkey.ToString();
                    List<GeneralConfigDMO> smstpdetails = new List<GeneralConfigDMO>();
                    smstpdetails = _db.GenConfig.Where(t => t.MI_Id.Equals(MI_Id)).ToList();
                    if (smstpdetails.FirstOrDefault().IVRMGC_APIOrSMTPFlg == "API")
                    {
                        var message = new SendGridMessage();
                        message.From = new EmailAddress(SendingEmail, institutionName[0].MI_Name);
                        message.Subject = Subject;
                        message.AddTo(Email);
                        var img = _db.IVRM_EMAIL_ATT_DMO.Where(i => i.ISES_Id == template[0].ISES_Id).ToList();
                        if (img.Count > 0)
                        {
                            for (int i = 0; i < img.Count; i++)
                            {
                                if (img[i].IVRM_Att_Path != null && img[i].IVRM_Att_Path != "")
                                {
                                    var webClient = new WebClient();
                                    byte[] imageBytes = webClient.DownloadData(img[i].IVRM_Att_Path);
                                    string fileContentsAsBase64 = Convert.ToBase64String(imageBytes);
                                    message.AddAttachment(img[i].IVRM_Att_Name, fileContentsAsBase64, null, null, null);
                                }
                            }
                        }
                        if (mailcc != null && mailcc != "")
                        {
                            string[] ccmaildetails = mailcc.Split(',');

                            foreach (var c in ccmaildetails)
                            {
                                if (c != Email)
                                {
                                    message.AddCc(c);
                                }
                            }
                        }
                        if (mailbcc != null && mailbcc != "")
                        {
                            string[] bccmaildetails = mailbcc.Split(',');

                            foreach (var c in bccmaildetails)
                            {
                                if (c != Email)
                                {
                                    message.AddBcc(c);
                                }
                            }
                        }
                        message.HtmlContent = Mailmsg;
                        var client = new SendGridClient(sengridkey);
                        client.SendEmailAsync(message).Wait();
                    }
                    else
                    {
                        using (var clientsmtp = new SmtpClient())
                        {
                            var credential = new NetworkCredential
                            {
                                UserName = smstpdetails.FirstOrDefault().IVRMGC_emailUserName,
                                Password = smstpdetails.FirstOrDefault().IVRMGC_emailPassword
                            };
                            clientsmtp.Credentials = credential;
                            clientsmtp.Host = smstpdetails.FirstOrDefault().IVRMGC_HostName;
                            clientsmtp.Port = smstpdetails.FirstOrDefault().IVRMGC_PortNo;
                            clientsmtp.EnableSsl = true;
                            using (var emailMessage = new MailMessage())
                            {
                                emailMessage.To.Add(new MailAddress(Email));
                                emailMessage.From = new MailAddress(smstpdetails.FirstOrDefault().IVRMGC_emailUserName);
                                emailMessage.Subject = Subject;
                                emailMessage.Body = Mailmsg;
                                emailMessage.IsBodyHtml = true;
                                if (Attechement.Equals("1"))
                                {
                                    var img = _db.IVRM_EMAIL_ATT_DMO.Where(i => i.ISES_Id == template[0].ISES_Id).ToList();
                                    if (img.Count > 0)
                                    {
                                        for (int i = 0; i < img.Count; i++)
                                        {
                                            foreach (var attache in img.ToList())
                                            {
                                                //emailMessage.Attachments.Add(new System.Net.Mail.Attachment("https://bdcampusstrg.blob.core.windows.net/files/4/Prospects Ver 03.pdf"));
                                                System.Net.HttpWebRequest request = System.Net.WebRequest.Create(attache.IVRM_Att_Path) as HttpWebRequest;
                                                System.Net.HttpWebResponse response = request.GetResponseAsync().Result as System.Net.HttpWebResponse;
                                                Stream stream = response.GetResponseStream();
                                                emailMessage.Attachments.Add(new System.Net.Mail.Attachment(stream, attache.IVRM_Att_Name));
                                            }
                                        }
                                    }
                                }

                                if (mailcc != null && mailcc != "")
                                {
                                    string[] ccmaildetails = mailcc.Split(',');

                                    foreach (var c in ccmaildetails)
                                    {
                                        emailMessage.CC.Add(c);
                                    }
                                }
                                if (mailbcc != null && mailbcc != "")
                                {
                                    string[] bccmaildetails = mailbcc.Split(',');

                                    foreach (var c in bccmaildetails)
                                    {
                                        emailMessage.Bcc.Add(c);
                                    }
                                }
                                clientsmtp.Send(emailMessage);
                            }
                        }
                    }
                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        var template1010 = _db.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_SMSActiveFlag == true).Select(d => d.IVRMIM_Id).ToList();
                        var moduleid = _db.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(d => d.IVRMM_Id).ToList();
                        var modulename = _db.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(d => d.IVRMM_ModuleName).ToList();
                        cmd.CommandText = "IVRM_Email_Outgoing";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@EmailId",
                            SqlDbType.NVarChar)
                        {
                            Value = Email
                        });
                        cmd.Parameters.Add(new SqlParameter("@Message",
                           SqlDbType.NVarChar)
                        {
                            Value = Mailcontent
                        });
                        cmd.Parameters.Add(new SqlParameter("@module",
                        SqlDbType.VarChar)
                        {
                            Value = modulename[0]
                        });
                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                       SqlDbType.BigInt)
                        {
                            Value = MI_Id
                        });

                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();
                        try
                        {
                            using (var dataReader = cmd.ExecuteReader())
                            {
                            }
                        }
                        catch (Exception ex)
                        {
                            return ex.Message;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return ex.Message;
            }
            return "success";
        }
    }
}

