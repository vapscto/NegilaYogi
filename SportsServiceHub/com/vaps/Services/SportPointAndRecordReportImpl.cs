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
using System.Linq;
using System.Threading.Tasks;

namespace SportsServiceHub.com.vaps.Services
{
    public class SportPointAndRecordReportImpl : Interfaces.SportPointAndRecordReportInterface
    {
        private static ConcurrentDictionary<string, SportStudentParticipationReportDTO> _login =
   new ConcurrentDictionary<string, SportStudentParticipationReportDTO>();

        private readonly SportsContext _sportcontext;
        public StudentAttendanceReportContext _db;
        ILogger<SportPointAndRecordReportImpl> _acdimpl;
        public SportPointAndRecordReportImpl(SportsContext sportcontext, StudentAttendanceReportContext db)
        {
            _sportcontext = sportcontext;
            _db = db;
        }


        public SportStudentParticipationReportDTO Getdetails(SportStudentParticipationReportDTO data)//int IVRMM_Id
        {

            try
            {
                List<MasterAcademic> list = new List<MasterAcademic>();
                list = _sportcontext.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_ActiveFlag == 1).OrderByDescending(t => t.ASMAY_Order).ToList();
                data.yearlist = list.ToArray();

                data.DivisionList = (from a in _sportcontext.SportMasterDivisionDMO
                                     where (a.MI_Id == data.MI_Id && a.SPCCMD_ActiveFlag == true)
                                     select new SportMasterHouseDTO
                                     {
                                         SPCCMD_Id = a.SPCCMD_Id,
                                         SPCCMD_DivisionName = a.SPCCMD_DivisionName,

                                     }).Distinct().ToArray();
                data.houseList = (from a in _sportcontext.SportMasterHouseDMO
                                  where (a.MI_Id == data.MI_Id && a.SPCCMH_ActiveFlag == true)
                                  select new SportMasterHouseDTO
                                  {
                                      SPCCMH_Id = a.SPCCMH_Id,
                                      SPCCMH_HouseName = a.SPCCMH_HouseName,

                                  }).Distinct().ToArray();
                data.eventList = _sportcontext.MasterEventsDMO.Where(d => d.MI_Id == data.MI_Id && d.SPCCME_ActiveFlag == true /*&& d.SPCCME_Flag == "others"*/).Select(d => new SportStudentParticipationReportDTO { SPCCME_Id = d.SPCCME_Id, SPCCME_EventName = d.SPCCME_EventName }).ToArray();

                data.events = (from a in _sportcontext.ProgramMasterDMO
                               from b in _sportcontext.EventsMappingDMO
                               where /*a.SPCCPM_Id == b.SPCCPM_Id &&*/ b.MI_Id == data.MI_Id && a.SPCCPM_ActiveFlag == true
                               select new SportStudentParticipationReportDTO
                               {
                                   SPCCE_Id = b.SPCCE_Id,
                                   SPCCPM_Id = a.SPCCPM_Id,
                                   SPCCPM_Name = a.SPCCPM_Name
                               }
                             ).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                _acdimpl.LogError(ex.Message);
                _acdimpl.LogDebug(ex.Message);
            }
            return data;

        }

        public SportStudentParticipationReportDTO showdetails(SportStudentParticipationReportDTO data)
        {

            try
            {
                List<SportStudentParticipationReportDTO> result = new List<SportStudentParticipationReportDTO>();

                string report_name = "sportspointsandrecordreport";

                using (var cmd = _sportcontext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Sports_Reports";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@mi_id", SqlDbType.Int) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@asmay_id", SqlDbType.Int) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@report_name", SqlDbType.VarChar) { Value = report_name });
                    cmd.Parameters.Add(new SqlParameter("@spccme_id", SqlDbType.Int) { Value = data.SPCCME_Id });
                    cmd.Parameters.Add(new SqlParameter("@spcce_id", SqlDbType.Int) { Value = data.SPCCE_Id });
                    cmd.Parameters.Add(new SqlParameter("@spccmh_id", SqlDbType.Int) { Value = data.SPCCMH_Id });
                    cmd.Parameters.Add(new SqlParameter("@spccmd_id", SqlDbType.Int) { Value = data.SPCCMD_Id });
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                result.Add(new SportStudentParticipationReportDTO
                                {
                                    adm_no = dataReader["AMST_AdmNo"].ToString(),
                                    studentname = dataReader["AMST_FirstName"].ToString(),
                                    SPCCESTR_Place = Convert.ToInt32(dataReader["SPCCESTR_Place"].ToString()),
                                    SPCCESTR_Points = Convert.ToDouble(dataReader["SPCCESTR_Points"].ToString()),
                                    SPCCMD_DivisionName = dataReader["SPCCMD_DivisionName"].ToString(),
                                    SPCCMH_HouseName = dataReader["SPCCMH_HouseName"].ToString()
                                });
                                data.viewlist = result.ToArray();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }


                //dto.viewlist = (from a in _sportcontext.admissionyearstudent
                //                from b in _sportcontext.admissionStduent
                //                from c in _sportcontext.admissionClass
                //                from d in _db.masterSection
                //                from e in _sportcontext.EventsMappingDMO
                //                from g in _sportcontext.MasterEventsDMO
                //                from h in _sportcontext.SportMasterHouseDMO
                //                from i in _sportcontext.SportStudentHouseDivisionDMO
                //                from j in _sportcontext.SportMasterDivisionDMO
                //                from k in _sportcontext.ProgramMasterDMO
                //                where (a.AMST_Id == b.AMST_Id && a.ASMCL_Id == c.ASMCL_Id && c.ASMCL_ActiveFlag == true && a.ASMAY_Id == dto.ASMAY_Id && a.AMAY_ActiveFlag == 1 && a.ASMS_Id == d.ASMS_Id && k.SPCCPM_Id == e.SPCCPM_Id && h.SPCCMH_Id == i.SPCCMH_Id && b.MI_Id == dto.MI_Id && i.ASMCL_Id == c.ASMCL_Id && i.ASMS_Id == d.ASMS_Id && i.AMST_Id == b.AMST_Id && g.SPCCME_Id == dto.SPCCME_Id && h.SPCCMH_Id == dto.SPCCMH_Id && i.SPCCMD_Id == j.SPCCMD_Id)
                //                select new SportStudentParticipationReportDTO
                //                {
                //                    AMST_Id = b.AMST_Id,
                //                    adm_no = b.AMST_AdmNo,
                //                    studentname = b.AMST_FirstName + (string.IsNullOrEmpty(b.AMST_MiddleName) || b.AMST_MiddleName == "0" ? "" : ' ' + b.AMST_MiddleName) + (string.IsNullOrEmpty(b.AMST_LastName) || b.AMST_LastName == "0" ? "" : ' ' + b.AMST_LastName),
                //                    classname = c.ASMCL_ClassName,
                //                    sectionname = d.ASMC_SectionName,
                //                    SPCCE_StartDate = e.SPCCE_StartDate,
                //                    SPCCMH_HouseName = h.SPCCMH_HouseName,
                //                    SPCCMD_DivisionName = j.SPCCMD_DivisionName,

                //                }).Distinct().ToArray();

            }
            catch (Exception ex)
            {
                _acdimpl.LogError(ex.Message);
                _acdimpl.LogDebug(ex.Message);
            }

            return data;
        }

    }
}
