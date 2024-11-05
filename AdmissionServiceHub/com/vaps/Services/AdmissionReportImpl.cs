using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.admission;
using DataAccessMsSqlServerProvider.com.vapstech.College.Fee;
using Microsoft.EntityFrameworkCore;
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
    public class AdmissionReportImpl : Interfaces.AdmissionReportInterface
    {
        public AdmissionRegisterContext _db;

        public AdmissionReportImpl(AdmissionRegisterContext db)
        {
            _db = db;
        }

        public AdmissionReportDTO getloaddata(AdmissionReportDTO data)
        {
            try
            {
                data.institutlistnew = (from a in _db.UserRoleWithInstituteDMO
                                        from b in _db.master_institution
                                        where (a.MI_Id == b.MI_Id)
                                        select new AdmissionReportDTO
                                        {
                                            MI_Id = b.MI_Id,
                                            MI_Name = b.MI_Name
                                        }).Distinct().ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }


        public async Task<AdmissionReportDTO> onreport(AdmissionReportDTO data)
        {
            try
            {
                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {

                    string MI_Id = "0";
                    List<long> mi_ids = new List<long>();
                    foreach (var item in data.institutlist)
                    {
                        mi_ids.Add(item.mI_Id);
                    }

                    for (int s = 0; s < mi_ids.Count(); s++)
                    {
                        MI_Id = MI_Id + ',' + mi_ids[s].ToString();
                    }
                    //string institution = "0";
                    //foreach (var a in data.institutlist)
                    //{
                    //    institution = institution + "," +   
                    //}
                    cmd.CommandText = "Admission_StudentsCount";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
             SqlDbType.VarChar)
                    {
                        Value = MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
             SqlDbType.VarChar)
                    {
                        Value = data.ASMAY_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@Fromdatee",
                 SqlDbType.VarChar)
                    {
                        Value = data.Fromdatee
                    });
                    cmd.Parameters.Add(new SqlParameter("@ToDate",
                 SqlDbType.VarChar)
                    {
                        Value = data.ToDate
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
                        data.get_Report = retObject.ToArray();
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
