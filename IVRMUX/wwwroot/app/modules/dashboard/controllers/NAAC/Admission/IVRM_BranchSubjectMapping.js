(function () {
    'use strict';
    angular
        .module('app')
        .controller('IVRM_BranchSubjectMappingController', IVRM_BranchSubjectMappingController)

    IVRM_BranchSubjectMappingController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http']
    function IVRM_BranchSubjectMappingController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http) {

        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;
        $scope.searchValue = "";
        $scope.sort = function (key) { //sorting column
            $scope.sortReverse = ($scope.sortKey == key) ? !$scope.sortReverse : $scope.sortReverse;//for reverse
            $scope.sortKey = key;
        }

        //================Page Load
        $scope.loaddata = function () {
            var pageid = 3;
            apiService.getURI("IVRM_BranchSubjectMapping/loaddata", pageid).then(function (promise) {
                $scope.cousrselist = promise.cousrselist;
                $scope.branchlist = promise.branchlist;
                $scope.subjectlist = promise.subjectlist;
                $scope.alldata = promise.alldata;

            });
        };
        //========================

        //============interacted form
        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        //=========================
        $scope.submitted = false;
        $scope.savedata = function () {
            if ($scope.myForm.$valid) {
                var data = {
                    "IMSBR_Id": $scope.imsbR_Id,
                    "ISMS_Id": $scope.ISMS_Id,
                    "AMB_Id": $scope.AMB_Id,
                    "AMCO_Id": $scope.AMCO_Id,
                }
                apiService.create("IVRM_BranchSubjectMapping/savedata", data).
                    then(function (promise) {
                        if (promise.returnval != null && promise.duplicate != null) {
                            if (promise.duplicate == false) {
                                if (promise.returnval == true) {
                                    if ($scope.imsbR_Id > 0) {
                                        swal('Record Updated Successfully!!!');
                                    }
                                    else {
                                        swal('Record Saved Successfully!!!');
                                    }
                                }
                                else {
                                    if (promise.returnval == false) {
                                        if ($scope.imsbR_Id > 0) {
                                            swal('Record Not Update Successfully!!!');
                                        }
                                        else {
                                            swal('Record Not Saved Successfully!!!');
                                        }
                                    }
                                }
                            }
                            else {
                                swal("Records Already Exist!");
                            }
                            $state.reload();
                        }
                        else {
                            swal("Kindly Contact Administrator!!!");
                        }
                    })
            }
            else {
                $scope.submitted = true;
            }
        };
        //=================================

        //==============Cancel
        $scope.clearid = function () {
            $state.reload();
        }

        //==================Get Branch
        $scope.get_branch = function () {            
            var data = {
                "AMCO_Id": $scope.AMCO_Id,
            }
            apiService.create("IVRM_BranchSubjectMapping/get_Branch", data).then(function (promise) {
                $scope.branchlist = promise.branchlist;
            });
        }
        //========================

        //============Edit Records
        $scope.editdata = function (user) {
            var data = {
                "IMSBR_Id": user,
            }
            apiService.create("IVRM_BranchSubjectMapping/editdata", data).then(function (promise) {
                if (promise.editlist.length > 0) {
                    $scope.imsbR_Id = promise.editlist[0].imsbR_Id;
                    $scope.AMCO_Id = promise.editlist[0].amcO_Id;
                    $scope.AMB_Id = promise.editlist[0].amB_Id;
                    $scope.ISMS_Id = promise.editlist[0].ismS_Id;
                }
            });
        }
        //==================================

        //==================Active/Deactive records
        $scope.deactiveY = function (newuser1, SweetAlertt) {
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            var mgs = "";
            if (newuser1.imsbR_ActiveFlg == false) {

                mgs = "Activate";

            }
            else {
                mgs = "Deactivate";
            }
            swal({
                title: "Are you sure?",
                text: "Do you want to  " + mgs + " the Record?",
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
                        apiService.create("IVRM_BranchSubjectMapping/deactiveY/", newuser1).
                            then(function (promise) {
                                if (promise.returnval == true) {

                                    swal("Record " + mgs + "d Successfully!!!");
                                    $state.reload();
                                }
                                else {
                                    swal("Record Not " + mgs + "d Successfully!!!");
                                    $state.reload();
                                }
                            })
                    }
                    else {
                        swal("Cancelled");
                    }
                })
        }
        //========================================



    }
})();