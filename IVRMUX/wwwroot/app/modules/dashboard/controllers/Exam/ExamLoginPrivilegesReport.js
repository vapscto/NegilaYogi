(function () {
    'use strict';
    angular.module('app').controller('ExamLoginPrivilegesReportController', ExamLoginPrivilegesReportController)

    ExamLoginPrivilegesReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$timeout','Excel']
    function ExamLoginPrivilegesReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $timeout,Excel) {

        $scope.reportdetails = true;
        $scope.classlist = [];
        $scope.classwise_sectionlist = [];
        $scope.classwise_sectionwise_emplist = [];
        $scope.classwise_sectionwise_emp_subjectlist = [];
        $scope.finalarray = [];

        var paginationformasters;
        var copty;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings !== null && ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_ReportPagination;
            copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
        } else {
            paginationformasters = 10;
            copty = 0;
        }

        $scope.itemsPerPage = paginationformasters;
        $scope.itemsPerPage_model = paginationformasters;
        $scope.coptyright = copty;

        var logopath = "";
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings !== null && admfigsettings.length > 0) {
            logopath = admfigsettings[0].asC_Logo_Path;
        } else {
            logopath = "";
        }

        $scope.imgname = logopath;  

        $scope.BindData = function () {
            apiService.getDATA("ExamLoginPrivilegesReport/getdetails").then(function (promise) {
                $scope.qualification_type = 'all';
                $scope.emp_checked = 0;
                $scope.acdlist = promise.acdlist;
            });
        };

        $scope.onselectAcdYear = function (ASMAY_Id) {
            $scope.EMCA_Id = "";
            $scope.ASMCL_Id = "";
            $scope.ASMS_Id = "";
            $scope.main_list = [];
            $scope.reportdetails = true;
            $scope.obj.HRME_Id = "";

            $scope.finalarray = [];
            $scope.classlist = [];
            $scope.classwise_sectionlist = [];
            $scope.classwise_sectionwise_emplist = [];
            $scope.classwise_sectionwise_emp_subjectlist = [];

            var data = {
                "ASMAY_Id": ASMAY_Id
            };
            apiService.create("ExamLoginPrivilegesReport/onselectAcdYear", data).then(function (promise) {
                $scope.catlist = promise.catlist;
            });
        };

        $scope.onchangecategory = function () {
            $scope.ASMCL_Id = "";
            $scope.ASMS_Id = "";
            $scope.main_list = [];
            $scope.reportdetails = true;
            $scope.obj.HRME_Id = "";

            $scope.finalarray = [];
            $scope.classlist = [];
            $scope.classwise_sectionlist = [];
            $scope.classwise_sectionwise_emplist = [];
            $scope.classwise_sectionwise_emp_subjectlist = [];

            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "EMCA_Id": $scope.EMCA_Id,
                "report_type": $scope.qualification_type
            };
            apiService.create("ExamLoginPrivilegesReport/onchangecategory", data).then(function (promise) {
                if (promise !== null) {
                    $scope.ctlist = promise.ctlist;
                    if ($scope.qualification_type === "all") {
                        $scope.stafflist = promise.stafflist;
                    }
                }
            });
        };

        $scope.onselectclass = function (ASMCL_Id, ASMAY_Id) {
            $scope.ASMS_Id = "";
            $scope.main_list = [];
            $scope.reportdetails = true;
            $scope.obj.HRME_Id = "";

            $scope.finalarray = [];
            $scope.classlist = [];
            $scope.classwise_sectionlist = [];
            $scope.classwise_sectionwise_emplist = [];
            $scope.classwise_sectionwise_emp_subjectlist = [];

            var data = {
                "ASMCL_Id": ASMCL_Id,
                "ASMAY_Id": ASMAY_Id,
                "EMCA_Id": $scope.EMCA_Id,
                "report_type": $scope.qualification_type
            };
            apiService.create("ExamLoginPrivilegesReport/onselectclass", data).then(function (promise) {
                $scope.seclist = promise.seclist;
            });
        };

        $scope.onchangesection = function (ASMCL_Id, ASMAY_Id) {
            $scope.main_list = [];
            $scope.reportdetails = true;
            $scope.obj.HRME_Id = "";

            $scope.finalarray = [];
            $scope.classlist = [];
            $scope.classwise_sectionlist = [];
            $scope.classwise_sectionwise_emplist = [];
            $scope.classwise_sectionwise_emp_subjectlist = [];

            var data = {
                "ASMCL_Id": $scope.ASMCL_Id,
                "ASMAY_Id": $scope.ASMAY_Id,
                "EMCA_Id": $scope.EMCA_Id,
                "report_type": $scope.qualification_type,
                "ASMS_Id": $scope.ASMS_Id

            };
            apiService.create("ExamLoginPrivilegesReport/onchangesection", data).then(function (promise) {
                $scope.stafflist = promise.stafflist;
            });
        };

        $scope.selectemp1 = function () {
            $scope.reportdetails = true;
            $scope.main_list = [];
        };

        $scope.obj = {};
        $scope.submitted = false;
        $scope.onreport = function (obj) {
            $scope.submitted = true;
            $scope.reportdetails = true;

            $scope.finalarray = [];
            $scope.classlist = [];
            $scope.classwise_sectionlist = [];
            $scope.classwise_sectionwise_emplist = [];
            $scope.classwise_sectionwise_emp_subjectlist = [];

            if ($scope.myForm.$valid) {
                var classid = 0;
                var sectionid = 0;

                if ($scope.qualification_type === 'all') {
                    classid = 0;
                    sectionid = 0;
                } else {
                    classid = $scope.ASMCL_Id;
                    sectionid = $scope.ASMS_Id;
                }
                var hrmeid = 0;
                if ($scope.emp_checked === 1) {
                    hrmeid = obj.HRME_Id.hrmE_Id;
                } else {
                    hrmeid = 0;
                }
                    
                var data = {
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "ASMCL_Id": classid,
                    "ASMS_Id": sectionid,
                    "EMCA_Id": $scope.EMCA_Id,
                    "HRME_Id": hrmeid,
                    "report_type": $scope.qualification_type,
                    "check_type": $scope.emp_checked
                };

                apiService.create("ExamLoginPrivilegesReport/onreport", data).then(function (promise) {
                    $scope.main_list = promise.datareport;
                    if ($scope.main_list !== null && $scope.main_list.length > 0) {

                        $scope.classlist = [];
                        $scope.classwise_sectionlist = [];
                        $scope.classwise_sectionwise_emplist = [];
                        $scope.classwise_sectionwise_emp_subjectlist = [];

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

                        // Taking Distinct Class Section Employee List 

                        angular.forEach($scope.classlist, function (cls) {
                            $scope.classwise_sectionwise_emplist = [];
                            angular.forEach(cls.sectionlist, function (sec) {
                                $scope.classwise_sectionwise_emplist = [];
                                angular.forEach($scope.main_list, function (dd) {
                                    if (cls.ASMCL_Id === dd.ASMCL_Id && sec.ASMS_Id === dd.ASMS_Id) {
                                        if ($scope.classwise_sectionwise_emplist.length === 0) {
                                            $scope.classwise_sectionwise_emplist.push({
                                                ASMCL_ClassName: cls.ASMCL_ClassName, ASMCL_Id: cls.ASMCL_Id, ASMS_Id: dd.ASMS_Id,
                                                ASMC_SectionName: dd.ASMC_SectionName, HRME_Id: dd.HRME_Id , Employeename : dd.HRME_EmployeeFirstName
                                            });
                                        } else if ($scope.classwise_sectionwise_emplist.length > 0) {
                                            var countemployee = 0;
                                            angular.forEach($scope.classwise_sectionwise_emplist, function (emp) {
                                                if (cls.ASMCL_Id === emp.ASMCL_Id && sec.ASMS_Id === emp.ASMS_Id && emp.HRME_Id === dd.HRME_Id) {
                                                    countemployee += 1;
                                                }
                                            });

                                            if (countemployee === 0) {
                                                $scope.classwise_sectionwise_emplist.push({
                                                    ASMCL_ClassName: cls.ASMCL_ClassName, ASMCL_Id: cls.ASMCL_Id, ASMS_Id: dd.ASMS_Id,
                                                    ASMC_SectionName: dd.ASMC_SectionName, HRME_Id: dd.HRME_Id, Employeename: dd.HRME_EmployeeFirstName
                                                });
                                            }
                                        }
                                    }
                                });
                                sec.employeelist = $scope.classwise_sectionwise_emplist;
                            });                            
                        });
                        console.log($scope.classlist);

                        $scope.finalarray = [];

                        angular.forEach($scope.classlist, function (cls) {
                            $scope.classwise_sectionwise_emp_subjectlist = [];
                            angular.forEach(cls.sectionlist, function (sec) {
                                $scope.classwise_sectionwise_emp_subjectlist = [];
                                angular.forEach(sec.employeelist, function (emp) {
                                    $scope.classwise_sectionwise_emp_subjectlist = [];
                                    angular.forEach($scope.main_list, function (dd) {
                                        if (cls.ASMCL_Id === dd.ASMCL_Id && sec.ASMS_Id === dd.ASMS_Id && emp.HRME_Id === dd.HRME_Id) {
                                            $scope.classwise_sectionwise_emp_subjectlist.push({
                                                ASMCL_ClassName: cls.ASMCL_ClassName, ASMCL_Id: cls.ASMCL_Id, ASMS_Id: dd.ASMS_Id,
                                                ASMC_SectionName: dd.ASMC_SectionName, HRME_Id: dd.HRME_Id, Employeename: emp.Employeename,
                                                ISMS_Id: dd.ISMS_Id, ISMS_SubjectName: dd.ISMS_SubjectName
                                            });
                                        }
                                    });

                                    $scope.finalarray.push({
                                        ASMCL_ClassName: cls.ASMCL_ClassName, ASMC_SectionName: sec.ASMC_SectionName, Employeename: emp.Employeename,
                                        subjectlist: $scope.classwise_sectionwise_emp_subjectlist
                                    });
                                });
                            });
                        });

                        console.log($scope.finalarray);
                        $scope.reportdetails = false;

                        angular.forEach($scope.acdlist, function (ye) {
                            if (ye.asmaY_Id === parseInt($scope.ASMAY_Id)) {
                                $scope.reportdetails = "Academic Year : " + ye.asmaY_Year;
                            }
                        });

                        $scope.institutename = "";
                        $scope.instituteaddress = "";

                        $scope.institution = promise.institution;
                        if ($scope.institution !== null && $scope.institution.length > 0) {
                            $scope.institutename = $scope.institution[0].mI_Name;
                            $scope.instituteaddress = $scope.institution[0].mI_Address1;
                        }

                    } else {
                        $scope.reportdetails = true;
                        swal("No Records Found");
                    }
                });
            }
        };

        $scope.interacted = function (field) {

            return $scope.submitted;
        };

        $scope.onselectradio = function () {
            if ($scope.qualification_type === 'all') {
                $scope.class_disable = true;
                $scope.sec_disable = true;
                $scope.ASMCL_Id = '';
                $scope.ASMS_Id = '';
                $scope.submitted = false;
                $scope.myForm.$setPristine();
                $scope.myForm.$setUntouched();
                $scope.search = "";
                $scope.reportdetails = true;
            }
            else {
                $scope.class_disable = false;
                $scope.sec_disable = false;
                $scope.reportdetails = true;
            }
        };

        $scope.cancel = function () {
            $state.reload();
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
            var exportHref = Excel.tableToExcel(tableId, 'sheet name');
            var excelname = "Exam Login Privileges Report.xls";
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