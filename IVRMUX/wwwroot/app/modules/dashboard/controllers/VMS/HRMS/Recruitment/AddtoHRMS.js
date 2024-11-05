(function () {
    'use strict';
    angular
        .module('app')
        .controller('addtoHRMSController', addtoHRMSController);

    addtoHRMSController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', '$filter'];
    function addtoHRMSController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, $filter) {

        $scope.EarningDet = {};
        $scope.addjob = {};
        $scope.addjob.hrjD_Id = 0;
        $scope.submitted = true;
        $scope.CandidateView = false;

        $scope.onLoadGetData = function () {
            var pageid = 2;
            apiService.getURI("AddtoHRMS/getalldetails", pageid).then(function (promise) {
                $scope.candidatelist = promise.candidatelist;
                $scope.employeetypelist = promise.masterEmployeetype;
                $scope.grouptypelist = promise.groupTypeList;
                $scope.employeegradelist = promise.gradeList;
                $scope.maritalstatuslist = promise.maritalstatuslist;
                $scope.religionlist = promise.masterreligionlist;
                $scope.casteCategorylist = promise.castecategorylist;
                $scope.castelist = promise.castelist;
                $scope.employeedepartmentlist = promise.employeedepartmentlist;
                $scope.employeedesignationlist = promise.employeedesignationlist;
            });
        };

        // clear form data
        $scope.cancel = function () {
            $state.reload();
        };

        $scope.submitted = false;
        $scope.interacted1 = function (field) {
            return $scope.submitted;
        };


        //==============================save data
        $scope.savejob = function () {

            if ($scope.myForm.$valid) {
                $scope.mrfCand.hrmqC_RecruitmentStatus = "New";
                var data = $scope.mrfCand;
                apiService.create("Appointment/", data).
                    then(function (promise) {
                        if (promise.retrunMsg !== "") {

                            if (promise.retrunMsg === "Duplicate") {
                                swal("Record already exist..!!");
                                return;
                            }

                            else if (promise.retrunMsg === "false") {
                                swal("Record Not saved / Updated..", 'Fail');

                            }
                            else if (promise.retrunMsg === "Add") {
                                swal("Record Saved Successfully..");
                            }
                            else if (promise.retrunMsg === "Update") {
                                swal("Record Updated Successfully..");
                            }
                            else {
                                swal("Something went wrong ..!", 'Kindly contact Administrator');
                            }

                            //if (promise.employeeTypeList !== null && promise.employeeTypeList.length > 0) {
                            //    $scope.gridOptionsEmployeeType.data = promise.employeeTypeList;
                            //}
                            $scope.cancel();
                        }
                    });
            }
            else {
                $scope.submitted = true;
            }
            
        };


        //=============================Get Designation List
        $scope.Get_Desgination = function (hrmD_Id) {
            var data = {
                "HRMD_Id": hrmD_Id
            };

            apiService.create("Appointment/Get_Desgination", data).then(function (promise) {
                $scope.desgnationlist = promise.desgnationlist;
            });
        };

        $scope.getEmployeeSalaryDetailsByHead = function (data) {
            $scope.submittedSalarydetails = true;
            //if ($scope.myForm1.$valid) {
                $scope.netSalary = 0;
                $scope.EarningTotal = 0;
                $scope.DeductionTotal = 0;
                $scope.ArrearTotal = 0;

                if ($scope.Candidate === undefined || $scope.Candidate == null) {
                    swal("Select Candidate..!!");
                    return;
                }

                data.hrcD_Id = $scope.Candidate.hrcD_Id;
                apiService.create("Appointment/getEmployeeSalaryDetailsByHead", data).
                    then(function (promise) {
                        //earning & deduction details
                        //$scope.loaddata(data.hrcD_Id);
                        //var a = promise.employeeEarningsDeductionsDetails[1].hrceD_Amount;
                        if (promise.employeeEarningsDeductionsDetails != null && promise.employeeEarningsDeductionsDetails.length > 0) {
                            var employeeEarningsDeductionsDetails = promise.employeeEarningsDeductionsDetails;
                            var EarningDetails = $filter('filter')(promise.employeeEarningsDeductionsDetails, function (d) {
                                return d.hrmeD_EarnDedFlag === 'Earning';
                            });
                            var DeductionDetails = $filter('filter')(promise.employeeEarningsDeductionsDetails, function (d) {
                                return d.hrmeD_EarnDedFlag === 'Deduction';
                            });
                            var ArrearDetails = $filter('filter')(promise.employeeEarningsDeductionsDetails, function (d) {
                                return d.hrmeD_EarnDedFlag === 'Arrear';
                            });
                            var GrossDetails = $filter('filter')(promise.employeeEarningsDeductionsDetails, function (d) {
                                return d.hrmeD_EarnDedFlag === 'Gross';
                            });
                            if (GrossDetails != null && GrossDetails.length > 0) {
                                $scope.Salary = GrossDetails[0];
                                 $scope.Salary.hrceD_ActiveFlag = GrossDetails[0].hrceD_ActiveFlag;

                            } else {
                                $scope.Salary.hrceD_Percentage = '0.00';
                                $scope.Salary.hrceD_Amount = '0.00';
                                $scope.Salary.hrmeD_AppPercent = '0.00';
                                $scope.Salary.hrmeD_Percent = '0.00';
                                $scope.Salary.hrmeD_Details = '';
                                $scope.Salary.hrmeD_Id = 0;
                                $scope.Salary.hrceD_ActiveFlag = true;
                            }

                            var totalEarning = 0;
                            angular.forEach($scope.earningList, function (value, key) {
                                angular.forEach(EarningDetails, function (value1, key1) {
                                    if (value.hrmeD_Id == value1.hrmeD_Id) {
                                        $scope.earningList[key].hrceD_ActiveFlag = value1.hrceD_ActiveFlag;
                                        $scope.earningList[key].hrceD_Id = value1.hrceD_Id;
                                        $scope.earningList[key].hrceD_Amount = value1.hrceD_Amount;
                                        $scope.earningList[key].hrceD_Percentage = value1.hrceD_Percentage;
                                        $scope.earningList[key].hrmE_Id = value1.hrmE_Id;
                                        totalEarning = totalEarning + value1.hrceD_Amount;
                                    }
                                });
                            });

                            var totalDeduction = 0;
                            angular.forEach($scope.detectionList, function (value, key) {
                                angular.forEach(DeductionDetails, function (value1, key1) {
                                    if (value.hrmeD_Id == value1.hrmeD_Id) {
                                        $scope.detectionList[key].hrceD_ActiveFlag = value1.hrceD_ActiveFlag;
                                        $scope.detectionList[key].hrceD_Id = value1.hrceD_Id;
                                        $scope.detectionList[key].hrceD_Amount = value1.hrceD_Amount;
                                        $scope.detectionList[key].hrceD_Percentage = value1.hrceD_Percentage;
                                        $scope.detectionList[key].hrmE_Id = value1.hrmE_Id;
                                        totalDeduction = totalDeduction + value1.hrceD_Amount;
                                    }
                                });
                            });

                            var totalArrear = 0;
                            angular.forEach($scope.arrearList, function (value, key) {
                                angular.forEach(ArrearDetails, function (value1, key1) {
                                    if (value.hrmeD_Id == value1.hrmeD_Id) {
                                        $scope.arrearList[key].hrceD_ActiveFlag = value1.hrceD_ActiveFlag;
                                        $scope.arrearList[key].hrceD_Id = value1.hrceD_Id;
                                        $scope.arrearList[key].hrceD_Amount = value1.hrceD_Amount;
                                        $scope.arrearList[key].hrceD_Percentage = value1.hrceD_Percentage;
                                        $scope.arrearList[key].hrmE_Id = value1.hrmE_Id;
                                        totalArrear = totalArrear + value1.hrceD_Amount;
                                    }
                                });
                            });

                            $scope.EarningTotal = $scope.getEarningTotal();
                            $scope.DeductionTotal = $scope.getDeductionTotal();
                            $scope.ArrearTotal = $scope.getArrearTotal();
                            $scope.netSalary = ($scope.EarningTotal + $scope.ArrearTotal) - $scope.DeductionTotal;
                        }
                    });
            //}
        };

        //===========================Save Appointment data
        $scope.saveAddtohrms = function (listofdata) {
            if ($scope.myForm1.$valid) { 
                var Template = document.getElementById("welcomeletter").innerHTML;
                var data = {
                    Employeedto: $scope.Candidate,
                    Template: Template
                };
                apiService.create("AddtoHRMS/savedata", data).then(function (promise) {
                    if (promise.retrunMsg !== "") {
                        if (promise.retrunMsg == "Duplicate") {
                            swal("Record already exist..!!");
                            return;
                        }
                        else if (promise.retrunMsg === "false") {
                            swal("Record Not saved / Updated..", 'Fail');
                        }
                        else if (promise.retrunMsg === "Add") {
                            swal("Record Saved Successfully..");
                            $scope.cancel();
                        }
                        else if (promise.retrunMsg === "Update") {
                            swal("Record Updated Successfully..");
                            $scope.cancel();
                        }
                        else {
                            swal("Something went wrong ..!", 'Kindly contact Administrator');
                        }
                        $scope.cancel();
                    }
                    });
            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.addtohrms = function () {
            if ($scope.myForm1.$valid) {
                var selectedEarning = [];
                if ($scope.Salary.hrmeD_Id > 0) {
                    selectedEarning.push($scope.Salary);
                }
                else {
                    $scope.Salary.hrceD_Percentage = '0.00';
                    $scope.Salary.hrceD_Amount = '0.00';
                    $scope.Salary.hrmeD_AppPercent = '0.00';
                    $scope.Salary.hrmeD_Percent = '0.00';
                    $scope.Salary.hrmeD_Details = '';
                    $scope.Salary.hrmeD_Id = 0;
                    $scope.Salary.hrceD_ActiveFlag = true;
                }

                if ($scope.earningList !== null && $scope.earningList.length > 0) {
                    angular.forEach($scope.earningList, function (earning) {
                        if (earning.hrceD_ActiveFlag) {
                            earning.hrceD_ActiveFlag = true;
                        } else {
                            earning.hrceD_ActiveFlag = false;
                        }
                        selectedEarning.push(earning);
                    });
                }

                var selectedDetection = [];
                if ($scope.detectionList !== null && $scope.detectionList.length > 0) {
                    angular.forEach($scope.detectionList, function (detection) {
                        if (detection.hrceD_ActiveFlag) {
                            detection.hrceD_ActiveFlag = true;
                        } else {
                            detection.hrceD_ActiveFlag = false;
                        }
                        selectedDetection.push(detection);
                    });
                }

                var selectedArrear = [];
                if ($scope.arrearList !== null && $scope.arrearList.length > 0) {
                    angular.forEach($scope.arrearList, function (arrear) {
                        if (arrear.hrceD_ActiveFlag) {
                            arrear.hrceD_ActiveFlag = true;
                        } else {
                            arrear.hrceD_ActiveFlag = false;
                        }
                        selectedArrear.push(arrear);
                    });
                }

                if (selectedEarning.length === 0 && selectedDetection.length === 0) {
                    swal('Kindly select atleast one record from Earning / Deduction');
                    return;
                }

                var data = {
                    Employeedto: $scope.Candidate,
                    EarningDTO: selectedEarning,
                    DeductionDTO: selectedDetection,
                    ArrearDTO: selectedArrear
                };

                apiService.create("Appointment/savetohrms", data).then(function (promise) {
                    if (promise.retrunMsg !== "") {
                        if (promise.retrunMsg == "Duplicate") {
                            swal("Record already exist..!!");
                            return;
                        }
                        else if (promise.retrunMsg === "false") {
                            swal("Record Not saved / Updated..", 'Fail');
                        }
                        else if (promise.retrunMsg === "Add") {
                            swal("Record Saved Successfully..");
                            $scope.cancel();
                        }
                        else if (promise.retrunMsg === "Update") {
                            swal("Record Updated Successfully..");
                            $scope.cancel();
                        }
                        else {
                            swal("Something went wrong ..!", 'Kindly contact Administrator');
                        }
                        $scope.cancel();
                    }
                });
            }
        };

        var buttonenable = false;
        $scope.loaddata = function (cand) {
            var data = {
                "HRCD_Id": cand
            };
            //var data = candidate;

            apiService.create("Appointment/getcandidate", data).then(function (promise) {
                if (promise.retrunMsg == "") {
                    $scope.currentdate = new Date();
                    $scope.Candidate = promise.candidateDetails[0];
                    //$scope.Candidate.hrmD_Id = promise.candidateDetails[0].hrcD_Department;
                    //$scope.Candidate.hrmdeS_Id = promise.candidateDetails[0].hrcD_Designation;
                    $scope.CandidateView = true;
                    $scope.imagepath = "";
                    $scope.jobpost = promise.hrmJ_JobTiTle;
                    //string imagepath = "";
                    if (promise.mI_Id == 16) { $scope.imagepath = "https://dcampusstrg.blob.core.windows.net/logos/Letter-2.jpg"; }
                    else if (promise.mI_Id == 17) { $scope.imagepath = "https://dcampusstrg.blob.core.windows.net/logos/Letter-1.jpg"; }
                    else if (promise.mI_Id == 20) { $scope.imagepath = "https://dcampusstrg.blob.core.windows.net/logos/Letter-4.jpg"; }
                    else if (promise.mI_Id == 21) { $scope.imagepath = "https://dcampusstrg.blob.core.windows.net/logos/Letter-5.jpg"; }
                    else if (promise.mI_Id == 22) { $scope.imagepath = "https://dcampusstrg.blob.core.windows.net/logos/Letter-3.jpg"; }
                    else if (promise.mI_Id == 24) { $scope.imagepath = "";}
                }
            });
        };

        $scope.getaddress = function () {
            if ($scope.Candidate.copyaddress == true) {
                $scope.Candidate.hrcD_AddressPermanent = $scope.Candidate.hrcD_AddressLocal;
            }
        };

        $scope.printToCart = function (vapsappointment) {
            var innerContents = document.getElementById("vapsappointment").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/pfchallan/PFChallanPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };

        $scope.CancelAppointment = function () {
            $state.reload();
        };

        $scope.printvapssalary = function (vapssalary) {
            var innerContents = document.getElementById("vapssalary").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/pfchallan/PFChallanPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };

        $scope.Cancelsalary = function () {
            $state.reload();
        };

        $scope.printvapmou = function (vapsmou) {
            var innerContents = document.getElementById("vapsmou").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/pfchallan/PFChallanPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };

        $scope.Cancelmou = function () {
            $state.reload();
        };

        $scope.printvapnda = function (vapsnda) {
            var innerContents = document.getElementById("vapsnda").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/pfchallan/PFChallanPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };

        $scope.Cancelnda = function () {
            $state.reload();
        };

        $scope.getEarningTotal = function () {
            var total = 0;
            for (var i = 0; i < $scope.earningList.length; i++) {
                if ($scope.earningList[i].hrceD_ActiveFlag) {
                    var product = $scope.earningList[i];
                    total += product.hrceD_Amount;
                }
            }
            return total;
        };

        $scope.getDeductionTotal = function () {
            var total = 0;
            for (var i = 0; i < $scope.detectionList.length; i++) {
                if ($scope.detectionList[i].hrceD_ActiveFlag) {
                    var product = $scope.detectionList[i];
                    total += product.hrceD_Amount;
                }
            }
            return total;
        };

        $scope.getArrearTotal = function () {
            var total = 0;
            for (var i = 0; i < $scope.arrearList.length; i++) {
                if ($scope.arrearList[i].hrceD_ActiveFlag) {
                    var product = $scope.arrearList[i];
                    total += product.hrceD_Amount;
                }
            }
            return total;
        };

        $scope.printDataappoint = function (Employee) {
            var innerContents = document.getElementById('vapsappointment').innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/EmpSalarySlip/EmpPaySlipPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };
    }
})();