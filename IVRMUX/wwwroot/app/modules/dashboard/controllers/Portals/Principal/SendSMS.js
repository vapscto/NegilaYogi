
//dashboard.controller("SendSMSController", ['$rootScope', '$scope', '$state', '$location', 'dashboardService', 'Flash', 'apiService', '$stateParams', '$filter', 'superCache',
//function ($rootScope, $scope, $state, $location, dashboardService, Flash, apiService, $stateParams, $filter, superCache) {


(function () {
    'use strict';
    angular
        .module('app')
        .controller('SendSMSController', SendSMSController)
    SendSMSController.$inject = ['$rootScope', '$scope', '$state', '$location', 'dashboardService', 'Flash', 'apiService', '$stateParams', '$filter', 'superCache']
    function SendSMSController($rootScope, $scope, $state, $location, dashboardService, Flash, apiService, $stateParams, $filter, superCache) {

    $scope.currentPage = 1;
    $scope.itemsPerPage = 10;
        $scope.loadradioval = true;
        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }
        $scope.currentPage = 1;
        $scope.itemsPerPage = paginationformasters;
    $scope.changeradio = function () {
        
        //alert('a')
        //$scope.currentPage = 1;
        //$scope.itemsPerPage = 10;
        if ($scope.loadradioval == true) {
            $scope.radioval = 'General';
            $scope.loadradioval = false;
        }

        if ($scope.radioval == 'General') {
            $scope.mobno = "";
            $scope.mes = "";
        }

        if ($scope.radioval == 'Student') {

            $scope.stdmsg = "";
            var data = {
                "selectedRadiobtn": $scope.radioval
            }

            $scope.studentList = [];
            apiService.create("SendSMS/Getdetails/", data).
         then(function (promise) {
             
             $scope.fillacademiyear = promise.yearlist;
             $scope.currentYear = promise.currentYear;
             for (var i = 0; i < $scope.fillacademiyear.length; i++) {
                 if ($scope.currentYear[0].asmaY_Id == $scope.fillacademiyear[i].asmaY_Id) {
                     $scope.ASMAY_Id = $scope.currentYear[0].asmaY_Id;
                 }
                 $scope.classlist = promise.classlist;
                 $scope.ASMCL_Id = $scope.classlist[0].asmcL_Id;
                 $scope.sectionlist = promise.sectionlist;
                 $scope.asmS_Id = $scope.sectionlist[0].asmS_Id;

                 if (promise.studentCount > 0) {
                     $scope.count = promise.studentCount;
                     $scope.studentList = promise.studentlist;
                     console.log($scope.studentList);
                 }
                 else {
                     swal("No records found for selected academicYear,class and section");
                     $scope.count = 0;
                 }
             }
         });
        }
        if ($scope.radioval == 'Staff') {
            var id = 1;
            $scope.departmentdropdown = [];
            $scope.Designation_types = [];
            $scope.deptcheck = false;
            $scope.desgcheck = false;
            $scope.employeelst = [];
            $scope.stfmsg = "";
            apiService.getURI("SendSMS/Getdepartment", id).
     then(function (promise) {
         $scope.departmentdropdown = promise.departmentdropdown;
     })
        }
    }

    $scope.all_checkdep = function () {
        
        var toggleStatus = $scope.deptcheck;
        angular.forEach($scope.departmentdropdown, function (itm) {
            itm.selected = toggleStatus;
        });
        $scope.get_designation();
    }

    $scope.get_designation = function () {
        $scope.deptcheck = $scope.departmentdropdown.every(function (options) {
            return options.selected;
        });
        
        $scope.get_designationnew();
    }
    $scope.get_designationnew = function () {
        $scope.desgcheck = "";
        var groupidss;
        for (var i = 0; i < $scope.departmentdropdown.length; i++) {
            if ($scope.departmentdropdown[i].selected == true) {

                if (groupidss == undefined)
                    groupidss = $scope.departmentdropdown[i].hrmD_Id;
                else
                    groupidss = groupidss + "," + $scope.departmentdropdown[i].hrmD_Id;
            }
        }

        if (groupidss != undefined) {
            var data = {
                "multipledep": groupidss,
            }
            apiService.create("SendSMS/get_designation", data).
            then(function (promise) {
                
                $scope.Designation_types = promise.designationdropdown;
            })
        }
        else {
            $scope.Designation_types = "";
            $scope.Employeelst = "";
        }
    }

    $scope.all_checkdesg = function () {
        
        var toggleStatus = $scope.desgcheck;
        angular.forEach($scope.Designation_types, function (itm) {
            itm.selected = toggleStatus;
        });
        $scope.get_employee();
    }

    //fill desg end
    //fill employee start
    $scope.get_employee = function () {
        $scope.desgcheck = $scope.Designation_types.every(function (options) {

            return options.selected;
        });
        
        $scope.get_employeenew();
    }
    $scope.get_employeenew = function () {
        $scope.stf = false;
        $scope.employeelst = [];
        var deptIds;
        for (var i = 0; i < $scope.departmentdropdown.length; i++) {
            if ($scope.departmentdropdown[i].selected == true) {

                if (deptIds == undefined)
                    deptIds = $scope.departmentdropdown[i].hrmD_Id;
                else
                    deptIds = deptIds + "," + $scope.departmentdropdown[i].hrmD_Id;
            }
        }
        var groupidss;
        for (var i = 0; i < $scope.Designation_types.length; i++) {
            if ($scope.Designation_types[i].selected == true) {

                if (groupidss == undefined)
                    groupidss = $scope.Designation_types[i].hrmdeS_Id;
                else
                    groupidss = groupidss + "," + $scope.Designation_types[i].hrmdeS_Id;
            }
        }
        if (groupidss != undefined) {
            var data = {
                "multipledes": groupidss,
                "multipledep": deptIds
            }
            apiService.create("SendSMS/get_employee", data).
            then(function (promise) {
                
                $scope.employeelst = promise.stafflist;
                if ($scope.employeelst.length > 0) {
                    $scope.stf = true;
                }
            })
        }
        else {
            $scope.employeelst = "";
        }
    }

    $scope.GetStudentDetails = function () {
        var data = {
            "ASMAY_Id": $scope.ASMAY_Id,
            "ASMCL_Id": $scope.ASMCL_Id,
            "ASMS_Id": $scope.asmS_Id
        }
        apiService.create("SendSMS/GetStudentDetails/", data).
   then(function (promise) {

       if (promise.studentCount > 0) {
           $scope.count = promise.studentCount;
           $scope.studentList = promise.studentlist;
       } else {
           swal("No records found for selected academicYear,class and section");
           $scope.studentList = "";
           $scope.count = 0;
       }
   });
        }

        $scope.stfsearchValue = '';
    $scope.order = function (keyname) {
        $scope.sortKey = keyname;
        $scope.reverse = !$scope.reverse;
    };
    $scope.submitted = false;
    $scope.SendData = function () {
        
        if ($scope.myForm.$valid) {
            var Send = {
                "Mobno": $scope.mobno,
                "mes": $scope.mes,
                "radiotype": $scope.radioval
            }
            apiService.create("SendSMS/savedetail", Send).then(function (promise) {
                if (promise.smsStatus == 'sent') {
                    swal("SMS Sent Successfully.");
                    $state.reload();
                }
                else {
                    swal("SMS Sending Cancelled.");
                }
            });
        }
        else {
            $scope.submitted = true;
        }
    }

    $scope.SendMSG = function (stdmsg) {
        //alert($scope.radioval)
        
        $scope.printstudents = [];
        angular.forEach($scope.studentList, function (user) {
            if (!!user.selected) $scope.printstudents.push(user);
        })
        
        if ($scope.printstudents.length > 0) {
            swal({
                title: "Are you sure?",
                text: "Do you want to Send SMS?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, Send it!",
                cancelButtonText: "Cancel!!!!!!",
                closeOnConfirm: false,
                closeOnCancel: true
            },
         function (isConfirm) {
             if (isConfirm) {

                 var data = {
                     studentlistdto: $scope.printstudents,
                     "SmsMailText": stdmsg,
                     "radiotype": $scope.radioval
                 };
                 apiService.create("SendSMS/savedetail", data).then(function (promise) {
                     if (promise.emailStatus == 'sent') {
                         swal("SMS/EMAIL Sent Successfully.");
                         $state.reload();
                     }
                     else {
                         swal("SMS/EMAIL Sending Cancelled.");
                     }
                 });
             }
         });

        } else {
            swal("Kindly select atleast a record to procced..!");
            return;
        }
    }


    $scope.SendStaffData = function (stfmsg) {
        //alert($scope.radioval)
        
        $scope.printstudents = [];
        angular.forEach($scope.employeelst, function (employee) {
            if (employee.hrmE_MobileNo!=null) {
                if (!!employee.selected) $scope.printstudents.push(employee);
            }
           
        })
        
        if ($scope.printstudents.length > 0) {
            swal({
                title: "Are you sure?",
                text: "Do you want to Send SMS?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, Send it!",
                cancelButtonText: "Cancel!!!!!!",
                closeOnConfirm: false,
                closeOnCancel: true
            },
         function (isConfirm) {
             if (isConfirm) {
                 
                 var data = {
                     studentlistdto: $scope.printstudents,
                     "SmsMailText": stfmsg,
                     "radiotype": $scope.radioval
                 };
                 apiService.create("SendSMS/savedetail", data)
                 .then(function (promise) {
                     if (promise.smsStatus == 'sent') {
                         swal("SMS/EMAIL Sent Successfully.");
                         $state.reload();
                     }
                     else {
                         swal("SMS/EMAIL Sending Cancelled.");
                     }
                 });
             }
         });

        } else {
            swal("Kindly select atleast a record to having  mobile number to procced..!");
            return;
        }
    }

    $scope.onSelectclass = function (classId) {
        if ($scope.ASMAY_Id > 0 && $scope.ASMCL_Id > 0 && $scope.asmS_Id > 0) {
            $scope.GetStudentDetails();
        }
    }
    $scope.onSelectyear = function () {
        if ($scope.ASMAY_Id > 0 && $scope.ASMCL_Id > 0 && $scope.asmS_Id > 0) {
            $scope.GetStudentDetails();
        }
    }
    $scope.printstudents = [];
    $scope.toggleAll = function () {
        var toggleStatus = $scope.all;
        angular.forEach($scope.studentList, function (itm) {
            itm.selected = toggleStatus;
            if ($scope.all == true) {
                if ($scope.printstudents.indexOf(itm) === -1) {
                    $scope.printstudents.push(itm);
                }
            }
            else {
                $scope.printstudents.splice(itm);
            }
        });
    }

    $scope.printstudents = [];
    $scope.toggleAll1 = function () {
        var toggleStatus = $scope.Stflist;
        angular.forEach($scope.employeelst, function (itm) {
            itm.selected = toggleStatus;
            if ($scope.Stflist == true) {
                if ($scope.printstudents.indexOf(itm) === -1) {
                    $scope.printstudents.push(itm);
                }
            }
            else {
                $scope.printstudents.splice(itm);
            }
        });
    }

    $scope.optionToggled = function (SelectedStudentRecord, index) {
        $scope.all = $scope.employeelst.every(function (itm)
        { return itm.selected; });
        if ($scope.printstudents.indexOf(SelectedStudentRecord) === -1) {
            $scope.printstudents.push(SelectedStudentRecord);
        }
        else {
            $scope.printstudents.splice($scope.printstudents.indexOf(SelectedStudentRecord), 1);
        }
    }

    $scope.interacted = function (field) {
        return $scope.submitted;
    };
    $scope.interacted1 = function (field1) {
        return $scope.submitted1;
    };
    $scope.interacted2 = function (field2) {
        return $scope.submitted2;
    };

        $scope.searchValue = '';
    $scope.cancel = function () {
        $scope.mobno = "";
        $scope.mes = "";
        $state.reload();
    }
    $scope.cancel1 = function () {

        $scope.stdmsg = "";
        
        $scope.all = false;  
        angular.forEach($scope.studentList, function (user) {         
          user.selected = false;
        })
    }
    $scope.cancel2 = function () {
        $scope.stfmsg = "";
        $scope.Stflist = false;
        angular.forEach($scope.departmentdropdown, function (user) {
            user.selected = false;
        })
        $scope.employeelst = [];
        $scope.desgcheck = false;
        $scope.stf = false;
        $scope.Designation_types = [];     
    }
    }
})();