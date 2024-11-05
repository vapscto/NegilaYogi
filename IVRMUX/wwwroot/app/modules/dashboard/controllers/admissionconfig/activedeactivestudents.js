(function () {
    'use strict';
    angular
        .module('app')
        .controller('ActivateDeactivateStudentController', ActivateDeactivateStudentController)

    ActivateDeactivateStudentController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache']
    function ActivateDeactivateStudentController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {

        $scope.pagesrecord = {};
        $scope.students = [];
        $scope.adtable = true;
        $scope.searchValue = '';

        $scope.ddate = {};
        $scope.ddate = new Date();

        $scope.usrname = localStorage.getItem('username');

        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));

        var paginationformasters;
        var copty;

        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));

        if (ivrmcofigsettings != null) {
            if (ivrmcofigsettings.length > 0) {
                paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
                copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
            } else {
                paginationformasters = 10;
                copty = "";
            }
        } else {
            paginationformasters = 10;
            copty = "";
        }


        $scope.itemsPerPage = paginationformasters;
        $scope.coptyright = copty;

        var logopath = "";

        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));

        if (admfigsettings != null) {
            if (admfigsettings.length > 0) {
                logopath = admfigsettings[0].asC_Logo_Path;
            }
        } else {
            logopath = "";
        }

        $scope.imgname = logopath;


        $scope.getpagesname = function () {
            $scope.currentPage = 1;
            var pageid = 1;
            apiService.getURI("ActivateDeactivateStudent/getdata", pageid).
                then(function (promise) {
                    $scope.yearlist = promise.yearfilllist;
                    $scope.classlist = promise.classfilllist;
                    $scope.sectionlist = promise.sectionfilllist;
                });
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };


        $scope.propertyName = 'regno';
        $scope.sortBy = function (propertyName) {
            $scope.reverse = ($scope.propertyName === propertyName) ? !$scope.reverse : false;
            $scope.propertyName = propertyName;
        };


        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        };

        $scope.toggleAll = function () {
            var toggleStatus = $scope.all;
            angular.forEach($scope.students, function (itm) {
                itm.checkedvalue = toggleStatus;

            });
        };

        $scope.optionToggled = function () {
            $scope.all = $scope.students.every(function (itm) { return itm.checkedvalue; })
        };

        $scope.radiobtnchange = function () {
            $scope.angularData =
                {
                    'nameList': []
                };

            $scope.vals = [];
            $scope.ASMAY_Id = '';
            $scope.ASMCL_Id = '';
            $scope.ASMC_Id = '';
            $scope.submitted = false;
            $scope.adtable = true;
            $scope.all = '';
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
        };

        $scope.onacademicyearchange = function (yearlist) {
            var yearid = $scope.ASMAY_Id;
            var data = {
                "yearid": $scope.ASMAY_Id
            };

            apiService.create("ActivateDeactivateStudent/getACS", data).

                then(function (promise) {
                    $scope.classlist = promise.classfilllist;
                });
        };

        $scope.classchange = function () {
            if ($scope.ASMAY_Id > 0 && $scope.ASMCL_Id > 0 && $scope.ASMC_Id && $scope.AMST_SOL != "") {
                $scope.fillstudentlist();
            }
        };

        $scope.submitted = false;
        $scope.angularData =
            {
                'nameList': []
            };

        $scope.vals = [];

        $scope.fillstudentlist = function () {

            $scope.vals = [];
            $scope.angularData =
                {
                    'nameList': []
                };
            var data = {
                "yearid": $scope.ASMAY_Id,
                "asmcL_Id": $scope.ASMCL_Id,
                "sectionid": $scope.ASMC_Id,
                "AMST_SOL": $scope.AMST_SOL
            };



            apiService.create("ActivateDeactivateStudent/getS/", data).

                then(function (promise) {

                    if (promise.studentlist.length > 0) {
                        $scope.students = promise.studentlist;
                        $scope.presentCountgrid = $scope.students.length;

                        //for (var i = 0; i < promise.studentlist.length; i++) {

                        //    var name = promise.studentlist[i].stuFN;
                        //    if (promise.studentlist[i].stuMN !== null) {
                        //        name += " " + promise.studentlist[i].stuMN;
                        //    }
                        //    if (promise.studentlist[i].stuLN != null) {
                        //        name += " " + promise.studentlist[i].stuLN;
                        //    }
                        //    $scope.vals.push(name);

                        //}
                        //angular.forEach($scope.vals, function (v, k) {
                        //    $scope.angularData.nameList.push({
                        //        'fullname': v
                        //    });
                        //});
                        //var j = 0;
                        //angular.forEach($scope.students, function (obj) {
                        //    //Using bracket notation
                        //    obj["fullname"] = $scope.angularData.nameList[j].fullname;
                        //    j++;
                        //});

                        //angular.forEach($scope.students, function (objectt) {
                        //    if (objectt.fullname.length > 0) {
                        //        var string = objectt.fullname
                        //        objectt.namme = string.replace(/  +/g, ' ');
                        //    }
                        //});

                        $scope.adtable = false;
                    }
                    else {
                        swal("No Records Found");
                        $scope.adtable = true;
                    }
                });
        };

        $scope.Clearid_up = function () {
            $state.reload();
        };


        var studclear = [];
        $scope.Clearid = function () {

            $state.reload();
            $scope.AMST_SOL = 'S';
            $scope.ASMAY_Id = '';
            $scope.ASMCL_Id = '';
            $scope.ASMC_Id = '';
            $scope.submitted = false;
            $scope.adtable = true;
            $scope.all = '';
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();

        };
        $scope.isOptionsRequired = function () {
            return !$scope.students.some(function (options) {
                return options.checkedvalue;
            });
        };

        $scope.students1 = [];
        $scope.submitted = false;

        $scope.savedata = function (pagesrecord) {

            if ($scope.myForm.$valid) {

                if ($scope.all == true) {
                    angular.forEach(pagesrecord, function (student) {
                        $scope.students1.push(student);
                    });
                } else {
                    angular.forEach(pagesrecord, function (student) {
                        if (student.checkedvalue == true) {
                            $scope.students1.push(student);
                        }
                    });
                }

                if ($scope.AMST_SOL == 'S') {
                    $scope.AMST_SOL_activate = 'D';
                }
                else {
                    $scope.AMST_SOL_activate = 'S';
                }
                var data = {
                    "yearid": $scope.ASMAY_Id,
                    "asmcL_Id": $scope.ASMCL_Id,
                    "sectionid": $scope.ASMC_Id,
                    "AMST_SOL": $scope.AMST_SOL,
                    "AMST_SOL_activate": $scope.AMST_SOL_activate,
                    "remarks": $scope.remarks,
                    savetmpdata: $scope.students1
                };

                apiService.create("ActivateDeactivateStudent/savedata/", data).

                    then(function (promise) {

                        if (promise.returnval == true) {

                            if ($scope.AMST_SOL == 'S') {
                                swal('Record Deactivated Successfully');
                                $scope.Clearid();

                            }
                            else if ($scope.AMST_SOL == 'D') {
                                swal('Record Activated Successfully');
                                $scope.Clearid();
                            }
                        }
                        else {
                            swal('Record Not Activated/Deactivated Successfully');
                            $scope.Clearid();
                        }
                    });
            }
            else {
                $scope.submitted = true;
            }
        };

    }
})();

