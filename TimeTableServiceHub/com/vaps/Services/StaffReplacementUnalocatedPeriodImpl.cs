using AutoMapper;
using DataAccessMsSqlServerProvider.com.vapstech.TT;
using DomainModel.Model;
using DomainModel.Model.com.vaps.admission;
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
namespace TimeTableServiceHub.com.vaps.Services
{
    public class StaffReplacementUnalocatedPeriodImpl : Interfaces.StaffReplacementUnalocatedPeriodInterface
    {

        private static ConcurrentDictionary<string, StaffReplacementUnalocatedPeriodDTO> _login =
               new ConcurrentDictionary<string, StaffReplacementUnalocatedPeriodDTO>();


        public TTContext _ttcategorycontext;
        public StaffReplacementUnalocatedPeriodImpl(TTContext ttcategory)
        {
            _ttcategorycontext = ttcategory;
        }
        public StaffReplacementUnalocatedPeriodDTO getdetails(int id)
        {
            StaffReplacementUnalocatedPeriodDTO TTMC = new StaffReplacementUnalocatedPeriodDTO();
            try
            {
                List<AcademicYear> acad = new List<AcademicYear>();
                acad = _ttcategorycontext.AcademicYear.Where(t => t.MI_Id.Equals(id) && t.Is_Active==true).ToList();
                TTMC.academiclist = acad.ToArray();

                List<TTMasterCategoryDMO> mcat = new List<TTMasterCategoryDMO>();
                mcat = _ttcategorycontext.TTMasterCategoryDMO.Where(t => t.MI_Id.Equals(id) && t.TTMC_ActiveFlag.Equals(true)).ToList();
                TTMC.catelist = mcat.ToArray();

                TTMC.staffDrpDwn = (from e in _ttcategorycontext.HR_Master_Employee_DMO
                                    where (e.MI_Id.Equals(id) && e.HRME_ActiveFlag.Equals(true))
                                    select new StaffReplacementUnalocatedPeriodDTO
                                    {
                                        HRME_Id = e.HRME_Id,
                                        staffNamelst = e.HRME_EmployeeFirstName + " " + (e.HRME_EmployeeMiddleName == null || e.HRME_EmployeeMiddleName == " " || e.HRME_EmployeeMiddleName == "0" ? " " : e.HRME_EmployeeMiddleName) + " " + (e.HRME_EmployeeLastName == null || e.HRME_EmployeeLastName == " " || e.HRME_EmployeeLastName == "0" ? " " : e.HRME_EmployeeLastName),
                                    }
                                    ).ToArray();

                List<TT_Master_PeriodDMO> allperiods = new List<TT_Master_PeriodDMO>();
                allperiods = _ttcategorycontext.TT_Master_PeriodDMO.Where(c => c.TTMP_ActiveFlag.Equals(true) && c.MI_Id.Equals(id)).ToList();
                TTMC.periodslst = allperiods.ToArray();

            }
            catch (Exception ee)
            {
                TTMC.returnval = false;
            }
            return TTMC;

        }

        public StaffReplacementUnalocatedPeriodDTO get_catg(StaffReplacementUnalocatedPeriodDTO data)
        {
            try
            {
                data.catelist = (from a in _ttcategorycontext.TTMasterCategoryDMO
                                 where (a.MI_Id.Equals(data.MI_Id) && a.TTMC_ActiveFlag.Equals(true))
                                 select new StaffReplacementUnalocatedPeriodDTO
                                 {
                                     TTMC_Id = a.TTMC_Id,
                                     TTMC_CategoryName = a.TTMC_CategoryName,
                                 }
          ).Distinct().ToArray();


            }
            catch (Exception ee)
            {
                data.returnval = false;
            }
            return data;

        }

       public async Task<StaffReplacementUnalocatedPeriodDTO> getrpt(StaffReplacementUnalocatedPeriodDTO data)
        {
            try
            {
                List<TT_Master_PeriodDMO> allperiods = new List<TT_Master_PeriodDMO>();
                allperiods = _ttcategorycontext.TT_Master_PeriodDMO.Where(c => c.TTMP_ActiveFlag.Equals(true) && c.MI_Id.Equals(data.MI_Id)).ToList();
                data.periodslst = allperiods.ToArray();

                using (var cmd = _ttcategorycontext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TT_Getstaff_report";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@staffid",
                        SqlDbType.BigInt)
                    {
                        Value = data.HRME_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@mi_id",
                       SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@Asmay_id",
                      SqlDbType.BigInt)
                    {
                        Value = data.ASMAY_Id
                    });
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();

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
                    data.datalst = retObject.ToArray();
                }
            }
            catch (Exception ee)
            {
                data.returnval = false;
            }
            return data;

        }


    }
}
