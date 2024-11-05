(function () {
    'use strict';
    angular
.module('app')
        .controller('StudentRouteMappingReportController', StudentRouteMappingReportController)
    StudentRouteMappingReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', 'Excel', '$timeout', 'superCache', '$filter']
    function StudentRouteMappingReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, Excel, $timeout, superCache, $filter) {
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
        $scope.ddate = new Date();
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
        $scope.routetypelist = [];
        $scope.vh =false;
        $scope.FMG_Id =0;
        $scope.BindData = function () {
            apiService.getDATA("StudentRouteMappingReport/getdata").then(function (promise) {
                if (promise != null) {
                    $scope.YearList = promise.yearList;
                    $scope.routedetails = promise.routename;
                    $scope.seclist = promise.seclist;
                    $scope.classlist = promise.classlist;
                    $scope.grouplist = promise.grouplist;
                    $scope.sessionlist = promise.sessionlist;


                    $scope.routetypelist = [];
                    $scope.routetypelist.push({ id: 0, type: 'ALL' })
                    $scope.routetypelist.push({ id: 1, type: 'TWO WAY' })
                    $scope.routetypelist.push({ id: 2, type: 'ONE WAY -PICK UP' })
                    $scope.routetypelist.push({ id: 3, type: 'ONE WAY -DROP' })
                   
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

        $scope.allgroupcheck = function () {

            if ($scope.allcheck == true) {
                angular.forEach($scope.routetypelist, function (obj) {
                    obj.selected = true;
                   
                });
            }
            else {
                angular.forEach($scope.routetypelist, function (obj) {
                    obj.selected = false;
                });
            }

        }

        $scope.optionToggledGF = function () {

            $scope.allcheck = $scope.routetypelist.every(function (itm) { return itm.selected; })

        }

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

                debugger;
                var data =
                    {
                        //"validreport": $scope.regorname_map,
                        "asmaY_Id": $scope.asmaY_Id,
                        "trmR_Id": $scope.trmR_Id,
                        "ASMCL_Id": $scope.asmcL_Id,
                        "ASMS_Id": $scope.asmS_Id,
                        "ASMS_Id": $scope.asmS_Id,
                        "ASTA_ApplStatus": $scope.routetype,
                        "FMG_Id": $scope.FMG_Id,
                        "feeflag": $scope.vh,
                        "TRMS_Id": $scope.TRMS_Id,
                    }
                apiService.create("StudentRouteMappingReport/Getreportdetails", data).
                    then(function (promise) {
                        debugger;
                     
                    
                        if (promise.messagelist == null || promise.messagelist==0) {
                             $scope.reporsmart = false;
                             swal("Record Not Found !!");
                            
                         }
                         else {
                             
                             $scope.allandfalse = true;
                             $scope.counttrue = false;
                             $scope.studentss = promise.messagelist;

                             for (var i = 0; i < $scope.studentss.length; i++) {

                                 if ($scope.studentss[i].trmR_Idd != 0 && $scope.studentss[i].trmR_Idp != 0) {
                                     $scope.studentss[i].onetwoway = 'Two Way';
                                 }
                                 else if ($scope.studentss[i].trmR_Idd != 0 && $scope.studentss[i].trmR_Idp == 0) {
                                     $scope.studentss[i].onetwoway = 'One Way';
                                 }
                                 else if ($scope.studentss[i].trmR_Idd == 0 && $scope.studentss[i].trmR_Idp != 0) {
                                     $scope.studentss[i].onetwoway = 'One Way';
                                 }
                             }
                             for (var i = 0; i < $scope.studentss.length; i++) {

                                 if ($scope.studentss[i].astA_Amount == 0) {
                                     $scope.studentss[i].paidornotpaid = 'Not Paid';
                                 }
                                 else if ($scope.studentss[i].astA_Amount != 0) {
                                     $scope.studentss[i].paidornotpaid = 'Paid';
                                 }
                                
                             }

                             $scope.students = $scope.studentss;
                             $scope.routename = $scope.students[0].trmR_RouteName;
                             $scope.presentCountgrid = $scope.students.length;
                             $scope.exp_excel_flag = false;
                             $scope.print_flag = false;

                             angular.forEach($scope.routedetails, function (ff){
                                 if ($scope.trmR_Id == ff.trmR_Id) {
                                     $scope.routename = ff.trmR_RouteName;
                                 }
                             })
                             debugger;
                             if ($scope.trmR_Id==0) {
                                 $scope.routename = 'ALL ROUTE';
                             }
                            // alert($scope.routename)
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
          '</head> <body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 133000);">' + innerContents + '</html>'
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
            if ($scope.routename == undefined || $scope.routename == null || $scope.routename=='') {
                $scope.routename = 'ALL ROUTE';
            }
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