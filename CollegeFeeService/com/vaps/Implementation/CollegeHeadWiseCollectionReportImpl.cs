using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs;
using DataAccessMsSqlServerProvider;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using DomainModel.Model;
using System.Collections.Concurrent;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using System.Data;
using System.Data.SqlClient;
using PreadmissionDTOs.com.vaps.Fees;
using DomainModel.Model.com.vaps.Fee;
using System.Dynamic;
using DomainModel.Model.com.vapstech.Fee;
using PreadmissionDTOs.com.vaps.College.Fees;
using PreadmissionDTOs.com.vaps.College.Admission;
using DataAccessMsSqlServerProvider.com.vapstech.College.Admission;
using Microsoft.Extensions.Logging;
using DomainModel.Model.com.vapstech.College.Admission;
using DomainModel.Model.com.vapstech.College.Fees;
using CollegeFeeService.com.vaps.Interfaces;
using DomainModel.Model.com.vaps.admission;
using DataAccessMsSqlServerProvider.com.vapstech.College.Fee;

namespace CollegeFeeService.com.vaps.Implementation
{
    public class CollegeHeadWiseCollectionReportImpl : CollegeHeadWiseCollectionReportInterface
    {

        public CollFeeGroupContext _ClgAdmissionContext;
        readonly ILogger<CollegeHeadWiseCollectionReportImpl> _logger;
        public CollegeHeadWiseCollectionReportImpl(CollFeeGroupContext _ClgAdmissionCon, ILogger<CollegeHeadWiseCollectionReportImpl> log)
        {
            _logger = log;
            _ClgAdmissionContext = _ClgAdmissionCon;
        }


