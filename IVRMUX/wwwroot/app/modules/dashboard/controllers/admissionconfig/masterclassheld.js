(function () {
    'use strict';
    angular.module('app').controller('masterclassheldController', masterclassheldController)

    masterclassheldController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache']
    function masterclassheldController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {

        $scope.checkboxchcked = [];
        $scope.sectioncheckboxchcked = [];
        $scope.noOfDays = [];
        $scope.monthList = [];
        $scope.noOfClassHeldCount = [];
        $scope.position = [];

        $scope.IsHidden = true;
        $scope.ShowHide = function () {
            //If DIV is hidden it will be visible and vice versa.
            $scope.IsHidden = $scope.IsHidden ? false : true;
        };

        $scope.IsHidden1 = true;
        $scope.ShowHide1 = function () {
            //If DIV is hidden it will be visible and vice versa.
            $scope.IsHidden1 = $scope.IsHidden1 ? false : true;
        };
        var name = "";
        $scope.loadData = function () {
            var pageid = 2;
            apiService.getURI("MasterClassHeld/getAllData", pageid).then(function (promise) {
                $scope.displaygrid = false;
                $scope.allAcademicYear = promise.allyear;
                $scope.CurrentacdYear = promise.currentYear;
                
                for (var i = 0; i < $scope.allAcademicYear.length; i++) {
                    name = parseInt($scope.allAcademicYear[i].asmaY_Id);
                    for (var j = 0; j < $scope.CurrentacdYear.length; j++) {
                        if (name === parseInt($scope.CurrentacdYear[j].asmaY_Id)) {
                            $scope.allAcademicYear[i].Selected = true;
                            $scope.AMAY_Id = $scope.CurrentacdYear[j].asmaY_Id;
                        }
                    }
                }

                $scope.asmaYFromDate = $scope.CurrentacdYear[0].asmaY_From_Date;
                $scope.classDrpDwn = promise.classDrpDwn;
                $scope.sectionDrpDwn = promise.sectionDrpDwn;
                $scope.monthlisttemp = promise.monthList;

                var date = new Date($scope.asmaYFromDate);

                $scope.monthNames = ["January", "February", "March", "April", "May", "June",
                    "July", "August", "September", "October", "November", "December"
                ];
                $scope.months = [];
                $scope.year = [];

                for (var i1 = 0; i1 < 12; i1++) {
                    $scope.months.push($scope.monthNames[date.getMonth()]);
                    $scope.year.push(date.getFullYear());
                    date.setMonth(date.getMonth() + 1);
                }
                $scope.monthByOrder = [];

                for (var i2 = 0; i2 < $scope.months.length; i2++) {
                    name = $scope.months[i2].trim();
                    for (var j1 = 0; j1 < promise.monthList.length; j1++) {
                        if (name.toLowerCase() === promise.monthList[j1].ivrM_Month_Name.toLowerCase().trim()) {
                            $scope.monthByOrder.push(promise.monthList[j1]);
                        }
                    }
                }

                $scope.monthList = $scope.monthByOrder;

                angular.forEach($scope.monthList, function (value1, i) {
                    $scope.monthList[i].ascH_ClassHeld = 0; $scope.usercheck = false; $scope.all_check();
                });

                angular.forEach($scope.monthList, function (value1, i) {
                    $scope.monthList[i].acd_Year = $scope.year[i];
                });
            });
        };

        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };

        $scope.isOptionsRequired = function () {
            return !$scope.sectionDrpDwn.some(function (options) {
                return options.ASMS_Id;
            });
        };

        $scope.onYearChange = function () {

            $scope.ASMCL_Id = "";
            $scope.ASMS_Id = "";
            $scope.monthList = [];
            $scope.displaygrid = false;

            angular.forEach($scope.allAcademicYear, function (yyy) {
                if (yyy.asmaY_Id === parseInt($scope.AMAY_Id)) {
                    $scope.asmaYFromDate = yyy.asmaY_From_Date;
                    var date = new Date($scope.asmaYFromDate);
                    $scope.monthNames = ["January", "February", "March", "April", "May", "June",
                        "July", "August", "September", "October", "November", "December"];

                    $scope.months = [];
                    $scope.year = [];

                    for (var i = 0; i < 12; i++) {
                        $scope.months.push($scope.monthNames[date.getMonth()]);
                        $scope.year.push(date.getFullYear());                       
                        date.setMonth(date.getMonth() + 1);
                    }
                    $scope.monthByOrder = [];
                    for (var i = 0; i < $scope.months.length; i++) {
                        name = $scope.months[i].trim();
                        for (var j = 0; j < $scope.monthlisttemp.length; j++) {
                            if (name.toLowerCase() === $scope.monthlisttemp[j].ivrM_Month_Name.toLowerCase().trim()) {
                                $scope.monthByOrder.push($scope.monthlisttemp[j]);
                            }
                        }
                    }

                    $scope.monthList = $scope.monthByOrder;

                    angular.forEach($scope.monthList, function (dd) {
                        dd.Selected = false;
                    });

                    angular.forEach($scope.monthList, function (value1, i) {
                        $scope.monthList[i].ascH_ClassHeld = 0;  
                        
                    });
                    angular.forEach($scope.monthList, function (value1, i) {
                        $scope.monthList[i].acd_Year = $scope.year[i];
                    });
                }
            });

            //if ($scope.checkboxchcked.length > 0 && $scope.ASMS_Id > 0 && $scope.AMAY_Id > 0 && $scope.ASMCL_Id > 0) {
            //    $scope.getNoOfClassHeld($scope.position);
            //}
        };

        $scope.onclassChange = function () {           
            $scope.asmC_Id = "";         
            $scope.displaygrid = false;
            //$scope.usercheck = false; $scope.all_check(); $scope.displaygrid = false; $scope.asmC_Id = '';
            //if ($scope.checkboxchcked.length > 0 && $scope.AMAY_Id > 0 && $scope.ASMS_Id > 0 && $scope.ASMCL_Id > 0) {
            //    $scope.getNoOfClassHeld($scope.position);
            //}
        };

        $scope.getSelectedSection = function (data) {            
            $scope.displaygrid = false;
            $scope.ASMS_Id = $scope.asmC_Id;            
            if ($scope.AMAY_Id > 0 && $scope.ASMCL_Id > 0) {
                $scope.getNoOfClassHeld($scope.position);
            }
        };



        $scope.No_Of_ClassHeld = function (classheld) {
            if (classheld !== nul && classheld !== undefined && classheld !== "") {
                $scope.noOfDays.push(classheld);
                return true;
            }
            else {
                var index1 = $scope.noOfDays.indexOf(classheld);
                $scope.noOfDays.splice(index1);
                swal("Please Enter Value For Class Held Column");
                return false;
            }
        };

        $scope.getSelectedData = function (data, position) {
            if ($scope.checkboxchcked.indexOf(data) === -1) {
                $scope.checkboxchcked.push(data);
                $scope.position.push(position);
                $scope.getNoOfClassHeld(position);
            }
            else {
                var index = $scope.checkboxchcked.indexOf(data);
                $scope.checkboxchcked.splice(index);
                var index1 = $scope.noOfDays.indexOf(data.ASCH_ClassHeld);
                $scope.noOfDays.splice(index1);
                var index2 = $scope.position.indexOf(data);
                $scope.position.splice(index2);
            }
        };

        $scope.getNoOfClassHeld = function (position) {
            var monthList = $scope.checkboxchcked;
            console.log(monthList);
            var listOfClassHeld = {
                selectedmonthList: monthList,
                "AMAY_Id": $scope.AMAY_Id,
                "ASMCL_Id": $scope.ASMCL_Id,
                "ASMS_Id": $scope.ASMS_Id
            };

            apiService.create("MasterClassHeld/getClassHeld/", listOfClassHeld).then(function (promise) {
                console.log(promise.noOfClassHeldCount);
                $scope.displaygrid = true;
                angular.forEach(promise.noOfClassHeldCount, function (value1, i) {
                    if ($scope.monthList[$scope.position[i]].ascH_ClassHeld > 0) {
                        //$scope.monthList[$scope.position[i]].ascH_ClassHeld = $scope.monthList[$scope.position[i]].ascH_ClassHeld;
                        $scope.monthList[$scope.position[i]].ascH_ClassHeld = promise.noOfClassHeldCount[i];
                    }
                    else if (promise.noOfClassHeldCount[i] == 0) {
                        // $scope.monthList[$scope.position[i]].ascH_ClassHeld = user.ascH_ClassHeld;
                    }
                    else {
                        $scope.monthList[$scope.position[i]].ascH_ClassHeld = promise.noOfClassHeldCount[i];
                    }
                });
            });
        };

        $scope.isOptionsRequired1 = function () {
            return !$scope.monthList.some(function (options) {
                return options.Selected;
            });
        };

        $scope.saveclassHeld = function (data) {

            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var monthList = $scope.checkboxchcked;
                var listOfClassHeld = {
                    selectedmonthList: monthList,
                    "AMAY_Id": $scope.AMAY_Id,
                    "ASMCL_Id": $scope.ASMCL_Id,
                    "ASMS_Id": $scope.ASMS_Id
                };
                apiService.create("MasterClassHeld", listOfClassHeld).then(function (promise) {
                    if (promise.msgcount > 0) {
                        for (var i = 0; i < promise.msgcount; i++) {
                            alert(promise.message[i]);
                        }
                    }
                    if (promise.returnVal === true) {
                        $scope.Clearid();
                        swal('Data Saved Successfully');
                    }
                    else {
                        swal('Failed to Save/Update Data');
                    }
                });
            }
        };

        $scope.Clearid = function () {
            $state.reload();
        };

        $scope.CalNoOfDays = function daysInMonth(m, y) { // m is 0 indexed: 0-11
            y = parseInt(y);
            switch (m) {
                case 2:
                    return (y % 4 === 0 && y % 100) || y % 400 === 0 ? 29 : 28;
                case 9: case 4: case 6: case 11:
                    return 30;
                default:
                    return 31;
            }
        };

        $scope.all_check = function () {
            var toggleStatus = $scope.usercheck;            
            if (toggleStatus === true) {
                angular.forEach($scope.monthList, function (user, i) {
                    user.Selected = toggleStatus;
                    $scope.checkboxchcked.push(user);
                    $scope.position.push(i);
                });

                $scope.getNoOfClassHeld();
            }
            else {
                angular.forEach($scope.monthList, function (user, i) {
                    user.Selected = toggleStatus;
                    var index = $scope.checkboxchcked.indexOf(user);
                    $scope.checkboxchcked.splice(index);
                    var index1 = $scope.noOfDays.indexOf(user.ASCH_ClassHeld);
                    $scope.noOfDays.splice(index1);
                    var index2 = $scope.position.indexOf(user);
                    $scope.position.splice(index2);
                });
            }
        };

    }
})();