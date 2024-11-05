(function () {
    'use strict';
    angular
.module('app')
.controller('seatblockController', seatblockController)

    seatblockController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', '$filter','superCache']
    function seatblockController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, $filter, superCache) {

        //Date:23-12-2016 for displaying privileges.
        $scope.userPrivileges = "";
        var pageid = $stateParams.pageId;
        var privlgs = JSON.parse(localStorage.getItem("privileges"));
        for (var i = 0; i < privlgs.length; i++) {
            if (privlgs[i].pageId == pageid) {
                $scope.userPrivileges = privlgs[i];
            }
        }



        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }

        $scope.sortKey = "pasbS_Id";   //set the sortKey to the param passed
        $scope.reverse = true; //if true make it false and vice versa

        $scope.presentCountgrid = 0;


        $scope.disable = true;
        $scope.inst_Id = "";
        $scope.yearId = "";

        $scope.IsHidden = true;
        $scope.ShowHide = function () {
            //If DIV is hidden it will be visible and vice versa.
            $scope.IsHidden = $scope.IsHidden ? false : true;
        }
        $scope.IsHidden1 = true;
        $scope.ShowHide1 = function () {
            //If DIV is hidden it will be visible and vice versa.
            $scope.IsHidden1 = $scope.IsHidden1 ? false : true;
        }
        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };
        $scope.SeatBlockDropdownList = function () {
            $scope.myBtn = true;
            $scope.myBtn1 = false;
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            var pageid = 2;
            apiService.getURI("SeatBlock/getalldetails", pageid).
        then(function (promise) {

            $scope.InstuteDropDown = promise.instuteDropDown;
            $scope.YearDropdown = promise.yearDropdown;
            $scope.RegstudentdropDown = promise.regstudentdropDown;
            $scope.staffUserDropdown = promise.staffUserDropdown;
            if (promise.count == 0)
            {
                swal("No Records Found.....!!");
            }
            else {
                $scope.seatBlockedList = promise.seatBlockedList;
                $scope.presentCountgrid = promise.seatBlockedList.length;
            }
            //$scope.PASBS_Date = new Date();

            //$scope.predicate = 'sno';
            //$scope.reverse = false;
            //$scope.currentPage = 1;
            //$scope.order = function (predicate) {
            //    $scope.reverse = ($scope.predicate === predicate) ? !$scope.reverse : false;
            //    $scope.predicate = predicate;

            //};


            //$scope.totalItems = $scope.seatBlockedList.length;
            //$scope.numPerPage = 10;
            //$scope.paginate = function (value) {
            //    var begin, end, index;
            //    begin = ($scope.currentPage - 1) * $scope.numPerPage;
            //    end = begin + $scope.numPerPage;
            //    index = $scope.seatBlockedList.indexOf(value);
            //    return (begin <= index && index < end);
            //};


            //list = $scope.SeatBlockedList;
        })
        };
        $scope.sort = function (keyname) {
            $scope.sortColumn = true;
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }
        $scope.getInstitutionId = function (MI_Id) {
            if (MI_Id == undefined) {
                swal('Please Select Institution Name', '');
                return;
            }
            else {
                $scope.inst_Id = MI_Id;
                $scope.getStudentList($scope.yearId);
            }
        }

        //search
        $scope.searchValue = '';
        $scope.filterValue = function (obj) {
            return ($filter('date')(obj.pasbS_Date, 'dd-MM-yyyy').indexOf($scope.searchValue) >= 0) || angular.lowercase(obj.instituteName).indexOf(angular.lowercase($scope.searchValue)) >= 0 || angular.lowercase(obj.studentName).indexOf(angular.lowercase($scope.searchValue)) >= 0 || angular.lowercase(obj.staff).indexOf(angular.lowercase($scope.searchValue)) >= 0 || angular.lowercase(obj.year).indexOf(angular.lowercase($scope.searchValue)) >= 0;
        }

        $scope.getStudentList = function (ASMAY_Id) {

            if (ASMAY_Id == undefined) {
                swal('Please Select Academic Year', '');
                return;
            }
            if ($scope.inst_Id !== "" && $scope.inst_Id != null && ASMAY_Id != "" && ASMAY_Id != null) {
                $scope.disable = false;
                $scope.yearId = ASMAY_Id;
                var data = {
                    "MI_Id": $scope.inst_Id,
                    "ASMAY_Id": $scope.ASMAY_Id,
                }
                apiService.create("SeatBlock/getSeatConfirmedStud", data).
              then(function (promise) {
                  $scope.RegstudentdropDown = promise.regstudentdropDown;
              })
            }
            else {
                $scope.disable = true;
            }
        }

        //date nad time pick
        //$scope.myDate = new Date();
        //$scope.minDate = new Date(
        //    $scope.myDate.getFullYear(),
        //    $scope.myDate.getMonth() - 2,
        //    $scope.myDate.getDate());
        //$scope.maxDate = new Date(
        //    $scope.myDate.getFullYear(),
        //    $scope.myDate.getMonth() + 2,
        //    $scope.myDate.getDate());



        //$scope.onlyWeekendsPredicate = function (date) {
        //    var day = date.getDay();
        //    return day === 0 || day === 6;
        //};

        $scope.edit = function (id) {

            apiService.getURI("SeatBlock/getSeatBlockById", id).
        then(function (promise) {
            
            angular.forEach(promise.seatBlockedList, function (value, key) {
                if (value.pasbS_Id !== 0) {
                    $scope.myBtn = false;
                    $scope.myBtn1 = true;
                  //  $scope.disable = false;
                    // swal(value.pasbS_Id);
                    $scope.pasbS_Id = value.pasbS_Id;
                    $scope.MI_Id = value.mI_Id;
                    $scope.ASMAY_Id = value.asmaY_Id;
                    $scope.pasr_id = value.pasR_Id;
                    $scope.PASBS_Date = new Date(value.pasbS_Date);
                    $scope.IVRMSTAUL_Id = value.ivrmstauL_Id;
                }
            });
        })
        }

        $scope.cleardata = function () {
            $state.reload();

            //$scope.myBtn = true;
            //$scope.myBtn1 = false;
            //$scope.pasbS_Id = "";
            //$scope.MI_Id = "";
            //$scope.ASMAY_Id = "";
            //$scope.pasr_id = "";
            //$scope.PASBS_Date = "";
            //$scope.IVRMSTAUL_Id = "";
        }

        $scope.saveSeatBlock = function () {
            //if (($scope.MI_Id === undefined || $scope.MI_Id === "") || ($scope.pasr_id === undefined || $scope.pasr_id === "") || ($scope.ASMAY_Id === undefined || $scope.ASMAY_Id === "")) {
            //    if ($scope.MI_Id === undefined || $scope.MI_Id === "") {
            //        swal("Please select Institution Name");
            //        return;
            //    }

            //    if ($scope.pasr_id === undefined || $scope.pasr_id === "") {
            //        swal("Please select Student Name");
            //        return;
            //    }
            //    if ($scope.ASMAY_Id === undefined || $scope.ASMAY_Id === "") {
            //        swal("Please select Year");
            //        return;
            //    }
            //    //if($scope.PASBS_Date === undefined || $scope.PASBS_Date === "")
            //    //{
            //    //    swal("Please select Date");
            //    //    return;
            //    //}

            //} else {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var data = {
                    "MI_Id": $scope.MI_Id,
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "PASR_Id": $scope.pasr_id,
                    "PASBS_Date": new Date($scope.PASBS_Date).toDateString(),
                    "IVRMSTAUL_Id": $scope.IVRMSTAUL_Id,
                }

                apiService.create("SeatBlock/", data).
                then(function (promise) {
                    if (promise.returnVal == true) {
                        swal('Seat blocked successfully', 'Success');
                        $state.reload();
                    }
                    else {
                        swal('Failed to block the Seat', 'Fail');
                    }
                    $scope.seatBlockedList = promise.seatBlockedList;
                    $scope.presentCountgrid = promise.seatBlockedList.length;
                })
            }
        };
        $scope.delete = function (id,SweetAlert) {
           // var id = $scope.pasbS_Id;
            // swal(id);
            swal({
                title: "Are you sure?",
                text: "Do you want to Unblock Seat..!?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, delete it!",
                cancelButtonText: "Cancel..!",
                closeOnConfirm: false,
                closeOnCancel: false
            },
            function (isConfirm) {
                if (isConfirm) {
                    apiService.DeleteURI("SeatBlock/deletedetails", id).
                    then(function (promise) {
                        $scope.seatBlockedList = promise.seatBlockedList;
                        $scope.presentCountgrid = promise.seatBlockedList.length;
                        if (promise.count == 0)
                        {
                            swal("No Records Found.....!!");
                            return;
                        }
                        if (promise.message != "" && promise.message != null)
                        {
                            swal(promise.message);
                            return;
                        }
                        if (promise.returnVal == true) {
                            swal('Seat Unblocked successfully..!', 'success');
                            $state.reload();
                        }
                        else {
                            swal('Fail to Unlock Seat..!', 'Fail');
                        }
                      
                    })
                }
                else {
                    swal("Seat Unblock Cancelled", "Ok");
                }
            });
        }

    }

})();
