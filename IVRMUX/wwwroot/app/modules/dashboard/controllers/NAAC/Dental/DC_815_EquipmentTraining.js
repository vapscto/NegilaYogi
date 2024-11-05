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
                $scope.alldata = promise.alldata815DC;

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
            debugger;
            if ($scope.myForm.$valid) {

                if ($scope.check1 == '1') {
                    $scope.NCDCEQT815_ConeBeamComputedTomogramFlag = true;
                }
                if ($scope.check2 == '1') {
                    $scope.NCDCEQT815_CAMFacilityFlag = true;
                }
                if ($scope.check3 == '1') {
                    $scope.NCDCEQT815_ImagingMorphomEtricSoftwaresFlag = true;
                }
                if ($scope.check4 == '1') {
                    $scope.NCDCEQT815_DentalLASERUnitFlag = true;
                }
                if ($scope.check5 == '1') {
                    $scope.NCDCEQT815_ExtendedApplicationLightBasedMicroscopyFlag = true;
                }


                var data = {
                    "MI_Id": $scope.mI_Id,
                    "NCDCEQT815_Id": $scope.ncdceqT815_Id,
                    "ASMAY_Id": $scope.NCDCEQT815_Year,
                    "NCDCEQT815_ConeBeamComputedTomogramFlag": $scope.NCDCEQT815_ConeBeamComputedTomogramFlag,
                    "NCDCEQT815_CAMFacilityFlag": $scope.NCDCEQT815_CAMFacilityFlag,
                    "NCDCEQT815_ImagingMorphomEtricSoftwaresFlag": $scope.NCDCEQT815_ImagingMorphomEtricSoftwaresFlag,
                    "NCDCEQT815_DentalLASERUnitFlag": $scope.NCDCEQT815_DentalLASERUnitFlag,
                    "NCDCEQT815_ExtendedApplicationLightBasedMicroscopyFlag": $scope.NCDCEQT815_ExtendedApplicationLightBasedMicroscopyFlag

                }

                apiService.create("MC_819_Accredition_Clinicallab/savedata2", data).then(function (promise) {
                    debugger;
                    if (promise.duplicate == false) {
                        if (promise.returnval == true) {
                            if ($scope.ncdceqT815_Id > 0) {
                                swal('Record Updated Successfully!');
                            }
                            else {
                                swal('Record Saved Successfully!');
                            }
                            $state.reload();
                        }
                        else {
                            if ($scope.ncdceqT815_Id > 0) {
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



        ////=========================Edit For Tab2 Mapping data
        //$scope.edittab2 = function (user) {
        //    var data = {
        //        "NCMCCL819_Id": user.ncmccL819_Id,
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
        //==========================cancel Button  for Tab2
        $scope.canceltab2 = function () {
            $state.reload();
        }

        $scope.change_institution = function () {
           // $scope.materaldocuupload = [{ id: 'Materal1' }];
            $scope.NCDCEQT815_Year = '';
            $scope.ncdceqT815_Id = 0;
            $scope.NCDCEQT815_ConeBeamComputedTomogramFlag = '';
            $scope.NCDCEQT815_CAMFacilityFlag = '';
            $scope.NCDCEQT815_ImagingMorphomEtricSoftwaresFlag = '';
            $scope.NCDCEQT815_DentalLASERUnitFlag = '';
            $scope.NCDCEQT815_ExtendedApplicationLightBasedMicroscopyFlag = '';
            $scope.submitted = false;
        }



    }

})();
