using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.admission;
using DomainModel.Model;
using DomainModel.Model.com.vaps.admission;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.admission;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace AdmissionServiceHub.com.vaps.Services
{
    public class classwisestudentdetailsssImpl : Interfaces.ClasswisestudentdetailsInterface
    {
        public StudentYearLossReportContext _Classwisestudentdetailscontext;
        public classwisestudentdetailsssImpl(StudentYearLossReportContext frgContext)
        {
            _Classwisestudentdetailscontext = frgContext;
        }
        private readonly UserManager<ApplicationUser> _UserManager;
        public classwisestudentdetailsssImpl(StudentYearLossReportContext Classwisestudentdetailscontext, UserManager<ApplicationUser> UserManager)
        {
            _Classwisestudentdetailscontext = Classwisestudentdetailscontext;
            _UserManager = UserManager;
        }
        public ClasswisestudentdetailsDTO getdetails(ClasswisestudentdetailsDTO data)
        {

            try
            {
                List<MasterAcademic> year = new List<MasterAcademic>();
                year = _Classwisestudentdetailscontext.AcademicYear.Where(y => y.MI_Id == data.mid && y.Is_Active == true).OrderByDescending(a => a.ASMAY_Order).ToList();
                data.fillyear = year.ToArray();

                List<School_M_Class> classname = new List<School_M_Class>();
                classname = _Classwisestudentdetailscontext.admissioncls.Where(c => c.MI_Id == data.mid && c.ASMCL_ActiveFlag == true).OrderBy(c => c.ASMCL_Order).ToList();
                data.fillclass = classname.ToArray();


                List<School_M_Section> secname = new List<School_M_Section>();
                secname = _Classwisestudentdetailscontext.school_M_Section.Where(s => s.MI_Id == data.mid && s.ASMC_ActiveFlag == 1).OrderBy(s => s.ASMC_Order).ToList();
                data.fillsection = secname.ToArray();





                var rolet = _Classwisestudentdetailscontext.IVRM_Role_Type.Where(t => t.IVRMRT_Id == data.IVRMRT_Id).ToList();
                data.HRME_Id = 0;
                if (rolet.FirstOrDefault().IVRMRT_Role.Equals("ADMIN", StringComparison.OrdinalIgnoreCase)
                    || rolet.FirstOrDefault().IVRMRT_Role.Equals("Admission End User", StringComparison.OrdinalIgnoreCase)
                     || rolet.FirstOrDefault().IVRMRT_Role.Equals("COORDINATOR", StringComparison.OrdinalIgnoreCase)
                      || rolet.FirstOrDefault().IVRMRT_Role.Equals("END USER", StringComparison.OrdinalIgnoreCase)
                       || rolet.FirstOrDefault().IVRMRT_Role.Equals("Fee End User", StringComparison.OrdinalIgnoreCase)
                        || rolet.FirstOrDefault().IVRMRT_Role.Equals("Administrator", StringComparison.OrdinalIgnoreCase))
                {
                    data.HRME_Id = 0;
                }
                else
                {
                    var hrmeidcount = _Classwisestudentdetailscontext.Staff_User_Login.Where(c => c.Id == data.UserId && c.MI_Id == data.mid).Count();
                    if (hrmeidcount > 0)
                    {
                        data.HRME_Id = _Classwisestudentdetailscontext.Staff_User_Login.Single(c => c.Id == data.UserId && c.MI_Id == data.mid).Emp_Code;
                    }


                }
                //IVRM_StaffwiseClassStdata

                using (var cmd = _Classwisestudentdetailscontext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "IVRM_StaffwiseClassStdata_new";

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                SqlDbType.BigInt)
                    {
                        Value = data.mid
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                     SqlDbType.BigInt)
                    {
                        Value = data.ASMAY_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@UserId",
                SqlDbType.BigInt)
                    {
                        Value = data.UserId
                    });
                    //    cmd.Parameters.Add(new SqlParameter("@HRME_Id",
                    //SqlDbType.BigInt)
                    //    {
                    //        Value = data.HRME_Id
                    //    });
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
                        data.fillclass = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                var cat = _Classwisestudentdetailscontext.GenConfig.Where(g => g.MI_Id == data.mid && g.IVRMGC_CatLogoFlg == true).ToList();
                if (cat.Count > 0)
                {


                    data.category_list = _Classwisestudentdetailscontext.category.Where(f => f.MI_Id == data.mid && f.AMC_ActiveFlag == 1).ToArray();
                    data.categoryflag = true;
                }
                else
                {
                    data.categoryflag = false;
                }


            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public async Task<ClasswisestudentdetailsDTO> Getreportdetails(ClasswisestudentdetailsDTO data)
        {
            string IVRM_CLM_coloumn = "";
            string name = "";

            try
            {
                //data.ASMAY_Id = data.ASMC_Id;
                for (int i = 0; i < data.TempararyArrayheadList.Length; i++)
                {
                    name = data.TempararyArrayheadList[i].columnID;
                    if (name != null)
                    {
                        if (name == "serial_num")
                        {
                            if (name == "serial_num")
                            {
                                if (IVRM_CLM_coloumn != "")
                                {
                                    IVRM_CLM_coloumn = "ROW_NUMBER() OVER(ORDER BY (SELECT 1)) AS serial_num,";
                                }
                                else
                                {
                                    IVRM_CLM_coloumn = "ROW_NUMBER() OVER(ORDER BY (SELECT 1)) AS serial_num,";
                                }
                            }
                        }
                        else if (name == "AMST_FirstName")
                        {
                            if (data.TempararyArrayheadList.Length == 1)
                            {
                                IVRM_CLM_coloumn += "AMST_FirstName = CASE WHEN  AMST_FirstName is null or AMST_FirstName=''  then '' else AMST_FirstName end+CASE WHEN  AMST_MiddleName is null or AMST_MiddleName = '' or AMST_MiddleName = '0' then ''  ELSE ' ' + AMST_MiddleName END +  CASE WHEN AMST_LastName is null or AMST_LastName = '' or AMST_LastName = '0' then ''  ELSE ' '  + AMST_LastName END,";
                            }

                            else
                            {
                                IVRM_CLM_coloumn += "AMST_FirstName = CASE WHEN  AMST_FirstName is null or AMST_FirstName=''  then '' else AMST_FirstName end+CASE WHEN  AMST_MiddleName is null or AMST_MiddleName = '' or AMST_MiddleName = '0' then ''  ELSE ' '  + AMST_MiddleName END +  CASE WHEN AMST_LastName is null or AMST_LastName = '' or AMST_LastName = '0' then ''  ELSE ' '  + AMST_LastName END,";
                            }
                        }

                        else if (name == "AMST_PerAdd3")
                        {
                            if (name == "AMST_PerAdd3")
                            {

                            
                                if (IVRM_CLM_coloumn != "")
                            {
                                IVRM_CLM_coloumn += "AMST_PerAdd3 = CASE WHEN  REPLACE(AMST_PerStreet,',','') is null or REPLACE(AMST_PerStreet,',','')='' then '' else REPLACE(AMST_PerStreet,',','') end+ CASE WHEN  REPLACE(AMST_PerArea, ',', '') is null or REPLACE(AMST_PerArea,',', '')= '' then ''  ELSE ', ' + REPLACE(AMST_PerArea, ',', '') END +   CASE WHEN REPLACE(AMST_PerCity,',', '') is null or REPLACE(AMST_PerCity,',', '')= '' then ''  ELSE ', ' + REPLACE(AMST_PerCity, ',', '') END + CASE WHEN AMST_PerDistrict is null or AMST_PerDistrict = '' then ''  ELSE ', ' +(SELECT IVRMMD_Name FROM  IVRM_Master_District  PER WHERE IVRMMD_Id=AMST_PerDistrict ) END      +CASE WHEN AMST_PerState is null or AMST_PerState = '' then ''  ELSE ', ' + (SELECT IVRMMS_Name FROM IVRM_Master_State PER WHERE PER.IVRMMS_Id = AMST_PerState) END + CASE WHEN AMST_PerCountry is null or AMST_PerCountry = '' then ''  ELSE ', ' +(SELECT IVRMMC_CountryName FROM IVRM_Master_Country PER WHERE PER.IVRMMC_Id = AMST_PerCountry ) END + CASE WHEN CAST(amst_perpincode as varchar(max)) is null or CAST(amst_perpincode as varchar(max))= '' or CAST(amst_perpincode as varchar(max))= 0 then ''  ELSE '-' + CAST(amst_perpincode as varchar(max)) END,";
                            }
                            else
                            {
                                IVRM_CLM_coloumn += "AMST_PerAdd3 = CASE WHEN  REPLACE(AMST_PerStreet,',','') is null or REPLACE(AMST_PerStreet,',','')='' then '' else REPLACE(AMST_PerStreet,',','') end+ CASE WHEN  REPLACE(AMST_PerArea, ',', '') is null or REPLACE(AMST_PerArea,',', '')= '' then ''  ELSE ', ' + REPLACE(AMST_PerArea, ',', '') END +   CASE WHEN REPLACE(AMST_PerCity,',', '') is null or REPLACE(AMST_PerCity,',', '')= '' then ''  ELSE ', ' + REPLACE(AMST_PerCity, ',', '') END + CASE WHEN AMST_PerDistrict is null or AMST_PerDistrict = '' then ''  ELSE ', ' +(SELECT IVRMMD_Name FROM  IVRM_Master_District  PER WHERE IVRMMD_Id=AMST_PerDistrict ) END       + CASE WHEN AMST_PerState is null or AMST_PerState = '' then ''  ELSE ', ' +(SELECT IVRMMS_Name FROM IVRM_Master_State PER WHERE PER.IVRMMS_Id = AMST_PerState) END +CASE WHEN AMST_PerCountry is null or AMST_PerCountry = '' then ''  ELSE ', ' +(SELECT IVRMMC_CountryName FROM IVRM_Master_Country PER WHERE PER.IVRMMC_Id = AMST_PerCountry ) END + CASE WHEN CAST(amst_perpincode as varchar(max)) is null or CAST(amst_perpincode as varchar(max))= '' or CAST(amst_perpincode as varchar(max))= 0 then ''  ELSE '-' + CAST(amst_perpincode as varchar(max)) END,";
                            }
                            }
                        }

                        else if (name == "AMST_ConCity")
                        {
                            if (name == "AMST_ConCity")
                            {
                                if (IVRM_CLM_coloumn != "")
                                {
                                    IVRM_CLM_coloumn += "AMST_ConCity = CASE WHEN  REPLACE(AMST_ConStreet,',','') is null or REPLACE(AMST_ConStreet,',','')='' then '' else REPLACE(AMST_ConStreet,',','') end+CASE WHEN  REPLACE(AMST_ConArea, ',', '') is null or REPLACE(AMST_ConArea,',', '')= '' then ''  ELSE ', ' + REPLACE(AMST_ConArea, ',', '') END +     CASE WHEN REPLACE(AMST_ConCity,',', '') is null or REPLACE(AMST_ConCity,',', '')= '' then ''  ELSE ', ' + REPLACE(AMST_ConCity, ',', '') END + CASE WHEN AMST_PerDistrict is null or AMST_PerDistrict = '' then ''  ELSE ', ' +(SELECT IVRMMD_Name FROM  IVRM_Master_District  PER WHERE IVRMMD_Id=AMST_PerDistrict ) END   + CASE WHEN AMST_ConState is null or AMST_ConState = '' then ''  ELSE ', ' + (SELECT IVRMMS_Name FROM IVRM_Master_State PER WHERE PER.IVRMMS_Id = AMST_ConState) END +CASE WHEN AMST_ConCountry is null or AMST_ConCountry = '' then ''  ELSE ', ' + (SELECT IVRMMC_CountryName FROM IVRM_Master_Country PER WHERE PER.IVRMMC_Id = AMST_ConCountry ) END + CASE WHEN CAST(AMST_ConPincode as varchar(max)) is null or CAST(AMST_ConPincode as varchar(max))= '' or CAST(AMST_ConPincode as varchar(max))= 0 then ''  ELSE '-' + CAST(AMST_ConPincode as varchar(max)) END,";
                                }
                                else
                                {
                                    IVRM_CLM_coloumn += "AMST_ConCity = CASE WHEN  REPLACE(AMST_ConStreet,',','') is null or REPLACE(AMST_ConStreet,',','')='' then '' else REPLACE(AMST_ConStreet,',','') end+CASE WHEN  REPLACE(AMST_ConArea, ',', '') is null or REPLACE(AMST_ConArea,',', '')= '' then ''  ELSE ', ' + REPLACE(AMST_ConArea, ',', '') END +     CASE WHEN REPLACE(AMST_ConCity,',', '') is null or REPLACE(AMST_ConCity,',', '')= '' then ''  ELSE ', ' + REPLACE(AMST_ConCity, ',', '') END + CASE WHEN AMST_PerDistrict is null or AMST_PerDistrict = '' then ''  ELSE ', ' +(SELECT IVRMMD_Name FROM  IVRM_Master_District  PER WHERE IVRMMD_Id=AMST_PerDistrict ) END   + CASE WHEN AMST_ConState is null or AMST_ConState = '' then ''  ELSE ', ' + (SELECT IVRMMS_Name FROM IVRM_Master_State PER WHERE PER.IVRMMS_Id = AMST_ConState) END + CASE WHEN AMST_ConCountry is null or AMST_ConCountry = '' then ''  ELSE ', ' +(SELECT IVRMMC_CountryName FROM IVRM_Master_Country PER WHERE PER.IVRMMC_Id = AMST_ConCountry ) END +  CASE WHEN CAST(AMST_ConPincode as varchar(max)) is null or CAST(AMST_ConPincode as varchar(max))= '' or CAST(AMST_ConPincode as varchar(max))= 0 then ''  ELSE '-' + CAST(AMST_ConPincode as varchar(max)) END,";
                                }
                            }
                        }

                        else if (name == "AMST_MotherName")
                        {
                            if (name == "AMST_MotherName")
                            {
                                if (IVRM_CLM_coloumn != "")
                                {
                                    IVRM_CLM_coloumn += "AMST_MotherName = CASE WHEN AMST_MotherName ='NULL' or AMST_MotherName is null or AMST_MotherName=''  then '' else AMST_MotherName end+CASE WHEN AMST_MotherSurname ='NULL' or AMST_MotherSurname is null or AMST_MotherSurname = '' or AMST_MotherSurname = '0' then ''  ELSE ' ' + AMST_MotherSurname END,";
                                }
                                else
                                {
                                    IVRM_CLM_coloumn += "AMST_MotherName = CASE WHEN AMST_MotherName ='NULL' or AMST_MotherName is null or AMST_MotherName=''  then '' else AMST_MotherName end+CASE WHEN AMST_MotherSurname ='NULL' or AMST_MotherSurname is null or AMST_MotherSurname = '' or AMST_MotherSurname = '0' then ''  ELSE ' ' + AMST_MotherSurname END,";
                                }
                            }
                        }

                        else if (name == "AMST_FatherName")
                        {
                            if (name == "AMST_FatherName")
                            {
                                if (IVRM_CLM_coloumn != "")
                                {
                                    IVRM_CLM_coloumn += "AMST_FatherName = CASE WHEN AMST_FatherName ='NULL' or AMST_FatherName is null or AMST_FatherName=''  then '' else AMST_FatherName end+CASE WHEN AMST_FatherSurname ='NULL' or AMST_FatherSurname is null or AMST_FatherSurname = '' or AMST_FatherSurname = '0' then ''  ELSE ' ' + AMST_FatherSurname END,";
                                }
                                else
                                {
                                    IVRM_CLM_coloumn += "AMST_FatherName = CASE WHEN AMST_FatherName ='NULL' or AMST_FatherName is null or AMST_FatherName=''  then '' else AMST_FatherName end+CASE WHEN AMST_FatherSurname ='NULL' or AMST_FatherSurname is null or AMST_FatherSurname = '' or AMST_FatherSurname = '0' then ''  ELSE ' ' + AMST_FatherSurname END,";
                                }
                            }
                        }

                        else if (name == "AMST_MobileNo")
                        {
                            if (name == "AMST_MobileNo")
                            {
                                if (IVRM_CLM_coloumn != "")
                                {
                                    IVRM_CLM_coloumn += "stuff((select '/' +AMSTSMS_MobileNo  from dbo.Adm_Master_Student_SMSNo ff  where dbo.Adm_M_Student.AMST_Id=ff.AMST_Id for xml path('')),1,1,'')AMST_MobileNo,";
                                }
                                else
                                {
                                    IVRM_CLM_coloumn += "stuff((select '/' +AMSTSMS_MobileNo  from dbo.Adm_Master_Student_SMSNo ff  where dbo.Adm_M_Student.AMST_Id=ff.AMST_Id for xml path('')),1,1,'')AMST_MobileNo,";
                                }
                            }
                        }

                        else if (name == "AMST_emailId")
                        {
                            if (name == "AMST_emailId")
                            {
                                if (IVRM_CLM_coloumn != "")
                                {
                                    IVRM_CLM_coloumn += "stuff((select '/ ' +isnull(AMSTE_EmailId,'')    from dbo.Adm_Master_Student_EmailId gg  where dbo.Adm_M_Student.AMST_Id=gg.AMST_Id for xml path('')),1,1,'')AMST_emailId,";
                                }
                                else
                                {
                                    IVRM_CLM_coloumn += "stuff((select '/ ' +isnull(AMSTE_EmailId,'')    from dbo.Adm_Master_Student_EmailId gg  where dbo.Adm_M_Student.AMST_Id=gg.AMST_Id for xml path('')),1,1,'')AMST_emailId,";
                                }
                            }
                        }
                        //-----Father Details----------//
                        else if (name == "AMST_FatherMobleNo")
                        {
                            if (name == "AMST_FatherMobleNo")
                            {
                                if (IVRM_CLM_coloumn != "")
                                {
                                    IVRM_CLM_coloumn += "stuff((select '/ ' +cast(AMST_FatherMobile_No as varchar(30))  from dbo.Adm_Master_FatherMobileNo hh  where dbo.Adm_M_Student.AMST_Id=hh.AMST_Id for xml path('')),1,1,'')AMST_FatherMobleNo,";
                                }
                                else
                                {
                                    IVRM_CLM_coloumn += "stuff((select '/ ' +cast(AMST_FatherMobile_No as varchar(30))  from dbo.Adm_Master_FatherMobileNo hh  where dbo.Adm_M_Student.AMST_Id=hh.AMST_Id for xml path('')),1,1,'')AMST_FatherMobleNo,";
                                }
                            }
                        }

                        else if (name == "AMST_FatheremailId")
                        {
                            if (name == "AMST_FatheremailId")
                            {
                                if (IVRM_CLM_coloumn != "")
                                {
                                    IVRM_CLM_coloumn += "stuff((select '/ ' +AMST_FatheremailId  from dbo.Adm_Master_FatherEmail_Id ii   where dbo.Adm_M_Student.AMST_Id=ii.AMST_Id for xml path('')),1,1,'')AMST_FatheremailId,";
                                }
                                else
                                {
                                    IVRM_CLM_coloumn += "stuff((select '/ ' +AMST_FatheremailId  from dbo.Adm_Master_FatherEmail_Id ii   where dbo.Adm_M_Student.AMST_Id=ii.AMST_Id for xml path('')),1,1,'')AMST_FatheremailId,";
                                }
                            }
                        }

                        //------Mother Details---------//

                        else if (name == "AMST_MotherMobileNo")
                        {
                            if (name == "AMST_MotherMobileNo")
                            {
                                if (IVRM_CLM_coloumn != "")
                                {
                                    IVRM_CLM_coloumn += "stuff((select '/ ' +cast(AMST_MotherMobileNo as varchar(30))  from dbo.Adm_Master_MotherMobileNo jj where dbo.Adm_M_Student.AMST_Id=jj.AMST_Id for xml path('')),1,1,'')AMST_MotherMobileNo,";
                                }
                                else
                                {
                                    IVRM_CLM_coloumn += "stuff((select '/ ' +cast(AMST_MotherMobileNo as varchar(30))  from dbo.Adm_Master_MotherMobileNo jj where dbo.Adm_M_Student.AMST_Id=jj.AMST_Id for xml path('')),1,1,'')AMST_MotherMobileNo,";
                                }
                            }
                        }

                        else if (name == "AMST_MotherEmailId")
                        {
                            if (name == "AMST_MotherEmailId")
                            {
                                if (IVRM_CLM_coloumn != "")
                                {
                                    IVRM_CLM_coloumn += "stuff((select '/ ' +AMST_MotherEmailId from dbo.Adm_Master_MotherEmail_Id kk where dbo.Adm_M_Student.AMST_Id=kk.AMST_Id for xml path('')),1,1,'')AMST_MotherEmailId,";
                                }
                                else
                                {
                                    IVRM_CLM_coloumn += "stuff((select '/ ' +AMST_MotherEmailId from dbo.Adm_Master_MotherEmail_Id kk where dbo.Adm_M_Student.AMST_Id=kk.AMST_Id for xml path('')),1,1,'')AMST_MotherEmailId,";
                                }
                            }
                        }

                        else if (name == "AMST_DOB")
                        {
                            if (name == "AMST_DOB")
                            {
                                if (IVRM_CLM_coloumn != "")
                                {
                                    IVRM_CLM_coloumn += "(CONVERT(varchar(10), AMST_DOB, 103)) AMST_DOB,";
                                }
                                else
                                {
                                    IVRM_CLM_coloumn += "(CONVERT(varchar(10), AMST_DOB, 103)) AMST_DOB,";
                                }
                            }
                        }
                        else if (name == "AMST_Date")
                        {
                            if (name == "AMST_Date")
                            {
                                if (IVRM_CLM_coloumn != "")
                                {
                                    IVRM_CLM_coloumn += "(CONVERT(varchar(10), AMST_Date, 103)) AMST_Date,";
                                }
                                else
                                {
                                    IVRM_CLM_coloumn += "(CONVERT(varchar(10), AMST_Date, 103)) AMST_Date,";
                                }
                            }
                        }
                        else if (name == "JoinedYear")
                        {
                            if (name == "JoinedYear")
                            {
                                if (IVRM_CLM_coloumn != "")
                                {
                                    IVRM_CLM_coloumn += "(select a.ASMAY_Year from Adm_School_M_Academic_Year a where a.ASMAY_Id=Adm_M_Student.ASMAY_Id) as JoinedYear,";
                                }
                                else
                                {
                                    IVRM_CLM_coloumn += "(select a.ASMAY_Year from Adm_School_M_Academic_Year a where a.ASMAY_Id=Adm_M_Student.ASMAY_Id) as JoinedYear,";
                                }
                            }
                        }

                        else if (name == "AMST_PlaceOfBirthState")
                        {
                            if (name == "AMST_PlaceOfBirthState")
                            {
                                if (IVRM_CLM_coloumn != "")
                                {
                                    IVRM_CLM_coloumn += "(select a.IVRMMS_Name from IVRM_Master_State a where a.IVRMMS_Id=Adm_M_Student.AMST_PlaceOfBirthState) as AMST_PlaceOfBirthState,";
                                }
                                else
                                {
                                    IVRM_CLM_coloumn += "(select a.IVRMMS_Name from IVRM_Master_State a where a.IVRMMS_Id=Adm_M_Student.AMST_PlaceOfBirthState) as AMST_PlaceOfBirthState,";
                                }
                            }
                        }

                        else if (name == "AMST_PlaceOfBirthCountry")
                        {
                            if (name == "AMST_PlaceOfBirthCountry")
                            {
                                if (IVRM_CLM_coloumn != "")
                                {
                                    IVRM_CLM_coloumn += "(select a.IVRMMC_CountryName from IVRM_Master_Country a where a.IVRMMC_Id=Adm_M_Student.AMST_PlaceOfBirthCountry) as AMST_PlaceOfBirthCountry,";
                                }
                                else
                                {
                                    IVRM_CLM_coloumn += "(select a.IVRMMC_CountryName from IVRM_Master_Country a where a.IVRMMC_Id=Adm_M_Student.AMST_PlaceOfBirthCountry) as AMST_PlaceOfBirthCountry,";
                                }
                            }
                        }


                        else if (name == "JoinedClass")
                        {
                            if (name == "JoinedClass")
                            {
                                if (IVRM_CLM_coloumn != "")
                                {
                                    IVRM_CLM_coloumn += "(select n.ASMCL_ClassName from Adm_School_M_Class n where n.ASMCL_Id=Adm_M_Student.ASMCL_Id) as JoinedClass,";
                                }
                                else
                                {
                                    IVRM_CLM_coloumn += "(select n.ASMCL_ClassName from Adm_School_M_Class n where n.ASMCL_Id=Adm_M_Student.ASMCL_Id) as JoinedClass,";
                                }
                            }
                        }

                        else if (name == "AMSTPS_PrvTCDate")
                        {
                            if (name == "AMSTPS_PrvTCDate")
                            {
                                if (IVRM_CLM_coloumn != "")
                                {
                                    IVRM_CLM_coloumn += "(CONVERT(varchar(10), AMSTPS_PrvTCDate, 103)) AMSTPS_PrvTCDate,";
                                }
                                else
                                {
                                    IVRM_CLM_coloumn += "(CONVERT(varchar(10), AMSTPS_PrvTCDate, 103)) AMSTPS_PrvTCDate,";
                                }
                            }
                        }
                        else if (name == "SPCCMH_HouseName")
                        {
                            if (name == "SPCCMH_HouseName")
                            {
                                if (IVRM_CLM_coloumn != "")
                                {
                                    IVRM_CLM_coloumn += "SPCC_Master_House.SPCCMH_HouseName,";
                                }
                                else
                                {
                                    IVRM_CLM_coloumn += "SPCC_Master_House.SPCCMH_HouseName,";
                                }
                            }
                        }

                        else if (name == "Electives")
                        {
                            if (name == "Electives")
                            {
                                if (IVRM_CLM_coloumn != "")
                                {
                                    IVRM_CLM_coloumn += "(SELECT distinct STUFF((SELECT ', ' + CAST(ISMS_SubjectName AS VARCHAR(max)) [text()] FROM IVRM_Master_Subjects a inner join exm.Exm_Studentwise_Subjects b on a.ISMS_Id = b.ISMS_Id WHERE b.amst_id = dbo.Adm_School_Y_Student.amst_id  and b.ASMAY_Id=dbo.Adm_School_Y_Student.ASMAY_Id and b.ESTSU_ElecetiveFlag=1  and b.ASMCL_Id=dbo.Adm_School_Y_Student.ASMCL_Id and b.ASMS_Id=dbo.Adm_School_Y_Student.ASMS_Id FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)'),1,2,' ') List_Output FROM IVRM_Master_Subjects t) as Electives,";
                                }
                                else
                                {
                                    IVRM_CLM_coloumn += "(SELECT distinct STUFF((SELECT ', ' + CAST(ISMS_SubjectName AS VARCHAR(max)) [text()] FROM IVRM_Master_Subjects a inner join exm.Exm_Studentwise_Subjects b on a.ISMS_Id = b.ISMS_Id WHERE b.amst_id = dbo.Adm_School_Y_Student.amst_id  and b.ASMAY_Id=dbo.Adm_School_Y_Student.ASMAY_Id and b.ESTSU_ElecetiveFlag=1  and  b.ASMCL_Id=dbo.Adm_School_Y_Student.ASMCL_Id and b.ASMS_Id=dbo.Adm_School_Y_Student.ASMS_Id FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)'),1,2,' ') List_Output FROM IVRM_Master_Subjects t) as Electives,";
                                }
                            }
                        }

                        else
                        {
                            if (IVRM_CLM_coloumn == "")
                            {
                                IVRM_CLM_coloumn = name;
                            }
                            else
                            {
                                IVRM_CLM_coloumn = name + "," + IVRM_CLM_coloumn;
                            }
                        }
                    }
                }
                string coloumns = "";
                if (IVRM_CLM_coloumn.EndsWith(","))
                {
                    coloumns = IVRM_CLM_coloumn.Remove(IVRM_CLM_coloumn.Length - 1);
                }
                else
                {
                    coloumns = IVRM_CLM_coloumn;
                }


                if (data.AMC_Id == null || data.AMC_Id == 0)
                {
                    data.AMC_Id = 0;

                }
                var amcid = data.AMC_Id.ToString();

                // data.AMC_logo = _Classwisestudentdetailscontext.category.Where(p => p.AMC_Id == data.AMC_Id && p.MI_Id == data.mid && p.AMC_ActiveFlag == 1).Select(p => p.AMC_FilePath).ToArray();

                //var amc_ids = "0";
                //if (data.categorylistarray.Length > 0)
                //{
                //    foreach (var ue in data.categorylistarray)
                //    {
                //        amc_ids = amc_ids + "," + ue.AMC_Id;

                //    }

                //}




                var Classid = "0";
                if (data.classlsttwo != null && data.classlsttwo.Length > 0)
                {
                    foreach (var item in data.classlsttwo)
                    {

                        Classid = Classid + "," + item.ASMCL_Id;
                    }
                }

                var sectionid = "0";
                if (data.sectionlistarray != null && data.sectionlistarray.Length > 0)
                {
                    foreach (var item in data.sectionlistarray)
                    {

                        sectionid = sectionid + "," + item.ASMS_Id;
                    }
                }

                using (var cmd = _Classwisestudentdetailscontext.Database.GetDbConnection().CreateCommand())
                {
                    // Admission_classwisestudentdetails_category
                    cmd.CommandText = "Admission_classwisestudentdetails";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@year",
                    SqlDbType.VarChar)
                    {
                        Value = data.ASMAY_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@class",
                    SqlDbType.VarChar)
                    {
                        Value = Classid
                    });
                    cmd.Parameters.Add(new SqlParameter("@tablepara",
                    SqlDbType.VarChar)
                    {
                        Value = coloumns
                    });
                    cmd.Parameters.Add(new SqlParameter("@flag",
                    SqlDbType.VarChar)
                    {
                        Value = data.flag
                    });
                    cmd.Parameters.Add(new SqlParameter("@mi_id",
                    SqlDbType.VarChar)
                    {
                        Value = data.mid
                    });
                    cmd.Parameters.Add(new SqlParameter("@sec",
                    SqlDbType.VarChar)
                    {
                        Value = sectionid
                    });
                    if (cmd.Connection.State != ConnectionState.Open) cmd.Connection.Open();
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
                                    var datatype = dataReader.GetFieldType(iFiled);

                                    if (datatype.Name == "DateTime")
                                    {
                                        var dateval = (Convert.ToDateTime(dataReader[iFiled]).Date).ToString("d");
                                        string val = dataReader[iFiled].ToString();
                                        if (val == "")
                                        {
                                            dataRow.Add(dataReader.GetName(iFiled), "Not Available");
                                        }
                                        else
                                        {
                                            dataRow.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? "Not Available" : dateval);
                                        }
                                    }

                                    else
                                    {
                                        string val = dataReader[iFiled].ToString();
                                        if (val == "")
                                        {
                                            dataRow.Add(dataReader.GetName(iFiled), "Not Available");
                                        }
                                        else
                                        {
                                            dataRow.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? "Not Available" : dataReader[iFiled]);
                                        }
                                    }
                                }

                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.alldatagridreport = retObject.ToArray();
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
        //public async Task<ClasswisestudentdetailsDTO> Getreportdetails(ClasswisestudentdetailsDTO data)
        //{
        //    string IVRM_CLM_coloumn = "";
        //    string name = "";

        //    try
        //    {
        //        for (int i = 0; i < data.TempararyArrayheadList.Length; i++)
        //        {
        //            name = data.TempararyArrayheadList[i].columnID;
        //            if (name != null)
        //            {
        //                if (name == "AMST_FirstName")
        //                {
        //                    if (data.TempararyArrayheadList.Length == 1)
        //                    {
        //                        IVRM_CLM_coloumn = "AMST_FirstName = CASE WHEN  AMST_FirstName is null or AMST_FirstName=''  then '' else AMST_FirstName end+CASE WHEN  AMST_MiddleName is null or AMST_MiddleName = '' or AMST_MiddleName = '0' then ''  ELSE ' ' + AMST_MiddleName END +  CASE WHEN AMST_LastName is null or AMST_LastName = '' or AMST_LastName = '0' then ''  ELSE ' '  + AMST_LastName END,";
        //                    }

        //                    else
        //                    {
        //                        IVRM_CLM_coloumn = "AMST_FirstName = CASE WHEN  AMST_FirstName is null or AMST_FirstName=''  then '' else AMST_FirstName end+CASE WHEN  AMST_MiddleName is null or AMST_MiddleName = '' or AMST_MiddleName = '0' then ''  ELSE ' '  + AMST_MiddleName END +  CASE WHEN AMST_LastName is null or AMST_LastName = '' or AMST_LastName = '0' then ''  ELSE ' '  + AMST_LastName END,";
        //                    }
        //                }

        //                else if (name == "AMST_PerAdd3")
        //                {
        //                    if (IVRM_CLM_coloumn != "")
        //                    {
        //                        IVRM_CLM_coloumn += "AMST_PerAdd3 = CASE WHEN  REPLACE(AMST_PerStreet,',','') is null or REPLACE(AMST_PerStreet,',','')='' then '' else REPLACE(AMST_PerStreet,',','') end+ CASE WHEN  REPLACE(AMST_PerArea, ',', '') is null or REPLACE(AMST_PerArea,',', '')= '' then ''  ELSE ', ' + REPLACE(AMST_PerArea, ',', '') END +   CASE WHEN REPLACE(AMST_PerCity,',', '') is null or REPLACE(AMST_PerCity,',', '')= '' then ''  ELSE ', ' + REPLACE(AMST_PerCity, ',', '') END + CASE WHEN AMST_PerState is null or AMST_PerState = '' then ''  ELSE ', ' + (SELECT IVRMMS_Name FROM IVRM_Master_State PER WHERE PER.IVRMMS_Id = AMST_PerState) END + CASE WHEN AMST_PerCountry is null or AMST_PerCountry = '' then ''  ELSE ', ' +(SELECT IVRMMC_CountryName FROM IVRM_Master_Country PER WHERE PER.IVRMMC_Id = AMST_PerCountry ) END + CASE WHEN CAST(amst_perpincode as varchar(max)) is null or CAST(amst_perpincode as varchar(max))= '' or CAST(amst_perpincode as varchar(max))= 0 then ''  ELSE '-' + CAST(amst_perpincode as varchar(max)) END,";
        //                    }
        //                    else
        //                    {
        //                        IVRM_CLM_coloumn += "AMST_PerAdd3 = CASE WHEN  REPLACE(AMST_PerStreet,',','') is null or REPLACE(AMST_PerStreet,',','')='' then '' else REPLACE(AMST_PerStreet,',','') end+ CASE WHEN  REPLACE(AMST_PerArea, ',', '') is null or REPLACE(AMST_PerArea,',', '')= '' then ''  ELSE ', ' + REPLACE(AMST_PerArea, ',', '') END +   CASE WHEN REPLACE(AMST_PerCity,',', '') is null or REPLACE(AMST_PerCity,',', '')= '' then ''  ELSE ', ' + REPLACE(AMST_PerCity, ',', '') END +CASE WHEN AMST_PerState is null or AMST_PerState = '' then ''  ELSE ', ' +(SELECT IVRMMS_Name FROM IVRM_Master_State PER WHERE PER.IVRMMS_Id = AMST_PerState) END +CASE WHEN AMST_PerCountry is null or AMST_PerCountry = '' then ''  ELSE ', ' +(SELECT IVRMMC_CountryName FROM IVRM_Master_Country PER WHERE PER.IVRMMC_Id = AMST_PerCountry ) END + CASE WHEN CAST(amst_perpincode as varchar(max)) is null or CAST(amst_perpincode as varchar(max))= '' or CAST(amst_perpincode as varchar(max))= 0 then ''  ELSE '-' + CAST(amst_perpincode as varchar(max)) END,";
        //                    }
        //                }

        //                else if (name == "AMST_ConCity")
        //                {
        //                    if (name == "AMST_ConCity")
        //                    {
        //                        if (IVRM_CLM_coloumn != "")
        //                        {
        //                            IVRM_CLM_coloumn += "AMST_ConCity = CASE WHEN  REPLACE(AMST_ConStreet,',','') is null or REPLACE(AMST_ConStreet,',','')='' then '' else REPLACE(AMST_ConStreet,',','') end+CASE WHEN  REPLACE(AMST_ConArea, ',', '') is null or REPLACE(AMST_ConArea,',', '')= '' then ''  ELSE ', ' + REPLACE(AMST_ConArea, ',', '') END +     CASE WHEN REPLACE(AMST_ConCity,',', '') is null or REPLACE(AMST_ConCity,',', '')= '' then ''  ELSE ', ' + REPLACE(AMST_ConCity, ',', '') END + CASE WHEN AMST_ConState is null or AMST_ConState = '' then ''  ELSE ', ' + (SELECT IVRMMS_Name FROM IVRM_Master_State PER WHERE PER.IVRMMS_Id = AMST_ConState) END +CASE WHEN AMST_ConCountry is null or AMST_ConCountry = '' then ''  ELSE ', ' + (SELECT IVRMMC_CountryName FROM IVRM_Master_Country PER WHERE PER.IVRMMC_Id = AMST_ConCountry ) END + CASE WHEN CAST(AMST_ConPincode as varchar(max)) is null or CAST(AMST_ConPincode as varchar(max))= '' or CAST(AMST_ConPincode as varchar(max))= 0 then ''  ELSE '-' + CAST(AMST_ConPincode as varchar(max)) END,";
        //                        }
        //                        else
        //                        {
        //                            IVRM_CLM_coloumn += "AMST_ConCity = CASE WHEN  REPLACE(AMST_ConStreet,',','') is null or REPLACE(AMST_ConStreet,',','')='' then '' else REPLACE(AMST_ConStreet,',','') end+CASE WHEN  REPLACE(AMST_ConArea, ',', '') is null or REPLACE(AMST_ConArea,',', '')= '' then ''  ELSE ', ' + REPLACE(AMST_ConArea, ',', '') END +     CASE WHEN REPLACE(AMST_ConCity,',', '') is null or REPLACE(AMST_ConCity,',', '')= '' then ''  ELSE ', ' + REPLACE(AMST_ConCity, ',', '') END + CASE WHEN AMST_ConState is null or AMST_ConState = '' then ''  ELSE ', ' + (SELECT IVRMMS_Name FROM IVRM_Master_State PER WHERE PER.IVRMMS_Id = AMST_ConState) END + CASE WHEN AMST_ConCountry is null or AMST_ConCountry = '' then ''  ELSE ', ' +(SELECT IVRMMC_CountryName FROM IVRM_Master_Country PER WHERE PER.IVRMMC_Id = AMST_ConCountry ) END +  CASE WHEN CAST(AMST_ConPincode as varchar(max)) is null or CAST(AMST_ConPincode as varchar(max))= '' or CAST(AMST_ConPincode as varchar(max))= 0 then ''  ELSE '-' + CAST(AMST_ConPincode as varchar(max)) END,";
        //                        }
        //                    }
        //                }

        //                else if (name == "AMST_MotherName")
        //                {
        //                    if (name == "AMST_MotherName")
        //                    {
        //                        if (IVRM_CLM_coloumn != "")
        //                        {
        //                            IVRM_CLM_coloumn += "AMST_MotherName = CASE WHEN AMST_MotherName ='NULL' or AMST_MotherName is null or AMST_MotherName=''  then '' else AMST_MotherName end+CASE WHEN AMST_MotherSurname ='NULL' or AMST_MotherSurname is null or AMST_MotherSurname = '' or AMST_MotherSurname = '0' then ''  ELSE ' ' + AMST_MotherSurname END,";
        //                        }
        //                        else
        //                        {
        //                            IVRM_CLM_coloumn += "AMST_MotherName = CASE WHEN AMST_MotherName ='NULL' or AMST_MotherName is null or AMST_MotherName=''  then '' else AMST_MotherName end+CASE WHEN AMST_MotherSurname ='NULL' or AMST_MotherSurname is null or AMST_MotherSurname = '' or AMST_MotherSurname = '0' then ''  ELSE ' ' + AMST_MotherSurname END,";
        //                        }
        //                    }
        //                }

        //                else if (name == "AMST_FatherName")
        //                {
        //                    if (name == "AMST_FatherName")
        //                    {
        //                        if (IVRM_CLM_coloumn != "")
        //                        {
        //                            IVRM_CLM_coloumn += "AMST_FatherName = CASE WHEN AMST_FatherName ='NULL' or AMST_FatherName is null or AMST_FatherName=''  then '' else AMST_FatherName end+CASE WHEN AMST_FatherSurname ='NULL' or AMST_FatherSurname is null or AMST_FatherSurname = '' or AMST_FatherSurname = '0' then ''  ELSE ' ' + AMST_FatherSurname END,";
        //                        }
        //                        else
        //                        {
        //                            IVRM_CLM_coloumn += "AMST_FatherName = CASE WHEN AMST_FatherName ='NULL' or AMST_FatherName is null or AMST_FatherName=''  then '' else AMST_FatherName end+CASE WHEN AMST_FatherSurname ='NULL' or AMST_FatherSurname is null or AMST_FatherSurname = '' or AMST_FatherSurname = '0' then ''  ELSE ' ' + AMST_FatherSurname END,";
        //                        }
        //                    }
        //                }

        //                else if (name == "AMST_MobileNo")
        //                {
        //                    if (name == "AMST_MobileNo")
        //                    {
        //                        if (IVRM_CLM_coloumn != "")
        //                        {
        //                            IVRM_CLM_coloumn += "stuff((select '/' +AMSTSMS_MobileNo  from dbo.Adm_Master_Student_SMSNo ff  where dbo.Adm_M_Student.AMST_Id=ff.AMST_Id for xml path('')),1,1,'')AMST_MobileNo,";
        //                        }
        //                        else
        //                        {
        //                            IVRM_CLM_coloumn += "stuff((select '/' +AMSTSMS_MobileNo  from dbo.Adm_Master_Student_SMSNo ff  where dbo.Adm_M_Student.AMST_Id=ff.AMST_Id for xml path('')),1,1,'')AMST_MobileNo,";
        //                        }
        //                    }
        //                }

        //                else if (name == "AMST_emailId")
        //                {
        //                    if (name == "AMST_emailId")
        //                    {
        //                        if (IVRM_CLM_coloumn != "")
        //                        {
        //                            IVRM_CLM_coloumn += "stuff((select '/ ' +isnull(AMSTE_EmailId,'')    from dbo.Adm_Master_Student_EmailId gg  where dbo.Adm_M_Student.AMST_Id=gg.AMST_Id for xml path('')),1,1,'')AMST_emailId,";
        //                        }
        //                        else
        //                        {
        //                            IVRM_CLM_coloumn += "stuff((select '/ ' +isnull(AMSTE_EmailId,'')    from dbo.Adm_Master_Student_EmailId gg  where dbo.Adm_M_Student.AMST_Id=gg.AMST_Id for xml path('')),1,1,'')AMST_emailId,";
        //                        }
        //                    }
        //                }
        //                //-----Father Details----------//
        //                else if (name == "AMST_FatherMobleNo")
        //                {
        //                    if (name == "AMST_FatherMobleNo")
        //                    {
        //                        if (IVRM_CLM_coloumn != "")
        //                        {
        //                            IVRM_CLM_coloumn += "stuff((select '/ ' +cast(AMST_FatherMobile_No as varchar(30))  from dbo.Adm_Master_FatherMobileNo hh  where dbo.Adm_M_Student.AMST_Id=hh.AMST_Id for xml path('')),1,1,'')AMST_FatherMobleNo,";
        //                        }
        //                        else
        //                        {
        //                            IVRM_CLM_coloumn += "stuff((select '/ ' +cast(AMST_FatherMobile_No as varchar(30))  from dbo.Adm_Master_FatherMobileNo hh  where dbo.Adm_M_Student.AMST_Id=hh.AMST_Id for xml path('')),1,1,'')AMST_FatherMobleNo,";
        //                        }
        //                    }
        //                }

        //                else if (name == "AMST_FatheremailId")
        //                {
        //                    if (name == "AMST_FatheremailId")
        //                    {
        //                        if (IVRM_CLM_coloumn != "")
        //                        {
        //                            IVRM_CLM_coloumn += "stuff((select '/ ' +AMST_FatheremailId  from dbo.Adm_Master_FatherEmail_Id ii   where dbo.Adm_M_Student.AMST_Id=ii.AMST_Id for xml path('')),1,1,'')AMST_FatheremailId,";
        //                        }
        //                        else
        //                        {
        //                            IVRM_CLM_coloumn += "stuff((select '/ ' +AMST_FatheremailId  from dbo.Adm_Master_FatherEmail_Id ii   where dbo.Adm_M_Student.AMST_Id=ii.AMST_Id for xml path('')),1,1,'')AMST_FatheremailId,";
        //                        }
        //                    }
        //                }

        //                //------Mother Details---------//

        //                else if (name == "AMST_MotherMobileNo")
        //                {
        //                    if (name == "AMST_MotherMobileNo")
        //                    {
        //                        if (IVRM_CLM_coloumn != "")
        //                        {
        //                            IVRM_CLM_coloumn += "stuff((select '/ ' +cast(AMST_MotherMobileNo as varchar(30))  from dbo.Adm_Master_MotherMobileNo jj where dbo.Adm_M_Student.AMST_Id=jj.AMST_Id for xml path('')),1,1,'')AMST_MotherMobileNo,";
        //                        }
        //                        else
        //                        {
        //                            IVRM_CLM_coloumn += "stuff((select '/ ' +cast(AMST_MotherMobileNo as varchar(30))  from dbo.Adm_Master_MotherMobileNo jj where dbo.Adm_M_Student.AMST_Id=jj.AMST_Id for xml path('')),1,1,'')AMST_MotherMobileNo,";
        //                        }
        //                    }
        //                }

        //                else if (name == "AMST_MotherEmailId")
        //                {
        //                    if (name == "AMST_MotherEmailId")
        //                    {
        //                        if (IVRM_CLM_coloumn != "")
        //                        {
        //                            IVRM_CLM_coloumn += "stuff((select '/ ' +AMST_MotherEmailId from dbo.Adm_Master_MotherEmail_Id kk where dbo.Adm_M_Student.AMST_Id=kk.AMST_Id for xml path('')),1,1,'')AMST_MotherEmailId,";
        //                        }
        //                        else
        //                        {
        //                            IVRM_CLM_coloumn += "stuff((select '/ ' +AMST_MotherEmailId from dbo.Adm_Master_MotherEmail_Id kk where dbo.Adm_M_Student.AMST_Id=kk.AMST_Id for xml path('')),1,1,'')AMST_MotherEmailId,";
        //                        }
        //                    }
        //                }

        //                else if (name == "AMST_DOB")
        //                {
        //                    if (name == "AMST_DOB")
        //                    {
        //                        if (IVRM_CLM_coloumn != "")
        //                        {
        //                            IVRM_CLM_coloumn += "(CONVERT(varchar(10), AMST_DOB, 103)) AMST_DOB,";
        //                        }
        //                        else
        //                        {
        //                            IVRM_CLM_coloumn += "(CONVERT(varchar(10), AMST_DOB, 103)) AMST_DOB,";
        //                        }
        //                    }
        //                }
        //                else if (name == "AMST_Date")
        //                {
        //                    if (name == "AMST_Date")
        //                    {
        //                        if (IVRM_CLM_coloumn != "")
        //                        {
        //                            IVRM_CLM_coloumn += "(CONVERT(varchar(10), AMST_Date, 103)) AMST_Date,";
        //                        }
        //                        else
        //                        {
        //                            IVRM_CLM_coloumn += "(CONVERT(varchar(10), AMST_Date, 103)) AMST_Date,";
        //                        }
        //                    }
        //                }
        //                else if (name == "JoinedYear")
        //                {
        //                    if (name == "JoinedYear")
        //                    {
        //                        if (IVRM_CLM_coloumn != "")
        //                        {
        //                            IVRM_CLM_coloumn += "(select a.ASMAY_Year from Adm_School_M_Academic_Year a where a.ASMAY_Id=Adm_M_Student.ASMAY_Id) as JoinedYear,";
        //                        }
        //                        else
        //                        {
        //                            IVRM_CLM_coloumn += "(select a.ASMAY_Year from Adm_School_M_Academic_Year a where a.ASMAY_Id=Adm_M_Student.ASMAY_Id) as JoinedYear,";
        //                        }
        //                    }
        //                }

        //                else if (name == "AMST_PlaceOfBirthState")
        //                {
        //                    if (name == "AMST_PlaceOfBirthState")
        //                    {
        //                        if (IVRM_CLM_coloumn != "")
        //                        {
        //                            IVRM_CLM_coloumn += "(select a.IVRMMS_Name from IVRM_Master_State a where a.IVRMMS_Id=Adm_M_Student.AMST_PlaceOfBirthState) as AMST_PlaceOfBirthState,";
        //                        }
        //                        else
        //                        {
        //                            IVRM_CLM_coloumn += "(select a.IVRMMS_Name from IVRM_Master_State a where a.IVRMMS_Id=Adm_M_Student.AMST_PlaceOfBirthState) as AMST_PlaceOfBirthState,";
        //                        }
        //                    }
        //                }

        //                else if (name == "AMST_PlaceOfBirthCountry")
        //                {
        //                    if (name == "AMST_PlaceOfBirthCountry")
        //                    {
        //                        if (IVRM_CLM_coloumn != "")
        //                        {
        //                            IVRM_CLM_coloumn += "(select a.IVRMMC_CountryName from IVRM_Master_Country a where a.IVRMMC_Id=Adm_M_Student.AMST_PlaceOfBirthCountry) as AMST_PlaceOfBirthCountry,";
        //                        }
        //                        else
        //                        {
        //                            IVRM_CLM_coloumn += "(select a.IVRMMC_CountryName from IVRM_Master_Country a where a.IVRMMC_Id=Adm_M_Student.AMST_PlaceOfBirthCountry) as AMST_PlaceOfBirthCountry,";
        //                        }
        //                    }
        //                }


        //                else if (name == "JoinedClass")
        //                {
        //                    if (name == "JoinedClass")
        //                    {
        //                        if (IVRM_CLM_coloumn != "")
        //                        {
        //                            IVRM_CLM_coloumn += "(select n.ASMCL_ClassName from Adm_School_M_Class n where n.ASMCL_Id=Adm_M_Student.ASMCL_Id) as JoinedClass,";
        //                        }
        //                        else
        //                        {
        //                            IVRM_CLM_coloumn += "(select n.ASMCL_ClassName from Adm_School_M_Class n where n.ASMCL_Id=Adm_M_Student.ASMCL_Id) as JoinedClass,";
        //                        }
        //                    }
        //                }

        //                else if (name == "AMSTPS_PrvTCDate")
        //                {
        //                    if (name == "AMSTPS_PrvTCDate")
        //                    {
        //                        if (IVRM_CLM_coloumn != "")
        //                        {
        //                            IVRM_CLM_coloumn += "(CONVERT(varchar(10), AMSTPS_PrvTCDate, 103)) AMSTPS_PrvTCDate,";
        //                        }
        //                        else
        //                        {
        //                            IVRM_CLM_coloumn += "(CONVERT(varchar(10), AMSTPS_PrvTCDate, 103)) AMSTPS_PrvTCDate,";
        //                        }
        //                    }
        //                }

        //                else
        //                {
        //                    if (IVRM_CLM_coloumn == "")
        //                    {
        //                        IVRM_CLM_coloumn = name;
        //                    }
        //                    else
        //                    {
        //                        IVRM_CLM_coloumn = name + "," + IVRM_CLM_coloumn;
        //                    }
        //                }
        //            }
        //        }
        //        string coloumns = "";
        //        if (IVRM_CLM_coloumn.EndsWith(","))
        //        {
        //            coloumns = IVRM_CLM_coloumn.Remove(IVRM_CLM_coloumn.Length - 1);
        //        }
        //        else
        //        {
        //            coloumns = IVRM_CLM_coloumn;
        //        }


        //        if (data.AMC_Id == null || data.AMC_Id == 0)
        //        {
        //            data.AMC_Id = 0;

        //        }
        //        var amcid = data.AMC_Id.ToString();

        //       // data.AMC_logo = _Classwisestudentdetailscontext.category.Where(p => p.AMC_Id == data.AMC_Id && p.MI_Id == data.mid && p.AMC_ActiveFlag == 1).Select(p => p.AMC_FilePath).ToArray();

        //        //var amc_ids = "0";
        //        //if (data.categorylistarray.Length > 0)
        //        //{
        //        //    foreach (var ue in data.categorylistarray)
        //        //    {
        //        //        amc_ids = amc_ids + "," + ue.AMC_Id;

        //        //    }

        //        //}


        //        using (var cmd = _Classwisestudentdetailscontext.Database.GetDbConnection().CreateCommand())
        //        {
        //            // Admission_classwisestudentdetails_category
        //            cmd.CommandText = "Admission_classwisestudentdetails";
        //            cmd.CommandType = CommandType.StoredProcedure;
        //            cmd.Parameters.Add(new SqlParameter("@year", SqlDbType.VarChar) { Value = data.ASMAY_Id });
        //            cmd.Parameters.Add(new SqlParameter("@class", SqlDbType.VarChar) { Value = data.ASMCL_Id });
        //            cmd.Parameters.Add(new SqlParameter("@sec", SqlDbType.VarChar) { Value = data.ASMC_Id });
        //            //cmd.Parameters.Add(new SqlParameter("@allindiflag",//SqlDbType.VarChar)//{//    Value = data.allorindiflag//});
        //            cmd.Parameters.Add(new SqlParameter("@tablepara", SqlDbType.VarChar) { Value = coloumns });
        //            cmd.Parameters.Add(new SqlParameter("@flag", SqlDbType.VarChar) { Value = data.flag });
        //            cmd.Parameters.Add(new SqlParameter("@mi_id", SqlDbType.VarChar) { Value = data.mid });
        //           // cmd.Parameters.Add(new SqlParameter("@AMC_Id", SqlDbType.VarChar) { Value = data.AMC_Id });
        //            if (cmd.Connection.State != ConnectionState.Open) cmd.Connection.Open();
        //            var retObject = new List<dynamic>();

        //            try
        //            {
        //                using (var dataReader = await cmd.ExecuteReaderAsync())
        //                {
        //                    while (await dataReader.ReadAsync())
        //                    {
        //                        var dataRow = new ExpandoObject() as IDictionary<string, object>;
        //                        for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
        //                        {
        //                            var datatype = dataReader.GetFieldType(iFiled);

        //                            if (datatype.Name == "DateTime")
        //                            {
        //                                var dateval = (Convert.ToDateTime(dataReader[iFiled]).Date).ToString("d");
        //                                string val = dataReader[iFiled].ToString();
        //                                if (val == "")
        //                                {
        //                                    dataRow.Add(dataReader.GetName(iFiled), "Not Available");
        //                                }
        //                                else
        //                                {
        //                                    dataRow.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? "Not Available" : dateval);
        //                                }
        //                            }

        //                            else
        //                            {
        //                                string val = dataReader[iFiled].ToString();
        //                                if (val == "")
        //                                {
        //                                    dataRow.Add(dataReader.GetName(iFiled), "Not Available");
        //                                }
        //                                else
        //                                {
        //                                    dataRow.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? "Not Available" : dataReader[iFiled]);
        //                                }
        //                            }
        //                        }

        //                        retObject.Add((ExpandoObject)dataRow);
        //                    }
        //                }
        //                data.alldatagridreport = retObject.ToArray();
        //            }
        //            catch (Exception ex)
        //            {
        //                Console.WriteLine(ex.Message);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //    }
        //    return data;
        //}
        public ClasswisestudentdetailsDTO getsection(ClasswisestudentdetailsDTO data)
        {

            try
            {
                //data.ASMAY_Id = data.ASMC_Id;
                var rolet = _Classwisestudentdetailscontext.IVRM_Role_Type.Where(t => t.IVRMRT_Id == data.IVRMRT_Id).ToList();
                data.HRME_Id = 0;
                if (rolet.FirstOrDefault().IVRMRT_Role.Equals("ADMIN", StringComparison.OrdinalIgnoreCase)
                  || rolet.FirstOrDefault().IVRMRT_Role.Equals("Admission End User", StringComparison.OrdinalIgnoreCase)
                   || rolet.FirstOrDefault().IVRMRT_Role.Equals("COORDINATOR", StringComparison.OrdinalIgnoreCase)
                    || rolet.FirstOrDefault().IVRMRT_Role.Equals("END USER", StringComparison.OrdinalIgnoreCase)
                     || rolet.FirstOrDefault().IVRMRT_Role.Equals("Fee End User", StringComparison.OrdinalIgnoreCase)
                      || rolet.FirstOrDefault().IVRMRT_Role.Equals("Administrator", StringComparison.OrdinalIgnoreCase))
                {
                    data.HRME_Id = 0;
                }
                else
                {
                    var hrmeidcount = _Classwisestudentdetailscontext.Staff_User_Login.Where(c => c.Id == data.UserId && c.MI_Id == data.mid).Count();
                    if (hrmeidcount > 0)
                    {
                        data.HRME_Id = _Classwisestudentdetailscontext.Staff_User_Login.Single(c => c.Id == data.UserId && c.MI_Id == data.mid).Emp_Code;
                    }


                }
                string Class_Id = "0";
                //List<long> classid = new List<long>();
                //foreach (var e in data.classlsttwo)
                //{
                //    classid.Add(e.ASMCL_Id);
                //}


                //var sectiondata = (from a in _Classwisestudentdetailscontext.admissioncls
                //                   from b in _Classwisestudentdetailscontext.school_M_Section
                //                   from c in _Classwisestudentdetailscontext.AcademicYear
                //                   from d in _Classwisestudentdetailscontext.Masterclasscategory
                //                   from e in _Classwisestudentdetailscontext.AdmSchoolMasterClassCatSec
                //                   where (a.ASMCL_Id == d.ASMCL_Id && b.ASMS_Id == e.ASMS_Id && c.ASMAY_Id == d.ASMAY_Id && d.ASMCC_Id == e.ASMCC_Id
                //                   && d.MI_Id == data.mid && d.ASMAY_Id == data.ASMAY_Id && classid.Contains(d.ASMCL_Id) && e.ASMCCS_ActiveFlg == true)
                //                   select b
                //                 ).Distinct().OrderBy(g => g.ASMC_Order).ToArray();


                //if (sectiondata.Length > 0)
                //{
                //    data.fillsection = sectiondata.ToArray();
                //}
                //else
                //{
                //    List<School_M_Section> secname = new List<School_M_Section>();
                //    secname = _Classwisestudentdetailscontext.school_M_Section.Where(s => s.MI_Id == data.mid && s.ASMC_ActiveFlag == 1).OrderBy(s => s.ASMC_Order).ToList();
                //    data.fillsection = secname.ToArray();
                //}
                if (data.classlsttwo.Length > 0)
                {
                    for (var i = 0; i < data.classlsttwo.Length; i++)
                    {
                        Class_Id += "," + data.classlsttwo[i].ASMCL_Id;
                    }

                }

                using (var cmd = _Classwisestudentdetailscontext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "IVRM_StaffwiseSectionStdata_new";

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                SqlDbType.VarChar)
                    {
                        Value = data.mid
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                     SqlDbType.VarChar)
                    {
                        Value = data.ASMAY_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@UserId",
                SqlDbType.VarChar)
                    {
                        Value = data.UserId
                    });
                    //    cmd.Parameters.Add(new SqlParameter("@HRME_Id",
                    //SqlDbType.VarChar)
                    //    {
                    //        Value = data.HRME_Id
                    //    });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
              SqlDbType.VarChar)
                    {
                        Value = Class_Id
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
                        data.fillsection = retObject.ToArray();
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

        public ClasswisestudentdetailsDTO fetchclassbyYearId(ClasswisestudentdetailsDTO data)
        {

            try
            {
                //data.ASMAY_Id = data.ASMC_Id;

                var rolet = _Classwisestudentdetailscontext.IVRM_Role_Type.Where(t => t.IVRMRT_Id == data.IVRMRT_Id).ToList();
                data.HRME_Id = 0;
                if (rolet.FirstOrDefault().IVRMRT_Role.Equals("ADMIN", StringComparison.OrdinalIgnoreCase)
                 || rolet.FirstOrDefault().IVRMRT_Role.Equals("Admission End User", StringComparison.OrdinalIgnoreCase)
                  || rolet.FirstOrDefault().IVRMRT_Role.Equals("COORDINATOR", StringComparison.OrdinalIgnoreCase)
                   || rolet.FirstOrDefault().IVRMRT_Role.Equals("END USER", StringComparison.OrdinalIgnoreCase)
                    || rolet.FirstOrDefault().IVRMRT_Role.Equals("Fee End User", StringComparison.OrdinalIgnoreCase)
                     || rolet.FirstOrDefault().IVRMRT_Role.Equals("Administrator", StringComparison.OrdinalIgnoreCase))
                {
                    data.HRME_Id = 0;
                }
                else
                {
                    var hrmeidcount = _Classwisestudentdetailscontext.Staff_User_Login.Where(c => c.Id == data.UserId && c.MI_Id == data.mid).Count();
                    if (hrmeidcount > 0)
                    {
                        data.HRME_Id = _Classwisestudentdetailscontext.Staff_User_Login.Single(c => c.Id == data.UserId && c.MI_Id == data.mid).Emp_Code;
                    }


                }
                //IVRM_StaffwiseClassStdata

                using (var cmd = _Classwisestudentdetailscontext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "IVRM_StaffwiseClassStdata_new";

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                SqlDbType.BigInt)
                    {
                        Value = data.mid
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                     SqlDbType.BigInt)
                    {
                        Value = data.ASMAY_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@UserId",
                SqlDbType.BigInt)
                    {
                        Value = data.UserId
                    });
                    // cmd.Parameters.Add(new SqlParameter("@HRME_Id",
                    //SqlDbType.BigInt)
                    // {
                    //     Value = data.HRME_Id
                    // });
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
                        data.fillclass = retObject.ToArray();
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
