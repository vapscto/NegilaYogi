/// <reference path="../../views/fees/installmentreport.html" />
(function () {
    'use strict';
    angular
        .module('app')
        .controller('SiblingEmployeeMapping', SiblingEmployeeMapping)

    SiblingEmployeeMapping.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache']
    function SiblingEmployeeMapping($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {

        $scope.samplearry = [
            { nname: 'First Child', amsT_ORDER: 1 },
            { nname: 'Second Child', amsT_ORDER: 2 },
            { nname: 'Third Child', amsT_ORDER: 3 },
            { nname: 'Fourth Child', amsT_ORDER: 4 },
            { nname: 'Fifth Child', amsT_ORDER: 5 }
        ];

        $scope.maingrid = false;
        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings !== null && ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        } else {
            paginationformasters = 10;
        }

        $scope.sortKey = "amsS_Id";   //set the sortKey to the param passed
        $scope.reverse = true; //if true make it false and vice versa
        $scope.searchthird = "";
        $scope.remflg = false;

        $scope.addnewbtn = true;

        $scope.formload = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;

            var pageid = 1;

            var data = {
                "pageid": 1
            };
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            };

            apiService.create("SiblingEmployeeMapping/getalldetails", data).
                then(function (promise) {

                    $scope.yearlst = promise.fillacademic;
                    $scope.staffcount = promise.fillstaff;
                    $scope.studentcount = promise.allstudentdata;

                    if (promise.alldata.length > 0) {
                        $scope.maingrid = true;
                        $scope.thirdgrid = promise.alldata;
                    }
                    else {
                        swal("No records found");
                        $scope.maingrid = false;
                    }
                });

            $scope.sort = function (keyname) {
                $scope.sortKey = keyname;   //set the sortKey to the param passed
                $scope.reverse = !$scope.reverse; //if true make it false and vice versa
            };
        };

        $scope.changeradio = function (chkradio) {

            if (chkradio === "stfoth") {
                $scope.showstaff = true;
            }
            else if (chkradio === "stud") {
                $scope.showstaff = false;
            }

            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "radiobtnval": $scope.stuchk
            };
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            };
            apiService.create("SiblingEmployeeMapping/selectradio", data).
                then(function (promise) {

                    if (promise.alldata.length > 0) {
                        if (chkradio === "stfoth") {
                            $scope.maingrid1 = true;
                            $scope.maingrid = false;
                            $scope.thirdgrid = promise.alldata;
                        }
                        else if (chkradio === "stud") {
                            $scope.maingrid = true;
                            $scope.maingrid1 = false;
                            $scope.thirdgrid = promise.alldata;
                        }
                    }
                    else {
                        swal("No records found");
                        $scope.maingrid = false;
                        $scope.maingrid1 = false;
                    }
                });
        };

        $scope.totalgrid = [];
        $scope.onselectacade = function (emptyrow) {

            $scope.grigview1 = true;

            $scope.totalgrid.push({
                'AMST_Id': emptyrow.AMST_Id,
            });
            $scope.PD = {};
        };

        $scope.sampcng = function (user, index) {
            for (var k = 0; k < $scope.totalgrid.length; k++) {
                var roll = parseInt(user.samplee);
                var arryind = $scope.totalgrid.indexOf($scope.totalgrid[k]);
                console.log(arryind);
                if (arryind !== index) {
                    if ($scope.totalgrid[k].samplee === roll && $scope.totalgrid[k].AMST_Id !== user.AMST_Id) {
                        swal("Already Exist");
                        $scope.totalgrid[index].samplee = "";
                        break;
                    }
                }
            }
        };

        $scope.onselectacade1 = function () {
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "radiobtnval": $scope.stuchk
            };
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            };
            apiService.create("SiblingEmployeeMapping/selectacademic", data).
                then(function (promise) {

                    $scope.totalgrid.push({
                        'AMST_Id': emptyrow.AMST_Id,
                        'studorder': emptyrow.studorder
                    });
                    $scope.PD = {};
                });
        };

        $scope.totalgridtest = [];

        $scope.addNew = function (totalgrid) {
            $scope.totalgrid.push({
                'AMST_Id': totalgrid.AMST_Id,
            });
            $scope.totalgridtest.push({
                'AMST_Id': totalgrid.AMST_Id,
            });
            if ($scope.totalgridtest.length > 0) {
                $scope.remflg = true;
            } else {
                $scope.remflg = false;
            }
            $scope.PD = {};
        };

        $scope.removerow = function (totalgrid) {
            $scope.totalgrid.pop({
                'AMST_Id': totalgrid.AMST_Id,
            });
            $scope.totalgridtest.pop({
                'AMST_Id': totalgrid.AMST_Id,
            });
            if ($scope.totalgridtest.length > 0) {
                $scope.remflg = true;
            } else {
                $scope.remflg = false;
            }
            // $scope.PD = {};
        };

        $scope.remove = function (totalgrid) {
            var newDataList = [];
            $scope.selectedAll = false;
            angular.forEach($scope.totalgrid, function (selected) {
                if (!selected.selected) {
                    newDataList.push(selected);
                }
            });
            $scope.totalgrid = newDataList;
        };

        $scope.onselectgroup = function (groupcount, emptyrow) {

            if ($scope.FMG_Id === "") {
                swal("Please Select Any Group !!!");
                $scope.grigview1 = false;
                $scope.totalgrid = [];
            }
            else {

                var groupid = $scope.FMG_Id;

                var data = {
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "FMG_Id": groupid
                };
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                };

                apiService.create("YearlyFeeGroupMapping/getadetailsongroup", data).
                    then(function (promise) {
                        $scope.grigview1 = true;
                        $scope.totalgrid = promise.alldata;

                        if ($scope.totalgrid.length === 0) {
                            $scope.totalgrid.push({
                                'AMST_Id': totalgrid.AMST_Id
                            });
                            $scope.PD = {};
                        }
                    });
            }
        };

        $scope.DeletRecord = function (user) {
            $scope.editEmployee = user.amsS_Id;
            var orgid = $scope.editEmployee;

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
                        apiService.DeleteURI("SiblingEmployeeMapping/Deletedetails", orgid).
                            then(function (promise) {

                                if (promise.returnval === "true") {
                                    swal("Record saved Sucessfully");
                                }
                                else {
                                    swal("Record saved Sucessfully");
                                }
                                $state.reload();
                            });
                    }
                    else {
                        swal("Record Deletion Cancelled");
                        $scope.formload();
                    }
                });
        };

        $scope.cleardata = function () {
            $state.reload();
        };

        $scope.EditMasterSectvalue = function (employee) {
            $scope.editEmployee = employee.fyghM_Id;
            var orgid = $scope.editEmployee;
            //orgid = 12;
            apiService.getURI("YearlyFeeGroupMapping/Editdetails", orgid).
                then(function (promise) {
                    $scope.addnewbtn = false;
                    $scope.FMG_Id = promise.alldata[0].fmG_Id;

                    //$scope.totalgrid.headcount.selected.FMH_Id = promise.alldata[0].fmH_Id;
                    //$scope.totalgrid.installmentcount.FMI_Id = promise.alldata[0].fmI_Id;

                    $scope.grigview1 = true;

                    $scope.totalgrid = promise.alldata;

                    if (promise.alldata[0].fyghM_FineApplicableFlag === 0) {

                        $scope.totalgrid.FYGHM_FineApplicableFlag = true;
                    }
                    if (promise.alldata[0].fyghM_Common_AmountFlag === 0) {

                        $scope.totalgrid.FYGHM_Common_AmountFlag = true;
                    }
                    if (promise.alldata[0].fyghM_ActiveFlag === 0) {

                        $scope.totalgrid.FYGHM_ActiveFlag = true;
                    }
                });
        };

        $scope.filterby = function () {
            var entereddata = $scope.search;

            var data = {
                "FMG_GroupName": $scope.searchthird,
                "FMH_FeeName": $scope.typethird
            };

            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            };

            apiService.create("YearlyFeeGroupMapping/1", data).
                then(function (promise) {
                    $scope.thirdgrid = promise.alldata;
                    swal("searched Successfully");
                });
        };

        $scope.interacted = function (field) {

            return $scope.submitted;
        };

        $scope.submitted = false;

        $scope.savedata = function (totalgrid) {

            if ($scope.myform.$valid) {
                var data = "";
                if ($scope.stuchk === "stud") {
                    data = {
                        savetmpdata: totalgrid,
                        "ASMAY_Id": $scope.ASMAY_Id,
                        "radiobtnval": $scope.stuchk
                    };
                }
                else if ($scope.stuchk === "stfoth") {
                    data = {
                        "HRME_Id": $scope.HRME_Id,
                        savetmpdata: totalgrid,
                        "ASMAY_Id": $scope.ASMAY_Id,
                        "radiobtnval": $scope.stuchk
                    };
                }


                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                };

                apiService.create("SiblingEmployeeMapping/savedata", data).
                    then(function (promise) {

                        if (promise.returnval === "true") {
                            swal("Record Saved Sucessfully");
                        }
                        else {
                            swal("Kindly contact Administrator");
                        }

                        $state.reload();

                    });
            }
            else {
                $scope.submitted = true;
            }

        };

       

    }

})();