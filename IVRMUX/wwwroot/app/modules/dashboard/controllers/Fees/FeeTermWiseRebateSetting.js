
(function () {
    'use strict';
    angular
        .module('app')
        .controller('FeeTermWiseRebateSettingController', fee1)

    fee1.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$stateParams']
    function fee1($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $stateParams) {

        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }

        //  $scope.sortKey = "fmH_Id";   //set the sortKey to the param passed
        $scope.sortKey = "fmH_Order";   //set the sortKey to the param passed

        $scope.reverse = false; 
        $scope.gridrecord = false; 

        $scope.feeorder = false;
        $scope.feetext = true;
        $scope.searchValue = "";
        $scope.filterValue = function (obj) {
            if (obj.fmH_ActiveFlag == true) {
                $scope.test = "Active";
            } else if (obj.fmH_ActiveFlag == false) {
                $scope.test = "Deactive";
            }
            return angular.lowercase(obj.fmH_FeeName).indexOf(angular.lowercase($scope.searchValue)) >= 0 || JSON.stringify(obj.fmH_Order).indexOf($scope.searchValue) >= 0 || JSON.stringify(obj.fmH_SpecialFeeFlag).indexOf($scope.searchValue) >= 0 || JSON.stringify(obj.fmH_PDAFlag).indexOf($scope.searchValue) >= 0 || JSON.stringify(obj.fmH_RefundFlag).indexOf($scope.searchValue) >= 0 || angular.lowercase(obj.fmH_Flag).indexOf(angular.lowercase($scope.searchValue)) >= 0 || angular.lowercase($scope.test).indexOf(angular.lowercase($scope.searchValue)) >= 0;
        }

        $scope.savedisable = true;

        var pageid = $stateParams.pageId;
        var privlgs = JSON.parse(localStorage.getItem("privileges"));
        for (var i = 0; i < privlgs.length; i++) {
            if (privlgs[i].pageId == pageid) {
                $scope.disabled = true;
                if (privlgs[i].ivrmirP_AddFlag == true) {
                    $scope.save = true;
                    $scope.savebtn = true;

                }
                else {
                    $scope.save = false;
                    $scope.savebtn = false;
                }
                if (privlgs[i].ivrmirP_UpdateFlag == true) {
                    $scope.editflag = true;
                    $scope.editbtn = true;

                }
                else {
                    $scope.editflag = false;
                    $scope.editbtn = false;
                }
                if (privlgs[i].ivrmirP_DeleteFlag == true) {
                    $scope.deactiveflag = true;
                    $scope.deletebtn = true;
                }
                else {
                    $scope.deactiveflag = false;
                    $scope.deletebtn = false;
                }


            }
        }

        $scope.resetLists = function () {
            $scope.configA = {
                // Changed sorting within list
                onUpdate: function (evt) {
                    var itemEl = evt.item;  // dragged HTMLElement
                    // + indexes from onEnd
                }
            };
        };
        $scope.init = function () {

            $scope.resetLists();

        };
        $scope.init();


        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            var pageid = 2;
            apiService.getURI("FeeTermWiseRebateSetting/getalldetails", pageid).
                then(function (promise) {

                    $scope.arrlist6 = promise.academicdrp;
                    $scope.termname = promise.termlist;
                    $scope.totcountfirst = promise.groupHeadData.length;

                    $scope.students = promise.groupHeadData;

                    if ($scope.students.length > 0) {
                        $scope.gridrecord = true; 

                    }

                    $scope.students.data = promise.groupHeadData;
                    $scope.studentListOrder = promise.groupHeadData;
                })
            $scope.sort = function (keyname) {
                $scope.sortKey = keyname;   //set the sortKey to the param passed
                $scope.reverse = !$scope.reverse; //if true make it false and vice versa
            }

        }

        $scope.submitted = false;
        $scope.saveGroupdata = function () {

            if ($scope.myFormhead.$valid) {


                var data = {
                    "FMTRS_Id": $scope.FMTRS_Id,
                    "FMTRS_RebateApplicableDate": new Date($scope.FMTRS_RebateApplicableDate).toDateString(),
                    "FMTRS_RebateAmountPercentFlg": $scope.FMTRS_RebateAmountPercentFlg,
                    "FMTRS_RebateAmountPercentValue": $scope.FMTRS_RebateAmountPercentValue,
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "FMT_Id": $scope.FMT_Id


                }
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }
                apiService.create("FeeTermWiseRebateSetting/", data).
                    then(function (promise) {
                        if (promise.returnval === true) {
                            $scope.students = promise.groupHeadData;
                            $scope.loaddata();
                            if (promise.returnval === true) {
                                if (promise.message != null) {
                                    swal('Record Updated Successfully', 'success');
                                    $state.reload();
                                }
                                else {
                                    swal('Record Saved Successfully', 'success');
                                    $state.reload();
                                }
                                if ($scope.students.length > 0) {
                                    $scope.gridrecord = true;

                                }
                                

                            }
                            //swal('Record Saved/Updated Successfully');
                        }
                        else if (promise.returnval === false && promise.returnduplicatestatus === 'Duplicate') {
                            swal('Record Already Exist');
                        }
                        else {
                            if (promise.message != null) {
                                swal('Record Not Updated', 'success');
                            }
                            else {
                                swal('Record Not Saved', 'success');
                            }
                        }
                    })
                //$scope.cance();

            }
            else {
                $scope.submitted = true;
            }
        };
        $scope.interacted1 = function (field) {

            return $scope.submitted || field.$dirty;
        };


        $scope.cance = function () {
            $scope.FMT_Id = "",
                $scope.ASMAY_Id = "";
            $scope.FMTRS_RebateApplicableDate = "";
            $scope.FMTRS_RebateAmountPercentFlg = "";
            $scope.FMTRS_RebateAmountPercentValue = "";
     

        }
         $scope.editEmployee = {}
        $scope.deletedata = function (employee, SweetAlert) {
            $scope.editEmployee = employee.fmH_Id;
            var pageid = $scope.editEmployee;
            swal({
                title: "Are you sure?",
                text: "Do You Want To Delete Record?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, Delete it",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.DeleteURI("FeeTermWiseRebateSetting/deletepages", pageid).
                            then(function (promise) {
                                $scope.students = promise.groupHeadData;
                                if (promise.returnval === true) {
                                    swal('Record Deleted Successfully');
                                }
                                else if (promise.dupr === false && promise.returnval != true) {
                                    swal('Record Not Deleted', 'Fee Head Already Mapped Yearly Group');
                                }
                                else {
                                    swal('Record Not Deleted');
                                }
                                $scope.loaddata();
                            })
                        $scope.loaddata();
                    }
                    else {
                        swal("Record Deletion Cancelled");
                        $scope.loaddata();
                    }
                });
        }
        $scope.getorgvalue = function (employee) {
            $scope.editEmployee = employee.fmtrS_Id;
            var pageid = $scope.editEmployee;
            apiService.getURI("FeeTermWiseRebateSetting/getdetails", pageid).
                then(function (promise) {

                    $scope.FMTRS_RebateApplicableDate = new Date(promise.groupHeadData[0].fmtrS_RebateApplicableDate);
                    $scope.FMTRS_RebateAmountPercentFlg = promise.groupHeadData[0].fmtrS_RebateAmountPercentFlg;
                    $scope.FMTRS_RebateAmountPercentValue = promise.groupHeadData[0].fmtrS_RebateAmountPercentValue;
                    $scope.ASMAY_Id = promise.groupHeadData[0].asmaY_Id;
                    $scope.FMT_Id = promise.groupHeadData[0].fmT_Id;



                    $scope.FMTRS_Id = promise.groupHeadData[0].fmtrS_Id;
                })
        }

        $scope.deactive = function (groupHeadData, SweetAlert) {
            var mgs = "";
            var confirmmgs = "";
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
           
            swal({
                title: "Are you sure?",
                text: "Do You Want To Delete Record?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + mgs + " it",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {


                        apiService.create("FeeTermWiseRebateSetting/deactivate", groupHeadData).
                            then(function (promise) {
                            
                                if (promise.returnval == true) {
                                    swal("Record Deleted Successfully");
                                }
                                
                                else {
                                    swal("Record Not Deleted Successfully");
                                }

                                $state.reload();
                            })
                    }
                    else {
                        swal("Request Failed");
                    }

                    $state.reload();
                })
        }
    };
})();


