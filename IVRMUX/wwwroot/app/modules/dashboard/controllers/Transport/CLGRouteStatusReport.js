(function () {
    'use strict';
    angular
.module('app')
        .controller('CLGRouteStatusReportController', CLGRouteStatusReportController)

    CLGRouteStatusReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', 'Excel', '$timeout', 'superCache', '$filter']
    function CLGRouteStatusReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, Excel, $timeout, superCache, $filter) {
        $scope.sortKey = "asmcL_Order";
        $scope.columnsTest = [];
        $scope.obj = {};
        $scope.tadprint = false;
        $scope.exp_excel_flag = true;
        $scope.print_flag = true;
        $scope.printstudents = [];
        $scope.count12 = false;
        $scope.cancel=function()
        {
            $state.reload();
        }

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

        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey == key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        }

        $scope.BindData = function () {
            apiService.getDATA("CLGRouteStatusReport/getdata").then(function (promise) {
                if (promise != null) {
                    $scope.YearList = promise.yearList;
                    $scope.routedetails = promise.routename;
                }
                else {
                    swal("No Records Found")
                }
            })
        }
        //$scope.cnt12 = function ()
        //{
        //    
        //    if ($scope.count12 == true)
        //    {
        //        $scope.regorname_map = false;
        //    }
        //}

        $scope.onclickloaddata = function () {


            if ($scope.allorindiv == "All") {

                $scope.myForm.$setPristine();
                $scope.myForm.$setUntouched();
                $scope.regnonamestd = true;
                $scope.reporsmart = false;

            }
            else if ($scope.allorindiv === "indi") {
                $scope.myForm.$setPristine();
                $scope.myForm.$setUntouched();
                $scope.regnonamestd = false;
                $scope.reporsmart = false;

            }
            $scope.students = [];
        };

        $scope.toggleAll = function () {
            var toggleStatus = $scope.all;
            $scope.printstudents = [];
            angular.forEach($scope.students, function (itm) {
                itm.selected = toggleStatus;
                if (itm.selected == true) {
                    if ($scope.printstudents.indexOf(itm) === -1) {
                        $scope.printstudents.push(itm);
                    }
                }
                //else {
                //    $scope.printstudents.splice(itm);
                //}
            });
            //if ($scope.students.length === 0 && $scope.printstudents.length > 0) {
            //    angular.forEach($scope.printstudents, function (itm) {
            //        $scope.printstudents.splice(itm);
            //    });
            //}
        }

        $scope.toggleAll1 = function () {
            var toggleStatus1 = $scope.all1;
            $scope.printstudents = [];
            angular.forEach($scope.countsts, function (itm) {
                itm.selected1 = toggleStatus1;
                if (itm.selected1 == true) {
                    if ($scope.printstudents.indexOf(itm) === -1) {
                        $scope.printstudents.push(itm);
                    }
                }
                //else {
                //    $scope.printstudents.splice(itm);
                //}
            });
            //if ($scope.countsts.length === 0 && $scope.printstudents.length > 0) {
            //    angular.forEach($scope.printstudents, function (itm) {
            //        $scope.printstudents.splice(itm);
            //    });
            //}
        }

        $scope.searchValue = '';

        $scope.filterValue = function (obj) {
            return angular.lowercase(obj.amsT_FirstName).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
                 angular.lowercase(obj.trmR_PickRouteName).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
                 angular.lowercase(obj.trmL_PickLocationName).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
                 angular.lowercase(obj.trmR_DropRouteName).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
                 angular.lowercase(obj.trmL_DropLocationName).indexOf(angular.lowercase($scope.searchValue)) >= 0;
        }

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.getreport = function (obj) {
            $scope.studentss = [];
            $scope.students = [];
            $scope.presentCountgrid1 = 0;
            $scope.presentCountgrid = 0;
            $scope.all = "";
            $scope.searchValue = '';
            // $scope.toggleAll();
            // $scope.students = [];
            if ($scope.myForm.$valid) {


                var data =
                    {
                        //"validreport": $scope.regorname_map,
                        "asmaY_Id": $scope.asmaY_Id,
                        "trmR_Id": $scope.trmR_Id,
                        "ASTA_ApplStatus": $scope.sts1,
                        "regorname_map": $scope.regorname_map,
                        "Paidnotpaid": $scope.Paidnotpaid
                    }
                apiService.create("CLGRouteStatusReport/Getreportdetails", data).
                    then(function (promise) {
 
                      //  $scope.allandfalse = true;
                      
                     if ($scope.count12 == true) {
                         if (promise.countlist.length == 0) {
                             swal("Record Not Found !!");
                             $state.reload();
                         }
                         else {
                             
                             $scope.countsts = promise.countlist;
                             $scope.presentCountgrid1 = $scope.countsts.length;
                             $scope.exp_excel_flag = false;
                             $scope.print_flag = false;
                             $scope.counttrue = true;
                             $scope.allandfalse = false;
                         }

                     } else {
                         if (promise.messagelist == null) {
                             $scope.reporsmart = false;
                             swal("Record Not Found !!");
                             $state.reload();
                         }
                         else {
                             
                             $scope.allandfalse = true;
                             $scope.counttrue = false;
                             $scope.studentss = promise.messagelist;

                             for (var i = 0; i < $scope.studentss.length; i++) {

                                 if ($scope.studentss[i].ASTACO_PickUp_TRMR_Id != 0 && $scope.studentss[i].ASTACO_Drop_TRMR_Id != 0) {
                                     $scope.studentss[i].onetwoway = 'Two Way';
                                 }
                                 else if ($scope.studentss[i].ASTACO_Drop_TRMR_Id != 0 && $scope.studentss[i].ASTACO_PickUp_TRMR_Id == 0) {
                                     $scope.studentss[i].onetwoway = 'One Way';
                                 }
                                 else if ($scope.studentss[i].ASTACO_Drop_TRMR_Id == 0 && $scope.studentss[i].ASTACO_PickUp_TRMR_Id != 0) {
                                     $scope.studentss[i].onetwoway = 'One Way';
                                 }
                             }
                             for (var i = 0; i < $scope.studentss.length; i++) {

                                 if ($scope.studentss[i].ASTACO_Amount == 0) {
                                     $scope.studentss[i].paidornotpaid = 'Not Paid';
                                 }
                                 else if ($scope.studentss[i].ASTACO_Amount != 0) {
                                     $scope.studentss[i].paidornotpaid = 'Paid';
                                 }
                                
                             }

                             $scope.students = $scope.studentss;
                             //$scope.routename = $scope.students[0].trmR_RouteName;
                             $scope.presentCountgrid = $scope.students.length;
                             $scope.exp_excel_flag = false;
                             $scope.print_flag = false;

                             angular.forEach($scope.routedetails, function (ff){
                                 if ($scope.trmR_Id == ff.trmR_Id) {
                                     $scope.routename = ff.trmR_RouteName;
                                 }
                             })
                             if ($scope.trmR_Id == 0 || $scope.trmR_Id == '' || $scope.trmR_Id == undefined) {
                                 $scope.routename = "ALL";
                             }
                         }
                     }
                 })
            }

            else {
                $scope.submitted = true;
            }

        }


        $scope.printstudents = [];
        $scope.printData = function () {

            if ($scope.printstudents !== null && $scope.students.length > 0) {
                var innerContents = "";
                innerContents = document.getElementById("printareaId").innerHTML;
                var popupWinindow = window.open('');
                popupWinindow.document.open();
                popupWinindow.document.write('<html><head>' +
              '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
          '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
           '<link type="text/css" media="print" href="css/print/preadmission/AdmissionReports.css" rel="stylesheet" />' +
          '</head> <body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100000);">' + innerContents + '</html>'
          );
                popupWinindow.document.close();
            }

            else {
                swal("Please Select Records to be Printed");
            }
        }

        $scope.exportToExcel = function () {
          //  var excelname = angular.lowercase($scope.routename) + ' Route Status Report.xls';
          //var  excelnamefirst = excelname.substring(0, 1);
          //excelname = angular.uppercase(excelnamefirst) + excelname.substring(1, excelname.length);

          var excelname = $scope.routename.toLowerCase();
          excelname = excelname.substring(0, 1).toUpperCase() + excelname.substring(1) + ' Route Status Report.xls';
            var printSectionId = '#table12';
            if ($scope.printstudents !== null && $scope.printstudents.length > 0) {
                var exportHref = Excel.tableToExcel(printSectionId, 'Route Status Report');
                $timeout(function () {
                    var a = document.createElement('a');
                    a.href = exportHref;
                    a.download = excelname;
                    document.body.appendChild(a);
                    a.click();
                    a.remove();
                }, 100);
                //$state.reload();
            }
            else {
                swal("Please Select Records to be Exported");
            }

        }

        $scope.validreport = function () {
            if ($scope.regorname_map == "new") {

                $scope.myForm.$setPristine();
                $scope.myForm.$setUntouched();
                $scope.regnonamestd = true;
                $scope.reporsmart = false;

            }
            else if ($scope.regorname_map === "regular") {
                $scope.myForm.$setPristine();
                $scope.myForm.$setUntouched();
                $scope.regnonamestd = false;
                $scope.reporsmart = false;

            }
            else if ($scope.regorname_map === "both") {
                $scope.myForm.$setPristine();
                $scope.myForm.$setUntouched();
                $scope.regnonamestd = false;
                $scope.reporsmart = false;

            }

            $scope.students = [];

        }

        $scope.optionToggled = function (SelectedStudentRecord, index) {
            
            $scope.all = $scope.students.every(function (itm)
            { return itm.selected; });
            if ($scope.printstudents.indexOf(SelectedStudentRecord) === -1) {
                $scope.printstudents.push(SelectedStudentRecord);
            }
            else {
                $scope.printstudents.splice($scope.printstudents.indexOf(SelectedStudentRecord), 1);
            }

        }




    };

})();