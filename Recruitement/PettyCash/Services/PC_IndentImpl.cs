using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonLibrary;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.IssueManager;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.IssueManager.PettyCash;
using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.IssueManager.PettyCash;

namespace IssueManager.com.PettyCash.Services
{
    public class PC_IndentImpl : Interface.PC_IndentInterface
    {
        public PettyCashContext _context;
        public DomainModelMsSqlServerContext _Contextdb;
        public PC_IndentImpl(PettyCashContext _con, DomainModelMsSqlServerContext _Contextdbsql)
        {
            _context = _con;
            _Contextdb = _Contextdbsql;
        }
        public PC_IndentDTO onloaddata(PC_IndentDTO data)
        {
            try
            {
                var rolet = _context.MasterRoleType.Where(t => t.IVRMRT_Id == data.roleid).ToList();
                var roletype = "";

                var getuserinstitution = _context.UserRoleWithInstituteDMO.Where(a => a.Id == data.Userid).ToList();

                List<long> miids = new List<long>();

                foreach (var c in getuserinstitution)
                {
                    miids.Add(c.MI_Id);
                }

                if (rolet.Count > 0)
                {
                    roletype = rolet.FirstOrDefault().IVRMRT_Role;

                    if (roletype.Equals("Admin", StringComparison.OrdinalIgnoreCase) || roletype.Equals("HR", StringComparison.OrdinalIgnoreCase))
                    {
                        data.getloaddata = (from a in _context.PC_IndentDMO
                                            from b in _context.MasterEmployee
                                            from c in _context.HR_Master_Department
                                            where (a.HRME_Id == b.HRME_Id && b.HRMD_Id == c.HRMD_Id && a.MI_Id == data.MI_Id)
                                            select new PC_IndentDTO
                                            {
                                                departmentname = c.HRMD_DepartmentName,
                                                employeename = ((b.HRME_EmployeeFirstName == null || b.HRME_EmployeeFirstName == "" ? "" : " "
                                                + b.HRME_EmployeeFirstName) + (b.HRME_EmployeeMiddleName == null || b.HRME_EmployeeMiddleName == ""
                                                || b.HRME_EmployeeMiddleName == "0" ? "" : " " + b.HRME_EmployeeMiddleName) + (b.HRME_EmployeeLastName == null
                                                || b.HRME_EmployeeLastName == "" || b.HRME_EmployeeLastName == "0" ? "" : " " + b.HRME_EmployeeLastName)
                                                + (b.HRME_EmployeeCode == null || b.HRME_EmployeeCode == "" ? "" : " : " + b.HRME_EmployeeCode)).Trim(),
                                                PCINDENT_ActiveFlg = a.PCINDENT_ActiveFlg,
                                                PCINDENT_Date = a.PCINDENT_Date,
                                                PCINDENT_Desc = a.PCINDENT_Desc,
                                                PCINDENT_Id = a.PCINDENT_Id,
                                                PCINDENT_IndentNo = a.PCINDENT_IndentNo,
                                                createdate = a.PCINDENT_CreatedDate
                                            }).OrderByDescending(a => a.createdate).ToArray();

                    }
                    else
                    {
                        data.getloaddata = (from a in _context.PC_IndentDMO
                                            from b in _context.MasterEmployee
                                            from c in _context.HR_Master_Department
                                            where (a.HRME_Id == b.HRME_Id && b.HRMD_Id == c.HRMD_Id && a.PCINDENT_CreatedBy == data.Userid && a.MI_Id == data.MI_Id)
                                            select new PC_IndentDTO
                                            {
                                                departmentname = c.HRMD_DepartmentName,
                                                employeename = ((b.HRME_EmployeeFirstName == null || b.HRME_EmployeeFirstName == "" ? "" : " "
                                                + b.HRME_EmployeeFirstName) + (b.HRME_EmployeeMiddleName == null || b.HRME_EmployeeMiddleName == ""
                                                || b.HRME_EmployeeMiddleName == "0" ? "" : " " + b.HRME_EmployeeMiddleName) + (b.HRME_EmployeeLastName == null
                                                || b.HRME_EmployeeLastName == "" || b.HRME_EmployeeLastName == "0" ? "" : " " + b.HRME_EmployeeLastName)
                                                + (b.HRME_EmployeeCode == null || b.HRME_EmployeeCode == "" ? "" : " : " + b.HRME_EmployeeCode)).Trim(),
                                                PCINDENT_ActiveFlg = a.PCINDENT_ActiveFlg,
                                                PCINDENT_Date = a.PCINDENT_Date,
                                                PCINDENT_Desc = a.PCINDENT_Desc,
                                                PCINDENT_Id = a.PCINDENT_Id,
                                                PCINDENT_IndentNo = a.PCINDENT_IndentNo,
                                                createdate = a.PCINDENT_CreatedDate
                                            }).OrderByDescending(a => a.createdate).ToArray();
                    }
                }

                data.getapproveddata = _context.PC_Indent_ApprovedDMO.Where(a => a.MI_Id == data.MI_Id).ToArray();

                data.getuserinstitution = _context.Institution.Where(a => a.MI_ActiveFlag == 1 && miids.Contains(a.MI_Id)).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public PC_IndentDTO OnChangeInstitution(PC_IndentDTO data)
        {
            try
            {
                var rolet = _context.MasterRoleType.Where(t => t.IVRMRT_Id == data.roleid).ToList();
                var roletype = "";

                if (rolet.Count > 0)
                {
                    roletype = rolet.FirstOrDefault().IVRMRT_Role;

                    if (roletype.Equals("Admin", StringComparison.OrdinalIgnoreCase) || roletype.Equals("HR", StringComparison.OrdinalIgnoreCase))
                    {
                        data.getloaddata = (from a in _context.PC_IndentDMO
                                            from b in _context.MasterEmployee
                                            from c in _context.HR_Master_Department
                                            where (a.HRME_Id == b.HRME_Id && b.HRMD_Id == c.HRMD_Id && a.MI_Id == data.MI_Id)
                                            select new PC_IndentDTO
                                            {
                                                departmentname = c.HRMD_DepartmentName,
                                                employeename = ((b.HRME_EmployeeFirstName == null || b.HRME_EmployeeFirstName == "" ? "" : " "
                                                + b.HRME_EmployeeFirstName) + (b.HRME_EmployeeMiddleName == null || b.HRME_EmployeeMiddleName == ""
                                                || b.HRME_EmployeeMiddleName == "0" ? "" : " " + b.HRME_EmployeeMiddleName) + (b.HRME_EmployeeLastName == null
                                                || b.HRME_EmployeeLastName == "" || b.HRME_EmployeeLastName == "0" ? "" : " " + b.HRME_EmployeeLastName)
                                                + (b.HRME_EmployeeCode == null || b.HRME_EmployeeCode == "" ? "" : " : " + b.HRME_EmployeeCode)).Trim(),
                                                PCINDENT_ActiveFlg = a.PCINDENT_ActiveFlg,
                                                PCINDENT_Date = a.PCINDENT_Date,
                                                PCINDENT_Desc = a.PCINDENT_Desc,
                                                PCINDENT_Id = a.PCINDENT_Id,
                                                PCINDENT_IndentNo = a.PCINDENT_IndentNo,
                                                createdate = a.PCINDENT_CreatedDate
                                            }).OrderByDescending(a => a.createdate).ToArray();

                    }
                    else
                    {
                        data.getloaddata = (from a in _context.PC_IndentDMO
                                            from b in _context.MasterEmployee
                                            from c in _context.HR_Master_Department
                                            where (a.HRME_Id == b.HRME_Id && b.HRMD_Id == c.HRMD_Id && a.PCINDENT_CreatedBy == data.Userid && a.MI_Id == data.MI_Id)
                                            select new PC_IndentDTO
                                            {
                                                departmentname = c.HRMD_DepartmentName,
                                                employeename = ((b.HRME_EmployeeFirstName == null || b.HRME_EmployeeFirstName == "" ? "" : " "
                                                + b.HRME_EmployeeFirstName) + (b.HRME_EmployeeMiddleName == null || b.HRME_EmployeeMiddleName == ""
                                                || b.HRME_EmployeeMiddleName == "0" ? "" : " " + b.HRME_EmployeeMiddleName) + (b.HRME_EmployeeLastName == null
                                                || b.HRME_EmployeeLastName == "" || b.HRME_EmployeeLastName == "0" ? "" : " " + b.HRME_EmployeeLastName)
                                                + (b.HRME_EmployeeCode == null || b.HRME_EmployeeCode == "" ? "" : " : " + b.HRME_EmployeeCode)).Trim(),
                                                PCINDENT_ActiveFlg = a.PCINDENT_ActiveFlg,
                                                PCINDENT_Date = a.PCINDENT_Date,
                                                PCINDENT_Desc = a.PCINDENT_Desc,
                                                PCINDENT_Id = a.PCINDENT_Id,
                                                PCINDENT_IndentNo = a.PCINDENT_IndentNo,
                                                createdate = a.PCINDENT_CreatedDate
                                            }).OrderByDescending(a => a.createdate).ToArray();
                    }
                }

                data.getapproveddata = _context.PC_Indent_ApprovedDMO.Where(a => a.MI_Id == data.MI_Id).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public PC_IndentDTO onchangedate(PC_IndentDTO data)
        {
            try
            {
                List<long> PCREQTN_Ids = new List<long>();

                var getrequisitionids = (from a in _context.PC_IndentDMO
                                         from b in _context.PC_Indent_DetailsDMO
                                         where (a.PCINDENT_Id == b.PCINDENT_Id && a.MI_Id == data.MI_Id)
                                         select new PC_IndentDTO
                                         {
                                             PCREQTN_Id = b.PCREQTN_Id
                                         }).Distinct().ToList();

                if (getrequisitionids.Count > 0)
                {
                    foreach (var c in getrequisitionids)
                    {
                        PCREQTN_Ids.Add(c.PCREQTN_Id);
                    }
                }
                else
                {
                    PCREQTN_Ids.Add(0);
                }

                data.requisitiondetais = (from a in _context.PC_RequisitionDMO
                                          from b in _context.MasterEmployee
                                          from c in _context.HR_Master_Department
                                          where (a.HRME_Id == b.HRME_Id && b.HRMD_Id == c.HRMD_Id && !PCREQTN_Ids.Contains(a.PCREQTN_Id) && a.MI_Id == data.MI_Id
                                          && (a.PCREQTN_Date >= data.PCINDENT_Date_From && a.PCREQTN_Date <= data.PCINDENT_Date_To) && a.PCREQTN_ActiveFlg == true)
                                          select new PC_IndentDTO
                                          {
                                              departmentname = c.HRMD_DepartmentName,
                                              employeename = ((b.HRME_EmployeeFirstName == null || b.HRME_EmployeeFirstName == "" ? "" : " " + b.HRME_EmployeeFirstName) + (b.HRME_EmployeeMiddleName == null || b.HRME_EmployeeMiddleName == "" || b.HRME_EmployeeMiddleName == "0" ? "" : " " + b.HRME_EmployeeMiddleName) + (b.HRME_EmployeeLastName == null || b.HRME_EmployeeLastName == ""
                                              || b.HRME_EmployeeLastName == "0" ? "" : " " + b.HRME_EmployeeLastName) + (b.HRME_EmployeeCode == null
                                              || b.HRME_EmployeeCode == "" ? "" : " : " + b.HRME_EmployeeCode)).Trim(),
                                              PCREQTN_Date = a.PCREQTN_Date,
                                              PCREQTN_Purpose = a.PCREQTN_Purpose,
                                              PCREQTN_Id = a.PCREQTN_Id,
                                              PCREQTN_RequisitionNo = a.PCREQTN_RequisitionNo,
                                              PCREQTN_TotAmount = a.PCREQTN_TotAmount,
                                              createdate = a.PCREQTN_CreatedDate

                                          }).OrderByDescending(a => a.createdate).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public PC_IndentDTO getrequisitiondetails(PC_IndentDTO data)
        {
            try
            {
                List<long> ids = new List<long>();

                foreach (var c in data.temp_requisitionid)
                {
                    ids.Add(c.PCREQTN_Id);
                }

                data.requisitionparticulardetais = (from a in _context.PC_RequisitionDMO
                                                    from b in _context.PC_Requisition_DetailsDMO
                                                    from c in _context.PC_Master_ParticularsDMO
                                                    where (a.PCREQTN_Id == b.PCREQTN_Id && b.PCMPART_Id == c.PCMPART_Id && ids.Contains(b.PCREQTN_Id)
                                                    && a.PCREQTN_ActiveFlg == true && b.PCREQTNDET_ActiveFlg == true && a.MI_Id == data.MI_Id)
                                                    select new PC_IndentDTO
                                                    {
                                                        pcreqtndeT_Id = b.PCREQTNDET_Id,
                                                        PCMPART_Id = b.PCMPART_Id,
                                                        PCREQTN_Id = b.PCREQTN_Id,
                                                        PCREQTN_RequisitionNo = a.PCREQTN_RequisitionNo,
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
        public PC_IndentDTO saverecord(PC_IndentDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

                data.HRME_Id = _context.Staff_User_Login.Where(a => a.Id == data.Userid).Select(a => a.Emp_Code).FirstOrDefault();

                if (data.PCINDENT_Id > 0)
                {
                    var checkid = _context.PC_IndentDMO.Where(a => a.PCINDENT_Id == data.PCINDENT_Id).ToList();

                    if (checkid.Count > 0)
                    {
                        var checkidresult = _context.PC_IndentDMO.Single(a => a.PCINDENT_Id == data.PCINDENT_Id);
                        checkidresult.PCINDENT_Date = data.PCINDENT_Date;
                        checkidresult.PCINDENT_Desc = data.PCINDENT_Desc;
                        checkidresult.PCINDENT_UpdatedDate = indiantime0;
                        checkidresult.PCINDENT_UpdatedBy = data.Userid;
                        checkidresult.PCINDENT_SanctionedAmt = data.PCINDENT_ApprovedAmt;

                        _context.Update(checkidresult);

                        List<long> temparr = new List<long>();
                        List<long> temparr1 = new List<long>();

                        //GETTING ALL FILES
                        foreach (PC_Indent_DetailsDTO ph in data.PC_Indent_DetailsDTO)
                        {
                            temparr.Add(ph.PCINDENTDET_Id);
                        }

                        //REMOVING FILES
                        Array files_Noresultremove = _context.PC_Indent_DetailsDMO.Where(t => !temparr.Contains(t.PCINDENTDET_Id)
                        && t.PCREQTN_Id == data.PCREQTN_Id).ToArray();
                        foreach (PC_Indent_DetailsDMO ph1 in files_Noresultremove)
                        {
                            _context.Remove(ph1);
                        }

                        foreach (PC_Indent_DetailsDTO ph in data.PC_Indent_DetailsDTO)
                        {
                            if (ph.PCINDENTDET_Id > 0)
                            {
                                var Files_Noresult = _context.PC_Indent_DetailsDMO.Single(t => t.PCINDENTDET_Id == ph.PCINDENTDET_Id);
                                Files_Noresult.PCINDENTDET_Remarks = ph.PCINDENTDET_Remarks;
                                Files_Noresult.PCINDENTDET_SanctionedAmt = ph.PCINDENTDET_ApprovedAmt;
                                Files_Noresult.PCINDENTDET_UpdatedBy = data.Userid;
                                Files_Noresult.PCINDENTDET_UpdatedDate = indiantime0;
                                _context.Update(Files_Noresult);
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
                    //MM = _Contextdb.Master_Numbering.Where(t => t.IMN_Flag == "PCINDENT" && t.MI_Id == data.MI_Id).ToList();
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
                    var refernceno = pettyCash_AutoRefernceNo_Generation.GenerateReferenceNo(data.MI_Id, acd_Id, "Indent");

                    if (refernceno == "" || refernceno=="Error")
                    {
                        data.saveorupdate = "ReferenceNotGenerated";
                        return data;
                    }

                    PC_IndentDMO pC_IndentDMO = new PC_IndentDMO();
                    pC_IndentDMO.MI_Id = data.MI_Id;
                    pC_IndentDMO.HRME_Id = data.HRME_Id;
                    pC_IndentDMO.PCINDENT_IndentNo = refernceno;
                    pC_IndentDMO.PCINDENT_Date = data.PCINDENT_Date;
                    pC_IndentDMO.PCINDENT_Desc = data.PCINDENT_Desc;
                    pC_IndentDMO.PCINDENT_RequestedAmount = data.PCINDENT_RequestedAmount;
                    pC_IndentDMO.PCINDENT_SanctionedAmt = data.PCINDENT_ApprovedAmt;
                    pC_IndentDMO.PCINDENT_ActiveFlg = true;
                    pC_IndentDMO.PCINDENT_CreatedDate = indiantime0;
                    pC_IndentDMO.PCINDENT_UpdatedDate = indiantime0;
                    pC_IndentDMO.PCINDENT_CreatedBy = data.Userid;
                    pC_IndentDMO.PCINDENT_UpdatedBy = data.Userid;

                    _context.Add(pC_IndentDMO);

                    foreach (var c in data.PC_Indent_DetailsDTO)
                    {
                        PC_Indent_DetailsDMO pC_Indent_DetailsDMO = new PC_Indent_DetailsDMO();
                        pC_Indent_DetailsDMO.PCINDENT_Id = pC_IndentDMO.PCINDENT_Id;
                        pC_Indent_DetailsDMO.PCMPART_Id = c.PCMPART_Id;
                        pC_Indent_DetailsDMO.PCREQTN_Id = c.PCREQTN_Id;
                        pC_Indent_DetailsDMO.PCINDENTDET_RequestedAmount = c.PCINDENTDET_Amount;
                        pC_Indent_DetailsDMO.PCINDENTDET_Remarks = c.PCINDENTDET_Remarks;
                        pC_Indent_DetailsDMO.PCINDENTDET_SanctionedAmt = c.PCINDENTDET_ApprovedAmt;
                        pC_Indent_DetailsDMO.PCINDENTDET_ActiveFlg = true;
                        pC_Indent_DetailsDMO.PCINDENTDET_CreatedDate = indiantime0;
                        pC_Indent_DetailsDMO.PCINDENTDET_UpdatedDate = indiantime0;
                        pC_Indent_DetailsDMO.PCINDENTDET_CreatedBy = data.Userid;
                        pC_Indent_DetailsDMO.PCINDENTDET_UpdatedBy = data.Userid;

                        _context.Add(pC_Indent_DetailsDMO);
                    }

                    var i = _context.SaveChanges();

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
        public PC_IndentDTO EditData(PC_IndentDTO data)
        {
            try
            {
                data.geteditdata = _context.PC_IndentDMO.Where(a => a.PCINDENT_Id == data.PCINDENT_Id && a.MI_Id == data.MI_Id).ToArray();

                data.geteditparticularsdata = (from a in _context.PC_IndentDMO
                                               from b in _context.PC_Indent_DetailsDMO
                                               from c in _context.PC_Master_ParticularsDMO
                                               where (a.PCINDENT_Id == b.PCINDENT_Id && b.PCMPART_Id == c.PCMPART_Id && b.PCINDENT_Id == data.PCINDENT_Id
                                                && a.MI_Id == data.MI_Id)
                                               select new PC_IndentDTO
                                               {
                                                   PCINDENTDET_Id = b.PCINDENTDET_Id,
                                                   PCMPART_Id = b.PCMPART_Id,
                                                   PCINDENT_Id = b.PCINDENT_Id,
                                                   PCMPART_ParticularName = c.PCMPART_ParticularName,
                                                   PCINDENTDET_Amount = b.PCINDENTDET_RequestedAmount,
                                                   PCINDENTDET_ApprovedAmt = b.PCINDENTDET_SanctionedAmt,
                                                   PCINDENTDET_ActiveFlg = b.PCINDENTDET_ActiveFlg,
                                                   PCINDENTDET_Remarks = b.PCINDENTDET_Remarks,
                                                   PCINDENT_IndentNo = a.PCINDENT_IndentNo

                                               }).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public PC_IndentDTO deactiveY(PC_IndentDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

                var getresult = _context.PC_IndentDMO.Where(a => a.PCINDENT_Id == data.PCINDENT_Id && a.MI_Id == data.MI_Id).ToList();

                if (getresult.Count > 0)
                {
                    var checkresult = _context.PC_IndentDMO.Single(a => a.PCINDENT_Id == data.PCINDENT_Id && a.MI_Id == data.MI_Id);
                    if (checkresult.PCINDENT_ActiveFlg == true)
                    {
                        checkresult.PCINDENT_ActiveFlg = false;
                    }
                    else
                    {
                        checkresult.PCINDENT_ActiveFlg = true;
                    }
                    checkresult.PCINDENT_UpdatedDate = indiantime0;
                    checkresult.PCINDENT_UpdatedBy = data.Userid;

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
        public PC_IndentDTO Viewdata(PC_IndentDTO data)
        {
            try
            {
                data.getviewdata = (from a in _context.PC_IndentDMO
                                    from b in _context.PC_Indent_DetailsDMO
                                    from c in _context.PC_Master_ParticularsDMO
                                    where (a.PCINDENT_Id == b.PCINDENT_Id && b.PCMPART_Id == c.PCMPART_Id && b.PCINDENT_Id == data.PCINDENT_Id
                                     && a.MI_Id == data.MI_Id)
                                    select new PC_IndentDTO
                                    {
                                        PCINDENTDET_Id = b.PCINDENTDET_Id,
                                        PCMPART_Id = b.PCMPART_Id,
                                        PCINDENT_Id = b.PCINDENT_Id,
                                        PCMPART_ParticularName = c.PCMPART_ParticularName,
                                        PCINDENTDET_Amount = b.PCINDENTDET_RequestedAmount,
                                        PCINDENTDET_ApprovedAmt = b.PCINDENTDET_SanctionedAmt,
                                        PCINDENTDET_ActiveFlg = b.PCINDENTDET_ActiveFlg,
                                        PCINDENTDET_Remarks = b.PCINDENTDET_Remarks,

                                    }).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public PC_IndentDTO deactiveparticulars(PC_IndentDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

                var getresult = _context.PC_Indent_DetailsDMO.Where(a => a.PCINDENTDET_Id == data.PCINDENTDET_Id).ToList();

                if (getresult.Count > 0)
                {
                    var checkresult = _context.PC_Indent_DetailsDMO.Single(a => a.PCINDENTDET_Id == data.PCINDENTDET_Id);
                    if (checkresult.PCINDENTDET_ActiveFlg == true)
                    {
                        checkresult.PCINDENTDET_ActiveFlg = false;
                    }
                    else
                    {
                        checkresult.PCINDENTDET_ActiveFlg = true;
                    }
                    checkresult.PCINDENTDET_UpdatedDate = indiantime0;
                    checkresult.PCINDENTDET_UpdatedBy = data.Userid;

                    _context.Update(checkresult);
                    var i = _context.SaveChanges();
                    if (i > 0)
                    {
                        decimal? totamount = 0;
                        var updateamount = _context.PC_Indent_DetailsDMO.Where(a => a.PCINDENT_Id == data.PCINDENT_Id && a.PCINDENTDET_ActiveFlg == true).ToList();

                        if (updateamount.Count > 0)
                        {
                            foreach (var c in updateamount)
                            {
                                totamount += c.PCINDENTDET_SanctionedAmt;
                            }

                            var checkresult_main = _context.PC_IndentDMO.Single(a => a.PCINDENT_Id == data.PCINDENT_Id);
                            checkresult_main.PCINDENT_SanctionedAmt = totamount;
                            checkresult_main.PCINDENT_UpdatedBy = data.Userid;
                            checkresult_main.PCINDENT_UpdatedDate = indiantime0;

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

                data.getviewdata = (from a in _context.PC_IndentDMO
                                    from b in _context.PC_Indent_DetailsDMO
                                    from c in _context.PC_Master_ParticularsDMO
                                    where (a.PCINDENT_Id == b.PCINDENT_Id && b.PCMPART_Id == c.PCMPART_Id && b.PCINDENT_Id == data.PCINDENT_Id && a.MI_Id == data.MI_Id)
                                    select new PC_IndentDTO
                                    {
                                        PCINDENTDET_Id = b.PCINDENTDET_Id,
                                        PCMPART_Id = b.PCMPART_Id,
                                        PCINDENT_Id = b.PCINDENT_Id,
                                        PCMPART_ParticularName = c.PCMPART_ParticularName,
                                        PCINDENTDET_Amount = b.PCINDENTDET_RequestedAmount,
                                        PCINDENTDET_ApprovedAmt = b.PCINDENTDET_SanctionedAmt,
                                        PCINDENTDET_ActiveFlg = b.PCINDENTDET_ActiveFlg,
                                        PCINDENTDET_Remarks = b.PCINDENTDET_Remarks,

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
