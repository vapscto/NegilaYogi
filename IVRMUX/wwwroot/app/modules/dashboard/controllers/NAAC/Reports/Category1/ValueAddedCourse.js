(function () {
    'use strict';
    angular
        .module('app')
        .controller('ValueAddedCourseController', ValueAddedCourseController)

    ValueAddedCourseController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', 'Excel', '$timeout']
    function ValueAddedCourseController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, Excel, $timeout) {


        //$scope.ListofItems = [];
        //$scope.ListofItems = [{ name: "BC0031", program: "Program-E", certificateName: "Automation Certificate", year: "2018-2019" },
        //{ name: "BC0032", program: "Program-D", certificateName: " Certificate-D", year: "2017-2018" },
        //{ name: "BC0033", program: "Program-C", certificateName: " Certificate-C", year: "2016-2017" },
        //{ name: "BC0034", program: "Program-B", certificateName: " Certificate-B", year: "2015-2016" },
        //{ name: "BC0035", program: "Program-A", certificateName: " Certificate-A", year: "2014-2015" },
        //]

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
            apiService.getURI("CurricularAspects/getdata", pageid).then(function (promise) {

                $scope.getinstitutioncycle = promise.getinstitutioncycle;
                $scope.cycleid = promise.getinstitutioncycle[0].cycleid;
                $scope.getparentidzero = promise.getinstitution;

                $scope.naacsL_InstitutionTypeFlg = promise.naacsL_InstitutionTypeFlg;

                angular.forEach($scope.getparentidzero, function (options) {
                    options.select = true;
                })

                //$scope.yearlist = promise.yearlist;
                //$scope.yearlist = [
                //    { asmaY_Year: "2018-2019", asmaY_Id: 5 },
                //    { asmaY_Year: "2017-2018", asmaY_Id: 4 },
                //    { asmaY_Year: "2016-2017", asmaY_Id: 3 },
                //    { asmaY_Year: "2015-2016", asmaY_Id: 2 },
                //    { asmaY_Year: "2014-2015", asmaY_Id: 1 },
                //]

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

        //$scope.togchkbx = function () {
        //    $scope.noofyear = 0;
        //    angular.forEach($scope.yearlist, function (ff) {
        //        if (ff.select == true) {
        //            $scope.noofyear += 1;
        //        }
        //    })
        //}

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
            //$scope.selectedYear = [];
            //angular.forEach($scope.yearlist, function (yy) {
            //    if (yy.select) {
            //        $scope.selectedYear.push(yy);
            //    }
            //})
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
                apiService.create("CurricularAspects/get_report_132", data).then(function (promise) {

                    //$scope.reportlist = promise.reportlist;
                    if (promise.reportlist.length > 0) {
                        $scope.showflag = true;
                        $scope.reportlist = promise.reportlist
                        $scope.yearlist = promise.yearlist

                        $scope.vacfiles = promise.instsclist;
                        $scope.vacdetailsfile = promise.instsclistfiles;

                        console.log($scope.reportlist);
                        console.log($scope.vacfiles);
                        console.log($scope.vacdetailsfile);

                        $scope.mainlist = [];
                        angular.forEach($scope.yearlist, function (y1) {
                            $scope.sublist = [];
                            angular.forEach($scope.reportlist, function (y2) {
                                //if (y1.select) {
                                if (y1.asmaY_Id == y2.ASMAY_Id) {
                                    $scope.sublist.push(y2);
                                }
                                //}
                            });
                            //if (y1.select) {
                            if ($scope.sublist.length > 0) {
                                $scope.mainlist.push({ ASMAY_Id: y1.asmaY_Id, yearname: y1.asmaY_Year, sublist1: $scope.sublist });
                            }
                            //  }
                            angular.forEach($scope.mainlist, function (yy) {
                                angular.forEach(yy.sublist1, function (rr) {
                                    var subarr1 = [];
                                    angular.forEach($scope.vacfiles, function (tt) {
                                        if (tt.ncacvaC132_Id == rr.NCACVAC132_Id) {
                                            subarr1.push(tt);
                                        }
                                    })
                                    var subarr2 = [];
                                    angular.forEach($scope.vacdetailsfile, function (zz) {
                                        if (zz.ncacvaC132_Id == rr.NCACVAC132_Id) {
                                            subarr2.push(zz);
                                        }
                                    })
                                    rr.ar1 = subarr1;
                                    rr.ar2 = subarr2;
                                })
                            })
                        });
                        console.log("======================================");


                        console.log($scope.mainlist);

                    }
                    else {
                        swal('Records are not Available!')
                        $scope.showflag = false;
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

