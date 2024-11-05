
(function () {
    'use strict';
    angular
        .module('app')
        .controller('StaffLoginController', StaffLoginController);

    StaffLoginController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q', '$cookieStore', '$stateParams', '$filter', 'FormSubmitter', '$window', 'superCache']
    function StaffLoginController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $q, $cookieStore, $stateParams, $filter, FormSubmitter, $window, superCache) {


        $scope.startextramarks = function () {
            window.open("https://gotoschool.extramarks.com/");
        };

        $scope.MIId = "0";
        $scope.userPrivileges = "";
        var pageid = $stateParams.pageId;
        var privlgs = JSON.parse(localStorage.getItem("privileges"));
        for (var i = 0; i < privlgs.length; i++) {
            if (privlgs[i].pageId == pageid) {
                $scope.userPrivileges = privlgs[i];
            }
        }
        $scope.allll = false;
        $scope.modalclick = function () {
            $scope.search55 = "";
            $('#myModal').modal({ backdrop: 'static', keyboard: false })
        }
        $scope.search4455 = '';
        $scope.modalchangeusername = function () {
            $scope.submitted = false;
            $scope.showotpno = true;
            $scope.verifyotp = false;
            $scope.updateshow = true;
            $scope.newuserr = "";
            $scope.forgetEmailOTPno = "";
            $('#myModaluser').modal({ backdrop: 'static', keyboard: false })
        }
        $scope.adddelte = "Add";

        $scope.showstaff = false;
        //$scope.enablestaff = false;
        $scope.catshow = false;
        $scope.showmodulecat = true;
        $scope.changeusername = function (curuser, newuser) {
            if (newuser != '') {
                if (newuser.length > 3) {
                    if ($scope.User_Name == newuser) {
                        swal("Current Username and New Username Are Same!");
                    }
                    else {
                        var data = {
                            "curuser": $scope.User_Name,
                            "newuser": newuser,
                        }
                        apiService.create("StaffLogin/updateuser", data).
                            then(function (promise) {
                                if (promise.returnMsg == 'Success') {
                                    $scope.onchange($scope.HRME_Id.hrmE_Id);
                                    $('#myModaluser').modal('hide');
                                    swal("Username Changed successfully!!");
                                }
                                else {
                                    swal(promise.returnMsg);
                                }

                            })
                    }
                }
                else {
                    swal("Username must have minimum 4 characters!!");
                }
            }
            else {
                swal("Enter New Username!!");
            }
        }

        $scope.verifyotp = false;
        $scope.showotpno = true;
        $scope.updateshow = true;
        //$scope.pagesrecord = {};
        //$scope.savedpagesrecord = {};
        $scope.disableuser = false;
        $scope.userrole = false;
        $scope.savedgridd = [];
        $scope.tempsecondgrid = [];
        $scope.catdiv = false;
        $scope.moduleview = false;
        $scope.userview = false;
        $scope.disableint = false;
        $scope.gridviewdlete = false;

        $scope.institutiondiv = false;
        $scope.trustdiv = false;
        $scope.staffnamediv = false;

        $scope.searchlist = false;
        $scope.adds = {};
        var stuDelRecord = {};

        var selection = {};
        $scope.SelectedStudentRecord = {};
        $scope.deleteselectedrecord = {};

        $scope.page1 = "page1";
        $scope.page2 = "page2";
        $scope.page3 = "page3";
        $scope.page4 = "page4";
        $scope.page5 = "page5";
        $scope.page8 = "page8";
        $scope.page9 = "page9";
        $scope.page10 = "page10";
        $scope.page11 = "page11";
        $scope.page12 = "page12";



        $scope.currentPage4 = 1;
        $scope.itemsPerPage4 = 10;

        $scope.currentPage11 = 1;
        $scope.itemsPerPage11 = 10;

        $scope.currentPage12 = 1;
        $scope.itemsPerPage12 = 10;

        $scope.currentPage10 = 1;
        $scope.itemsPerPage10 = 10;

        $scope.currentPage9 = 1;
        $scope.itemsPerPage9 = 10;

        $scope.currentPage1 = 1;
        $scope.itemsPerPage1 = 10;

        $scope.currentPage2 = 1;
        $scope.itemsPerPage2 = 10;

        $scope.currentPage5 = 1;
        $scope.itemsPerPage5 = 10;

        $scope.currentPage3 = 1;
        $scope.itemsPerPage3 = 10;

        $scope.currentPage8 = 1;
        $scope.itemsPerPage8 = 10;
        $scope.reverse1 = true;

        $scope.pagelistgrid = false;

        $scope.modulefill = function () {
            var pageid = 2;


            apiService.getURI("StaffLogin/getalldetails", pageid).
                then(function (promise) {

                    $scope.trustname = promise.fillorganisation;
                    $scope.institutionname = promise.fillinstitution;
                    $scope.institutionnameload = promise.fillinstitution;
                    $scope.roletype = promise.fillroletype;
                    $scope.modulename = promise.fillmodule;

                    $scope.staffname = promise.fillstaff;
                    $scope.fillstaffusers = promise.fillstaffusers;
                    $scope.categoryName = promise.fillcategory;

                    $scope.gridview1 = promise.showgrid1;
                    //if (promise.thirdgriddata != null) {
                    //    $scope.searchlist = true;
                    //    $scope.thirdgriddata = promise.thirdgriddata;

                    //    $scope.totalItems = $scope.thirdgriddata.length;
                    //    $scope.numPerPage = 5;
                    //}
                    $scope.secondgrid = [];
                });

            $scope.sort = function (keyname) {
                $scope.sortKey = keyname;   //set the sortKey to the param passed
                $scope.reverse = !$scope.reverse; //if true make it false and vice versa
            }

        };


        //$scope.staffenable = function (staff) {

        //    if (staff == true) {
        //        roleflag = "S"
        //        $scope.enablestaff = staff;
        //    }
        //    else {
        //        $scope.enablestaff = staff;
        //        roleflag == "U"
        //    }
        //}

        $scope.changeinstitutionadmin = function (institutionnamefrom) {
            $scope.catshow = false;
            $scope.showmodulecat = true;
            $scope.HRME_Id = "";
            $scope.secondgrid = [];
            $scope.gridview1 = [];
            $scope.clearalldroprole();
            var institutionid = [];
            $scope.multipleinstitution = [];

            for (var i = 0; i < institutionnamefrom.length; i++) {
                if (institutionnamefrom[i].MIId == true) {
                    institutionid = institutionnamefrom[0].mI_Id
                }
            }

            for (var i = 0; i < institutionnamefrom.length; i++) {
                if (institutionnamefrom[i].MIId == true) {
                    $scope.multipleinstitution.push(institutionnamefrom[i])
                }
            }

            if (institutionid != 0) {
                var data1 = {
                    "MI_Id": institutionid,
                    "rolenamess": rolename,
                    multipleinsti: $scope.multipleinstitution,
                    "flag": roleflag
                };

                apiService.create("StaffLogin/changeinsti", data1).
                    then(function (promise) {

                        if (promise.fillcategory != null && promise.fillcategory.length > 0) {
                            $scope.catdiv = true;
                            $scope.categoryName = promise.fillcategory;
                        }
                        else {
                            $scope.catdiv = false;
                        }

                        if (promise.fillmodule.length > 0) {
                            $scope.modulename = promise.fillmodule;
                            $scope.moduleview = true;
                        }
                        else {
                            $scope.moduleview = false;
                            swal("Kindly map module with selected Institution")
                            $state.reload();
                        }
                        //Super admin start
                        if (promise.thirdgriddata.length > 0) {
                            //$scope.searchlist = false;
                            $scope.thirdgriddata = promise.thirdgriddata;
                        }
                        else {
                            //$scope.searchlist = false;
                        }
                        //

                        if (promise.fillstaff.length > 0) {
                            $scope.staffname = promise.fillstaff;
                            $scope.fillstaffusers = promise.fillstaffusers;
                        }
                        if (promise.studentDetails.length > 0) {
                            $scope.employeelistadmin = promise.studentDetails
                            $scope.showviewadmin = true;
                        }

                    })
            }

        }


        $scope.changeinstitution = function (institutionid) {
            $scope.catshow = false;
            $scope.showmodulecat = true;
            $scope.secondgrid = [];
            $scope.gridview1 = [];
            $scope.HRME_Id = "";
            $scope.clearalldroprole();
            if (institutionid != 0) {
                var data1 = {
                    "MI_Id": institutionid,
                    "rolenamess": rolename,
                    "IVRMRT_Id": $scope.IVRMRT_Id,
                    multipleinsti: [],
                    "flag": roleflag
                };

                apiService.create("StaffLogin/changeinsti", data1).
                    then(function (promise) {

                        if (promise.fillcategory.length > 0) {
                            $scope.catdiv = true;
                            $scope.categoryName = promise.fillcategory;
                        }
                        else {
                            $scope.catdiv = false;
                        }

                        if (promise.fillmodule.length > 0) {
                            $scope.modulename = promise.fillmodule;
                            $scope.moduleview = true;
                        }
                        else {
                            $scope.moduleview = false;
                            swal("Kindly map module with selected Institution")
                            $state.reload();
                            //$scope.modulename = promise.fillmodule; $scope.searchlist = false;
                        }

                        //super admin only show
                        if (promise.thirdgriddata.length > 0) {
                            //$scope.searchlist = true;
                            $scope.thirdgriddata = promise.thirdgriddata;
                        }
                        else {
                            //$scope.searchlist = false;
                        }
                        //
                        if (promise.fillstaff.length > 0) {
                            $scope.staffname = promise.fillstaff;
                            $scope.fillstaffusers = promise.fillstaffusers;
                        }

                        if (promise.studentDetails.length > 0) {
                            $scope.employeelist = promise.studentDetails
                            $scope.showview = true;
                        }
                    })
            }

        }

        $scope.predicate = 'sno';
        $scope.reverse = false;
        $scope.reverse2 = false;
        $scope.reverse5 = false;
        $scope.reverse4 = false;
        $scope.currentPage = 1;
        $scope.order = function (predicate) {
            $scope.reverse = ($scope.predicate === predicate) ? !$scope.reverse : false;
            $scope.predicate = predicate;
        };


        //Staff Search Dropdown
        $scope.searchfilter = function (objj) {

            if (objj.search.length >= '2') {

                //var data = {
                //    "searchfilter": objj.search,
                //    "MI_Id": $scope.MIId,

                //}
                //var config = {
                //    headers: {
                //        'Content-Type': 'application/json;'
                //    }
                //}

                //apiService.create("StaffLogin/searchfilter", data).
                //    then(function (promise) {

                //$scope.staffname = promise.fillstaff;

                angular.forEach($scope.staffname, function (objectt) {
                    if (objectt.stafname.length > 0) {
                        var string = objectt.stafname;
                        objectt.stafname = string.replace(/  +/g, ' ');
                    }
                })
                //})
            }

        }

        //save

        //-----Employee name selection change
        $scope.onchange = function (studentlst) {
            $scope.clearalldroStaff();
            var data = {
                "IVRMSTAUL_Id": $scope.HRME_Id.hrmE_Id,
                "MI_Id": $scope.MIId,
                "rolenamess": rolename,
                "IVRMRT_Id": $scope.IVRMRT_Id,
            }
            apiService.create("StaffLogin/getstudata", data).
                then(function (promise) {
                    if (promise.singleemployee.length > 0) {
                        $scope.singleemployee = promise.singleemployee
                        angular.forEach($scope.singleemployee, function (itm) {
                            $scope.User_Name = itm.user_Name;
                            $scope.disableuser = true;
                            $scope.usrerolename = promise.usrerolename;
                            $scope.userrole = true;
                        });
                        $scope.userview = true;
                        angular.forEach(promise.categaryids, function (itm1) {
                            angular.forEach($scope.categoryName, function (itm) {
                                //if (itm1.amC_Id == itm.amC_Id) {
                                itm.catid = true;
                                //}
                            })
                        });
                        $scope.catshow = false;
                        angular.forEach(promise.moduleexistid, function (itm1) {
                            angular.forEach($scope.modulename, function (itm) {
                                if (itm1.ivrmM_Id == itm.ivrmM_Id) {
                                    itm.model = true;
                                }
                            })
                        });
                    }
                    else {
                        $scope.User_Name = "";
                        $scope.disableuser = false;
                        $scope.userview = true;
                        $scope.userrole = false;
                    }
                    if (promise.savedgrid.length > 0) {
                        $scope.savgrid = true;
                        $scope.gridview2 = false;
                        $scope.savebtnview = true;
                        $scope.savedgridd = promise.savedgrid;
                    }
                    if (promise.showgrid1 != null && promise.showgrid1.length > 0) {
                        $scope.pagelistgrid = true;
                        $scope.gridview1 = promise.showgrid1;

                    }

                    if (promise.thirdgriddatamobile.length > 0) {
                        $scope.thirdgrid = promise.thirdgriddatamobile;
                    }
                    else {
                        $scope.thirdgrid = [];
                    }
                    if (promise.previousgrid.length > 0) {
                        $scope.previousgrid = promise.previousgrid;
                    }
                    else {
                        $scope.previousgrid = [];
                    }

                    if ($scope.gridview1.length > 0) {
                        $scope.pagelistgrid = true;
                        $scope.savgrid = true;
                    }
                    else {
                        $scope.pagelistgrid = false;
                        $scope.savgrid = false;
                    }

                    if (promise.savedgrid.length > 0) {
                        $scope.savgrid = true;
                    }
                    else {
                        $scope.savgrid = false;
                    }
                });
        };

        $scope.multipleinstitutionf = [];
        $scope.onchangeuser = function (studentlst) {
            if ($scope.HRME_Id === undefined || $scope.HRME_Id === "") {
                for (var i = 0; i < $scope.institutionname.length; i++) {
                    if ($scope.institutionname[i].MIId === true) {
                        $scope.MIId = $scope.institutionname[i].mI_Id,
                            $scope.multipleinstitutionf.push($scope.institutionname[i]);
                    }
                }
                var data = {
                    "MI_Id": $scope.MIId,
                    "rolenamess": rolename,
                    "IVRMRT_Id": $scope.IVRMRT_Id,
                    "User_Name": studentlst,
                    multipleinsti: $scope.multipleinstitutionf
                };
                apiService.create("StaffLogin/onchangeuser", data).then(function (promise) {
                    if (promise.noUserName === true) {
                        $scope.User_Name = promise.user_Name_exact;
                        $scope.disableuser = true;
                        $scope.usrerolename = promise.usrerolenameexact;
                        $scope.userrole = true;
                        $scope.userview = true;
                        $scope.disableint = true;
                        if (promise.usrerolenameexact === promise.rolenamess) {
                            angular.forEach(promise.categaryids, function (itm1) {
                                angular.forEach($scope.categoryName, function (itm) {
                                    itm.catid = true;
                                });
                            });
                            $scope.catshow = false;
                            angular.forEach(promise.moduleexistid, function (itm1) {
                                angular.forEach($scope.modulename, function (itm) {
                                    if (itm1.ivrmM_Id === itm.ivrmM_Id) {
                                        itm.model = true;
                                    }
                                });
                            });
                            if (promise.savedgrid.length > 0) {
                                $scope.savgrid = true;
                                $scope.gridview2 = false;
                                $scope.savebtnview = true;
                                $scope.savedgridd = promise.savedgrid;
                                $scope.thirdgrid = promise.thirdgriddatamobile;
                                if (promise.previousgrid.length > 0) {
                                    $scope.previousgrid = promise.previousgrid;
                                }
                            }
                            if (promise.showgrid1 != null && promise.showgrid1.length > 0) {
                                $scope.pagelistgrid = true;
                                $scope.gridview1 = promise.showgrid1;
                            }

                            if ($scope.gridview1.length > 0) {
                                $scope.pagelistgrid = true;
                                $scope.savgrid = true;
                            }
                            else {
                                $scope.pagelistgrid = false;
                                $scope.savgrid = false;
                            }

                            if (promise.savedgrid.length > 0) {
                                $scope.savgrid = true;
                            }
                            else {
                                $scope.savgrid = false;
                            }
                            $scope.showmodulecat = true;
                        }
                        else {
                            $scope.showmodulecat = false;
                        }
                    }
                    else {
                        $scope.savgrid = false;
                        $scope.pagelistgrid = false;
                        $scope.savedgridd = [];
                        $scope.gridview1 = [];
                        $scope.gridview2 = false;
                        $scope.disableint = false;
                    }
                });
            }
        };


        $scope.multionchangeuser = function () {
            if ($scope.adddelte == "Del") {
                $scope.albumNameArrayMulti1 = [];
                $scope.deletesavedgrid = [];
                $scope.deletesavedgrid2 = [];
                $scope.secondgridmobiledelete = [];
                $scope.search1 = "";
                angular.forEach($scope.fillstaffusers, function (role) {
                    if (role.selected === true) $scope.albumNameArrayMulti1.push(role);
                });
                var data = {
                    "MI_Id": $scope.MIId,
                    "rolenamess": rolename,
                    "IVRMRT_Id": $scope.IVRMRT_Id,
                    multiplestaff: $scope.albumNameArrayMulti1
                };
                apiService.create("StaffLogin/multionchangeuser", data).
                    then(function (promise) {
                        if (promise.savedgrid != null || promise.previousgrid != null) {
                            if (promise.savedgrid.length > 0 || promise.previousgrid.length > 0) {
                                if (promise.savedgrid.length > 0) {
                                    $scope.savedgridd = promise.savedgrid;
                                    $scope.savgrid = true;
                                }
                                else {
                                    $scope.savedgridd = {};
                                    $scope.savgrid = false;
                                }
                                if (promise.previousgrid.length > 0) {
                                    $scope.previousgrid = promise.previousgrid;
                                }
                                else {
                                    $scope.previousgrid = {};
                                }
                            }
                            else {
                                $scope.savgrid = false;
                                $scope.savedgridd = [];
                                $scope.previousgrid = [];
                            }
                        }
                        else {
                            $scope.savgrid = false;
                            $scope.savedgridd = [];
                            $scope.previousgrid = [];
                        }
                    });
            }
        };


        $scope.Toggle_addheader1 = function () {

            var toggleStatus2 = $scope.all2;
            angular.forEach($scope.gridview1, function (itm) {
                itm.ivrmrP_AddFlag = toggleStatus2;
            });
        }
        $scope.Toggle_add = function () {

            $scope.all2 = $scope.gridview1.every(function (itm) { return itm.ivrmrP_AddFlag; });
        }

        $scope.Toggle_Deleteheader1 = function () {

            var toggleStatus2 = $scope.allDelete;
            angular.forEach($scope.gridview1, function (itm) {
                itm.ivrmrP_DeleteFlag = toggleStatus2;
            });
        }

        $scope.Toggle_Delete = function () {

            $scope.allDelete = $scope.gridview1.every(function (itm) { return itm.ivrmrP_DeleteFlag; });
        }

        $scope.Toggle_Updateheader1 = function () {

            var toggleStatus2 = $scope.allUpdate;
            angular.forEach($scope.gridview1, function (itm) {
                itm.ivrmrP_UpdateFlag = toggleStatus2;
            });
        }

        $scope.Toggle_Update = function () {

            $scope.allUpdate = $scope.gridview1.every(function (itm) { return itm.ivrmrP_UpdateFlag; });
        }

        $scope.Toggle_Processheader1 = function () {

            var toggleStatus2 = $scope.allProcess;
            angular.forEach($scope.gridview1, function (itm) {
                itm.ivrmrP_ProcessFlag = toggleStatus2;
            });
        }

        $scope.Toggle_Process = function () {

            $scope.allProcess = $scope.gridview1.every(function (itm) { return itm.ivrmrP_ProcessFlag; });
        }


        $scope.Toggle_Reportheader1 = function () {

            var toggleStatus2 = $scope.allReport;
            angular.forEach($scope.gridview1, function (itm) {
                itm.ivrmrP_ReportFlag = toggleStatus2;
            });
        }

        $scope.Toggle_Report = function () {

            $scope.allReport = $scope.gridview1.every(function (itm) { return itm.ivrmrP_ReportFlag; });
        }











        //

        $scope.ordermasterPage = function (predicate) {
            $scope.reverse4 = ($scope.predicate === predicate) ? !$scope.reverse4 : false;
            $scope.predicate = predicate;
        };

        //
        $scope.orderDefault = function (predicate) {
            $scope.reverse2 = ($scope.predicate === predicate) ? !$scope.reverse2 : false;
            $scope.predicate = predicate;
        };

        // list 

        $scope.orders = function (predicate) {
            $scope.reverse1 = !$scope.reverse1;
            $scope.sortKey = predicate;
        };

        $scope.paginate = function (value) {
            var begin, end, index;
            begin = ($scope.currentPage - 1) * $scope.numPerPage;
            end = begin + $scope.numPerPage;
            index = $scope.thirdgriddata.indexOf(value);
            return (begin <= index && index < end);
        };

        $scope.checkusernameduplicate = function (username) {
            if (username != "" && username != undefined) {
                var data1 = {
                    "MI_Id": $scope.MIId,
                    "User_Name": username
                };
                apiService.create("StaffLogin/checkdupli", data1).
                    then(function (promise) {
                        if (promise.returnval != "") {
                            swal(promise.returnval);
                            $scope.User_Name = "";
                            $scope.amc_id = "";
                        }
                    })

            }

        }

        $scope.changetrust = function (trustid) {
            $scope.HRME_Id = "";
            $scope.disableint = false;
            var data = {
                "MO_Id": trustid,
            };

            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            };

            apiService.create("StaffLogin/checktrust", data).
                then(function (promise) {
                    $scope.institutionname = promise.fillinstitution;
                });
        };

        $scope.getstaffmobilepages = function () {
            var data = {
                "MO_Id": 1,
                "IVRMRT_Id": $scope.IVRMRT_Id,
                "MI_Id": $scope.MIId
            };
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            };

            apiService.create("StaffLogin/getstaffmobilepages", data).
                then(function (promise) {
                    if (promise.thirdgriddatamobileMulti.length > 0) {
                        $scope.thirdgrid = promise.thirdgriddatamobileMulti;
                    }
                    else {
                        $scope.thirdgrid = {};
                    }
                });
        };

        $scope.temptermarray = [];
        $scope.multipleinstitution = [];
        $scope.modulechange = function (modulename, categoryName, institutionnamefrom) {

            if ($scope.myForm.$valid) {
                $scope.formvalid = true;
            }
            else {
                $scope.formvalid = false;
            }

            if ($scope.formvalid == true || $scope.filterdata === "Multiple") {
                // $scope.clearalldropmodule();
                $scope.gridview1 = [];
                $scope.temptermarray = [];
                var catid;
                $scope.tempsecondgrid = $scope.secondgrid;
                $scope.secondgrid = [];
                $scope.albumNameArray = [];
                angular.forEach(modulename, function (role) {
                    if (role.model == true) $scope.albumNameArray.push(role);
                })
                $scope.gridview1 = [];
                $scope.multipleinstitution = [];
                if (roleflag == "A") {
                    if (institutionnamefrom != null) {
                        for (var i = 0; i < institutionnamefrom.length; i++) {
                            if (institutionnamefrom[i].MIId == true) {
                                $scope.multipleinstitution.push(institutionnamefrom[i])
                            }
                        }
                    }
                }

                if (roleflag == "S") {
                    var data = {
                        "MI_Id": $scope.MIId,
                        "TempararyArrayList": $scope.albumNameArray,
                        "IVRMRT_Id": $scope.IVRMRT_Id,
                        "IVRMSTAUL_Id": $scope.HRME_Id.hrmE_Id,
                        "User_Name": $scope.User_Name,
                        "AMC_Id": catid,
                        "rolenamess": rolename,
                        multipleinsti: $scope.multipleinstitution
                    };
                }
                else if (roleflag == "A") {
                    var data = {

                        "TempararyArrayList": $scope.albumNameArray,
                        "IVRMRT_Id": $scope.IVRMRT_Id,
                        "User_Name": $scope.User_Name,
                        "rolenamess": rolename,
                        multipleinsti: $scope.multipleinstitution
                    };
                }
                else {

                    for (var i = 0; i < categoryName.length; i++) {
                        if (categoryName[i].catid == true) {
                            catid = categoryName[i].amC_Id
                        }
                    }

                    var data = {
                        "MI_Id": $scope.MIId,
                        "TempararyArrayList": $scope.albumNameArray,
                        "IVRMRT_Id": $scope.IVRMRT_Id,
                        "User_Name": $scope.User_Name,
                        "rolenamess": rolename

                    };
                }


                apiService.create("StaffLogin/getpagedetailsrolemodulewise", data).
                    then(function (promise) {
                        if (promise.savedgrid != null && promise.savedgrid.length > 0) {

                            if (promise.savedgrid != null) {
                                $scope.savedgridd = promise.savedgrid;
                                $scope.savgrid = true;
                                $scope.gridview2 = false;
                                $scope.savebtnview = true;
                            }
                        }
                        if (promise.showgrid1 != null && promise.showgrid1.length > 0) {
                            $scope.pagelistgrid = true;
                            $scope.gridview1 = promise.showgrid1;

                            for (var i = 0; i < promise.showgrid1.length; i++) {
                                $scope.temptermarray.push(promise.showgrid1[i]);
                            }

                            angular.forEach($scope.tempsecondgrid, function (qwe1) {
                                angular.forEach($scope.temptermarray, function (qwe) {
                                    if (qwe1.ivrmimP_Id == qwe.ivrmimP_Id) {
                                        qwe.checked = true;
                                        $scope.secondgrid.push(qwe1);
                                    }
                                })
                            });
                            $scope.gridview1 = $scope.temptermarray;
                        }
                        else if (promise.returnval != "") {
                            //swal(promise.returnval);
                            // $scope.IVRMRT_Id = "";
                            $scope.myForm.$setPristine();
                            $scope.myForm.$setUntouched();
                        }

                        if ($scope.secondgrid.length > 0) {
                            $scope.gridview2 = true;
                        }
                        else {
                            $scope.gridview2 = false;
                        }

                        if ($scope.gridview1.length > 0) {
                            $scope.pagelistgrid = true;
                            //$scope.savgrid = true;
                        }
                        else {
                            $scope.pagelistgrid = false;
                            $scope.savgrid = false;
                        }

                        if (promise.savedgrid.length > 0) {
                            $scope.savgrid = true;
                        }
                        else {
                            $scope.savgrid = false;
                        }

                    });


                $scope.all2 = false;
                $scope.allDelete = false;
                $scope.allUpdate = false;
                $scope.allProcess = false;
                $scope.allReport = false;
                $scope.Addsaveall2 = false;
                $scope.Deletesaveall2 = false;
                $scope.Updatesaveall2 = false;
                $scope.Processsaveall2 = false;
                $scope.Reportsaveall2 = false;
                //$scope.all2 = $scope.gridview1.every(function (itm)
                for (var i = 0; i < $scope.gridview1.length; i++) {

                    if ($scope.gridview1[i].ivrmrP_AddFlag == true) {
                        $scope.all2 = true;
                    }
                    else {
                        $scope.all2 = false;
                        break;

                    }
                }
                for (var i = 0; i < $scope.gridview1.length; i++) {

                    if ($scope.gridview1[i].ivrmrP_DeleteFlag == true) {
                        $scope.allDelete = true;
                    }
                    else {
                        $scope.allDelete = false;
                        break;

                    }
                }

                for (var i = 0; i < $scope.gridview1.length; i++) {

                    if ($scope.gridview1[i].ivrmrP_UpdateFlag == true) {
                        $scope.allUpdate = true;
                    }
                    else {
                        $scope.allUpdate = false;
                        break;

                    }
                }

                for (var i = 0; i < $scope.gridview1.length; i++) {

                    if ($scope.gridview1[i].ivrmrP_ProcessFlag == true) {
                        $scope.allProcess = true;
                    }
                    else {
                        $scope.allProcess = false;
                        break;

                    }
                }
                for (var i = 0; i < $scope.gridview1.length; i++) {

                    if ($scope.gridview1[i].ivrmrP_ReportFlag == true) {
                        $scope.allReport = true;
                    }
                    else {
                        $scope.allReport = false;
                        break;

                    }
                }
            }
            else {

                $scope.submitted = true;
                angular.forEach($scope.modulename, function (qwe) {
                    qwe.model = false;

                });
            }

        }

        $scope.clearalldroprole = function () {
            $scope.gridview1 = [];
            $scope.savedgridd = [];
            $scope.secondgrid = [];
            $scope.secondgridmobile = [];
            $scope.secondgriddelete = [];
            $scope.thirdgriddatamobile = [];
            $scope.secondgridmobiledelete = [];
            $scope.User_Name = "";
            $scope.disableuser = false;
            $scope.staffname = [];
            $scope.modulename = [];
            $scope.categoryName = [];
            $scope.pagelistgrid = false;
            $scope.savgrid = false;
            $scope.gridview2 = false;
            $scope.employeelist = [];
            $scope.showview = false;
            $scope.showviewadmin = false;
            $scope.employeelistadmin = [];
            $scope.userrole = false;
            $scope.previousgrid = [];
            $scope.secondgridmobile = [];
            $scope.thirdgrid = [];
            $scope.filterdata = "Single";
        }

        $scope.clearalldropmodule = function () {
            $scope.secondgrid = [];
            $scope.pagelistgrid = false;
            $scope.savgrid = false;
            $scope.gridview2 = false;
        }

        $scope.clearalldropreset = function () {
            $state.reload();
        }

        $scope.clearalldroStaff = function () {
            $scope.gridview1 = [];
            $scope.savedgridd = [];
            $scope.secondgrid = [];
            $scope.secondgridmobile = [];
            $scope.secondgriddelete = [];
            $scope.thirdgriddatamobile = [];
            $scope.secondgridmobiledelete = [];
            $scope.deletesavedgrid = [];
            $scope.previousgrid = [];
            $scope.previousgrid = [];
            $scope.thirdgrid = [];
            $scope.User_Name = "";
            $scope.userrole = false;
            $scope.disableuser = false;
            $scope.pagelistgrid = false;
            $scope.savgrid = false;
            $scope.gridview2 = false;
            angular.forEach($scope.modulename, function (role) {
                role.model = false;
            });
            angular.forEach($scope.categoryName, function (role) {
                role.catid = false;
            })
        }

        $scope.savgrid = false;
        var rolename;
        var roleflag;
        $scope.selectrole = function (roleid, categoryName, modulename, gridview1, institutionnamefrom) {
            //$scope.showstaff = true;
            $scope.pagelistgrid = false;
            $scope.secondgrid = {};
            $scope.gridview2 = false;
            $scope.clearalldroprole();
            $scope.fillstaffusers = {};
            $scope.MIId = "0";
            $scope.institutionname = $scope.institutionnameload;
            $scope.MO_Id = "";
            $scope.catdiv = false;
            $scope.moduleview = false;
            $scope.catshow = false;
            $scope.showmodulecat = true;
            $scope.HRME_Id = "";
            //if (institutionname != null) {
            //    for (var i = 0; i < institutionname.length; i++) {
            //        institutionname[i].MIId = false
            //    }
            //}


            if (categoryName != null) {
                for (var i = 0; i < categoryName.length; i++) {
                    categoryName[i].catid = false
                }
            }

            for (var i = 0; i < modulename.length; i++) {
                modulename[i].IVRMM_Id = false;
            }

            if (gridview1 != null) {
                for (var i = 0; i < gridview1.length; i++) {
                    gridview1[i].checked = false;
                }
            }


            for (var i = 0; i < $scope.roletype.length; i++) {
                if ($scope.roletype[i].ivrmrT_Id == roleid) {
                    rolename = $scope.roletype[i].ivrmrT_Role;
                    roleflag = $scope.roletype[i].flag;
                    $scope.rolenameof = rolename;

                }
            }
            if (roleflag == "V") {
                $scope.searchlist = true;
                $scope.institutiondiv = true;
            }
            else {
                $scope.searchlist = false;
            }

            if (roleflag == "T") {
                $scope.institutiondiv = true;
                $scope.trustdiv = true;
                $scope.staffnamediv = false;
                $scope.institutiondivadmin = false;
                $scope.userview = true;
            }

            if (roleflag == "A") {
                $scope.institutiondivadmin = true;
                $scope.trustdiv = true;
                $scope.staffnamediv = false;
                $scope.institutiondiv = false;
                $scope.userview = true;
            }

            else if (roleflag != "S") {
                $scope.institutiondiv = true;
                $scope.trustdiv = false;
                $scope.staffnamediv = false;
                $scope.institutiondivadmin = false;
                $scope.userview = true;
            }

            else if (roleflag == "S") {
                $scope.institutiondiv = true;
                $scope.trustdiv = false;
                $scope.staffnamediv = true;
                $scope.institutiondivadmin = false;
            }


            //if ($scope.MI_Id == "" || $scope.MI_Id == undefined) {
            //    swal('Institution Not selected');
            //    $scope.MI_Id == ""
            //    $scope.IVRMRT_Id = "";
            //    $scope.myForm.$setPristine();
            //    $scope.myForm.$setUntouched();
            //    return;
            //}
            //if ($scope.HRME_Id == "" || $scope.HRME_Id == undefined) {
            //    swal('Staff Not selected');
            //    $scope.HRME_Id == ""
            //    $scope.IVRMRT_Id = "";
            //    $scope.myForm.$setPristine();
            //    $scope.myForm.$setUntouched();

            //    return;
            //}
            //if ($scope.IVRMM_Id == "" || $scope.IVRMM_Id == undefined) {
            //    swal('Module Not selected');
            //    $scope.IVRMM_Id == ""
            //    $scope.IVRMRT_Id = "";
            //    $scope.myForm.$setPristine();
            //    $scope.myForm.$setUntouched();
            //    return;
            //}
            //if ($scope.amc_id == "" || $scope.amc_id == undefined) {
            //    swal('Category Not selected');
            //    $scope.amc_id == ""
            //    $scope.IVRMRT_Id = "";
            //    $scope.myForm.$setPristine();
            //    $scope.myForm.$setUntouched();

            //    return;
            //}

            if ($scope.IVRMRT_Id == "" || $scope.IVRMRT_Id == undefined) {
                swal('Role Type Not selected');
                return;
            }

            var data = {
                "MI_Id": $scope.MIId,
                "IVRMM_Id": $scope.IVRMM_Id,
                "IVRMRT_Id": $scope.IVRMRT_Id,
                "IVRMSTAUL_Id": $scope.HRME_Id.hrmE_Id,
                "User_Name": $scope.User_Name,
                "AMC_Id": $scope.amc_id,
                "rolenamess": rolename
            };

            apiService.create("StaffLogin/getpagedetailsrolemodulewise", data).
                then(function (promise) {
                    if (promise.showgrid1 != null && promise.showgrid1.length > 0) {
                        $scope.savgrid = true;
                        $scope.gridview2 = false;
                        $scope.savebtnview = true;
                        $scope.savedgridd = promise.savedgrid;
                    }
                    if (promise.showgrid1 != null && promise.showgrid1.length > 0) {
                        $scope.pagelistgrid = true;
                        $scope.gridview1 = promise.showgrid1;
                    }
                    else if (promise.returnval != "") {
                        //swal(promise.returnval);
                        // $scope.IVRMRT_Id = "";
                        $scope.myForm.$setPristine();
                        $scope.myForm.$setUntouched();
                    }
                });
        };
        $scope.isOptionsRequired = function () {
            return !$scope.categoryName.some(function (options) {
                return options.catid;
            });
        }
        $scope.savebtnview = false;
        $scope.gridview2 = false;

        //$scope.addtocart = function (SelectedStudentRecord, index) {
        //    $scope.gridview2 = true;
        //    $scope.savebtnview = true;
        //    $scope.secondgrid[index] = SelectedStudentRecord;
        //};

        //$scope.addtocart = function (SelectedStudentRecord, savedgridd, index) {
        //    $scope.gridview2 = true;
        //    $scope.btns = true;

        //    if (savedgridd != undefined) {
        //        var valid;
        //        for (var i = 0; i < savedgridd.length; i++) {
        //            if (savedgridd[i].ivrmimP_Id == SelectedStudentRecord.ivrmimP_Id) {
        //                swal("Already this page is saved with Current role and Module..Kindly select other pages")
        //                valid = "committ";
        //                SelectedStudentRecord.checked = false;
        //                // $scope.disablecheck[index] = true;
        //            }
        //        }

        //        if (valid != "committ") {
        //            if ($scope.secondgrid.indexOf(SelectedStudentRecord) === -1) {
        //                $scope.secondgrid.push(SelectedStudentRecord);
        //            }
        //            else {
        //                $scope.secondgrid.splice($scope.secondgrid.indexOf(SelectedStudentRecord), 1);
        //            }
        //        }
        //    }
        //    else {
        //        if ($scope.secondgrid.indexOf(SelectedStudentRecord) === -1) {
        //            $scope.secondgrid.push(SelectedStudentRecord);
        //            $scope.savebtnview = true;
        //        }
        //        else {
        //            $scope.secondgrid.splice($scope.secondgrid.indexOf(SelectedStudentRecord), 1);
        //            $scope.savebtnview = true;
        //        }
        //    }
        //};



        //$scope.addtocart = function (record, previouslysaveddata, index) {
        //    $scope.gridview2 = true;
        //    $scope.btns = true;
        //    if ($scope.secondgrid.indexOf(record) === -1) {
        //        var valid;

        //        if (previouslysaveddata != "undefined") {
        //            for (var i = 0; i < previouslysaveddata.length; i++) {
        //                //if (previouslysaveddata[i].ivrmimP_Id == record.ivrmimP_Id) {
        //                //    swal("Already this page is saved with Current role and Module..Kindly select other pages")
        //                //    valid = "committ";
        //                //    record.checked = false;
        //                //    // $scope.disablecheck[index] = true;
        //                //}
        //            }
        //        }


        //        if (valid != "committ") {
        //            if ($scope.secondgrid.indexOf(record) === -1) {
        //                $scope.secondgrid.push(record);
        //                $scope.savebtnview = true;
        //            }
        //            else {
        //                $scope.secondgrid.splice($scope.secondgrid.indexOf(record), 1);
        //            }
        //        }
        //    }
        //    else {
        //        $scope.secondgrid.splice($scope.secondgrid.indexOf(record), 1);
        //    }
        //};

        $scope.all_check = function (ack) {
            $scope.checkall = ack;
            var toggleStatusst = $scope.checkall;
            angular.forEach($scope.fillstaffusers, function (uem) {
                uem.selected = toggleStatusst;
            });

            if ($scope.adddelte == "Del") {
                $scope.multionchangeuser();
            }
        };

        $scope.clearsinglestaff = function (stafftype) {
            if (stafftype === "Multiple") {
                $scope.userview = false;
                $scope.User_Name = "";
                $scope.disableuser = false;
                $scope.checkall = false;
                $scope.userrole = false;
                $scope.pagelistgrid = false;
                $scope.gridview1 = {};
                $scope.thirdgrid = {};
                $scope.previousgrid = {};
                $scope.savgrid = false;
                $scope.savedgridd = {};
                $scope.gridviewdlete = false;
                $scope.gridview2 = false;
                $scope.secondgrid = {};
                $scope.searchValueE = "";
                angular.forEach($scope.fillstaffusers, function (uem) {
                    uem.selected = false;
                });
                angular.forEach($scope.modulename, function (uem) {
                    uem.model = false;
                });
                $scope.search4 = "";
                $scope.search4455 = "";
                $scope.search45 = "";
                $scope.search1 = "";
                $scope.search5 = "";
                $scope.secondgridmobile = [];
                $scope.getstaffmobilepages();
                $scope.deletesavedgrid = [];
                $scope.deletesavedgrid2 = [];
                $scope.secondgridmobiledelete = [];
            }
            else if (stafftype === "Single") {
                $scope.userview = true;
                $scope.User_Name = "";
                $scope.userrole = false;
                $scope.pagelistgrid = false;
                $scope.gridview1 = {};
                $scope.thirdgrid = {};
                $scope.previousgrid = {};
                $scope.savgrid = false;
                $scope.savedgridd = {};
                $scope.gridviewdlete = false;
                $scope.searchValueE = "";
                angular.forEach($scope.fillstaffusers, function (uem) {
                    uem.selected = false;
                });
                angular.forEach($scope.modulename, function (uem) {
                    uem.model = false;
                });
                $scope.search4 = "";
                $scope.search4455 = "";
                $scope.search45 = "";
                $scope.search1 = "";
                $scope.search5 = "";
                $scope.disableuser = false;
                $scope.checkall = false;
                $scope.gridview2 = false;
                $scope.secondgrid = {};
                $scope.secondgridmobile = [];
                $scope.adddelte = "Add";
                $scope.deletesavedgrid = [];
                $scope.deletesavedgrid2 = [];
                $scope.secondgridmobiledelete = [];
            }
        };

        $scope.secondgrid = [];
        $scope.search4 = "";
        $scope.addtocart = function (SelectedStudentRecord, index) {
            $scope.gridview2 = true;
            $scope.btns = true;
            if ($scope.search4 == '') {
                $scope.alll = $scope.gridview1.every(function (itm) { return itm.checked; });

                if (SelectedStudentRecord.checked == true) {
                    angular.forEach($scope.gridview1, function (qwe) {
                        if (qwe.ivrmimP_Id == SelectedStudentRecord.ivrmimP_Id) {

                            $scope.secondgrid.push(qwe);
                        }
                    })
                } else if (SelectedStudentRecord.checked == false) {
                    angular.forEach($scope.secondgrid, function (qwe) {
                        if (qwe.ivrmimP_Id == SelectedStudentRecord.ivrmimP_Id) {

                            $scope.secondgrid.splice($scope.secondgrid.indexOf(qwe), 1);
                        }
                    })

                }

            }

            if ($scope.search4 != '') {
                $scope.alll = $scope.filterValue1master.every(function (itm) { return itm.checked; });
                if (SelectedStudentRecord.checked == true) {
                    angular.forEach($scope.gridview1, function (qwe) {
                        if (qwe.ivrmimP_Id == SelectedStudentRecord.ivrmimP_Id) {

                            $scope.secondgrid.push(qwe);
                        }
                    })
                } else if (SelectedStudentRecord.checked == false) {
                    angular.forEach($scope.secondgrid, function (qwe) {
                        if (qwe.ivrmimP_Id == SelectedStudentRecord.ivrmimP_Id) {

                            //$scope.secondgrid.splice(qwe);
                            $scope.secondgrid.splice($scope.secondgrid.indexOf(qwe), 1);
                        }
                    })

                }
            }
            if ($scope.secondgrid.length > 0) {
                $scope.gridview2 = true;
                $scope.savebtnview = true;
                $scope.savebtnviewofmulti = false;
            }
            else {
                $scope.gridview2 = false;
                $scope.savebtnview = false;
                $scope.savebtnviewofmulti = true;
            };
        };

        $scope.secondgridmobile = [];
        $scope.addtocartmobile = function (SelectedStudentRecord, previousgrid, index) {

            //$scope.gridview2 = true;
            //$scope.btns = true;

            if (previousgrid != undefined) {
                var valid;
                for (var i = 0; i < previousgrid.length; i++) {
                    if (previousgrid[i].ivrmrP_Id == SelectedStudentRecord.ivrmrP_Id) {
                        swal("Already this page is saved with Current role and Module..Kindly select other pages")
                        valid = "committ";
                        SelectedStudentRecord.checked = false;
                        // $scope.disablecheck[index] = true;
                    }
                }

                if (valid != "committ") {
                    if ($scope.secondgridmobile.indexOf(SelectedStudentRecord) === -1) {
                        $scope.secondgridmobile.push(SelectedStudentRecord);
                    }
                    else {
                        $scope.secondgridmobile.splice($scope.secondgridmobile.indexOf(SelectedStudentRecord), 1);
                    }
                }
            }
            else {
                if ($scope.secondgridmobile.indexOf(SelectedStudentRecord) === -1) {
                    $scope.secondgridmobile.push(SelectedStudentRecord);
                }
                else {
                    $scope.secondgridmobile.splice($scope.secondgridmobile.indexOf(SelectedStudentRecord), 1);
                }
            }
            if ($scope.secondgridmobile.length > 0) { $scope.udisable = true }
            else { $scope.udisable = false }

            if ($scope.filterdata == "Multiple") {
                $scope.checkmodule = [];
                angular.forEach($scope.modulename, function (role) {
                    if (role.model == true) $scope.checkmodule.push(role);
                })
                if ($scope.checkmodule.length == undefined || $scope.checkmodule.length == 0) {
                    $scope.savebtnviewofmulti = true;
                }
                else {
                    $scope.savebtnviewofmulti = false;
                }

            }

        };


        $scope.secondgridmobiledelete = [];
        $scope.addtocartmobiledelete = function (SelectedStudentRecord, previousgrid, index) {

            //$scope.gridview2 = true;
            //$scope.btns = true;

            if (previousgrid != undefined) {
                var valid;
                if (valid != "committ") {
                    if ($scope.secondgridmobiledelete.indexOf(SelectedStudentRecord) === -1) {
                        $scope.secondgridmobiledelete.push(SelectedStudentRecord);
                    }
                    else {
                        $scope.secondgridmobiledelete.splice($scope.secondgridmobiledelete.indexOf(SelectedStudentRecord), 1);
                    }
                }
            }
            else {
                if ($scope.secondgridmobiledelete.indexOf(SelectedStudentRecord) === -1) {
                    $scope.secondgridmobiledelete.push(SelectedStudentRecord);
                }
                else {
                    $scope.secondgridmobiledelete.splice($scope.secondgridmobiledelete.indexOf(SelectedStudentRecord), 1);
                }
            }
            if ($scope.secondgridmobiledelete.length > 0) { $scope.udisable = true }
            else { $scope.udisable = false }
        };


        $scope.sendotpsms = function (forgetEmails) {

            if (forgetEmails == null || forgetEmails == '') {
                swal("Enter Mobile Number!!");
            }
            else {
                $scope.forgetEmailOTP = "";
                $scope.emailidforotp = "test@mail";


                var mobno = {
                    "clickedlinkname": 'M',
                    "MI_ID": $scope.MIId,
                    "Mobile": forgetEmails.toString(),
                    "Email": $scope.emailidforotp.toString(),
                }


                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }

                apiService.create("Login/Userupdate", mobno).then(function (promise) {

                    $scope.verifyotp = true;
                    $scope.showotpno = false;
                    swal(promise.message);

                })
            }

        }


        $scope.VerifyforgetEmailOtp = function (data) {

            if (data === "" || data === undefined) {
                swal("Please Enter OTP Number!");
            }
            else {

                var mobno = {
                    "EMAILOTP": data
                }

                apiService.create("Login/VerifyEmailOtpgen", mobno).then(function (promise) {

                    if (promise === "Success") {

                        $scope.updateshow = false;
                        $scope.verifyotp = false;
                    }
                    else if (promise === "Fail") {
                        $scope.updateshow = true;
                        $scope.resendotpbutton = true;
                        $scope.verifyotp = true;
                        swal("OTP Mismatch!");
                    }
                    else {
                        //$('#myModalswal').modal('hide');
                        //$('#myModalotp').modal('show');
                        swal("OTP Expired. Please resend OTP");

                    }
                })
            }
        }

        $scope.resendotp = function (forgetEmailOTPno) {
            $scope.sendotpsms(forgetEmailOTPno);
        }


        //Delete Saved Pages

        $scope.deletesavedgrid = [];
        $scope.deletesavedgrid2 = [];
        $scope.search5 = "";
        $scope.addtoremoovecart = function (SelectedStudentRecord, index) {
            $scope.gridview5 = true;
            $scope.btns = true;
            if ($scope.search5 == '') {
                $scope.alll = $scope.savedgridd.every(function (itm) { return itm.checked; });

                if (SelectedStudentRecord.checked == true) {

                    if ($scope.adddelte == "Del") {
                        $scope.gridviewdlete = true;
                        angular.forEach($scope.savedgridd, function (qwe) {
                            if (qwe.ivrmstauP_Id == SelectedStudentRecord.ivrmstauP_Id) {
                                if ($scope.adddelte == "Del") {
                                    $scope.deletesavedgrid2.push(qwe);
                                    $scope.deletesavedgrid = $scope.deletesavedgrid2;
                                } else {
                                    $scope.deletesavedgrid.push(qwe);
                                    $scope.deletesavedgrid2 = [];
                                }

                            }
                        })
                        $scope.$apply(function () {
                            $scope.deletesavedgrid
                        });
                    }
                    else {
                        swal({
                            title: "Are you sure",
                            text: "Do you want to Remoove Record????",
                            type: "warning",
                            showCancelButton: true,
                            confirmButtonColor: "#DD6B55",
                            confirmButtonText: "Yes, delete it!",
                            cancelButtonText: "Cancel!!!!!!",
                            closeOnConfirm: true,
                            closeOnCancel: true
                        },
                            function (isConfirm) {

                                if (isConfirm) {
                                    $scope.gridviewdlete = true;
                                    angular.forEach($scope.savedgridd, function (qwe) {
                                        if (qwe.ivrmstauP_Id == SelectedStudentRecord.ivrmstauP_Id) {
                                            if ($scope.adddelte == "Del") {
                                                $scope.deletesavedgrid2.push(qwe);
                                                $scope.deletesavedgrid = $scope.deletesavedgrid2;
                                            } else {
                                                $scope.deletesavedgrid.push(qwe);
                                                $scope.deletesavedgrid2 = [];
                                            }

                                        }
                                    })
                                    $scope.$apply(function () {
                                        $scope.deletesavedgrid
                                    });

                                }
                                else {

                                    angular.forEach($scope.savedgridd, function (qwe) {
                                        if (qwe.ivrmstauP_Id == SelectedStudentRecord.ivrmstauP_Id) {

                                            qwe.checked = false;
                                        }
                                    })
                                    $scope.$apply(function () {
                                        $scope.savedgridd;
                                    });
                                }
                            }
                        )
                    }


                } else if (SelectedStudentRecord.checked == false) {
                    angular.forEach($scope.deletesavedgrid, function (qwe) {
                        if (qwe.ivrmstauP_Id == SelectedStudentRecord.ivrmstauP_Id) {

                            $scope.deletesavedgrid.splice($scope.deletesavedgrid.indexOf(qwe), 1);
                        }
                    })

                }

            }

            if ($scope.search5 != '') {
                $scope.alll = $scope.filterValue1master.every(function (itm) { return itm.checked; });
                if (SelectedStudentRecord.checked == true) {
                    swal({
                        title: "Are you sure",
                        text: "Do you want to Remoove Record????",
                        type: "warning",
                        showCancelButton: true,
                        confirmButtonColor: "#DD6B55",
                        confirmButtonText: "Yes, delete it!",
                        cancelButtonText: "Cancel!!!!!!",
                        closeOnConfirm: false,
                        closeOnCancel: false
                    },
                        function (isConfirm) {
                            if (isConfirm) {
                                angular.forEach($scope.savedgridd, function (qwe) {
                                    if (qwe.ivrmimP_Id == SelectedStudentRecord.ivrmimP_Id) {

                                        $scope.deletesavedgrid.push(qwe);
                                    }
                                })
                            }
                            else {
                                swal("Cancelled Successfully");
                            }
                        }
                    )

                } else if (SelectedStudentRecord.checked == false) {
                    angular.forEach($scope.deletesavedgrid, function (qwe) {
                        if (qwe.ivrmimP_Id == SelectedStudentRecord.ivrmimP_Id) {

                            //$scope.secondgrid.splice(qwe);
                            $scope.deletesavedgrid.splice($scope.deletesavedgrid.indexOf(qwe), 1);
                        }
                    })

                }
            }
            if ($scope.deletesavedgrid.length > 0) {
                $scope.gridviewdlete = true;
            }
            else {
                $scope.gridviewdlete = false;
            }

        }

        //

        $scope.toggleAllmaster = function () {
            $scope.secondgrid = [];
            if ($scope.search4 == '') {
                var toggleStatus = $scope.alll;
                angular.forEach($scope.gridview1, function (itm) {
                    itm.checked = toggleStatus;
                    if ($scope.alll == true) {
                        $scope.secondgrid.push(itm);
                    }
                    else {
                        $scope.secondgrid.splice(itm);
                    }
                });
            }

            if ($scope.search4 != '') {
                var toggleStatus = $scope.alll;
                angular.forEach($scope.filterValue1master, function (itm) {
                    itm.checked = toggleStatus;
                    if ($scope.alll == true) {
                        $scope.secondgrid.push(itm);
                    }
                    else {
                        $scope.secondgrid.splice(itm);
                    }
                });
            }
            if ($scope.secondgrid.length > 0) {
                $scope.savebtnview = true;
                $scope.gridview2 = true;
            }
            else {
                $scope.gridview2 = false;
                $scope.savebtnview = false;
            }

        }



        $scope.toggleAllmobilemaster = function (allll) {
            $scope.secondgridmobile = [];
            if ($scope.search4455 == '') {
                var toggleStatus = allll;
                angular.forEach($scope.thirdgrid, function (itm) {
                    itm.checked = toggleStatus;
                    if (allll == true) {
                        $scope.secondgridmobile.push(itm);
                    }
                    else {
                        $scope.secondgridmobile.splice(itm);
                    }
                });
            }

            if ($scope.search4455 != '') {
                var toggleStatus = allll;
                angular.forEach($scope.thirdgrid, function (itm) {
                    itm.checked = toggleStatus;
                    if (allll == true) {
                        $scope.secondgridmobile.push(itm);
                    }
                    else {
                        $scope.secondgridmobile.splice(itm);
                    }
                });
            }
            //if ($scope.secondgridmobile.length > 0) {
            //    $scope.savebtnview = true;
            //    $scope.gridview2 = true;
            //}
            //else {
            //    $scope.gridview2 = false;
            //    $scope.savebtnview = false;
            //}

        }




        $scope.currenrow = {};
        $scope.deletesecondgriddatamobile = function (index, stuDelRecord, currenrow) {



            swal({
                title: "Are you sure",
                text: "Do you want to delete record????",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55",
                confirmButtonText: "Yes, delete it!",
                cancelButtonText: "Cancel!!!!!!",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        stuDelRecord.checked = false;

                        $scope.$apply(function () {
                            //$scope.alll = $scope.thirdgrid.every(function (itm) { return itm.checked; });
                            //$scope.secondgridmobile.splice($scope.secondgridmobile.indexOf(stuDelRecord), 1);

                            stuDelRecord.checked = false;
                            $scope.allll = $scope.thirdgrid.every(function (itm) { return itm.checked; });
                            if ($scope.secondgridmobile.indexOf(stuDelRecord) === -1) {
                                $scope.secondgridmobile.push(stuDelRecord);

                            }
                            else {
                                $scope.secondgridmobile.splice($scope.secondgridmobile.indexOf(stuDelRecord), 1);
                            }
                        });
                        swal("Row as been removed from list Successfully");
                    }
                    else {
                        swal("Cancelled Successfully");
                    }
                });




        };

        //$scope.addtocartdelete = function (record) {
        //    if ($scope.secondgriddelete.indexOf(record) === -1) {
        //        var valid;
        //        if (valid != "committ") {
        //            if ($scope.secondgriddelete.indexOf(record) === -1) {
        //                $scope.secondgriddelete.push(record);
        //            }
        //            else {
        //                $scope.secondgriddelete.splice($scope.secondgriddelete.indexOf(record), 1);
        //            }
        //        }
        //    }
        //    else {
        //        $scope.secondgriddelete.splice($scope.secondgriddelete.indexOf(record), 1);
        //    }
        //};

        $scope.printoff = function () {
            var innerContents = document.getElementById("myModalprint").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +

                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/hutchings/Hutchingspdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +

                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        }

        $scope.secondgriddelete = [];
        $scope.selected = function (SelectedStudentRecord, index) {
            if ($scope.searchValue == '') {
                $scope.all = $scope.thirdgriddata.every(function (itm) { return itm.checked; });
                if ($scope.secondgriddelete.indexOf(SelectedStudentRecord) === -1) {
                    $scope.secondgriddelete.push(SelectedStudentRecord);

                }
                else {
                    $scope.secondgriddelete.splice($scope.secondgriddelete.indexOf(SelectedStudentRecord), 1);
                }
            }

            if ($scope.searchValue != '') {
                $scope.all = $scope.filterValue1.every(function (itm) { return itm.checked; });
                if ($scope.secondgriddelete.indexOf(SelectedStudentRecord) === -1) {
                    $scope.secondgriddelete.push(SelectedStudentRecord);
                }
                else {
                    $scope.secondgriddelete.splice($scope.secondgriddelete.indexOf(SelectedStudentRecord), 1);
                }
            }
            if ($scope.secondgriddelete.length > 0) {
                $scope.searchlistdelete = true;
            }
            else {
                $scope.searchlistdelete = false;
            }

        }

        $scope.toggleAll = function () {
            $scope.secondgriddelete = [];
            if ($scope.searchValue == '') {
                var toggleStatus = $scope.all;
                angular.forEach($scope.thirdgriddata, function (itm) {
                    itm.checked = toggleStatus;
                    if ($scope.all == true) {
                        $scope.secondgriddelete.push(itm);
                    }
                    else {
                        $scope.secondgriddelete.splice(itm);
                    }
                });
            }

            if ($scope.searchValue != '') {
                var toggleStatus = $scope.all;
                angular.forEach($scope.filterValue1, function (itm) {
                    itm.checked = toggleStatus;
                    if ($scope.all == true) {
                        $scope.secondgriddelete.push(itm);
                    }
                    else {
                        $scope.secondgriddelete.splice(itm);
                    }
                });
            }
            if ($scope.secondgriddelete.length > 0) {
                $scope.searchlistdelete = true;
            }
            else {
                $scope.searchlistdelete = false;
            }

        }





        $scope.currenrow = {};
        //$scope.deletesecondgriddata = function (index, stuDelRecord, currenrow) {

        //    //var currentrowid = currenrow.seconduser.ivrmmP_Id

        //    $scope.newDataList = {};
        //    var i = 0;

        //    angular.forEach(stuDelRecord, function () {
        //        if (i !== index) {
        //            // $scope.newDataList.push(stuDelRecord[i])
        //            $scope.newDataList[i] = $scope.secondgrid[i];
        //        }
        //        i++;
        //    });

        //    $scope.secondgrid = $scope.newDataList;

        //};

        //$scope.deletesecondgriddata = function (index, stuDelRecord) {


        //    stuDelRecord.checked = false;
        //    $scope.secondgrid.splice($scope.secondgrid.indexOf(stuDelRecord), 1);

        //};

        $scope.checkAll = function () {
            $scope.grid1.roles = angular.copy($scope.roles);
        };


        $scope.deletesecondgriddata = function (index, SelectedStudentRecord) {
            SelectedStudentRecord.checked = false;
            $scope.alll = $scope.gridview1.every(function (itm) { return itm.checked; });
            if ($scope.secondgrid.indexOf(SelectedStudentRecord) === -1) {
                $scope.secondgrid.push(SelectedStudentRecord);

            }
            else {
                $scope.secondgrid.splice($scope.secondgrid.indexOf(SelectedStudentRecord), 1);
            }
            if ($scope.secondgrid.length > 0) {
                $scope.gridview2 = true;
                $scope.savebtnview = true;
            }
            else {
                $scope.gridview2 = false;
                $scope.savebtnview = false;
            }
        }


        $scope.deletrec = function () {
            var data = {
                "MI_Id": $scope.MIId,
                "TempararyArrayListdelete": $scope.secondgriddelete
            };
            swal({
                title: "Are you sure?",
                text: "Do you want to delete record !!!!!!!!",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, delete it!",
                cancelButtonText: "Cancel!!!!!!",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("StaffLogin/deletemodpages", data).
                            then(function (promise) {
                                $scope.thirdgriddata = promise.thirdgriddata;
                                $scope.secondgriddelete = [];
                                $scope.searchlistdelete = false;
                                swal(promise.returnval)
                            });
                    }
                    else {
                        swal("Record Deletion Cancelled");
                    }
                });
        };


        $scope.interacted = function (field) {

            return $scope.submitted;
        };

        $scope.clickall = function (pagesrecord) {
            console.log("dfgt");
        };

        var catid;
        $scope.submitted = false;
        $scope.multipleinstitution = [];
        $scope.mobilepages = [];
        $scope.savadata = function (pagesrecord, savedpagesrecord, categoryName, institutionnamefrom, deletesavedgrid, modulename) {

            if ($scope.adddelte !== "Del") {
                $scope.ArrayMulti = [];
                if ($scope.filterdata == "Multiple") {
                    angular.forEach($scope.fillstaffusers, function (role) {
                        if (role.selected == true) $scope.ArrayMulti.push(role);
                    });
                    if ($scope.ArrayMulti.length == undefined || $scope.ArrayMulti.length == 0) {
                        swal("Select Any One Employee!");
                        return;
                    }
                }
                $scope.multipleinstitution = [];
                if (roleflag === "A") {
                    if (institutionnamefrom !== null) {
                        for (var i = 0; i < institutionnamefrom.length; i++) {
                            if (institutionnamefrom[i].MIId === true) {
                                $scope.multipleinstitution.push(institutionnamefrom[i])
                            }
                        }
                    }
                }
                $scope.albumNameArray = [];
                angular.forEach(modulename, function (role) {
                    if (role.model === true) $scope.albumNameArray.push(role);
                });
                $scope.submitted = true;
                var array = $.map(pagesrecord, function (value, index) {
                    return [value];
                });
                var mobilearray = $.map($scope.secondgridmobile, function (value, index) {
                    return [value];
                });
                $scope.newmobilearray = [];
                if (mobilearray !== null && mobilearray.length > 0) {
                    angular.forEach(mobilearray, function (tt) {
                        $scope.newmobilearray.push({
                            ivrmmaP_Id: tt.ivrmmaP_Id, ivrmmaP_AddFlg: tt.ivrmmaP_AddFlg, ivrmmaP_UpdateFlg: tt.ivrmmaP_UpdateFlg, ivrmmaP_DeleteFlg: tt.ivrmmaP_DeleteFlg
                        });
                    });
                }

                var mobilearraydelete = $.map($scope.secondgridmobiledelete, function (value, index) {
                    return [value];
                });
                var updatemobileprevileges = $.map($scope.previousgrid, function (value, index) {
                    return [value];
                });
                if (savedpagesrecord !== undefined) {
                    var savedarray = $.map(savedpagesrecord, function (value, index) {
                        return [value];
                    });
                }
                if (deletesavedgrid !== undefined) {
                    var deletedarray = $.map(deletesavedgrid, function (value, index) {
                        return [value];
                    });
                }
                if (roleflag === "U") {
                    if (savedpagesrecord !== undefined) {
                        var data = {
                            "MI_Id": $scope.MIId,
                            "User_Name": $scope.User_Name,
                            "IVRMRT_Id": $scope.IVRMRT_Id,
                            savetmpdata: array,
                            savetmpdatamobile: $scope.newmobilearray,
                            updatemobilepagespre: $scope.previousgrid,
                            deletmobile: mobilearraydelete,
                            datasaved: savedarray,
                            deletesaved: deletesavedgrid
                        };
                    }
                }
                else if (roleflag === "A") {
                    data = {
                        "MI_Id": $scope.MIId,
                        "User_Name": $scope.User_Name,
                        "IVRMRT_Id": $scope.IVRMRT_Id,
                        "amc_id": catid,
                        savetmpdata: array,
                        savetmpdatamobile: $scope.newmobilearray,
                        updatemobilepagespre: $scope.previousgrid,
                        deletmobile: mobilearraydelete,
                        datasaved: savedarray,
                        multipleinsti: $scope.multipleinstitution,
                        deletesaved: deletesavedgrid,
                        "TempararyArrayList": $scope.albumNameArray
                    };
                }
                else {

                    for (var i = 0; i < categoryName.length; i++) {
                        if (categoryName[i].catid === true) {
                            catid = categoryName[i].amC_Id;
                        }
                    }

                    if (savedpagesrecord !== undefined) {

                        if ($scope.filterdata === "Single") {
                            data = {
                                "MI_Id": $scope.MIId,
                                "IVRMSTAUL_Id": $scope.HRME_Id.hrmE_Id,
                                "User_Name": $scope.User_Name,
                                "IVRMRT_Id": $scope.IVRMRT_Id,
                                //"amc_id": $scope.amc_id,
                                "amc_id": 0,
                                savetmpdata: array,
                                savetmpdatamobile: $scope.newmobilearray,
                                updatemobilepagespre: $scope.previousgrid,
                                deletmobile: mobilearraydelete,
                                datasaved: savedarray,
                                deletesaved: deletesavedgrid,
                                "TempararyArrayList": $scope.albumNameArray
                            };
                        }
                        else if ($scope.filterdata === "Multiple") {
                            $scope.albumNameArrayMulti = [];

                            angular.forEach($scope.fillstaffusers, function (role) {
                                if (role.selected === true) $scope.albumNameArrayMulti.push(role);
                            })
                            data = {
                                "MI_Id": $scope.MIId,
                                "IVRMSTAUL_Id": 0,
                                "User_Name": $scope.User_Name,
                                "IVRMRT_Id": $scope.IVRMRT_Id,
                                //"amc_id": $scope.amc_id,
                                "amc_id": catid,
                                "singlemulti": $scope.filterdata,
                                savetmpdata: array,
                                multiplestaff: $scope.albumNameArrayMulti,
                                savetmpdatamobile: $scope.newmobilearray,
                                //updatemobilepagespre: $scope.previousgrid,
                                "TempararyArrayList": $scope.albumNameArray
                            };
                        }

                    }
                }
                if (savedpagesrecord === undefined) {
                    data = {
                        "MI_Id": $scope.MIId,
                        "IVRMSTAUL_Id": $scope.HRME_Id.hrmE_Id,
                        "User_Name": $scope.User_Name,
                        "IVRMRT_Id": $scope.IVRMRT_Id,
                        "amc_id": $scope.amc_id,
                        savetmpdata: array,
                        savetmpdatamobile: $scope.newmobilearray,
                        updatemobilepagespre: $scope.previousgrid,
                        deletmobile: mobilearraydelete,
                        datasaved: "",
                        deletesaved: deletesavedgrid
                    };
                }
                apiService.create("StaffLogin/", data).
                    then(function (promise) {

                        //$scope.students = promise.fillpagesdata;

                        if (promise.returnval == 'User already exist for this username!!!') {
                            swal(promise.returnval);
                        }
                        else {
                            swal(promise.returnval)
                            $state.reload();
                        }


                        //if (promise.returnval === true) {
                        //    swal('Record Saved/Updated Successfully.UserName & Password is sent to Email Id', 'success');
                        //    $state.reload();
                        //}
                        //else if (promise.returnval === false)
                        //{
                        //    swal("Username already exists for the Selected institution and staff");
                        //}
                        //else {
                        //    swal('Record Not Saved/Updated Successfully', 'Failed');
                        //}

                    });
            }
            else {
                mobilearraydelete = $.map($scope.secondgridmobiledelete, function (value, index) {
                    return [value];
                });
                data = {
                    deletmobile: mobilearraydelete,
                    deletesaved: deletesavedgrid
                };
                apiService.create("StaffLogin/multiuserdeletpages", data).
                    then(function (promise) {
                        if (promise.returnval == "true") {
                            swal("Pages Removed Successfully!");
                            $scope.clearsinglestaff($scope.filterdata);
                        }
                        else {
                            swal("Pages Not Removed!");
                        }
                    });

            }
        };
        $scope.clearForm = function () {
            $state.reload();
        };
    }

})();
