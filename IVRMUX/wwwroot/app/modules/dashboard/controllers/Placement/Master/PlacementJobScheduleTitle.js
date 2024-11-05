(function () {
    'use strict';
    angular.module('app').controller('PlacementJobScheduleTitleController', PlacementJobScheduleTitleController)

    PlacementJobScheduleTitleController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$filter', '$timeout', 'Excel', '$q']
    function PlacementJobScheduleTitleController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $filter, $timeout, Excel, $q) {



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
            apiService.getURI("PlacementJobScheduleTitle/loaddata", pageid).then(function (promise) {

                if (promise.shiftrole != null && promise.shiftrole.length > 0) {
                    $scope.flags = promise.shiftrole[0].flag;

                    $scope.scompany = promise.scompany;
                    $scope.scourse = promise.scourse;
                    $scope.sbranch = promise.sbranch;
                    $scope.schedulestudentname = promise.schedulestudentname;
                    $scope.schedulestudentnames = promise.schedulestudentnames;

                    $scope.studentgridtable = promise.studentgridtable;
                    $scope.admingridtable = promise.admingridtable;
                    $scope.registration = promise.registration;


                }



            });
        };


        $scope.submitted1 = false;
        $scope.saveRecord = function () {


            $scope.from_dates = new Date($scope.obj.Fromdate).toDateString();
            $scope.submitted = true;
            if ($scope.myForm1.$valid) {
                var data = {
                    "PLCISCHCOMJT_Id": $scope.obj.comp_id,
                    "AMCST_Id": $scope.obj.studentnamedetails,
                    "fromdate": $scope.from_dates,
                    "PLCISCHCOMJTST_Id": $scope.obj.plcischcomjtsT_Id
                }
                apiService.create("PlacementJobScheduleTitle/savedetails", data).then(function (promise) {
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

        $scope.interacted1 = function (field) {            return $scope.submitted1;        };
       
        
        


        $scope.all_check = function () {
            var toggleStatus = $scope.obj.checkallm;
            angular.forEach($scope.scourse, function (itm) {
                itm.selected = toggleStatus;
            });
        }

        $scope.optionToggled1 = function (x) {
            $scope.obj.checkallm = $scope.scourse.every(function (itm) { return itm.selected; })

        }


        $scope.all_check1 = function () {
            var toggleStatus = $scope.obj.checkallm1;
            angular.forEach($scope.sbranch, function (itm) {
                itm.selected1 = toggleStatus;
            });
        }

        $scope.optionToggled2 = function (x) {
            $scope.obj.checkallm1 = $scope.sbranch.every(function (itm) { return itm.selected1; })

        }


        $scope.all_check2 = function () {
            var toggleStatus = $scope.obj.checkallm2;
            angular.forEach($scope.schedulestudentname, function (itm) {
                itm.selected2 = toggleStatus;
            });
        }

        $scope.optionToggled3 = function (x) {
            $scope.obj.checkallm2 = $scope.schedulestudentname.every(function (itm) { return itm.selected2; })

        }




        $scope.edit = function (item) {
            var data = {
                "PLCISCHCOMJTST_Id": item.plcischcomjtsT_Id
            }
            apiService.create("PlacementJobScheduleTitle/editdetails", data).then(function (promise) {
                if (promise.editdata != null && promise.editdata.length > 0) {
                    $scope.editdata = promise.editdata;

                    

                    $scope.obj.comp_id = $scope.editdata[0].plmcomP_CompanyName;
                    $scope.obj.comp_id = $scope.editdata[0].plcischcomjT_Id;
                    $scope.obj.studentnamedetails = $scope.editdata[0].amcsT_MiddleName;
                    $scope.obj.studentnamedetails = $scope.editdata[0].amcsT_Id;
                    $scope.obj.Fromdate = $scope.editdata[0].plcischcomjtsT_Date;
                    $scope.obj.plcischcomjtsT_Id = $scope.editdata[0].plcischcomjtsT_Id;
                }
            })
        }


        $scope.deactive = function (item, SweetAlert) {
            $scope.PLCISCHCOMJTST_Id = item.plcischcomjtsT_Id;
            var dystring = "";
            if (item.plcischcomjtsT_ActiveFlag == true) {
                dystring = "Deactivate";
            }
            else if (item.plcischcomjtsT_ActiveFlag == false) {
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
                        apiService.create("PlacementJobScheduleTitle/deactive", item).
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





        $scope.selected_Inst = [];
        $scope.approveddetails = [];
        $scope.approveddetailss = [];
        $scope.get_Report = function () {

            $scope.to_date = new Date($scope.obj.ToDate).toDateString();

            angular.forEach($scope.scourse, function (role) {
                if (role.selected == true) {
                    $scope.selected_Inst.push({
                        amcoid: role.idschedulecourse
                    });
                }
            })


            angular.forEach($scope.sbranch, function (role) {
                if (role.selected1 == true) {
                    $scope.approveddetails.push({
                        ambid: role.idschedulebranch
                    });
                }
            })

            angular.forEach($scope.schedulestudentname, function (role) {
                if (role.selected2 == true) {
                    $scope.approveddetailss.push({
                        sid: role.idschedulestudentname
                    });
                }
            })


            $scope.to_date = new Date($scope.Todate).toDateString();

            var data = {
                "grid_arraydatasamb": $scope.approveddetails,
                "grid_arraydatassamco": $scope.selected_Inst,
                "grid_arraydatasss": $scope.approveddetailss,
                "todate": $scope.to_date
            }

            apiService.create("PlacementJobScheduleTitle/report", data).then(function (promise) {
                if (promise.admingridtable != null && promise.admingridtable.length > 0) {
                    $scope.DisplayApprovalTable = promise.admingridtable;
                    $scope.presentCountgrid = $scope.DisplayApprovalTable.length;


                    

                }

                else {
                    swal("No Record  Found..... !!");

                }

            });

        }









    }
})();