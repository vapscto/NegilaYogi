(function () {
    'use strict';

    angular
        .module('app')
        .controller('EventsStudentRecord', EventsStudentRecord);

    EventsStudentRecord.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache'];

    function EventsStudentRecord($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {


        $scope.searchValue = "";
        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;
        $scope.SPCCESTR_Id = 0;
        $scope.studentDropdown23 = [];
        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey == key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        }


        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;
            $scope.reverse3 = !$scope.reverse3;
        }
        $scope.itemsPerPage3 = 10;
        $scope.search3 = "";
        $scope.currentPage3 = 1;


        $scope.spccmsccG_SCCFlag = "Sports";
        $scope.spccesT_House_Class_Flag = "CC";


        //=================================== $scope.qualification_type = 'others';
        $scope.loadgrid = function () {

            apiService.getURI("EventsStudentRecord/loadgrid/", 1).then(function (promise) {
                $scope.academicYear = promise.academicYear;
                $scope.events = promise.events;
                //$scope.houselist = promise.houselist;
                //$scope.eventname = promise.eventname;
                //$scope.classList = promise.classList;
                //$scope.sectionList = promise.sectionList;
                //$scope.studentDropdown23 = promise.studentList;

                $scope.categoryList = promise.categoryList;
                $scope.categoryListtt2 = promise.categoryListtt2;
                $scope.compitionLevelList = promise.compitionLevelList;
                $scope.categoryListttCls = promise.categoryListttCls;

                $scope.sportsCCList = promise.sportsCCList;
                $scope.uomList = promise.uomList;
                $scope.spcgrplist_new = promise.groupsportdata;

                $scope.eventsStudentRecordList = promise.eventsStudentRecordList;

            });
        }
        //==============================================================================

        //==================================Change Asmay_Id
        $scope.acdyrChange = function () {

            $scope.studentDropdown23 = "";
            $scope.sectonlst = "";
            $scope.asmcL_Id = "";
            $scope.spccmcC_Id = "";
            $scope.houselist = "";
            $scope.classList = "";
            $scope.usercheckhous = "";
            $scope.usercheck = "";
            $scope.student_flag = false;

            var data = {
                "ASMAY_Id": $scope.asmaY_Id,

            }
            apiService.create("EventsStudentRecord/get_class_house", data).then(function (promise) {

                // $scope.classList = promise.classList;
                $scope.listof_class = promise.listof_class;
                $scope.houselisttt2 = promise.houselist;
                $scope.eventname = promise.eventname;
                //$scope.sectionList = promise.sectionList;


            })
        }
        //==================================end


        //==================================radio button
        $scope.onselectradio = function () {
            var obj = {
                "radiotype": $scope.qualification_type
            }
            apiService.create("EventsStudentRecord/getevent/", obj).then(function (promise) {
                $scope.eventname = promise.eventname;
            })
        };
        //==================================end



        //==================================Change Class
        $scope.classChange = function () {
            //$scope.spccmH_Id = "";
            $scope.studentDropdown23 = "";
            $scope.sectionList = [];
            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
                "ASMCL_Id": $scope.asmcL_Id,

            }
            apiService.create("EventsStudentRecord/classChange", data).then(function (promise) {
                if (promise.sectionList != null) {
                    $scope.sectionList = promise.sectionList;
                }

            })
        }
        //==================================end



        //===========================================Get_Student Data
        $scope.getStudent = function () {
            $scope.studentDropdown23 = "";
            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
                "ASMCL_Id": $scope.asmcL_Id,
                "ASMS_Id": $scope.asmS_Id
            }
            apiService.create("EventsStudentRecord/getStudents", data).then(function (promise) {
                if (promise.studentList != null) {
                    //$scope.studentDropdown23 = promise.studentList;
                    angular.forEach(promise.studentList, function (dev) {
                        if ($scope.studentDropdown23.length === 0) {
                            $scope.studentDropdown23.push(dev);
                        }

                        else if ($scope.studentDropdown23.length > 0) {
                            var intcount = 0;
                            angular.forEach($scope.studentDropdown23, function (emp) {
                                if (emp.amsT_Id === dev.amsT_Id) {
                                    intcount += 1;
                                }
                            });
                            if (intcount === 0) {
                                $scope.studentDropdown23.push(dev);
                            }
                        }
                    });
                }

            })
        }
        //==============================================================================


        //========================================================Save Record
        $scope.submitted = false;
        $scope.houselistdata = [];
        $scope.saveRecord = function () {
            $scope.sectonlst122 = [];
            $scope.studlsitdata1 = [];
            $scope.houslistdat = [];
            $scope.houselistdata23 = [];
            $scope.houslistdattt2 = "";

            if ($scope.spccesT_House_Class_Flag == "House") {
                angular.forEach($scope.houselisttt2, function (cls) {
                    if (cls.housselectt2 == true) {
                        $scope.houselistdata23.push(cls);
                    }
                });
                if ($scope.houselistdata23.length > 0) {
                    $scope.submitted = true;
                }
                else {
                    swal('Please Select House!!');
                    $scope.submitted = false;
                }
            }


            if ($scope.myForm.$valid) {
                if ($scope.spccesT_House_Class_Flag == "CC") {
                    angular.forEach($scope.studentlsitdata, function (cls) {
                        if (cls.selected == true) {
                            $scope.studlsitdata1.push(cls);
                        }
                    });

                    angular.forEach($scope.houselist, function (cls) {
                        if (cls.selected == true) {
                            $scope.houselistdata.push(cls);
                        }
                    });

                    var obj = {
                        "ASMAY_Id": $scope.asmaY_Id,
                        "SPCCESTR_Id": $scope.SPCCESTR_Id,
                        "SPCCME_Id": $scope.spccmE_Id,
                        "spccmcL_Id": $scope.spccmcL_Id,
                        "SPCCMCC_Id": $scope.spccmcC_Id,
                        "SPCCMSCCG_Id": $scope.spccmsccG_Id,
                        "spccmscC_Id": $scope.spccmscC_Id,
                        "SPCCMUOM_Id": $scope.spccmuoM_Id,
                        "SPCCEST_Remarks": $scope.spccesT_Remarks,
                        "SPCCEST_House_Class_Flag": $scope.spccesT_House_Class_Flag,
                        "SPCCEST_OldRecord": $scope.spccesT_OldRecord,
                        student1: $scope.studlsitdata1,
                        houslistdat: $scope.houselistdata,
                        "SPCCEST_Id": $scope.spccesT_Id,
                        "sendmail": $scope.sendmail,
                        "sendsms": $scope.sendsms,
                    }

                }

                else if ($scope.spccesT_House_Class_Flag == "House") {

                    angular.forEach($scope.studentlsitdata, function (cls) {
                        if (cls.selected == true) {
                            $scope.studlsitdata1.push(cls);
                        }
                    });

                    var obj = {
                        "ASMAY_Id": $scope.asmaY_Id,
                        "SPCCESTR_Id": $scope.SPCCESTR_Id,
                        "SPCCME_Id": $scope.spccmE_Id,
                        "spccmcL_Id": $scope.spccmcL_Id,
                        "SPCCMCC_Id": $scope.comcat_ids,
                        "SPCCMSCCG_Id": $scope.spccmsccG_Id,
                        "spccmscC_Id": $scope.spccmscC_Id,
                        "SPCCMUOM_Id": $scope.spccmuoM_Id,
                        "SPCCEST_Remarks": $scope.spccesT_Remarks,
                        "SPCCEST_House_Class_Flag": $scope.spccesT_House_Class_Flag,
                        "SPCCEST_OldRecord": $scope.spccesT_OldRecord,
                        student1: $scope.studlsitdata1,
                        "SPCCEST_Id": $scope.spccesT_Id,
                        "sendmail": $scope.sendmail,
                        "sendsms": $scope.sendsms,
                        houslistdattt2: $scope.houselistdata23,
                    }
                }

                else if ($scope.spccesT_House_Class_Flag == "CS") {

                    angular.forEach($scope.sectionList, function (cls) {
                        if (cls.sectionsecl == true) {
                            $scope.sectonlst122.push(cls);
                        }
                    });

                    angular.forEach($scope.studentlsitdata, function (cls) {
                        if (cls.selected == true) {
                            $scope.studlsitdata1.push(cls);
                        }
                    });

                    var obj = {
                        "ASMAY_Id": $scope.asmaY_Id,
                        // sectonlst: $scope.sectonlst122,
                        // "ASMCL_Id": $scope.asmcL_Id,
                        "SPCCESTR_Id": $scope.SPCCESTR_Id,
                        "SPCCME_Id": $scope.spccmE_Id,
                        "spccmcL_Id": $scope.spccmcL_Id,
                        "SPCCMCC_Id": $scope.comcat_idcls,
                        "SPCCMSCCG_Id": $scope.spccmsccG_Id,
                        "spccmscC_Id": $scope.spccmscC_Id,
                        "SPCCMUOM_Id": $scope.spccmuoM_Id,
                        "SPCCEST_Remarks": $scope.spccesT_Remarks,
                        "SPCCEST_House_Class_Flag": $scope.spccesT_House_Class_Flag,
                        "SPCCEST_OldRecord": $scope.spccesT_OldRecord,
                        student1: $scope.studlsitdata1,
                        "SPCCEST_Id": $scope.spccesT_Id,
                        "sendmail": $scope.sendmail,
                        "sendsms": $scope.sendsms,
                    }
                }


                if ($scope.studlsitdata1.length > 0) {


                    apiService.create("EventsStudentRecord/saveRecord", obj).
                        then(function (promise) {
                            if (promise.returnVal == 'Saved!') {
                                swal("Record Saved Successfully");
                                $state.reload();
                            }
                            else if (promise.returnVal == 'updated') {
                                swal("Record Updated Successfully");
                                $state.reload();
                            }
                            else if (promise.returnVal == 'duplicate') {
                                swal("Record already exist");
                            }
                            else if (promise.returnVal == "Save Failed!") {
                                swal("Failed to save record");
                            }
                            else if (promise.returnVal == "updateFailed") {
                                swal("Failed to update record");
                            }
                            else {
                                swal("Sorry...something went wrong");
                            }
                            $scope.loadgrid();
                            $scope.sendsms = 0;
                            $scope.sendmail = 0;
                        });
                }
                else {
                    swal("Please Select Student Records For Saving!");
                }
                //}
                //else {
                //    swal("Please Select Student Records For Saving!");
                //}

            }
            else {
                $scope.submitted = true;
            }
        }
        //=======================================================================================


        //====================================================Edit Record
        $scope.studentlsitdata = [];
        $scope.edit = function (data) {
            $scope.studentlsitdata = [];
            $scope.addbutton = false;


            //var data = {
            //    "SPCCEST_Id": data.spccesT_Id,
            //    "AMST_Id": data.amsT_Id,
            //    "SPCCESTR_Id": data.spccestR_Id,
            //    "ASMAY_Id": data.asmaY_Id,

            //}

            apiService.create("EventsStudentRecord/Edit/", data).then(function (promise) {


                //$scope.editDetails = promise.editDetails;
                //$scope.asmaY_Id = promise.editDetails[0].asmaY_Id;
                $scope.spccesT_Id = promise.editDetails[0].spccesT_Id;

                $scope.spccmcL_Id = promise.editDetails[0].spccmcL_Id;
                $scope.spccmscC_Id = promise.editDetails[0].spccmscC_Id;

                $scope.spccmuoM_Id = promise.editDetails[0].spccmuoM_Id;
                $scope.spccmE_Id = promise.editDetails[0].spccmE_Id;
                $scope.spccmsccG_Id = promise.editDetails[0].spccmsccG_Id;
                $scope.spccesT_Remarks = promise.editDetails[0].spccesT_Remarks;
                $scope.spccesT_House_Class_Flag = promise.editDetails[0].spccesT_House_Class_Flag;

                $scope.asmaY_Id = promise.editClsSecYear[0].asmaY_Id;
                //$scope.acdyrChange();
                $scope.eventname = promise.eventname;

                $scope.houselisttt2 = promise.houselistedit;

                if ($scope.spccesT_House_Class_Flag == "CC") {

                    $scope.houselist = promise.houselist;
                    $scope.spccmH_Id = promise.editDetails[0].spccmH_Id;
                    angular.forEach($scope.houselist, function (ss) {
                        angular.forEach(promise.editDetails, function (tt) {
                            if (tt.spccmH_Id == ss.spccmH_Id) {
                                ss.housselect = true;
                            }
                        })
                    })

                    $scope.spccmcC_Id = promise.editDetails[0].spccmcC_Id;

                    $scope.classList = promise.classList;
                    $scope.asmcL_Id = promise.editClsSecYear[0].asmcL_Id;

                    angular.forEach($scope.classList, function (ss) {

                        angular.forEach(promise.editClsSecYear, function (tt) {
                            if (tt.asmcL_Id == ss.asmcL_Id) {
                                ss.classselect = true;
                            }
                        })
                    })


                    $scope.asmS_Id = promise.editClsSecYear[0].asmS_Id;
                    $scope.sectionList = promise.sectionList;

                    angular.forEach($scope.sectionList, function (ss) {

                        angular.forEach(promise.editClsSecYear, function (tt) {
                            if (tt.asmS_Id == ss.asmS_Id) {
                                ss.sectionsecl = true;
                            }
                        })
                    })

                }
                else if ($scope.spccesT_House_Class_Flag == "House") {
                    $scope.categoryListtt2 = promise.categoryListtt2;
                    $scope.comcat_ids = promise.editDetails[0].spccmcC_Id;
                    $scope.houselisttt2 = promise.houselistedit;

                    $scope.house_id = promise.editDetails[0].spccmH_Id;

                    angular.forEach($scope.houselisttt2, function (ss) {

                        angular.forEach(promise.editDetails, function (tt) {
                            if (tt.spccmH_Id == ss.house_id) {
                                ss.housselectt2 = true;
                            }
                        })
                    })

                    $scope.classList = promise.classList;
                    $scope.asmcL_Id = promise.editClsSecYear[0].asmcL_Id;

                    angular.forEach($scope.classList, function (ss) {

                        angular.forEach(promise.editClsSecYear, function (tt) {
                            if (tt.asmcL_Id == ss.asmcL_Id) {
                                ss.classselect = true;
                            }
                        })
                    })

                    $scope.asmS_Id = promise.editClsSecYear[0].asmS_Id;
                    $scope.sectionList = promise.sectionList;

                    angular.forEach($scope.sectionList, function (ss) {
                        angular.forEach(promise.editClsSecYear, function (tt) {
                            if (tt.asmS_Id == ss.asmS_Id) {
                                ss.sectionsecl = true;
                            }
                        })
                    })
                }
                else if ($scope.spccesT_House_Class_Flag == "CS") {

                    $scope.usercheckclass23 = "";

                    $scope.categoryListttCls = promise.categoryListttCls;
                    $scope.comcat_idcls = promise.editDetails[0].spccmcC_Id;
                    $scope.houselistclass = promise.houselistclass;

                    $scope.house_idcls = promise.editDetails[0].spccmH_Id;

                    angular.forEach($scope.houselistclass, function (ss) {
                        angular.forEach(promise.editDetails, function (tt) {
                            if (tt.spccmH_Id == ss.house_idcls) {
                                ss.housselectt23 = true;
                            }
                        })
                    })

                    $scope.listof_class = promise.listof_class;
                    $scope.asmcL_Id = promise.editClsSecYear[0].asmcL_Id;
                    angular.forEach($scope.listof_class, function (ss) {
                        angular.forEach(promise.editClsSecYear, function (tt) {
                            if (tt.asmcL_Id == ss.class_id) {
                                ss.classselect23 = true;
                            }
                        })
                    })

                    $scope.asmS_Id = promise.editClsSecYear[0].asmS_Id;

                    $scope.sectionList = promise.sectionList;
                    angular.forEach($scope.sectionList, function (ss) {
                        angular.forEach(promise.editClsSecYear, function (tt) {
                            if (tt.asmS_Id == ss.asmS_Id) {
                                ss.sectionsecl = true;
                            }
                        })
                    })
                }


                $scope.spccmsccG_SCCFlag = promise.groupsportdata[0].spccmsccG_SCCFlag;
                $scope.onChangeActivities();
                $scope.datastudentas = promise.editstulist;
                $scope.amsT_Id = promise.editstulist[0].amsT_Id;
                //$scope.studentDropdown23 = promise.studentList
                $scope.studentDropdown23 = [];
                $scope.student_flag = true;

                angular.forEach(promise.studentList, function (dev) {
                    if ($scope.studentDropdown23.length === 0) {
                        $scope.studentDropdown23.push(dev);
                    }

                    else if ($scope.studentDropdown23.length > 0) {
                        var intcount = 0;
                        angular.forEach($scope.studentDropdown23, function (emp) {
                            if (emp.amsT_Id === dev.amsT_Id) {
                                intcount += 1;
                            }
                        });
                        if (intcount === 0) {
                            $scope.studentDropdown23.push(dev);
                        }
                    }
                });
                angular.forEach($scope.studentDropdown23, function (tt) {

                    angular.forEach($scope.datastudentas, function (ss) {
                        if (tt.amsT_Id == ss.amsT_Id) {
                            tt.stud = true;
                            $scope.studentlsitdata.push({ selected: true, spccestR_Rank: promise.editstulist[0].spccestR_Rank, amsT_Id: promise.editstulist[0].amsT_Id, spccestR_Points: promise.editstulist[0].spccestR_Points, spccestR_RecordBrokenFlag: promise.editstulist[0].spccestR_RecordBrokenFlag, spccestR_Remarks: promise.editstulist[0].spccestR_Remarks, studentName: promise.editstulist[0].studentName, amsT_AdmNo: promise.editstulist[0].amsT_AdmNo, asmcL_ClassName: promise.editstulist[0].asmcL_ClassName, asmC_SectionName: promise.editstulist[0].asmC_SectionName, spccmH_HouseName: promise.editstulist[0].spccmH_HouseName, ivrmgC_SportsPointsDropdownFlg: promise.editstulist[0].ivrmgC_SportsPointsDropdownFlg });
                        }
                    })
                });
            });
        }
        //=====================================================================================



        //==================================================Active/Deactive      
        $scope.deactivate = function (employee, SweetAlert) {

            var mgs = "";
            var confirmmgs = "";
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            if (employee.spccesT_ActiveFlag == true) {
                mgs = "Deactive";
                confirmmgs = "Deactivated";
            }
            else {
                mgs = "Active";
                confirmmgs = "Activated";
            }
            swal({
                title: "Are you sure",
                text: "Do you want to " + mgs + " record??????",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + mgs + " it!",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {

                        apiService.create("EventsStudentRecord/deactivate", employee).
                            then(function (promise) {
                                if (promise.returnVal2 == true) {
                                    swal(confirmmgs + " " + "successfully.");
                                }
                                else {
                                    swal(confirmmgs + " " + " successfully");
                                }
                                $state.reload();
                            })
                    }
                    else {
                        swal("Record " + mgs + " Cancelled");
                    }
                });
        }
        //===========================================================================


        //==============================================Cancel
        $scope.cancel = function () {

            $state.reload();

        }
        //====================================================

        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        $scope.searchchkbx23 = "";
        $scope.searchchkbx = "";
        $scope.filterchkbx = function (obj) {
            return (angular.lowercase(obj.studentName)).indexOf(angular.lowercase($scope.searchchkbx)) >= 0;
        }
        $scope.togchkbx = function () {
            $scope.usercheck = $scope.studentDropdown23.every(function (options) {
                return options.stud;
            });
        }
        $scope.all_check = function () {
            var checkStatus = $scope.usercheck;
            angular.forEach($scope.studentDropdown23, function (itm) {
                itm.stud = checkStatus;
            });
        }
        $scope.isOptionsRequired = function () {
            return !$scope.studentDropdown23.some(function (options) {
                return options.stud;
            });
        }
        $scope.addColumn4 = function () {
            $scope.selected = $scope.studentList.every(function (itm) { return itm.selected; });
        };



        $scope.all_check23 = function () {

            var checkStatus = $scope.usercheck23;
            angular.forEach($scope.sectionList, function (itm) {
                itm.sectionsecl = checkStatus;
            });

            if ($scope.usercheck23 == false) {

                $scope.usercheck = "";
                $scope.studentDropdown23 = [];
            }
            else if ($scope.usercheck23 == true) {
                $scope.sectionWiseCompStudentList();
            }

        }
        $scope.isOptionsRequired23 = function () {
            return !$scope.sectionList.some(function (options) {
                return options.sectionsecl;
            });
        }
        $scope.togchkbx23 = function () {
            $scope.usercheck23 = $scope.sectionList.every(function (options) {
                return options.sectionsecl;
            });
        }


        $scope.searchchkbxclass = "";
        $scope.all_checkclass = function () {
            var checkStatusCls = $scope.usercheckclass;
            angular.forEach($scope.classList, function (clas) {
                clas.classselect = checkStatusCls;
            });
            if ($scope.usercheckclass == false) {
                $scope.usercheck23 = "";
                $scope.sectionList = [];

                $scope.usercheck = "";
                $scope.studentDropdown23 = [];
            }
            else if ($scope.usercheckclass == true) {
                $scope.classWiseCompStudentList();
            }
        }
        $scope.togchkbxclass = function () {
            $scope.usercheckclass = $scope.classList.every(function (options) {
                return options.classselect;
            });
        }
        //========================================================Student Information House wise
        $scope.studentlsitdata = [];
        $scope.get_House_Stud_info = function () {


            $scope.student_flag = true;
            $scope.studentlsitdata = [];

            /////====================First
            angular.forEach($scope.studentDropdown23, function (cls) {
                if (cls.stud == true) {
                    $scope.studentlsitdata.push(cls);
                }
            });

        }
        //==================================================End

        //================Student Information Class/Section Wise

        $scope.get_Cls_Sec_student_info = function () {
            $scope.studentlsitdata23 = [];


            /////======================Second
            angular.forEach($scope.studentDropdown23, function (cls) {
                if (cls.stud == true) {
                    $scope.studentlsitdata23.push(cls);
                }
            });


            var obj = {
                "ASMAY_Id": $scope.asmaY_Id,
                "ASMCL_Id": $scope.asmcL_Id,
                "ASMS_Id": $scope.asmS_Id,
                "studentids": $scope.studentlsitdata23,
            }
            apiService.create("EventsStudentRecord/get_student_info/", obj).then(function (promise) {
                $scope.studentlsitdata = promise.studentList;
            })

        }
        //=============================================End

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

        //================================================Sports and Co Curricular Activities
        $scope.onChangeActivities = function () {
            var data = {
                "SPCCMSCCG_SCCFlag": $scope.spccmsccG_SCCFlag,
            }
            apiService.create("EventsStudentRecord/onChangeActivities/", data).then(function (promise) {

                $scope.spcgrplist_new = promise.groupsportdata;
            })
        }



        //=====================================================House Change        

        //$scope.changeRadiobtn = function () {

        //    if($scope.spccesT_House_Class_Flag == 'CS') {

        //    }
        //    else if ($scope.spccesT_House_Class_Flag != 'CS') {
        //        $scope.asmaY_Id = "";
        //        $scope.asmcL_Id = "";
        //        $scope.asmS_Id = "";
        //    }
        //}


        $scope.onhousechage = function () {
            $scope.studentDropdown23 = [];
            $scope.sectonlst122 = [];
            angular.forEach($scope.sectionList, function (cls) {
                if (cls.sectionsecl == true) {
                    $scope.sectonlst122.push(cls);
                }
            });

            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
                "ASMCL_Id": $scope.asmcL_Id,
                "SPCCMH_Id": $scope.spccmH_Id,
                //"ASMS_Id": $scope.asmS_Id,
                sectonlst: $scope.sectonlst122,
            }
            apiService.create("EventsStudentRecord/onhousechage/", data).then(function (promise) {

                //$scope.studentDropdown23 = promise.studentList;
                angular.forEach(promise.studentList, function (dev) {
                    if ($scope.studentDropdown23.length === 0) {
                        $scope.studentDropdown23.push(dev);
                    }

                    else if ($scope.studentDropdown23.length > 0) {
                        var intcount = 0;
                        angular.forEach($scope.studentDropdown23, function (emp) {
                            if (emp.amsT_Id === dev.amsT_Id) {
                                intcount += 1;
                            }
                        });
                        if (intcount === 0) {
                            $scope.studentDropdown23.push(dev);
                        }
                    }
                });
            })
        }
        //==================================


        //============Model click
        $scope.onmodelclick = function (id) {
            var data = {
                "SPCCEST_Id": id,
            }

            apiService.create("EventsStudentRecord/get_modeldata", data).then(function (promise) {
                $scope.modlastudlist = promise.modlastudlist;
            });
        };

        //=======================================



        //============================Active/Deactive  student data
        $scope.Deactivatestud = function (employee, SweetAlert) {

            var mgs = "";
            var confirmmgs = "";
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            if (employee.spccestR_ActiveFlag == true) {
                mgs = "Deactive";
                confirmmgs = "Deactivated";
            }
            else {
                mgs = "Active";
                confirmmgs = "Activated";
            }
            swal({
                title: "Are you sure",
                text: "Do you want to " + mgs + " record??????",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + mgs + " it!",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {

                        apiService.create("EventsStudentRecord/Deactivatestud", employee).
                            then(function (promise) {
                                if (promise.returnVal2 == true) {
                                    swal(confirmmgs + " " + "successfully.");
                                }
                                else {
                                    swal(confirmmgs + " " + " successfully");
                                }
                                $state.reload();
                            })
                    }
                    else {
                        swal("Record " + mgs + " Cancelled");
                    }
                });
        }
        //=====================================================
        $scope.student_flag = false;
        $scope.selectedSectionlist = [];
        $scope.get_StudentAgeFilter = function () {
            $scope.usercheck = "";
            $scope.student_flag = false;
            $scope.studentDropdown23 = [];
            //angular.forEach($scope.sectionList, function (sec) {
            //    if (sec.sectionsecl == true) {
            //        $scope.selectedSectionlist.push({ asmS_Id: sec.asmS_Id });
            //    }
            //});

            var obj = {
                "ASMAY_Id": $scope.asmaY_Id,
                //"SPCCMH_Id": $scope.spccmH_Id,
                //"ASMCL_Id": $scope.asmcL_Id,
                "SPCCMCC_Id": $scope.spccmcC_Id,
                //selectedSectionlist: $scope.selectedSectionlist,
            }
            apiService.create("EventsStudentRecord/get_StudentAgeFilter", obj).
                then(function (promise) {
                    angular.forEach(promise.studentList, function (dev) {
                        if ($scope.studentDropdown23.length === 0) {
                            $scope.studentDropdown23.push(dev);
                        }

                        else if ($scope.studentDropdown23.length > 0) {
                            var intcount = 0;
                            angular.forEach($scope.studentDropdown23, function (emp) {
                                if (emp.amsT_Id === dev.amsT_Id) {
                                    intcount += 1;
                                }
                            });
                            if (intcount === 0) {
                                $scope.studentDropdown23.push(dev);
                            }
                        }
                    });
                    // $scope.studentDropdown23 = promise.studentList;
                    console.log($scope.studentDropdown23);

                    $scope.houselist = promise.houselist;
                    if ($scope.houselist.length > 0) {
                        angular.forEach($scope.houselist, function (itm) {
                            itm.housselect = true;
                        });
                        $scope.togchkbxhouse();
                    }

                    $scope.classList = promise.classList;

                    if ($scope.classList.length > 0) {
                        angular.forEach($scope.classList, function (clss) {
                            clss.classselect = true;
                        });
                        $scope.togchkbxclass();
                    }
                    $scope.sectionList = promise.sectionList;
                    if ($scope.sectionList.length > 0) {
                        angular.forEach($scope.sectionList, function (sect) {
                            sect.sectionsecl = true;
                        });
                        $scope.togchkbx23();
                    }
                });
        }



        //========================Get Sports Name
        $scope.get_SportsName = function () {
            var obj = {
                "SPCCMSCCG_Id": $scope.spccmsccG_Id,
                //"ASMAY_Id": $scope.asmaY_Id,
            }
            apiService.create("EventsStudentRecord/get_SportsName", obj).
                then(function (promise) {

                    $scope.sportsCCList = promise.sportsCCList;
                });
        }

        //======================Get UOM List
        $scope.get_uom_Name = function () {
            var obj = {
                "SPCCMSCC_Id": $scope.spccmscC_Id,
            }
            apiService.create("EventsStudentRecord/get_uom_Name", obj).
                then(function (promise) {

                    $scope.uomList = promise.uomList;
                });
        }

        //======Data filter based on check box selection
        $scope.searchchkbxhos = "";
        $scope.all_checkhous = function () {
            var checkStatus = $scope.usercheckhous;
            angular.forEach($scope.houselist, function (itm) {
                itm.housselect = checkStatus;
            });

            if ($scope.usercheckhous == false) {
                $scope.usercheck23 = "";
                $scope.sectionList = [];

                $scope.usercheckclass = "";
                $scope.classList = [];

                $scope.usercheck = "";
                $scope.studentDropdown23 = [];
            }
            else if ($scope.usercheckhous == true) {
                $scope.houseWiseCompStudentList();
            }
        }

        $scope.togchkbxhouse = function () {
            $scope.usercheckhous = $scope.houselist.every(function (options) {
                return options.housselect;
            });
        }


        //======get student list based on House selection
        $scope.houseWiseCompStudentList = function () {
            $scope.houselistdata = [];
            $scope.usercheck = "";
            $scope.student_flag = false;
            $scope.houslistdat = [];
            $scope.studentDropdown23 = [];
            angular.forEach($scope.houselist, function (cls) {
                if (cls.housselect == true) {
                    $scope.houselistdata.push(cls);
                }
            });

            var obj = {
                "ASMAY_Id": $scope.asmaY_Id,
                "SPCCMCC_Id": $scope.spccmcC_Id,
                houslistdat: $scope.houselistdata,
            }
            apiService.create("EventsStudentRecord/houseWiseCompStudentList", obj).
                then(function (promise) {

                    //$scope.studentDropdown23 = promise.studentList;
                    angular.forEach(promise.studentList, function (dev) {
                        if ($scope.studentDropdown23.length === 0) {
                            $scope.studentDropdown23.push(dev);
                        }

                        else if ($scope.studentDropdown23.length > 0) {
                            var intcount = 0;
                            angular.forEach($scope.studentDropdown23, function (emp) {
                                if (emp.amsT_Id === dev.amsT_Id) {
                                    intcount += 1;
                                }
                            });
                            if (intcount === 0) {
                                $scope.studentDropdown23.push(dev);
                            }
                        }
                    });

                    $scope.classList = promise.classList;

                    if ($scope.classList.length > 0) {
                        angular.forEach($scope.classList, function (clss) {
                            clss.classselect = true;
                        });
                        $scope.togchkbxclass();
                    }
                    $scope.sectionList = promise.sectionList;
                    if ($scope.sectionList.length > 0) {
                        angular.forEach($scope.sectionList, function (sect) {
                            sect.sectionsecl = true;
                        });
                        $scope.togchkbx23();
                    }

                });
        }

        //======get student list based on Class selection
        $scope.classWiseCompStudentList = function () {
            $scope.studentDropdown23 = [];
            $scope.clssslistdata = [];
            $scope.houselistdata = [];
            $scope.usercheck = "";
            $scope.student_flag = false;
            $scope.clslistdat = [];
            $scope.houslistdat = [];
            $scope.houslistdattt2 = [];
            $scope.houselistdata23 = [];

            angular.forEach($scope.classList, function (cls) {
                if (cls.classselect == true) {
                    $scope.clssslistdata.push(cls);
                }
            });

            if ($scope.spccesT_House_Class_Flag == 'CC') {
                angular.forEach($scope.houselist, function (cls) {
                    if (cls.housselect == true) {
                        $scope.houselistdata.push(cls);
                    }
                });
                var obj = {
                    "ASMAY_Id": $scope.asmaY_Id,
                    "SPCCMCC_Id": $scope.spccmcC_Id,
                    houslistdat: $scope.houselistdata,
                    clslistdat: $scope.clssslistdata,
                    "SPCCEST_House_Class_Flag": $scope.spccesT_House_Class_Flag,
                }

            }
            else if ($scope.spccesT_House_Class_Flag == 'House') {

                angular.forEach($scope.houselisttt2, function (cls) {
                    if (cls.housselectt2 == true) {
                        $scope.houselistdata23.push(cls);
                    }
                });
                var obj = {
                    "ASMAY_Id": $scope.asmaY_Id,
                    "SPCCMCC_Id": $scope.comcat_ids,
                    houslistdattt2: $scope.houselistdata23,
                    clslistdat: $scope.clssslistdata,
                    "SPCCEST_House_Class_Flag": $scope.spccesT_House_Class_Flag,
                }
            }

            apiService.create("EventsStudentRecord/classWiseCompStudentList", obj).
                then(function (promise) {
                    angular.forEach(promise.studentList, function (dev) {
                        if ($scope.studentDropdown23.length === 0) {
                            $scope.studentDropdown23.push(dev);
                        }

                        else if ($scope.studentDropdown23.length > 0) {
                            var intcount = 0;
                            angular.forEach($scope.studentDropdown23, function (emp) {
                                if (emp.amsT_Id === dev.amsT_Id) {
                                    intcount += 1;
                                }
                            });
                            if (intcount === 0) {
                                $scope.studentDropdown23.push(dev);
                            }
                        }
                    });
                    // $scope.studentDropdown23 = promise.studentList;
                    $scope.sectionList = promise.sectionList;
                    if ($scope.sectionList.length > 0) {
                        angular.forEach($scope.sectionList, function (sect) {
                            sect.sectionsecl = true;
                        });
                        $scope.togchkbx23();
                    }
                });

        }

        //========get student list based on section selection
        $scope.sectionWiseCompStudentList = function () {

            $scope.houselistdata = [];
            $scope.clssslistdata = [];
            $scope.sectiondata34 = [];
            $scope.usercheck = "";
            $scope.student_flag = false;
            $scope.houslistdat = [];
            $scope.clslistdat = [];
            $scope.sectonlst = [];
            $scope.houslistdattt2 = [];
            $scope.houselistdata23 = [];
            $scope.clssslistdata234 = [];
            $scope.clslistdat234 = [];
            $scope.studentDropdown23 = [];
            angular.forEach($scope.sectionList, function (cls) {
                if (cls.sectionsecl == true) {
                    $scope.sectiondata34.push(cls);
                }
            });

            if ($scope.spccesT_House_Class_Flag == 'CC') {
                angular.forEach($scope.classList, function (cls) {
                    if (cls.classselect == true) {
                        $scope.clssslistdata.push(cls);
                    }
                });

                angular.forEach($scope.houselist, function (cls) {
                    if (cls.housselect == true) {
                        $scope.houselistdata.push(cls);
                    }
                });

                var obj = {
                    "ASMAY_Id": $scope.asmaY_Id,
                    "SPCCMCC_Id": $scope.spccmcC_Id,
                    houslistdat: $scope.houselistdata,
                    clslistdat: $scope.clssslistdata,
                    sectonlst: $scope.sectiondata34,
                    "SPCCEST_House_Class_Flag": $scope.spccesT_House_Class_Flag,
                }

            }
            else if ($scope.spccesT_House_Class_Flag == 'House') {

                angular.forEach($scope.classList, function (cls) {
                    if (cls.classselect == true) {
                        $scope.clssslistdata.push(cls);
                    }
                });

                angular.forEach($scope.houselisttt2, function (cls) {
                    if (cls.housselectt2 == true) {
                        $scope.houselistdata23.push(cls);
                    }
                });
                var obj = {
                    "ASMAY_Id": $scope.asmaY_Id,
                    "SPCCMCC_Id": $scope.comcat_ids,
                    houslistdattt2: $scope.houselistdata23,
                    clslistdat: $scope.clssslistdata,
                    "SPCCEST_House_Class_Flag": $scope.spccesT_House_Class_Flag,
                    sectonlst: $scope.sectiondata34,
                }
            }

            else if ($scope.spccesT_House_Class_Flag == 'CS') {

                angular.forEach($scope.listof_class, function (cls) {
                    if (cls.classselect23 == true) {
                        $scope.clssslistdata234.push(cls);
                    }
                });

                angular.forEach($scope.houselisttt2, function (cls) {
                    if (cls.housselectt2 == true) {
                        $scope.houselistdata23.push(cls);
                    }
                });

                var obj = {
                    "ASMAY_Id": $scope.asmaY_Id,
                    "SPCCMCC_Id": $scope.comcat_idcls,
                    houslistdattt2: $scope.houselistdata23,
                    clslistdat234: $scope.clssslistdata234,
                    "SPCCEST_House_Class_Flag": $scope.spccesT_House_Class_Flag,
                    sectonlst: $scope.sectiondata34,
                }

            }

            apiService.create("EventsStudentRecord/sectionWiseCompStudentList", obj).
                then(function (promise) {

                    //$scope.studentDropdown23 = promise.studentList;
                    angular.forEach(promise.studentList, function (dev) {
                        if ($scope.studentDropdown23.length === 0) {
                            $scope.studentDropdown23.push(dev);
                        }

                        else if ($scope.studentDropdown23.length > 0) {
                            var intcount = 0;
                            angular.forEach($scope.studentDropdown23, function (emp) {
                                if (emp.amsT_Id === dev.amsT_Id) {
                                    intcount += 1;
                                }
                            });
                            if (intcount === 0) {
                                $scope.studentDropdown23.push(dev);
                            }
                        }
                    });
                    $scope.houselistclass = promise.houselistclass;
                    if ($scope.houselistclass.length > 0) {
                        angular.forEach($scope.houselistclass, function (sec) {
                            sec.housselectt23 = true;
                        });
                        $scope.togchkbxhouses23();
                    }
                });
        }

        //============filter data
        $scope.searchchkbxhoss2 = "";
        $scope.all_checkhouss2 = function () {
            var checkStatus = $scope.usercheckhouss2;
            angular.forEach($scope.houselisttt2, function (itm) {
                itm.housselectt2 = checkStatus;
            });
        }

        $scope.togchkbxhouses2 = function () {
            $scope.usercheckhouss2 = $scope.houselisttt2.every(function (options) {
                return options.housselectt2;
            });
        }

        //== student Age Filter Based on House Selection
        $scope.get_houseCatAgeFilter = function () {

            $scope.student_flag = false;
            $scope.houslistdattt2 = [];
            $scope.houselistdata23 = [];
            $scope.studentDropdown23 = [];
            angular.forEach($scope.houselisttt2, function (cls) {
                if (cls.housselectt2 == true) {
                    $scope.houselistdata23.push(cls);
                }
            });
            if ($scope.houselistdata23.length > 0) {
                var obj = {
                    "ASMAY_Id": $scope.asmaY_Id,
                    "SPCCMCC_Id": $scope.comcat_ids,
                    houslistdattt2: $scope.houselistdata23,
                }

                apiService.create("EventsStudentRecord/get_houseCatAgeFilter", obj).
                    then(function (promise) {

                        // $scope.studentDropdown23 = promise.studentList;
                        angular.forEach(promise.studentList, function (dev) {
                            if ($scope.studentDropdown23.length === 0) {
                                $scope.studentDropdown23.push(dev);
                            }

                            else if ($scope.studentDropdown23.length > 0) {
                                var intcount = 0;
                                angular.forEach($scope.studentDropdown23, function (emp) {
                                    if (emp.amsT_Id === dev.amsT_Id) {
                                        intcount += 1;
                                    }
                                });
                                if (intcount === 0) {
                                    $scope.studentDropdown23.push(dev);
                                }
                            }
                        });
                        $scope.classList = promise.classList;

                        if ($scope.classList.length > 0) {
                            angular.forEach($scope.classList, function (clss) {
                                clss.classselect = true;
                            });
                            $scope.togchkbxclass();
                        }
                        $scope.sectionList = promise.sectionList;
                        if ($scope.sectionList.length > 0) {
                            angular.forEach($scope.sectionList, function (sect) {
                                sect.sectionsecl = true;
                            });
                            $scope.togchkbx23();
                        }


                    });
            }
            else {
                swal("Please Select Any one House!!!");
            }
        }

        //=================================================================

        //======Filter Data Based on Selection of Check box
        $scope.searchchkbxclass23 = "";
        $scope.all_checkclass23 = function () {
            var checkStatusallCls = $scope.usercheckclass23;
            angular.forEach($scope.listof_class, function (cla) {
                cla.classselect23 = checkStatusallCls;
            });
            $scope.get_ComCatgrylist();
        }
        $scope.togchkbxclass23 = function () {
            $scope.usercheckclass23 = $scope.listof_class.every(function (options) {
                return options.classselect23;
            });
        }


        $scope.searchchkbxhoss23 = "";
        $scope.all_checkhouss23 = function () {
            var checkStatusallhous = $scope.usercheckhouss23;
            angular.forEach($scope.houselistclass, function (hos) {
                hos.housselectt23 = checkStatusallhous;
            });
            if ($scope.usercheckhouss23 == false) {
                $scope.usercheck = "";
                $scope.studentDropdown23 = [];
            }
            else if ($scope.usercheckhouss23 == true) {

                $scope.sectionWiseCompStudentList();
            }

        }
        $scope.togchkbxhouses23 = function () {
            $scope.usercheckhouss23 = $scope.houselistclass.every(function (options) {
                return options.housselectt23;
            });
        }

        //============class wise age filter
        $scope.comcatwise_classAgefilter = function () {

            $scope.usercheck = "";
            $scope.student_flag = false;
            $scope.clssslistdata234 = [];
            $scope.clslistdat234 = [];
            $scope.studentDropdown23 = [];
            angular.forEach($scope.listof_class, function (cls) {
                if (cls.classselect23 == true) {
                    $scope.clssslistdata234.push(cls);
                }
            });
            if ($scope.clssslistdata234.length > 0) {

                var obj = {
                    "ASMAY_Id": $scope.asmaY_Id,
                    "SPCCMCC_Id": $scope.comcat_idcls,
                    clslistdat234: $scope.clssslistdata234,
                }

                apiService.create("EventsStudentRecord/comcatwise_classAgefilter", obj).
                    then(function (promise) {

                        //$scope.studentDropdown23 = promise.studentList;
                        angular.forEach(promise.studentList, function (dev) {
                            if ($scope.studentDropdown23.length === 0) {
                                $scope.studentDropdown23.push(dev);
                            }

                            else if ($scope.studentDropdown23.length > 0) {
                                var intcount = 0;
                                angular.forEach($scope.studentDropdown23, function (emp) {
                                    if (emp.amsT_Id === dev.amsT_Id) {
                                        intcount += 1;
                                    }
                                });
                                if (intcount === 0) {
                                    $scope.studentDropdown23.push(dev);
                                }
                            }
                        });
                        $scope.sectionList = promise.sectionList;
                        if ($scope.sectionList.length > 0) {
                            angular.forEach($scope.sectionList, function (sect) {
                                sect.sectionsecl = true;
                            });
                            $scope.togchkbx23();
                        }

                        $scope.houselistclass = promise.houselistclass;
                        if ($scope.houselistclass.length > 0) {
                            angular.forEach($scope.houselistclass, function (sec) {
                                sec.housselectt23 = true;
                            });
                            $scope.togchkbxhouses23();
                        }
                    });
            }
            else {
                swal("Please Select Any one Class!!!");
            }
        }

        //===get student,Section,Class and Competition Category based on House Change
        $scope.houseWiseCompcatClssSectStudentList = function () {
            $scope.studentDropdown23 = [];
            $scope.usercheck = "";
            $scope.student_flag = false;
            $scope.clssslistdata234 = [];
            $scope.sectonlst = [];
            $scope.sectiondata34 = [];
            $scope.houseclsselist = [];
            $scope.hosueclssecllist = [];

            angular.forEach($scope.listof_class, function (cls) {
                if (cls.classselect23 == true) {
                    $scope.clssslistdata234.push(cls);
                }
            });
            angular.forEach($scope.sectionList, function (cls) {
                if (cls.sectionsecl == true) {
                    $scope.sectiondata34.push(cls);
                }
            });

            angular.forEach($scope.houselistclass, function (cls) {
                if (cls.housselectt23 == true) {
                    $scope.houseclsselist.push(cls);
                }
            });


            var obj = {
                "ASMAY_Id": $scope.asmaY_Id,
                "SPCCMCC_Id": $scope.comcat_idcls,
                clslistdat234: $scope.clssslistdata234,
                sectonlst: $scope.sectiondata34,
                hosueclssecllist: $scope.houseclsselist,
            }

            apiService.create("EventsStudentRecord/houseWiseCompcatClssSectStudentList", obj).
                then(function (promise) {

                    // $scope.studentDropdown23 = promise.studentList;
                    angular.forEach(promise.studentList, function (dev) {
                        if ($scope.studentDropdown23.length === 0) {
                            $scope.studentDropdown23.push(dev);
                        }

                        else if ($scope.studentDropdown23.length > 0) {
                            var intcount = 0;
                            angular.forEach($scope.studentDropdown23, function (emp) {
                                if (emp.amsT_Id === dev.amsT_Id) {
                                    intcount += 1;
                                }
                            });
                            if (intcount === 0) {
                                $scope.studentDropdown23.push(dev);
                            }
                        }
                    });
                });

        }

        //=======get List Of Competition Category(Class)
        $scope.get_ComCatgrylist = function () {

            $scope.clssslistdata234 = [];
            angular.forEach($scope.listof_class, function (cls) {
                if (cls.classselect23 == true) {
                    $scope.clssslistdata234.push(cls);
                }
            });
            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
                clslistdat234: $scope.clssslistdata234,
            }
            apiService.create("EventsStudentRecord/get_ComCatgrylistClassWise", data).then(function (promise) {

                $scope.categoryListttCls = promise.categoryListttCls;

            });
        }

        //===============================End===============================//

    }
})();
