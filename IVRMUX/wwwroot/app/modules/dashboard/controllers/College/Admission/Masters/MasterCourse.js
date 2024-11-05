(function () {
    'use strict';

    angular
        .module('app')
        .controller('MasterCourseController', MasterCourseController);

    MasterCourseController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout'];

    function MasterCourseController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout) {


        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        } else {
            paginationformasters = 10;
        }
        $scope.itemsPerPage = paginationformasters;

        $scope.BindData = function (MasterCourseList) {
            $scope.currentPage = 1;
            // $scope.itemsPerPage = 5;
            $scope.search = "";

            apiService.getDATA("MasterCourse/getalldetails").then(function (promise) {
                $scope.MasterCourseList = promise.masterCourseList;
                $scope.MasterCourseList1 = promise.masterCourseList1;
                $scope.grouptypeListOrder = $scope.MasterCourseList1;
            });
        };

        //For Save data record 
        $scope.savedata = function () {

            if ($scope.myForm.$valid) {
                var data = {
                    "AMCO_Id": $scope.AMCO_Id,
                    "AMCO_CourseName": $scope.courseName,
                    "AMCO_CourseCode": $scope.code,
                    "AMCO_CourseInfo": $scope.courseInfo,
                    "AMCO_NoOfYears": $scope.noyear,
                    "AMCO_NoOfSemesters": $scope.noOfSemester,
                    "AMCO_Order": $scope.order,
                    "AMCO_MinAttPer": Number($scope.noOfAttendance),
                    "AMCO_CourseFlag": $scope.courseFlag,
                    "AMCO_FeeAplFlg": $scope.feeApplicable,
                    "AMCO_RegFeeFlg": $scope.regFeeFlag

                }
                apiService.create("MasterCourse/Savedetails", data).then(function (promise) {

                    if (promise.returnval != null && promise.duplicateval != null) {
                        if (promise.duplicateval == false) {
                            if (promise.returnval == true) {

                                if ($scope.AMCO_Id > 0) {
                                    swal("Data record Updated Successfully!!!");
                                }
                                else {
                                    swal("Data record Inserted Successfully!!!");
                                }

                            }
                            else {
                                if (promise.returnval == false) {
                                    if ($scope.AMCO_Id > 0) {
                                        swal("Updation Failed!!!");
                                    }
                                    else {
                                        swal("Insertion Failed!!!");
                                    }
                                }
                            }
                        }
                        else {
                            swal("Record already exist");
                        }
                    }
                    else {
                        swal("Kindly Contact Administrator!!!");
                    }
                    $state.reload();
                })
            }
            else {
                $scope.submitted = true;
            }
        }

        $scope.EditData = function (item) {

            var data = item;
            apiService.create("MasterCourse/EditData", data).then(function (promise) {

                if (promise.msg == "Mapped") {
                    $scope.edit = true;
                } else {
                    $scope.edit = false;
                }
            })
            $scope.AMCO_Id = item.amcO_Id;
            $scope.courseName = item.amcO_CourseName;
            $scope.code = item.amcO_CourseCode,
                $scope.courseInfo = item.amcO_CourseInfo,
                $scope.noyear = item.amcO_NoOfYears,
                $scope.noOfSemester = item.amcO_NoOfSemesters,
                $scope.order = item.amcO_Order,
                $scope.noOfAttendance = item.amcO_MinAttPer
            $scope.regFeeFlag = item.amcO_RegFeeFlg,
                $scope.courseFlag = item.amcO_CourseFlag,
                $scope.feeApplicable = item.amcO_FeeAplFlg
        }

        //For Delete data record
        $scope.deactiveY = function (item, SweetAlert) {

            $scope.AMCO_Id = item.amcO_Id;

            var dystring = "";
            if (item.amcO_ActiveFlag == 1) {
                dystring = "Deactivate";
            }
            else if (item.amcO_ActiveFlag == 0) {
                dystring = "Activate";
            }
            swal({
                title: "Are you sure?",
                text: "Do You Want To " + dystring + " Record?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + dystring + " it",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("MasterCourse/Deletedetails", item).
                            then(function (promise) {
                                if (promise.returnval == true) {

                                    swal("Record " + dystring + "d Successfully!!!");
                                }
                                else {

                                    swal("Record Not " + dystring + "d Successfully!!!");
                                }
                                $state.reload();
                            })
                    }
                    else {
                        swal("Record Deactivation Cancelled!!!");
                    }
                });
        }

        //For Cancel data record 
        $scope.Cancel = function () {
            $state.reload();
        }

        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }


        $scope.resultClick = function () {
            $scope.AMCO_Id = item.amcO_Id;
        }

        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };


        //Set Order

        $scope.resetLists = function () {
            $scope.configA = {
                // Changed sorting within list
                onUpdate: function (evt) {
                    var itemEl = evt.item;  // dragged HTMLElement
                    // + indexes from onEnd
                }
            };
        };


        $scope.init = function () {
            $scope.resetLists();
        };
        $scope.init();



        $scope.sortableOptions = {
            stop: function (e, ui) {
                for (var index in $scope.grouptypeListOrder) {
                    $scope.grouptypeListOrder[index].amcO_Order = Number(index) + 1;

                }
            }
        };

        $scope.getOrder = function (orderarray) {
            var data = {
                coursedto: orderarray,
            }
            apiService.create("MasterCourse/getOrder", data).
                then(function (promise) {
                    if (promise.retrunMsg != "" && promise.retrunMsg != undefined && promise.retrunMsg != null) {
                        swal(promise.retrunMsg);
                       // $('#myModal').modal('hide');
                    }
                  //  $state.reload();
                    $('#myModal').modal('hide');
                });

        }


    }
})();
