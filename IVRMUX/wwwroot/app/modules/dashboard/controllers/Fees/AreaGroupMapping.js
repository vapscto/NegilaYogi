
(function () {
    'use strict';
    angular
.module('app')
.controller('AreaGroupMappingController', AreaGroupMapping)

    AreaGroupMapping.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache']
    function AreaGroupMapping($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {

        var paginationformasters;
        //var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        //if (ivrmcofigsettings.length > 0) {
        //    paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        //}
        paginationformasters = 5;
        $scope.dselectAll1 = false;
        $scope.sortKey = "fgaM_Id";   //set the sortKey to the param passed
        $scope.reverse = true; //if true make it false and vice versa
        $scope.searchValue = "";
        var paginationformasters1 = 5;
        $scope.currentPage = 1;
        $scope.itemsPerPage = paginationformasters;

        $scope.currentPage1 = 1;
        $scope.itemsPerPage1 = paginationformasters1;
        $scope.loaddata = function () {
            
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            var pageid = 2;
            apiService.getURI("AreaGroupMapping/getalldetails", pageid).
        then(function (promise) {
            
            $scope.totcountfirst = promise.fillareagroup.length;
            $scope.fillareagroup = promise.fillareagroup;
            $scope.fillarea = promise.fillarea;
            $scope.fillgroup = promise.fillgroup;       

            $scope.arrlistnew = promise.yearlist;
            $scope.arealist = promise.fillarea;

            $scope.students = promise.areadata;
        })
        }

        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }

        $scope.toggleAll1 = function () {
            var toggleStatus = $scope.selectAll1;
            angular.forEach($scope.fillarea, function (role) { role.selected = toggleStatus; });

        }

        $scope.optionToggled1 = function (selectedrole) {
            
            if ($scope.fgaM_Id > 0)
            {
                for (var i = 0; i < $scope.fillarea.length; i++) {
                    if (selectedrole.trmA_Id == $scope.fillarea[i].trmA_Id) {
                        if (selectedrole.selected == true)
                            $scope.fillarea[i].selected = true
                        else
                            $scope.fillarea[i].selected = true
                    }
                    else {
                        $scope.fillarea[i].selected = false;
                    }
                }
                
            }
            else
            {
                $scope.selectAll1 = $scope.fillarea.every(function (role) { return role.selected; })
            }
            
        }
        $scope.isOptionsRequired1 = function () {
            return !$scope.fillarea.some(function (options) {
                return options.selected;
            });
        }
      
        $scope.submitted = false;
        $scope.savedata = function (arrlistchk) {
            
            if ($scope.myForm.$valid) {
                $scope.albumNameArray = [];
                angular.forEach(arrlistchk, function (role) {
                    if (!!role.selected) $scope.albumNameArray.push(role);
                })

                if ($scope.albumNameArray.length === 0) {
                    swal('Please Select Area From List Atleast One....!')
                }
                else {
                    if ($scope.fgaM_Id > 0)
                    {
                        var TRMA_Id1 = 0;
                        for (var i = 0; i < $scope.albumNameArray.length; i++) {
                            TRMA_Id1 = $scope.albumNameArray[i].trmA_Id;
                        }                      
                        var data = {
                            "FGAM_Id": $scope.fgaM_Id,
                            "FMG_Id": $scope.FMG_GroupName,
                            "FGAM_ActiveFlag": true,
                            "FGAM_WayFlag": "1",
                            "TRMA_Id": TRMA_Id1
                            //"TempararyArrayList": $scope.albumNameArray,
                        }
                    }
                    else
                    {
                        var data = {
                            "FGAM_Id": $scope.fgaM_Id,
                            "FMG_Id": $scope.FMG_GroupName,
                            "FGAM_ActiveFlag": true,
                            "FGAM_WayFlag": "1",
                            "TempararyArrayList": $scope.albumNameArray,
                        }
                    }
                   
                    var config = {
                        headers: {
                            'Content-Type': 'application/json;'
                        }
                    }
                    apiService.create("AreaGroupMapping/", data).
                    then(function (promise) {
                        if (promise.returnduplicatestatus == 'Duplicate') {
                            swal("Record Already Exist");
                            $scope.cance1($scope.arrlistchk);
                            $scope.loaddata();
                            $scope.cance();
                        }

                        else if (promise.returnduplicatestatus == "Save") {
                            $scope.students = promise.groupData;
                            swal('Record Saved Successfully');                            
                            $scope.loaddata();
                            $scope.cance();
                        }
                        else if (promise.returnduplicatestatus == "NotSave") {
                            swal('Record Not Saved');
                        }
                        else if (promise.returnduplicatestatus == "Update") {
                            swal('Record Updated Successfully');
                            $scope.loaddata();
                            $scope.cance();
                            
                        }
                        else if (promise.returnduplicatestatus == "NotUpdate") {
                            swal('Record Not Updated');                                                      
                        }
                        else {
                            swal('Error');
                        }
                    })
                }
            }
            else {
                $scope.submitted = true;
            }
        };
        $scope.interacted = function (field) {

            return $scope.submitted;
        };
        $scope.deactive = function (user, SweetAlert) {
            var mgs = "";
            var confirmmgs = "";
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            if (user.fgaM_ActiveFlag == true) {
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

                apiService.create("AreaGroupMapping/deactivate", user).
                then(function (promise) {
                    if (promise.returnval == true) {
                        swal("Record " + confirmmgs + " Successfully");
                    }
                    else {
                        swal("Record " + confirmmgs + " Successfully");
                    }
                    $scope.loaddata();
                })

            }
            else {
                swal("Record " + mgs + " Cancelled");
            }
        });
        }
        $scope.cance = function () {
            
            $state.reload();
        };
        $scope.getorgvalue = function (employee, fillarea) {
            
            $scope.editEmployee = employee.fgaM_Id;
            var pageid = $scope.editEmployee;
            apiService.getURI("AreaGroupMapping/getdetails", pageid).
            then(function (promise) {
                for (var i = 0; i < fillarea.length; i++) {
                    name = fillarea[i].trmA_Id;
                    if (name == promise.fillareagroup[0].trmA_Id) {
                        fillarea[i].selected = true
                    }
                    else {
                        fillarea[i].selected = false;
                    }
                }
                $scope.fgaM_Id = promise.fillareagroup[0].fgaM_Id;
                $scope.FMG_GroupName = promise.fillareagroup[0].fmG_Id;
                $scope.fgaM_WayFlag = promise.fillareagroup[0].fgaM_WayFlag;
                $scope.dselectAll1 = true;
                if (promise.fillareagroup[0].fgaM_ActiveFlag == "1") {
                    $scope.fgaM_ActiveFlag = true;
                }
                else {
                    $scope.fgaM_ActiveFlag = false;
                }                
            })
        }

        $scope.interacted1 = function (field) {
            return $scope.submitted1;
        };
        $scope.cleardata = function () {
            $scope.loaddata();
        }
        $scope.savadataamount = function () {

            $scope.submitted1 = true;

            if ($scope.myForm1.$valid) {


                var data = {
                    "TRMAAMT_Id": $scope.TRMAAMT_Id,
                    "ASMAY_Id": $scope.ASMAY_Idnew,
                    "TRMA_Id": $scope.TRMA_Idnew,
                    "TRMAAMT_OneWayAmount": $scope.TRMAAMT_OneWayAmount,
                    "TRMAAMT_TwoWayAmount": $scope.TRMAAMT_TwoWayAmount
                }
                apiService.create("AreaGroupMapping/savedataamount", data).then(function (promise) {
                    if (promise != null) {
                        $scope.students = promise.areadata;
                        if (promise.message == "Add") {
                            if (promise.retrval == true) {
                                swal("Record Saved Successfully");
                            }
                            else {
                                swal("Failed To Save Record");
                            }
                        }
                        else if (promise.message == "Update") {
                            if (promise.retrval == true) {
                                swal("Record Update Successfully");
                            }
                            else {
                                swal("Failed To Update Record");
                            }
                        }
                        else if (promise.message == "Duplicate") {
                            swal("Record Already Exists");
                        }
                    }
                    else {

                    }
                   // $state.reload();
                    $scope.clear1();
                })
            } else {
                $scope.submitted1 = true;
            }
        }

        $scope.clear1 = function () {
          
            $scope.TRMAAMT_OneWayAmount = "";
            $scope.TRMAAMT_TwoWayAmount = "";
            var pageid = 2;
            apiService.getURI("AreaGroupMapping/getalldetails", pageid).
                then(function (promise) {
                    $scope.arrlistnew = promise.yearlist;
                    $scope.arealist = promise.fillarea;

                    $scope.students = promise.areadata;
                });

        }

        $scope.editamount = function (user) {
            var data = {
                "TRMAAMT_Id": user.TRMAAMT_Id
            }
            apiService.create("AreaGroupMapping/geteditdataamount", data).then(function (Promise) {
                if (Promise.editdatadetails.length != null) {
                    debugger;
                         $scope.ASMAY_Idnew = Promise.editdatadetails[0].asmaY_Id;
                    $scope.TRMA_Idnew = Promise.editdatadetails[0].trmA_Id;

                    $scope.TRMAAMT_Id = Promise.editdatadetails[0].trmaamT_Id;
                    $scope.TRMAAMT_OneWayAmount = Promise.editdatadetails[0].trmaamT_OneWayAmount;

                    $scope.TRMAAMT_TwoWayAmount = Promise.editdatadetails[0].trmaamT_TwoWayAmount;


                }
            })
        }


        $scope.deactiveamount = function (user, SweetAlert) {
            var mgs = "";
            var confirmmgs = "";
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }

       
            if (user.trmaamT_ActiveFlg === true) {
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
                        apiService.create("AreaGroupMapping/activedeactiveamount/", user).
                            then(function (promise) {
                                if (promise.message != null) {
                                    swal(promise.message);
                                }
                                else {
                                    if (promise.returnval == true) {
                                        swal(confirmmgs + " " + "Successfully.");
                                        $state.reload();
                                    }
                                    else {
                                        swal(confirmmgs + " " + " Successfully");
                                        $state.reload();
                                    }
                                    $scope.clear1();
                                }
                            })
                    }
                    else {
                        swal("Record " + mgs + " Cancelled");
                    }
                    $state.reload();
                });
        }

    };
})();


