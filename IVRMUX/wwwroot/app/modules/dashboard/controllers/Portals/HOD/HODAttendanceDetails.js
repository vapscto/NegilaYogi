(function () {
    'use strict';
    angular
        .module('app')
        .controller('HODAttendanceDetailsController', HODAttendanceDetailsController)

    HODAttendanceDetailsController.$inject = ['$rootScope', '$scope', '$state', '$location', 'dashboardService', 'Flash', 'apiService', '$stateParams', '$filter', 'superCache', '$window']
    function HODAttendanceDetailsController($rootScope, $scope, $state, $location, dashboardService, Flash, apiService, $stateParams, $filter, superCache, $window) {


        $scope.searchValue = "";
        $scope.totalregstudent = 0;

        $scope.totalnewstudent = 0;
        $scope.sms = 0;
        $scope.email = 0;
        $scope.regular = [];

        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }
        $scope.masterlist = false;
        $scope.currentPage = 1;
        $scope.itemsPerPage = paginationformasters;
        $scope.searchValue = "";
        if ($scope.itemsPerPage == undefined) {
            $scope.itemsPerPage = 15
        }
        $scope.fields = function () {

            $scope.newadmissionstdtotal = [];
            $scope.datagraph = [];
            $scope.regularstdtotal = [];
            $scope.newadmstdgraphdta = [];


            $scope.Todaydate = new Date();
        }
        $scope.studentdrp = false;
        $scope.Binddata = function () {
            $scope.fields();


            apiService.getDATA("HODAttendanceDetails/Getdetails").
                then(function (promise) {


                    $scope.yearlt = promise.yearlist;

                    $scope.asmaY_Id = promise.yearlist[0].asmaY_Id;

                    $scope.OnAcdyear($scope.asmaY_Id);

                })

        }



        $scope.getDataByType = function (type) {
            if (type == 1) {
                $scope.studentdrp = false;
                $scope.allstudentlist = false;
                $scope.indattendance = false;
                $.$scope.amstid = 0;
            }
            if (type == 2) {
                $scope.studentdrp = true;
                $scope.allstudentlist = false;
                $scope.indattendance = false;

            }

        }



        $scope.OnAcdyear = function (asmaY_Id) {

            $scope.asmcL_Id = '';
            $scope.asmS_Id = '';
            if ($scope.type == 2) {
                $scope.fillstudents = [];
                $scope.amstid = '';
            }
            $scope.fields();
            $scope.section = [];

            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
            }

            apiService.create("HODAttendanceDetails/getclass", data).
                then(function (promise) {
                    $scope.classarray = promise.fillclass;
                })


        }

        $scope.interacted = function (field) {
            return $scope.submitted;
        };


        $scope.OnClass = function () {
            //alert($scope.type)
            //$scope.asmcL_Id = asmcL_Id;
            $scope.asmS_Id = '';
            if ($scope.type == 2) {
                $scope.fillstudents = [];
                $scope.amstid = '';
            }
            $scope.fields();
            var data = {
                "ASMCL_Id": $scope.asmcL_Id,
                "ASMAY_Id": $scope.asmaY_Id,
            }
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            apiService.create("HODAttendanceDetails/Getsection", data).
                then(function (promise) {


                    $scope.section = promise.fillsection;



                })


        }
        $scope.OnSection = function () {

            if ($scope.type == 2) {

                $scope.amstid = '';
            }


            $scope.fields();
            var data = {
                "ASMS_Id": $scope.asmS_Id,
                "ASMCL_Id": $scope.asmcL_Id,
                "ASMAY_Id": $scope.asmaY_Id,
            }
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            apiService.create("HODAttendanceDetails/GetAttendence", data).
                then(function (promise) {



                    // $scope.indattendance = true;
                    $scope.fillstudents = promise.fillstudents;





                })


        }

        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey == key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        }

        //$scope.sort = function (keyname) {
        //    $scope.sortKey = keyname;   //set the sortKey to the param passed
        //    $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        //}
        $scope.showreport = function () {

            $scope.allstudentlist = false;
            $scope.indattendance = false;

            //  var a = $scope.asmcL_Id;
            // alert(a)
            // $scope.fields();

            if ($scope.type == 1) {

                $scope.amstid = 0;
            }

            if ($scope.myForm.$valid) {
                var data = {
                    "asmcL_Id": $scope.asmcL_Id,
                    "ASMAY_Id": $scope.asmaY_Id,
                    "asmS_Id": $scope.asmS_Id,
                    "amstid": $scope.amstid,
                    "type": $scope.type,
                }
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }
                apiService.create("HODAttendanceDetails/GetIndividualAttendence", data).
                    then(function (promise) {


                        if ($scope.type == 2) {
                            if (promise.attendencelist != null) {
                                $scope.indattendance = true;
                                $scope.allstudentlist = false;
                                $scope.attendencelist = promise.attendencelist;
                            }
                            else {
                                swal("No Record Found")
                            }


                        }

                        if ($scope.type == 1) {
                            if (promise.allstudent != null) {
                                $scope.allstudentlist = true;
                                $scope.indattendance = false;
                                $scope.montharry = promise.fillmonths;
                                $scope.allstudent = promise.allstudent;



                                $scope.mail_month_list = [];
                                angular.forEach($scope.allstudent, function (st) {
                                    if ($scope.mail_month_list.length == 0) {
                                        $scope.mail_month_list.push({ monthid: st.monthid, monthname: st.monthname });
                                    }
                                    else if ($scope.mail_month_list.length > 0) {
                                        var ALREADY_CNT = 0;
                                        angular.forEach($scope.mail_month_list, function (a) {
                                            if (a.monthid == st.monthid) {
                                                ALREADY_CNT += 1;
                                            }
                                        })
                                        if (ALREADY_CNT == 0) {
                                            $scope.mail_month_list.push({ monthid: st.monthid, monthname: st.monthname });
                                        }
                                    }
                                })


                                $scope.insarray = [{ name: "CH", field: "classheld" }, { name: "P", field: "present" }, { name: "PER", field: "perc" }];


                                $scope.newarray = [];
                                angular.forEach($scope.mail_month_list, function (s) {

                                    angular.forEach($scope.insarray, function (s1) {
                                        $scope.newarray.push({ name: s1.name, monthid: s.monthid, name1: s.monthid + s1.name, field: s1.field });
                                    })

                                })
                                $scope.newarray1 = $scope.newarray;


                                var temp_stu_data = [];
                                angular.forEach($scope.allstudent, function (st1) {
                                    var stu_id = st1.amstid;
                                    var stu_month_list = [];

                                    //angular.forEach($scope.allstudentlist, function (st2) {
                                    //    if(st2.amstid==stu_id)
                                    //    {
                                    //        angular.forEach($scope.mail_month_list, function (a) {
                                    //            if(st2.monthid==a.monthid)
                                    //            {
                                    //                stu_month_list.push({ monthid: a.monthid, monthname: a.monthname, classheld: st2.classheld, present: st2.present, perc: st2.perc });
                                    //            }
                                    //        })
                                    //    }

                                    //})
                                    if (temp_stu_data.length == 0) {
                                        angular.forEach($scope.allstudent, function (st2) {
                                            if (st2.amstid == stu_id) {
                                                angular.forEach($scope.mail_month_list, function (a) {
                                                    if (st2.monthid == a.monthid) {
                                                        stu_month_list.push({ monthid: a.monthid, monthname: a.monthname, classheld: st2.classheld, present: st2.present, perc: st2.perc });
                                                    }
                                                })
                                            }

                                        })

                                        var total = {};
                                        var chtotal = 0;
                                        var prtotal = 0;
                                        var totalper = 0;

                                        angular.forEach(stu_month_list, function (t1) {
                                            chtotal = chtotal + t1.classheld;
                                            prtotal = prtotal + t1.present;
                                            totalper = (prtotal / chtotal) * 100;

                                        })

                                        total = { chtotal: chtotal, prtotal: prtotal, totalper: totalper };
                                        temp_stu_data.push({ amstid: st1.amstid, studentname: st1.studentname, stu_list: stu_month_list, total: total });
                                    }
                                    else if (temp_stu_data.length > 0) {
                                        var al_cnt = 0;
                                        angular.forEach(temp_stu_data, function (a1) {
                                            if (a1.amstid == st1.amstid) {
                                                al_cnt += 1;
                                            }
                                        })
                                        if (al_cnt == 0) {
                                            angular.forEach($scope.allstudent, function (st2) {
                                                if (st2.amstid == stu_id) {
                                                    angular.forEach($scope.mail_month_list, function (a) {
                                                        if (st2.monthid == a.monthid) {
                                                            stu_month_list.push({ monthid: a.monthid, monthname: a.monthname, classheld: st2.classheld, present: st2.present, perc: st2.perc });
                                                        }
                                                    })
                                                }

                                            })
                                            //pvn total
                                            var total = {};
                                            var chtotal = 0;
                                            var prtotal = 0;
                                            var totalper = 0;

                                            angular.forEach(stu_month_list, function (t1) {
                                                chtotal = chtotal + t1.classheld;
                                                prtotal = prtotal + t1.present;
                                                totalper = (prtotal / chtotal) * 100;
                                                // roundToTwo(totalper);

                                            })

                                            total = { chtotal: chtotal, prtotal: prtotal, totalper: totalper };
                                            temp_stu_data.push({ amstid: st1.amstid, studentname: st1.studentname, stu_list: stu_month_list, total: total });
                                        }
                                    }

                                })
                                $scope.data = temp_stu_data;

                                console.log(temp_stu_data);

                            }
                            else {
                                swal("No Record Found")
                            }


                        }




                    })


            }
            else {
                $scope.submitted = true;
            }

        }

        $scope.cancel = function () {
            $state.reload();
        }


    };
})();

