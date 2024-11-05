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
    public class Hostel_Allotment_ReportImpl : Interface.Hostel_Allotment_ReportInterface
    {
        public HostelContext _HostelContext;
        public DomainModelMsSqlServerContext _dbcontext;
        public Hostel_Allotment_ReportImpl(HostelContext para1, DomainModelMsSqlServerContext para2)
        {
            _HostelContext = para1;
            _dbcontext = para2;
        }
        public Hostel_Allotment_ReportDTO getdata(Hostel_Allotment_ReportDTO data)
        {
            try
            {
                var list = _HostelContext.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).OrderByDescending(a => a.ASMAY_Order).ToList();
                data.yearlist = list.ToArray();
                data.hostellist = _HostelContext.HL_Master_Hostel_DMO.Where(a => a.MI_Id == data.MI_Id && a.HLMH_ActiveFlag == true).Distinct().ToArray();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public async Task<Hostel_Allotment_ReportDTO> getreport(Hostel_Allotment_ReportDTO data)
        {
            try
            {
                using (var cmd = _HostelContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "HOSTEL_STUDENT_ALLOTMENT_REPORT";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@type", SqlDbType.VarChar)
                    {
                        Value = data.type
                    });
                    cmd.Parameters.Add(new SqlParameter("@frmdate", SqlDbType.VarChar)
                    {
                        Value = data.frmdate
                    });
                    cmd.Parameters.Add(new SqlParameter("@todate", SqlDbType.VarChar)
                    {
                        Value = data.todate
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.BigInt)
                    {
                        Value = data.ASMAY_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@HLMH_Id", SqlDbType.BigInt)
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
                        data.griddata = retObject.ToArray();
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

        //Hostel Allotment Graphical Presentation Report
        public Hostel_Allotment_ReportDTO Get_GP_OnLoad_Report(Hostel_Allotment_ReportDTO data)
        {
            try
            {
                data.yearlist = _HostelContext.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).OrderByDescending(a => a.ASMAY_Order).ToArray();

                data.hostellist = _HostelContext.HL_Master_Hostel_DMO.Where(a => a.MI_Id == data.MI_Id && a.HLMH_ActiveFlag == true).Distinct().ToArray();

                data.roomcategorylist = _HostelContext.HL_Master_Room_Category_DMO.Where(a => a.MI_Id == data.MI_Id
                && a.HLMRCA_ActiveFlag == true).Distinct().ToArray();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public Hostel_Allotment_ReportDTO OnChangeHostel(Hostel_Allotment_ReportDTO data)
        {
            try
            {
                data.floorlist = _HostelContext.HR_Master_Floor_DMO.Where(a => a.MI_Id == data.MI_Id && a.HLMH_Id == data.HLMH_Id
                && a.HRMF_ActiveFlag == true).Distinct().ToArray();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public Hostel_Allotment_ReportDTO Get_GP_Report(Hostel_Allotment_ReportDTO data)
        {
            try
            {
                string floorids = "0";
                if (data.Floor_DTO_Temp != null && data.Floor_DTO_Temp.Length > 0)
                {
                    foreach (var c in data.Floor_DTO_Temp)
                    {
                        floorids += "," + c.HLMF_Id.ToString();
                    }
                }

                string roomcategoryids = "0";
                if (data.Room_Category_DTO_Temp != null && data.Room_Category_DTO_Temp.Length > 0)
                {
                    foreach (var c in data.Room_Category_DTO_Temp)
                    {
                        roomcategoryids += "," + c.HLMRCA_Id.ToString();
                    }
                }

                var get_institutiondetails = _HostelContext.Institute.Where(a => a.MI_Id == data.MI_Id).ToList();

                string institution_flag = "S";
                if (get_institutiondetails != null && get_institutiondetails.Count > 0)
                {
                    institution_flag = get_institutiondetails.FirstOrDefault().MI_SchoolCollegeFlag == "S" ? "S" : "C";
                }

                using (var cmd = _HostelContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "HL_Hostel_Alloted_Graphic_Presentation_Details";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@HLMH_Id", SqlDbType.VarChar) { Value = data.HLMH_Id });
                    cmd.Parameters.Add(new SqlParameter("@HLMF_Id", SqlDbType.VarChar) { Value = floorids });
                    cmd.Parameters.Add(new SqlParameter("@HLMRCA_Id", SqlDbType.VarChar) { Value = roomcategoryids });
                    cmd.Parameters.Add(new SqlParameter("@Type", SqlDbType.VarChar) { Value = institution_flag });

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
                                    dataRow.Add(dataReader.GetName(iFiled),
                                        dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.griddata = retObject.ToArray();
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
        public Hostel_Allotment_ReportDTO Get_GP_RoomWise_StudentAlloted_Details(Hostel_Allotment_ReportDTO data)
        {
            try
            {                 

                var get_institutiondetails = _HostelContext.Institute.Where(a => a.MI_Id == data.MI_Id).ToList();

                string institution_flag = "S";
                if (get_institutiondetails != null && get_institutiondetails.Count > 0)
                {
                    institution_flag = get_institutiondetails.FirstOrDefault().MI_SchoolCollegeFlag == "S" ? "S" : "C";
                }

                data.institution_flag = institution_flag;

                using (var cmd = _HostelContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "HL_Hostel_Alloted_Graphic_Presentation_StudentDetails";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@HLMH_Id", SqlDbType.VarChar) { Value = data.HLMH_Id });
                    cmd.Parameters.Add(new SqlParameter("@HLMF_Id", SqlDbType.VarChar) { Value = data.HLMF_Id });
                    cmd.Parameters.Add(new SqlParameter("@HLMRCA_Id", SqlDbType.VarChar) { Value = data.HLMRCA_Id });
                    cmd.Parameters.Add(new SqlParameter("@HRMRM_Id", SqlDbType.VarChar) { Value = data.HRMRM_Id });
                    cmd.Parameters.Add(new SqlParameter("@Type", SqlDbType.VarChar) { Value = institution_flag });

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
                                    dataRow.Add(dataReader.GetName(iFiled),
                                        dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.getstudentalloteddata = retObject.ToArray();
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
    }
}