﻿(function () {
    'use strict';
    angular
        .module('app')
        .controller('CumulativeSalaryReportBGHSController', CumulativeSalaryReportBGHSController)

    CumulativeSalaryReportBGHSController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', '$filter', 'superCache', 'Excel', '$timeout']
    function CumulativeSalaryReportBGHSController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, $filter, superCache, Excel, $timeout) {


        //    $scope.gridOptions = {

        //        enableColumnMenus: false,
        //        enableFiltering: true,
        //        paginationPageSizes: [5, 10, 15],
        //        paginationPageSize: 5,
        //        //category: [{ name: 'earningresult', visible: true }],
        //        columnDefs: [
        //              { name: 'SlNo', field: 'name', enableColumnMenu: false, enableFiltering: false, enableSorting: false, cellTemplate: '<div class="ui-grid-cell-contents">{{grid.options.paginationPageSize *(grid.options.paginationCurrentPage-1)+grid.renderContainers.body.visibleRowCache.indexOf(row)+1}}</div>' },
        //          { name: 'hrmE_EmployeeCode', displayName: 'Employee Code', enableHiding: false },
        //          { name: 'hrmE_EmployeeFirstName', displayName: 'Employee Name', enableHiding: false },
        //          { name: 'hrmeD_EarnDedFlag', displayName: 'Designation Name', enableHiding: false },
        ////          { name: 'employeeSalaryslipDetails.earningresult.hresD_Amount', displayName: 'Earning', enableHiding: false },
        ////              { name: 'earningresult', displayName: 'Earning', enableHiding: false },
        ////{ name: 'earningresult', displayName: 'Earning', enableHiding: false },
        ////{ name: 'earningresult', displayName: 'Earning', enableHiding: false },

        ////          { name: 'grossDeduction', displayName: 'Deduction', enableHiding: false }


        //        ],
        //        onRegisterApi: function (gridApi) {
        //            $scope.gridApi = gridApi;
        //            // $scope.gridApi.core.refresh($scope.gridOptions.data);
        //        }

        //    };



        $scope.onLoadGetData = function () {
            var pageid = 2;
            apiService.getURI("CumulativeSalaryReport/getalldetails", pageid).then(function (promise) {


                if (promise.leaveyeardropdown !== null && promise.leaveyeardropdown.length > 0) {

                    $scope.leaveyeardropdown = promise.leaveyeardropdown;
                }

                if (promise.monthdropdown !== null && promise.monthdropdown.length > 0) {
                    $scope.monthdropdown = promise.monthdropdown;


                }

                if (promise.employeedropdown !== null && promise.employeedropdown.length > 0) {
                    $scope.employeedropdown = promise.employeedropdown;
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

                if (promise.configurationDetails != null) {

                    $scope.SalaryFromDay = promise.configurationDetails.hrC_SalaryFromDay;
                    $scope.SalaryToDay = promise.configurationDetails.hrC_SalaryToDay;

                }

            })
            $scope.loadmodulefeedback();
        }
        $scope.loadmodulefeedback = function () {


            var data = {
                "Flag": "FrontOffice"                  // here use your module name (Eg : If Module is COE , then add  "Flag": "COE" )  

            };
            apiService.create("FeedbackTransaction/loadfeedbackquestion", data).then(function (promise) {

                if (promise.feedbackquestion !== undefined && promise.feedbackquestion !== null && promise.feedbackquestion.length > 0) {

                    $scope.feedbackquestion = promise.feedbackquestion;
                    $scope.feedbackoption = promise.feedbackoption;

                    $scope.TempGetFeedbackOption = [];

                    angular.forEach($scope.feedbackquestion, function (fqe) {
                        $scope.TempGetFeedbackOption = [];
                        angular.forEach($scope.feedbackoption, function (fop) {
                            if (fqe.fmtY_Id == fop.fmtY_Id && fqe.fmqE_Id == fop.fmqE_Id) {
                                $scope.TempGetFeedbackOption.push(fop)
                            }
                        });

                        fqe.feedbackoptiondata = $scope.TempGetFeedbackOption;

                    })
                    $("#feedback").modal('show');
                }
            });

        };

        $scope.submitted1 = false;

        $scope.Savefeedback = function () {
            if ($scope.myForm1.$valid) {
                //fp.fmoP_Id > 0 && 
                $scope.temp = [];
                angular.forEach($scope.feedbackquestion, function (fop) {
                    angular.forEach(fop.feedbackoptiondata, function (fp) {
                        if (fop.name == fp.fmoP_Id && fop.fmqE_Id == fp.fmqE_Id && fp.fmoP_FeedbackOptions != "") {
                            $scope.temp.push({
                                name: fp.fmoP_Id,
                                FMOP_FeedbackOptions: fp.fmoP_FeedbackOptions,
                                FMOP_OptionsValue: fp.fmoP_OptionsValue,
                                FMTY_Id: fop.fmtY_Id,
                                FMQE_Id: fp.fmqE_Id,
                                FMQE_FeedbackQuestions: fop.fmqE_FeedbackQuestions,
                                FMQE_FeedbackQRemarks: fop.fmqE_FeedbackQRemarks,
                                FMTY_FeedbackTypeName: fop.fmtY_FeedbackTypeName,
                                FSTTR_FeedBack: fop.fsttR_FeedBack

                            })
                        }

                    });
                });



                var data = {
                    "savemodulefeedback": $scope.temp

                };
                apiService.create("FeedbackTransaction/Savefeedback", data).then(function (promise) {
                    if (promise !== null) {
                        if (promise.returnval === true) {
                            swal("Record Saved/Updated Successfully");
                        }
                        else {
                            swal("Failed Saved/Updated Record");
                        }
                        $state.reload();
                    }
                });
            } else {
                $scope.submitted1 = true;
            }
        };
        $scope.getTotal = function () {
            var total = 0;
            for (var i = 0; i < $scope.employeeSalaryslipDetails.length; i++) {
                var product = $scope.employeeSalaryslipDetails[i];
                total += product.totalEmployees;
            }
            return Math.round(total);
        }





        $scope.employeeSalaryslipDetails = [];
        $scope.institutionDetails = {};
        $scope.EmployeeDis = false;

        $scope.submitted = false;
        $scope.SearchEmployee = function () {
            $scope.EmployeeDis = false;
            $scope.submitted = true;
            if ($scope.myForm.$valid) {

                $scope.institutionDetails = {};
                var groupTypeselected = [];
                angular.forEach($scope.groupTypedropdown, function (itm) {
                    if (itm.selected) {
                        groupTypeselected.push(itm.hrmgT_Id);//
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


                var data = {
                    "HRES_Year": $scope.Employee.hreS_Year,
                    "HRES_Month": $scope.Employee.hreS_Month,
                    groupTypeIdList: groupTypeselected,
                    hrmD_IdList: departmentselected,
                    hrmdeS_IdList: designationselected,
                    "comm": "0"
                }

                apiService.create("CumulativeSalaryReport/getEmployeedetailsBySelection", data).
                    then(function (promise) {
                        if (promise.headList !== null && promise.headList.length > 0) {
                            $scope.headList = promise.headList;
                        }


                        if (promise.institutionDetails != null) {
                            $scope.institutionDetails = promise.institutionDetails;

                            //  $('#blah').attr('src', 'https://bdcampusstrg.blob.core.windows.net/files/' + $scope.institutionDetails.mi_id + "/" + "EmployeeProfilePics" + "/" + $scope.institutionDetails.mI_Logo);

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

                        //get current date
                        $scope.HRES_Year = promise.hreS_Year;

                        //get DOI
                        $scope.HRES_Month = promise.hreS_Month;

                        if (promise.employeeSalaryslipDetails !== null && promise.employeeSalaryslipDetails.length > 0) {
                            $scope.EmployeeDis = true;

                            $scope.employeeSalaryslipDetails = promise.employeeSalaryslipDetails;
                            // console.log($scope.employeeSalaryslipDetails);
                            console.log($scope.employeeSalaryslipDetails);
                            // $scope.gridOptions.data = promise.employeeSalaryslipDetails;
                            $scope.earningheadlist = $scope.employeeSalaryslipDetails[0].earningresult;

                            angular.forEach($scope.earningheadlist, function (headresult) {

                                headresult.netamount = 0;

                            });

                            $scope.deductionheadlist = $scope.employeeSalaryslipDetails[0].deductionresult;

                            angular.forEach($scope.deductionheadlist, function (headresult) {

                                headresult.netamount = 0;

                            });



                            angular.forEach($scope.employeeSalaryslipDetails, function (itm) {

                                itm.grossEarning = Math.round(itm.grossEarning);
                                itm.grossDeduction = Math.round(itm.grossDeduction);
                                itm.netSalary = Math.round(itm.netSalary);

                                //earning head totalAmount
                                angular.forEach(itm.earningresult, function (result) {

                                    result.hresD_Amount = Math.round(result.hresD_Amount);
                                    angular.forEach($scope.earningheadlist, function (headresult) {

                                        if (headresult.hrmeD_Name == result.hrmeD_Name) {

                                            headresult.netamount = Math.round(headresult.netamount + result.hresD_Amount);
                                        }

                                    });

                                });
                                //deduction head totalAmount
                                angular.forEach(itm.deductionresult, function (result) {

                                    result.hresD_Amount = Math.round(result.hresD_Amount);

                                    angular.forEach($scope.deductionheadlist, function (headresult) {

                                        if (headresult.hrmeD_Name == result.hrmeD_Name) {

                                            headresult.netamount = Math.round(headresult.netamount + result.hresD_Amount);
                                        }

                                    });

                                });


                            });



                            $scope.earnlen = $scope.earningheadlist.length;
                            $scope.dedlen = $scope.deductionheadlist.length;

                        }
                        else {
                            $scope.EmployeeDis = false;
                            swal('No Record found to display .. !');
                            return;
                        }
                        var temp_emp_list = [];
                        angular.forEach($scope.departmentdropdown, function (dept) {
                            if (dept.selected) {
                                temp_emp_list.push({ hrmE_EmployeeCode: dept.hrmD_DepartmentName });
                                angular.forEach($scope.employeeSalaryslipDetails, function (emp) {
                                    if (emp.hrmD_Id == dept.hrmD_Id) {
                                        temp_emp_list.push(emp);
                                    }
                                })
                            }
                        })
                        $scope.employeeSalaryslipDetails_final = temp_emp_list;


                    })
            }

        }



        //By group Type
        $scope.GetEmployeeBygroupTypeAll = function (groupTypeselectedAll) {
            if ($scope.EmployeeDis) {
                $scope.EmployeeDis = false;
            }
            var toggleStatus = $scope.groupTypeselectedAll;
            angular.forEach($scope.groupTypedropdown, function (itm) {
                itm.selected = toggleStatus;
            });
            $scope.get_depts();
        }


        //single
        $scope.GetEmployeeBygroupType = function (groupType) {
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
            $scope.get_desig();

        }


        //By Department Single
        $scope.GetEmployeeByDepartment = function (department) {
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
            $scope.employeeSalaryslipDetails = [];
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
            // $scope.gridOptions.data = promise.employeeSalaryslipDetails;
            $scope.onLoadGetData();

        }

        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };



        $scope.printData = function () {
            var divToPrint = document.getElementById("Table");
            var newWin = window.open();
            newWin.document.write(divToPrint.outerHTML);
            newWin.print();
            newWin.close();
            // $state.reload();
        }

        //Total for per column

        $scope.TotalgrossEarning = function () {
            var total = 0;
            if ($scope.employeeSalaryslipDetails != null) {
                for (var i = 0; i < $scope.employeeSalaryslipDetails.length; i++) {


                    var product = $scope.employeeSalaryslipDetails[i];
                    total += product.grossEarning;
                }
            }

            return Math.round(total);
        }

        $scope.TotalgrossDeduction = function () {
            var total = 0;
            if ($scope.employeeSalaryslipDetails != null) {
                for (var i = 0; i < $scope.employeeSalaryslipDetails.length; i++) {


                    var product = $scope.employeeSalaryslipDetails[i];
                    total += product.grossDeduction;
                }
            }

            return Math.round(total);
        }

        $scope.TotalnetSalary = function () {
            var total = 0;
            if ($scope.employeeSalaryslipDetails != null) {
                for (var i = 0; i < $scope.employeeSalaryslipDetails.length; i++) {


                    var product = $scope.employeeSalaryslipDetails[i];
                    total += product.netSalary;
                }
            }

            return Math.round(total);
        }



        $scope.printToCart = function (Baldwin) {
            var innerContents = document.getElementById("Baldwin").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/pfchallan/PFChallanPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        }

        $scope.exportToExcel = function (tableId) {
            //var exportHref = Excel.tableToExcel(tableId, 'sheet name');
            var exportHref = Excel.tableToExcel(tableId, 'CumulativeSalary');
            $timeout(function () {
                location.href = exportHref;
            }, 100);
        };


    }


})();