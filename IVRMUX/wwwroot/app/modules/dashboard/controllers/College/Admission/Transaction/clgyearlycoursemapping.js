
(function () {
    'use strict';
    angular
        .module('app')
        .controller('ClgyearlycoursemappingController', ClgyearlycoursemappingController)

    ClgyearlycoursemappingController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$filter', 'superCache']
    function ClgyearlycoursemappingController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $filter, superCache) {


        $scope.searchProspectus = '';
        $scope.user = {};
        $scope.sortReverse = true;
        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }
        else {
            paginationformasters = 10;
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

        $scope.semester_list = [];
        $scope.academicDet = function () {
            var pageid = 2;
            apiService.getURI("Clgyearlycoursemapping/getalldetails", pageid).
                then(function (promise) {
                    $scope.accyearlist = promise.accyearlist;
                    $scope.courselist = promise.courselist;
                    $scope.semesterlistget = promise.semesterlistget;
                    $scope.disable_flag = false;
                    $scope.getsaveddata = promise.getsaveddata;
                    if ($scope.getsaveddata !== null && $scope.getsaveddata.length > 0) {
                        $scope.detailslist = true;
                    } else {
                        $scope.detailslist = false;
                    }
                });
        };

        //Get Accyear start date and end date
        $scope.get_courses = function () {
            $scope.fromdate = "";
            $scope.todate = "";
            angular.forEach($scope.accyearlist, function (y) {
                if (y.asmaY_Id == $scope.ASMAY_Id) {
                    $scope.ACAYC_From_Date = new Date(y.asmaY_From_Date);
                    $scope.ACAYC_To_Date = new Date(y.asmaY_To_Date);
                    $scope.ACAYC_To_Date1 = new Date(y.asmaY_To_Date);
                    $scope.ACAYC_From_Date1 = new Date(y.asmaY_From_Date);
                }
            });
        };

        //Change for course from date
        $scope.checkErr = function (ACAYC_From_Date, ACAYC_To_Date) {
            if (new Date(ACAYC_To_Date) < new Date(ACAYC_From_Date)) {
                $scope.ACAYC_To_Date = $scope.ACAYC_From_Date;
            }
        };

        //Change for course end date checkErr1
        $scope.checkErr1 = function (ACAYC_From_Date, ACAYC_To_Date) {
            if (new Date(ACAYC_From_Date) > new Date(ACAYC_To_Date)) {
                $scope.ACAYC_To_Date = $scope.ACAYC_From_Date;
            }
        };


        //Get Branch and course start date and end date
        $scope.getbranches = function () {
            $scope.disable_flag = false;
            $scope.ACAYC_NoOfSEM = null;
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "AMCO_Id": $scope.AMCO_Id,
            };
            apiService.create("Clgyearlycoursemapping/getbranches", data).then(function (promise) {
                if (promise != null) {
                    $scope.branchlist = promise.branchlist;
                    $scope.getcourselist = promise.getcourselist;
                    if ($scope.getcourselist.length > 0) {
                        $scope.ACAYC_From_Date = new Date($scope.getcourselist[0].acayC_From_Date);
                        $scope.ACAYC_To_Date = new Date($scope.getcourselist[0].acayC_To_Date);
                        $scope.ACAYC_NoOfSEM = $scope.getcourselist[0].acayC_NoOfSEM;
                        $scope.disable_flag = true;
                    } else {

                        angular.forEach($scope.courselist, function (yy) {
                            if (yy.amcO_Id == $scope.AMCO_Id) {
                                $scope.ACAYC_NoOfSEM = yy.amcO_NoOfSemesters
                                $scope.disable_flag = true;
                            }
                        });
                    }

                } else {
                    //dd
                }
            })
        }


        $scope.getsemisters = function () {
            $scope.semesterlist1 = [];
            $scope.semester_list = [];
            $scope.ACAYCB_PreAdm_FDate = null;
            $scope.ACAYCB_PreAdm_TDate = null;
            $scope.ACAYB_ReferenceDate = null;
            if ($scope.AMB_Id != null && $scope.AMB_Id != "") {
                var data = {
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "AMCO_Id": $scope.AMCO_Id,
                    "AMB_Id": $scope.AMB_Id,
                    "noofsem": $scope.noofsem
                }
                apiService.create("Clgyearlycoursemapping/getsemisters", data).then(function (promise) {
                    if (promise != null) {
                        $scope.semesterlist1 = promise.semesterlist;
                        //  $scope.ACAYC_NoOfSEM = $scope.semesterlist1.length;
                        $scope.branchlistdate = promise.branchlistdate;
                        if ($scope.branchlistdate.length > 0) {
                            $scope.ACAYCB_PreAdm_FDate = new Date($scope.branchlistdate[0].acaycB_PreAdm_FDate);
                            $scope.ACAYCB_PreAdm_TDate = new Date($scope.branchlistdate[0].acaycB_PreAdm_TDate);
                            $scope.ACAYB_ReferenceDate = new Date($scope.branchlistdate[0].acayB_ReferenceDate);
                        }
                        //$scope.semester_list = [];
                        angular.forEach(promise.semesterlistget, function (sem) {
                            var obj = sem;
                            obj.ACAYCBS_SemOrder = sem.amsE_SEMOrder;
                            angular.forEach($scope.semesterlist1, function (sem1) {
                                if (sem1.amsE_Id == sem.amsE_Id) {
                                    obj.xyz = true;
                                    obj.acaycbS_SemStartDate = new Date(sem1.acaycbS_SemStartDate);
                                    obj.acaycbS_SemEndDate = new Date(sem1.acaycbS_SemEndDate);
                                    obj.ACAYCBS_SemOrder = sem1.acaycbS_SemOrder;
                                }
                            })
                            $scope.semester_list.push(obj);
                        })

                    }
                })
            }
        };



        $scope.toggleAll = function () {

            angular.forEach($scope.semester_list, function (stud) {
                stud.xyz = $scope.all;
                stud.acaycbS_SemStartDate = null;
                stud.acaycbS_SemEndDate = null;
            })
        };

        $scope.optionToggled = function (user) {
            $scope.all = $scope.semester_list.every(function (itm) { return itm.xyz; });
            var cnt = 0;
            angular.forEach($scope.semester_list, function (stud) {
                if (stud.xyz) cnt += 1;;
            })
            if (cnt > Number($scope.ACAYC_NoOfSEM)) {
                swal("Can't Select More Than No.Of Semesters");
                user.xyz = false;
            }
            user.acaycbS_SemStartDate = null;
            user.acaycbS_SemEndDate = null;
        };

        $scope.searchdata = function () {
            if ($scope.myForm.$valid) {
                var data = {
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "AMCO_Id": $scope.AMCO_Id,
                    "AMB_Id": $scope.AMB_Id,
                    "noofsem": $scope.noofsem
                };
                apiService.create("Clgyearlycoursemapping/searchdata", data).then(function (promise) {
                    if (promise !== null) {
                        $scope.semesterlist = promise.semesterlist;
                        angular.forEach($scope.semesterlist, function (y) {
                            y.acaycbS_SemEndDate = new Date(y.acaycbS_SemEndDate);
                            y.acaycbS_SemStartDate = new Date(y.acaycbS_SemStartDate);
                        });
                    } else {
                        //dd
                    }
                })
            } else {
                $scope.submitted = true;
            }
        };

        $scope.submitted1 = false;
        $scope.totalgridtest = [];

        $scope.addNew = function (semesterlist) {

            if ($scope.myForm1.$valid) {
                $scope.semesterlist.push({
                    'asmE_Id': semesterlist.asmE_Id,
                    'acaycbS_SemStartDate': semesterlist.acaycbS_SemStartDate,
                    'acaycbS_SemEndDate': semesterlist.acaycbS_SemEndDate,
                });
                $scope.totalgridtest.push({
                    'asmE_Id': semesterlist.asmE_Id,
                    'acaycbS_SemStartDate': semesterlist.acaycbS_SemStartDate,
                    'acaycbS_SemEndDate': semesterlist.acaycbS_SemEndDate,
                });
                if ($scope.totalgridtest.length > 0) {
                    $scope.remflg = true;
                } else {
                    $scope.remflg = false;
                }
                $scope.PD = {};
            }
            else {
                $scope.submitted1 = true;
            }
        };

        $scope.checkvalidation = function (ff, index) {
            $scope.selindex = index;
            angular.forEach($scope.semesterlist, function (dd) {
                $scope.aryindex = $scope.semesterlist.indexOf(dd);
                if (ff.amsE_Id == dd.amsE_Id && $scope.selindex != $scope.aryindex) {
                    swal("Already Selected");
                    ff.amsE_Id = "";

                }
            });
        };

        $scope.removerow = function (semesterlist) {

            $scope.semesterlist.pop({
                'asmE_Id': semesterlist.asmE_Id,
                'acaycbS_SemStartDate': semesterlist.acaycbS_SemStartDate,
                'acaycbS_SemEndDate': semesterlist.acaycbS_SemEndDate,

            });

            $scope.totalgridtest.pop({
                'asmE_Id': semesterlist.asmE_Id,
                'acaycbS_SemStartDate': semesterlist.acaycbS_SemStartDate,
                'acaycbS_SemEndDate': semesterlist.acaycbS_SemEndDate,
            });

            if ($scope.totalgridtest.length > 0) {
                $scope.remflg = true;
            } else {
                $scope.remflg = false;
            }
        };


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

        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey == key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        };

        $scope.savedata = function () {
            if ($scope.myForm.$valid) {
                var sem_list = [];
                angular.forEach($scope.semester_list, function (semi) {
                    if (semi.xyz) {
                        sem_list.push({ AMSE_Id: semi.amsE_Id, ACAYCBS_SemStartDate: new Date(semi.acaycbS_SemStartDate).toDateString(), ACAYCBS_SemEndDate: new Date(semi.acaycbS_SemEndDate).toDateString(), ACAYCBS_SemOrder: semi.ACAYCBS_SemOrder });
                    }
                });
                var data = {
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "AMCO_Id": $scope.AMCO_Id,
                    "ACAYC_From_Date": new Date($scope.ACAYC_From_Date).toDateString(),
                    "ACAYC_To_Date": new Date($scope.ACAYC_To_Date).toDateString(),
                    "ACAYC_NoOfSEM": $scope.ACAYC_NoOfSEM,
                    "AMB_Id": $scope.AMB_Id,
                    "ACAYCB_PreAdm_FDate": new Date($scope.ACAYCB_PreAdm_FDate).toDateString(),
                    "ACAYCB_PreAdm_TDate": new Date($scope.ACAYCB_PreAdm_TDate).toDateString(),
                    "ACAYB_ReferenceDate": new Date($scope.ACAYB_ReferenceDate).toDateString(),
                    temp_sem_branch_DTO: sem_list
                };

                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                };
                apiService.create("Clgyearlycoursemapping/savedata", data)
                    .then(function (promise) {

                        if (promise.returnval) {
                            if ($scope.semesterlist1.length > 0) {
                                swal("Record Updated Successfully");
                            } else {
                                swal("Record Saved Successfully");
                            }
                        }
                        else if (!promise.returnval) {
                            if ($scope.semesterlist1.length > 0) {
                                swal("Failed To Update Record");
                            } else {
                                swal("Failed To Save Record");
                            }
                        }
                        else {
                            swal("Something Went Wrong Please Contact Administrator");
                        }
                        $scope.clearid();
                    });
            } else {
                //$scope.yrerr = "To year must be greater than From year";
                $scope.submitted = true;
            }
        };       

        $scope.clearid = function () {
            $state.reload();
        };
        // $scope.submitted = false;        

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.interacted1 = function (field) {
            return $scope.submitted1;
        };  

        $scope.validate_from_date = function (usrfrmdate, user) {

            var alr_cnt = 0;
            angular.forEach($scope.semester_list, function (semi) {
                if (semi.xyz && user.amsE_Id != semi.amsE_Id) {
                    if ((new Date(usrfrmdate) >= new Date(semi.acaycbS_SemStartDate) && new Date(usrfrmdate) <= new Date(semi.acaycbS_SemEndDate)))                    
                    {
                        alr_cnt += 1;
                    }
                }
            });            
            user.acaycbS_SemEndDate = null;
        };

        $scope.validate_to_date = function (usrtodate, user) {

            if (user.acaycbS_SemEndDate !== null && user.acaycbS_SemEndDate !== undefined) {
                if (user.acaycbS_SemStartDate !== null && user.acaycbS_SemStartDate !== undefined) {
                    var alr_cnt = 0;
                    if (new Date(usrtodate) < new Date(user.acaycbS_SemStartDate)) {
                        // alr_cnt += 1;
                        swal("Don't Select Before Date of Sem Start Date!!!");
                        user.acaycbS_SemEndDate = null;
                    }
                    else {
                        angular.forEach($scope.semester_list, function (semi) {
                            if (semi.xyz && user.amsE_Id != semi.amsE_Id) {
                                if ((new Date(usrtodate) >= new Date(semi.acaycbS_SemStartDate) && new Date(usrtodate) <= new Date(semi.acaycbS_SemEndDate)))
                                //|| (new Date(usrfrmdate) < new Date(semi.acaycbS_SemStartDate) && new Date(usrfrmdate) > new Date(semi.acaycbS_SemEndDate))
                                {
                                    alr_cnt += 1;
                                }
                            }
                        });
                        //if (alr_cnt > 0) {
                        //    swal("Don't Select Other Sem Dates!!!");
                        //    user.acaycbS_SemEndDate = null;
                        //}
                    }
                }
                else {
                    swal("First Select From Date !!!");
                    user.acaycbS_SemEndDate = null;
                }
            }
        };    
        
       

        $scope.viewrecordspopup = function (user) {
            var data = user;
            apiService.create("Clgyearlycoursemapping/viewrecordspopup", data).then(function (promise) {
                if (promise !== null) {
                    $scope.getviewdetails = promise.getviewdetails;
                }

            });
        };


    }
})();

