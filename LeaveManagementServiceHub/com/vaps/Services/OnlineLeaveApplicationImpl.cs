using AutoMapper;
using DataAccessMsSqlServerProvider.com.vapstech.LeaveManagement;
using DomainModel.Model.com.vapstech.LeaveManagement;
using LeaveManagementServiceHub.com.vaps.Interfaces;
using PreadmissionDTOs.com.vaps.LeaveManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using DataAccessMsSqlServerProvider;
using PreadmissionDTOs;
using CommonLibrary;
using DomainModel.Model;
using System.Text.RegularExpressions;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using System.Net;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;
using SendGrid;
using Newtonsoft.Json.Linq;
using PreadmissionDTOs.com.vaps.TT;
using DataAccessMsSqlServerProvider.com.vapstech.TT;

namespace LeaveManagementServiceHub.com.vaps.Services
{
    public class OnlineLeaveApplicationImpl : OnlineLeaveApplicationInterface
    {
        public LMContext _lmContext;
        public DomainModelMsSqlServerContext _db;
        //new///////////////////////////////////////////////////////////////////////////////////////////
        public TTContext _ttcontext;

        public OnlineLeaveApplicationImpl(LMContext ttcategory, TTContext ttcntx, DomainModelMsSqlServerContext abc)
        {
            _lmContext = ttcategory;
            _ttcontext = ttcntx;

            _db = abc;
        }

        public LeaveCreditDTO getonlineLeave(LeaveCreditDTO data)
        {

            data.daylist = _ttcontext.TT_Master_DayDMO.Where(t => t.MI_Id.Equals(data.MI_Id) && t.TTMD_ActiveFlag == true).ToList().ToArray();
            data.classlist = _ttcontext.School_M_Class.Where(t => t.MI_Id == data.MI_Id && t.ASMCL_ActiveFlag == true).ToList().ToArray();
            long yearid = _lmContext.HR_Master_LeaveYear_DMO.Where(t => t.HRMLY_FromDate <= DateTime.Today && t.HRMLY_ToDate >= DateTime.Today && t.HRMLY_ActiveFlag == true && t.MI_Id == data.MI_Id).Select(t => t.HRMLY_Id).FirstOrDefault();
            List<MasterAcademic> allyear = new List<MasterAcademic>();
            allyear = _db.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).ToList();
           data.acayear= allyear.OrderByDescending(y => y.ASMAY_Order).Distinct().ToArray();

            List<MasterAcademic> defaultyear = new List<MasterAcademic>();
            defaultyear = _db.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true && t.ASMAY_Id == data.asmay_id).ToList();
            data.academicListdefault = defaultyear.OrderByDescending(a => a.ASMAY_Order).ToArray();

      

            var qyery1 = (from q in _db.Staff_User_Login
                          from r in _db.HR_Master_Employee_DMO
                          where (q.Emp_Code == r.HRME_Id && q.Id == data.UserId)
                          select new LeaveCreditDTO
                          {
                              HRME_Id = r.HRME_Id,
                          }).Distinct().ToArray();


            if (qyery1.Length > 0)
            {



                data.leave_name = (from x in _lmContext.HR_Master_Leave_DMO
                                   from y in _lmContext.HR_Emp_Leave_StatusDMO
                                   where (x.HRML_Id == y.HRML_Id && x.MI_Id == data.MI_Id && x.HRML_LeaveCreditFlg == true && y.HRME_Id == qyery1.FirstOrDefault().HRME_Id && y.HRMLY_Id == yearid)
                                   select new LeaveCreditDTO
                                   {
                                       HRELS_Id = y.HRELS_Id,
                                       HRML_LeaveName = x.HRML_LeaveName,
                                       HRML_LeaveCode = x.HRML_LeaveCode,
                                       HRELS_TotalLeaves = y.HRELS_TotalLeaves,
                                       HRELS_CreditedLeaves = y.HRELS_CreditedLeaves,
                                       HRELS_TransLeaves = y.HRELS_TransLeaves,
                                       HRELS_CBLeaves = y.HRELS_CBLeaves,
                                       HRML_Id = y.HRML_Id,
                                       HRME_Id = y.HRME_Id,
                                       HRML_WhenToApplyFlg = x.HRML_WhenToApplyFlg,
                                       HRML_NoOfDays = x.HRML_NoOfDays
                                   }).Distinct().ToArray();


                data.online_leave = (from a in _lmContext.HR_Master_Employee_DMO
                                     from b in _lmContext.HR_Master_Designation_DMO
                                     from c in _lmContext.HR_Emp_Leave_StatusDMO
                                     from d in _lmContext.Emp_Email_Id
                                     from e in _lmContext.Emp_MobileNo
                                     where (a.HRMDES_Id == b.HRMDES_Id && a.HRME_Id == qyery1.FirstOrDefault().HRME_Id && d.HRME_Id == a.HRME_Id && e.HRME_Id == a.HRME_Id && d.HRMEM_DeFaultFlag == "default" && e.HRMEMNO_DeFaultFlag == "default")
                                     select new LeaveCreditDTO
                                     {
                                         HRME_Id = a.HRME_Id,
                                         HRME_EmployeeFirstName = a.HRME_EmployeeFirstName + " " + (a.HRME_EmployeeMiddleName == null || a.HRME_EmployeeMiddleName == " " || a.HRME_EmployeeMiddleName == "0" ? " " : a.HRME_EmployeeMiddleName) + " " + (a.HRME_EmployeeLastName == null || a.HRME_EmployeeLastName == " " || a.HRME_EmployeeLastName == "0" ? " " : a.HRME_EmployeeLastName),
                                         HRME_DOJ = a.HRME_DOJ,
                                         HRME_MobileNo = e.HRMEMNO_MobileNo,
                                         HRME_EmailId = d.HRMEM_EmailId,
                                         HRMDES_DesignationName = b.HRMDES_DesignationName,
                                         HRME_EmployeeCode = a.HRME_EmployeeCode
                                     }).Distinct().ToArray();
              
            }

