using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs;
using WebApplication1.Interfaces;
using System.Collections.Concurrent;
using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using DomainModel.Model.com.vaps.admission;

namespace WebApplication1.Services
{
    public class MasterSubjectImpl : MasterSubjectInterface
    {
        private static ConcurrentDictionary<string, MasterSubjectInterface> _login =
         new ConcurrentDictionary<string, MasterSubjectInterface>();

        public MasterSubjectContext _MasterSubjectContext;
        public MasterSubjectImpl(MasterSubjectContext masterSubjectContext)
        {
            _MasterSubjectContext = masterSubjectContext;
        }

        public MasterSubjectDTO DeleteMasterSubDetails(int id)
        {
           // bool returnresult = false;
            MasterSubjectDTO master = new MasterSubjectDTO();
            List<MasterSubjectDMO> mastersect = new List<MasterSubjectDMO>(); // Mapper.Map<Organisation>(org);


            try
            {
              var  result = _MasterSubjectContext.masterSubject.Single(t => t.PAMSU_Id.Equals(id));

                if (result.PAMSU_ActiveFlag == 1)
                {
                    result.PAMSU_ActiveFlag = 0;
                    result.UpdatedDate = DateTime.Now;
                    result.CreatedDate = result.CreatedDate;
                    _MasterSubjectContext.Update(result);
                    _MasterSubjectContext.SaveChanges();
                    master.returnval = "true";
                }
                else
                {
                    result.UpdatedDate = DateTime.Now;
                    result.CreatedDate = result.CreatedDate;
                    result.PAMSU_ActiveFlag = 1;
                    _MasterSubjectContext.Update(result);
                    _MasterSubjectContext.SaveChanges();
                    master.returnval ="false";
                }


                //if (mastersect.Any())
                //{
                //    _MasterSubjectContext.Remove(mastersect.ElementAt(0));
                //    var contactExists = _MasterSubjectContext.SaveChanges();
                //    if (contactExists == 1)
                //    {
                //        returnresult = true;
                //        master.returnval = returnresult;
                //    }
                //    else
                //    {
                //        returnresult = false;
                //        master.returnval = returnresult;
                //    }
                //}

                List<MasterSubjectDMO> allmastersection = new List<MasterSubjectDMO>();
                allmastersection = _MasterSubjectContext.masterSubject.Where(t => t.MI_Id == result.MI_Id).ToList();
                master.MasterSubjectData = allmastersection.ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return master;
        }

        public MasterSubjectDTO EditMasterSubDetails(int id)
        {
            MasterSubjectDTO mast = new MasterSubjectDTO();
            try
            {
                List<MasterSubjectDMO> mastersub = new List<MasterSubjectDMO>();
                mastersub = _MasterSubjectContext.masterSubject.AsNoTracking().Where(t => t.PAMSU_Id.Equals(id)).ToList();

                mast.MasterSubjectData = mastersub.ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return mast;
        }

        public MasterSubjectDTO GetMasterSubDetails(MasterSubjectDTO master)
        {
            //MasterSectionDTO mast = new MasterSectionDTO();
            try
            {
                List<MasterSubjectDMO> mastersec = new List<MasterSubjectDMO>();
                mastersec = _MasterSubjectContext.masterSubject.Where(t=>t.MI_Id==master.MI_Id).ToList();
                master.MasterSubjectData = mastersec.ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return master;
        }

        public MasterSubjectDTO SaveMasterSubDetails(MasterSubjectDTO master)
        {

           // bool returnresult = false;
            School_M_Section maspge = new School_M_Section();
            if (master.PAMS_Id > 0)
            {
                var duplicateresult = _MasterSubjectContext.masterSubject.Where(t =>t.MI_Id==master.MI_Id && t.PAMSU_SubjectName==master.PAMS_SubjectName).Count();
                if (duplicateresult > 0)
                {
                    master.returnval = "Duplicate";
                    return master;
                }
                else
                {
                    var result = _MasterSubjectContext.masterSubject.Single(t => t.PAMSU_Id == master.PAMS_Id);
                    // var result = _OrganisationContext.Organisation.AsNoTracking().Single(t => t.MO_Id == enq.MO_Id);

                    result.MI_Id = master.MI_Id;
                    result.PAMSU_SubjectName = master.PAMS_SubjectName;
                    result.PAMSU_SubjectCode = master.PAMS_SubjectCode;
                    result.PAMSU_MaxMarks = master.PAMS_MaxMarks;
                    result.PAMSU_MinMarks = master.PAMS_MinMarks;
                    result.PAMSU_SubjectFlag = master.PAMS_SubjectFlag;
                    //added by 02/02/2017
                    result.UpdatedDate = DateTime.Now;
                    _MasterSubjectContext.Update(result);
                    var contactExists = _MasterSubjectContext.SaveChanges();

                    if (contactExists == 1)
                    {
                       // returnresult = true;
                        master.returnval = "true";
                    }
                    else
                    {
                       // returnresult = false;
                        master.returnval = "false";
                    }
                }
            }

            else
            {
                MasterSubjectDMO mas = Mapper.Map<MasterSubjectDMO>(master);
                var duplicateresult = _MasterSubjectContext.masterSubject.Where(t => t.MI_Id == master.MI_Id && t.PAMSU_SubjectName == master.PAMS_SubjectName).Count();
                if (duplicateresult > 0)
                {
                    master.returnval = "Duplicate";
                    return master;
                }
                else
                {

                    mas.PAMSU_ActiveFlag = 1;
                    //added by 02/02/2017
                    mas.CreatedDate = DateTime.Now;
                    mas.UpdatedDate = DateTime.Now;
                    _MasterSubjectContext.Add(mas);
                    _MasterSubjectContext.SaveChanges();
                }
            }

            List<MasterSubjectDMO> allmastersection = new List<MasterSubjectDMO>();
            allmastersection = _MasterSubjectContext.masterSubject.Where(t => t.MI_Id == master.MI_Id).ToList();
            master.MasterSubjectData = allmastersection.ToArray();


            return master;
        }
    }
}
