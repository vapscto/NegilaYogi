using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.Sports;
using DomainModel.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.com.vaps.Sports;
using SportsServiceHub.com.vaps.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsServiceHub.com.vaps.Services
{
    public class HSEligibilityCerficateImpl : HSEligibilityCerficateInterface
    {
        private readonly SportsContext _sportcontext;
        public StudentAttendanceReportContext _db;
        ILogger<HSEligibilityCerficateImpl> _acdimpl;
        public HSEligibilityCerficateImpl(SportsContext sportcontext, StudentAttendanceReportContext db)
        {
            _sportcontext = sportcontext;
            _db = db;
        }

        public HSEligibilityCerficateDTO Getdetails(HSEligibilityCerficateDTO data)
        {
            try
            {
              //  List<MasterAcademic> list = new List<MasterAcademic>();
                data.yearlist= _sportcontext.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).Distinct().OrderByDescending(R => R.ASMAY_Id).ToArray();
                var list = _sportcontext.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).ToList().OrderByDescending(R => R.ASMAY_Id);
                data.ASMAY_Id = list.FirstOrDefault().ASMAY_Id;             
                data.EventList = (from a in _sportcontext.MasterEventsDMO
                                  where (a.MI_Id == data.MI_Id && a.SPCCME_ActiveFlag == true)
                                  select new MasterEventsDTO
                                  {
                                      SPCCME_Id = a.SPCCME_Id,
                                      SPCCME_EventName = a.SPCCME_EventName,

                                  }).Distinct().ToArray();
                using (var cmd = _sportcontext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "SPORTS_StudentList";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.BigInt) { Value = data.ASMAY_Id });
                   
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject1 = new List<dynamic>();

                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(
                                        dataReader.GetName(iFiled1),
                                        dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}
                                    );
                                }

                                retObject1.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.pudatareport = retObject1.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                //
               // data.datareport = _sportcontext.MasterSportsCCGroupDMO.Where(d => d.MI_Id == data.MI_Id && d.SPCCMSCCG_ActiveFlag == true && d.SPCCMSCCG_Under == null).Distinct().OrderBy(t => t.SPCCMSCCG_Id).ToArray();
                //datareport
                using (var cmd = _sportcontext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "SPORTS_SUBEVENT_LIST";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = data.MI_Id });
                  

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject1 = new List<dynamic>();

                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(
                                        dataReader.GetName(iFiled1),
                                        dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}
                                    );
                                }

                                retObject1.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.datareport = retObject1.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

            }
            catch (Exception ex)
            {
                _acdimpl.LogError(ex.Message);
                _acdimpl.LogDebug(ex.Message);
            }
            return data;

        }

        public HSEligibilityCerficateDTO get_class(HSEligibilityCerficateDTO dto)
        {
            try
            {
                dto.ClassList = (from a in _db.admissionyearstudent
                                 from b in _db.admissionClass
                                 where (a.ASMAY_Id == dto.ASMAY_Id && a.AMAY_ActiveFlag == 1 && b.ASMCL_ActiveFlag == true && b.MI_Id == dto.MI_Id)
                                 select b).Distinct().ToArray();
                using (var cmd = _sportcontext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "SPORTS_StudentList";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = dto.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.BigInt) { Value = dto.ASMAY_Id });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject1 = new List<dynamic>();

                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(
                                        dataReader.GetName(iFiled1),
                                        dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}
                                    );
                                }

                                retObject1.Add((ExpandoObject)dataRow1);
                            }
                        }
                        dto.pudatareport = retObject1.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }
        public HSEligibilityCerficateDTO get_section(HSEligibilityCerficateDTO dto)
        {
            try
            {
                dto.SectionList = (from a in _db.admissionyearstudent
                                   from b in _db.masterSection
                                   where (a.ASMAY_Id == dto.ASMAY_Id && a.AMAY_ActiveFlag == 1 && b.ASMC_ActiveFlag == 1 && b.MI_Id == dto.MI_Id && a.ASMCL_Id == dto.ASMCL_Id)
                                   select b).Distinct().ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }

        public HSEligibilityCerficateDTO get_age(HSEligibilityCerficateDTO data)
        {
            try
            {
                List<SPCC_Student_House_DTO> obj1 = new List<SPCC_Student_House_DTO>();
                if (data.SPCCME_EventName=="New")
                {
                    //data.AMST_Date = DateTime.Now;
                   
                    using (var cmd = _sportcontext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "Sports_Student_Age_Calculation_SRKVS";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@mi_id", SqlDbType.BigInt) { Value = data.MI_Id });
                        cmd.Parameters.Add(new SqlParameter("@asmay_id", SqlDbType.BigInt) { Value = data.ASMAY_Id });
                        cmd.Parameters.Add(new SqlParameter("@asmcl_id", SqlDbType.BigInt) { Value = data.ASMCL_Id });
                        cmd.Parameters.Add(new SqlParameter("@asms_id", SqlDbType.BigInt) { Value = data.ASMS_Id });
                        cmd.Parameters.Add(new SqlParameter("@amst_id", SqlDbType.VarChar) { Value = data.AMST_Id });
                        cmd.Parameters.Add(new SqlParameter("@spccsH_AsOnDate", SqlDbType.DateTime) { Value = data.AMST_Date });


                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();

                        var retObject = new List<dynamic>();
                        try
                        {
                            using (var resultc = cmd.ExecuteReader())
                            {
                                
                                while (resultc.Read())
                                {
                                   

                                    obj1.Add(new SPCC_Student_House_DTO
                                    {

                                        SPCCSH_Age = (resultc["studAge"]).ToString(),
                                        SPCCSH_Age_Format = (resultc["ageFormat"]).ToString(),
                                       

                                    });
                                }
                                data.age_tilldate = obj1.ToArray();
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.Write(ex.Message);
                        }
                    }
                }
                else
                {
                    data.AMST_Date = DateTime.Now;
                   
                    using (var cmd = _sportcontext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "Sports_Student_Age_Calculation";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@mi_id", SqlDbType.BigInt) { Value = data.MI_Id });
                        cmd.Parameters.Add(new SqlParameter("@asmay_id", SqlDbType.BigInt) { Value = data.ASMAY_Id });
                        cmd.Parameters.Add(new SqlParameter("@asmcl_id", SqlDbType.BigInt) { Value = data.ASMCL_Id });
                        cmd.Parameters.Add(new SqlParameter("@asms_id", SqlDbType.BigInt) { Value = data.ASMS_Id });
                        cmd.Parameters.Add(new SqlParameter("@amst_id", SqlDbType.VarChar) { Value = data.AMST_Id });
                        cmd.Parameters.Add(new SqlParameter("@spccsH_AsOnDate", SqlDbType.DateTime) { Value = data.AMST_Date });


                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();

                        var retObject = new List<dynamic>();
                        try
                        {
                            using (var resultc = cmd.ExecuteReader())
                            {
                                //_logatt.LogInformation("entered in dataReader block");
                                while (resultc.Read())
                                {
                                    // _logatt.LogInformation("entered in while block");

                                    obj1.Add(new SPCC_Student_House_DTO
                                    {

                                        SPCCSH_Age = (resultc["studAge"]).ToString(),
                                        //SPCCSH_Age = (resultc["ageFormat"]).ToString(),

                                    });
                                }
                                data.age_tilldate = obj1.ToArray();
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.Write(ex.Message);
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

        public HSEligibilityCerficateDTO get_student(HSEligibilityCerficateDTO dto)
        {
            try
            {
                dto.StudentList1 = (from a in _sportcontext.admissionyearstudent
                                    from b in _sportcontext.admissionStduent
                                    where (a.AMST_Id == b.AMST_Id && a.ASMAY_Id == dto.ASMAY_Id && a.AMAY_ActiveFlag == 1 && a.ASMS_Id == dto.ASMS_Id && b.MI_Id == dto.MI_Id && a.ASMCL_Id == dto.ASMCL_Id && a.ASMS_Id == dto.ASMS_Id)
                                    select new HSEligibilityCerficateDTO
                                    {
                                        AMST_Id = b.AMST_Id,
                                        studentname = b.AMST_FirstName + (string.IsNullOrEmpty(b.AMST_MiddleName) || b.AMST_MiddleName == "0" ? "" : ' ' + b.AMST_MiddleName) + (string.IsNullOrEmpty(b.AMST_LastName) || b.AMST_LastName == "0" ? "" : ' ' + b.AMST_LastName),
                                    }
                                   ).Distinct().ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;

        }


        public HSEligibilityCerficateDTO get_certificate(HSEligibilityCerficateDTO data)
        {
            try
            {
                List<long> lastyearid = new List<long>();

                var lastyear = (from a in _sportcontext.AcademicYear
                                where (a.MI_Id == data.MI_Id && a.ASMAY_Year == data.ASMAY_Year && a.Is_Active == true)
                                select a).Distinct().ToList();
                if (lastyear.Count > 0)
                {
                    foreach (var item in lastyear)
                    {
                        lastyearid.Add(item.ASMAY_Id);
                    }
                }
                if(data.SPCCME_EventName == "New")
                {
                    using (var cmd = _sportcontext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "SP_Eligibility_PreviuosClass";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = data.MI_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMAY_ID", SqlDbType.BigInt) { Value = data.ASMAY_Id });
                        cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.BigInt) { Value = data.AMST_Id });

                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();

                        var retObject = new List<dynamic>();

                        try
                        {

                            using (var dataReader = cmd.ExecuteReader())
                            {
                                while (dataReader.Read())
                                {
                                    var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                    for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                    {
                                        dataRow1.Add(
                                            dataReader.GetName(iFiled1),
                                            dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}
                                        );
                                    }

                                    retObject.Add((ExpandoObject)dataRow1);
                                }
                            }
                            data.clsslst = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                    using (var cmd = _sportcontext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "SPORTS_StudentList_Report";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = data.MI_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.BigInt) { Value = data.ASMAY_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.BigInt) { Value = data.ASMCL_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.BigInt) { Value = data.ASMS_Id });
                        cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.BigInt) { Value = data.AMST_Id });
                        cmd.Parameters.Add(new SqlParameter("@SPCCME_Id", SqlDbType.BigInt) { Value = data.SPCCME_Id });
                        
                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();

                        var retObject = new List<dynamic>();

                        try
                        {

                            using (var dataReader = cmd.ExecuteReader())
                            {
                                while (dataReader.Read())
                                {
                                    var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                    for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                    {
                                        dataRow1.Add(
                                            dataReader.GetName(iFiled1),
                                            dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}
                                        );
                                    }

                                    retObject.Add((ExpandoObject)dataRow1);
                                }
                            }
                            data.datareport = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
                else
                {
                    var prevcls = (from a in _sportcontext.Adm_M_Student
                                   from b in _sportcontext.admissionyearstudent
                                   from d in _sportcontext.admissionClass

                                   where (a.AMST_Id == b.AMST_Id && b.ASMCL_Id == d.ASMCL_Id && lastyearid.Contains(b.ASMAY_Id) && a.AMST_Id == data.AMST_Id && a.MI_Id == data.MI_Id)
                                   select new HSEligibilityCerficateDTO
                                   {
                                       prevclsname = d.ASMCL_ClassName,
                                   }).Distinct().ToList();


                    data.clsslst = prevcls.ToArray();
                    data.datareport = (from a in _sportcontext.Institution
                                       from b in _sportcontext.Adm_M_Student
                                       from c in _sportcontext.admissionyearstudent
                                       from d in _sportcontext.admissionClass
                                       from e in _sportcontext.masterSection
                                       from f in _sportcontext.Institution_MobileNo
                                       from g in _sportcontext.Institution_EmailId
                                       from h in _sportcontext.EventsStudentRecordDMO
                                       from newtable in _sportcontext.SPCC_Events_Students_DMO
                                       from i in _sportcontext.EventsMappingDMO
                                       from j in _sportcontext.MasterEventsDMO                                         
                                       from l in _sportcontext.MasterEventsDMO
                                       where (a.MI_Id == data.MI_Id && a.MI_Id == b.MI_Id && b.AMST_Id == data.AMST_Id && c.AMST_Id == data.AMST_Id && b.AMST_Id == c.AMST_Id && d.ASMCL_Id == c.ASMCL_Id && e.ASMS_Id == c.ASMS_Id && f.MI_Id == a.MI_Id && g.MI_Id == a.MI_Id && h.AMST_Id == data.AMST_Id && newtable.SPCCME_Id == i.SPCCME_Id && newtable.SPCCEST_Id == h.SPCCEST_Id /*&& i.SPCCPM_Id == l.SPCCPM_Id*/ && h.AMST_Id == data.AMST_Id && l.SPCCME_Id == data.SPCCME_Id && c.ASMAY_Id == data.ASMAY_Id)
                                       select new HSEligibilityCerficateDTO
                                       {
                                           AMST_BPLCardNo = b.AMST_BPLCardNo,
                                           AMST_AadharNo = b.AMST_AadharNo,
                                           AMST_Id = b.AMST_Id,
                                           studentname = b.AMST_FirstName + (string.IsNullOrEmpty(b.AMST_MiddleName) || b.AMST_MiddleName == "0" ? "" : ' ' + b.AMST_MiddleName) + (string.IsNullOrEmpty(b.AMST_LastName) || b.AMST_LastName == "0" ? "" : ' ' + b.AMST_LastName),
                                           AMST_FatherName = b.AMST_FatherName,
                                           MI_Name = a.MI_Name,
                                           MI_Address = a.MI_Address1 + (string.IsNullOrEmpty(a.MI_Address2) || a.MI_Address2 == "0" ? "" : ' ' + a.MI_Address2) /*+ (string.IsNullOrEmpty(a.MI_Address3) || a.MI_Address3 == "0" ? "" : ' ' + a.MI_Address3)*/ + (string.IsNullOrEmpty(a.MI_AddressArea) || a.MI_AddressArea == "0" ? "" : ' ' + a.MI_AddressArea),
                                           MIMN_MobileNo = f.MIMN_MobileNo,
                                           MIE_EmailId = g.MIE_EmailId,
                                           AMST_DOB = b.AMST_DOB,
                                           AMST_DOB_Words = b.AMST_DOB_Words,
                                           SPCCPM_Name = l.SPCCME_EventName,
                                           //Age_Years = k.Age_Years,
                                           //Age_Months = k.Age_Months,
                                           //Age_Days = k.Age_Days,
                                           Address = b.AMST_PerStreet + (string.IsNullOrEmpty(b.AMST_PerArea) || b.AMST_PerArea == "0" ? "" : ' ' + b.AMST_PerArea) + (string.IsNullOrEmpty(b.AMST_PerAdd3) || b.AMST_PerAdd3 == "0" ? "" : ' ' + b.AMST_PerAdd3),
                                           AMST_FatherMobleNo = b.AMST_FatherMobleNo,
                                           AMST_AdmNo = b.AMST_AdmNo,
                                           AMST_Date = b.AMST_Date,
                                           ASMCL_ClassName = d.ASMCL_ClassName,
                                           MI_Pincode = a.MI_Pincode,
                                           ASMC_SectionName = e.ASMC_SectionName,
                                           AMST_Photoname = b.AMST_Photoname,
                                           AMST_GovtAdmno = b.AMST_GovtAdmno,
                                       }).Distinct().Take(1).ToArray();
                    //b.spccmE_EventName
                }




            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;

        }


        public HSEligibilityCerficateDTO get_PUcertificate(HSEligibilityCerficateDTO data)
        {
            try
            {
                data.pudatareport = (from a in _sportcontext.Institution
                                     from b in _sportcontext.Adm_M_Student
                                     from c in _sportcontext.admissionyearstudent
                                     from d in _sportcontext.admissionClass
                                     from e in _sportcontext.masterSection
                                     from f in _sportcontext.Institution_MobileNo
                                     from g in _sportcontext.Institution_EmailId
                                     from h in _sportcontext.EventsStudentRecordDMO
                                     from newtable in _sportcontext.SPCC_Events_Students_DMO
                                     from i in _sportcontext.EventsMappingDMO
                                     from j in _sportcontext.MasterEventsDMO
                                         //from k in _sportcontext.StudentAgeCalcDMO
                                         // from l in _sportcontext.ProgramMasterDMO
                                     from l in _sportcontext.MasterEventsDMO
                                     where (a.MI_Id == data.MI_Id && a.MI_Id == b.MI_Id && b.AMST_Id == data.AMST_Id && c.AMST_Id == data.AMST_Id && b.AMST_Id == c.AMST_Id && d.ASMCL_Id == c.ASMCL_Id && e.ASMS_Id == c.ASMS_Id && f.MI_Id == a.MI_Id && g.MI_Id == a.MI_Id && h.AMST_Id == data.AMST_Id && newtable.SPCCME_Id == i.SPCCME_Id && newtable.SPCCEST_Id == h.SPCCEST_Id /*&& i.SPCCPM_Id == l.SPCCPM_Id*/ && h.AMST_Id == data.AMST_Id && l.SPCCME_Id == data.SPCCME_Id && c.ASMAY_Id == data.ASMAY_Id)
                                     select new HSEligibilityCerficateDTO
                                     {
                                         AMST_BPLCardNo = b.AMST_BPLCardNo,
                                         AMST_AadharNo = b.AMST_AadharNo,
                                         AMST_MobileNo = b.AMST_MobileNo,
                                         AMST_Id = b.AMST_Id,
                                         studentname = b.AMST_FirstName + (string.IsNullOrEmpty(b.AMST_MiddleName) || b.AMST_MiddleName == "0" ? "" : ' ' + b.AMST_MiddleName) + (string.IsNullOrEmpty(b.AMST_LastName) || b.AMST_LastName == "0" ? "" : ' ' + b.AMST_LastName),
                                         AMST_FatherName = b.AMST_FatherName,
                                         AMST_MotherName = b.AMST_MotherName,
                                         MI_Name = a.MI_Name,
                                         MI_Address = a.MI_Address1 + (string.IsNullOrEmpty(a.MI_Address2) || a.MI_Address2 == "0" ? "" : ' ' + a.MI_Address2) + (string.IsNullOrEmpty(a.MI_Address3) || a.MI_Address3 == "0" ? "" : ' ' + a.MI_Address3) + (string.IsNullOrEmpty(a.MI_AddressArea) || a.MI_AddressArea == "0" ? "" : ' ' + a.MI_AddressArea),
                                         MIMN_MobileNo = f.MIMN_MobileNo,
                                         MIE_EmailId = g.MIE_EmailId,
                                         AMST_DOB = b.AMST_DOB,
                                         AMST_DOB_Words = b.AMST_DOB_Words,
                                         SPCCPM_Name = l.SPCCME_EventName,
                                         //Age_Years = k.Age_Years,
                                         //Age_Months = k.Age_Months,
                                         //Age_Days = k.Age_Days,
                                         Address = b.AMST_PerStreet + (string.IsNullOrEmpty(b.AMST_PerArea) || b.AMST_PerArea == "0" ? "" : ' ' + b.AMST_PerArea) + (string.IsNullOrEmpty(b.AMST_PerAdd3) || b.AMST_PerAdd3 == "0" ? "" : ' ' + b.AMST_PerAdd3),
                                         AMST_FatherMobleNo = b.AMST_FatherMobleNo,
                                         AMST_AdmNo = b.AMST_AdmNo,
                                         AMST_Date = b.AMST_Date,
                                         ASMCL_ClassName = d.ASMCL_ClassName,
                                         ASMC_SectionName = e.ASMC_SectionName,
                                         AMST_Photoname = b.AMST_Photoname,
                                         AMST_GovtAdmno = b.AMST_GovtAdmno,
                                         AMST_MotherBankAccNo = b.AMST_MotherBankAccNo,
                                         AMST_MotherBankIFSC_Code = b.AMST_MotherBankIFSC_Code,
                                     }).Distinct().Take(1).ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;

        }

    }
}
