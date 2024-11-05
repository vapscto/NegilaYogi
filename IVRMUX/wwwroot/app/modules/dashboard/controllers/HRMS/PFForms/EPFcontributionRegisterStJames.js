﻿(function () {
    'use strict';
    angular
        .module('app')
        .controller('EPFcontributionRegisterStJamesController', EPFcontributionRegisterStJamesController)

    EPFcontributionRegisterStJamesController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', '$filter', 'superCache', '$timeout', 'Excel']
    function EPFcontributionRegisterStJamesController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, $filter, superCache, $timeout, Excel) {

        //Employeee
        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;

        // Get form Details at onload 
        $scope.onLoadGetData = function () {
            var pageid = 2;
            apiService.getURI("ESIReport/getalldetails", pageid).then(function (promise) {

                if (promise.leaveyeardropdown !== null && promise.leaveyeardropdown.length > 0) {

                    $scope.leaveyeardropdown = promise.leaveyeardropdown;
                }

                if (promise.monthdropdown !== null && promise.monthdropdown.length > 0) {
                    $scope.monthdropdown = promise.monthdropdown;


                }

                if (promise.groupTypedropdown !== null && promise.groupTypedropdown.length > 0) {
                    $scope.groupTypedropdown = promise.groupTypedropdown;
                    $scope.groupTypeselectedAll = true;
                    $scope.GetEmployeeBygroupTypeAll($scope.groupTypeselectedAll);

                }


                if (promise.departmentdropdown !== null && promise.departmentdropdown.length > 0) {
                    $scope.departmentdropdown = promise.departmentdropdown;
                    $scope.departmentselectedAll = true;
                    $scope.GetEmployeeByDepartmentAll($scope.departmentselectedAll);
                }

                if (promise.designationdropdown !== null && promise.designationdropdown.length > 0) {
                    $scope.designationdropdown = promise.designationdropdown;

                    $scope.designationselectedAll = true;
                    $scope.GetEmployeeByDesignationAll($scope.designationselectedAll);


                }


            })
        }


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

                var groupTypeselected = [];
                angular.forEach($scope.groupTypedropdown, function (itm) {
                    if (itm.selected) {
                        groupTypeselected.push(itm.hrmgT_Id);
                    }
                });

                var departmentselected = [];
                angular.forEach($scope.departmentdropdown, function (itm) {
                    if (itm.selected) {
                        departmentselected.push(itm.hrmD_Id);
                    }

                });

                var designationselected = [];
                angular.forEach($scope.designationdropdown, function (itm) {
                    if (itm.selected) {
                        designationselected.push(itm.hrmdeS_Id);
                    }

                });

                if (groupTypeselected.length == 0 && departmentselected.length == 0 && designationselected.length == 0) {
                    swal('Kindly select atleast one record');
                    return;
                }
                // var data = $scope.Employee;

                var data = {
                    "HRES_Year": $scope.Employee.hreS_Year,
                    "HRES_Month": $scope.Employee.hreS_Month,
                    groupTypeIdList: groupTypeselected,
                    hrmD_IdList: departmentselected,
                    hrmdeS_IdList: designationselected
                };
                apiService.create("EPFcontributionRegister/getEmployeedetailsBySelectionStJames", data).
                    then(function (promise) {
                        $scope.EmployeeDis = true;
                        if (promise.pfreport !== null && promise.pfreport.length > 0) {
                            angular.forEach(promise.pfreport, function (temp) {

                                //Pension Fund
                                $scope.sumcondition = 0;
                                $scope.sumcondition = Math.round(temp.basicamount) + Math.round(temp.dAamount) + Math.round(temp.othersamount);

                                //if (temp.hrmE_PFuAN == '100733210567') { ///tocheck singal                              
                            if (temp.hrmE_Age >= 58) {
                                //    temp.pensionFund = 0.00;
                                if (temp.hreS_WorkingDays > 0) {
                                    temp.pensionFund = Math.round((1250 * temp.hreS_WorkingDays) / (temp.abc), 2);
                                    // temp.pensionFund = Math.round(((12150) * (8.33 / 100)) / (temp.abc) * temp.hreS_WorkingDays);
                                }
                                else {
                                    temp.pensionFund = 0.00;
                                }
                            }
                            else {
                                if ($scope.sumcondition < 15000 && temp.hrmE_FPFNotApplicableFlg == true) {
                                    temp.pensionFund = Math.round((8.33 / 100) * $scope.sumcondition)

                                }
                                else if (temp.hrmE_FPFNotApplicableFlg == true) {
                                    temp.pensionFund = 1250.00;
                                }
                                else {
                                    temp.pensionFund = 0.00;
                                }
                            }
                                if (promise.hreS_Month == 'June' && promise.hreS_Year == 2023 && temp.hrmE_PFuAN == '100733210567') {

                                    if (temp.hrmE_Age >= 58) {
                                        //    temp.pensionFund = 0.00;
                                        if (temp.hreS_WorkingDays > 0) {
                                            //temp.pensionFund = Math.round((1250 * temp.hreS_WorkingDays) / (temp.abc), 2);
                                            temp.pensionFund = Math.round(((12150) * (8.33 / 100)) / (temp.abc) * temp.hreS_WorkingDays);
                                        }
                                        else {
                                            temp.pensionFund = 0.00;
                                        }
                                    }
                                    else {
                                        if ($scope.sumcondition < 15000 && temp.hrmE_FPFNotApplicableFlg == true) {
                                            temp.pensionFund = Math.round((8.33 / 100) * $scope.sumcondition)

                                        }
                                        else if (temp.hrmE_FPFNotApplicableFlg == true) {
                                            temp.pensionFund = 1250.00;
                                        }
                                        else {
                                            temp.pensionFund = 0.00;
                                        }
                                    }

                                }

                            if (temp.hrmE_FPFNotApplicableFlg == false) {
                                temp.schoolpf = temp.stjOwnPF;
                            }
                            else {
                                temp.schoolpf = temp.stjOwnPF - temp.pensionFund;
                            }
                            temp.emptotdedSal = (temp.stjOwnPF + temp.vpfAmount);


                            // }


                        });
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
}

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
    })
    var data = {
        hrmgT_IdList: ids
    }
    apiService.create("CumulativeSalaryReport/get_depts", data).
        then(function (promise) {

            if (promise.departmentdropdown !== null && promise.departmentdropdown.length > 0) {
                $scope.departmentdropdown = promise.departmentdropdown;
                $scope.departmentselectedAll = true;
                $scope.GetEmployeeByDepartmentAll($scope.departmentselectedAll);
            }
        })
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

    })

    $scope.get_desig();

}


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
}
$scope.get_desig = function () {
    var ids = [];
    angular.forEach($scope.groupTypedropdown, function (grp_t) {
        if (grp_t.selected) {
            ids.push(grp_t.hrmgT_Id);
        }
    })
    var ids1 = [];
    angular.forEach($scope.departmentdropdown, function (grp_t) {
        if (grp_t.selected) {
            ids1.push(grp_t.hrmD_Id);
        }
    })
    var data = {
        hrmgT_IdList: ids,
        hrmD_IdList: ids1
    }
    apiService.create("CumulativeSalaryReport/get_desig", data).
        then(function (promise) {
            if (promise.designationdropdown !== null && promise.designationdropdown.length > 0) {
                $scope.designationdropdown = promise.designationdropdown;
                $scope.designationselectedAll = true;
                $scope.GetEmployeeByDesignationAll($scope.designationselectedAll);
            }
        })
};



