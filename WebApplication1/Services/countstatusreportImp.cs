using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs;
using DataAccessMsSqlServerProvider;
using WebApplication1.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using DomainModel.Model;
using AutoMapper;
using System.Collections.Concurrent;
using CommonLibrary;

namespace WebApplication1.Services
{
    public class countstatusreportImpl : Interfaces.countstatusreportInterface
    {

        private static ConcurrentDictionary<string, StudentApplicationDTO> _login =
              new ConcurrentDictionary<string, StudentApplicationDTO>();

        public DomainModelMsSqlServerContext _db;

        public countstatusreportImpl(DomainModelMsSqlServerContext db)
        {
            _db = db;
        }

        // get initial dropdown data
        public async Task<CommonDTO> getInitailData(int mi_id)
        {
            CommonDTO ctdo = new CommonDTO();
            try
            {
                List<MasterAcademic> allyear = new List<MasterAcademic>();
                allyear = await _db.AcademicYear.Where(y=>y.MI_Id==mi_id && y.Is_Active == true).OrderByDescending(d => d.ASMAY_Order).ToListAsync();
                ctdo.AcademicList = allyear.ToArray();

                //List<School_M_Class> allclass = new List<School_M_Class>();
                //allclass = await _db.School_M_Class.ToListAsync();
                //ctdo.classlist = allclass.ToArray();

                List<AdmissionStatus> status = new List<AdmissionStatus>();
                status = await _db.status.ToListAsync();
                ctdo.statuslist = status.ToArray();

                //ctdo.applicationstatus = (from a in _db.AdmissionStatus
                //                          from b in _db.StudentApplication
                //                          where a.PAMST_Id == b.PAMS_Id
                //                          select new CommonDTO
                //                          {
                //                               PASRAPS_ID =b.PASRAPS_ID


                //                           }).GroupBy(t=> t.PASRAPS_ID, (key, values) => new { Id = key, Count = values.Count() }).ToArray();
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return ctdo;
        }



        // search student data based on class, year and status
      
    }
}
