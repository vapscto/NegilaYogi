(function () {
    'use strict';

    angular
        .module('app')
        .controller('SportsReportTeamPage', SportsReportTeamPage);

    SportsReportTeamPage.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', 'Excel', '$timeout'];

    function SportsReportTeamPage($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, Excel, $timeout) {


        $scope.itemsPerPage3 = 10;
        $scope.currentPage3 = 1;
        $scope.search3 = "";
        $scope.searchValue = "";
        $scope.DeleteRecord = {};
        $scope.obj = {};
        $scope.studentlist = false;
        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;
        $scope.sortReverse = true;
        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey == key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        }

        $scope.reverse3 = true;
        $scope.sort = function (key) {
            $scope.reverse3 = ($scope.sortKey == key) ? !$scope.reverse3 : $scope.reverse3;
            $scope.sortKey = key;
        }

        $scope.searchchkbx234 = "";
        $scope.searchchkbx23 = "";
        $scope.imgname = "";
        $scope.fromdate = new Date();
        $scope.studentdetails = [];
        $scope.act = 'add';

        //============TO  GEt The Values iN Grid================================
        $scope.BindData = function () {
            apiService.getDATA("SportsReportTeamPage/Getdetails").
                then(function (promise) {
                    $scope.yearlt = promise.yearlist;
                    $scope.categoryList = promise.categoryList;
                    $scope.CompetetionLevel = promise.competetionLevel;
                    $scope.SportsName = promise.sportsName;
                    $scope.alldata = promise.alldata;

                })
        };




        //ShowDetails in Dropdwon==================================================
        $scope.RepeatDta = function (spccE_Id) {
            $scope.MasterEvent = [];
            $scope.studentList = [];
            $scope.studentdetails = [];
            $scope.obj.spccE_Id = "";
            var data = {

                "ASMAY_Id": $scope.asmaY_Id,

            }
            apiService.create("SportsReportTeamPage/showdetails", data).then(function (promise) {

                $scope.MasterEvent = promise.masterEvent;
                if (spccE_Id > 0) {
                    $scope.obj.spccE_Id = spccE_Id;
                }

                if (promise.studentList != null && promise.studentList.length > 0) {
                    $scope.studentList = promise.studentList;
                    angular.forEach($scope.studentList, function (itm) {
                        itm.stud = false;
                    });
                }
            })
        }



        //Search grid records==========================================================================
        $scope.searchValue = "";
        $scope.searchchkbx = "";
        $scope.filterchkbx = function (obj) {
            return (angular.lowercase(obj.studentname)).indexOf(angular.lowercase($scope.searchchkbx)) >= 0;
        }

        //$scope.togchkbx = function () {
        //    $scope.usercheck = $scope.studentList.every(function (options) {
        //        return options.stud;
        //    });
        //}

        $scope.togchkbx = function () {
            $scope.usercheck = $scope.studentList.filter(function (options) {
                return options.stud;
            }).length;
        };



        $scope.all_check = function () {
            var toggleStatus = $scope.usercheck;
            angular.forEach($scope.studentList, function (itm) {
                itm.stud = toggleStatus;
            });
        }

        $scope.isOptionsRequired = function () {
            return !$scope.studentList.some(function (options) {
                return options.stud;
            });
        }



        //========================================Get Student Data in Grid
        $scope.get_student = function () {
            var data = {

                "ASMAY_Id": $scope.asmaY_Id,
                "ASMCL_Id": $scope.asmcL_Id
            }
            apiService.create("SportsReportTeamPage/get_student", data).then(function (promise) {
                $scope.studentList = promise.studentList;
            })
        }



        //save fields Data in database================================================================
        $scope.submitted = false;
        $scope.savedata = function () {
            $scope.studlsitdata1 = [];
            var spccetM_Id = 0;
            if ($scope.myForm.$valid) {
                if ($scope.studentdetails != null && $scope.studentdetails.length > 0) {
                    angular.forEach($scope.studentdetails, function (cls) {
                        $scope.studlsitdata1.push({
                            AMST_Id: cls.AMST_Id
                        });
                    })
                }
                else {
                    swal("Students Are  Not Selected !");
                    return;
                }
                if ($scope.spccetM_Id > 0) {
                    spccetM_Id = $scope.spccetM_Id;
                }
                var data = {
                    "ASMAY_Id": $scope.asmaY_Id,
                    "SPCCE_Id": $scope.obj.spccE_Id,
                    "SPCCMCL_Id": $scope.obj.spccmcL_Id,
                    "SPCCMCC_Id": $scope.spccmcC_Id,
                    "SPCCMSCC_Id": $scope.spccmscC_Id,
                    "SPCCETM_TeamName": $scope.spccetM_TeamName,
                    "SPCCETM_NoOfParticipants": $scope.spccetM_NoOfParticipants,
                    "TeamList": $scope.studlsitdata1,
                    "SPCCETM_Id": $scope.spccetM_Id,

                }
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                };
                apiService.create("SportsReportTeamPage/saveRecord", data).
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

        $scope.cancel = function () {
            $state.reload();
        }

        $scope.userselect = "";
        $scope.get_studlistt = function () {
            $scope.userselect = $scope.studentlsitdata.every(function (options) {
                return options.selected;
            });
        }

        $scope.check_allbox = function () {

            var toggleStatus1 = $scope.userselect;
            angular.forEach($scope.studentlsitdata, function (itm) {
                itm.selected = toggleStatus1;
            });
        }

        $scope.onmodelclick = function (id) {
            var data = {

                "SPCCETM_Id": id.SPCCETM_Id,
            }
            apiService.create("SportsReportTeamPage/get_modeldata", data).then(function (promise) {

                if (promise.modalsponsorlist.length > 0) {
                    $scope.modalsponsorlist = promise.modalsponsorlist;
                }
                else {
                    swal('Sponsor Not Found!!');
                }
            });
        };



        //=============================================Get Edit Data
        $scope.Editrecord = function (event) {
            $scope.act = 'edit';
            $scope.studentdetails = [];
            $scope.spccetM_Id = event.SPCCETM_Id;
            var data = {
                "SPCCETM_Id": event.SPCCETM_Id,
            }
            apiService.create("SportsReportTeamPage/EditRecord", data).then(function (promise) {

                if (promise.editrecord.length > 0) {
                    $scope.editrecord = promise.editrecord;
                    $scope.asmaY_Id = promise.editrecord[0].asmaY_Id;
                    $scope.RepeatDta(promise.editrecord[0].spccE_Id)
                    $scope.obj.spccmcL_Id = promise.editrecord[0].spccmcL_Id;
                    $scope.spccetM_TeamName = promise.editrecord[0].spccetM_TeamName;
                    $scope.spccetM_NoOfParticipants = promise.editrecord[0].spccetM_NoOfParticipants;
                    $scope.spccmcC_Id = promise.editrecord[0].spccmcC_Id;
                    $scope.spccmscC_Id = promise.editrecord[0].spccmscC_Id;
                    $scope.studentdetails = promise.studentList;
                }
                else {
                    swal('no record found!');
                }
            })
        };

        $scope.interacted = function (field) {

            return $scope.submitted;
        };



        //Add Checked student in grid And Remove duplicates==============================================
        $scope.Mapstudent = function () {
            if ($scope.spccetM_NoOfParticipants > 0) {
                if ($scope.studentList != null && $scope.studentList.length > 0) {
                    angular.forEach($scope.studentList, function (tt) {
                        if (tt.stud == true) {

                            if ($scope.studentdetails.length === 0) {
                                $scope.studentdetails.push({
                                    AMST_Id: tt.amsT_Id,
                                    amsT_FirstName: tt.amsT_FirstName,
                                    amsT_AdmNo: tt.amsT_AdmNo,
                                    asmcL_ClassName: tt.asmcL_ClassName,
                                    asmC_SectionName: tt.asmC_SectionName,
                                });
                            }

                            else if ($scope.studentdetails.length > 0 && ($scope.studentdetails.length < $scope.spccetM_NoOfParticipants)) {
                                var intcount = 0;
                                angular.forEach($scope.studentdetails, function (emp) {
                                    if (emp.AMST_Id === tt.amsT_Id) {
                                        intcount += 1;
                                    }
                                });
                                if (intcount === 0) {
                                    $scope.studentdetails.push({
                                        AMST_Id: tt.amsT_Id,
                                        amsT_FirstName: tt.amsT_FirstName,
                                        amsT_AdmNo: tt.amsT_AdmNo,
                                        asmcL_ClassName: tt.asmcL_ClassName,
                                        asmC_SectionName: tt.asmC_SectionName,
                                    });
                                }
                            }
                            else {
                                swal("Select  No.Of Participants  Full !")
                            }
                        }
                    })
                }
            }
            else {
                swal("Select  No.Of Participants !")
            }
           
        };

        $scope.SportsQuata = function () {
            $scope.studentdetails = [];
            $scope.spccetM_NoOfParticipants = 0;

            angular.forEach($scope.SportsName, function (emp) {
                if (emp.spccmscC_Id == $scope.spccmscC_Id) {
                    $scope.spccetM_NoOfParticipants = emp.spccmscC_NoOfMembers;
                }
            });
        }

        //Delete Added record==============================================
        $scope.delete = function (row, index) {
            $scope.act = 'add';
            $scope.dis = false;
            angular.forEach($scope.studentList, function (stu) {
                stu.stud = false;
            });

            for (var x = 0; x < $scope.studentdetails.length; x++) {
                if (x == index) {
                    $scope.studentdetails.splice(x, 1);
                }
            }
        };



        //=============================================Deactivate
        $scope.deactivate = function (newuser1, SweetAlertt) {
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }

            var mgs = "";
            if (newuser1.SPCCETM_ActiveFlag == false) {

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
                        apiService.create("SportsReportTeamPage/deactivate", newuser1).
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
        }

        //======================================================







    }
})();