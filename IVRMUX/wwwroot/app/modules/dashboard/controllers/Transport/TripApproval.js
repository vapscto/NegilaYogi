(function () {
    'use strict';

    angular
        .module('app')
        .controller('TripApproval', TripApproval);

    TripApproval.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q', '$cookieStore', '$stateParams', '$filter', 'FormSubmitter', '$window', 'superCache'];

    function TripApproval($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $q, $cookieStore, $stateParams, $filter, FormSubmitter, $window, superCache) {
       
        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;
        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey == key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        }
        $scope.vehicles = [{ id: 'vehicle1' }];
        $scope.loaddata = function () {
            
            apiService.getURI("Trip/GetTripDetails/", 1).then(function (promise) {
                if (promise.tripDetails != null) {
                    $scope.vehicleList = promise.vehicleList;
                    $scope.driverList = promise.driverList;
                    $scope.tripDetails = promise.tripDetails;
                    $scope.PickUpLocation = promise.tripDetails[0].trtP_PickUpLocation;
                    $scope.TRTP_BookingDate = new Date(promise.tripDetails[0].trtP_BookingDate);
                    $scope.TRTP_TripDate = new Date(promise.tripDetails[0].trtP_TripDate);
                    $scope.TRTP_HirerName = promise.tripDetails[0].trtP_HirerName;
                    $scope.TRTP_HirerContactNo = promise.tripDetails[0].trtP_HirerContactNo;
                    $scope.TRTP_TripAddress = promise.tripDetails[0].trtP_TripAddress;
                    $scope.TRTP_TripFromDate = new Date(promise.tripDetails[0].trtP_TripFromDate);
                    $scope.TRTP_TripToDate = new Date(promise.tripDetails[0].trtP_TripToDate);
                    $scope.TRTP_Id = promise.tripDetails[0].trtP_Id;
                    $scope.TRTP_BillGeneratedFlag = promise.tripDetails[0].trtP_BillGeneratedFlag;
                    $scope.vehiclesAssignedList = promise.vehicleDriverAllottmentList;
                   // $scope.TRTP_Id = promise.trtP_Id;
                }
                if (promise.approvedTripList != null) {
                    $scope.approvedTripList = promise.approvedTripList;
                    $scope.presentCountgrid = $scope.approvedTripList.length;
                }
                else {
                    swal("No Records Found");
                }

              
            });
        }
        $scope.close = function () {
            $('#myModal').modal('hide');
        }
        $scope.view = function () {
            if ($scope.vehiclesAssignedList != null) {
                
                $scope.vehicles = $scope.vehiclesAssignedList;
                $scope.disableDriverallottment = true;
                
            }
            else {
                $scope.disableDriverallottment = false;
            }
            $('#myModal').modal('show');
        }
        $scope.approve = function (tripdet) {
            swal({
                title: "Are you sure?",
                text: "Do you want to Approve this Trip!!!!!!",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, Approve it!",
                cancelButtonText: "Cancel!!!!!!",
                closeOnConfirm: false,
                closeOnCancel: false
            },
          function (isConfirm) {
              if (isConfirm) {
                  $scope.tripId = tripdet.trtP_Id;
                  apiService.getURI("Trip/approveTrip/", $scope.tripId).then(function (promise) {
                      if (promise.returnVal == 'approved') {
                          swal("Trip Approved Successfully");
                          $state.reload();
                      }
                      else {
                          swal("Failed");
                      }
                 
                  });
              }
              else {
                  swal("Cancelled");
              }
          });
            
        }
        $scope.reject = function (tripdet) {
            swal({
                title: "Are you sure?",
                text: "Do you want to Reject this Trip!!!!!!",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, Reject it!",
                cancelButtonText: "Cancel!!!!!!",
                closeOnConfirm: false,
                closeOnCancel: false
            },
          function (isConfirm) {
              if (isConfirm) {
                  $scope.tripId = tripdet.trtP_Id;
                  apiService.getURI("Trip/rejectTrip/", $scope.tripId).then(function (promise) {
                      if (promise.returnVal == 'rejected') {
                          swal("Trip Rejected Successfully");
                          $state.reload();
                      }
                      else {
                          swal("Failed");
                      }

                  });
              }
              else {
                  swal("Cancelled");
              }
          });
        }
    }
})();
