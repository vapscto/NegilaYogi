(function () {
    'use strict';

    angular
        .module('app')
        .controller('HSU_ClinicalLaboratory232', HSU_ClinicalLaboratory232);

    HSU_ClinicalLaboratory232.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout', '$q', '$filter', '$sce', 'myFactorynaac'];

    function HSU_ClinicalLaboratory232($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout, $q, $filter, $sce, myFactorynaac) {
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
            apiService.getURI("HSU_MasterCR2/loaddata", $scope.mI_Id).then(function (promise) {

                $scope.institutionlist = promise.institutionlist;
                $scope.mI_Id = promise.mI_Id;
                $scope.yearlist = promise.yearlist;

                $scope.alldata = promise.alldata232;

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
                    $scope.NCHSUCS232_CsTrclinicalskillsRelevantFlag = true;
                }
                if ($scope.check2 == '1') {
                    $scope.NCHSUCS232_PatientSimulatorsSimulationbasedFlag = true;
                }
                if ($scope.check3 == '1') {
                    $scope.NCHSUCS232_StProgConductedSssessmentStudentsFlag = true;
                }
                if ($scope.check4 == '1') {
                    $scope.NCHSUCS232_TrProgConForCsSblearningFlag = true;
                }
                
                
                var data = {
                    "MI_Id": $scope.mI_Id,
                    "NCHSUCS232_Id": $scope.nchsucS232_Id,
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "NCHSUCS232_CsTrclinicalskillsRelevantFlag": $scope.NCHSUCS232_CsTrclinicalskillsRelevantFlag,
                    "NCHSUCS232_PatientSimulatorsSimulationbasedFlag": $scope.NCHSUCS232_PatientSimulatorsSimulationbasedFlag,                  
                    "NCHSUCS232_StProgConductedSssessmentStudentsFlag": $scope.NCHSUCS232_StProgConductedSssessmentStudentsFlag,
                    "NCHSUCS232_TrProgConForCsSblearningFlag": $scope.NCHSUCS232_TrProgConForCsSblearningFlag,
                   
                }

                apiService.create("HSU_MasterCR2/save_HSU_232", data).then(function (promise) {
                    if (promise.duplicate == false) {
                        if (promise.returnval == true) {
                            if ($scope.nchsucS232_Id > 0) {
                                swal('Record Updated Successfully!');
                            }
                            else {
                                swal('Record Saved Successfully!');
                            }
                            $state.reload();
                        }
                        else {
                            if ($scope.nchsucS232_Id > 0) {
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
                "NCHSUCS232_Id": user.nchsucS232_Id,
                "MI_Id": user.mI_Id,
            }
            apiService.create("HSU_MasterCR2/editdata142", data).then(function (promise) {
                $scope.editdata = promise.editdata;
                $scope.institute_flag = true;
                $scope.nchsucS232_Id = promise.editdata[0].nchsucS232_Id;
                $scope.mI_Id = promise.editdata[0].mI_Id;
                $scope.ASMAY_Id = promise.editdata[0].ncmcvAC142_year;

                if (promise.editdata[0].NCHSUCS232_CsTrclinicalskillsRelevantFlag == true) {
                    $scope.check1 = 1;
                }
                if (promise.editdata[0].NCHSUCS232_PatientSimulatorsSimulationbasedFlag == true) {
                    $scope.check2 = 1;
                }
                if (promise.editdata[0].NCHSUCS232_StProgConductedSssessmentStudentsFlag == true) {
                    $scope.check3 = 1;
                }
                if (promise.editdata[0].NCHSUCS232_TrProgConForCsSblearningFlag == true) {
                    $scope.check4 = 1;
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
