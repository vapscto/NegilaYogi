
   
     (function () {
         'use strict';
         angular
     .module('app')
     .controller('Ch_DatewiseAttendanceController', Ch_DatewiseAttendanceController)

         Ch_DatewiseAttendanceController.$inject = ['$rootScope', '$scope', '$state', '$location', 'dashboardService', 'Flash', 'apiService', '$stateParams', '$filter', 'superCache', '$window']
         function Ch_DatewiseAttendanceController($rootScope, $scope, $state, $location, dashboardService, Flash, apiService, $stateParams, $filter, superCache, $window) {

             $scope.castedetails = [];

             $scope.indattendance = false;
             $scope.searchValue = "";
            
             var cast_list = [];

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
       
        
                 apiService.getDATA("Ch_DatewiseAttendance/Getdetails").
      then(function (promise) {
          
         
          $scope.yearlt = promise.yearlist;
        


      })

    }
  
   

    $scope.OnAcdyear = function (asmaY_Id) {
        $scope.asmcL_Id = '';
        var a = $scope.asmaY_Id;
       // alert(asmaY_Id)
        $scope.fields();

        apiService.getURI("Ch_DatewiseAttendance/getclass", asmaY_Id).
      then(function (promise) {
          $scope.classarray = promise.classarray;
      })
       

    }

    $scope.interacted = function (field) {
        return $scope.submitted;
    };

   
    


 



    $scope.OnClass = function (asmcL_Id) {
        $scope.asmS_Id = '';
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
        apiService.create("Ch_DatewiseAttendance/Getsection", data).
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
        if ($scope.myForm.$valid) {
            $scope.attendencelist = [];
            var data = {
                "asmcL_Id": $scope.asmcL_Id,
                "ASMAY_Id": $scope.asmaY_Id,
                "asmS_Id": $scope.asmS_Id,
                "condition": $scope.condition,
                "value": $scope.rangevalue,
                "fromdate": $scope.FMCB_fromDATE,
                "todate": $scope.FMCB_toDATE,
            }
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            apiService.create("Ch_DatewiseAttendance/Getreport", data).
          then(function (promise) {

              
         
              if (promise.attendencelist!=null) {
                      $scope.indattendance = true;
                      $scope.attendencelist = promise.attendencelist;
                     
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