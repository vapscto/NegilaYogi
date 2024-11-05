using AutoMapper;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.Sports;
using DomainModel.Model.com.vapstech.Sports;
using PreadmissionDTOs.com.vaps.Sports;
using SportsServiceHub.com.vaps.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsServiceHub.com.vaps.Services
{
    public class ProgramMasterImpl : ProgramMasterInterface
    {
        DomainModelMsSqlServerContext _db;
        SportsContext _context;
        public ProgramMasterImpl(DomainModelMsSqlServerContext db, SportsContext context)
        {
            _db = db;
            _context = context;
        }

        public ProgramMasterDTO getDetails(ProgramMasterDTO data)
        {
            try
            {
                data.programList = _context.ProgramMasterDMO.Where(d => d.MI_Id == data.MI_Id).ToList().ToArray();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public ProgramMasterDTO saveRecord(ProgramMasterDTO obj)
        {
            try
            {
                if (obj.SPCCPM_Id > 0)
                {
                    var checkduplicate = _context.ProgramMasterDMO.Where(d => d.MI_Id == obj.MI_Id && d.SPCCPM_Name.Equals(obj.SPCCPM_Name) && d.SPCCPM_Id != obj.SPCCPM_Id).ToList();
                    if (checkduplicate.Count > 0)
                    {
                        obj.returnVal = "duplicate";
                    }
                    else
                    {
                        var query = _context.ProgramMasterDMO.Where(d => d.SPCCPM_Id == obj.SPCCPM_Id).ToList();
                        if (query.Count > 0)
                        {
                            var update = _context.ProgramMasterDMO.Single(d => d.SPCCPM_Id == obj.SPCCPM_Id);
                            update.UpdatedDate = DateTime.Now;
                            update.SPCCPM_Name = obj.SPCCPM_Name;
                            update.SPCCPM_Description = obj.SPCCPM_Description;
                            update.SPCCPM_ActiveFlag = true;
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
                else
                {
                    var checkduplicate = _context.ProgramMasterDMO.Where(d => d.MI_Id == obj.MI_Id && d.SPCCPM_Name.Equals(obj.SPCCPM_Name)).ToList();
                    if (checkduplicate.Count > 0)
                    {
                        obj.returnVal = "duplicate";
                    }
                    else
                    {
                        var mapp = Mapper.Map<ProgramMasterDMO>(obj);
                        mapp.SPCCPM_ActiveFlag = true;
                        mapp.CreatedDate = DateTime.Now;
                        mapp.UpdatedDate = DateTime.Now;
                        _context.Add(mapp);
                        int s = _context.SaveChanges();
                        if (s > 0)
                        {
                            obj.returnVal = "saved";
                        }
                        else
                        {
                            obj.returnVal = "savingFailed";
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

        public ProgramMasterDTO EditDetails(int id)
        {
            ProgramMasterDTO resp = new ProgramMasterDTO();
            try
            {
                resp.editDetails = _context.ProgramMasterDMO.Where(d => d.SPCCPM_Id == id).ToArray();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return resp;
        }

        public ProgramMasterDTO deactivate(ProgramMasterDTO obj)
        {
            try
            {
                var query = _context.ProgramMasterDMO.Where(d => d.SPCCPM_Id == obj.SPCCPM_Id).ToList();
                if (query.Count > 0)
                {
                    var update = _context.ProgramMasterDMO.Single(d => d.SPCCPM_Id == obj.SPCCPM_Id);
                    update.UpdatedDate = DateTime.Now;
                    update.SPCCPM_ActiveFlag = obj.SPCCPM_ActiveFlag;
                    _context.Update(update);
                    int s = _context.SaveChanges();
                    if (s > 0)
                    {
                        if (obj.SPCCPM_ActiveFlag == true)
                        {
                            obj.returnVal = "Record Activated Successfully";
                        }
                        else if (obj.SPCCPM_ActiveFlag == false)
                        {
                            obj.returnVal = "Record DeActivated Successfully";
                        }
                    }
                    else
                    {
                        obj.returnVal = "activationFailed";
                    }
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
