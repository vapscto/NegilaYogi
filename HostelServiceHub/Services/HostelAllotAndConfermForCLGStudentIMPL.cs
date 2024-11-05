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
    public class HostelAllotAndConfermForCLGStudentIMPL : Interface.HostelAllotAndConfermForCLGStudentInterface
    {

        public HostelContext _HostelContext;
        public DomainModelMsSqlServerContext _dbcontext;
        public HostelAllotAndConfermForCLGStudentIMPL(HostelContext para1, DomainModelMsSqlServerContext para2)
        {
            _HostelContext = para1;
            _dbcontext = para2;
        }
        public async Task<CLGStudentRequestConfirmDTO> loaddata(CLGStudentRequestConfirmDTO data)
        {
            try
            {

                var yeardata = (from a in _dbcontext.AcademicYear
                                where (a.MI_Id == data.MI_Id && a.Is_Active == true)
                                select new CLGStudentRequestConfirmDTO { ASMAY_Id = a.ASMAY_Id, ASMAY_Year = a.ASMAY_Year }).Distinct().ToList();

                if (yeardata.Count > 0)
                {
                    data.yearlist = yeardata.ToArray();
                    data.ASMAY_Id = data.ASMAY_Id;
                }

                data.hostel_list = (from a in _HostelContext.HL_Master_Hostel_DMO
                                    from b in _HostelContext.HL_Hostel_Student_Request_College_Confirm_DMO
                                    where (a.MI_Id == data.MI_Id && b.HLMH_Id == a.HLMH_Id && a.HLMH_ActiveFlag == true && b.HLHSREQCC_ActiveFlag == true && b.HLHSREQCC_BookingStatus == "Approved")
                                    select new CLGStudentRequestConfirmDTO { HLMH_Id = a.HLMH_Id, HLMH_Name = a.HLMH_Name }).Distinct().ToArray();


                data.room_list = (from a in _HostelContext.HR_Master_Room_DMO
                                  where (a.MI_Id == data.MI_Id && a.HRMRM_ActiveFlag == true)
                                  select new CLGStudentRequestConfirmDTO
                                  {
                                      HRMRM_Id = a.HRMRM_Id,
                                      HRMRM_RoomNo = a.HRMRM_RoomNo
                                  }).Distinct().ToArray();

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

                using (var cmd = _HostelContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "CLG_HOSTEL_STUDENT_REQUEST_DETAILS";
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
                        data.student_RequestList = retObject.ToArray();
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
        public CLGStudentRequestConfirmDTO requestApproved(CLGStudentRequestConfirmDTO data)
        {
            try
            {
                var requestdata = _HostelContext.HL_Hostel_Student_Request_College_DMO.Where(t => t.HLHSREQC_Id == data.HLHSREQC_Id && t.MI_Id == data.MI_Id).Distinct().ToList();
                if (requestdata.Count > 0)
                {
                    long Emp_Code = _dbcontext.Staff_User_Login.Where(t => t.Id == data.UserId).Distinct().SingleOrDefault().Emp_Code;
                    if (Emp_Code > 0)
                    {

                        var updateCofirm = _HostelContext.HL_Hostel_Student_Request_College_Confirm_DMO.Where(t => t.HLHSREQC_Id == data.HLHSREQC_Id).Single();

                        //updateCofirm.HLHSREQCC_Id = data.HLHSREQCC_Id;
                        updateCofirm.HLHSREQC_Id = requestdata[0].HLHSREQC_Id;
                        updateCofirm.HLHSREQC_Date = requestdata[0].HLHSREQC_RequestDate;
                        updateCofirm.HRME_Id = Emp_Code;
                        updateCofirm.HLMH_Id = requestdata[0].HLMH_Id;
                        updateCofirm.HLMRCA_Id = requestdata[0].HLMRCA_Id;
                        //updateCofirm.AMST_Id = requestdata[0].AMST_Id;
                        //updateCofirm.HLHSREQCC_ACRoomFlg = requestdata[0].HLHSREQC_ACRoomFlg;
                        //updateCofirm.HLHSREQCC_SingleRoomFlg = requestdata[0].HLHSREQC_SingleRoomFlg;
                        //updateCofirm.HLHSREQCC_VegMessFlg = requestdata[0].HLHSREQC_VegMessFlg;
                        //updateCofirm.HLHSREQCC_NonVegMessFlg = requestdata[0].HLHSREQC_NonVegMessFlg;
                        updateCofirm.HLHSREQCC_Remarks = data.HLHSREQC_Remarks;
                        updateCofirm.HLHSREQCC_BookingStatus = "Approved";
                        updateCofirm.HLHSREQCC_CreatedDate = DateTime.Now;
                        updateCofirm.HLHSREQCC_UpdatedDate = DateTime.Now;
                        updateCofirm.HLHSREQCC_CreatedBy = data.UserId;
                        updateCofirm.HLHSREQCC_UpdatedBy = data.UserId;
                        updateCofirm.HRMRM_Id = data.HRMRM_Id;

                        _HostelContext.Update(updateCofirm);

                        if (requestdata[0].HLHSREQC_Id != 0)
                        {
                            var update = _HostelContext.HL_Hostel_Student_Request_College_DMO.Where(t => t.MI_Id == data.MI_Id && t.HLHSREQC_Id == data.HLHSREQC_Id).SingleOrDefault();

                            update.HLHSREQC_BookingStatus = "Approved";
                            update.HLHSREQC_UpdatedDate = DateTime.Now;
                            update.HLHSREQC_UpdatedBy = data.UserId;

                            _HostelContext.Update(update);

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
                    else
                    {

                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public CLGStudentRequestConfirmDTO requestRejected(CLGStudentRequestConfirmDTO data)
        {
            try
            {
                var requestdata = _HostelContext.HL_Hostel_Student_Request_College_DMO.Where(t => t.HLHSREQC_Id == data.HLHSREQC_Id && t.MI_Id == data.MI_Id).Distinct().ToList();

                if (requestdata.Count > 0)
                {
                    var Emp_Code = _dbcontext.Staff_User_Login.Where(t => t.Id == data.UserId).Distinct().SingleOrDefault().Emp_Code;

                    var updateCofirm = _HostelContext.HL_Hostel_Student_Request_College_Confirm_DMO.Where(t => t.HLHSREQC_Id == data.HLHSREQC_Id && t.HLHSREQC_Id == data.HLHSREQC_Id).Single();

                    //updateCofirm.HLHSREQCC_Id = data.HLHSREQCC_Id;
                    updateCofirm.HLHSREQC_Id = requestdata[0].HLHSREQC_Id;
                    updateCofirm.HLHSREQC_Date = requestdata[0].HLHSREQC_RequestDate;
                    updateCofirm.HRME_Id = Emp_Code;
                    updateCofirm.HLMH_Id = requestdata[0].HLMH_Id;
                    updateCofirm.HLMRCA_Id = requestdata[0].HLMRCA_Id;
                    //updateCofirm.HLHSREQCC_ACRoomFlg = requestdata[0].HLHSREQC_ACRoomFlg;
                    //updateCofirm.HLHSREQCC_SingleRoomFlg = requestdata[0].HLHSREQC_SingleRoomFlg;
                    //updateCofirm.HLHSREQCC_VegMessFlg = requestdata[0].HLHSREQC_VegMessFlg;
                    //updateCofirm.HLHSREQCC_NonVegMessFlg = requestdata[0].HLHSREQC_NonVegMessFlg;
                    updateCofirm.HLHSREQCC_Remarks = data.HLHSREQC_Remarks;
                    updateCofirm.HLHSREQCC_BookingStatus = "Rejected";
                    updateCofirm.HLHSREQCC_CreatedDate = DateTime.Now;
                    updateCofirm.HLHSREQCC_UpdatedDate = DateTime.Now;
                    updateCofirm.HLHSREQCC_CreatedBy = data.UserId;
                    updateCofirm.HLHSREQCC_UpdatedBy = data.UserId;

                    _HostelContext.Update(updateCofirm);

                    if (requestdata[0].HLHSREQC_Id != 0)
                    {
                        var update = _HostelContext.HL_Hostel_Student_Request_College_DMO.Where(t => t.MI_Id == data.MI_Id && t.HLHSREQC_Id == data.HLHSREQC_Id).SingleOrDefault();

                        update.HLHSREQC_BookingStatus = "Rejected";
                        update.HLHSREQC_UpdatedDate = DateTime.Now;
                        update.HLHSREQC_UpdatedBy = data.UserId;

                        _HostelContext.Update(update);
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

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public CLGStudentRequestConfirmDTO bedcapacity(CLGStudentRequestConfirmDTO data)
        {
            try
            {
                long cc = 0;
                data.roomdetails = _HostelContext.HR_Master_Room_DMO.Where(t => t.MI_Id == data.MI_Id && t.HRMRM_Id == data.HRMRM_Id && t.HRMRM_ActiveFlag == true).Distinct().ToArray();

                var bcount = _HostelContext.HL_Hostel_Student_Allot_College_DMO.Where(t => t.MI_Id == data.MI_Id && t.HRMRM_Id == data.HRMRM_Id && t.HLHSALTC_ActiveFlag == true).Count();
                var BedCapacity = _HostelContext.HR_Master_Room_DMO.Where(t => t.MI_Id == data.MI_Id && t.HRMRM_Id == data.HRMRM_Id).ToArray();

                cc = BedCapacity[0].HRMRM_BedCapacity;
                if (bcount >= cc)
                {
                    data.bedcount = true;
                }
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
            return data;
        }

        public CLGStudentRequestConfirmDTO Ydeactive(CLGStudentRequestConfirmDTO data)
        {
            try
            {
                //var result = _HostelContext.HL_Master_Room_Tariff_DMO.Single(t => t.HLMRTF_Id == data.HLMRTF_Id && t.MI_Id == data.MI_Id);

                //if (result.HLMRTF_ActiveFlag == true)
                //{
                //    result.HLMRTF_ActiveFlag = false;
                //}
                //else if (result.HLMRTF_ActiveFlag == false)
                //{
                //    result.HLMRTF_ActiveFlag = true;
                //}
                //result.HLMRTF_UpdatedDate = DateTime.Now;
                //result.HLMRTF_UpdatedBy = data.UserId;

                //_HostelContext.Update(result);
                //int rowAffected = _HostelContext.SaveChanges();

                //if (rowAffected > 0)
                //{
                //    data.returnval = true;
                //}
                //else
                //{
                //    data.returnval = false;
                //}
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public async Task<CLGStudentRequestConfirmDTO> get_studInfo(CLGStudentRequestConfirmDTO data)
        {
            try
            {
                using (var cmd = _HostelContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "CLG_HOSTEL_HOUSE_WISE_STUDENT_LIST_EDIT";
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

                data.floor_list = (from a in _HostelContext.HR_Master_Floor_DMO
                                   where (a.HLMH_Id == data.HLMH_Id && a.MI_Id == data.MI_Id && a.HRMF_ActiveFlag == true)
                                   select new CLGStudentRequestConfirmDTO
                                   {
                                       HLMF_Id = a.HLMF_Id,
                                       HRMF_FloorName = a.HRMF_FloorName,
                                   }).Distinct().OrderBy(a => a.HRMF_FloorName).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
    }
}

