using CommonLibrary;
using DataAccessMsSqlServerProvider.NAAC;
using DomainModel.Model.NAAC.Admission;
using PreadmissionDTOs.NAAC.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.Admission.Services
{
    public class NAAC_Criteria_6_ReportImpl : Interface.NAAC_Criteria_6_ReportInterface
    {
        public GeneralContext _context;
        public NAAC_Criteria_6_ReportImpl(GeneralContext w)
        {
            _context = w;
        }
        public NAAC_Criteria_6_DTO loaddata(NAAC_Criteria_6_DTO data)
        {
            try
            {
                
                data.allacademicyear = _context.Academic.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).Distinct().OrderByDescending(t => t.ASMAY_Year).ToArray();

                var getinstitution = _context.Institution.Where(a => a.MI_Id == data.MI_Id).ToList();

                string NAACSL_InstitutionTypeFlg = "";
                List<long> miid = new List<long>();
                if (getinstitution.Count() > 0)
                {
                    NAACSL_InstitutionTypeFlg = getinstitution.FirstOrDefault().MI_NAAC_InstitutionTypeFlg;
                }

                NAAC_CommonDetails naaccomm = new NAAC_CommonDetails(_context);

                data.getinstitutioncycle = naaccomm.get_cycle_list(data.MI_Id, data.UserId);

                data.getinstitution = naaccomm.get_Institution_list(data.MI_Id, data.UserId);



            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
            return data;
        }
        public NAAC_Criteria_6_DTO get_report(NAAC_Criteria_6_DTO data)
        {
            try
            {
  

        List<long> mi_id = new List<long>();

                if (data.yerlistdata.Length > 0)
                {
                    foreach (var item in data.yerlistdata)
                    {
                        mi_id.Add(item.MI_Id);
                    }
                  
                }
                
                string yearid = "";
                NAAC_CommonDetails naaccomm = new NAAC_CommonDetails(_context);

                yearid = naaccomm.get_Year_list(data.MI_Id, data.UserId, data.cycleid);

                
                List<string> asmyid = yearid.Split(',').ToList();
                
                if (data.org== "623")
                {
                    data.alldata1 = (from a in _context.Academic
                                     from b in _context.NAAC_AC_623_EGovernance_DMO
                                     where (a.MI_Id == b.MI_Id && a.Is_Active == true && a.ASMAY_Id == b.NCAC623EGOV_ImpYear && asmyid.Contains(b.NCAC623EGOV_ImpYear.ToString()) && mi_id.Contains(b.MI_Id))
                                     select new NAAC_Criteria_6_DTO
                                     {
                                         NCAC623EGOV_Id = b.NCAC623EGOV_Id,
                                         Name = b.NCAC623EGOV_GovernanceArea,
                                         ASMAY_Id = b.NCAC623EGOV_ImpYear,
                                         ASMAY_Year = a.ASMAY_Year,
                                         flag2 = b.NCAC623EGOV_ActiveFlg,
                                     }).Distinct().ToArray();

                    data.savedresult = (
                                     from b in _context.NAAC_AC_623_EGovernance_DMO
                                     from c in _context.NAAC_AC_623_EGovernance_Files_DMO
                                     where (asmyid.Contains(b.NCAC623EGOV_ImpYear.ToString()) && b.NCAC623EGOV_Id == c.NCAC623EGOV_Id && mi_id.Contains(b.MI_Id))
                                     select new NAAC_Criteria_6_DTO
                                     {
                                         NCAC623EGOV_Id = b.NCAC623EGOV_Id,
                                         filepath = c.NCAC623EGOVF_FilePath,
                                         FileName = c.NCAC623EGOVF_FileName,
                                         description=c.NCAC623EGOVF_Filedesc
                                     }).Distinct().ToArray();


                }
                else if (data.org == "632")
                {
                    data.alldata1 = (from a in _context.Academic
                                     from b in _context.NAAC_AC_632_FinanceSupport_DMO
                                     where (asmyid.Contains(b.NCAC632FINSUP_Year.ToString())
                                     && mi_id.Contains(b.MI_Id) &&  a.MI_Id == b.MI_Id 
                                     && b.NCAC632FINSUP_Year == a.ASMAY_Id && a.Is_Active == true )
                                     select new NAAC_Criteria_6_DTO
                                     {
                                         NCAC632FINSUP_Id = b.NCAC632FINSUP_Id,
                                         Name = b.NCAC632FINSUP_TeacherName,
                                         ASMAY_Id = b.NCAC632FINSUP_Year,
                                         amount = Convert.ToInt32(b.NCAC632FINSUP_AmountPaid),
                                         flag7 = b.NCAC632FINSUP_Name,
                                         flag6 = b.NCAC632FINSUP_ConferenceProfBodyFlg,
                                         ASMAY_Year = a.ASMAY_Year,
                                         flag2 = b.NCAC632FINSUP_ActiveFlg,
                                     }).Distinct().ToArray();

                    data.savedresult = (
                                  from b in _context.NAAC_AC_632_FinanceSupport_DMO
                                  from c in _context.NAAC_AC_632_FinanceSupport_Files_DMO
                                  where (asmyid.Contains(b.NCAC632FINSUP_Year.ToString()) && b.NCAC632FINSUP_Id == c.NCAC632FINSUP_Id && mi_id.Contains(b.MI_Id))
                                  select new NAAC_Criteria_6_DTO
                                  {
                                      NCAC632FINSUP_Id = b.NCAC632FINSUP_Id,
                                      filepath = c.NCAC632FINSUPF_FilePath,
                                      FileName = c.NCAC632FINSUPF_FileName,
                                      description = c.NCAC632FINSUPF_Filedesc
                                  }).Distinct().ToArray();
                }
                else if (data.org == "633")
                {
                    data.alldata1 = (from a in _context.Academic
                                     from b in _context.NAAC_AC_633_AdmTraining_DMO
                                     where (a.MI_Id == b.MI_Id && a.Is_Active == true && a.ASMAY_Id == b.NCAC633ADMTRG_Year && asmyid.Contains(b.NCAC633ADMTRG_Year.ToString()) && mi_id.Contains(b.MI_Id))
                                     select new NAAC_Criteria_6_DTO
                                     {
                                         NCAC633ADMTRG_Id = b.NCAC633ADMTRG_Id,
                                         Name = b.NCAC633ADMTRG_Title,
                                         ASMAY_Id = b.NCAC633ADMTRG_Year,
                                         TotalCount = b.NCAC633ADMTRG_NoOfParticipants,
                                         fdate = b.NCAC633ADMTRG_FromDate.ToString("dd/MM/yyyy"),
                                         tdate = b.NCAC633ADMTRG_ToDate.ToString("dd/MM/yyyy"),
                                         flag7 = b.NCAC633ADMTRG_ProfDevAdmTrgFlg,
                                         ASMAY_Year = a.ASMAY_Year,
                                         flag2 = b.NCAC633ADMTRG_ActiveFlg,
                                     }).Distinct().ToArray();

                    data.savedresult = (
                                 from b in _context.NAAC_AC_633_AdmTraining_DMO
                                 from c in _context.NAAC_AC_633_AdmTraining_files_DMO
                                 where (asmyid.Contains(b.NCAC633ADMTRG_Year.ToString()) && b.NCAC633ADMTRG_Id == c.NCAC633ADMTRG_Id && asmyid.Contains(b.NCAC633ADMTRG_Year.ToString()) && mi_id.Contains(b.MI_Id))
                                 select new NAAC_Criteria_6_DTO
                                 {
                                     NCAC633ADMTRG_Id = b.NCAC633ADMTRG_Id,
                                     filepath = c.NCAC633ADMTRGF_FilePath,
                                     FileName = c.NCAC633ADMTRGF_FileName,
                                     description = c.NCAC633ADMTRGF_Filedesc
                                 }).Distinct().ToArray();
                }
                else if (data.org == "634")
                {
                    data.alldata1 = (from a in _context.Academic
                                     from b in _context.NAAC_AC_634_DevPrograms_DMO
                                     where (a.MI_Id == b.MI_Id && a.Is_Active == true  && a.ASMAY_Id == b.NCAC634DEVPRG_Year && asmyid.Contains(b.NCAC634DEVPRG_Year.ToString()) && mi_id.Contains(b.MI_Id))
                                     select new NAAC_Criteria_6_DTO
                                     {
                                         NCAC634DEVPRG_Id = b.NCAC634DEVPRG_Id,
                                         Name = b.NCAC634DEVPRG_PDProgTitle,
                                         TotalCount = b.NCAC634DEVPRG_NoOfTeachersAttnd,
                                         fdate = b.NCAC634DEVPRG_FromDate.ToString("dd/MM/yyyy"),
                                         tdate = b.NCAC634DEVPRG_ToDate.ToString("dd/MM/yyyy"),
                                         ASMAY_Year = a.ASMAY_Year,
                                         flag2 = b.NCAC634DEVPRG_ActiveFlg,
                                     }).Distinct().ToArray();

                    data.savedresult = (
                              from b in _context.NAAC_AC_634_DevPrograms_DMO
                              from c in _context.NAAC_AC_634_DevPrograms_files_DMO
                              where (asmyid.Contains(b.NCAC634DEVPRG_Year.ToString()) && b.NCAC634DEVPRG_Id == c.NCAC634DEVPRG_Id && mi_id.Contains(b.MI_Id))
                               select new NAAC_Criteria_6_DTO
                               {
                                   NCAC634DEVPRG_Id = b.NCAC634DEVPRG_Id,
                                   filepath = c.NCAC634DEVPRGF_FilePath,
                                   FileName = c.NCAC634DEVPRGF_FileName,
                                   description = c.NCAC634DEVPRGF_Filedesc
                               }).Distinct().ToArray();
                }
                else if (data.org == "642")
                {
                    data.alldata1 = (from a in _context.Academic
                                     from b in _context.NAAC_AC_642_Funds_DMO
                                     where (a.MI_Id == b.MI_Id && a.Is_Active == true  && a.ASMAY_Id == b.NCAC642FUND_Year && asmyid.Contains(b.NCAC642FUND_Year.ToString()) && mi_id.Contains(b.MI_Id))
                                     select new NAAC_Criteria_6_DTO
                                     {
                                         NCAC642FUND_Id = b.NCAC642FUND_Id,
                                         Name = b.NCAC642FUND_AgencyName,
                                         ASMAY_Id = b.NCAC642FUND_Year,
                                         amount = b.NCAC642FUND_Amount,
                                         flag7 = b.NCAC642FUND_Initiative,
                                         ASMAY_Year = a.ASMAY_Year,
                                         flag2 = b.NCAC642FUND_ActiveFlg,
                                     }).Distinct().ToArray();


                    data.savedresult = (
                            from b in _context.NAAC_AC_642_Funds_DMO
                            from c in _context.NAAC_AC_642_Funds_files_DMO
                             where (b.NCAC642FUND_Id==c.NCAC642FUND_Id && asmyid.Contains(b.NCAC642FUND_Year.ToString()) && mi_id.Contains(b.MI_Id))
                             select new NAAC_Criteria_6_DTO
                             {
                                 NCAC642FUND_Id = b.NCAC642FUND_Id,
                                 filepath = c.NCAC642FUNDF_FilePath,
                                 FileName = c.NCAC642FUNDF_FileName,
                                 description = c.NCAC642FUNDF_Filedesc
                             }).Distinct().ToArray();
                }
                else if (data.org == "653")
                {
                    data.alldata1 = (from a in _context.Academic
                                     from b in _context.NAAC_AC_653_IQAC_DMO                                
                                     where (a.MI_Id == b.MI_Id && a.Is_Active == true && a.ASMAY_Id == b.NCAC653IQAC_Year && asmyid.Contains(b.NCAC653IQAC_Year.ToString()) && mi_id.Contains(b.MI_Id))
                                     select new NAAC_Criteria_6_DTO
                                     {
                                         NCAC653IQAC_Id = b.NCAC653IQAC_Id,
                                         Name = b.NCAC653IQAC_QualityName,
                                         ASMAY_Id = b.NCAC653IQAC_Year,
                                         fdate = b.NCAC653IQAC_Date.ToString("dd/MM/yyyy"),
                                         TotalCount = b.NCAC653IQAC_NoOfParticipants,
                                         ASMAY_Year = a.ASMAY_Year,
                                         flag2 = b.NCAC653IQAC_ActiveFlg,
                                      
                                     }).Distinct().ToArray();

                    data.savedresult = (
                        from b in _context.NAAC_AC_653_IQAC_DMO
                        from c in _context.NAAC_AC_653_IQAC_files_DMO
                        where ( b.NCAC653IQAC_Id == c.NCAC653IQAC_Id && asmyid.Contains(b.NCAC653IQAC_Year.ToString()) && mi_id.Contains(b.MI_Id))
                         select new NAAC_Criteria_6_DTO
                         {
                             NCAC653IQAC_Id = b.NCAC653IQAC_Id,
                             filepath = c.NCAC653IQACF_FilePath,
                             FileName = c.NCAC653IQACF_FileName,
                             description = c.NCAC653IQACF_Filedesc
                         }).Distinct().ToArray();
                }
                else 
                {
                    data.alldata1 = (from a in _context.Academic
                                     from b in _context.NAAC_AC_654_QualityAssurance_DMO
                                     from c in _context.NAAC_AC_654_QualityAssurance_files_DMO
                                     where (a.MI_Id == b.MI_Id && a.Is_Active == true && a.ASMAY_Id == b.NCAC654QUAS_Year && b.NCAC654QUAS_Id == c.NCAC654QUAS_Id && asmyid.Contains(b.NCAC654QUAS_Year.ToString()) && mi_id.Contains(b.MI_Id))
                                     select new NAAC_Criteria_6_DTO
                                     {
                                         NCAC654QUAS_Id = b.NCAC654QUAS_Id,
                                         ASMAY_Id = b.NCAC654QUAS_Year,
                                         ASMAY_Year = a.ASMAY_Year,
                                         flag1 = b.NCAC654QUAS_ISOFlg,
                                         flag2 = b.NCAC654QUAS_NBAFlg,
                                         flag3 = b.NCAC654QUAS_NIRFFlg,
                                         flag4 = b.NCAC654QUAS_AQARFlg,
                                         flag5 = b.NCAC654QUAS_AAAFlg,
                                         flag6 = b.NCAC654QUAS_ActiveFlg,
                                         filepath = c.NCAC654QUASF_FilePath,
                                     }).Distinct().ToArray();

                    data.savedresult = (
                      from b in _context.NAAC_AC_654_QualityAssurance_DMO
                      from c in _context.NAAC_AC_654_QualityAssurance_files_DMO
                      where (b.NCAC654QUAS_Id == c.NCAC654QUAS_Id && asmyid.Contains(b.NCAC654QUAS_Year.ToString()) && mi_id.Contains(b.MI_Id))
                      select new NAAC_Criteria_6_DTO
                      {
                          NCAC654QUAS_Id = b.NCAC654QUAS_Id,
                          filepath = c.NCAC654QUASF_FilePath,
                          FileName = c.NCAC654QUASF_FileName,
                          description = c.NCAC654QUASF_Filedesc
                      }).Distinct().ToArray();
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
