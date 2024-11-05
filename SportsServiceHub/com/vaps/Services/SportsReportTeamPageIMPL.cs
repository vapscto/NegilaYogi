using DataAccessMsSqlServerProvider.com.vapstech.Sports;
using DomainModel.Model;
using DomainModel.Model.com.vaps.admission;
using DomainModel.Model.com.vapstech.Sports;
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
    public class SportsReportTeamPageIMPL : Interfaces.SportsReportTeamPageInterface
    {
        private static ConcurrentDictionary<string, SportsReportTeamPageDto> _login = new ConcurrentDictionary<string, SportsReportTeamPageDto>();

        private readonly SportsContext _sportcontext;

        public SportsReportTeamPageIMPL(SportsContext sportcontext)
        {
            _sportcontext = sportcontext;

        } 


        public SportsReportTeamPageDto saveRecord(SportsReportTeamPageDto data)
        {

            try
            {
                if (data.SPCCETM_Id == 0)
                {
                    var checkduplicate = _sportcontext.EventsTeamDMO.Where(d => d.MI_Id == data.MI_Id && d.ASMAY_Id == data.ASMAY_Id && d.SPCCETM_TeamName == data.SPCCETM_TeamName && d.SPCCE_Id == data.SPCCE_Id && d.SPCCMCL_Id == data.SPCCMCL_Id && d.SPCCMCC_Id == data.SPCCMCC_Id && d.SPCCMSCC_Id == data.SPCCMSCC_Id).ToList();

                    if (checkduplicate.Count() > 0)
                    {
                        data.msg = "duplicate";
                    }
                    else
                    {
                        EventsTeamDMO mapp = new EventsTeamDMO();

                        mapp.MI_Id = data.MI_Id;
                        mapp.ASMAY_Id = data.ASMAY_Id;
                        mapp.SPCCE_Id = data.SPCCE_Id;
                        mapp.SPCCMCL_Id = data.SPCCMCL_Id;
                        mapp.SPCCMCC_Id = data.SPCCMCC_Id;
                        mapp.SPCCMSCC_Id = data.SPCCMSCC_Id;
                        mapp.SPCCETM_TeamName = data.SPCCETM_TeamName;
                        mapp.SPCCETM_NoOfParticipants = data.SPCCETM_NoOfParticipants;
                        mapp.SPCCETM_ActiveFlag = true;
                        mapp.SPCCETM_CreatedDate = DateTime.Now;
                        mapp.SPCCETM_UpdatedDate = DateTime.Now;
                        _sportcontext.Add(mapp);

                        for (int i = 0; i < data.TeamList.Length; i++)
                        {

                            EventsTeamStudentsDMO mapp2 = new EventsTeamStudentsDMO();
                            mapp2.SPCCETM_Id = mapp.SPCCETM_Id;
                            mapp2.SPCCETMSTD_ActiveFlag = true;
                            mapp2.SPCCETMSTD_CreatedDate = DateTime.Now;
                            mapp2.SPCCETMSTD_UpdatedDate = DateTime.Now;
                            mapp2.AMST_Id = data.TeamList[i].AMST_Id;
                            _sportcontext.Add(mapp2);
                        }
                        int s = _sportcontext.SaveChanges();
                        if (s > 0)
                        {
                            data.msg = "saved";
                        }
                        else
                        {
                            data.msg = "savingFailed";
                        }
                    }

                }
                else if (data.SPCCETM_Id > 0)
                {
                    var checkduplicate = _sportcontext.EventsTeamDMO.
                        Where(d => d.MI_Id == data.MI_Id && d.ASMAY_Id == data.ASMAY_Id && d.SPCCE_Id != data.SPCCE_Id
                        && d.SPCCETM_Id == data.SPCCETM_Id && d.SPCCMCL_Id == data.SPCCMCL_Id && d.SPCCMCC_Id == data.SPCCMCC_Id
                        && d.SPCCMSCC_Id == data.SPCCMSCC_Id && d.SPCCETM_TeamName ==data.SPCCETM_TeamName).ToList();
                    if (checkduplicate.Count > 0)
                    {
                        data.msg = "duplicate";
                    }
                    else
                    {

                        var update = _sportcontext.EventsTeamDMO.Single(d => d.SPCCETM_Id == data.SPCCETM_Id && d.MI_Id == data.MI_Id);                       
                        update.ASMAY_Id = data.ASMAY_Id;
                        update.SPCCE_Id = data.SPCCE_Id;
                        update.SPCCMCL_Id = data.SPCCMCL_Id;
                        update.SPCCMCC_Id = data.SPCCMCC_Id;
                        update.SPCCMSCC_Id = data.SPCCMSCC_Id;
                        update.SPCCETM_TeamName = data.SPCCETM_TeamName;
                        update.SPCCETM_NoOfParticipants = data.SPCCETM_NoOfParticipants;
                        update.SPCCETM_ActiveFlag = true;
                        update.SPCCETM_CreatedDate = DateTime.Now;
                        update.SPCCETM_UpdatedDate = DateTime.Now;
                        _sportcontext.Update(update);
                        if (data.TeamList !=null && data.TeamList.Length > 0)
                        {
                            var studentData = _sportcontext.EventsTeamStudentsDMO.Where(R => R.SPCCETM_Id == data.SPCCETM_Id).ToList();
                            if(studentData !=null && studentData.Count > 0)
                            {
                                foreach(var d in studentData)
                                {
                                    _sportcontext.Remove(d);
                                }
                            }
                            for (int i = 0; i < data.TeamList.Length; i++)
                            {

                                EventsTeamStudentsDMO mapp2 = new EventsTeamStudentsDMO();
                                mapp2.SPCCETM_Id = update.SPCCETM_Id;
                                mapp2.SPCCETMSTD_ActiveFlag = true;
                                mapp2.SPCCETMSTD_CreatedDate = DateTime.Now;
                                mapp2.SPCCETMSTD_UpdatedDate = DateTime.Now;
                                mapp2.AMST_Id = data.TeamList[i].AMST_Id;
                                _sportcontext.Add(mapp2);

                            }

                        }

                        
                        int s = _sportcontext.SaveChanges();
                        if (s > 0)
                        {
                            data.msg = "updated";
                        }
                        else
                        {
                            data.msg = "updateFailed";
                        }

                    }
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }

        //SaveRecords
        public SportsReportTeamPageDto SaveRecords(SportsReportTeamPageDto data)
        {
            try
            {
                if (data.SPCCETMSCH_Id == 0)
                {
                    var checkduplicate = _sportcontext.EventsTeamScheduleDMO.Where(d => d.MI_Id == data.MI_Id && d.SPCCETMSCH_Team1 == data.SPCCETMSCH_Team1 && d.SPCCETMSCH_Team2 == data.SPCCETMSCH_Team2 && d.SPCCETMSCH_Date == data.SPCCETMSCH_Date && d.SPCCETMSCH_Time == data.SPCCETMSCH_Time).ToList();

                    if (checkduplicate.Count() > 0)
                    {
                        data.msg = "duplicate";
                    }
                    else
                    {
                        EventsTeamScheduleDMO mapp = new EventsTeamScheduleDMO();

                        mapp.MI_Id = data.MI_Id;                     
                        mapp.SPCCETMSCH_Team1 = data.SPCCETMSCH_Team1;
                        mapp.SPCCETMSCH_Team2 = data.SPCCETMSCH_Team2;
                        mapp.SPCCETMSCH_Date = data.SPCCETMSCH_Date;
                        mapp.SPCCETMSCH_Time = data.SPCCETMSCH_Time;
                        mapp.SPCCETMSCH_Result = data.SPCCETMSCH_Result;
                        mapp.SPCCETMSCH_Remarks = data.SPCCETMSCH_Remarks;
                        mapp.SPCCETMSCH_ActiveFlag = true;
                        mapp.SPCCETMSCH_CreatedDate = DateTime.Now;
                        mapp.SPCCETMSCH_UpdatedDate = DateTime.Now;
                        _sportcontext.Add(mapp);

                        
                        int s = _sportcontext.SaveChanges();
                        if (s > 0)
                        {
                            data.msg = "saved";
                        }
                        else
                        {
                            data.msg = "savingFailed";
                        }
                    }

                }   
                //UpdateRecords
                if(data.SPCCETMSCH_Id > 0)
                {
                       var checkduplicate = _sportcontext.EventsTeamScheduleDMO.Where(d => d.MI_Id == data.MI_Id && d.SPCCETMSCH_Team1 == data.SPCCETMSCH_Team1 && d.SPCCETMSCH_Team2 == data.SPCCETMSCH_Team2 && d.SPCCETMSCH_Date == data.SPCCETMSCH_Date && d.SPCCETMSCH_Time == data.SPCCETMSCH_Time).ToList();

                    if(checkduplicate.Count > 0)
                    {
                        data.msg = "duplicate";
                    }
                    else
                    {                       
                        var update = _sportcontext.EventsTeamScheduleDMO.Single(d => d.SPCCETMSCH_Id == data.SPCCETMSCH_Id && d.MI_Id == data.MI_Id);
                        update.MI_Id = data.MI_Id;
                        update.SPCCETMSCH_Team1 = data.SPCCETMSCH_Team1;
                        update.SPCCETMSCH_Team2 = data.SPCCETMSCH_Team2;
                        update.SPCCETMSCH_Date = data.SPCCETMSCH_Date;
                        update.SPCCETMSCH_Time = data.SPCCETMSCH_Time;
                        update.SPCCETMSCH_Result = data.SPCCETMSCH_Result;
                        update.SPCCETMSCH_Remarks = data.SPCCETMSCH_Remarks;
                        update.SPCCETMSCH_ActiveFlag = true;
                        update.SPCCETMSCH_CreatedDate = DateTime.Now;
                        update.SPCCETMSCH_UpdatedDate = DateTime.Now;
                        _sportcontext.Update(update);


                        int s = _sportcontext.SaveChanges();
                        if (s > 0)
                        {
                            data.msg = "updated";
                        }
                        else
                        {
                            data.msg = "updateFailed";
                        }
                    }
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }

        //GetDetails
        public SportsReportTeamPageDto Getdetails(SportsReportTeamPageDto data)//int IVRMM_Id
        {

            try
            {
                var list = _sportcontext.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).OrderByDescending(t => t.ASMAY_Order).ToList();
                data.yearlist = list.ToArray();

                data.categoryList = (from a in _sportcontext.MasterCompitionCategoryDMO
                                     where (a.MI_Id == data.MI_Id && a.SPCCMCC_ActiveFlag == true)
                                     select new EventsStudentRecordDTO
                                     {
                                         SPCCMCC_Id = a.SPCCMCC_Id,
                                         SPCCMCC_CompitionCategory = a.SPCCMCC_CompitionCategory,
                                     }).Distinct().ToArray();

                //CompetetionLevel
                data.CompetetionLevel = _sportcontext.SportMasterCompitionLevelDMO.Where(R => R.SPCCMCL_ActiveFlag == true && R.MI_Id == data.MI_Id).Distinct().ToArray();

                //MasterEventsDMO
                //data.MasterEvent = _sportcontext.MasterEventsDMO.Where(R => R.MI_Id == data.MI_Id && R.SPCCME_ActiveFlag == true).Distinct().ToArray();

                data.sportsName = (from a in _sportcontext.MasterSportsCCNameDMO
                                   where (a.MI_Id == data.MI_Id && a.SPCCMSCC_ActiveFlag == true)
                                   select new SportsReportTeamPageDto
                                   {
                                       SPCCMSCC_Id = a.SPCCMSCC_Id,
                                       SPCCMSCC_SportsCCName = a.SPCCMSCC_SportsCCName,
                                       SPCCMSCC_NoOfMembers =a.SPCCMSCC_NoOfMembers
                                   }).Distinct().ToArray();
                
                data.teamlistone = _sportcontext.EventsTeamDMO.Where(R => R.SPCCETM_ActiveFlag == true && R.MI_Id == data.MI_Id).Distinct().ToArray();

                //data.teamlistone = (from a in _sportcontext.EventsTeamDMO
                //                    where (a.MI_Id == data.MI_Id && a.SPCCETM_ActiveFlag == true)
                //                     select new SportsReportTeamPageDto
                //                     {
                //                         SPCCETMSCH_Id = a.SPCCETMSCH_Id,
                //                         SPCCETMSCH_Team1 = a.SPCCETMSCH_Team1,
                //                         SPCCETMSCH_Team2 = a.SPCCETMSCH_Team2,                                        
                //                         MI_Id = a.MI_Id,                                       
                //                     }).Distinct().ToArray();

                // data.GetMasterEvent = _sportcontext.MasterSportsCCGroupDMO.Where(d => d.MI_Id == data.MI_Id && d.SPCCMSCCG_ActiveFlag == true && d.SPCCMSCCG_Under == null).Distinct().OrderBy(t => t.SPCCMSCCG_Id).ToArray();

                //List<School_M_Class> allclass = new List<School_M_Class>();

                //allclass = _sportcontext.School_M_Class.Where(d => d.ASMCL_ActiveFlag == true && d.MI_Id == data.MI_Id).OrderBy(d => d.ASMCL_Order).ToList();
                //data.classDrpDwn = allclass.ToArray();

                //data.alldata 
                using (var cmd = _sportcontext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "SPC_Sports_Events_Report";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });



                    if (cmd.Connection.State != ConnectionState.Open) cmd.Connection.Open();
                    var retObject = new List<dynamic>();

                    using (var dataReader = cmd.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                            for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                            {
                                dataRow1.Add(
                                    dataReader.GetName(iFiled1),
                                    dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}
                                );
                            }
                            retObject.Add((ExpandoObject)dataRow1);
                        }
                    }
                    data.alldata = retObject.ToArray();
                }

                //alldata----
                using (var cmd = _sportcontext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "SPCC_Events_Team_Schedule";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });



                    if (cmd.Connection.State != ConnectionState.Open) cmd.Connection.Open();
                    var retObject = new List<dynamic>();

                    using (var dataReader = cmd.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                            for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                            {
                                dataRow1.Add(
                                    dataReader.GetName(iFiled1),
                                    dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}
                                );
                            }
                            retObject.Add((ExpandoObject)dataRow1);
                        }
                    }
                    data.alldatas = retObject.ToArray();
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;

        }
        public SportsReportTeamPageDto EditRecord(SportsReportTeamPageDto data)
        {
            try
            {
                //  var editData = (from a in _sportcontext.Adm_M_Student
                //                  from b in _sportcontext.EventsTeamDMO
                //                  from c in _sportcontext.MasterCompitionCategoryDMO
                //                  from d in _sportcontext.SportMasterCompitionLevelDMO
                //                  from e in _sportcontext.MasterSportsCCNameDMO
                //                  from f in _sportcontext.AcademicYear
                //                  from g in _sportcontext.EventsTeamStudentsDMO
                //                  from h in _sportcontext.School_M_Class
                //                  where (f.ASMAY_Id == b.ASMAY_Id && b.SPCCMCL_Id == d.SPCCMCL_Id && b.SPCCMCC_Id == c.SPCCMCC_Id && e.SPCCMSCC_Id == b.SPCCMSCC_Id && b.SPCCETM_Id==data.SPCCETM_Id && g.AMST_Id == a.AMST_Id && a.ASMCL_Id== h.ASMCL_Id)
                //                  select new SportsReportTeamPageDto
                //                  {

                //                      ASMAY_Id = b.ASMAY_Id,
                //                      SPCCE_Id = b.SPCCE_Id,
                //                      SPCCMCL_Id = b.SPCCMCL_Id,
                //                      SPCCETM_TeamName = b.SPCCETM_TeamName,
                //                      SPCCETM_NoOfParticipants = b.SPCCETM_NoOfParticipants,
                //                      SPCCMCC_Id = b.SPCCMCC_Id,
                //                      SPCCMSCC_Id = b.SPCCMSCC_Id,
                //                      ASMCL_Id = h.ASMCL_Id,
                //                      AMST_FirstName = a.AMST_FirstName + (string.IsNullOrEmpty(a.AMST_MiddleName) || a.AMST_MiddleName == "0" ? "" : ' ' + a.AMST_MiddleName) + (string.IsNullOrEmpty(a.AMST_LastName) || a.AMST_LastName == "0" ? "" : ' ' + a.AMST_LastName),
                //                      AMST_AdmNo = a.AMST_AdmNo,

                //                  }).ToList();
                //  data.editrecord = editData.ToArray();

                //  data.studentList = (from a in _sportcontext.admissionyearstudent
                //                      from c in _sportcontext.admissionClass
                //                      from b in _sportcontext.admissionStduent
                //                      from d in _sportcontext.EventsTeamStudentsDMO
                //                      where (b.MI_Id == c.MI_Id && b.ASMCL_Id == c.ASMCL_Id && d.AMST_Id == b.AMST_Id && b.MI_Id == data.MI_Id && a.ASMAY_Id == editData.FirstOrDefault().ASMAY_Id && a.ASMCL_Id == editData.FirstOrDefault().ASMCL_Id && a.AMAY_ActiveFlag == 1 && b.AMST_SOL == "S")
                //                      select new SportsReportTeamPageDto
                //                      {
                //                          AMST_Id = b.AMST_Id,
                //                          AMST_FirstName = b.AMST_FirstName + (string.IsNullOrEmpty(b.AMST_MiddleName) || b.AMST_MiddleName == "0" ? "" : ' ' + b.AMST_MiddleName) + (string.IsNullOrEmpty(b.AMST_LastName) || b.AMST_LastName == "0" ? "" : ' ' + b.AMST_LastName),
                //                          AMST_AdmNo = b.AMST_AdmNo,
                //                      }
                //).Distinct().OrderBy(b => b.AMST_FirstName).ToArray();

                var editrecord = _sportcontext.EventsTeamDMO.Where(R => R.MI_Id == data.MI_Id && R.SPCCETM_Id == data.SPCCETM_Id).Distinct().ToList();
                data.editrecord = editrecord.ToArray();

                //data.studentList = (from a in _sportcontext.Adm_M_Student
                //                    from b in _sportcontext.EventsTeamDMO
                //                    from c in _sportcontext.EventsTeamStudentsDMO
                //                    from d  in  _sportcontext.admissionyearstudent 
                //                    from f  in  _sportcontext.School_M_Class 
                //                    from g in _sportcontext.masterSection
                //                    where (b.MI_Id == data.MI_Id && b.ASMAY_Id == b.ASMAY_Id && b.SPCCETM_Id == c.SPCCETM_Id && a.AMST_Id == c.AMST_Id 
                //                    && c.AMST_Id==d.AMST_Id &&  c.AMST_Id==a.AMST_Id &&  d.ASMCL_Id==f.ASMCL_Id && g.ASMS_Id==d.ASMS_Id && b.ASMAY_Id== d.ASMAY_Id && b.ASMAY_Id== editrecord.FirstOrDefault().ASMAY_Id)
                //                    select new SportsReportTeamPageDto
                //                    {
                //                        ASMCL_Id = a.ASMCL_Id,
                //                        AMST_Id = a.AMST_Id,
                //                        AMST_FirstName = a.AMST_FirstName,
                //                        AMST_AdmNo = a.AMST_AdmNo,
                //                        ASMCL_ClassName = f.ASMCL_ClassName,
                //                        ASMC_SectionName = g.ASMC_SectionName,
                //                    }
                //                    ).Distinct().OrderBy(b => b.AMST_FirstName).ToArray();               


                using (var cmd = _sportcontext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "SPC_Sports_Student_List";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.Parameters.Add(new SqlParameter("@SPCCETM_Id", SqlDbType.VarChar) { Value = data.SPCCETM_Id });



                    if (cmd.Connection.State != ConnectionState.Open) cmd.Connection.Open();
                    var retObject = new List<dynamic>();

                    using (var dataReader = cmd.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                            for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                            {
                                dataRow1.Add(
                                    dataReader.GetName(iFiled1),
                                    dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}
                                );
                            }
                            retObject.Add((ExpandoObject)dataRow1);
                        }
                    }
                    data.studentList = retObject.ToArray();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return data;
        }

        //GetEditData
        public SportsReportTeamPageDto GetEditData(SportsReportTeamPageDto data)
        {
            try
            {
                var geteditdata = _sportcontext.EventsTeamScheduleDMO.Where(R => R.MI_Id == data.MI_Id && R.SPCCETMSCH_Id == data.SPCCETMSCH_Id).Distinct().ToList();
                data.geteditdata = geteditdata.ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public SportsReportTeamPageDto deactivate(SportsReportTeamPageDto dto)
        {
            try
            {

                var result = _sportcontext.EventsTeamDMO.Single(t => t.MI_Id == dto.MI_Id && t.SPCCETM_Id == dto.SPCCETM_Id);
                if (result.SPCCETM_ActiveFlag == true)
                {
                    result.SPCCETM_ActiveFlag = false;
                }
                else
                {
                    result.SPCCETM_ActiveFlag = true;
                }
                result.SPCCETM_CreatedDate = result.SPCCETM_CreatedDate;
                result.SPCCETM_UpdatedDate = DateTime.Now;
                _sportcontext.Update(result);
                var flag = _sportcontext.SaveChanges();
                if (flag == 1)
                {
                    dto.returnVal = true;

                    if (result.SPCCETM_ActiveFlag == true)
                    {
                        dto.msg = "Activated Successfully.";
                    }
                    else if (result.SPCCETM_ActiveFlag == false)
                    {
                        dto.msg = "Deactivated Successfully.";
                    }
                }
                else
                {
                    dto.returnVal = false;
                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }

        //Deactivated
        public SportsReportTeamPageDto deactivated(SportsReportTeamPageDto dto)
        {
            try
            {

                var result = _sportcontext.EventsTeamScheduleDMO.Single(t => t.MI_Id == dto.MI_Id && t.SPCCETMSCH_Id == dto.SPCCETMSCH_Id);
                if (result.SPCCETMSCH_ActiveFlag == true)
                {
                    result.SPCCETMSCH_ActiveFlag = false;
                }
                else
                {
                    result.SPCCETMSCH_ActiveFlag = true;
                }
                result.SPCCETMSCH_CreatedDate = result.SPCCETMSCH_CreatedDate;
                result.SPCCETMSCH_UpdatedDate = DateTime.Now;
                _sportcontext.Update(result);
                var flag = _sportcontext.SaveChanges();
                if (flag == 1)
                {
                    dto.returnVal = true;

                    if (result.SPCCETMSCH_ActiveFlag == true)
                    {
                        dto.msg = "Activated Successfully.";
                    }
                    else if (result.SPCCETMSCH_ActiveFlag == false)
                    {
                        dto.msg = "Deactivated Successfully.";
                    }
                }
                else
                {
                    dto.returnVal = false;
                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }

        public SportsReportTeamPageDto get_student(SportsReportTeamPageDto data)
        {
            try
            {
                //&& a.ASMCL_Id == data.ASMCL_Id //&& c.ASMCL_Id== data.ASMCL_Id
                data.studentList = (from a in _sportcontext.admissionyearstudent
                                    from b in _sportcontext.admissionStduent
                                    from c in _sportcontext.School_M_Class
                                    from d in _sportcontext.masterSection
                                    where (b.AMST_ActiveFlag == 1 && b.MI_Id == data.MI_Id
                                    && a.AMST_Id == b.AMST_Id
                                    && a.ASMCL_Id == c.ASMCL_Id && d.ASMS_Id == a.ASMS_Id && a.ASMAY_Id == data.ASMAY_Id)
                                    select new SportsReportTeamPageDto
                                    {
                                        AMST_Id = b.AMST_Id,
                                        AMST_FirstName = b.AMST_FirstName,
                                        AMST_AdmNo = b.AMST_AdmNo,
                                        ASMCL_ClassName = c.ASMCL_ClassName,
                                        ASMC_SectionName = d.ASMC_SectionName,

                                    }).Distinct().OrderBy(t => t.AMST_Id).ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;

        }

        public SportsReportTeamPageDto showdetails(SportsReportTeamPageDto data)
        {
            try
            {
                //data.MasterEvent = _sportcontext.MasterEventsDMO.Where(R => R.MI_Id == data.MI_Id && R.SPCCME_ActiveFlag == true).Distinct().ToArray();
                data.MasterEvent = (from a in _sportcontext.MasterEventsDMO
                                    from b in _sportcontext.EventsMappingDMO
                                    where (a.SPCCME_Id == b.SPCCME_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id)
                                    select new SportsReportTeamPageDto
                                    {
                                        SPCCE_Id = b.SPCCE_Id,
                                        SPCCME_EventName = a.SPCCME_EventName

                                    }).Distinct().ToArray();

                //classDrpDwn
                using (var cmd = _sportcontext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Spc_Get_Classlist";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });

                    if (cmd.Connection.State != ConnectionState.Open) cmd.Connection.Open();
                    var retObject = new List<dynamic>();

                    using (var dataReader = cmd.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                            for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                            {
                                dataRow1.Add(
                                    dataReader.GetName(iFiled1),
                                    dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}
                                );
                            }
                            retObject.Add((ExpandoObject)dataRow1);
                        }
                    }
                    data.classDrpDwn = retObject.ToArray();
                }

                //studentlist
                data.studentList = (from a in _sportcontext.admissionyearstudent
                                    from b in _sportcontext.admissionStduent
                                    from c in _sportcontext.School_M_Class
                                    from d in _sportcontext.masterSection
                                    where (b.AMST_ActiveFlag == 1 && b.MI_Id == data.MI_Id
                                    && a.AMST_Id == b.AMST_Id
                                    && a.ASMCL_Id == c.ASMCL_Id && d.ASMS_Id == a.ASMS_Id && a.ASMAY_Id == data.ASMAY_Id)
                                    select new SportsReportTeamPageDto
                                    {
                                        AMST_Id = b.AMST_Id,
                                        AMST_FirstName = b.AMST_FirstName,
                                        AMST_AdmNo = b.AMST_AdmNo,
                                        ASMCL_ClassName = c.ASMCL_ClassName,
                                        ASMC_SectionName = d.ASMC_SectionName
                                    }).Distinct().OrderBy(t => t.AMST_Id).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return data;
        }

        public SportsReportTeamPageDto get_modeldata(SportsReportTeamPageDto data)
        {
            try
            {

                using (var cmd = _sportcontext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "SPC_Sports_Student_Details";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.Parameters.Add(new SqlParameter("@SPCCETM_Id", SqlDbType.VarChar) { Value = data.SPCCETM_Id });



                    if (cmd.Connection.State != ConnectionState.Open) cmd.Connection.Open();
                    var retObject = new List<dynamic>();

                    using (var dataReader = cmd.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                            for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                            {
                                dataRow1.Add(
                                    dataReader.GetName(iFiled1),
                                    dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}
                                );
                            }
                            retObject.Add((ExpandoObject)dataRow1);
                        }
                    }
                    data.modalsponsorlist = retObject.ToArray();
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


