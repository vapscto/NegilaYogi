using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs;
using WebApplication1.Interfaces;
using System.Collections.Concurrent;
using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Services
{
    public class MasterReferenceImpl : MasterReferenceInterface
    {
        private static ConcurrentDictionary<string, MasterRefernceDTO> _login =
         new ConcurrentDictionary<string, MasterRefernceDTO>();

        public MasterReferenceContext _MasterRefernceContext;
        public MasterReferenceImpl(MasterReferenceContext masterRefernceContext)
        {
            _MasterRefernceContext = masterRefernceContext;
        }

        public MasterRefernceDTO DeleteMasterReferncDetails(int id)
        {
            
            MasterRefernceDTO master = new MasterRefernceDTO();
            List<MasterReference> mastersect = new List<MasterReference>(); // Mapper.Map<Organisation>(org);
            try
            {

                mastersect = _MasterRefernceContext.masterRefernce.Where(t => t.PAMR_Id.Equals(id)).ToList();

                if (mastersect.Any())
                {
                    _MasterRefernceContext.Remove(mastersect.ElementAt(0));
                    var contactExists = _MasterRefernceContext.SaveChanges();
                    if (contactExists == 1)
                    {
                        master.returnval = "True";
                    }
                    else
                    {
                        master.returnval = "False";
                    }
                }

                List<MasterReference> allmasterRefer= new List<MasterReference>();
                allmasterRefer = _MasterRefernceContext.masterRefernce.ToList();
                master.RefernceData = allmasterRefer.ToArray();
            }
            catch (Exception ee)
            {
                master.message = "Sorry...You can not delete this record.Because this record is mapped with student";
                List<MasterReference> allmasterRefer = new List<MasterReference>();
                allmasterRefer = _MasterRefernceContext.masterRefernce.ToList();
                master.RefernceData = allmasterRefer.ToArray();
                Console.WriteLine(ee.Message);
            }

            return master;
        }

    public MasterRefernceDTO EditMasterRefDetails(int id)
        {
            MasterRefernceDTO mast = new MasterRefernceDTO();
            try
            {
                List<MasterReference> mastersec = new List<MasterReference>();
                mastersec = _MasterRefernceContext.masterRefernce.AsNoTracking().Where(t => t.PAMR_Id.Equals(id)).ToList();

                mast.RefernceData = mastersec.ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return mast;
        }

        public MasterRefernceDTO GetMasterReferncDetails(MasterRefernceDTO master)
        {
            try
            {
                List<MasterReference> masterref = new List<MasterReference>();
                masterref = _MasterRefernceContext.masterRefernce.ToList();
                master.RefernceData = masterref.ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return master;


        }




        public MasterRefernceDTO SaveMasterRefernceDetails(MasterRefernceDTO master)
        {
            

            var duplicateresult = _MasterRefernceContext.masterRefernce.Where(t => t.PAMR_ReferenceName == master.PAMR_ReferenceName && t.PAMR_ReferenceDesc == master.PAMR_ReferenceDesc && t.PAMR_Id == master.PAMR_Id).ToList();
            var duplicateresult_update_by_ref = _MasterRefernceContext.masterRefernce.Where(t => t.PAMR_ReferenceName == master.PAMR_ReferenceName  && t.PAMR_Id != master.PAMR_Id).ToList();
            var duplicateresult_update = _MasterRefernceContext.masterRefernce.Where(t => ((t.PAMR_ReferenceName == master.PAMR_ReferenceName && t.PAMR_ReferenceDesc != master.PAMR_ReferenceDesc)|| (t.PAMR_ReferenceName != master.PAMR_ReferenceName && t.PAMR_ReferenceDesc == master.PAMR_ReferenceDesc)) && (t.PAMR_Id == master.PAMR_Id)).ToList();
            var duplicateresult_update_by_id = _MasterRefernceContext.masterRefernce.Where(t => t.PAMR_ReferenceName != master.PAMR_ReferenceName && t.PAMR_ReferenceDesc != master.PAMR_ReferenceDesc && t.PAMR_Id == master.PAMR_Id).ToList();
         


            if (duplicateresult.Count > 0)
                {
                    master.returnval = "Duplicate";
                    return master;
                }

            else if (duplicateresult_update_by_ref.Count > 0)
            {
                master.returnval = "Duplicate";
                return master;
            }

            else if(duplicateresult_update.Count>0)
            {
                var result = _MasterRefernceContext.masterRefernce.Single(t => t.PAMR_Id == master.PAMR_Id);

                result.PAMR_ReferenceName = master.PAMR_ReferenceName;
                result.PAMR_ReferenceDesc = master.PAMR_ReferenceDesc;
                

                result.UpdatedDate = DateTime.Now;
                _MasterRefernceContext.Update(result);
                var contactExists = _MasterRefernceContext.SaveChanges();

                if (contactExists == 1)
                {
                    master.returnval_update = "True";
                }
                else
                {
                    master.returnval_update = "False";
                }
            }

            else if (duplicateresult_update_by_id.Count > 0)
            {
                var result = _MasterRefernceContext.masterRefernce.Single(t => t.PAMR_Id == master.PAMR_Id);

                result.PAMR_ReferenceName = master.PAMR_ReferenceName;
                result.PAMR_ReferenceDesc = master.PAMR_ReferenceDesc;
                //added by 02/02/2017

                result.UpdatedDate = DateTime.Now;
                _MasterRefernceContext.Update(result);
                var contactExists = _MasterRefernceContext.SaveChanges();

                if (contactExists == 1)
                {
                    master.returnval_update = "True";
                }
                else
                {
                    master.returnval_update = "False";
                }
            }
            
            else
            {
                    MasterReference mas = Mapper.Map<MasterReference>(master);
                    //added by 02/02/2017
                    mas.CreatedDate = DateTime.Now;
                    mas.UpdatedDate = DateTime.Now;
                    _MasterRefernceContext.Add(mas);
                    //  _MasterRefernceContext.SaveChanges();
                    var contactExists = _MasterRefernceContext.SaveChanges();

                    if (contactExists == 1)
                    {
                        master.returnval_save = "True";
                    }
                    else
                    {
                        master.returnval_save = "False";
                    }
                }

           

            List<MasterReference> allmasterRefernce = new List<MasterReference>();
            allmasterRefernce = _MasterRefernceContext.masterRefernce.ToList();
            master.RefernceData = allmasterRefernce.ToArray();


            return master;
        }
        public MasterRefernceDTO getsearchdata(int id, MasterRefernceDTO org)
        {
            //string filetype = "All";
            MasterRefernceDTO pagedata = new MasterRefernceDTO();
            try
            {
                List<MasterReference> lorg = new List<MasterReference>();
                if (org.PAMR_ReferenceDesc == "Reference Name")
                {
                    lorg = _MasterRefernceContext.masterRefernce.Where(t => t.PAMR_ReferenceName.Contains(org.PAMR_ReferenceName)).ToList();

                }
                else if (org.PAMR_ReferenceDesc == "Reference Desc")
                {
                    lorg = _MasterRefernceContext.masterRefernce.Where(t => t.PAMR_ReferenceDesc.Contains(org.PAMR_ReferenceDesc)).ToList();

                }

              else if (org.PAMR_ReferenceDesc == "All")
                {
                    lorg = _MasterRefernceContext.masterRefernce.ToList();
                }
                org.RefernceData = lorg.ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return org;
        }
    }
}
