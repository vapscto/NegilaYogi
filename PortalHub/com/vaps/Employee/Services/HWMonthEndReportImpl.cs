using DataAccessMsSqlServerProvider.com.vapstech.Portals;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.admission;
using DomainModel.Model.com.vapstech.LeaveManagement;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.Portals.Employee;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace PortalHub.com.vaps.Employee.Services
{
   public class HWMonthEndReportImpl : Interfaces.HWMonthEndReportInterface


    {

        public PortalContext _portalContext;
        //   public AdmissionFormContext _AdmissionFormContext;
        public HWMonthEndReportImpl(PortalContext frgContext)
        {
            _portalContext = frgContext;
            //   _AdmissionFormContext = _admc;
        }

        public HomeWorkUploadDTO getdata123(HomeWorkUploadDTO data)
        {

            try
            {
                List<AcademicYear> year = new List<AcademicYear>();
                year = _portalContext.AcademicYearDMO.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).OrderByDescending(t => t.ASMAY_Order).ToList();
                data.acayear = year.ToArray();


                List<IVRM_Month_DMO> mnth = new List<IVRM_Month_DMO>();
                mnth = _portalContext.IVRM_Month_DMO.Where(t => t.Is_Active == true).ToList();
                data.Month_array = mnth.ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        public async Task<HomeWorkUploadDTO> getreport(HomeWorkUploadDTO data)
        {
            List<HomeWorkUploadDTO> AllInOne = new List<HomeWorkUploadDTO>();
            try
            {
               

                //AllInOne.Add(temp);
               

                using (var cmd = _portalContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "HWmonthend_report";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@Year",
                       SqlDbType.VarChar)
                    {
                        Value = data.year
                    });
                    cmd.Parameters.Add(new SqlParameter("@month",
                  SqlDbType.VarChar)
                    {
                        Value = data.month
                    });
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
                    cmd.Parameters.Add(new SqlParameter("@flag",
                SqlDbType.VarChar)
                    {
                        Value = data.type
                    });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();
                    //var data = cmd.ExecuteNonQuery();
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
                        data.reportlist = retObject.ToArray();

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                return data;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

    
    }
}
