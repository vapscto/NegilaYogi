
(function () {
    'use strict';
    angular
        .module('app')
        .controller('LeaveCreditController', LeaveCreditController)
    LeaveCreditController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q', '$stateParams', '$filter']
    function LeaveCreditController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $q, $stateParams, $filter) {
        $scope.editEmployee = {};

        $scope.hstep = 1;
        $scope.mstep = 1;
        $scope.albumNameArraycolumn = [];
        $scope.Minserviceappl = false;
        $scope.Maxleaveappl = false;
        $scope.activeminleaveappl = false;
        $scope.maxla = 0;
        $scope.minla = 0;
        $scope.ismeridian = true;
        $scope.toggleMode = function () {
            $scope.ismeridian = !$scope.ismeridian;
        };

        $scope.showdepartment = true;
        $scope.showdesignation = true;

        var paginationformasters;
        var copty;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        //if (ivrmcofigsettings.length > 0) {
        //    paginationformasters = ivrmcofigsettings[0].ivrmgC_ReportPagination;
        //    copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
        //}

        $scope.itemsPerPage = paginationformasters;
        if ($scope.itemsPerPage == undefined || $scope.itemsPerPage == null) {
            $scope.itemsPerPage = 5;
        }
        $scope.currentPage = 1;
        $scope.coptyright = copty;

        $scope.searchValue = '';

        $scope.filterValue = function (obj) {
            return (angular.lowercase(obj.hrmE_EmployeeFirstName)).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
                (angular.lowercase(obj.hrmL_LeaveName)).indexOf(angular.lowercase($scope.searchValue)) >= 0
        };

        $scope.loadData = function () {
            var id = 2;

            // $scope.all_check();
            apiService.getURI("LeaveCredit/GetLeaveCredit", id).
                then(function (promise) {

                    $scope.staff_types = promise.stf_types;
                    $scope.Department_types = promise.department_types;
                    $scope.Designation_types = promise.designation_types;
                    $scope.grade_name = promise.grade_name;
                    $scope.leave_name = promise.leave_name;
                    $scope.credit_month = promise.credit_month;
                    $scope.cfmonth = promise.credit_month;
                    $scope.earnded = promise.earnded;

                    $scope.selectedept = promise.department_types;
                    $scope.selectedesg = promise.designation_types;

                    $scope.count = $scope.Department_types.length;
                    $scope.count1 = $scope.Designation_types.length;



                    $scope.results11 = promise.result;
                    $scope.presentCountgrid = $scope.results11.length;

                    $scope.selectcarry = true;
                    $scope.selectmonth = true;
                    $scope.selectCmonth = true;

                    $scope.activeyear = true;
                    $scope.activemonth = true;
                    $scope.activeday = true;

                    $scope.activemaxleave = true;
                    $scope.activeminleave = true;

                    $scope.activeye = true;
                    $scope.activemo = true;

                    $scope.activeEncashyears = true;
                    $scope.activeymtxt = true;

                    $scope.hideearnded = true;
                    $scope.hideamt = true;


                    if ($scope.count == 0) {
                        swal("Data not Found !!");
                        $scope.ckdept = false;
                    }
                });

        };


        $scope.onselectLeave = function () {



            if ($scope.hrmL_Id != "") {
                var data = {
                    "leavecode": $scope.hrmL_Id
                };
                apiService.create("LeaveCredit/get_leavecode", data).
                    then(function (promise) {

                        if (promise.leave_code.length > 0) {
                            $scope.leavecode = promise.leave_code[0].hrmL_LeaveCode;
                        }
                    });
                $scope.nofday = false;
            }
        };

        $scope.all_check_type = function (emtype) {

            var toggleStatus1 = emtype;
            angular.forEach($scope.staff_types, function (itm) {
                itm.typ = toggleStatus1;
            });
            if ($scope.emtype == true) {
                // $scope.showdepartment = true;
                $scope.showdepartment = true;
            }
            else {
                $scope.showdepartment = false;
                $scope.showdesignation = false;
                if ($scope.showdepartment == false) {
                    $scope.dept = false;
                    angular.forEach($scope.Department_types, function (obj) {
                        obj.dep = false;
                    });

                }
            }
            //   $scope.showdepartment = true;
            //   $scope.showdesignation = false;
        };

        $scope.addColumn1 = function () {

            $scope.emtype = $scope.staff_types.every(function (itm) { return itm.typ; });
            $scope.get_departments();
        };

        //Department
        $scope.all_check_dept = function (dept) {

            var toggleStatus2 = dept;
            angular.forEach($scope.Department_types, function (itm) {
                itm.dep = toggleStatus2;
            });
            if ($scope.dept == true) {
                // $scope.showdepartment = true;
                $scope.showdesignation = true;
            }
            else {
                $scope.showdesignation = false;
            }

        };
        $scope.addColumn2 = function () {
            $scope.dept = $scope.Department_types.every(function (itm) { return itm.dep; });
            $scope.get_designation();
        };

        //Designation
        $scope.all_check_desig = function (desig) {

            var toggleStatus3 = desig;
            angular.forEach($scope.Designation_types, function (itm) {
                itm.desg = toggleStatus3;
            });
            // $scope.get_employee();
        };
        $scope.addColumn3 = function () {
            $scope.desig = $scope.Designation_types.every(function (itm) { return itm.desg; });
            //  $scope.get_employee();
        };


        $scope.all_check_grade = function (desig) {

            var toggleStatus3 = desig;
            angular.forEach($scope.grade_name, function (itm) {
                itm.grade = toggleStatus3;
            });
            //   $scope.get_employee();
        };
        $scope.addColumn40 = function () {
            $scope.grade = $scope.grade_name.every(function (itm) { return itm.grade; });
            // $scope.get_employee();
        };


        $scope.all_check_month = function (monthcheck) {
            var toggleStatus7 = monthcheck;
            angular.forEach($scope.credit_month, function (itm) {
                itm.classmonth = toggleStatus7;
            });
        };

        $scope.addColumn55 = function () {
            $scope.desig = $scope.credit_month.every(function (itm) { return itm.classmonth; });
            // $scope.get_employee();
        };


        $scope.all_check_month2 = function () {

            if ($scope.monthcheck == 1) {
                $scope.selectmonth = false;
            }
            else {
                $scope.selectmonth = true;
            }
        };
        $scope.showleave = function () {

            if ($scope.Maxcarry == 1) {
                $scope.selectcarry = false;
            }
            else {
                $scope.selectcarry = true;
            }
        };


        $scope.activeymd = function () {
            if ($scope.Minserviceappl == 1) {
                $scope.activeyear = false;
                $scope.activemonth = false;
                $scope.activeday = false;
            }
            else {
                $scope.activeyear = true;
                $scope.activemonth = true;
                $scope.activeday = true;
            }
        };
        $scope.activemaxleaveappl = function () {
            if ($scope.Maxleaveappl == 1) {
                $scope.activemaxleave = false;
            }
            else {
                $scope.activemaxleave = true;
            }
        };
        $scope.activeminleaveappl = function () {
            if ($scope.Minleaveappl == 1) {
                $scope.activeminleave = false;
            }
            else {
                $scope.activeminleave = true;
            }
        };
        $scope.activeyearmonth = function () {
            if ($scope.Encash == 1) {
                $scope.activeye = false;
                $scope.activemo = false;
                $scope.activeEncashyears = false;
                $scope.activeymtxt = false;
            }
            else {
                $scope.activeye = true;
                $scope.activemo = true;
                $scope.activeEncashyears = true;
                $scope.activeymtxt = true;
            }
        };


        $scope.monthchkbx = function () {
            $scope.monthcheck = $scope.credit_month.every(function (options) {
                return options.classmonth;
            });
        };

        $scope.get_departments = function () {

            $scope.selectedemptypes = [];
            angular.forEach($scope.staff_types, function (role) {
                if (role.typ) $scope.selectedemptypes.push(role);
            });
            if ($scope.selectedemptypes.length != 0) {
                $scope.showdepartment = true;
                var data = {
                    emptypes: $scope.selectedemptypes,
                };
                apiService.create("LeaveCredit/get_departments", data).
                    then(function (promise) {

                        $scope.Department_types = promise.department_types;
                        $scope.count = $scope.Department_types.length;

                        if ($scope.count == 0 && ($scope.selectedemptypes.length != 0)) {
                            swal("No Department Are Mapped with Selected Group Type !!!!");

                        }
                    });
            }
            else if ($scope.selectedemptypes.length == 0) {
                $scope.Department_types = $scope.selectedept;
                $scope.showdepartment = false;
                $scope.showdesignation = false;
                if ($scope.showdepartment == false) {
                    $scope.dept = false;
                }
            }
        };

        $scope.get_grade = function () {

            $scope.selectedemptypes = [];
            angular.forEach($scope.staff_types, function (role) {
                if (role.typ) $scope.selectedemptypes.push(role);
            });
            if ($scope.selectedemptypes.length != 0) {
                $scope.showdepartment = true;
                var data = {
                    emptypes: $scope.selectedemptypes,
                };
                apiService.create("LeaveCredit/get_grade", data).
                    then(function (promise) {

                        $scope.grade_name = promise.grade_name;
                        $scope.count = $scope.grade_name.length;

                        if ($scope.count == 0 && ($scope.selectedemptypes.length != 0)) {
                            swal("No Grade Are Mapped with Selected Group Type !!!!");

                        }
                    });
            }
            else if ($scope.selectedemptypes.length == 0) {
                $scope.Department_types = $scope.selectedept;
                $scope.showdepartment = false;
                $scope.showdesignation = false;
                if ($scope.showdepartment == false) {
                    $scope.dept = false;
                }
            }
        };


        $scope.get_designation = function () {
            $scope.selecteddesgtypes = [];
            angular.forEach($scope.Department_types, function (role) {
                if (role.dep) $scope.selecteddesgtypes.push(role);
            });
            if ($scope.selecteddesgtypes.length != 0) {
                $scope.showdesignation = true;
                var data = {
                    emptypes: $scope.selecteddesgtypes,
                };
                apiService.create("LeaveCredit/get_designation", data).
                    then(function (promise) {

                        $scope.Designation_types = promise.designation_types;
                    });
            }
            else if ($scope.selecteddesgtypes.length == 0) {
                $scope.Designation_types = $scope.selectedesg;
                $scope.showdesignation = false;
            }
        };


        //--------------------------------------------------------------------------------------------Tab



        $scope.interacted = function (field) {

            return $scope.submitted;
        };
        $scope.interacted1 = function (field) {

            return $scope.submitted1;
        };
        $scope.interacted2 = function (field) {

            return $scope.submitted2;
        };


        $scope.submitted = false;
        $scope.submitted1 = false;
        $scope.submitted2 = false;
        $scope.isduplicate = false;
        $scope.carry = true;
        $scope.encash = true;
        $scope.myTabIndex = 0;

        $scope.validateStuDet = function (data) {
            $scope.submitted = true;
            if ($scope.myForm1.$valid) {
                $scope.carry = false;
                $scope.myTabIndex = $scope.myTabIndex + 1;
            }
            else {
                $scope.submitted = true;
                $scope.carry = true;
            }
        };
        $scope.clear_first_tab = function () {
            $scope.hrmL_Id = "";
            $scope.leavecode = "";
            $scope.noofday = "";
            $scope.Maxl = "";
            $scope.desigcarry = false;
            $scope.desiggg = false;
            $scope.monthcheck = false;
            $scope.all_check_month($scope.monthcheck);

            angular.forEach(
                angular.element("input[type='file']"),
                function (inputElem) {
                    angular.element(inputElem).val(null);
                });

            $scope.submitted = false;
            $scope.myForm1.$setPristine();
            $scope.myForm1.$setUntouched();
        };

        $scope.clear_second_tab = function () {
            $scope.Maxcarry = false;
            $scope.monthcheck = false;
            $scope.monthcheck = false;
            $scope.all_check_month($scope.monthcheck);

            $scope.submitted = false;
            $scope.myForm1.$setPristine();
            $scope.myForm1.$setUntouched();
        };

        $scope.clear_fifth_tab = function () {
            $scope.Minserviceappl = false;
            $scope.yearmin = "";
            $scope.monthmin = "";
            $scope.daymin = "";
            $scope.Maxleaveappl = false;
            $scope.maxla = "";
            $scope.Minleaveappl = false;
            $scope.minla = "";
            $scope.Encash = false;
            $scope.checkboxval = false;
            $scope.amttxt = "";
            $scope.activeym = false;
            $scope.ScheduleYear = "";
            $scope.ScheduleMonth = "";

            $scope.submitted = false;
            $scope.myForm1.$setPristine();
            $scope.myForm1.$setUntouched();
        };

        //carry form validation
        $scope.submitted2 = false;
        $scope.previous = function () {
            //validateStuDet
            $scope.myTabIndex = $scope.myTabIndex - 1;
        };

        $scope.validateadd = function () {
            if ($scope.myForm2.$valid) {
                $scope.encash = false;
                $scope.myTabIndex = $scope.myTabIndex + 1;
            }
            else {
                $scope.submitted2 = true;
                $scope.encash = true;
            }
        };

        //others form validation
        $scope.submitted1 = false;
        $scope.validateOtherPrevious = function () {
            $scope.myTabIndex = $scope.myTabIndex - 1;
        };

        $scope.previous_document = function () {
            $scope.myTabIndex = $scope.myTabIndex - 1;
        };

        $scope.SelectedMonthList = [];
        $scope.SelectedcarryMonthList = [];
        $scope.selectedemptypesss = []; $scope.selectedarrayemptypes = [];
        $scope.selecteddesgtypess = []; $scope.selectedarraydesgtypes = [];
        $scope.selectedeptss = []; $scope.selectearraydept = [];
        $scope.grade_namess = []; $scope.gradearray_name = [];
        $scope.selecteddesignation = []; $scope.selecteddesignation_name = [];

        $scope.savedata = function () {
            if ($scope.myForm.$valid) {
                $scope.submitted = true;
            }

            //array of credit month
            if ($scope.myForm1.$valid) {
                if ($scope.credit_month != "" || $scope.credit_month == null) {
                    if ($scope.credit_month.length > 0) {
                        for (var i = 0; i < $scope.credit_month.length; i++) {
                            if ($scope.credit_month[i].classmonth == true) {
                                $scope.SelectedMonthList.push($scope.credit_month[i].ivrM_Month_Id);
                                $scope.SelectedcarryMonthList.push($scope.credit_month[i].ivrM_Month_Id);
                            }
                        }
                    }
                }
                //array of employee type
                if ($scope.staff_types != "" || $scope.staff_types == null) {
                    if ($scope.staff_types.length > 0) {
                        for (var i = 0; i < $scope.staff_types.length; i++) {
                            if ($scope.staff_types[i].typ == true) {
                                $scope.selectedemptypesss.push($scope.staff_types[i]);
                                $scope.selectedarrayemptypes.push($scope.staff_types[i]);
                            }
                        }
                    }
                }
                //array of employee department
                if ($scope.Department_types != "" || $scope.Department_types == null) {
                    if ($scope.Department_types.length > 0) {
                        for (var i = 0; i < $scope.Department_types.length; i++) {
                            if ($scope.Department_types[i].dep == true) {
                                $scope.selectedeptss.push($scope.Department_types[i]);
                                $scope.selectearraydept.push($scope.Department_types[i]);
                            }
                        }
                    }
                }
                //array of emmploye designation
                if ($scope.Designation_types != "" || $scope.Designation_types == null) {
                    if ($scope.Designation_types.length > 0) {
                        for (var i = 0; i < $scope.Designation_types.length; i++) {
                            if ($scope.Designation_types[i].desg == true) {
                                $scope.selecteddesignation.push($scope.Designation_types[i]);
                                $scope.selecteddesignation_name.push($scope.Designation_types[i]);
                            }
                        }
                    }
                }
                // array of employee grade
                if ($scope.grade_name != "" || $scope.grade_name == null) {
                    if ($scope.grade_name.length > 0) {
                        for (var i = 0; i < $scope.grade_name.length; i++) {
                            if ($scope.grade_name[i].grade == true) {
                                $scope.grade_namess.push($scope.grade_name[i]);
                                $scope.gradearray_name.push($scope.grade_name[i]);
                            }
                        }
                    }
                }

                //if ($scope.credit_month.length > 0) {
                //    for (var i = 0; i < $scope.credit_month.length; i++) {
                //        if ($scope.credit_month[i].ivrM_Month_Name == true) {
                //            $scope.SelectedMonthList.push($scope.credit_month[i].ivrM_Month_Id);
                //        }
                //    }
                //}

                //if ($scope.cfmonth.length > 0) {
                //    for (var i = 0; i < $scope.cfmonth.length; i++) {
                //        if ($scope.cfmonth[i].ivrM_Month_Name == true) {
                //            $scope.selectedcarryMonthList.push($scope.cfmonth[i].ivrM_Month_Id);
                //        }
                //    }
                //}

                //if ($scope.carrymonthcheck == 1)
                //{
                //    $scope.HRMLDCF_MaxLeaveAplFlg = 1;
                //}
                //else if ($scope.carrymonthcheck == 0)
                //{
                //    $scope.HRMLDCF_MaxLeaveAplFlg = 0;
                //}
                //if ($scope.Maxleaveappl == 1) {
                //    $scope.HRMLDEC_MaxLeaveFlg = 1;
                //}
                //else if ($scope.Maxleaveappl == 0) {
                //    $scope.HRMLDEC_MaxLeaveFlg = 0;
                //}
                //if ($scope.Minleaveappl == 1) {
                //    $scope.HRMLDEC_MinLeaveFlg = 1;
                //}
                //else if ($scope.Minleaveappl == 0) {
                //    $scope.HRMLDEC_MinLeaveFlg = 0;
                //}
                //if ($scope.Encash == 1) {
                //    $scope.HRMLDEC_ScheduleFlg = 1;
                //}
                //else if ($scope.Encash == 0) {
                //    $scope.HRMLDEC_ScheduleFlg = 0;
                //}
                //if ($scope.Minserviceappl == 1) {
                //    $scope.HRMLDEC_ServiceAplFlg = 1;
                //}
                //else if ($scope.Minserviceappl == 0) {
                //    $scope.HRMLDEC_ServiceAplFlg = 0;
                //}

                var data = {
                    "HRML_Id": $scope.hrmL_Id,
                    "HRML_LeaveCode": $scope.leavecode,
                    "HRMLD_NoOfDays": Number($scope.noofday),
                    "HRMLD_MaxLeaveApplicable": Number($scope.Maxl),
                    "Emp_types": $scope.selectedemptypesss,
                    "dept_types": $scope.selectedeptss,
                    "desig_types": $scope.selecteddesignation,
                    "grade_types": $scope.grade_namess,
                    "HRMLD_CarryForFlg": $scope.desigcarry,
                    "HRMLD_EncashFlg": $scope.desiggg,
                    "HRMLD_EncashAmount": $scope.amttxt,
                    //"HRMLD_EncashOn":0,
                    selectedMonthList: $scope.SelectedMonthList,

                    "HRMLDCF_MaxLeaveAplFlg": $scope.Maxcarry,
                    "HRMLDCF_MaxLeaveCF": $scope.maxleavecarry,
                    selectedcarryMonthList: $scope.SelectedcarryMonthList,

                    "HRMLDEC_ServiceAplFlg": $scope.Minserviceappl,
                    "HRMLDEC_ServiceYear": $scope.yearmin,
                    "HRMLDEC_ServiceMonth": $scope.monthmin,
                    "HRMLDEC_ServiceDays": $scope.daymin,
                    "HRMLDEC_MaxLeaveFlg": $scope.Maxleaveappl,
                    "HRMLDEC_MaxLeaves": $scope.maxla,
                    "HRMLDEC_MinLeaveFlg": $scope.activeminleaveappl,
                    "HRMLDEC_MinLeaves": $scope.minla,
                    "HRMLDEC_ScheduleFlg": $scope.Encash,
                    "HRMLDEC_ScheduleYear": $scope.ScheduleYear,
                    "HRMLDEC_ScheduleMonth": $scope.ScheduleMonth
                };

                apiService.create("LeaveCredit/SaveData", data).then(function (promise) {
                    if (promise.returnval == true) {
                        swal("Record Saved/Updated Successfully", "Success");
                        $state.reload();
                    }
                    if (promise.returnval == false) {
                        swal("Record Not Saved/Updated Successfully", "Success");
                    }
                });
                // $scope.BindData();
            }
            else {
                $scope.submitted = true;
                $scope.submitted1 = true;
                $scope.submitted2 = true;
                $scope.submitted3 = true;
                $scope.submitted4 = true;
            }
        };
    }
})();