

using System.Collections.Generic;
using System.Linq;
using PreadmissionDTOs;
using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using System.Collections.Concurrent;
using System;
using DomainModel.Model.com.vaps.Exam;
using AutoMapper;
using PreadmissionDTOs.com.vaps.Exam;
using DataAccessMsSqlServerProvider.com.vapstech.Portals;
using DomainModel.Model.com.vapstech.TT;
using DomainModel.Model.com.vaps.admission;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using PreadmissionDTOs.com.vaps.Portals.Chirman;
using DataAccessMsSqlServerProvider.com.vapstech.COE;

namespace PortalHub.com.vaps.Chairman.Services
{
    public class Ch_DatewiseAttendanceImpl : Interfaces.Ch_DatewiseAttendanceInterface
    {
        private static ConcurrentDictionary<string, Ch_DatewiseAttendanceDTO> _login =
         new ConcurrentDictionary<string, Ch_DatewiseAttendanceDTO>();

        private readonly PortalContext _ChairmanDashboardContext;
        ILogger<HomeSchoolAdmImpl> _acdimpl;
        public DomainModelMsSqlServerContext _db;
        public Ch_DatewiseAttendanceImpl(PortalContext cpContext, DomainModelMsSqlServerContext db)
        {
            _ChairmanDashboardContext = cpContext;
            _db = db;
        }
        
        public Ch_DatewiseAttendanceDTO Getdetails(Ch_DatewiseAttendanceDTO data)
        {
         
            try
            {

                List<Ch_DatewiseAttendanceDTO> result3 = new List<Ch_DatewiseAttendanceDTO>();
                List<MasterAcademic> list = new List<MasterAcademic>();
                list = _db.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true ).OrderByDescending(t => t.ASMAY_Order).ToList();
                data.yearlist = list.OrderByDescending(t => t.ASMAY_Order).ToArray();


            }
            catch (Exception ex)
            {
                _acdimpl.LogError(ex.Message);
                _acdimpl.LogDebug(ex.Message);
            }
            return data;

        }


        public Ch_DatewiseAttendanceDTO Getreport(Ch_DatewiseAttendanceDTO data)
        {
            try
            {
                //data.asmS_Id = 21;
               // data.asmcL_Id = 34;
                List<Ch_DatewiseAttendanceDTO> result1 = new List<Ch_DatewiseAttendanceDTO>();
                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Chairman_Datewise_attendence";
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
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                      SqlDbType.BigInt)
                    {
                        Value = data.asmcL_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id",
                      SqlDbType.BigInt)
                    {
                        Value = data.asmS_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@FRM_DATE",
                     SqlDbType.Date)
                    {
                        Value = data.fromdate
                    });
                    cmd.Parameters.Add(new SqlParameter("@TO_DATE",
                      SqlDbType.Date)
                    {
                        Value = data.todate
                    });
                    cmd.Parameters.Add(new SqlParameter("@CONDITION",
                      SqlDbType.VarChar,50)
                    {
                        Value = data.condition
                    }); cmd.Parameters.Add(new SqlParameter("@VALUE",
                     SqlDbType.Decimal)
                    {
                        Value = data.value
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
                                result1.Add(new Ch_DatewiseAttendanceDTO
                                {
                                    present= Convert.ToDecimal(dataReader["TOTAL_PRESENT"].ToString()),
                                    classheld = Convert.ToDecimal(dataReader["CLASS_HELD"].ToString()),
                                    perc = Convert.ToDecimal(dataReader["per"].ToString()),
                                    name = (dataReader["name"].ToString()),
                                    admno =(dataReader["AMST_AdmNo"].ToString()),
                                    rollno = Convert.ToInt32(dataReader["AMAY_RollNo"].ToString()),



                                });
                                data.attendencelist = result1.ToArray();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
            return data;
        }

     
          public Ch_DatewiseAttendanceDTO Getsection(Ch_DatewiseAttendanceDTO data)
        {
          
            try
            {

                data.fillsection = (from a in _db.School_Adm_Y_StudentDMO
                                    from b in _db.admissioncls
                                   // from c in _db.Adm_M_Student
                                    from d in _db.Section
                                    where ( a.ASMCL_Id == b.ASMCL_Id && b.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && b.ASMCL_Id == a.ASMCL_Id  && a.ASMCL_Id == data.asmcL_Id && d.ASMS_Id == a.ASMS_Id)
                                    select new Ch_DatewiseAttendanceDTO
                                    {
                                        sectionname = d.ASMC_SectionName,
                                        asmS_Id = d.ASMS_Id
                                    }).Distinct().OrderBy(t => t.asmS_Id).ToArray();



            }
            catch (Exception ex)
            {
                _acdimpl.LogError(ex.Message);
                _acdimpl.LogDebug(ex.Message);
            }
            return data;

        }
        public Ch_DatewiseAttendanceDTO getclass(Ch_DatewiseAttendanceDTO data)//int IVRMM_Id
        {
            
            try
            {
                List<Ch_DatewiseAttendanceDTO> result3 = new List<Ch_DatewiseAttendanceDTO>();
               

                data.classarray = (from a in _db.School_Adm_Y_StudentDMO
                                   from b in _db.admissioncls
                                   where (b.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == b.ASMCL_Id && b.ASMCL_ActiveFlag == true && a.AMAY_ActiveFlag == 1)


                                   select new Ch_DatewiseAttendanceDTO
                                   {
                                       Class_Name = b.ASMCL_ClassName,
                                       asmcL_Id = b.ASMCL_Id
                                   }).Distinct().OrderBy(t => t.asmcL_Id).ToArray();


                


            }
            catch (Exception ex)
            {
                _acdimpl.LogError(ex.Message);
                _acdimpl.LogDebug(ex.Message);
            }
            return data;

        }








    }
}
