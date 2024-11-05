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
    public class VBSC_Events_CategoryIMPL : Interfaces.VBSC_Events_CategoryInterface
    {
        //VidyaBharathiContext
        public VidyaBharathiContext  _VidyaBharathiContext;
        public VBSC_Events_CategoryIMPL(VidyaBharathiContext VidyaBharathiContext)
        {
            _VidyaBharathiContext = VidyaBharathiContext;
        }
        public VBSC_Events_CategoryDTO loaddata(VBSC_Events_CategoryDTO data)
        { 
            try
            {
                data.getMasterEvents = _VidyaBharathiContext.VBSC_Master_EventsDMO.Where(R => R.VBSCME_ActiveFlag == true).Distinct().ToArray();

                data.getcompitionCategory = _VidyaBharathiContext.VBSC_Master_Competition_CategoryDMO.Where(R => R.VBSCMCC_ActiveFlag == true).Distinct().ToArray();

                data.getSportsCCName = _VidyaBharathiContext.VBSC_Master_SportsCCNameDMO.Where(R => R.VBSCMSCC_ActiveFlag == true).Distinct().ToArray();

                data.geteventcategory = (from a in _VidyaBharathiContext.VBSC_Master_EventsDMO
                                         from b in _VidyaBharathiContext.VBSC_Master_Competition_CategoryDMO
                                         from c in _VidyaBharathiContext.VBSC_Master_SportsCCNameDMO
                                         from d in _VidyaBharathiContext.VBSC_Events_CategoryDMO
                                         where (d.VBSCMCC_Id==b.VBSCMCC_Id && d.VBSCMSCC_Id==c.VBSCMSCC_Id && d.VBSCME_Id==a.VBSCME_Id)
                                         select new VBSC_Events_CategoryDTO
                                         {
                                             VBSCME_Id=a.VBSCME_Id,
                                             VBSCMCC_Id = b.VBSCMCC_Id,
                                             VBSCMSCC_Id = c.VBSCMSCC_Id,
                                             VBSCME_EventName = a.VBSCME_EventName,
                                             VBSCMCC_CompetitionCategory = b.VBSCMCC_CompetitionCategory,
                                             VBSCMSCC_SportsCCName = c.VBSCMSCC_SportsCCName,
                                             VBSCECT_ActiveFlag = d.VBSCECT_ActiveFlag,
                                             VBSCECT_Id = d.VBSCECT_Id,
                                             VBSCECT_GroupActivityFlg=d.VBSCECT_GroupActivityFlg,
                                             VBSCECT_MaxNoOfGroup=d.VBSCECT_MaxNoOfGroup,
                                             VBSCECT_MaxNoOfStudents=d.VBSCECT_MaxNoOfStudents,

                                         }).Distinct().OrderByDescending(m => m.VBSCME_Id).ToArray();

                

            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;

        }


        public VBSC_Events_CategoryDTO savedata(VBSC_Events_CategoryDTO data)
        {
            try
            {
                if (data.VBSCECT_Id != 0)
                {
                    var res = _VidyaBharathiContext.VBSC_Events_CategoryDMO.Where(t => t.VBSCECT_Id != data.VBSCECT_Id && t.VBSCMSCC_Id == data.VBSCMSCC_Id && t.VBSCME_Id == data.VBSCME_Id && t.VBSCECT_GroupActivityFlg == data.VBSCECT_GroupActivityFlg && t.VBSCECT_MaxNoOfGroup == data.VBSCECT_MaxNoOfGroup).ToList();
                    if (res.Count > 0)
                    {
                        data.returnduplicatestatus = "Duplicate";
                    }
                    else
                    {
                        var result = _VidyaBharathiContext.VBSC_Events_CategoryDMO.Single(t => t.VBSCECT_Id == data.VBSCECT_Id);
                      
                        result.VBSCMCC_Id = data.VBSCMCC_Id;
                        result.VBSCMSCC_Id = data.VBSCMSCC_Id;
                        result.VBSCME_Id = data.VBSCME_Id;
                        result.VBSCECT_GroupActivityFlg =data.VBSCECT_GroupActivityFlg;
                        result.VBSCECT_MaxNoOfGroup = data.VBSCECT_MaxNoOfGroup;
                        result.VBSCECT_MaxNoOfStudents = data.VBSCECT_MaxNoOfStudents;
                        result.VBSCECT_Remarks = data.VBSCECT_Remarks;
                        result.VBSCECT_ActiveFlag = true;
                        result.VBSCECT_UpdatedBy = data.User_Id;
                        result.VBSCECT_UpdatedDate = DateTime.Now;
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
                    var res = _VidyaBharathiContext.VBSC_Events_CategoryDMO.Where(t => t.VBSCMCC_Id == data.VBSCMCC_Id && t.VBSCMSCC_Id == data.VBSCMSCC_Id && t.VBSCME_Id == data.VBSCME_Id && t.VBSCECT_GroupActivityFlg==data.VBSCECT_GroupActivityFlg && t.VBSCECT_MaxNoOfGroup==data.VBSCECT_MaxNoOfGroup).ToList();

                    if (res.Count > 0)
                    {
                        data.returnduplicatestatus = "Duplicate";
                    }
                    else
                    {
                        VBSC_Events_CategoryDMO tax = new VBSC_Events_CategoryDMO();
                        tax.VBSCMCC_Id = data.VBSCMCC_Id;
                        tax.VBSCMSCC_Id = data.VBSCMSCC_Id;
                        tax.VBSCME_Id = data.VBSCME_Id;
                        tax.VBSCECT_GroupActivityFlg = data.VBSCECT_GroupActivityFlg;
                        tax.VBSCECT_MaxNoOfGroup = data.VBSCECT_MaxNoOfGroup;
                        tax.VBSCECT_MaxNoOfStudents = data.VBSCECT_MaxNoOfStudents;
                        tax.VBSCECT_Remarks = data.VBSCECT_Remarks;
                        tax.VBSCECT_ActiveFlag = true;
                        tax.VBSCECT_UpdatedBy = data.User_Id;
                        tax.VBSCECT_CreatedBy = data.User_Id;
                        tax.VBSCECT_CreatedDate = DateTime.Now;
                        tax.VBSCECT_UpdatedDate = DateTime.Now;
                        _VidyaBharathiContext.Add(tax);

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
            }
            catch (Exception ex)
            {
                data.message = "Error";
           
            }
            return data;
        }


        public VBSC_Events_CategoryDTO deactive(VBSC_Events_CategoryDTO data)
        {
            try
            {
                var result = _VidyaBharathiContext.VBSC_Events_CategoryDMO.Single(t => t.VBSCECT_Id == data.VBSCECT_Id);

                if (result.VBSCECT_ActiveFlag == true)
                {
                    result.VBSCECT_ActiveFlag = false;
                }
                else if (result.VBSCECT_ActiveFlag == false)
                {
                    result.VBSCECT_ActiveFlag = true;
                }
                result.VBSCECT_UpdatedDate = DateTime.Now;
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
