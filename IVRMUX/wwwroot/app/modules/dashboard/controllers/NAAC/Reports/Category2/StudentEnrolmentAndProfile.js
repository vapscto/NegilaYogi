(function () {
    'use strict';
    angular
        .module('app')
        .controller('StudentEnrolmentAndProfileController', StudentEnrolmentAndProfileController)

    StudentEnrolmentAndProfileController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', 'Excel', '$timeout']
    function StudentEnrolmentAndProfileController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, Excel, $timeout) {




        $scope.printflag = false;
        $scope.showflag = false;

        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;
        //$scope.searchValue23 = "";

        $scope.sortReverse = true;
        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey == key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        }


        $scope.loaddata = function () {
            $scope.printflag = false;
            $scope.showflag = false;
            var pageid = 2;
            apiService.getURI("DisabilityStudent/getdata", pageid).then(function (promise) {

                $scope.getinstitutioncycle = promise.getinstitutioncycle;
                $scope.cycleid = promise.getinstitutioncycle[0].cycleid;
                $scope.getparentidzero = promise.getinstitution;

                $scope.naacsL_InstitutionTypeFlg = promise.naacsL_InstitutionTypeFlg;

                angular.forEach($scope.getparentidzero, function (options) {
                    options.select = true;
                })

                //$scope.noofyear = 2;
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

        $scope.togchkbx = function () {

            $scope.noofyear = 0;
            angular.forEach($scope.yearlist, function (ff) {
                if (ff.select == true) {
                    $scope.noofyear += 1;
                }

            })
        }

        //============================For Print Option
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
        }
        $scope.ddate = new Date();

        //$scope.usrname = localStorage.getItem('username');
        //var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));
        //var paginationformasters;
        //var copty;
        //var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        //if (ivrmcofigsettings.length > 0) {
        //    paginationformasters = ivrmcofigsettings[0].ivrmgC_ReportPagination;
        //    copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
        //}
        //$scope.coptyright = copty;
        //var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        //if (admfigsettings.length > 0) {
        //    var logopath = admfigsettings[0].asC_Logo_Path;
        //}
        //$scope.imgname = logopath;




        //======================Get Report.
        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        $scope.submitted = false;
        $scope.get_report = function () {
            $scope.showflag = false;
            $scope.selectedYear = [];

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
                apiService.create("DisabilityStudent/Student_Enrolment_Profile_Report21", data).then(function (promise) {
                    $scope.yearlist = promise.yearlist;
                    $scope.otherstatereportlist = promise.otherstatereportlist;
                    $scope.otherstatereportlist = promise.otherstatereportlist;
                    //$scope.othercountryreportlist = promise.othercountryreportlist;
                    $scope.otherstatecount = promise.otherstatecount;
                    $scope.othercountrycount = promise.othercountrycount;


                    $scope.mainresult = [];

                    angular.forEach($scope.yearlist, function (y1) {
                        $scope.resultlist = [];
                        angular.forEach($scope.otherstatereportlist, function (y2) {
                            //if (y1.select) {
                                if (y1.asmaY_Id == y2.ASMAY_Id) {

                                    $scope.resultlist.push(y2);
                                //}
                            }
                        })
                        //angular.forEach($scope.resultlist, function (rr) {
                        //    $scope.cntrystudentcount = [];
                        //    angular.forEach($scope.othercountrycount, function (sc) {
                        //        if (rr.ASMAY_Id == sc.asmaY_Id) {
                        //            $scope.cntrystudentcount.push(sc);
                        //        }
                        //    })
                        //})

                        //if (y1.select) {
                            $scope.mainresult.push({ ASMAY_Id: y1.asmaY_Id, yearname: y1.asmaY_Year, result: $scope.resultlist, /*studentcount: $scope.cntrystudentcount*/ });
                        //}
                    })

                    console.log("============================================================");
                    console.log($scope.resultlist);
                    console.log($scope.mainresult);
                    console.log("============================================================");
                    //console.log($scope.cntrystudentcount);
                    //var a = [1, 2, 3, 1, 2, 3, 2, 2, 3, 4, 5, 5, 12, 1, 23, 4, 1, '23'],
                    //    filtered = a.filter(function (a) {
                    //        if (!this.has(a)) {
                    //            this.set(a, true);
                    //            return true;
                    //        }
                    //    }, new Map);

                    //console.log(filtered);

                    if ($scope.otherstatereportlist.length > 0) {
                        $scope.showflag = true;
                        $scope.printflag = true;
                    }
                    else {
                        $scope.showflag = false;
                        $scope.printflag = false;
                        swal('Records Not Found!');
                    }
                })
            }
            else {
                $scope.submitted = true;
            }
           
        }






        //======================For Excel sheet
        $scope.exportToExcel = function (table) {

            var excelname = 'Cat 2.1xls';
            var exportHref = Excel.tableToExcel(table, '2.1');
            $timeout(function () {
                var a = document.createElement('a');
                a.href = exportHref;
                a.download = excelname;
                document.body.appendChild(a);
                a.click();
                a.remove();
            }, 100);
        }


        //======================for Cancel Button
        $scope.cancel = function () {
            $state.reload();
        }






    }
})();

