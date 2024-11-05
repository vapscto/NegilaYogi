
(function () {
    'use strict';
    angular.module('app').controller('CollegeCumulativeAvgBestReportController', CollegeCumulativeAvgBestReportController)

    CollegeCumulativeAvgBestReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout']
    function CollegeCumulativeAvgBestReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout) {

        $scope.reportd = true;

        $scope.BindData = function () {
            var pageid = 2;
            apiService.getURI("CollegeCumulativeAvgBestReport/Getdetails", pageid).then(function (promise) {
                $scope.yearlist = promise.getyear;
                $scope.datagriv = false;
            });
        };

        $scope.onchangeyear = function () {
            $scope.AMCO_Id = "";
            $scope.AMB_Id = "";
            $scope.AMSE_Id = "";
            $scope.ACMS_Id = "";
            $scope.EME_Id = "";
            $scope.ACSS_Id = "";
            $scope.ACST_Id = "";
            $scope.datagriv = false;
            $scope.studentslt1 = [];
            $scope.studentslt = [];
            $scope.reportd = true;
            $scope.reportdata = [];

            var data = {
                "ASMAY_Id": $scope.ASMAY_Id
            };
            apiService.create("CollegeCumulativeAvgBestReport/onchangeyear", data).then(function (promise) {
                $scope.course_list = promise.getcourse;
            });
        };

        $scope.onchangecourse = function () {
            $scope.AMB_Id = "";
            $scope.AMSE_Id = "";
            $scope.ACMS_Id = "";
            $scope.EME_Id = "";
            $scope.ACSS_Id = "";
            $scope.ACST_Id = "";
            $scope.datagriv = false;
            $scope.studentslt1 = [];
            $scope.studentslt = [];
            $scope.reportd = true;
            $scope.reportdata = [];
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "AMCO_Id": $scope.AMCO_Id
            };
            apiService.create("CollegeCumulativeAvgBestReport/onchangecourse", data).then(function (promise) {
                $scope.branch_list = promise.getbranch;
            });
        };

        $scope.onchangebranch = function () {

            $scope.AMSE_Id = "";
            $scope.ACMS_Id = "";
            $scope.EME_Id = "";
            $scope.ACSS_Id = "";
            $scope.ACST_Id = "";
            $scope.datagriv = false;
            $scope.studentslt1 = [];
            $scope.studentslt = [];
            $scope.reportd = true;
            $scope.reportdata = [];
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "AMCO_Id": $scope.AMCO_Id,
                "AMB_Id": $scope.AMB_Id
            };
            apiService.create("CollegeCumulativeAvgBestReport/onchangebranch", data).then(function (promise) {
                $scope.semisters_list = promise.getsemester;
            });
        };

        $scope.onchangesemester = function () {
            $scope.ACMS_Id = "";
            $scope.EME_Id = "";
            $scope.ACSS_Id = "";
            $scope.ACST_Id = "";
            $scope.datagriv = false;
            $scope.studentslt1 = [];
            $scope.studentslt = [];
            $scope.reportd = true;
            $scope.reportdata = [];
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "AMCO_Id": $scope.AMCO_Id,
                "AMB_Id": $scope.AMB_Id,
                "AMSE_Id": $scope.AMSE_Id
            };
            apiService.create("CollegeCumulativeAvgBestReport/onchangesemester", data).then(function (promise) {
                $scope.seclist = promise.getsection;
                $scope.subjectschema_list = promise.subjectshemalist;
            });
        };

        $scope.onchangesubjectscheme = function () {
            $scope.EME_Id = "";
            $scope.ACST_Id = "";
            $scope.datagriv = false;
            $scope.studentslt1 = [];
            $scope.studentslt = [];
            $scope.reportd = true;
            $scope.reportdata = [];
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "AMCO_Id": $scope.AMCO_Id,
                "AMB_Id": $scope.AMB_Id,
                "AMSE_Id": $scope.AMSE_Id,
                "ACSS_Id": $scope.ACSS_Id
            };
            apiService.create("CollegeCumulativeAvgBestReport/onchangesubjectscheme", data).then(function (promise) {
                $scope.schmetype_list = promise.schmetypelist;
            });
        };


        $scope.onchangeschemetype = function () {
            $scope.EME_Id = "";
            $scope.datagriv = false;
            $scope.studentslt1 = [];
            $scope.studentslt = [];
            $scope.reportdata = [];
            $scope.reportd = true;
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "AMCO_Id": $scope.AMCO_Id,
                "AMB_Id": $scope.AMB_Id,
                "AMSE_Id": $scope.AMSE_Id,
                "ACSS_Id": $scope.ACSS_Id,
                "ACST_Id": $scope.ACST_Id,
                "ACMS_Id": $scope.ACMS_Id
            };
            apiService.create("CollegeCumulativeAvgBestReport/onchangeschemetype", data).then(function (promise) {
                $scope.exam_list = promise.getexam;
                $scope.subjectlist = promise.getsubject;
            });
        };

        $scope.isOptionsRequired = function () {
            return !$scope.exam_list.some(function (options) {
                return options.EME_Id;
            });
        };


        // TO Save The Data
        $scope.submitted = false;
        $scope.Getreport = function () {
            $scope.reportd = true;
            $scope.submitted = true;
            $scope.reportdata = [];
            if ($scope.myForm.$valid) {
                $scope.examid = [];

                angular.forEach($scope.exam_list, function (ddd) {
                    if (ddd.EME_Id === true) {
                        $scope.examid.push({ EME_Id: ddd.emE_Id, EME_ExamName: ddd.emE_ExamName });
                    }
                });
                var bestcountf = 0;
                if ($scope.bestcount !== undefined && $scope.bestcount !== null && $scope.bestcount !== "") {
                    bestcountf = $scope.bestcount;
                } else {
                    bestcountf = 0;
                }

                var data = {
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "AMCO_Id": $scope.AMCO_Id,
                    "AMB_Id": $scope.AMB_Id,
                    "AMSE_Id": $scope.AMSE_Id,
                    "ACMS_Id": $scope.ACMS_Id,
                    "ACSS_Id": $scope.ACSS_Id,
                    "ACST_Id": $scope.ACST_Id,
                    "ISMS_Id": $scope.ISMS_Id,
                    "examids": $scope.examid,
                    "Flag": $scope.Flag,
                    "bestcount": bestcountf
                };

                apiService.create("CollegeCumulativeAvgBestReport/Getcmreport", data).
                    then(function (promise) {
                        
                        if (promise.reportdata !== null && promise.reportdata.length > 0) {
                            $scope.datagriv = true;
                            $scope.reportd = false;
                            $scope.masterinst = promise.configuration;
                            var count = 0;
                            if ($scope.masterinst.length > 0) {
                                if ($scope.masterinst[0].exmConfig_RegnoColumnDisplay === true) {
                                    $scope.regno = true;
                                    count = count + 1;

                                } else {
                                    $scope.regno = false;
                                }

                                if ($scope.masterinst[0].exmConfig_AdmnoColumnDisplay === true) {
                                    $scope.admno = true;
                                    count = count + 1;
                                } else {
                                    $scope.admno = false;
                                }

                                if ($scope.masterinst[0].exmConfig_RollnoColumnDisplay === true) {
                                    $scope.rollno = true;
                                    count = count + 1;
                                } else {
                                    $scope.rollno = false;
                                }

                                if (count === 0) {
                                    $scope.admno = true;
                                    $scope.rollno = true;
                                }


                            } else {
                                $scope.admno = true;
                                $scope.rollno = true;
                            }

                            $scope.reportdata = promise.reportdata;

                            angular.forEach($scope.course_list, function (itm) {
                                if (itm.amcO_Id === parseInt($scope.AMCO_Id)) {
                                    $scope.cour = itm.amcO_CourseName;
                                }
                            });
                            angular.forEach($scope.branch_list, function (itm) {
                                if (itm.amB_Id === parseInt($scope.AMB_Id)) {
                                    $scope.branc = itm.amB_BranchName;
                                }
                            });

                            angular.forEach($scope.yearlist, function (itm) {
                                if (itm.asmaY_Id === parseInt($scope.ASMAY_Id)) {
                                    $scope.yearname = itm.asmaY_Year;
                                }
                            });

                            angular.forEach($scope.seclist, function (itm) {
                                if (itm.acmS_Id === parseInt($scope.ACMS_Id)) {
                                    $scope.sectionname = itm.acmS_SectionName;
                                }
                            });

                            angular.forEach($scope.semisters_list, function (itm) {
                                if (itm.amsE_Id === parseInt($scope.AMSE_Id)) {
                                    $scope.semestername = itm.amsE_SEMName;
                                }
                            });

                            angular.forEach($scope.subjectlist, function (itm) {
                                if (itm.ismS_Id === parseInt($scope.ISMS_Id)) {
                                    if (itm.ismS_SubjectCode !== null && itm.ismS_SubjectCode !== "") {
                                        $scope.subjectname = itm.ismS_SubjectName + " : " + itm.ismS_SubjectCode;
                                    } else {
                                        $scope.subjectname = itm.ismS_SubjectName;
                                    }

                                }
                            });

                            $scope.instname = promise.instname;
                            $scope.inst_name = $scope.instname[0].mI_Name;
                            $scope.inst_address1 = $scope.instname[0].mI_Address1;
                            $scope.inst_address2 = $scope.instname[0].mI_Address2;
                            $scope.inst_address3 = $scope.instname[0].mI_Address3;
                            $scope.inst_pincode = $scope.instname[0].mI_Pincode;

                            $scope.colspan = 6 + $scope.examid.length;

                        } else {
                            swal("No Records Found");
                        }
                    });
            }
        };

        $scope.exportToExcel = function (tableIds) {
            var excelname = 'Cumulative Report - ' + $scope.yearname+'.xls';
            var exportHref = Excel.tableToExcel(tableIds, 'Cumulative Report');
            $timeout(function () {
                var a = document.createElement('a');
                a.href = exportHref;
                a.download = excelname;
                document.body.appendChild(a);
                a.click();
                a.remove();
            }, 100);
        };


        //$scope.exportToExcel = function (tableIds) {
        //    var exportHref = Excel.tableToExcel(tableIds, 'sheet name');
        //    $timeout(function () { location.href = exportHref; }, 100);
        //};      
  

        $scope.DeleteRecord = {};
        $scope.EditRecord = {};
        $scope.obj = {};
        $scope.studentlist = false;
        $scope.currentPage = 1;
        $scope.itemsPerPage = 5;       

       
        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        }; 

        $scope.sortKey = 'amsT_FirstName';

        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };  

        $scope.printToCart = function () {
            var innerContents = document.getElementById("Baldwin").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/CumulativeReportBBPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };

        $scope.cancel = function () {
            $state.reload();
        };

        $scope.toggleAll = function () {
            var toggleStatus = $scope.all;
            angular.forEach($scope.subjectlt1, function (itm) {
                itm.xyz = toggleStatus;
            });
        };

        $scope.optionToggled = function (chk_box) {
            $scope.all = $scope.subjectlt1.every(function (itm) { return itm.xyz; })
        };
    }

})();