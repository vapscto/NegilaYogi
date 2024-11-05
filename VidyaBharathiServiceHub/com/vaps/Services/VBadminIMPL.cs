﻿using DataAccessMsSqlServerProvider.com.vapstech.VidyaBharathi;
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
                using (var cmd = _VidyaBharathiContext.Database.GetDbConnection().CreateCommand())

                using (var cmd = _VidyaBharathiContext.Database.GetDbConnection().CreateCommand())


                using (var cmd = _VidyaBharathiContext.Database.GetDbConnection().CreateCommand())


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