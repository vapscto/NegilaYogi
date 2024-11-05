﻿(function () {
    'use strict';
    angular
        .module('app')
        .controller('CumulativeSalaryReportController', CumulativeSalaryReportController)

    CumulativeSalaryReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', '$filter', 'Excel', '$timeout', 'superCache']
    function CumulativeSalaryReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, $filter, Excel, $timeout, superCache) {
        //object

        //Employeee
        //$scope.currentPage = 1;
        //$scope.itemsPerPage = 10;

        // Get form Details at onload 
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
            });
            $scope.loadmodulefeedback();
        };

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
        };

        $scope.employeeSalaryslipDetails = [];
        $scope.institutionDetails = {};
        $scope.EmployeeDis = false;
        //Search employee
        $scope.submitted = false;
        $scope.SearchEmployee = function () {
            $scope.EmployeeDis = false;
            $scope.submitted = true;
            if ($scope.myForm.$valid) {

                $scope.institutionDetails = {};
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

                var data = {
                    "HRES_Year": $scope.Employee.hreS_Year,
                    "HRES_Month": $scope.Employee.hreS_Month,
                    groupTypeIdList: groupTypeselected,
                    hrmD_IdList: departmentselected,
                    hrmdeS_IdList: designationselected,
                    "comm": "0"
                };

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

                            $scope.earningheadlist = $scope.employeeSalaryslipDetails[0].earningresult;

                            angular.forEach($scope.earningheadlist, function (headresult) {
                                headresult.netamount = 0;
                            });

                            $scope.deductionheadlist = $scope.employeeSalaryslipDetails[0].deductionresult;

                            angular.forEach($scope.deductionheadlist, function (headresult) {
                                headresult.netamount = 0;
                            });
                            var fname = "";
                            var mname = "";
                            var Lname = "";

                            angular.forEach($scope.employeeSalaryslipDetails, function (itm) {
                                //fname = itm.hrmE_EmployeeFirstName.length > 0 ? itm.hrmE_EmployeeFirstName : '';
                                //mname = itm.hrmE_EmployeeMiddleName.length > 0 ? itm.hrmE_EmployeeMiddleName : '';
                                //Lname = itm.hrmE_EmployeeLastName.length > 0 ? itm.hrmE_EmployeeLastName : '';


                                itm.hrmE_EmployeeFirstName = itm.hrmE_EmployeeFirstName;
                                //itm.hrmE_EmployeeFirstName = fname + " " + mname + " " + Lname;



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

                                itm.deductionresult.push({ hrmeD_Name: "PF GROSS", hrmeD_Id: 994 });
                                itm.deductionresult.push({ hrmeD_Name: "PENSION", hrmeD_Id: 995 });
                                itm.deductionresult.push({ hrmeD_Name: "SCHOOL PF", hrmeD_Id: 996 });
                                itm.deductionresult.push({ hrmeD_Name: "PF SAL", hrmeD_Id: 997 });
                                itm.deductionresult.push({ hrmeD_Name: "EDLI SAL", hrmeD_Id: 998 });
                                itm.deductionresult.push({ hrmeD_Name: "PENSION SAL", hrmeD_Id: 999 });

                                //deduction head totalAmount
                                angular.forEach(itm.deductionresult, function (result) {
                                    if (result.hrmeD_Name == "PF GROSS") {
                                        result.hresD_Amount = 0;
                                        angular.forEach(itm.earningresult, function (calc) {
                                            if (calc.hrmeD_Name == "Basic Pay" || calc.hrmeD_Name == "DA" || calc.hrmeD_Name == "PERSONAL PAY" || calc.hrmeD_Name == "CL AMT") {
                                                result.hresD_Amount = result.hresD_Amount + calc.hresD_Amount;
                                            }
                                        });
                                    }

                                    else if (result.hrmeD_Name == "PENSION") {
                                        result.hresD_Amount = 0;
                                        $scope.sumcondition = 0;
                                        angular.forEach(itm.earningresult, function (calc) {
                                            if (calc.hrmeD_Name == "Basic Pay" || calc.hrmeD_Name == "DA" || calc.hrmeD_Name == "PERSONAL PAY" || calc.hrmeD_Name == "CL AMT") {
                                                $scope.sumcondition = $scope.sumcondition + calc.hresD_Amount;
                                            }
                                        });

                                        //if ($scope.sumcondition >= 15000) {
                                        //    var joiningDate = new Date(itm.hrmE_DOJ);
                                        //    //var checkingDate = new Date("2003-01-31");
                                        //    var checkingDate = new Date("2014-08-31");

                                        //    if (itm.hrmE_EmployeeCode == "303" || itm.hrmE_EmployeeCode == "308" || itm.hrmE_EmployeeCode == "319" || itm.hrmE_EmployeeCode == "321" || itm.hrmE_EmployeeCode == "322" || itm.hrmE_EmployeeCode == "324" || itm.hrmE_EmployeeCode == "336" || itm.hrmE_EmployeeCode == "302" || itm.hrmE_EmployeeCode == "301" || itm.hrmE_EmployeeCode == "334" || itm.hrmE_EmployeeCode == "293" || itm.hrmE_EmployeeCode == "318" || itm.hrmE_EmployeeCode == "326"|| itm.hrmE_EmployeeCode == "329" || itm.hrmE_EmployeeCode == "311" || itm.hrmE_EmployeeCode == "312" || itm.hrmE_EmployeeCode == "337") {
                                        //        result.hresD_Amount = 1250.00;
                                        //    }
                                        //    else if (joiningDate > checkingDate && (itm.hrmE_EmployeeCode != '242' && itm.hrmE_EmployeeCode != '237' && itm.hrmE_EmployeeCode != '70')) {
                                        //        result.hresD_Amount = 0.00;
                                        //    }
                                        //    else
                                        //    {
                                        //        if (parseInt(itm.hrmE_age) >= 58) { result.hresD_Amount = 0.00; }
                                        //        else { result.hresD_Amount = 1250.00; }
                                        //    }
                                        //    //result.hresD_Amount = 1250.00;
                                        //}
                                        //else {
                                        //    if (parseInt(itm.hrmE_age) > 58) { result.hresD_Amount = 0.00; }
                                        //    else { result.hresD_Amount = Math.round((8.33 / 100) * $scope.sumcondition, 2); }
                                        //    //result.hresD_Amount = (8.33 / 100) * $scope.sumcondition;
                                        //}
                                        ////temp.schoolpf = 1800 - temp.pensionFund;


                                        if (parseInt(itm.hrmE_age) >= 58) {
                                            if (itm.hreS_WorkingDays > 0) {
                                                result.hresD_Amount = Math.round((1250 * itm.hreS_WorkingDays) / 30, 2);
                                            }
                                            else {
                                                result.hresD_Amount = 0.00;
                                            }
                                        }
                                        else if ($scope.sumcondition < 15000 && itm.hrmE_FPFNotApplicableFlg == true) {
                                            result.hresD_Amount = Math.round((8.33 / 100) * $scope.sumcondition, 2);

                                        }
                                        else if (itm.hrmE_FPFNotApplicableFlg == true) {
                                            result.hresD_Amount = 1250.00;
                                        }
                                        else {
                                            result.hresD_Amount = 0.00;
                                        }




                                    }


                                    else if (result.hrmeD_Name == "SCHOOL PF") {
                                        result.hresD_Amount = 0;
                                        $scope.sumcondition = 0;
                                        $scope.pensionFund = 0;
                                        $scope.stjOwnPF = 0;

                                        angular.forEach(itm.deductionresult, function (calc) {
                                            if (calc.hrmeD_Name == "P F") {
                                                $scope.stjOwnPF = calc.hresD_Amount;
                                            }
                                        });

                                        angular.forEach(itm.earningresult, function (calc) {
                                            if (calc.hrmeD_Name == "Basic Pay" || calc.hrmeD_Name == "DA" || calc.hrmeD_Name == "PERSONAL PAY" || calc.hrmeD_Name == "CL AMT") {
                                                $scope.sumcondition = $scope.sumcondition + calc.hresD_Amount;
                                            }
                                        });

                                        if (parseInt(itm.hrmE_age) >= 58) { $scope.pensionFund = 0.00; }
                                        else if ($scope.sumcondition < 15000 && itm.hrmE_FPFNotApplicableFlg == true) {
                                            // $scope.pensionFund = Math.round((8.33 / 100) * $scope.sumcondition)
                                            $scope.pensionFund = Math.round((8.33 / 100) * $scope.sumcondition, 2);

                                        }
                                        else if (itm.hrmE_FPFNotApplicableFlg == true) {
                                            $scope.pensionFund = 1250.00;
                                        }
                                        else {
                                            $scope.pensionFund = 0.00;
                                        }


                                        if (itm.hrmE_FPFNotApplicableFlg == false) {

                                            result.hresD_Amount = $scope.stjOwnPF;
                                        }
                                        else {
                                            //temp.stjOwnPF = (temp.schoolpf + temp.pensionFund); // temp.stjOwnPF; 
                                            result.hresD_Amount = $scope.stjOwnPF - $scope.pensionFund;
                                        }



                                        //if (itm.hrmE_age <= '58') {

                                        //    angular.forEach(itm.deductionresult, function (calc) {
                                        //        if (calc.hrmeD_Name == "P F") {
                                        //            $scope.stjOwnPF = calc.hresD_Amount;
                                        //        }
                                        //    });

                                        //    angular.forEach(itm.earningresult, function (calc) {
                                        //        if (calc.hrmeD_Name == "Basic Pay" || calc.hrmeD_Name == "DA" || calc.hrmeD_Name == "PERSONAL PAY" || calc.hrmeD_Name == "CL AMT") {
                                        //            $scope.sumcondition = $scope.sumcondition + calc.hresD_Amount;
                                        //        }
                                        //    });

                                        //    if ($scope.sumcondition >= 15000) {
                                        //        var joiningDate = new Date(itm.hrmE_DOJ);
                                        //        //var checkingDate = new Date("2003-01-31");
                                        //        var checkingDate = new Date("2014-08-31");

                                        //        if (itm.hrmE_EmployeeCode == "303" || itm.hrmE_EmployeeCode == "308" || itm.hrmE_EmployeeCode == "319" || itm.hrmE_EmployeeCode == "321" || itm.hrmE_EmployeeCode == "322" || itm.hrmE_EmployeeCode == "324" || itm.hrmE_EmployeeCode == "336" || itm.hrmE_EmployeeCode == "302" || itm.hrmE_EmployeeCode == "301" || itm.hrmE_EmployeeCode == "334" || itm.hrmE_EmployeeCode == "293" || itm.hrmE_EmployeeCode == "318" || itm.hrmE_EmployeeCode == "326" || itm.hrmE_EmployeeCode == "236" || itm.hrmE_EmployeeCode == "329" || itm.hrmE_EmployeeCode == "311" || itm.hrmE_EmployeeCode == "312" || itm.hrmE_EmployeeCode == "337") {
                                        //            $scope.pensionFund = 1250.00;
                                        //        }
                                        //        else if (joiningDate > checkingDate && (itm.hrmE_EmployeeCode != '242' && itm.hrmE_EmployeeCode != '237' && itm.hrmE_EmployeeCode != '70')) {
                                        //            $scope.pensionFund = 0.00;
                                        //        }
                                        //        else {
                                        //            if (parseInt(itm.hrmE_age) >= 58)
                                        //            { $scope.pensionFund = 0.00; }
                                        //            else
                                        //            { $scope.pensionFund = 1250.00; }
                                        //        }
                                        //        //$scope.pensionFund = 1250.00;
                                        //    }
                                        //    else {
                                        //        if (parseInt(itm.hrmE_age) >= 58) { $scope.pensionFund = 0.00; }
                                        //        else { $scope.pensionFund = Math.round((8.33 / 100) * $scope.sumcondition, 2); }
                                        //        //$scope.pensionFund = (8.33 / 100) * $scope.sumcondition;
                                        //    }
                                        //}
                                        //else {
                                        //    angular.forEach(itm.earningresult, function (calc) {
                                        //        if (calc.hrmeD_Name == "Basic Pay" || calc.hrmeD_Name == "DA" || calc.hrmeD_Name == "PERSONAL PAY" || calc.hrmeD_Name == "CL AMT") {
                                        //            $scope.sumcondition = $scope.sumcondition + calc.hresD_Amount;
                                        //        }
                                        //    });

                                        //    if ($scope.sumcondition >= 15000) {
                                        //        var joiningDate = new Date(itm.hrmE_DOJ);
                                        //        //var checkingDate = new Date("2003-01-31");
                                        //        var checkingDate = new Date("2014-08-31");

                                        //        if (itm.hrmE_EmployeeCode == "303" || itm.hrmE_EmployeeCode == "308" || itm.hrmE_EmployeeCode == "319" || itm.hrmE_EmployeeCode == "321" || itm.hrmE_EmployeeCode == "322" || itm.hrmE_EmployeeCode == "324" || itm.hrmE_EmployeeCode == "336" || itm.hrmE_EmployeeCode == "302" || itm.hrmE_EmployeeCode == "301" || itm.hrmE_EmployeeCode == "334" || itm.hrmE_EmployeeCode == "293" || itm.hrmE_EmployeeCode == "318" || itm.hrmE_EmployeeCode == "326" || itm.hrmE_EmployeeCode == "236" || itm.hrmE_EmployeeCode == "329" || itm.hrmE_EmployeeCode == "311" || itm.hrmE_EmployeeCode == "312" || itm.hrmE_EmployeeCode == "337") {
                                        //            $scope.pensionFund = 1250.00;
                                        //        }
                                        //        else if (joiningDate > checkingDate && (itm.hrmE_EmployeeCode != '242' && itm.hrmE_EmployeeCode != '237' && itm.hrmE_EmployeeCode != '70')) {
                                        //            $scope.pensionFund = 0.00;
                                        //        }
                                        //        else {
                                        //            if (parseInt(itm.hrmE_age) >= 58) { $scope.pensionFund = 0.00; }
                                        //            else { $scope.pensionFund = 1250.00; }
                                        //        }
                                        //        //$scope.pensionFund = 1250.00;
                                        //    }
                                        //    else {
                                        //        if (parseInt(itm.hrmE_age) >= 58) { $scope.pensionFund = 0.00; }
                                        //        else { $scope.pensionFund = Math.round((8.33 / 100) * $scope.sumcondition, 2); }
                                        //        //$scope.pensionFund = (8.33 / 100) * $scope.sumcondition;
                                        //    }

                                        //    $scope.sumcondition = 0;
                                        //    angular.forEach(itm.deductionresult, function (calc) {
                                        //        if (calc.hrmeD_Name == "P F") {
                                        //            $scope.stjOwnPF = calc.hresD_Amount;
                                        //        }
                                        //    });
                                        //}

                                        ////result.hresD_Amount = 1800 - $scope.pensionFund;
                                        //result.hresD_Amount = $scope.stjOwnPF - $scope.pensionFund;
                                    }

                                    else if (result.hrmeD_Name == "PF SAL") {
                                        $scope.pfamount = 0;
                                        $scope.sumamount = 0;

                                        angular.forEach(itm.deductionresult, function (calc) {
                                            if (calc.hrmeD_Name == "P F") {
                                                $scope.pfamount = calc.hresD_Amount;
                                            }
                                        });

                                        //result.hresD_Amount = 0;
                                        angular.forEach(itm.earningresult, function (calc) {
                                            if (calc.hrmeD_Name == "Basic Pay" || calc.hrmeD_Name == "DA" || calc.hrmeD_Name == "PERSONAL PAY" || calc.hrmeD_Name == "CL AMT") {
                                                $scope.sumamount = $scope.sumamount + calc.hresD_Amount;
                                            }
                                        });

                                        if ($scope.pfamount > 1800) {
                                            result.hresD_Amount = $scope.sumamount;
                                        }
                                        else if ($scope.pfamount < 1800) {
                                            result.hresD_Amount = $scope.sumamount;
                                        }
                                        else if ($scope.pfamount == 1800) {
                                            result.hresD_Amount = 15000;
                                        }
                                        else if ($scope.pfamount == 0 || $scope.pfamount == null) {
                                            result.hresD_Amount = 0;
                                        }
                                    }
                                    else if (result.hrmeD_Name == "EDLI SAL") {
                                        result.hresD_Amount = 0;
                                        $scope.sumcondition = 0;
                                        angular.forEach(itm.earningresult, function (calc) {
                                            if (calc.hrmeD_Name == "Basic Pay" || calc.hrmeD_Name == "DA" || calc.hrmeD_Name == "PERSONAL PAY" || calc.hrmeD_Name == "CL AMT") {
                                                $scope.sumcondition = $scope.sumcondition + calc.hresD_Amount;
                                            }
                                        });
                                        if ($scope.sumcondition > 15000) { result.hresD_Amount = 15000; }
                                        else { result.hresD_Amount = $scope.sumcondition; }
                                    }
                                    else if (result.hrmeD_Name == "PENSION SAL") {
                                        result.hresD_Amount = 0;
                                        $scope.sumcondition = 0;
                                        $scope.pension = 0;

                                        angular.forEach(itm.earningresult, function (calc) {
                                            if (calc.hrmeD_Name == "Basic Pay" || calc.hrmeD_Name == "DA" || calc.hrmeD_Name == "PERSONAL PAY" || calc.hrmeD_Name == "CL AMT") {
                                                $scope.sumcondition = $scope.sumcondition + calc.hresD_Amount;
                                            }
                                        });
                                        if ($scope.sumcondition >= 15000) { result.hresD_Amount = 15000; }
                                        else { result.hresD_Amount = $scope.sumcondition; }

                                        //if ($scope.sumcondition >= 15000) {
                                        //    var joiningDate = new Date(itm.hrmE_DOJ);
                                        //    //var checkingDate = new Date("2003-01-31");
                                        //    var checkingDate = new Date("2014-08-31");

                                        //    if (itm.hrmE_EmployeeCode == "303" || itm.hrmE_EmployeeCode == "308" || itm.hrmE_EmployeeCode == "319" || itm.hrmE_EmployeeCode == "321" || itm.hrmE_EmployeeCode == "322" || itm.hrmE_EmployeeCode == "324" || itm.hrmE_EmployeeCode == "336" || itm.hrmE_EmployeeCode == "302" || itm.hrmE_EmployeeCode == "301" || itm.hrmE_EmployeeCode == "334" || itm.hrmE_EmployeeCode == "293" || itm.hrmE_EmployeeCode == "318" || itm.hrmE_EmployeeCode == "326" || itm.hrmE_EmployeeCode == "329" || itm.hrmE_EmployeeCode == "311" || itm.hrmE_EmployeeCode == "312" || itm.hrmE_EmployeeCode == "337") {
                                        //        $scope.pension = 1250.00;
                                        //    }
                                        //    else if (joiningDate > checkingDate && (itm.hrmE_EmployeeCode != '242' && itm.hrmE_EmployeeCode != '237' && itm.hrmE_EmployeeCode != '70')) {
                                        //        $scope.pension = 0.00;
                                        //    }
                                        //    else {
                                        //        if (parseInt(itm.hrmE_age) > 58) { $scope.pension = 0.00; }
                                        //        else { $scope.pension = 1250.00; }
                                        //    }
                                        //}
                                        //else {
                                        //    if (parseInt(itm.hrmE_age) > 58) { $scope.pension = 0.00; }
                                        //    else { $scope.pension = Math.round((8.33 / 100) * $scope.sumcondition, 2); }
                                        //}


                                        if (parseInt(itm.hrmE_age) >= 58) { $scope.pension = 0.00; }
                                        else if ($scope.sumcondition < 15000 && itm.hrmE_FPFNotApplicableFlg == true) {
                                            $scope.pension = Math.round((8.33 / 100) * $scope.sumcondition)

                                        }
                                        else if (itm.hrmE_FPFNotApplicableFlg == true) {
                                            $scope.pension = 1250.00;
                                        }
                                        else {
                                            $scope.pension = 0.00;
                                        }
                                        if ($scope.pension == 0) {
                                            result.hresD_Amount = 0;
                                        }
                                    }
                                    else {
                                        result.hresD_Amount = Math.round(result.hresD_Amount);
                                        angular.forEach($scope.deductionheadlist, function (headresult) {
                                            if (headresult.hrmeD_Name == result.hrmeD_Name) {
                                                headresult.netamount = Math.round(headresult.netamount + result.hresD_Amount);
                                            }
                                        });
                                    }
                                });
                            });









                            $scope.earnlen = $scope.earningheadlist.length;
                            $scope.dedlen = $scope.deductionheadlist.length;

                            $scope.coltotalarray = [];
                            $scope.earnhead = [];
                            angular.forEach($scope.earningheadlist, function (ww) {
                                $scope.earnhead.push({ title: ww.hrmeD_Name, field: 'id' + ww.hrmeD_Id, width: 100, aggregates: ["sum"], footerTemplate: "#=sum#" });
                                $scope.coltotalarray.push({ name: 'id' + ww.hrmeD_Id, field: 'id' + ww.hrmeD_Id, aggregate: "sum" });
                            });

                            $scope.coltotalarray.push({ name: 'grossEarning', field: 'grossEarning', aggregate: "sum" });

                            $scope.dedhead = [];
                            angular.forEach($scope.deductionheadlist, function (www) {
                                $scope.dedhead.push({ title: www.hrmeD_Name, field: 'id' + www.hrmeD_Id, width: 100, aggregates: ["sum"], footerTemplate: "#=sum#" });
                                $scope.coltotalarray.push({ name: 'id' + www.hrmeD_Id, field: 'id' + www.hrmeD_Id, aggregate: "sum" });
                            });

                            $scope.coltotalarray.push({ name: 'grossDeduction', field: 'grossDeduction', aggregate: "sum" });

                            angular.forEach($scope.employeeSalaryslipDetails, function (rr) {
                                angular.forEach(rr.earningresult, function (ff) {
                                    rr['id' + ff.hrmeD_Id] = ff.hresD_Amount;
                                });

                                angular.forEach(rr.deductionresult, function (ff) {
                                    rr['id' + ff.hrmeD_Id] = ff.hresD_Amount;
                                });
                            });

                            $scope.coltotalarray.push({ name: 'netSalary', field: 'netSalary', aggregate: "sum" });

                            //KINDO IMPLEMENT
                            $scope.colarrayall = [];
                            $scope.colarrayall = [
                                { title: 'Sl.No', template: "<span class='row-number'></span>", width: 100 },
                                { name: 'hrmE_EmployeeCode', field: 'hrmE_EmployeeCode', title: 'Employee Code', width: 200 },
                                { name: 'hrmE_EmployeeFirstName', field: 'hrmE_EmployeeFirstName', title: 'Employee Name', width: 200 },
                                //{ name: 'hrmdeS_Designationname', field: 'hrmdeS_Designationname', title: 'Designation', width: 200 },
                                //{ name: 'hreS_WorkingDays', field: 'hreS_WorkingDays', title: 'Worked Days', width: 200 },
                                { name: 'hrmE_PFAccNo', field: 'hrmE_PFAccNo', title: 'PF Acc No', width: 200 },
                                { name: 'Earnings', field: 'Earnings', title: 'Earnings', columns: $scope.earnhead, aggregates: ["sum"], footerTemplate: "#=sum#", groupFooterTemplate: " #=sum#" },
                                { name: 'grossEarning', field: 'grossEarning', title: 'Gross Earning', width: 200, aggregates: ["sum"], footerTemplate: "#=sum#", groupFooterTemplate: " #=sum#" },
                                { name: 'Deductions', field: 'Deductions', title: 'Deductions', columns: $scope.dedhead, aggregates: ["sum"], footerTemplate: "#=sum#", groupFooterTemplate: " #=sum#" },
                                { name: 'grossDeduction', field: 'grossDeduction', title: 'Gross Deduction', width: 200, aggregates: ["sum"], footerTemplate: "#=sum#", groupFooterTemplate: " #=sum#" },
                                { name: 'netSalary', field: 'netSalary', title: 'Net Salary', width: 200, aggregates: ["sum"], footerTemplate: "#=sum#", groupFooterTemplate: " #=sum#" }
                            ];

                            $(document).ready(function () {
                                $('#kindogridhhs').empty();
                                $("#kindogridhhs").kendoGrid({
                                    toolbar: ["excel"],

                                    excel: {
                                        fileName: "ConsolidatedReport.xls",
                                        //allPages: true,
                                        proxyURL: "",
                                        filterable: true,
                                        allPages: true
                                    },

                                    excelExport: function (e) {

                                        var sheet = e.workbook.sheets[0];
                                        sheet.frozenRows = 2;
                                        sheet.name = "CumulativeSalaryReport";

                                        var myHeaders = [{
                                            value: "Cumulative Salary Report For the Month of - " + $scope.HRES_Month + " - " + $scope.HRES_Year,
                                            fontSize: 20,
                                            textAlign: "center",
                                            background: "#ffffff",
                                            color: "black"
                                        }];

                                        sheet.rows.splice(0, 0, { cells: myHeaders, type: "header", height: 70 });
                                    },

                                    dataSource: {
                                        data: $scope.employeeSalaryslipDetails,
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
                        }
                        else {
                            $scope.EmployeeDis = false;
                            swal('No Record found to display .. !');
                            return;
                        }

                    });
            }

        };



        //By group Type
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
        $scope.exportToExcel = function (tableId) {

            //var exportHref = Excel.tableToExcel(tableId, 'sheet name');
            var exportHref = Excel.tableToExcel(tableId, 'SalaryReport');

            $timeout(function () { location.href = exportHref; }, 100);
            $timeout(function () {
                location.href = exportHref;
            }, 100);
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


    }


})();
