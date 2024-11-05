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
using System.IO;
using DomainModel.Model.com.vaps.admission;
using DomainModel.Model.com.vapstech.Transport;

namespace FeeServiceHub.com.vaps.services
{
    public class FeeDetailsReportImpl : interfaces.FeeDetailsReportInterface
    {
        public FeeGroupContext _FeeGroupContext;

        public FeeDetailsReportImpl(FeeGroupContext frgContext)
        {
            _FeeGroupContext = frgContext;
        }
        public FeeTransactionPaymentDTO getdetails(FeeTransactionPaymentDTO data)
        {
            try
            {


                List<MasterAcademic> year = new List<MasterAcademic>();
                year = _FeeGroupContext.AcademicYear.Where(y => y.Is_Active == true && y.MI_Id == data.MI_ID).OrderByDescending(y => y.ASMAY_Order).ToList();
                data.adcyear = year.GroupBy(y => y.ASMAY_Year).Select(y => y.First()).ToArray();

                List<School_M_Class> allclas = new List<School_M_Class>();
                allclas = _FeeGroupContext.admissioncls.Where(t => t.MI_Id == data.MI_ID && t.ASMCL_ActiveFlag == true).ToList();
                data.fillclass = allclas.ToArray();


                data.fillmasterhead = (from a in _FeeGroupContext.FeeHeadDMO
                                       from b in _FeeGroupContext.FeeYearlygroupHeadMappingDMO
                                       from c in _FeeGroupContext.FEeGroupLoginPreviledgeDMO
                                       where (a.FMH_Id == b.FMH_Id && b.MI_Id == data.MI_ID && b.ASMAY_Id == data.ASMAY_Id && c.User_Id == data.userid && b.FMG_Id == c.FMG_ID && a.FMH_ActiveFlag == true)
                                       select new FeeRefundDTO
                                       {
                                           fmh_id = a.FMH_Id,
                                           fmh_feename = a.FMH_FeeName,

                                       }
               ).Distinct().ToArray();

                //List<FeeTermDMO> terms = new List<FeeTermDMO>();
                //terms = _FeeGroupContext.feeTr.Where(t => t.FMT_ActiveFlag == true && t.MI_Id == data.MI_ID).ToList();
                //data.fillterms = terms.ToArray();

                data.fillterms = (from a in _FeeGroupContext.feeMTH
                                  from b in _FeeGroupContext.feeTr
                                  from c in _FeeGroupContext.FEeGroupLoginPreviledgeDMO
                                  where (a.FMH_Id == c.FMH_Id && a.FMT_Id == b.FMT_Id && a.MI_Id == data.MI_ID && c.User_Id == data.userid) /*&& a.fmg_id.Contains(data.fmg_id)*/
                                  select new FeeStudentTransactionDTO
                                  {
                                      FMT_Name = b.FMT_Name,
                                      FMT_Id = a.FMT_Id,
                                  }
                         ).Distinct().ToArray();


                List<MasterRouteDMO> installments = new List<MasterRouteDMO>();
                installments = _FeeGroupContext.MasterRouteDMO.Where(i => i.TRMR_ActiveFlg == true && i.MI_Id == data.MI_ID).ToList();
                data.fillinstallment = installments.ToArray();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }


        public FeeTransactionPaymentDTO getheadisbygrpid(FeeTransactionPaymentDTO data)
        {
            List<long> GrpId = new List<long>();
            try
            {

                data.fillmasterhead = (from a in _FeeGroupContext.FeeHeadDMO
                                       from b in _FeeGroupContext.FeeYearlygroupHeadMappingDMO
                                       from c in _FeeGroupContext.FEeGroupLoginPreviledgeDMO
                                       where (a.FMH_Id == b.FMH_Id && b.MI_Id == data.MI_ID && b.ASMAY_Id == data.ASMAY_Id && b.FMG_Id == c.FMG_ID && c.User_Id == data.userid && a.FMH_ActiveFlag == true)
                                       select new FeeRefundDTO
                                       {
                                           fmh_id = a.FMH_Id,
                                           fmh_feename = a.FMH_FeeName,

                                       }
               ).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public FeeTransactionPaymentDTO getsection(FeeTransactionPaymentDTO data)
        {
            List<long> GrpId = new List<long>();
            try
            {

                List<School_M_Section> section = new List<School_M_Section>();
                data.fillsection = (from a in _FeeGroupContext.School_Adm_Y_StudentDMO
                                    from b in _FeeGroupContext.school_M_Section
                                    where (a.ASMS_Id == b.ASMS_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id && b.MI_Id == data.MI_ID)
                                    select new FeeRefundDTO
                                    {
                                        AMSC_Id = b.ASMS_Id,
                                        asmc_sectionname = b.ASMC_SectionName
                                    }
                          ).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }


        public async Task<FeeTransactionPaymentDTO> radiobtndata([FromBody] FeeTransactionPaymentDTO temp)
        {
            List<long> GrpId = new List<long>();

            var fmt_ids = "";

            foreach (var x in temp.FMT_Ids)
            {
                fmt_ids += x + ",";
            }
            fmt_ids = fmt_ids.Substring(0, (fmt_ids.Length - 1));


            using (var cmd = _FeeGroupContext.Database.GetDbConnection().CreateCommand())
            {
                cmd.CommandText = "fee_report_details_1";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@asmay_id",
                    SqlDbType.VarChar)
                {
                    Value = temp.ASMAY_Id
                });
                cmd.Parameters.Add(new SqlParameter("@asmcl_id",
                   SqlDbType.VarChar)
                {
                    Value = temp.ASMCL_Id
                });

                cmd.Parameters.Add(new SqlParameter("@asms_id",
               SqlDbType.VarChar)
                {
                    Value = temp.AMSC_Id
                });
                cmd.Parameters.Add(new SqlParameter("@fmh_id",
                SqlDbType.VarChar)
                {
                    Value = temp.FMH_Id
                });
                cmd.Parameters.Add(new SqlParameter("@fmt_ids",
              SqlDbType.VarChar)
                {
                    Value = fmt_ids
                });

                cmd.Parameters.Add(new SqlParameter("@type",
             SqlDbType.VarChar)
                {
                    Value = temp.radioval
                });
                cmd.Parameters.Add(new SqlParameter("@trmr_id",
           SqlDbType.VarChar)
                {
                    Value = temp.TRMR_Id
                });
                cmd.Parameters.Add(new SqlParameter("@mi_id",
           SqlDbType.VarChar)
                {
                    Value = temp.MI_ID
                });
                cmd.Parameters.Add(new SqlParameter("@userid",
           SqlDbType.VarChar)
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
                cmd.Parameters.Add(new SqlParameter("@report",
             SqlDbType.VarChar)
                {
                    Value = temp.reporttype
                });
                cmd.Parameters.Add(new SqlParameter("@details",
             SqlDbType.VarChar)
                {
                    Value = temp.type
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

                    if (temp.reporttype == "all")
                    {
                        if (temp.radioval == "FSW")
                        {
                            temp.studentalldata = retObject.ToArray();
                        }
                        else if (temp.radioval == "FRW")
                        {
                            temp.groupalldata = retObject.ToArray();
                        }
                        else if (temp.radioval == "FHW")
                        {
                            temp.headalldata = retObject.ToArray();
                        }
                        else if (temp.radioval == "FCW")
                        {
                            temp.classalldata = retObject.ToArray();
                        }
                    }

                    else
                    {
                        if (temp.type == "OB")
                        {
                            temp.categorydata = retObject.ToArray();
                        }
                        else if (temp.type == "AA")
                        {
                            temp.fillstudent = retObject.ToArray();
                        }
                        else if (temp.type == "EA")
                        {
                            temp.fillterms = retObject.ToArray();
                        }
                        else if (temp.type == "WO")
                        {
                            temp.duration = retObject.ToArray();
                        }
                        else if (temp.type == "RA")
                        {
                            temp.rebate = retObject.ToArray();
                        }
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                return temp;
            }

        }



    }
}

