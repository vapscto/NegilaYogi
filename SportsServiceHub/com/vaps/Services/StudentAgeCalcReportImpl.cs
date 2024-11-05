using DataAccessMsSqlServerProvider.com.vapstech.Sports;
using Microsoft.EntityFrameworkCore;
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
    public class StudentAgeCalcReportImpl : Interfaces.StudentAgeCalcReportInterface
    {

        private static ConcurrentDictionary<string, StudentAgeCalcReport_DTO> _login = new ConcurrentDictionary<string, StudentAgeCalcReport_DTO>();

        private readonly SportsContext _sportcontext;

        public StudentAgeCalcReportImpl(SportsContext sportcontext)
        {
            _sportcontext = sportcontext;

        }


        public StudentAgeCalcReport_DTO Getdetails(StudentAgeCalcReport_DTO data)//int IVRMM_Id
        {

            try
            {

                var list = _sportcontext.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).OrderByDescending(t => t.ASMAY_Order).ToList();
                data.yearlist = list.ToArray();


                //data.houseList = (from a in _sportcontext.SportMasterHouseDMO
                //                  where (a.MI_Id == data.MI_Id && a.SPCCMH_ActiveFlag == true)
                //                  select new StudentAgeCalcReport_DTO
                //                  {
                //                      SPCCMH_Id = a.SPCCMH_Id,
                //                      SPCCMH_HouseName = a.SPCCMH_HouseName
                //                  }).Distinct().ToArray();

                data.categoryList = (from a in _sportcontext.MasterCompitionCategoryDMO
                                     where (a.MI_Id == data.MI_Id && a.SPCCMCC_ActiveFlag == true)
                                     select new EventsStudentRecordDTO
                                     {
                                         SPCCMCC_Id = a.SPCCMCC_Id,
                                         SPCCMCC_CompitionCategory = a.SPCCMCC_CompitionCategory,
                                     }).Distinct().ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;

        }

        public StudentAgeCalcReport_DTO get_class(StudentAgeCalcReport_DTO dto)
        {
            try
            {
                dto.classList = (from a in _sportcontext.admissionyearstudent
                                 from b in _sportcontext.admissionClass
                                 where (a.ASMAY_Id == dto.ASMAY_Id && a.AMAY_ActiveFlag == 1 && a.ASMCL_Id == b.ASMCL_Id && b.ASMCL_ActiveFlag == true && b.MI_Id == dto.MI_Id)
                                 select b).Distinct().OrderBy(t => t.ASMCL_Id).ToArray();

                dto.houseList = (from t in _sportcontext.SportMasterHouseDMO
                                 from b in _sportcontext.SportStudentHouseDivisionDMO
                                 where (t.MI_Id == b.MI_Id && t.SPCCMH_Id == b.SPCCMH_Id && t.MI_Id == dto.MI_Id && b.ASMAY_Id == dto.ASMAY_Id && t.SPCCMH_ActiveFlag == true && b.SPCCMH_ActiveFlag == true)
                                 select t
                                 ).Distinct().OrderBy(t => t.SPCCMH_Id).ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;

        }

        public StudentAgeCalcReport_DTO get_section(StudentAgeCalcReport_DTO dto)
        {
            try
            {
                dto.sectionList = (from a in _sportcontext.admissionyearstudent
                                   from b in _sportcontext.masterSection

                                   where (a.ASMAY_Id == dto.ASMAY_Id && a.AMAY_ActiveFlag == 1 && a.ASMS_Id == b.ASMS_Id && b.ASMC_ActiveFlag == 1 && b.MI_Id == dto.MI_Id && a.ASMCL_Id == dto.ASMCL_Id)
                                   select b).Distinct().OrderBy(t => t.ASMS_Id).ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;

        }

        public async Task<StudentAgeCalcReport_DTO> showdetails(StudentAgeCalcReport_DTO data)
        {
            try
            {
                string hous_idss = "0";
                string section_idss = "0";

                List<long> house_ids = new List<long>();
                List<long> section_ids = new List<long>();

                if (data.Type == "CS")
                {
                    foreach (var item in data.selectedhouselist)
                    {
                        house_ids.Add(item.SPCCMH_Id);
                    }
                    foreach (var item in data.selectedSectionlist)
                    {
                        section_ids.Add(item.ASMS_Id);
                    }


                    for (int s = 0; s < house_ids.Count(); s++)
                    {
                        hous_idss = hous_idss + ',' + house_ids[s].ToString();
                    }

                    for (int s = 0; s < section_ids.Count(); s++)
                    {
                        section_idss = section_idss + ',' + section_ids[s].ToString();
                    }

                }
                else
                {
                    foreach (var item in data.selectedhouselist)
                    {
                        house_ids.Add(item.SPCCMH_Id);

                    }
                    for (int s = 0; s < house_ids.Count(); s++)
                    {
                        hous_idss = hous_idss + ',' + house_ids[s].ToString();
                    }
                }





                using (var cmd = _sportcontext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "SPC_Age_Category_Wise_Student_Report";
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
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                 SqlDbType.BigInt)
                    {
                        Value = data.ASMCL_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@Type",
                    SqlDbType.VarChar)
                    {
                        Value = data.Type
                    });
                    cmd.Parameters.Add(new SqlParameter("@SPCCMH_Id",
                   SqlDbType.VarChar)
                    {
                        Value = hous_idss
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id",
                     SqlDbType.VarChar)
                    {
                        Value = section_idss
                    });
                    //cmd.Parameters.Add(new SqlParameter("@SPCCMCC_Id",
                    //SqlDbType.VarChar)
                    //{
                    //    Value = Convert.ToString(data.SPCCMCC_Id)
                    //});
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
            }

            return data;
        }



    }
}
