(function () {
    'use strict';

    angular
        .module('app')
        .controller('HODFeesCollectionController', HODFeesCollectionController);

    HODFeesCollectionController.$inject = ['$rootScope', '$scope', '$state', '$location', 'dashboardService', 'Flash', 'apiService', '$stateParams', '$filter', 'superCache', '$window'];

    function HODFeesCollectionController($rootScope, $scope, $state, $location, dashboardService, Flash, apiService, $stateParams, $filter, superCache, $window) {

        $scope.tablegraph = false;
        $scope.tablegrid = false;


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

        $scope.loadbasicdata = function () {
            $scope.fields();

            
            apiService.getDATA("HODFeesCollection/Getdetails").
          then(function (promise) {
              


              $scope.totalyearfees = promise.fillfee;

              $scope.totalyearfees = promise.fillfee;
              if ($scope.totalyearfees != null) {
                  $scope.yearlt = promise.yearlist;
                  $scope.grdyear = promise.selectedyear;

                  $scope.asmaY_Id = promise.asmaY_Id;

                  $scope.yr = $scope.grdyear[0].asmaY_Year;
                  //  alert($scope.yr)
                  $scope.newadmstdgraph = promise.newadmstd;
                  
                  var rectotal = 0;
                  var colltotal = 0;
                  var constotal = 0;
                  var baltotal = 0;
                  if ($scope.totalyearfees.length > 0) {

                      for (var i = 0; i < $scope.totalyearfees.length; i++) {
                          rectotal = rectotal + $scope.totalyearfees[i].recived;
                          colltotal = colltotal + $scope.totalyearfees[i].paid;
                          constotal = constotal + $scope.totalyearfees[i].concession;
                          baltotal = baltotal + $scope.totalyearfees[i].ballance;
                      }
                  }

                  $scope.rectotal = rectotal;
                  $scope.colltotal = colltotal;
                  $scope.constotal = constotal;
                  $scope.baltotal = baltotal;


                  $scope.yearfee = [];


                  $scope.yearfee.push({ yr: $scope.yr, r: $scope.rectotal, c: $scope.colltotal, cn: $scope.constotal, b: $scope.baltotal })









                  $scope.loadcharts();
                  $scope.tablegraph = true;
                  $scope.tablegrid = true;

              }
              else {
                  swal("No Record Found");
              }



          })

        }

        $scope.showsectionGrid = function (classid) {

            //alert($scope.asmaY_Id);
            var data = {
                "classid": classid,
                "ASMAY_Id": $scope.asmaY_Id,

            }
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            apiService.create("HODFeesCollection/Getsectioncount", data).
          then(function (promise) {

              

              $scope.sectionarray = promise.sectionarray;





          })

        }

        $scope.loadcharts = function () {
            var total = 0;
            var total1 = 0;


            $scope.totalregstudent = total;



            $scope.totalnewstudent = total1;




            if ($scope.totalyearfees != null) {

                for (var i = 0; i < $scope.totalyearfees.length; i++) {
                    $scope.datagraph.push({ label: $scope.totalyearfees[i].feeclass, "y": $scope.totalyearfees[i].paid })
                }
            }



            if ($scope.yearfee != null) {

                for (var i = 0; i < $scope.yearfee.length; i++) {
                    $scope.newadmissionstdtotal.push({ label: $scope.yearfee[i].yr, "y": $scope.yearfee[i].c })
                }
            }






            columnchart
            var chart = new CanvasJS.Chart("rangeBarChat", {
                height: 350,
                axisX: {
                    labelFontSize: 12,
                    interval: 1,
                    // title:"Class",
                },
                axisY: {
                    labelFontSize: 12,
                    //  title: "Students",
                },

                data: [
                {
                    type: "column",
                    showInLegend: true,
                    dataPoints: $scope.datagraph

                }
                ]

            });

            chart.render();



            var chart = new CanvasJS.Chart("columnchart",
            {
                height: 350,
                axisX: {
                    labelFontSize: 12,
                    interval: 1,
                    //title: "Designation",
                },
                axisY: {
                    labelFontSize: 12,
                    // title: "No.of. Staffs",

                },

                data: [
              {
                  type: "column",
                  showInLegend: true,
                  dataPoints: $scope.newadmissionstdtotal
              }
                ]
            });

            chart.render();





        }




        $scope.OnAcdyear = function (asmaY_Id) {
            $scope.tablegraph = false;
            $scope.tablegrid = false;
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var a = $scope.asmaY_Id;
                //  alert(asmaY_Id)
                $scope.fields();

                apiService.getURI("HODFeesCollection/getdata", asmaY_Id).
              then(function (promise) {


                  $scope.totalyearfees = promise.fillfee;
                  if ($scope.totalyearfees != null) {
                      $scope.tablegraph = true;
                      $scope.tablegrid = true;
                      $scope.yearlt = promise.yearlist;
                      $scope.grdyear = promise.selectedyear;

                      $scope.asmaY_Id = promise.asmaY_Id;

                      $scope.yr = $scope.grdyear[0].asmaY_Year;
                      // alert($scope.yr)
                      $scope.newadmstdgraph = promise.newadmstd;
                      
                      var rectotal = 0;
                      var colltotal = 0;
                      var constotal = 0;
                      var baltotal = 0;
                      if ($scope.totalyearfees.length > 0) {

                          for (var i = 0; i < $scope.totalyearfees.length; i++) {
                              rectotal = rectotal + $scope.totalyearfees[i].recived;
                              colltotal = colltotal + $scope.totalyearfees[i].paid;
                              constotal = constotal + $scope.totalyearfees[i].concession;
                              baltotal = baltotal + $scope.totalyearfees[i].ballance;
                          }
                      }

                      $scope.rectotal = rectotal;
                      $scope.colltotal = colltotal;
                      $scope.constotal = constotal;
                      $scope.baltotal = baltotal;


                      $scope.yearfee = [];


                      $scope.yearfee.push({ yr: $scope.yr, r: $scope.rectotal, c: $scope.colltotal, cn: $scope.constotal, b: $scope.baltotal })

                      $scope.loadcharts();

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




    };
})();
