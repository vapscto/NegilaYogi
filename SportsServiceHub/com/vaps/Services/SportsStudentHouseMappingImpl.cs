using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.Sports;
using DomainModel.Model.com.vapstech.Sports;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.Sports;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsServiceHub.com.vaps.Services
{
    public class SportsStudentHouseMappingImpl : Interfaces.SportsStudentHouseMappingInterface
    {
        DomainModelMsSqlServerContext _db;
        SportsContext _context;
        public SportsStudentHouseMappingImpl(DomainModelMsSqlServerContext db, SportsContext context)
        {
            _db = db;
            _context = context;
        }

        public SPCC_Student_House_DTO getdetails(SPCC_Student_House_DTO dTO)
        {

            try
            {


                dTO.yearlist = _context.AcademicYear.Where(d => d.MI_Id == dTO.MI_Id && d.Is_Active == true).OrderByDescending(a => a.ASMAY_Order).ToArray();


                dTO.houseList = (from a in _context.SportMasterHouseDMO
                                 where (a.MI_Id == dTO.MI_Id && a.SPCCMH_ActiveFlag == true)
                                 select new SPCC_Student_House_DTO
                                 {
                                     SPCCMH_Id = a.SPCCMH_Id,
                                     SPCCMH_HouseName = a.SPCCMH_HouseName,

                                 }).Distinct().ToArray();

                dTO.alldata = (from a in _context.SportStudentHouseDivisionDMO
                               from b in _context.admissionStduent
                               from d in _context.SportMasterHouseDMO
                               from c in _context.admissionClass
                               from s in _context.masterSection
                               from y in _context.admissionyearstudent
                               from z in _context.AcademicYear
                               where (a.ASMAY_Id == y.ASMAY_Id && a.ASMCL_Id == y.ASMCL_Id && a.ASMS_Id == y.ASMS_Id && y.AMST_Id == a.AMST_Id && y.AMST_Id == b.AMST_Id && c.ASMCL_Id == y.ASMCL_Id && s.ASMS_Id == y.ASMS_Id && a.SPCCMH_Id == d.SPCCMH_Id && a.MI_Id == dTO.MI_Id && z.ASMAY_Id == y.ASMAY_Id  && b.AMST_SOL=="S")
                               select new SPCC_Student_House_DTO
                               {
                                   SPCCMH_Id = d.SPCCMH_Id,
                                   SPCCSH_Id = a.SPCCSH_Id,
                                   AMST_Id = a.AMST_Id,
                                   ASMS_Id = a.ASMS_Id,
                                   ASMAY_Id = a.ASMAY_Id,
                                   ASMCL_Id = a.ASMCL_Id,
                                   AMST_AdmNo = b.AMST_AdmNo,
                                   // SPCCSH_Height = a.SPCCSH_Height,
                                   //SPCCSH_Weight = a.SPCCSH_Weight,
                                   //  SPCCSH_Age = a.SPCCSH_Age,
                                   SPCCMH_HouseName = d.SPCCMH_HouseName,
                                   studentname = b.AMST_FirstName + (string.IsNullOrEmpty(b.AMST_MiddleName) || b.AMST_MiddleName == "0" ? "" : ' ' + b.AMST_MiddleName) + (string.IsNullOrEmpty(b.AMST_LastName) || b.AMST_LastName == "0" ? "" : ' ' + b.AMST_LastName),
                                   // SPCCSH_BMI = a.SPCCSH_BMI,
                                   //SPCCSH_BMIRemarks = a.SPCCSH_BMIRemarks,
                                   SPCCSH_Remarks = a.SPCCSH_Remarks,
                                   ASMCL_ClassName = c.ASMCL_ClassName,
                                   SPCCMH_ActiveFlag = a.SPCCMH_ActiveFlag,
                                   ASMC_SectionName = s.ASMC_SectionName,
                                   ASMAY_Year = z.ASMAY_Year,
                               }).Distinct().ToArray();


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return dTO;
        }

        public SPCC_Student_House_DTO get_class(SPCC_Student_House_DTO dto)
        {
            try
            {
                dto.classList = (from a in _context.admissionyearstudent
                                 from b in _context.admissionClass
                                 where (a.ASMCL_Id == b.ASMCL_Id && b.MI_Id == dto.MI_Id && a.ASMAY_Id == dto.ASMAY_Id && a.AMAY_ActiveFlag == 1 && b.ASMCL_ActiveFlag == true)
                                 select new SPCC_Student_House_DTO
                                 {
                                     ASMCL_Id = b.ASMCL_Id,
                                     ASMCL_ClassName = b.ASMCL_ClassName,

                                 }).Distinct().OrderBy(t => t.ASMCL_Id).ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;

        }

        public SPCC_Student_House_DTO get_section(SPCC_Student_House_DTO dto)
        {
            try
            {
                dto.SectionList = (from a in _context.admissionyearstudent
                                   from b in _context.masterSection
                                   where (a.ASMAY_Id == dto.ASMAY_Id && a.AMAY_ActiveFlag == 1 && b.ASMC_ActiveFlag == 1 && b.MI_Id == dto.MI_Id && a.ASMCL_Id == dto.ASMCL_Id)
                                   select new SPCC_Student_House_DTO
                                   {
                                       ASMS_Id = b.ASMS_Id,
                                       ASMC_SectionName = b.ASMC_SectionName,

                                   }).Distinct().OrderBy(t => t.ASMS_Id).ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;

        }

        public SPCC_Student_House_DTO get_student(SPCC_Student_House_DTO dto)
        {
            try
            {

                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "SPC_Filter_Student_HouseMapping_List";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = dto.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.BigInt) { Value = dto.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.BigInt) { Value = dto.ASMCL_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.BigInt) { Value = dto.ASMS_Id });


                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();
                    var retObject = new List<dynamic>();
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

                    dto.StudentList = retObject.ToArray();
                }

                #region due to performanace issue

                //var studlist = _context.SportStudentHouseDivisionDMO.Where(t => t.MI_Id == dto.MI_Id && t.ASMAY_Id==dto.ASMAY_Id).ToList();
                //if (studlist.Count > 0)
                //{
                //    var stud_ids = studlist.Select(t => t.AMST_Id).ToList();

                //dto.StudentList = (from a in _context.admissionyearstudent
                //                   from c in _context.admissionClass
                //                   from s in _context.masterSection
                //                   from b in _context.admissionStduent
                //                   where (b.MI_Id == c.MI_Id && a.ASMCL_Id == c.ASMCL_Id && a.ASMS_Id == s.ASMS_Id && a.AMST_Id == b.AMST_Id && b.MI_Id == dto.MI_Id && a.ASMAY_Id == dto.ASMAY_Id && a.ASMCL_Id == dto.ASMCL_Id && a.ASMS_Id == dto.ASMS_Id && !stud_ids.Contains(b.AMST_Id) && a.AMAY_ActiveFlag == 1 && b.AMST_SOL == "S")
                //                   select new SPCC_Student_House_DTO
                //                   {
                //                       AMST_Id = b.AMST_Id,
                //                       studentname = b.AMST_FirstName + (string.IsNullOrEmpty(b.AMST_MiddleName) || b.AMST_MiddleName == "0" ? "" : ' ' + b.AMST_MiddleName) + (string.IsNullOrEmpty(b.AMST_LastName) || b.AMST_LastName == "0" ? "" : ' ' + b.AMST_LastName),
                //                       AMST_AdmNo = b.AMST_AdmNo,
                //                       ASMCL_Id = c.ASMCL_Id,
                //                       ASMCL_ClassName = c.ASMCL_ClassName,
                //                       ASMS_Id = s.ASMS_Id,
                //                       ASMC_SectionName = s.ASMC_SectionName,
                //                   }
                //          ).Distinct().OrderBy(b => b.studentname).ToArray();                    

                // }
                //else
                //{
                //    dto.StudentList = (from a in _context.admissionyearstudent
                //                       from c in _context.admissionClass
                //                       from s in _context.masterSection
                //                       from b in _context.admissionStduent
                //                       where (b.MI_Id == c.MI_Id && a.ASMCL_Id == c.ASMCL_Id && a.ASMS_Id == s.ASMS_Id && a.AMST_Id == b.AMST_Id && b.MI_Id == dto.MI_Id && a.ASMAY_Id == dto.ASMAY_Id && a.ASMCL_Id == dto.ASMCL_Id && a.ASMS_Id == dto.ASMS_Id && a.AMAY_ActiveFlag == 1 && b.AMST_SOL == "S")
                //                       select new SPCC_Student_House_DTO
                //                       {
                //                           AMST_Id = b.AMST_Id,
                //                           studentname = b.AMST_FirstName + (string.IsNullOrEmpty(b.AMST_MiddleName) || b.AMST_MiddleName == "0" ? "" : ' ' + b.AMST_MiddleName) + (string.IsNullOrEmpty(b.AMST_LastName) || b.AMST_LastName == "0" ? "" : ' ' + b.AMST_LastName),
                //                           AMST_AdmNo = b.AMST_AdmNo,
                //                           ASMCL_Id = c.ASMCL_Id,
                //                           ASMCL_ClassName = c.ASMCL_ClassName,
                //                           ASMS_Id = s.ASMS_Id,
                //                           ASMC_SectionName = s.ASMC_SectionName,
                //                       }
                //              ).Distinct().OrderBy(b => b.studentname).ToArray();
                //}

                #endregion

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;

        }

        public SPCC_Student_House_DTO get_student_info(SPCC_Student_House_DTO dto)
        {
            try
            {


                //foreach (var item in dto.studList1)
                //  {
                //      var list_ofstud = (from a in _context.admissionyearstudent
                //                        from b in _context.admissionStduent
                //                        where  (a.AMST_Id==b.AMST_Id && a.ASMAY_Id==dto.ASMAY_Id && a.ASMCL_Id==dto.ASMCL_Id && a.ASMS_Id==dto.ASMS_Id && a.AMST_Id.Equals(item.amsT_Id) && a.AMAY_ActiveFlag==1 && b.AMST_SOL=="S")
                //                        select new SPCC_Student_House_DTO
                //                        {
                //                            AMST_Id = b.AMST_Id,
                //                            studentname = b.AMST_FirstName + (string.IsNullOrEmpty(b.AMST_MiddleName) || b.AMST_MiddleName == "0" ? "" : ' ' + b.AMST_MiddleName) + (string.IsNullOrEmpty(b.AMST_LastName) || b.AMST_LastName == "0" ? "" : ' ' + b.AMST_LastName),
                //                            AMST_AdmNo=b.AMST_AdmNo,
                //                        }).Distinct().ToList();
                //  }


                foreach (var item in dto.studList1)
                {
                    var list_ofstud = (from a in _context.admissionyearstudent
                                       from b in _context.admissionStduent
                                       where (a.AMST_Id == b.AMST_Id && a.ASMAY_Id == dto.ASMAY_Id && a.ASMCL_Id == dto.ASMCL_Id && a.ASMS_Id == dto.ASMS_Id && a.AMST_Id.Equals(item.AMST_Id) && a.AMAY_ActiveFlag == 1 && b.AMST_SOL == "S")
                                       select new SPCC_Student_House_DTO
                                       {
                                           AMST_Id = b.AMST_Id,
                                           studentname = b.AMST_FirstName + (string.IsNullOrEmpty(b.AMST_MiddleName) || b.AMST_MiddleName == "0" ? "" : ' ' + b.AMST_MiddleName) + (string.IsNullOrEmpty(b.AMST_LastName) || b.AMST_LastName == "0" ? "" : ' ' + b.AMST_LastName),
                                           AMST_AdmNo = b.AMST_AdmNo,
                                       }).Distinct().ToList();
                }


            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;

        }

        public SPCC_Student_House_DTO saveRecord(SPCC_Student_House_DTO obj)
        {
            //List<SPCC_Student_House_DTO> agedata = new List<SPCC_Student_House_DTO>();

            #region old code
            //try
            //{               
            //    if (obj.SPCCSH_Id == 0)
            //    {

            //        for (int i = 0; i < obj.studList1.Count(); i++)
            //        {
            //            var std = obj.studList1[i].amsT_Id;
            //            var duplicate = _context.SportStudentHouseDivisionDMO.Where(t => t.MI_Id == obj.MI_Id && t.AMST_Id == std ).ToList();

            //            if (duplicate.Count > 0)
            //            {
            //                obj.count += 1;
            //                obj.msg = "duplicate";
            //            }

            //            else
            //            {
            //               obj.count1 += 1;
            //                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
            //                {
            //                    cmd.CommandText = "Sports_age_calc";
            //                    cmd.CommandType = CommandType.StoredProcedure;
            //                    cmd.Parameters.Add(new SqlParameter("@mi_id", SqlDbType.Int) { Value = obj.MI_Id });
            //                    cmd.Parameters.Add(new SqlParameter("@amst_id", SqlDbType.Int) { Value = obj.studList1[i].amsT_Id });
            //                    cmd.Parameters.Add(new SqlParameter("@asmcl_id", SqlDbType.Int) { Value = obj.ASMCL_Id });
            //                    cmd.Parameters.Add(new SqlParameter("@asms_id", SqlDbType.Int) { Value = obj.ASMS_Id });
            //                    cmd.Parameters.Add(new SqlParameter("@asmay_id", SqlDbType.Int) { Value = obj.ASMAY_Id });
            //                    cmd.Parameters.Add(new SqlParameter("@spccsH_AsOnDate", SqlDbType.DateTime) { Value = obj.SPCCSH_AsOnDate });
            //                    if (cmd.Connection.State != ConnectionState.Open)
            //                        cmd.Connection.Open();
            //                    var studAge = cmd.ExecuteScalar();

            //                    SportStudentHouseDivisionDMO mapp = new SportStudentHouseDivisionDMO();

            //                    mapp.AMST_Id = obj.studList1[i].amsT_Id;
            //                    mapp.SPCCSH_Height = obj.studList1[i].height;
            //                    mapp.SPCCSH_Weight = obj.studList1[i].weight;
            //                    mapp.SPCCSH_BMI = obj.studList1[i].spccmhD_BMI;
            //                    mapp.SPCCSH_BMIRemarks = obj.studList1[i].spccmhD_BMI_Remark;
            //                    mapp.SPCCMH_Id = obj.SPCCMH_Id;
            //                    mapp.MI_Id = obj.MI_Id;
            //                    mapp.ASMAY_Id = obj.ASMAY_Id;
            //                    mapp.ASMCL_Id = obj.ASMCL_Id;
            //                    mapp.SPCCSH_AsOnDate = obj.SPCCSH_AsOnDate;
            //                    mapp.ASMS_Id = obj.ASMS_Id;
            //                    mapp.SPCCSH_Age = studAge.ToString();
            //                    mapp.SPCCMH_Id = obj.SPCCMH_Id;
            //                    mapp.SPCCMH_ActiveFlag = true;
            //                    mapp.CreatedDate = DateTime.Now;
            //                    mapp.UpdatedDate = DateTime.Now;

            //                    _context.Add(mapp);
            //                }
            //            }
            //        }
            //        if (obj.count1 != 0)
            //        {
            //            int s = _context.SaveChanges();
            //            if (s > 0)
            //            {
            //                obj.msg = "saved";
            //            }
            //            else
            //            {
            //                obj.msg = "savingFailed";
            //            }
            //        }
            //        else
            //        {
            //            obj.msg = "duplicate";
            //        }


            //    }
            //    else if (obj.SPCCSH_Id > 0)
            //    {
            //        for (int i = 0; i < obj.studList1.Count(); i++)
            //        {
            //            var std = obj.studList1[i].amsT_Id;
            //            var duplicate = _context.SportStudentHouseDivisionDMO.Where(d => d.MI_Id == obj.MI_Id && d.SPCCSH_Id!=obj.SPCCSH_Id && d.AMST_Id == std && d.SPCCSH_Height==obj.SPCCSH_Height && d.SPCCSH_Weight==obj.SPCCSH_Weight && d.ASMAY_Id ==obj.ASMAY_Id && d.ASMCL_Id==obj.ASMCL_Id && d.ASMS_Id==obj.ASMS_Id && d.SPCCMH_Id==obj.SPCCMH_Id && d.SPCCSH_AsOnDate==obj.SPCCSH_AsOnDate && d.SPCCSH_BMIRemarks==obj.SPCCSH_BMIRemarks ).ToList();

            //            if (duplicate.Count > 0)
            //            {
            //                obj.count += 1;
            //                obj.msg = "duplicate";
            //            }
            //            else
            //            {
            //                obj.count1 += 1;
            //                var query = _context.SportStudentHouseDivisionDMO.Where(d => d.SPCCSH_Id == obj.SPCCSH_Id && d.MI_Id==obj.MI_Id).ToList();
            //                if (query.Count > 0)
            //                {
            //                    #region
            //                    //using (var cmd = _context.Database.GetDbConnection().CreateCommand())
            //                    //{
            //                    //    cmd.CommandText = "Sports_age_calc";
            //                    //    cmd.CommandType = CommandType.StoredProcedure;
            //                    //    cmd.Parameters.Add(new SqlParameter("@mi_id", SqlDbType.Int) { Value = obj.MI_Id });
            //                    //    cmd.Parameters.Add(new SqlParameter("@amst_id", SqlDbType.Int) { Value = obj.studList1[i].amsT_Id });
            //                    //    cmd.Parameters.Add(new SqlParameter("@asmcl_id", SqlDbType.Int) { Value = obj.ASMCL_Id });
            //                    //    cmd.Parameters.Add(new SqlParameter("@asms_id", SqlDbType.Int) { Value = obj.ASMS_Id });
            //                    //    cmd.Parameters.Add(new SqlParameter("@asmay_id", SqlDbType.Int) { Value = obj.ASMAY_Id });
            //                    //    if (cmd.Connection.State != ConnectionState.Open)
            //                    //        cmd.Connection.Open();
            //                    //    var studAge = cmd.ExecuteScalar();
            //                    //    var result = _context.SportStudentHouseDivisionDMO.Single(d => d.SPCCSH_Id == obj.SPCCSH_Id);

            //                    //    //result.SPCCSH_Id = obj.SPCCSH_Id;
            //                    //    result.SPCCMH_Id = obj.SPCCMH_Id;
            //                    //    result.AMST_Id = obj.studList1[i].amsT_Id;
            //                    //    result.ASMS_Id = obj.ASMS_Id;
            //                    //    result.ASMCL_Id = obj.ASMCL_Id;
            //                    //    result.ASMAY_Id = obj.ASMAY_Id;
            //                    //    result.SPCCSH_Age = studAge.ToString();
            //                    //    result.SPCCSH_Height = obj.studList1[i].height;
            //                    //    result.SPCCSH_Weight = obj.studList1[i].weight;

            //                    //    result.SPCCSH_BMI = obj.studList1[i].spccmhD_BMI;
            //                    //    result.SPCCSH_BMIRemarks = obj.studList1[i].spccmhD_BMI_Remark;

            //                    //    result.UpdatedDate = DateTime.Now;

            //                    //    _context.Update(result);

            //                    //}
            //                    #endregion

            //                    var result = _context.SportStudentHouseDivisionDMO.Single(d => d.SPCCSH_Id == obj.SPCCSH_Id);

            //                    //result.SPCCSH_Id = obj.SPCCSH_Id;
            //                    result.SPCCMH_Id = obj.SPCCMH_Id;
            //                    result.AMST_Id = obj.studList1[i].amsT_Id;
            //                    result.ASMS_Id = obj.ASMS_Id;
            //                    result.ASMCL_Id = obj.ASMCL_Id;
            //                    result.ASMAY_Id = obj.ASMAY_Id;
            //                    //result.SPCCSH_Age = studAge.ToString();
            //                    result.SPCCSH_Height = obj.studList1[i].height;
            //                    result.SPCCSH_Weight = obj.studList1[i].weight;

            //                    result.SPCCSH_BMI = obj.studList1[i].spccmhD_BMI;
            //                    result.SPCCSH_BMIRemarks = obj.studList1[i].spccmhD_BMI_Remark;

            //                    result.UpdatedDate = DateTime.Now;

            //                    _context.Update(result);
            //                }
            //            }
            //        }
            //        if (obj.count1 != 0)
            //        {
            //            int s = _context.SaveChanges();
            //            if (s > 0)
            //            {
            //                obj.msg = "updated";
            //            }
            //            else
            //            {
            //                obj.msg = "updateFailed";
            //            }
            //        }
            //        else
            //        {
            //            obj.msg = "duplicate";
            //        }

            //    }
            //}
            #endregion old code close

            try
            {

                if (obj.SPCCSH_Id == 0)
                {

                    for (int i = 0; i < obj.studList1.Count(); i++)
                    {
                        var std = obj.studList1[i].AMST_Id;
                        var duplicate = _context.SportStudentHouseDivisionDMO.Where(t => t.MI_Id == obj.MI_Id && t.ASMAY_Id == obj.ASMAY_Id && t.AMST_Id == std).ToList();

                        if (duplicate.Count > 0)
                        {
                            obj.count += 1;
                            obj.msg = "duplicate";
                        }

                        else
                        {
                            obj.count1 += 1;
                            using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                            {
                                cmd.CommandText = "Sports_age_calc";
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.Add(new SqlParameter("@mi_id", SqlDbType.Int) { Value = obj.MI_Id });
                                cmd.Parameters.Add(new SqlParameter("@amst_id", SqlDbType.Int) { Value = obj.studList1[i].AMST_Id });
                                cmd.Parameters.Add(new SqlParameter("@asmcl_id", SqlDbType.Int) { Value = obj.ASMCL_Id });
                                cmd.Parameters.Add(new SqlParameter("@asms_id", SqlDbType.Int) { Value = obj.ASMS_Id });
                                cmd.Parameters.Add(new SqlParameter("@asmay_id", SqlDbType.Int) { Value = obj.ASMAY_Id });
                                cmd.Parameters.Add(new SqlParameter("@spccsH_AsOnDate", SqlDbType.DateTime) { Value = obj.SPCCSH_Date });
                                if (cmd.Connection.State != ConnectionState.Open)
                                    cmd.Connection.Open();
                                //var studAge = cmd.ExecuteScalar();
                                //var ageFormat = cmd.ExecuteScalar();
                                var retObject = new List<dynamic>();
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
                                            //agedata.Add(new SPCC_Student_House_DTO
                                            //{
                                            //    SPCCSH_Age= dataReader["studAge"].ToString(),
                                            //    SPCCSH_Age_Format = dataReader["ageFormat"].ToString(),
                                            //});
                                        }
                                        retObject.Add((ExpandoObject)dataRow);
                                    }
                                }
                                // obj.agelist = retObject.ToArray();
                                var datalis = retObject.ToList();

                                //var datalis = obj.agelist;

                                SportStudentHouseDivisionDMO mapp = new SportStudentHouseDivisionDMO();

                                mapp.AMST_Id = obj.studList1[i].AMST_Id;
                                mapp.MI_Id = obj.MI_Id;
                                mapp.ASMAY_Id = obj.ASMAY_Id;
                                mapp.ASMCL_Id = obj.ASMCL_Id;
                                mapp.ASMS_Id = obj.ASMS_Id;
                                mapp.SPCCMH_Id = obj.SPCCMH_Id;
                                mapp.SPCCMH_ActiveFlag = true;
                                mapp.SPCCSH_Remarks = obj.SPCCSH_Remarks;
                                mapp.CreatedDate = DateTime.Now;
                                mapp.UpdatedDate = DateTime.Now;

                                mapp.SPCCSH_Date = obj.SPCCSH_Date;

                                if (datalis.Count > 0)
                                {
                                    mapp.SPCCSH_Age = datalis[0].studAge;
                                    mapp.SPCCSH_Age_Format = datalis[0].ageFormat;
                                }


                                _context.Add(mapp);
                            }
                        }
                    }
                    if (obj.count1 != 0)
                    {
                        int s = _context.SaveChanges();
                        if (s > 0)
                        {
                            obj.msg = "saved";
                        }
                        else
                        {
                            obj.msg = "savingFailed";
                        }
                    }
                    else
                    {
                        obj.msg = "duplicate";
                    }


                }
                else if (obj.SPCCSH_Id > 0)
                {
                    for (int i = 0; i < obj.studList1.Count(); i++)
                    {
                        var std = obj.studList1[i].AMST_Id;
                        var duplicate = _context.SportStudentHouseDivisionDMO.Where(d => d.MI_Id == obj.MI_Id && d.SPCCSH_Id != obj.SPCCSH_Id && d.AMST_Id == std && d.ASMAY_Id == obj.ASMAY_Id && d.ASMCL_Id == obj.ASMCL_Id && d.ASMS_Id == obj.ASMS_Id).ToList();

                        if (duplicate.Count > 0)
                        {
                            obj.count += 1;
                            obj.msg = "duplicate";
                        }
                        else
                        {
                            obj.count1 += 1;

                            using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                            {
                                cmd.CommandText = "Sports_age_calc";
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.Add(new SqlParameter("@mi_id", SqlDbType.Int) { Value = obj.MI_Id });
                                cmd.Parameters.Add(new SqlParameter("@amst_id", SqlDbType.Int) { Value = obj.studList1[i].AMST_Id });
                                cmd.Parameters.Add(new SqlParameter("@asmcl_id", SqlDbType.Int) { Value = obj.ASMCL_Id });
                                cmd.Parameters.Add(new SqlParameter("@asms_id", SqlDbType.Int) { Value = obj.ASMS_Id });
                                cmd.Parameters.Add(new SqlParameter("@asmay_id", SqlDbType.Int) { Value = obj.ASMAY_Id });
                                cmd.Parameters.Add(new SqlParameter("@spccsH_AsOnDate", SqlDbType.DateTime) { Value = obj.SPCCSH_Date });
                                if (cmd.Connection.State != ConnectionState.Open)
                                    cmd.Connection.Open();

                                //var studAge = cmd.ExecuteScalar();
                                //var ageFormat = cmd.ExecuteScalar();


                                var retObject = new List<dynamic>();
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
                                            //agedata.Add(new SPCC_Student_House_DTO
                                            //{
                                            //    SPCCSH_Age= dataReader["studAge"].ToString(),
                                            //    SPCCSH_Age_Format = dataReader["ageFormat"].ToString(),
                                            //});
                                        }
                                        retObject.Add((ExpandoObject)dataRow);
                                    }
                                }
                                // obj.agelist = retObject.ToArray();
                                var datalis = retObject.ToList();

                                //var datalis = obj.agelist;


                                var query = _context.SportStudentHouseDivisionDMO.Where(d => d.SPCCSH_Id == obj.SPCCSH_Id && d.MI_Id == obj.MI_Id).ToList();
                                if (query.Count > 0)
                                {

                                    var result = _context.SportStudentHouseDivisionDMO.Single(d => d.SPCCSH_Id == obj.SPCCSH_Id);

                                    //result.SPCCSH_Id = obj.SPCCSH_Id;
                                    result.SPCCMH_Id = obj.SPCCMH_Id;
                                    result.AMST_Id = obj.studList1[i].AMST_Id;
                                    result.ASMS_Id = obj.ASMS_Id;
                                    result.ASMCL_Id = obj.ASMCL_Id;
                                    result.ASMAY_Id = obj.ASMAY_Id;
                                    result.SPCCSH_Remarks = obj.SPCCSH_Remarks;
                                    result.UpdatedDate = DateTime.Now;

                                    result.SPCCSH_Date = obj.SPCCSH_Date;
                                    if (datalis.Count > 0)
                                    {
                                        result.SPCCSH_Age_Format = datalis[0].ageFormat;
                                        result.SPCCSH_Age = datalis[0].studAge;
                                    }

                                    _context.Update(result);
                                }
                            }
                        }
                    }
                    if (obj.count1 != 0)
                    {
                        int s = _context.SaveChanges();
                        if (s > 0)
                        {
                            obj.msg = "updated";
                        }
                        else
                        {
                            obj.msg = "updateFailed";
                        }
                    }
                    else
                    {
                        obj.msg = "duplicate";
                    }

                }


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return obj;
        }

        public SPCC_Student_House_DTO EditRecord(SPCC_Student_House_DTO data)
        {
            try
            {

                var editData = (from a in _context.SportStudentHouseDivisionDMO
                                from b in _context.admissionyearstudent
                                from c in _context.Adm_M_Student

                                where (a.ASMAY_Id == b.ASMAY_Id && a.ASMCL_Id == b.ASMCL_Id && a.ASMS_Id == b.ASMS_Id && a.AMST_Id == b.AMST_Id && a.AMST_Id == c.AMST_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && a.SPCCSH_Id == data.SPCCSH_Id)
                                select new SPCC_Student_House_DTO
                                {
                                    ASMAY_Id = a.ASMAY_Id,
                                    MI_Id = a.MI_Id,
                                    ASMCL_Id = a.ASMCL_Id,
                                    ASMS_Id = a.ASMS_Id,
                                    AMST_Id = a.AMST_Id,
                                    //SPCCSH_AsOnDate=a.SPCCSH_AsOnDate,
                                    SPCCMH_Id = a.SPCCMH_Id,
                                    SPCCSH_Id = a.SPCCSH_Id,
                                    // SPCCSH_Age =a.SPCCSH_Age,
                                    //SPCCSH_Height=a.SPCCSH_Height,
                                    //SPCCSH_Weight=a.SPCCSH_Weight,
                                    SPCCMH_ActiveFlag = a.SPCCMH_ActiveFlag,
                                    // SPCCSH_BMI=a.SPCCSH_BMI,
                                    //SPCCSH_BMIRemarks=a.SPCCSH_BMIRemarks,
                                    SPCCSH_Remarks = a.SPCCSH_Remarks,
                                    studentname = c.AMST_FirstName + (string.IsNullOrEmpty(c.AMST_MiddleName) || c.AMST_MiddleName == "0" ? "" : ' ' + c.AMST_MiddleName) + (string.IsNullOrEmpty(c.AMST_LastName) || c.AMST_LastName == "0" ? "" : ' ' + c.AMST_LastName),
                                    AMST_AdmNo = c.AMST_AdmNo,
                                    SPCCSH_Date = a.SPCCSH_Date,

                                }).ToList();
                data.editrecord = editData.ToArray();

                data.StudentList = (from a in _context.admissionyearstudent
                                    from c in _context.admissionClass
                                    from s in _context.masterSection
                                    from b in _context.admissionStduent
                                    where (b.MI_Id == c.MI_Id && a.ASMCL_Id == c.ASMCL_Id && a.ASMS_Id == s.ASMS_Id && a.AMST_Id == b.AMST_Id && b.MI_Id == data.MI_Id && a.ASMAY_Id == editData.FirstOrDefault().ASMAY_Id && a.ASMCL_Id == editData.FirstOrDefault().ASMCL_Id && a.ASMS_Id == editData.FirstOrDefault().ASMS_Id && a.AMAY_ActiveFlag == 1 && b.AMST_SOL == "S")
                                    select new SPCC_Student_House_DTO
                                    {
                                        AMST_Id = b.AMST_Id,
                                        studentname = b.AMST_FirstName + (string.IsNullOrEmpty(b.AMST_MiddleName) || b.AMST_MiddleName == "0" ? "" : ' ' + b.AMST_MiddleName) + (string.IsNullOrEmpty(b.AMST_LastName) || b.AMST_LastName == "0" ? "" : ' ' + b.AMST_LastName),
                                        AMST_AdmNo = b.AMST_AdmNo,
                                    }
                              ).Distinct().OrderBy(b => b.studentname).ToArray();


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return data;
        }

        public SPCC_Student_House_DTO deactivate(SPCC_Student_House_DTO dto)
        {
            try
            {

                var result = _context.SportStudentHouseDivisionDMO.Single(t => t.MI_Id == dto.MI_Id && t.SPCCSH_Id == dto.SPCCSH_Id);
                if (result.SPCCMH_ActiveFlag == true)
                {
                    result.SPCCMH_ActiveFlag = false;
                }
                else
                {
                    result.SPCCMH_ActiveFlag = true;
                }
                result.CreatedDate = result.CreatedDate;
                result.UpdatedDate = DateTime.Now;
                _context.Update(result);
                var flag = _context.SaveChanges();
                if (flag == 1)
                {
                    dto.returnVal = true;

                    if (result.SPCCMH_ActiveFlag == true)
                    {
                        dto.msg = "House Mapping Activated Successfully.";
                    }
                    else if (result.SPCCMH_ActiveFlag == false)
                    {
                        dto.msg = "House Mapping Deactivated Successfully.";
                    }
                }
                else
                {
                    dto.returnVal = false;
                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }


    }
}
