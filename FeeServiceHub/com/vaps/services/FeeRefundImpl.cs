using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs;
using FeeServiceHub.com.vaps.interfaces;
using System.Collections.Concurrent;
using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Text;
using System.IO;
using PreadmissionDTOs.com.vaps.Fees;
using DomainModel.Model.com.vaps.Fee;
using PreadmissionDTOs.com.vaps.admission;
using DomainModel.Model.com.vaps.admission;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;

namespace FeeServiceHub.com.vaps.services
{
    public class FeeRefundImpl : FeeRefundInterface
    {
        public FeeGroupContext _FeeGroupContext;
        //   public AdmissionFormContext _AdmissionFormContext;
        public FeeRefundImpl(FeeGroupContext frgContext)
        {
            _FeeGroupContext = frgContext;

        }
        

        public  FeeRefundDTO getalldetails(FeeRefundDTO id)
        {
            FeeRefundDTO ctdo = new FeeRefundDTO();
            try
            {
              
                List<MasterAcademic> year = new List<MasterAcademic>();
                year = _FeeGroupContext.AcademicYear.Where(y => y.Is_Active == true && y.MI_Id == id.MI_ID).OrderByDescending(y => y.ASMAY_Order).ToList();
                id.academicyr = year.GroupBy(y => y.ASMAY_Year).Select(y => y.First()).ToArray();


                List<School_M_Class> allclas = new List<School_M_Class>();
                allclas = _FeeGroupContext.admissioncls.Where(t => t.MI_Id == id.MI_ID  && t.ASMCL_ActiveFlag == true).ToList();
                id.fillclass = allclas.ToArray();

                //List<FeeHeadDMO> feehead = new List<FeeHeadDMO>();
                //feehead = _FeeGroupContext.FeeHeadDMO.Where(f => f.MI_Id == id.MI_ID && f.FMH_ActiveFlag == true ).ToList();
                //id.fillfeehead = feehead.ToArray();

                id.fillfeehead = (from a in _FeeGroupContext.FeeHeadDMO
                                  from b in _FeeGroupContext.FeeYearlygroupHeadMappingDMO
                                  from c in _FeeGroupContext.FEeGroupLoginPreviledgeDMO
                                  where (a.FMH_Id == b.FMH_Id && b.MI_Id == id.MI_ID && b.ASMAY_Id == id.asmyid && c.User_Id==id.userid && b.FMG_Id == c.FMG_ID && a.FMH_ActiveFlag==true)
                                    select new FeeRefundDTO
                                    {
                                        fmh_id = a.FMH_Id,
                                        fmh_feename = a.FMH_FeeName,
                                       
                                    }
                ).Distinct().ToArray();



            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return id;
        }

        public FeeRefundDTO getsection(FeeRefundDTO data)
        {
            List<long> GrpId = new List<long>();
            try
            {
                List<School_M_Section> section = new List<School_M_Section>();
                data.fillsec = (from a in _FeeGroupContext.School_Adm_Y_StudentDMO
                                    from b in _FeeGroupContext.school_M_Section
                                    where (a.ASMS_Id == b.ASMS_Id && a.ASMAY_Id == data.asmyid && a.ASMCL_Id == data.ASMCL_Id && b.MI_Id == data.MI_ID)
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

        public FeeRefundDTO getstudent(FeeRefundDTO data)
        {
            List<long> GrpId = new List<long>();
            try
            {
                List<School_M_Section> section = new List<School_M_Section>();


                data.fillfeehead = (from a in _FeeGroupContext.FeeHeadDMO
                                  from b in _FeeGroupContext.FeeYearlygroupHeadMappingDMO
                                  from c in _FeeGroupContext.FEeGroupLoginPreviledgeDMO
                                  where (a.FMH_Id == b.FMH_Id && b.MI_Id == data.MI_ID && b.ASMAY_Id == data.asmyid && c.User_Id == data.userid && b.FMG_Id == c.FMG_ID && a.FMH_ActiveFlag == true)
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

        public async Task<FeeRefundDTO> getreport(FeeRefundDTO data)
        {

            long cls = 0;
            long sec = 0;
            if(data.regornamedetails== "all")
            {
                cls = 0;
                sec = 0;
            }
            else
            {
                cls = data.ASMCL_Id;
                sec = data.AMSC_Id;
            }


            var retObject = new List<dynamic>();
            using (var cmd = _FeeGroupContext.Database.GetDbConnection().CreateCommand())
            {
                cmd.CommandText = "fee_refund_report";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@asmay_id",
                    SqlDbType.BigInt)
                {
                    Value = Convert.ToInt64(data.asmyid)
                });

                cmd.Parameters.Add(new SqlParameter("@asmcl_id",
                  SqlDbType.BigInt)
                {
                    Value = cls
                });

                cmd.Parameters.Add(new SqlParameter("@asmc_id",
                  SqlDbType.BigInt)
                {
                    Value = sec
                });
                cmd.Parameters.Add(new SqlParameter("@mi_id",
                 SqlDbType.BigInt)
                {
                    Value = Convert.ToInt64(data.MI_ID)
                });

                cmd.Parameters.Add(new SqlParameter("@fmh_id",
                  SqlDbType.BigInt)
                {
                    Value = Convert.ToInt64(data.fmh_id)
                });
                cmd.Parameters.Add(new SqlParameter("@from_date",
                        SqlDbType.DateTime)
                {
                    Value = data.Fromdate
                });
                cmd.Parameters.Add(new SqlParameter("@to_date",
                 SqlDbType.DateTime)
                {
                    Value = data.Todate
                });
                if (cmd.Connection.State != ConnectionState.Open)
                    cmd.Connection.Open();

                // var retObject = new List<dynamic>();

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
                                    dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null 
                                );
                            }

                            retObject.Add((ExpandoObject)dataRow);
                        }

                    }

                    data.refunddata = retObject.ToArray();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                return data;
            }
        }

            //public FeeRefundDTO getstuddet(FeeRefundDTO data)
            //{
            //    throw new NotImplementedException();
            //}
        }
}
