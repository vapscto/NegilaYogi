(function () {
    'use strict';
    angular.module('app').controller('GeneralSiblingEmployeeMapping', GeneralSiblingEmployeeMapping)

    GeneralSiblingEmployeeMapping.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache']
    function GeneralSiblingEmployeeMapping($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {

        $scope.samplearry = [
            { nname: 'I', amsT_ORDER: 1 },
            { nname: 'II', amsT_ORDER: 2 },
            { nname: 'III', amsT_ORDER: 3 },
            { nname: 'IV', amsT_ORDER: 4 },
            { nname: 'V', amsT_ORDER: 5 },
            { nname: 'VI', amsT_ORDER: 6 }
        ];

        $scope.totalgrid = [];
        $scope.maingrid = false;
        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings !== null && ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        } else {
            paginationformasters = 10;
        }

        $scope.sortKey = "amsS_Id";
        $scope.reverse = true;
        $scope.searchthird = "";
        $scope.searchthirdd = "";

        $scope.remflg = false;
        $scope.addnewbtn = true;

        $scope.formload = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;

            var data = {
                "pageid": 1
            };

            apiService.create("GeneralSiblingEmployeeMapping/getalldetails", data).then(function (promise) {
                $scope.yearlst = promise.fillacademic;
                $scope.staffcount = promise.fillstaff;
                $scope.studentcount = promise.allstudentdata;

                if (promise.alldata.length > 0) {
                    $scope.maingrid = true;
                    $scope.thirdgrid = promise.alldata;
                }
                else {
                    swal("No records found");
                    $scope.maingrid = false;
                }
            });

            $scope.sort = function (keyname) {
                $scope.sortKey = keyname;   //set the sortKey to the param passed
                $scope.reverse = !$scope.reverse; //if true make it false and vice versa
            };
        };

        // On Select Of Radio Button
        $scope.changeradio = function (chkradio) {
            $scope.disablesave = false;
            $scope.AMST_Id = "";
            $scope.studentname = [];
            $scope.HRME_Id = "";
            $scope.staffcount = [];

            if (chkradio === "stud") {
                $scope.student = true;
                $scope.RTE = false;
                $scope.showstaff = false;
                $scope.grigview1 = false;
                $scope.grigview21 = false;

            } else if (chkradio === "stfoth") {
                $scope.student = false;
                $scope.showstaff = true;
                $scope.grigview1 = false;
                $scope.grigview21 = false;
                $scope.RTE = false;
            } else if (chkradio === "R") {
                $scope.student = false;
                $scope.RTE = true;
                $scope.showstaff = false;
                $scope.grigview1 = false;
                $scope.grigview21 = false;
            }

            $scope.maingrid1 = false;
            $scope.maingrid = false;

            var data = {
                "radiobtnval": $scope.stuchk
            };

            apiService.create("GeneralSiblingEmployeeMapping/selectradio", data).then(function (promise) {

                if (promise !== null) {
                    $scope.getclassdetails = promise.getclassdetails;
                    if (chkradio === "stud") {
                        if (promise.returnval === "No Data") {
                            swal("Concession Type Is Not Entered To Map The Siblings Details , Kindly Enter The Concession Type");
                        } else {
                            $scope.studentname = promise.alldata;
                        }

                        $scope.thirdgrid = promise.getdisplaydetails;
                        if ($scope.thirdgrid !== null && $scope.thirdgrid.length > 0) {
                            $scope.totcountfirsts = $scope.thirdgrid.length;
                            $scope.maingrid = true;
                            $scope.maingrid1 = false;
                        } else {
                            $scope.maingrid1 = false;
                            $scope.maingrid = false;
                        }

                    } else if (chkradio === "stfoth") {
                        if (promise.returnval === "No Data") {
                            swal("Concession Type Is Not Entered To Map The Employee Details , Kindly Enter The Concession Type");
                        } else {
                            $scope.staffcount = promise.alldata;
                        }
                        $scope.getdisplaydetailsstaff = promise.getdisplaydetails;
                        $scope.totcountfirst = $scope.getdisplaydetailsstaff.length;
                        if ($scope.getdisplaydetailsstaff !== null && $scope.getdisplaydetailsstaff.length > 0) {
                            $scope.maingrid1 = true;
                            $scope.maingrid = false;
                        } else {
                            $scope.maingrid1 = false;
                            $scope.maingrid = false;
                        }
                    } else if (chkradio === "R") {
                        if (promise.returnval === "No Data") {
                            swal("Concession Type Is Not Entered To Map The RTE Details , Kindly Enter The Concession Type");
                        } else {
                            $scope.studentnamerte = promise.alldata;
                        }
                    }
                } else {
                    swal("Something Went Wrong Kindly Contact Administratior");
                }


            });
        };       

        // On Student Siblings
        $scope.onstudentnamechange = function (emptyrow) {
            $scope.totalgrid = [];
            $scope.getstudentlistsavedd = [];
            var data = {
                "AMST_Id": $scope.AMST_Id.amsT_Id
            };
            apiService.create("GeneralSiblingEmployeeMapping/onstudentnamechange", data).then(function (promise) {

                if (promise !== null) {
                    if (promise.returnval === "Duplicate") {
                        swal("Kindly Select The First Child Student Name ");

                    } else {
                        $scope.addflg = true;
                        $scope.disablesave = false;
                        $scope.getstudentlistsavedd = promise.getstudentlistsaved;
                        if ($scope.getstudentlistsavedd !== null && $scope.getstudentlistsavedd.length > 0) {
                            $scope.getstudentlistsaved = $scope.getstudentlistsavedd;

                            angular.forEach($scope.getstudentlistsaved, function (dd) {
                                $scope.totalgrid.push({
                                    AMST_Id: dd, samplee: dd.amstS_SiblingsOrder, relation: dd.amstG_SiblingsRelation, class: dd.amcL_Id, fisrt: true,
                                    AMSTS_Id: dd.amstS_Id
                                });
                            });
                        } else {
                            $scope.totalgrid.push({
                                'AMST_Id': $scope.AMST_Id,
                                'samplee': 1,
                                'relation': "",
                                "fisrt": true,
                                "class": $scope.AMST_Id.amcL_Id,
                                "AMSTS_Id": 0
                            });
                        }

                        $scope.studentcount = promise.getsudentlist;
                        $scope.grigview1 = true;
                        $scope.grigview1rte = false;
                        console.log($scope.totalgrid);
                    }

                } else {
                    swal("Something Went Wrong Kindly Contact Administrator");
                }
            });
        };       

        $scope.viewsiblingdetails = function (user) {
            apiService.create("GeneralSiblingEmployeeMapping/viewsiblingdetails", user).then(function (promise) {

                if (promise !== null) {
                    $scope.getviewdetails = promise.getviewdetails;
                }
            });
        };

        $scope.DeletRecord = function (user) {
            $scope.editEmployee = user.amsT_Id;
            var orgid = $scope.editEmployee;

            swal({
                title: "Are you sure?",
                text: "All sibling data will be deleted, Do you want to proceed?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, Delete it",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("GeneralSiblingEmployeeMapping/Deletedetails", user).
                            then(function (promise) {

                                if (promise.message === "Mapped") {
                                    swal("You Can Not Delete The Record Receipts Are Raised For These Students First Delete The Receipts");
                                    return;
                                }

                                if (promise.returnval === "true") {
                                    swal("Record Deleted Successfully");
                                }
                                else {
                                    swal("Failed To Delete Record");
                                }
                                $state.reload();
                            });
                    }
                    else {
                        swal("Record Deletion Cancelled");
                        $scope.formload();
                    }
                });
        };


        // On Staff 
        $scope.onselectstaff = function (emptyrow) {
            $scope.totalgrid = [];
            $scope.getstudentlistsavedd = [];
            var data = {
                "HRME_Id": $scope.HRME_Id.hrmE_Id
            };
            apiService.create("GeneralSiblingEmployeeMapping/onselectstaff", data).then(function (promise) {

                if (promise !== null) {

                    $scope.getstudentlistsavedd = promise.getstudentlistsaved;
                    $scope.grigview21 = true;
                    $scope.addflg = true;
                    $scope.grigview1rte = false;
                    $scope.studentcount = promise.getsudentlist;
                    if ($scope.getstudentlistsavedd !== null && $scope.getstudentlistsavedd.length > 0) {
                        $scope.getstudentlistsaved = $scope.getstudentlistsavedd;

                        angular.forEach($scope.getstudentlistsaved, function (dd) {
                            $scope.totalgrid.push({ AMST_Id: dd, class: dd.amcL_Id, AMSTE_Id: dd.amstE_Id, samplee: dd.amstE_SiblingsOrder, fisrt: true });
                        });
                    } else {
                        $scope.totalgrid.push({ 'AMST_Id': emptyrow.AMST_Id, "class": emptyrow.class, "AMSTE_Id": 0, fisrt: false });
                    }
                } else {
                    swal("Something Went Wrong Kindly Contact Administrator");
                }

            });

        };

        $scope.viewsiblingdetailsemployee = function (user) {
            apiService.create("GeneralSiblingEmployeeMapping/viewsiblingdetailsemployee", user).then(function (promise) {

                if (promise !== null) {
                    $scope.getviewdetails = promise.getviewdetails;
                }
            });
        };

        $scope.DeletRecordemployee = function (user) {

            swal({
                title: "Are you sure?",
                text: "All student data will be deleted, Do you want to proceed?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, Delete it",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("GeneralSiblingEmployeeMapping/DeletRecordemployee", user).
                            then(function (promise) {

                                if (promise.message === "Mapped") {
                                    swal("You Can Not Delete The Record Receipts Are Raised For These Students First Delete The Receipts");
                                    return;
                                }

                                if (promise.returnval === "true") {
                                    swal("Record Deleted Successfully");
                                }
                                else {
                                    swal("Failed To Delete Record");
                                }
                                $state.reload();
                            });
                    }
                    else {
                        swal("Record Deletion Cancelled");
                        $scope.formload();
                    }
                });
        };

        // Checking Duplicate Student Name
        $scope.sampname = function (user, index) {
            var count = 0;
            for (var k = 0; k < $scope.totalgrid.length; k++) {
                var roll = parseInt(user.AMST_Id.amsT_Id);
                $scope.totalgrid[index].class = user.AMST_Id.amcL_Id;
                var arryind = $scope.totalgrid.indexOf($scope.totalgrid[k]);
                console.log(arryind);
                if (arryind !== index) {
                    var rollindex = parseInt($scope.totalgrid[k].AMST_Id.amsT_Id);
                    if (rollindex === roll) {
                        swal("Student Name Already Exists");
                        $scope.totalgrid[index].AMST_Id = "";
                        $scope.totalgrid[index].class = "";
                        count += 1;
                        break;
                    }
                }
            }
        };

        // Checking the order of the student
        $scope.sampcng = function (user, index) {
            var indexvalue = index + 1;
            var ordervalue = parseInt(user.samplee);

            if (indexvalue !== ordervalue) {
                swal("Select Order Wise Only Like 1,2,3...");
                $scope.totalgrid[index].samplee = "";
                return;
            }

            for (var k = 0; k < $scope.totalgrid.length; k++) {
                var roll = parseInt(user.samplee);
                var arryind = $scope.totalgrid.indexOf($scope.totalgrid[k]);
                console.log(arryind);
                if (arryind !== index) {
                    var rollindex = parseInt($scope.totalgrid[k].samplee);
                    if (rollindex === roll) {
                        swal("Sibling Order Already Exists");
                        $scope.totalgrid[index].samplee = "";
                        break;
                    }
                }
            }
        };

        $scope.totalgridtest = [];

        // Add New Row
        $scope.addNew = function (totalgrid) {
            $scope.totalgrid.push({
                'AMST_Id': totalgrid.AMST_Id
            });
            $scope.totalgridtest.push({
                'AMST_Id': totalgrid.AMST_Id
            });
            if ($scope.totalgrid.length > 0) {
                $scope.remflg = true;
            } else {
                $scope.remflg = false;
            }

            if ($scope.totalgrid.length === 5) {
                $scope.addflg = false;
            } else {
                $scope.addflg = true;
            }

            $scope.PD = {};
        };

        // Remove Last Row
        $scope.removerow = function (totalgrid) {
            $scope.totalgrid.pop({
                'AMST_Id': totalgrid.AMST_Id
            });
            $scope.totalgridtest.pop({
                'AMST_Id': totalgrid.AMST_Id
            });
            if ($scope.totalgrid.length - 1 > 0) {
                $scope.remflg = true;
            } else {
                $scope.remflg = false;
            }
            if ($scope.totalgrid.length === 5) {
                $scope.addflg = false;
            } else {
                $scope.addflg = true;
            }
            // $scope.PD = {};
        };

        // Save 
        $scope.savedata = function (totalgrid) {
            $scope.temparray = [];
            if ($scope.myform.$valid) {
                var data = "";
                if ($scope.stuchk === "stud") {
                    angular.forEach(totalgrid, function (dd) {
                        $scope.temparray.push({ AMSTS_SiblingsAMST_Id: dd.AMST_Id.amsT_Id, AMSTS_SiblingsName: dd.AMST_Id.sibilingname, AMSTG_SiblingsRelation: dd.relation, AMSTS_SiblingsOrder: dd.samplee, AMCL_Id: dd.class, AMSTS_Id: dd.AMSTS_Id });
                    });

                    if ($scope.temparray.length === 1) {
                        swal("You Can Not Save Single Sibling Child");
                        return;
                    }

                    data = {
                        savesiblingsDTO: $scope.temparray,
                        "AMST_Id": $scope.AMST_Id.amsT_Id,
                        "radiobtnval": $scope.stuchk
                    };
                }
                else if ($scope.stuchk === "stfoth") {

                    angular.forEach(totalgrid, function (dd) {
                        $scope.temparray.push({ AMST_Id: dd.AMST_Id.amsT_Id, ASMCL_Id: dd.class, AMSTE_Id: dd.AMSTE_Id, AMSTE_SiblingsOrder: dd.samplee });
                    });

                    data = {
                        "HRME_Id": $scope.HRME_Id.hrmE_Id,
                        savestudentemployeeDTO: $scope.temparray,
                        "radiobtnval": $scope.stuchk
                    };
                }

                else if ($scope.stuchk === "R") {
                    data = {
                        "AMST_Id": $scope.AMST_IdRTE.amsT_Id,
                        "radiobtnval": $scope.stuchk
                    };
                }

                apiService.create("GeneralSiblingEmployeeMapping/savedata", data).
                    then(function (promise) {
                        if (promise.returnval === "true") {
                            swal("Record Saved Sucessfully");
                        }
                        else {
                            swal("Kindly contact Administrator");
                        }
                        $state.reload();
                    });
            }
            else {
                $scope.submitted = true;
            }

        };

        $scope.cleardata = function () {
            $state.reload();
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.submitted = false;



        // No Use Fuction
        $scope.onselectacade = function (emptyrow) {
            $scope.grigview1 = true;

            $scope.totalgrid.push({
                'AMST_Id': emptyrow.AMST_Id
            });
            $scope.PD = {};
        };

        $scope.onselectacade1 = function () {
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "radiobtnval": $scope.stuchk
            };
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            };
            apiService.create("GeneralSiblingEmployeeMapping/selectacademic", data).
                then(function (promise) {

                    $scope.totalgrid.push({
                        'AMST_Id': emptyrow.AMST_Id,
                        'studorder': emptyrow.studorder
                    });
                    $scope.PD = {};
                });
        };

        $scope.filterby = function () {
            var entereddata = $scope.search;

            var data = {
                "FMG_GroupName": $scope.searchthird,
                "FMH_FeeName": $scope.typethird
            };

            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            };

            apiService.create("YearlyFeeGroupMapping/1", data).
                then(function (promise) {
                    $scope.thirdgrid = promise.alldata;
                    swal("searched Successfully");
                });
        };

        $scope.remove = function (totalgrid) {
            var newDataList = [];
            $scope.selectedAll = false;
            angular.forEach($scope.totalgrid, function (selected) {
                if (!selected.selected) {
                    newDataList.push(selected);
                }
            });
            $scope.totalgrid = newDataList;
        };

        $scope.EditMasterSectvalue = function (employee) {
            $scope.editEmployee = employee.fyghM_Id;
            var orgid = $scope.editEmployee;
            //orgid = 12;
            apiService.getURI("YearlyFeeGroupMapping/Editdetails", orgid).
                then(function (promise) {
                    $scope.addnewbtn = false;
                    $scope.FMG_Id = promise.alldata[0].fmG_Id;

                    //$scope.totalgrid.headcount.selected.FMH_Id = promise.alldata[0].fmH_Id;
                    //$scope.totalgrid.installmentcount.FMI_Id = promise.alldata[0].fmI_Id;

                    $scope.grigview1 = true;

                    $scope.totalgrid = promise.alldata;

                    if (promise.alldata[0].fyghM_FineApplicableFlag === 0) {

                        $scope.totalgrid.FYGHM_FineApplicableFlag = true;
                    }
                    if (promise.alldata[0].fyghM_Common_AmountFlag === 0) {

                        $scope.totalgrid.FYGHM_Common_AmountFlag = true;
                    }
                    if (promise.alldata[0].fyghM_ActiveFlag === 0) {

                        $scope.totalgrid.FYGHM_ActiveFlag = true;
                    }
                });
        };

        $scope.onselectgroup = function (groupcount, emptyrow) {

            if ($scope.FMG_Id === "") {
                swal("Please Select Any Group !!!");
                $scope.grigview1 = false;
                $scope.totalgrid = [];
            }
            else {

                var groupid = $scope.FMG_Id;

                var data = {
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "FMG_Id": groupid
                };
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                };

                apiService.create("YearlyFeeGroupMapping/getadetailsongroup", data).
                    then(function (promise) {
                        $scope.grigview1 = true;
                        $scope.totalgrid = promise.alldata;

                        if ($scope.totalgrid.length === 0) {
                            $scope.totalgrid.push({
                                'AMST_Id': totalgrid.AMST_Id
                            });
                            $scope.PD = {};
                        }
                    });
            }
        };

    }

})();