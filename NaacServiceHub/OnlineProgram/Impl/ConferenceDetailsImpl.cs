using AutoMapper;
using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.OnlineExam;
using DomainModel.Model.com.vapstech.OnlineProgram;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.OnlineProgram;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaacServiceHub.OnlineProgram.Impl
{
    public class ConferenceDetailsImpl : Interfaces.ConferenceDetailsInterface
    {
        private static ConcurrentDictionary<string, OnlineProgramDTO> _login =
           new ConcurrentDictionary<string, OnlineProgramDTO>();

        ILogger<YearlyProgramImpl> _log;
        public DomainModelMsSqlServerContext _dbContext;
        public ConferenceDetailsImpl(DomainModelMsSqlServerContext dbcontext, ILogger<YearlyProgramImpl> log)
        {
            _dbContext = dbcontext;
            _log = log;
        }


        public OnlineProgramDTO getloaddata(OnlineProgramDTO data)
        {
            try
            {

                data.Typelist = _dbContext.ProgramsMasterTypeDMO.Where(a => a.MI_Id == data.MI_Id).ToArray();

                data.levellist = _dbContext.ProgramsMasterLevelDMO.Where(a => a.MI_Id == data.MI_Id).ToArray();

                data.programlist = (from a in _dbContext.ProgramsYearlyDMO
                                        //from b in _dbContext.ProgramsYearlyFileDMO
                                        // from c in _dbContext.ProgramsYearlyGuestDMO
                                    from d in _dbContext.AcademicYear
                                    from e in _dbContext.HR_Master_Department
                                    where ( /*a.PRYR_Id == c.PRYR_Id &&*/ a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && d.ASMAY_Id == a.ASMAY_Id && e.HRMD_Id == a.HRMD_Id)
                                    //where (a.PRYR_Id == b.PRYR_Id && a.PRYR_Id == c.PRYR_Id && a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && d.ASMAY_Id == a.ASMAY_Id && e.HRMD_Id==a.HRMD_Id)
                                    select new OnlineProgramDTO
                                    {
                                        PRYR_Id = a.PRYR_Id,
                                        ASMAY_Id = a.ASMAY_Id,
                                        ASMAY_Year = d.ASMAY_Year,
                                        programname = a.PRYR_ProgramName,
                                        departmentname = e.HRMD_DepartmentName,
                                        description = a.PRYR_StartDate.ToString("dd/MM/yyyy"),
                                        PRYRA_ActiveFlag = a.PRYR_ActiveFlag
                                    }
                       ).Distinct().OrderByDescending(t => t.PRYR_Id).ToArray();



                var res1 = _dbContext.HR_Master_Department.Where(t => t.MI_Id == data.MI_Id).ToList();
                data.filldepartment = res1.Distinct().ToArray();

                List<MasterAcademic> year = new List<MasterAcademic>();
                year = _dbContext.AcademicYear.Where(y => y.Is_Active == true && y.MI_Id == data.MI_Id).OrderByDescending(y => y.ASMAY_Order).ToList();
                data.fillyear = year.GroupBy(y => y.ASMAY_Year).Select(y => y.First()).ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return data;
        }

        public OnlineProgramDTO Savedata(OnlineProgramDTO data)
        {
            if (data.PRYR_Id > 0)
            {
                var res = _dbContext.ProgramsYearlyDMO.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.PRYR_ProgramName.Equals(data.programname) && t.PRYR_Id != data.PRYR_Id).ToList();

                if (res.Count == 0)
                {
                    var objpge1 = _dbContext.ProgramsYearlyDMO.Single(t => t.PRYR_Id == data.PRYR_Id);

                    objpge1.ASMAY_Id = data.ASMAY_Id;
                    objpge1.MI_Id = data.MI_Id;
                    objpge1.PRYR_ProgramName = data.programname;
                    objpge1.PRYR_StartDate = data.Fromdate;
                    objpge1.PRYR_EndDate = data.Fromdate;
                    objpge1.PRYR_StartTime = data.start_time;
                    //objpge1.PRYR_EndTime = data.start_time;
                    objpge1.PRYR_ProgramDescription = data.description;
                    objpge1.PRYR_SponsorAgency = data.PRYR_SponsorAgency;
                    objpge1.PRYR_ActiveFlag = true;
                    objpge1.CreatedDate = DateTime.Now;
                    objpge1.UpdatedDate = DateTime.Now;
                    objpge1.PRYR_CreatedBy = data.UserId;
                    objpge1.PRYR_UpdatedBy = data.UserId;
                    // objpge2.PRYRG_GuestPhoneNo = data.PRYRG_GuestPhoneNo;                
                    objpge1.PRYR_IntParticipants = data.Int_part;
                    objpge1.PRYR_OthCollStudents = data.Stud_oth;
                    objpge1.PRYR_NatParticipants = data.Nat_part;
                    objpge1.PRYR_ResearchScholars = data.Rch_schl;
                    objpge1.PRYR_OurCollStudents = data.strength;
                    objpge1.PRYR_LecturesFlg = data.Lecture_1;
                    objpge1.PRYR_LecturesNo = data.Lecture;
                    objpge1.PRYR_TrainingFlg = data.traning_1;
                    objpge1.PRYR_TrainingNo = data.traning;
                    objpge1.PRYR_OralPresentationFlg = data.Oral_1;
                    objpge1.PRYR_OralPresentation = data.Oral;
                    objpge1.PRYR_PosterPresentationFlg = data.Poster_p_1;
                    objpge1.PRYR_PosterPresentation = data.Poster_p;
                    objpge1.PRYR_Faculty = data.Facty;
                    objpge1.PRYR_TotalParticipants = data.participants;
                    objpge1.PRYR_PrgramConvenor = data.PRYRG_GuestSpeech;
                    objpge1.PRYR_PrgramLevel = data.PRMTLE_Id;
                    objpge1.PRYR_ProgramTypeId = data.PRMTY_Id;
                    objpge1.HRMD_Id = data.HRMD_Id;

                    if (data.pgTempDTO2.Length > 0)
                    {
                        if (data.pgTempDTO2[0].LPMTR_Resources != null)
                        {
                            objpge1.PRYR_AccountStatement = data.pgTempDTO2[0].file_name;
                            objpge1.PRYR_ASPath = data.pgTempDTO2[0].LPMTR_Resources;
                        }

                    }
                    if (data.pgTempDTO4.Length > 0)
                    {
                        if (data.pgTempDTO4[0].LPMTR_Resources != null)
                        {
                            objpge1.PRYR_WinnerList = data.pgTempDTO4[0].file_name;
                            objpge1.PRYR_WListPath = data.pgTempDTO4[0].LPMTR_Resources;
                        }
                    }

                    // _dbContext.ProgramsYearlyDMO.Update(objpge1);
                    _dbContext.Update(objpge1);
                    var obbt = _dbContext.ProgramsYearlyActivitiesDMO.Single(t => t.PRYR_Id == data.PRYR_Id);
                    obbt.PRYR_Id = objpge1.PRYR_Id;
                    obbt.PRYRA_ActivityName = data.programname;
                    obbt.PRYRA_StartTime = data.start_time;
                    obbt.PRYRA_Duration = data.PRYRG_GuestPhoneNo;
                    obbt.PRYRA_Description = data.description;
                    obbt.PRYRA_ActiveFlag = true;
                    obbt.UpdatedDate = DateTime.Now;
                    obbt.PRYRA_CreatedBy = data.UserId;
                    _dbContext.Update(obbt);

                    var CountRemoveFiles = _dbContext.ProgramsYearlyFileDMO.Where(t => t.PRYR_Id == data.PRYR_Id).ToList();
                    if (CountRemoveFiles.Count > 0)
                    {
                        foreach (var RemoveFiles in CountRemoveFiles)
                        {
                            _dbContext.Remove(RemoveFiles);
                        }
                        if (data.pgTempDTO7.Length > 0)
                        {
                            foreach (pgTempDTO7 ph1 in data.pgTempDTO7)
                            {
                                if (data.pgTempDTO7[0].LPMTR_Resources != null)
                                {

                                    ProgramsYearlyFileDMO obj2 = new ProgramsYearlyFileDMO();
                                    obj2.PRYR_Id = objpge1.PRYR_Id;
                                    obj2.PRYRF_FileName = ph1.file_name;
                                    obj2.PRYRF_FileType = ph1.filetype;
                                    obj2.PRYRF_FilePath = ph1.LPMTR_Resources;
                                    obj2.PRYRF_ActiveFlag = true;
                                    obj2.PRYRF_CreatedBy = data.UserId;
                                    obj2.PRYRF_UpdatedBy = data.UserId;
                                    obj2.CreatedDate = DateTime.Now;
                                    obj2.UpdatedDate = DateTime.Now;

                                    _dbContext.Add(obj2);
                                }
                            }
                        }
                    }
                    else if (CountRemoveFiles.Count == 0)
                    {
                        if (data.pgTempDTO7.Length > 0)
                        {
                            foreach (pgTempDTO7 ph1 in data.pgTempDTO7)
                            {
                                if (data.pgTempDTO7[0].LPMTR_Resources != null)
                                {
                                    ProgramsYearlyFileDMO obj2 = new ProgramsYearlyFileDMO();
                                    obj2.PRYR_Id = objpge1.PRYR_Id;
                                    obj2.PRYRF_FileName = ph1.file_name;
                                    obj2.PRYRF_FileType = ph1.filetype;
                                    obj2.PRYRF_FilePath = ph1.LPMTR_Resources;
                                    obj2.PRYRF_ActiveFlag = true;
                                    obj2.PRYRF_CreatedBy = data.UserId;
                                    obj2.PRYRF_UpdatedBy = data.UserId;
                                    obj2.CreatedDate = DateTime.Now;
                                    obj2.UpdatedDate = DateTime.Now;

                                    _dbContext.Add(obj2);
                                }
                            }
                        }
                    }

                    var objpge2 = _dbContext.ProgramsYearlyGuestDMO.Single(t => t.PRYR_Id == data.PRYR_Id);

                    objpge2.PRYR_Id = objpge1.PRYR_Id;
                    //convenor
                    objpge2.PRYRG_GuestName = data.PRYRG_GuestName;
                    //title
                    objpge2.PRYRG_GuestType = data.PRYRG_GuestType;
                    //objpge2.PRYRG_GuestPhoneNo = data.PRYRG_GuestPhoneNo;
                    objpge2.UpdatedDate = DateTime.Now;
                    objpge2.PRYRG_UpdatedBy = data.UserId;
                    _dbContext.ProgramsYearlyGuestDMO.Update(objpge2);
                    _dbContext.Update(objpge2);

                    var contactExists = _dbContext.SaveChanges();
                    if (contactExists > 0)
                    {
                        data.returnresult = true;
                        data.message = "Update";
                    }
                    else
                    {
                        data.returnresult = false;
                        data.message = "Not Update";
                    }
                }
                else
                {
                    data.message = "Duplicate";
                }
            }

            else
            {
                try
                {
                    var res = _dbContext.ProgramsYearlyDMO.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.PRYR_ProgramName.Equals(data.programname)).ToList();

                    if (res.Count == 0)
                    {
                        ProgramsYearlyDMO objpge1 = new ProgramsYearlyDMO();
                        ProgramsYearlyGuestDMO objpge3 = new ProgramsYearlyGuestDMO();
                        ProgramsYearlyActivitiesDMO obbb = new ProgramsYearlyActivitiesDMO();

                        objpge1.ASMAY_Id = data.ASMAY_Id;
                        objpge1.MI_Id = data.MI_Id;
                        objpge1.PRYR_ProgramName = data.programname;
                        objpge1.PRYR_StartDate = data.Fromdate;
                        objpge1.PRYR_EndDate = data.Fromdate;
                        objpge1.PRYR_StartTime = data.start_time;
                        objpge1.PRYR_ProgramDescription = data.description;
                        objpge1.PRYR_ActiveFlag = true;
                        objpge1.CreatedDate = DateTime.Now;
                        objpge1.UpdatedDate = DateTime.Now;
                        objpge1.PRYR_CreatedBy = data.UserId;
                        objpge1.PRYR_UpdatedBy = data.UserId;
                        objpge1.PRYR_SponsorAgency = data.PRYR_SponsorAgency;
                        objpge1.PRYR_IntParticipants = data.Int_part;
                        objpge1.PRYR_OthCollStudents = data.Stud_oth;
                        objpge1.PRYR_NatParticipants = data.Nat_part;
                        objpge1.PRYR_ResearchScholars = data.Rch_schl;
                        objpge1.PRYR_OurCollStudents = data.strength;
                        objpge1.PRYR_LecturesFlg = data.Lecture_1;
                        objpge1.PRYR_LecturesNo = data.Lecture;
                        objpge1.PRYR_TrainingFlg = data.traning_1;
                        objpge1.PRYR_TrainingNo = data.traning;
                        objpge1.PRYR_OralPresentationFlg = data.Oral_1;
                        objpge1.PRYR_OralPresentation = data.Oral;
                        objpge1.PRYR_PosterPresentationFlg = data.Poster_p_1;
                        objpge1.PRYR_PosterPresentation = data.Poster_p;
                        objpge1.PRYR_Faculty = data.Facty;
                        objpge1.PRYR_TotalParticipants = data.participants;
                        objpge1.PRYR_PrgramConvenor = data.PRYRG_GuestSpeech;
                        objpge1.PRYR_PrgramLevel = data.PRMTLE_Id;
                        objpge1.PRYR_ProgramTypeId = data.PRMTY_Id;
                        objpge1.HRMD_Id = data.HRMD_Id;


                        if (data.pgTempDTO2.Length > 0)
                        {
                            if (data.pgTempDTO2[0].filetype != null)
                            {
                                objpge1.PRYR_AccountStatement = data.pgTempDTO2[0].file_name;
                                objpge1.PRYR_ASPath = data.pgTempDTO2[0].LPMTR_Resources;
                            }

                        }
                        if (data.pgTempDTO4.Length > 0)
                        {
                            if (data.pgTempDTO4[0].filetype != null)
                            {

                                objpge1.PRYR_WinnerList = data.pgTempDTO4[0].file_name;
                                objpge1.PRYR_WListPath = data.pgTempDTO4[0].LPMTR_Resources;
                            }
                        }

                        _dbContext.ProgramsYearlyDMO.Add(objpge1);


                        foreach (pgTempDTO7 ph1 in data.pgTempDTO7)
                        {
                            if (data.pgTempDTO7[0].filetype != null)
                            {
                                ProgramsYearlyFileDMO objpge2 = new ProgramsYearlyFileDMO();
                                objpge2.PRYR_Id = objpge1.PRYR_Id;

                                objpge2.PRYRF_FileName = ph1.file_name;
                                objpge2.PRYRF_FileType = ph1.filetype;
                                objpge2.PRYRF_FilePath = ph1.LPMTR_Resources;
                                objpge2.PRYRF_ActiveFlag = true;
                                objpge2.PRYRF_CreatedBy = data.UserId;
                                objpge2.PRYRF_UpdatedBy = data.UserId;
                                objpge2.CreatedDate = DateTime.Now;
                                objpge2.UpdatedDate = DateTime.Now;
                                _dbContext.ProgramsYearlyFileDMO.Add(objpge2);
                            }
                        }


                        obbb.PRYR_Id = objpge1.PRYR_Id;
                        obbb.PRYRA_ActivityName = data.programname;
                        obbb.PRYRA_StartTime = data.start_time;
                        obbb.PRYRA_Duration = data.PRYRG_GuestPhoneNo;
                        obbb.PRYRA_Description = data.description;
                        obbb.PRYRA_ActiveFlag = true;
                        obbb.CreatedDate = DateTime.Now;
                        obbb.UpdatedDate = DateTime.Now;
                        obbb.PRYRA_CreatedBy = data.UserId;
                        obbb.PRYRA_UpdatedBy = data.UserId;
                        _dbContext.ProgramsYearlyActivitiesDMO.Add(obbb);

                        objpge3.PRYR_Id = objpge1.PRYR_Id;
                        //invited speaker
                        objpge3.PRYRG_GuestName = data.PRYRG_GuestName;
                        objpge3.PRYRG_GuestType = data.PRYRG_GuestType;
                        objpge3.PRYRG_ActiveFlag = true;
                        objpge3.CreatedDate = DateTime.Now;
                        objpge3.UpdatedDate = DateTime.Now;
                        objpge3.PRYRG_UpdatedBy = data.UserId;
                        objpge3.PRYRG_CreatedBy = data.UserId;

                        _dbContext.ProgramsYearlyGuestDMO.Add(objpge3);

                        var contactExists = _dbContext.SaveChanges();
                        if (contactExists >= 1)
                        {
                            data.returnresult = true;
                         data.message = "Saved";
                        }
                        else
                        {
                            data.returnresult = false;
                            data.message = "Not Saved";
                        }
                    }
                    else
                    {
                        data.message = "Duplicate";
                    }
                }

                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
            return data;
        }


        public OnlineProgramDTO getdetails(OnlineProgramDTO data)
        {
            try
            {

                //data.programlist = _dbContext.ProgramsYearlyDMO.Where(a => a.MI_Id == data.MI_Id && a.PRYR_Id==data.PRYR_Id).ToArray();
                data.programlist = (from a in _dbContext.ProgramsYearlyDMO
                                    from b in _dbContext.ProgramsYearlyActivitiesDMO
                                    where (a.PRYR_Id == b.PRYR_Id && a.MI_Id == data.MI_Id && a.PRYR_Id == data.PRYR_Id)
                                    select new OnlineProgramDTO
                                    {
                                        PRYR_ProgramName = a.PRYR_ProgramName,
                                        PRYR_ProgramTypeId = a.PRYR_ProgramTypeId,
                                        PRYR_ProgramDescription = a.PRYR_ProgramDescription,
                                        PRYR_SponsorAgency = a.PRYR_SponsorAgency,
                                        HRMD_Id = a.HRMD_Id,
                                        PRYR_PrgramConvenor = a.PRYR_PrgramConvenor,
                                        PRYR_StartTime = a.PRYR_StartTime,
                                        PRYR_TotalParticipants = a.PRYR_TotalParticipants,
                                        PRYR_OurCollStudents = a.PRYR_OurCollStudents,
                                        PRYR_Faculty = a.PRYR_Faculty,
                                        PRYR_OthCollStudents = a.PRYR_OthCollStudents,
                                        PRYR_NatParticipants = a.PRYR_NatParticipants,
                                        PRYR_IntParticipants = a.PRYR_IntParticipants,
                                        PRYR_ResearchScholars = a.PRYR_ResearchScholars,
                                        PRYR_OralPresentation = a.PRYR_OralPresentation,
                                        PRYR_LecturesNo = a.PRYR_LecturesNo,
                                        PRYR_TrainingNo = a.PRYR_TrainingNo,
                                        PRYR_Id = a.PRYR_Id,
                                        PRYR_PosterPresentation = a.PRYR_PosterPresentation,
                                        PRYR_OralPresentationFlg = a.PRYR_OralPresentationFlg,
                                        PRYR_LecturesFlg = a.PRYR_LecturesFlg,
                                        PRYR_TrainingFlg = a.PRYR_TrainingFlg,
                                        PRYR_PosterPresentationFlg = a.PRYR_PosterPresentationFlg,
                                        PRYR_StartDate = a.PRYR_StartDate,
                                        PRYR_PrgramLevel = a.PRYR_PrgramLevel,
                                        PRYRG_GuestPhoneNo = b.PRYRA_Duration,
                                        // a.PRYR_StartDate.ToString("dd/MM/yyyy"),

                                    }).Distinct().ToArray();
                data.alllist = _dbContext.ProgramsYearlyGuestDMO.Where(a => a.PRYR_Id == data.PRYR_Id).ToArray();

                data.fillActivities = _dbContext.ProgramsYearlyGuestDMO.Where(a => a.PRYR_Id == data.PRYR_Id).ToArray();


                var res1 = _dbContext.HR_Master_Department.Where(t => t.MI_Id == data.MI_Id).ToList();
                data.filldepartment = res1.Distinct().ToArray();


                data.uploadfiles2 = (from a in _dbContext.ProgramsYearlyDMO
                                     where (a.MI_Id == data.MI_Id && a.PRYR_Id == data.PRYR_Id)
                                     select new OnlineProgramDTO
                                     {
                                         PRYR_Id = a.PRYR_Id,
                                         LPMTR_FileName = a.PRYR_AccountStatement,
                                         LPMTR_ResourceType = a.PRYR_ASPath
                                     }).Distinct().ToArray();

                data.testarray = (from a in _dbContext.ProgramsYearlyDMO
                                  where (a.MI_Id == data.MI_Id && a.PRYR_Id == data.PRYR_Id)
                                  select new OnlineProgramDTO
                                  {
                                      PRYR_Id = a.PRYR_Id,
                                      LPMTR_FileName = a.PRYR_WinnerList,
                                      LPMTR_ResourceType = a.PRYR_WListPath
                                  }).Distinct().ToArray();



                data.uploadfiles1 = (from a in _dbContext.ProgramsYearlyDMO
                                     from b in _dbContext.ProgramsYearlyFileDMO
                                     where (a.PRYR_Id == b.PRYR_Id && a.MI_Id == data.MI_Id && a.PRYR_Id == data.PRYR_Id)
                                     select new OnlineProgramDTO
                                     {
                                         PRYRF_Id = b.PRYRF_Id,
                                         PRYR_Id = a.PRYR_Id,
                                         LPMTR_FileName = b.PRYRF_FileName,
                                         LPMTR_Filetype = b.PRYRF_FileType,
                                         LPMTR_ResourceType = b.PRYRF_FilePath
                                     }).Distinct().ToArray();


            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return data;
        }



        public OnlineProgramDTO delete(OnlineProgramDTO data)
        {
            try
            {

                var lorg1 = _dbContext.ProgramsYearlyDMO.Single(t => t.PRYR_Id == data.PRYR_Id);

                var lorg2 = _dbContext.ProgramsYearlyFileDMO.Where(t => t.PRYR_Id == data.PRYR_Id);

                var lorg3 = _dbContext.ProgramsYearlyGuestDMO.Where(t => t.PRYR_Id == data.PRYR_Id).ToList();

                var lorg4 = _dbContext.ProgramsYearlyActivitiesDMO.Where(t => t.PRYR_Id == data.PRYR_Id).ToList();
                foreach (var vv in lorg4)
                {
                    var checkresult = _dbContext.ProgramsYearlyActivitiesDMO.Single(a => a.PRYRA_Id == vv.PRYRA_Id);
                    _dbContext.Remove(checkresult);
                }

                foreach (var c in lorg3)
                {
                    var checkresult = _dbContext.ProgramsYearlyGuestDMO.Single(a => a.PRYRG_Id == c.PRYRG_Id);
                    _dbContext.Remove(checkresult);

                }
                foreach (var b in lorg2)
                {
                    var checkresult = _dbContext.ProgramsYearlyFileDMO.Single(a => a.PRYRF_Id == b.PRYRF_Id);
                    _dbContext.Remove(checkresult);

                }

                _dbContext.Remove(lorg1);

                var contactexisttransaction = 0;
                using (var dbCtxTxn = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        contactexisttransaction = _dbContext.SaveChanges();
                        dbCtxTxn.Commit();
                        data.returnval = "true";
                    }
                    catch (Exception ex)
                    {
                        dbCtxTxn.Rollback();
                        data.returnval = "false";
                    }
                }


            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
            }
            return data;
        }




        public OnlineProgramDTO viewuploadflies(OnlineProgramDTO data)
        {
            try
            {
                if (data.description == "account")
                {
                    data.uploadfiles2 = (from a in _dbContext.ProgramsYearlyDMO
                                         where (a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.PRYR_Id == data.PRYR_Id)
                                         select new OnlineProgramDTO
                                         {
                                             PRYR_Id = a.PRYR_Id,
                                             LPMTR_FileName = a.PRYR_AccountStatement,
                                             LPMTR_ResourceType = a.PRYR_ASPath
                                         }).Distinct().ToArray();
                }
                else if (data.description == "winner")
                {
                    data.uploadfiles = (from a in _dbContext.ProgramsYearlyDMO
                                        where (a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.PRYR_Id == data.PRYR_Id)
                                        select new OnlineProgramDTO
                                        {
                                            PRYR_Id = a.PRYR_Id,
                                            LPMTR_FileName = a.PRYR_WinnerList,
                                            LPMTR_ResourceType = a.PRYR_WListPath
                                        }).Distinct().ToArray();
                }
                else if (data.description == "programphoto")
                {
                    data.uploadfiles1 = (from a in _dbContext.ProgramsYearlyDMO
                                         from b in _dbContext.ProgramsYearlyFileDMO
                                         where (a.PRYR_Id == b.PRYR_Id && a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && a.PRYR_Id == data.PRYR_Id)
                                         select new OnlineProgramDTO
                                         {
                                             PRYRF_Id = b.PRYRF_Id,
                                             PRYR_Id = a.PRYR_Id,
                                             LPMTR_FileName = b.PRYRF_FileName,
                                             LPMTR_Filetype = b.PRYRF_FileType,
                                             LPMTR_ResourceType = b.PRYRF_FilePath
                                         }).Distinct().ToArray();
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
