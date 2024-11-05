using DataAccessMsSqlServerProvider.com.vapstech.VidyaBharathi;
using DomainModel.Model.com.vapstech.VidyaBharathi;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.VidyaBharathi;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace VidyaBharathiServiceHub.com.vaps.Services
{
    public class VBadminInterIMPL : Interfaces.VBadminInterface
    {
        //VidyaBharathiContext
        public VidyaBharathiContext  _VidyaBharathiContext;
        public VBadminInterIMPL(VidyaBharathiContext VidyaBharathiContext)
        {
            _VidyaBharathiContext = VidyaBharathiContext;
        }
        public VBadminDTO LoadData(VBadminDTO data)
        { 
            try
            {
                using (var cmd = _VidyaBharathiContext.Database.GetDbConnection().CreateCommand())                {                    cmd.CommandText = "User_State_Getreport";                    cmd.CommandType = CommandType.StoredProcedure;                    if (cmd.Connection.State != ConnectionState.Open)                        cmd.Connection.Open();                    var retObject = new List<dynamic>();                    try                    {                        using (var dataReader = cmd.ExecuteReader())                        {                            while (dataReader.Read())                            {                                var dataRow = new ExpandoObject() as IDictionary<string, object>;                                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)                                {                                    dataRow.Add(                                        dataReader.GetName(iFiled),                                        dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}                                    );                                }                                retObject.Add((ExpandoObject)dataRow);                            }                        }                        data.getsateusers = retObject.ToArray();                    }                    catch (Exception ex)                    {                        Console.WriteLine(ex.Message);                    }                }

                using (var cmd = _VidyaBharathiContext.Database.GetDbConnection().CreateCommand())                {                    cmd.CommandText = "User_District_Getreport";                    cmd.CommandType = CommandType.StoredProcedure;                    if (cmd.Connection.State != ConnectionState.Open)                        cmd.Connection.Open();                    var retObject = new List<dynamic>();                    try                    {                        using (var dataReader = cmd.ExecuteReader())                        {                            while (dataReader.Read())                            {                                var dataRow = new ExpandoObject() as IDictionary<string, object>;                                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)                                {                                    dataRow.Add(                                        dataReader.GetName(iFiled),                                        dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}                                    );                                }                                retObject.Add((ExpandoObject)dataRow);                            }                        }                        data.getdistrictusers = retObject.ToArray();                    }                    catch (Exception ex)                    {                        Console.WriteLine(ex.Message);                    }                }


                using (var cmd = _VidyaBharathiContext.Database.GetDbConnection().CreateCommand())                {                    cmd.CommandText = "User_Graphs_Getreport";                    cmd.CommandType = CommandType.StoredProcedure;                    if (cmd.Connection.State != ConnectionState.Open)                        cmd.Connection.Open();                    var retObject = new List<dynamic>();                    try                    {                        using (var dataReader = cmd.ExecuteReader())                        {                            while (dataReader.Read())                            {                                var dataRow = new ExpandoObject() as IDictionary<string, object>;                                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)                                {                                    dataRow.Add(                                        dataReader.GetName(iFiled),                                        dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}                                    );                                }                                retObject.Add((ExpandoObject)dataRow);                            }                        }                        data.getgraphsreport = retObject.ToArray();                    }                    catch (Exception ex)                    {                        Console.WriteLine(ex.Message);                    }                }


            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;

        }


        public VBadminDTO ViewCOEDetails(VBadminDTO data)
        {
            try
            {
             
                data.getCOEEventDetails = (from a in _VidyaBharathiContext.VBSC_Master_EventsDMO
                                           from b in _VidyaBharathiContext.VBSC_Master_Competition_CategoryDMO
                                           from c in _VidyaBharathiContext.VBSC_Master_SportsCCNameDMO
                                           from d in _VidyaBharathiContext.VBSC_Events_CategoryDMO
                                           where (d.VBSCMCC_Id == b.VBSCMCC_Id && d.VBSCMSCC_Id == c.VBSCMSCC_Id && d.VBSCME_Id == a.VBSCME_Id)
                                           select new VBSC_Events_CategoryDTO
                                           {
                                               VBSCME_Id = a.VBSCME_Id,
                                               VBSCMCC_Id = b.VBSCMCC_Id,
                                               VBSCMSCC_Id = c.VBSCMSCC_Id,
                                               VBSCME_EventName = a.VBSCME_EventName,
                                               VBSCMCC_CompetitionCategory = b.VBSCMCC_CompetitionCategory,
                                               VBSCMSCC_SportsCCName = c.VBSCMSCC_SportsCCName,
                                               VBSCECT_ActiveFlag = d.VBSCECT_ActiveFlag,
                                               VBSCECT_Id = d.VBSCECT_Id,
                                               VBSCECT_GroupActivityFlg = d.VBSCECT_GroupActivityFlg,
                                               VBSCECT_MaxNoOfGroup = d.VBSCECT_MaxNoOfGroup,
                                               VBSCECT_MaxNoOfStudents = d.VBSCECT_MaxNoOfStudents,

                                           }).Distinct().OrderByDescending(m => m.VBSCME_Id).ToArray();

            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;

        }
    }
}