            return data;
        }


        public LeaveCreditDTO saveonlineLeave(LeaveCreditDTO _category)
        {
            int count = 0;
            HR_Emp_Leave_ApplicationDMO objpge = Mapper.Map<HR_Emp_Leave_ApplicationDMO>(_category);

            HR_Emp_Leave_Trans_DMO objLvTrans = Mapper.Map<HR_Emp_Leave_Trans_DMO>(_category);
            HR_Emp_Leave_Trans_Details_DMO objLvTrDetail = Mapper.Map<HR_Emp_Leave_Trans_Details_DMO>(_category);

            var leave_year = _lmContext.HR_Master_LeaveYear_DMO.Where(e => e.MI_Id == _category.MI_Id && DateTime.Now.Date >= e.HRMLY_FromDate.Date && DateTime.Now.Date <= e.HRMLY_ToDate.Date && e.HRMLY_ActiveFlag == true).ToList();

            var leave_id = _lmContext.HR_Master_Leave_DMO.Where(p => p.HRML_LeaveName == _category.temp_table_data.FirstOrDefault().HRML_LeaveName && p.MI_Id == _category.MI_Id).Select(p => p.HRML_Id).FirstOrDefault();
            long TransactionID = 0;
            try
            {

                var duplicate = _lmContext.HR_Emp_Leave_ApplicationDMO.Where(z => z.MI_Id == _category.MI_Id && z.HRELAP_FromDate >= _category.frmToDates[0].HRELAP_FromDate && z.HRELAP_ToDate <= _category.frmToDates[0].HRELAP_ToDate && z.HRELAP_ActiveFlag == true && z.HRME_Id == _category.HRME_Id).ToArray();
                if (duplicate.Length == 0)
                {
                    if (objpge.HRELAP_Id > 0)
                    {
                        var resultCount = _lmContext.HR_Emp_Leave_ApplicationDMO.Where(t => t.HRELAP_FromDate <= objpge.HRELAP_FromDate && t.HRELAP_ToDate >= objpge.HRELAP_ToDate).Count();

                        if (resultCount == 0)
                        {
                            var result = _lmContext.HR_Emp_Leave_ApplicationDMO.Single(t => t.MI_Id == objpge.MI_Id);

                            result.MI_Id = objpge.MI_Id;
                            result.HRME_Id = objpge.HRME_Id;
                            result.HRELAP_ApplicationDate = objpge.HRELAP_ApplicationDate;
                            result.HRELAP_FromDate = objpge.HRELAP_FromDate;
                            result.HRELAP_ToDate = objpge.HRELAP_ToDate;
                            result.HRELAP_TotalDays = objpge.HRELAP_TotalDays;
                            result.HRELAP_LeaveReason = objpge.HRELAP_LeaveReason;
                            result.HRELAP_ContactNoOnLeave = objpge.HRELAP_ContactNoOnLeave;
                            result.HRELAP_ReportingDate = objpge.HRELAP_ReportingDate;
                            // result.HRELAP_ApplicationID = objpge.HRELAP_ApplicationID;

                            result.UpdatedDate = DateTime.Now;
                            _lmContext.Update(result);
                            var contactExists = _lmContext.SaveChanges();
                            if (contactExists == 1)
                            {
                                _category.returnval = true;
                            }
                            else
                            {
                                _category.returnval = false;
                            }
                        }
                        else
                        {
                            _category.returnduplicatestatus = "Duplicate";
                            return _category;
                        }
                    }
                    else
                    {
                        var qyery1 = (from q in _db.Staff_User_Login
                                      from r in _db.HR_Master_Employee_DMO
                                      where (q.Emp_Code == r.HRME_Id && q.Id == _category.UserId)
                                      select new LeaveCreditDTO
                                      {
                                          HRME_Id = r.HRME_Id,
                                          LoginId = q.Id
                                      }).Distinct().ToArray();
                        

                        if (qyery1.Length > 0)
                        {

                            var checkbalanceleave = _lmContext.HR_Emp_Leave_StatusDMO.Where(d => d.MI_Id == _category.MI_Id && d.HRME_Id == qyery1.FirstOrDefault().HRME_Id && d.HRML_Id == _category.temp_table_data.FirstOrDefault().HRML_Id && d.HRMLY_Id == leave_year.FirstOrDefault().HRMLY_Id).ToList();
                            var bal_leave = checkbalanceleave.FirstOrDefault().HRELS_CBLeaves - _category.frmToDates.FirstOrDefault().HRELAP_TotalDays;
                            var fromdate = Convert.ToDateTime(_category.frmToDates.FirstOrDefault().HRELAP_FromDate).ToString("d");
                            var todate = Convert.ToDateTime(_category.frmToDates.FirstOrDefault().HRELAP_ToDate).ToString("d");

                            string leavetype = _lmContext.HR_Master_Leave_DMO.Where(p => p.HRML_Id == _category.temp_table_data[0].HRML_Id && p.MI_Id == _category.MI_Id).Select(p => p.HRML_LeaveName).FirstOrDefault();

                            if (leavetype == "ON DUTY")
                            {
                                var checkduplicate = (from m in _lmContext.HR_Emp_Leave_ApplicationDMO
                                                      from n in _lmContext.HR_Emp_Leave_Appl_DetailsDMO
                                                      where m.HRELAP_Id == n.HRELAP_Id && m.MI_Id == _category.MI_Id && m.HRME_Id == qyery1.FirstOrDefault().HRME_Id
                                                      && m.HRELAP_FromDate.Value.Date.ToString("dd/MM/yyyy") == fromdate
                                                      && m.HRELAP_ToDate.Value.Date.ToString("dd/MM/yyyy") == todate
                                                      && n.HRML_Id == _category.temp_table_data.FirstOrDefault().HRML_Id
                                                      select new LeaveCreditDTO
                                                      {
                                                          HRELAP_Id = m.HRELAP_Id
                                                      }
                                                     ).ToList();
                                if (checkduplicate.Count == 0)
                                {

                                     
                                        var id = _lmContext.HR_Emp_Leave_Trans_Details_DMO.Where(t => t.HRELTD_FromDate.Date >= Convert.ToDateTime(fromdate).Date && t.HRELTD_ToDate.Date <= Convert.ToDateTime(todate).Date && t.MI_Id == _category.MI_Id && t.HRELTD_LWPFlag == true && t.HRME_Id == qyery1.FirstOrDefault().HRME_Id).ToList();
                                    if (id.Count > 0)
                                    {
                                        for (int i = 0; i < id.Count(); i++)
                                        {
                                            id[i].HRELTD_LWPFlag = false;
                                            id[i].UpdatedDate = DateTime.Now;
                                            _lmContext.Update(id[i]);
                                            //  _lmcontext.savechanges();

                                            var hreltdid = id[i].HRELTD_Id;
                                            var hreltid = id[i].HRELT_Id;

                                            var idtrans11 = _lmContext.HR_Emp_Leave_Trans_DMO.Single(t => t.HRELT_Id == hreltid && t.MI_Id == _category.MI_Id && t.HRME_Id == qyery1.FirstOrDefault().HRME_Id);
                                            idtrans11.HRELT_LeaveReason = "OD";
                                            idtrans11.HRELT_Status = "Applied";
                                            idtrans11.UpdatedDate = DateTime.Now;
                                            _lmContext.Update(idtrans11);
                                            _lmContext.SaveChanges();

                                        }

                                        //Leave Id Creation from transaction numbering.
                                        var transnumconfigsettings = _db.Master_Numbering.Where(d => d.MI_Id == _category.MI_Id && d.IMN_Flag.Equals("LeaveNo")).ToList();
                                        if (transnumconfigsettings.Count > 0)
                                        {
                                            GenerateTransactionNumbering a = new GenerateTransactionNumbering(_db);
                                            Master_NumberingDTO num = new Master_NumberingDTO();
                                            num.MI_Id = _category.MI_Id;
                                            num.ASMAY_Id = _category.asmay_id;
                                            num.IMN_AutoManualFlag = transnumconfigsettings.FirstOrDefault().IMN_AutoManualFlag;
                                            num.IMN_DuplicatesFlag = transnumconfigsettings.FirstOrDefault().IMN_DuplicatesFlag;
                                            num.IMN_StartingNo = transnumconfigsettings.FirstOrDefault().IMN_StartingNo;
                                            num.IMN_WidthNumeric = transnumconfigsettings.FirstOrDefault().IMN_WidthNumeric;
                                            num.IMN_ZeroPrefixFlag = transnumconfigsettings.FirstOrDefault().IMN_ZeroPrefixFlag;
                                            num.IMN_PrefixAcadYearCode = transnumconfigsettings.FirstOrDefault().IMN_PrefixAcadYearCode;
                                            num.IMN_PrefixFinYearCode = transnumconfigsettings.FirstOrDefault().IMN_PrefixFinYearCode;
                                            num.IMN_PrefixCalYearCode = transnumconfigsettings.FirstOrDefault().IMN_PrefixCalYearCode;
                                            num.IMN_PrefixParticular = transnumconfigsettings.FirstOrDefault().IMN_PrefixParticular;
                                            num.IMN_SuffixAcadYearCode = transnumconfigsettings.FirstOrDefault().IMN_SuffixAcadYearCode;
                                            num.IMN_SuffixFinYearCode = transnumconfigsettings.FirstOrDefault().IMN_SuffixFinYearCode;
                                            num.IMN_SuffixCalYearCode = transnumconfigsettings.FirstOrDefault().IMN_SuffixCalYearCode;
                                            num.IMN_SuffixParticular = transnumconfigsettings.FirstOrDefault().IMN_SuffixParticular;
                                            num.IMN_RestartNumFlag = transnumconfigsettings.FirstOrDefault().IMN_RestartNumFlag;
                                            num.IMN_Flag = "LeaveNo";
                                            _category.HRELAP_ApplicationID = a.GenerateNumber(num);
                                        }

                                        //LEAVE APPLICATION
                                        objpge.HRME_Id = qyery1.FirstOrDefault().HRME_Id;
                                        objpge.CreatedDate = DateTime.Now;
                                        objpge.UpdatedDate = DateTime.Now;
                                        objpge.HRELAP_ActiveFlag = true;
                                        objpge.HRELAP_ApplicationID = _category.HRELAP_ApplicationID;
                                        objpge.HRELAP_TotalDays = _category.frmToDates.FirstOrDefault().HRELAP_TotalDays;
                                        objpge.HRELAP_ApplicationStatus = "Applied";
                                        objpge.HRELAP_SanctioningLevel = "1";
                                        objpge.HRELAP_FromDate = Convert.ToDateTime(_category.frmToDates.FirstOrDefault().HRELAP_FromDate);
                                        objpge.HRELAP_ToDate = Convert.ToDateTime(_category.frmToDates.FirstOrDefault().HRELAP_ToDate);
                                        objpge.HRELAP_SupportingDocument = _category.HRELT_SupportingDocument;
                                        _lmContext.Add(objpge);
                                        count = _lmContext.SaveChanges();


                                        //LEAVE APPLICATION DETAILS
                                        if (objpge.HRELAP_Id > 0)
                                        {
                                            HR_Emp_Leave_Appl_DetailsDMO updatedet = new HR_Emp_Leave_Appl_DetailsDMO();
                                            updatedet.HRELAPD_FromDate = _category.frmToDates.FirstOrDefault().HRELAP_FromDate;
                                            updatedet.HRELAPD_LeaveStatus = "Applied";
                                            updatedet.HRELAPD_ToDate = _category.frmToDates.FirstOrDefault().HRELAP_ToDate;
                                            updatedet.HRELAPD_TotalDays = _category.frmToDates.FirstOrDefault().HRELAP_TotalDays;
                                            updatedet.HRELAP_Id = objpge.HRELAP_Id;
                                            updatedet.HRML_Id = _category.temp_table_data.FirstOrDefault().HRML_Id;
                                            updatedet.UpdatedDate = DateTime.Now;
                                            updatedet.CreatedDate = DateTime.Now;
                                            updatedet.HRELAPD_ActiveFlag = true;
                                            //Mapper.Map(objpge1, resultempltransDetails);
                                            _lmContext.Add(updatedet);
                                            _lmContext.SaveChanges();
                                        }
                                    }



                                }

                         

                            }
                            else
                            {
                                if (checkbalanceleave.FirstOrDefault().HRELS_CBLeaves > 0 && bal_leave >= 0)
                                {
                                    var checkduplicate = (from m in _lmContext.HR_Emp_Leave_ApplicationDMO
                                                          from n in _lmContext.HR_Emp_Leave_Appl_DetailsDMO
                                                          where m.HRELAP_Id == n.HRELAP_Id && m.MI_Id == _category.MI_Id && m.HRME_Id == qyery1.FirstOrDefault().HRME_Id && m.HRELAP_FromDate == _category.frmToDates.FirstOrDefault().HRELAP_FromDate && m.HRELAP_ToDate == _category.frmToDates.FirstOrDefault().HRELAP_ToDate
                                                          //&& m.HRELAP_FromDate.Value.Date.ToString("dd/MM/yyyy") == fromdate
                                                          //&& m.HRELAP_ToDate.Value.Date.ToString("dd/MM/yyyy") == todate
                                                          && n.HRML_Id == _category.temp_table_data.FirstOrDefault().HRML_Id
                                                          select new LeaveCreditDTO
                                                          {
                                                              HRELAP_Id = m.HRELAP_Id
                                                          }).ToList();

                                    if (checkduplicate.Count == 0)
                                    {
                                        objpge.HRME_Id = qyery1.FirstOrDefault().HRME_Id;
                                        objpge.CreatedDate = DateTime.Now;
                                        objpge.UpdatedDate = DateTime.Now;
                                        objpge.HRELAP_ActiveFlag = true;

                                        objLvTrans.MI_Id = _category.MI_Id;
                                        objLvTrans.HRME_Id = qyery1.FirstOrDefault().HRME_Id;
                                        objLvTrans.HRMLY_Id = leave_year.FirstOrDefault().HRMLY_Id;
                                        objLvTrans.HRELT_LeaveId = leave_id;
                                        objLvTrans.HRELT_Reportingdate = _category.HRELT_Reportingdate;
                                        objLvTrans.HRELT_LeaveReason = _category.HRELT_LeaveReason;
                                        objLvTrans.HRELT_Status = "Applied";
                                        objLvTrans.CreatedDate = DateTime.Now;
                                        objLvTrans.UpdatedDate = DateTime.Now;
                                        objLvTrans.HRELT_ActiveFlag = true;
                                        objLvTrans.HRELT_SupportingDocument = _category.HRELT_SupportingDocument;

                                        objLvTrDetail.HRML_Id = _category.temp_table_data.FirstOrDefault().HRML_Id;
                                        objLvTrDetail.HRME_Id = qyery1.FirstOrDefault().HRME_Id;
                                        objLvTrDetail.MI_Id = _category.MI_Id;
                                        objLvTrDetail.HRELTD_LWPFlag = false;
                                        objLvTrDetail.CreatedDate = DateTime.Now;
                                        objLvTrDetail.UpdatedDate = DateTime.Now;

                                        //Leave Id Creation from transaction numbering.
                                        var transnumconfigsettings = _db.Master_Numbering.Where(d => d.MI_Id == _category.MI_Id && d.IMN_Flag.Equals("LeaveNo")).ToList();
                                        if (transnumconfigsettings.Count > 0)
                                        {
                                            GenerateTransactionNumbering a = new GenerateTransactionNumbering(_db);
                                            Master_NumberingDTO num = new Master_NumberingDTO();
                                            num.MI_Id = _category.MI_Id;
                                            num.ASMAY_Id = _category.asmay_id;
                                            num.IMN_AutoManualFlag = transnumconfigsettings.FirstOrDefault().IMN_AutoManualFlag;
                                            num.IMN_DuplicatesFlag = transnumconfigsettings.FirstOrDefault().IMN_DuplicatesFlag;
                                            num.IMN_StartingNo = transnumconfigsettings.FirstOrDefault().IMN_StartingNo;
                                            num.IMN_WidthNumeric = transnumconfigsettings.FirstOrDefault().IMN_WidthNumeric;
                                            num.IMN_ZeroPrefixFlag = transnumconfigsettings.FirstOrDefault().IMN_ZeroPrefixFlag;
                                            num.IMN_PrefixAcadYearCode = transnumconfigsettings.FirstOrDefault().IMN_PrefixAcadYearCode;
                                            num.IMN_PrefixFinYearCode = transnumconfigsettings.FirstOrDefault().IMN_PrefixFinYearCode;
                                            num.IMN_PrefixCalYearCode = transnumconfigsettings.FirstOrDefault().IMN_PrefixCalYearCode;
                                            num.IMN_PrefixParticular = transnumconfigsettings.FirstOrDefault().IMN_PrefixParticular;
                                            num.IMN_SuffixAcadYearCode = transnumconfigsettings.FirstOrDefault().IMN_SuffixAcadYearCode;
                                            num.IMN_SuffixFinYearCode = transnumconfigsettings.FirstOrDefault().IMN_SuffixFinYearCode;
                                            num.IMN_SuffixCalYearCode = transnumconfigsettings.FirstOrDefault().IMN_SuffixCalYearCode;
                                            num.IMN_SuffixParticular = transnumconfigsettings.FirstOrDefault().IMN_SuffixParticular;
                                            num.IMN_RestartNumFlag = transnumconfigsettings.FirstOrDefault().IMN_RestartNumFlag;
                                            num.IMN_Flag = "LeaveNo";
                                            _category.HRELAP_ApplicationID = a.GenerateNumber(num);
                                        }
                                        else
                                        {
                                            _category.returnmsg = "Nomapping";
                                        }
                                        if (_category.HRELAP_ApplicationID != null)
                                        {
                                            objLvTrans.HRELT_FromDate = Convert.ToDateTime(_category.frmToDates.FirstOrDefault().HRELAP_FromDate);
                                            objLvTrans.HRELT_ToDate = Convert.ToDateTime(_category.frmToDates.FirstOrDefault().HRELAP_ToDate);
                                            objLvTrans.HRELT_TotDays = _category.frmToDates.FirstOrDefault().HRELAP_TotalDays;

                                            objLvTrDetail.HRELTD_FromDate = Convert.ToDateTime(_category.frmToDates.FirstOrDefault().HRELAP_FromDate);
                                            objLvTrDetail.HRELTD_ToDate = Convert.ToDateTime(_category.frmToDates.FirstOrDefault().HRELAP_ToDate);
                                            objLvTrDetail.HRELTD_TotDays = _category.frmToDates.FirstOrDefault().HRELAP_TotalDays;

                                            objpge.HRELAP_ApplicationID = _category.HRELAP_ApplicationID;
                                            objpge.HRELAP_TotalDays = _category.frmToDates.FirstOrDefault().HRELAP_TotalDays;
                                            objpge.HRELAP_ApplicationStatus = "Applied";
                                            objpge.HRELAP_SanctioningLevel = "1";
                                            objpge.HRELAP_FromDate = Convert.ToDateTime(_category.frmToDates.FirstOrDefault().HRELAP_FromDate);
                                            objpge.HRELAP_ToDate = Convert.ToDateTime(_category.frmToDates.FirstOrDefault().HRELAP_ToDate);
                                            objpge.HRELAP_SupportingDocument = _category.HRELT_SupportingDocument;
                                            _lmContext.Add(objpge);
                                            count = _lmContext.SaveChanges();

                                            _lmContext.Add(objLvTrans);
                                            _lmContext.SaveChanges();

                                            var hrelt_id = _lmContext.HR_Emp_Leave_Trans_DMO.Where(z => z.MI_Id == _category.MI_Id && z.HRELT_FromDate == Convert.ToDateTime(_category.frmToDates.FirstOrDefault().HRELAP_FromDate) && z.HRELT_ToDate == Convert.ToDateTime(_category.frmToDates.FirstOrDefault().HRELAP_ToDate) && z.HRELT_TotDays == (_category.frmToDates.FirstOrDefault().HRELAP_TotalDays) && z.HRME_Id == qyery1.FirstOrDefault().HRME_Id && z.HRELT_LeaveId == leave_id && z.HRELT_LeaveReason == _category.HRELT_LeaveReason).Select(s => s.HRELT_Id).FirstOrDefault();

                                            objLvTrDetail.HRELT_Id = hrelt_id;
                                            _lmContext.Add(objLvTrDetail);
                                            _lmContext.SaveChanges();
                                        }
                                        var contactExists = count;
                                        HR_Emp_Leave_Appl_DetailsDMO objpge1 = Mapper.Map<HR_Emp_Leave_Appl_DetailsDMO>(_category);
                                        TransactionID = objpge.HRELAP_Id;
                                        if (objpge.HRELAP_Id > 0)
                                        {
                                            HR_Emp_Leave_Appl_DetailsDMO updatedet = new HR_Emp_Leave_Appl_DetailsDMO();
                                            updatedet.HRELAPD_FromDate = _category.frmToDates.FirstOrDefault().HRELAP_FromDate;
                                            updatedet.HRELAPD_LeaveStatus = "Applied";
                                            updatedet.HRELAPD_ToDate = _category.frmToDates.FirstOrDefault().HRELAP_ToDate;
                                            updatedet.HRELAPD_TotalDays = _category.frmToDates.FirstOrDefault().HRELAP_TotalDays;
                                            updatedet.HRELAP_Id = objpge.HRELAP_Id;
                                            updatedet.HRML_Id = _category.temp_table_data.FirstOrDefault().HRML_Id;
                                            updatedet.UpdatedDate = DateTime.Now;
                                            updatedet.CreatedDate = DateTime.Now;
                                            updatedet.HRELAPD_ActiveFlag = true;
                                            _lmContext.Add(updatedet);
                                            _lmContext.SaveChanges();

                                            string emailid = _lmContext.Emp_Email_Id.Where(p => p.HRME_Id == checkbalanceleave[0].HRME_Id && p.HRMEM_DeFaultFlag == "default").Select(p => p.HRMEM_EmailId).FirstOrDefault();
                                            long mobileno = _lmContext.Emp_MobileNo.Where(p => p.HRME_Id == checkbalanceleave[0].HRME_Id && p.HRMEMNO_DeFaultFlag == "default").Select(p => p.HRMEMNO_MobileNo).FirstOrDefault();
                                            _category.HRME_MobileNo = mobileno;
                                            _category.HRME_Id = checkbalanceleave[0].HRME_Id;
                                            if (emailid != null && emailid.Length > 0)
                                            {
                                                sendmail(checkbalanceleave[0].MI_Id, emailid, "Leave_Apply", checkbalanceleave[0].HRME_Id, _category.frmToDates.FirstOrDefault().HRELAP_FromDate);
                                            }
                                            if (mobileno != null && mobileno > 0)
                                            {
                                                sendSms(checkbalanceleave[0].MI_Id, mobileno, "Leave_Apply", checkbalanceleave[0].HRME_Id, _category.frmToDates.FirstOrDefault().HRELAP_FromDate);
                                            }
                                            var devicenew = _lmContext.HR_Master_Employee_DMO.Where(t => t.HRME_Id == checkbalanceleave[0].HRME_Id).Select(t => t.HRME_AppDownloadedDeviceId).FirstOrDefault();
                                           


                                            if (devicenew!=null && devicenew.Length > 0)
                                            {
                                                
                                                PushNotification push_noti = new PushNotification(_db);
                                                push_noti.Call_PushNotificationGeneral(devicenew, _category.MI_Id, _category.UserId, qyery1.FirstOrDefault().LoginId, TransactionID, _category.HRELT_LeaveReason, "LeaveStatus", "LeaveApplication");

                                            }




                                            //callnotification(devicenew, objpge.HRELAP_Id, _category.MI_Id, _category, "LEAVE MANAGEMENT");
                                        }
                                        else
                                        {

                                        }

                                        if (contactExists > 0)
                                        {
                                            _category.returnval = true;

                                            //var statusUpdate = _lmContext.HR_Emp_Leave_StatusDMO.Single(d => d.HRELS_Id == _category.temp_table_data.FirstOrDefault().HRELS_Id && d.HRMLY_Id == leave_year.FirstOrDefault().HRMLY_Id && d.HRME_Id == qyery1.FirstOrDefault().HRME_Id);
                                            //statusUpdate.HRELS_TransLeaves = _category.frmToDates.FirstOrDefault().HRELAP_TotalDays;
                                            //statusUpdate.HRELS_CBLeaves = bal_leave;
                                            //_lmContext.Update(statusUpdate);
                                            //_lmContext.SaveChanges();
                                        }
                                        else
                                        {
                                            _category.returnval = false;
                                        }
                                    }
                                    else
                                    {
                                        _category.returnduplicatestatus = "Duplicate";
                                    }

                                }
                                else
                                {
                                    _category.returnmsg = "You Crossed Your Leave Limits";
                                    List<HR_Emp_Leave_ApplicationDMO> mm_events = new List<HR_Emp_Leave_ApplicationDMO>();
                                    mm_events = _lmContext.HR_Emp_Leave_ApplicationDMO.Where(e => e.MI_Id == _category.MI_Id).ToList();
                                    _category.master_eventlist = mm_events.ToArray();
                                    return _category;
                                }
                            }
                        }
                    }
                }
                else
                {
                    _category.returnmsg = "Leave Already applied on these dates";
                    List<HR_Emp_Leave_ApplicationDMO> m_events = new List<HR_Emp_Leave_ApplicationDMO>();
                    m_events = _lmContext.HR_Emp_Leave_ApplicationDMO.Where(e => e.MI_Id == _category.MI_Id).ToList();
                    _category.master_eventlist = m_events.ToArray();
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return _category;
        }

        public LeaveCreditDTO saveadminLeave(LeaveCreditDTO _category)
        {
            int count = 0;
            HR_Emp_Leave_ApplicationDMO objpge = Mapper.Map<HR_Emp_Leave_ApplicationDMO>(_category);

            HR_Emp_Leave_Trans_DMO objLvTrans = Mapper.Map<HR_Emp_Leave_Trans_DMO>(_category);
            HR_Emp_Leave_Trans_Details_DMO objLvTrDetail = Mapper.Map<HR_Emp_Leave_Trans_Details_DMO>(_category);

            var leave_year = _lmContext.HR_Master_LeaveYear_DMO.Where(e => e.MI_Id == _category.MI_Id && DateTime.Now.Date >= e.HRMLY_FromDate.Date && DateTime.Now.Date <= e.HRMLY_ToDate.Date).ToList();

            var leave_id = _lmContext.HR_Master_Leave_DMO.Where(p => p.HRML_LeaveName == _category.temp_table_data.FirstOrDefault().HRML_LeaveName && p.MI_Id == _category.MI_Id).Select(p => p.HRML_Id).FirstOrDefault();

            try
            {
                var duplicate = _lmContext.HR_Emp_Leave_ApplicationDMO.Where(z => z.MI_Id == _category.MI_Id && z.HRELAP_FromDate >= _category.frmToDates[0].HRELAP_FromDate && z.HRELAP_ToDate <= _category.frmToDates[0].HRELAP_ToDate && z.HRELAP_ActiveFlag == true && z.HRME_Id == _category.HRME_Id).ToArray();
                if (duplicate.Length == 0)
                {
                    if (objpge.HRELAP_Id > 0)
                    {
                        var resultCount = _lmContext.HR_Emp_Leave_ApplicationDMO.Where(t => t.HRELAP_FromDate <= objpge.HRELAP_FromDate && t.HRELAP_ToDate >= objpge.HRELAP_ToDate).Count();

                        if (resultCount == 0)
                        {
                            var result = _lmContext.HR_Emp_Leave_ApplicationDMO.Single(t => t.MI_Id == objpge.MI_Id);

                            result.MI_Id = objpge.MI_Id;
                            result.HRME_Id = objpge.HRME_Id;
                            result.HRELAP_ApplicationDate = objpge.HRELAP_ApplicationDate;
                            result.HRELAP_FromDate = objpge.HRELAP_FromDate;
                            result.HRELAP_ToDate = objpge.HRELAP_ToDate;
                            result.HRELAP_TotalDays = objpge.HRELAP_TotalDays;
                            result.HRELAP_LeaveReason = objpge.HRELAP_LeaveReason;
                            result.HRELAP_ContactNoOnLeave = objpge.HRELAP_ContactNoOnLeave;
                            result.HRELAP_ReportingDate = objpge.HRELAP_ReportingDate;
                           // result.HRELAP_SupportingDocument= HRELT_SupportingDocument
                            // result.HRELAP_ApplicationID = objpge.HRELAP_ApplicationID;

                            result.UpdatedDate = DateTime.Now;
                            _lmContext.Update(result);
                            var contactExists = _lmContext.SaveChanges();
                            if (contactExists == 1)
                            {
                                _category.returnval = true;
                            }
                            else
                            {
                                _category.returnval = false;
                            }
                        }
                        else
                        {
                            _category.returnduplicatestatus = "Duplicate";
                            return _category;
                        }
                    }
                    else
                    {
                        var checkbalanceleave = _lmContext.HR_Emp_Leave_StatusDMO.Where(d => d.MI_Id == _category.MI_Id && d.HRME_Id == _category.HRME_Id && d.HRML_Id == _category.temp_table_data.FirstOrDefault().HRML_Id && d.HRMLY_Id == leave_year.FirstOrDefault().HRMLY_Id).ToList();
                        var bal_leave = checkbalanceleave.FirstOrDefault().HRELS_CBLeaves - _category.frmToDates.FirstOrDefault().HRELAP_TotalDays;
                        var fromdate = Convert.ToDateTime(_category.frmToDates.FirstOrDefault().HRELAP_FromDate).ToString("d");
                        var todate = Convert.ToDateTime(_category.frmToDates.FirstOrDefault().HRELAP_ToDate).ToString("d");

                        string leavetype = _lmContext.HR_Master_Leave_DMO.Where(p => p.HRML_Id == _category.temp_table_data[0].HRML_Id && p.MI_Id == _category.MI_Id).Select(p => p.HRML_LeaveName).FirstOrDefault();

                        if (leavetype == "ON DUTY")
                        {
                            var checkduplicate = (from m in _lmContext.HR_Emp_Leave_ApplicationDMO
                                                  from n in _lmContext.HR_Emp_Leave_Appl_DetailsDMO
                                                  where m.HRELAP_Id == n.HRELAP_Id && m.MI_Id == _category.MI_Id && m.HRME_Id == _category.HRME_Id
                                                  && m.HRELAP_FromDate.Value.Date.ToString("dd/MM/yyyy") == fromdate
                                                  && m.HRELAP_ToDate.Value.Date.ToString("dd/MM/yyyy") == todate
                                                  && n.HRML_Id == _category.temp_table_data.FirstOrDefault().HRML_Id
                                                  select new LeaveCreditDTO
                                                  {
                                                      HRELAP_Id = m.HRELAP_Id
                                                  }
                                                    ).ToList();
                            if (checkduplicate.Count == 0)
                            {
                                objpge.HRME_Id = _category.HRME_Id;
                                objpge.CreatedDate = DateTime.Now;
                                objpge.UpdatedDate = DateTime.Now;
                                objpge.HRELAP_ActiveFlag = true;

                                objLvTrans.MI_Id = _category.MI_Id;
                                objLvTrans.HRME_Id = _category.HRME_Id;
                                objLvTrans.HRMLY_Id = leave_year.FirstOrDefault().HRMLY_Id;
                                objLvTrans.HRELT_LeaveId = leave_id;
                                objLvTrans.HRELT_Reportingdate = _category.HRELT_Reportingdate;
                                objLvTrans.HRELT_LeaveReason = _category.HRELT_LeaveReason;
                                objLvTrans.HRELT_Status = "Applied";
                                objLvTrans.CreatedDate = DateTime.Now;
                                objLvTrans.UpdatedDate = DateTime.Now;
                                objLvTrans.HRELT_ActiveFlag = true;

                                objLvTrDetail.HRML_Id = _category.temp_table_data.FirstOrDefault().HRML_Id;
                                objLvTrDetail.HRME_Id = _category.HRME_Id;
                                objLvTrDetail.MI_Id = _category.MI_Id;
                                objLvTrDetail.HRELTD_LWPFlag = false;
                                objLvTrDetail.CreatedDate = DateTime.Now;
                                objLvTrDetail.UpdatedDate = DateTime.Now;

                                //Leave Id Creation from transaction numbering.
                                var transnumconfigsettings = _db.Master_Numbering.Where(d => d.MI_Id == _category.MI_Id && d.IMN_Flag.Equals("LeaveNo")).ToList();
                                if (transnumconfigsettings.Count > 0)
                                {
                                    GenerateTransactionNumbering a = new GenerateTransactionNumbering(_db);
                                    Master_NumberingDTO num = new Master_NumberingDTO();
                                    num.MI_Id = _category.MI_Id;
                                    num.ASMAY_Id = _category.asmay_id;
                                    num.IMN_AutoManualFlag = transnumconfigsettings.FirstOrDefault().IMN_AutoManualFlag;
                                    num.IMN_DuplicatesFlag = transnumconfigsettings.FirstOrDefault().IMN_DuplicatesFlag;
                                    num.IMN_StartingNo = transnumconfigsettings.FirstOrDefault().IMN_StartingNo;
                                    num.IMN_WidthNumeric = transnumconfigsettings.FirstOrDefault().IMN_WidthNumeric;
                                    num.IMN_ZeroPrefixFlag = transnumconfigsettings.FirstOrDefault().IMN_ZeroPrefixFlag;
                                    num.IMN_PrefixAcadYearCode = transnumconfigsettings.FirstOrDefault().IMN_PrefixAcadYearCode;
                                    num.IMN_PrefixFinYearCode = transnumconfigsettings.FirstOrDefault().IMN_PrefixFinYearCode;
                                    num.IMN_PrefixCalYearCode = transnumconfigsettings.FirstOrDefault().IMN_PrefixCalYearCode;
                                    num.IMN_PrefixParticular = transnumconfigsettings.FirstOrDefault().IMN_PrefixParticular;
                                    num.IMN_SuffixAcadYearCode = transnumconfigsettings.FirstOrDefault().IMN_SuffixAcadYearCode;
                                    num.IMN_SuffixFinYearCode = transnumconfigsettings.FirstOrDefault().IMN_SuffixFinYearCode;
                                    num.IMN_SuffixCalYearCode = transnumconfigsettings.FirstOrDefault().IMN_SuffixCalYearCode;
                                    num.IMN_SuffixParticular = transnumconfigsettings.FirstOrDefault().IMN_SuffixParticular;
                                    num.IMN_RestartNumFlag = transnumconfigsettings.FirstOrDefault().IMN_RestartNumFlag;
                                    num.IMN_Flag = "LeaveNo";
                                    _category.HRELAP_ApplicationID = a.GenerateNumber(num);
                                }
                                else
                                {
                                    _category.returnmsg = "Nomapping";
                                }

                                if (_category.HRELAP_ApplicationID != null)
                                {
                                    objLvTrans.HRELT_FromDate = Convert.ToDateTime(_category.frmToDates.FirstOrDefault().HRELAP_FromDate);
                                    objLvTrans.HRELT_ToDate = Convert.ToDateTime(_category.frmToDates.FirstOrDefault().HRELAP_ToDate);
                                    objLvTrans.HRELT_TotDays = _category.frmToDates.FirstOrDefault().HRELAP_TotalDays;

                                    objLvTrDetail.HRELTD_FromDate = Convert.ToDateTime(_category.frmToDates.FirstOrDefault().HRELAP_FromDate);
                                    objLvTrDetail.HRELTD_ToDate = Convert.ToDateTime(_category.frmToDates.FirstOrDefault().HRELAP_ToDate);
                                    objLvTrDetail.HRELTD_TotDays = _category.frmToDates.FirstOrDefault().HRELAP_TotalDays;

                                    objpge.HRELAP_ApplicationID = _category.HRELAP_ApplicationID;
                                    objpge.HRELAP_TotalDays = _category.frmToDates.FirstOrDefault().HRELAP_TotalDays;
                                    objpge.HRELAP_ApplicationStatus = "Applied";
                                    objpge.HRELAP_SanctioningLevel = "1";
                                    objpge.HRELAP_FromDate = Convert.ToDateTime(_category.frmToDates.FirstOrDefault().HRELAP_FromDate);
                                    objpge.HRELAP_ToDate = Convert.ToDateTime(_category.frmToDates.FirstOrDefault().HRELAP_ToDate);
                                    objpge.HRELAP_SupportingDocument = _category.HRELT_SupportingDocument;
                                    _lmContext.Add(objpge);
                                    count = _lmContext.SaveChanges();

                                    _lmContext.Add(objLvTrans);
                                    _lmContext.SaveChanges();

                                    var hrelt_id = _lmContext.HR_Emp_Leave_Trans_DMO.Where(z => z.MI_Id == _category.MI_Id && z.HRELT_FromDate == Convert.ToDateTime(_category.frmToDates.FirstOrDefault().HRELAP_FromDate) && z.HRELT_ToDate == Convert.ToDateTime(_category.frmToDates.FirstOrDefault().HRELAP_ToDate) && z.HRELT_TotDays == (_category.frmToDates.FirstOrDefault().HRELAP_TotalDays) && z.HRME_Id == _category.HRME_Id && z.HRELT_LeaveId == leave_id && z.HRELT_LeaveReason == _category.HRELT_LeaveReason).Select(s => s.HRELT_Id).FirstOrDefault();

                                    objLvTrDetail.HRELT_Id = hrelt_id;
                                    _lmContext.Add(objLvTrDetail);
                                    _lmContext.SaveChanges();
                                }
                                var contactExists = count;
                                HR_Emp_Leave_Appl_DetailsDMO objpge1 = Mapper.Map<HR_Emp_Leave_Appl_DetailsDMO>(_category);

                                if (objpge.HRELAP_Id > 0)
                                {
                                    HR_Emp_Leave_Appl_DetailsDMO updatedet = new HR_Emp_Leave_Appl_DetailsDMO();
                                    updatedet.HRELAPD_FromDate = _category.frmToDates.FirstOrDefault().HRELAP_FromDate;
                                    updatedet.HRELAPD_LeaveStatus = "Applied";
                                    updatedet.HRELAPD_ToDate = _category.frmToDates.FirstOrDefault().HRELAP_ToDate;
                                    updatedet.HRELAPD_TotalDays = _category.frmToDates.FirstOrDefault().HRELAP_TotalDays;
                                    updatedet.HRELAP_Id = objpge.HRELAP_Id;
                                    updatedet.HRML_Id = _category.temp_table_data.FirstOrDefault().HRML_Id;
                                    updatedet.UpdatedDate = DateTime.Now;
                                    updatedet.CreatedDate = DateTime.Now;
                                    updatedet.HRELAPD_ActiveFlag = true;
                                    //Mapper.Map(objpge1, resultempltransDetails);
                                    _lmContext.Add(updatedet);
                                    _lmContext.SaveChanges();

                                    string emailid = _lmContext.Emp_Email_Id.Where(p => p.HRME_Id == checkbalanceleave[0].HRME_Id && p.HRMEM_DeFaultFlag == "default").Select(p => p.HRMEM_EmailId).FirstOrDefault();
                                    long mobileno = _lmContext.Emp_MobileNo.Where(p => p.HRME_Id == checkbalanceleave[0].HRME_Id && p.HRMEMNO_DeFaultFlag == "default").Select(p => p.HRMEMNO_MobileNo).FirstOrDefault();

                                    sendmail(checkbalanceleave[0].MI_Id, emailid, "Leave_Apply", checkbalanceleave[0].HRME_Id, _category.frmToDates.FirstOrDefault().HRELAP_FromDate);
                                    sendSms(checkbalanceleave[0].MI_Id, mobileno, "Leave_Apply", checkbalanceleave[0].HRME_Id, _category.frmToDates.FirstOrDefault().HRELAP_FromDate);
                                }
                            }

                        }
                        else
                        {
                            if (checkbalanceleave.FirstOrDefault().HRELS_CBLeaves > 0 && bal_leave >= 0)
                            {
                                var checkduplicate = (from m in _lmContext.HR_Emp_Leave_ApplicationDMO
                                                      from n in _lmContext.HR_Emp_Leave_Appl_DetailsDMO
                                                      where m.HRELAP_Id == n.HRELAP_Id && m.MI_Id == _category.MI_Id && m.HRME_Id == _category.HRME_Id
                                                      && m.HRELAP_FromDate.Value.Date.ToString("dd/MM/yyyy") == fromdate
                                                      && m.HRELAP_ToDate.Value.Date.ToString("dd/MM/yyyy") == todate
                                                      && n.HRML_Id == _category.temp_table_data.FirstOrDefault().HRML_Id
                                                      select new LeaveCreditDTO
                                                      {
                                                          HRELAP_Id = m.HRELAP_Id
                                                      }
                                                        ).ToList();
                                if (checkduplicate.Count == 0)
                                {
                                    objpge.HRME_Id = _category.HRME_Id;
                                    objpge.CreatedDate = DateTime.Now;
                                    objpge.UpdatedDate = DateTime.Now;
                                    objpge.HRELAP_ActiveFlag = true;

                                    objLvTrans.MI_Id = _category.MI_Id;
                                    objLvTrans.HRME_Id = _category.HRME_Id;
                                    objLvTrans.HRMLY_Id = leave_year.FirstOrDefault().HRMLY_Id;
                                    objLvTrans.HRELT_LeaveId = leave_id;
                                    objLvTrans.HRELT_Reportingdate = _category.HRELT_Reportingdate;
                                    objLvTrans.HRELT_LeaveReason = _category.HRELT_LeaveReason;
                                    objLvTrans.HRELT_Status = "Applied";
                                    objLvTrans.CreatedDate = DateTime.Now;
                                    objLvTrans.UpdatedDate = DateTime.Now;
                                    objLvTrans.HRELT_ActiveFlag = true;

                                    objLvTrDetail.HRML_Id = _category.temp_table_data.FirstOrDefault().HRML_Id;
                                    objLvTrDetail.HRME_Id = _category.HRME_Id;
                                    objLvTrDetail.MI_Id = _category.MI_Id;
                                    objLvTrDetail.HRELTD_LWPFlag = false;
                                    objLvTrDetail.CreatedDate = DateTime.Now;
                                    objLvTrDetail.UpdatedDate = DateTime.Now;

                                    //Leave Id Creation from transaction numbering.
                                    var transnumconfigsettings = _db.Master_Numbering.Where(d => d.MI_Id == _category.MI_Id && d.IMN_Flag.Equals("LeaveNo")).ToList();
                                    if (transnumconfigsettings.Count > 0)
                                    {
                                        GenerateTransactionNumbering a = new GenerateTransactionNumbering(_db);
                                        Master_NumberingDTO num = new Master_NumberingDTO();
                                        num.MI_Id = _category.MI_Id;
                                        num.ASMAY_Id = _category.asmay_id;
                                        num.IMN_AutoManualFlag = transnumconfigsettings.FirstOrDefault().IMN_AutoManualFlag;
                                        num.IMN_DuplicatesFlag = transnumconfigsettings.FirstOrDefault().IMN_DuplicatesFlag;
                                        num.IMN_StartingNo = transnumconfigsettings.FirstOrDefault().IMN_StartingNo;
                                        num.IMN_WidthNumeric = transnumconfigsettings.FirstOrDefault().IMN_WidthNumeric;
                                        num.IMN_ZeroPrefixFlag = transnumconfigsettings.FirstOrDefault().IMN_ZeroPrefixFlag;
                                        num.IMN_PrefixAcadYearCode = transnumconfigsettings.FirstOrDefault().IMN_PrefixAcadYearCode;
                                        num.IMN_PrefixFinYearCode = transnumconfigsettings.FirstOrDefault().IMN_PrefixFinYearCode;
                                        num.IMN_PrefixCalYearCode = transnumconfigsettings.FirstOrDefault().IMN_PrefixCalYearCode;
                                        num.IMN_PrefixParticular = transnumconfigsettings.FirstOrDefault().IMN_PrefixParticular;
                                        num.IMN_SuffixAcadYearCode = transnumconfigsettings.FirstOrDefault().IMN_SuffixAcadYearCode;
                                        num.IMN_SuffixFinYearCode = transnumconfigsettings.FirstOrDefault().IMN_SuffixFinYearCode;
                                        num.IMN_SuffixCalYearCode = transnumconfigsettings.FirstOrDefault().IMN_SuffixCalYearCode;
                                        num.IMN_SuffixParticular = transnumconfigsettings.FirstOrDefault().IMN_SuffixParticular;
                                        num.IMN_RestartNumFlag = transnumconfigsettings.FirstOrDefault().IMN_RestartNumFlag;
                                        num.IMN_Flag = "LeaveNo";
                                        _category.HRELAP_ApplicationID = a.GenerateNumber(num);
                                    }
                                    else
                                    {
                                        _category.returnmsg = "Nomapping";
                                    }
                                    if (_category.HRELAP_ApplicationID != null)
                                    {
                                        objLvTrans.HRELT_FromDate = Convert.ToDateTime(_category.frmToDates.FirstOrDefault().HRELAP_FromDate);
                                        objLvTrans.HRELT_ToDate = Convert.ToDateTime(_category.frmToDates.FirstOrDefault().HRELAP_ToDate);
                                        objLvTrans.HRELT_TotDays = _category.frmToDates.FirstOrDefault().HRELAP_TotalDays;

                                        objLvTrDetail.HRELTD_FromDate = Convert.ToDateTime(_category.frmToDates.FirstOrDefault().HRELAP_FromDate);
                                        objLvTrDetail.HRELTD_ToDate = Convert.ToDateTime(_category.frmToDates.FirstOrDefault().HRELAP_ToDate);
                                        objLvTrDetail.HRELTD_TotDays = _category.frmToDates.FirstOrDefault().HRELAP_TotalDays;

                                        objpge.HRELAP_ApplicationID = _category.HRELAP_ApplicationID;
                                        objpge.HRELAP_TotalDays = _category.frmToDates.FirstOrDefault().HRELAP_TotalDays;
                                        objpge.HRELAP_ApplicationStatus = "Applied";
                                        objpge.HRELAP_SanctioningLevel = "1";
                                        objpge.HRELAP_FromDate = Convert.ToDateTime(_category.frmToDates.FirstOrDefault().HRELAP_FromDate);
                                        objpge.HRELAP_ToDate = Convert.ToDateTime(_category.frmToDates.FirstOrDefault().HRELAP_ToDate);
                                        objpge.HRELAP_SupportingDocument = _category.HRELT_SupportingDocument;
                                        _lmContext.Add(objpge);
                                        count = _lmContext.SaveChanges();

                                        _lmContext.Add(objLvTrans);
                                        _lmContext.SaveChanges();

                                        var hrelt_id = _lmContext.HR_Emp_Leave_Trans_DMO.Where(z => z.MI_Id == _category.MI_Id && z.HRELT_FromDate == Convert.ToDateTime(_category.frmToDates.FirstOrDefault().HRELAP_FromDate) && z.HRELT_ToDate == Convert.ToDateTime(_category.frmToDates.FirstOrDefault().HRELAP_ToDate) && z.HRELT_TotDays == (_category.frmToDates.FirstOrDefault().HRELAP_TotalDays) && z.HRME_Id == _category.HRME_Id && z.HRELT_LeaveId == leave_id && z.HRELT_LeaveReason == _category.HRELT_LeaveReason).Select(s => s.HRELT_Id).FirstOrDefault();

                                        objLvTrDetail.HRELT_Id = hrelt_id;
                                        _lmContext.Add(objLvTrDetail);
                                        _lmContext.SaveChanges();
                                    }
                                    var contactExists = count;
                                    HR_Emp_Leave_Appl_DetailsDMO objpge1 = Mapper.Map<HR_Emp_Leave_Appl_DetailsDMO>(_category);


                                    if (objpge.HRELAP_Id > 0)
                                    {
                                        HR_Emp_Leave_Appl_DetailsDMO updatedet = new HR_Emp_Leave_Appl_DetailsDMO();
                                        updatedet.HRELAPD_FromDate = _category.frmToDates.FirstOrDefault().HRELAP_FromDate;
                                        updatedet.HRELAPD_LeaveStatus = "Applied";
                                        updatedet.HRELAPD_ToDate = _category.frmToDates.FirstOrDefault().HRELAP_ToDate;
                                        updatedet.HRELAPD_TotalDays = _category.frmToDates.FirstOrDefault().HRELAP_TotalDays;
                                        updatedet.HRELAP_Id = objpge.HRELAP_Id;
                                        updatedet.HRML_Id = _category.temp_table_data.FirstOrDefault().HRML_Id;
                                        updatedet.UpdatedDate = DateTime.Now;
                                        updatedet.CreatedDate = DateTime.Now;
                                        updatedet.HRELAPD_ActiveFlag = true;
                                        //Mapper.Map(objpge1, resultempltransDetails);
                                        _lmContext.Add(updatedet);
                                        _lmContext.SaveChanges();

                                        string emailid = _lmContext.Emp_Email_Id.Where(p => p.HRME_Id == checkbalanceleave[0].HRME_Id && p.HRMEM_DeFaultFlag == "default").Select(p => p.HRMEM_EmailId).FirstOrDefault();
                                        long mobileno = _lmContext.Emp_MobileNo.Where(p => p.HRME_Id == checkbalanceleave[0].HRME_Id && p.HRMEMNO_DeFaultFlag == "default").Select(p => p.HRMEMNO_MobileNo).FirstOrDefault();

                                        sendmail(checkbalanceleave[0].MI_Id, emailid, "Leave_Apply", checkbalanceleave[0].HRME_Id, _category.frmToDates.FirstOrDefault().HRELAP_FromDate);
                                        sendSms(checkbalanceleave[0].MI_Id, mobileno, "Leave_Apply", checkbalanceleave[0].HRME_Id, _category.frmToDates.FirstOrDefault().HRELAP_FromDate);
                                    }
                                    else
                                    {

                                    }

                                    if (contactExists > 0)
                                    {
                                        _category.returnval = true;

                                        //var statusUpdate = _lmContext.HR_Emp_Leave_StatusDMO.Single(d => d.HRELS_Id == _category.temp_table_data.FirstOrDefault().HRELS_Id && d.HRMLY_Id == leave_year.FirstOrDefault().HRMLY_Id && d.HRME_Id == qyery1.FirstOrDefault().HRME_Id);
                                        //statusUpdate.HRELS_TransLeaves = _category.frmToDates.FirstOrDefault().HRELAP_TotalDays;
                                        //statusUpdate.HRELS_CBLeaves = bal_leave;
                                        //_lmContext.Update(statusUpdate);
                                        //_lmContext.SaveChanges();
                                    }
                                    else
                                    {
                                        _category.returnval = false;
                                    }
                                }
                                else
                                {
                                    _category.returnduplicatestatus = "Duplicate";
                                }

                            }
                            else
                            {
                                _category.message = "You Crossed Your Leave Limits";
                                List<HR_Emp_Leave_ApplicationDMO> mm_events = new List<HR_Emp_Leave_ApplicationDMO>();
                                mm_events = _lmContext.HR_Emp_Leave_ApplicationDMO.Where(e => e.MI_Id == _category.MI_Id).ToList();
                                _category.master_eventlist = mm_events.ToArray();
                                return _category;
                            }
                        }
                    }
                }
                else
                {
                    _category.message = "Leave Already applied on these dates";
                    List<HR_Emp_Leave_ApplicationDMO> m_events = new List<HR_Emp_Leave_ApplicationDMO>();
                    m_events = _lmContext.HR_Emp_Leave_ApplicationDMO.Where(e => e.MI_Id == _category.MI_Id).ToList();
                    _category.master_eventlist = m_events.ToArray();
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return _category;
        }

        public LeaveCreditDTO getonlineLeavestatus(LeaveCreditDTO data)
        {
            try
            {
                var qyery1 = (from q in _db.Staff_User_Login
                              from r in _db.HR_Master_Employee_DMO
                              where (q.Emp_Code == r.HRME_Id && q.Id == data.UserId)
                              select new LeaveCreditDTO
                              {
                                  HRME_Id = r.HRME_Id,
                              }).Distinct().ToArray();

                if (qyery1.Length > 0)
                {

                    data.leave_name = (from a in _lmContext.HR_Master_Leave_DMO
                                       from b in _lmContext.HR_Emp_Leave_Appl_DetailsDMO
                                       from c in _lmContext.HR_Emp_Leave_ApplicationDMO
                                       join d in _lmContext.HR_Emp_Leave_Appl_AuthorisationDMO on c.HRELAP_Id equals d.HRELAP_Id into FULLTABLE
                                       from rw in FULLTABLE.DefaultIfEmpty()
                                       where (a.HRML_Id == b.HRML_Id && c.HRELAP_Id == b.HRELAP_Id && a.MI_Id == data.MI_Id && c.HRME_Id == qyery1.FirstOrDefault().HRME_Id)
                                       select new LeaveCreditDTO
                                       {
                                           HRELAP_Id = c.HRELAP_Id,
                                           HRML_LeaveName = a.HRML_LeaveName,
                                           HRELAP_FromDate = c.HRELAP_FromDate,
                                           HRELAP_ToDate = c.HRELAP_ToDate,
                                           HRELTD_TotDays = b.HRELAPD_TotalDays,
                                           HRELAP_TotalDays = b.HRELAPD_TotalDays,
                                           HRELAP_ApplicationStatus = b.HRELAPD_LeaveStatus,
                                           HRML_Id = b.HRML_Id,
                                           HRELAP_LeaveReason = c.HRELAP_LeaveReason,
                                           HRELAPA_Remarks = rw.HRELAPA_Remarks,
                                            HRELAP_SupportingDocument = c.HRELAP_SupportingDocument
                                       }).OrderByDescending(d => d.HRELAP_Id).Distinct().ToArray();

                }

                using (var cmd = _lmContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "HR_Employee_Leave_Grid";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@userid", SqlDbType.VarChar) { Value = data.UserId });

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
                                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                                {
                                    dataRow.Add(
                                        dataReader.GetName(iFiled),
                                        dataReader.IsDBNull(iFiled) ? 0 : dataReader[iFiled]
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.compoffodgridvalues = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }


            }
            catch (Exception ex)
            {

            }
            return data;
        }

        public LeaveCreditDTO getSingleEmpLeavestatus(LeaveCreditDTO data)
        {
            try
            {
                long yearid = _lmContext.HR_Master_LeaveYear_DMO.Where(t => t.HRMLY_FromDate <= DateTime.Today && t.HRMLY_ToDate >= DateTime.Today && t.HRMLY_ActiveFlag == true && t.MI_Id == data.MI_Id).Select(t => t.HRMLY_Id).FirstOrDefault();

                data.multi_leave_name = (from x in _lmContext.HR_Master_Leave_DMO
                                         from y in _lmContext.HR_Emp_Leave_StatusDMO
                                         from z in _lmContext.Emp_MobileNo
                                         from a in _lmContext.HR_Master_LeaveYear_DMO
                                         where (x.HRML_Id == y.HRML_Id && x.MI_Id == data.MI_Id && y.HRMLY_Id == yearid && x.HRML_LeaveCreditFlg == true && y.HRME_Id == data.HRME_Id && z.HRME_Id == data.HRME_Id && z.HRMEMNO_DeFaultFlag == "default" && a.HRMLY_FromDate <= DateTime.Today && a.HRMLY_ToDate >= DateTime.Today && y.HRMLY_Id == a.HRMLY_Id && a.MI_Id == data.MI_Id && a.HRMLY_ActiveFlag == true)
                                         select new LeaveCreditDTO
                                         {
                                             HRELS_Id = y.HRELS_Id,
                                             HRML_LeaveName = x.HRML_LeaveName,
                                             HRELS_TotalLeaves = y.HRELS_TotalLeaves,
                                             HRELS_CreditedLeaves = y.HRELS_CreditedLeaves,
                                             HRELS_TransLeaves = y.HRELS_TransLeaves,
                                             HRELS_CBLeaves = y.HRELS_CBLeaves,
                                             HRML_Id = y.HRML_Id,
                                             HRMEMNO_MobileNo = z.HRMEMNO_MobileNo
                                         }
                         ).Distinct().ToArray();

                data.leave_name = (from a in _lmContext.HR_Master_Leave_DMO
                                   from b in _lmContext.HR_Emp_Leave_Appl_DetailsDMO
                                   from c in _lmContext.HR_Emp_Leave_ApplicationDMO
                                   join d in _lmContext.HR_Emp_Leave_Appl_AuthorisationDMO on c.HRELAP_Id equals d.HRELAP_Id into FULLTABLE
                                   from rw in FULLTABLE.DefaultIfEmpty()
                                   where (a.HRML_Id == b.HRML_Id && c.HRELAP_Id == b.HRELAP_Id && a.MI_Id == data.MI_Id && c.HRME_Id == data.HRME_Id)
                                   select new LeaveCreditDTO
                                   {
                                       HRELAP_Id = c.HRELAP_Id,
                                       HRML_LeaveName = a.HRML_LeaveName,
                                       HRELAP_FromDate = c.HRELAP_FromDate,
                                       HRELAP_ToDate = c.HRELAP_ToDate,
                                       HRELTD_TotDays = b.HRELAPD_TotalDays,
                                       HRELAP_ApplicationStatus = b.HRELAPD_LeaveStatus,
                                       HRML_Id = b.HRML_Id,
                                       HRELAP_LeaveReason = c.HRELAP_LeaveReason,
                                       HRELAPA_Remarks = rw.HRELAPA_Remarks,
                                       HRELAP_SupportingDocument = c.HRELAP_SupportingDocument
                                   }
                            ).OrderByDescending(d => d.HRELAP_Id).Distinct().ToArray();

                return data;
            }
            catch (Exception ex)
            {

            }
            return data;
        }

        public LeaveCreditDTO getemployeeadmin(LeaveCreditDTO data)
        {
            try
            {
                data.online_leave = (from a in _lmContext.HR_Master_Employee_DMO
                                     where (a.MI_Id == data.MI_Id && a.HRME_ActiveFlag == true && a.HRME_LeftFlag == false)
                                     select new LeaveCreditDTO
                                     {
                                         HRME_Id = a.HRME_Id,
                                         //HRME_EmployeeFirstName = a.HRME_EmployeeFirstName,
                                         //HRME_EmployeeMiddleName = a.HRME_EmployeeMiddleName,
                                         //HRME_EmployeeLastName = a.HRME_EmployeeLastName,
                                         HRME_EmployeeFirstName = a.HRME_EmployeeFirstName + (a.HRME_EmployeeMiddleName == null || a.HRME_EmployeeMiddleName == " " || a.HRME_EmployeeMiddleName == "0" ? " " : a.HRME_EmployeeMiddleName) + (a.HRME_EmployeeLastName == null || a.HRME_EmployeeLastName == " " || a.HRME_EmployeeLastName == "0" ? " " : a.HRME_EmployeeLastName),
                                         HRME_EmployeeCode = a.HRME_EmployeeCode
                                     }).Distinct().ToArray();
            }
            catch (Exception ex)
            {

            }
            return data;
        }

        public LeaveCreditDTO deactivate(LeaveCreditDTO dto)
        {
            dto.retrunMsg = "";
            try
            {
                var contactExistsP = _lmContext.Database.ExecuteSqlCommand("HR_Employee_Leave_Delete @p0", dto.HRELAP_Id);
                if (contactExistsP > 0)
                {
                    dto.retrunMsg = "updated";
                }
                else
                {
                    dto.retrunMsg = "notUpdated";
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
                dto.retrunMsg = "Error occured";
            }

            return dto;
        }
        public LeaveCreditDTO requestleave(LeaveCreditDTO data)
        {
            try
            {
                data.returnmsg = "";
                data.returnval = false;
                HR_Emp_Leave_ApplicationDMO objpge = Mapper.Map<HR_Emp_Leave_ApplicationDMO>(data);
                if (data.HRML_LeaveType == "COMPOFF")
                {
                    data.HRML_Id = _lmContext.HR_Master_Leave_DMO.Where(t => t.HRML_LeaveType == "Comp_Off" && t.MI_Id == data.MI_Id).Select(t => t.HRML_Id).FirstOrDefault();
                }
                else if (data.HRML_LeaveType == "OD")
                {
                    data.HRML_Id = _lmContext.HR_Master_Leave_DMO.Where(t => t.HRML_LeaveCode == "OD" && t.MI_Id == data.MI_Id).Select(t => t.HRML_Id).FirstOrDefault();
                }

                if (data.HRML_Id > 0)
                {
                    var qyery1 = (from q in _db.Staff_User_Login
                                  from r in _db.HR_Master_Employee_DMO
                                  where (q.Emp_Code == r.HRME_Id && q.Id == data.UserId)
                                  select new LeaveCreditDTO
                                  {
                                      HRME_Id = r.HRME_Id,
                                  }).Distinct().ToArray();

                    var checkduplicate = (from m in _lmContext.HR_Emp_Leave_ApplicationDMO
                                          from n in _lmContext.HR_Emp_Leave_Appl_DetailsDMO
                                          where (m.HRELAP_Id == n.HRELAP_Id && n.HRML_Id == data.HRML_Id && m.MI_Id == data.MI_Id && m.HRME_Id == qyery1.FirstOrDefault().HRME_Id && m.HRELAP_FromDate == data.HRELAP_FromDate && m.HRELAP_ToDate == data.HRELAP_ToDate && m.HRELAP_ApplicationStatus != "Rejected")
                                          select new LeaveCreditDTO
                                          {
                                              HRELAP_Id = m.HRELAP_Id
                                          }).ToList();

                    if (checkduplicate.Count == 0)
                    {
                        //Leave Id Creation from transaction numbering.
                        var transnumconfigsettings = _db.Master_Numbering.Where(d => d.MI_Id == data.MI_Id && d.IMN_Flag.Equals("LeaveNo")).ToList();
                        if (transnumconfigsettings.Count > 0)
                        {
                            GenerateTransactionNumbering a = new GenerateTransactionNumbering(_db);
                            Master_NumberingDTO num = new Master_NumberingDTO();
                            num.MI_Id = data.MI_Id;
                            num.ASMAY_Id = data.asmay_id;
                            num.IMN_AutoManualFlag = transnumconfigsettings.FirstOrDefault().IMN_AutoManualFlag;
                            num.IMN_DuplicatesFlag = transnumconfigsettings.FirstOrDefault().IMN_DuplicatesFlag;
                            num.IMN_StartingNo = transnumconfigsettings.FirstOrDefault().IMN_StartingNo;
                            num.IMN_WidthNumeric = transnumconfigsettings.FirstOrDefault().IMN_WidthNumeric;
                            num.IMN_ZeroPrefixFlag = transnumconfigsettings.FirstOrDefault().IMN_ZeroPrefixFlag;
                            num.IMN_PrefixAcadYearCode = transnumconfigsettings.FirstOrDefault().IMN_PrefixAcadYearCode;
                            num.IMN_PrefixFinYearCode = transnumconfigsettings.FirstOrDefault().IMN_PrefixFinYearCode;
                            num.IMN_PrefixCalYearCode = transnumconfigsettings.FirstOrDefault().IMN_PrefixCalYearCode;
                            num.IMN_PrefixParticular = transnumconfigsettings.FirstOrDefault().IMN_PrefixParticular;
                            num.IMN_SuffixAcadYearCode = transnumconfigsettings.FirstOrDefault().IMN_SuffixAcadYearCode;
                            num.IMN_SuffixFinYearCode = transnumconfigsettings.FirstOrDefault().IMN_SuffixFinYearCode;
                            num.IMN_SuffixCalYearCode = transnumconfigsettings.FirstOrDefault().IMN_SuffixCalYearCode;
                            num.IMN_SuffixParticular = transnumconfigsettings.FirstOrDefault().IMN_SuffixParticular;
                            num.IMN_RestartNumFlag = transnumconfigsettings.FirstOrDefault().IMN_RestartNumFlag;
                            num.IMN_Flag = "LeaveNo";
                            data.HRELAP_ApplicationID = a.GenerateNumber(num);
                        }


                        //LEAVE APPLICATION
                        objpge.HRME_Id = qyery1.FirstOrDefault().HRME_Id;
                        objpge.CreatedDate = DateTime.Now;
                        objpge.UpdatedDate = DateTime.Now;
                        objpge.HRELAP_CreatedBy = data.UserId;
                        objpge.HRELAP_UpdatedBy = data.UserId;
                        objpge.HRELAP_ActiveFlag = true;
                        objpge.HRELAP_ApplicationID = data.HRELAP_ApplicationID != null ? data.HRELAP_ApplicationID : "";
                        objpge.HRELAP_ApplicationStatus = "Requested";
                        objpge.HRELAP_SanctioningLevel = "1";
                        objpge.HRELAP_CompOffCreditApplFlg = false;
                        objpge.HRELAP_FromDate = Convert.ToDateTime(data.HRELAP_FromDate);
                        objpge.HRELAP_ToDate = Convert.ToDateTime(data.HRELAP_ToDate);
                        objpge.HRELAP_SupportingDocument = data.HRELT_SupportingDocument;
                        _lmContext.Add(objpge);
                        _lmContext.SaveChanges();

                        //LEAVE APPLICATION DETAILS
                        if (objpge.HRELAP_Id > 0)
                        {
                            HR_Emp_Leave_Appl_DetailsDMO updatedet = new HR_Emp_Leave_Appl_DetailsDMO();
                            updatedet.HRELAPD_FromDate = data.HRELAP_FromDate;
                            updatedet.HRELAPD_LeaveStatus = "Applied";
                            updatedet.HRELAPD_ToDate = data.HRELAP_ToDate;
                            updatedet.HRELAPD_TotalDays = data.HRELAP_TotalDays;
                            updatedet.HRELAP_Id = objpge.HRELAP_Id;
                            updatedet.HRML_Id = data.HRML_Id;
                            updatedet.UpdatedDate = DateTime.Now;
                            updatedet.CreatedDate = DateTime.Now;
                            updatedet.HRELAPD_UpdatedBy = data.UserId;
                            updatedet.HRELAPD_CreatedBy = data.UserId;
                            updatedet.HRELAPD_ActiveFlag = true;
                            updatedet.HRELAPD_InTime = data.HRELAPD_InTime;
                            updatedet.HRELAPD_OutTime = data.HRELAPD_OutTime;
                            _lmContext.Add(updatedet);
                            _lmContext.SaveChanges();
                            data.returnval = true;
                        }
                    }
                    else
                    {
                        data.returnmsg = "Duplicate";
                    }
                }
                else
                {
                    data.returnmsg = "Leave Not Mapped";
                }
            }
            catch (Exception ex)
            {
                data.returnval = false;
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public string sendmail(long MI_Id, string Email, string Template, long UserID,DateTime? LeaveDate)
        {
            try
            {
                Dictionary<string, string> val = new Dictionary<string, string>();
                var template = _db.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_MailActiveFlag == true).ToList();
                if (template.Count == 0)
                {
                    return "Email Template not Mapped to the selected Institution";
                }
                var institutionName = _db.Institution.Where(i => i.MI_Id == MI_Id).ToList();
                var Paramaeters = _db.SMS_MAIL_SAVED_PARAMETER_DMO.Where(i => i.MI_Id == MI_Id && i.ISES_Id == template[0].ISES_Id && i.Flag == "M").Select(d => d.ISMP_ID).ToList();
                var ParamaetersName = _db.SMS_MAIL_PARAMETER_DMO.Where(i => Paramaeters.Contains(i.ISMP_ID)).ToList();
                string Mailmsg = template.FirstOrDefault().ISES_MailBody;
                List<Match> variables = new List<Match>();

                foreach (Match match in Regex.Matches(Mailmsg, @"\[.*?\]"))
                {
                    variables.Add(match);
                }

                var employeename = _lmContext.HR_Master_Employee_DMO.Where(p => p.HRME_Id == UserID && p.MI_Id == MI_Id).Select(p => new {p.HRME_EmployeeFirstName,p.HRME_EmployeeMiddleName,p.HRME_EmployeeLastName}).FirstOrDefault();
                string Empname = employeename.HRME_EmployeeFirstName + " " + employeename.HRME_EmployeeMiddleName + " " + employeename.HRME_EmployeeLastName;
                Mailmsg = Mailmsg.Replace("[NAME]", Empname);
                DateTime date = DateTime.Parse(LeaveDate.ToString());
                Mailmsg = Mailmsg.Replace("[DATE]", date.ToString("dd/MM/yyyy"));

                List<EMAIL_DETAILS_DMO> alldetails = new List<EMAIL_DETAILS_DMO>();
                alldetails = _db.EMAIL_DETAILS_DMO.Where(t => t.MI_ID.Equals(MI_Id)).ToList();

                string Attechement = "";
                if (alldetails.Count > 0)
                {
                    string SendingEmail = alldetails[0].IVRMMD_Mail_ID;
                    string SendingEmailPassword = alldetails[0].IVRMMD_Mail_PASSWORD;
                    string SendingEmailHostName = alldetails[0].IVRMMD_HOSTNAME;
                    Int32 PortNumber = Convert.ToInt32(alldetails[0].IVRMMD_PORTNO);
                    string Subject = template[0].ISES_MailSubject.ToString();
                    string sengridkey = alldetails[0].IVRM_sendgridkey.ToString();
                    string mailcc = "";
                    string mailbcc = "";
                    if (alldetails[0].IVRM_mailcc != null && alldetails[0].IVRM_mailcc != "")
                    {
                        string[] ccmail = alldetails[0].IVRM_mailcc.Split(',');

                        mailcc = ccmail[0].ToString();

                        if (ccmail.Length > 1)
                        {
                            if (ccmail[1] != null || ccmail[1] != "")
                            {
                                mailbcc = ccmail[1].ToString();
                            }
                        }

                    }
                    if (alldetails[0].IVRMMD_Attechement != null && alldetails[0].IVRMMD_Attechement != "")
                    {
                        Attechement = alldetails[0].IVRMMD_Attechement.ToString();
                    }

                    var message = new SendGridMessage();
                    message.From = new EmailAddress(SendingEmail, institutionName[0].MI_Name);
                    message.Subject = Subject;
                    message.AddTo(Email);

                    if (Attechement.Equals("1"))
                    {
                        var img = _db.IVRM_EMAIL_ATT_DMO.Where(i => i.ISES_Id == template[0].ISES_Id).ToList();

                        if (img.Count > 0)
                        {
                            for (int i = 0; i < img.Count; i++)
                            {
                                System.Net.HttpWebRequest request = System.Net.WebRequest.Create(img[i].IVRM_Att_Path) as HttpWebRequest;
                                System.Net.HttpWebResponse response = request.GetResponseAsync().Result as System.Net.HttpWebResponse;
                                Stream stream = response.GetResponseStream();
                                message.AddAttachment(stream.ToString(), img[i].IVRM_Att_Name);
                            }
                        }
                    }

                    if (mailcc != null && mailcc != "")
                    {
                        message.AddCc(mailcc);
                    }
                    if (mailbcc != null && mailbcc != "")
                    {
                        message.AddBcc(mailbcc);
                    }
                    message.HtmlContent = Mailmsg;
                    var client = new SendGridClient(sengridkey);
                    client.SendEmailAsync(message).Wait();
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
                            Value = Mailmsg
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

        public async Task<string> sendSms(long MI_Id, long mobileNo, string Template, long UserID, DateTime? LeaveDate)
        {
            try
            {
                Dictionary<string, string> val = new Dictionary<string, string>();
                var template = _db.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_SMSActiveFlag == true).ToList();
                if (template.Count == 0)
                {
                    return "SMS Template not Mapped to the selected Institution";
                }

                var institutionName = _db.Institution.Where(i => i.MI_Id == MI_Id).ToList();
                var Paramaeters = _db.SMS_MAIL_SAVED_PARAMETER_DMO.Where(i => i.MI_Id == MI_Id && i.ISES_Id == template[0].ISES_Id && i.Flag == "S").Select(d => d.ISMP_ID).ToList();

                var ParamaetersName = _db.SMS_MAIL_PARAMETER_DMO.Where(i => Paramaeters.Contains(i.ISMP_ID)).ToList();
                string sms = template.FirstOrDefault().ISES_SMSMessage;

                var employeename = _lmContext.HR_Master_Employee_DMO.Where(p => p.HRME_Id == UserID && p.MI_Id == MI_Id).Select(p => new { p.HRME_EmployeeFirstName, p.HRME_EmployeeMiddleName, p.HRME_EmployeeLastName }).FirstOrDefault();
                string Empname = employeename.HRME_EmployeeFirstName + " " + employeename.HRME_EmployeeMiddleName + " " + employeename.HRME_EmployeeLastName;
                sms = sms.Replace("[NAME]", Empname);
                DateTime date = DateTime.Parse(LeaveDate.ToString());
                sms = sms.Replace("[DATE]", date.ToString("dd/MM/yyyy"));

                List<Match> variables = new List<Match>();
                foreach (Match match in Regex.Matches(sms, @"\[.*?\]"))
                {
                    variables.Add(match);
                }

                List<SMS_DETAILS_DMO> alldetails = new List<SMS_DETAILS_DMO>();
                alldetails = _db.SMS_DETAILS_DMO.Where(t => t.MI_ID.Equals(MI_Id)).ToList();
                if (alldetails.Count > 0)
                {
                    string url = alldetails[0].IVRMSD_URL.ToString();
                    string PHNO = mobileNo.ToString();
                    url = url.Replace("PHNO", PHNO);
                    url = url.Replace("MESSAGE", sms);

                    System.Net.HttpWebRequest request = System.Net.WebRequest.Create(url) as HttpWebRequest;
                    System.Net.HttpWebResponse response = await request.GetResponseAsync() as System.Net.HttpWebResponse;
                    Stream stream = response.GetResponseStream();

                    StreamReader readStream = new StreamReader(stream, Encoding.UTF8);
                    string responseparameters = readStream.ReadToEnd();
                    var myContent = JsonConvert.SerializeObject(responseparameters);

                    dynamic responsedata = JsonConvert.DeserializeObject(myContent);
                    string messageid = responsedata;

                    #region
                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        var template1010 = _db.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_SMSActiveFlag == true).Select(d => d.IVRMIM_Id).ToList();
                        var moduleid = _db.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(d => d.IVRMM_Id).ToList();
                        var modulename = _db.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(d => d.IVRMM_ModuleName).ToList();
                        cmd.CommandText = "IVRM_SMS_Outgoing";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MobileNo",
                            SqlDbType.NVarChar)
                        {
                            Value = PHNO
                        });
                        cmd.Parameters.Add(new SqlParameter("@Message",
                           SqlDbType.NVarChar)
                        {
                            Value = sms
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
                        cmd.Parameters.Add(new SqlParameter("@status",
                   SqlDbType.VarChar)
                        {
                            Value = "Delivered"
                        });
                        cmd.Parameters.Add(new SqlParameter("@Message_id",
                SqlDbType.VarChar)
                        {
                            Value = messageid
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
                    #endregion
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return ex.Message;
            }
            return "success";
        }

        public string callnotification(string devicenew, long HRELAP_Id, long mi_id, LeaveCreditDTO dto, string header_flg)
        {
            try
            {
                var key = _lmContext.MobileApplAuthenticationDMO.Single(a => a.MI_Id == mi_id).MAAN_AuthenticationKey;

                LeaveCreditDTO data = new LeaveCreditDTO();
                var leavedetails = _lmContext.HR_Emp_Leave_ApplicationDMO.Where(h => h.MI_Id == mi_id && h.HRELAP_ActiveFlag == true && h.HRELAP_Id == HRELAP_Id).Distinct().ToList();

                string url = "";
                url = "https://fcm.googleapis.com/fcm/send";

                List<string> notificationparams = new List<string>();
                string daata = "";

                string sound = "default";
                string notId = "4";

                daata = "{ " + '"' + "registration_ids" + '"' + ":" + devicenew + "," +
               "" + '"' + "notification" + '"' + " : " + "{" + '"' + "icon" + '"' + "" + ":" + '"' + "ic_notification_icon" + '"' + " , " + '"' + "title" + '"' + ":" + '"' + "Classwork" + '"' + " ,  " + '"' + "sound" + '"' + "" + ":" + '"' + sound + '"' + " , " + '"' + "notId" + '"' + "" + ":" + '"' + notId + '"' + " , " + '"' + "body" + '"' + ":" + '"' + leavedetails.FirstOrDefault().HRELAP_LeaveReason + '"' + ", " + '"' + "iconcolor" + '"' + ":" + '"' + "#692a7b" + '"' + " } }";

                notificationparams.Add(daata.ToString());

                // var mycontent = JsonConvert.SerializeObject(notificationparams);
                var mycontent = notificationparams[0];
                string postdata = mycontent.ToString();
                HttpWebRequest connection = (HttpWebRequest)WebRequest.Create(url);
                connection.ContentType = "application/json";
                connection.MediaType = "application/json";
                connection.Accept = "application/json";

                connection.Method = "post";
                connection.Headers["authorization"] = "key=" + key;

                using (StreamWriter requestwriter = new StreamWriter(connection.GetRequestStream()))
                {
                    requestwriter.Write(postdata);
                }
                string responsedata = string.Empty;

                using (StreamReader responsereader = new StreamReader(connection.GetResponse().GetResponseStream()))
                {
                    responsedata = responsereader.ReadToEnd();
                    JObject joresponse1 = JObject.Parse(responsedata);
                }

                PushNotification push_noti = new PushNotification(_db);
                push_noti.Insert_PushNotification_leaveapply(HRELAP_Id, mi_id, devicenew, dto, header_flg);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return ex.Message;
            }
            return "success";
        }


        //-------------------------------periodwiseleave--------------------------------//////////////////////////////////////////////////////
        public LeaveCreditDTO getdetails(LeaveCreditDTO data)
        {
            try
            {
                data.Time_Table = _ttcontext.TT_Master_PeriodDMO.Where(t => t.MI_Id.Equals(data.MI_Id) && t.TTMP_ActiveFlag == true).ToList().ToArray();
                data.stafflist = (from t in _ttcontext.HR_Master_Employee_DMO
                                  where(t.MI_Id.Equals(data.MI_Id) && t.HRME_ActiveFlag == true && t.HRME_LeftFlag == false)
                                  select new LeaveCreditDTO
                                  {
                                      HRME_Id=t.HRME_Id,
                                      HRME_EmployeeFirstName = t.HRME_EmployeeFirstName + (string.IsNullOrEmpty(t.HRME_EmployeeMiddleName) ? "" : ' ' + t.HRME_EmployeeMiddleName) + (string.IsNullOrEmpty(t.HRME_EmployeeLastName) ? "" : ' ' + t.HRME_EmployeeLastName)
                                  }).ToArray();
                //data.classlist = _ttcontext.School_M_Class.Where(t => t.MI_Id == data.MI_Id && t.ASMCL_ActiveFlag == true).ToList().ToArray();
                long yearid = _lmContext.HR_Master_LeaveYear_DMO.Where(t => t.HRMLY_FromDate <= DateTime.Today && t.HRMLY_ToDate >= DateTime.Today && t.HRMLY_ActiveFlag == true && t.MI_Id == data.MI_Id).Select(t => t.HRMLY_Id).FirstOrDefault();
                List<MasterAcademic> allyear = new List<MasterAcademic>();
                allyear = _db.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).ToList();
                data.acayear = allyear.OrderByDescending(y => y.ASMAY_Order).Distinct().ToArray();

                List<MasterAcademic> defaultyear = new List<MasterAcademic>();
                defaultyear = _db.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true && t.ASMAY_Id == data.asmay_id).ToList();
                data.academicListdefault = defaultyear.OrderByDescending(a => a.ASMAY_Order).ToArray();



                var qyery1 = (from q in _db.Staff_User_Login
                              from r in _db.HR_Master_Employee_DMO
                              where (q.Emp_Code == r.HRME_Id && q.Id == data.UserId)
                              select new LeaveCreditDTO
                              {
                                  HRME_Id = r.HRME_Id,
                              }).Distinct().ToArray();


                if (qyery1.Length > 0)
                {



                    data.leave_name = (from x in _lmContext.HR_Master_Leave_DMO
                                       from y in _lmContext.HR_Emp_Leave_StatusDMO
                                       where (x.HRML_Id == y.HRML_Id && x.MI_Id == data.MI_Id && x.HRML_LeaveCreditFlg == true && y.HRME_Id == qyery1.FirstOrDefault().HRME_Id && y.HRMLY_Id == yearid)
                                       select new LeaveCreditDTO
                                       {
                                           HRELS_Id = y.HRELS_Id,
                                           HRML_LeaveName = x.HRML_LeaveName,
                                           HRML_LeaveCode = x.HRML_LeaveCode,
                                           HRELS_TotalLeaves = y.HRELS_TotalLeaves,
                                           HRELS_CreditedLeaves = y.HRELS_CreditedLeaves,
                                           HRELS_TransLeaves = y.HRELS_TransLeaves,
                                           HRELS_CBLeaves = y.HRELS_CBLeaves,
                                           HRML_Id = y.HRML_Id,
                                           HRME_Id = y.HRME_Id,
                                           HRML_WhenToApplyFlg = x.HRML_WhenToApplyFlg,
                                           HRML_NoOfDays = x.HRML_NoOfDays
                                       }).Distinct().ToArray();


                    data.online_leave = (from a in _lmContext.HR_Master_Employee_DMO
                                         from b in _lmContext.HR_Master_Designation_DMO
                                         from c in _lmContext.HR_Emp_Leave_StatusDMO
                                         from d in _lmContext.Emp_Email_Id
                                         from e in _lmContext.Emp_MobileNo
                                         where (a.HRMDES_Id == b.HRMDES_Id && a.HRME_Id == qyery1.FirstOrDefault().HRME_Id && d.HRME_Id == a.HRME_Id && e.HRME_Id == a.HRME_Id && d.HRMEM_DeFaultFlag == "default" && e.HRMEMNO_DeFaultFlag == "default")
                                         select new LeaveCreditDTO
                                         {
                                             HRME_Id = a.HRME_Id,
                                             HRME_EmployeeFirstName = a.HRME_EmployeeFirstName + " " + (a.HRME_EmployeeMiddleName == null || a.HRME_EmployeeMiddleName == " " || a.HRME_EmployeeMiddleName == "0" ? " " : a.HRME_EmployeeMiddleName) + " " + (a.HRME_EmployeeLastName == null || a.HRME_EmployeeLastName == " " || a.HRME_EmployeeLastName == "0" ? " " : a.HRME_EmployeeLastName),
                                             HRME_DOJ = a.HRME_DOJ,
                                             HRME_MobileNo = e.HRMEMNO_MobileNo,
                                             HRME_EmailId = d.HRMEM_EmailId,
                                             HRMDES_DesignationName = b.HRMDES_DesignationName,
                                             HRME_EmployeeCode = a.HRME_EmployeeCode
                                         }).Distinct().ToArray();

                    //data.empleavename = (from a in _lmContext.HR_Master_Leave_DMO
                    //                     from b in _lmContext.HR_Emp_Leave_Appl_DetailsDMO
                    //                     from c in _lmContext.HR_Emp_Leave_ApplicationDMO
                    //                     join d in _lmContext.HR_Emp_Leave_Appl_AuthorisationDMO on c.HRELAP_Id equals d.HRELAP_Id into FULLTABLE
                    //                     from rw in FULLTABLE.DefaultIfEmpty()
                    //                     where (a.HRML_Id == b.HRML_Id && c.HRELAP_Id == b.HRELAP_Id && a.MI_Id == data.MI_Id && c.HRME_Id == qyery1.FirstOrDefault().HRME_Id)
                    //                     select new LeaveCreditDTO
                    //                     {
                    //                         HRELAP_Id = c.HRELAP_Id,
                    //                         HRML_LeaveName = a.HRML_LeaveName,
                    //                         HRELAP_FromDate = c.HRELAP_FromDate,
                    //                         HRELAP_ToDate = c.HRELAP_ToDate,
                    //                         HRELTD_TotDays = b.HRELAPD_TotalDays,
                    //                         HRELAP_TotalDays = b.HRELAPD_TotalDays,
                    //                         HRELAP_ApplicationStatus = b.HRELAPD_LeaveStatus,
                    //                         HRML_Id = b.HRML_Id,
                    //                         HRELAP_LeaveReason = c.HRELAP_LeaveReason,
                    //                         HRELAPA_Remarks = rw.HRELAPA_Remarks,
                    //                         HRELAP_SupportingDocument = c.HRELAP_SupportingDocument
                    //                     }).OrderByDescending(d => d.HRELAP_Id).Distinct().ToArray();

                    using (var cmd = _lmContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "EMPLOYEE_LEAVE_DETAILS_Grid";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                        cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.VarChar) { Value = qyery1.FirstOrDefault().HRME_Id });

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
                                    for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                                    {
                                        dataRow.Add(
                                            dataReader.GetName(iFiled),
                                            dataReader.IsDBNull(iFiled) ? 0 : dataReader[iFiled]
                                        );
                                    }
                                    retObject.Add((ExpandoObject)dataRow);
                                }
                            }
                            data.empleavename = retObject.ToArray();
                        }

                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                

                }

                using (var cmd = _lmContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "LeaveApprovalLevelsDetails";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.VarChar) { Value = qyery1.FirstOrDefault().HRME_Id });

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
                                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                                {
                                    dataRow.Add(
                                        dataReader.GetName(iFiled),
                                        dataReader.IsDBNull(iFiled) ? 0 : dataReader[iFiled]
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.getemployeeleavedetails = retObject.ToArray();
                    }

                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }


                using (var cmd = _lmContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "IVRM_PeriodWiseLeaveapplygrid";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@userid", SqlDbType.VarChar) { Value = data.UserId });

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
                                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                                {
                                    dataRow.Add(
                                        dataReader.GetName(iFiled),
                                        dataReader.IsDBNull(iFiled) ? 0 : dataReader[iFiled]
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.appliedgrid = retObject.ToArray();
                    }

                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
            return data;

        }

        public LeaveCreditDTO getabsentstaff(LeaveCreditDTO data)
        {
            try
            {
                using (var cmd = _lmContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TT_SCHOOL_GET_ABSENTSTAFF_FOR_DEPUTATION";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.ASMAY_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@StaffDate",
                    SqlDbType.Date)
                    {
                        Value = data.TTSD_Date
                    });

                    cmd.Parameters.Add(new SqlParameter("@FLAG",
                   SqlDbType.Bit)
                    {
                        Value = data.absflag
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
                                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                                {
                                    dataRow.Add(
                                    dataReader.GetName(iFiled),
                                    dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.stafflist = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

            }
            catch (Exception ee)
            {

                Console.WriteLine(ee.Message);
            }
            return data;

        }

        public LeaveCreditDTO get_free_stfdets(LeaveCreditDTO data)
        {

            LeaveCreditDTO objpge = Mapper.Map<LeaveCreditDTO>(data);
            List<LeaveCreditDTO> list = new List<LeaveCreditDTO>();
            try
            {

                objpge.periodslst = _ttcontext.TT_Master_PeriodDMO.Where(c => c.TTMP_ActiveFlag.Equals(true) && c.MI_Id.Equals(objpge.MI_Id)).ToList().ToArray();

                objpge.gridweeks = _ttcontext.TT_Master_DayDMO.Where(t => t.MI_Id.Equals(objpge.MI_Id) && t.TTMD_ActiveFlag == true).ToArray();


                using (var cmd = _lmContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TT_SCHOOL_GET_DEPUTATION_LIST_EMPLOYEEWISE";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                    SqlDbType.BigInt)
                    {
                        Value = objpge.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                    SqlDbType.BigInt)
                    {
                        Value = objpge.ASMAY_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@TTMD_Id",
                    SqlDbType.NVarChar)
                    {
                        Value = objpge.TTMD_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@TTMP_Id",
                   SqlDbType.NVarChar)
                    {
                        Value = objpge.TTMP_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id",
                   SqlDbType.BigInt)
                    {
                        Value = objpge.HRME_Id
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
                                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                                {
                                    dataRow.Add(
                                    dataReader.GetName(iFiled),
                                    dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        objpge.Time_Table_substitute = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }


                using (var cmd = _lmContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TT_StaffDeputationCount";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                    SqlDbType.BigInt)
                    {
                        Value = objpge.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                    SqlDbType.BigInt)
                    {
                        Value = objpge.ASMAY_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@StaffDate",
                    SqlDbType.Date)
                    {
                        Value = objpge.TTSD_Date
                    });


                    cmd.Parameters.Add(new SqlParameter("@TTMD_Id",
                    SqlDbType.NVarChar)
                    {
                        Value = objpge.TTMD_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@TTMP_Id",
                   SqlDbType.NVarChar)
                    {
                        Value = objpge.TTMP_Id
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
                                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                                {
                                    dataRow.Add(
                                    dataReader.GetName(iFiled),
                                    dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        objpge.dpcount = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }





                using (var cmd = _lmContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TT_AbsentStaffDeputationCount_ALL";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                    SqlDbType.BigInt)
                    {
                        Value = objpge.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                    SqlDbType.BigInt)
                    {
                        Value = objpge.ASMAY_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@StaffDate",
                    SqlDbType.Date)
                    {
                        Value = objpge.TTSD_Date
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
                                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                                {
                                    dataRow.Add(
                                    dataReader.GetName(iFiled),
                                    dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        objpge.absentstfcnt = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                using (var cmd = _lmContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "GET_DEPUTATION_WEEKLY_PERIOD_COUNT";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                    SqlDbType.BigInt)
                    {
                        Value = objpge.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                    SqlDbType.BigInt)
                    {
                        Value = objpge.ASMAY_Id
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
                                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                                {
                                    dataRow.Add(
                                    dataReader.GetName(iFiled),
                                    dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        objpge.weeklycntlist = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }


            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }


            return data;
        }
      public LeaveCreditDTO get_period_alloted(LeaveCreditDTO data)
        {
            LeaveCreditDTO objpge = Mapper.Map<LeaveCreditDTO>(data);
            List<LeaveCreditDTO> list = new List<LeaveCreditDTO>();
            var qyery1 = (from q in _db.Staff_User_Login
                          from r in _db.HR_Master_Employee_DMO
                          where (q.Emp_Code == r.HRME_Id && q.Id == data.UserId)
                          select new LeaveCreditDTO
                          {
                              HRME_Id = r.HRME_Id,
                          }).Distinct().ToArray();
            try
            {

                objpge.periodslst = _ttcontext.TT_Master_PeriodDMO.Where(c => c.TTMP_ActiveFlag.Equals(true) && c.MI_Id.Equals(objpge.MI_Id)).ToList().ToArray();

                objpge.gridweeks = _ttcontext.TT_Master_DayDMO.Where(t => t.MI_Id.Equals(objpge.MI_Id) && t.TTMD_ActiveFlag == true).ToArray();

                objpge.Time_Table = (from a in _ttcontext.TT_Master_DayDMO
                                     from b in _ttcontext.TT_Master_PeriodDMO
                                     from c in _ttcontext.School_M_Class
                                     from d in _ttcontext.School_M_Section
                                     from e in _ttcontext.IVRM_School_Master_SubjectsDMO
                                     from f in _ttcontext.HR_Master_Employee_DMO
                                     from g in _ttcontext.TT_Final_GenerationDMO
                                     from h in _ttcontext.TT_Final_Generation_DetailedDMO
                                     from ii in _ttcontext.TTMasterCategoryDMO
                                     where (g.MI_Id == objpge.MI_Id && a.MI_Id == g.MI_Id && b.MI_Id == g.MI_Id && c.MI_Id == g.MI_Id && d.MI_Id == g.MI_Id && e.MI_Id == g.MI_Id && f.MI_Id == g.MI_Id && ii.MI_Id == g.MI_Id && ii.TTMC_Id == g.TTMC_Id && h.TTFG_Id == g.TTFG_Id && c.ASMCL_Id == h.ASMCL_Id && d.ASMS_Id == h.ASMS_Id && e.ISMS_Id == h.ISMS_Id && h.HRME_Id == qyery1.FirstOrDefault().HRME_Id && f.HRME_Id == h.HRME_Id && a.TTMD_Id == h.TTMD_Id && b.TTMP_Id == h.TTMP_Id  && g.ASMAY_Id == objpge.ASMAY_Id && h.TTMD_Id == objpge.TTMD_Id)
                                     select new TTDeputationDTO
                                     {
                                         TTFGD_Id = h.TTFGD_Id,
                                         TTFG_Id = g.TTFG_Id,
                                         ASMCL_Id = h.ASMCL_Id,
                                         ASMS_Id = h.ASMS_Id,
                                         HRME_Id = h.HRME_Id,
                                         ISMS_Id = h.ISMS_Id,
                                         TTMD_Id = h.TTMD_Id,
                                         TTMP_Id = h.TTMP_Id,
                                         TTMC_Id = g.TTMC_Id,
                                         ASMAY_Id = g.ASMAY_Id,
                                         TTMD_DayName = a.TTMD_DayName,
                                         TTMP_PeriodName = b.TTMP_PeriodName,
                                         ASMCL_ClassName = c.ASMCL_ClassName,
                                         ASMC_SectionName = d.ASMC_SectionName,
                                         staffName = f.HRME_EmployeeFirstName + (f.HRME_EmployeeMiddleName == null || f.HRME_EmployeeMiddleName == " " || f.HRME_EmployeeMiddleName == "0" ? " " : f.HRME_EmployeeMiddleName) + (f.HRME_EmployeeLastName == null || f.HRME_EmployeeLastName == " " || f.HRME_EmployeeLastName == "0" ? " " : f.HRME_EmployeeLastName),
                                         ISMS_SubjectName = e.ISMS_SubjectName,
                                         TTMC_CategoryName = ii.TTMC_CategoryName,

                                     }
                                  ).Distinct().OrderBy(f => f.TTMP_Id).ToArray();



                using (var cmd = _lmContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TT_AbsentStaffDeputationCount";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                    SqlDbType.BigInt)
                    {
                        Value = objpge.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                    SqlDbType.BigInt)
                    {
                        Value = objpge.ASMAY_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@StaffDate",
                    SqlDbType.Date)
                    {
                        Value = objpge.TTSD_Date
                    });


                    cmd.Parameters.Add(new SqlParameter("@HRME_Id",
                    SqlDbType.NVarChar)
                    {
                        Value = qyery1.FirstOrDefault().HRME_Id
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
                                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                                {
                                    dataRow.Add(
                                    dataReader.GetName(iFiled),
                                    dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        objpge.absentdpcount = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }


            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }


            return data;
        }

        public LeaveCreditDTO savedetails(LeaveCreditDTO data)
        {
            int count = 0;
            HR_Emp_Leave_ApplicationDMO objpge = Mapper.Map<HR_Emp_Leave_ApplicationDMO>(data);

            HR_Emp_Leave_Trans_DMO objLvTrans = Mapper.Map<HR_Emp_Leave_Trans_DMO>(data);
            HR_Emp_Leave_Trans_Details_DMO objLvTrDetail = Mapper.Map<HR_Emp_Leave_Trans_Details_DMO>(data);

            var leave_year = _lmContext.HR_Master_LeaveYear_DMO.Where(e => e.MI_Id == data.MI_Id && DateTime.Now.Date >= e.HRMLY_FromDate.Date && DateTime.Now.Date <= e.HRMLY_ToDate.Date && e.HRMLY_ActiveFlag == true).ToList();

            var leave_id = _lmContext.HR_Master_Leave_DMO.Where(p => p.HRML_LeaveName == data.temp_table_data.FirstOrDefault().HRML_LeaveName && p.MI_Id == data.MI_Id).Select(p => p.HRML_Id).FirstOrDefault();
            long TransactionID = 0;
            try
            {

                var duplicate = _lmContext.HR_Emp_Leave_ApplicationDMO.Where(z => z.MI_Id == data.MI_Id && z.HRELAP_FromDate >= data.frmToDates[0].HRELAP_FromDate && z.HRELAP_ToDate <= data.frmToDates[0].HRELAP_ToDate && z.HRELAP_ActiveFlag == true && z.HRME_Id == data.HRME_Id).ToArray();
                if (duplicate.Length == 0)
                {
                    if (objpge.HRELAP_Id > 0)
                    {
                        var resultCount = _lmContext.HR_Emp_Leave_ApplicationDMO.Where(t => t.HRELAP_FromDate <= objpge.HRELAP_FromDate && t.HRELAP_ToDate >= objpge.HRELAP_ToDate).Count();

                        if (resultCount == 0)
                        {
                            var result = _lmContext.HR_Emp_Leave_ApplicationDMO.Single(t => t.MI_Id == objpge.MI_Id);

                            result.MI_Id = objpge.MI_Id;
                            result.HRME_Id = objpge.HRME_Id;
                            result.HRELAP_ApplicationDate = objpge.HRELAP_ApplicationDate;
                            result.HRELAP_FromDate = objpge.HRELAP_FromDate;
                            result.HRELAP_ToDate = objpge.HRELAP_ToDate;
                            result.HRELAP_TotalDays = objpge.HRELAP_TotalDays;
                            result.HRELAP_LeaveReason = objpge.HRELAP_LeaveReason;
                            result.HRELAP_ContactNoOnLeave = objpge.HRELAP_ContactNoOnLeave;
                            result.HRELAP_ReportingDate = objpge.HRELAP_ReportingDate;
                            // result.HRELAP_ApplicationID = objpge.HRELAP_ApplicationID;

                            result.UpdatedDate = DateTime.Now;
                            _lmContext.Update(result);
                            var contactExists = _lmContext.SaveChanges();
                            if (contactExists == 1)
                            {
                                data.returnval = true;
                            }
                            else
                            {
                                data.returnval = false;
                            }
                        }
                        else
                        {
                            data.returnduplicatestatus = "Duplicate";
                            return data;
                        }
                    }
                    else
                    {
                        var qyery1 = (from q in _db.Staff_User_Login
                                      from r in _db.HR_Master_Employee_DMO
                                      where (q.Emp_Code == r.HRME_Id && q.Id == data.UserId)
                                      select new LeaveCreditDTO
                                      {
                                          HRME_Id = r.HRME_Id,
                                          LoginId = q.Id
                                      }).Distinct().ToArray();


                        if (qyery1.Length > 0)
                        {

                            var checkbalanceleave = _lmContext.HR_Emp_Leave_StatusDMO.Where(d => d.MI_Id == data.MI_Id && d.HRME_Id == qyery1.FirstOrDefault().HRME_Id && d.HRML_Id == data.temp_table_data.FirstOrDefault().HRML_Id && d.HRMLY_Id == leave_year.FirstOrDefault().HRMLY_Id).ToList();
                            var bal_leave = checkbalanceleave.FirstOrDefault().HRELS_CBLeaves - data.frmToDates.FirstOrDefault().HRELAP_TotalDays;
                            var fromdate = Convert.ToDateTime(data.frmToDates.FirstOrDefault().HRELAP_FromDate).ToString("d");
                            var todate = Convert.ToDateTime(data.frmToDates.FirstOrDefault().HRELAP_ToDate).ToString("d");

                            string leavetype = _lmContext.HR_Master_Leave_DMO.Where(p => p.HRML_Id == data.temp_table_data[0].HRML_Id && p.MI_Id == data.MI_Id).Select(p => p.HRML_LeaveName).FirstOrDefault();

                            if (leavetype == "ON DUTY")
                            {
                                var checkduplicate = (from m in _lmContext.HR_Emp_Leave_ApplicationDMO
                                                      from n in _lmContext.HR_Emp_Leave_Appl_DetailsDMO
                                                      where m.HRELAP_Id == n.HRELAP_Id && m.MI_Id == data.MI_Id && m.HRME_Id == qyery1.FirstOrDefault().HRME_Id
                                                      && m.HRELAP_FromDate.Value.Date.ToString("dd/MM/yyyy") == fromdate
                                                      && m.HRELAP_ToDate.Value.Date.ToString("dd/MM/yyyy") == todate
                                                      && n.HRML_Id == data.temp_table_data.FirstOrDefault().HRML_Id
                                                      select new LeaveCreditDTO
                                                      {
                                                          HRELAP_Id = m.HRELAP_Id
                                                      }
                                                     ).ToList();
                                if (checkduplicate.Count == 0)
                                {


                                    var id = _lmContext.HR_Emp_Leave_Trans_Details_DMO.Where(t => t.HRELTD_FromDate.Date >= Convert.ToDateTime(fromdate).Date && t.HRELTD_ToDate.Date <= Convert.ToDateTime(todate).Date && t.MI_Id == data.MI_Id && t.HRELTD_LWPFlag == true && t.HRME_Id == qyery1.FirstOrDefault().HRME_Id).ToList();
                                    if (id.Count > 0)
                                    {
                                        for (int i = 0; i < id.Count(); i++)
                                        {
                                            id[i].HRELTD_LWPFlag = false;
                                            id[i].UpdatedDate = DateTime.Now;
                                            _lmContext.Update(id[i]);
                                            //  _lmcontext.savechanges();

                                            var hreltdid = id[i].HRELTD_Id;
                                            var hreltid = id[i].HRELT_Id;

                                            var idtrans11 = _lmContext.HR_Emp_Leave_Trans_DMO.Single(t => t.HRELT_Id == hreltid && t.MI_Id == data.MI_Id && t.HRME_Id == qyery1.FirstOrDefault().HRME_Id);
                                            idtrans11.HRELT_LeaveReason = "OD";
                                            idtrans11.HRELT_Status = "Applied";
                                            idtrans11.UpdatedDate = DateTime.Now;
                                            _lmContext.Update(idtrans11);
                                            _lmContext.SaveChanges();

                                        }

                                        //Leave Id Creation from transaction numbering.
                                        var transnumconfigsettings = _db.Master_Numbering.Where(d => d.MI_Id == data.MI_Id && d.IMN_Flag.Equals("LeaveNo")).ToList();
                                        if (transnumconfigsettings.Count > 0)
                                        {
                                            GenerateTransactionNumbering a = new GenerateTransactionNumbering(_db);
                                            Master_NumberingDTO num = new Master_NumberingDTO();
                                            num.MI_Id = data.MI_Id;
                                            num.ASMAY_Id = data.asmay_id;
                                            num.IMN_AutoManualFlag = transnumconfigsettings.FirstOrDefault().IMN_AutoManualFlag;
                                            num.IMN_DuplicatesFlag = transnumconfigsettings.FirstOrDefault().IMN_DuplicatesFlag;
                                            num.IMN_StartingNo = transnumconfigsettings.FirstOrDefault().IMN_StartingNo;
                                            num.IMN_WidthNumeric = transnumconfigsettings.FirstOrDefault().IMN_WidthNumeric;
                                            num.IMN_ZeroPrefixFlag = transnumconfigsettings.FirstOrDefault().IMN_ZeroPrefixFlag;
                                            num.IMN_PrefixAcadYearCode = transnumconfigsettings.FirstOrDefault().IMN_PrefixAcadYearCode;
                                            num.IMN_PrefixFinYearCode = transnumconfigsettings.FirstOrDefault().IMN_PrefixFinYearCode;
                                            num.IMN_PrefixCalYearCode = transnumconfigsettings.FirstOrDefault().IMN_PrefixCalYearCode;
                                            num.IMN_PrefixParticular = transnumconfigsettings.FirstOrDefault().IMN_PrefixParticular;
                                            num.IMN_SuffixAcadYearCode = transnumconfigsettings.FirstOrDefault().IMN_SuffixAcadYearCode;
                                            num.IMN_SuffixFinYearCode = transnumconfigsettings.FirstOrDefault().IMN_SuffixFinYearCode;
                                            num.IMN_SuffixCalYearCode = transnumconfigsettings.FirstOrDefault().IMN_SuffixCalYearCode;
                                            num.IMN_SuffixParticular = transnumconfigsettings.FirstOrDefault().IMN_SuffixParticular;
                                            num.IMN_RestartNumFlag = transnumconfigsettings.FirstOrDefault().IMN_RestartNumFlag;
                                            num.IMN_Flag = "LeaveNo";
                                            data.HRELAP_ApplicationID = a.GenerateNumber(num);
                                        }

                                        //LEAVE APPLICATION
                                        objpge.HRME_Id = qyery1.FirstOrDefault().HRME_Id;
                                        objpge.CreatedDate = DateTime.Now;
                                        objpge.UpdatedDate = DateTime.Now;
                                        objpge.HRELAP_ActiveFlag = true;
                                        objpge.HRELAP_ApplicationID = data.HRELAP_ApplicationID;
                                        objpge.HRELAP_TotalDays = data.frmToDates.FirstOrDefault().HRELAP_TotalDays;
                                        objpge.HRELAP_ApplicationStatus = "Applied";
                                        objpge.HRELAP_SanctioningLevel = "1";
                                        objpge.HRELAP_FromDate = Convert.ToDateTime(data.frmToDates.FirstOrDefault().HRELAP_FromDate);
                                        objpge.HRELAP_ToDate = Convert.ToDateTime(data.frmToDates.FirstOrDefault().HRELAP_ToDate);
                                        objpge.HRELAP_SupportingDocument = data.HRELT_SupportingDocument;
                                        _lmContext.Add(objpge);
                                        count = _lmContext.SaveChanges();


                                        //LEAVE APPLICATION DETAILS
                                        if (objpge.HRELAP_Id > 0)
                                        {
                                            HR_Emp_Leave_Appl_DetailsDMO updatedet = new HR_Emp_Leave_Appl_DetailsDMO();
                                            updatedet.HRELAPD_FromDate = data.frmToDates.FirstOrDefault().HRELAP_FromDate;
                                            updatedet.HRELAPD_LeaveStatus = "Applied";
                                            updatedet.HRELAPD_ToDate = data.frmToDates.FirstOrDefault().HRELAP_ToDate;
                                            updatedet.HRELAPD_TotalDays = data.frmToDates.FirstOrDefault().HRELAP_TotalDays;
                                            updatedet.HRELAP_Id = objpge.HRELAP_Id;
                                            updatedet.HRML_Id = data.temp_table_data.FirstOrDefault().HRML_Id;
                                            updatedet.UpdatedDate = DateTime.Now;
                                            updatedet.CreatedDate = DateTime.Now;
                                            updatedet.HRELAPD_ActiveFlag = true;
                                            //Mapper.Map(objpge1, resultempltransDetails);
                                            _lmContext.Add(updatedet);
                                            _lmContext.SaveChanges();
                                        }
                                    }



                                }



                            }
                            else
                            {
                                if (checkbalanceleave.FirstOrDefault().HRELS_CBLeaves > 0 && bal_leave >= 0)
                                {
                                    var checkduplicate = (from m in _lmContext.HR_Emp_Leave_ApplicationDMO
                                                          from n in _lmContext.HR_Emp_Leave_Appl_DetailsDMO
                                                          where m.HRELAP_Id == n.HRELAP_Id && m.MI_Id == data.MI_Id && m.HRME_Id == qyery1.FirstOrDefault().HRME_Id && m.HRELAP_FromDate == data.frmToDates.FirstOrDefault().HRELAP_FromDate && m.HRELAP_ToDate == data.frmToDates.FirstOrDefault().HRELAP_ToDate
                                                          //&& m.HRELAP_FromDate.Value.Date.ToString("dd/MM/yyyy") == fromdate
                                                          //&& m.HRELAP_ToDate.Value.Date.ToString("dd/MM/yyyy") == todate
                                                          && n.HRML_Id == data.temp_table_data.FirstOrDefault().HRML_Id
                                                          select new LeaveCreditDTO
                                                          {
                                                              HRELAP_Id = m.HRELAP_Id
                                                          }).ToList();

                                    if (checkduplicate.Count == 0)
                                    {
                                        objpge.HRME_Id = qyery1.FirstOrDefault().HRME_Id;
                                        objpge.CreatedDate = DateTime.Now;
                                        objpge.UpdatedDate = DateTime.Now;
                                        objpge.HRELAP_ActiveFlag = true;

                                        objLvTrans.MI_Id = data.MI_Id;
                                        objLvTrans.HRME_Id = qyery1.FirstOrDefault().HRME_Id;
                                        objLvTrans.HRMLY_Id = leave_year.FirstOrDefault().HRMLY_Id;
                                        objLvTrans.HRELT_LeaveId = leave_id;
                                        objLvTrans.HRELT_Reportingdate = data.HRELT_Reportingdate;
                                        objLvTrans.HRELT_LeaveReason = data.HRELT_LeaveReason;
                                        objLvTrans.HRELT_Status = "Applied";
                                        objLvTrans.CreatedDate = DateTime.Now;
                                        objLvTrans.UpdatedDate = DateTime.Now;
                                        objLvTrans.HRELT_ActiveFlag = true;
                                        objLvTrans.HRELT_SupportingDocument = data.HRELT_SupportingDocument;

                                        objLvTrDetail.HRML_Id = data.temp_table_data.FirstOrDefault().HRML_Id;
                                        objLvTrDetail.HRME_Id = qyery1.FirstOrDefault().HRME_Id;
                                        objLvTrDetail.MI_Id = data.MI_Id;
                                        objLvTrDetail.HRELTD_LWPFlag = false;
                                        objLvTrDetail.CreatedDate = DateTime.Now;
                                        objLvTrDetail.UpdatedDate = DateTime.Now;

                                        //Leave Id Creation from transaction numbering.
                                        var transnumconfigsettings = _db.Master_Numbering.Where(d => d.MI_Id == data.MI_Id && d.IMN_Flag.Equals("LeaveNo")).ToList();
                                        if (transnumconfigsettings.Count > 0)
                                        {
                                            GenerateTransactionNumbering a = new GenerateTransactionNumbering(_db);
                                            Master_NumberingDTO num = new Master_NumberingDTO();
                                            num.MI_Id = data.MI_Id;
                                            num.ASMAY_Id = data.asmay_id;
                                            num.IMN_AutoManualFlag = transnumconfigsettings.FirstOrDefault().IMN_AutoManualFlag;
                                            num.IMN_DuplicatesFlag = transnumconfigsettings.FirstOrDefault().IMN_DuplicatesFlag;
                                            num.IMN_StartingNo = transnumconfigsettings.FirstOrDefault().IMN_StartingNo;
                                            num.IMN_WidthNumeric = transnumconfigsettings.FirstOrDefault().IMN_WidthNumeric;
                                            num.IMN_ZeroPrefixFlag = transnumconfigsettings.FirstOrDefault().IMN_ZeroPrefixFlag;
                                            num.IMN_PrefixAcadYearCode = transnumconfigsettings.FirstOrDefault().IMN_PrefixAcadYearCode;
                                            num.IMN_PrefixFinYearCode = transnumconfigsettings.FirstOrDefault().IMN_PrefixFinYearCode;
                                            num.IMN_PrefixCalYearCode = transnumconfigsettings.FirstOrDefault().IMN_PrefixCalYearCode;
                                            num.IMN_PrefixParticular = transnumconfigsettings.FirstOrDefault().IMN_PrefixParticular;
                                            num.IMN_SuffixAcadYearCode = transnumconfigsettings.FirstOrDefault().IMN_SuffixAcadYearCode;
                                            num.IMN_SuffixFinYearCode = transnumconfigsettings.FirstOrDefault().IMN_SuffixFinYearCode;
                                            num.IMN_SuffixCalYearCode = transnumconfigsettings.FirstOrDefault().IMN_SuffixCalYearCode;
                                            num.IMN_SuffixParticular = transnumconfigsettings.FirstOrDefault().IMN_SuffixParticular;
                                            num.IMN_RestartNumFlag = transnumconfigsettings.FirstOrDefault().IMN_RestartNumFlag;
                                            num.IMN_Flag = "LeaveNo";
                                            data.HRELAP_ApplicationID = a.GenerateNumber(num);
                                        }
                                        else
                                        {
                                            data.returnmsg = "Nomapping";
                                        }
                                        if (data.HRELAP_ApplicationID != null)
                                        {
                                            objLvTrans.HRELT_FromDate = Convert.ToDateTime(data.frmToDates.FirstOrDefault().HRELAP_FromDate);
                                            objLvTrans.HRELT_ToDate = Convert.ToDateTime(data.frmToDates.FirstOrDefault().HRELAP_ToDate);
                                            objLvTrans.HRELT_TotDays = data.frmToDates.FirstOrDefault().HRELAP_TotalDays;

                                            objLvTrDetail.HRELTD_FromDate = Convert.ToDateTime(data.frmToDates.FirstOrDefault().HRELAP_FromDate);
                                            objLvTrDetail.HRELTD_ToDate = Convert.ToDateTime(data.frmToDates.FirstOrDefault().HRELAP_ToDate);
                                            objLvTrDetail.HRELTD_TotDays = data.frmToDates.FirstOrDefault().HRELAP_TotalDays;

                                            objpge.HRELAP_ApplicationID = data.HRELAP_ApplicationID;
                                            objpge.HRELAP_TotalDays = data.frmToDates.FirstOrDefault().HRELAP_TotalDays;
                                            objpge.HRELAP_ApplicationStatus = "Applied";
                                            objpge.HRELAP_SanctioningLevel = "1";
                                            objpge.HRELAP_FromDate = Convert.ToDateTime(data.frmToDates.FirstOrDefault().HRELAP_FromDate);
                                            objpge.HRELAP_ToDate = Convert.ToDateTime(data.frmToDates.FirstOrDefault().HRELAP_ToDate);
                                            objpge.HRELAP_SupportingDocument = data.HRELT_SupportingDocument;
                                            _lmContext.Add(objpge);
                                            count = _lmContext.SaveChanges();

                                            _lmContext.Add(objLvTrans);
                                            _lmContext.SaveChanges();

                                            var hrelt_id = _lmContext.HR_Emp_Leave_Trans_DMO.Where(z => z.MI_Id == data.MI_Id && z.HRELT_FromDate == Convert.ToDateTime(data.frmToDates.FirstOrDefault().HRELAP_FromDate) && z.HRELT_ToDate == Convert.ToDateTime(data.frmToDates.FirstOrDefault().HRELAP_ToDate) && z.HRELT_TotDays == (data.frmToDates.FirstOrDefault().HRELAP_TotalDays) && z.HRME_Id == qyery1.FirstOrDefault().HRME_Id && z.HRELT_LeaveId == leave_id && z.HRELT_LeaveReason == data.HRELT_LeaveReason).Select(s => s.HRELT_Id).FirstOrDefault();

                                            objLvTrDetail.HRELT_Id = hrelt_id;
                                            _lmContext.Add(objLvTrDetail);
                                            _lmContext.SaveChanges();
                                        }
                                        var contactExists = count;
                                        HR_Emp_Leave_Appl_DetailsDMO objpge1 = Mapper.Map<HR_Emp_Leave_Appl_DetailsDMO>(data);
                                        TransactionID = objpge.HRELAP_Id;
                                        if (objpge.HRELAP_Id > 0)
                                        {
                                            HR_Emp_Leave_Appl_DetailsDMO updatedet = new HR_Emp_Leave_Appl_DetailsDMO();
                                            updatedet.HRELAPD_FromDate = data.frmToDates.FirstOrDefault().HRELAP_FromDate;
                                            updatedet.HRELAPD_LeaveStatus = "Applied";
                                            updatedet.HRELAPD_ToDate = data.frmToDates.FirstOrDefault().HRELAP_ToDate;
                                            updatedet.HRELAPD_TotalDays = data.frmToDates.FirstOrDefault().HRELAP_TotalDays;
                                            updatedet.HRELAP_Id = objpge.HRELAP_Id;
                                            updatedet.HRML_Id = data.temp_table_data.FirstOrDefault().HRML_Id;
                                            updatedet.UpdatedDate = DateTime.Now;
                                            updatedet.CreatedDate = DateTime.Now;
                                            updatedet.HRELAPD_ActiveFlag = true;
                                            _lmContext.Add(updatedet);
                                            _lmContext.SaveChanges();
                                            if (data.tempemp != null && data.tempemp.Length > 0)
                                            {
                                                foreach (var item in data.tempemp)
                                                {
                                                    if (item.HRME_Id >0)
                                                    {
                                                        HR_Emp_Leave_Application_DeputationDMO result = new HR_Emp_Leave_Application_DeputationDMO();
                                                        result.HRELAPD_Id = updatedet.HRELAPD_Id;
                                                        result.HRELAPDD_Id = data.HRELAPDD_Id;
                                                        result.HRME_Id = item.HRME_Id;
                                                        result.HRELAPDD_Date = item.HRELAPDD_Date;
                                                        result.HRELAPDD_Period = item.HRELAPDD_Period;                                                        
                                                        result.HRELAPDD_ActiveFlag = true;
                                                        result.HRELAPDD_ApprovalFlg = "Applied";
                                                        result.HRELAPDD_UpdatedBy = data.UserId;
                                                        result.HRELAPDD_CreatedBy = data.UserId;
                                                        result.HRELAPDD_CreatedDate = DateTime.Now;
                                                        result.HRELAPDD_UpdatedDate = DateTime.Now;
                                                        _lmContext.Add(result);
                                                        _lmContext.SaveChanges();

                                                    }
                                                }
                                            }
                                            string emailid = _lmContext.Emp_Email_Id.Where(p => p.HRME_Id == checkbalanceleave[0].HRME_Id && p.HRMEM_DeFaultFlag == "default").Select(p => p.HRMEM_EmailId).FirstOrDefault();
                                            long mobileno = _lmContext.Emp_MobileNo.Where(p => p.HRME_Id == checkbalanceleave[0].HRME_Id && p.HRMEMNO_DeFaultFlag == "default").Select(p => p.HRMEMNO_MobileNo).FirstOrDefault();
                                            data.HRME_MobileNo = mobileno;
                                            data.HRME_Id = checkbalanceleave[0].HRME_Id;

                                            if (emailid!=null && emailid.Length>0)
                                            {
                                                sendmail(checkbalanceleave[0].MI_Id, emailid, "Leave_Apply", checkbalanceleave[0].HRME_Id, data.frmToDates.FirstOrDefault().HRELAP_FromDate);
                                            }

                                            if (mobileno != null && mobileno > 0)
                                            {
                                                sendSms(checkbalanceleave[0].MI_Id, mobileno, "Leave_Apply", checkbalanceleave[0].HRME_Id, data.frmToDates.FirstOrDefault().HRELAP_FromDate);
                                            }


                                          

                                            var devicenew = _lmContext.HR_Master_Employee_DMO.Where(t => t.HRME_Id == checkbalanceleave[0].HRME_Id).Select(t => t.HRME_AppDownloadedDeviceId).FirstOrDefault();



                                            if (devicenew != null && devicenew.Length > 0)
                                            {

                                                PushNotification push_noti = new PushNotification(_db);
                                                push_noti.Call_PushNotificationGeneral(devicenew, data.MI_Id, data.UserId, qyery1.FirstOrDefault().LoginId, TransactionID, data.HRELT_LeaveReason, "LeaveStatus", "LeaveApplication");

                                            }

                                            //callnotification(devicenew, objpge.HRELAP_Id, _category.MI_Id, _category, "LEAVE MANAGEMENT");
                                        }
                                        else
                                        {

                                        }
                                    
                                        if (contactExists > 0)
                                        {
                                            data.returnval = true;                                                                                    
                                        }
                                        else
                                        {
                                            data.returnval = false;
                                        }
                                    }
                                    else
                                    {
                                        data.returnduplicatestatus = "Duplicate";
                                    }

                                }
                                else
                                {
                                    data.returnmsg = "You Crossed Your Leave Limits";
                                    List<HR_Emp_Leave_ApplicationDMO> mm_events = new List<HR_Emp_Leave_ApplicationDMO>();
                                    mm_events = _lmContext.HR_Emp_Leave_ApplicationDMO.Where(e => e.MI_Id == data.MI_Id).ToList();
                                    data.master_eventlist = mm_events.ToArray();
                                    return data;
                                }
                            }
                        }
                    }
                }
                else
                {
                    data.returnmsg = "Leave Already applied on these dates";
                    List<HR_Emp_Leave_ApplicationDMO> m_events = new List<HR_Emp_Leave_ApplicationDMO>();
                    m_events = _lmContext.HR_Emp_Leave_ApplicationDMO.Where(e => e.MI_Id == data.MI_Id).ToList();
                    data.master_eventlist = m_events.ToArray();
                }
                
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }


            return data;
        }

        public LeaveCreditDTO updatedetails ( LeaveCreditDTO data)
        {
           
            try
            {
                data.edit_auth = _lmContext.HR_Emp_Leave_Application_DeputationDMO.Where(t => t.HRELAPDD_Id == data.HRELAPDD_Id).ToArray();

                if(data.edit_auth.Length>0)
                {
                   if( data.HRELAPDD_Id>0)
                    {
                        var result1 = _lmContext.HR_Emp_Leave_Application_DeputationDMO.Single(a => a.HRELAPDD_Id == data.HRELAPDD_Id);
                        result1.HRELAPD_Id = data.HRELAPD_Id;                   
                        result1.HRELAPDD_ApprovalFlg = "Staff Changed";
                        result1.HRELAPDD_UpdatedBy = data.UserId;                    
                        result1.HRELAPDD_UpdatedDate = DateTime.Now;
                        _lmContext.Update(result1);
                        _lmContext.SaveChanges();

                    }
                    if (data.HRME_Id >0)
                    {
                        HR_Emp_Leave_Application_DeputationDMO result = new HR_Emp_Leave_Application_DeputationDMO();
                        result.HRELAPD_Id = data.HRELAPD_Id;                        
                        result.HRME_Id = data.HRME_Id;
                        result.HRELAPDD_Date = data.HRELAPDD_Date;
                        result.HRELAPDD_Period = data.HRELAPDD_Period;
                        result.HRELAPDD_ActiveFlag = true;
                        result.HRELAPDD_ApprovalFlg = "Applied";
                        result.HRELAPDD_UpdatedBy = data.UserId;
                        result.HRELAPDD_CreatedBy = data.UserId;
                        result.HRELAPDD_CreatedDate = DateTime.Now;
                        result.HRELAPDD_UpdatedDate = DateTime.Now;
                        _lmContext.Add(result);
                        var contactExists = _lmContext.SaveChanges();
                        if (contactExists == 1)
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
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }

    }
}

