(function () {
    'use strict';
    angular
        .module('app')
        .controller('PFTransactionController', PFTransactionController)

    PFTransactionController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', '$filter', 'superCache']
    function PFTransactionController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, $filter, superCache) {

        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;
        $scope.Editflag = false;

        $scope.HRME_PFDate = new Date();
        $scope.obj = {};

        // Get form Details at onload 
        $scope.onLoadGetData = function () {
            var pageid = 2;
            apiService.getURI("PFTransaction/getalldetails", pageid).then(function (promise) {

                if (promise.leaveyeardropdown !== null && promise.leaveyeardropdown.length > 0) {
                    $scope.leaveyeardropdown = promise.leaveyeardropdown;
                }

                if (promise.monthdropdown !== null && promise.monthdropdown.length > 0) {
                    $scope.monthdropdown = promise.monthdropdown;
                }
                if (promise.employeedropdown !== null && promise.employeedropdown.length > 0) {
                    $scope.employeelist = promise.employeedropdown;
                }
                if (promise.getpfgriddata !== null && promise.getpfgriddata.length > 0) {
                    $scope.vpfgriddata = promise.getpfgriddata;
                }

                if (promise.pfreport !== null && promise.pfreport.length > 0) {
                    $scope.pfgriddata = promise.pfreport;
                }


            });
        };

        $scope.pfreport = [];
        $scope.EmployeeDis = false;
        //Search employee
        $scope.submitted = false;
        $scope.SearchEmployee = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                $scope.institution = {};
                $scope.pfreport = [];
                $scope.CurrentInstuteAddress = "";
                $scope.month = "";
                $scope.year = "";



                var data = {
                    "HRES_Year": $scope.Employee.hreS_Year,
                    "HRES_Month": $scope.Employee.hreS_Month,
                    groupTypeIdList: groupTypeselected,
                };
                apiService.create("PFTransaction/getEmployeedetailsBySelection", data).
                    then(function (promise) {
                        $scope.EmployeeDis = true;
                        if (promise.pfreport !== null && promise.pfreport.length > 0) {
                            $scope.pfreport = promise.pfreport;
                        }

                        if (promise.hreS_Month != "") {
                            $scope.month = promise.hreS_Month;
                        }

                        if (promise.hreS_Year != "") {
                            $scope.year = promise.hreS_Year;
                        }

                        //Institution Details
                        if (promise.institutionDetails !== null && promise.institutionDetails.length > 0) {

                            $scope.institution = promise.institutionDetails[0];
                            var instuteAddress = "";
                            if ($scope.institution.mI_Address1 != null && $scope.institution.mI_Address1 != "") {
                                instuteAddress = $scope.institution.mI_Address1;
                            }

                            if ($scope.institution.mI_Address2 != null && $scope.institution.mI_Address2 != "") {
                                instuteAddress = instuteAddress + ',' + $scope.institution.mI_Address2;
                            }

                            if ($scope.institution.mI_Address3 != null && $scope.institution.mI_Address3 != "") {
                                instuteAddress = instuteAddress + ',' + $scope.institution.mI_Address3;
                            }

                            $scope.CurrentInstuteAddress = instuteAddress;
                        }
                    });
            }
        };


        $scope.editdata = function (item) {
            var ids = 0;
            if ($scope.PFVPFflag == 'PF') {
                ids = item.HREPFST_Id
            }
            else {
                ids = item.HREVPFST_Id
            }


            var data = {
                "TransactionID": ids,
                "Flag": $scope.PFVPFflag
            };

            apiService.create("PFTransaction/editdata", data).
                then(function (promise) {
                    if (promise.payrollStandard !== null && promise.payrollStandard.length > 0) {

                        $scope.Editflag = true;
                        $scope.payrollStandard = promise.payrollStandard;
                        $scope.HRME_EmployeeFirstName = promise.payrollStandard[0].HRME_EmployeeFirstName;
                        //$scope.IMFY_Id = promise.payrollStandard[0].IMFY_Id;
                        //$scope.HRME_Id = promise.payrollStandard[0].HRME_Id;
                        //$scope.Remark = promise.payrollStandard[0].remarks;
                        //$scope.HRME_PFDate = promise.payrollStandard[0].pfdate;
                        ////$scope.PFVPFflag = promise.PayrollStandard[0].IVRM_Month_Name;
                        //$scope.DepositWithdrow = promise.payrollStandard[0].IVRM_Month_Name;
                        //$scope.HeadType = promise.payrollStandard[0].flag;
                        //$scope.obj.Amount = promise.payrollStandard[0].HREVPFW_Amount;
                        //$scope.obj.ownamount = promise.payrollStandard[0].IVRM_Month_Name;
                        //$scope.obj.schoolamount = promise.payrollStandard[0].IVRM_Month_Name;
                    }
                });
        };


        $scope.changepftypeui = function (groupTypeselectedAll) {
            $scope.Editflag = false;
        };


        $scope.GetEmployeeBygroupTypeAll = function (groupTypeselectedAll) {
            if ($scope.EmployeeDis) {
                $scope.EmployeeDis = false;
            }
            var toggleStatus = $scope.groupTypeselectedAll;

            angular.forEach($scope.groupTypedropdown, function (itm) {
                itm.selected = toggleStatus;
            });

            angular.forEach($scope.designationdropdown, function (itm22) {
                itm22.selected = toggleStatus;
                $scope.designationselectedAll = toggleStatus;
            });

            angular.forEach($scope.departmentdropdown, function (itm232) {
                itm232.selected = toggleStatus;
                $scope.departmentselectedAll = toggleStatus;
            });

            $scope.get_depts();
        };

        //single
        $scope.GetEmployeeBygroupType = function (groupType) {
            $scope.departmentdropdown = [];
            $scope.designationdropdown = [];
            $scope.designationselectedAll = false;
            $scope.departmentselectedAll = false;

            if ($scope.EmployeeDis) {
                $scope.EmployeeDis = false;
            }

            $scope.groupTypeselectedAll = $scope.groupTypedropdown.every(function (itm) {
                return itm.selected;
            });

            $scope.get_depts();
        };

        $scope.get_depts = function () {
            var ids = [];
            angular.forEach($scope.groupTypedropdown, function (grp_t) {
                if (grp_t.selected) {
                    ids.push(grp_t.hrmgT_Id);
                }
            });

            var data = {
                hrmgT_IdList: ids
            };

            apiService.create("CumulativeSalaryReport/get_depts", data).
                then(function (promise) {
                    if (promise.departmentdropdown !== null && promise.departmentdropdown.length > 0) {
                        $scope.departmentdropdown = promise.departmentdropdown;
                        $scope.departmentselectedAll = true;
                        $scope.GetEmployeeByDepartmentAll($scope.departmentselectedAll);
                    }
                });
        };



        //By Department
        $scope.GetEmployeeByDepartmentAll = function (departmentselectedAll) {
            if ($scope.EmployeeDis) {
                $scope.EmployeeDis = false;
            }

            var toggleStatus = $scope.departmentselectedAll;
            angular.forEach($scope.departmentdropdown, function (itm) {
                itm.selected = toggleStatus;
            });
            angular.forEach($scope.designationdropdown, function (itm1) {
                itm1.selected = toggleStatus;
                $scope.designationselectedAll = toggleStatus;

            });

            $scope.get_desig();

        };

        //$scope.daysInMonth =function (month, year) {
        //    return new Date(year, month, 0).getDate();
        //}

        //// July


        //$scope.datevalidation = function (month, ) {


        //    daysInMonth(7, 2009);
        //}




        //By Department Single
        $scope.GetEmployeeByDepartment = function (department) {
            $scope.designationdropdown = [];
            $scope.designationselectedAll = false;
            if ($scope.EmployeeDis) {
                $scope.EmployeeDis = false;
            }
            $scope.departmentselectedAll = $scope.departmentdropdown.every(function (itm) {

                return itm.selected;
            });
            $scope.get_desig();
        };

        $scope.pfreport = [];
        $scope.EmployeeDis = false;
        //Search employee
        $scope.submitted = false;
        $scope.SavePFData = function () {

            var amount = 0;
            if ($scope.PFVPFflag == 'VPF') {
                amount = $scope.obj.Amount;
            }
            else {
                amount = $scope.obj.ownamount;
            }


            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                $scope.institution = {};
                $scope.pfreport = [];
                $scope.CurrentInstuteAddress = "";
                $scope.month = "";
                $scope.year = "";
                var data = {
                    "IMFY_Id": $scope.IMFY_Id,
                    "IVRM_Month_Id": $scope.ivrM_Month_Id,
                    "HRME_Id": $scope.HRME_Id.hrmE_Id,
                    "HeadType": $scope.HeadType,
                    "PFVPFflag": $scope.PFVPFflag,
                    "TransAmount": amount,
                    "Schoolamount": $scope.obj.schoolamount,
                    "Remark": $scope.Remark,
                    "DepositWithdrow": $scope.DepositWithdrow,
                    "HRME_PFDate": new Date($scope.HRME_PFDate).toDateString(),
                };
                apiService.create("PFTransaction/SavePFData", data).then(function (promise) {

                    if (promise.retrunMsg == "Add") {
                        swal("Record Saved Succussfully");
                        $state.reload();
                    }
                    else if (promise.retrunMsg == "notAdded") {
                        swal("Record Not Saved Succussfully");
                    }
                    else {
                        swal("Kindly Contect To Administrator");
                    }




                });
            }
        };



        $scope.submitted = false;
        $scope.PFBlurcalculation = function (newuser) {




            $scope.submitted = true;

            $scope.institution = {};
            var data = {};
            $scope.pfreport = [];
            $scope.pfTranaction = [];
            $scope.vpfTranaction = [];
            $scope.payrollStandard = [];
            $scope.CurrentInstuteAddress = "";
            $scope.month = "";
            $scope.year = "";


            if ($scope.PFVPFflag == 'VPF') {

                $scope.vpfTranaction.push({
                    HREVPFST_Id: newuser.HREVPFST_Id,
                    HREVPFST_VOBAmount: newuser.HREVPFST_VOBAmount,
                    HREVPFST_Contribution: newuser.HREVPFST_Contribution,
                    HREVPFST_Intersest: newuser.HREVPFST_Intersest,
                    HREVPFST_TransferAmount: newuser.HREVPFST_TransferAmount,
                    HREVPFST_WithdrawnAmount: newuser.HREVPFST_WithdrawnAmount,
                    HREVPFST_SettledAmount: newuser.HREVPFST_SettledAmount,
                    HREVPFST_DepositAdjustmentAmount: newuser.HREVPFST_DepositAdjustmentAmount,
                    HREVPFST_WithsrawAdjustmentAmount: newuser.HREVPFST_WithsrawAdjustmentAmount,
                    HREVPFST_ClosingBalance: newuser.HREVPFST_ClosingBalance
                });

                data = {
                    vpfTranaction: $scope.vpfTranaction,
                    "PFVPFflag": $scope.PFVPFflag,
                    "HRME_PFDate": new Date($scope.HRME_PFDate).toDateString(),
                };
            }
            else {

                $scope.pfTranaction.push({
                    HREPFST_Id: newuser.HREPFST_Id,
                    HREPFST_InstituteClosingBalance: newuser.HREPFST_InstituteClosingBalance,
                    HREPFST_InstituteDepositAdjustmentAmount: newuser.HREPFST_InstituteDepositAdjustmentAmount,
                    HREPFST_InstituteInterest: newuser.HREPFST_InstituteInterest,
                    HREPFST_InstituteLSettlementAmount: newuser.HREPFST_InstituteLSettlementAmount,
                    HREPFST_InstituteTransferAmount: newuser.HREPFST_InstituteTransferAmount,
                    HREPFST_InstituteWithdrawAdjustmentAmount: newuser.HREPFST_InstituteWithdrawAdjustmentAmount,
                    HREPFST_InstituteWithdrawnAmount: newuser.HREPFST_InstituteWithdrawnAmount,
                    HREPFST_IntstituteContribution: newuser.HREPFST_IntstituteContribution,
                    HREPFST_OBInstituteAmount: newuser.HREPFST_OBInstituteAmount,
                    HREPFST_OBOwnAmount: newuser.HREPFST_OBOwnAmount,
                    HREPFST_OwnClosingBalance: newuser.HREPFST_OwnClosingBalance,
                    HREPFST_OwnContribution: newuser.HREPFST_OwnContribution,
                    HREPFST_OwnDepositAdjustmentAmount: newuser.HREPFST_OwnDepositAdjustmentAmount,
                    HREPFST_OwnInterest: newuser.HREPFST_OwnInterest,
                    HREPFST_OwnSettlementAmount: newuser.HREPFST_OwnSettlementAmount,
                    HREPFST_OwnTransferAmount: newuser.HREPFST_OwnTransferAmount,
                    HREPFST_OwnWithdrawAdjustmentAmount: newuser.HREPFST_OwnWithdrawAdjustmentAmount,
                    HREPFST_OwnWithdrwanAmount: newuser.HREPFST_OwnWithdrwanAmount
                });

                data = {
                    pfTranaction: $scope.pfTranaction,
                    "PFVPFflag": $scope.PFVPFflag,
                    "HRME_PFDate": new Date($scope.HRME_PFDate).toDateString(),
                };

            }
            apiService.create("PFTransaction/PFBlurcalculation", data).then(function (promise) {

                if (promise.payrollStandard.length > 0) {
                    $scope.payrollStandard = promise.payrollStandard;
                }
            });

        };






        $scope.submitted = false;
        $scope.EditSave = function (newuser) {

            $scope.submitted = true;
            var data = {};
            $scope.pfreport = [];
            $scope.pfTranaction = [];
            $scope.vpfTranaction = [];
            //$scope.payrollStandard = [];

            if ($scope.PFVPFflag == 'VPF') {


                angular.forEach($scope.payrollStandard, function (newuser) {
                    $scope.vpfTranaction.push({
                        HREVPFST_Id: newuser.HREVPFST_Id,
                        HREVPFST_VOBAmount: newuser.HREVPFST_VOBAmount,
                        HREVPFST_Contribution: newuser.HREVPFST_Contribution,
                        HREVPFST_Intersest: newuser.HREVPFST_Intersest,
                        HREVPFST_TransferAmount: newuser.HREVPFST_TransferAmount,
                        HREVPFST_WithdrawnAmount: newuser.HREVPFST_WithdrawnAmount,
                        HREVPFST_SettledAmount: newuser.HREVPFST_SettledAmount,
                        HREVPFST_DepositAdjustmentAmount: newuser.HREVPFST_DepositAdjustmentAmount,
                        HREVPFST_WithsrawAdjustmentAmount: newuser.HREVPFST_WithsrawAdjustmentAmount,
                        HREVPFST_ClosingBalance: newuser.HREVPFST_ClosingBalance
                    });
                });



                data = {
                    vpfTranaction: $scope.vpfTranaction,
                    "PFVPFflag": $scope.PFVPFflag,
                };
            }
            else {
                angular.forEach($scope.payrollStandard, function (newuser) {
                    $scope.pfTranaction.push({
                        HREPFST_Id: newuser.HREPFST_Id,
                        HREPFST_InstituteClosingBalance: newuser.HREPFST_InstituteClosingBalance,
                        HREPFST_InstituteDepositAdjustmentAmount: newuser.HREPFST_InstituteDepositAdjustmentAmount,
                        HREPFST_InstituteInterest: newuser.HREPFST_InstituteInterest,
                        HREPFST_InstituteLSettlementAmount: newuser.HREPFST_InstituteLSettlementAmount,
                        HREPFST_InstituteTransferAmount: newuser.HREPFST_InstituteTransferAmount,
                        HREPFST_InstituteWithdrawAdjustmentAmount: newuser.HREPFST_InstituteWithdrawAdjustmentAmount,
                        HREPFST_InstituteWithdrawnAmount: newuser.HREPFST_InstituteWithdrawnAmount,
                        HREPFST_IntstituteContribution: newuser.HREPFST_IntstituteContribution,
                        HREPFST_OBInstituteAmount: newuser.HREPFST_OBInstituteAmount,
                        HREPFST_OBOwnAmount: newuser.HREPFST_OBOwnAmount,
                        HREPFST_OwnClosingBalance: newuser.HREPFST_OwnClosingBalance,
                        HREPFST_OwnContribution: newuser.HREPFST_OwnContribution,
                        HREPFST_OwnDepositAdjustmentAmount: newuser.HREPFST_OwnDepositAdjustmentAmount,
                        HREPFST_OwnInterest: newuser.HREPFST_OwnInterest,
                        HREPFST_OwnSettlementAmount: newuser.HREPFST_OwnSettlementAmount,
                        HREPFST_OwnTransferAmount: newuser.HREPFST_OwnTransferAmount,
                        HREPFST_OwnWithdrawAdjustmentAmount: newuser.HREPFST_OwnWithdrawAdjustmentAmount,
                        HREPFST_OwnWithdrwanAmount: newuser.HREPFST_OwnWithdrwanAmount
                    });
                });
                data = {
                    pfTranaction: $scope.pfTranaction,
                    "PFVPFflag": $scope.PFVPFflag,
                };

            }


            var dystring = "Update";
            swal({
                title: "Are you sure?",
                text: "Do You Want To " + dystring + " Record?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + dystring + " it",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {


                        apiService.create("PFTransaction/EditSave", data).then(function (promise) {

                            if (promise.retrunMsg == "Add" && promise.retrunMsg != null) {
                                swal("Record Updated Succussfully")
                                $scope.Editflag = false;
                            }
                            else {
                                swal("Record Not Updated Succussfully")
                            }
                        });

                    }
                    else {
                        swal("Record " + dystring + " Cancelled!!!");
                    }
                });

        };








        $scope.FinalVerifypopup = function () {
            $('#finalrunpopup').modal('show');

        };

        $scope.isOptionsRequired3 = function () {
            return !$scope.employeelist.some(function (options) {
                return options.selected;
            });
        }


        $scope.all_check_empl = function () {
            var checkStatus = $scope.empl;
            var count = 0;
            angular.forEach($scope.employeelist, function (itm) {
                itm.selected = checkStatus;
                if (itm.selected == true) {
                    count += 1;
                }
                else {
                    count = 0;
                }
            });
        }



        $scope.employeeDetails = [];
        $scope.EmployeeDis = false;
        //Search employee
        $scope.submitted = false;
        $scope.finalverify = function () {


            var dystring = "Update";
            swal({
                title: "Are you sure?",
                text: "Do You Want To " + dystring + " Record? \n Note:After Final Run You Cannot Edit The Perticular Employee Record.",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + dystring + " it",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {


            $scope.submitted = true;
            $scope.albumNameArray1 = [];
            angular.forEach($scope.employeelist, function (role) {
                if (role.selected) $scope.albumNameArray1.push(role);
            })
                var data = {
                    "PFVPFflag": $scope.PFVPFflagfinal,
                    "IMFY_Id": $scope.IMFY_Id,
                    employee: $scope.albumNameArray1,
                }
                apiService.create("PFTransaction/finalverify", data).then(function (promise) {

                    if (promise.retrunMsg.length > 0 && promise.retrunMsg != null) {
                        if (promise.retrunMsg == "Add") {
                            swal("Final Run Done Successfully")
                            $state.reload();
                        }
                        else {
                            swal("Final Run Faild")
                        }
                    }

                })
                    }
                    else {
                        swal("Record " + dystring + " Cancelled!!!");
                    }
                });

        }





        $scope.DepositWithdra = function () {
            $scope.HeadType = "";

        };
        $scope.cleardata = function () {
            $state.reload();

        };

        //Clear data
        //$scope.Employee = {};
        //$scope.cleardata = function () {
        //    $scope.Employee = {};
        //    $scope.pfreport = [];
        //    $scope.institution = {};
        //    $scope.month = "";
        //    $scope.year = "";
        //    $scope.CurrentInstuteAddress = "";
        //    $scope.submitted = false;

        //    if ($scope.EmployeeDis) {
        //        $scope.EmployeeDis = false;
        //    }

        //    $scope.myForm.$setPristine();
        //    $scope.myForm.$setUntouched();
        //    $scope.onLoadGetData();
        //};

        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };

        $scope.disableGrid = function () {
            if ($scope.EmployeeDis) {
                $scope.EmployeeDis = false;
            }
        };

        $scope.getAmountofWagesTotal = function () {
            var total = 0;
            if ($scope.pfreport != null) {
                for (var i = 0; i < $scope.pfreport.length; i++) {
                    var product = $scope.pfreport[i];
                    total += product.amountofWages;
                }
            }
            return total;
        };

        $scope.getpfAmountTotal = function () {
            var total = 0;
            if ($scope.pfreport != null) {
                for (var i = 0; i < $scope.pfreport.length; i++) {
                    var product = $scope.pfreport[i];
                    total += product.pfAmount;
                }
            }
            return total;
        };

        $scope.gethreS_EPFTotal = function () {
            var total = 0;
            if ($scope.pfreport != null) {
                for (var i = 0; i < $scope.pfreport.length; i++) {
                    var product = $scope.pfreport[i];
                    total += product.hreS_EPF;
                }
            }
            return total;
        };

        $scope.gethreS_FPFTotal = function () {
            var total = 0;
            if ($scope.pfreport != null) {
                for (var i = 0; i < $scope.pfreport.length; i++) {
                    var product = $scope.pfreport[i];
                    total += product.hreS_FPF;
                }
            }
            return total;
        };

        $scope.getTotalBasic = function () {
            var total = 0;
            if ($scope.pfreport != null) {
                for (var i = 0; i < $scope.pfreport.length; i++) {
                    var product = $scope.pfreport[i];
                    total += product.basicvalue;
                }
            }
            return total;
        };

        $scope.getTotalDA = function () {
            var total = 0;
            if ($scope.pfreport != null) {
                for (var i = 0; i < $scope.pfreport.length; i++) {
                    var product = $scope.pfreport[i];
                    total += product.davalue;
                }
            }
            return total;
        };


        // deactive
        $scope.deactive = function (item) {
            var TransactionID = 0;
            if (item.HREPFST_Id > 0) {
                TransactionID = item.HREPFST_Id
            }
            else {
                TransactionID = item.HREVPFST_Id
            }

            var data = {
                "TransactionID": TransactionID,
                "PFVPFflag": $scope.PFVPFflag,
            };

            var dystring = "Delete";
            swal({
                title: "Are you sure?",
                text: "Do You Want To " + dystring + " Record?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + dystring + " it",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("PFTransaction/FilterEmployeeData", data).
                            then(function (promise) {
                                if (promise.retrunMsg == "updated") {
                                    swal("Record " + dystring + "d Successfully!!!");
                                }
                                else {
                                    swal("Record Not " + dystring + "d Successfully!!!");
                                }
                                $state.reload();
                            })
                    }
                    else {
                        swal("Record " + dystring + " Cancelled!!!");
                    }
                });
        }



        //$scope.printData = function () {
        //    var divToPrint = document.getElementById("Table");
        //    var newWin = window.open();
        //    newWin.document.write(divToPrint.outerHTML);
        //    newWin.print();
        //    newWin.close();
        //    // $state.reload();
        //}

        $scope.printToCart = function (Baldwin) {
            var innerContents = document.getElementById("Baldwin").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/pfchallan/EMPPFSchemePdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };
    }
})();