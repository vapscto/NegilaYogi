using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.TT;
using DataAccessMsSqlServerProvider.com.vapstech.Exam;
using PreadmissionDTOs.com.vaps.Portals.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccessMsSqlServerProvider.com.vapstech.FrontOffice;
using DataAccessMsSqlServerProvider.com.vapstech.COE;
using DataAccessMsSqlServerProvider.com.vapstech.Portals;
using DomainModel.Model.com.vapstech.MobileApp;
using PreadmissionDTOs;
using DomainModel.Model.com.vapstech.Portals.Student;
using System.Data;
using System.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Dynamic;
using System.Net.Http;
using System.Text;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using CommonLibrary;
using DomainModel.Model.com.vapstech.Portals;
using DomainModel.Model;

namespace PortalHub.com.vaps.Employee.Services
{
    public class EmployeePtalImpl : Interfaces.EmployeePtalInterface
    {
        public HRMSContext _hrms;
        public TTContext _tt;
        public ExamContext _exm;
        public FOContext _FOContext;
        public COEContext _COEContext;
        public PortalContext _PortalContext;
        public EmployeePtalImpl(HRMSContext hrms, TTContext tt, FOContext fOContext, ExamContext exm, COEContext COEContext, PortalContext portalContext)
        {
            _hrms = hrms;
            _tt = tt;
            _COEContext = COEContext;
            _FOContext = fOContext;
            _exm = exm;
            _PortalContext = portalContext;
        }

        public EmployeeDashboardDTO getdata(EmployeeDashboardDTO dto)
        {
            try
            {
                dto.yearlist = _PortalContext.AcademicYearDMO.Where(a => a.MI_Id == dto.MI_Id && a.Is_Active == true).OrderByDescending(a => a.ASMAY_Order).ToArray();

                dto.displyamessage = (from a in _PortalContext.IVRM_NoticeBoardDMO
                                      where (a.MI_Id == dto.MI_Id && a.INTB_ActiveFlag == true && a.INTB_ToStaffFlg == true && a.NTB_TTSylabusFlg == "DM")
                                      select new EmployeeDashboardDTO
                                      {
                                          INTB_Title = a.INTB_Title,
                                          INTB_Description = a.INTB_Description,
                                          NTB_TTSylabusFlg = a.NTB_TTSylabusFlg,
                                          INTB_Id = a.INTB_Id
                                      }).ToArray();


                var moId = _PortalContext.Institute.Where(t => t.MI_Id == dto.MI_Id && t.MI_ActiveFlag == 1).ToList();
                int moIDActive = 0;

                if (moId.Count > 0)
                {
                    moIDActive = _PortalContext.Organisation.Where(t => t.MO_Id == moId.FirstOrDefault().MO_Id && t.MO_ActiveFlag == 1).Count();
                }
                if (moId.Count == 0)
                {
                    dto.disabledint = true;
                    dto.blockdashboard = true;
                    dto.disableflag = "INT";
                    dto.messag = geterrormessage(dto);
                    return dto;
                }
                if (moIDActive == 0)
                {
                    dto.disabledorg = true;
                    dto.blockdashboard = true;
                    dto.disableflag = "ORG";
                    dto.messag = geterrormessage(dto);
                    return dto;
                }
                if (dto.disabledint == false && dto.disabledorg == false)
                {
                    Master_Institution_SubscriptionValidity Subscriptiondetails = _PortalContext.Master_Institution_SubscriptionValidity.Where(t => t.MI_Id.Equals(dto.MI_Id)).FirstOrDefault();

                    DateTime subscriptionenddate = Convert.ToDateTime(Subscriptiondetails.MISV_ToDate);
                    DateTime curdate = DateTime.Now;

                    var subscri = Subscriptiondetails.MISV_ActiveFlag;
                    if (subscri == false)
                    {
                        dto.disablesubscription = true;
                        dto.blockdashboard = true;
                        dto.disableflag = "SUBACTIVE";
                        dto.messag = geterrormessage(dto);
                        return dto;
                    }
                    if (subscriptionenddate < curdate)
                    {
                        dto.subscriptionover = true;
                        dto.blockdashboard = true;
                        dto.disableflag = "SUBDIS";
                        dto.messag = geterrormessage(dto);
                        return dto;
                    }

                    var emp_Id = _exm.Staff_User_Login.Where(c => c.Id == dto.UserId && c.MI_Id == dto.MI_Id).Distinct().ToList();
                    if (emp_Id.Count > 0)
                    {
                        dto.HRME_Id = emp_Id.FirstOrDefault().Emp_Code;
                    }
                    var empdetails = _exm.HR_Master_Employee_DMO.Where(e => e.MI_Id == dto.MI_Id && e.HRME_ActiveFlag == true && e.HRME_LeftFlag == false && e.HRME_Id == dto.HRME_Id).Distinct().ToList();
                    if (empdetails.Count > 0)
                    {
                        dto.HRME_TechNonTeachingFlg = empdetails.FirstOrDefault().HRME_TechNonTeachingFlg;
                        dto.HRMD_Id = emp_Id.FirstOrDefault().Emp_Code;
                    }

                    var acdyear = _PortalContext.AcademicYearDMO.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.Is_Active == true && Convert.ToDateTime(t.ASMAY_From_Date) <= Convert.ToDateTime(System.DateTime.Today.Date) && Convert.ToDateTime(t.ASMAY_To_Date) >= Convert.ToDateTime(System.DateTime.Today.Date)).Select(d => d.ASMAY_Id).FirstOrDefault();
                    if (acdyear != dto.ASMAY_Id)
                    {
                        dto.ASMAY_Id = acdyear;
                        dto.updatedyear = acdyear;
                    }
                    else
                    {
                        dto.updatedyear = 0;
                    }


                    using (var cmd = _PortalContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "Portal_DashboardAssetList";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar)
                        {
                            Value = dto.MI_Id
                        });

                        cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.VarChar)
                        {
                            Value = dto.HRME_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@Roleflg", SqlDbType.VarChar)
                        {
                            Value = "Asset"
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
                            dto.assetlist = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }

                    if (dto.assetlist.Length > 0)
                    {
                        dto.assetcount = dto.assetlist.Length;
                    }


                    else
                    {

                        dto.assetcount = 0;
                    }


                    using (var cmd = _PortalContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "Portal_DashboardAssetList";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar)
                        {
                            Value = dto.MI_Id
                        });

