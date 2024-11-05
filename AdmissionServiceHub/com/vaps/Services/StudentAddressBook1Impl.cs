using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.admission;
using DomainModel.Model;
using System.Collections.Concurrent;
using Microsoft.EntityFrameworkCore;
using DataAccessMsSqlServerProvider.com.vapstech.admission;
using DomainModel.Model.com.vaps.admission;
using AdmissionServiceHub.com.vaps.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace AdmissionServiceHub.com.vaps.Services
{
    public class StudentAddressBook1Impl : Interfaces.StudentAddressBook1Interface
    {       
        private StudentAddressBook1Context _db;
        public StudentAddressBook1Impl(StudentAddressBook1Context st)
        {
            _db = st;
        }
        //load
        public async Task<StudentAddressBook1DTO> getInitailData(int id)
        {
            StudentAddressBook1DTO ctdo = new StudentAddressBook1DTO();
            try
            {
                List<AcademicYear> allyear = new List<AcademicYear>();
                allyear = await _db.year.Where(d => d.MI_Id == id && d.Is_Active == true).OrderByDescending(d => d.ASMAY_Order).ToListAsync();
                ctdo.accyear = allyear.ToArray();

                List<School_M_Class> allclass = new List<School_M_Class>();
                allclass = await _db.AdmClass.Where(d => d.MI_Id == id && d.ASMCL_ActiveFlag == true).OrderBy(d => d.ASMCL_Order).ToListAsync();
                ctdo.classlist = allclass.ToArray();

                List<School_M_Section> allsection = new List<School_M_Section>();
                allsection = await _db.admsection.Where(d => d.MI_Id == id && d.ASMC_ActiveFlag == 1).OrderBy(d => d.ASMC_Order).ToListAsync();
                ctdo.sectionlist = allsection.ToArray();

            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return ctdo;
        }
        public async Task<StudentAddressBook1DTO> getclass([FromBody] StudentAddressBook1DTO data)
        {
            StudentAddressBook1DTO ctdo = new StudentAddressBook1DTO();

            using (var cmd1 = _db.Database.GetDbConnection().CreateCommand())
            {
                cmd1.CommandText = "Adm_namebinding";
                cmd1.CommandType = CommandType.StoredProcedure;
                cmd1.Parameters.Add(new SqlParameter("@asmaY_Id",SqlDbType.BigInt)
                {
                    Value = Convert.ToInt16(data.ASMAY_Id)
                });

                cmd1.Parameters.Add(new SqlParameter("@asmcL_Id", SqlDbType.BigInt)
                {
                    Value = Convert.ToInt16(data.ASMCL_Id)
                });

                cmd1.Parameters.Add(new SqlParameter("@asmC_Id", SqlDbType.BigInt)
                {
                    Value = Convert.ToInt16(data.ASMC_Id)
                });


                cmd1.Parameters.Add(new SqlParameter("@flag", SqlDbType.Text)
                {
                    Value = Convert.ToString(1)
                });

                if (cmd1.Connection.State != ConnectionState.Open)
                    cmd1.Connection.Open();

                var retObject = new List<dynamic>();
                try
                {
                    using (var dataReader = await cmd1.ExecuteReaderAsync())
                    {
                        while (await dataReader.ReadAsync())
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
                    data.getdetails = retObject.ToArray();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return data;
        }
        public async Task<StudentAddressBook1DTO> getyear([FromBody] StudentAddressBook1DTO data)
        {
            StudentAddressBook1DTO ctdo = new StudentAddressBook1DTO();

            using (var cmd1 = _db.Database.GetDbConnection().CreateCommand())
            {
                cmd1.CommandText = "Adm_namebinding";
                cmd1.CommandType = CommandType.StoredProcedure;
                cmd1.Parameters.Add(new SqlParameter("@asmaY_Id",SqlDbType.BigInt)
                {
                    Value = Convert.ToInt16(data.ASMAY_Id)
                });
                cmd1.Parameters.Add(new SqlParameter("@asmcL_Id", SqlDbType.BigInt)
                {
                    Value = Convert.ToInt16(data.ASMCL_Id)
                });
                cmd1.Parameters.Add(new SqlParameter("@asmC_Id", SqlDbType.BigInt)
                {
                    Value = Convert.ToInt16(data.ASMC_Id)
                });
                cmd1.Parameters.Add(new SqlParameter("@flag", SqlDbType.Text)
                {
                    Value = Convert.ToString(1)
                });
                if (cmd1.Connection.State != ConnectionState.Open)
                    cmd1.Connection.Open();

                var retObject = new List<dynamic>();
                try
                {
                    using (var dataReader = await cmd1.ExecuteReaderAsync())
                    {
                        while (await dataReader.ReadAsync())
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
                    data.getdetails = retObject.ToArray();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return data;
        }

        //section wise
        public async Task<StudentAddressBook1DTO> getsection([FromBody] StudentAddressBook1DTO data)
        {
            StudentAddressBook1DTO ctdo = new StudentAddressBook1DTO();
            using (var cmd1 = _db.Database.GetDbConnection().CreateCommand())
            {
                cmd1.CommandText = "Adm_namebinding";
                cmd1.CommandType = CommandType.StoredProcedure;
                cmd1.Parameters.Add(new SqlParameter("@asmaY_Id",SqlDbType.BigInt)
                {
                    Value = Convert.ToInt16(data.ASMAY_Id)
                });

                cmd1.Parameters.Add(new SqlParameter("@asmcL_Id", SqlDbType.BigInt)
                {
                    Value = Convert.ToInt16(data.ASMCL_Id)
                });

                cmd1.Parameters.Add(new SqlParameter("@asmC_Id", SqlDbType.BigInt)
                {
                    Value = Convert.ToInt16(data.ASMC_Id)
                });
                cmd1.Parameters.Add(new SqlParameter("@flag", SqlDbType.Text)
                {
                    Value = Convert.ToString(1)
                });

                if (cmd1.Connection.State != ConnectionState.Open)
                    cmd1.Connection.Open();

                var retObject = new List<dynamic>();              

                try
                {
                    using (var dataReader = await cmd1.ExecuteReaderAsync())
                    {
                        while (await dataReader.ReadAsync())
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
                    data.getdetails = retObject.ToArray();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return data;
        }
        public async Task<StudentAddressBook1DTO> getdetails([FromBody] StudentAddressBook1DTO data)
        {
            using (var cmd = _db.Database.GetDbConnection().CreateCommand())
            {
                cmd.CommandText = "student_Address_Book";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@asmaY_Id",SqlDbType.BigInt)
                {
                    Value = Convert.ToInt16(data.ASMAY_Id)
                });
                cmd.Parameters.Add(new SqlParameter("@asmcL_Id", SqlDbType.BigInt)
                {
                    Value = Convert.ToInt16(data.ASMCL_Id)
                });
                cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.BigInt)
                {
                    Value = Convert.ToInt16(data.ASMC_Id)
                });
                cmd.Parameters.Add(new SqlParameter("@flag", SqlDbType.Text)
                {
                    Value = Convert.ToString(data.flag)
                });
                cmd.Parameters.Add(new SqlParameter("@amst_id", SqlDbType.BigInt)
                {
                    Value = Convert.ToInt16(data.AMST_Id)
                });
                cmd.Parameters.Add(new SqlParameter("@all1", SqlDbType.Text)
                {
                    Value = Convert.ToString(data.sall)
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
                                dataRow.Add(dataReader.GetName(iFiled),
                                dataReader.IsDBNull(iFiled) ? "Not Available" : dataReader[iFiled] // use null instead of {}
                              );
                            }
                            retObject.Add((ExpandoObject)dataRow);
                        }
                    }
                    data.getdetails = retObject.ToArray();
                    if (data.getdetails.Length > 0)
                    {
                        data.count = data.getdetails.Length;
                    }
                    else
                    {
                        data.count = 0;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return data;
        }
        public async Task<StudentAddressBook1DTO> sectinchange(StudentAddressBook1DTO data)
        {

            var flag = "";
            var aciveflag = 0;
            int amayflag = 0;
            try
            {
                var checkflag = _db.School_M_ClassDMO.Where(a => a.MI_Id == data.MI_id).ToList();
                var checkflag1 = _db.GeneralConfigDMO.Where(a => a.MI_Id == data.MI_id).ToList();

                if (data.flag == "S")
                {
                    flag = "S";
                    aciveflag = 1;
                    amayflag = 1;
                }
                if (data.flag == "L")
                {
                    flag = "L";
                    aciveflag = 0;
                    amayflag = 0;
                }
                if (checkflag1.FirstOrDefault().IVRMGC_AdmNo_RegNo_RollNo_DefaultFlag == "1")
                {
                    data.studentlist = await (from m in _db.student
                                              from n in _db.School_Adm_Y_StudentDMO
                                              where m.AMST_Id == n.AMST_Id && n.ASMAY_Id == data.ASMAY_Id && n.ASMCL_Id == data.ASMCL_Id
                                              && n.ASMS_Id == data.ASMC_Id && m.MI_Id == data.MI_id && m.AMST_SOL == flag && m.AMST_ActiveFlag == aciveflag && n.AMAY_ActiveFlag == amayflag
                                              select new StudentAddressBook1DTO
                                              {
                                                  AMST_Id = m.AMST_Id,
                                                  AMST_FirstName = ((m.AMST_FirstName == null ? " " : m.AMST_FirstName) + (m.AMST_MiddleName == null ? " " : m.AMST_MiddleName) + (m.AMST_LastName == null ? " " : m.AMST_LastName) + ":" + (m.AMST_AdmNo == null ? " " : m.AMST_AdmNo)).Trim(),

                                              }).ToArrayAsync();
                }

                else if (checkflag1.FirstOrDefault().IVRMGC_AdmNo_RegNo_RollNo_DefaultFlag == "2")
                {
                    data.studentlist = await (from m in _db.student
                                              from n in _db.School_Adm_Y_StudentDMO
                                              where m.AMST_Id == n.AMST_Id && n.ASMAY_Id == data.ASMAY_Id && n.ASMCL_Id == data.ASMCL_Id
                                              && n.ASMS_Id == data.ASMC_Id && m.MI_Id == data.MI_id && m.AMST_SOL == flag && m.AMST_ActiveFlag == aciveflag && n.AMAY_ActiveFlag == amayflag
                                              select new StudentAddressBook1DTO
                                              {
                                                  AMST_Id = m.AMST_Id,
                                                  AMST_FirstName = ((m.AMST_RegistrationNo == null ? " " : m.AMST_RegistrationNo) + ':' + (m.AMST_FirstName == null ? " " : m.AMST_FirstName) + (m.AMST_MiddleName == null ? " " : m.AMST_MiddleName) + (m.AMST_LastName == null ? " " : m.AMST_LastName)).Trim(),

                                              }).ToArrayAsync();
                }

                else if (checkflag1.FirstOrDefault().IVRMGC_AdmNo_RegNo_RollNo_DefaultFlag == "3")
                {
                    data.studentlist = await (from m in _db.student
                                              from n in _db.School_Adm_Y_StudentDMO
                                              where m.AMST_Id == n.AMST_Id && n.ASMAY_Id == data.ASMAY_Id && n.ASMCL_Id == data.ASMCL_Id
                                              && n.ASMS_Id == data.ASMC_Id && m.MI_Id == data.MI_id && m.AMST_SOL == flag && m.AMST_ActiveFlag == aciveflag && n.AMAY_ActiveFlag == amayflag
                                              select new StudentAddressBook1DTO
                                              {
                                                  AMST_Id = m.AMST_Id,
                                                  AMST_FirstName = ((m.AMST_AdmNo == null ? " " : m.AMST_AdmNo) + ':' + (m.AMST_FirstName == null ? " " : m.AMST_FirstName) + (m.AMST_MiddleName == null ? " " : m.AMST_MiddleName) + (m.AMST_LastName == null ? " " : m.AMST_LastName)).Trim(),

                                              }).ToArrayAsync();
                }

                else if (checkflag1.FirstOrDefault().IVRMGC_AdmNo_RegNo_RollNo_DefaultFlag == "4")
                {
                    data.studentlist = await (from m in _db.student
                                              from n in _db.School_Adm_Y_StudentDMO
                                              where m.AMST_Id == n.AMST_Id && n.ASMAY_Id == data.ASMAY_Id && n.ASMCL_Id == data.ASMCL_Id
                                              && n.ASMS_Id == data.ASMC_Id && m.MI_Id == data.MI_id && m.AMST_SOL == flag && m.AMST_ActiveFlag == aciveflag && n.AMAY_ActiveFlag == amayflag
                                              select new StudentAddressBook1DTO
                                              {
                                                  AMST_Id = m.AMST_Id,
                                                  AMST_FirstName = ((m.AMST_FirstName == null ? " " : m.AMST_FirstName) + (m.AMST_MiddleName == null ? " " : m.AMST_MiddleName) + (m.AMST_LastName == null ? " " : m.AMST_LastName) + ':' + (m.AMST_RegistrationNo == null ? " " : m.AMST_RegistrationNo)).Trim(),

                                              }).ToArrayAsync();
                }
                else if (checkflag1.FirstOrDefault().IVRMGC_AdmNo_RegNo_RollNo_DefaultFlag == "6")
                {
                    data.studentlist = await (from m in _db.student
                                              from n in _db.School_Adm_Y_StudentDMO
                                              where m.AMST_Id == n.AMST_Id && n.ASMAY_Id == data.ASMAY_Id && n.ASMCL_Id == data.ASMCL_Id
                                              && n.ASMS_Id == data.ASMC_Id && m.MI_Id == data.MI_id && m.AMST_SOL == flag && m.AMST_ActiveFlag == aciveflag && n.AMAY_ActiveFlag == amayflag
                                              select new StudentAddressBook1DTO
                                              {
                                                  AMST_Id = m.AMST_Id,
                                                  AMST_FirstName = ((n.AMAY_RollNo.ToString() == null ? " " : n.AMAY_RollNo.ToString()) + ':' + (m.AMST_FirstName == null ? " " : m.AMST_FirstName) + (m.AMST_MiddleName == null ? " " : m.AMST_MiddleName) + (m.AMST_LastName == null ? " " : m.AMST_LastName)).Trim(),

                                              }).ToArrayAsync();
                }

                else if (checkflag1.FirstOrDefault().IVRMGC_AdmNo_RegNo_RollNo_DefaultFlag == "5")
                {
                    data.studentlist = await (from m in _db.student
                                              from n in _db.School_Adm_Y_StudentDMO
                                              where m.AMST_Id == n.AMST_Id && n.ASMAY_Id == data.ASMAY_Id && n.ASMCL_Id == data.ASMCL_Id
                                              && n.ASMS_Id == data.ASMC_Id && m.MI_Id == data.MI_id && m.AMST_SOL == flag && m.AMST_ActiveFlag == aciveflag && n.AMAY_ActiveFlag == amayflag
                                              select new StudentAddressBook1DTO
                                              {
                                                  AMST_Id = m.AMST_Id,
                                                  AMST_FirstName = ((m.AMST_FirstName == null ? " " : m.AMST_FirstName) + (m.AMST_MiddleName == null ? " " : m.AMST_MiddleName) + (m.AMST_LastName == null ? " " : m.AMST_LastName) + ':' + (n.AMAY_RollNo.ToString() == null ? " " : n.AMAY_RollNo.ToString())).Trim(),

                                              }).ToArrayAsync();
                }

                else
                {
                    data.studentlist = await (from m in _db.student
                                              from n in _db.School_Adm_Y_StudentDMO
                                              where m.AMST_Id == n.AMST_Id && n.ASMAY_Id == data.ASMAY_Id && n.ASMCL_Id == data.ASMCL_Id
                                              && n.ASMS_Id == data.ASMC_Id && m.MI_Id == data.MI_id && m.AMST_SOL == flag && m.AMST_ActiveFlag == aciveflag && n.AMAY_ActiveFlag == amayflag
                                              select new StudentAddressBook1DTO
                                              {
                                                  AMST_Id = m.AMST_Id,
                                                  AMST_FirstName = ((m.AMST_FirstName == null ? " " : m.AMST_FirstName) + (m.AMST_MiddleName == null ? " " : m.AMST_MiddleName) + (m.AMST_LastName == null ? " " : m.AMST_LastName) + ':' + (m.AMST_RegistrationNo == null ? " " : m.AMST_RegistrationNo)).Trim(),
                                              }).ToArrayAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public async Task<StudentAddressBook1DTO> yearchange(StudentAddressBook1DTO data)
        {
            var flag = "";
            var aciveflag = 0;
            int amayflag = 0;
            try
            {
                if (data.flag == "S")
                {
                    flag = "S";
                    aciveflag = 1;
                    amayflag = 1;
                }
                if (data.flag == "L")
                {
                    flag = "L";
                    aciveflag = 0;
                    amayflag = 0;
                }
                data.studentlist = await (from m in _db.student
                                          from n in _db.School_Adm_Y_StudentDMO
                                          where m.AMST_Id == n.AMST_Id && n.ASMAY_Id == data.ASMAY_Id && m.MI_Id == data.MI_id && m.AMST_SOL == flag && m.AMST_ActiveFlag == aciveflag && n.AMAY_ActiveFlag == amayflag
                                          select new StudentAddressBook1DTO
                                          {
                                              AMST_Id = m.AMST_Id,
                                              AMST_FirstName = ((m.AMST_FirstName == null ? " " : m.AMST_FirstName) + (m.AMST_MiddleName == null ? " " : m.AMST_MiddleName) + (m.AMST_LastName == null ? " " : m.AMST_LastName) + ":" + (m.AMST_AdmNo == null ? " " : m.AMST_AdmNo)).Trim(),

                                          }).ToArrayAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
    }
}