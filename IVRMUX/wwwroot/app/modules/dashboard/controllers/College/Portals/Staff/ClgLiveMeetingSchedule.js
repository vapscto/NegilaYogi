(function () {
    'use strict';
    angular
.module('app')
.controller('ClgLiveMeetingScheduleController', ClgLiveMeetingScheduleController)

    ClgLiveMeetingScheduleController.$inject = ['$rootScope', '$scope', '$state', '$location', 'dashboardService', 'Flash', 'apiService', '$stateParams', '$filter', 'superCache']
    function ClgLiveMeetingScheduleController($rootScope, $scope, $state, $location, dashboardService, Flash, apiService, $stateParams, $filter, superCache) {
        debugger;

        $scope.LoadData = function () {
            
            apiService.getDATA("ClgLiveMeetingSchedule/getloaddata")
                .then(function (promise) {
                    $scope.yearlt = promise.academicList;
                    $scope.subjlist = promise.subjlist;
                    $scope.meetinglist = promise.meetinglist;
                    $scope.stafflist = promise.stafflist;
                })
        };
        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;
        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey == key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        }
        $scope.searchValue = '';

        $scope.GetReport = function () {
            debugger;
            $scope.selectedcourse_list = [];
            $scope.selectedSectionlist = [];
            $scope.selectedsublist = [];
            $scope.selectedstflist = [];
            if ($scope.stf == true || $scope.stud == true)
            {
            if ($scope.myForm.$valid) {
                $scope.selectedsem = [];
                $scope.selectedcourse_list = [];
                $scope.selectedbranch = [];
             

                angular.forEach($scope.branchlist, function (cls) {
                    if (cls.select == true) {
                        $scope.selectedbranch.push({ AMB_Id: cls.amB_Id, AMB_BranchCode: cls.amB_BranchCode });
                    }
                });
                angular.forEach($scope.semisterlist, function (cls) {
                    if (cls.select == true) {
                        $scope.selectedsem.push({ AMSE_Id: cls.amsE_Id });
                    }
                });


                angular.forEach($scope.course_list, function (cls) {
                    if (cls.select == true) {
                        $scope.selectedcourse_list.push({ AMCO_Id: cls.amcO_Id, AMCO_CourseCode: cls.amcO_CourseCode });
                    }
                });

                angular.forEach($scope.sectionList, function (sect) {
                    if (sect.select == true) {
                        $scope.selectedSectionlist.push({ acmS_Id: sect.acmS_Id, ACMS_SectionCode: sect.acmS_SectionCode });
                    }
                });
                angular.forEach($scope.subjlist, function (sect) {
                    if (sect.select == true) {
                        $scope.selectedsublist.push({ ismS_Id: sect.ismS_Id, ISMS_SubjectCode: sect.ismS_SubjectCode });
                    }
                });
                angular.forEach($scope.stafflist, function (sect) {
                    if (sect.select == true) {
                        $scope.selectedstflist.push({ UserId: sect.userId, HRME_Id: sect.hrmE_Id });
                    }
                });




                var fromdate1 = $scope.LMSLMEET_PlannedDate == null ? "" : $filter('date')($scope.LMSLMEET_PlannedDate, "yyyy-MM-dd");
                var data = {
                    "LMSLMEET_Id": $scope.LMSLMEET_Id,
                    "stafflag": $scope.stf,
                    "studflag": $scope.stud,
                    "ASMAY_Id": $scope.asmaY_Id,
                    "LMSLMEET_MeetingTopic": $scope.LMSLMEET_MeetingTopic,
                    "LMSLMEET_PlannedDate": fromdate1,
                    "LMSLMEET_PlannedStartTime": $filter('date')($scope.FOMST_IHalfLoginTime, "HH:mm"),
                    "LMSLMEET_PlannedEndTime": $filter('date')($scope.FOMST_IIHalfLogoutTime, "HH:mm"),
                    selectedcourse_list: $scope.selectedcourse_list,
                    secids: $scope.selectedSectionlist,
                    subids: $scope.selectedsublist,
                    stfids: $scope.selectedstflist,
                    selectedbranch: $scope.selectedbranch,
                    selectedsem: $scope.selectedsem,
                    
                }
               
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }
                apiService.create("ClgLiveMeetingSchedule/savedata", data).
                    then(function (promise) {
                        if (promise.returnval === true) {

                            swal('Meeting Scheduled Successfully');
                        }
                        else if (promise.returnvalue === "Duplicate") {

                            $scope.gridOptions1.data = promise.filldata;

                            swal('Record already exist.');
                        }
                        else {
                            swal('Record Not Saved/Updated Successfully');
                        }

                        $state.reload();;
                    })
        }
            else
            {
                $scope.submitted = true;
            }

        }
            else
        {
            swal('Select Student OR Staff Details');
        }
    };


        $scope.edit = function (data) {

            apiService.create("ClgLiveMeetingSchedule/editdata", data).then(function (promise) {
                $scope.editlist = promise.editlist;
                $scope.course_list = promise.course_list;
                $scope.branchlist = promise.branch_list;
                $scope.semisterlist = promise.semisterlist;
                $scope.sectionList = promise.sectionList;
                $scope.LMSLMEET_Id = promise.editlist[0].lmslmeeT_Id;
                $scope.stf = promise.stafflag;
                $scope.stud = promise.studflag;
         
                $scope.LMSLMEET_MeetingTopic = promise.editlist[0].lmslmeeT_MeetingTopic;
                $scope.LMSLMEET_PlannedDate = new Date(promise.editlist[0].lmslmeeT_PlannedDate);
                $scope.FOMST_IHalfLoginTime = moment(promise.editlist[0].lmslmeeT_PlannedStartTime, 'HH:mm').format();
                $scope.FOMST_IIHalfLogoutTime = moment(promise.editlist[0].lmslmeeT_PlannedEndTime, 'HH:mm').format();
              

                //if (promise.emp_punchDetails[0].asmaY_Id > 0) {
                //    $scope.OnAcdyear(promise.emp_punchDetails[0].asmaY_Id);
                //}

                if ($scope.stud == true) {
                    $scope.asmaY_Id = promise.emp_punchDetails[0].asmaY_Id;

                    angular.forEach($scope.subjlist, function (ee) {
                        angular.forEach(promise.emp_punchDetails, function (xx) {
                            if (xx.ismS_Id == ee.ismS_Id) {
                                ee.select = true;
                            }

                        })
                    })
                    angular.forEach($scope.branchlist, function (ee) {
                        angular.forEach(promise.emp_punchDetails, function (xx) {
                            if (xx.amB_Id == ee.amB_Id) {
                                ee.select = true;
                            }

                        })
                    })

                    angular.forEach($scope.semisterlist, function (ee) {
                        angular.forEach(promise.emp_punchDetails, function (xx) {
                            if (xx.amsE_Id == ee.amsE_Id) {
                                ee.select = true;
                            }

                        })
                    })


                    if ($scope.course_list != undefined) {

                        angular.forEach($scope.course_list, function (ee) {
                            angular.forEach(promise.emp_punchDetails, function (xx) {
                                if (xx.amcO_Id == ee.amcO_Id) {
                                    ee.select = true;
                                }

                            })
                        })
                    }

                   

                    angular.forEach($scope.sectionList, function (ee) {
                        angular.forEach(promise.emp_punchDetails, function (xx) {
                            if (xx.acmS_Id == ee.acmS_Id) {
                                ee.select = true;
                            }

                        })
                    })
                }

                if ($scope.stf == true) {
                    angular.forEach($scope.stafflist, function (ee) {
                        angular.forEach(promise.empdetails, function (xx) {
                            if (xx.hrmE_Id == ee.hrmE_Id) {
                                ee.select = true;
                            }

                        })
                    })
                }

               



            });
        }



        $scope.isOptionsRequiredsub = function () {
            if ($scope.stud == true) {
            return !$scope.subjlist.some(function (options) {
                return options.select;
                });
              
        }
        }
        $scope.isOptionsRequiredcls = function () {
            if ($scope.stud == true) {
            return !$scope.course_list.some(function (options) {
                return options.select;
            });
        }
        }
        $scope.brisOptionsRequiredcls = function () {
            if ($scope.stud == true) {
            return !$scope.branchlist.some(function (options) {
                return options.select;
            });
        }
        }
        $scope.smisOptionsRequiredcls = function () {
            if ($scope.stud == true) {
            return !$scope.semisterlist.some(function (options) {
                return options.select;
            });
        }
        }

        $scope.isOptionsRequiredcls1 = function () {
           
            if ($scope.stf == true) {
                return !$scope.stafflist.some(function (options) {
                    return options.select;
                });
            }
          
        }
        $scope.isOptionsRequiredsec = function () {
            if ($scope.stud == true) {
            return !$scope.sectionList.some(function (options) {
                return options.select;
            });
        }
        }


        $scope.submitted = false;
        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.searchfilter = function (studentlst) {
            
            var studid = studentlst.amst_Id;
            var data = {
                "Amst_Id": studid
            }


            apiService.create("ClgLiveMeetingSchedule/getstudentdetails", data).
                then(function (promise) {
                    
                    $scope.studentlistall = promise.fillstudentalldetails;
                    $scope.examdetails = promise.examlist;
                    if ($scope.studentlistall != null) {
                        $scope.showStudentD = true;

                    } else {
                        $scope.showStudentD = false;

                    }
                    if ($scope.examdetails != null && $scope.examdetails != 0) {
                        $scope.showExamD = true;

                    } else {
                        $scope.showExamD = false;

                        swal("Student did't attend any Exam....!!")
                    }

                })
        };

        $scope.section = [];
        $scope.OnAcdyear = function (asmaY_Id) {

            var data = {
                "ASMAY_Id": asmaY_Id
            }
            apiService.create("ClgLiveMeetingSchedule/getcoursedata", data).
                then(function (promise) {
                    $scope.course_list = promise.course_list;
                })


        }

        $scope.selectedcourse_list = [];
        $scope.getbranch = function () {
          
            $scope.sectionList = [];
            $scope.selectedcourse_list = [];
            angular.forEach($scope.course_list, function (cls) {
                if (cls.select == true) {
                    $scope.selectedcourse_list.push({ AMCO_Id: cls.amcO_Id });
                }
            });


            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
                selectedcourse_list: $scope.selectedcourse_list,
            }
            apiService.create("ClgLiveMeetingSchedule/getbranchdata", data).
                then(function (promise) {
                   
                    $scope.branchlist = promise.branch_list;
                    
                })


        }


        $scope.selectedbranch = [];
        $scope.getsem = function () {

            $scope.sectionList = [];
            $scope.selectedcourse_list = [];
            angular.forEach($scope.course_list, function (cls) {
                if (cls.select == true) {
                    $scope.selectedcourse_list.push({ AMCO_Id: cls.amcO_Id });
                }
            });

            angular.forEach($scope.branchlist, function (cls) {
                if (cls.select == true) {
                    $scope.selectedbranch.push({ AMB_Id: cls.amB_Id });
                }
            });



            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
                selectedcourse_list: $scope.selectedcourse_list,
                selectedbranch: $scope.selectedbranch,
            }
            apiService.create("ClgLiveMeetingSchedule/getsemdata", data).
                then(function (promise) {

                    $scope.semisterlist = promise.semisterlist;
                    debugger;

                })


        }

        $scope.hmin = new Date();
        $scope.hmin.setHours(0);
        $scope.hmin.setMinutes(0);
        $scope.FOMST_HDWHrMin = $scope.hmin;



        var d = new Date();
        d.setHours(0);
        d.setMinutes(0);
        $scope.min = d;

        var maxsnfttim = new Date();
        maxsnfttim.setHours(18);
        maxsnfttim.setMinutes(0);
        $scope.maxtimme = maxsnfttim;

        var d2 = new Date();
        d2.setHours(7);
        d2.setMinutes(0);
        $scope.max = d2;

        $scope.hstep = 1;
        $scope.mstep = 1;

        $scope.options = {
            hstep: [1, 2, 3],
            mstep: [1, 5, 10, 15, 25, 30]
        };

        $scope.ismeridian = false;
        $scope.validatemax = function (maxdata) {

            // $scope.FOMST_IHalfLoginTime = maxdata;
            //$scope.FOMST_IIHalfLogoutTime = "";
            var dsttimee = $scope.FOMST_IHalfLoginTime;
            $scope.sresult = $filter('date')(dsttimee, 'HH:mm:ss a');
            $scope.eresult = $filter('date')(maxdata, 'HH:mm:ss a');
            //var startTime = moment(dsttimee, "HH:mm:ss a");
            //  var endTime = moment(maxdata, "HH:mm:ss a");
            var startTime = moment($scope.sresult, "HH:mm:ss a");
            var endTime = moment($scope.eresult, "HH:mm:ss a");
            var duration = moment.duration(endTime.diff(startTime));
            var hours = parseInt(duration.asHours());
            var minutes = parseInt(duration.asMinutes()) - hours * 60;
            var finlrst = hours + ":" + minutes;

            $scope.tmin = new Date();
            $scope.tmin.setHours(hours);
            $scope.tmin.setMinutes(minutes);
            $scope.tmax = new Date();
            $scope.tmax.setHours(hours);
            $scope.tmax.setMinutes(minutes);

            $scope.ttst = new Date();
            $scope.ttst.setHours(hours);
            $scope.ttst.setMinutes(minutes);

            $scope.FOMST_FDWHrMin = $scope.ttst;
            //$scope.FOMST_HDWHrMin = $scope.ttst;

            $scope.htmax = new Date();
            $scope.htmax.setHours(hours);
            $scope.htmax.setMinutes(minutes);

            if (maxdata >= new Date($scope.FOMST_IHalfLoginTime)) {
                $scope.totimemax = maxdata;
                var hh = $scope.totimemax.getHours();
                var mm = $scope.totimemax.getMinutes();
                $scope.max = maxdata;

                $scope.max.setMinutes(hh);
                $scope.max.setMinutes(mm);
            }
            else {
                $scope.FOMST_IIHalfLogoutTime = "";
            }

            // $scope.FOMST_IHalfLogoutTime = "";
        }


        $scope.deactive = function (employee) {
         ;
            var flag = employee.lmslmeeT_ActiveFlg;
            var mgs = "";
            var confirmmgs = "";

            var data = {
                "LMSLMEET_Id": employee.lmslmeeT_Id,
            }

            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            if (flag === true) {
                mgs = "Cancel";
                // mgs = "Delete";
                confirmmgs = "Cancelled";
            }
            else {
                mgs = "Re-Schedule";
                //mgs = "Activate";
                confirmmgs = "Re-Schedule";



            }
            swal({
                title: "Are you sure",
                text: "Do you want to " + mgs + " the Meeting??????",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + mgs + " it!",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {

                        apiService.create("ClgLiveMeetingSchedule/deactive", data).
                            then(function (promise) {
                                if (promise.returnval == true) {
                                    swal("Meeting " + confirmmgs + " " + "successfully");
                                }
                                else {
                                    swal("Meeting " + mgs + " Failed");
                                }
                                $state.reload();
                                //$scope.clear();
                                //$scope.BindData();
                            })

                    }
                    else {
                        swal("Meeting " + mgs + " Cancelled");
                    }
                });
        }
        $scope.cleardata = function () {
            $state.reload();;
        }


        $scope.validateTomintime = function (timedata) {

            //$scope.timedis1 = false;
            //$scope.timedis2 = true;
            $scope.FOMST_IIHalfLogoutTime = "";
            $scope.totime = timedata;
            var hh = $scope.totime.getHours();
            var mm = $scope.totime.getMinutes();
            $scope.min = timedata;

            $scope.min.setHours(hh);
            $scope.min.setMinutes(mm);
            $scope.minlnc = timedata;

            $scope.minlnc.setHours(hh);
            $scope.minlnc.setMinutes(mm);
            $scope.FOMST_IHalfLogoutTime = "";
        }
        $scope.searchchkbx23 = '';
        $scope.filterchkbx23 = function (obj) {
            return (angular.lowercase(obj.acmS_SectionName)).indexOf(angular.lowercase($scope.searchchkbx23)) >= 0;
        }
        $scope.searchchkbx231 = '';
        $scope.filterchkbx231 = function (obj) {
            return (angular.lowercase(obj.ismS_SubjectName)).indexOf(angular.lowercase($scope.searchchkbx231)) >= 0;
        }
        $scope.searchchkbx = '';
         $scope.filterchkbx = function (obj) {
             return (angular.lowercase(obj.amcO_CourseName)).indexOf(angular.lowercase($scope.searchchkbx)) >= 0;
        }
        $scope.brsearchchkbx = '';
         $scope.brfilterchkbx = function (obj) {
             return (angular.lowercase(obj.amB_BranchName)).indexOf(angular.lowercase($scope.brsearchchkbx)) >= 0;
        }


        $scope.smsearchchkbx = '';
        $scope.smfilterchkbx = function (obj) {
            return (angular.lowercase(obj.amsE_SEMName)).indexOf(angular.lowercase($scope.smsearchchkbx)) >= 0;
        }



        $scope.searchchkbx5 = '';
         $scope.filterchkbx5 = function (obj) {
             return (angular.lowercase(obj.hrmE_EmployeeFirstName)).indexOf(angular.lowercase($scope.searchchkbx5)) >= 0;
        }
        $scope.togchkbx = function () {
            $scope.usercheck = $scope.course_list.every(function (options) {
                return options.select;
            });
            $scope.getbranch();

        }

        $scope.brtogchkbx = function () {
            $scope.usercheck = $scope.branchlist.every(function (options) {
                return options.select;
            });
            $scope.getsem();

        }


        $scope.smtogchkbx = function () {
            $scope.usercheck = $scope.semisterlist.every(function (options) {
                return options.select;
            });
            $scope.getsection();

        }
        $scope.selectedsem = [];
        $scope.getsection = function () {
            debugger
            $scope.selectedsem = [];
            $scope.selectedcourse_list = [];
            $scope.selectedbranch= [];
            angular.forEach($scope.course_list, function (cls) {
                if (cls.select == true) {
                    $scope.selectedcourse_list.push({ AMCO_Id: cls.amcO_Id });
                }
            });

            angular.forEach($scope.branchlist, function (cls) {
                if (cls.select == true) {
                    $scope.selectedbranch.push({ AMB_Id: cls.amB_Id });
                }
            });
            angular.forEach($scope.semisterlist, function (cls) {
                if (cls.select == true) {
                    $scope.selectedsem.push({ AMSE_Id: cls.amsE_Id });
                }
            });


            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
                selectedcourse_list: $scope.selectedcourse_list,
                selectedbranch: $scope.selectedbranch,
                selectedsem: $scope.selectedsem,
            }
            apiService.create("ClgLiveMeetingSchedule/getsection", data).
                then(function (promise) {

                    $scope.sectionList = promise.sectionList;
                    debugger;

                })


        }


    };
})();