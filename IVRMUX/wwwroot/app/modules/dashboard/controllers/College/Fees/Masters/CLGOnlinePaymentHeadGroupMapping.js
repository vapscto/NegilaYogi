(function () {
    'use strict';
    angular
        .module('app')
        .controller('CLGOnlinePaymentHeadGroupMappingController', CLGOnlinePaymentHeadGroupMappingController)

    CLGOnlinePaymentHeadGroupMappingController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache']
    function CLGOnlinePaymentHeadGroupMappingController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {
        $scope.search = "";
        $scope.loaddetails = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = 5;
            var pageid = 2;
            apiService.getURI("CLGOnlinePaymentHeadGroupMapping/onlineMappingDetails", pageid).
                then(function (promise) {
                    
                    $scope.institution = promise.institutionList;
                    $scope.MI_Id = promise.institutionList[0].mI_Id;
                    $scope.courselist = promise.courselist;
                    $scope.submerchantId = promise.subMerchantIdList;
                    $scope.grouplist = promise.groupNameList;
                    $scope.headlist = promise.headNameList;

                    $scope.yearlst = promise.fillyear;


                    $scope.installmentlist = promise.installmentList;
                    $scope.termlist = promise.termList;
                    //if (promise.count > 0) {
                        $scope.feeonlinepaymentmappingList = promise.feeonlinepaymentmappingList;
                        $scope.totcountfirst = promise.feeonlinepaymentmappingList.length;
                        $scope.count = 1;
                    //}
                    //else {
                    //    swal("No Records Found");
                    //    $scope.count = 0;
                    //}

                })
            $scope.sort = function (keyname) {
                $scope.sortKey = keyname;   //set the sortKey to the param passed
                $scope.reverse = !$scope.reverse; //if true make it false and vice versa
            }
        };

        $scope.onselectacade = function () {
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id
            }

            apiService.create("CLGOnlinePaymentHeadGroupMapping/academicsel", data).
                then(function (promise) {

                    if (promise.courselist.length > 0) {
                        $scope.courselist = promise.courselist;
                    }
                    else {
                        swal("No Class Is Mapped To Selected Year");
                    }

                    if (promise.groupNameList.length > 0) {
                        $scope.grouplist = promise.groupNameList;
                    }
                    else {
                        swal("No Group Name Is Mapped To Selected Year");
                    }

                })

        }

        $scope.isOptionsRequired1 = function () {

            return !$scope.headlist.some(function (options) {
                return options.Selected1;
            });
        }
        $scope.selectgroup = function () {  
            var data = {
                "FMG_Id": $scope.FMG_Id,
                "ASMAY_Id": $scope.ASMAY_Id
            }

            apiService.create("CLGOnlinePaymentHeadGroupMapping/groupsel", data).
                then(function (promise) {
                    if (promise.headlistcount > 0) {
                        $scope.headlist = promise.headNameList;
                    }
                    else {
                        $scope.headlist = [];
                        swal("No Head Name Is Mapped To Selected Group Name");
                    }

                })
        }
        $scope.temparray = [];
        $scope.selecthead = function (headid, index) {

            $scope.temparray.length = 0;
            if ($scope.Id > 0) {
                if (headid[index].Selected1 == true) {
                    $scope.temparray.push(headid[index]);
                }
            }
            else {
                if ($scope.headlist != "" && $scope.headlist != null) {
                    if ($scope.headlist.length > 0) {
                        for (var j = 0; j < $scope.headlist.length; j++) {
                            if ($scope.headlist[j].Selected1 == true) {
                                $scope.temparray.push($scope.headlist[j]);
                            }
                        }
                    }
                }
            }


            var data = {
                "FMG_Id": $scope.FMG_Id,
                "selectedheadList": $scope.temparray,
                "ASMAY_Id": $scope.ASMAY_Id
            }
            apiService.create("CLGOnlinePaymentHeadGroupMapping/headsel", data).
                then(function (promise) {
                    if (promise.installmentlistcount > 0) {
                        $scope.installmentlist = promise.installmentList;
                    }
                    else {
                        swal("No Installment Is Mapped To Selected Group Name and Head Name");
                    }
                })
        }

        $scope.delete = function (det, SweetAlert) {
            $scope.deleteId = det.cfopM_Id;
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
                        apiService.DeleteURI("CLGOnlinePaymentHeadGroupMapping/delete/", $scope.deleteId).
                            then(function (promise) {
                                if (promise.returnval == "deleted") {
                                    swal('Record Deleted Successfully');
                                    $state.reload();
                                    return;
                                }
                                else if (promise.returnval == "deletefailed") {
                                    swal('Record Not Deleted');
                                    return;
                                }
                                else if (promise.returnval == "failed") {
                                    swal("Sorry.....You can not delete this record.Because it is already mapped");
                                }
                            })
                    }
                    else {
                        swal("Cancelled");
                    }
                });
        }
        $scope.edit = function (det) {
            $scope.Id = det.cfopM_Id;
            apiService.getURI("CLGOnlinePaymentHeadGroupMapping/edit", $scope.Id).
                then(function (promise) {

                    $scope.MI_Id = promise.editdata[0].mI_Id;
                    $scope.AMCO_Id = promise.editdata[0].AMCO_Id;

                    if ($scope.headlist != null && promise.editdata != null) {

                        for (var i = 0; i < $scope.headlist.length; i++) {
                            $scope.headlist[i].Selected1 = false;
                        }
                        for (var i = 0; i < $scope.headlist.length; i++) {
                            if ($scope.headlist[i].fmH_Id == promise.editdata[0].fmH_Id) {
                                $scope.headlist[i].Selected1 = true;
                            }
                        }
                    }
                    $scope.FPGD_Id = promise.editdata[0].fpgd_id;
                    $scope.FMG_Id = promise.editdata[0].fmg_id;
                    $scope.FTI_Id = promise.editdata[0].fti_id;
                    $scope.FMT_Id = promise.editdata[0].fmt_id;
                    $("input:checkbox").on('click', function () {
                        // in the handler, 'this' refers to the box clicked on
                        var $box = $(this);
                        if ($box.is(":checked")) {
                            // the name of the box is retrieved using the .attr() method
                            // as it is assumed and expected to be immutable
                            var group = "input:checkbox[name='" + $box.attr("name") + "']";
                            // the checked state of the group/box on the other hand will change
                            // and the current value is retrieved using .prop() method
                            $(group).prop("checked", false);
                            $box.prop("checked", true);
                        } else {
                            $box.prop("checked", false);
                        }
                    });
                })
        }
        $scope.submitted = false;
        $scope.selectedheadList = [];
        $scope.save = function () {
            if ($scope.myForm.$valid) {
                $scope.selectedheadList.length = 0;
                if ($scope.Id > 0) {
                    $scope.selectedheadList = $scope.temparray;
                }
                else {
                    if ($scope.headlist != "" && $scope.headlist != null) {
                        if ($scope.headlist.length > 0) {
                            for (var i = 0; i < $scope.headlist.length; i++) {
                                if ($scope.headlist[i].Selected1 == true) {
                                    $scope.selectedheadList.push($scope.headlist[i]);
                                }
                            }
                        }
                    }
                }
                var data = {
                    "CFOPM_Id": $scope.Id,
                    "MI_Id": $scope.MI_Id,
                    "AMCO_Id": $scope.AMCO_Id,
                    "selectedheadList": $scope.selectedheadList,
                    "FPGD_Id": $scope.FPGD_Id,
                    "FMG_Id": $scope.FMG_Id,
                    "fti_id": $scope.FTI_Id,
                    "FMT_Id": $scope.FMT_Id,
                }
                apiService.create("CLGOnlinePaymentHeadGroupMapping/", data)

                    .then(function (promise) {
                        if (promise.isduplicate == "duplicate" && promise.returnval == "saved") {
                            alert('Some Record already exist');
                        }
                        else if (promise.isduplicate == "duplicate") {
                            swal('Record already exist');
                        }

                        if (promise.returnval == "saved") {
                            swal('Record Saved Successfully');
                            $state.reload();
                        }
                        else if (promise.returnval == "savingfailed") {
                            swal('Record Saving Failed');
                        }
                        else if (promise.returnval == "updated") {
                            swal('Record Updated Successfully');
                            $state.reload();
                        }
                        else if (promise.returnval == "updatefailed") {
                            swal('Record Updation Failed');
                        }
                        else if (promise.returnval == "failed") {
                            swal('Operation Failed');
                        }
                    })
            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.clearid = function () {
            $state.reload();

        }
        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };
    }
})();
