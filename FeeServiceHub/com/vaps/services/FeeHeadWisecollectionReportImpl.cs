using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using DomainModel.Model.com.vaps.admission;
using DomainModel.Model.com.vaps.Fee;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.Fees;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace FeeServiceHub.com.vaps.services
{
    public class FeeHeadWisecollectionReportImpl :interfaces .FeeHeadWisecollectionReportInterface
    {

        public FeeGroupContext _FeeGroupContext;
        string IVRM_CLM_coloumn = "";
        string IVRM_CLM_coloumn1 = "";
        public FeeHeadWisecollectionReportImpl(FeeGroupContext frgContext)
        {
            _FeeGroupContext = frgContext;
        }

        private readonly UserManager<ApplicationUser> _UserManager;

        public FeeHeadWisecollectionReportImpl(FeeGroupContext FeeGroupContext, UserManager<ApplicationUser> UserManager)
        {
            _FeeGroupContext = FeeGroupContext;
            _UserManager = UserManager;
        }
        public FeeHeadWisecollectionReportDTO getdetails(FeeHeadWisecollectionReportDTO data)
        {

            try
            {
                List<MasterAcademic> year = new List<MasterAcademic>();
                year = _FeeGroupContext.AcademicYear.OrderBy(y => y.ASMAY_Order).ToList();
                data.fillyear = year.ToArray();

                List<School_M_Class> classname = new List<School_M_Class>();
                classname = _FeeGroupContext.admissioncls.ToList();
                data.fillclass = classname.ToArray();

                List<School_M_Section> secname = new List<School_M_Section>();
                secname = _FeeGroupContext.school_M_Section.ToList();
                data.fillsec = secname.ToArray();

                List<FeeGroupDMO> feegrp = new List<FeeGroupDMO>();
                feegrp = _FeeGroupContext.FeeGroupDMO.Where(t => t.FMG_ActiceFlag == true).ToList();
                data.fillfeegroup = feegrp.ToArray();


                List<FeeHeadDMO> feelisthead = new List<FeeHeadDMO>();
                feelisthead = _FeeGroupContext.feehead.Where(h => h.FMH_ActiveFlag == true).ToList();
                data.fillfeehead = feelisthead.ToArray();

                


            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }

        public async Task<FeeHeadWisecollectionReportDTO> getreport(FeeHeadWisecollectionReportDTO data)
        {
            IVRM_CLM_coloumn = "";
            string name = "";

            IVRM_CLM_coloumn1 = "";
            string name1 = "";
            try
            {
                for (int i = 0; i < data.tempgroupids.Length; i++)
                {
                    name = data.tempgroupids[i].columnID;
                    if (name != null)
                    {

                        IVRM_CLM_coloumn = name + "," + IVRM_CLM_coloumn;
                    }
                }
                string coloumns = "";
                coloumns = IVRM_CLM_coloumn.Remove(IVRM_CLM_coloumn.Length - 1);



                using (var cmd = _FeeGroupContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Fee_HeadWiseCollection_Report";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@year",
                        SqlDbType.VarChar)
                    {
                        Value = data.ASMAY_Id
                    });
                   
                    cmd.Parameters.Add(new SqlParameter("@fromdate",
                     SqlDbType.VarChar)
                    {
                        Value = data.fromdate
                    });
                    cmd.Parameters.Add(new SqlParameter("@todate",
                    SqlDbType.VarChar)
                    {
                        Value = data.todate
                    });
                    cmd.Parameters.Add(new SqlParameter("@ondate",
                    SqlDbType.VarChar)
                    {
                        Value = data.ondate
                    });

                    cmd.Parameters.Add(new SqlParameter("@groupids",
                   SqlDbType.VarChar)
                    {
                        Value = coloumns
                    });
                  
                    cmd.Parameters.Add(new SqlParameter("@class",
                 SqlDbType.VarChar)
                    {
                        Value = data.ASMCL_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@secid",
            SqlDbType.VarChar)
                    {
                        Value = data.ASMC_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@allorindiorcolflag",
              SqlDbType.VarChar)
                    {
                        Value = data.allorindiorcons
                    });
                    cmd.Parameters.Add(new SqlParameter("@dateorbetflag",
            SqlDbType.VarChar)
                    {
                        Value = data.dateorbteween
                    });

                    cmd.Parameters.Add(new SqlParameter("@headids",
         SqlDbType.VarChar)
                    {
                        Value = data.FMH_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@consflag",
        SqlDbType.VarChar)
                    {
                        Value = data.consolidateflag
                    });
                    cmd.Parameters.Add(new SqlParameter("@activeleft",
        SqlDbType.VarChar)
                    {
                        Value = data.activeleft
                    });
                    cmd.Parameters.Add(new SqlParameter("@nonconsolidateflag",
       SqlDbType.VarChar)
                    {
                        Value = data.nonconsolidateflag
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
                                    dataRow.Add(
                                        dataReader.GetName(iFiled),
                                        dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                                    );
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


            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
    }
}
