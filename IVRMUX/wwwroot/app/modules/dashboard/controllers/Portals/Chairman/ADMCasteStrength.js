
   
     (function () {
         'use strict';
         angular
     .module('app')
     .controller('ADMCasteStrengthController', ADMCasteStrengthController)

         ADMCasteStrengthController.$inject = ['$rootScope', '$scope', '$state', '$location', 'dashboardService', 'Flash', 'apiService', '$stateParams', '$filter', 'superCache', '$window']
         function ADMCasteStrengthController($rootScope, $scope, $state, $location, dashboardService, Flash, apiService, $stateParams, $filter, superCache, $window) {

             $scope.castedetails = [];

             $scope.searchValue = "";
            
         

             var paginationformasters;
             var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
             if (ivrmcofigsettings.length > 0) {
                 paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
             }
             $scope.masterlist = false;
             $scope.currentPage = 1;
             $scope.itemsPerPage = paginationformasters;
             $scope.searchValue = "";
             if ($scope.itemsPerPage == undefined) {
                 $scope.itemsPerPage = 15
             }
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
       
        
                 apiService.getDATA("ADMCasteStrength/Getdetails").
      then(function (promise) {
          
         
          $scope.yearlt = promise.yearlist;
        


      })

    }
  
   

    $scope.OnAcdyear = function (asmaY_Id) {
        $scope.asmcL_Id = '';
        var a = $scope.asmaY_Id;
       // alert(asmaY_Id)
        $scope.fields();

        apiService.getURI("ADMCasteStrength/getclass", asmaY_Id).
      then(function (promise) {
          $scope.classarray = promise.classarray;
      })
       

    }

    $scope.interacted = function (field) {
        return $scope.submitted;
    };

   
    $scope.loadchart = function () {
       
        var cast_list = [];
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
                labelAngle: -20,
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


    $scope.showstudentGrid = function (castid) {
        
        var data = {
            "asmcL_Id": $scope.asmcL_Id,
            "ASMAY_Id": $scope.asmaY_Id,
            "asmS_Id": $scope.asmS_Id,
            "castid": castid,
        }
        var config = {
            headers: {
                'Content-Type': 'application/json;'
            }
        }
        apiService.create("ADMCasteStrength/Getstudentdetails", data).
    then(function (promise) {

        
        $scope.studentlist = promise.studentlist;



    })
        
    }


    $scope.OnClass = function (asmcL_Id) {
        //alert($scope.type)
        $scope.asmS_Id = '';
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
        apiService.create("ADMCasteStrength/Getsection", data).
      then(function (promise) {

          
          $scope.section = promise.fillsection;
          
         

      })


    }
   
    
    $scope.sort = function (keyname) {
        $scope.sortKey = keyname;   //set the sortKey to the param passed
        $scope.reverse = !$scope.reverse; //if true make it false and vice versa
    }

    $scope.showreport = function () {
        
        $scope.indattendance = false;
        $scope.submitted = true;
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
            apiService.create("ADMCasteStrength/Getreport", data).
          then(function (promise) {

              
         
              if (promise.castedetails!=null) {
                      $scope.indattendance = true;
                      $scope.castedetails = promise.castedetails;
                      $scope.loadchart();
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