//By Designation
$scope.GetEmployeeByDesignationAll = function (designationselectedAll) {
    if ($scope.EmployeeDis) {
        $scope.EmployeeDis = false;
    }
    var toggleStatus = $scope.designationselectedAll;
    angular.forEach($scope.designationdropdown, function (itm) {
        itm.selected = toggleStatus;

    });

}


//By Designation Single
$scope.GetEmployeeByDesignation = function (designation) {
    if ($scope.EmployeeDis) {
        $scope.EmployeeDis = false;
    }
    $scope.designationselectedAll = $scope.designationdropdown.every(function (itm) {

        return itm.selected;
    });
}


//Clear data
$scope.Employee = {};
$scope.cleardata = function () {
    $scope.Employee = {};
    $scope.pfreport = [];
    $scope.institution = {};
    $scope.month = "";
    $scope.year = "";
    $scope.CurrentInstuteAddress = "";
    $scope.submitted = false;

    if ($scope.EmployeeDis) {
        $scope.EmployeeDis = false;
    }

    $scope.myForm.$setPristine();
    $scope.myForm.$setUntouched();
    $scope.onLoadGetData();

}

$scope.interacted = function (field) {
    return $scope.submitted || field.$dirty;
};

$scope.disableGrid = function () {
    if ($scope.EmployeeDis) {
        $scope.EmployeeDis = false;
    }
}

