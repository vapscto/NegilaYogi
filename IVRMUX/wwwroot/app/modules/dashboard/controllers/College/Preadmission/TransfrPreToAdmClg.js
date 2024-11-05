
(function () {
    'use strict';
    angular
        .module('app')
        .controller('TransfrPreToAdmClgController', TransfrPreToAdmClgController)

    TransfrPreToAdmClgController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q', '$cookieStore', '$stateParams', '$filter', 'FormSubmitter', '$window', 'superCache']
    function TransfrPreToAdmClgController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $q, $cookieStore, $stateParams, $filter, FormSubmitter, $window, superCache) {
        //Date:23-12-2016 for displaying privileges.

        var configsettings = JSON.parse(localStorage.getItem("configsettings"));
        $scope.obj = {};
        $scope.userPrivileges = "";
        var pageid = $stateParams.pageId;
        var privlgs = JSON.parse(localStorage.getItem("privileges"));
        for (var i = 0; i < privlgs.length; i++) {
            if (privlgs[i].pageId == pageid) {
                $scope.userPrivileges = privlgs[i];
            }
        }

        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }

        $scope.sortKey = "pasR_Id";   //set the sortKey to the param passed
        $scope.reverse = true; //if true make it false and vice versa

        $scope.presentCountgrid = 0;


        $scope.checkboxchcked = [];
        $scope.isOptionsRequired = function () {
            return !$scope.preAdmtoAdmStuList.some(function (options) {
                return options.isSelected;
            });
        }
        $scope.currentPage = 1;
        $scope.itemsPerPage = paginationformasters;
        //$scope.toggleAll = function () {
        //    var toggleStatus = $scope.obj.all2;
        //    angular.forEach($scope.preAdmtoAdmStuList, function (itm) { itm.isSelected = toggleStatus; });

        //}


        $scope.checkboxselected = [];
        $scope.toggleAll = function () {
            var toggleStatus = $scope.obj.all2;
            angular.forEach($scope.preAdmtoAdmStuList, function (itm) {
                itm.isSelected = toggleStatus;
                if ($scope.obj.all2 == true) {
                    if ($scope.checkboxselected.indexOf(itm) === -1) {
                        $scope.checkboxselected.push(itm);
                    }
                }
                else {
                    $scope.checkboxselected.splice(itm);
                }
            });
        }


        $scope.optionToggled = function (data) {

            $scope.obj.all2 = $scope.preAdmtoAdmStuList.every(function (itm) { return itm.isSelected; })
            if ($scope.checkboxselected.indexOf(data) === -1) {
                var arr1 = {
                    "pasR_Id": data.pasR_Id
                }
                $scope.checkboxselected.push(arr1);
            }
            else {
                $scope.checkboxselected.splice($scope.checkboxselected.indexOf(data), 1);
            }
        }
        $scope.loadData = function () {
       
            apiService.getDATA("TransfrPreToAdmClg/getloaddata").
                then(function (promise) {

                    if ($scope.ASMAY_Id === promise.yearlist[0].asmaY_Id) {
                    }
                    else {
                        $scope.academicdrp = promise.yearlist;
                        $scope.ASMAY_Id = promise.yearlist[0].asmaY_Id;
                    }

                    $scope.courselist = promise.courselist;
                })
            $scope.sort = function (keyname) {
                $scope.sortKey = keyname;   //set the sortKey to the param passed
                $scope.reverse = !$scope.reverse; //if true make it false and vice versa
            }
        }
        $scope.get_branchs = function () {
           
            var data = {
                "courselistarray": $scope.courselistarray
            };

            apiService.create("TransfrPreToAdmClg/get_branchs", data).
                then(function (promise) {
                    if (promise.branchlist !== null && promise.branchlist.length > 0) {
                        $scope.branchlist = promise.branchlist;
                    
                    }
                });
        };

        $scope.get_semesters = function () {
          
            var data = {
                "branchlistarray": $scope.branchlistarray,
                "courselistarray": $scope.courselistarray
            };

            apiService.create("TransfrPreToAdmClg/get_semester", data).
                then(function (promise) {
                    if (promise.semesterlist !== null && promise.semesterlist.length > 0) {
                        $scope.semesterlist = promise.semesterlist;

                    }
                });
        };

        $scope.cleardata = function () {
            $scope.firstgrid = false;
            $scope.obj = {};
            $scope.submitted = false;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
            $scope.courselist = [];
            $scope.branchlist = [];
            $scope.semesterlist = [];
            $state.reload();
        }
        $scope.clear = function () {
            $scope.courseselectedAll = [];
            $scope.courselist = [];
            $scope.branchlist = [];
            $scope.semesterlist = [];
        
        }
        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };
        $scope.interacted1 = function (field) {
            return $scope.submitted1;
        };
        $scope.submitted = false;
        $scope.submitted1 = false;
        //$scope.searchdata = function (obj) {
        //    $scope.obj.all2 = false;
        //    if ($scope.myForm.$valid) {
        //        $scope.firstgrid = false;


               

        //        var listOfStu = {
        //            "ASMAY_Id": $scope.obj.ASMAY_Id,
        //            "ASMCL_Id": $scope.obj.ASMCL_Id
        //        }

        //        var config = {
        //            headers: {
        //                'Content-Type': 'application/json;'
        //            }
        //        }

        //        apiService.create("TransfrPreToAdmClg/searchdata", listOfStu).
        //            then(function (promise) {
        //                $scope.firstgrid = true;



        //                $scope.albumNameArray1 = [];
        //                for (var i = 0; i < promise.preAdmtoAdmStuList.length; i++) {
        //                    if (promise.preAdmtoAdmStuList[i].amsT_FirstName != '') {
        //                        if (promise.preAdmtoAdmStuList[i].amsT_MiddleName != null && promise.preAdmtoAdmStuList[i].amsT_MiddleName != '' && promise.preAdmtoAdmStuList[i].amsT_MiddleName != "") {
        //                            if (promise.preAdmtoAdmStuList[i].amsT_LastName != null && promise.preAdmtoAdmStuList[i].amsT_LastName != '' && promise.preAdmtoAdmStuList[i].amsT_LastName != "") {

        //                                $scope.albumNameArray1.push({ name: promise.preAdmtoAdmStuList[i].amsT_FirstName + " " + promise.preAdmtoAdmStuList[i].amsT_MiddleName + " " + promise.preAdmtoAdmStuList[i].amsT_LastName, pasR_Id: promise.preAdmtoAdmStuList[i].pasR_Id });
        //                            }
        //                            else {
        //                                $scope.albumNameArray1.push({ name: promise.preAdmtoAdmStuList[i].amsT_FirstName + " " + promise.preAdmtoAdmStuList[i].amsT_MiddleName, pasR_Id: promise.preAdmtoAdmStuList[i].pasR_Id });
        //                            }
        //                        }
        //                        else {
        //                            if (promise.preAdmtoAdmStuList[i].amsT_LastName != null && promise.preAdmtoAdmStuList[i].amsT_LastName != '' && promise.preAdmtoAdmStuList[i].amsT_LastName != "") {
        //                                $scope.albumNameArray1.push({ name: promise.preAdmtoAdmStuList[i].amsT_FirstName + " " + promise.preAdmtoAdmStuList[i].amsT_LastName, pasR_Id: promise.preAdmtoAdmStuList[i].pasR_Id });
        //                            }
        //                            else {
        //                                $scope.albumNameArray1.push({ name: promise.preAdmtoAdmStuList[i].amsT_FirstName, pasR_Id: promise.preAdmtoAdmStuList[i].pasR_Id });
        //                            }
        //                        }

        //                        promise.preAdmtoAdmStuList[i].name = $scope.albumNameArray1[i].name;
        //                    }
        //                }


        //                $scope.preAdmtoAdmStuList = promise.preAdmtoAdmStuList;



        //                $scope.presentCountgrid = promise.preAdmtoAdmStuList.length;
        //                if (promise.preAdmtoAdmStuList.length > 0) {
        //                    // swal('Record searched Successfully', '');
        //                }
        //                else {
        //                    swal('No Records', '');
        //                    $scope.firstgrid = false;
        //                }

        //            })
        //    }
        //    else {
        //        $scope.submitted = true;
        //    }


        //}

        for (var i = 0; i < configsettings.length; i++) {
            if (configsettings.length > 0) {

                $scope.configurationsettings = configsettings[i];

            }
        }


        $scope.savedatatrans = [];
        $scope.exporttoadmissiondata = function (preAdmtoAdmStuList) {

            $scope.savedatatrans = [];
            angular.forEach($scope.preAdmtoAdmStuList, function (user) {
                if (user.isSelected) {
                    $scope.savedatatrans.push(user);
                }
            })
            if ($scope.checkboxselected.length > 0) {
                var listOfStu = {
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "ASMCL_Id": $scope.ASMCL_Id,
                    studentdetails: $scope.savedatatrans,
                    configurationsettings: $scope.configurationsettings
                };

                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                };

                swal({
                    title: "Are you sure?",
                    text: "Do you want to transfer to admission ..!?",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonColor: "#DD6B55", confirmButtonText: "Yes!",
                    cancelButtonText: "Cancel..!",
                    closeOnConfirm: false,
                    closeOnCancel: false
                },
                    function (isConfirm) {
                        if (isConfirm) {
                            apiService.create("TransfrPreToAdmClg/exporttoadmission", listOfStu).
                                then(function (promise) {
                                    if (promise.returnval != null && promise.returnval != "") {
                                        if (promise.returnval == "true") {
                                            swal('Record Saved/Updated Successfully', '');
                                            $state.reload();
                                        }
                                        else if (promise.returnval == "false") {
                                            swal('Record Not Saved/Updated', '');
                                        }
                                        else {
                                            swal(promise.returnMsg);
                                        }

                                    }
                                });
                        }
                        else {
                            swal("Record Transfer Cancelled");
                        }

                    });
            } else {

                swal('Kindly select atleast one student..!');
                return;
            }
        };


        //--course--//
        $scope.GetCourseAll = function (courseselectedAll) {
            $scope.courselistarray = [];
            var toggleStatus = $scope.courseselectedAll;
            if (toggleStatus == false) {
                $scope.branchlist = [];
                $scope.semesterlist = [];
                angular.forEach($scope.courselist, function (itm) {
                    itm.selected = toggleStatus;
                });
                $scope.branchAll = false;
                $scope.semesterAll = false;
            }
            else {
                angular.forEach($scope.courselist, function (itm) {
                    itm.selected = toggleStatus;
                });
                $scope.sectionlistarray = [];
                angular.forEach($scope.courselist, function (qq) {
                    if (qq.selected == true) {
                        $scope.courselistarray.push({ AMCO_Id: qq.amcO_Id })
                    }
                });
            }
           

          

            $scope.get_branchs();
        };
        //By course Single
        $scope.GetCourse = function (option) {
            $scope.courselistarray = [];
            $scope.courseselectedAll = $scope.courselist.every(function (itm) {
                return itm.selected;
            });
            angular.forEach($scope.courselist, function (qq) {
            if (qq.selected == true) {
                $scope.courselistarray.push({ AMCO_Id: qq.amcO_Id })
            }
            });
            $scope.get_branchs();
        };

         //By branch
        $scope.GetBranchAll = function (branchAll) {
           
            $scope.branchlistarray = [];
            var toggleStatus = $scope.branchAll;

            if (toggleStatus == false) {
                $scope.semesterlist = [];
                angular.forEach($scope.branchlist, function (itm) {
                    itm.selected = toggleStatus;
                });
                $scope.semesterAll = false;
            }
            else {
                angular.forEach($scope.branchlist, function (itm) {
                    itm.selected = toggleStatus;
                });

                angular.forEach($scope.branchlist, function (qq) {
                    if (qq.selected == true) {
                        $scope.branchlistarray.push({ AMB_Id: qq.amB_Id })
                    }
                });
                $scope.get_semesters();
            }
            

          

        };

        //By branch Single
        $scope.Getbranch = function (option) {
            $scope.branchlistarray = [];
            $scope.branchAll = $scope.branchlist.every(function (itm) {

                return itm.selected;
            });
            angular.forEach($scope.branchlist, function (qq) {
                if (qq.selected == true) {
                    $scope.branchlistarray.push({ AMB_Id: qq.amB_Id })
                }
            });

            $scope.get_semesters();
        };


        //By semester
        $scope.GetSemesterAll = function (semesterAll) {
            $scope.semesterlistarray = [];
            var toggleStatus = $scope.semesterAll;
            if (toggleStatus == false) {
               
                $scope.semesterlist = [];
            }
            else {
                angular.forEach($scope.semesterlist, function (itm) {
                    itm.selected = toggleStatus;

                });

                angular.forEach($scope.semesterlist, function (qq) {
                    if (qq.selected == true) {
                        $scope.semesterlistarray.push({ AMSE_Id: qq.amsE_Id })
                    }
                });
            }
            
        };

        //By semester single
        $scope.GetSemester = function (option) {
            $scope.semesterlistarray = [];
            $scope.semesterAll = $scope.semesterlist.every(function (itm) {

                return itm.selected;
            });

            angular.forEach($scope.semesterlist, function (qq) {
                if (qq.selected == true) {
                    $scope.semesterlistarray.push({ AMSE_Id: qq.amsE_Id })
                }
            });
        };



        $scope.searchdata = function (obj) {
            $scope.obj.all2 = false;
            if ($scope.myForm.$valid) {
                $scope.firstgrid = false;

                var data = {
                    "branchlistarray": $scope.branchlistarray,
                    "courselistarray": $scope.courselistarray,
                    "semesterlistarray": $scope.semesterlistarray,
                    "ASMAY_Id": $scope.obj.ASMAY_Id
                };

                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }

                apiService.create("TransfrPreToAdmClg/searchdata", data).
                    then(function (promise) {
                        $scope.firstgrid = true;



                        //$scope.albumNameArray1 = [];
                        //for (var i = 0; i < promise.preAdmtoAdmStuList.length; i++) {
                        //    if (promise.preAdmtoAdmStuList[i].amsT_FirstName != '') {
                        //        if (promise.preAdmtoAdmStuList[i].amsT_MiddleName != null && promise.preAdmtoAdmStuList[i].amsT_MiddleName != '' && promise.preAdmtoAdmStuList[i].amsT_MiddleName != "") {
                        //            if (promise.preAdmtoAdmStuList[i].amsT_LastName != null && promise.preAdmtoAdmStuList[i].amsT_LastName != '' && promise.preAdmtoAdmStuList[i].amsT_LastName != "") {

                        //                $scope.albumNameArray1.push({ name: promise.preAdmtoAdmStuList[i].amsT_FirstName + " " + promise.preAdmtoAdmStuList[i].amsT_MiddleName + " " + promise.preAdmtoAdmStuList[i].amsT_LastName, pasR_Id: promise.preAdmtoAdmStuList[i].pasR_Id });
                        //            }
                        //            else {
                        //                $scope.albumNameArray1.push({ name: promise.preAdmtoAdmStuList[i].amsT_FirstName + " " + promise.preAdmtoAdmStuList[i].amsT_MiddleName, pasR_Id: promise.preAdmtoAdmStuList[i].pasR_Id });
                        //            }
                        //        }
                        //        else {
                        //            if (promise.preAdmtoAdmStuList[i].amsT_LastName != null && promise.preAdmtoAdmStuList[i].amsT_LastName != '' && promise.preAdmtoAdmStuList[i].amsT_LastName != "") {
                        //                $scope.albumNameArray1.push({ name: promise.preAdmtoAdmStuList[i].amsT_FirstName + " " + promise.preAdmtoAdmStuList[i].amsT_LastName, pasR_Id: promise.preAdmtoAdmStuList[i].pasR_Id });
                        //            }
                        //            else {
                        //                $scope.albumNameArray1.push({ name: promise.preAdmtoAdmStuList[i].amsT_FirstName, pasR_Id: promise.preAdmtoAdmStuList[i].pasR_Id });
                        //            }
                        //        }

                        //        promise.preAdmtoAdmStuList[i].name = $scope.albumNameArray1[i].name;
                        //    }
                        //}


                        $scope.preAdmtoAdmStuList = promise.preAdmtoAdmStuList;



                        $scope.presentCountgrid = promise.preAdmtoAdmStuList.length;
                        if (promise.preAdmtoAdmStuList.length > 0) {
                            // swal('Record searched Successfully', '');
                        }
                        else {
                            swal('No Records', '');
                            $scope.firstgrid = false;
                        }

                    })
            }
            else {
                $scope.submitted = true;
            }


        }


        $scope.savedatatrans = [];
        $scope.exporttoadmissiondata = function (preAdmtoAdmStuList) {

            $scope.savedatatrans = [];
            angular.forEach($scope.preAdmtoAdmStuList, function (user) {
                if (user.isSelected) {
                    $scope.savedatatrans.push(user);
                }
            })
            if ($scope.checkboxselected.length > 0) {
                var listOfStu = {
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "branchlistarray": $scope.branchlistarray,
                    "courselistarray": $scope.courselistarray,
                    "semesterlistarray": $scope.semesterlistarray,
                    studentdetails: $scope.savedatatrans,
                    configurationsettings: $scope.configurationsettings
                };

                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                };

                swal({
                    title: "Are you sure?",
                    text: "Do you want to transfer to admission ..!?",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonColor: "#DD6B55", confirmButtonText: "Yes!",
                    cancelButtonText: "Cancel..!",
                    closeOnConfirm: false,
                    closeOnCancel: false
                },
                    function (isConfirm) {
                        if (isConfirm) {
                            apiService.create("TransfrPreToAdmClg/exporttoadmission", listOfStu).
                                then(function (promise) {
                                    if (promise.returnMsg != null && promise.returnMsg != "") {
                                        if (promise.returnMsg == "true") {
                                            swal('Record Saved/Updated Successfully', '');
                                            $state.reload();
                                        }
                                        else if (promise.returnMsg == "false") {
                                            swal('Record Not Saved/Updated', '');
                                        }
                                        else {
                                            swal(promise.returnMsg);
                                        }

                                    }
                                });
                        }
                        else {
                            swal("Record Transfer Cancelled");
                        }

                    });
            } else {

                swal('Kindly select atleast one student..!');
                return;
            }
        };
    }
})();
