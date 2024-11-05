
(function () {
    'use strict';
    angular
        .module('app')
        .controller('SportStudentHouseDivisionController', SportStudentHouseDivisionController)

    SportStudentHouseDivisionController.$inject = ['$rootScope', '$scope', '$state', '$location', 'apiService', 'Flash', '$http', '$q', '$stateParams', 'superCache', '$filter']
    function SportStudentHouseDivisionController($rootScope, $scope, $state, $location, apiService, Flash, $http, $q, $stateParams, superCache, $filter) {


        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;
        $scope.itemsPerPage3 = 10;
        $scope.currentPage3 = 1;
        $scope.search3 = "";

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

        //========================TO  GEt The Values iN Grid
        $scope.BindData = function () {
            //var pageid = 2;
            apiService.getURI("SportsStudentHouseMapping/getdetails", 2).then(function (promise) {

                $scope.yearlist = promise.yearlist;

                $scope.houseList = promise.houseList;

                $scope.alldata = promise.alldata;

            })
        };


        //========================================Get Class Data
        $scope.get_class = function () {
            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
            }
            apiService.create("SportsStudentHouseMapping/get_class", data).then(function (promise) {
                $scope.classList = promise.classList;
            })
        }


        //========================================Get Section Data
        $scope.get_section = function () {
            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
                "ASMCL_Id": $scope.asmcL_Id
            }
            apiService.create("SportsStudentHouseMapping/get_section", data).then(function (promise) {
                $scope.sectionDropdown = promise.sectionList;
            })
        }


        //========================================Get Student Data
        $scope.get_student = function () {
            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
                "ASMS_Id": $scope.asmS_Id,
                "ASMCL_Id": $scope.asmcL_Id
            }
            apiService.create("SportsStudentHouseMapping/get_student", data).then(function (promise) {
                $scope.studentList = promise.studentList;
            })
        }


        //=============================================Get Edit Data
        $scope.EditRecord = function (EditRecord) {
            $scope.studentlsitdata = [];
            var data = {
                "SPCCSH_Id": EditRecord.spccsH_Id,
                "ASMAY_Id": EditRecord.asmaY_Id,
                "AMST_Id": EditRecord.amsT_Id,
            }
            apiService.create("SportsStudentHouseMapping/EditRecord/", data).then(function (promise) {

                if (promise.editrecord.length > 0) {
                    $scope.spccsH_Id = promise.editrecord[0].spccsH_Id;
                    $scope.spccmH_Id = promise.editrecord[0].spccmH_Id;
                    $scope.amsT_Id = promise.editrecord[0].amsT_Id;
                    $scope.spccsH_Date = new Date(promise.editrecord[0].spccsH_Date);
                    // $scope.spccsH_Age = promise.editrecord[0].spccsH_Age;
                    //$scope.height = promise.editrecord[0].spccsH_Height;
                    //$scope.weight = promise.editrecord[0].spccsH_Weight;
                    //$scope.studentname = promise.editrecord[0].studentname;
                    // $scope.amsT_AdmNo = promise.editrecord[0].amsT_AdmNo;
                    //$scope.spccmhD_BMI = promise.editrecord[0].spccsH_BMI;
                    // $scope.spccmhD_BMI_Remark = promise.editrecord[0].spccsH_BMIRemarks;

                    //$scope.spccsH_AsOnDate = new Date(promise.editrecord[0].spccsH_AsOnDate);
                    $scope.spccsH_Remarks = promise.editrecord[0].spccsH_Remarks;
                    $scope.asmaY_Id = promise.editrecord[0].asmaY_Id;
                    if ($scope.asmaY_Id > 0) {
                        $scope.get_class();
                    }
                    $scope.asmcL_Id = promise.editrecord[0].asmcL_Id;
                    if ($scope.asmcL_Id > 0) {
                        $scope.get_section();
                        $scope.asmS_Id = promise.editrecord[0].asmS_Id;
                    }

                    debugger;
                    $scope.studentList = promise.studentList;
                    if ($scope.asmS_Id > 0) {
                        angular.forEach($scope.studentList, function (tt) {
                            if (tt.amsT_Id == promise.editrecord[0].amsT_Id) {
                                tt.stud = true;
                                $scope.studentlsitdata.push({ selected: true, height: $scope.height, amsT_Id: $scope.amsT_Id, weight: $scope.weight, spccmhD_BMI: $scope.spccmhD_BMI, spccmhD_BMI_Remark: $scope.spccmhD_BMI_Remark, studentname: $scope.studentname, amsT_AdmNo: $scope.amsT_AdmNo });
                            }
                        })
                    }
                }
                else {
                    swal('No Record Found!');
                }
            })
        };

        $scope.interacted = function (field) {

            return $scope.submitted;
        };



        //============================================== TO Save The Data
        $scope.submitted = false;
        $scope.savedata = function () {
            $scope.studlsitdata1 = [];
            debugger;
            if ($scope.myForm.$valid) {
                angular.forEach($scope.studentList, function (cls) {
                    if (cls.stud == true) {
                        $scope.studlsitdata1.push(cls);
                    }
                })
                var agedate = $scope.spccsH_Date == null ? "" : $filter('date')($scope.spccsH_Date, "yyyy-MM-dd");
                var data = {
                    "SPCCSH_Id": $scope.spccsH_Id,
                    "ASMCL_Id": $scope.asmcL_Id,
                    "ASMS_Id": $scope.asmS_Id,
                    "ASMAY_Id": $scope.asmaY_Id,
                    "SPCCMH_Id": $scope.spccmH_Id,
                    "SPCCSH_Remarks": $scope.spccsH_Remarks,
                    "SPCCSH_Date": agedate,
                    studList1: $scope.studlsitdata1,

                }
                apiService.create("SportsStudentHouseMapping/saveRecord", data).
                    then(function (promise) {
                        debugger;
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


        //==============================================Deactivate
        $scope.deactivate = function (newuser1, SweetAlertt) {
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }

            var mgs = "";
            if (newuser1.spccmH_ActiveFlag == false) {

                mgs = "Activate";

            } else {
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
                        apiService.create("SportsStudentHouseMapping/deactivate", newuser1).
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



        //===========================cancel
        $scope.cancel = function () {
            $state.reload();
        }

        $scope.searchValue = "";
        $scope.searchchkbx = "";
        $scope.filterchkbx = function (obj) {
            return (angular.lowercase(obj.studentname)).indexOf(angular.lowercase($scope.searchchkbx)) >= 0;
        }

        $scope.togchkbx = function () {
            $scope.usercheck = $scope.studentList.every(function (options) {
                return options.stud;
            });
        }

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

        //========================BMI Calculation
        $scope.getBMI = function (item) {
            debugger;

            var height_in_mts = item.height / 100;
            var metersqr = height_in_mts * height_in_mts;
            item.spccmhD_BMI = item.weight / metersqr;

            var z = item.spccmhD_BMI;
            if (z <= 18.5) {
                item.spccmhD_BMI_Remark = "Underweight";
            }
            else if ((z >= 18.5) && (z <= 24.9)) {
                item.spccmhD_BMI_Remark = "Normal";
            }
            else if ((z >= 25) && (z <= 29.9)) {
                item.spccmhD_BMI_Remark = "Overweight";
            }
            else if ((z >= 30.0) && (z <= 39.5)) {
                item.spccmhD_BMI_Remark = "Obese";
            }
            else if (z > 39.5) {
                item.spccmhD_BMI_Remark = "Extreme Obese";
            }

        }

        //========================================================Student Information
        $scope.studentlsitdata = [];
        $scope.get_student_info = function () {
            $scope.studentlsitdata = [];
            debugger;
            angular.forEach($scope.studentList, function (cls) {
                if (cls.stud == true) {
                    $scope.studentlsitdata.push(cls);
                }
            });
        }
        //================================================================================End
        $scope.userselect = "";
        $scope.get_studlistt = function () {

            debugger;
            $scope.userselect = $scope.studentlsitdata.every(function (options) {
                return options.selected;
            });
        }

        $scope.check_allbox = function () {
            debugger;
            var toggleStatus1 = $scope.userselect;
            angular.forEach($scope.studentlsitdata, function (itm) {
                itm.selected = toggleStatus1;
            });
        }








    }
})();





//(function () {
//    'use strict';
//    angular
//        .module('app')
//        .controller('SportStudentHouseDivisionController', SportStudentHouseDivisionController)

//    SportStudentHouseDivisionController.$inject = ['$rootScope', '$scope', '$state', '$location', 'apiService', 'Flash', '$http', '$q', '$stateParams', 'superCache', '$filter']
//    function SportStudentHouseDivisionController($rootScope, $scope, $state, $location, apiService, Flash, $http, $q, $stateParams, superCache, $filter) {


//        $scope.currentPage = 1;
//        $scope.itemsPerPage = 10;


//        $scope.sortReverse = true;

//        //========================TO  GEt The Values iN Grid
//        $scope.BindData = function () {
//            //var pageid = 2;
//            apiService.getURI("SportsStudentHouseMapping/getdetails", 2).then(function (promise) {

//                $scope.yearlist = promise.yearlist;

//                $scope.houseList = promise.houseList;

//                $scope.alldata = promise.alldata;

//            })
//        };


//        //========================================Get Class Data
//        $scope.get_class = function () {
//            var data = {
//                "ASMAY_Id": $scope.asmaY_Id,
//            }
//            apiService.create("SportsStudentHouseMapping/get_class", data).then(function (promise) {
//                    $scope.classList = promise.classList;
//                })
//        }


//        //========================================Get Section Data
//        $scope.get_section = function () {
//            var data = {
//                "ASMAY_Id": $scope.asmaY_Id,
//                "ASMCL_Id": $scope.asmcL_Id
//            }
//            apiService.create("SportsStudentHouseMapping/get_section", data).then(function (promise) {
//                    $scope.sectionDropdown = promise.sectionList;
//                })
//        }


//        //========================================Get Student Data
//        $scope.get_student = function () {
//            var data = {
//                "ASMAY_Id": $scope.asmaY_Id,
//                "ASMS_Id": $scope.asmS_Id,
//                "ASMCL_Id": $scope.asmcL_Id
//            }
//            apiService.create("SportsStudentHouseMapping/get_student", data).then(function (promise) {
//                    $scope.studentList = promise.studentList;
//                })
//        }


//        //=============================================Get Edit Data
//        $scope.EditRecord = function (EditRecord) {
//            $scope.studentlsitdata = [];
//            var data = {
//                "SPCCSH_Id": EditRecord.spccsH_Id,
//                "ASMAY_Id": EditRecord.asmaY_Id,
//                "AMST_Id": EditRecord.amsT_Id,
//            }
//            apiService.create("SportsStudentHouseMapping/EditRecord/", data).then(function (promise) {

//                if (promise.editrecord.length > 0) {
//                    $scope.spccsH_Id = promise.editrecord[0].spccsH_Id;                    
//                    $scope.spccmH_Id = promise.editrecord[0].spccmH_Id;
//                    $scope.amsT_Id = promise.editrecord[0].amsT_Id;
//                    $scope.spccsH_Age = promise.editrecord[0].spccsH_Age;
//                    $scope.height = promise.editrecord[0].spccsH_Height;
//                    $scope.weight = promise.editrecord[0].spccsH_Weight;
//                    $scope.studentname = promise.editrecord[0].studentname;
//                    $scope.amsT_AdmNo = promise.editrecord[0].amsT_AdmNo;
//                    $scope.spccmhD_BMI = promise.editrecord[0].spccsH_BMI;
//                    $scope.spccmhD_BMI_Remark = promise.editrecord[0].spccsH_BMIRemarks;

//                    $scope.spccsH_AsOnDate = new Date(promise.editrecord[0].spccsH_AsOnDate);
//                    $scope.asmaY_Id = promise.editrecord[0].asmaY_Id;
//                    if ($scope.asmaY_Id > 0) {
//                        $scope.get_class();
//                    }
//                    $scope.asmcL_Id = promise.editrecord[0].asmcL_Id;
//                    if ($scope.asmcL_Id > 0) {
//                        $scope.get_section();
//                        $scope.asmS_Id = promise.editrecord[0].asmS_Id;
//                    }
//                    //$scope.get_student();
//                    debugger;
//                    $scope.studentList = promise.studentList;
//                    if ($scope.asmS_Id > 0) {                       
//                        angular.forEach($scope.studentList, function (tt) {                           
//                            if (tt.amsT_Id == promise.editrecord[0].amsT_Id) {
//                                tt.stud = true;
//                                $scope.studentlsitdata.push({ selected:true, height: $scope.height, amsT_Id: $scope.amsT_Id, weight: $scope.weight, spccmhD_BMI: $scope.spccmhD_BMI, spccmhD_BMI_Remark: $scope.spccmhD_BMI_Remark, studentname: $scope.studentname, amsT_AdmNo: $scope.amsT_AdmNo });
//                            }
//                        })
//                    }

//                }
//                else {
//                    swal('No Record Found!');
//                }

//            })
//        };

//        $scope.interacted = function (field) {

//            return $scope.submitted;
//        };

//        $scope.sort = function (key) {
//            $scope.sortReverse = ($scope.sortKey == key) ? !$scope.sortReverse : $scope.sortReverse;
//            $scope.sortKey = key;
//        }

//        //============================================== TO Save The Data
//        $scope.submitted = false;
//        $scope.savedata = function () {            
//            $scope.studlsitdata1 = [];
//            debugger;
//            if ($scope.studentlsitdata.length > 0) {
//                angular.forEach($scope.studentlsitdata, function (cls) {
//                    if (cls.selected == true) {
//                        $scope.studlsitdata1.push(cls);
//                    }
//                });
//                if ($scope.studlsitdata1.length > 0) {

//                    if ($scope.myForm.$valid) {
//                        var entrydate = $scope.spccsH_AsOnDate == null ? "" : $filter('date')($scope.spccsH_AsOnDate, "yyyy-MM-dd");



//                        //if ($scope.spccsH_Age == "")
//                        //    $scope.spccsH_Age = 0;
//                        //if ($scope.height == "")
//                        //    $scope.height = 0;
//                        //if ($scope.weight == "")
//                        //    $scope.weight = 0;
//                        //if ($scope.spccmhD_BMI == "") {
//                        //    $scope.spccmhD_BMI = 0;
//                        //    }

//                        var data = {
//                            "SPCCSH_Id": $scope.spccsH_Id,
//                            "ASMCL_Id": $scope.asmcL_Id,
//                            "ASMS_Id": $scope.asmS_Id,
//                            "ASMAY_Id": $scope.asmaY_Id,
//                            "SPCCSH_AsOnDate": entrydate,
//                            //"SPCCSH_Height": $scope.height,
//                            //"SPCCSH_Weight": $scope.weight,
//                            //"SPCCSH_BMI": $scope.spccmhD_BMI,
//                            //"SPCCSH_BMIRemarks": $scope.spccmhD_BMI_Remark,
//                            "SPCCMH_Id": $scope.spccmH_Id,

//                            "studList1": $scope.studlsitdata1,

//                        }

//                        apiService.create("SportsStudentHouseMapping/saveRecord", data).
//                            then(function (promise) {
//                                debugger;
//                                if (promise.msg == 'saved') {
//                                    swal("Saved Record" + promise.count1 + " Duplicate Record" + promise.count);
//                                    $state.reload();
//                                }
//                                else if (promise.msg == 'updated') {
//                                    swal("Updated Record" + promise.count1 + "Duplicate Record" + promise.count);
//                                    $state.reload();
//                                }
//                                else if (promise.msg == 'duplicate') {
//                                    swal("Record already exist");
//                                }
//                                else if (promise.msg == "savingFailed") {
//                                    swal("Failed to save record");
//                                }
//                                else if (promise.msg == "updateFailed") {
//                                    swal("Failed to update record");
//                                }
//                                else {
//                                    swal("Sorry...something went wrong");
//                                }

//                            })
//                    }
//                    else {
//                        $scope.submitted = true;
//                    }
//                }
//                else {
//                    swal("Please Select Student Records For Saving!");
//                }
//            }

//            else {
//                swal("Please Select Student Records For Saving!");
//            }
//        };


//        //==============================================Deactivate
//        $scope.deactivate = function (newuser1, SweetAlertt) {
//            var config = {
//                headers: {
//                    'Content-Type': 'application/json;'
//                }
//            }

//            var mgs = "";
//            if (newuser1.spccmH_ActiveFlag == false) {

//                mgs = "Activate";

//            } else {
//                mgs = "Deactivate";
//            }
//            swal({
//                title: "Are you sure?",
//                text: "Do you want to  " + mgs + " Record?",
//                type: "warning",
//                showCancelButton: true,
//                confirmButtonColor: "#DD6B55",
//                confirmButtonText: "Yes, " + mgs + " it!",
//                cancelButtonText: "Cancel..!",
//                closeOnConfirm: false,
//                closeOnCancel: false
//            },
//                function (isConfirm) {
//                    if (isConfirm) {
//                        apiService.create("SportsStudentHouseMapping/deactivate", newuser1).
//                            then(function (promise) {

//                                if (promise.returnVal == true) {
//                                    if (promise.msg != null) {
//                                        swal(promise.msg);
//                                        $state.reload();
//                                    }
//                                }
//                                else {
//                                    swal('Failed to Activate/Deactivate the Record');
//                                }
//                            })
//                    } else {
//                        swal("Cancelled");
//                    }
//                })
//        }



//        //===========================cancel
//        $scope.cancel = function () {
//            //$scope.SPCCSHD_Id = 0;
//            //$scope.SPCCMD_Id = "";
//            //$scope.Height = "";
//            //$scope.Weight = "";
//            //$scope.Age = "";
//            //$scope.SPCCMH_Id = "";
//            //$scope.ASMS_Id = "";
//            //$scope.ASMCL_Id = "";
//            //$scope.AMST_Id = "";
//            //$scope.ASMAY_Id = "";
//            //$scope.studentList = "";
//            //$scope.submitted = false;
//            //$scope.myForm.$setPristine();
//            //$scope.myForm.$setUntouched();
//            //$scope.SPCCMHD_BMI = "";
//            //$scope.SPCCMHD_BMI_Remark = "";
//            $state.reload();
//        }

//        $scope.searchValue = "";
//        //$scope.filterValue = function (obj) {
//        //    return (angular.lowercase(obj.spccmhD_DesignationDescription)).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
//        //        (angular.lowercase(obj.spccmhD_DesignationName)).indexOf(angular.lowercase($scope.searchValue)) >= 0
//        //}

//        $scope.searchchkbx = "";
//        $scope.filterchkbx = function (obj) {
//            return (angular.lowercase(obj.studentname)).indexOf(angular.lowercase($scope.searchchkbx)) >= 0;
//        }

//        $scope.togchkbx = function () {
//            $scope.usercheck = $scope.studentList.every(function (options) {
//                return options.stud;
//            });
//        }

//        $scope.all_check = function () {
//            var toggleStatus = $scope.usercheck;
//            angular.forEach($scope.studentList, function (itm) {
//                itm.stud = toggleStatus;
//            });
//        }

//        $scope.isOptionsRequired = function () {
//            return !$scope.studentList.some(function (options) {
//                return options.stud;
//            });
//        }

//        //========================BMI Calculation
//        $scope.getBMI = function (item) {
//            debugger;

//            var height_in_mts = item.height / 100;
//            var metersqr = height_in_mts * height_in_mts;
//            item.spccmhD_BMI = item.weight / metersqr;

//            var z = item.spccmhD_BMI;
//            if (z <= 18.5) {
//                item.spccmhD_BMI_Remark = "Underweight";
//            }
//            else if ((z >= 18.5) && (z <= 24.9)) {
//                item.spccmhD_BMI_Remark = "Normal";
//            }
//            else if ((z >= 25) && (z <= 29.9)) {
//                item.spccmhD_BMI_Remark = "Overweight";
//            }
//            else if ((z >= 30.0) && (z <= 39.5)) {
//                item.spccmhD_BMI_Remark = "Obese";
//            }
//            else if (z > 39.5) {
//                item.spccmhD_BMI_Remark = "Extreme Obese";
//            }

//        }

//        //========================================================Student Information
//        $scope.studentlsitdata = [];
//        $scope.get_student_info = function () {
//            $scope.studentlsitdata = [];
//            debugger;
//            angular.forEach($scope.studentList, function (cls) {
//                if (cls.stud == true) {
//                    $scope.studentlsitdata.push(cls);
//                }
//            });
//            //var data = {               
//            //    "ASMAY_Id": $scope.asmaY_Id,
//            //    "ASMS_Id": $scope.asmS_Id,
//            //    "ASMCL_Id": $scope.asmcL_Id,
//            //    "StudList": $scope.studentlsitdata,

//            //}
//            //apiService.create("SportsStudentHouseMapping/deactivate", data).
//            //    then(function (promise) {


//            //    });
//        }
//        //================================================================================End
//        $scope.userselect = "";
//        $scope.get_studlistt = function () {

//            debugger;
//            $scope.userselect = $scope.studentlsitdata.every(function (options) {
//                return options.selected;
//            });
//        }

//        $scope.check_allbox = function () {
//            debugger;
//            var toggleStatus1 = $scope.userselect;
//            angular.forEach($scope.studentlsitdata, function (itm) {
//                itm.selected = toggleStatus1;
//            });
//        }



//    }
//})();