$scope.getbasicTotal = function () {
    var total = 0;
    if ($scope.pfreport != null) {
        for (var i = 0; i < $scope.pfreport.length; i++) {
            var product = $scope.pfreport[i];
            total += product.basicamount;
        }
    }
    return total;
}

$scope.getdaTotal = function () {
    var total = 0;
    if ($scope.pfreport != null) {
        for (var i = 0; i < $scope.pfreport.length; i++) {
            var product = $scope.pfreport[i];
            total += product.dAamount;
        }
    }
    return total;
}

$scope.getotherTotal = function () {
    var total = 0;
    if ($scope.pfreport != null) {
        for (var i = 0; i < $scope.pfreport.length; i++) {
            var product = $scope.pfreport[i];
            total += product.othersamount;
        }
    }
    return total;
}

$scope.getnetTotal = function () {
    var total = 0;
    if ($scope.pfreport != null) {
        for (var i = 0; i < $scope.pfreport.length; i++) {
            var product = $scope.pfreport[i];
            total += product.netsalary;
        }
    }
    return total;
}

$scope.getpfAmountTotal = function () {
    var total = 0;
    if ($scope.pfreport != null) {
        for (var i = 0; i < $scope.pfreport.length; i++) {
            var product = $scope.pfreport[i];
            total += product.pfAmount;
        }
    }
    return total;
}

$scope.getownpfTotal = function () {
    var total = 0;
    if ($scope.pfreport != null) {
        for (var i = 0; i < $scope.pfreport.length; i++) {
            var product = $scope.pfreport[i];
            total += product.stjOwnPF;
        }
    }
    return total;
}

$scope.getschoolpfTotal = function () {
    var total = 0;
    if ($scope.pfreport != null) {
        for (var i = 0; i < $scope.pfreport.length; i++) {
            var product = $scope.pfreport[i];
            total += product.schoolpf;
        }
    }
    return total;
}

$scope.getpensionTotal = function () {
    var total = 0;
    if ($scope.pfreport != null) {
        for (var i = 0; i < $scope.pfreport.length; i++) {
            var product = $scope.pfreport[i];
            total += product.pensionFund;
        }
    }
    return total;
}

$scope.getvpfTotal = function () {
    var total = 0;
    if ($scope.pfreport != null) {
        for (var i = 0; i < $scope.pfreport.length; i++) {
            var product = $scope.pfreport[i];
            total += product.vpfAmount;
        }
    }
    return total;
}

$scope.getdedTotal = function () {
    var total = 0;
    if ($scope.pfreport != null) {
        for (var i = 0; i < $scope.pfreport.length; i++) {
            var product = $scope.pfreport[i];
            total += product.emptotdedSal;
        }
    }
    return total;
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
}

$scope.exportToExcel = function (tableId) {
    var exportHref = Excel.tableToExcel(tableId, 'pfreportid');
    $timeout(function () { location.href = exportHref; }, 100);
}

    }


}) ();
