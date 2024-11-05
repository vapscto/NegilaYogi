
(function () {
    'use strict';
    angular
        .module('app')
        .controller('oralController', OralTestMarksEntryController)

    OralTestMarksEntryController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$filter', '$stateParams']
    function OralTestMarksEntryController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $filter, $stateParams) {
        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings != null && ivrmcofigsettings != undefined && ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }
        var configsettings = JSON.parse(localStorage.getItem("configsettings"));
        if (configsettings != null && configsettings != undefined && configsettings.length > 0) {
            var orltestby = configsettings[0].ispaC_ApplCutOffDateFlag;
        }
        $scope.hidestudents = false;
        $scope.all = true;
        $scope.sortKey = "pasR_Id";   //set the sortKey to the param passed
        $scope.reverse = true; //if true make it false and vice versa

        $scope.presentCountgrid = 0;

        $scope.currentPage1 = 1;
        $scope.itemsPerPageff = 10;
        $scope.pageddd = "dddd";
        $scope.reverse1 = true;

        $scope.angularData1 = {
            'nameList1': []
        };
        $scope.vals1 = [];

        $scope.obtMarks = {};
        //Date:23-12-2016 for displaying privileges.
        $scope.userPrivileges = "";
        var pageid = $stateParams.pageId;
        var privlgs = JSON.parse(localStorage.getItem("privileges"));
        for (var i = 0; i < privlgs.length; i++) {
            if (privlgs[i].pageId === pageid) {
                $scope.userPrivileges = privlgs[i];
            }
        }

        var configsettings = JSON.parse(localStorage.getItem("configsettings"));
        if (configsettings != null && configsettings != undefined && configsettings.length > 0) {
            for (var i = 0; i < configsettings.length; i++) {
                if (configsettings.length > 0) {
                    $scope.configurationsettings = configsettings[i];
                }
            }
        }

        if (configsettings != null && configsettings != undefined && configsettings[0].ispaC_OralTestApplFlag === 1) {
            $scope.myValue = true;
        }
        else if (configsettings[0].ispaC_OralTestApplFlag === 0) {
            $scope.myValue = false;
        }

        if (configsettings != null && configsettings != undefined && configsettings[0].ispaC_OralTestBy != "" && configsettings[0].ispaC_OralTestBy != null) {
            $scope.OralBy = configsettings[0].ispaC_OralTestBy;
            $scope.OralByDis = true;
        }
        else {
            $scope.OralByDis = false;
        }


        $scope.angularData = {
            'nameList': []
        };

        var absUrl = $location.absUrl();
        $scope.vals = [];
        $scope.fullname = "";
        $scope.BindGridData = {};
        $scope.stuRecord = {};
        $scope.writtenMarks = [];
        $scope.BindData = function () {
            var Id = 1;
            //  
            $scope.stu_List = false;
            $scope.currentPage = 1;
            $scope.itemsPerPage = 5;
            apiService.getURI("OralTestMarksEntry/Getdetails", Id).
                then(function (promise) {
                    if (promise.chq_config == false) {
                        swal("PLEASE CHECK CONFIGURATION  !");
                    } {
                        //  
                        //$scope.OralTestBy = promise.oralTestBy;
                        if (promise.oralTestSchedule.length > 0) {
                            $scope.ScheduleName = promise.oralTestSchedule;
                        }
                        else {
                            swal("No Schedule's are found!!");
                        }

                        //$scope.StudentName = promise.studentDetails;
                        //for (var i = 0; i < promise.studentDetails.length; i++) {
                        //    var name = promise.studentDetails[i].pasR_FirstName;
                        //    if (promise.studentDetails[i].pasR_MiddleName !== null) {
                        //        name += " " + promise.studentDetails[i].pasR_MiddleName;
                        //    }
                        //    if (promise.studentDetails[i].pasR_LastName != null) {
                        //        name += " " + promise.studentDetails[i].pasR_LastName;
                        //    }
                        //    $scope.vals.push(name);
                        //}
                        //angular.forEach($scope.vals, function (v, k) {
                        //    $scope.angularData.nameList.push({
                        //        'fullname': v
                        //    });
                        //});
                        //var j = 0;
                        //angular.forEach($scope.StudentName, function (obj) {
                        //    //Using bracket notation
                        //    obj["fullname"] = $scope.angularData.nameList[j].fullname;
                        //    j++;
                        //});

                        //added by suryan
                        $scope.albumNameArray1 = [];
                        for (var i = 0; i < promise.studentDetails.length; i++) {
                            if (promise.studentDetails[i].pasR_FirstName != '') {
                                if (promise.studentDetails[i].pasR_MiddleName !== null) {
                                    if (promise.studentDetails[i].pasR_LastName !== null) {

                                        $scope.albumNameArray1.push({ name: promise.studentDetails[i].pasR_FirstName + promise.studentDetails[i].pasR_MiddleName + promise.studentDetails[i].pasR_LastName, pasR_Id: promise.studentDetails[i].pasR_Id });
                                    }
                                    else {
                                        $scope.albumNameArray1.push({ name: promise.studentDetails[i].pasR_FirstName + promise.studentDetails[i].pasR_MiddleName, pasR_Id: promise.studentDetails[i].pasR_Id });
                                    }
                                }
                                else {
                                    $scope.albumNameArray1.push({ name: promise.studentDetails[i].pasR_FirstName, pasR_Id: promise.studentDetails[i].pasR_Id });
                                }
                            }
                        }
                        // $scope.StudentName = $scope.albumNameArray1;
                    }
                })
            $scope.sort = function (keyname) {
                $scope.sortKey = keyname;   //set the sortKey to the param passed
                $scope.reverse = !$scope.reverse; //if true make it false and vice versa
            }
        };

        $scope.checkmarks = function (total, obtain) {
            // 
            if (obtain > total) {
                //  $state.reload();

                //swal("Obtain marks must be less than or equal " + total);
                //   $scope.BindGrid();
                $scope.Marks.obtMarks = "";

            }

        };


        $scope.toggleAll = function (all) {
            var toggleStatus = $scope.all;
            angular.forEach($scope.StudentName, function (itm) {
                itm.checked = all;

            });
        }


        $scope.order = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }

        $scope.submitted = false;
        $scope.BindGrid = function (details) {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {

                $scope.checkboxchcked = [];
                for (var i = 0; i < details.length; i++) {
                    if (details[i].checked === true) {
                        $scope.checkboxchcked.push(details[i]);
                    }
                }

                $scope.stu_List = true;
                var StudentIDs = $scope.checkboxchcked;

                $scope.SelectedOralBy = $scope.OralBy;

                var data = {
                    "SelectedStudentDetails": StudentIDs,
                    "OralTestByPerson": $scope.SelectedOralBy,
                    "PAOTS_Id": $scope.paotS_Id
                };
                apiService.create("OralTestMarksEntry/GetOralTestMarks", data).
                    then(function (promise) {
                        $scope.hidestudents = true;
                        $scope.wirettenTestStudentMarks = promise;

                        $scope.albumNameArray = [];
                        for (var i = 0; i < $scope.wirettenTestStudentMarks.length; i++) {
                            if ($scope.wirettenTestStudentMarks[i].flagsubject == 'Common') {
                                $scope.albumNameArray.push({ header: $scope.wirettenTestStudentMarks[i].oralTestByPerson + " (" + $scope.wirettenTestStudentMarks[i].oral_MaxMarks + ")" });
                            }
                        }

                        $scope.oralmarks = $scope.albumNameArray;




                        $scope.albumNameArray1 = [];
                        for (var i = 0; i < $scope.wirettenTestStudentMarks.length; i++) {
                            if ($scope.wirettenTestStudentMarks[i].pasR_FirstName != '') {
                                if ($scope.wirettenTestStudentMarks[i].pasR_MiddleName != null && $scope.wirettenTestStudentMarks[i].pasR_MiddleName != '' && $scope.wirettenTestStudentMarks[i].pasR_MiddleName != "") {
                                    if ($scope.wirettenTestStudentMarks[i].pasR_LastName != null && $scope.wirettenTestStudentMarks[i].pasR_LastName != '' && $scope.wirettenTestStudentMarks[i].pasR_LastName != "") {

                                        $scope.albumNameArray1.push({ name: $scope.wirettenTestStudentMarks[i].pasR_FirstName + " " + $scope.wirettenTestStudentMarks[i].pasR_MiddleName + " " + $scope.wirettenTestStudentMarks[i].pasR_LastName, pasR_Id: $scope.wirettenTestStudentMarks[i].pasR_Id });
                                    }
                                    else {
                                        $scope.albumNameArray1.push({ name: $scope.wirettenTestStudentMarks[i].pasR_FirstName + " " + $scope.wirettenTestStudentMarks[i].pasR_MiddleName, pasR_Id: $scope.wirettenTestStudentMarks[i].pasR_Id });
                                    }
                                }
                                else {
                                    if ($scope.wirettenTestStudentMarks[i].pasR_LastName != null && $scope.wirettenTestStudentMarks[i].pasR_LastName != '' && $scope.wirettenTestStudentMarks[i].pasR_LastName != " ") {
                                        $scope.albumNameArray1.push({ name: $scope.wirettenTestStudentMarks[i].pasR_FirstName + " " + $scope.wirettenTestStudentMarks[i].pasR_LastName, pasR_Id: $scope.wirettenTestStudentMarks[i].pasR_Id });
                                    }
                                    else {
                                        $scope.albumNameArray1.push({ name: $scope.wirettenTestStudentMarks[i].pasR_FirstName, pasR_Id: $scope.wirettenTestStudentMarks[i].pasR_Id });
                                    }
                                }

                                //  $scope.wirettenTestStudentMarks= $scope.albumNameArray1;
                            }
                        }

                        $scope.datanames = $scope.albumNameArray1;
                        $scope.presentCountgrid = StudentIDs.length;

                    })
            }
            else {
                $scope.stu_List = false;
            }
        };

        $scope.isOptionsRequired = function () {
            return !$scope.StudentName.some(function (StuName) {
                return StuName.checked;
            });
        }

        //$scope.IsHidden = true;
        //$scope.ShowHide = function () {
        //    $scope.IsHidden = $scope.IsHidden ? false : true;
        //}

        //$scope.IsHidden1 = true;
        //$scope.ShowHide1 = function () {
        //    $scope.IsHidden1 = $scope.IsHidden1 ? false : true;
        //}

        $scope.cancel = function () {
            $state.reload();
        };


        $scope.submitted1 = false;
        $scope.saveWrittenTestMarksEntrydata = function (wirettenTestStudentMarks) {
            $scope.submitted1 = true;
            if ($scope.myForm1.$valid) {
                var data = {
                    "PAOTM_OralBy": $scope.OralBy,
                    "SelectedStudentMarksData": wirettenTestStudentMarks,
                    "OralTestScheduleAppFlag": $scope.myValue,
                    "PAOTS_Id": $scope.paotS_Id
                };
                apiService.create("OralTestMarksEntry/", data).
                    then(function (promise) {
                        swal("Record Saved Successfully");
                        $state.reload();




                    })
            }
            //$state.reload();
            //$scope.BindData();




        };



        // Get student by sch
        //  onSelectGetStudent(PAOTS_Id)
        $scope.onSelectGetStudent = function (id) {
            $scope.submitted = false;
            $scope.hidestudents = false;


            if (id === undefined || id === "") {
                id = 0;
            }

            var data = {
                "PAOTS_Id": id
            };

            apiService.create("OralTestMarksEntry/GetdetailsOnSchedule", data).then(function (promise) {

                if (promise.studentDetails.length > 0) {

                    $scope.StudentName = promise.studentDetails;
                    $scope.wirettenTestStudentMarks = [];
                    $scope.checkboxchcked = [];


                    //added by suryan
                    $scope.albumNameArray1 = [];
                    for (var i = 0; i < $scope.StudentName.length; i++) {
                        if ($scope.StudentName[i].pasR_FirstName != '') {
                            if ($scope.StudentName[i].pasR_MiddleName != null && $scope.StudentName[i].pasR_MiddleName != '' && $scope.StudentName[i].pasR_MiddleName != "") {
                                if ($scope.StudentName[i].pasR_LastName != null && $scope.StudentName[i].pasR_LastName != '' && $scope.StudentName[i].pasR_LastName != "") {

                                    $scope.albumNameArray1.push({ name: $scope.StudentName[i].pasR_FirstName + " " + $scope.StudentName[i].pasR_MiddleName + " " + $scope.StudentName[i].pasR_LastName, pasR_Id: $scope.StudentName[i].pasR_Id });
                                }
                                else {
                                    $scope.albumNameArray1.push({ name: $scope.StudentName[i].pasR_FirstName + " " + $scope.StudentName[i].pasR_MiddleName, pasR_Id: $scope.StudentName[i].pasR_Id });
                                }
                            }
                            else {
                                if ($scope.StudentName[i].pasR_LastName != null && $scope.StudentName[i].pasR_LastName != '' && $scope.StudentName[i].pasR_LastName != " ") {
                                    $scope.albumNameArray1.push({ name: $scope.StudentName[i].pasR_FirstName + " " + $scope.StudentName[i].pasR_LastName, pasR_Id: $scope.StudentName[i].pasR_Id });
                                }
                                else {
                                    $scope.albumNameArray1.push({ name: $scope.StudentName[i].pasR_FirstName, pasR_Id: $scope.StudentName[i].pasR_Id });
                                }
                            }

                            $scope.StudentName[i].name = $scope.albumNameArray1[i].name;
                            $scope.StudentName[i].checked = true;
                        }
                    }

                }
                else {

                    swal("No records found!!");
                }
            })

        }


        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.interacted1 = function (field) {
            return $scope.submitted1;
        };

        //search added by hemalekha
        //oral test student list
        $scope.searchValue = "";
        $scope.filterValue = function (obj) {

            return angular.lowercase(obj.fullname).indexOf(angular.lowercase($scope.searchValue)) >= 0;
        }

    }

})();