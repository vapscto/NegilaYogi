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
    public class YearlyProgramImpl : Interfaces.YearlyProgramInterface
    {
        private static ConcurrentDictionary<string, OnlineProgramDTO> _login =
           new ConcurrentDictionary<string, OnlineProgramDTO>();

        ILogger<YearlyProgramImpl> _log;
        public DomainModelMsSqlServerContext _dbContext;
        public YearlyProgramImpl(DomainModelMsSqlServerContext dbcontext, ILogger<YearlyProgramImpl> log)
        {
            _dbContext = dbcontext;
            _log = log;
        }
        public OnlineProgramDTO getloaddata(OnlineProgramDTO data)
        {
            try
            {

                data.fillActivities = _dbContext.ProgramsYearlyActivitiesDMO.ToArray();

                data.Typelist = _dbContext.ProgramsMasterTypeDMO.Where(a => a.MI_Id == data.MI_Id && a.PRMTY_ActiveFlg == true).ToArray();

                data.levellist = _dbContext.ProgramsMasterLevelDMO.Where(a => a.MI_Id == data.MI_Id && a.PRMTLE_ActiveFlg == true).ToArray();



                //data.programlist = (from a in _dbContext.ProgramsYearlyDMO
                //                    from b in _dbContext.ProgramsYearlyFileDMO
                //                    from c in _dbContext.ProgramsYearlyGuestDMO
                //                    from d in _dbContext.AcademicYear
                //                    where (a.PRYR_Id == b.PRYR_Id && a.PRYR_Id == c.PRYR_Id && a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && d.ASMAY_Id == a.ASMAY_Id)
                //                    select new OnlineProgramDTO
                //                    {
                //                        PRYR_Id = a.PRYR_Id,
                //                        ASMAY_Id=a.ASMAY_Id,
                //                        ASMAY_Year = d.ASMAY_Year,
                //                        programname = a.PRYR_ProgramName,
                //                        start_time = a.PRYR_StartTime,
                //                        end_time = a.PRYR_EndTime,
                //                        club = a.PRYR_StartDate.ToString("dd/MM/yyyy"),
                //                        Org_name = Convert.ToDateTime(a.PRYR_EndDate).ToString("dd/MM/yyyy"),
                //                        PRYRA_ActiveFlag = a.PRYR_ActiveFlag
                //                    }
                //       ).Distinct().OrderByDescending(t => t.PRYR_Id).ToArray();



                data.programlist = (from a in _dbContext.ProgramsYearlyDMO
                                    from d in _dbContext.AcademicYear
                                    where (a.MI_Id == data.MI_Id && d.ASMAY_Id == a.ASMAY_Id && a.PRYR_EndTime != null&&a.PRYR_EndTime!="NULL")
                                    select new OnlineProgramDTO
                                    {
                                        PRYR_Id = a.PRYR_Id,
                                        ASMAY_Id = a.ASMAY_Id,
                                        ASMAY_Year = d.ASMAY_Year,
                                        programname = a.PRYR_ProgramName,
                                        start_time = a.PRYR_StartTime,
                                        end_time = a.PRYR_EndTime,
                                        club = a.PRYR_StartDate.ToString("dd/MM/yyyy"),
                                        Org_name = Convert.ToDateTime(a.PRYR_EndDate).ToString("dd/MM/yyyy"),
                                        PRYRA_ActiveFlag = a.PRYR_ActiveFlag
                                    }
              ).Distinct().OrderByDescending(t => t.PRYR_Id).ToArray();

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

        public OnlineProgramDTO editguest(OnlineProgramDTO data)
        {
            try
            {
                data.listg = _dbContext.ProgramsYearlyGuestDMO.Where(t => t.PRYRG_Id == data.PRYRG_Id).Distinct().ToArray();
                data.uploadfiles77 = (from a in _dbContext.ProgramsYearlyDMO
                                      from b in _dbContext.ProgramsYearlyGuestDMO
                                      where (a.PRYR_Id == b.PRYR_Id && a.MI_Id == data.MI_Id && b.PRYRG_Id == data.PRYRG_Id)
                                      select new OnlineProgramDTO
                                      {
                                          PRYR_Id = a.PRYR_Id,
                                          PRYRG_Id = b.PRYRG_Id,
                                          LPMTR_FileName = b.PRYRG_GuestProfileFileName,
                                          LPMTR_ResourceType = b.PRYRG_GuestProfileFilePath
                                      }).Distinct().ToArray();
                data.uploadfiles88 = (from a in _dbContext.ProgramsYearlyDMO
                                      from b in _dbContext.ProgramsYearlyGuestDMO
                                      where (a.PRYR_Id == b.PRYR_Id && a.MI_Id == data.MI_Id && b.PRYRG_Id == data.PRYRG_Id)
                                      select new OnlineProgramDTO
                                      {
                                          PRYR_Id = a.PRYR_Id,
                                          PRYRG_Id = b.PRYRG_Id,
                                          LPMTR_FileName = b.PRYRG_GuestSpeechName,
                                          LPMTR_ResourceType = b.PRYRG_GuestSpeechFilePath
                                      }).Distinct().ToArray();

                data.uploadfiles99 = (from a in _dbContext.ProgramsYearlyDMO
                                      from b in _dbContext.ProgramsYearlyGuestDMO
                                      where (a.PRYR_Id == b.PRYR_Id && a.MI_Id == data.MI_Id && b.PRYRG_Id == data.PRYRG_Id)
                                      select new OnlineProgramDTO
                                      {
                                          //PRYRG_Id = b.PRYRG_Id,

                                          PRYR_Id = a.PRYR_Id,
                                          PRYRG_Id = b.PRYRG_Id,
                                          LPMTR_FileName = b.PRYRG_GuestPhotoVideo,
                                          LPMTR_ResourceType = b.PRYRG_GuestPhotoVideoPath
                                      }).Distinct().ToArray();



                //if (data.PRYRG_Id > 0)
                //{
                //    var lorg3 = _dbContext.ProgramsYearlyGuestDMO.Where(t => t.PRYRG_Id == data.PRYRG_Id).ToList();
                //    foreach (var c in lorg3)
                //    {
                //        var checkresult = _dbContext.ProgramsYearlyGuestDMO.Single(a => a.PRYRG_Id == c.PRYRG_Id);
                //        _dbContext.Remove(checkresult);
                //    }
                //}

                //var contactexisttransaction = 0;
                //using (var dbCtxTxn = _dbContext.Database.BeginTransaction())
                //{
                //    try
                //    {
                //        contactexisttransaction = _dbContext.SaveChanges();
                //        dbCtxTxn.Commit();
                //        data.returnvaledit = "true";


                //    }
                //    catch (Exception ex)
                //    {
                //        dbCtxTxn.Rollback();
                //        data.returnvaledit = "false";
                //    }
                //}


            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
            return data;
        }

        public OnlineProgramDTO Savedata(OnlineProgramDTO data)
        {
            if (data.PRYR_Id > 0)
            {
                var res = _dbContext.ProgramsYearlyDMO.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.PRYR_Id != data.PRYR_Id && t.PRYR_ProgramName == data.programname).ToList();
                if (res.Count == 0)
                {
                    var objpge1 = _dbContext.ProgramsYearlyDMO.Single(t => t.PRYR_Id == data.PRYR_Id);
                    objpge1.ASMAY_Id = data.ASMAY_Id;
                    objpge1.MI_Id = data.MI_Id;
                    objpge1.PRYR_ProgramName = data.programname;
                    objpge1.PRYR_StartDate = data.Fromdate;
                    objpge1.PRYR_EndDate = data.Todate;
                    objpge1.PRYR_StartTime = data.start_time;
                    objpge1.PRYR_EndTime = data.end_time;
                    objpge1.PRYR_ProgramDescription = data.description;


                    objpge1.UpdatedDate = DateTime.Now;

                    objpge1.PRYR_UpdatedBy = data.UserId;
                    if (data.pgTempDTO1.Length > 0)
                    {
                        if (data.pgTempDTO1[0].LPMTR_Resources != null)
                        {
                            objpge1.PRYR_ProgramChart = data.pgTempDTO1[0].file_name;
                            objpge1.PRYR_ProgramChartPath = data.pgTempDTO1[0].LPMTR_Resources;
                        }
                    }

                    objpge1.PRYR_PrgramLevel = data.PRMTLE_Id;
                    objpge1.PRYR_ProgramTypeId = data.PRMTY_Id;
                    objpge1.PRYR_PrgramConvenor = data.PRYRG_GuestName;
                    objpge1.PRYR_TotalParticipants = data.strength;


                    if (data.pgTempDTO3.Length > 0)
                    {
                        if (data.pgTempDTO3[0].LPMTR_Resources != null)
                        {
                            objpge1.PRYR_ProgramInvitation = data.pgTempDTO3[0].LPMTR_Resources;
                        }
                    }

                    if (data.pgTempDTO.Length > 0)
                    {
                        if (data.pgTempDTO[0].LPMTR_Resources != null)
                        {
                            objpge1.PRYR_ParticipantList = data.pgTempDTO[0].file_name;
                            objpge1.PRYR_PListPath = data.pgTempDTO[0].LPMTR_Resources;
                        }
                    }

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

                    _dbContext.Update(objpge1);

                    var actv = _dbContext.ProgramsYearlyActivitiesDMO.Single(t => t.PRYR_Id == data.PRYR_Id);
                    actv.PRYR_Id = objpge1.PRYR_Id;
                    actv.PRYRA_ActivityName = data.club;
                    actv.PRYRA_StartTime = data.start_time;
                    actv.PRYRA_Description = data.description;

                    actv.UpdatedDate = DateTime.Now;
                    actv.PRYRA_UpdatedBy = data.UserId;
                    _dbContext.Update(actv);

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
                                    obj2.PRYRF_UpdatedBy = data.UserId;
                                    obj2.UpdatedDate = DateTime.Now;
                                    obj2.PRYRF_ActiveFlag = true;
                                    obj2.CreatedDate = DateTime.Now;
                                    obj2.PRYRF_CreatedBy = data.UserId;

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

                                    obj2.PRYRF_UpdatedBy = data.UserId;
                                    obj2.PRYRF_ActiveFlag = true;
                                    obj2.CreatedDate = DateTime.Now;
                                    obj2.PRYRF_CreatedBy = data.UserId;
                                    obj2.UpdatedDate = DateTime.Now;

                                    _dbContext.Add(obj2);
                                }
                            }
                        }
                    }



                    if (data.pgTempDTO8.Length > 0)
                    {
                        for (int i = 0; i < data.pgTempDTO8.Length; i++)
                        {
                            if (data.pgTempDTO8[i].Speech != null && data.pgTempDTO8[i].profile != null)
                            {

                                if (data.pgTempDTO8[i].PRYRG_Id > 0)
                                {

                                    var lorg3 = _dbContext.ProgramsYearlyGuestDMO.Where(t => t.PRYRG_Id == data.pgTempDTO8[i].PRYRG_Id).ToList();
                                    foreach (var c in lorg3)
                                    {
                                        var checkresult = _dbContext.ProgramsYearlyGuestDMO.Single(a => a.PRYRG_Id == c.PRYRG_Id);
                                        _dbContext.Remove(checkresult);
                                    }


                                    var contactexisttransaction = 0;
                                    using (var dbCtxTxn = _dbContext.Database.BeginTransaction())
                                    {
                                        try
                                        {
                                            contactexisttransaction = _dbContext.SaveChanges();
                                            dbCtxTxn.Commit();
                                            data.returnvaledit = "true";


                                        }
                                        catch (Exception ex)
                                        {
                                            dbCtxTxn.Rollback();
                                            data.returnvaledit = "false";
                                        }
                                    }







                                    ProgramsYearlyGuestDMO obb6 = new ProgramsYearlyGuestDMO();
                                    obb6.PRYRG_GuestName = data.pgTempDTO8[i].name;
                                    obb6.PRYRG_GuestPhoneNo = data.pgTempDTO8[i].number;
                                    obb6.PRYRG_GuestAddress = data.pgTempDTO8[i].address;
                                    obb6.PRYRG_GuestType = data.pgTempDTO8[i].type;
                                    obb6.PRYRG_GuestEmailId = data.pgTempDTO8[i].email;
                                    obb6.PRYR_Id = objpge1.PRYR_Id;
                                    obb6.PRYR_Id = data.PRYR_Id;
                                    obb6.PRYRG_ActiveFlag = true;
                                    obb6.PRYRG_CreatedBy = data.UserId;
                                    obb6.PRYRG_UpdatedBy = data.UserId;
                                    obb6.CreatedDate = DateTime.Now;
                                    obb6.UpdatedDate = DateTime.Now;
                                    if (data.pgTempDTO8[i].Speech.Length > 0)
                                    {
                                        obb6.PRYRG_GuestSpeechFilePath = data.pgTempDTO8[i].Speech[0].LPMTR_Resources;
                                        obb6.PRYRG_GuestSpeechName = data.pgTempDTO8[i].Speech[0].file_name;
                                    }
                                    if (data.pgTempDTO8[i].profile.Length > 0)
                                    {
                                        obb6.PRYRG_GuestProfileFilePath = data.pgTempDTO8[i].profile[0].LPMTR_Resources;
                                        obb6.PRYRG_GuestProfileFileName = data.pgTempDTO8[i].profile[0].file_name;
                                    }
                                    if (data.pgTempDTO8[i].Programgst.Length > 0)
                                    {
                                        obb6.PRYRG_GuestPhotoVideoPath = data.pgTempDTO8[i].Programgst[0].LPMTR_Resources;
                                        obb6.PRYRG_GuestPhotoVideo = data.pgTempDTO8[i].Programgst[0].file_name;
                                    }

                                    _dbContext.Add(obb6);


                                }


                                else
                                {
                                    ProgramsYearlyGuestDMO obb6 = new ProgramsYearlyGuestDMO();
                                    obb6.PRYRG_GuestName = data.pgTempDTO8[i].name;
                                    obb6.PRYRG_GuestPhoneNo = data.pgTempDTO8[i].number;
                                    obb6.PRYRG_GuestAddress = data.pgTempDTO8[i].address;
                                    obb6.PRYRG_GuestType = data.pgTempDTO8[i].type;
                                    obb6.PRYRG_GuestEmailId = data.pgTempDTO8[i].email;
                                    obb6.PRYR_Id = objpge1.PRYR_Id;
                                    obb6.PRYRG_ActiveFlag = true;
                                    obb6.PRYRG_CreatedBy = data.UserId;
                                    obb6.PRYRG_UpdatedBy = data.UserId;
                                    obb6.CreatedDate = DateTime.Now;
                                    obb6.UpdatedDate = DateTime.Now;
                                    if (data.pgTempDTO8[i].Speech.Length > 0)
                                    {
                                        obb6.PRYRG_GuestSpeechFilePath = data.pgTempDTO8[i].Speech[0].LPMTR_Resources;
                                        obb6.PRYRG_GuestSpeechName = data.pgTempDTO8[i].Speech[0].file_name;
                                    }
                                    if (data.pgTempDTO8[i].profile.Length > 0)
                                    {
                                        obb6.PRYRG_GuestProfileFilePath = data.pgTempDTO8[i].profile[0].LPMTR_Resources;
                                        obb6.PRYRG_GuestProfileFileName = data.pgTempDTO8[i].profile[0].file_name;
                                    }
                                    if (data.pgTempDTO8[i].Programgst.Length > 0)
                                    {
                                        obb6.PRYRG_GuestPhotoVideoPath = data.pgTempDTO8[i].Programgst[0].LPMTR_Resources;
                                        obb6.PRYRG_GuestPhotoVideo = data.pgTempDTO8[i].Programgst[0].file_name;
                                    }

                                    _dbContext.Add(obb6);
                                }


                            }
                        }
                    }






                    //if (data.pgTempDTO8.Length > 0)
                    //{


                    //    for (int i = 0; i < data.pgTempDTO8.Length; i++)
                    //    {
                    //        if (data.pgTempDTO8[i].Programgst != null)
                    //        {
                    //            for (int t = 0; t < data.pgTempDTO8[i].Programgst.Length; t++)
                    //            {
                    //                ProgramsYearlyGuestDMO obb6 = new ProgramsYearlyGuestDMO();
                    //                obb6.PRYRG_GuestName = data.pgTempDTO8[i].name;
                    //                obb6.PRYRG_GuestPhoneNo = data.pgTempDTO8[i].number;
                    //                obb6.PRYRG_GuestAddress = data.pgTempDTO8[i].address;
                    //                obb6.PRYRG_GuestType = data.pgTempDTO8[i].type;
                    //                obb6.PRYRG_GuestEmailId = data.pgTempDTO8[i].email;
                    //                obb6.PRYR_Id = objpge1.PRYR_Id;
                    //                obb6.PRYRG_ActiveFlag = true;
                    //                obb6.PRYRG_CreatedBy = data.UserId;
                    //                obb6.PRYRG_UpdatedBy = data.UserId;
                    //                obb6.CreatedDate = DateTime.Now;
                    //                obb6.UpdatedDate = DateTime.Now;
                    //                if (data.pgTempDTO8[i].Speech.Length > 0)
                    //                {
                    //                    obb6.PRYRG_GuestSpeechFilePath = data.pgTempDTO8[i].Speech[0].LPMTR_Resources;
                    //                    obb6.PRYRG_GuestSpeechName = data.pgTempDTO8[i].Speech[0].file_name;
                    //                }
                    //                if (data.pgTempDTO8[i].profile.Length > 0)
                    //                {
                    //                    obb6.PRYRG_GuestProfileFilePath = data.pgTempDTO8[i].profile[0].LPMTR_Resources;
                    //                    obb6.PRYRG_GuestProfileFileName = data.pgTempDTO8[i].profile[0].file_name;
                    //                }

                    //                obb6.PRYRG_GuestPhotoVideoPath = data.pgTempDTO8[i].Programgst[t].LPMTR_Resources;
                    //                obb6.PRYRG_GuestPhotoVideo = data.pgTempDTO8[i].Programgst[t].file_name;

                    //                _dbContext.Add(obb6);

                    //            }
                    //        }
                    //    }
                    //}


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
                var res = _dbContext.ProgramsYearlyDMO.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id
                && t.PRYR_ProgramName == data.programname).ToList();

                if (res.Count == 0)
                {
                    try
                    {
                        ProgramsYearlyDMO objpge1 = new ProgramsYearlyDMO();

                        objpge1.ASMAY_Id = data.ASMAY_Id;
                        objpge1.MI_Id = data.MI_Id;
                        objpge1.PRYR_ProgramName = data.programname;
                        objpge1.PRYR_StartDate = data.Fromdate;
                        objpge1.PRYR_EndDate = data.Todate;
                        objpge1.PRYR_StartTime = data.start_time;
                        objpge1.PRYR_EndTime = data.end_time;
                        objpge1.PRYR_ProgramDescription = data.description;
                        objpge1.PRYR_ActiveFlag = true;
                        objpge1.CreatedDate = DateTime.Now;
                        objpge1.UpdatedDate = DateTime.Now;
                        objpge1.PRYR_CreatedBy = data.UserId;
                        objpge1.PRYR_UpdatedBy = data.UserId;

                        if (data.pgTempDTO1.Length > 0)
                        {
                            if (data.pgTempDTO1[0].LPMTR_Resources != null)
                            {
                                objpge1.PRYR_ProgramChart = data.pgTempDTO1[0].file_name;
                                objpge1.PRYR_ProgramChartPath = data.pgTempDTO1[0].LPMTR_Resources;
                            }
                        }

                        objpge1.PRYR_PrgramLevel = data.PRMTLE_Id;
                        objpge1.PRYR_ProgramTypeId = data.PRMTY_Id;
                        objpge1.PRYR_PrgramConvenor = data.PRYRG_GuestName;
                        objpge1.PRYR_TotalParticipants = data.strength;

                        if (data.pgTempDTO3.Length > 0)
                        {
                            if (data.pgTempDTO3[0].LPMTR_Resources != null)
                            {
                                objpge1.PRYR_ProgramInvitation = data.pgTempDTO3[0].LPMTR_Resources;
                            }
                        }

                        if (data.pgTempDTO.Length > 0)
                        {
                            if (data.pgTempDTO[0].LPMTR_Resources != null)
                            {
                                objpge1.PRYR_ParticipantList = data.pgTempDTO[0].file_name;
                                objpge1.PRYR_PListPath = data.pgTempDTO[0].LPMTR_Resources;
                            }
                        }

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

                        //_dbContext.ProgramsYearlyDMO.Add(objpge1);
                        _dbContext.Add(objpge1);


                        ProgramsYearlyActivitiesDMO act = new ProgramsYearlyActivitiesDMO();
                        act.PRYR_Id = objpge1.PRYR_Id;
                        act.PRYRA_ActivityName = data.club;
                        act.PRYRA_StartTime = data.start_time;
                        act.PRYRA_Description = data.description;
                        act.PRYRA_ActiveFlag = true;
                        act.UpdatedDate = DateTime.Now;
                        act.PRYRA_UpdatedBy = data.UserId;
                        act.CreatedDate = DateTime.Now;
                        act.PRYRA_CreatedBy = data.UserId;
                        _dbContext.Add(act);


                        foreach (pgTempDTO7 ph1 in data.pgTempDTO7)
                        {
                            if (data.pgTempDTO7[0].LPMTR_Resources != null)
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

                                _dbContext.Add(objpge2);
                            }
                        }

                        if (data.GuestName != null)
                        {

                            for (int i = 0; i < data.pgTempDTO99.Length; i++)
                            //foreach (pgTempDTO99 ph1 in data.pgTempDTO99)                                           
                            {
                                if (data.pgTempDTO99[i].LPMTR_Resources != null)

                                {
                                    ProgramsYearlyGuestDMO obb = new ProgramsYearlyGuestDMO();

                                    obb.PRYRG_GuestName = data.GuestName;
                                    obb.PRYRG_GuestPhoneNo = data.gno;
                                    obb.PRYRG_GuestAddress = data.gaddress;
                                    obb.PRYRG_GuestType = data.gtype;
                                    obb.PRYRG_GuestEmailId = data.emailid;
                                    obb.PRYR_Id = objpge1.PRYR_Id;
                                    obb.PRYRG_ActiveFlag = true;
                                    obb.PRYRG_CreatedBy = data.UserId;
                                    obb.PRYRG_UpdatedBy = data.UserId;
                                    obb.CreatedDate = DateTime.Now;
                                    obb.UpdatedDate = DateTime.Now;

                                    if (data.pgTempDTO6.Length > 0)
                                    {
                                        //for (int f = 0; f < data.pgTempDTO.Length; f++)
                                        //{
                                        if (data.pgTempDTO6[0].LPMTR_Resources != null)
                                        {
                                            obb.PRYRG_GuestProfileFileName = data.pgTempDTO4[0].file_name;
                                            obb.PRYRG_GuestProfileFilePath = data.pgTempDTO4[0].LPMTR_Resources;
                                        }
                                        //}
                                    }
                                    if (data.pgTempDTO5.Length > 0)
                                    {
                                        if (data.pgTempDTO5[0].LPMTR_Resources != null)
                                        {
                                            obb.PRYRG_GuestSpeechName = data.pgTempDTO4[0].file_name;
                                            obb.PRYRG_GuestSpeechFilePath = data.pgTempDTO4[0].LPMTR_Resources;
                                        }
                                    }

                                    //ProgramsYearlyFileDMO objpge2 = new ProgramsYearlyFileDMO();
                                    //obb.PRYR_Id = objpge1.PRYR_Id;
                                    obb.PRYRG_GuestPhotoVideoPath = data.pgTempDTO99[i].LPMTR_Resources;
                                    obb.PRYRG_GuestPhotoVideo = data.pgTempDTO99[i].file_name;

                                    // _dbContext.ProgramsYearlyFileDMO.Add(objpge2);
                                    _dbContext.Add(obb);
                                }
                            }

                        }










                        if (data.pgTempDTO8.Length > 0)
                        {
                            for (int i = 0; i < data.pgTempDTO8.Length; i++)
                            {
                                if (data.pgTempDTO8[i].Speech != null && data.pgTempDTO8[i].profile != null)
                                {
                                    ProgramsYearlyGuestDMO obb6 = new ProgramsYearlyGuestDMO();
                                    obb6.PRYRG_GuestName = data.pgTempDTO8[i].name;
                                    obb6.PRYRG_GuestPhoneNo = data.pgTempDTO8[i].number;
                                    obb6.PRYRG_GuestAddress = data.pgTempDTO8[i].address;
                                    obb6.PRYRG_GuestType = data.pgTempDTO8[i].type;
                                    obb6.PRYRG_GuestEmailId = data.pgTempDTO8[i].email;
                                    obb6.PRYR_Id = objpge1.PRYR_Id;
                                    obb6.PRYRG_ActiveFlag = true;
                                    obb6.PRYRG_CreatedBy = data.UserId;
                                    obb6.PRYRG_UpdatedBy = data.UserId;
                                    obb6.CreatedDate = DateTime.Now;
                                    obb6.UpdatedDate = DateTime.Now;
                                    if (data.pgTempDTO8[i].Speech.Length > 0)
                                    {
                                        obb6.PRYRG_GuestSpeechFilePath = data.pgTempDTO8[i].Speech[0].LPMTR_Resources;
                                        obb6.PRYRG_GuestSpeechName = data.pgTempDTO8[i].Speech[0].file_name;
                                    }
                                    if (data.pgTempDTO8[i].profile.Length > 0)
                                    {
                                        obb6.PRYRG_GuestProfileFilePath = data.pgTempDTO8[i].profile[0].LPMTR_Resources;
                                        obb6.PRYRG_GuestProfileFileName = data.pgTempDTO8[i].profile[0].file_name;
                                    }
                                    if (data.pgTempDTO8[i].Programgst.Length > 0)
                                    {
                                        obb6.PRYRG_GuestPhotoVideoPath = data.pgTempDTO8[i].Programgst[0].LPMTR_Resources;
                                        obb6.PRYRG_GuestPhotoVideo = data.pgTempDTO8[i].Programgst[0].file_name;
                                    }

                                    _dbContext.Add(obb6);
                                }
                            }
                        }





                        //if (data.pgTempDTO8.Length > 0)
                        //{

                        //    for (int j = 0; j < data.pgTempDTO8.Length; j++)//3
                        //    {
                        //        for (int i = 0; i < data.pgTempDTO8[j].Programgst.Length; i++)
                        //        {
                        //            ProgramsYearlyGuestDMO obb6 = new ProgramsYearlyGuestDMO();
                        //            obb6.PRYRG_GuestName = data.pgTempDTO8[j].name;
                        //            obb6.PRYRG_GuestPhoneNo = data.pgTempDTO8[j].number;
                        //            obb6.PRYRG_GuestAddress = data.pgTempDTO8[j].address;
                        //            obb6.PRYRG_GuestType = data.pgTempDTO8[j].type;
                        //            obb6.PRYRG_GuestEmailId = data.pgTempDTO8[j].email;
                        //            obb6.PRYR_Id = objpge1.PRYR_Id;
                        //            obb6.PRYRG_ActiveFlag = true;
                        //            obb6.PRYRG_CreatedBy = data.UserId;
                        //            obb6.PRYRG_UpdatedBy = data.UserId;
                        //            obb6.CreatedDate = DateTime.Now;
                        //            obb6.UpdatedDate = DateTime.Now;
                        //            if (data.pgTempDTO8[j].Speech.Length > 0)
                        //            {
                        //                obb6.PRYRG_GuestSpeechFilePath = data.pgTempDTO8[j].Speech[0].LPMTR_Resources;
                        //                obb6.PRYRG_GuestSpeechName = data.pgTempDTO8[j].Speech[0].file_name;
                        //            }
                        //            if (data.pgTempDTO8[j].profile.Length > 0)
                        //            {
                        //                obb6.PRYRG_GuestProfileFilePath = data.pgTempDTO8[j].profile[0].LPMTR_Resources;
                        //                obb6.PRYRG_GuestProfileFileName = data.pgTempDTO8[j].profile[0].file_name;
                        //            }

                        //            obb6.PRYRG_GuestPhotoVideoPath = data.pgTempDTO8[j].Programgst[i].LPMTR_Resources;
                        //            obb6.PRYRG_GuestPhotoVideo = data.pgTempDTO8[j].Programgst[i].file_name;
                        //            _dbContext.Add(obb6);
                        //        }
                        //    }
                        //}




                        try
                        {
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
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                        }
                    }

                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                }
                else
                {
                    data.message = "Duplicate";
                }
            }
            return data;
        }
        public OnlineProgramDTO getdetails(OnlineProgramDTO data)
        {
            try
            {

                data.programlist = (from a in _dbContext.ProgramsYearlyDMO


                                    where (a.MI_Id == data.MI_Id && a.PRYR_Id == data.PRYR_Id)
                                    select new OnlineProgramDTO
                                    {
                                        PRYR_Id = a.PRYR_Id,
                                        ASMAY_Id = a.ASMAY_Id,
                                        programname = a.PRYR_ProgramName,
                                        start_time = a.PRYR_StartTime,
                                        end_time = a.PRYR_EndTime,
                                        Fromdate = a.PRYR_StartDate,
                                        Todate = Convert.ToDateTime(a.PRYR_EndDate),
                                        PRYRA_ActiveFlag = a.PRYR_ActiveFlag,
                                        PRMTLE_Id = a.PRYR_PrgramLevel,
                                        PRMTY_Id = a.PRYR_ProgramTypeId,
                                        strength = a.PRYR_TotalParticipants,
                                        Org_name = a.PRYR_PrgramConvenor,
                                        description = a.PRYR_ProgramDescription,

                                    }).Distinct().OrderByDescending(t => t.PRYR_Id).ToArray();

                data.fillActivities1 = _dbContext.ProgramsYearlyActivitiesDMO.Where(t => t.PRYR_Id == data.PRYR_Id).Distinct().ToArray();


                data.uploadfiles11 = (from a in _dbContext.ProgramsYearlyDMO
                                      where (a.MI_Id == data.MI_Id && a.PRYR_Id == data.PRYR_Id)
                                      select new OnlineProgramDTO
                                      {
                                          PRYR_Id = a.PRYR_Id,
                                          LPMTR_FileName = a.PRYR_ParticipantList,
                                          LPMTR_ResourceType = a.PRYR_PListPath
                                      }).Distinct().ToArray();


                data.uploadfiles22 = (from a in _dbContext.ProgramsYearlyDMO
                                      where (a.MI_Id == data.MI_Id && a.PRYR_Id == data.PRYR_Id)
                                      select new OnlineProgramDTO
                                      {
                                          PRYR_Id = a.PRYR_Id,
                                          LPMTR_FileName = a.PRYR_WinnerList,
                                          LPMTR_ResourceType = a.PRYR_WListPath
                                      }).Distinct().ToArray();



                data.uploadfiles33 = (from a in _dbContext.ProgramsYearlyDMO
                                      where (a.MI_Id == data.MI_Id && a.PRYR_Id == data.PRYR_Id)
                                      select new OnlineProgramDTO
                                      {
                                          PRYR_Id = a.PRYR_Id,
                                          LPMTR_FileName = a.PRYR_AccountStatement,
                                          LPMTR_ResourceType = a.PRYR_ASPath
                                      }).Distinct().ToArray();



                data.uploadfiles44 = (from a in _dbContext.ProgramsYearlyDMO
                                      where (a.MI_Id == data.MI_Id && a.PRYR_Id == data.PRYR_Id)
                                      select new OnlineProgramDTO
                                      {
                                          PRYR_Id = a.PRYR_Id,
                                          LPMTR_FileName = a.PRYR_ProgramChart,
                                          LPMTR_ResourceType = a.PRYR_ProgramChartPath
                                      }).Distinct().ToArray();

                data.uploadfiles55 = (from a in _dbContext.ProgramsYearlyDMO
                                      where (a.MI_Id == data.MI_Id && a.PRYR_Id == data.PRYR_Id)
                                      select new OnlineProgramDTO
                                      {
                                          PRYR_Id = a.PRYR_Id,
                                          LPMTR_ResourceType = a.PRYR_ProgramInvitation
                                      }).Distinct().ToArray();

                data.uploadfiles66 = (from a in _dbContext.ProgramsYearlyDMO
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

                if (data.GuestName != null)
                {
                    data.msg22 = "common";

                    data.guest = _dbContext.ProgramsYearlyGuestDMO.Where(t => t.PRYR_Id == data.PRYR_Id).Distinct().ToArray();

                    data.uploadfiles77 = (from a in _dbContext.ProgramsYearlyDMO
                                          from b in _dbContext.ProgramsYearlyGuestDMO
                                          where (a.PRYR_Id == b.PRYR_Id && a.MI_Id == data.MI_Id && a.PRYR_Id == data.PRYR_Id)
                                          select new OnlineProgramDTO
                                          {
                                              PRYR_Id = a.PRYR_Id,
                                              LPMTR_FileName = b.PRYRG_GuestProfileFileName,
                                              LPMTR_ResourceType = b.PRYRG_GuestProfileFilePath
                                          }).Distinct().ToArray();
                    data.uploadfiles88 = (from a in _dbContext.ProgramsYearlyDMO
                                          from b in _dbContext.ProgramsYearlyGuestDMO
                                          where (a.PRYR_Id == b.PRYR_Id && a.MI_Id == data.MI_Id && a.PRYR_Id == data.PRYR_Id)
                                          select new OnlineProgramDTO
                                          {
                                              PRYR_Id = a.PRYR_Id,
                                              LPMTR_FileName = b.PRYRG_GuestSpeechName,
                                              LPMTR_ResourceType = b.PRYRG_GuestSpeechFilePath
                                          }).Distinct().ToArray();

                    data.uploadfiles99 = (from a in _dbContext.ProgramsYearlyDMO
                                          from b in _dbContext.ProgramsYearlyGuestDMO
                                          where (a.PRYR_Id == b.PRYR_Id && a.MI_Id == data.MI_Id && a.PRYR_Id == data.PRYR_Id)
                                          select new OnlineProgramDTO
                                          {
                                              //PRYRG_Id = b.PRYRG_Id,

                                              PRYR_Id = a.PRYR_Id,
                                              LPMTR_FileName = b.PRYRG_GuestPhotoVideo,
                                              LPMTR_ResourceType = b.PRYRG_GuestPhotoVideoPath
                                          }).Distinct().ToArray();
                }
                data.msg22 = "grid";

                //data.guestgrid = _dbContext.ProgramsYearlyGuestDMO.Where(t => t.PRYR_Id == data.PRYR_Id).Distinct().ToArray();

                data.guestgrid = (from a in _dbContext.ProgramsYearlyGuestDMO
                                  where (a.PRYR_Id == data.PRYR_Id)
                                  select new OnlineProgramDTO
                                  {
                                      PRYRG_Id = a.PRYRG_Id,
                                      name = a.PRYRG_GuestName,
                                      email = a.PRYRG_GuestEmailId,
                                      address = a.PRYRG_GuestAddress,
                                      number = a.PRYRG_GuestPhoneNo,
                                      type = a.PRYRG_GuestType,

                                  }).Distinct().ToArray();



                data.uploadfiles77 = (from a in _dbContext.ProgramsYearlyDMO
                                      from b in _dbContext.ProgramsYearlyGuestDMO
                                      where (a.PRYR_Id == b.PRYR_Id && a.MI_Id == data.MI_Id && a.PRYR_Id == data.PRYR_Id)
                                      select new OnlineProgramDTO
                                      {
                                          PRYR_Id = a.PRYR_Id,
                                          name = b.PRYRG_GuestName,
                                          PRYRG_GuestProfileFileName = b.PRYRG_GuestProfileFileName,
                                          PRYRG_GuestProfileFilePath = b.PRYRG_GuestProfileFilePath,
                                      }).Distinct().ToArray();

                data.uploadfiles88 = (from a in _dbContext.ProgramsYearlyDMO
                                      from b in _dbContext.ProgramsYearlyGuestDMO
                                      where (a.PRYR_Id == b.PRYR_Id && a.MI_Id == data.MI_Id && a.PRYR_Id == data.PRYR_Id)
                                      select new OnlineProgramDTO
                                      {
                                          PRYR_Id = a.PRYR_Id,
                                          name = b.PRYRG_GuestName,
                                          PRYRG_GuestSpeech = b.PRYRG_GuestSpeechName,
                                          PRYRG_GuestSpeechFilePath = b.PRYRG_GuestSpeechFilePath,
                                      }).Distinct().ToArray();

                data.uploadfiles99 = (from a in _dbContext.ProgramsYearlyDMO
                                      from b in _dbContext.ProgramsYearlyGuestDMO
                                      where (a.PRYR_Id == b.PRYR_Id && a.MI_Id == data.MI_Id && a.PRYR_Id == data.PRYR_Id)
                                      select new OnlineProgramDTO
                                      {
                                          //PRYRG_Id = b.PRYRG_Id,
                                          PRYR_Id = a.PRYR_Id,
                                          name = b.PRYRG_GuestName,
                                          PRYRG_GuestPhotoVideo = b.PRYRG_GuestPhotoVideo,
                                          PRYRG_GuestPhotoVideoPath = b.PRYRG_GuestPhotoVideoPath,
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
                foreach (var bv in lorg4)
                {
                    var checkresult = _dbContext.ProgramsYearlyActivitiesDMO.Single(a => a.PRYRA_Id == bv.PRYRA_Id);
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

        public OnlineProgramDTO removeNewsiblinguest(OnlineProgramDTO data)
        {
            try
            {


                //var removefile = _dbContext.ProgramsYearlyGuestDMO.Where(t => t.PRYRG_GuestName == data.name3&&t.PRYRG_GuestType==data.type3&&t.PRYRG_GuestAddress==data.address3).Distinct().ToList();
                var removefile = _dbContext.ProgramsYearlyGuestDMO.Where(t => t.PRYRG_Id == data.PRYRG_Id).Distinct().ToList();
                if (removefile.Count > 0)
                {
                    foreach (var item in removefile)
                    {
                        _dbContext.Remove(item);
                    }
                }
                _dbContext.SaveChanges();
                data.returnval = "true";

                //var lorg333 = _dbContext.ProgramsYearlyGuestDMO.Where(t => t.PRYRG_GuestType==data.type3 && t.PRYRG_GuestName==data.name3&&t.PRYRG_GuestAddress==data.address3);

                //foreach (var c in lorg333)
                //{
                //    var checkresult = _dbContext.ProgramsYearlyGuestDMO.Single(a => a.PRYRG_GuestName == c.PRYRG_GuestName&&a.PRYRG_GuestType==c.PRYRG_GuestType&&a.PRYRG_GuestAddress==c.PRYRG_GuestAddress);
                //    _dbContext.Remove(checkresult);

                //}


                //var contactexisttransaction = 0;
                //using (var dbCtxTxn = _dbContext.Database.BeginTransaction())
                //{
                //    try
                //    {
                //        contactexisttransaction = _dbContext.SaveChanges();
                //        dbCtxTxn.Commit();
                //        data.returnval = "true";

                //        //data.sss = (from a in _dbContext.ProgramsYearlyGuestDMO
                //        //                  where (a.PRYR_Id == data.PRYR_Id)
                //        //                  select new OnlineProgramDTO
                //        //                  {

                //        //                      name = a.PRYRG_GuestName,
                //        //                      email = a.PRYRG_GuestEmailId,
                //        //                      address = a.PRYRG_GuestAddress,
                //        //                      number = a.PRYRG_GuestPhoneNo,
                //        //                      type = a.PRYRG_GuestType,

                //        //                  }).Distinct().ToArray();


                //    }
                //    catch (Exception ex)
                //    {
                //        dbCtxTxn.Rollback();
                //        data.returnval = "false";
                //    }
                //}


            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
            return data;
        }
        public OnlineProgramDTO viewuploadflies(OnlineProgramDTO data)
        {
            try
            {
                //over
                if (data.description == "chart")
                {
                    data.uploadfiles = (from a in _dbContext.ProgramsYearlyDMO
                                        where (a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.PRYR_Id == data.PRYR_Id)
                                        select new OnlineProgramDTO
                                        {
                                            PRYR_Id = a.PRYR_Id,
                                            LPMTR_FileName = a.PRYR_ProgramChart,
                                            LPMTR_ResourceType = a.PRYR_ProgramChartPath
                                        }).Distinct().ToArray();
                }
                //over
                else if (data.description == "invitation")
                {
                    data.uploadfiles = (from a in _dbContext.ProgramsYearlyDMO
                                        where (a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.PRYR_Id == data.PRYR_Id)
                                        select new OnlineProgramDTO
                                        {
                                            PRYR_Id = a.PRYR_Id,
                                            LPMTR_ResourceType = a.PRYR_ProgramInvitation,
                                            LPMTR_FileName = a.PRYR_ProgramChart,
                                        }).Distinct().ToArray();
                }


                //over
                else if (data.description == "list")
                {
                    data.uploadfiles = (from a in _dbContext.ProgramsYearlyDMO
                                        where (a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.PRYR_Id == data.PRYR_Id)
                                        select new OnlineProgramDTO
                                        {
                                            PRYR_Id = a.PRYR_Id,
                                            LPMTR_FileName = a.PRYR_ParticipantList,
                                            LPMTR_ResourceType = a.PRYR_PListPath
                                        }).Distinct().ToArray();
                }
                //over
                else if (data.description == "account")
                {
                    data.uploadfiles = (from a in _dbContext.ProgramsYearlyDMO
                                        where (a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.PRYR_Id == data.PRYR_Id)
                                        select new OnlineProgramDTO
                                        {
                                            PRYR_Id = a.PRYR_Id,
                                            LPMTR_FileName = a.PRYR_AccountStatement,
                                            LPMTR_ResourceType = a.PRYR_ASPath
                                        }).Distinct().ToArray();
                }
                //over
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
                //over
                else if (data.description == "programphoto")
                {
                    data.uploadfiles = (from a in _dbContext.ProgramsYearlyDMO
                                        from b in _dbContext.ProgramsYearlyFileDMO
                                        where (a.PRYR_Id == b.PRYR_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.PRYR_Id == data.PRYR_Id)
                                        select new OnlineProgramDTO
                                        {
                                            PRYRF_Id = b.PRYRF_Id,
                                            PRYR_Id = a.PRYR_Id,
                                            LPMTR_FileName = b.PRYRF_FileName,
                                            LPMTR_Filetype = b.PRYRF_FileType,
                                            LPMTR_ResourceType = b.PRYRF_FilePath
                                        }).Distinct().ToArray();
                }
                //over
                else if (data.description == "gprofile")
                {
                    data.uploadfiles = (from a in _dbContext.ProgramsYearlyDMO
                                        from b in _dbContext.ProgramsYearlyGuestDMO
                                        where (a.PRYR_Id == b.PRYR_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.PRYR_Id == data.PRYR_Id)
                                        select new OnlineProgramDTO
                                        {
                                            PRYR_Id = a.PRYR_Id,
                                            LPMTR_FileName = b.PRYRG_GuestProfileFileName,
                                            LPMTR_ResourceType = b.PRYRG_GuestProfileFilePath
                                        }).Distinct().ToArray();
                }
                //over
                else if (data.description == "gspeech")
                {
                    data.uploadfiles = (from a in _dbContext.ProgramsYearlyDMO
                                        from b in _dbContext.ProgramsYearlyGuestDMO
                                        where (a.PRYR_Id == b.PRYR_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.PRYR_Id == data.PRYR_Id)
                                        select new OnlineProgramDTO
                                        {
                                            PRYR_Id = a.PRYR_Id,
                                            LPMTR_FileName = b.PRYRG_GuestSpeechName,
                                            LPMTR_ResourceType = b.PRYRG_GuestSpeechFilePath
                                        }).Distinct().ToArray();
                }
                //over
                else
                {
                    data.uploadfiles = (from a in _dbContext.ProgramsYearlyDMO
                                        from b in _dbContext.ProgramsYearlyGuestDMO
                                        where (a.PRYR_Id == b.PRYR_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.PRYR_Id == data.PRYR_Id)
                                        select new OnlineProgramDTO
                                        {
                                            //PRYRG_Id = b.PRYRG_Id,
                                            PRYR_Id = a.PRYR_Id,
                                            LPMTR_FileName = b.PRYRG_GuestPhotoVideo,
                                            LPMTR_ResourceType = b.PRYRG_GuestPhotoVideoPath
                                        }).Distinct().ToArray();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }


        #region        
        //public OnlineProgramDTO getloaddata(OnlineProgramDTO data)
        //{
        //    try
        //    {

        //        data.fillActivities = _dbContext.ProgramsYearlyActivitiesDMO.ToArray();

        //        data.Typelist = _dbContext.ProgramsMasterTypeDMO.Where(a => a.MI_Id == data.MI_Id).ToArray();

        //        data.levellist = _dbContext.ProgramsMasterLevelDMO.Where(a => a.MI_Id == data.MI_Id).ToArray();



        //        //data.programlist = (from a in _dbContext.ProgramsYearlyDMO
        //        //                    from b in _dbContext.ProgramsYearlyFileDMO
        //        //                    from c in _dbContext.ProgramsYearlyGuestDMO
        //        //                    from d in _dbContext.AcademicYear
        //        //                    where (a.PRYR_Id == b.PRYR_Id && a.PRYR_Id == c.PRYR_Id && a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && d.ASMAY_Id == a.ASMAY_Id)
        //        //                    select new OnlineProgramDTO
        //        //                    {
        //        //                        PRYR_Id = a.PRYR_Id,
        //        //                        ASMAY_Id=a.ASMAY_Id,
        //        //                        ASMAY_Year = d.ASMAY_Year,
        //        //                        programname = a.PRYR_ProgramName,
        //        //                        start_time = a.PRYR_StartTime,
        //        //                        end_time = a.PRYR_EndTime,
        //        //                        club = a.PRYR_StartDate.ToString("dd/MM/yyyy"),
        //        //                        Org_name = Convert.ToDateTime(a.PRYR_EndDate).ToString("dd/MM/yyyy"),
        //        //                        PRYRA_ActiveFlag = a.PRYR_ActiveFlag
        //        //                    }
        //        //       ).Distinct().OrderByDescending(t => t.PRYR_Id).ToArray();



        //        data.programlist = (from a in _dbContext.ProgramsYearlyDMO
        //                            from d in _dbContext.AcademicYear
        //                            where (a.MI_Id == data.MI_Id && d.ASMAY_Id == a.ASMAY_Id&&a.PRYR_EndTime!=null)
        //                            select new OnlineProgramDTO
        //                            {
        //                                PRYR_Id = a.PRYR_Id,
        //                                ASMAY_Id = a.ASMAY_Id,
        //                                ASMAY_Year = d.ASMAY_Year,
        //                                programname = a.PRYR_ProgramName,
        //                                start_time = a.PRYR_StartTime,
        //                                end_time = a.PRYR_EndTime,
        //                                club = a.PRYR_StartDate.ToString("dd/MM/yyyy"),
        //                                Org_name = Convert.ToDateTime(a.PRYR_EndDate).ToString("dd/MM/yyyy"),
        //                                PRYRA_ActiveFlag = a.PRYR_ActiveFlag
        //                            }
        //      ).Distinct().OrderByDescending(t => t.PRYR_Id).ToArray();

        //        List<MasterAcademic> year = new List<MasterAcademic>();
        //        year = _dbContext.AcademicYear.Where(y => y.Is_Active == true && y.MI_Id == data.MI_Id).OrderByDescending(y => y.ASMAY_Order).ToList();
        //        data.fillyear = year.GroupBy(y => y.ASMAY_Year).Select(y => y.First()).ToArray();

        //    }
        //    catch (Exception ee)
        //    {
        //        Console.WriteLine(ee.Message);
        //    }

        //    return data;
        //}



        //public OnlineProgramDTO Savedata(OnlineProgramDTO data)
        //{

        //    var res = _dbContext.ProgramsYearlyDMO.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.PRYR_Id == data.PRYR_Id).ToList();
        //    if (res.Count > 0)
        //    {
        //        var objpge1 = _dbContext.ProgramsYearlyDMO.Single(t => t.PRYR_Id == data.PRYR_Id);
        //        objpge1.ASMAY_Id = data.ASMAY_Id;
        //        objpge1.MI_Id = data.MI_Id;
        //        objpge1.PRYR_ProgramName = data.programname;
        //        objpge1.PRYR_StartDate = data.Fromdate;
        //        objpge1.PRYR_EndDate = data.Todate;
        //        objpge1.PRYR_StartTime = data.start_time;
        //        objpge1.PRYR_EndTime = data.end_time;
        //        objpge1.PRYR_ProgramDescription = data.description;


        //        objpge1.UpdatedDate = DateTime.Now;

        //        objpge1.PRYR_UpdatedBy = data.UserId;
        //        if (data.pgTempDTO1.Length > 0)
        //        {
        //            if (data.pgTempDTO1[0].LPMTR_Resources != null)
        //            {
        //                objpge1.PRYR_ProgramChart = data.pgTempDTO1[0].file_name;
        //                objpge1.PRYR_ProgramChartPath = data.pgTempDTO1[0].LPMTR_Resources;
        //            }
        //        }

        //        objpge1.PRYR_PrgramLevel = data.PRMTLE_Id;
        //        objpge1.PRYR_ProgramTypeId = data.PRMTY_Id;
        //        objpge1.PRYR_PrgramConvenor = data.PRYRG_GuestName;
        //        objpge1.PRYR_TotalParticipants = data.strength;


        //        if (data.pgTempDTO3.Length > 0)
        //        {
        //            if (data.pgTempDTO3[0].LPMTR_Resources != null)
        //            {
        //                objpge1.PRYR_ProgramInvitation = data.pgTempDTO3[0].LPMTR_Resources;
        //            }
        //        }

        //        if (data.pgTempDTO.Length > 0)
        //        {
        //            if (data.pgTempDTO[0].LPMTR_Resources != null)
        //            {
        //                objpge1.PRYR_ParticipantList = data.pgTempDTO[0].file_name;
        //                objpge1.PRYR_PListPath = data.pgTempDTO[0].LPMTR_Resources;
        //            }
        //        }

        //        if (data.pgTempDTO2.Length > 0)
        //        {
        //            if (data.pgTempDTO2[0].LPMTR_Resources != null)
        //            {
        //                objpge1.PRYR_AccountStatement = data.pgTempDTO2[0].file_name;
        //                objpge1.PRYR_ASPath = data.pgTempDTO2[0].LPMTR_Resources;
        //            }
        //        }

        //        if (data.pgTempDTO4.Length > 0)
        //        {
        //            if (data.pgTempDTO4[0].LPMTR_Resources != null)
        //            {
        //                objpge1.PRYR_WinnerList = data.pgTempDTO4[0].file_name;
        //                objpge1.PRYR_WListPath = data.pgTempDTO4[0].LPMTR_Resources;
        //            }
        //        }


        //        _dbContext.Update(objpge1);



        //        var actv = _dbContext.ProgramsYearlyActivitiesDMO.Single(t => t.PRYR_Id == data.PRYR_Id);
        //        actv.PRYR_Id = objpge1.PRYR_Id;
        //        actv.PRYRA_ActivityName = data.club;
        //        actv.PRYRA_StartTime = data.start_time;
        //        actv.PRYRA_Description = data.description;

        //        actv.UpdatedDate = DateTime.Now;
        //        actv.PRYRA_UpdatedBy = data.UserId;
        //        _dbContext.Update(actv);







        //        var CountRemoveFiles = _dbContext.ProgramsYearlyFileDMO.Where(t => t.PRYR_Id == data.PRYR_Id).ToList();
        //        if (CountRemoveFiles.Count > 0)
        //        {
        //            foreach (var RemoveFiles in CountRemoveFiles)
        //            {
        //                _dbContext.Remove(RemoveFiles);
        //            }
        //            if (data.pgTempDTO7.Length > 0)
        //            {
        //                foreach (pgTempDTO7 ph1 in data.pgTempDTO7)
        //                {
        //                    if (data.pgTempDTO7[0].LPMTR_Resources != null)
        //                    {
        //                        //var obj2 = _dbContext.ProgramsYearlyFileDMO.Single(t => t.PRYR_Id == data.PRYR_Id);
        //                        ProgramsYearlyFileDMO obj2 = new ProgramsYearlyFileDMO();
        //                        // var obj2 = _dbContext.ProgramsYearlyFileDMO.Where(t => t.PRYR_Id == data.PRYR_Id).Distinct().ToArray();

        //                        //obj2.PRYRF_FileName = obj2[ii].PRYRF_FileName;
        //                        obj2.PRYR_Id = objpge1.PRYR_Id;
        //                        obj2.PRYRF_FileName = ph1.file_name;
        //                        obj2.PRYRF_FileType = ph1.filetype;
        //                        obj2.PRYRF_FilePath = ph1.LPMTR_Resources;
        //                        obj2.PRYRF_UpdatedBy = data.UserId;
        //                        obj2.UpdatedDate = DateTime.Now;
        //                        obj2.PRYRF_ActiveFlag = true;
        //                        obj2.CreatedDate = DateTime.Now;
        //                        obj2.PRYRF_CreatedBy = data.UserId;

        //                        _dbContext.Add(obj2);

        //                    }
        //                }
        //            }
        //        }
        //        else if (CountRemoveFiles.Count == 0)
        //        {
        //            if (data.pgTempDTO7.Length > 0)
        //            {
        //                foreach (pgTempDTO7 ph1 in data.pgTempDTO7)
        //                {
        //                    if (data.pgTempDTO7[0].LPMTR_Resources != null)
        //                    {
        //                        ProgramsYearlyFileDMO obj2 = new ProgramsYearlyFileDMO();

        //                        obj2.PRYR_Id = objpge1.PRYR_Id;
        //                        obj2.PRYRF_FileName = ph1.file_name;
        //                        obj2.PRYRF_FileType = ph1.filetype;
        //                        obj2.PRYRF_FilePath = ph1.LPMTR_Resources;

        //                        obj2.PRYRF_UpdatedBy = data.UserId;
        //                        obj2.PRYRF_ActiveFlag = true;
        //                        obj2.CreatedDate = DateTime.Now;
        //                        obj2.PRYRF_CreatedBy = data.UserId;
        //                        obj2.UpdatedDate = DateTime.Now;

        //                        _dbContext.Add(obj2);
        //                    }
        //                }
        //            }
        //        }













        //        var CountRemoveFiles1 = _dbContext.ProgramsYearlyGuestDMO.Where(t => t.PRYR_Id == data.PRYR_Id).ToList();
        //        if (CountRemoveFiles1.Count > 0)
        //        {
        //            foreach (var RemoveFiles in CountRemoveFiles1)
        //            {
        //                _dbContext.Remove(RemoveFiles);
        //            }
        //            if (data.GuestName != null)
        //            {

        //                for (int i = 0; i < data.pgTempDTO99.Length; i++)

        //                {

        //                    if (data.pgTempDTO99[i].LPMTR_Resources != null)

        //                    {

        //                        ProgramsYearlyGuestDMO obb = new ProgramsYearlyGuestDMO();

        //                        obb.PRYRG_GuestName = data.GuestName;
        //                        obb.PRYRG_GuestPhoneNo = data.gno;
        //                        obb.PRYRG_GuestAddress = data.gaddress;
        //                        obb.PRYRG_GuestType = data.gtype;
        //                        obb.PRYRG_GuestEmailId = data.emailid;
        //                        obb.PRYR_Id = objpge1.PRYR_Id;
        //                        obb.PRYRG_ActiveFlag = true;
        //                        obb.CreatedDate = DateTime.Now;
        //                        obb.PRYRG_CreatedBy = data.UserId;


        //                        obb.PRYRG_UpdatedBy = data.UserId;

        //                        obb.UpdatedDate = DateTime.Now;

        //                        if (data.pgTempDTO6.Length > 0)
        //                        {

        //                            if (data.pgTempDTO6[0].LPMTR_Resources != null)
        //                            {
        //                                obb.PRYRG_GuestProfileFileName = data.pgTempDTO4[0].file_name;
        //                                obb.PRYRG_GuestProfileFilePath = data.pgTempDTO4[0].LPMTR_Resources;
        //                            }

        //                        }
        //                        if (data.pgTempDTO5.Length > 0)
        //                        {
        //                            if (data.pgTempDTO5[0].LPMTR_Resources != null)
        //                            {
        //                                obb.PRYRG_GuestSpeechName = data.pgTempDTO4[0].file_name;
        //                                obb.PRYRG_GuestSpeechFilePath = data.pgTempDTO4[0].LPMTR_Resources;
        //                            }

        //                        }


        //                        obb.PRYRG_GuestPhotoVideoPath = data.pgTempDTO99[i].LPMTR_Resources;
        //                        obb.PRYRG_GuestPhotoVideo = data.pgTempDTO99[i].file_name;


        //                        _dbContext.Add(obb);
        //                    }
        //                }

        //            }
        //        }
        //        else if (CountRemoveFiles1.Count == 0)
        //        {
        //            if (data.GuestName != null)
        //            {

        //                for (int i = 0; i < data.pgTempDTO99.Length; i++)

        //                {
        //                    if (data.pgTempDTO99[i].LPMTR_Resources != null)
        //                    {

        //                        ProgramsYearlyGuestDMO obb = new ProgramsYearlyGuestDMO();

        //                        obb.PRYRG_GuestName = data.GuestName;
        //                        obb.PRYRG_GuestPhoneNo = data.gno;
        //                        obb.PRYRG_GuestAddress = data.gaddress;
        //                        obb.PRYRG_GuestType = data.gtype;
        //                        obb.PRYRG_GuestEmailId = data.emailid;
        //                        obb.PRYRG_ActiveFlag = true;
        //                        obb.CreatedDate = DateTime.Now;
        //                        obb.PRYRG_CreatedBy = data.UserId;


        //                        obb.PRYRG_UpdatedBy = data.UserId;

        //                        obb.UpdatedDate = DateTime.Now;

        //                        if (data.pgTempDTO6.Length > 0)
        //                        {

        //                            if (data.pgTempDTO6[0].LPMTR_Resources != null)
        //                            {
        //                                obb.PRYRG_GuestProfileFileName = data.pgTempDTO4[0].file_name;
        //                                obb.PRYRG_GuestProfileFilePath = data.pgTempDTO4[0].LPMTR_Resources;
        //                            }

        //                        }
        //                        if (data.pgTempDTO5.Length > 0)
        //                        {
        //                            if (data.pgTempDTO5[0].LPMTR_Resources != null)
        //                            {
        //                                obb.PRYRG_GuestSpeechName = data.pgTempDTO4[0].file_name;
        //                                obb.PRYRG_GuestSpeechFilePath = data.pgTempDTO4[0].LPMTR_Resources;
        //                            }
        //                        }


        //                        obb.PRYRG_GuestPhotoVideoPath = data.pgTempDTO99[i].LPMTR_Resources;
        //                        obb.PRYRG_GuestPhotoVideo = data.pgTempDTO99[i].file_name;
        //                        obb.PRYR_Id = objpge1.PRYR_Id;

        //                        _dbContext.Add(obb);
        //                    }
        //                }

        //            }
        //        }


        //        var contactExists = _dbContext.SaveChanges();
        //        if (contactExists > 0)
        //        {
        //            data.returnresult = true;
        //            data.message = "Update";
        //        }
        //        else
        //        {
        //            data.returnresult = false;
        //            data.message = "Not Update";
        //        }


        //    }

        //    else
        //    {
        //        try
        //        {
        //            ProgramsYearlyDMO objpge1 = new ProgramsYearlyDMO();

        //            objpge1.ASMAY_Id = data.ASMAY_Id;
        //            objpge1.MI_Id = data.MI_Id;
        //            objpge1.PRYR_ProgramName = data.programname;
        //            objpge1.PRYR_StartDate = data.Fromdate;
        //            objpge1.PRYR_EndDate = data.Todate;
        //            objpge1.PRYR_StartTime = data.start_time;
        //            objpge1.PRYR_EndTime = data.end_time;
        //            objpge1.PRYR_ProgramDescription = data.description;
        //            objpge1.PRYR_ActiveFlag = true;
        //            objpge1.CreatedDate = DateTime.Now;
        //            objpge1.UpdatedDate = DateTime.Now;
        //            objpge1.PRYR_CreatedBy = data.UserId;
        //            objpge1.PRYR_UpdatedBy = data.UserId;

        //            if (data.pgTempDTO1.Length > 0)
        //            {
        //                if (data.pgTempDTO1[0].LPMTR_Resources != null)
        //                {
        //                    objpge1.PRYR_ProgramChart = data.pgTempDTO1[0].file_name;
        //                    objpge1.PRYR_ProgramChartPath = data.pgTempDTO1[0].LPMTR_Resources;
        //                }
        //            }

        //            objpge1.PRYR_PrgramLevel = data.PRMTLE_Id;
        //            objpge1.PRYR_ProgramTypeId = data.PRMTY_Id;
        //            objpge1.PRYR_PrgramConvenor = data.PRYRG_GuestName;
        //            objpge1.PRYR_TotalParticipants = data.strength;

        //            if (data.pgTempDTO3.Length > 0)
        //            {
        //                if (data.pgTempDTO3[0].LPMTR_Resources != null)
        //                {
        //                    objpge1.PRYR_ProgramInvitation = data.pgTempDTO3[0].LPMTR_Resources;
        //                }
        //            }

        //            if (data.pgTempDTO.Length > 0)
        //            {
        //                if (data.pgTempDTO[0].LPMTR_Resources != null)
        //                {
        //                    objpge1.PRYR_ParticipantList = data.pgTempDTO[0].file_name;
        //                    objpge1.PRYR_PListPath = data.pgTempDTO[0].LPMTR_Resources;
        //                }
        //            }

        //            if (data.pgTempDTO2.Length > 0)
        //            {
        //                if (data.pgTempDTO2[0].LPMTR_Resources != null)
        //                {
        //                    objpge1.PRYR_AccountStatement = data.pgTempDTO2[0].file_name;
        //                    objpge1.PRYR_ASPath = data.pgTempDTO2[0].LPMTR_Resources;
        //                }
        //            }

        //            if (data.pgTempDTO4.Length > 0)
        //            {
        //                if (data.pgTempDTO4[0].LPMTR_Resources != null)
        //                {
        //                    objpge1.PRYR_WinnerList = data.pgTempDTO4[0].file_name;
        //                    objpge1.PRYR_WListPath = data.pgTempDTO4[0].LPMTR_Resources;
        //                }
        //            }

        //            //_dbContext.ProgramsYearlyDMO.Add(objpge1);
        //            _dbContext.Add(objpge1);


        //            ProgramsYearlyActivitiesDMO act = new ProgramsYearlyActivitiesDMO();
        //            act.PRYR_Id = objpge1.PRYR_Id;
        //            act.PRYRA_ActivityName = data.club;
        //            act.PRYRA_StartTime = data.start_time;
        //            act.PRYRA_Description = data.description;
        //            act.PRYRA_ActiveFlag = true;
        //            act.UpdatedDate = DateTime.Now;
        //            act.PRYRA_UpdatedBy = data.UserId;
        //            act.CreatedDate = DateTime.Now;
        //            act.PRYRA_CreatedBy = data.UserId;
        //            _dbContext.Add(act);


        //            foreach (pgTempDTO7 ph1 in data.pgTempDTO7)
        //            {
        //                if (data.pgTempDTO7[0].LPMTR_Resources != null)
        //                {
        //                    ProgramsYearlyFileDMO objpge2 = new ProgramsYearlyFileDMO();
        //                    objpge2.PRYR_Id = objpge1.PRYR_Id;
        //                    objpge2.PRYRF_FileName = ph1.file_name;
        //                    objpge2.PRYRF_FileType = ph1.filetype;
        //                    objpge2.PRYRF_FilePath = ph1.LPMTR_Resources;
        //                    objpge2.PRYRF_ActiveFlag = true;
        //                    objpge2.PRYRF_CreatedBy = data.UserId;
        //                    objpge2.PRYRF_UpdatedBy = data.UserId;
        //                    objpge2.CreatedDate = DateTime.Now;
        //                    objpge2.UpdatedDate = DateTime.Now;


        //                    // _dbContext.ProgramsYearlyFileDMO.Add(objpge2);
        //                    _dbContext.Add(objpge2);
        //                }
        //            }

        //            if (data.GuestName != null)
        //            {

        //                for (int i = 0; i < data.pgTempDTO99.Length; i++)
        //                //foreach (pgTempDTO99 ph1 in data.pgTempDTO99)                                           
        //                {
        //                    if (data.pgTempDTO99[i].LPMTR_Resources != null)

        //                    {
        //                        ProgramsYearlyGuestDMO obb = new ProgramsYearlyGuestDMO();

        //                        obb.PRYRG_GuestName = data.GuestName;
        //                        obb.PRYRG_GuestPhoneNo = data.gno;
        //                        obb.PRYRG_GuestAddress = data.gaddress;
        //                        obb.PRYRG_GuestType = data.gtype;
        //                        obb.PRYRG_GuestEmailId = data.emailid;
        //                        obb.PRYR_Id = objpge1.PRYR_Id;
        //                        obb.PRYRG_ActiveFlag = true;
        //                        obb.PRYRG_CreatedBy = data.UserId;
        //                        obb.PRYRG_UpdatedBy = data.UserId;
        //                        obb.CreatedDate = DateTime.Now;
        //                        obb.UpdatedDate = DateTime.Now;

        //                        if (data.pgTempDTO6.Length > 0)
        //                        {
        //                            for (int f = 0; f < data.pgTempDTO.Length; f++)
        //                            {
        //                                if (data.pgTempDTO6[f].LPMTR_Resources != null)
        //                                {
        //                                    obb.PRYRG_GuestProfileFileName = data.pgTempDTO4[f].file_name;
        //                                    obb.PRYRG_GuestProfileFilePath = data.pgTempDTO4[f].LPMTR_Resources;
        //                                }
        //                            }
        //                        }
        //                        if (data.pgTempDTO5.Length > 0)
        //                        {
        //                            if (data.pgTempDTO5[0].LPMTR_Resources != null)
        //                            {
        //                                obb.PRYRG_GuestSpeechName = data.pgTempDTO4[0].file_name;
        //                                obb.PRYRG_GuestSpeechFilePath = data.pgTempDTO4[0].LPMTR_Resources;
        //                            }
        //                        }

        //                        //ProgramsYearlyFileDMO objpge2 = new ProgramsYearlyFileDMO();
        //                        //obb.PRYR_Id = objpge1.PRYR_Id;
        //                        obb.PRYRG_GuestPhotoVideoPath = data.pgTempDTO99[i].LPMTR_Resources;
        //                        obb.PRYRG_GuestPhotoVideo = data.pgTempDTO99[i].file_name;

        //                        // _dbContext.ProgramsYearlyFileDMO.Add(objpge2);
        //                        _dbContext.Add(obb);
        //                    }
        //                }

        //            }



        //            try
        //            {
        //                var contactExists = _dbContext.SaveChanges();

        //                if (contactExists >= 1)
        //                {
        //                    data.returnresult = true;
        //                    data.message = "Saved";
        //                }
        //                else
        //                {
        //                    data.returnresult = false;
        //                    data.message = "Not Saved";
        //                }
        //            }
        //            catch (Exception e)
        //            {
        //                Console.WriteLine(e);
        //            }
        //        }

        //        catch (Exception e)
        //        {
        //            Console.WriteLine(e);
        //        }
        //    }
        //    return data;
        //}


        //public OnlineProgramDTO getdetails(OnlineProgramDTO data)
        //{
        //    try
        //    {

        //        //data.programlist = (from a in _dbContext.ProgramsYearlyDMO
        //        //                    from b in _dbContext.ProgramsYearlyFileDMO
        //        //                    from c in _dbContext.ProgramsYearlyGuestDMO
        //        //                    from d in _dbContext.AcademicYear
        //        //                    where (a.PRYR_Id == b.PRYR_Id && a.PRYR_Id == c.PRYR_Id && a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && d.ASMAY_Id == a.ASMAY_Id && a.PRYR_Id==data.PRYR_Id)
        //        //                    select new OnlineProgramDTO
        //        //                    {
        //        //                        PRYR_Id = a.PRYR_Id,
        //        //                        ASMAY_Id = a.ASMAY_Id,
        //        //                        ASMAY_Year = d.ASMAY_Year,
        //        //                        programname = a.PRYR_ProgramName,
        //        //                        start_time = a.PRYR_StartTime,
        //        //                        end_time = a.PRYR_EndTime,
        //        //                        Fromdate = a.PRYR_StartDate,
        //        //                        Todate = Convert.ToDateTime(a.PRYR_EndDate),
        //        //                        PRYRA_ActiveFlag = a.PRYR_ActiveFlag,
        //        //                        PRMTLE_Id = a.PRYR_PrgramLevel,
        //        //                        PRMTY_Id = a.PRYR_ProgramTypeId,
        //        //                        strength = a.PRYR_TotalParticipants,
        //        //                        PRYRG_GuestName = c.PRYRG_GuestName,
        //        //                        PRYRG_GuestPhoneNo = c.PRYRG_GuestPhoneNo,
        //        //                        PRYRG_GuestEmailId = c.PRYRG_GuestEmailId,
        //        //                        PRYRG_GuestAddress = c.PRYRG_GuestAddress,
        //        //                        PRYRG_GuestType = c.PRYRG_GuestType,
        //        //                        Org_name = a.PRYR_PrgramConvenor,
        //        //                        description = a.PRYR_ProgramDescription,

        //        //                    }
        //        //   ).Distinct().OrderByDescending(t => t.PRYR_Id).ToArray();

        //        //data.programlist = (from a in _dbContext.ProgramsYearlyDMO
        //        //                    from c in _dbContext.ProgramsYearlyGuestDMO
        //        //                    from d in _dbContext.AcademicYear
        //        //                    where (a.PRYR_Id == c.PRYR_Id && a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && d.ASMAY_Id == a.ASMAY_Id && a.PRYR_Id == data.PRYR_Id)
        //        //                    select new OnlineProgramDTO
        //        //                    {
        //        //                        PRYR_Id = a.PRYR_Id,
        //        //                        ASMAY_Id = a.ASMAY_Id,
        //        //                        ASMAY_Year = d.ASMAY_Year,
        //        //                        programname = a.PRYR_ProgramName,
        //        //                        start_time = a.PRYR_StartTime,
        //        //                        end_time = a.PRYR_EndTime,
        //        //                        Fromdate = a.PRYR_StartDate,
        //        //                        Todate = Convert.ToDateTime(a.PRYR_EndDate),
        //        //                        PRYRA_ActiveFlag = a.PRYR_ActiveFlag,
        //        //                        PRMTLE_Id = a.PRYR_PrgramLevel,
        //        //                        PRMTY_Id = a.PRYR_ProgramTypeId,
        //        //                        strength = a.PRYR_TotalParticipants,
        //        //                        PRYRG_GuestName = c.PRYRG_GuestName,
        //        //                        PRYRG_GuestPhoneNo = c.PRYRG_GuestPhoneNo,
        //        //                        PRYRG_GuestEmailId = c.PRYRG_GuestEmailId,
        //        //                        PRYRG_GuestAddress = c.PRYRG_GuestAddress,
        //        //                        PRYRG_GuestType = c.PRYRG_GuestType,
        //        //                        Org_name = a.PRYR_PrgramConvenor,
        //        //                        description = a.PRYR_ProgramDescription,

        //        //                    }
        //        //  ).Distinct().OrderByDescending(t => t.PRYR_Id).ToArray();


        //        data.programlist = (from a in _dbContext.ProgramsYearlyDMO


        //                            where (a.MI_Id == data.MI_Id && a.PRYR_Id == data.PRYR_Id)
        //                            select new OnlineProgramDTO
        //                            {
        //                                PRYR_Id = a.PRYR_Id,
        //                                ASMAY_Id = a.ASMAY_Id,
        //                                // ASMAY_Year = d.ASMAY_Year,
        //                                programname = a.PRYR_ProgramName,
        //                                start_time = a.PRYR_StartTime,
        //                                end_time = a.PRYR_EndTime,
        //                                Fromdate = a.PRYR_StartDate,
        //                                Todate = Convert.ToDateTime(a.PRYR_EndDate),
        //                                PRYRA_ActiveFlag = a.PRYR_ActiveFlag,
        //                                PRMTLE_Id = a.PRYR_PrgramLevel,
        //                                PRMTY_Id = a.PRYR_ProgramTypeId,
        //                                strength = a.PRYR_TotalParticipants,
        //                                // PRYRG_GuestName = c.PRYRG_GuestName,
        //                                // PRYRG_GuestPhoneNo = c.PRYRG_GuestPhoneNo,
        //                                //PRYRG_GuestEmailId = c.PRYRG_GuestEmailId,
        //                                // PRYRG_GuestAddress = c.PRYRG_GuestAddress,
        //                                //PRYRG_GuestType = c.PRYRG_GuestType,
        //                                Org_name = a.PRYR_PrgramConvenor,
        //                                description = a.PRYR_ProgramDescription,

        //                            }
        //          ).Distinct().OrderByDescending(t => t.PRYR_Id).ToArray();
        //        data.fillActivities1 = _dbContext.ProgramsYearlyActivitiesDMO.Where(t => t.PRYR_Id == data.PRYR_Id).Distinct().ToArray();

        //        data.guest = _dbContext.ProgramsYearlyGuestDMO.Where(t => t.PRYR_Id == data.PRYR_Id).Distinct().ToArray();


        //        data.uploadfiles11 = (from a in _dbContext.ProgramsYearlyDMO
        //                              where (a.MI_Id == data.MI_Id && a.PRYR_Id == data.PRYR_Id)
        //                              select new OnlineProgramDTO
        //                              {
        //                                  PRYR_Id = a.PRYR_Id,
        //                                  LPMTR_FileName = a.PRYR_ParticipantList,
        //                                  LPMTR_ResourceType = a.PRYR_PListPath
        //                              }).Distinct().ToArray();


        //        data.uploadfiles22 = (from a in _dbContext.ProgramsYearlyDMO
        //                              where (a.MI_Id == data.MI_Id && a.PRYR_Id == data.PRYR_Id)
        //                              select new OnlineProgramDTO
        //                              {
        //                                  PRYR_Id = a.PRYR_Id,
        //                                  LPMTR_FileName = a.PRYR_WinnerList,
        //                                  LPMTR_ResourceType = a.PRYR_WListPath
        //                              }).Distinct().ToArray();



        //        data.uploadfiles33 = (from a in _dbContext.ProgramsYearlyDMO
        //                              where (a.MI_Id == data.MI_Id && a.PRYR_Id == data.PRYR_Id)
        //                              select new OnlineProgramDTO
        //                              {
        //                                  PRYR_Id = a.PRYR_Id,
        //                                  LPMTR_FileName = a.PRYR_AccountStatement,
        //                                  LPMTR_ResourceType = a.PRYR_ASPath
        //                              }).Distinct().ToArray();



        //        data.uploadfiles44 = (from a in _dbContext.ProgramsYearlyDMO
        //                              where (a.MI_Id == data.MI_Id && a.PRYR_Id == data.PRYR_Id)
        //                              select new OnlineProgramDTO
        //                              {
        //                                  PRYR_Id = a.PRYR_Id,
        //                                  LPMTR_FileName = a.PRYR_ProgramChart,
        //                                  LPMTR_ResourceType = a.PRYR_ProgramChartPath
        //                              }).Distinct().ToArray();
        //        data.uploadfiles55 = (from a in _dbContext.ProgramsYearlyDMO
        //                              where (a.MI_Id == data.MI_Id && a.PRYR_Id == data.PRYR_Id)
        //                              select new OnlineProgramDTO
        //                              {
        //                                  PRYR_Id = a.PRYR_Id,
        //                                  LPMTR_ResourceType = a.PRYR_ProgramInvitation
        //                              }).Distinct().ToArray();

        //        data.uploadfiles66 = (from a in _dbContext.ProgramsYearlyDMO
        //                              from b in _dbContext.ProgramsYearlyFileDMO
        //                              where (a.PRYR_Id == b.PRYR_Id && a.MI_Id == data.MI_Id && a.PRYR_Id == data.PRYR_Id)
        //                              select new OnlineProgramDTO
        //                              {
        //                                  PRYRF_Id = b.PRYRF_Id,
        //                                  PRYR_Id = a.PRYR_Id,
        //                                  LPMTR_FileName = b.PRYRF_FileName,
        //                                  LPMTR_Filetype = b.PRYRF_FileType,
        //                                  LPMTR_ResourceType = b.PRYRF_FilePath
        //                              }).Distinct().ToArray();

        //        data.uploadfiles77 = (from a in _dbContext.ProgramsYearlyDMO
        //                              from b in _dbContext.ProgramsYearlyGuestDMO
        //                              where (a.PRYR_Id == b.PRYR_Id && a.MI_Id == data.MI_Id && a.PRYR_Id == data.PRYR_Id)
        //                              select new OnlineProgramDTO
        //                              {
        //                                  PRYR_Id = a.PRYR_Id,
        //                                  LPMTR_FileName = b.PRYRG_GuestProfileFileName,
        //                                  LPMTR_ResourceType = b.PRYRG_GuestProfileFilePath
        //                              }).Distinct().ToArray();
        //        data.uploadfiles88 = (from a in _dbContext.ProgramsYearlyDMO
        //                              from b in _dbContext.ProgramsYearlyGuestDMO
        //                              where (a.PRYR_Id == b.PRYR_Id && a.MI_Id == data.MI_Id && a.PRYR_Id == data.PRYR_Id)
        //                              select new OnlineProgramDTO
        //                              {
        //                                  PRYR_Id = a.PRYR_Id,
        //                                  LPMTR_FileName = b.PRYRG_GuestSpeechName,
        //                                  LPMTR_ResourceType = b.PRYRG_GuestSpeechFilePath
        //                              }).Distinct().ToArray();

        //        data.uploadfiles99 = (from a in _dbContext.ProgramsYearlyDMO
        //                              from b in _dbContext.ProgramsYearlyGuestDMO
        //                              where (a.PRYR_Id == b.PRYR_Id && a.MI_Id == data.MI_Id && a.PRYR_Id == data.PRYR_Id)
        //                              select new OnlineProgramDTO
        //                              {
        //                                  PRYRG_Id = b.PRYRG_Id,
        //                                  PRYR_Id = a.PRYR_Id,
        //                                  LPMTR_FileName = b.PRYRG_GuestPhotoVideo,
        //                                  LPMTR_ResourceType = b.PRYRG_GuestPhotoVideoPath
        //                              }).Distinct().ToArray();
        //    }
        //    catch (Exception ee)
        //    {
        //        Console.WriteLine(ee.Message);
        //    }

        //    return data;
        //}




        //public OnlineProgramDTO delete(OnlineProgramDTO data)
        //{
        //    try
        //    {



        //        var lorg1 = _dbContext.ProgramsYearlyDMO.Single(t => t.PRYR_Id == data.PRYR_Id);

        //        var lorg2 = _dbContext.ProgramsYearlyFileDMO.Where(t => t.PRYR_Id == data.PRYR_Id);

        //        var lorg3 = _dbContext.ProgramsYearlyGuestDMO.Where(t => t.PRYR_Id == data.PRYR_Id).ToList();
        //        var lorg4 = _dbContext.ProgramsYearlyActivitiesDMO.Where(t => t.PRYR_Id == data.PRYR_Id).ToList();


        //        foreach (var c in lorg3)
        //        {
        //            var checkresult = _dbContext.ProgramsYearlyGuestDMO.Single(a => a.PRYRG_Id == c.PRYRG_Id);
        //            _dbContext.Remove(checkresult);

        //        }
        //        foreach (var b in lorg2)
        //        {
        //            var checkresult = _dbContext.ProgramsYearlyFileDMO.Single(a => a.PRYRF_Id == b.PRYRF_Id);
        //            _dbContext.Remove(checkresult);

        //        }
        //        foreach (var bv in lorg4)
        //        {
        //            var checkresult = _dbContext.ProgramsYearlyActivitiesDMO.Single(a => a.PRYRA_Id == bv.PRYRA_Id);
        //            _dbContext.Remove(checkresult);

        //        }

        //        _dbContext.Remove(lorg1);

        //        var contactexisttransaction = 0;
        //        using (var dbCtxTxn = _dbContext.Database.BeginTransaction())
        //        {
        //            try
        //            {
        //                contactexisttransaction = _dbContext.SaveChanges();
        //                dbCtxTxn.Commit();
        //                data.returnval = "true";
        //            }
        //            catch (Exception ex)
        //            {
        //                dbCtxTxn.Rollback();
        //                data.returnval = "false";
        //            }
        //        }

        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine(e.InnerException);
        //    }
        //    return data;
        //}




        //public OnlineProgramDTO viewuploadflies(OnlineProgramDTO data)
        //{
        //    try
        //    {
        //        //over
        //        if (data.description == "chart")
        //        {
        //            data.uploadfiles = (from a in _dbContext.ProgramsYearlyDMO
        //                                where (a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.PRYR_Id == data.PRYR_Id)
        //                                select new OnlineProgramDTO
        //                                {
        //                                    PRYR_Id = a.PRYR_Id,
        //                                    LPMTR_FileName = a.PRYR_ProgramChart,
        //                                    LPMTR_ResourceType = a.PRYR_ProgramChartPath
        //                                }).Distinct().ToArray();
        //        }
        //        //over
        //        else if (data.description == "invitation")
        //        {
        //            data.uploadfiles = (from a in _dbContext.ProgramsYearlyDMO
        //                                where (a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.PRYR_Id == data.PRYR_Id)
        //                                select new OnlineProgramDTO
        //                                {
        //                                    PRYR_Id = a.PRYR_Id,
        //                                    LPMTR_ResourceType = a.PRYR_ProgramInvitation,
        //                                     LPMTR_FileName = a.PRYR_ProgramChart,
        //                                }).Distinct().ToArray();
        //        }


        //        //over
        //        else if (data.description == "list")
        //        {
        //            data.uploadfiles = (from a in _dbContext.ProgramsYearlyDMO
        //                                where (a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.PRYR_Id == data.PRYR_Id)
        //                                select new OnlineProgramDTO
        //                                {
        //                                    PRYR_Id = a.PRYR_Id,
        //                                    LPMTR_FileName = a.PRYR_ParticipantList,
        //                                    LPMTR_ResourceType = a.PRYR_PListPath
        //                                }).Distinct().ToArray();
        //        }
        //        //over
        //        else if (data.description == "account")
        //        {
        //            data.uploadfiles = (from a in _dbContext.ProgramsYearlyDMO
        //                                where (a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.PRYR_Id == data.PRYR_Id)
        //                                select new OnlineProgramDTO
        //                                {
        //                                    PRYR_Id = a.PRYR_Id,
        //                                    LPMTR_FileName = a.PRYR_AccountStatement,
        //                                    LPMTR_ResourceType = a.PRYR_ASPath
        //                                }).Distinct().ToArray();
        //        }
        //        //over
        //        else if (data.description == "winner")
        //        {
        //            data.uploadfiles = (from a in _dbContext.ProgramsYearlyDMO
        //                                where (a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.PRYR_Id == data.PRYR_Id)
        //                                select new OnlineProgramDTO
        //                                {
        //                                    PRYR_Id = a.PRYR_Id,
        //                                    LPMTR_FileName = a.PRYR_WinnerList,
        //                                    LPMTR_ResourceType = a.PRYR_WListPath
        //                                }).Distinct().ToArray();
        //        }
        //        //over
        //        else if (data.description == "programphoto")
        //        {
        //            data.uploadfiles = (from a in _dbContext.ProgramsYearlyDMO
        //                                from b in _dbContext.ProgramsYearlyFileDMO
        //                                where (a.PRYR_Id == b.PRYR_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.PRYR_Id == data.PRYR_Id)
        //                                select new OnlineProgramDTO
        //                                {
        //                                    PRYRF_Id = b.PRYRF_Id,
        //                                    PRYR_Id = a.PRYR_Id,
        //                                    LPMTR_FileName = b.PRYRF_FileName,
        //                                    LPMTR_Filetype = b.PRYRF_FileType,
        //                                    LPMTR_ResourceType = b.PRYRF_FilePath
        //                                }).Distinct().ToArray();
        //        }
        //        //over
        //        else if (data.description == "gprofile")
        //        {
        //            data.uploadfiles = (from a in _dbContext.ProgramsYearlyDMO
        //                                from b in _dbContext.ProgramsYearlyGuestDMO
        //                                where (a.PRYR_Id == b.PRYR_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.PRYR_Id == data.PRYR_Id)
        //                                select new OnlineProgramDTO
        //                                {
        //                                    PRYR_Id = a.PRYR_Id,
        //                                    LPMTR_FileName = b.PRYRG_GuestProfileFileName,
        //                                    LPMTR_ResourceType = b.PRYRG_GuestProfileFilePath
        //                                }).Distinct().ToArray();
        //        }
        //        //over
        //        else if (data.description == "gspeech")
        //        {
        //            data.uploadfiles = (from a in _dbContext.ProgramsYearlyDMO
        //                                from b in _dbContext.ProgramsYearlyGuestDMO
        //                                where (a.PRYR_Id == b.PRYR_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.PRYR_Id == data.PRYR_Id)
        //                                select new OnlineProgramDTO
        //                                {
        //                                    PRYR_Id = a.PRYR_Id,
        //                                    LPMTR_FileName = b.PRYRG_GuestSpeechName,
        //                                    LPMTR_ResourceType = b.PRYRG_GuestSpeechFilePath
        //                                }).Distinct().ToArray();
        //        }
        //        //over
        //        else
        //        {
        //            data.uploadfiles = (from a in _dbContext.ProgramsYearlyDMO
        //                                from b in _dbContext.ProgramsYearlyGuestDMO
        //                                where (a.PRYR_Id == b.PRYR_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.PRYR_Id == data.PRYR_Id)
        //                                select new OnlineProgramDTO
        //                                {
        //                                    PRYRG_Id = b.PRYRG_Id,
        //                                    PRYR_Id = a.PRYR_Id,
        //                                    LPMTR_FileName = b.PRYRG_GuestPhotoVideo,
        //                                    LPMTR_ResourceType = b.PRYRG_GuestPhotoVideoPath
        //                                }).Distinct().ToArray();
        //        }



        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //    }
        //    return data;
        //}
        #endregion


    }
}
