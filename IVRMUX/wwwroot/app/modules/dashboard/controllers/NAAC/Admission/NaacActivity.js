(function () {
    'use strict';
    angular.module('app').controller('NaacActivityController', NaacActivityController)

    NaacActivityController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$q', '$sce','myFactorynaac']
    function NaacActivityController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $q, $sce, myFactorynaac) {


        $scope.search = "";
        var miid = myFactorynaac.get();
        if (miid > 0) {
            $scope.mI_Id = miid;
        } else {
            $scope.mI_Id = 0;
        }
        $scope.ncacsA343_Id = 0;
        $scope.loaddata = function () {
            $scope.ncacsA343_Id = 0;
            $scope.change_institution();
            if ($scope.mI_Id == undefined || $scope.mI_Id == null || $scope.mI_Id == '') {
                $scope.mI_Id = 0;
            }
            //var pageid = 2;
            apiService.getURI("NaacActivity/loaddata", $scope.mI_Id).then(function (promise) {
             
                $scope.institutionlist = promise.institutionlist;
                $scope.mI_Id = promise.mI_Id;
                $scope.yeardata = promise.yearlist;
                $scope.filldepartment = promise.filldepartment;
                $scope.alldata1 = promise.alldata1;
            })
        };

        //==================get course
        $scope.get_course = function () {
            $scope.usercheck = false;
            $scope.AMCO_Id = "";
            $scope.courselist = [];
            $scope.AMB_Id = "";
            $scope.branchlist = [];
            $scope.studentlist = [];
            $scope.AMSE_Id = "";
            $scope.semisterlist = [];
            $scope.ACMS_Id = "";
            $scope.sectionlist = [];
            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
                "MI_Id": $scope.mI_Id,
            }
            apiService.create("NaacActivity/get_course", data).then(function (promise) {
                if (promise.courselist.length > 0) {
                    $scope.courselist = promise.courselist;
                }
                else {
                    swal("For Selected Year, Course is not Available!");
                }
            });
        }

        //==================get branch
        $scope.get_branch = function () {
            $scope.usercheck = false;
            $scope.AMB_Id = "";
            $scope.branchlist = [];
            $scope.studentlist = [];
            $scope.AMSE_Id = "";
            $scope.semisterlist = [];
            $scope.ACMS_Id = "";
            $scope.sectionlist = [];
            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
                "AMCO_Id": $scope.AMCO_Id,
                "MI_Id": $scope.mI_Id,
            }
            apiService.create("NaacActivity/get_branch", data).then(function (promise) {
                if (promise.branchlist.length > 0) {
                    $scope.branchlist = promise.branchlist;
                }
                else {
                    swal('For Selected Course, Branch is not Available!')
                }
            });
        }

        //==================get semester
        $scope.get_sems = function () {
            $scope.usercheck = false;
            $scope.studentlist = [];
            $scope.AMSE_Id = "";
            $scope.semisterlist = [];
            $scope.ACMS_Id = "";
            $scope.sectionlist = [];
            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
                "AMCO_Id": $scope.AMCO_Id,
                "AMB_Id": $scope.AMB_Id,
                "MI_Id": $scope.mI_Id,
            }
            apiService.create("NaacActivity/get_sems", data).then(function (promise) {
                if (promise.semisterlist.length > 0) {
                    $scope.semisterlist = promise.semisterlist;
                }
                else {
                    swal('For Selected Branch, Semester is not Available!')
                }
            });
        }

        //==================get section
        $scope.get_section = function () {
            $scope.usercheck = false;
            $scope.studentlist = [];
            $scope.ACMS_Id = "";
            $scope.sectionlist = [];
            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
                "AMCO_Id": $scope.AMCO_Id,
                "AMB_Id": $scope.AMB_Id,
                "AMSE_Id": $scope.AMSE_Id,
                "MI_Id": $scope.mI_Id,
            }
            apiService.create("NaacActivity/get_section", data).then(function (promise) {
                if (promise.sectionlist.length > 0) {
                    $scope.sectionlist = promise.sectionlist;
                }
                else {
                    swal('For Selected Semester, Section is not Available!')
                }
            });
        }

        //==================get student 
        $scope.GetStudentDetails = function () {
            $scope.studentlist = [];
            $scope.usercheck = false;
            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
                "AMCO_Id": $scope.AMCO_Id,
                "AMB_Id": $scope.AMB_Id,
                "AMSE_Id": $scope.AMSE_Id,
                "ACMS_Id": $scope.ACMS_Id,
                "MI_Id": $scope.mI_Id,
            }
            apiService.create("NaacActivity/GetStudentDetails", data).then(function (promise) {
                if (promise.studentlist.length > 0) {
                    $scope.studentlist = promise.studentlist;
                }
                else {
                    swal('For Selected Section Students are not Available!');
                }
            });
        }

        //==================get designation
        $scope.get_Designation = function () {
            $scope.usercheckC = false;
            $scope.HRMDES_Id = "";
            $scope.filldesignation = [];
            $scope.stafftlist = [];
            var data = {
                "HRMD_Id": $scope.HRMD_Id,
                "MI_Id": $scope.mI_Id,
            }
            apiService.create("NaacActivity/get_Designation", data).then(function (promise) {
                if (promise.filldesignation.length > 0) {
                    $scope.filldesignation = promise.filldesignation;
                }
                else {
                    swal('For Selected Department, Designation is not Available!');
                }
            });
        }

        //==================get employee
        $scope.get_Employee = function () {
            $scope.usercheckC = false;
            $scope.stafftlist = [];
            var data = {
                "HRMD_Id": $scope.HRMD_Id,
                "HRMDES_Id": $scope.HRMDES_Id,
                "MI_Id": $scope.mI_Id,
            }
            apiService.create("NaacActivity/get_Employee", data).then(function (promise) {
                if (promise.stafftlist.length > 0) {
                    $scope.stafftlist = promise.stafftlist;
                }
                else {
                    swal('For Selected Designation,Staffs are not Available!');
                }
            });
        }

        //======================For Student Count
        $scope.searchchkbx = "";
        $scope.NCACSA343_NoOfStudents = 0;
        $scope.all_check = function () {
            var checkStatus = $scope.usercheck;
            var count = 0;
            angular.forEach($scope.studentlist, function (itm) {
                itm.stud = checkStatus;
                if (itm.stud == true) {
                    count += 1;
                }
                else {
                    count = 0;
                }
            });
            $scope.NCACSA343_NoOfStudents = count;
        }

        //======================For student selection
        $scope.togchkbx = function () {
            $scope.usercheck = $scope.studentlist.every(function (options) {
                return options.stud;
            });
            var count = 0;
            angular.forEach($scope.studentlist, function (item) {
                if (item.stud == true) {
                    count = count + 1;
                }
            })
            $scope.NCACSA343_NoOfStudents = count;
        }

        //======================For student validation
        $scope.isOptionsRequired = function () {
            return !$scope.studentlist.some(function (options) {
                return options.stud;
            });
        }

        //$scope.filterchkbx = function (obj) {
        //    return (angular.lowercase(obj.spccmH_HouseName)).indexOf(angular.lowercase($scope.searchchkbx)) >= 0;
        //}


        //======================For Staff Count
        $scope.searchchkbx1 = "";
        $scope.NCACSA343_NoOfTeachers = 0;
        $scope.usercheckC = false;
        $scope.all_checkC = function () {
            var selectedstaf = $scope.usercheckC;
            var count2 = 0;
            angular.forEach($scope.stafftlist, function (stf) {
                stf.selected = selectedstaf;
                if (stf.selected == true) {
                    count2 += 1;
                }
                else {
                    count2 = 0;
                }
            })
            $scope.NCACSA343_NoOfTeachers = count2;
        }

        //======================For Staff selection
        $scope.togchkbxC = function () {
            $scope.usercheckC = $scope.stafftlist.every(function (options) {
                return options.selected;
            });
            var count2 = 0;
            angular.forEach($scope.stafftlist, function (itm) {
                if (itm.selected == true) {
                    count2 = count2 + 1;
                }
            });
            $scope.NCACSA343_NoOfTeachers = count2;
        }

        //======================For Staff validation
        $scope.isOptionsRequired12 = function () {
            return !$scope.stafftlist.some(function (options) {
                return options.selected;
            });
        }


        //=============================Save records
        $scope.submitted = false;
        $scope.saverecord = function () {
            $scope.selectdStudentlist = [];
            $scope.selectdStafflist = [];
            angular.forEach($scope.studentlist, function (st) {
                if (st.stud) {
                    $scope.selectdStudentlist.push({ AMCST_Id: st.amcsT_Id });
                }
            })
            angular.forEach($scope.stafftlist, function (staf) {
                if (staf.selected) {
                    $scope.selectdStafflist.push({ HRME_Id: staf.hrmE_Id });
                }
            })
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var data = {
                    "NCACSA343_Id": $scope.ncacsA343_Id,
                    "ASMAY_Id": $scope.asmaY_Id,
                    "NCACSA343_OrgAgency": $scope.NCACSA343_OrgAgency,
                    "NCACSA343_Place": $scope.NCACSA343_Place,
                    "NCACSA343_Duration": $scope.NCACSA343_Duration,
                    "NCACSA343_TypeOfActivity": $scope.NCACSA343_TypeOfActivity,
                    selectdStudentlist: $scope.selectdStudentlist,
                    "NCACSA343_ActivityDate": $scope.NCACSA343_ActivityDate,
                    "NCACSA343_NoOfStudents": $scope.NCACSA343_NoOfStudents,
                    selectdStafflist: $scope.selectdStafflist,
                    "NCACSA343_NoOfTeachers": $scope.NCACSA343_NoOfTeachers,
                    "NCACSA343S_Role": $scope.NCACSA343S_Role,
                    "NCACSA343E_Role": $scope.NCACSA343E_Role,
                    filelist: $scope.materaldocuupload,
                    filelist_student: $scope.materaldocuuploadStudent,
                    filelist_staff: $scope.materaldocuuploadStaff,
                    "MI_Id": $scope.mI_Id,
                }
                apiService.create("NaacActivity/saverecord", data).then(function (promise) {
                    if (promise.msg == "saved") {
                        swal("Data Saved Successfully");
                        $state.reload();
                    }
                    else if (promise.msg == "notsaved") {
                        swal("Data Not Saved");
                    }
                    else if (promise.msg == "saved1") {
                        swal("Data Updated Successfully...!!!");
                        $state.reload();
                    }
                    else if (promise.msg == "notsaved1") {
                        swal("Data Not Updated");
                    }
                    else {
                        swal("Something Went Wrong");
                    }
                });
            }
            else {
                $scope.submitted = true;
            }
        }


        //=======================================Edit Data
        $scope.disablecheckbox = false;
        $scope.instit = false;
        $scope.EditData = function (user) {
            debugger;
            var data = {
                "NCACSA343_Id": user.ncacsA343_Id,
                "MI_Id": $scope.mI_Id,
            }
            apiService.create("NaacActivity/EditData", data).then(function (promise) {
                debugger;
                if (promise.editlist.length > 0) {
                    $scope.instit = true;
                    $scope.ncacsA343_Id = promise.editlist[0].ncacsA343_Id;
                    $scope.NCACSA343_ActivityDate = new Date(promise.editlist[0].ncacsA343_ActivityDate);
                    $scope.NCACSA343_OrgAgency = promise.editlist[0].ncacsA343_OrgAgency;
                    $scope.NCACSA343_Place = promise.editlist[0].ncacsA343_Place;
                    $scope.NCACSA343_Duration = promise.editlist[0].ncacsA343_Duration;
                    $scope.NCACSA343_NoOfStudents = promise.editlist[0].ncacsA343_NoOfStudents;
                    $scope.NCACSA343_NoOfTeachers = promise.editlist[0].ncacsA343_NoOfTeachers;
                    $scope.NCACSA343_TypeOfActivity = promise.editlist[0].ncacsA343_TypeOfActivity;
                    $scope.mI_Id = promise.editlist[0].mI_Id;

                    $scope.asmaY_Id = promise.editlist[0].ncacsA343_Year;
                    $scope.courselist = promise.courselist;
                    $scope.editcourse = promise.editcourse;
                    $scope.AMCO_Id = promise.editcourse[0].amcO_Id;
                    $scope.branchlist = promise.branchlist;
                    $scope.editbranch = promise.editbranch;
                    $scope.AMB_Id = promise.editbranch[0].amB_Id;
                    $scope.semisterlist = promise.semisterlist;
                    $scope.editsemeste = promise.editsemeste;
                    $scope.AMSE_Id = promise.editsemeste[0].amsE_Id;
                    $scope.sectionlist = promise.sectionlist;
                    $scope.editsection = promise.editsection;
                    $scope.ACMS_Id = promise.editsection[0].acmS_Id;
                    $scope.studentlist = promise.studentlist;
                    $scope.editstudetdata = promise.editstudetdata;

                    //$scope.disablecheckbox = true;
                    angular.forEach($scope.studentlist, function (mainlist) {
                        angular.forEach($scope.editstudetdata, function (edilist) {
                            if (mainlist.amcsT_Id == edilist.amcsT_Id) {
                                mainlist.stud = true;
                            }
                        })
                    })
                    //$scope.studentRoledata = promise.studentRoledata;
                    $scope.NCACSA343S_Role = promise.studentRoledata[0].ncacsA343S_Role;

                    $scope.empdeptSelectedId = promise.empdeptSelectedId;
                    $scope.HRMD_Id = promise.empdeptSelectedId;
                    $scope.filldesignation = promise.filldesignation;
                    $scope.empDesSelectedId = promise.empDesSelectedId;
                    $scope.HRMDES_Id = promise.empDesSelectedId;
                    $scope.stafftlist = promise.stafftlist;
                    $scope.empdatarole = promise.empdatarole;
                    $scope.NCACSA343E_Role = promise.empdatarole[0].ncacsA343E_Role;

                    angular.forEach($scope.stafftlist, function (allstaf) {
                        angular.forEach($scope.empdatarole, function (selectstaf) {
                            if (allstaf.hrmE_Id === selectstaf.hrmE_Id) {
                                allstaf.selected = true;
                            }
                        })
                    })

                    $scope.materaldocuupload = promise.editMainSActFileslist;
                    if ($scope.materaldocuupload.length > 0) {
                        angular.forEach($scope.materaldocuupload, function (ddd) {
                            var img = ddd.cfilepath;
                            var imagarr = img.split('.');
                            var lastelement = imagarr[imagarr.length - 1];
                            ddd.filetype = lastelement;
                            if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                                ddd.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + ddd.cfilepath;
                            }
                        })
                    }
                    else {
                        $scope.materaldocuupload = [{ id: 'Materal1' }];
                    }

                    $scope.materaldocuuploadStudent = promise.editStudentActFileslist;
                    if ($scope.materaldocuuploadStudent.length > 0) {
                        angular.forEach($scope.materaldocuuploadStudent, function (sss) {
                            var img = sss.cfilepath;
                            var imagarr = img.split('.');
                            var lastelement = imagarr[imagarr.length - 1];
                            sss.filetype = lastelement;
                            if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                                sss.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + sss.cfilepath;
                            }
                        })
                    }
                    else {
                        $scope.materaldocuuploadStudent = [{ id: 'Materal1' }];
                    }

                    $scope.materaldocuuploadStaff = promise.editStaffActFileslist;
                    if ($scope.materaldocuuploadStaff.length > 0) {
                        angular.forEach($scope.materaldocuuploadStaff, function (stt) {
                            var img = stt.cfilepath;
                            var imagarr = img.split('.');
                            var lastelement = imagarr[imagarr.length - 1];
                            stt.filetype = lastelement;
                            if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                                stt.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + stt.cfilepath;
                            }
                        })
                    }
                    else {
                        $scope.materaldocuuploadStaff = [{ id: 'Materal1' }];
                    }
                }
            })
        }
        //============Field Form interacted.
        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        $scope.sort = function (keyname) {
            $scope.propertyName = keyname;   //set the propertyName to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }

        $scope.itemsPerPage = 10;
        $scope.currentPage = 1;
        //=============Clear all Field value from Form
        $scope.cancel = function () {
            $state.reload();
        }
        //============================For Deactive
        $scope.DeactiveY = function (usersem, SweetAlert) {
            var dystring = "";
            if (usersem.ncacsA343_ActiveFlg == true) {
                dystring = "Deactivate";
            }
            else if (usersem.ncacsA343_ActiveFlg == false) {
                dystring = "Activate"
            }
            swal({
                title: "Are you sure?",
                text: "Do You Want To " + dystring + " Record?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + dystring + " it",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("NaacActivity/deactiveStudent", usersem).
                            then(function (promise) {

                                if (promise.ret == true) {
                                    swal("Record " + dystring + "d" + " Successfully!!!");
                                    $state.reload();
                                }
                                else {
                                    swal("Record Not " + dystring + "d " + "Successfully!!!");
                                    $state.reload();
                                }
                            })
                    }
                    else {
                        swal("Record Deactivation Cancelled!!!");
                    }
                });
        }

        //=================================For Modal Student Data
        $scope.get_MappedStudent = function (user) {
            var data = {
                "NCACSA343_Id": user.ncacsA343_Id,
                "MI_Id": $scope.mI_Id,
            }
            apiService.create("NaacActivity/get_MappedStudent", data).then(function (promise) {
                $scope.studentmodaldata = promise.studentmodaldata;
            });
        }

        //=========================For Modal Staff Data
        $scope.get_MappedStaff = function (user) {
            var data = {
                "NCACSA343_Id": user.ncacsA343_Id,
                "MI_Id": $scope.mI_Id,
            }
            apiService.create("NaacActivity/get_MappedStaff", data).then(function (promise) {
                $scope.staffmodaldata = promise.staffmodaldata;
            });
        }

        //================================for Deactive/Active Student Record
        $scope.deactive_student = function (usersem, SweetAlert) {
            var dystring = "";
            if (usersem.NCACSA343S_ActiveFlg == true) {
                dystring = "Deactivate";
            }
            else if (usersem.NCACSA343S_ActiveFlg == false) {
                dystring = "Activate"
            }
            swal({
                title: "Are you sure?",
                text: "Do You Want To " + dystring + " Record?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + dystring + " it",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("NaacActivity/deactive_student", usersem).
                            then(function (promise) {
                                if (promise.returnval == true) {
                                    swal("Record " + dystring + "d" + " Successfully!!!");
                                    $state.reload();
                                }
                                else {
                                    swal("Record Not " + dystring + "d " + "Successfully!!!");
                                    $state.reload();
                                }
                            })
                    }
                    else {
                        swal("Record Deactivation Cancelled!!!");
                    }
                });
        }

        //================================for Deactive/Active Staff Record
        $scope.deactive_staff = function (usersem, SweetAlert) {
            var dystring = "";
            if (usersem.NCACSA343E_ActiveFlg == true) {
                dystring = "Deactivate";
            }
            else if (usersem.NCACSA343E_ActiveFlg == false) {
                dystring = "Activate"
            }
            swal({
                title: "Are you sure?",
                text: "Do You Want To " + dystring + " Record?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + dystring + " it",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("NaacActivity/deactive_staff", usersem).
                            then(function (promise) {
                                if (promise.returnval == true) {
                                    swal("Record " + dystring + "d" + " Successfully!!!");
                                    $state.reload();
                                }
                                else {
                                    swal("Record Not " + dystring + "d" + " Successfully!!!");
                                    $state.reload();
                                }
                            })
                    }
                    else {
                        swal("Record Deactivation Cancelled!!!");
                    }
                });
        }

        //===============================view document files
        $scope.viewdocument_MainActUploadFiles = function (obj) {
            $scope.uploaddocfiles = [];
            $scope.uploadfilesdetails = [];
            $scope.objdata = obj;

            apiService.create("NaacActivity/viewdocument_MainActUploadFiles", obj).then(function (promise) {
                if (promise !== null) {
                 
                    $scope.uploadfilesdetails = promise.viewdocument_MainActUploadFiles;
                    if (promise.viewdocument_MainActUploadFiles !== null && promise.viewdocument_MainActUploadFiles.length > 0) {
                        $scope.uploaddocfiles = promise.viewdocument_MainActUploadFiles;

                        angular.forEach($scope.uploaddocfiles, function (dd) {
                            var img = dd.cfilepath;
                            var imagarr = img.split('.');
                            var lastelement = imagarr[imagarr.length - 1];
                            dd.filetype = lastelement;
                            if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                                dd.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + dd.cfilepath;
                            }
                        });
                    } else {
                        $('#popup11').modal('hide');
                        swal("No Documents Found");
                    }
                }
            });
        };

        //===============================delete document files       
        $scope.delete_MainActUploadFiles = function (docfile) {

            var data = {
                "NCACSA343_Id": docfile.ncacsA343_Id,
                "NCACSA343F_Id": docfile.ncacsA343F_Id,
                "MI_Id": docfile.mI_Id
            }

            swal({
                title: "Are You Sure",
                text: "Do You Want To Delete The Record ?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, Delete It!",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("NaacActivity/delete_MainActUploadFiles", data).then(function (promise) {

                            if (promise.returnval === true) {
                                swal("Record Deleted successfully");
                                //}
                                //else {
                                //    swal("Record Deletion Failed");
                                //}
                                $scope.viewuploadflies = promise.viewdocument_MainActUploadFiles;
                                //$scope.ncaC414BD_Id = promise.viewuploadflies[0].ncaC414BD_Id;
                                $scope.uploadfilesdetails = promise.viewuploadflies;
                                $scope.uploaddocfiles = promise.viewuploadflies;
                                $scope.uploadfilesdetails = promise.viewuploadflies;


                                if (promise.viewdocument_MainActUploadFiles !== null && promise.viewdocument_MainActUploadFiles.length > 0) {
                                    $scope.uploaddocfiles = promise.viewdocument_MainActUploadFiles;

                                    angular.forEach($scope.uploaddocfiles, function (dd) {
                                        var img = dd.cfilepath;
                                        var imagarr = img.split('.');
                                        var lastelement = imagarr[imagarr.length - 1];
                                        dd.filetype = lastelement;
                                        if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                                            dd.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + dd.cfilepath;
                                        }
                                    });
                                }
                                //else {
                                //    $('#popup11').modal('hide');
                                //    swal("No Documents Found");
                                //}
                            }
                            else {
                                swal("Record Deletion Failed");
                            }
                        });






                    }
                    else {
                        swal("Record Deletion Cancelled");
                    }
                });
        };

        //================================================
        //=============================Activity upload
        $scope.materaldocuupload = [{ id: 'Materal1' }];

        $scope.addmateral = function () {
            var newItemNo = $scope.materaldocuupload.length + 1;

            if (newItemNo <= 10) {
                $scope.materaldocuupload.push({ 'id': 'Materal' + newItemNo });
            }
        };

        $scope.removemateral = function (index) {
            var newItemNo = $scope.materaldocuupload.length - 1;
            $scope.materaldocuupload.splice(index, 1);

            if ($scope.materaldocuupload.length === 0) {
                //data
            }
        };
        // Save Function For Materal Guide Upload

        $scope.uploadmateraldocuments1 = [];

        $scope.uploadmateraldocuments = function (input, document) {
            debugger;
            $scope.uploadmateraldocuments1 = input.files;

            $scope.filename = input.files[0].name;

            if (input.files && input.files[0]) {

                if (input.files[0].type === "image/jpeg" || input.files[0].type === "image/png" && input.files[0].size <= 2097152)  // 2097152 bytes = 2MB 
                {
                    UploaddianmateralPhoto(document);
                }
                else if (input.files[0].type === "video/mp4") {
                    UploaddianmateralPhoto(document);
                }
                else if (input.files[0].type === "application/pdf") {
                    UploaddianmateralPhoto(document);
                }
                else if (input.files[0].type === "application/msword") {
                    UploaddianmateralPhoto(document);
                }
                else if (input.files[0].type === "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet") {
                    UploaddianmateralPhoto(document);
                }
                else if (input.files[0].type === "application/vnd.ms-excel") {
                    UploaddianmateralPhoto(document);
                }
                else if (input.files[0].type === "application/vnd.openxmlformats-officedocument.wordprocessingml.document,") {
                    UploaddianmateralPhoto(document);
                }
                else if (input.files[0].type === "application/vnd.openxmlformats-officedocument.wordprocessingml.document") {
                    UploaddianmateralPhoto(document);
                }
                else if (input.files[0].size > 2097152) {
                    swal("size should be less than 2MB");
                    return;
                }
                else {
                    swal("Upload MP4, Pdf, Image Files Only");
                }
            }
        };

        function UploaddianmateralPhoto(data) {
            console.log("Student Upload  :" + data);
            var formData = new FormData();
            for (var i = 0; i <= $scope.uploadmateraldocuments1.length; i++) {
                formData.append("File", $scope.uploadmateraldocuments1[i]);
            }
            var defer = $q.defer();
            $http.post("/api/ImageUpload/Uploadnaacdocuments", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                })
                .success(function (d) {
                    defer.resolve(d);
                    data.cfilepath = d;
                    data.cfilename = $scope.filename;
                    $('#').attr('src', data.cfilepath);
                    var img = data.cfilepath;
                    var imagarr = img.split('.');
                    var lastelement = imagarr[imagarr.length - 1];
                    data.filetype = lastelement;
                    if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                        data.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + data.cfilepath;
                    }
                })
                .error(function () {
                    defer.reject("File Upload Failed!");
                });
        }

        $scope.trustSrc = function (src) {
            return $sce.trustAsResourceUrl(src);
        };

        $scope.showmothersign = function (path) {
            $('#preview').attr('src', path);
            $('img').bind('contextmenu', function (e) {
                return false;
            });
            $('#myModalimg').modal('show');
        };

        $scope.showGuardianPhotonew = function (data) {
            $scope.view_videos = [];
            $scope.videoSources = [];
            $scope.preview1 = data.cfilepath;
            $scope.videdfd = data.cfilepath;
            $scope.movie = { src: data.cfilepath };
            $scope.movie1 = { src: data.cfilepath };
            $scope.view_videos.push({ id: 1, coeeV_Videos: data.cfilepath });
            console.log($scope.view_videos);
        };

        $scope.showpdf = false;
        //  change 
        $scope.onview = function (filepath, filename) {
            var imagedownload = filepath;
            $scope.content = "";
            var fileURL = "";
            var file = "";
            $http.get(imagedownload, { responseType: 'arraybuffer' })
                .success(function (response) {
                    file = new Blob([(response)], { type: 'application/pdf' });
                    fileURL = URL.createObjectURL(file);
                    //  $scope.content = $sce.trustAsResourceUrl(fileURL);
                    var pdfId = document.getElementById("pdfIdzz");
                    pdfId.removeChild(pdfId.childNodes[0]);
                    var embed = document.createElement('embed');
                    embed.setAttribute('src', fileURL);
                    embed.setAttribute('type', 'application/pdf');
                    embed.setAttribute('width', '100%');
                    embed.setAttribute('height', '1000');
                    pdfId.appendChild(embed);
                    $('#showpdf').modal('show');
                });
        };




        //$scope.onview = function (filepath, filename) {
        //    //var myPdfUrl = 'https://bdcampusstrg.blob.core.windows.net/files/6/LessonPlanner/1e55cab1-2e21-4878-802d-ad87b8507e6b.pdf';
        //    var imagedownload = filepath;
        //    $scope.content = "";
        //    var fileURL = "";
        //    var file = "";
        //    $http.get(imagedownload, { responseType: 'arraybuffer' })
        //        .success(function (response) {
        //            file = new Blob([(response)], { type: 'application/pdf' });
        //            fileURL = URL.createObjectURL(file);
        //            $scope.content = $sce.trustAsResourceUrl(fileURL);
        //            $('#showpdf').modal('show');
        //        });
        //};

        //==================Student Activity Upload   
        $scope.materaldocuuploadStudent = [{ id: 'Materal1' }];

        $scope.addmateralstudent = function () {
            var newItemNo = $scope.materaldocuuploadStudent.length + 1;

            if (newItemNo <= 10) {
                $scope.materaldocuuploadStudent.push({ 'id': 'Materal' + newItemNo });
            }
        };

        $scope.removemateralstudent = function (index) {
            var newItemNo = $scope.materaldocuuploadStudent.length - 1;
            $scope.materaldocuuploadStudent.splice(index, 1);

            if ($scope.materaldocuuploadStudent.length === 0) {
                //data
            }
        };

        $scope.showmothersignstudent = function (path) {
            $('#previewstudent').attr('src', path);
            $('img').bind('contextmenu', function (e) {
                return false;
            });
            $('#myModalimgstudent').modal('show');
        };

        $scope.showGuardianPhotonewStudent = function (data) {
            $scope.view_student_videos = [];
            $scope.videoSources = [];
            $scope.preview1 = data.cfilepath;
            $scope.videdfd = data.cfilepath;
            $scope.movie = { src: data.cfilepath };
            $scope.movie1 = { src: data.cfilepath };
            $scope.view_student_videos.push({ id: 1, coeeV_Videos: data.cfilepath });
            console.log($scope.view_student_videos);
        };

        // change 2
        $scope.onviewstudent = function (filepath, filename) {
            var imagedownload = filepath;
            $scope.content = "";
            var fileURL = "";
            var file = "";
            $http.get(imagedownload, { responseType: 'arraybuffer' })
                .success(function (response) {
                    file = new Blob([(response)], { type: 'application/pdf' });
                    fileURL = URL.createObjectURL(file);
                    //  $scope.content = $sce.trustAsResourceUrl(fileURL);
                    var pdfId = document.getElementById("pdfIdzz2");
                    pdfId.removeChild(pdfId.childNodes[0]);
                    var embed = document.createElement('embed');
                    embed.setAttribute('src', fileURL);
                    embed.setAttribute('type', 'application/pdf');
                    embed.setAttribute('width', '100%');
                    embed.setAttribute('height', '1000');
                    pdfId.appendChild(embed);
                    $('#showpdfstudent').modal('show');
                });
        };
        //$scope.onviewstudent = function (filepath, filename) {
        //    //var myPdfUrl = 'https://bdcampusstrg.blob.core.windows.net/files/6/LessonPlanner/1e55cab1-2e21-4878-802d-ad87b8507e6b.pdf';
        //    var imagedownload = filepath;
        //    $scope.contentstudent = "";
        //    var fileURL = "";
        //    var file = "";
        //    $http.get(imagedownload, { responseType: 'arraybuffer' })
        //        .success(function (response) {
        //            file = new Blob([(response)], { type: 'application/pdf' });
        //            fileURL = URL.createObjectURL(file);
        //            $scope.contentstudent = $sce.trustAsResourceUrl(fileURL);
        //            $('#showpdfstudent').modal('show');
        //        });
        //};

        //===============================view Student document files
        $scope.viewdocument_StudentActUploadFiles = function (obj) {
            $scope.uploaddocfiles = [];
            $scope.uploadfilesdetails = [];
            $scope.objdata = obj;

            apiService.create("NaacActivity/viewdocument_StudentActUploadFiles", obj).then(function (promise) {
                if (promise !== null) {
                
                    $scope.uploadfilesdetails = promise.viewdocument_StudentActUploadFiles;
                    if (promise.viewdocument_StudentActUploadFiles !== null && promise.viewdocument_StudentActUploadFiles.length > 0) {
                        $scope.uploaddocfiles = promise.viewdocument_StudentActUploadFiles;

                        angular.forEach($scope.uploaddocfiles, function (dd) {
                            var img = dd.cfilepath;
                            var imagarr = img.split('.');
                            var lastelement = imagarr[imagarr.length - 1];
                            dd.filetype = lastelement;
                            if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                                dd.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + dd.cfilepath;
                            }
                        });
                    } else {
                        $('#popup12').modal('hide');
                        swal("No Documents Found");
                    }
                }
            });
        };

        //===============================delete Student document files
        $scope.delete_StudentActUploadFiles = function (docfile) {

            var data = {
                "NCACSA343SF_Id": docfile.ncacsA343SF_Id,
                "NCACSA343S_Id": docfile.ncacsA343S_Id,
                "MI_Id": docfile.mI_Id
            }

            swal({
                title: "Are You Sure",
                text: "Do You Want To Delete The Record ?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, Delete It!",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("NaacActivity/delete_StudentActUploadFiles", data).then(function (promise) {
                            debugger;
                            if (promise.returnval === true) {
                                swal("Record Deleted successfully");
                                //}
                                //else {
                                //    swal("Record Deletion Failed");
                                //}
                                $scope.viewuploadflies = promise.viewdocument_StudentActUploadFiles;
                                //$scope.ncaC414BD_Id = promise.viewuploadflies[0].ncaC414BD_Id;
                                $scope.uploaddocfiles = promise.viewdocument_StudentActUploadFiles;
                                if ($scope.uploaddocfiles.length > 0) {
                                    $scope.uploaddocfiles = promise.viewdocument_StudentActUploadFiles;

                                    angular.forEach($scope.uploaddocfiles, function (dd) {
                                        var img = dd.cfilepath;
                                        var imagarr = img.split('.');
                                        var lastelement = imagarr[imagarr.length - 1];
                                        dd.filetype = lastelement;
                                        if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                                            dd.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + dd.cfilepath;
                                        }
                                    });
                                }
                                //else {
                                //    $('#popup20').modal('hide');
                                //    swal("No Documents Found");
                                //}


                            }
                            else {
                                swal("Record Deletion Failed");
                            }
                        });
                    }
                    else {
                        swal("Record Deletion Cancelled");
                    }
                });
        };

        //================================================

        //==================Staff Activity Upload   
        $scope.materaldocuuploadStaff = [{ id: 'Materal1' }];

        $scope.addmateralstaff = function () {
            var newItemNo = $scope.materaldocuuploadStaff.length + 1;

            if (newItemNo <= 10) {
                $scope.materaldocuuploadStaff.push({ 'id': 'Materal' + newItemNo });
            }
        };

        $scope.removemateralstaff = function (index) {
            var newItemNo = $scope.materaldocuuploadStaff.length - 1;
            $scope.materaldocuuploadStaff.splice(index, 1);

            if ($scope.materaldocuuploadStaff.length === 0) {
                //data
            }
        };

        $scope.showmothersignstaff = function (path) {
            $('#previewstaff').attr('src', path);
            $('img').bind('contextmenu', function (e) {
                return false;
            });
            $('#myModalimgstaff').modal('show');
        };

        $scope.showGuardianPhotonewstaff = function (data) {
            $scope.view_staff_videos = [];
            $scope.videoSources = [];
            $scope.preview1 = data.cfilepath;
            $scope.videdfd = data.cfilepath;
            $scope.movie = { src: data.cfilepath };
            $scope.movie1 = { src: data.cfilepath };
            $scope.view_staff_videos.push({ id: 1, coeeV_Videos: data.cfilepath });
            console.log($scope.view_staff_videos);
        };
        // change 3

        $scope.onviewstaff = function (filepath, filename) {
            var imagedownload = filepath;
            $scope.content = "";
            var fileURL = "";
            var file = "";
            $http.get(imagedownload, { responseType: 'arraybuffer' })
                .success(function (response) {
                    file = new Blob([(response)], { type: 'application/pdf' });
                    fileURL = URL.createObjectURL(file);
                    //  $scope.content = $sce.trustAsResourceUrl(fileURL);
                    var pdfId = document.getElementById("pdfIdzz3");
                    pdfId.removeChild(pdfId.childNodes[0]);
                    var embed = document.createElement('embed');
                    embed.setAttribute('src', fileURL);
                    embed.setAttribute('type', 'application/pdf');
                    embed.setAttribute('width', '100%');
                    embed.setAttribute('height', '1000');
                    pdfId.appendChild(embed);
                    $('#showpdfstaff').modal('show');
                });
        };

        //$scope.onviewstaff = function (filepath, filename) {
        //    //var myPdfUrl = 'https://bdcampusstrg.blob.core.windows.net/files/6/LessonPlanner/1e55cab1-2e21-4878-802d-ad87b8507e6b.pdf';
        //    var imagedownload = filepath;
        //    $scope.contentstaff = "";
        //    var fileURL = "";
        //    var file = "";
        //    $http.get(imagedownload, { responseType: 'arraybuffer' })
        //        .success(function (response) {
        //            file = new Blob([(response)], { type: 'application/pdf' });
        //            fileURL = URL.createObjectURL(file);
        //            $scope.contentstaff = $sce.trustAsResourceUrl(fileURL);
        //            $('#showpdfstaff').modal('show');
        //        });
        //};

        //===============================view Staff document files
        $scope.viewdocument_StaffActUploadFiles = function (obj) {
            $scope.uploaddocfiles = [];
            $scope.uploadfilesdetails = [];
            $scope.objdata = obj;

            apiService.create("NaacActivity/viewdocument_StaffActUploadFiles", obj).then(function (promise) {
                if (promise !== null) {
                  
                    $scope.uploadfilesdetails = promise.viewdocument_StaffActUploadFiles;
                    if (promise.viewdocument_StaffActUploadFiles !== null && promise.viewdocument_StaffActUploadFiles.length > 0) {
                        $scope.uploaddocfiles = promise.viewdocument_StaffActUploadFiles;

                        angular.forEach($scope.uploaddocfiles, function (dd) {
                            var img = dd.cfilepath;
                            var imagarr = img.split('.');
                            var lastelement = imagarr[imagarr.length - 1];
                            dd.filetype = lastelement;
                            if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                                dd.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + dd.cfilepath;
                            }
                        });
                    } else {
                        $('#popup20').modal('hide');
                        swal("No Documents Found");
                    }
                }
            });
        };

        //===============================delete Staff document files       
        $scope.delete_StaffActUploadFiles = function (docfile) {

            var data = {
                "NCACSA343EF_Id": docfile.ncacsA343EF_Id,
                "NCACSA343E_Id": docfile.ncacsA343E_Id,
                "MI_Id": docfile.mI_Id
            }

            swal({
                title: "Are You Sure",
                text: "Do You Want To Delete The Record ?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, Delete It!",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("NaacActivity/delete_StaffActUploadFiles", data).then(function (promise) {
                            debugger;
                            if (promise.returnval === true) {
                                swal("Record Deleted successfully");
                                //}
                                //else {
                                //    swal("Record Deletion Failed");
                                //}
                                //$scope.viewuploadflies = promise.viewdocument_StaffActUploadFiles;
                                //$scope.ncaC414BD_Id = promise.viewuploadflies[0].ncaC414BD_Id;
                                $scope.uploaddocfiles = promise.viewdocument_StaffActUploadFiles;
                                if ($scope.uploaddocfiles.length > 0) {
                                    $scope.uploaddocfiles = promise.viewdocument_StaffActUploadFiles;

                                    angular.forEach($scope.uploaddocfiles, function (dd) {
                                        var img = dd.cfilepath;
                                        var imagarr = img.split('.');
                                        var lastelement = imagarr[imagarr.length - 1];
                                        dd.filetype = lastelement;
                                        if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                                            dd.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + dd.cfilepath;
                                        }
                                    });
                                }
                                //else {
                                //    $('#popup20').modal('hide');
                                //    swal("No Documents Found");
                                //}


                            }
                            else {
                                swal("Record Deletion Failed");
                            }
                        });
                    }
                    else {
                        swal("Record Deletion Cancelled");
                    }
                });
        };


        $scope.change_institution = function () {
          
            $scope.ncacsA343_Id = 0;
            $scope.asmaY_Id = '';
            $scope.NCACSA343S_Role = '';
            $scope.NCACSA343_TypeOfActivity = '';
            $scope.AMCO_Id = '';
            $scope.AMB_Id = '';
            $scope.AMSE_Id = '';
            $scope.ACMS_Id = '';
            $scope.NCACSA343_NoOfStudents = '';
            $scope.NCACSA343_OrgAgency = '';
            $scope.NCACSA343_Place = '';
            $scope.NCACSA343_Duration = '';
            $scope.NCACSA343_ActivityDate = '';
            $scope.NCACSA343E_Role = '';
            $scope.HRMD_Id = '';
            $scope.HRMDES_Id = '';
            $scope.NCACSA343_NoOfTeachers = '';
            $scope.courselist = [];
            $scope.branchlist = [];
            $scope.semisterlist = [];
            $scope.sectionlist = [];
            $scope.filldesignation = [];

           

           
            $scope.usercheckC = '';
            $scope.materaldocuuploadStaff = [];
            $scope.materaldocuuploadStudent = [];
            $scope.materaldocuupload = [];
            $scope.stafftlist = [];
            $scope.studentlist = [];
            $scope.materaldocuuploadStaff = [{ id: 'Materal1' }];
            $scope.materaldocuuploadStudent = [{ id: 'Materal1' }];
            $scope.materaldocuupload = [{ id: 'Materal1' }];

            angular.forEach($scope.studentlist, function (tt) {
                tt.selected = false;
            })
            
            angular.forEach($scope.stafftlist, function (tt) {
                tt.selected = false;
            })
        }
        //================================================

    }

    angular.module('app').filter("trustUrl", function ($sce) {
        return function (Url) { return $sce.trustAsResourceUrl(Url); };
    });

    angular.module('app').directive('txtArea', function () {
        return {
            restrict: 'AE',
            replace: 'true',
            scope: { data: '=', model: '=ngModel' },
            template: "<textarea></textarea>",
            link: function (scope, elem) {
                scope.$watch('data', function (newVal) {
                    if (newVal) {
                        scope.model += newVal[0];
                    }
                });
            }
        };
    });

})();