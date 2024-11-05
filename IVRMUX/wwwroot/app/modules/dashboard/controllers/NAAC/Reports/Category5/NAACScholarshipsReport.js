﻿(function () {
    'use strict';
    angular
        .module('app')
        .controller('NAACScholarshipsReportController', NAACScholarshipsReportController)

    NAACScholarshipsReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', 'Excel', '$timeout']
    function NAACScholarshipsReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, Excel, $timeout) {

        $scope.printflag = false;

        $scope.currentPage = 1;
        $scope.itemsPerPage = 15;
        //$scope.searchValue23 = "";

        $scope.sortReverse = true;
        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey == key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        }


        $scope.noofyear = 5;
        

        $scope.exportToExcel = function (table) {

            var excelname = 'Cat 1.1.xls';
            var exportHref = Excel.tableToExcel(table, '1.1');
            $timeout(function () {
                var a = document.createElement('a');
                a.href = exportHref;
                a.download = excelname;
                document.body.appendChild(a);
                a.click();
                a.remove();
            }, 100);
        }



        $scope.printData = function () {
            var innerContents = document.getElementById("printSectionId1").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/Library/ExchangableCoursePdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 1000);">' + innerContents + '</html>');
            popupWinindow.document.close();
        }

        $scope.exportToExcel = function (tableId) {
            var exportHref = Excel.tableToExcel(tableId, '5.1.1&5.1.2');
            $timeout(function () { location.href = exportHref; }, 1000);
        }

        //$scope.exportToExcel = function (table) {

        //    var excelname = 'Cat 1.1.3.xls';
        //    var exportHref = Excel.tableToExcel(table, '1.1.3');
        //    $timeout(function () {
        //        var a = document.createElement('a');
        //        a.href = exportHref;
        //        a.download = excelname;
        //        document.body.appendChild(a);
        //        a.click();
        //        a.remove();
        //    }, 100);
        //}
        $scope.getinstitutioncycle = [];
        $scope.loaddata = function () {
            $scope.printflag = false;
            var pageid = 2;
            apiService.getURI("NAACCriteriaFiveReport/getdata", pageid).then(function (promise) {


                $scope.getinstitutioncycle = promise.getinstitutioncycle;
                $scope.getparentidzero = promise.getinstitution;

                angular.forEach($scope.getparentidzero, function (options) {
                    options.select = true;
                })
                if ($scope.getinstitutioncycle.length > 0) {
                    $scope.cycleid = $scope.getinstitutioncycle[0].cycleid;
                }
                //$scope.yearlist = promise.yearlist;

                //var s = 0;
                //angular.forEach($scope.yearlist, function (pp) {
                //    if (s < $scope.noofyear) {
                //        pp.select = true;
                //    }
                //    s += 1;
                //})
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
        $scope.isOptionsRequired = function () {
            return !$scope.getparentidzero.some(function (options) {
                return options.select;
            });
        }
        $scope.togchkbx = function () {
            //$scope.usercheck = $scope.yearlist.every(function (options) {
            //    return options.select;
            //});
            $scope.noofyear = 0;
            angular.forEach($scope.yearlist, function (ff) {
                if (ff.select == true) {
                    $scope.noofyear += 1;
                }

            })
        }

        $scope.interacted = function () {
            return $scope.submitted;
        }
        //======================report.
        $scope.showflag = false;
        $scope.showdetails = function () {
            $scope.showflag = false;
            $scope.printflag = false;
            $scope.mainarray = [];
            if ($scope.myForm.$valid) {

                $scope.selected_Inst = [];
                angular.forEach($scope.getparentidzero, function (mm) {
                    if (mm.select) {
                        $scope.selected_Inst.push(mm);
                    }
                })

            var data = {
                "cycleid": $scope.cycleid,
                selected_Inst: $scope.selected_Inst,
            }
            apiService.create("NAACCriteriaFiveReport/get_report", data).then(function (promise) {

                $scope.govtsclist = promise.govtsclist;
                $scope.instsclist = promise.instsclist;
                $scope.govtsclistfiles = promise.govtsclistfiles;
                $scope.instsclistfiles = promise.instsclistfiles;
                if ($scope.govtsclist.length > 0 || $scope.instsclist.length>0) {
                    $scope.showflag = true;
                    $scope.printflag = true;
                    $scope.mainarray = [];

                    angular.forEach($scope.govtsclist, function (gg) {
                        var govtfiles = [];

                        angular.forEach($scope.govtsclistfiles, function (ff) {
                            if (gg.ncaC511GSCH_Id == ff.ncaC511GSCH_Id) {
                                govtfiles.push({ filename: ff.ncaC511GSCHF_FileName, filepath: ff.ncaC511GSCHF_FilePath, filedesc: ff.ncaC511GSCHF_Filedesc})
                            }
                        })
                        $scope.mainarray.push({ asmaY_Year: gg.asmaY_Year,aasmay_id: gg.ncaC511GSCH_Year, schname: gg.ncaC511GSCH_SchemeName, gcount: gg.ncaC511GSCH_NoOfStudents, incount:0, filelist: govtfiles})
                    })
                    //////INST/////
                    angular.forEach($scope.instsclist, function (gg) {
                        var govtfiles = [];

                        angular.forEach($scope.instsclistfiles, function (ff) {
                            if (gg.ncaC512INSCH_Id == ff.ncaC512INSCH_Id) {
                                govtfiles.push({ filename: ff.ncaC512INSCHF_FileName, filepath: ff.ncaC512INSCHF_FilePath, filedesc: ff.ncaC512INSCHF_Filedesc })
                            }
                        })
                        $scope.mainarray.push({ asmaY_Year: gg.asmaY_Year,aasmay_id: gg.ncaC512INSCH_Year, schname: gg.ncaC512INSCH_SchemeName, gcount:0, incount: gg.ncaC512INSCH_NoOfStudents, filelist: govtfiles })
                    })


                    console.log($scope.mainarray);
                    $scope.finalarray = [];

                    $scope.selectedYear=promise.yearlist;
                    //angular.forEach($scope.selectedYear, function (yy) {
                    //    $scope.tfinalarray = [];
                    //    angular.forEach($scope.mainarray, function (tt) {
                    //        if (yy.asmaY_Year == tt.asmaY_Year) {
                    //            $scope.tfinalarray.push({ YEAR: yy.asmaY_Year, SCHEME: tt.schname, GCNT: tt.gcount, ICNT: tt.incount, FILES: tt.filelist})
                    //        }

                    //    })
                    //    if ($scope.tfinalarray.length>0) {
                    //        $scope.finalarray.push({ YEAR: yy.asmaY_Year, dlist: $scope.tfinalarray })
                    //    }
                       

                    //})
                    $scope.tfinalarray = [];
                    angular.forEach($scope.selectedYear, function (yy) {
                        
                        angular.forEach($scope.mainarray, function (tt) {
                            if (yy.asmaY_Year == tt.asmaY_Year) {
                                $scope.tfinalarray.push({ YEAR: yy.asmaY_Year, SCHEME: tt.schname, GCNT: tt.gcount, ICNT: tt.incount, FILES: tt.filelist })
                            }

                        })
                        //if ($scope.tfinalarray.length > 0) {
                        //    $scope.finalarray.push({ YEAR: yy.asmaY_Year, dlist: $scope.tfinalarray })
                        //}


                    })





                    console.log($scope.tfinalarray);
                }
                else {
                    $scope.showflag = false;
                    $scope.printflag = false;

                    swal('No Record Found')
                }
            })

        }
            else {
            $scope.submitted = true;
        }
        }


        $scope.cancel = function () {
            $state.reload();
        }

    }
})();
