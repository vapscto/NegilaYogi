(function () {
    'use strict';
    angular
.module('app')
        .controller('OnlinePaymentHeadGroupMappingController', OnlinePaymentHeadGroupMappingController)

    OnlinePaymentHeadGroupMappingController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$stateParams']
    function OnlinePaymentHeadGroupMappingController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache,$stateParams) {
        $scope.search = "";
        $scope.savedisable = true;
        $scope.checkallhrd1 = "";
        var pageid = $stateParams.pageId;
        var privlgs = JSON.parse(localStorage.getItem("privileges"));
        for (var i = 0; i < privlgs.length; i++) {
            if (privlgs[i].pageId == pageid) {
                $scope.disabled = true;
                if (privlgs[i].ivrmirP_AddFlag == true) {
                    $scope.saveflg = true;
                    $scope.savebtn = true;

                }
                else {
                    $scope.saveflg = false;
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

        //Added By Praveen Gouda
        $scope.hrdallcheck1 = function () {
            var toggleStatus1 = $scope.checkallhrd1;
            angular.forEach($scope.headlist, function (itm) {
                itm.Selected1 = toggleStatus1;
            });
        }

        $scope.loaddetails = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = 5;
            var pageid = 2;
            apiService.getURI("OnlinePaymentHeadGroupMapping/onlineMappingDetails", pageid).
            then(function (promise) {
                $scope.institution = promise.institutionList;
                $scope.MI_Id = promise.institutionList[0].mI_Id;
                $scope.classlist = promise.classList;
                $scope.submerchantId = promise.subMerchantIdList;
                $scope.grouplist = promise.groupNameList;
                $scope.headlist = promise.headNameList;

                $scope.yearlst = promise.fillyear;

                
                $scope.installmentlist = promise.installmentList;
                $scope.termlist = promise.termList;
                if (promise.count > 0) {
                    $scope.feeonlinepaymentmappingList = promise.feeonlinepaymentmappingList;
                    $scope.totcountfirst = promise.feeonlinepaymentmappingList.length;
                    $scope.count = 1;
                }
                else {
                    swal("No Records Found");
                    $scope.count = 0;
                }

            })
            $scope.sort = function (keyname) {
                $scope.sortKey = keyname;   //set the sortKey to the param passed
                $scope.reverse = !$scope.reverse; //if true make it false and vice versa
            }
        };

        $scope.onselectacade=function()
        {
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id
            }

            apiService.create("OnlinePaymentHeadGroupMapping/academicsel", data).
           then(function (promise) {

               if (promise.classlist.length > 0) {
                   $scope.classlist = promise.classList;
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
                "ASMAY_Id":$scope.ASMAY_Id
            }

            apiService.create("OnlinePaymentHeadGroupMapping/groupsel", data).
            then(function (promise) {
                if (promise.headlistcount > 0) {
                    $scope.headlist = promise.headNameList;
                }
                else {
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
            apiService.create("OnlinePaymentHeadGroupMapping/headsel", data).
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
            $scope.deleteId = det.fopM_Id;
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
                    apiService.DeleteURI("OnlinePaymentHeadGroupMapping/delete/", $scope.deleteId).
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
            $scope.Id = det.fopM_Id;
            apiService.getURI("OnlinePaymentHeadGroupMapping/edit", $scope.Id).
            then(function (promise) {
                
                $scope.MI_Id = promise.editdata[0].mI_Id;
                $scope.ASMCL_Id = promise.editdata[0].asmcL_Id;

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
                //if ($scope.Id > 0) {
                //    $scope.selectedheadList = $scope.temparray;
                //}
                //else {
                    if ($scope.headlist != "" && $scope.headlist != null) {
                        if ($scope.headlist.length > 0) {
                            for (var i = 0; i < $scope.headlist.length; i++) {
                                if ($scope.headlist[i].Selected1 == true) {
                                    $scope.selectedheadList.push($scope.headlist[i]);
                                }
                            }
                        }
                    }
               // }
                var data = {
                    "FOPM_Id": $scope.Id,
                    "MI_Id": $scope.MI_Id,
                    "ASMCL_Id": $scope.ASMCL_Id,
                    "selectedheadList": $scope.selectedheadList,
                    "FPGD_Id": $scope.FPGD_Id,
                    "fmg_id": $scope.FMG_Id,
                    "fti_id": $scope.FTI_Id,
                    "FMT_Id": $scope.FMT_Id,
                }
                apiService.create("OnlinePaymentHeadGroupMapping/", data)

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
