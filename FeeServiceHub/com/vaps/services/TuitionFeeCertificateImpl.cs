using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.com.vaps.Fees;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace FeeServiceHub.com.vaps.services
{
    public class TuitionFeeCertificateImpl : interfaces.TuitionFeeCertificateInterface
    {
        private static ConcurrentDictionary<string, TuitionFeeCertificate_DTO> _login =
               new ConcurrentDictionary<string, TuitionFeeCertificate_DTO>();



        public FeeGroupContext _FeeGroupContext;
        readonly ILogger<TuitionFeeCertificateImpl> _logger;
        public TuitionFeeCertificateImpl(FeeGroupContext frgContext, ILogger<TuitionFeeCertificateImpl> log)
        {
            _FeeGroupContext = frgContext;
            _logger = log;
        }




        public TuitionFeeCertificateImpl(FeeGroupContext a)
        {
            _FeeGroupContext = a;
        }

        public TuitionFeeCertificate_DTO getdata(TuitionFeeCertificate_DTO data)
        {

            try
            {
                List<MasterAcademic> allyear = new List<MasterAcademic>();
                allyear = _FeeGroupContext.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).OrderByDescending(t => t.ASMAY_Order).ToList(); /*&& t.ASMAY_Id == org.ASMAY_ID*/
                data.academicdrp = allyear.Distinct().ToArray();




                //data.allclasslist = _DomainModelMsSqlServerContext.School_M_Class.Where(t => t.MI_Id == data.MI_Id && t.ASMCL_ActiveFlag == true).ToArray();
                //data.allsectionlist = _DomainModelMsSqlServerContext.AdmSection.Where(t => t.MI_Id == data.MI_Id && t.ASMC_ActiveFlag == 1).ToArray();





                //acdmc.adm_m_student = (from a in _DomainModelMsSqlServerContext.Adm_M_Student
                //                       from b in _DomainModelMsSqlServerContext.SchoolYearWiseStudent
                //                       where (a.AMST_Id == b.AMST_Id && a.AMST_ActiveFlag == 1 && b.AMAY_ActiveFlag == 1 && a.AMST_SOL == "S" && a.MI_Id == stu.MI_Id)
                //                       select new StudycertificateDTO
                //                       {
                //                           AMST_Id = a.AMST_Id,
                //                           AMST_FirstName = ((a.AMST_FirstName == null ? "" : a.AMST_FirstName) + " " + (a.AMST_MiddleName == null ? "" : a.AMST_MiddleName) + " " + (a.AMST_LastName == null ? "" : a.AMST_LastName)).Trim(),
                //                       }).ToArray();

                //data.MasterCompany = (from a in _FeeGroupContext.institution
                //                      where (a.MI_Id == data.MI_Id)
                //                      select new TuitionFeeCertificate_DTO
                //                      {
                //                          companyname = a.IVRMMCT_Name,
                //                          MI_Id = a.MI_Id,
                //                      }).ToArray();
            }



            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
            return data;
        }
        public TuitionFeeCertificate_DTO searchfilter(TuitionFeeCertificate_DTO data)
        {
            try
            {
                data.searchfilter = data.searchfilter.ToUpper();
                var flag = "";
                int flagactive = 0;
                int amatactiveflag = 0;
                if (data.AMST_SOL == "S")
                {
                    flag = "S";
                    flagactive = 1;
                    amatactiveflag = 1;

                }
                else if (data.AMST_SOL == "L")
                {
                    flag = "L";
                    flagactive = 0;
                    amatactiveflag = 0;
                }
                else if (data.AMST_SOL == "D")
                {
                    flag = "D";
                    flagactive = 1;
                    amatactiveflag = 1;
                }

                if (data.allorindid == "A")
                {
                    data.fillstudlist = (from a in _FeeGroupContext.SchoolYearWiseStudent
                                         from b in _FeeGroupContext.Adm_M_Student
                                         where (a.AMST_Id == b.AMST_Id && b.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && b.AMST_SOL == flag && b.AMST_ActiveFlag == flagactive && a.AMAY_ActiveFlag == amatactiveflag && ((b.AMST_FirstName.Trim().ToUpper().Trim() + ' ' + b.AMST_MiddleName.Trim().ToUpper()
                                         + ' ' + b.AMST_LastName.Trim().ToUpper()).Contains(data.searchfilter) || b.AMST_FirstName.Trim().ToUpper().StartsWith(data.searchfilter) || b.AMST_MiddleName.Trim().ToUpper().StartsWith(data.searchfilter) || b.AMST_LastName.Trim().ToUpper().StartsWith(data.searchfilter)))
                                         select new TuitionFeeCertificate_DTO
                                         {
                                             AMST_Id = a.AMST_Id,
                                             AMST_FirstName = ((b.AMST_FirstName == null || b.AMST_FirstName == "0" ? "" : b.AMST_FirstName) + " " + (b.AMST_MiddleName == null || b.AMST_MiddleName == "0" ? "" : b.AMST_MiddleName) + " " + (b.AMST_LastName == null || b.AMST_LastName == "0" ? "" : b.AMST_LastName)).Trim(),

                                         }
             ).ToArray();
                }
                else
                {
                    data.fillstudlist = (from a in _FeeGroupContext.SchoolYearWiseStudent
                                         from b in _FeeGroupContext.Adm_M_Student
                                         where (a.AMST_Id == b.AMST_Id && b.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && b.AMST_SOL == flag && b.AMST_ActiveFlag == flagactive && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMC_Id && a.AMAY_ActiveFlag == amatactiveflag && ((b.AMST_FirstName.Trim().ToUpper() + ' ' + b.AMST_MiddleName.Trim().ToUpper() + ' ' + b.AMST_LastName.Trim().ToUpper()).Contains(data.searchfilter) || b.AMST_FirstName.Trim().ToUpper().StartsWith(data.searchfilter) || b.AMST_MiddleName.Trim().ToUpper().StartsWith(data.searchfilter) || b.AMST_LastName.Trim().ToUpper().StartsWith(data.searchfilter)))
                                         select new TuitionFeeCertificate_DTO
                                         {
                                             AMST_Id = a.AMST_Id,
                                             AMST_FirstName = ((b.AMST_FirstName == null || b.AMST_FirstName == "0" ? "" : b.AMST_FirstName) + " " + (b.AMST_MiddleName == null || b.AMST_MiddleName == "0" ? "" : b.AMST_MiddleName) + " " + (b.AMST_LastName == null || b.AMST_LastName == "0" ? "" : b.AMST_LastName)).Trim(),

                                         }
             ).ToArray();
                }



                if (data.fillstudlist.Length > 0)
                {
                    data.count = data.fillstudlist.Length;
                }
                else
                {
                    data.count = 0;
                }
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
            return data;
        }
        public TuitionFeeCertificate_DTO onchangeyear(TuitionFeeCertificate_DTO data)
        {
            try
            {


                //data.searchfilter = data.searchfilter.ToUpper();
                var flag = "";
                int flagactive = 0;
                int amatactiveflag = 0;
                if (data.AMST_SOL == "S")
                {
                    flag = "S";
                    flagactive = 1;
                    amatactiveflag = 1;

                }
                else if (data.AMST_SOL == "L")
                {
                    flag = "L";
                    flagactive = 0;
                    amatactiveflag = 0;
                }
                else if (data.AMST_SOL == "D")
                {
                    flag = "D";
                    flagactive = 1;
                    amatactiveflag = 1;
                }
                data.allclasslist = _FeeGroupContext.School_M_Class.Where(t => t.MI_Id == data.MI_Id && t.ASMCL_ActiveFlag == true).ToArray();

                //   if (data.allorindi == "A")
                //   {

                //       data.fillstudlist = (from a in _FeeGroupContext.SchoolYearWiseStudent
                //                            from b in _FeeGroupContext.Adm_M_Student
                //                            where (a.AMST_Id == b.AMST_Id && b.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && b.AMST_SOL == flag && b.AMST_ActiveFlag == flagactive && a.AMAY_ActiveFlag == amatactiveflag && ((b.AMST_FirstName.Trim().ToUpper().Trim() + ' ' + b.AMST_MiddleName.Trim().ToUpper()
                //                            + ' ' + b.AMST_LastName.Trim().ToUpper()).Contains(data.searchfilter) || b.AMST_FirstName.Trim().ToUpper().StartsWith(data.searchfilter) || b.AMST_MiddleName.Trim().ToUpper().StartsWith(data.searchfilter) || b.AMST_LastName.Trim().ToUpper().StartsWith(data.searchfilter)))
                //                            select new TuitionFeeCertificate_DTO
                //                            {
                //                                AMST_Id = a.AMST_Id,
                //                                AMST_FirstName = ((b.AMST_FirstName == null || b.AMST_FirstName == "0" ? "" : b.AMST_FirstName) + " " + (b.AMST_MiddleName == null || b.AMST_MiddleName == "0" ? "" : b.AMST_MiddleName) + " " + (b.AMST_LastName == null || b.AMST_LastName == "0" ? "" : b.AMST_LastName)).Trim(),

                //                            }
                //).ToArray();
                //   }
                //   else
                //   {
                //       data.allclasslist = _FeeGroupContext.School_M_Class.Where(t => t.MI_Id == data.MI_Id && t.ASMCL_ActiveFlag == true).ToArray();


                //   }

            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
            return data;
        }
        public TuitionFeeCertificate_DTO onchangeclass(TuitionFeeCertificate_DTO data)
        {
            try
            {
                data.allsectionlist = _FeeGroupContext.AdmSection.Where(t => t.MI_Id == data.MI_Id && t.ASMC_ActiveFlag == 1).ToArray();

            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
            return data;
        }
        public TuitionFeeCertificate_DTO onchangesection(TuitionFeeCertificate_DTO data)
        {
            try
            {

                data.searchfilter = data.searchfilter.ToUpper();
                var flag = "";
                int flagactive = 0;
                int amatactiveflag = 0;
                if (data.AMST_SOL == "S")
                {
                    flag = "S";
                    flagactive = 1;
                    amatactiveflag = 1;

                }
                else if (data.AMST_SOL == "L")
                {
                    flag = "L";
                    flagactive = 0;
                    amatactiveflag = 0;
                }
                else if (data.AMST_SOL == "D")
                {
                    flag = "D";
                    flagactive = 1;
                    amatactiveflag = 1;
                }

                data.fillstudlist = (from a in _FeeGroupContext.SchoolYearWiseStudent
                                     from b in _FeeGroupContext.Adm_M_Student
                                     where (a.AMST_Id == b.AMST_Id && b.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && b.AMST_SOL == flag && b.AMST_ActiveFlag == flagactive && a.AMAY_ActiveFlag == amatactiveflag && ((b.AMST_FirstName.Trim().ToUpper().Trim() + ' ' + b.AMST_MiddleName.Trim().ToUpper()
                                     + ' ' + b.AMST_LastName.Trim().ToUpper()).Contains(data.searchfilter) || b.AMST_FirstName.Trim().ToUpper().StartsWith(data.searchfilter) || b.AMST_MiddleName.Trim().ToUpper().StartsWith(data.searchfilter) || b.AMST_LastName.Trim().ToUpper().StartsWith(data.searchfilter)))
                                     select new TuitionFeeCertificate_DTO
                                     {
                                         AMST_Id = a.AMST_Id,
                                         AMST_FirstName = ((b.AMST_FirstName == null || b.AMST_FirstName == "0" ? "" : b.AMST_FirstName) + " " + (b.AMST_MiddleName == null || b.AMST_MiddleName == "0" ? "" : b.AMST_MiddleName) + " " + (b.AMST_LastName == null || b.AMST_LastName == "0" ? "" : b.AMST_LastName)).Trim(),
                                     }).ToArray();
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
            return data;
        }
        public async Task<TuitionFeeCertificate_DTO> getStudData(TuitionFeeCertificate_DTO data)
        {
            try
            {
                using (var cmd = _FeeGroupContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "[Fee_Tuition_Fee_Certificate]";
                    cmd.CommandType = CommandType.StoredProcedure;
                    //   cmd.Parameters.Add(new SqlParameter("@mywhere", SqlDbType.VarChar) { Value = Convert.ToString(mywhere) });
                    //cmd.Parameters.Add(new SqlParameter("@SOL", SqlDbType.VarChar) { Value = stuDTO.AMST_SOL });
                    cmd.Parameters.Add(new SqlParameter("@yearId", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@classid", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                    cmd.Parameters.Add(new SqlParameter("@sectionid", SqlDbType.VarChar) { Value = data.ASMC_Id });
                    cmd.Parameters.Add(new SqlParameter("@studid", SqlDbType.VarChar) { Value = data.AMST_Id });
                    cmd.Parameters.Add(new SqlParameter("@mi_id", SqlDbType.VarChar) { Value = data.MI_Id });

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
                        data.studentlist = retObject.ToArray();

                    }

                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }

                data.number = data.reF_number;
                data.MasterCompany = (from a in _FeeGroupContext.Institution
                                      where (a.MI_Id == data.MI_Id)
                                      select new TuitionFeeCertificate_DTO
                                      {
                                          companyname = a.IVRMMCT_Name,
                                          MI_Id = a.MI_Id,
                                      }).ToArray();

                data.academicList1 = _FeeGroupContext.AcademicYear.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id).ToArray();

                data.principalsign = _FeeGroupContext.GenConfig.Where(a => a.MI_Id == data.MI_Id).ToArray();


                //                select sum(fss_paidamount) from fee_student_status a
                //inner
                //                                           join fee_master_head b on a.fmh_id = b.fmh_id
                //where a.mi_id = 5 and asmay_id = 9 and amst_id = 6109 and fmh_feename like '%tution%'


                //var results = from p in Products
                //              join o in Orders on p.ProductID equals o.ProductID
                //              where o.Status == 1
                //              group o by p into orderTotals
                //              select new { ProductName = orderTotals.Key.ProductName, TotalAmount = orderTotals.Sum(o => o.Amount) };


               var  amount = (from a in _FeeGroupContext.FeeStudentTransactionDMO
                               from b in _FeeGroupContext.FeeHeadDMO
                               where (a.FMH_Id == b.FMH_Id && a.MI_Id == data.MI_Id &&a.ASMAY_Id==data.ASMAY_Id &&a.AMST_Id == data.AMST_Id && b.FMH_FeeName.Contains("Tution") )
                               group new { a, b } by a.FMH_Id into g1
                               select new TuitionFeeCertificate_DTO
                               {
 
                                   //FMH_FeeName = g1.FirstOrDefault().b.FMH_FeeName,
                                   totalNo=Convert.ToDecimal(g1.Sum(a=>a.a.FSS_PaidAmount))
                                   //totalNo = Convert.ToDecimal(g1.Sum(a=>a.a.FSS_NetAmount)),

                               }).ToList();

                //data.amountdata = amount.Select(t=>t.totalNo).ToArray();
                data.amountdata = amount.Select(t => t.totalNo).ToArray();


            }



            //list = (from m in _context.fee_student_status

            //        where (m.MI_Id == data.MI_Id)
            //        group new { m } by m.FSS_Id into g
            //        select new ReportDTO
            //        {
            //            totalNo = g.Count(),

            //        }).ToList();





            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
            return data;
        }


    }
}
