using DataAccessMsSqlServerProvider;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.com.vaps.admission;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace AdmissionServiceHub.com.vaps.Services
{
    public class SwimmingAttendanceReportImpl : Interfaces.SwimmingAttendanceReportInterface
    {
        private DomainModelMsSqlServerContext _context;

        ILogger<SwimmingAttendanceReportImpl> _acdimpl;

        public SwimmingAttendanceReportImpl(DomainModelMsSqlServerContext _contex, ILogger<SwimmingAttendanceReportImpl> _acdimp)
        {
            _context = _contex;
            _acdimpl = _acdimp;
        }
        public SwimmingAttendanceReportDTO loaddata(SwimmingAttendanceReportDTO data)
        {
            try
            {
                data.getyear = _context.AcademicYear.Where(a => a.MI_Id == data.MI_Id && a.Is_Active == true).OrderByDescending(a => a.ASMAY_Order).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public SwimmingAttendanceReportDTO onchnageyear(SwimmingAttendanceReportDTO data)
        {
            try
            {
                data.getclass = (from a in _context.Masterclasscategory
                                 from b in _context.School_M_Class
                                 from c in _context.AcademicYear
                                 where (a.ASMAY_Id == c.ASMAY_Id && a.ASMCL_Id == b.ASMCL_Id && a.Is_Active == true && b.ASMCL_ActiveFlag == true && a.ASMAY_Id == data.ASMAY_Id)
                                 select b).Distinct().OrderBy(a => a.ASMCL_Order).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public SwimmingAttendanceReportDTO onchangeclass(SwimmingAttendanceReportDTO data)
        {
            try
            {
                data.getsection = (from a in _context.Masterclasscategory
                                   from b in _context.School_M_Class
                                   from c in _context.AcademicYear
                                   from d in _context.AdmSchoolMasterClassCatSec
                                   from e in _context.School_M_Section
                                   where (a.ASMAY_Id == c.ASMAY_Id && a.ASMCL_Id == b.ASMCL_Id && a.ASMCC_Id == d.ASMCC_Id && d.ASMS_Id == e.ASMS_Id
                                   && e.ASMC_ActiveFlag == 1 && a.Is_Active == true && b.ASMCL_ActiveFlag == true && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id && d.ASMCCS_ActiveFlg == true)
                                   select e).Distinct().OrderBy(a => a.ASMC_Order).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public SwimmingAttendanceReportDTO search(SwimmingAttendanceReportDTO data)
        {
            try
            {
                string confromdate = "";
                DateTime fromdatecon = Convert.ToDateTime(data.date.Value.Date.ToString("yyyy-MM-dd"));
                confromdate = fromdatecon.ToString("yyyy-MM-dd");

                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    _acdimpl.LogInformation("smart card stored procedure absent details ");
                    cmd.CommandText = "Admission_Swimming_Attendance_Report";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                    cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar) { Value = data.flag });
                    cmd.Parameters.Add(new SqlParameter("@REPORTTYPE", SqlDbType.VarChar) { Value = data.reporttype });
                    cmd.Parameters.Add(new SqlParameter("@DATE", SqlDbType.DateTime) { Value = confromdate });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();


                    var retObject = new List<dynamic>();

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
                    data.getreport = retObject.ToArray();
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
