
(function () {
    'use strict';
    angular
        .module('app')
        .controller('ProgramDetailsController', ProgramDetailsController)

    ProgramDetailsController.$inject = ['$rootScope', '$scope', '$state', '$location', 'apiService', 'Flash', '$http', '$q', '$stateParams', '$filter', '$sce']
    function ProgramDetailsController($rootScope, $scope, $state, $location, apiService, Flash, $http, $q, $stateParams, $filter, $sce) {
        $scope.editEmployee = {};
      
        $scope.hstep = 1;
        $scope.mstep = 1;

        $scope.ismeridian = true;
        $scope.toggleMode = function () {
            $scope.ismeridian = !$scope.ismeridian;
        };

        //Ui Grid view rendering data from data base
      

        $scope.gridOptions2 = {
            enableColumnMenus: false,
            enableFiltering: true,
            paginationPageSizes: [5, 10, 15],
            paginationPageSize: 5,

            columnDefs: [
                { name: 'SNO', width: '5%', enableFiltering: false, field: 'name', cellTemplate: '<div class="ui-grid-cell-contents">{{grid.options.paginationPageSize *(grid.options.paginationCurrentPage-1)+grid.renderContainers.body.visibleRowCache.indexOf(row)+1}}</div>' },
                { name: 'PRYR_ProgramName', width: '15%', displayName: 'Program Name' },
                {
                    name: 'images', displayName: 'Images', enableFiltering: false, enableSorting: false, cellTemplate: '<div class="grid-action-cell">' +
                        '<a   href="javascript:void(0)" data-toggle="modal" data-target="#popup11" data-backdrop="static" ng-click="grid.appScope.viewrecordspopup2(row.entity);"> <i class="fa fa-picture-o text-green" ></i></a>  &nbsp; &nbsp;' + '</div>'
                },//fa-file-image-o,fa-film,fa-video-camera,fa-file-video-o,fa-picture-o
                {
                    name: 'videos', displayName: 'Videos', enableFiltering: false, enableSorting: false, cellTemplate: '<div class="grid-action-cell">' +
                        '<a  href="javascript:void(0)" data-toggle="modal" data-target="#popup15" data-backdrop="static" ng-click="grid.appScope.viewrecordspopup2(row.entity);"> <i class="fa fa-film text-blue" ></i></a>  &nbsp; &nbsp;' + '</div>'
                },
                
                {
                    field: 'id', name: '',
                    displayName: 'Actions', enableFiltering: false, enableSorting: false, cellTemplate:
                     
                        '<div class="grid-action-cell">' +
                        //'<a  href="javascript:void(0)" data-toggle="modal" data-target="#popup2" data-backdrop="static" ng-click="grid.appScope.viewrecordspopup2(row.entity);"> <i class="fa fa-eye text-purple" ></i></a>  &nbsp; &nbsp;' +
                        '<a href="javascript:void(0)" ng-click="grid.appScope.getorgvalue2(row.entity);"> <md-tooltip md-direction="down">Edit Now</md-tooltip> <i class="fa fa-pencil-square-o" ></i></a>  &nbsp; &nbsp;' +
                        '<a ng-if="row.entity.coeE_ActiveFlag === false" href="javascript:void(0)" style="color:green;" ng-click="grid.appScope.switch2(row.entity);"> <md-tooltip md-direction="down">Activate Now</md-tooltip> <i class="fa fa-toggle-off text-red" aria-hidden="true"></i></a>' +
                        '<span ng-if="row.entity.coeE_ActiveFlag === true"><a href="javascript:void(0)"  style="color:red;" ng-click="grid.appScope.switch2(row.entity);">  <md-tooltip md-direction="down">Deactivate Now</md-tooltip> <i class="fa fa-toggle-on text-green"  aria-hidden="true"></i></a><span>' +

                        '</div>'
                }
            ]

        };



        // TO Save The Data
        $scope.submitted1 = false;
     
        $scope.coeE_EStartDate = "";
     
        $scope.submitted2 = false;
        $scope.savemappedeventsdata = function () {
            
            if ($scope.myForm2.$valid) {
                $scope.albumNameArray1 = [];
                $scope.albumNameArray2 = [];
                
                if ($scope.images_paths == null || $scope.images_paths == undefined || $scope.images_paths == "") {
                    $scope.images_paths = [];
                }
                if ($scope.videos_paths == null || $scope.videos_paths == undefined || $scope.videos_paths == "") {
                    $scope.videos_paths = [];
                }
                var data = {
                    "PRYR_Id": $scope.pryR_Id,
                    images_list: $scope.images_paths,
                    videos_list: $scope.videos_paths,
                }
                apiService.create("ProgramDetails/savedetail2", data).
                    then(function (promise) {

                        if (promise.returnval == true) {
                            swal('Data successfully Saved');
                            // $state.reload();
                        }
                        else if (promise.returnduplicatestatus == 'Duplicate') {
                            swal('Records AlReady Exist !');
                            //$state.reload();
                        }
                        else if (promise.returnval == false) {
                            swal('Data Not Saved !');
                            //$state.reload();
                        }
                    
                    })
                $scope.images_paths = [];
                $scope.videos_paths = [];
                $scope.loadData();
                $scope.clear2();
            }
            else {
                $scope.submitted2 = true;

            }

        };

        //TO  GEt The Values iN Grid
        $scope.loadData = function () {

            $scope.usercheck = true;
            $scope.usercheck1 = true;
            $scope.files_flag = false;
            $scope.parameter_list = [];
            var pageid = 2;
            //$scope.all_check();
            //$scope.all_check1();
            apiService.getURI("ProgramDetails/getloaddata",pageid).
                then(function (promise) {

                    $scope.currentPage = 1;
                    $scope.itemsPerPage = 5;
                    $scope.programlist = promise.programlist;
                   // $scope.alllist = promise.alllist;
                    $scope.gridOptions2.data = promise.alllist;
                })
        };
        $scope.togchkbx = function () {

            $scope.usercheck = $scope.class_list.every(function (options) {
                return options.class;
            });
        }
        $scope.togchkbx1 = function () {
            $scope.usercheck1 = $scope.staff_type_list.every(function (options) {
                return options.sec;
            });
        }
        $scope.all_check = function () {

            var toggleStatus = $scope.usercheck;
            angular.forEach($scope.class_list, function (itm) {
                itm.class = toggleStatus;
            });
        }



        $scope.all_check1 = function () {
            var toggleStatus = $scope.usercheck1;
            angular.forEach($scope.staff_type_list, function (itm) {
                itm.sec = toggleStatus;
            });
        }
        $scope.isOptionsRequired = function () {
            if ($scope.stud_chk == "1") {
                return !$scope.class_list.some(function (options) {
                    return options.class;
                });
            }
            else if ($scope.stud_chk == "0") {
                return false;
            }

        }

        $scope.isOptionsRequired1 = function () {
            if ($scope.stf_chk == "1") {
                return !$scope.staff_type_list.some(function (options) {
                    return options.sec;
                });
            }
            else if ($scope.stf_chk == "0") {
                return false;
            }

        }
        

        $scope.coemE_Id_f = "";
        $scope.evt_flg = true;
        $scope.get_event_details = function () {

            if ($scope.coemE_Id_f == "" || $scope.coemE_Id_f == undefined) {
                swal("Please Select Valid Event Name !!!");
                $scope.evt_flg = true;
                $scope.coeE_EventDesc = "";
                $scope.coeE_SMSMessage = "";
                $scope.coeE_MailSubject = "";
                $scope.coeE_MailHeader = "";
                $scope.coeE_MailFooter = "";
                $scope.coeE_Mail_Message = "";
                $scope.coeE_MailHTMLTemplate = "";
            }
            else {
                var data = {

                    "COEME_Id": $scope.coemE_Id_f,

                }
                apiService.create("ProgramDetails/geteventdetails", data).
                    then(function (promise) {
                        //$scope.COEME_Id=promise.
                        //$scope.coeE_EventName = promise.selected_master_event[0].coemE_EventName;
                        $scope.evt_flg = false;
                        $scope.coeE_EventDesc = promise.selected_master_event[0].coemE_EventDesc;
                        $scope.coeE_SMSMessage = promise.selected_master_event[0].coemE_SMSMessage;
                        $scope.coeE_MailSubject = promise.selected_master_event[0].coemE_MailSubject;
                        $scope.coeE_MailHeader = promise.selected_master_event[0].coemE_MailHeader;
                        $scope.coeE_MailFooter = promise.selected_master_event[0].coemE_MailFooter;
                        $scope.coeE_Mail_Message = promise.selected_master_event[0].coemE_Mail_Message;
                        $scope.coeE_MailHTMLTemplate = promise.selected_master_event[0].coemE_MailHTMLTemplate;
                    })

            }

        }





        $scope.coemE_SMSMessage = "";
        $scope.coemE_Mail_Message = "";
        //TO clear  data
        $scope.clear1 = function () {
            $scope.COEME_Id = 0;
            $scope.coemE_EventName = "";
            $scope.coemE_EventDesc = "";
            $scope.coemE_SMSMessage = "";
            $scope.coemE_MailSubject = "";
            $scope.coemE_MailHeader = "";
            $scope.coemE_MailFooter = "";
            $scope.coemE_Mail_Message = "";
            $scope.coemE_MailHTMLTemplate = "";
            $scope.submitted1 = false;
            $scope.myForm1.$setPristine();
            $scope.myForm1.$setUntouched();
            $scope.search = "";
        };

        $scope.clear2 = function () {
            $scope.files_flag = false;
            $scope.PRYR_Id = 0;
            $scope.usercheck = true;
            $scope.usercheck1 = true;
            $scope.all_check();
            $scope.all_check1();

            $scope.asmaY_Id = "";
            $scope.coemE_Id_f = "";
      
            $scope.evt_flg = true;

            $scope.coeE_EventDesc = "";
            $scope.coeE_MailHTMLTemplate = "";
            $scope.coeE_SMSMessage = "";
            $scope.coeE_MailSubject = "";
            $scope.coeE_MailHeader = "";
            $scope.coeE_MailFooter = "";
            $scope.coeE_Mail_Message = "";
            $scope.coeE_EStartDate = "";
         
            $scope.coeE_EEndDate = "";
            $scope.reminder_days = "";
            $scope.coeE_ReminderDate = "";
            $scope.coeE_EStartTime = "";
            $scope.coeE_EEndTime = "";
            $scope.email_flag = "No";
            $scope.sms_flag = "No";
            $scope.repeate_flag = "No";
            $scope.coeE_ReminderSchedule = "";
            $scope.stud_chk = "0";
            $scope.stf_chk = "0";
            $scope.oldstud_chk = "0";
            $scope.oth_chk = "0";
            $scope.asmaY_Id_o_s = "";
            $scope.images_temp = [];
            $scope.videos_temp = [];
            $scope.mobilenos_temp = [];
            $scope.Others_list = [];
            $scope.mobilecount = 0;
            $scope.c1 = false;
            $scope.c2 = false;
            $scope.c3 = false;
            $scope.c4 = false;
      
            $scope.submitted2 = false;
            $scope.myForm2.$setPristine();
            $scope.myForm2.$setUntouched();
            $scope.search = "";
        };




        //TO clear  data
        $scope.clearid = function () {
            $scope.submitted = false;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
            $scope.search = "";
            $scope.asmaY_Id = "";
            $scope.TTMSAB_Id = 0;
            $scope.Staff_Id = "";
            $scope.StaffAbbreviation = "";

        };

        //TO  Edit Record
         $scope.getorgvalue2 = function (employee) {
            $scope.editEmployee = employee.PRYR_Id;
            var pageid = $scope.editEmployee;
            apiService.getURI("ProgramDetails/getdetails2", pageid).
                then(function (promise) {
                    $scope.PRYR_Id = promise.edit_map_event[0].PRYR_Id;
                    $scope.asmaY_Id = promise.edit_map_event[0].asmaY_Id,
                        $scope.coemE_Id_f = promise.edit_map_event[0].coemE_Id;
                    $scope.evt_flg = false;
                    $scope.coeE_EventDesc = promise.edit_map_event[0].coeE_EventDesc;
                    $scope.coeE_SMSMessage = promise.edit_map_event[0].coeE_SMSMessage;
                    $scope.coeE_MailSubject = promise.edit_map_event[0].coeE_MailSubject;
                    $scope.coeE_MailHeader = promise.edit_map_event[0].coeE_MailHeader;
                    $scope.coeE_MailFooter = promise.edit_map_event[0].coeE_MailFooter;
                    $scope.coeE_Mail_Message = promise.edit_map_event[0].coeE_Mail_Message;
                    $scope.coeE_MailHTMLTemplate = promise.edit_map_event[0].coeE_MailHTMLTemplate;

                    $scope.coeE_EStartDate = new Date(promise.edit_map_event[0].coeE_EStartDate);
                    var tdyDate = new Date();
                    tdyDate.setHours(0, 0, 0, 0)
                    if (tdyDate > new Date($scope.coeE_EStartDate)) {
                        swal("Selected Event Start Date Is Before the Today's Date,So U Need To Change");
                        $scope.coeE_EStartDate = "";
                        $scope.coeE_EEndDate = "";
                        $scope.coeE_ReminderDate = "";
                        $scope.reminder_days = "";
                        $scope.coeE_EStartTime = "";
                        $scope.coeE_EEndTime = "";
                    }
                    else {
                        $scope.coeE_EEndDate = new Date(promise.edit_map_event[0].coeE_EEndDate);
                        $scope.coeE_EStartTime = moment(promise.edit_map_event[0].coeE_EStartTime, 'h:mm a').format();
                        $scope.coeE_EEndTime = moment(promise.edit_map_event[0].coeE_EEndTime, 'h:mm a').format();
                        $scope.coeE_ReminderDate = new Date(promise.edit_map_event[0].coeE_ReminderDate);
                        
                        if (tdyDate.getDate() > new Date($scope.coeE_ReminderDate).getDate()) {
                            swal("Selected Event Reminder Date Is Before the Today's Date,So U Need To Change");
                            $scope.coeE_ReminderDate = "";
                            $scope.reminder_days = "";
                        }
                        else {
                            //BY HL
                            var firstDate1 = new Date($scope.coeE_ReminderDate);
                            var secondDate1 = new Date($scope.coeE_EStartDate);
                            //var firstDate2 = new Date();
                            firstDate1 = $filter('date')(firstDate1, "dd/MM/yy");
                            secondDate1 = $filter('date')(secondDate1, "dd/MM/yy");
                            var date2 = new Date($scope.formatString(secondDate1));
                            var date1 = new Date($scope.formatString(firstDate1));
                            var timeDiff1 = Math.abs(date2.getTime() - date1.getTime());
                            var diffDays1;
                            diffDays1 = Math.ceil(timeDiff1 / (1000 * 3600 * 24));
                            //HL
                            $scope.reminder_days = diffDays1;
                        }
                    }
                 
                    $scope.coeE_MailActiveFlag = promise.edit_map_event[0].coeE_MailActiveFlag;
                    $scope.coeE_SMSActiveFlag = promise.edit_map_event[0].coeE_SMSActiveFlag;
                   
                    $scope.coeE_ReminderSchedule = promise.edit_map_event[0].coeE_ReminderSchedule;
                    $scope.coeE_StudentFlag = promise.edit_map_event[0].coeE_StudentFlag;
                    $scope.coeE_AlumniFlag = promise.edit_map_event[0].coeE_AlumniFlag;
                    $scope.coeE_EmployeeFlag = promise.edit_map_event[0].coeE_EmployeeFlag;
                    $scope.coeE_ManagementFlag = promise.edit_map_event[0].coeE_ManagementFlag;
                    $scope.coeE_OtherFlag = promise.edit_map_event[0].coeE_OtherFlag;


                    if ($scope.coeE_StudentFlag == true) {
                        $scope.stud_chk = "1";
                        $scope.c1 = true;
                        $scope.usercheck = false;
                        angular.forEach($scope.class_list, function (role) {
                           
                            role.class = false;
                        })
                    }
                    else if ($scope.coeE_StudentFlag == false) {
                        $scope.stud_chk = "0";
                        $scope.c1 = false;
                    }

                    if ($scope.coeE_AlumniFlag == true) {
                        $scope.oldstud_chk = "1";
                        $scope.c2 = true;
                    }
                    else if ($scope.coeE_AlumniFlag == false) {
                        $scope.oldstud_chk = "0";
                        $scope.c2 = false;
                    }

                    if ($scope.coeE_EmployeeFlag == true) {
                        $scope.stf_chk = "1";
                        $scope.c3 = true;
                        $scope.usercheck1 = false;
                        angular.forEach($scope.staff_type_list, function (role) {
                            role.sec = false;
                           
                        })
                    }
                    else if ($scope.coeE_EmployeeFlag == false) {
                        $scope.stf_chk = "0";
                        $scope.c3 = false;
                    }

                    if ($scope.coeE_OtherFlag == true) {
                        $scope.oth_chk = "1";
                        $scope.c4 = true;
                    }
                    else if ($scope.coeE_OtherFlag == false) {
                        $scope.oth_chk = "0";
                        $scope.c4 = false;
                    }
                    $scope.files_flag = true;
                 
                    for (var c = 0; c < promise.edit_stu_class_list.length; c++) {
                        angular.forEach($scope.class_list, function (role) {

                            if (role.asmcL_Id == promise.edit_stu_class_list[c].asmcL_Id) {
                                role.class = true;
                            }

                        })
                    }
                    $scope.togchkbx();
                    for (var e = 0; e < promise.edit_emp_type_list.length; e++) {
                        angular.forEach($scope.staff_type_list, function (role) {
                            if (role.hrmD_Id == promise.edit_emp_type_list[e].hrmD_Id)
                                role.sec = true;
                        })
                    }
                    $scope.togchkbx1();
                    $scope.Others_list = [];
                    angular.forEach(promise.edit_oth_mobilenos_list, function (role) {
                        $scope.Others_list.push({ COEEO_MobileNo: role.coeeO_MobileNo, COEEO_Emailid: role.coeeO_Emailid, COEEO_Name: role.coeeO_Name });
                    })
                    $scope.mobilecount = $scope.Others_list.length;
                    $scope.Oters_Name = "";
                    $scope.Oters_Mob_No = "";
                    $scope.Oters_Mail_Id = "";
                   

                })
        }
        //TO  delete Record
        $scope.deletedata = function (employee, SweetAlert) {
            $scope.editEmployee = employee.ttmsaB_Id;
            var pageid = $scope.editEmployee;
            swal({
                title: "Are you sure",
                text: "Do you want to delete record????",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, delete it!",
                cancelButtonText: "Cancel!!!!!!",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.DeleteURI("ProgramDetails/deletepages", pageid).
                            then(function (promise) {
                                if (promise.returnval === true) {
                                    swal('Record Deleted Successfully');
                                }
                                else {
                                    swal('Record Not Deleted Successfully!');
                                }
                                $scope.BindData();
                            })
                        $scope.BindData();
                    }
                    else {
                        swal("Record Deletion Cancelled");
                        $scope.BindData();
                    }
                });
        };

      

        //TO  View Record
        $scope.viewrecordspopup2 = function (employee, SweetAlert) {

            $scope.editEmployee = employee.PRYR_Id;
            var pageid = $scope.editEmployee;

            apiService.getURI("ProgramDetails/getalldetailsviewrecords", pageid).
                then(function (promise) {

                    //$scope.Event_Name = promise.edit_m_event[0].coemE_EventName;
                    //$scope.viewrecordspopupdisplay2 = promise.edit_map_event;
                    $scope.view_imgs = promise.photolist;
                    $scope.view_videos = promise.videolist;
                    //$scope.view_classes = promise.edit_stu_class_list;
                    //$scope.view_employees = promise.edit_emp_type_list;
                    //$scope.view_others = promise.edit_oth_mobilenos_list;
                    //$scope.xyz = promise.edit_videos_list[0].coeeV_Videos.toString();
                })

        };

        //TO clear  popupgrid data
        $scope.clearpopupgrid1 = function () {
            $scope.viewrecordspopupdisplay1 = "";
        };
        $scope.clearpopupgrid2 = function () {
            $scope.viewrecordspopupdisplay2 = "";
        };
        $scope.clearpopupgrid11 = function () {
            $scope.view_imgs = "";
        };
        $scope.clearpopupgrid15 = function () {
            $scope.view_videos = "";
        };
        $scope.clearpopupgrid12 = function () {
            $scope.view_classes = "";
        };
        $scope.clearpopupgrid13 = function () {
            $scope.view_employees = "";
        };
        $scope.clearpopupgrid14 = function () {
            $scope.view_others = "";
        };



        $scope.interacted1 = function (field) {

            return $scope.submitted1 || field.$dirty;
        };
        $scope.interacted2 = function (field) {

            return $scope.submitted2 || field.$dirty;
        };

        //active switch
        $scope.switch1 = function (employee, SweetAlert) {

            var mgs = "";
            var confirmmgs = "";
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            if (employee.coemE_ActiveFlag === true) {
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

                        apiService.create("ProgramDetails/deactivate1", employee).
                            then(function (promise) {
                                if (promise.returnval == true) {
                                    swal(confirmmgs + " " + "successfully.");
                                }
                                else {
                                    swal(confirmmgs + " " + " successfully");
                                }
                                $scope.loadData();
                            })

                    }
                    else {
                        swal("Record " + mgs + " Cancelled");
                    }
                });
        }

        //active switch
        $scope.switch2 = function (employee, SweetAlert) {

            var mgs = "";
            var confirmmgs = "";
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            if (employee.coeE_ActiveFlag === true) {
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

                        apiService.create("ProgramDetails/deactivate2", employee).
                            then(function (promise) {
                                if (promise.returnval == true) {
                                    swal(confirmmgs + " " + "successfully.");
                                }
                                else {
                                    swal(confirmmgs + " " + " successfully");
                                }
                                $scope.loadData();
                            })

                    }
                    else {
                        swal("Record " + mgs + " Cancelled");
                    }
                });
        }



        $scope.minDatemf = new Date();
     

        $scope.changed = function (timedata) {
            $scope.coeE_EEndTime = "";
            $scope.totime = timedata;
            var hh = $scope.totime.getHours();
            var mm = $scope.totime.getMinutes();
            $scope.min = timedata;

            $scope.min.setMinutes(hh);
            $scope.min.setMinutes(mm);
        }

        $scope.asmaY_Id = "";
        // for not Based on AM AND PM  HL
        $scope.get_end_remind1 = function (secondDate1) {

            if ($scope.asmaY_Id != "") {
                var firstDate1 = new Date();
                firstDate1 = $filter('date')(firstDate1, "dd/MM/yy");
                secondDate1 = $filter('date')(secondDate1, "dd/MM/yy");
                var date2 = new Date($scope.formatString(secondDate1));
                var date1 = new Date($scope.formatString(firstDate1));
                var timeDiff = Math.abs(date2.getTime() - date1.getTime());
                $scope.dayDifference = Math.ceil(timeDiff / (1000 * 3600 * 24));
                // alert($scope.dayDifference);

                var someDate = new Date();
                // var numberOfDaysToAdd = 6;
                someDate.setDate(someDate.getDate() + $scope.dayDifference);
                swal("Difference Days :" + $scope.dayDifference + " and Date:" + someDate.toDateString());
            }
            else {
                swal("First Select Academic Year !!!");
                $scope.coeE_EStartDate = "";
            }
            $scope.coeE_EEndDate = "";
        }
        $scope.formatString = function (format) {

            var day = parseInt(format.substring(0, 2));
            var month = parseInt(format.substring(3, 5));
            var year = parseInt(format.substring(6, 10));
            var date = new Date(year, month - 1, day);
            return date;
        }

        $scope.get_date1 = function (days1, secondDate1) {

            var date123 = secondDate1;
            if (days1 != "" && days1 != undefined) {
                days1 = Number(days1);
                var firstDate1 = new Date();
                firstDate1 = $filter('date')(firstDate1, "dd/MM/yy");
                secondDate1 = $filter('date')(secondDate1, "dd/MM/yy");
                var date2 = new Date($scope.formatString(secondDate1));
                var date1 = new Date($scope.formatString(firstDate1));
                var timeDiff = Math.abs(date2.getTime() - date1.getTime());
                $scope.dayDifference = Math.ceil(timeDiff / (1000 * 3600 * 24));

                if (days1 <= $scope.dayDifference) {

                    var someDate = date123;
                    var remind_date = new Date(someDate);
                    remind_date.setDate(remind_date.getDate() - days1);
                    $scope.coeE_ReminderDate = remind_date;
                }
                else {
                    swal("Reminder Date Can't be Before  The Today's Date !!!");
                    $scope.coeE_ReminderDate = "";
                    $scope.reminder_days = "";
                }
            }
            else {
                $scope.coeE_ReminderDate = "";
            }
        }
        //HL

        // for Based on AM AND PM considers MB
        $scope.get_end_remind = function (secondDate) {

            if ($scope.asmaY_Id != "") {
                var firstDate = new Date();
                var diffDays;
                // $scope.calcDiff = function (firstDate, secondDate) {
                var oneDay = 24 * 60 * 60 * 1000; // hours*minutes*seconds*milliseconds    
                if (firstDate.toDateString() == secondDate.toDateString()) {
                    diffDays = 0;
                }
                else {
                    diffDays = moment(secondDate).startOf('day').diff(moment(firstDate).startOf('day'), 'days');
                    diffDays = Math.round(Math.abs((firstDate.getTime() - secondDate.getTime()) / (oneDay)));
                    // diffDays += 1;
                }
                //}
                var someDate = new Date();
                // var numberOfDaysToAdd = 6;
                someDate.setDate(someDate.getDate() + diffDays);
                $scope.coeE_EEndDate = "";
                $scope.coeE_ReminderDate = "";
                $scope.reminder_days = "";
                //$scope.coeE_EEndDate = "";
                swal("Difference Days :" + diffDays + " and Date:" + someDate.toDateString());
            }
            else {
                swal("First Select Academic Year !!!");
                $scope.coeE_EStartDate = "";
            }
        }
        $scope.get_date = function (days, secondDate) {

            if (days != "" && days != undefined) {
                days = Number(days);
                var firstDate = new Date();
                var diffDays;
                var oneDay = 24 * 60 * 60 * 1000; // hours*minutes*seconds*milliseconds    
                if (firstDate.toDateString() == secondDate.toDateString()) {
                    diffDays = 0;
                }
                else {
                    diffDays = Math.round(Math.abs((firstDate.getTime() - secondDate.getTime()) / (oneDay)));
                    // diffDays += 1;
                }
                if (days <= diffDays) {
                    
                    var someDate = secondDate;
                    var remind_date = new Date(someDate);
                    remind_date.setDate(remind_date.getDate() - days);
                    // var remind_date=someDate.setDate(someDate.getDate() - days).toDate();
                    $scope.coeE_ReminderDate = remind_date;
                }
                else {
                    //swal("Reminder Days Can't Exceed The Difference of Start Date And Today's Date !!!");
                    swal("Reminder Date Can't be Before  The Today's Date !!!");
                    $scope.coeE_ReminderDate = "";
                    $scope.reminder_days = "";
                }
            }
            else {
                $scope.coeE_ReminderDate = "";
            }
        }
        //MB

        $scope.UploadStudentProfilePic = [];
        $scope.images_temp = [];

        $scope.uploadStudentProfilePic = function (input, document) {

            $scope.UploadStudentProfilePic = input.files;

            for (var a = 0; a < $scope.UploadStudentProfilePic.length; a++) {

                if (input.files && input.files[a]) {

                    if ((input.files[a].type == "image/jpeg" || input.files[a].type == "image/png") && input.files[a].size <= 2097152)  // 2097152 bytes = 2MB || input.files[a].type == "image/png"   video/mp4,video/3gpp
                    {

                       
                        var count = 0;
                        angular.forEach($scope.images_temp, function (imgt123) {
                            if (imgt123.name == input.files[a].name) {
                                count += 1;
                            }
                        });

                        if (count == 0) {
                            $scope.images_temp.push(input.files[a]);
                        }
                        var reader = new FileReader();
                        // var id = input.files[a].name;
                        reader.onload = function (e) {


                            $('#blah')
                                .attr('src', e.target.result)

                        };
                        reader.readAsDataURL(input.files[a]);
                        //$scope.images_temp.push(input.files[a]);
                    }
                    else if (input.files[a].type != "image/jpeg") {
                        swal("Please Upload the JPEG file");
                        return;
                    } else if (input.files[a].size > 2097152) {
                        swal("Image size should be less than 2MB");
                        return;
                    }
                }

            }
           
        }

        $scope.remove_img = function (sel_img_del) {

            for (var i = 0; i < $scope.images_temp.length; i++) {

                var imgt123 = $scope.images_temp[i];
                if (imgt123.name == sel_img_del.name) {

                    $scope.images_temp.splice(i, 1);
                }
            }
          


        }


        $scope.UploadStudentProfileVideo = [];
        $scope.videos_temp = [];

        $scope.uploadStudentProfileVideo = function (input, document) {

            $scope.UploadStudentProfileVideo = input.files;

            for (var a = 0; a < $scope.UploadStudentProfileVideo.length; a++) {

                if (input.files && input.files[a]) {

                    if ((input.files[a].type == "video/mp4") && input.files[a].size <= 27262976)  // 2097152 bytes = 2MB || input.files[a].type == "image/png"   video/mp4,video/3gpp,video/x-ms-wmv 4194304  video/x-ms-wmv 27262976  || input.files[a].type == "video/x-ms-wmv"
                    {

                        var reader = new FileReader();

                        reader.onload = function (e) {

                            $('#blah')
                                .attr('src', e.target.result)
                        };
                        reader.readAsDataURL(input.files[a]);
                        //  Uploadprofile();
                        var count = 0;
                        angular.forEach($scope.videos_temp, function (vid123) {
                            if (vid123.name == input.files[a].name) {
                                count += 1;
                            }
                        });

                        if (count == 0) {
                            $scope.videos_temp.push(input.files[a]);
                        }
                        //$scope.images_temp.push(input.files[a]);
                    }
                    else if (input.files[a].type != "video/mp4") {// && input.files[a].type != "video/x-ms-wmv"
                        swal("Please Upload the MP4 file " + a);
                        return;
                    } else if (input.files[a].size > 27262976) {
                        swal("Video size should be less than 27MB ");
                        return;
                    }
                }

            }
            // $scope.images_temp= $scope.UploadStudentProfilePic ;
        }

        $scope.remove_video = function (sel_video_del) {

            for (var i = 0; i < $scope.videos_temp.length; i++) {

                var vide123 = $scope.videos_temp[i];
                if (vide123.name == sel_video_del.name) {

                    $scope.videos_temp.splice(i, 1);
                }
            }
        }

        $scope.mobilenos_temp = [];
        $scope.mobilecount = 0;
        $scope.Others_list = [];

        $scope.add_mobile_nos = function () {

            var count = 0;


            angular.forEach($scope.Others_list, function (other123) {
                if (other123.COEEO_MobileNo == $scope.Oters_Mob_No || other123.COEEO_Emailid == $scope.Oters_Mail_Id) {
                    count += 1;
                }
            });
            if (count == 0) {
       
                $scope.Others_list.push({ COEEO_MobileNo: $scope.Oters_Mob_No, COEEO_Emailid: $scope.Oters_Mail_Id, COEEO_Name: $scope.Oters_Name });
                $scope.Oters_Mob_No = "";
                $scope.Oters_Mail_Id = "";
                $scope.Oters_Name = "";

            }
            else if (count > 0) {
                swal("Entered Details are Already Added !!!");
            }

            $scope.mobilecount = $scope.Others_list.length;

        }
        $scope.remove_mob_no = function (sel_mob_no_del) {

            for (var i = 0; i < $scope.Others_list.length; i++) {

                var mob123 = $scope.Others_list[i];
                if (mob123 == sel_mob_no_del) {

                    $scope.Others_list.splice(i, 1);
                }
            }
            $scope.mobilecount = $scope.Others_list.length;

        }


        $scope.clear_others = function () {
            $scope.Oters_Name = "";
            $scope.Oters_Mob_No = "";
            $scope.Oters_Mail_Id = "";
        }

        function Uploadprofile1() {

            var formData = new FormData();

            for (var i = 0; i <= $scope.images_temp.length; i++) {
                formData.append("File", $scope.images_temp[i]);
            }
       
            formData.append("Id", 0);

            var defer = $q.defer();
            $http.post("/api/ImageUpload/OnlineProgramdoc", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                })
                .success(function (d) {

                    defer.resolve(d);
                    swal(d);
                    $scope.images_paths = d;
                    if ($scope.videos_temp.length > 0) {
                        Uploadprofile2();
                    }
                    else if ($scope.videos_temp.length == 0) {
                        $scope.savemappedeventsdata();
                    }

                    // $scope.obj.image = d;
                })
                .error(function () {
                    defer.reject("File Upload Failed!");
                });
            // Uploads1(miid);
        }


        function Uploadprofile2() {

            var formData = new FormData();

            for (var i = 0; i <= $scope.videos_temp.length; i++) {
                formData.append("File", $scope.videos_temp[i]);
            }

            //We can send more data to server using append         
            formData.append("Id", 0);

            var defer = $q.defer();
            $http.post("/api/ImageUpload/OnlineProgramdoc", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                })
                .success(function (d) {

                    defer.resolve(d);
                    swal(d);
                    $scope.videos_paths = d;
                    // if ($scope.videos_temp.length > 0) {
                    $scope.savemappedeventsdata();
                    // }


                    // $scope.obj.image = d;
                })
                .error(function () {
                    defer.reject("File Upload Failed!");
                });
            // Uploads1(miid);
        }

        $scope.submit = function () {

            if ($scope.myForm2.$valid) {
                if ($scope.images_temp.length == 0 && $scope.videos_temp.length == 0) {
                    $scope.savemappedeventsdata();
                }
                else if (($scope.images_temp.length != 0 && $scope.videos_temp.length != 0) || $scope.images_temp.length != 0) {
                    Uploadprofile1();
                   

                }
               
                else if ($scope.images_temp.length == 0 && $scope.videos_temp.length != 0) {

                    Uploadprofile2();
                    // $scope.savemappedeventsdata();
                }
            }
            else {
                $scope.submitted2 = true;

            }
        }
    }

    angular
        .module('app').filter("trustUrl", function ($sce) {
            return function (Url) {
                return $sce.trustAsResourceUrl(Url);
            };
        });

    angular
        .module('app').directive('txtArea', function () {
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