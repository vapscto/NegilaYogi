(function () {
    'use strict';
    angular
        .module('app')
        .controller('NoOfValueAddedCourses132', NoOfValueAddedCourses132)

    NoOfValueAddedCourses132.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', 'Excel', '$timeout']
    function NoOfValueAddedCourses132($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, Excel, $timeout) {



        $scope.exportToExcel = function (table) {
            var excelname = 'Cat 1.3.2.xls';
            var exportHref = Excel.tableToExcel(table, '1.3.2');
            $timeout(function () {
                var a = document.createElement('a');
                a.href = exportHref;
                a.download = excelname;
                document.body.appendChild(a);
                a.click();
                a.remove();
            }, 100);
        }


        $scope.loaddata = function () {
            var pageid = 2;
            apiService.getURI("Medical_Criteria1Reports/getdata", pageid).then(function (promise) {

                $scope.getinstitutioncycle = promise.getinstitutioncycle;
                $scope.cycleid = promise.getinstitutioncycle[0].cycleid;
                $scope.getparentidzero = promise.getinstitution;

                $scope.naacsL_InstitutionTypeFlg = promise.naacsL_InstitutionTypeFlg;

                angular.forEach($scope.getparentidzero, function (options) {
                    options.select = true;
                })
            })
        }


        $scope.get_selcetYear = function (data) {

            var nofyear = Number($scope.noofyear);
            angular.forEach($scope.yearlist, function (tt) {
                tt.select = false;
            })
            var s = 0;
            angular.forEach($scope.yearlist, function (pp) {
                if (s < nofyear) {
                    pp.select = true;
                }
                s += 1;
            })
        }

        $scope.togchkbx = function () {
            $scope.getparentidzero.every(function (options) {
                return options.select;
            });
        }

        $scope.cancel = function () {
            $state.reload();
        }
        $scope.printData = function () {

            var innerContents = document.getElementById("printSectionId").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/ExchangableCoursePdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };
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
        $scope.usrname = localStorage.getItem('username');
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings !== null && admfigsettings.length !== 0 && admfigsettings.length !== undefined) {
            logopath = admfigsettings[0].asC_Logo_Path;
        } else {
            logopath = "";
        }
        $scope.imgname = logopath;


        //======================report.
        $scope.submitted = false;
        $scope.showflag = false;
        $scope.showdetails = function () {
          
            $scope.selected_Inst = [];
            angular.forEach($scope.getparentidzero, function (mm) {
                if (mm.select) {
                    $scope.selected_Inst.push(mm);
                }
            })
            if ($scope.myForm.$valid) {
                var data = {
                    //selectedYear: $scope.selectedYear,
                    "cycleid": $scope.cycleid,
                    selected_Inst: $scope.selected_Inst,
                }
                apiService.create("Medical_Criteria1Reports/MC_VAC_report_132", data).then(function (promise) {

                    $scope.reportlist = promise.reportlist;
                    $scope.reportlist2 = promise.reportlist2;
                    if (promise.reportlist.length > 0) {
                        angular.forEach($scope.reportlist, function (tt) {
                            $scope.mainArray = [];
                            angular.forEach($scope.reportlist2, function (ss) {
                                if (tt.filefkid == ss.ncacvaC132_Id) {
                                    $scope.mainArray.push(ss);
                                }
                            })
                            tt.listdata = $scope.mainArray;
                        })
                        console.log($scope.reportlist);
                        $scope.showflag = true;
                    }
                    else {
                        $scope.showflag = false;
                        swal('Record Not Found!');
                    }
                })
            }
            else {
                $scope.submitted = true;
            }
        }
        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        //$scope.isOptionsRequired = function () {
        //    return !$scope.yearlist.some(function (options) {
        //        return options.select;
        //    });
        //}


        $scope.isOptionsRequired = function () {
            return !$scope.getparentidzero.some(function (options) {
                return options.select;
            });
        }




    }
})();

