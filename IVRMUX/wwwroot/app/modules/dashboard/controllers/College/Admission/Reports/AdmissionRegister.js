(function () {
    'use strict';
    angular
        .module('app')
        .controller('CollegeAdmissionRegisterController', CollegeAdmissionRegisterController)

    CollegeAdmissionRegisterController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout']
    function CollegeAdmissionRegisterController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout) {

        $scope.report = false;
        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length != 0 && ivrmcofigsettings.length != null && ivrmcofigsettings.length != undefined) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        } else {
            paginationformasters = 10;
        }

        $scope.itemsPerPage = paginationformasters;
        $scope.currentPage = 1;

        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length != 0 && admfigsettings.length != null && admfigsettings.length != undefined) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        } else {
            var logopath = "";
        }
        $scope.imgname = logopath;

        $scope.exportToExcel = function (export_id) {

            if ($scope.printstudents !== null && $scope.printstudents.length > 0) {
                var exportHref = Excel.tableToExcel(export_id, 'studentregisterrepor');
                $timeout(function () {
                    location.href = exportHref;
                }, 100);
            }
            else {
                swal("Please Select Records to be Exported");
            }
        }


        $scope.BindData = function () {
            apiService.getDATA("CollegeAdmissionRegister/getdetails").
                then(function (promise) {

                    $scope.acdlist = promise.acdlist;                   
                    $scope.seclist = promise.seclist;
                    $scope.quotalist = promise.quotalist;
                    $scope.checklist = promise.check_list;
                    $scope.categoryDropdown = promise.category_list;

                })
        };

        $scope.onselectAcdYear = function (ASMAY_Id) {
            var data = {
                "ASMAY_Id": ASMAY_Id
            }
            apiService.create("CollegeAdmissionRegister/onselectAcdYear", data).
                then(function (promise) {
                    $scope.courselist = promise.courselist;

                })
        };

        $scope.onselectCourse = function (ASMAY_Id, AMCO_Id) {
            var data = {
                "AMCO_Id": AMCO_Id,
                "ASMAY_Id": ASMAY_Id
            }
            apiService.create("CollegeAdmissionRegister/onselectCourse", data).
                then(function (promise) {
                    $scope.branchlist = promise.branchlist;
                })
        };

        $scope.onselectBranch = function (ASMAY_Id, AMCO_Id, AMB_Id) {
            var data = {

                "ASMAY_Id": ASMAY_Id,
                "AMCO_Id": AMCO_Id,
                "AMB_Id": AMB_Id
            }
            apiService.create("CollegeAdmissionRegister/onselectBranch", data).
                then(function (promise) {
                    $scope.semlist = promise.semlist;
                })
        };


        $scope.all_check = function () {

            var toggleStatus = $scope.detail_checked;
            angular.forEach($scope.checklist, function (itm) {
                itm.ivrm_id = toggleStatus;
            });
        }

        $scope.albumNameArraycolumn = [];
        $scope.columnsTest = [];

        $scope.addColumn2 = function (role1) {
            $scope.detail_checked = $scope.checklist.every(function (itm) { return itm.selected; });

            if (role1.selected == true) {

                $scope.albumNameArraycolumn.push(role1);

                var newCol = { id: role1.ivrM_CLGREG_NAME, checked: true, value: role1.ivrM_CLGREG_PAR }

                $scope.columnsTest.push(newCol);
            }
            else {
                var som = $scope.albumNameArraycolumn.indexOf(role1);
                $scope.columnsTest.splice($scope.albumNameArraycolumn.indexOf(role1), 1);
                $scope.albumNameArraycolumn.splice($scope.albumNameArraycolumn.indexOf(role1), 1);

            }
        };


        $scope.order = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }

        $scope.searchValue = '';

        $scope.students = [];
        $scope.catreport = true;
        $scope.submitted = false;

        $scope.onreport = function () {
            $scope.all = false;

            if ($scope.myForm.$valid) {
                $scope.albumNameArraycolumn = [];
                $scope.columntest1 = [];
                $scope.columntest2 = [];
                $scope.columnsTest = [];

                angular.forEach($scope.checklist, function (role) {
                    if (!!role.ivrm_id) $scope.albumNameArraycolumn.push(role);
                });

                var data = {
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "AMCO_Id": $scope.AMCO_Id,
                    "AMB_Id": $scope.AMB_Id,
                    "AMSE_Id": $scope.AMSE_Id,
                    "ACMS_Id": $scope.ACMS_Id,
                    "TempararyArrayListcoloumn": $scope.albumNameArraycolumn,
                    "gender": $scope.mfa,
                    "ACQ_Id": $scope.ACQ_Id
                };

                

                apiService.create("CollegeAdmissionRegister/onreport", data).
                    then(function (promise) {

                        //$scope.columnsTest = $scope.albumNameArraycolumn;
                        $scope.columntest1 = $scope.albumNameArraycolumn;
                        $scope.columntest2.push({ ivrM_CLGREG_NAME: "SNo", ivrM_CLGREG_PAR: "SNo" });

                        angular.forEach($scope.columntest1, function (ddd) {
                            $scope.columntest2.push(ddd);
                        });

                        $scope.students = promise.studentlist;
                        $scope.presentCountgrid = $scope.students.length;

                        var id = 0;
                        angular.forEach($scope.students, function (dd) {
                            id = id + 1;
                            dd.SNo = id;
                        });

                        angular.forEach($scope.columntest2, function (qwe) {
                            qwe.field = qwe.ivrM_CLGREG_PAR;
                            qwe.title = qwe.ivrM_CLGREG_NAME;
                            qwe.width = 160;

                        });

                        angular.forEach($scope.columntest2, function (wer) {
                            $scope.columnsTest.push(wer);
                        });

                        //kendo start

                        var gridall;

                        $(document).ready(function () {
                            initGridall();
                        });

                        function initGridall() {
                            $('#gridlst').empty();
                            gridall = $("#gridlst").kendoGrid({
                                toolbar: ["excel"],
                                excel: {
                                    fileName: "Student REGISTER REPORT.xlsx",
                                    proxyURL: "",
                                    filterable: true,
                                    allPages: true
                                },
                                //pdf: {
                                //    fileName: "Kendo UI Grid Export.pdf",
                                //    allPages: true
                                //},
                                dataSource: {
                                    data: $scope.students,
                                    pageSize: 10
                                },

                                //excelExport: function (e) {
                                //    var rows = e.workbook.sheets[0].rows;
                                //    rows.unshift({
                                //        cells: [{ value: "My title", background: "#ff0000", colSpan: 2, textAlign: "right" }]
                                //    });
                                //},

                                sortable: true,
                                //pageable: false,
                                pageable: true,
                                groupable: false,
                                filterable: true,
                                columnMenu: true,
                                reorderable: true,
                                resizable: true,
                                columns: $scope.columnsTest,
                                dataBound: function () {
                                    var pagenum = this.dataSource.page();
                                    var pageitms = this.dataSource.pageSize();
                                    var rows = this.items();
                                    $(rows).each(function () {
                                        var index = (pagenum - 1) * pageitms + $(this).index() + 1;
                                        var rowLabel = $(this).find(".row-number");
                                        $(rowLabel).html(index);
                                    });
                                }

                            }).data("kendoGrid");
                        }



                        //kendo end

                        if ($scope.students.length > 0 && $scope.students != null && $scope.students != undefined) {
                            $scope.catreport = false;
                            $scope.exp_excel_flag = false;
                            $scope.print_flag = false;
                            $scope.report = true;

                        }
                        else {
                            swal("No Records Found!");
                            $scope.catreport = true;
                            $scope.report = false;
                            $state.reload();
                        }
                    })
            }
            else {
                $scope.submitted = true;
            }
        }


        $scope.cancel = function () {
            $scope.ASMAY_Id = '';
            $scope.AMCO_Id = '';
            $scope.AMB_Id = '';
            $scope.AMSE_Id = '';
            $scope.ACMS_Id = '';
            $scope.quota = '';
            $scope.mfa = '';
            $scope.catreport = true;
            $scope.submitted = false;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
            $scope.search = "";
        };


        $scope.printData = function () {

            var innerContents = document.getElementById("testprid").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/CumulativeReportPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        }

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.isOptionsRequired = function () {
            return !$scope.checklist.some(function (options) {
                return options.ivrm_id;
            });
        }

        $scope.printstudents = [];
        $scope.toggleAll = function () {

            var toggleStatus = $scope.all;
            angular.forEach($scope.students, function (itm) {
                itm.selected = toggleStatus;
                if ($scope.all == true) {
                    if ($scope.printstudents.indexOf(itm) === -1) {
                        $scope.printstudents.push(itm);
                    }
                }
                else {
                    $scope.printstudents.splice(itm);
                }
            });
        }

        $scope.optionToggled = function (SelectedStudentRecord, index) {
            $scope.all = $scope.students.every(function (itm) { return itm.selected; });
            if ($scope.printstudents.indexOf(SelectedStudentRecord) === -1) {
                $scope.printstudents.push(SelectedStudentRecord);
            }
            else {
                $scope.printstudents.splice($scope.printstudents.indexOf(SelectedStudentRecord), 1);
            }
        }

    }

})();