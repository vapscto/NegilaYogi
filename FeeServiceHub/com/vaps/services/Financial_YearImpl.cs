using DataAccessMsSqlServerProvider;
using DomainModel.Model.com.vapstech.Fee;
using DomainModel.Model.com.vapstech.HRMS;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.com.vaps.Fees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace FeeServiceHub.com.vaps.services
{
    
   
        public class Financial_YearImpl : interfaces.Financial_YearInterface
        {
           
            public DomainModelMsSqlServerContext _context;
            readonly ILogger<FeeStudentTransactionImpl> _logger;
            public Financial_YearImpl(DomainModelMsSqlServerContext context)
            {
               
                _context = context;
            }
            public Financial_YearDTO loaddata(Financial_YearDTO data)
            {
                try
                {
                data.alldata1 = (from a in _context.IVRM_Master_FinancialYear
                                 from b in _context.AcademicYear
                                 where (b.MI_Id == data.MI_Id && b.Is_Active == true &&b.ASMAY_Year.Contains(a.IMFY_AssessmentYear) )
                                 select new Financial_YearDTO
                                 {
                                     IMFY_FromDate = a.IMFY_FromDate,
                                     IMFY_ToDate = a.IMFY_ToDate,
                                     IMFY_AssessmentYear = a.IMFY_AssessmentYear,
                                     IMFY_FinancialYear = a.IMFY_FinancialYear,
                                     ASMAY_Year = b.ASMAY_Year,
                                 }).Distinct().ToArray();


                //data.alldata1 = (from a in _context.IVRM_Master_FinancialYear
                //                from b in _context.AcademicYear
                //                where ( b.MI_Id == data.MI_Id && b.Is_Active == true && a.IMFY_AssessmentYear == b.ASMAY_Year && a.IMFY_FinancialYear == b.ASMAY_Year)
                //                select new Financial_YearDTO
                //                {
                //                    IMFY_FromDate = a.IMFY_FromDate,
                //                    IMFY_ToDate = a.IMFY_ToDate,
                //                    IMFY_AssessmentYear = a.IMFY_AssessmentYear,
                //                    IMFY_FinancialYear = a.IMFY_FinancialYear,
                //                    ASMAY_Year = b.ASMAY_Year,
                //                }).Distinct().ToArray();

               

                data.yeardata = _context.AcademicYear.Where(t => t.MI_Id == data.MI_Id).Distinct().ToArray();
                data.rowcount = _context.IVRM_Master_FinancialYear.Distinct().ToArray();

            }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                return data;
            }

            public Financial_YearDTO save(Financial_YearDTO data)
            {
                try
                {
                var rowcount = _context.IVRM_Master_FinancialYear.Distinct().ToArray();

                long c1 = rowcount.Count() ;
                data.IMFY_OrderBy = c1+1;
                if (data.IMFY_Id == 0)
                    {
                    var duplicate = _context.IVRM_Master_FinancialYear.Where(t => t.IMFY_FromDate == data.IMFY_FromDate && t.IMFY_ToDate == data.IMFY_ToDate && t.IMFY_FinancialYear == data.ASMAY_Year && t.IMFY_AssessmentYear == data.ASMAY_Year && t.IMFY_Id != 0).Distinct().ToArray();
                        if (duplicate.Count() > 0)
                        {
                            data.duplicate = true;
                        }
                        else
                        {
                        var assye = _context.AcademicYear.Where(t => t.ASMAY_Id == data.asmaY_Id).ToList();
                        data.IMFY_FinancialYear = assye[0].ASMAY_Year;
                        var assye1 = _context.AcademicYear.Where(t => t.ASMAY_Id == data.year2).ToList();
                        data.IMFY_AssessmentYear = assye1[0].ASMAY_Year;

                        IVRM_Master_FinancialYear a = new IVRM_Master_FinancialYear();
                            a.IMFY_Id = data.IMFY_Id;
                            a.IMFY_FromDate = data.IMFY_FromDate;
                            a.IMFY_ToDate = data.IMFY_ToDate;
                            a.IMFY_FinancialYear = data.IMFY_FinancialYear;
                            a.IMFY_AssessmentYear = data.IMFY_AssessmentYear;
                        a.IMFY_OrderBy = data.IMFY_OrderBy;

                        a.CreatedDate = DateTime.Now;
                            a.UpdatedDate = DateTime.Now;
                        a.IMFY_OrderBy = data.IMFY_OrderBy;
                          
                            _context.Add(a);
                        }
                        var w = _context.SaveChanges();
                        if (w > 0)
                        {
                            data.msg = "Saved";
                        }
                        else
                        {
                            data.msg = "Failed";
                        }
                    }
                    else if (data.IMFY_Id > 0)
                    {
                        var duplicate = _context.IVRM_Master_FinancialYear.Where(t => t.IMFY_Id != data.IMFY_Id && t.IMFY_FromDate == data.IMFY_FromDate && t.IMFY_ToDate == data.IMFY_ToDate && t.IMFY_FinancialYear == data.ASMAY_Year && t.IMFY_AssessmentYear == data.ASMAY_Year).Distinct().ToArray();
                        if (duplicate.Count() > 0)
                        {
                            data.duplicate = true;
                        }
                        else
                        {
                        var assye = _context.AcademicYear.Where(t => t.ASMAY_Id == data.asmaY_Id).ToList();
                        data.IMFY_FinancialYear = assye[0].ASMAY_Year;
                        var assye1 = _context.AcademicYear.Where(t => t.ASMAY_Id == data.year2).ToList();
                        data.IMFY_AssessmentYear = assye1[0].ASMAY_Year;
                        
                        var j = _context.IVRM_Master_FinancialYear.Where(t =>  t.IMFY_Id == data.IMFY_Id).SingleOrDefault();
                            j.IMFY_Id = data.IMFY_Id;
                     
                            j.IMFY_FromDate = data.IMFY_FromDate;
                            j.IMFY_ToDate = data.IMFY_ToDate;
                            j.IMFY_FinancialYear = data.IMFY_FinancialYear;
                            j.IMFY_AssessmentYear = data.IMFY_AssessmentYear;
                            j.IMFY_OrderBy = data.IMFY_OrderBy;
                        j.CreatedDate = DateTime.Now;
                            j.UpdatedDate = DateTime.Now;
                        _context.Update(j);
                            var r = _context.SaveChanges();
                            if (r > 0)
                            {
                                data.msg = "updated";
                            }
                            else
                            {
                                data.msg = "failed";
                            }

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
