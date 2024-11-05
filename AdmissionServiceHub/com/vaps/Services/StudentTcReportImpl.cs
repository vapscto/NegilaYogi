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
using DomainModel.Model.com.vaps.Fee;
using DataAccessMsSqlServerProvider.com.vapstech.admission;
using Microsoft.AspNetCore.Identity;
using DomainModel.Model.com.vaps.admission;


namespace AdmissionServiceHub.com.vaps.Services
{
    public class StudentTcReportImpl : Interfaces.StudentTcReportInterface
    {
        public StudentTcReportContext _StudentTcReportContext;
        public StudentTcReportImpl(StudentTcReportContext frgContext)
        {
            _StudentTcReportContext = frgContext;
        }
        private readonly UserManager<ApplicationUser> _UserManager;
        public StudentTcReportImpl(StudentTcReportContext StudentTcReportContext, UserManager<ApplicationUser> UserManager)
        {
            _StudentTcReportContext = StudentTcReportContext;
            _UserManager = UserManager;
        }
        public StudentTcReportDTO getdetails(StudentTcReportDTO data)
        {

            try
            {
                List<MasterAcademic> year = new List<MasterAcademic>();

                year = _StudentTcReportContext.AcademicYear.Where(d => d.MI_Id == data.mid && d.Is_Active == true).OrderByDescending(a => a.ASMAY_Order).ToList();
                data.fillyear = year.ToArray();

                List<School_M_Class> classname = new List<School_M_Class>();
                classname = _StudentTcReportContext.admissioncls.Where(d => d.MI_Id == data.mid && d.ASMCL_ActiveFlag == true).OrderBy(c => c.ASMCL_Order).ToList();

                data.fillclass = classname.ToArray();



                List<School_M_Section> secname = new List<School_M_Section>();

                secname = _StudentTcReportContext.school_M_Section.Where(d => d.MI_Id == data.mid && d.ASMC_ActiveFlag == 1).OrderBy(s => s.ASMC_Order).ToList();

                data.fillsection = secname.ToArray();

                var cat = _StudentTcReportContext.GenConfig.Where(g => g.MI_Id == data.mid && g.IVRMGC_CatLogoFlg == true).ToList();
                if (cat.Count > 0)
                {
                    data.category_list = _StudentTcReportContext.category.Where(f => f.MI_Id == data.mid && f.AMC_ActiveFlag == 1).ToArray();
                    data.categoryflag = true;
                }
                else
                {
                    data.categoryflag = false;
                }

                //List<FeeGroupDMO> feegrp = new List<FeeGroupDMO>();
                //feegrp = _FeeGroupContext.feeGroup.ToList();
                //data.fillfeegroup = feegrp.ToArray();


                //List<FeeHeadDMO> feelisthead = new List<FeeHeadDMO>();
                //feelisthead = _FeeGroupContext.feehead.ToList();
                //data.fillfeehead = feelisthead.ToArray();

                // var SourceIds = mas.SelectedSourceDetails.Select(d => d.PAMS_Id).ToArray();

                //   var SourceIds = mas.TempararyArrayListstring.Select(d => d.FMG_Id).ToArray();

                //  !SourceIds.Contains(t.PAMS_Id);


            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public StudentTcReportDTO Getdata1(StudentTcReportDTO dto)
        {


            //DateTime fromdate = new DateTime();
            //DateTime todate = new DateTime();
            try
            {
                if (dto.tcallorindi == "all")
                {
                    if (dto.tcperortemp == "PTC")
                    {
                        dto.alldatagridreport = (from a in _StudentTcReportContext.AcademicYear
                                                 from b in _StudentTcReportContext.school_M_Section
                                                 from c in _StudentTcReportContext.admissioncls
                                                 from d in _StudentTcReportContext.AdmissionStudent
                                                 from e in _StudentTcReportContext.studenttc

                                                 where (c.ASMCL_Id == e.ASMCL_Id && e.ASMS_Id == b.ASMS_Id && e.AMST_Id == d.AMST_Id && e.MI_Id == a.MI_Id
                                                 && e.ASMAY_Id == a.ASMAY_Id && a.ASMAY_Id == dto.ASMAY_Id)
                                                 select new StudentTcReportDTO
                                                 {
                                                     ASTC_TCNO = e.ASTC_TCNO,
                                                     AMST_RegistrationNo = d.AMST_RegistrationNo,
                                                     AMST_FirstName = d.AMST_FirstName,
                                                     ASMCL_ClassName = c.ASMCL_ClassName,
                                                     ASMC_SectionName = b.ASMC_SectionName,
                                                     ASTC_LeavingReason = e.ASTC_LeavingReason,
                                                     ASTC_Remarks = e.ASTC_Remarks,
                                                     AMST_AdmNo = d.AMST_AdmNo,
                                                     //AMST_Date=d.AMST_Date,
                                                     //ASTC_TCIssueDate=e.ASTC_TCIssueDate,
                                                     //AMST_FatherName=d.AMST_FatherName,
                                                     //AMST_MotherName=d.AMST_MotherName,
                                                     //AMST_MobileNo=d.AMST_MobileNo,
                                                     //AMST_emailId=d.AMST_emailId,
                                                     //AMST_DOB=d.AMST_DOB,
                                                     AMST_PerCity = d.AMST_PerCity,
                                                     AMST_PerAdd3 = d.AMST_PerAdd3,
                                                     AMST_BPLCardNo = d.AMST_BPLCardNo,
                                                 }).Distinct().ToArray();
                    }
                    else
                    {
                        dto.alldatagridreport = (from a in _StudentTcReportContext.AcademicYear
                                                 from b in _StudentTcReportContext.school_M_Section
                                                 from c in _StudentTcReportContext.admissioncls
                                                 from d in _StudentTcReportContext.AdmissionStudent
                                                 from e in _StudentTcReportContext.studenttc

                                                 where (c.ASMCL_Id == e.ASMCL_Id && e.ASMS_Id == b.ASMS_Id && e.AMST_Id == d.AMST_Id && e.MI_Id == a.MI_Id
                                                 && e.ASMAY_Id == a.ASMAY_Id && a.ASMAY_Id == dto.ASMAY_Id)
                                                 select new StudentTcReportDTO
                                                 {
                                                     ASTC_TCNO = e.ASTC_TCNO,
                                                     AMST_RegistrationNo = d.AMST_RegistrationNo,
                                                     AMST_FirstName = d.AMST_FirstName,
                                                     ASMCL_ClassName = c.ASMCL_ClassName,
                                                     ASMC_SectionName = b.ASMC_SectionName,
                                                     ASTC_LeavingReason = e.ASTC_LeavingReason,
                                                     ASTC_Remarks = e.ASTC_Remarks,
                                                     AMST_AdmNo = d.AMST_AdmNo,
                                                     //AMST_Date = d.AMST_Date,
                                                     //ASTC_TCIssueDate = e.ASTC_TCIssueDate,
                                                     //AMST_FatherName = d.AMST_FatherName,
                                                     //AMST_MotherName = d.AMST_MotherName,
                                                     //AMST_MobileNo = d.AMST_MobileNo,
                                                     //AMST_emailId = d.AMST_emailId,
                                                     //AMST_DOB = d.AMST_DOB,
                                                     AMST_PerCity = d.AMST_PerCity,
                                                     AMST_PerAdd3 = d.AMST_PerAdd3,
                                                     AMST_BPLCardNo = d.AMST_BPLCardNo,
                                                 }).Distinct().ToArray();
                    }

                }
                else
                {
                    if (dto.tcperortemp == "PTC")
                    {
                        dto.alldatagridreport = (from a in _StudentTcReportContext.AcademicYear
                                                 from b in _StudentTcReportContext.school_M_Section
                                                 from c in _StudentTcReportContext.admissioncls
                                                 from d in _StudentTcReportContext.AdmissionStudent
                                                 from e in _StudentTcReportContext.studenttc

                                                 where (c.ASMCL_Id == e.ASMCL_Id && e.ASMS_Id == b.ASMS_Id && e.AMST_Id == d.AMST_Id && e.MI_Id == a.MI_Id
                                                 && e.ASMAY_Id == a.ASMAY_Id && a.ASMAY_Id == dto.ASMAY_Id && c.ASMCL_Id == dto.ASMCL_Id && b.ASMS_Id == dto.ASMC_Id)
                                                 select new StudentTcReportDTO
                                                 {
                                                     ASTC_TCNO = e.ASTC_TCNO,
                                                     AMST_RegistrationNo = d.AMST_RegistrationNo,
                                                     AMST_FirstName = d.AMST_FirstName,
                                                     ASMCL_ClassName = c.ASMCL_ClassName,
                                                     ASMC_SectionName = b.ASMC_SectionName,
                                                     ASTC_LeavingReason = e.ASTC_LeavingReason,
                                                     ASTC_Remarks = e.ASTC_Remarks,
                                                     AMST_AdmNo = d.AMST_AdmNo,
                                                     //AMST_Date = d.AMST_Date,
                                                     //ASTC_TCIssueDate = e.ASTC_TCIssueDate,
                                                     //AMST_FatherName = d.AMST_FatherName,
                                                     //AMST_MotherName = d.AMST_MotherName,
                                                     //AMST_MobileNo = d.AMST_MobileNo,
                                                     //AMST_emailId = d.AMST_emailId,
                                                     //AMST_DOB = d.AMST_DOB,
                                                     AMST_PerCity = d.AMST_PerCity,
                                                     AMST_PerAdd3 = d.AMST_PerAdd3,
                                                     AMST_BPLCardNo = d.AMST_BPLCardNo,
                                                 }).Distinct().ToArray();
                    }
                    else
                    {
                        dto.alldatagridreport = (from a in _StudentTcReportContext.AcademicYear
                                                 from b in _StudentTcReportContext.school_M_Section
                                                 from c in _StudentTcReportContext.admissioncls
                                                 from d in _StudentTcReportContext.AdmissionStudent
                                                 from e in _StudentTcReportContext.studenttc

                                                 where (c.ASMCL_Id == e.ASMCL_Id && e.ASMS_Id == b.ASMS_Id && e.AMST_Id == d.AMST_Id && e.MI_Id == a.MI_Id
                                                 && e.ASMAY_Id == a.ASMAY_Id && a.ASMAY_Id == dto.ASMAY_Id && c.ASMCL_Id == dto.ASMCL_Id && b.ASMS_Id == dto.ASMC_Id)
                                                 select new StudentTcReportDTO
                                                 {
                                                     ASTC_TCNO = e.ASTC_TCNO,
                                                     AMST_RegistrationNo = d.AMST_RegistrationNo,
                                                     AMST_FirstName = d.AMST_FirstName,
                                                     ASMCL_ClassName = c.ASMCL_ClassName,
                                                     ASMC_SectionName = b.ASMC_SectionName,
                                                     ASTC_LeavingReason = e.ASTC_LeavingReason,
                                                     ASTC_Remarks = e.ASTC_Remarks,
                                                     AMST_AdmNo = d.AMST_AdmNo,

                                                     AMST_PerCity = d.AMST_PerCity,
                                                     AMST_PerAdd3 = d.AMST_PerAdd3,
                                                     AMST_BPLCardNo = d.AMST_BPLCardNo,
                                                 }).Distinct().ToArray();
                    }

                }


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return dto;
        }
        public async Task<StudentTcReportDTO> Getdata(StudentTcReportDTO dto)
        {
            List<string> headId = new List<string>();
            string IVRM_CLM_coloumn = "";
            string name = "";
            try
            {
                for (int i = 0; i < dto.TempararyArrayheadList.Length; i++)
                {
                    string Id = dto.TempararyArrayheadList[i].columnID;
                    if (Id != null)
                    {
                        name = Id;
                        if (name == "AMST_FirstName")
                        {
                            if (dto.TempararyArrayheadList.Length == 1)
                            {
                                name = "AMST_FirstName = CASE WHEN  AMST_FirstName is null or AMST_FirstName=''  then '' else AMST_FirstName end+CASE WHEN  AMST_MiddleName is null or AMST_MiddleName = '' or AMST_MiddleName = '0' then ''  ELSE ' ' + AMST_MiddleName END +  CASE WHEN AMST_LastName is null or AMST_LastName = '' or AMST_LastName = '0' then ''  ELSE ' ' + AMST_LastName END ";
                            }

                            else
                            {
                                name = "AMST_FirstName = CASE WHEN  AMST_FirstName is null or AMST_FirstName=''  then '' else AMST_FirstName end+CASE WHEN  AMST_MiddleName is null or AMST_MiddleName = '' or AMST_MiddleName = '0' then ''  ELSE ' ' + AMST_MiddleName END +  CASE WHEN AMST_LastName is null or AMST_LastName = '' or AMST_LastName = '0' then ''  ELSE ' ' + AMST_LastName END";

                            }
                        }
                        if (name == "AMST_PerAdd3")
                        {

                            if (IVRM_CLM_coloumn != "")
                            {
                                name = "AMST_PerAdd3 = CASE WHEN  REPLACE(AMST_PerStreet,',','') is null or REPLACE(AMST_PerStreet,',','')='' then '' else REPLACE(AMST_PerStreet,',','') end+ CASE WHEN  REPLACE(AMST_PerArea, ',', '') is null or REPLACE(AMST_PerArea,',', '')= '' then ''  ELSE ', ' + REPLACE(AMST_PerArea, ',', '') END +   CASE WHEN REPLACE(AMST_PerCity,',', '') is null or REPLACE(AMST_PerCity,',', '')= '' then ''  ELSE ', ' + REPLACE(AMST_PerCity, ',', '') END +         CASE WHEN IVRMMS_Name is null or IVRMMS_Name = '' then ''  ELSE ', ' + IVRMMS_Name END +          CASE WHEN IVRMMC_CountryName is null or IVRMMC_CountryName = '' then ''  ELSE ', ' + IVRMMC_CountryName END +           CASE WHEN CAST(amst_perpincode as varchar(max)) is null or CAST(amst_perpincode as varchar(max))= '' or CAST(amst_perpincode as varchar(max))= 0 then ''  ELSE '-' + CAST(amst_perpincode as varchar(max)) END";

                            }
                            else if (IVRM_CLM_coloumn == "" && dto.TempararyArrayheadList.Length > 1)
                            {
                                name = "AMST_PerAdd3 = CASE WHEN  REPLACE(AMST_PerStreet,',','') is null or REPLACE(AMST_PerStreet,',','')='' then '' else REPLACE(AMST_PerStreet,',','') end+ CASE WHEN  REPLACE(AMST_PerArea, ',', '') is null or REPLACE(AMST_PerArea,',', '')= '' then ''  ELSE ', ' + REPLACE(AMST_PerArea, ',', '') END +   CASE WHEN REPLACE(AMST_PerCity,',', '') is null or REPLACE(AMST_PerCity,',', '')= '' then ''  ELSE ', ' + REPLACE(AMST_PerCity, ',', '') END +         CASE WHEN IVRMMS_Name is null or IVRMMS_Name = '' then ''  ELSE ', ' + IVRMMS_Name END +          CASE WHEN IVRMMC_CountryName is null or IVRMMC_CountryName = '' then ''  ELSE ', ' + IVRMMC_CountryName END +           CASE WHEN CAST(amst_perpincode as varchar(max)) is null or CAST(amst_perpincode as varchar(max))= '' or CAST(amst_perpincode as varchar(max))= 0 then ''  ELSE '-' + CAST(amst_perpincode as varchar(max)) END";
                            }
                            else
                            {
                                name = "AMST_PerAdd3 = CASE WHEN  REPLACE(AMST_PerStreet,',','') is null or REPLACE(AMST_PerStreet,',','')='' then '' else REPLACE(AMST_PerStreet,',','') end+ CASE WHEN  REPLACE(AMST_PerArea, ',', '') is null or REPLACE(AMST_PerArea,',', '')= '' then ''  ELSE ', ' + REPLACE(AMST_PerArea, ',', '') END +   CASE WHEN REPLACE(AMST_PerCity,',', '') is null or REPLACE(AMST_PerCity,',', '')= '' then ''  ELSE ', ' + REPLACE(AMST_PerCity, ',', '') END +         CASE WHEN IVRMMS_Name is null or IVRMMS_Name = '' then ''  ELSE ', ' + IVRMMS_Name END +          CASE WHEN IVRMMC_CountryName is null or IVRMMC_CountryName = '' then ''  ELSE ', ' + IVRMMC_CountryName END +           CASE WHEN CAST(amst_perpincode as varchar(max)) is null or CAST(amst_perpincode as varchar(max))= '' or CAST(amst_perpincode as varchar(max))= 0 then ''  ELSE '-' + CAST(amst_perpincode as varchar(max)) END";

                            }
                        }


                        if (name == "AMST_ConCity")
                        {

                            if (name != "")
                            {
                                name = "AMST_ConCity = CASE WHEN  REPLACE(AMST_ConStreet,',','') is null or REPLACE(AMST_ConStreet,',','')='' then '' else REPLACE(AMST_ConStreet,',','') end+CASE WHEN  REPLACE(AMST_ConArea, ',', '') is null or REPLACE(AMST_ConArea,',', '')= '' then ''  ELSE ', ' + REPLACE(AMST_ConArea, ',', '') END +     CASE WHEN REPLACE(AMST_ConCity,',', '') is null or REPLACE(AMST_ConCity,',', '')= '' then ''  ELSE ', ' + REPLACE(AMST_ConCity, ',', '') END +         CASE WHEN IVRMMS_Name is null or IVRMMS_Name = '' then ''  ELSE ', ' + IVRMMS_Name END +          CASE WHEN IVRMMC_CountryName is null or IVRMMC_CountryName = '' then ''  ELSE ', ' + IVRMMC_CountryName END +           CASE WHEN CAST(AMST_ConPincode as varchar(max)) is null or CAST(AMST_ConPincode as varchar(max))= '' or CAST(AMST_ConPincode as varchar(max))= 0 then ''  ELSE '-' + CAST(AMST_ConPincode as varchar(max)) END";
                            }
                            else
                            {
                                name = "AMST_ConCity = CASE WHEN  REPLACE(AMST_ConStreet,',','') is null or REPLACE(AMST_ConStreet,',','')='' then '' else REPLACE(AMST_ConStreet,',','') end+CASE WHEN  REPLACE(AMST_ConArea, ',', '') is null or REPLACE(AMST_ConArea,',', '')= '' then ''  ELSE ', ' + REPLACE(AMST_ConArea, ',', '') END +     CASE WHEN REPLACE(AMST_ConCity,',', '') is null or REPLACE(AMST_ConCity,',', '')= '' then ''  ELSE ', ' + REPLACE(AMST_ConCity, ',', '') END +         CASE WHEN IVRMMS_Name is null or IVRMMS_Name = '' then ''  ELSE ', ' + IVRMMS_Name END +          CASE WHEN IVRMMC_CountryName is null or IVRMMC_CountryName = '' then ''  ELSE ', ' + IVRMMC_CountryName END +           CASE WHEN CAST(AMST_ConPincode as varchar(max)) is null or CAST(AMST_ConPincode as varchar(max))= '' or CAST(AMST_ConPincode as varchar(max))= 0 then ''  ELSE '-' + CAST(AMST_ConPincode as varchar(max)) END";
                            }

                        }

                        if (name == "ASTC_ASMCL_ID")
                        {
                            if (name != "")
                            {
                                name = "(select cls.ASMCL_ClassName from Adm_School_M_Class cls where cls.asmcl_id=adm.asmcl_id)  as  ASTC_ASMCL_ID";
                            }
                            else
                            {
                                name = "(select cls.ASMCL_ClassName from Adm_School_M_Class cls where cls.asmcl_id=adm.asmcl_id)  as  ASTC_ASMCL_ID";
                            }
                        }

                        if (name == "ASTC_TCDate")
                        {
                            if (name != "")
                            {
                                name = "convert(varchar(10),ASTC_TCDate,103) as  ASTC_TCDate";
                            }
                            else
                            {
                                name = "convert(varchar(10),ASTC_TCDate,103) as  ASTC_TCDate";
                            }
                        }

                        if (name == "ASTC_TCIssueDate")
                        {
                            if (name != "")
                            {
                                name = "convert(varchar(10),ASTC_TCIssueDate,103) as  ASTC_TCIssueDate";
                            }
                            else
                            {
                                name = "convert(varchar(10),ASTC_TCIssueDate,103) as  ASTC_TCIssueDate";
                            }
                        }

                        if (name == "AMST_Date")
                        {
                            if (name != "")
                            {
                                name = "convert(varchar(10),AMST_Date,103) as  AMST_Date";
                            }
                            else
                            {
                                name = "convert(varchar(10),AMST_Date,103) as  AMST_Date";
                            }
                        }
                        if (name == "ASTC_TCApplicationDate")
                        {
                            if (name != "")
                            {
                                name = "convert(varchar(10),ASTC_TCApplicationDate,103) as  ASTC_TCApplicationDate";
                            }
                            else
                            {
                                name = "convert(varchar(10),ASTC_TCApplicationDate,103) as  ASTC_TCApplicationDate";
                            }
                        }





                        IVRM_CLM_coloumn = name + "," + IVRM_CLM_coloumn;
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

                if (dto.AMC_Id == null || dto.AMC_Id == 0)
                {
                    dto.AMC_Id = 0;

                }
                var amcid = dto.AMC_Id.ToString();

                dto.AMC_logo = _StudentTcReportContext.category.Where(p => p.AMC_Id == dto.AMC_Id && p.MI_Id == dto.mid && p.AMC_ActiveFlag == 1).Select(p => p.AMC_FilePath).ToArray();

                //var amc_ids = "0";
                //if (dto.categorylistarray.Length > 0)
                //{
                //    foreach (var ue in dto.categorylistarray)
                //    {
                //        amc_ids = amc_ids + "," + ue.AMC_Id;

                //    }

                //}

                using (var cmd = _StudentTcReportContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Admission_Tc_Report";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@tableparam", SqlDbType.VarChar) { Value = coloumns });
                    cmd.Parameters.Add(new SqlParameter("@Asmayid", SqlDbType.VarChar) { Value = dto.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@asmclid", SqlDbType.VarChar) { Value = dto.ASMCL_Id });
                    cmd.Parameters.Add(new SqlParameter("@asmcid", SqlDbType.VarChar) { Value = dto.ASMC_Id });
                    cmd.Parameters.Add(new SqlParameter("@allorind", SqlDbType.VarChar) { Value = dto.tcallorindi });
                    cmd.Parameters.Add(new SqlParameter("@PermOrtemp", SqlDbType.VarChar) { Value = dto.tcperortemp });
                    cmd.Parameters.Add(new SqlParameter("@mid", SqlDbType.VarChar) { Value = dto.mid });
                    cmd.Parameters.Add(new SqlParameter("@AMC_Id", SqlDbType.VarChar) { Value = dto.AMC_Id });
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
                        dto.alldatagridreport = retObject.ToArray();
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
            return dto;
        }
        public StudentTcReportDTO getsecton(StudentTcReportDTO data)
        {
            try
            {
                var sectiondata = (from a in _StudentTcReportContext.admissioncls
                                   from b in _StudentTcReportContext.school_M_Section
                                   from c in _StudentTcReportContext.AcademicYear
                                   from d in _StudentTcReportContext.Masterclasscategory
                                   from e in _StudentTcReportContext.AdmSchoolMasterClassCatSec
                                   where (a.ASMCL_Id == d.ASMCL_Id && b.ASMS_Id == e.ASMS_Id && c.ASMAY_Id == d.ASMAY_Id && d.ASMCC_Id == e.ASMCC_Id
                                   && d.MI_Id == data.mid && d.ASMAY_Id == data.ASMAY_Id && d.ASMCL_Id == data.ASMCL_Id && e.ASMCCS_ActiveFlg == true)
                                   select b).Distinct().OrderBy(g => g.ASMC_Order).ToArray();


                if (sectiondata.Length > 0)
                {
                    data.fillsection = sectiondata.ToArray();
                }
                else
                {
                    List<School_M_Section> secname = new List<School_M_Section>();
                    secname = _StudentTcReportContext.school_M_Section.Where(s => s.MI_Id == data.mid && s.ASMC_ActiveFlag == 1).OrderBy(s => s.ASMC_Order).ToList();
                    data.fillsection = secname.ToArray();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }


        public StudentTcReportDTO getclass(StudentTcReportDTO data)
        {
            try
            {

                using (var cmd = _StudentTcReportContext.Database.GetDbConnection().CreateCommand())
                {
                    //AlumnistudentsearchReport_new
                    //AlumnistudentsearchReport
                    cmd.CommandText = "Total_strength_class";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.BigInt) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@AMC_Id", SqlDbType.BigInt) { Value = data.AMC_Id });
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = data.mid });
                    cmd.Parameters.Add(new SqlParameter("@Emp_Id", SqlDbType.BigInt) { Value = 0 });

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
                        //if (data.fillclass.Length > 0)
                        //{
                        //    data.count = data.fillclass.Length;
                        //}
                        //else
                        //{
                        //    data.count = 0;
                        //}
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }





                //if (sectiondata.Length > 0)
                //{
                //    data.AllSection = sectiondata.ToArray();
                //}
                //else
                //{
                //    List<School_M_Section> secname = new List<School_M_Section>();
                //    secname = _ActivateDeactivateContext.masterSection.Where(s => s.MI_Id == data.MI_Id && s.ASMC_ActiveFlag == 1).OrderBy(s => s.ASMC_Order).ToList();
                //    data.AllSection = secname.ToArray();
                //}

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
    }
}
