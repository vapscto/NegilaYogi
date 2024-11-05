(function () {
    'use strict';
    angular
        .module('app')
        .controller('EnquiryController', EnquiryController)

    EnquiryController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache']
    function EnquiryController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {

        //$scope.sortKey = 'pamS_Id';
        $scope.sortReverse = true;

        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }

        $scope.obj = {};
        $scope.ddate = {};
        $scope.ddate = new Date();

        $scope.usrname = localStorage.getItem('username');
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }

        $scope.imgname = logopath;

        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));
        $scope.obj.usercheckCC = false;
        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.currentPage1 = 1;
            $scope.itemsPerPage = 10
            $scope.itemsPerPage1 = 10
            $scope.currentPage3 = 1;
            $scope.itemsPerPage3 = 10
            $scope.currentPage4 = 1;
            $scope.itemsPerPage4 = 10

            $scope.currentPage5 = 1;
            $scope.itemsPerPage5 = 10
            var pageid = 2;
            apiService.getDATA("Enquiry/getstoragedetails", pageid).
                then(function (promise) {
                    if (promise.rowdata != null && promise.rowdata.length > 0) {
                        $scope.rowdata = promise.rowdata;
                    }
                    if (promise.roledata != null && promise.roledata.length > 0) {
                        $scope.roledata = promise.roledata;
                    }

                    if (promise.mappingdata != null && promise.mappingdata.length > 0) {
                        $scope.mappingdata = promise.mappingdata;
                    }

                    if (promise.userdata != null && promise.userdata.length > 0) {
                        $scope.userdata = promise.userdata;
                    }
                    if (promise.preadmissionmapping != null && promise.preadmissionmapping.length > 0) {
                        $scope.preadmissionmapping = promise.preadmissionmapping;
                    }

                    if (promise.institutionlist != null && promise.institutionlist.length > 0) {
                        $scope.institutionlist = promise.institutionlist;
                    }
                    // $scope.presentCountgrid1 = $scope.subdetailsarray.length;
                    $scope.clearfields();
                    $scope.clearfields_sub();
                })

        }

        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey == key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        }

        $scope.searchsource = function () {
            var data = {
                "PAMS_SourceName": $scope.search,
                "PAMS_SourceDesc": $scope.type
            }
            apiService.create("CLGStatus/1", data).
                then(function (promise) {
                    $scope.pages = promise.pagesdata;
                })
        }

        $scope.searchValue = '';
        $scope.searchValue1 = '';
        $scope.filterValue = function (obj) {

            return (angular.lowercase(obj.ivrM_SD_Access_Name)).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
                (angular.lowercase(obj.ivrM_SD_Access_Key)).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
                (angular.lowercase(obj.ivrM_VMS_Subscription_URL)).indexOf(angular.lowercase($scope.searchValue)) >= 0
        }

        $scope.filterValue2 = function (obj) {

            return (angular.lowercase(obj.ivrmP_Dasboard_PageName)).indexOf(angular.lowercase($scope.searchValue1)) >= 0 ||
                (angular.lowercase(obj.ivrmrT_Role)).indexOf(angular.lowercase($scope.searchValue1)) >= 0
        }
        $scope.filterValue3 = function (obj) {

            return (angular.lowercase(obj.papG_PAGENAME)).indexOf(angular.lowercase($scope.searchValue1)) >= 0 ||
                (angular.lowercase(obj.mI_Id)).indexOf(angular.lowercase($scope.searchValue1)) >= 0
        }




        $scope.editEmployee = {}
        //$scope.deletedata = function (employee, SweetAlert) {
        //    $scope.editEmployee = employee.amcexM_Id;
        //    var pageid = $scope.editEmployee;
        //    swal({
        //        title: "Are you sure?",
        //        text: "Do You Want To Delete Record !!!!!!!!",
        //        type: "warning",
        //        showCancelButton: true,
        //        confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, delete it!",
        //        cancelButtonText: "Cancel!!!!!!",
        //        closeOnConfirm: false,
        //        closeOnCancel: false
        //    },
        //        function (isConfirm) {
        //            if (isConfirm) {
        //                apiService.DeleteURI("Enquiry/deleterecord", pageid).
        //                    then(function (promise) {
        //                        if (promise.returnMsg == "Delete") {
        //                            swal('You Can Not Delete This Record. It Is Already Mapped With Student');
        //                        }
        //                        else {
        //                            if (promise.returnval === true) {
        //                                swal('Record Deleted Successfully');

        //                            }
        //                            else {
        //                                swal('Record Not Deleted Successfully');
        //                            }
        //                        }
        //                        $scope.pages = promise.pagesdata;


        //                        $state.reload();
        //                    })
        //            }
        //            else {
        //                swal("Cancelled");
        //                $state.reload();
        //            }
        //        });
        //}

        $scope.deletedatasec = function (employees, SweetAlert) {
            $scope.editEmployee = employees.ivrM_DBID;
            var pageid = $scope.editEmployee;
            swal({
                title: "Are you sure?",
                text: "Do You Want To Delete Record !!!!!!!!",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, delete it!",
                cancelButtonText: "Cancel!!!!!!",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.getURI("Enquiry/deletemappingrecord", pageid).
                            then(function (promise) {
                                if (promise.returnMsg == "Delete") {
                                    swal('You Can Not Delete This Record. It Is Already Mapped With Student');
                                }
                                else {
                                    if (promise.returnval === true) {
                                        swal('Record Deleted Successfully');
                                        $scope.loaddata();
                                    }
                                    else {
                                        swal('Record Not Deleted Successfully');
                                        $scope.loaddata();
                                    }
                                }

                            })
                    }
                    else {
                        swal("Cancelled");
                        $state.reload();
                    }
                });
        }



        $scope.deletedatasecc = function (papG_ID) {

            var data = {
                "PAPG_ID": papG_ID
            }
            swal({
                title: "Are you sure?",
                text: "Do You Want To Delete Record !",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, delete it!",
                cancelButtonText: "Cancel!",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("Enquiry/deletepremappingrecord", data).
                            then(function (promise) {
                                if (promise.PAPG_ID == "Delete") {
                                    swal('You Can Not Delete This Record. It Is Already Mapped With Student');
                                }
                                else if (promise.returnval === true) {
                                    swal('Record Deleted Successfully');
                                    $scope.loaddata();
                                }
                                else {
                                    swal('Record Not Deleted Successfully');
                                    $scope.loaddata();

                                }

                            })
                    }
                    else {
                        swal("Cancelled");
                        $state.reload();
                    }
                });
        }

        $scope.getorgvalue = function (employee) {
            $scope.editEmployee = employee.ivrM_SD;
            var pageid = $scope.editEmployee;

            apiService.getURI("Enquiry/editstorage", pageid).
                then(function (promise) {
                    if (promise.roweditdata.length > 0) {
                        $scope.IVRM_SD_Access_Name = promise.roweditdata[0].ivrM_SD_Access_Name;
                        $scope.IVRM_SD_Access_Key = promise.roweditdata[0].ivrM_SD_Access_Key;
                        $scope.IVRM_VMS_Subscription_URL = promise.roweditdata[0].ivrM_VMS_Subscription_URL;
                        $scope.IVRM_SD = promise.roweditdata[0].ivrM_SD;

                    }
                })
        }

        $scope.getorgvaluesec = function (rec) {
            $scope.editEmployee = rec.ivrM_DBID;
            var pageid = $scope.editEmployee;

            apiService.getURI("Enquiry/getmappingedit", pageid).
                then(function (promise) {
                    if (promise.mappingeditdata.length > 0) {
                        $scope.obj.IVRMP_Dashboard_PageName = promise.mappingeditdata[0].ivrmP_Dasboard_PageName;
                        $scope.obj.IVRMRT_Role = promise.mappingeditdata[0].ivrmrT_Role;
                        $scope.IVRM_DBID = promise.mappingeditdata[0].ivrM_DBID;
                        //$scope.AMCEXMSUB_SubjectName = promise.mappingeditdata[0].amcexmsuB_SubjectName;
                        //$scope.AMCEXMSUB_MaxMarks = promise.mappingeditdata[0].amcexmsuB_MaxMarks;
                    }

                })
        }


        $scope.getorgvaluesecc = function (papG_ID) {
            var data = {
                "PAPG_ID": papG_ID
            }

            apiService.create("Enquiry/getpremappingedit", data).
                then(function (promise) {
                    if (promise.preadmissionmapping.length > 0) {
                        $scope.obj.PAPG_PAGENAME = promise.preadmissionmapping[0].papG_PAGENAME;
                        $scope.obj.mI_Id = promise.preadmissionmapping[0].papG_MIID;

                    }

                })
        }


        $scope.clearfields = function () {
            $scope.IVRM_SD = "";
            $scope.IVRM_SD_Access_Name = "";
            $scope.IVRM_SD_Access_Key = "";
            $scope.IVRM_VMS_Subscription_URL = "";

            $scope.obj.mI_Id = "";
            $scope.obj.PAPG_PAGENAME = "";
        }
        $scope.clearfields_sub = function () {
            $scope.obj.IVRMRT_Role = "";
            $scope.obj.IVRMP_Dashboard_PageName = "";
            $scope.obj.mI_Id = "";
            $scope.obj.PAPG_PAGENAME = "";


        }
        $scope.clearall = function () {
            $state.reload();
        };

        $scope.submitted = false;
        $scope.savepages = function () {

            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var IVRM_SD = 0;
                if ($scope.IVRM_SD > 0) {
                    IVRM_SD = $scope.IVRM_SD;
                }

                var data = {
                    "IVRM_SD_Access_Name": $scope.IVRM_SD_Access_Name,
                    "IVRM_SD_Access_Key": $scope.IVRM_SD_Access_Key,
                    "IVRM_VMS_Subscription_URL": $scope.IVRM_VMS_Subscription_URL,
                    "IVRM_SD": IVRM_SD

                }
                apiService.create("Enquiry/saveStoragedetail", data).
                    then(function (promise) {
                        if (promise.returnMsg != "") {
                            if (promise.rowdata !== null && promise.rowdata > 0) {
                                swal('Record Inserted Sucessfully! ');
                                $state.reload();
                                return;

                            } else if (promise.returnMsg == 'noupdate') {
                                swal('Already Record exist! You Cannot Insert another.');

                                $state.reload();
                                $scope.clearfields();
                                $scope.clearfields_sub();
                            }
                            else if (promise.returnMsg == "update") {
                                swal('Record Updated Successfully');
                                $state.reload();
                                $scope.clearfields();
                                $scope.clearfields_sub();
                            }
                        } else {
                            swal('Failed To Save/Update Record');
                            $state.reload();
                        }
                    })
            }


        };

        //firsttab
        $scope.submittedsec = false;
        $scope.saveSecondtab = function () {
            $scope.submittedsec = true;
            if ($scope.myForm.$valid) {
                var IVRM_DBID = 0;
                if ($scope.IVRM_DBID > 0) {
                    IVRM_DBID = $scope.IVRM_DBID
                }
                var data = {

                    "IVRMRT_Role": $scope.obj.IVRMRT_Role,
                    "IVRMP_Dasboard_PageName": $scope.obj.IVRMP_Dashboard_PageName,
                    "IVRM_DBID": IVRM_DBID
                }
                apiService.create("Enquiry/saveMappingDetails", data).
                    then(function (promise) {
                        if (promise.returnMsg != "") {
                            if (promise.returnMsg == "duplicate") {
                                swal('Master Source Record Already Exist');                               
                                return;

                            } else if (promise.returnMsg == "add") {
                                swal('Record Saved Successfully');
                                // $state.reload();
                                $scope.loaddata();
                                // clearfields();
                                $scope.clearfields_sub();

                            } else if (promise.returnMsg == "update") {
                                swal('Record Updated Successfully');
                                // $state.reload();
                                $scope.loaddata();
                                // clearfields();
                                $scope.clearfields_sub();
                            }
                        } else {
                            swal('Failed To Save/Update Record');
                           
                        }
                    })
            }
        };

        $scope.submittedsec = false;
        $scope.saveFourthtab = function () {
            $scope.submittedsec = true;

            if ($scope.myForm.$valid) {
                var data = {

                    "PAPG_PAGENAME": $scope.obj.PAPG_PAGENAME,
                    "PAPG_MIID": $scope.obj.mI_Id
                }
                apiService.create("Enquiry/savepreadmissionmapping", data).
                    then(function (promise) {
                        if (promise.returnMsg != "") {
                            if (promise.returnMsg == "duplicate") {
                                swal('Master Source Record Already Exist');
                                return;

                            } else if (promise.returnMsg == "add") {
                                swal('Record Saved Successfully');
                                $state.reload();
                                $scope.loaddata();
                                clearfields();
                                $scope.clearfields_sub();

                            } else if (promise.returnMsg == "update") {
                                swal('Record Updated Successfully');
                                $state.reload();
                                $scope.loaddata();
                                clearfields();
                                $scope.clearfields_sub();
                            }
                        } else {
                            swal('Failed To Save/Update Record');

                        }
                    })
            }
        };


        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.interactedsec = function (field) {
            return $scope.submittedsec;
        };

        // $scope.clearfields_sub = function (field) {


        //third tab

        $scope.type = false;
        $scope.getuser = function (IVRMRT_Roleone) {


            $scope.userlist = [];
            $scope.institutionlist = [];
            $scope.cartdataarray = [];
            $scope.institutionMappedData = [];
            var data = {
                "IVRMRT_Id": IVRMRT_Roleone,

            }

            apiService.create("Enquiry/getuser", data).then(function (promise) {

                if (promise.roleuserdata != null && promise.roleuserdata != undefined) {
                    $scope.userlist = promise.roleuserdata;
                }
                else {
                    $scope.IsHidden1 = false;
                    $scope.showPrintB = false;
                    $scope.showExportB = false;
                    swal("No records Found");
                }

            });

        }
        //$scope.loadthirddata = function () {
        //    var pageid = 2;
        //    apiService.getDATA("Enquiry/loadthirddata", pageid).
        //        then(function (promise) {
        //            if (promise.gridalldata != null && promise.gridalldata.length > 0) {
        //                $scope.gridalldata = promise.gridalldata;
        //            }


        //            // $scope.presentCountgrid1 = $scope.subdetailsarray.length;
        //            //$scope.clearfields();
        //            //$scope.clearfields_sub();
        //        })
        //}


        $scope.al_checkinst = function (all) {

            $scope.institutionlistarray = [];
            $scope.obj.usercheckCC = all;

            var toggleStatus = $scope.obj.usercheckCC;
            angular.forEach($scope.institutionlist, function (role) {
                role.selected = toggleStatus;
            });

            //$scope.institutionlistarray = [];

        }
      
        $scope.Onclickinst = function () {
            var count = 0;
            angular.forEach($scope.institutionlist, function (role) {
              
                if (role.selected == false) {
                    role.selected = false;
                    $scope.obj.usercheckCC = false
                    count += 1;
                }
   
                if (count == 0) {
                    $scope.obj.usercheckCC = true;
                }
                else {
                    $scope.obj.usercheckCC = false;
                }
                  
                 
               
            });
        }
        $scope.isOptionsRequiredinst = function () {
            return !$scope.institutionlist.some(function (item) {
                return item.selected;
            });

        };



        // add to cart
        $scope.getsection = function (ASMCL_Id) {

            $scope.institutionlistarray = [];

            angular.forEach($scope.institutionlist, function (aa) {
                if (aa.selected == true) {
                    $scope.institutionlistarray.push({ MI_Id: aa.mI_Id })
                }

            });

            var data = {
                "classlsttwo": $scope.institutionlistarray,
                "IVRMRT_Id": $scope.ivrmrT_Id,
                "Userid": $scope.id,
            }
            apiService.create("Enquiry/getcartdata", data).then(function (promise) {
                if (promise.sectionlist.length > 0 || promise.sectionlist != null) {
                    $scope.sectionlist = promise.sectionlist;
                    $scope.getclass = false;
                }
                else {
                    swal('No data Found!!!');
                }
            });
            //
        }

       
        $scope.addtocart = function () {

            $scope.institutionlistarray = [];

            var roleName = "";
            var userName = "";
            var miName = "";
            $scope.cartdataarray = [];
            if ($scope.userdata != null && $scope.userdata.length > 0) {
                angular.forEach($scope.userdata, function (qq) {
                    if (qq.ivrmrT_Id == $scope.obj.ivrmrT_Id) {
                        roleName = qq.ivrmrT_Role;
                    }
                });
            }
            if ($scope.userlist != null && $scope.userlist.length > 0) {
                angular.forEach($scope.userlist, function (qq) {
                    if (qq.Id == $scope.obj.userid) {
                        userName = qq.UserName;
                    }
                });
            }

            angular.forEach($scope.institutionlist, function (qq) {
                if (qq.selected == true) {
                    //$scope.institutionlistarray.push({ MI_Id: qq.MI_Id })
                    $scope.cartdataarray.push({
                        IVRMRT_Id: $scope.obj.ivrmrT_Id,
                        Id: $scope.obj.userid,
                        MI_Id: qq.MI_Id,
                        roleName: roleName,
                        userName: userName,
                        miName: qq.MI_Name
                    })
                }
            });


        }


        $scope.deletecartdata = function (index) {
            //var newItemNo = $scope.cartdataarray.length - 1;
            $scope.cartdataarray.splice(index, 1);
            $scope.loaddata();
        }

        $scope.deletegriddata = function (employees) {
            $scope.editEmployee = employees.IVRMULI_Id;
            var pageid = $scope.editEmployee;
            var data={
                "IVRMULI_Id": pageid,
                "Id": $scope.obj.userid,
                "IVRMRT_Id": $scope.IVRMRT_Roleone
            }
            swal({
                title: "Are you sure?",
                text: "Do You Want To Delete Record !!!!!!!!",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, delete it!",
                cancelButtonText: "Cancel!!!!!!",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("Enquiry/deletegriddata", data).
                            then(function (promise) {

                                if (promise.returnMsg === 'Update') {
                                    swal('Record Deleted Successfully');
                                   $state.reload();
                                 //   $scope.getinstitution($scope.usrname);

                                    if (promise.institutionMappedData != null && promise.institutionMappedData != undefined) {
                                        $scope.institutionMappedData = promise.institutionMappedData;
                                    }
                                    if (promise.institutiondata != null && promise.institutiondata != undefined) {
                                        $scope.institutionlist = promise.institutiondata;
                                    }
                                   
                                }
                                else if (promise.returnMsg === 'minimum') {
                                    swal('Atleast One Institution should be mapped');
                                }
                                else {
                                    swal('Record Not Deleted Successfully');
                                    //$state.reload();
                                   // $scope.getinstitution($scope.usrname);
                                   
                                }


                            })
                    }
                    else {
                        swal("Cancelled");
                        $state.reload();
                    }
                });
        }


        $scope.getinstitution = function () {

            var data = {
                "Id": $scope.obj.userid,
                "IVRMRT_Id": $scope.obj.ivrmrT_Id,
                //"type":

            }

            apiService.create("Enquiry/getinstitution", data).then(function (promise) {

                if (promise.institutiondata != null && promise.institutiondata != undefined) {
                    $scope.institutionlist = promise.institutiondata;
                    if (promise.institutionMappedData != null && promise.institutionMappedData != undefined) {
                        $scope.institutionMappedData = promise.institutionMappedData;
                    }

                }
                else {
                    $scope.IsHidden1 = false;
                    $scope.showPrintB = false;
                    $scope.showExportB = false;
                    swal("No records Found");
                }

            });

        }

        $scope.clearthirdtab = function () {
            //$state.reload();

            $scope.obj.ivrmrT_Id = "";
            $scope.obj.userid = "";
            $scope.obj.usercheckCC = false;

            //var toggleStatus = $scope.obj.usercheckCC;
            angular.forEach($scope.institutionlist, function (role) {
                if (role.selected == true) {
                    role.selected = false;
                }
                
            });
            $scope.institutionMappedData = [];
        }
      
        
        $scope.gridflg = false;
        $scope.printdatatable = [];
        $scope.toggleAll = function (all2) {
            $scope.printdatatable = [];
            var toggleStatus = all2;
            angular.forEach($scope.cartdataarray, function (itm) {
                itm.selected = toggleStatus;
                if (all2 == true) {
                    if ($scope.printdatatable.indexOf(itm) === -1) {
                        $scope.printdatatable.push(itm);
                    }
                }
                else {
                    $scope.printdatatable.splice(itm);
                }
                if ($scope.printdatatable.length > 0) {
                    $scope.gridflg = true;
                }
            });

        }


        $scope.selected = function (SelectedStudentRecord, index) {


            $scope.all2 = $scope.cartdataarray.every(function (itm) { return itm.selected; });
            if ($scope.printdatatable.indexOf(SelectedStudentRecord) === -1) {
                $scope.printdatatable.push(SelectedStudentRecord);
            }
            else {
                $scope.printdatatable.splice($scope.printdatatable.indexOf(SelectedStudentRecord), 1);
            }
            if ($scope.printdatatable.length > 0) {
                $scope.gridflg = true;
            }
        }


        $scope.submitted1 = false;
        $scope.savethirdData = function (data, data1) {

            $scope.submitted1 = true;
            if ($scope.myForm.$valid) {

                $scope.albumNameArray = [];
                angular.forEach($scope.cartdataarray, function (user) {
                    if (!!user.selected) {
                        $scope.albumNameArray.push(user);
                    }
                });
                var IVRMULI_Id = 0;
                if ($scope.IVRMULI_Id > 0) {
                    IVRMULI_Id = $scope.IVRMULI_Id
                }
                var datalist = {
                    "Selected_List": $scope.albumNameArray,
                    "IVRMULI_Id": IVRMULI_Id,
                    "Id": $scope.obj.userid,
                    "IVRMRT_Id": $scope.obj.ivrmrT_Id

                }

                apiService.create("Enquiry/saveThirdData", datalist).then(function (promise) {
                   // if (promise.count == 0) {
                    if ( promise.returnMsg == "add") {
                        swal('Record Saved Successfully...!', 'success');
                       // $state.reload();
                        if (promise.institutionMappedData != null && promise.institutionMappedData != undefined) {
                            $scope.institutionMappedData = promise.institutionMappedData;
                        }
                        if (promise.institutiondata != null && promise.institutiondata != undefined) {
                            $scope.institutionlist = promise.institutiondata;
                        }
                        //angular.forEach($scope.institutionlist, function (role) {
                        //    role.empck = false;
                        //});
                        $scope.cartdataarray = [];
                    }
                    else if ( promise.returnMsg == "update") {
                        swal('Record Updated Successfully...!', 'success');
                       // $state.reload();
                        if (promise.institutionMappedData != null && promise.institutionMappedData != undefined) {
                            $scope.institutionMappedData = promise.institutionMappedData;
                        }
                      
                        //angular.forEach($scope.institutionlist, function (role) {
                        //    role.empck = false;
                        //});
                        if (promise.institutiondata != null && promise.institutiondata != undefined) {
                            $scope.institutionlist = promise.institutiondata;
                        }
                        $scope.cartdataarray = [];
                    }
                    else {
                        swal('Status Updated Successfully...!', 'success');
                       // $state.reload();
                        if (promise.institutionMappedData != null && promise.institutionMappedData != undefined) {
                            $scope.institutionMappedData = promise.institutionMappedData;                        
                        }
                        if (promise.institutiondata != null && promise.institutiondata != undefined) {
                            $scope.institutionlist = promise.institutiondata;
                        }
                        $scope.cartdataarray = [];
                        //angular.forEach($scope.institutionlist, function (role) {
                        //    role.empck = false;
                        //});
                       
                    }
                });



            }

            else {
                $scope.submitted1 = true;
            }

        }


        $scope.interactedthird = function (field) {
            return $scope.submitted1 || field.$dirty1;
        };
    }

})();