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
    public class CLGStudentRequestImpl : Interface.CLGStudentRequestInterface
    {

        public HostelContext _hostelContext;
        public DomainModelMsSqlServerContext _dbContext;
        public CLGStudentRequestImpl(HostelContext context, DomainModelMsSqlServerContext context2)
        {
            _hostelContext = context;
            _dbContext = context2;
        }

        public async Task<CLGStudentRequest_DTO> loaddata(CLGStudentRequest_DTO data)
        {
            try
            {
                List<CLGStudentRequest_DTO> devicelist = new List<CLGStudentRequest_DTO>();
                //var ddata = new List();
                var ddata = new { };
                using (var cmd = _hostelContext.Database.GetDbConnection().CreateCommand())
                {

                    cmd.CommandText = "CLG_HOSTEL_SINGLE_STUDENT_DETAILS";
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
                    cmd.Parameters.Add(new SqlParameter("@AMCST_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.AMCST_Id
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
                                devicelist.Add(new CLGStudentRequest_DTO
                                {


                                    AMCST_Sex = Convert.ToString(dataReader["AMCST_Sex"])
                                });
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                CLGStudentRequest_DTO dto = new CLGStudentRequest_DTO();
                dto.devicelist12 = devicelist.ToList();

                using (var cmd = _hostelContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "CLG_HOSTEL_SINGLE_STUDENT_DETAILS";
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
                    cmd.Parameters.Add(new SqlParameter("@AMCST_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.AMCST_Id
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
                        data.student_wisedata = retObject.ToArray();

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }


                var gender = "";
                if (dto.devicelist12.Count > 0)
                {
                    foreach (var r in dto.devicelist12)
                    {
                        if (r.AMCST_Sex == "Male" || r.AMCST_Sex == "MALE")
                        {
                            gender = "Boys";
                        }
                        if (r.AMCST_Sex == "Female" || r.AMCST_Sex == "FEMALE")
                        {
                            gender = "Girls";
                        }
                    }
                }

                data.hostel_list = _hostelContext.HL_Master_Hostel_DMO.Where(t => t.MI_Id == data.MI_Id && t.HLMH_BoysGirlsFlg == gender && t.HLMH_ActiveFlag == true).Distinct().ToArray();


                if (data.student_wisedata.Length > 0)
                {
                    using (var cmd = _hostelContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "CLG_HOSTEL_STUDENT_DETAILS_FOR_GRID";
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
                        cmd.Parameters.Add(new SqlParameter("@AMCST_Id",
                        SqlDbType.BigInt)
                        {
                            Value = data.AMCST_Id
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
                            data.all_requestdata = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
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

        public CLGStudentRequest_DTO save(CLGStudentRequest_DTO data)
        {
            try
            {
                if (data.HLHSREQC_Id == 0)
                {
                    var duplicate = _hostelContext.HL_Hostel_Student_Request_College_DMO.Where(t => t.AMCST_Id == data.AMCST_Id && t.HLMH_Id == data.HLMH_Id && t.HLHSREQC_Id != 0 && t.HLHSREQC_ActiveFlag == true && (t.HLHSREQC_BookingStatus == "Approved" || t.HLHSREQC_BookingStatus == "Waiting")).Distinct().ToArray();

                    //var duplicate1 = _hostelContext.HL_Hostel_Student_Allot_College_DMO.Where(t => t.AMCST_Id == data.AMCST_Id && t.HLMH_Id == data.HLMH_Id && t.HLMRCA_Id == data.HLMRCA_Id && t.HLHSALTC_VacateFlg == true).Distinct().ToArray();

                    var duplicate1 = _hostelContext.HL_Hostel_Student_Allot_College_DMO.Where(t => t.AMCST_Id == data.AMCST_Id  && t.HLHSALTC_VacateFlg == false).Distinct().ToArray();


                    if (duplicate.Count() > 0 || duplicate1.Count() > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        HL_Hostel_Student_Request_College_DMO obj1 = new HL_Hostel_Student_Request_College_DMO();
                        obj1.MI_Id = data.MI_Id;
                        obj1.HLHSREQC_RequestDate = data.HLHSREQC_RequestDate;
                        obj1.HLMH_Id = data.HLMH_Id;
                        obj1.HLMRCA_Id = data.HLMRCA_Id;
                        obj1.AMCST_Id = data.AMCST_Id;
                        obj1.HLHSREQC_ACRoomReqdFlg = data.HLHSREQC_ACRoomReqdFlg;
                        obj1.HLHSREQC_EntireRoomReqdFlg = data.HLHSREQC_EntireRoomReqdFlg;
                        obj1.HLHSREQC_VegMessReqdFlg = data.HLHSREQC_VegMessReqdFlg;
                        obj1.HLHSREQC_NonVegMessReqdFlg = data.HLHSREQC_NonVegMessReqdFlg;
                        obj1.HLHSREQC_Remarks = data.HLHSREQC_Remarks;
                        obj1.HLHSREQC_BookingStatus = "Waiting";
                        obj1.HLHSREQC_ActiveFlag = true;
                        obj1.HLHSREQC_CreatedDate = DateTime.Now;
                        obj1.HLHSREQC_UpdatedDate = DateTime.Now;
                        obj1.HLHSREQC_CreatedBy = data.UserId;
                        obj1.HLHSREQC_UpdatedBy = data.UserId;
                        obj1.HRMRM_Id = data.HRMRM_Id;
                        _hostelContext.Add(obj1);
                        var a = _hostelContext.SaveChanges();
                        if (a > 0)
                        {
                            data.returnval = true;
                        }
                        else
                        {
                            data.returnval = false;
                        }
                    }
                }
                else if (data.HLHSREQC_Id > 0)
                {
                    //var duplicate = _hostelContext.HL_Hostel_Student_Request_College_DMO.Where(t => t.HLHSREQC_Id != data.HLHSREQC_Id && t.HLMH_Id == data.HLMH_Id && t.HLMRCA_Id == data.HLMRCA_Id).Distinct().ToArray();
                    //if (duplicate.Count() > 0)
                    //{
                    //    data.duplicate = true;
                    //}
                    //else
                    //{

                    var obj1 = _hostelContext.HL_Hostel_Student_Request_College_DMO.Where(t => t.HLHSREQC_Id == data.HLHSREQC_Id).SingleOrDefault();
                    obj1.HLHSREQC_RequestDate = data.HLHSREQC_RequestDate;
                    obj1.HLMH_Id = data.HLMH_Id;
                    obj1.HLMRCA_Id = data.HLMRCA_Id;
                    obj1.AMCST_Id = data.AMCST_Id;
                    obj1.HLHSREQC_ACRoomReqdFlg = data.HLHSREQC_ACRoomReqdFlg;
                    obj1.HLHSREQC_EntireRoomReqdFlg = data.HLHSREQC_EntireRoomReqdFlg;
                    obj1.HLHSREQC_VegMessReqdFlg = data.HLHSREQC_VegMessReqdFlg;
                    obj1.HLHSREQC_NonVegMessReqdFlg = data.HLHSREQC_NonVegMessReqdFlg;
                    obj1.HLHSREQC_Remarks = data.HLHSREQC_Remarks;
                    obj1.HLHSREQC_BookingStatus = "Waiting";
                    obj1.HLHSREQC_UpdatedDate = DateTime.Now;
                    obj1.HLHSREQC_UpdatedBy = data.UserId;
                    obj1.HRMRM_Id = data.HRMRM_Id;
                    _hostelContext.Update(obj1);
                    var r = _hostelContext.SaveChanges();
                    if (r > 0)
                    {
                        data.returnupdate = "updated";
                    }
                    else
                    {
                        data.returnupdate = "failed";
                    }
                }
                // }

            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
            return data;
        }
        public CLGStudentRequest_DTO edittab1(CLGStudentRequest_DTO data)
        {
            try
            {


                data.editlist = (from d in _hostelContext.HL_Hostel_Student_Request_College_DMO
                                 where (d.HLHSREQC_Id == data.HLHSREQC_Id && d.MI_Id == data.MI_Id)
                                 select d).Distinct().ToArray();
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
            return data;
        }
        public CLGStudentRequest_DTO roomdetails(CLGStudentRequest_DTO data)
        {
            try
            {
                long cc = 0;
                //data.roomdetails = _hostelContext.HR_Master_Room_DMO.Where(t => t.MI_Id == data.MI_Id && t.HLMRCA_Id == data.HLMRCA_Id && t.HRMRM_ActiveFlag == true).Distinct().ToArray();

                data.roomdetails = _hostelContext.HL_Master_Room_Category_DMO.Where(t => t.MI_Id == data.MI_Id && t.HLMRCA_Id == data.HLMRCA_Id).ToArray();


                var bcount = _hostelContext.HL_Hostel_Student_Allot_College_DMO.Where(t => t.MI_Id == data.MI_Id && t.HLMRCA_Id == data.HLMRCA_Id && t.HLHSALTC_ActiveFlag == true).Count();
                var BedCapacity = _hostelContext.HL_Master_Room_Category_DMO.Where(t => t.MI_Id == data.MI_Id && t.HLMRCA_Id == data.HLMRCA_Id).ToArray();

                //.Select(t => Convert.ToInt32( t.HLMRCA_MaxCapacity));
                cc = BedCapacity[0].HLMRCA_MaxCapacity;
                if (bcount >= cc)
                {
                    data.bedcount = true;
                }
                var Type = "Search";
                var HRMRM_Id = 0;


                using (var cmd = _hostelContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "CLG_HOSTEL_Preperd_partnar";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.BigInt)
                    {
                        Value = data.ASMAY_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@HLMH_Id", SqlDbType.BigInt)
                    {
                        Value = data.HLMH_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@Type", SqlDbType.VarChar)
                    {
                        Value = Type
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRMRM_Id", SqlDbType.BigInt)
                    {
                        Value = HRMRM_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@HLMRCA_Id", SqlDbType.BigInt)
                    {
                        Value = data.HLMRCA_Id
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
                        data.studentList = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }



            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
            return data;
        }


        public CLGStudentRequest_DTO Catgory(CLGStudentRequest_DTO data)
        {
            try
            {

                data.room_list = _hostelContext.HL_Master_Room_Category_DMO.Where(t => t.MI_Id == data.MI_Id && t.HLMH_Id == data.HLMH_Id && t.HLMRCA_ActiveFlag == true).Distinct().ToArray();



               // data.facility_list = _hostelContext.HL_Master_Hostel_Facilities_DMO.Where(t => t.MI_Id == data.MI_Id && t.HLMH_Id == data.HLMH_Id && t.HLMHF_ActiveFlg == true).Distinct().ToArray();

                data.facility_list = (from a in _hostelContext.HL_Master_Hostel_Facilities_DMO
                                      from b in _hostelContext.HL_Master_Facility_DMO
                                      where (a.HLMH_Id == data.HLMH_Id && a.HLMFTY_Id==b.HLMFTY_Id && a.MI_Id == data.MI_Id && b.HLMFTY_ActiveFlag==true)
                                      select new CLGStudentRequest_DTO
                                      {
                                          HLMFTY_Id=a.HLMFTY_Id,
                                          HLMFTY_FacilityName =b.HLMFTY_FacilityName
                                      }).Distinct().ToArray();


            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
            return data;
        }
        public CLGStudentRequest_DTO getPdetails(CLGStudentRequest_DTO data)
        {
            try
            {
                var HLMF_Id = 0;
                using (var cmd = _hostelContext.Database.GetDbConnection().CreateCommand())
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
                        Value = HLMF_Id
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
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
            return data;
        }
        public CLGStudentRequest_DTO deactive(CLGStudentRequest_DTO data)
        {
            try
            {
                var g = _hostelContext.HL_Hostel_Student_Request_College_DMO.Where(t => t.HLHSREQC_Id == data.HLHSREQC_Id).SingleOrDefault();
                if (g.HLHSREQC_ActiveFlag == true)
                {
                    g.HLHSREQC_ActiveFlag = false;
                }
                else if (g.HLHSREQC_ActiveFlag == false)
                {
                    g.HLHSREQC_ActiveFlag = true;
                }

                g.MI_Id = data.MI_Id;
                _hostelContext.Update(g);
                int s = _hostelContext.SaveChanges();
                if (s > 0)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }

    }
}
