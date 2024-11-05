(function () {
    'use strict';

    angular
        .module('app')
        .controller('MC_819_Accredition_ClinicallabController', MC_819_Accredition_ClinicallabController);

    MC_819_Accredition_ClinicallabController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout', '$q', '$filter', '$sce', 'myFactorynaac'];

    function MC_819_Accredition_ClinicallabController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout, $q, $filter, $sce, myFactorynaac) {
        $scope.searc_button = true;

        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        } else {
            paginationformasters = 10;
        }

        var miid = myFactorynaac.get();
        if (miid > 0) {
            $scope.mI_Id = miid;
        } else {
            $scope.mI_Id = 0;
        }
        $scope.currentPage = 1;
        $scope.itemsPerPage = paginationformasters;
        $scope.search = "";
        $scope.institute_flag = false;
        //=======================Page Load
        $scope.loaddata = function () {

            $scope.change_institution();
            if ($scope.mI_Id == undefined || $scope.mI_Id == null || $scope.mI_Id == '') {
                $scope.mI_Id = 0;
            }
            apiService.getURI("MC_819_Accredition_Clinicallab/loaddata", $scope.mI_Id).then(function (promise) {

                $scope.institutionlist = promise.institutionlist;
                $scope.mI_Id = promise.mI_Id;
                $scope.yearlist = promise.yearlist;
                $scope.alldata = promise.alldata813DC;

            })
        }

        $scope.sortBy = function (propertyName) {
            $scope.reverse = ($scope.propertyName === propertyName) ? !$scope.reverse : false;
            $scope.propertyName = propertyName;
        };

        //========================
        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.submitted = false;
        $scope.savedata = function () {
           
            if ($scope.myForm.$valid) {

                if ($scope.check1 == '1') {
                    $scope.NCDCCL813_CentralSterileSuppliesDepartmentFlag = true;
                }
                if ($scope.check2 == '1') {
                    $scope.NCDCCL813_ProvidesPersonalProtectiveEquipmentFlag = true;
                }
                if ($scope.check3 == '1') {
                    $scope.NCDCCL813_PatientSafetyCurriculumFlag = true;
                }
                if ($scope.check4 == '1') {
                    $scope.NCDCCL813_PeriodicFumigationClinicalAreasFlag = true;
                }
                if ($scope.check5 == '1') {
                    $scope.NCDCCL813_ImmunizationOfAllTheCaregiversFlag = true;
                }
                if ($scope.check6 == '1') {
                    $scope.NCDCCL813_NeedleStickInjuryRegisterFlag = true;
                }


                var data = {
                    "MI_Id": $scope.mI_Id,
                    "NCDCCL813_Id": $scope.ncdccL813_Id,
                    "ASMAY_Id": $scope.NCDCCL813_Year,
                    "NCDCCL813_CentralSterileSuppliesDepartmentFlag": $scope.NCDCCL813_CentralSterileSuppliesDepartmentFlag,
                    "NCDCCL813_ProvidesPersonalProtectiveEquipmentFlag": $scope.NCDCCL813_ProvidesPersonalProtectiveEquipmentFlag,
                    "NCDCCL813_PatientSafetyCurriculumFlag": $scope.NCDCCL813_PatientSafetyCurriculumFlag,
                    "NCDCCL813_PeriodicFumigationClinicalAreasFlag": $scope.NCDCCL813_PeriodicFumigationClinicalAreasFlag,
                    "NCDCCL813_ImmunizationOfAllTheCaregiversFlag": $scope.NCDCCL813_ImmunizationOfAllTheCaregiversFlag,
                    "NCDCCL813_NeedleStickInjuryRegisterFlag": $scope.NCDCCL813_NeedleStickInjuryRegisterFlag
                }

                apiService.create("MC_819_Accredition_Clinicallab/savedata1", data).then(function (promise) {
                  
                    if (promise.duplicate == false) {
                        if (promise.returnval == true) {
                            if ($scope.ncdccL813_Id > 0) {
                                swal('Record Updated Successfully!');
                            }
                            else {
                                swal('Record Saved Successfully!');
                            }
                            $state.reload();
                        }
                        else {
                            if ($scope.ncdccL813_Id > 0) {
                                swal('Record Nolt Updated Successfully!');
                            }
                            else {
                                swal('Record Not Saved Successfully!');
                            }
                        }
                    }
                    else {
                        swal('Record Already Exist!');
                    }
                })
            }
            else {
                $scope.submitted = true;
            }
        }



        //=========================Edit For Tab2 Mapping data
        //$scope.edittab2 = function (user) {
        //    var data = {
        //        "NCDCCL813_Id": user.ncdccL813_Id,
        //        "MI_Id": user.mI_Id,
        //    }
        //    apiService.create("MC_819_Accredition_Clinicallab/editdata", data).then(function (promise) {
        //        $scope.editdata = promise.editdata;
        //        $scope.institute_flag = true;
        //        $scope.NCMCCL819_Id = promise.editdata[0].ncmccL819_Id;
        //        $scope.mI_Id = promise.editdata[0].mI_Id;
        //        $scope.ASMAY_Id = promise.editdata[0].asmaY_Year;

        //        if (promise.editdata[0].ncmccL819_NABHAccnTechHoslFlg == true) {
        //            $scope.check1 = 1;
        //        }
        //        if (promise.editdata[0].ncmccL819_NABHAccnTechlabslFlg == true) {
        //            $scope.check2 = 1;
        //        }
        //        if (promise.editdata[0].ncmccL819_CertificationDeptlFlg == true) {
        //            $scope.check3 = 1;
        //        }
        //        if (promise.editdata[0].ncmccL819_OtherRecAccCertificationFlg == true) {
        //            $scope.check4 = 1;
        //        }

        //    })
        //}
        //==========================cancel Button  
        $scope.canceltab1 = function () {
            $state.reload();
        }

        $scope.change_institution = function () {
           // $scope.materaldocuupload = [{ id: 'Materal1' }];
            $scope.NCDCCL813_Year = '';
            $scope.ncdccL813_Id = 0;            
            $scope.NCDCCL813_CentralSterileSuppliesDepartmentFlag = '';
            $scope.NCDCCL813_ProvidesPersonalProtectiveEquipmentFlag = '';
            $scope.NCDCCL813_PatientSafetyCurriculumFlag = '';
            $scope.NCDCCL813_PeriodicFumigationClinicalAreasFlag = '';
            $scope.NCDCCL813_ImmunizationOfAllTheCaregiversFlag = '';
            $scope.NCDCCL813_NeedleStickInjuryRegisterFlag = '';
            $scope.submitted = false;
        }        
    }
})();
