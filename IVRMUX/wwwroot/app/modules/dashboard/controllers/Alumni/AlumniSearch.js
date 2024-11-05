(function () {
    'use strict';
    angular
        .module('app')
        .controller('AlumniSearchController', AlumniSearchController)
    AlumniSearchController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', 'Excel', '$timeout', '$filter']
    function AlumniSearchController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, Excel, $timeout, $filter) {

        $scope.searchValue = '';
        $scope.ddate = {};
        $scope.ddate = new Date();

        $scope.usrname = localStorage.getItem('username');

        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));

        var paginationformasters;
        var copty;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings != null && ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_ReportPagination;
            copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
        }
        $scope.itemsPerPage = paginationformasters;
        $scope.coptyright = copty;

        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }

        $scope.imgname = logopath;

        $scope.grid_flag = false;
        $scope.tadprint = false;
        $scope.items = {};
        $scope.email = true;
        $scope.ph = true;
        $scope.emailfunction = function (email) {
            if (email === true) {
                $scope.checkemail = true;
            }
            else {
                $scope.checkemail = false;
            }

            //  $scope.printstudents

            angular.forEach($scope.searchResult, function (dd) {
                //  dd.selected = toggleStatus;
                if (dd.selected == true) {
                    dd.selected = false;
                }
            });

            angular.forEach($scope.printstudents, function (dd) {
                //  dd.selected = toggleStatus;
                if (dd.selected == true) {
                    dd.selected = false;
                }
            });

            $scope.printstudents = [];
        };
        $scope.BindData = function () {
            apiService.getDATA("AlumniSearch/Getdetails").

                then(function (promise) {
                    $scope.newuser1 = promise.yearList;
                    $scope.newuser2 = promise.classList;
                    //$scope.countrylist = promise.countrylist;
                    //$scope.IVRMMC_Id = 101;
                    //$scope.IVRMMS_Id = 17;
                    //$scope.statelist = promise.statelistall
                    //$scope.citylist1 = [];
                    //$scope.occupationlist1 = [];

                });
        };


        $scope.result = [
            {
                "operator": [
                    { "value": "=", "name": "Equal to" },
                    { "value": "!=", "name": "Not Equal to" },
                    { "value": "like", "name": "Like" },
                    //{ "value": ">", "name": "Greater than" },
                    //{ "value": "<", "name": "Less than" },
                ],
                "fields": [
                    { "value": "StudentName", "name": "Alumni Name" },
                    { "value": "ALMST_MobileNo", "name": "MobileNo" },
                    { "value": "ALMST_emailId", "name": "Email id" },
                    { "value": "IVRMMC_CountryName", "name": "Country" },
                    { "value": "IVRMMS_Name", "name": "State" },
                    { "value": "ALMST_PerCity", "name": "City" },
                    { "value": "ALSPR_Designation", "name": "Profession" },
                    { "value": "ALMST_BloodGroup", "name": "Blood Group" },

                    { "value": "ALMST_FatherName", "name": "Father Name" },
                    { "value": "ALMST_FullAddess", "name": "FullAddess" },
                    { "value": "ASMCL_Id_Join", "name": "Class Joined" },
                    { "value": "ASMCL_Id_Left", "name": "Class Left" },
                    { "value": "ASMAY_Id_Join", "name": "Year Joined" },
                    { "value": "ASMAY_Id_Left", "name": "Year Left" },
                    { "value": "ALMST_MembershipId", "name": "Membership ID" },
                    { "value": "ALMMC_MembershipCategory", "name": "Membership Category" },



                ],
                "condition": [
                    { "value": "AND", "name": "AND" },
                    { "value": "OR", "name": "OR" },
                ]

            }]
        $scope.addNew = function (index) {
            $scope['condflag' + index] = true;
            $scope.result.push({
                "operator": [
                    { "value": "=", "name": "Equal to" },
                    { "value": "!=", "name": "Not Equal to" },
                    { "value": "like", "name": "Like" },
                    //{ "value": ">", "name": "Greater than" },
                    //{ "value": "<", "name": "Less than" },
                ],
                "fields": [
                    { "value": "StudentName", "name": "Alumni Name" },
                    { "value": "ALMST_MobileNo", "name": "MobileNo" },
                    { "value": "ALMST_emailId", "name": "Email id" },
                    { "value": "IVRMMC_CountryName", "name": "Country" },
                    { "value": "IVRMMS_Name", "name": "State" },
                    { "value": "ALMST_PerCity", "name": "City" },
                    { "value": "ALSPR_Designation", "name": "Profession" },
                    { "value": "ALMST_BloodGroup", "name": "Blood Group" },

                    { "value": "ALMST_FatherName", "name": "Father Name" },
                    { "value": "ALMST_FullAddess", "name": "FullAddess" },
                    { "value": "ASMCL_Id_Join", "name": "Class Joined" },
                    { "value": "ASMCL_Id_Left", "name": "Class Left" },
                    { "value": "ASMAY_Id_Join", "name": "Year Joined" },
                    { "value": "ASMAY_Id_Left", "name": "Year Left" },
                    { "value": "ALMST_MembershipId", "name": "Membership ID" },
                    { "value": "ALMMC_MembershipCategory", "name": "Membership Category" },


                ],
                "condition": [
                    { "value": "AND", "name": "AND" },
                    { "value": "OR", "name": "OR" }
                ]
            });
        };
        $scope.removeRow = function (index) {

            $scope.result.pop();
            $scope['condflag' + (index - 1)] = false;
            if ($scope.items.val != '' && $scope.items.val != null) {
                $scope.items.val[index] = '';
            }
            if ($scope.items.oprtr != '' && $scope.items.oprtr != null) {
                $scope.items.oprtr[index] = '';
            }
            if ($scope.items.field != '' && $scope.items.field != null) {
                $scope.items.field[index] = '';
            }
            if ($scope.items.conditn != '' && $scope.items.conditn != null) {
                $scope.items.conditn[index] = '';
            }
        }
        var abc = "";
        $scope.minall = abc;
        $scope.filterOperator = function (field, index) {


            if (field == "StudentName") {
                $scope.result[index].operator.splice(0, 5);
                $scope.result[index].operator.push(
                    { "value": "=", "name": "Equal to" },
                    { "value": "!=", "name": "Not Equal to" },
                    { "value": "like", "name": "Like" }
                );
            }
            else if (field == "ALMST_MobileNo") {
                $scope.result[index].operator.splice(0, 5);
                $scope.result[index].operator.push(
                    { "value": "=", "name": "Equal to" },
                    { "value": "!=", "name": "Not Equal to" },
                    { "value": "like", "name": "Like" }
                );
            }
            else if (field == "ALMST_emailId") {
                $scope.result[index].operator.splice(0, 5);
                $scope.result[index].operator.push(
                    { "value": "=", "name": "Equal to" },
                    { "value": "!=", "name": "Not Equal to" },
                    { "value": "like", "name": "Like" }
                );
            }
            else if (field == "IVRMMC_CountryName") {

                $scope.result[index].operator.splice(0, 5);
                $scope.result[index].operator.push(
                    { "value": "=", "name": "Equal to" },
                    { "value": "!=", "name": "Not Equal to" },
                    { "value": "like", "name": "Like" }
                    // { "value": ">", "name": "Greater than" },
                    //{ "value": "<", "name": "Less than" }
                );
            }
            else if (field == "IVRMMS_Name") {

                $scope.result[index].operator.splice(0, 5);
                $scope.result[index].operator.push(
                    { "value": "=", "name": "Equal to" },
                    { "value": "!=", "name": "Not Equal to" },
                    { "value": "like", "name": "Like" }
                    // { "value": ">", "name": "Greater than" },
                    //{ "value": "<", "name": "Less than" }
                );
            }

            else if (field == "ALMST_PerCity") {
                $scope.result[index].operator.splice(0, 5);
                $scope.result[index].operator.push(
                    { "value": "=", "name": "Equal to" },
                    { "value": "!=", "name": "Not Equal to" },
                    { "value": "like", "name": "Like" }
                );
            }
            else if (field == "ALSPR_Designation") {
                //swal("Please Enter Date in 'yyyy-mm-dd' format");
                $scope.result[index].operator.splice(0, 5);
                $scope.result[index].operator.push(
                    { "value": "=", "name": "Equal to" },
                    { "value": "!=", "name": "Not Equal to" },
                    { "value": "like", "name": "Like" }

                );

            }
            else if (field == "ALMST_BloodGroup") {
                //swal("Please Enter Date in 'yyyy-mm-dd' format");
                $scope.result[index].operator.splice(0, 5);
                $scope.result[index].operator.push(
                    { "value": "=", "name": "Equal to" },
                    { "value": "!=", "name": "Not Equal to" },
                    { "value": "like", "name": "Like" }

                );

            }


            else if (field == "ALMST_FullAddess") {
                //swal("Please Enter Date in 'yyyy-mm-dd' format");
                $scope.result[index].operator.splice(0, 5);
                $scope.result[index].operator.push(
                    { "value": "=", "name": "Equal to" },
                    { "value": "!=", "name": "Not Equal to" },
                    { "value": "like", "name": "Like" }

                );

            }

            else if (field == "ASMCL_Id_Join") {
                //swal("Please Enter Date in 'yyyy-mm-dd' format");
                $scope.result[index].operator.splice(0, 5);
                $scope.result[index].operator.push(
                    { "value": "=", "name": "Equal to" },
                    { "value": "!=", "name": "Not Equal to" },
                    //{ "value": "like", "name": "Like" }

                );

            }

            else if (field == "ASMCL_Id_Join") {
                //swal("Please Enter Date in 'yyyy-mm-dd' format");
                $scope.result[index].operator.splice(0, 5);
                $scope.result[index].operator.push(
                    { "value": "=", "name": "Equal to" },
                    { "value": "!=", "name": "Not Equal to" },
                    //{ "value": "like", "name": "Like" }

                );

            }

            else if (field == "ASMCL_Id_Left") {
                //swal("Please Enter Date in 'yyyy-mm-dd' format");
                $scope.result[index].operator.splice(0, 5);
                $scope.result[index].operator.push(
                    { "value": "=", "name": "Equal to" },
                    { "value": "!=", "name": "Not Equal to" },
                    //{ "value": "like", "name": "Like" }

                );

            }
            else if (field == "ASMAY_Id_Join") {
                //swal("Please Enter Date in 'yyyy-mm-dd' format");
                $scope.result[index].operator.splice(0, 5);
                $scope.result[index].operator.push(
                    { "value": "=", "name": "Equal to" },
                    { "value": "!=", "name": "Not Equal to" },
                    //{ "value": "like", "name": "Like" }

                );

            }

            else if (field == "ASMAY_Id_Left") {
                //swal("Please Enter Date in 'yyyy-mm-dd' format");
                $scope.result[index].operator.splice(0, 5);
                $scope.result[index].operator.push(
                    { "value": "=", "name": "Equal to" },
                    { "value": "!=", "name": "Not Equal to" },
                    //{ "value": "like", "name": "Like" }

                );

            }
            else if (field == "ALMST_MembershipId") {
                //swal("Please Enter Date in 'yyyy-mm-dd' format");
                $scope.result[index].operator.splice(0, 5);
                $scope.result[index].operator.push(
                    { "value": "=", "name": "Equal to" },
                    { "value": "!=", "name": "Not Equal to" },
                    { "value": "like", "name": "Like" }

                );

            }
            else if (field == "ALMMC_MembershipCategory") {
                //swal("Please Enter Date in 'yyyy-mm-dd' format");
                $scope.result[index].operator.splice(0, 5);
                $scope.result[index].operator.push(
                    { "value": "=", "name": "Equal to" },
                    { "value": "!=", "name": "Not Equal to" },
                    { "value": "like", "name": "Like" }

                );

            }



            if ($scope.items.val != '' && $scope.items.val != null) {
                $scope.items.val[index] = '';
            }
        }

        $scope.IsHidden = true;
        $scope.ShowHide = function () {
            //If DIV is hidden it will be visible and vice versa.
            $scope.IsHidden = $scope.IsHidden ? false : true;
        }
        $scope.IsHidden1 = true;
        $scope.ShowHide1 = function () {
            //If DIV is hidden it will be visible and vice versa.
            $scope.IsHidden1 = $scope.IsHidden1 ? false : true;
        }
        $scope.IsHidden2 = true;
        $scope.ShowHide2 = function () {
            //If DIV is hidden it will be visible and vice versa.
            $scope.IsHidden2 = $scope.IsHidden2 ? false : true;
        }
        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };
        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed   
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }
        $scope.searchStud = function (inputs) {

            $scope.currentPage = 1;
            $scope.itemsPerPage = 10;
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var output = [];
                for (var key in inputs.field) {
                    // must create a temp object to set the key using a variable
                    var tempObj = {};
                    tempObj[key] = inputs.field[key];
                    output.push(tempObj);
                }

                var output1 = [];
                for (var key in inputs.oprtr) {
                    // must create a temp object to set the key using a variable
                    var tempObj = {};
                    tempObj[key] = inputs.oprtr[key];
                    output1.push(tempObj);
                }

                var output2 = [];
                for (var key in inputs.val) {
                    // must create a temp object to set the key using a variable
                    var tempObj = {};
                    tempObj[key] = inputs.val[key];
                    output2.push(tempObj);
                }

                var output3 = [];
                for (var key in inputs.conditn) {
                    // must create a temp object to set the key using a variable
                    var tempObj = {};
                    tempObj[key] = inputs.conditn[key];
                    output3.push(tempObj);
                }

                var data = {
                    output,
                    output1,
                    output2,
                    output3
                };

                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }
                apiService.create("AlumniSearch/", data).
                    then(function (promise) {
                        if (promise.count == 0) {
                            swal("No Records Found.....!!");
                            $scope.grid_flag = false;
                            $state.reload();
                        }
                        else {
                            $scope.searchResult = promise.searchResult;
                            $scope.presentCountgrid = $scope.searchResult.length;
                            $scope.grid_flag = true;
                        }
                    })
            };
        }

        $scope.Clearid = function () {
            $state.reload();
            // $scope.radioValue = "";
            // $scope.items = "";
        }
        $scope.printstudents = [];
        $scope.toggleAll = function () {
            var toggleStatus = $scope.all;
            //angular.forEach($scope.searchResult, function (itm) {
            //    itm.selected = toggleStatus;
            //    if ($scope.all == true) {
            //        if ($scope.printstudents.indexOf(itm) === -1) {
            //            $scope.printstudents.push(itm);
            //        }
            //    }
            //    else {
            //        $scope.printstudents.splice(itm);
            //    }
            //});



            if ($scope.searchResult != null && $scope.searchResult.length > 0) {

                angular.forEach($scope.searchResult, function (t3) {
                    t3.selected = toggleStatus;
                    if (t3.selected == true) {
                        var al_cnt = 0;
                        //angular.forEach($scope.printstudents, function (rt) {
                        //    if ((rt.almst_id == t3.almst_id && rt.ALMST_FullAddess == t3.ALMST_FullAddess) || (rt.amsT_FirstName == t3.amsT_FirstName && rt.ALMST_FullAddess == t3.ALMST_FullAddess)) {
                        //        al_cnt += 1;
                        //    }
                        //});
                        if ($scope.email == true) {

                            //$scope.printstudents.push(t3);


                            if ($scope.email == true || $scope.ph == true) {
                                if ($scope.email == true && $scope.ph == true) {
                                    $scope.printstudents.push({
                                        almst_id: t3.almst_id,
                                        ASMAY_Id_NEW_Left: t3.ASMAY_Id_NEW_Left,
                                        ASMAY_Id_Join: t3.ASMAY_Id_Join,
                                        ASMAY_Id_Left: t3.ASMAY_Id_Left, ALMST_MembershipId: t3.ALMST_MembershipId, ALMMC_MembershipCategory: t3.ALMMC_MembershipCategory, amsT_FirstName: t3.amsT_FirstName,

                                        ALMST_FullAddess: t3.ALMST_FullAddess, amsT_emailId: t3.amsT_emailId, amsT_MobileNo: t3.amsT_MobileNo, ALMST_PerPincode: t3.ALMST_PerPincode

                                        ALMST_FullAddess: t3.ALMST_FullAddess, amsT_emailId: t3.amsT_emailId, amsT_MobileNo: t3.amsT_MobileNo, amsT_BloodGroup: t3.amsT_BloodGroup, IVRMMS_Name: t3.IVRMMS_Name, ALMST_ConCity: t3.ALMST_ConCity

                                    });
                                }
                                else if ($scope.email == true && $scope.ph == false) {
                                    $scope.printstudents.push({
                                        almst_id: t3.almst_id,
                                        ASMAY_Id_NEW_Left: t3.ASMAY_Id_NEW_Left,
                                        ASMAY_Id_Join: t3.ASMAY_Id_Join,

                                        ASMAY_Id_Left: t3.ASMAY_Id_Left, ALMST_MembershipId: t3.ALMST_MembershipId, ALMMC_MembershipCategory: t3.ALMMC_MembershipCategory, amsT_FirstName: t3.amsT_FirstName,
                                        ALMST_FullAddess: t3.ALMST_FullAddess, amsT_emailId: t3.amsT_emailId, ALMST_PerPincode: t3.ALMST_PerPincode

                                        ASMAY_Id_Left: t3.ASMAY_Id_Left,
                                        ALMST_MembershipId: t3.ALMST_MembershipId, ALMMC_MembershipCategory: t3.ALMMC_MembershipCategory, amsT_FirstName: t3.amsT_FirstName,
                                        ALMST_FullAddess: t3.ALMST_FullAddess, amsT_emailId: t3.amsT_emailId

                                    });
                                }
                                else if ($scope.email == false && $scope.ph == true) {
                                    $scope.printstudents.push({
                                        almst_id: t3.almst_id,
                                        ASMAY_Id_NEW_Left: t3.ASMAY_Id_NEW_Left,
                                        ASMAY_Id_Join: t3.ASMAY_Id_Join,
                                        ASMAY_Id_Left: t3.ASMAY_Id_Left, ALMST_MembershipId: t3.ALMST_MembershipId, ALMMC_MembershipCategory: t3.ALMMC_MembershipCategory, amsT_FirstName: t3.amsT_FirstName,
                                        ALMST_FullAddess: t3.ALMST_FullAddess, amsT_MobileNo: t3.amsT_MobileNo, ALMST_PerPincode: t3.ALMST_PerPincode
                                    });
                                }
                            }
                            else {
                                if ($scope.email == false && $scope.ph == false) {
                                    $scope.printstudents.push({
                                        ASMCL_Id_Join: t3.ASMCL_Id_Join,
                                        Leftclass: t3.Leftclass,
                                        almst_id: t3.almst_id,
                                        ASMAY_Id_NEW_Left: t3.ASMAY_Id_NEW_Left,
                                        ASMAY_Id_Join: t3.ASMAY_Id_Join,
                                        ASMAY_Id_Left: t3.ASMAY_Id_Left, ALMST_MembershipId: t3.ALMST_MembershipId, ALMMC_MembershipCategory: t3.ALMMC_MembershipCategory, amsT_FirstName: t3.amsT_FirstName,
                                        ALMST_FullAddess: t3.ALMST_FullAddess,
                                        ALMST_PerPincode: t3.ALMST_PerPincode
                                    });
                                }
                            }

                        }
                    }
                });


            }
        }
        //$scope.exportToExcel = function (table1) {
        //    if ($scope.printstudents !== null && $scope.printstudents.length > 0) {
        //        var exportHref = Excel.tableToExcel(table1, 'sheet name');
        //        $timeout(function () { location.href = exportHref; }, 100);
        //        //$state.reload();
        //    }
        //    else {
        //        swal("Please select records to be exported");
        //    }

        //}

        $scope.exportToExcel = function (table1) {
            if ($scope.printstudents !== null && $scope.printstudents.length > 0) {
                var excelnamemain = "SCHOLARSHIP ATTENDENCE APPROVAL REPORT";
                var printSectionId = table1;
                excelnamemain = excelnamemain + '-' + $scope.from_dateex + ' To ' + $scope.to_dateex + '.xls';
                var exportHref = Excel.tableToExcel(printSectionId, 'sheet name');
                $timeout(function () {
                    var a = document.createElement('a');
                    a.href = exportHref;
                    a.download = excelnamemain;
                    document.body.appendChild(a);
                    a.click();
                    a.remove();
                }, 100);

            }
            else {
                swal("Please select records to be exported");
            }
        };

        $scope.optionToggled = function (SelectedStudentRecord, index) {
            $scope.all = $scope.searchResult.every(function (itm) { return itm.selected; });
            if ($scope.printstudents.indexOf(SelectedStudentRecord) === -1) {
                //$scope.printstudents.push(SelectedStudentRecord);





                if ($scope.email == true || $scope.ph == true) {
                    if ($scope.email == true && $scope.ph == true) {
                        $scope.printstudents.push({
                            ASMCL_Id_Join: SelectedStudentRecord.ASMCL_Id_Join,
                            Leftclass: SelectedStudentRecord.Leftclass,
                            almst_id: SelectedStudentRecord.almst_id,
                            ASMAY_Id_NEW_Left: SelectedStudentRecord.ASMAY_Id_NEW_Left,
                            ASMAY_Id_Join: SelectedStudentRecord.ASMAY_Id_Join,
                            ASMAY_Id_Left: SelectedStudentRecord.ASMAY_Id_Left, ALMST_MembershipId: SelectedStudentRecord.ALMST_MembershipId, ALMMC_MembershipCategory: SelectedStudentRecord.ALMMC_MembershipCategory, amsT_FirstName: SelectedStudentRecord.amsT_FirstName,
                            ALMST_FullAddess: SelectedStudentRecord.ALMST_FullAddess, amsT_emailId: SelectedStudentRecord.amsT_emailId, amsT_MobileNo: SelectedStudentRecord.amsT_MobileNo, ALMST_PerPincode: SelectedStudentRecord.ALMST_PerPincode
                        });
                    }
                    else if ($scope.email == true && $scope.ph == false) {
                        $scope.printstudents.push({
                            ASMCL_Id_Join: SelectedStudentRecord.ASMCL_Id_Join,
                            Leftclass: SelectedStudentRecord.Leftclass,
                            almst_id: SelectedStudentRecord.almst_id,
                            ASMAY_Id_NEW_Left: SelectedStudentRecord.ASMAY_Id_NEW_Left,
                            ASMAY_Id_Join: SelectedStudentRecord.ASMAY_Id_Join,
                            ASMAY_Id_Left: SelectedStudentRecord.ASMAY_Id_Left, ALMST_MembershipId: SelectedStudentRecord.ALMST_MembershipId, ALMMC_MembershipCategory: SelectedStudentRecord.ALMMC_MembershipCategory, amsT_FirstName: SelectedStudentRecord.amsT_FirstName,
                            ALMST_FullAddess: SelectedStudentRecord.ALMST_FullAddess, amsT_emailId: SelectedStudentRecord.amsT_emailId, ALMST_PerPincode: SelectedStudentRecord.ALMST_PerPincode
                        });
                    }
                    else if ($scope.email == false && $scope.ph == true) {
                        $scope.printstudents.push({
                            ASMCL_Id_Join: SelectedStudentRecord.ASMCL_Id_Join,
                            Leftclass: SelectedStudentRecord.Leftclass,
                            almst_id: SelectedStudentRecord.almst_id,
                            ASMAY_Id_NEW_Left: SelectedStudentRecord.ASMAY_Id_NEW_Left,
                            ASMAY_Id_Join: SelectedStudentRecord.ASMAY_Id_Join,
                            ASMAY_Id_Left: SelectedStudentRecord.ASMAY_Id_Left, ALMST_MembershipId: SelectedStudentRecord.ALMST_MembershipId, ALMMC_MembershipCategory: SelectedStudentRecord.ALMMC_MembershipCategory, amsT_FirstName: SelectedStudentRecord.amsT_FirstName,
                            ALMST_FullAddess: SelectedStudentRecord.ALMST_FullAddess, amsT_MobileNo: SelectedStudentRecord.amsT_MobileNo, ALMST_PerPincode: SelectedStudentRecord.ALMST_PerPincode
                        });
                    }
                }


                else if ($scope.email == false || $scope.ph == false) {
                    if ($scope.email == false && $scope.ph == false) {
                        $scope.printstudents.push({
                            ASMCL_Id_Join: SelectedStudentRecord.ASMCL_Id_Join,
                            Leftclass: SelectedStudentRecord.Leftclass,
                            almst_id: SelectedStudentRecord.almst_id,
                            ASMAY_Id_NEW_Left: SelectedStudentRecord.ASMAY_Id_NEW_Left,
                            ASMAY_Id_Join: SelectedStudentRecord.ASMAY_Id_Join,
                            ASMAY_Id_Left: SelectedStudentRecord.ASMAY_Id_Left, ALMST_MembershipId: SelectedStudentRecord.ALMST_MembershipId, ALMMC_MembershipCategory: SelectedStudentRecord.ALMMC_MembershipCategory, amsT_FirstName: SelectedStudentRecord.amsT_FirstName,
                            ALMST_FullAddess: SelectedStudentRecord.ALMST_FullAddess, ALMST_PerPincode: SelectedStudentRecord.ALMST_PerPincode
                        });
                    }
                    else if ($scope.email == true && $scope.ph == false) {
                        $scope.printstudents.push({
                            ASMCL_Id_Join: SelectedStudentRecord.ASMCL_Id_Join,
                            Leftclass: SelectedStudentRecord.Leftclass,
                            almst_id: SelectedStudentRecord.almst_id,
                            ASMAY_Id_NEW_Left: SelectedStudentRecord.ASMAY_Id_NEW_Left,
                            ASMAY_Id_Join: SelectedStudentRecord.ASMAY_Id_Join,
                            ASMAY_Id_Left: SelectedStudentRecord.ASMAY_Id_Left, ALMST_MembershipId: SelectedStudentRecord.ALMST_MembershipId, ALMMC_MembershipCategory: SelectedStudentRecord.ALMMC_MembershipCategory, amsT_FirstName: SelectedStudentRecord.amsT_FirstName,
                            ALMST_FullAddess: SelectedStudentRecord.ALMST_FullAddess, amsT_emailId: SelectedStudentRecord.amsT_emailId, ALMST_PerPincode: SelectedStudentRecord.ALMST_PerPincode
                        });
                    }
                    else if ($scope.email == false && $scope.ph == true) {
                        $scope.printstudents.push({
                            ASMCL_Id_Join: SelectedStudentRecord.ASMCL_Id_Join,
                            Leftclass: SelectedStudentRecord.Leftclass,
                            almst_id: SelectedStudentRecord.almst_id,
                            ASMAY_Id_NEW_Left: SelectedStudentRecord.ASMAY_Id_NEW_Left,
                            ASMAY_Id_Join: SelectedStudentRecord.ASMAY_Id_Join,
                            ASMAY_Id_Left: SelectedStudentRecord.ASMAY_Id_Left, ALMST_MembershipId: SelectedStudentRecord.ALMST_MembershipId, ALMMC_MembershipCategory: SelectedStudentRecord.ALMMC_MembershipCategory, amsT_FirstName: SelectedStudentRecord.amsT_FirstName,
                            ALMST_FullAddess: SelectedStudentRecord.ALMST_FullAddess, amsT_MobileNo: SelectedStudentRecord.amsT_MobileNo, ALMST_PerPincode: SelectedStudentRecord.ALMST_PerPincode
                        });
                    }
                }
            }
            else {
                // $scope.printstudents.splice($scope.printstudents.indexOf(SelectedStudentRecord), 1);



                if ($scope.searchResult != null && $scope.searchResult.length > 0) {

                    angular.forEach($scope.searchResult, function (t3) {
                        if (t3.selected == true) {
                            var al_cnt = 0;
                            angular.forEach($scope.printstudents, function (rt) {
                                if (((rt.almst_id == t3.almst_id && rt.ALMST_FullAddess == t3.ALMST_FullAddess) || (rt.amsT_FirstName == t3.amsT_FirstName && rt.ALMST_FullAddess == t3.ALMST_FullAddess) || (rt.almst_id == t3.almst_id && rt.amsT_FirstName == t3.amsT_FirstName)) && (rt.ASMAY_Id_Left != rt3.ASMAY_Id_Left)) {
                                    al_cnt += 1;
                                }
                                else if ((rt.amsT_FirstName == t3.amsT_FirstName) && (rt.ASMAY_Id_Left != rt3.ASMAY_Id_Left) && (rt.ASMAY_Id_Join != rt3.ASMAY_Id_Join)) {
                                    al_cnt += 1;
                                }
                            });
                            if (al_cnt === 0) {

                                //$scope.printstudents.push(SelectedStudentRecord);


                                if ($scope.email == true || $scope.ph == true) {
                                    if ($scope.email == true && $scope.ph == true) {
                                        $scope.printstudents.push({
                                            almst_id: SelectedStudentRecord.almst_id,
                                            ASMAY_Id_NEW_Left: SelectedStudentRecord.ASMAY_Id_NEW_Left,
                                            ASMAY_Id_Join: SelectedStudentRecord.ASMAY_Id_Join,
                                            ASMAY_Id_Left: SelectedStudentRecord.ASMAY_Id_Left, ALMST_MembershipId: SelectedStudentRecord.ALMST_MembershipId, ALMMC_MembershipCategory: SelectedStudentRecord.ALMMC_MembershipCategory, amsT_FirstName: SelectedStudentRecord.amsT_FirstName,
                                            ALMST_FullAddess: SelectedStudentRecord.ALMST_FullAddess, amsT_emailId: SelectedStudentRecord.amsT_emailId, amsT_MobileNo: SelectedStudentRecord.amsT_MobileNo, ALMST_PerPincode: SelectedStudentRecord.ALMST_PerPincode
                                        });
                                    }
                                    else if ($scope.email == true && $scope.ph == false) {
                                        $scope.printstudents.push({
                                            almst_id: SelectedStudentRecord.almst_id,
                                            ASMAY_Id_NEW_Left: SelectedStudentRecord.ASMAY_Id_NEW_Left,
                                            ASMAY_Id_Join: SelectedStudentRecord.ASMAY_Id_Join,
                                            ASMAY_Id_Left: SelectedStudentRecord.ASMAY_Id_Left, ALMST_MembershipId: SelectedStudentRecord.ALMST_MembershipId, ALMMC_MembershipCategory: SelectedStudentRecord.ALMMC_MembershipCategory, amsT_FirstName: SelectedStudentRecord.amsT_FirstName,
                                            ALMST_FullAddess: SelectedStudentRecord.ALMST_FullAddess, amsT_emailId: SelectedStudentRecord.amsT_emailId, ALMST_PerPincode: SelectedStudentRecord.ALMST_PerPincode
                                        });
                                    }
                                    else if ($scope.email == false && $scope.ph == true) {
                                        $scope.printstudents.push({
                                            almst_id: SelectedStudentRecord.almst_id,
                                            ASMAY_Id_NEW_Left: SelectedStudentRecord.ASMAY_Id_NEW_Left,
                                            ASMAY_Id_Join: SelectedStudentRecord.ASMAY_Id_Join,
                                            ASMAY_Id_Left: SelectedStudentRecord.ASMAY_Id_Left, ALMST_MembershipId: SelectedStudentRecord.ALMST_MembershipId, ALMMC_MembershipCategory: SelectedStudentRecord.ALMMC_MembershipCategory, amsT_FirstName: SelectedStudentRecord.amsT_FirstName,
                                            ALMST_FullAddess: SelectedStudentRecord.ALMST_FullAddess, amsT_MobileNo: SelectedStudentRecord.amsT_MobileNo, ALMST_PerPincode: SelectedStudentRecord.ALMST_PerPincode
                                        });
                                    }
                                }

                                else if ($scope.email == false || $scope.ph == false) {
                                    if ($scope.email == false && $scope.ph == false) {
                                        $scope.printstudents.push({
                                            ASMCL_Id_Join: SelectedStudentRecord.ASMCL_Id_Join,
                                            Leftclass: SelectedStudentRecord.Leftclass,
                                            almst_id: SelectedStudentRecord.almst_id,
                                            ASMAY_Id_NEW_Left: SelectedStudentRecord.ASMAY_Id_NEW_Left,
                                            ASMAY_Id_Join: SelectedStudentRecord.ASMAY_Id_Join,
                                            ASMAY_Id_Left: SelectedStudentRecord.ASMAY_Id_Left, ALMST_MembershipId: SelectedStudentRecord.ALMST_MembershipId, ALMMC_MembershipCategory: SelectedStudentRecord.ALMMC_MembershipCategory, amsT_FirstName: SelectedStudentRecord.amsT_FirstName,
                                            ALMST_FullAddess: SelectedStudentRecord.ALMST_FullAddess, ALMST_PerPincode: SelectedStudentRecord.ALMST_PerPincode
                                        });
                                    }
                                    else if ($scope.email == true && $scope.ph == false) {
                                        $scope.printstudents.push({
                                            ASMCL_Id_Join: SelectedStudentRecord.ASMCL_Id_Join,
                                            Leftclass: SelectedStudentRecord.Leftclass,
                                            almst_id: SelectedStudentRecord.almst_id,
                                            ASMAY_Id_NEW_Left: SelectedStudentRecord.ASMAY_Id_NEW_Left,
                                            ASMAY_Id_Join: SelectedStudentRecord.ASMAY_Id_Join,
                                            ASMAY_Id_Left: SelectedStudentRecord.ASMAY_Id_Left, ALMST_MembershipId: SelectedStudentRecord.ALMST_MembershipId, ALMMC_MembershipCategory: SelectedStudentRecord.ALMMC_MembershipCategory, amsT_FirstName: SelectedStudentRecord.amsT_FirstName,
                                            ALMST_FullAddess: SelectedStudentRecord.ALMST_FullAddess, amsT_emailId: SelectedStudentRecord.amsT_emailId, ALMST_PerPincode: SelectedStudentRecord.ALMST_PerPincode
                                        });
                                    }
                                    else if ($scope.email == false && $scope.ph == true) {
                                        $scope.printstudents.push({
                                            ASMCL_Id_Join: SelectedStudentRecord.ASMCL_Id_Join,
                                            Leftclass: SelectedStudentRecord.Leftclass,
                                            almst_id: SelectedStudentRecord.almst_id,
                                            ASMAY_Id_NEW_Left: SelectedStudentRecord.ASMAY_Id_NEW_Left,
                                            ASMAY_Id_Join: SelectedStudentRecord.ASMAY_Id_Join,
                                            ASMAY_Id_Left: SelectedStudentRecord.ASMAY_Id_Left, ALMST_MembershipId: SelectedStudentRecord.ALMST_MembershipId, ALMMC_MembershipCategory: SelectedStudentRecord.ALMMC_MembershipCategory, amsT_FirstName: SelectedStudentRecord.amsT_FirstName,
                                            ALMST_FullAddess: SelectedStudentRecord.ALMST_FullAddess, amsT_MobileNo: SelectedStudentRecord.amsT_MobileNo, ALMST_PerPincode: SelectedStudentRecord.ALMST_PerPincode
                                        });
                                    }
                                }

                            }
                        }
                    });

                }
            }

        }
        $scope.printDataadd = function () {
            if ($scope.printstudents !== null && $scope.printstudents.length > 0) {
                var innerContents = document.getElementById("SRKVSStudentAddressBook").innerHTML;
                var popupWinindow = window.open('');
                popupWinindow.document.open();
                popupWinindow.document.write('<html><head>' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
                    '<link type="text/css" media="print" rel="stylesheet"  href="css/print/SRKVSStudentAddressBook/SRKVSStudentAddressBookPdf.css" />' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                    '</head><body onload="window.print()" onfocus="window.setTimeout(function() { window.close(); }, 300);">' + innerContents + '</html>');
                popupWinindow.document.close();
            }
            else {
                swal("Select atleast one record !")
            }
        }


        $scope.printData = function () {
            if ($scope.printstudents !== null && $scope.printstudents.length > 0) {

                var innerContents = document.getElementById("printSectionId").innerHTML;
                var popupWinindow = window.open('');
                popupWinindow.document.open();
                popupWinindow.document.write('<html><head>' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
                    '<link type="text/css" media="print" href="css/print/preadmission/RegReportPdf.css" rel="stylesheet" />' +
                    '</head> <body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>'
                );
                popupWinindow.document.close();
            }
            else {
                swal("Please Select Records to be Printed");
            }
        }


        $("#btnExport").click(function (e) {
            window.open('data:application/vnd.ms-excel,' + $('#Table').html());
            e.preventDefault();
        });
    }
})
    ();