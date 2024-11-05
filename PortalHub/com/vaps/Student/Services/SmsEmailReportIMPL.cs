using DataAccessMsSqlServerProvider.com.vapstech.Portals;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.Portals.Student;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace PortalHub.com.vaps.Student.Services
{
    public class SmsEmailReportIMPL : Interfaces.SmsEmailReportInterFace
    {
        private static ConcurrentDictionary<string, SmsEmailReportDTO> _login =
          new ConcurrentDictionary<string, SmsEmailReportDTO>();
        private PortalContext _Portalcontext;

        public SmsEmailReportIMPL(PortalContext Feecontext)
        {            
            _Portalcontext = Feecontext;
        }
        public SmsEmailReportDTO getloaddata(SmsEmailReportDTO data)
        {
            try
            {
                data.yearlist = _Portalcontext.AcademicYearDMO.Where(s => s.MI_Id == data.MI_Id && s.Is_Active == true).Distinct().OrderByDescending(t => t.ASMAY_Order).ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public SmsEmailReportDTO getdata(SmsEmailReportDTO data)
        {
            try
            {
                string FromDate = ""; string ToDate = "";

                if (data.FromDate != null && data.ToDate != null)
                {
                    FromDate = data.FromDate.Value.ToString("yyyy-MM-dd");
                    ToDate = data.ToDate.Value.ToString("yyyy-MM-dd");
                }
                string Mobile_Number = "";
                if (data.Mobile_no !=null)
                {
                    Mobile_Number = data.Mobile_no;
                }
                using (var cmd = _Portalcontext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "EMAILSMS_HISTORY";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@startdate", SqlDbType.VarChar)
                    {
                        Value = FromDate
                    });
                    cmd.Parameters.Add(new SqlParameter("@enddate", SqlDbType.VarChar)
                    {
                        Value = ToDate
                    });
                    cmd.Parameters.Add(new SqlParameter("@optionflag", SqlDbType.VarChar)
                    {
                        Value = data.optionflag
                    });
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    //cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.BigInt)
                    //{
                    //    Value =data.ASMAY_Id
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
                                        dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead 
                                    );
                                }

                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.studlist = retObject.ToArray();
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
