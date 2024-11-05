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
    public class SportStudentParticipationReportImpl : Interfaces.SportStudentParticipationReportInterface
    {
        private static ConcurrentDictionary<string, StudentAgeCalcReport_DTO> _login =
     new ConcurrentDictionary<string, StudentAgeCalcReport_DTO>();

        private readonly SportsContext _sportcontext;
        // public StudentAttendanceReportContext _db;
        //// ILogger<SportStudentParticipationReportImpl> _acdimpl;
        public SportStudentParticipationReportImpl(SportsContext sportcontext/*, StudentAttendanceReportContext db*/)
        {
            _sportcontext = sportcontext;
            //_db = db;
        }


        public StudentAgeCalcReport_DTO Getdetails(StudentAgeCalcReport_DTO data)//int IVRMM_Id
        {

            try
            {

                var list = _sportcontext.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).Distinct().OrderByDescending(t => t.ASMAY_Order).ToList();
                data.yearlist = list.ToArray();


                //data.houseList = (from a in _sportcontext.SportMasterHouseDMO
                //                  where (a.MI_Id == data.MI_Id && a.SPCCMH_ActiveFlag == true)
                //                  select new StudentAgeCalcReport_DTO
                //                  {
                //                      SPCCMH_Id = a.SPCCMH_Id,
                //                      SPCCMH_HouseName = a.SPCCMH_HouseName
                //                  }).Distinct().ToArray();

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
                    cmd.CommandText = "Sports_House_Student_Age_Report";
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



        #region
        //public SportStudentParticipationReportDTO Getdetails(SportStudentParticipationReportDTO data)//int IVRMM_Id
        //{

        //    try
        //    {
        //        List<MasterAcademic> list = new List<MasterAcademic>();
        //        list = _sportcontext.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_ActiveFlag == 1).ToList();
        //        data.yearlist = list.ToArray();

        //        data.eventname = _sportcontext.MasterEventsDMO.Where(d => d.MI_Id == data.MI_Id && d.SPCCME_ActiveFlag == true /*&& d.SPCCME_Flag == "others"*/).Select(d => new SportStudentParticipationReportDTO { SPCCME_Id = d.SPCCME_Id, SPCCME_EventName = d.SPCCME_EventName }).ToArray();

        //    }
        //    catch (Exception ex)
        //    {
        //        _acdimpl.LogError(ex.Message);
        //        _acdimpl.LogDebug(ex.Message);
        //    }
        //    return data;

        //}
        //public SportStudentParticipationReportDTO showdetails(SportStudentParticipationReportDTO dto)
        //{

        //    //try
        //    //{


        //    //    if(dto.radiotype == "all")
        //    //    {
        //    //        dto.viewlist = (from a in _sportcontext.EventsStudentRecordDMO
        //    //                        from b in _sportcontext.admissionStduent
        //    //                        from c in _sportcontext.MasterEventsDMO
        //    //                        from d in _sportcontext.SportStudentHouseDivisionDMO
        //    //                        from e in _sportcontext.admissionClass
        //    //                        from f in _sportcontext.masterSection
        //    //                        from g in _sportcontext.EventsMappingDMO
        //    //                        from h in _sportcontext.SportMasterHouseDMO
        //    //                        where (a.AMST_Id == b.AMST_Id && c.SPCCME_Id == a.SPCCME_Id && d.AMST_Id == a.AMST_Id && a.ASMAY_Id == dto.ASMAY_Id && b.MI_Id == dto.MI_Id && a.ASMCL_Id == e.ASMCL_Id && a.ASMS_Id == f.ASMS_Id && a.SPCCE_Id == g.SPCCE_Id && d.SPCCMH_Id == h.SPCCMH_Id)
        //    //                        select new SportStudentParticipationReportDTO
        //    //                        {
        //    //                            AMST_Id = b.AMST_Id,
        //    //                            adm_no = b.AMST_AdmNo,
        //    //                            studentname = b.AMST_FirstName + (string.IsNullOrEmpty(b.AMST_MiddleName) || b.AMST_MiddleName == "0" ? "" : ' ' + b.AMST_MiddleName) + (string.IsNullOrEmpty(b.AMST_LastName) || b.AMST_LastName == "0" ? "" : ' ' + b.AMST_LastName),
        //    //                            classname = e.ASMCL_ClassName,
        //    //                            sectionname = f.ASMC_SectionName,
        //    //                            SPCCE_StartDate = g.SPCCE_StartDate,
        //    //                            SPCCME_EventName = c.SPCCME_EventName,
        //    //                            SPCCMH_HouseName = h.SPCCMH_HouseName

        //    //                        }).Distinct().ToArray();

        //    //    }
        //    //    else
        //    //    {

        //    //        List<long> amstids = new List<long>();
        //    //        foreach (SportStudentParticipationReportDTO item in dto.StudentList)
        //    //        {
        //    //            amstids.Add(item.AMST_Id);
        //    //        }
        //    //        dto.viewlist = (from a in _sportcontext.EventsStudentRecordDMO
        //    //                        from b in _sportcontext.admissionStduent
        //    //                        from c in _sportcontext.MasterEventsDMO
        //    //                        from d in _sportcontext.SportStudentHouseDivisionDMO
        //    //                        from e in _sportcontext.admissionClass
        //    //                        from f in _sportcontext.masterSection
        //    //                        from g in _sportcontext.EventsMappingDMO
        //    //                        from h in _sportcontext.SportMasterHouseDMO
        //    //                        where (a.AMST_Id == b.AMST_Id && c.SPCCME_Id == a.SPCCME_Id && d.AMST_Id == a.AMST_Id && a.ASMAY_Id == dto.ASMAY_Id && a.ASMS_Id == dto.ASMS_Id && b.MI_Id == dto.MI_Id && a.ASMCL_Id == dto.ASMCL_Id && amstids.Contains(a.AMST_Id) && a.ASMCL_Id == e.ASMCL_Id && a.ASMS_Id == f.ASMS_Id && a.SPCCE_Id == g.SPCCE_Id && d.SPCCMH_Id == h.SPCCMH_Id && a.SPCCME_Id == dto.SPCCME_Id)
        //    //                        select new SportStudentParticipationReportDTO
        //    //                        {
        //    //                            AMST_Id = b.AMST_Id,
        //    //                            adm_no = b.AMST_AdmNo,
        //    //                            studentname = b.AMST_FirstName + (string.IsNullOrEmpty(b.AMST_MiddleName) || b.AMST_MiddleName == "0" ? "" : ' ' + b.AMST_MiddleName) + (string.IsNullOrEmpty(b.AMST_LastName) || b.AMST_LastName == "0" ? "" : ' ' + b.AMST_LastName),
        //    //                            classname = e.ASMCL_ClassName,
        //    //                            sectionname = f.ASMC_SectionName,
        //    //                            SPCCE_StartDate = g.SPCCE_StartDate,
        //    //                            SPCCME_EventName = c.SPCCME_EventName,
        //    //                            SPCCMH_HouseName = h.SPCCMH_HouseName

        //    //                        }).Distinct().ToArray();

        //    //    }







        //    //}
        //    //catch (Exception ex)
        //    //{
        //    //    _acdimpl.LogError(ex.Message);
        //    //    _acdimpl.LogDebug(ex.Message);
        //    //}

        //    return dto;
        //}
        //public SportStudentParticipationReportDTO getevent(SportStudentParticipationReportDTO data)
        //{
        //    try
        //    {
        //        data.eventname = _sportcontext.MasterEventsDMO.Where(d => d.MI_Id == data.MI_Id && d.SPCCME_ActiveFlag == true /*&& d.SPCCME_Flag == data.radiotype*/).Select(d => new SportStudentParticipationReportDTO { SPCCME_Id = d.SPCCME_Id, SPCCME_EventName = d.SPCCME_EventName }).ToArray();

        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //    }
        //    return data;
        //}
        //public SportStudentParticipationReportDTO get_class(SportStudentParticipationReportDTO dto)
        //{
        //    try
        //    {
        //        dto.ClassList = (from a in _db.admissionyearstudent
        //                         from b in _db.admissionClass
        //                         where (a.ASMAY_Id == dto.ASMAY_Id && a.AMAY_ActiveFlag == 1 && a.ASMCL_Id == b.ASMCL_Id && b.ASMCL_ActiveFlag == true && b.MI_Id == dto.MI_Id)
        //                         select b).Distinct().ToArray();

        //    }
        //    catch (Exception ee)
        //    {
        //        Console.WriteLine(ee.Message);
        //    }
        //    return dto;

        //}
        //public SportStudentParticipationReportDTO get_section(SportStudentParticipationReportDTO dto)
        //{
        //    try
        //    {
        //        dto.SectionList = (from a in _db.admissionyearstudent
        //                           from b in _db.masterSection

        //                           where (a.ASMAY_Id == dto.ASMAY_Id && a.AMAY_ActiveFlag == 1 && a.ASMS_Id == b.ASMS_Id && b.ASMC_ActiveFlag == 1 && b.MI_Id == dto.MI_Id && a.ASMCL_Id == dto.ASMCL_Id)
        //                           select b).Distinct().ToArray();

        //    }
        //    catch (Exception ee)
        //    {
        //        Console.WriteLine(ee.Message);
        //    }
        //    return dto;

        //}
        //public SportStudentParticipationReportDTO get_student(SportStudentParticipationReportDTO dto)
        //{

        //    try
        //    {
        //        dto.StudentList1 = (from a in _sportcontext.admissionyearstudent
        //                           from b in _sportcontext.admissionStduent

        //                           where (a.AMST_Id == b.AMST_Id && a.ASMAY_Id == dto.ASMAY_Id && a.AMAY_ActiveFlag == 1 && a.ASMS_Id == dto.ASMS_Id && b.MI_Id == dto.MI_Id && a.ASMCL_Id == dto.ASMCL_Id)
        //                           select new SportStudentParticipationReportDTO.Ostudent
        //                           {
        //                               AMST_Id = b.AMST_Id,
        //                               studentname = b.AMST_FirstName + (string.IsNullOrEmpty(b.AMST_MiddleName) || b.AMST_MiddleName == "0" ? "" : ' ' + b.AMST_MiddleName) + (string.IsNullOrEmpty(b.AMST_LastName) || b.AMST_LastName == "0" ? "" : ' ' + b.AMST_LastName),

        //                           }
        //                           ).Distinct().OrderBy(b => b.studentname).ToArray();


        //    }
        //    catch (Exception ee)
        //    {
        //        Console.WriteLine(ee.Message);
        //    }
        //    return dto;

        //}

        #endregion
    }
}
