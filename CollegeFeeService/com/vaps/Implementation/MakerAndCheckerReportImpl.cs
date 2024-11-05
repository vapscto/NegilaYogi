﻿using System;
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
using DataAccessMsSqlServerProvider.com.vapstech.College.Fee;
namespace CollegeFeeService.com.vaps.Implementation
{
    public class MakerAndCheckerReportImpl:Interfaces.MakerAndCheckerReportInterface
    {

        public CollFeeGroupContext _ClgAdmissionContext;
        readonly ILogger<CollegemasterstudentconcessionImpl> _logger;
        public MakerAndCheckerReportImpl(CollFeeGroupContext _ClgAdmissionCon, ILogger<CollegemasterstudentconcessionImpl> log)
        {
            _logger = log;
            _ClgAdmissionContext = _ClgAdmissionCon;

        }


        public CollegePaymentApprovalDTO getdetails(CollegePaymentApprovalDTO dt)
        {
            // CollegePaymentApprovalDTO data = new CollegePaymentApprovalDTO();
            try
            {


                var year = _ClgAdmissionContext.AcademicYear.Where(y => y.Is_Active == true && y.MI_Id == dt.MI_ID).ToList();
                dt.yearlst = year.Distinct().OrderByDescending(t => t.ASMAY_Order).GroupBy(y => y.ASMAY_Year).Select(y => y.First()).ToArray();

                dt.grouplist = (from a in _ClgAdmissionContext.FeeGroupClgDMO
                                from b in _ClgAdmissionContext.FeeYearGroupDMO
                                where (a.FMG_Id == b.FMG_Id && a.MI_Id == dt.MI_ID && b.ASMAY_Id == dt.ASMAY_Id)
                                select new FeeGroupDMO
                                {
                                    FMG_Id = a.FMG_Id,
                                    FMG_GroupName = a.FMG_GroupName
                                }
                    ).Distinct().ToArray();


                List<FeeHeadClgDMO> headlist = new List<FeeHeadClgDMO>();
                headlist = _ClgAdmissionContext.FeeHeadClgDMO.Where(h => h.FMH_ActiveFlag == true && h.MI_Id == dt.MI_ID).ToList();
                dt.fillfeehead = headlist.Distinct().ToArray();


            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return dt;
        }



        public CollegePaymentApprovalDTO get_courses(CollegePaymentApprovalDTO data)
        {
            try
            {

                data.courselist = (from a in _ClgAdmissionContext.MasterCourseDMO
                                   from b in _ClgAdmissionContext.CLG_Adm_College_AY_CourseDMO
                                   where (a.MI_Id == data.MI_ID && a.AMCO_ActiveFlag && b.MI_Id == data.MI_ID && b.ASMAY_Id == data.ASMAY_Id && b.ACAYC_ActiveFlag && b.AMCO_Id == a.AMCO_Id)
                                   select a).Distinct().OrderBy(t => t.AMCO_Order).ToArray();

                data.grouplist = (from a in _ClgAdmissionContext.FeeGroupClgDMO
                                  from b in _ClgAdmissionContext.FeeYearGroupDMO
                                  where (a.FMG_Id == b.FMG_Id && a.MI_Id == data.MI_ID && b.ASMAY_Id == data.ASMAY_Id)
                                  select new FeeGroupDMO
                                  {
                                      FMG_Id = a.FMG_Id,
                                      FMG_GroupName = a.FMG_GroupName
                                  }
                   ).Distinct().ToArray();


                data.grouplist = (from a in _ClgAdmissionContext.FeeGroupClgDMO
                                  from b in _ClgAdmissionContext.FeeYearGroupDMO
                                  where (a.FMG_Id == b.FMG_Id && a.MI_Id == data.MI_ID && b.ASMAY_Id == data.ASMAY_Id)
                                  select new FeeGroupDMO
                                  {
                                      FMG_Id = a.FMG_Id,
                                      FMG_GroupName = a.FMG_GroupName
                                  }
                    ).Distinct().ToArray();


            }
            catch (Exception ex)
            {
                _logger.LogInformation("Atten_Batch_Mapping  get_courses :" + ex.Message);
            }
            return data;
        }
        public CollegePaymentApprovalDTO get_branches(CollegePaymentApprovalDTO data)
        {

            try
            {
                var branchlist = (from a in _ClgAdmissionContext.ClgMasterBranchDMO
                                  from b in _ClgAdmissionContext.CLG_Adm_College_AY_CourseDMO
                                  from c in _ClgAdmissionContext.CLG_Adm_College_AY_Course_BranchDMO
                                  where (a.MI_Id == data.MI_ID && a.AMB_ActiveFlag && b.MI_Id == data.MI_ID && b.ASMAY_Id == data.ASMAY_Id && b.ACAYC_ActiveFlag && c.ACAYC_Id == b.ACAYC_Id && c.MI_Id == data.MI_ID && c.AMB_Id == a.AMB_Id && c.ACAYCB_ActiveFlag && data.AMCO_Ids.Contains(b.AMCO_Id))
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
        public CollegePaymentApprovalDTO get_semisters(CollegePaymentApprovalDTO data)
        {
            try
            {
                var semisterlist = (from a in _ClgAdmissionContext.CLG_Adm_Master_SemesterDMO
                                    from b in _ClgAdmissionContext.CLG_Adm_College_AY_CourseDMO
                                    from c in _ClgAdmissionContext.CLG_Adm_College_AY_Course_BranchDMO
                                    from d in _ClgAdmissionContext.CLG_Adm_College_AY_Course_Branch_SemesterDMO
                                    where (a.MI_Id == data.MI_ID && a.AMSE_ActiveFlg && b.MI_Id == data.MI_ID && b.ASMAY_Id == data.ASMAY_Id && b.ACAYC_ActiveFlag && b.AMCO_Id == data.AMCO_Id && c.ACAYC_Id == b.ACAYC_Id && c.MI_Id == data.MI_ID && c.AMB_Id == data.AMB_Id && c.ACAYCB_ActiveFlag && d.MI_Id == data.MI_ID && d.ACAYCB_Id == c.ACAYCB_Id && d.AMSE_Id == a.AMSE_Id && d.ACAYCBS_ActiveFlag == true)
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
        public CollegePaymentApprovalDTO get_semisters_new(CollegePaymentApprovalDTO data)
        {
            try
            {
                var amco_ids = "";
                foreach (var x in data.AMCO_Ids)
                {
                    amco_ids += x + ",";
                }
                amco_ids = amco_ids.Substring(0, (amco_ids.Length - 1));


                var amb_ids = "";
                foreach (var x in data.AMB_Ids)
                {
                    amb_ids += x + ",";
                }
                amb_ids = amb_ids.Substring(0, (amb_ids.Length - 1));

                using (var cmd1 = _ClgAdmissionContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd1.CommandText = "College_Semester_Select";
                    cmd1.CommandType = CommandType.StoredProcedure;

                    cmd1.Parameters.Add(new SqlParameter("@ASMAY_Id",
                     SqlDbType.BigInt)
                    {
                        Value = data.ASMAY_Id
                    });

                    cmd1.Parameters.Add(new SqlParameter("@MI_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.MI_ID
                    });

                    cmd1.Parameters.Add(new SqlParameter("@amco_ids",
                        SqlDbType.VarChar)
                    {
                        Value = amco_ids

                    });
                    cmd1.Parameters.Add(new SqlParameter("@amb_ids",
                        SqlDbType.VarChar)
                    {
                        Value = amb_ids

                    });



                    if (cmd1.Connection.State != ConnectionState.Open)
                        cmd1.Connection.Open();

                    var retObject1 = new List<dynamic>();
                    try
                    {
                        using (var dataReader1 = cmd1.ExecuteReader())
                        {
                            while (dataReader1.Read())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader1.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(
                                        dataReader1.GetName(iFiled1),
                                        dataReader1.IsDBNull(iFiled1) ? 0 : dataReader1[iFiled1] // use null instead of {}
                                    );
                                }

                                retObject1.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.semisterlistnew = retObject1.ToArray();
                    }


                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

            }
            catch (Exception ex)
            {
                _logger.LogInformation("Atten_Batch_Mapping  get_semisters :" + ex.Message);
            }
            return data;
        }
        public CollegePaymentApprovalDTO getgroupmappedheads(CollegePaymentApprovalDTO data)
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
                                where (a.FMG_Id == c.FMG_Id && b.FMH_Id == c.FMH_Id && c.MI_Id == data.MI_ID && c.ASMAY_Id == data.ASMAY_Id && GrpId.Contains(c.FMG_Id))
                                select new CollegePaymentApprovalDTO
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



        public CollegePaymentApprovalDTO getgroupheadsid(CollegePaymentApprovalDTO data)
        {
            List<string> headId = new List<string>();
            List<long> GrpIdhead = new List<long>();
            try
            {
                foreach (var item in data.TempararyArrayheadList)
                {
                    headId.Add(item.FMH_FeeName);
                    GrpIdhead.Add(item.FMH_Id);
                }
                data.alldatahead = (
                               from b in _ClgAdmissionContext.FeeHeadClgDMO
                               where (GrpIdhead.Contains(b.FMH_Id))

                               select new CollegePaymentApprovalDTO
                               {
                                   FMH_Id = b.FMH_Id,
                                   FMH_FeeName = b.FMH_FeeName,

                               }
                   ).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }



        public async Task<CollegePaymentApprovalDTO> Getreportdetails(CollegePaymentApprovalDTO data)
        {
            long fmgg_id = 0;
            var amco_ids = "";

            foreach (var x in data.AMCO_Ids)
            {
                amco_ids += x + ",";
            }
            amco_ids = amco_ids.Substring(0, (amco_ids.Length - 1));


            var amb_ids = "";
            foreach (var x in data.AMB_Ids)
            {
                amb_ids += x + ",";
            }
            amb_ids = amb_ids.Substring(0, (amb_ids.Length - 1));

            var amse_ids = "";
            foreach (var x in data.AMSE_Ids)
            {
                amse_ids += x + ",";
            }
            amse_ids = amse_ids.Substring(0, (amse_ids.Length - 1));

            var fmg_id = "";
            foreach (var x in data.FMG_Ids)
            {
                fmg_id += x + ",";
            }
            fmg_id = fmg_id.Substring(0, (fmg_id.Length - 1));


            try
            {

                if (data.allorindivflag == "Regular")
                {
                    using (var cmd1 = _ClgAdmissionContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd1.CommandText = "SP_MakerAndChecker";
                        cmd1.CommandType = CommandType.StoredProcedure;

                        cmd1.Parameters.Add(new SqlParameter("@Asmay_id",
                         SqlDbType.BigInt)
                        {
                            Value = data.ASMAY_Id
                        });

                        cmd1.Parameters.Add(new SqlParameter("@Mi_Id",
                        SqlDbType.BigInt)
                        {
                            Value = data.MI_ID
                        });

                        cmd1.Parameters.Add(new SqlParameter("@from_date",
                         SqlDbType.DateTime)
                        {
                            Value = data.fromdate
                        });
                        cmd1.Parameters.Add(new SqlParameter("@to_date",
                         SqlDbType.DateTime)
                        {
                            Value = data.todate
                        });
                        cmd1.Parameters.Add(new SqlParameter("@amco_ids",
                            SqlDbType.VarChar)
                        {
                            Value = amco_ids

                        });
                        cmd1.Parameters.Add(new SqlParameter("@amb_ids",
                            SqlDbType.VarChar)
                        {
                            Value = amb_ids

                        });

                        cmd1.Parameters.Add(new SqlParameter("@amse_ids",
                            SqlDbType.VarChar)
                        {
                            Value = amse_ids

                        });
                        cmd1.Parameters.Add(new SqlParameter("@fmg_id",
                          SqlDbType.VarChar)
                        {
                            Value = fmg_id

                        });

                        if (cmd1.Connection.State != ConnectionState.Open)
                            cmd1.Connection.Open();

                        var retObject1 = new List<dynamic>();
                        try
                        {
                            using (var dataReader1 = await cmd1.ExecuteReaderAsync())
                            {
                                while (await dataReader1.ReadAsync())
                                {
                                    var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                    for (var iFiled1 = 0; iFiled1 < dataReader1.FieldCount; iFiled1++)
                                    {
                                        dataRow1.Add(
                                            dataReader1.GetName(iFiled1),
                                            dataReader1.IsDBNull(iFiled1) ? 0 : dataReader1[iFiled1] // use null instead of {}
                                        );
                                    }

                                    retObject1.Add((ExpandoObject)dataRow1);
                                }
                            }
                            data.savedrecord = retObject1.ToArray();
                        }


                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
                else if (data.allorindivflag == "Advance")
                {

                    using (var cmd1 = _ClgAdmissionContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd1.CommandText = "SP_MakerAndCheckerAdvance";
                        cmd1.CommandType = CommandType.StoredProcedure;

                        cmd1.Parameters.Add(new SqlParameter("@Asmay_id",
                         SqlDbType.VarChar)
                        {
                            Value = data.ASMAY_Id
                        });

                        cmd1.Parameters.Add(new SqlParameter("@Mi_Id",
                        SqlDbType.VarChar)
                        {
                            Value = data.MI_ID
                        });

                        cmd1.Parameters.Add(new SqlParameter("@from_date",
                         SqlDbType.VarChar)
                        {
                            Value = data.fromdate
                        });
                        cmd1.Parameters.Add(new SqlParameter("@to_date",
                         SqlDbType.VarChar)
                        {
                            Value = data.todate
                        });
                        cmd1.Parameters.Add(new SqlParameter("@amco_ids",
                            SqlDbType.VarChar)
                        {
                            Value = amco_ids

                        });
                        cmd1.Parameters.Add(new SqlParameter("@amb_ids",
                            SqlDbType.VarChar)
                        {
                            Value = amb_ids

                        });

                        cmd1.Parameters.Add(new SqlParameter("@amse_ids",
                            SqlDbType.VarChar)
                        {
                            Value = amse_ids

                        });
                        cmd1.Parameters.Add(new SqlParameter("@fmg_id",
                          SqlDbType.VarChar)
                        {
                            Value = fmg_id

                        });

                        if (cmd1.Connection.State != ConnectionState.Open)
                            cmd1.Connection.Open();

                        var retObject1 = new List<dynamic>();
                        try
                        {
                            using (var dataReader1 = await cmd1.ExecuteReaderAsync())
                            {
                                while (await dataReader1.ReadAsync())
                                {
                                    var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                    for (var iFiled1 = 0; iFiled1 < dataReader1.FieldCount; iFiled1++)
                                    {
                                        dataRow1.Add(
                                            dataReader1.GetName(iFiled1),
                                            dataReader1.IsDBNull(iFiled1) ? 0 : dataReader1[iFiled1] // use null instead of {}
                                        );
                                    }

                                    retObject1.Add((ExpandoObject)dataRow1);
                                }
                            }
                            data.savedrecord = retObject1.ToArray();
                        }


                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return data;
        }


        public CollegePaymentApprovalDTO getdata(CollegePaymentApprovalDTO data)
        {
            data.grouplist = (from a in _ClgAdmissionContext.FeeGroupClgDMO
                              from b in _ClgAdmissionContext.FeeYearGroupDMO
                              where (a.FMG_Id == b.FMG_Id && a.MI_Id == data.MI_ID && b.ASMAY_Id == data.ASMAY_Id)
                              select new FeeGroupDMO
                              {
                                  FMG_Id = a.FMG_Id,
                                  FMG_GroupName = a.FMG_GroupName
                              }
                     ).Distinct().ToArray();
            return data;
        }

    }
}
