(function () {
    'use strict';
    angular.module('app').controller('GroupwiseSubListReportController', GroupwiseSubListReportController)

    GroupwiseSubListReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout']
    function GroupwiseSubListReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout) {

        $scope.reportname = true;

        var paginationformasters;
        var copty;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings !== null && ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_ReportPagination;
            copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
        }

        $scope.itemsPerPage = paginationformasters;
        $scope.coptyright = copty;

        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings !== null && admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }

        $scope.imgname = logopath;

        $scope.BindData = function () {
            apiService.getDATA("GroupwiseSubListReport/getdetails").then(function (promise) {
                $scope.qualification_type = 'all';
                $scope.group = promise.group;
                $scope.examlist = promise.examlist;
                $scope.yearlist = promise.getyear;
            });
        };

        $scope.onchangeyear = function () {
            $scope.main_list = [];
            $scope.grouplist = [];
            $scope.subjectlist_temp = [];
            $scope.subjectname = [];

            $scope.reportname = true;
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id
            };

            apiService.create("GroupwiseSubListReport/onchangeyear", data).then(function (promise) {
                if (promise !== null) {
                    $scope.groupname = promise.groupname;
                    $scope.examlist = promise.examlist;
                }
            });
        };


        $scope.onchangeexam = function () {
            $scope.main_list = [];
            $scope.grouplist = [];
            $scope.subjectlist_temp = [];
            $scope.subjectname = [];
            $scope.reportname = true;
        };

        $scope.onchangegroup = function () {
            $scope.main_list = [];
            $scope.grouplist = [];
            $scope.subjectlist_temp = [];
            $scope.subjectname = [];
            $scope.reportname = true;
        };


        $scope.onselectradio = function () {
            if ($scope.qualification_type === 'all') {
                $scope.group_disable = true;
                $scope.EMG_Id = "";
                $scope.submitted = false;
                $scope.myForm.$setPristine();
                $scope.myForm.$setUntouched();
                $scope.search = "";
            }
            else {
                $scope.group_disable = false;
            }
            $scope.main_list = [];
            $scope.grouplist = [];
            $scope.subjectlist_temp = [];
            $scope.subjectname = [];
            $scope.reportname = true;
        };


        $scope.subrow = 2;
        $scope.submitted = false;
        $scope.onreport = function () {

            $scope.submitted = true;
            $scope.main_list = [];
            $scope.grouplist = [];
            $scope.subjectlist_temp = [];
            $scope.subjectname = [];
            $scope.reportname = true;

            if ($scope.myForm.$valid) {
                var emgid = 0;
                var asmayid = 0;
                var emeid = 0;

                if ($scope.qualification_type === 'all') {
                    emgid = 0;
                } else {
                    emgid = $scope.EMG_Id;
                }

                if ($scope.masteryearly === "master") {
                    asmayid = 0;
                } else {
                    asmayid = $scope.ASMAY_Id;
                }

                if ($scope.examwiseorwithout === "withoutexam") {
                    emeid = 0;
                } else {
                    emeid = $scope.EME_Id;
                }

                var data = {
                    "ASMAY_Id": asmayid,
                    "EMG_Id": emgid,
                    "EME_Id": emeid,
                    "report_type": $scope.qualification_type,
                    "examwiseorwithout": $scope.examwiseorwithout,
                    "masteryearly": $scope.masteryearly
                };

                apiService.create("GroupwiseSubListReport/onreport", data).then(function (promise) {

                    if ($scope.masteryearly === 'master') {

                        $scope.subjectname = promise.subjectname;

                        if ($scope.subjectname !== null && $scope.subjectname.length > 0) {

                            $scope.grouplist = [];
                            $scope.subjectlist_temp = [];
                            $scope.reportname = false;

                            angular.forEach($scope.subjectname, function (dd) {
                                if ($scope.grouplist.length === 0) {
                                    $scope.grouplist.push({
                                        EMG_Id: dd.EMG_Id, EMG_GroupName: dd.EMG_GroupName, EMG_MaxAplSubjects: dd.EMG_MaxAplSubjects,
                                        EMG_MinAplSubjects: dd.EMG_MinAplSubjects, EMG_BestOff: dd.EMG_BestOff, EMG_ElectiveFlg: dd.EMG_ElectiveFlg,
                                        EMG_TotSubjects: dd.EMG_TotSubjects
                                    });
                                } else if ($scope.grouplist.length > 0) {
                                    var groupcount = 0;
                                    angular.forEach($scope.grouplist, function (d) {
                                        if (d.EMG_Id === dd.EMG_Id) {
                                            groupcount += 1;
                                        }
                                    });
                                    if (groupcount === 0) {
                                        $scope.grouplist.push({
                                            EMG_Id: dd.EMG_Id, EMG_GroupName: dd.EMG_GroupName, EMG_MaxAplSubjects: dd.EMG_MaxAplSubjects,
                                            EMG_MinAplSubjects: dd.EMG_MinAplSubjects, EMG_BestOff: dd.EMG_BestOff, EMG_ElectiveFlg: dd.EMG_ElectiveFlg,
                                            EMG_TotSubjects: dd.EMG_TotSubjects
                                        });
                                    }
                                }
                            });

                            console.log($scope.grouplist);

                            $scope.subjectlist_temp = [];

                            angular.forEach($scope.grouplist, function (gr) {
                                $scope.subjectlist_temp = [];
                                angular.forEach($scope.subjectname, function (dd) {
                                    if (gr.EMG_Id === dd.EMG_Id) {
                                        $scope.subjectlist_temp.push({ EMG_Id: gr.EMG_Id, ISMS_Id: dd.ISMS_Id, ISMS_SubjectName: dd.ISMS_SubjectName });
                                    }
                                });
                                gr.subjectlist = $scope.subjectlist_temp;
                            });
                            console.log($scope.grouplist);
                        } else {
                            swal("No Reocrd Found");
                        }
                    }


                    if ($scope.masteryearly === 'yearly' && $scope.examwiseorwithout === 'withoutexam') {

                        $scope.subjectname = promise.subjectname;

                        if ($scope.subjectname !== null && $scope.subjectname.length > 0) {

                            $scope.grouplist = [];
                            $scope.subjectlist_temp = [];
                            $scope.reportname = false;

                            angular.forEach($scope.subjectname, function (dd) {
                                if ($scope.grouplist.length === 0) {
                                    $scope.grouplist.push({
                                        EMG_Id: dd.EMG_Id, EMG_GroupName: dd.EMG_GroupName, EMG_MaxAplSubjects: dd.EMG_MaxAplSubjects,
                                        EMG_MinAplSubjects: dd.EMG_MinAplSubjects, EMG_BestOff: dd.EMG_BestOff, EMG_ElectiveFlg: dd.EMG_ElectiveFlg,
                                        EMG_TotSubjects: dd.EMG_TotSubjects
                                    });
                                } else if ($scope.grouplist.length > 0) {
                                    var groupcount = 0;
                                    angular.forEach($scope.grouplist, function (d) {
                                        if (d.EMG_Id === dd.EMG_Id) {
                                            groupcount += 1;
                                        }
                                    });
                                    if (groupcount === 0) {
                                        $scope.grouplist.push({
                                            EMG_Id: dd.EMG_Id, EMG_GroupName: dd.EMG_GroupName, EMG_MaxAplSubjects: dd.EMG_MaxAplSubjects,
                                            EMG_MinAplSubjects: dd.EMG_MinAplSubjects, EMG_BestOff: dd.EMG_BestOff, EMG_ElectiveFlg: dd.EMG_ElectiveFlg,
                                            EMG_TotSubjects: dd.EMG_TotSubjects
                                        });
                                    }
                                }
                            });

                            console.log($scope.grouplist);

                            $scope.subjectlist_temp = [];

                            angular.forEach($scope.grouplist, function (gr) {
                                $scope.subjectlist_temp = [];
                                angular.forEach($scope.subjectname, function (dd) {
                                    if (gr.EMG_Id === dd.EMG_Id) {
                                        $scope.subjectlist_temp.push({ EMG_Id: gr.EMG_Id, ISMS_Id: dd.ISMS_Id, ISMS_SubjectName: dd.ISMS_SubjectName });
                                    }
                                });
                                gr.subjectlist = $scope.subjectlist_temp;
                            });
                            console.log($scope.grouplist);
                        } else {
                            swal("No Reocrd Found");
                        }
                    }

                    if ($scope.masteryearly === 'yearly' && $scope.examwiseorwithout !== 'withoutexam') {

                        $scope.subjectname = promise.subjectname;

                        if ($scope.subjectname !== null && $scope.subjectname.length > 0) {

                            $scope.grouplist = [];
                            $scope.subjectlist_temp = [];
                            $scope.reportname = false;

                            angular.forEach($scope.subjectname, function (dd) {
                                if ($scope.grouplist.length === 0) {
                                    $scope.grouplist.push({
                                        EMG_Id: dd.EMG_Id, EMG_GroupName: dd.EMG_GroupName, EMG_MaxAplSubjects: dd.EMG_MaxAplSubjects,
                                        EMG_MinAplSubjects: dd.EMG_MinAplSubjects, EMG_BestOff: dd.EMG_BestOff, EMG_ElectiveFlg: dd.EMG_ElectiveFlg,
                                        EMG_TotSubjects: dd.EMG_TotSubjects
                                    });
                                } else if ($scope.grouplist.length > 0) {
                                    var groupcount = 0;
                                    angular.forEach($scope.grouplist, function (d) {
                                        if (d.EMG_Id === dd.EMG_Id) {
                                            groupcount += 1;
                                        }
                                    });
                                    if (groupcount === 0) {
                                        $scope.grouplist.push({
                                            EMG_Id: dd.EMG_Id, EMG_GroupName: dd.EMG_GroupName, EMG_MaxAplSubjects: dd.EMG_MaxAplSubjects,
                                            EMG_MinAplSubjects: dd.EMG_MinAplSubjects, EMG_BestOff: dd.EMG_BestOff, EMG_ElectiveFlg: dd.EMG_ElectiveFlg,
                                            EMG_TotSubjects: dd.EMG_TotSubjects
                                        });
                                    }
                                }
                            });

                            console.log($scope.grouplist);

                            $scope.subjectlist_temp = [];

                            angular.forEach($scope.grouplist, function (gr) {
                                $scope.subjectlist_temp = [];
                                angular.forEach($scope.subjectname, function (dd) {
                                    if (gr.EMG_Id === dd.EMG_Id) {
                                        $scope.subjectlist_temp.push({
                                            EMG_Id: gr.EMG_Id, ISMS_Id: dd.ISMS_Id, ISMS_SubjectName: dd.ISMS_SubjectName,
                                            EYCES_AplResultFlg: dd.EYCES_AplResultFlg
                                        });
                                    }
                                });
                                gr.subjectlist = $scope.subjectlist_temp;
                            });
                            console.log($scope.grouplist);
                        } else {
                            swal("No Reocrd Found");
                        }
                    }

                    $scope.reportdetails = "";

                    angular.forEach($scope.yearlist, function (yr) {
                        if (yr.asmaY_Id === parseInt($scope.ASMAY_Id)) {
                            $scope.reportdetails = "Academic Year : " + yr.asmaY_Year;
                        }
                    });

                    angular.forEach($scope.examlist, function (ex) {
                        if (ex.emE_Id === parseInt($scope.EME_Id)) {
                            $scope.reportdetails = $scope.reportdetails + " " + "Exam : " + ex.emE_ExamName;
                        }
                    });

                    $scope.masterinstitution = promise.institution;

                    $scope.institutename = $scope.masterinstitution[0].mI_Name;
                    $scope.instituteaddress = $scope.masterinstitution[0].mI_Address1;
                });
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
            var data = "";

            if ($scope.masteryearly === 'master') {
                data = "printSectionIdmaster";
            } else if ($scope.masteryearly === 'yearly' && $scope.examwiseorwithout === 'withoutexam') {
                data = "printSectionIdmaster";
            }
            else if ($scope.masteryearly === 'yearly' && $scope.examwiseorwithout !== 'withoutexam') {
                data = "printSectionIdyearly";
            }

            innerContents = document.getElementById(data).innerHTML;
            popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/CumulativeReportPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };

        $scope.exportToExcel = function (tableId) {
            var exportHref = "";
            var data = "";

            var excelname = "";


            if ($scope.masteryearly === 'master') {
                data = "#printSectionIdecelmaster";
                excelname = 'Master Group With Subject List.xls';
            } else if ($scope.masteryearly === 'yearly' && $scope.examwiseorwithout === 'withoutexam') {
                data = "#printSectionIdecelmaster";
                excelname = 'Master Group With Subject List.xls';
            } else if ($scope.masteryearly === 'yearly' && $scope.examwiseorwithout !== 'withoutexam') {
                data = "#printSectionIdecelyearly";
                excelname = 'Master Group With Subject List.xls';
            }

            exportHref = Excel.tableToExcel(data, 'sheet name');
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