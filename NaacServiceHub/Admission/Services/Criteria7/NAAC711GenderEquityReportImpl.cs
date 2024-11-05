using CommonLibrary;
using DataAccessMsSqlServerProvider.NAAC;
using PreadmissionDTOs.NAAC.Admission;
using PreadmissionDTOs.NAAC.Reports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.Admission.Services.Criteria7
{
    public class NAAC711GenderEquityReportImpl : Interface.Criteria7.NAAC711GenderEquityReportInterface
    {
        public GeneralContext _context;
        public NAAC711GenderEquityReportImpl(GeneralContext w)
        {
            _context = w;
        }
        public NAACAC7Report_DTO loaddata(NAACAC7Report_DTO data)
        {
            try
            {
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

                data.NAACSL_InstitutionTypeFlg = NAACSL_InstitutionTypeFlg = getinstitution.FirstOrDefault().MI_NAAC_InstitutionTypeFlg;

                //data.allacademicyear = _context.Academic.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).Distinct().OrderByDescending(t => t.ASMAY_Order).ToArray();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public NAACAC7Report_DTO Report(NAACAC7Report_DTO data)
        {
            try
            {
                List<long> mi_ids = new List<long>();
                if (data.selected_Inst.Length > 0)
                {
                    foreach (var item in data.selected_Inst)
                    {
                        mi_ids.Add(item.MI_Id);
                    }
                }

                string yearid = "";
                NAAC_CommonDetails naaccomm = new NAAC_CommonDetails(_context);
                yearid = naaccomm.get_Year_list(data.MI_Id, data.UserId, data.cycleid);

                if (data.Type == "711GenderEquity")
                {
                    data.alldata = (from a in _context.Academic
                                    from b in _context.NAAC_AC_711_GenderEquityDMO
                                    where (a.MI_Id == b.MI_Id && a.ASMAY_Id == b.NCAC711GENEQ_Year && yearid.Contains(b.NCAC711GENEQ_Year.ToString()) && a.Is_Active == true && mi_ids.Contains(a.MI_Id) && b.NCAC711GENEQ_ActiveFlg == true)
                                    select new NAACAC7Report_DTO
                                    {
                                        NCAC711GENEQ_Id = b.NCAC711GENEQ_Id,
                                        NCAC711GENEQ_ProgramTitle = b.NCAC711GENEQ_ProgramTitle,
                                        NCAC711GENEQ_Year = b.NCAC711GENEQ_Year,
                                        NCAC711GENEQ_FromDate = b.NCAC711GENEQ_FromDate,
                                        ASMAY_Year = a.ASMAY_Year,
                                        NCAC711GENEQ_ToDate = b.NCAC711GENEQ_ToDate,
                                        NCAC711GENEQ_NoOfParticipantsMale = b.NCAC711GENEQ_NoOfParticipantsMale,
                                        NCAC711GENEQ_NoOfParticipantsFeMale = b.NCAC711GENEQ_NoOfParticipantsFeMale,
                                    }).Distinct().OrderByDescending(t => t.NCAC711GENEQ_Id).ToArray();

                    data.alldatafile = (from a in _context.NAAC_AC_711_GenderEquity_FilesDMO
                                        from b in _context.NAAC_AC_711_GenderEquityDMO
                                        where (b.NCAC711GENEQ_Id == a.NCAC711GENEQ_Id && b.NCAC711GENEQ_ActiveFlg == true && yearid.Contains(b.NCAC711GENEQ_Year.ToString()) && mi_ids.Contains(b.MI_Id))
                                        select a).Distinct().OrderByDescending(t => t.NCAC711GENEQ_Id).ToArray();
                }
                else if (data.Type == "713AlternateEnergy")
                {
                    data.alldata = (from a in _context.Academic
                                    from b in _context.NAAC_AC_713_AlternateEnergyDMO
                                    where (mi_ids.Contains(a.MI_Id) && a.MI_Id == b.MI_Id && a.ASMAY_Id == b.NCAC713ALTENE_Year && yearid.Contains(b.NCAC713ALTENE_Year.ToString()) && a.Is_Active == true &&  b.NCAC713ALTENE_ActiveFlg == true)
                                    select new NAACAC7Report_DTO
                                    {
                                        NCAC713ALTENE_Id = b.NCAC713ALTENE_Id,
                                        NCAC713ALTENE_PowerRequirements = b.NCAC713ALTENE_PowerRequirements,
                                        NCAC713ALTENE_Year = b.NCAC713ALTENE_Year,
                                        NCAC713ALTENE_TotalPowerReq = b.NCAC713ALTENE_TotalPowerReq,
                                        ASMAY_Year = a.ASMAY_Year,
                                        NCAC713ALTENE_EnergySource = b.NCAC713ALTENE_EnergySource,
                                        NCAC713ALTENE_EnergyUsed = b.NCAC713ALTENE_EnergyUsed,
                                        NCAC713ALTENE_EnergySupplied = b.NCAC713ALTENE_EnergySupplied,
                                    }).Distinct().OrderByDescending(t => t.NCAC713ALTENE_Id).ToArray();

                    data.alldatafile = (from a in _context.NAAC_AC_713_AlternateEnergy_FilesDMO
                                        from b in _context.NAAC_AC_713_AlternateEnergyDMO
                                        where (mi_ids.Contains(b.MI_Id) && b.NCAC713ALTENE_Id == a.NCAC713ALTENE_Id && b.NCAC713ALTENE_ActiveFlg == true && yearid.Contains(b.NCAC713ALTENE_Year.ToString()))
                                        select a).Distinct().OrderByDescending(t => t.NCAC713ALTENE_Id).ToArray();
                }
                else if (data.Type == "714LEDBulbs")
                {
                    data.alldata = (from a in _context.Academic
                                    from b in _context.NAAC_AC_714_LEDBulbsDMO
                                    where (a.MI_Id == b.MI_Id && a.ASMAY_Id == b.NCAC714LEDBU_Year && yearid.Contains(b.NCAC714LEDBU_Year.ToString()) && a.Is_Active == true && mi_ids.Contains(a.MI_Id) && b.NCAC714LEDBU_ActiveFlg == true)
                                    select new NAACAC7Report_DTO
                                    {
                                        NCAC714LEDBU_Id = b.NCAC714LEDBU_Id,
                                        NCAC714LEDBU_LightingsRequirements = b.NCAC714LEDBU_LightingsRequirements,
                                        NCAC714LEDBU_Year = b.NCAC714LEDBU_Year,
                                        NCAC714LEDBU_LughtingLED = b.NCAC714LEDBU_LughtingLED,
                                        ASMAY_Year = a.ASMAY_Year,
                                        NCAC714LEDBU_OtherSource = b.NCAC714LEDBU_OtherSource
                                    }).Distinct().OrderByDescending(t => t.NCAC714LEDBU_Id).ToArray();

                    data.alldatafile = (from a in _context.NAAC_AC_714_LEDBulbs_FilesDMO
                                        from b in _context.NAAC_AC_714_LEDBulbsDMO
                                        where (mi_ids.Contains(b.MI_Id) && b.NCAC714LEDBU_Id == a.NCAC714LEDBU_Id && b.NCAC714LEDBU_ActiveFlg == true && yearid.Contains(b.NCAC714LEDBU_Year.ToString()))
                                        select a).Distinct().OrderByDescending(t => t.NCAC714LEDBU_Id).ToArray();
                }
                else if (data.Type == "718WasteManagement")
                {
                    data.alldata = (from a in _context.Academic
                                    from b in _context.NAAC_AC_718_WasteManagementDMO
                                    where (a.MI_Id == b.MI_Id && a.ASMAY_Id == b.NCAC718WAMAN_Year && yearid.Contains(b.NCAC718WAMAN_Year.ToString()) && a.Is_Active == true && mi_ids.Contains(a.MI_Id) && b.NCAC718WAMAN_ActiveFlg == true)
                                    select new NAACAC7Report_DTO
                                    {
                                        NCAC718WAMAN_Id = b.NCAC718WAMAN_Id,
                                        NCAC718WAMAN_Expenditure = b.NCAC718WAMAN_Expenditure,
                                        NCAC718WAMAN_Year = b.NCAC718WAMAN_Year,
                                        ASMAY_Year = a.ASMAY_Year
                                    }).Distinct().OrderByDescending(t => t.NCAC718WAMAN_Id).ToArray();

                    data.alldatafile = (from a in _context.NAAC_AC_718_WasteManagement_FilesDMO
                                        from b in _context.NAAC_AC_718_WasteManagementDMO
                                        where (mi_ids.Contains(b.MI_Id) && b.NCAC718WAMAN_Id == a.NCAC718WAMAN_Id && b.NCAC718WAMAN_ActiveFlg == true && yearid.Contains(b.NCAC718WAMAN_Year.ToString()))
                                        select a).Distinct().OrderByDescending(t => t.NCAC718WAMAN_Id).ToArray();
                }
                else if (data.Type == "719DifferentlyAbled")
                {
                    data.alldata = (from a in _context.Academic
                                    from b in _context.NAAC_AC_719_DifferentlyAbledDMO
                                    where (a.MI_Id == b.MI_Id && a.ASMAY_Id == b.NCAC719DIFFAB_Year && yearid.Contains(b.NCAC719DIFFAB_Year.ToString()) && a.Is_Active == true && mi_ids.Contains(a.MI_Id) && b.NCAC719DIFFAB_ActiveFlg == true)
                                    select new NAACAC7Report_DTO
                                    {
                                        NCAC719DIFFAB_Id = b.NCAC719DIFFAB_Id,
                                        NCAC719DIFFAB_LIFTFacilityFlg = b.NCAC719DIFFAB_LIFTFacilityFlg,
                                        NCAC719DIFFAB_Year = b.NCAC719DIFFAB_Year,
                                        NCAC719DIFFAB_PhysicalFacilityFlg = b.NCAC719DIFFAB_PhysicalFacilityFlg,
                                        ASMAY_Year = a.ASMAY_Year,
                                        NCAC719DIFFAB_BrailleSaoftFlg = b.NCAC719DIFFAB_BrailleSaoftFlg,
                                        NCAC719DIFFAB_RestRoomFlg = b.NCAC719DIFFAB_RestRoomFlg,
                                        NCAC719DIFFAB_ExamScribeFlg = b.NCAC719DIFFAB_ExamScribeFlg,
                                        NCAC719DIFFAB_SPLSkilDevFlg = b.NCAC719DIFFAB_SPLSkilDevFlg,
                                        NCAC719DIFFAB_RampFacilityFlg = b.NCAC719DIFFAB_RampFacilityFlg,
                                        NCAC719DIFFAB_OtherFacility = b.NCAC719DIFFAB_OtherFacility,
                                    }).Distinct().OrderByDescending(t => t.NCAC719DIFFAB_Id).ToArray();

                    data.alldatafile = (from a in _context.NAAC_AC_719_DifferentlyAbled_FilesDMO
                                        from b in _context.NAAC_AC_719_DifferentlyAbledDMO
                                        where (mi_ids.Contains(b.MI_Id) && b.NCAC719DIFFAB_Id == a.NCAC719DIFFAB_Id && b.NCAC719DIFFAB_ActiveFlg == true && yearid.Contains(b.NCAC719DIFFAB_Year.ToString()))
                                        select a).Distinct().OrderByDescending(t => t.NCAC719DIFFAB_Id).ToArray();
                }
                else if (data.Type == "7110Locational")
                {
                    data.alldata = (from a in _context.Academic
                                    from b in _context.NAAC_AC_7110_LocationalAdvtgDMO
                                    where (a.MI_Id == b.MI_Id && a.ASMAY_Id == b.NCAC7110LOCADVTG_Year && yearid.Contains(b.NCAC7110LOCADVTG_Year.ToString()) && a.Is_Active == true && mi_ids.Contains(a.MI_Id) && b.NCAC7110LOCADVTG_ActiveFlg == true)
                                    select new NAACAC7Report_DTO
                                    {
                                        NCAC7110LOCADVTG_Id = b.NCAC7110LOCADVTG_Id,
                                        NCAC7110LOCADVTG_NoOfAddress = b.NCAC7110LOCADVTG_NoOfAddress,
                                        NCAC7110LOCADVTG_Year = b.NCAC7110LOCADVTG_Year,
                                        NCAC7110LOCADVTG_NoOfEngage = b.NCAC7110LOCADVTG_NoOfEngage,
                                        ASMAY_Year = a.ASMAY_Year,
                                        NCAC7110LOCADVTG_Date = b.NCAC7110LOCADVTG_Date,
                                        NCAC7110LOCADVTG_Duration = b.NCAC7110LOCADVTG_Duration,
                                        NCAC7110LOCADVTG_InitiativeName = b.NCAC7110LOCADVTG_InitiativeName,
                                        NCAC7110LOCADVTG_IssuesAddressed = b.NCAC7110LOCADVTG_IssuesAddressed,
                                        NCAC7110LOCADVTG_NoOfParticipant = b.NCAC7110LOCADVTG_NoOfParticipant,
                                    }).Distinct().OrderByDescending(t => t.NCAC7110LOCADVTG_Id).ToArray();

                    data.alldatafile = (from a in _context.NAAC_AC_7110_LocationalAdvtg_FilesDMO
                                        from b in _context.NAAC_AC_7110_LocationalAdvtgDMO
                                        where (mi_ids.Contains(b.MI_Id) && b.NCAC7110LOCADVTG_Id == a.NCAC7110LOCADVTG_Id && b.NCAC7110LOCADVTG_ActiveFlg == true && yearid.Contains(b.NCAC7110LOCADVTG_Year.ToString()))
                                        select a).Distinct().OrderByDescending(t => t.NCAC7110LOCADVTG_Id).ToArray();

                    data.alldata11 = (from a in _context.Academic
                                    from b in _context.NAAC_AC_7111_LocalCommunityDMO
                                      where (a.MI_Id == b.MI_Id && a.ASMAY_Id == b.NCAC7111LOCCOM_Year && yearid.Contains(b.NCAC7111LOCCOM_Year.ToString()) && a.Is_Active == true && mi_ids.Contains(a.MI_Id) && b.NCAC7111LOCCOM_ActiveFlg == true)
                                    select new NAACAC7Report_DTO
                                    {
                                        NCAC7111LOCCOM_Id = b.NCAC7111LOCCOM_Id,
                                        NCAC7111LOCCOM_NoOfAddress = b.NCAC7111LOCCOM_NoOfAddress,
                                        NCAC7111LOCCOM_Year = b.NCAC7111LOCCOM_Year,
                                        NCAC7111LOCCOM_NoOfEngage = b.NCAC7111LOCCOM_NoOfEngage,
                                        ASMAY_Year = a.ASMAY_Year,
                                        NCAC7111LOCCOM_Date = b.NCAC7111LOCCOM_Date,
                                        NCAC7111LOCCOM_Duration = b.NCAC7111LOCCOM_Duration,
                                        NCAC7111LOCCOM_InitiativeName = b.NCAC7111LOCCOM_InitiativeName,
                                        NCAC7111LOCCOM_IssuesAddressed = b.NCAC7111LOCCOM_IssuesAddressed,
                                        NCAC7111LOCCOM_NoOfParticipant = b.NCAC7111LOCCOM_NoOfParticipant,
                                    }).Distinct().OrderByDescending(t => t.NCAC7111LOCCOM_Id).ToArray();

                    data.alldata11file = (from a in _context.NAAC_AC_7111_LocalCommunity_FilesDMO
                                          from b in _context.NAAC_AC_7111_LocalCommunityDMO
                                          where (mi_ids.Contains(b.MI_Id) && b.NCAC7111LOCCOM_Id == a.NCAC7111LOCCOM_Id && b.NCAC7111LOCCOM_ActiveFlg == true && yearid.Contains(b.NCAC7111LOCCOM_Year.ToString()))
                                        select a).Distinct().OrderByDescending(t => t.NCAC7111LOCCOM_Id).ToArray();
                }
                else if (data.Type == "7112CodeOfCoduct")
                {
                    data.alldata = (from a in _context.Academic
                                    from b in _context.NAAC_AC_7112_CodeOfCoductDMO
                                    where (a.MI_Id == b.MI_Id && a.ASMAY_Id == b.NCAC7112CODCON_Year && yearid.Contains(b.NCAC7112CODCON_Year.ToString()) && a.Is_Active == true && mi_ids.Contains(a.MI_Id) && b.NCAC7112CODCON_ActiveFlg == true)
                                    select new NAACAC7Report_DTO
                                    {
                                        NCAC7112CODCON_Id = b.NCAC7112CODCON_Id,
                                        NCAC7112CODCON_URL = b.NCAC7112CODCON_URL,
                                        NCAC7112CODCON_Year = b.NCAC7112CODCON_Year,
                                        ASMAY_Year = a.ASMAY_Year
                                    }).Distinct().OrderByDescending(t => t.NCAC7112CODCON_Id).ToArray();

                    data.alldatafile = (from a in _context.NAAC_AC_7112_CodeOfCoduct_FilesDMO
                                        from b in _context.NAAC_AC_7112_CodeOfCoductDMO
                                        where (mi_ids.Contains(b.MI_Id) && b.NCAC7112CODCON_Id == a.NCAC7112CODCON_Id && b.NCAC7112CODCON_ActiveFlg == true && yearid.Contains(b.NCAC7112CODCON_Year.ToString()))
                                        select a).Distinct().OrderByDescending(t => t.NCAC7112CODCON_Id).ToArray();
                }
                else if (data.Type == "7113CoreValues")
                {
                    data.alldata = (from a in _context.Academic
                                    from b in _context.NAAC_AC_7113_CoreValuesDMO
                                    where (a.MI_Id == b.MI_Id && a.ASMAY_Id == b.NCAC7113CORVAL_Year && yearid.Contains(b.NCAC7113CORVAL_Year.ToString()) && a.Is_Active == true && mi_ids.Contains(a.MI_Id) && b.NCAC7113CORVAL_ActiveFlg == true)
                                    select new NAACAC7Report_DTO
                                    {
                                        NCAC7113CORVAL_Id = b.NCAC7113CORVAL_Id,
                                        NCAC7113CORVAL_URL = b.NCAC7113CORVAL_URL,
                                        NCAC7113CORVAL_Year = b.NCAC7113CORVAL_Year,
                                        ASMAY_Year = a.ASMAY_Year
                                    }).Distinct().OrderByDescending(t => t.NCAC7113CORVAL_Id).ToArray();

                    data.alldatafile = (from a in _context.NAAC_AC_7113_CoreValues_FilesDMO
                                        from b in _context.NAAC_AC_7113_CoreValuesDMO
                                        where (mi_ids.Contains(b.MI_Id) && b.NCAC7113CORVAL_Id == a.NCAC7113CORVAL_Id && b.NCAC7113CORVAL_ActiveFlg == true && yearid.Contains(b.NCAC7113CORVAL_Year.ToString()))
                                        select a).Distinct().OrderByDescending(t => t.NCAC7113CORVAL_Id).ToArray();
                }
                else if (data.Type == "7114HumanValues")
                {
                    data.alldata = (from a in _context.Academic
                                    from b in _context.NAAC_AC_7114_HumanValuesDMO
                                    where (a.MI_Id == b.MI_Id && a.ASMAY_Id == b.NCAC7114HUVAL_Year && yearid.Contains(b.NCAC7114HUVAL_Year.ToString()) && a.Is_Active == true && mi_ids.Contains(a.MI_Id) && b.NCAC7114HUVAL_ActiveFlg == true)
                                    select new NAACAC7Report_DTO
                                    {
                                        NCAC7114HUVAL_Id = b.NCAC7114HUVAL_Id,
                                        NCAC7114HUVAL_ProgramTitle = b.NCAC7114HUVAL_ProgramTitle,
                                        NCAC7114HUVAL_Year = b.NCAC7114HUVAL_Year,
                                        NCAC7114HUVAL_NoOfPartcipants = b.NCAC7114HUVAL_NoOfPartcipants,
                                        ASMAY_Year = a.ASMAY_Year,
                                        NCAC7114HUVAL_FromDate = b.NCAC7114HUVAL_FromDate,
                                        NCAC7114HUVAL_ToDate = b.NCAC7114HUVAL_ToDate
                                    }).Distinct().OrderByDescending(t => t.NCAC7114HUVAL_Id).ToArray();

                    data.alldatafile = (from a in _context.NAAC_AC_7114_HumanValues_FilesDMO
                                        from b in _context.NAAC_AC_7114_HumanValuesDMO
                                        where (mi_ids.Contains(b.MI_Id) && b.NCAC7114HUVAL_Id == a.NCAC7114HUVAL_Id && b.NCAC7114HUVAL_ActiveFlg == true && yearid.Contains(b.NCAC7114HUVAL_Year.ToString()))
                                        select a).Distinct().OrderByDescending(t => t.NCAC7114HUVAL_Id).ToArray();
                }
                else if (data.Type == "7115ProfessionalEthics")
                {
                    data.alldata = (from a in _context.Academic
                                    from b in _context.NAAC_AC_7115_ProfessionalEthicsDMO
                                    where (a.MI_Id == b.MI_Id && a.ASMAY_Id == b.NCAC7115PROETH_Year && yearid.Contains(b.NCAC7115PROETH_Year.ToString()) && a.Is_Active == true && mi_ids.Contains(a.MI_Id) && b.NCAC7115PROETH_ActiveFlg == true)
                                    select new NAACAC7Report_DTO
                                    {
                                        NCAC7115PROETH_Id = b.NCAC7115PROETH_Id,
                                        NCAC7115PROETH_URL = b.NCAC7115PROETH_URL,
                                        NCAC7115PROETH_Year = b.NCAC7115PROETH_Year,
                                        ASMAY_Year = a.ASMAY_Year
                                    }).Distinct().OrderByDescending(t => t.NCAC7115PROETH_Id).ToArray();

                    data.alldatafile = (from a in _context.NAAC_AC_7115_ProfessionalEthics_FilesDMO
                                        from b in _context.NAAC_AC_7115_ProfessionalEthicsDMO
                                        where (mi_ids.Contains(b.MI_Id) && b.NCAC7115PROETH_Id == a.NCAC7115PROETH_Id && b.NCAC7115PROETH_ActiveFlg == true && yearid.Contains(b.NCAC7115PROETH_Year.ToString()))
                                        select a).Distinct().OrderByDescending(t => t.NCAC7115PROETH_Id).ToArray();
                }
                else if (data.Type == "7116StatutoryBodies")
                {
                    data.alldata = (from a in _context.Academic
                                    from b in _context.NAAC_AC_7116_StatutoryBodiesDMO
                                    where (a.MI_Id == b.MI_Id && a.ASMAY_Id == b.NCAC7116STABOD_Year && yearid.Contains(b.NCAC7116STABOD_Year.ToString()) && a.Is_Active == true && mi_ids.Contains(a.MI_Id) && b.NCAC7116STABOD_ActiveFlg == true)
                                    select new NAACAC7Report_DTO
                                    {
                                        NCAC7116STABOD_Id = b.NCAC7116STABOD_Id,
                                        NCAC7116STABOD_URL = b.NCAC7116STABOD_URL,
                                        NCAC7116STABOD_Year = b.NCAC7116STABOD_Year,
                                        ASMAY_Year = a.ASMAY_Year
                                    }).Distinct().OrderByDescending(t => t.NCAC7116STABOD_Id).ToArray();

                    data.alldatafile = (from a in _context.NAAC_AC_7116_StatutoryBodies_FilesDMO
                                        from b in _context.NAAC_AC_7116_StatutoryBodiesDMO
                                        where (mi_ids.Contains(b.MI_Id) && b.NCAC7116STABOD_Id == a.NCAC7116STABOD_Id && b.NCAC7116STABOD_ActiveFlg == true && yearid.Contains(b.NCAC7116STABOD_Year.ToString()))
                                        select a).Distinct().OrderByDescending(t => t.NCAC7116STABOD_Id).ToArray();
                }
                else if (data.Type == "7117UniversalValues")
                {
                    data.alldata = (from a in _context.Academic
                                    from b in _context.NAAC_AC_7117_UniversalValuesDMO
                                    where (a.MI_Id == b.MI_Id && a.ASMAY_Id == b.NCAC7117UNIVAL_Year && yearid.Contains(b.NCAC7117UNIVAL_Year.ToString()) && a.Is_Active == true && mi_ids.Contains(a.MI_Id) && b.NCAC7117UNIVAL_ActiveFlg == true)
                                    select new NAACAC7Report_DTO
                                    {
                                        NCAC7117UNIVAL_Id = b.NCAC7117UNIVAL_Id,
                                        NCAC7117UNIVAL_ProgramTitle = b.NCAC7117UNIVAL_ProgramTitle,
                                        NCAC7117UNIVAL_Year = b.NCAC7117UNIVAL_Year,
                                        NCAC7117UNIVAL_NoOfPartcipants = b.NCAC7117UNIVAL_NoOfPartcipants,
                                        ASMAY_Year = a.ASMAY_Year,
                                        NCAC7117UNIVAL_FromDate = b.NCAC7117UNIVAL_FromDate,
                                        NCAC7117UNIVAL_ToDate = b.NCAC7117UNIVAL_ToDate
                                    }).Distinct().OrderByDescending(t => t.NCAC7117UNIVAL_Id).ToArray();

                    data.alldatafile = (from a in _context.NAAC_AC_7117_UniversalValues_FilesDMO
                                        from b in _context.NAAC_AC_7117_UniversalValuesDMO
                                        where (mi_ids.Contains(b.MI_Id) && b.NCAC7117UNIVAL_Id == a.NCAC7117UNIVAL_Id && b.NCAC7117UNIVAL_ActiveFlg == true && yearid.Contains(b.NCAC7117UNIVAL_Year.ToString()))
                                        select a).Distinct().OrderByDescending(t => t.NCAC7117UNIVAL_Id).ToArray();
                }
                else if (data.Type == "715WaterConservFacilities")
                {
                    data.alldata = (from a in _context.Academic
                                    from b in _context.NAAC_MC_715_WaterConservFacilitiesDMO
                                    where (a.MI_Id == b.MI_Id && a.ASMAY_Id == b.NCMC715WCF_Year && yearid.Contains(b.NCMC715WCF_Year.ToString()) && a.Is_Active == true && mi_ids.Contains(a.MI_Id) && b.NCMC715WCF_ActiveFlg == true)
                                    select new NAACAC7Report_DTO
                                    {
                                        NCMC715WCF_Id = b.NCMC715WCF_Id,
                                        NCMC715WCF_RainWaterHarvestingFlag = b.NCMC715WCF_RainWaterHarvestingFlag,
                                        NCMC715WCF_Year = b.NCMC715WCF_Year,
                                        NCMC715WCF_BorewellOpenwellRecFlag = b.NCMC715WCF_BorewellOpenwellRecFlag,
                                        ASMAY_Year = a.ASMAY_Year,
                                        NCMC715WCF_ConstructionOftanksbundsFlag = b.NCMC715WCF_ConstructionOftanksbundsFlag,
                                        NCMC715WCF_MaintenanceOfWaterbodiesDSFlag = b.NCMC715WCF_MaintenanceOfWaterbodiesDSFlag
                                    }).Distinct().OrderByDescending(t => t.NCMC715WCF_Id).ToArray();
                }
                else if (data.Type == "716GreenCampusInitiatives")
                {
                    data.alldata = (from a in _context.Academic
                                    from b in _context.NAAC_MC_716_GreenCampusInitiativesDMO
                                    where (a.MI_Id == b.MI_Id && a.ASMAY_Id == b.NCMC716GCI_Year && yearid.Contains(b.NCMC716GCI_Year.ToString()) && a.Is_Active == true && mi_ids.Contains(a.MI_Id) && b.NCMC716GCI_ActiveFlg == true)
                                    select new NAACAC7Report_DTO
                                    {
                                        NCMC716GCI_Id = b.NCMC716GCI_Id,
                                        NCMC716GCI_RestrictedentryOfAutomobilesFlag = b.NCMC716GCI_RestrictedentryOfAutomobilesFlag,
                                        NCMC716GCI_Year = b.NCMC716GCI_Year,
                                        NCMC716GCI_BatterypoweredvehiclesFlag = b.NCMC716GCI_BatterypoweredvehiclesFlag,
                                        ASMAY_Year = a.ASMAY_Year,
                                        NCMC716GCI_PedestrianFriendlyPathwaysFlag = b.NCMC716GCI_PedestrianFriendlyPathwaysFlag,
                                        NCMC716GCI_BanOnTheuseOfPlasticsFlag = b.NCMC716GCI_BanOnTheuseOfPlasticsFlag,
                                        NCMC716GCI_LandscapingwithtreesplantsFlag = b.NCMC716GCI_LandscapingwithtreesplantsFlag
                                    }).Distinct().OrderByDescending(t => t.NCAC7117UNIVAL_Id).ToArray();
                }
                else if (data.Type == "717DisabledFriendlyEnvironment")
                {
                    data.alldata = (from a in _context.Academic
                                    from b in _context.NAAC_MC_717_DisabledFriendlyEnvironmentDMO
                                    where (a.MI_Id == b.MI_Id && a.ASMAY_Id == b.NCMC717DFE_Year && yearid.Contains(b.NCMC717DFE_Year.ToString()) && a.Is_Active == true && mi_ids.Contains(a.MI_Id) && b.NCMC717DFE_ActiveFlg == true)
                                    select new NAACAC7Report_DTO
                                    {
                                        NCMC717DFE_Id = b.NCMC717DFE_Id,
                                        NCMC717DFE_BuiltEnvwithRampsORLiftsFlag = b.NCMC717DFE_BuiltEnvwithRampsORLiftsFlag,
                                        NCMC717DFE_Year = b.NCMC717DFE_Year,
                                        NCMC717DFE_DisabledFriendlyWashroomsFlag = b.NCMC717DFE_DisabledFriendlyWashroomsFlag,
                                        ASMAY_Year = a.ASMAY_Year,
                                        NCMC717DFE_SignageIncTactilePathssignpostsFlag = b.NCMC717DFE_SignageIncTactilePathssignpostsFlag,
                                        NCMC717DFE_AssistiveTechnologyFacfacMEFlag = b.NCMC717DFE_AssistiveTechnologyFacfacMEFlag,
                                        NCMC717DFE_ProvisionForEnquiryScreenReadingFlag = b.NCMC717DFE_ProvisionForEnquiryScreenReadingFlag
                                    }).Distinct().OrderByDescending(t => t.NCAC7117UNIVAL_Id).ToArray();
                }
                else if (data.Type == "719DifferentlyAbledMC")
                {
                    data.alldata = (from a in _context.Academic
                                    from b in _context.NAAC_AC_719_DifferentlyAbledDMO
                                    where (a.MI_Id == b.MI_Id && a.ASMAY_Id == b.NCAC719DIFFAB_Year && yearid.Contains(b.NCAC719DIFFAB_Year.ToString()) && a.Is_Active == true && mi_ids.Contains(a.MI_Id) && b.NCAC719DIFFAB_ActiveFlg == true && b.NCAC719DIFFAB_CodeOfConductDisplayedWebsiteFlag != null && b.NCAC719DIFFAB_CommitteeMonitorAdherenceCodeConductFlag != null && b.NCAC719DIFFAB_ProfProgOrgStuStaffFlag != null && b.NCAC719DIFFAB_AnnualAwsProgConductOrganizedFlag != null)
                                    select new NAACAC7Report_DTO
                                    {
                                        NCAC719DIFFAB_Id = b.NCAC719DIFFAB_Id,
                                        NCAC719DIFFAB_CodeOfConductDisplayedWebsiteFlag = b.NCAC719DIFFAB_CodeOfConductDisplayedWebsiteFlag,
                                        NCAC719DIFFAB_Year = b.NCAC719DIFFAB_Year,
                                        NCAC719DIFFAB_CommitteeMonitorAdherenceCodeConductFlag = b.NCAC719DIFFAB_CommitteeMonitorAdherenceCodeConductFlag,
                                        ASMAY_Year = a.ASMAY_Year,
                                        NCAC719DIFFAB_ProfProgOrgStuStaffFlag = b.NCAC719DIFFAB_ProfProgOrgStuStaffFlag,
                                        NCAC719DIFFAB_AnnualAwsProgConductOrganizedFlag = b.NCAC719DIFFAB_AnnualAwsProgConductOrganizedFlag,
                                    }).Distinct().OrderByDescending(t => t.NCAC719DIFFAB_Id).ToArray();
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
