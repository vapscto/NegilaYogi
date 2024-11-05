using DataAccessMsSqlServerProvider;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.COE;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace COEServiceHub.com.vaps.Services
{
    public class CoeReportGraphIMPL : Interfaces.CoeReportGraphInterface
    {
        public DomainModelMsSqlServerContext _db;
       
        public CoeReportGraphIMPL(DomainModelMsSqlServerContext db)
        {
            _db = db;
          

        }
        public async Task<COEReportDTO> getinitialData(int id)
        {
            COEReportDTO dto = new COEReportDTO();
            try
            {
                dto.fillyear = _db.AcademicYear.Where(R => R.MI_Id == id).Distinct().ToArray();
               
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return dto;
        }
        public COEReportDTO getReport(COEReportDTO data)
        {
            try
            {
                List<long> ASMAY_Idd = new List<long>();


                string ASMAY_Id = "0";
                if (data.ASMAY_IdList != null)
                {
                    foreach (var g in data.ASMAY_IdList)
                    {
                        ASMAY_Id = ASMAY_Id + "," + g.ASMAY_Id;
                        ASMAY_Idd.Add(g.ASMAY_Id);
                    }
                }
                else
                {
                    ASMAY_Idd.Add(data.ASMAY_Id);
                }
                data.fillyear = _db.AcademicYear.Where(R => R.MI_Id == data.MI_Id && ASMAY_Idd.Contains(R.ASMAY_Id)).Distinct().ToArray();
                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "COE_YearwiseCOEcount";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_ID", SqlDbType.VarChar)
                    {

                        Value = ASMAY_Id
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
                            data.coereport = retObject.Distinct().ToArray();
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }

                }


                data.fillmonth = _db.AcademicYear.Where(R => R.MI_Id == data.MI_Id).Distinct().ToArray();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }

        public COEReportDTO mothreport(COEReportDTO data)
        {
            try
            {
               
               

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
    }
}
