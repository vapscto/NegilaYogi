using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.Hostel;
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
    public class StaffRequestConfirmImpl : Interface.StaffRequestConfirmInterface
    {

        public HostelContext _HostelContext;
        public DomainModelMsSqlServerContext _dbcontext;
        public StaffRequestConfirmImpl(HostelContext para1, DomainModelMsSqlServerContext para2)
        {
            _HostelContext = para1;
            _dbcontext = para2;
        }
        public async Task<StaffRequestConfirm_DTO> loaddata(StaffRequestConfirm_DTO data)
        {
            try
            {
                data.room_list = (from a in _HostelContext.HR_Master_Room_DMO
                                  where (a.MI_Id == data.MI_Id && a.HRMRM_ActiveFlag == true)
                                  select new StudentRequestConfirm_DTO
                                  {
                                      HRMRM_Id = a.HRMRM_Id,
                                      HRMRM_RoomNo = a.HRMRM_RoomNo
                                  }).Distinct().ToArray();
                using (var cmd = _HostelContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "HOSTEL_STAFF_REQUEST_DETAILS";
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
                        data.staff_RequestList = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                using (var cmd = _HostelContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "HOSTEL_STAFF_REQUEST_APPROVAL_DETAILS";
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
                        data.staff_ApprovalList = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                data.roletype = (from a in _dbcontext.appUserRole
                                 from b in _dbcontext.MasterRoleType
                                 where (a.RoleTypeId == b.IVRMRT_Id && a.UserId==data.UserId)
                                 select b).Select(t => t.IVRMRT_RoleFlag).FirstOrDefault();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public StaffRequestConfirm_DTO requestApproved(StaffRequestConfirm_DTO data)
        {
            try
            {
                var requestdata = _HostelContext.HL_Hostel_Staff_Request_DMO.Where(t => t.HLHSTREQ_Id == data.HLHSTREQ_Id && t.MI_Id == data.MI_Id).Distinct().ToList();

                if (requestdata.Count > 0)
                {
                    var Emp_Code = _dbcontext.Staff_User_Login.Where(t => t.Id == data.UserId).Distinct().Single().Emp_Code;
                    if (Emp_Code > 0)
                    {
                        var updateCofirm = _HostelContext.HL_Hostel_Staff_Request_Confirm_DMO.Where(t => t.HLHSTREQC_Id == data.HLHSTREQC_Id && t.HLHSTREQ_Id == data.HLHSTREQ_Id).Single();

                        updateCofirm.HLHSTREQC_Id = data.HLHSTREQC_Id;
                        updateCofirm.HLHSTREQ_Id = requestdata[0].HLHSTREQ_Id;
                        updateCofirm.HLHSTREQC_RequestDate = DateTime.Now;
                        updateCofirm.HRME_Id = Emp_Code;
                        updateCofirm.HLMH_Id = requestdata[0].HLMH_Id;
                        updateCofirm.HLMRCA_Id = requestdata[0].HLMRCA_Id;
                        //updateCofirm.HLHSTREQC_HRMEId = Emp_Code;
                        updateCofirm.HLHSTREQC_ACRoomFlg = requestdata[0].HLHSTREQ_ACRoomReqdFlg;
                        updateCofirm.HLHSTREQC_SingleRoomFlg = requestdata[0].HLHSTREQ_EntireRoomReqdFlg;
                        updateCofirm.HLHSTREQC_VegMessFlg = requestdata[0].HLHSTREQ_VegMessReqdFlg;
                        updateCofirm.HLHSTREQC_NonVegMessFlg = requestdata[0].HLHSTREQ_NonVegMessReqdFlg;
                        updateCofirm.HLHSTREQC_Remarks = data.HLHSTREQC_Remarks;
                        updateCofirm.HLHSTREQC_BookingStatus = "Approved";
                        updateCofirm.HLHSTREQC_UpdatedDate = DateTime.Now;
                        updateCofirm.HLHSTREQC_UpdatedBy = data.UserId;
                        updateCofirm.HRMRM_Id = data.HRMRM_Id;

                        _HostelContext.Update(updateCofirm);

                        if (requestdata[0].HLHSTREQ_Id != 0)
                        {
                            var update = _HostelContext.HL_Hostel_Staff_Request_DMO.Where(t => t.MI_Id == data.MI_Id && t.HLHSTREQ_Id == data.HLHSTREQ_Id).SingleOrDefault();

                            update.HLHSTREQ_BookingStatus = "Approved";
                            update.HLHSTREQ_UpdatedDate = DateTime.Now;
                            update.HLHSTREQ_UpdatedBy = data.UserId;

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
        public StaffRequestConfirm_DTO requestRejected(StaffRequestConfirm_DTO data)
        {
            try
            {
                var requestdata = _HostelContext.HL_Hostel_Staff_Request_DMO.Where(t => t.HLHSTREQ_Id == data.HLHSTREQ_Id && t.MI_Id == data.MI_Id).Distinct().ToList();

                if (requestdata.Count > 0)
                {
                    var Emp_Code = _dbcontext.Staff_User_Login.Where(t => t.Id == data.UserId).Distinct().Single().Emp_Code;

                    var updateCofirm = _HostelContext.HL_Hostel_Staff_Request_Confirm_DMO.Where(t => t.HLHSTREQC_Id == data.HLHSTREQC_Id && t.HLHSTREQ_Id == data.HLHSTREQ_Id).Single();

                    updateCofirm.HLHSTREQC_Id = data.HLHSTREQC_Id;
                    updateCofirm.HLHSTREQ_Id = requestdata[0].HLHSTREQ_Id;
                    updateCofirm.HLHSTREQC_RequestDate = DateTime.Now;
                    updateCofirm.HRME_Id = Emp_Code;
                    updateCofirm.HLMH_Id = requestdata[0].HLMH_Id;
                    updateCofirm.HLMRCA_Id = requestdata[0].HLMRCA_Id;
                    //updateCofirm.HLHSTREQC_HRMEId = Emp_Code;
                    updateCofirm.HLHSTREQC_ACRoomFlg = requestdata[0].HLHSTREQ_ACRoomReqdFlg;
                    updateCofirm.HLHSTREQC_SingleRoomFlg = requestdata[0].HLHSTREQ_EntireRoomReqdFlg;
                    updateCofirm.HLHSTREQC_VegMessFlg = requestdata[0].HLHSTREQ_VegMessReqdFlg;
                    updateCofirm.HLHSTREQC_NonVegMessFlg = requestdata[0].HLHSTREQ_NonVegMessReqdFlg;
                    updateCofirm.HLHSTREQC_Remarks = data.HLHSTREQC_Remarks;
                    updateCofirm.HLHSTREQC_BookingStatus = "Rejected";
                    updateCofirm.HLHSTREQC_UpdatedDate = DateTime.Now;
                    updateCofirm.HLHSTREQC_UpdatedBy = data.UserId;

                    _HostelContext.Update(updateCofirm);

                    if (requestdata[0].HLHSTREQ_Id != 0)
                    {
                        var update = _HostelContext.HL_Hostel_Staff_Request_DMO.Where(t => t.MI_Id == data.MI_Id && t.HLHSTREQ_Id == data.HLHSTREQ_Id).SingleOrDefault();

                        update.HLHSTREQ_BookingStatus = "Rejected";
                        update.HLHSTREQ_UpdatedDate = DateTime.Now;
                        update.HLHSTREQ_UpdatedBy = data.UserId;

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

    }
}
