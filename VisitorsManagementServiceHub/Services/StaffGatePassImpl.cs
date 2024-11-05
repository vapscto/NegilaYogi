using CommonLibrary;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.VisitorsManagement;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.VisitorsManagement;
using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.VisitorsManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VisitorsManagementServiceHub.Services
{
    public class StaffGatePassImpl : Interfaces.StaffGatePassInterface
    {
        DomainModelMsSqlServerContext _db;
        public VisitorsManagementContext _visctxt;
        public DomainModelMsSqlServerContext _context;

        public StaffGatePassImpl(VisitorsManagementContext spc, DomainModelMsSqlServerContext contxt, DomainModelMsSqlServerContext pad)
        {
            _visctxt = spc;
            _db = contxt;
            _context = pad;
        }

        public StaffGatePass_DTO Getdetails(StaffGatePass_DTO data)
        {
            try
            {
                List<MasterAcademic> list = new List<MasterAcademic>();
                list = _visctxt.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).Distinct().OrderByDescending(t => t.ASMAY_Order).ToList();
                data.yearlist = list.ToArray();

                data.filldepartment = _visctxt.HR_Master_Department.Where(a => a.MI_Id == data.MI_Id && a.HRMD_ActiveFlag == true).Distinct().OrderBy(e => e.HRMD_Order).ToArray();

                data.filldesignation = (from a in _visctxt.MasterEmployee
                                        from b in _visctxt.HR_Master_Designation
                                        where (a.MI_Id == data.MI_Id && a.HRME_ActiveFlag && a.HRMDES_Id == b.HRMDES_Id 
                                        && b.MI_Id.Equals(data.MI_Id) && b.HRMDES_ActiveFlag == true)
                                        select b).Distinct().ToArray();
                data.emplist = (from a in _visctxt.MasterEmployee
                                where (a.MI_Id == data.MI_Id && a.HRME_ActiveFlag == true && a.HRME_LeftFlag == false  )
                                select new StaffGatePass_DTO
                                {
                                    empname = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + " " + (a.HRME_EmployeeMiddleName == null ? " " : a.HRME_EmployeeMiddleName) + " " + (a.HRME_EmployeeLastName == null ? " " : a.HRME_EmployeeLastName)).Trim(),
                                    HRME_Id = a.HRME_Id,
                                    HRMDES_Id = a.HRMDES_Id,
                                    HRMD_Id = a.HRMD_Id,
                                }).Distinct().OrderBy(a => a.HRME_Id).ToArray();

                data.alldata = (from a in _visctxt.Gate_Pass_Staff_DMO
                                from b in _visctxt.MasterEmployee
                                from d in _visctxt.HR_Master_Department
                                from f in _visctxt.HR_Master_Designation
                                where (a.MI_Id == b.MI_Id && a.HRME_Id == b.HRME_Id && d.HRMD_Id == b.HRMD_Id && f.HRMDES_Id == b.HRMDES_Id
                                && b.HRME_ActiveFlag == true && b.HRME_LeftFlag == false && a.MI_Id == data.MI_Id)
                                select new StaffGatePass_DTO
                                {
                                    empname = b.HRME_EmployeeFirstName + (string.IsNullOrEmpty(b.HRME_EmployeeMiddleName) ? "" : ' ' + b.HRME_EmployeeMiddleName) + (string.IsNullOrEmpty(b.HRME_EmployeeLastName) ? "" : ' ' + b.HRME_EmployeeLastName),
                                    HRME_Id = a.HRME_Id,
                                    GPHST_Id = a.GPHST_Id,
                                    GPHST_GatePassNo = a.GPHST_GatePassNo,
                                    GPHST_IDCardNo = a.GPHST_IDCardNo,
                                    GPHST_DateTime = a.GPHST_DateTime,
                                    GPHST_Remarks = a.GPHST_Remarks,
                                    HRMD_DepartmentName = d.HRMD_DepartmentName,
                                    HRMDES_DesignationName = f.HRMDES_DesignationName,
                                    GPHST_ActiveFlg = a.GPHST_ActiveFlg,
                                    HRMD_Id = d.HRMD_Id,
                                    HRMDES_Id = f.HRMDES_Id,
                                    GPHST_InTime = a.GPHST_InTime,
                                    GPHST_OutTime = a.GPHST_OutTime

                                }).Distinct().OrderByDescending(a => a.GPHST_DateTime).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public StaffGatePass_DTO getdepchange(StaffGatePass_DTO data)
        {
            try
            {
                data.filldesignation = (from a in _visctxt.MasterEmployee
                                        from b in _visctxt.HR_Master_Designation
                                        where (a.MI_Id == data.MI_Id && a.HRME_ActiveFlag && a.HRMDES_Id == b.HRMDES_Id && data.HRMD_Id == a.HRMD_Id
                                        && b.MI_Id.Equals(data.MI_Id) && b.HRMDES_ActiveFlag == true)
                                        select b).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return data;
        }
        public StaffGatePass_DTO get_staff1(StaffGatePass_DTO data)
        {
            try
            {
                data.emplist = (from a in _visctxt.MasterEmployee
                                where (a.MI_Id == data.MI_Id && a.HRME_ActiveFlag == true && a.HRME_LeftFlag == false && a.HRMDES_Id == data.HRMDES_Id && a.HRMD_Id == data.HRMD_Id)
                                select new StaffGatePass_DTO
                                {
                                    empname = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + " " + (a.HRME_EmployeeMiddleName == null ? " " : a.HRME_EmployeeMiddleName) + " " + (a.HRME_EmployeeLastName == null ? " " : a.HRME_EmployeeLastName)).Trim(),
                                    HRME_Id = a.HRME_Id,
                                }).Distinct().OrderBy(a => a.HRME_Id).ToArray();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return data;
        }
        public StaffGatePass_DTO saverecord(StaffGatePass_DTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);

                Master_NumberingDTO check = new Master_NumberingDTO();
                data.transnumbconfigurationsettingsss = check;
                List<Master_Numbering> MM = new List<Master_Numbering>();
                MM = _context.Master_Numbering.Where(t => t.MI_Id == data.MI_Id && t.IMN_Flag == "StaffGatePass").ToList();

                if (MM.Count() > 0)
                {
                    data.transnumbconfigurationsettingsss.IMN_AutoManualFlag = MM.FirstOrDefault().IMN_AutoManualFlag;
                    data.transnumbconfigurationsettingsss.IMN_DuplicatesFlag = MM.FirstOrDefault().IMN_DuplicatesFlag;
                    data.transnumbconfigurationsettingsss.IMN_Flag = MM.FirstOrDefault().IMN_Flag;
                    data.transnumbconfigurationsettingsss.IMN_Id = MM.FirstOrDefault().IMN_Id;
                    data.transnumbconfigurationsettingsss.IMN_PrefixAcadYearCode = MM.FirstOrDefault().IMN_PrefixAcadYearCode;
                    data.transnumbconfigurationsettingsss.IMN_PrefixCalYearCode = MM.FirstOrDefault().IMN_PrefixCalYearCode;
                    data.transnumbconfigurationsettingsss.IMN_PrefixFinYearCode = MM.FirstOrDefault().IMN_PrefixFinYearCode;
                    data.transnumbconfigurationsettingsss.IMN_PrefixParticular = MM.FirstOrDefault().IMN_PrefixParticular;
                    data.transnumbconfigurationsettingsss.IMN_RestartNumFlag = MM.FirstOrDefault().IMN_RestartNumFlag;
                    data.transnumbconfigurationsettingsss.IMN_StartingNo = MM.FirstOrDefault().IMN_StartingNo;
                    data.transnumbconfigurationsettingsss.IMN_SuffixAcadYearCode = MM.FirstOrDefault().IMN_SuffixAcadYearCode;
                    data.transnumbconfigurationsettingsss.IMN_SuffixCalYearCode = MM.FirstOrDefault().IMN_SuffixCalYearCode;
                    data.transnumbconfigurationsettingsss.IMN_SuffixFinYearCode = MM.FirstOrDefault().IMN_SuffixFinYearCode;
                    data.transnumbconfigurationsettingsss.IMN_SuffixParticular = MM.FirstOrDefault().IMN_SuffixParticular;
                    data.transnumbconfigurationsettingsss.IMN_WidthNumeric = MM.FirstOrDefault().IMN_WidthNumeric;
                    data.transnumbconfigurationsettingsss.IMN_ZeroPrefixFlag = MM.FirstOrDefault().IMN_ZeroPrefixFlag;
                    data.transnumbconfigurationsettingsss.MI_Id = MM.FirstOrDefault().MI_Id;
                }

                if (data.GPHST_Id == 0)
                {
                    var Duplicate = _visctxt.Gate_Pass_Staff_DMO.Where(vm => vm.MI_Id == data.MI_Id && vm.GPHST_IDCardNo == data.GPHST_IDCardNo && vm.GPHST_DateTime == data.GPHST_DateTime && vm.GPHST_GatePassNo == data.GPHST_GatePassNo && vm.HRME_Id == data.HRME_Id).ToList();
                    if (Duplicate.Count > 0)
                    {
                        data.dulicate = true;
                    }
                    else
                    {
                        if (data.transnumbconfigurationsettingsss.IMN_AutoManualFlag == "Auto")
                        {
                            GenerateTransactionNumbering a = new GenerateTransactionNumbering(_context);
                            data.transnumbconfigurationsettingsss.MI_Id = data.MI_Id;
                            data.transnumbconfigurationsettingsss.ASMAY_Id = data.ASMAY_Id;
                            data.trans_id = a.GenerateNumber(data.transnumbconfigurationsettingsss);
                        }

                        Gate_Pass_Staff_DMO obj = new Gate_Pass_Staff_DMO();
                        obj.MI_Id = data.MI_Id;
                        obj.HRME_Id = data.HRME_Id;
                        obj.GPHST_GatePassNo = data.trans_id;
                        obj.GPHST_IDCardNo = data.GPHST_IDCardNo;
                        obj.GPHST_DateTime = indianTime;
                        obj.GPHST_Remarks = data.GPHST_Remarks;
                        obj.GPHST_InTime = data.GPHST_InTime;
                        obj.GPHST_OutTime = data.GPHST_OutTime;
                        obj.GPHST_ActiveFlg = true;
                        obj.CreatedDate = DateTime.Now;
                        obj.UpdatedDate = DateTime.Now;
                        obj.GPHST_CreatedBy = data.UserId;
                        obj.GPHST_UpdatedBy = data.UserId;

                        _visctxt.Add(obj);
                        int rowAffected = _visctxt.SaveChanges();

                        if (rowAffected > 0)
                        {
                            data.returnval = true;

                            data.GPHST_Id = obj.GPHST_Id;

                            var staff_gatepass = _visctxt.Gate_Pass_Staff_DMO.OrderByDescending(d => d.MI_Id == data.MI_Id && d.GPHST_Id == data.GPHST_Id).First();
                            var hrmid = staff_gatepass.HRME_Id;

                            data.currentstaffdata = (from a in _visctxt.Gate_Pass_Staff_DMO
                                                     from b in _visctxt.MasterEmployee
                                                     from d in _visctxt.HR_Master_Department
                                                     from e in _visctxt.HR_Master_Designation
                                                     where (a.HRME_Id == b.HRME_Id && d.HRMD_Id == b.HRMD_Id && e.HRMDES_Id == b.HRMDES_Id
                                                     && a.GPHST_Id == staff_gatepass.GPHST_Id)
                                                     select new StaffGatePass_DTO
                                                     {
                                                         GPHST_Id = a.GPHST_Id,
                                                         GPHST_DateTime = a.GPHST_DateTime,
                                                         GPHST_GatePassNo = a.GPHST_GatePassNo,
                                                         GPHST_IDCardNo = a.GPHST_IDCardNo,
                                                         GPHST_Remarks = a.GPHST_Remarks,
                                                         empname = b.HRME_EmployeeFirstName + (string.IsNullOrEmpty(b.HRME_EmployeeMiddleName) ? "" : ' ' + b.HRME_EmployeeMiddleName) + (string.IsNullOrEmpty(b.HRME_EmployeeLastName) ? "" : ' ' + b.HRME_EmployeeLastName),
                                                         HRME_MobileNo = b.HRME_MobileNo,
                                                         HRMD_DepartmentName = d.HRMD_DepartmentName,
                                                         HRMDES_DesignationName = e.HRMDES_DesignationName,
                                                         GPHST_InTime = a.GPHST_InTime,
                                                         GPHST_OutTime = a.GPHST_OutTime
                                                     }).ToArray();
                        }
                        else
                        {
                            data.returnval = false;
                        }
                    }
                }
                else if (data.GPHST_Id > 0)
                {
                    var Duplicate = _visctxt.Gate_Pass_Staff_DMO.Where(vm => vm.MI_Id == data.MI_Id && vm.GPHST_Id != data.GPHST_Id && vm.GPHST_IDCardNo == data.GPHST_IDCardNo && vm.GPHST_DateTime == data.GPHST_DateTime && vm.GPHST_GatePassNo == data.GPHST_GatePassNo && vm.HRME_Id == data.HRME_Id).ToList();
                    if (Duplicate.Count > 0)
                    {
                        data.dulicate = true;
                    }
                    else
                    {
                        var result = _visctxt.Gate_Pass_Staff_DMO.Single(t => t.MI_Id == data.MI_Id && t.GPHST_Id == data.GPHST_Id);
                        result.HRME_Id = data.HRME_Id;
                        result.GPHST_IDCardNo = data.GPHST_IDCardNo;
                        result.GPHST_DateTime = indianTime;
                        result.GPHST_Remarks = data.GPHST_Remarks;
                        result.UpdatedDate = DateTime.Now;
                        result.GPHST_UpdatedBy = data.UserId;
                        result.GPHST_InTime = data.GPHST_InTime;
                        result.GPHST_OutTime = data.GPHST_OutTime;
                        _visctxt.Update(result);

                        int rowAffected = _visctxt.SaveChanges();

                        if (rowAffected > 0)
                        {
                            data.returnval = true;

                            var staff_gatepass = _visctxt.Gate_Pass_Staff_DMO.OrderByDescending(d => d.GPHST_Id).First();
                            var hrmid = staff_gatepass.HRME_Id;
                            data.currentstaffdata = (from a in _visctxt.Gate_Pass_Staff_DMO
                                                     from b in _visctxt.MasterEmployee
                                                     from d in _visctxt.HR_Master_Department
                                                     from e in _visctxt.HR_Master_Designation
                                                     where (a.HRME_Id == b.HRME_Id && d.HRMD_Id == b.HRMD_Id && e.HRMDES_Id == b.HRMDES_Id
                                                     && a.GPHST_Id == staff_gatepass.GPHST_Id)
                                                     select new StaffGatePass_DTO
                                                     {
                                                         GPHST_Id = a.GPHST_Id,
                                                         GPHST_DateTime = a.GPHST_DateTime,
                                                         GPHST_GatePassNo = a.GPHST_GatePassNo,
                                                         GPHST_IDCardNo = a.GPHST_IDCardNo,
                                                         GPHST_Remarks = a.GPHST_Remarks,
                                                         empname = b.HRME_EmployeeFirstName + (string.IsNullOrEmpty(b.HRME_EmployeeMiddleName) ? "" : ' ' + b.HRME_EmployeeMiddleName) + (string.IsNullOrEmpty(b.HRME_EmployeeLastName) ? "" : ' ' + b.HRME_EmployeeLastName),
                                                         HRME_MobileNo = b.HRME_MobileNo,
                                                         HRMD_DepartmentName = d.HRMD_DepartmentName,
                                                         HRMDES_DesignationName = e.HRMDES_DesignationName,
                                                         GPHST_InTime = a.GPHST_InTime,
                                                         GPHST_OutTime = a.GPHST_OutTime
                                                     }).ToArray();
                        }
                        else
                        {
                            data.returnval = false;
                        }
                    }
                }

                data.institution = _db.Institution.Where(i => i.MI_Id == data.MI_Id).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public StaffGatePass_DTO deactive(StaffGatePass_DTO data)
        {
            try
            {
                var result = _visctxt.Gate_Pass_Staff_DMO.Single(t => t.MI_Id == data.MI_Id && t.GPHST_Id == data.GPHST_Id);

                if (result.GPHST_ActiveFlg == true)
                {
                    result.GPHST_ActiveFlg = false;
                }
                else if (result.GPHST_ActiveFlg == false)
                {
                    result.GPHST_ActiveFlg = true;
                }
                result.UpdatedDate = DateTime.Now;
                _visctxt.Update(result);
                int rowAffected = _visctxt.SaveChanges();
                if (rowAffected > 0)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public StaffGatePass_DTO editrecord(StaffGatePass_DTO data)
        {
            try
            {
                data.editlist = (from a in _visctxt.Gate_Pass_Staff_DMO
                                 from b in _visctxt.MasterEmployee
                                 from c in _visctxt.HR_Master_Department
                                 from d in _visctxt.HR_Master_Designation
                                 where (a.MI_Id == b.MI_Id && a.HRME_Id == b.HRME_Id && c.HRMD_Id == b.HRMD_Id && d.HRMDES_Id == b.HRMDES_Id
                                 && a.GPHST_Id == data.GPHST_Id)
                                 select new StaffGatePass_DTO
                                 {
                                     GPHST_Id = a.GPHST_Id,
                                     HRME_Id = a.HRME_Id,
                                     GPHST_GatePassNo = a.GPHST_GatePassNo,
                                     GPHST_IDCardNo = a.GPHST_IDCardNo,
                                     GPHST_DateTime = a.GPHST_DateTime,
                                     GPHST_Remarks = a.GPHST_Remarks,
                                     HRMD_Id = c.HRMD_Id,
                                     HRMDES_Id = d.HRMDES_Id,
                                     GPHST_InTime = a.GPHST_InTime,
                                     GPHST_OutTime = a.GPHST_OutTime,
                                     empname = ((b.HRME_EmployeeFirstName == null ? " " : b.HRME_EmployeeFirstName) + " " + (b.HRME_EmployeeMiddleName == null ? " " : b.HRME_EmployeeMiddleName) + " " + (b.HRME_EmployeeLastName == null ? " " : b.HRME_EmployeeLastName)).Trim(),
                                 }).Distinct().ToArray();

                data.emplist = (from a in _visctxt.MasterEmployee
                                where (a.MI_Id == data.MI_Id && a.HRME_ActiveFlag == true && a.HRME_LeftFlag == false && a.HRMDES_Id == data.HRMDES_Id
                                && a.HRMD_Id == data.HRMD_Id)
                                select new StaffGatePass_DTO
                                {
                                    empname = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + " " + (a.HRME_EmployeeMiddleName == null ? " " : a.HRME_EmployeeMiddleName) + " " + (a.HRME_EmployeeLastName == null ? " " : a.HRME_EmployeeLastName)).Trim(),
                                    HRME_Id = a.HRME_Id,
                                }).Distinct().OrderBy(a => a.HRME_Id).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public StaffGatePass_DTO PrintGatePass(StaffGatePass_DTO data)
        {
            try
            {
                data.currentstaffdata = (from a in _visctxt.Gate_Pass_Staff_DMO
                                         from b in _visctxt.MasterEmployee
                                         from d in _visctxt.HR_Master_Department
                                         from e in _visctxt.HR_Master_Designation
                                         where (a.HRME_Id == b.HRME_Id && d.HRMD_Id == b.HRMD_Id && e.HRMDES_Id == b.HRMDES_Id
                                         && a.GPHST_Id == data.GPHST_Id)
                                         select new StaffGatePass_DTO
                                         {
                                             GPHST_Id = a.GPHST_Id,
                                             GPHST_DateTime = a.GPHST_DateTime,
                                             GPHST_GatePassNo = a.GPHST_GatePassNo,
                                             GPHST_IDCardNo = a.GPHST_IDCardNo,
                                             GPHST_Remarks = a.GPHST_Remarks,
                                             empname = b.HRME_EmployeeFirstName + (string.IsNullOrEmpty(b.HRME_EmployeeMiddleName) ? "" : ' ' +
                                             b.HRME_EmployeeMiddleName) + (string.IsNullOrEmpty(b.HRME_EmployeeLastName) ? "" : ' ' + b.HRME_EmployeeLastName),
                                             HRME_MobileNo = b.HRME_MobileNo,
                                             HRMD_DepartmentName = d.HRMD_DepartmentName,
                                             HRMDES_DesignationName = e.HRMDES_DesignationName,
                                             GPHST_InTime = a.GPHST_InTime,
                                             GPHST_OutTime = a.GPHST_OutTime,
                                             HRME_Photo = b.HRME_Photo
                                         }).ToArray();

                data.institution = _db.Institution.Where(i => i.MI_Id == data.MI_Id).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
    }
}