(function () {
    'use strict';

    angular
        .module('app')
        .controller('HSU_ESM255', HSU_ESM255);

    HSU_ESM255.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout', '$q', '$filter', '$sce', 'myFactorynaac'];

    function HSU_ESM255($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout, $q, $filter, $sce, myFactorynaac) {
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

                $scope.alldata = promise.alldata255;

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
                    $scope.NCHSUEM255_AnDivImpEMFlag = true;
                }
                if ($scope.check2 == '1') {
                    $scope.NCHSUEM255_StuRegHtIssueProcessingFlag = true;
                }
                if ($scope.check3 == '1') {
                    $scope.NCHSUEM255_StuRegResultProcFlag = true;
                }
                if ($scope.check4 == '1') {
                    $scope.NCHSUEM255_ResultProcAtdFlag = true;
                }
                if ($scope.check5 == '1') {
                    $scope.NCHSUEM255_ManualMethodologyFlag = true;
                }
                
                var data = {
                    "MI_Id": $scope.mI_Id,
                    "NCHSUEM255_Id": $scope.nchsueM255_Id,
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "NCHSUEM255_AnDivImpEMFlag": $scope.NCHSUEM255_AnDivImpEMFlag,
                    "NCHSUEM255_StuRegHtIssueProcessingFlag": $scope.NCHSUEM255_StuRegHtIssueProcessingFlag,                 
                    "NCHSUEM255_StuRegResultProcFlag": $scope.NCHSUEM255_StuRegResultProcFlag,
                    "NCHSUEM255_ResultProcAtdFlag": $scope.NCHSUEM255_ResultProcAtdFlag,
                    "NCHSUEM255_ManualMethodologyFlag": $scope.NCHSUEM255_ManualMethodologyFlag,
                }

                apiService.create("HSU_MasterCR2/save_HSU_255", data).then(function (promise) {
                    if (promise.duplicate == false) {
                        if (promise.returnval == true) {
                            if ($scope.nchsueM255_Id > 0) {
                                swal('Record Updated Successfully!');
                            }
                            else {
                                swal('Record Saved Successfully!');
                            }
                            $state.reload();
                        }
                        else {
                            if ($scope.nchsueM255_Id > 0) {
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
                "NCHSUEM255_Id": user.nchsueM255_Id,
                "MI_Id": user.mI_Id,
            }
            apiService.create("HSU_MasterCR2/editdata142", data).then(function (promise) {
                $scope.editdata = promise.editdata;
                $scope.institute_flag = true;
                $scope.nchsueM255_Id = promise.editdata[0].nchsueM255_Id;
                $scope.mI_Id = promise.editdata[0].mI_Id;
                $scope.ASMAY_Id = promise.editdata[0].ncmcvAC142_year;

                if (promise.editdata[0].NCHSUEM255_AnDivImpEMFlag == true) {
                    $scope.check1 = 1;
                }
                if (promise.editdata[0].NCHSUEM255_StuRegHtIssueProcessingFlag == true) {
                    $scope.check2 = 1;
                }
                if (promise.editdata[0].NCHSUEM255_StuRegResultProcFlag == true) {
                    $scope.check3 = 1;
                }
                if (promise.editdata[0].NCHSUEM255_ResultProcAtdFlag == true) {
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
