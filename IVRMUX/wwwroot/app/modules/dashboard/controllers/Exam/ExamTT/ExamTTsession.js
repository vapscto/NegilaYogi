(function () {
    'use strict';
    angular
.module('app')
.controller('ExamTTsessionmasterController', ExamTTsessionmasterController)

    ExamTTsessionmasterController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$filter']
    function ExamTTsessionmasterController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $filter) {


        $scope.DeleteRecord = {};
        $scope.EditRecord = {};
        //TO  GEt The Values iN Grid
        // var temp_exam_list = [];


        $scope.BindData = function () {
            apiService.getDATA("ExamTTsessionmaster/Getdetails/2").
       then(function (promise) {
           
           if (promise.getdetails != null && promise.getdetails.length > 0) {
               $scope.gridOptions.data = promise.getdetails;
           } else {
               swal("No Reocrds Found");
           }
       })
        };

        $scope.gridOptions = {
            enableColumnMenus: false,
            enableFiltering: true,
            paginationPageSizes: [5, 10, 15],
            paginationPageSize: 5,

            columnDefs: [
                   { name: 'SLNO', displayName: 'SL NO', enableFiltering: false, enableSorting: false, field: 'name', cellTemplate: '<div class="ui-grid-cell-contents">{{grid.options.paginationPageSize *(grid.options.paginationCurrentPage-1)+grid.renderContainers.body.visibleRowCache.indexOf(row)+1}}</div>' },
              { name: 'ettS_SessionName', displayName: 'Session Name' },
              { name: 'ettS_StartTime', displayName: 'Session Start Time' },
              { name: 'ettS_EndTime', displayName: 'Session End Time' },

               {
                   field: 'id', name: '',
                   displayName: 'Actions', enableFiltering: false, enableSorting: false, cellTemplate:
                       '<div class="grid-action-cell">' +
                 '<a href="javascript:void(0)" ng-click="grid.appScope.Editexammasterdata(row.entity);"><md-tooltip md-direction="down">Edit</md-tooltip> <i class="fa fa-pencil-square-o" ></i></a>  &nbsp; &nbsp;' +
                '<a ng-if="row.entity.ettS_Activeflag === false" href="javascript:void(0)" style="color:green;" ng-click="grid.appScope.deactive(row.entity);"> <md-tooltip md-direction="down">Activate Now</md-tooltip> <i class="fa fa-toggle-off text-red" aria-hidden="true"></i></a>' +
              '<span ng-if="row.entity.ettS_Activeflag === true"><a href="javascript:void(0)"  style="color:red;" ng-click="grid.appScope.deactive(row.entity);">  <md-tooltip md-direction="down">Deactivate Now</md-tooltip> <i class="fa fa-toggle-on text-green"  aria-hidden="true"></i></a><span>' +
                '</div>'
               }
            ],
            onRegisterApi: function (gridApi) {
                $scope.gridApi = gridApi;
                // $scope.gridApi.core.refresh($scope.gridOptions.data);
            }

        };

        $scope.hstep = 1;
        $scope.mstep = 1;

        $scope.options = {
            hstep: [1, 2, 3],
            mstep: [1, 5, 10, 15, 25, 30]
        };

        $scope.ismeridian = true;
        $scope.toggleMode = function () {
            $scope.ismeridian = !$scope.ismeridian;
        };

        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };

        $scope.IsHidden1 = true;
        $scope.ShowHide1 = function () {
            //If DIV is hidden it will be visible and vice versa.
            $scope.IsHidden1 = $scope.IsHidden1 ? false : true;
        }

        $scope.IsHidden2 = true;
        $scope.ShowHide2 = function () {
            //If DIV is hidden it will be visible and vice versa.
            $scope.IsHidden2 = $scope.IsHidden2 ? false : true;
        }



        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }

        // TO Save The Data
        $scope.submitted = false;
        $scope.saveddata = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var data = {
                    "ETTS_Id": $scope.ettS_Id,
                    "ETTS_SessionName": $scope.ETTS_SessionName,
                    "ETTS_StartTime": $filter('date')($scope.ETTS_StartTime, "h:mm a"),
                    "ETTS_EndTime": $filter('date')($scope.ETTS_EndTime, "h:mm a"),
                }
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }
                apiService.create("ExamTTsessionmaster/savedetails", data).
                             then(function (promise) {
                                 if (promise.message == "Duplicate") {
                                     swal("Record Already Exists")
                                 } else {
                                     if (promise.returnval == true) {
                                         if (promise.message == "Add") {
                                             swal("Record Saved Successfully")
                                         } else if (promise.message == "Update") {
                                             swal("Record Updated Successfully")
                                         }
                                     } else {
                                         if (promise.message == "Add") {
                                             swal("Failed To Save Record")
                                         } else if (promise.message == "Update") {
                                             swal("Failed To Update Record")
                                         }
                                     }
                                 }
                                 $state.reload();
                             })
            }
            else {
                $scope.submitted = true;
            }
        };


        // to Edit Data
        $scope.Editexammasterdata = function (EditRecord) {
            
            var data = {
                "ETTS_Id": EditRecord.ettS_Id
            }
            apiService.create("ExamTTsessionmaster/editdetails/", data).
            then(function (promise) {
                $scope.ettS_Id = promise.editlist[0].ettS_Id;
                $scope.ETTS_SessionName = promise.editlist[0].ettS_SessionName;
                $scope.ETTS_StartTime = moment(promise.editlist[0].ettS_StartTime, 'h:mm a').format(),
                $scope.ETTS_EndTime = moment(promise.editlist[0].ettS_EndTime, 'h:mm a').format()
            })
        };


        //to deactive the data

        $scope.deactive = function (deactiveRecord) {
            var mgs = "";
            var confirmmgs = "";
            if (deactiveRecord.ettS_Activeflag == true) {
                mgs = "Deactivate";
                confirmmgs = "De-activated";
            }
            else {
                mgs = "Activate";
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

                   var config = {
                       headers: {
                           'Content-Type': 'application/json;'
                       }
                   }
                   apiService.create("ExamTTsessionmaster/deactivate", deactiveRecord).
                       then(function (promise) {
                           if (promise.already_cnt == true) {
                               swal("You Can Not Deactivate This Record,It Has Dependency");
                           }
                           else {
                               if (promise.returnval == true) {
                                   swal("Record " + confirmmgs + " " + "successfully");
                               }
                               else {
                                   swal("Record " + mgs + " Failed");
                               }
                           }
                       })
               }
               else {
                   swal("Record " + mgs + " Cancelled");
               }
               $state.reload();
           });

        };

        $scope.cancel = function () {
            $state.reload();
        }
    }

})();