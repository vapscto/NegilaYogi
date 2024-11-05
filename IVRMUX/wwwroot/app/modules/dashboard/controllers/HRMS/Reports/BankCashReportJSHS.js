(function () {
    'use strict';
    angular
        .module('app')
        .controller('BankCashReportJSHSController', BankCashReportJSHSController)

    BankCashReportJSHSController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', '$filter', 'Excel', '$timeout', 'superCache']

    function BankCashReportJSHSController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, $filter, Excel, $timeout, superCache) {

        //Employeee
        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;
        $scope.SalaryFromDay = "";
        $scope.SalaryToDay = "";


        var logopath = "";
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings !== null && admfigsettings.length > 0) {
            logopath = admfigsettings[0].asC_Logo_Path;
        } else {
            logopath = "";
        }
        $scope.imgname = logopath;


        // Get form Details at onload 
        $scope.onLoadGetData = function () {
            var pageid = 2;
            apiService.getURI("BankCashReport/getalldetails", pageid).then(function (promise) {

                if (promise.leaveyeardropdown !== null && promise.leaveyeardropdown.length > 0) {
                    $scope.leaveyeardropdown = promise.leaveyeardropdown;
                }

                if (promise.monthdropdown !== null && promise.monthdropdown.length > 0) {
                    $scope.monthdropdown = promise.monthdropdown;
                }

                if (promise.bankdropdown !== null && promise.bankdropdown.length > 0) {
                    $scope.bankdropdown = promise.bankdropdown;
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

            });
        };

        $scope.selobj = {};
        $scope.addColumn = function (role, indexx, headertest) {
            $scope.selobj = role.hrmD_DepartmentName;
            angular.forEach(headertest, function (subscription, index) {
                if (indexx != index)
                    subscription.selected = false;
            });
        };

        $scope.employeeDetails = [];
        $scope.EmployeeDis = false;
        $scope.institutionDetails = {};

        //Search employee
        $scope.submitted = false;
        $scope.SearchEmployee = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                $scope.institutionDetails = {};
                var groupTypeselected = [];
                var departmentselected = [];
                // var designationselected = [];
                var data = {};

                angular.forEach($scope.groupTypedropdown, function (itm) {
                    if (itm.selected) {
                        groupTypeselected.push(itm.hrmgT_Id);
                    }
                });

                angular.forEach($scope.departmentdropdown, function (itm) {
                    if (itm.selected) {
                        departmentselected.push(itm.hrmD_Id);
                    }
                });

                if (groupTypeselected.length == 0 && departmentselected.length == 0) {
                    swal('Kindly select atleast one record');
                    return;
                }

                $scope.Employee.groupTypeselected = groupTypeselected;
                $scope.Employee.departmentselected = departmentselected;
                // $scope.Employee.designationselected = designationselected;
                data = $scope.Employee;

                apiService.create("BankCashReport/getEmployeedetailsBySelection", data).
                    then(function (promise) {

                        if (promise.institutionDetails != null) {
                            $scope.institutionDetails = promise.institutionDetails;

                            $scope.selectedMonth = promise.hreS_Month;
                            $scope.selectedYear = promise.hreS_Year;

                            var instuteAddress = "";
                            if ($scope.institutionDetails.mI_Address1 != null && $scope.institutionDetails.mI_Address1 != "") {
                                instuteAddress = $scope.institutionDetails.mI_Address1;
                            }

                            if ($scope.institutionDetails.mI_Address2 != null && $scope.institutionDetails.mI_Address2 != "") {
                                instuteAddress = instuteAddress + ',' + $scope.institutionDetails.mI_Address2;
                            }

                            if ($scope.institutionDetails.mI_Address3 != null && $scope.institutionDetails.mI_Address3 != "") {
                                instuteAddress = instuteAddress + ',' + $scope.institutionDetails.mI_Address3;
                            }

                            $scope.CurrentInstuteAddress = instuteAddress;
                        }

                        if (promise.employeeDetails !== null && promise.employeeDetails.length > 0) {
                            $scope.EmployeeDis = true;
                            $scope.employeeDetails = promise.employeeDetails;

                            var totalgrp = 0;
                            var not_paid_list = [];
                            var temp = [];
                            angular.forEach($scope.employeeDetails, function (cls) {
                                cls.bankAcNumber = cls.bankAcNumber.toString();
                                cls.netSalary = Math.round(cls.netSalary, 0);
                                if (cls != null) {
                                    not_paid_list.push(cls);
                                }
                                else {
                                    temp.push(cls);
                                }
                            });

                            $scope.employeeDetails = not_paid_list;
                            angular.forEach($scope.employeeDetails, function (ty) {
                                totalgrp += Math.round(ty.netSalary,0);
                            });

                            //$scope.totalgrp = totalgrp;

                            $scope.coltotalarray = [];
                            $scope.coltotalarray.push({ name: 'netSalary', field: 'netSalary', aggregate: "sum" });

                            //KINDO IMPLEMENT
                            $scope.colarrayall = [];
                            $scope.colarrayall = [
                                { title: 'Sl.No', template: "<span class='row-number'></span>", width: 100 },
                                { name: 'employeeCode', field: 'employeeCode', title: 'EMPLOYEE CODE', width: 200 },
                                { name: 'employeeName', field: 'employeeName', title: 'NAME OF THE STAFF', width: 200 },
                                { name: 'bankAcNumber', field: 'bankAcNumber', title: 'S.B A/C NO.', width: 200 },
                                { name: 'netSalary', field: 'netSalary', title: 'AMOUNT', width: 200, aggregates: ["sum"], footerTemplate: "#=sum#", groupFooterTemplate: " #=sum#" }
                            ];

                            $(document).ready(function () {
                                $('#kindogridBankCash').empty();
                                $("#kindogridBankCash").kendoGrid({
                                    toolbar: ["excel"],

                                    excel: {
                                        fileName: "BankCashReport.xls",
                                        //allPages: true,
                                        proxyURL: "",
                                        filterable: true,
                                        allPages: true
                                    },

                                    excelExport: function (e) {
                                        var sheet = e.workbook.sheets[0];
                                        sheet.frozenRows = 2;
                                        sheet.name = "BankCashReport";

                                        var myHeaders = [{
                                           value: "Bank Cash Report",
                                            fontSize: 15,
                                            textAlign: "center",
                                            background: "#ffffff",
                                            color: "black"
                                        }];

                                        sheet.rows.splice(0, 0, { cells: myHeaders, type: "header", height: 70 });
                                    },

                                    dataSource: {
                                        data: $scope.employeeDetails,
                                        pageSize: 500,
                                        aggregate: $scope.coltotalarray
                                    },

                                    sortable: true,
                                    // pageable: true,
                                    groupable: false,
                                    filterable: true,
                                    columnMenu: true,
                                    reorderable: true,
                                    resizable: true,

                                    columns: $scope.colarrayall,
                                    dataBound: function () {
                                        var pagenum = this.dataSource.page();
                                        var pageitms = this.dataSource.pageSize();
                                        var rows = this.items();
                                        $(rows).each(function () {
                                            var index = (pagenum - 1) * pageitms + $(this).index() + 1;
                                            var rowLabel = $(this).find(".row-number");
                                            $(rowLabel).html(index);
                                        });
                                    }
                                });
                            });
                            //KINDO IMPLEMENT

                            //var x = totalgrp;
                            //x = x.toString();
                            //var lastThree = x.substring(x.length - 3);
                            //var otherNumbers = x.substring(0, x.length - 3);
                            //if (otherNumbers != '')
                            //    lastThree = ',' + lastThree;
                            //var res = otherNumbers.replace(/\B(?=(\d{2})+(?!\d))/g, ",") + lastThree;
                            // alert(res);

                            $scope.totalgrp = totalgrp;
                            $scope.NetAmountInwords = toWords($scope.totalgrp) + 'only)';
                        }
                        else {
                            $scope.EmployeeDis = false;
                            swal('No Record found to display .. !');
                            return;
                        }

                    });
            }

        };

        //By Employee Type
        $scope.GetEmployeeBygroupTypeAll = function (groupTypeselectedAll) {
            if ($scope.EmployeeDis) {
                $scope.EmployeeDis = false;
            }
            var toggleStatus = $scope.groupTypeselectedAll;
            angular.forEach($scope.groupTypedropdown, function (itm) {
                itm.selected = toggleStatus;
            });
        };

        //single
        $scope.GetEmployeeBygroupType = function (groupType) {
            if ($scope.EmployeeDis) {
                $scope.EmployeeDis = false;
            }
            $scope.groupTypeselectedAll = $scope.groupTypedropdown.every(function (itm) {
                return itm.selected;
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
                $scope.selobj = itm.hrmD_DepartmentName;
            });
        };

        //By Department Single
        $scope.GetEmployeeByDepartment = function (department) {
            if ($scope.EmployeeDis) {
                $scope.EmployeeDis = false;
            }
            $scope.departmentselectedAll = $scope.departmentdropdown.every(function (itm) {
                return itm.selected;
            });

            if (department.selected) {
                $scope.selobj = department.hrmD_DepartmentName;
            }
            else if (!department.selected) {
                $scope.selobj = {};
                angular.forEach($scope.departmentdropdown, function (itm) {
                    if (itm.selected) {
                        $scope.selobj = itm.hrmD_DepartmentName;
                    }
                });
            }
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
        };

        //By Designation Single
        $scope.GetEmployeeByDesignation = function (designation) {
            if ($scope.EmployeeDis) {
                $scope.EmployeeDis = false;
            }
            $scope.designationselectedAll = $scope.designationdropdown.every(function (itm) {
                return itm.selected;
            });
        };

        //Clear data
        $scope.Employee = {};
        $scope.cleardata = function () {
            $scope.Employee = {};
            $scope.employeeDetails = [];
            $scope.submitted = false;
            $scope.groupTypeselectedAll = false;
            $scope.departmentselectedAll = false;
            $scope.designationselectedAll = false;
            $scope.employeeSelectedAll = false;
            if ($scope.EmployeeDis) {
                $scope.EmployeeDis = false;
            }
            $scope.search = "";
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
            $scope.onLoadGetData();
        };

        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };

        $scope.disableGrid = function () {
            if ($scope.EmployeeDis) {
                $scope.EmployeeDis = false;
            }
        };

        //$scope.printData = function () {
        //    var divToPrint = document.getElementById("Table");
        //        var newWin = window.open();
        //        newWin.document.write(divToPrint.outerHTML);
        //        newWin.print();
        //        newWin.close();
        //        // $state.reload();
        //    }

        //var th = ['', 'thousand', 'lakhs', 'billion', 'trillion'];
        //var dg = ['Zero', 'One', 'Two', 'Three', 'Four', 'Five', 'Six', 'Seven', 'Eight', 'Nine'];
        //var tn = ['Ten', 'Eleven', 'Twelve', 'Thirteen', 'Fourteen', 'Fifteen', 'Sixteen', 'Seventeen', 'Eighteen', 'Nineteen'];
        //var tw = ['Twenty', 'Thirty', 'Forty', 'Fifty', 'Sixty', 'Seventy', 'Eighty', 'Ninety'];

        //function toWords(s) {
        //    s = s.toString();
        //    s = s.replace(/[\, ]/g, '');
        //    if (s != parseFloat(s)) return 'not a number';
        //    var x = s.indexOf('.');
        //    if (x == -1) x = s.length;
        //    if (x > 15) return 'too big';
        //    var n = s.split('');
        //    var str = '';
        //    var sk = 0;
        //    for (var i = 0; i < x; i++) {
        //        if ((x - i) % 3 == 2) {
        //            if (n[i] == '1') {
        //                str += tn[Number(n[i + 1])] + ' ';
        //                i++;
        //                sk = 1;
        //            }
        //            else if (n[i] != 0) {
        //                str += tw[n[i] - 2] + ' ';
        //                sk = 1;
        //            }
        //        }
        //        else if (n[i] != 0) {
        //            str += dg[n[i]] + ' ';
        //            if ((x - i) % 3 == 0) str += 'hundred ';
        //            sk = 1;
        //        }


        //        if ((x - i) % 3 == 1) {
        //            if (sk) str += th[(x - i - 1) / 3] + ' ';
        //            sk = 0;
        //        }
        //    }
        //    if (x != s.length) {
        //        var y = s.length;
        //        str += 'point ';
        //        for (var i = x + 1; i < y; i++) str += dg[n[i]] + ' ';
        //    }
        //    return str.replace(/\s+/g, ' ');
        //}

        function toWords(totalc) {
            var words = new Array();
            words[0] = '';
            words[1] = 'One';
            words[2] = 'Two';
            words[3] = 'Three';
            words[4] = 'Four';
            words[5] = 'Five';
            words[6] = 'Six';
            words[7] = 'Seven';
            words[8] = 'Eight';
            words[9] = 'Nine';
            words[10] = 'Ten';
            words[11] = 'Eleven';
            words[12] = 'Twelve';
            words[13] = 'Thirteen';
            words[14] = 'Fourteen';
            words[15] = 'Fifteen';
            words[16] = 'Sixteen';
            words[17] = 'Seventeen';
            words[18] = 'Eighteen';
            words[19] = 'Nineteen';
            words[20] = 'Twenty';
            words[30] = 'Thirty';
            words[40] = 'Forty';
            words[50] = 'Fifty';
            words[60] = 'Sixty';
            words[70] = 'Seventy';
            words[80] = 'Eighty';
            words[90] = 'Ninety';
            totalc = totalc.toString();
            var atemp = totalc.split(".");
            var number = atemp[0].split(",").join("");
            var n_length = number.length;
            var words_string = "";
            if (n_length <= 9) {
                var n_array = new Array(0, 0, 0, 0, 0, 0, 0, 0, 0);
                var received_n_array = new Array();
                for (var i = 0; i < n_length; i++) {
                    received_n_array[i] = number.substr(i, 1);
                }
                for (var i = 9 - n_length, j = 0; i < 9; i++ , j++) {
                    n_array[i] = received_n_array[j];
                }
                for (var i = 0, j = 1; i < 9; i++ , j++) {
                    if (i == 0 || i == 2 || i == 4 || i == 7) {
                        if (n_array[i] == 1) {
                            n_array[j] = 10 + parseInt(n_array[j]);
                            n_array[i] = 0;
                        }
                    }
                }
                totalc = "";
                for (var i = 0; i < 9; i++) {
                    if (i == 0 || i == 2 || i == 4 || i == 7) {
                        totalc = n_array[i] * 10;
                    } else {
                        totalc = n_array[i];
                    }
                    if (totalc != 0) {
                        words_string += words[totalc] + " ";
                    }
                    if ((i == 1 && totalc != 0) || (i == 0 && totalc != 0 && n_array[i + 1] == 0)) {
                        words_string += "Crores ";
                    }
                    if ((i == 3 && totalc != 0) || (i == 2 && totalc != 0 && n_array[i + 1] == 0)) {
                        words_string += "Lakhs ";
                    }
                    if ((i == 5 && totalc != 0) || (i == 4 && totalc != 0 && n_array[i + 1] == 0)) {
                        words_string += "Thousand ";
                    }
                    if (i == 6 && totalc != 0 && (n_array[i + 1] != 0 && n_array[i + 2] != 0)) {
                        words_string += "Hundred and ";
                    } else if (i == 6 && totalc != 0) {
                        words_string += "Hundred ";
                    }
                }
                words_string = words_string.split("  ").join(" ");
            }
            return words_string;
        }

        $scope.printToCart = function (Baldwin) {
            var innerContents = document.getElementById("Baldwin").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/font-awesome.min.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/pfchallan/PFChallanPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };

        //$scope.exportToExcel = function (tableId) {
        //    //var exportHref = Excel.tableToExcel(tableId, 'sheet name');
        //    var exportHref = Excel.tableToExcel(tableId, 'BankCash');
        //    $timeout(function () {
        //        location.href = exportHref;
        //    }, 500);
        //};

        $scope.exportToExcel = function (Baldwin1) {
            var exportHref = Excel.tableToExcel(Baldwin1, 'BankCash');
            $timeout(function () { location.href = exportHref; }, 100);
        };

    }

})();