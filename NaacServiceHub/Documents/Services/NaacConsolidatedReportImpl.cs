using CommonLibrary;
using DataAccessMsSqlServerProvider.NAAC;
using DataAccessMsSqlServerProvider.NAAC.Documents;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.NAAC.Documents;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.Documents.Services
{
    public class NaacConsolidatedReportImpl : Interface.NaacConsolidatedReportInterface
    {
        public DocumentsContext _DocumentsContext;
        public GeneralContext _GeneralContext;

        public NaacConsolidatedReportImpl(DocumentsContext DocumentsContext, GeneralContext praa)
        {
            _DocumentsContext = DocumentsContext;
            _GeneralContext = praa;

        }
        public NaacDocumentUploadReport_DTO getdata(NaacDocumentUploadReport_DTO data)
        {
            try
            {
                var getinstitution = _GeneralContext.Institution.Where(a => a.MI_Id == data.MI_Id).ToList();


                string NAACSL_InstitutionTypeFlg = "";
                List<long> miid = new List<long>();
                if (getinstitution.Count() > 0)
                {
                    NAACSL_InstitutionTypeFlg = getinstitution.FirstOrDefault().MI_NAAC_InstitutionTypeFlg;
                }

                NAAC_CommonDetails naaccomm = new NAAC_CommonDetails(_GeneralContext);

                data.getinstitutioncycle = naaccomm.get_cycle_list(data.MI_Id, data.UserId);
                data.getinstitution = naaccomm.get_Institution_list(data.MI_Id, data.UserId);

                if (getinstitution.Count() > 0)
                {
                    NAACSL_InstitutionTypeFlg = getinstitution.FirstOrDefault().MI_NAAC_InstitutionTypeFlg;
                }              
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public async Task<NaacDocumentUploadReport_DTO> get_report(NaacDocumentUploadReport_DTO data)
        {
            try
            {
                var getinstitution = _DocumentsContext.Institution.Where(a => a.MI_Id == data.MI_Id).ToList();

                string NAACSL_InstitutionTypeFlg = "";

                if (getinstitution.Count() > 0)
                {
                    NAACSL_InstitutionTypeFlg = getinstitution.FirstOrDefault().MI_NAAC_InstitutionTypeFlg;

                    if (NAACSL_InstitutionTypeFlg.ToUpper() == "Affiliated College")
                    {
                        NAACSL_InstitutionTypeFlg = getinstitution.FirstOrDefault().MI_NAAC_SubInstitutionTypeFlg.ToUpper();
                    }
                }

                data.getparentidzero = _DocumentsContext.NaacDocumentUploadDMO.Where(a => a.NAACSL_ActiveFlag == true && a.NAACSL_ParentId == 0 && a.NAACSL_InstitutionTypeFlg.ToUpper() == NAACSL_InstitutionTypeFlg).OrderBy(a => a.NAACSL_SLNoOrder).ToArray();

                //data.getparentidone = _DocumentsContext.NaacDocumentUploadDMO.Where(a => a.NAACSL_ActiveFlag == true && a.NAACSL_ParentId !=0 && a.NAACSL_InstitutionTypeFlg.ToUpper() == NAACSL_InstitutionTypeFlg).OrderBy(a => a.NAACSL_SLNoOrder).ToArray();

                data.getalldata = _DocumentsContext.NaacDocumentUploadDMO.Where(a => a.NAACSL_ActiveFlag == true
                && a.NAACSL_InstitutionTypeFlg.ToUpper() == NAACSL_InstitutionTypeFlg).OrderBy(a => a.NAACSL_SLNoOrder).ToArray();

                data.getsavealldata = (from a in _DocumentsContext.NaacDocumentUploadDetailsDMO
                                       from b in _DocumentsContext.NaacDocumentUploadDMO
                                       where (a.NAACSL_Id == b.NAACSL_Id && a.MI_Id == data.MI_Id && a.NAACMSL_ActiveFlag == true
                                       && b.NAACSL_InstitutionTypeFlg.ToUpper() == NAACSL_InstitutionTypeFlg)
                                       select new NaacDocumentUploadReport_DTO
                                       {
                                           NAACSL_Id = a.NAACSL_Id,
                                           NAACSL_Percentage = b.NAACSL_Percentage,
                                           NAACSL_SLNoDescription = b.NAACSL_SLNoDescription,
                                           NAACSL_ParentId = b.NAACSL_ParentId,
                                           NAACSL_SLNote = b.NAACSL_SLNote,
                                           NAACSL_TextBoxFlg = b.NAACSL_TextBoxFlg,
                                           NAACMSL_Status = a.NAACMSL_Status,
                                           NAACMSL_CGPA = a.NAACMSL_CGPA,
                                           NAACSL_SLNo = b.NAACSL_SLNo,
                                       }).Distinct().ToArray();

                using (var cmd = _DocumentsContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "NAAC_CriteriaStatus";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@NCMACY_Id",
             SqlDbType.BigInt)
                    {
                        Value = data.cycleid
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
                        data.get_Report = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }        
    }
}