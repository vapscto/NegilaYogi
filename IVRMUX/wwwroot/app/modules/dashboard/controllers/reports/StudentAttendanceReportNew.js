
(function () {
    'use strict';
    angular
        .module('app')
        .controller('StudentAttendanceReportControllerNew', StudentAttendanceReportControllerNew)

    StudentAttendanceReportControllerNew.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', 'Excel', '$timeout', 'superCache']
    function StudentAttendanceReportControllerNew($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, Excel, $timeout, superCache) {
        $scope.currentPage = 1;
        //   $scope.itemsPerPage = 10;
        $scope.printdatatable = [];
        $scope.ddate = {};
        $scope.ddate = new Date();

        $scope.usrname = localStorage.getItem('username');

        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));

        var paginationformasters;
        var copty;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_ReportPagination;
            copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
        }
        $scope.itemsPerPage = paginationformasters;
        $scope.coptyright = copty;

        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }

        $scope.imgname = logopath;
        //$scope.itemsPerPage = 10;




        $scope.obj = {};
        $scope.StuAttRptDropdownList = function () {
            $scope.studentname = false;
            $scope.ftdate = false;
            $scope.grid_flag = false;
            $scope.excel_flag = true;
            $scope.print_flag = true;
            $scope.order_flag = false;
            $scope.report_btn = false;


            apiService.get("StudentAttendanceReport/getdetails/").then(function (promise) {
                {

                    $scope.all = true;
                    $scope.yearDropdown = promise.academicList;
                    $scope.allAcademicYear1 = promise.academicListdefault;

                    for (var i = 0; i < $scope.yearDropdown.length; i++) {
                        name = $scope.yearDropdown[i].asmaY_Id;
                        for (var j = 0; j < $scope.allAcademicYear1.length; j++) {
                            if (name == $scope.allAcademicYear1[j].asmaY_Id) {
                                $scope.yearDropdown[i].Selected = true;
                                $scope.asmaY_Id = $scope.allAcademicYear1[j].asmaY_Id;
                                //$scope.yearId = $scope.allAcademicYear1[j].asmaY_Id;
                            }
                        }
                    }
                    $scope.classDropdown = promise.classlist;
                    $scope.sectionDropdown = promise.sectionList;
                    $scope.monthDropdown = promise.monthList;
                    // $scope.fromdate = new Date();
                    // $scope.todate = new Date();
                    $scope.fromdaily = new Date();

                    $scope.fromdate = new Date();
                    $scope.maxDatef = new Date(
                        $scope.fromdate.getFullYear(),
                        $scope.fromdate.getMonth(),
                        $scope.fromdate.getDate());

                    $scope.todate = new Date();
                    $scope.maxDatet = new Date(
                        $scope.todate.getFullYear(),
                        $scope.todate.getMonth(),
                        $scope.todate.getDate());

                    $scope.obj.fromdaily = new Date();
                    console.log($scope.obj.fromdaily);
                    $scope.maxDatedf = new Date(
                        $scope.fromdaily.getFullYear(),
                        $scope.fromdaily.getMonth(),
                        $scope.fromdaily.getDate());
                }
            })
        }





        $scope.toggleAll = function () {
            $scope.printdatatable = [];
            var toggleStatus = $scope.all2;
            angular.forEach($scope.filterValue1, function (itm) {
                itm.selected = toggleStatus;
                if ($scope.all2 == true) {
                    $scope.printdatatable.push(itm);
                }
                else {
                    $scope.printdatatable.splice(itm);
                    $scope.printdatatable = [];
                }
            });
        }
        $scope.printdatatabledate = [];
        $scope.toggleAlldate = function () {
            $scope.printdatatabledate = [];
            var toggleStatus1 = $scope.all2date;
            angular.forEach($scope.filterValue2, function (itm) {
                itm.selected = toggleStatus1;
                if ($scope.all2date == true) {
                    $scope.printdatatabledate.push(itm);
                }
                else {
                    $scope.printdatatabledate.splice(itm);
                    $scope.printdatatabledate = [];
                }
            });
        }

        $scope.optionToggleddate = function (SelectedStudentRecord, index) {
            $scope.all2date = $scope.students1.every(function (itm) { return itm.selected; });
            if ($scope.printdatatabledate.indexOf(SelectedStudentRecord) === -1) {
                $scope.printdatatabledate.push(SelectedStudentRecord);
            }
            else {
                $scope.printdatatabledate.splice($scope.printdatatabledate.indexOf(SelectedStudentRecord), 1);
                $scope.printdatatabledate = [];
            }
        }



        $scope.propertyName = 'name';
        $scope.sortBy = function (propertyName) {
            $scope.reverse = ($scope.propertyName === propertyName) ? !$scope.reverse : false;
            $scope.propertyName = propertyName;
        };


        $scope.optionToggled = function (SelectedStudentRecord, index) {
            $scope.all2 = $scope.students.every(function (itm) { return itm.selected; });
            if ($scope.printdatatable.indexOf(SelectedStudentRecord) === -1) {
                $scope.printdatatable.push(SelectedStudentRecord);
            }
            else {
                $scope.printdatatable.splice($scope.printdatatable.indexOf(SelectedStudentRecord), 1);
                $scope.printdatatable = [];
            }
        }

        $scope.exportToExcel = function () {

            angular.forEach($scope.yearDropdown, function (yy) {
                if (yy.asmaY_Id == $scope.asmaY_Id) {
                    $scope.year = yy.asmaY_Year
                }
            });


            if ($scope.type == 1) {

                var excelname = "Class Wise Attendance Report _" + $scope.year + ".xls";

            } else {

                angular.forEach($scope.classDropdown, function (cls) {
                    if (cls.asmcL_Id == $scope.asmcL_Id) {
                        $scope.class = cls.asmcL_ClassName
                    }
                })

                angular.forEach($scope.sectionDropdown, function (sec) {
                    if (sec.asmS_Id == $scope.asmC_Id) {
                        $scope.sec = sec.asmC_SectionName
                    }
                })
                var excelname = "Class Wise Attendance Report _" + $scope.year + "  " + $scope.class + " : " + $scope.sec + ".xls";
            }


            var exportHref = Excel.tableToExcel('#table21', 'Class Wise Attendance Report');

            //var exportHref = Excel.tableToExcel('#table21', 'sheet name');
            $timeout(function () {
                var a = document.createElement('a');
                a.href = exportHref;
                a.download = excelname;
                document.body.appendChild(a);
                a.click();
                a.remove();
            }, 100);
        }

        $scope.printData = function () {
            var innerContents = document.getElementById("details").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
                '<link type="text/css" media="print" href="css/print/preadmission/AdmissionReports.css" rel="stylesheet" />' +
                '</head> <body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>'
            );
            popupWinindow.document.close();
        }

        $scope.getDataByType = function (type) {
            if (type == 1)      //all
            {
                $scope.studentname = false;
                $scope.order_flag = false
                $scope.grid_flag = false;

                $scope.grid_flag = false;
            }
            else if (type == 2)         //individual
            {
                $scope.AMST_Id = "";
                $scope.studentname = true;
                $scope.order_flag = true;

                $scope.submitted = false;

                $scope.grid_flag = false;
                $scope.stu_name = true;

            }
        }

        $scope.type1 = "1";
        $scope.studlist = function (iddata) {

            for (var k = 0; k < $scope.yearDropdown.length; k++) {
                if ($scope.yearDropdown[k].asmaY_Id == iddata) {
                    var data = $scope.yearDropdown[k].asmaY_Year;
                }
            }
            if (data != null) {

                console.log(data);
                var name, name1;
                for (var i = 0; i < data.length; i++) {
                    if (i < 4) {
                        if (i == 0) {
                            name = data[i];
                        } else {
                            name += data[i];
                        }
                    }
                    if (i > 4) {
                        if (i == 5) {
                            name1 = data[5];
                        } else {
                            name1 += data[i];
                        }
                    }
                }
                $scope.fromDate = name;
                $scope.toDate = name1;
                $scope.frommon = "";
                $scope.tomon = "";
                $scope.fromDay = "";
                $scope.toDay = "";
                // For Academic From Date
                $scope.minDatef = new Date(
                    $scope.fromDate,
                    $scope.frommon,
                    $scope.fromDay + 1);

                $scope.maxDatef = new Date(
                    $scope.toDate,
                    $scope.tomon,
                    $scope.toDay + 365);

            }




            if ($scope.type1 == "2") {

                var data = {
                    "ASMAY_Id": $scope.asmaY_Id,
                    "ASMCL_Id": $scope.asmcL_Id,
                    "ASMC_Id": $scope.asmC_Id,
                    "radiotype": $scope.monthly,
                    "type1": "name",
                }
            }
            else if ($scope.type1 == "1") {
                data = {
                    "ASMAY_Id": $scope.asmaY_Id,
                    "ASMCL_Id": $scope.asmcL_Id,
                    "ASMC_Id": $scope.asmC_Id,
                    "radiotype": $scope.monthly,
                    "type1": "admno",
                }
            }


            apiService.create("StudentAttendanceReport/getdatatype", data)
                .then(function (promise) {

                    if (promise.studentAttendanceList != null && promise.studentAttendanceList.length > 0) {
                        $scope.studentDropdown = promise.studentAttendanceList;
                        $scope.stu_name = false;
                        $scope.report_btn = false;

                    }

                    else {
                        swal("Student is Not Available For This Selection,Please Select Different");
                        $scope.stu_name = true;
                        $scope.report_btn = true;

                    }

                })
        }


        $scope.interacted = function (field) {

            return $scope.submitted;
        };

        //Showing Report
        $scope.submitted = false;

        $scope.savetmpldatanew = function () {
            if ($scope.myForm.$valid) {
                var data = {
                    "ASMAY_Id": $scope.asmaY_Id,
                    "ASMCL_Id": $scope.asmcL_Id,
                    "ASMC_Id": $scope.asmC_Id,
                    "AMM_ID": $scope.amM_Id,
                    "type": $scope.type
                }
                apiService.create("StudentAttendanceReport/savetmpldatanew", data).then(function (promise) {
                    if (promise != null) {
                        if (promise.newarray.length > 0) {

                            $scope.excel_flag = false;
                            $scope.print_flag = false;

                            angular.forEach($scope.monthDropdown, function (mm) {
                                if (mm.amM_ID == $scope.amM_Id) {
                                    $scope.monthname = mm.amM_NAME;
                                }
                            })

                            $scope.attdatewisecount = promise.newarray;
                            $scope.days = promise.newarray_date;
                            $scope.classsecdetails = promise.classsecdetails;
                            $scope.classdetails = promise.classdetails;

                            angular.forEach($scope.classdetails, function (c) {
                                var cls_secs = [];
                                var status_s = [];
                                angular.forEach($scope.classsecdetails, function (c_s) {
                                    if (c.asmcL_Id == c_s.asmcL_Id) {
                                        cls_secs.push(c_s);
                                        var present_s = [];
                                        angular.forEach($scope.days, function (d) {
                                            var value = null;
                                            angular.forEach($scope.attdatewisecount, function (att) {
                                                if (att.ASMCL_Id == c.asmcL_Id && att.ASMS_Id == c_s.asmS_Id && att.flag == 'P')
                                                    value = att[d.day];
                                            })
                                            if (value == -1)
                                                value = 'S';
                                            else if (value == -2)
                                                value = 'HO';
                                            else if (value == -3)
                                                value = 'NE';
                                            present_s.push(value);
                                        })

                                        var absent_s = [];
                                        angular.forEach($scope.days, function (d) {
                                            var value = null;
                                            angular.forEach($scope.attdatewisecount, function (att) {
                                                if (att.ASMCL_Id == c.asmcL_Id && att.ASMS_Id == c_s.asmS_Id && att.flag == 'A')
                                                    value = att[d.day];
                                            })
                                            if (value == -1)
                                                value = 'S';
                                            else if (value == -2)
                                                value = 'HO';
                                            else if (value == -3)
                                                value = 'NE';
                                            absent_s.push(value);
                                        })

                                        status_s.push({ name: 'Present', flag: 'P', asmC_SectionName: c_s.asmC_SectionName, att: present_s });
                                        status_s.push({ name: 'Absent', flag: 'A', att: absent_s });

                                    }
                                })
                                c.secs = cls_secs;
                                var pre_tot = [];
                                angular.forEach($scope.days, function (d) {
                                    var value = null;
                                    angular.forEach($scope.attdatewisecount, function (att) {
                                        if (att.ASMCL_Id == c.asmcL_Id && att.flag == 'P' && att[d.day] != -1 && att[d.day] != -2 && att[d.day] != -3)
                                            value += att[d.day];
                                    })
                                    pre_tot.push(value);
                                })
                                var abs_tot = [];
                                angular.forEach($scope.days, function (d) {
                                    var value = null;
                                    angular.forEach($scope.attdatewisecount, function (att) {
                                        if (att.ASMCL_Id == c.asmcL_Id && att.flag == 'A' && att[d.day] != -1 && att[d.day] != -2 && att[d.day] != -3)
                                            value += att[d.day];
                                    })
                                    abs_tot.push(value);
                                })
                                status_s.push({ name: 'Total No Of Present', flag: 'TP', att: pre_tot });
                                status_s.push({ name: 'Total No Of Absent', flag: 'TA', att: abs_tot });
                                c.sts = status_s;
                            })
                            console.log($scope.attdatewisecount);
                            console.log($scope.days);
                            console.log($scope.classsecdetails);
                            console.log($scope.classdetails);
                        } else {
                            swal("No Records Found")
                        }

                    } else {
                        swal("No Records Found")
                    }
                })

            }
            else {
                $scope.submitted = true;
            }
        }

        $scope.Clearid = function () {
            $state.reload();
            $scope.asmaY_Id = "";
            $scope.asmcL_Id = "";
            $scope.asmC_Id = "";
            $scope.fromdate = "";
            $scope.todate = "";
            // $scope.amsT_Id = "";
            $scope.amM_Id = "";
        }

        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }

        $scope.setTodate = function (data1) {
            $scope.todatef = false;
            $scope.todate1 = data1.fromdate;
            $scope.minDatet = new Date(
                $scope.todate1.getFullYear(),
                $scope.todate1.getMonth(),
                $scope.todate1.getDate() + 1);

            $scope.maxDatef = new Date(
                $scope.todate1.getFullYear(),
                $scope.todate1.getMonth(),
                $scope.todate1.getDate() + 1);
        }


        $scope.setTodate1 = function (datato) {

            $scope.todate2 = datato.todate;
            $scope.minDatet = new Date(
                $scope.todate2.getFullYear(),
                $scope.todate2.getMonth(),
                $scope.todate2.getDate() + 1);
        }

    }
})();