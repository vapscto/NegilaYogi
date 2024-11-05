using PreadmissionDTOs.com.vaps.Portals.Student;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using DataAccessMsSqlServerProvider.com.vapstech.Portals;

namespace PortalHub.com.vaps.Student.Services
{
    public class AttendanceDetailsImpl : Interfaces.AttendanceDetailsInterface
    {
        private static ConcurrentDictionary<string, StudentDashboardDTO> _login =
           new ConcurrentDictionary<string, StudentDashboardDTO>();
        private PortalContext _Attcontext;
        public AttendanceDetailsImpl(PortalContext Attcontext)
        {
            _Attcontext = Attcontext;
        }

        public StudentDashboardDTO getloaddata(StudentDashboardDTO data)
        {
            try
            {
                data.attyearlist = (from d in _Attcontext.AcademicYearDMO
                                    from a in _Attcontext.School_M_Class
                                    from b in _Attcontext.School_M_Section
                                    from c in _Attcontext.School_Adm_Y_StudentDMO
                                    where (c.AMST_Id == data.AMST_Id && a.ASMCL_Id == c.ASMCL_Id && b.ASMS_Id == c.ASMS_Id && d.ASMAY_Id == c.ASMAY_Id &&
                                    a.MI_Id == data.MI_Id && b.MI_Id == data.MI_Id && d.MI_Id == data.MI_Id)
                                    select new StudentDashboardDTO
                                    {
                                        ASMAY_Id = c.ASMAY_Id,
                                        ASMAY_Year = d.ASMAY_Year,
                                        ASMAY_Order = d.ASMAY_Order
                                    }
                             ).OrderByDescending(T => T.ASMAY_Order).ToArray();

                data.currentyear = (from a in _Attcontext.School_Adm_Y_StudentDMO
                                    from b in _Attcontext.AcademicYearDMO
                                    where (b.ASMAY_Id == data.ASMAY_Id && a.AMST_Id == data.AMST_Id && b.MI_Id == data.MI_Id)
                                    select new StudentDashboardDTO
                                    {
                                        ASMAY_Id = b.ASMAY_Id,
                                        ASMAY_Year = b.ASMAY_Year,
                                        ASMAY_Order=b.ASMAY_Order
                                    }
                           ).OrderByDescending(T=>T.ASMAY_Order).ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public async Task<StudentDashboardDTO> getAttdata(StudentDashboardDTO data)
        {
            try
            {
                using (var cmd = _Attcontext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "PORTAL_STUDENT_MONTHLY_ATTENDANCE";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@asmay_id",
             SqlDbType.VarChar)
                    {
                        Value = data.ASMAY_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@mi_id",
                     SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@amst_id",
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
                        data.attList = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
    }
}
