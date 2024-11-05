(function () {
    'use strict';
    angular.module('app').controller('PercentagewiseDetailsReportController', PercentagewiseDetailsReportController)
    PercentagewiseDetailsReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout']
    function PercentagewiseDetailsReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout) {

        var logopath = "";
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings !== null && admfigsettings.length > 0) {
            logopath = admfigsettings[0].asC_Logo_Path;
        } else {
            logopath = "";
        }

        $scope.imgname = logopath;

        $scope.percentagearray = [
            { id: '1', value: '80-100' },
            { id: '2', value: '60-80' },
            { id: '3', value: '40-60' },
            { id: '4', value: '30-40' },
            { id: '5', value: 'Less Than 30' }
        ];


        $scope.reportdetails = true;
        $scope.main_list = [];
        $scope.BindData = function () {
            apiService.getDATA("PercentagewiseDetailsReport/getdetails").then(function (promise) {
                $scope.qualification_type = 'all';
                $scope.acdlist = promise.acdlist;
            });
        };

        $scope.onselectAcdYear = function (ASMAY_Id) {
            $scope.reportdetails = true;
            $scope.classlist = [];
            $scope.classwise_sectionlist = [];
            $scope.classwise_sectionwise_emplist = [];
            $scope.classwise_sectionwise_emp_subjectlist = [];
            $scope.main_list = [];
            var data = {
                "ASMAY_Id": ASMAY_Id
            };

            apiService.create("PercentagewiseDetailsReport/onselectAcdYear", data).then(function (promise) {
                $scope.catlist = promise.catlist;
            });
        };

        $scope.onselectCategory = function (ASMAY_Id, EMCA_Id) {
            $scope.reportdetails = true;
            $scope.classlist = [];
            $scope.classwise_sectionlist = [];
            $scope.classwise_sectionwise_emplist = [];
            $scope.classwise_sectionwise_emp_subjectlist = [];
            $scope.main_list = [];
            var data = {
                "ASMAY_Id": ASMAY_Id,
                "EMCA_Id": EMCA_Id
            };

            apiService.create("PercentagewiseDetailsReport/onselectCategory", data).then(function (promise) {
                $scope.ctlist = promise.ctlist;
                $scope.examlist = promise.examlist;
            });
        };

        $scope.onselectclass = function (ASMCL_Id, ASMAY_Id, EMCA_Id) {
            $scope.reportdetails = true;
            $scope.classlist = [];
            $scope.classwise_sectionlist = [];
            $scope.classwise_sectionwise_emplist = [];
            $scope.classwise_sectionwise_emp_subjectlist = [];
            $scope.main_list = [];
            var data = {
                "ASMCL_Id": ASMCL_Id,
                "ASMAY_Id": ASMAY_Id,
                "EMCA_Id": EMCA_Id
            };

            apiService.create("PercentagewiseDetailsReport/onselectclass", data).then(function (promise) {
                $scope.seclist = promise.seclist;
            });
        };

        $scope.onselectSection = function () {
            $scope.reportdetails = true;
            $scope.classlist = [];
            $scope.classwise_sectionlist = [];
            $scope.classwise_sectionwise_emplist = [];
            $scope.classwise_sectionwise_emp_subjectlist = [];
            $scope.main_list = [];
            var data = {
                "ASMS_Id": $scope.ASMS_Id,
                "ASMCL_Id": $scope.ASMCL_Id,
                "ASMAY_Id": $scope.ASMAY_Id
            };

            apiService.create("PercentagewiseDetailsReport/onselectSection", data).then(function (promise) {
                $scope.examlist = promise.examlist;
                $scope.studentlist = promise.studentlist;
            });
        };


        $scope.onselectradio = function () {
            $scope.classlist = [];
            $scope.classwise_sectionlist = [];
            $scope.classwise_sectionwise_emplist = [];
            $scope.classwise_sectionwise_emp_subjectlist = [];
            $scope.main_list = [];
            $scope.reportdetails = true;
            if ($scope.qualification_type === 'all') {
                $scope.stud_disable = true;
                $scope.AMST_Id = "";
                $scope.submitted = false;
                $scope.myForm.$setPristine();
                $scope.myForm.$setUntouched();
                $scope.search = "";
            }
            else {
                $scope.stud_disable = false;
            }
        };

        $scope.submitted = false;
        $scope.onreport = function () {
            $scope.submitted = true;
            $scope.reportdetails = true;
            $scope.classlist = [];
            $scope.classwise_sectionlist = [];
            $scope.classwise_sectionwise_emplist = [];
            $scope.classwise_sectionwise_emp_subjectlist = [];
            $scope.main_list = [];
            if ($scope.myForm.$valid) {

                var classid = 0;
                var sectionid = 0;

                if ($scope.qualification_type === 'all') {
                    classid = 0;
                    sectionid = 0;
                } else {
                    sectionid = $scope.ASMS_Id;
                    classid = $scope.ASMCL_Id;
                }

                var data = {
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "ASMCL_Id": classid,
                    "ASMS_Id": sectionid,
                    "EMCA_Id": $scope.EMCA_Id,
                    "EME_Id": $scope.EME_Id,
                    "report_type": $scope.qualification_type,
                    "percent": $scope.percent
                };

                apiService.create("PercentagewiseDetailsReport/onreport", data).then(function (promise) {
                    $scope.main_list = promise.datareport;

                    if ($scope.main_list !== null && $scope.main_list.length > 0) {
                        $scope.classlist = [];
                        $scope.classwise_sectionlist = [];
                        $scope.classwise_sectionwise_emplist = [];
                        $scope.classwise_sectionwise_emp_subjectlist = [];

                        $scope.institutename = "";
                        $scope.instituteaddress = "";

                        $scope.institution = promise.institution;
                        if ($scope.institution !== null && $scope.institution.length > 0) {
                            $scope.institutename = $scope.institution[0].mI_Name;
                            $scope.instituteaddress = $scope.institution[0].MI_Address1;
                        }


                        // Taking Disitnct Class From List

                        angular.forEach($scope.main_list, function (cls) {
                            if ($scope.classlist.length === 0) {
                                $scope.classlist.push({ ASMCL_ClassName: cls.ASMCL_ClassName, ASMCL_Id: cls.ASMCL_Id });
                            }
                            else if ($scope.classlist.length > 0) {
                                var countclass = 0;
                                angular.forEach($scope.classlist, function (dd) {
                                    if (dd.ASMCL_Id === cls.ASMCL_Id) {
                                        countclass += 1;
                                    }
                                });
                                if (countclass === 0) {
                                    $scope.classlist.push({ ASMCL_ClassName: cls.ASMCL_ClassName, ASMCL_Id: cls.ASMCL_Id });
                                }
                            }
                        });

                        console.log($scope.classlist);

                        // Taking Disitnct Class Wise Section From List

                        angular.forEach($scope.classlist, function (cls) {
                            $scope.classwise_sectionlist = [];
                            angular.forEach($scope.main_list, function (dd) {
                                if (cls.ASMCL_Id === dd.ASMCL_Id) {
                                    if ($scope.classwise_sectionlist.length === 0) {
                                        $scope.classwise_sectionlist.push({
                                            ASMCL_ClassName: cls.ASMCL_ClassName, ASMCL_Id: cls.ASMCL_Id, ASMS_Id: dd.ASMS_Id,
                                            ASMC_SectionName: dd.ASMC_SectionName
                                        });
                                    } else if ($scope.classwise_sectionlist.length > 0) {
                                        var countsection = 0;
                                        angular.forEach($scope.classwise_sectionlist, function (sec) {
                                            if (sec.ASMCL_Id === cls.ASMCL_Id && sec.ASMS_Id === dd.ASMS_Id) {
                                                countsection += 1;
                                            }
                                        });
                                        if (countsection === 0) {
                                            $scope.classwise_sectionlist.push({
                                                ASMCL_ClassName: cls.ASMCL_ClassName, ASMCL_Id: cls.ASMCL_Id, ASMS_Id: dd.ASMS_Id,
                                                ASMC_SectionName: dd.ASMC_SectionName
                                            });
                                        }
                                    }
                                }
                            });
                            cls.sectionlist = $scope.classwise_sectionlist;
                        });

                        console.log($scope.classlist);

                        $scope.finalarray = [];

                        angular.forEach($scope.classlist, function (cls) {
                            $scope.classwise_sectionwise_emp_subjectlist = [];
                            angular.forEach(cls.sectionlist, function (sec) {
                                $scope.classwise_sectionwise_emp_subjectlist = [];
                                $scope.classwise_sectionwise_emp_subjectlist = [];
                                angular.forEach($scope.main_list, function (dd) {
                                    if (cls.ASMCL_Id === dd.ASMCL_Id && sec.ASMS_Id === dd.ASMS_Id) {
                                        $scope.classwise_sectionwise_emp_subjectlist.push({
                                            ASMCL_ClassName: cls.ASMCL_ClassName, ASMCL_Id: cls.ASMCL_Id, ASMS_Id: dd.ASMS_Id,
                                            ASMC_SectionName: dd.ASMC_SectionName, AMST_Id: dd.AMST_Id, studentname: dd.studentname,
                                            percentage: dd.ESTMP_Percentage, ESTMP_TotalMaxMarks: dd.ESTMP_TotalMaxMarks,
                                            ESTMP_TotalObtMarks: dd.ESTMP_TotalObtMarks, ESTMP_TotalGrade: dd.ESTMP_TotalGrade
                                        });
                                    }
                                });

                                $scope.finalarray.push({
                                    ASMCL_ClassName: cls.ASMCL_ClassName, ASMC_SectionName: sec.ASMC_SectionName,
                                    subjectlist: $scope.classwise_sectionwise_emp_subjectlist
                                });
                            });
                        });

                        console.log($scope.finalarray);
                        $scope.reportdetails = false;

                        $scope.emename = "";

                        angular.forEach($scope.examlist, function (ex) {
                            if (ex.emE_Id === parseInt($scope.EME_Id)) {
                                $scope.emename = "Exam : " + ex.emE_ExamName;
                            }
                        });

                        $scope.yearname = "";
                        angular.forEach($scope.acdlist, function (yr) {
                            if (yr.asmaY_Id === parseInt($scope.ASMAY_Id)) {
                                $scope.yearname = "Academic Year : " + yr.asmaY_Year;
                            }
                        });

                        $scope.pername = "";
                        angular.forEach($scope.percentagearray, function (pe) {
                            if (parseInt(pe.id) === parseInt($scope.percent)) {
                                $scope.pername = "Percentage Range : " + pe.value;
                            }
                        });


                        $scope.classname = "";
                        $scope.sectionname = "";

                        if ($scope.qualification_type !== 'all') {
                            angular.forEach($scope.ctlist, function (cls) {
                                if (cls.asmcL_Id === parseInt($scope.ASMCL_Id)) {
                                    $scope.classname = "Class : " + cls.asmcL_ClassName;
                                }
                            });

                            if (parseInt($scope.ASMS_Id) === 0) {
                                $scope.sectionname = "All";
                            } else {
                                angular.forEach($scope.seclist, function (sec) {
                                    if (sec.asmS_Id === parseInt($scope.ASMS_Id)) {
                                        $scope.sectionname = "Section : " + sec.asmC_SectionName;
                                    }
                                });
                            }
                        }
                        $scope.reportdetailsd = $scope.yearname + ' ' + $scope.classname + ' ' + $scope.sectionname + ' ' + $scope.emename;

                    } else {
                        swal("No Records Found");
                    }

                });
            }
        };

        $scope.printData = function () {
            var innerContents = document.getElementById("printSectionId").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/CumulativeReportPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };

        $scope.exportToExcel = function (tableId) {
            var data = '#printSectionIdecel';
            var exportHref = Excel.tableToExcel(data, 'sheet name');
            var excelname = "Percentage Wise Report.xls";
            $timeout(function () {
                var a = document.createElement('a');
                a.href = exportHref;
                a.download = excelname;
                document.body.appendChild(a);
                a.click();
                a.remove();
            }, 100);
        };



        $scope.cancel = function () {
            $state.reload();

        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

    }

})();