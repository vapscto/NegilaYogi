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
    public class IVRM_User_Login_StateIMPL : Interfaces.IVRM_User_Login_StateInterface
    {
        //VidyaBharathiContext
        public VidyaBharathiContext  _VidyaBharathiContext;
        public IVRM_User_Login_StateIMPL(VidyaBharathiContext VidyaBharathiContext)
        {
            _VidyaBharathiContext = VidyaBharathiContext;
        }
        public IVRM_User_Login_StateDTO loaddata(IVRM_User_Login_StateDTO data)
        { 
            try
            {
                data.getusers = (from a in _VidyaBharathiContext.ApplicationUserDMO
                                from b in _VidyaBharathiContext.UserRoleWithInstituteDMO
                                where (a.Id == b.Id && b.MI_Id== data.MI_Id)
                                 select new IVRM_User_Login_StateDTO
                                 {
                                     Id = a.Id,
                                     IVRMULI_Id = b.IVRMULI_Id,
                                     NormalizedUserName=a.NormalizedUserName,
                                 }).Distinct().OrderByDescending(m => m.NormalizedUserName).ToArray();


                data.getcountry = _VidyaBharathiContext.Country.Distinct().OrderBy(a => a.IVRMMC_CountryName).ToArray();

                data.statelist = _VidyaBharathiContext.State.Where(R => R.IVRMMS_ActiveFlag == true).Distinct().ToArray();


                using (var cmd = _VidyaBharathiContext.Database.GetDbConnection().CreateCommand())                {                    cmd.CommandText = "User_State_Getreport";                    cmd.CommandType = CommandType.StoredProcedure;                    if (cmd.Connection.State != ConnectionState.Open)                        cmd.Connection.Open();                    var retObject = new List<dynamic>();                    try                    {                        using (var dataReader = cmd.ExecuteReader())                        {                            while (dataReader.Read())                            {                                var dataRow = new ExpandoObject() as IDictionary<string, object>;                                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)                                {                                    dataRow.Add(                                        dataReader.GetName(iFiled),                                        dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}                                    );                                }                                retObject.Add((ExpandoObject)dataRow);                            }                        }                        data.getusermap = retObject.ToArray();                    }                    catch (Exception ex)                    {                        Console.WriteLine(ex.Message);                    }                }

            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;

        }


        public IVRM_User_Login_StateDTO savedata(IVRM_User_Login_StateDTO data)
        {
            try
            {
                int saveCount = 0;
                int DuplicateCount = 0;

                if (data.IVRMULST_Id != 0)
                {
                    var res = _VidyaBharathiContext.IVRM_User_Login_StateDMO.Where(t => t.IVRMUL_Id == data.IVRMUL_Id &&
                    t.IVRMULST_Id != data.IVRMULST_Id).ToList();
                    if (res.Count > 0)
                    {
                        data.returnduplicatestatus = "Duplicate";
                    }
                    else
                    {
                        

                        var result = _VidyaBharathiContext.IVRM_User_Login_StateDMO.Single(t => t.IVRMULST_Id == data.IVRMULST_Id);
                        result.IVRMUL_Id = data.IVRMUL_Id;                      
                      
                        result.IVRMULST_UpdatedDate = DateTime.Now;
                        _VidyaBharathiContext.Update(result);

                        var contactExists = _VidyaBharathiContext.SaveChanges();
                        if (contactExists > 0)
                        {
                            data.returnval = true;
                        }
                        else
                        {
                            data.returnval = false;
                        }
                    }
                }
                else
                {
                   
                    
                    if (data.multiplstatelistone != null)
                    {
                        foreach (var ue in data.multiplstatelistone)
                        {
                            var res = _VidyaBharathiContext.IVRM_User_Login_StateDMO.Where(t => t.IVRMUL_Id == data.IVRMUL_Id && t.IVRMMS_Id==ue.IVRMMS_Id).ToList();

                            if (res.Count > 0)
                            {
                                DuplicateCount += 1;
                            }
                            else
                            {
                                IVRM_User_Login_StateDMO tax = new IVRM_User_Login_StateDMO();
                                tax.IVRMUL_Id = data.IVRMUL_Id;
                                tax.IVRMMS_Id = ue.IVRMMS_Id;
                                tax.IVRMULST_ActiveFlag = true;
                                tax.IVRMULST_CreatedDate = DateTime.Now;
                                tax.IVRMULST_UpdatedDate = DateTime.Now;
                                _VidyaBharathiContext.Add(tax);

                               
                            }
                           
                        }

                        int contactExists = _VidyaBharathiContext.SaveChanges();
                        if (contactExists > 0)
                        {
                            saveCount = contactExists;
                            data.returnval = true;
                        }
                        else
                        {
                            data.returnval = false;
                        }
                    }

                }

                data.savecount = saveCount;
                data.Duplicatecount = DuplicateCount;
            }
            catch (Exception ex)
            {
                data.message = "Error";
           
            }
            return data;
        }


        public IVRM_User_Login_StateDTO deactive(IVRM_User_Login_StateDTO data)
        {
            try
            {
                var result = _VidyaBharathiContext.IVRM_User_Login_StateDMO.Single(t => t.IVRMULST_Id == data.IVRMULST_Id);

                if (result.IVRMULST_ActiveFlag == true)
                {
                    result.IVRMULST_ActiveFlag = false;
                }
                else if (result.IVRMULST_ActiveFlag == false)
                {
                    result.IVRMULST_ActiveFlag = true;
                }
                result.IVRMULST_UpdatedDate = DateTime.Now;
                _VidyaBharathiContext.Update(result);
                int returnval = _VidyaBharathiContext.SaveChanges();
                if (returnval > 0)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return data;
        }


        public IVRM_User_Login_StateDTO edit(IVRM_User_Login_StateDTO data)
        {
            try
            {


                data.IVRMUL_Id = _VidyaBharathiContext.IVRM_User_Login_StateDMO.Where(a => a.IVRMULST_Id == data.IVRMULST_Id).Select(a => a.IVRMULST_Id).FirstOrDefault();


                var editData = (from a in _VidyaBharathiContext.IVRM_User_Login_StateDMO
                                from t in _VidyaBharathiContext.State
                                where (a.IVRMMS_Id==t.IVRMMS_Id && a.IVRMULST_Id==data.IVRMULST_Id)
                                select new IVRM_User_Login_StateDTO
                                {
                                    IVRMUL_Id = a.IVRMUL_Id,
                                    IVRMMS_Id = a.IVRMMS_Id,

                                    IVRMULST_ActiveFlag = a.IVRMULST_ActiveFlag,
                                }).ToList();
                data.editDetails = editData.ToArray();
                
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return data;
        }

    }
}
