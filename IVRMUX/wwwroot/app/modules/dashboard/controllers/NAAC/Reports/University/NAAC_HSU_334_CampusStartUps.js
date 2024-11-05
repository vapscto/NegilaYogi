(function () {
    'use strict';
    angular
        .module('app')
        .controller('NAAC_HSU_323_ResearchProjectsRatioController', NAAC_HSU_323_ResearchProjectsRatioController)

    NAAC_HSU_323_ResearchProjectsRatioController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', 'Excel', '$timeout']
    function NAAC_HSU_323_ResearchProjectsRatioController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, Excel, $timeout) {


        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;
        //$scope.searchValue23 = "";

        $scope.showflag = false;

        $scope.sortReverse = true;
        $scope.sort = function (key) {
            $scope.sortReverse = $scope.sortKey === key ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        };


        $scope.cancel = function () {
            $state.reload();
        };
        $scope.loaddata = function () {

            var pageid = 2;
            apiService.getURI("NAAC_HSU_323_ResearchProjectsRatio/getdata", pageid).then(function (promise) {

                $scope.getinstitutioncycle = promise.getinstitutioncycle;
                $scope.cycleid = promise.getinstitutioncycle[0].cycleid;
                $scope.getparentidzero = promise.getinstitution;
                $scope.naacsL_InstitutionTypeFlg = promise.naacsL_InstitutionTypeFlg;
                angular.forEach($scope.getparentidzero, function (options) {
                    options.select = true;
                });
            });
        };
        $scope.get_selcetYear = function (data) {

            var nofyear = Number($scope.noofyear);
            angular.forEach($scope.yearlist, function (tt) {
                tt.select = false;
            });
            var s = 0;
            angular.forEach($scope.yearlist, function (pp) {
                if (s < nofyear) {
                    pp.select = true;
                }
                s += 1;
            });

        };
        $scope.togchkbx = function () {
            $scope.getparentidzero.every(function (options) {
                return options.select;
            });
        };
        $scope.exportToExcel = function (table) {

            var excelname = 'Cat 3.2.4.xls';
            var exportHref = Excel.tableToExcel(table, '3.2.4');
            $timeout(function () {
                var a = document.createElement('a');
                a.href = exportHref;
                a.download = excelname;
                document.body.appendChild(a);
                a.click();
                a.remove();
            }, 100);
        };
        //========================print details
        $scope.printData = function () {
            var innerContents = document.getElementById("printSectionId").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/Library/ExchangableCoursePdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };
        $scope.isOptionsRequired = function () {
            return !$scope.getparentidzero.some(function (options) {
                return options.select;
            });
        };
        $scope.submitted = false;
        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        //==============report
        $scope.get_report = function () {
            
            $scope.showflag = false;
            $scope.selected_Inst = [];
            angular.forEach($scope.getparentidzero, function (mm) {
                if (mm.select) {
                    $scope.selected_Inst.push(mm);
                }
            });

            if ($scope.myForm.$valid) {
                var data = {
                    //selectedYear: $scope.selectedYear,
                    "cycleid": $scope.cycleid,
                    selected_Inst: $scope.selected_Inst
                };
                apiService.create("NAAC_HSU_323_ResearchProjectsRatio/get_334U_report", data).then(function (promise) {
                    if (promise.reportlist.length > 0 && promise.reportlist != null) {
                        $scope.showflag = true;
                        $scope.printflag = true;
                        $scope.reportlist = promise.reportlist;
                        $scope.reportlist2 = promise.reportlist2;

                        $scope.finalarray = [];
                        angular.forEach($scope.reportlist, function (gg) {
                            var govtfiles = [];
                            angular.forEach($scope.reportlist2, function (ff) {
                                if (gg.NCHSU324CS_Id === ff.nchsU324CS_Id) {                               
                                    govtfiles.push({ nchsU324CSF_FileName: ff.nchsU324CSF_FileName, nchsU324CSF_FilePath: ff.nchsU324CSF_FilePath, nchsU324CSF_Filedesc: ff.nchsU324CSF_Filedesc })
                                }
                            })
                            $scope.finalarray.push({ YEAR: gg.ASMAY_Year, NCHSU324CS_Id: gg.NCHSU324CS_Id, NCHSU324CS_Year: gg.NCHSU324CS_Year, NCHSU324CS_StartUpName: gg.NCHSU324CS_StartUpName, NCHSU324CS_NatureOfStartUp: gg.NCHSU324CS_NatureOfStartUp, NCHSU324CS_Contactinfo: gg.NCHSU324CS_Contactinfo, FILES: govtfiles })
                        })
                        $scope.yearlist = promise.yearlist;
                        $scope.finalarray1 = [];
                        angular.forEach($scope.yearlist, function (yy) {
                            var temparr = [];
                            angular.forEach($scope.finalarray, function (dd) {
                                if (yy.asmaY_Year == dd.YEAR) {
                                    temparr.push(dd);
                                }
                            })
                            if (temparr.length > 0) {
                                $scope.finalarray1.push({ YEAR: yy.asmaY_Year, dlist: temparr })
                            }
                        })
                        console.log($scope.finalarray);
                    }
                    else {
                        $scope.showflag = false;
                        swal('Record Not Found!');
                    }
                });
            }
            else {
                $scope.submitted = true;
            }
        };
    }
})();
