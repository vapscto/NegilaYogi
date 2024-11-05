using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs;
using DataAccessMsSqlServerProvider;
using FeeServiceHub.com.vaps.interfaces;
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
using Microsoft.SqlServer.Server;
using DomainModel.Model.com.vaps.admission;

namespace FeeServiceHub.com.vaps.services
{
    public class fromtabledata
    {
        public long From_FMG_id { get; set; }
        public long From_FMH_id { get; set; }
        public long From_FTI_id { get; set; }
        public long From_FMA_id { get; set; }
        public long RunningExcessAmount { get; set; }
    }
    public class totabledata
    {
        public long To_FMG_id { get; set; }
        public long To_FMH_id { get; set; }
        public long To_FTI_id { get; set; }
        public long To_FMA_id { get; set; }
        public long adjustedamount { get; set; }
    }
    public class fromCollection : List<fromtabledata>, IEnumerable<SqlDataRecord>
    {
        IEnumerator<SqlDataRecord> IEnumerable<SqlDataRecord>.GetEnumerator()
        {
            var sqlRow = new SqlDataRecord(
                  new SqlMetaData("From_FMG_id", SqlDbType.BigInt),
                  new SqlMetaData("From_FMH_id", SqlDbType.BigInt),
                  new SqlMetaData("From_FTI_id", SqlDbType.BigInt),
                  new SqlMetaData("From_FMA_id", SqlDbType.BigInt),
                  new SqlMetaData("RunningExcessAmount", SqlDbType.BigInt));
            foreach (fromtabledata from in this)
            {
                
                sqlRow.SetSqlInt64(0, from.From_FMG_id);
                sqlRow.SetSqlInt64(1, from.From_FMH_id);
                sqlRow.SetSqlInt64(2, from.From_FTI_id);
                sqlRow.SetSqlInt64(3, from.From_FMA_id);
                sqlRow.SetSqlInt64(4, from.RunningExcessAmount);
                yield return sqlRow;
            }
        }
    }
    public class toCollection : List<totabledata>, IEnumerable<SqlDataRecord>
    {
        IEnumerator<SqlDataRecord> IEnumerable<SqlDataRecord>.GetEnumerator()
        {
            var sqlRow = new SqlDataRecord(
                  new SqlMetaData("To_FMG_id", SqlDbType.BigInt),
                  new SqlMetaData("To_FMH_id", SqlDbType.BigInt),
                  new SqlMetaData("To_FTI_id", SqlDbType.BigInt),
                  new SqlMetaData("To_FMA_id", SqlDbType.BigInt),
                  new SqlMetaData("adjustedamount", SqlDbType.BigInt));
            foreach (totabledata to in this)
            {

                sqlRow.SetSqlInt64(0, to.To_FMG_id);
                sqlRow.SetSqlInt64(1, to.To_FMH_id);
                sqlRow.SetSqlInt64(2, to.To_FTI_id);
                sqlRow.SetSqlInt64(3, to.To_FMA_id);
                sqlRow.SetSqlInt64(4, to.adjustedamount);
                yield return sqlRow;
            }
        }
    }
    public class FeeAdjustmentImpl : interfaces.FeeAdjustmentInterface
    {
       
        private static ConcurrentDictionary<string, FeeStudentAdjustmentDTO> _login =
        new ConcurrentDictionary<string, FeeStudentAdjustmentDTO>();

