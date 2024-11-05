
   
     (function () {
         'use strict';
         angular
     .module('app')
     .controller('ChairmanFeeAudcntController', ChairmanFeeAudcntController)

         ChairmanFeeAudcntController.$inject = ['$rootScope', '$scope', '$state', '$location', 'dashboardService', 'Flash', 'apiService', '$stateParams', '$filter', 'superCache', '$window']
         function ChairmanFeeAudcntController($rootScope, $scope, $state, $location, dashboardService, Flash, apiService, $stateParams, $filter, superCache, $window) {


             $scope.showgrid = false;
             $scope.booktype = 'C';
             $scope.searchValue = '';
    
             $scope.totalregstudent = 0;
             $scope.itemsPerPage = 15;
             $scope.currentPage = 1;

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
                 apiService.create("ChairmanFeeAudcnt/Getdetails", data).
                     then(function (promise) {

                       

                         $scope.totalyearfees = promise.fillgroupfee;

                         if (promise.fillgroupfee !== null && promise.fillgroupfee.length>0) {
                             $scope.showgrid = true;
                             $scope.totalyearfees = promise.fillgroupfee;
                             $scope.loadcharts();
                             
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
                     "MI_Id": classid.MI_Id,

                 }
                 var config = {
                     headers: {
                         'Content-Type': 'application/json;'
                     }
                 }
                 apiService.create("ChairmanFeeAudcnt/Getsectionpop", data).
                     then(function (promise) {



                         $scope.sectionarray1 = promise.fillgroupfee;


                         $scope.grnlist = [];
                         $scope.Typelist = [];

                         angular.forEach($scope.sectionarray1, function (ss) {

                             if ($scope.grnlist.length === 0) {
                                 $scope.grnlist.push({
                                     FYP_Id: ss.FYP_Id,
                                 });
                             }
                             else if ($scope.grnlist.length > 0) {
                                 var al_exm_cnt = 0;
                                 angular.forEach($scope.grnlist, function (exm) {
                                     if (exm.FYP_Id === ss.FYP_Id) {
                                         al_exm_cnt += 1;
                                     }
                                 });
                                 if (al_exm_cnt === 0) {
                                     $scope.grnlist.push({
                                         FYP_Id: ss.FYP_Id,
                                     });
                                 }
                             }


                         });


                         console.log($scope.grnlist);

                         $scope.mainarray = [];
                         angular.forEach($scope.grnlist, function (xx) {
                             var recno = '';
                             var bankname = '';
                             var FYP_Bank_Or_Cash = '';
                             var FYP_DD_Cheque_No = '';
                             var FYP_DD_Cheque_Date = '';
                             var FYP_Date = '';
                             var FYP_Tot_Amount = '';
                             var DeletedDate = '';
                             var FYP_Remarks = '';

                             angular.forEach($scope.sectionarray1, function (ff) {
                                 if (xx.FYP_Id === ff.FYP_Id) {
                                     DeletedDate = ff.DeletedDate;
                                     debugger;
                                     if (ff.IATD_ColumnName === 'FYP_Receipt_No') {
                                         recno = ff.IATD_PreviousValue;
                                     }
                                     if (ff.IATD_ColumnName === 'FYP_Bank_Name') {
                                         bankname = ff.IATD_PreviousValue;
                                     }
                                     if (ff.IATD_ColumnName === 'FYP_Bank_Or_Cash') {
                                         FYP_Bank_Or_Cash = ff.IATD_PreviousValue;
                                     }
                                     if (ff.IATD_ColumnName === 'FYP_DD_Cheque_No') {
                                         FYP_DD_Cheque_No = ff.IATD_PreviousValue;
                                     }
                                     if (ff.IATD_ColumnName === 'FYP_DD_Cheque_Date') {
                                         FYP_DD_Cheque_Date = ff.IATD_PreviousValue;
                                     }
                                     if (ff.IATD_ColumnName === 'FYP_Date') {

                                         FYP_Date = ff.IATD_PreviousValue;
                                     }
                                     if (ff.IATD_ColumnName === 'FYP_Tot_Amount') {
                                         FYP_Tot_Amount = ff.IATD_PreviousValue;
                                     }
                                     if (ff.IATD_ColumnName === 'FYP_Remarks') {
                                         FYP_Remarks = ff.IATD_PreviousValue;
                                     }

                                 }

                             });
                             $scope.mainarray.push({ FYP_Id: xx.FYP_Id, FYP_Receipt_No: recno, FYP_Bank_Name: bankname, FYP_Bank_Or_Cash: FYP_Bank_Or_Cash, FYP_DD_Cheque_No: FYP_DD_Cheque_No, FYP_DD_Cheque_Date: FYP_DD_Cheque_Date, FYP_Date: FYP_Date, FYP_Tot_Amount: FYP_Tot_Amount, FYP_Remarks: FYP_Remarks, DeletedDate: DeletedDate });
                         });

                         console.log($scope.mainarray);
                     });

             };

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
        if ($scope.totalyearfees != null) {

            for (var i = 0; i < $scope.totalyearfees.length; i++) {
                $scope.datagraph.push({ label: $scope.totalyearfees[i].MI_Name, "y": $scope.totalyearfees[i].FYPRecordsCount })
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

            apiService.create("ChairmanFeeAudcnt/ondatechange", data).
          then(function (promise) {
              
              $scope.totalyearfees = promise.fillgroupfee;
              if (promise.fillgroupfee !== null && promise.fillgroupfee.length > 0) {
                  $scope.showgrid = true;
                  $scope.totalyearfees = promise.fillgroupfee;
                  $scope.loadcharts();

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