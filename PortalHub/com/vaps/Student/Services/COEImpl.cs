using PreadmissionDTOs.com.vaps.Portals.Student;
using System;
using System.Linq;
using System.Collections.Concurrent;
using DataAccessMsSqlServerProvider.com.vapstech.Portals;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlClient;
using System.Data;
using System.Collections.Generic;
using System.Dynamic;

namespace PortalHub.com.vaps.Student.Services
{
    public class COEImpl : Interfaces.COEInterface
    {
        private static ConcurrentDictionary<string, StudentDashboardDTO> _login =
           new ConcurrentDictionary<string, StudentDashboardDTO>();
        private PortalContext _coecontext;
        public COEImpl(PortalContext coecontext)
        {
            _coecontext = coecontext;
        }
        public StudentDashboardDTO getloaddata(StudentDashboardDTO data)
        {
            try
            {
                //data.coeyearlist = (from a in _coecontext.School_Adm_Y_StudentDMO
                //                    from b in _coecontext.AcademicYearDMO
                //                    where (b.MI_Id == data.MI_Id && a.AMST_Id == data.AMST_Id)
                //                    select new StudentDashboardDTO
                //                    {
                //                        ASMAY_Id = b.ASMAY_Id,
                //                        ASMAY_Year = b.ASMAY_Year
                //                    }
                //             ).ToArray();

                data.currentyear = (from a in _coecontext.School_Adm_Y_StudentDMO
                                    from b in _coecontext.AcademicYearDMO
                                    where (b.ASMAY_Id == data.ASMAY_Id && a.AMST_Id == data.AMST_Id && b.MI_Id == data.MI_Id)
                                    select new StudentDashboardDTO
                                    {
                                        ASMAY_Id = b.ASMAY_Id,
                                        ASMAY_Year = b.ASMAY_Year,
                                        ASMAY_Order=b.ASMAY_Order
                                    }
                             ).OrderByDescending(t=>t.ASMAY_Order).ToArray();
                data.coeyearlist = (from d in _coecontext.AcademicYearDMO
                                    from a in _coecontext.School_M_Class
                                    from b in _coecontext.School_M_Section
                                    from c in _coecontext.School_Adm_Y_StudentDMO
                                    where (c.AMST_Id == data.AMST_Id && a.ASMCL_Id == c.ASMCL_Id && b.ASMS_Id == c.ASMS_Id && d.ASMAY_Id == c.ASMAY_Id &&
                                    a.MI_Id == data.MI_Id && b.MI_Id == data.MI_Id && d.MI_Id == data.MI_Id)
                                    select new ExamDTO
                                    {
                                        ASMCL_Id = c.ASMCL_Id,
                                        ASMCL_ClassName = a.ASMCL_ClassName,
                                        ASMS_Id = c.ASMS_Id,
                                        ASMC_SectionName = b.ASMC_SectionName,
                                        ASMAY_Id = c.ASMAY_Id,
                                        ASMAY_Year = d.ASMAY_Year,
                                        ASMAY_Order = d.ASMAY_Order
                                    }
                             ).OrderByDescending(t => t.ASMAY_Order).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public StudentDashboardDTO getcoedata(StudentDashboardDTO data)
        {
            try
            {
                var clssec1 = (from a in _coecontext.Adm_M_Student
                               from b in _coecontext.School_Adm_Y_StudentDMO
                               from c in _coecontext.School_M_Class
                               from s in _coecontext.School_M_Section
                               where (b.ASMCL_Id == c.ASMCL_Id && b.ASMS_Id == s.ASMS_Id && a.MI_Id == c.MI_Id && a.MI_Id == data.MI_Id
                               && b.ASMAY_Id == data.ASMAY_Id && a.AMST_Id == data.AMST_Id && b.AMST_Id == data.AMST_Id)
                               select new StudentDashboardDTO
                               {
                                   ASMCL_Id = c.ASMCL_Id,
                                   ASMCL_ClassName = c.ASMCL_ClassName,
                                   ASMS_Id = s.ASMS_Id,
                                   ASMC_SectionName = s.ASMC_SectionName
                               }).Distinct().ToList();

                //data.coereportlist = (from a in _coecontext.COE_Master_EventsDMO
                //                      from b in _coecontext.COE_EventsDMO
                //                      from c in _coecontext.School_Adm_Y_StudentDMO
                //                      from d in _coecontext.Adm_M_Student
                //                      from e in _coecontext.COE_Events_ClassesDMO
                //                      where (a.COEME_Id == b.COEME_Id && b.COEE_Id == e.COEE_Id && b.ASMAY_Id == c.ASMAY_Id && c.AMST_Id == d.AMST_Id && c.AMST_Id == data.AMST_Id && b.ASMAY_Id == data.ASMAY_Id && c.ASMCL_Id == clssec1.FirstOrDefault().ASMCL_Id && a.MI_Id == data.MI_Id && b.COEE_EStartDate.Value.Month == data.month && b.COEE_ActiveFlag==true)
                //                      select new StudentDashboardDTO
                //                      {
                //                          COEME_EventName = a.COEME_EventName,
                //                          COEME_EventDesc = a.COEME_EventDesc,
                //                          COEE_EStartDate = b.COEE_EStartDate,
                //                          COEE_EEndDate = b.COEE_EEndDate,
                //                          ASMAY_Id = b.ASMAY_Id,
                //                          COEE_EStartTime = b.COEE_EStartTime,
                //                          COEE_EEndTime = b.COEE_EEndTime,
                //                      }).Distinct().OrderBy(t => t.COEE_EStartDate).ToArray();


                using (var cmd = _coecontext.Database.GetDbConnection().CreateCommand())                {                    cmd.CommandText = "COE_Events_Details";                    cmd.CommandType = CommandType.StoredProcedure;                    cmd.Parameters.Add(new SqlParameter("@mi_id",             SqlDbType.BigInt)                    {                        Value = data.MI_Id                    });                    cmd.Parameters.Add(new SqlParameter("@amst_id",                     SqlDbType.BigInt)                    {                        Value = data.AMST_Id                    });                    cmd.Parameters.Add(new SqlParameter("@asmay_id ",                      SqlDbType.BigInt)                    {                        Value = data.ASMAY_Id                    });
                    cmd.Parameters.Add(new SqlParameter("@asmcl_id ",                      SqlDbType.BigInt)                    {                        Value = clssec1.FirstOrDefault().ASMCL_Id                    });
                    cmd.Parameters.Add(new SqlParameter("@month_id ",                      SqlDbType.BigInt)                    {                        Value = data.month                    });                    if (cmd.Connection.State != ConnectionState.Open)                        cmd.Connection.Open();                    var retObject = new List<dynamic>();                    try                    {                        using (var dataReader = cmd.ExecuteReader())                        {                            while (dataReader.Read())                            {                                var dataRow = new ExpandoObject() as IDictionary<string, object>;                                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)                                {                                    dataRow.Add(                                        dataReader.GetName(iFiled),                                        dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}                                    );                                }                                retObject.Add((ExpandoObject)dataRow);                            }                        }                        data.coereportlist = retObject.ToArray();                    }                    catch (Exception ex)                    {                        Console.WriteLine(ex.Message);                    }                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
    }
}




