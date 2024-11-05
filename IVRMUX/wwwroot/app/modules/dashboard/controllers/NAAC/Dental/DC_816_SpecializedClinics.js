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

                $scope.alldata = promise.alldata816DC;

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
                    $scope.NCDCSC816_ComprehensiveclinicFlag = true;
                }
                if ($scope.check2 == '1') {
                    $scope.NCDCSC816_ImplantClinicFlag = true;
                }
                if ($scope.check3 == '1') {
                    $scope.NCDCSC816_GeriatricClinicFlag = true;
                }
                if ($scope.check4 == '1') {
                    $scope.NCDCSC816_SpecialHealthCareNeedsClinicFlag = true;
                }
                if ($scope.check4 == '1') {
                    $scope.NCDCSC816_TobaccoCessationClinicFlag = true;
                }
                if ($scope.check4 == '1') {
                    $scope.NCDCSC816_EstheticClinicFlag = true;
                }


                var data = {
                    "MI_Id": $scope.mI_Id,
                    "NCDCSC816_Id": $scope.ncdcsC816_Id,
                    "ASMAY_Id": $scope.NCDCSC816_Year,
                    "NCDCSC816_ComprehensiveclinicFlag": $scope.NCDCSC816_ComprehensiveclinicFlag,
                    "NCDCSC816_ImplantClinicFlag": $scope.NCDCSC816_ImplantClinicFlag,
                    "NCDCSC816_GeriatricClinicFlag": $scope.NCDCSC816_GeriatricClinicFlag,
                    "NCDCSC816_SpecialHealthCareNeedsClinicFlag": $scope.NCDCSC816_SpecialHealthCareNeedsClinicFlag,
                    "NCDCSC816_TobaccoCessationClinicFlag": $scope.NCDCSC816_TobaccoCessationClinicFlag,
                    "NCDCSC816_EstheticClinicFlag": $scope.NCDCSC816_EstheticClinicFlag,

                }

                apiService.create("MC_819_Accredition_Clinicallab/savedata3", data).then(function (promise) {
                    debugger;
                    if (promise.duplicate == false) {
                        if (promise.returnval == true) {
                            if ($scope.ncdcsC816_Id > 0) {
                                swal('Record Updated Successfully!');
                            }
                            else {
                                swal('Record Saved Successfully!');
                            }
                            $state.reload();
                        }
                        else {
                            if ($scope.ncdcsC816_Id > 0) {
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
        $scope.canceltab = function () {
            $state.reload();
        }

        $scope.change_institution = function () {
           // $scope.materaldocuupload = [{ id: 'Materal1' }];
            $scope.NCDCSC816_Year = '';
            $scope.ncdcsC816_Id = 0;
            $scope.NCDCSC816_ComprehensiveclinicFlag = '';
            $scope.NCDCSC816_ImplantClinicFlag = '';
            $scope.NCDCSC816_GeriatricClinicFlag = '';
            $scope.NCDCSC816_SpecialHealthCareNeedsClinicFlag = '';
            $scope.NCDCSC816_TobaccoCessationClinicFlag = '';
            $scope.submitted = false;
        }



    }

})();
