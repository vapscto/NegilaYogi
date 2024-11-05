using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using DataAccessMsSqlServerProvider.SeatingArrangment;
using DomainModel.Model.SeatingArrangment;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.SeatingArrangment;

namespace SeatingArrangment.Services
{
    public class School_Exam_Date_RoomImpl : Interface.School_Exam_Date_RoomInterface
    {
        public SAMasterBuildingContext _context;
        public School_Exam_Date_RoomImpl(SAMasterBuildingContext _cont)
        {
            _context = _cont;
        }
        public School_Exam_Date_RoomDTO GetExamDateRoomloaddata(School_Exam_Date_RoomDTO data)
        {
            try
            {
                data.GetAcademicYearList = _context.AcademicYearDMO.Where(a => a.MI_Id == data.MI_Id && a.Is_Active == true).OrderByDescending(a => a.ASMAY_Order).ToArray();

                data.GetExamList = _context.exammasterDMO.Where(a => a.MI_Id == data.MI_Id && a.EME_ActiveFlag == true).OrderBy(a => a.EME_ExamOrder).ToArray();

                data.GetExamSlotList = _context.Exam_SA_ExamSlotDMO.Where(a => a.MI_Id == data.MI_Id && a.ESAESLOT_ActiveFlg == true).Distinct().ToArray();

                data.GetSavedDetails = (from a in _context.School_Exam_SA_ExamDate_SchoolDMO
                                        from b in _context.exammasterDMO
                                        from c in _context.AcademicYearDMO
                                        from d in _context.Exam_SA_ExamSlotDMO
                                        where (a.ASMAY_Id == c.ASMAY_Id && a.EME_Id == b.EME_Id && a.ESAESLOT_Id == d.ESAESLOT_Id && a.MI_Id == data.MI_Id)
                                        select new School_Exam_Date_RoomDTO
                                        {
                                            ASMAY_Id = a.ASMAY_Id,
                                            EME_Id = a.EME_Id,
                                            ESAEDATESCH_Id = a.ESAEDATESCH_Id,
                                            ESAESLOT_Id = a.ESAESLOT_Id,
                                            ESAEDATESCH_ActiveFlg = a.ESAEDATESCH_ActiveFlg,
                                            ESAEDATESCH_CreatedDate = a.ESAEDATESCH_CreatedDate,
                                            ESAEDATESCH_ExamDate = a.ESAEDATESCH_ExamDate,
                                            ASMAY_Year = c.ASMAY_Year,
                                            EME_ExamName = b.EME_ExamName,
                                            ESAESLOT_SlotName = d.ESAESLOT_SlotName,
                                            ESAESLOT_StartTime = d.ESAESLOT_StartTime,
                                            ESAESLOT_EndTime = d.ESAESLOT_EndTime,
                                            ASMAY_Order = c.ASMAY_Order,
                                        }).Distinct().OrderByDescending(a => a.ASMAY_Order).OrderByDescending(a => a.ESAEDATESCH_CreatedDate).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public School_Exam_Date_RoomDTO GetSearchExamDateRoomData(School_Exam_Date_RoomDTO data)
        {
            try
            {
                data.GetRoomList = (from a in _context.Exam_SA_BuildingDMO
                                    from b in _context.Exam_SA_FloorDMO
                                    from c in _context.Exam_SA_RoomDMO
                                    where (a.ESABLD_Id == b.ESABLD_Id && b.ESAFLR_Id == c.ESAFLR_Id && a.MI_Id == data.MI_Id
                                    && b.MI_Id == data.MI_Id && c.MI_Id == data.MI_Id)
                                    select c).Distinct().ToArray();

                data.GetaSavedRoomDetails = (from a in _context.School_Exam_SA_ExamDate_SchoolDMO
                                             from b in _context.School_Exam_SA_ExamDate_Room_SchoolDMO
                                             from c in _context.Exam_SA_RoomDMO
                                             where (a.ESAEDATESCH_Id == b.ESAEDATESCH_Id && b.ESAROOM_Id == c.ESAROOM_Id && a.MI_Id == data.MI_Id
                                             && a.ASMAY_Id == data.ASMAY_Id && a.EME_Id == data.EME_Id && a.ESAESLOT_Id == data.ESAESLOT_Id)
                                             select b).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public School_Exam_Date_RoomDTO SaveExamDateRoomData(School_Exam_Date_RoomDTO data)
        {
            try
            {
                data.ReturnValue = false;
                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

                if (data.ESAEDATESCH_Id > 0)
                {
                    var checkduplicate = _context.School_Exam_SA_ExamDate_SchoolDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                    && a.ESAEDATESCH_ExamDate == data.ESAEDATESCH_ExamDate && a.ESAESLOT_Id == data.ESAESLOT_Id
                    && a.ESAEDATESCH_Id == data.ESAEDATESCH_Id).ToList();

                    if (checkduplicate.Count > 0)
                    {
                        var resultid = _context.School_Exam_SA_ExamDate_SchoolDMO.Single(a => a.ESAEDATESCH_Id == data.ESAEDATESCH_Id);
                        resultid.ESAEDATESCH_UpdatedDate = indiantime0;
                        resultid.ESAEDATESCH_UpdatedBy = data.UserId;
                        _context.Update(resultid);

                        if (data.School_Room_Temp_Details != null && data.School_Room_Temp_Details.Length > 0)
                        {
                            SaveorUpdateRoomDetails(data);
                        }

                        var i = _context.SaveChanges();
                        if (i > 0)
                        {
                            data.ReturnValue = true;
                        }
                    }
                }
                else
                {
                    var checkduplicate = _context.School_Exam_SA_ExamDate_SchoolDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                    && a.ESAEDATESCH_ExamDate == data.ESAEDATESCH_ExamDate && a.ESAESLOT_Id == data.ESAESLOT_Id).ToList();

                    if (checkduplicate.Count > 0)
                    {
                        data.ESAEDATESCH_Id = checkduplicate.FirstOrDefault().ESAEDATESCH_Id;

                        var getresultid = _context.School_Exam_SA_ExamDate_SchoolDMO.Single(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                        && a.ESAEDATESCH_ExamDate == data.ESAEDATESCH_ExamDate && a.ESAESLOT_Id == data.ESAESLOT_Id
                        && a.ESAEDATESCH_Id == checkduplicate.FirstOrDefault().ESAEDATESCH_Id);

                        getresultid.ESAEDATESCH_UpdatedDate = indiantime0;
                        getresultid.ESAEDATESCH_UpdatedBy = data.UserId;
                        _context.Update(getresultid);

                        if (data.School_Room_Temp_Details != null && data.School_Room_Temp_Details.Length > 0)
                        {
                            SaveorUpdateRoomDetails(data);
                        }

                        var i = _context.SaveChanges();
                        if (i > 0)
                        {
                            data.ReturnValue = true;
                        }
                    }
                    else
                    {
                        School_Exam_SA_ExamDate_SchoolDMO school_Exam_SA_ExamDate_SchoolDMO = new School_Exam_SA_ExamDate_SchoolDMO();
                        school_Exam_SA_ExamDate_SchoolDMO.MI_Id = data.MI_Id;
                        school_Exam_SA_ExamDate_SchoolDMO.ASMAY_Id = data.ASMAY_Id;
                        school_Exam_SA_ExamDate_SchoolDMO.EME_Id = data.EME_Id;
                        school_Exam_SA_ExamDate_SchoolDMO.ESAESLOT_Id = data.ESAESLOT_Id;
                        school_Exam_SA_ExamDate_SchoolDMO.ESAEDATESCH_ExamDate = data.ESAEDATESCH_ExamDate;
                        school_Exam_SA_ExamDate_SchoolDMO.ESAEDATESCH_ActiveFlg = true;
                        school_Exam_SA_ExamDate_SchoolDMO.ESAEDATESCH_CreatedDate = indiantime0;
                        school_Exam_SA_ExamDate_SchoolDMO.ESAEDATESCH_UpdatedDate = indiantime0;
                        school_Exam_SA_ExamDate_SchoolDMO.ESAEDATESCH_CreatedBy = data.UserId;
                        school_Exam_SA_ExamDate_SchoolDMO.ESAEDATESCH_UpdatedBy = data.UserId;
                        _context.Add(school_Exam_SA_ExamDate_SchoolDMO);
                        data.ESAEDATESCH_Id = school_Exam_SA_ExamDate_SchoolDMO.ESAEDATESCH_Id;
                        SaveorUpdateRoomDetails(data);
                        var i = _context.SaveChanges();
                        if (i > 0)
                        {
                            data.ReturnValue = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public School_Exam_Date_RoomDTO EditExamDateRoomData(School_Exam_Date_RoomDTO data)
        {
            try
            {
                data.GetEditedDateDetails = _context.School_Exam_SA_ExamDate_SchoolDMO.Where(a => a.MI_Id == data.MI_Id
                && a.ESAEDATESCH_Id == data.ESAEDATESCH_Id).ToArray();

                data.GetEditedRoomDateDetails = (from a in _context.School_Exam_SA_ExamDate_SchoolDMO
                                                 from b in _context.School_Exam_SA_ExamDate_Room_SchoolDMO
                                                 from c in _context.Exam_SA_RoomDMO
                                                 where (a.ESAEDATESCH_Id == b.ESAEDATESCH_Id && b.ESAROOM_Id == c.ESAROOM_Id && b.ESAEDATESCH_Id == data.ESAEDATESCH_Id)
                                                 select b).Distinct().ToArray();

                data.GetRoomList = (from a in _context.Exam_SA_BuildingDMO
                                    from b in _context.Exam_SA_FloorDMO
                                    from c in _context.Exam_SA_RoomDMO
                                    where (a.ESABLD_Id == b.ESABLD_Id && b.ESAFLR_Id == c.ESAFLR_Id && a.MI_Id == data.MI_Id
                                    && b.MI_Id == data.MI_Id && c.MI_Id == data.MI_Id)
                                    select c).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public School_Exam_Date_RoomDTO ViewExamDateRoomDetails(School_Exam_Date_RoomDTO data)
        {
            try
            {
                data.GetViewRoomDetails = (from a in _context.School_Exam_SA_ExamDate_SchoolDMO
                                           from b in _context.School_Exam_SA_ExamDate_Room_SchoolDMO
                                           from c in _context.Exam_SA_RoomDMO
                                           where (a.ESAEDATESCH_Id == b.ESAEDATESCH_Id && b.ESAROOM_Id == c.ESAROOM_Id && a.MI_Id == data.MI_Id
                                           && a.ESAEDATESCH_Id == data.ESAEDATESCH_Id)
                                           select new School_Exam_Date_RoomDTO
                                           {
                                               ESAEDATESCH_Id = a.ESAEDATESCH_Id,
                                               ESAROOM_Id = b.ESAROOM_Id,
                                               ESAEDATERSCH_ActiveFlg = b.ESAEDATERSCH_ActiveFlg,
                                               ESAROOM_RoomName = c.ESAROOM_RoomName,
                                               ESAEDATERSCH_Id = b.ESAEDATERSCH_Id
                                           }).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public School_Exam_Date_RoomDTO ActiveDeactiveExamRoomDate(School_Exam_Date_RoomDTO data)
        {
            try
            {
                data.ReturnValue = false;
                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

                var checkresult = _context.School_Exam_SA_ExamDate_SchoolDMO.Where(a => a.MI_Id == data.MI_Id && a.ESAEDATESCH_Id == data.ESAEDATESCH_Id).ToList();
                if (checkresult.Count > 0)
                {
                    var result = _context.School_Exam_SA_ExamDate_SchoolDMO.Single(a => a.MI_Id == data.MI_Id && a.ESAEDATESCH_Id == data.ESAEDATESCH_Id);
                    result.ESAEDATESCH_ActiveFlg = result.ESAEDATESCH_ActiveFlg == true ? false : true;
                    result.ESAEDATESCH_UpdatedBy = data.UserId;
                    result.ESAEDATESCH_UpdatedDate = indiantime0;
                    _context.Update(result);

                    var i = _context.SaveChanges();
                    if (i > 0)
                    {
                        data.ReturnValue = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public School_Exam_Date_RoomDTO ActiveDeactiveExamDateRoomDetails(School_Exam_Date_RoomDTO data)
        {
            try
            {
                data.ReturnValue = false;
                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

                var checkresult = _context.School_Exam_SA_ExamDate_Room_SchoolDMO.Where(a => a.ESAEDATERSCH_Id == data.ESAEDATERSCH_Id).ToList();
                if (checkresult.Count > 0)
                {
                    var result = _context.School_Exam_SA_ExamDate_Room_SchoolDMO.Single(a => a.ESAEDATERSCH_Id == data.ESAEDATERSCH_Id);
                    result.ESAEDATERSCH_ActiveFlg = result.ESAEDATERSCH_ActiveFlg == true ? false : true;
                    result.ESAEDATERSCH_UpdatedBy = data.UserId;
                    result.ESAEDATERSCH_UpdatedDate = indiantime0;
                    _context.Update(result);

                    var i = _context.SaveChanges();
                    if (i > 0)
                    {
                        data.ReturnValue = true;
                    }

                    data.GetViewRoomDetails = (from a in _context.School_Exam_SA_ExamDate_SchoolDMO
                                               from b in _context.School_Exam_SA_ExamDate_Room_SchoolDMO
                                               from c in _context.Exam_SA_RoomDMO
                                               where (a.ESAEDATESCH_Id == b.ESAEDATESCH_Id && b.ESAROOM_Id == c.ESAROOM_Id && a.MI_Id == data.MI_Id
                                               && a.ESAEDATESCH_Id == data.ESAEDATESCH_Id)
                                               select new School_Exam_Date_RoomDTO
                                               {
                                                   ESAEDATESCH_Id = a.ESAEDATESCH_Id,
                                                   ESAROOM_Id = b.ESAROOM_Id,
                                                   ESAEDATERSCH_ActiveFlg = b.ESAEDATERSCH_ActiveFlg,
                                                   ESAROOM_RoomName = c.ESAROOM_RoomName,
                                                   ESAEDATERSCH_Id = b.ESAEDATERSCH_Id
                                               }).Distinct().ToArray();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public School_Exam_Date_RoomDTO SaveorUpdateRoomDetails(School_Exam_Date_RoomDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

                List<long> id = new List<long>();

                foreach (var c in data.School_Room_Temp_Details)
                {
                    id.Add(c.ESAEDATERSCH_Id);
                }

                Array removeids = _context.School_Exam_SA_ExamDate_Room_SchoolDMO.Where(a => !id.Contains(a.ESAEDATERSCH_Id)
                && a.ESAEDATESCH_Id == data.ESAEDATESCH_Id).ToArray();

                foreach (School_Exam_SA_ExamDate_Room_SchoolDMO ph1 in removeids)
                {
                    _context.Remove(ph1);
                }

                foreach (var c in data.School_Room_Temp_Details)
                {
                    if (c.ESAEDATERSCH_Id > 0)
                    {
                        var Room_Noresult = _context.School_Exam_SA_ExamDate_Room_SchoolDMO.Single(t => t.ESAEDATERSCH_Id == c.ESAEDATERSCH_Id);
                        Room_Noresult.ESAEDATERSCH_UpdatedBy = data.UserId;
                        Room_Noresult.ESAEDATERSCH_UpdatedDate = indiantime0;
                        Room_Noresult.ESAROOM_Id = c.ESAROOM_Id;
                        _context.Update(Room_Noresult);
                    }
                    else
                    {
                        School_Exam_SA_ExamDate_Room_SchoolDMO exam_SA_ExamDate_RoomDMO = new School_Exam_SA_ExamDate_Room_SchoolDMO();

                        exam_SA_ExamDate_RoomDMO.ESAROOM_Id = c.ESAROOM_Id;
                        exam_SA_ExamDate_RoomDMO.ESAEDATESCH_Id = data.ESAEDATESCH_Id;
                        exam_SA_ExamDate_RoomDMO.ESAEDATERSCH_ActiveFlg = true;
                        exam_SA_ExamDate_RoomDMO.ESAEDATERSCH_UpdatedBy = data.UserId;
                        exam_SA_ExamDate_RoomDMO.ESAEDATERSCH_UpdatedDate = indiantime0;
                        exam_SA_ExamDate_RoomDMO.ESAEDATERSCH_CreatedBy = data.UserId;
                        exam_SA_ExamDate_RoomDMO.ESAEDATERSCH_CreatedDate = indiantime0;
                        _context.Add(exam_SA_ExamDate_RoomDMO);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }


        //Room  Date Class Subject Mapping 
        public School_Exam_Date_RoomDTO GetExamDateRoomClassMappingloaddata(School_Exam_Date_RoomDTO data)
        {
            try
            {
                data.GetAcademicYearList = _context.AcademicYearDMO.Where(a => a.MI_Id == data.MI_Id && a.Is_Active == true).OrderByDescending(a => a.ASMAY_Order).ToArray();

                data.GetExamList = _context.exammasterDMO.Where(a => a.MI_Id == data.MI_Id && a.EME_ActiveFlag == true).OrderBy(a => a.EME_ExamOrder).ToArray();

                data.GetExamSlotList = _context.Exam_SA_ExamSlotDMO.Where(a => a.MI_Id == data.MI_Id && a.ESAESLOT_ActiveFlg == true).Distinct().ToArray();

                data.GetSavedDetails = (from a in _context.School_Exam_SA_Room_SchoolDMO
                                        from b in _context.AcademicYearDMO
                                        from c in _context.exammasterDMO
                                        from d in _context.Exam_SA_ExamSlotDMO
                                        from e in _context.Exam_SA_RoomDMO
                                        where (a.ASMAY_Id == b.ASMAY_Id && a.EME_Id == c.EME_Id && a.ESAESLOT_Id == d.ESAESLOT_Id && a.ESAROOM_Id == e.ESAROOM_Id
                                        && a.MI_Id == data.MI_Id)
                                        select new School_Exam_Date_RoomDTO
                                        {
                                            EME_ExamName = c.EME_ExamName,
                                            ASMAY_Year = b.ASMAY_Year,
                                            ESAESLOT_SlotName = d.ESAESLOT_SlotName,
                                            ESAROOM_RoomName = e.ESAROOM_RoomName,
                                            ESARMSCH_ActiveFlg = a.ESARMSCH_ActiveFlg,
                                            ESARMSCH_ExamDate = a.ESARMSCH_ExamDate,
                                            ESARMSCH_Id = a.ESARMSCH_Id,
                                            ASMAY_Order = b.ASMAY_Order,
                                            ESARMSCH_CreatedDate = a.ESARMSCH_CreatedDate
                                        }).Distinct().OrderByDescending(a => a.ASMAY_Order).OrderByDescending(a => a.ESARMSCH_CreatedDate).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public School_Exam_Date_RoomDTO GetSubjectListRoomClassMapping(School_Exam_Date_RoomDTO data)
        {
            try
            {
                data.GetRoomList = (from a in _context.School_Exam_SA_ExamDate_Room_SchoolDMO
                                    from b in _context.School_Exam_SA_ExamDate_SchoolDMO
                                    from c in _context.Exam_SA_RoomDMO
                                    where (a.ESAEDATESCH_Id == b.ESAEDATESCH_Id && a.ESAROOM_Id == c.ESAROOM_Id && a.ESAEDATERSCH_ActiveFlg == true
                                    && b.MI_Id == data.MI_Id && c.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.EME_Id == data.EME_Id
                                    && b.ESAESLOT_Id == data.ESAESLOT_Id && b.ESAEDATESCH_ExamDate == data.ESARMSCH_ExamDate)
                                    select c).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public School_Exam_Date_RoomDTO GetSearchExamDateRoomClassMappingData(School_Exam_Date_RoomDTO data)
        {
            try
            {
                data.GetRoomDetails = _context.Exam_SA_RoomDMO.Where(a => a.MI_Id == data.MI_Id && a.ESAROOM_Id == data.ESAROOM_Id).ToArray();

                data.GetClassList = _context.School_M_Class.Where(a => a.MI_Id == data.MI_Id && a.ASMCL_ActiveFlag == true).OrderBy(a => a.ASMCL_Order).ToArray();

                data.GetSubjectList = _context.IVRM_Master_SubjectsDMO.Where(a => a.MI_Id == data.MI_Id && a.ISMS_ActiveFlag == 1).Distinct().ToArray();

                data.GetSavedClassSubjectList = (from a in _context.School_Exam_SA_Room_ClassSubject_SchoolDMO
                                                 from b in _context.School_M_Class
                                                 from c in _context.IVRM_Master_SubjectsDMO
                                                 from d in _context.School_Exam_SA_Room_SchoolDMO
                                                 where (a.ESARMSCH_Id == d.ESARMSCH_Id && a.ASMCL_Id == b.ASMCL_Id && a.ISMS_Id == c.ISMS_Id
                                                 && d.ESAROOM_Id == data.ESAROOM_Id && d.ASMAY_Id == data.ASMAY_Id && d.EME_Id == data.EME_Id
                                                 && d.ESAESLOT_Id == data.ESAESLOT_Id && d.ESARMSCH_ExamDate == data.ESARMSCH_ExamDate)
                                                 select new School_Exam_Date_RoomDTO
                                                 {
                                                     ESARMCSSCH_Id = a.ESARMCSSCH_Id,
                                                     ASMCL_Id = a.ASMCL_Id,
                                                     ISMS_Id = a.ISMS_Id,
                                                     ASMCL_ClassName = b.ASMCL_ClassName,
                                                     ISMS_SubjectName = c.ISMS_SubjectName
                                                 }).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public School_Exam_Date_RoomDTO SaveExamDateRoomClassMappingData(School_Exam_Date_RoomDTO data)
        {
            try
            {
                data.ReturnValue = false;
                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

                if (data.ESARMSCH_Id > 0)
                {
                    var resultcheck = _context.School_Exam_SA_Room_SchoolDMO.Where(a => a.MI_Id == data.MI_Id && a.ESARMSCH_Id == data.ESARMSCH_Id).ToList();

                    if (resultcheck.Count > 0)
                    {
                        var result = _context.School_Exam_SA_Room_SchoolDMO.Single(a => a.MI_Id == data.MI_Id && a.ESARMSCH_Id == data.ESARMSCH_Id);

                        result.ESARMSCH_UpdatedBy = data.UserId;
                        result.ESARMSCH_UpdatedDate = indiantime0;
                        _context.Update(result);

                        if (data.School_Room_ClassSubject_Temp_Details != null && data.School_Room_ClassSubject_Temp_Details.Length > 0)
                        {
                            SaveorUpdateRoomClassSubjectDetails(data);
                        }
                        var i = _context.SaveChanges();
                        if (i > 0)
                        {
                            data.ReturnValue = true;
                        }
                    }
                }
                else
                {
                    var checkresult = _context.School_Exam_SA_Room_SchoolDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                    && a.EME_Id == data.EME_Id && a.ESAESLOT_Id == data.ESAESLOT_Id && a.ESAROOM_Id == data.ESAROOM_Id
                    && a.ESARMSCH_ExamDate == data.ESARMSCH_ExamDate).ToList();

                    if (checkresult.Count > 0)
                    {
                        data.ESARMSCH_Id = checkresult.FirstOrDefault().ESARMSCH_Id;
                        var result = _context.School_Exam_SA_Room_SchoolDMO.Single(a => a.MI_Id == data.MI_Id && a.ESARMSCH_Id == data.ESARMSCH_Id);

                        result.ESARMSCH_UpdatedBy = data.UserId;
                        result.ESARMSCH_UpdatedDate = indiantime0;
                        _context.Update(result);

                        if (data.School_Room_ClassSubject_Temp_Details != null && data.School_Room_ClassSubject_Temp_Details.Length > 0)
                        {
                            SaveorUpdateRoomClassSubjectDetails(data);
                        }
                        var i = _context.SaveChanges();
                        if (i > 0)
                        {
                            data.ReturnValue = true;
                        }
                    }

                    else
                    {
                        School_Exam_SA_Room_SchoolDMO school_Exam_SA_Room_SchoolDMO = new School_Exam_SA_Room_SchoolDMO();
                        school_Exam_SA_Room_SchoolDMO.MI_Id = data.MI_Id;
                        school_Exam_SA_Room_SchoolDMO.ASMAY_Id = data.ASMAY_Id;
                        school_Exam_SA_Room_SchoolDMO.EME_Id = data.EME_Id;
                        school_Exam_SA_Room_SchoolDMO.ESAESLOT_Id = data.ESAESLOT_Id;
                        school_Exam_SA_Room_SchoolDMO.ESARMSCH_ExamDate = data.ESARMSCH_ExamDate;
                        school_Exam_SA_Room_SchoolDMO.ESAROOM_Id = data.ESAROOM_Id;
                        school_Exam_SA_Room_SchoolDMO.ESARMSCH_ActiveFlg = true;
                        school_Exam_SA_Room_SchoolDMO.ESARMSCH_CreatedDate = indiantime0;
                        school_Exam_SA_Room_SchoolDMO.ESARMSCH_UpdatedDate = indiantime0;
                        school_Exam_SA_Room_SchoolDMO.ESARMSCH_CreatedBy = data.UserId;
                        school_Exam_SA_Room_SchoolDMO.ESARMSCH_UpdatedBy = data.UserId;
                        _context.Add(school_Exam_SA_Room_SchoolDMO);
                        data.ESARMSCH_Id = school_Exam_SA_Room_SchoolDMO.ESARMSCH_Id;

                        if (data.School_Room_ClassSubject_Temp_Details != null && data.School_Room_ClassSubject_Temp_Details.Length > 0)
                        {
                            SaveorUpdateRoomClassSubjectDetails(data);
                        }
                        var i = _context.SaveChanges();
                        if (i > 0)
                        {
                            data.ReturnValue = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public School_Exam_Date_RoomDTO EditExamDateRoomClassMappingData(School_Exam_Date_RoomDTO data)
        {
            try
            {
                var details = _context.School_Exam_SA_Room_SchoolDMO.Where(a => a.MI_Id == data.MI_Id && a.ESARMSCH_Id == data.ESARMSCH_Id).ToList();

                data.GetEditDetails = details.ToArray();

                data.GetRoomDetails = _context.Exam_SA_RoomDMO.Where(a => a.MI_Id == data.MI_Id && a.ESAROOM_Id == details.FirstOrDefault().ESAROOM_Id).ToArray();

                data.GetClassList = _context.School_M_Class.Where(a => a.MI_Id == data.MI_Id && a.ASMCL_ActiveFlag == true).OrderBy(a => a.ASMCL_Order).ToArray();

                data.GetSubjectList = _context.IVRM_Master_SubjectsDMO.Where(a => a.MI_Id == data.MI_Id && a.ISMS_ActiveFlag == 1).Distinct().ToArray();

                data.GetRoomList = (from a in _context.School_Exam_SA_ExamDate_Room_SchoolDMO
                                    from b in _context.School_Exam_SA_ExamDate_SchoolDMO
                                    from c in _context.Exam_SA_RoomDMO
                                    where (a.ESAEDATESCH_Id == b.ESAEDATESCH_Id && a.ESAROOM_Id == c.ESAROOM_Id && a.ESAEDATERSCH_ActiveFlg == true
                                    && b.MI_Id == data.MI_Id && c.MI_Id == data.MI_Id && b.ASMAY_Id == details.FirstOrDefault().ASMAY_Id 
                                    && b.EME_Id == details.FirstOrDefault().EME_Id && b.ESAESLOT_Id == details.FirstOrDefault().ESAESLOT_Id 
                                    && b.ESAEDATESCH_ExamDate == details.FirstOrDefault().ESARMSCH_ExamDate)
                                    select c).Distinct().ToArray();

                data.GetSavedClassSubjectList = (from a in _context.School_Exam_SA_Room_ClassSubject_SchoolDMO
                                                 from b in _context.School_M_Class
                                                 from c in _context.IVRM_Master_SubjectsDMO
                                                 from d in _context.School_Exam_SA_Room_SchoolDMO
                                                 where (a.ESARMSCH_Id == d.ESARMSCH_Id && a.ASMCL_Id == b.ASMCL_Id && a.ISMS_Id == c.ISMS_Id
                                                 && d.ESAROOM_Id == details.FirstOrDefault().ESAROOM_Id && d.ASMAY_Id == details.FirstOrDefault().ASMAY_Id
                                                 && d.EME_Id == details.FirstOrDefault().EME_Id && d.ESAESLOT_Id == details.FirstOrDefault().ESAESLOT_Id
                                                 && d.ESARMSCH_ExamDate == details.FirstOrDefault().ESARMSCH_ExamDate && a.ESARMSCH_Id == data.ESARMSCH_Id)
                                                 select new School_Exam_Date_RoomDTO
                                                 {
                                                     ESARMCSSCH_Id = a.ESARMCSSCH_Id,
                                                     ASMCL_Id = a.ASMCL_Id,
                                                     ISMS_Id = a.ISMS_Id,
                                                     ASMCL_ClassName = b.ASMCL_ClassName,
                                                     ISMS_SubjectName = c.ISMS_SubjectName
                                                 }).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public School_Exam_Date_RoomDTO ViewExamDateRoomClassMappingDetails(School_Exam_Date_RoomDTO data)
        {
            try
            {
                data.GetViewRoomClassSubjectDetails = (from a in _context.School_Exam_SA_Room_ClassSubject_SchoolDMO
                                                       from b in _context.School_M_Class
                                                       from c in _context.IVRM_Master_SubjectsDMO
                                                       where (a.ASMCL_Id == b.ASMCL_Id && a.ISMS_Id == c.ISMS_Id && a.ESARMSCH_Id == data.ESARMSCH_Id)
                                                       select new School_Exam_Date_RoomDTO
                                                       {
                                                           ASMCL_ClassName = b.ASMCL_ClassName,
                                                           ISMS_SubjectName = c.ISMS_SubjectName,
                                                           ESARMSCH_Id = a.ESARMSCH_Id,
                                                           ESARMCSSCH_Id = a.ESARMCSSCH_Id,
                                                           ASMCL_Order = b.ASMCL_Order,
                                                           ESARMCSSCH_ActiveFlg = a.ESARMCSSCH_ActiveFlg
                                                       }).Distinct().OrderBy(a => a.ASMCL_Order).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public School_Exam_Date_RoomDTO ActiveDeactiveExamRoomClassMappingDate(School_Exam_Date_RoomDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

                var result = _context.School_Exam_SA_Room_SchoolDMO.Single(a => a.MI_Id == data.MI_Id && a.ESARMSCH_Id == data.ESARMSCH_Id);

                result.ESARMSCH_UpdatedBy = data.UserId;
                result.ESARMSCH_UpdatedDate = indiantime0;
                result.ESARMSCH_ActiveFlg = result.ESARMSCH_ActiveFlg == true ? false : true;
                _context.Update(result);
                var i = _context.SaveChanges();
                if (i > 0)
                {
                    data.ReturnValue = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public School_Exam_Date_RoomDTO ActiveDeactiveExamDateRoomClassMappingDetails(School_Exam_Date_RoomDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

                var result = _context.School_Exam_SA_Room_ClassSubject_SchoolDMO.Single(a => a.ESARMCSSCH_Id == data.ESARMCSSCH_Id);

                result.ESARMCSSCH_UpdatedBy = data.UserId;
                result.ESARMCSSCH_UpdatedDate = indiantime0;
                result.ESARMCSSCH_ActiveFlg = result.ESARMCSSCH_ActiveFlg == true ? false : true;
                _context.Update(result);
                var i = _context.SaveChanges();
                if (i > 0)
                {
                    data.ReturnValue = true;
                }

                data.GetViewRoomClassSubjectDetails = (from a in _context.School_Exam_SA_Room_ClassSubject_SchoolDMO
                                                       from b in _context.School_M_Class
                                                       from c in _context.IVRM_Master_SubjectsDMO
                                                       where (a.ASMCL_Id == b.ASMCL_Id && a.ISMS_Id == c.ISMS_Id && a.ESARMSCH_Id == data.ESARMSCH_Id)
                                                       select new School_Exam_Date_RoomDTO
                                                       {
                                                           ASMCL_ClassName = b.ASMCL_ClassName,
                                                           ISMS_SubjectName = c.ISMS_SubjectName,
                                                           ESARMSCH_Id = a.ESARMSCH_Id,
                                                           ESARMCSSCH_Id = a.ESARMCSSCH_Id,
                                                           ASMCL_Order = b.ASMCL_Order,
                                                           ESARMCSSCH_ActiveFlg = a.ESARMCSSCH_ActiveFlg
                                                       }).Distinct().OrderBy(a=>a.ASMCL_Order).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public School_Exam_Date_RoomDTO SaveorUpdateRoomClassSubjectDetails(School_Exam_Date_RoomDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

                List<long> id = new List<long>();

                foreach (var c in data.School_Room_ClassSubject_Temp_Details)
                {
                    id.Add(c.ESARMCSSCH_Id);
                }

                Array removeids = _context.School_Exam_SA_Room_ClassSubject_SchoolDMO.Where(a => !id.Contains(a.ESARMCSSCH_Id)
                && a.ESARMSCH_Id == data.ESARMSCH_Id).ToArray();

                foreach (School_Exam_SA_Room_ClassSubject_SchoolDMO ph1 in removeids)
                {
                    _context.Remove(ph1);
                }

                foreach (var c in data.School_Room_ClassSubject_Temp_Details)
                {
                    if (c.ESARMCSSCH_Id > 0)
                    {
                        var Room_Noresult = _context.School_Exam_SA_Room_ClassSubject_SchoolDMO.Single(t => t.ESARMCSSCH_Id == c.ESARMCSSCH_Id);
                        Room_Noresult.ESARMCSSCH_UpdatedBy = data.UserId;
                        Room_Noresult.ESARMCSSCH_UpdatedDate = indiantime0;
                        Room_Noresult.ASMCL_Id = c.ASMCL_Id;
                        Room_Noresult.ISMS_Id = c.ISMS_Id;
                        _context.Update(Room_Noresult);
                    }
                    else
                    {
                        School_Exam_SA_Room_ClassSubject_SchoolDMO exam_SA_ExamDate_RoomDMO = new School_Exam_SA_Room_ClassSubject_SchoolDMO();

                        exam_SA_ExamDate_RoomDMO.ISMS_Id = c.ISMS_Id;
                        exam_SA_ExamDate_RoomDMO.ASMCL_Id = c.ASMCL_Id;
                        exam_SA_ExamDate_RoomDMO.ESARMCSSCH_ActiveFlg = true;
                        exam_SA_ExamDate_RoomDMO.ESARMSCH_Id = data.ESARMSCH_Id;
                        exam_SA_ExamDate_RoomDMO.ESARMCSSCH_UpdatedBy = data.UserId;
                        exam_SA_ExamDate_RoomDMO.ESARMCSSCH_UpdatedDate = indiantime0;
                        exam_SA_ExamDate_RoomDMO.ESARMCSSCH_CreatedBy = data.UserId;
                        exam_SA_ExamDate_RoomDMO.ESARMCSSCH_CreatedDate = indiantime0;
                        _context.Add(exam_SA_ExamDate_RoomDMO);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        // Seating Arrangment Allotment
        public School_Exam_Date_RoomDTO SchoolSAAllotmentloaddata(School_Exam_Date_RoomDTO data)
        {
            try
            {
                data.GetAcademicYearList = _context.AcademicYearDMO.Where(a => a.MI_Id == data.MI_Id && a.Is_Active == true).OrderByDescending(a => a.ASMAY_Order).ToArray();

                data.GetExamList = _context.exammasterDMO.Where(a => a.MI_Id == data.MI_Id && a.EME_ActiveFlag == true).OrderBy(a => a.EME_ExamOrder).ToArray();

                data.GetExamSlotList = _context.Exam_SA_ExamSlotDMO.Where(a => a.MI_Id == data.MI_Id && a.ESAESLOT_ActiveFlg == true).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }        
        public School_Exam_Date_RoomDTO SchoolGenerateSeatAllotment(School_Exam_Date_RoomDTO data)
        {
            try
            {
                string confromdate = "";
                DateTime fromdate = DateTime.Now;

                fromdate = Convert.ToDateTime(data.ESAEDATESCH_ExamDate.Date.ToString("yyyy-MM-dd"));

                confromdate = fromdate.ToString("yyyy-MM-dd");

                var outputval1 = _context.Database.ExecuteSqlCommand("SA_School_ExamSlotwise_StudentsRecords_Insert  @p0,@p1,@p2,@p3,@p4",
                                          data.MI_Id, data.ASMAY_Id, data.EME_Id, data.ESAESLOT_Id, confromdate);

                if (outputval1 >= 1)
                {
                    data.ReturnValue = true;
                }
                else
                {
                    data.ReturnValue = false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        //Seat Allotment Report 
        public School_Exam_Date_RoomDTO GetSeatAllotedReport(School_Exam_Date_RoomDTO data)
        {
            try
            {
                data.GetAcademicYearList = _context.AcademicYearDMO.Where(a => a.MI_Id == data.MI_Id && a.Is_Active == true).OrderByDescending(a => a.ASMAY_Order).ToArray();

                data.GetExamList = _context.exammasterDMO.Where(a => a.MI_Id == data.MI_Id && a.EME_ActiveFlag == true).OrderBy(a => a.EME_ExamOrder).ToArray();

                data.GetExamSlotList = _context.Exam_SA_ExamSlotDMO.Where(a => a.MI_Id == data.MI_Id && a.ESAESLOT_ActiveFlg == true).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        } 
        public School_Exam_Date_RoomDTO GetSchoolSeatAllotementReport(School_Exam_Date_RoomDTO data)
        {
            try
            {
                string confromdate = "";
                DateTime fromdate = DateTime.Now;

                fromdate = Convert.ToDateTime(data.ExamDate.Value.Date.ToString("yyyy-MM-dd"));

                confromdate = fromdate.ToString("yyyy-MM-dd");

                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "SeatingArrangment_School_SeatAllotment_Report";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id }); 
                    cmd.Parameters.Add(new SqlParameter("@EME_Id", SqlDbType.VarChar) { Value = data.EME_Id });
                    cmd.Parameters.Add(new SqlParameter("@ESAESLOT_Id", SqlDbType.VarChar) { Value = data.ESAESLOT_Id });
                    cmd.Parameters.Add(new SqlParameter("@ExamDate", SqlDbType.VarChar) { Value = confromdate });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                                {
                                    dataRow.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]);
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.GetSeatAllotedReport = retObject.ToArray();
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