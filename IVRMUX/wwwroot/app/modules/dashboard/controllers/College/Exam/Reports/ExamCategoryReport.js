(function () {
    'use strict';
    angular.module('app').controller('ExamCategoryReportController', ExamCategoryReportController)

    ExamCategoryReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', 'Excel', '$timeout', '$filter']
    function ExamCategoryReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, Excel, $timeout, $filter) {

        $scope.gridflag = false;
        $scope.print_flag = true;
        $scope.excel_flag = true;
        $scope.currentPage = 1;
        //  $scope.itemsPerPage = 10;
        $scope.printdatatable = [];
        $scope.studentAttendanceList = {};

        $scope.userPrivileges = "";
        var pageid = $stateParams.pageId;

        $scope.ddate = {};
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

        $scope.itemsPerPage = paginationformasters;
        $scope.coptyright = copty;

        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings !== null && admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }

        $scope.imgname = logopath;

        $scope.LoadData = function () {
            apiService.get("ExamCategoryReport/getdetails/").then(function (promise) {
                // $scope.all = true;
                $scope.yearDropdown = promise.yearlist;
                $scope.classDropdown = promise.grlist;
            });
        };    

        $scope.onchangeyear = function () {
            $scope.gridflag = false;
            $scope.print_flag = true;
            $scope.excel_flag = true;
            $scope.recivedamtarray1 = [];
            $scope.recivedamtarray2 = [];
            $scope.recivedamtarray3 = [];
        };

        $scope.onchangecategory = function () {
            $scope.gridflag = false;
            $scope.print_flag = true;
            $scope.excel_flag = true;
            $scope.recivedamtarray1 = [];
            $scope.recivedamtarray2 = [];
            $scope.recivedamtarray3 = [];
        };

        $scope.submitted = false;   
        $scope.savetmpldata = function () {
            $scope.printdatatable = [];
            $scope.searchValue = "";
            if (parseInt($scope.type) === 1) {
                $scope.asmcL_Id = 0;
                $scope.asmC_Id = 0;
            }

            var data = {
                "type": $scope.type,
                "ASMAY_Id": $scope.asmaY_Id,
                "EMCA_Id": $scope.emcA_Id
            };

            if ($scope.myForm.$valid) {
                if (parseInt($scope.type) === 1) {
                    if ($scope.asmaY_Id === undefined || $scope.asmaY_Id === null) {
                        swal("Select The Fields !");
                    }
                    else {
                        apiService.create("ExamCategoryReport/getAttendetails", data).then(function (promise) {
                            $scope.date = new Date();
                            if (promise.studentAttendanceList !== null &&  promise.studentAttendanceList.length > 0 ) {
                                $scope.students = promise.studentAttendanceList;
                                $scope.presentCountgrid = $scope.students.length;
                                $scope.masterinstitution = promise.masterinstitution;

                                $scope.recivedamtarray1 = [];
                                $scope.recivedamtarray2 = [];
                                $scope.recivedamtarray3 = [];

                                angular.forEach($scope.students, function (t3) {
                                    var al_cnt = 0;
                                    angular.forEach($scope.recivedamtarray1, function (rt) {
                                        if (parseInt(rt.emcA_Id) === parseInt(t3.emcA_Id)) {
                                            al_cnt += 1;
                                        }
                                    });
                                    if (al_cnt === 0) {
                                        $scope.recivedamtarray1.push({ emcA_Id: t3.emcA_Id, emcA_CategoryName: t3.emcA_CategoryName });
                                    }
                                });
                                angular.forEach($scope.students, function (t3) {
                                    var al_cnt = 0;
                                    angular.forEach($scope.recivedamtarray2, function (rt) {
                                        if (parseInt(rt.emG_Id) === parseInt(t3.emG_Id)) {
                                            al_cnt += 1;
                                        }
                                    });
                                    if (al_cnt === 0) {
                                        $scope.recivedamtarray2.push({ emG_Id: t3.emG_Id, emG_GroupName: t3.emG_GroupName });
                                    }
                                });
                                angular.forEach($scope.students, function (t3) {
                                    var al_cnt = 0;
                                    angular.forEach($scope.recivedamtarray3, function (rt) {
                                        if (parseInt(rt.ismS_Id) === parseInt(t3.ismS_Id)) {
                                            al_cnt += 1;
                                        }
                                    });
                                    if (al_cnt === 0) {
                                        $scope.recivedamtarray3.push({ ismS_Id: t3.ismS_Id, ismS_SubjectName: t3.ismS_SubjectName });
                                    }
                                });
                                var details_list = [];

                                angular.forEach($scope.recivedamtarray1, function (cat) {
                                    var alrdy_cnt = 0;
                                    angular.forEach(details_list, function (final_t) {
                                        if (parseInt(final_t.emcA_Id) === parseInt(cat.emcA_Id)) {
                                            alrdy_cnt += 1;
                                        }
                                    });

                                    if (alrdy_cnt === 0) {
                                        var cat_grp_list = [];
                                        var temp_cat_grp_list = []
                                        angular.forEach($scope.students, function (t1) {
                                            if (parseInt(t1.emcA_Id) === parseInt(cat.emcA_Id)) {
                                                temp_cat_grp_list.push(t1);
                                            }
                                        });
                                        angular.forEach($scope.recivedamtarray2, function (grp) {
                                            var grp_subjs = [];
                                            var count = 0;
                                            angular.forEach(temp_cat_grp_list, function (t2) {
                                                if (parseInt(t2.emG_Id) === parseInt(grp.emG_Id)) {
                                                    count += 1;
                                                    grp_subjs.push(t2);
                                                }
                                            });
                                            if (count > 0) {
                                                cat_grp_list.push({ emG_Id: grp.emG_Id, emG_GroupName: grp.emG_GroupName, grp_subjs: grp_subjs });
                                            }
                                        });
                                        var rowspan = temp_cat_grp_list.length;
                                        details_list.push({ emcA_Id: cat.emcA_Id, emcA_CategoryName: cat.emcA_CategoryName, cat_grp_list: cat_grp_list, rowspan: rowspan });
                                    }
                                });
                                console.log(details_list);
                                $scope.final_details_list = details_list;
                                $scope.gridflag = true;
                                $scope.print_flag = false;
                                $scope.excel_flag = false;

                                $scope.institutename = $scope.masterinstitution[0].mI_Name;
                                $scope.instituteaddress = $scope.masterinstitution[0].mI_Address1;

                                angular.forEach($scope.yearDropdown, function (dd) {
                                    if (dd.asmaY_Id === parseInt($scope.asmaY_Id)) {
                                        $scope.yearname = dd.asmaY_Year;
                                    }
                                });

                                $scope.reportdetails = "Academic Year : " + $scope.yearname;
                            }
                            else {
                                swal("No record Found !");
                                $scope.gridflag = false;
                                $scope.print_flag = true;
                                $scope.excel_flag = true;
                                $scope.recivedamtarray1 = [];
                                $scope.recivedamtarray2 = [];
                                $scope.recivedamtarray3 = [];
                            }
                        });
                    }
                }

                if (parseInt($scope.type) === 2) {
                    if ($scope.asmaY_Id === undefined || $scope.asmaY_Id === null) {
                        swal("Select The Fields !");
                    }
                    else {
                        apiService.create("ExamCategoryReport/getAttendetails", data).then(function (promise) {
                            if (promise.studentAttendanceList !== null && promise.studentAttendanceList.length > 0 ) {
                                $scope.students = promise.studentAttendanceList;
                                $scope.presentCountgrid = $scope.students.length;
                                $scope.masterinstitution = promise.masterinstitution;
                                $scope.recivedamtarray1 = [];
                                $scope.recivedamtarray2 = [];
                                $scope.recivedamtarray3 = [];

                                angular.forEach($scope.students, function (t3) {
                                    var al_cnt = 0;
                                    angular.forEach($scope.recivedamtarray1, function (rt) {
                                        if (parseInt(rt.emcA_Id) === parseInt(t3.emcA_Id)) {
                                            al_cnt += 1;
                                        }
                                    });
                                    if (al_cnt === 0) {
                                        $scope.recivedamtarray1.push({ emcA_Id: t3.emcA_Id, emcA_CategoryName: t3.emcA_CategoryName });
                                    }
                                });

                                angular.forEach($scope.students, function (t3) {
                                    var al_cnt = 0;
                                    angular.forEach($scope.recivedamtarray2, function (rt) {
                                        if (parseInt(rt.emG_Id) === parseInt(t3.emG_Id)) {
                                            al_cnt += 1;
                                        }
                                    });
                                    if (al_cnt === 0) {
                                        $scope.recivedamtarray2.push({ emG_Id: t3.emG_Id, emG_GroupName: t3.emG_GroupName });
                                    }
                                });

                                angular.forEach($scope.students, function (t3) {
                                    var al_cnt = 0;
                                    angular.forEach($scope.recivedamtarray3, function (rt) {
                                        if (parseInt(rt.ismS_Id) === parseInt(t3.ismS_Id)) {
                                            al_cnt += 1;
                                        }
                                    });
                                    if (al_cnt === 0) {
                                        $scope.recivedamtarray3.push({ ismS_Id: t3.ismS_Id, ismS_SubjectName: t3.ismS_SubjectName });
                                    }
                                });
                                var details_list = [];
                                angular.forEach($scope.recivedamtarray1, function (cat) {
                                    var alrdy_cnt = 0;
                                    angular.forEach(details_list, function (final_t) {
                                        if (parseInt(final_t.emcA_Id) === parseInt(cat.emcA_Id)) {
                                            alrdy_cnt += 1;
                                        }
                                    });

                                    if (alrdy_cnt === 0) {
                                        var cat_grp_list = [];
                                        var temp_cat_grp_list = [];
                                        angular.forEach($scope.students, function (t1) {
                                            if (parseInt(t1.emcA_Id) === parseInt(cat.emcA_Id)) {
                                                temp_cat_grp_list.push(t1);
                                            }
                                        });
                                        angular.forEach($scope.recivedamtarray2, function (grp) {
                                            var grp_subjs = [];
                                            var count = 0;
                                            angular.forEach(temp_cat_grp_list, function (t2) {
                                                if (parseInt(t2.emG_Id) === parseInt(grp.emG_Id)) {
                                                    count += 1;
                                                    grp_subjs.push(t2);
                                                }
                                            });
                                            if (count > 0) {
                                                cat_grp_list.push({ emG_Id: grp.emG_Id, emG_GroupName: grp.emG_GroupName, grp_subjs: grp_subjs });
                                            }
                                        });
                                        var rowspan = temp_cat_grp_list.length;
                                        details_list.push({ emcA_Id: cat.emcA_Id, emcA_CategoryName: cat.emcA_CategoryName, cat_grp_list: cat_grp_list, rowspan: rowspan });
                                    }
                                });
                                console.log(details_list);
                                $scope.final_details_list = details_list;
                                $scope.gridflag = true;
                                $scope.print_flag = false;
                                $scope.excel_flag = false;

                                angular.forEach($scope.yearDropdown, function (dd) {
                                    if (dd.asmaY_Id === parseInt($scope.asmaY_Id)) {
                                        $scope.yearname = dd.asmaY_Year;
                                    }
                                });

                                angular.forEach($scope.classDropdown, function (dd) {
                                    if (dd.emcA_Id === parseInt($scope.emcA_Id)) {
                                        $scope.category = dd.emcA_CategoryName;
                                    }
                                });

                                $scope.reportdetails = "Academic Year : " + $scope.yearname + "  Category : " + $scope.category ; 

                                $scope.institutename = $scope.masterinstitution[0].mI_Name;
                                $scope.instituteaddress = $scope.masterinstitution[0].MI_Address1;
                            }
                            else {
                                swal("No record Found !");
                                $scope.gridflag = false;
                                $scope.print_flag = true;
                                $scope.excel_flag = true;
                                $scope.recivedamtarray1 = [];
                                $scope.recivedamtarray2 = [];
                                $scope.recivedamtarray3 = [];
                            }
                        });
                    }
                }
            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.sortBy = function (propertyName) {
            $scope.reverse = ($scope.propertyName === propertyName) ? !$scope.reverse : false;
            $scope.propertyName = propertyName;
        };

        $scope.getDataByType = function (type) {
            if (parseInt(type) === 1) {
                $scope.classname = false;
                $scope.sectionname = false;
                $scope.gridflag = false;
                $scope.asmaY_Id = "";
                $scope.submitted = false;
                $scope.myForm.$setPristine();
                $scope.myForm.$setUntouched();
            }
            if (parseInt(type) === 2) {
                $scope.classname = true;
                $scope.gridflag = false;
                $scope.asmcL_Id = "";
                $scope.asmC_Id = "";
                $scope.submitted = false;
                $scope.myForm.$setPristine();
                $scope.myForm.$setUntouched();
            }
        };       

        $scope.searchValue = '';
        $scope.filterValue = function (obj) {
            return (angular.lowercase(obj.namme)).indexOf($scope.searchValue) >= 0 ||
                (angular.lowercase(obj.AMST_AdmNo)).indexOf($scope.searchValue) >= 0 ||
                (angular.lowercase(obj.AMST_RegistrationNo)).indexOf($scope.searchValue) >= 0 ||
                (angular.lowercase(obj.ASMCL_ClassName)).indexOf($scope.searchValue) >= 0 ||
                (angular.lowercase(obj.ASMC_SectionName)).indexOf($scope.searchValue) >= 0 ||
                (JSON.stringify(obj.classes)).indexOf($scope.searchValue) >= 0
        };

        $scope.toggleAll = function () {
            var toggleStatus = $scope.all2;
            $scope.printdatatable = [];
            angular.forEach($scope.final_details_list, function (itm) {
                itm.selected = toggleStatus;
                if (itm.selected === true) {
                    $scope.printdatatable.push(itm);
                }
            });
        };

        $scope.optionToggled = function (SelectedStudentRecord, index) {
            $scope.all2 = $scope.final_details_list.every(function (itm) { return itm.selected; });
            $scope.printdatatable = [];
            angular.forEach($scope.final_details_list, function (itm) {
                if (itm.selected === true) {
                    $scope.printdatatable.push(itm);
                }
            });
        };

        $scope.printData = function () {
            var innerContents = document.getElementById("printSectionId").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
                '<link type="text/css" media="print" href="css/print/preadmission/AdmissionReports.css" rel="stylesheet" />' +
                '</head> <body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>'
            );
            popupWinindow.document.close();
        };

        $scope.exportToExcel = function (tableId) {
            var exportHref = Excel.tableToExcel(tableId, 'sheet name');
            var excelname = "Exam Category Group Wise Subject Wise Report.xlxs";
            $timeout(function () {
                var a = document.createElement('a');
                a.href = exportHref;
                a.download = excelname;
                document.body.appendChild(a);
                a.click();
                a.remove();
            }, 100);
        };

        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };

        $scope.cancel = function () {
            $state.reload();
        };
    }
})();
