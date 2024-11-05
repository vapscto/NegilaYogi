
(function () {
    'use strict';
    angular
.module('app')
.controller('MasterCOEController', MasterCOEController)

    MasterCOEController.$inject = ['$rootScope', '$scope', '$state', '$location', 'apiService', 'Flash', '$http', '$q', '$stateParams', '$filter', '$sce']
    function MasterCOEController($rootScope, $scope, $state, $location, apiService, Flash, $http, $q, $stateParams, $filter, $sce) {
        $scope.editEmployee = {};
        // $scope.usercheck = true;
        // $scope.class = true;
        //swal("gdhjjjjjjj");
        $scope.editflag = false;
        $scope.hstep = 1;
        $scope.mstep = 1;

        $scope.ismeridian = true;
        $scope.toggleMode = function () {
            $scope.ismeridian = !$scope.ismeridian;
        };

        //Ui Grid view rendering data from data base
        $scope.gridOptions1 = {
            enableColumnMenus: false,
            enableFiltering: true,
            paginationPageSizes: [5, 10, 15],
            paginationPageSize: 5,

            columnDefs: [
                  { name: 'SNO', width: '10%', enableFiltering: false, field: 'name', cellTemplate: '<div class="ui-grid-cell-contents">{{grid.options.paginationPageSize *(grid.options.paginationCurrentPage-1)+grid.renderContainers.body.visibleRowCache.indexOf(row)+1}}</div>' },
                     //{ name: 'yearName', displayName: 'Academic Year' },
              { name: 'coemE_EventName', width: '30%', displayName: 'Event Name' },
            { name: 'coemE_EventDesc', width: '45%', displayName: 'Event Description' },

               {
                   field: 'id', name: '',
                   displayName: 'Actions', enableFiltering: false, enableSorting: false, cellTemplate:

                    '<div class="grid-action-cell">' +
                     '<a  href="javascript:void(0)" data-toggle="modal" data-target="#popup1" data-backdrop="static" ng-click="grid.appScope.viewrecordspopup1(row.entity);"> <i class="fa fa-eye text-purple" ></i></a>  &nbsp; &nbsp;' +
                 '<a href="javascript:void(0)" ng-click="grid.appScope.getorgvalue1(row.entity);"> <md-tooltip md-direction="down">Edit Now</md-tooltip> <i class="fa fa-pencil-square-o" ></i></a>  &nbsp; &nbsp;' +
                '<a ng-if="row.entity.coemE_ActiveFlag === false" href="javascript:void(0)" style="color:green;" ng-click="grid.appScope.switch1(row.entity);"> <md-tooltip md-direction="down">Activate Now</md-tooltip> <i class="fa fa-toggle-off text-red" aria-hidden="true"></i></a>' +
              '<span ng-if="row.entity.coemE_ActiveFlag === true"><a href="javascript:void(0)"  style="color:red;" ng-click="grid.appScope.switch1(row.entity);">  <md-tooltip md-direction="down">Deactivate Now</md-tooltip> <i class="fa fa-toggle-on text-green"  aria-hidden="true"></i></a><span>' +

                '</div>'
               }
            ]

        };

        $scope.gridOptions2 = {
            enableColumnMenus: false,
            enableFiltering: true,
            paginationPageSizes: [5, 10, 15],
            paginationPageSize: 5,

            columnDefs: [
                  { name: 'SNO', width: '5%', enableFiltering: false, field: 'name', cellTemplate: '<div class="ui-grid-cell-contents">{{grid.options.paginationPageSize *(grid.options.paginationCurrentPage-1)+grid.renderContainers.body.visibleRowCache.indexOf(row)+1}}</div>' },
                     { name: 'asmaY_Year', width: '10%', displayName: 'Academic Year' },
              { name: 'coemE_EventName', width: '15%', displayName: 'Event Name' },
            {
                name: 'images', displayName: 'Images', enableFiltering: false, enableSorting: false, cellTemplate: '<div class="grid-action-cell">' +
                       '<a   href="javascript:void(0)" data-toggle="modal" data-target="#popup11" data-backdrop="static" ng-click="grid.appScope.viewrecordspopup2(row.entity);"> <i class="fa fa-picture-o text-green" ></i></a>  &nbsp; &nbsp;' + '</div>'
            },//fa-file-image-o,fa-film,fa-video-camera,fa-file-video-o,fa-picture-o
            {
                name: 'videos', displayName: 'Videos', enableFiltering: false, enableSorting: false, cellTemplate: '<div class="grid-action-cell">' +
                       '<a  href="javascript:void(0)" data-toggle="modal" data-target="#popup15" data-backdrop="static" ng-click="grid.appScope.viewrecordspopup2(row.entity);"> <i class="fa fa-film text-blue" ></i></a>  &nbsp; &nbsp;' + '</div>'
            },
            {
                name: 'classes', displayName: 'Classes', enableFiltering: false, enableSorting: false, cellTemplate: '<div class="grid-action-cell">' +
                       '<a  href="javascript:void(0)" data-toggle="modal" data-target="#popup12" data-backdrop="static" ng-click="grid.appScope.viewrecordspopup2(row.entity);"> <i class="fa fa-eye text-blue" ></i></a>  &nbsp; &nbsp;' + '</div>'
            },
             {
                 name: 'employees', displayName: 'Employees', enableFiltering: false, enableSorting: false, cellTemplate: '<div class="grid-action-cell">' +
                        '<a  href="javascript:void(0)" data-toggle="modal" data-target="#popup13" data-backdrop="static" ng-click="grid.appScope.viewrecordspopup2(row.entity);"> <i class="fa fa-eye text-orange" ></i></a>  &nbsp; &nbsp;' + '</div>'
             },
              {
                  name: 'others', displayName: 'Others', enableFiltering: false, enableSorting: false, cellTemplate: '<div class="grid-action-cell">' +
                         '<a  href="javascript:void(0)" data-toggle="modal" data-target="#popup14" data-backdrop="static" ng-click="grid.appScope.viewrecordspopup2(row.entity);"> <i class="fa fa-eye text-red" ></i></a>  &nbsp; &nbsp;' + '</div>'
              },
                {
                    name: 'Alumni', displayName: 'Alumni', enableFiltering: false, enableSorting: false, cellTemplate: '<div class="grid-action-cell">' +
                        '<a  href="javascript:void(0)" data-toggle="modal" data-target="#popup16" data-backdrop="static" ng-click="grid.appScope.viewrecordspopup2(row.entity);"> <i class="fa fa-eye text-blue" ></i></a>  &nbsp; &nbsp;' + '</div>'
                },
                //{
                //    name: 'EventCompleted', displayName: 'EventCompleted', enableFiltering: false, enableSorting: false, cellTemplate: '<div class="grid-action-cell">' +
                //        '<span ng-if="row.entity.buttonflag === true"><a href="javascript:void(0)" ><button>Button<i class="fa fa-primary" ></i></a></button> <span>' + '</div>'
                //},
               {
                   field: 'id', name: '',
                   displayName: 'Actions', enableFiltering: false, enableSorting: false, cellTemplate:
                 //'<div class="grid-action-cell">' +
                 //'<a href="javascript:void(0)" ng-click="grid.appScope.getorgvalue(row.entity);"> <md-tooltip md-direction="down">Edit</md-tooltip><i class="fa fa-pencil-square-o" aria-hidden="true"></i></a>' +
                 //'<a ng-if="row.entity.ttmsaB_ActiveFlag === false" href="javascript:void(0)" style="color:green;" ng-click="grid.appScope.switch(row.entity);"><md-tooltip md-direction="down">Active Now</md-tooltip> <i class="fa fa-toggle-on text-red" aria-hidden="true"></i></a>' +
                 // '<span ng-if="row.entity.ttmsaB_ActiveFlag === true ""><a href="javascript:void(0)"  style="color:red;" ng-click="grid.appScope.switch(row.entity);"> <md-tooltip md-direction="down">Deactive Now</md-tooltip> <i class="fa fa-toggle-off text-green" aria-hidden="true"></i></a><span>' +
                 //'</div>'
                 //'<a href="javascript:void(0)" ng-click="grid.appScope.getorgvalue(row.entity);"> <i class="fa fa-pencil-square-o" ></i></a>' +
                 //'<a href="javascript:void(0)" ng-click="grid.appScope.deletedata(row.entity);"> <i class="fa fa-trash text-danger"></i></a>' +
                   //'</div>'
                    '<div class="grid-action-cell">' +
                     '<a  href="javascript:void(0)" data-toggle="modal" data-target="#popup2" data-backdrop="static" ng-click="grid.appScope.viewrecordspopup2(row.entity);"> <i class="fa fa-eye text-purple" ></i></a>  &nbsp; &nbsp;' +
                 '<a href="javascript:void(0)" ng-click="grid.appScope.getorgvalue2(row.entity);"> <md-tooltip md-direction="down">Edit Now</md-tooltip> <i class="fa fa-pencil-square-o" ></i></a>  &nbsp; &nbsp;' +
                '<a ng-if="row.entity.coeE_ActiveFlag === false" href="javascript:void(0)" style="color:green;" ng-click="grid.appScope.switch2(row.entity);"> <md-tooltip md-direction="down">Activate Now</md-tooltip> <i class="fa fa-toggle-off text-red" aria-hidden="true"></i></a>' +
                       '<span ng-if="row.entity.coeE_ActiveFlag === true"><a href="javascript:void(0)"  style="color:red;" ng-click="grid.appScope.switch2(row.entity);">  <md-tooltip md-direction="down">Deactivate Now</md-tooltip> <i class="fa fa-toggle-on text-green"  aria-hidden="true"></i></a><span>' +

                      
                      

                '</div>'
                },
              
            ]

        };



        // TO Save The Data
        $scope.submitted1 = false;
        $scope.savemastereventsdata = function () {

            
            if ($scope.myForm1.$valid) {

                var data = {
                    // "ASMAY_Id": $scope.asmaY_Id,COEME_Id,COEME_EventName,COEME_EventDesc,COEME_SMSMessage,COEME_MailSubject,COEME_MailHeader,COEME_MailFooter,COEME_Mail_Message,COEME_MailHTMLTemplate
                    "COEME_Id": $scope.COEME_Id,
                    "COEME_EventName": $scope.coemE_EventName,
                    "COEME_EventDesc": $scope.coemE_EventDesc,
                    "COEME_SMSMessage": $scope.coemE_SMSMessage,
                    "COEME_MailSubject": $scope.coemE_MailSubject,
                    "COEME_MailHeader": $scope.coemE_MailHeader,
                    "COEME_MailFooter": $scope.coemE_MailFooter,
                    "COEME_Mail_Message": $scope.coemE_Mail_Message,
                    "COEME_MailHTMLTemplate": $scope.coemE_MailHTMLTemplate,
                    // "TTMC_CategoryName": $scope.TTMC_CategoryName,   
                }
                apiService.create("MasterCOE/savedetail1", data).
                    then(function (promise) {
                        if (promise.returnval === true) {
                            swal('Data successfully Saved');
                            // $state.reload();
                        }
                        else if (promise.returnduplicatestatus === 'Duplicate') {
                            swal('Records AlReady Exist !');
                            //$state.reload();
                        }
                        else if (promise.returnval === false) {
                            swal('Data Not Saved !');
                            //$state.reload();
                        }
                        // $scope.loadData();
                        $scope.gridOptions1.data = promise.master_eventlist;
                        //$scope.loadData();
                    })

                // $scope.BindData();
                $scope.loadData();
                $scope.clear1();
                $scope.clear2();
            }
            else {
                $scope.submitted1 = true;

            }

        };

        $scope.stud_chk = "0";
        $scope.oldstud_chk = "0";
        $scope.stf_chk = "0";
        $scope.oth_chk = "0";



        // TO Save The Data
        $scope.coeE_EStartDate = "";
        // $scope.coeE_EEndDate = "";
        $scope.submitted2 = false;
        $scope.savemappedeventsdata = function () {



            
            if ($scope.myForm2.$valid) {
                $scope.albumNameArray1 = [];
                $scope.albumNameArray2 = [];

                if ($scope.sms_flag == "Yes") {
                    $scope.coeE_SMSActiveFlag = true;
                }
                else if ($scope.sms_flag == "No") {
                    $scope.coeE_SMSActiveFlag = false;
                }

                if ($scope.email_flag == "Yes") {
                    $scope.coeE_MailActiveFlag = true;
                }
                else if ($scope.email_flag == "No") {
                    $scope.coeE_MailActiveFlag = false;
                }

                if ($scope.repeate_flag == "Yes") {
                    $scope.coeE_RepeatFlag = true;

                }
                else if ($scope.repeate_flag == "No") {
                    $scope.coeE_RepeatFlag = false;
                    $scope.coeE_ReminderSchedule = "";
                }
                //
                if ($scope.stud_chk == "1") {
                    $scope.coeE_StudentFlag = true;
                    angular.forEach($scope.class_list, function (role) {
                        if (role.class) $scope.albumNameArray1.push(role);
                    })
                }
                else if ($scope.stud_chk == "0") {
                    $scope.coeE_StudentFlag = false;
                }

                if ($scope.oldstud_chk == "1") {
                    $scope.coeE_AlumniFlag = true;
                }
                else if ($scope.oldstud_chk == "0") {
                    $scope.coeE_AlumniFlag = false;
                }

                if ($scope.stf_chk == "1") {
                    $scope.coeE_EmployeeFlag = true;
                    angular.forEach($scope.staff_type_list, function (role) {
                        if (role.sec) $scope.albumNameArray2.push(role);
                    })
                }
                else if ($scope.stf_chk == "0") {
                    $scope.coeE_EmployeeFlag = false;
                }

                if ($scope.oth_chk == "1") {
                    $scope.coeE_OtherFlag = true;
                }
                else if ($scope.oth_chk == "0") {
                    $scope.coeE_OtherFlag = false;
                }


                // added by akash
                $scope.documentListOtherDetails11 = [];
                if ($scope.editflag == true) {
                    if ($scope.images_temp != null) {
                        angular.forEach($scope.images_temp, function (qq) {

                            $scope.documentListOtherDetails11.push({ COEEI_Images: qq.intbfl_FilePath, COEEI_Id: qq.coeeI_Id });

                        })
                        $scope.filedoc = $scope.documentListOtherDetails11;
                    }
                }
                else {
                    if ($scope.images_temp != null && $scope.images_temp.length>0) {
                        angular.forEach($scope.images_temp, function (qq) {

                            $scope.documentListOtherDetails11.push({ COEEI_Images: qq.intbfl_FilePath});

                        })
                        $scope.filedoc = $scope.documentListOtherDetails11;
                    }
                }
               

                //if ($scope.checklink == true) {
                //    angular.forEach($scope.urldocumentlist, function (qq) {
                //        if (qq.IHW_FilePath != null) {
                //            $scope.filedoc.push({ IHW_FilePath: qq.IHW_FilePath, FileName: qq.IHW_FilePath });
                //        }
                //    });
                //}



                //angular.forEach($scope.class_list, function (role) {
                //    if (role.class) $scope.albumNameArray1.push(role);
                //})
                //angular.forEach($scope.staff_type_list, function (role) {
                //    if (role.sec) $scope.albumNameArray2.push(role);
                //})

                // Uploadprofile($scope.images_temp);
                // Uploadprofile($scope.videos_temp);

                $scope.coeE_EStartDate = new Date($scope.coeE_EStartDate).toDateString();
                $scope.coeE_EEndDate = new Date($scope.coeE_EEndDate).toDateString();
                $scope.coeE_ReminderDate = new Date($scope.coeE_ReminderDate).toDateString();

                if ($scope.images_paths == null || $scope.images_paths == undefined || $scope.images_paths == "") {
                    $scope.images_paths = [];
                }
                

                if ($scope.videos_paths == null || $scope.videos_paths == undefined || $scope.videos_paths == "") {
                    $scope.videos_paths = [];
                }

                //if ($scope.coeE_EStartTime != null || $scope.coeE_EStartTime != undefined || $scope.coeE_EStartTime != "") {
                //    $scope.coeE_EStartTime = $filter('date')($scope.coeE_EStartTime, "h:mm a");
                //}
                //else {
                //    $scope.coeE_EStartTime = " ";
                //}
                //if ($scope.coeE_EEndTime != null || $scope.coeE_EEndTime != undefined || $scope.coeE_EEndTime != "") {

                //    $scope.coeE_EEndTime = $filter('date')($scope.coeE_EEndTime, "h:mm a");
                //}
                //else {
                //    $scope.coeE_EEndTime = " ";
                //} 

                var data = {
                    // "ASMAY_Id": $scope.asmaY_Id,COEME_Id,COEME_EventName,COEME_EventDesc,COEME_SMSMessage,COEME_MailSubject,COEME_MailHeader,COEME_MailFooter,COEME_Mail_Message,COEME_MailHTMLTemplate 
                    //[COEE_Id]  ,[MI_Id] ,[COEME_Id] ,[COEE_EStartDate],[COEE_EEndDate],[COEE_EEStartTime],[COEE_EEndTime],[COEE_SMSMessage] ,[COEE_SMSActiveFlag]     ,[COEE_MailSubject]     ,[COEE_MailHeader]     ,[COEE_MailFooter]      ,[COEE_Mail_Message]      ,[COEE_MailHTMLTemplate]      ,[COEE_MailActiveFlag]      ,[COEE_ReminderDate]      ,[COEE_AllDayFlag]      ,[COEE_RepeatFlag]      ,[COEE_ReminderSchedule]      ,[COEE_ReminderNotification]      ,[COEE_Memos]      ,[COEE_StudentFlag]      ,[COEE_AlumniFlag]      ,[COEE_EmployeeFlag]      ,[COEE_ManagementFlag]      ,[CreatedDate]      ,[UpdatedDate]  $filter('date')($scope.coeE_EEStartTime, "h:mm a"),
                    "COEE_Id": $scope.COEE_Id,
                    "ASMAY_Id": $scope.asmaY_Id,
                    "COEME_Id": $scope.coemE_Id_f,
                    "COEE_EStartDate": $scope.coeE_EStartDate,
                    "COEE_EEndDate": $scope.coeE_EEndDate,
                    "COEE_EStartTime":  $filter('date')($scope.coeE_EStartTime, "h:mm a"),
                    "COEE_EEndTime": $filter('date')($scope.coeE_EEndTime, "h:mm a"),
                    "COEE_SMSMessage": $scope.coeE_SMSMessage,
                    "COEE_SMSActiveFlag": $scope.coeE_SMSActiveFlag,
                    "COEE_MailSubject": $scope.coeE_MailSubject,
                    "COEE_MailHeader": $scope.coeE_MailHeader,
                    "COEE_MailFooter": $scope.coeE_MailFooter,
                    "COEE_Mail_Message": $scope.coeE_Mail_Message,
                    "COEE_MailHTMLTemplate": $scope.coeE_MailHTMLTemplate,
                    "COEE_MailActiveFlag": $scope.coeE_MailActiveFlag,
                    "COEE_ReminderDate": $scope.coeE_ReminderDate,
                    //  "COEE_AllDayFlag": $scope.coemE_MailFooter,
                    "COEE_RepeatFlag": $scope.coeE_RepeatFlag,
                    "COEE_ReminderSchedule": $scope.coeE_ReminderSchedule,
                    //"COEE_ReminderNotification": $scope.coemE_MailSubject,
                    //  "COEE_Memos": $scope.coemE_MailHeader,
                    "COEE_StudentFlag": $scope.coeE_StudentFlag,
                    "COEE_AlumniFlag": $scope.coeE_AlumniFlag,
                    "COEE_EmployeeFlag": $scope.coeE_EmployeeFlag,
                    "COEE_ManagementFlag": $scope.coeE_ManagementFlag,
                    "COEE_OtherFlag": $scope.coeE_OtherFlag,
                    stu_class_list: $scope.albumNameArray1,
                    emp_type_list: $scope.albumNameArray2,
                    //oth_mobilenos_list: $scope.mobilenos_temp,
                    others_list: $scope.Others_list,
                    images_listCoe: $scope.filedoc,
                    videos_list: $scope.videos_paths,
                }
                apiService.create("MasterCOE/savedetail2", data).
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
                    //$scope.BindData();
                    $scope.gridOptions2.data = promise.mapped_eventlist;
                })
                $scope.images_paths = [];
                $scope.videos_paths = [];
                $scope.loadData();
                $scope.clear2();
                //$state.reload();
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

            //$scope.all_check();
            //$scope.all_check1();
            apiService.getDATA("MasterCOE/getalldetails").
       then(function (promise) {
           $scope.totallist = [];
           $scope.Complist = [];
           $scope.currentPage = 1;
           $scope.itemsPerPage = 5;
           //  $scope.yearlist = promise.acayear;
           //-----------
           $scope.parameter_list.push({ "id": "1", "name": "[NAME]" }, { "id": "2", "name": "[INSTUITENAME]" });
           //------
           //  $scope.parameter_list = promise.parameterlist;
           $scope.year_list = promise.yearlist;
           $scope.class_list = promise.classlist;
           $scope.arrlistchkgroup = promise.classlist;
           // $scope.master_event_list = promise.master_eventlist;
           $scope.master_event_list = promise.eventlist_act;
           $scope.gridOptions1.data = promise.master_eventlist;
           
           $scope.staff_type_list = promise.stafftypelist;
           $scope.all_check();
           $scope.all_check1();
          
           //  $scope.stafflist = promise.stafflist;
           //$scope.gridOptions.data = promise.ttstafflist;

           $scope.totallist = promise.mapped_eventlist;
           $scope.Complist = promise.completeddate;

           angular.forEach($scope.totallist, function (total) {
               total.buttonflag = false;
               angular.forEach($scope.Complist, function (com) {
                   if (total.coeE_Id == com.coeE_Id) {
                       total.buttonflag = true;
                   }
               })
           })
           $scope.gridOptions2.data = $scope.totallist;
           console.log($scope.totallist);

                })

           // $state.reload();
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

        //$scope.isOptionsRequired = function () {
        //    return !$scope.class_list.some(function (options) {
        //        return options.class;
        //    });

        //}

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

        //$scope.isOptionsRequired1 = function () {
        //    //return $scope.alll1;
        //    return !$scope.staff_type_list.some(function (options) {
        //        return options.sec;
        //    });

        //}


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
                apiService.create("MasterCOE/geteventdetails", data).
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
            //$scope.class_list1 = $scope.temp_classlist;
            //  $scope.section_list1 = $scope.temp_sectionlist;
            // $scope.staff_list1 = $scope.temp_stafflist;
            // $scope.subject_list1 = $scope.temp_subjectlist;
            //  $scope.period_list1 = $scope.temp_periodlist;
            // $scope.all_check();
            $scope.coemE_EventName = "";
            $scope.coemE_EventDesc = "";
            $scope.coemE_SMSMessage = "";
            $scope.coemE_MailSubject = "";
            $scope.coemE_MailHeader = "";
            $scope.coemE_MailFooter = "";
            $scope.coemE_Mail_Message = "";
            $scope.coemE_MailHTMLTemplate = "";
            //  $scope.TTMC_CategoryName = "";
            // $scope.TTMC_Id = 0; 
            $scope.submitted1 = false;
            $scope.myForm1.$setPristine();
            $scope.myForm1.$setUntouched();
            $scope.search = "";

            $scope.radiotype = 'Manual';
            $scope.manualflag = true;
        };

        $scope.clear2 = function () {
            $scope.files_flag = false;
            $scope.COEE_Id = 0;
            //$scope.class_list1 = $scope.temp_classlist;
            //  $scope.section_list1 = $scope.temp_sectionlist;
            // $scope.staff_list1 = $scope.temp_stafflist;
            // $scope.subject_list1 = $scope.temp_subjectlist;
            //  $scope.period_list1 = $scope.temp_periodlist;
            $scope.usercheck = true;
            $scope.usercheck1 = true;
            $scope.all_check();
            $scope.all_check1();

            $scope.asmaY_Id = "";
            $scope.coemE_Id_f = "";
            // $scope.get_event_details();
            $scope.evt_flg = true;

            $scope.coeE_EventDesc = "";
            $scope.coeE_MailHTMLTemplate = "";
            $scope.coeE_SMSMessage = "";
            $scope.coeE_MailSubject = "";
            $scope.coeE_MailHeader = "";
            $scope.coeE_MailFooter = "";
            $scope.coeE_Mail_Message = "";
            $scope.coeE_EStartDate = "";
            // $scope.get_end_remind();
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
            //$scope.images_temp = [];
            $scope.images_temp = [{ id: 'document' }];
            $scope.videos_temp = [];
            $scope.mobilenos_temp = [];
            $scope.Others_list = [];
            $scope.mobilecount = 0;
            $scope.c1 = false;
            $scope.c2 = false;
            $scope.c3 = false;
            $scope.c4 = false;
            // $scope.group_check = "0";

            //$scope.coemE_MailHTMLTemplate = "";
            //  $scope.TTMC_CategoryName = "";
            // $scope.TTMC_Id = 0; 
            $scope.submitted2 = false;
            $scope.myForm2.$setPristine();
            $scope.myForm2.$setUntouched();
            $scope.search = "";
        };




        //TO clear  data
        $scope.clearid = function () {
            // $scope.TTMC_CategoryName = "";
            //$scope.TTMC_Id = 0;
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
        $scope.getorgvalue1 = function (employee) {
            $scope.editEmployee = employee.coemE_Id;
            var pageid = $scope.editEmployee;
            apiService.getURI("MasterCOE/getdetails1", pageid).
                then(function (promise) {

                $scope.radiotype = "";
                $scope.COEME_Id = promise.edit_m_event[0].coemE_Id;

                $scope.coemE_EventName = promise.edit_m_event[0].coemE_EventName;
                $scope.coemE_EventDesc = promise.edit_m_event[0].coemE_EventDesc;
                $scope.coemE_SMSMessage = promise.edit_m_event[0].coemE_SMSMessage;
                $scope.coemE_MailSubject = promise.edit_m_event[0].coemE_MailSubject;
                $scope.coemE_MailHeader = promise.edit_m_event[0].coemE_MailHeader;
                $scope.coemE_MailFooter = promise.edit_m_event[0].coemE_MailFooter;
                $scope.coemE_Mail_Message = promise.edit_m_event[0].coemE_Mail_Message;
                $scope.coemE_MailHTMLTemplate = promise.edit_m_event[0].coemE_MailHTMLTemplate;

                    if ($scope.coemE_SMSMessage != "" && $scope.coemE_Mail_Message != "") {
                        $scope.radiotype = 'Manual'
                        $scope.manualflag = true;
                    }
                    else {
                        $scope.radiotype = 'Default'
                        $scope.manualflag = false;
                    }
                    
            })
        }
        
        $scope.getorgvalue2 = function (employee) {
            $scope.images_temp = [];
            $scope.images_temp = [{ id: 'document' }];
            $scope.editEmployee = employee.coeE_Id;
            var pageid = $scope.editEmployee;
            apiService.getURI("MasterCOE/getdetails2", pageid).
                then(function (promise) {

                   
                    $scope.editflag = true;
                    if (promise.edit_images_list !=null && promise.edit_images_list.length>0) {
                        $scope.images_temp = promise.edit_images_list;
                    }
                  
                 

                    //if (promise.edit_images_list != 0) {

                    //}



                $scope.COEE_Id = promise.edit_map_event[0].coeE_Id;
                $scope.asmaY_Id = promise.edit_map_event[0].asmaY_Id,
                $scope.coemE_Id_f = promise.edit_map_event[0].coemE_Id;
                $scope.evt_flg = false;
                // $scope.coeE_EventName = promise.edit_map_event[0].coeE_EventName;
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
                //if (tdyDate.getDate() > new Date($scope.coeE_EStartDate).getDate()) {
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
                    // $scope.coeE_EStartDate = new Date(promise.edit_map_event[0].coeE_EStartDate);
                    $scope.coeE_EEndDate = new Date(promise.edit_map_event[0].coeE_EEndDate);
                    if (promise.edit_map_event[0].coeE_EStartTime == null || promise.edit_map_event[0].coeE_EStartTime == undefined || promise.edit_map_event[0].coeE_EStartTime == "") {
                        //$scope.coeE_EStartTime = "";
                    }
                    else if (promise.edit_map_event[0].coeE_EStartTime != null || promise.edit_map_event[0].coeE_EStartTime != undefined || promise.edit_map_event[0].coeE_EStartTime != "") {
                        $scope.coeE_EStartTime = moment(promise.edit_map_event[0].coeE_EStartTime, 'h:mm a').format();
                    }
                   
                    if (promise.edit_map_event[0].coeE_EEndTime == null || promise.edit_map_event[0].coeE_EEndTime == undefined || promise.edit_map_event[0].coeE_EEndTime == "") {
                        //$scope.coeE_EEndTime = "";
                    }
                    else if(promise.edit_map_event[0].coeE_EEndTime != null || promise.edit_map_event[0].coeE_EEndTime != undefined || promise.edit_map_event[0].coeE_EEndTime != "") {
                        $scope.coeE_EEndTime = moment(promise.edit_map_event[0].coeE_EEndTime, 'h:mm a').format();
                    }
                    
                    $scope.coeE_ReminderDate = new Date(promise.edit_map_event[0].coeE_ReminderDate);
                    var tdyDate = new Date();
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
                //$scope.coeE_EStartDate = new Date(promise.edit_map_event[0].coeE_EStartDate);
                // $scope.coeE_EEndDate =  new Date(promise.edit_map_event[0].coeE_EEndDate);
                //$scope.coeE_EStartTime = moment(promise.edit_map_event[0].coeE_EStartTime, 'h:mm a').format();
                //$scope.coeE_EEndTime = moment(promise.edit_map_event[0].coeE_EEndTime, 'h:mm a').format();
                //$scope.coeE_ReminderDate = new Date(promise.edit_map_event[0].coeE_ReminderDate);
                //if (new Date() > new Date($scope.coeE_ReminderDate)) {
                //    swal("Selected Event Reminder Date Is Before the Today's Date,So U Need To Change");
                //    $scope.coeE_ReminderDate = "";
                //    $scope.reminder_days = "";
                //}
                //else {
                //    //BY HL
                //    var firstDate1 = new Date($scope.coeE_ReminderDate);
                //    var secondDate1 = new Date($scope.coeE_EStartDate);
                //    //var firstDate2 = new Date();
                //    firstDate1 = $filter('date')(firstDate1, "dd/MM/yy");
                //    secondDate1 = $filter('date')(secondDate1, "dd/MM/yy");
                //    var date2 = new Date($scope.formatString(secondDate1));
                //    var date1 = new Date($scope.formatString(firstDate1));
                //    var timeDiff1 = Math.abs(date2.getTime() - date1.getTime());
                //    var diffDays1;
                //    // var diffDays2;
                //    diffDays1 = Math.ceil(timeDiff1 / (1000 * 3600 * 24));
                //    // firstDate2 = $filter('date')(firstDate2, "dd/MM/yy");
                //    // var date3 = new Date($scope.formatString(firstDate2));
                //    //var timeDiff2 = Math.abs(date2.getTime() - date3.getTime());
                //    //diffDays2 = Math.ceil(timeDiff2 / (1000 * 3600 * 24));
                //    //HL
                //    //MB
                //    //var firstDate = new Date($scope.coeE_ReminderDate);
                //    //var secondDate = new Date($scope.coeE_EStartDate);
                //    // var firstDate1 = new Date();
                //    //var diffDays;
                //    //var diffDays1;
                //    //var oneDay = 24 * 60 * 60 * 1000; // hours*minutes*seconds*milliseconds    
                //    //if (firstDate.toDateString() == secondDate.toDateString()) {
                //    //    diffDays = 0;
                //    // }
                //    // else {
                //    // diffDays = Math.round(Math.abs((firstDate.getTime() - secondDate.getTime()) / (oneDay)));
                //    // diffDays += 1;
                //    // }
                //    //if (firstDate1.toDateString() == secondDate.toDateString()) {
                //    //    diffDays1 = 0;
                //    // }
                //    // else {
                //    // diffDays1 = Math.round(Math.abs((firstDate1.getTime() - secondDate.getTime()) / (oneDay)));
                //    // diffDays += 1;
                //    //  }
                //    //  if (diffDays1 <= diffDays2)
                //    // {
                //    $scope.reminder_days = diffDays1;
                //    // }
                //    // else {
                //    // swal("Selected Event Reminder Date Before Today's Date !!!!");

                //    // }
                //    //$scope.reminder_days = diffDays;
                //    //MB
                //}
                $scope.coeE_MailActiveFlag = promise.edit_map_event[0].coeE_MailActiveFlag;
                $scope.coeE_SMSActiveFlag = promise.edit_map_event[0].coeE_SMSActiveFlag;
                if ($scope.coeE_MailActiveFlag == true) {
                    $scope.email_flag = "Yes";
                }
                else if ($scope.coeE_MailActiveFlag == false) {
                    $scope.email_flag = "No";
                }
                if ($scope.coeE_SMSActiveFlag == true) {
                    $scope.sms_flag = "Yes";
                }
                else if ($scope.coeE_SMSActiveFlag == false) {
                    $scope.sms_flag = "No";
                }
                $scope.coeE_RepeatFlag = promise.edit_map_event[0].coeE_RepeatFlag;
                if ($scope.coeE_RepeatFlag == true) {
                    $scope.repeate_flag = "Yes";

                }
                else if ($scope.coeE_RepeatFlag == false) {
                    $scope.repeate_flag = "No";
                }

                // $scope.coeE_ReminderSchedule = "";
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
                        // if (role.class) $scope.albumNameArray1.push(role);
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
                        //if (role.sec) $scope.albumNameArray2.push(role);
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
                //   $scope.images_temp = promise.edit_images_list;
                //   $scope.videos_temp = promise.edit_videos_list;
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
                // $scope.mobilenos_temp = [];
                // angular.forEach(promise.edit_oth_mobilenos_list, function (role) {
                //     $scope.mobilenos_temp.push(role.coeeO_MobileNo);
                // })
                // $scope.mobilecount = $scope.mobilenos_temp.length;
                // $scope.Oters_Mob_No = "";
                //// $scope.mobilenos_temp = promise.edit_oth_mobilenos_list;
                $scope.Others_list = [];
                angular.forEach(promise.edit_oth_mobilenos_list, function (role) {
                    $scope.Others_list.push({ COEEO_MobileNo: role.coeeO_MobileNo, COEEO_Emailid: role.coeeO_Emailid, COEEO_Name: role.coeeO_Name });
                })
                $scope.mobilecount = $scope.Others_list.length;
                $scope.Oters_Name = "";
                $scope.Oters_Mob_No = "";
                $scope.Oters_Mail_Id = "";
                // $scope.mobilenos_temp = promise.edit_oth_mobilenos_list;


                    // added by akash

                    if (promise.attachementlist != null || promise.attachementlist > 0) {
                        $scope.documentListOtherDetails = [];
                        $scope.urldocumentlist = [];
                        angular.forEach(promise.attachementlist, function (aa) {
                            $scope.img = aa.ihwatT_Attachment;
                            if ($scope.img != null) {
                                var imagarr = $scope.img.split('.');
                                var lastelement = imagarr[imagarr.length - 1];

                                $scope.filetype2 = lastelement;
                            }

                            if ($scope.filetype2 == 'mp4' || $scope.filetype2 == 'wmv' || $scope.filetype2 == 'mp3'
                                || $scope.filetype2 == 'jpg' || $scope.filetype2 == 'jpeg' || $scope.filetype2 == 'png' ||
                                $scope.filetype2 == 'pdf' || $scope.filetype2 == 'xls' || $scope.filetype2 == 'xlsx'
                                || $scope.filetype2 == 'ppt' || $scope.filetype2 == 'pptx' || $scope.filetype2 == 'ppsx'
                                || $scope.filetype2 == 'doc' || $scope.filetype2 == 'docx') {

                                $scope.documentListOtherDetails.push({
                                    id: 1, IHW_FilePath: aa.ihwatT_Attachment,
                                    FileName: aa.ihwatT_FileName
                                });

                            }
                            else {
                                $scope.urldocumentlist.push({
                                    id: 1, IHW_FilePath: aa.ihwatT_Attachment,
                                    FileName: aa.ihwatT_FileName
                                });
                            }
                        });
                    }


        //


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
                    apiService.DeleteURI("MasterCOE/deletepages", pageid).
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
        $scope.viewrecordspopup1 = function (employee, SweetAlert) {
            
            $scope.editEmployee = employee.coemE_Id;
            var pageid = $scope.editEmployee;

            apiService.getURI("MasterCOE/getalldetailsviewrecords1", pageid).
                    then(function (promise) {
                        
                        $scope.Event_Name = promise.edit_m_event[0].coemE_EventName;
                        // $scope.day_Name = promise.edit_m_event[0].ttmD_DayName;
                        //  $scope.gridOptionspopup.data == promise.detailspopuparray;
                        $scope.viewrecordspopupdisplay1 = promise.edit_m_event;

                    })

        };

        //TO  View Record
        $scope.viewrecordspopup2 = function (employee, SweetAlert) {
            
            $scope.editEmployee = employee.coeE_Id;
            var pageid = $scope.editEmployee;

            apiService.getURI("MasterCOE/getalldetailsviewrecords2", pageid).
                    then(function (promise) {
                        
                        $scope.Event_Name = promise.edit_m_event[0].coemE_EventName;
                        // $scope.day_Name = promise.edit_m_event[0].ttmD_DayName;
                        //  $scope.gridOptionspopup.data == promise.detailspopuparray;
                        
                        $scope.viewrecordspopupdisplay2 = promise.edit_map_event;
                        $scope.view_imgs = promise.edit_images_list;
                        $scope.view_videos = promise.edit_videos_list;
                        $scope.view_classes = promise.edit_stu_class_list;
                        $scope.view_employees = promise.edit_emp_type_list;
                        $scope.view_others = promise.edit_oth_mobilenos_list;

                        $scope.view_alumniClslist = promise.alumniClslist;
                        if (promise.edit_videos_list != null && promise.edit_videos_list.length > 0) {
                            if (promise.edit_videos_list[0].coeeV_Videos.toString() != null && promise.edit_videos_list[0].coeeV_Videos.toString() != "") {
                                $scope.xyz = promise.edit_videos_list[0].coeeV_Videos.toString();
                            }

                        }
                       



                      
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

                apiService.create("MasterCOE/deactivate1", employee).
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

                apiService.create("MasterCOE/deactivate2", employee).
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
        $scope.getStudentBYYear = function (iddata) {
            
            for (var k = 0; k < $scope.year_list.length; k++) {

                if ($scope.year_list[k].asmaY_Id == iddata) {

                    var data = $scope.year_list[k].asmaY_Year;

                }

            }

            if (data != null) {
                
                console.log(data);
                var name, name1;
                for (var i = 0; i < data.length; i++) {
                    if (i < 4) {
                        if (i == 0) {
                            name = data[i];
                        } else {
                            name += data[i];
                        }
                    }
                    if (i > 4) {
                        if (i == 5) {
                            name1 = data[5];
                        } else {
                            name1 += data[i];
                        }
                    }
                }
                $scope.fromDate = name;
                $scope.toDate = name1;
                $scope.frommon = "";
                $scope.tomon = "";
                $scope.fromDay = "";
                $scope.toDay = "";
                // For Academic From Date


                //$scope.minDatemf = new Date(
                //      $scope.fromDate,
                //       $scope.frommon,
                //        $scope.fromDay + 1);

                $scope.maxDatemf = new Date(
                      $scope.toDate,
                       $scope.tomon,
                        $scope.toDay + 365);
                $scope.minDatemf = new Date();

            }


        }

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
                //var firstDate1 = new Date();
                //firstDate1 = $filter('date')(firstDate1, "dd/MM/yy");
                //secondDate1 = $filter('date')(secondDate1, "dd/MM/yy");
                //var date2 = new Date($scope.formatString(secondDate1));
                //var date1 = new Date($scope.formatString(firstDate1));
                //var timeDiff = Math.abs(date2.getTime() - date1.getTime());
                //$scope.dayDifference = Math.ceil(timeDiff / (1000 * 3600 * 24));
                //// alert($scope.dayDifference);

                //var someDate = new Date();
                //// var numberOfDaysToAdd = 6;
                //someDate.setDate(someDate.getDate() + $scope.dayDifference);
                //swal("Difference Days :" + $scope.dayDifference + " and Date:" + someDate.toDateString());
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

                    //for based on today's date
                    // var someDate = new Date();

                    // someDate.setDate(someDate.getDate() + days);
                    // $scope.coeE_ReminderDate = someDate;

                    //for based on start date
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

                        //$scope.ldr = true;
                        ////$('#' + document.id).removeAttr('src');
                        //$scope.SelectedFileForUploadzdOtherDetail = input.files;
                        //if (input.files && input.files[0]) {
                        //    var reader = new FileReader();
                        //    reader.onload = function (e) {
                        //        $('#blahD')
                        //            .attr('src', e.target.result);
                        //    };
                        //  reader.readAsDataURL(input.files[0]);
                          Uploadprofile();
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
            // $scope.images_temp= $scope.UploadStudentProfilePic ;
        }

        $scope.remove_img = function (sel_img_del) {
            
            for (var i = 0; i < $scope.images_temp.length; i++) {
                
                var imgt123 = $scope.images_temp[i];
                if (imgt123.name == sel_img_del.name) {
                    
                    $scope.images_temp.splice(i, 1);
                }
            }
            //angular.forEach($scope.images_temp, function (imgt123) {
            //    if(imgt123.name==sel_img_del.name)
            //            {
            //                
            //                $scope.images_temp.splice(sel_img_del);
            //            }
            //});


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
            //angular.forEach($scope.images_temp, function (imgt123) {
            //    if(imgt123.name==sel_img_del.name)
            //            {
            //                
            //                $scope.images_temp.splice(sel_img_del);
            //            }
            //});


        }

        $scope.mobilenos_temp = [];
        $scope.mobilecount = 0;
        $scope.Others_list = [];

        $scope.add_mobile_nos = function () {
            
            var count = 0;

            //angular.forEach($scope.mobilenos_temp, function (mobileno123) {
            //    if (mobileno123 == $scope.Oters_Mob_No) {
            //        count_m += 1;
            //    }
            //});


            angular.forEach($scope.Others_list, function (other123) {
                if (other123.COEEO_MobileNo == $scope.Oters_Mob_No || other123.COEEO_Emailid == $scope.Oters_Mail_Id) {
                    count += 1;
                }
            });
            if (count == 0) {
                //$scope.mobilenos_temp.push($scope.Oters_Mob_No);
                //$scope.Oters_Mob_No = "";
                $scope.Others_list.push({ COEEO_MobileNo: $scope.Oters_Mob_No, COEEO_Emailid: $scope.Oters_Mail_Id, COEEO_Name: $scope.Oters_Name });
                $scope.Oters_Mob_No = "";
                $scope.Oters_Mail_Id = "";
                $scope.Oters_Name = "";

            }
            else if (count > 0) {
                swal("Entered Details are Already Added !!!");
            }



            //  $scope.mobilenos_temp.push($scope.Oters_Mob_No);
            $scope.mobilecount = $scope.Others_list.length;

            //$scope.Oters_Mob_No = "";
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

            //We can send more data to server using append         
            formData.append("Id", 0);

            var defer = $q.defer();
            $http.post("/api/ImageUpload/Upload_imgs_vids", formData,
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
            $http.post("/api/ImageUpload/Upload_imgs_vids", formData,
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
                if ($scope.images_temp.length != 0 && $scope.videos_temp.length == 0) {                    
                    $scope.savemappedeventsdata();
                }
                else if (($scope.images_temp.length != 0 && $scope.videos_temp.length != 0) || $scope.images_temp.length != 0) {                    
                    Uploadprofile1();
                    // Uploadprofile2();
                    // $scope.savemappedeventsdata();


                    //$scope.savemappedeventsdata();
                    //Uploadprofile2();
                    //Uploadprofile1();

                }
                    //else if ($scope.images_temp.length != 0 && $scope.videos_temp.length == 0) {
                    //    
                    //    Uploadprofile1();
                    //    $scope.savemappedeventsdata();
                    //}
                else if ($scope.images_temp.length == 0 && $scope.videos_temp.length != 0) {
                    
                    Uploadprofile2();
                    // $scope.savemappedeventsdata();
                }
            }
            else {
                $scope.submitted2 = true;

            }

        }


        $scope.radiotype = 'Manual';
        $scope.manualflag = true;

        $scope.onselectradio = function ()
        {
            if ($scope.radiotype == "Manual") {
                $scope.manualflag = true;
                
            }

            if ($scope.radiotype == "Default") {
                $scope.manualflag = false;
            }

        }
        //added 
        $scope.UploadStudentProfilePic = [];
        $scope.images_temp = [];

        $scope.uploadStudentProfilePic = function (input, document) {
            //if ($scope.editflag == true) {
            //    if ($scope.images_temp.length > 0) {

            //    }
            //}
           

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

        $scope.imagepreview = []; 
        $scope.previewimg_img = function (img) {
            $scope.name = img + '';
            var img = $scope.name;
            if (img != null) {
                var imagarr = img.split('.');

                var lastelement = imagarr[imagarr.length - 1];
                if (lastelement == 'jpg' || lastelement == 'jpeg' || lastelement == 'png') {
                    $('#preview').attr('src', $scope.name);
                    $('#myimagePreview').modal('show');
                }
                else if (lastelement == 'doc' || lastelement == 'docx' || lastelement == 'xls' || lastelement == 'xlsx' || lastelement == 'ppt' || lastelement == 'pptx') {
                    $window.open($scope.name);
                }
                else if (lastelement == 'pdf') {
                    $('#showpdf').modal('hide');
                    var imagedownload1 = "";
                    imagedownload1 = $scope.name;
                    $http.get(imagedownload1, { responseType: 'arraybuffer' })
                        .success(function (response) {
                            var fileURL = "";
                            var file = "";
                            var embed = "";
                            var pdfId = "";
                            file = new Blob([(response)], { type: 'application/pdf' });
                            fileURL = URL.createObjectURL(file);
                            pdfId = document.getElementById("pdfIdzz");
                            pdfId.removeChild(pdfId.childNodes[0]);
                            embed = document.createElement('embed');
                            embed.setAttribute('src', fileURL);
                            embed.setAttribute('type', 'application/pdf');
                            embed.setAttribute('width', '100%');
                            embed.setAttribute('height', '1000');
                            pdfId.appendChild(embed);
                            $('#showpdf').modal('show');
                        });
                }
            }
            else {
                $window.open($scope.ismclT_FilePath)
            }
        };
        $scope.previewimg_new = function (img) {
            $scope.name = img + '';
            var img = $scope.name;
            if (img != null) {
                var imagarr = img.split('.');

                var lastelement = imagarr[imagarr.length - 1];
                if (lastelement == 'mp4') {
                    $('#previewVideo').attr('src', $scope.name);
                    $('#myvideoPreview').modal('show');
                }
            }
            else {
                $window.open($scope.ismclT_FilePath)
            }
        };
        //file upload
        $scope.selectFileforUploadzdOtherDetail = function (input, document) {
            $scope.SelectedFileForUploadzdOtherDetail = input.files;
            $scope.v_FileName = input.files[0].name;
            if (input.files && input.files[0]) {
                var reader = new FileReader();
                reader.onload = function (e) {
                    $('#blahD')
                        .attr('src', e.target.result);
                };
                reader.readAsDataURL(input.files[0]);
                UploadEmployeeDocumentOtherDetail(document);

            }
        };


        // added by akash
        $scope.selectFileforUploadzdOtherDetail1 = function (input, document) {
            $scope.SelectedFileForUploadzdOtherDetail = input.files;
            $scope.v_FileName = input.files[0].name;
            if (input.files && input.files[0]) {
                var reader = new FileReader();
                reader.onload = function (e) {
                    $('#blahD')
                        .attr('src', e.target.result);
                };
                reader.readAsDataURL(input.files[0]);
                UploadEmployeeDocumentOtherDetail(document);

            }
        };


        function UploadEmployeeDocumentOtherDetail(data) {
            var formData = new FormData();
            for (var i = 0; i <= $scope.selectFileforUploadzdOtherDetail.length; i++) {
                formData.append("File", $scope.SelectedFileForUploadzdOtherDetail[i]);
            }
            var defer = $q.defer();
            $http.post("/api/ImageUpload/Upload_imgs_vids", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                })
                .success(function (d) {
                    defer.resolve(d);
                    if (d !== "PathNotFound") {

                        //  data.INTBFL_FilePath = d[0].path;
                        data.intbfl_FilePath = d[0];
                        data.FileName = $scope.v_FileName;
                        //data.name = $scope.v_FileName;
                        data.name = data.intbfl_FilePath;

                    } else {
                        swal('Document Storage Path Not Found ..!!');
                    }
                })
                .error(function () {
                    $('#' + data.id).removeAttr('src');
                    defer.reject("File Upload Failed!");
                });
        }

        $scope.documentListOtherDetails = [{ id: 'document' }];
        $scope.addNewDocumentOtherDetail = function () {
            if ($scope.documentListOtherDetails.length !=0) {
                var newItemNo = $scope.documentListOtherDetails.length + 1;
                if (newItemNo <= 30) {
                    $scope.documentListOtherDetails.push({ 'id': 'document' + newItemNo });
                }
            
            }
        };
        $scope.removeNewDocumentOtherDetail = function (index, data) {
            if ($scope.documentListOtherDetails.length != 0) {
                var newItemNo = $scope.documentListOtherDetails.length - 1;
                $scope.documentListOtherDetails.splice(index, 1);
                if (data.hreothdeT_Id > 0) {
                    $scope.DeleteDocumentDataOthers(data);
                }
            
            }
        };

        //added by akash


        $scope.selectFileforUploadzdOtherDetail12 = function (input, document) {


            $scope.ldr = true;
            //$('#' + document.id).removeAttr('src');
            $scope.SelectedFileForUploadzdOtherDetail = input.files;
            if (input.files && input.files[0]) {
                var reader = new FileReader();
                reader.onload = function (e) {
                    $('#blahD')
                        .attr('src', e.target.result);
                };
                reader.readAsDataURL(input.files[0]);
                UploadEmployeeDocumentOtherDetail(document);

            }
        };



        $scope.images_temp = [{ id: 'document' }];
        $scope.addNewDocumentOtherDetail1 = function () {
            var newItemNo = $scope.images_temp.length + 1;
            if (newItemNo <= 30) {
                $scope.images_temp.push({ 'id': 'document' + newItemNo });
            }
        };

        $scope.removeNewDocumentOtherDetail1 = function (index, data) {
            var newItemNo = $scope.images_temp.length - 1;
            $scope.images_temp.splice(index, 1);
            if (data.hreothdeT_Id > 0) {
                $scope.DeleteDocumentDataOthers(data);
            }
        };

        function Uploadprofile2() {

            var formData = new FormData();

            for (var i = 0; i <= $scope.documentListOtherDetails.length; i++) {
                formData.append("File", $scope.videos_temp[i]);
            }

            //We can send more data to server using append         
            formData.append("Id", 0);

            var defer = $q.defer();
            $http.post("/api/ImageUpload/Upload_imgs_vids", formData,
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

        //function Uploadprofile1() {

        //    var formData = new FormData();

        //    for (var i = 0; i <= $scope.images_temp.length; i++) {
        //        formData.append("File", $scope.images_temp[i]);
        //    }

        //    //We can send more data to server using append         
        //    formData.append("Id", 0);

        //    var defer = $q.defer();
        //    $http.post("/api/ImageUpload/Upload_imgs_vids", formData,
        //        {
        //            withCredentials: true,
        //            headers: { 'Content-Type': undefined },
        //            transformRequest: angular.identity
        //        })
        //        .success(function (d) {

        //            defer.resolve(d);
        //            swal(d);
        //            $scope.images_paths = d;
        //            if ($scope.documentListOtherDetails.length > 0) {
        //                Uploadprofile2();
        //            }
        //            else if ($scope.documentListOtherDetails.length == 0) {
        //                $scope.savemappedeventsdata();
        //            }

        //            // $scope.obj.image = d;
        //        })
        //        .error(function () {
        //            defer.reject("File Upload Failed!");
        //        });
        //    // Uploads1(miid);
        //}
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