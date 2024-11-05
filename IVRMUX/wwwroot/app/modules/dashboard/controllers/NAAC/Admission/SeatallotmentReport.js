(function () {
    'use strict';
    angular.module('app').controller('SeatallotmentReportController', SeatallotmentReportController)

    SeatallotmentReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout']
    function SeatallotmentReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout) {


        $scope.temparray = [
            { id: 1, name: "Number of seats earmarked for reserved category as per GOI or State Government Rule" },
            { id: 2, name: "Number of students admitted from the reserved category" }
        ];


        $scope.report = false;
        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings !== null && ivrmcofigsettings.length !== 0 && ivrmcofigsettings.length !== undefined) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        } else {
            paginationformasters = 10;
        }

        $scope.itemsPerPage = paginationformasters;
        $scope.currentPage = 1;
        var logopath = "";
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings !== null && admfigsettings.length !== 0 && admfigsettings.length !== undefined) {
            logopath = admfigsettings[0].asC_Logo_Path;
        } else {
            logopath = "";
        }

        //================Page Load
        $scope.BindData = function () {
            var id = 0;
            apiService.getURI("SeatallotmentReport/getdetails", id).then(function (promise) {
                $scope.yearlist = promise.getyear;
            });
        };
        $scope.students = [];
        $scope.catreport = true;
        $scope.submitted = false;
        $scope.searchValue = '';

        $scope.clearreport = function () {
            $scope.catreport = true;
        };
        //======================Report
        $scope.onreport = function () {
            $scope.all = false;
            $scope.getdetails1 = [];
            if ($scope.myForm.$valid) {
                var data = {
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "Noofyears": $scope.Noofyears
                };
                apiService.create("SeatallotmentReport/onreport", data).
                    then(function (promise) {
                        $scope.getdetails1 = promise.getdetails;
                        if ($scope.getdetails1 !== null) {
                            $scope.getdetails = promise.getdetails;
                            $scope.getyearlist = promise.getyearlist;
                            $scope.catreport = false;
                            $scope.tempnewmain = [];
                            angular.forEach($scope.temparray, function (f) {
                                $scope.tempnew = [];
                                angular.forEach($scope.getdetails, function (d) {
                                    if (f.id === parseInt(d.id)) {
                                        $scope.tempnew.push(d);
                                    }
                                });
                                $scope.tempnewmain.push({ id: f.id, name: f.name, temp: $scope.tempnew });
                            });
                            $scope.exam_list = [];
                            $scope.overalltotalmax = 0;
                            $scope.mainheading = [];
                            angular.forEach($scope.tempnewmain, function (tt) {
                                $scope.mainheading1 = [];
                                angular.forEach(tt.temp, function (st) {
                                    if ($scope.exam_list.length === 0) {
                                        $scope.exam_list.push({ id: st.id, categoryname: st.categoryname, ACQC_Id: st.ACQC_Id });
                                        $scope.mainheading1.push({ id: st.id, categoryname: st.categoryname, ACQC_Id: st.ACQC_Id });
                                    }
                                    else if ($scope.exam_list.length > 0) {
                                        var al_exm_cnt = 0;
                                        angular.forEach($scope.exam_list, function (exm) {
                                            if (exm.ACQC_Id === st.ACQC_Id && exm.id === st.id) {
                                                al_exm_cnt += 1;
                                            }
                                        });
                                        if (al_exm_cnt === 0) {
                                            $scope.exam_list.push({ id: st.id, categoryname: st.categoryname, ACQC_Id: st.ACQC_Id });
                                            $scope.mainheading1.push({ id: st.id, categoryname: st.categoryname, ACQC_Id: st.ACQC_Id });
                                        }
                                    }
                                });
                                $scope.mainheading.push({ id: tt.id, name: tt.name, list: $scope.mainheading1 })
                            });
                            console.log($scope.tempnewmain);
                            console.log($scope.exam_list);
                            console.log($scope.mainheading);

                        } else {
                            swal("No Records Found");
                            $scope.catreport = true;
                        }
                    });
            }
            else {
                $scope.submitted = true;
            }
        };
        //=================Cancel
        $scope.cancel = function () {
            $scope.ASMAY_Id = '';
            $scope.Noofyears = "";
            $scope.catreport = true;
            $scope.submitted = false;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
            $scope.search = "";
        };
        //==============Print data
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
        $scope.imgname = logopath;

        //===================Excel Sheet
        $scope.exportToExcel = function (export_id) {
            var excelname = "Naac Seat Allotment Report For Last " + $scope.Noofyears + " Years " + ".xls";
            var exportHref = Excel.tableToExcel(export_id, 'Naac Seat Allotment Report' + $scope.Noofyears + 'Years');
            $timeout(function () {
                //location.href = exportHref;
                var a = document.createElement('a');
                a.href = exportHref;
                a.download = excelname;
                document.body.appendChild(a);
                a.click();
                a.remove();
            }, 100);
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.isOptionsRequired = function () {
            return !$scope.checklist.some(function (options) {
                return options.ivrm_id;
            });
        };

        $scope.printstudents = [];
        $scope.toggleAll = function () {
            var toggleStatus = $scope.all;
            angular.forEach($scope.students, function (itm) {
                itm.selected = toggleStatus;
                if ($scope.all === true) {
                    if ($scope.printstudents.indexOf(itm) === -1) {
                        $scope.printstudents.push(itm);
                    }
                }
                else {
                    $scope.printstudents.splice(itm);
                }
            });
        };

        $scope.optionToggled = function (SelectedStudentRecord, index) {
            $scope.all = $scope.students.every(function (itm) { return itm.selected; });
            if ($scope.printstudents.indexOf(SelectedStudentRecord) === -1) {
                $scope.printstudents.push(SelectedStudentRecord);
            }
            else {
                $scope.printstudents.splice($scope.printstudents.indexOf(SelectedStudentRecord), 1);
            }
        };

        $scope.all_check = function () {
            var toggleStatus = $scope.detail_checked;
            angular.forEach($scope.checklist, function (itm) {
                itm.ivrm_id = toggleStatus;
            });
        };

        $scope.albumNameArraycolumn = [];
        $scope.columnsTest = [];
        $scope.addColumn2 = function (role1) {
            $scope.detail_checked = $scope.checklist.every(function (itm) { return itm.selected; });
            if (role1.selected === true) {
                $scope.albumNameArraycolumn.push(role1);
                var newCol = { id: role1.ivrM_CLGREG_NAME, checked: true, value: role1.ivrM_CLGREG_PAR };
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
        };

        $scope.Clearid = function () {
            $state.reload();
        }

    }

})();