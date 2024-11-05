(function () {
    'use strict';
    angular
        .module('app')
        .controller('appointmentController', appointmentController);

    appointmentController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', '$filter'];
    function appointmentController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, $filter) {

        $scope.EarningDet = {};
        $scope.addjob = {};
        $scope.addjob.hrjD_Id = 0;
        //$scope.Candidate.hrmC_Id = "";
        //$scope.submitted = true;
        //$scope.submitted2 = true;
        //$scope.submitted3 = true;
        $scope.currentmi_id = 0;
        $scope.salary = true;
        $scope.document = true;
        $scope.hrcA_Id = 0;

        $scope.camelcase = function (str) {
            if (str != undefined && str != null) {
                var array1 = str.split(' ');
                var newarray1 = [];

                for (var x = 0; x < array1.length; x++) {
                    newarray1.push(array1[x].charAt(0).toUpperCase() + array1[x].slice(1).toLowerCase());
                }
                $scope.stuDobwords = newarray1;
                str = newarray1.join(' ');
            }
            return str;
        };

        // Get form Details at onload 
        $scope.onLoadGetData = function () {
            var pageid = 2;
            apiService.getURI("Appointment/getalldetails", pageid).then(function (promise) {
                if (promise.masterGender !== null && promise.masterGender.length > 0) {
                    $scope.masterGender = promise.masterGender;
                }

                if (promise.masterjob !== null && promise.masterjob.length > 0) {
                    $scope.masterjob = promise.masterjob;
                }

                if (promise.masterQualification !== null && promise.masterQualification.length > 0) {
                    $scope.masterQualification = promise.masterQualification;
                }

                if (promise.masterCaste !== null && promise.masterCaste.length > 0) {
                    $scope.masterCaste = promise.masterCaste;
                }

                if (promise.mastermaritalstatus !== null && promise.mastermaritalstatus.length > 0) {
                    $scope.mastermaritalstatus = promise.mastermaritalstatus;
                }

                $scope.currentmi_id = promise.mI_Id;
                $scope.companylist = promise.companylist;
                $scope.departmenlist = promise.departmenlist;
                $scope.candidatelist = promise.candidatelist;
                $scope.earingdeductionlist = promise.earingdeductionlist;
                $scope.detectionList = promise.detectionList;
                $scope.earningList = promise.earningList;
                $scope.arrearList = promise.arrearList;
                $scope.desgnationlist = promise.desgnationlist;
                $scope.departmentlist = promise.departmentlist;

                angular.forEach($scope.earningList, function (value, key) {
                    $scope.earningList[key].hrceD_ActiveFlag = true;
                    if (value.hrmeD_AmountPercentFlag === "Percentage") {
                        $scope.earningList[key].hrceD_Percentage = value.hrmeD_AmountPercent;
                        $scope.earningList[key].hrceD_Amount = '0.00';
                        $scope.earningList[key].hrmeD_Percent = value.hrmeD_AmountPercent;
                        $scope.earningList[key].hrmeD_AppPercent = value.hrmeD_AmountPercent;
                        $scope.earningList[key].hrmeD_Details = value.hrmeD_AmountPercent + '% of ' + value.percentOff;
                        $scope.earningList[key].AmountDis = true;
                        $scope.earningList[key].PercentDis = false;
                    }
                    else {
                        $scope.earningList[key].hrceD_Percentage = '0.00';
                        $scope.earningList[key].hrceD_Amount = value.hrmeD_AmountPercent;
                        $scope.earningList[key].hrmeD_AppPercent = '0.00';
                        $scope.earningList[key].hrmeD_Percent = '0.00';
                        $scope.earningList[key].hrmeD_Details = '';
                        $scope.earningList[key].AmountDis = false;
                        $scope.earningList[key].PercentDis = true;
                    }
                });

                angular.forEach($scope.detectionList, function (value, key) {
                    $scope.detectionList[key].hrceD_ActiveFlag = true;
                    if (value.hrmeD_AmountPercentFlag === "Percentage") {
                        $scope.detectionList[key].hrmeD_Percent = value.hrmeD_AmountPercent;
                        $scope.detectionList[key].hrceD_Amount = '0.00';
                        $scope.detectionList[key].hrceD_Percentage = value.hrmeD_AmountPercent;
                        $scope.detectionList[key].hrmeD_Details = value.hrmeD_AmountPercent + '% of ' + value.percentOff;
                        $scope.detectionList[key].AmountDis = true;
                        $scope.detectionList[key].PercentDis = false;
                    }
                    else {
                        $scope.detectionList[key].hrmeD_Percent = '0.00';
                        $scope.detectionList[key].hrceD_Amount = value.hrmeD_AmountPercent;
                        $scope.detectionList[key].hrceD_Percentage = '0.00';
                        $scope.detectionList[key].hrmeD_Details = '';
                        $scope.detectionList[key].AmountDis = false;
                        $scope.detectionList[key].PercentDis = true;
                    }
                });

                angular.forEach($scope.arrearList, function (value, key) {
                    $scope.arrearList[key].hrceD_ActiveFlag = true;
                    if (value.hrmeD_AmountPercentFlag === "Percentage") {
                        $scope.arrearList[key].hrceD_Percentage = value.hrmeD_AmountPercent;
                        $scope.arrearList[key].hrceD_Amount = '0.00';
                        $scope.arrearList[key].hrmeD_Percent = value.hrmeD_AmountPercent;
                        $scope.arrearList[key].hrmeD_AppPercent = value.hrmeD_AmountPercent;
                        $scope.arrearList[key].hrmeD_Details = value.hrmeD_AmountPercent + '% of ' + value.percentOff;
                        $scope.arrearList[key].AmountDis = true;
                        $scope.arrearList[key].PercentDis = false;
                    }
                    else {
                        $scope.arrearList[key].hrceD_Percentage = '0.00';
                        $scope.arrearList[key].hrceD_Amount = value.hrmeD_AmountPercent;
                        $scope.arrearList[key].hrmeD_AppPercent = '0.00';
                        $scope.arrearList[key].hrmeD_Percent = '0.00';
                        $scope.arrearList[key].hrmeD_Details = '';
                        $scope.arrearList[key].AmountDis = false;
                        $scope.arrearList[key].PercentDis = true;
                    }
                });

                if (promise.grossList[0] != null && promise.grossList[0] != undefined) {
                    $scope.Salary = promise.grossList[0];
                    $scope.Salary.hrceD_ActiveFlag = true;
                    $scope.Salary.hrceD_Percentage = '0.00';
                    $scope.Salary.hrceD_Amount = $scope.Salary.hrmeD_AmountPercent;
                    $scope.Salary.hrmeD_AppPercent = '0.00';
                    $scope.Salary.hrmeD_Percent = '0.00';
                    $scope.Salary.hrmeD_Details = '';
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

                $scope.EarningTotal = $scope.getEarningTotal();
                $scope.DeductionTotal = $scope.getDeductionTotal();
                $scope.ArrearTotal = $scope.getArrearTotal();
                $scope.netSalary = ($scope.EarningTotal + $scope.ArrearTotal) - $scope.DeductionTotal;
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

        $scope.submitted2 = false;
        $scope.interacted2 = function (field) {
            return $scope.submitted2;
        };

        $scope.submitted3 = false;
        $scope.interacted3 = function (field) {
            return $scope.submitted3;
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
                data.mI_Id = $scope.Candidate.mI_Id;

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

                            $scope.totalEarning = 0;
                            angular.forEach($scope.earningList, function (value, key) {
                                angular.forEach(EarningDetails, function (value1, key1) {
                                    if (value.hrmeD_Id == value1.hrmeD_Id) {
                                        $scope.earningList[key].hrceD_ActiveFlag = value1.hrceD_ActiveFlag;
                                        $scope.earningList[key].hrceD_Id = value1.hrceD_Id;
                                        $scope.earningList[key].hrceD_Amount = value1.hrceD_Amount;
                                        $scope.earningList[key].hrceD_Percentage = value1.hrceD_Percentage;
                                        $scope.earningList[key].hrmE_Id = value1.hrmE_Id;
                                        $scope.totalEarning = $scope.totalEarning + value1.hrceD_Amount;
                                    }
                                });
                            });

                            $scope.totalDeduction = 0;
                            angular.forEach($scope.detectionList, function (value, key) {
                                angular.forEach(DeductionDetails, function (value1, key1) {
                                    if (value.hrmeD_Id == value1.hrmeD_Id) {
                                        $scope.detectionList[key].hrceD_ActiveFlag = value1.hrceD_ActiveFlag;
                                        $scope.detectionList[key].hrceD_Id = value1.hrceD_Id;
                                        $scope.detectionList[key].hrceD_Amount = value1.hrceD_Amount;
                                        $scope.detectionList[key].hrceD_Percentage = value1.hrceD_Percentage;
                                        $scope.detectionList[key].hrmE_Id = value1.hrmE_Id;
                                        $scope.totalDeduction = $scope.totalDeduction + value1.hrceD_Amount;
                                    }    
                                });
                            });

                            $scope.totalArrear = 0;
                            angular.forEach($scope.arrearList, function (value, key) {
                                angular.forEach(ArrearDetails, function (value1, key1) {
                                    if (value.hrmeD_Id == value1.hrmeD_Id) {
                                        $scope.arrearList[key].hrceD_ActiveFlag = value1.hrceD_ActiveFlag;
                                        $scope.arrearList[key].hrceD_Id = value1.hrceD_Id;
                                        $scope.arrearList[key].hrceD_Amount = value1.hrceD_Amount;
                                        $scope.arrearList[key].hrceD_Percentage = value1.hrceD_Percentage;
                                        $scope.arrearList[key].hrmE_Id = value1.hrmE_Id;
                                        $scope.totalArrear = $scope.totalArrear + value1.hrceD_Amount;
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
        $scope.saveAppointmentdata = function () {
            if ($scope.myForm1.$valid) { 
                var selectedEarning = [];
                $scope.buttonenable = true;

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

                $scope.Candidate.hrcD_JoiningDate = new Date($scope.Candidate.hrcD_JoiningDate).toDateString();
                $scope.Candidate.hrcD_InterviewDate = new Date($scope.Candidate.hrcD_InterviewDate).toDateString();

                var data = {
                    Employeedto: $scope.Candidate,
                    EarningDTO: selectedEarning,
                    DeductionDTO: selectedDetection,
                    ArrearDTO: selectedArrear
                };

                apiService.create("Appointment/saveAppointmentdata", data).then(function (promise) {
                    if (promise.retrunMsg !== "") {
                        if (promise.retrunMsg == "Duplicate") {
                            swal("Record already exist..!!");
                            return;
                        }
                        else if (promise.retrunMsg === "false") {
                            swal("Record Not saved / Updated..", 'Fail');
                        }
                        else if (promise.retrunMsg === "Add") {
                            if ($scope.currentmi_id == $scope.Candidate.mI_Id) {
                                $scope.salary = false;
                                $scope.document = false;
                                swal("Record Saved Successfully..");
                            }
                            else { swal("Candidate Moved Successfully..!"); }
                        }
                        else if (promise.retrunMsg === "Update") {
                            $scope.salary = false;
                            $scope.document = false;
                            swal("Record Updated Successfully..");
                        }
                        else {
                            swal("Something went wrong ..!", 'Kindly contact Administrator');
                        }
                    }
                });
            }
            else {
                $scope.submitted = true;
                $scope.buttonenable = false;
                $scope.salary = true;
                $scope.document = true;
            }
        };

        $scope.savesalarydata = function () {
            if ($scope.myFormSalary.$valid) {
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

                apiService.create("Appointment/savesalarydata", data).then(function (promise) {
                    if (promise.retrunMsg !== "") {
                        if (promise.retrunMsg == "Duplicate") {
                            swal("Record already exist..!!");
                        }
                        else if (promise.retrunMsg === "false") {
                            swal("Record Not saved / Updated..", 'Fail');
                        }
                        else if (promise.retrunMsg === "Add") {
                            //$scope.salary = false;
                            //$scope.document = false;
                            swal("Record Saved Successfully..");
                        }
                        else if (promise.retrunMsg === "Update") {
                            swal("Record Updated Successfully..");
                        }
                        else {
                            swal("Something went wrong ..!", 'Kindly contact Administrator');
                        }
                    }
                });
            }
            else {
                $scope.submitted2 = true;
            }
        };

        $scope.buttonenable = false;
        $scope.loaddata = function (cand) {
            var data = {
                "HRCD_Id": cand
            };
            //var data = candidate;

            apiService.create("Appointment/getcandidate", data).then(function (promise) {
                if (promise.retrunMsg == "") {
                    //if (promise.candidateDetails[0].hrcD_FatherName !== null && promise.candidateDetails[0].hrcD_FatherName !== "") {
                    //    $scope.buttonenable = true;
                    //    $scope.salary = false;
                    //    $scope.document = false;
                    //}
                    //else {
                    //    $scope.buttonenable = false;
                    //    $scope.salary = true;
                    //    $scope.document = true;
                    //}
                    $scope.gendername = promise.ivrmmG_GenderName;
                    $scope.hrcD_SHGenderName = promise.hrcD_SHGenderName;

                    if (promise.candidateDetails[0].hrcD_InterviewDate !== null) {
                        promise.candidateDetails[0].hrcD_InterviewDate = new Date(promise.candidateDetails[0].hrcD_InterviewDate);
                    }
                    else {
                        promise.candidateDetails[0].hrcD_InterviewDate = null;
                    }

                    if (promise.candidateDetails[0].hrcD_JoiningDate !== null) {
                        promise.candidateDetails[0].hrcD_JoiningDate = new Date(promise.candidateDetails[0].hrcD_JoiningDate);
                    }
                    else {
                        promise.candidateDetails[0].hrcD_JoiningDate = null;
                    }

                    if (promise.hrcD_AppDate !== null) {
                        $scope.hrcD_AppDate = new Date(promise.hrcD_AppDate);
                    }
                    else {
                        $scope.hrcD_AppDate = null;
                    }

                    promise.candidateDetails[0].hrcD_FirstName = $scope.camelcase(promise.candidateDetails[0].hrcD_FirstName);
                    promise.candidateDetails[0].hrcD_MiddleName = $scope.camelcase(promise.candidateDetails[0].hrcD_MiddleName);
                    promise.candidateDetails[0].hrcD_LastName = $scope.camelcase(promise.candidateDetails[0].hrcD_LastName);

                    promise.candidateDetails[0].hrcD_FatherName = $scope.camelcase(promise.candidateDetails[0].hrcD_FatherName);
                    promise.candidateDetails[0].hrcD_SHName = $scope.camelcase(promise.candidateDetails[0].hrcD_SHName);

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
                        }
                        else {
                            $scope.Salary.hrceD_Percentage = '0.00';
                            $scope.Salary.hrceD_Amount = '0.00';
                            $scope.Salary.hrmeD_AppPercent = '0.00';
                            $scope.Salary.hrmeD_Percent = '0.00';
                            $scope.Salary.hrmeD_Details = '';
                            $scope.Salary.hrceD_ActiveFlag = true;
                        }

                        //Earning List
                        $scope.totalEarning = 0;
                        angular.forEach($scope.earningList, function (value, key) {
                            angular.forEach(EarningDetails, function (value1, key1) {
                                if (value.hrmeD_Id == value1.hrmeD_Id) {
                                    $scope.earningList[key].hrceD_ActiveFlag = value1.hrceD_ActiveFlag;
                                    $scope.earningList[key].hrceD_Id = value1.hrceD_Id;
                                    $scope.earningList[key].hrceD_Amount = value1.hrceD_Amount;
                                    $scope.earningList[key].hrceD_Percentage = value1.hrceD_Percentage;
                                    $scope.earningList[key].hrcD_Id = value1.hrcD_Id;
                                    $scope.totalEarning = parseInt($scope.totalEarning) + parseInt(value1.hrceD_Amount);
                                }
                            });
                        });

                        //deductionlist
                        $scope.totalDeduction = 0;
                        angular.forEach($scope.detectionList, function (value, key) {
                            angular.forEach(DeductionDetails, function (value1, key1) {
                                if (value.hrmeD_Id == value1.hrmeD_Id) {
                                    //  $scope.detectionList[key].Selected = true;
                                    $scope.detectionList[key].hrceD_ActiveFlag = value1.hrceD_ActiveFlag;
                                    $scope.detectionList[key].hrceD_Id = value1.hrceD_Id;
                                    $scope.detectionList[key].hrceD_Amount = value1.hrceD_Amount;
                                    $scope.detectionList[key].hrceD_Percentage = value1.hrceD_Percentage;
                                    $scope.detectionList[key].hrcD_Id = value1.hrcD_Id;
                                    $scope.totalDeduction = parseInt($scope.totalDeduction) + parseInt(value1.hrceD_Amount);
                                }
                            });
                        });

                        //arrearlist
                        $scope.totalArrear = 0;
                        angular.forEach($scope.arrearList, function (value, key) {
                            angular.forEach(ArrearDetails, function (value1, key1) {
                                if (value.hrmeD_Id == value1.hrmeD_Id) {
                                    // $scope.arrearList[key].Selected = true;
                                    $scope.arrearList[key].hrceD_ActiveFlag = value1.hrceD_ActiveFlag;
                                    $scope.arrearList[key].hrceD_Id = value1.hrceD_Id;
                                    $scope.arrearList[key].hrceD_Amount = value1.hrceD_Amount;
                                    $scope.arrearList[key].hrceD_Percentage = value1.hrceD_Percentage;
                                    $scope.arrearList[key].hrcD_Id = value1.hrcD_Id;
                                    $scope.totalArrear = parseInt($scope.totalArrear) + parseInt(value1.hrceD_Amount);
                                }
                            });
                        });

                        $scope.EarningTotal = $scope.getEarningTotal();
                        $scope.DeductionTotal = $scope.getDeductionTotal();
                        $scope.ArrearTotal = $scope.getArrearTotal();
                        $scope.netSalary = parseInt($scope.EarningTotal + $scope.ArrearTotal) - parseInt($scope.DeductionTotal);
                    }

                    if (promise.employeeEarningsDeductionsDetails != null && promise.employeeEarningsDeductionsDetails.length > 0) {
                        $scope.basicamount = $filter('filter')(promise.employeeEarningsDeductionsDetails, function (d) {
                            return d.hrmeD_Name === 'Basic Pay';
                        });

                        $scope.hraamount = $filter('filter')(promise.employeeEarningsDeductionsDetails, function (d) {
                            return d.hrmeD_Name === 'HRA';
                        });

                        $scope.conveyanceamount = $filter('filter')(promise.employeeEarningsDeductionsDetails, function (d) {
                            return d.hrmeD_Name === 'Conveyance Allowance';
                        });

                        $scope.medicalamount = $filter('filter')(promise.employeeEarningsDeductionsDetails, function (d) {
                            return d.hrmeD_Name === 'Medical Allowance';
                        });

                        $scope.mobileamount = $filter('filter')(promise.employeeEarningsDeductionsDetails, function (d) {
                            return d.hrmeD_Name === 'Mobile Allowance';
                        });

                        $scope.otherallowanceamount = $filter('filter')(promise.employeeEarningsDeductionsDetails, function (d) {
                            return d.hrmeD_Name === 'other allowance';
                        });

                        $scope.ccaamount = $filter('filter')(promise.employeeEarningsDeductionsDetails, function (d) {
                            return d.hrmeD_Name === 'CCA';
                        });

                        $scope.outstationamount = $filter('filter')(promise.employeeEarningsDeductionsDetails, function (d) {
                            return d.hrmeD_Name === 'OUTSTATION';
                        });

                        $scope.grossamount = $filter('filter')(promise.employeeEarningsDeductionsDetails, function (d) {
                            return d.hrmeD_Name === 'Gross';
                        });

                        $scope.pfamount = $filter('filter')(promise.employeeEarningsDeductionsDetails, function (d) {
                            return d.hrmeD_Name === 'PF';
                        });

                        $scope.esiamount = $filter('filter')(promise.employeeEarningsDeductionsDetails, function (d) {
                            return d.hrmeD_Name === 'ESI';
                        });

                        $scope.ptamount = $filter('filter')(promise.employeeEarningsDeductionsDetails, function (d) {
                            return d.hrmeD_Name === 'PT';
                        });

                        $scope.medicalinsuranceamount = $filter('filter')(promise.employeeEarningsDeductionsDetails, function (d) {
                            return d.hrmeD_Name === 'Medical Insurance';
                        });

                        $scope.foodamount = $filter('filter')(promise.employeeEarningsDeductionsDetails, function (d) {
                            return d.hrmeD_Name === 'Food';
                        });

                        $scope.esicamount = $filter('filter')(promise.employeeEarningsDeductionsDetails, function (d) {
                            return d.hrmeD_Name === 'ESI';
                        });

                        $scope.pfonbasicamount = $filter('filter')(promise.employeeEarningsDeductionsDetails, function (d) {
                            return d.hrmeD_Name === 'PF';
                        });

                        $scope.performancebonusamount = $filter('filter')(promise.employeeEarningsDeductionsDetails, function (d) {
                            return d.hrmeD_Name === 'PB';
                        });

                        $scope.monitoryallowanceamount = $filter('filter')(promise.employeeEarningsDeductionsDetails, function (d) {
                            return d.hrmeD_Name === 'MB';
                        });

                        $scope.localconveyanceamount = $filter('filter')(promise.employeeEarningsDeductionsDetails, function (d) {
                            return d.hrmeD_Name === 'Local Conveyance';
                        });

                        $scope.ctcamount = Math.round($scope.netSalary * 12);
                    }

                    if (promise.masterQualification !== null && promise.masterQualification.length > 0) {
                        $scope.masterQualification = promise.masterQualification;
                    }

                    if (promise.masterCaste !== null && promise.masterCaste.length > 0) {
                        $scope.masterCaste = promise.masterCaste;
                    }

                    if (promise.mastermaritalstatus !== null && promise.mastermaritalstatus.length > 0) {
                        $scope.mastermaritalstatus = promise.mastermaritalstatus;
                    }

                    $scope.currentdate = new Date();
                    $scope.Candidate = promise.candidateDetails[0];
                    console.log($scope.Candidate);
                    $scope.companydetails = promise.companydetails[0];

                    if (promise.letterdetails.length > 0) {
                        $scope.hrcA_Id = promise.letterdetails[0].hrcA_Id;
                        $scope.documentnameone = promise.letterdetails[0].hrcA_FirstDocName;

                        if (promise.letterdetails[0].hrcA_FirstDocDate !== null) {
                            promise.letterdetails[0].hrcA_FirstDocDate = new Date(promise.letterdetails[0].hrcA_FirstDocDate);
                        }
                        else {
                            promise.letterdetails[0].hrcA_FirstDocDate = null;
                        }

                        $scope.documentnametwo = promise.letterdetails[0].hrcA_SecDocName;

                        if (promise.letterdetails[0].hrcA_SecDocDate !== null) {
                            promise.letterdetails[0].hrcA_SecDocDate = new Date(promise.letterdetails[0].hrcA_SecDocDate);
                        }
                        else {
                            promise.letterdetails[0].hrcA_SecDocDate = null;
                        }

                        $scope.first_doc_date = promise.letterdetails[0].hrcA_FirstDocDate;
                        $scope.second_doc_date = promise.letterdetails[0].hrcA_SecDocDate;
                        $scope.candidatectc = promise.letterdetails[0].hrcA_AnnualCTC;
                        $scope.monthlyctc = promise.letterdetails[0].hrcA_MonthlyCTC;
                        $scope.apprefno = promise.letterdetails[0].hrcA_AppointmentRefNo;
                        $scope.ackrefno = promise.letterdetails[0].hrcA_AcknowledgementRefNo;
                    }

                    if (promise.candidateDetails[0].mI_Id == 16) { $scope.companyowner = "Mr. Siddesh Kumar R"; }
                    if (promise.candidateDetails[0].mI_Id == 17) { $scope.companyowner = "Mr. Siddesh Kumar R"; }
                    if (promise.candidateDetails[0].mI_Id == 20) { $scope.companyowner = "Mr. Rajeev kanchan"; }
                    if (promise.candidateDetails[0].mI_Id == 21) { $scope.companyowner = "Mr. Somashekar"; }
                    if (promise.candidateDetails[0].mI_Id == 22) { $scope.companyowner = "Mr. Himantharaj"; }
                    if (promise.candidateDetails[0].mI_Id == 23) { $scope.companyowner = "Mr. Siddesh Kumar R"; }
                    if (promise.candidateDetails[0].mI_Id == 24) { $scope.companyowner = "Mr. Anil Patil Kulkarni"; }
                    if (promise.candidateDetails[0].mI_Id == 27) { $scope.companyowner = "Mr. Siddesh Kumar R"; }
                    $scope.buttonenable = false;
                }
            });
        };

        $scope.loadsalary = function (cand) {
            if (cand != undefined && cand != null) {
                var data = {
                    "HRCD_Id": cand
                };

                apiService.create("Appointment/getcandidate", data).then(function (promise) {
                    if (promise.retrunMsg == "") {
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
                            }
                            else {
                                $scope.Salary.hrceD_Percentage = '0.00';
                                $scope.Salary.hrceD_Amount = '0.00';
                                $scope.Salary.hrmeD_AppPercent = '0.00';
                                $scope.Salary.hrmeD_Percent = '0.00';
                                $scope.Salary.hrmeD_Details = '';
                                $scope.Salary.hrceD_ActiveFlag = true;
                            }

                            //Earning List
                            $scope.totalEarning = 0;
                            angular.forEach($scope.earningList, function (value, key) {
                                angular.forEach(EarningDetails, function (value1, key1) {
                                    if (value.hrmeD_Id == value1.hrmeD_Id) {
                                        $scope.earningList[key].hrceD_ActiveFlag = value1.hrceD_ActiveFlag;
                                        $scope.earningList[key].hrceD_Id = value1.hrceD_Id;
                                        $scope.earningList[key].hrceD_Amount = value1.hrceD_Amount;
                                        $scope.earningList[key].hrceD_Percentage = value1.hrceD_Percentage;
                                        $scope.earningList[key].hrcD_Id = value1.hrcD_Id;
                                        $scope.totalEarning = parseInt($scope.totalEarning) + parseInt(value1.hrceD_Amount);
                                    }
                                });
                            });

                            //deductionlist
                            $scope.totalDeduction = 0;
                            angular.forEach($scope.detectionList, function (value, key) {
                                angular.forEach(DeductionDetails, function (value1, key1) {
                                    if (value.hrmeD_Id == value1.hrmeD_Id) {
                                        //  $scope.detectionList[key].Selected = true;
                                        $scope.detectionList[key].hrceD_ActiveFlag = value1.hrceD_ActiveFlag;
                                        $scope.detectionList[key].hrceD_Id = value1.hrceD_Id;
                                        $scope.detectionList[key].hrceD_Amount = value1.hrceD_Amount;
                                        $scope.detectionList[key].hrceD_Percentage = value1.hrceD_Percentage;
                                        $scope.detectionList[key].hrcD_Id = value1.hrcD_Id;
                                        $scope.totalDeduction = parseInt($scope.totalDeduction) + parseInt(value1.hrceD_Amount);
                                    }
                                });
                            });

                            //arrearlist
                            $scope.totalArrear = 0;
                            angular.forEach($scope.arrearList, function (value, key) {
                                angular.forEach(ArrearDetails, function (value1, key1) {
                                    if (value.hrmeD_Id == value1.hrmeD_Id) {
                                        // $scope.arrearList[key].Selected = true;
                                        $scope.arrearList[key].hrceD_ActiveFlag = value1.hrceD_ActiveFlag;
                                        $scope.arrearList[key].hrceD_Id = value1.hrceD_Id;
                                        $scope.arrearList[key].hrceD_Amount = value1.hrceD_Amount;
                                        $scope.arrearList[key].hrceD_Percentage = value1.hrceD_Percentage;
                                        $scope.arrearList[key].hrcD_Id = value1.hrcD_Id;
                                        $scope.totalArrear = parseInt($scope.totalArrear) + parseInt(value1.hrceD_Amount);
                                    }
                                });
                            });

                            $scope.EarningTotal = $scope.getEarningTotal();
                            $scope.DeductionTotal = $scope.getDeductionTotal();
                            $scope.ArrearTotal = $scope.getArrearTotal();
                            $scope.netSalary = parseInt($scope.EarningTotal + $scope.ArrearTotal) - parseInt($scope.DeductionTotal);
                        }

                        if (promise.employeeEarningsDeductionsDetails != null && promise.employeeEarningsDeductionsDetails.length > 0) {
                            $scope.basicamount = $filter('filter')(promise.employeeEarningsDeductionsDetails, function (d) {
                                return d.hrmeD_Name === 'Basic Pay';
                            });

                            $scope.hraamount = $filter('filter')(promise.employeeEarningsDeductionsDetails, function (d) {
                                return d.hrmeD_Name === 'HRA';
                            });

                            $scope.conveyanceamount = $filter('filter')(promise.employeeEarningsDeductionsDetails, function (d) {
                                return d.hrmeD_Name === 'Conveyance  allowance';
                            });

                            $scope.medicalamount = $filter('filter')(promise.employeeEarningsDeductionsDetails, function (d) {
                                return d.hrmeD_Name === 'MEDICAL ALLOW';
                            });

                            $scope.mobileamount = $filter('filter')(promise.employeeEarningsDeductionsDetails, function (d) {
                                return d.hrmeD_Name === 'MOB REC';
                            });

                            $scope.otherallowanceamount = $filter('filter')(promise.employeeEarningsDeductionsDetails, function (d) {
                                return d.hrmeD_Name === 'other allowance';
                            });

                            $scope.ccaamount = $filter('filter')(promise.employeeEarningsDeductionsDetails, function (d) {
                                return d.hrmeD_Name === 'CCA';
                            });

                            $scope.outstationamount = $filter('filter')(promise.employeeEarningsDeductionsDetails, function (d) {
                                return d.hrmeD_Name === 'OUTSTATION';
                            });

                            $scope.grossamount = $filter('filter')(promise.employeeEarningsDeductionsDetails, function (d) {
                                return d.hrmeD_Name === 'Gross';
                            });

                            $scope.pfamount = $filter('filter')(promise.employeeEarningsDeductionsDetails, function (d) {
                                return d.hrmeD_Name === 'PF';
                            });

                            $scope.esiamount = $filter('filter')(promise.employeeEarningsDeductionsDetails, function (d) {
                                return d.hrmeD_Name === 'ESI';
                            });

                            $scope.ptamount = $filter('filter')(promise.employeeEarningsDeductionsDetails, function (d) {
                                return d.hrmeD_Name === 'PT';
                            });

                            $scope.medicalinsuranceamount = $filter('filter')(promise.employeeEarningsDeductionsDetails, function (d) {
                                return d.hrmeD_Name === 'MEDICALINSURANCE';
                            });

                            $scope.foodamount = $filter('filter')(promise.employeeEarningsDeductionsDetails, function (d) {
                                return d.hrmeD_Name === 'Food';
                            });

                            $scope.esicamount = $filter('filter')(promise.employeeEarningsDeductionsDetails, function (d) {
                                return d.hrmeD_Name === 'ESI';
                            });

                            $scope.pfonbasicamount = $filter('filter')(promise.employeeEarningsDeductionsDetails, function (d) {
                                return d.hrmeD_Name === 'PF';
                            });

                            $scope.performancebonusamount = $filter('filter')(promise.employeeEarningsDeductionsDetails, function (d) {
                                return d.hrmeD_Name === 'PB';
                            });

                            $scope.monitoryallowanceamount = $filter('filter')(promise.employeeEarningsDeductionsDetails, function (d) {
                                return d.hrmeD_Name === 'MB';
                            });

                            $scope.localconveyanceamount = $filter('filter')(promise.employeeEarningsDeductionsDetails, function (d) {
                                return d.hrmeD_Name === 'LOCAL CONVEYANCE';
                            });

                            $scope.ctcamount = Math.round($scope.netSalary * 12);
                        }
                    }
                });
            }
        };

        $scope.printToCartapp1 = function () {
            var innerContents = document.getElementById("vapsappointment1").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/EmpSalarySlip/appointmentpdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };

        $scope.printToCartapp2 = function () {
            var innerContents = document.getElementById("vapsappointment2").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/EmpSalarySlip/appointmentpdf.css" />' +
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
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/EmpSalarySlip/appointmentpdf.css" />' +
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
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/EmpSalarySlip/appointmentpdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()" margin-top: 100px; onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };

        $scope.printacknowledgement = function (vapsacknowledgement) {
            //$scope.first_doc_date = new Date($scope.first_doc_date);
            //$scope.second_doc_date = new Date($scope.second_doc_date);

            var innerContents = document.getElementById("vapsacknowledgement").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/EmpSalarySlip/appointmentpdf.css" />' +
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
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/EmpSalarySlip/appointmentpdf.css" />' +
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
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/EmpSalarySlip/appointmentpdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };

        $scope.getcompanydetails = function () {
            var data = {
                "MI_Id": $scope.Candidate.mI_Id
            };

            apiService.create("Appointment/getcompanydetails", data).then(function (promise) {                
                if (promise.masterGender !== null && promise.masterGender.length > 0) {
                    $scope.masterGender = promise.masterGender;
                }

                if (promise.masterQualification !== null && promise.masterQualification.length > 0) {
                    $scope.masterQualification = promise.masterQualification;
                }

                if (promise.masterCaste !== null && promise.masterCaste.length > 0) {
                    $scope.masterCaste = promise.masterCaste;
                }

                if (promise.mastermaritalstatus !== null && promise.mastermaritalstatus.length > 0) {
                    $scope.mastermaritalstatus = promise.mastermaritalstatus;
                }
                
                $scope.desgnationlist = promise.desgnationlist;
                $scope.departmentlist = promise.departmentlist;
                $scope.companydetails = promise.companydetails[0];

                $scope.earingdeductionlist = promise.earingdeductionlist;
                $scope.detectionList = promise.detectionList;
                $scope.earningList = promise.earningList;
                $scope.arrearList = promise.arrearList;

                angular.forEach($scope.earningList, function (value, key) {
                    $scope.earningList[key].hrceD_ActiveFlag = true;
                    if (value.hrmeD_AmountPercentFlag === "Percentage") {
                        $scope.earningList[key].hrceD_Percentage = value.hrmeD_AmountPercent;
                        $scope.earningList[key].hrceD_Amount = '0.00';
                        $scope.earningList[key].hrmeD_Percent = value.hrmeD_AmountPercent;
                        $scope.earningList[key].hrmeD_AppPercent = value.hrmeD_AmountPercent;
                        $scope.earningList[key].hrmeD_Details = value.hrmeD_AmountPercent + '% of ' + value.percentOff;
                        $scope.earningList[key].AmountDis = true;
                        $scope.earningList[key].PercentDis = false;
                    }
                    else {
                        $scope.earningList[key].hrceD_Percentage = '0.00';
                        $scope.earningList[key].hrceD_Amount = value.hrmeD_AmountPercent;
                        $scope.earningList[key].hrmeD_AppPercent = '0.00';
                        $scope.earningList[key].hrmeD_Percent = '0.00';
                        $scope.earningList[key].hrmeD_Details = '';
                        $scope.earningList[key].AmountDis = false;
                        $scope.earningList[key].PercentDis = true;
                    }
                });

                angular.forEach($scope.detectionList, function (value, key) {
                    $scope.detectionList[key].hrceD_ActiveFlag = true;
                    if (value.hrmeD_AmountPercentFlag === "Percentage") {
                        $scope.detectionList[key].hrmeD_Percent = value.hrmeD_AmountPercent;
                        $scope.detectionList[key].hrceD_Amount = '0.00';
                        $scope.detectionList[key].hrceD_Percentage = value.hrmeD_AmountPercent;
                        $scope.detectionList[key].hrmeD_Details = value.hrmeD_AmountPercent + '% of ' + value.percentOff;
                        $scope.detectionList[key].AmountDis = true;
                        $scope.detectionList[key].PercentDis = false;
                    }
                    else {
                        $scope.detectionList[key].hrmeD_Percent = '0.00';
                        $scope.detectionList[key].hrceD_Amount = value.hrmeD_AmountPercent;
                        $scope.detectionList[key].hrceD_Percentage = '0.00';
                        $scope.detectionList[key].hrmeD_Details = '';
                        $scope.detectionList[key].AmountDis = false;
                        $scope.detectionList[key].PercentDis = true;
                    }
                });

                angular.forEach($scope.arrearList, function (value, key) {
                    $scope.arrearList[key].hrceD_ActiveFlag = true;
                    if (value.hrmeD_AmountPercentFlag === "Percentage") {
                        $scope.arrearList[key].hrceD_Percentage = value.hrmeD_AmountPercent;
                        $scope.arrearList[key].hrceD_Amount = '0.00';
                        $scope.arrearList[key].hrmeD_Percent = value.hrmeD_AmountPercent;
                        $scope.arrearList[key].hrmeD_AppPercent = value.hrmeD_AmountPercent;
                        $scope.arrearList[key].hrmeD_Details = value.hrmeD_AmountPercent + '% of ' + value.percentOff;
                        $scope.arrearList[key].AmountDis = true;
                        $scope.arrearList[key].PercentDis = false;
                    }
                    else {
                        $scope.arrearList[key].hrceD_Percentage = '0.00';
                        $scope.arrearList[key].hrceD_Amount = value.hrmeD_AmountPercent;
                        $scope.arrearList[key].hrmeD_AppPercent = '0.00';
                        $scope.arrearList[key].hrmeD_Percent = '0.00';
                        $scope.arrearList[key].hrmeD_Details = '';
                        $scope.arrearList[key].AmountDis = false;
                        $scope.arrearList[key].PercentDis = true;
                    }
                });

                if (promise.grossList[0] != null && promise.grossList[0] != undefined) {
                    $scope.Salary = promise.grossList[0];
                    $scope.Salary.hrceD_ActiveFlag = true;
                    $scope.Salary.hrceD_Percentage = '0.00';
                    $scope.Salary.hrceD_Amount = $scope.Salary.hrmeD_AmountPercent;
                    $scope.Salary.hrmeD_AppPercent = '0.00';
                    $scope.Salary.hrmeD_Percent = '0.00';
                    $scope.Salary.hrmeD_Details = '';
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

                $scope.EarningTotal = $scope.getEarningTotal();
                $scope.DeductionTotal = $scope.getDeductionTotal();
                $scope.ArrearTotal = $scope.getArrearTotal();
                $scope.netSalary = ($scope.EarningTotal + $scope.ArrearTotal) - $scope.DeductionTotal;

                $scope.Candidate.hrcD_Department = "";
                $scope.Candidate.hrcD_Designation = "";
                $scope.Candidate.hrmC_Id = "";
                $scope.Candidate.hrcD_MaritalStatus = "";
                $scope.Candidate.hrcD_CasteId = "";
                $scope.Candidate.hrcD_SHGender = "";
                //$scope.submitted = false;
            });
        };

        $scope.saveappointmenttab = function () {
            if ($scope.myForm3.$valid) {
                $scope.buttonenable = true;

                $scope.firstdocdate = new Date($scope.first_doc_date).toDateString();
                $scope.seconddocdate = new Date($scope.second_doc_date).toDateString();

                var data = {
                    Employeedto: $scope.Candidate,
                    "HRCA_Id": $scope.hrcA_Id,
                    "HRCA_FirstDocName": $scope.documentnameone,
                    "HRCA_FirstDocDate": $scope.firstdocdate,
                    "HRCA_SecDocName": $scope.documentnametwo,
                    "HRCA_SecDocDate": $scope.seconddocdate,
                    "HRCA_AnnualCTC": $scope.candidatectc,
                    "HRCA_MonthlyCTC": $scope.monthlyctc,
                    "HRCA_AppointmentRefNo": $scope.apprefno,
                    "HRCA_AcknowledgementRefNo": $scope.ackrefno
                };

                apiService.create("AddCandidateVMS/saveAppointmenttab", data).then(function (promise) {
                    if (promise.retrunMsg !== "") {
                        if (promise.retrunMsg == "Duplicate") {
                            swal("Record already exist..!!");
                            return;
                        }
                        else if (promise.retrunMsg === "false") {
                            swal("Record Not saved / Updated..", 'Fail');
                        }
                        else if (promise.retrunMsg === "Add") {
                            if ($scope.currentmi_id == $scope.Candidate.mI_Id) {
                                $scope.salary = false;
                                $scope.document = false;
                                swal("Record Saved Successfully..");
                            }
                            else { swal("Candidate Moved Successfully..!"); }
                        }
                        else if (promise.retrunMsg === "Update") {
                            $scope.salary = false;
                            $scope.document = false;
                            swal("Record Updated Successfully..");
                        }
                        else {
                            swal("Something went wrong ..!", 'Kindly contact Administrator');
                        }
                    }
                });
            }
            else {
                $scope.submitted = true;
                $scope.buttonenable = false;
                $scope.salary = true;
                $scope.document = true;
            }
        };
    }
})();