
(function () {
    'use strict';
    angular
.module('app')
.controller('TTMasterRoomController', TTMasterRoomController)

    TTMasterRoomController.$inject = ['$rootScope', '$scope', '$state', '$location', 'apiService', 'Flash', '$http', '$q', '$stateParams', 'superCache']
    function TTMasterRoomController($rootScope, $scope, $state, $location, apiService, Flash, $http, $q, $stateParams, superCache) {
        $scope.editEmployee = {};
       

        //Ui Grid view rendering data from data base
        $scope.gridOptions = {
            enableColumnMenus: false,
            enableFiltering: true,
            paginationPageSizes: [5, 10, 15],
            paginationPageSize: 5,

            columnDefs: [
                  { name: 'SNO', enableFiltering: false, field: 'name', cellTemplate: '<div class="ui-grid-cell-contents">{{grid.options.paginationPageSize *(grid.options.paginationCurrentPage-1)+grid.renderContainers.body.visibleRowCache.indexOf(row)+1}}</div>' },                    
                { name: 'ttmrM_RoomName', displayName: 'Room Name' },
                { name: 'ttmrM_RoomDetails', displayName: 'Room Details' },
               {
                   field: 'id1', name: '1',
                   displayName: 'View Facility', enableFiltering: false, enableSorting: false, cellTemplate:
             
                       '<div class="grid-action-cell">' +
                       '<a href="javascript:void(0)" data-toggle="modal" data-target="#myModal3" data-backdrop="static" ng-click="grid.appScope.viewrecordspopup(row.entity);"> <i class="fa fa-eye text-purple" ></i></a> ' +
                '</div>'
                } ,
               {
                   field: 'id', name: '',
                   displayName: 'Actions', enableFiltering: false, enableSorting: false, cellTemplate:
             
                     '<div class="grid-action-cell">' +
                 '<a href="javascript:void(0)" ng-click="grid.appScope.getorgvalue(row.entity);"> <i class="fa fa-pencil-square-o" ></i></a>  &nbsp; &nbsp;' +
                '<a ng-if="row.entity.ttmrM_ActiveFlg === false" href="javascript:void(0)" style="color:green;" ng-click="grid.appScope.deactive(row.entity);"> <md-tooltip md-direction="down">Activate Now</md-tooltip> <i class="fa fa-toggle-off text-red" aria-hidden="true"></i></a>' +
              '<span ng-if="row.entity.ttmrM_ActiveFlg === true"><a href="javascript:void(0)"  style="color:red;" ng-click="grid.appScope.deactive(row.entity);">  <md-tooltip md-direction="down">Deactivate Now</md-tooltip> <i class="fa fa-toggle-on text-green"  aria-hidden="true"></i></a><span>' +
          
                '</div>'
               }
            ]          

        };

        //TO  View Record
        $scope.viewrecordspopup = function (employee) {

            $scope.editEmployee = employee.ttmrM_Id;
            var pageid = $scope.editEmployee;
            $scope.combname = employee.ttmrM_RoomName;
            apiService.getURI("TTMasterRoom/Viewfacility", pageid).
                then(function (promise) {
                   
                    $scope.viewrecordspopupdisplay = promise.viewdata;

                })

        };

        // TO Save The Data
        $scope.submitted = false;
        $scope.savedata = function () {
            $scope.submitted = true;

            if ($scope.myForm.$valid) {
                $scope.flistdat = [];
                angular.forEach($scope.facilitylistall, function (ss) {
                    if (ss.stf == true) {
                        $scope.flistdat.push({ TTMFA_Id: ss.ttmfA_Id});
                    }

                })

                debugger;
                var data = {
                    "TTMRM_Id": $scope.TTMRM_Id,
                    "TTMRM_RoomName": $scope.TTMRM_RoomName,
                    "TTMRM_RoomDetails": $scope.TTMRM_RoomDetails,
                    flist: $scope.flistdat,
                }
                apiService.create("TTMasterRoom/savedetail", data).
                    then(function (promise) {
                        if (promise.returnval === true)
                        {
                            swal('Data Saved/Updated successfully');
                           
                            $scope.clearid();
                        }
                        else if (promise.returnval === false && promise.returnduplicatestatus === 'Duplicate')
                        {
                            swal('Records Already Exist !');                            
                        }
                        else
                        {
                            swal('Data Not Saved !');
                        }
                       
                    })
              
            }

        };
        $scope.facilitylistall = [];


        $scope.isOptionsRequired = function () {

            return !$scope.facilitylistall.some(function (options) {
                return options.stf;
            });
        }


        $scope.searchchkbx = "";
        $scope.filterchkbx = function (obj) {
            return angular.lowercase(obj.ttmfA_FacilityName).indexOf(angular.lowercase($scope.searchchkbx)) >= 0;
        }

        $scope.togchkbx = function () {
            $scope.usercheck = $scope.facilitylistall.every(function (options) {
                return options.stf;
            });
        }
        $scope.all_check = function () {
            var toggleStatus = $scope.usercheck;
            angular.forEach($scope.facilitylistall, function (itm) {
                itm.stf = toggleStatus;
            });
           
        }

        $scope.itemsPerPage1 = 15;

        //TO  GEt The Values iN Grid
        $scope.BindData = function () {
            apiService.getDATA("TTMasterRoom/getalldetails").
       then(function (promise) {
           $scope.currentPage = 1;
           $scope.itemsPerPage = 5;
           $scope.facilitylistall = promise.facilitylistall;
           $scope.gridOptions.data = promise.alldata;
          
       })
        };
        //TO clear  data
        $scope.clearid= function () {      
            $state.reload();


        };
        //TO  Edit Record
        $scope.getorgvalue = function (employee) {
            $scope.editEmployee = employee.ttmrM_Id;
            var pageid = $scope.editEmployee;
            apiService.getURI("TTMasterRoom/getdetails", pageid).
            then(function (promise) {

                $scope.TTMRM_Id = promise.editlist[0].ttmrM_Id;
                $scope.TTMRM_RoomName = promise.editlist[0].ttmrM_RoomName;
                $scope.TTMRM_RoomDetails = promise.editlist[0].ttmrM_RoomDetails;


                angular.forEach($scope.facilitylistall, function (ss) {

                    angular.forEach(promise.editfaclist, function (ww) {
                        if (ww.ttmfA_Id == ss.ttmfA_Id) {
                            ss.stf = true;
                        }


                    })

                })

                $scope.togchkbx();

            })
        }
        //TO  delete Record
        $scope.deletedata = function (employee, SweetAlert) {
            $scope.editEmployee = employee.ttmsuaB_Id;
            var pageid = $scope.editEmployee;
            swal({
                title: "Are you sure",
                text: "Do you want to delete record????",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, delete it!",
                cancelButtonText: "Cancel!!!!!!",
                closeOnConfirm: false,
                closeOnCancel: false
            },
            function (isConfirm) {
                if (isConfirm) {
                    apiService.DeleteURI("TTMasterRoom/deletepages", pageid).
                        then(function (promise) {
                            if (promise.returnval=='MAP') {
                                swal('Facilities Are Already Mapped with Room');
                            }
                            else {
                                if (promise.returnval === true) {
                                    swal('Record Deleted Successfully');
                                }
                                else {
                                    swal('Record Not Deleted Successfully!');
                                }
                                $scope.BindData();
                            }
                        
                    })
                    $scope.BindData();
                }
                else {
                    swal("Record Deletion Cancelled");
                }
            });
        };

        $scope.interacted = function (field) {

            return $scope.submitted || field.$dirty;
        };
        $scope.deactive = function (employee, SweetAlert) {
            var mgs = "";
            var confirmmgs = "";
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            if (employee.ttmrM_ActiveFlg === true) {
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

                apiService.create("TTMasterRoom/deactivate", employee).
                then(function (promise) {
                    if (promise.returnval == true) {
                        swal(confirmmgs + " " + "successfully.");
                    }
                    else {
                        swal(confirmmgs + " " + " successfully");
                    }
                    $state.reload();
                })

            }
            else {
                swal("Record " + mgs + " Cancelled");
            }
        });
        }
    }

})();