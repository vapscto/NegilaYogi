
(function () {
    'use strict';
    angular
.module('app')
.controller('ClgMasterAcademicYearController', ClgMasterAcademicYearController)

    ClgMasterAcademicYearController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$filter', 'superCache']
    function ClgMasterAcademicYearController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $filter, superCache) {


        $scope.searchProspectus = '';

        $scope.sortKey = 'asmaY_Id';
        $scope.sortReverse = true;
        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }

        $scope.ddate = {};
        $scope.getyear = [];
        $scope.ddate = new Date();

        $scope.usrname = localStorage.getItem('username');
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }
        $scope.currentPage = 1;
        $scope.itemsPerPage = paginationformasters;
        $scope.imgname = logopath;

        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));

        //  $scope.ASMAY_From_Date = new Date();
        $scope.asmaY_Id = "";
        $scope.sortColumn = false;
        $scope.selacdfryr = true;
        $scope.selacdtoyr = true;
        $scope.prestdt = true;
        $scope.preenddate = true;
        $scope.setfromdate = function (data) {
            
            if (data != null) {

                console.log(data);
                $scope.fromDate = data;
                $scope.frommon = "";
                $scope.fromDay = "";

                $scope.fromD = "";
                $scope.toD = "";

                $scope.fromM = "";
                $scope.toM = "";


                if ($scope.getyear.length > 0) {

                    $scope.fromD = new Date($scope.getyear[0].asmaY_From_Date);
                    $scope.toD = new Date($scope.getyear[0].asmaY_To_Date);

                    console.log($scope.getyear);
                    $scope.minDatef = new Date(
                          $scope.fromDate,
               $scope.fromD.getMonth(),
              $scope.fromD.getDate());


                    $scope.maxDatef = new Date(
                          $scope.fromDate,
               $scope.toD.getMonth(),
              $scope.toD.getDate() + 365);


                } else {
                    console.log($scope.getyear);
                    $scope.minDatef = new Date(
                          $scope.fromDate,
               $scope.fromD,
              $scope.fromM + 1);


                    $scope.maxDatef = new Date(
                          $scope.fromDate,
               $scope.toD,
              $scope.toM + 365);
                }



                ////for Preadmission From date
                //$scope.minDatefP = new Date(
                //     $scope.fromDate - 1,
                //      $scope.frommon,
                //       $scope.fromDay + 1);


                //$scope.maxDatefP = new Date(
                //     $scope.fromDate,
                //      $scope.frommon,
                //       $scope.fromDay + 365);


                $scope.selacdfryr = false;
            }
            else {
                $scope.selacdfryr = true;
            }

            // $scope.ASMAY_from_Year = "";
            $scope.ASMAY_From_Date = "";
            $scope.ASMAY_PreAdm_F_Date = "";
            $scope.ASMAY_Order = "";
            $scope.ASMAY_to_Year = "";
            $scope.ASMAY_To_Date = "";
            $scope.ASMAY_PreAdm_T_Date = "";
            $scope.ASMAY_Cut_Of_Date = "";
            $scope.ASMAY_ActiveFlag = "";
            $scope.ASMAY_Pre_ActiveFlag = "";

        }

        $scope.validatetodate = function (data1) {
            $scope.selacdtoyr = false;
            $scope.prestdt = false;
            $scope.toDate = data1;

            //$scope.fromDate = data1;
            //$scope.frommon = "";
            //$scope.fromDay = "";

            // For Academic End Date
            //$scope.minDatet = new Date(
            //      $scope.fromDate.getFullYear() + 1,
            //       $scope.frommon,
            //        $scope.fromDay + 1);

            //$scope.maxDatet = new Date(
            //      $scope.fromDate.getFullYear(),
            //       $scope.fromDate.getMonth(),
            //        $scope.fromDay + 365);

            $scope.minDatet = new Date(
           $scope.toDate.getFullYear(),
           $scope.toDate.getMonth() + 1,
          $scope.toDate.getDate());

            $scope.maxDatet = new Date(
          $scope.toDate.getFullYear() + 1,
          $scope.toDate.getMonth() - 1,
         $scope.toDate.getDate());

            $scope.maxDatefP = new Date(
            $scope.toDate.getFullYear(),
          $scope.toDate.getMonth(),
         $scope.toDate.getDate() - 1);
        }

        $scope.validatetodatepre = function (data1) {

            $scope.selacdtoyr = false;
            $scope.preenddate = false;
            $scope.toDate1 = data1;
            $scope.minDatetP = new Date(
           $scope.toDate1.getFullYear(),
           $scope.toDate1.getMonth(),
           $scope.toDate1.getDate() + 1);

            $scope.maxDatetP = new Date(
            $scope.ASMAY_From_Date.getFullYear(),
          $scope.ASMAY_From_Date.getMonth(),
         $scope.ASMAY_From_Date.getDate() - 1);

            //   $scope.maxDatetP = new Date(
            // $scope.toDate1.getFullYear() + 1,
            // $scope.toDate1.getMonth(),
            //$scope.toDate1.getDate());


            //pre cut off
            $scope.minDatetPC = new Date(
                 $scope.toDate1.getFullYear(),
           $scope.toDate1.getMonth(),
          $scope.toDate1.getDate());

            $scope.maxDatetPC = new Date(
           $scope.toDate1.getFullYear() + 1,
          $scope.toDate1.getMonth(),
         $scope.toDate1.getDate());
        }

        $scope.checkyr = function (data) {
            if (data != null) {
                $scope.selacdtoyr = false;
                $scope.yrerr = "";
                if ($scope.ASMAY_from_Year >= $scope.ASMAY_to_Year) {
                    $scope.yrerr = "To year must be greater than From year";
                    return false;
                }
                $scope.toDate = data;
                $scope.tomon = "";
                $scope.toDay = "";


                $scope.minDatet = new Date(
                      $scope.toDate,
                       $scope.tomon,
                        $scope.toDay + 1);

                $scope.maxDatet = new Date(
                  $scope.toDate,
                   $scope.tomon,
                    $scope.toDay + 365);

                //preadmission To date

                $scope.minDatetP = new Date(
                  $scope.toDate - 1,
                   $scope.tomon,
                    $scope.toDay + 1);

                $scope.maxDatetP = new Date(
                     $scope.toDate,
                      $scope.tomon,
                       $scope.toDay + 365);

                //Preadmission Cutof date

                $scope.minDatetPC = new Date(
                  $scope.toDate - 1,
                   $scope.tomon + 1,
                    $scope.toDay);

                $scope.maxDatetPC = new Date(
                     $scope.toDate,
                      $scope.tomon,
                       $scope.toDay + 365);
            }
            else {
                $scope.selacdtoyr = true;
            }
        }

        $scope.count1 = function () {
            //$scope.ASMAY_to_Year = $scope.ASMAY_from_Year + 1;
            var To_Year = parseInt($scope.ASMAY_from_Year)
            $scope.ASMAY_to_Year = To_Year + 1;
        }

        $scope.todatevalidation = function () {
            var year_from = parseInt($scope.ASMAY_from_Year)
            var year_to = parseInt($scope.ASMAY_to_Year)
            var yeardiff = year_to - year_from
            if (yeardiff != 1) {
                swal("Difference Should Be One Year");
                var To_Year = parseInt($scope.ASMAY_from_Year)
                $scope.ASMAY_to_Year = To_Year + 1;
            }
        }



        $scope.IsHidden = true;
        $scope.ShowHide = function () {
            //If DIV is hidden it will be visible and vice versa.
            $scope.IsHidden = $scope.IsHidden ? false : true;
        }

        $scope.IsHidden1 = true;
        $scope.ShowHide1 = function () {
            //If DIV is hidden it will be visible and vice versa.
            $scope.IsHidden1 = $scope.IsHidden1 ? false : true;
        }

        $scope.onlyWeekendsPredicate = function (date) {
            var day = date.getDay();
            return day === 0 || day === 6;
        };

        $scope.searchValue = '';
        $scope.filterValue = function (obj) {
            return ($filter('date')(obj.acmaY_AYFromDate, 'dd/MM/yyyy').indexOf($scope.searchValue) >= 0) || ($filter('date')(obj.acmaY_AYToDate, 'dd/MM/yyyy').indexOf($scope.searchValue) >= 0) || (angular.lowercase(obj.acmaY_AcademicYear)).indexOf(angular.lowercase($scope.searchValue)) >= 0 || (angular.lowercase(obj.acmaY_AcademicYearCode)).indexOf(angular.lowercase($scope.searchValue)) >= 0 || (JSON.stringify(obj.acmaY_AYOrder)).indexOf($scope.searchValue) >= 0;
        }

        $scope.academicDet = function () {
            var pageid = 2;
            apiService.getURI("ClgMasterAcademicYear/getalldetails", pageid).
        then(function (promise) {
            if (promise.getdetails.length == 0) {
                swal("No Records Found.....!!");
            }
            else {
                $scope.getdetails = promise.getdetails;
                $scope.presentCountgrid = $scope.getdetails.length;
                $scope.getyear = promise.getyear;
                $scope.newuser2 = promise.getallyear;
            }
            $scope.institutionList = promise.institutionList;
        })
        }


        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey == key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        }

        $scope.saveaccyear = function () {
            
            var dateValidation = $scope.checkErr($scope.ASMAY_From_Date, $scope.ASMAY_To_Date);
            var dateValidation1 = $scope.checkErr1($scope.ASMAY_PreAdm_F_Date, $scope.ASMAY_PreAdm_T_Date);
            var dateValidation2 = $scope.checkyr($scope.ASMAY_from_Year, $scope.ASMAY_to_Year);
            if (dateValidation == false || dateValidation1 == false || dateValidation2 == false) {
                $scope.submitted = false;
            }
                // $scope.submitted = true;
            else {
                if ($scope.myForm.$valid) {
                    if ($scope.acmaY_Id != "" && $scope.acmaY_Id != null) {
                        $scope.Id = $scope.acmaY_Id;
                    }
                    var ASMAY_Pre_ActiveFlag = $scope.ASMAY_Pre_ActiveFlag;

                    if ($scope.ASMAY_Pre_ActiveFlag) {
                        ASMAY_Pre_ActiveFlag = 1;

                    } else {
                        ASMAY_Pre_ActiveFlag = 0;
                    }
                    var data = {
                        "MI_Id": $scope.MI_Id,
                        "ACMAY_AcademicYear": $scope.ASMAY_from_Year + '-' + $scope.ASMAY_to_Year,
                        "ACMAY_AYFromDate": new Date($scope.ASMAY_From_Date).toDateString(),
                        "ACMAY_AYToDate": new Date($scope.ASMAY_To_Date).toDateString(),
                        "ACMAY_AcademicYearCode": $scope.ACMAY_AcademicYearCode,
                        "ACMAY_AYOrder": $scope.ASMAY_Order,
                        "ACMAB_PAActiveFlg": ASMAY_Pre_ActiveFlag,
                        "ACMAY_Id": $scope.Id
                    }

                    var config = {
                        headers: {
                            'Content-Type': 'application/json;'
                        }
                    }
                    apiService.create("ClgMasterAcademicYear/saveaccyear", data)

                    .then(function (promise) {
                        
                        if (promise.count == 0) {
                            swal("No Records Found")

                        }
                        if (promise.message == "Duplicate") {
                            swal("Record Already Exists")
                            $state.reload();
                        }
                        else {
                            if (promise.returnval == true) {
                                $scope.clearid();
                                if (promise.message == "Update") {
                                    swal('Record Updated Successfully');
                                    $state.reload();
                                }
                                else {
                                    swal('Record Saved Successfully');
                                    $state.reload();
                                }
                            }
                            else {
                                if (promise.message == "Update") {
                                    swal('Failed To Update Record');
                                    $state.reload();
                                }
                                else {
                                    swal('Failed To Save Record');
                                    $state.reload();
                                }
                            }
                        }
                    })
                } else {
                    //$scope.yrerr = "To year must be greater than From year";
                    $scope.submitted = true;
                }

            }
        };

        $scope.searchByColumn = function (searchProspectus, searchColumn) {
            if (searchProspectus != null || searchProspectus != undefined && searchColumn != null || searchColumn != undefined) {
                var data = {
                    "EnteredData": searchProspectus,
                    "SearchColumn": searchColumn,
                }
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }
                apiService.create("Academic/SearchByColumn", data)
                .then(function (promise) {
                    if (promise.count == 0) {
                        swal("No Records Found.....!!");
                    }
                    if (promise.message != "" && promise.message != null) {
                        swal(promise.message);
                        $scope.getdetails = promise.academicList;
                        $scope.presentCountgrid = $scope.getdetails.length;
                    }
                    else {
                        $scope.searchProspectus = "";
                        $scope.getdetails = promise.academicList;
                        $scope.presentCountgrid = $scope.getdetails.length;
                    }
                })
            }
            else {

            }
        }

        $scope.delete = function (employee, SweetAlert) {
            $scope.editEmployee = employee.asmaY_Id;
            var orgaid = $scope.editEmployee
            swal({
                title: "Are you sure?",
                text: "Do you want to delete record !!!!!!!!",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, delete it!",
                cancelButtonText: "Cancel!!!!!!",
                closeOnConfirm: false,
                closeOnCancel: false
            },
            function (isConfirm) {
                if (isConfirm) {
                    apiService.DeleteURI("Academic/deletedetails", orgaid).
                    then(function (promise) {
                        if (promise.count == 0) {
                            swal("No Records Found")
                        }
                        if (promise.message != "" && promise.message != null) {
                            swal(promise.message);
                        }
                        else {
                            if (promise.returnval == true) {
                                $scope.getdetails = promise.academicList;
                                $scope.presentCountgrid = $scope.getdetails.length;
                                swal('Record Deleted Successfully');
                            }
                            else {
                                swal('Failed To Delete Record');
                            }
                        }
                    });
                }
                else {
                    swal("Record Deletion Cancelled");
                }
                $state.reload();
            });
        }

        $scope.edit = function (employee) {

            apiService.create("ClgMasterAcademicYear/edit", employee).
            then(function (promise) {
                $scope.year = promise.editdetails[0].acmaY_AcademicYear.split('-');

                $scope.ASMAY_from_Year = $scope.year[0];
                $scope.ASMAY_to_Year = $scope.year[1];

                $scope.ASMAY_From_Date = new Date(promise.editdetails[0].acmaY_AYFromDate);
                $scope.ASMAY_To_Date = new Date(promise.editdetails[0].acmaY_AYToDate);

                $scope.ASMAY_Order = promise.editdetails[0].acmaY_AYOrder;
                $scope.ACMAY_AcademicYearCode = promise.editdetails[0].acmaY_AcademicYearCode;


                $scope.ACMAY_AcademicYear = new Date(promise.editdetails[0].acmaY_AcademicYear);

                if (promise.editdetails[0].acmaB_PAActiveFlg == 1) {
                    $scope.ASMAY_Pre_ActiveFlag = true;
                }
                else {
                    $scope.ASMAY_Pre_ActiveFlag = false;
                }
                $scope.acmaY_Id = promise.editdetails[0].acmaY_Id;

            })
        }

        $scope.clearid = function () {
            $state.reload();
        }
        // $scope.submitted = false;

        $scope.checkErr = function (FromDate, ToDate) {
            $scope.errMessage = '';
            var curDate = new Date();
            if (new Date(FromDate) > new Date(ToDate)) {
                $scope.errMessage = 'To Date should be greater than from date';
                return false;
            }
        };

        $scope.checkErr1 = function (ASMAY_PreAdm_F_Date, ASMAY_PreAdm_T_Date) {
            $scope.errMessage1 = '';
            var curDate = new Date();
            if (new Date(ASMAY_PreAdm_F_Date) > new Date(ASMAY_PreAdm_T_Date)) {
                $scope.errMessage1 = 'To Date should be greater than from date';
                return false;
            }
        };



        $scope.deactive = function (academicYear, SweetAlertt) {
            
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            var mgs = "";
            if (academicYear.is_Active == false) {
                mgs = "Activate";
            } else {
                mgs = "Deactivate";
            }
            swal({
                title: "Are you sure?",
                text: "Do you want to  " + mgs + " the Academic Year?",
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
                        apiService.create("ClgMasterAcademicYear/deactivate", academicYear).
                        then(function (promise) {                            
                            if (promise.message != null) {
                                swal(promise.message);
                            }
                            if (promise.is_Active== true) {
                                if (promise.returnval == true) {
                                    swal('Academic Year  Deactivated Successfully');
                                }
                            }
                            else if (promise.is_Active == false) {
                                swal('Academic Year Activated Successfully');
                            }
                            $state.reload();
                        })
                    } else {
                        swal("Cancelled");
                        $state.reload();
                    }

                });
        }

        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };

        $scope.getyearorder = function () {
            
            var pageid = 2;
            apiService.getURI("ClgMasterAcademicYear/getalldetails", pageid).
            then(function (promosie) {
                if (promosie != null) {
                    $scope.newuser2 = promosie.getdetails
                }
                else {
                    swal("No Records Found");
                }
            })
        }


        $scope.resetLists = function () {
            $scope.configA = {
                // Changed sorting within list
                onUpdate: function (evt) {
                    var itemEl = evt.item;  // dragged HTMLElement
                    // + indexes from onEnd
                }
            };
        }


        $scope.saveorder = function (newuser3) {
            var data = {
                "yearorder": $scope.newuser2,
            }
            apiService.create("Academic/saveorder/", data).then(
                function (promoise) {
                    if (promoise != null) {
                        if (promoise.returnval == true) {
                            swal("Records Updated Sucessfully");
                            $state.reload();
                        }
                        else {
                            swal("Failed to Update the Record");
                            $state.reload();
                        }
                    }
                    else {
                        swal("No Records Updated");
                        $state.reload();
                    }                   
                })
        }
    }
})();