        public CollegeConcessionDTO getdetails(CollegeConcessionDTO dt)
        {
            // CollegeConcessionDTO data = new CollegeConcessionDTO();
            try
            {


                var year = _ClgAdmissionContext.AcademicYear.Where(y => y.Is_Active == true && y.MI_Id == dt.MI_Id).ToList();
                dt.yearlst = year.Distinct().GroupBy(y => y.ASMAY_Year).Select(y => y.First()).ToArray();

                dt.grouplist = (from a in _ClgAdmissionContext.FeeGroupClgDMO
                                from b in _ClgAdmissionContext.FeeYearGroupDMO
                                where (a.FMG_Id == b.FMG_Id && a.MI_Id == dt.MI_Id && b.ASMAY_Id == dt.ASMAY_Id)
                                select new FeeGroupDMO
                                {
                                    FMG_Id = a.FMG_Id,
                                    FMG_GroupName = a.FMG_GroupName
                                }
                    ).Distinct().ToArray();


                List<FeeHeadClgDMO> headlist = new List<FeeHeadClgDMO>();
                headlist = _ClgAdmissionContext.FeeHeadClgDMO.Where(h => h.FMH_ActiveFlag == true && h.MI_Id == dt.MI_Id).ToList();
                dt.fillfeehead = headlist.Distinct().ToArray();

                List<School_M_Section> section = new List<School_M_Section>();
                dt.fillsection = (from a in _ClgAdmissionContext.Adm_College_Yearly_StudentDMO
                                  from b in _ClgAdmissionContext.Adm_College_Master_SectionDMO
                                  where (a.AMSE_Id == b.ACMS_Id && a.ASMAY_Id == dt.ASMAY_Id && b.MI_Id == dt.MI_Id)
                                  select new CollegeConcessionDTO
                                  {
                                      AMSC_Id = b.ACMS_Id,
                                      ASMC_SectionName = b.ACMS_SectionName
                                  }
                          ).Distinct().ToArray();




            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return dt;
        }



        public CollegeConcessionDTO get_courses(CollegeConcessionDTO data)
        {
            try
            {

                data.courselist = (from a in _ClgAdmissionContext.MasterCourseDMO
                                   from b in _ClgAdmissionContext.CLG_Adm_College_AY_CourseDMO
                                   where (a.MI_Id == data.MI_Id && a.AMCO_ActiveFlag && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.ACAYC_ActiveFlag && b.AMCO_Id == a.AMCO_Id)
                                   select a).Distinct().OrderBy(t => t.AMCO_Order).ToArray();

            }
            catch (Exception ex)
            {
                _logger.LogInformation("Atten_Batch_Mapping  get_courses :" + ex.Message);
            }
            return data;
        }
        public CollegeConcessionDTO get_branches(CollegeConcessionDTO data)
        {

            try
            {
                var branchlist = (from a in _ClgAdmissionContext.ClgMasterBranchDMO
                                  from b in _ClgAdmissionContext.CLG_Adm_College_AY_CourseDMO
                                  from c in _ClgAdmissionContext.CLG_Adm_College_AY_Course_BranchDMO
                                  where (a.MI_Id == data.MI_Id && a.AMB_ActiveFlag && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.ACAYC_ActiveFlag && c.ACAYC_Id == b.ACAYC_Id && c.MI_Id == data.MI_Id && c.AMB_Id == a.AMB_Id && c.ACAYCB_ActiveFlag && data.AMCO_Ids.Contains(b.AMCO_Id))
                                  select new ClgMasterBranchDMO
                                  {
                                      AMB_Id = a.AMB_Id,
                                      AMB_BranchName = a.AMB_BranchName,
                                      AMB_BranchCode = a.AMB_BranchCode,
                                      AMB_BranchInfo = a.AMB_BranchInfo,
                                      AMB_BranchType = a.AMB_BranchType,
                                      AMB_StudentCapacity = a.AMB_StudentCapacity,
                                      AMB_Order = a.AMB_Order,
                                      AMB_AidedUnAided = a.AMB_AidedUnAided
                                  }).Distinct().ToList();
                data.branchlist = branchlist.OrderBy(t => t.AMB_Order).ToArray();

            }
            catch (Exception ex)
            {
                _logger.LogInformation("Atten_Batch_Mapping  get_branches :" + ex.Message);
            }
            return data;
        }
        public CollegeConcessionDTO get_semisters(CollegeConcessionDTO data)
        {
            try
            {
                var semisterlist = (from a in _ClgAdmissionContext.CLG_Adm_Master_SemesterDMO
                                    from b in _ClgAdmissionContext.CLG_Adm_College_AY_CourseDMO
                                    from c in _ClgAdmissionContext.CLG_Adm_College_AY_Course_BranchDMO
                                    from d in _ClgAdmissionContext.CLG_Adm_College_AY_Course_Branch_SemesterDMO
                                    where (a.MI_Id == data.MI_Id && a.AMSE_ActiveFlg && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.ACAYC_ActiveFlag && b.AMCO_Id == data.AMCO_Id && c.ACAYC_Id == b.ACAYC_Id && c.MI_Id == data.MI_Id && c.AMB_Id == data.AMB_Id && c.ACAYCB_ActiveFlag && d.MI_Id == data.MI_Id && d.ACAYCB_Id == c.ACAYCB_Id && d.AMSE_Id == a.AMSE_Id && d.ACAYCBS_ActiveFlag)
                                    select new CLG_Adm_Master_SemesterDMO
                                    {
                                        AMSE_Id = a.AMSE_Id,
                                        AMSE_SEMName = a.AMSE_SEMName,
                                        AMSE_SEMInfo = a.AMSE_SEMInfo,
                                        AMSE_SEMCode = a.AMSE_SEMCode,
                                        AMSE_SEMTypeFlag = a.AMSE_SEMTypeFlag,
                                        AMSE_SEMOrder = a.AMSE_SEMOrder,
                                        AMSE_Year = a.AMSE_Year,
                                        AMSE_EvenOdd = a.AMSE_EvenOdd
                                    }).Distinct().ToList();
                data.semisterlist = semisterlist.OrderBy(t => t.AMSE_SEMOrder).ToArray();

            }
            catch (Exception ex)
            {
                _logger.LogInformation("Atten_Batch_Mapping  get_semisters :" + ex.Message);
            }
            return data;
        }




        public CollegeConcessionDTO getgroupmappedheads(CollegeConcessionDTO data)
        {
            List<long> GrpId = new List<long>();
            try
            {
                foreach (var item in data.TempararyArrayList)
                {
                    GrpId.Add(item.FMG_Id);
                }
                data.alldata = (from a in _ClgAdmissionContext.FeeGroupClgDMO
                                from b in _ClgAdmissionContext.FeeHeadClgDMO
                                from c in _ClgAdmissionContext.CLG_Fee_Yearly_Group_Head_Mapping
                                where (a.FMG_Id == c.FMG_Id && b.FMH_Id == c.FMH_Id && c.MI_Id == data.MI_Id && c.ASMAY_Id == data.ASMAY_Id && GrpId.Contains(c.FMG_Id))
                                select new CollegeConcessionDTO
                                {
                                    FMH_Id = b.FMH_Id,
                                    FMH_FeeName = b.FMH_FeeName,

                                }
                    ).Distinct().OrderBy(h => h.FMH_FeeName).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return data;
        }


        public async Task<CollegeConcessionDTO> radiobtndata(CollegeConcessionDTO temp)
        {
            long fmgg_id = 0;
            var amco_ids = "";
            foreach (var x in temp.AMCO_Ids)
            {
                amco_ids += x + ",";
            }
            amco_ids = amco_ids.Substring(0, (amco_ids.Length - 1));


            var amb_ids = "";
            foreach (var x in temp.AMB_Ids)
            {
                amb_ids += x + ",";
            }
            amb_ids = amb_ids.Substring(0, (amb_ids.Length - 1));

            var fmg_id = "";
            foreach (var x in temp.FMG_Ids)
            {
                fmg_id += x + ",";
            }
            fmg_id = fmg_id.Substring(0, (fmg_id.Length - 1));


            if (temp.reporttype == "year")
            {
                temp.Fromdate = Convert.ToDateTime("01 / 01 / 2017");
                //temp.To_Date = Convert.ToDateTime("01 / 01 / 2017");
                //  temp.duedate = "null";
            }


            using (var cmd = _ClgAdmissionContext.Database.GetDbConnection().CreateCommand())
            {
                cmd.CommandText = "College_headwise_Collection_report";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@fmg_id",
                    SqlDbType.VarChar)
                {
                    // Value = coloumns
                    Value = fmg_id
                });
                cmd.Parameters.Add(new SqlParameter("@Mi_id",
                  SqlDbType.BigInt)
                {
                    Value = temp.MI_Id
                });
                cmd.Parameters.Add(new SqlParameter("@ASMAY_ID",
                   SqlDbType.BigInt)
                {
                    Value = Convert.ToInt64(temp.ASMAY_Id)
                });
                cmd.Parameters.Add(new SqlParameter("@amco_ids",
               SqlDbType.VarChar)
                {
                    Value = amco_ids
                });
                cmd.Parameters.Add(new SqlParameter("@amb_ids",
               SqlDbType.VarChar)
                {
                    Value = amb_ids
                });
               

                cmd.Parameters.Add(new SqlParameter("@fromdate",
           SqlDbType.DateTime)
                {
                    Value = temp.Fromdate
                });

                cmd.Parameters.Add(new SqlParameter("@todate",
             SqlDbType.DateTime)
                {
                    Value = temp.Todate
                });

                cmd.Parameters.Add(new SqlParameter("@section",
          SqlDbType.BigInt)
                {
                    Value = temp.AMSC_Id
                });

                cmd.Parameters.Add(new SqlParameter("@userid",
         SqlDbType.BigInt)
                {
                    Value = temp.userid
                });
                cmd.Parameters.Add(new SqlParameter("@active",
              SqlDbType.BigInt)
                {
                    Value = temp.active
                });
                cmd.Parameters.Add(new SqlParameter("@deactive",
              SqlDbType.BigInt)
                {
                    Value = temp.deactive
                });
                cmd.Parameters.Add(new SqlParameter("@left",
              SqlDbType.BigInt)
                {
                    Value = temp.left
                });


                if (cmd.Connection.State != ConnectionState.Open)
                    cmd.Connection.Open();

                var retObject = new List<dynamic>();

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

                    temp.alldata = retObject.ToArray();

                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                    Console.WriteLine(ex.Message);
                }
                return temp;
            }


        }

    


    }
}
