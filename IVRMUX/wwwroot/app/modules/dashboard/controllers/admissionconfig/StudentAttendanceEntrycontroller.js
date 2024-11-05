(function () {
    'use strict';
    angular.module('app').controller('studentAttendanceEntrycontroller', studentAttendanceEntrycontroller);
    studentAttendanceEntrycontroller.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$filter', 'superCache', '$stateParams'];

    function studentAttendanceEntrycontroller($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $filter, superCache, $stateParams) {
        $scope.chckedIndexs = [];
        $scope.unchckedIndexs = [];
        $scope.albumNameArraycolumn = [];
        $scope.columnsTest = [];

        $scope.SaveDis = false;
        $scope.IsHiddenup = true;
        $scope.IsHiddendown = false;
        $scope.currentPage = 1;
        //$scope.itemsPerPage = 10;       

        $scope.attcount = false;
        $scope.attcount1 = false;
        $scope.batchno = true;        
        $scope.userPrivileges = "";
        var pageid = $stateParams.pageId;
        var privlgs = JSON.parse(localStorage.getItem("privileges"));
        for (var i = 0; i < privlgs.length; i++) {
            if (privlgs[i].pageId == pageid) {
                $scope.userPrivileges = privlgs[i];
            }
        }
        $scope.att = {};
        $scope.att.ASA_Regular_Extra = 'Regular';
        $scope.searchValue_att_del = "";

        //disable the sundays in calender
        $scope.onlyWeekendsPredicate = function (date) {
            var day = date.getDay();
            return day === 1 || day === 2 || day === 3 || day === 4 || day === 5 || day === 6;
        };

        $scope.AttendenceCheckDis = true;

        $scope.loadInitialData = function () {
            var dataf, datat;
            var yearid = 0;

            $scope.classList = [];
            $scope.att.asmcL_Id = "";
            $scope.att.entry = "";
            $scope.entry_flag = false;

            $scope.sectionList = [];
            $scope.att.asmC_Id = "";

            $scope.att.ASA_Entry_DateTime = null;


            if ($scope.att.asmaY_Id === undefined || $scope.att.asmaY_Id === null || $scope.att.asmaY_Id === "") {
                yearid = 0;
            } else {
                yearid = $scope.att.asmaY_Id;
            }

            apiService.getURI("StudentAttendanceEntry/loadinitialdata", yearid).then(function (promise) {
                if (promise.message !== null) {
                    swal(promise.message);
                }

                $scope.academicYearList = promise.academicYearList;
                $scope.currentYear = promise.currentYear;
                //Current Year load and Date Validation Code Starts.
                for (var i = 0; i < $scope.academicYearList.length; i++) {
                    name = $scope.academicYearList[i].asmaY_Id;
                    for (var j = 0; j < $scope.currentYear.length; j++) {
                        if (name == $scope.currentYear[j].asmaY_Id) {
                            $scope.academicYearList[i].Selected = true;
                            $scope.att.asmaY_Id = $scope.currentYear[j].asmaY_Id;

                            if (yearid === 0) {
                                $scope.att.ASA_Entry_DateTime = new Date();
                            } else {
                                $scope.att.ASA_Entry_DateTime = new Date($scope.currentYear[j].asmaY_From_Date);
                            }
                            dataf = new Date($scope.currentYear[j].asmaY_From_Date);
                            datat = new Date($scope.currentYear[j].asmaY_To_Date);
                            $scope.fDate = dataf;
                            $scope.tDate = datat;
                            $scope.dd = new Date();

                            $scope.minDatedof = new Date(
                                $scope.fDate.getFullYear(),
                                $scope.fDate.getMonth(),
                                $scope.fDate.getDate());

                            $scope.maxDatedof = new Date(
                                $scope.dd.getFullYear(),
                                $scope.dd.getMonth(),
                                $scope.dd.getDate());

                            $scope.minDatedtf = new Date(
                                $scope.fDate.getFullYear(),
                                $scope.fDate.getMonth(),
                                $scope.fDate.getDate());

                            $scope.maxDatedtf = new Date(
                                $scope.dd.getFullYear(),
                                $scope.dd.getMonth(),
                                $scope.dd.getDate());

                            $scope.minDatemf = new Date(
                                $scope.fDate.getFullYear(),
                                $scope.fDate.getMonth(),
                                $scope.fDate.getDate());

                            $scope.maxDatemf = new Date(
                                $scope.dd.getFullYear(),
                                $scope.dd.getMonth(),
                                $scope.dd.getDate());

                            $scope.minDatedf = new Date(
                                $scope.fDate.getFullYear(),
                                $scope.fDate.getMonth(),
                                $scope.fDate.getDate());

                            $scope.maxDatedf = new Date(
                                $scope.dd.getFullYear(),
                                $scope.dd.getMonth(),
                                $scope.dd.getDate());

                            $scope.att.asmcL_Id = "";
                            $scope.att.asmC_Id = "";
                            $scope.studentlist = [];
                            $scope.studentgrid = false;
                            $scope.entry_flag = false;
                        }
                    }
                }
                $scope.asmaYFromDate = $scope.currentYear[0].asmaY_From_Date;

                var date = new Date($scope.asmaYFromDate);

                $scope.monthNames = ["January", "February", "March", "April", "May", "June",
                    "July", "August", "September", "October", "November", "December"
                ];
                $scope.months = [];
                $scope.year = [];

                for (var i = 0; i < 12; i++) {
                    $scope.months.push($scope.monthNames[date.getMonth()]);
                    $scope.year.push(date.getFullYear());
                    // Add a month each time
                    date.setMonth(date.getMonth() + 1);
                }
                $scope.monthByOrder = [];
                for (var i = 0; i < $scope.months.length; i++) {
                    name = $scope.months[i].trim();
                    for (var j = 0; j < promise.monthList.length; j++) {
                        if (name.toLowerCase() == promise.monthList[j].ivrM_Month_Name.toLowerCase().trim()) {
                            $scope.monthByOrder.push(promise.monthList[j]);
                        }
                    }
                }
                $scope.monthList = $scope.monthByOrder;

                //Current Year load and Date Validation Code Ends.
                $scope.classList = promise.classList;
                $scope.sectionList = promise.sectionList;
                //$scope.monthList = promise.monthList;
                $scope.subjectList = promise.subjectList;
                $scope.batchList = promise.batchList;
                $scope.periodlist = promise.periodlist;

                $scope.att.ASA_FromDate = new Date();
                $scope.att.ASA_ToDate = new Date();
            });
        };

        $scope.isOptionsRequired = function () {
            return !$scope.studentlist.some(function (options) {
                return options.Selected;
            });
        };

        $scope.submitted = false;

        $scope.SaveStudentAttendance = function (att) {
            var fromdate = "";
            var todate = "";
            var ASA_Class_Attended;
            var asasB_Id;
            if ($scope.myForm.$valid) {

                var ASA_ClassHeld;
                if ($scope.att.entry == 'Dailyonce') {

                    angular.forEach($scope.studentlist, function (user) {
                        if (user.pdays != 0) {
                            user.pdays = 0;
                        }
                        //$scope.studentlist.push(user);
                    })

                    fromdate = new Date($scope.att.ASA_Entry_DateTime).toDateString();
                    todate = new Date($scope.att.ASA_Entry_DateTime).toDateString();
                    ASA_ClassHeld = '1.00';
                    ASA_Class_Attended = '1.0';
                    asasB_Id = 0;
                }

                if ($scope.att.entry == 'Dailytwice') {

                    angular.forEach($scope.studentlist, function (user) {
                        if (user.pdays != 0) {
                            user.pdays = 0;
                        }
                        //$scope.studentlist.push(user);
                    })


                    if ($scope.att.ASA_ClassHeld1 == true || $scope.att.ASA_ClassHeld2 == true) {
                        ASA_ClassHeld = '0.5';
                    }
                    if ($scope.att.ASA_ClassHeld1 == true && $scope.att.ASA_ClassHeld2 == true) {
                        ASA_ClassHeld = '1.00';
                    }
                    if ($scope.att.ASA_ClassHeld1 == false && $scope.att.ASA_ClassHeld2 == false) {
                        swal("Kindy Select 1st Half/2nd Half Or both");
                        return;
                    }

                    fromdate = new Date($scope.att.ASA_Entry_DateTime).toDateString();
                    todate = new Date($scope.att.ASA_Entry_DateTime).toDateString();
                    ASA_ClassHeld = '1.00';
                    asasB_Id = 0;

                }
                if ($scope.att.entry == 'monthly') {
                    //var countclass = parseInt($scope.countclass);
                    var countclass = ($scope.countclass);
                    ASA_ClassHeld = countclass;
                    ASA_Class_Attended = '0.0';
                    fromdate = new Date($scope.att.ASA_FromDate).toDateString();
                    todate = new Date($scope.att.ASA_ToDate).toDateString();
                    asasB_Id = 0;
                }
                if ($scope.att.entry == 'period') {

                    angular.forEach($scope.studentlist, function (user) {
                        if (user.pdays != 0) {
                            user.pdays = 0;
                        }
                        //$scope.studentlist.push(user);
                    })
                    fromdate = new Date($scope.att.ASA_Entry_DateTime).toDateString();
                    todate = new Date($scope.att.ASA_Entry_DateTime).toDateString();
                    ASA_ClassHeld = '1.00';
                    if ($scope.att.asasB_Id == "") {
                        asasB_Id = 0;
                    } else {
                        asasB_Id = $scope.att.asasB_Id;
                    }
                }

                //var entrydate = new Date($scope.att.ASA_Entry_DateTime).toDateString();
                var entrydate = new Date().toDateString();


                var dataaaaaa = {
                    "ASA_Id": $scope.att.asA_Id,
                    "ismS_Id": $scope.att.ismS_Id,
                    "ASMAY_Id": $scope.att.asmaY_Id,
                    "ASA_Att_Type": $scope.att.entry,
                    "ASMCL_Id": $scope.att.asmcL_Id,
                    "ASMS_Id": $scope.att.asmC_Id,
                    "TTMP_Id": $scope.att.ttmP_Id,
                    "ASA_Entry_DateTime": entrydate,
                    "ASA_FromDate": fromdate,
                    "ASA_ToDate": todate,
                    "ASA_ClassHeld": ASA_ClassHeld,
                    "ASA_Regular_Extra": $scope.att.ASA_Regular_Extra,
                    "asasB_Id": asasB_Id,
                    stdList: $scope.studentlist
                };
                apiService.create("StudentAttendanceEntry/", dataaaaaa).then(function (promise) {
                    if (promise.message != "" && promise.message != null) {
                        swal(promise.message);
                        return;
                    }
                    if (promise.returnval === true) {
                        swal('Record Saved Successfully!', 'success');
                        loadInitialData();
                        $state.reload();
                    }
                    else {
                        swal('Record Not Saved Successfully!', 'Failed');
                        // loadInitialData();
                        $state.reload();
                    }
                });
            }

            else {
                $scope.submitted = true;
            }
        };

        //student details by Class
        $scope.getStudentBYClass = function () {
            $scope.att.asmC_Id = "";
            $scope.AttendenceCheckDis = true;
            $scope.studentgrid = false;
            $scope.studentlist = [];
            var acedamicId = $scope.att.asmaY_Id;
            var classId = $scope.att.asmcL_Id;
            if (acedamicId === undefined || acedamicId === "") {
                acedamicId = 0;
            }
            if (classId === undefined || classId === "") {
                classId = 0;
            }

            var data = {
                "ASMAY_Id": acedamicId,
                "ASMCL_Id": classId,
                "classsecflag": '1'
            };

            if (acedamicId !== undefined && acedamicId !== "" && classId !== undefined && classId !== "") {

                apiService.create("StudentAttendanceEntry/getstudentdata/", data).then(function (promise) {

                    if (promise.message === null) {
                        if (promise.studentList !== null && promise.studentList.length > 0) {
                            $scope.studentgrid = false;
                            $scope.entry_flag = true;

                            // $scope.studentlist = promise.studentList;

                            if (promise.asA_Att_EntryType === 'D') {
                                $scope.att.entry = 'Dailyonce';
                            }
                            else if (promise.asA_Att_EntryType === 'H') {
                                $scope.att.entry = 'Dailytwice';
                            }
                            if (promise.asA_Att_EntryType === 'M') {
                                $scope.att.entry = 'monthly';
                            }
                            if (promise.asA_Att_EntryType === 'P') {
                                $scope.att.entry = 'period';
                                $scope.attcount = true;
                                $scope.attcount1 = true;
                            }
                        }
                        else {
                            $scope.studentgrid = false;
                            $scope.entry_flag = false;
                            swal("No Records Found");
                        }
                        $scope.sectionList = promise.sectionList;
                    }
                    else {
                        swal(promise.message);
                    }
                });
            }
        };

        var savecount = 0;
        var updatecount = 0;
        var deletecount = 0;

        // Get Student by Criteria section
        $scope.getStudent = function () {
            $scope.studentgrid = false;
            $scope.studentlist = [];
            $scope.att.ivrM_Month_Id = "";

            var acedamicId = $scope.att.asmaY_Id;
            var classId = $scope.att.asmcL_Id;
            var sectionId = $scope.att.asmC_Id;
            var fromdate = new Date($scope.att.ASA_Entry_DateTime).toDateString();
            if (acedamicId === undefined || acedamicId === "") {
                acedamicId = 0;
            }
            if (classId === undefined || classId === "") {
                classId = 0;
            }
            if (sectionId === undefined || sectionId === "") {
                sectionId = 0;
            }

            var entry;
            if ($scope.att.entry === 'Dailyonce') {
                entry = 'D';
            }
            else if ($scope.att.entry === 'Dailytwice') {
                entry = 'H';
            }
            else if ($scope.att.entry === 'monthly') {
                entry = 'M';
            }
            else if ($scope.att.entry === 'period') {
                entry = 'P';
            }

            var data = {
                "ASMAY_Id": acedamicId,
                "ASMCL_Id": classId,
                "ASMS_Id": sectionId,
                "monthflag1": $scope.att.entry,
                "ASA_FromDate": fromdate,
                "monthflag": entry,
                "checksubject": '1'
            };

            if (acedamicId !== undefined && acedamicId !== "" && classId !== undefined && classId !== "") {

                apiService.create("StudentAttendanceEntry/getstudentdata/", data).then(function (promise) {
                    if (promise.studentList !== null && promise.studentList.length > 0 && ($scope.att.asmC_Id !== undefined)) {
                        $scope.studentlist = [];
                        $scope.AttendenceCheckDis = false;
                        $scope.studentgrid = true;
                        $scope.entry_flag = true;

                        $scope.configuration = promise.admissionstandarad;

                        if ($scope.configuration.length > 0) {

                            if ($scope.configuration[0].asC_Att_Default_OrderFlag == 1) {
                                $scope.sortKey = "studentname";
                            }
                            else if ($scope.configuration[0].asC_Att_Default_OrderFlag == 2) {
                                $scope.sortKey = "studentname";
                            }
                            else if ($scope.configuration[0].asC_Att_Default_OrderFlag == 3) {
                                $scope.sortKey = "amaY_RollNo";
                            }
                            else if ($scope.configuration[0].asC_Att_Default_OrderFlag == 4) {
                                $scope.sortKey = "studentname";
                            }
                            else if ($scope.configuration[0].asC_Att_Default_OrderFlag == 5) {
                                $scope.sortKey = "studentname";
                            }
                            else if ($scope.configuration[0].asC_Att_Default_OrderFlag == 6) {
                                $scope.sortKey = "amsT_RegistrationNo";
                            }
                            else if ($scope.configuration[0].asC_Att_Default_OrderFlag == 7) {
                                $scope.sortKey = "amsT_AdmNo";
                            }
                            else {
                                $scope.sortKey = "studentname";
                            }

                        } else {
                            $scope.sortKey = "studentname";
                        }

                        var count = 0;
                        if (promise.getstandarad.length > 0) {
                            if (promise.getstandarad[0].ivrmgC_AdmnoColumnDisplay == 1) {
                                $scope.showadmno = true;
                                count = count + 1;
                            } else {
                                $scope.showadmno = false;
                            }

                            if (promise.getstandarad[0].ivrmgC_RegnoColumnDisplay == 1) {
                                $scope.showregno = true;
                                count = count + 1;
                            } else {
                                $scope.showregno = false;
                            }

                            if (promise.getstandarad[0].ivrmgC_RollnoColumnDisplay == 1) {
                                $scope.showrollno = true;
                                count = count + 1;
                            } else {
                                $scope.showrollno = false;
                            }
                        }
                        if (count == 0) {
                            $scope.showadmno = true;
                            $scope.showrollno = true;
                        }



                        //if (promise.attendanceentryflag == 'A') {
                        //    $scope.attentrytype = "Attendance Entry Type Is Absent";
                        //} else {
                        //    $scope.attentrytype = "Attendance Entry Type Is Present";
                        //}

                        if (promise.asA_Att_EntryType != 'M') {
                            $scope.attentrytype = promise.attendanceentryflag;
                            if (promise.attendanceentryflag == 'A') {
                                $scope.attentrytype = "Attendance Entry Type Is Absent";
                            } else {
                                $scope.attentrytype = "Attendance Entry Type Is Present";
                            }
                        }


                        //$scope.studentlist = promise.studentList;
                        if (promise.asA_Att_EntryType == 'D') {
                            $scope.att.entry = 'Dailyonce';
                            if (promise.studentList.length > 0) {

                                savecount = promise.countclass1;

                                if (promise.attendanceentryflag == 'A') {
                                    angular.forEach(promise.studentList, function (user) {
                                        if (user.pdays == 0) {
                                            user.selected = true;
                                            //savecount = user.asA_Id;
                                        } else {
                                            user.selected = false;
                                            //savecount = user.asA_Id;
                                        }
                                        $scope.studentlist.push(user);
                                    });
                                }
                                else if (promise.attendanceentryflag == 'P') {
                                    angular.forEach(promise.studentList, function (user) {
                                        if (user.pdays > 0) {
                                            user.selected = true;
                                            //savecount = user.asA_Id;
                                        } else {
                                            user.selected = false;
                                            //savecount = user.asA_Id;
                                        }
                                        $scope.studentlist.push(user);
                                    });
                                }
                            }
                        }
                        else if (promise.asA_Att_EntryType == 'H') {
                            $scope.att.entry = 'Dailytwice';

                            if (promise.studentList.length > 0) {
                                savecount = promise.countclass1;
                                angular.forEach(promise.studentList, function (user) {
                                    if (promise.attendanceentryflag == 'A') {
                                        if (user.pdays == 0.5) {
                                            if (user.asA_Dailytwice_Flag == "firsthalf") {
                                                user.FirstHalfflag = false;
                                                user.SecondHalfflag = true;
                                                //savecount = user.asA_Id;
                                            }
                                            else if (user.asA_Dailytwice_Flag == "Secondhalf") {
                                                user.SecondHalfflag = false;
                                                user.FirstHalfflag = true;
                                                //savecount = user.asA_Id;
                                            }

                                        } else if (user.pdays == 1) {
                                            user.FirstHalfflag = false;
                                            user.SecondHalfflag = false;
                                            //savecount = user.asA_Id;
                                        }
                                        else if (user.pdays == 0) {
                                            user.FirstHalfflag = true;
                                            user.SecondHalfflag = true;
                                            // savecount = user.asA_Id;
                                        }
                                        else {
                                            user.FirstHalfflag = false;
                                            user.SecondHalfflag = false;
                                            // savecount = user.asA_Id;
                                        }
                                        $scope.studentlist.push(user);
                                    }

                                    else if (promise.attendanceentryflag == 'P') {
                                        if (user.pdays == 0.5) {
                                            if (user.asA_Dailytwice_Flag == "firsthalf") {
                                                user.FirstHalfflag = true;
                                                user.SecondHalfflag = false;
                                                //savecount = user.asA_Id;
                                            }
                                            else if (user.asA_Dailytwice_Flag == "Secondhalf") {
                                                user.SecondHalfflag = true;
                                                user.FirstHalfflag = false;
                                                // savecount = user.asA_Id;
                                            }

                                        } else if (user.pdays == 1) {
                                            user.FirstHalfflag = true;
                                            user.SecondHalfflag = true;
                                            //savecount = user.asA_Id;
                                        }
                                        else {
                                            user.FirstHalfflag = false;
                                            user.SecondHalfflag = false;
                                            //savecount = user.asA_Id;
                                        }
                                        $scope.studentlist.push(user);
                                    }
                                });
                            }
                        }
                        if (promise.asA_Att_EntryType == 'M') {
                            $scope.att.entry = 'monthly';
                            //$scope.studentlist = promise.studentList;
                            $scope.studentgrid = false;
                        }
                        if (promise.asA_Att_EntryType == 'P') {
                            $scope.att.entry = 'period';
                            $scope.subjectList = promise.subjectList;
                            $scope.studentgrid = false;
                            $scope.attcount = false;
                            $scope.attcount1 = true;

                        }
                    }
                    else {
                        //  $scope.studentgrid = false;
                        //  $scope.entry_flag = false;
                        $scope.att.asmC_Id = "";
                        $scope.studentlist = [];
                        swal("No Records Found for selected Section");
                        //$scope.getStudentBYClass();
                        user.FirstHalfflag = false;
                        user.SecondHalfflag = false;
                        user.selected = false;
                        $scope.attcount = true;
                        $scope.attcount1 = true;
                    }
                    $scope.update = savecount;
                });
            }
        };

        $scope.addtotempscope = function (data) {
            if ($scope.chckedIndexs.indexOf(data) === -1) {
                $scope.chckedIndexs.push(data);
            }
            else {
                $scope.chckedIndexs.splice($scope.chckedIndexs.indexOf(data), 1);

                var index1 = $scope.noOfDays.indexOf(data.pdays);
                $scope.noOfDays.splice(index1);
                $scope.unchckedIndexs.push(data);
            }
            console.log($scope.chckedIndexs);
        };

        //validating the class held with number of present days
        $scope.addtopdays = function (user) {
            if (user.pdays != "" && user.pdays != undefined && user.pdays != null) {
                //var countclass = parseInt($scope.countclass);
                var countclass = ($scope.countclass);

                if (user.pdays > countclass) {
                    if (countclass == 0) {
                        user.pdays = 0;
                        swal('Total Class Held Is ZERO Please Map The Class Held In Class Held Master');
                    }
                    else {
                        user.pdays = 0;
                        swal('Present Days Is More Than Class Held');
                    }
                    $scope.SaveDis = true;
                    return;
                }
                $scope.SaveDis = false;
                //  $scope.albumNameArraycolumn.push(user.pdays);
                return true;
            }
            else {
                // var index1 = $scope.albumNameArraycolumn.indexOf(user.pdays);
                // $scope.albumNameArraycolumn.splice(index1);
                // swal("Please Enter The Valid Input 10.5,10");
                $scope.SaveDis = true;
                return false;
            }
        };

        $scope.getSelectedData = function (data) {
            if ($scope.checkboxchcked.indexOf(data) === -1) {
                $scope.checkboxchcked.push(data);
            }
            else {
                var index = $scope.checkboxchcked.indexOf(data);
                $scope.checkboxchcked.splice(index);
                var index1 = $scope.noOfDays.indexOf(data.ASCH_ClassHeld);
                $scope.noOfDays.splice(index1);
            }
        };

        $scope.pageChanged = function (newPage) {
            if (newPage > 0) {
                $scope.newPage = newPage;
            }
        };

        $scope.sortBy = function (key) {
            $scope.sortReverse = ($scope.sortKey == key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        //search
        $scope.searchValue = '';
        $scope.filterValue = function (obj) {
            return JSON.stringify(obj.amaY_RollNo).indexOf($scope.searchValue) >= 0 ||
                angular.lowercase(obj.studentname).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
                (obj.amsT_RegistrationNo).indexOf($scope.searchValue) >= 0 ||
                (obj.amsT_AdmNo).indexOf($scope.searchValue) >= 0;
        };

        // getting the class held in monthly
        $scope.getmonthclassheld = function () {
            $scope.studentgrid = false;
            $scope.studentlist = [];
            var entry;
            var fromdate = new Date($scope.att.ASA_Entry_DateTime).toDateString();
            if ($scope.att.entry == 'Dailyonce') {
                entry = 'D';
            }
            else if ($scope.att.entry == 'Dailytwice') {
                entry = 'H';
            }
            else if ($scope.att.entry == 'monthly') {
                entry = 'M';
            }
            else if ($scope.att.entry == 'period') {
                entry = 'P';
            }
            var data = {
                "ASMAY_Id": $scope.att.asmaY_Id,
                "ASMCL_Id": $scope.att.asmcL_Id,
                "ASMS_Id": $scope.att.asmC_Id,
                "monthid": $scope.att.ivrM_Month_Id,
                "monthflag": entry,
                "monthflag1": $scope.att.entry,
                "ASA_FromDate": fromdate
            };

            apiService.create("StudentAttendanceEntry/getmonthclassheld/", data).then(function (promise) {
                if (promise !== null) {
                    if (promise.message !== null) {
                        swal(promise.message);
                        $scope.studentgrid = false;
                    }
                    else {
                        $scope.studentgrid = true;
                    }
                    $scope.att.entry = 'monthly';
                    $scope.countclass = promise.countclass;

                    if ($scope.countclass !== null && $scope.countclass > 0) {

                        $scope.studentlist = promise.studentList;

                        savecount = promise.countclass1;

                        $scope.update = savecount;

                        $scope.att.ASA_FromDate = new Date(promise.startdate);
                        $scope.att.ASA_ToDate = new Date(promise.enddate);

                        $scope.configuration = promise.admissionstandarad;

                        if ($scope.configuration.length > 0) {

                            if ($scope.configuration[0].asC_Att_Default_OrderFlag == 1) {
                                $scope.sortKey = "studentname";
                            }
                            else if ($scope.configuration[0].asC_Att_Default_OrderFlag == 2) {
                                $scope.sortKey = "studentname";
                            }
                            else if ($scope.configuration[0].asC_Att_Default_OrderFlag == 3) {
                                $scope.sortKey = "amaY_RollNo";
                            }
                            else if ($scope.configuration[0].asC_Att_Default_OrderFlag == 4) {
                                $scope.sortKey = "studentname";
                            }
                            else if ($scope.configuration[0].asC_Att_Default_OrderFlag == 5) {
                                $scope.sortKey = "studentname";
                            }
                            else if ($scope.configuration[0].asC_Att_Default_OrderFlag == 6) {
                                $scope.sortKey = "amsT_RegistrationNo";
                            }
                            else if ($scope.configuration[0].asC_Att_Default_OrderFlag == 7) {
                                $scope.sortKey = "amsT_AdmNo";
                            }
                            else {
                                $scope.sortKey = "studentname";
                            }

                        } else {
                            $scope.sortKey = "studentname";
                        }

                        var count = 0;
                        if (promise.getstandarad.length > 0) {
                            if (promise.getstandarad[0].ivrmgC_AdmnoColumnDisplay == 1) {
                                $scope.showadmno = true;
                                count = count + 1;
                            } else {
                                $scope.showadmno = false;
                            }

                            if (promise.getstandarad[0].ivrmgC_RegnoColumnDisplay == 1) {
                                $scope.showregno = true;
                                count = count + 1;
                            } else {
                                $scope.showregno = false;
                            }

                            if (promise.getstandarad[0].ivrmgC_RollnoColumnDisplay == 1) {
                                $scope.showrollno = true;
                                count = count + 1;
                            } else {
                                $scope.showrollno = false;
                            }
                        }
                        if (count === 0) {
                            $scope.showadmno = true;
                            $scope.showrollno = true;
                        }
                    } else {
                        swal("Please Enter Number Of Class Held In Class Held Master For This Class..")
                    }
                }
                else {
                    swal('Please Enter Number Of Class Held In Class Held Master For This Class..');
                }
            });
        };

        //clear data
        $scope.clearData = function () {
            $state.reload();
        };

        //date wise attendance
        $scope.getdatewiseatt = function () {
            $scope.studentgrid = false;
            $scope.studentlist = [];
            var entry;
            var fromdate = new Date($scope.att.ASA_Entry_DateTime).toDateString();
            if ($scope.att.entry == 'Dailyonce') {
                entry = 'D';
            }
            else if ($scope.att.entry == 'Dailytwice') {
                entry = 'H';
            }
            else if ($scope.att.entry == 'monthly') {
                entry = 'M';
            }
            else if ($scope.att.entry == 'period') {
                entry = 'P';
            }
            var data = {
                "ASMAY_Id": $scope.att.asmaY_Id,
                "ASMCL_Id": $scope.att.asmcL_Id,
                "ASMS_Id": $scope.att.asmC_Id,
                "monthid": 0,
                "monthflag": entry,
                "monthflag1": $scope.att.entry,
                "ASA_FromDate": fromdate,
            }
            apiService.create("StudentAttendanceEntry/getdatewiseatt/", data).then(function (promise) {
                if (promise != null && promise.studentList.length > 0) {
                    $scope.studentlist = [];
                    $scope.AttendenceCheckDis = false;
                    $scope.studentgrid = true;
                    $scope.entry_flag = true;

                    $scope.configuration = promise.admissionstandarad;

                    if ($scope.configuration.length > 0) {

                        if ($scope.configuration[0].asC_Att_Default_OrderFlag == 1) {
                            $scope.sortKey = "studentname";
                        }
                        else if ($scope.configuration[0].asC_Att_Default_OrderFlag == 2) {
                            $scope.sortKey = "studentname";
                        }
                        else if ($scope.configuration[0].asC_Att_Default_OrderFlag == 3) {
                            $scope.sortKey = "amaY_RollNo";
                        }
                        else if ($scope.configuration[0].asC_Att_Default_OrderFlag == 4) {
                            $scope.sortKey = "studentname";
                        }
                        else if ($scope.configuration[0].asC_Att_Default_OrderFlag == 5) {
                            $scope.sortKey = "studentname";
                        }
                        else if ($scope.configuration[0].asC_Att_Default_OrderFlag == 6) {
                            $scope.sortKey = "amsT_RegistrationNo";
                        }
                        else if ($scope.configuration[0].asC_Att_Default_OrderFlag == 7) {
                            $scope.sortKey = "amsT_AdmNo";
                        }
                        else {
                            $scope.sortKey = "studentname";
                        }

                    } else {
                        $scope.sortKey = "studentname";
                    }




                    var count = 0;
                    if (promise.getstandarad.length > 0) {
                        if (promise.getstandarad[0].ivrmgC_AdmnoColumnDisplay == 1) {
                            $scope.showadmno = true;
                            count = count + 1;
                        } else {
                            $scope.showadmno = false;
                        }


                        if (promise.getstandarad[0].ivrmgC_RegnoColumnDisplay == 1) {
                            $scope.showregno = true;
                            count = count + 1;
                        } else {
                            $scope.showregno = false;
                        }

                        if (promise.getstandarad[0].ivrmgC_RollnoColumnDisplay == 1) {
                            $scope.showrollno = true;
                            count = count + 1;
                        } else {
                            $scope.showrollno = false;
                        }
                    }
                    if (count == 0) {
                        $scope.showadmno = true;
                        $scope.showrollno = true;
                    }



                    // $scope.studentlist = promise.studentList;
                    if (promise.asA_Att_EntryType == 'D') {
                        $scope.att.entry = 'Dailyonce';
                        if (promise.studentList.length > 0) {
                            savecount = promise.countclass1;
                            if (promise.attendanceentryflag == 'A') {
                                angular.forEach(promise.studentList, function (user) {
                                    if (user.pdays == 0) {
                                        user.selected = true;
                                        //savecount = user.asA_Id;
                                    } else {
                                        user.selected = false;
                                        //savecount = user.asA_Id;
                                    }
                                    $scope.studentlist.push(user);
                                });
                            }
                            else if (promise.attendanceentryflag == 'P') {
                                angular.forEach(promise.studentList, function (user) {
                                    if (user.pdays > 0) {
                                        user.selected = true;
                                        //savecount = user.asA_Id;
                                    } else {
                                        user.selected = false;
                                        //savecount = user.asA_Id;
                                    }
                                    $scope.studentlist.push(user);
                                });
                            }

                        }
                    }
                    else if (promise.asA_Att_EntryType == 'H') {
                        $scope.att.entry = 'Dailytwice';

                        if (promise.studentList.length > 0) {
                            savecount = promise.countclass1;
                            angular.forEach(promise.studentList, function (user) {
                                if (promise.attendanceentryflag == 'A') {
                                    if (user.pdays == 0.5) {
                                        if (user.asA_Dailytwice_Flag == "firsthalf") {
                                            user.FirstHalfflag = false;
                                            user.SecondHalfflag = true;
                                            //savecount = user.asA_Id;
                                        }
                                        else if (user.asA_Dailytwice_Flag == "Secondhalf") {
                                            user.SecondHalfflag = false;
                                            user.FirstHalfflag = true;
                                            //savecount = user.asA_Id;
                                        }

                                    } else if (user.pdays == 1) {
                                        user.FirstHalfflag = false;
                                        user.SecondHalfflag = false;
                                        //savecount = user.asA_Id;
                                    }
                                    else if (user.pdays == 0) {
                                        user.FirstHalfflag = true;
                                        user.SecondHalfflag = true;
                                        //savecount = user.asA_Id;
                                    }
                                    else {
                                        user.FirstHalfflag = false;
                                        user.SecondHalfflag = false;
                                        //savecount = user.asA_Id;
                                    }
                                    $scope.studentlist.push(user);
                                }

                                else if (promise.attendanceentryflag == 'P') {
                                    if (user.pdays == 0.5) {
                                        if (user.asA_Dailytwice_Flag == "firsthalf") {
                                            user.FirstHalfflag = true;
                                            user.SecondHalfflag = false;
                                            //savecount = user.asA_Id;
                                        }
                                        else if (user.asA_Dailytwice_Flag == "Secondhalf") {
                                            user.SecondHalfflag = true;
                                            user.FirstHalfflag = false;
                                            // savecount = user.asA_Id;
                                        }

                                    } else if (user.pdays == 1) {
                                        user.FirstHalfflag = true;
                                        user.SecondHalfflag = true;
                                        //savecount = user.asA_Id;
                                    }
                                    else {
                                        user.FirstHalfflag = false;
                                        user.SecondHalfflag = false;
                                        //savecount = user.asA_Id;
                                    }
                                    $scope.studentlist.push(user);
                                }
                            });
                        }

                    }
                    if (promise.asA_Att_EntryType == 'M') {
                        $scope.att.entry = 'monthly';
                        $scope.studentlist = promise.studentList;
                    }
                    if (promise.asA_Att_EntryType == 'P') {
                        $scope.att.entry = 'period';
                    }
                }
                else {
                    swal("No Records Found...");
                }

                $scope.update = savecount;
            });

        };

        $scope.batchno1 = 0;

        //subjectwise batch list
        $scope.getbatchlist = function () {
            $scope.studentgrid = false;
            $scope.studentlist = [];
            var data = {
                "ASMAY_Id": $scope.att.asmaY_Id,
                "ASMCL_Id": $scope.att.asmcL_Id,
                "ASMS_Id": $scope.att.asmC_Id,
                "ismS_Id": $scope.att.ismS_Id,
            }
            apiService.create("StudentAttendanceEntry/getbatchlist/", data).then(function (promise) {
                if (promise != null) {
                    if (promise.batchList != null && promise.batchList.length > 0) {
                        $scope.batchList = promise.batchList;
                        $scope.batchno = false;
                        $scope.attcount1 = true;
                        $scope.batchno1 = 1;

                    }
                    else {
                        //  $scope.studentlist = promise.studentList;
                        $scope.batchno = true;
                        $scope.attcount = false;
                        $scope.attcount1 = false;
                        $scope.batchno1 = 0;
                    }

                }
                else {
                    swal("No Branch Is Mapped For This Combination")
                    $scope.batchno = true;
                }
            })
        }

        //--Disable Period --//
        $scope.getperiod = function () {
            if ($scope.att.asasB_Id == undefined) {
                $scope.attcount1 = true;
            }
            else {
                $scope.attcount1 = false;
            }
        };

        //get the student list with period
        $scope.getstdlistperiod = function () {

            if ($scope.att.ttmP_Id != undefined) {
                $scope.studentlist = [];
                $scope.studentgrid = false;
                var entry;
                var asasB_Id;
                var fromdate = new Date($scope.att.ASA_Entry_DateTime).toDateString();
                if ($scope.att.entry == 'Dailyonce') {
                    entry = 'D';
                }
                else if ($scope.att.entry == 'Dailytwice') {
                    entry = 'H';
                }
                else if ($scope.att.entry == 'monthly') {
                    entry = 'M';
                }
                else if ($scope.att.entry == 'period') {
                    entry = 'P';
                }
                if ($scope.att.asasB_Id == "") {
                    asasB_Id = 0
                }
                else {
                    asasB_Id = $scope.att.asasB_Id;
                }


                var data = {
                    "ASMAY_Id": $scope.att.asmaY_Id,
                    "ASMCL_Id": $scope.att.asmcL_Id,
                    "ASMS_Id": $scope.att.asmC_Id,
                    "ismS_Id": $scope.att.ismS_Id,
                    "TTMP_Id": $scope.att.ttmP_Id,
                    "monthflag": entry,
                    "monthflag1": $scope.att.entry,
                    "ASA_FromDate": fromdate,
                    "asasB_Id": asasB_Id
                }
                apiService.create("StudentAttendanceEntry/getstdlistperiod/", data).then(function (promise) {
                    if (promise.message != null) {
                        swal(promise.message);
                    }
                    else {
                        $scope.studentlist = [];
                        if (promise.studentList !== null && promise.studentList.length > 0) {

                            $scope.configuration = promise.admissionstandarad;

                            if ($scope.configuration.length > 0) {

                                if ($scope.configuration[0].asC_Att_Default_OrderFlag == 1) {
                                    $scope.sortKey = "studentname";
                                }
                                else if ($scope.configuration[0].asC_Att_Default_OrderFlag == 2) {
                                    $scope.sortKey = "studentname";
                                }
                                else if ($scope.configuration[0].asC_Att_Default_OrderFlag == 3) {
                                    $scope.sortKey = "amaY_RollNo";
                                }
                                else if ($scope.configuration[0].asC_Att_Default_OrderFlag == 4) {
                                    $scope.sortKey = "studentname";
                                }
                                else if ($scope.configuration[0].asC_Att_Default_OrderFlag == 5) {
                                    $scope.sortKey = "studentname";
                                }
                                else if ($scope.configuration[0].asC_Att_Default_OrderFlag == 6) {
                                    $scope.sortKey = "amsT_RegistrationNo";
                                }
                                else if ($scope.configuration[0].asC_Att_Default_OrderFlag == 7) {
                                    $scope.sortKey = "amsT_AdmNo";
                                }
                                else {
                                    $scope.sortKey = "studentname";
                                }

                            } else {
                                $scope.sortKey = "studentname";
                            }


                            var count = 0;
                            if (promise.getstandarad.length > 0) {
                                if (promise.getstandarad[0].ivrmgC_AdmnoColumnDisplay == 1) {
                                    $scope.showadmno = true;
                                    count = count + 1;
                                } else {
                                    $scope.showadmno = false;
                                }


                                if (promise.getstandarad[0].ivrmgC_RegnoColumnDisplay == 1) {
                                    $scope.showregno = true;
                                    count = count + 1;
                                } else {
                                    $scope.showregno = false;
                                }

                                if (promise.getstandarad[0].ivrmgC_RollnoColumnDisplay == 1) {
                                    $scope.showrollno = true;
                                    count = count + 1;
                                } else {
                                    $scope.showrollno = false;
                                }
                            }
                            if (count == 0) {
                                $scope.showadmno = true;
                                $scope.showrollno = true;
                            }


                            if (promise.attcount == 1) {
                                $scope.attcount = true;
                                $scope.attcount1 = true;
                                $scope.attcount2 = true;
                                $scope.batchno = true;
                            }
                            else {
                                $scope.attcount = false;
                                $scope.attcount1 = false;
                                $scope.attcount2 = false;
                            }

                            $scope.asA_Id_Temp_array = [];
                            $scope.asA_Id_Temp_array = promise.studentList.filter((item, i, arr) => arr.findIndex((t) => t.asA_Id === item.asA_Id && t.asA_Id > 0) === i);
                            $scope.asa_id_temp = 0;

                            if ($scope.asA_Id_Temp_array !== null && $scope.asA_Id_Temp_array.length > 0 && $scope.asA_Id_Temp_array.length === 1) {
                                $scope.asa_id_temp = $scope.asA_Id_Temp_array[0].asA_Id;
                            }

                            $scope.att.ASA_Regular_Extra = promise.asA_Regular_Extra;
                            savecount = promise.countclass1;
                            if (promise.attendanceentryflag == 'A') {
                                angular.forEach(promise.studentList, function (user) {
                                    $scope.update += user.asA_Id > 0 ? 1 : 0;
                                    user.asA_Id = user.asA_Id > 0 ? user.asA_Id : $scope.asa_id_temp;
                                    if (user.pdays == 0) {
                                        user.selected = true;
                                    } else {
                                        user.selected = false;
                                    }
                                    $scope.studentlist.push(user);
                                });
                            }
                            else if (promise.attendanceentryflag == 'P') {
                                angular.forEach(promise.studentList, function (user) {
                                    $scope.update += user.asA_Id > 0 ? 1 : 0;
                                    user.asA_Id = user.asA_Id > 0 ? user.asA_Id : $scope.asa_id_temp;
                                    if (user.pdays > 0) {
                                        user.selected = true;
                                    } else {
                                        user.selected = false;
                                    }
                                    $scope.studentlist.push(user);
                                })
                            }
                            $scope.studentgrid = true;
                        }
                    }
                });
            }
            else {
                //  $scope.submitted = true;
                $scope.studentgrid = false;
            }
            //$scope.update = savecount;
        };

        $scope.getperioddatewiseatt = function () {
            $scope.studentlist = [];
            $scope.att.asasB_Id = "";
            $scope.att.ttmP_Id = "";
            $scope.studentgrid = false;
        };

        $scope.ViewAttendanceDetailsStaffWise = function (att, att_entry_type) {
            $scope.viewStudentPeriodWiseAttDetails = [];
            $scope.searchValue_att_del = "";
            $scope.itemsPerPage1 = 10;
            $scope.currentPage1 = 1;
            $scope.att_entry_type_Temp = att_entry_type;
            var data = {
                "ASMAY_Id": att.asmaY_Id,
                "ASMCL_Id": att.asmcL_Id,
                "ASMS_Id": att.asmC_Id,
                "att_entry_type": att_entry_type,
            };

            apiService.create("StudentAttendanceEntry/ViewAttendanceDetailsStaffWise", data).then(function (promise) {
                if (promise !== null) {
                    if (promise.viewStudentPeriodWiseAttDetails !== null && promise.viewStudentPeriodWiseAttDetails.length > 0) {
                        $scope.viewStudentPeriodWiseAttDetails = promise.viewStudentPeriodWiseAttDetails;
                        $('#MymodalViewAttendance').modal('show');
                    }
                }
            });
        };

        $scope.Deleteattendance = function (objas, SweetAlert) {

            var mgs = "";
            var confirmmgs = "";
            var todate = "";
            var fromdate = "";
            var ASA_Class_Attended;
            var asasB_Id;

            swal({
                title: "Are you sure",
                text: "Do You Want To Delete The Selected Attendace Details ?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, Delete it!",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {

                        var ASA_ClassHeld;
                        if ($scope.att.entry == 'Dailyonce') {

                            angular.forEach($scope.studentlist, function (user) {
                                if (user.pdays != 0) {
                                    user.pdays = 0;
                                }
                                //$scope.studentlist.push(user);
                            });

                            fromdate = new Date($scope.att.ASA_Entry_DateTime).toDateString();
                            todate = new Date($scope.att.ASA_Entry_DateTime).toDateString();
                            ASA_ClassHeld = '1.00';
                            ASA_Class_Attended = '1.0';
                            asasB_Id = 0;
                        }

                        if ($scope.att.entry == 'Dailytwice') {

                            angular.forEach($scope.studentlist, function (user) {
                                if (user.pdays != 0) {
                                    user.pdays = 0;
                                }
                                //$scope.studentlist.push(user);
                            });


                            if ($scope.att.ASA_ClassHeld1 == true || $scope.att.ASA_ClassHeld2 == true) {
                                ASA_ClassHeld = '0.5';
                            }
                            if ($scope.att.ASA_ClassHeld1 == true && $scope.att.ASA_ClassHeld2 == true) {
                                ASA_ClassHeld = '1.00';
                            }
                            if ($scope.att.ASA_ClassHeld1 == false && $scope.att.ASA_ClassHeld2 == false) {
                                swal("Kindy Select 1st Half/2nd Half Or both");
                                return;
                            }

                            fromdate = new Date($scope.att.ASA_Entry_DateTime).toDateString();
                            todate = new Date($scope.att.ASA_Entry_DateTime).toDateString();
                            ASA_ClassHeld = '1.00';
                            asasB_Id = 0;
                        }

                        if ($scope.att.entry == 'monthly') {
                            //var countclass = parseInt($scope.countclass);
                            var countclass = ($scope.countclass);
                            ASA_ClassHeld = countclass;
                            ASA_Class_Attended = '0.0';
                            fromdate = new Date($scope.att.ASA_FromDate).toDateString();
                            todate = new Date($scope.att.ASA_ToDate).toDateString();
                            asasB_Id = 0;
                        }
                        if ($scope.att.entry == 'period') {

                            angular.forEach($scope.studentlist, function (user) {
                                if (user.pdays != 0) {
                                    user.pdays = 0;
                                }
                                //$scope.studentlist.push(user);
                            })
                            fromdate = new Date($scope.att.ASA_Entry_DateTime).toDateString();
                            todate = new Date($scope.att.ASA_Entry_DateTime).toDateString();
                            ASA_ClassHeld = '1.00';
                            if ($scope.att.asasB_Id == "") {
                                asasB_Id = 0;
                            } else {
                                asasB_Id = $scope.att.asasB_Id;
                            }
                        }

                        //var entrydate = new Date($scope.att.ASA_Entry_DateTime).toDateString();
                        var entrydate = new Date().toDateString();

                        var data = {
                            "ASA_Id": $scope.att.asA_Id,
                            "ismS_Id": $scope.att.ismS_Id,
                            "ASMAY_Id": $scope.att.asmaY_Id,
                            "ASA_Att_Type": $scope.att.entry,
                            "ASMCL_Id": $scope.att.asmcL_Id,
                            "ASMS_Id": $scope.att.asmC_Id,
                            "TTMP_Id": $scope.att.ttmP_Id,
                            "ASA_Entry_DateTime": entrydate,
                            "ASA_FromDate": fromdate,
                            "ASA_ToDate": todate,
                            "ASA_ClassHeld": ASA_ClassHeld,
                            "ASA_Regular_Extra": $scope.att.ASA_Regular_Extra,
                            "asasB_Id": asasB_Id,
                            stdList: $scope.studentlist,
                        };

                        apiService.create("StudentAttendanceEntry/Deleteattendance", data).
                            then(function (promise) {

                                if (promise.returnval == true) {
                                    swal("Record Delected Successfully");
                                } else {
                                    swal("Failed To Delect Record");
                                }
                            });
                    }
                    else {
                        swal("Record Delection Cancelled");
                    }
                    $state.reload();
                });
        };

        $scope.AttendanceDeleteRecordWise = function (user_delete) {
            var data = {
                "ASMAY_Id": $scope.att.asmaY_Id,
                "ASMCL_Id": $scope.att.asmcL_Id,
                "ASMS_Id": $scope.att.asmC_Id,
                "ASA_Id": user_delete.ASA_Id,
                "att_entry_type": $scope.att_entry_type_Temp
            };

            swal({
                title: "Are you sure",
                text: "Do You Want To Delete The Selected Attendace Details ?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, Delete it!",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("StudentAttendanceEntry/AttendanceDeleteRecordWise", data).then(function (promise) {
                            if (promise.returnval == true) {
                                swal("Record Delected Successfully");
                            } else {
                                swal("Failed To Delect Record");
                            }

                            if (promise.viewStudentPeriodWiseAttDetails !== null && promise.viewStudentPeriodWiseAttDetails.length > 0) {
                                $scope.viewStudentPeriodWiseAttDetails = promise.viewStudentPeriodWiseAttDetails;
                            } else {
                                $('#MymodalViewAttendance').modal('hide');
                            }
                        });
                    }
                    else {
                        swal("Record Delection Cancelled");
                    }
                });
        };
    }
})();
