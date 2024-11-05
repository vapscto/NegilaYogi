(function () {
    'use strict';
    angular
        .module('app')
        .controller('PL_CI_Schedule_Company_JobTitleController', PL_CI_Schedule_Company_JobTitleController)
    PL_CI_Schedule_Company_JobTitleController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$window', '$stateParams', '$filter', 'superCache', '$q']
    function PL_CI_Schedule_Company_JobTitleController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $window, $stateParams, $filter, superCache, $q) {
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
        $scope.obj = {};

        $scope.loaddata = function () {
            var pageid = 2;
            apiService.getURI("PL_CI_Schedule_Company_JobTitle/loaddata", pageid).then(function (promise) {                 
            $scope.companylist = promise.pages;             
            $scope.savedata = promise.save;
            $scope.presentCountgrid = $scope.savedata.length;
               
            if (promise.savedata > 0) {
                   
                $scope.companynameList = promise.companynameList;
            }
            $scope.cancel();
        });
    }   
        $scope.submitted = false;

        $scope.saveRecord = function () {

       if ($scope.myForm.$valid) {
       var data = {
                "PLCISCHCOMJT_Id": $scope.plcischcomjT_Id,
                 "PLCISCHCOM_Id": $scope.PLMCOMP_Id,
                 "PLCISCHCOMJT_QulaificationCriteria": $scope.plcischcomjT_QulaificationCriteria,
                "PLCISCHCOMJT_NoOfInterviewRounds": $scope.plcischcomjT_NoOfInterviewRounds,
                "PLCISCHCOMJT_OtherDetails": $scope.PLCISCHCOMJT_OtherDetails,
                "PLCISCHCOMJT_JobTitle": $scope.PLCISCHCOMJT_JobTitle,
                }
              
            apiService.create("PL_CI_Schedule_Company_JobTitle/saveRecord", data).then(function (promise) {
                if (promise.returnval == 'saved') {
                    swal("Record Saved Successfully");
                    $scope.loaddata();
                }
                else if (promise.returnval == 'updated') {
                    swal("Record Updated Successfully");
                    $scope.loaddata();
                }
                else if (promise.returnval == 'duplicate') {
                    swal("Record already exist");
                }
                else if (promise.returnVal == "savingFailed") {
                    swal("Failed to save record");
                }
                else if (promise.returnVal == "updateFailed") {
                    swal("Failed to update record");
                }

                else {
                    swal("Sorry...something went wrong");
                }

            })
        }
        else {
            $scope.submitted = true;
        }
    };


        $scope.edit = function (item) {

            var data = {
                "PLCISCHCOMJT_Id": item.plcischcomjT_Id
            }
            apiService.create("PL_CI_Schedule_Company_JobTitle/EditDetails/", data).then(function (promise) {     
                $scope.editDetails = promise.editDetails;
                $scope.plcischcomjT_Id = promise.editDetails[0].plcischcomjT_Id;
                $scope.PLMCOMP_Id = promise.editDetails[0].plcischcoM_Id;
                $scope.plcischcomjT_QulaificationCriteria = promise.editDetails[0].plcischcomjT_QulaificationCriteria
                $scope.PLCISCHCOMJT_JobTitle = promise.editDetails[0].plcischcomjT_JobTitle;             
                $scope.plcischcomjT_NoOfInterviewRounds = promise.editDetails[0].plcischcomjT_NoOfInterviewRounds; 
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
            if (newuser1.plcischcomjT_ActiveFlag == false) {

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
                apiService.create("PL_CI_Schedule_Company_JobTitle/deactivate", newuser1).
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
            $scope.plcischcomjT_ActiveFlag = 0;
            $scope.PLMCOMP_Id = "";         
            $scope.plcischcomjT_NoOfInterviewRounds = "";
            $scope.plcischcomjT_QulaificationCriteria = "";
            $scope.PLCISCHCOMJT_OtherDetails = "";
            $scope.PLCISCHCOMJT_JobTitle = "";

            $scope.submitted = false;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();

        }
        $scope.interacted = function (field) {
            return $scope.submitted;
        };
            }
        }) ();
