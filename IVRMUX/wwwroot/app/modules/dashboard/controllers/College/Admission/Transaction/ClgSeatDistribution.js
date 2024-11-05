(function () {
    'use strict';
    angular
        .module('app')
        .controller('ClgSeatDistributionController', ClgSeatDistributionController)
    ClgSeatDistributionController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache']
    function ClgSeatDistributionController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {


        $scope.searc_button = true;
        $scope.sortKey = 'acscD_Id';
        $scope.sortReverse = true;
        $scope.searchValue = "";

        $scope.percent = true;
        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        } else {
            paginationformasters = 10;
        }

        $scope.ddate = {};
        $scope.ddate = new Date();

        $scope.usrname = localStorage.getItem('username');
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }
        $scope.imgname = logopath;
        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));
        //----------------------------------Page Load------------------------------------------------
        $scope.clgSeatDistributionload = function () {
            var pageid = 2;
            apiService.getURI("ClgSeatDistribution/getalldetails", pageid).
                then(function (promise) {

                    $scope.currentPage = 1;
                    $scope.itemsPerPage = paginationformasters;
                    $scope.getSeatsdetails = promise.getSeatsdetails;
                    $scope.presentCountgrid = $scope.getSeatsdetails.length;

                    $scope.getYear = promise.getYear;
                })
        };

        //-----------Academic year Change
        $scope.onyearchange = function () {

            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
            }
            apiService.create("ClgSeatDistribution/getCoursedata", data).
                then(function (promise) {

                    $scope.getCourse = promise.getCourse;

                })
        }
        //-----------Course Change
        $scope.onCoursechange = function () {

            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
                "AMCO_Id": $scope.amcO_Id,

            }
            apiService.create("ClgSeatDistribution/getBranchdata", data).
                then(function (promise) {

                    $scope.getBranch = promise.getBranch;
                })
        }
        //-----------Branch Change
        $scope.onBranchchange = function () {

            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
                "AMCO_Id": $scope.amcO_Id,
                "AMB_Id": $scope.amB_Id,
            }
            apiService.create("ClgSeatDistribution/getSemesterdata", data).
                then(function (promise) {


                    $scope.getBranchDetails = promise.getBranchDetails;
                    $scope.amB_StudentCapacity = $scope.getBranchDetails[0].amB_StudentCapacity;
                    $scope.getSemester = promise.getSemester;

                    $scope.usercheck = true;
                    angular.forEach($scope.getSemester, function (se) {
                        se.checkedsub = true;
                    })

                })
        }
        //---- Semester Select All
        $scope.all_check = function () {

            var toggleStatus = $scope.usercheck;
            angular.forEach($scope.getSemester, function (itm) {
                itm.checkedsub = toggleStatus;
            });
        }
        $scope.togchkbx = function () {

            $scope.usercheck = $scope.getSemester.every(function (options) {
                return options.checkedsub;
            });
        }

        $scope.isOptionsRequired = function () {
            return !$scope.getSemester.some(function (options) {
                return options.checkedsub;
            });
        }

        $scope.submitted = false;
        //----------------------- category Grid   
        $scope.get_Category = function () {
            $scope.submitted = true;
            if ($scope.myForm1.$valid) {
                var pageid = 2;

                var data = {
                    "ASMAY_Id": $scope.asmaY_Id,
                    "AMCO_Id": $scope.amcO_Id,
                    "AMB_Id": $scope.amB_Id,
                }


                apiService.create("ClgSeatDistribution/get_Category", data).then(function (promise) {
                    $scope.getSeatCategory = promise.getSeatCategory;
                    $scope.getSeatsdetails1 = promise.getSeatsdetails1;

                    if ($scope.getSeatsdetails1.length > 0) {

                        angular.forEach($scope.getSeatCategory, function (sa) {

                            angular.forEach($scope.getSeatsdetails1, function (sav) {

                                if (sav.acQ_Id == sa.acQ_Id && sav.acqC_Id == sa.acqC_Id) {
                                    sa.xyz = true;
                                    sa.acscD_SeatPer = sav.acscD_SeatPer;
                                    sa.acscD_SeatNos = sav.acscD_SeatNos;
                                    sa.acscD_Remarks = sav.acscD_Remarks;
                                } else {

                                }
                            })

                        })



                    }


                })
            } else {
                $scope.submitted = true;
            }

        };

        $scope.toggleAll = function () {
            angular.forEach($scope.getSeatCategory, function (subj) {
                subj.xyz = $scope.all;
            })
        };

        $scope.optionToggled = function () {
            $scope.all = $scope.getSeatCategory.every(function (itm) { return itm.xyz; });
            angular.forEach($scope.getSeatCategory, function (dd) {
                if (dd.xyz == false) {
                    dd.acscD_SeatPer = 0;
                    dd.acscD_SeatNos = 0;
                }
            })

        };
        $scope.checkper = {};

        $scope.check_seats = function (user) {

            $scope.total_leftSeat = 0;
            $scope.totnumsets = 0;

            angular.forEach($scope.getSeatCategory, function (seat) {
                if (seat.xyz == true) {
                    $scope.checkper = parseInt(seat.acscD_SeatPer);

                    seat.acscD_SeatNos = ($scope.amB_StudentCapacity / 100) * $scope.checkper;

                    //$scope.total_leftSeat = $scope.amB_StudentCapacity - seat.acscD_SeatNos;
                }
                $scope.totnumsets = $scope.totnumsets + seat.acscD_SeatNos;

                if ($scope.totnumsets > $scope.amB_StudentCapacity) {
                    //swal("Please Enter Value less than or equal to Total Strength :" +$scope.totnumsets+);                        
                    swal("Total Seats Available Is : " + $scope.amB_StudentCapacity + " Selected Seats Are :" + $scope.totnumsets)
                    seat.xyz = false;
                    seat.acscD_SeatNos = 0;
                    seat.acscD_SeatPer = 0;
                }

            })


        }
        //------------------------------Save
        $scope.savedata = function () {
            $scope.submitted = true;
            angular.forEach($scope.getSeatCategory, function (obj) {
                if (obj.xyz == true) {
                    if (obj.acscD_SeatPer == 0) {
                        swal("Please enter seat percentage more than 0%");
                    }
                }
            })
            if ($scope.myForm1.$valid) {


                $scope.sem = [];
                var chk_count = 0;
                angular.forEach($scope.getSemester, function (itm) {
                    if (itm.checkedsub == true) {
                        chk_count += 1;
                        $scope.sem.push(itm.amsE_Id);
                    }
                });
                $scope.seats = [];
                angular.forEach($scope.getSeatCategory, function (seat) {
                    if (seat.xyz) {
                        $scope.seats.push({ acQ_Id: seat.acQ_Id, acqC_Id: seat.acqC_Id, acscD_SeatPer: seat.acscD_SeatPer, acscD_SeatNos: seat.acscD_SeatNos, acscD_Remarks: seat.acscD_Remarks });
                    }
                })
                if ($scope.seats.length > 0) {
                    var data = {
                        "ASMAY_Id": $scope.asmaY_Id,
                        "AMCO_Id": $scope.amcO_Id,
                        "AMB_Id": $scope.amB_Id,
                        "AMSE_Sem": $scope.sem,
                        "seat_data": $scope.seats,
                    }
                    apiService.create("ClgSeatDistribution/savedata", data).then(function (promise) {
                        if (promise.returnval == true) {
                            if (promise.acscD_Id == 0 || promise.acscD_Id < 0) {
                                swal("Record saved Successfully");
                            } else if (promise.acscD_Id > 0) {
                                swal("Record Upadte Successfully");
                            }
                        }
                        else if (promise.returnduplicatestatus == 'Duplicate') {
                            swal('Record already exist');
                        }
                        else {
                            if (promise.acscD_Id == 0 || promise.acscD_Id < 0) {
                                swal('Failed to save, please contact administrator');
                            }
                            else if (promise.acscD_Id > 0) {
                                swal('Failed to update, please contact administrator');
                            }
                        }
                        $scope.clear();
                        $scope.clgSeatDistributionload();
                    })
                }
                else {
                    swal("Select Atleast Subject To Save Data!!!");
                }
            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.clear = function () {
            $state.reload();
        }


        //--------------------------Grid
        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey == key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        }

        $scope.IsHidden = true;
        $scope.ShowHide = function () {
            $scope.IsHidden = $scope.IsHidden ? false : true;
        }

        $scope.search_box = function () {
            if ($scope.searchValue != "" || $scope.searchValue != null) {
                $scope.searc_button = false;
            }
            else {
                $scope.searc_button = true;
            }
        }

        $scope.filterValue = function (obj) {
            return (angular.lowercase(obj.amcO_CourseName)).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
                (angular.lowercase(obj.amB_BranchName)).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
                (angular.lowercase(obj.amsE_SEMName)).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
                (angular.lowercase(obj.acQ_QuotaName)).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
                (angular.lowercase(obj.acqC_CategoryName)).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
                (JSON.stringify(obj.acscD_SeatPer)).indexOf($scope.searchValue) >= 0 ||
                (JSON.stringify(obj.acscD_SeatNos)).indexOf($scope.searchValue) >= 0;

        };
    }
})();