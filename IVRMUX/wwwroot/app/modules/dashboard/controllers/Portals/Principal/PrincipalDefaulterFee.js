
(function () {
    'use strict';
    angular
        .module('app')
        .controller('PrincipalDefaulterFeeController', PrincipalDefaulterFeeController)
    PrincipalDefaulterFeeController.$inject = ['$rootScope', '$scope', '$state', '$location', 'dashboardService', 'Flash', 'apiService', '$stateParams', '$filter', 'superCache', '$window']
    function PrincipalDefaulterFeeController($rootScope, $scope, $state, $location, dashboardService, Flash, apiService, $stateParams, $filter, superCache, $window ) {

//    dashboard.controller("PrincipalDefaulterFeeController", ['$rootScope', '$scope', '$state', '$location', 'dashboardService', 'Flash', 'apiService', '$stateParams', '$filter', 'superCache','$window',
//function ($rootScope, $scope, $state, $location, dashboardService, Flash, apiService, $stateParams, $filter, superCache, $window ) {


        $scope.tadprint = false;
    $scope.castedetails = [];

    $scope.searchValue = "";

    var cast_list = [];

    var paginationformasters;
    var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
    if (ivrmcofigsettings.length > 0) {
        paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }

        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }
        $scope.imgname = logopath;
    $scope.masterlist = false;
    $scope.currentPage = 1;
    $scope.itemsPerPage = paginationformasters;
    $scope.searchValue = "";
    if ($scope.itemsPerPage == undefined) {
        $scope.itemsPerPage = 15
        }


        $scope.ddate = new Date();
        $scope.usrname = localStorage.getItem('username');
    $scope.fields = function () {

        $scope.newadmissionstdtotal = [];
        $scope.datagraph = [];
        $scope.regularstdtotal = [];
        $scope.newadmstdgraphdta = [];


        $scope.Todaydate = new Date();
    }
    $scope.studentdrp = false;
    $scope.Binddata = function () {
        $scope.fields();

        
        apiService.getDATA("PrincipalDefaulterFee/Getdetails").
then(function (promise) {
    

    $scope.yearlt = promise.yearlist;

})

    }



    $scope.OnAcdyear = function (asmaY_Id) {

        var a = $scope.asmaY_Id;
        // alert(asmaY_Id)
        $scope.fields();

        apiService.getURI("PrincipalDefaulterFee/getclass", asmaY_Id).
      then(function (promise) {
          $scope.classarray = promise.classarray;
      })


    }

    $scope.interacted = function (field) {
        return $scope.submitted;
    };


    $scope.loadchart = function () {


        if ($scope.castedetails != null) {
            
            for (var i = 0; i < $scope.castedetails.length; i++) {
                cast_list.push({ label: $scope.castedetails[i].caste, "y": $scope.castedetails[i].total })
            }
        }

        var chart = new CanvasJS.Chart("columnchart", {
            height: 350,
            width: 1075,
            axisX: {
                labelFontSize: 12,
                interval: 1,
            },
            axisY: {
                labelFontSize: 12,
            },

            data: [
            {
                type: "column",
                showInLegend: true,
                dataPoints: cast_list
            }
            ]
        });

        chart.render();
    }


    $scope.showstudentGrid = function () {

        var data = {
                
            "ASMAY_Id": $scope.asmaY_Id,
               
    
        }
        var config = {
            headers: {
                'Content-Type': 'application/json;'
            }
        }
        apiService.create("PrincipalDefaulterFee/Getstudentdetails", data).
    then(function (promise) {
            
        $scope.Student = [];
        $scope.studdata = promise.fillstudentstrenth;
        $scope.studentbalance = promise.studbal;
            angular.forEach($scope.studentbalance, function (s2) {
                    $scope.Student.push({ classname: s2.class_Name, stball: s2.balance })
            })
    })
    }



    $scope.OnClass = function (asmcL_Id) {
        //alert($scope.type)
        $scope.asmcL_Id = asmcL_Id;
        // alert(asmaY_Id)
        $scope.fields();
        var data = {
            "asmcL_Id": asmcL_Id,
            "ASMAY_Id": $scope.asmaY_Id,
        }
        var config = {
            headers: {
                'Content-Type': 'application/json;'
            }
        }
        apiService.create("PrincipalDefaulterFee/Getsection", data).
      then(function (promise) {

          
          $scope.section = promise.fillsection;
      })
    }


    $scope.sort = function (keyname) {
        $scope.sortKey = keyname;   //set the sortKey to the param passed
        $scope.reverse = !$scope.reverse; //if true make it false and vice versa
    }

        $scope.showreport = function () {
            $scope.yrname = '';
            $scope.clname = '';
            $scope.secname = '';

        if ($scope.myForm.$valid) {
            var data = {
                "asmcL_Id": $scope.asmcL_Id,
                "ASMAY_Id": $scope.asmaY_Id,
                "asmS_Id": $scope.asmS_Id,
            }
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            apiService.create("PrincipalDefaulterFee/Getreport", data).
          then(function (promise) {
              
              if (promise.studbal.length > 0) {
                  $scope.indattendance = true;
                  $scope.studentlist = promise.studbal;
                  angular.forEach($scope.yearlt, function (gg) {

                      if ($scope.asmaY_Id == gg.asmaY_Id) {
                          $scope.yrname = gg.asmaY_Year
                      }
                  })
                  angular.forEach($scope.classarray, function (dd) {

                      if ($scope.asmcL_Id == dd.asmcL_Id) {
                          $scope.clname = dd.class_Name
                      }
                  })

                  angular.forEach($scope.section, function (geg) {

                      if ($scope.asmS_Id == geg.asmS_Id) {
                          $scope.secname = geg.sectionname
                      }
                  })
              }
              else {
                  swal("No Record Found")
              }
          })
        }
        else {
            $scope.submitted = true;
        }

    }



    $scope.showdefaulter = function () {
        
        if ($scope.myForm.$valid) {
            var data = {
                "asmcL_Id": $scope.asmcL_Id,
                "ASMAY_Id": $scope.asmaY_Id,
                "asmS_Id": $scope.asmS_Id,
            }
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            apiService.create("PrincipalDefaulterFee/Getsection", data).
          then(function (promise) {
              
              if (promise.Fillstudentstrenth.length > 0) {
                  $scope.indattendance = true;
                  $scope.Fillstudentstrenth = promise.Fillstudentstrenth;
                  $scope.fillfee = promise.fillfee;



                  //   $scope.loadchart();
              }
              else {
                  swal("No Record Found")
              }
          })
        }
        else {
            $scope.submitted = true;
        }
    }




    $scope.printToCart2 = function () {

        var innerContents = document.getElementById("tbl2").innerHTML;
        var popupWinindow = window.open('');
        popupWinindow.document.open();
        //popupWinindow.document.write('<html><head>' +
        //     '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
        //      '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/ProgressReport/BGIProgressReportPdf.css" />' +
        //  '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
        //'</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
        //popupWinindow.document.close();
        popupWinindow.document.write('<html><head>' +
            '<link type="text/css" media="print" rel="stylesheet" href="css/print.css" />' +
            '<link type="text/css" rel="stylesheet" href="css/style.css" />' +
            '<link type="text/css" href="plugins/bootstrap/css/bootstrap.min.css" rel="stylesheet" />' +
            '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
        popupWinindow.document.close();
        $state.reload();
    }

    $scope.printToCart1 = function () {
        var innerContents = document.getElementById("tbl1").innerHTML;
        var popupWinindow = window.open('');
        popupWinindow.document.open();
        popupWinindow.document.write('<html><head>' +
             '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
              '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/ProgressReport/BGIProgressReportPdf.css" />' +
          '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
        '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
        popupWinindow.document.close();

    } 
        }
})();