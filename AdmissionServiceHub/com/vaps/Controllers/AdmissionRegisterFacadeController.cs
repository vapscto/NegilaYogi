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



// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace AdmissionServiceHub.com.vaps.Interfaces
{
    [Route("api/[controller]")]

    public class AdmissionRegisterFacadeController : Controller
    {
        string IVRM_CLM_coloumn = "";
        private readonly DomainModelMsSqlServerContext _db;       
        public AdmissionRegisterInterface _Clswisedailyatt;
        public AdmissionRegisterFacadeController(AdmissionRegisterInterface clswisedailyatt, DomainModelMsSqlServerContext db)
        {
            _Clswisedailyatt = clswisedailyatt;
            _db = db;
        }


        //    [HttpGet]

        [Route("Getdetails")]
        public SchoolYearWiseStudentDTO Getdetails([FromBody]SchoolYearWiseStudentDTO castecategoryDTO)//int IVRMM_Id
        {

            return _Clswisedailyatt.GetddlDatabind(castecategoryDTO);

        }

        //[HttpGet]
        [HttpPost]
        [Route("Getdetailsreport/")]
        public async Task<SchoolYearWiseStudentDTO> Getdetailsreport([FromBody] SchoolYearWiseStudentDTO reg)
        {

            List<long> clsId = new List<long>();

            foreach (var item in reg.TempararyArrayListclass)
            {
                clsId.Add(item.ASMCL_Id);
            }           

            string Ids = "";
            for (int i = 0; i < reg.TempararyArrayListclass.Length; i++)
            {
                string Id = reg.TempararyArrayListclass[i].ASMCL_Id.ToString();
                if (Id != null)
                {
                    if (i == 0)
                    {
                        Ids = Id;
                    }
                    else
                    {
                        Ids = Ids + "," + Id;
                    }
                }
            }

            for (int i = 0; i < reg.TempararyArrayListcoloumn.Length; i++)
            {
                string Id = reg.TempararyArrayListcoloumn[i].IVRM_CLM_PAR;
                if (Id != null)
                {
                    string name = Id;

                    if (name == "StudentsName")
                    {
                        if (reg.TempararyArrayListcoloumn.Length == 1)
                        {                            
                            name = "StudentsName = CASE WHEN  AMST_FirstName is null or AMST_FirstName=''  then '' else AMST_FirstName end+CASE WHEN  AMST_MiddleName is null or AMST_MiddleName = '' then ''  ELSE ' ' + AMST_MiddleName END +  CASE WHEN AMST_LastName is null or AMST_LastName = '' then ''  ELSE ' ' + AMST_LastName END ";
                        }

                        else
                        {
                            name = "StudentsName = CASE WHEN  AMST_FirstName is null or AMST_FirstName=''  then '' else AMST_FirstName end+CASE WHEN  AMST_MiddleName is null or AMST_MiddleName = ''  then ''  ELSE ' ' + AMST_MiddleName END +  CASE WHEN AMST_LastName is null or AMST_LastName = '' then ''  ELSE ' ' + AMST_LastName END ";

                        }
                    }
                    if (name == "AMST_ConStreet")
                    {
                        if (reg.TempararyArrayListcoloumn.Length == 1)
                        {
                            name = "AMST_ConStreet = CASE WHEN  REPLACE(AMST_ConStreet,',','') is null or REPLACE(AMST_ConStreet,',','')='' then '' else REPLACE(AMST_ConStreet,',','') end+CASE WHEN  REPLACE(AMST_ConArea, ',', '') is null or REPLACE(AMST_ConArea,',', '')= '' then ''  ELSE ', ' + REPLACE(AMST_ConArea, ',', '') END +     CASE WHEN REPLACE(AMST_ConCity,',', '') is null or REPLACE(AMST_ConCity,',', '')= '' then ''  ELSE ', ' + REPLACE(AMST_ConCity, ',', '') END +         CASE WHEN IVRMMS_Name is null or IVRMMS_Name = '' then ''  ELSE ', ' + IVRMMS_Name END +          CASE WHEN IVRMMC_CountryName is null or IVRMMC_CountryName = '' then ''  ELSE ', ' + IVRMMC_CountryName END +           CASE WHEN CAST(AMST_ConPincode as varchar(max)) is null or CAST(AMST_ConPincode as varchar(max))= '' or CAST(AMST_ConPincode as varchar(max))= 0 then ''  ELSE '-' + CAST(AMST_ConPincode as varchar(max)) END";
                        }

                        else
                        {
                            name = "AMST_ConStreet = CASE WHEN  REPLACE(AMST_ConStreet,',','') is null or REPLACE(AMST_ConStreet,',','')='' then '' else REPLACE(AMST_ConStreet,',','') end+CASE WHEN  REPLACE(AMST_ConArea, ',', '') is null or REPLACE(AMST_ConArea,',', '')= '' then ''  ELSE ', ' + REPLACE(AMST_ConArea, ',', '') END +     CASE WHEN REPLACE(AMST_ConCity,',', '') is null or REPLACE(AMST_ConCity,',', '')= '' then ''  ELSE ', ' + REPLACE(AMST_ConCity, ',', '') END +         CASE WHEN IVRMMS_Name is null or IVRMMS_Name = '' then ''  ELSE ', ' + IVRMMS_Name END +          CASE WHEN IVRMMC_CountryName is null or IVRMMC_CountryName = '' then ''  ELSE ', ' + IVRMMC_CountryName END +           CASE WHEN CAST(AMST_ConPincode as varchar(max)) is null or CAST(AMST_ConPincode as varchar(max))= '' or CAST(AMST_ConPincode as varchar(max))= 0 then ''  ELSE '-' + CAST(AMST_ConPincode as varchar(max)) END";

                        }

                    }
                    if (name == "AMST_PerAdd3")
                    {

                        if (reg.TempararyArrayListcoloumn.Length == 1)
                        {
                            name = "AMST_PerAdd3 = CASE WHEN  REPLACE(AMST_PerStreet,',','') is null or REPLACE(AMST_PerStreet,',','')='' then '' else REPLACE(AMST_PerStreet,',','') end+ CASE WHEN  REPLACE(AMST_PerArea, ',', '') is null or REPLACE(AMST_PerArea,',', '')= '' then ''  ELSE ', ' + REPLACE(AMST_PerArea, ',', '') END +   CASE WHEN REPLACE(AMST_PerCity,',', '') is null or REPLACE(AMST_PerCity,',', '')= '' then ''  ELSE ', ' + REPLACE(AMST_PerCity, ',', '') END +         CASE WHEN IVRMMS_Name is null or IVRMMS_Name = '' then ''  ELSE ', ' + IVRMMS_Name END +          CASE WHEN IVRMMC_CountryName is null or IVRMMC_CountryName = '' then ''  ELSE ', ' + IVRMMC_CountryName END +           CASE WHEN CAST(amst_perpincode as varchar(max)) is null or CAST(amst_perpincode as varchar(max))= '' or CAST(amst_perpincode as varchar(max))= 0 then ''  ELSE '-' + CAST(amst_perpincode as varchar(max)) END ";

                        }
                        else
                        {
                            name = "AMST_PerAdd3 = CASE WHEN  REPLACE(AMST_PerStreet,',','') is null or REPLACE(AMST_PerStreet,',','')='' then '' else REPLACE(AMST_PerStreet,',','') end+ CASE WHEN  REPLACE(AMST_PerArea, ',', '') is null or REPLACE(AMST_PerArea,',', '')= '' then ''  ELSE ', ' + REPLACE(AMST_PerArea, ',', '') END +   CASE WHEN REPLACE(AMST_PerCity,',', '') is null or REPLACE(AMST_PerCity,',', '')= '' then ''  ELSE ', ' + REPLACE(AMST_PerCity, ',', '') END +         CASE WHEN IVRMMS_Name is null or IVRMMS_Name = '' then ''  ELSE ', ' + IVRMMS_Name END +          CASE WHEN IVRMMC_CountryName is null or IVRMMC_CountryName = '' then ''  ELSE ', ' + IVRMMC_CountryName END +           CASE WHEN CAST(amst_perpincode as varchar(max)) is null or CAST(amst_perpincode as varchar(max))= '' or CAST(amst_perpincode as varchar(max))= 0 then ''  ELSE '-' + CAST(amst_perpincode as varchar(max)) END ";
                        }

                    }

                    if (name == "AMST_FatherName")
                    {
                        if (reg.TempararyArrayListcoloumn.Length == 1)
                        {
                            name = "(isnull(AMST_FatherName,'')+' '+ isnull(amst_fathersurname,'')) as AMST_FatherName";

                        }
                        else
                        {
                            name = "(isnull(AMST_FatherName,'')+' '+ isnull(amst_fathersurname,'')) as AMST_FatherName";
                        }
                    }
                    if (name == "AMST_MotherName")
                    {
                        if (reg.TempararyArrayListcoloumn.Length == 1)
                        {
                            name = "(isnull(AMST_MotherName,'')) as AMST_MotherName";

                        }
                        else
                        {
                            name = "(isnull(AMST_MotherName,'')) as AMST_MotherName";
                        }
                    }
                    else if (name == "JoinedYear")
                    {
                        if (reg.TempararyArrayListcoloumn.Length == 1)
                        {
                            name = "(select a.ASMAY_Year from Adm_School_M_Academic_Year a where a.ASMAY_Id=Adm_M_Student.ASMAY_Id) as JoinedYear";

                        }
                        else
                        {
                            name = "(select a.ASMAY_Year from Adm_School_M_Academic_Year a where a.ASMAY_Id=Adm_M_Student.ASMAY_Id) as JoinedYear";
                        }
                    }

                    else if (name == "JoinedClass")
                    {
                        if (reg.TempararyArrayListcoloumn.Length == 1)
                        {
                            name = "(select n.ASMCL_ClassName from Adm_School_M_Class n where n.ASMCL_Id=Adm_M_Student.ASMCL_Id) as JoinedClass";

                        }
                        else
                        {
                            name = "(select n.ASMCL_ClassName from Adm_School_M_Class n where n.ASMCL_Id=Adm_M_Student.ASMCL_Id) as JoinedClass";
                        }
                    }
                    else if (name == "JoinedSection")
                    {
                        if (reg.TempararyArrayListcoloumn.Length == 1)
                        {
                            name = "(select mseca.ASMC_SectionName from Adm_School_Y_Student ysa inner join Adm_School_M_Section mseca " +
                                "on mseca.ASMS_Id = ysa.ASMS_Id and ysa.AMST_Id=adm_M_student.AMST_Id where ysa.ASMAY_Id=" + reg.ASMAY_Id + ") as JoinedSection";

                        }
                        else
                        {
                            name = "(select mseca.ASMC_SectionName from Adm_School_Y_Student ysa inner join Adm_School_M_Section mseca " +
                                "on mseca.ASMS_Id = ysa.ASMS_Id and ysa.AMST_Id=adm_M_student.AMST_Id where ysa.ASMAY_Id=" + reg.ASMAY_Id + ") as JoinedSection";
                        }
                    }
                    else if (name == "AMST_PlaceOfBirthState")
                    {
                        if (reg.TempararyArrayListcoloumn.Length == 1)
                        {
                            name = "(Select n.IVRMMS_Name from IVRM_Master_State n where n.IVRMMS_Id=Adm_M_Student.AMST_PlaceOfBirthState) as AMST_PlaceOfBirthState";
                        }
                        else
                        {
                            name = "(Select n.IVRMMS_Name from IVRM_Master_State n where n.IVRMMS_Id=Adm_M_Student.AMST_PlaceOfBirthState) as AMST_PlaceOfBirthState";
                        }

                    }
                    else if (name == "AMST_PlaceOfBirthCountry")
                    {
                        if (reg.TempararyArrayListcoloumn.Length == 1)
                        {
                            name = "(Select n.IVRMMC_CountryName from IVRM_Master_Country n where n.IVRMMC_Id=Adm_M_Student.AMST_PlaceOfBirthCountry) as AMST_PlaceOfBirthCountry";
                        }
                        else
                        {
                            name = "(Select n.IVRMMC_CountryName from IVRM_Master_Country n where n.IVRMMC_Id=Adm_M_Student.AMST_PlaceOfBirthCountry) as AMST_PlaceOfBirthCountry";
                        }
                    }

                    else if (name == "AMSTPS_PrvSchoolName")
                    {
                        if (reg.TempararyArrayListcoloumn.Length == 1)
                        {
                            name = "isnull(AMSTPS_PrvSchoolName,'') as AMSTPS_PrvSchoolName";

                        }
                        else
                        {
                            name = "isnull(AMSTPS_PrvSchoolName,'') as AMSTPS_PrvSchoolName";
                        }
                    }                   

                    else if(name == "AMST_Village")
                    {
                        if (reg.TempararyArrayListcoloumn.Length == 1)
                        {
                            name = "isnull(AMST_Village,'') as AMST_Village";
                        }
                        else
                        {
                            name = "isnull(AMST_Village,'') as AMST_Village";
                        }
                    }
                    else if(name == "AMST_Town")
                    {
                        if (reg.TempararyArrayListcoloumn.Length == 1)
                        {
                            name = "isnull(AMST_Town,'') as AMST_Town";
                        }
                        else
                        {
                            name = "isnull(AMST_Town,'') as AMST_Town";
                        }
                    }
                    else if(name == "AMST_Taluk")
                    { 
                        if (reg.TempararyArrayListcoloumn.Length == 1)
                        {
                            name = "isnull(AMST_Taluk,'') as AMST_Taluk";
                        }
                        else
                        {
                            name = "isnull(AMST_Taluk,'') as AMST_Taluk";
                        }
                    }

                    else if (name == "AMST_Distirct")
                    {
                        if (reg.TempararyArrayListcoloumn.Length == 1)
                        {
                            name = "isnull(AMST_Distirct,'') as AMST_Distirct";
                        }
                        else
                        {
                            name = "isnull(AMST_Distirct,'') as AMST_Distirct";
                        }
                    }

                    else if (name == "AMSTPS_PreviousClass")
                    {
                        if (reg.TempararyArrayListcoloumn.Length == 1)
                        {
                            name = "isnull(AMSTPS_PreviousClass,'') as AMSTPS_PreviousClass";

                        }
                        else
                        {
                            name = "isnull(AMSTPS_PreviousClass,'') as AMSTPS_PreviousClass";
                        }
                    }

                    else if (name == "AMST_BPLCardNo")
                    {
                        if (reg.TempararyArrayListcoloumn.Length == 1)
                        {
                            name = "isnull(AMST_BPLCardNo,'') as AMST_BPLCardNo";

                        }
                        else
                        {
                            name = "isnull(AMST_BPLCardNo,'') as AMST_BPLCardNo";
                        }
                    }
                    else if (name == "IMCC_CategoryName")
                    {
                        if (reg.TempararyArrayListcoloumn.Length == 1)
                        {
                            name = "isnull(IMCC_CategoryName,'') as IMCC_CategoryName";

                        }
                        else
                        {
                            name = "isnull(IMCC_CategoryName,'') as IMCC_CategoryName";
                        }
                    }
                    else if (name == "IMC_CasteName")
                    {
                        if (reg.TempararyArrayListcoloumn.Length == 1)
                        {
                            name = "isnull(IMC_CasteName,'') as IMC_CasteName";

                        }
                        else
                        {
                            name = "isnull(IMC_CasteName,'') as IMC_CasteName";
                        }
                    }
                    else if (name == "AMST_Tpin")
                    {
                        if (reg.TempararyArrayListcoloumn.Length == 1)
                        {
                            name = "isnull(AMST_Tpin,'') as AMST_Tpin";

                        }
                        else
                        {
                            name = "isnull(AMST_Tpin,'') as AMST_Tpin";
                        }
                    }
                    else if (name == "AMST_GovtAdmno")
                    {
                        if (reg.TempararyArrayListcoloumn.Length == 1)
                        {
                            name = "isnull(AMST_GovtAdmno,'') as AMST_GovtAdmno";

                        }
                        else
                        {
                            name = "isnull(AMST_GovtAdmno,'') as AMST_GovtAdmno";
                        }
                    }
                    else if (name == "AMC_Name")
                    {
                        if (reg.TempararyArrayListcoloumn.Length == 1)
                        {
                            name = "isnull(AMC_Name,'') as AMC_Name";

                        }
                        else
                        {
                            name = "isnull(AMC_Name,'') as AMC_Name";
                        }
                    }
                    else if (name == "AMST_Date")
                    {
                        if (reg.TempararyArrayListcoloumn.Length == 1)
                        {
                            name = "convert(varchar(10),AMST_Date ,103) as AMST_Date";

                        }
                        else
                        {
                            name = "convert(varchar(10),AMST_Date ,103) as AMST_Date";
                        }
                    }
                    else if (name == "AMST_DOB")
                    {
                        if (reg.TempararyArrayListcoloumn.Length == 1)
                        {
                            name = "convert(varchar(10),AMST_DOB ,103) as AMST_DOB";

                        }
                        else
                        {
                            name = "convert(varchar(10),AMST_DOB ,103) as AMST_DOB";
                        }
                    }

                    else if (name == "AMSTPS_PrvTCDate")
                    {
                        if (reg.TempararyArrayListcoloumn.Length == 1)
                        {
                            name = "convert(varchar(10),AMSTPS_PrvTCDate ,103) as AMSTPS_PrvTCDate";

                        }
                        else
                        {
                            name = "convert(varchar(10),AMSTPS_PrvTCDate ,103) as AMSTPS_PrvTCDate";
                        }
                    }


                    IVRM_CLM_coloumn = name + "," + IVRM_CLM_coloumn;
                }
            }
            string coloumns = IVRM_CLM_coloumn.Remove(IVRM_CLM_coloumn.Length - 1);

            if (reg.AMC_Id == null || reg.AMC_Id == 0)
            {
                reg.AMC_Id = 0;

            }

            using (var cmd = _db.Database.GetDbConnection().CreateCommand())
            {
                cmd.CommandText = "Admission_Register_Report";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@tableparam",
                    SqlDbType.VarChar)
                {
                    Value = coloumns
                });
                cmd.Parameters.Add(new SqlParameter("@year",
                   SqlDbType.VarChar)
                {
                    Value = reg.ASMAY_Id
                });
                cmd.Parameters.Add(new SqlParameter("@class",
                 SqlDbType.VarChar)
                {
                    Value = Ids
                });
                cmd.Parameters.Add(new SqlParameter("@miid",
                SqlDbType.VarChar)
                {
                    Value = reg.MI_Id
                });
                cmd.Parameters.Add(new SqlParameter("@att",
            SqlDbType.VarChar)
                {
                    Value = reg.AMST_SOL
                });
                cmd.Parameters.Add(new SqlParameter("@AMC_Id",
          SqlDbType.VarChar)
                {
                    Value = reg.AMC_Id
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
                                var datatype = dataReader.GetFieldType(iFiled);

                                if (datatype.Name == "DateTime")
                                {
                                    var dateval = (Convert.ToDateTime(dataReader[iFiled]).Date).ToString("d");
                                    string val = dataReader[iFiled].ToString();
                                    if (val == "")
                                    {
                                        dataRow.Add(dataReader.GetName(iFiled), "");
                                    }
                                    else
                                    {
                                        dataRow.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? "" : dateval);
                                    }
                                }
                                else
                                {
                                    string val = dataReader[iFiled].ToString();
                                    if (val == "")
                                    {
                                        dataRow.Add(dataReader.GetName(iFiled), "");
                                    }
                                    else
                                    {
                                        dataRow.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? "" : dataReader[iFiled]);
                                        // use null instead of {}
                                    }
                                }
                            }
                            retObject.Add((ExpandoObject)dataRow);
                        }
                    }
                    reg.SearchstudentDetails = retObject.ToArray();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return reg;
        }
        [Route("getclass")]
        public SchoolYearWiseStudentDTO getclass([FromBody] SchoolYearWiseStudentDTO studData)
        {
            return _Clswisedailyatt.getclass(studData);
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
