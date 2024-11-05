(function () {
    'use strict';

    angular
        .module('app')
        .controller('NAAC_MC_VAC_141Controller', NAAC_MC_VAC_141Controller);

    NAAC_MC_VAC_141Controller.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout', '$q', '$filter', '$sce', 'myFactorynaac'];

    function NAAC_MC_VAC_141Controller($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout, $q, $filter, $sce, myFactorynaac) {
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
            apiService.getURI("NAAC_MC_VACcommon/loaddata", $scope.mI_Id).then(function (promise) {

                $scope.institutionlist = promise.institutionlist;
                $scope.mI_Id = promise.mI_Id;
                $scope.yearlist = promise.yearlist;

                $scope.alldata = promise.alldata141;

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
                    $scope.NCMCVAC141_FKFromStudents = true;
                }
                if ($scope.check2 == '1') {
                    $scope.NCMCVAC141_FKFromteachers = true;
                }
                if ($scope.check3 == '1') {
                    $scope.NCMCVAC141_FKFromemployers = true;
                }
                if ($scope.check4 == '1') {
                    $scope.NCMCVAC141_FKFromalumni = true;
                }
                if ($scope.check5 == '1') {
                    $scope.FkCollFromOtherProfs = true;
                }
               
                    var data = {
                        "MI_Id": $scope.mI_Id,
                        "NCMCVAC141_Id": $scope.ncmcvaC141_Id,
                        "ASMAY_Id": $scope.ASMAY_Id,                      
                        "NCMCVAC141_FKFromStudents": $scope.NCMCVAC141_FKFromStudents,
                        "NCMCVAC141_FKFromteachers": $scope.NCMCVAC141_FKFromteachers,
                        "NCMCVAC141_FKFromemployers": $scope.NCMCVAC141_FKFromemployers,
                        "NCMCVAC141_FKFromalumni": $scope.NCMCVAC141_FKFromalumni,
                        "FkCollFromOtherProfs": $scope.FkCollFromOtherProfs,
                    }
               
                apiService.create("NAAC_MC_VACcommon/savedata141", data).then(function (promise) {
                    if (promise.duplicate == false) {
                        if (promise.returnval == true) {
                            if ($scope.ncmcvaC141_Id > 0) {
                                swal('Record Updated Successfully!');
                            }
                            else {
                                swal('Record Saved Successfully!');
                            }
                            $state.reload();
                        }
                        else {
                            if ($scope.ncmcvaC141_Id > 0) {
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
        $scope.edittab2 = function (user) {
            var data = {
                "NCMCVAC141_Id": user.ncmcvaC141_Id,
                "MI_Id": user.mI_Id,
            }
            apiService.create("NAAC_MC_VACcommon/editdata", data).then(function (promise) {
                $scope.editdata = promise.editdata;
                $scope.institute_flag = true;
                $scope.NCMCVAC141_Id = promise.editdata[0].ncmcvaC141_Id;
                $scope.mI_Id = promise.editdata[0].mI_Id;
                $scope.ASMAY_Id = promise.editdata[0].ncmcvaC141_year;

                if (promise.editdata[0].ncmcvaC141_FKFromStudents == true) {
                    $scope.check1 = 1;
                }
                if (promise.editdata[0].ncmcvaC141_FKFromteachers == true) {
                    $scope.check2 = 1;
                }
                if (promise.editdata[0].ncmcvaC141_FKFromemployers == true) {
                    $scope.check3 = 1;
                }
                if (promise.editdata[0].ncmcvaC141_FKFromalumni == true) {
                    $scope.check4 = 1;
                }
                if (promise.editdata[0].fkCollFromOtherProfs == true) {
                    $scope.check5 = 1;
                }
            })
        }
        //==========================cancel Button  for Tab2
        $scope.canceltab2 = function () {
            $state.reload();
        }
       
        $scope.change_institution = function () {
            $scope.materaldocuupload = [{ id: 'Materal1' }];
            $scope.ASMAY_Id = "";
            $scope.ncacmpR112_Id = 0;
            $scope.ncacpR112_Id = 0;
            $scope.AMCO_Id = "";
            $scope.AMB_Id = "";
            $scope.NCACMPR112_DiplomaCertName = "";
            $scope.ncacpR112_Date = "";
            $scope.submitted = false;
        }



    }

})();