                        cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.VarChar)
                        {
                            Value = dto.HRME_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@Roleflg", SqlDbType.VarChar)
                        {
                            Value = "Purchase"
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
                            dto.purchaselist = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }

                    if (dto.purchaselist.Length > 0)
                    {
                        dto.purchasecount = dto.purchaselist.Length;
                    }
                    else
                    {

                        dto.purchasecount = 0;
                    }

                    using (var cmd = _PortalContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "Portal_DashboardAssetList";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar)
                        {
                            Value = dto.MI_Id
                        });

                        cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.VarChar)
                        {
                            Value = dto.HRME_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@Roleflg", SqlDbType.VarChar)
                        {
                            Value = "BOOK"
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
                            dto.BookList = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }


                    if (dto.BookList.Length > 0)
                    {
                        dto.bookcount = dto.BookList.Length;
                    }
                    else
                    {

                        dto.bookcount = 0;
                    }

                    var currentyear = DateTime.Now.Year;
                    //currentyear = 2017;
                    //dto.salarylist = (from m in _hrms.HR_Employee_Salary
                    //                  from n in _hrms.HR_Employee_Salary_Details
                    //                  from o in _hrms.HR_Master_EarningsDeductions
                    //                  where m.HRES_Id == n.HRES_Id && n.HRMED_Id == o.HRMED_Id && m.MI_Id == dto.MI_Id && m.HRME_Id == dto.HRME_Id && m.HRES_Year == Convert.ToString(currentyear) && o.HRMED_EarnDedFlag.Equals("Earning")
                    //                  group new { m, n, o }
                    //                    by new { m.HRES_Month } into g
                    //                  select new EmployeeDashboardDTO
                    //                  {
                    //                      salary = g.Sum(d => d.n.HRESD_Amount),
                    //                      monthName = g.FirstOrDefault().m.HRES_Month,

                    //                  }
                    //               ).ToArray();


                    #region Employee Details
                    //using (var cmd = _hrms.Database.GetDbConnection().CreateCommand())
                    //{
                    //    cmd.CommandText = "PORTAL_Employee_Dashboard";
                    //    cmd.CommandType = CommandType.StoredProcedure;

                    //    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    //    {
                    //        Value = dto.MI_Id
                    //    });

                    //    cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.BigInt)
                    //    {
                    //        Value = dto.HRME_Id
                    //    });


                    //    if (cmd.Connection.State != ConnectionState.Open)
                    //        cmd.Connection.Open();

                    //    var retObject = new List<dynamic>();
                    //    try
                    //    {
                    //        using (var dataReader = cmd.ExecuteReader())
                    //        {
                    //            while (dataReader.Read())
                    //            {
                    //                var dataRow = new ExpandoObject() as IDictionary<string, object>;
                    //                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                    //                {
                    //                    dataRow.Add(
                    //                        dataReader.GetName(iFiled),
                    //                        dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                    //                    );
                    //                }

                    //                retObject.Add((ExpandoObject)dataRow);
                    //            }
                    //        }
                    //        dto.filldepartment = retObject.ToArray();
                    //    }
                    //    catch (Exception ex)
                    //    {
                    //        Console.WriteLine(ex.Message);
                    //    }
                    //}
                    #endregion


                    dto.filldepartment = (from a in _FOContext.HR_Master_Employee_DMO
                                          from b in _FOContext.HR_Master_Department_DMO
                                          from c in _FOContext.HR_Master_Designation_DMO

                                          where (a.HRMD_Id == b.HRMD_Id && a.HRMDES_Id == c.HRMDES_Id && a.HRME_ActiveFlag == true && b.HRMD_ActiveFlag == true && c.HRMDES_ActiveFlag == true && a.MI_Id == dto.MI_Id && a.HRME_Id == dto.HRME_Id)
                                          select new EmployeeDashboardDTO
                                          {

                                              HRME_Id = a.HRME_Id,
                                              HRME_EmployeeFirstName = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + " " + (a.HRME_EmployeeMiddleName == null ? " " : a.HRME_EmployeeMiddleName) + " " + (a.HRME_EmployeeLastName == null ? " " : a.HRME_EmployeeLastName)).Trim(),

                                              HRME_EmployeeMiddleName = a.HRME_EmployeeMiddleName,
                                              HRME_EmployeeLastName = a.HRME_EmployeeLastName,
                                              HRME_DOJ = a.HRME_DOJ,
                                              HRMD_DepartmentName = b.HRMD_DepartmentName,
                                              HRMDES_DesignationName = c.HRMDES_DesignationName,
                                              HRME_EmployeeCode = a.HRME_EmployeeCode,
                                              HRME_DOB = a.HRME_DOB,
                                              HRME_PhotoNo = a.HRME_Photo,
                                              DeviceID = (a.HRME_AppDownloadedDeviceId == null ? " " : a.HRME_AppDownloadedDeviceId)
                                          }).Distinct().ToArray();


                    dto.classsection = (from a in _FOContext.HR_Master_Employee_DMO
                                        from d in _FOContext.ClassTeacherMappingDMO
                                        from e in _FOContext.Adm_School_M_ClassDMO
                                        from f in _FOContext.school_M_Section
                                        where (a.HRME_ActiveFlag == true && a.MI_Id == dto.MI_Id && a.HRME_Id == dto.HRME_Id && d.MI_Id == dto.MI_Id && d.HRME_Id == dto.HRME_Id && d.ASMAY_Id == dto.ASMAY_Id && d.ASMCL_Id == e.ASMCL_Id && d.ASMS_Id == f.ASMS_Id)
                                        select new EmployeeDashboardDTO
                                        {
                                            ASMCL_ClassName = e.ASMCL_ClassName,
                                            ASMC_SectionName = f.ASMC_SectionName
                                        }).Distinct().ToArray();

                    dto.classhandling = (from b in _FOContext.Exm_Login_Privilege_SubjectsDMO
                                         from d in _FOContext.Exm_Login_PrivilegeDMO
                                         from e in _FOContext.Adm_School_M_ClassDMO
                                         from f in _FOContext.school_M_Section
                                         from a in _FOContext.Staff_User_Login
                                         where (d.MI_Id == dto.MI_Id && b.ELP_Id == d.ELP_Id && d.ASMAY_Id == dto.ASMAY_Id &&
                                         b.ASMS_Id == f.ASMS_Id && b.ASMCL_Id == e.ASMCL_Id && d.ELP_ActiveFlg == true && b.ELPs_ActiveFlg == true && d.Login_Id == a.IVRMSTAUL_Id && a.Id == dto.UserId)
                                         select new EmployeeDashboardDTO
                                         {
                                             ASMCL_ClassName = e.ASMCL_ClassName,
                                             ASMC_SectionName = f.ASMC_SectionName
                                         }).Distinct().ToArray();


                    dto.mobile = (from a in _hrms.Emp_MobileNo
                                  where (a.HRME_Id == dto.HRME_Id && a.HRMEMNO_DeFaultFlag == "default")
                                  select new EmployeeDashboardDTO
                                  {
                                      HRME_MobileNo = a.HRMEMNO_MobileNo,
                                  }).Distinct().ToArray();


                    dto.email = (from a in _hrms.Emp_Email_Id

                                 where (a.HRME_Id == dto.HRME_Id && a.HRMEM_DeFaultFlag == "default")
                                 select new EmployeeDashboardDTO
                                 {
                                     HRME_EmailId = a.HRMEM_EmailId,
                                 }).Distinct().ToArray();

                    dto.TT_final_generation = (from a in _tt.TT_Final_GenerationDMO
                                               from b in _tt.TT_Final_Generation_DetailedDMO
                                               from c in _tt.School_M_Class
                                               from d in _tt.School_M_Section
                                               from e in _tt.TT_Master_PeriodDMO
                                               from f in _tt.TT_Master_DayDMO

                                               where (a.ASMAY_Id == dto.ASMAY_Id && a.TTFG_Id == b.TTFG_Id && a.MI_Id == dto.MI_Id && b.TTMP_Id == e.TTMP_Id && b.ASMCL_Id == c.ASMCL_Id && c.MI_Id == dto.MI_Id && d.ASMC_ActiveFlag == 1 && d.ASMS_Id == b.ASMS_Id && b.TTMD_Id == f.TTMD_Id && b.HRME_Id == dto.HRME_Id && a.ASMAY_Id == dto.ASMAY_Id)
                                               select new
                                               {
                                                   DayName = f.TTMD_DayName,
                                                   PeriodCount = e.TTMP_PeriodName.Count()


                                               }).Distinct().GroupBy(f => f.DayName).Select(g => new EmployeeDashboardDTO { DayName = g.Key, PeriodCount = g.Count() }).ToArray();

                    dto.allperiods = _tt.TT_Master_DayDMO.Where(c => c.TTMD_ActiveFlag == true && c.MI_Id == dto.MI_Id).ToArray();




                    var clstchname = (from a in _PortalContext.ClassTeacherMappingDMO
                                      from e in _PortalContext.HR_Master_Employee_DMO
                                      where (a.HRME_Id == e.HRME_Id && a.MI_Id == dto.MI_Id && a.ASMAY_Id == dto.ASMAY_Id && a.IMCT_ActiveFlag == true && a.HRME_Id == dto.HRME_Id)
                                      select new EmployeeDashboardDTO
                                      {
                                          HRME_Id = a.HRME_Id,
                                          ASMCL_Id = a.ASMCL_Id,
                                          ASMS_Id = a.ASMS_Id,

                                          HRME_EmployeeFirstName = ((e.HRME_EmployeeFirstName == null ? " " : e.HRME_EmployeeFirstName) + " " + (e.HRME_EmployeeMiddleName == null ? " " : e.HRME_EmployeeMiddleName) + " " + (e.HRME_EmployeeLastName == null ? " " : e.HRME_EmployeeLastName)).Trim(),
                                      }).Distinct().ToList();


                    if (clstchname != null && clstchname.Count() > 0)
                    {


                        dto.fillstudent = (from a in _PortalContext.School_Adm_Y_StudentDMO
                                           from b in _PortalContext.ClassTeacherMappingDMO
                                           from c in _PortalContext.AdmissionStudentDMO
                                           where (a.ASMCL_Id == b.ASMCL_Id && a.ASMS_Id == b.ASMS_Id && b.HRME_Id == dto.HRME_Id && c.AMST_Id == a.AMST_Id && c.AMST_SOL == "S" && a.AMAY_ActiveFlag == 1 && a.ASMAY_Id == dto.ASMAY_Id && b.MI_Id == dto.MI_Id)
                                           select new EmployeeDashboardDTO
                                           {
                                               Amst_Id = a.AMST_Id,
                                               AMST_FirstName = c.AMST_FirstName + ' ' + c.AMST_MiddleName + ' ' + c.AMST_LastName,
                                               amst_FirstName = ((c.AMST_FirstName == null ? " " : c.AMST_FirstName) + " " + (c.AMST_MiddleName == null ? " " : c.AMST_MiddleName) + " " + (c.AMST_LastName == null ? " " : c.AMST_LastName)).Trim(),
                                               AMST_MiddleName = c.AMST_MiddleName,
                                               AMST_LastName = c.AMST_LastName,
                                               studentnameorder = ((c.AMST_FirstName == null || c.AMST_FirstName == "" ? "" : " " + c.AMST_FirstName) +
                                              (c.AMST_MiddleName == null || c.AMST_MiddleName == "" || c.AMST_MiddleName == "0" ? "" : " " + c.AMST_MiddleName) +
                                              (c.AMST_LastName == null || c.AMST_LastName == "" || c.AMST_LastName == "0" ? "" : " " + c.AMST_LastName)).Trim(),
                                           }).Distinct().OrderBy(t => t.studentnameorder).ToArray();



                        using (var cmd = _PortalContext.Database.GetDbConnection().CreateCommand())
                        {
                            cmd.CommandText = "Portal_StudentCount";
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                            {
                                Value = dto.MI_Id
                            });
                            cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.BigInt)
                            {
                                Value = dto.HRME_Id
                            });
                            cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.BigInt)
                            {
                                Value = dto.ASMAY_Id
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
                                               dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
                                            );
                                        }
                                        retObject.Add((ExpandoObject)dataRow);
                                    }
                                }
                                dto.fillstudent = retObject.ToArray();
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                        }


                        if (dto.fillstudent.Length > 0)
                        {
                            dto.studentcount = dto.fillstudent.Length;
                        }
                    }
                    else
                    {
                        var fillclass = (from a in _PortalContext.Exm_Login_Privilege_SubjectsDMO
                                         from b in _PortalContext.Exm_Login_PrivilegeDMO
                                         from c in _PortalContext.IVRM_Staff_User_Login
                                         from d in _PortalContext.HR_Master_Employee_DMO
                                         where (b.ELP_Id == a.ELP_Id && c.IVRMSTAUL_Id == b.Login_Id && d.HRME_Id == c.Emp_Code && d.HRME_Id == dto.HRME_Id && b.ASMAY_Id == dto.ASMAY_Id && b.MI_Id == dto.MI_Id && b.ELP_Flg == "st")
                                         select new EmployeeDashboardDTO
                                         {
                                             ASMCL_Id = a.ASMCL_Id,
                                             //ASMS_Id=a.ASMS_Id,
                                             //ISMS_Id=a.ISMS_Id

                                         }).Distinct().ToArray();

                        var fillsection = (from a in _PortalContext.Exm_Login_Privilege_SubjectsDMO
                                           from b in _PortalContext.Exm_Login_PrivilegeDMO
                                           from c in _PortalContext.IVRM_Staff_User_Login
                                           from d in _PortalContext.HR_Master_Employee_DMO
                                           where (b.ELP_Id == a.ELP_Id && c.IVRMSTAUL_Id == b.Login_Id && d.HRME_Id == c.Emp_Code && d.HRME_Id == dto.HRME_Id && b.ASMAY_Id == dto.ASMAY_Id && b.MI_Id == dto.MI_Id && b.ELP_Flg == "st")
                                           select new EmployeeDashboardDTO
                                           {

                                               ASMS_Id = a.ASMS_Id


                                           }).Distinct().ToArray();

                        var fillsubid = (from a in _PortalContext.Exm_Login_Privilege_SubjectsDMO
                                         from b in _PortalContext.Exm_Login_PrivilegeDMO
                                         from c in _PortalContext.IVRM_Staff_User_Login
                                         from d in _PortalContext.HR_Master_Employee_DMO
                                         where (b.ELP_Id == a.ELP_Id && c.IVRMSTAUL_Id == b.Login_Id && d.HRME_Id == c.Emp_Code && d.HRME_Id == dto.HRME_Id && b.ASMAY_Id == dto.ASMAY_Id && b.MI_Id == dto.MI_Id && b.ELP_Flg == "st")
                                         select new EmployeeDashboardDTO
                                         {

                                             ISMS_Id = a.ISMS_Id

                                         }).Distinct().ToArray();
                        string asmcl_id = "0";
                        foreach (var ue in fillclass)
                        {
                            asmcl_id = asmcl_id + "," + ue.ASMCL_Id;
                        }
                        string asms_id = "0";
                        foreach (var ue in fillsection)
                        {
                            asms_id = asms_id + "," + ue.ASMS_Id;
                        }
                        string isms_id = "0";
                        foreach (var ue in fillsubid)
                        {
                            isms_id = isms_id + "," + ue.ISMS_Id;
                        }



                        using (var cmd = _PortalContext.Database.GetDbConnection().CreateCommand())
                        {
                            cmd.CommandText = "Portal_StudentCount_test";
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar)
                            {
                                Value = dto.MI_Id
                            });
                            cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.VarChar)
                            {
                                Value = dto.HRME_Id
                            });
                            cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar)
                            {
                                Value = dto.ASMAY_Id
                            });
                            cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar)
                            {
                                Value = asmcl_id
                            });
                            cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar)
                            {
                                Value = asms_id
                            });
                            cmd.Parameters.Add(new SqlParameter("@ISMS_Id", SqlDbType.VarChar)
                            {
                                Value = isms_id
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
                                               dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
                                            );
                                        }
                                        retObject.Add((ExpandoObject)dataRow);
                                    }
                                }
                                dto.fillstudent = retObject.ToArray();

                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }

                        }

                        if (dto.fillstudent.Length > 0)
                        {
                            dto.studentcount = dto.fillstudent.Length;
                        }



                    }
                    #region events

                    DateTime now = DateTime.Now;
                    int A = now.Month;

                    //dto.coedata = (from m in _COEContext.COE_Master_EventsDMO
                    //               from n in _COEContext.COE_EventsDMO
                    //               where m.COEME_Id == n.COEME_Id && n.MI_Id == dto.MI_Id && n.ASMAY_Id == dto.ASMAY_Id && n.COEE_EStartDate.Value.Month == A
                    //               select new EmployeeDashboardDTO
                    //               {
                    //                   eventName = m.COEME_EventName,
                    //                   eventDesc = m.COEME_EventDesc,
                    //                   COEE_EStartDate = n.COEE_EStartDate,
                    //                   COEE_EEndDate = n.COEE_EEndDate,
                    //               }).ToArray();
                    #endregion

                    #region  CALENDER 
                    //try
                    //{
                    //    dto.calenderlist = (from m in _PortalContext.COE_Master_EventsDMO
                    //                        from n in _PortalContext.COE_EventsDMO
                    //                        from o in _PortalContext.School_Adm_Y_StudentDMO
                    //                        where (m.COEME_Id == n.COEME_Id && n.MI_Id == dto.MI_Id && o.ASMAY_Id == dto.ASMAY_Id && n.COEE_EStartDate != null)
                    //                        select new EmployeeDashboardDTO
                    //                        {
                    //                            COEME_EventName = m.COEME_EventName,
                    //                            COEME_EventDesc = m.COEME_EventDesc,
                    //                            COEE_EStartDate = n.COEE_EStartDate,
                    //                            COEE_EEndDate = n.COEE_EEndDate,
                    //                            COEE_ReminderDate = n.COEE_ReminderDate
                    //                        }).OrderByDescending(c => c.COEE_EStartDate).Distinct().ToArray();

                    //}
                    //catch (Exception ex)
                    //{
                    //    Console.WriteLine(ex.Message);
                    //}


                    dto.calenderlist = (from m in _COEContext.COE_Master_EventsDMO
                                        from n in _COEContext.COE_EventsDMO
                                        where m.COEME_Id == n.COEME_Id && n.MI_Id == dto.MI_Id && n.ASMAY_Id == dto.ASMAY_Id && n.COEE_EStartDate.Value.Month == A && m.COEME_ActiveFlag==true 
                                        select new EmployeeDashboardDTO
                                        {
                                            COEME_EventName = m.COEME_EventName,
                                            COEME_EventDesc = m.COEME_EventDesc,
                                            COEE_EStartDate = n.COEE_EStartDate,
                                            COEE_EEndDate = n.COEE_EEndDate,
                                            COEE_ReminderDate = n.COEE_ReminderDate
                                        }).ToArray();

                    #endregion

                    #region MobileapppagePrivileges

                    List<DataAccessMsSqlServerProvider.ApplicationUserRole> user = new List<DataAccessMsSqlServerProvider.ApplicationUserRole>();

                    user = _PortalContext.ApplicationUserRole.Where(g => g.UserId == dto.UserId).ToList();

                    if (user.Count() > 0)
                    {

                        List<IVRM_Role_MobileApp_Privileges> Staffmobileappprivileges = new List<IVRM_Role_MobileApp_Privileges>();
                        Staffmobileappprivileges = _PortalContext.IVRM_Role_MobileApp_Privileges.Where(f => f.IVRMRT_Id == dto.roleid && f.MI_ID == dto.MI_Id).ToList();

                        if (Staffmobileappprivileges.Count() > 0)
                        {

                            dto.Staffmobileappprivileges = (from Mobilepage in _PortalContext.IVRM_MobileApp_Page
                                                            from MobileRolePrivileges in _PortalContext.IVRM_Role_MobileApp_Privileges
                                                            from UserRolePrivileges in _PortalContext.IVRM_User_MobileApp_Login_Privileges
                                                            where (MobileRolePrivileges.MI_ID == UserRolePrivileges.MI_Id && Mobilepage.IVRMMAP_Id == MobileRolePrivileges.IVRMMAP_Id && Mobilepage.IVRMMAP_Id == UserRolePrivileges.IVRMMAP_Id && MobileRolePrivileges.IVRMRT_Id == dto.roleid && MobileRolePrivileges.MI_ID == dto.MI_Id && UserRolePrivileges.IVRMUL_Id == dto.UserId && Mobilepage.IVRMMAP_ActiveFlg == true && MobileRolePrivileges.IVRMRMAP_ActiveFlg == true && UserRolePrivileges.IVRMUMALP_ActiveFlg == true)
                                                            select new LoginDTO
                                                            {
                                                                Pagename = Mobilepage.IVRMMAP_AppPageName,
                                                                Pageicon = Mobilepage.IVRMMAP_AppPageDesc,
                                                                Pageurl = Mobilepage.IVRMMAP_AppPageURL,
                                                                IVRMRMAP_Id = MobileRolePrivileges.IVRMRMAP_Id,
                                                                IVRMMAP_AddFlg = UserRolePrivileges.IVRMUMALP_AddFlg,
                                                                IVRMMAP_UpdateFlg = UserRolePrivileges.IVRMUMALP_UpdateFlg,
                                                                IVRMMAP_DeleteFlg = UserRolePrivileges.IVRMUMALP_DeleteFlg
                                                            }).ToArray();

                            dto.mobileprivileges = "true";
                        }
                        else
                        {
                            dto.mobileprivileges = "false";
                        }
                    }

                    #endregion

                    #region Mobileversion
                    using (var cmdv = _PortalContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmdv.CommandText = "Versioncheck";
                        cmdv.CommandType = CommandType.StoredProcedure;

                        cmdv.Parameters.Add(new SqlParameter("@MI_Id",
                    SqlDbType.BigInt)
                        {
                            Value = dto.MI_Id
                        });
                        cmdv.Parameters.Add(new SqlParameter("@Type",
                         SqlDbType.VarChar, 50)
                        {
                            Value = "Staff"
                        });
                        cmdv.Parameters.Add(new SqlParameter("@version",
                   SqlDbType.VarChar, 50)
                        {
                            Direction = ParameterDirection.Output
                        });


                        if (cmdv.Connection.State != ConnectionState.Open)
                            cmdv.Connection.Open();

                        var data1 = cmdv.ExecuteNonQuery();
                        // x.FSS_FineAmount += Convert.ToDecimal(cmd.Parameters["@amt"].Value);
                        dto.version = cmdv.Parameters["@version"].Value.ToString();
                    }

                    #endregion

                    #region  Image/Video Gallery

                    //   using (var cmd = _PortalContext.Database.GetDbConnection().CreateCommand())
                    //   {
                    //       cmd.CommandText = "Portal_DashboardImageGallery";
                    //       cmd.CommandType = CommandType.StoredProcedure;

                    //       cmd.Parameters.Add(new SqlParameter("@MI_Id",
                    //   SqlDbType.BigInt)
                    //       {
                    //           Value = dto.MI_Id
                    //       });
                    //       cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                    //        SqlDbType.VarChar)
                    //       {
                    //           Value = 0
                    //       });
                    //       cmd.Parameters.Add(new SqlParameter("@IGA_CommonGalleryFlg",
                    // SqlDbType.BigInt)
                    //       {
                    //           Value = dto.IGA_CommonGalleryFlg
                    //       });
                    //       cmd.Parameters.Add(new SqlParameter("@Roleflg",
                    //SqlDbType.BigInt)
                    //       {
                    //           Value = "Staff"
                    //       });
                    //       if (cmd.Connection.State != ConnectionState.Open)
                    //           cmd.Connection.Open();
                    //       var retObject = new List<dynamic>();
                    //       try
                    //       {
                    //           using (var dataReader = cmd.ExecuteReader())
                    //           {
                    //               while (dataReader.Read())
                    //               {
                    //                   var dataRow = new ExpandoObject() as IDictionary<string, object>;
                    //                   for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                    //                   {
                    //                       dataRow.Add(
                    //                           dataReader.GetName(iFiled),
                    //                           dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                    //                       );
                    //                   }
                    //                   retObject.Add((ExpandoObject)dataRow);
                    //               }
                    //           }
                    //           dto.imagegallery = retObject.ToArray();
                    //       }
                    //       catch (Exception ex)
                    //       {
                    //           Console.WriteLine(ex.Message);
                    //       }
                    //   }



                    //   using (var cmd = _PortalContext.Database.GetDbConnection().CreateCommand())
                    //   {
                    //       cmd.CommandText = "Portal_DashboardVideosGallery";
                    //       cmd.CommandType = CommandType.StoredProcedure;

                    //       cmd.Parameters.Add(new SqlParameter("@MI_Id",
                    //   SqlDbType.BigInt)
                    //       {
                    //           Value = dto.MI_Id
                    //       });
                    //       cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                    //        SqlDbType.VarChar)
                    //       {
                    //           Value = 0
                    //       });
                    //       cmd.Parameters.Add(new SqlParameter("@IGA_CommonGalleryFlg",
                    // SqlDbType.BigInt)
                    //       {
                    //           Value = dto.IGA_CommonGalleryFlg
                    //       });
                    //       cmd.Parameters.Add(new SqlParameter("@Roleflg",
                    //SqlDbType.BigInt)
                    //       {
                    //           Value = "Staff"
                    //       });
                    //       if (cmd.Connection.State != ConnectionState.Open)
                    //           cmd.Connection.Open();
                    //       var retObject = new List<dynamic>();
                    //       try
                    //       {
                    //           using (var dataReader = cmd.ExecuteReader())
                    //           {
                    //               while (dataReader.Read())
                    //               {
                    //                   var dataRow = new ExpandoObject() as IDictionary<string, object>;
                    //                   for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                    //                   {
                    //                       dataRow.Add(
                    //                           dataReader.GetName(iFiled),
                    //                           dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                    //                       );
                    //                   }
                    //                   retObject.Add((ExpandoObject)dataRow);
                    //               }
                    //           }
                    //           dto.videogallery = retObject.ToArray();
                    //       }
                    //       catch (Exception ex)
                    //       {
                    //           Console.WriteLine(ex.Message);
                    //       }
                    //   }
                    #endregion

                    #region Payment Notfication
                    //if (dto.PaymentNootificationStaff == 0)
                    //{
                    //    TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                    //    DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

                    //    var checkdata = _PortalContext.IVRM_Payment_Subscription_RemarksDetilsDMO.Where(a => a.UserId == dto.UserId
                    //    && a.IVRMPSLR_LoginDatetime.Value.Date == indiantime0.Date && a.MI_Id == dto.MI_Id).ToList();

                    //    if (checkdata.Count == 0)
                    //    {
                    //        SubscriptionPaymentNotification _sub = new SubscriptionPaymentNotification(_PortalContext);
                    //        dto.getpaymentnotificationdetails = _sub.getnotificationdetails(dto.MI_Id, dto.UserId);
                    //    }
                    //}
                    #endregion

                    //#region MObiledeviceid
                    //var stureg = _PortalContext.HR_Master_Employee_DMO.Single(s => s.HRME_Id == dto.HRME_Id);
                    //stureg.HRME_AppDownloadedDeviceId = dto.mobiledeviceid;
                    //_PortalContext.Update(stureg);
                    //_PortalContext.SaveChanges();
                    //#endregion
                }
                //adeded By Praveen Gowda
                using (var cmd = _PortalContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "HR_Employee_Medical_Report";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = dto.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@flag", SqlDbType.VarChar) { Value = 3});
                    cmd.Parameters.Add(new SqlParameter("@HREMR_Id", SqlDbType.VarChar) { Value = dto.HRME_Id });


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
                        dto.MedicalCount = retObject.ToArray();
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
            return dto;
        }
        public EmployeeDashboardDTO saveakpkfile(EmployeeDashboardDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);

                IVRM_MobileApp_Download_DMO objdata = new IVRM_MobileApp_Download_DMO();


                objdata.MI_Id = data.MI_Id;
                objdata.IVRMUL_Id = data.IVRMUL_Id;
                objdata.IVRMMAD_MobileModel = "Staff";
                objdata.IVRMMAD_DownlaodDateTime = indianTime;
                objdata.CreatedDate = indianTime;
                objdata.UpdatedDate = indianTime;

                _PortalContext.Add(objdata);
                int rowAffected = _PortalContext.SaveChanges();
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
        public EmployeeDashboardDTO viewnotice(EmployeeDashboardDTO dto)
        {
            try
            {
                if (dto.INTB_Id > 0)
                {
                    dto.attachementlist = (from a in _PortalContext.IVRM_NoticeBoardDMO
                                           from b in _PortalContext.IVRM_NoticeBoard_FilesDMO_con
                                           where a.INTB_Id == dto.INTB_Id && a.INTB_Id == b.INTB_Id
                                           select new IVRM_NoticeBoardDTO
                                           {
                                               INTBFL_FileName = b.INTBFL_FileName,
                                               INTBFL_FilePath = b.INTBFL_FilePath,
                                               INTB_Attachment = a.INTB_Attachment,
                                               INTB_Id = a.INTB_Id
                                           }).ToArray();
                }
                else
                {
                    //a.MI_Id == dto.MI_Id


                    dto.displyamessage = (from a in _PortalContext.IVRM_NoticeBoardDMO
                                          where (a.MI_Id == dto.MI_Id && a.INTB_ActiveFlag == true && a.INTB_ToStaffFlg == true && a.NTB_TTSylabusFlg == "DM")
                                          select new EmployeeDashboardDTO
                                          {
                                              INTB_Title = a.INTB_Title,
                                              INTB_Description = a.INTB_Description,
                                              NTB_TTSylabusFlg = a.NTB_TTSylabusFlg,
                                              INTB_Id = a.INTB_Id
                                          }).ToArray();
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return dto;
        }
        public EmployeeDashboardDTO onclick_notice(EmployeeDashboardDTO dto)
        {
            try
            {

                if(dto.message ==null || dto.message=="")
                {
                    dto.message = "";
                }


                var emp_Id = _exm.Staff_User_Login.Where(c => c.Id == dto.UserId && c.MI_Id == dto.MI_Id).Distinct().ToList();
                if (emp_Id.Count > 0)
                {
                    dto.HRME_Id = emp_Id.FirstOrDefault().Emp_Code;
                }
                var empdetails = _exm.HR_Master_Employee_DMO.Where(e => e.MI_Id == dto.MI_Id && e.HRME_ActiveFlag == true && e.HRME_LeftFlag == false && e.HRME_Id == dto.HRME_Id).Distinct().ToList();
                if (empdetails.Count > 0)
                {
                    dto.HRME_TechNonTeachingFlg = empdetails.FirstOrDefault().HRME_TechNonTeachingFlg;
                    dto.HRMD_Id = emp_Id.FirstOrDefault().Emp_Code;
                }

                using (var cmd = _PortalContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Portal_DashboardNoticeBoard";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar)
                    {
                        Value = dto.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.VarChar)
                    {
                        Value = dto.HRME_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@Roleflg", SqlDbType.VarChar)
                    {
                        Value = "Staff"
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar)
                    {
                        Value = dto.ASMAY_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@type", SqlDbType.VarChar)
                    {
                        Value = dto.message
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
                        dto.noticeboard = retObject.ToArray();
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
            return dto;
        }
        public EmployeeDashboardDTO onclick_events(EmployeeDashboardDTO dto)
        {
            try
            {
                DateTime now = DateTime.Now;
                int A = now.Month;

                dto.coedata = (from m in _COEContext.COE_Master_EventsDMO
                               from n in _COEContext.COE_EventsDMO
                               where m.COEME_Id == n.COEME_Id && n.MI_Id == dto.MI_Id && n.ASMAY_Id == dto.ASMAY_Id && n.COEE_EStartDate.Value.Month == A && m.COEME_ActiveFlag==true && n.COEE_ActiveFlag==true
                               select new EmployeeDashboardDTO
                               {
                                   eventName = m.COEME_EventName,
                                   eventDesc = m.COEME_EventDesc,
                                   COEE_EStartDate = n.COEE_EStartDate,
                                   COEE_EEndDate = n.COEE_EEndDate,
                               }).ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }
    
        // 04-02-2023 
        public EmployeeDashboardDTO onclick_asset(EmployeeDashboardDTO dto)
        {
            try
            {

                if (dto.message == null || dto.message == "")
                {
                    dto.message = "";
                }


                var emp_Id = _exm.Staff_User_Login.Where(c => c.Id == dto.UserId && c.MI_Id == dto.MI_Id).Distinct().ToList();
                if (emp_Id.Count > 0)
                {
                    dto.HRME_Id = emp_Id.FirstOrDefault().Emp_Code;
                }
                var empdetails = _exm.HR_Master_Employee_DMO.Where(e => e.MI_Id == dto.MI_Id && e.HRME_ActiveFlag == true && e.HRME_LeftFlag == false && e.HRME_Id == dto.HRME_Id).Distinct().ToList();
                if (empdetails.Count > 0)
                {
                    dto.HRME_TechNonTeachingFlg = empdetails.FirstOrDefault().HRME_TechNonTeachingFlg;
                    dto.HRMD_Id = emp_Id.FirstOrDefault().Emp_Code;
                }
                if(dto.flag== "BOOKLIST")
                {
                    dto.HRME_Id = dto.INTB_Id;
                }
                using (var cmd = _PortalContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Portal_DashboardAssetList";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar)
                    {
                        Value = dto.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.VarChar)
                    {
                        Value = dto.HRME_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@Roleflg", SqlDbType.VarChar)
                    {
                        Value = dto.flag
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
                        dto.assetlist = retObject.ToArray();
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
            return dto;
        }


        public string geterrormessage(EmployeeDashboardDTO dto)
        {
            List<EmployeeDashboardDTO> errormessage = new List<EmployeeDashboardDTO>();
            try
            {
                using (var cmd = _PortalContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Dashboard_Mobile_Disable_Alertmessage";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@mi_id",
                  SqlDbType.BigInt)
                    {
                        Value = dto.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@flag",
                 SqlDbType.VarChar)
                    {
                        Value = dto.disableflag
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
                                errormessage.Add(new EmployeeDashboardDTO
                                {
                                    messag = Convert.ToString(dataReader["messag"])
                                });
                            }
                        }
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

            if (errormessage.Count > 0)
            {
                dto.messag = errormessage.FirstOrDefault().messag;
            }

            return dto.messag;
        }

        public EmployeeDashboardDTO onclick_Homework_datewise(EmployeeDashboardDTO dto)
        {
            try
            {
                string Class_Id = "0";
                string Section_Id = "0";

                string startdate = "";
                string enddate = "";
                var fillclass = (from a in _PortalContext.Exm_Login_Privilege_SubjectsDMO
                                 from b in _PortalContext.Exm_Login_PrivilegeDMO
                                 from c in _PortalContext.IVRM_Staff_User_Login
                                 from d in _PortalContext.HR_Master_Employee_DMO
                                 where (b.ELP_Id == a.ELP_Id && c.IVRMSTAUL_Id == b.Login_Id && d.HRME_Id == c.Emp_Code && d.HRME_Id == dto.HRME_Id && b.ASMAY_Id == dto.ASMAY_Id && b.MI_Id == dto.MI_Id && b.ELP_Flg == "st")
                                 select new EmployeeDashboardDTO
                                 {
                                     ASMCL_Id = a.ASMCL_Id,
                                     //ASMS_Id=a.ASMS_Id,
                                     //ISMS_Id=a.ISMS_Id

                                 }).Distinct().ToArray();

                var fillsection = (from a in _PortalContext.Exm_Login_Privilege_SubjectsDMO
                                   from b in _PortalContext.Exm_Login_PrivilegeDMO
                                   from c in _PortalContext.IVRM_Staff_User_Login
                                   from d in _PortalContext.HR_Master_Employee_DMO
                                   where (b.ELP_Id == a.ELP_Id && c.IVRMSTAUL_Id == b.Login_Id && d.HRME_Id == c.Emp_Code && d.HRME_Id == dto.HRME_Id && b.ASMAY_Id == dto.ASMAY_Id && b.MI_Id == dto.MI_Id && b.ELP_Flg == "st")
                                   select new EmployeeDashboardDTO
                                   {

                                       ASMS_Id = a.ASMS_Id


                                   }).Distinct().ToArray();

                startdate = dto.INTB_StartDate.ToString("yyyy-MM-dd");
                enddate = dto.INTB_EndDate.ToString("yyyy-MM-dd");

                if (fillclass.Length > 0)
                {
                    for (var i = 0; i < fillclass.Length; i++)
                    {
                        Class_Id += "," + fillclass[i].ASMCL_Id;
                    }

                }
                if (fillsection.Length > 0)
                {
                    for (var i = 0; i < fillsection.Length; i++)
                    {
                        Section_Id += "," + fillsection[i].ASMS_Id;
                    }


                }


                using (var cmd = _PortalContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Portal_HomeWorkClasswork_datewise_Staff";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = dto.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.BigInt) { Value = dto.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = Class_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = Section_Id });
                    cmd.Parameters.Add(new SqlParameter("@type", SqlDbType.VarChar) { Value = "Homework" });
                    cmd.Parameters.Add(new SqlParameter("@startdate", SqlDbType.VarChar) { Value = startdate });
                    cmd.Parameters.Add(new SqlParameter("@enddate", SqlDbType.VarChar) { Value = enddate });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.BigInt) { Value = dto.HRME_Id });


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
                        dto.homeworklist = retObject.ToArray();
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
            return dto;
        }
        public EmployeeDashboardDTO onclick_classwork_datewise(EmployeeDashboardDTO dto)
        {
            try
            {
                string Class_Id = "0";
                string Section_Id = "0";
                string startdate = "";
                string enddate = "";

                var fillclass = (from a in _PortalContext.Exm_Login_Privilege_SubjectsDMO
                                 from b in _PortalContext.Exm_Login_PrivilegeDMO
                                 from c in _PortalContext.IVRM_Staff_User_Login
                                 from d in _PortalContext.HR_Master_Employee_DMO
                                 where (b.ELP_Id == a.ELP_Id && c.IVRMSTAUL_Id == b.Login_Id && d.HRME_Id == c.Emp_Code && d.HRME_Id == dto.HRME_Id && b.ASMAY_Id == dto.ASMAY_Id && b.MI_Id == dto.MI_Id && b.ELP_Flg == "st")
                                 select new EmployeeDashboardDTO
                                 {
                                     ASMCL_Id = a.ASMCL_Id,
                                     //ASMS_Id=a.ASMS_Id,
                                     //ISMS_Id=a.ISMS_Id

                                 }).Distinct().ToArray();

                var fillsection = (from a in _PortalContext.Exm_Login_Privilege_SubjectsDMO
                                   from b in _PortalContext.Exm_Login_PrivilegeDMO
                                   from c in _PortalContext.IVRM_Staff_User_Login
                                   from d in _PortalContext.HR_Master_Employee_DMO
                                   where (b.ELP_Id == a.ELP_Id && c.IVRMSTAUL_Id == b.Login_Id && d.HRME_Id == c.Emp_Code && d.HRME_Id == dto.HRME_Id && b.ASMAY_Id == dto.ASMAY_Id && b.MI_Id == dto.MI_Id && b.ELP_Flg == "st")
                                   select new EmployeeDashboardDTO
                                   {

                                       ASMS_Id = a.ASMS_Id


                                   }).Distinct().ToArray();

                startdate = dto.INTB_StartDate.ToString("yyyy-MM-dd");
                enddate = dto.INTB_EndDate.ToString("yyyy-MM-dd");


                if (fillclass.Length > 0)
                {
                    for (var i = 0; i < fillclass.Length; i++)
                    {
                        Class_Id += "," + fillclass[i].ASMCL_Id;
                    }

                }
                if (fillsection.Length > 0)
                {
                    for (var i = 0; i < fillsection.Length; i++)
                    {
                        Section_Id += "," + fillsection[i].ASMS_Id;
                    }


                }



                using (var cmd = _PortalContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Portal_HomeWorkClasswork_datewise_Staff";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = dto.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.BigInt) { Value = dto.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = Class_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = Section_Id });
                    cmd.Parameters.Add(new SqlParameter("@type", SqlDbType.VarChar) { Value = "Classwork" });
                    cmd.Parameters.Add(new SqlParameter("@startdate", SqlDbType.VarChar) { Value = startdate });
                    cmd.Parameters.Add(new SqlParameter("@enddate", SqlDbType.VarChar) { Value = enddate });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = dto.HRME_Id });
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
                        dto.assignmentlist = retObject.ToArray();
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
            return dto;
        }
        public EmployeeDashboardDTO onclick_noticeboard_datewise(EmployeeDashboardDTO dto)
        {
            try
            {
                string Class_Id = "0";
                string Section_Id = "0";

                var fillclass = (from a in _PortalContext.Exm_Login_Privilege_SubjectsDMO
                                 from b in _PortalContext.Exm_Login_PrivilegeDMO
                                 from c in _PortalContext.IVRM_Staff_User_Login
                                 from d in _PortalContext.HR_Master_Employee_DMO
                                 where (b.ELP_Id == a.ELP_Id && c.IVRMSTAUL_Id == b.Login_Id && d.HRME_Id == c.Emp_Code && d.HRME_Id == dto.HRME_Id && b.ASMAY_Id == dto.ASMAY_Id && b.MI_Id == dto.MI_Id && b.ELP_Flg == "st")
                                 select new EmployeeDashboardDTO
                                 {
                                     ASMCL_Id = a.ASMCL_Id,
                                     //ASMS_Id=a.ASMS_Id,
                                     //ISMS_Id=a.ISMS_Id

                                 }).Distinct().ToArray();

                var fillsection = (from a in _PortalContext.Exm_Login_Privilege_SubjectsDMO
                                   from b in _PortalContext.Exm_Login_PrivilegeDMO
                                   from c in _PortalContext.IVRM_Staff_User_Login
                                   from d in _PortalContext.HR_Master_Employee_DMO
                                   where (b.ELP_Id == a.ELP_Id && c.IVRMSTAUL_Id == b.Login_Id && d.HRME_Id == c.Emp_Code && d.HRME_Id == dto.HRME_Id && b.ASMAY_Id == dto.ASMAY_Id && b.MI_Id == dto.MI_Id && b.ELP_Flg == "st")
                                   select new EmployeeDashboardDTO
                                   {

                                       ASMS_Id = a.ASMS_Id


                                   }).Distinct().ToArray();
                var clssec1 = _PortalContext.School_Adm_Y_StudentDMO.Where(a => a.ASMAY_Id == dto.ASMAY_Id && a.AMST_Id == dto.HRME_Id
                && a.AMAY_ActiveFlag == 1).ToList();


                if (fillclass.Length > 0)
                {
                    for (var i = 0; i < fillclass.Length; i++)
                    {
                        Class_Id += "," + fillclass[i].ASMCL_Id;
                    }

                }
                if (fillsection.Length > 0)
                {
                    for (var i = 0; i < fillsection.Length; i++)
                    {
                        Section_Id += "," + fillsection[i].ASMS_Id;
                    }


                }

                var date = DateTime.Now;
                using (var cmd = _PortalContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Portal_NoticeBoardYearWise_Datewise";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
             SqlDbType.VarChar)
                    {
                        Value = dto.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                    SqlDbType.VarChar)
                    {
                        Value = Class_Id
                        // Value=dto.ASMCL_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                SqlDbType.VarChar)
                    {
                        Value = dto.ASMAY_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@AMST_Id",
               SqlDbType.VarChar)
                    {
                        Value = dto.HRME_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@Flag",
                SqlDbType.VarChar)
                    {
                        Value = 'O'
                    });

                    cmd.Parameters.Add(new SqlParameter("@Type",
                SqlDbType.VarChar)
                    {
                        Value = "Staff"
                    });
                    cmd.Parameters.Add(new SqlParameter("@startdate",
               SqlDbType.VarChar)
                    {
                        Value = dto.INTB_StartDate
                    });

                    cmd.Parameters.Add(new SqlParameter("@enddate",
                SqlDbType.VarChar)
                    {
                        Value = dto.INTB_EndDate
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
                        dto.noticelist = retObject.ToArray();
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
            return dto;
        }

    }
}