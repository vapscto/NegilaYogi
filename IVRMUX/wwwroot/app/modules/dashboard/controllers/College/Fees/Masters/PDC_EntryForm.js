
(function () {
    'use strict';
    angular
        .module('app')
        .controller('PDC_EntryFormController', PDC_EntryFormController)

    PDC_EntryFormController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache']
    function PDC_EntryFormController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {

        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }
        else if (ivrmcofigsettings.length == 0 || ivrmcofigsettings == undefined || ivrmcofigsettings == null) {
            paginationformasters = 5;
        }
        $scope.tabledetails = false;
        $scope.sortKey = "fmG_Id";   //set the sortKey to the param passed
        $scope.reverse = true; //if true make it false and vice versa

        $scope.sortKey1 = "fyG_Id";
        $scope.reverse1 = true;
        $scope.disablecurrency = false;
        $scope.disablecourse = false;
        $scope.disablebranch = false;
        $scope.disablestudent = false;
        $scope.searchValue = "";
        $scope.filterValue = function (obj) {
            if (obj.fmG_ActiceFlag == true) {
                $scope.test = "Active";
            } else if (obj.fmG_ActiceFlag == false) {
                $scope.test = "Deactive";
            }
            return angular.lowercase(obj.fmG_GroupName).indexOf(angular.lowercase($scope.searchValue)) >= 0 || angular.lowercase(obj.fmG_Remarks).indexOf(angular.lowercase($scope.searchValue)) >= 0 || angular.lowercase($scope.test).indexOf(angular.lowercase($scope.searchValue)) >= 0;
        }

        $scope.searchValue1 = "";
        $scope.filterValue1 = function (obj) {
            if (obj.fyG_ActiveFlag == true) {
                $scope.test = "Active";
            } else if (obj.fyG_ActiveFlag == false) {
                $scope.test = "Deactive";
            }
            return angular.lowercase(obj.grpname).indexOf(angular.lowercase($scope.searchValue1)) >= 0 || angular.lowercase(obj.yearname).indexOf(angular.lowercase($scope.searchValue1)) >= 0 || angular.lowercase($scope.test).indexOf(angular.lowercase($scope.searchValue1)) >= 0;
        }
        $scope.isOptionsRequired = function () {
            return !$scope.feegroup.some(function (options) {
                return options.selected;
            });
        }

        var academicyrlst = JSON.parse(localStorage.getItem("academicyearlist"));
        $scope.loaddata = function () {

            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            $scope.currentPage1 = 1;
            $scope.itemsPerPage1 = paginationformasters;
            $scope.page1 = "page1";
            $scope.page2 = "page2";
            var pageid = 2;
            $scope.FCSPDC_ChequeDate = new Date();
            apiService.getURI("PDC_EntryForm/getalldetails", pageid).
                then(function (promise) {

                    $scope.FillBank = promise.fillBank;
                    $scope.Fillcourse = promise.fillcourse;
                    $scope.FillBranch = promise.fillbranch;
                    $scope.FillSemester = promise.fillSemester;
                    $scope.FillStudent = promise.fillCategory;
                    $scope.pages = promise.pdcrecord;



                })
        }
        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }

        $scope.sort1 = function (keyname) {
            $scope.sortKey1 = keyname;   //set the sortKey to the param passed
            $scope.reverse1 = !$scope.reverse1; //if true make it false and vice versa
        }

        $scope.cance = function () {
            $state.reload();
        }

        $scope.searchchkbx = "";
        $scope.filterchkbx = function (obj) {

            return angular.lowercase(obj.fmG_GroupName).indexOf(angular.lowercase($scope.searchchkbx)) >= 0;
        }

        $scope.selectacademicyear = function () {
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id
            }

            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }

            apiService.create("PDC_EntryForm/selectacademicyear", data).
                then(function (promise) {

                    // $scope.arrlistchk = promise.fillmastergroup;
                })

        }

        $scope.submitted = false;
        $scope.savedata = function () {
            $scope.students1 = [];
            var count = 0;

            angular.forEach($scope.students, function (student) {
                if (student.checkedvalue == true) {
                    count = count + 1;
                }
            });
            if (count > 0) {
                if ($scope.myForm.$valid) {
                    //  var PACA_DOB = new Date($scope.FCSPDC_ChequeDate).toDateString();

                    angular.forEach($scope.students, function (student) {
                        if (student.checkedvalue == true) {
                            $scope.students1.push(student);
                        }
                    });
                    console.log($scope.students1);
                    var data = {

                        //"FCSPDC_Id": $scope.fcspdC_Id,
                        //"AMCST_Id": $scope.AMCST_Id,
                        "AMCO_Id": $scope.AMCO_Id,
                        "AMB_Id": $scope.AMB_Id,
                        "AMSE_Id": $scope.AMSE_Id,
                        savetmpdata: $scope.students1,
                        //"FCSPDC_ChequeNo": $scope.FCSPDC_ChequeNo,
                        //"FCSPDC_ChequeDate": new Date($scope.FCSPDC_ChequeDate).toDateString(),

                        // "FCSPDC_Amount": $scope.FCSPDC_Amount,
                        // "FMBANK_Id": $scope.FMBANK_Id,
                        // "FCSPDC_Currency": $scope.CLGSEATCAT,

                    }
                    var config = {
                        headers: {
                            'Content-Type': 'application/json;'
                        }
                    }
                    apiService.create("PDC_EntryForm/", data).then(function (promise) {
                        if (promise.returnduplicatestatus == "Duplicate") {
                            swal('Record Already Exist');
                            // $scope.cance();
                        }

                        else if (promise.returnduplicatestatus == "Save") {

                            swal('Record Saved Successfully');
                            $state.reload();
                        }

                        else if (promise.returnduplicatestatus == "NotSave") {

                            swal('Record Not Saved');
                            $state.reload();
                        }
                        else if (promise.returnduplicatestatus == "Update") {

                            swal('Record Updated Successfully');
                            $state.reload();
                        }
                        else if (promise.returnduplicatestatus == "NotUpdate") {

                            swal('Record Not Updated');
                            $state.reload();
                        }


                        else {
                            if (promise.message != null) {
                                swal('Record Not Updated', 'success');
                            }
                            else {
                                swal('Record Not Saved', 'success');
                            }
                            $scope.cance();
                        }
                    })
                }
                else {
                    $scope.submitted = true;
                }
            }
            else {
                swal("Select the record");
            }

        };
        $scope.interacted = function (field) {

            return $scope.submitted;
        };

        $scope.interacted1 = function (field) {
            return $scope.submitted1;
        };
        $scope.searchsource = function () {
            var entereddata = $scope.search;
            var data = {
                "FMG_GroupName": $scope.search,
                "FMG_Remarks": $scope.type
            }
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            apiService.create("PDC_EntryForm/1", data).
                then(function (promise) {
                    $scope.pages = promise.groupData;
                    swal("searched Successfully");
                })
        }
        $scope.editEmployee = {}
        $scope.getorgvalue = function (employee) {
            $scope.editEmployee = employee.FCSPDC_Id;
            var pageid = $scope.editEmployee;
            apiService.getURI("PDC_EntryForm/getdetails", pageid).
                then(function (promise) {

                    $scope.disablecurrency = true;
                    $scope.disablecourse = true;
                    $scope.disablebranch = true;
                    $scope.disablestudent = true;

                    $scope.students = promise.admsudentslist;
                    $scope.AMCO_Id = promise.groupData[0].amcO_Id;
                    $scope.AMB_Id = promise.groupData[0].amB_Id;
                    $scope.AMSE_Id = promise.groupData[0].amsE_Id;
                    $scope.fcspdC_Currency = promise.groupData[0].fcspdC_Currency;
                    $scope.newcaste = [];
                    if ($scope.FillStudent.length > 0) {
                        for (var i = 0; i < $scope.FillStudent.length; i++) {
                            if (promise.groupData[0].amcsT_Id == $scope.FillStudent[i].amcsT_Id) {
                                $scope.FillStudent[i].Selected = true;
                                $scope.stud = $scope.FillStudent[i];
                                $scope.newcaste = promise.groupData[0].amcsT_Id;
                            }
                        }
                    }
                    $scope.students = promise.groupData;
                    angular.forEach($scope.students, function (stu) {
                        stu.bankname = promise.fillBank;
                        $scope.tabledetails = true;
                        stu.amcsT_Id = promise.groupData[0].amcsT_Id;
                        stu.fcspdC_ChequeNo = promise.groupData[0].fcspdC_ChequeNo;
                        stu.fcspdC_ChequeDate = new Date(promise.groupData[0].fcspdC_ChequeDate),
                            stu.fcspdC_Amount = promise.groupData[0].fcspdC_Amount;
                        stu.fmbanK_Id = promise.groupData[0].fmbanK_Id;
                        stu.fcspdC_Currency = promise.groupData[0].fcspdC_Currency;

                        stu.fcspdC_Id = promise.groupData[0].fcspdC_Id;
                        stu.amsE_Id = promise.groupData[0].amsE_Id;
                        stu.amsE_SEMName = promise.groupData[0].amsE_SEMName;
                    });

                    //$scope.AMCST_Id = promise.groupData[0].amcsT_Id;
                    //$scope.FCSPDC_ChequeNo = promise.groupData[0].fcspdC_ChequeNo;
                    //$scope.FCSPDC_ChequeDate = new Date(promise.fcspdC_ChequeDate).toDateString(),
                    //$scope.FCSPDC_Amount = promise.groupData[0].fcspdC_Amount;
                    //$scope.FMBANK_Id = promise.groupData[0].fmbanK_Id;


                    //$scope.CLGSEATCAT = promise.groupData[0].fcspdC_Currency;

                    //$scope.fcspdC_Id = promise.groupData[0].fcspdC_Id;
                    //for (var i = 0; i < $scope.feegroup.length; i++) {

                    //    $scope.disablegroups = true;

                    //    name = $scope.feegroup[i].fmG_Id;
                    //    if (name == promise.groupData[0].fmG_Id) {
                    //        $scope.feegroup[i].selected = true
                    //        //$scope. arrlistchk[i].disablegroups = true;
                    //    }
                    //    else {
                    //        $scope.feegroup[i].selected = false;

                    //    }

                    //}

                    //$scope.fcqcfG_Id = promise.groupData[0].fcqcfG_Id;
                })
        }
        $scope.deactive = function (groupData, SweetAlert) {
            var mgs = "";
            var confirmmgs = "";
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            if (groupData.FCSPDC_ActiveFlg == true) {
                mgs = "Deactivate";
                confirmmgs = "Deactivated";

            }
            else {
                mgs = "Activate";
                confirmmgs = "Activated";

            }
            swal({
                title: "Are you sure?",
                text: "Do You Want To " + mgs + " Record?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + mgs + " it",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {

                        apiService.create("PDC_EntryForm/deactivate", groupData).
                            then(function (promise) {
                                if (promise.returnval == "true") {
                                    swal("Record " + confirmmgs + " Successfully");
                                }
                                else {
                                    swal("Request Failed!!!");
                                }
                                $scope.loaddata();
                            })

                    }
                    else {
                        swal("Record " + mgs + " Cancelled");
                    }
                });
        }

        $scope.toggleAll = function () {
            var toggleStatus = $scope.selectAll;
            angular.forEach($scope.arrlistchk, function (role) { role.selected = toggleStatus; });

        }
        $scope.optionToggled = function (user) {
            $scope.all = $scope.students.every(function (itm) { return itm.checkedvalue; })
        }
        $scope.cancel1 = function () {
            $scope.loaddata();
            $scope.tabledetails = false;
            $scope.disablecurrency = false;
            $scope.disablecourse = false;
            $scope.disablebranch = false;
            $scope.disablestudent = false;
        }
        $scope.cancel2 = function () {
            // $scope.loaddata();
            $scope.tabledetails = false;
            $scope.disablecurrency = false;
            $scope.disablecourse = false;
            $scope.disablebranch = false;
            $scope.disablestudent = false;
        }
        $scope.toggleAll = function () {
            var toggleStatus = $scope.all;
            angular.forEach($scope.students, function (itm) {
                itm.checkedvalue = toggleStatus;
            });
        };
        $scope.Showdata = function () {

            if ($scope.myForm.$valid) {



                var data = {

                    "AMCO_Id": $scope.AMCO_Id,
                    "AMB_Id": $scope.AMB_Id,
                    //"AMSE_Id": $scope.AMSE_Id,
                    "AMCST_Id": $scope.stud.amcsT_Id,

                }
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }
                apiService.create("PDC_EntryForm/showdata", data).then(function (promise) {

                    $scope.students = promise.admsudentslist;


                    angular.forEach($scope.students, function (stu) {
                        stu.bankname = promise.fillBank;
                        $scope.tabledetails = true;


                    });
                })
            }
            else {
                $scope.submitted = true;
            }
        };


        $scope.onselectcourse = function () {


            var data = {
                "AMCO_Id": $scope.AMCO_Id,

            }

            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }

            apiService.create("PDC_EntryForm/selectcourse", data).
                then(function (promise) {


                    if (promise.fillbranch.length > 0) {
                        $scope.FillBranch = promise.fillbranch;
                    }
                    else {
                        swal("No Records found.!!")
                    }
                })

        };

        $scope.onselectbranch = function () {


            var data = {
                "AMCO_Id": $scope.AMCO_Id,
                "AMB_Id": $scope.AMB_Id,
            }
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            apiService.create("PDC_EntryForm/selectbran", data).
                then(function (promise) {

                    if (promise.fillsemester.length > 0) {

                        $scope.FillSemester = promise.fillSemester;
                    }
                    else {
                        swal("No Records found.!!")
                    }
                })
        };

        $scope.onselectsemester = function () {






            var data = {
                "AMCO_Id": $scope.AMCO_Id,
                "AMB_Id": $scope.AMB_Id,
                "AMSE_Id": $scope.AMSE_Id,


            }

            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }

            apiService.create("PDC_EntryForm/selectsem", data).
                then(function (promise) {

                    if (promise.fillCategory.length > 0) {

                        $scope.FillStudent = promise.fillCategory;
                    }
                    else {
                        swal("No Records found.!!")
                    }
                })

        };






    }
    function getIndependentDrpDwnss() {
        apiService.getURI("PDC_EntryForm/getdpforyear", 2).then(function (promise) {

            $scope.arrlist6 = promise.academicdrp;

            // for pagination 
            $scope.currentPage = 1;
            $scope.itemsPerPage = 5;
            $scope.pages = promise.academicdrp;
            // for pagination
        })
        $scope.order = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }
    }


})();

