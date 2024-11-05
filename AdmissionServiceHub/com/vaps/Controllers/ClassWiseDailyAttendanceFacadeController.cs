using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AdmissionServiceHub.com.vaps.Interfaces;
using PreadmissionDTOs.com.vaps.admission;
using Microsoft.AspNetCore.Http;
using PreadmissionDTOs;
using System.Text;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using DataAccessMsSqlServerProvider;
using Microsoft.EntityFrameworkCore;
using DomainModel.Model;
using System.Dynamic;
using System.Net;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace AdmissionServiceHub.com.vaps.Interfaces
{
    [Route("api/[controller]")]

    public class ClassWiseDailyAttendanceFacadeController : Controller
    {
        private readonly DomainModelMsSqlServerContext _db;       
        public ClassWiseDailyAttendanceInterface _Clswisedailyatt;        
        public ClassWiseDailyAttendanceFacadeController(ClassWiseDailyAttendanceInterface clswisedailyatt, DomainModelMsSqlServerContext db)
        {
            _Clswisedailyatt = clswisedailyatt;
            _db = db;
        }


        // [HttpGet]

        [Route("Getdetails")]
        public SchoolYearWiseStudentDTO Getdetails([FromBody]SchoolYearWiseStudentDTO castecategoryDTO)//int IVRMM_Id
        {
            //SchoolYearWiseStudentDTO castecategoryDTO = new SchoolYearWiseStudentDTO();
            //  castecategoryDTO.MI_Id = mi_id;
            return _Clswisedailyatt.GetddlDatabind(castecategoryDTO);

        }

        [Route("getsection")]
        public SchoolYearWiseStudentDTO getsection([FromBody] SchoolYearWiseStudentDTO MMD)
        {
            return _Clswisedailyatt.getsection(MMD);
        }
        [Route("setfromdate")]
        public SchoolYearWiseStudentDTO setfromdate([FromBody] SchoolYearWiseStudentDTO MMD)
        {
            return _Clswisedailyatt.setfromdate(MMD);
        }



        //[HttpGet]
        [HttpPost]
        [Route("Getdetailsreport/")]

        public async Task<SchoolYearWiseStudentDTO> Getdetailsreport([FromBody] SchoolYearWiseStudentDTO reg)
        {
            //int k = 0;             
            var asmsid = "";
            var check_rolename = (from a in _db.MasterRoleType
                                  where (a.IVRMRT_Id == reg.roleId)
                                  select new StudentAttendanceEntryDTO
                                  {
                                      rolename = a.IVRMRT_Role,
                                  }
                                  ).ToList();

            var empcode_check = (from a in _db.Staff_User_Login
                                 where (a.MI_Id == reg.MI_Id && a.IVRMSTAUL_UserName.Equals(reg.username.Trim()))
                                 select new StudentAttendanceEntryDTO
                                 {
                                     Emp_Code = a.Emp_Code,
                                 }).ToList();


            if (check_rolename.FirstOrDefault().rolename.Equals("STAFF") || check_rolename.FirstOrDefault().rolename.Equals("Staff"))
            {
                //k = 1;
                if (empcode_check.Count > 0)
                {
                    if (reg.ASMS_Id == 0)
                    {
                        var SectionList12 = (from a in _db.Adm_SchAttLoginUserClass
                                             from b in _db.Adm_SchAttLoginUser
                                             from c in _db.Section
                                             where (a.ASALU_Id == b.ASALU_Id && c.ASMS_Id == a.ASMS_Id
                                             && b.MI_Id == reg.MI_Id && b.ASMAY_Id == reg.ASMAY_Id
                                             && b.HRME_Id == empcode_check.FirstOrDefault().Emp_Code
                                             && c.ASMC_ActiveFlag == 1)
                                             select new StudentAttendanceEntryDTO
                                             {
                                                 ASMS_Id = c.ASMS_Id,
                                                 asmC_SectionName = c.ASMC_SectionName,
                                             }).Distinct().ToList();

                        for (int i = 0; i < SectionList12.Count; i++)
                        {

                            if (i == 0)
                            {
                                asmsid = SectionList12[i].ASMS_Id.ToString();
                            }
                            else
                            {
                                asmsid = asmsid + ',' + SectionList12[i].ASMS_Id.ToString();
                            }
                        }
                    }
                    else
                    {
                        asmsid = reg.ASMS_Id.ToString();
                    }
                }
            }
            else
            {
                if (reg.ASMS_Id == 0)
                {
                    var sectiondata = (from a in _db.admissioncls
                                       from b in _db.Section
                                       from c in _db.AcademicYear
                                       from d in _db.Masterclasscategory
                                       from e in _db.AdmSchoolMasterClassCatSec
                                       where (a.ASMCL_Id == d.ASMCL_Id && b.ASMS_Id == e.ASMS_Id && c.ASMAY_Id == d.ASMAY_Id && d.ASMCC_Id == e.ASMCC_Id
                                       && d.MI_Id == reg.MI_Id && d.ASMAY_Id == reg.ASMAY_Id && d.ASMCL_Id == reg.ASMCL_Id)
                                       select b
                               ).Distinct().OrderBy(g => g.ASMC_Order).ToList();


                    for (int i = 0; i < sectiondata.Count; i++)
                    {

                        if (i == 0)
                        {
                            asmsid = sectiondata[i].ASMS_Id.ToString();
                        }
                        else
                        {
                            asmsid = asmsid + ',' + sectiondata[i].ASMS_Id.ToString();
                        }
                    }
                }
                else
                {
                    asmsid = reg.ASMS_Id.ToString();
                }
            }



            using (var cmd = _db.Database.GetDbConnection().CreateCommand())
            {
                cmd.CommandText = "Classwise_Dailyattendence_Report";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@year",
                    SqlDbType.VarChar)
                {
                    Value = Convert.ToInt32(reg.ASMAY_Id)
                });
                cmd.Parameters.Add(new SqlParameter("@fromdate",
                   SqlDbType.VarChar)
                {
                    Value = Convert.ToDateTime(reg.ASA_FromDate).ToString("dd/MM/yyyy")
                });
                cmd.Parameters.Add(new SqlParameter("@class",
               SqlDbType.VarChar)
                {
                    Value = reg.ASMCL_Id
                });
                cmd.Parameters.Add(new SqlParameter("@sec",
               SqlDbType.VarChar)
                {
                    Value = asmsid
                });
                cmd.Parameters.Add(new SqlParameter("@miid",
            SqlDbType.VarChar)
                {
                    Value = reg.MI_Id
                });


                if (cmd.Connection.State != ConnectionState.Open)
                    cmd.Connection.Open();

                var retObject = new List<dynamic>();
                //var data = cmd.ExecuteNonQuery();

                try
                {
                    // var data = cmd.ExecuteNonQuery();

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
                    reg.SearchstudentDetails = retObject.ToArray();// SearchstudentDetails     ASA_AttendanceFlag == Absent  asmcl_classname   AMST_FirstName

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

            }

            return reg;
        }

        //[Route("GetSelectedRowDetails/{id:int}")]
        //public castecategoryDTO GetSelectedRowDetails(int ID)
        //{

        //    return _Clswisedailyatt.GetSelectedRowDetails(ID);
        //}

        //[HttpPost]
        //public castecategoryDTO Post([FromBody] castecategoryDTO masterMDT)
        //{

        //    return _Clswisedailyatt.castecategoryData(masterMDT);
        //}

        //[HttpDelete]
        //[Route("MasterDeleteModulesDATA/{id:int}")]
        //public castecategoryDTO MasterDeleteModulesDATA(int ID)
        //{

        //    return _Clswisedailyatt.MasterDeleteModulesData(ID);
        //}
        [Route("absentsendsms")]
        public Task<SchoolYearWiseStudentDTO> absentsendsms([FromBody] SchoolYearWiseStudentDTO data)
        {
            return _Clswisedailyatt.absentsendsms(data);
        }

        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {

        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {

        }

    }
}
