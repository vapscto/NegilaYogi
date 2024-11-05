(function () {
    'use strict';
    angular
.module('app')
        .controller('TRApplDetailsController', TRApplDetailsController)
    TRApplDetailsController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', 'Excel', '$timeout', 'superCache', '$filter']
    function TRApplDetailsController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, Excel, $timeout, superCache, $filter) {
        $scope.sortKey = "astA_Id";
        $scope.columnsTest = [];
        $scope.obj = {};
        $scope.tadprint = false;
        $scope.exp_excel_flag = true;
        $scope.print_flag = true;
        $scope.printstudents = [];
        $scope.count12 = false;
        $scope.datecheck = false;
        $scope.usrname = localStorage.getItem('username');


        //var id = 1;
        //$scope.itemsPerPage = 10;
        //$scope.currentPage = 1;
        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));

        var paginationformasters;
        var copty;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_ReportPagination;
            copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
        }
        $scope.ddate = new Date();
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
            apiService.getDATA("TRApplDetails/getdata").then(function (promise) {
                if (promise != null) {
                    $scope.yearList = promise.yearList;
                      
                }
                else {
                    swal("No Records Found")
                }
            })
        }

        $scope.cancel = function () {
            //$scope.searchValue = '';
            //$scope.frmdate = '';
            //$scope.todate = '';
            //$scope.griddata = [];
            //$scope.griddeatails = false;
            //$scope.griddata.length = 0;
            $state.reload();
            
        }

        $scope.searchValue = '';
        $scope.griddeatails = false;


        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        $scope.griddata = [];
       
        $scope.sortBy = function (propertyName) {
            $scope.reverse = ($scope.propertyName === propertyName) ? !$scope.reverse : false;
            $scope.propertyName = propertyName;
        };
        $scope.printstudents = [];
        $scope.userselect = "";
        $scope.toggleAll = function () {
            debugger;
            var toggleStatus = $scope.userselect;
            $scope.printstudents = [];
            angular.forEach($scope.griddata, function (itm) {
                itm.selected = toggleStatus;
                if (itm.selected == true) {
                    if ($scope.printstudents.indexOf(itm) === -1) {
                        $scope.printstudents.push(itm);
                    }
                }
            });

        }
       
        $scope.optionToggled = function (SelectedStudentRecord, index) {

            $scope.userselect = $scope.griddata.every(function (itm) { return itm.selected; });
            if ($scope.printstudents.indexOf(SelectedStudentRecord) === -1) {
                $scope.printstudents.push(SelectedStudentRecord);
            }
            else {
                $scope.printstudents.splice($scope.printstudents.indexOf(SelectedStudentRecord), 1);
            }
        }
        $scope.gpname = '';
        $scope.type = 'APL'
        $scope.cdeposit = [];
        $scope.getreport = function () {
            $scope.griddata = [];
            $scope.gpname = '';
          
            $scope.searchValue = '';
            if ($scope.myForm.$valid) {
              
                    var data = {
                        "ASMAY_Id": $scope.ASMAY_Id,
                        "TYPE": $scope.type,
                    }
              
                
                apiService.create("TRApplDetails/Getreportdetails", data).
                 then(function (promise) {
                     debugger;
                     if (promise.griddata.length>0) {
                         $scope.griddata = promise.griddata;

                         angular.forEach($scope.yearList, function (ll) {
                             if (ll.asmaY_Id == $scope.asmaY_Id) {
                                 $scope.gpname = ll.asmaY_Year;
                             }
                         })




                         $scope.griddeatails = true;
                       

                     }
                     else {
                         $scope.reporsmart = false;
                         swal("Record Not Found !!");
                         $state.reload();
                     }
                    
                 })
            }
            else {
                $scope.submitted = true;
            }
        }

        $scope.transtypechange = function () {
            $scope.griddeatails = false;
            $scope.griddata = [];
        }
        $scope.msg = '';
        $scope.sendmsg = function () {
            //$scope.searchValue = '';
            //if ($scope.myForm.$valid) {
            debugger;
            angular.forEach($scope.printstudents, function (ff) {
                if (ff.AMST_MobileNo == "" || ff.AMST_MobileNo == null) {
                    ff.AMST_MobileNo = 0;
                }

            })

                var data = {
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "TYPE": $scope.type,
                    "MSG": $scope.msg,
                    "smscheck": $scope.sms,
                    "emailcheck": $scope.email,
                    "Temp_sms_List": $scope.printstudents
                }


            apiService.create("TRApplDetails/sendmsg", data).
                    then(function (promise) {
                        debugger;
                        swal("Message sent Successfully !!");
                        $state.reload();
                        if (promise.griddata.length > 0) {
                            $scope.griddata = promise.griddata;

                            angular.forEach($scope.yearList, function (ll) {
                                if (ll.asmaY_Id == $scope.asmaY_Id) {
                                    $scope.gpname = ll.asmaY_Year;
                                }
                            })




                            $scope.griddeatails = true;


                        }
                        else {
                            $scope.reporsmart = false;
                            swal("Record Not Found !!");
                            $state.reload();
                        }

                    })
            //}
            //else {
            //    $scope.submitted = true;
            //}
        }


        $scope.printData = function () {

            var innerContents = document.getElementById("printareaId").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
           '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
       '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
        '<link type="text/css" media="print" href="css/print/preadmission/RegReportPdf.css" rel="stylesheet" />' +
       '</head> <body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 1000);">' + innerContents + '</html>'
       );
            popupWinindow.document.close();

        }

        $scope.exportToExcel = function (tabel1) {

            var exportHref = Excel.tableToExcel(tabel1, 'sheet name');
            $timeout(function () { location.href = exportHref; }, 100);
        }

        $scope.validreport = function () {

            $scope.students = [];

        }
    };

})();