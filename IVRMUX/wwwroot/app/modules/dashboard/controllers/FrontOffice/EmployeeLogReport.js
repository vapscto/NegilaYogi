(function () {
    'use strict';
    angular
        .module('app')
        .controller('EmployeeLogReportController', EmployeeLogReportController)

    EmployeeLogReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'apiService', 'Flash', '$http', '$q', '$stateParams', '$filter']
    function EmployeeLogReportController($rootScope, $scope, $state, $location, apiService, Flash, $http, $q, $stateParams, $filter) {
        $scope.maxDatemf = new Date();
        //loading start
        $scope.loadData = function () {


            var id = 2;

            apiService.getURI("EmployeeLogReport/getalldetails", id).
                then(function (promise) {

                    $scope.staff_types = promise.filltypes;
                    $scope.fillmonth = promise.fillmonth;
                    $scope.fillyear = promise.fillyear;
                    $scope.All_Individual('All');
                    $scope.rdoPunch = "punch";
                    $scope.exclude = "IA";
                    $scope.grid_view = false;
                })
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

        //loading end
        $scope.gettodate = function () {

            $scope.minDatemf = new Date($scope.fromdate);
            $scope.maxDatemf = new Date();
        };
        //validation start
        $scope.interacted = function (field) {

            return $scope.submitted || field.$dirty;
        };
        $scope.isOptionsRequired = function () {
            return !$scope.staff_types.some(function (options) {
                return options.selected;
            });
        }
        $scope.isOptionsRequired1 = function () {
            return !$scope.Department_types.some(function (options) {
                return options.selected;
            });
        }
        $scope.isOptionsRequired2 = function () {
            return !$scope.Designation_types.some(function (options) {
                return options.selected;
            });
        }
        $scope.sort = function (keyname) {

            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }
        //validation end
        //all-individual start
        $scope.All_Individual = function () {

            if ($scope.allind == 'Indi')
                $scope.disabledata = false;
            else
                $scope.disabledata = true;

        }
        //all-individual end
        //date change start
        $scope.datechange = function (datedata) {

            $scope.disabledate1 = true; $scope.disabledate2 = true; $scope.disabledate3 = true; $scope.disabledate4 = true; $scope.disabledate5 = true;
            $scope.Datepic = null; $scope.month = null; $scope.year = null; $scope.fromdate = null; $scope.todate = null;
            if (datedata == 'fromwise') {
                $scope.disabledate4 = false; $scope.disabledate5 = false;
            }
            else if (datedata == 'monthwise') {
                $scope.disabledate2 = false; $scope.disabledate3 = false;
            }
            else {
                $scope.disabledate1 = false;
            }
        }
        //date chage end
        //all check button start
        $scope.all_check = function () {

            var toggleStatus = $scope.usercheck;
            angular.forEach($scope.staff_types, function (itm) {
                itm.selected = toggleStatus;
            });
            $scope.get_departments();
        }
        $scope.all_checkdep = function () {

            var toggleStatus = $scope.deptcheck;
            angular.forEach($scope.Department_types, function (itm) {
                itm.selected = toggleStatus;
            });
            $scope.get_designation();
        }
        $scope.all_checkdesg = function () {

            var toggleStatus = $scope.desgcheck;
            angular.forEach($scope.Designation_types, function (itm) {
                itm.selected = toggleStatus;
            });
            $scope.get_employee();
        }
        //all-check button end
        // fill dep start
        $scope.get_departments = function () {
            $scope.usercheck = $scope.staff_types.every(function (options) {
                return options.selected;
            });
            $scope.deptcheck = "";
            $scope.desgcheck = "";

            var groupidss;
            for (var i = 0; i < $scope.staff_types.length; i++) {
                if ($scope.staff_types[i].selected == true) {

                    if (groupidss == undefined)
                        groupidss = $scope.staff_types[i].hrmgT_Id;
                    else
                        groupidss = groupidss + "," + $scope.staff_types[i].hrmgT_Id;
                }
            }
            if (groupidss != undefined) {
                var data = {
                    "multipletype": groupidss,
                }
                apiService.create("EmployeeLogReport/get_departments", data).
                    then(function (promise) {

                        $scope.Department_types = promise.filldepartment;
                        if ($scope.Department_types.length > 0) {
                            // $scope.Department_types[0].selected = true;
                            $scope.get_designation();
                        }

                    })
            }
            else {
                $scope.Department_types = "";
                $scope.Designation_types = "";
                $scope.Employeelst = "";
            }
        }
        //fill department end
        //fill desg start
        $scope.get_designation = function () {
            $scope.deptcheck = $scope.Department_types.every(function (options) {
                return options.selected;
            });

            $scope.get_designationnew();
        }
        $scope.get_designationnew = function () {
            $scope.desgcheck = "";
            var groupidss;
            for (var i = 0; i < $scope.Department_types.length; i++) {
                if ($scope.Department_types[i].selected == true) {

                    if (groupidss == undefined)
                        groupidss = $scope.Department_types[i].hrmD_Id;
                    else
                        groupidss = groupidss + "," + $scope.Department_types[i].hrmD_Id;
                }
            }
            if (groupidss != undefined) {
                var data = {
                    "multipledep": groupidss,
                }
                apiService.create("EmployeeLogReport/get_designation", data).
                    then(function (promise) {

                        $scope.Designation_types = promise.filldesignation;
                        if ($scope.Designation_types.length > 0) {
                            //  $scope.Designation_types[0].selected = true;
                            $scope.get_employee();
                        }
                    })
            }
            else {
                $scope.Designation_types = "";
                $scope.Employeelst = "";
            }
        }
        //fill desg end
        //fill employee start
        $scope.get_employee = function () {
            $scope.desgcheck = $scope.Designation_types.every(function (options) {

                return options.selected;
            });

            $scope.get_employeenew();
        }
        $scope.get_employeenew = function () {

            var typeIds;

            for (var i = 0; i < $scope.staff_types.length; i++) {
                if ($scope.staff_types[i].selected == true) {

                    if (typeIds == undefined)
                        typeIds = $scope.staff_types[i].hrmgT_Id;
                    else
                        typeIds = typeIds + "," + $scope.staff_types[i].hrmgT_Id;
                }
            }
            var deptIds;
            for (var i = 0; i < $scope.Department_types.length; i++) {
                if ($scope.Department_types[i].selected == true) {

                    if (deptIds == undefined)
                        deptIds = $scope.Department_types[i].hrmD_Id;
                    else
                        deptIds = deptIds + "," + $scope.Department_types[i].hrmD_Id;
                }
            }
            var groupidss;
            for (var i = 0; i < $scope.Designation_types.length; i++) {
                if ($scope.Designation_types[i].selected == true) {

                    if (groupidss == undefined)
                        groupidss = $scope.Designation_types[i].hrmdeS_Id;
                    else
                        groupidss = groupidss + "," + $scope.Designation_types[i].hrmdeS_Id;
                }
            }
            if (groupidss != undefined) {
                var data = {
                    "multipledes": groupidss,
                    "multipletype": typeIds,
                    "multipledep": deptIds
                }
                apiService.create("EmployeeLogReport/get_employee", data).
                    then(function (promise) {

                        $scope.Employeelst = promise.fillemployee;
                        //if ($scope.Employeelst.length > 0) {
                        //   // $scope.hideempse = false;
                        //   // $scope.Employeelst[0].Selected = true;
                        //    $scope.hrmE_Id = $scope.Employeelst[0].hrmE_Id;
                        //}
                    })
            }
            else {
                $scope.Employeelst = "";
            }
        }
        //fill employee end
        //get report start
        $scope.submitted = false;
        $scope.GetReport = function () {


            if ($scope.myForm.$valid) {
                var groupidss;
                if ($scope.allind == "All") {
                    for (var i = 0; i < $scope.Employeelst.length; i++) {
                        if (groupidss == undefined)
                            groupidss = $scope.Employeelst[i].hrmE_Id;
                        else
                            groupidss = groupidss + "," + $scope.Employeelst[i].hrmE_Id;
                    }

                }
                else {
                    groupidss = $scope.hrmE_Id;
                }
                if (groupidss != undefined) {

                    var Datepic1 = $scope.Datepic == null ? "" : $filter('date')($scope.Datepic, "yyyy-MM-dd");
                    var fromdate1 = $scope.fromdate == null ? "" : $filter('date')($scope.fromdate, "yyyy-MM-dd");
                    var todate1 = $scope.todate == null ? "" : $filter('date')($scope.todate, "yyyy-MM-dd");
                    var month1 = $scope.month == null ? "" : $scope.month;
                    var year1 = $scope.year == null ? "" : $scope.year;

                    //angular.forEach($scope.fillyear, function (test) {
                    //    if (test.hrmlY_Id == $scope.year) {
                    //        $scope.year1 = test.hrmlY_LeaveYear;
                    //    }
                    //} )


                    var data = {
                        "selectdate": Datepic1,
                        "selectmonth": month1,
                        "selectyear": year1,
                        "fromdate": fromdate1,
                        "todate": todate1,
                        "multiplehrmeid": groupidss,
                        "punchtype": $scope.rdoPunch,
                    }
                    apiService.create("EmployeeLogReport/getrpt", data).
                        then(function (promise) {

                            if (promise.filldata.length > 0) {

                                $scope.grid_view = true;
                                $scope.gridpunchdate = false;
                                $scope.gridpunchmonth = false;
                                $scope.gridlatedate = false;
                                $scope.gridlatemonth = false;
                                $scope.gridearlydate = false;
                                $scope.gridearlymonth = false;
                                $scope.punchtypenew = promise.punchtype;
                                $scope.selectdatenew = promise.selectdate;
                                $scope.yearlyemprep = [];
                                if (promise.punchtype == "punch") {

                                    if (promise.selectdate != "") {
                                        $scope.gridpunchdate = true;
                                        $scope.datenew = $filter('date')(promise.filldata[0].punchdate, "dd-MM-yyyy");
                                        $scope.headformonth = ["punchdate", "punchday", "intime", "outtime", "workingtime"];
                                        //  $scope.yearlyemprep = promise.filldata;
                                        var cutoffminuites = 0;
                                        for (var i = 0; i < promise.filldata.length; i++) {

                                            var cutoffminuites = $scope.convertH2M(promise.filldata[i].intime)
                                            if (cutoffminuites > 420) {
                                                $scope.latein = "lateinearly";
                                            }
                                            else {
                                                $scope.latein = "";
                                            }

                                            var newCol = { ecode: promise.filldata[i].ecode, ename: promise.filldata[i].ename, desgname: promise.filldata[i].desgname, depname: promise.filldata[i].depname, gtype: promise.filldata[i].gtype, intemperature: promise.filldata[i].intemperature, punchdate: $filter('date')(promise.filldata[i].punchdate, "dd-MM-yyyy"), punchday: promise.filldata[i].punchday, intime: promise.filldata[i].intime, latein: $scope.latein, outtime: promise.filldata[i].outtime, workingtime: promise.filldata[i].workingtime, lateby: promise.filldata[i].lateby, earlyby: promise.filldata[i].earlyby }
                                            $scope.yearlyemprep.push(newCol);
                                        }
                                    }
                                    else {
                                        $scope.gridpunchmonth = true;
                                        var temp_array = [];
                                        var temp_array1 = [];
                                        for (var i = 0; i < promise.filldata.length; i++) {

                                            var cutoffminuites = $scope.convertH2M(promise.filldata[i].intime)
                                            if (cutoffminuites > 420) {
                                                $scope.latein = "lateinearly";
                                            }
                                            else {
                                                $scope.latein = "";
                                            }

                                            var newCol = { punchdate: $filter('date')(promise.filldata[i].punchdate, "dd-MM-yyyy"), punchday: promise.filldata[i].punchday, intime: promise.filldata[i].intime, latein: $scope.latein, outtime: promise.filldata[i].outtime, workingtime: promise.filldata[i].workingtime }
                                            temp_array.push(newCol);
                                            if (i < promise.filldata.length - 1) {
                                                if (promise.filldata[i].ecode == promise.filldata[i + 1].ecode)
                                                    continue;
                                            }
                                            var newCol1 = { punchtime: temp_array, ecode: promise.filldata[i].ecode, ename: promise.filldata[i].ename, depname: promise.filldata[i].depname, desgname: promise.filldata[i].desgname, gtype: promise.filldata[i].gtype }
                                            temp_array1.push(newCol1);
                                            temp_array = [];
                                        }
                                        $scope.yearlyemprep = temp_array1;
                                    }
                                }
                                else if (promise.punchtype == "late") {

                                    if (promise.selectdate != "") {
                                        $scope.gridlatedate = true;
                                        $scope.yearlyemprep = promise.filldata;
                                    }
                                    else {
                                        $scope.gridlatemonth = true;
                                        var temp_array = [];
                                        var temp_array1 = [];
                                        for (var i = 0; i < promise.filldata.length; i++) {

                                            var newCol = { punchdate: promise.filldata[i].punchdate, punchday: promise.filldata[i].punchday, intime: promise.filldata[i].intime, actualtime: promise.filldata[i].actualtime, relaxtime: promise.filldata[i].relaxtime, lateby: promise.filldata[i].lateby }
                                            temp_array.push(newCol);
                                            if (i < promise.filldata.length - 1) {
                                                if (promise.filldata[i].ecode == promise.filldata[i + 1].ecode)
                                                    continue;
                                            }
                                            var newCol1 = { punchtime: temp_array, ecode: promise.filldata[i].ecode, ename: promise.filldata[i].ename, depname: promise.filldata[i].depname, desgname: promise.filldata[i].desgname, gtype: promise.filldata[i].gtype }
                                            temp_array1.push(newCol1);
                                            temp_array = [];
                                        }
                                        $scope.yearlyemprep = temp_array1;
                                    }
                                }
                                else if (promise.punchtype == "early") {

                                    if (promise.selectdate != "") {
                                        $scope.gridearlydate = true;
                                        $scope.yearlyemprep = promise.filldata;
                                    }
                                    else {
                                        $scope.gridearlymonth = true;
                                        var temp_array = [];
                                        var temp_array1 = [];
                                        for (var i = 0; i < promise.filldata.length; i++) {

                                            var newCol = { punchdate: $filter('date')(promise.filldata[i].punchdate, "dd-MM-yyyy"), punchday: promise.filldata[i].punchday, outtime: promise.filldata[i].outtime, actualtime: promise.filldata[i].actualtime, relaxtime: promise.filldata[i].relaxtime, earlyby: promise.filldata[i].earlyby }
                                            temp_array.push(newCol);
                                            if (i < promise.filldata.length - 1) {
                                                if (promise.filldata[i].ecode == promise.filldata[i + 1].ecode)
                                                    continue;
                                            }
                                            var newCol1 = { punchtime: temp_array, ecode: promise.filldata[i].ecode, ename: promise.filldata[i].ename, depname: promise.filldata[i].depname, desgname: promise.filldata[i].desgname, gtype: promise.filldata[i].gtype }
                                            temp_array1.push(newCol1);
                                            temp_array = [];
                                        }
                                        $scope.yearlyemprep = temp_array1;
                                    }
                                }


                                for (var i = 0; i < $scope.yearlyemprep.length; i++) {
                                    $scope.yearlyemprep[i].punchtimeing = [];
                                    $scope.yearlyemprep[i].punchtimeing = $scope.yearlyemprep[i].punchtime;
                                    $scope.yearlyemprep[i].punchtime = [];
                                    for (var j = 0; j < $scope.yearlyemprep[i].punchtimeing.length; j++) {
                                        if ($scope.exclude == "B") {
                                            if ($scope.yearlyemprep[i].punchtimeing[j].intime != "A" && $scope.yearlyemprep[i].punchtimeing[j].intime != "H") {
                                                $scope.yearlyemprep[i].punchtime.push($scope.yearlyemprep[i].punchtimeing[j])
                                            }
                                        }
                                        else if ($scope.exclude == "A") {
                                            if ($scope.yearlyemprep[i].punchtimeing[j].intime != "A") {
                                                $scope.yearlyemprep[i].punchtime.push($scope.yearlyemprep[i].punchtimeing[j])
                                            }
                                        }
                                        else if ($scope.exclude == "H") {
                                            if ($scope.yearlyemprep[i].punchtimeing[j].intime != "H") {
                                                $scope.yearlyemprep[i].punchtime.push($scope.yearlyemprep[i].punchtimeing[j])
                                            }
                                        }
                                        else if ($scope.exclude == "IA") {
                                            $scope.yearlyemprep[i].punchtime.push($scope.yearlyemprep[i].punchtimeing[j])
                                        }
                                    }


                                }
                            }
                            else {
                                $scope.grid_view = false;
                                swal("Record not found.");
                            }
                        })
                }
            }
            else {
                $scope.submitted = true;
            }
        }

        $scope.convertH2M = function (timeInHour) {
            var timeParts = timeInHour.split(":");
            return Number(timeParts[0]) * 60 + Number(timeParts[1]);
        }

        //get report end
        //print start
        $scope.printData = function () {

            if ($scope.punchtypenew == "punch") {
                if ($scope.selectdatenew != "") { var divToPrint = document.getElementById("table1"); }
                else { var divToPrint = document.getElementById("table2"); }

            }
            else if ($scope.punchtypenew == "late") {
                if ($scope.selectdatenew != "") { var divToPrint = document.getElementById("table3"); }
                else { var divToPrint = document.getElementById("table4"); }

            }
            else if ($scope.punchtypenew == "early") {
                if ($scope.selectdatenew != "") { var divToPrint = document.getElementById("table5"); }
                else { var divToPrint = document.getElementById("table6"); }
            }

            var newWin = window.open("");
            newWin.document.write(divToPrint.outerHTML);
            newWin.print();
            newWin.close();
        }
        //print end


        $scope.exptoex = function () {

            $scope.gridpunchdate = false;
            $scope.gridpunchmonth = false;
            $scope.gridlatedate = false;
            $scope.gridlatemonth = false;
            $scope.gridearlydate = false;
            $scope.gridearlymonth = false;

            if ($scope.punchtypenew == "punch") {
                if ($scope.selectdatenew != "") {
                    $scope.gridpunchdate = true;
                    var divToPrint = document.getElementById("table1");
                    var blob = new Blob([divToPrint.outerHTML], {
                        type: "application/vnd.ms-excel;charset=utf-8"
                    });
                    saveAs(blob, "Punch Report of " + $scope.datenew + ".xls");
                }
                else {
                    $scope.gridpunchmonth = true;
                    var htmldiv = "";
                    for (var x = 0; x < $scope.yearlyemprep.length; x++) {
                        $scope.tableId = 'tablep' + (x + 1);
                        if (htmldiv == "")
                            htmldiv = document.getElementById($scope.tableId).outerHTML;
                        else
                            htmldiv = htmldiv + "<br>" + document.getElementById($scope.tableId).outerHTML;

                    }
                    var blob = new Blob([htmldiv], {
                        type: "application/vnd.ms-excel;charset=utf-8"
                    });
                    saveAs(blob, "Punch Report.xls");
                }
            }
            else if ($scope.punchtypenew == "late") {
                if ($scope.selectdatenew != "") {
                    $scope.gridlatedate = true;
                    var divToPrint = document.getElementById("table3");
                    var blob = new Blob([divToPrint.outerHTML], {
                        type: "application/vnd.ms-excel;charset=utf-8"
                    });
                    saveAs(blob, "Late By Report of " + $scope.datenew + ".xls");
                }
                else {
                    $scope.gridlatemonth = true;
                    var htmldiv = "";
                    for (var x = 0; x < $scope.yearlyemprep.length; x++) {
                        $scope.tableId = 'tablel' + (x + 1);
                        if (htmldiv == "")
                            htmldiv = document.getElementById($scope.tableId).outerHTML;
                        else
                            htmldiv = htmldiv + "<br>" + document.getElementById($scope.tableId).outerHTML;

                    }
                    var blob = new Blob([htmldiv], {
                        type: "application/vnd.ms-excel;charset=utf-8"
                    });
                    saveAs(blob, "Late By Report.xls");


                    //for (var x = 0; x < $scope.yearlyemprep.length; x++) {
                    //    $scope.tableId = '' + (x + 1);
                    //    var blob = new Blob([document.getElementById($scope.tableId).outerHTML], {
                    //        type: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;charset=utf-8"
                    //    });
                    //    saveAs(blob, " of " + $scope.yearlyemprep[x].ename + ".xls");
                    //}
                }
            }
            else if ($scope.punchtypenew == "early") {
                if ($scope.selectdatenew != "") {
                    $scope.gridearlydate = true;
                    var divToPrint = document.getElementById("table5");
                    var blob = new Blob([divToPrint.outerHTML], {
                        type: "application/vnd.ms-excel;charset=utf-8"
                    });
                    saveAs(blob, "Early By Report of " + $scope.datenew + ".xls");
                }
                else {
                    $scope.gridearlymonth = true;
                    var htmldiv = "";
                    for (var x = 0; x < $scope.yearlyemprep.length; x++) {
                        $scope.tableId = 'tablee' + (x + 1);
                        if (htmldiv == "")
                            htmldiv = document.getElementById($scope.tableId).outerHTML;
                        else
                            htmldiv = htmldiv + "<br>" + document.getElementById($scope.tableId).outerHTML;

                    }
                    var blob = new Blob([htmldiv], {
                        type: "application/vnd.ms-excel;charset=utf-8"
                    });
                    saveAs(blob, "Early By Report.xls");
                    //for (var x = 0; x < $scope.yearlyemprep.length; x++) {
                    //    $scope.tableId = 'tablee' + (x + 1);
                    //    var blob = new Blob([document.getElementById($scope.tableId).outerHTML], {
                    //        type: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;charset=utf-8"
                    //    });
                    //    saveAs(blob, "Early By Report of " + $scope.yearlyemprep[x].ename + ".xls");
                    //}
                }
            }
        };



        $scope.exlcudetype = function (exclude) {
            $scope.yearlyemprep = [];
            $scope.yearlyemprep = $scope.inouttiming;
            if ($scope.gridpunchmonth == true) {
                for (var i = 0; i < $scope.yearlyemprep.length; i++) {
                    $scope.yearlyemprep[i].punchtimeing = [];
                    $scope.yearlyemprep[i].punchtimeing = $scope.yearlyemprep[i].punchtime;
                    $scope.yearlyemprep[i].punchtime = [];
                    for (var j = 0; j < $scope.yearlyemprep[i].punchtimeing.length; j++) {
                        if (exclude == "B") {
                            if ($scope.yearlyemprep[i].punchtimeing[j].intime != "A" && $scope.yearlyemprep[i].punchtimeing[j].intime != "H") {
                                $scope.yearlyemprep[i].punchtime.push($scope.yearlyemprep[i].punchtimeing[j])
                            }
                        }
                        else if (exclude == "A") {
                            if ($scope.yearlyemprep[i].punchtimeing[j].intime != "A") {
                                $scope.yearlyemprep[i].punchtime.push($scope.yearlyemprep[i].punchtimeing[j])
                            }
                        }
                        else if (exclude == "H") {
                            if ($scope.yearlyemprep[i].punchtimeing[j].intime != "H") {
                                $scope.yearlyemprep[i].punchtime.push($scope.yearlyemprep[i].punchtimeing[j])
                            }
                        }
                        else if (exclude == "IA") {
                            $scope.yearlyemprep[i].punchtime.push($scope.yearlyemprep[i].punchtimeing[j])
                        }
                    }

                }
            }

        };

        //export end
        //TO clear  data
        $scope.clearid = function () {
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
            $scope.submitted = false;
            $scope.Department_types = "";
            $scope.Designation_types = "";
            $scope.Employeelst = "";
            $scope.usercheck = "";
            $scope.deptcheck = "";
            $scope.desgcheck = "";
            $scope.allind = "All";
            $scope.Datepic = "";
            $scope.year = "";
            $scope.month = "";
            $scope.fromdate = "";
            $scope.todate = "";
            $scope.rdoPunch = "punch";

            $scope.disabledata = true;
            $scope.grid_view = false;
            for (var i = 0; i < $scope.staff_types.length; i++) {
                $scope.staff_types[i].selected = false;
            }
        };
        //clear end

    }
})(); 