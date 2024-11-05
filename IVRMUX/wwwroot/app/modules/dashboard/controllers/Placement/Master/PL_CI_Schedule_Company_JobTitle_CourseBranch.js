(function () {
    'use strict';
    angular
        .module('app')
        .controller('PL_CI_Schedule_Company_JobTitle_CourseBranchController', PL_CI_Schedule_Company_JobTitle_CourseBranchController)
    PL_CI_Schedule_Company_JobTitle_CourseBranchController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$window', '$stateParams', '$filter', 'superCache', '$q']
    function PL_CI_Schedule_Company_JobTitle_CourseBranchController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $window, $stateParams, $filter, superCache, $q) {
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
            apiService.getURI("PL_CI_Schedule_Company_JobTitle_CourseBranch/loaddata", pageid).then(function (promise) {
                $scope.joblist = promise.pages;
                $scope.courselist = promise.course;
                $scope.branchlist = promise.branch;
                $scope.savedata = promise.save;
                $scope.presentCountgrid = $scope.savedata.length;

                if (promise.savedata > 0) {

                    $scope.branchcourseList = promise.branchcourseList;
                }
                $scope.cancel();
            });
        }


        //$scope.submitted = false;
        //$scope.saveRecord = function () {
          
        //    if ($scope.myForm.$valid) {

        //        var data = {
        //            "PLCISCHCOMJTCB_Id": $scope.plcischcomjtcB_Id,
        //            "PLCISCHCOMJT_Id": $scope.PLCISCHCOMJT_Id,
        //            "AMCO_Id": $scope.AMCO_Id,
        //            "AMB_Id": $scope.AMB_Id,                  
        //            "PLCISCHCOMJTCB_ApplicableSEM": $scope.PLCISCHCOMJTCB_ApplicableSEM,           
        //        }
        //        apiService.create("PL_CI_Schedule_Company_JobTitle_CourseBranch/saveRecord", data).then(function (promise) {
        //            if (promise.returnval == 'saved') {
        //                swal("Record Saved Successfully");
        //                $scope.loaddata();
        //            }
        //            else if (promise.returnval == 'updated') {
        //                    swal("Record Updated Successfully");
        //                $scope.loaddata();
        //                }
        //            else if (promise.returnval == 'duplicate') {
        //                swal("Record already exist");
        //            }
        //            else if (promise.returnVal == "savingFailed") {
        //                swal("Failed to save record");
        //            }
        //            else if (promise.returnVal == "updateFailed") {
        //                swal("Failed to update record");
        //            }

        //            else {
        //                swal("Sorry...something went wrong");
        //            }
                 
        //        })
        //    }
        //    else {
        //        $scope.submitted = true;
        //    }
        //};


        $scope.submitted = false;
        $scope.saveRecord = function () {

            if ($scope.PLCISCHCOMJTCB_Id == undefined) {
                $scope.PLCISCHCOMJTCB_Id = 0;

            }

            if ($scope.myForm.$valid) {
                var data = {               
                    "PLCISCHCOMJTCB_Id": $scope.plcischcomjtcB_Id,
                    "PLCISCHCOMJT_Id": $scope.PLCISCHCOMJT_Id,
                    "AMCO_Id": $scope.AMCO_Id,
                    "AMB_Id": $scope.AMB_Id,                  
                    "PLCISCHCOMJTCB_ApplicableSEM": $scope.PLCISCHCOMJTCB_ApplicableSEM,    

                }
                apiService.create("PL_CI_Schedule_Company_JobTitle_CourseBranch/saveRecord/", data).then(function (promise) {
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
                "PLCISCHCOMJTCB_Id": item.plcischcomjtcB_Id
            }
            apiService.create("PL_CI_Schedule_Company_JobTitle_CourseBranch/EditDetails/", data).then(function (promise) {
                $scope.editDetails = promise.editDetails;
                $scope.PLCISCHCOMJT_Id = promise.editDetails[0].plcischcomjT_Id;
                $scope.AMCO_Id = promise.editDetails[0].amcO_Id;
                $scope.AMB_Id = promise.editDetails[0].amB_Id;
                $scope.PLCISCHCOMJTCB_ApplicableSEM = promise.editDetails[0].plcischcomjtcB_ApplicableSEM;
                $scope.plcischcomjtcB_Id = promise.editDetails[0].plcischcomjtcB_Id;          
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
            if (newuser1.plcischcomjtcB_ActiveFlag == false) {

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
                        apiService.create("PL_CI_Schedule_Company_JobTitle_CourseBranch/deactivate", newuser1).
                            then(function (promise) {

                                if (promise.retval == true) {
                                    swal("Record Not " + mgs + "d Successfully!!!");
                                    $scope.loaddata();
                                }
                                else {
                                    swal("Record  " + mgs + "d Successfully!!!");
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
            $scope.PLCISCHCOMJTCB_ActiveFlag = 0;
            $scope.PLCISCHCOMJT_Id = "";
            $scope.AMCO_Id = "";
            $scope.AMB_Id = "";    
            $scope.PLCISCHCOMJTCB_ApplicableSEM = "";
           
            $scope.submitted = false;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();

        }
        $scope.interacted = function (field) {
            return $scope.submitted;
        };
    }
})();
