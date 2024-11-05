
(function () {
    'use strict';
    angular
        .module('app')
        .controller('readmitstudentController', readmitstudentController)

    readmitstudentController.$inject = ['$rootScope', '$scope', '$state', '$location', 'apiService', 'Flash', '$http', '$q', '$stateParams', '$window', 'superCache']
    function readmitstudentController($rootScope, $scope, $state, $location, apiService, Flash, $http, $q, $stateParams, $window, superCache) {

        $rootScope.refresh = function () {
            $log.info("fired");
            eventService.events();
        }

        $scope.studentList = [];
        $scope.resultData = [];
        // On load page get all dropdown list
        $scope.LoadData = function () {
            var pageid = 2;
            apiService.getURI("readmitstudent/getalldetails", pageid).
                then(function (promise) {

                    $scope.yearList = promise.yearList;
                    $scope.yearList1 = promise.yearList;

                    $scope.classList = promise.classList;
                    $scope.classList1 = promise.classList;

                    $scope.studentList = promise.studentList;
                    $scope.studentList1 = promise.studentList;

                    $scope.section = promise.sectionList;
                    $scope.sectionlist1 = promise.sectionList;

                   // $scope.AMAY_Iddd = promise.yearList[0].asmaY_Id;



                })
        }


        $scope.GetStudentListByYearAndCLass_NS = function () {
            $scope.chckedIndexs = [];


            var data = {
                "AMAY_Id": $scope.AMAY_Iddd,
                "ASMCL_Id": $scope.ASMCL_Id,
                "ASMS_Id": $scope.ASMS_Id,

            }

            apiService.create("readmitstudent/GetStudentListByYearAndCLass", data).
                then(function (promise) {
                    if (promise != null) {
                        if (promise.studentList.length > 0 && promise.studentList != null) {
                            $scope.studentList1 = promise.studentList;
                        } else {
                            swal('No Records found to display');
                        }
                    }

                })
        }

        //table data new section 
        $scope.chckedIndexs = [];
        $scope.chckedIndexs1 = [];
        $scope.checkAll = function () {
            if ($scope.studentList.length > 0) {
                var toggleStatus = $scope.selectedAll;
                angular.forEach($scope.studentList, function (itm) {
                    itm.Selected = toggleStatus;
                    if ($scope.chckedIndexs.indexOf(itm) === -1) {
                        $scope.chckedIndexs.push(itm);
                    }
                    else {
                        $scope.chckedIndexs.splice($scope.chckedIndexs.indexOf(itm), 1);
                    }
                });
            } else {
                $scope.selectedAll = false;
            }
        }

        $scope.checkAll1 = function () {

            if ($scope.resultData.length > 0) {
                $scope.selectedAll1 = true;
                var toggleStatus = $scope.selectedAll1;
                angular.forEach($scope.resultData, function (itm) {
                    itm.Selected1 = toggleStatus;
                    if ($scope.chckedIndexs1.indexOf(itm) === -1) {
                        $scope.chckedIndexs1.push(itm);
                    }
                    else {
                        $scope.chckedIndexs1.splice($scope.chckedIndexs1.indexOf(itm), 1);
                    }
                });

            }
            else {
                $scope.selectedAll1 = false;
            }
        }

        $scope.test = function (data) {
            $scope.selectedAll = $scope.studentList.every(function (itm) {
                return itm.Selected;
            })
        };

        $scope.GetFirtTableData = function () {

            if ($scope.selectedAll == true) {
                angular.forEach($scope.studentList, function (student) {
                    $scope.resultData.push(student);
                });
            } else {
                angular.forEach($scope.studentList, function (student) {
                    if (student.Selected == true) {
                        $scope.resultData.push(student);
                    }
                });
            }
            $scope.studentList = $scope.studentList.filter(function (student) {
                return !student.Selected
            })

            $scope.checkAll1();
            $scope.checkAll();
        }

        $scope.RemoveSecondTableData = function () {
            if ($scope.selectedAll1 == true) {
                angular.forEach($scope.resultData, function (student) {
                    $scope.studentList.push(student);
                });
            } else {
                angular.forEach($scope.resultData, function (student) {
                    if (student.Selected1 == true) {
                        $scope.studentList.push(student);
                    }
                });
            }
            $scope.resultData = $scope.resultData.filter(function (student) {
                return !student.Selected1;
            })
            $scope.checkAll();
            $scope.checkAll1();
        }

        $scope.test1 = function (data) {
            $scope.selectedAll1 = $scope.resultData.every(function (itm) { return itm.Selected1; })
        };


        $scope.submitted = false;
        $scope.saveSectionNew = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {

                var plg = $scope.resultData;

                var data = {

                    "AMAY_ID_OLD": $scope.AMAY_Iddd,
                    "AMCL_ID_OLD": $scope.ASMCL_Id,
                    "AMCL_Id_New": $scope.ASMCL_Id_New,
                    "AMAY_Id_New": $scope.AMAY_Id_New,
                    "AMST_Id_New": $scope.AMST_Id_New,
                    "AMST_Id": $scope.AMST_Id,
                    //  "AMST_Id": $scope.ASMS_Id,
                    //   "AMST_Id_New":$scope.ASMS_Id_New,
                    resultData: plg
                }
                apiService.create("readmitstudent/", data).
                    then(function (promise) {
                        $scope.submitted = false;
                        if (promise.returnval == true) {
                            swal('Record Saved Successfully');
                            $scope.LoadData();
                            $state.reload();
                        }
                        else {
                            swal('Record Failed To Save');
                        }
                    })
            }

        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };


        //--New Section--// Clearid

        $scope.Clearid = function () {
            $scope.LoadData();
            $scope.submitted = false;
            $state.reload();
        }

        $scope.getnewjoinlist = function () {
            var data = {
                "AMAY_Id_New": $scope.AMAY_Id_New,
                "ASMCL_Id_New": $scope.ASMCL_Id_New,
                "ASMS_Id_New": $scope.ASMS_Id_New,
            }
            apiService.create("readmitstudent/getnewjoinlist", data).then
                (function (promoise) {
                    if (promoise != null) {
                        $scope.stdlistnew = promoise.newstudlist;
                    }
                    else {
                        swal("Records Not Found");
                    }
                })
        }

    }
})();