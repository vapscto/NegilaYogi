using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.Sports;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.Sports;
using SportsServiceHub.com.vaps.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsServiceHub.com.vaps.Services
{
    public class BMIReportImpl : BMIReportInterface
    {
        DomainModelMsSqlServerContext _db;
        SportsContext _context;
        public BMIReportImpl(DomainModelMsSqlServerContext db, SportsContext context)
        {
            _db = db;
            _context = context;
        }


        public BMICalculationDTO getDetails(BMICalculationDTO data)
        {
            try
            {

                data.academicYear = _context.AcademicYear.Where(d => d.MI_Id == data.MI_Id && d.Is_Active == true).OrderByDescending(a => a.ASMAY_Order).ToArray();



                data.houseList = (from a in _context.SportMasterHouseDMO
                                  where (a.MI_Id == data.MI_Id && a.SPCCMH_ActiveFlag == true)
                                  select new House_Report_DTO
                                  {
                                      SPCCMH_Id = a.SPCCMH_Id,
                                      SPCCMH_HouseName = a.SPCCMH_HouseName
                                  }).Distinct().ToArray();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }

        public BMICalculationDTO getStudents(BMICalculationDTO data)
        {
            try
            {
                var student = (from m in _db.School_Adm_Y_StudentDMO
                               from n in _db.Adm_M_Student
                               where m.AMST_Id == n.AMST_Id && m.AMAY_ActiveFlag == 1 && n.AMST_ActiveFlag == 1 && m.ASMAY_Id == data.ASMAY_Id && n.MI_Id == data.MI_Id && n.AMST_SOL.Equals("S") && m.ASMCL_Id == data.ASMCL_Id && m.ASMS_Id == data.ASMS_Id
                               select new BMICalculationDTO
                               {
                                   AMST_Id = m.AMST_Id,
                                   studentName = n.AMST_FirstName + (string.IsNullOrEmpty(n.AMST_MiddleName) ? "" : ' ' + n.AMST_MiddleName) + (string.IsNullOrEmpty(n.AMST_LastName) ? "" : ' ' + n.AMST_LastName),
                               }).Distinct().OrderBy(n => n.studentName).ToList();
                if (student.Count > 0)
                {
                    data.studentList = student.ToArray();
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public BMICalculationDTO get_section(BMICalculationDTO data)
        {
            try
            {
                data.sectionList = (from a in _db.School_Adm_Y_StudentDMO
                                    from b in _db.School_M_Section
                                    where (a.ASMAY_Id == data.ASMAY_Id && a.AMAY_ActiveFlag == 1 && b.ASMC_ActiveFlag == 1 && b.MI_Id == data.MI_Id && a.ASMCL_Id == data.ASMCL_Id)
                                    select b).Distinct().ToArray();


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public BMICalculationDTO get_class(BMICalculationDTO dto)
        {
            try
            {
                dto.classList = (from a in _context.admissionyearstudent
                                 from b in _context.admissionClass
                                 where (a.ASMAY_Id == dto.ASMAY_Id && a.AMAY_ActiveFlag == 1 && a.ASMCL_Id == b.ASMCL_Id && b.ASMCL_ActiveFlag == true && b.MI_Id == dto.MI_Id)
                                 select b).Distinct().OrderBy(t => t.ASMCL_Id).ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;

        }

        public async Task<BMICalculationDTO> report(BMICalculationDTO data)
        {


            try
            {
                string section_idss = "0";

                List<long> section_ids = new List<long>();

                foreach (var item in data.selectedSectionlist)
                {

                    section_ids.Add(item.ASMS_Id);
                }
                for (int s = 0; s < section_ids.Count(); s++)
                {
                    section_idss = section_idss + ',' + section_ids[s].ToString();
                }
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Sports_age_BMIcalc_Report";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                SqlDbType.BigInt)
                    {
                        Value = data.ASMCL_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id",
                   SqlDbType.VarChar)
                    {
                        Value = section_idss
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                     SqlDbType.BigInt)
                    {
                        Value = data.ASMAY_Id
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
            }





            return data;
        }



    }
}
