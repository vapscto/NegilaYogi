
(function () {
    'use strict';
    angular.module('app').controller('SubjectWiseAttendanceSMSReportController', SubjectWiseAttendanceSMSReportController)

    SubjectWiseAttendanceSMSReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', 'Excel', '$timeout', 'superCache']
    function SubjectWiseAttendanceSMSReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, Excel, $timeout, superCache) {
        $scope.currentPage = 1;
        $scope.printdatatable = [];
        $scope.ddate = {};
        $scope.ddate = new Date();
        $scope.obj = {};
        $scope.usrname = localStorage.getItem('username');

        $scope.sortKey = 'AMAY_RollNo';
        $scope.obj.type = 'Date';
        $scope.sortReverse = false;
        $scope.excel_flag = true;

        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));

        var paginationformasters;
        var copty;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings !== null && ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_ReportPagination;
            copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
        } else {
            paginationformasters = 10;
            copty = "";
        }
        $scope.itemsPerPage = paginationformasters;
        $scope.currentPage = 1;
        $scope.coptyright = copty;

        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings !== null && admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }

        $scope.imgname = logopath;

        $scope.LoadData = function () {
            var pageid = 2;
            apiService.getURI("StudentAttendanceReport/LoadData", pageid).then(function (promise) {
                $scope.yearDropdown = promise.academicList;
                $scope.classDropdown = promise.classlist;
                $scope.ASMAY_Id = promise.asmaY_Id;
                $scope.monthList = promise.monthList;

                $scope.yearlist = [];

                angular.forEach($scope.yearDropdown, function (dd) {
                    if (dd.asmaY_Id === promise.asmaY_Id) {
                        $scope.minDatef = new Date(dd.asmaY_From_Date);
                        $scope.maxDatef = new Date(dd.asmaY_To_Date);
                        $scope.maxDatet = new Date(dd.asmaY_To_Date);

                        var yearlist = dd.asmaY_Year.split('-');

                        $scope.yearlist.push({ id: yearlist[0], value: yearlist[0] })
                        $scope.yearlist.push({ id: yearlist[1], value: yearlist[1] })
                    }
                });
            });
        };

        $scope.OnChangeFlag = function () {
            $scope.obj.fromdate = null;
            $scope.obj.todate = null;
            $scope.excel_flag = true;
            $scope.newarray = [];
        };

        $scope.OnChangeYear = function () {
            $scope.classDropdown = [];
            $scope.sectionDropdown = [];
            $scope.subjectDropdown = [];

            $scope.obj.fromdate = null;
            $scope.obj.todate = null;
            $scope.excel_flag = true;

            $scope.ASMCL_Id = "";
            $scope.ASMS_Id = "";
            $scope.obj.ISMS_Id = "";
            $scope.obj.YearId = "";
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id
            };

            apiService.create("StudentAttendanceReport/OnChangeAcademicYear", data).then(function (promise) {
                $scope.classDropdown = promise.classlist;

                $scope.yearlist = [];

                angular.forEach($scope.yearDropdown, function (dd) {
                    if (dd.asmaY_Id === parseInt($scope.ASMAY_Id)) {
                        $scope.minDatef = new Date(dd.asmaY_From_Date);
                        $scope.maxDatef = new Date(dd.asmaY_To_Date);
                        $scope.maxDatet = new Date(dd.asmaY_To_Date);
                        var yearlist = dd.asmaY_Year.split('-');
                        $scope.yearlist.push({ id: yearlist[0], value: yearlist[0] })
                        $scope.yearlist.push({ id: yearlist[1], value: yearlist[1] })
                    }
                });
            });
        };

        $scope.OnChangeClass = function (usercheckCC, ASMCL_Id) {
            $scope.sectionDropdown = [];
            $scope.subjectDropdown = [];
            $scope.excel_flag = true;
            $scope.obj.fromdate = null;
            $scope.obj.todate = null;
            $scope.ASMS_Id = "";
            $scope.obj.ISMS_Id = "";


            $scope.classlistarray = [];

            angular.forEach($scope.classDropdown, function (aa) {
                if (aa.selected == true) {
                    $scope.classlistarray.push({ ASMCL_Id: aa.asmcL_Id })
                }

            });
            if ($scope.classlistarray.length > 0)
            {
                $scope.OnChangeSection();
            }

            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                // "ASMCL_Id": $scope.ASMCL_Id,
                "classlsttwo": $scope.classlistarray
            };

            apiService.create("StudentAttendanceReport/OnChangeClassAbsent", data).then(function (promise) {
                $scope.sectionDropdown = promise.sectionList;
            });
        };
        $scope.isOptionsRequiredclass = function () {
            return !$scope.classDropdown.some(function (item) {
                return item.selected;
            });
        };

        $scope.isOptionsRequiredC = function () {
            return !$scope.sectionDropdown.some(function (item) {
                return item.selected;
            });
        };

        $scope.al_checkclass = function (all, ASMCL_Id) {

            $scope.classlistarray = [];
            $scope.obj.usercheckCC = all;
            // if ($scope.obj.usercheckCC == true) {



            var toggleStatus = $scope.obj.usercheckCC;
            angular.forEach($scope.classDropdown, function (role) {
                role.selected = toggleStatus;
            });


            $scope.classlistarray = [];
            angular.forEach($scope.classDropdown, function (qq) {
                if (qq.selected == true) {
                    $scope.classlistarray.push({ ASMCL_Id: qq.asmcL_Id, ASMCL_Id: qq.asmcL_Id })
                }
            });

            $scope.OnChangeClass();

            // }

            if ($scope.obj.usercheckCC == false) {
                //$scope.classDropdown = [];
                //$scope.classlistarray = [];
                $scope.sectionlistarray = [];
                $scope.sectionDropdown = [];
                $scope.subjectlistarray = [];
                $scope.subjectDropdown = [];
                $scope.classlistarraynew = [];
                $scope.sectionlistarraynew = [];
                $scope.subjectlistarraynew = [];
                $scope.obj.usercheckCC = false;
                $scope.obj.usercheckC = false;
              //  $scope.obj.usercheckS = false;

            }


        }


        $scope.all_checkC = function (all) {
            $scope.sectionlistarray = [];
            $scope.obj.usercheckC = all;

            var toggleStatus = $scope.obj.usercheckC;
            angular.forEach($scope.sectionDropdown, function (role) {
                role.selected = toggleStatus;
            });

            $scope.sectionlistarray = [];
            angular.forEach($scope.sectionDropdown, function (qq) {
                if (qq.selected == true) {
                    $scope.sectionlistarray.push({ ASMS_Id: qq.asmS_Id })

                    //$scope.sectionlistarray.push({ ASMS_Id: qq.asmS_Id, ASMCL_Id: qq.asmcL_Id })
                }
            });
            $scope.obj.usercheckC = all;

            $scope.OnChangeSection();
            if ($scope.obj.usercheckC == false) {
                $scope.sectionDropdown = [];
                $scope.sectionlistarray = [];
                $scope.subjectlistarray = [];
                $scope.obj.usercheckC = false;
            }
        };







        $scope.OnChangeSection = function () {
            $scope.excel_flag = true;
            $scope.obj.fromdate = null;
            $scope.obj.todate = null;
            $scope.subjectDropdown = [];
            $scope.obj.ISMS_Id = "";


            $scope.classlistarray = [];

            if ($scope.classlistarray.length == 0 || $scope.classlistarray == null) {
                angular.forEach($scope.classDropdown, function (qq) {
                    if (qq.selected == true) {
                        $scope.classlistarray.push({ ASMCL_Id: qq.asmcL_Id })

                    }
                });
            }
            $scope.sectionlistarray = [];
            if ($scope.sectionDropdown != null && $scope.sectionDropdown.length > 0) {
                $scope.sectionlistarray = [];
                angular.forEach($scope.sectionDropdown, function (qq) {
                    if (qq.selected == true) {
                        $scope.sectionlistarray.push({ ASMS_Id: qq.asmS_Id })
                        // $scope.sectionlistarray.push({ ASMS_Id: qq.asmS_Id, ASMCL_Id: qq.asmcL_Id })
                    }
                });
            }
            if ($scope.sectionlistarray.length > 0) {
                $scope.OnChangeSubject();
            }

            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                //"ASMCL_Id": $scope.ASMCL_Id,
                //"ASMS_Id": $scope.ASMS_Id,
                "classlsttwo": $scope.classlistarray,
                "sectionlistarray": $scope.sectionlistarray,
            };

            apiService.create("StudentAttendanceReport/OnChangeSectionAbsent", data).then(function (promise) {
                $scope.subjectDropdown = promise.subjectlist;

                $scope.getstudentlist = promise.getstudentlist;
            });
        };
        $scope.isOptionsRequiredS = function () {
            return !$scope.subjectDropdown.some(function (item) {
                return item.selected;
            });
        };


        $scope.all_checkS = function (all) {
            $scope.subjectlistarray = [];
            $scope.obj.usercheckS = all;

            var toggleStatus = $scope.obj.usercheckS;
            angular.forEach($scope.subjectDropdown, function (role) {
                role.selected = toggleStatus;
            });


            angular.forEach($scope.subjectDropdown, function (qq) {
                if (qq.selected == true) {
                    // $scope.subjectlistarray.push({ ASMS_Id: qq.asmS_Id, ASMCL_Id: qq.asmcL_Id, ISMS_Id: qq.ismS_Id })
                    $scope.subjectlistarray.push({ ISMS_Id: qq.ismS_Id })


                }
            });
            if ($scope.obj.usercheckS == false) {
                $scope.subjectDropdown = [];
                $scope.subjectlistarray = [];
                $scope.subjectDropdown = [];
               // $scope.obj.usercheckS = false;
            }
        };








        //$scope.OnChangeClass = function (usercheckCC, ASMCL_Id) {



        //    $scope.subjectlistarray = [];

        //    angular.forEach($scope.subjectDropdown, function (aa) {
        //        if (aa.selected == true) {
        //            $scope.subjectlistarray.push({ ISMS_Id: aa.ismS_Id })
        //        }

        //    });

        //};
        $scope.OnChangeSubject = function (ismS_Id) {
            $scope.subjectlistarray = [];

            angular.forEach($scope.subjectDropdown, function (qq) {
                if (qq.selected == true) {
                    // $scope.subjectlistarray.push({ ASMS_Id: qq.asmS_Id, ASMCL_Id: qq.asmcL_Id, ISMS_Id: qq.ismS_Id })
                    $scope.subjectlistarray.push({ ISMS_Id: qq.ismS_Id })

                }
            });
          

        };

        $scope.OnReport = function () {
            if ($scope.myForm.$valid) {
                $scope.excel_flag = true;
                $scope.newarray = [];


                $scope.classlistarraynew = [];

                if ($scope.classlistarray != null || $scope.classlistarray > 0) {
                    $scope.classlistarraynew = $scope.classlistarray;
                }
                else {
                    angular.forEach($scope.classDropdown, function (qq) {
                        $scope.classlistarraynew.push({ ASMCL_Id: qq.asmcL_Id });
                    })
                }

                $scope.sectionlistarraynew = [];

                if ($scope.sectionlistarray != null || $scope.sectionlistarray > 0) {
                    $scope.sectionlistarraynew = $scope.sectionlistarray;
                }
                else {
                    angular.forEach($scope.sectionDropdown, function (qq) {
                        $scope.sectionlistarraynew.push({ ASMS_Id: qq.asmS_Id })
                        // $scope.sectionlistarraynew.push({ ASMS_Id: qq.asmS_Id , ASMCL_Id: qq.asmcL_Id });
                    })

                }

                $scope.subjectlistarraynew = [];

                if ($scope.subjectlistarray != null || $scope.subjectlistarray > 0) {
                    $scope.subjectlistarraynew = $scope.subjectlistarray;
                }
                else {
                    angular.forEach($scope.subjectDropdown, function (qq) {
                        //  $scope.subjectlistarraynew.push({ ASMS_Id: qq.asmS_Id, ASMCL_Id: qq.asmcL_Id, ISMS_Id: qq.ismS_Id });

                        $scope.subjectlistarraynew.push({ ISMS_Id: qq.ismS_Id });

                    })
                }

                var data = {
                    "ASMAY_Id": $scope.ASMAY_Id,
                    //"ASMCL_Id": $scope.ASMCL_Id,
                    //"ASMS_Id": $scope.ASMS_Id,
                    "reportflag": $scope.obj.type,
                    "classlsttwo": $scope.classlistarraynew,
                    "sectionlistarray": $scope.sectionlistarraynew,
                    "subjectlistarray": $scope.subjectlistarraynew

                };


                data.ISMS_Id = $scope.obj.ISMS_Id;
                data.fromdate = new Date($scope.obj.fromdate).toDateString();
                data.todate = new Date($scope.obj.todate).toDateString();


                apiService.create("StudentAttendanceReport/getstudetails", data).then(function (promise) {


                    if (promise.newarray_total !== null && promise.newarray_total.length > 0) {
                        $scope.excel_flag = false;

                        $scope.newarray_total = promise.newarray_total;

                    } else {
                        swal("No Records Found");
                    }
                });
            } else {
                $scope.submitted = true;
            }
        };



        $scope.selected = function (SelectedStudentRecord, index) {
            if ($scope.searchValue == '') {
                $scope.all = $scope.newarray_total.every(function (itm) { return itm.selected; });
                if ($scope.printdatatable.indexOf(SelectedStudentRecord) === -1) {
                    $scope.printdatatable.push(SelectedStudentRecord);
                }
                else {
                    $scope.printdatatable.splice($scope.printdatatable.indexOf(SelectedStudentRecord), 1);
                }
            }
            if ($scope.searchValue != '') {
                $scope.all = $scope.newarray_total.every(function (itm) { return itm.selected; });
                if ($scope.printdatatable.indexOf(SelectedStudentRecord) === -1) {
                    $scope.printdatatable.push(SelectedStudentRecord);
                }
                else {
                    $scope.printdatatable.splice($scope.printdatatable.indexOf(SelectedStudentRecord), 1);
                }
            }
        };


        $scope.toggleAll = function (all) {
            if ($scope.searchValue == '') {
                var toggleStatus = all;
                angular.forEach($scope.newarray_total, function (itm) {
                    itm.selected = toggleStatus;
                    if ($scope.all == true) {
                        if ($scope.printdatatable.indexOf(itm) === -1) {
                            $scope.printdatatable.push(itm);
                        }
                    }
                    else {
                        $scope.printdatatable.splice(itm);
                    }
                });
            }

            if ($scope.searchValue != '') {
                var toggleStatus = all;
                angular.forEach($scope.newarray_total, function (itm) {
                    itm.selected = toggleStatus;
                    if (all == true) {
                        if ($scope.printdatatable.indexOf(itm) === -1) {
                            $scope.printdatatable.push(itm);
                        }
                    }
                    else {
                        $scope.printdatatable.splice(itm);
                    }
                });
            }
        };

        $scope.sendSMS = function () {
            $scope.amst_array = [];
            if ($scope.printdatatable != null && $scope.printdatatable.length > 0) {
                angular.forEach($scope.printdatatable, function (itm) {
                    $scope.amst_array.push({ AMST_Id: itm.AMST_Id });
                });
            }


            $scope.classlistarraynew = [];

            if ($scope.classlistarray != null || $scope.classlistarray > 0) {
                $scope.classlistarraynew = $scope.classlistarray;
            }
            else {
                angular.forEach($scope.classDropdown, function (qq) {
                    $scope.classlistarraynew.push({ ASMCL_Id: qq.asmcL_Id });
                })
            }

            $scope.sectionlistarraynew = [];

            if ($scope.sectionlistarray != null || $scope.sectionlistarray > 0) {
                $scope.sectionlistarraynew = $scope.sectionlistarray;
            }
            else {
                angular.forEach($scope.sectionDropdown, function (qq) {
                    $scope.sectionlistarraynew.push({ ASMS_Id: qq.asmS_Id })
                    // $scope.sectionlistarraynew.push({ ASMS_Id: qq.asmS_Id, ASMCL_Id: qq.asmcL_Id });
                })

            }

            $scope.subjectlistarraynew = [];

            if ($scope.subjectlistarray != null || $scope.subjectlistarray > 0) {
                $scope.subjectlistarraynew = $scope.subjectlistarray;
            }
            else {
                angular.forEach($scope.sectionDropdown, function (qq) {
                    //  $scope.subjectlistarraynew.push({ ASMS_Id: qq.asmS_Id, ASMCL_Id: qq.asmcL_Id, ISMS_Id: qq.ismS_Id });

                    $scope.subjectlistarraynew.push({ ISMS_Id: qq.ismS_Id });

                })
            }


            if ($scope.printdatatable != null && $scope.printdatatable.length > 0) {
                var data = {
                    "ASMAY_Id": $scope.ASMAY_Id,
                    //"ASMCL_Id": $scope.ASMCL_Id,
                    //"ASMS_Id": $scope.ASMS_Id,
                    //"ISMS_Id": $scope.obj.ISMS_Id,
                    "classlsttwo": $scope.classlistarraynew,
                    "sectionlistarray": $scope.sectionlistarraynew,
                    "subjectlistarray": $scope.subjectlistarraynew,
                    "AbsentSMS": $scope.amst_array
                };
                data.fromdate = new Date($scope.obj.fromdate).toDateString();
                data.todate = new Date($scope.obj.todate).toDateString();
                apiService.create("StudentAttendanceReport/OnsendSMS", data).then(function (promise) {

                    if (promise.return_msg == "admin") {
                        swal("Kindly contact administrator!");
                        $state.reload();
                    }
                    else {
                        swal("SMS Sent Successfully!");
                        $state.reload();



                    }

                });
            }
            else {
                swal("Select Atlest one student!");
            }
        };

        $scope.setTodate = function () {
            $scope.obj.todate = null;
            $scope.minDatet = new Date($scope.obj.fromdate);
            $scope.minDatetd = new Date($scope.obj.fromdate);
            $scope.maxDatet = new Date($scope.minDatetd.getFullYear(),
                $scope.minDatetd.getMonth(),
                $scope.minDatetd.getDate());

        };

        $scope.exportToExcel = function () {
            var exportHref = "";
            var excelname = "";

            exportHref = Excel.tableToExcel('#excelid', 'Subject Wise Attendance Report');
            excelname = "Subject Wise Attendance Report.xlsx";

            $timeout(function () {
                var a = document.createElement('a');
                a.href = exportHref;
                a.download = excelname;
                document.body.appendChild(a);
                a.click();
                a.remove();
            }, 100);
        };

        $scope.printData = function () {
            var innerContents = "";
            var popupWinindow = "";


            innerContents = document.getElementById("printsection").innerHTML;

            popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
                '<link type="text/css" media="print" href="css/print/preadmission/AdmissionReports.css" rel="stylesheet" />' +
                '</head> <body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>'
            );
            popupWinindow.document.close();
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.submitted = false;

        $scope.Clearid = function () {
            $state.reload();
            $scope.asmaY_Id = "";
            $scope.asmcL_Id = "";
            $scope.asmC_Id = "";
            $scope.fromdate = "";
            $scope.todate = "";
            // $scope.amsT_Id = "";
            $scope.amM_Id = "";
        };

        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey === key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        };

    }
}) ();