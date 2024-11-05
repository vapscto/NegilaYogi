
   
     (function () {
         'use strict';
         angular
     .module('app')
     .controller('ModewiseFeeCollectionController', ModewiseFeeCollectionController)

         ModewiseFeeCollectionController.$inject = ['$rootScope', '$scope', '$state', '$location', 'dashboardService', 'Flash', 'apiService', '$stateParams', '$filter', 'superCache', '$window']
         function ModewiseFeeCollectionController($rootScope, $scope, $state, $location, dashboardService, Flash, apiService, $stateParams, $filter, superCache, $window) {

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
             $scope.columns = [];
    $scope.loadbasicdata = function () {
        $scope.fields();
       
        $scope.columns = [];
        apiService.getDATA("ModewiseFeeCollection/Getdetails").
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


              $scope.columns = [];
              var cntt = 0;
              angular.forEach($scope.totalyearfees, function (ee) {
                  if (cntt === 0) {
                      debugger;
                      var id = 0;
                      angular.forEach(ee, function (gg, jj) {
                          if (jj !== 'MI_Id' && jj !== 'MI_Name') {
                              $scope.columns.push({ id: id + 1, head: jj });
                          }

                      });
                  }
                  cntt += 1;
              });

            
            
              

             // $scope.rectotal = rectotal;
              $scope.colltotal = colltotal;
           //   $scope.constotal = constotal;
           //   $scope.baltotal = baltotal;

              
           

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
        apiService.create("ModewiseFeeCollection/Getsectioncount", data).
      then(function (promise) {

          

          $scope.sectionarray = promise.sectionarray;





      })

    }
   
    $scope.loadcharts = function () {
        var total = 0;
        var total1 = 0;
       

        $scope.totalregstudent = total;



        $scope.totalnewstudent = total1;



        

        $scope.feegraphseries1 = [];
        if ($scope.totalyearfees != null) {

            for (var i = 0; i < $scope.totalyearfees.length; i++) {
                $scope.feegraphseries1.push({ label: $scope.totalyearfees[i].MI_Name, "y": $scope.totalyearfees[i].Bank })
            }
        }


        console.log($scope.feegraphseries1);

        //function toggleDataSeries(e) {
   
        //    if (typeof (e.dataSeries.visible) === "undefined" || e.dataSeries.visible) {
        //        e.dataSeries.visible = false;
        //    }
        //    else {
        //        e.dataSeries.visible = true;
        //    }
        //    chart.render();
        //}
        var chart = new CanvasJS.Chart("rangeBarChat");
        chart.animationEnabled= true;
        chart.width= 1060;
        chart.options.axisX = {
            interval: 1, labelFontSize: 10, labelAngle: -20, labelFontColor: "black",
            labelFontWeight: "bold", labelWrap: true 
        };
        chart.options.axisY = { labelFontSize: 12 };



        //chart.options.toolTip = {
        //    shared: true
        //};
        //chart.options.legend = {
        //    cursor: "pointer",
               
        //};





        chart.options.data = [];
        var series1 = [];
      
       
        angular.forEach($scope.columns, function (ddd) {
            
             series1 = [];

            series1.push({ type: "column", name: ddd.head, showInLegend:true, legendText: ddd.head, indexLabelPlacement: "outside" });

     
           
            $scope.feegraphseries1 = [];
            angular.forEach($scope.totalyearfees, function (ww, ss) {
                debugger;

                $scope.feegraphseries1.push({ label: ww.MI_Name, "y": ww[ddd.head], legendText: ddd.head});

            });
            series1.dataPoints = $scope.feegraphseries1;
          
            chart.options.data.push(series1);
           // }
           
        })
        console.log(chart.options.data);

        chart.render();

        


        //var series3 = { //dataSeries - second quarter
        //    type: "column",
        //    name: "Balance",
        //    showInLegend: true
        //};

        //var chart = new CanvasJS.Chart("rangeBarChat", {
        //    height: 350,
        //    width:1030,
        //    axisX: {
        //        labelFontSize: 10,
        //        interval: 1,
        //        labelAngle: -20,
        //        // title:"Class",
        //    },
        //    axisY: {
        //        labelFontSize: 10,
              
        //        //  title: "Students",
        //    },

        //    data: [
        //    {
        //        type: "column",
        //        showInLegend: true,
        //        dataPoints: $scope.datagraph

        //    }
        //    ]

        //});

        //chart.render();

       

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

        apiService.getURI("ModewiseFeeCollection/getdata", asmaY_Id).
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



               $scope.columns = [];
              var cntt = 0;
              angular.forEach($scope.totalyearfees, function (ee) {
                  if (cntt === 0) {
                      debugger;
                      var id = 0;
                      angular.forEach(ee, function (gg, jj) {
                          if (jj !== 'MI_Id' && jj !== 'MI_Name') {
                              $scope.columns.push({ id: id + 1, head: jj });
                          }

                      });
                  }
                  cntt += 1;
              });


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