using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.admission;
using DomainModel.Model;
using DomainModel.Model.com.vaps.admission;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.com.vaps.admission;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace AdmissionServiceHub.com.vaps.Services
{
    public class studenttccustomreportImpl : Interfaces.studenttccustomreportInterface
    {
        public HHSTCCustomReportContext _reporttc;
        public studenttccustomreportImpl(HHSTCCustomReportContext reporttc)
        {
            _reporttc = reporttc;
        }
        public studenttccustomreportDTO getinitialdata(int id)
        {
            studenttccustomreportDTO data = new studenttccustomreportDTO();
            data.accyear = _reporttc.accyear.Where(t => t.MI_Id == id && t.Is_Active == true).OrderByDescending(t => t.ASMAY_Order).ToArray();
            data.accclass = _reporttc.accclass.Where(t => t.MI_Id == id && t.ASMCL_ActiveFlag == true).OrderBy(t => t.ASMCL_Order).ToArray();
            data.accsection = _reporttc.accsection.Where(t => t.MI_Id == id && t.ASMC_ActiveFlag == 1).OrderBy(t => t.ASMC_Order).ToArray();
            return data;
        }
        public studenttccustomreportDTO changeyear(studenttccustomreportDTO data)
        {
            try
            {
                var flag = "";
                if (data.tctemporper == "Temp")
                {
                    flag = "T";
                }
                else if (data.tctemporper == "Per")
                {
                    flag = "L";
                }

                data.accclass = (from a in _reporttc.accclass
                                 from b in _reporttc.Masterclasscategory
                                 where (a.ASMCL_Id == b.ASMCL_Id && b.ASMAY_Id == data.ASMAY_Id && b.Is_Active == true)
                                 select a).Distinct().OrderBy(a => a.ASMCL_Order).ToArray();

                if (data.report_type == "All")
                {
                    data.studentlist = (from a in _reporttc.studenttc
                                        from b in _reporttc.yearwisestudent
                                        from c in _reporttc.student
                                        where (a.AMST_Id == b.AMST_Id && c.AMST_Id == b.AMST_Id && a.ASMAY_Id == data.ASMAY_Id && c.MI_Id == data.MI_Id
                                        && c.AMST_SOL == flag && b.ASMAY_Id == data.ASMAY_Id && a.ASTC_DeletedFlag==false)
                                        select new studenttccustomreportDTO
                                        {
                                            AMST_Id = a.AMST_Id,
                                            studentname = ((c.AMST_FirstName == null ? " " : c.AMST_FirstName) + (c.AMST_MiddleName == null ? " " : c.AMST_MiddleName) + (c.AMST_LastName == null ? " " : c.AMST_LastName) + ':' + (c.AMST_AdmNo == null ? " " : c.AMST_AdmNo)).Trim(),
                                        }).Distinct().ToArray();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public studenttccustomreportDTO changeclass(studenttccustomreportDTO data)
        {
            try
            {
                data.accsection = (from a in _reporttc.AdmSchoolMasterClassCatSec
                                   from b in _reporttc.Masterclasscategory
                                   from c in _reporttc.accclass
                                   from d in _reporttc.accsection
                                   where (a.ASMCC_Id == b.ASMCC_Id && b.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == d.ASMS_Id && b.ASMCL_Id == data.ASMCL_Id
                                   && b.ASMAY_Id == data.ASMAY_Id && b.Is_Active == true && a.ASMCCS_ActiveFlg == true)
                                   select d).Distinct().OrderBy(a => a.ASMC_Order).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public studenttccustomreportDTO changesection(studenttccustomreportDTO data)
        {
            try
            {
                var flag = "";
                if (data.tctemporper == "Temp")
                {
                    flag = "T";
                }
                else if (data.tctemporper == "Per")
                {
                    flag = "L";
                }

                data.studentlist = (from a in _reporttc.studenttc
                                    from b in _reporttc.yearwisestudent
                                    from c in _reporttc.student
                                    where (a.AMST_Id == b.AMST_Id && c.AMST_Id == b.AMST_Id && a.ASMAY_Id == data.ASMAY_Id && c.MI_Id == data.MI_Id
                                    && c.AMST_SOL == flag && b.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id
                                    && b.ASMCL_Id == data.ASMCL_Id && b.ASMS_Id == data.ASMS_Id)
                                    select new studenttccustomreportDTO
                                    {
                                        AMST_Id = a.AMST_Id,
                                        studentname = ((c.AMST_FirstName == null ? " " : c.AMST_FirstName) + (c.AMST_MiddleName == null ? " " : c.AMST_MiddleName) + (c.AMST_LastName == null ? " " : c.AMST_LastName) + ':' + (c.AMST_AdmNo == null ? " " : c.AMST_AdmNo)).Trim(),
                                    }).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public studenttccustomreportDTO getTCdata(studenttccustomreportDTO data)
        {
            try
            {
                var flag = "";
                if (data.tctemporper == "temp" || data.tctemporper == "Temp")
                {
                    flag = "T";
                }
                else if (data.tctemporper == "Per")
                {
                    flag = "L";
                }
                //data.studentTCList = (from a in _reporttc.student
                //                      from b in _reporttc.studenttc
                //                      from c in _reporttc.accsection
                //                      from d in _reporttc.accclass
                //                      from e in _reporttc.Institution
                //                      where (a.AMST_Id == b.AMST_Id && b.ASMS_Id == c.ASMS_Id && a.AMST_Id == data.AMST_Id && b.ASMCL_Id == d.ASMCL_Id && e.MI_Id==a.MI_Id 
                //                      && a.AMST_SOL == flag)
                //                      select new studenttccustomreportDTO
                //                      {
                //                          studentname = ((a.AMST_FirstName == null ? " " : a.AMST_FirstName) + (a.AMST_MiddleName == null ? " " : " " + a.AMST_MiddleName) + (a.AMST_LastName == null ? " " : " " + a.AMST_LastName)).Trim(),
                //                          AMST_AdmNo = a.AMST_AdmNo,
                //                          AMST_RegistrationNo = a.AMST_RegistrationNo,
                //                          AMST_FatherName = a.AMST_FatherName,
                //                          AMST_MotherName = a.AMST_MotherName,
                //                          Nationality = a.AMST_Nationality != 0 && a.AMST_Nationality != null ? _reporttc.Country.Single(v => v.IVRMMC_Id == a.AMST_Nationality).IVRMMC_Nationality : "",
                //                          AMST_BirthPlace = a.AMST_BirthPlace,
                //                          ASTC_LastAttendedDate = b.ASTC_LastAttendedDate,
                //                          AMST_Sex = a.AMST_Sex,
                //                          AMST_DOB = a.AMST_DOB.Date,
                //                          AMST_DOB_Words = a.AMST_DOB_Words,
                //                          AMST_Date = a.AMST_Date.Date,
                //                          astC_TCNO = b.ASTC_TCNO,
                //                          AMST_emailId = a.AMST_emailId,
                //                          AMST_AadharNo = a.AMST_AadharNo,
                //                          AMST_MobileNo = a.AMST_MobileNo,
                //                          ASMCL_Id = d.ASMCL_Id,
                //                          Last_Class_Studied = d.ASMCL_ClassName,
                //                          astC_LeavingReason = b.ASTC_LeavingReason,
                //                          astC_TCIssueDate = b.ASTC_TCDate,
                //                          AMST_PerCity = a.AMST_PerCity,
                //                          AMST_PerStreet = a.AMST_PerStreet,
                //                          AMST_PerArea = a.AMST_PerArea,
                //                          AMST_ConStreet = a.AMST_ConStreet,
                //                          AMST_ConArea = a.AMST_ConArea,
                //                          AMST_ConCity = a.AMST_ConCity,
                //                          ASTC_Remarks = b.ASTC_Remarks,
                //                          Religion = a.IVRMMR_Id != 0 && a.IVRMMR_Id != null ? _reporttc.religion.Single(v => v.IVRMMR_Id == a.IVRMMR_Id).IVRMMR_Name : "",
                //                          caste = a.IC_Id != 0 && a.IC_Id != null ? _reporttc.caste.Single(v => v.IMC_Id == a.IC_Id && v.MI_Id == data.MI_Id).IMC_CasteName : "",
                //                          ASTC_Conduct = b.ASTC_Conduct,
                //                          ASMC_SectionName = c.ASMC_SectionName,
                //                          ASTC_Qual_PromotionFlag = b.ASTC_Qual_Class,
                //                          Fee_Due_Amnt = b.Fee_Due_Amnt,
                //                          Library_Due_Amnt = b.Library_Due_Amnt,
                //                          Store_Canteen_Due = b.Store_Canteen_Due,
                //                          PDA_Due = b.PDA_Due,
                //                          classname = d.ASMCL_ClassName,
                //                          qualificlass = b.ASTC_Qual_Class,
                //                          tcdatess = b.ASTC_TCDate,
                //                          langustudies = b.ASTC_LanguageStudied,
                //                          electivestudies = b.ASTC_ElectivesStudied,
                //                          promotedflag = b.ASTC_Qual_PromotionFlag,
                //                          feedue = b.ASTC_FeePaid,
                //                          feeconcession = b.ASTC_FeeConcession,
                //                          totalworkingdays = b.ASTC_WorkingDays,
                //                          noworkingdays = b.ASTC_AttendedDays,
                //                          govtadmno = a.AMST_GovtAdmno,
                //                          ASTC_TCApplicationDate = b.ASTC_TCApplicationDate,
                //                          subcaste = a.AMST_SubCasteIMC_Id,
                //                          medium = b.ASTC_MediumOfINStruction,
                //                          logo = e.MI_Logo,
                //                          classjoinname = a.ASMCL_Id != 0 && a.ASMCL_Id != null ? _reporttc.accclass.Single(v => v.ASMCL_Id == a.ASMCL_Id).ASMCL_ClassName : "",
                //                          streamname = a.ASMST_Id != 0 && a.ASMST_Id != null ? _reporttc.Adm_School_Master_Stream.Single(v => v.ASMST_Id == a.ASMST_Id).ASMST_StreamName : "",
                //                      }).ToArray();


                using (
                    
                    var cmd = _reporttc.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TC_Data";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.AMST_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                   SqlDbType.BigInt)
                    {
                        Value = data.ASMAY_Id
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
                                       dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.studentTCList = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }


                data.MasterCompany = (from a in _reporttc.Institution
                                      where (a.MI_Id == data.MI_Id)
                                      select new studenttccustomreportDTO
                                      {
                                          companyname = a.IVRMMCT_Name,
                                          MI_Id = a.MI_Id,
                                      }).ToArray();

                data.academicList1 = _reporttc.accyear.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id).ToArray();

                data.previousschool = _reporttc.StudentPrevSchoolDMO.Where(a => a.MI_Id == data.MI_Id && a.AMST_Id == data.AMST_Id).ToArray();

                var getnextclass1 = (from a in _reporttc.studenttc
                                     where (a.MI_Id == data.MI_Id && a.AMST_Id == data.AMST_Id)
                                     select new studenttccustomreportDTO
                                     {
                                         classid = a.ASMCL_Id,
                                     }).Distinct().ToArray();

                var classnext = getnextclass1.FirstOrDefault().classid + 1;
                data.getnextclass = _reporttc.accclass.Where(a => a.MI_Id == data.MI_Id && a.ASMCL_Id == Convert.ToInt64(classnext.ToString())).ToArray();

                data.classnamejoin = (from a in _reporttc.student
                                      from b in _reporttc.accclass
                                      where (a.ASMCL_Id == b.ASMCL_Id && a.MI_Id == data.MI_Id && a.AMST_Id == data.AMST_Id)
                                      select new studenttccustomreportDTO
                                      {
                                          joinclassid = a.ASMCL_Id,
                                          classjoinname = b.ASMCL_ClassName
                                      }).ToArray();

                data.studenttcdetails = _reporttc.studenttc.Where(a => a.AMST_Id == data.AMST_Id && a.ASMAY_Id == data.ASMAY_Id).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
    }
}



