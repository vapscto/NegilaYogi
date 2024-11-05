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
    public class StudentRequestConfirmImpl : Interface.StudentRequestConfirmInterface
    {

        public HostelContext _HostelContext;
        public DomainModelMsSqlServerContext _dbcontext;
        public StudentRequestConfirmImpl(HostelContext para1, DomainModelMsSqlServerContext para2)
        {
            _HostelContext = para1;
            _dbcontext = para2;
        }
        public async Task<StudentRequestConfirm_DTO> loaddata(StudentRequestConfirm_DTO data)
        {
            try
            {
                data.room_list = (from a in _HostelContext.HR_Master_Room_DMO
                                  where (a.MI_Id == data.MI_Id && a.HRMRM_ActiveFlag == true)
                                  select new StudentRequestConfirm_DTO
                                  {
                                      HRMRM_Id=a.HRMRM_Id,
                                      HRMRM_RoomNo =a.HRMRM_RoomNo
                                  }).Distinct().ToArray();

                using (var cmd = _HostelContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "HOSTEL_STUDENT_REQUEST_DETAILS";
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

                using (var cmd = _HostelContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "STUDENT_REQUEST_APPROVAL_HOSTEL";
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
                        data.student_RequestApp = retObject.ToArray();
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
        public StudentRequestConfirm_DTO requestApproved(StudentRequestConfirm_DTO data)
        {
            try
            {
                var requestdata = _HostelContext.HL_Hostel_Student_Request_DMO.Where(t => t.HLHSREQ_Id == data.HLHSREQ_Id && t.MI_Id == data.MI_Id).Distinct().ToList();

                if (requestdata.Count > 0)
                {
                    long Emp_Code = _dbcontext.Staff_User_Login.Where(t => t.Id == data.UserId).Distinct().SingleOrDefault().Emp_Code;
                    if (Emp_Code > 0)
                    {
                        var updateCofirm = _HostelContext.HL_Hostel_Student_Request_Confirm_DMO.Where(t => t.HLHSREQC_Id == data.HLHSREQC_Id && t.HLHSREQ_Id == data.HLHSREQ_Id).Single();

                        updateCofirm.HLHSREQC_Id = data.HLHSREQC_Id;
                        updateCofirm.HLHSREQ_Id = requestdata[0].HLHSREQ_Id;
                        updateCofirm.HLHSREQC_Date = requestdata[0].HLHSREQ_RequestDate;
                        updateCofirm.HRME_Id = Emp_Code;
                        updateCofirm.HLMH_Id = requestdata[0].HLMH_Id;
                        updateCofirm.HLMRCA_Id = requestdata[0].HLMRCA_Id;
                        //updateCofirm.AMST_Id = requestdata[0].AMST_Id;
                        updateCofirm.HLHSREQC_ACRoomFlg = requestdata[0].HLHSREQ_ACRoomFlg;
                        updateCofirm.HLHSREQC_SingleRoomFlg = requestdata[0].HLHSREQ_SingleRoomFlg;
                        updateCofirm.HLHSREQC_VegMessFlg = requestdata[0].HLHSREQ_VegMessFlg;
                        updateCofirm.HLHSREQC_NonVegMessFlg = requestdata[0].HLHSREQ_NonVegMessFlg;
                        updateCofirm.HLHSREQC_Remarks = data.HLHSREQC_Remarks;
                        updateCofirm.HLHSREQC_BookingStatus = "Approved";
                        updateCofirm.HLHSREQC_CreatedDate = DateTime.Now;
                        updateCofirm.HLHSREQC_UpdatedDate = DateTime.Now;
                        updateCofirm.HLHSREQC_CreatedBy = data.UserId;
                        updateCofirm.HLHSREQC_UpdatedBy = data.UserId;
                        updateCofirm.HRMRM_Id = data.HRMRM_Id;

                        _HostelContext.Update(updateCofirm);

                        if (requestdata[0].HLHSREQ_Id != 0)
                        {
                            var update = _HostelContext.HL_Hostel_Student_Request_DMO.Where(t => t.MI_Id == data.MI_Id && t.HLHSREQ_Id == data.HLHSREQ_Id).SingleOrDefault();

                            update.HLHSREQ_BookingStatus = "Approved";
                            update.HLHSREQ_UpdatedDate = DateTime.Now;
                            update.HLHSREQ_UpdatedBy = data.UserId;

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
        public StudentRequestConfirm_DTO requestRejected(StudentRequestConfirm_DTO data)
        {
            try
            {
                var requestdata = _HostelContext.HL_Hostel_Student_Request_DMO.Where(t => t.HLHSREQ_Id == data.HLHSREQ_Id && t.MI_Id == data.MI_Id).Distinct().ToList();

                if (requestdata.Count > 0)
                {
                    var Emp_Code = _dbcontext.Staff_User_Login.Where(t => t.Id == data.UserId).Distinct().SingleOrDefault().Emp_Code;

                    var updateCofirm = _HostelContext.HL_Hostel_Student_Request_Confirm_DMO.Where(t => t.HLHSREQC_Id == data.HLHSREQC_Id && t.HLHSREQ_Id == data.HLHSREQ_Id).Single();

                    updateCofirm.HLHSREQC_Id = data.HLHSREQC_Id;
                    updateCofirm.HLHSREQ_Id = requestdata[0].HLHSREQ_Id;
                    updateCofirm.HLHSREQC_Date = requestdata[0].HLHSREQ_RequestDate;
                    updateCofirm.HRME_Id = Emp_Code;
                    updateCofirm.HLMH_Id = requestdata[0].HLMH_Id;
                    updateCofirm.HLMRCA_Id = requestdata[0].HLMRCA_Id;
                    //updateCofirm.AMST_Id = requestdata[0].AMST_Id;
                    updateCofirm.HLHSREQC_ACRoomFlg = requestdata[0].HLHSREQ_ACRoomFlg;
                    updateCofirm.HLHSREQC_SingleRoomFlg = requestdata[0].HLHSREQ_SingleRoomFlg;
                    updateCofirm.HLHSREQC_VegMessFlg = requestdata[0].HLHSREQ_VegMessFlg;
                    updateCofirm.HLHSREQC_NonVegMessFlg = requestdata[0].HLHSREQ_NonVegMessFlg;
                    updateCofirm.HLHSREQC_Remarks = data.HLHSREQC_Remarks;
                    updateCofirm.HLHSREQC_BookingStatus = "Rejected";
                    updateCofirm.HLHSREQC_CreatedDate = DateTime.Now;
                    updateCofirm.HLHSREQC_UpdatedDate = DateTime.Now;
                    updateCofirm.HLHSREQC_CreatedBy = data.UserId;
                    updateCofirm.HLHSREQC_UpdatedBy = data.UserId;

                    _HostelContext.Update(updateCofirm);

                    if (requestdata[0].HLHSREQ_Id != 0)
                    {
                        var update = _HostelContext.HL_Hostel_Student_Request_DMO.Where(t => t.MI_Id == data.MI_Id && t.HLHSREQ_Id == data.HLHSREQ_Id).SingleOrDefault();

                        update.HLHSREQ_BookingStatus = "Rejected";
                        update.HLHSREQ_UpdatedDate = DateTime.Now;
                        update.HLHSREQ_UpdatedBy = data.UserId;

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
        public StudentRequestConfirm_DTO Ydeactive(StudentRequestConfirm_DTO data)
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
    }
}
