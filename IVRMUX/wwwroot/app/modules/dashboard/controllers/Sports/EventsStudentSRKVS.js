(function () {
    'use strict';

    angular
        .module('app')
        .controller('EventsStudentSRKVS', EventsStudentSRKVS);

    EventsStudentSRKVS.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache'];

    function EventsStudentSRKVS($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {


        $scope.searchValue = "";
        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;
        $scope.SPCCESTR_Id = 0;
        $scope.studentDropdown23 = [];
        $scope.editstulisttemp = [];
        var SPCCMSCC_NoOfAttempts = 0;
        $scope.obj = {};
        $scope.obj.userselecttwo = false;
        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey == key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        }
        $scope.editflag = false;

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
                $scope.eventname = promise.eventname;
                //$scope.classList = promise.classList;
                //$scope.sectionList = promise.sectionList;
                //$scope.studentDropdown23 = promise.studentList;

                $scope.categoryList = promise.categoryList;
                $scope.categoryListtt2 = promise.categoryListtt2;
                $scope.compitionLevelList = promise.compitionLevelList;
                $scope.categoryListttCls = promise.categoryListttCls;

                $scope.sportsCCList = promise.sportsCCList;
                $scope.uomList = promise.uomList;
                $scope.uomListtemp = promise.uomList;
                $scope.spcgrplist_new = promise.getMasterEvent;

                $scope.eventsStudentRecordList = promise.eventsStudentRecordListSRKVS;
                $scope.eventsListVenue = promise.eventmappingList;

            });
        }
        //==============================================================================

        //==================================Change Asmay_Id
        $scope.acdyrChange = function () {
            $scope.listof_classtwo = [];
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
                $scope.listof_classtwo = promise.listof_class;
                //$scope.eventname = promise.eventname;
                $scope.sectionList = promise.sectionList;


            })
        }
        $scope.VenUeOption = function () {            angular.forEach($scope.eventsListVenue, function (tt) {                if (tt.spccmeV_Id == $scope.spccmeV_Id) {                    $scope.spccE_StartDate = new Date(tt.spccE_StartDate);                    $scope.spccE_EndDate = new Date(tt.spccE_EndDate);                    $scope.minDatef = new Date(
                        $scope.fromDate,
                        $scope.frommon,
                        $scope.fromDay);                    $scope.maxDatef = new Date(
                        $scope.fromDate,
                        $scope.frommon,
                        $scope.fromDay + 365);                    return;                }            });        }
        $scope.Validations = function (dev) {
            //if ($scope.employeeid != null && $scope.employeeid.length > 0) {
            //    angular.forEach($scope.employeeid, function (tt) {
            //        if (tt.spccestR_Rank == dev.spccestR_Rank && dev.amsT_Id != tt.amsT_Id) {
            //            swal("Rank Already Selectd   !")
            //            tt.spccestR_Rank = "";
            //        }

            //    });
            //}
        }
        //==================================end


        //==================================radio button
        $scope.onselectradio = function () {
            var obj = {
                "radiotype": $scope.qualification_type
            }
            apiService.create("EventsStudentRecord/getevent/", obj).then(function (promise) {
                //$scope.eventname = promise.eventname;
            })
        };
        //==================================end



        //==================================Change Class
        $scope.classChange = function () {

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
        $scope.EvenTVenue = function () {
            $scope.eventsListVenue = [];
            $scope.spccmeV_Id = "";
            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
                "SPCCME_Id": $scope.spccmE_Id,
                "sportsName": "SRKVS",

            }
            apiService.create("EventsStudentRecord/EditDetailsSRKVS", data).then(function (promise) {
                if (promise.eventmappingList != null && promise.eventmappingList.length > 0) {
                    $scope.eventsListVenue = promise.eventmappingList;

                }

            })
        }
        $scope.UpdateStudent = function () {

            $scope.studentlistSrkvs = [];
            if ($scope.employeeid != null && $scope.employeeid.length > 0) {
                angular.forEach($scope.employeeid, function (tt) {
                    if (tt.selected == true) {

                        var spccestR_Rank = "";
                        if (tt.spccestR_Rank != "" && tt.spccestR_Rank != "0") {
                            spccestR_Rank = tt.spccestR_Rank;
                        }

                        $scope.studentlistSrkvs.push({
                            spccestR_Rank: spccestR_Rank,
                            amsT_Id: tt.amsT_Id,
                            spccestR_Points: tt.spccestR_Points,
                            spccestR_RecordBrokenFlag: tt.spccestR_RecordBrokenFlag,
                            spccestR_Remarks: tt.spccestR_Remarks,
                            studentName: tt.studentName,
                            amsT_AdmNo: tt.amsT_AdmNo,
                            asmcL_ClassName: tt.asmcL_ClassName,
                            asmC_SectionName: tt.asmC_SectionName,
                            ivrmgC_SportsPointsDropdownFlg: tt.ivrmgC_SportsPointsDropdownFlg,
                            spccmuoM_Id: tt.spccmuoM_Id,
                            spccestR_Value: tt.spccestR_Value,
                            spccestR_EventRecordFlg: tt.spccestR_EventRecordFlg,
                            spccestR_Id: tt.spccestR_Id,
                            MultipleBroken: tt.txxxx,
                            spccesT_Id: tt.spccesT_Id,
                            spccmscC_MultiAttemptFlg: tt.spccmscC_MultiAttemptFlg,
                        });
                    }


                });
            }
            if ($scope.studentlistSrkvs != null && $scope.studentlistSrkvs.length > 0) {
                var data = {
                    "student1": $scope.studentlistSrkvs

                }
                apiService.create("EventsStudentRecord/UpdateStudentSRKVS", data).then(function (promise) {
                    if (promise.returnVal == "Saved") {
                        swal("Record Saved Sucessfully !");
                        $('#myModalstudentList').modal('hide');
                    }
                    else if (promise.returnVal == "Notsaved") {
                        swal("Record Not Saved ");
                    }
                    else if (promise.returnVal == "admin") {
                        swal("Please Contact Administrator !");
                    }
                })

            }
            else {
                swal("Select Atleast One Student !")
            }

        }
        //getstudentdata
        $scope.viewstudentBook = function (data) {
            $scope.studentlsitdataModalT = [];
            $scope.studentlsitdataModalTemp = [];
            apiService.create("EventsStudentRecord/EditDetailsSRKVS/", data).then(function (promise) {

                $scope.studentlsitdataModalTemp = promise.editstulist
                if ($scope.studentlsitdataModalTemp != null && $scope.studentlsitdataModalTemp.length > 0) {
                    angular.forEach($scope.studentlsitdataModalTemp, function (tt) {
                        $scope.studentlsitdataModalT.push({
                            selected: true,
                            spccestR_Rank: tt.spccestR_Rank,
                            amsT_Id: tt.amsT_Id,
                            spccestR_Points: tt.spccestR_Points,
                            spccestR_RecordBrokenFlag: tt.spccestR_RecordBrokenFlag,
                            spccestR_Remarks: tt.spccestR_Remarks,
                            studentName: tt.studentName,
                            amsT_AdmNo: tt.amsT_AdmNo,
                            asmcL_ClassName: tt.asmcL_ClassName,
                            asmC_SectionName: tt.asmC_SectionName,
                            ivrmgC_SportsPointsDropdownFlg: tt.ivrmgC_SportsPointsDropdownFlg,
                            spccmuoM_Id: tt.spccmuoM_Id,
                            spccestR_Value: tt.spccestR_Value,
                            spccestR_EventRecordFlg: tt.spccestR_EventRecordFlg,
                            spccestR_Id: tt.spccestR_Id,
                            spccmscC_MultiAttemptFlg: tt.spccmscC_MultiAttemptFlg,
                            spccmscC_NoOfAttempts: tt.spccmscC_NoOfAttempts,
                            spccesT_Id: tt.spccesT_Id

                        });

                    });
                    //added By sanjeev
                    // $scope.get_grnreport = promise.studentlsitdataModalTemp;
                    $scope.employeeidTemp = [];
                    $scope.employeeid = [];
                    angular.forEach($scope.studentlsitdataModalTemp, function (tt) {
                        if ($scope.employeeid.length === 0) {
                            $scope.employeeid.push({
                                selected: true,
                                spccestR_Rank: tt.spccestR_Rank,
                                amsT_Id: tt.amsT_Id,
                                spccestR_Points: tt.spccestR_Points,
                                spccestR_RecordBrokenFlag: tt.spccestR_RecordBrokenFlag,
                                spccestR_Remarks: tt.spccestR_Remarks,
                                studentName: tt.studentName,
                                amsT_AdmNo: tt.amsT_AdmNo,
                                asmcL_ClassName: tt.asmcL_ClassName,
                                asmC_SectionName: tt.asmC_SectionName,
                                ivrmgC_SportsPointsDropdownFlg: tt.ivrmgC_SportsPointsDropdownFlg,
                                spccmuoM_Id: tt.spccmuoM_Id,
                                spccestR_Value: tt.spccestR_Value,
                                spccestR_EventRecordFlg: tt.spccestR_EventRecordFlg,
                                spccestR_Id: tt.spccestR_Id,
                                spccmscC_MultiAttemptFlg: tt.spccmscC_MultiAttemptFlg,
                                spccmscC_NoOfAttempts: tt.spccmscC_NoOfAttempts,
                                spccesT_Id: tt.spccesT_Id

                            });
                        } else if ($scope.employeeid.length > 0) {
                            var intcount = 0;
                            angular.forEach($scope.employeeid, function (emp) {
                                if (emp.amsT_Id === tt.amsT_Id) {
                                    intcount += 1;
                                }
                            });
                            if (intcount === 0) {
                                $scope.employeeid.push({
                                    selected: true,
                                    spccestR_Rank: tt.spccestR_Rank,
                                    amsT_Id: tt.amsT_Id,
                                    spccestR_Points: tt.spccestR_Points,
                                    spccestR_RecordBrokenFlag: tt.spccestR_RecordBrokenFlag,
                                    spccestR_Remarks: tt.spccestR_Remarks,
                                    studentName: tt.studentName,
                                    amsT_AdmNo: tt.amsT_AdmNo,
                                    asmcL_ClassName: tt.asmcL_ClassName,
                                    asmC_SectionName: tt.asmC_SectionName,
                                    ivrmgC_SportsPointsDropdownFlg: tt.ivrmgC_SportsPointsDropdownFlg,
                                    spccmuoM_Id: tt.spccmuoM_Id,
                                    spccestR_Value: tt.spccestR_Value,
                                    spccestR_EventRecordFlg: tt.spccestR_EventRecordFlg,
                                    spccestR_Id: tt.spccestR_Id,
                                    spccmscC_MultiAttemptFlg: tt.spccmscC_MultiAttemptFlg,
                                    spccmscC_NoOfAttempts: tt.spccmscC_NoOfAttempts,
                                    spccesT_Id: tt.spccesT_Id
                                });
                            }
                        }
                    });

                    angular.forEach($scope.employeeid, function (ddd) {
                        $scope.templist = [];
                        angular.forEach($scope.studentlsitdataModalTemp, function (dd) {
                            if (dd.amsT_Id === ddd.amsT_Id) {
                                $scope.templist.push(dd);
                            }
                        });
                        ddd.detailsstudent = $scope.templist;
                    });

                }



                $('#myModalstudentList').modal('show');
                $scope.userselecttwo = true;
            });

        }
        $scope.addtocart = function () {
            var taxx = [];
            var i = 0;
            if ($scope.materaldocuupload != null && $scope.materaldocuupload.length > 0) {
                angular.forEach($scope.materaldocuupload, function (cl) {
                    i = i + 1;
                    if (cl.spccestR_Value != null && cl.spccestR_Value != "") {
                        taxx.push({
                            spccestR_Value: cl.spccestR_Value,
                            spccestR_Id: cl.spccestR_Id,
                            indexValue: i
                        })
                    }
                });
                if ($scope.employeeid != null && $scope.employeeid.length > 0) {
                    for (var i = 0; i < $scope.employeeid.length; i++) {                        if ($scope.employeeid[i].spccestR_Id == $scope.SPCCESTR_Id) {                            $scope.employeeid[i].txxxx = taxx;                        }                    }
                }

            }

        }
        $scope.ViewAllMultipLe = function (user) {
            $scope.materaldocuupload = [];
            SPCCMSCC_NoOfAttempts = 0;
            $scope.SPCCESTR_Id = 0;
            SPCCMSCC_NoOfAttempts = user.spccmscC_NoOfAttempts;
            $('#myModalTax').modal('show');
            $scope.materaldocuupload = [{ id: 'materal' }];
            $scope.SPCCESTR_Id = user.spccestR_Id;
            if (user.detailsstudent !== null && user.detailsstudent.length > 0) {
                $scope.materaldocuupload = user.detailsstudent;
                angular.forEach(user.materaldocuupload, function (itm) {
                    itm.spccestR_Value = itm.spccestR_Value;
                    itm.spccestR_Id = itm.spccestR_Id;
                });
            }

        }
        $scope.materaldocuupload = [{ id: 'materal' }];
        $scope.addgrnrows = function () {
            var newItemNo = $scope.materaldocuupload.length + 1;
            if (newItemNo <= SPCCMSCC_NoOfAttempts) {
                $scope.materaldocuupload.push({ 'id': 'Materal' + newItemNo });
            }
        };
        $scope.removegrnrows = function (index) {
            var newItemNo = $scope.materaldocuupload.length - 1;
            $scope.materaldocuupload.splice(index, 1);
        };
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
                    $scope.studentDropdown23 = promise.studentList;
                    if ($scope.studentDropdown23 != null && $scope.studentDropdown23.length > 0) {
                        angular.forEach($scope.studentDropdown23, function (cls) {
                            cls.stud = true;
                        });
                    }

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
            if ($scope.myForm.$valid) {
                if ($scope.sectionList != null && $scope.sectionList.length > 0) {
                    angular.forEach($scope.sectionList, function (cls) {
                        if (cls.sectionsecl == true) {
                            $scope.sectonlst122.push(cls);
                        }
                    });
                }
                if ($scope.studentDropdown23 != null && $scope.studentDropdown23.length > 0) {
                    angular.forEach($scope.studentDropdown23, function (cls) {
                        if (cls.stud == true) {
                            $scope.studlsitdata1.push({
                                amsT_Id: cls.amsT_Id
                            });
                        }
                    });
                }
                //studentDropdown23
                var obj = {
                    "SPCCME_Id": $scope.spccmE_Id,
                    "spccmcL_Id": $scope.spccmcL_Id,
                    "SPCCMCC_Id": $scope.comcat_idcls,
                    "spccmscC_Id": $scope.spccmscC_Id,
                    "SPCCMUOM_Id": $scope.spccmuoM_Id,
                    "SPCCEST_House_Class_Flag": "CS",
                    "SPCCEST_OldRecord": $scope.spccesT_OldRecord,
                    "SPCCEST_Remarks": $scope.spccesT_Remarks,
                    "SPCCMSCCG_Id": $scope.spccmsccG_Idsub,
                    "ASMAY_Id": $scope.asmaY_Id,
                    "SPCCEST_Id": $scope.spccesT_Id,
                    "SPCCMEV_Id": $scope.spccmeV_Id,
                    "StartDate": new Date($scope.spccE_StartDate).toDateString(),
                    "EndDate": new Date($scope.spccE_EndDate).toDateString(),
                    student1: $scope.studlsitdata1,
                }

                apiService.create("EventsStudentRecord/saveRecordSRKVS", obj).
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
                        $state.reload();
                    });

            }
            else {
                $scope.submitted = true;
            }
        }
        //=======================================================================================
        // edit --  

        $scope.editdata = function (data) {
            $scope.studentlsitdata = [];
            $scope.editflag = false;
            angular.forEach($scope.listof_classtwo, function (itm) {
                itm.classselect23 = false;
            });
            angular.forEach($scope.sectionList, function (itm) {
                itm.sectionsecl = false;
            });
            $scope.spccesT_Id = "";

            //EditDetailsSRKVS
            //editdata
            apiService.create("EventsStudentRecord/EditDetailsSRKVS/", data).then(function (promise) {
                $scope.asmaY_Id = data.asmaY_Id;

                $scope.spccmE_Id = data.spccmE_Id;
                $scope.spccmE_Id = data.spccmE_Id;
                //  $scope.EvenTVenue();
                $scope.comcat_idcls = data.SPCCMCC_Id;
                $scope.spccmcL_Id = data.SPCCMCL_Id;
                $scope.spccmsccG_Id = data.mainID;
                $scope.get_SportsName();
                $scope.spccmscC_Id = data.spccmscC_Id;

                $scope.get_uom_Name();
                $scope.spccmsccG_Idsub = data.spccmsccG_Id;
                $scope.spccmuoM_Id = data.spccmuoM_Id;
                //$scope.spccE_StartDate = new Date(data.spccE_StartDate);
                //$scope.spccE_EndDate = new Date(data.SPCCE_EndDate);
                $scope.spccE_StartDate = new Date(data.StartDate);
                $scope.spccE_EndDate = new Date(data.EndDate);
                $scope.spccmeV_Id = data.SPCCMEV_Id;
                $scope.spccesT_Remarks = data.SPCCEST_Remarks;
                if (promise.editstulist != null && promise.editstulist.length > 0) {
                    $scope.editstulisttemp = promise.editstulist;
                    $scope.spccesT_Id = data.spccesT_Id;
                    $scope.listof_classtwo = [];
                    $scope.listof_classtwo = promise.listof_class;
                    $scope.sectionList = promise.sectionList;

                    angular.forEach($scope.listof_classtwo, function (itm) {
                        angular.forEach(promise.editstulist, function (itmtwo) {
                            if (itmtwo.ASMCL_Id == itm.class_id) {
                                itm.classselect23 = true;
                            }

                        });
                    });
                    angular.forEach($scope.sectionList, function (itm) {
                        angular.forEach(promise.editstulist, function (itmtwo) {
                            if (itmtwo.ASMS_Id == itm.asmS_Id) {
                                itm.sectionsecl = true;
                            }

                        });
                    });
                    $scope.sectionWiseCompStudentList(promise.editstulist);
                }

            });
        }






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
            $scope.SPCCEST_Remarks = "";
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
        $scope.check_allboxthree = function () {
            var toggleStatus1 = $scope.obj.userselecttwo;
            angular.forEach($scope.employeeid, function (itm) {
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

                $scope.studentDropdown23 = promise.studentList;
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



        //========================Get Sports Name
        $scope.get_SportsName = function () {
            $scope.spcgrplist_newsub = [];
            $scope.sportsCCList = [];
            $scope.spccmsccG_Idsub = "";
            $scope.spccmscC_Id = "";
            var obj = {
                "SPCCMSCCG_Id": $scope.spccmsccG_Id,
            }
            apiService.create("EventsStudentRecord/get_SportsName", obj).
                then(function (promise) {

                    $scope.sportsCCList = promise.sportsCCList;
                });
        }

        //======================Get UOM List
        $scope.get_uom_Name = function () {
            $scope.spcgrplist_newsub = [];
            $scope.spccmsccG_Idsub = "";
            var obj = {
                "SPCCMSCC_Id": $scope.spccmscC_Id,
                "SPCCMSCCG_Id": $scope.spccmsccG_Id,
            }
            apiService.create("EventsStudentRecord/get_uom_Name", obj).
                then(function (promise) {

                    // $scope.uomList = promise.uomList;
                    $scope.spcgrplist_newsub = promise.getsubsubevent;

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

                    $scope.studentDropdown23 = promise.studentList;


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

                    $scope.studentDropdown23 = promise.studentList;
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
        $scope.sectionWiseCompStudentList = function (editstulist) {

            $scope.studentDropdown23 = [];
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
            if ($scope.sectionList != null && $scope.sectionList.length > 0) {
                angular.forEach($scope.sectionList, function (cls) {
                    if (cls.sectionsecl == true) {
                        $scope.sectiondata34.push(cls);
                    }
                });
            }

            if ($scope.listof_classtwo != null && $scope.listof_classtwo.length > 0) {
                angular.forEach($scope.listof_classtwo, function (cls) {
                    if (cls.classselect23 == true) {
                        $scope.clssslistdata234.push(cls);

                    }
                });
            }




            var obj = {
                "ASMAY_Id": $scope.asmaY_Id,
                "sportsName": "SRKVS",
                clslistdat234: $scope.clssslistdata234,
                sectonlst: $scope.sectiondata34,
            }


            apiService.create("EventsStudentRecord/getStudents", obj).
                then(function (promise) {

                    $scope.studentDropdown23 = promise.studentList;
                    //angular.forEach($scope.studentDropdown23, function (cls) {

                    //    cls.stud = false;
                    //});

                    //
                    if ($scope.editstulisttemp != null && $scope.editstulisttemp.length > 0) {
                        angular.forEach($scope.studentDropdown23, function (itm) {
                            angular.forEach($scope.editstulisttemp, function (itmtwo) {
                                if (itmtwo.amsT_Id == itm.amsT_Id) {
                                    itm.stud = true;
                                }

                            });
                        });
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

                        $scope.studentDropdown23 = promise.studentList;
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
            angular.forEach($scope.listof_classtwo, function (cla) {
                cla.classselect23 = checkStatusallCls;
            });
            // $scope.get_ComCatgrylist();
        }
        $scope.togchkbxclass23 = function () {
            $scope.usercheckclass23 = $scope.listof_classtwo.every(function (options) {
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

            angular.forEach($scope.listof_classtwo, function (cls) {
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

                        $scope.studentDropdown23 = promise.studentList;

                        $scope.sectionList = promise.sectionList;
                        if ($scope.sectionList.length > 0) {
                            angular.forEach($scope.sectionList, function (sect) {
                                sect.sectionsecl = false;
                            });
                            $scope.togchkbx23();
                        }


                    });
            }
            else {
                //swal("Please Select Any one Class!!!");
                // $scope.comcat_idcls = "";
            }
        }


        //=======get List Of Competition Category(Class)
        $scope.get_ComCatgrylist = function () {
            $scope.categoryListttCls = [];
            $scope.clssslistdata234 = [];
            angular.forEach($scope.listof_classtwo, function (cls) {
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
        $scope.Deletemastercastedata = function (DeleteRecord, SweetAlert) {

            $scope.deleteId = DeleteRecord.spccesT_Id;
            var MdeleteId = $scope.deleteId;

            // swal(id);
            swal({
                title: "Are you sure?",
                text: "Do you want to delete the Caste ..!?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, delete it!",
                cancelButtonText: "Cancel..!",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {

                        var inputs = {
                            "SPCCEST_Id": MdeleteId

                        }
                        apiService.create("EventsStudentRecord/MasterDeleteEventsStudent", inputs).then(function (promise) {
                            if (promise.returnVal == 'Update') {
                                swal(" Deleted Successfully");
                                $state.reload();
                            }
                            else {
                                swal("Failed to Delete ");
                            }
                        })
                    }
                    else {
                        swal(" Deletion Cancelled");
                    }

                });
        }

    }
})();
