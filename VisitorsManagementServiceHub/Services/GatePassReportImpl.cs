using DataAccessMsSqlServerProvider.com.vapstech.VisitorsManagement;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.VisitorsManagement;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using VisitorsManagementServiceHub.Interfaces;

namespace VisitorsManagementServiceHub.Services
{
    public class GatePassReportImpl : GatePassReportInterface
    {
        public VisitorsManagementContext visctxt;
        public GatePassReportImpl(VisitorsManagementContext context)
        {
            visctxt = context;
        }
        public async Task<GatePassReportDTO> report(GatePassReportDTO data)
        {

            try
            {
                #region
                //if (data.radiotype == "std")
                //{
                //    data.viewlist = (from a in visctxt.Adm_M_Student
                //                     from b in visctxt.Gate_Pass_Student_DMO
                //                     from y in visctxt.School_Adm_Y_StudentDMO
                //                     from c in visctxt.admissionClass
                //                     from s in visctxt.masterSection
                //                     where (b.MI_Id == data.MI_Id && a.AMST_Id == b.AMST_Id && y.AMST_Id==b.AMST_Id && y.ASMCL_Id==c.ASMCL_Id && y.ASMS_Id==s.ASMS_Id && y.ASMAY_Id==data.ASMAY_Id)
                //                     select new GatePassReportDTO
                //                     {
                //                         AMST_AdmNo = a.AMST_AdmNo,                                        
                //                         AMST_FirstName = a.AMST_FirstName + (string.IsNullOrEmpty(a.AMST_MiddleName) ? "" : ' ' + a.AMST_MiddleName) + (string.IsNullOrEmpty(a.AMST_LastName) ? "" : ' ' + a.AMST_LastName),
                //                         AMST_MobileNo = a.AMST_MobileNo,
                //                         AMST_emailId = a.AMST_emailId,
                //                         GPHS_GatePassNo=b.GPHS_GatePassNo,
                //                         GPHS_IDCardNo=b.GPHS_IDCardNo,
                //                         GPHS_DateTime=b.GPHS_DateTime,
                //                         GPHS_Remarks=b.GPHS_Remarks,
                //                         ASMCL_ClassName=c.ASMCL_ClassName,
                //                         ASMC_SectionName=s.ASMC_SectionName,

                //                     }).ToArray();


                //}
                //else if (data.radiotype == "emp")
                //{
                //    data.viewlist = (from a in visctxt.MasterEmployee
                //                     from b in visctxt.Gate_Pass_Staff_DMO
                //                     from d in visctxt.HR_Master_Department
                //                     from e in visctxt.HR_Master_Designation
                //                         //from c in visctxt.Emp_MobileNo
                //                     where (a.MI_Id == data.MI_Id  && b.HRME_Id == a.HRME_Id && d.HRMD_Id==a.HRMD_Id && e.HRMDES_Id==a.HRMDES_Id)
                //                     /*&& a.HRME_Id==c.HRME_Id && c.HRMEMNO_DeFaultFlag=="default"*/
                //                     select new GatePassReportDTO
                //                     {
                //                         HRME_EmployeeCode = a.HRME_EmployeeCode,

                //                         HRME_EmployeeFirstName = a.HRME_EmployeeFirstName + (string.IsNullOrEmpty(a.HRME_EmployeeMiddleName) ? "" : ' ' + a.HRME_EmployeeMiddleName) + (string.IsNullOrEmpty(a.HRME_EmployeeLastName) ? "" : ' ' + a.HRME_EmployeeLastName),
                //                         HRME_EmailId = a.HRME_EmailId,
                //                         GPHST_GatePassNo=b.GPHST_GatePassNo,
                //                         GPHST_IDCardNo=b.GPHST_IDCardNo,
                //                         GPHST_DateTime=b.GPHST_DateTime,
                //                         GPHST_Remarks=b.GPHST_Remarks,
                //                         HRME_MobileNo = a.HRME_MobileNo,
                //                         HRMD_DepartmentName = d.HRMD_DepartmentName,
                //                         HRMDES_DesignationName = e.HRMDES_DesignationName,
                //                     }).ToArray();


                //}
                #endregion


                if (data.all1 == "1")
                {
                    data.month_id = "";
                }
                else
                {
                    data.fromdate = "";
                    data.todate = "";
                }


                using (var cmd = visctxt.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Gate_Pass_Report";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                  
                    cmd.Parameters.Add(new SqlParameter("@radiotype",
                   SqlDbType.VarChar)
                    {
                        Value = data.radiotype
                    });
                    cmd.Parameters.Add(new SqlParameter("@fromdate",
                  SqlDbType.VarChar)
                    {
                        Value = data.fromdate
                    });
                    cmd.Parameters.Add(new SqlParameter("@todate",
                  SqlDbType.VarChar)
                    {
                        Value = data.todate
                    });
                    cmd.Parameters.Add(new SqlParameter("@months",
                 SqlDbType.VarChar)
                    {
                        Value = data.month_id
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
                        data.viewlist = retObject.ToArray();

                    }


                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                data.yarname = (from z in visctxt.AcademicYear
                              where (z.ASMAY_Id == data.ASMAY_Id && z.MI_Id == data.MI_Id)
                              select new GatePassReportDTO
                              {
                                  ASMAY_Year = z.ASMAY_Year
                              }).Distinct().ToArray();

            }

            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }


        public GatePassReportDTO loaddata(GatePassReportDTO data)
        {
            try
            {
                var q = (from a in visctxt.month
                         where (a.Is_Active == true)
                         select new
                         {
                             monthid = a.IVRM_Month_Id,
                             monthname = a.IVRM_Month_Name,
                         }).Distinct().ToArray();

                var query = q.Distinct().ToArray();
                data.month_list = (from a in query
                                 select new GatePassReportDTO
                                 {
                                     monthid = Convert.ToInt32(a.monthid),
                                     monthname = a.monthname
                                 }).Distinct().OrderBy(t => t.monthid).ToArray();

                //var q = (from a in visctxt.holidaydate
                //         where (a.MI_Id == data.MI_Id && a.FOMHWD_ActiveFlg == true)
                //         select new
                //         {
                //             monthid = a.FOMHWDD_FromDate.Value.Month,
                //             monthname = Convert.ToDateTime(a.FOMHWDD_FromDate).ToString("MMMMM").ToString()
                //         }).Distinct().ToArray();

                //var query = q.Distinct().ToArray();
                //data.month_list = (from a in query
                //                   select new GatePassReportDTO
                //                   {
                //                       monthid = a.monthid,
                //                       monthname = a.monthname
                //                   }).Distinct().OrderBy(t => t.monthid).ToArray();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return data;
        }


        public async Task<GatePassReportDTO> reportforMobile(GatePassReportDTO data)
        {

            try
            {
                
                using (var cmd = visctxt.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Gate_Pass_Report_For_Mobile";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@radiotype",
                   SqlDbType.VarChar)
                    {
                        Value = data.radiotype
                    });
                    cmd.Parameters.Add(new SqlParameter("@fromdate",
                  SqlDbType.VarChar)
                    {
                        Value = data.fromdate
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
                        data.viewlist = retObject.ToArray();

                    }


                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                data.yarname = (from z in visctxt.AcademicYear
                                where (z.ASMAY_Id == data.ASMAY_Id && z.MI_Id == data.MI_Id)
                                select new GatePassReportDTO
                                {
                                    ASMAY_Year = z.ASMAY_Year
                                }).Distinct().ToArray();

            }

            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }

        #region
        //public GatePassReportDTO report(GatePassReportDTO data)
        //{

        //    try
        //    {
        //        if (data.radiotype == "std")
        //        {
        //            data.viewlist = (from a in visctxt.Adm_M_Student
        //                             from b in visctxt.GatePassDMO
        //                             where (b.MI_Id == data.MI_Id && b.AGPH_PassType == "Student" && a.AMST_Id == b.AGPH_Idno)
        //                             select new GatePassReportDTO
        //                             {
        //                                 AMST_AdmNo = a.AMST_AdmNo,
        //                                 AGPH_PassType = b.AGPH_PassType,
        //                                 AGPH_Dategiven = b.AGPH_Dategiven,
        //                                 AGPH_Remark = b.AGPH_Remark,
        //                                 AMST_FirstName = a.AMST_FirstName,
        //                                 AMST_MobileNo = a.AMST_MobileNo,
        //                                 AMST_emailId = a.AMST_emailId
        //                             }
        //                             ).ToArray();

        //            //data.viewlist = visctxt.GatePassDMO.Where(a => a.MI_Id == data.MI_Id && a.AGPH_PassType == "Student").ToArray();
        //        }
        //        else if (data.radiotype == "emp")
        //        {
        //            data.viewlist = (from a in visctxt.MasterEmployee
        //                             from b in visctxt.GatePassDMO
        //                             where (a.MI_Id == data.MI_Id && b.AGPH_PassType == "Employee" && b.AGPH_Idno == a.HRME_Id)
        //                             select new GatePassReportDTO
        //                             {
        //                                 HRME_EmployeeCode = a.HRME_EmployeeCode,
        //                                 AGPH_PassType = b.AGPH_PassType,
        //                                 AGPH_Dategiven = b.AGPH_Dategiven,
        //                                 AGPH_Remark = b.AGPH_Remark,
        //                                 HRME_EmployeeFirstName = a.HRME_EmployeeFirstName,
        //                                 HRME_EmailId = a.HRME_EmailId,
        //                                 HRME_MobileNo = a.HRME_MobileNo
        //                             }
        //                             ).ToArray();
        //            //data.viewlist = visctxt.GatePassDMO.Where(a => a.MI_Id == data.MI_Id && a.AGPH_PassType == "Employee").ToArray();
        //        }

        //    }

        //    catch (Exception e)
        //    {
        //        Console.WriteLine(e.Message);
        //    }
        //    return data;
        //}
        #endregion
    }
}
