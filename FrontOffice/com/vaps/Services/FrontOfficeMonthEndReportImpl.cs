using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using DomainModel.Model.com.vaps.admission;
using DomainModel.Model.com.vapstech.Birthday;
using DomainModel.Model.com.vapstech.TT;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.admission;
using PreadmissionDTOs.com.vaps.BirthDay;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using CommonLibrary;
using SendGrid.Helpers.Mail;
using SendGrid;
using DataAccessMsSqlServerProvider.com.vapstech.FrontOffice;
using PreadmissionDTOs.com.vaps.HRMS;
using PreadmissionDTOs.com.vaps.FrontOffice;
using System.Threading.Tasks;
using DomainModel.Model.com.vapstech.HRMS;

namespace FrontOfficeHub.com.vaps.Services
{
    public class FrontOfficeMonthEndReportImpl : Interfaces.FrontOfficeMonthEndReportInterface
    {
        int MI_ID = 0;
        private static ConcurrentDictionary<string, BirthDayDTO> _login = new ConcurrentDictionary<string, BirthDayDTO>();
        public DomainModelMsSqlServerContext _db;
        public FOContext _FOContext;
        public HRMSContext _HRMSContext;

        public FrontOfficeMonthEndReportImpl(DomainModelMsSqlServerContext db, FOContext fOContext, HRMSContext HRMSContext)
        {
            _db = db;
            _FOContext = fOContext;
            _HRMSContext = HRMSContext;
        }
        public BirthDayDTO getdata(int id)
        {
            BirthDayDTO dto = new BirthDayDTO();
            try
            {
              
                //  dto.yeardropdown = _db.AcademicYear.Where(d => d.MI_Id == id && d.Is_Active == true).ToArray();
                dto.classDrpDwn = (from m in _db.School_M_Class
                                   where m.MI_Id == id && m.ASMCL_ActiveFlag == true
                                   select new School_M_ClassDTO
                                   {
                                       ASMCL_Id = m.ASMCL_Id,
                                       ASMCL_ClassName = m.ASMCL_ClassName
                                   }).ToArray();

                dto.sectionDrpDwn = (from n in _db.School_M_Section
                                     where n.MI_Id == id && n.ASMC_ActiveFlag == 1
                                     select new School_M_Section
                                     {
                                         ASMS_Id = n.ASMS_Id,
                                         ASMC_SectionName = n.ASMC_SectionName
                                     }).ToArray();

                dto.fillmonth = (from a in _db.month
                                 where (a.Is_Active == true)
                                 select new EmployeeLogReportDTO
                                 {
                                     monthid = Convert.ToInt32(a.IVRM_Month_Id),
                                     monthname = a.IVRM_Month_Name,
                                 }).Distinct().ToArray();



                dto.fillyear = (from a in _db.AcademicYear
                                where (a.MI_Id == id && a.ASMAY_ActiveFlag == 1)
                                select new HR_Master_LeaveYearDTO
                                {
                                    HRMLY_Id = Convert.ToInt32(a.ASMAY_Id),
                                    HRMLY_LeaveYear = a.ASMAY_Year
                                }
                    ).Distinct().ToArray();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
            return dto;
        }

        public BirthDayDTO getmonthreport(BirthDayDTO rpt)
        {
            try
            {
                var acd_Id = _db.AcademicYear.Where(t => t.MI_Id.Equals(rpt.MI_Id) && Convert.ToDateTime(t.ASMAY_From_Date) <= Convert.ToDateTime(System.DateTime.Today.Date) && Convert.ToDateTime(t.ASMAY_To_Date) >= Convert.ToDateTime(System.DateTime.Today.Date)).Select(d => d.ASMAY_Id).FirstOrDefault();

                var date11 = _db.AcademicYear.Where(t => t.ASMAY_Year == rpt.day && t.MI_Id == rpt.MI_Id && t.ASMAY_ActiveFlag == 1).Select(t => t.ASMAY_From_Date).FirstOrDefault();
                var date12 = _db.AcademicYear.Where(t => t.ASMAY_Year == rpt.day && t.MI_Id == rpt.MI_Id && t.ASMAY_ActiveFlag == 1).Select(t => t.ASMAY_To_Date).FirstOrDefault();

                //rpt.staffList = (from a in _db.HR_Master_Employee_DMO
                //                 from b in _db.Multiple_Email_DMO
                //                 from c in _db.Multiple_Mobile_DMO
                //                 where (a.HRME_Id == b.HRME_Id && a.HRME_Id == c.HRME_Id && (b.HRMEM_DeFaultFlag.Equals("default") || b.HRMEM_DeFaultFlag.Equals("null") || c.HRMEMNO_DeFaultFlag.Equals("default") || c.HRMEMNO_DeFaultFlag.Equals("null")) && a.MI_Id == rpt.MI_Id && a.HRME_ActiveFlag == true && a.HRME_DOJ.Value >= date11.Value && a.HRME_DOJ.Value <= date12.Value && a.HRME_DOJ.Value.Month == rpt.month)
                //                 select new BirthDayDTO
                //                 {
                //                     HRME_Id = a.HRME_Id,
                //                     employeeName = a.HRME_EmployeeFirstName + (string.IsNullOrEmpty(a.HRME_EmployeeMiddleName) ? "" : ' ' + a.HRME_EmployeeMiddleName) + (string.IsNullOrEmpty(a.HRME_EmployeeLastName) ? "" : ' ' + a.HRME_EmployeeLastName),
                //                     HRME_EmailId = b.HRMEM_EmailId == null || b.HRMEM_EmailId == "" ? "" : b.HRMEM_EmailId,
                //                     HRME_MobileNo = c.HRMEMNO_MobileNo == 0 ? 0 : c.HRMEMNO_MobileNo,
                //                     HRME_DOB = a.HRME_DOB
                //                 }
                //            ).ToArray();

                rpt.staffList = _db.HR_Master_Employee_DMO.Where(a => a.MI_Id == rpt.MI_Id && a.HRME_ActiveFlag == true && a.HRME_DOJ.Value >= date11.Value && a.HRME_DOJ.Value <= date12.Value && a.HRME_DOJ.Value.Month == rpt.month).ToArray();

                if (rpt.staffList.Length > 0)
                {
                    rpt.count = rpt.staffList.Length;
                }
                else
                {
                    rpt.count = 0;
                }

                //workingEmployee
                List<MasterEmployee> workingEmployee = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(rpt.MI_Id) && t.HRME_LeftFlag == false && Convert.ToDateTime(t.HRME_DOL).Month <= rpt.month && Convert.ToDateTime(t.HRME_DOL).Year <= Convert.ToDateTime( date11).Year && t.HRME_ActiveFlag == true).ToList();
                rpt.workingEmployee = workingEmployee.Count();

                //missingPhoto
                rpt.missingPhoto = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(rpt.MI_Id) && t.HRME_LeftFlag == false && Convert.ToDateTime(t.HRME_DOL).Month <= rpt.month && Convert.ToDateTime(t.HRME_DOL).Year <= Convert.ToDateTime( date11).Year && t.HRME_Photo == null && t.HRME_ActiveFlag == true).Count();

                //missingEmailId
                var EmplForEmail = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(rpt.MI_Id) && t.HRME_LeftFlag == false && Convert.ToDateTime(t.HRME_DOL).Month <= rpt.month && Convert.ToDateTime(t.HRME_DOL).Year <= Convert.ToDateTime( date11).Year && t.HRME_ActiveFlag == true).ToList();
                if (EmplForEmail.Count() > 0)
                {
                    var EmplForEmailCount = _HRMSContext.Emp_Email_Id.Where(t => EmplForEmail.Select(a => a.HRME_Id).Contains(t.HRME_Id)).Select(t => t.HRME_Id);
                    if (EmplForEmailCount.Count() > 0)
                    {
                        rpt.missingEmailId = EmplForEmail.Where(u => !EmplForEmailCount.Contains(u.HRME_Id)).Count();
                    }
                    else
                    {
                        rpt.missingEmailId = EmplForEmail.Count();
                    }
                }
                else
                {
                    rpt.missingEmailId = 0;
                }

