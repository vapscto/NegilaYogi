
(function () {
    'use strict';
    angular
.module('app')
.controller('FeeHeadClgController', FeeHeadClgController)

    FeeHeadClgController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache']
    function FeeHeadClgController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {

        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }
        else if (ivrmcofigsettings.length == 0 || ivrmcofigsettings == undefined) {
            paginationformasters = 10;
        }

        //  $scope.sortKey = "fmH_Id";   //set the sortKey to the param passed
        $scope.sortKey = "fmH_Order";   //set the sortKey to the param passed

        $scope.reverse = false; //if true make it false and vice versa

        $scope.feeorder = false;
        $scope.feetext = true;
        $scope.searchValue = "";
        $scope.filterValue = function (obj) {
            if (obj.fmH_ActiveFlag == true) {
                $scope.test = "Active";
            } else if (obj.fmH_ActiveFlag == false) {
                $scope.test = "Deactive";
            }
            return angular.lowercase(obj.fmH_FeeName).indexOf(angular.lowercase($scope.searchValue)) >= 0 || JSON.stringify(obj.fmH_Order).indexOf($scope.searchValue) >= 0 || JSON.stringify(obj.fmH_SpecialFeeFlag).indexOf($scope.searchValue) >= 0 || JSON.stringify(obj.fmH_PDAFlag).indexOf($scope.searchValue) >= 0 || JSON.stringify(obj.fmH_RefundFlag).indexOf($scope.searchValue) >= 0 || angular.lowercase(obj.fmH_Flag).indexOf(angular.lowercase($scope.searchValue)) >= 0 || angular.lowercase($scope.test).indexOf(angular.lowercase($scope.searchValue)) >= 0;
        }

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
        $scope.getOrder = function (orderarray) {
            
            angular.forEach(orderarray, function (value, key) {
                if (value.fmH_Id !== 0) {
                    orderarray[key].fmH_Order = key + 1;
                }
            });
            var data = {
                CourseDTO: orderarray,
            }
            apiService.create("FeeHeadClg/validateordernumber/", data).
                then(function (promise) {
                    //if (promise.retrunMsg != "" && promise.retrunMsg != undefined && promise.retrunMsg != null) {
                    //    swal(promise.retrunMsg);
                    //}
                    if (promise.returnval === true) {
                        swal('Record Saved/Updated Successfully');
                        $state.reload();
                    }
                    else {
                        swal('Record not Saved/Updated Successfully');
                    }
                });
        }


        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            var pageid = 2;
            apiService.getURI("FeeHeadClg/getalldetails", pageid).
        then(function (promise) {
            if (promise.groupHeadData !=null) {
                $scope.totcountfirst = promise.groupHeadData.length;

            }
           
            $scope.students = promise.groupHeadData;
            $scope.students.data = promise.groupHeadData;
            $scope.studentListOrder = promise.groupHeadData;
        })
            $scope.sort = function (keyname) {
                $scope.sortKey = keyname;   //set the sortKey to the param passed
                $scope.reverse = !$scope.reverse; //if true make it false and vice versa
            }

        }

        $scope.submitted = false;
        $scope.saveGroupdata = function (students) {
            
            if ($scope.myFormhead.$valid) {

                var Refundflag = $scope.FMH_RefundFlag
                if (Refundflag == true) {
                    Refundflag = 1;
                }
                else {
                    Refundflag = 0;
                }
                var PDFflag = $scope.FMH_PDAFlag
                if (PDFflag == true) {
                    PDFflag = 1;
                }
                else {
                    PDFflag = 0;
                }
                var Spefeeflag = $scope.FMH_SpecialFeeFlag
                if (Spefeeflag == true) {
                    Spefeeflag = 1;
                }
                else {
                    Spefeeflag = 0;
                }
                var flaghead = $scope.hdtype
                if (flaghead == "1") {
                    flaghead = "G";
                }
                else if (flaghead == "2") {
                    flaghead = "T";
                }
                else if (flaghead == "3") {
                    flaghead = "E";
                }
                else if (flaghead == "4") {
                    flaghead = "R";
                }
                else if (flaghead == "5") {
                    flaghead = "A";
                }

                else if (flaghead == "6") {
                    flaghead = "H";
                }
                else if (flaghead == "7") {
                    flaghead = "N";
                }

                else if (flaghead == "8") {
                    flaghead = "S";
                }
                else if (flaghead == "9") {
                    flaghead = "F";
                }

                var flagcount = 0;

                if (flaghead == "R") {
                    for (var t = 0; t < students.length; t++) {
                        if (students[t].fmH_Flag == "R") {
                            flagcount = Number(flagcount) + 1;
                        }
                    }
                }

                if (flagcount == 0) {
                    var data = {
                        "FMH_Id": $scope.fmH_Id,
                        "FMH_FeeName": $scope.FMH_FeeName,
                        "FMH_Flag": flaghead,
                        "FMH_RefundFlag": Refundflag,
                        "FMH_PDAFlag": PDFflag,
                        "FMH_SpecialFeeFlag": Spefeeflag,
                        "FMH_Order": $scope.FMH_Order,
                        "FMH_ActiveFlag": true,

                    }
                    var config = {
                        headers: {
                            'Content-Type': 'application/json;'
                        }
                    }
                    apiService.create("FeeHeadClg/", data).
                    then(function (promise) {
                        if (promise.returnval === true) {
                            $scope.masterse = promise.groupHeadData;
                            $scope.loaddata();
                            if (promise.returnval === true) {
                                if (promise.message != null) {
                                    swal('Record Updated Successfully', 'success');
                                    $state.reload();
                                }
                                else {
                                    swal('Record Saved Successfully', 'success');
                                    $state.reload();
                                }
                            }
                            //swal('Record Saved/Updated Successfully');
                        }
                        else if (promise.returnval === false && promise.returnduplicatestatus === 'Duplicate') {
                            swal('Record Already Exist');
                        }
                        else {
                            if (promise.message != null) {
                                swal('Record Not Updated', 'success');
                            }
                            else {
                                swal('Record Not Saved', 'success');
                            }
                        }
                    })
                    //$scope.cance();
                }
                else {
                    swal("Fee Head of Type Registration already exists");
                }
            }
            else {
                $scope.submitted = true;
            }
        };
        $scope.interacted1 = function (field) {

            return $scope.submitted || field.$dirty;
        };
        //var list = [];
        //$scope.order = function () {
        //    
        //    angular.forEach($scope.students, function (itm) {
        //        list.push(Number(itm.fmH_Order));
        //    })
        //    var count = Math.max.apply(null, list);
        //    $scope.FMH_Order= (count = count + 1);
        //    swal("Fee Order:"+ count);
        //};

        $scope.cance = function () {
            
            //$scope.FMH_Id = "",
            //$scope.FMH_FeeName = "";
            //$scope.FMH_SpecialFeeFlag = "";
            //$scope.FMH_PDAFlag = "";
            //$scope.FMH_RefundFlag = "";
            //$scope.FMH_Order = "";
            //$scope.hdtype = "";
            //$scope.feeorder = false;
            //$scope.submitted = false;
            //$scope.myFormhead.$setPristine();
            //$scope.myFormhead.$setUntouched();
            //$scope.searchValue = ""
            $state.reload();
            
        }
        $scope.searchsource = function () {
            var entereddata = $scope.search;
            var data = {
                "FMH_FeeName": $scope.search,
                "FMH_Flag": $scope.type

            }
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            apiService.create("FeeHeadClg/1", data).
        then(function (promise) {
            $scope.students = promise.groupHeadData;
            swal("searched Successfully");
        })
        }
        $scope.editEmployee = {} 
        $scope.deletedata = function (employee, SweetAlert) {
            $scope.editEmployee = employee.fmH_Id;
            var pageid = $scope.editEmployee;
            swal({
                title: "Are you sure?",
                text: "Do You Want To Delete Record?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, Delete it",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
            function (isConfirm) {
                if (isConfirm) {
                    apiService.DeleteURI("FeeHeadClg/deletepages", pageid).
                    then(function (promise) {
                        $scope.students = promise.groupHeadData;
                        if (promise.returnval === true) {
                            swal('Record Deleted Successfully');
                        }
                        else if (promise.dupr === false && promise.returnval != true) {
                            swal('Record Not Deleted', 'Fee Head Already Mapped Yearly Group');
                        }
                        else {
                            swal('Record Not Deleted');
                        }
                        $scope.loaddata();
                    })
                    $scope.loaddata();
                }
                else {
                    swal("Record Deletion Cancelled");
                    $scope.loaddata();
                }
            });
        }
        $scope.getorgvalue = function (employee) {
            $scope.editEmployee = employee.fmH_Id;
            var pageid = $scope.editEmployee;
            apiService.getURI("FeeHeadClg/getdetails", pageid).
            then(function (promise) {

                $scope.FMH_FeeName = promise.groupHeadData[0].fmH_FeeName;
                $scope.FMH_Order = promise.groupHeadData[0].fmH_Order;
                $scope.feeorder = true;
                if (promise.groupHeadData[0].fmH_SpecialFeeFlag == true) {
                    $scope.FMH_SpecialFeeFlag = true;
                }
                else {
                    $scope.FMH_SpecialFeeFlag = false;
                }
                if (promise.groupHeadData[0].fmH_PDAFlag == true) {
                    $scope.FMH_PDAFlag = true;
                }
                else {
                    $scope.FMH_PDAFlag = false;
                }
                if (promise.groupHeadData[0].fmH_RefundFlag == true) {
                    $scope.FMH_RefundFlag = true;
                }
                else {
                    $scope.FMH_RefundFlag = false;
                }

                if (promise.groupHeadData[0].fmH_Flag == "G") {
                    $scope.hdtype = 1;
                }
                else if (promise.groupHeadData[0].fmH_Flag == "T") {
                    $scope.hdtype = 2;
                }
                else if (promise.groupHeadData[0].fmH_Flag == "E") {
                    $scope.hdtype = 3;
                }
                else if (promise.groupHeadData[0].fmH_Flag == "R") {
                    $scope.hdtype = 4;
                }
                else if (promise.groupHeadData[0].fmH_Flag == "A") {
                    $scope.hdtype = 5;
                }

                else if (promise.groupHeadData[0].fmH_Flag == "H") {
                    $scope.hdtype = 6;
                }
                else if (promise.groupHeadData[0].fmH_Flag == "N") {
                    $scope.hdtype = 7;
                }


                else if (promise.groupHeadData[0].fmH_Flag == "S") {
                    $scope.hdtype = 8;
                }
                else if (promise.groupHeadData[0].fmH_Flag == "F") {
                    $scope.hdtype = 9;
                }


                $scope.fmH_Id = promise.groupHeadData[0].fmH_Id;
            })
        }

        $scope.deactive = function (groupHeadData, SweetAlert) {
            
            var mgs = "";
            var confirmmgs = "";
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            if (groupHeadData.fmH_ActiveFlag == 1) {
                mgs = "Deactivate";
                confirmmgs = "Deactivated";

            }
            else {
                mgs = "Activate";
                confirmmgs = "Activated";

            }
            swal({
                title: "Are you sure?",
                text: "Do You Want To " + mgs + " Record?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + mgs + " it",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
        function (isConfirm) {
            if (isConfirm) {
                
                apiService.create("FeeHeadClg/deactivate", groupHeadData).
                then(function (promise) {
                    $scope.FMH_FeeName = "";
                    $scope.FMH_SpecialFeeFlag = "";
                    $scope.FMH_PDAFlag = "";
                    $scope.FMH_RefundFlag = "";
                    $scope.hdtype = "";
                    $scope.FMH_Order = "";
                    // $scope.students = promise.groupHeadData;
                    if (promise.returnval == true) {
                        swal("Record " + confirmmgs + " Successfully");
                    }
                    else if (promise.message == "used") {
                        swal("Record already Used");
                    }
                    else {
                        swal("Record " + confirmmgs + " Successfully");
                    }

                    $state.reload();
                })
            }
            else {
                swal("Record " + mgs + " Cancelled");
            }
        })
        }
    };
})();


