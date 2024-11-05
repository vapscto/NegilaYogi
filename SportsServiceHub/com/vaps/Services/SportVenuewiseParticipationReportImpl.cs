using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.Sports;
using DomainModel.Model;
using DomainModel.Model.com.vaps.admission;
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
    public class SportVenuewiseParticipationReportImpl : Interfaces.SportVenuewiseParticipationReportInterface
    {
        private static ConcurrentDictionary<string, SportStudentParticipationReportDTO> _login =
    new ConcurrentDictionary<string, SportStudentParticipationReportDTO>();

        private readonly SportsContext _sportcontext;
        public StudentAttendanceReportContext _db;
        ILogger<SportVenuewiseParticipationReportImpl> _acdimpl;
        public SportVenuewiseParticipationReportImpl(SportsContext sportcontext, StudentAttendanceReportContext db)
        {
            _sportcontext = sportcontext;
            _db = db;
        }


        public SportStudentParticipationReportDTO Getdetails(SportStudentParticipationReportDTO data)//int IVRMM_Id
        {

            try
            {
                List<MasterAcademic> list = new List<MasterAcademic>();
                list = _sportcontext.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).Distinct().OrderByDescending(t => t.ASMAY_Order).ToList();
                data.yearlist = list.ToArray();

                ///data.houseList = _sportcontext.SportMasterHouseDMO.Where(t => t.MI_Id == data.MI_Id).Distinct().OrderBy(t => t.SPCCMH_Id).ToArray();

                //data.venueList = (from a in _sportcontext.MasterEventVenueDMO
                //                  where (a.MI_Id == data.MI_Id && a.SPCCMEV_ActiveFlag == true)
                //                     select new SportStudentParticipationReportDTO
                //                     {
                //                         SPCCMEV_Id = a.SPCCMEV_Id,
                //                         SPCCMEV_EventVenue = a.SPCCMEV_EventVenue,

                //                     }).Distinct().OrderBy(t=>t.SPCCMEV_EventVenue).ToArray();

                ////data.eventList = (from a in _sportcontext.MasterEventsDMO
                //                  from b in _sportcontext.EventsMappingDMO
                //                  where (a.MI_Id == b.MI_Id && a.SPCCME_Id == b.SPCCME_Id && a.MI_Id == data.MI_Id && a.SPCCME_ActiveFlag == true && b.SPCCE_ActiveFlag == true)
                //                  select a).Distinct().OrderBy(t => t.SPCCME_Id).ToArray();


            }
            catch (Exception ex)
            {
                _acdimpl.LogError(ex.Message);
                _acdimpl.LogDebug(ex.Message);
            }
            return data;

        }


        public SportStudentParticipationReportDTO get_class(SportStudentParticipationReportDTO dto)
        {
            try
            {
                dto.ClassList = (from a in _db.admissionyearstudent
                                 from b in _db.admissionClass
                                 where (a.ASMAY_Id == dto.ASMAY_Id && a.AMAY_ActiveFlag == 1 && b.ASMCL_ActiveFlag == true && b.MI_Id == dto.MI_Id)
                                 select b).Distinct().ToArray();
                dto.houseList = (from a in _sportcontext.SportMasterHouseDMO
                                 from b in _sportcontext.SportStudentHouseDivisionDMO
                                 where (a.MI_Id == b.MI_Id && a.SPCCMH_Id == b.SPCCMH_Id && a.MI_Id == dto.MI_Id && b.ASMAY_Id == dto.ASMAY_Id)
                                 select a).Distinct().OrderBy(t => t.SPCCMH_Id).ToArray();

                dto.venueList = (from a in _sportcontext.MasterEventVenueDMO
                                 from b in _sportcontext.EventsMappingDMO
                                 where (a.MI_Id == b.MI_Id && a.SPCCMEV_Id == b.SPCCMEV_Id && a.MI_Id == dto.MI_Id && b.ASMAY_Id == dto.ASMAY_Id && a.SPCCMEV_ActiveFlag == true)
                                 select new SportStudentParticipationReportDTO
                                 {
                                     SPCCMEV_Id = a.SPCCMEV_Id,
                                     SPCCMEV_EventVenue = a.SPCCMEV_EventVenue,

                                 }).Distinct().OrderBy(t => t.SPCCMEV_EventVenue).ToArray();


            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;

        }
        public SportStudentParticipationReportDTO get_section(SportStudentParticipationReportDTO dto)
        {
            try
            {
                dto.SectionList = (from a in _db.admissionyearstudent
                                   from b in _db.masterSection
                                   where (a.ASMAY_Id == dto.ASMAY_Id && a.AMAY_ActiveFlag == 1 && b.ASMC_ActiveFlag == 1 && b.MI_Id == dto.MI_Id && a.ASMCL_Id == dto.ASMCL_Id)
                                   select b).Distinct().ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;

        }
        public SportStudentParticipationReportDTO get_student(SportStudentParticipationReportDTO dto)
        {

            try
            {
                dto.StudentList1 = (from a in _sportcontext.admissionyearstudent
                                    from b in _sportcontext.admissionStduent
                                    where (a.AMST_Id == b.AMST_Id && a.ASMAY_Id == dto.ASMAY_Id && a.AMAY_ActiveFlag == 1 && a.ASMS_Id == dto.ASMS_Id && b.MI_Id == dto.MI_Id && a.ASMCL_Id == dto.ASMCL_Id)
                                    select new SportStudentParticipationReportDTO.Ostudent
                                    {
                                        AMST_Id = b.AMST_Id,
                                        studentname = b.AMST_FirstName + (string.IsNullOrEmpty(b.AMST_MiddleName) || b.AMST_MiddleName == "0" ? "" : ' ' + b.AMST_MiddleName) + (string.IsNullOrEmpty(b.AMST_LastName) || b.AMST_LastName == "0" ? "" : ' ' + b.AMST_LastName),

                                    }
                                   ).Distinct().OrderBy(b => b.studentname).ToArray();


            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;

        }

        public async Task<SportStudentParticipationReportDTO> showdetails(SportStudentParticipationReportDTO data)
        {

            try
            {
                string hous_ids = "0";
                string section_idss = "0";
                string venues_idss = "0";

                List<long> house_ids = new List<long>();
                List<long> section_ids = new List<long>();
                List<long> venue_ids = new List<long>();

                if (data.Type == "CS")
                {
                    foreach (var item in data.selectedhouselist)
                    {
                        house_ids.Add(item.SPCCMH_Id);
                    }
                    for (int s = 0; s < house_ids.Count(); s++)
                    {
                        hous_ids = hous_ids + ',' + house_ids[s].ToString();
                    }

                    foreach (var item in data.selectedSectionlist)
                    {
                        section_ids.Add(item.ASMS_Id);
                    }
                    for (int s = 0; s < section_ids.Count(); s++)
                    {
                        section_idss = section_idss + ',' + section_ids[s].ToString();
                    }

                    foreach (var item in data.selectedVenuelist)
                    {
                        venue_ids.Add(item.SPCCMEV_Id);
                    }
                    for (int s = 0; s < venue_ids.Count(); s++)
                    {
                        venues_idss = venues_idss + ',' + venue_ids[s].ToString();
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
                        hous_ids = hous_ids + ',' + house_ids[s].ToString();
                    }

                    foreach (var item in data.selectedVenuelist)
                    {
                        venue_ids.Add(item.SPCCMEV_Id);
                    }
                    for (int s = 0; s < venue_ids.Count(); s++)
                    {
                        venues_idss = venues_idss + ',' + venue_ids[s].ToString();
                    }


                }

                using (var cmd = _sportcontext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Sports_VenuewiseParticipation_Report";
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
                    cmd.Parameters.Add(new SqlParameter("@Type",
                    SqlDbType.VarChar)
                    {
                        Value = data.Type
                    });
                    cmd.Parameters.Add(new SqlParameter("@SPCCMH_Id",
                   SqlDbType.VarChar)
                    {
                        Value = hous_ids
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
                    cmd.Parameters.Add(new SqlParameter("@SPCCMEV_Id",
                   SqlDbType.VarChar)
                    {
                        Value = venues_idss
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