                //missingContactNumber
                var EmplForContactNumber = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(rpt.MI_Id) && t.HRME_LeftFlag == false && Convert.ToDateTime(t.HRME_DOL).Month <= rpt.month && Convert.ToDateTime(t.HRME_DOL).Year <= Convert.ToDateTime( date11).Year && t.HRME_ActiveFlag == true).ToList();

                if (EmplForContactNumber.Count() > 0)
                {
                    var EmplForContactNumberCount = _HRMSContext.Emp_MobileNo.Where(t => EmplForContactNumber.Select(a => a.HRME_Id).Contains(t.HRME_Id)).Select(t => t.HRME_Id);
                    if (EmplForContactNumberCount.Count() > 0)
                    {
                        rpt.missingContactNumber = EmplForContactNumber.Where(u => !EmplForContactNumberCount.Contains(u.HRME_Id)).Count();
                    }
                    else
                    {
                        rpt.missingContactNumber = EmplForContactNumber.Count();
                    }
                }
                else
                {
                    rpt.missingContactNumber = 0;
                }

                string[] strModule = { "Late-In Alert message", "Early-Out Alert message", "Absent/Not Punched Alert message", "Late-In details", "Early-Out details" };
                rpt.smscount = (from aw in _db.IVRM_sms_sentBoxDMO
                                 where(aw.MI_Id == rpt.MI_Id && strModule.Contains(aw.Module_Name) && aw.Datetime.Date >= date11.Value.Date && aw.Datetime.Date <= date12.Value.Date && aw.Datetime.Month==rpt.month)
                                 select new BirthDayDTO
                                 {
                                     ssb = aw.IVRM_SSB_ID
                                 }).Count();

