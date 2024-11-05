(function () {
    'use strict';
    angular
        .module('app')
        .controller('PL_CI_Schedule_Company_JobTitle_CriteriaController', PL_CI_Schedule_Company_JobTitle_CriteriaController)
   PL_CI_Schedule_Company_JobTitle_CriteriaController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$window', '$stateParams', '$filter', 'superCache', '$q']
    function PL_CI_Schedule_Company_JobTitle_CriteriaController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $window, $stateParams, $filter, superCache, $q) {
        var paginationformasters;
        var copty;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings != null && ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
            copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
        }
        $scope.itemsPerPage = paginationformasters;
        $scope.coptyright = copty;
        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;
        $scope.searchValue = "";
        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey == key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        }

        $scope.loaddata = function () {
            var pageid = 2;
            apiService.getURI("PL_CI_Schedule_Company_JobTitle_Criteria/loaddata", pageid).then(function (promise) {            
             
                $scope.joblist = promise.jobtitlelist;    
                $scope.courselist = promise.course;
                $scope.savedata = promise.save;
                $scope.presentCountgrid = $scope.savedata.length;
                if (promise.savedata > 0) {

                    $scope.crietrialist = promise.crietrialist;
                }
                $scope.cancel();
            });
        }


        $scope.submitted = false;
        $scope.saveRecord = function () {

            if ($scope.PLCISCHCOMJTCR_Id == undefined) {
                $scope.PLCISCHCOMJTCR_Id = 0;

            }

            if ($scope.myForm.$valid) {
                var data = {
                    "PLCISCHCOMJTCR_Id": $scope.PLCISCHCOMJTCR_Id,
                    "AMCO_Id": $scope.AMCO_Id,
                    "PLCISCHCOMJT_Id": $scope.PLCISCHCOMJT_Id,
                    "plcischcomjtcR_CutOfMark": $scope.plcischcomjtcR_CutOfMark,
                    "PLCISCHCOMJTCR_OtherDetails": $scope.PLCISCHCOMJTCR_OtherDetails,           
                }
                apiService.create("PL_CI_Schedule_Company_JobTitle_Criteria/saveRecord/", data).then(function (promise) {
                    if (promise.returnval == 'saved') {
                        swal("Record Saved Successfully");
                    }
                    else if (promise.returnval == 'savingFailed') {
                        swal("Record Not saved");
                    }
                    else if (promise.returnval == 'duplicate') {
                        swal("Record Already Exist");
                    }
                    else if (promise.returnval == 'updated') {
                        swal("Record Update Sucessfully !");
                    }
                    else if (promise.returnval == 'updateFailed') {
                        swal("Record Not Update  !");

                    }
                    else if (promise.returnval == 'admin') {
                        swal("Please Contact Administrartor   !");
                    }
                    $state.reload();
                })
            }
            else {
                $scope.submitted = true;
            }
        };        
      

        $scope.edit = function (item) {

            var data = {
                "PLCISCHCOMJTCR_Id": item.plcischcomjtcR_Id
            }
            apiService.create("PL_CI_Schedule_Company_JobTitle_Criteria/EditDetails/", data).then(function (promise) {
                $scope.editDetails = promise.editDetails;
                $scope.PLCISCHCOMJTCR_Id = promise.editDetails[0].plcischcomjtcR_Id;
                $scope.AMCO_Id = promise.editDetails[0].amcO_Id;
                $scope.PLCISCHCOMJT_Id = promise.editDetails[0].plcischcomjT_Id
                $scope.plcischcomjtcR_CutOfMark = promise.editDetails[0].plcischcomjtcR_CutOfMark;
                $scope.PLCISCHCOMJTCR_OtherDetails = promise.editDetails[0].plcischcomjtcR_OtherDetails;
            });
        }
        //==============================================Deactivate
        $scope.deactive = function (newuser1, SweetAlertt) {
            
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            var mgs = "";
            if (newuser1.plcischcomjtcR_ActiveFlag == false) {

                mgs = "Activate";

            } else {
                mgs = "Deactivate";
            }
            swal({
                title: "Are you sure?",
                text: "Do you want to  " + mgs + " Record?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55",
                confirmButtonText: "Yes, " + mgs + " it!",
                cancelButtonText: "Cancel..!",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("PL_CI_Schedule_Company_JobTitle_Criteria/deactivate", newuser1).
                            then(function (promise) {
                            
                                if (promise.retval == true) {
                                    swal("Record " + mgs + "d Successfully!!!");
                                    $scope.loaddata();
                                }
                                else {
                                    swal("Record Not " + mgs + "d Successfully!!!");
                                    $scope.loaddata();
                                }

                            })
                    }
                    else {
                        swal("Cancelled");
                    }
                })
        }

        $scope.cancel = function () {
            $scope.plcischcomjtcR_ActiveFlag = 0;
            $scope.plcischcomjtcR_CutOfMark = "";
            $scope.PLCISCHCOMJTCR_OtherDetails = "";
            $scope.AMCO_Id = "";
            $scope.PLCISCHCOMJT_Id = "";

            $scope.submitted = false;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();

        }
        $scope.interacted = function (field) {
            return $scope.submitted;
        };
    }
})();
