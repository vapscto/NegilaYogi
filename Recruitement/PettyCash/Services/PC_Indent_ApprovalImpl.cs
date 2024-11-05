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
    public class PC_Indent_ApprovalImpl : Interface.PC_Indent_ApprovalInterface
    {
        public PettyCashContext _context;
        public DomainModelMsSqlServerContext _Contextdb;
        public PC_Indent_ApprovalImpl(PettyCashContext _con, DomainModelMsSqlServerContext _Contextdbsql)
        {
            _context = _con;
            _Contextdb = _Contextdbsql;
        }

        public PC_Indent_ApprovalDTO onloaddata(PC_Indent_ApprovalDTO data)
        {
            try
            {
                var getuserinstitution = _context.UserRoleWithInstituteDMO.Where(a => a.Id == data.Userid).ToList();

                List<long> miids = new List<long>();

                foreach (var c in getuserinstitution)
                {
                    miids.Add(c.MI_Id);
                }

                data.getuserinstitution = _context.Institution.Where(a => a.MI_ActiveFlag == 1 && miids.Contains(a.MI_Id)).ToArray();

                data.getloaddata = (from a in _context.PC_Indent_ApprovedDMO
                                    from b in _context.MasterEmployee
                                    from c in _context.HR_Master_Department
                                    where (a.HRME_Id == b.HRME_Id && b.HRMD_Id == c.HRMD_Id && a.MI_Id == data.MI_Id)
                                    select new PC_Indent_ApprovalDTO
                                    {
                                        departmentname = c.HRMD_DepartmentName,
                                        employeename = ((b.HRME_EmployeeFirstName == null || b.HRME_EmployeeFirstName == "" ? "" : " " + b.HRME_EmployeeFirstName) + (b.HRME_EmployeeMiddleName == null || b.HRME_EmployeeMiddleName == "" || b.HRME_EmployeeMiddleName == "0" ? "" : " " + b.HRME_EmployeeMiddleName) + (b.HRME_EmployeeLastName == null || b.HRME_EmployeeLastName == ""
                                        || b.HRME_EmployeeLastName == "0" ? "" : " " + b.HRME_EmployeeLastName) + (b.HRME_EmployeeCode == null
                                        || b.HRME_EmployeeCode == "" ? "" : " : " + b.HRME_EmployeeCode)).Trim(),
                                        PCINDENTAPDT_ActiveFlg = a.PCINDENTAPT_ActiveFlg,
                                        PCINDENTAPT_Date = a.PCINDENTAPT_Date,
                                        PCINDENTAPT_Desc = a.PCINDENTAPT_Desc,
                                        PCINDENTAP_Id = a.PCINDENTAP_Id,
                                        PCINDENTAPT_IndentNo = a.PCINDENTAPT_IndentNo,
                                        PCINDENTAPT_SanctionedAmt = a.PCINDENTAPT_SanctionedAmt,
                                        createdate = a.PCINDENTAPT_CreatedDate
                                    }).OrderByDescending(a => a.createdate).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public PC_Indent_ApprovalDTO OnChangeInstitution(PC_Indent_ApprovalDTO data)
        {
            try
            {
                data.getloaddata = (from a in _context.PC_Indent_ApprovedDMO
                                    from b in _context.MasterEmployee
                                    from c in _context.HR_Master_Department
                                    where (a.HRME_Id == b.HRME_Id && b.HRMD_Id == c.HRMD_Id && a.MI_Id == data.MI_Id)
                                    select new PC_Indent_ApprovalDTO
                                    {
                                        departmentname = c.HRMD_DepartmentName,
                                        employeename = ((b.HRME_EmployeeFirstName == null || b.HRME_EmployeeFirstName == "" ? "" : " " + b.HRME_EmployeeFirstName) + (b.HRME_EmployeeMiddleName == null || b.HRME_EmployeeMiddleName == "" || b.HRME_EmployeeMiddleName == "0" ? "" : " " + b.HRME_EmployeeMiddleName) + (b.HRME_EmployeeLastName == null || b.HRME_EmployeeLastName == ""
                                        || b.HRME_EmployeeLastName == "0" ? "" : " " + b.HRME_EmployeeLastName) + (b.HRME_EmployeeCode == null
                                        || b.HRME_EmployeeCode == "" ? "" : " : " + b.HRME_EmployeeCode)).Trim(),
                                        PCINDENTAPDT_ActiveFlg = a.PCINDENTAPT_ActiveFlg,
                                        PCINDENTAPT_Date = a.PCINDENTAPT_Date,
                                        PCINDENTAPT_Desc = a.PCINDENTAPT_Desc,
                                        PCINDENTAP_Id = a.PCINDENTAP_Id,
                                        PCINDENTAPT_IndentNo = a.PCINDENTAPT_IndentNo,
                                        PCINDENTAPT_SanctionedAmt = a.PCINDENTAPT_SanctionedAmt,
                                        createdate = a.PCINDENTAPT_CreatedDate
                                    }).OrderByDescending(a => a.createdate).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public PC_Indent_ApprovalDTO onchangedate(PC_Indent_ApprovalDTO data)
        {
            try
            {
                List<string> indentno = new List<string>();
                List<long> indentid = new List<long>();

                var getsaveindent = _context.PC_Indent_ApprovedDMO.Where(a=>a.MI_Id==data.MI_Id).ToList();

                if (getsaveindent.Count > 0)
                {
                    foreach (var c in getsaveindent)
                    {
                        indentid.Add(c.PCINDENT_Id);
                    }
                }


                data.indentdetails = (from a in _context.PC_IndentDMO
                                      from b in _context.MasterEmployee
                                      from c in _context.HR_Master_Department
                                      where (a.HRME_Id == b.HRME_Id && b.HRMD_Id == c.HRMD_Id && a.MI_Id == data.MI_Id
                                      && (a.PCINDENT_Date >= data.PCINDENT_Date_From && a.PCINDENT_Date <= data.PCINDENT_Date_To) && a.PCINDENT_ActiveFlg == true
                                      && !indentid.Contains(a.PCINDENT_Id))
                                      select new PC_Indent_ApprovalDTO
                                      {
                                          departmentname = c.HRMD_DepartmentName,
                                          employeename = ((b.HRME_EmployeeFirstName == null || b.HRME_EmployeeFirstName == "" ? "" : " " + b.HRME_EmployeeFirstName) + (b.HRME_EmployeeMiddleName == null || b.HRME_EmployeeMiddleName == "" || b.HRME_EmployeeMiddleName == "0" ? "" : " " + b.HRME_EmployeeMiddleName) + (b.HRME_EmployeeLastName == null || b.HRME_EmployeeLastName == ""
                                          || b.HRME_EmployeeLastName == "0" ? "" : " " + b.HRME_EmployeeLastName) + (b.HRME_EmployeeCode == null
                                          || b.HRME_EmployeeCode == "" ? "" : " : " + b.HRME_EmployeeCode)).Trim(),
                                          PCINDENT_Date = a.PCINDENT_Date,
                                          PCINDENT_Desc = a.PCINDENT_Desc,
                                          PCINDENT_Id = a.PCINDENT_Id,
                                          PCINDENT_IndentNo = a.PCINDENT_IndentNo,
                                          PCINDENT_RequestedAmount = a.PCINDENT_RequestedAmount,
                                          PCINDENT_ApprovedAmt = a.PCINDENT_SanctionedAmt,
                                          createdate = a.PCINDENT_CreatedDate

                                      }).OrderByDescending(a => a.createdate).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public PC_Indent_ApprovalDTO getindentdetails(PC_Indent_ApprovalDTO data)
        {
            try
            {
                List<long> ids = new List<long>();

                foreach (var c in data.temp_indentid)
                {
                    ids.Add(c.PCINDENT_Id);
                }

                data.indentparticulardetails = (from a in _context.PC_IndentDMO
                                                from b in _context.PC_Indent_DetailsDMO
                                                from c in _context.PC_Master_ParticularsDMO
                                                where (a.PCINDENT_Id == b.PCINDENT_Id && b.PCMPART_Id == c.PCMPART_Id && ids.Contains(b.PCINDENT_Id)
                                                && a.PCINDENT_ActiveFlg == true && b.PCINDENTDET_ActiveFlg == true && a.MI_Id == data.MI_Id)
                                                select new PC_Indent_ApprovalDTO
                                                {
                                                    pcindentdeT_Id = b.PCINDENTDET_Id,
                                                    PCMPART_Id = b.PCMPART_Id,
                                                    PCINDENT_Id = b.PCINDENT_Id,
                                                    PCINDENT_IndentNo = a.PCINDENT_IndentNo,
                                                    PCMPART_ParticularName = c.PCMPART_ParticularName,
                                                    PCINDENTDET_Amount = b.PCINDENTDET_RequestedAmount,
                                                    PCINDENTDET_ApprovedAmt = b.PCINDENTDET_SanctionedAmt,
                                                    PCINDENTDET_Remarks = b.PCINDENTDET_Remarks,
                                                    PCINDENTDET_ActiveFlg = b.PCINDENTDET_ActiveFlg,
                                                }).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public PC_Indent_ApprovalDTO saverecord(PC_Indent_ApprovalDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

                data.HRME_Id = _context.Staff_User_Login.Where(a => a.Id == data.Userid).Select(a => a.Emp_Code).FirstOrDefault();

                if (data.PCINDENTAP_Id > 0)
                {

                }
                else
                {
                    PC_Indent_ApprovedDMO pC_Indent_ApprovedDMO = new PC_Indent_ApprovedDMO();
                    pC_Indent_ApprovedDMO.MI_Id = data.MI_Id;
                    pC_Indent_ApprovedDMO.HRME_Id = data.HRME_Id;
                    pC_Indent_ApprovedDMO.PCINDENTAPT_IndentNo = data.PCINDENTAPT_IndentNo;
                    pC_Indent_ApprovedDMO.PCINDENT_Id = data.PCINDENT_Id;
                    pC_Indent_ApprovedDMO.PCINDENTAPT_Date = data.PCINDENTAPT_Date;
                    pC_Indent_ApprovedDMO.PCINDENTAPT_Desc = data.PCINDENTAPT_Desc;
                    pC_Indent_ApprovedDMO.PCINDENTAPT_RequestedAmount = data.PCINDENTAPT_RequestedAmount;
                    pC_Indent_ApprovedDMO.PCINDENTAPT_SanctionedAmt = data.PCINDENTAPT_SanctionedAmt;
                    pC_Indent_ApprovedDMO.PCINDENTAPT_ActiveFlg = true;
                    pC_Indent_ApprovedDMO.PCINDENTAPT_CreatedDate = indiantime0;
                    pC_Indent_ApprovedDMO.PCINDENTAPT_UpdatedDate = indiantime0;
                    pC_Indent_ApprovedDMO.PCINDENTAPT_CreatedBy = data.Userid;
                    pC_Indent_ApprovedDMO.PCINDENTAPT_UpdatedBy = data.Userid;

                    _context.Add(pC_Indent_ApprovedDMO);

                    if (data.PC_Indent_Approved_Details_DTO.Length > 0)
                    {
                        foreach (var c in data.PC_Indent_Approved_Details_DTO)
                        {
                            PC_Indent_Approved_DetailsDMO pC_Indent_Approved_DetailsDMO = new PC_Indent_Approved_DetailsDMO();
                            pC_Indent_Approved_DetailsDMO.PCINDENTAP_Id = pC_Indent_ApprovedDMO.PCINDENTAP_Id;
                            pC_Indent_Approved_DetailsDMO.PCMPART_Id = c.PCMPART_Id;
                            pC_Indent_Approved_DetailsDMO.PCINDENTAPDT_RequestedAmount = c.PCINDENTAPDT_RequestedAmount;
                            pC_Indent_Approved_DetailsDMO.PCINDENTAPDT_Remarks = c.PCINDENTAPDT_Remarks;
                            pC_Indent_Approved_DetailsDMO.PCINDENTAPDT_SanctionedAmt = c.PCINDENTAPDT_SanctionedAmt;
                            pC_Indent_Approved_DetailsDMO.PCINDENTAPDT_ActiveFlg = true;

                            pC_Indent_Approved_DetailsDMO.PCINDENTAPDT_CreatedDate = indiantime0;
                            pC_Indent_Approved_DetailsDMO.PCINDENTAPDT_UpdatedDate = indiantime0;
                            pC_Indent_Approved_DetailsDMO.PCINDENTAPDT_CreatedBy = data.Userid;
                            pC_Indent_Approved_DetailsDMO.PCINDENTAPDT_UpdatedBy = data.Userid;

                            _context.Add(pC_Indent_Approved_DetailsDMO);

                        }
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
        public PC_Indent_ApprovalDTO Viewdata(PC_Indent_ApprovalDTO data)
        {
            try
            {
                data.getviewdata = (from a in _context.PC_Indent_ApprovedDMO
                                    from b in _context.PC_Indent_Approved_DetailsDMO
                                    from c in _context.PC_Master_ParticularsDMO
                                    where (a.PCINDENTAP_Id == b.PCINDENTAP_Id && b.PCMPART_Id == c.PCMPART_Id && b.PCINDENTAP_Id == data.PCINDENTAP_Id
                                    && a.MI_Id == data.MI_Id)
                                    select new PC_Indent_ApprovalDTO
                                    {
                                        PCINDENTAPDT_Id = b.PCINDENTAPDT_Id,
                                        PCMPART_Id = b.PCMPART_Id,
                                        PCINDENTAP_Id = b.PCINDENTAP_Id,
                                        PCMPART_ParticularName = c.PCMPART_ParticularName,
                                        PCINDENTAPDT_RequestedAmount = b.PCINDENTAPDT_RequestedAmount,
                                        PCINDENTAPT_SanctionedAmt = b.PCINDENTAPDT_SanctionedAmt,
                                        PCINDENTAPDT_ActiveFlg = b.PCINDENTAPDT_ActiveFlg,
                                        PCINDENTAPDT_Remarks = b.PCINDENTAPDT_Remarks,

                                    }).Distinct().ToArray();

                data.getinstitutiondetails = _context.Institution.Where(a => a.MI_Id == data.MI_Id).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        // Expenditure
        public PC_Indent_ApprovalDTO ExpenditureLoaddata(PC_Indent_ApprovalDTO data)
        {
            try
            {
                data.getindentapprovaldetails = (from a in _context.PC_Indent_ApprovedDMO
                                                 from b in _context.PC_IndentDMO
                                                 where (a.PCINDENT_Id == b.PCINDENT_Id && a.MI_Id == data.MI_Id && b.MI_Id == data.MI_Id
                                                 && a.PCINDENTAPT_ActiveFlg == true && b.PCINDENT_ActiveFlg == true)
                                                 select new PC_Indent_ApprovalDTO
                                                 {
                                                     PCINDENTAP_Id = a.PCINDENTAP_Id,
                                                     PCINDENT_IndentNo = b.PCINDENT_IndentNo,
                                                     createdate = a.PCINDENTAPT_CreatedDate
                                                 }).Distinct().OrderByDescending(a => a.createdate).ToArray();

                data.getparticularsdetails = _context.PC_Master_ParticularsDMO.Where(a => a.MI_Id == data.MI_Id && a.PCMPART_ActiveFlg == true).ToArray();

                var getuserinstitution = _context.UserRoleWithInstituteDMO.Where(a => a.Id == data.Userid).ToList();

                List<long> miids = new List<long>();

                foreach (var c in getuserinstitution)
                {
                    miids.Add(c.MI_Id);
                }

                data.getuserinstitution = _context.Institution.Where(a => a.MI_ActiveFlag == 1 && miids.Contains(a.MI_Id)).ToArray();

                data.getexpenditureloaddata = (from a in _context.PC_ExpenditureDMO
                                               from b in _context.PC_Master_ParticularsDMO
                                               from c in _context.MasterEmployee
                                               where (a.PCMPART_Id == b.PCMPART_Id && a.HRME_Id==c.HRME_Id && a.MI_Id == data.MI_Id && a.PCEXPTR_ActiveFlg == true)
                                               select new PC_Indent_ApprovalDTO
                                               {
                                                   PCMPART_ParticularName = b.PCMPART_ParticularName,
                                                   PCEXPTR_Amount = a.PCEXPTR_Amount,
                                                   PCEXPTR_ExpenditureNo = a.PCEXPTR_ExpenditureNo,
                                                   PCEXPTR_Date = a.PCEXPTR_Date,
                                                   PCEXPTR_ModeOfPayment = a.PCEXPTR_ModeOfPayment,
                                                   PCEXPTR_ReferenceNo = a.PCEXPTR_ReferenceNo,
                                                   PCEXPTR_Id = a.PCEXPTR_Id,
                                                   PCEXPTR_Desc = a.PCEXPTR_Desc,
                                                   PCEXPTR_ActiveFlg = a.PCEXPTR_ActiveFlg,
                                                   employeename = ((c.HRME_EmployeeFirstName == null || c.HRME_EmployeeFirstName == "" ? "" : " "
                                                   + c.HRME_EmployeeFirstName) + (c.HRME_EmployeeMiddleName == null || c.HRME_EmployeeMiddleName == ""
                                                   || c.HRME_EmployeeMiddleName == "0" ? "" : " " + c.HRME_EmployeeMiddleName) + (c.HRME_EmployeeLastName == null
                                                   || c.HRME_EmployeeLastName == "" || c.HRME_EmployeeLastName == "0" ? "" : " " + c.HRME_EmployeeLastName)
                                                   + (c.HRME_EmployeeCode == null || c.HRME_EmployeeCode == "" ? "" : " : " + c.HRME_EmployeeCode)).Trim(),
                                               }).Distinct().ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public PC_Indent_ApprovalDTO OnChangeExpenditureInstitution(PC_Indent_ApprovalDTO data)
        {
            try
            {
                data.getindentapprovaldetails = (from a in _context.PC_Indent_ApprovedDMO
                                                 from b in _context.PC_IndentDMO
                                                 where (a.PCINDENT_Id == b.PCINDENT_Id && a.MI_Id == data.MI_Id && b.MI_Id == data.MI_Id
                                                 && a.PCINDENTAPT_ActiveFlg == true && b.PCINDENT_ActiveFlg == true)
                                                 select new PC_Indent_ApprovalDTO
                                                 {
                                                     PCINDENTAP_Id = a.PCINDENTAP_Id,
                                                     PCINDENT_IndentNo = b.PCINDENT_IndentNo,
                                                     createdate = a.PCINDENTAPT_CreatedDate
                                                 }).Distinct().OrderByDescending(a => a.createdate).ToArray();

                data.getparticularsdetails = _context.PC_Master_ParticularsDMO.Where(a => a.MI_Id == data.MI_Id && a.PCMPART_ActiveFlg == true).ToArray();

                data.getexpenditureloaddata = (from a in _context.PC_ExpenditureDMO
                                               from b in _context.PC_Master_ParticularsDMO
                                               from c in _context.MasterEmployee
                                               where (a.PCMPART_Id == b.PCMPART_Id && a.HRME_Id == c.HRME_Id && a.MI_Id == data.MI_Id && a.PCEXPTR_ActiveFlg == true)
                                               select new PC_Indent_ApprovalDTO
                                               {
                                                   PCMPART_ParticularName = b.PCMPART_ParticularName,
                                                   PCEXPTR_Amount = a.PCEXPTR_Amount,
                                                   PCEXPTR_ExpenditureNo = a.PCEXPTR_ExpenditureNo,
                                                   PCEXPTR_Date = a.PCEXPTR_Date,
                                                   PCEXPTR_ModeOfPayment = a.PCEXPTR_ModeOfPayment,
                                                   PCEXPTR_ReferenceNo = a.PCEXPTR_ReferenceNo,
                                                   PCEXPTR_Id = a.PCEXPTR_Id,
                                                   PCEXPTR_Desc = a.PCEXPTR_Desc,
                                                   PCEXPTR_ActiveFlg = a.PCEXPTR_ActiveFlg,
                                                   employeename = ((c.HRME_EmployeeFirstName == null || c.HRME_EmployeeFirstName == "" ? "" : " "
                                                   + c.HRME_EmployeeFirstName) + (c.HRME_EmployeeMiddleName == null || c.HRME_EmployeeMiddleName == ""
                                                   || c.HRME_EmployeeMiddleName == "0" ? "" : " " + c.HRME_EmployeeMiddleName) + (c.HRME_EmployeeLastName == null
                                                   || c.HRME_EmployeeLastName == "" || c.HRME_EmployeeLastName == "0" ? "" : " " + c.HRME_EmployeeLastName)
                                                   + (c.HRME_EmployeeCode == null || c.HRME_EmployeeCode == "" ? "" : " : " + c.HRME_EmployeeCode)).Trim(),
                                               }).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public PC_Indent_ApprovalDTO OnChangeExpenditureIndent(PC_Indent_ApprovalDTO data)
        {
            try
            {
                data.getparticularsdetails = (from a in _context.PC_Indent_ApprovedDMO
                                              from b in _context.PC_Indent_Approved_DetailsDMO
                                              from c in _context.PC_Master_ParticularsDMO
                                              where (a.PCINDENTAP_Id == b.PCINDENTAP_Id && b.PCMPART_Id == c.PCMPART_Id && a.PCINDENTAP_Id == data.PCINDENTAP_Id
                                              && b.PCINDENTAP_Id == data.PCINDENTAP_Id && a.MI_Id == data.MI_Id && c.MI_Id == data.MI_Id
                                              && b.PCINDENTAPDT_ActiveFlg == true)
                                              select c).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public PC_Indent_ApprovalDTO OnChangeExpenditureParticular(PC_Indent_ApprovalDTO data)
        {
            try
            {
                data.getparticularsindentdetails = (from a in _context.PC_Indent_ApprovedDMO
                                                    from b in _context.PC_Indent_Approved_DetailsDMO
                                                    from c in _context.PC_Master_ParticularsDMO
                                                    where (a.PCINDENTAP_Id == b.PCINDENTAP_Id && b.PCMPART_Id == c.PCMPART_Id && a.PCINDENTAP_Id == data.PCINDENTAP_Id
                                                    && b.PCINDENTAP_Id == data.PCINDENTAP_Id && a.MI_Id == data.MI_Id && c.MI_Id == data.MI_Id
                                                    && b.PCINDENTAPDT_ActiveFlg == true && b.PCMPART_Id == data.PCMPART_Id)
                                                    select new PC_Indent_ApprovalDTO
                                                    {
                                                        PCINDENTAPDT_RequestedAmount = b.PCINDENTAPDT_RequestedAmount,
                                                        PCINDENTAPDT_SanctionedAmt = b.PCINDENTAPDT_SanctionedAmt,
                                                        PCINDENTAPDT_AmountSpent = b.PCINDENTAPDT_AmountSpent,
                                                        PCINDENTAPDT_BalanceAmount = b.PCINDENTAPDT_BalanceAmount,
                                                        PCMPART_ParticularName = c.PCMPART_ParticularName,
                                                        PCMPART_Id = c.PCMPART_Id,
                                                        PCINDENTAPDT_Id = b.PCINDENTAPDT_Id,
                                                    }).Distinct().ToArray();

                data.getindentdetails = (from a in _context.PC_Indent_ApprovedDMO
                                         from b in _context.PC_IndentDMO
                                         where (a.PCINDENT_Id == b.PCINDENT_Id && a.PCINDENTAP_Id == data.PCINDENTAP_Id && a.MI_Id == data.MI_Id)
                                         select new PC_Indent_ApprovalDTO
                                         {
                                             PCINDENT_IndentNo = b.PCINDENT_IndentNo,
                                             PCINDENTAPT_RequestedAmount = a.PCINDENTAPT_RequestedAmount,
                                             PCINDENTAPT_SanctionedAmt = a.PCINDENTAPT_SanctionedAmt,
                                             PCINDENTAPT_AmountSpent = a.PCINDENTAPT_AmountSpent,
                                             PCINDENTAPT_BalanceAmount = a.PCINDENTAPT_BalanceAmount,

                                         }).Distinct().ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public PC_Indent_ApprovalDTO SaveExpenditure(PC_Indent_ApprovalDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

                data.HRME_Id = _context.Staff_User_Login.Where(a => a.Id == data.Userid).Select(a => a.Emp_Code).FirstOrDefault();

                var acd_Id = _context.IVRM_Master_FinancialYear.Where(t => Convert.ToDateTime(t.IMFY_FromDate) <= Convert.ToDateTime(indiantime0.Date) && Convert.ToDateTime(t.IMFY_ToDate) >= Convert.ToDateTime(indiantime0.Date)).Select(d => d.IMFY_Id).FirstOrDefault();

                if (data.PCEXPTR_Id > 0)
                {
                    data.saveorupdate = "Update";

                }
                else
                {
                    PettyCash_AutoRefernceNo_Generation pettyCash_AutoRefernceNo_Generation = new PettyCash_AutoRefernceNo_Generation(_context);
                    var refernceno = pettyCash_AutoRefernceNo_Generation.GenerateReferenceNo(data.MI_Id, acd_Id, "Expenditure");

                    if (refernceno == "" || refernceno == "Error")
                    {
                        data.saveorupdate = "ReferenceNotGenerated";
                        return data;
                    }

                    var count = 0;
                    if (data.PCINDENTAP_Id > 0)
                    {
                        var updatedcount = _context.Database.ExecuteSqlCommand("PC_Expenditure_Update @p0,@p1,@p2,@p3,@p4", data.MI_Id, data.PCINDENTAP_Id, data.PCMPART_Id, data.PCEXPTR_Amount, data.Userid);

                        if (updatedcount > 0)
                        {
                            count = 1;
                        }
                        else
                        {
                            count = 0;
                        }
                    }
                    else
                    {
                        count = 1;
                    }

                    if (count > 0)
                    {
                        data.saveorupdate = "Add";
                        PC_ExpenditureDMO pC_ExpenditureDMO = new PC_ExpenditureDMO();
                        pC_ExpenditureDMO.MI_Id = data.MI_Id;
                        pC_ExpenditureDMO.HRME_Id = data.HRME_Id;
                        pC_ExpenditureDMO.PCMPART_Id = data.PCMPART_Id;
                        pC_ExpenditureDMO.PCEXPTR_Amount = data.PCEXPTR_Amount;
                        pC_ExpenditureDMO.PCEXPTR_ExpenditureNo = refernceno;

                        if (data.PCINDENTAP_Id > 0)
                        {
                            pC_ExpenditureDMO.PCINDENTAP_Id = data.PCINDENTAP_Id;
                        }
                        pC_ExpenditureDMO.PCEXPTR_Date = data.PCEXPTR_Date;
                        pC_ExpenditureDMO.PCEXPTR_Desc = data.PCEXPTR_Desc;
                        pC_ExpenditureDMO.PCEXPTR_ModeOfPayment = data.PCEXPTR_ModeOfPayment;
                        pC_ExpenditureDMO.PCEXPTR_ReferenceNo = data.PCEXPTR_ReferenceNo;
                        pC_ExpenditureDMO.PCEXPTR_ActiveFlg = true;
                        pC_ExpenditureDMO.PCEXPTR_CreatedDate = indiantime0;
                        pC_ExpenditureDMO.PCEXPTR_UpdatedDate = indiantime0;
                        pC_ExpenditureDMO.PCEXPTR_CreatedBy = data.Userid;
                        pC_ExpenditureDMO.PCEXPTR_UpdatedBy = data.Userid;

                        _context.Add(pC_ExpenditureDMO);

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
            }
            catch (Exception ex)
            {
                data.saveorupdate = "Error";
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public PC_Indent_ApprovalDTO DeleteExpenditure(PC_Indent_ApprovalDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

                var checkresult = _context.PC_ExpenditureDMO.Where(a => a.MI_Id == data.MI_Id && a.PCEXPTR_Id == data.PCEXPTR_Id).ToList();

                if (checkresult.Count > 0)
                {
                    var count = 0;

                    if (checkresult.FirstOrDefault().PCINDENTAP_Id > 0)
                    {
                        //var updatedcount = _context.Database.ExecuteSqlCommand("PC_Expenditure_Update @p0,@p1,@p2,@p3,@p4", data.MI_Id, data.PCINDENTAP_Id, data.PCMPART_Id, data.PCEXPTR_Amount, data.Userid);

                        //if (updatedcount > 0)
                        //{
                        //    count = 1;
                        //}
                        //else
                        //{
                        //    count = 0;
                        //}
                    }
                    else
                    {
                        count = 1;
                    }

                    if (count > 0)
                    {
                        var result = _context.PC_ExpenditureDMO.Single(a => a.MI_Id == data.MI_Id && a.PCEXPTR_Id == data.PCEXPTR_Id);
                        result.PCEXPTR_ActiveFlg = false;
                        result.PCEXPTR_UpdatedBy = data.Userid;
                        result.PCEXPTR_UpdatedDate = indiantime0;
                        result.PCEXPTR_DeletedRemarks = data.PCEXPTR_DeletedRemarks;
                        _context.Update(result);

                        var i = _context.SaveChanges();
                        if (i > 0)
                        {
                            data.returnval = true;
                        }
                        else { data.returnval = false; }
                    }                    
                }

            }
            catch (Exception ex)
            {
                data.saveorupdate = "Error";
                Console.WriteLine(ex.Message);
            }
            return data;
        }
    }
}