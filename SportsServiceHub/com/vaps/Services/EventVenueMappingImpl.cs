using AutoMapper;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.Sports;
using DomainModel.Model.com.vapstech.Sports;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.Sports;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsServiceHub.com.vaps.Services
{
    public class EventVenueMappingImpl : Interfaces.EventVenueMappingInterface
    {
        DomainModelMsSqlServerContext _db;
        SportsContext _context;

        public EventVenueMappingImpl(DomainModelMsSqlServerContext db, SportsContext context)
        {
            _db = db;
            _context = context;
        }
        public EventsMappingDTO getDetails(EventsMappingDTO data)
        {
            try
            {
                data.academicYear = _db.AcademicYear.Where(d => d.MI_Id == data.MI_Id && d.Is_Active == true).Distinct().OrderByDescending(t => t.ASMAY_Order).ToArray();
                data.eventsList = _context.MasterEventsDMO.Where(d => d.MI_Id == data.MI_Id && d.SPCCME_ActiveFlag == true).Distinct().OrderBy(t => t.SPCCME_Id).ToArray();
                data.sponsorList = _context.MasterSponserDMO.Where(d => d.MI_Id == data.MI_Id && d.SPCCMSP_ActiveFlag == true).Distinct().OrderBy(t => t.SPCCMSP_Id).ToArray();
                data.venuelist = _context.MasterEventVenueDMO.Where(t => t.MI_Id == data.MI_Id && t.SPCCMEV_ActiveFlag == true).Distinct().OrderBy(t => t.SPCCMEV_Id).ToArray();

                data.eventmappingList = (from a in _context.EventsMappingDMO
                                         from b in _context.MasterEventsDMO
                                         from c in _context.MasterEventVenueDMO
                                         from y in _context.AcademicYear
                                         where (a.SPCCME_Id == b.SPCCME_Id && a.SPCCMEV_Id == c.SPCCMEV_Id && a.MI_Id == b.MI_Id && a.ASMAY_Id == y.ASMAY_Id && a.MI_Id == data.MI_Id)
                                         select new EventsMappingDTO
                                         {
                                             SPCCE_Id = a.SPCCE_Id,
                                             ASMAY_Id = a.ASMAY_Id,
                                             SPCCE_StartDate = a.SPCCE_StartDate,
                                             SPCCE_EndDate = a.SPCCE_EndDate,
                                             SPCCE_StartTime = a.SPCCE_StartTime,
                                             SPCCE_EndTime = a.SPCCE_EndTime,
                                             SPCCE_Remarks = a.SPCCE_Remarks,
                                             SPCCE_ActiveFlag = a.SPCCE_ActiveFlag,

                                             SPCCME_Id = b.SPCCME_Id,
                                             SPCCME_EventName = b.SPCCME_EventName,

                                             SPCCMEV_Id = c.SPCCMEV_Id,
                                             SPCCMEV_EventVenue = c.SPCCMEV_EventVenue,
                                             SPCCE_SponsorFlag = a.SPCCE_SponsorFlag,
                                             ASMAY_Year = y.ASMAY_Year,
                                         }).ToArray();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }

        public EventsMappingDTO saveRecord(EventsMappingDTO obj)
        {
            try
            {
                if (obj.SPCCE_Id == 0)
                {
                    var checkduplicate = _context.EventsMappingDMO.Where(d => d.MI_Id == obj.MI_Id && d.ASMAY_Id == obj.ASMAY_Id && d.SPCCME_Id == obj.SPCCME_Id && d.SPCCMEV_Id == obj.SPCCMEV_Id && d.SPCCE_StartDate.Value.Date == obj.SPCCE_StartDate.Value.Date && d.SPCCE_EndDate.Value.Date == obj.SPCCE_EndDate.Value.Date).ToList();
                    if (checkduplicate.Count() > 0)
                    {
                        obj.returnVal = "duplicate";
                    }
                    else
                    {
                        //var mapp = Mapper.Map<EventsMappingDMO>(obj);
                        //mapp.SPCCE_ActiveFlag = true;
                        //mapp.CreatedDate = DateTime.Now;
                        //mapp.UpdatedDate = DateTime.Now;
                        //_context.Add(mapp);

                        EventsMappingDMO obj1 = new EventsMappingDMO();
                        //obj1.SPCCE_Id = obj.SPCCME_Id;
                        obj1.MI_Id = obj.MI_Id;
                        obj1.ASMAY_Id = obj.ASMAY_Id;
                        obj1.SPCCME_Id = obj.SPCCME_Id;
                        obj1.SPCCMEV_Id = obj.SPCCMEV_Id;
                        obj1.SPCCE_StartDate = obj.SPCCE_StartDate;
                        obj1.SPCCE_StartTime = obj.SPCCE_StartTime;
                        obj1.SPCCE_EndDate = obj.SPCCE_EndDate;
                        obj1.SPCCE_EndTime = obj.SPCCE_EndTime;
                        obj1.SPCCE_Remarks = obj.SPCCE_Remarks;
                        obj1.SPCCE_SponsorFlag = obj.SPCCE_SponsorFlag;
                        obj1.SPCCE_ActiveFlag = true;
                        obj1.CreatedDate = DateTime.Now;
                        obj1.UpdatedDate = DateTime.Now;

                        _context.Add(obj1);

                        foreach (var ss in obj.sponsordata)
                        {
                            EventsSponsorDMO obj2 = new EventsSponsorDMO();
                            //obj2.SPCCESP_Id = obj.SPCCESP_Id;
                            obj2.SPCCE_Id = obj1.SPCCE_Id;
                            obj2.MI_Id = obj.MI_Id;
                            obj2.SPCCMSP_Id = ss.SPCCMSP_Id;
                            obj2.SPCCESP_ActiveFlag = true;
                            obj2.CreatedDate = DateTime.Now;
                            obj2.UpdatedDate = DateTime.Now;

                            _context.Add(obj2);
                        }
                        int rowAffected = _context.SaveChanges();
                        if (rowAffected > 0)
                        {
                            obj.returnVal = "saved";
                        }
                        else
                        {
                            obj.returnVal = "savingFailed";
                        }
                    }

                }
                else if (obj.SPCCE_Id > 0)
                {
                    var checkduplicate = _context.EventsMappingDMO.Where(d => d.MI_Id == obj.MI_Id && d.ASMAY_Id == obj.ASMAY_Id && d.SPCCME_Id == obj.SPCCME_Id && d.SPCCMEV_Id == obj.SPCCMEV_Id && d.SPCCE_StartDate.Value.Date == obj.SPCCE_StartDate.Value.Date && d.SPCCE_EndDate.Value.Date == obj.SPCCE_EndDate.Value.Date && d.SPCCE_Id != obj.SPCCE_Id).ToList();
                    if (checkduplicate.Count > 0)
                    {
                        obj.returnVal = "duplicate";
                    }
                    else
                    {

                        var update = _context.EventsMappingDMO.Single(d => d.SPCCE_Id == obj.SPCCE_Id & d.MI_Id == obj.MI_Id && d.ASMAY_Id == obj.ASMAY_Id);

                        update.ASMAY_Id = obj.ASMAY_Id;
                        update.SPCCME_Id = obj.SPCCME_Id;
                        update.SPCCMEV_Id = obj.SPCCMEV_Id;
                        update.SPCCE_StartDate = obj.SPCCE_StartDate;
                        update.SPCCE_StartTime = obj.SPCCE_StartTime;
                        update.SPCCE_EndDate = obj.SPCCE_EndDate;
                        update.SPCCE_EndTime = obj.SPCCE_EndTime;
                        update.SPCCE_Remarks = obj.SPCCE_Remarks;
                        update.SPCCE_SponsorFlag = obj.SPCCE_SponsorFlag;
                        update.UpdatedDate = DateTime.Now;

                        _context.Add(update);

                        var resultclass = _context.EventsSponsorDMO.Where(d => d.SPCCE_Id == obj.SPCCE_Id & d.MI_Id == obj.MI_Id);
                        if (resultclass.Count() > 0)
                        {
                            foreach (var item in resultclass)
                            {
                                _context.Remove(item);
                            }
                        }
                        foreach (var ss in obj.sponsordata)
                        {
                            EventsSponsorDMO obj2 = new EventsSponsorDMO();
                            //obj2.SPCCESP_Id = obj.SPCCESP_Id;
                            obj2.SPCCE_Id = update.SPCCE_Id;
                            obj2.MI_Id = obj.MI_Id;
                            obj2.SPCCMSP_Id = ss.SPCCMSP_Id;
                            obj2.SPCCESP_ActiveFlag = true;
                            obj2.CreatedDate = DateTime.Now;
                            obj2.UpdatedDate = DateTime.Now;

                            _context.Add(obj2);
                        }

                        _context.Update(update);
                        int s = _context.SaveChanges();
                        if (s > 0)
                        {
                            obj.returnVal = "updated";
                        }
                        else
                        {
                            obj.returnVal = "updateFailed";
                        }

                    }
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return obj;
        }

        public async Task<EventsMappingDTO> EditDetails(EventsMappingDTO data)
        {
            //EventsMappingDTO resp = new EventsMappingDTO();
            try
            {
                //resp.editDetails = _context.EventsMappingDMO.Where(d => d.SPCCE_Id == id).ToArray();
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "SPC_EditEventMappingSponsor";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                     SqlDbType.BigInt)
                    {
                        Value = data.ASMAY_Id,

                    });
                    cmd.Parameters.Add(new SqlParameter("@SPCCE_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.SPCCE_Id
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
                        data.editDetails = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }


            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }

        public EventsMappingDTO deactivate(EventsMappingDTO obj)
        {
            try
            {
                var query = _context.EventsMappingDMO.Where(t => t.MI_Id == obj.MI_Id && t.ASMAY_Id == obj.ASMAY_Id && t.SPCCE_Id == obj.SPCCE_Id).SingleOrDefault();

                if (query.SPCCE_ActiveFlag == true)
                {
                    query.SPCCE_ActiveFlag = false;
                }
                else
                {
                    query.SPCCE_ActiveFlag = true;
                }
                query.UpdatedDate = DateTime.Now;
                _context.Update(query);
                var contactExists = _context.SaveChanges();
                if (contactExists > 0)
                {
                    obj.returnval = true;
                }
                else
                {
                    obj.returnval = false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return obj;
        }

        public EventsMappingDTO get_modeldata(EventsMappingDTO dto)
        {
            try
            {

                dto.modalsponsorlist = (from m in _context.EventsMappingDMO
                                        from o in _context.year
                                        from s in _context.MasterSponserDMO
                                        from sv in _context.EventsSponsorDMO
                                        where (m.SPCCE_Id == sv.SPCCE_Id && sv.SPCCMSP_Id == s.SPCCMSP_Id && m.MI_Id == sv.MI_Id && o.ASMAY_Id == m.ASMAY_Id && m.MI_Id == dto.MI_Id && m.ASMAY_Id == dto.ASMAY_Id && m.SPCCE_Id == dto.SPCCE_Id)
                                        select new EventsMappingDTO
                                        {
                                            SPCCESP_Id = sv.SPCCESP_Id,
                                            SPCCMSP_Id = s.SPCCMSP_Id,
                                            SPCCMSP_SponsorName = s.SPCCMSP_SponsorName,
                                            SPCCMSP_ContactPerson = s.SPCCMSP_ContactPerson,
                                            SPCCMSP_ContactNo = s.SPCCMSP_ContactNo,
                                            SPCCMSP_SponsorDetails = s.SPCCMSP_SponsorDetails,
                                            SPCCESP_ActiveFlag = sv.SPCCESP_ActiveFlag

                                        }).Distinct().OrderBy(t => t.SPCCMSP_Id).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return dto;
        }

        public EventsMappingDTO Deactivesponsor(EventsMappingDTO obj)
        {
            try
            {
                var query = _context.EventsSponsorDMO.Where(t => t.MI_Id == obj.MI_Id && t.SPCCESP_Id == obj.SPCCESP_Id).SingleOrDefault();

                if (query.SPCCESP_ActiveFlag == true)
                {
                    query.SPCCESP_ActiveFlag = false;
                }
                else
                {
                    query.SPCCESP_ActiveFlag = true;
                }
                query.UpdatedDate = DateTime.Now;
                _context.Update(query);
                var contactExists = _context.SaveChanges();
                if (contactExists > 0)
                {
                    obj.returnval = true;
                }
                else
                {
                    obj.returnval = false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return obj;
        }

    }
}
