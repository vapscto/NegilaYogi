(function () {
    'use strict';

    angular
        .module('app')
        .controller('SportsReportTeamController', SportsReportTeamController);

    SportsReportTeamController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', 'Excel', '$timeout'];

    function SportsReportTeamController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, Excel, $timeout) {
        $scope.searchValue = "";
        $scope.DeleteRecord = {};
        $scope.EditRecord = {};
        $scope.obj = {};
        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;
        $scope.teamlisttwo = [];

        $scope.usrname = localStorage.getItem('username');
        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));
        var paginationformasters;
        var copty;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings != null && ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_ReportPagination;
            copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
        }
        $scope.coptyright = copty;
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings != null && admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }

        //$scope.foesT_Date = new Date();

        //TO  GEt The Values iN Grid//
        $scope.BindData = function () {
            apiService.getDATA("SportsReportTeamPage/Getdetails").
                then(function (promise) {

                    $scope.teamlistone = promise.teamlistone;
                    //$scope.teamlisttwo = promise.teamlistone;
                    $scope.alldatas = promise.alldatas;
                })
        };

        $scope.validateTomintime = function (timedata) {

            $scope.fomsT_Time = "";
            $scope.totime1 = timedata; myForm
            var hh = $scope.totime1.getHours();
            var mm = $scope.totime1.getMinutes();
            $scope.min1 = timedata;

            $scope.min1.setMinutes(hh);
            $scope.min1.setMinutes(mm);
            $scope.fomsT_Time = timedata;
        }

        $scope.hstep = 1;
        $scope.mstep = 1;

        $scope.options = {
            hstep: [1, 2, 3],
            mstep: [1, 5, 10, 15, 25, 30]
        };


        //To SaveData
        $scope.submitted = false;
        $scope.savedata = function () {
            if ($scope.myForm.$valid) {
                var data = {
                    "SPCCETMSCH_Id": $scope.SPCCETMSCH_Id,
                    "SPCCETMSCH_Team1": $scope.SPCCETMSCH_Team1,
                    "SPCCETMSCH_Team2": $scope.SPCCETMSCH_Team2,
                    "SPCCETMSCH_Date": new Date($scope.foesT_Date),
                    "SPCCETMSCH_Time": $scope.fomsT_Time,
                    "SPCCETMSCH_Remarks": $scope.spccetmscH_Remarks,
                    "SPCCETMSCH_Result": $scope.spccetmscH_Result,
                }

                apiService.create("SportsReportTeamPage/SaveRecords", data).
                    then(function (promise) {
                        if (promise.msg == 'saved') {
                            swal("Record Saved Successfully!");
                            //swal("Saved Record" + promise.count1 + " Duplicate Record" + promise.count);
                            $state.reload();
                        }
                        else if (promise.msg == 'updated') {
                            swal("Record Updated Successfully!");
                            //swal("Updated Record" + promise.count1 + "Duplicate Record" + promise.count);
                            $state.reload();
                        }
                        else if (promise.msg == 'duplicate') {
                            swal("Record already exist");
                        }
                        else if (promise.msg == "savingFailed") {
                            swal("Failed to save record");
                        }
                        else if (promise.msg == "updateFailed") {
                            swal("Failed to update record");
                        }
                        else {
                            swal("Sorry...something went wrong");
                        }

                    })
            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };


        $scope.cancel = function () {
            $state.reload();
        }

        //EditData
        $scope.Editdata = function (editdata) {
            var data = {
                "SPCCETMSCH_Id": editdata.SPCCETMSCH_Id,
            }
            apiService.create("SportsReportTeamPage/GetEditData", data).then(function (promise) {

                if (promise.geteditdata.length > 0) {
                    $scope.geteditdata = promise.geteditdata;
                    $scope.SPCCETMSCH_Id = promise.geteditdata[0].spccetmscH_Id;
                    $scope.SPCCETMSCH_Team1 = promise.geteditdata[0].spccetmscH_Team1;
                    $scope.SPCCETMSCH_Team2 = promise.geteditdata[0].spccetmscH_Team2;
                    $scope.foesT_Date = new Date(promise.geteditdata[0].spccetmscH_Date);
                    $scope.fomsT_Time = promise.geteditdata[0].spccetmscH_Time;
                    $scope.spccetmscH_Result = promise.geteditdata[0].spccetmscH_Result;
                    $scope.spccetmscH_Remarks = promise.geteditdata[0].spccetmscH_Remarks;
                }
                else {
                    swal('no record found!');
                }

            })
        };


        //Deactivate
        $scope.deactivate = function (newuser1) {
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }

            var mgs = "";
            if (newuser1.SPCCETMSCH_ActiveFlag == false) {

                mgs = "Activate";

            }
            else {
                mgs = "Deactivate";
            }
            swal({
                title: "Are you sure?",
                text: "Do you want to  " + mgs + " Record?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55",
                confirmButtonText: "Yes, " + mgs + " it!",
                cancelButtonText: "Cancel..!",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("SportsReportTeamPage/deactivated", newuser1).
                            then(function (promise) {

                                if (promise.returnVal == true) {
                                    if (promise.msg != null) {
                                        swal(promise.msg);
                                        $state.reload();
                                    }
                                }
                                else {
                                    swal('Failed to Activate/Deactivate the Record');
                                }
                            })
                    } else {
                        swal("Cancelled");
                    }
                })
        };

             
        $scope.onchangesource = function () {
           $scope.teamlisttwo = [];           
            angular.forEach($scope.teamlistone, function (team) {
                if (team.spccetM_TeamName !== $scope.SPCCETMSCH_Team1) {

                    $scope.teamlisttwo.push(team);
                }

            });

            //$scope.teamlisttwo = $scope.remainingTeams;
        }

        
     
    }
})();