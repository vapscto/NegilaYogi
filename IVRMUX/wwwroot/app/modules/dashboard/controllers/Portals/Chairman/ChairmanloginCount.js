
   
     (function () {
         'use strict';
         angular
     .module('app')
     .controller('ChairmanloginCountController', ChairmanloginCountController)

         ChairmanloginCountController.$inject = ['$rootScope', '$scope', '$state', '$location', 'dashboardService', 'Flash', 'apiService', '$stateParams', '$filter', 'superCache', '$window']
         function ChairmanloginCountController($rootScope, $scope, $state, $location, dashboardService, Flash, apiService, $stateParams, $filter, superCache, $window) {


             $scope.showgrid = false;
             $scope.booktype = 'C';
    
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
                 $scope.cccc = 0;
                 $scope.rcccc = 0;
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
                     "eventName": $scope.booktype,

                 }
                 var config = {
                     headers: {
                         'Content-Type': 'application/json;'
                     }
                 }
                 apiService.create("ChairmanloginCount/Getdetails", data).
                     then(function (promise) {

                       

                         $scope.totalyearfees = promise.fillgroupfee;

                         if (promise.fillgroupfee !== null && promise.fillgroupfee.length>0) {
                             $scope.showgrid = true;
                             $scope.grnlist = [];
                             $scope.Typelist = [];

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



                                 if ($scope.Typelist.length === 0) {
                                     $scope.Typelist.push({
                                         IVRMMALD_logintype: ss.IVRMMALD_logintype

                                     });
                                 }
                                 else if ($scope.Typelist.length > 0) {
                                     var al_exm_cnt1 = 0;
                                     angular.forEach($scope.Typelist, function (exm) {
                                         if (exm.IVRMMALD_logintype === ss.IVRMMALD_logintype) {
                                             al_exm_cnt1 += 1;
                                         }
                                     });
                                     if (al_exm_cnt1 === 0) {
                                         $scope.Typelist.push({

                                             IVRMMALD_logintype: ss.IVRMMALD_logintype

                                         });
                                     }
                                 }


                             });

                             console.log($scope.Typelist);
                             console.log($scope.grnlist);


                             angular.forEach($scope.grnlist, function (ee) {
                                 var cnountlist = [];
                                 angular.forEach($scope.totalyearfees, function (xx) {
                                     if (ee.MI_Id === xx.MI_Id) {
                                         cnountlist.push(xx);
                                     }

                                 });

                                 ee.LIST = cnountlist;

                             });
                         }
                         else {
                             swal('NO RECORD FOUND');
                         }
                         
                     });

             };

    $scope.showgroupGrid = function (classid) {
        $scope.sectionpop = [];
        var frmdate = $scope.FMCB_fromDATE == null ? "" : $filter('date')($scope.FMCB_fromDATE, "yyyy-MM-dd");
        var todate = $scope.FMCB_toDATE == null ? "" : $filter('date')($scope.FMCB_toDATE, "yyyy-MM-dd");
        var data = {
            "fromdate": frmdate,
            "todate": todate,
            "classid": classid,

        }
        var config = {
            headers: {
                'Content-Type': 'application/json;'
            }
        }
        apiService.create("ChairmanloginCount/Getsectionpop", data).
      then(function (promise) {

          

          $scope.sectionarray = promise.sectionarray;





      })
    
}

    $scope.gettodate = function () {
        
        $scope.minDatemf = new Date($scope.fromdate);
        $scope.maxDatemf = new Date();
    };
   
    $scope.loadcharts = function () {
        var total = 0;
        var total1 = 0;
       

        $scope.totalregstudent = total;




        $scope.totalnewstudent = total1;


        

        $scope.datagraph = [];
        if ($scope.groupclass != null) {

            for (var i = 0; i < $scope.groupclass.length; i++) {
                $scope.datagraph.push({ label: $scope.groupclass[i].class_Name, "y": $scope.groupclass[i].paid })
            }
        }
        


      




       
        var chart = new CanvasJS.Chart("rangeBarChat", {
            width: 1060,
            height:340,
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

     

      


      

    }


             $scope.cccc = 0;
             $scope.rcccc = 0;

    $scope.onfromdatechange = function () {
        $scope.cccc = 0;
        $scope.rcccc = 0;
     
        $scope.showgrid=false;
        $scope.submitted = true;
        if ($scope.myForm.$valid) {
            $scope.fields();
            var frmdate = $scope.FMCB_fromDATE == null ? "" : $filter('date')($scope.FMCB_fromDATE, "yyyy-MM-dd");
            var todate = $scope.FMCB_toDATE == null ? "" : $filter('date')($scope.FMCB_toDATE, "yyyy-MM-dd");
            var data = {
                "fromdate": frmdate,
                "todate": todate,
                "eventName": $scope.booktype,

            }
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }

            apiService.create("ChairmanloginCount/ondatechange", data).
          then(function (promise) {
              
              $scope.totalyearfees = promise.fillgroupfee;

              if (promise.fillgroupfee !== null && promise.fillgroupfee.length > 0) {
                  $scope.showgrid = true;
                  $scope.grnlist = [];
                  $scope.Typelist = [];

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



                      if ($scope.Typelist.length === 0) {
                          $scope.Typelist.push({
                              IVRMMALD_logintype: ss.IVRMMALD_logintype

                          });
                      }
                      else if ($scope.Typelist.length > 0) {
                          var al_exm_cnt1 = 0;
                          angular.forEach($scope.Typelist, function (exm) {
                              if (exm.IVRMMALD_logintype === ss.IVRMMALD_logintype) {
                                  al_exm_cnt1 += 1;
                              }
                          });
                          if (al_exm_cnt1 === 0) {
                              $scope.Typelist.push({

                                  IVRMMALD_logintype: ss.IVRMMALD_logintype

                              });
                          }
                      }


                  });

                  console.log($scope.Typelist);
                  console.log($scope.grnlist);


                  angular.forEach($scope.grnlist, function (ee) {
                      var cnountlist = [];
                      angular.forEach($scope.totalyearfees, function (xx) {
                          if (ee.MI_Id === xx.MI_Id) {
                              cnountlist.push(xx);
                          }

                      });

                      ee.LIST = cnountlist;

                  });
              }
              else {
                  swal('NO RECORD FOUND');
              }
            
          })
        } else {
            $scope.submitted = true;

        }

    }



   
         };
     })();