using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.Sports;
using DomainModel.Model;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.Sports;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsServiceHub.com.vaps.Services
{
    public class HouseInchargeReportImpl : Interfaces.HouseInchargeReportInterface
    {

        DomainModelMsSqlServerContext _db;
        SportsContext _sportcontext;

        public HouseInchargeReportImpl(SportsContext spc, DomainModelMsSqlServerContext contxt)
        {
            _sportcontext = spc;
            _db = contxt;
        }

        public HouseInchargeReport_DTO get_details(HouseInchargeReport_DTO data)
        {
            try
            {
                List<MasterAcademic> list = new List<MasterAcademic>();
                list = _sportcontext.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).Distinct().OrderByDescending(t => t.ASMAY_Order).ToList();
                data.yearlist = list.ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public HouseInchargeReport_DTO get_house(HouseInchargeReport_DTO data)
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

        public async Task<HouseInchargeReport_DTO> get_reports(HouseInchargeReport_DTO data)
        {
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
                    cmd.CommandText = "SPC_HouseInCharge_Reports";
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
                        data.report_list = retObject.ToArray();
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



    }
}
