
(function () {
    'use strict';
    angular
        .module('app')
        .controller('instituteWiseMandatorysettingsController', instituteWiseMandatorysettingsController)

    instituteWiseMandatorysettingsController.$inject = ['$rootScope', '$scope', '$state', '$location', 'dashboardService', 'Flash', 'apiService', '$stateParams', '$filter', 'superCache', '$window', 'uiGridGroupingConstants']
    function instituteWiseMandatorysettingsController($rootScope, $scope, $state, $location, dashboardService, Flash, apiService, $stateParams, $filter, superCache, $window, uiGridGroupingConstants) {

        $scope.predicate = 'sno';
        $scope.reverse = false;
        $scope.currentPage = 1;

        $scope.order = function (predicate) {
            $scope.reverse = ($scope.predicate === predicate) ? !$scope.reverse : false;
            $scope.predicate = predicate;
        };

        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }

        $scope.getData = false;

        $scope.paginate = function (value) {
            var begin, end, index;
            begin = ($scope.currentPage - 1) * $scope.numPerPage;
            end = begin + $scope.numPerPage;
            index = $scope.pageList.indexOf(value);
            return (begin <= index && index < end);
        };


        $scope.mandatoryfieldList = [{ id: 'mandatoryfield' }];

        //dynamic table for mandatoryfield

        $scope.mandatoryfieldList = [{ id: 'mandatoryfield' }];
        $scope.addNewMandatoryfield = function () {
            var newItemNo = $scope.mandatoryfieldList.length + 1;

            $scope.mandatoryfieldList.push({ 'id': 'mandatoryfield' + newItemNo });


        };

        $scope.removeNewMandatoryfield = function (index, data) {
            var newItemNo = $scope.mandatoryfieldList.length - 1;
            $scope.mandatoryfieldList.splice(index, 1);

        };

        $scope.Page = {};

        // Get form Details at onload 
        $scope.onLoadGetData = function () {
            var pageid = 2;

            $scope.pageList = [];
            $scope.institutiondropdown = [];
            $scope.pagedropdown = [];

            apiService.getURI("instituteWiseMandatorysettings/getalldetails", pageid).then(function (promise) {

                if (promise.pagedropdown !== null && promise.pagedropdown.length > 0) {
                    $scope.pagedropdown = promise.pagedropdown;
                }
                if (promise.institutiondropdown !== null && promise.institutiondropdown.length > 0) {
                    $scope.institutiondropdown = promise.institutiondropdown;
                }
                if (promise.pageList !== null && promise.pageList.length > 0) {
                    $scope.currentPage = 1;
                    $scope.itemsPerPage = 10;

                    $scope.pageList = promise.pageList;

                    $scope.totalItems = $scope.pageList.length;
                    $scope.numPerPage = 5;
                }




            })
        }


        // clear form data
        $scope.cancel = function () {
            // $scope.search = "";
            $scope.getData = false;
            $scope.Page = {};
            $scope.mandatoryfieldList = [{ id: 'mandatoryfield' }];
            $scope.submitted = false;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
            $scope.onLoadGetData();
        }


        $scope.onSelectgetinstituteWiseMandatorysettings = function (record) {

            $scope.mandatoryfieldList = [{ id: 'mandatoryfield' }];
            $scope.getData = false;

            var id = record.ivrmP_Id;
            var miid = record.mI_Id;

            if (id != undefined && id != "" && miid != undefined && miid != "") {
                var mandatoryfieldList = [];

                var data = {
                    "IVRMP_Id": id,
                    "MI_Id": miid,
                    "mandatoryfieldList": mandatoryfieldList
                };


                apiService.create("instituteWiseMandatorysettings/getPagedetailsBySelection", data).
                    then(function (promise) {

                        if (promise.pageList != null && promise.pageList.length > 0) {
                            $scope.getData = true;
                            $scope.mandatoryfieldList = promise.pageList;
                        }

                    })
            }

        }



        //saving/updating Record
        $scope.submitted = false;
        $scope.saveData = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {

                $scope.Page.mandatoryfieldList = $scope.mandatoryfieldList;

                var data = $scope.Page;

                apiService.create("instituteWiseMandatorysettings/", data).
                    then(function (promise) {
                        if (promise.retrunMsg !== "") {

                            if (promise.retrunMsg == "Error") {
                                swal("Record Not saved / Updated..");

                            }
                            else if (promise.retrunMsg == "Added") {
                                swal("Record Saved / Updated Successfully..");
                            }

                            else {
                                swal("Something went wrong ..!");
                            }

                            $scope.cancel();
                        }
                    })
            }

        };

        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };


        // Edit Single Record
        $scope.EditData = function (record) {

            apiService.create("instituteWiseMandatorysettings/editRecord", record).
                then(function (promise) {

                    if (promise.pageList != null && promise.pageList.length > 0) {
                        $scope.Page.ivrmP_Id = promise.pageList[0].ivrmP_Id;
                        $scope.Page.mI_Id = promise.pageList[0].mI_Id;
                        $scope.mandatoryfieldList = promise.pageList;
                    }

                })
        }


        //deactivate record
        $scope.DeletRecord = function (data, SweetAlert) {


            swal({
                title: "Are you sure?",
                text: "Do you want to Delete Record..!?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, Delete it!",
                cancelButtonText: "Cancel..!",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("instituteWiseMandatorysettings/ActiveDeactiveRecord", data).
                            then(function (promise) {

                                if (promise.retrunMsg !== "") {

                                    if (promise.retrunMsg === "Deleted") {
                                        swal("Record Deleted successfully");
                                    }
                                    else if (promise.retrunMsg === "NotDeleted") {
                                        swal("Record Not Deactivated successfully");
                                    }
                                    else if (promise.retrunMsg === "Error") {
                                        swal("Something went Wrong");
                                    }
                                }
                                $scope.cancel();

                            })
                    }
                    else {
                        swal("Cancelled");
                    }
                }

            );
        }

    }
    }) ();