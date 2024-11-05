
   
     (function () {
         'use strict';
         angular
     .module('app')
     .controller('ADMClassSectionStrengthController', ADMClassSectionStrengthController)

         ADMClassSectionStrengthController.$inject = ['$rootScope', '$scope', '$state', '$location', 'dashboardService', 'Flash', 'apiService', '$stateParams', '$filter', 'superCache', '$window']
         function ADMClassSectionStrengthController($rootScope, $scope, $state, $location, dashboardService, Flash, apiService, $stateParams, $filter, superCache, $window) {

             $scope.studentstrenthgr = false;
             $scope.totalregstudent = 0;

             $scope.totalnewstudent = 0;
             $scope.sms = 0;
             $scope.email = 0;
             $scope.regular = [];
             $scope.fields = function () {
                 
                 $scope.newadmissionstdtotal = [];
                 $scope.datagraph = [];
                 $scope.regularstdtotal = [];
                 $scope.newadmstdgraphdta = [];
              
               
                 $scope.Todaydate = new Date();
             }

             $scope.withtc = false;
             $scope.withdeactive = false;

             $scope.interacted = function (field) {
                 return $scope.submitted;
             };

             $scope.Binddata = function () {
        $scope.fields();
        $scope.studentstrenthgr = false;
        
        apiService.getDATA("ADMClassSectionStrength/Getdetails").
      then(function (promise) {
          
          $scope.studentstrenth = promise.classarray;


          if ($scope.studentstrenth.length > 0) {
              $scope.studentstrenthgr = true;

             
              $scope.yearlt = promise.yearlist;
              $scope.regular = promise.sectionwisestrenth;

              $scope.asmaY_Id = promise.asmaY_Id;

              //added for new requirment
              $scope.newdata = [];
              angular.forEach($scope.studentstrenth, function (xx) {
                  var std_cnt = 0;
                  angular.forEach($scope.regular, function (dd) {

                      if (xx.asmcL_Id == dd.classid) {
                          std_cnt += dd.stud_count;
                      }

                  })

                  $scope.newdata.push({ class_Name: xx.class_Name, stud_count: std_cnt, classid: xx.asmcL_Id, asmayid: $scope.asmaY_Id });
              })


              $scope.studentstrenth = $scope.newdata;

              console.log($scope.studentstrenth);


              $scope.loadcharts();

          }
          else {
              swal("No Record Found")
          }
       



      })

    }

             $scope.showsectionGrid = function (classid) {
                 
                 //alert(asmayid)
                 //alert(classid)


                 var data = {
                     "classid": classid,
                     "ASMAY_Id": $scope.asmaY_Id,
                     "withtc": $scope.withtc,
                     "withdeactive": $scope.withdeactive,
                    
                 }
                 var config = {
                     headers: {
                         'Content-Type': 'application/json;'
                     }
                 }
                 apiService.create("ADMClassSectionStrength/Getsectioncount", data).
               then(function (promise) {

                   
                   
                   $scope.fillsectioncount = promise.fillsectioncount;
                  




               })

             }
   
    $scope.loadcharts = function () {
        var total = 0;
        var total1 = 0;
        

        if ($scope.regular != null) {

            for (var i = 0; i < $scope.regular.length; i++) {
                $scope.regularstdtotal.push({ label: $scope.regular[i].class_Name + '-' + $scope.regular[i].sectionname, "y": $scope.regular[i].stud_count })
            }
        }



      


        

        if ($scope.sectioncount != null) {

            for (var i = 0; i < $scope.sectioncount.length; i++) {
                $scope.newadmstdgraphdta.push({ label: $scope.sectioncount[i].section, "y": $scope.sectioncount[i].stud_count })
            }
        }

      


        var chart = new CanvasJS.Chart("areachart",
        {
            width: 1070,
            height:348,
            axisX: {
                labelFontSize: 6,
                interval: 1,
                labelFontColor: "black",
                labelAngle: -20 ,
                title: "CLASS-SECTION",
                labelFormatter: function () {
                    return " ";
                }
            },
            axisY: {
                labelFontSize: 12,
                // title: "No.of. Staffs",
                labelFontColor: "black",

            },

            data: [
          {
              type: "column",
              showInLegend: true,
                    dataPoints: $scope.regularstdtotal,
                    color:"#3399ff",
          }
            ]
        });

        chart.render();

    }


             $scope.changetc = function () {
                 debugger;
                 $scope.OnAcdyear();
             }
             $scope.changetc1 = function () {
                 debugger;
                 $scope.OnAcdyear();
             }


   

    $scope.OnAcdyear = function (asmaY_Id) {
        $scope.regularstdtotal = [];
        $scope.studentstrenthgr = false;
        $scope.submitted = true;
        if ($scope.myForm.$valid) {
        var a = $scope.asmaY_Id;
       // alert(asmaY_Id)
        $scope.fields();
            var data = {
               "withtc": $scope.withtc,
              "withdeactive": $scope.withdeactive,
                "ASMAY_Id": a,
            }
            apiService.create("ADMClassSectionStrength/getclass", data).
      then(function (promise) {
          $scope.studentstrenth = promise.classarray;
          if ($scope.studentstrenth.length > 0 ) {
    
        
          $scope.studentstrenthgr = true;
        
          // $scope.yearlt = promise.yearlist;
          $scope.regular = promise.sectionwisestrenth;

          $scope.asmaY_Id = promise.asmaY_Id;


              //added for new requirment
              $scope.newdata = [];
              angular.forEach($scope.studentstrenth, function (xx) {
                  var std_cnt = 0;
                  angular.forEach($scope.regular, function (dd) {

                      if (xx.asmcL_Id == dd.classid) {
                          std_cnt += dd.stud_count;
                      }

                  })

                  $scope.newdata.push({ class_Name: xx.class_Name, stud_count: std_cnt, classid: xx.asmcL_Id, asmayid: $scope.asmaY_Id });
              })


              $scope.studentstrenth = $scope.newdata;
          $scope.loadcharts();
         
      }
        else {
              swal("No Record Found")
        }
       
      })
    }
else {
            $scope.submitted = true;
            $scope.studentstrenthgr = false;
}

    }
    $scope.OnClass = function (asmcL_Id) {

        var a = $scope.asmcL_Id;
        // alert(asmaY_Id)
        $scope.fields();
        var data = {
            "classid": asmcL_Id,
            "ASMAY_Id": $scope.asmaY_Id,
            "withtc": $scope.withtc,
            "withdeactive": $scope.withdeactive,
        }
        var config = {
            headers: {
                'Content-Type': 'application/json;'
            }
        }
        apiService.create("ADMClassSectionStrength/Getsection", data).
      then(function (promise) {

          
         // $scope.sectioncount = promise.fillstudentstrenth;
         // $scope.regular = promise.sectionwisestrenth;
          // alert($scope.regular)
          // $scope.yearlt = promise.yearlist;
          // $scope.asmaY_Id = promise.yearlist[0].asmaY_Id;
          //$scope.regstdtotal = promise.fillregstd;
          //$scope.newadmstdtotal = promise.fillnewadmstd;
          //$scope.asmaY_Id = promise.asmaY_Id;
          //$scope.newadmstdgraph = promise.newadmstd;
          //$scope.year = promise.yearlist[0].asmaY_Year;

          //
          //$scope.classarray = promise.classarray;
          //$scope.sectionarray = promise.sectionarray;
          //$scope.newadmit = promise.sectionwisestrenth;

            $scope.loadcharts();

      })


    }


    
   
         };
     })();