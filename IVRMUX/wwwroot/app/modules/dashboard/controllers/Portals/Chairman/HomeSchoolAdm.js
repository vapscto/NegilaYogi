     (function () {
         'use strict';
         angular
     .module('app')
     .controller('HomeSchoolAdmController', HomeSchoolAdmController)

         HomeSchoolAdmController.$inject = ['$rootScope', '$scope', '$state', '$location', 'dashboardService', 'Flash', 'apiService', '$stateParams', '$filter', 'superCache', '$window']
         function HomeSchoolAdmController($rootScope, $scope, $state, $location, dashboardService, Flash, apiService, $stateParams, $filter, superCache,$window) {
             $scope.totalregstudent = 0;
             $scope.graph111 = false;
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
             $scope.graph = false;
             $scope.newclsgraph = false;
             $scope.regclsgraph = false;
    $scope.loadbasicdata = function () {
        $scope.fields();
        $scope.studentstrenth = [];
        $scope.regstdtotal = [];
        $scope.newadmstdtotal = [];
        $scope.newadmstdgraph = [];
        $scope.regular = [];
        $scope.classarray = [];
        $scope.sectionarray = [];
        $scope.newadmit = [];
        
        apiService.getDATA("HomeSchoolAdm/Getdetails").
      then(function (promise) {
          
          $scope.tabgrid = true;
          $scope.tabgrap = false;
        
          $scope.regclsgraph = true;
          $scope.classarray = promise.classarray;
          $scope.sectionarray = promise.sectionarray;
          $scope.regular = promise.sectionwisestrenth;
          $scope.newadmit = promise.sectionwisestrenthnewadm;
          console.log($scope.regular);

          if ($scope.regular.length>0) {
              $scope.regclsgraph = true;
          }
          if ($scope.newadmit.length > 0) {
              $scope.newclsgraph = true;
          }



          //if (promise.newadmstd == null && promise.fillstudentstrenth == null) {
          //    $scope.tabgrap = true;
          //    $scope.tabgrid = false;
          //}
          //if (promise.fillstudentstrenth !=null) {
          //    $scope.studentstrenth = promise.fillstudentstrenth;
          //  //  $scope.newclsgraph = false;
          //    $scope.regclsgraph = true;
          //}
          //if (promise.newadmstd !=null) {
          //    $scope.newadmstdgraph = promise.newadmstd;
          //    $scope.newclsgraph = true;
          //}
       
          

          $scope.yearlt = promise.yearlist;
         // $scope.asmaY_Id = promise.yearlist[0].asmaY_Id;
          $scope.regstdtotal = promise.fillregstd;
          $scope.newadmstdtotal = promise.fillnewadmstd;
          $scope.asmaY_Id = promise.asmaY_Id;
       
          $scope.year= promise.yearlist[0].asmaY_Year;
         
       

          $scope.loadcharts();
        
          $scope.graph = true;



      })

    }

   
   
    $scope.loadcharts = function () {
        $scope.totalregstudent = 0;
        $scope.totalnewstudent = 0;
        
        var total = 0;
        var total1 = 0;
        if ($scope.regular.length > 0) {

            for (var i = 0; i < $scope.regular.length; i++) {
                total = total + $scope.regular[i].stud_count;
                
            }
        }

        $scope.totalregstudent = total;



        if ($scope.newadmit.length > 0) {

            for (var i = 0; i < $scope.newadmit.length; i++) {
                total1 = total1 + $scope.newadmit[i].stud_count;
                
            }
        }

        $scope.totalnewstudent = total1;


        //    alert(total);
        $scope.studentstrenth = [];
        $scope.newdata = [];
        angular.forEach($scope.classarray, function (xx) {
            var std_cnt = 0;
            angular.forEach($scope.regular, function (dd) {

                if (xx.asmcL_Id == dd.classid) {
                    std_cnt += dd.stud_count;
                }
            })
            $scope.newdata.push({ class_Name: xx.asmcL_ClassName, stud_count: std_cnt, classid: xx.asmcL_Id });
        })


       $scope.studentstrenth = $scope.newdata;
    

        $scope.datagraph = [];
        if ($scope.studentstrenth != null) {

            for (var i = 0; i < $scope.studentstrenth.length; i++) {
                $scope.datagraph.push({ label: $scope.studentstrenth[i].class_Name, "y": $scope.studentstrenth[i].stud_count })
            }
        }
       

        $scope.newadmstdgraphdta = [];
        $scope.newdata1 = [];
        $scope.newadmstdgraph = [];
        angular.forEach($scope.classarray, function (xx) {
            var std_cnt = 0;
            angular.forEach($scope.newadmit, function (dd) {

                if (xx.asmcL_Id == dd.classid) {
                    std_cnt += dd.stud_count;
                }
            })
            $scope.newdata1.push({ class_Name: xx.asmcL_ClassName, stud_count: std_cnt, classid: xx.asmcL_Id });
        })


        $scope.newadmstdgraph = $scope.newdata1;




        
        if ($scope.newadmstdgraph != null) {

            for (var i = 0; i < $scope.newadmstdgraph.length; i++) {
                $scope.newadmstdgraphdta.push({ label: $scope.newadmstdgraph[i].class_Name, "y": $scope.newadmstdgraph[i].stud_count })
            }
        }
        $scope.regularstdtotal = [];
        if ($scope.regstdtotal != null) {

            for (var i = 0; i < $scope.regstdtotal.length; i++) {
                $scope.regularstdtotal.push({ label: $scope.regstdtotal[i].year, "y": $scope.regstdtotal[i].stud_count })
            }
        }


        $scope.newadmissionstdtotal = [];
        if ($scope.newadmstdtotal != null) {

            for (var i = 0; i < $scope.newadmstdtotal.length; i++) {
                $scope.newadmissionstdtotal.push({ label: $scope.newadmstdtotal[i].year, "y": $scope.newadmstdtotal[i].stud_count })
            }
        }


       
      
     


        //columnchart
        var chart = new CanvasJS.Chart("rangeBarChat", {
            height: 350,
            width: 1046,
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

        var chart = new CanvasJS.Chart("chartContainer",
       {
           height: 350,
           width:1046,
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
             dataPoints: $scope.newadmstdgraphdta
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


        var chart = new CanvasJS.Chart("areachart",
        {
            height: 350,
            axisX: {
                labelFontSize: 12,
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
              dataPoints: $scope.regularstdtotal
          }
            ]
        });

        chart.render();


    }

    $scope.interacted = function (field) {
        return $scope.submitted;
    };

    $scope.OnAcdyear = function (asmaY_Id) {
        $scope.newclsgraph = false;
        $scope.regclsgraph = false;
        $scope.studentstrenth = [];
        $scope.regstdtotal = [];
        $scope.newadmstdtotal = [];
        $scope.regstdtotal = [];
        $scope.newadmstdtotal = [];
        $scope.newadmstdgraph = [];
        $scope.regular = [];
        $scope.classarray = [];
        $scope.sectionarray = [];
        $scope.newadmit = [];
        $scope.graph = false;
        $scope.submitted = true;
        if ($scope.myForm.$valid) {
            var a = $scope.asmaY_Id;
            //alert(asmaY_Id)
            $scope.fields();
            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
                "withtc": $scope.withtc,
                "withdeactive": $scope.withdeactive,
            }
            apiService.create("HomeSchoolAdm/getdata", data).
                then(function (promise) {
                 
              $scope.tabgrid = true;
           ;
             
              $scope.tabgrap = true;
              $scope.graph = true;
                    $scope.tabgrid = true;
                    $scope.tabgrap = false;

                    $scope.regclsgraph = true;
                    $scope.classarray = promise.classarray;
                    $scope.sectionarray = promise.sectionarray;
                    $scope.regular = promise.sectionwisestrenth;
                    $scope.newadmit = promise.sectionwisestrenthnewadm;
                    console.log($scope.regular);

                    if ($scope.regular.length > 0) {
                        $scope.regclsgraph = true;
                    }
                    if ($scope.newadmit.length > 0) {
                        $scope.newclsgraph = true;
                    }
             
              $scope.yearlt = promise.yearlist;
              // $scope.asmaY_Id = promise.yearlist[0].asmaY_Id;
              if (promise.fillregstd !=null) {
                  $scope.regstdtotal = promise.fillregstd;

              }
              if (promise.fillnewadmstd !=null) {
                  $scope.newadmstdtotal = promise.fillnewadmstd;
              }
             
              $scope.asmaY_Id = promise.asmaY_Id;

             
              
              $scope.year = promise.yearlist[0].asmaY_Year;
             
             
         
             
             
              
              $scope.loadcharts();
              $scope.graph = true;

          })

        }
        else {
            $scope.submitted = true;
            $scope.graph = false;
        }
    }
             $scope.changetc = function () {
                 debugger;
                 $scope.OnAcdyear();
             }
             $scope.changetc1 = function () {
                 debugger;
                 $scope.OnAcdyear();
             }



   
         };
     })();