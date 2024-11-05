using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using CommonLibrary;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.IssueManager;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.IssueManager.PettyCash;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.IssueManager.PettyCash;

namespace IssueManager.com.PettyCash.Services
{
    public class PC_RequisitionImpl : Interface.PC_RequisitionInterface
    {
        public PettyCashContext _context;
        public DomainModelMsSqlServerContext _Contextdb;
        public PC_RequisitionImpl(PettyCashContext _con, DomainModelMsSqlServerContext _Contextdbsql)
        {
            _context = _con;
            _Contextdb = _Contextdbsql;
        }
        public PC_RequisitionDTO onloaddata(PC_RequisitionDTO data)
        {
            try
            {

                data.getdept = _context.HR_Master_Department.Where(a => a.MI_Id == data.MI_Id && a.HRMD_ActiveFlag == true).OrderBy(a => a.HRMD_DepartmentName).ToArray();
                data.getparticulars = _context.PC_Master_ParticularsDMO.Where(a => a.MI_Id == data.MI_Id && a.PCMPART_ActiveFlg == true).ToArray();

                var rolet = _context.MasterRoleType.Where(t => t.IVRMRT_Id == data.roleid).ToList();
                var roletype = "";

                if (rolet.Count > 0)
                {
                    roletype = rolet.FirstOrDefault().IVRMRT_Role;
                    if (roletype.Equals("Admin", StringComparison.OrdinalIgnoreCase) || roletype.Equals("HR", StringComparison.OrdinalIgnoreCase))
                    {
                        data.getloaddata = (from a in _context.PC_RequisitionDMO
                                            from b in _context.MasterEmployee
                                            from c in _context.HR_Master_Department
                                            where (a.HRME_Id == b.HRME_Id && b.HRMD_Id == c.HRMD_Id && a.MI_Id == data.MI_Id)
                                            select new PC_RequisitionDTO
                                            {
                                                departmentname = c.HRMD_DepartmentName,
                                                employeename = ((b.HRME_EmployeeFirstName == null || b.HRME_EmployeeFirstName == "" ? "" : " " + b.HRME_EmployeeFirstName) + (b.HRME_EmployeeMiddleName == null || b.HRME_EmployeeMiddleName == "" || b.HRME_EmployeeMiddleName == "0" ? "" : " " + b.HRME_EmployeeMiddleName) + (b.HRME_EmployeeLastName == null || b.HRME_EmployeeLastName == ""
                                                || b.HRME_EmployeeLastName == "0" ? "" : " " + b.HRME_EmployeeLastName) + (b.HRME_EmployeeCode == null
                                                || b.HRME_EmployeeCode == "" ? "" : " : " + b.HRME_EmployeeCode)).Trim(),
                                                PCREQTN_ActiveFlg = a.PCREQTN_ActiveFlg,
                                                PCREQTN_Date = a.PCREQTN_Date,
                                                PCREQTN_Purpose = a.PCREQTN_Purpose,
                                                PCREQTN_Id = a.PCREQTN_Id,
                                                PCREQTN_RequisitionNo = a.PCREQTN_RequisitionNo,
                                                PCREQTN_CreatedDate = a.PCREQTN_CreatedDate,
                                            }).OrderByDescending(a => a.PCREQTN_CreatedDate).ToArray();
                    }
                    else
                    {
                        data.getloaddata = (from a in _context.PC_RequisitionDMO
                                            from b in _context.MasterEmployee
                                            from c in _context.HR_Master_Department
                                            where (a.HRME_Id == b.HRME_Id && b.HRMD_Id == c.HRMD_Id && a.MI_Id == data.MI_Id && a.PCREQTN_CreatedBy == data.Userid)
                                            select new PC_RequisitionDTO
                                            {
                                                departmentname = c.HRMD_DepartmentName,
                                                employeename = ((b.HRME_EmployeeFirstName == null || b.HRME_EmployeeFirstName == "" ? "" : " " + b.HRME_EmployeeFirstName) + (b.HRME_EmployeeMiddleName == null || b.HRME_EmployeeMiddleName == "" || b.HRME_EmployeeMiddleName == "0" ? "" : " " + b.HRME_EmployeeMiddleName) + (b.HRME_EmployeeLastName == null || b.HRME_EmployeeLastName == ""
                                                || b.HRME_EmployeeLastName == "0" ? "" : " " + b.HRME_EmployeeLastName) + (b.HRME_EmployeeCode == null
                                                || b.HRME_EmployeeCode == "" ? "" : " : " + b.HRME_EmployeeCode)).Trim(),
                                                PCREQTN_ActiveFlg = a.PCREQTN_ActiveFlg,
                                                PCREQTN_Date = a.PCREQTN_Date,
                                                PCREQTN_Purpose = a.PCREQTN_Purpose,
                                                PCREQTN_Id = a.PCREQTN_Id,
                                                PCREQTN_RequisitionNo = a.PCREQTN_RequisitionNo,
                                                PCREQTN_CreatedDate = a.PCREQTN_CreatedDate,
                                            }).OrderByDescending(a => a.PCREQTN_CreatedDate).ToArray();
                    }
                }
                else
                {
                    roletype = "";
                }

                data.getapprovedindent = (from a in _context.PC_IndentDMO
                                          from b in _context.PC_Indent_DetailsDMO
                                          where (a.PCINDENT_Id == b.PCINDENT_Id && a.MI_Id == data.MI_Id)
                                          select new PC_RequisitionDTO
                                          {
                                              PCREQTN_Id = b.PCREQTN_Id
                                          }).Distinct().ToArray();
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public PC_RequisitionDTO onchangedept(PC_RequisitionDTO data)
        {

            try
            {
                data.getemp = (from a in _context.MasterEmployee
                               from b in _context.HR_Master_Department
                               where (a.HRMD_Id == b.HRMD_Id && a.HRME_ActiveFlag == true && a.HRME_LeftFlag == false && a.MI_Id == data.MI_Id
                               && a.HRMD_Id == data.HRMD_Id)
                               select new PC_RequisitionDTO
                               {
                                   HRME_Id = a.HRME_Id,
                                   empname = ((a.HRME_EmployeeFirstName == null || a.HRME_EmployeeFirstName == "" ? "" : " " + a.HRME_EmployeeFirstName) + (a.HRME_EmployeeMiddleName == null || a.HRME_EmployeeMiddleName == "" || a.HRME_EmployeeMiddleName == "0" ? "" : " " + a.HRME_EmployeeMiddleName) +
                                   (a.HRME_EmployeeLastName == null || a.HRME_EmployeeLastName == "" || a.HRME_EmployeeLastName == "0" ? "" : " " + a.HRME_EmployeeLastName) +
                                    (a.HRME_EmployeeCode == null || a.HRME_EmployeeCode == "" ? "" : " : " + a.HRME_EmployeeCode)).Trim(),

                               }).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public PC_RequisitionDTO saverecord(PC_RequisitionDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

                if (data.PCREQTN_Id > 0)
                {
                    var checkid = _context.PC_RequisitionDMO.Where(a => a.MI_Id == data.MI_Id && a.PCREQTN_Id == data.PCREQTN_Id).ToList();

                    if (checkid.Count > 0)
                    {
                        var checkidresult = _context.PC_RequisitionDMO.Single(a => a.MI_Id == data.MI_Id && a.PCREQTN_Id == data.PCREQTN_Id);
                        checkidresult.PCREQTN_Date = data.PCREQTN_Date;
                        checkidresult.PCREQTN_Purpose = data.PCREQTN_Purpose;
                        checkidresult.PCREQTN_UpdatedDate = indiantime0;
                        checkidresult.PCREQTN_UpdatedBy = data.Userid;
                        checkidresult.PCREQTN_TotAmount = data.PCREQTN_TotAmount;

                        _context.Update(checkidresult);

                        List<long> temparr = new List<long>();
                        List<long> temparr1 = new List<long>();

                        //GETTING ALL FILES
                        foreach (PC_Requisition_DetailsDTO ph in data.PC_Requisition_DetailsDTO)
                        {
                            temparr.Add(ph.PCREQTNDET_Id);
                        }

                        //REMOVING FILES
                        Array files_Noresultremove = _context.PC_Requisition_DetailsDMO.Where(t => !temparr.Contains(t.PCREQTNDET_Id)
                        && t.PCREQTN_Id == data.PCREQTN_Id).ToArray();
                        foreach (PC_Requisition_DetailsDMO ph1 in files_Noresultremove)
                        {
                            _context.Remove(ph1);
                        }


                        foreach (PC_Requisition_DetailsDTO ph in data.PC_Requisition_DetailsDTO)
                        {

                            if (ph.PCREQTNDET_Id > 0)
                            {
                                var Files_Noresult = _context.PC_Requisition_DetailsDMO.Single(t => t.PCREQTNDET_Id == ph.PCREQTNDET_Id);
                                Files_Noresult.PCREQTNDET_Remarks = ph.PCREQTNDET_Remarks;
                                Files_Noresult.PCREQTNDET_Amount = ph.PCREQTNDET_Amount;
                                Files_Noresult.PCREQTNDET_UpdatedBy = data.Userid;
                                Files_Noresult.PCREQTNDET_UpdatedDate = indiantime0;
                                _context.Update(Files_Noresult);
                            }
                            else
                            {
                                PC_Requisition_DetailsDMO pC_Requisition_DetailsDMO = new PC_Requisition_DetailsDMO();

                                pC_Requisition_DetailsDMO.PCREQTN_Id = data.PCREQTN_Id;
                                pC_Requisition_DetailsDMO.PCMPART_Id = ph.PCMPART_Id;
                                pC_Requisition_DetailsDMO.PCREQTNDET_Amount = ph.PCREQTNDET_Amount;
                                pC_Requisition_DetailsDMO.PCREQTNDET_ActiveFlg = true;
                                pC_Requisition_DetailsDMO.PCREQTNDET_Remarks = ph.PCREQTNDET_Remarks;
                                pC_Requisition_DetailsDMO.PCREQTNDET_CreatedDate = indiantime0;
                                pC_Requisition_DetailsDMO.PCREQTNDET_UpdatedDate = indiantime0;
                                pC_Requisition_DetailsDMO.PCREQTNDET_CreatedBy = data.Userid;
                                pC_Requisition_DetailsDMO.PCREQTNDET_UpdatedBy = data.Userid;

                                _context.Add(pC_Requisition_DetailsDMO);
                            }
                        }

                        if (data.uploaddocments != null && data.uploaddocments.Length > 0)
                        {
                            foreach (var c in data.uploaddocments)
                            {
                                var contactExistsP = _context.Database.ExecuteSqlCommand("PC_Requistion_Upload_Insert @p0,@p1,@p2,@p3", data.PCREQTN_Id, c.PCREQTNUP_Id, c.PCREQTNUP_FileName, c.PCREQTNUP_FileLocation);
                                if (contactExistsP > 0)
                                {
                                    data.message = "Add";
                                }
                                else
                                {
                                    data.message = "notAdded";
                                }

                            }
                        }

                        var i = _context.SaveChanges();

                        if (i > 0)
                        {
                            data.message = "Update";
                            data.returnval = true;
                        }
                        else
                        {
                            data.message = "Update";
                            data.returnval = false;
                        }

                    }
                }
                else
                {
                    //Master_NumberingDTO check = new Master_NumberingDTO();
                    //data.transnumbconfigurationsettingsss = check;
                    //List<Master_Numbering> MM = new List<Master_Numbering>();

                    //MM = _Contextdb.Master_Numbering.Where(t => t.IMN_Flag == "PCREQUISITION" && t.MI_Id == data.MI_Id).ToList();
                    //if (MM.Count() > 0)
                    //{
                    //    data.transnumbconfigurationsettingsss.IMN_AutoManualFlag = MM.FirstOrDefault().IMN_AutoManualFlag;
                    //    data.transnumbconfigurationsettingsss.IMN_DuplicatesFlag = MM.FirstOrDefault().IMN_DuplicatesFlag;
                    //    data.transnumbconfigurationsettingsss.IMN_Flag = MM.FirstOrDefault().IMN_Flag;
                    //    data.transnumbconfigurationsettingsss.IMN_Id = MM.FirstOrDefault().IMN_Id;
                    //    data.transnumbconfigurationsettingsss.IMN_PrefixAcadYearCode = MM.FirstOrDefault().IMN_PrefixAcadYearCode;
                    //    data.transnumbconfigurationsettingsss.IMN_PrefixCalYearCode = MM.FirstOrDefault().IMN_PrefixCalYearCode;
                    //    data.transnumbconfigurationsettingsss.IMN_PrefixFinYearCode = MM.FirstOrDefault().IMN_PrefixFinYearCode;
                    //    data.transnumbconfigurationsettingsss.IMN_PrefixParticular = MM.FirstOrDefault().IMN_PrefixParticular;
                    //    data.transnumbconfigurationsettingsss.IMN_RestartNumFlag = MM.FirstOrDefault().IMN_RestartNumFlag;
                    //    data.transnumbconfigurationsettingsss.IMN_StartingNo = MM.FirstOrDefault().IMN_StartingNo;
                    //    data.transnumbconfigurationsettingsss.IMN_SuffixAcadYearCode = MM.FirstOrDefault().IMN_SuffixAcadYearCode;
                    //    data.transnumbconfigurationsettingsss.IMN_SuffixCalYearCode = MM.FirstOrDefault().IMN_SuffixCalYearCode;
                    //    data.transnumbconfigurationsettingsss.IMN_SuffixFinYearCode = MM.FirstOrDefault().IMN_SuffixFinYearCode;
                    //    data.transnumbconfigurationsettingsss.IMN_SuffixParticular = MM.FirstOrDefault().IMN_SuffixParticular;
                    //    data.transnumbconfigurationsettingsss.IMN_WidthNumeric = MM.FirstOrDefault().IMN_WidthNumeric;
                    //    data.transnumbconfigurationsettingsss.IMN_ZeroPrefixFlag = MM.FirstOrDefault().IMN_ZeroPrefixFlag;
                    //    data.transnumbconfigurationsettingsss.MI_Id = MM.FirstOrDefault().MI_Id;
                    //}

                    //if (data.transnumbconfigurationsettingsss.IMN_AutoManualFlag == "Auto")
                    //{
                    //    GenerateTransactionNumbering a = new GenerateTransactionNumbering(_Contextdb);
                    //    data.transnumbconfigurationsettingsss.MI_Id = data.MI_Id;
                    //    data.transnumbconfigurationsettingsss.ASMAY_Id = data.ASMAY_Id;
                    //    data.Auto_id = a.GenerateNumber(data.transnumbconfigurationsettingsss);
                    //}

                    var acd_Id = _context.IVRM_Master_FinancialYear.Where(t => Convert.ToDateTime(t.IMFY_FromDate) <= Convert.ToDateTime(indiantime0.Date) && Convert.ToDateTime(t.IMFY_ToDate) >= Convert.ToDateTime(indiantime0.Date)).Select(d => d.IMFY_Id).FirstOrDefault();

                    PettyCash_AutoRefernceNo_Generation pettyCash_AutoRefernceNo_Generation = new PettyCash_AutoRefernceNo_Generation(_context);
                    var refernceno = pettyCash_AutoRefernceNo_Generation.GenerateReferenceNo(data.MI_Id, acd_Id, "Requisition");

                    if (refernceno == "" || refernceno == "Error")
                    {
                        data.saveorupdate = "ReferenceNotGenerated";
                        return data;
                    }

                    PC_RequisitionDMO pC_RequisitionDMO = new PC_RequisitionDMO();
                    pC_RequisitionDMO.MI_Id = data.MI_Id;
                    pC_RequisitionDMO.HRME_Id = data.HRME_Id;
                    pC_RequisitionDMO.PCREQTN_Purpose = data.PCREQTN_Purpose;
                    pC_RequisitionDMO.PCREQTN_TotAmount = data.PCREQTN_TotAmount;
                    pC_RequisitionDMO.PCREQTN_Date = data.PCREQTN_Date;
                    pC_RequisitionDMO.PCREQTN_RequisitionNo = refernceno;
                    pC_RequisitionDMO.PCREQTN_ActiveFlg = true;
                    pC_RequisitionDMO.PCREQTN_CreatedDate = indiantime0;
                    pC_RequisitionDMO.PCREQTN_UpdatedDate = indiantime0;
                    pC_RequisitionDMO.PCREQTN_CreatedBy = data.Userid;
                    pC_RequisitionDMO.PCREQTN_UpdatedBy = data.Userid;

                    _context.Add(pC_RequisitionDMO);

                    foreach (var c in data.PC_Requisition_DetailsDTO)
                    {
                        PC_Requisition_DetailsDMO pC_Requisition_DetailsDMO = new PC_Requisition_DetailsDMO();

                        pC_Requisition_DetailsDMO.PCREQTN_Id = pC_RequisitionDMO.PCREQTN_Id;
                        pC_Requisition_DetailsDMO.PCMPART_Id = c.PCMPART_Id;
                        pC_Requisition_DetailsDMO.PCREQTNDET_Amount = c.PCREQTNDET_Amount;
                        pC_Requisition_DetailsDMO.PCREQTNDET_ActiveFlg = true;
                        pC_Requisition_DetailsDMO.PCREQTNDET_Remarks = c.PCREQTNDET_Remarks;
                        pC_Requisition_DetailsDMO.PCREQTNDET_CreatedDate = indiantime0;
                        pC_Requisition_DetailsDMO.PCREQTNDET_UpdatedDate = indiantime0;
                        pC_Requisition_DetailsDMO.PCREQTNDET_CreatedBy = data.Userid;
                        pC_Requisition_DetailsDMO.PCREQTNDET_UpdatedBy = data.Userid;

                        _context.Add(pC_Requisition_DetailsDMO);
                    }

                    var i = _context.SaveChanges();

                    if (data.uploaddocments != null && data.uploaddocments.Length > 0)
                    {
                        foreach (var c in data.uploaddocments)
                        {
                            var contactExistsP = _context.Database.ExecuteSqlCommand("PC_Requistion_Upload_Insert @p0,@p1,@p2,@p3", pC_RequisitionDMO.PCREQTN_Id, 0, c.PCREQTNUP_FileName, c.PCREQTNUP_FileLocation);
                            if (contactExistsP > 0)
                            {
                                data.message = "Add";
                            }
                            else
                            {
                                data.message = "notAdded";
                            }

                        }
                    }

                    if (i > 0)
                    {
                        data.message = "Add";
                        data.returnval = true;
                    }
                    else
                    {
                        data.message = "Add";
                        data.returnval = false;
                    }
                }
            }
            catch (Exception ex)
            {
                data.message = "Error";
                data.returnval = false;
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public PC_RequisitionDTO EditData(PC_RequisitionDTO data)
        {
            try
            {
                var getdetails = _context.PC_RequisitionDMO.Where(a => a.MI_Id == data.MI_Id && a.PCREQTN_Id == data.PCREQTN_Id).ToList();
                data.documentdetails = _context.PC_RequisitionDocumentDMO.Where(a => a.PCREQTN_Id == data.PCREQTN_Id).ToArray();

                data.geteditdetails = getdetails.ToArray();

                data.geteditdept = (from a in _context.HR_Master_Department
                                    from b in _context.MasterEmployee
                                    where (a.HRMD_Id == b.HRMD_Id && b.HRME_Id == getdetails.FirstOrDefault().HRME_Id)
                                    select a).Distinct().OrderBy(a => a.HRMD_DepartmentName).ToArray();

                data.geteditsavedemp = (from a in _context.MasterEmployee
                                        from b in _context.HR_Master_Department
                                        where (a.HRMD_Id == b.HRMD_Id && a.HRME_ActiveFlag == true && a.HRME_LeftFlag == false && a.MI_Id == data.MI_Id
                                        && a.HRME_Id == getdetails.FirstOrDefault().HRME_Id)
                                        select new PC_RequisitionDTO
                                        {
                                            HRME_Id = a.HRME_Id,
                                            empname = ((a.HRME_EmployeeFirstName == null || a.HRME_EmployeeFirstName == "" ? "" : " " + a.HRME_EmployeeFirstName) + (a.HRME_EmployeeMiddleName == null || a.HRME_EmployeeMiddleName == "" || a.HRME_EmployeeMiddleName == "0" ? "" : " " + a.HRME_EmployeeMiddleName) +
                                            (a.HRME_EmployeeLastName == null || a.HRME_EmployeeLastName == "" || a.HRME_EmployeeLastName == "0" ? "" : " " + a.HRME_EmployeeLastName) +
                                             (a.HRME_EmployeeCode == null || a.HRME_EmployeeCode == "" ? "" : " : " + a.HRME_EmployeeCode)).Trim(),

                                        }).Distinct().ToArray();

                var getdeptid = _context.MasterEmployee.Where(a => a.MI_Id == data.MI_Id && a.HRME_Id == getdetails.FirstOrDefault().HRME_Id).ToList();

                data.geteditemp = (from a in _context.MasterEmployee
                                   from b in _context.HR_Master_Department
                                   where (a.HRMD_Id == b.HRMD_Id && a.HRME_ActiveFlag == true && a.HRME_LeftFlag == false && a.MI_Id == data.MI_Id
                                   && a.HRMD_Id == getdeptid.FirstOrDefault().HRMD_Id)
                                   select new PC_RequisitionDTO
                                   {
                                       HRME_Id = a.HRME_Id,
                                       empname = ((a.HRME_EmployeeFirstName == null || a.HRME_EmployeeFirstName == "" ? "" : " " + a.HRME_EmployeeFirstName) + (a.HRME_EmployeeMiddleName == null || a.HRME_EmployeeMiddleName == "" || a.HRME_EmployeeMiddleName == "0" ? "" : " " + a.HRME_EmployeeMiddleName) +
                                       (a.HRME_EmployeeLastName == null || a.HRME_EmployeeLastName == "" || a.HRME_EmployeeLastName == "0" ? "" : " " + a.HRME_EmployeeLastName) +
                                        (a.HRME_EmployeeCode == null || a.HRME_EmployeeCode == "" ? "" : " : " + a.HRME_EmployeeCode)).Trim(),

                                   }).Distinct().ToArray();


                data.geteditsavedparticulars = (from a in _context.PC_RequisitionDMO
                                                from b in _context.PC_Requisition_DetailsDMO
                                                from c in _context.PC_Master_ParticularsDMO
                                                where (a.PCREQTN_Id == b.PCREQTN_Id && b.PCMPART_Id == c.PCMPART_Id && b.PCREQTN_Id == data.PCREQTN_Id)
                                                select new PC_RequisitionDTO
                                                {
                                                    pcreqtndeT_Id = b.PCREQTNDET_Id,
                                                    PCMPART_Id = b.PCMPART_Id,
                                                    PCREQTN_Id = b.PCREQTN_Id,
                                                    PCMPART_ParticularName = c.PCMPART_ParticularName,
                                                    PCREQTNDET_Amount = b.PCREQTNDET_Amount,
                                                    PCREQTNDET_Remarks = b.PCREQTNDET_Remarks,
                                                    PCREQTNDET_ActiveFlg = b.PCREQTNDET_ActiveFlg,

                                                }).Distinct().ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public PC_RequisitionDTO deactiveY(PC_RequisitionDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

                var getresult = _context.PC_RequisitionDMO.Where(a => a.MI_Id == data.MI_Id && a.PCREQTN_Id == data.PCREQTN_Id).ToList();

                if (getresult.Count > 0)
                {
                    var checkresult = _context.PC_RequisitionDMO.Single(a => a.MI_Id == data.MI_Id && a.PCREQTN_Id == data.PCREQTN_Id);
                    if (checkresult.PCREQTN_ActiveFlg == true)
                    {
                        checkresult.PCREQTN_ActiveFlg = false;
                    }
                    else
                    {
                        checkresult.PCREQTN_ActiveFlg = true;
                    }
                    checkresult.PCREQTN_UpdatedDate = indiantime0;
                    checkresult.PCREQTN_UpdatedBy = data.Userid;

                    _context.Update(checkresult);
                    var i = _context.SaveChanges();
                    if (i > 0)
                    {
                        data.returnval = true;
                    }
                    else
                    {
                        data.returnval = false;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public PC_RequisitionDTO Viewdata(PC_RequisitionDTO data)
        {
            try
            {
                data.getviewdata = (from a in _context.PC_RequisitionDMO
                                    from b in _context.PC_Requisition_DetailsDMO
                                    from c in _context.PC_Master_ParticularsDMO
                                    where (a.PCREQTN_Id == b.PCREQTN_Id && b.PCMPART_Id == c.PCMPART_Id && b.PCREQTN_Id == data.PCREQTN_Id)
                                    select new PC_RequisitionDTO
                                    {
                                        pcreqtndeT_Id = b.PCREQTNDET_Id,
                                        PCMPART_Id = b.PCMPART_Id,
                                        PCREQTN_Id = b.PCREQTN_Id,
                                        PCMPART_ParticularName = c.PCMPART_ParticularName,
                                        PCREQTNDET_Amount = b.PCREQTNDET_Amount,
                                        PCREQTNDET_Remarks = b.PCREQTNDET_Remarks,
                                        PCREQTNDET_ActiveFlg = b.PCREQTNDET_ActiveFlg,

                                    }).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public PC_RequisitionDTO deactiveparticulars(PC_RequisitionDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

                var getresult = _context.PC_Requisition_DetailsDMO.Where(a => a.PCREQTNDET_Id == data.pcreqtndeT_Id).ToList();

                if (getresult.Count > 0)
                {
                    var checkresult = _context.PC_Requisition_DetailsDMO.Single(a => a.PCREQTNDET_Id == data.pcreqtndeT_Id);
                    if (checkresult.PCREQTNDET_ActiveFlg == true)
                    {
                        checkresult.PCREQTNDET_ActiveFlg = false;
                    }
                    else
                    {
                        checkresult.PCREQTNDET_ActiveFlg = true;
                    }
                    checkresult.PCREQTNDET_UpdatedDate = indiantime0;
                    checkresult.PCREQTNDET_UpdatedBy = data.Userid;

                    _context.Update(checkresult);
                    var i = _context.SaveChanges();
                    if (i > 0)
                    {
                        decimal? totamount = 0;
                        var updateamount = _context.PC_Requisition_DetailsDMO.Where(a => a.PCREQTN_Id == data.PCREQTN_Id && a.PCREQTNDET_ActiveFlg == true).ToList();

                        if (updateamount.Count > 0)
                        {
                            foreach (var c in updateamount)
                            {
                                totamount += c.PCREQTNDET_Amount;
                            }

                            var checkresult_main = _context.PC_RequisitionDMO.Single(a => a.PCREQTN_Id == data.PCREQTN_Id);
                            checkresult_main.PCREQTN_TotAmount = totamount;
                            checkresult_main.PCREQTN_UpdatedBy = data.Userid;
                            checkresult_main.PCREQTN_UpdatedDate = indiantime0;

                            _context.Update(checkresult_main);

                            var i1 = _context.SaveChanges();
                            if (i1 > 0)
                            {
                                data.returnval = true;
                            }
                        }
                        data.returnval = true;
                    }
                    else
                    {
                        data.returnval = false;
                    }
                }

                data.getviewdata = (from a in _context.PC_RequisitionDMO
                                    from b in _context.PC_Requisition_DetailsDMO
                                    from c in _context.PC_Master_ParticularsDMO
                                    where (a.PCREQTN_Id == b.PCREQTN_Id && b.PCMPART_Id == c.PCMPART_Id && b.PCREQTN_Id == data.PCREQTN_Id)
                                    select new PC_RequisitionDTO
                                    {
                                        pcreqtndeT_Id = b.PCREQTNDET_Id,
                                        PCMPART_Id = b.PCMPART_Id,
                                        PCREQTN_Id = b.PCREQTN_Id,
                                        PCMPART_ParticularName = c.PCMPART_ParticularName,
                                        PCREQTNDET_Amount = b.PCREQTNDET_Amount,
                                        PCREQTNDET_Remarks = b.PCREQTNDET_Remarks,
                                        PCREQTNDET_ActiveFlg = b.PCREQTNDET_ActiveFlg,

                                    }).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
    }
}