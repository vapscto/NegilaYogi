
   
     (function () {
         'use strict';
         angular
     .module('app')
     .controller('AllFeeCollectionController', AllFeeCollectionController)

         AllFeeCollectionController.$inject = ['$rootScope', '$scope', '$state', '$location', 'dashboardService', 'Flash', 'apiService', '$stateParams', '$filter', 'superCache', '$window']
         function AllFeeCollectionController($rootScope, $scope, $state, $location, dashboardService, Flash, apiService, $stateParams, $filter, superCache, $window) {

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
       
        
        apiService.getDATA("AllFeeCollection/Getdetails").
      then(function (promise) {
          
         
          
          $scope.totalyearfees = promise.sectionwisestrenth;
          
          $scope.totalyearfees = promise.sectionwisestrenth;
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
              //if ($scope.totalyearfees.length > 0) {

              //    for (var i = 0; i < $scope.totalyearfees.length; i++) {
              //       // rectotal = rectotal + $scope.totalyearfees[i].recived;
              //        colltotal = colltotal + $scope.totalyearfees[i].TotalAmount;
              //       // constotal = constotal + $scope.totalyearfees[i].concession;
              //       // baltotal = baltotal + $scope.totalyearfees[i].ballance;
              //    }
              //}



              $scope.grnlist = [];

              angular.forEach($scope.totalyearfees, function (ss) {




                  if ($scope.grnlist.length === 0) {
                      $scope.grnlist.push({
                          MI_Id: ss.MI_Id,
                          MI_Name: ss.MI_Name

                      });
                  }
                  else if ($scope.grnlist.length > 0) {
                      var al_exm_cnt = 0;
                      angular.forEach($scope.grnlist, function (exm) {
                          if (exm.MI_Id === ss.MI_Id) {
                              al_exm_cnt += 1;
                          }
                      });
                      if (al_exm_cnt === 0) {
                          $scope.grnlist.push({
                              MI_Id: ss.MI_Id,
                              MI_Name: ss.MI_Name

                          });
                      }
                  }
              });

              angular.forEach($scope.grnlist, function (dd) {
                  var totalamt = 0;
                  angular.forEach($scope.totalyearfees, function (vv) {
                      if (vv.MI_Id == dd.MI_Id) {
                          totalamt = totalamt + vv.TotalAmount;
                          colltotal = colltotal + vv.TotalAmount;
                      }

                  })

                  dd.TotalAmount = totalamt;

              })




             // $scope.rectotal = rectotal;
              $scope.colltotal = colltotal;
           //   $scope.constotal = constotal;
           //   $scope.baltotal = baltotal;

              
              console.log($scope.totalyearfees);

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
        apiService.create("AllFeeCollection/Getsectioncount", data).
      then(function (promise) {

          

          $scope.sectionarray = promise.sectionarray;





      })

    }
   
    $scope.loadcharts = function () {
        var total = 0;
        var total1 = 0;
       

        $scope.totalregstudent = total;



        $scope.totalnewstudent = total1;



        
        if ($scope.grnlist != null) {

            for (var i = 0; i < $scope.grnlist.length; i++) {
                $scope.datagraph.push({ label: $scope.grnlist[i].MI_Name, "y": $scope.grnlist[i].TotalAmount })
            }
        }
        

     
        var chart = new CanvasJS.Chart("rangeBarChat", {
            height: 350,
            width:1030,
            axisX: {
                labelFontSize: 10,
                interval: 1,
                labelAngle: -20,
                // title:"Class",
            },
            axisY: {
                labelFontSize: 10,
              
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

       

        //var chart = new CanvasJS.Chart("columnchart",
        //{
        //    height: 350,
        //    width: 1030,
        //    axisX: {
        //        labelFontSize: 12,
        //        labelAngle: -20,
        //        //interval: 1,
        //        //title: "Designation",
        //    },
        //    axisY: {
        //        labelFontSize: 12,
        //        // title: "No.of. Staffs",

        //    },

        //    data: [
        //  {
        //      type: "column",
        //      showInLegend: true,
        //      dataPoints: $scope.newadmissionstdtotal
        //  }
        //    ]
        //});

        //chart.render();


        


    }

  
   

    $scope.OnAcdyear = function (asmaY_Id) {
        $scope.tablegraph = false;
        $scope.tablegrid = false;
        $scope.submitted = true;
        if ($scope.myForm.$valid) {
        var a = $scope.asmaY_Id;
      //  alert(asmaY_Id)
        $scope.fields();

        apiService.getURI("AllFeeCollection/getdata", asmaY_Id).
      then(function (promise) {
          $scope.yearlt = promise.yearlist;
          $scope.grdyear = promise.selectedyear;

          $scope.asmaY_Id = promise.asmaY_Id;

          $scope.yr = $scope.grdyear[0].asmaY_Year;

          $scope.totalyearfees = promise.sectionwisestrenth;
          if ($scope.totalyearfees != null) {
              $scope.tablegraph = true;
              $scope.tablegrid = true;
              
              // alert($scope.yr)
              $scope.newadmstdgraph = promise.newadmstd;
              
              var rectotal = 0;
              var colltotal = 0;
              var constotal = 0;
              var baltotal = 0;
              //if ($scope.totalyearfees.length > 0) {

              //    for (var i = 0; i < $scope.totalyearfees.length; i++) {
              //     //   rectotal = rectotal + $scope.totalyearfees[i].recived;
              //        colltotal = colltotal + $scope.totalyearfees[i].TotalAmount;
              //     //   constotal = constotal + $scope.totalyearfees[i].concession;
              //       // baltotal = baltotal + $scope.totalyearfees[i].ballance;
              //    }
              //}



              $scope.grnlist = [];

              angular.forEach($scope.totalyearfees, function (ss) {




                  if ($scope.grnlist.length === 0) {
                      $scope.grnlist.push({
                          MI_Id: ss.MI_Id,
                          MI_Name: ss.MI_Name

                      });
                  }
                  else if ($scope.grnlist.length > 0) {
                      var al_exm_cnt = 0;
                      angular.forEach($scope.grnlist, function (exm) {
                          if (exm.MI_Id === ss.MI_Id) {
                              al_exm_cnt += 1;
                          }
                      });
                      if (al_exm_cnt === 0) {
                          $scope.grnlist.push({
                              MI_Id: ss.MI_Id,
                              MI_Name: ss.MI_Name

                          });
                      }
                  }
              });

              angular.forEach($scope.grnlist, function (dd) {
                  var totalamt = 0;
                  angular.forEach($scope.totalyearfees, function (vv) {
                      if (vv.MI_Id == dd.MI_Id) {
                          totalamt = totalamt + vv.TotalAmount;
                          colltotal = colltotal + vv.TotalAmount;
                      }

                  })

                  dd.TotalAmount = totalamt;

              })

           //   $scope.rectotal = rectotal;
              $scope.colltotal = colltotal;
           //   $scope.constotal = constotal;
          //    $scope.baltotal = baltotal;


             

              $scope.loadcharts();

          }
          else {
              swal("No REcord Found")
          }
         
      })
       
        }
        else {
            $scope.submitted = true;
        }
    }



   
         };
     })();