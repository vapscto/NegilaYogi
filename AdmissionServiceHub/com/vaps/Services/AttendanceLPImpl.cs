using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.admission;
using DomainModel.Model;
using DomainModel.Model.com.vaps.admission;
using DataAccessMsSqlServerProvider;
using System.Collections.Concurrent;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Dynamic;
using AutoMapper;
using System.Data.SqlClient;

namespace AdmissionServiceHub.com.vaps.Services
{
    public class AttendanceLPImpl : Interfaces.AttendanceLPInterface
    {
        private static ConcurrentDictionary<string, AttendanceLPDTO> _login =
             new ConcurrentDictionary<string, AttendanceLPDTO>();
        // create context obj
        private DomainModelMsSqlServerContext _db;
        private ApplicationDBContext _AppDB;
     

        // default contructor
        public AttendanceLPImpl() { }

        // parameterized constructor
        public AttendanceLPImpl(DomainModelMsSqlServerContext db, ApplicationDBContext AppDB)
        {
            _db = db;
            _AppDB = AppDB;
        }

        public AttendanceLPDTO GetInitialData(long MIID)
        {
            AttendanceLPDTO attdto = new AttendanceLPDTO();
            try
            {
                List<AttendanceLPDTO> teachers = new List<AttendanceLPDTO>();
                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "getdata";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.Int)
                    {
                        Value = Convert.ToInt32(MIID)
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
                                teachers.Add(new AttendanceLPDTO
                                {
                                    UserId = Convert.ToInt32(dataReader["Id"]),
                                    UserName = Convert.ToString(dataReader["UserName"])
                                });
                                attdto.teacherList = teachers.ToArray();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }


                attdto.accyear = _db.AcademicYear.Where(a => a.MI_Id == MIID && a.Is_Active == true).OrderByDescending(a => a.ASMAY_Order).ToArray();



            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return attdto;
        }
        public AttendanceLPDTO GetEditData(AttendanceLPDTO attdto)
        {
            try
            {
                var attendanceDTOData = (from at in _db.Adm_SchAttLoginUser
                                         where (at.ASALU_Id == attdto.ASALU_Id)
                                         select new AttendanceLPDTO { ASALU_EntryTypeFlag = at.ASALU_EntryTypeFlag, IVRMUL_Id = Convert.ToInt32(at.HRME_Id), MI_Id = at.MI_Id }
                                            );
                attdto.ASALU_EntryTypeFlag = attendanceDTOData.First().ASALU_EntryTypeFlag;
                attdto.IVRMUL_Id = attendanceDTOData.First().IVRMUL_Id;
                attdto.MI_Id = attendanceDTOData.FirstOrDefault().MI_Id;
                if (attdto.ASALU_EntryTypeFlag == 1)
                {
                    var ASALUCS_Id = _db.Adm_schAttLoginUserClassSubjects.Where(d => d.ASALUCS_Id == attdto.ASALUCS_Id).ToList();
                    var subject = _AppDB.Subject.Where(s => s.ISMS_Id == ASALUCS_Id.FirstOrDefault().ISMS_Id).ToList();
                    attdto.subjectList = subject.ToArray();
                }
                List<AttendanceLPDTO> teachers = new List<AttendanceLPDTO>();
                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "getEditdata";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                  SqlDbType.Int)
                    {
                        Value = Convert.ToInt32(attdto.MI_Id)
                    });
                    cmd.Parameters.Add(new SqlParameter("@IVRMSTAUL_Id",
                 SqlDbType.Int)
                    {
                        Value = Convert.ToInt32(attdto.IVRMUL_Id)
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
                                teachers.Add(new AttendanceLPDTO
                                {
                                    UserId = Convert.ToInt32(dataReader["Id"]),
                                    UserName = Convert.ToString(dataReader["UserName"])
                                });
                                attdto.teacherList = teachers.ToArray();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }
                if (attdto.ASALU_Id != 0 && attdto.ASALUC_Id != 0 && attdto.ASALUCS_Id == 0)
                {
                    //    var attendanceDTOData = ( from at in _db.Adm_SchAttLoginUser
                    //                              where (at.ASALU_Id == attdto.ASALU_Id)
                    //                              select new AttendanceLPDTO { ASALU_EntryTypeFlag = at.ASALU_EntryTypeFlag, IVRMUL_Id = at.IVRMUL_Id }
                    //                         );

                    //attdto.ASALU_EntryTypeFlag = attendanceDTOData.First().ASALU_EntryTypeFlag;
                    //attdto.IVRMUL_Id = attendanceDTOData.First().IVRMUL_Id;

                    List<AttendanceLPDTO> classsection = new List<AttendanceLPDTO>();
                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "getClassSectionEditdata";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@ASALU_Id", SqlDbType.Int)
                        {
                            Value = Convert.ToInt32(attdto.ASALU_Id)
                        });
                        cmd.Parameters.Add(new SqlParameter("@ASALUC_Id", SqlDbType.Int)
                        {
                            Value = Convert.ToInt32(attdto.ASALUC_Id)
                        });
                        cmd.Parameters.Add(new SqlParameter("@ASALUCS_Id", SqlDbType.Int)
                        {
                            Value = Convert.ToInt32(attdto.ASALUCS_Id)
                        });
                        cmd.Parameters.Add(new SqlParameter("@type", SqlDbType.Int)
                        {
                            Value = Convert.ToInt32(0)
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
                                    classsection.Add(new AttendanceLPDTO
                                    {
                                        ASALUC_Id = Convert.ToInt64(dataReader["ASALUC_Id"]),
                                        ASALU_Id = Convert.ToInt64(dataReader["ASALU_Id"]),
                                        name = Convert.ToString(dataReader["name"]),
                                        ASMCL_Id = Convert.ToInt64(dataReader["ASMCL_Id"]),
                                        ASMC_Id = Convert.ToInt64(dataReader["ASMC_Id"]),
                                        classsection = Convert.ToString(dataReader["classsection"]),
                                        ASMAY_Id = Convert.ToInt64(dataReader["ASMAY_Id"]),
                                    });
                                    attdto.resultclasssectionData = classsection.ToArray();
                                }
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
                    List<AttendanceLPDTO> classsection = new List<AttendanceLPDTO>();
                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "getClassSectionEditdata";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@ASALU_Id", SqlDbType.Int)
                        {
                            Value = Convert.ToInt32(attdto.ASALU_Id)
                        });
                        cmd.Parameters.Add(new SqlParameter("@ASALUC_Id", SqlDbType.Int)
                        {
                            Value = Convert.ToInt32(attdto.ASALUC_Id)
                        });
                        cmd.Parameters.Add(new SqlParameter("@ASALUCS_Id", SqlDbType.Int)
                        {
                            Value = Convert.ToInt32(attdto.ASALUCS_Id)
                        });
                        cmd.Parameters.Add(new SqlParameter("@type", SqlDbType.Int)
                        {
                            Value = Convert.ToInt32(1)
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
                                    classsection.Add(new AttendanceLPDTO
                                    {
                                        ASALUC_Id = Convert.ToInt64(dataReader["ASALUC_Id"]),
                                        ASALU_Id = Convert.ToInt64(dataReader["ASALU_Id"]),
                                        name = Convert.ToString(dataReader["name"]),
                                        ASMCL_Id = Convert.ToInt64(dataReader["ASMCL_Id"]),
                                        ASMC_Id = Convert.ToInt64(dataReader["ASMC_Id"]),
                                        classsection = Convert.ToString(dataReader["classsection"]),
                                        ASMAY_Id = Convert.ToInt64(dataReader["ASMAY_Id"]),
                                    });
                                    attdto.resultclasssectionData = classsection.ToArray();
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
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return attdto;
        }
        public AttendanceLPDTO SaveData_working(AttendanceLPDTO adtto)
        {
            try
            {

                if (adtto.classsectionList1 != null && adtto.classsectionList1.Count() > 0)
                {
                    if (adtto.ASALU_EntryTypeFlag == 2 || adtto.ASALU_EntryTypeFlag == 3)
                    {
                        List<AttendanceLPDTO> lnn = new List<AttendanceLPDTO>();
                        for (int i = 0; i < adtto.classsectionList1.Count(); i++)
                        {
                            using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                            {
                                cmd.CommandText = "Att_Log_Priv_Check_Duplicate";
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.Add(new SqlParameter("@ASALU_EntryTypeFlag",
                                SqlDbType.Int)
                                {
                                    Value = Convert.ToInt32(adtto.ASALU_EntryTypeFlag)
                                });
                                cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                               SqlDbType.Int)
                                {
                                    Value = Convert.ToInt32(adtto.ASMAY_Id)
                                });
                                cmd.Parameters.Add(new SqlParameter("@IVRMUL_Id",
                               SqlDbType.Int)
                                {
                                    Value = Convert.ToInt32(adtto.HRME_Id)
                                });
                                cmd.Parameters.Add(new SqlParameter("@MI_Id",
                              SqlDbType.Int)
                                {
                                    Value = Convert.ToInt32(adtto.MI_Id)
                                });
                                cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                              SqlDbType.Int)
                                {
                                    Value = Convert.ToInt32(adtto.classsectionList1[i].asmcL_Id)
                                });
                                cmd.Parameters.Add(new SqlParameter("@ASMC_Id",
                              SqlDbType.Int)
                                {
                                    Value = Convert.ToInt32(adtto.classsectionList1[i].asmC_Id)
                                });
                                cmd.Parameters.Add(new SqlParameter("@PAMS_Id",
                                    SqlDbType.Int)
                                {
                                    Value = Convert.ToInt32(0)
                                });
                                cmd.Parameters.Add(new SqlParameter("@Type",
                             SqlDbType.Int)
                                {
                                    Value = Convert.ToInt32(0)
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
                                            lnn.Add(new AttendanceLPDTO
                                            {
                                                ASALUCS_Id = Convert.ToInt64(dataReader["ASALU_Id"]),
                                                ASALUC_Id = Convert.ToInt64(dataReader["ASALUC_Id"]),
                                                ASMCL_Id = Convert.ToInt64(dataReader["ASMCL_Id"]),
                                                ASMC_Id = Convert.ToInt64(dataReader["ASMS_Id"]),
                                            });
                                            adtto.duplicateList = lnn.ToArray();
                                        }
                                    }
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine(e.Message);
                                }
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < adtto.classsectionList1.Count(); i++)
                        {
                            if (adtto.subjectsList != null && adtto.subjectsList.Count() > 0)
                            {
                                List<AttendanceLPDTO> lnn1 = new List<AttendanceLPDTO>();
                                for (int j = 0; j < adtto.subjectsList.Count(); j++)
                                {
                                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                                    {
                                        cmd.CommandText = "Att_Log_Priv_Check_Duplicate";
                                        cmd.CommandType = CommandType.StoredProcedure;
                                        cmd.Parameters.Add(new SqlParameter("@ASALU_EntryTypeFlag",
                                        SqlDbType.Int)
                                        {
                                            Value = Convert.ToInt32(adtto.ASALU_EntryTypeFlag)
                                        });
                                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                                       SqlDbType.Int)
                                        {
                                            Value = Convert.ToInt32(adtto.ASMAY_Id)
                                        });
                                        cmd.Parameters.Add(new SqlParameter("@IVRMUL_Id",
                                       SqlDbType.Int)
                                        {
                                            Value = Convert.ToInt32(adtto.HRME_Id)
                                        });
                                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                                      SqlDbType.Int)
                                        {
                                            Value = Convert.ToInt32(adtto.MI_Id)
                                        });
                                        cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                                      SqlDbType.Int)
                                        {
                                            Value = Convert.ToInt32(adtto.classsectionList1[i].asmcL_Id)
                                        });
                                        cmd.Parameters.Add(new SqlParameter("@ASMC_Id",
                                      SqlDbType.Int)
                                        {
                                            Value = Convert.ToInt32(adtto.classsectionList1[i].asmC_Id)
                                        });
                                        cmd.Parameters.Add(new SqlParameter("@PAMS_Id",
                                     SqlDbType.Int)
                                        {
                                            Value = Convert.ToInt32(adtto.subjectsList[j].ISMS_Id)
                                        });
                                        cmd.Parameters.Add(new SqlParameter("@Type",
                                     SqlDbType.Int)
                                        {
                                            Value = Convert.ToInt32(1)
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
                                                    lnn1.Add(new AttendanceLPDTO
                                                    {
                                                        ASALU_Id = Convert.ToInt64(dataReader["ASALU_Id"]),
                                                        ASALUC_Id = Convert.ToInt64(dataReader["ASALUC_Id"]),
                                                        ASALUCS_Id = Convert.ToInt64(dataReader["ASALUCS_Id"]),
                                                        ASMCL_Id = Convert.ToInt64(dataReader["ASMCL_Id"]),
                                                        ASMC_Id = Convert.ToInt64(dataReader["ASMS_Id"]),
                                                        PAMS_Id = Convert.ToInt64(dataReader["ISMS_Id"]),
                                                    });
                                                    adtto.duplicateList = lnn1.ToArray();
                                                }
                                            }
                                        }
                                        catch (Exception e)
                                        {
                                            Console.WriteLine(e.Message);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                if (adtto.ASALU_Id == 0)
                {
                    using (var transaction = _db.Database.BeginTransaction())
                    {
                        try
                        {
                            //checking the employee id is there or not in login 
                            Adm_SchoolAttendanceLoginUser enq = Mapper.Map<Adm_SchoolAttendanceLoginUser>(adtto);
                            var emp_check = (from a in _db.Adm_SchAttLoginUser
                                             where (a.MI_Id == adtto.MI_Id && a.ASMAY_Id == adtto.ASMAY_Id && a.HRME_Id == adtto.HRME_Id && a.ASALU_EntryTypeFlag == adtto.ASALU_EntryTypeFlag)
                                             select new AttendanceLPDTO
                                             {
                                                 ASALU_Id = a.ASALU_Id
                                             }
                                           ).ToList();
                            if (emp_check.Count == 0)
                            {

                                if (adtto.MI_Id > 0 && adtto.ASMAY_Id > 0)
                                {
                                    enq.CreatedDate = DateTime.Now;
                                    enq.UpdatedDate = DateTime.Now;
                                    _db.Add(enq);
                                    _db.SaveChanges();
                                }
                            }
                            else
                            {
                                enq.ASALU_Id = emp_check.FirstOrDefault().ASALU_Id;
                            }


                            if (enq.ASALU_Id > 0)
                            {

                                if (adtto.classsectionList1 != null && adtto.classsectionList1.Count() > 0)
                                {
                                    if (adtto.ASALU_EntryTypeFlag == 2 || adtto.ASALU_EntryTypeFlag == 3)
                                    {
                                        for (int i = 0; i < adtto.classsectionList1.Count(); i++)
                                        {
                                            if (adtto.duplicateList != null)
                                            {
                                                adtto.CheckIsDuplicate = adtto.duplicateList.Where(d => d.ASMCL_Id == adtto.classsectionList1[i].asmcL_Id && d.ASMC_Id == adtto.classsectionList1[i].asmC_Id).ToArray();
                                            }

                                            //var checkduplicate = (from m in _db.Adm_SchAttLoginUser
                                            //                      from n in _db.Adm_SchAttLoginUserClass
                                            //                      where m.ASALU_Id == n.ASALU_Id  && m.ASALU_EntryTypeFlag == adtto.ASALU_EntryTypeFlag
                                            //                      && m.ASMAY_Id == adtto.ASMAY_Id && m.IVRMUL_Id == adtto.IVRMUL_Id && m.MI_Id == adtto.MI_Id
                                            //                      && n.ASMCL_Id == adtto.classsectionList1[i].asmcL_Id && n.ASMC_Id == adtto.classsectionList1[i].asmC_Id
                                            //                      select new { m, n}).ToList();
                                            if (adtto.CheckIsDuplicate == null || adtto.CheckIsDuplicate.Length == 0)
                                            {
                                                Adm_SchoolAttendanceLoginUserClass enq222 = Mapper.Map<Adm_SchoolAttendanceLoginUserClass>(adtto.classsectionList1[i]);
                                                enq222.ASALU_Id = enq.ASALU_Id;
                                                enq222.ASMCL_Id = adtto.classsectionList1[i].asmcL_Id;
                                                enq222.ASMS_Id = adtto.classsectionList1[i].asmC_Id;
                                                enq222.CreatedDate = DateTime.Now;
                                                enq222.UpdatedDate = DateTime.Now;
                                                _db.Add(enq222);
                                                var flag = _db.SaveChanges();
                                                if (flag > 0)
                                                {
                                                    adtto.returnval = true;
                                                    adtto.operation = "saved";
                                                    adtto.count = 1;
                                                }
                                                else
                                                {
                                                    adtto.returnval = false;
                                                }
                                            }
                                            else
                                            {
                                                adtto.message = "Record Already Exists";
                                                //adtto.message = "Failed List" + adtto.classsectionList1[i];
                                            }
                                        }
                                        if (adtto.count > 0)
                                        {
                                            transaction.Commit();
                                        }
                                    }

                                    else
                                    {
                                        for (int i = 0; i < adtto.classsectionList1.Count(); i++)
                                        {
                                            Adm_SchoolAttendanceLoginUserClass enq2 = Mapper.Map<Adm_SchoolAttendanceLoginUserClass>(adtto.classsectionList1[i]);
                                            enq2.ASALU_Id = enq.ASALU_Id;
                                            enq2.ASMCL_Id = adtto.classsectionList1[i].asmcL_Id;
                                            enq2.ASMS_Id = adtto.classsectionList1[i].asmC_Id;
                                            enq2.CreatedDate = DateTime.Now;
                                            enq2.UpdatedDate = DateTime.Now;
                                            _db.Add(enq2);
                                            _db.SaveChanges();

                                            if (enq2.ASALUC_Id > 0)
                                            {
                                                if (adtto.subjectsList != null && adtto.subjectsList.Count() > 0)
                                                {
                                                    for (int j = 0; j < adtto.subjectsList.Count(); j++)
                                                    {
                                                        if (adtto.duplicateList != null)
                                                        {
                                                            adtto.CheckIsDuplicate = adtto.duplicateList.Where(d => d.ASMCL_Id == adtto.classsectionList1[i].asmcL_Id && d.ASMC_Id == adtto.classsectionList1[i].asmC_Id && d.PAMS_Id == adtto.subjectsList[j].ISMS_Id).ToArray();
                                                        }
                                                        //var checkduplicate = (from m in _db.Adm_SchAttLoginUser
                                                        //                      from n in _db.Adm_SchAttLoginUserClass
                                                        //                      from o in _db.Adm_schAttLoginUserClassSubjects
                                                        //                      where m.ASALU_Id == n.ASALU_Id && n.ASALUC_Id == o.ASALUC_Id && m.ASALU_EntryTypeFlag == adtto.ASALU_EntryTypeFlag
                                                        //                      && m.ASMAY_Id == adtto.ASMAY_Id && m.IVRMUL_Id == adtto.IVRMUL_Id && m.MI_Id == adtto.MI_Id
                                                        //                      && n.ASMCL_Id == adtto.classsectionList1[i].asmcL_Id && n.ASMC_Id == adtto.classsectionList1[i].asmC_Id
                                                        //                      && o.PAMS_Id == adtto.subjectsList[j].PAMS_Id
                                                        //                      select new { m, n, o }).ToList();
                                                        if (adtto.CheckIsDuplicate == null || adtto.CheckIsDuplicate.Length == 0)
                                                        {
                                                            Adm_SchoolAttendanceLoginUserClassSubject enq3 = Mapper.Map<Adm_SchoolAttendanceLoginUserClassSubject>(adtto.subjectsList[j]);
                                                            enq3.ASALUC_Id = enq2.ASALUC_Id;
                                                            enq3.ISMS_Id = adtto.subjectsList[j].ISMS_Id;
                                                            enq3.CreatedDate = DateTime.Now;
                                                            enq3.UpdatedDate = DateTime.Now;
                                                            _db.Add(enq3);
                                                            var flag = _db.SaveChanges();
                                                            if (flag > 0)
                                                            {
                                                                adtto.returnval = true;
                                                                adtto.operation = "saved";
                                                                adtto.count = 1;
                                                            }
                                                            else
                                                            {
                                                                adtto.returnval = false;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            //  adtto.message = "Some duplicate record exist";
                                                            adtto.message = " Record Already Exists";


                                                        }
                                                    }

                                                }
                                            }
                                        }
                                        if (adtto.count > 0)
                                        {
                                            transaction.Commit();
                                        }
                                    }
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                            transaction.Rollback();
                        }
                    }
                }

                else
                {
                    using (var transaction = _db.Database.BeginTransaction())
                    {
                        try
                        {
                            if (adtto.ASALU_EntryTypeFlag == 1)
                            {
                                Adm_SchoolAttendanceLoginUser enqq = Mapper.Map<Adm_SchoolAttendanceLoginUser>(adtto);
                                var result11 = _db.Adm_SchAttLoginUser.Where(t => t.ASALU_Id == adtto.ASALU_Id && t.HRME_Id == adtto.HRME_Id).ToList();
                                if (result11.Count > 0)
                                {
                                    var result111 = _db.Adm_SchAttLoginUser.Single(t => t.ASALU_Id == adtto.ASALU_Id);
                                    result111.UpdatedDate = DateTime.Now;
                                    result111.CreatedDate = result111.CreatedDate;
                                    result111.HRME_Id = adtto.HRME_Id;
                                    _db.Update(result111);
                                    _db.SaveChanges();
                                    enqq.ASALU_Id = result111.ASALU_Id;
                                }
                                else
                                {
                                    Adm_SchoolAttendanceLoginUser user = new Adm_SchoolAttendanceLoginUser();
                                    user.ASALU_Att_Exam_Flag = adtto.ASALU_Att_Exam_Flag;
                                    user.ASALU_EntryTypeFlag = adtto.ASALU_EntryTypeFlag;
                                    user.ASMAY_Id = adtto.ASMAY_Id;
                                    user.HRME_Id = adtto.HRME_Id;
                                    user.MI_Id = adtto.MI_Id;
                                    user.CreatedDate = DateTime.Now;
                                    user.UpdatedDate = DateTime.Now;
                                    _db.Add(user);
                                    _db.SaveChanges();
                                    enqq.ASALU_Id = user.ASALU_Id;
                                }
                                if (enqq.ASALU_Id > 0)
                                {
                                    if (adtto.classsectionList1 != null && adtto.classsectionList1.Count() > 0)
                                    {
                                        for (int i = 0; i < adtto.classsectionList1.Count(); i++)
                                        {
                                            Adm_SchoolAttendanceLoginUserClass enq22 = Mapper.Map<Adm_SchoolAttendanceLoginUserClass>(adtto.classsectionList1[i]);


                                            var result = _db.Adm_SchAttLoginUserClass.Single(t => t.ASALUC_Id == adtto.ASALUC_Id);
                                            result.ASALU_Id = enqq.ASALU_Id;
                                            result.ASMCL_Id = adtto.classsectionList1[i].asmcL_Id;
                                            result.ASMS_Id = adtto.classsectionList1[i].asmC_Id;
                                            result.UpdatedDate = DateTime.Now;
                                            result.CreatedDate = result.CreatedDate;
                                            _db.Update(result);
                                            _db.SaveChanges();

                                            if (adtto.subjectsList != null && adtto.subjectsList.Count() > 0)
                                            {
                                                if (adtto.ASALUC_Id > 0)
                                                {
                                                    for (int j = 0; j < adtto.subjectsList.Count(); j++)
                                                    {



                                                        Adm_SchoolAttendanceLoginUserClassSubject enq33 = Mapper.Map<Adm_SchoolAttendanceLoginUserClassSubject>(adtto.subjectsList[j]);
                                                        //var checkduplicate = (from m in _db.Adm_SchAttLoginUser
                                                        //                      join n in _db.Adm_SchAttLoginUserClass on m.ASALU_Id equals n.ASALU_Id
                                                        //                      join o in _db.Adm_schAttLoginUserClassSubjects on n.ASALUC_Id equals o.ASALUC_Id
                                                        //                      where m.ASALU_EntryTypeFlag == adtto.ASALU_EntryTypeFlag
                                                        //                      && m.ASMAY_Id == adtto.ASMAY_Id && m.IVRMUL_Id == adtto.IVRMUL_Id && m.MI_Id == adtto.MI_Id
                                                        //                      && n.ASMCL_Id == adtto.classsectionList1[i].asmcL_Id && n.ASMC_Id == adtto.classsectionList1[i].asmC_Id
                                                        //                      && o.PAMS_Id == adtto.subjectsList[j].PAMS_Id
                                                        //                      select o.ASALUCS_Id).ToList();
                                                        if (adtto.duplicateList != null)
                                                        {
                                                            adtto.CheckIsDuplicate = adtto.duplicateList.Where(d => d.ASMCL_Id == adtto.classsectionList1[i].asmcL_Id && d.ASMC_Id == adtto.classsectionList1[i].asmC_Id && d.PAMS_Id == adtto.subjectsList[j].ISMS_Id).ToArray();
                                                        }

                                                        if (adtto.CheckIsDuplicate == null)
                                                        {
                                                            var result1 = _db.Adm_schAttLoginUserClassSubjects.Single(t => t.ASALUCS_Id == adtto.ASALUCS_Id);
                                                            result1.ASALUC_Id = adtto.ASALUC_Id;
                                                            result1.ISMS_Id = adtto.subjectsList[j].ISMS_Id;
                                                            result1.UpdatedDate = DateTime.Now;
                                                            result1.CreatedDate = result1.CreatedDate;
                                                            _db.Update(result1);
                                                            var flag1 = _db.SaveChanges();
                                                            if (flag1 > 0)
                                                            {
                                                                adtto.returnval = true;
                                                                adtto.operation = "updated";
                                                                transaction.Commit();
                                                            }
                                                            else
                                                            {
                                                                adtto.returnval = false;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            //adtto.message = "duplicate record exist ";
                                                            adtto.message = " Record Already Exists";
                                                        }
                                                    }
                                                }
                                            }
                                        }

                                    }
                                }
                            }


                            else if (adtto.ASALU_EntryTypeFlag == 2 || adtto.ASALU_EntryTypeFlag == 3)
                            {
                                Adm_SchoolAttendanceLoginUser enqq = Mapper.Map<Adm_SchoolAttendanceLoginUser>(adtto);
                                var result22 = _db.Adm_SchAttLoginUser.Where(t => t.ASALU_Id == adtto.ASALU_Id && t.HRME_Id == adtto.HRME_Id).ToList();
                                if (result22.Count > 0)
                                {
                                    var result222 = _db.Adm_SchAttLoginUser.Single(t => t.ASALU_Id == adtto.ASALU_Id);
                                    result222.UpdatedDate = DateTime.Now;
                                    result222.CreatedDate = result222.CreatedDate;
                                    result222.HRME_Id = adtto.HRME_Id;
                                    _db.Update(result222);
                                    _db.SaveChanges();
                                    enqq.ASALU_Id = result222.ASALU_Id;
                                }
                                else
                                {
                                    Adm_SchoolAttendanceLoginUser user = new Adm_SchoolAttendanceLoginUser();
                                    user.ASALU_Att_Exam_Flag = adtto.ASALU_Att_Exam_Flag;
                                    user.ASALU_EntryTypeFlag = adtto.ASALU_EntryTypeFlag;
                                    user.ASMAY_Id = adtto.ASMAY_Id;
                                    user.HRME_Id = adtto.HRME_Id;
                                    user.MI_Id = adtto.MI_Id;
                                    user.CreatedDate = DateTime.Now;
                                    user.UpdatedDate = DateTime.Now;
                                    _db.Add(user);
                                    _db.SaveChanges();
                                    enqq.ASALU_Id = user.ASALU_Id;
                                }
                                if (enqq.ASALU_Id > 0)
                                {
                                    if (adtto.classsectionList1 != null && adtto.classsectionList1.Count() > 0)
                                    {
                                        for (int i = 0; i < adtto.classsectionList1.Count(); i++)
                                        {

                                            Adm_SchoolAttendanceLoginUserClass enq22 = Mapper.Map<Adm_SchoolAttendanceLoginUserClass>(adtto.classsectionList1[i]);
                                            //var checkduplicate = (from m in _db.Adm_SchAttLoginUser
                                            //                      from n in _db.Adm_SchAttLoginUserClass
                                            //                      where m.ASALU_Id == n.ASALU_Id  && m.ASALU_EntryTypeFlag == adtto.ASALU_EntryTypeFlag
                                            //                      && m.ASMAY_Id == adtto.ASMAY_Id && m.IVRMUL_Id == adtto.IVRMUL_Id && m.MI_Id == adtto.MI_Id
                                            //                      && n.ASMCL_Id == adtto.classsectionList1[i].asmcL_Id && n.ASMC_Id == adtto.classsectionList1[i].asmC_Id
                                            //                      select new { m, n}).ToList();
                                            if (adtto.duplicateList != null)
                                            {
                                                adtto.CheckIsDuplicate = adtto.duplicateList.Where(d => d.ASMCL_Id == adtto.classsectionList1[i].asmcL_Id && d.ASMC_Id == adtto.classsectionList1[i].asmC_Id).ToArray();
                                            }

                                            if (adtto.CheckIsDuplicate == null)
                                            {
                                                var result = _db.Adm_SchAttLoginUserClass.Single(t => t.ASALUC_Id == adtto.ASALUC_Id);
                                                result.ASALU_Id = enqq.ASALU_Id;
                                                result.ASMCL_Id = adtto.classsectionList1[i].asmcL_Id;
                                                result.ASMS_Id = adtto.classsectionList1[i].asmC_Id;
                                                result.UpdatedDate = DateTime.Now;
                                                result.CreatedDate = result.CreatedDate;
                                                _db.Update(result);
                                                var flag = _db.SaveChanges();
                                                if (flag > 0)
                                                {
                                                    transaction.Commit();
                                                    adtto.returnval = true;
                                                    adtto.operation = "updated";
                                                }
                                                else
                                                {
                                                    adtto.returnval = false;
                                                }
                                            }
                                            else
                                            {
                                                //adtto.message = "Duplicate records Exist";
                                                adtto.message = " Record Already Exists";
                                            }

                                        }
                                    }
                                }
                            }

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                            transaction.Rollback();
                        }
                    }
                }

                List<LoginPrevilegesDataDTO> ln = new List<LoginPrevilegesDataDTO>();
                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "GetLoginPreviledges";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MIID",
                    SqlDbType.Int)
                    {
                        Value = Convert.ToInt32(adtto.MI_Id)
                    });
                    cmd.Parameters.Add(new SqlParameter("@Year",
                   SqlDbType.Int)
                    {
                        Value = Convert.ToInt32(adtto.ASMAY_Id)
                    });
                    cmd.Parameters.Add(new SqlParameter("@EntryFlag",
                   SqlDbType.Int)
                    {
                        Value = Convert.ToInt32(adtto.ASALU_EntryTypeFlag)
                    });
                    cmd.Parameters.Add(new SqlParameter("@hrme_id",
                 SqlDbType.VarChar)
                    {
                        Value = adtto.HRME_Id
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
                                ln.Add(new LoginPrevilegesDataDTO
                                {
                                    ASALU_Id = Convert.ToInt64(dataReader["ASALU_Id"]),
                                    ASALUC_Id = Convert.ToInt64(dataReader["ASALUC_Id"]),
                                    ASALUCS_Id = Convert.ToInt64(dataReader["ASALUCS_Id"]),
                                    UserName = Convert.ToString(dataReader["IVRMSTAUL_UserName"]),
                                    ASMCL_ClassName = Convert.ToString(dataReader["ASMCL_ClassName"]),
                                    ASMC_SectionName = Convert.ToString(dataReader["ASMC_SectionName"]),
                                    PAMS_SubjectName = Convert.ToString(dataReader["PAMS_SubjectName"])
                                });
                                adtto.loginPData = ln.ToArray();
                                if (adtto.loginPData.Length > 0)
                                {
                                    adtto.count = adtto.loginPData.Length;
                                }
                                else
                                {
                                    adtto.count = 0;
                                }
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return adtto;
        }
        public AttendanceLPDTO SaveData(AttendanceLPDTO adtto)
        {
            try
            {

                if (adtto.classsectionList1 != null && adtto.classsectionList1.Count() > 0)
                {
                    if (adtto.ASALU_EntryTypeFlag == 2 || adtto.ASALU_EntryTypeFlag == 3)
                    {
                        List<AttendanceLPDTO> lnn = new List<AttendanceLPDTO>();
                        for (int i = 0; i < adtto.classsectionList1.Count(); i++)
                        {
                            using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                            {
                                cmd.CommandText = "Att_Log_Priv_Check_Duplicate_New";
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.Add(new SqlParameter("@ASALU_EntryTypeFlag",
                                SqlDbType.Int)
                                {
                                    Value = Convert.ToInt32(adtto.ASALU_EntryTypeFlag)
                                });
                                cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                               SqlDbType.Int)
                                {
                                    Value = Convert.ToInt32(adtto.ASMAY_Id)
                                });
                                cmd.Parameters.Add(new SqlParameter("@IVRMUL_Id",
                               SqlDbType.Int)
                                {
                                    Value = Convert.ToInt32(adtto.HRME_Id)
                                });
                                cmd.Parameters.Add(new SqlParameter("@MI_Id",
                              SqlDbType.Int)
                                {
                                    Value = Convert.ToInt32(adtto.MI_Id)
                                });
                                cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                              SqlDbType.Int)
                                {
                                    Value = Convert.ToInt32(adtto.classsectionList1[i].asmcL_Id)
                                });
                                cmd.Parameters.Add(new SqlParameter("@ASMC_Id",
                              SqlDbType.Int)
                                {
                                    Value = Convert.ToInt32(adtto.classsectionList1[i].asmC_Id)
                                });
                                cmd.Parameters.Add(new SqlParameter("@PAMS_Id",
                                    SqlDbType.Int)
                                {
                                    Value = Convert.ToInt32(0)
                                });
                                cmd.Parameters.Add(new SqlParameter("@Type",
                             SqlDbType.Int)
                                {
                                    Value = Convert.ToInt32(0)
                                });
                                cmd.Parameters.Add(new SqlParameter("@ASALU_Id",
                             SqlDbType.VarChar)
                                {
                                    Value = adtto.ASALU_Id
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
                                            lnn.Add(new AttendanceLPDTO
                                            {
                                                ASALUCS_Id = Convert.ToInt64(dataReader["ASALU_Id"]),
                                                ASALUC_Id = Convert.ToInt64(dataReader["ASALUC_Id"]),
                                                ASMCL_Id = Convert.ToInt64(dataReader["ASMCL_Id"]),
                                                ASMC_Id = Convert.ToInt64(dataReader["ASMS_Id"]),
                                            });
                                            adtto.duplicateList = lnn.ToArray();
                                        }
                                    }
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine(e.Message);
                                }
                            }
                        }
                    }
                    //For Saving the subject wise teachers
                    else
                    {
                        List<AttendanceLPDTO> lnn12 = new List<AttendanceLPDTO>();
                        for (int k = 0; k < adtto.selectedcls_sec_subs.Length; k++)
                        {
                            for (int j = 0; j < adtto.selectedcls_sec_subs[k].subs.Length; j++)
                            {
                                using (var cmd1 = _db.Database.GetDbConnection().CreateCommand())
                                {
                                    cmd1.CommandText = "Att_Log_Priv_Check_Duplicate_New";
                                    cmd1.CommandType = CommandType.StoredProcedure;
                                    cmd1.Parameters.Add(new SqlParameter("@ASALU_EntryTypeFlag",
                                    SqlDbType.Int)
                                    {
                                        Value = Convert.ToInt32(adtto.ASALU_EntryTypeFlag)
                                    });
                                    cmd1.Parameters.Add(new SqlParameter("@ASMAY_Id",
                                   SqlDbType.Int)
                                    {
                                        Value = Convert.ToInt32(adtto.ASMAY_Id)
                                    });
                                    cmd1.Parameters.Add(new SqlParameter("@IVRMUL_Id",
                                   SqlDbType.Int)
                                    {
                                        Value = Convert.ToInt32(adtto.HRME_Id)
                                    });
                                    cmd1.Parameters.Add(new SqlParameter("@MI_Id",
                                  SqlDbType.Int)
                                    {
                                        Value = Convert.ToInt32(adtto.MI_Id)
                                    });
                                    cmd1.Parameters.Add(new SqlParameter("@ASMCL_Id",
                                  SqlDbType.Int)
                                    {
                                        Value = Convert.ToInt32(adtto.selectedcls_sec_subs[k].asmcL_Id)
                                    });
                                    cmd1.Parameters.Add(new SqlParameter("@ASMC_Id",
                                  SqlDbType.Int)
                                    {
                                        Value = Convert.ToInt32(adtto.selectedcls_sec_subs[k].asmC_Id)
                                    });
                                    cmd1.Parameters.Add(new SqlParameter("@PAMS_Id",
                                 SqlDbType.Int)
                                    {
                                        Value = Convert.ToInt32(adtto.selectedcls_sec_subs[k].subs[j].ISMS_Id)
                                    });
                                    cmd1.Parameters.Add(new SqlParameter("@Type",
                                 SqlDbType.Int)
                                    {
                                        Value = Convert.ToInt32(1)
                                    });
                                    cmd1.Parameters.Add(new SqlParameter("@ASALU_Id",
                            SqlDbType.VarChar)
                                    {
                                        Value = adtto.ASALU_Id
                                    });


                                    if (cmd1.Connection.State != ConnectionState.Open)
                                        cmd1.Connection.Open();

                                    var retObject = new List<dynamic>();
                                    try
                                    {
                                        using (var dataReader = cmd1.ExecuteReader())
                                        {
                                            while (dataReader.Read())
                                            {
                                                lnn12.Add(new AttendanceLPDTO
                                                {
                                                    ASALU_Id = Convert.ToInt64(dataReader["ASALU_Id"]),
                                                    ASALUC_Id = Convert.ToInt64(dataReader["ASALUC_Id"]),
                                                    ASALUCS_Id = Convert.ToInt64(dataReader["ASALUCS_Id"]),
                                                    ASMCL_Id = Convert.ToInt64(dataReader["ASMCL_Id"]),
                                                    ASMC_Id = Convert.ToInt64(dataReader["ASMS_Id"]),
                                                    PAMS_Id = Convert.ToInt64(dataReader["ISMS_Id"]),
                                                });
                                                adtto.duplicateList = lnn12.ToArray();
                                            }
                                        }
                                    }
                                    catch (Exception e)
                                    {
                                        Console.WriteLine(e.Message);
                                    }
                                }
                            }
                        }
                    }
                }
                if (adtto.ASALU_Id == 0)
                {
                    using (var transaction = _db.Database.BeginTransaction())
                    {
                        try
                        {
                            //checking the employee id is there or not in login 
                            Adm_SchoolAttendanceLoginUser enq = Mapper.Map<Adm_SchoolAttendanceLoginUser>(adtto);
                            var emp_check = (from a in _db.Adm_SchAttLoginUser
                                             where (a.MI_Id == adtto.MI_Id && a.ASMAY_Id == adtto.ASMAY_Id && a.HRME_Id == adtto.HRME_Id && a.ASALU_EntryTypeFlag == adtto.ASALU_EntryTypeFlag)
                                             select new AttendanceLPDTO
                                             {
                                                 ASALU_Id = a.ASALU_Id
                                             }
                                           ).ToList();
                            if (emp_check.Count == 0)
                            {

                                if (adtto.MI_Id > 0 && adtto.ASMAY_Id > 0)
                                {
                                    enq.CreatedDate = DateTime.Now;
                                    enq.UpdatedDate = DateTime.Now;
                                    _db.Add(enq);
                                    _db.SaveChanges();
                                }
                            }
                            else
                            {
                                enq.ASALU_Id = emp_check.FirstOrDefault().ASALU_Id;
                            }


                            if (enq.ASALU_Id > 0)
                            {

                                if (adtto.classsectionList1 != null && adtto.classsectionList1.Count() > 0)
                                {
                                    if (adtto.ASALU_EntryTypeFlag == 2 || adtto.ASALU_EntryTypeFlag == 3)
                                    {
                                        for (int i = 0; i < adtto.classsectionList1.Count(); i++)
                                        {
                                            if (adtto.duplicateList != null)
                                            {
                                                adtto.CheckIsDuplicate = adtto.duplicateList.Where(d => d.ASMCL_Id == adtto.classsectionList1[i].asmcL_Id && d.ASMC_Id == adtto.classsectionList1[i].asmC_Id).ToArray();
                                            }

                                            if (adtto.CheckIsDuplicate == null || adtto.CheckIsDuplicate.Length == 0)
                                            {
                                                Adm_SchoolAttendanceLoginUserClass enq222 = Mapper.Map<Adm_SchoolAttendanceLoginUserClass>(adtto.classsectionList1[i]);
                                                enq222.ASALU_Id = enq.ASALU_Id;
                                                enq222.ASMCL_Id = adtto.classsectionList1[i].asmcL_Id;
                                                enq222.ASMS_Id = adtto.classsectionList1[i].asmC_Id;
                                                enq222.CreatedDate = DateTime.Now;
                                                enq222.UpdatedDate = DateTime.Now;
                                                _db.Add(enq222);
                                                var flag = _db.SaveChanges();
                                                if (flag > 0)
                                                {
                                                    adtto.returnval = true;
                                                    adtto.operation = "saved";
                                                    adtto.count = 1;
                                                }
                                                else
                                                {
                                                    adtto.returnval = false;
                                                }
                                            }
                                            else
                                            {
                                                adtto.message = "Record Already Exists";
                                                //adtto.message = "Failed List" + adtto.classsectionList1[i];
                                            }
                                        }
                                        if (adtto.count > 0)
                                        {
                                            transaction.Commit();
                                        }
                                    }

                                    //saving subject wise teacher mapping
                                    else
                                    {
                                        for (int k = 0; k < adtto.selectedcls_sec_subs.Length; k++)
                                        {
                                            Adm_SchoolAttendanceLoginUserClass enq21 = new Adm_SchoolAttendanceLoginUserClass();
                                            enq21.ASALU_Id = enq.ASALU_Id;
                                            enq21.ASMCL_Id = adtto.selectedcls_sec_subs[k].asmcL_Id;
                                            enq21.ASMS_Id = adtto.selectedcls_sec_subs[k].asmC_Id;
                                            enq21.CreatedDate = DateTime.Now;
                                            enq21.UpdatedDate = DateTime.Now;
                                            _db.Add(enq21);
                                            _db.SaveChanges();


                                            if (enq21.ASALUC_Id > 0)
                                            {
                                                for (int j = 0; j < adtto.selectedcls_sec_subs[k].subs.Length; j++)
                                                {
                                                    if (adtto.duplicateList != null)
                                                    {
                                                        adtto.CheckIsDuplicate = adtto.duplicateList.Where(d => d.ASMCL_Id == adtto.selectedcls_sec_subs[k].asmcL_Id && d.ASMC_Id == adtto.selectedcls_sec_subs[k].asmC_Id && d.PAMS_Id == adtto.selectedcls_sec_subs[k].subs[j].ISMS_Id).ToArray();
                                                    }
                                                    if (adtto.CheckIsDuplicate == null || adtto.CheckIsDuplicate.Length == 0)
                                                    {
                                                        Adm_SchoolAttendanceLoginUserClassSubject enq31 = new Adm_SchoolAttendanceLoginUserClassSubject();
                                                        enq31.ASALUC_Id = enq21.ASALUC_Id;
                                                        enq31.ISMS_Id = adtto.selectedcls_sec_subs[k].subs[j].ISMS_Id;
                                                        enq31.CreatedDate = DateTime.Now;
                                                        enq31.UpdatedDate = DateTime.Now;
                                                        _db.Add(enq31);
                                                        var flag = _db.SaveChanges();
                                                        if (flag > 0)
                                                        {
                                                            adtto.returnval = true;
                                                            adtto.operation = "saved";
                                                            adtto.count = 1;
                                                        }
                                                        else
                                                        {
                                                            adtto.returnval = false;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        adtto.message = " Record Already Exists";
                                                    }
                                                }
                                            }
                                        }

                                        if (adtto.count > 0)
                                        {
                                            transaction.Commit();
                                        }

                                    }
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                            transaction.Rollback();
                        }
                    }
                }
                else
                {
                    using (var transaction = _db.Database.BeginTransaction())
                    {
                        try
                        {
                            //saving for subject teacher mapping
                            if (adtto.ASALU_EntryTypeFlag == 1)
                            {
                                Adm_SchoolAttendanceLoginUser enqq = new Adm_SchoolAttendanceLoginUser();
                                var result11 = _db.Adm_SchAttLoginUser.Where(t => t.ASALU_Id == adtto.ASALU_Id && t.HRME_Id == adtto.HRME_Id).ToList();
                                if (result11.Count > 0)
                                {
                                    var result111 = _db.Adm_SchAttLoginUser.Single(t => t.ASALU_Id == adtto.ASALU_Id);
                                    result111.UpdatedDate = DateTime.Now;
                                    result111.CreatedDate = result111.CreatedDate;
                                    result111.HRME_Id = adtto.HRME_Id;
                                    _db.Update(result111);
                                    _db.SaveChanges();
                                    enqq.ASALU_Id = result111.ASALU_Id;
                                }
                                else
                                {
                                    Adm_SchoolAttendanceLoginUser user = new Adm_SchoolAttendanceLoginUser();
                                    user.ASALU_Att_Exam_Flag = adtto.ASALU_Att_Exam_Flag;
                                    user.ASALU_EntryTypeFlag = adtto.ASALU_EntryTypeFlag;
                                    user.ASMAY_Id = adtto.ASMAY_Id;
                                    user.HRME_Id = adtto.HRME_Id;
                                    user.MI_Id = adtto.MI_Id;
                                    user.CreatedDate = DateTime.Now;
                                    user.UpdatedDate = DateTime.Now;
                                    _db.Add(user);
                                    _db.SaveChanges();
                                    enqq.ASALU_Id = user.ASALU_Id;
                                }
                                if (enqq.ASALU_Id > 0)
                                {

                                    if (adtto.classsectionList1 != null && adtto.classsectionList1.Count() > 0)
                                    {

                                        for (int k = 0; k < adtto.selectedcls_sec_subs.Length; k++)
                                        {
                                            Adm_SchoolAttendanceLoginUserClass enq221 = new Adm_SchoolAttendanceLoginUserClass();

                                            var result = _db.Adm_SchAttLoginUserClass.Single(t => t.ASALUC_Id == adtto.ASALUC_Id);
                                            result.ASALU_Id = enqq.ASALU_Id;
                                            result.ASMCL_Id = adtto.selectedcls_sec_subs[k].asmcL_Id;
                                            result.ASMS_Id = adtto.selectedcls_sec_subs[k].asmC_Id;
                                            result.UpdatedDate = DateTime.Now;
                                            result.CreatedDate = result.CreatedDate;
                                            _db.Update(result);
                                            _db.SaveChanges();


                                            for (int j = 0; j < adtto.selectedcls_sec_subs[k].subs.Length; j++)
                                            {
                                                Adm_SchoolAttendanceLoginUserClassSubject enq331 = new Adm_SchoolAttendanceLoginUserClassSubject();
                                                if (adtto.duplicateList != null)
                                                {
                                                    adtto.CheckIsDuplicate = adtto.duplicateList.Where(d => d.ASMCL_Id == adtto.classsectionList1[k].asmcL_Id && d.ASMC_Id == adtto.classsectionList1[k].asmC_Id && d.PAMS_Id == adtto.selectedcls_sec_subs[k].subs[j].ISMS_Id).ToArray();
                                                }

                                                if (adtto.CheckIsDuplicate == null)
                                                {
                                                    var result1 = _db.Adm_schAttLoginUserClassSubjects.Single(t => t.ASALUCS_Id == adtto.ASALUCS_Id);
                                                    result1.ASALUC_Id = adtto.ASALUC_Id;
                                                    result1.ISMS_Id = adtto.selectedcls_sec_subs[k].subs[j].ISMS_Id;
                                                    result1.UpdatedDate = DateTime.Now;
                                                    result1.CreatedDate = result1.CreatedDate;
                                                    _db.Update(result1);
                                                    var flag1 = _db.SaveChanges();
                                                    if (flag1 > 0)
                                                    {
                                                        adtto.returnval = true;
                                                        adtto.operation = "updated";
                                                        transaction.Commit();
                                                    }
                                                    else
                                                    {
                                                        adtto.returnval = false;
                                                    }
                                                }
                                                else
                                                {
                                                    //adtto.message = "duplicate record exist ";
                                                    adtto.message = " Record Already Exists";
                                                }

                                            }
                                        }
                                    }
                                }
                            }


                            else if (adtto.ASALU_EntryTypeFlag == 2 || adtto.ASALU_EntryTypeFlag == 3)
                            {
                                Adm_SchoolAttendanceLoginUser enqq = Mapper.Map<Adm_SchoolAttendanceLoginUser>(adtto);
                                var result22 = _db.Adm_SchAttLoginUser.Where(t => t.ASALU_Id == adtto.ASALU_Id && t.HRME_Id == adtto.HRME_Id).ToList();
                                if (result22.Count > 0)
                                {
                                    var result222 = _db.Adm_SchAttLoginUser.Single(t => t.ASALU_Id == adtto.ASALU_Id);
                                    result222.UpdatedDate = DateTime.Now;
                                    result222.CreatedDate = result222.CreatedDate;
                                    result222.HRME_Id = adtto.HRME_Id;
                                    _db.Update(result222);
                                    _db.SaveChanges();
                                    enqq.ASALU_Id = result222.ASALU_Id;
                                }
                                else
                                {
                                    Adm_SchoolAttendanceLoginUser user = new Adm_SchoolAttendanceLoginUser();
                                    user.ASALU_Att_Exam_Flag = adtto.ASALU_Att_Exam_Flag;
                                    user.ASALU_EntryTypeFlag = adtto.ASALU_EntryTypeFlag;
                                    user.ASMAY_Id = adtto.ASMAY_Id;
                                    user.HRME_Id = adtto.HRME_Id;
                                    user.MI_Id = adtto.MI_Id;
                                    user.CreatedDate = DateTime.Now;
                                    user.UpdatedDate = DateTime.Now;
                                    _db.Add(user);
                                    _db.SaveChanges();
                                    enqq.ASALU_Id = user.ASALU_Id;
                                }
                                if (enqq.ASALU_Id > 0)
                                {
                                    if (adtto.classsectionList1 != null && adtto.classsectionList1.Count() > 0)
                                    {
                                        for (int i = 0; i < adtto.classsectionList1.Count(); i++)
                                        {

                                            Adm_SchoolAttendanceLoginUserClass enq22 = Mapper.Map<Adm_SchoolAttendanceLoginUserClass>(adtto.classsectionList1[i]);
                                            if (adtto.duplicateList != null)
                                            {
                                                adtto.CheckIsDuplicate = adtto.duplicateList.Where(d => d.ASMCL_Id == adtto.classsectionList1[i].asmcL_Id && d.ASMC_Id == adtto.classsectionList1[i].asmC_Id).ToArray();
                                            }

                                            if (adtto.CheckIsDuplicate == null)
                                            {
                                                var result = _db.Adm_SchAttLoginUserClass.Single(t => t.ASALUC_Id == adtto.ASALUC_Id);
                                                result.ASALU_Id = enqq.ASALU_Id;
                                                result.ASMCL_Id = adtto.classsectionList1[i].asmcL_Id;
                                                result.ASMS_Id = adtto.classsectionList1[i].asmC_Id;
                                                result.UpdatedDate = DateTime.Now;
                                                result.CreatedDate = result.CreatedDate;
                                                _db.Update(result);
                                                var flag = _db.SaveChanges();
                                                if (flag > 0)
                                                {
                                                    transaction.Commit();
                                                    adtto.returnval = true;
                                                    adtto.operation = "updated";
                                                }
                                                else
                                                {
                                                    adtto.returnval = false;
                                                }
                                            }
                                            else
                                            {
                                                adtto.message = " Record Already Exists";
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                            transaction.Rollback();
                        }
                    }
                }

                List<LoginPrevilegesDataDTO> ln = new List<LoginPrevilegesDataDTO>();
                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "GetLoginPreviledges";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MIID",
                    SqlDbType.Int)
                    {
                        Value = Convert.ToInt32(adtto.MI_Id)
                    });
                    cmd.Parameters.Add(new SqlParameter("@Year",
                   SqlDbType.Int)
                    {
                        Value = Convert.ToInt32(adtto.ASMAY_Id)
                    });
                    cmd.Parameters.Add(new SqlParameter("@EntryFlag",
                   SqlDbType.Int)
                    {
                        Value = Convert.ToInt32(adtto.ASALU_EntryTypeFlag)
                    });
                    cmd.Parameters.Add(new SqlParameter("@hrme_id",
                 SqlDbType.VarChar)
                    {
                        Value = adtto.HRME_Id
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
                                ln.Add(new LoginPrevilegesDataDTO
                                {
                                    ASALU_Id = Convert.ToInt64(dataReader["ASALU_Id"]),
                                    ASALUC_Id = Convert.ToInt64(dataReader["ASALUC_Id"]),
                                    ASALUCS_Id = Convert.ToInt64(dataReader["ASALUCS_Id"]),
                                    UserName = Convert.ToString(dataReader["IVRMSTAUL_UserName"]),
                                    ASMCL_ClassName = Convert.ToString(dataReader["ASMCL_ClassName"]),
                                    ASMC_SectionName = Convert.ToString(dataReader["ASMC_SectionName"]),
                                    PAMS_SubjectName = Convert.ToString(dataReader["PAMS_SubjectName"])
                                });
                                adtto.loginPData = ln.ToArray();
                                if (adtto.loginPData.Length > 0)
                                {
                                    adtto.count = adtto.loginPData.Length;
                                }
                                else
                                {
                                    adtto.count = 0;
                                }
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return adtto;
        }
        public AttendanceLPDTO getDataByTypeSelected(AttendanceLPDTO dto)
        {
            AttendanceLPDTO AttDTO_Obj = new AttendanceLPDTO();
            List<LoginPrevilegesDataDTO> ln = new List<LoginPrevilegesDataDTO>();
            using (var cmd = _db.Database.GetDbConnection().CreateCommand())
            {
                cmd.CommandText = "GetLoginPreviledges";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@MIID",
                SqlDbType.Int)
                {
                    Value = Convert.ToInt32(dto.MI_Id)
                });
                cmd.Parameters.Add(new SqlParameter("@Year",
               SqlDbType.Int)
                {
                    Value = Convert.ToInt32(dto.ASMAY_Id)
                });
                cmd.Parameters.Add(new SqlParameter("@EntryFlag",
               SqlDbType.Int)
                {
                    Value = Convert.ToInt32(dto.ASALU_EntryTypeFlag)
                });
                cmd.Parameters.Add(new SqlParameter("@hrme_id",
                  SqlDbType.VarChar)
                {
                    Value = dto.HRME_Id
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
                            ln.Add(new LoginPrevilegesDataDTO
                            {
                                ASALU_Id = Convert.ToInt64(dataReader["ASALU_Id"]),
                                ASALUC_Id = Convert.ToInt64(dataReader["ASALUC_Id"]),
                                ASALUCS_Id = Convert.ToInt64(dataReader["ASALUCS_Id"]),
                                UserName = Convert.ToString(dataReader["IVRMSTAUL_UserName"]),
                                ASMCL_ClassName = Convert.ToString(dataReader["ASMCL_ClassName"]),
                                ASMC_SectionName = Convert.ToString(dataReader["ASMC_SectionName"]),
                                PAMS_SubjectName = Convert.ToString(dataReader["PAMS_SubjectName"]),
                                ASMAY_Year = Convert.ToString(dataReader["ASMAY_Year"]),
                            });
                            AttDTO_Obj.loginPData = ln.ToArray();
                            if (AttDTO_Obj.loginPData.Length > 0)
                            {
                                AttDTO_Obj.count = AttDTO_Obj.loginPData.Length;
                            }
                            else
                            {
                                AttDTO_Obj.count = 0;
                            }
                        }
                    }

                }
                catch (Exception ex)
                {
                    Console.Write(ex.Message);
                }
            }

            try
            {
                // get subject list
                var subject = _AppDB.Subject.Where(s => s.MI_Id == dto.MI_Id && s.ISMS_ActiveFlag == 1 && s.ISMS_AttendanceFlag == true).ToList();
                AttDTO_Obj.subjectList = subject.ToArray();

                List<AttendanceLPDTO> classsection = new List<AttendanceLPDTO>();
                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "getClassSectiondata_New";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_ID",
                 SqlDbType.Int)
                    {
                        Value = Convert.ToInt32(dto.MI_Id)
                    });
                    cmd.Parameters.Add(new SqlParameter("@EntryFlag",
               SqlDbType.Int)
                    {
                        Value = Convert.ToInt32(dto.ASALU_EntryTypeFlag)
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
             SqlDbType.Int)
                    {
                        Value = Convert.ToInt32(dto.ASMAY_Id)
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
                                classsection.Add(new AttendanceLPDTO
                                {
                                    name = Convert.ToString(dataReader["name"]),
                                    ASMCL_Id = Convert.ToInt64(dataReader["ASMCL_Id"]),
                                    ASMC_Id = Convert.ToInt64(dataReader["ASMC_Id"]),
                                    classsection = Convert.ToString(dataReader["classsection"])
                                });
                                AttDTO_Obj.classsectionList = classsection.ToArray();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }
                List<AttendanceLPDTO> teachers = new List<AttendanceLPDTO>();
                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Adm_Att_Login_PL_getdata";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                  SqlDbType.Int)
                    {
                        Value = Convert.ToInt32(dto.MI_Id)
                    });
                    cmd.Parameters.Add(new SqlParameter("@flag",
               SqlDbType.Int)
                    {
                        Value = Convert.ToInt32(dto.ASALU_EntryTypeFlag)
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
              SqlDbType.VarChar)
                    {
                        Value = Convert.ToInt32(dto.ASMAY_Id)
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
                                teachers.Add(new AttendanceLPDTO
                                {
                                    UserId = Convert.ToInt32(dataReader["Id"]),
                                    UserName = Convert.ToString(dataReader["UserName"])
                                });
                                AttDTO_Obj.teacherList = teachers.ToArray();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return AttDTO_Obj;
        }
        public AttendanceLPDTO staffwisegrid(AttendanceLPDTO dto)
        {
            try
            {
                List<LoginPrevilegesDataDTO> ln = new List<LoginPrevilegesDataDTO>();
                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "GetLoginPreviledges_New";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MIID",
                    SqlDbType.Int)
                    {
                        Value = Convert.ToInt32(dto.MI_Id)
                    });
                    cmd.Parameters.Add(new SqlParameter("@Year",
                   SqlDbType.Int)
                    {
                        Value = Convert.ToInt32(dto.ASMAY_Id)
                    });
                    cmd.Parameters.Add(new SqlParameter("@EntryFlag",
                   SqlDbType.Int)
                    {
                        Value = Convert.ToInt32(dto.ASALU_EntryTypeFlag)
                    });
                    cmd.Parameters.Add(new SqlParameter("@hrme_id",
                  SqlDbType.VarChar)
                    {
                        Value = dto.HRME_Id
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
                                ln.Add(new LoginPrevilegesDataDTO
                                {
                                    ASALU_Id = Convert.ToInt64(dataReader["ASALU_Id"]),
                                    ASALUC_Id = Convert.ToInt64(dataReader["ASALUC_Id"]),
                                    ASALUCS_Id = Convert.ToInt64(dataReader["ASALUCS_Id"]),
                                    UserName = Convert.ToString(dataReader["IVRMSTAUL_UserName"]),
                                    ASMCL_ClassName = Convert.ToString(dataReader["ASMCL_ClassName"]),
                                    ASMC_SectionName = Convert.ToString(dataReader["ASMC_SectionName"]),
                                    PAMS_SubjectName = Convert.ToString(dataReader["PAMS_SubjectName"]),
                                    ASMAY_Year = Convert.ToString(dataReader["ASMAY_Year"]),
                                });
                                dto.loginPData = ln.ToArray();
                                if (dto.loginPData.Length > 0)
                                {
                                    dto.count = dto.loginPData.Length;
                                }
                                else
                                {
                                    dto.count = 0;
                                }
                            }
                        }


                        //getting class section for class teacher

                        List<AttendanceLPDTO> classsection = new List<AttendanceLPDTO>();
                        using (var cmd1 = _db.Database.GetDbConnection().CreateCommand())
                        {
                            cmd1.CommandText = "Adm_getClassSectiondata_By_Classteacher_New";
                            cmd1.CommandType = CommandType.StoredProcedure;
                            cmd1.Parameters.Add(new SqlParameter("@MI_ID",
                         SqlDbType.VarChar)
                            {
                                Value = Convert.ToInt32(dto.MI_Id)
                            });

                            cmd1.Parameters.Add(new SqlParameter("@asmay_id",
                        SqlDbType.VarChar)
                            {
                                Value = Convert.ToInt32(dto.ASMAY_Id)
                            });
                            cmd1.Parameters.Add(new SqlParameter("@HRME_Id",
                        SqlDbType.VarChar)
                            {
                                Value = Convert.ToInt32(dto.HRME_Id)
                            });

                            cmd1.Parameters.Add(new SqlParameter("@type",
                     SqlDbType.VarChar)
                            {
                                Value = dto.ASALU_EntryTypeFlag
                            });

                            if (cmd1.Connection.State != ConnectionState.Open)
                                cmd1.Connection.Open();
                            var retObject1 = new List<dynamic>();

                            try
                            {
                                using (var dataReader = cmd1.ExecuteReader())
                                {
                                    while (dataReader.Read())
                                    {
                                        classsection.Add(new AttendanceLPDTO
                                        {
                                            name = Convert.ToString(dataReader["name"]),
                                            ASMCL_Id = Convert.ToInt64(dataReader["ASMCL_Id"]),
                                            ASMC_Id = Convert.ToInt64(dataReader["ASMC_Id"]),
                                            classsection = Convert.ToString(dataReader["classsection"])
                                        });
                                        dto.classsectionList = classsection.ToArray();
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.Write(ex.Message);
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }
            }

            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return dto;
        }
        public AttendanceLPDTO deleteAttPrivileges(AttendanceLPDTO obj)
        {
            try
            {
                if (obj.ASALUCS_Id > 0)
                {
                    var rec = _db.Adm_schAttLoginUserClassSubjects.Where(d => d.ASALUCS_Id == obj.ASALUCS_Id).ToList();
                    if (rec.Any())
                    {
                        _db.Remove(rec.ElementAt(0));
                        var flag = _db.SaveChanges();
                        if (flag > 0)
                        {
                            obj.returnval = true;
                        }
                        else
                        {
                            obj.returnval = false;
                        }
                    }

                }
                else if (obj.ASALUC_Id > 0)
                {
                    var rec = _db.Adm_SchAttLoginUserClass.Where(d => d.ASALUC_Id == obj.ASALUC_Id).ToList();
                    if (rec.Any())
                    {
                        _db.Remove(rec.ElementAt(0));
                        var flag = _db.SaveChanges();
                        if (flag > 0)
                        {
                            obj.returnval = true;
                        }
                        else
                        {
                            obj.returnval = false;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                obj.message = "Sorry You Can Not Delete This Record.Because It Is Mapped In Attendance";
                Console.WriteLine(ex.Message);
            }
            return obj;
        }
        public AttendanceLPDTO getyear(AttendanceLPDTO dto)
        {
            AttendanceLPDTO AttDTO_Obj = new AttendanceLPDTO();
            List<LoginPrevilegesDataDTO> ln = new List<LoginPrevilegesDataDTO>();
            using (var cmd = _db.Database.GetDbConnection().CreateCommand())
            {
                cmd.CommandText = "GetLoginPreviledges";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@MIID",
                SqlDbType.Int)
                {
                    Value = Convert.ToInt32(dto.MI_Id)
                });
                cmd.Parameters.Add(new SqlParameter("@Year",
               SqlDbType.Int)
                {
                    Value = Convert.ToInt32(dto.ASMAY_Id)
                });
                cmd.Parameters.Add(new SqlParameter("@EntryFlag",
               SqlDbType.Int)
                {
                    Value = Convert.ToInt32(dto.ASALU_EntryTypeFlag)
                });
                cmd.Parameters.Add(new SqlParameter("@hrme_id",
                  SqlDbType.VarChar)
                {
                    Value = dto.HRME_Id
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
                            ln.Add(new LoginPrevilegesDataDTO
                            {
                                ASALU_Id = Convert.ToInt64(dataReader["ASALU_Id"]),
                                ASALUC_Id = Convert.ToInt64(dataReader["ASALUC_Id"]),
                                ASALUCS_Id = Convert.ToInt64(dataReader["ASALUCS_Id"]),
                                UserName = Convert.ToString(dataReader["IVRMSTAUL_UserName"]),
                                ASMCL_ClassName = Convert.ToString(dataReader["ASMCL_ClassName"]),
                                ASMC_SectionName = Convert.ToString(dataReader["ASMC_SectionName"]),
                                PAMS_SubjectName = Convert.ToString(dataReader["PAMS_SubjectName"]),
                                ASMAY_Year = Convert.ToString(dataReader["ASMAY_Year"]),
                            });
                            AttDTO_Obj.loginPData = ln.ToArray();
                            if (AttDTO_Obj.loginPData.Length > 0)
                            {
                                AttDTO_Obj.count = AttDTO_Obj.loginPData.Length;
                            }
                            else
                            {
                                AttDTO_Obj.count = 0;
                            }
                        }
                    }

                }
                catch (Exception ex)
                {
                    Console.Write(ex.Message);
                }
            }

            try
            {
                // get subject list
                var subject = _AppDB.Subject.Where(s => s.MI_Id == dto.MI_Id && s.ISMS_ActiveFlag == 1 && s.ISMS_AttendanceFlag == true).ToList();
                AttDTO_Obj.subjectList = subject.ToArray();

                List<AttendanceLPDTO> classsection = new List<AttendanceLPDTO>();
                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "getClassSectiondata_New";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_ID",
                 SqlDbType.Int)
                    {
                        Value = Convert.ToInt32(dto.MI_Id)
                    });
                    cmd.Parameters.Add(new SqlParameter("@EntryFlag",
               SqlDbType.Int)
                    {
                        Value = Convert.ToInt32(dto.ASALU_EntryTypeFlag)
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
             SqlDbType.Int)
                    {
                        Value = Convert.ToInt32(dto.ASMAY_Id)
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
                                classsection.Add(new AttendanceLPDTO
                                {
                                    name = Convert.ToString(dataReader["name"]),
                                    ASMCL_Id = Convert.ToInt64(dataReader["ASMCL_Id"]),
                                    ASMC_Id = Convert.ToInt64(dataReader["ASMC_Id"]),
                                    classsection = Convert.ToString(dataReader["classsection"])
                                });
                                AttDTO_Obj.classsectionList = classsection.ToArray();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }
                List<AttendanceLPDTO> teachers = new List<AttendanceLPDTO>();
                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Adm_Att_Login_PL_getdata";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                  SqlDbType.Int)
                    {
                        Value = Convert.ToInt32(dto.MI_Id)
                    });
                    cmd.Parameters.Add(new SqlParameter("@flag",
               SqlDbType.Int)
                    {
                        Value = Convert.ToInt32(dto.ASALU_EntryTypeFlag)
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
              SqlDbType.VarChar)
                    {
                        Value = Convert.ToInt32(dto.ASMAY_Id)
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
                                teachers.Add(new AttendanceLPDTO
                                {
                                    UserId = Convert.ToInt32(dataReader["Id"]),
                                    UserName = Convert.ToString(dataReader["UserName"])
                                });
                                AttDTO_Obj.teacherList = teachers.ToArray();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return AttDTO_Obj;
        }

    }
}
