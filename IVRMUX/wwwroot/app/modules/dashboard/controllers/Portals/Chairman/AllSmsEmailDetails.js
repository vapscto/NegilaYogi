(function () {
    'use strict';
    angular
.module('app')
        .controller('AllSmsEmailDetailsController', AllSmsEmailDetailsController)

    AllSmsEmailDetailsController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', 'Excel', '$timeout', 'superCache', '$filter']
    function AllSmsEmailDetailsController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, Excel, $timeout, superCache, $filter) {
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
       
        $scope.issuertype1 = 'SMS';
        //var id = 1;
        //$scope.itemsPerPage = 10;
        //$scope.currentPage = 1;
        $scope.yerlist = [{ yearid: 1, name: '2016', start: new Date('01/01/1016'), end: new Date('12/31/1016') },
        { yearid: 2, name: '2017', start: new Date('01/01/2017'), end: new Date('12/31/2017') },
        { yearid: 3, name: '2018', start: new Date('01/01/2018'), end: new Date('12/31/2018') },
        { yearid: 4, name: '2019', start: new Date('01/01/2019'), end: new Date('12/31/2019') },
        { yearid: 5, name: '2020', start: new Date('01/01/2020'), end: new Date('12/31/2020') },
        { yearid: 6, name: '2021', start: new Date('01/01/2021'), end: new Date('12/31/2021') },
        { yearid: 7, name: '2022', start: new Date('01/01/2022'), end: new Date('12/31/2022') },
        { yearid: 8, name: '2023', start: new Date('01/01/2023'), end: new Date('12/31/2023') },
        { yearid: 9, name: '2024', start: new Date('01/01/2024'), end: new Date('12/31/2024') }]
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

        $scope.tadprint = false;
       
        $scope.ddate = new Date();
        $scope.usrname = localStorage.getItem('username');

        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey == key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        }

        $scope.BindData = function () {
            apiService.getDATA("SmsEmailDetails/getdata").then(function (promise) {
                if (promise != null) {
                    $scope.monthlist = promise.monthlist;
                    $scope.smsmodulelist = promise.smsmodulelist;
                    $scope.YearList = promise.yearList;
                    $scope.termlist = promise.termlist;
                   
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


        $scope.cdeposit = [];
        $scope.getreport = function () {
            debugger;
            var startDateTime = new Date();
            var endDatetime = new Date();
            $scope.all = "";
            $scope.searchValue = '';
            if ($scope.myForm.$valid) {
              
                if ($scope.issuertype2 == 'mnw') {
                    angular.forEach($scope.yerlist, function (ff) {

                        if (ff.yearid == $scope.asmay_id) {

                            angular.forEach($scope.monthlist, function (zz) {

                                if (zz.ivrM_Month_Name.trim() == $scope.ivrM_Month_Name.trim()) {
                                    startDateTime = $scope.ivrM_Month_Name + '/01/' + + ff.name;
                                    endDatetime = $scope.ivrM_Month_Name + '/' + zz.ivrM_Month_Max_Days + '/' + + ff.name;

                                    startDateTime = new Date(startDateTime);
                                    endDatetime = new Date(endDatetime);
                                    startDateTime = startDateTime == null ? "" : $filter('date')(startDateTime, "yyyy-MM-dd");
                                    endDatetime = endDatetime == null ? "" : $filter('date')(endDatetime, "yyyy-MM-dd");
                                }
                            }
                            )
                        }
                    })

                }
                else {
                     startDateTime = $scope.frmdate == null ? "" : $filter('date')($scope.frmdate, "yyyy-MM-dd");
                    endDatetime = $scope.todate == null ? "" : $filter('date')($scope.todate, "yyyy-MM-dd");
                }
               

                    var data = {
                        "frmdate": startDateTime,
                        "todate": endDatetime,
                        "type": $scope.issuertype1,
                        "templete": $scope.templete,
                    }
              
                
                apiService.create("SmsEmailDetails/Getreportdetails1", data).
                 then(function (promise) {
                     debugger;
                     if (promise.griddata.length>0) {
                         $scope.griddata = promise.griddata;
                         console.log($scope.griddata);
                         $scope.griddeatails = true;
                        

                     
                     }
                     else {
                         $scope.reporsmart = false;
                         swal("Record Not Found !!");
                        // $state.reload();
                     }
                    
                 })
            }
            else {
                $scope.submitted = true;
            }
        }


        $scope.changedrdo = function () {

            $scope.griddata = [];
            $scope.griddeatails = false;
        }
        $scope.printData = function () {

            var innerContents = document.getElementById("printareaId").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
           '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
       '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
        
       '</head> <body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>'
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