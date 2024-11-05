using DataAccessMsSqlServerProvider.SeatingArrangment;
using DomainModel.Model.SeatingArrangment;
using PreadmissionDTOs.SeatingArrangment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeatingArrangment.Services
{
    public class Exam_Room_DateImpl : Interface.Exam_Room_DateInterface
    {
        public SAMasterBuildingContext _context;
        public Exam_Room_DateImpl(SAMasterBuildingContext cn)
        {
            _context = cn;
        }
        /* Room Transaction Exam Date With Student Allotment */
        public Exam_Room_DateDTO GetExamDateloaddata(Exam_Room_DateDTO data)
        {
            try
            {
                data.Getyearlist = _context.AcademicYearDMO.Where(a => a.MI_Id == data.MI_Id && a.Is_Active == true).OrderByDescending(a => a.ASMAY_Order).ToArray();

                data.Getexamlisst = _context.exammasterDMO.Where(a => a.MI_Id == data.MI_Id && a.EME_ActiveFlag == true).OrderBy(a => a.EME_ExamOrder).ToArray();

                data.Getuniversityexamlist = _context.Exam_SA_University_ExamDMO.Where(a => a.MI_Id == data.MI_Id && a.ESAUE_ActiveFlag == true).OrderBy(a => a.ESAUE_ExamOrder).ToArray();

                data.Getcourselist = _context.MasterCourseDMO.Where(a => a.MI_Id == data.MI_Id && a.AMCO_ActiveFlag == true).OrderBy(a => a.AMCO_Order).ToArray();

                data.Getslotlist = _context.Exam_SA_ExamSlotDMO.Where(a => a.MI_Id == data.MI_Id && a.ESAESLOT_ActiveFlg == true).OrderBy(a => a.ESAESLOT_SlotName).ToArray();

                data.GetSavedDetails = (from a in _context.Exam_SA_ExamDateDMO
                                        from b in _context.AcademicYearDMO
                                        from c in _context.exammasterDMO
                                        from d in _context.Exam_SA_University_ExamDMO
                                        from e in _context.Exam_SA_ExamSlotDMO
                                        where (a.ASMAY_Id == b.ASMAY_Id && a.EME_Id == c.EME_Id && a.ESAUE_Id == d.ESAUE_Id && a.ESAESLOT_Id == e.ESAESLOT_Id
                                        && a.MI_Id == data.MI_Id)
                                        select new Exam_Room_DateDTO
                                        {
                                            ESAEDATE_Id = a.ESAEDATE_Id,                                            
                                            ASMAY_Id = a.ASMAY_Id,
                                            EME_Id = a.EME_Id,
                                            ESAUE_Id = a.ESAUE_Id,
                                            ESAEDATE_ExamDate = a.ESAEDATE_ExamDate,
                                            EME_ExamName = c.EME_ExamName,
                                            ESAESLOT_SlotName = e.ESAESLOT_SlotName,
                                            ESAUE_ExamName = d.ESAUE_ExamName,
                                            ASMAY_Year = b.ASMAY_Year,
                                            ASMAY_Order = b.ASMAY_Order,
                                            ESAEDATE_ActiveFlg = a.ESAEDATE_ActiveFlg
                                        }).Distinct().OrderByDescending(a => a.ASMAY_Order).ThenByDescending(a => a.ESAEDATE_ExamDate).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public Exam_Room_DateDTO GetSearchExamDateData(Exam_Room_DateDTO data)
        {
            try
            {
                data.Getroomdetails = (from a in _context.Exam_SA_BuildingDMO
                                       from b in _context.Exam_SA_FloorDMO
                                       from c in _context.Exam_SA_RoomDMO
                                       where (a.ESABLD_Id == b.ESABLD_Id && b.ESAFLR_Id == c.ESAFLR_Id && a.MI_Id == data.MI_Id
                                       && b.MI_Id == data.MI_Id && c.MI_Id == data.MI_Id)
                                       select c).Distinct().ToArray();

                data.Getsavedroomdetails = (from a in _context.Exam_SA_ExamDateDMO
                                            from b in _context.Exam_SA_ExamDate_RoomDMO
                                            from c in _context.Exam_SA_RoomDMO
                                            where (a.ESAEDATE_Id == b.ESAEDATE_Id && b.ESAROOM_Id == c.ESAROOM_Id && a.MI_Id == data.MI_Id
                                            && a.ASMAY_Id == data.ASMAY_Id && a.EME_Id == data.EME_Id && a.ESAESLOT_Id == data.ESAESLOT_Id
                                            && a.ESAUE_Id == data.ESAUE_Id)
                                            select b).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public Exam_Room_DateDTO SaveExamDateData(Exam_Room_DateDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

                if (data.ESAEDATE_Id > 0)
                {
                    var checkduplicate = _context.Exam_SA_ExamDateDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                    && a.ESAEDATE_ExamDate == data.ESAEDATE_ExamDate && a.ESAESLOT_Id == data.ESAESLOT_Id
                    && a.ESAEDATE_Id == data.ESAEDATE_Id).ToList();

                    if (checkduplicate.Count > 0)
                    {
                        var resultid = _context.Exam_SA_ExamDateDMO.Single(a => a.ESAEDATE_Id == data.ESAEDATE_Id);
                        resultid.ESAEDATE_UpdatedDate = indiantime0;
                        resultid.ESAEDATE_UpdatedBy = data.UserId;
                        _context.Update(resultid);

                        if (data.Room_Temp_Details != null && data.Room_Temp_Details.Length > 0)
                        {
                            AddUpdateRoomDetails(data);
                        }

                        var i = _context.SaveChanges();
                        if (i > 0)
                        {
                            data.Returnval = true;
                        }
                        else
                        {
                            data.Returnval = false;
                        }
                    }
                }
                else
                {
                    var checkduplicate = _context.Exam_SA_ExamDateDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                    && a.ESAEDATE_ExamDate == data.ESAEDATE_ExamDate && a.ESAESLOT_Id == data.ESAESLOT_Id).ToList();

                    if (checkduplicate.Count > 0)
                    {
                        data.ESAEDATE_Id = checkduplicate.FirstOrDefault().ESAEDATE_Id;

                        var getresultid = _context.Exam_SA_ExamDateDMO.Single(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                        && a.ESAEDATE_ExamDate == data.ESAEDATE_ExamDate && a.ESAESLOT_Id == data.ESAESLOT_Id
                        && a.ESAEDATE_Id == checkduplicate.FirstOrDefault().ESAEDATE_Id);

                        getresultid.ESAEDATE_UpdatedDate = indiantime0;
                        getresultid.ESAEDATE_UpdatedBy = data.UserId;
                        _context.Update(getresultid);

                        if (data.Room_Temp_Details != null && data.Room_Temp_Details.Length > 0)
                        {
                            AddUpdateRoomDetails(data);
                        }

                        var i = _context.SaveChanges();
                        if (i > 0)
                        {
                            data.Returnval = true;
                        }
                        else
                        {
                            data.Returnval = false;
                        }
                    }
                    else
                    {
                        Exam_SA_ExamDateDMO exam_SA_ExamDateDMO = new Exam_SA_ExamDateDMO();
                        exam_SA_ExamDateDMO.MI_Id = data.MI_Id;
                        exam_SA_ExamDateDMO.ASMAY_Id = data.ASMAY_Id;
                        exam_SA_ExamDateDMO.EME_Id = data.EME_Id;
                        exam_SA_ExamDateDMO.ESAESLOT_Id = data.ESAESLOT_Id;
                        exam_SA_ExamDateDMO.ESAEDATE_ExamDate = data.ESAEDATE_ExamDate;
                        exam_SA_ExamDateDMO.ESAEDATE_ActiveFlg = true;
                        exam_SA_ExamDateDMO.ESAEDATE_CreatedDate = indiantime0;
                        exam_SA_ExamDateDMO.ESAEDATE_UpdatedDate = indiantime0;
                        exam_SA_ExamDateDMO.ESAEDATE_CreatedBy = data.UserId;
                        exam_SA_ExamDateDMO.ESAEDATE_UpdatedBy = data.UserId;
                        exam_SA_ExamDateDMO.ESAUE_Id = data.ESAUE_Id;                        
                        _context.Add(exam_SA_ExamDateDMO);
                        data.ESAEDATE_Id = exam_SA_ExamDateDMO.ESAEDATE_Id;
                        if (data.Room_Temp_Details != null && data.Room_Temp_Details.Length > 0)
                        {
                            AddUpdateRoomDetails(data);
                        }

                        var i = _context.SaveChanges();
                        if (i > 0)
                        {
                            data.Returnval = true;
                        }
                        else
                        {
                            data.Returnval = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                data.Message = "Error";
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public Exam_Room_DateDTO EditExamDateData(Exam_Room_DateDTO data)
        {
            try
            {
                data.GetEditedDateDetails = _context.Exam_SA_ExamDateDMO.Where(a => a.MI_Id == data.MI_Id && a.ESAEDATE_Id == data.ESAEDATE_Id).ToArray();

                data.GetEditedRoomDetails = (from a in _context.Exam_SA_ExamDateDMO
                                             from b in _context.Exam_SA_ExamDate_RoomDMO
                                             from c in _context.Exam_SA_RoomDMO
                                             where (a.ESAEDATE_Id == b.ESAEDATE_Id && b.ESAROOM_Id == c.ESAROOM_Id && b.ESAEDATE_Id == data.ESAEDATE_Id)
                                             select b).Distinct().ToArray();

                data.Getroomdetails = (from a in _context.Exam_SA_BuildingDMO
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
        public Exam_Room_DateDTO ViewRoomDetails(Exam_Room_DateDTO data)
        {
            try
            {
                data.GetViewRoomDetails = (from a in _context.Exam_SA_ExamDate_RoomDMO
                                           from b in _context.Exam_SA_ExamDateDMO
                                           from c in _context.Exam_SA_RoomDMO
                                           where (a.ESAEDATE_Id == b.ESAEDATE_Id && a.ESAROOM_Id == c.ESAROOM_Id && b.MI_Id == data.MI_Id
                                           && a.ESAEDATE_Id == data.ESAEDATE_Id)
                                           select new Exam_Room_DateDTO
                                           {
                                               ESAEDATED_Id = a.ESAEDATED_Id,
                                               ESAROOM_Id = a.ESAROOM_Id,
                                               //ESAEDATED_AllotedCapacity = a.ESAEDATED_AllotedCapacity,
                                               ESAEDATED_ActiveFlg = a.ESAEDATED_ActiveFlg,
                                               ESAROOM_RoomName = c.ESAROOM_RoomName,
                                               ESAROOM_RoomMaxCapacity = c.ESAROOM_RoomMaxCapacity,
                                               ESAEDATE_Id = a.ESAEDATE_Id
                                           }).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public Exam_Room_DateDTO ActiveDeactiveExamDate(Exam_Room_DateDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

                var checkresult = _context.Exam_SA_ExamDateDMO.Where(a => a.MI_Id == data.MI_Id && a.ESAEDATE_Id == data.ESAEDATE_Id).ToList();
                if (checkresult.Count > 0)
                {
                    var result = _context.Exam_SA_ExamDateDMO.Single(a => a.MI_Id == data.MI_Id && a.ESAEDATE_Id == data.ESAEDATE_Id);
                    result.ESAEDATE_ActiveFlg = result.ESAEDATE_ActiveFlg == true ? false : true;
                    result.ESAEDATE_UpdatedBy = data.UserId;
                    result.ESAEDATE_UpdatedDate = indiantime0;
                    _context.Update(result);

                    var i = _context.SaveChanges();
                    if (i > 0)
                    {
                        data.Returnval = true;
                    }
                    else
                    {
                        data.Returnval = false;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public Exam_Room_DateDTO ActiveDeactiveRoomDetails(Exam_Room_DateDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

                var checkresult = _context.Exam_SA_ExamDate_RoomDMO.Where(a => a.ESAEDATED_Id == data.ESAEDATED_Id).ToList();
                if (checkresult.Count > 0)
                {
                    var result = _context.Exam_SA_ExamDate_RoomDMO.Single(a => a.ESAEDATED_Id == data.ESAEDATED_Id);
                    result.ESAEDATED_ActiveFlg = result.ESAEDATED_ActiveFlg == true ? false : true;
                    result.ESAEDATED_UpdatedBy = data.UserId;
                    result.ESAEDATED_UpdatedDate = indiantime0;
                    _context.Update(result);

                    var i = _context.SaveChanges();
                    if (i > 0)
                    {
                        data.Returnval = true;
                    }
                    else
                    {
                        data.Returnval = false;
                    }


                    data.GetViewRoomDetails = (from a in _context.Exam_SA_ExamDate_RoomDMO
                                               from b in _context.Exam_SA_ExamDateDMO
                                               from c in _context.Exam_SA_RoomDMO
                                               where (a.ESAEDATE_Id == b.ESAEDATE_Id && a.ESAROOM_Id == c.ESAROOM_Id && b.MI_Id == data.MI_Id
                                               && a.ESAEDATE_Id == data.ESAEDATE_Id)
                                               select new Exam_Room_DateDTO
                                               {
                                                   ESAEDATED_Id = a.ESAEDATED_Id,
                                                   ESAROOM_Id = a.ESAROOM_Id,
                                                   //ESAEDATED_AllotedCapacity = a.ESAEDATED_AllotedCapacity,
                                                   ESAEDATED_ActiveFlg = a.ESAEDATED_ActiveFlg,
                                                   ESAROOM_RoomName = c.ESAROOM_RoomName,
                                                   ESAROOM_RoomMaxCapacity = c.ESAROOM_RoomMaxCapacity,
                                                   ESAEDATE_Id = a.ESAEDATE_Id
                                               }).Distinct().ToArray();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public Exam_Room_DateDTO CheckCount(Exam_Room_DateDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);
                long? count = 0;
                count += data.ESAEDATED_AllotedCapacity;
                if (data.ESAEDATED_Id > 0)
                {
                    var Getroomsavedcounts = (from b in _context.Exam_SA_ExamDate_RoomDMO
                                              from a in _context.Exam_SA_ExamDateDMO
                                              where (a.ESAEDATE_Id == b.ESAEDATE_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                                              && a.EME_Id == data.EME_Id && a.ESAUE_Id == data.ESAUE_Id && a.ESAEDATE_ExamDate == data.ESAEDATE_ExamDate
                                              && b.ESAROOM_Id == data.ESAROOM_Id && b.ESAEDATED_Id !=data.ESAEDATED_Id)
                                              select b).ToList();

                    if (Getroomsavedcounts.Count > 0)
                    {
                        foreach (var c in Getroomsavedcounts)
                        {
                            //count += c.ESAEDATED_AllotedCapacity;
                        }
                    }

                    if (count > 0)
                    {
                        if (count > data.ESAROOM_RoomMaxCapacity)
                        {
                            data.Message = "Room Capactity";
                        }
                        if (count == data.ESAROOM_RoomMaxCapacity)
                        {
                            data.Message = "Max Capactity";
                        }
                    }
                }
                else
                {
                    var Getroomsavedcounts = (from b in _context.Exam_SA_ExamDate_RoomDMO
                                              from a in _context.Exam_SA_ExamDateDMO
                                              where (a.ESAEDATE_Id == b.ESAEDATE_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                                              && a.EME_Id == data.EME_Id && a.ESAUE_Id == data.ESAUE_Id && a.ESAEDATE_ExamDate == data.ESAEDATE_ExamDate
                                              && b.ESAROOM_Id == data.ESAROOM_Id)
                                              select b).ToList();

                    if (Getroomsavedcounts.Count > 0)
                    {
                        foreach (var c in Getroomsavedcounts)
                        {
                            //count += c.ESAEDATED_AllotedCapacity;
                        }
                    }

                    if (count > 0)
                    {
                        if (count > data.ESAROOM_RoomMaxCapacity)
                        {
                            data.Message = "Room Capactity";
                        }

                        if (count == data.ESAROOM_RoomMaxCapacity)
                        {
                            data.Message = "Max Capactity";
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
        public Exam_Room_DateDTO AddUpdateRoomDetails(Exam_Room_DateDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

                List<long> id = new List<long>();

                foreach (var c in data.Room_Temp_Details)
                {
                    id.Add(c.ESAEDATED_Id);
                }

                Array removeids = _context.Exam_SA_ExamDate_RoomDMO.Where(a => !id.Contains(a.ESAEDATED_Id) && a.ESAEDATE_Id == data.ESAEDATE_Id).ToArray();

                foreach (Exam_SA_ExamDate_RoomDMO ph1 in removeids)
                {
                    _context.Remove(ph1);
                }

                foreach (var c in data.Room_Temp_Details)
                {
                    if (c.ESAEDATED_Id > 0)
                    {
                        var Room_Noresult = _context.Exam_SA_ExamDate_RoomDMO.Single(t => t.ESAEDATED_Id == c.ESAEDATED_Id);
                        //Room_Noresult.ESAEDATED_AllotedCapacity = c.ESAEDATED_AllotedCapacity;
                        Room_Noresult.ESAEDATED_UpdatedBy = data.UserId;
                        Room_Noresult.ESAEDATED_UpdatedDate = indiantime0;
                        _context.Update(Room_Noresult);
                    }
                    else
                    {
                        Exam_SA_ExamDate_RoomDMO exam_SA_ExamDate_RoomDMO = new Exam_SA_ExamDate_RoomDMO();
                        //exam_SA_ExamDate_RoomDMO.ESAEDATED_AllotedCapacity = c.ESAEDATED_AllotedCapacity;
                        exam_SA_ExamDate_RoomDMO.ESAROOM_Id = c.ESAROOM_Id;
                        exam_SA_ExamDate_RoomDMO.ESAEDATE_Id = data.ESAEDATE_Id;
                        exam_SA_ExamDate_RoomDMO.ESAROOM_Id = c.ESAROOM_Id;
                        exam_SA_ExamDate_RoomDMO.ESAEDATED_ActiveFlg = true;
                        exam_SA_ExamDate_RoomDMO.ESAEDATED_UpdatedBy = data.UserId;
                        exam_SA_ExamDate_RoomDMO.ESAEDATED_UpdatedDate = indiantime0;
                        exam_SA_ExamDate_RoomDMO.ESAEDATED_CreatedBy = data.UserId;
                        exam_SA_ExamDate_RoomDMO.ESAEDATED_CreatedDate = indiantime0;
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

        /* Room Sitting Style Details */
        public Exam_Room_DateDTO GetRoomSittingStyleloaddata(Exam_Room_DateDTO data)
        {
            try
            {
                data.GetRoomSittingStyleDetails = _context.Exam_SA_Room_Sitting_StyleDMO.Where(a => a.MI_Id == data.MI_Id).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public Exam_Room_DateDTO SaveRoomSittingStyle(Exam_Room_DateDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

                if (data.ESARSITSTY_Id > 0)
                {
                    data.Message = "Update";
                    var result = _context.Exam_SA_Room_Sitting_StyleDMO.Single(a => a.MI_Id == data.MI_Id && a.ESARSITSTY_Id == data.ESARSITSTY_Id);                    
                    result.ESARSITSTY_SameBranchSameSemFlg = data.ESARSITSTY_SameBranchSameSemFlg;
                    result.ESARSITSTY_DiffBranchSameSemFlg = data.ESARSITSTY_DiffBranchSameSemFlg;
                    result.ESARSITSTY_SameBranchDiffSemFlg = data.ESARSITSTY_SameBranchDiffSemFlg;
                    result.ESARSITSTY_DiffBranchDiffSemFlg = data.ESARSITSTY_DiffBranchDiffSemFlg;
                    result.ESARSITSTY_AnyBranchAnySemFlg = data.ESARSITSTY_AnyBranchAnySemFlg;
                    result.ESARSITSTY_UpdatedDate = indiantime0;
                    result.ESARSITSTY_UpdatedBy = data.UserId;
                    _context.Update(result);
                    var i = _context.SaveChanges();
                    if (i > 0)
                    {
                        data.Returnval = true;
                    }
                    else
                    {
                        data.Returnval = false;
                    }
                }
                else
                {
                    data.Message = "Add";
                    Exam_SA_Room_Sitting_StyleDMO exam_SA_Room_Sitting_StyleDMO = new Exam_SA_Room_Sitting_StyleDMO();
                    exam_SA_Room_Sitting_StyleDMO.MI_Id = data.MI_Id;
                    exam_SA_Room_Sitting_StyleDMO.ESARSITSTY_SameBranchSameSemFlg = data.ESARSITSTY_SameBranchSameSemFlg;
                    exam_SA_Room_Sitting_StyleDMO.ESARSITSTY_DiffBranchSameSemFlg = data.ESARSITSTY_DiffBranchSameSemFlg;
                    exam_SA_Room_Sitting_StyleDMO.ESARSITSTY_SameBranchDiffSemFlg = data.ESARSITSTY_SameBranchDiffSemFlg;
                    exam_SA_Room_Sitting_StyleDMO.ESARSITSTY_DiffBranchDiffSemFlg = data.ESARSITSTY_DiffBranchDiffSemFlg;
                    exam_SA_Room_Sitting_StyleDMO.ESARSITSTY_AnyBranchAnySemFlg = data.ESARSITSTY_AnyBranchAnySemFlg;
                    exam_SA_Room_Sitting_StyleDMO.ESARSITSTY_ActiveFlg = true;
                    exam_SA_Room_Sitting_StyleDMO.ESARSITSTY_CreatedDate = indiantime0;
                    exam_SA_Room_Sitting_StyleDMO.ESARSITSTY_UpdatedDate= indiantime0;
                    exam_SA_Room_Sitting_StyleDMO.ESARSITSTY_CreatedBy = data.UserId;
                    exam_SA_Room_Sitting_StyleDMO.ESARSITSTY_UpdatedBy = data.UserId;
                    _context.Add(exam_SA_Room_Sitting_StyleDMO);
                    var i = _context.SaveChanges();
                    if (i > 0)
                    {
                        data.Returnval = true;
                    }
                    else
                    {
                        data.Returnval = false;
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