        public FeeGroupContext _FeeGroupContext;
        public DomainModelMsSqlServerContext _context;
        public FeeAdjustmentImpl(FeeGroupContext FeeGroupContext, DomainModelMsSqlServerContext context)
        {
            _FeeGroupContext = FeeGroupContext;
            _context = context;
        }
        public FeeStudentAdjustmentDTO getdata(FeeStudentAdjustmentDTO data)
        {
            try
            {

                List<MasterAcademic> year = new List<MasterAcademic>();
                year = _FeeGroupContext.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).OrderByDescending(t=>t.ASMAY_Order).ToList();
                data.fillyear = year.Distinct().ToArray();



                data.fillclass = (from a in _FeeGroupContext.School_Adm_Y_StudentDMO
                                  from b in _FeeGroupContext.School_M_Class
                                  where (a.ASMCL_Id == b.ASMCL_Id && b.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id)
                                  select new FeeStudentAdjustmentDTO
                                  {
                                      ASMCL_Id = a.ASMCL_Id,
                                      ASMCL_ClassName = b.ASMCL_ClassName,
                                  }).Distinct().OrderBy(t=>t.ASMCL_Id).ToArray();
                data.fillsection = (from a in _FeeGroupContext.School_Adm_Y_StudentDMO
                                    from b in _FeeGroupContext.school_M_Section
                                    where (a.ASMS_Id == b.ASMS_Id && b.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id)
                                    select new FeeStudentAdjustmentDTO
                                    {
                                        ASMS_Id = a.ASMS_Id,
                                        ASMC_SectionName = b.ASMC_SectionName,
                                    }).Distinct().OrderBy(t=>t.ASMS_Id).ToArray();

                data.filldata = (from a in _FeeGroupContext.feeStudentAdjustment
                                 from b in _FeeGroupContext.feehead
                                 from c in _FeeGroupContext.feehead
                                 from d in _FeeGroupContext.AdmissionStudentDMO
                                 from e in _FeeGroupContext.feeMIY
                                 from f in _FeeGroupContext.feeMIY
                                 where (a.FSA_From_FMH_Id == b.FMH_Id && a.FSA_To_FMH_Id == c.FMH_Id && a.AMST_Id == d.AMST_Id && a.FSA_From_FTI_Id == e.FTI_Id && a.FSA_TO_FTI_Id == f.FTI_Id )

                                 select new FeeStudentAdjustmentDTO
                                 {
                                     FSA_Id = a.FSA_Id,
                                     AMST_FirstName = d.AMST_FirstName,
                                     AMST_MiddleName = d.AMST_MiddleName,
                                     AMST_LastName = d.AMST_LastName,
                                     FMH_FeeNameF = b.FMH_FeeName,
                                     FMH_FeeNameT = c.FMH_FeeName,
                                     FTI_NameF = e.FTI_Name,
                                     FTI_NameT = f.FTI_Name,
                                     FSA_AdjustedAmount = a.FSA_AdjustedAmount,
                                     FSA_Date = a.FSA_Date,

                                 }).Distinct().OrderBy(t => t.FSA_Id).ToList().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public FeeStudentAdjustmentDTO getdataclassdet(FeeStudentAdjustmentDTO data)
        {
            try
            {
                 data.fillclass = (from a in _FeeGroupContext.School_Adm_Y_StudentDMO
                                  from b in _FeeGroupContext.School_M_Class
                                  where (a.ASMCL_Id==b.ASMCL_Id && b.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && b.ASMCL_ActiveFlag == true)
                                  select new FeeStudentAdjustmentDTO
                                  {
                                      ASMCL_Id = a.ASMCL_Id,
                                      ASMCL_ClassName = b.ASMCL_ClassName,
                                  }).Distinct().OrderBy(t => t.ASMCL_Id).ToArray();

                data.filldata = (from a in _FeeGroupContext.feeStudentAdjustment
                                 from b in _FeeGroupContext.feehead
                                 from c in _FeeGroupContext.feehead
                                 from d in _FeeGroupContext.AdmissionStudentDMO
                                 from e in _FeeGroupContext.feeMIY
                                 from f in _FeeGroupContext.feeMIY
                                 where (a.FSA_From_FMH_Id == b.FMH_Id && a.FSA_To_FMH_Id == c.FMH_Id && a.AMST_Id == d.AMST_Id && a.FSA_From_FTI_Id == e.FTI_Id && a.FSA_TO_FTI_Id == f.FTI_Id)

                                 select new FeeStudentAdjustmentDTO
                                 {
                                     FSA_Id = a.FSA_Id,
                                     AMST_FirstName = d.AMST_FirstName,
                                     AMST_MiddleName = d.AMST_MiddleName,
                                     AMST_LastName = d.AMST_LastName,
                                     FMH_FeeNameF = b.FMH_FeeName,
                                     FMH_FeeNameT = c.FMH_FeeName,
                                     FTI_NameF = e.FTI_Name,
                                     FTI_NameT = f.FTI_Name,
                                     FSA_AdjustedAmount = a.FSA_AdjustedAmount,
                                     FSA_Date = a.FSA_Date,

                                 }).Distinct().OrderBy(t => t.FSA_Id).ToList().ToArray();


            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public FeeStudentAdjustmentDTO getdatasectiondet(FeeStudentAdjustmentDTO data)
        {
            try
            {
                
                data.fillsection = (from a in _FeeGroupContext.School_Adm_Y_StudentDMO
                                    from b in _FeeGroupContext.school_M_Section
                                    where (a.ASMS_Id == b.ASMS_Id && b.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id && b.ASMC_ActiveFlag == 1)
                                    select new FeeStudentAdjustmentDTO
                                    {
                                        ASMS_Id = a.ASMS_Id,
                                        ASMC_SectionName = b.ASMC_SectionName,
                                    }).Distinct().OrderBy(t => t.ASMS_Id).ToArray();
                //}
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }

        public FeeStudentAdjustmentDTO getdatastudentdet(FeeStudentAdjustmentDTO data)
        {
            try
            {
                    data.fillstudent = (from a in _FeeGroupContext.AdmissionStudentDMO
                                        from b in _FeeGroupContext.School_Adm_Y_StudentDMO
                                        from c in _FeeGroupContext.FeeStudentTransactionDMO
                                        where (a.AMST_Id==b.AMST_Id && b.AMST_Id==c.AMST_Id && c.ASMAY_Id==b.ASMAY_Id && b.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && b.ASMCL_Id == data.ASMCL_Id && b.ASMS_Id == data.ASMS_Id && a.AMST_ActiveFlag==1 && c.FSS_RunningExcessAmount>0)
                                        select new FeeStudentAdjustmentDTO
                                        {
                                            AMST_Id = a.AMST_Id,
                                            AMST_FirstName = a.AMST_FirstName,
                                            AMST_MiddleName = a.AMST_MiddleName,
                                            AMST_LastName = a.AMST_LastName,
                                            AMST_RegistrationNo = a.AMST_AdmNo,

                                        }).OrderBy(t=>t.AMST_FirstName).Distinct().ToArray();
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public FeeStudentAdjustmentDTO getdatabothgroupdet(FeeStudentAdjustmentDTO data)
        {
            try
            {
                bool refundflag = false;
                if (data.filterrefund == "Refunable")
                    refundflag = true;
                data.fillfromgroup = (from a in _FeeGroupContext.FeeStudentTransactionDMO
                                      from b in _FeeGroupContext.FeeGroupDMO
                                      from c in _FeeGroupContext.FeeHeadDMO
                                      where ( a.FMH_Id==c.FMH_Id && a.FMG_Id == b.FMG_Id && a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && a.AMST_Id == data.AMST_Id && b.FMG_ActiceFlag == true  && c.FMH_RefundFlag== refundflag)
                                      select new FeeStudentAdjustmentDTO
                                      {
                                          FSA_From_FMG_Id = b.FMG_Id,
                                          FMG_GroupNameF = b.FMG_GroupName,
                                      }
                              ).Distinct().ToArray();;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public FeeStudentAdjustmentDTO getdatafromheaddet(FeeStudentAdjustmentDTO data)
        {
            try
            {
                bool refundflag = false;
                if (data.filterrefund == "Refunable")
                    refundflag = true;
                data.filltogroup = (from a in _FeeGroupContext.FeeStudentTransactionDMO
                                    from b in _FeeGroupContext.FeeGroupDMO
                                    where (a.FMG_Id == b.FMG_Id && a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && a.AMST_Id == data.AMST_Id   && a.FSS_ToBePaid > 0)
                                    select new FeeStudentAdjustmentDTO
                                    {
                                        FSA_To_FMG_Id = b.FMG_Id,
                                        FMG_GroupNameT = b.FMG_GroupName,
                                    }).Distinct().ToArray();
                data.fillfromhead = (from a in _FeeGroupContext.FeeStudentTransactionDMO
                                     from b in _FeeGroupContext.feehead
                                     from c in _FeeGroupContext.feeMIY
                                     where (a.FMH_Id == b.FMH_Id && a.FTI_Id == c.FTI_Id && a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && a.AMST_Id == data.AMST_Id  && data.multiplegroupF.ToString().Contains(Convert.ToString(a.FMG_Id)) && b.FMH_RefundFlag== refundflag  && ((a.FSS_RunningExcessAmount > 0 || b.FMH_Flag == "E" || (a.FSS_RefundableAmount > 0 )) && b.FMH_Flag != "F" || (a.FSS_RunningExcessAmount + a.FSS_RefundableAmount) > 0))
                                     select new FeeStudentAdjustmentDTO
                                     {
                                         FSA_From_FMH_Id = b.FMH_Id,
                                         FMH_FeeNameF = b.FMH_FeeName,
                                         FSA_From_FTI_Id = c.FTI_Id,
                                         FTI_NameF = c.FTI_Name,
                                         FSA_From_FMG_Id = a.FMG_Id,
                                         FSA_From_FMA_Id = a.FMA_Id,
                                         FSS_RunningExcessAmount =  a.FSS_RunningExcessAmount + a.FSS_RefundableAmount,

                                     }).Distinct().ToArray();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public FeeStudentAdjustmentDTO getdatatoheaddet(FeeStudentAdjustmentDTO data)
        {
            try
            {
                data.filltohead = (from a in _FeeGroupContext.FeeStudentTransactionDMO
                                   from b in _FeeGroupContext.feehead
                                   from c in _FeeGroupContext.feeMIY
                                   where (a.FMH_Id == b.FMH_Id && a.FTI_Id == c.FTI_Id && a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && a.AMST_Id == data.AMST_Id  && data.multiplegroupT.ToString().Contains(Convert.ToString(a.FMG_Id)) && a.FSS_ToBePaid > 0)// && a.FSS_RunningExcessAmount == 0 
                                   select new FeeStudentAdjustmentDTO
                                   {
                                       FSA_To_FMH_Id = b.FMH_Id,
                                       FMH_FeeNameT = b.FMH_FeeName,
                                       FSA_TO_FTI_Id = c.FTI_Id,
                                       FTI_NameT = c.FTI_Name,
                                       FSA_To_FMG_Id = a.FMG_Id,
                                       FSA_To_FMA_Id = a.FMA_Id,
                                       tobepaid=a.FSS_ToBePaid
                                   }
                                     ).Distinct().ToArray();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        
        public FeeStudentAdjustmentDTO savedatadelegate(FeeStudentAdjustmentDTO data)
        {
            try
            {
                FeeStudentAdjustmentDTO feepge = Mapper.Map<FeeStudentAdjustmentDTO>(data);
                if (feepge.FSA_Id > 0)
                {
                    var contactExists = _FeeGroupContext.Database.ExecuteSqlCommand("Fee_Student_Adjustment_Edit @p0, @p1", data.FSA_Id, data.tolist[0].FSA_AdjustedAmount);
                    if (contactExists > 0)
                    {

                        data.returnduplicatestatus = "Updated";
                    }
                    else
                    {
                        data.returnduplicatestatus = "not Updated";
                    }
                }
                else
                {
                    var result = _FeeGroupContext.feeStudentAdjustment.Where(t => t.MI_Id == feepge.MI_Id && t.ASMAY_Id == feepge.ASMAY_Id && t.AMST_Id == feepge.AMST_Id && t.FSA_From_FMG_Id == feepge.fromlist[0].FSA_From_FMG_Id && t.FSA_From_FMH_Id == feepge.fromlist[0].FSA_From_FMH_Id && t.FSA_From_FTI_Id == feepge.fromlist[0].FSA_From_FTI_Id && t.FSA_From_FMA_Id== feepge.fromlist[0].FSA_From_FMA_Id && t.FSA_To_FMG_Id == feepge.tolist[0].FSA_To_FMG_Id && t.FSA_To_FMH_Id == feepge.tolist[0].FSA_To_FMH_Id && t.FSA_TO_FTI_Id == feepge.tolist[0].FSA_TO_FTI_Id && t.FSA_To_FMA_Id == feepge.tolist[0].FSA_To_FMA_Id).ToList();
                    if (result.Count() > 0)
                    {
                        data.returnduplicatestatus = "Duplicate Record";
                    }
                    else
                    {
                        fromCollection fromdata = new fromCollection();

                        for (int i = 0; i < data.fromlist.Count; i++)
                        {
                            fromtabledata df = new fromtabledata();
                            df.From_FMG_id = data.fromlist[i].FSA_From_FMG_Id;
                            df.From_FMH_id = data.fromlist[i].FSA_From_FMH_Id;
                            df.From_FTI_id = data.fromlist[i].FSA_From_FTI_Id;
                            df.From_FMA_id = data.fromlist[i].FSA_From_FMA_Id;
                            df.RunningExcessAmount = data.fromlist[i].FSS_RunningExcessAmount;
                            fromdata.Add(df);
                        }
                        toCollection todata = new toCollection();
                        for (int i = 0; i < data.tolist.Count; i++)
                        {
                            totabledata dt = new totabledata();
                            dt.To_FMG_id = data.tolist[i].FSA_To_FMG_Id;
                            dt.To_FMH_id = data.tolist[i].FSA_To_FMH_Id;
                            dt.To_FTI_id = data.tolist[i].FSA_TO_FTI_Id;
                            dt.To_FMA_id = data.tolist[i].FSA_To_FMA_Id;
                            dt.adjustedamount = data.tolist[i].FSA_AdjustedAmount;
                            todata.Add(dt);
                        }


                        using (var cmd = _FeeGroupContext.Database.GetDbConnection().CreateCommand())
                        {
                            cmd.CommandText = "Fee_Student_Adjustment_insert";
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add(new SqlParameter("@miid",
                                SqlDbType.BigInt)
                            {
                                Value = data.MI_Id
                            });
                            cmd.Parameters.Add(new SqlParameter("@asmayid",
                               SqlDbType.BigInt)
                            {
                                Value = data.ASMAY_Id
                            });
                            cmd.Parameters.Add(new SqlParameter("@amstid",
                           SqlDbType.BigInt)
                            {
                                Value = data.AMST_Id
                            });

                            cmd.Parameters.Add(new SqlParameter("@fsa_date",
                        SqlDbType.DateTime)
                            {
                                Value = data.FSA_Date
                            });

                            cmd.Parameters.Add(new SqlParameter("@fsa_flag",
                              SqlDbType.Bit)
                            {
                                Value = 1
                            });

                            cmd.Parameters.Add(new SqlParameter("@INFO_ARRAYF",
                              SqlDbType.Structured)
                            {
                                Value = fromdata
                            });
                            cmd.Parameters.Add(new SqlParameter("@INFO_ARRAYT",
                           SqlDbType.Structured)
                            {
                                Value = todata
                            });
                            cmd.Parameters.Add(new SqlParameter("@userid",
                               SqlDbType.Decimal)
                            {
                                Value = data.userid
                            });

                            if (cmd.Connection.State != ConnectionState.Open)
                                cmd.Connection.Open();

                            var data1 = cmd.ExecuteNonQuery();

                            if (data1 >= 1)
                            {
                                data.returnduplicatestatus = "Saved";
                            }
                            else
                            {
                                data.returnduplicatestatus = "not Saved";
                            }
                        }
                    }
                }
               
            }
            catch (Exception ee)
            {
                data.returnduplicatestatus = "Error Found";
                Console.WriteLine(ee.Message);
            }
            return data;
        }
       
        public FeeStudentAdjustmentDTO getpageedit(int id)
        {
            FeeStudentAdjustmentDTO data = new FeeStudentAdjustmentDTO();
            try
            {
                
                var result = _FeeGroupContext.feeStudentAdjustment.Single(t => t.FSA_Id == id);
                data.FSA_Id = result.FSA_Id;
                data.MI_Id = result.MI_Id;
                data.AMST_Id = result.AMST_Id;
                data.ASMAY_Id = result.ASMAY_Id;
                data.FSA_Date = result.FSA_Date;
                data.FSA_From_FMH_Id = result.FSA_From_FMH_Id;
                data.FSA_From_FMG_Id = result.FSA_From_FMG_Id;
                data.FSA_From_FTI_Id = result.FSA_From_FTI_Id;
                data.FSA_From_FMA_Id = result.FSA_From_FMA_Id;
                data.FSA_AdjustedAmount = result.FSA_AdjustedAmount;
                data.FSA_To_FMH_Id = result.FSA_To_FMH_Id;
                data.FSA_To_FMG_Id = result.FSA_To_FMG_Id;
                data.FSA_TO_FTI_Id = result.FSA_TO_FTI_Id;
                data.FSA_To_FMA_Id = result.FSA_To_FMA_Id;
                data.FSA_ActiveFlag = result.FSA_ActiveFlag;
                data.userid = result.User_Id;

                var resASMCL_Id = (from a in _FeeGroupContext.School_Adm_Y_StudentDMO
                                   from b in _FeeGroupContext.School_M_Class
                                   where (a.ASMCL_Id == b.ASMCL_Id && b.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.AMST_Id == data.AMST_Id)
                                   select new FeeStudentAdjustmentDTO
                                   {
                                       ASMCL_Id = a.ASMCL_Id,
                                       ASMCL_ClassName = b.ASMCL_ClassName,
                                   }).Distinct().Single();

                data.ASMCL_Id = resASMCL_Id.ASMCL_Id;
                data.fillclass = (from a in _FeeGroupContext.School_Adm_Y_StudentDMO
                                  from b in _FeeGroupContext.School_M_Class
                                  where (a.ASMCL_Id == b.ASMCL_Id && b.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && b.ASMCL_ActiveFlag == true && a.ASMCL_Id==data.ASMCL_Id)
                                  select new FeeStudentAdjustmentDTO
                                  {
                                      ASMCL_Id = a.ASMCL_Id,
                                      ASMCL_ClassName = b.ASMCL_ClassName,
                                  }).Distinct().OrderBy(t => t.ASMCL_Id).ToArray();
                var resASMS_Id = (from a in _FeeGroupContext.School_Adm_Y_StudentDMO
                                   from b in _FeeGroupContext.school_M_Section
                                  where (a.ASMS_Id == b.ASMS_Id && b.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id== data.ASMCL_Id && a.AMST_Id == data.AMST_Id)
                                   select new FeeStudentAdjustmentDTO
                                   {
                                       ASMS_Id = a.ASMS_Id,
                                       ASMC_SectionName = b.ASMC_SectionName,
                                   }).Distinct().Single();
                data.ASMS_Id = resASMS_Id.ASMS_Id;
                data.fillsection = (from a in _FeeGroupContext.School_Adm_Y_StudentDMO
                                    from b in _FeeGroupContext.school_M_Section
                                    where (a.ASMS_Id == b.ASMS_Id && b.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id && b.ASMC_ActiveFlag == 1 && a.ASMS_Id== data.ASMS_Id)
                                    select new FeeStudentAdjustmentDTO
                                    {
                                        ASMS_Id = a.ASMS_Id,
                                        ASMC_SectionName = b.ASMC_SectionName,
                                    }).Distinct().OrderBy(t => t.ASMS_Id).ToArray();
                data.fillstudent = (from a in _FeeGroupContext.AdmissionStudentDMO
                                    from b in _FeeGroupContext.School_Adm_Y_StudentDMO
                                    where (a.AMST_Id == b.AMST_Id && a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && b.ASMCL_Id == data.ASMCL_Id && b.ASMS_Id == data.ASMS_Id && a.AMST_Id==data.AMST_Id)
                                    select new FeeStudentAdjustmentDTO
                                    {
                                        AMST_Id = a.AMST_Id,
                                        AMST_FirstName = a.AMST_FirstName,
                                        AMST_MiddleName = a.AMST_MiddleName,
                                        AMST_LastName = a.AMST_LastName,
                                        AMST_RegistrationNo = a.AMST_RegistrationNo,

                                    }).Distinct().ToArray();
                data.fillfromgroup = (from a in _FeeGroupContext.FeeStudentTransactionDMO
                                      from b in _FeeGroupContext.FeeGroupDMO
                                      where (a.FMG_Id == b.FMG_Id && a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && a.AMST_Id == data.AMST_Id && b.FMG_Id==data.FSA_From_FMG_Id )
                                      select new FeeStudentAdjustmentDTO
                                      {
                                          FSA_From_FMG_Id = b.FMG_Id,
                                          FMG_GroupNameF = b.FMG_GroupName,
                                      }
                              ).Distinct().ToArray();
                data.filltogroup = (from a in _FeeGroupContext.FeeStudentTransactionDMO
                                    from b in _FeeGroupContext.FeeGroupDMO
                                    where (a.FMG_Id == b.FMG_Id && a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && a.AMST_Id == data.AMST_Id && b.FMG_Id == data.FSA_To_FMG_Id )
                                    select new FeeStudentAdjustmentDTO
                                    {
                                        FSA_To_FMG_Id = b.FMG_Id,
                                        FMG_GroupNameT = b.FMG_GroupName,
                                    }
                              ).Distinct().ToArray();
                data.fillfromhead = (from a in _FeeGroupContext.FeeStudentTransactionDMO
                                     from b in _FeeGroupContext.feehead
                                     from c in _FeeGroupContext.feeMIY
                                     where (a.FMH_Id == b.FMH_Id && a.FTI_Id == c.FTI_Id && a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && a.AMST_Id == data.AMST_Id && a.FMG_Id==data.FSA_From_FMG_Id &&  a.FMH_Id==data.FSA_From_FMH_Id && a.FTI_Id == data.FSA_From_FTI_Id && a.FMA_Id == data.FSA_From_FMA_Id )
                                     select new FeeStudentAdjustmentDTO
                                     {
                                         FSA_From_FMH_Id = b.FMH_Id,
                                         FMH_FeeNameF = b.FMH_FeeName,
                                         FSA_From_FTI_Id = c.FTI_Id,
                                         FTI_NameF = c.FTI_Name,
                                         FSA_From_FMG_Id = a.FMG_Id,
                                         FSA_From_FMA_Id = a.FMA_Id,
                                         FSS_RunningExcessAmount = a.FSS_RunningExcessAmount+a.FSS_RefundableAmount + data.FSA_AdjustedAmount,

                                     }).Distinct().ToArray();
                data.filltohead = (from a in _FeeGroupContext.FeeStudentTransactionDMO
                                   from b in _FeeGroupContext.feehead
                                   from c in _FeeGroupContext.feeMIY
                                   where (a.FMH_Id == b.FMH_Id && a.FTI_Id == c.FTI_Id && a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && a.AMST_Id == data.AMST_Id && a.FMG_Id == data.FSA_To_FMG_Id && a.FMH_Id == data.FSA_To_FMH_Id && a.FTI_Id == data.FSA_TO_FTI_Id && a.FMA_Id == data.FSA_To_FMA_Id )
                                   select new FeeStudentAdjustmentDTO
                                   {
                                       FSA_To_FMH_Id = b.FMH_Id,
                                       FMH_FeeNameT = b.FMH_FeeName,
                                       FSA_TO_FTI_Id = c.FTI_Id,
                                       FTI_NameT = c.FTI_Name,
                                       FSA_To_FMG_Id = a.FMG_Id,
                                       FSA_To_FMA_Id = a.FMA_Id,
                                       FSS_RunningExcessAmount = a.FSS_RunningExcessAmount+a.FSS_RefundableAmount,
                                       tobepaid = a.FSS_ToBePaid + data.FSA_AdjustedAmount
                                   }
                                    ).Distinct().ToArray();

            }
            catch (Exception ee)
            {
                //_logger.LogError(ee.Message);
                Console.WriteLine(ee.Message);
            }
            return data;
        }

       
        public FeeStudentAdjustmentDTO deleterec(int id)
        {
            
            FeeStudentAdjustmentDTO page = new FeeStudentAdjustmentDTO();
            List<FeeStudentAdjustment> lorg = new List<FeeStudentAdjustment>();
            lorg = _FeeGroupContext.feeStudentAdjustment.Where(t => t.FSA_Id.Equals(id)).ToList();

           
            try
            {
                if (lorg.Any())
                { 
                    var result = _FeeGroupContext.feeStudentAdjustment.FirstOrDefault(t => t.FSA_Id == id);
                   var contactExists = _FeeGroupContext.Database.ExecuteSqlCommand("Fee_Student_Adjustment_Delete @p0", result.FSA_Id);
                    if (contactExists > 0)
                    {

                        page.returnduplicatestatus = "success";
                    }
                    else
                    {
                        page.returnduplicatestatus = "fail";
                    }
                }

                
            }
            catch (Exception ee)
            {
                // _logger.LogError(ee.Message);
                Console.WriteLine(ee.Message);
            }

            return page;
        }

      
        public FeeStudentAdjustmentDTO searching(FeeStudentAdjustmentDTO data)
        {

            try
            {

                switch (data.searchType)
                {

                    case "1":
                        string str = "";
                        data.searchtext = data.searchtext.ToUpper();
                        data.filldata = (from a in _FeeGroupContext.feeStudentAdjustment
                                         from b in _FeeGroupContext.feehead
                                         from c in _FeeGroupContext.feehead
                                         from d in _FeeGroupContext.AdmissionStudentDMO
                                         from e in _FeeGroupContext.feeMIY
                                         from f in _FeeGroupContext.feeMIY
                                         where (a.FSA_From_FMH_Id == b.FMH_Id && a.FSA_To_FMH_Id == c.FMH_Id && a.AMST_Id == d.AMST_Id && a.FSA_From_FTI_Id == e.FTI_Id && a.FSA_TO_FTI_Id == f.FTI_Id  && (((d.AMST_FirstName.ToUpper().Trim() + ' ' + (string.IsNullOrEmpty(d.AMST_MiddleName.ToUpper().Trim()) == true ? str : d.AMST_MiddleName.ToUpper().Trim())).Trim() + ' ' + (string.IsNullOrEmpty(d.AMST_LastName.ToUpper().Trim()) == true ? str : d.AMST_LastName.ToUpper().Trim())).Trim().Contains(data.searchtext) || d.AMST_FirstName.ToUpper().ToUpper().StartsWith(data.searchtext) || d.AMST_MiddleName.ToUpper().ToUpper().StartsWith(data.searchtext) || d.AMST_LastName.ToUpper().StartsWith(data.searchtext)))

                                         select new FeeStudentAdjustmentDTO
                                         {
                                             FSA_Id = a.FSA_Id,
                                             AMST_FirstName = d.AMST_FirstName,
                                             AMST_MiddleName = d.AMST_MiddleName,
                                             AMST_LastName = d.AMST_LastName,
                                             FMH_FeeNameF = b.FMH_FeeName,
                                             FMH_FeeNameT = c.FMH_FeeName,
                                             FTI_NameF = e.FTI_Name,
                                             FTI_NameT = f.FTI_Name,
                                             FSA_AdjustedAmount = a.FSA_AdjustedAmount,
                                             FSA_Date = a.FSA_Date,

                                         }).Distinct().OrderByDescending(t => t.AMST_FirstName).ToList().ToArray();

                        break;
                    case "2":
                        data.filldata = (from a in _FeeGroupContext.feeStudentAdjustment
                                              from b in _FeeGroupContext.feehead
                                              from c in _FeeGroupContext.feehead
                                              from d in _FeeGroupContext.AdmissionStudentDMO
                                              from e in _FeeGroupContext.feeMIY
                                              from f in _FeeGroupContext.feeMIY
                                              where (a.FSA_From_FMH_Id == b.FMH_Id && a.FSA_To_FMH_Id == c.FMH_Id && a.AMST_Id == d.AMST_Id && a.FSA_From_FTI_Id == e.FTI_Id && a.FSA_TO_FTI_Id == f.FTI_Id && b.FMH_FeeName.Contains(data.searchtext) )

                                              select new FeeStudentAdjustmentDTO
                                              {
                                                  FSA_Id = a.FSA_Id,
                                                  AMST_FirstName = d.AMST_FirstName,
                                                  AMST_MiddleName = d.AMST_MiddleName,
                                                  AMST_LastName = d.AMST_LastName,
                                                  FMH_FeeNameF = b.FMH_FeeName,
                                                  FMH_FeeNameT = c.FMH_FeeName,
                                                  FTI_NameF = e.FTI_Name,
                                                  FTI_NameT = f.FTI_Name,
                                                  FSA_AdjustedAmount = a.FSA_AdjustedAmount,
                                                  FSA_Date = a.FSA_Date,

                                              }).Distinct().OrderByDescending(t => t.FMH_FeeNameF).ToList().ToArray();
                        break;
                    case "3":
                        data.filldata = (from a in _FeeGroupContext.feeStudentAdjustment
                                         from b in _FeeGroupContext.feehead
                                         from c in _FeeGroupContext.feehead
                                         from d in _FeeGroupContext.AdmissionStudentDMO
                                         from e in _FeeGroupContext.feeMIY
                                         from f in _FeeGroupContext.feeMIY
                                         where (a.FSA_From_FMH_Id == b.FMH_Id && a.FSA_To_FMH_Id == c.FMH_Id && a.AMST_Id == d.AMST_Id && a.FSA_From_FTI_Id == e.FTI_Id && a.FSA_TO_FTI_Id == f.FTI_Id && e.FTI_Name.Contains(data.searchtext) )

                                         select new FeeStudentAdjustmentDTO
                                         {
                                             FSA_Id = a.FSA_Id,
                                             AMST_FirstName = d.AMST_FirstName,
                                             AMST_MiddleName = d.AMST_MiddleName,
                                             AMST_LastName = d.AMST_LastName,
                                             FMH_FeeNameF = b.FMH_FeeName,
                                             FMH_FeeNameT = c.FMH_FeeName,
                                             FTI_NameF = e.FTI_Name,
                                             FTI_NameT = f.FTI_Name,
                                             FSA_AdjustedAmount = a.FSA_AdjustedAmount,
                                             FSA_Date = a.FSA_Date,

                                         }).Distinct().OrderByDescending(t => t.FMH_FeeNameT).ToList().ToArray();
                       
                        break;
                    case "4":
                        data.filldata = (from a in _FeeGroupContext.feeStudentAdjustment
                                         from b in _FeeGroupContext.feehead
                                         from c in _FeeGroupContext.feehead
                                         from d in _FeeGroupContext.AdmissionStudentDMO
                                         from e in _FeeGroupContext.feeMIY
                                         from f in _FeeGroupContext.feeMIY
                                         where (a.FSA_From_FMH_Id == b.FMH_Id && a.FSA_To_FMH_Id == c.FMH_Id && a.AMST_Id == d.AMST_Id && a.FSA_From_FTI_Id == e.FTI_Id && a.FSA_TO_FTI_Id == f.FTI_Id && c.FMH_FeeName.Contains(data.searchtext))

                                         select new FeeStudentAdjustmentDTO
                                         {
                                             FSA_Id = a.FSA_Id,
                                             AMST_FirstName = d.AMST_FirstName,
                                             AMST_MiddleName = d.AMST_MiddleName,
                                             AMST_LastName = d.AMST_LastName,
                                             FMH_FeeNameF = b.FMH_FeeName,
                                             FMH_FeeNameT = c.FMH_FeeName,
                                             FTI_NameF = e.FTI_Name,
                                             FTI_NameT = f.FTI_Name,
                                             FSA_AdjustedAmount = a.FSA_AdjustedAmount,
                                             FSA_Date = a.FSA_Date,

                                         }).Distinct().OrderByDescending(t => t.FTI_NameF).ToList().ToArray();
                        break;
                    case "5":
                        data.filldata = (from a in _FeeGroupContext.feeStudentAdjustment
                                         from b in _FeeGroupContext.feehead
                                         from c in _FeeGroupContext.feehead
                                         from d in _FeeGroupContext.AdmissionStudentDMO
                                         from e in _FeeGroupContext.feeMIY
                                         from f in _FeeGroupContext.feeMIY
                                         where (a.FSA_From_FMH_Id == b.FMH_Id && a.FSA_To_FMH_Id == c.FMH_Id && a.AMST_Id == d.AMST_Id && a.FSA_From_FTI_Id == e.FTI_Id && a.FSA_TO_FTI_Id == f.FTI_Id && f.FTI_Name.Contains(data.searchtext))

                                         select new FeeStudentAdjustmentDTO
                                         {
                                             FSA_Id = a.FSA_Id,
                                             AMST_FirstName = d.AMST_FirstName,
                                             AMST_MiddleName = d.AMST_MiddleName,
                                             AMST_LastName = d.AMST_LastName,
                                             FMH_FeeNameF = b.FMH_FeeName,
                                             FMH_FeeNameT = c.FMH_FeeName,
                                             FTI_NameF = e.FTI_Name,
                                             FTI_NameT = f.FTI_Name,
                                             FSA_AdjustedAmount = a.FSA_AdjustedAmount,
                                             FSA_Date = a.FSA_Date,

                                         }).Distinct().OrderByDescending(t => t.FTI_NameT).ToList().ToArray();

                        break;
                    case "6":
                        data.filldata = (from a in _FeeGroupContext.feeStudentAdjustment
                                         from b in _FeeGroupContext.feehead
                                         from c in _FeeGroupContext.feehead
                                         from d in _FeeGroupContext.AdmissionStudentDMO
                                         from e in _FeeGroupContext.feeMIY
                                         from f in _FeeGroupContext.feeMIY
                                         where (a.FSA_From_FMH_Id == b.FMH_Id && a.FSA_To_FMH_Id == c.FMH_Id && a.AMST_Id == d.AMST_Id && a.FSA_From_FTI_Id == e.FTI_Id && a.FSA_TO_FTI_Id == f.FTI_Id && a.FSA_AdjustedAmount.ToString().Contains(data.searchnumber))

                                         select new FeeStudentAdjustmentDTO
                                         {
                                             FSA_Id = a.FSA_Id,
                                             AMST_FirstName = d.AMST_FirstName,
                                             AMST_MiddleName = d.AMST_MiddleName,
                                             AMST_LastName = d.AMST_LastName,
                                             FMH_FeeNameF = b.FMH_FeeName,
                                             FMH_FeeNameT = c.FMH_FeeName,
                                             FTI_NameF = e.FTI_Name,
                                             FTI_NameT = f.FTI_Name,
                                             FSA_AdjustedAmount = a.FSA_AdjustedAmount,
                                             FSA_Date = a.FSA_Date,

                                         }).Distinct().OrderByDescending(t => t.FSA_AdjustedAmount).ToList().ToArray();
                      
                        break;
                    case "7":
                        var date_format = data.searchdate.ToString("dd/MM/yyyy");
                        data.filldata = (from a in _FeeGroupContext.feeStudentAdjustment
                                         from b in _FeeGroupContext.feehead
                                         from c in _FeeGroupContext.feehead
                                         from d in _FeeGroupContext.AdmissionStudentDMO
                                         from e in _FeeGroupContext.feeMIY
                                         from f in _FeeGroupContext.feeMIY
                                         where (a.FSA_From_FMH_Id == b.FMH_Id && a.FSA_To_FMH_Id == c.FMH_Id && a.AMST_Id == d.AMST_Id && a.FSA_From_FTI_Id == e.FTI_Id && a.FSA_TO_FTI_Id == f.FTI_Id  && a.FSA_Date.ToString("dd/MM/yyyy") == data.searchdate.ToString("dd/MM/yyyy") )

                                         select new FeeStudentAdjustmentDTO
                                         {
                                             FSA_Id = a.FSA_Id,
                                             AMST_FirstName = d.AMST_FirstName,
                                             AMST_MiddleName = d.AMST_MiddleName,
                                             AMST_LastName = d.AMST_LastName,
                                             FMH_FeeNameF = b.FMH_FeeName,
                                             FMH_FeeNameT = c.FMH_FeeName,
                                             FTI_NameF = e.FTI_Name,
                                             FTI_NameT = f.FTI_Name,
                                             FSA_AdjustedAmount = a.FSA_AdjustedAmount,
                                             FSA_Date = a.FSA_Date,

                                         }).Distinct().OrderByDescending(t => t.FSA_Date).ToList().ToArray();
                        break;
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
    }
       
}


