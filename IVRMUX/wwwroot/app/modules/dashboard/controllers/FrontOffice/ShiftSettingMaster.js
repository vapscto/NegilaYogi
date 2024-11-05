(function () {
    'use strict';
    angular
.module('app')
.controller('ShiftSettingMasterController', ShiftSettingMasterController)

    ShiftSettingMasterController.$inject = ['$rootScope', '$scope', '$state', '$location', 'apiService', 'Flash', '$http', '$q', '$stateParams', '$filter']
    function ShiftSettingMasterController($rootScope, $scope, $state, $location, apiService, Flash, $http, $q, $stateParams, $filter) {
        //$scope.timedis1 = true;
        //$scope.timedis2 = true;

        $scope.hmin = new Date();
        $scope.hmin.setHours(0);
        $scope.hmin.setMinutes(0);
     $scope.FOMST_HDWHrMin = $scope.hmin;



        var d = new Date();
        d.setHours(0);
        d.setMinutes(0);
        $scope.min = d;

        var maxsnfttim= new Date();
        maxsnfttim.setHours(18);
        maxsnfttim.setMinutes(0);
        $scope.maxtimme = maxsnfttim;

        var d2 = new Date();
        d2.setHours(7);
        d2.setMinutes(0);
        $scope.max = d2;
       
        $scope.hstep = 1;
        $scope.mstep = 1;
      
        $scope.options = {
            hstep: [1, 2, 3],
            mstep: [1, 5, 10, 15, 25, 30]
        };

        $scope.ismeridian = false;
        //$scope.toggleMode = function () {
        //    $scope.ismeridian = !$scope.ismeridian;
        //};
        $scope.validateTomintime = function (timedata) {
            
            //$scope.timedis1 = false;
            //$scope.timedis2 = true;
            $scope.FOMST_IIHalfLogoutTime = "";
            $scope.totime = timedata;
            var hh = $scope.totime.getHours();
            var mm = $scope.totime.getMinutes();
            $scope.min = timedata;

            $scope.min.setHours(hh);
            $scope.min.setMinutes(mm);
            $scope.minlnc = timedata;

            $scope.minlnc.setHours(hh);
            $scope.minlnc.setMinutes(mm);
            $scope.FOMST_IHalfLogoutTime = "";
        }
        $scope.validatemax = function (maxdata) {
            
           // $scope.FOMST_IHalfLoginTime = maxdata;
            //$scope.FOMST_IIHalfLogoutTime = "";
            var dsttimee = $scope.FOMST_IHalfLoginTime;
            $scope.sresult = $filter('date')(dsttimee, 'HH:mm:ss a');
            $scope.eresult = $filter('date')(maxdata, 'HH:mm:ss a');
            //var startTime = moment(dsttimee, "HH:mm:ss a");
            //  var endTime = moment(maxdata, "HH:mm:ss a");
            var startTime = moment($scope.sresult, "HH:mm:ss a");
            var endTime = moment($scope.eresult, "HH:mm:ss a");
            var duration = moment.duration(endTime.diff(startTime));
            var hours = parseInt(duration.asHours());
            var minutes = parseInt(duration.asMinutes()) - hours * 60;
            var finlrst = hours + ":" + minutes;

            $scope.tmin = new Date();
            $scope.tmin.setHours(hours);
            $scope.tmin.setMinutes(minutes);
            $scope.tmax = new Date();
            $scope.tmax.setHours(hours);
            $scope.tmax.setMinutes(minutes);

            $scope.ttst = new Date();
            $scope.ttst.setHours(hours);
            $scope.ttst.setMinutes(minutes);

            $scope.FOMST_FDWHrMin = $scope.ttst;
            //$scope.FOMST_HDWHrMin = $scope.ttst;

            $scope.htmax = new Date();
            $scope.htmax.setHours(hours);
            $scope.htmax.setMinutes(minutes);

            if (maxdata >= new Date($scope.FOMST_IHalfLoginTime)) {
                $scope.totimemax = maxdata;
                var hh = $scope.totimemax.getHours();
                var mm = $scope.totimemax.getMinutes();
                $scope.max = maxdata;

                $scope.max.setMinutes(hh);
                $scope.max.setMinutes(mm);
            }
            else {
                $scope.FOMST_IIHalfLogoutTime = "";
            }
         
           // $scope.FOMST_IHalfLogoutTime = "";
        }
        $scope.validatefromtime1 = function (maxdata1) {
            
            // $scope.FOMST_IHalfLoginTime = maxdata;
            //$scope.FOMST_IIHalfLogoutTime = "";
            if (maxdata1 >= new Date($scope.FOMST_IHalfLogoutTime)) {
               // $scope.totimemax1 = maxdata1;
                //var hh1 = $scope.totimemax1.getHours();
                //var mm1 = $scope.totimemax1.getMinutes();
                //$scope.max1 = maxdata1;

                //$scope.max1.setMinutes(hh);
                //$scope.max1.setMinutes(mm);
            }
            else {
                $scope.FOMST_IIHalfLoginTime = "";
            }

            // $scope.FOMST_IHalfLogoutTime = "";
        }
        $scope.validateTomintime1 = function (timedata1) {
            
            $scope.FOMST_IIHalfLoginTime = "";
            $scope.totime1 = timedata1;
            var hh = $scope.totime1.getHours();
            var mm = $scope.totime1.getMinutes();
            $scope.min1 = timedata1;

            $scope.min1.setMinutes(hh);
            $scope.min1.setMinutes(mm);
            $scope.FOMST_IIHalfLoginTime = timedata1;
        }

        //Ui Grid view rendering data from data base
        $scope.gridOptions1 = {
            
            enableColumnMenus: false,
            enableFiltering: true,
            paginationPageSizes: [5, 10, 15],
            paginationPageSize: 5,

            
            columnDefs: [
                { name: 'SNO', width: '10%', enableFiltering: false, enableSorting: false, field: 'name', cellTemplate: '<div class="ui-grid-cell-contents">{{grid.options.paginationPageSize *(grid.options.paginationCurrentPage-1)+grid.renderContainers.body.visibleRowCache.indexOf(row)+1}}</div>' },
                { name: 'fomS_ShiftName', width: '30%', displayName: 'Shift Name' },
                 { name: 'fomsT_IHalfLoginTime', displayName: 'Shift Start Time' },
                  { name: 'fomsT_IIHalfLogoutTime', displayName: 'Shift End Time' },
               {
                   field: 'id', name: '',
                   displayName: 'Actions', enableFiltering: false, enableSorting: false, cellTemplate:
                 //'<div class="grid-action-cell">' +
                 //'<a href="javascript:void(0)" ng-click="grid.appScope.getorgvalue(row.entity);"> <md-tooltip md-direction="down">Edit</md-tooltip><i class="fa fa-pencil-square-o" aria-hidden="true"></i></a>' +
                 //'<a ng-if="row.entity.ttmsaB_ActiveFlag === false" href="javascript:void(0)" style="color:green;" ng-click="grid.appScope.switch(row.entity);"><md-tooltip md-direction="down">Active Now</md-tooltip> <i class="fa fa-toggle-on text-red" aria-hidden="true"></i></a>' +
                 // '<span ng-if="row.entity.ttmsaB_ActiveFlag === true ""><a href="javascript:void(0)"  style="color:red;" ng-click="grid.appScope.switch(row.entity);"> <md-tooltip md-direction="down">Deactive Now</md-tooltip> <i class="fa fa-toggle-off text-green" aria-hidden="true"></i></a><span>' +
                 //'</div>'
                 //'<a href="javascript:void(0)" ng-click="grid.appScope.getorgvalue(row.entity);"> <i class="fa fa-pencil-square-o" ></i></a>' +
                 //'<a href="javascript:void(0)" ng-click="grid.appScope.deletedata(row.entity);"> <i class="fa fa-trash text-danger"></i></a>' +
                   //'</div>'
                    '<div class="grid-action-cell">' +
                     '<a  href="javascript:void(0)" data-toggle="modal" data-target="#popup1" data-backdrop="static" ng-click="grid.appScope.viewrecordspopup1(row.entity);"> <i class="fa fa-eye text-purple" ><md-tooltip md-direction="down">View</md-tooltip></i></a>  &nbsp; &nbsp;' +
                 '<a href="javascript:void(0)" ng-click="grid.appScope.editdata(row.entity);"> <md-tooltip md-direction="down">Edits</md-tooltip><i class="fa fa-pencil-square-o" ></i></a>  &nbsp; &nbsp;' +
                '<a ng-if="row.entity.fomS_ActiveFlg === false" href="javascript:void(0)" style="color:green;" ng-click="grid.appScope.deactive(row.entity);"> <md-tooltip md-direction="down">Activate Now</md-tooltip> <i class="fa fa-toggle-off text-red" aria-hidden="true"></i></a>' +
              '<span ng-if="row.entity.fomS_ActiveFlg === true"><a href="javascript:void(0)"  style="color:red;" ng-click="grid.appScope.deactive(row.entity);">  <md-tooltip md-direction="down">Deactivate Now</md-tooltip> <i class="fa fa-toggle-on text-green"  aria-hidden="true"></i></a><span>' +

                '</div>'
               }
            ]

        };
        //grid end        
        //validation start 
        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };
        //popup strart
        //TO  View Record
        $scope.viewrecordspopup1 = function (employee, SweetAlert) {
            
            $scope.editEmployee = employee.fomsT_Id;
            var pageid = $scope.editEmployee;

            apiService.getURI("ShiftSettingMaster/getalldetailsviewrecords1", pageid).
                    then(function (promise) {
                        
                        $scope.FOMS_ShiftName = promise.filldata[0].fomS_ShiftName;
                        $scope.viewrecordspopupdisplay1 = promise.filldata;

                    })

        };

        //TO clear  popupgrid data
        $scope.clearpopupgrid1 = function () {
            $scope.viewrecordspopupdisplay1 = "";
        };
        //popup end
        //validation end
        /* loading start*/
        $scope.loaddata = function () {
            
            $scope.currentPage = 1;
            $scope.itemsPerPage = 5;
            var pageid = 20;
          
            apiService.getURI("ShiftSettingMaster/getalldetails", pageid).
            then(function (promise) {

                $scope.disabledata = false; 
                $scope.hwtypelst = promise.filltype;
                $scope.gridOptions1.data = promise.filldata;
                $scope.gridOptions1 = promise.filldata;
            })
        }
        /* loading end*/
        /* post data start*/

        $scope.submitted = false;
        $scope.savedata = function () {
            if ($scope.myForm.$valid) {
            var data = {
                "FOMST_Id": $scope.fomsT_Id,
               // "FOHWDT_Id": $scope.FOHWDT_Id,
                "FOMS_ShiftName": $scope.FOMS_ShiftName,
                "FOMST_IHalfLoginTime": $filter('date')($scope.FOMST_IHalfLoginTime, "HH:mm"),
                "FOMST_IIHalfLogoutTime": $filter('date')($scope.FOMST_IIHalfLogoutTime, "HH:mm"),
                "FOMST_IHalfLogoutTime": $filter('date')($scope.FOMST_IHalfLogoutTime, "HH:mm"),
                "FOMST_IIHalfLoginTime": $filter('date')($scope.FOMST_IIHalfLoginTime, "HH:mm"),
                "FOMST_FDWHrMin": $filter('date')($scope.FOMST_FDWHrMin, "HH:mm"),
                "FOMST_HDWHrMin": $filter('date')($scope.FOMST_HDWHrMin, "HH:mm"),
                "FOMST_LunchHoursDuration": $filter('date')($scope.FOMST_LunchHoursDuration, "HH:mm"),
                "FOMST_FixTimings": $filter('date')($scope.FOMST_FixTimings, "HH:mm"),
                "FOMST_DelayPerShiftHrMin": $filter('date')($scope.FOMST_DelayPerShiftHrMin, "HH:mm"),
                "FOMST_EarlyPerShiftHrMin": $filter('date')($scope.FOMST_EarlyPerShiftHrMin, "HH:mm")
            }
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            apiService.create("ShiftSettingMaster/savedata", data).
                then(function (promise) {
                    if (promise.returnvalue === "success") {

                        $scope.gridOptions1.data = promise.filldata;

                        swal('Record Saved/Updated Successfully');
                    }
                    else if (promise.returnvalue === "Duplicate") {

                        $scope.gridOptions1.data = promise.filldata;

                        swal('Record already exist.');
                    }
                    else {
                        swal('Record Not Saved/Updated Successfully');
                    }
                })
            $scope.cleardata();
        }
    else { $scope.submitted = true; }
        };
        /* post data end*/
        //clearing data start

        $scope.cleardata = function () {
            $scope.disabledata = false;
            $scope.submitted = false;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched(); 
            $scope.FOMS_ShiftName = "";
           // $scope.FOHWDT_Id = "";
            $scope.FOMST_IHalfLoginTime = "";
            $scope.FOMST_IIHalfLogoutTime = "";
            $scope.FOMST_FDWHrMin = "";
            $scope.FOMST_HDWHrMin = "";
            $scope.FOMST_LunchHoursDuration = "";
            $scope.FOMST_IHalfLogoutTime = "";
            $scope.FOMST_IIHalfLoginTime = "";
            $scope.FOMST_FixTimings = "";
            $scope.FOMST_DelayPerShiftHrMin = "";
            $scope.FOMST_EarlyPerShiftHrMin = "";
        }
        //clearing data end
        //edit start
        $scope.editdata = function (employee) {
            $scope.editEmployee = employee.fomsT_Id;
            var pageid = $scope.editEmployee;
            apiService.getURI("ShiftSettingMaster/getdetails", pageid).
            then(function (promise) {

                $scope.disabledata = false;
                $scope.fomsT_Id = promise.filldata[0].fomsT_Id;
                $scope.FOMS_ShiftName = promise.filldata[0].fomS_ShiftName;
             //   $scope.FOHWDT_Id = promise.filldata[0].fohwdT_Id;
                $scope.FOMST_IHalfLoginTime = moment(promise.filldata[0].fomsT_IHalfLoginTime, 'HH:mm').format();
                $scope.FOMST_IIHalfLogoutTime = moment(promise.filldata[0].fomsT_IIHalfLogoutTime, 'HH:mm').format();
                $scope.FOMST_FDWHrMin = moment(promise.filldata[0].fomsT_FDWHrMin, 'HH:mm').format();
                $scope.FOMST_HDWHrMin = moment(promise.filldata[0].fomsT_HDWHrMin, 'HH:mm').format();
                $scope.FOMST_LunchHoursDuration = moment(promise.filldata[0].fomsT_LunchHoursDuration, 'HH:mm').format();
                $scope.FOMST_IHalfLogoutTime = moment(promise.filldata[0].fomsT_IHalfLogoutTime, 'HH:mm').format();
                $scope.FOMST_IIHalfLoginTime = moment(promise.filldata[0].fomsT_IIHalfLoginTime, 'HH:mm').format();
                $scope.FOMST_FixTimings = moment(promise.filldata[0].fomsT_FixTimings, 'HH:mm').format();
                $scope.FOMST_DelayPerShiftHrMin = moment(promise.filldata[0].fomsT_DelayPerShiftHrMin, 'HH:mm').format();
                $scope.FOMST_EarlyPerShiftHrMin = moment(promise.filldata[0].fomsT_EarlyPerShiftHrMin, 'HH:mm').format();
            })
        }
        //edit end
        //activate start
        $scope.deactive = function (Data, SweetAlert) {
            var mgs = "";
            var confirmmgs = "";
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            if (Data.fomS_ActiveFlg === true) {
                mgs = "Deactive";
                confirmmgs = "Deactivated";

            }
            else {
                mgs = "Active";
                confirmmgs = "Activated";

            }
            swal({
                title: "Are you sure",
                text: "Do you want to " + mgs + " record??????",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + mgs + " it!",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
        function (isConfirm) {
            if (isConfirm) {

                apiService.create("ShiftSettingMaster/deactivate", Data).
                then(function (promise) {
                    if (promise.returnvalue === "success") {
                        swal(confirmmgs + " " + "successfully.");
                    }
                    else {
                        swal(confirmmgs + " " + " successfully");
                    }
                    $scope.loaddata();
                })

            }
            else {
                swal("Record " + mgs + " Cancelled");
            }
        });
        }
        //activate end
        //$scope.validateTomintime = function (timedata) {
        //    $scope.ScheduleTimeTo = "";
        //    $scope.totime = timedata;
        //    var hh = $scope.totime.getHours();
        //    var mm = $scope.totime.getMinutes();
        //    $scope.min = timedata;

        //    $scope.min.setMinutes(hh);
        //    $scope.min.setMinutes(mm);
        //}
        
    }
    
})();   