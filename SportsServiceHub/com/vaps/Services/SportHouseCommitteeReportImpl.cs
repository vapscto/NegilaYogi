using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.Sports;
using DomainModel.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.com.vaps.Sports;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsServiceHub.com.vaps.Services
{
    public class SportHouseCommitteeReportImpl : Interfaces.SportHouseCommitteeReportInterface
    {
        private static ConcurrentDictionary<string, House_Committe_Report_DTO> _login =
 new ConcurrentDictionary<string, House_Committe_Report_DTO>();

        private readonly SportsContext _sportcontext;
        public SportHouseCommitteeReportImpl(SportsContext sportcontext)
        {
            _sportcontext = sportcontext;

        }


        public House_Committe_Report_DTO Getdetails(House_Committe_Report_DTO data)//int IVRMM_Id
        {

            try
            {
                data.asmay_list = _sportcontext.AcademicYear.Where(a => a.MI_Id == data.MI_Id && a.Is_Active).Distinct().OrderByDescending(t => t.ASMAY_Order).ToArray();


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;

        }

        public async Task<House_Committe_Report_DTO> showdetailsAsync(House_Committe_Report_DTO data)
        {
            #region
            //try
            //{

            //    dto.viewlist = (from a in _sportcontext.SportMasterHouseCommitteDMO
            //                    from b in _sportcontext.admissionStduent
            //                    from c in _sportcontext.SportMasterHouseDessignationDMO
            //                    from d in _sportcontext.SportMasterHouseDMO
            //                    from y in _sportcontext.admissionyearstudent
            //                    from cl in _sportcontext.admissionClass
            //                    from sc in _sportcontext.masterSection
            //                    from yr in _sportcontext.AcademicYear
            //                    where ( a.SPCCMHD_Id == c.SPCCMHD_Id && a.SPCCMH_Id == d.SPCCMH_Id && y.AMST_Id==a.AMST_Id  && y.AMST_Id==b.AMST_Id && y.ASMCL_Id==cl.ASMCL_Id && y.ASMS_Id==sc.ASMS_Id && y.ASMAY_Id==yr.ASMAY_Id && a.MI_Id == dto.MI_Id && y.ASMAY_Id==dto.ASMAY_Id && a.SPCCMH_Id == dto.SPCCMH_Id)
            //                    select new House_Committe_Report_DTO
            //                    {
            //                        SPCCMHC_Id = a.SPCCMHC_Id,
            //                        SPCCMHC_ContactNo = Convert.ToInt64(b.AMST_MobileNo),
            //                        SPCCMHC_EmailId = b.AMST_emailId,
            //                        SPCCMHD_DesignationName = c.SPCCMHD_DesignationName,
            //                        SPCCMH_HouseName = d.SPCCMH_HouseName,
            //                        studentname = b.AMST_FirstName + (string.IsNullOrEmpty(b.AMST_MiddleName) || b.AMST_MiddleName == "0" ? "" : ' ' + b.AMST_MiddleName) + (string.IsNullOrEmpty(b.AMST_LastName) || b.AMST_LastName == "0" ? "" : ' ' + b.AMST_LastName),
            //                        AMST_AdmNo=b.AMST_AdmNo,
            //                        ASMCL_ClassName=cl.ASMCL_ClassName,
            //                        ASMC_SectionName=sc.ASMC_SectionName,
            //                        ASMAY_Id=y.ASMAY_Id,
            //                        ASMAY_Year=yr.ASMAY_Year,
            //                    }).Distinct().ToArray();






            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex.Message);
            //}
            #endregion

            try
            {
                string hous_ids = "0";
                List<long> house_ids = new List<long>();
                foreach (var item in data.selectedhouselist)
                {
                    house_ids.Add(item.SPCCMH_Id);
                }
                for (int s = 0; s < house_ids.Count(); s++)
                {
                    hous_ids = hous_ids + ',' + house_ids[s].ToString();
                }

                using (var cmd = _sportcontext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "SPC_HouseCommitte_Reports";
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
                    cmd.Parameters.Add(new SqlParameter("@SPCCMH_Id",
                    SqlDbType.VarChar)
                    {
                        Value = hous_ids
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
                        data.viewlist = retObject.ToArray();
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
                return data;
            }
            return data;
        }



        public House_Committe_Report_DTO get_House(House_Committe_Report_DTO data)
        {
            try
            {
                data.houseList = (from t in _sportcontext.SportMasterHouseDMO
                                  from b in _sportcontext.SportStudentHouseDivisionDMO
                                  where (t.MI_Id == b.MI_Id && t.SPCCMH_Id == b.SPCCMH_Id && t.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && t.SPCCMH_ActiveFlag == true && b.SPCCMH_ActiveFlag == true)
                                  select t
                                  ).Distinct().OrderBy(t => t.SPCCMH_Id).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

    }
}
