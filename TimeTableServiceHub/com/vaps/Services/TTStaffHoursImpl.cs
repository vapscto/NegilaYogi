using AutoMapper;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.TT;
using DomainModel.Model;
using DomainModel.Model.com.vaps.admission;
using DomainModel.Model.com.vapstech.admission;
using DomainModel.Model.com.vapstech.TT;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.TT;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using TimeTableServiceHub.com.vaps.Interfaces;

namespace TimeTableServiceHub.com.vaps.Services
{
    public class TTStaffHoursImpl : Interfaces.TTStaffHoursInterface
    {

        public TTContext _ttcontext;
        public DomainModelMsSqlServerContext _db;

        public TTStaffHoursImpl(TTContext ttcntx)
        {
            _ttcontext = ttcntx;
        }

   
        public TTStaffHoursDTO getdetails(int id)
        {
            TTStaffHoursDTO data = new TTStaffHoursDTO();
            try
            {
                data.acayear = _ttcontext.AcademicYear.AsNoTracking().Where(t => t.MI_Id == id && t.Is_Active == true).ToList().Distinct().OrderByDescending(rr=>rr.ASMAY_Order).ToArray();
                data.categorylist = _ttcontext.TTMasterCategoryDMO.AsNoTracking().Where(t => t.MI_Id == id && t.TTMC_ActiveFlag == true).ToList().Distinct().ToArray();
                data.stafflist = (from b in _ttcontext.HR_Master_Employee_DMO
                                  from a in _ttcontext.TT_Master_Staff_AbbreviationDMO
                                  from c in _ttcontext.TT_Final_Generation_DetailedDMO
                                  from d in _ttcontext.TT_Final_GenerationDMO
                                  where (b.MI_Id.Equals(id) && b.HRME_ActiveFlag.Equals(true) && a.HRME_Id == b.HRME_Id && a.TTMSAB_ActiveFlag == true && c.HRME_Id == a.HRME_Id && c.TTFG_Id == d.TTFG_Id && d.TTFG_ActiveFlag == true)
                                  select new TTStaffHoursDTO
                                  {
                                      HRME_Id = b.HRME_Id,
                                      staffName = b.HRME_EmployeeFirstName+" " + (b.HRME_EmployeeMiddleName == null || b.HRME_EmployeeMiddleName == " " || b.HRME_EmployeeMiddleName == "0" ? " " : b.HRME_EmployeeMiddleName) + " " + (b.HRME_EmployeeLastName == null || b.HRME_EmployeeLastName == " " || b.HRME_EmployeeLastName == "0" ? " " : b.HRME_EmployeeLastName),
                                  }).Distinct().OrderBy(i=>i.staffName).ToArray();
                data.classlist = _ttcontext.School_M_Class.Where(t => t.MI_Id == id && t.ASMCL_ActiveFlag == true).ToList().ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;

        }

        public TTStaffHoursDTO getreport(TTStaffHoursDTO data)
        {
            try
            {
                if (data.staffarray!=null)
                {
                    string stfid = "0";
                    foreach (var item in data.staffarray)
                    {
                        stfid = stfid + "," + item.HRME_Id;
                    }
                    using (var cmd = _ttcontext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "TT_StaffDay_WorkingHrs";
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
                        cmd.Parameters.Add(new SqlParameter("@HRME_Id",
                         SqlDbType.VarChar)
                        {
                            Value = stfid
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
                            data.gridweeks = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.Write(ex.Message);
                        }
                    }
                }
               
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }

    }
}
