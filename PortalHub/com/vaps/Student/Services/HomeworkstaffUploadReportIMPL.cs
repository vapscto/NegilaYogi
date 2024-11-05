using CommonLibrary;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.Portals;
using DomainModel.Model;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.Portals.Student;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace PortalHub.com.vaps.Student.Services
{
    public class HomeworkstaffUploadReportIMPL : Interfaces.HomeworkstaffUploadReportInterface
    {
        public PortalContext _Context;
        private DomainModelMsSqlServerContext _dbsms;
        public HomeworkstaffUploadReportIMPL(PortalContext context)
        {
            _Context = context;
        }
        public HomeworkStaffReportDTO getAllDetail(HomeworkStaffReportDTO id)
        {
            try
            {
                id.Select_list = (from a in _Context.School_M_Class
                                  from  b in _Context.Masterclasscategory 
                                  where(a.MI_Id == id.MI_Id && b.Is_Active == true && a.ASMCL_Id==b.ASMCL_Id)
                                  select a).Distinct().OrderBy(a => a.ASMCL_Order).ToArray();
               
                //  List<MasterAcademic> allyear = new List<MasterAcademic>();
                //  id.academicdrp = _Context.AcademicYearDMO.Where(t => t.MI_Id == id.MI_Id && t.Is_Active == true).OrderByDescending(t => t.ASMAY_Order).Distinct().ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }
            return id;
        }
        public HomeworkStaffReportDTO get_load_onchange(HomeworkStaffReportDTO dto)
        {
            try
            {
                dto.Section_list = (from a in _Context.AdmSchoolMasterClassCatSec
                                    from b in _Context.Masterclasscategory
                                    from c in _Context.School_M_Section
                                    where (a.ASMCC_Id == b.ASMCC_Id && a.ASMS_Id == c.ASMS_Id
                                    && b.ASMCL_Id == dto.ASMCL_Id
                                    && a.ASMCCS_ActiveFlg == true)
                                    select c).Distinct().OrderBy(a => a.ASMC_Order).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }

            return dto;
        }

        //getOnchange
        public HomeworkStaffReportDTO getOnchange(HomeworkStaffReportDTO dto)
        {
            try
            {
                string FromDate = dto.IHW_DateFrom.Value.ToString("yyyy-MM-dd");
                string ToDate = dto.IHW_DateTo.Value.Date.ToString("yyyy-MM-dd");
                using (var cmd = _Context.Database.GetDbConnection().CreateCommand())
                {
                    //IVRM_HWSubjectwiseCount
                    cmd.CommandText = "IVRM_HWSWNotUploadedStudents";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    {
                        Value = dto.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.BigInt)
                    {
                        Value = dto.ASMAY_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.BigInt)
                    {
                        Value = dto.ASMCL_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.BigInt)
                    {
                        Value = dto.ASMS_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ISMS_Id", SqlDbType.BigInt)
                    {
                        Value = dto.ISMS_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@FromDate", SqlDbType.VarChar)
                    {
                        Value = FromDate
                    });
                    cmd.Parameters.Add(new SqlParameter("@Todate", SqlDbType.VarChar)
                    {
                        Value = ToDate
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
                        dto.getview = retObject.Distinct().ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                //dto.Select_list = (from a in _Context.School_M_Class
                //                    from b in _Context.Masterclasscategory
                //                    from c in _Context.AcademicYearDMO
                //                    where (a.MI_Id == dto.MI_Id && a.ASMCL_Id == b.ASMCL_Id && b.ASMAY_Id == c.ASMAY_Id && b.ASMAY_Id==dto.ASMAY_Id)
                //                    select a
                //                  ).Distinct().ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return dto;
        }
        public HomeworkStaffReportDTO getReport(HomeworkStaffReportDTO dto)
        {
            string sec_Id = "0";
            foreach (var c in dto.ASMSId_Filter)
            {
                sec_Id = sec_Id + "," + c.ASMS_Id.ToString();
            }
            string FromDate = dto.IHW_DateFrom.Value.ToString("yyyy-MM-dd");
            string ToDate = dto.IHW_DateTo.Value.Date.ToString("yyyy-MM-dd");
            using (var cmd = _Context.Database.GetDbConnection().CreateCommand())
            {
                //IVRM_HWSubjectwiseCount
                cmd.CommandText = "IVRM_HWSubjectwiseCount";
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar)
                {
                    Value = dto.MI_Id
                });
                cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar)
                {
                    Value = dto.ASMAY_Id
                });
                cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar)
                {
                    Value = dto.ASMCL_Id
                });
                cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar)
                {
                    Value = sec_Id
                });                            
                cmd.Parameters.Add(new SqlParameter("@FromDate", SqlDbType.VarChar)
                {
                    Value = FromDate
                });
                cmd.Parameters.Add(new SqlParameter("@Todate", SqlDbType.VarChar)
                {
                    Value = ToDate
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
                    dto.getReport = retObject.Distinct().ToArray();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return dto;
        }

        //-----Class Wise Report-------//
        public HomeworkStaffReportDTO getloadDetails(HomeworkStaffReportDTO id)
        {
            //Staff_User_Login     
            try
            {
                id.classlist = (from a in _Context.School_M_Class
                                from b in _Context.IVRM_ClassWorkDMO
                                where a.MI_Id == id.MI_Id && b.ICW_ActiveFlag == true
                                 select a).Distinct().OrderBy(a => a.ASMCL_Order).Distinct().ToArray();

                                    //if (id.roleType == "COORDINATOR" || id.roleType == "ADMIN")
                                    //{
                                    //    id.classlist = (
                                    //  from a in _Context.School_M_Class
                                    //  join b in _Context.IVRM_ClassWorkDMO on a.ASMCL_Id equals b.ASMCL_Id
                                    //  where a.MI_Id == id.MI_Id && b.ICW_ActiveFlag == true
                                    //  select a).Distinct().OrderBy(a => a.ASMCL_Order).Distinct().ToArray();
                                    //}
                                    //if (id.roleType == "Staff" || id.roleType == "STAFF")
                                    //{

                                    //}
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }



            return id;
        }
        public HomeworkStaffReportDTO getLoad_onchange(HomeworkStaffReportDTO dto)
        {

            try
            {
                dto.Section_list = (from a in _Context.AdmSchoolMasterClassCatSec
                                    from b in _Context.IVRM_ClassWorkDMO
                                    from c in _Context.School_M_Section
                                    where (a.ASMS_Id == b.ASMS_Id && b.ASMS_Id == c.ASMS_Id
                                    && b.ASMCL_Id == dto.ASMCL_Id
                                    && a.ASMCCS_ActiveFlg == true)
                                    select c).Distinct().OrderBy(a => a.ASMC_Order).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }

            return dto;

        }

        //smsemail
        public HomeworkStaffReportDTO smsemail(HomeworkStaffReportDTO dto)
        {            
            try
            {
                int y = 0;
                int z = 0;
                string msg = "";
                string msg1 = "";      
                //-------Email------------
                if(dto.studentemail != null)
                {
                    for (int k = 0; k < dto.studentemail.Length; k++)
                    {
                        try
                        {
                            var studDet = _Context.AdmissionStudentDMO.Where(t => t.MI_Id == dto.MI_Id && t.AMST_Id == dto.studentemail[k].AMST_Id).ToList();
                            if (studDet.Count > 0)
                            {
                                if (Convert.ToString(studDet.FirstOrDefault().AMST_emailId) != null)
                                {
                                    y = y + 1;
                                    try
                                    {
                                        Email email = new Email(_dbsms);
                                        if (dto.flagstring == "homework")
                                        {
                                             string m = email.sendmail(dto.MI_Id, studDet.FirstOrDefault().AMST_emailId, "homeworkNotupload", dto.studentemail[k].AMST_Id.Value);
                                        }
                                        else if (dto.flagstring == "classwork")
                                        {
                                            string m = email.sendmail(dto.MI_Id, studDet.FirstOrDefault().AMST_emailId, "classworkNotupload", dto.studentemail[k].AMST_Id.Value);
                                        }

                                    }
                                    catch (Exception ex)
                                    {
                                        msg = dto.studentemail[k].studentname;
                                        msg1 += msg;
                                        Console.WriteLine(ex.Message);
                                        continue;
                                    }
                                }
                                else
                                {
                                    msg = dto.studentemail[k].studentname;
                                    msg1 += msg;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);

                        }
                    }
                }
                if(dto.studentsms !=null)
                {
                    for (int R = 0; R < dto.studentsms.Length; R++)
                    {
                        try
                        {
                            var studDet = _Context.AdmissionStudentDMO.Where(t => t.MI_Id == dto.MI_Id && t.AMST_Id == dto.studentsms[R].AMST_Id).ToList();
                            if (studDet.Count > 0)
                            {
                                if (Convert.ToString(studDet.FirstOrDefault().AMST_MobileNo) != null)
                                {
                                    z = z + 1;
                                    try
                                    {
                                        SMS sms = new SMS(_dbsms);
                                        if (dto.flagstring == "homework")
                                        {
                                            //  string s = sms.sendSms(dto.MI_Id, studDet.FirstOrDefault().AMST_MobileNo, "homework", dto.studentsms[R].AMST_Id.Value).Result;
                                        }
                                        else if (dto.flagstring == "classwork")
                                        {
                                            //  string s = sms.sendSms(dto.MI_Id, studDet.FirstOrDefault().AMST_MobileNo, "classwork", dto.studentsms[R].AMST_Id.Value).Result;
                                        }

                                    }
                                    catch (Exception ex)
                                    {
                                        msg = dto.studentsms[R].studentname;
                                        msg1 += msg;
                                        Console.WriteLine(ex.Message);
                                        continue;
                                    }
                                }
                                else
                                {
                                    msg = dto.studentsms[R].studentname;
                                    msg1 += msg;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);

                        }
                    }
                }
               

                //----sms--------
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }
            return dto;
        }
        public HomeworkStaffReportDTO getReport_classwise(HomeworkStaffReportDTO dto)
        {
            string sec_Id = "0";
            foreach (var c in dto.ASMSId_Filter)
            {
                sec_Id = sec_Id + "," + c.ASMS_Id.ToString();
            }
            string FromDate = ""; string ToDate = "";
            if(dto.IHW_DateFrom !=null && dto.IHW_DateTo !=null)
            {
                FromDate = dto.IHW_DateFrom.Value.ToString("yyyy-MM-dd");
                ToDate = dto.IHW_DateTo.Value.Date.ToString("yyyy-MM-dd");
            }            
            using (var cmd = _Context.Database.GetDbConnection().CreateCommand())
            {
                cmd.CommandText = "IVRM_CWSubjectwiseCount";
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar)
                {
                    Value = dto.MI_Id
                });
               
                cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar)
                {
                    Value = dto.ASMAY_Id
                });
                cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar)
                {
                    Value = dto.ASMCL_Id
                });
                cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar)
                {
                    Value = sec_Id
                });
                cmd.Parameters.Add(new SqlParameter("@FromDate", SqlDbType.VarChar)
                {
                    Value = FromDate
                });
                cmd.Parameters.Add(new SqlParameter("@Todate", SqlDbType.VarChar)
                {
                    Value = ToDate
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
                    dto.getReport = retObject.Distinct().ToArray();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return dto;
        }
        public HomeworkStaffReportDTO getOnchangeclass(HomeworkStaffReportDTO dto)
        {
            try
            {
                string FromDate = ""; string ToDate = "";
                if(dto.IHW_DateFrom !=null && dto.IHW_DateTo !=null)
                {
                    FromDate = dto.IHW_DateFrom.Value.ToString("yyyy-MM-dd");
                    ToDate = dto.IHW_DateTo.Value.Date.ToString("yyyy-MM-dd");
                }
            
                using (var cmd = _Context.Database.GetDbConnection().CreateCommand())
                {
                    //IVRM_HWSubjectwiseCount
                    cmd.CommandText = "IVRM_CWSWNotUploadedStudents";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    {
                        Value = dto.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.BigInt)
                    {
                        Value = dto.ASMAY_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.BigInt)
                    {
                        Value = dto.ASMCL_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.BigInt)
                    {
                        Value = dto.ASMS_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ISMS_Id", SqlDbType.BigInt)
                    {
                        Value = dto.ISMS_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@FromDate", SqlDbType.VarChar)
                    {
                        Value = FromDate
                    });
                    cmd.Parameters.Add(new SqlParameter("@Todate", SqlDbType.VarChar)
                    {
                        Value = ToDate
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
                        dto.getview = retObject.Distinct().ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                //dto.Select_list = (from a in _Context.School_M_Class
                //                    from b in _Context.Masterclasscategory
                //                    from c in _Context.AcademicYearDMO
                //                    where (a.MI_Id == dto.MI_Id && a.ASMCL_Id == b.ASMCL_Id && b.ASMAY_Id == c.ASMAY_Id && b.ASMAY_Id==dto.ASMAY_Id)
                //                    select a
                //                  ).Distinct().ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return dto;
        }
    }
}