                rpt.emailcount = (from aw2 in _db.ivrm_email_sentbox
                                    where(aw2.MI_Id == rpt.MI_Id && strModule.Contains(aw2.Module_Name) && aw2.Datetime.Value.Date >= date11.Value.Date && aw2.Datetime.Value.Date <= date12.Value.Date && aw2.Datetime.Value.Month == rpt.month)
                                    select new BirthDayDTO
                                    {
                                        esb = aw2.IVRMESB_ID
                                    }).Count();

                string[] strLateInModule = { "Late-In Alert message", "Late-In details"};
                rpt.onlylateinDetailssms = (from aw in _db.IVRM_sms_sentBoxDMO
                                            where (aw.MI_Id == rpt.MI_Id && strLateInModule.Contains(aw.Module_Name) && aw.Datetime.Date >=  date11.Value.Date && aw.Datetime.Date <=  date12.Value.Date && aw.Datetime.Month == rpt.month)
                                            select new BirthDayDTO
                                            {
                                                ssb = aw.IVRM_SSB_ID
                                            }).Count();

                rpt.onlylateinDetailsEmail = (from aw2 in _db.ivrm_email_sentbox
                                            where (aw2.MI_Id == rpt.MI_Id && strLateInModule.Contains(aw2.Module_Name) && aw2.Datetime.Value.Date >=  date11.Value.Date && aw2.Datetime.Value.Date <=  date12.Value.Date && aw2.Datetime.Value.Month == rpt.month)
                                            select new BirthDayDTO
                                            {
                                                esb = aw2.IVRMESB_ID
                                            }).Count();

                string[] strEarlyOutModule = { "Early-Out Alert message", "Early-Out details" };
                rpt.onlyEarlyOutDetailssms = (from aw in _db.IVRM_sms_sentBoxDMO
                                            where (aw.MI_Id == rpt.MI_Id && strEarlyOutModule.Contains(aw.Module_Name) && aw.Datetime.Date >=  date11.Value.Date && aw.Datetime.Date <=  date12.Value.Date && aw.Datetime.Month == rpt.month)
                                            select new BirthDayDTO
                                            {
                                                ssb = aw.IVRM_SSB_ID
                                            }).Count();

                rpt.onlyEarlyOutDetailsEmail = (from aw2 in _db.ivrm_email_sentbox
                                            where (aw2.MI_Id == rpt.MI_Id && strEarlyOutModule.Contains(aw2.Module_Name) && aw2.Datetime.Value.Date >=  date11.Value.Date && aw2.Datetime.Value.Date <=  date12.Value.Date && aw2.Datetime.Value.Month == rpt.month)
                                            select new BirthDayDTO
                                            {
                                                esb = aw2.IVRMESB_ID
                                            }).Count();

                string[] strAbsentModule = { "Absent/Not Punched Alert message"};
                rpt.onlyAbsentDetailssms = (from aw in _db.IVRM_sms_sentBoxDMO
                                              where (aw.MI_Id == rpt.MI_Id && strAbsentModule.Contains(aw.Module_Name) && aw.Datetime.Date >=  date11.Value.Date && aw.Datetime.Date <=  date12.Value.Date && aw.Datetime.Month == rpt.month)
                                              select new BirthDayDTO
                                              {
                                                  ssb = aw.IVRM_SSB_ID
                                              }).Count();

                rpt.onlyAbsentDetailsEmail = (from aw2 in _db.ivrm_email_sentbox
                                              where (aw2.MI_Id == rpt.MI_Id && strAbsentModule.Contains(aw2.Module_Name) && aw2.Datetime.Value.Date >=  date11.Value.Date && aw2.Datetime.Value.Date <=  date12.Value.Date && aw2.Datetime.Value.Month == rpt.month)
                                              select new BirthDayDTO
                                              {
                                                  esb = aw2.IVRMESB_ID
                                              }).Count();
            
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return rpt;
        }

    }
}
