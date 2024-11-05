using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs;

namespace WebApplication1.Interfaces
{
    public interface EnquiryInterface
    {
        //string enqdata(Enq id);
        Enq countrydrp(Enq en);

        StateDTO enqdrpcountrydata(int id);
        Task<Enq>  saveEnqdata(Enq enqu);
        Enq clearEnqdata(int Id);
        CityDTO getcity(int id);
        Enq EditDetails(int  id);
        Enq DeleteEnqDetails(Enq enqu);
        //Enq GetAllDetais(Enq en);

        //Dashboard mapping 
        dasAzure_StorageDTO storageDetails(dasAzure_StorageDTO enqu);
        dasAzure_StorageDTO editstorage(int id);
        dasAzure_StorageDTO saveStoragedetails(dasAzure_StorageDTO enqu);

        dasMappingDTO saveMappingdetail(dasMappingDTO enqu);
        dasMappingDTO getmappingedit(int id);
        dasMappingDTO deletemappingrecord(int id);

        dasMappingDTO savepreadmissionDetail(dasMappingDTO enqu);
        dasMappingDTO getpremappingedit(dasMappingDTO data);
        dasMappingDTO deletepremappingrecord(dasMappingDTO data);

        //Rolewise Instituion Mapping
        IVRM_User_Login_InstitutionwiseDTO getuserdata(IVRM_User_Login_InstitutionwiseDTO enqu);
        IVRM_User_Login_InstitutionwiseDTO getinstitution(IVRM_User_Login_InstitutionwiseDTO enqu);
        IVRM_User_Login_InstitutionwiseDTO getcartdata(IVRM_User_Login_InstitutionwiseDTO enqu);
        IVRM_User_Login_InstitutionwiseDTO savethirdDetail(IVRM_User_Login_InstitutionwiseDTO enqu);

      //  IVRM_User_Login_InstitutionwiseDTO loadthirddata(IVRM_User_Login_InstitutionwiseDTO enqu);
        IVRM_User_Login_InstitutionwiseDTO deletegriddata(IVRM_User_Login_InstitutionwiseDTO enqu);
        
    }
}
