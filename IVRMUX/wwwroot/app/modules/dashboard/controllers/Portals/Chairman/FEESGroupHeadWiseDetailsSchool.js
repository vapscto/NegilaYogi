   
     (function () {
         'use strict';
         angular
     .module('app')
     .controller('FEESGroupHeadWiseDetailsSchoolController', FEESGroupHeadWiseDetailsSchoolController)

         FEESGroupHeadWiseDetailsSchoolController.$inject = ['$rootScope', '$scope', '$state', '$location', 'dashboardService', 'Flash', 'apiService', '$stateParams', '$filter', 'superCache', '$window']
         function FEESGroupHeadWiseDetailsSchoolController($rootScope, $scope, $state, $location, dashboardService, Flash, apiService, $stateParams, $filter, superCache, $window) {



    
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
             $scope.tabgridgraph = false;
             $scope.tabgrid = false;
    $scope.loadbasicdata = function () {
        $scope.fields();
       
        
        apiService.getDATA("FEESGroupHeadWiseDetailsSchool/Getdetails").
      then(function (promise) {
          
         
          $scope.yearlt = promise.yearlist;
          $scope.groupfee = promise.fillgroupfee;
          $scope.grdyear = promise.selectedyear;

          $scope.asmaY_Id = promise.asmaY_Id;
          $scope.fillhead = promise.fillhead;
          if ($scope.groupfee !=null) {
              $scope.tabgridgraph = true;
              $scope.tabgrid = true;
           
            

              $scope.yr = $scope.grdyear[0].asmaY_Year;

              $scope.newadmstdgraph = promise.newadmstd;
          



              $scope.loadcharts();
          }
         
          else {
              swal("No Record Found")
          }




      })

    }

    $scope.showgroupGrid = function (groupid) {
        $scope.groupclass = [];
        var data = {
            "groupid": groupid,
            "ASMAY_Id": $scope.asmaY_Id,

        }
        var config = {
            headers: {
                'Content-Type': 'application/json;'
            }
        }
        apiService.create("FEESGroupHeadWiseDetailsSchool/Getgroupclasscount", data).
      then(function (promise) {

          

          $scope.groupclass = promise.groupclass;





      })
    
}

    $scope.showsectionGrid = function (headid) {
        $scope.sectionarray = [];
       // alert($scope.asmaY_Id);
        var data = {
            "headid": headid,
            "ASMAY_Id": $scope.asmaY_Id,

        }
        var config = {
            headers: {
                'Content-Type': 'application/json;'
            }
        }
        apiService.create("FEESGroupHeadWiseDetailsSchool/Getsectioncount", data).
      then(function (promise) {

          

          $scope.sectionarray = promise.sectionarray;





      })

    }
   
    $scope.loadcharts = function () {
        var total = 0;
        var total1 = 0;
     

        $scope.totalregstudent = total;



        $scope.totalnewstudent = total1;


        //    alert(total);

        
        if ($scope.groupfee != null) {

            for (var i = 0; i < $scope.groupfee.length; i++) {
                $scope.datagraph.push({ label: $scope.groupfee[i].groupname, "y": $scope.groupfee[i].paid })
            }
        }
       

        if ($scope.fillhead != null) {

            for (var i = 0; i < $scope.fillhead.length; i++) {
                $scope.newadmissionstdtotal.push({ label: $scope.fillhead[i].headname, "y": $scope.fillhead[i].paid })
            }
        }





       

        columnchart
        var chart = new CanvasJS.Chart("rangeBarChat", {
            height: 350,
            width: 1060,
            axisX: {
                labelFontSize: 9,
                interval: 1,
                labelAngle: -20 ,
                 labelFontColor: "black",
                labelFontWeight: "bold", 
                // title:"Class",
            },
            axisY: {
                labelFontSize: 9,
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
            width: 1060,

            axisX: {
                labelFontSize: 11,
                interval: 1,
                labelAngle: -20 ,
              labelFontColor: "black",
               labelFontWeight: "bold", 
                //title: "Designation",
            },
            axisY: {
                labelFontSize: 9,
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

  
    $scope.interacted = function (field) {
        return $scope.submitted;
    };

    
    $scope.OnAcdyear = function (asmaY_Id) {
        $scope.tabgridgraph=false;
        $scope.tabgrid=false;
        $scope.submitted = true;
        if ($scope.myForm.$valid) {
        var a = $scope.asmaY_Id;
      
        $scope.fields();

        apiService.getURI("FEESGroupHeadWiseDetailsSchool/getdata", asmaY_Id).
      then(function (promise) {
          
          $scope.groupfee = promise.fillgroupfee;
          $scope.yearlt = promise.yearlist;
          $scope.grdyear = promise.selectedyear;

          $scope.asmaY_Id = promise.asmaY_Id;

          $scope.yr = $scope.grdyear[0].asmaY_Year;
          $scope.fillhead = promise.fillhead;
        
          if ($scope.groupfee !=null) {
              $scope.tabgridgraph=true;
              $scope.tabgrid=true;
              

              $scope.newadmstdgraph = promise.newadmstd;
       



              $scope.loadcharts();
          }
          else {
    swal("No Record found")
          }
        
      })
       
    }
else {
            $scope.submitted = true;
}
}

    }



   
         //};
     })();