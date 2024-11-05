using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.admission;
using DomainModel.Model;
using DomainModel.Model.com.vaps.admission;
using DomainModel.Model.com.vapstech.admission;
using Microsoft.AspNetCore.Identity;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.admission;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;

namespace AdmissionServiceHub.com.vaps.Services
{
    public class StudentYraeLossReportImpl : Interfaces.StudentYearLossReportInterface
    {
        public StudentYearLossReportContext _StudentTcReportContext;
        string IVRM_CLM_coloumn = "";
        public StudentYraeLossReportImpl(StudentYearLossReportContext frgContext)
        {
            _StudentTcReportContext = frgContext;
        }
        private readonly UserManager<ApplicationUser> _UserManager;

        public StudentYraeLossReportImpl(StudentYearLossReportContext StudentTcReportContext, UserManager<ApplicationUser> UserManager)
        {
            _StudentTcReportContext = StudentTcReportContext;
            _UserManager = UserManager;
        }
        public StudentYearLosReportDTO getdetails(StudentYearLosReportDTO data)
        {

            try
            {
                //List<MasterAcademic> year = new List<MasterAcademic>();
                //year = _StudentTcReportContext.AcademicYear.ToList();
                //data.fillyear = year.ToArray();

                List<MasterAcademic> year = new List<MasterAcademic>();
                year = _StudentTcReportContext.AcademicYear.Where(t => t.MI_Id == data.mid && t.Is_Active == true).OrderByDescending(a => a.ASMAY_Order).ToList();
                data.fillyear = year.ToArray();


                List<School_M_Class> classname = new List<School_M_Class>();
                classname = _StudentTcReportContext.admissioncls.Where(t=>t.MI_Id==data.mid && t.ASMCL_ActiveFlag==true).OrderBy(c => c.ASMCL_Order).ToList();
                data.fillclass = classname.ToArray();

                List<School_M_Section> secname = new List<School_M_Section>();
                secname = _StudentTcReportContext.school_M_Section.Where(t => t.MI_Id == data.mid && t.ASMC_ActiveFlag == 1).OrderBy(s => s.ASMC_Order).ToList();
                data.fillsection = secname.ToArray();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }

        public async Task<StudentYearLosReportDTO> Getdata(StudentYearLosReportDTO dto)
        {
            List<string> headId = new List<string>();
            IVRM_CLM_coloumn = "";
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
                                IVRM_CLM_coloumn += "(isnull(AMST_FirstName, ' ') +' '+ ISNULL(AMST_MiddleName, ' ') +' '+ ISNULL(AMST_LastName, ' ')) as AMST_FirstName";
                            }

                            else
                            {
                                IVRM_CLM_coloumn += "(isnull(AMST_FirstName, ' ') +' '+ ISNULL(AMST_MiddleName, ' ') +' '+ ISNULL(AMST_LastName, ' ')) as AMST_FirstName,";

                            }
                        }
                        else if (name == "AMST_PerAdd3")
                        {

                            if (IVRM_CLM_coloumn != "")
                            {
                                IVRM_CLM_coloumn += "(isnull(AMST_PerStreet, ' ')+ ',' + ISNULL(AMST_PerArea, ' ') + ',' +ISNULL(AMST_PerCity, ' ') + ',' + CAST(ISNULL(AMST_PerState, ' ') as varchar(max)) + ',' +CAST(ISNULL(AMST_PerCountry, ' ') as varchar(max))) as AMST_PerAdd3,";

                            }
                            else if (IVRM_CLM_coloumn == "" && dto.TempararyArrayheadList.Length > 1)
                            {
                                IVRM_CLM_coloumn += "(isnull(AMST_PerStreet, ' ')+ ',' + ISNULL(AMST_PerArea, ' ') + ',' +ISNULL(AMST_PerCity, ' ') + ',' + CAST(ISNULL(AMST_PerState, ' ') as varchar(max)) + ',' +CAST(ISNULL(AMST_PerCountry, ' ') as varchar(max))) as AMST_PerAdd3,";
                            }
                            else
                            {
                                IVRM_CLM_coloumn += "(isnull(AMST_PerStreet, ' ')+ ',' + ISNULL(AMST_PerArea, ' ') + ',' +ISNULL(AMST_PerCity, ' ') + ',' + CAST(ISNULL(AMST_PerState, ' ') as varchar(max)) + ',' +CAST(ISNULL(AMST_PerCountry, ' ') as varchar(max))) as AMST_PerAdd3";

                            }

                        }


                        else if (name == "AMST_ConCity")
                        {
                            
                                if (IVRM_CLM_coloumn != "")
                                {
                                    IVRM_CLM_coloumn += "(isnull(AMST_ConStreet, ' ') +','+ISNULL(AMST_ConArea, ' ') + ',' + ISNULL(AMST_ConCity, ' ') + ',' +CAST(ISNULL(AMST_ConState, ' ') as varchar(max)) + ',' + CAST(ISNULL(AMST_ConCountry, ' ') as varchar(max))) as AMST_ConCity,";
                                }
                                else
                                {
                                    IVRM_CLM_coloumn += "(isnull(AMST_ConStreet, ' ') +','+ISNULL(AMST_ConArea, ' ') + ',' + ISNULL(AMST_ConCity, ' ') + ',' +CAST(ISNULL(AMST_ConState, ' ') as varchar(max)) + ',' + CAST(ISNULL(AMST_ConCountry, ' ') as varchar(max))) as AMST_ConCity";
                                }
                           
                        }

                        else
                        {

                            if (IVRM_CLM_coloumn == "" && dto.TempararyArrayheadList.Length == 1)
                            {
                                IVRM_CLM_coloumn = name;
                            }
                            else if (IVRM_CLM_coloumn == "" && dto.TempararyArrayheadList.Length > 1)
                            {
                                IVRM_CLM_coloumn = name + "," + IVRM_CLM_coloumn;
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

               
               // coloumns = IVRM_CLM_coloumn.Remove(IVRM_CLM_coloumn.Length - 1);

                using (var cmd = _StudentTcReportContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Admission_YearLoss_Report";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@tableparam",
                        SqlDbType.VarChar)
                    {
                        Value = coloumns
                    });
                    cmd.Parameters.Add(new SqlParameter("@Asmayid",
                       SqlDbType.BigInt)
                    {
                        Value = dto.ASMAY_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@asmclid",
                       SqlDbType.BigInt)
                    {
                        Value = dto.ASMCL_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@asmcid",
                                   SqlDbType.BigInt)
                    {
                        Value = dto.ASMC_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@allorind",
                                 SqlDbType.VarChar)
                    {
                        Value = dto.tcallorindi
                    });
                    cmd.Parameters.Add(new SqlParameter("@mid",
                               SqlDbType.VarChar)
                    {
                        Value = dto.mid
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
                                        dataRow.Add(
                                        dataReader.GetName(iFiled),
                                        dataReader.IsDBNull(iFiled) ? "Not Available" : dateval  // use null instead of {}
                                    );
                                    }
                                    else
                                    {
                                        dataRow.Add(
                                      dataReader.GetName(iFiled),
                                      dataReader.IsDBNull(iFiled) ? "Not Available" : dataReader[iFiled] // use null instead of {}
                                  );
                                    }
                                    

                                }
                                try
                                {
                                    retObject.Add((ExpandoObject)dataRow);
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.Message);
                                }
                            }
                        }
                        dto.alldatagridreport = retObject.ToArray();
                        
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                return dto;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return dto;
        }




    }
}
