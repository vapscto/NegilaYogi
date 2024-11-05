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
    public class NaacDocumentUploadReportImpl:Interface.NaacDocumentUploadReportInterface
    {

        public DocumentsContext _context;
        public NaacDocumentUploadReportImpl(DocumentsContext _conte)
        {
            _context = _conte;
        }

        public async Task<NaacDocumentUploadReport_DTO> loaddata(NaacDocumentUploadReport_DTO data)
        {
            try
            {
                var getinstitution = _context.Institution.Where(a => a.MI_Id == data.MI_Id).ToList();

                string NAACSL_InstitutionTypeFlg = "";

                if (getinstitution.Count() > 0)
                {
                    NAACSL_InstitutionTypeFlg = getinstitution.FirstOrDefault().MI_NAAC_InstitutionTypeFlg;

                    if (NAACSL_InstitutionTypeFlg.ToUpper() == "DEEMED")
                    {
                        NAACSL_InstitutionTypeFlg = getinstitution.FirstOrDefault().MI_NAAC_SubInstitutionTypeFlg.ToUpper();
                    }
                }

                data.getparentidzero = _context.NaacDocumentUploadDMO.Where(a => a.NAACSL_ActiveFlag == true && a.NAACSL_ParentId == 0
                && a.NAACSL_InstitutionTypeFlg.ToUpper() == NAACSL_InstitutionTypeFlg).OrderBy(a => a.NAACSL_SLNoOrder).ToArray();

                data.getalldata = _context.NaacDocumentUploadDMO.Where(a => a.NAACSL_ActiveFlag == true
                && a.NAACSL_InstitutionTypeFlg.ToUpper() == NAACSL_InstitutionTypeFlg).OrderBy(a => a.NAACSL_SLNoOrder).ToArray();

                data.getsavealldata = (from a in _context.NaacDocumentUploadDetailsDMO
                                       from b in _context.NaacDocumentUploadDMO
                                       where (a.NAACSL_Id == b.NAACSL_Id && a.MI_Id == data.MI_Id && a.NAACMSL_ActiveFlag == true
                                       && b.NAACSL_InstitutionTypeFlg.ToUpper() == NAACSL_InstitutionTypeFlg)
                                       select new NaacDocumentUploadReport_DTO {
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

                var getapprovedlist = (from a in _context.NaacDocumentUploadDetailsDMO
                                       from b in _context.NaacDocumentUploadDMO
                                       where (a.NAACSL_Id == b.NAACSL_Id && a.MI_Id == data.MI_Id && a.NAACMSL_ActiveFlag == true && a.NAACMSL_Status == "Approved"
                                       && b.NAACSL_InstitutionTypeFlg.ToUpper() == NAACSL_InstitutionTypeFlg)
                                       select new NaacDocumentUploadReport_DTO
                                       {
                                           NAACSL_Id = a.NAACSL_Id,
                                           NAACSL_Percentage = b.NAACSL_Percentage
                                       }).Distinct().ToList();

                decimal percentage = 0;

                if (getapprovedlist.Count > 0)
                {
                    foreach (var c in getapprovedlist)
                    {
                        percentage += Convert.ToDecimal(c.NAACSL_Percentage);
                    }

                    data.percentage = percentage;
                }
                else
                {
                    data.percentage = 0;
                }

                var getmaxmarkslist = (from b in _context.NaacDocumentUploadDMO
                                       where (b.NAACSL_ActiveFlag == true && b.NAACSL_InstitutionTypeFlg.ToUpper() == NAACSL_InstitutionTypeFlg)
                                       select new NaacDocumentUploadDTO
                                       {
                                           NAACSL_Percentage = b.NAACSL_Percentage
                                       }).Distinct().ToList();

                decimal maxpercentage = 0;

                if (getmaxmarkslist.Count > 0)
                {
                    foreach (var c in getmaxmarkslist)
                    {
                        maxpercentage += Convert.ToDecimal(c.NAACSL_Percentage);
                    }
                }
                else
                {
                    maxpercentage = 0;
                }
                if (percentage > 0 && maxpercentage > 0)
                {
                    var totalpercentage = (percentage / maxpercentage) * 100;
                    data.percentage = Convert.ToDecimal(String.Format("{0:0.00}", totalpercentage));
                }
                else
                {
                    data.percentage = 0;
                }


                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "NAAC_Document_Upload_Report";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
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
                        data.reportlist = retObject.ToArray();

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
