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
    public class IVRM_User_Login_DistrictIMPL : Interfaces.IVRM_User_Login_DistrictInterface
    {
        //VidyaBharathiContext
        public VidyaBharathiContext  _VidyaBharathiContext;
        public IVRM_User_Login_DistrictIMPL(VidyaBharathiContext VidyaBharathiContext)
        {
            _VidyaBharathiContext = VidyaBharathiContext;
        }
        public IVRM_User_Login_DistrictDTO loaddata(IVRM_User_Login_DistrictDTO data)
        { 
            try
            {
                data.getusers = (from a in _VidyaBharathiContext.ApplicationUserDMO
                                from b in _VidyaBharathiContext.UserRoleWithInstituteDMO
                                where (a.Id == b.Id && b.MI_Id== data.MI_Id)
                                 select new IVRM_User_Login_DistrictDTO
                                 {
                                     Id = a.Id,
                                     IVRMULI_Id = b.IVRMULI_Id,
                                     NormalizedUserName=a.NormalizedUserName,
                                 }).Distinct().OrderByDescending(m => m.NormalizedUserName).ToArray();


                data.getcountry = _VidyaBharathiContext.Country.Distinct().OrderBy(a => a.IVRMMC_CountryName).ToArray();

                data.statelist = _VidyaBharathiContext.State.Where(R => R.IVRMMS_ActiveFlag == true).Distinct().ToArray();

                data.getdistrict = _VidyaBharathiContext.DistrictDMO.Where(R => R.IVRMMD_ActiveFlag == true).Distinct().ToArray();
            


                using (var cmd = _VidyaBharathiContext.Database.GetDbConnection().CreateCommand())                {                    cmd.CommandText = "User_District_Getreport";                    cmd.CommandType = CommandType.StoredProcedure;                    if (cmd.Connection.State != ConnectionState.Open)                        cmd.Connection.Open();                    var retObject = new List<dynamic>();                    try                    {                        using (var dataReader = cmd.ExecuteReader())                        {                            while (dataReader.Read())                            {                                var dataRow = new ExpandoObject() as IDictionary<string, object>;                                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)                                {                                    dataRow.Add(                                        dataReader.GetName(iFiled),                                        dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}                                    );                                }                                retObject.Add((ExpandoObject)dataRow);                            }                        }                        data.getusermap = retObject.ToArray();                    }                    catch (Exception ex)                    {                        Console.WriteLine(ex.Message);                    }                }

            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;

        }


        public IVRM_User_Login_DistrictDTO savedata(IVRM_User_Login_DistrictDTO data)
        {
            try
            {
                int saveCount = 0;
                int DuplicateCount = 0;

                if (data.IVRMULDT_Id != 0)
                {
                    var res = _VidyaBharathiContext.IVRM_User_Login_DistrictDMO.Where(t => t.IVRMUL_Id == data.IVRMUL_Id &&
                    t.IVRMULDT_Id != data.IVRMULDT_Id).ToList();
                    if (res.Count > 0)
                    {
                        data.returnduplicatestatus = "Duplicate";
                    }
                    else
                    {
                        var result = _VidyaBharathiContext.IVRM_User_Login_DistrictDMO.Single(t => t.IVRMULDT_Id == data.IVRMULDT_Id);
                        result.IVRMUL_Id = data.IVRMUL_Id;

                        result.IVRMULDT_UpdatedDate = DateTime.Now;
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
                            var res = _VidyaBharathiContext.IVRM_User_Login_DistrictDMO.Where(t => t.IVRMUL_Id == data.IVRMUL_Id && t.IVRMMD_Id == ue.IVRMMD_Id).ToList();

                            if (res.Count > 0)
                            {
                                DuplicateCount += 1;
                            }
                            else
                            {
                                IVRM_User_Login_DistrictDMO tax = new IVRM_User_Login_DistrictDMO();
                                tax.IVRMUL_Id = data.IVRMUL_Id;
                                tax.IVRMMD_Id = ue.IVRMMD_Id;
                                tax.IVRMULDT_ActiveFlag = true;
                                tax.IVRMULDT_CreatedDate = DateTime.Now;
                                tax.IVRMULDT_UpdatedDate = DateTime.Now;
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


        public IVRM_User_Login_DistrictDTO deactive(IVRM_User_Login_DistrictDTO data)
        {
            try
            {
                var result = _VidyaBharathiContext.IVRM_User_Login_DistrictDMO.Single(t => t.IVRMULDT_Id == data.IVRMULDT_Id);

                if (result.IVRMULDT_ActiveFlag == true)
                {
                    result.IVRMULDT_ActiveFlag = false;
                }
                else if (result.IVRMULDT_ActiveFlag == false)
                {
                    result.IVRMULDT_ActiveFlag = true;
                }
                result.IVRMULDT_UpdatedDate = DateTime.Now;
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
    }
}
