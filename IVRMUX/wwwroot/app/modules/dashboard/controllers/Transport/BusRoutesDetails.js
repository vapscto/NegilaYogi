(function () {
    'use strict';
    angular
.module('app')
.controller('BusRoutesDetailsController', BusRoutesDetailsController)

    BusRoutesDetailsController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', 'Excel', '$timeout', 'superCache', '$filter']
    function BusRoutesDetailsController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, Excel, $timeout, superCache, $filter) {
        $scope.sortKey = "astA_Id";
        $scope.columnsTest = [];
        $scope.obj = {};
        $scope.tadprint = false;
        $scope.exp_excel_flag = true;
        $scope.print_flag = true;
        $scope.printstudents = [];
        $scope.count12 = false;

        $scope.usrname = localStorage.getItem('username');

        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));

        var paginationformasters;
        var copty;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_ReportPagination;
            copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
        }

        $scope.itemsPerPage = paginationformasters;
        if ($scope.itemsPerPage == undefined || $scope.itemsPerPage == null) {
            $scope.itemsPerPage = 5;
        }
        $scope.currentPage = 1;
        $scope.coptyright = copty;

        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }
        $scope.imgname = logopath;
        $scope.reporsmart = false;
        $scope.reporsmart1 = false;

        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey == key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        }

        $scope.BindData = function () {
            apiService.getDATA("BusRoutesDetails/getdata").then(function (promise) {
                if (promise != null) {
                    $scope.YearList = promise.yearList;
                    $scope.seclist = promise.seclist;
                    $scope.classlist = promise.classlist;
                }
                else {
                    swal("No Records Found")
                }
            })
        }

        $scope.searchValue = '';

        $scope.filterValue = function (obj) {
            return angular.lowercase(obj.asmcL_ClassName).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
                 angular.lowercase(obj.trmR_RouteName).indexOf(angular.lowercase($scope.searchValue)) >= 0;
        }

        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        $scope.reporsmart2 = false;
        $scope.detailsgrid = false;
        $scope.reporsmart = false;
        $scope.getreport = function (obj) {
            $scope.reporsmart2 = false;
            $scope.mainarray = [];
            $scope.studentgriddata = [];
            $scope.students = [];
            $scope.classarray = [];
            $scope.newseclist = [];
            $scope.newclsist = [];
            $scope.all = "";
            $scope.reporsmart = false;
            $scope.reporsmart = false;
            $scope.detailsgrid = false;
            $scope.searchValue = '';
            if ($scope.myForm.$valid) {

                if ($scope.type =='stdcount') {
                    var data = {
                        "asmaY_Id": $scope.asmaY_Id,
                        "flag": $scope.allorindiv,
                        "type": $scope.type
                    }
                } else {
                    
                    angular.forEach($scope.classlist, function (cc) {
                        if ($scope.asmcL_Id == '0') {
                            $scope.newclsist.push(cc);
                        }
                        else if ($scope.asmcL_Id == cc.asmcL_Id) {
                            $scope.newclsist.push(cc);
                        }

                    })

                    angular.forEach($scope.seclist, function (ss) {
                        if ($scope.asmS_Id == '0') {
                            $scope.newseclist.push(ss);
                        }
                        else if ($scope.asmS_Id == ss.asmS_Id) {
                            $scope.newseclist.push(ss);
                        }

                    })

                    var data = {
                        "asmaY_Id": $scope.asmaY_Id,
                        "flag": $scope.allorindiv,
                        "type": $scope.type,
                        "secidlist": $scope.newseclist,
                            "clsidlist": $scope.newclsist
                    }

                      
                }

               
                apiService.create("BusRoutesDetails/Getreportdetails", data).
                 then(function (promise) {

                     if ($scope.type == 'stdcount') {
                         if (promise.griddata == null) {
                             $scope.reporsmart = false;
                             $scope.reporsmart1 = false;
                             swal("Record Not Found !!");
                             //$state.reload();
                         }
                         else {
                             $scope.reporsmart1 = false;
                             angular.forEach($scope.YearList, function (yy) {
                                 if (yy.asmaY_Id == $scope.asmaY_Id) {
                                     $scope.year = yy.asmaY_Year;
                                 }
                             })

                             if ($scope.allorindiv == "Approved") {
                                 $scope.status = " Approved List ";
                             }
                             else if ($scope.allorindiv == "Waiting") {
                                 $scope.status = "Waiting List";
                             }

                             else if ($scope.allorindiv == "Rejected") {
                                 $scope.status = "Rejected List";
                             }

                             else {
                                 $scope.status = "";
                             }

                             $scope.reporsmart = true;
                             $scope.reporsmart1 = true;
                             $scope.students = promise.griddata;
                             $scope.classarray = promise.classdata;

                             $scope.presentCountgrid = $scope.students.length;
                             angular.forEach($scope.students, function (y) {
                                 var total_days = 0;
                                 angular.forEach($scope.classarray, function (z) {
                                     total_days += y[z.asmcL_ClassName];
                                 })
                                 y.Total = total_days;
                             })


                             $scope.exp_excel_flag = false;
                             $scope.print_flag = false;
                             $scope.temparray = [];
                             $scope.temparray2 = [];

                             angular.forEach($scope.classarray, function (y) {
                                 var rtcnt1 = 0;
                                 angular.forEach($scope.students, function (z) {
                                     rtcnt1 += z[y.asmcL_ClassName]
                                 })
                                 //  y.Total = total_days;
                                 $scope.temparray2.push({ asmcL_ClassName: y.asmcL_ClassName, total: rtcnt1 })
                             })

                             //angular.forEach($scope.classarray, function (t1) {
                             //    var rtcnt1 = 0;
                             //    angular.forEach($scope.griddata, function (t2) {

                             //        if (t1.asmcL_ClassName == t2.asmcL_Id) {
                             //            rtcnt1 += t2.stud_count
                             //        }

                             //    })
                             //    $scope.temparray2.push({ asmcL_Id: t1.asmcL_Id, total: rtcnt1 })

                             //})
                             $scope.total12 = 0;
                             angular.forEach($scope.temparray2, function (t2) {
                                 $scope.total12 += t2.total
                             })
                         }
                     }
                     else {
                         if ($scope.allorindiv == "Approved") {
                             $scope.status = " Approved List ";
                         }
                         else if ($scope.allorindiv == "Waiting") {
                             $scope.status = "Waiting List";
                         }

                         else if ($scope.allorindiv == "Rejected") {
                             $scope.status = "Rejected List";
                         }

                         else {
                             $scope.status = "";
                         }
                         //added praveen
                         $scope.detailsgrid = true;
                         $scope.studentgriddata = promise.studentgriddata;
                         if ($scope.studentgriddata.length>0) {
                             $scope.selectedsection = [];
                             $scope.reporsmart = true;
                             $scope.reporsmart1 = false;

                             $scope.reporsmart2 = true;
                             $scope.detailsgrid = true;
                             angular.forEach($scope.studentgriddata, function (stu2) {

                                 if ($scope.selectedsection.length == 0) {
                                     $scope.selectedsection.push({ TRMR_Id: stu2.TRMR_Id, TRMR_RouteName: stu2.TRMR_RouteName, trmr_order: stu2.trmr_order });
                                 }
                                 else if ($scope.selectedsection.length > 0) {
                                     var al_ct = 0;
                                     angular.forEach($scope.selectedsection, function (uf) {
                                         if (uf.TRMR_Id == stu2.TRMR_Id) {
                                             al_ct += 1;
                                         }
                                     })
                                     if (al_ct == 0) {
                                         $scope.selectedsection.push({ TRMR_Id: stu2.TRMR_Id, TRMR_RouteName: stu2.TRMR_RouteName, trmr_order: stu2.trmr_order });
                                     }
                                 }


                             })


                             $scope.mainarray = [];
                             angular.forEach($scope.selectedsection, function (ff) {
                                 var listtemparray = [];
                                 angular.forEach($scope.studentgriddata, function (ac) {
                                     if (ac.TRMR_Id == ff.TRMR_Id) {
                                         listtemparray.push(ac)
                                     }

                                 })

                                 $scope.mainarray.push({ TRMR_Id: ff.TRMR_Id, TRMR_RouteName: ff.TRMR_RouteName, trmr_order: ff.trmr_order, arraylist: listtemparray });

                             })



                         }
                         else {
                             $scope.detailsgrid = false;
                             $scope.detailsgrid = false;
                             swal('No Record Found');
                         }

                         
                     }
                  

                 })
            }
            else {
                $scope.submitted = true;
            }
        }

        $scope.printData = function () {
            if ($scope.type == 'stdcount') {
                var innerContents = document.getElementById("printareaId").innerHTML;
                var popupWinindow = window.open('');
                popupWinindow.document.open();
                popupWinindow.document.write('<html><head>' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
                    '<link type="text/css" media="print" href="css/print/preadmission/RegReportPdf.css" rel="stylesheet" />' +
                    '</head> <body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>'
                );
                popupWinindow.document.close();
            }
            else {
                var innerContents = document.getElementById("printareaId1").innerHTML;
                var popupWinindow = window.open('');
                popupWinindow.document.open();
                popupWinindow.document.write('<html><head>' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
                    '<link type="text/css" media="print" href="css/print/RegReportPdf.css" rel="stylesheet" />' +
                    '</head> <body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>'
                );
                popupWinindow.document.close();
            }
               
            

        }


        $scope.exportToExcel1 = function (export_id) {
            debugger;
            $scope.fromdateforexcel = new Date($scope.FMCB_fromDATE).toDateString();
            $scope.todateforexcel = new Date($scope.FMCB_toDATE).toDateString()

            $scope.year

            var excelname = "Transport Route Class Wise Details Report _" + $scope.year + "  " + $scope.status + ".xls";

            var exportHref = Excel.tableToExcel(export_id, 'sheet name');

            $timeout(function () {
                var a = document.createElement('a');
                a.href = exportHref;
                a.download = excelname;
                document.body.appendChild(a);
                a.click();
                a.remove();
            }, 100);

            //  $timeout(function () { location.href = exportHref; }, 100);
        }

        //--Cancel--//
        $scope.cancel = function () {
            $state.reload();
        }



        $scope.exportToExcel = function () {
            var exp = '';
            if ($scope.type == 'stdcount') { 
                exp = '#export_id'
            }
            else {
                exp = '#export_id1'
            }


            $scope.fromdateforexcel = new Date($scope.FMCB_fromDATE).toDateString();
            $scope.todateforexcel = new Date($scope.FMCB_toDATE).toDateString()
           
            $scope.year

            var excelname = "Transport Route Class Wise Details Report _" +  $scope.year + "  "+ $scope.status +".xls";

            var exportHref = Excel.tableToExcel(exp, 'sheet name');

            $timeout(function () {
                var a = document.createElement('a');
                a.href = exportHref;
                a.download = excelname;
                document.body.appendChild(a);
                a.click();
                a.remove();
            }, 100);

          //  $timeout(function () { location.href = exportHref; }, 100);
        }

        $scope.validreport = function () {

            $scope.students = [];

        }
    };

})();