(function () {
    'use strict';
    angular.module('app').controller('PassFailReportController', PassFailReportController)
    PassFailReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout']
    function PassFailReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout) {

        $scope.ddate = {};
        $scope.ddate = new Date();

        $scope.class_disable = true;
        $scope.sec_disable = true;
        $scope.stud_disable = true;
        $scope.report_print = true;

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




        $scope.BindData = function () {
            apiService.getDATA("PassFailReport/getdetails").then(function (promise) {
                $scope.acdlist = promise.acdlist;
                $scope.catlist = promise.catlist;
                $scope.ctlist = promise.ctlist;
                $scope.seclist = promise.seclist;
                $scope.examlist = promise.examlist;
                $scope.studentlist = promise.studentlist;
            });
        };

        $scope.onselectCategory = function (ASMAY_Id, EMCA_Id) {
            $scope.main_list = [];
            $scope.main_list2 = [];
            $scope.main_list3 = [];
            $scope.main_list4 = [];
            $scope.student_list = [];
            $scope.examlist = [];
            $scope.report_print = true;
            var data = {
                "ASMAY_Id": ASMAY_Id,
                "EMCA_Id": EMCA_Id
            };
            apiService.create("PassFailReport/onselectCategory", data).then(function (promise) {
                $scope.ctlist = promise.ctlist;
                $scope.examlist = promise.examlist;
            });
        };


        $scope.onselectclass = function (ASMCL_Id, ASMAY_Id, EMCA_Id) {
            $scope.main_list = [];
            $scope.main_list2 = [];
            $scope.main_list3 = [];
            $scope.student_list = [];
            $scope.main_list4 = [];
            $scope.examlist = [];
            $scope.report_print = true;
            var data = {
                "ASMCL_Id": ASMCL_Id,
                "ASMAY_Id": ASMAY_Id,
                "EMCA_Id": EMCA_Id
            };
            apiService.create("PassFailReport/onselectclass", data).then(function (promise) {
                $scope.seclist = promise.seclist;
            });
        };


        $scope.onselectSection = function () {
            $scope.main_list = [];
            $scope.main_list2 = [];
            $scope.main_list3 = [];
            $scope.main_list4 = [];
            $scope.student_list = [];
            $scope.examlist = [];
            $scope.report_print = true;
            var data = {
                "ASMCL_Id": $scope.ASMCL_Id,
                "ASMAY_Id": $scope.ASMAY_Id,
                "ASMS_Id": $scope.ASMS_Id
            };
            apiService.create("PassFailReport/onselectSection", data).then(function (promise) {
                $scope.studentlist = promise.studentlist;
                $scope.examlist = promise.examlist;
            });
        };

        $scope.onstudentnamechange = function () {
            $scope.main_list = [];
            $scope.main_list2 = [];
            $scope.main_list3 = [];
            $scope.main_list4 = [];
            $scope.student_list = [];
            $scope.report_print = true;
        };


        $scope.submitted = false;

        $scope.obj = {};

        $scope.onreport = function (obj) {

            $scope.main_list = [];
            $scope.main_list2 = [];
            $scope.main_list3 = [];
            $scope.main_list4 = [];
            $scope.student_list = [];
            $scope.report_print = true;

            $scope.submitted = true;

            if ($scope.myForm.$valid) {

                var classid = 0;
                var sectionid = 0;
                var stdid = 0;
                var emeid = 0;

                if ($scope.qualification_type === 'all') {
                    classid = 0;
                    sectionid = 0;
                    stdid = 0;
                    emeid = $scope.EME_Id;
                }
                else if ($scope.qualification_type === 'individual' || $scope.qualification_type === 'Absent' || $scope.qualification_type === 'Medical'
                    || $scope.qualification_type === 'onDuty') {
                    stdid = 0;
                    classid = $scope.ASMCL_Id;
                    sectionid = $scope.ASMS_Id;
                    emeid = $scope.EME_Id;
                }
                else if ($scope.qualification_type === 'Studentwise') {
                    //emeid = 0;
                    emeid = $scope.EME_Id;
                    //stdid =  obj.AMST_Id.amsT_Id;
                    stdid = 0;
                    classid = $scope.ASMCL_Id;
                    sectionid = $scope.ASMS_Id;
                }

                var data = {
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "ASMCL_Id": classid,
                    "ASMS_Id": sectionid,
                    "EMCA_Id": $scope.EMCA_Id,
                    "EME_Id": emeid,
                    "AMST_Id": stdid,
                    "report_type": $scope.qualification_type
                };

                apiService.create("PassFailReport/onreport", data).then(function (promise) {
                    //$scope.main_list = promise.datareport;
                    //$scope.main_list2 = promise.datareport2;
                    //$scope.main_list3 = promise.datareport3;
                    //$scope.main_list4 = promise.datareport4;

                    $scope.main_list = promise.datareport;

                    angular.forEach($scope.acdlist, function (dd) {
                        if (dd.asmaY_Id === parseInt($scope.ASMAY_Id)) {
                            $scope.year = dd.asmaY_Year;
                        }
                    });

                    angular.forEach($scope.examlist, function (dde) {
                        if (dde.emE_Id === parseInt($scope.EME_Id)) {
                            $scope.examname = dde.emE_ExamName;
                        }
                    });


                    if ($scope.qualification_type === 'individual' || $scope.qualification_type === 'all') {
                        if ($scope.main_list === null || $scope.main_list === undefined || $scope.main_list.length === 0) {
                            $scope.main_list = [];
                            $scope.report_print = true;
                            swal('No Records Found!!!');
                        }
                        else {
                            $scope.report_print = false;
                            $scope.details = "For Year : " + $scope.year + " " + "Exam  : " + $scope.examname;
                            $scope.main_list = promise.datareport;
                            $scope.main_temp_list = $scope.main_list;
                            var main_temp_list = [];
                            angular.forEach($scope.main_list, function (obj) {
                                var count = 0;
                                angular.forEach(main_temp_list, function (obj2) {
                                    if (obj2.ASMCL_ClassName === obj.ASMCL_ClassName) {
                                        count++;
                                    }
                                });

                                if (count === 0) {
                                    var temp_array = [];
                                    angular.forEach($scope.main_list, function (obj1) {
                                        if (obj1.ASMCL_ClassName === obj.ASMCL_ClassName) {
                                            temp_array.push(obj1);
                                        }
                                    });
                                    main_temp_list.push({ ASMCL_Id: obj.ASMCL_Id, ASMCL_ClassName: obj.ASMCL_ClassName, temp_array: temp_array });
                                }
                            });
                            $scope.main_temp_list = main_temp_list;
                            console.log(main_temp_list);
                        }
                    }

                    else
                        if ($scope.qualification_type === 'Studentwise') {
                            if (promise.datareport === null || promise.datareport === undefined || promise.datareport.length === 0) {
                                $scope.main_list2 = [];
                                $scope.report_print = true;
                                swal('No Records Found!!!');
                            }
                            else {

                                $scope.report_print = false;
                                angular.forEach($scope.ctlist, function (dd) {
                                    if (parseInt($scope.ASMCL_Id) === dd.asmcL_Id) {
                                        $scope.classname = dd.asmcL_ClassName;
                                    }
                                });

                                $scope.sectionname = "All";

                                angular.forEach($scope.seclist, function (dd) {
                                    if (parseInt($scope.ASMS_Id) === dd.asmS_Id) {
                                        $scope.sectionname = dd.asmC_SectionName;
                                    }
                                });

                                $scope.details = "For Year : " + $scope.year + " " + "Class - Section  : " + $scope.classname + "-" + $scope.sectionname;
                                $scope.main_list2 = promise.datareport;

                                $scope.student_list = [];
                                $scope.student_subject_list = [];

                                angular.forEach($scope.main_list2, function (stu) {
                                    if ($scope.student_list.length === 0) {
                                        $scope.student_list.push({
                                            AMST_Id: stu.AMST_Id, AMST_FirstName: stu.AMST_FirstName, ASMCL_ClassName: stu.ASMCL_ClassName,
                                            ASMC_SectionName: stu.ASMC_SectionName
                                        });
                                    } else if ($scope.student_list.length > 0) {
                                        var studentcount = 0;
                                        angular.forEach($scope.student_list, function (dd) {
                                            if (dd.AMST_Id === stu.AMST_Id) {
                                                studentcount += 1;
                                            }
                                        });

                                        if (studentcount === 0) {
                                            $scope.student_list.push({
                                                AMST_Id: stu.AMST_Id, AMST_FirstName: stu.AMST_FirstName, ASMCL_ClassName: stu.ASMCL_ClassName,
                                                ASMC_SectionName: stu.ASMC_SectionName
                                            });
                                        }
                                    }
                                });

                                angular.forEach($scope.student_list, function (stu) {
                                    $scope.student_subject_list = [];
                                    angular.forEach($scope.main_list2, function (dd) {
                                        if (stu.AMST_Id === dd.AMST_Id) {
                                            $scope.student_subject_list.push({
                                                AMST_Id: stu.AMST_Id, ISMS_SubjectName: dd.ISMS_SubjectName, ESTMPS_MaxMarks: dd.ESTMPS_MaxMarks,
                                                ESTMPS_ObtainedMarks: dd.ESTMPS_ObtainedMarks
                                            });
                                        }
                                    });
                                    stu.subjectlist = $scope.student_subject_list;
                                });
                            }
                        }
                        //else if ($scope.qualification_type === 'Medical') {
                        //    if ($scope.main_list3 === null || $scope.main_list3 === undefined || $scope.main_list3.length === 0) {
                        //        $scope.main_list3 = [];
                        //        $scope.report_print = true;
                        //        swal('No Records Found!!!');
                        //    }
                        //    else {
                        //        $scope.report_print = false;
                        //        $scope.main_list3 = promise.datareport;
                        //    }
                        //}
                             
                        else if ($scope.qualification_type === 'Absent' || $scope.qualification_type === 'onDuty' || $scope.qualification_type === 'Medical') {
                            if (promise.datareport === null || promise.datareport === undefined || promise.datareport.length === 0) {
                                $scope.main_list4 = [];
                                $scope.report_print = true;
                                swal('No Records Found!!!');
                            }
                            else {
                                $scope.report_print = false;
                                angular.forEach($scope.ctlist, function (dd) {
                                    if (parseInt($scope.ASMCL_Id) === dd.asmcL_Id) {
                                        $scope.classname = dd.asmcL_ClassName;
                                    }
                                });
                                angular.forEach($scope.seclist, function (dd) {
                                    if (parseInt($scope.ASMS_Id) === dd.asmS_Id) {
                                        $scope.sectionname = dd.asmC_SectionName;
                                    }
                                });

                                if ($scope.qualification_type === 'onDuty') {
                                    $scope.type = "On Duty";
                                } else if ($scope.qualification_type === 'Absent') {
                                    $scope.type = "Absent";
                                } else {
                                    $scope.type = "Medical";
                                }

                                $scope.details = " - " + $scope.type + " For Year : " + $scope.year + " " + "Exam  : " + $scope.examname;

                                $scope.main_list4 = promise.datareport;

                                $scope.student_list = [];
                                $scope.student_subject_list = [];

                                angular.forEach($scope.main_list4, function (stu) {
                                    if ($scope.student_list.length === 0) {
                                        $scope.student_list.push({
                                            AMST_Id: stu.AMST_Id, AMST_FirstName: stu.AMST_FirstName, ASMCL_ClassName: stu.ASMCL_ClassName,
                                            ASMC_SectionName: stu.ASMC_SectionName
                                        });
                                    } else if ($scope.student_list.length > 0) {
                                        var studentcount = 0;
                                        angular.forEach($scope.student_list, function (dd) {
                                            if (dd.AMST_Id === stu.AMST_Id) {
                                                studentcount += 1;
                                            }
                                        });

                                        if (studentcount === 0) {
                                            $scope.student_list.push({
                                                AMST_Id: stu.AMST_Id, AMST_FirstName: stu.AMST_FirstName, ASMCL_ClassName: stu.ASMCL_ClassName,
                                                ASMC_SectionName: stu.ASMC_SectionName
                                            });
                                        }
                                    }
                                });

                                angular.forEach($scope.student_list, function (stu) {
                                    $scope.student_subject_list = [];
                                    angular.forEach($scope.main_list4, function (dd) {
                                        if (stu.AMST_Id === dd.AMST_Id) {
                                            $scope.student_subject_list.push({
                                                AMST_Id: stu.AMST_Id, ISMS_SubjectName: dd.ISMS_SubjectName
                                            });
                                        }
                                    });
                                    stu.subjectlist = $scope.student_subject_list;
                                });
                            }
                        }
                    $scope.getinst = promise.getinstitution;
                    $scope.institutename = $scope.getinst[0].mI_Name;
                    $scope.instituteaddress = $scope.getinst[0].mI_Address1;
                });
            }
        };

        $scope.onselectradio = function () {
            $scope.student_list = [];
            $scope.main_list = [];
            $scope.main_list2 = [];
            $scope.main_list3 = [];
            $scope.main_list4 = [];
            $scope.search = "";
            $scope.report_print = true;
            $scope.EME_Id = '';

            if ($scope.qualification_type === 'all') {
                $scope.class_disable = true;
                $scope.sec_disable = true;
                $scope.stud_disable = true;
                $scope.ASMCL_Id = '';
                $scope.ASMS_Id = '';
                $scope.AMST_Id = '';
                $scope.submitted = false;
                $scope.myForm.$setPristine();
                $scope.myForm.$setUntouched();
            } else {
                $scope.class_disable = false;
                $scope.sec_disable = false;
                $scope.stud_disable = false;
            }
            if ($scope.qualification_type === 'individual' || $scope.qualification_type === 'Absent' || $scope.qualification_type === 'Medical' || $scope.qualification_type === 'onDuty') {
                $scope.stud_disable = true;
                $scope.AMST_Id = '';
                $scope.submitted = false;
                $scope.myForm.$setPristine();
                $scope.myForm.$setUntouched();

            } if ($scope.qualification_type === 'Studentwise') {
                $scope.EME_Id = '';
                $scope.submitted = false;
                $scope.myForm.$setPristine();
                $scope.myForm.$setUntouched();
            }
            else {
                $scope.exam_disable = false;
            }
        };

        $scope.cancel = function () {
            $state.reload();
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };


        $scope.printData = function () {
            var innerContents = "";
            var popupWinindow = "";
            if ($scope.qualification_type === 'all' || $scope.qualification_type === 'individual') {
                innerContents = document.getElementById("table").innerHTML;
                popupWinindow = window.open('');
                popupWinindow.document.open();
                popupWinindow.document.write('<html><head>' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/CumulativeReportPdf.css" />' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                    '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
                popupWinindow.document.close();
            }
            else if ($scope.qualification_type === 'Studentwise') {
                innerContents = document.getElementById("table2").innerHTML;
                popupWinindow = window.open('');
                popupWinindow.document.open();
                popupWinindow.document.write('<html><head>' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/CumulativeReportPdf.css" />' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                    '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
                popupWinindow.document.close();
            }

            else if ($scope.qualification_type === 'Absent' || $scope.qualification_type === 'onDuty' || $scope.qualification_type === 'Medical') {
                innerContents = document.getElementById("table4").innerHTML;
                popupWinindow = window.open('');
                popupWinindow.document.open();
                popupWinindow.document.write('<html><head>' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/CumulativeReportPdf.css" />' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                    '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
                popupWinindow.document.close();
            }
        };

        $scope.exportToExcel = function () {
            var data = "";

            if ($scope.qualification_type === 'all' || $scope.qualification_type === 'individual') {
                data = "#printSectionIdecel";
            } else if ($scope.qualification_type === 'Studentwise') {
                data = "#printSectionIdstudentecel";
            } else {
                data = "#printSectionIdabsentecel";
            }


            var exportHref = Excel.tableToExcel(data, 'sheet name');
            var excelname = "Pass Fail Report.xls";
            $timeout(function () {
                var a = document.createElement('a');
                a.href = exportHref;
                a.download = excelname;
                document.body.appendChild(a);
                a.click();
                a.remove();
            }, 100);
        };
    }

})();