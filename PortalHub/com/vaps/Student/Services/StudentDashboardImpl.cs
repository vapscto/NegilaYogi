using PreadmissionDTOs.com.vaps.Portals.Student;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs;
using System.Collections.Concurrent;
using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Text;
using System.IO;
using PreadmissionDTOs.com.vaps.Fees;
using DomainModel.Model.com.vaps.Fee;
using PreadmissionDTOs.com.vaps.admission;
using DomainModel.Model.com.vaps.admission;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using DataAccessMsSqlServerProvider.com.vapstech.Portals;
using DomainModel.Model.com.vapstech.Portals.Employee;
using System.Globalization;
using PreadmissionDTOs.com.vaps.Portals.Employee;
using DomainModel.Model.com.vapstech.Portals.Student;
using DomainModel.Model.com.vapstech.MobileApp;

namespace PortalHub.com.vaps.Student.Services
{
    public class StudentDashboardImpl : Interfaces.StudentDashboardInterface
    {
        private static ConcurrentDictionary<string, StudentDashboardDTO> _login =
           new ConcurrentDictionary<string, StudentDashboardDTO>();
        private PortalContext _studentDashboardContext;
        public StudentDashboardImpl(PortalContext studentDashboardContext)
        {
            _studentDashboardContext = studentDashboardContext;
        }
        public async Task<StudentDashboardDTO> Getdetails(StudentDashboardDTO data)
        {
            try
            {
                using (var cmd = _studentDashboardContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Gallery_List";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.BigInt)
                    {
                        Value = data.ASMAY_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.BigInt)
                    {
                        Value = data.AMST_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
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
                        data.gallerlist = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                data.yearlist = _studentDashboardContext.AcademicYearDMO.Where(a => a.MI_Id == data.MI_Id && a.Is_Active == true).OrderByDescending(a => a.ASMAY_Order).ToArray();

                var moId = _studentDashboardContext.Institute.Where(t => t.MI_Id == data.MI_Id && t.MI_ActiveFlag == 1).ToList();
                int moIDActive = 0;
                if (moId.Count > 0)
                {
                    moIDActive = _studentDashboardContext.Organisation.Where(t => t.MO_Id == moId.FirstOrDefault().MO_Id && t.MO_ActiveFlag == 1).Count();
                }
                using (var cmd = _studentDashboardContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Portal_DashboardAssetList";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.VarChar)
                    {
                        Value = 0
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
                        data.BookList = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }


                if (data.BookList.Length > 0)
                {
                    data.bookcount = data.BookList.Length;
                }
                else
                {

                    data.bookcount = 0;
                }
                if (moId.Count == 0)
                {
                    data.disabledint = true;
                    data.blockdashboard = true;
                    data.disableflag = "INT";
                    data.messag = geterrormessage(data);
                    return data;
                }
                if (moIDActive == 0)
                {
                    data.disabledorg = true;
                    data.blockdashboard = true;
                    data.disableflag = "ORG";
                    data.messag = geterrormessage(data);
                    return data;
                }
                if (data.disabledint == false && data.disabledorg == false)
                {
                    Master_Institution_SubscriptionValidity Subscriptiondetails = _studentDashboardContext.Master_Institution_SubscriptionValidity.Where(t => t.MI_Id.Equals(data.MI_Id)).FirstOrDefault();
                    DateTime subscriptionenddate = Convert.ToDateTime(Subscriptiondetails.MISV_ToDate);
                    DateTime curdate = DateTime.Now;
                    var subscri = Subscriptiondetails.MISV_ActiveFlag;
                    if (subscri == false)
                    {
                        data.disablesubscription = true;
                        data.blockdashboard = true;
                        data.disableflag = "SUBACTIVE";
                        data.messag = geterrormessage(data);
                        return data;
                    }
                    if (subscriptionenddate < curdate)
                    {
                        data.subscriptionover = true;
                        data.blockdashboard = true;
                        data.disableflag = "SUBDIS";
                        data.messag = geterrormessage(data);
                        return data;
                    }
                    var acdyear = _studentDashboardContext.AcademicYearDMO.Where(t => t.MI_Id.Equals(data.MI_Id) && t.Is_Active == true && Convert.ToDateTime(t.ASMAY_From_Date) <= Convert.ToDateTime(System.DateTime.Today.Date) && Convert.ToDateTime(t.ASMAY_To_Date) >= Convert.ToDateTime(System.DateTime.Today.Date)).Select(d => d.ASMAY_Id).FirstOrDefault();

                    if (acdyear != data.ASMAY_Id)
                    {
                        data.ASMAY_Id = acdyear;
                        data.updatedyear = acdyear;
                    }
                    else
                    {
                        data.updatedyear = 0;
                    }

                    var studenttctaken = _studentDashboardContext.Adm_M_Student.Where(t => t.MI_Id.Equals(data.MI_Id) && t.AMST_Id == data.AMST_Id
                    && t.AMST_SOL != "S").ToList();
                    if (studenttctaken != null && studenttctaken.Count() > 0)
                    {
                        data.tctaken = true;
                        data.tctakenalert = "Sorry, You Are Not Allowed to Access This Application!";
                    }
                    else
                    {
                        data.tctaken = false;
                        data.tctakenalert = "";
                    }

                    #region Display the text content

                    data.pagelist = (from a in _studentDashboardContext.Institution_Module_Page
                                     from b in _studentDashboardContext.Institution_Module
                                     from c in _studentDashboardContext.masterPage
                                     where a.IVRMIM_Id == b.IVRMIM_Id && a.IVRMP_Id == c.IVRMP_Id && b.MI_Id == data.MI_Id && a.IVRMIMP_Flag == 1
                                     && c.IVRMP_PageURL.Trim() == "app.studentDashboard"
                                     select new StudentDashboardDTO
                                     {
                                         IVRMIMP_DisplayContent = a.IVRMIMP_DisplayContent,
                                     }).Distinct().ToArray();


                    #endregion

                    #region Student Update Details
                    if (data.stdupdate == 0)
                    {
                        var dayscount = 0;
                        var configlist = _studentDashboardContext.GeneralConfigDMO.Where(f => f.MI_Id == data.MI_Id).ToList();
                        if (configlist.Count > 0)
                        {
                            data.updateflag = configlist.FirstOrDefault().IVRMGC_StudentDataChangeAlertFlg;
                            if (data.updateflag == true)
                            {
                                if (configlist.FirstOrDefault().IVRMGC_StudentDataChangeAlertDays == null)
                                {
                                    dayscount = 30;
                                }
                                else
                                {
                                    dayscount = Convert.ToInt32(configlist.FirstOrDefault().IVRMGC_StudentDataChangeAlertDays);
                                }

                                List<Country> allCountry = new List<Country>();
                                allCountry = await _studentDashboardContext.country.ToListAsync();
                                data.countryDrpDown = allCountry.ToArray();

                                List<State> Allstates = new List<State>();

                                Allstates = await _studentDashboardContext.state.ToListAsync();
                                data.stateDrpDown = Allstates.ToArray();

                                using (var cmd = _studentDashboardContext.Database.GetDbConnection().CreateCommand())
                                {
                                    cmd.CommandText = "GET_PORTAL_STUDENTDASHBOARD_UPDATE";
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
                                    cmd.Parameters.Add(new SqlParameter("@AMST_Id",
                              SqlDbType.BigInt)
                                    {
                                        Value = data.AMST_Id
                                    });
                                    cmd.Parameters.Add(new SqlParameter("@NoofDays",
                              SqlDbType.BigInt)
                                    {
                                        Value = dayscount
                                    });

                                    if (cmd.Connection.State != ConnectionState.Open)
                                        cmd.Connection.Open();

                                    var retObject = new List<dynamic>();
                                    try
                                    {
                                        using (var dataReader = await cmd.ExecuteReaderAsync())
                                        {
                                            while (await dataReader.ReadAsync())
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
                                        data.updatestudetailslist = retObject.ToArray();
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine(ex.Message);
                                    }
                                }
                            }
                            else
                            {
                                data.updateflag = false;
                            }

                        }
                    }
                    #endregion

                    #region Student Details
                    using (var cmd = _studentDashboardContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "PORTAL_StudentDashboard";
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
                        cmd.Parameters.Add(new SqlParameter("@AMST_Id",
                  SqlDbType.BigInt)
                        {
                            Value = data.AMST_Id
                        });


                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();

                        var retObject = new List<dynamic>();
                        try
                        {
                            using (var dataReader = await cmd.ExecuteReaderAsync())
                            {
                                while (await dataReader.ReadAsync())
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
                            data.studetailslist = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                    #endregion

                    #region Fee Details
                    using (var cmd = _studentDashboardContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "PORTAL_STUDENT_FEE_DETAILS";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@asmay_id",
                    SqlDbType.BigInt)
                        {
                            Value = data.ASMAY_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@amst_id",
                         SqlDbType.BigInt)
                        {
                            Value = data.AMST_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@mi_id", SqlDbType.BigInt)
                        {
                            Value = data.MI_Id
                        });


                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();

                        var retObject = new List<dynamic>();
                        try
                        {
                            using (var dataReader = await cmd.ExecuteReaderAsync())
                            {
                                while (await dataReader.ReadAsync())
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
                            data.studentfeedetails = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                    #endregion

                    #region Fee Monthly Details
                    using (var cmd = _studentDashboardContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "PORTAL_STUDENT_MONTHLY_FEE";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@asmay_id",
                    SqlDbType.BigInt)
                        {
                            Value = data.ASMAY_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@amst_id",
                         SqlDbType.BigInt)
                        {
                            Value = data.AMST_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@mi_id",
                  SqlDbType.BigInt)
                        {
                            Value = data.MI_Id
                        });


                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();

                        var retObject = new List<dynamic>();
                        try
                        {
                            using (var dataReader = await cmd.ExecuteReaderAsync())
                            {
                                while (await dataReader.ReadAsync())
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
                            data.academicyearFeedata = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                    #endregion

                    #region Student Monthly Attendance 
                    using (var cmd = _studentDashboardContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "PORTAL_STUDENT_MONTHLY_ATTENDANCE";
                        cmd.CommandType = CommandType.StoredProcedure;


                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                      SqlDbType.VarChar)
                        {
                            Value = data.ASMAY_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@mi_id",
                    SqlDbType.VarChar)
                        {
                            Value = data.MI_Id
                        });

                        cmd.Parameters.Add(new SqlParameter("@AMST_ID",
                        SqlDbType.VarChar)
                        {
                            Value = data.AMST_Id
                        });

                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();

                        var retObject = new List<dynamic>();
                        try
                        {
                            using (var dataReader = await cmd.ExecuteReaderAsync())
                            {
                                while (await dataReader.ReadAsync())
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
                            data.academicyearAttendancedata = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                    #endregion                    

                    #region  Class/Section
                    var clssec1 = (from a in _studentDashboardContext.Adm_M_Student
                                   from b in _studentDashboardContext.School_Adm_Y_StudentDMO
                                   from c in _studentDashboardContext.School_M_Class
                                   from s in _studentDashboardContext.School_M_Section
                                   where (b.ASMCL_Id == c.ASMCL_Id && b.ASMS_Id == s.ASMS_Id && a.MI_Id == c.MI_Id && a.MI_Id == data.MI_Id
                                   && b.ASMAY_Id == data.ASMAY_Id && a.AMST_Id == data.AMST_Id && b.AMST_Id == data.AMST_Id)
                                   select new StudentDashboardDTO
                                   {
                                       ASMCL_Id = c.ASMCL_Id,
                                       ASMCL_ClassName = c.ASMCL_ClassName,
                                       ASMS_Id = s.ASMS_Id,
                                       ASMC_SectionName = s.ASMC_SectionName
                                   }).Distinct().ToList();
                    #endregion

                    #region  PROMOTION STATUS
                    try
                    {
                        if (clssec1.Count > 0)
                        {
                            var getemcaid = _studentDashboardContext.Exm_Category_ClassDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                            && a.ASMCL_Id == clssec1.FirstOrDefault().ASMCL_Id && a.ASMS_Id == clssec1.FirstOrDefault().ASMS_Id && a.ECAC_ActiveFlag == true).ToList();

                            if (getemcaid.Count > 0)
                            {
                                var geteycid = _studentDashboardContext.Exm_Yearly_CategoryDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id &&
                                 a.EMCA_Id == getemcaid.FirstOrDefault().EMCA_Id && a.EYC_ActiveFlg == true
                                 && DateTime.Now.Date >= a.EYC_MarksPublishDate.Value.Date).ToList();

                                if (geteycid.Count > 0)
                                {
                                    data.promotionstatus = _studentDashboardContext.ExamPromotionRemarksDMO.Where(p => p.MI_Id == data.MI_Id
                                    && p.AMST_Id == data.AMST_Id && p.EPRD_Promotionflag == "PE" && p.ASMAY_Id == data.ASMAY_Id
                                    && p.ASMCL_Id == clssec1.FirstOrDefault().ASMCL_Id && p.ASMS_Id == clssec1.FirstOrDefault().ASMS_Id).Distinct().ToArray();
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    #endregion


                    //added by roopa//
                    //PDA
                    using (var cmd = _studentDashboardContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "Portal_pdadetails";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                    SqlDbType.BigInt)
                        {
                            Value = data.MI_Id
                        });

                        cmd.Parameters.Add(new SqlParameter("@AMST_Id",
                  SqlDbType.BigInt)
                        {
                            Value = data.AMST_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
            SqlDbType.BigInt)
                        {
                            Value = data.ASMAY_Id
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
                            data.pdadetails = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                    //Inventory
                    using (var cmd = _studentDashboardContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "Portal_Student_Inventory";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                    SqlDbType.BigInt)
                        {
                            Value = data.MI_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@AMST_Id",
                  SqlDbType.VarChar)
                        {
                            Value = data.AMST_Id
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
                            data.inventorydetails = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }

                    //


                    if (clssec1.Count == 0)
                    {
                        data.messag = "";
                    }
                    else
                    {
                        long Class_Id = clssec1.FirstOrDefault().ASMCL_Id;
                        long Section_Id = clssec1.FirstOrDefault().ASMS_Id;

                        #region  CALENDER / EVENTS
                        try
                        {
                            //&& n.COEE_EStartDate.Value.Month == month 
                            int month = DateTime.Now.Month;
                            data.coereportlist = (from m in _studentDashboardContext.COE_Master_EventsDMO
                                                  from n in _studentDashboardContext.COE_EventsDMO
                                                  from p in _studentDashboardContext.COE_Events_ClassesDMO
                                                  from o in _studentDashboardContext.School_Adm_Y_StudentDMO
                                                  where (m.COEME_Id == n.COEME_Id && n.COEE_Id == p.COEE_Id && p.ASMCL_Id == o.ASMCL_Id && n.MI_Id == data.MI_Id
                                                  && o.ASMAY_Id == data.ASMAY_Id && n.ASMAY_Id == data.ASMAY_Id && p.ASMCL_Id == Class_Id && o.ASMCL_Id == Class_Id
                                                  && o.ASMS_Id == Section_Id && n.COEE_EStartDate.Value.Month == month

                                                  && n.COEE_ActiveFlag == true
                                                  && n.ASMAY_Id == o.ASMAY_Id && o.AMST_Id == data.AMST_Id)
                                                  select new StudentDashboardDTO
                                                  {
                                                      COEME_EventName = m.COEME_EventName,
                                                      COEME_EventDesc = m.COEME_EventDesc,
                                                      COEE_EStartDate = n.COEE_EStartDate,
                                                      COEE_EEndDate = n.COEE_EEndDate,
                                                      COEE_ReminderDate = n.COEE_ReminderDate,
                                                      COEE_Id = n.COEE_Id,
                                                  }).OrderBy(c => c.COEE_EStartDate).Distinct().ToArray();

                            data.COE_ImageList = (from a in _studentDashboardContext.COE_Events_ImagesDMO
                                                  from b in _studentDashboardContext.COE_EventsDMO
                                                  from c in _studentDashboardContext.COE_Master_EventsDMO
                                                  where a.COEE_Id == b.COEE_Id && b.COEME_Id == c.COEME_Id && b.COEE_ActiveFlag == true && b.COEME_Id == c.COEME_Id
                                                  select new StudentDashboardDTO
                                                  {
                                                      COEME_EventName = c.COEME_EventName,
                                                      COEEI_Images = a.COEEI_Images,
                                                      COEE_Id = a.COEE_Id
                                                  }).ToArray();

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        #endregion

                        //#region  Notice Board 
                        //try
                        //{
                        //    var date = DateTime.Now;
                        //    using (var cmd = _studentDashboardContext.Database.GetDbConnection().CreateCommand())
                        //    {
                        //        cmd.CommandText = "Portal_NoticeBoardYearWise";
                        //        cmd.CommandType = CommandType.StoredProcedure;

                        //        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                        // SqlDbType.VarChar)
                        //        {
                        //            Value = data.MI_Id
                        //        });
                        //        cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                        //        SqlDbType.VarChar)
                        //        {
                        //            Value = Class_Id
                        //        });
                        //        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                        //    SqlDbType.VarChar)
                        //        {
                        //            Value = data.ASMAY_Id
                        //        });

                        //        if (cmd.Connection.State != ConnectionState.Open)
                        //            cmd.Connection.Open();

                        //        var retObject = new List<dynamic>();
                        //        try
                        //        {
                        //            using (var dataReader = await cmd.ExecuteReaderAsync())
                        //            {
                        //                while (await dataReader.ReadAsync())
                        //                {
                        //                    var dataRow = new ExpandoObject() as IDictionary<string, object>;
                        //                    for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                        //                    {
                        //                        dataRow.Add(
                        //                            dataReader.GetName(iFiled),
                        //                            dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                        //                        );
                        //                    }

                        //                    retObject.Add((ExpandoObject)dataRow);
                        //                }
                        //            }
                        //            data.noticelist = retObject.ToArray();
                        //        }
                        //        catch (Exception ex)
                        //        {
                        //            Console.WriteLine(ex.Message);
                        //        }
                        //    }
                        //}
                        //catch (Exception ex)
                        //{
                        //    Console.WriteLine(ex.Message);
                        //}
                        //#endregion

                        //#region Classwork / HomeWork 



                        //try
                        //{
                        //    #region
                        //    using (var cmd = _studentDashboardContext.Database.GetDbConnection().CreateCommand())
                        //    {
                        //        cmd.CommandText = "Portal_HomeWorkClasswork";
                        //        cmd.CommandType = CommandType.StoredProcedure;

                        //        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                        //    SqlDbType.BigInt)
                        //        {
                        //            Value = data.MI_Id
                        //        });

                        //        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                        //        SqlDbType.BigInt)
                        //        {
                        //            Value = data.ASMAY_Id
                        //        });
                        //        cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                        //        SqlDbType.BigInt)
                        //        {
                        //            Value = Class_Id
                        //        });

                        //        cmd.Parameters.Add(new SqlParameter("@ASMS_Id",
                        //         SqlDbType.BigInt)
                        //        {
                        //            Value = Section_Id
                        //        });
                        //        cmd.Parameters.Add(new SqlParameter("@type",
                        //         SqlDbType.VarChar)
                        //        {
                        //            Value = "Homework"
                        //        });


                        //        if (cmd.Connection.State != ConnectionState.Open)
                        //            cmd.Connection.Open();

                        //        var retObject = new List<dynamic>();
                        //        try
                        //        {
                        //            using (var dataReader = await cmd.ExecuteReaderAsync())
                        //            {
                        //                while (await dataReader.ReadAsync())
                        //                {
                        //                    var dataRow = new ExpandoObject() as IDictionary<string, object>;
                        //                    for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                        //                    {
                        //                        dataRow.Add(
                        //                            dataReader.GetName(iFiled),
                        //                            dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                        //                        );
                        //                    }

                        //                    retObject.Add((ExpandoObject)dataRow);
                        //                }
                        //            }
                        //            data.homeworklist = retObject.ToArray();
                        //        }
                        //        catch (Exception ex)
                        //        {
                        //            Console.WriteLine(ex.Message);
                        //        }



                        //    }
                        //    #endregion

                        data.homeworklist = (from a in _studentDashboardContext.IVRM_Homework_DMO
                                             from b in _studentDashboardContext.IVRM_Master_SubjectsDMO
                                             from c in _studentDashboardContext.School_Adm_Y_StudentDMO
                                             from d in _studentDashboardContext.AcademicYearDMO
                                             where (a.ISMS_Id == b.ISMS_Id && a.ASMCL_Id == c.ASMCL_Id && a.MI_Id == b.MI_Id && a.ASMAY_Id == d.ASMAY_Id && a.ASMAY_Id == data.ASMAY_Id
                                             && a.ASMCL_Id == Class_Id && a.ASMS_Id == Section_Id
                                             && a.MI_Id == data.MI_Id && c.AMAY_ActiveFlag == 1
                                             && a.IHW_ActiveFlag == true)
                                             select new StudentDashboardDTO
                                             {
                                                 IHW_Id = a.IHW_Id,
                                                 IHW_AssignmentNo = a.IHW_AssignmentNo,
                                                 IHW_Date = a.IHW_Date,
                                                 IHW_Topic = a.IHW_Topic,
                                                 IHW_Assignment = a.IHW_Assignment,
                                                 IHW_Attachment = a.IHW_Attachment,
                                                 ASMS_Id = a.ASMS_Id,
                                                 IHW_FilePath = a.IHW_FilePath,
                                                 ISMS_Id = a.ISMS_Id,
                                                 ISMS_SubjectName = b.ISMS_SubjectName,
                                                 ASMCL_Id = a.ASMCL_Id
                                             }).Distinct().OrderByDescending(d => d.IHW_Id).ToArray();

                        //    //******************************************* CLASS Work
                        //    using (var cmd = _studentDashboardContext.Database.GetDbConnection().CreateCommand())
                        //    {
                        //        cmd.CommandText = "Portal_HomeWorkClasswork";
                        //        cmd.CommandType = CommandType.StoredProcedure;

                        //        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                        //    SqlDbType.BigInt)
                        //        {
                        //            Value = data.MI_Id
                        //        });

                        //        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                        //        SqlDbType.BigInt)
                        //        {
                        //            Value = data.ASMAY_Id
                        //        });
                        //        cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                        //        SqlDbType.BigInt)
                        //        {
                        //            Value = Class_Id
                        //        });

                        //        cmd.Parameters.Add(new SqlParameter("@ASMS_Id",
                        //         SqlDbType.BigInt)
                        //        {
                        //            Value = Section_Id
                        //        });
                        //        cmd.Parameters.Add(new SqlParameter("@type",
                        //         SqlDbType.VarChar)
                        //        {
                        //            Value = "Classwork"
                        //        });


                        //        if (cmd.Connection.State != ConnectionState.Open)
                        //            cmd.Connection.Open();

                        //        var retObject = new List<dynamic>();
                        //        try
                        //        {
                        //            using (var dataReader = await cmd.ExecuteReaderAsync())
                        //            {
                        //                while (await dataReader.ReadAsync())
                        //                {
                        //                    var dataRow = new ExpandoObject() as IDictionary<string, object>;
                        //                    for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                        //                    {
                        //                        dataRow.Add(
                        //                            dataReader.GetName(iFiled),
                        //                            dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                        //                        );
                        //                    }

                        //                    retObject.Add((ExpandoObject)dataRow);
                        //                }
                        //            }
                        //            data.assignmentlist = retObject.ToArray();
                        //        }
                        //        catch (Exception ex)
                        //        {
                        //            Console.WriteLine(ex.Message);
                        //        }
                        //    }

                        //    //data.assignmentlist = (from m in _studentDashboardContext.IVRM_ClassWorkDMO
                        //    //                       from n in _studentDashboardContext.School_M_Class
                        //    //                       from o in _studentDashboardContext.School_M_Section
                        //    //                       from p in _studentDashboardContext.AcademicYearDMO
                        //    //                       from q in _studentDashboardContext.IVRM_Master_SubjectsDMO
                        //    //                       where (m.ASMCL_Id == n.ASMCL_Id && m.ASMS_Id == o.ASMS_Id && m.ISMS_Id == q.ISMS_Id && m.ASMAY_Id == p.ASMAY_Id && m.ASMAY_Id == data.ASMAY_Id && m.MI_Id == data.MI_Id
                        //    //                       && m.ASMCL_Id == Class_Id
                        //    //                       && m.ASMS_Id == Section_Id
                        //    //                       && m.ICW_ActiveFlag == true)
                        //    //                       select new StudentDashboardDTO
                        //    //                       {
                        //    //                           ASMCL_ClassName = n.ASMCL_ClassName,
                        //    //                           ICW_Content = m.ICW_Content,
                        //    //                           ICW_Topic = m.ICW_Topic,
                        //    //                           ICW_SubTopic = m.ICW_SubTopic,
                        //    //                           ICW_FromDate = m.ICW_FromDate,
                        //    //                           ICW_ToDate = m.ICW_ToDate,
                        //    //                           ICW_Assignment = m.ICW_Assignment,
                        //    //                           ICW_ActiveFlag = m.ICW_ActiveFlag,
                        //    //                           ICW_Id = m.ICW_Id,
                        //    //                           ICW_Evaluation = m.ICW_Evaluation,
                        //    //                           ICW_Attachment = m.ICW_Attachment,
                        //    //                           ICW_FilePath = m.ICW_FilePath,
                        //    //                           ASMCL_Id = m.ASMCL_Id,
                        //    //                           ISMS_SubjectName = q.ISMS_SubjectName,
                        //    //                       }).Distinct().OrderByDescending(d => d.ICW_FromDate).ToArray();



                        //}
                        //catch (Exception ex)
                        //{
                        //    Console.WriteLine(ex.Message);
                        //}
                        //#endregion

                        #region  Push Notifications
                        try
                        {
                            data.pushnotification = (from a in _studentDashboardContext.IVRM_PushNotificationDMO
                                                     from b in _studentDashboardContext.IVRM_PushNotification_Student_DMO
                                                     from c in _studentDashboardContext.Adm_M_Student
                                                     where (a.IPN_Id == b.IPN_Id && b.AMST_Id == c.AMST_Id && a.MI_Id == c.MI_Id && a.ASMAY_Id == c.ASMAY_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.IPN_ActiveFlag == true && c.AMST_SOL == "S" && b.AMST_Id == data.AMST_Id)
                                                     select new StudentDashboardDTO
                                                     {
                                                         IPN_Id = a.IPN_Id,
                                                         IPN_StuStaffFlg = a.IPN_StuStaffFlg,
                                                         IPN_PushNotification = a.IPN_PushNotification,
                                                         IPN_No = a.IPN_No,
                                                         IPN_Date = a.IPN_Date,
                                                         IPNS_Id = b.IPNS_Id,
                                                         AMST_Id = b.AMST_Id,
                                                         IVRMUL_Id = a.IVRMUL_Id,
                                                         AMST_FirstName = ((c.AMST_FirstName == null || c.AMST_FirstName == "" ? "" : " " + c.AMST_FirstName) + (c.AMST_MiddleName == null || c.AMST_MiddleName == "" || c.AMST_MiddleName == "0" ? "" : " " + c.AMST_MiddleName) + (c.AMST_LastName == null || c.AMST_LastName == "" || c.AMST_LastName == "0" ? "" : " " + c.AMST_LastName) + ":" + (c.AMST_AdmNo == null ? " " : c.AMST_AdmNo)).Trim(),
                                                     }).Distinct().OrderByDescending(d => d.IPN_Date).ToArray();

                            string datestring = DateTime.Now.AddDays(-30).ToString("dd-MM-yyyy", CultureInfo.InvariantCulture);
                            //DateTime startDate = DateTime.Parse(datestring);
                            DateTime expiryDate = Convert.ToDateTime(datestring);

                           // DateTime startDate= datestring.AddDays(TimeSpan.FromDays(10));





                            data.pushnotiflist = (from a in _studentDashboardContext.IVRM_PushNotificationDMO
                                                  from b in _studentDashboardContext.IVRM_PushNotification_Student_DMO
                                                  from c in _studentDashboardContext.Adm_M_Student
                                                  where (a.IPN_Id == b.IPN_Id && b.AMST_Id == c.AMST_Id && a.MI_Id == c.MI_Id && a.ASMAY_Id == c.ASMAY_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.IPN_ActiveFlag == true && c.AMST_SOL == "S" && b.AMST_Id == data.AMST_Id && a.IPN_Date >= expiryDate)
                                                  select new StudentDashboardDTO
                                                  {
                                                      IPN_Id = a.IPN_Id,
                                                      IPN_StuStaffFlg = a.IPN_StuStaffFlg,
                                                      IPN_PushNotification = a.IPN_PushNotification,
                                                      IPN_No = a.IPN_No,
                                                      IPN_Date = a.IPN_Date,
                                                      IPNS_Id = b.IPNS_Id,
                                                      AMST_Id = b.AMST_Id,
                                                      IVRMUL_Id = a.IVRMUL_Id,
                                                      AMST_FirstName = ((c.AMST_FirstName == null || c.AMST_FirstName == "" ? "" : " " + c.AMST_FirstName) + (c.AMST_MiddleName == null || c.AMST_MiddleName == "" || c.AMST_MiddleName == "0" ? "" : " " + c.AMST_MiddleName) + (c.AMST_LastName == null || c.AMST_LastName == "" || c.AMST_LastName == "0" ? "" : " " + c.AMST_LastName) + ":" + (c.AMST_AdmNo == null ? " " : c.AMST_AdmNo)).Trim(),
                                                  }).Distinct().OrderByDescending(d => d.IPN_Date).ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        #endregion

                        //   #region  Image/Video Gallery
                        //   using (var cmd = _studentDashboardContext.Database.GetDbConnection().CreateCommand())
                        //   {
                        //       cmd.CommandText = "Portal_DashboardImageGallery";
                        //       cmd.CommandType = CommandType.StoredProcedure;

                        //       cmd.Parameters.Add(new SqlParameter("@MI_Id",
                        //   SqlDbType.BigInt)
                        //       {
                        //           Value = data.MI_Id
                        //       });
                        //       cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                        //        SqlDbType.VarChar)
                        //       {
                        //           Value = Class_Id
                        //       });
                        //       cmd.Parameters.Add(new SqlParameter("@IGA_CommonGalleryFlg",
                        // SqlDbType.VarChar)
                        //       {
                        //           Value = data.IGA_CommonGalleryFlg
                        //       });
                        //       cmd.Parameters.Add(new SqlParameter("@Roleflg",
                        //SqlDbType.VarChar)
                        //       {
                        //           Value = "Student"
                        //       });

                        //       if (cmd.Connection.State != ConnectionState.Open)
                        //           cmd.Connection.Open();

                        //       var retObject = new List<dynamic>();
                        //       try
                        //       {
                        //           using (var dataReader = await cmd.ExecuteReaderAsync())
                        //           {
                        //               while (await dataReader.ReadAsync())
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
                        //           data.imagegallery = retObject.ToArray();
                        //       }
                        //       catch (Exception ex)
                        //       {
                        //           Console.WriteLine(ex.Message);
                        //       }
                        //   }

                        //   using (var cmd = _studentDashboardContext.Database.GetDbConnection().CreateCommand())
                        //   {
                        //       cmd.CommandText = "Portal_DashboardVideosGallery";
                        //       cmd.CommandType = CommandType.StoredProcedure;

                        //       cmd.Parameters.Add(new SqlParameter("@MI_Id",
                        //   SqlDbType.BigInt)
                        //       {
                        //           Value = data.MI_Id
                        //       });
                        //       cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                        //        SqlDbType.VarChar)
                        //       {
                        //           Value = Class_Id
                        //       });
                        //       cmd.Parameters.Add(new SqlParameter("@IGA_CommonGalleryFlg",
                        // SqlDbType.VarChar)
                        //       {
                        //           Value = data.IGA_CommonGalleryFlg
                        //       });
                        //       cmd.Parameters.Add(new SqlParameter("@Roleflg",
                        //SqlDbType.VarChar)
                        //       {
                        //           Value = "Student"
                        //       });

                        //       if (cmd.Connection.State != ConnectionState.Open)
                        //           cmd.Connection.Open();

                        //       var retObject = new List<dynamic>();
                        //       try
                        //       {
                        //           using (var dataReader = await cmd.ExecuteReaderAsync())
                        //           {
                        //               while (await dataReader.ReadAsync())
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
                        //           data.videogallery = retObject.ToArray();
                        //       }
                        //       catch (Exception ex)
                        //       {
                        //           Console.WriteLine(ex.Message);
                        //       }
                        //   }
                        //   #endregion
                    }

                    if (data.type != "" || data.type != null)
                    {
                        #region MobileapppagePrivileges

                        List<DataAccessMsSqlServerProvider.ApplicationUserRole> user = new List<DataAccessMsSqlServerProvider.ApplicationUserRole>();

                        user = _studentDashboardContext.ApplicationUserRole.Where(g => g.UserId == data.User_Id).ToList();

                        if (user.Count() > 0)
                        {

                            List<IVRM_Role_MobileApp_Privileges> Staffmobileappprivileges = new List<IVRM_Role_MobileApp_Privileges>();
                            Staffmobileappprivileges = _studentDashboardContext.IVRM_Role_MobileApp_Privileges.Where(f => f.IVRMRT_Id == data.roleid && f.MI_ID == data.MI_Id).ToList();

                            if (Staffmobileappprivileges.Count() > 0)
                            {

                                data.Staffmobileappprivileges = (from Mobilepage in _studentDashboardContext.IVRM_MobileApp_Page
                                                                 from MobileRolePrivileges in _studentDashboardContext.IVRM_Role_MobileApp_Privileges
                                                                 where (Mobilepage.IVRMMAP_Id == MobileRolePrivileges.IVRMMAP_Id && MobileRolePrivileges.IVRMRT_Id == data.roleid && MobileRolePrivileges.MI_ID == data.MI_Id && Mobilepage.IVRMMAP_ActiveFlg == true && MobileRolePrivileges.IVRMRMAP_ActiveFlg == true)
                                                                 select new LoginDTO
                                                                 {
                                                                     Pagename = Mobilepage.IVRMMAP_AppPageName,
                                                                     Pageicon = Mobilepage.IVRMMAP_AppPageDesc,
                                                                     Pageurl = Mobilepage.IVRMMAP_AppPageURL,
                                                                     IVRMRMAP_Id = MobileRolePrivileges.IVRMRMAP_Id,
                                                                     IVRMMAP_AddFlg = MobileRolePrivileges.IVRMMAP_AddFlg,
                                                                     IVRMMAP_UpdateFlg = MobileRolePrivileges.IVRMMAP_UpdateFlg,
                                                                     IVRMMAP_DeleteFlg = MobileRolePrivileges.IVRMMAP_DeleteFlg
                                                                 }).ToArray();

                                data.mobileprivileges = "true";
                            }
                            else
                            {
                                data.mobileprivileges = "false";
                            }
                        }
                        #endregion
                    }


                    //============student balance amount start===========
                    #region  Class/Section
                    var clssec2 = (from a in _studentDashboardContext.Adm_M_Student
                                   from b in _studentDashboardContext.School_Adm_Y_StudentDMO
                                   from c in _studentDashboardContext.School_M_Class
                                   from s in _studentDashboardContext.School_M_Section
                                   where (b.ASMCL_Id == c.ASMCL_Id && b.ASMS_Id == s.ASMS_Id && a.MI_Id == c.MI_Id && a.MI_Id == data.MI_Id
                                   && b.ASMAY_Id == data.ASMAY_Id && a.AMST_Id == data.AMST_Id && b.AMST_Id == data.AMST_Id)
                                   select new StudentDashboardDTO
                                   {
                                       ASMCL_Id = c.ASMCL_Id,
                                       ASMCL_ClassName = c.ASMCL_ClassName,
                                       ASMS_Id = s.ASMS_Id,
                                       ASMC_SectionName = s.ASMC_SectionName,
                                       AMST_AdmNo = a.AMST_AdmNo,
                                       AMST_FirstName = a.AMST_FirstName,
                                       AMST_MiddleName = a.AMST_MiddleName,
                                       AMST_LastName = a.AMST_LastName

                                   }).Distinct().ToList();

                    long Class_Id_t = clssec2.FirstOrDefault().ASMCL_Id;
                    data.student_details = clssec2.ToArray();

                    #endregion

                    data.rol_id = (from a in _studentDashboardContext.ApplicationUserRole
                                   from b in _studentDashboardContext.IVRM_Role_Type
                                   where a.RoleId == b.IVRMRT_Id && a.UserId == data.User_Id
                                   select new StudentDashboardDTO
                                   {
                                       IVRMRT_Role = b.IVRMRT_Role
                                   }).ToArray();


                    if (data.Feecheck != 1)
                    {
                        using (var cmd = _studentDashboardContext.Database.GetDbConnection().CreateCommand())
                        {
                            cmd.CommandText = "FEE_Balance_Amount_Show_in_portal";
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.Add(new SqlParameter("@MI_Id",
                     SqlDbType.VarChar)
                            {
                                Value = data.MI_Id
                            });
                            cmd.Parameters.Add(new SqlParameter("@AMST_Id",
                     SqlDbType.VarChar)
                            {
                                Value = data.AMST_Id
                            });
                            cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                            SqlDbType.VarChar)
                            {
                                Value = Class_Id_t
                            });
                            cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                        SqlDbType.VarChar)
                            {
                                Value = data.ASMAY_Id
                            });

                            if (cmd.Connection.State != ConnectionState.Open)
                                cmd.Connection.Open();

                            var retObject = new List<dynamic>();
                            try
                            {
                                using (var dataReader = await cmd.ExecuteReaderAsync())
                                {
                                    while (await dataReader.ReadAsync())
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
                                data.student_balance_list = retObject.ToArray();
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                        }

                        if (data.student_balance_list.Length > 0)
                        {
                            var fee = _studentDashboardContext.SMSEmailSetting.Where(a => a.MI_Id == data.MI_Id && a.ISES_Template_Name == "FEEPENDING").ToList();
                            if (fee.Count() > 0)
                            {
                                //data.feedetails = fee[0].ISES_MailHTMLTemplate;
                                data.feedetails = duadatecollect(data.MI_Id, data.AMST_Id, Class_Id_t, data.ASMAY_Id, "FEEPENDING");
                            }
                            //data.feedetails = fee[0].ISES_MailHTMLTemplate;
                        }
                        //============student balance amount End===========
                    }

                    TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                    DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

                    var getamstid = _studentDashboardContext.StudentAppUserLoginDMO_con.Where(a => a.STD_APP_ID == data.User_Id).ToList();

                    data.AMST_Id = getamstid.FirstOrDefault().AMST_ID;

                    var getstudentdetails = (from a in _studentDashboardContext.School_Adm_Y_StudentDMO
                                             from b in _studentDashboardContext.Adm_M_Student
                                             from c in _studentDashboardContext.AcademicYearDMO
                                             from d in _studentDashboardContext.School_M_Class
                                             from e in _studentDashboardContext.School_M_Section
                                             where (a.AMST_Id == b.AMST_Id && a.ASMAY_Id == c.ASMAY_Id && a.ASMCL_Id == d.ASMCL_Id && a.ASMS_Id == e.ASMS_Id
                                             && a.AMAY_ActiveFlag == 1 && b.AMST_SOL == "S" && b.AMST_ActiveFlag == 1 && b.MI_Id == data.MI_Id
                                             && a.AMST_Id == data.AMST_Id && a.ASMAY_Id == data.ASMAY_Id)
                                             select new StudentDashboardDTO
                                             {
                                                 AMST_Id = a.AMST_Id,
                                                 ASMCL_Id = a.ASMCL_Id,
                                                 ASMS_Id = a.ASMS_Id,
                                                 ASMAY_Id = a.ASMAY_Id,
                                                 AMST_Date = b.AMST_Date
                                             }).Distinct().ToList();

                    if (getstudentdetails.Count > 0)
                    {
                        var ASMCL_Id = getstudentdetails.FirstOrDefault().ASMCL_Id;
                        var ASMS_Id = getstudentdetails.FirstOrDefault().ASMS_Id;
                        var ASMAY_Id = getstudentdetails.FirstOrDefault().ASMAY_Id;
                        var AMST_Date = getstudentdetails.FirstOrDefault().AMST_Date;

                        var checkexamsubjects = _studentDashboardContext.LP_Master_OE_ExamDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == ASMAY_Id
                        && a.ASMCL_Id == ASMCL_Id && a.LPMOEEX_ActiveFlg == true).ToList();

                        List<long?> subjidd = new List<long?>();

                        foreach (var c in checkexamsubjects)
                        {
                            subjidd.Add(c.ISMS_Id);
                        }

                        var checksubjects = _studentDashboardContext.StudentMappingDMO.Where(a => a.MI_Id == data.MI_Id && a.AMST_Id == data.AMST_Id
                        && a.ASMAY_Id == ASMAY_Id && a.ESTSU_ActiveFlg == true && a.ASMCL_Id == ASMCL_Id && a.ASMS_Id == ASMS_Id
                        && subjidd.Contains(a.ISMS_Id)).ToList();

                        if (checksubjects.Count > 0)
                        {
                            List<long?> subjid = new List<long?>();

                            foreach (var c in checksubjects)
                            {
                                subjid.Add(c.ISMS_Id);
                            }

                            List<long> ids = new List<long>();

                            var getstudentexamids = _studentDashboardContext.LP_Students_ExamDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == ASMAY_Id
                            && a.AMST_Id == data.AMST_Id).Distinct().Select(a => a.LPMOEEX_Id);

                            var getstudentexamidsupload = (from a in _studentDashboardContext.LP_Students_ExamDMO
                                                           from c in _studentDashboardContext.LP_Master_OE_ExamDMO
                                                           where (a.LPMOEEX_Id == c.LPMOEEX_Id && a.MI_Id == data.MI_Id
                                                           && a.ASMAY_Id == ASMAY_Id && a.AMST_Id == data.AMST_Id && c.LPMOEEX_UploadExamPaperFlg == true)
                                                           select a).Distinct().Select(a => a.LPMOEEX_Id);

                            foreach (var c in getstudentexamidsupload)
                            {
                                ids.Add(c);
                            }

                            foreach (var c in getstudentexamids)
                            {
                                ids.Add(c);
                            }

                            //***************** Getting Today's Exam Details *********************//

                            var getexamdetails = (from a in _studentDashboardContext.LP_Master_OE_ExamDMO
                                                  from b in _studentDashboardContext.IVRM_Master_SubjectsDMO
                                                  where (a.ISMS_Id == b.ISMS_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == ASMAY_Id && a.ASMCL_Id == ASMCL_Id
                                                  && a.ASMS_Id == ASMS_Id && a.LPMOEEX_ActiveFlg == true && subjid.Contains(a.ISMS_Id)
                                                  && !ids.Contains(a.LPMOEEX_Id) 
                                                  && (a.LPMOEEX_FromDateTime.Value.Date == indiantime0.Date 
                                                  && a.LPMOEEX_ToDateTime.Value.Date == indiantime0.Date)
                                                  && a.LPMOEEX_FromDateTime.Value.Date > AMST_Date.Value.Date)
                                                  select new StudentDashboardDTO
                                                  {
                                                      LPMOEEX_ExamName = a.LPMOEEX_ExamName,
                                                      ISMS_SubjectName = b.ISMS_SubjectName,
                                                      ExamStartDateTime = a.LPMOEEX_FromDateTime,
                                                      ExamEndDateTime = a.LPMOEEX_ToDateTime,
                                                      LPMOEEX_ExamDuration = a.LPMOEEX_ExamDuration,
                                                  }).Distinct().OrderBy(a => a.ExamStartDateTime).ThenBy(a => a.ExamEndDateTime).ToArray();

                            data.gettodaysexamdetails = getexamdetails;
                        }
                    }


                    //Student Percentage 


                   // var shortageattalertflg = _studentDashboardContext.GeneralConfigDMO.Where(s => s.IVRMGC_AttendanceShortageAlertFlg == true && s.MI_Id == data.MI_Id).Select(m => m.IVRMGC_AttendanceShortageAlertFlg).FirstOrDefault();

                    //if (shortageattalertflg == true)
                    //{
                    //    var shortageattalert_perc = _studentDashboardContext.GeneralConfigDMO.Where(s => s.IVRMGC_AttendanceShortageAlertFlg == true && s.MI_Id == data.MI_Id).Select(m => m.IVRMGC_AttendanceShortagePercent).FirstOrDefault();

                        using (var cmd = _studentDashboardContext.Database.GetDbConnection().CreateCommand())
                        {
                            cmd.CommandText = "AttendanceDashboard_perc";
                            cmd.CommandType = CommandType.StoredProcedure;
                            //@ASMAY_Id
                            cmd.Parameters.Add(new SqlParameter("@mi_id", SqlDbType.BigInt)
                            {
                                Value = Convert.ToInt64(data.MI_Id)
                            });
                            cmd.Parameters.Add(new SqlParameter("@ASMAY_ID", SqlDbType.BigInt)
                            {
                                Value = Convert.ToInt64(data.ASMAY_Id)
                            });

                            cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.BigInt)
                            {
                                Value = data.AMST_Id
                            });

                            //cmd.Parameters.Add(new SqlParameter("@Percentage", SqlDbType.VarChar)
                            //{
                            //    Value = shortageattalert_perc
                            //});
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
                              data.studentAttendanceList = retObject.ToArray();
                            }

                            catch (Exception ex)
                            {
                                Console.Write(ex.Message);
                            }
                        }
                    //}
                }

                
                    
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return data;
        }
        public async Task<StudentDashboardDTO> getImages(StudentDashboardDTO data)
        {
            try
            {
                #region Gallery Images/Videos
                using (var cmd = _studentDashboardContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Portal_Images_Videos";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@IGA_Id",
                     SqlDbType.VarChar)
                    {
                        Value = data.IGA_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@fileflg",
              SqlDbType.VarChar)
                    {
                        Value = "I"
                    });
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();
                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader = await cmd.ExecuteReaderAsync())
                        {
                            while (await dataReader.ReadAsync())
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
                        data.get_Images = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                #endregion             
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return data;
        }
        public StudentDashboardDTO saveakpkfile(StudentDashboardDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);

                IVRM_MobileApp_Download_DMO objdata = new IVRM_MobileApp_Download_DMO();


                objdata.MI_Id = data.MI_Id;
                objdata.IVRMUL_Id = data.IVRMUL_Id;
                objdata.IVRMMAD_MobileModel = "Student";
                objdata.IVRMMAD_DownlaodDateTime = indianTime;
                objdata.CreatedDate = indianTime;
                objdata.UpdatedDate = indianTime;

                _studentDashboardContext.Add(objdata);
                int rowAffected = _studentDashboardContext.SaveChanges();
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
        public StudentDashboardDTO viewData(StudentDashboardDTO dto)
        {
            try
            {
                if (dto.COEE_Id == 0)
                {
                    dto.attachementlist = (from a in _studentDashboardContext.IVRM_HomeWork_Attatchment_DMO_con
                                           from b in _studentDashboardContext.IVRM_Homework_DMO
                                           where a.IHW_Id == dto.IHW_Id && a.IHWATT_ActiveFlag == true && a.IHW_Id == b.IHW_Id
                                           select new StudentDashboardDTO
                                           {
                                               IHWATT_Attachment = a.IHWATT_Attachment,
                                               IHW_Attachment = b.IHW_Attachment,
                                               IHWATT_FileName = a.IHWATT_FileName,
                                               IHW_Id = a.IHW_Id
                                           }).ToArray();

                    dto.attachementlist1 = (from a in _studentDashboardContext.IVRM_ClassWork_Attatchment_DMO_con
                                            from b in _studentDashboardContext.IVRM_ClassWorkDMO
                                            where a.ICW_Id == dto.ICW_Id && a.ICW_Id == b.ICW_Id
                                            select new IVRM_ClassWorkDTO
                                            {
                                                ICW_Id = b.ICW_Id,
                                                ICW_Attachment = b.ICW_Attachment,
                                                ICWATT_Attachment = a.ICWATT_Attachment,
                                                ICWATT_FileName = a.ICWATT_FileName
                                            }).ToArray();
                }
                else
                {
                    dto.attachementlist = (from a in _studentDashboardContext.COE_Events_ImagesDMO
                                           from b in _studentDashboardContext.COE_EventsDMO
                                           from c in _studentDashboardContext.COE_Master_EventsDMO
                                           where a.COEE_Id == b.COEE_Id && b.COEME_Id == c.COEME_Id && b.COEE_ActiveFlag == true && a.COEE_Id == dto.COEE_Id
                                           select new StudentDashboardDTO
                                           {
                                               COEME_EventName = c.COEME_EventName,
                                               COEEI_Images = a.COEEI_Images,
                                               COEE_Id = a.COEE_Id

                                           }).ToArray();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return dto;
        }
        public StudentDashboardDTO savecls_doc(StudentDashboardDTO dto)
        {
            try
            {
                var check = _studentDashboardContext.IVRM_ClassWork_Upload_DMO_con.Where(a => a.ICW_Id == dto.ICW_Id && a.AMST_Id == dto.AMST_Id).ToList();
                if (check.Count > 0)
                {
                    var getclassworkattach = _studentDashboardContext.IVRM_ClassWork_Upload_Attatchment_DMO_con.Where(a => a.ICWUPL_Id == check.FirstOrDefault().ICWUPL_Id).ToList();

                    foreach (var d in getclassworkattach)
                    {
                        _studentDashboardContext.Remove(d);
                    }

                    //_studentDashboardContext.Remove(check);
                }

                //IVRM_ClassWork_Upload_DMO iVRM_ClassWork_Upload_DMO = new IVRM_ClassWork_Upload_DMO();
                //iVRM_ClassWork_Upload_DMO.AMST_Id = dto.AMST_Id;
                //iVRM_ClassWork_Upload_DMO.ICW_Id = dto.ICW_Id;
                //iVRM_ClassWork_Upload_DMO.ICWUPL_Date = DateTime.Now;
                //iVRM_ClassWork_Upload_DMO.ICWUPL_ActiveFlag = true;
                //iVRM_ClassWork_Upload_DMO.CreatedDate = DateTime.Now;
                //iVRM_ClassWork_Upload_DMO.UpdatedDate = DateTime.Now;
                //_studentDashboardContext.Add(iVRM_ClassWork_Upload_DMO);

                //foreach (var item in dto.uploaddoc_array)
                //{
                //    if (item.DCO_Attachment != null && item.DCO_Attachment != "")
                //    {
                //        IVRM_ClassWork_Upload_Attatchment_DMO cud = new IVRM_ClassWork_Upload_Attatchment_DMO();
                //        cud.ICWUPL_Id = iVRM_ClassWork_Upload_DMO.ICWUPL_Id;
                //        cud.ICWUPLATT_FileName = item.Doc_FileName;
                //        cud.ICWUPLATT_Attachment = item.DCO_Attachment;
                //        cud.ICWUPLATT_ActiveFlag = true;
                //        cud.ICWUPLATT_CreatedDate = DateTime.Now;
                //        cud.ICWUPLATT_UpdatedDate = DateTime.Now;
                //        _studentDashboardContext.Add(cud);
                //    }
                //}

                var uploadcheck = _studentDashboardContext.IVRM_ClassWork_Upload_DMO_con.Where(s => s.ICW_Id == dto.ICW_Id && s.AMST_Id == dto.AMST_Id).ToList();
                if (uploadcheck.Count > 0)
                {
                    var uploadrecord = _studentDashboardContext.IVRM_ClassWork_Upload_DMO_con.Single(a => a.ICW_Id == dto.ICW_Id && a.AMST_Id == dto.AMST_Id);
                    uploadrecord.ICWUPL_Date = DateTime.Now;
                    uploadrecord.CreatedDate = DateTime.Now;
                    uploadrecord.UpdatedDate = DateTime.Now;
                    uploadrecord.ICWUPL_ActiveFlag = true;
                    _studentDashboardContext.Update(uploadrecord);

                    foreach (var item in dto.uploaddoc_array)
                    {
                        if (item.DCO_Attachment != null && item.DCO_Attachment != "")
                        {
                            IVRM_ClassWork_Upload_Attatchment_DMO cud = new IVRM_ClassWork_Upload_Attatchment_DMO();
                            cud.ICWUPL_Id = uploadcheck.FirstOrDefault().ICWUPL_Id;
                            cud.ICWUPLATT_FileName = item.Doc_FileName;
                            cud.ICWUPLATT_Attachment = item.DCO_Attachment;
                            cud.ICWUPLATT_ActiveFlag = true;
                            cud.ICWUPLATT_CreatedDate = DateTime.Now;
                            cud.ICWUPLATT_UpdatedDate = DateTime.Now;
                            _studentDashboardContext.Add(cud);
                        }
                    }
                }
               
                var result = _studentDashboardContext.IVRM_ClassWorkDMO.Single(a => a.ICW_Id == dto.ICW_Id && a.MI_Id == dto.MI_Id);
                result.ICW_FilePath = "1";
                _studentDashboardContext.Update(result);
                var std = _studentDashboardContext.SaveChanges();
                if (std > 0)
                {
                    dto.returnval = true;
                }
                else
                {
                    dto.returnval = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return dto;
        }
        public StudentDashboardDTO savehome_doc(StudentDashboardDTO dto)
        {
            try
            {
                var check = _studentDashboardContext.IVRM_HomeWork_Upload_DMO_con.Where(a => a.IHW_Id == dto.IHW_Id && a.AMST_Id == dto.AMST_Id).ToList();
                if (check.Count > 0)
                {
                    var gethomeworkattach = _studentDashboardContext.IVRM_HomeWork_Upload_Attatchment_DMO_con.Where(a => a.IHWUPL_Id == check.FirstOrDefault().IHWUPL_Id).ToList();

                    foreach (var d in gethomeworkattach)
                    {
                        _studentDashboardContext.Remove(d);
                    }

                    //_studentDashboardContext.Remove(check);
                }

                var uploadcheck = _studentDashboardContext.IVRM_HomeWork_Upload_DMO_con.Where(s => s.IHW_Id == dto.IHW_Id && s.AMST_Id == dto.AMST_Id).ToList();
                if (uploadcheck.Count > 0)
                {

                    var uploadrecord = _studentDashboardContext.IVRM_HomeWork_Upload_DMO_con.Single(a => a.IHW_Id == dto.IHW_Id && a.AMST_Id == dto.AMST_Id);
                    uploadrecord.IHWUPL_Date = DateTime.Now;
                    uploadrecord.CreatedDate = DateTime.Now;
                    uploadrecord.UpdatedDate = DateTime.Now;
                    uploadrecord.IHWUPL_ActiveFlag = true;
                    _studentDashboardContext.Update(uploadrecord);


                    foreach (var item in dto.uploaddoc_array)
                    {
                        if (item.DCO_Attachment != null && item.DCO_Attachment != "")
                        {
                            IVRM_HomeWork_Upload_Attatchment_DMO iVRM_HomeWork_Upload_Attatchment_DMO = new IVRM_HomeWork_Upload_Attatchment_DMO();
                            iVRM_HomeWork_Upload_Attatchment_DMO.IHWUPL_Id = uploadcheck.FirstOrDefault().IHWUPL_Id;
                            iVRM_HomeWork_Upload_Attatchment_DMO.IHWUPLATT_FileName = item.Doc_FileName;
                            iVRM_HomeWork_Upload_Attatchment_DMO.IHWUPLATT_Attachment = item.DCO_Attachment;
                            iVRM_HomeWork_Upload_Attatchment_DMO.IHWUPLATT_ActiveFlag = true;
                            iVRM_HomeWork_Upload_Attatchment_DMO.IHWUPLATT_CreatedDate = DateTime.Now;
                            iVRM_HomeWork_Upload_Attatchment_DMO.IHWUPLATT_UpdatedDate = DateTime.Now;
                            _studentDashboardContext.Add(iVRM_HomeWork_Upload_Attatchment_DMO);
                        }
                    }
                }
                //else
                //{
                //    IVRM_HomeWork_Upload_DMO cud = new IVRM_HomeWork_Upload_DMO();
                //    cud.AMST_Id = dto.AMST_Id;
                //    cud.IHW_Id = dto.IHW_Id;
                //    cud.IHWUPL_Date = DateTime.Now;
                //    cud.CreatedDate = DateTime.Now;
                //    cud.UpdatedDate = DateTime.Now;
                //    cud.IHWUPL_ActiveFlag = true;
                //    _studentDashboardContext.Add(cud);

                //    foreach (var item in dto.uploaddoc_array)
                //    {
                //        if (item.DCO_Attachment != null && item.DCO_Attachment != "")
                //        {
                //            IVRM_HomeWork_Upload_Attatchment_DMO iVRM_HomeWork_Upload_Attatchment_DMO = new IVRM_HomeWork_Upload_Attatchment_DMO();
                //            iVRM_HomeWork_Upload_Attatchment_DMO.IHWUPL_Id = cud.IHWUPL_Id;
                //            iVRM_HomeWork_Upload_Attatchment_DMO.IHWUPLATT_FileName = item.Doc_FileName;
                //            iVRM_HomeWork_Upload_Attatchment_DMO.IHWUPLATT_Attachment = item.DCO_Attachment;
                //            iVRM_HomeWork_Upload_Attatchment_DMO.IHWUPLATT_ActiveFlag = true;
                //            iVRM_HomeWork_Upload_Attatchment_DMO.IHWUPLATT_CreatedDate = DateTime.Now;
                //            iVRM_HomeWork_Upload_Attatchment_DMO.IHWUPLATT_UpdatedDate = DateTime.Now;
                //            _studentDashboardContext.Add(iVRM_HomeWork_Upload_Attatchment_DMO);
                //        }
                //    }
                //}

                var result = _studentDashboardContext.IVRM_Homework_DMO.Single(a => a.IHW_Id == dto.IHW_Id && a.MI_Id == dto.MI_Id);
                result.IHW_FilePath = "1";
                _studentDashboardContext.Update(result);
                var std = _studentDashboardContext.SaveChanges();
                if (std > 0)
                {
                    dto.returnval = true;
                }
                else
                {
                    dto.returnval = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return dto;
        }
        public StudentDashboardDTO viewData_doc(StudentDashboardDTO dto)
        {
            try
            {
                // Home Work View

                if (dto.IHW_Id > 0)
                {
                    dto.attachementlist = (from a in _studentDashboardContext.IVRM_HomeWork_Upload_DMO_con
                                           from b in _studentDashboardContext.IVRM_HomeWork_Upload_Attatchment_DMO_con
                                           where (a.IHWUPL_Id == b.IHWUPL_Id && a.IHW_Id == dto.IHW_Id && a.AMST_Id == dto.AMST_Id && a.IHWUPL_ActiveFlag == true)
                                           select new StudentDashboardDTO
                                           {
                                               IHWUPL_FileName = b.IHWUPLATT_FileName,
                                               IHWUPL_Attachment = b.IHWUPLATT_Attachment
                                           }).Distinct().ToArray();
                }

                // Class Work View
                if (dto.ICW_Id > 0)
                {
                    dto.attachementlist1 = (from a in _studentDashboardContext.IVRM_ClassWork_Upload_DMO_con
                                            from b in _studentDashboardContext.IVRM_ClassWork_Upload_Attatchment_DMO_con
                                            where (a.ICWUPL_Id == b.ICWUPL_Id && a.ICW_Id == dto.ICW_Id && a.AMST_Id == dto.AMST_Id
                                            && a.ICWUPL_ActiveFlag == true)
                                            select new StudentDashboardDTO
                                            {
                                                ICWUPL_FileName = b.ICWUPLATT_FileName,
                                                ICWUPL_Attachment = b.ICWUPLATT_Attachment
                                            }).Distinct().ToArray();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return dto;
        }
        public StudentDashboardDTO saverequest(StudentDashboardDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);

                if (data.ASTUREQ_Id > 0)
                {
                    var updatelist = _studentDashboardContext.Adm_Student_Update_RequestDMO.Single(d => d.ASTUREQ_Id == data.ASTUREQ_Id);
                    updatelist.ASTUREQ_PerStreet = data.STP_PERSTREET;
                    updatelist.ASTUREQ_PerArea = data.STP_PERAREA;
                    updatelist.ASTUREQ_PerCity = data.STP_PERCITY;
                    updatelist.ASTUREQ_PerState = data.STP_PERSTATE;
                    updatelist.IVRMMC_Id = data.STP_PERCOUNTRY;
                    updatelist.ASTUREQ_PerPincode = data.STP_PERPIN;
                    updatelist.ASTUREQ_ConStreet = data.STP_CURSTREET;
                    updatelist.ASTUREQ_ConArea = data.STP_CURAREA;
                    updatelist.ASTUREQ_ConCity = data.STP_CURCITY;
                    updatelist.ASTUREQ_ConState = data.STP_CURSTATE;
                    updatelist.ASTUREQ_ConCountryId = data.STP_CURCOUNTRY;
                    updatelist.ASTUREQ_ConPincode = data.STP_CURPIN;
                    updatelist.ASTUREQ_ConPincode = data.STP_CURPIN;
                    updatelist.ASTUREQ_UpdatedDate = indianTime;
                    updatelist.ASTUREQ_Date = indianTime;
                    updatelist.ASTUREQ_CreatedDate = indianTime;
                    updatelist.ASTUREQ_UpdatedBy = data.User_Id;
                    updatelist.ASTUREQ_MobileNo = data.Mobilenumber;
                    updatelist.ASTUREQ_EmailId = data.EmailidforCandidate;
                    updatelist.ASTUREQ_FatherMobleNo = data.AMST_FatherMobleNo;
                    updatelist.ASTUREQ_FatheremailId = data.AMST_FatheremailId;
                    updatelist.ASTUREQ_MotheremailId = data.AMST_MotherEmailId;
                    updatelist.ASTUREQ_MotherMobleNo = data.AMST_MotherMobileNo;
                    updatelist.ASTUREQ_BloodGroup = data.AMST_BloodGroup;
                    updatelist.ASTUREQ_ReqStatus = "PENDING";
                    updatelist.ASTUREQ_ChangeConfirmFlg = "CHANGE";

                    if (data.AMSTG_Id > 0)
                    {
                        updatelist.AMSTG_Id = data.AMSTG_Id;
                        updatelist.ASTUREQ_GuardianMobileNo = data.ASTUREQ_GuardianMobileNo;
                        updatelist.ASTUREQ_GuardianEmailId = data.ASTUREQ_GuardianEmailId;
                    }

                    _studentDashboardContext.Update(updatelist);
                    int rx = _studentDashboardContext.SaveChanges();
                    if (rx > 0)
                    {
                        data.returnval = true;
                    }
                    else
                    {
                        data.returnval = true;
                    }
                }
                else
                {


                    Adm_Student_Update_RequestDMO obj = new Adm_Student_Update_RequestDMO();

                    obj.MI_Id = data.MI_Id;
                    obj.ASMAY_Id = data.ASMAY_Id;
                    obj.AMST_Id = data.AMST_Id;
                    obj.ASTUREQ_PerStreet = data.STP_PERSTREET;
                    obj.ASTUREQ_PerArea = data.STP_PERAREA;
                    obj.ASTUREQ_PerCity = data.STP_PERCITY;
                    obj.ASTUREQ_PerState = data.STP_PERSTATE;
                    obj.IVRMMC_Id = data.STP_PERCOUNTRY;
                    obj.ASTUREQ_PerPincode = data.STP_PERPIN;
                    obj.ASTUREQ_ConStreet = data.STP_CURSTREET;
                    obj.ASTUREQ_ConArea = data.STP_CURAREA;
                    obj.ASTUREQ_ConCity = data.STP_CURCITY;
                    obj.ASTUREQ_ConState = data.STP_CURSTATE;
                    obj.ASTUREQ_ConCountryId = data.STP_CURCOUNTRY;
                    obj.ASTUREQ_ConPincode = data.STP_CURPIN;
                    obj.ASTUREQ_ConPincode = data.STP_CURPIN;
                    obj.ASTUREQ_UpdatedDate = indianTime;
                    obj.ASTUREQ_Date = indianTime;
                    obj.ASTUREQ_CreatedDate = indianTime;
                    obj.ASTUREQ_UpdatedBy = data.User_Id;
                    obj.ASTUREQ_ActiveFlag = true;
                    obj.ASTUREQ_BloodGroup = data.AMST_BloodGroup;
                    obj.ASTUREQ_ApprovedFlg = false;
                    obj.ASTUREQ_MobileNo = data.Mobilenumber;
                    obj.ASTUREQ_EmailId = data.EmailidforCandidate;
                    obj.ASTUREQ_FatherMobleNo = data.AMST_FatherMobleNo;
                    obj.ASTUREQ_FatheremailId = data.AMST_FatheremailId;
                    obj.ASTUREQ_MotheremailId = data.AMST_MotherEmailId;
                    obj.ASTUREQ_MotherMobleNo = data.AMST_MotherMobileNo;
                    obj.ASTUREQ_ReqStatus = "PENDING";
                    obj.ASTUREQ_ChangeConfirmFlg = "CHANGE";
                    if (data.AMSTG_Id > 0)
                    {
                        obj.AMSTG_Id = data.AMSTG_Id;
                        obj.ASTUREQ_GuardianMobileNo = data.ASTUREQ_GuardianMobileNo;
                        obj.ASTUREQ_GuardianEmailId = data.ASTUREQ_GuardianEmailId;
                    }

                    var existdata = _studentDashboardContext.Adm_M_Student.Where(a => a.AMST_Id == data.AMST_Id && a.MI_Id == data.MI_Id && a.AMST_ActiveFlag == 1).ToList();
                    if (existdata.Count > 0)
                    {
                        obj.ASTUREQ_ApplStatus = existdata[0].AMST_ApplStatus;
                        obj.ASTUREQ_FirstName = existdata[0].AMST_FirstName;
                        obj.ASTUREQ_MiddleName = existdata[0].AMST_MiddleName;
                        obj.ASTUREQ_LastName = existdata[0].AMST_LastName;
                        obj.ASTUREQ_RegistrationNo = existdata[0].AMST_RegistrationNo;
                        obj.ASTUREQ_AdmNo = existdata[0].AMST_AdmNo;
                        obj.AMC_Id = existdata[0].AMC_Id;
                        obj.ASTUREQ_Sex = existdata[0].AMST_Sex;
                        obj.ASTUREQ_DOB = existdata[0].AMST_DOB;
                        obj.ASTUREQ_DOBinwords = existdata[0].AMST_DOB_Words;
                        obj.ASTUREQ_Age = existdata[0].PASR_Age;
                        obj.ASMCL_Id = existdata[0].ASMCL_Id;
                        // obj.ASTUREQ_BloodGroup = existdata[0].AMST_BloodGroup;
                        obj.ASTUREQ_MotherTongue = existdata[0].AMST_MotherTongue;
                        obj.ASTUREQ_HomeLaguage = existdata[0].AMST_LanguageSpoken;
                        obj.ASTUREQ_BirthCertNo = existdata[0].AMST_BirthCertNO;
                        obj.IVRMMR_Id = existdata[0].IVRMMR_Id;
                        obj.IMCC_Id = existdata[0].IMCC_Id;
                        //  obj.IMC_Id = existdata[0].IC_Id;
                        obj.ASTUREQ_StudentSubCaste = existdata[0].AMST_SubCasteIMC_Id;
                        obj.ASTUREQ_PerAdd3 = existdata[0].AMST_PerAdd3;
                        // obj.ASTUREQ_ConAdd3 = existdata[0].AMST_PerAdd3;
                        obj.ASTUREQ_Village = existdata[0].AMST_Village;
                        obj.ASTUREQ_Taluk = existdata[0].AMST_Taluk;
                        obj.ASTUREQ_District = existdata[0].AMST_Distirct;
                        obj.ASTUREQ_AadharNo = existdata[0].AMST_AadharNo;
                        obj.ASTUREQ_StuBankAccNo = existdata[0].AMST_StuBankAccNo;
                        obj.ASTUREQ_StudentPANCard = existdata[0].AMST_StudentPANNo;
                        obj.ASTUREQ_StuBankIFSCCode = existdata[0].AMST_StuBankIFSC_Code;
                        obj.ASTUREQ_StuCasteCertiNo = existdata[0].AMST_StuCasteCertiNo;
                        obj.ASTUREQ_FatherAliveFlag = existdata[0].AMST_FatherAliveFlag;
                        obj.ASTUREQ_FatherMaritalStatusFlg = existdata[0].AMST_FatherMaritalStatus;
                        obj.ASTUREQ_FatherName = existdata[0].AMST_FatherName;
                        obj.ASTUREQ_FatherAadharNo = existdata[0].AMST_FatherAadharNo;
                        obj.ASTUREQ_FatherSurname = existdata[0].AMST_FatherSurname;
                        obj.ASTUREQ_FatherEducation = existdata[0].AMST_FatherEducation;
                        obj.ASTUREQ_FatherOccupation = existdata[0].AMST_FatherOccupation;
                        obj.ASTUREQ_FatherOfficeAdd = existdata[0].AMST_FatherOfficeAdd;
                        obj.ASTUREQ_FatherDesignation = existdata[0].AMST_FatherDesignation;
                        obj.ASTUREQ_FatherMonIncome = existdata[0].AMST_FatherMonIncome;
                        obj.ASTUREQ_FatherAnnIncome = existdata[0].AMST_FatherAnnIncome;
                        obj.ASTUREQ_FatherNationality = existdata[0].AMST_FatherNationality;


                        obj.ASTUREQ_FatherReligion = existdata[0].AMST_FatherReligion;


                        obj.ASTUREQ_FatherCaste = existdata[0].AMST_FatherCaste;


                        obj.ASTUREQ_FatherSubCaste = existdata[0].AMST_FatherSubCaste;
                        obj.ASTUREQ_FatherBankAccNo = existdata[0].AMST_FatherBankAccNo;
                        obj.ASTUREQ_FatherBankIFSCCode = existdata[0].AMST_FatherBankIFSC_Code;
                        obj.ASTUREQ_FatherCasteCertiNo = existdata[0].AMST_FatherCasteCertiNo;
                        obj.ASTUREQ_FatherPhoto = existdata[0].ANST_FatherPhoto;
                        obj.ASTUREQ_FatherSign = existdata[0].AMST_Father_Signature;
                        obj.ASTUREQ_FatherFingerprint = existdata[0].AMST_Father_FingerPrint;
                        obj.ASTUREQ_FatherPANCardNo = existdata[0].AMST_FatherPANNo;
                        obj.ASTUREQ_MotherAliveFlag = existdata[0].AMST_MotherAliveFlag;
                        obj.ASTUREQ_MotherName = existdata[0].AMST_MotherName;
                        obj.ASTUREQ_MotherAadharNo = existdata[0].AMST_MotherAadharNo;
                        obj.ASTUREQ_MotherSurname = existdata[0].AMST_MotherSurname;
                        obj.ASTUREQ_MotherEducation = existdata[0].AMST_MotherEducation;
                        obj.ASTUREQ_MotherOccupation = existdata[0].AMST_MotherOccupation;
                        obj.ASTUREQ_MotherOfficeAdd = existdata[0].AMST_MotherOfficeAdd;
                        obj.ASTUREQ_MotherDesignation = existdata[0].AMST_MotherDesignation;
                        obj.ASTUREQ_MotherMonIncome = existdata[0].AMST_MotherMonIncome;
                        obj.ASTUREQ_MotherAnnIncome = existdata[0].AMST_MotherAnnIncome;
                        obj.ASTUREQ_MotherNationality = existdata[0].AMST_MotherNationality;


                        obj.ASTUREQ_MotherReligion = existdata[0].AMST_MotherReligion;

                        obj.ASTUREQ_MotherCaste = existdata[0].AMST_MotherCaste;



                        obj.ASTUREQ_MotherSubCaste = existdata[0].AMST_MotherSubCaste;
                        obj.ASTUREQ_MotherBankAccNo = existdata[0].AMST_MotherBankAccNo;
                        obj.ASTUREQ_MotherBankIFSCCode = existdata[0].AMST_MotherBankIFSC_Code;
                        obj.ASTUREQ_MotherCasteCertiNo = existdata[0].AMST_MotherCasteCertiNo;
                        obj.ASTUREQ_MotherPANCardNo = existdata[0].AMST_MotherPANNo;
                        obj.ASTUREQ_TotalIncome = existdata[0].AMST_TotalIncome;
                        obj.ASTUREQ_MotherSign = existdata[0].AMST_Mother_Signature;
                        obj.ASTUREQ_MotherPhoto = existdata[0].ANST_MotherPhoto;
                        obj.ASTUREQ_MotherFingerprint = existdata[0].AMST_Mother_FingerPrint;
                        obj.ASTUREQ_BirthPlace = existdata[0].AMST_BirthPlace;
                        obj.ASTUREQ_Nationality = existdata[0].AMST_Nationality;
                        obj.ASTUREQ_BPLCardFlag = existdata[0].AMST_BPLCardFlag;
                        obj.ASTUREQ_BPLCardNo = existdata[0].AMST_BPLCardNo;
                        obj.ASTUREQ_HostelReqdFlag = existdata[0].AMST_HostelReqdFlag;
                        obj.ASTUREQ_TransportReqdFlag = existdata[0].AMST_TransportReqdFlag;
                        obj.ASTUREQ_GymReqdFlag = existdata[0].AMST_GymReqdFlag;
                        obj.ASTUREQ_ECSFlag = existdata[0].AMST_ECSFlag;
                        obj.ASTUREQ_PaymentFlag = existdata[0].AMST_PaymentFlag;
                        obj.ASTUREQ_AmountPaid = existdata[0].AMST_AmountPaid;
                        obj.ASTUREQ_PaymentType = existdata[0].AMST_PaymentType;
                        obj.ASTUREQ_PaymentDate = existdata[0].AMST_PaymentDate;
                        obj.ASTUREQ_ReceiptNo = existdata[0].AMST_ReceiptNo;
                        //obj.ASTUREQ_EMSINo = existdata[0].AMST_esi;

                        obj.ASTUREQ_FinalpaymentFlag = existdata[0].AMST_FinalpaymentFlag;
                        obj.ASTUREQ_StudentPhoto = existdata[0].AMST_Photoname;
                        //  obj.ASTUREQ_StudentSign = existdata[0].AMST_Studen;
                        // obj.ASTUREQ_StudentFingerprint = existdata[0].AMST_Student;
                        obj.ASTUREQ_NoofSiblingsSchool = existdata[0].AMST_NoOfSiblingsSchool;
                        obj.ASTUREQ_NoofSiblings = existdata[0].AMST_NoOfSiblings;
                        obj.ASTUREQ_NoOfBrothers = existdata[0].AMST_Noofbrothers;
                        obj.ASTUREQ_NoOfSisters = existdata[0].AMST_Noofsisters;
                        obj.ASTUREQ_NoOfElderBrothers = existdata[0].AMST_NoOfElderBrothers;
                        obj.ASTUREQ_NoOfYoungerBrothers = existdata[0].AMST_NoOfYoungerBrothers;
                        obj.ASTUREQ_NoOfElderSisters = existdata[0].AMST_NoOfElderSisters;
                        obj.ASTUREQ_NoOfYoungerSisters = existdata[0].AMST_NoOfYoungerSisters;
                        obj.ASTUREQ_NoOfDependencies = existdata[0].AMST_NoOfDependencies;
                        obj.ASTUREQ_TPINNO = existdata[0].AMST_Tpin;
                        //  obj.ASTUREQ_ConcessionCategory = existdata[0].AMST_Conc;
                        obj.ASTUREQ_MOInstruction = existdata[0].AMST_MOInstruction;
                        obj.ASTUREQ_GPSTrackingId = existdata[0].AMST_GPSTrackingId;
                        obj.ASTUREQ_AppDownloadedDeviceId = existdata[0].AMST_AppDownloadedDeviceId;
                        obj.ASTUREQ_SecretCode = existdata[0].AMST_SecretCode;
                        obj.ASTUREQ_BiometricId = existdata[0].AMST_BiometricId;
                        obj.ASTUREQ_RFCardNo = existdata[0].AMST_RFCardNo;
                        obj.ASTUREQ_FatherChurchAffiliation = existdata[0].AMST_FatherChurchAffiliation;
                        obj.ASTUREQ_MotherChurchAffiliation = existdata[0].AMST_MotherChurchAffiliation;
                        obj.ASTUREQ_FatherSelfEmployedFlg = existdata[0].AMST_FatherSelfEmployedFlg;
                        obj.ASTUREQ_MotherSelfEmployedFlg = existdata[0].AMST_MotherSelfEmployedFlg;
                        obj.ASTUREQ_ChurchBaptisedDate = existdata[0].AMST_ChurchBaptisedDate;

                    }

                    _studentDashboardContext.Add(obj);
                    int rx = _studentDashboardContext.SaveChanges();
                    if (rx > 0)
                    {
                        data.returnval = true;
                    }
                    else
                    {
                        data.returnval = true;
                    }


                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return data;
        }
        public StudentDashboardDTO conformdata(StudentDashboardDTO data)

        {
            try
            {

                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);


                Adm_Student_Update_RequestDMO obj = new Adm_Student_Update_RequestDMO();



                if (data.AMSTG_Id > 0)
                {

                    var guardlist = _studentDashboardContext.StudentGuardianDMO.Single(r => r.AMSTG_Id == data.AMSTG_Id);


                    obj.AMSTG_Id = data.AMSTG_Id;
                    if (guardlist.AMSTG_GuardianPhoneNo != null && guardlist.AMSTG_GuardianPhoneNo != "")
                    {
                        obj.ASTUREQ_GuardianMobileNo = guardlist.AMSTG_GuardianPhoneNo;
                    }

                    obj.ASTUREQ_GuardianEmailId = guardlist.AMSTG_emailid;
                }

                var existdata = _studentDashboardContext.Adm_M_Student.Where(a => a.AMST_Id == data.AMST_Id && a.MI_Id == data.MI_Id && a.AMST_ActiveFlag == 1).ToList();
                if (existdata.Count > 0)
                {


                    obj.MI_Id = data.MI_Id;
                    obj.ASMAY_Id = data.ASMAY_Id;
                    obj.AMST_Id = data.AMST_Id;
                    obj.ASTUREQ_PerStreet = existdata[0].AMST_PerStreet;
                    obj.ASTUREQ_PerArea = existdata[0].AMST_PerArea;
                    obj.ASTUREQ_PerCity = existdata[0].AMST_PerCity;
                    obj.ASTUREQ_PerState = existdata[0].AMST_PerState;
                    obj.IVRMMC_Id = existdata[0].AMST_PerCountry;
                    obj.ASTUREQ_PerPincode = existdata[0].AMST_PerPincode;
                    obj.ASTUREQ_ConStreet = existdata[0].AMST_ConStreet;
                    obj.ASTUREQ_ConArea = existdata[0].AMST_ConArea;
                    obj.ASTUREQ_ConCity = existdata[0].AMST_ConCity;
                    obj.ASTUREQ_ConState = existdata[0].AMST_ConState;
                    obj.ASTUREQ_ConCountryId = existdata[0].AMST_ConCountry;
                    obj.ASTUREQ_ConPincode = existdata[0].AMST_ConPincode;
                    obj.ASTUREQ_UpdatedDate = indianTime;
                    obj.ASTUREQ_Date = indianTime;
                    obj.ASTUREQ_CreatedDate = indianTime;
                    obj.ASTUREQ_UpdatedBy = data.User_Id;
                    obj.ASTUREQ_ActiveFlag = true;
                    obj.ASTUREQ_ConformBy = data.AMST_Id;
                    obj.ASTUREQ_CreatedBy = data.AMST_Id;
                    obj.ASTUREQ_ApprovedBy = data.User_Id;
                    obj.ASTUREQ_ConformFlg = true;
                    obj.ASTUREQ_ApprovedFlg = false;
                    obj.ASTUREQ_MobileNo = existdata[0].AMST_MobileNo;
                    obj.ASTUREQ_EmailId = existdata[0].AMST_emailId;
                    obj.ASTUREQ_FatherMobleNo = existdata[0].AMST_FatherMobleNo;
                    obj.ASTUREQ_FatheremailId = existdata[0].AMST_FatheremailId;
                    obj.ASTUREQ_MotheremailId = existdata[0].AMST_MotherEmailId;
                    obj.ASTUREQ_MotherMobleNo = existdata[0].AMST_MotherMobileNo;
                    obj.ASTUREQ_ReqStatus = "CONFIRMED";
                    obj.ASTUREQ_ChangeConfirmFlg = "CONFIRM";
                    obj.ASTUREQ_ApplStatus = existdata[0].AMST_ApplStatus;
                    obj.ASTUREQ_FirstName = existdata[0].AMST_FirstName;
                    obj.ASTUREQ_MiddleName = existdata[0].AMST_MiddleName;
                    obj.ASTUREQ_LastName = existdata[0].AMST_LastName;
                    obj.ASTUREQ_RegistrationNo = existdata[0].AMST_RegistrationNo;
                    obj.ASTUREQ_AdmNo = existdata[0].AMST_AdmNo;
                    obj.AMC_Id = existdata[0].AMC_Id;
                    obj.ASTUREQ_Sex = existdata[0].AMST_Sex;
                    obj.ASTUREQ_DOB = existdata[0].AMST_DOB;
                    obj.ASTUREQ_DOBinwords = existdata[0].AMST_DOB_Words;
                    obj.ASTUREQ_Age = existdata[0].PASR_Age;
                    obj.ASMCL_Id = existdata[0].ASMCL_Id;
                    obj.ASTUREQ_BloodGroup = existdata[0].AMST_BloodGroup;
                    obj.ASTUREQ_MotherTongue = existdata[0].AMST_MotherTongue;
                    obj.ASTUREQ_HomeLaguage = existdata[0].AMST_LanguageSpoken;
                    obj.ASTUREQ_BirthCertNo = existdata[0].AMST_BirthCertNO;
                    obj.IVRMMR_Id = existdata[0].IVRMMR_Id;
                    obj.IMCC_Id = existdata[0].IMCC_Id;
                    // obj.IMC_Id = existdata[0].IC_Id;
                    obj.ASTUREQ_StudentSubCaste = existdata[0].AMST_SubCasteIMC_Id;
                    obj.ASTUREQ_PerAdd3 = existdata[0].AMST_PerAdd3;
                    // obj.ASTUREQ_ConAdd3 = existdata[0].AMST_PerAdd3;
                    obj.ASTUREQ_Village = existdata[0].AMST_Village;
                    obj.ASTUREQ_Taluk = existdata[0].AMST_Taluk;
                    obj.ASTUREQ_District = existdata[0].AMST_Distirct;
                    obj.ASTUREQ_AadharNo = existdata[0].AMST_AadharNo;
                    obj.ASTUREQ_StuBankAccNo = existdata[0].AMST_StuBankAccNo;
                    obj.ASTUREQ_StudentPANCard = existdata[0].AMST_StudentPANNo;
                    obj.ASTUREQ_StuBankIFSCCode = existdata[0].AMST_StuBankIFSC_Code;
                    obj.ASTUREQ_StuCasteCertiNo = existdata[0].AMST_StuCasteCertiNo;
                    obj.ASTUREQ_FatherAliveFlag = existdata[0].AMST_FatherAliveFlag;
                    obj.ASTUREQ_FatherMaritalStatusFlg = existdata[0].AMST_FatherMaritalStatus;
                    obj.ASTUREQ_FatherName = existdata[0].AMST_FatherName;
                    obj.ASTUREQ_FatherAadharNo = existdata[0].AMST_FatherAadharNo;
                    obj.ASTUREQ_FatherSurname = existdata[0].AMST_FatherSurname;
                    obj.ASTUREQ_FatherEducation = existdata[0].AMST_FatherEducation;
                    obj.ASTUREQ_FatherOccupation = existdata[0].AMST_FatherOccupation;
                    obj.ASTUREQ_FatherOfficeAdd = existdata[0].AMST_FatherOfficeAdd;
                    obj.ASTUREQ_FatherDesignation = existdata[0].AMST_FatherDesignation;
                    obj.ASTUREQ_FatherMonIncome = existdata[0].AMST_FatherMonIncome;
                    obj.ASTUREQ_FatherAnnIncome = existdata[0].AMST_FatherAnnIncome;
                    obj.ASTUREQ_FatherNationality = existdata[0].AMST_FatherNationality;
                    obj.ASTUREQ_FatherReligion = existdata[0].AMST_FatherReligion;
                    obj.ASTUREQ_FatherCaste = existdata[0].AMST_FatherCaste;
                    obj.ASTUREQ_FatherSubCaste = existdata[0].AMST_FatherSubCaste;
                    obj.ASTUREQ_FatherBankAccNo = existdata[0].AMST_FatherBankAccNo;
                    obj.ASTUREQ_FatherBankIFSCCode = existdata[0].AMST_FatherBankIFSC_Code;
                    obj.ASTUREQ_FatherCasteCertiNo = existdata[0].AMST_FatherCasteCertiNo;
                    obj.ASTUREQ_FatherPhoto = existdata[0].ANST_FatherPhoto;
                    obj.ASTUREQ_FatherSign = existdata[0].AMST_Father_Signature;
                    obj.ASTUREQ_FatherFingerprint = existdata[0].AMST_Father_FingerPrint;
                    obj.ASTUREQ_FatherPANCardNo = existdata[0].AMST_FatherPANNo;
                    obj.ASTUREQ_MotherAliveFlag = existdata[0].AMST_MotherAliveFlag;
                    obj.ASTUREQ_MotherName = existdata[0].AMST_MotherName;
                    obj.ASTUREQ_MotherAadharNo = existdata[0].AMST_MotherAadharNo;
                    obj.ASTUREQ_MotherSurname = existdata[0].AMST_MotherSurname;
                    obj.ASTUREQ_MotherEducation = existdata[0].AMST_MotherEducation;
                    obj.ASTUREQ_MotherOccupation = existdata[0].AMST_MotherOccupation;
                    obj.ASTUREQ_MotherOfficeAdd = existdata[0].AMST_MotherOfficeAdd;
                    obj.ASTUREQ_MotherDesignation = existdata[0].AMST_MotherDesignation;
                    obj.ASTUREQ_MotherMonIncome = existdata[0].AMST_MotherMonIncome;
                    obj.ASTUREQ_MotherAnnIncome = existdata[0].AMST_MotherAnnIncome;
                    obj.ASTUREQ_MotherNationality = existdata[0].AMST_MotherNationality;
                    obj.ASTUREQ_MotherReligion = existdata[0].AMST_MotherReligion;
                    obj.ASTUREQ_MotherCaste = existdata[0].AMST_MotherCaste;
                    obj.ASTUREQ_MotherSubCaste = existdata[0].AMST_MotherSubCaste;
                    obj.ASTUREQ_MotherBankAccNo = existdata[0].AMST_MotherBankAccNo;
                    obj.ASTUREQ_MotherBankIFSCCode = existdata[0].AMST_MotherBankIFSC_Code;
                    obj.ASTUREQ_MotherCasteCertiNo = existdata[0].AMST_MotherCasteCertiNo;
                    obj.ASTUREQ_MotherPANCardNo = existdata[0].AMST_MotherPANNo;
                    obj.ASTUREQ_TotalIncome = existdata[0].AMST_TotalIncome;
                    obj.ASTUREQ_MotherSign = existdata[0].AMST_Mother_Signature;
                    obj.ASTUREQ_MotherPhoto = existdata[0].ANST_MotherPhoto;
                    obj.ASTUREQ_MotherFingerprint = existdata[0].AMST_Mother_FingerPrint;
                    obj.ASTUREQ_BirthPlace = existdata[0].AMST_BirthPlace;
                    obj.ASTUREQ_Nationality = existdata[0].AMST_Nationality;
                    obj.ASTUREQ_BPLCardFlag = existdata[0].AMST_BPLCardFlag;
                    obj.ASTUREQ_BPLCardNo = existdata[0].AMST_BPLCardNo;
                    obj.ASTUREQ_HostelReqdFlag = existdata[0].AMST_HostelReqdFlag;
                    obj.ASTUREQ_TransportReqdFlag = existdata[0].AMST_TransportReqdFlag;
                    obj.ASTUREQ_GymReqdFlag = existdata[0].AMST_GymReqdFlag;
                    obj.ASTUREQ_ECSFlag = existdata[0].AMST_ECSFlag;
                    obj.ASTUREQ_PaymentFlag = existdata[0].AMST_PaymentFlag;
                    obj.ASTUREQ_AmountPaid = existdata[0].AMST_AmountPaid;
                    obj.ASTUREQ_PaymentType = existdata[0].AMST_PaymentType;
                    obj.ASTUREQ_PaymentDate = existdata[0].AMST_PaymentDate;
                    obj.ASTUREQ_ReceiptNo = existdata[0].AMST_ReceiptNo;
                    obj.ASTUREQ_FinalpaymentFlag = existdata[0].AMST_FinalpaymentFlag;
                    obj.ASTUREQ_StudentPhoto = existdata[0].AMST_Photoname;
                    obj.ASTUREQ_NoofSiblingsSchool = existdata[0].AMST_NoOfSiblingsSchool;
                    obj.ASTUREQ_NoofSiblings = existdata[0].AMST_NoOfSiblings;
                    obj.ASTUREQ_NoOfBrothers = existdata[0].AMST_Noofbrothers;
                    obj.ASTUREQ_NoOfSisters = existdata[0].AMST_Noofsisters;
                    obj.ASTUREQ_NoOfElderBrothers = existdata[0].AMST_NoOfElderBrothers;
                    obj.ASTUREQ_NoOfYoungerBrothers = existdata[0].AMST_NoOfYoungerBrothers;
                    obj.ASTUREQ_NoOfElderSisters = existdata[0].AMST_NoOfElderSisters;
                    obj.ASTUREQ_NoOfYoungerSisters = existdata[0].AMST_NoOfYoungerSisters;
                    obj.ASTUREQ_NoOfDependencies = existdata[0].AMST_NoOfDependencies;
                    obj.ASTUREQ_TPINNO = existdata[0].AMST_Tpin;
                    obj.ASTUREQ_MOInstruction = existdata[0].AMST_MOInstruction;
                    obj.ASTUREQ_GPSTrackingId = existdata[0].AMST_GPSTrackingId;
                    obj.ASTUREQ_AppDownloadedDeviceId = existdata[0].AMST_AppDownloadedDeviceId;
                    obj.ASTUREQ_SecretCode = existdata[0].AMST_SecretCode;
                    obj.ASTUREQ_BiometricId = existdata[0].AMST_BiometricId;
                    obj.ASTUREQ_RFCardNo = existdata[0].AMST_RFCardNo;
                    obj.ASTUREQ_FatherChurchAffiliation = existdata[0].AMST_FatherChurchAffiliation;
                    obj.ASTUREQ_MotherChurchAffiliation = existdata[0].AMST_MotherChurchAffiliation;
                    obj.ASTUREQ_FatherSelfEmployedFlg = existdata[0].AMST_FatherSelfEmployedFlg;
                    obj.ASTUREQ_MotherSelfEmployedFlg = existdata[0].AMST_MotherSelfEmployedFlg;
                    obj.ASTUREQ_ChurchBaptisedDate = existdata[0].AMST_ChurchBaptisedDate;
                    _studentDashboardContext.Add(obj);

                }
                int ss = _studentDashboardContext.SaveChanges();
                if (ss > 0)
                {
                    var res = _studentDashboardContext.Adm_M_Student.Single(w => w.AMST_Id == data.AMST_Id);
                    res.UpdatedDate = indianTime;
                    _studentDashboardContext.Update(res);
                    _studentDashboardContext.SaveChanges();
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
            return data;
        }
        public StudentDashboardDTO viewnotice(StudentDashboardDTO dto)
        {
            try
            {
                dto.attachementlist = (from a in _studentDashboardContext.IVRM_NoticeBoardDMO
                                       from b in _studentDashboardContext.IVRM_NoticeBoard_FilesDMO_con
                                       where a.INTB_Id == dto.INTB_Id && a.INTB_Id == b.INTB_Id
                                       select new IVRM_NoticeBoardDTO
                                       {
                                           INTBFL_FileName = b.INTBFL_FileName,
                                           INTBFL_FilePath = b.INTBFL_FilePath,
                                           INTB_Attachment = a.INTB_Attachment,
                                           INTB_Id = a.INTB_Id
                                       }).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return dto;
        }
        public StudentDashboardDTO onclick_notice(StudentDashboardDTO dto)
        {
            try
            {

                var clssec1 = (from a in _studentDashboardContext.Adm_M_Student
                               from b in _studentDashboardContext.School_Adm_Y_StudentDMO
                               from c in _studentDashboardContext.School_M_Class
                               from s in _studentDashboardContext.School_M_Section
                               where (b.ASMCL_Id == c.ASMCL_Id && b.ASMS_Id == s.ASMS_Id && a.MI_Id == c.MI_Id && a.MI_Id == dto.MI_Id
                               && b.ASMAY_Id == dto.ASMAY_Id && a.AMST_Id == dto.AMST_Id && b.AMST_Id == dto.AMST_Id)
                               select new StudentDashboardDTO
                               {
                                   ASMCL_Id = c.ASMCL_Id,
                                   ASMCL_ClassName = c.ASMCL_ClassName,
                                   ASMS_Id = s.ASMS_Id,
                                   ASMC_SectionName = s.ASMC_SectionName
                               }).Distinct().ToList();

                //var clssec1 = _studentDashboardContext.School_Adm_Y_StudentDMO.Where(a => a.ASMAY_Id == dto.ASMAY_Id && a.AMST_Id == dto.AMST_Id
                //&& a.AMAY_ActiveFlag == 1).ToList();


                if (clssec1.Count == 0)
                {
                    dto.messag = "";
                }
                else
                {
                    long Class_Id = clssec1.FirstOrDefault().ASMCL_Id;
                    long Section_Id = clssec1.FirstOrDefault().ASMS_Id;

                    var date = DateTime.Now;
                    using (var cmd = _studentDashboardContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "Portal_NoticeBoardYearWise";
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
                            Value = dto.AMST_Id
                        });

                        cmd.Parameters.Add(new SqlParameter("@Flag",
                    SqlDbType.VarChar)
                        {
                            Value = dto.flag
                        });

                        cmd.Parameters.Add(new SqlParameter("@Type",
                    SqlDbType.VarChar)
                        {
                            Value = "Student"
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



            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }
        public StudentDashboardDTO onclick_TT(StudentDashboardDTO dto)
        {
            try
            {

                //var clssec1 = (from a in _studentDashboardContext.Adm_M_Student
                //               from b in _studentDashboardContext.School_Adm_Y_StudentDMO
                //               from c in _studentDashboardContext.School_M_Class
                //               from s in _studentDashboardContext.School_M_Section
                //               where (b.ASMCL_Id == c.ASMCL_Id && b.ASMS_Id == s.ASMS_Id && a.MI_Id == c.MI_Id && a.MI_Id == dto.MI_Id
                //               && b.ASMAY_Id == dto.ASMAY_Id && a.AMST_Id == dto.AMST_Id && b.AMST_Id == dto.AMST_Id)
                //               select new StudentDashboardDTO
                //               {
                //                   ASMCL_Id = c.ASMCL_Id,
                //                   ASMCL_ClassName = c.ASMCL_ClassName,
                //                   ASMS_Id = s.ASMS_Id,
                //                   ASMC_SectionName = s.ASMC_SectionName
                //               }).Distinct().ToList();

                var clssec1 = _studentDashboardContext.School_Adm_Y_StudentDMO.Where(a => a.ASMAY_Id == dto.ASMAY_Id && a.AMST_Id == dto.AMST_Id
               && a.AMAY_ActiveFlag == 1).ToList();


                if (clssec1.Count == 0)
                {
                    dto.messag = "";
                }
                else
                {
                    long Class_Id = clssec1.FirstOrDefault().ASMCL_Id;
                    long Section_Id = clssec1.FirstOrDefault().ASMS_Id;

                    var date = DateTime.Now;
                    using (var cmd = _studentDashboardContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "Portal_NoticeBoardYearWise";
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
                        });
                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                    SqlDbType.VarChar)
                        {
                            Value = dto.ASMAY_Id
                        });

                        cmd.Parameters.Add(new SqlParameter("@AMST_Id",
                   SqlDbType.VarChar)
                        {
                            Value = dto.AMST_Id
                        });

                        cmd.Parameters.Add(new SqlParameter("@Flag",
                    SqlDbType.VarChar)
                        {
                            Value = dto.flag
                        });

                        cmd.Parameters.Add(new SqlParameter("@Type",
                    SqlDbType.VarChar)
                        {
                            Value = "Student"
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

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }
        public StudentDashboardDTO onclick_syllabus(StudentDashboardDTO dto)
        {
            try
            {


                //var clssec1 = (from a in _studentDashboardContext.Adm_M_Student
                //               from b in _studentDashboardContext.School_Adm_Y_StudentDMO
                //               from c in _studentDashboardContext.School_M_Class
                //               from s in _studentDashboardContext.School_M_Section
                //               where (b.ASMCL_Id == c.ASMCL_Id && b.ASMS_Id == s.ASMS_Id && a.MI_Id == c.MI_Id && a.MI_Id == dto.MI_Id
                //               && b.ASMAY_Id == dto.ASMAY_Id && a.AMST_Id == dto.AMST_Id && b.AMST_Id == dto.AMST_Id)
                //               select new StudentDashboardDTO
                //               {
                //                   ASMCL_Id = c.ASMCL_Id,
                //                   ASMCL_ClassName = c.ASMCL_ClassName,
                //                   ASMS_Id = s.ASMS_Id,
                //                   ASMC_SectionName = s.ASMC_SectionName
                //               }).Distinct().ToList();

                var clssec1 = _studentDashboardContext.School_Adm_Y_StudentDMO.Where(a => a.ASMAY_Id == dto.ASMAY_Id && a.AMST_Id == dto.AMST_Id
               && a.AMAY_ActiveFlag == 1).ToList();


                if (clssec1.Count == 0)
                {
                    dto.messag = "";
                }
                else
                {
                    long Class_Id = clssec1.FirstOrDefault().ASMCL_Id;
                    long Section_Id = clssec1.FirstOrDefault().ASMS_Id;

                    var date = DateTime.Now;
                    using (var cmd = _studentDashboardContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "Portal_NoticeBoardYearWise";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar)
                        {
                            Value = dto.MI_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar)
                        {
                            Value = Class_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar)
                        {
                            Value = dto.ASMAY_Id
                        });

                        cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar)
                        {
                            Value = dto.AMST_Id
                        });

                        cmd.Parameters.Add(new SqlParameter("@Flag", SqlDbType.VarChar)
                        {
                            Value = dto.flag
                        });

                        cmd.Parameters.Add(new SqlParameter("@Type", SqlDbType.VarChar)
                        {
                            Value = "Student"
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


            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }
        public StudentDashboardDTO onclick_LIB(StudentDashboardDTO dto)
        {
            try
            {
                if(dto.AMST_IdTwo > 0)
                {
                    dto.AMST_Id = dto.AMST_IdTwo;
                }
               
                using (var cmd = _studentDashboardContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Portal_LibraryDetails";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                SqlDbType.BigInt)
                    {
                        Value = dto.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                     SqlDbType.BigInt)
                    {
                        Value = dto.ASMAY_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id",
              SqlDbType.BigInt)
                    {
                        Value = dto.AMST_Id
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
                        dto.librarydetails = retObject.ToArray();
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
        public StudentDashboardDTO onclick_Homework(StudentDashboardDTO dto)
        {
            try
            {

                var clssec1 = (from a in _studentDashboardContext.Adm_M_Student
                               from b in _studentDashboardContext.School_Adm_Y_StudentDMO
                               from c in _studentDashboardContext.School_M_Class
                               from s in _studentDashboardContext.School_M_Section
                               where (b.ASMCL_Id == c.ASMCL_Id && b.ASMS_Id == s.ASMS_Id && a.MI_Id == c.MI_Id && a.MI_Id == dto.MI_Id
                               && b.ASMAY_Id == dto.ASMAY_Id && a.AMST_Id == dto.AMST_Id && b.AMST_Id == dto.AMST_Id)
                               select new StudentDashboardDTO
                               {
                                   ASMCL_Id = c.ASMCL_Id,
                                   ASMCL_ClassName = c.ASMCL_ClassName,
                                   ASMS_Id = s.ASMS_Id,
                                   ASMC_SectionName = s.ASMC_SectionName
                               }).Distinct().ToList();


                if (clssec1.Count == 0)
                {
                    dto.messag = "";
                }
                else
                {
                    long Class_Id = clssec1.FirstOrDefault().ASMCL_Id;
                    long Section_Id = clssec1.FirstOrDefault().ASMS_Id;

                    using (var cmd = _studentDashboardContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "Portal_HomeWorkClasswork_test";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = dto.MI_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.BigInt) { Value = dto.ASMAY_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.BigInt) { Value = Class_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.BigInt) { Value = Section_Id });
                        cmd.Parameters.Add(new SqlParameter("@type", SqlDbType.VarChar) { Value = "Homework" });
                        cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.BigInt) { Value = dto.AMST_Id });


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
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }
        public StudentDashboardDTO onclick_Classwork(StudentDashboardDTO dto)
        {
            try
            {


                //var clssec1 = (from a in _studentDashboardContext.Adm_M_Student
                //               from b in _studentDashboardContext.School_Adm_Y_StudentDMO
                //               from c in _studentDashboardContext.School_M_Class
                //               from s in _studentDashboardContext.School_M_Section
                //               where (b.ASMCL_Id == c.ASMCL_Id && b.ASMS_Id == s.ASMS_Id && a.MI_Id == c.MI_Id && a.MI_Id == dto.MI_Id
                //               && b.ASMAY_Id == dto.ASMAY_Id && a.AMST_Id == dto.AMST_Id && b.AMST_Id == dto.AMST_Id)
                //               select new StudentDashboardDTO
                //               {
                //                   ASMCL_Id = c.ASMCL_Id,
                //                   ASMCL_ClassName = c.ASMCL_ClassName,
                //                   ASMS_Id = s.ASMS_Id,
                //                   ASMC_SectionName = s.ASMC_SectionName
                //               }).Distinct().ToList();

                var clssec1 = _studentDashboardContext.School_Adm_Y_StudentDMO.Where(a => a.ASMAY_Id == dto.ASMAY_Id && a.AMST_Id == dto.AMST_Id
               && a.AMAY_ActiveFlag == 1).ToList();


                if (clssec1.Count == 0)
                {
                    dto.messag = "";
                }
                else
                {
                    long Class_Id = clssec1.FirstOrDefault().ASMCL_Id;
                    long Section_Id = clssec1.FirstOrDefault().ASMS_Id;

                    using (var cmd = _studentDashboardContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "PORTAL_StudentDashboard";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                    SqlDbType.BigInt)
                        {
                            Value = dto.MI_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                         SqlDbType.BigInt)
                        {
                            Value = dto.ASMAY_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@AMST_Id",
                  SqlDbType.BigInt)
                        {
                            Value = dto.AMST_Id
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
                            dto.studetailslist = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                    using (var cmd = _studentDashboardContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "Portal_HomeWorkClasswork";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@MI_Id",SqlDbType.BigInt){Value = dto.MI_Id});
                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",SqlDbType.BigInt){Value = dto.ASMAY_Id});
                        cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",SqlDbType.BigInt){Value = Class_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMS_Id",SqlDbType.BigInt){Value = Section_Id});
                        cmd.Parameters.Add(new SqlParameter("@type",SqlDbType.VarChar){Value = "Classwork"});
                        cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.BigInt) { Value = dto.AMST_Id });
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

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }
        public StudentDashboardDTO onclick_Homework_load(StudentDashboardDTO dto)
        {
            try
            {
                dto.yearlist = _studentDashboardContext.AcademicYearDMO.Where(a => a.MI_Id == dto.MI_Id && a.Is_Active == true).OrderByDescending(a => a.ASMAY_Order).ToArray();

                var clssec1 = _studentDashboardContext.School_Adm_Y_StudentDMO.Where(a => a.ASMAY_Id == dto.ASMAY_Id && a.AMST_Id == dto.AMST_Id
                  && a.AMAY_ActiveFlag == 1).ToList();

                //var clssec1 = (from a in _studentDashboardContext.Adm_M_Student
                //               from b in _studentDashboardContext.School_Adm_Y_StudentDMO
                //               from c in _studentDashboardContext.School_M_Class
                //               from s in _studentDashboardContext.School_M_Section
                //               where (b.ASMCL_Id == c.ASMCL_Id && b.ASMS_Id == s.ASMS_Id && a.MI_Id == c.MI_Id && a.MI_Id == dto.MI_Id
                //               && b.ASMAY_Id == dto.ASMAY_Id && a.AMST_Id == dto.AMST_Id && b.AMST_Id == dto.AMST_Id)
                //               select new StudentDashboardDTO
                //               {
                //                   ASMCL_Id = c.ASMCL_Id,
                //                   ASMCL_ClassName = c.ASMCL_ClassName,
                //                   ASMS_Id = s.ASMS_Id,
                //                   ASMC_SectionName = s.ASMC_SectionName
                //               }).Distinct().ToList();


                if (clssec1.Count == 0)
                {
                    dto.messag = "";
                }
                else
                {
                    long Class_Id = clssec1.FirstOrDefault().ASMCL_Id;
                    long Section_Id = clssec1.FirstOrDefault().ASMS_Id;

                    using (var cmd = _studentDashboardContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "Portal_HomeWorkClasswork_Student_Modify";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = dto.MI_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.BigInt) { Value = dto.ASMAY_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.BigInt) { Value = Class_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.BigInt) { Value = Section_Id });
                        cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.BigInt) { Value = dto.AMST_Id });
                        cmd.Parameters.Add(new SqlParameter("@type", SqlDbType.VarChar) { Value = "Homework" });

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
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }
        public StudentDashboardDTO onclick_Classwork_load(StudentDashboardDTO dto)
        {
            try
            {

                dto.yearlist = _studentDashboardContext.AcademicYearDMO.Where(a => a.MI_Id == dto.MI_Id && a.Is_Active == true).OrderByDescending(a => a.ASMAY_Order).ToArray();

                //var clssec1 = (from a in _studentDashboardContext.Adm_M_Student
                //               from b in _studentDashboardContext.School_Adm_Y_StudentDMO
                //               from c in _studentDashboardContext.School_M_Class
                //               from s in _studentDashboardContext.School_M_Section
                //               where (b.ASMCL_Id == c.ASMCL_Id && b.ASMS_Id == s.ASMS_Id && a.MI_Id == c.MI_Id && a.MI_Id == dto.MI_Id
                //               && b.ASMAY_Id == dto.ASMAY_Id && a.AMST_Id == dto.AMST_Id && b.AMST_Id == dto.AMST_Id)
                //               select new StudentDashboardDTO
                //               {
                //                   ASMCL_Id = c.ASMCL_Id,
                //                   ASMCL_ClassName = c.ASMCL_ClassName,
                //                   ASMS_Id = s.ASMS_Id,
                //                   ASMC_SectionName = s.ASMC_SectionName
                //               }).Distinct().ToList();

                var clssec1 = _studentDashboardContext.School_Adm_Y_StudentDMO.Where(a => a.ASMAY_Id == dto.ASMAY_Id && a.AMST_Id == dto.AMST_Id
               && a.AMAY_ActiveFlag == 1).ToList();


                if (clssec1.Count == 0)
                {
                    dto.messag = "";
                }
                else
                {
                    long Class_Id = clssec1.FirstOrDefault().ASMCL_Id;
                    long Section_Id = clssec1.FirstOrDefault().ASMS_Id;

                    using (var cmd = _studentDashboardContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "PORTAL_StudentDashboard";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = dto.MI_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.BigInt) { Value = dto.ASMAY_Id });
                        cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.BigInt) { Value = dto.AMST_Id });


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
                            dto.studetailslist = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }

                    using (var cmd = _studentDashboardContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "Portal_HomeWorkClasswork_Student_Modify";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = dto.MI_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.BigInt) { Value = dto.ASMAY_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.BigInt) { Value = Class_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.BigInt) { Value = Section_Id });
                        cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.BigInt) { Value = dto.AMST_Id });
                        cmd.Parameters.Add(new SqlParameter("@type", SqlDbType.VarChar) { Value = "Classwork" });

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

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }
        public StudentDashboardDTO onclick_Sports(StudentDashboardDTO dto)
        {
            try
            {

                using (var cmd = _studentDashboardContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Portal_SportsDetails";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    {
                        Value = dto.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.BigInt)
                    {
                        Value = dto.ASMAY_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.BigInt)
                    {
                        Value = dto.AMST_Id
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
                        dto.sportsdetails = retObject.ToArray();
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
        public StudentDashboardDTO onclick_Inventory(StudentDashboardDTO dto)
        {
            try
            {
                using (var cmd = _studentDashboardContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Portal_Student_Inventory";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                SqlDbType.BigInt)
                    {
                        Value = dto.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id",
              SqlDbType.VarChar)
                    {
                        Value = dto.AMST_Id
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
                        dto.inventorydetails = retObject.ToArray();
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
        public StudentDashboardDTO onclick_PDA(StudentDashboardDTO dto)
        {
            try
            {

                using (var cmd = _studentDashboardContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Portal_pdadetails";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                SqlDbType.BigInt)
                    {
                        Value = dto.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@AMST_Id",
              SqlDbType.BigInt)
                    {
                        Value = dto.AMST_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
        SqlDbType.BigInt)
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
                                        dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                                    );
                                }

                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        dto.pdadetails = retObject.ToArray();
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
        public StudentDashboardDTO onclick_Gallery(StudentDashboardDTO dto)
        {
            try
            {

                if (dto.AMST_Id == 0)
                {
                    dto.displyamessage = (from a in _studentDashboardContext.IVRM_NoticeBoardDMO
                                          where (a.MI_Id == dto.MI_Id && a.INTB_ActiveFlag == true && a.INTB_ToStudentFlg == true && a.NTB_TTSylabusFlg == "DM")
                                          select new StudentDashboardDTO
                                          {
                                              INTB_Title = a.INTB_Title,
                                              INTB_Description = a.INTB_Description,
                                              NTB_TTSylabusFlg = a.NTB_TTSylabusFlg,
                                              INTB_Id = a.INTB_Id
                                          }).ToArray();
                }
                else
                {
                    using (var cmd = _studentDashboardContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "GET_STUDENT_GALLERY_PORTAL";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                        {
                            Value = dto.MI_Id
                        });

                        cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.BigInt)
                        {
                            Value = dto.AMST_Id
                        });

                        //cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.BigInt)
                        //{
                        //    Value = dto.ASMAY_Id
                        //});
                        
                        cmd.Parameters.Add(new SqlParameter("@IGA_Id", SqlDbType.BigInt)
                        {
                            Value = dto.IGA_Id
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
                            dto.imagegallery = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
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
        public string geterrormessage(StudentDashboardDTO dto)
        {
            List<StudentDashboardDTO> errormessage = new List<StudentDashboardDTO>();
            try
            {
                using (var cmd = _studentDashboardContext.Database.GetDbConnection().CreateCommand())
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
                                errormessage.Add(new StudentDashboardDTO
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
        public StudentDashboardDTO ViewStudentProfile(StudentDashboardDTO data)
        {
            try
            {
                data.yearlist = _studentDashboardContext.AcademicYearDMO.Where(a => a.MI_Id == data.MI_Id && a.Is_Active == true).OrderByDescending(a => a.ASMAY_Order).ToArray();

                data.viewstudentjoineddetails = (from a in _studentDashboardContext.Adm_M_Student
                                                 from b in _studentDashboardContext.AcademicYearDMO
                                                 from c in _studentDashboardContext.School_M_Class
                                                 where (a.ASMAY_Id == b.ASMAY_Id && a.ASMCL_Id == c.ASMCL_Id && a.AMST_Id == data.AMST_Id
                                                 && a.MI_Id == data.MI_Id)
                                                 select new StudentDashboardDTO
                                                 {
                                                     studentname = ((a.AMST_FirstName == null || a.AMST_FirstName == "" ? "" : a.AMST_FirstName) +
                                                     (a.AMST_MiddleName == null || a.AMST_MiddleName == "" ? "" : " " + a.AMST_MiddleName) +
                                                     (a.AMST_LastName == null || a.AMST_LastName == "" ? "" : " " + a.AMST_LastName)).Trim(),
                                                     AMST_AdmNo = a.AMST_AdmNo,
                                                     AMST_RegistrationNo = a.AMST_RegistrationNo,
                                                     ASMAY_Year = b.ASMAY_Year,
                                                     ASMCL_ClassName = c.ASMCL_ClassName,
                                                     AMST_Photoname = a.AMST_Photoname,
                                                     AMST_Sex = a.AMST_Sex,
                                                     AMST_SOL = a.AMST_SOL,
                                                     AMST_Date = a.AMST_Date,
                                                     AMST_DOB = a.AMST_DOB,
                                                 }).Distinct().ToArray();

                data.viewstudentdetails = _studentDashboardContext.Adm_M_Student.Where(a => a.MI_Id == data.MI_Id && a.AMST_Id == data.AMST_Id).ToArray();

                var viewstudentacademicyeardetails= _studentDashboardContext.School_Adm_Y_StudentDMO.Where(a => a.ASMAY_Id == data.ASMAY_Id
                && a.AMST_Id == data.AMST_Id).ToArray();

                data.viewstudentacademicyeardetails = (from a in _studentDashboardContext.School_Adm_Y_StudentDMO
                                                       from b in _studentDashboardContext.AcademicYearDMO
                                                       from c in _studentDashboardContext.School_M_Class
                                                       from d in _studentDashboardContext.School_M_Section
                                                       where (a.ASMAY_Id == b.ASMAY_Id && a.ASMCL_Id == c.ASMCL_Id && a.ASMS_Id == d.ASMS_Id
                                                       && a.AMST_Id == data.AMST_Id)
                                                       select new StudentDashboardDTO
                                                       {
                                                           ASMAY_Year = b.ASMAY_Year,
                                                           ASMCL_ClassName = c.ASMCL_ClassName,
                                                           ASMC_SectionName = d.ASMC_SectionName,
                                                           order = b.ASMAY_Order,
                                                           ASMAY_Id = a.ASMAY_Id,
                                                           AMAY_RollNo = a.AMAY_RollNo,
                                                           Status_Flag = a.ASMAY_Id == data.ASMAY_Id ? "Current Year" : ""
                                                       }).Distinct().OrderByDescending(a => a.order).ToArray();

                data.viewstudentguardiandetails = _studentDashboardContext.StudentGuardianDMO.Where(a => a.AMST_Id == data.AMST_Id).ToArray();


                var asms_id = _studentDashboardContext.School_Adm_Y_StudentDMO.Where(a => a.AMST_Id == data.AMST_Id && a.ASMAY_Id == data.ASMAY_Id).Select(s => s.ASMS_Id).FirstOrDefault();
                var asmcl_id = _studentDashboardContext.School_Adm_Y_StudentDMO.Where(a => a.AMST_Id == data.AMST_Id && a.ASMAY_Id == data.ASMAY_Id).Select(s => s.ASMCL_Id).FirstOrDefault();


                data.viewstudentsubjectdetails = (from a in _studentDashboardContext.StudentMappingDMO
                                                  from b in _studentDashboardContext.IVRM_Master_SubjectsDMO
                                                  where (a.ISMS_Id == b.ISMS_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.AMST_Id == data.AMST_Id && a.ASMCL_Id == asmcl_id && a.ASMS_Id == asms_id)
                                                  select new StudentDashboardDTO
                                                  {
                                                      ISMS_Id = a.ISMS_Id,
                                                      ISMS_SubjectName = b.ISMS_SubjectName,
                                                      subjorder = b.ISMS_OrderFlag,
                                                      ESTSU_ElecetiveFlag = a.ESTSU_ElecetiveFlag
                                                  }).Distinct().OrderBy(a => a.subjorder).ToArray();

                //data.viewstudentsubjectdetails = (from a in _studentDashboardContext.StudentMappingDMO
                //                                  from b in _studentDashboardContext.IVRM_Master_SubjectsDMO
                //                                  where (a.ISMS_Id == b.ISMS_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.AMST_Id == data.AMST_Id
                //                                  && a.ESTSU_ActiveFlg==true && a.ASMCL_Id== viewstudentacademicyeardetails.FirstOrDefault().ASMCL_Id
                //                                  && a.ASMS_Id == viewstudentacademicyeardetails.FirstOrDefault().ASMS_Id)
                //                                  select new StudentDashboardDTO
                //                                  {
                //                                      ISMS_Id = a.ISMS_Id,
                //                                      ISMS_SubjectName = b.ISMS_SubjectName,
                //                                      subjorder = b.ISMS_OrderFlag,
                //                                      ESTSU_ElecetiveFlag = a.ESTSU_ElecetiveFlag
                //                                  }).Distinct().OrderBy(a => a.subjorder).ToArray();

                //Over All Attendance
                using (var cmd = _studentDashboardContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Adm_View_StudentWise_Attendance";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = data.AMST_Id });

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
                                        dataReader.IsDBNull(iFiled) ? "Not Available" : dataReader[iFiled] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.viewstudentattendancetails = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                // Year Month Wise Attendance 
                using (var cmd = _studentDashboardContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Adm_View_StudentWise_Attendance_MonthWise";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = data.AMST_Id });

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
                                        dataReader.IsDBNull(iFiled) ? "Not Available" : dataReader[iFiled] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.viewstudentattendanceMonthdetails = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                //Over All Fee
                using (var cmd = _studentDashboardContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Portal_View_StudentWise_FeeDetails";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = data.AMST_Id });

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
                                        dataReader.IsDBNull(iFiled) ? "Not Available" : dataReader[iFiled] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.viewstudentfeedetails = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                // Year Wise Fee Paid  Details
                using (var cmd = _studentDashboardContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Portal_View_StudentWise_Fee_YearDetails";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = data.AMST_Id });

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
                                        dataReader.IsDBNull(iFiled) ? "Not Available" : dataReader[iFiled] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.viewstudenfeeyeardetails = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                //Student Complaints
                using (var cmd = _studentDashboardContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Adm_StudentCompliants_View";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar)
                    {
                        Value = data.ASMAY_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar)
                    {
                        Value = data.AMST_Id
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
                        data.studentdivlist = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                //Student Exam
                using (var cmd = _studentDashboardContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Portal_View_Exam_YearWise_Details";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar)
                    {
                        Value = data.ASMAY_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar)
                    {
                        Value = data.AMST_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@Flag", SqlDbType.VarChar)
                    {
                        Value = data.student_staffflag
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
                        data.viewstudentwiseexamdetails = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                
                //Student Address
                using (var cmd = _studentDashboardContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Adm_StudentWise_Address_Details";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.BigInt) { Value = data.AMST_Id });

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
                                    dataRow.Add(dataReader.GetName(iFiled),
                                        dataReader.IsDBNull(iFiled) ? "Not Available" : dataReader[iFiled] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.viewstudentaddressdetails = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }



                //staff details
                using (var cmd = _studentDashboardContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "class_teacher_list";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.BigInt) { Value = asmcl_id });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.BigInt) { Value = asms_id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.BigInt) { Value = data.ASMAY_Id });

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
                                    dataRow.Add(dataReader.GetName(iFiled),
                                        dataReader.IsDBNull(iFiled) ? "Not Available" : dataReader[iFiled] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.classteacher = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }



             //   data.classteacher = _studentDashboardContext.ClassTeacherMappingDMO.Where(s => s.MI_Id == data.MI_Id && s.ASMCL_Id == asmcl_id && s.ASMS_Id == asms_id && s.ASMAY_Id == data.ASMAY_Id).ToArray();


                // data.sujectteachers=

                using (var cmd = _studentDashboardContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "subject_teacher_list";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.BigInt) { Value = data.AMST_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.BigInt) { Value = asmcl_id });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.BigInt) { Value = asms_id});
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.BigInt) { Value = data.ASMAY_Id });

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
                                    dataRow.Add(dataReader.GetName(iFiled),
                                        dataReader.IsDBNull(iFiled) ? "Not Available" : dataReader[iFiled] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.sujectteachers = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public StudentDashboardDTO ViewMonthWiseAttendance(StudentDashboardDTO data)
        {
            try
            {
                using (var cmd = _studentDashboardContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Adm_View_StudentWise_Attendance_MonthWise";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = data.AMST_Id });

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
                                        dataReader.IsDBNull(iFiled) ? "Not Available" : dataReader[iFiled] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.viewstudentattendanceMonthdetails = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public StudentDashboardDTO ViewYearWiseFee(StudentDashboardDTO data)
        {
            try
            {
                using (var cmd = _studentDashboardContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Portal_View_StudentWise_Fee_YearDetails";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = data.AMST_Id });

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
                                        dataReader.IsDBNull(iFiled) ? "Not Available" : dataReader[iFiled] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.viewstudenfeeyeardetails = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public StudentDashboardDTO ViewExamSubjectWiseDetails(StudentDashboardDTO data)
        {
            try
            {
                using (var cmd = _studentDashboardContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Portal_View_StudentWise_Exam_SubjectDetails";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = data.AMST_Id });
                    cmd.Parameters.Add(new SqlParameter("@EME_Id", SqlDbType.VarChar) { Value = data.EME_Id });

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
                                        dataReader.IsDBNull(iFiled) ? "Not Available" : dataReader[iFiled] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.viewstudentexamsubjectdetails = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public string duadatecollect(long MI_Id, long AMST_Id, long Class_Id_t, long ASMAY_Id, string templatename)
        {

            Dictionary<string, string> val = new Dictionary<string, string>();
            var template = _studentDashboardContext.SMSEmailSetting.Where(a => a.MI_Id == MI_Id && a.ISES_Template_Name == templatename).ToList();

            var Paramaeters = _studentDashboardContext.SMS_MAIL_SAVED_PARAMETER_DMO.Where(i => i.MI_Id == MI_Id && i.ISES_Id == template[0].ISES_Id && i.Flag == "M").Select(d => d.ISMP_ID).ToList();

            var ParamaetersName = _studentDashboardContext.SMS_MAIL_PARAMETER_DMO.Where(i => Paramaeters.Contains(i.ISMP_ID)).ToList();


            string Mailmsg = template.FirstOrDefault().ISES_MailHTMLTemplate;

            string result = Mailmsg;

            using (var cmd = _studentDashboardContext.Database.GetDbConnection().CreateCommand())
            {

                cmd.CommandText = "FEE_Balance_Amount_Show_in_portalnotification";
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@MI_Id",
         SqlDbType.VarChar)
                {
                    Value = MI_Id
                });
                cmd.Parameters.Add(new SqlParameter("@AMST_Id",
         SqlDbType.VarChar)
                {
                    Value = AMST_Id
                });
                cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                SqlDbType.VarChar)
                {
                    Value = Class_Id_t
                });
                cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
            SqlDbType.VarChar)
                {
                    Value = ASMAY_Id
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
                        //result = Mailmsg.Replace(ParamaetersName[j].ISMP_NAME, val.Values.ToArray()[p]);
                        result = Mailmsg.Replace(ParamaetersName[j].ISMP_NAME, val.Values.ToArray()[p]);
                        Mailmsg = result;
                    }
                }
            }
            Mailmsg = result;


            return Mailmsg;
        }
        public StudentDashboardDTO onclick_Homework_seen(StudentDashboardDTO dto)
        {
            try
            {
                //added
                var duplicate = _studentDashboardContext.IVRM_HomeWork_Upload_DMO_con.Where(b => b.IHW_Id == dto.IHW_Id && b.AMST_Id == dto.AMST_Id && b.IHWUPL_ViewedFlg == true).ToList();
                if (duplicate.Count > 0)
                {

                }
                else
                {
                    var check = _studentDashboardContext.IVRM_HomeWork_Upload_DMO_con.Where(a => a.IHW_Id == dto.IHW_Id && a.AMST_Id == dto.AMST_Id).ToList();
                    if (check.Count > 0)
                    {
                        var cud = _studentDashboardContext.IVRM_HomeWork_Upload_DMO_con.Single(t => t.IHW_Id.Equals(dto.IHW_Id) && t.AMST_Id == dto.AMST_Id);
                        //  IVRM_HomeWork_Upload_DMO cud = new IVRM_HomeWork_Upload_DMO();

                        cud.IHWUPL_ViewedFlg = true;
                        cud.IHWUPL_ViewedDateTime = DateTime.Now;
                        _studentDashboardContext.Update(cud);

                    }
                    else
                    {
                        IVRM_HomeWork_Upload_DMO cud = new IVRM_HomeWork_Upload_DMO();
                        cud.IHW_Id = dto.IHW_Id;
                        cud.AMST_Id = dto.AMST_Id;

                        cud.IHWUPL_ViewedFlg = true;
                        cud.IHWUPL_ViewedDateTime = DateTime.Now;
                        cud.UpdatedDate = DateTime.Now;
                        _studentDashboardContext.Add(cud);



                    }


                    var std = _studentDashboardContext.SaveChanges();
                }

                //
                
                var clssec1 = (from a in _studentDashboardContext.Adm_M_Student
                               from b in _studentDashboardContext.School_Adm_Y_StudentDMO
                               from c in _studentDashboardContext.School_M_Class
                               from s in _studentDashboardContext.School_M_Section
                               where (b.ASMCL_Id == c.ASMCL_Id && b.ASMS_Id == s.ASMS_Id && a.MI_Id == c.MI_Id && a.MI_Id == dto.MI_Id
                               && a.AMST_Id == dto.AMST_Id && b.AMST_Id == dto.AMST_Id && b.ASMS_Id== dto.ASMS_Id && b.ASMCL_Id == dto.ASMCL_Id)
                               select new StudentDashboardDTO
                               {
                                   ASMCL_Id = c.ASMCL_Id,
                                   ASMCL_ClassName = c.ASMCL_ClassName,
                                   ASMS_Id = s.ASMS_Id,
                                   ASMC_SectionName = s.ASMC_SectionName
                               }).Distinct().ToList();


                if (clssec1.Count == 0)
                {
                    dto.messag = "";
                }
                else
                {

                    long Class_Id = clssec1.FirstOrDefault().ASMCL_Id;
                    long Section_Id = clssec1.FirstOrDefault().ASMS_Id;

                    using (var cmd = _studentDashboardContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "Portal_HomeWorkClasswork_seenById";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = dto.MI_Id });
                        //cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.BigInt) { Value = dto.ASMAY_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.BigInt) { Value = dto.ASMCL_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.BigInt) { Value = Section_Id });
                        cmd.Parameters.Add(new SqlParameter("@type", SqlDbType.VarChar) { Value = "Homework" });
                        cmd.Parameters.Add(new SqlParameter("@Id", SqlDbType.VarChar) { Value = dto.IHW_Id });


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
                            dto.homeworklist_byid = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
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
        public StudentDashboardDTO onclick_classwork_seen(StudentDashboardDTO dto)
        {
            try
            {
                //added
                var duplicate = _studentDashboardContext.IVRM_ClassWork_Upload_DMO_con.Where(b => b.ICW_Id == dto.ICW_Id && b.AMST_Id == dto.AMST_Id && b.ICWUPL_ViewedFlg == true).ToList();
                if (duplicate.Count > 0)
                {

                }
                else
                {
                    var check = _studentDashboardContext.IVRM_ClassWork_Upload_DMO_con.Where(a => a.ICW_Id == dto.ICW_Id && a.AMST_Id == dto.AMST_Id).ToList();
                    if (check.Count > 0)
                    {
                        var cud = _studentDashboardContext.IVRM_ClassWork_Upload_DMO_con.Single(t => t.ICW_Id.Equals(dto.ICW_Id) && t.AMST_Id == dto.AMST_Id);
                        //  IVRM_HomeWork_Upload_DMO cud = new IVRM_HomeWork_Upload_DMO();

                        cud.ICWUPL_ViewedFlg = true;
                        cud.ICWUPL_ViewedDateTime = DateTime.Now;
                        cud.UpdatedDate = DateTime.Now;
                        _studentDashboardContext.Update(cud);

                    }
                    else
                    {
                        IVRM_ClassWork_Upload_DMO cud = new IVRM_ClassWork_Upload_DMO();
                        cud.ICW_Id = dto.ICW_Id;
                        cud.AMST_Id = dto.AMST_Id;

                        cud.ICWUPL_ViewedFlg = true;

                        cud.ICWUPL_ViewedDateTime = DateTime.Now;

                        cud.UpdatedDate = DateTime.Now;
                        _studentDashboardContext.Add(cud);
                    }


                    var std = _studentDashboardContext.SaveChanges();
                }

                //
                var clssec1 = (from a in _studentDashboardContext.Adm_M_Student
                               from b in _studentDashboardContext.School_Adm_Y_StudentDMO
                               from c in _studentDashboardContext.School_M_Class
                               from s in _studentDashboardContext.School_M_Section
                               where (b.ASMCL_Id == c.ASMCL_Id && b.ASMS_Id == s.ASMS_Id && a.MI_Id == c.MI_Id && a.MI_Id == dto.MI_Id
                               && a.AMST_Id == dto.AMST_Id && b.AMST_Id == dto.AMST_Id && b.ASMS_Id == dto.ASMS_Id && b.ASMCL_Id == dto.ASMCL_Id)
                               select new StudentDashboardDTO
                               {
                                   ASMCL_Id = c.ASMCL_Id,
                                   ASMCL_ClassName = c.ASMCL_ClassName,
                                   ASMS_Id = s.ASMS_Id,
                                   ASMC_SectionName = s.ASMC_SectionName
                               }).Distinct().ToList();


                if (clssec1.Count == 0)
                {
                    dto.messag = "";
                }
                else
                {

                    long Class_Id = clssec1.FirstOrDefault().ASMCL_Id;
                    long Section_Id = clssec1.FirstOrDefault().ASMS_Id;

                    using (var cmd = _studentDashboardContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "Portal_HomeWorkClasswork_seenById";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = dto.MI_Id });
                        //cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.BigInt) { Value = dto.ASMAY_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.BigInt) { Value = dto.ASMCL_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.BigInt) { Value = Section_Id });
                        cmd.Parameters.Add(new SqlParameter("@type", SqlDbType.VarChar) { Value = "classwork" });
                        cmd.Parameters.Add(new SqlParameter("@Id", SqlDbType.VarChar) { Value = dto.ICW_Id });


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
                            dto.classworklist_byid = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
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
        public StudentDashboardDTO onclick_noticeboard_seen(StudentDashboardDTO dto)
        {
            try
            {
                //added
                var duplicate = _studentDashboardContext.IVRM_NoticeBoard_Student_ViewedDMO.Where(b => b.INTB_Id == dto.INTB_Id && b.AMST_Id == dto.AMST_Id && b.INTBCSTDV_ActiveFlag == true).ToList();
                if (duplicate.Count > 0)
                {

                }
                else
                {
                    var check = _studentDashboardContext.IVRM_NoticeBoard_Student_ViewedDMO.Where(a => a.INTB_Id == dto.INTB_Id && a.AMST_Id == dto.AMST_Id).ToList();
                    if (check.Count > 0)
                    {
                        var cud = _studentDashboardContext.IVRM_NoticeBoard_Student_ViewedDMO.Single(t => t.INTB_Id.Equals(dto.INTB_Id) && t.AMST_Id == dto.AMST_Id);
                        //  IVRM_HomeWork_Upload_DMO cud = new IVRM_HomeWork_Upload_DMO();

                        cud.INTBCSTDV_ActiveFlag = true;
                        cud.INTBCSTDV_UpdatedDate = DateTime.Now;
                        cud.INTBCSTDV_UpdatedBy = dto.User_Id;
                        _studentDashboardContext.Update(cud);

                    }
                    else
                    {
                        IVRM_NoticeBoard_Student_ViewedDMO cud = new IVRM_NoticeBoard_Student_ViewedDMO();
                        cud.INTB_Id = dto.INTB_Id;
                        cud.AMST_Id = dto.AMST_Id;

                        cud.INTBCSTDV_ActiveFlag = true;
                        cud.INTBCSTDV_CreatedDate= DateTime.Now; 
                        cud.INTBCSTDV_UpdatedDate = DateTime.Now;
                        cud.INTBCSTDV_UpdatedBy = dto.User_Id;
                        cud.INTBCSTDV_CreatedBy = dto.User_Id;


                        _studentDashboardContext.Add(cud);
                    }


                    var std = _studentDashboardContext.SaveChanges();
                }

                //
                var clssec1 = (from a in _studentDashboardContext.Adm_M_Student
                               from b in _studentDashboardContext.School_Adm_Y_StudentDMO
                               from c in _studentDashboardContext.School_M_Class
                               from s in _studentDashboardContext.School_M_Section
                               where (b.ASMCL_Id == c.ASMCL_Id && b.ASMS_Id == s.ASMS_Id && a.MI_Id == c.MI_Id && a.MI_Id == dto.MI_Id
                               && a.AMST_Id == dto.AMST_Id && b.AMST_Id == dto.AMST_Id &&  b.ASMAY_Id==dto.ASMAY_Id)
                               select new StudentDashboardDTO
                               {
                                   ASMCL_Id = c.ASMCL_Id,
                                   ASMCL_ClassName = c.ASMCL_ClassName,
                                   ASMS_Id = s.ASMS_Id,
                                   ASMC_SectionName = s.ASMC_SectionName
                               }).Distinct().ToList();


                if (clssec1.Count == 0)
                {
                    dto.messag = "";
                }
                else
                {

                    long Class_Id = clssec1.FirstOrDefault().ASMCL_Id;
                    long Section_Id = clssec1.FirstOrDefault().ASMS_Id;

                    using (var cmd = _studentDashboardContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "Portal_NoticeBoardYearWise_seenId";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                 SqlDbType.VarChar)
                        {
                            Value = dto.MI_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                        SqlDbType.VarChar)
                        {
                            //Value = Class_Id

                            Value= Class_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                    SqlDbType.VarChar)
                        {
                            Value = dto.ASMAY_Id
                        });

                        cmd.Parameters.Add(new SqlParameter("@AMST_Id",
                   SqlDbType.VarChar)
                        {
                            Value = dto.AMST_Id
                        });

                        cmd.Parameters.Add(new SqlParameter("@Flag",
                    SqlDbType.VarChar)
                        {
                            Value = dto.flag
                        });

                        cmd.Parameters.Add(new SqlParameter("@Type",
                    SqlDbType.VarChar)
                        {
                            Value = "Student"
                        });
                        cmd.Parameters.Add(new SqlParameter("@INTB_Id",
                  SqlDbType.VarChar)
                        {
                            Value = dto.INTB_Id
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
                            dto.noticelist_byid = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
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



        public StudentDashboardDTO onclick_Homework_datewise(StudentDashboardDTO dto)
        {
            try
            {
                string startdate = "";
                string enddate = "";

                var clssec1 = (from a in _studentDashboardContext.Adm_M_Student
                               from b in _studentDashboardContext.School_Adm_Y_StudentDMO
                               from c in _studentDashboardContext.School_M_Class
                               from s in _studentDashboardContext.School_M_Section
                               where (b.ASMCL_Id == c.ASMCL_Id && b.ASMS_Id == s.ASMS_Id && a.MI_Id == c.MI_Id && a.MI_Id == dto.MI_Id
                               && b.ASMAY_Id == dto.ASMAY_Id && a.AMST_Id == dto.AMST_Id && b.AMST_Id == dto.AMST_Id)
                               select new StudentDashboardDTO
                               {
                                   ASMCL_Id = c.ASMCL_Id,
                                   ASMCL_ClassName = c.ASMCL_ClassName,
                                   ASMS_Id = s.ASMS_Id,
                                   ASMC_SectionName = s.ASMC_SectionName
                               }).Distinct().ToList();


                if (clssec1.Count == 0)
                {
                    dto.messag = "";
                }
                else
                {

                    startdate = dto.INTB_StartDate.ToString("yyyy-MM-dd");
                    enddate = dto.INTB_EndDate.ToString("yyyy-MM-dd");

                    long Class_Id = clssec1.FirstOrDefault().ASMCL_Id;
                    long Section_Id = clssec1.FirstOrDefault().ASMS_Id;

                    using (var cmd = _studentDashboardContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "Portal_HomeWorkClasswork_datewise";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = dto.MI_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.BigInt) { Value = dto.ASMAY_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.BigInt) { Value = Class_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.BigInt) { Value = Section_Id });
                        cmd.Parameters.Add(new SqlParameter("@type", SqlDbType.VarChar) { Value = "Homework" });
                        cmd.Parameters.Add(new SqlParameter("@startdate", SqlDbType.DateTime) { Value = startdate });
                        cmd.Parameters.Add(new SqlParameter("@enddate",SqlDbType.DateTime) {Value = enddate });
                        cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.BigInt) { Value = dto.AMST_Id });


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
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }
        public StudentDashboardDTO onclick_classwork_datewise(StudentDashboardDTO dto)
        {
            try
            {

                string startdate = "";
                string enddate = ""; 

                var clssec1 = _studentDashboardContext.School_Adm_Y_StudentDMO.Where(a => a.ASMAY_Id == dto.ASMAY_Id && a.AMST_Id == dto.AMST_Id
               && a.AMAY_ActiveFlag == 1).ToList();


                if (clssec1.Count == 0)
                {
                    dto.messag = "";
                }
                else
                {
                    startdate = dto.INTB_StartDate.ToString("yyyy-MM-dd");
                    enddate = dto.INTB_EndDate.ToString("yyyy-MM-dd");


                    long Class_Id = clssec1.FirstOrDefault().ASMCL_Id;
                    long Section_Id = clssec1.FirstOrDefault().ASMS_Id;

                    using (var cmd = _studentDashboardContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "PORTAL_StudentDashboard";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                    SqlDbType.BigInt)
                        {
                            Value = dto.MI_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                         SqlDbType.BigInt)
                        {
                            Value = dto.ASMAY_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@AMST_Id",
                  SqlDbType.BigInt)
                        {
                            Value = dto.AMST_Id
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
                            dto.studetailslist = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                    using (var cmd = _studentDashboardContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "Portal_HomeWorkClasswork_Datewise";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = dto.MI_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.BigInt) { Value = dto.ASMAY_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.BigInt) { Value = Class_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.BigInt) { Value = Section_Id });
                        cmd.Parameters.Add(new SqlParameter("@type", SqlDbType.VarChar) { Value = "Classwork" });
                        cmd.Parameters.Add(new SqlParameter("@startdate", SqlDbType.DateTime) { Value = startdate });
                        cmd.Parameters.Add(new SqlParameter("@enddate", SqlDbType.DateTime) { Value = enddate });
                        cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.BigInt) { Value = dto.AMST_Id });
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

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }
        public StudentDashboardDTO onclick_noticeboard_datewise(StudentDashboardDTO dto)
        {
            try
            {
                var clssec1 = _studentDashboardContext.School_Adm_Y_StudentDMO.Where(a => a.ASMAY_Id == dto.ASMAY_Id && a.AMST_Id == dto.AMST_Id
                && a.AMAY_ActiveFlag == 1).ToList();


                if (clssec1.Count == 0)
                {
                    dto.messag = "";
                }
                else
                {
                    long Class_Id = clssec1.FirstOrDefault().ASMCL_Id;
                    long Section_Id = clssec1.FirstOrDefault().ASMS_Id;

                    var date = DateTime.Now;
                    using (var cmd = _studentDashboardContext.Database.GetDbConnection().CreateCommand())
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
                            Value = dto.AMST_Id
                        });

                        cmd.Parameters.Add(new SqlParameter("@Flag",
                    SqlDbType.VarChar)
                        {
                            Value = 'O'
                        });

                        cmd.Parameters.Add(new SqlParameter("@Type",
                    SqlDbType.VarChar)
                        {
                            Value = "Student"
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



            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }
        public StudentDashboardDTO onclick_notice_datewise(StudentDashboardDTO dto)
        {
            try
            {
                var clssec1 = _studentDashboardContext.School_Adm_Y_StudentDMO.Where(a => a.ASMAY_Id == dto.ASMAY_Id && a.AMST_Id == dto.AMST_Id
                && a.AMAY_ActiveFlag == 1).ToList();


                if (clssec1.Count == 0)
                {
                    dto.messag = "";
                }
                else
                {
                    long Class_Id = clssec1.FirstOrDefault().ASMCL_Id;
                    long Section_Id = clssec1.FirstOrDefault().ASMS_Id;

                    var date = DateTime.Now;
                    using (var cmd = _studentDashboardContext.Database.GetDbConnection().CreateCommand())
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
                            Value = dto.AMST_Id
                        });

                        cmd.Parameters.Add(new SqlParameter("@Flag",
                    SqlDbType.VarChar)
                        {
                            Value = 'O'
                        });

                        cmd.Parameters.Add(new SqlParameter("@Type",
                    SqlDbType.VarChar)
                        {
                            Value = "Student"
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

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }

      
        public StudentDashboardDTO onclick_Staff_details(StudentDashboardDTO dto)
        {
            try
            {

                using (var cmd = _studentDashboardContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Staff_profile";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = dto.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.BigInt) { Value = dto.HRME_Id });
           

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
                                    dataRow.Add(dataReader.GetName(iFiled),
                                        dataReader.IsDBNull(iFiled) ? "Not Available" : dataReader[iFiled] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        dto.staffdetails = retObject.ToArray();
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