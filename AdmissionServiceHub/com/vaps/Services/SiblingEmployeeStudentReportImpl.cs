using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using DataAccessMsSqlServerProvider;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.admission;

namespace AdmissionServiceHub.com.vaps.Services
{
    public class SiblingEmployeeStudentReportImpl : Interfaces.SiblingEmployeeStudentReportInterface
    {
        public AdmissionFormContext _context;
        public SiblingEmployeeStudentReportImpl(AdmissionFormContext _Admission)
        {
            _context = _Admission;
        }
        public SiblingEmployeeStudentReportDTO getdetails(SiblingEmployeeStudentReportDTO data)
        {
            try
            {
                data.getyearlist = _context.year.Where(a => a.MI_Id == data.MI_Id && a.Is_Active == true).OrderByDescending(a => a.ASMAY_Order).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public SiblingEmployeeStudentReportDTO getreport(SiblingEmployeeStudentReportDTO data)
        {
            try
            {
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Sibling_Studnet_Employee_Student_Report";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar)
                    {
                        Value = data.ASMAY_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@Flag", SqlDbType.VarChar)
                    {
                        Value = data.reporttype
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
                        data.getreportdetails = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                data.getinstitutiondetails = _context.Institution.Where(a => a.MI_Id == data.MI_Id && a.MI_ActiveFlag == 1).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
    }
}