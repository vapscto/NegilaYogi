using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.Hostel;
using DomainModel.Model.com.vapstech.Hostel;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.Hostel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace HostelServiceHub.Services
{
    public class HostelAllotForCLGStudentIMPL : Interface.HostelAllotForCLGStudentInterface
    {
        public HostelContext _HostelContext;
        public DomainModelMsSqlServerContext _dbcontext;
        public HostelAllotForCLGStudentIMPL(HostelContext para1, DomainModelMsSqlServerContext para2)
        {
            _HostelContext = para1;
            _dbcontext = para2;
        }
        public async Task<HostelAllotForCLGStudentDTO> loaddata(HostelAllotForCLGStudentDTO data)
        {
            try
            {
                var yeardata = (from a in _dbcontext.AcademicYear
                                where (a.MI_Id == data.MI_Id && a.Is_Active == true)
                                select new HostelAllotForCLGStudentDTO { ASMAY_Id = a.ASMAY_Id, ASMAY_Year = a.ASMAY_Year }).Distinct().ToList();
                if (yeardata.Count > 0)
                {
                    data.yearlist = yeardata.ToArray();
                    data.ASMAY_Id = data.ASMAY_Id;
                }

                //data.hostel_list = (from a in _HostelContext.HL_Master_Hostel_DMO
                //                    from b in _HostelContext.HL_Hostel_Student_Request_College_Confirm_DMO
                //                    where (a.MI_Id == data.MI_Id && b.HLMH_Id == a.HLMH_Id && a.HLMH_ActiveFlag == true && b.HLHSREQCC_ActiveFlag == true && b.HLHSREQCC_BookingStatus == "Approved")
                //                    select new HostelAllotForCLGStudentDTO { HLMH_Id = a.HLMH_Id, HLMH_Name = a.HLMH_Name }).Distinct().ToArray();

                data.hostel_list = (from a in _HostelContext.HL_Master_Hostel_DMO
                                    where (a.MI_Id == data.MI_Id && a.HLMH_ActiveFlag == true)
                                    select new HostelAllotForCLGStudentDTO { HLMH_Id = a.HLMH_Id, HLMH_Name = a.HLMH_Name }).Distinct().ToArray();


                data.courselist = (from a in _dbcontext.CLGADM_master_courseDMO
                                   where (a.MI_Id == data.MI_Id && a.AMCO_ActiveFlag == true)
                                   select new HostelAllotForCLGStudentDTO
                                   {
                                       AMCO_Id = a.AMCO_Id,
                                       AMCO_CourseName = a.AMCO_CourseName
                                   }).Distinct().ToArray();

                data.semisterlist = (from a in _dbcontext.CLG_Adm_Master_SemesterDMO
                                     where (a.MI_Id == data.MI_Id && a.AMSE_ActiveFlg == true)
                                     select new HostelAllotForCLGStudentDTO
                                     {
                                         AMSE_Id = a.AMSE_Id,
                                         AMSE_SEMName = a.AMSE_SEMName
                                     }).Distinct().ToArray();

                data.Sectionlist = (from a in _HostelContext.Adm_College_Master_SectionDMO
                                    where (a.MI_Id == data.MI_Id && a.ACMS_ActiveFlag == true)
                                    select new HostelAllotForCLGStudentDTO
                                    {
                                        ACMS_Id = a.ACMS_Id,
                                        ACMS_SectionName = a.ACMS_SectionName
                                    }).Distinct().ToArray();

                data.branchlist = (from a in _dbcontext.ClgMasterBranchDMO
                                   where (a.MI_Id == data.MI_Id && a.AMB_ActiveFlag == true)
                                   select new HostelAllotForCLGStudentDTO
                                   {
                                       AMB_Id = a.AMB_Id,
                                       AMB_BranchName = a.AMB_BranchName
                                   }).Distinct().ToArray();



                using (var cmd = _HostelContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Hostel_Requested_Student_List";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
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
                        data.student_Requestlist = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }


                using (var cmd = _HostelContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "CLG_HOSTEL_ALLOT_FOR_STUDENT";
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
                        data.student_allotlist = retObject.ToArray();
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
        public HostelAllotForCLGStudentDTO savedata(HostelAllotForCLGStudentDTO data)
        {
            try
            {
                if (data.HLHSALTC_Id == 0)
                {
                    var duplicate = _HostelContext.HL_Hostel_Student_Allot_College_DMO.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.AMCST_Id == data.AMCST_Id && (t.HLHSALTC_VacateFlg == false || t.HLHSALTC_VacateFlg == null)).ToList();
                    if (duplicate.Count > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        HL_Hostel_Student_Allot_College_DMO obj = new HL_Hostel_Student_Allot_College_DMO();

                        obj.HLHSALTC_Id = data.HLHSALTC_Id;
                        obj.MI_Id = data.MI_Id;
                        obj.HLHSALTC_AllotmentDate = data.HLHSALTC_AllotmentDate;
                        obj.ASMAY_Id = data.ASMAY_Id;
                        obj.HLMH_Id = data.HLMH_Id;
                        obj.HLMRCA_Id = data.HLMRCA_Id;
                        obj.AMCST_Id = data.AMCST_Id;
                        obj.AMCO_Id = data.AMCO_Id;
                        obj.AMB_Id = data.AMB_Id;
                        obj.AMSE_Id = data.AMSE_Id;
                        obj.ACMS_Id = data.ACMS_Id;
                        obj.HRMRM_Id = data.HRMRM_Id;
                        obj.HLHSALTC_EntireRoomReqdFlg = data.HLHSALTC_EntireRoomReqdFlg;
                        obj.HLHSALTC_AllotRemarks = data.HLHSALTC_AllotRemarks;
                        obj.HLHSALTC_VacateFlg = data.HLHSALTC_VacateFlg;
                        obj.HLHSALTC_VacatedDate = data.HLHSALTC_VacatedDate;
                        obj.HLHSALTC_VacateRemarks = data.HLHSALTC_VacateRemarks;
                        obj.HLHSALTC_ActiveFlag = true;
                        obj.HLHSALTC_CreatedDate = DateTime.Now;
                        obj.HLHSALTC_UpdatedDate = DateTime.Now;
                        obj.HLHSALTC_UpdatedBy = data.UserId;
                        obj.HLHSALTC_CreatedBy = data.UserId;
                        _HostelContext.Add(obj);
                        int row = _HostelContext.SaveChanges();
                        if (row > 0)
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
                    var duplicate = _HostelContext.HL_Hostel_Student_Allot_College_DMO.Where(t => t.HLHSALTC_Id != data.HLHSALTC_Id && t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.AMCST_Id == data.AMCST_Id && t.ACMS_Id == data.ACMS_Id && t.AMB_Id == data.AMB_Id && t.AMSE_Id == data.AMSE_Id).ToList();

                    if (duplicate.Count > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        var update = _HostelContext.HL_Hostel_Student_Allot_College_DMO.Where(t => t.HLHSALTC_Id == data.HLHSALTC_Id && t.MI_Id == data.MI_Id).Single();
                        update.HLHSALTC_AllotmentDate = data.HLHSALTC_AllotmentDate;
                        update.ASMAY_Id = data.ASMAY_Id;
                        update.HLMH_Id = data.HLMH_Id;
                        update.HLMRCA_Id = data.HLMRCA_Id;
                        update.AMCST_Id = data.AMCST_Id;
                        update.ACMS_Id = data.ACMS_Id;
                        update.AMB_Id = data.AMB_Id;
                        update.AMSE_Id = data.AMSE_Id;
                        update.HRMRM_Id = data.HRMRM_Id;
                        update.HLHSALTC_EntireRoomReqdFlg = data.HLHSALTC_EntireRoomReqdFlg;
                        update.HLHSALTC_AllotRemarks = data.HLHSALTC_AllotRemarks;
                       // update.HLHSALTC_AllotRemarks = data.HLHSALTC_AllotRemarks;
                        update.HLHSALTC_VacateFlg = data.HLHSALTC_VacateFlg;
                        update.HLHSALTC_VacatedDate = data.HLHSALTC_VacatedDate;
                        update.HLHSALTC_VacateRemarks = data.HLHSALTC_VacateRemarks;
                        update.HLHSALTC_UpdatedDate = DateTime.Now;
                        update.HLHSALTC_UpdatedBy = data.UserId;
                        _HostelContext.Update(update);
                        int row = _HostelContext.SaveChanges();
                        if (row > 0)
                        {
                            data.returnval = true;
                        }
                        else
                        {
                            data.returnval = false;
                        }
                    }
                }

                var stu_rec_list = _HostelContext.FeeMasterConfigurationDMO.Where(t => t.MI_Id == data.MI_Id).FirstOrDefault();

                if (stu_rec_list.FMC_RoomwiseHostelFeeFlg == true || stu_rec_list.FMC_CommonHostelFeeFlg == true)
                {
                    try
                    {
                        var MapHostelFee = _HostelContext.Database.ExecuteSqlCommand("CLG_Auto_Fee_Group_Mapping_Hostel_Request @p0,@p1,@p2,@p3,@p4", data.MI_Id, data.ASMAY_Id, data.AMCST_Id, data.UserId, data.HLHSREQC_EntireRoomReqdFlg);
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
        public async Task<HostelAllotForCLGStudentDTO> get_studInfo(HostelAllotForCLGStudentDTO data)
        {
            try
            {
                using (var cmd = _HostelContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "CLG_HOSTEL_HOUSE_WISE_STUDENT_LIST_EDIT_RM";
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
                    cmd.Parameters.Add(new SqlParameter("@HLMH_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.HLMH_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@Type",
                   SqlDbType.VarChar)
                    {
                        Value = data.Type
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRMRM_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.HRMRM_Id
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
                        data.housewise_studentList = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                //data.floor_list = (from a in _HostelContext.HR_Master_Floor_DMO
                //                   from b in _HostelContext.HR_Master_Room_DMO
                //                   where (a.HLMH_Id == data.HLMH_Id && a.HLMF_Id == b.HLMF_Id && a.MI_Id == data.MI_Id && a.HRMF_ActiveFlag == true)
                //                   select new HostelAllotForCLGStudentDTO
                //                   {
                //                       HLMF_Id = a.HLMF_Id,
                //                       HRMF_FloorName = a.HRMF_FloorName,
                //                   }).Distinct().OrderBy(a => a.HRMF_FloorName).ToArray();



                data.room_list = (from a in _HostelContext.HR_Master_Room_DMO
                                  where (/*a.HLMF_Id == data.HLMF_Id && a.HLMRCA_Id == data.HLMRCA_Id &&*/ a.MI_Id == data.MI_Id && a.HRMRM_ActiveFlag == true)
                                  select new HostelAllotForCLGStudentDTO
                                  {
                                      HRMRM_Id = a.HRMRM_Id,
                                      HLMF_Id = a.HLMF_Id,
                                      HRMRM_RoomNo = a.HRMRM_RoomNo,
                                  }).Distinct().OrderBy(a => a.HRMRM_RoomNo).ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public HostelAllotForCLGStudentDTO floor(HostelAllotForCLGStudentDTO data)
        {
            try
            {
                //data.room_list = (from a in _HostelContext.HR_Master_Room_DMO
                //                  where (a.HLMF_Id == data.HLMF_Id && a.HLMRCA_Id == data.HLMRCA_Id && a.MI_Id == data.MI_Id && a.HRMRM_ActiveFlag == true)
                //                  select new HostelAllotForCLGStudentDTO
                //                  {
                //                      HRMRM_Id = a.HRMRM_Id,
                //                      HRMRM_RoomNo = a.HRMRM_RoomNo,
                //                  }).Distinct().OrderBy(a => a.HRMRM_RoomNo).ToArray();

                var floor_list = (from a in _HostelContext.HR_Master_Floor_DMO
                                  from b in _HostelContext.HR_Master_Room_DMO
                                  where (a.HLMH_Id == data.HLMH_Id && a.HLMF_Id == b.HLMF_Id && a.MI_Id == data.MI_Id && a.HRMF_ActiveFlag == true)
                                  select new HostelAllotForCLGStudentDTO
                                  {
                                      HLMF_Id = a.HLMF_Id,
                                      HRMF_FloorName = a.HRMF_FloorName,
                                  }).Distinct().ToList();
                data.floor_list = floor_list.ToArray();

                data.roomcatgry_list = _HostelContext.HL_Master_Room_Category_DMO.Where(a => a.MI_Id == data.MI_Id && a.HLMH_Id == data.HLMH_Id && a.HLMRCA_ActiveFlag == true).Distinct().ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public HostelAllotForCLGStudentDTO room(HostelAllotForCLGStudentDTO data)
        {
            try
            {
                data.room_list = (from a in _HostelContext.HR_Master_Room_DMO
                                  where (a.HLMF_Id == data.HLMF_Id && a.HLMRCA_Id == data.HLMRCA_Id && a.MI_Id == data.MI_Id && a.HRMRM_ActiveFlag == true)
                                  select new HostelAllotForCLGStudentDTO
                                  {
                                      HRMRM_Id = a.HRMRM_Id,
                                      HRMRM_RoomNo = a.HRMRM_RoomNo,
                                  }).Distinct().OrderBy(a => a.HRMRM_RoomNo).ToArray();

                //&& a.HLMRCA_Id == data.HLMRCA_Id

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public HostelAllotForCLGStudentDTO roomForVacateReport(HostelAllotForCLGStudentDTO data)
        {
            try
            {
                if(data.HLMRCA_Id > 0)
                {
                    data.room_list = (from a in _HostelContext.HR_Master_Room_DMO
                                      where (a.HLMF_Id == data.HLMF_Id && a.HLMRCA_Id == data.HLMRCA_Id && a.HRMRM_Id != data.HRMRM_Id_Old && a.MI_Id == data.MI_Id && a.HRMRM_ActiveFlag == true)
                                      select new HostelAllotForCLGStudentDTO
                                      {
                                          HRMRM_Id = a.HRMRM_Id,
                                          HRMRM_RoomNo = a.HRMRM_RoomNo,
                                      }).Distinct().OrderBy(a => a.HRMRM_RoomNo).ToArray();
                }
                else
                {
                    data.room_list = _HostelContext.HR_Master_Room_DMO.Where(t => t.MI_Id == data.MI_Id && t.HLMF_Id == data.HLMF_Id && t.HRMRM_ActiveFlag == true).Distinct().OrderByDescending(t => t.HRMRM_Id).ToArray();
                }


                //&& a.HLMRCA_Id == data.HLMRCA_Id

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public HostelAllotForCLGStudentDTO roomdetails(HostelAllotForCLGStudentDTO data)
        {
            try
            {
                //data.room_Details = (from a in _HostelContext.HR_Master_Room_DMO
                //                  where (a.HLMF_Id == data.HLMF_Id && a.HLMRCA_Id==data.HLMRCA_Id && a.MI_Id == data.MI_Id && a.HRMRM_ActiveFlag == true)
                //                  select new HostelAllotForCLGStudentDTO
                //                  {
                //                      HRMRM_Id = a.HRMRM_Id,
                //                      HRMRM_RoomNo = a.HRMRM_RoomNo,
                //                  }).Distinct().OrderBy(a => a.HRMRM_RoomNo).ToArray();

                using (var cmd = _HostelContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Hostel_Room_Details";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@HLMH_Id", SqlDbType.BigInt)
                    {
                        Value = data.HLMH_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@HLMF_Id", SqlDbType.BigInt)
                    {
                        Value = data.HLMF_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@HLMRCA_Id", SqlDbType.BigInt)
                    {
                        Value = data.HLMRCA_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.BigInt)
                    {
                        Value = data.ASMAY_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRMRM_Id", SqlDbType.BigInt)
                    {
                        Value = data.HRMRM_Id
                    });
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
                                    dataRow.Add(
                                        dataReader.GetName(iFiled),
                                        dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.room_Details = retObject.ToArray();
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
        public HostelAllotForCLGStudentDTO get_roomdetails(HostelAllotForCLGStudentDTO data)
        {
            try
            {
                data.HRMRM_BedCapacity = _HostelContext.HR_Master_Room_DMO.Where(t => t.MI_Id == data.MI_Id && t.HRMRM_Id == data.HRMRM_Id).Single().HRMRM_BedCapacity;
                long cc = 0;
                var bcount = _HostelContext.HL_Hostel_Student_Allot_College_DMO.Where(t => t.MI_Id == data.MI_Id && t.HRMRM_Id == data.HRMRM_Id && t.HLHSALTC_ActiveFlag == true).Count();
                var BedCapacity = _HostelContext.HR_Master_Room_DMO.Where(t => t.MI_Id == data.MI_Id && t.HRMRM_Id == data.HRMRM_Id).ToArray();

                cc = BedCapacity[0].HRMRM_BedCapacity;
                if (bcount >= cc)
                {
                    data.bedcount = true;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public async Task<HostelAllotForCLGStudentDTO> editdata(HostelAllotForCLGStudentDTO data)
        {
            try
            {
                var edit = _HostelContext.HL_Hostel_Student_Allot_College_DMO.Where(a => a.HLHSALTC_Id == data.HLHSALTC_Id).ToList();

                data.editlist = edit.ToArray();


                data.floor_details = (from b in _HostelContext.HR_Master_Room_DMO
                                      from c in _HostelContext.HR_Master_Floor_DMO
                                      where (b.HRMRM_Id == edit.FirstOrDefault().HRMRM_Id && c.HLMF_Id == b.HLMF_Id)
                                      select new HostelAllotForCLGStudentDTO
                                      {
                                          HLMF_Id = b.HLMF_Id,
                                          HRMRM_Id = b.HRMRM_Id,
                                          HRMF_FloorName = c.HRMF_FloorName,
                                          HRMRM_RoomNo = b.HRMRM_RoomNo
                                      }).Distinct().ToArray();




                data.floor_list = (from a in _HostelContext.HR_Master_Floor_DMO
                                   where (a.HLMH_Id == edit.FirstOrDefault().HLMH_Id && a.MI_Id == data.MI_Id && a.HRMF_ActiveFlag == true)
                                   select new HostelAllotForCLGStudentDTO
                                   {
                                       HLMF_Id = a.HLMF_Id,
                                       HRMF_FloorName = a.HRMF_FloorName,
                                   }).Distinct().OrderBy(a => a.HRMF_FloorName).ToArray();
                //var floor_list = (from a in _HostelContext.HR_Master_Floor_DMO
                //                  where (a.HLMH_Id == edit.FirstOrDefault().HLMH_Id && a.MI_Id == data.MI_Id && a.HRMF_ActiveFlag == true)
                //                  select new HostelAllotForCLGStudentDTO
                //                  {
                //                      HLMF_Id = a.HLMF_Id,
                //                      HRMF_FloorName = a.HRMF_FloorName,
                //                  }).Distinct().OrderBy(a => a.HRMF_FloorName).ToList();
                //if(floor_list.Count > 0)
                //{
                //    List<long> HLMF_Id = new List<long>();
                //    foreach(var i in floor_list)
                //    {
                //        HLMF_Id.Add(i.HLMF_Id);
                //    }
                //}

                data.room_list = (from a in _HostelContext.HR_Master_Room_DMO
                                  where (a.MI_Id == data.MI_Id && a.HRMRM_ActiveFlag == true)
                                  select new HostelAllotForCLGStudentDTO
                                  {
                                      HRMRM_Id = a.HRMRM_Id,
                                      HRMRM_RoomNo = a.HRMRM_RoomNo,
                                  }).Distinct().OrderBy(a => a.HRMF_FloorName).ToArray();

                using (var cmd = _HostelContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "CLG_HOSTEL_HOUSE_WISE_STUDENT_LIST";
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
                    cmd.Parameters.Add(new SqlParameter("@HLMH_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.HLMH_Id
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
                        data.housewise_studentList = retObject.ToArray();
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

        public HostelAllotForCLGStudentDTO requestApproved(HostelAllotForCLGStudentDTO data)
        {
            try
            {
                long Emp_Code = _dbcontext.Staff_User_Login.Where(t => t.Id == data.UserId).Distinct().SingleOrDefault().Emp_Code;
                if (Emp_Code > 0)
                {
                    if (data.allottype == "Request")
                    {
                        HL_Hostel_Student_Request_College_Confirm_DMO updateCofirm = new HL_Hostel_Student_Request_College_Confirm_DMO();
                        updateCofirm.HLHSREQC_Id = data.HLHSREQC_Id;
                        updateCofirm.HLHSREQC_Date = data.HLHSREQC_RequestDate;
                        updateCofirm.HRME_Id = Emp_Code;
                        updateCofirm.HLMH_Id = data.HLMH_Id;
                        updateCofirm.HLMRCA_Id = data.HLMRCA_Id;
                        updateCofirm.HLHSREQCC_ACRoomFlg = data.HLHSREQC_ACRoomReqdFlg;
                        updateCofirm.HLHSREQCC_SingleRoomFlg = data.HLHSREQCC_SingleRoomFlg;
                        updateCofirm.HLHSREQCC_VegMessFlg = data.HLHSREQC_VegMessReqdFlg;
                        updateCofirm.HLHSREQCC_NonVegMessFlg = data.HLHSREQC_NonVegMessReqdFlg;
                        updateCofirm.HLHSREQCC_Remarks = data.HLHSREQCC_Remarks;
                        updateCofirm.HLHSREQCC_BookingStatus = "Approved";
                        updateCofirm.HLHSREQCC_ActiveFlag = true;
                        updateCofirm.HLHSREQCC_CreatedDate = DateTime.Now;
                        updateCofirm.HLHSREQCC_UpdatedDate = DateTime.Now;
                        updateCofirm.HLHSREQCC_CreatedBy = data.UserId;
                        updateCofirm.HLHSREQCC_UpdatedBy = data.UserId;
                        updateCofirm.HRMRM_Id = data.HRMRM_Id;
                        _HostelContext.Add(updateCofirm);
                        if (data.HLHSREQC_Id > 0)
                        {
                            var requestdata = _HostelContext.HL_Hostel_Student_Request_College_DMO.Where(t => t.HLHSREQC_Id == data.HLHSREQC_Id && t.MI_Id == data.MI_Id).Distinct().ToList();
                            if (requestdata[0].HLHSREQC_Id > 0)
                            {
                                var update = _HostelContext.HL_Hostel_Student_Request_College_DMO.Where(t => t.MI_Id == data.MI_Id && t.HLHSREQC_Id == data.HLHSREQC_Id).SingleOrDefault();

                                update.HLHSREQC_BookingStatus = "Approved";
                                update.HLHSREQC_UpdatedDate = DateTime.Now;
                                update.HLHSREQC_UpdatedBy = data.UserId;

                                _HostelContext.Update(update);

                            }
                        }
                        int row = _HostelContext.SaveChanges();
                        if (row > 0)
                        {
                            data.returnval = true;
                        }
                        else
                        {
                            data.returnval = true;
                        }
                    }

                    else if (data.allottype == "Manual")
                    {
                        long HLHSREQCIds = 0;

                        HL_Hostel_Student_Request_College_DMO reqCofirm1 = new HL_Hostel_Student_Request_College_DMO();
                        reqCofirm1.MI_Id = data.MI_Id;
                        reqCofirm1.HLHSREQC_RequestDate = DateTime.Now;
                        reqCofirm1.HLMH_Id = data.HLMH_Id;
                        reqCofirm1.HLMRCA_Id = data.HLMRCA_Id;
                        reqCofirm1.AMCST_Id = data.AMCST_Id;
                        reqCofirm1.HLHSREQC_ACRoomReqdFlg = data.HLHSREQC_ACRoomReqdFlg;
                        reqCofirm1.HLHSREQC_EntireRoomReqdFlg = data.HLHSREQC_EntireRoomReqdFlg;
                        reqCofirm1.HLHSREQC_VegMessReqdFlg = data.HLHSREQC_VegMessReqdFlg;
                        reqCofirm1.HLHSREQC_NonVegMessReqdFlg = data.HLHSREQC_NonVegMessReqdFlg;
                        reqCofirm1.HLHSREQC_Remarks = data.HLHSREQC_Remarks;
                        reqCofirm1.HLHSREQC_BookingStatus = "Approved";
                        reqCofirm1.HLHSREQC_ActiveFlag = true;
                        reqCofirm1.HLHSREQC_CreatedDate = DateTime.Now;
                        reqCofirm1.HLHSREQC_UpdatedDate = DateTime.Now;
                        reqCofirm1.HLHSREQC_CreatedBy = data.UserId;
                        reqCofirm1.HLHSREQC_UpdatedBy = data.UserId;
                        reqCofirm1.HRMRM_Id = data.HRMRM_Id;
                        _HostelContext.Add(reqCofirm1);
                        _HostelContext.SaveChanges();
                        HLHSREQCIds = reqCofirm1.HLHSREQC_Id;


                        HL_Hostel_Student_Request_College_Confirm_DMO updateCofirm1 = new HL_Hostel_Student_Request_College_Confirm_DMO();
                        updateCofirm1.HLHSREQC_Id = HLHSREQCIds;
                        updateCofirm1.HLHSREQC_Date = DateTime.Now;
                        updateCofirm1.HRME_Id = Emp_Code;
                        updateCofirm1.HLMH_Id = data.HLMH_Id;
                        updateCofirm1.HLMRCA_Id = data.HLMRCA_Id;
                        updateCofirm1.HLHSREQCC_ACRoomFlg = data.HLHSREQC_ACRoomReqdFlg;
                        updateCofirm1.HLHSREQCC_SingleRoomFlg = data.HLHSREQCC_SingleRoomFlg;
                        updateCofirm1.HLHSREQCC_VegMessFlg = data.HLHSREQC_VegMessReqdFlg;
                        updateCofirm1.HLHSREQCC_NonVegMessFlg = data.HLHSREQC_NonVegMessReqdFlg;
                        updateCofirm1.HLHSREQCC_Remarks = data.HLHSREQCC_Remarks;
                        updateCofirm1.HLHSREQCC_BookingStatus = "Approved";
                        updateCofirm1.HLHSREQCC_ActiveFlag = true;
                        updateCofirm1.HLHSREQCC_CreatedDate = DateTime.Now;
                        updateCofirm1.HLHSREQCC_UpdatedDate = DateTime.Now;
                        updateCofirm1.HLHSREQCC_CreatedBy = data.UserId;
                        updateCofirm1.HLHSREQCC_UpdatedBy = data.UserId;
                        updateCofirm1.HRMRM_Id = data.HRMRM_Id;
                        _HostelContext.Add(updateCofirm1);
                        int row = _HostelContext.SaveChanges();
                        if (row > 0)
                        {
                            data.returnval = true;
                        }
                        else
                        {
                            data.returnval = true;
                        }
                    }

                }
                else
                {

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public HostelAllotForCLGStudentDTO requestRejected(HostelAllotForCLGStudentDTO data)
        {
            try
            {


                var Emp_Code = _dbcontext.Staff_User_Login.Where(t => t.Id == data.UserId).Distinct().SingleOrDefault().Emp_Code;

                var updateCofirm = _HostelContext.HL_Hostel_Student_Request_College_Confirm_DMO.Where(t => t.HLHSREQC_Id == data.HLHSREQC_Id && t.HLHSREQC_Id == data.HLHSREQC_Id).Single();

                //updateCofirm.HLHSREQCC_Id = data.HLHSREQCC_Id;
                updateCofirm.HLHSREQC_Id = data.HLHSREQC_Id;
                updateCofirm.HLHSREQC_Date = data.HLHSREQC_RequestDate;
                updateCofirm.HRME_Id = Emp_Code;
                updateCofirm.HLMH_Id = data.HLMH_Id;
                updateCofirm.HLMRCA_Id = data.HLMRCA_Id;
                updateCofirm.HLHSREQCC_ACRoomFlg = data.HLHSREQC_ACRoomReqdFlg;
                updateCofirm.HLHSREQCC_SingleRoomFlg = data.HLHSREQC_EntireRoomReqdFlg;
                updateCofirm.HLHSREQCC_VegMessFlg = data.HLHSREQC_VegMessReqdFlg;
                updateCofirm.HLHSREQCC_NonVegMessFlg = data.HLHSREQC_NonVegMessReqdFlg;
                updateCofirm.HLHSREQCC_Remarks = data.HLHSALTC_AllotRemarks;
                updateCofirm.HLHSREQCC_BookingStatus = "Rejected";
                updateCofirm.HLHSREQCC_CreatedDate = DateTime.Now;
                updateCofirm.HLHSREQCC_UpdatedDate = DateTime.Now;
                updateCofirm.HLHSREQCC_CreatedBy = data.UserId;
                updateCofirm.HLHSREQCC_UpdatedBy = data.UserId;

                _HostelContext.Add(updateCofirm);
                if (data.HLHSREQC_Id > 0)
                {
                    var requestdata = _HostelContext.HL_Hostel_Student_Request_College_DMO.Where(t => t.HLHSREQC_Id == data.HLHSREQC_Id && t.MI_Id == data.MI_Id).Distinct().ToList();
                    if (requestdata[0].HLHSREQC_Id != 0)
                    {
                        var update = _HostelContext.HL_Hostel_Student_Request_College_DMO.Where(t => t.MI_Id == data.MI_Id && t.HLHSREQC_Id == data.HLHSREQC_Id).SingleOrDefault();

                        update.HLHSREQC_BookingStatus = "Rejected";
                        update.HLHSREQC_UpdatedDate = DateTime.Now;
                        update.HLHSREQC_UpdatedBy = data.UserId;

                        _HostelContext.Update(update);

                    }
                }

                int row = _HostelContext.SaveChanges();
                if (row > 0)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = true;
                }

            }



            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public HL_Hostel_Student_Transfer_CollegeDTO HostelT(HL_Hostel_Student_Transfer_CollegeDTO data)
        {
            try
            {
                if (data.HLHSTRSC_Id > 0)
                {
                    var duplicate = _HostelContext.HL_Hostel_Student_Transfer_CollegeDMO.Where(T => T.ASMAY_Id == data.ASMAY_Id && T.AMCST_Id == data.ACMST_Id && T.HLHSTRSC_Id != data.HLHSTRSC_Id).ToList();
                    if (duplicate.Count > 0)
                    {
                        data.returnval = "duplicate";
                    }
                    else
                    {


                        var obj = _HostelContext.HL_Hostel_Student_Transfer_CollegeDMO.Where(R => R.HLHSTRSC_Id == data.HLHSTRSC_Id).FirstOrDefault();
                        obj.MI_Id = data.MI_Id;
                        obj.HLHSALTC_TransferDate = data.HLHSALTC_TransferDate;
                        obj.ASMAY_Id = data.ASMAY_Id;
                        obj.HLMH_Id = data.HLMH_Id;
                        obj.HLMRCA_Id = data.HLMRCA_Id;
                        obj.AMCST_Id = data.ACMST_Id;
                        obj.HRMRM_Id = data.HRMRM_Id;
                        obj.HLHSTRSC_RoomFee = data.HLHSTRSC_RoomFee;
                        obj.HLHSTRSC_To_HLMRCA_Id = data.HLHSTRSC_To_HLMRCA_Id;
                        obj.HLMRCAC_To_HRMRM_Id = data.HLMRCAC_To_HRMRM_Id;
                        obj.HLHSTRSC_EntireRoomReqdFlg = data.HLHSTRSC_EntireRoomReqdFlg;
                        obj.HLHSTRSC_NewRoomFee = data.HLHSTRSC_NewRoomFee;
                        obj.HLHSTRSC_AllotRemarks = data.HLHSTRSC_AllotRemarks;
                        obj.HLHSTRSC_VacateRemarks = data.HLHSTRSC_VacateRemarks;
                        obj.HLHSTRSC_ActiveFlag = true;
                        obj.HLHSTRSC_CreatedDate = DateTime.Now;
                        obj.HLHSTRSC_UpdatedDate = DateTime.Now;
                        obj.HLHSTRSC_UpdatedBy = data.UserId;
                        obj.HLHSTRSC_CreatedBy = data.UserId;
                        _HostelContext.Update(obj);
                        int i = _HostelContext.SaveChanges();
                        if (i > 0)
                        {
                            data.returnval = "save";
                        }
                        else
                        {
                            data.returnval = "notsave";
                        }
                    }
                }
                else
                {
                    var duplicate = _HostelContext.HL_Hostel_Student_Transfer_CollegeDMO.Where(T => T.ASMAY_Id == data.ASMAY_Id && T.AMCST_Id == data.ACMST_Id).ToList();
                    if (duplicate.Count > 0)
                    {
                        data.returnval = "duplicate";
                    }
                    else
                    {
                        HL_Hostel_Student_Transfer_CollegeDMO obj = new HL_Hostel_Student_Transfer_CollegeDMO();
                        obj.MI_Id = data.MI_Id;
                        obj.HLHSALTC_TransferDate = data.HLHSALTC_TransferDate;
                        obj.ASMAY_Id = data.ASMAY_Id;
                        obj.HLMH_Id = data.HLMH_Id;
                        obj.HLMRCA_Id = data.HLMRCA_Id;
                        obj.AMCST_Id = data.ACMST_Id;
                        obj.HRMRM_Id = data.HRMRM_Id;
                        obj.HLHSTRSC_RoomFee = data.HLHSTRSC_RoomFee;
                        obj.HLHSTRSC_To_HLMRCA_Id = data.HLHSTRSC_To_HLMRCA_Id;
                        obj.HLMRCAC_To_HRMRM_Id = data.HLMRCAC_To_HRMRM_Id;
                        obj.HLHSTRSC_EntireRoomReqdFlg = data.HLHSTRSC_EntireRoomReqdFlg;
                        obj.HLHSTRSC_NewRoomFee = data.HLHSTRSC_NewRoomFee;
                        obj.HLHSTRSC_AllotRemarks = data.HLHSTRSC_AllotRemarks;
                        obj.HLHSTRSC_VacateRemarks = data.HLHSTRSC_VacateRemarks;
                        obj.HLHSTRSC_ActiveFlag = true;
                        obj.HLHSTRSC_CreatedDate = DateTime.Now;
                        obj.HLHSTRSC_UpdatedDate = DateTime.Now;
                        obj.HLHSTRSC_UpdatedBy = data.UserId;
                        obj.HLHSTRSC_CreatedBy = data.UserId;
                        _HostelContext.Add(obj);
                        int i = _HostelContext.SaveChanges();
                        if (i > 0)
                        {
                            var update = _HostelContext.HL_Hostel_Student_Allot_College_DMO.Where(R => R.ASMAY_Id == data.ASMAY_Id && R.AMCST_Id == data.ACMST_Id).ToList();
                            if (update.Count == 1)
                            {
                                update.FirstOrDefault().HLMRCA_Id = data.HLHSTRSC_To_HLMRCA_Id;
                                update.FirstOrDefault().HRMRM_Id = data.HLMRCAC_To_HRMRM_Id;
                                update.FirstOrDefault().HLHSALTC_UpdatedBy = data.UserId;
                                update.FirstOrDefault().HLHSALTC_UpdatedDate = DateTime.Now;

                                _HostelContext.Update(obj);
                                _HostelContext.SaveChanges();
                            }


                            data.returnval = "save";
                        }
                        else
                        {
                            data.returnval = "notsave";
                        }
                    }

                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                data.returnval = "admin";
            }
            return data;
        }

        public HostelAllotForCLGStudentDTO get_course(HostelAllotForCLGStudentDTO data)
        {
            try
            {
                data.courseLists = (from a in _dbcontext.Adm_College_Yearly_StudentDMO
                                  from b in _dbcontext.CLGADM_master_courseDMO
                                  where (a.AMCO_Id == b.AMCO_Id && b.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.ACYST_ActiveFlag == 1 && b.AMCO_ActiveFlag == true)
                                  select new HostelAllotForCLGStudentDTO
                                  {
                                      AMCO_Id = b.AMCO_Id,
                                      AMCO_CourseName = b.AMCO_CourseName
                                  }).Distinct().OrderBy(t => t.AMCO_Id).ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;

        }
        public HostelAllotForCLGStudentDTO get_branch(HostelAllotForCLGStudentDTO data)
        {
            try
            {
                data.branchLists = (from a in _dbcontext.Adm_College_Yearly_StudentDMO
                                    from b in _dbcontext.CLGADM_master_courseDMO
                                    from c in _dbcontext.ClgMasterBranchDMO
                                    where (a.AMB_Id==c.AMB_Id && a.AMCO_Id == data.AMCO_Id && b.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.ACYST_ActiveFlag == 1 && b.AMCO_ActiveFlag == true)
                                    select new HostelAllotForCLGStudentDTO
                                    {
                                        AMB_Id = c.AMB_Id,
                                        AMB_BranchName = c.AMB_BranchName
                                    }).Distinct().OrderBy(t => t.AMB_Id).ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;

        }
        public HostelAllotForCLGStudentDTO get_sem(HostelAllotForCLGStudentDTO data)
        {
            try
            {
                data.semLists = (from a in _dbcontext.Adm_College_Yearly_StudentDMO
                                 from b in _dbcontext.CLGADM_master_courseDMO
                                 from c in _dbcontext.ClgMasterBranchDMO
                                 from d in _dbcontext.CLG_Adm_Master_SemesterDMO
                                 where (a.AMSE_Id == d.AMSE_Id && a.AMB_Id == c.AMB_Id && a.AMCO_Id == data.AMCO_Id && a.AMB_Id == data.AMB_Id && b.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.ACYST_ActiveFlag == 1 && b.AMCO_ActiveFlag == true)
                                 select new HostelAllotForCLGStudentDTO
                                 {
                                     AMSE_Id = d.AMSE_Id,
                                     AMSE_SEMName = d.AMSE_SEMName
                                 }).Distinct().OrderBy(t => t.AMSE_Id).ToArray();

                if(data.semLists != null && data.semLists.Length > 0)
                {
                    data.secLists = (from a in _dbcontext.Adm_College_Yearly_StudentDMO
                                     from e in _dbcontext.Adm_College_Master_SectionDMO
                                     where (e.MI_Id == data.MI_Id &&
                                     a.ASMAY_Id == data.ASMAY_Id && a.ACYST_ActiveFlag == 1 && e.ACMS_ActiveFlag == true)
                                     select new HostelAllotForCLGStudentDTO
                                     {
                                         ACMS_Id = e.ACMS_Id,
                                         ACMS_SectionName = e.ACMS_SectionName
                                     }).Distinct().OrderBy(t => t.ACMS_Id).ToArray();
                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;

        }
        //public HostelAllotForCLGStudentDTO get_sec(HostelAllotForCLGStudentDTO data)
        //{
        //    try
        //    {
        //        data.secLists = (from a in _dbcontext.Adm_College_Yearly_StudentDMO
        //                         from b in _dbcontext.CLGADM_master_courseDMO
        //                         from c in _dbcontext.ClgMasterBranchDMO
        //                         from d in _dbcontext.CLG_Adm_Master_SemesterDMO
        //                         from e in _dbcontext.Adm_College_Master_SectionDMO
        //                         where (a.AMB_Id == c.AMB_Id && a.AMSE_Id == data.AMSE_Id && a.AMCO_Id == data.AMCO_Id && a.AMB_Id == data.AMB_Id && b.MI_Id == data.MI_Id &&
        //                         a.ASMAY_Id == data.ASMAY_Id && a.ACYST_ActiveFlag == 1 && b.AMCO_ActiveFlag == true && c.AMB_ActiveFlag == true && d.AMSE_ActiveFlg == true
        //                         && e.ACMS_ActiveFlag == true)
        //                         select new HostelAllotForCLGStudentDTO
        //                         {
        //                             ACMS_Id = e.ACMS_Id,
        //                             ACMS_SectionName = e.ACMS_SectionName
        //                         }).Distinct().OrderBy(t => t.ACMS_Id).ToArray();

        //    }
        //    catch (Exception ee)
        //    {
        //        Console.WriteLine(ee.Message);
        //    }
        //    return data;

        //}
        public HostelAllotForCLGStudentDTO get_student(HostelAllotForCLGStudentDTO data)
        {
            try
            {
                data.studlist = (from a in _dbcontext.Adm_College_Yearly_StudentDMO
                                 from b in _HostelContext.HL_Hostel_Student_Allot_College_DMO
                                 from c in _dbcontext.Adm_Master_College_StudentDMO
                                 where (c.MI_Id==data.MI_Id && a.ASMAY_Id==data.ASMAY_Id && a.AMCO_Id==data.AMCO_Id && a.AMB_Id==data.AMB_Id && a.AMSE_Id==data.AMSE_Id && a.ACMS_Id==data.ACMS_Id)
                                 select new HostelAllotForCLGStudentDTO
                                 {
                                     AMCST_Id = c.AMCST_Id,
                                     studentname = ((c.AMCST_FirstName == null ? " " : c.AMCST_FirstName) + " " + (c.AMCST_MiddleName == null ? " " : c.AMCST_MiddleName) + " " + (c.AMCST_LastName == null ? " " : c.AMCST_LastName)).Trim()
                                 }).Distinct().OrderBy(t => t.AMCST_Id).ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;

        }

    }
}
