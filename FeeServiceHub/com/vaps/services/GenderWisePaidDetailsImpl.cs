using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using DomainModel.Model.com.vaps.admission;
using Microsoft.AspNetCore.Mvc;
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
    public class GenderWisePaidDetailsImpl : interfaces.GenderWisePaidDetailsInterface
    {

        public FeeGroupContext _FeeGroupContext;
        public GenderWisePaidDetailsImpl(FeeGroupContext frgContext)
        {
            _FeeGroupContext = frgContext;
        }

        public GenderWisePaidDetailsDTO getdata123(GenderWisePaidDetailsDTO data)
        {

            try
            {

                List<MasterAcademic> year = new List<MasterAcademic>();
                year = _FeeGroupContext.AcademicYear.Where(y => y.Is_Active == true && y.MI_Id == data.MI_ID).ToList();
                data.acayear = year.Distinct().GroupBy(y => y.ASMAY_Year).Select(y => y.First()).OrderByDescending(y => y.ASMAY_Order).ToArray();

                data.fillmastergroup = (from a in _FeeGroupContext.feeMTH
                                        from b in _FeeGroupContext.feeTr
                                        from c in _FeeGroupContext.FEeGroupLoginPreviledgeDMO
                                        where (a.FMH_Id == c.FMH_Id && a.FMT_Id == b.FMT_Id && a.MI_Id == data.MI_ID && c.User_Id == data.userid) /*&& a.fmg_id.Contains(data.fmg_id)*/
                                        select new FeeStudentTransactionDTO
                                        {
                                            FMT_Name = b.FMT_Name,
                                            FMT_Id = a.FMT_Id,
                                        }
                   ).Distinct().ToArray();


                data.customlist = (from a in _FeeGroupContext.feegm
                                   from b in _FeeGroupContext.feeGGG
                                   from c in _FeeGroupContext.FEeGroupLoginPreviledgeDMO
                                   where (a.FMGG_Id == b.FMGG_Id && c.FMG_ID == b.FMG_Id && a.FMGG_ActiveFlag == true && a.MI_Id == data.MI_ID && c.User_Id == data.userid)
                                   select new FeeStudentTransactionDTO
                                   {
                                       FMGG_Id = a.FMGG_Id,
                                       fmg_groupname = a.FMGG_GroupName
                                   }
                     ).Distinct().ToArray();


             
                List<long> grpid = new List<long>();

                foreach (FeeStudentTransactionDTO item in data.customlist)
                {
                    grpid.Add(item.FMGG_Id);
                }

                data.grouplist = (from a in _FeeGroupContext.FeeGroupDMO
                                  from b in _FeeGroupContext.feeGGG
                                  from c in _FeeGroupContext.feegm
                                 
                                  where (a.FMG_Id == b.FMG_Id && b.FMGG_Id == c.FMGG_Id && grpid.Contains(c.FMGG_Id) && a.MI_Id == data.MI_ID)
                                  select new FeeStudentTransactionDTO
                                  {
                                      FMG_Id = a.FMG_Id,
                                      FMG_GroupName = a.FMG_GroupName
                                  }
                                  ).Distinct().ToArray();


            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }


        public GenderWisePaidDetailsDTO getsection(GenderWisePaidDetailsDTO data)
        {
           
            try
            {
              
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public GenderWisePaidDetailsDTO getstudent(GenderWisePaidDetailsDTO data)
        {
           
            try
            {
        

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public GenderWisePaidDetailsDTO getstuddet(GenderWisePaidDetailsDTO data)
        {
         
            try
            {
               

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }

        public async Task<GenderWisePaidDetailsDTO> getreport([FromBody] GenderWisePaidDetailsDTO data)
        {
            var retObject1 = new List<dynamic>();
            long fmgg_id = 0;
            var fmgg_ids = "";
            foreach (var x in data.FMGG_Ids)
            {
                fmgg_ids += x + ",";
            }
            fmgg_ids = fmgg_ids.Substring(0, (fmgg_ids.Length - 1));
            //fmgg_id = Convert.ToInt32(fmgg_ids);

            var fmg_ids = "";
            foreach (var x in data.FMG_Ids)
            {
                fmg_ids += x + ",";
            }
            fmg_ids = fmg_ids.Substring(0, (fmg_ids.Length - 1));


            var fmt_ids = "";
            foreach (var x in data.FMT_Ids)
            {
                fmt_ids += x + ",";
            }
            fmt_ids = fmt_ids.Substring(0, (fmt_ids.Length - 1));

            using (var cmd = _FeeGroupContext.Database.GetDbConnection().CreateCommand())
            {
                cmd.CommandText = "genderwisepaiddetails";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@MI_Id",
                    SqlDbType.VarChar)
                {
                    Value = Convert.ToInt64(data.MI_ID)
                });
                cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                   SqlDbType.VarChar)
                {
                    Value = Convert.ToInt64(data.asmay_id)
                });

                cmd.Parameters.Add(new SqlParameter("@FMG_Id",
               SqlDbType.VarChar)
                {
                    Value = fmg_ids
                });
                cmd.Parameters.Add(new SqlParameter("@FMT_Id",
               SqlDbType.VarChar)
                {
                    Value = fmt_ids
                });
                cmd.Parameters.Add(new SqlParameter("@Type",
                   SqlDbType.VarChar)
                {
                    Value = data.type
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
                                    dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                                );
                            }

                            retObject1.Add((ExpandoObject)dataRow);
                        }

                    }

                    data.getreportdata = retObject1.ToArray();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

            }
            return data;
        }



     


        public GenderWisePaidDetailsDTO get_groups(GenderWisePaidDetailsDTO data)
        {
            try
            {

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
    }
}
