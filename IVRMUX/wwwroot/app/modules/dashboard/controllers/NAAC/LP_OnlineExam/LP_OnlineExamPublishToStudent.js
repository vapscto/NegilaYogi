(function () {
    'use strict';
    angular.module('app').controller('LP_OnlineExamPublishToStudentController', LP_OnlineExamPublishToStudentController)

    LP_OnlineExamPublishToStudentController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', 'Excel', '$timeout', 'superCache', '$filter']
    function LP_OnlineExamPublishToStudentController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, Excel, $timeout, superCache, $filter) {

        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings !== null && ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        } else {
            paginationformasters = 10;
        }

        $scope.obj = {};
        $scope.ddate = {};
        $scope.ddate = new Date();
        $scope.usrname = localStorage.getItem('username');

        var copty;

        $scope.maxdate = new Date();

        $scope.itemsPerPage = paginationformasters;
        $scope.coptyright = copty;
        var logopath = "";
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings !== null && admfigsettings.length > 0) {
            logopath = admfigsettings[0].asC_Logo_Path;
        }

        $scope.imgname = logopath;
        $scope.reportbtn = true;

        $scope.loaddata = function () {
            var pageid = 4;
            apiService.getURI("LP_OnlineStudentExam/getloaddatareport", pageid).then(function (promise) {
                $scope.getyearlist = promise.getyearlist;
            });
        };

        $scope.onchangeyear = function () {
            $scope.ASMCL_Id = "";
            $scope.getclasslist = [];
            $scope.ISMS_Id = "";
            $scope.getsubjectlist = [];
            $scope.ASMS_Id = "";
            $scope.getsetionlist = [];
            $scope.LPMOEEX_Id = "";
            $scope.getexamlist = [];
            $scope.result = [];
            $scope.reportbtn = true;
            $scope.submitted11 = false;
            $scope.getstudentlistpublish = [];
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id
            };
            apiService.create("LP_OnlineStudentExam/onchangeyear", data).then(function (promise) {
                $scope.getclasslist = promise.getclasslist;
            });

        };

        $scope.onchangeclass = function () {
            $scope.ISMS_Id = "";
            $scope.getsubjectlist = [];
            $scope.ASMS_Id = "";
            $scope.getsetionlist = [];
            $scope.LPMOEEX_Id = "";
            $scope.getexamlist = [];
            $scope.result = [];
            $scope.reportbtn = true;
            $scope.submitted11 = false;
            $scope.getstudentlistpublish = [];
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "ASMCL_Id": $scope.ASMCL_Id
            };
            apiService.create("LP_OnlineStudentExam/onchangeclass", data).then(function (promise) {
                $scope.getsectionlist = promise.getsectionlist;
            });

        };

        $scope.OnchangeSection = function () {
            $scope.ISMS_Id = "";
            $scope.getsubjectlist = [];
            $scope.getstudentdetails = [];
            $scope.LPMOEEX_Id = "";
            $scope.getexamlist = [];
            $scope.result = [];
            $scope.reportbtn = true;
            $scope.submitted1 = false;
            $scope.submitted11 = false;
            $scope.checkall = false;
            $scope.submitted11 = false;
            $scope.getstudentlistpublish = [];
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "ASMCL_Id": $scope.ASMCL_Id,
                "ASMS_Id": $scope.ASMS_Id

            };
            apiService.create("LP_OnlineStudentExam/OnchangeSection", data).then(function (promise) {
                $scope.getsubjectlist = promise.getsubjectlist;
            });
        };

        $scope.onchangesubject = function () {

            $scope.LPMOEEX_Id = "";
            $scope.getexamlist = [];
            $scope.result = [];
            $scope.reportbtn = true;
            $scope.submitted11 = false;
            $scope.getstudentlistpublish = [];
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "ASMCL_Id": $scope.ASMCL_Id,
                "ISMS_Id": $scope.ISMS_Id,
                "ASMS_Id": $scope.ASMS_Id
            };
            apiService.create("LP_OnlineStudentExam/onchangesubject", data).then(function (promise) {
                $scope.getexamlist = promise.getexamlist;
            });
        };

        $scope.OnChangeExam = function () {
            $scope.getstudentlistpublish = [];
            $scope.submitted11 = false;
            angular.forEach($scope.getexamlist, function (dd) {
                if (dd.lpmoeeX_Id === parseInt($scope.LPMOEEX_Id)) {
                    $scope.obj.FMCB_fromDATE = new Date(dd.lpmoeeX_FromDateTime);
                    $scope.obj.FMCB_toDATE = new Date(dd.lpmoeeX_ToDateTime);
                }
            });
        };

        $scope.GetStudentListForPublish = function () {
            $scope.submitted11 = false;
            $scope.getstudentlistpublish = [];
            if ($scope.myForm.$valid) {
                $scope.fromdate = new Date($scope.obj.FMCB_fromDATE).toDateString();
                $scope.todate = new Date($scope.obj.FMCB_toDATE).toDateString();

                var data = {
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "ASMCL_Id": $scope.ASMCL_Id,
                    "ISMS_Id": $scope.ISMS_Id,
                    "LPMOEEX_Id": $scope.LPMOEEX_Id,
                    "fromdate": $scope.fromdate,
                    "todate": $scope.todate,
                    "ASMS_Id": $scope.ASMS_Id
                };

                apiService.create("LP_OnlineStudentExam/GetStudentListForPublish", data).then(function (promise) {
                    if (promise !== null) {
                        if (promise.getstudentlistpublish !== null && promise.getstudentlistpublish.length > 0) {
                            $scope.getstudentlistpublish = promise.getstudentlistpublish;

                            angular.forEach($scope.getstudentlistpublish, function (dd) {
                                dd.checkedvalue = false;
                            });

                        } else {
                            swal("No Records Found");
                        }
                    }
                });
            }
            else {
                $scope.submitted1 = true;
            }
        };

        $scope.CheckStudentMarksEntered = function (obj) {

            if (obj.checkedvalue === true) {
                var data = {
                    "AMST_Id": obj.amsT_Id,
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "ASMCL_Id": $scope.ASMCL_Id,
                    "ISMS_Id": $scope.ISMS_Id,
                    "LPMOEEX_Id": $scope.LPMOEEX_Id,
                    "ASMS_Id": $scope.ASMS_Id,
                    "LPSTUEX_Id": obj.lpstueX_Id,
                };

                apiService.create("LP_OnlineStudentExam/CheckStudentMarksEntered", data).then(function (promise) {
                    if (promise !== null) {
                        if (promise.message === "MarksNotCalculated") {                           
                            obj.checkedvalue = false;
                            swal("Still Marks Are Not Updated For This Student, So Kindly Update Marks And Then Publish");
                        }
                    }
                });
            }
        };

        $scope.PublishToStudent = function (objform) {

            $scope.submitted1 = true;
            $scope.reportbtn = true;
            $scope.result = [];

            if (objform.$valid) {
                $scope.fromdate = new Date($scope.obj.FMCB_fromDATE).toDateString();
                $scope.todate = new Date($scope.obj.FMCB_toDATE).toDateString();

                $scope.selectedstudetnts = [];

                angular.forEach($scope.getstudentlistpublish, function (d) {
                    if (d.checkedvalue) {
                        $scope.selectedstudetnts.push({
                            AMST_Id: d.amsT_Id, LPSTUEX_Id: d.lpstueX_Id,
                            LPSTUEX_PublishToStudent: d.lpstueX_PublishToStudent
                        });
                    }
                });

                swal({
                    title: "Are you sure",
                    text: "Do You Want To Publish Marks To Student Portal ?",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonColor: "#DD6B55", confirmButtonText: "Publish",
                    cancelButtonText: "Cancel",
                    closeOnConfirm: false,
                    closeOnCancel: false
                },
                    function (isConfirm) {
                        if (isConfirm) {
                            var data = {
                                "ASMAY_Id": $scope.ASMAY_Id,
                                "ASMCL_Id": $scope.ASMCL_Id,
                                "ISMS_Id": $scope.ISMS_Id,
                                "LPMOEEX_Id": $scope.LPMOEEX_Id,
                                "fromdate": $scope.fromdate,
                                "todate": $scope.todate,
                                "ASMS_Id": $scope.ASMS_Id,
                                "selectedstudetntspublish": $scope.selectedstudetnts
                            };

                            apiService.create("LP_OnlineStudentExam/PublishToStudent", data).then(function (promise) {

                                if (promise !== null) {
                                    if (promise.message === "Update") {
                                        swal("Marks Published To Student Portal");
                                    } else if (promise.message === "MarksNotCalculated") {
                                        swal("Still Marks Are Not Updated For Selected Exam, Kindly Enter The Marks First");
                                    } else {
                                        swal("Failed To Publish");
                                    }
                                    $scope.getstudentlistpublish = [];
                                }
                            });
                        }
                        else {
                            swal("Publish Cancelled");
                        }
                    });
            }
            else {
                $scope.submitted11 = true;
            }
        };

        $scope.ViewStudentWiseMarks = function (objs) {
            var data = {
                "AMST_Id": objs.AMST_Id,
                "ASMAY_Id": $scope.ASMAY_Id,
                "ASMCL_Id": $scope.ASMCL_Id,
                "ISMS_Id": $scope.ISMS_Id,
                "LPMOEEX_Id": $scope.LPMOEEX_Id,
                "ASMS_Id": $scope.ASMS_Id
            };

            apiService.create("LP_OnlineStudentExam/ViewStudentWiseMarks", data).then(function (promise) {
                if (promise.getmarksdetails !== null && promise.getmarksdetails.length > 0) {
                    $('#viewmarks').modal('show');
                    $scope.getmarksdetails = promise.getmarksdetails;


                    angular.forEach($scope.getexamlist, function (dd) {
                        if (dd.lpmoeeX_Id === parseInt($scope.LPMOEEX_Id)) {
                            $scope.examname1 = dd.lpmoeeX_ExamName;
                        }
                    });

                    angular.forEach($scope.getsubjectlist, function (dd) {
                        if (dd.ismS_Id === parseInt($scope.ISMS_Id)) {
                            $scope.subjectname1 = dd.ismS_SubjectName;
                        }
                    });

                    angular.forEach($scope.getyearlist, function (dd) {
                        if (dd.asmaY_Id === parseInt($scope.ASMAY_Id)) {
                            $scope.yearname1 = dd.asmaY_Year;
                        }
                    });

                    $scope.getstudentdetails = promise.getstudentdetails;

                    $scope.maxmarks = $scope.getmarksdetails[0].lpstueX_TotalMaxMarks;
                    $scope.marksobtained = $scope.getmarksdetails[0].lpstueX_TotalMarks;

                    $scope.examdate = $scope.getmarksdetails[0].lpstueX_Date;
                    $scope.durationtake = $scope.getmarksdetails[0].lpstueX_TotalTime;

                    $scope.questionattempt = $scope.getmarksdetails[0].lpstueX_TotalQnsAnswered;
                    $scope.questioncorrect = $scope.getmarksdetails[0].lpstueX_TotalCorrectAns;
                    $scope.maxmarkspercentage = $scope.getmarksdetails[0].lpstueX_Percentage;

                    $scope.getexamdetails = promise.getexamdetails;
                    $scope.lpmoeeX_UploadExamPaperFlg = $scope.getexamdetails[0].lpmoeeX_UploadExamPaperFlg;

                    $scope.getallmarksdetails = promise.getallmarksdetails;

                    if ($scope.lpmoeeX_UploadExamPaperFlg === true) {
                        angular.forEach($scope.getallmarksdetails, function (dd) {
                            var img = dd.lpstuexaS_AnswerSheetPath;
                            var imagarr = img.split('.');
                            var lastelement = imagarr[imagarr.length - 1];
                            dd.quesfiletypeview1 = lastelement;
                            if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                                dd.quesdocument_Pathnewview1 = "https://view.officeapps.live.com/op/view.aspx?src=" + data.lpstuexaS_AnswerSheetPath;
                            }
                        });
                    }

                } else {
                    swal("No Records Found");
                }
            });
        };

        $scope.isOptionsRequired = function () {
            return !$scope.getstudentlistpublish.some(function (options) {
                return options.checkedvalue;
            });
        };


        $scope.cancel = function () {
            $state.reload();
        };

        $scope.sort = function (key) {
            $scope.sortKey = key;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        };

        $scope.interacted = function (field) {
            return $scope.submitted1;
        };

        $scope.interacted1 = function (field) {
            return $scope.submitted11;
        };

        $scope.optionToggled123 = function () {
            $scope.obj.all123 = $scope.getstudentlistpublish.every(function (itm) { return itm.checkedvalue; });
        };

        $scope.toggleAll123 = function (all123) {
            angular.forEach($scope.getstudentlistpublish, function (dd) {
                dd.checkedvalue = all123;
            });
        };

        $scope.IsHidden = true;
        $scope.ShowHide = function () {
            $scope.IsHidden = $scope.IsHidden ? false : true;
        };


        $scope.printdatatable = [];
        $scope.exportToExcel = function (table1) {
            $scope.sheetname = "Year_" + $scope.yearname + "- Subject_" + $scope.subjectname + " - Exam_" + $scope.examname;
            //var exportHref = Excel.tableToExcel(table1, $scope.sheetname);
            //$timeout(function () { location.href = exportHref; }, 100);

            var exportHref = Excel.tableToExcel(table1, $scope.sheetname);
            var excelname = $scope.sheetname + ".xls";
            $timeout(function () {
                var a = document.createElement('a');
                a.href = exportHref;
                a.download = excelname;
                document.body.appendChild(a);
                a.click();
                a.remove();
            }, 100);

        };

        $scope.printData = function (printSectionId) {

            var innerContents = document.getElementById("printSectionId").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print.css" />' +
                '<link type="text/css" rel="stylesheet" href="css/style.css" />' +
                '<link type="text/css" href="plugins/bootstrap/css/bootstrap.min.css" rel="stylesheet" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();

        };

        $scope.toggleAll = function () {
            $scope.printdatatable = [];
            var toggleStatus = $scope.all;
            angular.forEach($scope.result, function (itm) {
                itm.selected = toggleStatus;
                if ($scope.all === true) {
                    $scope.printdatatable.push(itm);
                }
                else {
                    $scope.printdatatable.splice(itm);
                }
            });
            $scope.get_total_student_print();
        };
    }
})();