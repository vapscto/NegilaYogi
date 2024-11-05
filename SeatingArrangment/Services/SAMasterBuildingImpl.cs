using DataAccessMsSqlServerProvider.SeatingArrangment;
using DomainModel.Model.SeatingArrangment;
using PreadmissionDTOs.SeatingArrangment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeatingArrangment.Services
{
    public class SAMasterBuildingImpl : Interface.SAMasterBuildingInterface
    {
        public SAMasterBuildingContext _context;

        public SAMasterBuildingImpl(SAMasterBuildingContext _cont)
        {
            _context = _cont;
        }

        public SAMasterBuildingDTO LoadData(SAMasterBuildingDTO data)
        {
            try
            {
                data.getbuildingdetails = _context.Exam_SA_BuildingDMO.Where(a => a.MI_Id == data.MI_Id).Distinct().OrderByDescending(a => a.ESABLD_CreatedDate).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public SAMasterBuildingDTO SaveMasterBuilding(SAMasterBuildingDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

                if (data.ESABLD_Id > 0)
                {
                    var checkduplicate = _context.Exam_SA_BuildingDMO.Where(a => a.MI_Id == data.MI_Id && a.ESABLD_BuildingName.Equals(data.ESABLD_BuildingName)
                     && a.ESABLD_Id != data.ESABLD_Id).ToList();
                    if (checkduplicate.Count == 0)
                    {
                        data.message = "Update";
                        var checkresult = _context.Exam_SA_BuildingDMO.Single(a => a.MI_Id == data.MI_Id && a.ESABLD_Id == data.ESABLD_Id);
                        checkresult.ESABLD_BuildingName = data.ESABLD_BuildingName;
                        checkresult.ESABLD_BuildingDesc = data.ESABLD_BuildingDesc;
                        checkresult.ESABLD_UpdatedBy = data.UserId;
                        checkresult.ESABLD_UpdatedDate = indiantime0;
                        _context.Update(checkresult);
                        var i = _context.SaveChanges();
                        if (i > 0)
                        {
                            data.returnval = true;
                        }
                        else
                        {
                            data.returnval = false;
                        }
                    }
                    else
                    {
                        data.message = "Duplicate";
                    }
                }
                else
                {
                    data.message = "Add";
                    var checkduplicate = _context.Exam_SA_BuildingDMO.Where(a => a.MI_Id == data.MI_Id
                    && a.ESABLD_BuildingName.Equals(data.ESABLD_BuildingName)).ToList();
                    if (checkduplicate.Count == 0)
                    {
                        Exam_SA_BuildingDMO exam_SA_BuildingDMO = new Exam_SA_BuildingDMO();
                        exam_SA_BuildingDMO.MI_Id = data.MI_Id;
                        exam_SA_BuildingDMO.ESABLD_BuildingName = data.ESABLD_BuildingName;
                        exam_SA_BuildingDMO.ESABLD_BuildingDesc = data.ESABLD_BuildingDesc;
                        exam_SA_BuildingDMO.ESABLD_UpdatedBy = data.UserId;
                        exam_SA_BuildingDMO.ESABLD_CreatedBy = data.UserId;
                        exam_SA_BuildingDMO.ESABLD_UpdatedDate = indiantime0;
                        exam_SA_BuildingDMO.ESABLD_CreatedDate = indiantime0;
                        exam_SA_BuildingDMO.ESABLD_ActiveFlg = true;
                        _context.Add(exam_SA_BuildingDMO);
                        var i = _context.SaveChanges();
                        if (i > 0)
                        {
                            data.returnval = true;
                        }
                        else
                        {
                            data.returnval = false;
                        }
                    }
                    else
                    {
                        data.message = "Duplicate";
                    }
                }
            }
            catch (Exception ex)
            {
                data.message = "Error";
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public SAMasterBuildingDTO EditMasterBuilding(SAMasterBuildingDTO data)
        {
            try
            {
                data.geteditbuildingdetails = _context.Exam_SA_BuildingDMO.Where(a => a.MI_Id == data.MI_Id && a.ESABLD_Id == data.ESABLD_Id).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public SAMasterBuildingDTO ActiveDeactiveMasterBuilding(SAMasterBuildingDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

                var checkrecord = _context.Exam_SA_BuildingDMO.Single(a => a.MI_Id == data.MI_Id && a.ESABLD_Id == data.ESABLD_Id);

                if (checkrecord.ESABLD_ActiveFlg == true)
                {
                    var checkmapped = _context.Exam_SA_FloorDMO.Where(a => a.MI_Id == data.MI_Id && a.ESABLD_Id == data.ESABLD_Id).ToList();

                    var checkmapped1 = _context.Exam_SA_FloorDMO.Where(a => a.MI_Id == data.MI_Id && a.ESABLD_Id == data.ESABLD_Id).ToList();

                    if (checkmapped.Count > 0 || checkmapped1.Count > 0)
                    {
                        data.message = "Mapped";
                    }
                    else
                    {
                        checkrecord.ESABLD_ActiveFlg = false;
                        checkrecord.ESABLD_UpdatedBy = data.UserId;
                        checkrecord.ESABLD_UpdatedDate = indiantime0;
                        _context.Update(checkrecord);
                        var i = _context.SaveChanges();
                        if (i > 0)
                        {
                            data.returnval = true;
                        }
                        else
                        {
                            data.returnval = false;
                        }
                    }
                }
                else
                {
                    checkrecord.ESABLD_ActiveFlg = true;
                    checkrecord.ESABLD_UpdatedBy = data.UserId;
                    checkrecord.ESABLD_UpdatedDate = indiantime0;
                    _context.Update(checkrecord);
                    var i = _context.SaveChanges();
                    if (i > 0)
                    {
                        data.returnval = true;
                    }
                    else
                    {
                        data.returnval = false;
                    }
                }
            }
            catch (Exception ex)
            {
                data.returnval = false;
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        // Master Floor Data
        public SAMasterBuildingDTO OnFloorDataLoad(SAMasterBuildingDTO data)
        {
            try
            {
                data.getmasterbuilding = _context.Exam_SA_BuildingDMO.Where(a => a.MI_Id == data.MI_Id && a.ESABLD_ActiveFlg == true).Distinct().ToArray();

                data.getfloordetails = (from a in _context.Exam_SA_FloorDMO
                                        from b in _context.Exam_SA_BuildingDMO
                                        where (a.ESABLD_Id == b.ESABLD_Id && a.MI_Id == data.MI_Id)
                                        select new SAMasterBuildingDTO
                                        {
                                            ESAFLR_Id = a.ESAFLR_Id,
                                            ESABLD_Id = a.ESABLD_Id,
                                            ESAFLR_FloorName = a.ESAFLR_FloorName,
                                            ESAFLR_FloorDesc = a.ESAFLR_FloorDesc,

                                            ESABLD_BuildingName = b.ESABLD_BuildingName,
                                            Createdate = a.ESAFLR_CreatedDate,
                                            ESAFLR_ActiveFlg = a.ESAFLR_ActiveFlg

                                        }).Distinct().OrderByDescending(a => a.Createdate).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public SAMasterBuildingDTO SaveMasterFloor(SAMasterBuildingDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

                if (data.ESAFLR_Id > 0)
                {
                    var checkduplicate = _context.Exam_SA_FloorDMO.Where(a => a.MI_Id == data.MI_Id && a.ESAFLR_FloorName.Equals(data.ESAFLR_FloorName)
                   && a.ESABLD_Id == data.ESABLD_Id && a.ESAFLR_Id != data.ESAFLR_Id).ToList();
                    if (checkduplicate.Count > 0)
                    {
                        data.message = "Duplicate";
                    }
                    else
                    {
                        data.message = "Update";
                        var checkresult = _context.Exam_SA_FloorDMO.Single(a => a.MI_Id == data.MI_Id && a.ESAFLR_Id == data.ESAFLR_Id);
                        checkresult.ESAFLR_FloorName = data.ESAFLR_FloorName;
                        checkresult.ESAFLR_FloorDesc = data.ESAFLR_FloorDesc;
                        checkresult.ESAFLR_UpdatedBy = data.UserId;
                        checkresult.ESAFLR_UpdatedDate = indiantime0;
                        _context.Update(checkresult);
                        var i = _context.SaveChanges();
                        if (i > 0)
                        {
                            data.returnval = true;
                        }
                        else
                        {
                            data.returnval = false;
                        }
                    }
                }
                else
                {
                    var checkduplicate = _context.Exam_SA_FloorDMO.Where(a => a.MI_Id == data.MI_Id && a.ESAFLR_FloorName.Equals(data.ESAFLR_FloorName)
                    && a.ESABLD_Id == data.ESABLD_Id).ToList();
                    if (checkduplicate.Count > 0)
                    {
                        data.message = "Duplicate";
                    }
                    else
                    {
                        data.message = "Add";

                        Exam_SA_FloorDMO exam_SA_FloorDMO = new Exam_SA_FloorDMO();
                        exam_SA_FloorDMO.MI_Id = data.MI_Id;
                        exam_SA_FloorDMO.ESABLD_Id = data.ESABLD_Id;
                        exam_SA_FloorDMO.ESAFLR_FloorName = data.ESAFLR_FloorName;
                        exam_SA_FloorDMO.ESAFLR_FloorDesc = data.ESAFLR_FloorDesc;
                        exam_SA_FloorDMO.ESAFLR_ActiveFlg = true;
                        exam_SA_FloorDMO.ESAFLR_UpdatedBy = data.UserId;
                        exam_SA_FloorDMO.ESAFLR_CreatedBy = data.UserId;
                        exam_SA_FloorDMO.ESAFLR_UpdatedDate = indiantime0;
                        exam_SA_FloorDMO.ESAFLR_CreatedDate = indiantime0;
                        _context.Add(exam_SA_FloorDMO);
                        var i = _context.SaveChanges();
                        if (i > 0)
                        {
                            data.returnval = true;
                        }
                        else
                        {
                            data.returnval = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                data.message = "Error";
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public SAMasterBuildingDTO EditMasterFloor(SAMasterBuildingDTO data)
        {
            try
            {
                data.geteditfloordetails = _context.Exam_SA_FloorDMO.Where(a => a.MI_Id == data.MI_Id && a.ESAFLR_Id == data.ESAFLR_Id).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public SAMasterBuildingDTO ActiveDeactiveMasterFloor(SAMasterBuildingDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

                var checkresult = _context.Exam_SA_FloorDMO.Single(a => a.MI_Id == data.MI_Id && a.ESAFLR_Id == data.ESAFLR_Id);

                if (checkresult.ESAFLR_ActiveFlg == true)
                {
                    var checkmapped = _context.Exam_SA_RoomDMO.Where(a => a.MI_Id == data.MI_Id && a.ESAFLR_Id == data.ESAFLR_Id).ToList();

                    if (checkmapped.Count > 0)
                    {
                        data.message = "Mapped";
                    }
                    else
                    {
                        checkresult.ESAFLR_ActiveFlg = false;
                        checkresult.ESAFLR_UpdatedBy = data.UserId;
                        checkresult.ESAFLR_UpdatedDate = indiantime0;
                        _context.Update(checkresult);
                        var i = _context.SaveChanges();
                        if (i > 0)
                        {
                            data.returnval = true;
                        }
                        else
                        {
                            data.returnval = false;
                        }
                    }
                }
                else
                {
                    checkresult.ESAFLR_ActiveFlg = true;
                    checkresult.ESAFLR_UpdatedBy = data.UserId;
                    checkresult.ESAFLR_UpdatedDate = indiantime0;
                    _context.Update(checkresult);
                    var i = _context.SaveChanges();
                    if (i > 0)
                    {
                        data.returnval = true;
                    }
                    else
                    {
                        data.returnval = false;
                    }
                }

            }
            catch (Exception ex)
            {
                data.returnval = false;
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        // Master Room
        public SAMasterBuildingDTO OnRoomDataLoad(SAMasterBuildingDTO data)
        {
            try
            {
                data.getroomdetails = (from a in _context.Exam_SA_RoomDMO
                                       from b in _context.Exam_SA_BuildingDMO
                                       from c in _context.Exam_SA_FloorDMO
                                       where (a.ESABLD_Id == b.ESABLD_Id && a.ESAFLR_Id == c.ESAFLR_Id && a.MI_Id == data.MI_Id)
                                       select new SAMasterBuildingDTO
                                       {
                                           ESAROOM_Id = a.ESAROOM_Id,
                                           ESABLD_BuildingName = b.ESABLD_BuildingName,
                                           ESAFLR_FloorName = c.ESAFLR_FloorName,
                                           ESAROOM_RoomName = a.ESAROOM_RoomName,
                                           ESAROOM_RoomDesc = a.ESAROOM_RoomDesc,
                                           ESAROOM_RoomMaxCapacity = a.ESAROOM_RoomMaxCapacity,
                                           ESAROOM_MaxNoOfRows = a.ESAROOM_MaxNoOfRows,
                                           ESAROOM_MaxNoOfColumns = a.ESAROOM_MaxNoOfColumns,
                                           ESAROOM_BenchCapacity = a.ESAROOM_BenchCapacity,
                                           ESAROOM_RoomTypeFlg = a.ESAROOM_RoomTypeFlg,
                                           ESAROOM_ActiveFlg = a.ESAROOM_ActiveFlg,
                                           Createdate = a.ESAROOM_CreatedDate,

                                       }).Distinct().OrderByDescending(a => a.Createdate).ToArray();

                data.getmasterbuilding = _context.Exam_SA_BuildingDMO.Where(a => a.MI_Id == data.MI_Id && a.ESABLD_ActiveFlg == true).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public SAMasterBuildingDTO OnChangeBuilding(SAMasterBuildingDTO data)
        {
            try
            {
                data.getbuildingfloordetails = _context.Exam_SA_FloorDMO.Where(a => a.MI_Id == data.MI_Id && a.ESABLD_Id == data.ESABLD_Id
                && a.ESAFLR_ActiveFlg == true).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public SAMasterBuildingDTO SaveMasterRoom(SAMasterBuildingDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

                if (data.ESAROOM_Id > 0)
                {
                    var checkdupliacte = _context.Exam_SA_RoomDMO.Where(a => a.MI_Id == data.MI_Id && a.ESABLD_Id == data.ESABLD_Id && a.ESAFLR_Id == data.ESAFLR_Id
                     && a.ESAROOM_RoomName.Equals(data.ESAROOM_RoomName) && a.ESAROOM_Id != data.ESAROOM_Id).Distinct().ToList();

                    if (checkdupliacte.Count > 0)
                    {
                        data.message = "Duplicate";
                    }
                    else
                    {
                        data.message = "Update";
                        var checkresult = _context.Exam_SA_RoomDMO.Single(a => a.MI_Id == data.MI_Id && a.ESAROOM_Id == data.ESAROOM_Id);
                        checkresult.ESAROOM_RoomName = data.ESAROOM_RoomName;
                        checkresult.ESAROOM_RoomDesc = data.ESAROOM_RoomDesc;
                        checkresult.ESAROOM_RoomMaxCapacity = data.ESAROOM_RoomMaxCapacity;
                        checkresult.ESAROOM_RoomTypeFlg = data.ESAROOM_RoomTypeFlg;
                        checkresult.ESAROOM_MaxNoOfRows = data.ESAROOM_MaxNoOfRows;
                        checkresult.ESAROOM_MaxNoOfColumns = data.ESAROOM_MaxNoOfColumns;
                        checkresult.ESAROOM_BenchCapacity = data.ESAROOM_BenchCapacity;
                        checkresult.ESAROOM_UpdatedBy = data.UserId;
                        checkresult.ESAROOM_UpdatedDate = indiantime0;

                        _context.Update(checkresult);
                        var i = _context.SaveChanges();
                        if (i > 0)
                        {
                            data.returnval = true;
                        }
                        else
                        {
                            data.returnval = false;
                        }
                    }
                }
                else
                {
                    var checkdupliacte = _context.Exam_SA_RoomDMO.Where(a => a.MI_Id == data.MI_Id && a.ESABLD_Id == data.ESABLD_Id && a.ESAFLR_Id == data.ESAFLR_Id
                     && a.ESAROOM_RoomName.Equals(data.ESAROOM_RoomName)).Distinct().ToList();
                    if (checkdupliacte.Count > 0)
                    {
                        data.message = "Duplicate";
                    }
                    else
                    {
                        data.message = "Add";
                        Exam_SA_RoomDMO exam_SA_RoomDMO = new Exam_SA_RoomDMO();
                        exam_SA_RoomDMO.MI_Id = data.MI_Id;
                        exam_SA_RoomDMO.ESABLD_Id = data.ESABLD_Id;
                        exam_SA_RoomDMO.ESAFLR_Id = data.ESAFLR_Id;
                        exam_SA_RoomDMO.ESAROOM_RoomName = data.ESAROOM_RoomName;
                        exam_SA_RoomDMO.ESAROOM_RoomDesc = data.ESAROOM_RoomDesc;
                        exam_SA_RoomDMO.ESAROOM_RoomMaxCapacity = data.ESAROOM_RoomMaxCapacity;
                        exam_SA_RoomDMO.ESAROOM_RoomTypeFlg = data.ESAROOM_RoomTypeFlg;
                        exam_SA_RoomDMO.ESAROOM_MaxNoOfRows = data.ESAROOM_MaxNoOfRows;
                        exam_SA_RoomDMO.ESAROOM_MaxNoOfColumns = data.ESAROOM_MaxNoOfColumns;
                        exam_SA_RoomDMO.ESAROOM_BenchCapacity = data.ESAROOM_BenchCapacity;
                        exam_SA_RoomDMO.ESAROOM_UpdatedBy = data.UserId;
                        exam_SA_RoomDMO.ESAROOM_UpdatedDate = indiantime0;
                        exam_SA_RoomDMO.ESAROOM_CreatedBy = data.UserId;
                        exam_SA_RoomDMO.ESAROOM_CreatedDate = indiantime0;
                        exam_SA_RoomDMO.ESAROOM_ActiveFlg = true;

                        _context.Add(exam_SA_RoomDMO);
                        var i = _context.SaveChanges();
                        if (i > 0)
                        {
                            data.returnval = true;
                        }
                        else
                        {
                            data.returnval = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                data.message = "Error";
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public SAMasterBuildingDTO EditMasterRoom(SAMasterBuildingDTO data)
        {
            try
            {
                var geteditroomdetails = _context.Exam_SA_RoomDMO.Where(a => a.MI_Id == data.MI_Id && a.ESAROOM_Id == data.ESAROOM_Id).ToList();
                data.geteditroomdetails = geteditroomdetails.ToArray();

                data.geteditbuildingfloordetails = _context.Exam_SA_FloorDMO.Where(a => a.MI_Id == data.MI_Id
                && a.ESABLD_Id == geteditroomdetails.FirstOrDefault().ESABLD_Id && a.ESAFLR_ActiveFlg == true).ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public SAMasterBuildingDTO ActiveDeactiveMasterRoom(SAMasterBuildingDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

                var checkresult = _context.Exam_SA_RoomDMO.Single(a => a.MI_Id == data.MI_Id && a.ESAROOM_Id == data.ESAROOM_Id);
                if (checkresult.ESAROOM_ActiveFlg == true)
                {
                    checkresult.ESAROOM_ActiveFlg = false;
                    checkresult.ESAROOM_UpdatedBy = data.UserId;
                    checkresult.ESAROOM_UpdatedDate = indiantime0;
                    _context.Update(checkresult);
                    var i = _context.SaveChanges();
                    if (i > 0)
                    {
                        data.returnval = true;
                    }
                    else
                    {
                        data.returnval = false;
                    }
                }
                else
                {
                    checkresult.ESAROOM_ActiveFlg = true;
                    checkresult.ESAROOM_UpdatedBy = data.UserId;
                    checkresult.ESAROOM_UpdatedDate = indiantime0;
                    _context.Update(checkresult);
                    var i = _context.SaveChanges();
                    if (i > 0)
                    {
                        data.returnval = true;
                    }
                    else
                    {
                        data.returnval = false;
                    }
                }
            }
            catch (Exception ex)
            {
                data.returnval = false;
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        // Master University Exam
        public SAMasterBuildingDTO OnUniversityExamLoadData(SAMasterBuildingDTO data)
        {
            try
            {
                data.getuniversityexamdetails = _context.Exam_SA_University_ExamDMO.Where(a => a.MI_Id == data.MI_Id).Distinct().OrderByDescending(a => a.CreatedDate).ToArray();

                data.getuniversityexamorderdetails = _context.Exam_SA_University_ExamDMO.Where(a => a.MI_Id == data.MI_Id).Distinct().OrderBy(a => a.ESAUE_ExamOrder).ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public SAMasterBuildingDTO SaveMasterUniversityExam(SAMasterBuildingDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

                if (data.ESAUE_Id > 0)
                {
                    var checkduplicate = _context.Exam_SA_University_ExamDMO.Where(a => a.MI_Id == data.MI_Id && a.ESAUE_ExamName.Equals(data.ESAUE_ExamName)
                    && a.ESAUE_Id != data.ESAUE_Id).ToList();

                    if (checkduplicate.Count > 0)
                    {
                        data.message = "Duplicate";
                    }
                    else
                    {
                        data.message = "Update";

                        var checkresult = _context.Exam_SA_University_ExamDMO.Single(a => a.MI_Id == data.MI_Id && a.ESAUE_Id == data.ESAUE_Id);
                        checkresult.ESAUE_ExamName = data.ESAUE_ExamName;
                        checkresult.ESAUE_ExamCode = data.ESAUE_ExamCode;
                        checkresult.UpdatedDate = indiantime0;
                        checkresult.ESAUE_UpdatedBy = data.UserId;
                        _context.Update(checkresult);
                        var i = _context.SaveChanges();
                        if (i > 0)
                        {
                            data.returnval = true;
                        }
                        else
                        {
                            data.returnval = false;
                        }
                    }
                }
                else
                {
                    var checkduplicate = _context.Exam_SA_University_ExamDMO.Where(a => a.MI_Id == data.MI_Id && a.ESAUE_ExamName.Equals(data.ESAUE_ExamName)).ToList();

                    if (checkduplicate.Count > 0)
                    {
                        data.message = "Duplicate";
                    }
                    else
                    {
                        data.message = "Add";

                        var getcount = _context.Exam_SA_University_ExamDMO.Where(a => a.MI_Id == data.MI_Id).Count();

                        Exam_SA_University_ExamDMO exam_SA_University_ExamDMO = new Exam_SA_University_ExamDMO();

                        exam_SA_University_ExamDMO.MI_Id = data.MI_Id;
                        exam_SA_University_ExamDMO.ESAUE_ExamName = data.ESAUE_ExamName;
                        exam_SA_University_ExamDMO.ESAUE_ExamCode = data.ESAUE_ExamCode;
                        exam_SA_University_ExamDMO.ESAUE_ActiveFlag = true;
                        exam_SA_University_ExamDMO.ESAUE_ExamOrder = getcount + 1;
                        exam_SA_University_ExamDMO.CreatedDate = indiantime0;
                        exam_SA_University_ExamDMO.UpdatedDate = indiantime0;
                        exam_SA_University_ExamDMO.ESAUE_CreatedBy = data.UserId;
                        exam_SA_University_ExamDMO.ESAUE_UpdatedBy = data.UserId;
                        _context.Add(exam_SA_University_ExamDMO);
                        var i = _context.SaveChanges();
                        if (i > 0)
                        {
                            data.returnval = true;
                        }
                        else
                        {
                            data.returnval = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                data.message = "Error";
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public SAMasterBuildingDTO EditMasterUniverstityExam(SAMasterBuildingDTO data)
        {
            try
            {
                data.getedituniversityexamdetails = _context.Exam_SA_University_ExamDMO.Where(a => a.MI_Id == data.MI_Id && a.ESAUE_Id == data.ESAUE_Id).ToArray();
            }
            catch (Exception ex)
            {
                data.message = "Error";
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public SAMasterBuildingDTO ActiveDeactiveMasterUniverstityExam(SAMasterBuildingDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

                var checkresult = _context.Exam_SA_University_ExamDMO.Single(a => a.MI_Id == data.MI_Id && a.ESAUE_Id == data.ESAUE_Id);
                if (checkresult.ESAUE_ActiveFlag == true)
                {
                    checkresult.ESAUE_ActiveFlag = false;
                }
                else
                {
                    checkresult.ESAUE_ActiveFlag = true;
                }
                checkresult.UpdatedDate = indiantime0;
                checkresult.ESAUE_UpdatedBy = data.UserId;
                _context.Update(checkresult);
                var i = _context.SaveChanges();
                if (i > 0)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }
            }
            catch (Exception ex)
            {
                data.returnval = false;
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public SAMasterBuildingDTO UpdateMasterUniversityExamOrder(SAMasterBuildingDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

                int id = 0;
                for (int i = 0; i < data.Temp_OrderUpdate.Length; i++)
                {
                    var ESAUE_Id = data.Temp_OrderUpdate[i].ESAUE_Id;

                    var reult = _context.Exam_SA_University_ExamDMO.Single(t => t.MI_Id == data.MI_Id && t.ESAUE_Id == ESAUE_Id);
                    id = id + 1;
                    reult.ESAUE_ExamOrder = id;
                    reult.ESAUE_UpdatedBy = data.UserId;
                    reult.UpdatedDate = indiantime0;

                    _context.Update(reult);
                }
                var flag = _context.SaveChanges();
                if (flag > 0)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }
            }
            catch (Exception ex)
            {
                data.returnval = false;
                Console.WriteLine(ex.Message);
            }
            return data;
        }


        // Master Duty Type
        public SAMasterBuildingDTO OnDutyTypeLoadData(SAMasterBuildingDTO data)
        {
            try
            {
                data.getdutytypedetails = _context.Exam_SA_Allot_Staff_DutyTypeDMO.Where(a => a.MI_Id == data.MI_Id).Distinct().OrderByDescending(a => a.ESAALLSTADTP_CreatedDate).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public SAMasterBuildingDTO SaveMasterDutyType(SAMasterBuildingDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

                if (data.ESAALLSTADTP_Id > 0)
                {
                    var checkduplicate = _context.Exam_SA_Allot_Staff_DutyTypeDMO.Where(a => a.MI_Id == data.MI_Id
                    && a.ESAALLSTADTP_DTName.Equals(data.ESAALLSTADTP_DTName) && a.ESAALLSTADTP_Id != data.ESAALLSTADTP_Id).ToList();

                    if (checkduplicate.Count > 0)
                    {
                        data.message = "Duplicate";
                    }
                    else
                    {
                        data.message = "Update";

                        var checkresult = _context.Exam_SA_Allot_Staff_DutyTypeDMO.Single(a => a.MI_Id == data.MI_Id && a.ESAALLSTADTP_Id == data.ESAALLSTADTP_Id);
                        checkresult.ESAALLSTADTP_DTName = data.ESAALLSTADTP_DTName;
                        checkresult.ESAALLSTADTP_UpdatedDate = indiantime0;
                        checkresult.ESAALLSTADTP_UpdatedBy = data.UserId;
                        _context.Update(checkresult);
                        var i = _context.SaveChanges();
                        if (i > 0)
                        {
                            data.returnval = true;
                        }
                        else
                        {
                            data.returnval = false;
                        }
                    }
                }
                else
                {
                    var checkduplicate = _context.Exam_SA_Allot_Staff_DutyTypeDMO.Where(a => a.MI_Id == data.MI_Id
                  && a.ESAALLSTADTP_DTName.Equals(data.ESAALLSTADTP_DTName)).ToList();

                    if (checkduplicate.Count > 0)
                    {
                        data.message = "Duplicate";
                    }
                    else
                    {
                        data.message = "Add";


                        Exam_SA_Allot_Staff_DutyTypeDMO exam_SA_Allot_Staff_DutyTypeDMO = new Exam_SA_Allot_Staff_DutyTypeDMO();

                        exam_SA_Allot_Staff_DutyTypeDMO.MI_Id = data.MI_Id;
                        exam_SA_Allot_Staff_DutyTypeDMO.ESAALLSTADTP_DTName = data.ESAALLSTADTP_DTName;
                        exam_SA_Allot_Staff_DutyTypeDMO.ESAALLSTADTP_ActiveFlag = true;
                        exam_SA_Allot_Staff_DutyTypeDMO.ESAALLSTADTP_CreatedDate = indiantime0;
                        exam_SA_Allot_Staff_DutyTypeDMO.ESAALLSTADTP_UpdatedDate = indiantime0;
                        exam_SA_Allot_Staff_DutyTypeDMO.ESAALLSTADTP_UpdatedBy = data.UserId;
                        exam_SA_Allot_Staff_DutyTypeDMO.ESAALLSTADTP_CreatedBy = data.UserId;
                        _context.Add(exam_SA_Allot_Staff_DutyTypeDMO);
                        var i = _context.SaveChanges();
                        if (i > 0)
                        {
                            data.returnval = true;
                        }
                        else
                        {
                            data.returnval = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                data.message = "Error";
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public SAMasterBuildingDTO EditDutyType(SAMasterBuildingDTO data)
        {
            try
            {
                data.geteditdutytypedetails = _context.Exam_SA_Allot_Staff_DutyTypeDMO.Where(a => a.MI_Id == data.MI_Id
                && a.ESAALLSTADTP_Id == data.ESAALLSTADTP_Id).ToArray();
            }
            catch (Exception ex)
            {
                data.message = "Error";
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public SAMasterBuildingDTO ActiveDeactiveDutyType(SAMasterBuildingDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

                var checkresult = _context.Exam_SA_Allot_Staff_DutyTypeDMO.Single(a => a.MI_Id == data.MI_Id && a.ESAALLSTADTP_Id == data.ESAALLSTADTP_Id);
                if (checkresult.ESAALLSTADTP_ActiveFlag == true)
                {
                    checkresult.ESAALLSTADTP_ActiveFlag = false;
                }
                else
                {
                    checkresult.ESAALLSTADTP_ActiveFlag = true;
                }
                checkresult.ESAALLSTADTP_UpdatedDate = indiantime0;
                checkresult.ESAALLSTADTP_UpdatedBy = data.UserId;
                _context.Update(checkresult);
                var i = _context.SaveChanges();
                if (i > 0)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }
            }
            catch (Exception ex)
            {
                data.returnval = false;
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        // Master Time Slot
        public SAMasterBuildingDTO OnTimeSlotLoadData(SAMasterBuildingDTO data)
        {
            try
            {
                data.getslottimedetails = _context.Exam_SA_ExamSlotDMO.Where(a => a.MI_Id == data.MI_Id).Distinct().OrderByDescending(a => a.ESAESLOT_CreatedDate).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public SAMasterBuildingDTO SaveMasterTimeSlot(SAMasterBuildingDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

                if (data.ESAESLOT_Id > 0)
                {
                    var checkduplicate = _context.Exam_SA_ExamSlotDMO.Where(a => a.MI_Id == data.MI_Id
                    && a.ESAESLOT_SlotName.Equals(data.ESAESLOT_SlotName) && a.ESAESLOT_Id != data.ESAESLOT_Id).ToList();

                    if (checkduplicate.Count > 0)
                    {
                        data.message = "Duplicate";
                    }
                    else
                    {
                        data.message = "Update";

                        var checkresult = _context.Exam_SA_ExamSlotDMO.Single(a => a.MI_Id == data.MI_Id && a.ESAESLOT_Id == data.ESAESLOT_Id);
                        checkresult.ESAESLOT_SlotName = data.ESAESLOT_SlotName;
                        checkresult.ESAESLOT_StartTime = data.ESAESLOT_StartTime;
                        checkresult.ESAESLOT_EndTime = data.ESAESLOT_EndTime;
                        checkresult.ESAESLOT_UpdatedDate = indiantime0;
                        checkresult.ESAESLOT_UpdatedBy = data.UserId;
                        _context.Update(checkresult);
                        var i = _context.SaveChanges();
                        if (i > 0)
                        {
                            data.returnval = true;
                        }
                        else
                        {
                            data.returnval = false;
                        }
                    }
                }
                else
                {
                    var checkduplicate = _context.Exam_SA_ExamSlotDMO.Where(a => a.MI_Id == data.MI_Id && a.ESAESLOT_SlotName.Equals(data.ESAESLOT_SlotName)).ToList();

                    if (checkduplicate.Count > 0)
                    {
                        data.message = "Duplicate";
                    }
                    else
                    {
                        data.message = "Add";


                        Exam_SA_ExamSlotDMO exam_SA_ExamSlotDMO = new Exam_SA_ExamSlotDMO();

                        exam_SA_ExamSlotDMO.MI_Id = data.MI_Id;
                        exam_SA_ExamSlotDMO.ESAESLOT_SlotName = data.ESAESLOT_SlotName;
                        exam_SA_ExamSlotDMO.ESAESLOT_StartTime = data.ESAESLOT_StartTime;
                        exam_SA_ExamSlotDMO.ESAESLOT_EndTime = data.ESAESLOT_EndTime;
                        exam_SA_ExamSlotDMO.ESAESLOT_ActiveFlg = true;
                        exam_SA_ExamSlotDMO.ESAESLOT_UpdatedDate = indiantime0;
                        exam_SA_ExamSlotDMO.ESAESLOT_CreatedDate = indiantime0;
                        exam_SA_ExamSlotDMO.ESAESLOT_UpdatedBy = data.UserId;
                        exam_SA_ExamSlotDMO.ESAESLOT_CreatedBy = data.UserId;
                        _context.Add(exam_SA_ExamSlotDMO);
                        var i = _context.SaveChanges();
                        if (i > 0)
                        {
                            data.returnval = true;
                        }
                        else
                        {
                            data.returnval = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                data.message = "Error";
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public SAMasterBuildingDTO EditTimeSlot(SAMasterBuildingDTO data)
        {
            try
            {
                data.geteditslottimedetails = _context.Exam_SA_ExamSlotDMO.Where(a => a.MI_Id == data.MI_Id && a.ESAESLOT_Id == data.ESAESLOT_Id).ToArray();
            }
            catch (Exception ex)
            {
                data.message = "Error";
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public SAMasterBuildingDTO ActiveDeactiveTimeSlot(SAMasterBuildingDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

                var checkresult = _context.Exam_SA_ExamSlotDMO.Single(a => a.MI_Id == data.MI_Id && a.ESAESLOT_Id == data.ESAESLOT_Id);
                if (checkresult.ESAESLOT_ActiveFlg == true)
                {
                    checkresult.ESAESLOT_ActiveFlg = false;
                }
                else
                {
                    checkresult.ESAESLOT_ActiveFlg = true;
                }
                checkresult.ESAESLOT_UpdatedDate = indiantime0;
                checkresult.ESAESLOT_UpdatedBy = data.UserId;
                _context.Update(checkresult);
                var i = _context.SaveChanges();
                if (i > 0)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }
            }
            catch (Exception ex)
            {
                data.returnval = false;
                Console.WriteLine(ex.Message);
            }
            return data;
        }
    }
}
