(function () {
    'use strict';
    angular.module('app').controller('SiblingEmpMappingReportController', SiblingEmpMappingReportController)

    SiblingEmpMappingReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout']
    function SiblingEmpMappingReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout) {
        var data1;
        var frommonth;
        var tomonth;

        $scope.ddate = new Date();
        $scope.usrname = localStorage.getItem('username');
        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));

        var paginationformasters;
        var copty;

        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings !== null && ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_ReportPagination;
            copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
        }
        $scope.coptyright = copty;

        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings !== null && admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }
        $scope.imgname = logopath;



        //$scope.report = false;
        $scope.loaddata = function () {
            var pageid = 2;
            apiService.getURI("SiblingEmployeeStudentReport/getdetails", pageid).then(function (promise) {
                $scope.yearDropdown = promise.getyearlist;
            });
        };

        $scope.onclicktcperortemo = function () {
            $scope.gridflag = false;
            $scope.Print_flag = false;
        };

        $scope.onchangeyear = function () {
            $scope.gridflag = false;
            $scope.Print_flag = false;

            angular.forEach($scope.yearDropdown, function (year) {
                if (parseInt($scope.ASMAY_Id) === parseInt(year.asmaY_Id)) {
                    $scope.yearname = year.asmaY_Year;
                }
            });

        };

        $scope.submitted = false;

        $scope.getreport = function () {

            if ($scope.myform.$valid) {
                var data = {
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "reporttype": $scope.siblingemployee
                };

                apiService.create("SiblingEmployeeStudentReport/getreport", data).then(function (promise) {
                    $scope.getreportdetails_temp = promise.getreportdetails;
                    if ($scope.getreportdetails_temp !== null && $scope.getreportdetails_temp.length > 0) {

                        $scope.getreportdetails = promise.getreportdetails;


                        if ($scope.siblingemployee === "sibling") {
                            $scope.gridflag = true;
                            $scope.Print_flag = true;

                            $scope.details = "Siblings Student Report For ";

                            $scope.siblingfirstchild = [];

                            angular.forEach($scope.getreportdetails, function (student) {
                                if ($scope.siblingfirstchild.length === 0) {
                                    $scope.siblingfirstchild.push({
                                        firstamstid: student.firstamstid, FirstStudentName: student.FirstStudentName,
                                        FirstStudentclass: student.FirstStudentclass, FirstStudentsection: student.FirstStudentsection,
                                        FirstStudentAdmNo: student.FirstStudentAdmNo
                                    });
                                } else if ($scope.siblingfirstchild.length > 0) {
                                    var count = 0;
                                    angular.forEach($scope.siblingfirstchild, function (first) {
                                        if (first.firstamstid === student.firstamstid) {
                                            count += 1;
                                        }
                                    });
                                    if (count === 0) {
                                        $scope.siblingfirstchild.push({
                                            firstamstid: student.firstamstid, FirstStudentName: student.FirstStudentName,
                                            FirstStudentclass: student.FirstStudentclass, FirstStudentsection: student.FirstStudentsection,
                                            FirstStudentAdmNo: student.FirstStudentAdmNo
                                        });
                                    }
                                }
                            });

                            $scope.siblingdetails = [];

                            angular.forEach($scope.siblingfirstchild, function (firstchild) {
                                $scope.siblingdetails = [];
                                angular.forEach($scope.getreportdetails, function (students) {
                                    if (firstchild.firstamstid === students.firstamstid) {
                                        $scope.siblingdetails.push({
                                            AMSTS_SiblingsName: students.AMSTS_SiblingsName, SubAdmNo: students.SubAdmNo,
                                            Subclass: students.Subclass, Subsection: students.Subsection, orders: students.orders
                                        });
                                    }
                                });
                                firstchild.studentdetails = $scope.siblingdetails;
                            });

                            console.log($scope.siblingfirstchild);
                        }
                        else if ($scope.siblingemployee === 'parent') {

                            $scope.gridflag = true;
                            $scope.Print_flag = true;
                            $scope.details = "Employee Student Report For ";

                            $scope.employeedetails = [];

                            angular.forEach($scope.getreportdetails, function (emp) {
                                if ($scope.employeedetails.length === 0) {
                                    $scope.employeedetails.push({
                                        EMPLOYEENAME: emp.EMPLOYEENAME, EMPLOYEECODE: emp.EMPLOYEECODE,
                                        HRMD_DepartmentName: emp.HRMD_DepartmentName, HRME_Id: emp.HRME_Id
                                    });
                                } else if ($scope.employeedetails.length > 0) {
                                    var countnew = 0;
                                    angular.forEach($scope.employeedetails, function (emps) {
                                        if (emps.HRME_Id === emp.HRME_Id) {
                                            countnew += 1;
                                        }
                                    });
                                    if (countnew === 0) {
                                        $scope.employeedetails.push({
                                            EMPLOYEENAME: emp.EMPLOYEENAME, EMPLOYEECODE: emp.EMPLOYEECODE,
                                            HRMD_DepartmentName: emp.HRMD_DepartmentName, HRME_Id: emp.HRME_Id
                                        });
                                    }
                                }
                            });

                            $scope.empstudentdetails = [];
                            angular.forEach($scope.employeedetails, function (empnew) {
                                $scope.empstudentdetails = [];
                                angular.forEach($scope.getreportdetails, function (empnews) {
                                    if (empnew.HRME_Id === empnews.HRME_Id) {
                                        $scope.empstudentdetails.push({
                                            ADMNO: empnews.ADMNO, StudentName: empnews.StudentName,
                                            ASMCL_ClassName: empnews.ASMCL_ClassName, ASMC_SectionName: empnews.ASMC_SectionName,
                                            AMSTE_SiblingsOrder: empnews.AMSTE_SiblingsOrder
                                        });
                                    }
                                });
                                empnew.studentdetailsnew = $scope.empstudentdetails;
                            });

                            console.log($scope.employeedetails);
                        }
                        $scope.institutiondetails = promise.getinstitutiondetails;
                        $scope.instname = $scope.institutiondetails[0].mI_Name;
                        $scope.instaddress = $scope.institutiondetails[0].mI_Address1;
                    }
                    else {
                        swal("No Records Found");
                        $scope.gridflag = false;
                        $scope.Print_flag = false;
                    }
                });
            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.Clearid = function () {
            $state.reload();
        };

        $scope.ShowHideup = function () {
            $scope.IsHiddenup = $scope.IsHiddenup ? false : true;
        };

        $scope.ShowHidedown = function () {
            $scope.IsHiddendown = $scope.IsHiddendown ? false : true;
        };

        $scope.printToCart = function () {
            var innerContents = "";
            var popupWinindow = "";
            if ($scope.siblingemployee === 'sibling') {
                innerContents = document.getElementById('printSectionId').innerHTML;
                popupWinindow = window.open('');
                popupWinindow.document.open();
                popupWinindow.document.write('<html><head>' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/JSHS/JSHSTCPdf.css" />' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                    '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
                popupWinindow.document.close();
            } else if ($scope.siblingemployee === 'parent') {
                innerContents = document.getElementById('printSectionIdemp').innerHTML;
                popupWinindow = window.open('');
                popupWinindow.document.open();
                popupWinindow.document.write('<html><head>' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/JSHS/JSHSTCPdf.css" />' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                    '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
                popupWinindow.document.close();
            }

        };

        $scope.exportToExcel = function (tableId) {
            var data = "";
            var exportHref = "";
            var excelname = "";

            if ($scope.siblingemployee === 'sibling') {
                data = "Sibling Student Report";
                excelname = data + '- For Year ' + $scope.yearname + '.xls';
                exportHref = Excel.tableToExcel('#printSectionIdsibling', data);
            } else {
                data = "Employee Student Report";
                excelname = data + '- For Year ' + $scope.yearname + '.xls';
                exportHref = Excel.tableToExcel('#printSectionIdempnew', data);
            }
            $timeout(function () {
                var a = document.createElement('a');
                a.href = exportHref;
                a.download = excelname;
                document.body.appendChild(a);
                a.click();
                a.remove();
            }, 100);


            //$timeout(function () { location.href = exportHref; }, 100);
        };
    }
})();
