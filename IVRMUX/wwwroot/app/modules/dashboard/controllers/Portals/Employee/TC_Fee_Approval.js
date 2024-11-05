(function () {
    'use strict';
    angular
        .module('app')
        .controller('TC_FEE_ApprController', TC_FEE_ApprController)

    TC_FEE_ApprController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache']
    function TC_FEE_ApprController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {

        // $scope.sortKey = 'ATCCTAPP_Id';
        $scope.sortReverse = true;

        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }



        $scope.usrname = localStorage.getItem('username');
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }

        $scope.imgname = logopath;

        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));



        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey === key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        }
        $scope.searchValue = '';
        $scope.filterValue = function (obj) {

            return (angular.lowercase(obj.mob)).indexOf(angular.lowercase($scope.searchValue)) >= 0
        }

        //get data
        $scope.loaddata = function () {

            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            var pageid = 2;
            apiService.getURI("EmployeeLeaveApply/getdata_FEE", pageid)
                .then(function (promise) {
                    $scope.student_dd = promise.student_dd;
                    $scope.tc_fee_list = promise.tc_fee_list;
                    $scope.presentCountgrid = $scope.tc_fee_list.length;
                    $scope.editopn = false;
                })
        }
        //============
        $scope.getstudetails = function (qq) {
            var data = {
                "AMST_Id": qq
            }
            apiService.create("EmployeeLeaveApply/getstudetails_FEE", data).then(function (promise) {
                $scope.libstudetails = promise.libstudetails;
            });
        }
        //===========
        $scope.toggleAllstd = function () {
            var chck = $scope.stdall
            angular.forEach($scope.libstudetails, function (qq) {
                if (qq.Balance==0) {
                    qq.stdselected = chck;
                }
               
            })
        }

        $scope.isRequiredlocation = function () {
            if ($scope.libstudetails.length>0) {
                return !$scope.libstudetails.some(function (options) {
                    return options.stdselected;
                });
            }
        };

        $scope.optionToggledstd = function () {
            $scope.stdall = $scope.libstudetails.every(function (itm) {
                return itm.stdselected;
            });

           
        };
        //=====================
        //save
        $scope.submitted = false;
        $scope.savedata = function () {
            $scope.feeblcarray1 = [];
            angular.forEach($scope.libstudetails, function (qq) {
                if (qq.stdselected == true) {
                    $scope.feeblcarray1.push({ FMG_Id: qq.FMG_Id})
                }
            })
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var data = {
                    "AMST_Id": $scope.AMST_Id,
                    "ATCFAPP_ApprovedDate": $scope.ATCFAPP_ApprovedDate,
                    "ATCFAPP_ApprovalFlg":true,
                    "ATCFAPP_Remarks": $scope.ATCFAPP_Remarks,
                    "ATCFAPP_Id": $scope.Id,
                    "feeblcarray": $scope.feeblcarray1
                }
                apiService.create("EmployeeLeaveApply/SaveEdit_FEE", data).
                    then(function (promise) {


                        if (promise.returndata === "Update") {
                            swal('Record Updated Successfully.');
                            $scope.AMST_Id = "";
                            $scope.studentname = "";
                            $scope.ATCFAPP_ApprovedDate = "";
                            $scope.ATCFAPP_ApprovalFlg = false;
                            $scope.ATCFAPP_Remarks = "";
                            $scope.libstudetails = [];
                            $scope.loaddata();
                            return;
                        }
                        else if (promise.returndata === "Add") {
                            swal('Record Saved Successfully.');
                            $scope.AMST_Id = "";
                            $scope.studentname = "";
                            $scope.ATCFAPP_ApprovedDate = "";
                            $scope.ATCFAPP_ApprovalFlg = false;
                            $scope.ATCFAPP_Remarks = "";
                            $scope.libstudetails = [];
                            $scope.loaddata();
                            return;
                        }
                        else if (promise.returndata === "Error") {
                            swal('Record Not Saved/Updated successfully.');
                            $scope.AMST_Id = "";
                            $scope.studentname = "";
                            $scope.ATCFAPP_ApprovedDate = "";
                            $scope.ATCFAPP_ApprovalFlg = false;
                            $scope.ATCFAPP_Remarks = "";
                            $scope.libstudetails = [];
                            $scope.loaddata();
                            return;
                        }

                    })

            }
            else {

                $scope.submitted = true;
            }

        };


        $scope.submitted1 = false;
        $scope.save_pending = function () {
            $scope.feeblcarray1 = [];
            angular.forEach($scope.libstudetails_new, function (qq) {
                if (qq.stdselected == true) {
                    $scope.feeblcarray1.push({ FMG_Id: qq.FMG_Id })
                }
            })
            $scope.submitted1 = true;
            if ($scope.myForm1.$valid) {
                var data = {
                    "AMST_Id": $scope.AMST_Id_new,
                    "ATCFAPP_ApprovedDate": $scope.ATCFAPP_ApprovedDate1,
                    "ATCFAPP_ApprovalFlg": true,
                    "ATCFAPP_Remarks": $scope.ATCFAPP_Remarks1,
                    "ATCFAPP_Id": $scope.Id,
                    "feeblcarray": $scope.feeblcarray1
                }
                apiService.create("EmployeeLeaveApply/SaveEdit_FEE", data).
                    then(function (promise) {


                        if (promise.returndata === "Update") {
                            swal('Record Updated Successfully.');
                            $scope.AMST_Id = "";
                            $scope.studentname = "";
                            $scope.ATCFAPP_ApprovedDate1 = "";
                            $scope.ATCFAPP_ApprovalFlg = false;
                            $scope.ATCFAPP_Remarks1 = "";
                            $scope.libstudetails = [];
                            $scope.loaddata();
                            angular.element('#myModalfee_new').modal('hide');
                            return;
                        }
                        else if (promise.returndata === "Add") {
                            swal('Record Saved Successfully.');
                            $scope.AMST_Id = "";
                            $scope.studentname = "";
                            $scope.ATCFAPP_ApprovedDate1 = "";
                            $scope.ATCFAPP_ApprovalFlg = false;
                            $scope.ATCFAPP_Remarks1 = "";
                            $scope.libstudetails = [];
                            $scope.loaddata();
                            angular.element('#myModalfee_new').modal('hide');
                            return;
                        }
                        else if (promise.returndata === "Error") {
                            swal('Record Not Saved/Updated successfully.');
                            $scope.AMST_Id = "";
                            $scope.studentname = "";
                            $scope.ATCFAPP_ApprovedDate1 = "";
                            $scope.ATCFAPP_ApprovalFlg = false;
                            $scope.ATCFAPP_Remarks1 = "";
                            $scope.libstudetails = [];
                            $scope.loaddata();
                            angular.element('#myModalfee_new').modal('hide');
                            return;
                        }

                    })

            }
            else {

                $scope.submitted1 = true;
            }

        };

        //edit
        $scope.edibl = {}
        $scope.edit = function (bil) {
            $scope.edibl = bil.ATCLIBAPP_Id;
            var pageid = $scope.edibl;
            apiService.getURI("EmployeeLeaveApply/details_FEE", pageid).
                then(function (promise) {
                    $scope.editopn = true;
                    $scope.Id = promise.tc_fee_details[0].ATCFAPP_Id;
                    $scope.AMST_Id = promise.tc_fee_details[0].AMST_Id;
                    $scope.studentname = promise.tc_fee_details[0].studentname;
                    $scope.ATCFAPP_ApprovedDate = new Date(promise.tc_fee_details[0].ATCFAPP_ApprovedDate);
                    $scope.ATCFAPP_ActiveFlg = promise.tc_fee_details[0].ATCFAPP_ActiveFlg;
                    $scope.ATCFAPP_Remarks = promise.tc_fee_details[0].ATCFAPP_Remarks;
                   

                })
        };
        //=================
        $scope.view = function (ww) {
            var data = {
                "AMST_Id":ww.AMST_Id
            }
            apiService.create("EmployeeLeaveApply/feeheaddetails_FEE", data).then(function (promise) {
                $scope.feehead_details = promise.feehead_details;
            })
        }
        $scope.notapproval = function (ww) {
            var data = {
                "AMST_Id":ww.AMST_Id
            }
            apiService.create("EmployeeLeaveApply/feenot_approval_FEE", data).then(function (promise) {
                $scope.libstudetails_new = promise.libstudetails;
                $scope.studentname_new = ww.studentname; 
                $scope.AMST_Id_new = ww.AMST_Id; 
            })
        }


        $scope.cancel = function () {
            $scope.AMST_Id = "";
            $scope.studentname = "";
            $scope.ATCFAPP_ApprovedDate = "";
            $scope.ATCFAPP_ApprovedDate1 = "";
            $scope.ATCFAPP_ApprovalFlg = false;
            $scope.ATCFAPP_Remarks = "";
            $scope.ATCFAPP_Remarks1 = "";
            $scope.editopn = false;
            $scope.libstudetails = [];

        }

        //deactive
        $scope.deactive = function (flr, SweetAlertt) {

            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            };
            var mgs = "";
            if (flr.ATCLIBAPP_ActiveFlg === false) {
                mgs = "Activate";
            } else {
                mgs = "Deactivate";
            }
            swal({
                title: "Are you sure?",
                text: "Do you want to  " + mgs + " the Academic Year?",
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
                        apiService.create("EmployeeLeaveApply/deactivate_FEE", flr).
                            then(function (promise) {


                                if (promise.returndata === 'false') {
                                    swal('TC Library Approval Deactivated Successfully.');
                                }

                                else if (promise.returndata === 'true') {
                                    swal('TC Library Approval Activated Successfully.');
                                }

                                else if (promise.returndata === 'Error') {
                                    swal('Operation Failed!!!');
                                }


                                $scope.loaddata();

                            });
                    } else {
                        swal("Cancelled");
                        $scope.loaddata();
                    }

                });
        };


        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };
        
        $scope.interacted1 = function (field) {
            return $scope.submitted1 || field.$dirty;
        };


    }

})();



