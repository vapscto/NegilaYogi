(function () {
    'use strict';
    angular.module('app').controller('PL_CI_StudentStatusController', PL_CI_StudentStatusController)

    PL_CI_StudentStatusController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$filter', '$timeout', 'Excel', '$q']
    function PL_CI_StudentStatusController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $filter, $timeout, Excel, $q) {



        $scope.submitted1 = false;
        $scope.indentapproveddetais = [];
        $scope.maxdate = new Date();
        $scope.obj = {};
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings !== null && admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }
        var temp = [];
        var year = "";
        $scope.yearfromdate = "";
        $scope.monthlist_temp = [];
        $scope.consolidata_id = [];
        $scope.get_deviationreport = [];
        $scope.imgname = logopath;
        $scope.data = [];



        $scope.Loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = 10;
            $scope.search = "";
            $scope.flags = "";

            var pageid = 2;
            apiService.getURI("PL_CI_StudentStatus/loaddata", pageid).then(function (promise) {
                $scope.studentname = promise.studentname;
                $scope.tablegrid = promise.tablegrid;
              
            });
        };




        $scope.submitted1 = false;
        $scope.saveRecord = function () {
            debugger;


            if ($scope.flag == true) {
                $scope.flag = 1;
            } else {
                $scope.flag = 0;
            }
            
            if ($scope.myForm1.$valid) {

                var data = {
                    "PLCISCHCOMJTSTS_Id": $scope.plcischcomjtstS_Id,
                    "PLCISCHCOMJTST_Id" : $scope.obj.comp_id,
                    "PLCISCHCOMJTSTS_InterviewRound" : $scope.obj.interview, 
                    "PLCISCHCOMJTSTS_Marks" : $scope.obj.request,
                    "PLCISCHCOMJTSTS_TestType": $scope.obj.marks,
                    "PLCISCHCOMJTSTS_Remarks" : $scope.obj.remarks,
                    "PLCISCHCOMJTSTS_SelectedFlg" : $scope.flag
                }
                apiService.create("PL_CI_StudentStatus/savedetails", data).then(function (promise) {
                    if (promise.returnval == true) {

                        swal('Record Saved Sucessfully');


                    }
                    else if (promise.PLCISCHCOMJTST_Id > 0) {
                        swal('Record Updated Successfully');
                    } else {
                        $scope.submitted1 = true;
                        swal('Record Not Saved Successfully');
                    }

                    $state.reload();
                });
            }
            else {
                $scope.submitted = true;
            }
        }

        $scope.Clearid = function () {
            $state.reload();
        };

        $scope.interacted = function (field) {            return $scope.submitted;        };




        $scope.edit = function (item) {
            var data = {
                "PLCISCHCOMJTSTS_Id": item.plcischcomjtstS_Id
            }
            apiService.create("PL_CI_StudentStatus/editdetails", data).then(function (promise) {
                if (promise.editdata != null && promise.editdata.length > 0) {
                    $scope.editdata = promise.editdata;

                    $scope.plcischcomjtstS_Id = $scope.editdata[0].plcischcomjtstS_Id;
                    $scope.obj.comp_id = $scope.editdata[0].plcischcomjtsT_Id;
                    $scope.obj.interview = $scope.editdata[0].plcischcomjtstS_InterviewRound;
                    $scope.obj.request = $scope.editdata[0].plcischcomjtstS_Marks;
                    $scope.obj.marks = $scope.editdata[0].plcischcomjtstS_TestType;
                    $scope.obj.remarks = $scope.editdata[0].plcischcomjtstS_Remarks;
                    $scope.flag = $scope.editdata[0].plcischcomjtstS_SelectedFlg;

                }
            })
        }


        $scope.deactive = function (item, SweetAlert) {
            $scope.PLCISCHCOMJTSTS_Id = item.plcischcomjtstS_Id;
            var dystring = "";
            if (item.plcischcomjtstS_ActiveFlag == true) {
                dystring = "Deactivate";
            }
            else if (item.plcischcomjtstS_ActiveFlag == false) {
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
                        apiService.create("PL_CI_StudentStatus/deactive", item).
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
                        swal("Record " + dystring + " Cancelled!!!");
                    }
                });
        }


    }
})();