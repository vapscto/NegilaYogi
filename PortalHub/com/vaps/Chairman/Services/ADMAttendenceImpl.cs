

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
    public class ADMAttendenceImpl : Interfaces.ADMAttendenceInterface
    {
        private static ConcurrentDictionary<string, ADMAttendenceDTO> _login =
         new ConcurrentDictionary<string, ADMAttendenceDTO>();

        private readonly PortalContext _ChairmanDashboardContext;
        ILogger<HomeSchoolAdmImpl> _acdimpl;
        public DomainModelMsSqlServerContext _db;
        public ADMAttendenceImpl(PortalContext cpContext, DomainModelMsSqlServerContext db)
        {
            _ChairmanDashboardContext = cpContext;
            _db = db;
        }
        
        public ADMAttendenceDTO Getdetails(ADMAttendenceDTO data)//int IVRMM_Id
        {
           


                List<MasterAcademic> list = new List<MasterAcademic>();
            list = _db.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).OrderByDescending(t => t.ASMAY_Order).ToList();
            data.yearlist = list.OrderByDescending(t => t.ASMAY_Order).ToArray();


            return data;

        }



        
             public ADMAttendenceDTO Getsection(ADMAttendenceDTO data)//int IVRMM_Id
        {


            try
            {



                data.fillsection = (from a in _db.School_Adm_Y_StudentDMO
                                           from b in _db.admissioncls
                                           from d in _db.Section
                                           where (  a.ASMCL_Id == b.ASMCL_Id && b.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id   && b.ASMCL_Id == a.ASMCL_Id && a.ASMCL_Id == data.asmcL_Id && d.ASMS_Id == a.ASMS_Id)
                                    select new ADMAttendenceDTO
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
        public ADMAttendenceDTO getclass(ADMAttendenceDTO data)//int IVRMM_Id
        {


            try {

                data.fillclass = (from a in _db.School_Adm_Y_StudentDMO
                                   from b in _db.admissioncls
                                   where (b.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == b.ASMCL_Id && b.ASMCL_ActiveFlag == true && a.AMAY_ActiveFlag == 1)


                                   select new ADMAttendenceDTO
                                   {
                                       classname = b.ASMCL_ClassName,
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


        public ADMAttendenceDTO GetAttendence(ADMAttendenceDTO data)//int IVRMM_Id
        {


            try
            {
                //if (data.type==2)
                //{
                    data.Fillstudents = (from a in _db.School_Adm_Y_StudentDMO
                                        from b in _db.admissioncls
                                        from c in _db.Adm_M_Student
                                        from d in _db.Section
                                        where (
                                        a.ASMAY_Id==data.ASMAY_Id && a.ASMCL_Id==data.asmcL_Id && a.ASMS_Id==data.asmS_Id && c.MI_Id==b.MI_Id && b.MI_Id==d.MI_Id && a.AMAY_ActiveFlag==1 && c.AMST_SOL=="S" && c.AMST_ActiveFlag==1 && a.ASMCL_Id==b.ASMCL_Id && a.ASMS_Id==d.ASMS_Id && a.AMST_Id==c.AMST_Id
                                     //   a.AMST_Id == c.AMST_Id && a.ASMCL_Id == b.ASMCL_Id && b.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && c.AMST_SOL.Equals("S") && c.AMST_ActiveFlag == 1 && a.AMAY_ActiveFlag == 1 && b.ASMCL_Id == a.ASMCL_Id && b.ASMCL_Id == c.ASMCL_Id && a.ASMCL_Id == data.asmcL_Id && d.ASMS_Id == a.ASMS_Id && a.ASMS_Id==data.asmS_Id 
                    )
                                        select new ADMAttendenceDTO
                                        {
                                            //studentname = c.AMST_FirstName,
                                            AMST_FirstName=c.AMST_FirstName,
                                            studentname =((c.AMST_FirstName == null ? " " : c.AMST_FirstName) + " " +(c.AMST_MiddleName == null ? " " : c.AMST_MiddleName) + " " + (c.AMST_LastName == null ? " " : c.AMST_LastName)).Trim(), // studentname = ((e.AMST_FirstName == null || e.AMST_FirstName == "" ? "" : " " + e.AMST_FirstName) + (e.AMST_MiddleName == null || e.AMST_MiddleName == "" || e.AMST_MiddleName == "0" ? "" : " " + e.AMST_MiddleName) + (e.AMST_LastName == null || e.AMST_LastName == "" || e.AMST_LastName == "0" ? "" : " " + e.AMST_LastName)).Trim(),
                                            amstid = a.AMST_Id
                                        }).Distinct().OrderBy(t => t.AMST_FirstName).ToArray();
                //}


               





            }
            catch (Exception ex)
            {
                _acdimpl.LogError(ex.Message);
                _acdimpl.LogDebug(ex.Message);
            }
            return data;

        }



        public ADMAttendenceDTO GetIndividualAttendence(ADMAttendenceDTO data)
        {
            try
            {
                List<ADMAttendenceDTO> result2 = new List<ADMAttendenceDTO>();
                List<ADMAttendenceDTO> result1 = new List<ADMAttendenceDTO>();
                if (data.type==2)
                {
                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "PORTAL_STUDENT_MONTHLY_ATTENDANCE";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@asmay_id",
                         SqlDbType.BigInt)
                        {
                            Value = data.ASMAY_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@mi_id",
                          SqlDbType.BigInt)
                        {
                            Value = data.MI_Id
                        });

                        cmd.Parameters.Add(new SqlParameter("@amst_id",
                        SqlDbType.BigInt)
                        {
                            Value = data.amstid
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
                                    result1.Add(new ADMAttendenceDTO
                                    {
                                        monthname = dataReader["MONTH_NAME"].ToString(),
                                        present = Convert.ToDecimal(dataReader["TOTAL_PRESENT"].ToString()),
                                        classheld = Convert.ToDecimal(dataReader["CLASS_HELD"].ToString()),

                                        perc = (Convert.ToDecimal(dataReader["TOTAL_PRESENT"].ToString()) / Convert.ToDecimal(dataReader["CLASS_HELD"].ToString())) * 100


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
                if (data.type==1)
                {

                    data.fillmonths = _ChairmanDashboardContext.IVRM_Month_DMO.Where(t => t.Is_Active == true).ToArray();



                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "ALLSTUDENT_MONTHLY_ATTENDANCE_PORTAL";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@asmay_id",
                         SqlDbType.BigInt)
                        {
                            Value = data.ASMAY_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@mi_id",
                          SqlDbType.BigInt)
                        {
                            Value = data.MI_Id
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






                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();

                        var retObject = new List<dynamic>();


                        try
                        {
                            using (var dataReader = cmd.ExecuteReader())
                            {
                                while (dataReader.Read())
                                {
                                    result2.Add(new ADMAttendenceDTO
                                    {   amstid= Convert.ToInt64(dataReader["AMST_Id"].ToString()),
                                        studentname = dataReader["name"].ToString(),
                                        monthid = Convert.ToInt64(dataReader["month_id"].ToString()),
                                        monthname = dataReader["MONTH_NAME"].ToString(),
                                        present = Convert.ToDecimal(dataReader["TOTAL_PRESENT"].ToString()),
                                        classheld = Convert.ToDecimal(dataReader["CLASS_HELD"].ToString()),

                                        perc = Convert.ToDecimal(dataReader["per"].ToString()) 


                                    });
                                    data.allstudent = result2.ToArray();
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.Write(ex.Message);
                        }
                    }
                }


            }
            catch (Exception)
            {

                throw;
            }
            return data;
        }

    }
}
