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
    public class CategoryWiseTotalStrengthImpl : Interfaces.CategoryWiseTotalStrengthInterface
    {
        public StudentTcReportContext _StudentTcReportContext;       
        public CategoryWiseTotalStrengthImpl(StudentTcReportContext frgContext)
        {
            _StudentTcReportContext = frgContext;
        }
        private readonly UserManager<ApplicationUser> _UserManager;

        public CategoryWiseTotalStrengthImpl(StudentTcReportContext StudentTcReportContext, UserManager<ApplicationUser> UserManager)
        {
            _StudentTcReportContext = StudentTcReportContext;
            _UserManager = UserManager;
        }

        public CategoryWiseTotalStrengthDTO getdetails(CategoryWiseTotalStrengthDTO data)
        {
            try
            {
                List<MasterAcademic> year = new List<MasterAcademic>();
                year = _StudentTcReportContext.AcademicYear.Where(d => d.MI_Id == data.MI_Id && d.Is_Active == true).ToList();
                data.accyear = year.OrderByDescending(a => a.ASMAY_Order).ToArray();

                List<School_M_Class> classname = new List<School_M_Class>();
                classname = _StudentTcReportContext.admissioncls.Where(d => d.MI_Id == data.MI_Id && d.ASMCL_ActiveFlag == true).ToList();
                data.accclasss = classname.OrderBy(c => c.ASMCL_Order).ToArray();

                List<School_M_Section> secname = new List<School_M_Section>();
                secname = _StudentTcReportContext.school_M_Section.Where(d => d.MI_Id == data.MI_Id && d.ASMC_ActiveFlag == 1).ToList();
                data.accsec = secname.OrderBy(s => s.ASMC_Order).ToArray();

                List<mastercasteDMO> caste = new List<mastercasteDMO>();
                caste = _StudentTcReportContext.mastercasteDMO.Where(t => t.MI_Id == data.MI_Id).ToList();
                data.acccaste = caste.ToArray();

                List<castecategoryDMO> castecategory = new List<castecategoryDMO>();
                castecategory = _StudentTcReportContext.castecategoryDMO.ToList();
                data.castecategory = castecategory.ToArray();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }

        public async Task<CategoryWiseTotalStrengthDTO> Getreportdetails(CategoryWiseTotalStrengthDTO data)
        {
            try
            {
                string coloumns1 = "";
                string coloumns = "";
                if (data.casteorcategory == "2")
                {
                    List<string> headId = new List<string>();
                    string IVRM_CLM_coloumn = "";
                    string name = "";
                    for (int i = 0; i < data.TempararyArrayheadList.Length; i++)
                    {
                        name = data.TempararyArrayheadList[i].columnID;
                        if (i == 0)
                        {
                            IVRM_CLM_coloumn = name;
                        }
                        else
                        {
                            IVRM_CLM_coloumn = name + "," + IVRM_CLM_coloumn;
                        }
                    }
                    
                    if (IVRM_CLM_coloumn.EndsWith(","))
                    {
                        coloumns = IVRM_CLM_coloumn.Remove(IVRM_CLM_coloumn.Length - 1);
                        coloumns1 = "0";                       
                    }
                    else
                    {
                        coloumns = IVRM_CLM_coloumn;
                        coloumns1 = "0";
                    }
                }else if (data.casteorcategory == "1")
                {
                    //category
                    List<string> headId1 = new List<string>();
                    string IVRM_CLM_coloumn1 = "";
                    string name1 = "";
                    for (int i = 0; i < data.TempararyArrayheadListcastecategory.Length; i++)
                    {
                        name1 = data.TempararyArrayheadListcastecategory[i].castecategoryid;
                        if (i == 0)
                        {
                            IVRM_CLM_coloumn1 = name1;
                        }
                        else
                        {
                            IVRM_CLM_coloumn1 = name1 + "," + IVRM_CLM_coloumn1;
                        }
                    }
                    
                    if (IVRM_CLM_coloumn1.EndsWith(","))
                    {
                        coloumns1 = IVRM_CLM_coloumn1.Remove(IVRM_CLM_coloumn1.Length - 1);                        
                        coloumns = "0";
                    }
                    else
                    {
                        coloumns1 = IVRM_CLM_coloumn1;                        
                        coloumns = "0";
                    }
                }
                else
                {
                    coloumns1 = "0";
                    coloumns = "0";
                }

             

                using (var cmd = _StudentTcReportContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Adm_Caste_CategoryWise_Total_Strength";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                       SqlDbType.VarChar)
                    {
                        Value = data.ASMAY_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                            SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                       SqlDbType.VarChar)
                    {
                        Value = data.ASMCL_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_ID",
                                   SqlDbType.VarChar)
                    {
                        Value = data.ASMS_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@IMC_ID",
                                 SqlDbType.VarChar)
                    {
                        Value = coloumns
                    });

                    cmd.Parameters.Add(new SqlParameter("@ALLORINDI",
                              SqlDbType.VarChar)
                    {
                        Value = data.tcallorindi
                    });
                    cmd.Parameters.Add(new SqlParameter("@STUDENTORCASTE",
                                 SqlDbType.VarChar)
                    {
                        Value = data.tcperortemp
                    });

                    cmd.Parameters.Add(new SqlParameter("@casteorcategory",
                                 SqlDbType.VarChar)
                    {
                        Value = data.casteorcategory
                    });
                    cmd.Parameters.Add(new SqlParameter("@IMCC_ID",
                             SqlDbType.VarChar)
                    {
                        Value = coloumns1
                    });
                    cmd.Parameters.Add(new SqlParameter("@STUDENTORCATEGORY",
             SqlDbType.VarChar)
                    {
                        Value = data.categorystudent
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
                        data.report = retObject.ToArray();
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
    }
}
