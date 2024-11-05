(function () {
    'use strict';
    angular.module('app').controller('ImsClientController', ImsClientController)

    ImsClientController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache']
    function ImsClientController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {

        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;
        $scope.search = "";
        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        };
        $scope.allhrmd = false;
        $scope.currentPage2 = 1;
        $scope.itemsPerPage2 = 10;
        $scope.search2 = "";
        $scope.editfalg = false;
        $scope.editfalg1 = false;

        $scope.sort = function (keyname) {
            $scope.sortKey2 = keyname;   //set the sortKey to the param passed
            $scope.reverse2 = !$scope.reverse2; //if true make it false and vice versa
        };
        $scope.allhrmdrr = function (allhrmd) {

            var toggleStatus = allhrmd;
            angular.forEach($scope.institution, function (itm) {
                itm.mI_Idddd = toggleStatus;
            });

        };
        $scope.isRequiredmiBnk = function () {

            return !$scope.institution.some(function (options) {
                return options.mI_Idddd;
            });

        };
        //=========================================Load data
        $scope.Loaddata = function () {
            var pageid = 2;
            apiService.getURI("ImsClient/getdetails", pageid).then(function (promise) {
                $scope.get_clentlist = promise.get_clentlist;
                $scope.cordinartorlist = promise.cordinartorlist;
                $scope.teamleadlist = promise.teamleadlist;
                $scope.clientlist = promise.clientlist;
                $scope.ieEmpList = promise.ieEmpList;
                $scope.institution = promise.institution;
                $scope.allIeMappingdata = promise.allIeMappingdata;
                $scope.MI_Id = promise.mI_Id;

            });
        };

        $scope.OnChangeTab1Inst = function () {
            $scope.get_clentlist = [];
            $scope.search = "";
            $scope.ISMMCLT_ClientName = "";
            $scope.ISMMCLT_ContactNo = "";
            $scope.ISMMCLT_EmailId = "";
            $scope.HRME_Id = "";
            $scope.ISMMCLT_NOName = "";
            $scope.ISMMCLT_NOEmailId = "";
            $scope.leader_id = "";
            $scope.ISMMCLT_Desc = "";
            $scope.ISMMCLT_Address = "";
            $scope.ISMMCLT_RemainderDays = "";
            $scope.ISMMCLT_Code = "";
            $scope.ismmclT_Id = 0;

            var data = {
                "MI_Id": $scope.MI_Id
            };

            apiService.create("ImsClient/OnChangeTab1Inst", data).then(function (promise) {
                $scope.get_clentlist = promise.get_clentlist;

            });
        };


        //============================================Save record
        $scope.submitted = false;
        $scope.saveClientdata = function () {
            $scope.multipleclientstemp = [];
            if ($scope.myForm.$valid) {
                if ($scope.institution != null && $scope.institution.length > 0) {
                    if ($scope.rdopunch == "ALLO") {
                        angular.forEach($scope.institution, function (ss) {
                            if (ss.mI_Idddd == true) {
                                $scope.multipleclientstemp.push({
                                    MI_Id: ss.mI_Id
                                });
                            }
                        })
                    }
                    else {
                        $scope.multipleclientstemp.push({
                            MI_Id: $scope.MI_Id
                        });
                    }
                }
                if ($scope.HRME_Id === undefined || $scope.HRME_Id === null || $scope.HRME_Id === "") {
                    $scope.HRME_Id = 0;
                }
                if ($scope.leader_id === undefined || $scope.leader_id === null || $scope.leader_id === "") {
                    $scope.leader_id = 0;
                }
                var data = {
                    "ISMMCLT_Id": $scope.ismmclT_Id,
                    "ISMMCLT_ClientName": $scope.ISMMCLT_ClientName,
                    "ISMMCLT_ContactNo": $scope.ISMMCLT_ContactNo,
                    "ISMMCLT_EmailId": $scope.ISMMCLT_EmailId,

                    "ISMMCLT_CordinatorId": $scope.HRME_Id.ismmclT_CordinatorId,

                    "ISMMCLT_NOName": $scope.ISMMCLT_NOName,
                    "ISMMCLT_NOEmailId": $scope.ISMMCLT_NOEmailId,

                    "ISMMCLT_NOContactNo": $scope.ISMMCLT_NOContactNo,
                    "ISMMCLT_Desc": $scope.ISMMCLT_Desc,
                    "ISMMCLT_Address": $scope.ISMMCLT_Address,
                    "ISMMCLT_TeamLeadId": $scope.leader_id.ismmclT_TeamLeadId,
                    "ISMMCLT_RemainderDays": $scope.ISMMCLT_RemainderDays,
                    "ISMMCLT_Code": $scope.ISMMCLT_Code,
                    "MI_Id": $scope.MI_Id,
                    "ISMMCLT_GSTNO": $scope.ISMMCLT_GSTNO,
                    "multipleclients": $scope.multipleclientstemp,

                }
                apiService.create("ImsClient/saveClientdata", data).then(function (promise) {
                    if (promise.returnval !== null && promise.duplicate !== null) {
                        if (promise.duplicate == false) {
                            if (promise.returnval == true) {
                                if ($scope.ismmclT_Id > 0) {
                                    swal('Record Updated Successfully!!!');
                                    $scope.clientlist = promise.clientlist;
                                }
                                else {
                                    swal('Record Saved Successfully!!!');
                                    $scope.clientlist = promise.clientlist;
                                }
                            }
                            else {
                                if (promise.returnval == false) {
                                    if ($scope.ismmclT_Id > 0) {
                                        swal('Record Not Updated Successfully!!!');
                                    }
                                    else {
                                        swal('Record Not Saved Successfully!!!');
                                    }
                                }
                            }
                        }
                        else {
                            swal("Record already exist");
                        }

                        $state.reload();
                    }
                    else {
                        swal("Kindly Contact Administrator!!!");
                    }
                });
            }
            else {
                $scope.submitted = true;
            }
        };
        //===============================================End


        //=====================Editrecord
        $scope.editClientdata = function (user) {

            var data = {
                "ISMMCLT_Id": user.ismmclT_Id,
                "MI_Id": $scope.MI_Id
            };

            apiService.create("ImsClient/editClientdata", user).then(function (promise) {
                $scope.editfalg = true;
                $scope.ismmclT_Id = promise.editClient[0].ismmclT_Id;
                $scope.ISMMCLT_ClientName = promise.editClient[0].ismmclT_ClientName;
                $scope.ISMMCLT_ContactNo = promise.editClient[0].ismmclT_ContactNo;
                $scope.ISMMCLT_EmailId = promise.editClient[0].ismmclT_EmailId;
                $scope.ISMMCLT_NOName = promise.editClient[0].ismmclT_NOName;
                $scope.ISMMCLT_NOEmailId = promise.editClient[0].ismmclT_NOEmailId;
                $scope.ISMMCLT_NOContactNo = promise.editClient[0].ismmclT_NOContactNo;
                $scope.ISMMCLT_Address = promise.editClient[0].ismmclT_Address;
                $scope.MI_Id = promise.editClient[0].mI_Id;

                $scope.ISMMCLT_Desc = promise.editClient[0].ismmclT_Desc;
                $scope.ISMMCLT_RemainderDays = promise.editClient[0].ismmclT_RemainderDays;
                $scope.ISMMCLT_Code = promise.editClient[0].ismmclT_Code;
                $scope.ISMMCLT_GSTNO = promise.editClient[0].ismmclT_GSTNO;

                $scope.cordinartorlist = promise.cordinartorlist;
                $scope.teamleadlist = promise.teamleadlist;

                $scope.HRME_Id = promise.editClient[0];
                angular.forEach(promise.editClient, function (cord) {
                    angular.forEach(promise.cordinartorlist, function (emp1) {
                        if (cord.ismmclT_CordinatorId != null || cord.ismmclT_CordinatorId != undefined || cord.ismmclT_CordinatorId != "") {
                            if (emp1.ismmclT_CordinatorId == cord.ismmclT_CordinatorId) {
                                cord.cordinatorname = emp1.cordinatorname;
                            }
                        }
                    });
                });

                $scope.leader_id = promise.editClient[0];
                angular.forEach(promise.editClient, function (edit) {
                    angular.forEach(promise.teamleadlist, function (emp2) {
                        if (edit.ismmclT_TeamLeadId != null || edit.ismmclT_TeamLeadId != undefined || edit.ismmclT_TeamLeadId != "") {
                            if (emp2.ismmclT_TeamLeadId == edit.ismmclT_TeamLeadId) {
                                edit.leadername = emp2.leadername;
                            }
                        }
                    });
                });
            });
        };

        //=================Activation/Deactivation--Record.........
        $scope.clientDecative = function (user, SweetAlert) {

            $scope.ismmclT_Id = user.ismmclT_Id;
            user.MI_Id = $scope.MI_Id;

            var dystring = "";
            if (user.ismmclT_ActiveFlag == 1) {
                dystring = "Deactivate";
            }
            else if (user.ismmclT_ActiveFlag == 0) {
                dystring = "Activate";
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
                        apiService.create("ImsClient/clientDecative", user).then(function (promise) {
                            if (promise.returnval == true) {
                                swal("Record " + dystring + "d Successfully!!!");
                            }
                            else {
                                swal("Record Not " + dystring + "d Successfully!!!");
                            }
                            $state.reload();
                        });
                    }
                    else {
                        swal("Record " + dystring + " Cancelled");
                    }
                });
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        $scope.interacted2 = function (field) {
            return $scope.submitted2;
        };


        //===========----Clear Field
        $scope.Clearid = function () {
            //$state.reload();
            $scope.ISMMCLT_ClientName = "";
            $scope.ISMMCLT_ContactNo = "";
            $scope.ISMMCLT_EmailId = "";
            $scope.HRME_Id = "";
            $scope.ISMMCLT_NOName = "";
            $scope.ISMMCLT_NOEmailId = "";
            $scope.ISMMCLT_NOContactNo = "";
            $scope.leader_id = "";
            $scope.ISMMCLT_Desc = "";
            $scope.ISMMCLT_Address = "";
            $scope.ISMMCLT_RemainderDays = "";
            $scope.ISMMCLT_Code = "";
            $scope.MI_Id = "";
            $scope.ISMMCLT_GSTNO = "";
            $scope.Loaddata();
            $scope.submitted = false;
            $scope.editfalg = false;
        };

        $scope.searchchkbx1 = "";
        $scope.all_checkC = function () {
            var checkStatus = $scope.usercheckC;
            angular.forEach($scope.ieEmpList, function (itm) {
                itm.selected = checkStatus;
            });
        };

        $scope.togchkbxC = function () {
            $scope.usercheckC = $scope.ieEmpList.every(function (options) {
                return options.selected;
            });
        };

        $scope.isOptionsRequired = function () {
            return !$scope.ieEmpList.some(function (options) {
                return options.selected;
            });
        }

        $scope.get_emplist = function () {
            var data = {
                "HRMD_Id": $scope.HRMD_Id,
                "MI_Id": $scope.MI_Id
            };
            apiService.create("ImsClient/get_emplist", data).then(function (promise) {
                $scope.emplist = promise.emplist;
            });
        };

        $scope.tab1 = false;
        $scope.next = function () {
            $scope.tab2 = false;
            $scope.myTabIndex = $scope.myTabIndex + 1;
        };

        $scope.previous = function () {
            $scope.myTabIndex = $scope.myTabIndex - 1;
            $scope.tab2 = true;
            $scope.tab1 = false;
        };


        $scope.OnChangeTab2Inst = function () {
            $scope.search2 = "";
            $scope.ismmclT_Id = "";
            $scope.searchchkbx1 = "";
            $scope.clientlist = [];
            $scope.allIeMappingdata = [];
            $scope.usercheckC = false;

            angular.forEach($scope.ieEmpList, function (list) {
                list.selected = false;
            });

            var data = {
                "MI_Id": $scope.MI_Id,
                "Flag": "Tab2"
            };

            apiService.create("ImsClient/OnChangeTab2Inst", data).then(function (promise) {
                $scope.allIeMappingdata = promise.allIeMappingdata;
                $scope.clientlist = promise.clientlist;
            });
        };

        /////////////////////////////....................Save2
        $scope.submitted2 = false;
        $scope.saveClientMappingdata = function () {

            $scope.ieEmpListdata = [];
            if ($scope.myForm2.$valid) {
                angular.forEach($scope.ieEmpList, function (list) {
                    if (list.selected == true) {
                        $scope.ieEmpListdata.push({ HRME_Id: list.hrmE_Id });
                    }
                });

                var data = {
                    "ISMMCLTIE_Id": $scope.ismmcltiE_Id,
                    "ISMMCLT_Id": $scope.ismmclT_Id,
                    "MI_Id": $scope.MI_Id,
                    ieEmpListdata: $scope.ieEmpListdata
                };

                apiService.create("ImsClient/saveClientMappingdata", data).then(function (promise) {
                    if (promise.returnval != null && promise.duplicate != null) {
                        if (promise.duplicate == false) {
                            if (promise.returnval == true) {
                                if ($scope.ismmcltiE_Id > 0) {
                                    swal('Record Updated Successfully!!!');
                                    $scope.allIeMappingdata = promise.allIeMappingdata;
                                }
                                else {
                                    swal('Record Saved Successfully!!!');
                                    $scope.allIeMappingdata = promise.allIeMappingdata;
                                }
                            }
                            else {
                                if (promise.returnval == false) {
                                    if ($scope.ismmcltiE_Id > 0) {
                                        swal('Record Not Updated Successfully!!!');
                                    }
                                    else {
                                        swal('Record Not Saved Successfully!!!');
                                    }
                                }
                            }
                        }
                        else {
                            swal("Record already exist");
                        }

                        $scope.cancel2();
                    }
                    else {
                        swal("Kindly Contact Administrator!!!");
                    }
                });
            }
            else {
                $scope.submitted2 = true;
            }
        };

        //=====================Edit2  record
        $scope.editClientMappingdata = function (user) {

            user.MI_Id = $scope.MI_Id;

            var data = {
                "ISMMCLT_Id": user.ismmclT_Id,
                "MI_Id": $scope.MI_Id
            };
            apiService.create("ImsClient/editClientMappingdata", user).then(function (promise) {
                $scope.editfalg1 = true;
                $scope.ieEmpList = promise.ieEmpList;
                $scope.ismmcltiE_Id = promise.editCltMappinglist[0].ismmcltiE_Id;
                $scope.ismmclT_Id = promise.editCltMappinglist[0].ismmclT_Id;
                $scope.hrmE_Id = promise.editCltMappinglist[0].ismciM_IEList;
                angular.forEach($scope.ieEmpList, function (tt) {
                    angular.forEach(promise.editCltMappinglist, function (ss) {
                        if (tt.hrmE_Id == ss.ismciM_IEList) {
                            tt.selected = true;
                        }
                    })
                })
            });
        };

        //=================Activation 2 /Deactivation 2
        $scope.deactiveClientMappingdata = function (user, SweetAlert) {

            $scope.ismmcltiE_Id = user.ismmcltiE_Id;
            user.MI_Id = $scope.MI_Id;
            var dystring = "";
            if (user.ismmcltiE_ActiveFlag == 1) {
                dystring = "Deactivate";
            }
            else if (user.ismmcltiE_ActiveFlag == 0) {
                dystring = "Activate";
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
                        apiService.create("ImsClient/deactiveClientMappingdata", user).then(function (promise) {
                            if (promise.returnval == true) {
                                swal("Record " + dystring + "d Successfully!!!");
                                $scope.modaliEMapingList = promise.modaliEMapingList;
                            }
                            else {
                                swal("Record Not " + dystring + "d Successfully!!!");
                            }
                        });
                    }
                    else {
                        swal("Record " + dystring + " Cancelled");
                    }
                });
        };
        //=================================End-Activation2/Deactivation2


        $scope.cancel2 = function () {
            $scope.editfalg1 = false;
            $scope.ismmclT_Id = "";
            angular.forEach($scope.ieEmpList, function (tt) {
                return tt.selected = false;
            })
            $scope.usercheckC = false;
            $scope.submitted2 = false;
            $scope.MI_Id = "";
            $scope.Loaddata();
        };

        //===========================get modallist data
        $scope.get_MappedIElist = function (user) {

            var data = {
                "ISMMCLT_Id": user.ismmclT_Id,
                "MI_Id": $scope.MI_Id
            }
            apiService.create("ImsClient/modalListdata", data).then(function (promise) {
                $scope.modaliEMapingList = promise.modaliEMapingList;
            });
        };


        //VMS Client AND IVRM Client Mapping

        $scope.cleardata_tab3 = function () {
            $scope.IVRM_MI_Id = "";
            $scope.ISMMCLT_IVRM_URL = "";
            $scope.ISMMCLT_ClientCode = "";
            $scope.ISMMCLT_Id = "";
        };

        $scope.OnChangeTab3Inst = function () {
            $scope.IVRM_MI_Id = "";
            $scope.ISMMCLT_IVRM_URL = "";
            $scope.ISMMCLT_ClientCode = "";
            var data = {
                "MI_Id": $scope.MI_Id,
                "Flag": "Tab3"
            };

            apiService.create("ImsClient/OnChangeTab2Inst", data).then(function (promise) {
                $scope.clientlist = promise.clientlist;
            });
        };

        $scope.OnChangeClientTab3 = function () {

            var data = {
                "MI_Id": $scope.MI_Id,
                "ISMMCLT_Id": $scope.ISMMCLT_Id.ismmclT_Id
            };

            apiService.create("ImsClient/OnChangeClientTab3", data).then(function (promise) {
                $scope.clientdetails = promise.clientdetails;
                $scope.IVRM_MI_Id = $scope.clientdetails[0].ivrM_MI_Id;
                $scope.ISMMCLT_IVRM_URL = $scope.clientdetails[0].ismmclT_IVRM_URL;
                $scope.ISMMCLT_ClientCode = $scope.clientdetails[0].ismmclT_ClientCode;
            });
        };

        $scope.SaveVMSIVRMMapping = function () {

            var data = {
                "MI_Id": $scope.MI_Id,
                "ISMMCLT_Id": $scope.ISMMCLT_Id.ismmclT_Id,
                "IVRM_MI_Id": $scope.IVRM_MI_Id,
                "ISMMCLT_IVRM_URL": $scope.ISMMCLT_IVRM_URL,
                "ISMMCLT_ClientCode": $scope.ISMMCLT_ClientCode
            };

            apiService.create("ImsClient/SaveVMSIVRMMapping", data).then(function (promise) {
                if (promise.returnval === true) {
                    swal("Record Updated Successfully");
                } else {
                    swal("Failed To Update The Records");
                }
                $scope.cancel3();
            });
        };

        $scope.cancel3 = function () {
            $scope.IVRM_MI_Id = "";
            $scope.ISMMCLT_IVRM_URL = "";
            $scope.ISMMCLT_ClientCode = "";
            $scope.ISMMCLT_Id = "";
            $scope.Loaddata();
        };
    }
})();