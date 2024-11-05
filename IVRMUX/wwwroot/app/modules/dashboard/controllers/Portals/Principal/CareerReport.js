  
     (function () {
         'use strict';
         angular
     .module('app')
             .controller('CareerReportController', CareerReportController)

         CareerReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'dashboardService', 'Flash', 'apiService', '$stateParams', '$filter', 'superCache', '$window', 'Excel','$timeout']
         function CareerReportController($rootScope, $scope, $state, $location, dashboardService, Flash, apiService, $stateParams, $filter, superCache, $window, Excel, $timeout) {

             var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));

             var paginationformasters;
             var copty;
             var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
             if (ivrmcofigsettings.length > 0) {
                 paginationformasters = ivrmcofigsettings[0].ivrmgC_ReportPagination;
                 copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
             }
             $scope.usrname = localStorage.getItem('username');
             $scope.itemsPerPage = paginationformasters;
             if ($scope.itemsPerPage == undefined || $scope.itemsPerPage == null) {
                 $scope.itemsPerPage =15;
             }
             $scope.currentPage = 1;
             $scope.coptyright = copty;
             $scope.ddate = new Date();
             var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
             if (admfigsettings.length > 0) {
                 var logopath = admfigsettings[0].asC_Logo_Path;
             }
             $scope.imgname = logopath;
             $scope.showgrid = false;
             $scope.searchValue = '';
             $scope.totalregstudent = 0;

             $scope.totalnewstudent = 0;
             $scope.sms = 0;
             $scope.email = 0;
             $scope.fields = function () {
                 
                 $scope.newadmissionstdtotal = [];
                 $scope.datagraph = [];
                 $scope.regularstdtotal = [];
                 $scope.newadmstdgraphdta = [];
              
               
                 $scope.Todaydate = new Date();
    }

             $scope.interacted = function (field) {
                 return $scope.submitted;
             };
             $scope.maxDatemf = new Date();
             //loading start
    $scope.loadbasicdata = function () {
        $scope.fields();
        $scope.classfee = [];
        
        $scope.FMCB_fromDATE = new Date();
        $scope.FMCB_toDATE = new Date();
        var frmdate = $scope.FMCB_fromDATE == null ? "" : $filter('date')($scope.FMCB_fromDATE, "yyyy-MM-dd");
        var todate = $scope.FMCB_toDATE == null ? "" : $filter('date')($scope.FMCB_toDATE, "yyyy-MM-dd");

        var data = {
          //  "fromdate": $scope.FMCB_fromDATE,
          //  "todate": $scope.FMCB_toDATE,   
            "fromdate": frmdate,
            "todate": todate,

        }
        var config = {
            headers: {
                'Content-Type': 'application/json;'
            }
        }
        apiService.create("CareerReport/Getdetails",data).
      then(function (promise) {
          
         
          
          $scope.groupclass = promise.fillgroupfee;
          $scope.fillhead = promise.fillhead;
          if ($scope.groupclass != null) {
            

              $scope.showgrid = true;
          }
          else {
              swal("No Record Found");
          }
      
        





      })

    }

             $scope.GetReport = function () {
                 $scope.showgrid = false;
                 $scope.reportdata = [];
                 if ($scope.myForm.$valid) {
        var frmdate = $scope.FMCB_fromDATE == null ? "" : $filter('date')($scope.FMCB_fromDATE, "yyyy-MM-dd");
        var todate = $scope.FMCB_toDATE == null ? "" : $filter('date')($scope.FMCB_toDATE, "yyyy-MM-dd");
        var data = {
            "fromdate": frmdate,
            "todate": todate,
        }
        var config = {
            headers: {
                'Content-Type': 'application/json;'
            }
        }
                     apiService.create("CareerReport/getalldetails", data).
      then(function (promise) {
          $scope.reportdata = promise.reportdata;
          if ($scope.reportdata.length > 0) {




              $scope.showgrid = true;
          }
          else {
              swal('NO RECORD FOUND')
          }

       





                     })

             }
            else {
                 $scope.submitted = true;
             }
    
}

    $scope.gettodate = function () {
        
        $scope.minDatemf = new Date($scope.fromdate);
        $scope.maxDatemf = new Date();
    };
   
   

             $scope.printData = function () {

                 var innerContents = document.getElementById("printareaId").innerHTML;
                 var popupWinindow = window.open('');
                 popupWinindow.document.open();
                 popupWinindow.document.write('<html><head>' +
                     '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                     '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
                     '<link type="text/css" media="print" href="css/print/preadmission/RegReportPdf.css" rel="stylesheet" />' +
                     '</head> <body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); },900);">' + innerContents + '</html>'
                 );
                 popupWinindow.document.close();

             }

             $scope.exportToExcel = function (tabel1) {

                 var exportHref = Excel.tableToExcel(tabel1, 'sheet name');
                 $timeout(function () { location.href = exportHref; }, 900);
             }
  
   

    $scope.onfromdatechange = function () {
       
     
        $scope.showgrid=false;
        $scope.submitted = true;
        if ($scope.myForm.$valid) {
            $scope.fields();
            var frmdate = $scope.FMCB_fromDATE == null ? "" : $filter('date')($scope.FMCB_fromDATE, "yyyy-MM-dd");
            var todate = $scope.FMCB_toDATE == null ? "" : $filter('date')($scope.FMCB_toDATE, "yyyy-MM-dd");
            var data = {
                "fromdate": frmdate,
                "todate": todate,

            }
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }

            apiService.create("CareerReport/ondatechange", data).
          then(function (promise) {
              
              $scope.groupclass = promise.fillgroupfee;
              if ($scope.groupclass !=null) {
                  $scope.showgrid = true;
              }
              else {
                  swal("No Record Found");
              }




             
          })
        } else {
            $scope.submitted = true;

        }

    }



   
         };
     })();
